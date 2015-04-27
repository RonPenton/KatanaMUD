using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KatanaMUD.Importer.Structures
{
    public struct ActionBuffer
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 29)]
        public char[] Name;
        public byte Ignore11;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 74)]
        public char[] SingleToUser;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] Ignore00;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 74)]
        public char[] SingleToRoom;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] Ignore01;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 74)]
        public char[] UserToUser;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] Ignore02;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 74)]
        public char[] UserToOtherUser;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] Ignore03;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 74)]
        public char[] UserToRoom;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] Ignore04;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 74)]
        public char[] MonsterToUser;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] Ignore05;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 74)]
        public char[] MonsterToRoom;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] Ignore06;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 74)]
        public char[] InventoryToUser;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] Ignore07;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 74)]
        public char[] InventoryToRoom;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] Ignore08;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 74)]
        public char[] FloorItemToUser;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] Ignore09;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 74)]
        public char[] FloorItemToRoom;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] Ignore10;
        public byte IgnoreOffset;
    }
}