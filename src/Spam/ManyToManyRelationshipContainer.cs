using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Spam
{
    public class ManyToManyRelationshipContainer<E1, E2, K1, K2> : ICollection<E1> where E1 : Entity<K1> where E2 : Entity<K2>
    {
        HashSet<E1> _container = new HashSet<E1>(EntityContainer<E1, K1>.GetDefaultHashComparer<K1>());

        ManyToManyRelationshipContainer<E2, E1, K2, K1> _otherContainer;
        

        public ManyToManyRelationshipContainer()
        {
        }

        public int Count => _container.Count;

        public bool IsReadOnly => false;

        public void Add(E1 item)
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
            foreach (var item in this._container.ToList())
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