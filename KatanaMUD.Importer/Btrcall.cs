using System;
using System.Text;
using System.Runtime.InteropServices;

namespace CSharp
{
	/// <summary>
	/// frequently used constants
	/// </summary>
	public enum BTRCONSTS
	{
		KEY_BUF_LEN = 255,
		POSBLK_LEN = 128,
	}
	///<summary>
	/// BTR operation code we had to declare it as public since the default to private
	///</summary>
	public enum BTROPS : ushort
	{
		B_OPEN = 0,
		B_CLOSE = 1,
		B_INSERT = 2,
		B_UPDATE = 3,
		B_DELETE = 4,
		B_GET_EQUAL = 5,
		B_GET_NEXT = 6,
		B_GET_PREVIOUS = 7,
		B_GET_FIRST = 12,
		B_GET_LAST = 13,
		B_GET_STAT = 15,
		B_BEGIN_TRAN = 19,
		B_END_TRAN = 20,
		B_ABORT_TRAN = 21,
		B_STEP_NEXT = 24,
		B_STOP = 25,
		B_VERSION = 26,
		B_RESET = 28,
		B_STEP_FIRST = 33,
		B_STEP_LAST = 34,
		B_STEP_PREVIOUS = 35,
		B_GET_NEXT_EXTENDED = 36
	}

	public enum BTRSTATUS : short
	{
		B_NO_ERROR = 0,
		B_INVALID_FUNCTION = 1,
		B_IO_ERROR = 2,
		B_FILE_NOT_OPEN = 3,
		B_KEY_VALUE_NOT_FOUND = 4,
		B_DUPLICATE_KEY_VALUE = 5,
		B_INVALID_KEYNUMBER = 6,
		B_DIFFERENT_KEYNUMBER = 7,
		B_POSITION_NOT_SET = 8,
		B_END_OF_FILE = 9,
		B_MODIFIABLE_KEYVALUE_ERROR = 10,
		B_FILENAME_BAD = 11,
		B_FILE_NOT_FOUND = 12,
		B_EXTENDED_FILE_ERROR = 13,
		B_PREIMAGE_OPEN_ERROR = 14,
		B_PREIMAGE_IO_ERROR = 15,
		B_EXPANSION_ERROR = 16,
		B_CLOSE_ERROR = 17,
		B_DISKFULL = 18,
		B_UNRECOVERABLE_ERROR = 19,
		B_RECORD_MANAGER_INACTIVE = 20,
		B_KEYBUFFER_TOO_SHORT = 21,
		B_DATALENGTH_ERROR = 22,
		B_POSITIONBLOCK_LENGTH = 23,
		B_PAGE_SIZE_ERROR = 24,
		B_CREATE_IO_ERROR = 25,
		B_NUMBER_OF_KEYS = 26,
		B_INVALID_KEY_POSITION = 27,
		B_INVALID_RECORD_LENGTH = 28,
		B_INVALID_KEYLENGTH = 29,
		B_NOT_A_BTRIEVE_FILE = 30,
		B_FILE_ALREADY_EXTENDED = 31,
		B_EXTEND_IO_ERROR = 32,
		B_BTR_CANNOT_UNLOAD = 33,
		B_INVALID_EXTENSION_NAME = 34,
		B_DIRECTORY_ERROR = 35,
		B_TRANSACTION_ERROR = 36,
		B_TRANSACTION_IS_ACTIVE = 37,
		B_TRANSACTION_FILE_IO_ERROR = 38,
		B_END_TRANSACTION_ERROR = 39,
		B_TRANSACTION_MAX_FILES = 40,
		B_OPERATION_NOT_ALLOWED = 41,
		B_INCOMPLETE_ACCEL_ACCESS = 42,
		B_INVALID_RECORD_ADDRESS = 43,
		B_NULL_KEYPATH = 44,
		B_INCONSISTENT_KEY_FLAGS = 45,
		B_ACCESS_TO_FILE_DENIED = 46,
		B_MAXIMUM_OPEN_FILES = 47,
		B_INVALID_ALT_SEQUENCE_DEF = 48,
		B_KEY_TYPE_ERROR = 49,
		B_OWNER_ALREADY_SET = 50,
		B_INVALID_OWNER = 51,
		B_ERROR_WRITING_CACHE = 52,
		B_INVALID_INTERFACE = 53,
		B_VARIABLE_PAGE_ERROR = 54,
		B_AUTOINCREMENT_ERROR = 55,
		B_INCOMPLETE_INDEX = 56,
		B_EXPANED_MEM_ERROR = 57,
		B_COMPRESS_BUFFER_TOO_SHORT = 58,
		B_FILE_ALREADY_EXISTS = 59,
		B_REJECT_COUNT_REACHED = 60,
		B_SMALL_EX_GET_BUFFER_ERROR = 61,
		B_INVALID_GET_EXPRESSION = 62,
		B_INVALID_EXT_INSERT_BUFF = 63,
		B_OPTIMIZE_LIMIT_REACHED = 64,
		B_INVALID_EXTRACTOR = 65,
		B_RI_TOO_MANY_DATABASES = 66,
		B_RIDDF_CANNOT_OPEN = 67,
		B_RI_CASCADE_TOO_DEEP = 68,
		B_RI_CASCADE_ERROR = 69,
		B_RI_VIOLATION = 71,
		B_RI_REFERENCED_FILE_CANNOT_OPEN = 72,
		B_RI_OUT_OF_SYNC = 73,
		B_END_CHANGED_TO_ABORT = 74,
		B_RI_CONFLICT = 76,
		B_CANT_LOOP_IN_SERVER = 77,
		B_DEAD_LOCK = 78,
		B_PROGRAMMING_ERROR = 79,
		B_CONFLICT = 80,
		B_LOCKERROR = 81,
		B_LOST_POSITION = 82,
		B_READ_OUTSIDE_TRANSACTION = 83,
		B_RECORD_INUSE = 84,
		B_FILE_INUSE = 85,
		B_FILE_TABLE_FULL = 86,
		B_NOHANDLES_AVAILABLE = 87,
		B_INCOMPATIBLE_MODE_ERROR = 88,

