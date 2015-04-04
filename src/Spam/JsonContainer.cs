using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Spam
{
    public class JsonContainer : DynamicObject
    {
        IEntity _owner;
        Dictionary<string, object> _dictionary = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

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
            this._owner.Changed();
            return true;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void FromJson(string json)
        {
            this._dictionary = (Dictionary<string, object>)JsonConvert.DeserializeObject(json, typeof(Dictionary<string, object>));
        }
    }
}