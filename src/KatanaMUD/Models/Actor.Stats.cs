using System;
using System.Collections.Generic;
using Spam;
using System.Linq;

namespace KatanaMUD.Models
{
    public partial class Actor
    {
        public T GetStat<T>(string name, T baseValue, bool includePercent = true)
        {
            List<JsonContainer> containers = new List<JsonContainer>();

            containers.Add((JsonContainer)Stats);
            containers.AddRange(Items.Select(x => (JsonContainer)x.Stats));
            //TODO: Buffs here

            return JsonContainer.Calculate<T>(containers, name, baseValue, includePercent);
        }

        public long MaxEncumbrance
        {
            get
            {
                var strength = GetStat<long>("Strength", 0);
                return GetStat<long>("MaxEncumbrance", strength * 48);
            }
        }

        public long Encumbrance => Items.Sum(x => x.Weight);


        public long Perception {
            get
            {
                // This is the best approximation I've found for MajorMUD. If anyone knows a better formula, feel free to update it.'
                // To be honest, we may just end up rebalancing this anyway.
                return (long)Math.Floor(
                                ((double)GetStat<long>("Intellect", 0) * (9.0 / 14.0)) +
                                ((double)GetStat<long>("Willpower", 0) * (1.0 / 4.0)) +
                                ((double)GetStat<long>("Charm", 0) * (1.0 / 8.0)));
            }
        }
    }
}