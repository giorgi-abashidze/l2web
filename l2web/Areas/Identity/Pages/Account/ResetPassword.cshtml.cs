using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using l2web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using l2web.Data.DataModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using l2web.helpers.contracts;
using l2web.helpers;
using Edi.Captcha;

namespace l2web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IQueryHelper _queryHelper;
        private readonly ISessionBasedCaptcha _captcha;

        public ResetPasswordModel(
            UserManager<ApplicationUser> userManager, 
            IConfiguration config,
            IQueryHelper queryHelper,
            ISessionBasedCaptcha captcha)
        {
            _userManager = userManager;
            _config = config;
            _queryHelper = queryHelper;
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

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }

            [Required]
            [StringLength(4)]
            public string CaptchaCode { get; set; }
        }

        public IActionResult OnGet(string code = null)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                Input = new InputModel
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {


            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!_captcha.Validate(Input.CaptchaCode, HttpContext.Session))
            {
                ModelState.AddModelError("Input.CaptchaCode", "Captcha code is not correct.");
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");
            }


            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                string key = _config.GetSection("md5password").GetSection("Key").Value;

                if (string.IsNullOrEmpty(key))
                {
                    try
                    {
                        await _queryHelper.ResetPaassword(user.Login, Input.Password);

                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine("Errooooor: " + e.Message);
                    }
                }
                else {
                    try
                    {
                        await _queryHelper.ResetPaassword(user.Login, Input.Password, md5password: HelperFunctions.hCrypt(Input.Password, key));
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine("Errooooor: " + e.Message);
                    }
                }

                


                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }
}