		B_DEVICE_TABLE_FULL = 90,
		B_SERVER_ERROR = 91,
		B_TRANSACTION_TABLE_FULL = 92,
		B_INCOMPATIBLE_LOCK_TYPE = 93,
		B_PERMISSION_ERROR = 94,
		B_SESSION_NO_LONGER_VALID = 95,
		B_COMMUNICATIONS_ERROR = 96,
		B_DATA_MESSAGE_TOO_SMALL = 97,
		B_INTERNAL_TRANSACTION_ERROR = 98,
		B_REQUESTER_CANT_ACCESS_RUNTIME = 99,
		B_NO_CACHE_BUFFERS_AVAIL = 100,
		B_NO_OS_MEMORY_AVAIL = 101,
		B_NO_STACK_AVAIL = 102,
		B_CHUNK_OFFSET_TOO_LONG = 103,
		B_LOCALE_ERROR = 104,
		B_CANNOT_CREATE_WITH_BAT = 105,
		B_CHUNK_CANNOT_GET_NEXT = 106,
		B_CHUNK_INCOMPATIBLE_FILE = 107,

		B_TRANSACTION_TOO_COMPLEX = 109,

		B_ARCH_BLOG_OPEN_ERROR = 110,
		B_ARCH_FILE_NOT_LOGGED = 111,
		B_ARCH_FILE_IN_USE = 112,
		B_ARCH_LOGFILE_NOT_FOUND = 113,
		B_ARCH_LOGFILE_INVALID = 114,
		B_ARCH_DUMPFILE_ACCESS_ERROR = 115,
		B_LOCATOR_FILE_INDICATOR = 116,

		B_NO_SYSTEM_LOCKS_AVAILABLE = 130,
		B_FILE_FULL = 132,
		B_MORE_THAN_5_CONCURRENT_USERS = 133,

		B_ISR_READ_ERROR = 134,	 /* Old definition     */
		B_ISR_NOT_FOUND = 134,	/* New definition     */

		B_ISR_FORMAT_INVALID = 135,	 /* Old definition     */
		B_ISR_INVALID = 135,  /* New definition     */
		B_ACS_NOT_FOUND = 136,
		B_CANNOT_CONVERT_RP = 137,
		B_INVALID_NULL_INDICATOR = 138,
		B_INVALID_KEY_OPTION = 139,
		B_INCOMPATIBLE_CLOSE = 140,
		B_INVALID_USERNAME = 141,
		B_INVALID_DATABASE = 142,
		B_NO_SSQL_RIGHTS = 143,
		B_ALREADY_LOGGED_IN = 144,
		B_NO_DATABASE_SERVICES = 145,
		B_DUPLICATE_SYSTEM_KEY = 146,
		B_LOG_SEGMENT_MISSING = 147,
		B_ROLL_FORWARD_ERROR = 148,
		B_SYSTEM_KEY_INTERNAL = 149,
		B_DBS_INTERNAL_ERROR = 150,
		B_NESTING_DEPTH_ERROR = 151,

		B_INVALID_PARAMETER_TO_MKDE = 160,

		/* User Count Manager Return code */
		B_USER_COUNT_LIMIT_EXCEEDED = 161,

