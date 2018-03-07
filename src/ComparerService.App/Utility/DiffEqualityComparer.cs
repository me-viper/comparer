using System.Collections.Generic;

using ComparerService.App.Models;

namespace ComparerService.App.Utility
{
    public class DiffEqualityComparer : IEqualityComparer<DiffSpan>
    {
        public bool Equals(DiffSpan x, DiffSpan y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Equals(y);
        }

        public int GetHashCode(DiffSpan obj)
        {
            if (obj == null)
                return 0;

            return obj.Length ^ obj.Offset;
        }
    }
}