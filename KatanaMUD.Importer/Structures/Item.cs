using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KatanaMUD.Importer.Structures
{
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
	[Serializable]
	public struct ItemBuffer
	{
		public int Number;
		public short Ignore1;
		public short GameLimit;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public short[] Ignore2;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 157)]
		public char[] Ignore;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 29)]
		public char[] Name;
		public byte Ignore3;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
		public char[] DescriptionOne;
		public byte Ignore4;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
		public char[] DescriptionTwo;
		public byte Ignore5;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
		public char[] DescriptionThree;
		public byte Ignore6;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
		public char[] DescriptionFour;
		public byte Ignore7;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
		public char[] DescriptionFive;
		public byte Ignore8;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
		public char[] DescriptionSix;
		public byte Ignore9;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
		public char[] DescriptionSeven;
		public byte Ignore10;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
		public char[] DescriptionEight;
		public byte Ignore11;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
		public char[] DescriptionNine;
		public byte Ignore12;
		public short Ignore13;
		public short Weight;
		public short Type;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
		public short[] AbilityNumber;
		public short Uses;
		public short Ignore14;
		public short Cost;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public short[] Class;
		public short Ignore15;
		public short Ignore16;
		public short Ignore17;
		public short MinHit;
		public short MaxHit;
		public short AC;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public short[] Race;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public short[] Ignore18;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
		public short[] Negate;
		public short Weapon;
		public short Armour;
		public short WornOn;
		public short Accuracy;
		public short DamageResist;
		public byte Gettable;
		public byte Ignore19;
		public short RequiredStr;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
		public short[] Ignore20;
		public int OpenRunic;
		public int OpenPlatinum;
		public int OpenGold;
		public int OpenSilver;
		public int OpenCopper;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
		public short[] Ignore21;
		public short Speed;
		public short Ignore22;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
		public short[] AbilityValue;
		public short Ignore23;
		public int HitMsg;
		public int MissMsg;
		public int ReadTB;
		public int DestructMsg;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
		public short[] Ignore24;
		public byte NotDroppable;
		public byte CostType;
		public byte RetainAfterUses;
		public byte Robable;
		public byte DestroyOnDeath;
		public short Ignore25;


        public override string ToString()
        {
            return String.Format("{0} ({1})", new string(Name.TakeWhile(x => x != '\0').ToArray()), Number);
        }


        public const int RecordSize = 1072;
	}
}
