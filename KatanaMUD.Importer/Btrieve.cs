using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using KatanaMUD.Importer.Structures;
using KatanaMUD.Models;

namespace KatanaMUD.Importer
{
	public static class Btrieve
	{
        // ***************************************************************************************************************************
        [DllImport("WBTRV32.dll", CharSet = CharSet.Ansi)]
		public static extern short BTRCALL(ushort operation,
			[MarshalAs(UnmanagedType.LPArray, SizeConst = 128)] byte[] posBlk,
			[MarshalAs(UnmanagedType.Struct, SizeConst = 255)] ref RaceBuffer databuffer,
			ref int dataLength,
			[MarshalAs(UnmanagedType.LPArray, SizeConst = 255)] char[] keyBffer,
			ushort keyLength, ushort keyNum);


		public static List<RaceTemplate> GetAllRaces(string fileName, IEnumerable<RaceTemplate> races)
		{
			var dataBuffer = new RaceBuffer();
			var list = new List<RaceBuffer>();
			Func<RaceBuffer> newFunc = () => new RaceBuffer();
			Func<RaceBuffer, RaceTemplate> convert = x => x.ToRace(races.SingleOrDefault(y => y.Id == x.Number));

			// Yes, yes. Copied Code. How horrible. There's really no good alternative to DllImport-interfacing code though. Dynamic doesn't work,
			// templates don't work, etc. So. Given that the MajorMUD database format will never change in the future (it's been dead 10 years!),
			// I'm ok with copied code. Huzzah.
			byte[] posBlock = new byte[128];
			char[] keyBuffer = fileName.ToCharArray(); //@"E:\PROJECTS\DEVELOPMENT\00-Current\KatanaMUD\KatanaMUD.Importer\bin\Debug\dats\wccrace2.dat".ToCharArray();
			int bufferLength = System.Runtime.InteropServices.Marshal.SizeOf(dataBuffer);

			int status = Btrieve.BTRCALL(Btrieve.BOPEN, posBlock, ref dataBuffer, ref bufferLength, keyBuffer, (ushort)keyBuffer.Length, 0);

			if (status == 0)
			{
				status = Btrieve.BTRCALL(Btrieve.BGETFIRST, posBlock, ref dataBuffer, ref bufferLength, keyBuffer, (ushort)keyBuffer.Length, 0);
				if (status == 0)
					list.Add(dataBuffer);
				else
					throw new InvalidOperationException(ErrorCode(status));

				// Get subsequent records
				while (status == 0)	// BReturnCodes.END_OF_FILE or an error will occur
				{
					dataBuffer = newFunc();
					status = Btrieve.BTRCALL(Btrieve.BGETNEXT, posBlock, ref dataBuffer, ref bufferLength, keyBuffer, (ushort)keyBuffer.Length, 0);

					if (status == 0)
						list.Add(dataBuffer);
					else if (status != 9)
						throw new InvalidOperationException(ErrorCode(status));
				}
			}
			else
			{
				throw new InvalidOperationException(ErrorCode(status));
			}

			return list.Select(x => convert(x)).ToList();
		}

