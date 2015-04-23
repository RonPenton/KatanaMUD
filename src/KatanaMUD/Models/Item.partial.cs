using System;
using Spam;

namespace KatanaMUD.Models
{
    public partial class Item
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
    }
}