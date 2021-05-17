using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace l2web.Data.DataModels
{
    public class CastleCache
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public string CastleName { get; set; }
        public string Owner { get; set; }

    }
}
