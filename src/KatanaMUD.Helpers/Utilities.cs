using System;
using System.Collections.Generic;
using System.Linq;

namespace KatanaMUD
{
    public static class Utilities
    {
        public static IEnumerable<T> FindByName<T>(this IEnumerable<T> items, string name, Func<T, string> getter, bool includeStartsWith = false, bool includeSubstring = false)
        {
            // First look for an exact match.
            var found = items.Where(x => getter(x).Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (!found.Any() && includeStartsWith)
            {
                // If not, look for items that start with 
                found = items.Where(x => getter(x).StartsWith(name, StringComparison.InvariantCultureIgnoreCase));
            }
            if (!found.Any() && includeSubstring)
            {
                // lastly find substring matches
                found = items.Where(x => getter(x).IndexOf(name, StringComparison.InvariantCultureIgnoreCase) >= 0);
            }
            return found.ToList();
        }

        public static bool In<T>(this T item, params T[] items)
        {
            return items.Any(x => x.Equals(item));
        }
    }
}