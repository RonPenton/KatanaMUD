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
            race.Stats.Strength = MinStr;
            race.Stats.Agility = MinAgl;
            race.Stats.Health = MinHea;
            race.Stats.Intellect = MinInt;
            race.Stats.Willpower = MinWil;
            race.Stats.Charm = MinChm;
            race.Stats.StrengthCap = MaxStr;
            race.Stats.AgilityCap = MaxAgl;
            race.Stats.HealthCap = MaxHea;
            race.Stats.IntellectCap = MaxInt;
            race.Stats.WillpowerCap = MaxWil;
            race.Stats.CharmCap = MaxChm;
            race.Stats.HpMin = HPBonus;

            return race;
        }
    }
}