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
            containers.AddRange(EquippedItems.Select(x => (JsonContainer)x.Stats));
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

        public long Encumbrance
        {
            get
            {
                long enc = Items.Sum(x => x.Weight);
                enc += Game.Data.Currencies.Select(x => (long)(x.Weight * (long)Currency.Get(x, this.Cash))).Sum();
                return enc;
            }
        }

        /// <summary>
        /// The bese perception value for the player, not including Room Light. 
        /// </summary>
        public long PerceptionBase
        {
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

        /// <summary>
        /// Perception, factoring room light. 
        /// TODO: Determine if this is really the formula we want to go with.
        /// </summary>
        public long PerceptionFinal
        {
            get
            {
                // Illumination counts as half a percent. So -200 illumination (very dark, cannot see) means that a person with 100 perception cannot see anything. 
                // Night Vision counts directly against illumination. So -200 illumination and +100 NV means you have an effective -100 illumination.
                return PerceptionBase + ((Room.Illumination + NightVision) / 2);
            }
        }

        public long NightVision => GetStat<long>("NightVision", 0, true);

        // TODO: Get illumination from buffs and equipped light sources.
        public long Illumination => GetStat<long>("Illumination", 0);
    }
}