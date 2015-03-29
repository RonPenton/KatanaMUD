using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KatanaMUD.Importer.Structures
{
	public struct RoomBuffer
	{
		public int MapNumber;
		public int RoomNumber;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 253)]
		public char[] Ignore00;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 53)]
		public char[] Name;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 497)]
		public char[] RoomDescription;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
		public char[] AnsiMap;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public int[] RoomExit;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public short[] RoomTypes;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public int[] Para1;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public short[] Para2;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public int[] Para3;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public int[] Para4;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
		public int[] CurrentRoomMon;
		public short Type;
		public short NewSpot;
		public int ShopNum;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
		public short[] nothing1;
		public short MinIndex;
		public short MaxIndex;
		public int ByNumber;
		public short Light;
		public short GangHouseNumber;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
		public int[] RoomItems;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
		public short[] RoomItemUses;
		public short nothing4;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
		public int[] InvisItems;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
		public short[] InvisItemUses;
		public short nothing5;
		public int Runic;
		public int Platinum;
		public int Gold;
		public int Silver;
		public int Copper;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
		public int[] nothing7;
		public int MaxRegen;
		public short MonsterType;
		public short unknown69;
		public int Attributes;
		public int nothing9;
		public int DeathRoom;
		public int ExitRoom;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
		public short[] RoomItemQty;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
		public short[] InvisItemQty;
		public int CmdText;
		public int nothing10;
		public short Delay;
		public short MaxArea;
		public int Nothing11;
		public int ControlRoom;
		public int PermNPC;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public int[] PlacedItems;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public int[] Nothing12;
		public int Something1;
		public int Spell;
		public short unknown70;
		public byte NumMons;
		public byte unknown71;
	}
}
