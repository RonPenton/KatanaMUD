using System;
using System.Collections.Generic;
using System.Linq;

namespace KatanaMUD
{
    public static class Utilities
    {
        public static T FindByName<T>(this IEnumerable<T> items, string name, Func<T, string> getter, bool includeStartsWith = false, bool includeSubstring = false)
        {
            // First look for an exact match.
            var target = items.FirstOrDefault(x => getter(x).Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (target == null && includeStartsWith)
            {
                // If not, look for items that start with 
                target = items.FirstOrDefault(x => getter(x).StartsWith(name, StringComparison.InvariantCultureIgnoreCase));
            }
            if (target == null && includeSubstring)
            {
                // lastly find substring matches
                target = items.FirstOrDefault(x => getter(x).IndexOf(name, StringComparison.InvariantCultureIgnoreCase) >= 0);
            }
            return target;
        }
    }
}