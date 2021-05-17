using l2web.Data.DataModels;
using l2web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace l2web.helpers.contracts
{
    public interface ILocalDataHelper
    {
        Task<bool> CheckStatsUpdateLimit();
        Task<bool> CheckOnlineUpdateLimit();
        bool CheckCharactersUpdateLimit(ApplicationUser user);
        Task<int> GetLastOnline();
        List<Character> GetAccountInfo(ApplicationUser user);
        List<EpicJewelOwner> GetEpicOwners();
        List<Castle> GetCastleInfo();
        List<Clan> GetTopClans();
    }
}
