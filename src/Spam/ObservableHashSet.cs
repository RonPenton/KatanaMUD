using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Spam
{
    public class ObservableHashSet<T> : IEnumerable<T>
    {
        private HashSet<T> _container;

        public ObservableHashSet()
        {
            _container = new HashSet<T>();
        }

        public ObservableHashSet(IEnumerable<T> collection)
        {
            _container = new HashSet<T>(collection);
        }

        public ObservableHashSet(IEqualityComparer<T> comparer)
        {
            _container = new HashSet<T>(comparer);
        }

        public ObservableHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        {
            _container = new HashSet<T>(collection, comparer);
        }

        public event EventHandler<CollectionChangedEventArgs<T>> ItemsAdded;

        public event EventHandler<CollectionChangedEventArgs<T>> ItemsRemoved;

        protected void NotifyAdd(params T[] items)
        {
            if (ItemsAdded != null)
                ItemsAdded(this, new CollectionChangedEventArgs<T>(items));
        }
        protected void NotifyRemove(params T[] items)
        {
            if (ItemsRemoved != null)
                ItemsRemoved(this, new CollectionChangedEventArgs<T>(items));
        }

        public int Count => _container.Count;

        public bool IsReadOnly => false;

        public void AddRange(IEnumerable<T> items, bool skipNotify = false)
        {
            List<T> added = new List<T>();
            foreach (var item in items)
            {
                if (_container.Add(item) && !skipNotify)
                {
                    added.Add(item);
                }
            }

            if (!skipNotify && added.Any())
                NotifyAdd(added.ToArray());
        }

        public void Add(T item, bool skipNotify = false)
        {
            if (_container.Add(item) && !skipNotify)
            {
                NotifyAdd(item);
            }
        }

        public void Clear()
        {
            var items = _container.ToArray();
            if (items.Any())
            {
                _container.Clear();
                NotifyRemove(items);
            }
        }

        public bool Contains(T item) => _container.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _container.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => _container.GetEnumerator();

        public bool Remove(T item, bool skipNotify = false)
        {
            if (_container.Remove(item))
            {
                if (!skipNotify)
                    NotifyRemove(item);
                return true;
            }
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator() => _container.GetEnumerator();
    }

    public class CollectionChangedEventArgs<T>
    {
        public CollectionChangedEventArgs(T[] items)
        {
            Items = items;
        }

        public T[] Items { get; private set; }
    }
}