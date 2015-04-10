using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Spam
{
    public class ParentChildRelationshipContainer<P, C, K> : ICollection<C> where C : Entity<K>, new() where P : class where K : struct
    {
        HashSet<C> _container = new HashSet<C>(EntityContainer<C, K>.GetDefaultHashComparer<K>());
        P _parent;
        Func<C, P> _getter;
        Action<C, P> _setter;

        public ParentChildRelationshipContainer(P parent, Func<C, P> getter, Action<C, P> setter)
        {
            _parent = parent;
            _getter = getter;
            _setter = setter;
        }

        public int Count => _container.Count;

        public bool IsReadOnly => false;

        public void AddRange(IEnumerable<C> items)
        {
            foreach (var item in items)
                Add(item);
        }

        public void Add(C item)
        {
            if (item == null)
                throw new InvalidOperationException("Null entities are not allowed as children to entities");

            // Add the item to my container
            if (_container.Add(item))
            {
                // Check to see if the item's parent is already pointing to me. 
                var itemParent = _getter(item);
                if (_parent != itemParent)
                {
                    // If not, set the new parent.
                    _setter(item, _parent);
                }
            }
        }

        public void Clear()
        {
            foreach(var item in this._container.ToList())
            {
                Remove(item);
            }
        }

        public bool Contains(C item) => _container.Contains(item);

        public void CopyTo(C[] array, int arrayIndex) => _container.CopyTo(array, arrayIndex);

        public IEnumerator<C> GetEnumerator() => _container.GetEnumerator();

        public bool Remove(C item)
        {
            bool removed = _container.Remove(item);
            _setter(item, null);
            return removed;
        }

        IEnumerator IEnumerable.GetEnumerator() => _container.GetEnumerator();
    }
}