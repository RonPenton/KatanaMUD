using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KatanaMUD.Importer.Structures
{
	public struct SpellBuffer
	{
		public short Number { get; set; }
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 29)]
		public char[] Name;
		public byte AfterName;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public char[] DescA;
		public byte AfterDescA;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public char[] DescB;
		public byte AfterDescB;
		public short N01;
		public int CastMsgA;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
		public short[] N02;
		public byte LevelCap;
		public byte N03;
		public byte MsgStyle;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public byte[] N04;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public short[] AbilityB;
		public short Energy;
		public short Level;
		public short Min;
		public short Max;
		public short SpellAttackType;
		public short TypeOfResists;
		public short Difficulty;
		public short UNDEFINED01;
		public short Target;
		public short duration;
		public short TypeOfAttack;
		public short UNDEFINED02;
		public short ResistAbility;
		public short MageryA;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public short[] AbilityA;
		public int CastMsgB;
		public short Mana;
		public byte MaxIncrease;
		public byte LVLSMaxIncr;
		public short MageryB;
		public byte MinIncrease;
		public byte LVLSMinIncr;
		public byte DurIncrease;
		public byte LVLSDurIncr;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
		public char[] ShortName;
		public byte AfterShortName;
		public int N06;

	}
}
