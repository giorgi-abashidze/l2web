using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using l2web.helpers;
using l2web.helpers.contracts;
using l2web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PayPalCheckoutSdk.Orders;

namespace l2web.Views.UserAccount
{
    public class IndexModel : PageModel
    {

        public IPaymentHelper PaymentHelper { get; set; }

        public List<Character> Characters { get; set; }
        public List<EpicJewelOwner> EpicOwners { get; set; }
        public List<Clan> TopClans { get; set; }
        public List<Castle> CastleInfo { get; set; }
        public int OnlinePlayers { get; set; }
        public int SelectedEpicId { get; set; }


        [BindProperty]
        public InputModel Input { get; set; }


        public class InputModel
        {
            [Range(1, 100)]
            [Required]
            public int TobuyCoinQuantity { get; set; }
        }

        
        public long getBaiumRingCount()
        {

            return EpicOwners.Sum(o => o.ItemId == 6658 ? o.Amount : 0);
        }

        public long getValakasNecklaceCount()
        {

            return EpicOwners.Sum(o => o.ItemId == 6657 ? o.Amount : 0);
        }

        public long getZakenEarringCount()
        {

            return EpicOwners.Sum(o => o.ItemId == 6659 ? o.Amount : 0);
        }

        public long getQARingCount()
        {

            return EpicOwners.Sum(o => o.ItemId == 6660 ? o.Amount : 0);
        }

        public long getFrintezzaNecklaceCount()
        {

            return EpicOwners.Sum(o => o.ItemId == 8191 ? o.Amount : 0);
        }

        public long getAntarasEarringCount()
        {

            return EpicOwners.Sum(o => o.ItemId == 90992 ? o.Amount : 0);
        }
    }
}