        // ***************************************************************************************************************************
		[DllImport("WBTRV32.dll", CharSet = CharSet.Ansi)]
		public static extern short BTRCALL(ushort operation,
			[MarshalAs(UnmanagedType.LPArray, SizeConst = 128)] byte[] posBlk,
			[MarshalAs(UnmanagedType.Struct, SizeConst = 255)] ref ClassBuffer databuffer,
			ref int dataLength,
			[MarshalAs(UnmanagedType.LPArray, SizeConst = 255)] char[] keyBffer,
			ushort keyLength, ushort keyNum);
		public static List<ClassTemplate> GetAllClasses(string fileName, IEnumerable<ClassTemplate> classes)
		{
			var dataBuffer = new ClassBuffer();
			var list = new List<ClassBuffer>();
			Func<ClassBuffer> newFunc = () => new ClassBuffer();
			Func<ClassBuffer, ClassTemplate> convert = x => x.ToClass(classes.SingleOrDefault(y => y.Id == x.Number));

			// Yes, yes. Copied Code. How horrible. There's really no good alternative to DllImport-interfacing code though. Dynamic doesn't work,
			// templates don't work, etc. So. Given that the MajorMUD database format will never change in the future (it's been dead 10 years!),
			// I'm ok with copied code. Huzzah.
			byte[] posBlock = new byte[128];
			char[] keyBuffer = fileName.ToCharArray(); //@"E:\PROJECTS\DEVELOPMENT\00-Current\KatanaMUD\KatanaMUD.Importer\bin\Debug\dats\wccrace2.dat".ToCharArray();
			int bufferLength = 156;// System.Runtime.InteropServices.Marshal.SizeOf(dataBuffer);

			int status = Btrieve.BTRCALL(Btrieve.BOPEN, posBlock, ref dataBuffer, ref bufferLength, keyBuffer, (ushort)keyBuffer.Length, 0);

			if (status == 0)
			{
				status = Btrieve.BTRCALL(Btrieve.BGETFIRST, posBlock, ref dataBuffer, ref bufferLength, keyBuffer, (ushort)keyBuffer.Length, 0);
				if (status == 0)
					list.Add(dataBuffer);
				else
					throw new InvalidOperationException(ErrorCode(status));

				// Get subsequent records
				while (status == 0)	// BReturnCodes.END_OF_FILE or an error will occur
				{
					dataBuffer = newFunc();
					status = Btrieve.BTRCALL(Btrieve.BGETNEXT, posBlock, ref dataBuffer, ref bufferLength, keyBuffer, (ushort)keyBuffer.Length, 0);

					if (status == 0)
						list.Add(dataBuffer);
					else if (status != 9)
						throw new InvalidOperationException(ErrorCode(status));
				}
			}
			else
			{
				throw new InvalidOperationException(ErrorCode(status));
			}

			return list.Select(x => convert(x)).ToList();
		}

