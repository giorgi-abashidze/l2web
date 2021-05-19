using Microsoft.AspNetCore.Identity;
using System;


namespace l2web.Data.DataModels
{
    public class ApplicationUser : IdentityUser
    {
        public string Login { get; set; }
        public int CoinsOwned { get; set; }
        public DateTime LastAccountUpdateTime { get; set; }
        public string AccountId { get; set; }
        public Account Account {get;set;}

    }
}
