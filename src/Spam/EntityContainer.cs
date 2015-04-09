using KatanaMUD;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Spam
{
    public class EntityContainer<T, K> : IEnumerable<T>, IEntityContainer<K>, IEntityContainer where T : Entity<K>, new()
    {
        EntityContext _context;
        Dictionary<K, T> _storage = new Dictionary<K, T>();
        HashSet<Entity<K>> _changed = new HashSet<Entity<K>>(comparer: EntityContainer<T, K>.GetDefaultHashComparer<K>());
        HashSet<Entity<K>> _new = new HashSet<Entity<K>>(comparer: EntityContainer<T, K>.GetDefaultHashComparer<K>());
        HashSet<Entity<K>> _deleted = new HashSet<Entity<K>>(comparer: EntityContainer<T, K>.GetDefaultHashComparer<K>());
        KeyGenerator<K> _keyGenerator = EntityContainer<T, K>.GetDefaultKeyGenerator<K>();

        public EntityContainer(EntityContext context)
        {
            _context = context;
        }

        public T this[K key] { get { return _storage[key]; } }

        public int Count => _storage.Count;

        public bool IsReadOnly => false;

        public ICollection<K> Keys => _storage.Keys;

        public ICollection<T> Values => _storage.Values;

        public T New()
        {
            return New(default(K));
        }

        public T New(K key)
        {
            T item = new T();
            if(!key.Equals(default(K)))
            {
                item.Key = key;
            }

            Add(item, false);
            return item;
        }

        internal void Add(T item, bool fromLoad)
        {
            // Set the key if it's not already set.
            if (item.Key.Equals(default(K)))
            {
                if (_keyGenerator != null)
                    item.Key = _keyGenerator.NewKey();
                else
                    throw new InvalidOperationException("No Key Generator, must specify a key.");
            }

            if (_storage.ContainsKey(item.Key))
            {
                // Sanity check, make sure we're not overwriting an existing item.
                throw new InvalidOperationException("Key already exists");
            }

            if (item.Container != null)
            {
                // Another sanity check. Shouldn't ever happen but hey why not.
                throw new InvalidOperationException("Entity already belongs to another container");
            }

            if (!fromLoad)
                _new.Add(item);
            item.Container = (IEntityContainer<K>)this;
            item.Attach(_context);
            _storage[item.Key] = item;
        }

        public void Clear()
        {
            foreach (var item in _storage.Values.ToList())
            {
                Remove(item);
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
            if (!_storage.TryGetValue(key, out item))
            {
                return false;
            }

            return Remove(item);
        }

        public bool Remove(T item)
        {
            _storage.Remove(item.Key);
            if (!_new.Remove(item))
            {
                _deleted.Add(item);
            }
            _changed.Remove(item);

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

        internal static IEqualityComparer<Key> GetDefaultComparer<Key>()
        {
            return (IEqualityComparer<Key>)EqualityComparer<Key>.Default;
        }

        internal static IEqualityComparer<Entity<Key>> GetDefaultHashComparer<Key>()
        {
            return new EntityCompare<Key>(GetDefaultComparer<Key>());
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
            else if(typeof(Key) == typeof(string))
            {
                return null;
            }

            throw new InvalidOperationException("Type of key not supported: " + typeof(Key).Name);
        }

        public void SetChanged(Entity<K> entity)
        {
            this._changed.Add(entity);
        }

        public bool IsChanged(Entity<K> entity)
        {
            return this._changed.Contains(entity);
        }

        public bool IsNew(Entity<K> entity)
        {
            return this._new.Contains(entity);
        }

        public bool IsDeleted(Entity<K> entity)
        {
            return this._deleted.Contains(entity);
        }

        public IEnumerable<IEntity> ChangedEntities => _changed;

        public IEnumerable<IEntity> NewEntities => _new;

        public IEnumerable<IEntity> DeletededEntities => _deleted;

        public void ClearChanges()
        {
            _changed.Clear();
            _new.Clear();
            _deleted.Clear();
        }
    }

    internal class EntityCompare<K> : IEqualityComparer<Entity<K>>
    {
        private IEqualityComparer<K> _keyCompare;
        public EntityCompare(IEqualityComparer<K> keyCompare)
        {
            _keyCompare = keyCompare;
        }

        public bool Equals(Entity<K> x, Entity<K> y)
        {
            return this._keyCompare.Equals(x.Key, y.Key);
        }

        public int GetHashCode(Entity<K> obj)
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

    public class LinkEntityCompare<K1, K2> : IEqualityComparer<Tuple<K1, K2>>
    {
        public bool Equals(Tuple<K1, K2> x, Tuple<K1, K2> y)
        {
            return x.Item1.Equals(y.Item1) && x.Item2.Equals(y.Item2);
        }

        public int GetHashCode(Tuple<K1, K2> obj)
        {
			// Perform an XOR on the respective hash codes. It's consistent, 
			// it takes both into account, and it's fast. Should work.
            return obj.Item1.GetHashCode() ^ obj.Item2.GetHashCode();
        }
    }
}