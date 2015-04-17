using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace KatanaMUD.Helpers
{
	public class ConcurrentSet<T>
	{
		private HashSet<T> _set = new HashSet<T>();
		private object _sync = new object();

		public int Count
		{
			get
			{
				lock(_sync)
				{
					return _set.Count;
				}
			}
		}

		public bool Add(T item)
		{
			lock (_sync)
			{
				return _set.Add(item);
			}
		}

		public void Clear()
		{
			lock (_sync)
			{
				_set.Clear();
			}
		}

		public bool Contains(T item)
		{
			lock (_sync)
			{
				return _set.Contains(item);
			}
		}

		public bool Remove(T item)
		{
			lock (_sync)
			{
				return _set.Remove(item);
			}
		}

		public IEnumerable<T> Snapshot()
		{
			lock(_sync)
			{
				var list = _set.ToList();
				_set.Clear();
				return list;
			}
		}
	}
}