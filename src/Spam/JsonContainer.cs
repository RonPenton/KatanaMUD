using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Spam
{
    public class JsonContainer : DynamicObject
    {
        IEntity _owner;
        Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        public JsonContainer(IEntity owner)
        {
            this._owner = owner;
        }

        public int Count => _dictionary.Count;

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return _dictionary.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _dictionary[binder.Name] = value;
            if(_owner != null)
                _owner.Changed();
            return true;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this._dictionary);
        }

        public void FromJson(string json)
        {
            if(String.IsNullOrWhiteSpace(json))
            {
                this._dictionary = new Dictionary<string, object>();
                return;
            }
            this._dictionary = (Dictionary<string, object>)JsonConvert.DeserializeObject(json, typeof(Dictionary<string, object>));
        }

        public static dynamic Combine(params dynamic[] itemArray)
        {
            return Combine(items: itemArray);
        }

        public static dynamic Combine(IEnumerable<dynamic> items)
        {
            var result = new JsonContainer(null);
            foreach (var item in items)
                Merge(result, item);
            return result;
        }

        public static void Merge(dynamic left, params dynamic[] right)
        {
            foreach (var item in right)
                Merge(left, item);
        }

        public static void Merge(dynamic left, dynamic right)
        {
            var ljson = left as JsonContainer;
            var rjson = right as JsonContainer;

            if (ljson == null || rjson == null)
                throw new InvalidOperationException("Cannot combine the supplied objects");


            foreach(var key in rjson._dictionary.Keys)
            {
                object lv = null;
                object rv = rjson._dictionary[key];
                if(!ljson._dictionary.TryGetValue(key, out lv))
                {
                    // left value doesn't exist at all. Simply set it and exit.
                    ljson._dictionary[key] = rv;
                    continue;
                }

                var lvtype = lv.GetType();
                var rvtype = rv.GetType();

                // ltype must be equal to rtype in all instances except long/double. In the case of long/double,
                // the ltype can be "upgraded" to a double, though with the potential to lose precision. 
                if((lvtype == typeof(long) || lvtype == typeof(double)) && rvtype == typeof(double))
                {
                    lv = Convert.ToDouble(lv) + Convert.ToDouble(rv);
                }
                else if(lvtype != rvtype || lvtype == typeof(bool) || lvtype == typeof(string))
                {
                    // For unequal types, it's a difficult decision. Do we want to crash the game? 
                    // Fuck if I know. Why don't we overwrite and see if that causes any problems down the line.
                    lv = rv;
                }
                else if(lvtype == typeof(long))
                {
                    lv = Convert.ToInt64(lv) + Convert.ToInt64(rv);
                }
                else
                {
                    throw new InvalidOperationException("JSONContainer Type not supported: " + lvtype.ToString());
                }

                ljson._dictionary[key] = lv;
            }
        }
    }
}