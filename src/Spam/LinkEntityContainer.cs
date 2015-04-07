using KatanaMUD;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Spam
{
	public class LinkEntityContainer<E1, E2, K1, K2> : IEnumerable<Tuple<K1, K2>> where E1 : Entity<K1> where E2 : Entity<K2>
	{
		HashSet<Tuple<K1, K2>> _storage = new HashSet<Tuple<K1, K2>>(new LinkEntityCompare<K1, K2>());
		HashSet<Tuple<K1, K2>> _changed = new HashSet<Tuple<K1, K2>>(new LinkEntityCompare<K1, K2>());
		HashSet<Tuple<K1, K2>> _new = new HashSet<Tuple<K1, K2>>(new LinkEntityCompare<K1, K2>());
		HashSet<Tuple<K1, K2>> _deleted = new HashSet<Tuple<K1, K2>>(new LinkEntityCompare<K1, K2>());

		public LinkEntityContainer()
		{
		}

		public int Count => _storage.Count;

		public bool IsReadOnly => false;

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
	}
}