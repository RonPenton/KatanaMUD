using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KatanaMUD.Importer.Structures
{
    public struct TextBlockBuffer
    {
        public short PartNum;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
        public byte[] LeadIn;
        public int Number;
        public int LinkTo;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2000)]
        public char[] Data;



        public static string Decode(string data)
        {
            List<char> blockData = new List<char>();

            for (int i = 0; i < data.Length; i++)
            {
                int value = (int)data[i];

                if (value >= 32)
                {
                    blockData.Add((char)(value - 32));
                }
            }


            return new string(blockData.ToArray());
        }

        public static string DecryptTextblock(object sData)
        {
            Encoding cp437 = Encoding.GetEncoding(437);

            List<byte> sDecrypted = new List<byte>();
            char[] xData = (char[])sData;

            char sChar = '0';

            for (int x = 0; x < xData.Length; x++)
            {
                sChar = xData[x];

                if (sChar >= 32)
                {
                    if ((int)sChar > 0xFF)
                    {
                        sDecrypted.Add((byte)(ASCIILookup.GetDecode((int)sChar) - 32));
                    }
                    else
                    {
                        sDecrypted.Add((byte)(sChar - 32));
                    }
                }
            }

            return Encoding.ASCII.GetString(sDecrypted.ToArray()).Replace("'", "''");
        }
    }

    public static class ASCIILookup
    {
        private static Dictionary<int, byte> _mapTiny = new Dictionary<int, byte>();
        private static Dictionary<byte, string> _letters = new Dictionary<byte, string>();

        static ASCIILookup()
        {
            _mapTiny.Add(8364, 128);
            _mapTiny.Add(8218, 130);
            _mapTiny.Add(402, 131);
            _mapTiny.Add(8222, 132);
            _mapTiny.Add(8230, 133);
            _mapTiny.Add(8224, 134);
            _mapTiny.Add(8225, 135);
            _mapTiny.Add(710, 136);
            _mapTiny.Add(8240, 137);
            _mapTiny.Add(352, 138);
            _mapTiny.Add(8249, 139);
            _mapTiny.Add(338, 140);
            _mapTiny.Add(381, 142);
            _mapTiny.Add(8216, 145);
            _mapTiny.Add(8217, 146);
            _mapTiny.Add(8220, 147);
            _mapTiny.Add(8221, 148);
            _mapTiny.Add(8226, 149);
            _mapTiny.Add(8211, 150);
            _mapTiny.Add(8212, 151);
            _mapTiny.Add(732, 152);
            _mapTiny.Add(8482, 153);
            _mapTiny.Add(353, 154);
            _mapTiny.Add(8250, 155);
            _mapTiny.Add(339, 156);
            _mapTiny.Add(382, 158);
            _mapTiny.Add(376, 159);
        }

        public static byte GetDecode(int value)
        {
            return _mapTiny[value];
        }
    }
}