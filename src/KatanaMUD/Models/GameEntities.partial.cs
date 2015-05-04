using System;
using System.Collections.Generic;
using System.Linq;

namespace KatanaMUD.Models
{
    public partial class GameEntities
    {
        public bool ForceSave { get; set; }

        public IEnumerable<Currency> AllCurrencies => Currencies.OrderByDescending(x => x.Value);
    }
}