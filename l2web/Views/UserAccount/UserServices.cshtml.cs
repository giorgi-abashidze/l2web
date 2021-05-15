using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace l2web.Views.UserAccount
{
    public class UserServicesModel : PageModel
    {
        public int OnlinePlayers { get; set; }
    }
}
