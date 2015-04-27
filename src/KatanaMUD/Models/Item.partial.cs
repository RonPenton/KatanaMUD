using System;
using System.Collections.Generic;
using Spam;

namespace KatanaMUD.Models
{
    public partial class Item : IItem
    {
        public string Name => CustomName ?? ItemTemplate.Name;

        /// <summary>
        /// Gets a stat from the item. Tries to pick it off local item first, then falls back to item template.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetStat<T>(string name, T defaultValue)
        {
            object value;
            if (!((JsonContainer)Stats).GetValue(name, out value))
            {
                if (!((JsonContainer)ItemTemplate.Stats).GetValue(name, out value))
                {
                    return defaultValue;
                }
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }

        public long Weight => GetStat<long>("Weight", 0);

        /// <summary>
        /// A list of users who currently know of my existence. 
        /// This list is cleared when the owner of an item is changed.
        /// TODO: Clear this list at cleanup to prevent memory leaks.
        /// </summary>
        public HashSet<Actor> UsersWhoFoundMe { get; } = new HashSet<Actor>();

        partial void OnRoomChanging(Room oldValue, Room newValue)
        {
            if (oldValue != newValue)
                UsersWhoFoundMe.Clear();
        }
    }

    public interface IItem
    {
        string Name { get; }
    }
}