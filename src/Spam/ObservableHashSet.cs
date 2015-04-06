using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Spam
{
	public class ObservableHashSet<T> : INotifyCollectionChanged, ICollection<T>
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

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		protected void Notify(NotifyCollectionChangedAction action, params T[] items) {
			if (CollectionChanged != null)
				CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, items));
		}


		public int Count => _container.Count;

		public bool IsReadOnly => false;


		public void Add(T item)
		{
			Notify(NotifyCollectionChangedAction.Add, item);
			_container.Add(item);
		}

		public void Clear()
		{
			Notify(NotifyCollectionChangedAction.Reset);
			_container.Clear();
		}

		public bool Contains(T item) => _container.Contains(item);

		public void CopyTo(T[] array, int arrayIndex) => _container.CopyTo(array, arrayIndex);

		public IEnumerator<T> GetEnumerator() => _container.GetEnumerator();

		public bool Remove(T item) 
		{
			Notify(NotifyCollectionChangedAction.Remove, item);
			return _container.Remove(item);
		}

		IEnumerator IEnumerable.GetEnumerator() => _container.GetEnumerator();
	}
}