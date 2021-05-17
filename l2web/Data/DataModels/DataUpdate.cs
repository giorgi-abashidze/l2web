using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace l2web.Data.DataModels
{
    public class DataUpdate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public DateTime LastDataUpdate { get; set; }

        public DateTime LastOnlineUpdate { get; set; }
    }
}