		B_CLIENT_TABLE_FULL = 162,
		B_LAST_SEGMENT_ERROR = 163,

		/* Windows/OS2 Client Return codes */
		B_LOCK_PARM_OUTOFRANGE = 1001,
		B_MEM_ALLOCATION_ERR = 1002,
		B_MEM_PARM_TOO_SMALL = 1003,
		B_PAGE_SIZE_PARM_OUTOFRANGE = 1004,
		B_INVALID_PREIMAGE_PARM = 1005,
		B_PREIMAGE_BUF_PARM_OUTOFRANGE = 1006,
		B_FILES_PARM_OUTOFRANGE = 1007,
		B_INVALID_INIT_PARM = 1008,
		B_INVALID_TRANS_PARM = 1009,
		B_ERROR_ACC_TRANS_CONTROL_FILE = 1010,
		B_COMPRESSION_BUF_PARM_OUTOFRANGE = 1011,
		B_INV_N_OPTION = 1012,
		B_TASK_LIST_FULL = 1013,
		B_STOP_WARNING = 1014,
		B_POINTER_PARM_INVALID = 1015,
		B_ALREADY_INITIALIZED = 1016,
		B_REQ_CANT_FIND_RES_DLL = 1017,
		B_ALREADY_INSIDE_BTR_FUNCTION = 1018,
		B_CALLBACK_ABORT = 1019,
		B_INTF_COMM_ERROR = 1020,
		B_FAILED_TO_INITIALIZE = 1021,
		B_MKDE_SHUTTING_DOWN = 1022,

		/* Btrieve requestor status codes */
		B_INTERNAL_ERROR = 2000,
		B_INSUFFICIENT_MEM_ALLOC = 2001,
		B_INVALID_OPTION = 2002,
		B_NO_LOCAL_ACCESS_ALLOWED = 2003,
		B_SPX_NOT_INSTALLED = 2004,
		B_INCORRECT_SPX_VERSION = 2005,
		B_NO_AVAIL_SPX_CONNECTION = 2006,
		B_INVALID_PTR_PARM = 2007,
		B_CANT_CONNECT_TO_615 = 2008,
		B_CANT_LOAD_MKDE_ROUTER = 2009,
		B_UT_THUNK_NOT_LOADED = 2010,
		B_NO_RESOURCE_DLL = 2011,
		B_OS_ERROR = 2012,

		/*  MKDE Router status codes */
		B_MK_ROUTER_MEM_ERROR = 3000,
		B_MK_NO_LOCAL_ACCESS_ALLOWED = 3001,
		B_MK_NO_RESOURCE_DLL = 3002,
		B_MK_INCOMPAT_COMPONENT = 3003,
		B_MK_TIMEOUT_ERROR = 3004,
		B_MK_OS_ERROR = 3005,
		B_MK_INVALID_SESSION = 3006,
		B_MK_SERVER_NOT_FOUND = 3007,
		B_MK_INVALID_CONFIG = 3008,
		B_MK_NETAPI_NOT_LOADED = 3009,
		B_MK_NWAPI_NOT_LOADED = 3010,
		B_MK_THUNK_NOT_LOADED = 3011,
		B_MK_LOCAL_NOT_LOADED = 3012,
		B_MK_PNSL_NOT_LOADED = 3013,
		B_MK_CANT_FIND_ENGINE = 3014,
		B_MK_INIT_ERROR = 3015,
		B_MK_INTERNAL_ERROR = 3016,
		B_MK_LOCAL_MKDE_DATABUF_TOO_SMALL = 3017,
		B_MK_CLOSED_ERROR = 3018,
		B_MK_SEMAPHORE_ERROR = 3019,
		B_MK_LOADING_ERROR = 3020,
		B_MK_BAD_SRB_FORMAT = 3021,
		B_MK_DATABUF_LEN_TOO_LARGE = 3022,
		B_MK_TASK_TABLE_FULL = 3023,
		B_MK_INVALID_OP_ON_REMOTE = 3024,
		B_MK_PIDS_NOT_LOADED = 3025,
		B_MK_BAD_PIDS = 3026,
		B_MK_IDS_CONNECT_FAILURE = 3027,
		B_MK_IDS_LOGIN_FAILURE = 3028,

