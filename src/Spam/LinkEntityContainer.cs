using KatanaMUD;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Spam
{
	public class LinkEntityContainer<E1, E2, K1, K2> : ICollection<Tuple<K1, K2>> where E1 : Entity<K1> where E2 : Entity<K2>
	{
		HashSet<Tuple<K1, K2>> _storage = new HashSet<Tuple<K1, K2>>(new LinkEntityCompare<K1, K2>());
		HashSet<Tuple<K1, K2>> _changed = new HashSet<Tuple<K1, K2>>(new LinkEntityCompare<K1, K2>());
		HashSet<Tuple<K1, K2>> _new = new HashSet<Tuple<K1, K2>>(new LinkEntityCompare<K1, K2>());
		HashSet<Tuple<K1, K2>> _deleted = new HashSet<Tuple<K1, K2>>(new LinkEntityCompare<K1, K2>());

		ObservableHashSet<E1> _collection1 = new ObservableHashSet<E1>(EntityContainer<E1, K1>.GetDefaultHashComparer<K1>());
		ObservableHashSet<E2> _collection2 = new ObservableHashSet<E2>(EntityContainer<E2, K2>.GetDefaultHashComparer<K2>());

		public LinkEntityContainer()
		{
			_collection1.CollectionChanged += _collection1_CollectionChanged;
			_collection2.CollectionChanged += _collection2_CollectionChanged;
		}

		public int Count => _storage.Count;

		public bool IsReadOnly => false;

		public void Add(Tuple<K1, K2> item)
		{
			Add(item, false);
		}

		internal void Add(Tuple<K1, K2> item, bool fromLoad)
		{
			if (_storage.Contains(item))
			{
				// Sanity check, make sure we're not overwriting an existing item.
				throw new InvalidOperationException("Key already exists");
			}

			if (!fromLoad)
				_new.Add(item);
			_storage.Add(item);
		}

		public void Clear()
		{
			foreach (var item in _storage.ToList())
			{
				Remove(item);
			}

			_storage.Clear();
		}

		public bool Contains(Tuple<K1, K2> item) => _storage.Contains(item);

		public void CopyTo(Tuple<K1, K2>[] array, int arrayIndex) => _storage.CopyTo(array, arrayIndex);

		public IEnumerator<Tuple<K1, K2>> GetEnumerator() => _storage.GetEnumerator();

		public bool Remove(Tuple<K1, K2> item)
		{
			bool removed = _storage.Remove(item);
			if (!_new.Remove(item))
			{
				_deleted.Add(item);
			}
			_changed.Remove(item);

			return removed;
		}

		IEnumerator IEnumerable.GetEnumerator() => _storage.GetEnumerator();

		public bool IsChanged(Tuple<K1, K2> entity) => this._changed.Contains(entity);

		public bool IsNew(Tuple<K1, K2> entity) => this._new.Contains(entity);

		public bool IsDeleted(Tuple<K1, K2> entity) => this._deleted.Contains(entity);

		public void ClearChanges()
		{
			_changed.Clear();
			_new.Clear();
			_deleted.Clear();
		}

		private void _collection2_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void _collection1_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			throw new NotImplementedException();
		}

	}
}