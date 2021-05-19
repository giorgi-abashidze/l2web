using l2web.Data;
using l2web.Data.DataModels;
using l2web.helpers.contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace l2web.helpers
{


    public class QueryHelper : IQueryHelper
    {

        private readonly IConfiguration Configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        string connectionStr;
        string worldConnectionStr;
        private readonly ApplicationDbContext _db;

        public QueryHelper(IConfiguration configuration, ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            Configuration = configuration;
            _db = db;
            _userManager = userManager;
            connectionStr = Configuration.GetConnectionString("L2Connection");
            worldConnectionStr = Configuration.GetConnectionString("L2WorldConnection");

        }

        public async Task<bool> CheckAccount(string login)
        {
            string query = $"SELECT account FROM user_account WHERE account = '{login}'";

            using (SqlConnection connection = new SqlConnection(
               connectionStr))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                return await command.ExecuteNonQueryAsync() == 1;
            }
        }

        public async Task<bool> CheckEmail(string email)
        {
            string query = $"SELECT count(*) FROM user_account WHERE email = '{email}'";

            using (SqlConnection connection = new SqlConnection(
               connectionStr))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                var res = (int)await command.ExecuteScalarAsync();
                return res == 0;
            }
        }


        public async Task<bool> InsertSSN(string ssn, string name, string email, string phone = null)
        {
            string query = "INSERT INTO [ssn](ssn,name,email,job,phone,zip,addr_main,addr_etc,account_num)" +
                            $"VALUES('{ssn}', '{name}', '{email}', 0, 'telphone', '123456', '', '', 1)";

            using (SqlConnection connection = new SqlConnection(
               connectionStr))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                return await command.ExecuteNonQueryAsync() == 1;
            }
        }

        public async Task<bool> InsertUserAccount(string account, string email)
        {
            string query = "INSERT INTO user_account (account,pay_stat,email)" +
                            $"VALUES('{account}', 0, '{email}')";

            using (SqlConnection connection = new SqlConnection(
               connectionStr))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                return await command.ExecuteNonQueryAsync() == 1;
            }
        }

        public async Task<bool> InsertUserAuth(string account, string password, string md5password = null, string quiz1 = null, string quiz2 = null, string answer1 = null, string answer2 = null)
        {
            string query;

            byte[] passwordBytes = HelperFunctions.StrToByteArray(password);

            if (md5password.Length > 0)
            {
                HelperFunctions.Randomize(passwordBytes);

                query = "INSERT INTO user_auth (account,password,md5password,quiz1,quiz2,answer1,answer2)" +
                          $"VALUES('{account}',@BIN,'{md5password.ToLower()}', '{quiz1}', '{quiz2}',@BIN2,@BIN3)";
            }
            else
            {
                query = "INSERT INTO user_auth (account,password,quiz1,quiz2,answer1,answer2)" +
                              $"VALUES('{account}',@BIN,'{quiz1}', '{quiz2}',@BIN2,@BIN3)";
            }


            using (SqlConnection connection = new SqlConnection(
               connectionStr))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add("@BIN", SqlDbType.Binary, passwordBytes.Length).Value = passwordBytes;
                command.Parameters.Add("@BIN2", SqlDbType.Binary, passwordBytes.Length).Value = passwordBytes;
                command.Parameters.Add("@BIN3", SqlDbType.Binary, passwordBytes.Length).Value = passwordBytes;
                command.Connection.Open();
                return await command.ExecuteNonQueryAsync() == 1;
            }
        }

        public async Task<bool> InsertUserInfo(string account, string ssn)
        {
            string query = $"INSERT INTO user_info (account,create_date,ssn,status_flag,kind) VALUES('{account}','{DateTime.Now:yyyy-MM-dd hh:mm:ss}','{ssn}',0, 99)";

            using (SqlConnection connection = new SqlConnection(
               connectionStr))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                return await command.ExecuteNonQueryAsync() == 1;
            }
        }

        public async Task<bool> UpdatePay(string account)
        {
            string query = $"UPDATE user_account SET pay_stat = 1 WHERE account = '{account}' ";

            using (SqlConnection connection = new SqlConnection(
               connectionStr))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                return await command.ExecuteNonQueryAsync() == 1;
            }
        }

        public async Task<bool> GetCharactersInfo(ApplicationUser user)
        {


            string query = $"SELECT char_name,race,class,Lev FROM user_data WHERE account_name = '{user.Login}'";

            using (SqlConnection connection = new SqlConnection(
               worldConnectionStr))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                using (IDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var ch = new CharacterCache();

                        ch.Name = (string)reader["char_name"];
                        ch.Lvl = (byte)reader["Lev"];
                        ch.OcupationIndex = (byte)reader["class"];
                        ch.RaceIndex = (byte)reader["race"];
                        ch.AccountId = user.Account.Id;
                        ch.Account = user.Account;

                        var tmpChar = user.Account.Characters.Where(c => c.Name.Equals(ch.Name)).FirstOrDefault();

                        if (tmpChar == null)
                        {
                            user.Account.Characters.Add(ch);
                        }
                        else
                        {
                            _db.Attach(tmpChar);

                            tmpChar.RaceIndex = ch.RaceIndex;
                            tmpChar.OcupationIndex = ch.OcupationIndex;
                            tmpChar.Lvl = ch.Lvl;

                            _db.Entry(tmpChar).Property(p => p.RaceIndex).IsModified = true;
                            _db.Entry(tmpChar).Property(p => p.OcupationIndex).IsModified = true;
                            _db.Entry(tmpChar).Property(p => p.Lvl).IsModified = true;
                        }

                    }


                }
            }

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> GetOnlineCount()
        {

            string query = $"SELECT COUNT(*) FROM user_data where login > logout";

            using (SqlConnection connection = new SqlConnection(
               worldConnectionStr))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                int online = (int)await command.ExecuteScalarAsync();

                var OCache = new OnlineCache();
                OCache.Online = online;
                OCache.TDate = DateTime.Now;

                var count = _db.OnlineCache.Count();

                if (count == 0)
                {
                    _db.OnlineCache.Add(OCache);
                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    var last = await _db.OnlineCache.OrderByDescending(o => o.TDate).FirstAsync();
                    _db.Attach(last);
                    last.Online = OCache.Online;
                    last.TDate = DateTime.Now;
                    _db.Entry(last).Property(e => e.Online).IsModified = true;
                    _db.Entry(last).Property(e => e.TDate).IsModified = true;

                    await _db.SaveChangesAsync();
                    return true;
                }

            }
        }

        public async Task<bool> GetEpicOwners()
        {

            string query = "SELECT p.char_name,p.item_type, SUM(p.amount) as amount " +
                "FROM( " +
                "SELECT user_data.char_name,user_item.item_type,user_item.amount " +
                "FROM user_item " +
                "INNER JOIN user_data " +
                "ON user_item.char_id = user_data.char_id " +
                "AND user_item.item_type " +
                "IN(6657, 6658, 6659, 6660, 8191, 90992) " +
                "UNION ALL SELECT user_data.char_name,user_warehouse.item_type,user_warehouse.amount " +
                "FROM user_warehouse " +
                "INNER JOIN user_data " +
                "ON user_warehouse.char_id = user_data.char_id " +
                "AND user_warehouse.item_type " +
                "IN(6657, 6658, 6659, 6660, 8191, 90992) " +
                ")p " +
                "GROUP BY p.char_name, p.item_type";

            using (SqlConnection connection = new SqlConnection(
               worldConnectionStr))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                using (IDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var owner = new EpicOwnersCache();
                        owner.CharName = (string)reader["char_name"];
                        owner.ItemId = (int)reader["item_type"];
                        owner.Amount = (Int64)reader["amount"];


                        var tmpOwner = _db.EpicOwnersCache.Where(e => e.CharName.Equals(owner.CharName) && e.ItemId == owner.ItemId).FirstOrDefault();

                        //if record don't exists create one
                        if (tmpOwner == null)
                        {
                            _db.EpicOwnersCache.Add(owner);
                        }
                        //if exists record with same charname and itemId just update amount (I know Quantity would be better name :D)
                        else
                        {
                            _db.Attach(tmpOwner);
                            tmpOwner.Amount = owner.Amount;
                            _db.Entry(tmpOwner).Property(p => p.Amount).IsModified = true;
                        }

                    }
                }

            }

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> GetTopClans()
        {
            //clear Top 10 Clans Table and Insert new data instead of update
            //Its only 10 records so it wont be big operation I think..
            await _db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [ClanCache]");

            string query = "SELECT TOP 10 Pledge.name as clan_name "
              + ",user_data.char_name as leader_name "
              + ",skill_level as level "
              + ",Pledge_Crest.bitmap as icon "
              + "FROM Pledge "
              + "INNER JOIN user_data ON Pledge.ruler_id = user_data.char_id "
              + "INNER JOIN Pledge_Crest ON Pledge.crest_id = Pledge_Crest.crest_id "
              + "ORDER BY Pledge.skill_level DESC, siege_kill DESC, castle_siege_defence_count DESC";

            using (SqlConnection connection = new SqlConnection(
               worldConnectionStr))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                using (IDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var clan = new ClanCache();
                        clan.ClanName = (string)reader["clan_name"];
                        clan.LeaderName = (string)reader["leader_name"];
                        clan.ClanLevel = (Int16)reader["level"];
                        clan.Icon = (byte[])reader["icon"];


                        _db.ClanCache.Add(clan);

                    }
                }

            }

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> GetCastleInfo()
        {
            //clear castlecache and Insert new data Instead of update
            //same like top clans
            await _db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [CastleCache]");

            string query = "SELECT castle.name as castle, Pledge.name as owner FROM castle LEFT JOIN Pledge ON castle.pledge_id = Pledge.pledge_id";

            using (SqlConnection connection = new SqlConnection(
               worldConnectionStr))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                using (IDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var castle = new CastleCache();
                        castle.CastleName = (string)reader["castle"];
                        castle.Owner = Convert.IsDBNull(reader["owner"]) ? "None" : (string)reader["owner"];

                        _db.CastleCache.Add(castle);
                    }
                }

            }

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ResetPaassword(string account,string password,string md5password = null)
        {
            string query;

            byte[] passwordBytes = HelperFunctions.StrToByteArray(password);

            if (!string.IsNullOrEmpty(md5password))
            {
                HelperFunctions.Randomize(passwordBytes);

                query = "UPDATE user_auth SET password = @BIN, md5password = " + password.ToLower() + "WHERE account = " + account + ";";
 
            }
            else
            {
                query = "UPDATE user_auth SET password = @BIN WHERE account = " + account + ";";
            }

            using (SqlConnection connection = new SqlConnection(
               connectionStr))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add("@BIN", SqlDbType.Binary, passwordBytes.Length).Value = passwordBytes;
                command.Connection.Open();
                return await command.ExecuteNonQueryAsync() == 1;
            }

        }
    }

    

}


