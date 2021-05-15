using l2web.helpers;
using l2web.helpers.contracts;
using l2web.Models;
using l2web.Views.UserAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace l2web.Controllers
{
  
    [Authorize]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class UserAccountController : Controller
    {
        private readonly ILogger<UserAccountController> _logger;
        private readonly IQueryHelper _queryHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPaymentHelper _paymentHelper;
        public UserAccountController(ILogger<UserAccountController> logger, IQueryHelper queryHelper, UserManager<ApplicationUser> userManager, IPaymentHelper paymentHelper)
        {
            _logger = logger;
            _queryHelper = queryHelper;
            _userManager = userManager;
            _paymentHelper = paymentHelper;
        }
        // GET: AccountController
        public async Task<ActionResult> Index()
        {
            
            var user = await _userManager.GetUserAsync(HttpContext.User);

            IndexModel model = new IndexModel();
            model.PaymentHelper = _paymentHelper;
            model.Characters = await _queryHelper.GetCharactersInfo(user.Login);
            model.OnlinePlayers = await _queryHelper.GetOnlineCount();
            model.EpicOwners = await _queryHelper.GetEpicOwners();
            model.TopClans = await _queryHelper.GetTopClans();
            model.CastleInfo = await _queryHelper.GetCastleInfo();
            return View(model);
        }

        [HttpPost]
        [Route("UserAccountController/Buy/{coins}")]
        public async Task<IActionResult> Buy(int coins)
        {
            //TODO:implement payment logic
            
            return RedirectToPage("~/UsserAccount");

        }


        [Route("UserAccountController/Epic/{owners}")]
        public PartialViewResult Epic(string owners)
        {
            var deserialized = JsonConvert.DeserializeObject<List<EpicJewelOwner>>(owners);
            return PartialView("_EpicOwnersPartial", deserialized);
        }


        public async Task<IActionResult> Services()
        {
            var model = new UserServicesModel();
            model.OnlinePlayers = await _queryHelper.GetOnlineCount();
            return View("UserServices",model);
        }

        
       
    }
}
