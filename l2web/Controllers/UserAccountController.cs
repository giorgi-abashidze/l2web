using l2web.Data;
using l2web.Data.DataModels;
using l2web.helpers.contracts;
using l2web.Models;
using l2web.Views.UserAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace l2web.Controllers
{

    [Authorize(Policy = "RequireUserRole")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class UserAccountController : Controller
    {
        private readonly ILogger<UserAccountController> _logger;
        private readonly IQueryHelper _queryHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPaymentHelper _paymentHelper;
        private readonly ILocalDataHelper _localDataHelper;
        private readonly ApplicationDbContext _db;

        public UserAccountController(
            ILogger<UserAccountController> logger, 
            IQueryHelper queryHelper, 
            UserManager<ApplicationUser> userManager, 
            IPaymentHelper paymentHelper, 
            ILocalDataHelper localDataHelper,
            ApplicationDbContext db)
        {
            _logger = logger;
            _queryHelper = queryHelper;
            _userManager = userManager;
            _paymentHelper = paymentHelper;
            _localDataHelper = localDataHelper;
            _db = db;
        }
        // GET: AccountController
        public async Task<ActionResult> Index()
        {
            
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var userWithAccount = await _db.Users.Include(u => u.Account).ThenInclude(a => a.Characters).Where(u => u.Id.Equals(user.Id)).FirstOrDefaultAsync();

            IndexModel model = new IndexModel();
            model.PaymentHelper = _paymentHelper;

            if (_localDataHelper.CheckCharactersUpdateLimit(user)) {
                await _queryHelper.GetCharactersInfo(userWithAccount);
                user.LastAccountUpdateTime = DateTime.Now;
                _db.Attach(user);
                _db.Entry(user).Property(p => p.LastAccountUpdateTime).IsModified = true;
                await _db.SaveChangesAsync();
            }
            if (await _localDataHelper.CheckOnlineUpdateLimit())
            {
                await _queryHelper.GetOnlineCount();
                var updates = await _db.DataUpdates.FirstAsync();
                _db.Attach(updates);
                updates.LastOnlineUpdate = DateTime.Now;
                _db.Entry(updates).Property(p => p.LastOnlineUpdate).IsModified = true;
                await _db.SaveChangesAsync();
            }
            if (await _localDataHelper.CheckStatsUpdateLimit())
            {
                await _queryHelper.GetEpicOwners();
                await _queryHelper.GetTopClans();
                await _queryHelper.GetCastleInfo();

                var updates = await _db.DataUpdates.FirstAsync();
                _db.Attach(updates);
                updates.LastDataUpdate = DateTime.Now;
                _db.Entry(updates).Property(p => p.LastDataUpdate).IsModified = true;
                await _db.SaveChangesAsync();
            }
            model.Characters = _localDataHelper.GetAccountInfo(userWithAccount);
            model.OnlinePlayers = await _localDataHelper.GetLastOnline();
            model.EpicOwners = _localDataHelper.GetEpicOwners();
            model.TopClans = _localDataHelper.GetTopClans();
            model.CastleInfo =  _localDataHelper.GetCastleInfo();
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
            if (await _localDataHelper.CheckOnlineUpdateLimit())
            {
                await _queryHelper.GetOnlineCount();
                var updates = await _db.DataUpdates.FirstAsync();
                _db.Attach(updates);
                updates.LastOnlineUpdate = DateTime.Now;
                _db.Entry(updates).Property(p => p.LastOnlineUpdate).IsModified = true;
                await _db.SaveChangesAsync();
            }

            var model = new UserServicesModel();
            model.OnlinePlayers = await _localDataHelper.GetLastOnline();
            return View("UserServices",model);
        }

        
       
    }
}
