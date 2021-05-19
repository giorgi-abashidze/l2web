using l2web.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace l2web.helpers.contracts
{
    public interface IQueryHelper
    {
        Task<bool> CheckAccount(string login);
        Task<bool> CheckEmail(string email);
        Task<bool> InsertSSN(string ssn, string name, string email, string phone = null);
        Task<bool> InsertUserAccount(string account, string email);
        Task<bool> InsertUserAuth(string account, string password, string md5password = null, string quiz1 = null, string quiz2 = null, string answer1 = null, string answer2 = null);
        Task<bool> InsertUserInfo(string account, string ssn);
        Task<bool> UpdatePay(string account);
        Task<bool> GetCharactersInfo(ApplicationUser user);
        Task<bool> GetOnlineCount();
        Task<bool> GetEpicOwners();
        Task<bool> GetTopClans();
        Task<bool> GetCastleInfo();
        Task<bool> ResetPaassword(string account, string password, string md5password = null);
    }
}
