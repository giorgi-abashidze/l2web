using l2web.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace l2web.Models
{
    public class Character
    {
        public string Name { get; set; }
        public int Lvl { get; set; }
        public int OcupationIndex { get; set; }
        public int RaceIndex { get; set; }


        public string GetRaceName()
        {
            return HelperIds.races.First(r => r.Id == RaceIndex).Name;
        }

        public string GetClassName()
        {
            return HelperIds.classes.First(r => r.Id == OcupationIndex).Name;
        }

    }

   
}
