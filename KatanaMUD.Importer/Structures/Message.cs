using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KatanaMUD.Importer.Structures
{
	public struct MessageBuffer
	{
		public int Number;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 74)]
		public char[] MessageLine1;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public byte[] Padding00;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 74)]
		public char[] MessageLine2;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public byte[] Padding01;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 74)]
		public char[] MessageLine3;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
		public byte[] Padding02;

	}
}
