using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KatanaMUD.Importer.Structures
{
	public struct ShopBuffer
	{
		public int Number;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 39)]
		public char[] Name;
		public short ShopAfterName;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 52)]
		public char[] ShopDescriptionA;
		public byte ShopNothing1;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 52)]
		public char[] ShopDescriptionB;
		public byte ShopNothing2;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 52)]
		public char[] ShopDescriptionC;
		public byte ShopNothing3;
		public short ShopIndicator;
		public short ShopMinLvL;
		public short ShopMaxLvl;
		public short ShopMarkUp;
		public short ShopNothing4;
		public byte ShopClassLimit;
		public byte ShopNothingAA;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
		public int[] ShopItemNumber;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
		public short[] ShopMax;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
		public short[] ShopNow;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
		public short[] ShopRgnTime;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
		public short[] ShopRgnNumber;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
		public byte[] ShopRgnPercentage;
	}
}
