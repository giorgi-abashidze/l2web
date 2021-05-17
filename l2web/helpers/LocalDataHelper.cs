using l2web.Data;
using l2web.Data.DataModels;
using l2web.helpers.contracts;
using l2web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace l2web.helpers
{
    public class LocalDataHelper : ILocalDataHelper
    {
        private readonly IConfiguration Configuration;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private int StatsDataUpdateLimit { get; set; }
        private int OnlineUpdateLimit { get; set; }
        private int CharactersUpdateLimit { get; set; }

        public LocalDataHelper(IConfiguration configuration, ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            Configuration = configuration;
            _db = db;
            _userManager = userManager;
            StatsDataUpdateLimit = Int32.Parse(Configuration.GetValue<string>("stats_data_update_time"));
            OnlineUpdateLimit = Int32.Parse(Configuration.GetValue<string>("online_update_time"));
            CharactersUpdateLimit = Int32.Parse(Configuration.GetValue<string>("characters_info_update_time"));
        }

        //if true we can fetch new data (limit time is out)
        public async Task<bool> CheckStatsUpdateLimit()
        {
            var lastUpdate = await _db.DataUpdates.ToListAsync();
            return HelperFunctions.GetDiffInMinutes(lastUpdate.First().LastDataUpdate, DateTime.Now) >= StatsDataUpdateLimit;
        }

        //if true we can fetch new data (limit time is out)
        public async Task<bool> CheckOnlineUpdateLimit()
        {
            var lastUpdate = await _db.DataUpdates.ToListAsync();
            return HelperFunctions.GetDiffInMinutes(lastUpdate.First().LastOnlineUpdate, DateTime.Now) >= OnlineUpdateLimit;
        }

        //if true we can fetch new data (limit time is out)
        public bool CheckCharactersUpdateLimit(ApplicationUser user)
        {
            return HelperFunctions.GetDiffInMinutes(user.LastAccountUpdateTime, DateTime.Now) >= CharactersUpdateLimit;
        }

        public async Task<int> GetLastOnline() {
            var online = await _db.OnlineCache.OrderByDescending(i => i.TDate).FirstOrDefaultAsync();
            return online == null ? 0 : online.Online;
        }

        public List<Character> GetAccountInfo(ApplicationUser user) {


            List<Character> charList = new List<Character>();
            var acc = user.Account;
            foreach (var ch in user.Account.Characters) {
                var character = new Character();
                character.Lvl = ch.Lvl;
                character.Name = ch.Name;
                character.OcupationIndex = ch.OcupationIndex;
                character.RaceIndex = ch.RaceIndex;

                charList.Add(character);
            }

            return charList;
        }

        public List<EpicJewelOwner> GetEpicOwners(){

            var result = new List<EpicJewelOwner>();

            foreach (var o in _db.EpicOwnersCache) {
                var tmpOwner = new EpicJewelOwner(o.CharName,o.ItemId,o.Amount);
                result.Add(tmpOwner);
            }

            return result;

        }


        public List<Castle> GetCastleInfo()
        {

            var result = new List<Castle>();

            foreach (var c in _db.CastleCache)
            {
                var tmpCastle = new Castle();
                tmpCastle.CastleName = c.CastleName;
                tmpCastle.Owner = c.Owner;
               
                result.Add(tmpCastle);
            }

            return result;

        }


        public List<Clan> GetTopClans() {
            var result = new List<Clan>();

            foreach (var c in _db.ClanCache)
            {
                var tmpClan = new Clan();
                tmpClan.ClanLevel = c.ClanLevel;
                tmpClan.ClanName = c.ClanName;
                tmpClan.LeaderName = c.LeaderName;
                tmpClan.Icon = c.Icon;

                result.Add(tmpClan);
            }

            return result;

        }



    }
}
