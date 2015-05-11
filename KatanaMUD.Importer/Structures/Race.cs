using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using KatanaMUD.Models;

namespace KatanaMUD.Importer.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct RaceBuffer
    {
        public short Number;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 29)]
        char[] Name;
        byte nothing1;
        short MinInt;
        short MinWil;
        short MinStr;
        short MinHea;
        short MinAgl;
        short MinChm;
        short HPBonus;
        int nothing2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        short[] AbilityA;
        short CP;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        short[] AbilityB;
        int nothing3;
        short nothing4;
        short ExpChart;
        short nothing5;
        short MaxInt;
        short MaxWil;
        short MaxStr;
        short MaxHea;
        short MaxAgl;
        short MaxChm;
        int Nothing6;
        int nothing7;
        int nothing8;

        public RaceTemplate ToRace(RaceTemplate race)
        {
            if (race == null)
            {
                race = new RaceTemplate();
            }

            race.Id = Number;
            race.Name = new string(Name).Replace("\0", "").Trim();

            race.Stats.Set("Strength", (long)MinStr);
            race.Stats.Set("Agility", (long)MinAgl);
            race.Stats.Set("Health", (long)MinHea);
            race.Stats.Set("Intellect", (long)MinInt);
            race.Stats.Set("Willpower", (long)MinWil);
            race.Stats.Set("Charm", (long)MinStr);
            race.Stats.Set("StrengthCap", (long)MinChm);
            race.Stats.Set("AgilityCap", (long)MaxAgl);
            race.Stats.Set("HealthCap", (long)MaxHea);
            race.Stats.Set("IntellectCap", (long)MaxInt);
            race.Stats.Set("WillpowerCap", (long)MaxWil);
            race.Stats.Set("CharmCap", (long)MaxChm);
            race.Stats.Set("HpMin", (long)HPBonus);

            return race;
        }
    }
}