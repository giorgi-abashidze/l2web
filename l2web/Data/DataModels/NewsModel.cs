using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace l2web.Data.DataModels
{
    public class NewsModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        public string Body { get; set; }

        public DateTime CreateDate { get; set; }

        public string BackgroundImage { get; set; }

        public bool IsVideo { get; set; }

        public bool IsPinned { get; set; }

        public string VideoLink { get; set; }


    }
}