        // ***************************************************************************************************************************
        [DllImport("WBTRV32.dll", CharSet = CharSet.Ansi)]
        public static extern short BTRCALL(ushort operation,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 128)] byte[] posBlk,
            [MarshalAs(UnmanagedType.Struct, SizeConst = 255)] ref RoomBuffer databuffer,
            ref int dataLength,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 255)] char[] keyBffer,
            ushort keyLength, ushort keyNum);
        public static List<RoomBuffer> GetAllRooms(string fileName)
        {
            var dataBuffer = new RoomBuffer();
            var list = new List<RoomBuffer>();
            Func<RoomBuffer> newFunc = () => new RoomBuffer();

            // Yes, yes. Copied Code. How horrible. There's really no good alternative to DllImport-interfacing code though. Dynamic doesn't work,
            // templates don't work, etc. So. Given that the MajorMUD database format will never change in the future (it's been dead 10 years!),
            // I'm ok with copied code. Huzzah.
            byte[] posBlock = new byte[128];
            char[] keyBuffer = fileName.ToCharArray();
            int bufferLength = 1544; //System.Runtime.InteropServices.Marshal.SizeOf(dataBuffer);

            int status = Btrieve.BTRCALL(Btrieve.BOPEN, posBlock, ref dataBuffer, ref bufferLength, keyBuffer, (ushort)keyBuffer.Length, 0);

            if (status == 0)
            {
                status = Btrieve.BTRCALL(Btrieve.BGETFIRST, posBlock, ref dataBuffer, ref bufferLength, keyBuffer, (ushort)keyBuffer.Length, 0);
                if (status == 0)
                    list.Add(dataBuffer);
                else
                    throw new InvalidOperationException(ErrorCode(status));

                // Get subsequent records
                while (status == 0) // BReturnCodes.END_OF_FILE or an error will occur
                {
                    dataBuffer = newFunc();
                    status = Btrieve.BTRCALL(Btrieve.BGETNEXT, posBlock, ref dataBuffer, ref bufferLength, keyBuffer, (ushort)keyBuffer.Length, 0);

                    if (status == 0)
                        list.Add(dataBuffer);
                    else if (status != 9)
                        throw new InvalidOperationException(ErrorCode(status));
                }
            }
            else
            {
                throw new InvalidOperationException(ErrorCode(status));
            }

            return list;
        }

        // ***************************************************************************************************************************
        [DllImport("WBTRV32.dll", CharSet = CharSet.Ansi)]
        public static extern short BTRCALL(ushort operation,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 128)] byte[] posBlk,
            [MarshalAs(UnmanagedType.Struct, SizeConst = 255)] ref ItemBuffer databuffer,
            ref int dataLength,
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 255)] char[] keyBffer,
            ushort keyLength, ushort keyNum);


        public static List<ItemBuffer> GetAllItems(string fileName)
        {
            var dataBuffer = new ItemBuffer();
            var list = new List<ItemBuffer>();
            Func<ItemBuffer> newFunc = () => new ItemBuffer();

            // Yes, yes. Copied Code. How horrible. There's really no good alternative to DllImport-interfacing code though. Dynamic doesn't work,
            // templates don't work, etc. So. Given that the MajorMUD database format will never change in the future (it's been dead 10 years!),
            // I'm ok with copied code. Huzzah.
            byte[] posBlock = new byte[128];
            char[] keyBuffer = fileName.ToCharArray(); //@"E:\PROJECTS\DEVELOPMENT\00-Current\KatanaMUD\KatanaMUD.Importer\bin\Debug\dats\wccrace2.dat".ToCharArray();
            int bufferLength = ItemBuffer.RecordSize;

            int status = Btrieve.BTRCALL(Btrieve.BOPEN, posBlock, ref dataBuffer, ref bufferLength, keyBuffer, (ushort)keyBuffer.Length, 0);

            if (status == 0)
            {
                status = Btrieve.BTRCALL(Btrieve.BGETFIRST, posBlock, ref dataBuffer, ref bufferLength, keyBuffer, (ushort)keyBuffer.Length, 0);
                if (status == 0)
                    list.Add(dataBuffer);
                else
                    throw new InvalidOperationException(ErrorCode(status));

                // Get subsequent records
                while (status == 0) // BReturnCodes.END_OF_FILE or an error will occur
                {
                    dataBuffer = newFunc();
                    status = Btrieve.BTRCALL(Btrieve.BGETNEXT, posBlock, ref dataBuffer, ref bufferLength, keyBuffer, (ushort)keyBuffer.Length, 0);

                    if (status == 0)
                        list.Add(dataBuffer);
                    else if (status != 9)
                        throw new InvalidOperationException(ErrorCode(status));
                }
            }
            else
            {
                throw new InvalidOperationException(ErrorCode(status));
            }

            return list;
        }


        // ***************************************************************************************************************************
        public static string ErrorCode(int nStatus)
		{
			string BtrieveErrorCode = null;
			switch (nStatus)
			{
				case 1:
					BtrieveErrorCode = "Invalid Operation (1)";
					break;
				case 2:
					BtrieveErrorCode = "Disk I/O Error (2)";
					break;
				case 3:
					BtrieveErrorCode = "File Not Open (3)";
					break;
				case 4:
					BtrieveErrorCode = "Record Not Found (4)";
					break;
				case 5:
					BtrieveErrorCode = "Duplicate Record (5)";
					break;
				case 6:
					BtrieveErrorCode = "Invalid Record Number (6)";
					break;
				case 7:
					BtrieveErrorCode = "Different Record Number (7)";
					break;
				case 8:
					BtrieveErrorCode = "Invalid Positioning (8)";
					break;
				case 9:
					BtrieveErrorCode = "End-Of-File (9)";
					break;
				case 10:
					BtrieveErrorCode = "Modifiable Index Value Error (10)";
					break;
				case 11:
					BtrieveErrorCode = "Invalid Location (11)";
					break;
				case 12:
					BtrieveErrorCode = "File Not Found (12)";
					break;
				case 13:
					BtrieveErrorCode = "Extended File Error (13)";
					break;
				case 14:
					BtrieveErrorCode = "Pre-Image Open Error (14)";
					break;
				case 15:
					BtrieveErrorCode = "Pre-Image I/O Error (15)";
					break;
				case 17:
					BtrieveErrorCode = "Close Error (17)";
					break;
				case 18:
					BtrieveErrorCode = "Disk Full (18)";
					break;
				case 19:
					BtrieveErrorCode = "Unrecoverable Error (19)";
					break;
				case 20:
					BtrieveErrorCode = "Record Manager Inactive (20)";
					break;
				case 21:
					BtrieveErrorCode = "Index Buffer Too Short (21)";
					break;
				case 22:
					BtrieveErrorCode = "Data Buffer Length (22)";
					break;
				case 23:
					BtrieveErrorCode = "Position Block Length (23)";
					break;
				case 24:
					BtrieveErrorCode = "Page Size Error (24)";
					break;
				case 25:
					BtrieveErrorCode = "Create I/O Error (25)";
					break;
				case 26:
					BtrieveErrorCode = "Number of Keys (26)";
					break;
				case 27:
					BtrieveErrorCode = "Invalid Key Position (27)";
					break;
				case 28:
					BtrieveErrorCode = "Invalid Record Length (28)";
					break;
				case 29:
					BtrieveErrorCode = "Invalid Record Length (29)";
					break;
				case 30:
					BtrieveErrorCode = "Not A Btrieve File (30)";
					break;
				case 35:
					BtrieveErrorCode = "Directory Error (35), Go to File --> Settings and set your Datfile Path";
					break;
				case 36:
					BtrieveErrorCode = "TransactiOn Error (36)";
					break;
				case 37:
					BtrieveErrorCode = "Transaction Is Active (37)";
					break;
				case 38:
					BtrieveErrorCode = "Transaction Control File I/O Error (38)";
					break;
				case 39:
					BtrieveErrorCode = "End/Abort TransactiOn Error (39)";
					break;
				case 40:
					BtrieveErrorCode = "Transaction Max Files (40)";
					break;
				case 41:
					BtrieveErrorCode = "Operation Not Allowed (41)";
					break;
				case 43:
					BtrieveErrorCode = "Invalid Record Access (43)";
					break;
				case 44:
					BtrieveErrorCode = "Null Index Path (44)";
					break;
				case 46:
					BtrieveErrorCode = "Access To File Denied (46)";
					break;
				case 51:
					BtrieveErrorCode = "Invalid Owner (51)";
					break;
				case 52:
					BtrieveErrorCode = "Error Writing Cache (52)";
					break;
				case 54:
					BtrieveErrorCode = "Variable Page Error During a Step Direct operation (54)";
					break;
				case 55:
					BtrieveErrorCode = "Autoincrement Error (55)";
					break;
				case 58:
					BtrieveErrorCode = "Compression Buffer Too Short (58)";
					break;
				case 66:
					BtrieveErrorCode = "Maximum Number of Open Databases Exceeded (66)";
					break;
				case 67:
					BtrieveErrorCode = "Rl Could Not Open SQL Data Dictionaries (67)";
					break;
				case 68:
					BtrieveErrorCode = "Rl Cascades Too Deeply (68)";
					break;
				case 69:
					BtrieveErrorCode = "Rl Cascade Error (69)";
					break;
				case 71:
					BtrieveErrorCode = "Rl Definitions Violation (71)";
					break;
				case 72:
					BtrieveErrorCode = "Rl Referenced File Could Not Be Opnend (72)";
					break;
				case 73:
					BtrieveErrorCode = "Rl Definition Out Of Sync (73)";
					break;
				case 76:
					BtrieveErrorCode = "Rl Referenced File Conflict (76)";
					break;
				case 77:
					BtrieveErrorCode = "Wait Error (77)";
					break;
				case 78:
					BtrieveErrorCode = "Deadlock Detected (78)";
					break;
				case 79:
					BtrieveErrorCode = "Programming Error (79)";
					break;
				case 80:
					BtrieveErrorCode = "Conflict (80)";
					break;
				case 81:
					BtrieveErrorCode = "Lock Error (81)";
					break;
				case 82:
					BtrieveErrorCode = "Lost Position (82)";
					break;
				case 83:
					BtrieveErrorCode = "Read Outside Transaction (83)";
					break;
				case 84:
					BtrieveErrorCode = "Record In Use (84)";
					break;
				case 85:
					BtrieveErrorCode = "File In Use (85)";
					break;
				case 86:
					BtrieveErrorCode = "File Table Full";
					break;
				case 87:
					BtrieveErrorCode = "Handle Table Full";
					break;
				case 88:
					BtrieveErrorCode = "Incompatible Mode Error";
					break;
				case 90:
					BtrieveErrorCode = "Redirected Device Table Full";
					break;
				case 91:
					BtrieveErrorCode = "Server Error";
					break;
				case 92:
					BtrieveErrorCode = "Transaction Table Full";
					break;
				case 93:
					BtrieveErrorCode = "Incompatible Lock Type";
					break;
				case 94:
					BtrieveErrorCode = "PermissiOn Error";
					break;
				case 95:
					BtrieveErrorCode = "Session No Longer Valid";
					break;
				case 96:
					BtrieveErrorCode = "Communications Environment Error";
					break;
				case 97:
					BtrieveErrorCode = "Data Message Too Small";
					break;
				case 98:
					BtrieveErrorCode = "Internal TransactiOn Error";
					break;
				case 100:
					BtrieveErrorCode = "No Cache Buffers Available";
					break;
				case 101:
					BtrieveErrorCode = "No OS Memory Availabl";
					break;
				case 102:
					BtrieveErrorCode = "Not Enough Stack space";
					break;
				case 1001:
					BtrieveErrorCode = "Lock Option Out Of Range";
					break;
				case 1002:
					BtrieveErrorCode = "Memory AllocatiOn Error";
					break;
				case 1003:
					BtrieveErrorCode = "Memory Option Too Small";
					break;
				case 1004:
					BtrieveErrorCode = "Page Size Option Out Of Range";
					break;
				case 1005:
					BtrieveErrorCode = "Invalid Pre-Image Drive Option";
					break;
				case 1007:
					BtrieveErrorCode = "Files Option Out of Range";
					break;
				case 1008:
					BtrieveErrorCode = "Invalid Initialization Option";
					break;
				case 1009:
					BtrieveErrorCode = "Invalid Transaction File Open";
					break;
				case 1011:
					BtrieveErrorCode = "Compression Buffer Out Of Range";
					break;
				case 1013:
					BtrieveErrorCode = "Task Table Full";
					break;
				case 1014:
					BtrieveErrorCode = "Stop Warning";
					break;
				case 1015:
					BtrieveErrorCode = "Invalid Pointer";
					break;
				case 1016:
					BtrieveErrorCode = "Already Initialized";
					break;
				case 2001:
					BtrieveErrorCode = "Insufficient Memory";
					break;
				case 2003:
					BtrieveErrorCode = "No Local Access Allowed";
					break;
				case 2006:
					BtrieveErrorCode = "No Available SPX Connection";
					break;
				case 2007:
					BtrieveErrorCode = "Invalid Parameter";
					break;

				default:
					BtrieveErrorCode = "Unknown BTRIEVE Error, #" + nStatus;
					break;
			}

			return BtrieveErrorCode;
		}


		public const int BOPEN = 0;
		public const int BCLOSE = 1;
		public const int BINSERT = 2;
		public const int BUPDATE = 3;
		public const int BDELETE = 4;
		public const int BGETEQUAL = 5;
		public const int BGETNEXT = 6;
		public const int BGETPREVIOUS = 7;
		public const int BGETGREATER = 8;
		public const int BGETGREATEROREQUAL = 9;
		public const int BGETFIRST = 12;
		public const int BGETLAST = 13;
		public const int BCREATE = 14;
		public const int BSTAT = 15;
		public const int BBEGINTRANS = 19;
		public const int BTRANSSEND = 20;
		public const int BABORTTRANS = 21;
		public const int BGETPOSITION = 22;
		public const int BGETRECORD = 23;
		public const int BSTOP = 25;
		public const int BVERSION = 26;
		public const int BRESET = 28;
		public const int BGETNEXTEXTENDED = 36;
		public const int BGETKEY = 50;
		public const int KEY_BUF_LEN = 255;
		public const int FIXED = 67;

		//Rem Key Flags
		public const int DUP = 1;
		public const int MODIFIABLE = 2;
		public const int BIN = 4;
		public const int NUL = 8;
		public const int SEGMENT = 16;
		public const int SEQ = 32;
		public const int DEC = 64;
		public const int SUP = 128;
		//Rem Key Types
		public const int EXTTYPE = 256;
		public const int MANUAL = 512;
		public const int BSTRING = 0;
		public const int BINTEGER = 1;
		public const int BFLOAT = 2;
		public const int BDATE = 3;
		public const int BTIME = 4;
		public const int BDECIMAL = 5;
		public const int BNUMERIC = 8;
		public const int BZSTRING = 11;
		public const int BAUTOINC = 15;
		public const int B_NO_ERROR = 0;
		public const int B_END_OF_FILE = 9;
		public const int VAR_RECS = 1;
		public const int BLANK_TRUNC = 2;
		public const int PRE_ALLOC = 4;
		public const int DATA_COMP = 8;
		public const int KEY_ONLY = 16;
		public const int BALANCED_KEYS = 32;
		public const int FREE_10 = 64;
		public const int FREE_20 = 128;
		public const int FREE_30 = 192;
		public const int DUP_PTRS = 256;
		public const int INCLUDE_SYSTEM_DATA = 512;
		public const int NO_INCLUDE_SYSTEM_DATA = 4608;
		public const int SPECIFY_KEY_NUMS = 1024;
		public const int VATS_SUPPORT = 2048;
		public const int FLD_STRING = 0;
		public const int FLD_INTEGER = 1;
		public const int FLD_IEEE = 2;
		public const int FLD_DATE = 3;
		public const int FLD_TIME = 4;
		public const int FLD_MONEY = 6;
		public const int FLD_LOGICAL = 7;
		public const int FLD_BYTE = 19;
		public const int FLD_UNICODE = 20;
		public const int FLD_UNSIGNEDBINARY = 14;
	}
}
