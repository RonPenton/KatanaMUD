using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KatanaMUD.Importer.Structures
{
    public struct GangBuffer
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public char[] KeyName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public char[] DisplayName;
        public int Exp;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        public char[] Leader;
        public short DateCreated;
        public short unknown1;
        public short Members;
        public int unknown2;
        public int unknown3;
        public int RollOver;
        public int RollTimes;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 160)]
        public char[] unknown5;
    }
}