using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using l2web.Data.DataModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace l2web.Views.Admin
{
    public class IndexModel : PageModel
    {
        
        public IndexModel()
        {
            

        }


        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {


            return Page();
        }

        
    }
}
