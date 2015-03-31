using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Spam
{
    public class EntityContainer<T, K> : ICollection<T> where T : Entity<K>
    {
        Dictionary<K, T> _storage = new Dictionary<K, T>();
        HashSet<T> _changed = new HashSet<T>(comparer: EntityContainer<T, K>.GetDefaultHashComparer<T, K>());
        HashSet<T> _new = new HashSet<T>(comparer: EntityContainer<T, K>.GetDefaultHashComparer<T, K>());
        HashSet<T> _deleted = new HashSet<T>(comparer: EntityContainer<T, K>.GetDefaultHashComparer<T, K>());
        KeyGenerator<K> _keyGenerator = EntityContainer<T, K>.GetDefaultKeyGenerator<K>();

        public T this[K key] { get { return _storage[key]; } }

        public int Count => _storage.Count;

        public bool IsReadOnly => false;

        public ICollection<K> Keys => _storage.Keys;

        public ICollection<T> Values => _storage.Values;

        public void Add(T item)
        {
            // Set the key if it's not already set.
            if(item.Key.Equals(default(K)))
            {
                item.Key = _keyGenerator.NewKey();
            }

            if(_storage.ContainsKey(item.Key))
            {
                // Sanity check, make sure we're not overwriting an existing item.
                throw new InvalidOperationException("Key already exists");
            }

            _new.Add(item);
            _storage[item.Key] = item;
        }

        public void Clear()
        {
            foreach (var item in _storage)
            {
                _deleted.Add(item.Value);
            }

            _storage.Clear();
        }

        public bool Contains(T item) => _storage.ContainsKey(item.Key);

        public bool ContainsKey(K key) => _storage.ContainsKey(key);

        public void CopyTo(T[] array, int arrayIndex) => _storage.Values.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => _storage.Values.GetEnumerator();

        public bool Remove(K key)
        {
            T item;
            if(!_storage.TryGetValue(key, out item))
            {
                return false;
            }

            return Remove(item);
        }

        public bool Remove(T item)
        {
            _storage.Remove(item.Key);
            _deleted.Add(item);
            return true;
        }

        public bool TryGetValue(K key, out T value)
        {
            return _storage.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _storage.GetEnumerator();
        }

        private static IEqualityComparer<Key> GetDefaultComparer<Key>()
        {
            return (IEqualityComparer<Key>)EqualityComparer<Key>.Default;
        }

        private static IEqualityComparer<Ty> GetDefaultHashComparer<Ty, Key>() where Ty : Entity<Key>
        {
            return new EntityCompare<Ty, Key>(GetDefaultComparer<Key>());
        }

        private static KeyGenerator<Key> GetDefaultKeyGenerator<Key>()
        {
            if (typeof(Key) == typeof(Guid))
            {
                return (KeyGenerator<Key>)new GuidKeyGenerator();
            }
            else if (typeof(Key) == typeof(int))
            {
                return (KeyGenerator<Key>)new IntKeyGenerator();
            }
            else if (typeof(Key) == typeof(long))
            {
                return (KeyGenerator<Key>)new LongKeyGenerator();
            }

            throw new InvalidOperationException("Type of key not supported: " + typeof(Key).Name);
        }
    }

    internal class EntityCompare<T, K> : IEqualityComparer<T> where T : Entity<K>
    {
        private IEqualityComparer<K> _keyCompare;
        public EntityCompare(IEqualityComparer<K> keyCompare)
        {
            _keyCompare = keyCompare;
        }

        public bool Equals(T x, T y)
        {
            return this._keyCompare.Equals(x.Key, y.Key);
        }

        public int GetHashCode(T obj)
        {
            return _keyCompare.GetHashCode(obj.Key);
        }
    }

    internal interface KeyGenerator<K>
    {
        K NewKey();
        void Populated(IEnumerable<Entity<K>> collection);
    }

    internal class GuidKeyGenerator : KeyGenerator<Guid>
    {
        public Guid NewKey()
        {
            return Guid.NewGuid();
        }

        public void Populated(IEnumerable<Entity<Guid>> collection) { }
    }

    internal class IntKeyGenerator : KeyGenerator<int>
    {
        private int _nextKey = 1;

        public int NewKey()
        {
            var val = _nextKey;
            _nextKey++;
            return val;
        }

        public void Populated(IEnumerable<Entity<int>> collection)
        {
            _nextKey = collection.Max(x => x.Key) + 1;
        }
    }

    internal class LongKeyGenerator : KeyGenerator<long>
    {
        private long _nextKey = 1;

        public long NewKey()
        {
            var val = _nextKey;
            _nextKey++;
            return val;
        }

        public void Populated(IEnumerable<Entity<long>> collection)
        {
            _nextKey = collection.Max(x => x.Key) + 1;
        }
    }

}