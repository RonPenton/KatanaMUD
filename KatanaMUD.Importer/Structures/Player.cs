using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KatanaMUD.Importer.Structures
{
    public struct PlayerBuffer
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        public char[] BBSName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public char[] FirstName;
        public byte AfterFirstName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
        public char[] LastName;
        public byte AfterLastName;
        public int NotExperience;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public short[] SpellCasted;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public short[] SpellValue;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public short[] SpellRoundsLeft;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public char[] Title;
        public short Race;
        public short Class;
        public short Level;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public short[] Stat;
        public short MaxHP;
        public short CurrentHP;
        public short MaxENC;
        public short CurrentENC;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] Energy;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public short[] unknown1;
        public short MagicRes;
        public short MagicRes2;
        public int MapNumber;
        public int RoomNum;
        public short nothing2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public short[] unknown2;
        public short nothing3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] unknown3;
        public short nothing4;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public int[] Item;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public short[] ItemUses;
        public int nothing5;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
        public int[] Key;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
        public short[] KeyUses;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public int[] unknown4;
        public int BillionsOfExperience;
        public int MillionsOfExperience;
        public short Nothing6;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public short[] Spell;
        public short EvilPoints;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public int[] nothing7;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public int[] LastMap;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public int[] LastRoom;
        public short nothing8;
        public short BroadcastChan;
        public int unknown5;
        public short Perception;
        public short Stealth;
        public short MartialArts;
        public short Thievery;
        public short MaxMana;
        public short CurrentMana;
        public short SpellCasting;
        public short Traps;
        public short unknown6;
        public short Picklocks;
        public short Tracking;
        public short nothing9;
        public int Runic;
        public int Platinum;
        public int Gold;
        public int Silver;
        public int Copper;
        public int WeaponHand;
        public int nothing10;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public int[] WornItem;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public short[] unknown7;
        public short unknown8;
        public short LivesRemaining;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public short[] unknown9;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
        public char[] GangName;
        public byte AfterGangName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] unknown11;
        public short CPRemaining;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public char[] SuicidePassword;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public short[] unknown12a;
        public byte bEDITED;
        public byte unknown12c;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        public short[] unknown12d;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        public short[] Ability;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        public short[] AbilityModifier;
        public short unknown13a;
        public short unknown13b;
        public short unknown13c;
        public short unknown13d;
        public short unknown13e;
        public short unknown13f;
        public short unknown13g;
        public int CharLife;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public short[] unknown13;
        public byte Bitmask1;
        public byte Bitmask2;
        public byte TestFlag1;
        public byte TestFlag2;
        public short unknown14;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public int[] unknown15;
    }
}