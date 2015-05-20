using Spam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KatanaMUD.Models
{
    public partial class Room
    {
        public T GetStat<T>(string name, T baseValue, bool includePercent = true)
        {
            T t;
            if (Stats.TryGetValue(name, out t))
                return t;
            return baseValue;
        }

        public long Illumination
        {
            get
            {
                return GetStat<long>("Illumination", 0)
                    + Actors.Sum(x => x.Illumination)
                    + Items.Sum(x => x.Illumination);
            }
        }
    }
}
