using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace l2web.Data.DataModels
{
    public class Account
    {

        public string userId { get; set; }

        [JsonIgnore]
        public ApplicationUser User { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public string Id { get; set; }

        public virtual ICollection<CharacterCache> Characters { get; set; }
    }
}
