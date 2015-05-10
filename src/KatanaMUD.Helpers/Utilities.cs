using System;
using System.Collections.Generic;
using System.Linq;

namespace KatanaMUD
{
    public static class Utilities
    {
        public static bool In<T>(this T item, params T[] items)
        {
            return items.Any(x => x.Equals(item));
        }
    }
}