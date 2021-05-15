using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace l2web.Views.Home
{
    public class IndexModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {


            return Page();
        }
    }
}
