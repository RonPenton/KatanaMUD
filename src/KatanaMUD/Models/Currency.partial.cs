using System;
using Spam;

namespace KatanaMUD.Models
{
    public partial class Currency
    {
        public static long Get(Currency currency, dynamic collection)
        {
            var container = collection as JsonContainer;
            if (container == null)
                throw new InvalidOperationException("Not a JsonContainer");


            object value;
            container.GetValue(currency.ShortName, out value);
            return Convert.ToInt64(value);
        }

        public static void Add(Currency currency, dynamic collection, long amount)
        {
            var value = Get(currency, collection);
            Set(currency, collection, amount + value);
        }

        public static void Set(Currency currency, dynamic collection, long amount)
        {
            var container = collection as JsonContainer;
            if (container == null)
                throw new InvalidOperationException("Not a JsonContainer");


            container.SetValue(currency.ShortName, amount);
        }
    }
}