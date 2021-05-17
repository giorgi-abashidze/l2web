using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace l2web.Data.DataModels
{
    public class CharacterCache
    {
        public string AccountId { get; set; }

        [JsonIgnore]
        public Account Account { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Name { get; set; }
        public int Lvl { get; set; }
        public int OcupationIndex { get; set; }
        public int RaceIndex { get; set; }
    }
}
