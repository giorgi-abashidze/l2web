using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace l2web.Models
{
    public class Castle
    {
        public string CastleName { get; set; }
        public string Owner { get; set; }

        public string getNormalizedCastleName() {
            var underscFree = CastleName.Replace('_', ' ');

            return char.ToUpper(underscFree[0]) + underscFree.Substring(1);
        }
    }
}
