using KatanaMUD;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Spam
{
    public class JsonContainer : IDictionaryStore
    {
        IEntity _owner;
        Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        public object this[string key]
        {
            get
            {
                return _dictionary[key];
            }
            set
            {
                _dictionary[key] = value;
                if (_owner != null)
                    _owner.Changed();
            }
        }

        public JsonContainer(IEntity owner, string data)
        {
            this._owner = owner;
            FromJson(data);
        }

        public int Count => _dictionary.Count;

        public bool ContainsKey(string key) => _dictionary.ContainsKey(key);

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this._dictionary);
        }

        private void FromJson(string json)
        {
            if (String.IsNullOrWhiteSpace(json))
            {
                this._dictionary = new Dictionary<string, object>();
                return;
            }
            this._dictionary = (Dictionary<string, object>)JsonConvert.DeserializeObject(json, typeof(Dictionary<string, object>));
        }

        public bool TryGetValue<T>(string name, out T value)
        {
            object o;
            if (_dictionary.TryGetValue(name, out o))
            {
                value = (T)Convert.ChangeType(o, typeof(T));
                return true;
            }

            value = default(T);
            return false;
        }

        public T Get<T>(string name)
        {
            object o = _dictionary[name];
            return (T)Convert.ChangeType(o, typeof(T));
        }

        public void Set<T>(string name, T value)
        {
            this[name] = value;
        }
    }

    public class AddingContainer : IDictionaryStore
    {
        private Func<IEnumerable<IDictionaryStore>> _enumerator;
        private JsonContainer _primary;

        public AddingContainer(JsonContainer primary, Func<IEnumerable<IDictionaryStore>> enumerator)
        {
            _primary = primary;
            _enumerator = enumerator;
        }

        private Arithmetic<T> CheckType<T>()
        {

            if (typeof(T) == typeof(long))
            {
                return new LongArithmetic<T>();
            }
            else if (typeof(T) != typeof(double))
            {
                return new DoubleArithmetic<T>();
            }
            else
            {
                throw new InvalidOperationException("Invalid Type Specified. AddingContainers only support 'long' and 'double'");
            }
        }

        /// <summary>
        /// Gets the calculated value of the container, including all linked containers.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetCalculatedValue<T>(string name)
        {
            Arithmetic<T> math = CheckType<T>();

            var accumulator = default(T);
            var valid = _enumerator().Where(x => x.ContainsKey(name)).ToList();

            foreach (var container in valid)
            {
                T t;
                container.TryGetValue<T>(name, out t);
                accumulator = math.Add(accumulator, t);
            }

            //if (includePercent)
            //{
            //    var pct = GetCalculatedValue<double>(name + "Pct", 0.0, false);
            //    return math.Percent(accumulator, pct);
            //}

            return accumulator;
        }

        /// <summary>
        /// Gets the local value residing only in the primary container.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetLocalValue<T>(string name)
        {
            CheckType<T>();

            T t;
            if (_primary.TryGetValue(name, out t))
                return t;

            return default(T);
        }

        /// <summary>
        /// Sets the calculated value for the given key. This will retrieve the current calculated value, compute the difference
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetCalculatedValue<T>(string name, T value)
        {
            var math = CheckType<T>();

            var current = GetCalculatedValue<T>(name);
            var difference = math.Subtract(value, current);
            AddLocalValue(name, difference);
        }

        public void SetLocalValue<T>(string name, T value)
        {
            CheckType<T>();
            _primary[name] = value;
        }

        public void AddLocalValue<T>(string name, T value)
        {
            var math = CheckType<T>();
            var current = GetLocalValue<T>(name);
            _primary[name] = math.Add(current, value);
        }

        public bool TryGetValue<T>(string name, out T value)
        {
            value = GetCalculatedValue<T>(name);
            return true;
        }

        public bool ContainsKey(string key)
        {
            return _enumerator().Any(x => x.ContainsKey(key));
        }

        private abstract class Arithmetic<T>
        {
            public abstract T Add(object left, object right);
            public abstract T Subtract(object left, object right);
            public abstract T Percent(object value, double percent);
        }

        private class LongArithmetic<T> : Arithmetic<T>
        {
            public override T Add(object left, object right)
            {
                return (T)(object)(Convert.ToInt64(left) + Convert.ToInt64(right));
            }

            public override T Percent(object value, double percent)
            {
                return (T)(object)Convert.ToInt64((Convert.ToInt64(value) * (1.0 + (percent / 100.0))));
            }

            public override T Subtract(object left, object right)
            {
                return (T)(object)(Convert.ToInt64(left) - Convert.ToInt64(right));
            }
        }

        private class DoubleArithmetic<T> : Arithmetic<T>
        {
            public override T Add(object left, object right)
            {
                return (T)(object)(Convert.ToDouble(left) + Convert.ToDouble(right));
            }

            public override T Percent(object value, double percent)
            {
                return (T)(object)Convert.ToDouble((Convert.ToDouble(value) * (1.0 + (percent / 100.0))));
            }

            public override T Subtract(object left, object right)
            {
                return (T)(object)(Convert.ToDouble(left) - Convert.ToDouble(right));
            }
        }
    }

    public class CoalescingContainer : IDictionaryStore
    {
        private Func<IEnumerable<IDictionaryStore>> _enumerator;
        private JsonContainer _primary;

        public CoalescingContainer(JsonContainer primary, Func<IEnumerable<IDictionaryStore>> enumerator)
        {
            _primary = primary;
            _enumerator = enumerator;
        }

        private void CheckType<T>()
        {
            //TODO: Determine if we need to add more types.
            if (!typeof(T).In(typeof(int), typeof(long), typeof(double), typeof(string), typeof(bool)))
            {
                throw new InvalidOperationException("Invalid Type Specified. CoalescingContainers only support 'long', 'int', 'double', 'string', and 'bool'");
            }
        }

        /// <summary>
        /// Gets the value of the provided key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetValue<T>(string name)
        {
            CheckType<T>();

            foreach (var container in _enumerator())
            {
                T t;
                if (container.TryGetValue(name, out t))
                    return t;
            }

            return default(T);
        }

        /// <summary>
        /// Sets the value of the key on the local container.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetLocalValue<T>(string name, T value)
        {
            CheckType<T>();
            _primary[name] = value;
        }

        public bool TryGetValue<T>(string name, out T value)
        {
            value = GetValue<T>(name);
            return true;
        }

        public bool ContainsKey(string key)
        {
            return _enumerator().Any(x => x.ContainsKey(key));
        }
    }

    public interface IDictionaryStore
    {
        bool TryGetValue<T>(string name, out T value);

        bool ContainsKey(string key);
    }
}