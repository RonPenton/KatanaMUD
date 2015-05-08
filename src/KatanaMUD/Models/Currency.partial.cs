using System;
using Spam;
using System.Linq;

namespace KatanaMUD.Models
{
    public partial class Currency : IItem
    {
        public static long Get(Currency currency, JsonContainer collection)
        {
            object value;
            collection.TryGetValue(currency.ShortName, out value);
            if (value == null)
                return 0;
            return Convert.ToInt64(value);
        }

        public static void Add(Currency currency, JsonContainer collection, long amount)
        {
            var value = Get(currency, collection);
            Set(currency, collection, amount + value);
        }

        public static void Set(Currency currency, JsonContainer collection, long amount)
        {
            collection[currency.ShortName] = amount;
        }


        public static long GetValue(JsonContainer container) => Game.Data.AllCurrencies.Sum(x => Currency.Get(x, container) * x.Value);

        public static void Clear(JsonContainer container) => Game.Data.AllCurrencies.ForEach(x => Currency.Set(x, container, 0));

        /// <summary>
        /// "Minifies" a container of money, making it so that it takes up the smallest amount of coinage. 
        /// </summary>
        /// <param name="container"></param>
        public static void Minify(JsonContainer container)
        {
            var value = Convert.ToDecimal(GetValue(container));
            Clear(container);

            foreach (var currency in Game.Data.AllCurrencies)
            {
                var coins = Math.Floor(value / Convert.ToDecimal(currency.Value));
                Set(currency, container, Convert.ToInt64(coins));
                value -= (coins * currency.Value);
            }
        }
    }
}