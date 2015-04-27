using System;
using System.Collections.Generic;
using System.Linq;

namespace KatanaMUD
{
    public static class EnumerableHelper
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> toAdd)
        {
            foreach (var item in toAdd)
                collection.Add(item);
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> collection, T item)
        {
            return collection.Except(new T[] { item });
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
                action(item);
        }
    }
}