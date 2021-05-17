using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace l2web.Data.DataModels
{
    public class EpicOwnersCache
    {
        [Required]
        public string CharName { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public Int64 Amount { get; set; }
    }
}
