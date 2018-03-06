using System.Collections.Generic;

using ComparerService.App.Models;

namespace ComparerService.App.Utility
{
    public class DiffEqualityComparer : IEqualityComparer<Diff>
    {
        public bool Equals(Diff x, Diff y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Equals(y);
        }

        public int GetHashCode(Diff obj)
        {
            if (obj == null)
                return 0;

            return obj.Length ^ obj.Offset;
        }
    }
}