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

            name = name.Trim();

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

        /// <summary>
        /// Finds actors using a given name.
        /// </summary>
        /// <param name="actors"></param>
        /// <param name="name"></param>
        /// <param name="findPlayersFirst">Determines if it should find a meat-space player before matching all actors in the collection.</param>
        /// <returns></returns>
        public static IEnumerable<Actor> FindActorsByName(this IEnumerable<Actor> actors, string name, bool findPlayersFirst = false)
        {
            var found = actors.Where(x => x.User != null).FindByName(name, x => x.Name, true, true);
            if(!found.Any())
            {
                found = actors.FindByName(name, x => x.Name, true, true);
            }

            return found;
        }

        /// <summary>
        /// Groups a list of items according to its template ID and name. 
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static IEnumerable<ItemGroup> Group(this IEnumerable<Item> items)
        {
            var groups = items.GroupBy(x => x.Name + "|" + x.ItemTemplate.Id.ToString());
            return groups.Select(x => new ItemGroup() { Items = x.ToList() });
        }
    }
}
