using l2web.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace l2web.Models
{
    public class EpicJewelOwner
    {

        public EpicJewelOwner(string charName, int itemId, Int64 amount) {
            CharName = charName;
            ItemId = itemId;
            Amount = amount;
        }
        public string CharName { get; set; }

        public string ItemName { get; set; }

        public int ItemId { get; set; }

        public Int64 Amount { get; set; }

        public string GetItemName() {
            return HelperIds.epicIds.First(e => e.Id == ItemId).Name;
        }
    }
}
