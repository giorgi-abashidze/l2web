using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace l2web.Models
{
    public class Race
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Race(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
