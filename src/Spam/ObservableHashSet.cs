using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Spam
{
	public class ObservableHashSet<T> : ICollection<T>
	{
		private HashSet<T> _container;

        public ObservableHashSet()
		{
			_container = new HashSet<T>();
		}

		public ObservableHashSet(IEnumerable<T> collection) {
			_container = new HashSet<T>(collection);
		}

		public ObservableHashSet(IEqualityComparer<T> comparer) {
			_container = new HashSet<T>(comparer);
		}

		public ObservableHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
		{
			_container = new HashSet<T>(collection, comparer);
		}

        public event EventHandler<CollectionChangedEventArgs<T>> CollectionChanged;

        protected void Notify(CollectionChangedAction action, params T[] items) {
            if (CollectionChanged != null)
                CollectionChanged(this, new CollectionChangedEventArgs<T>(action, items));
		}

		public int Count => _container.Count;

		public bool IsReadOnly => false;

		public void Add(T item)
		{
			if(_container.Add(item))
            {
                Notify(CollectionChangedAction.Add, item);
            }
        }

        public void Clear()
        {
            var any = _container.Any();
            if (any)
            {
                _container.Clear();
                Notify( .Reset);
            }
        }

		public bool Contains(T item) => _container.Contains(item);

		public void CopyTo(T[] array, int arrayIndex) => _container.CopyTo(array, arrayIndex);

		public IEnumerator<T> GetEnumerator() => _container.GetEnumerator();

        public bool Remove(T item)
        {
            if (_container.Remove(item))
            {
                Notify(NotifyCollectionChangedAction.Remove, item);
                return true;
            }
            return false;
        }

		IEnumerator IEnumerable.GetEnumerator() => _container.GetEnumerator();
	}

    public class CollectionChangedEventArgs<T>
    {
        public CollectionChangedEventArgs(CollectionChangedAction action, T[] items)
        {
            Action = action;
            Items = items;
        }

        public CollectionChangedAction Action { get; private set; }

        public T[] Items { get; private set; }
    }

    public enum CollectionChangedAction
    {
        Add,
        Remove
    }
}