using System;
using System.Collections;
using System.Collections.Generic;

namespace Spam
{
    public class LinkEntity<K1, K2> : Entity<Tuple<K1, K2>>
    {
        Tuple<K1, K2> _key;

        public LinkEntity(K1 k1, K2 k2)
        {
            _key = new Tuple<K1, K2>(k1, k2);
        }

        public override Tuple<K1, K2> Key
        {
            get { return _key; }
            set
            {
                if (value == null)
                    throw new InvalidOperationException();
                _key = value;
            }
        }
    }
}