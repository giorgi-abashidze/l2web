using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using l2web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using l2web.helpers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;
using Edi.Captcha;

namespace l2web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IQueryHelper _queryHelper;
        private readonly IConfiguration _config;
        private readonly ISessionBasedCaptcha _captcha;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IQueryHelper queryHelper,
            IConfiguration config,
            ISessionBasedCaptcha captcha)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _queryHelper = queryHelper;
            _config = config;
            _captcha = captcha;
        }

        public IActionResult GetCaptchaImage()
        {
            var s = _captcha.GenerateCaptchaImageFileStream(
                HttpContext.Session,
                100,
                36
                );
            return s;
        }

        //use md5password for l2auth
        private bool usemd5Password = true;

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
            [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
            [Display(Name = "Prefix")]
            public string Prefix { get; set; }

            [Required]
            [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
            [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
            [Display(Name = "Login")]
            public string Login { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [StringLength(4)]
            public string CaptchaCode { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "UserAccount");

            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                if (!_captcha.Validate(Input.CaptchaCode, HttpContext.Session))
                {
                    ModelState.AddModelError("Input.CaptchaCode", "Captcha code is not correct.");
                    return Page();
                }

                if (!await _queryHelper.CheckEmail(Input.Email)) {
                    ModelState.AddModelError("Input.Email", "Account with this Email already exists.");
                   
                    return Page();
                }
                    

                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email,Login = Input.Prefix.ToLower()+Input.Login.ToLower() };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    
                    string ssn = HelperFunctions.GenerateSSN();
                    string key = String.Empty;

                    if (usemd5Password) { 
                        key = _config.GetSection("md5password").GetSection("Key").Value;
                    }
                    
                    

                    try
                    {
                        //await _queryHelper.InsertSSN(ssn, Input.Prefix.ToLower() + Input.Login.ToLower(), Input.Email);
                        await _queryHelper.InsertUserAccount(Input.Prefix.ToLower() + Input.Login.ToLower(), Input.Email);
                        await _queryHelper.InsertUserInfo(Input.Prefix.ToLower() + Input.Login.ToLower(), ssn);

                        //don't pass md5password: if you are not using md5 password in l2auth
                        await _queryHelper.InsertUserAuth(Input.Prefix.ToLower() + Input.Login.ToLower(),  Input.Password, md5password: HelperFunctions.hCrypt(Input.Password, key));

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    }
                    catch (SqlException e) {
                        Console.WriteLine("Errooooor: "+e.Message);
                    }


                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl, login = Input.Prefix.ToLower() + Input.Login.ToLower(), password = Input.Password });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
