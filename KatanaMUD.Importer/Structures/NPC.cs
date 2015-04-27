using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KatanaMUD.Importer.Structures
{
    public struct NPCBuffer
    {
        public int Number;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
        public char[] Nothing55;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 29)]
        public char[] Name;
        public byte nothing1;
        public short GroupID;
        public short nothingXX1;
        public int ExpMulti;
        public short Index;
        public short nothingXX3;
        public int Something2;
        public int WeaponNumber;
        public short DR;
        public short AC;
        public short Something3;
        public short Follow;
        public short MR;
        public short BSDefence;
        public int Experience;
        public short Hitpoints;
        public short Energy;
        public short HPRegen;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public short[] AbilityA;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public short[] AbilityB;
        public short GameLimit;
        public short Active;
        public short Type;
        public byte nothing2;
        public byte Undead;
        public short Alignment;
        public short nothing3;
        public short RegenTime;
        public short DateKilled;
        public short TimeKilled;
        public int MoveMsg;
        public int DeathMsg;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public int[] ItemNumber;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public short[] ItemUses;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] ItemDropPer;
        public short nothing9;
        public int Runic;
        public int Platinum;
        public int Gold;
        public int Silver;
        public int Copper;
        public int GreetTxt;
        public short CharmLvL;
        public short Nothing16;
        public int DescTxt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] AttackType;
        public byte Nothing22;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public short[] AttackAccuSpell;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] AttackPer;
        public byte Nothing17;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public short[] AttackMinHCastPer;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public short[] AttackMaxHCastLvl;
        public short Nothing18;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public int[] AttackHitMsg;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public int[] AttackDodgeMsg;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public int[] AttackMissMsg;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public short[] AttackEnergy;
        public short Nothing19;
        public int TalkTxt;
        public short CharmRes;
        public short Nothing21;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public short[] AttackHitSpell;
        public short DeathSpellNumber;
        public short Nothing23;
        public short Nothing24;
        public short Nothing25;
        public short Nothing26;
        public short Nothing27;
        public short Nothing28;
        public short Nothing29;
        public short CreateSpellNumber;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public short[] SpellNumber;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] SpellCastPer;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] SpellCastLvl;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 70)]
        public char[] DescLine1;
        public byte nothing10;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 70)]
        public char[] DescLine2;
        public byte Nothing11;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 70)]
        public char[] DescLine3;
        public byte Nothing12;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 70)]
        public char[] DescLine4;
        public byte Nothing13;
        public byte Gender;
        public byte Nothing14;
        public short Nothing15;
    }
}