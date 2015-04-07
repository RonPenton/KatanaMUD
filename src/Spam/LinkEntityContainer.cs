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
		HashSet<Tuple<K1, K2>> _new = new HashSet<Tuple<K1, K2>>(new LinkEntityCompare<K1, K2>());
		HashSet<Tuple<K1, K2>> _deleted = new HashSet<Tuple<K1, K2>>(new LinkEntityCompare<K1, K2>());

		public LinkEntityContainer()
		{
		}

		public int Count => _storage.Count;

		public bool IsReadOnly => false;

		public bool Contains(Tuple<K1, K2> item) => _storage.Contains(item);

		public IEnumerator<Tuple<K1, K2>> GetEnumerator() => _storage.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => _storage.GetEnumerator();

		public bool IsNew(Tuple<K1, K2> entity) => this._new.Contains(entity);

		public bool IsDeleted(Tuple<K1, K2> entity) => this._deleted.Contains(entity);

		public void ClearChanges()
		{
			_changed.Clear();
			_new.Clear();
			_deleted.Clear();
		}

		public void Link(E1 item1, E2 item2, bool fromLoad)
		{
			var key = new Tuple<K1, K2>(item1.Key, item2.Key);
			if (!fromLoad)
				_new.Add(key);
			_storage.Add(key);
		}

		public void Unlink(E1 item1, E2 item2)
		{
			var key = new Tuple<K1, K2>(item1.Key, item2.Key);

			bool removed = _storage.Remove(key);
			if (removed && !_new.Remove(key))
			{
				_deleted.Add(key);
			}
		}
	}
}