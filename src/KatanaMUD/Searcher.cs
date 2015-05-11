using KatanaMUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KatanaMUD
{
    public static class Searcher
    {
        public static IEnumerable<T> FindByName<T>(this IEnumerable<T> items, string name, Func<T, string> getter, bool includeStartsWith = false, bool includeSubstring = false)
        {
            if (String.IsNullOrWhiteSpace(name))
                return new List<T>();

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

        public static IEnumerable<ItemGroup> FindItemsByName(this IEnumerable<Item> items, string name)
        {
            var found = items.FindByName(name, x => x.Name, true, true);
            return found.Group();
        }

        public static IEnumerable<ItemGroup> Group(this IEnumerable<Item> items)
        {
            var groups = items.GroupBy(x => x.Name + "|" + x.ItemTemplate.Id.ToString());
            return groups.Select(x => new ItemGroup() { Items = x.ToList() });
        }
    }
}
