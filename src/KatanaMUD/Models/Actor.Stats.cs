using System;
using System.Collections.Generic;
using Spam;
using System.Linq;

namespace KatanaMUD.Models
{
    public partial class Actor
    {
        /*
            Values taken from: http://www.mudinfo.net/viewtopic.php?f=4&t=1647

            NOTE: If values are different, then they may have been tweaked from the original formula for balance changes.

            Ability: For every 10 pts added:

            * Strength: 480 Encumberance, +1 Damage
            * Agility: +2.5 A/C*, +1 Accuracy, +2.5 Stealth
            * Intellect: +1 Critical Hit, +6 Perception, +1 Stealth, +1.5 Thievery, +2 Traps, +3 Picklocks, +2 Tracking, +2 M/R, +5 S/C (Mage), +3.5 S/C (Druid), +1.6 S/C (Priest)
            * Willpower: +2 Perception, +1 Tracking, +7 M/R, +2 S/C (Mage), +3.5 S/C (Druid), +5 S/C (Priest), +2 S/C (Bard)
            * Health: +4 Hit Points
            * Charm: +1 Perception, +2.5 Stealth, +1.5 Thievery, +3 Traps, +1 Tracking, +5 S/C (Bard), +1 Critical Hit, +1.2 Accuracy, +1 Dodge
        */


        public long MaxEncumbrance
        {
            get
            {
                var strength = Stats.GetCalculatedValue<long>("Strength");
                return Stats.GetCalculatedValue<long>("MaxEncumbrance") + (strength * 50);
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
                                ((double)Stats.GetCalculatedValue<long>("Intellect") * (9.0 / 14.0)) +
                                ((double)Stats.GetCalculatedValue<long>("Willpower") * (1.0 / 4.0)) +
                                ((double)Stats.GetCalculatedValue<long>("Charm") * (1.0 / 8.0)));
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

        public long NightVision => Stats.GetCalculatedValue<long>("NightVision");

        // TODO: Get illumination from buffs and equipped light sources.
        public long Illumination => Stats.GetCalculatedValue<long>("Illumination");
    }
}