using KatanaMUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 71)]
        public char[] RoomDescription1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 71)]
        public char[] RoomDescription2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 71)]
        public char[] RoomDescription3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 71)]
        public char[] RoomDescription4;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 71)]
        public char[] RoomDescription5;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 71)]
        public char[] RoomDescription6;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 71)]
        public char[] RoomDescription7;
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

        public override string ToString()
        {
            return String.Format("({0},{1}) {2}", MapNumber, RoomNumber, new String(Name).Trim());
        }

        public string Description
        {
            get
            {
                Regex r = new Regex("\\s+");
                return r.Replace((Clean(RoomDescription1) +
                    Clean(RoomDescription2) +
                    Clean(RoomDescription3) +
                    Clean(RoomDescription4) +
                    Clean(RoomDescription5) +
                    Clean(RoomDescription6) +
                    Clean(RoomDescription7)).Trim(), " ");
            }
        }

        private string Clean(char[] input)
        {
            return new String(input.TakeWhile(x => x != '\0').ToArray()) + " ";
        }

        public Room ToRoom(Room room)
        {
            if (room == null)
            {
                room = new Room();
            }

            room.Id = GetRoomNumber(MapNumber, RoomNumber);
            room.Name = new string(Name.TakeWhile(x => x != '\0').ToArray()).Trim();

            SetRoom(0, room, (x, y) => x.NorthExit = y);
            SetRoom(1, room, (x, y) => x.SouthExit = y);
            SetRoom(2, room, (x, y) => x.EastExit = y);
            SetRoom(3, room, (x, y) => x.WestExit = y);
            SetRoom(4, room, (x, y) => x.NorthEastExit = y);
            SetRoom(5, room, (x, y) => x.NorthWestExit = y);
            SetRoom(6, room, (x, y) => x.SouthEastExit = y);
            SetRoom(7, room, (x, y) => x.SouthWestExit = y);
            SetRoom(8, room, (x, y) => x.UpExit = y);
            SetRoom(9, room, (x, y) => x.DownExit = y);

            return room;
        }

        private void SetRoom(int exitIndex, Room room, Action<Room, int> setter)
        {
            if (this.RoomTypes[exitIndex] == 0 && this.RoomExit[exitIndex] != 0)
            {
                setter(room, GetRoomNumber(MapNumber, RoomExit[exitIndex]));
            }
            if (this.RoomTypes[exitIndex] == 8 && this.RoomExit[exitIndex] != 0)
            {
                setter(room, GetRoomNumber(this.Para1[exitIndex], this.RoomExit[exitIndex]));
            }

            var et = this.RoomTypes[exitIndex];

            if ((et == 19 || et == 1 || et == 2 || et == 3 || et == 4 || et == 5 || et == 6 || et == 7 || et == 9 || et == 10 || et == 11 || et == 13 || et == 14 || et == 15 || et == 16 || et == 17 || et == 18 || et == 20 || et == 21 || et == 22 || et == 23 || et == 24) && this.RoomExit[exitIndex] != 0)
            {
                setter(room, GetRoomNumber(MapNumber, this.RoomExit[exitIndex]));
            }
        }


        public static int GetRoomNumber(int mapNumber, int roomNumber)
        {
            return mapNumber * 100000 + roomNumber;
        }
    }
}