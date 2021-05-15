using l2web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace l2web.helpers
{


    public interface IQueryHelper {
        Task<bool> CheckAccount(string login);
        Task<bool> CheckEmail(string email);
        Task<int> GetAccountsCount();
        Task<bool> InsertSSN(string ssn, string name, string email, string phone = null);
        Task<bool> InsertUserAccount(string account, string email);
        Task<bool> InsertUserAuth(string account, string password, string md5password = null, string quiz1 = null, string quiz2 = null, string answer1 = null, string answer2 = null);
        Task<bool> InsertUserInfo(string account, string ssn);
        Task<bool> UpdatePay(string account);
        Task<List<Character>> GetCharactersInfo(string account);
        Task<int> GetOnlineCount();
        Task<List<EpicJewelOwner>> GetEpicOwners();
        Task<List<Clan>> GetTopClans();
        Task<List<Castle>> GetCastleInfo();
    }
    public class QueryHelper : IQueryHelper
    {

        private readonly IConfiguration Configuration;
        string connectionStr;
        string worldConnectionStr;
    

        public QueryHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            connectionStr = Configuration.GetConnectionString("L2Connection");
            worldConnectionStr = Configuration.GetConnectionString("L2WorldConnection");
            
        }

        public async Task<bool> CheckAccount(string login) {
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

        public async Task<int> GetAccountsCount()
        {
            string query = "SELECT count(*) FROM user_account";

            using (SqlConnection connection = new SqlConnection(
               connectionStr))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                return (int)await command.ExecuteScalarAsync();
            }
        }

        public async Task<bool> InsertSSN(string ssn,string name,string email,string phone = null)
        {
            string query = "INSERT INTO [ssn](ssn,name,email,job,phone,zip,addr_main,addr_etc,account_num)"+
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
            string query = "INSERT INTO user_account (account,pay_stat,email)"+
                            $"VALUES('{account}', 0, '{email}')";

            using (SqlConnection connection = new SqlConnection(
               connectionStr))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                return await command.ExecuteNonQueryAsync() == 1;
            }
        }

        public async Task<bool> InsertUserAuth(string account, string password,string md5password = null, string quiz1 = null,string quiz2 = null,string answer1 = null,string answer2 = null)
        {
            string query;

            byte[] passwordBytes = HelperFunctions.StrToByteArray(password);

            if (md5password.Length > 0)
            {
                HelperFunctions.Randomize(passwordBytes);

                 query = "INSERT INTO user_auth (account,password,md5password,quiz1,quiz2,answer1,answer2)" +
                           $"VALUES('{account}',@BIN,'{md5password.ToLower()}', '{quiz1}', '{quiz2}',@BIN2,@BIN3)";
            }
            else {
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

        public async Task<List<Character>> GetCharactersInfo(string account)
        {
            List<Character> characters = new List<Character>();

            string query = $"SELECT char_name,race,class,Lev FROM user_data WHERE account_name = '{account}'";

            using (SqlConnection connection = new SqlConnection(
               worldConnectionStr))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                using (IDataReader reader = await command.ExecuteReaderAsync())
                {
                    while(reader.Read())
                    {
                        var ch = new Character();

                        ch.Name = (string)reader["char_name"];
                        ch.Lvl = (byte)reader["Lev"];
                        ch.OcupationIndex = (byte)reader["class"];
                        ch.RaceIndex = (byte)reader["race"];

                        characters.Add(ch);

                    }
                }
            }

            return characters;
            
        }

        public async Task<int> GetOnlineCount()
        {

            string query = $"SELECT COUNT(*) FROM user_data where login > logout";

            using (SqlConnection connection = new SqlConnection(
               worldConnectionStr))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                return (int)await command.ExecuteScalarAsync();

            }
        }

        public async Task<List<EpicJewelOwner>> GetEpicOwners()
        {

            List<EpicJewelOwner> owners = new List<EpicJewelOwner>();

            string query = "SELECT p.char_name,p.item_type, SUM(p.amount) as amount " +
                "FROM( "+
                "SELECT user_data.char_name,user_item.item_type,user_item.amount "+
                "FROM user_item "+
                "INNER JOIN user_data "+
                "ON user_item.char_id = user_data.char_id "+
                "AND user_item.item_type "+
                "IN(6657, 6658, 6659, 6660, 8191, 90992) "+
                "UNION ALL SELECT user_data.char_name,user_warehouse.item_type,user_warehouse.amount "+
                "FROM user_warehouse "+
                "INNER JOIN user_data "+
                "ON user_warehouse.char_id = user_data.char_id "+
                "AND user_warehouse.item_type "+
                "IN(6657, 6658, 6659, 6660, 8191, 90992) "+
                ")p "+
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
                        var owner = new EpicJewelOwner((string)reader["char_name"], (int)reader["item_type"], (Int64)reader["amount"]);

                        owners.Add(owner);

                    }
                }

            }

            return owners;
        }

        public async Task<List<Clan>> GetTopClans()
        {

            List<Clan> topClans = new List<Clan>();

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
                        var clan = new Clan();
                        clan.ClanName = (string)reader["clan_name"];
                        clan.LeaderName = (string)reader["leader_name"];
                        clan.ClanLevel = (Int16)reader["level"];
                        clan.Icon = (byte[])reader["icon"];
                       

                        topClans.Add(clan);

                    }
                }

            }

            return topClans;
        }

        public async Task<List<Castle>> GetCastleInfo()
        {

            List<Castle> castles = new List<Castle>();

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
                        var castle = new Castle();
                        castle.CastleName = (string)reader["castle"];
                        castle.Owner = Convert.IsDBNull(reader["owner"]) ? "None" : (string)reader["owner"];

                        castles.Add(castle);

                    }
                }

            }

            return castles;
        }





    }

    

}


