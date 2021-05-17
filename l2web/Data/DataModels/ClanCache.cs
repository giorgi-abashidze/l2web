using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace l2web.Data.DataModels
{
    public class ClanCache
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public string ClanName { get; set; }

        [Required]
        public string LeaderName { get; set; }

        [Required]
        public int ClanLevel { get; set; }
        public byte[] Icon { get; set; }

    }
}
