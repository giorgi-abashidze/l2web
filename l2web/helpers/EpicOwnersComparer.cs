using l2web.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace l2web.helpers
{
    public class EpicOwnersComparer : IEqualityComparer<EpicOwnersCache>
    {

        public bool Equals(EpicOwnersCache x1, EpicOwnersCache x2)
        {
            if (object.ReferenceEquals(x1, x2))
            {
                return true;
            }
            if (object.ReferenceEquals(x1, null) ||
                object.ReferenceEquals(x2, null))
            {
                return false;
            }

            return x1.CharName.Equals(x2.CharName) && x1.ItemId == x2.ItemId;
        }

        public int GetHashCode([DisallowNull] EpicOwnersCache obj)
        {
            if (obj == null)
            {
                return 0;
            }
            return obj.CharName.GetHashCode()+obj.ItemId.GetHashCode();
        }
    }
}