		/* PNSL status codes */
		B_NL_FAILURE = 3101,
		B_NL_NOT_INITIALIZED = 3102,
		B_NL_NAME_NOT_FOUND = 3103,
		B_NL_PERMISSION_ERROR = 3104,
		B_NL_NO_AVAILABLE_TRANSPORT = 3105,
		B_NL_CONNECTION_FAILURE = 3106,
		B_NL_OUT_OF_MEMORY = 3107,
		B_NL_INVALID_SESSION = 3108,
		B_NL_MORE_DATA = 3109,
		B_NL_NOT_CONNECTED = 3110,
		B_NL_SEND_FAILURE = 3111,
		B_NL_RECEIVE_FAILURE = 3112,
		B_NL_INVALID_SERVER_TYPE = 3113,
		B_NL_SRT_FULL = 3114,
		B_NL_TRANSPORT_FAILURE = 3115,
		B_NL_RCV_DATA_OVERFLOW = 3116,
		B_NL_CST_FULL = 3117,
		B_NL_INVALID_ADDRESS_FAMILY = 3118,
		B_NL_NO_AUTH_CONTEXT_AVAILABLE = 3119,
		B_NL_INVALID_AUTH_TYPE = 3120,
		B_NL_INVALID_AUTH_OBJECT = 3121,
		B_NL_AUTH_LEN_TOO_SMALL = 3122,
		B_NL_INVALID_SESSION_LEVEL_PARM = 3123,
		B_NL_TASK_TABLE_FULL = 3124,
		B_NL_NDS_NAME_RESOLUTION_ERROR = 3125,
		B_NL_FILE_NAME_RESOLUTION_ERROR = 3126,
		B_NL_IDS_SEND_FAILURE = 3127,
		B_NL_IDS_RCV_FAILURE = 3128,
	}

	// Btrieve engine version information structure
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
	public struct BTR_VERSION
	{
		public short LocalClntVersion;
		public short LocalClntRevision;
		public char LocalClntID;
		public short LocalSvrVersion;
		public short LocalSvrRevision;
		public char LocalSvrID;
		public short RemoteSvrVersion;
		public short RemoteSvrRevision;
		public char RemoteSvrID;
	}

	// Btrieve Key information structure
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
	public struct BTR_KEY_SPEC
	{
		public short Pos;
		public short Len;
		public short Flag;
		public int Total;
		public byte Type;
		public byte nullVal;
		public ushort notUsed;
		public byte manualKeyNum;
		public byte ascNum;
	}

	// Btrieve file information structure
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
	public struct BTR_FILE_SPEC
	{
		public short RecLen;
		public short PageSize;
		public byte Indexes;
		public byte FileVersionNum;
		public uint Records;
		public short FileFlags;
		public byte Duplicated;
		public byte NotUsed;
		public short UnusedPages;
	}

	// Btrieve file status structure
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi, Size = 96)]
	public struct BTR_FILE_STATS
	{
		public BTR_FILE_SPEC fileSpec;
		/// <remark>
		/// Couldn't get the array field marshalled, 
		/// so that we define, at least, one key and use Size attribute to reserve some 
		/// room for extra keys. The size specified is equivalent to the array 
		/// </remark>
		//[MarshalAs(UnmanagedType.LPArray, SizeConst=5)] public BTR_KEY_SPEC[] Keys; 
		public BTR_KEY_SPEC Keys;
	}

	/// <summary>
	/// Summary description for BtrCall.
	/// Using DllImport to declare BTRCALL as static external function.
	/// BTRCALL is overloaded for different parameter types
	/// </summary>
	public class BtrCall
	{
		[DllImport("w3btrv7.dll", CharSet = CharSet.Ansi)]
		public static extern short BTRCALL(
			ushort operation,
			[MarshalAs(UnmanagedType.LPArray, SizeConst = 128)] byte[] posBlk, // do not use string or StringBuilder
			IntPtr dataBuffer,
			ref uint dataLength,
			[MarshalAs(UnmanagedType.LPArray, SizeConst = 255)] byte[] keyBffer,// do not use string or StringBuilder
			byte keyLength,
			char keyNum);

		[DllImport("w3btrv7.dll", CharSet = CharSet.Ansi)]
		public static extern short BTRCALL(
			ushort operation,
			[MarshalAs(UnmanagedType.LPArray, SizeConst = 128)] byte[] posBlk, // do not use string or StringBuilder
			ref BTR_VERSION dataBuffer,
			ref uint dataLength,
			[MarshalAs(UnmanagedType.LPArray, SizeConst = 255)] byte[] keyBffer,// do not use string or StringBuilder
			byte keyLength,
			char keyNum);

		[DllImport("w3btrv7.dll", CharSet = CharSet.Ansi)]
		public static extern short BTRCALL(
			ushort operation,
			[MarshalAs(UnmanagedType.LPArray, SizeConst = 128)] byte[] posBlk, // do not use string or StringBuilder
			ref BTR_FILE_STATS dataBuffer,
			ref uint dataLength,
			[MarshalAs(UnmanagedType.LPArray, SizeConst = 255)] byte[] keyBffer,// do not use string or StringBuilder
			byte keyLength,
			char keyNum);
	}
}
