using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Win32PortEnumerate
{
	static class NativeMethods
	{
		public const int NO_ERROR = 0;
		public const long INVALID_HANDLE_VALUE = -1;
		public const int ERROR_NO_MORE_ITEMS = 259;

		[Flags]
		internal enum GetDeviceIdResult : uint
		{
			CR_SUCCESS = 0x00000000,
			CR_DEFAULT = 0x00000001,
			CR_OUT_OF_MEMORY = 0x00000002,
			CR_INVALID_POINTER = 0x00000003,
			CR_INVALID_FLAG = 0x00000004,
			CR_INVALID_DEVNODE = 0x00000005,
			CR_INVALID_DEVINST = CR_INVALID_DEVNODE,
			CR_INVALID_RES_DES = 0x00000006,
			CR_INVALID_LOG_CONF = 0x00000007,
			CR_INVALID_ARBITRATOR = 0x00000008,
			CR_INVALID_NODELIST = 0x00000009,
			CR_DEVNODE_HAS_REQS = 0x0000000A,
			CR_DEVINST_HAS_REQS = CR_DEVNODE_HAS_REQS,
			CR_INVALID_RESOURCEID = 0x0000000B,
			CR_DLVXD_NOT_FOUND = 0x0000000C,
			CR_NO_SUCH_DEVNODE = 0x0000000D,
			CR_NO_SUCH_DEVINST = CR_NO_SUCH_DEVNODE,
			CR_NO_MORE_LOG_CONF = 0x0000000E,
			CR_NO_MORE_RES_DES = 0x0000000F,
			CR_ALREADY_SUCH_DEVNODE = 0x00000010,
			CR_ALREADY_SUCH_DEVINST = CR_ALREADY_SUCH_DEVNODE,
			CR_INVALID_RANGE_LIST = 0x00000011,
			CR_INVALID_RANGE = 0x00000012,
			CR_FAILURE = 0x00000013,
			CR_NO_SUCH_LOGICAL_DEV = 0x00000014,
			CR_CREATE_BLOCKED = 0x00000015,
			CR_NOT_SYSTEM_VM = 0x00000016,
			CR_REMOVE_VETOED = 0x00000017,
			CR_APM_VETOED = 0x00000018,
			CR_INVALID_LOAD_TYPE = 0x00000019,
			CR_BUFFER_SMALL = 0x0000001A,
			CR_NO_ARBITRATOR = 0x0000001B,
			CR_NO_REGISTRY_HANDLE = 0x0000001C,
			CR_REGISTRY_ERROR = 0x0000001D,
			CR_INVALID_DEVICE_ID = 0x0000001E,
			CR_INVALID_DATA = 0x0000001F,
			CR_INVALID_API = 0x00000020,
			CR_DEVLOADER_NOT_READY = 0x00000021,
			CR_NEED_RESTART = 0x00000022,
			CR_NO_MORE_HW_PROFILES = 0x00000023,
			CR_DEVICE_NOT_THERE = 0x00000024,
			CR_NO_SUCH_VALUE = 0x00000025,
			CR_WRONG_TYPE = 0x00000026,
			CR_INVALID_PRIORITY = 0x00000027,
			CR_NOT_DISABLEABLE = 0x00000028,
			CR_FREE_RESOURCES = 0x00000029,
			CR_QUERY_VETOED = 0x0000002A,
			CR_CANT_SHARE_IRQ = 0x0000002B,
			CR_NO_DEPENDENT = 0x0000002C,
			CR_SAME_RESOURCES = 0x0000002D,
			CR_NO_SUCH_REGISTRY_KEY = 0x0000002E,
			CR_INVALID_MACHINENAME = 0x0000002F,
			CR_REMOTE_COMM_FAILURE = 0x00000030,
			CR_MACHINE_UNAVAILABLE = 0x00000031,
			CR_NO_CM_SERVICES = 0x00000032,
			CR_ACCESS_DENIED = 0x00000033,
			CR_CALL_NOT_IMPLEMENTED = 0x00000034,
			CR_INVALID_PROPERTY = 0x00000035,
			CR_DEVICE_INTERFACE_ACTIVE = 0x00000036,
			CR_NO_SUCH_DEVICE_INTERFACE = 0x00000037,
			CR_INVALID_REFERENCE_STRING = 0x00000038,
			CR_INVALID_CONFLICT_LIST = 0x00000039,
			CR_INVALID_INDEX = 0x0000003A,
			CR_INVALID_STRUCTURE_SIZE = 0x0000003B
		}

		[Flags]
		internal enum DiGetClassFlags : uint
		{
			DIGCF_DEFAULT = 0x00000001, // only valid with DIGCF_DEVICEINTERFACE
			DIGCF_PRESENT = 0x00000002,
			DIGCF_ALLCLASSES = 0x00000004,
			DIGCF_PROFILE = 0x00000008,
			DIGCF_DEVICEINTERFACE = 0x00000010
		}

		internal enum DeviceInfoKeyType : ulong
		{
			DIREG_DEV = 0x00000001, // Open/Create/Delete device key
			DIREG_DRV = 0x00000002, // Open/Create/Delete driver key
			DIREG_BOTH = 0x00000004  // Delete both driver and Device key
		}

		[Flags]
		internal enum DeviceInfoPropertyScope : uint
		{
			DICS_FLAG_GLOBAL = 0x00000001, // make change in all hardware profiles
			DICS_FLAG_CONFIGSPECIFIC = 0x00000002, // make change in specified profile only
			DICS_FLAG_CONFIGGENERAL = 0x00000004 // 1 or more hardware profile-specific changes to follow.
		}

		internal enum DeviceInfoRegistryKeyType : uint
		{
			DIREG_DEV = 0x00000001, // Open/Create/Delete device key
			DIREG_DRV = 0x00000002, // Open/Create/Delete driver key
			DIREG_BOTH = 0x00000004 // Delete both driver and Device key
		}

		[Flags]
		internal enum StandardAccessRights : uint
		{
			DELETE = 0x00010000,
			READ_CONTROL = 0x00020000,
			WRITE_DAC = 0x00040000,
			WRITE_OWNER = 0x00080000,
			SYNCHRONIZE = 0x00100000,

			STANDARD_RIGHTS_REQUIRED = 0x000F0000,

			STANDARD_RIGHTS_READ = READ_CONTROL,
			STANDARD_RIGHTS_WRITE = READ_CONTROL,
			STANDARD_RIGHTS_EXECUTE = READ_CONTROL,

			STANDARD_RIGHTS_ALL = 0x001F0000
		}

		[Flags]
		internal enum RegistrySpecificAccessRights : uint
		{
			KEY_QUERY_VALUE = 0x0001,
			KEY_SET_VALUE = 0x0002,
			KEY_CREATE_SUB_KEY = 0x0004,
			KEY_ENUMERATE_SUB_KEYS = 0x0008,
			KEY_NOTIFY = 0x0010,
			KEY_CREATE_LINK = 0x0020,
			KEY_WOW64_32KEY = 0x0200,
			KEY_WOW64_64KEY = 0x0100,
			KEY_WOW64_RES = 0x0300,

			KEY_READ = (StandardAccessRights.STANDARD_RIGHTS_READ | KEY_QUERY_VALUE | KEY_ENUMERATE_SUB_KEYS | KEY_NOTIFY) & ~StandardAccessRights.SYNCHRONIZE,
			KEY_WRITE = (StandardAccessRights.STANDARD_RIGHTS_WRITE | KEY_SET_VALUE | KEY_CREATE_SUB_KEY) & ~StandardAccessRights.SYNCHRONIZE,
			KEY_EXECUTE = KEY_READ & ~StandardAccessRights.SYNCHRONIZE,

			KEY_ALL_ACCESS = StandardAccessRights.STANDARD_RIGHTS_ALL | KEY_QUERY_VALUE | KEY_SET_VALUE | KEY_CREATE_SUB_KEY | KEY_ENUMERATE_SUB_KEYS | KEY_NOTIFY | KEY_CREATE_LINK & ~StandardAccessRights.SYNCHRONIZE
		}

		internal enum DeviceInfoRegistryProperty : uint
		{
			//
			// Device registry property codes
			// (Codes marked as read-only (R) may only be used for
			// SetupDiGetDeviceRegistryProperty)
			//
			// These values should cover the same set of registry properties
			// as defined by the CM_DRP codes in cfgmgr32.h.
			//
			// Note that SPDRP codes are zero based while CM_DRP codes are one based!
			//
			SPDRP_DEVICEDESC = 0x00000000,  // DeviceDesc = R/W,
			SPDRP_HARDWAREID = 0x00000001,  // HardwareID = R/W,
			SPDRP_COMPATIBLEIDS = 0x00000002,  // CompatibleIDs = R/W,
			SPDRP_UNUSED0 = 0x00000003,  // unused
			SPDRP_SERVICE = 0x00000004,  // Service = R/W,
			SPDRP_UNUSED1 = 0x00000005,  // unused
			SPDRP_UNUSED2 = 0x00000006,  // unused
			SPDRP_CLASS = 0x00000007,  // Class = R--tied to ClassGUID,
			SPDRP_CLASSGUID = 0x00000008,  // ClassGUID = R/W,
			SPDRP_DRIVER = 0x00000009,  // Driver = R/W,
			SPDRP_CONFIGFLAGS = 0x0000000A,  // ConfigFlags = R/W,
			SPDRP_MFG = 0x0000000B,  // Mfg = R/W,
			SPDRP_FRIENDLYNAME = 0x0000000C,  // FriendlyName = R/W,
			SPDRP_LOCATION_INFORMATION = 0x0000000D,  // LocationInformation = R/W,
			SPDRP_PHYSICAL_DEVICE_OBJECT_NAME = 0x0000000E,  // PhysicalDeviceObjectName = R,
			SPDRP_CAPABILITIES = 0x0000000F,  // Capabilities = R,
			SPDRP_UI_NUMBER = 0x00000010,  // UiNumber = R,
			SPDRP_UPPERFILTERS = 0x00000011,  // UpperFilters = R/W,
			SPDRP_LOWERFILTERS = 0x00000012,  // LowerFilters = R/W,
			SPDRP_BUSTYPEGUID = 0x00000013,  // BusTypeGUID = R,
			SPDRP_LEGACYBUSTYPE = 0x00000014,  // LegacyBusType = R,
			SPDRP_BUSNUMBER = 0x00000015,  // BusNumber = R,
			SPDRP_ENUMERATOR_NAME = 0x00000016,  // Enumerator Name = R,
			SPDRP_SECURITY = 0x00000017,  // Security = R/W, binary form,
			SPDRP_SECURITY_SDS = 0x00000018,  // Security = W, SDS form,
			SPDRP_DEVTYPE = 0x00000019,  // Device Type = R/W,
			SPDRP_EXCLUSIVE = 0x0000001A,  // Device is exclusive-access = R/W,
			SPDRP_CHARACTERISTICS = 0x0000001B,  // Device Characteristics = R/W,
			SPDRP_ADDRESS = 0x0000001C,  // Device Address = R,
			SPDRP_UI_NUMBER_DESC_FORMAT = 0X0000001D,  // UiNumberDescFormat = R/W,
			SPDRP_DEVICE_POWER_DATA = 0x0000001E,  // Device Power Data = R,
			SPDRP_REMOVAL_POLICY = 0x0000001F,  // Removal Policy = R,
			SPDRP_REMOVAL_POLICY_HW_DEFAULT = 0x00000020,  // Hardware Removal Policy = R,
			SPDRP_REMOVAL_POLICY_OVERRIDE = 0x00000021,  // Removal Policy Override = RW,
			SPDRP_INSTALL_STATE = 0x00000022,  // Device Install State = R,
			SPDRP_LOCATION_PATHS = 0x00000023,  // Device Location Paths = R,
			SPDRP_BASE_CONTAINERID = 0x00000024,  // Base ContainerID = R,

			SPDRP_MAXIMUM_PROPERTY = 0x00000025  // Upper bound on ordinals                
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct DevInfoData
		{
			public uint CbSize;
			public Guid ClassGuid;
			public uint DevInst;
			public UIntPtr Reserved;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct DEVPROPKEY
		{
			public Guid Fmtid;
			public UInt32 Pid;
		}


		[DllImport("setupapi.dll", SetLastError = true)]
		internal static extern bool SetupDiClassGuidsFromName(
			string ClassName,
			ref Guid ClassGuidArray1stItem, UInt32 ClassGuidArraySize,
			out UInt32 RequiredSize
		);

		[DllImport("SetupAPI.dll", SetLastError = true, CharSet = CharSet.Auto)]
		internal static extern IntPtr SetupDiGetClassDevs(
			ref Guid classGuid,
			[MarshalAs(UnmanagedType.LPTStr)] string enumerator,
			IntPtr hwndParent,
			DiGetClassFlags flags
		);

		[DllImport("SetupAPI.dll", SetLastError = true, CharSet = CharSet.Auto)]
		internal static extern IntPtr SetupDiCreateDeviceInfoList(
			IntPtr ClassGuid, /* should be 'ref Guid ClassGuid' but is optional (==null if not used but a ref cant be null) */
			IntPtr hwndParent
		);

		[DllImport("SetupAPI.dll", SetLastError = true)]
		internal static extern bool SetupDiEnumDeviceInfo(
			IntPtr deviceInfoSet,
			uint memberIndex,
			ref DevInfoData deviceInfoData
		);

		[DllImport("SetupAPI.dll", SetLastError = true)]
		internal static extern IntPtr SetupDiOpenDevRegKey(
			IntPtr deviceInfoSet,
			ref DevInfoData deviceInfoData,
			DeviceInfoPropertyScope scope,
			uint hwProfile,
			DeviceInfoRegistryKeyType keyType,
			RegistrySpecificAccessRights samDesired
		);

		[DllImport("SetupAPI.dll", SetLastError = true)]
		internal static extern bool SetupDiGetDeviceRegistryProperty(
			IntPtr deviceInfoSet,
			ref DevInfoData deviceInfoData,
			DeviceInfoRegistryProperty property,
			out uint propertyRegDataType,
			StringBuilder propertyBuffer,
			uint propertyBufferSize,
			out uint requiredSize
		);

		[DllImport("SetupAPI.dll", SetLastError = true)]
		internal static extern int SetupDiDestroyDeviceInfoList(
			IntPtr deviceInfoSet
		);

		[DllImport("setupapi.dll")]
		internal static extern int CM_Get_Parent(
			out UInt32 pdnDevInst,
			UInt32 dnDevInst,
			int flags = 0
		);

		[DllImport("setupapi.dll", SetLastError = true)]
		internal static extern int CM_Get_Device_ID_Size(
			out int pulLen,
			UInt32 dnDevInst,
			int flags = 0
		);

		[DllImport("setupapi.dll", SetLastError = true)]
		internal static extern int CM_Get_Device_ID(
			uint dnDevInst,
			StringBuilder Buffer,
			int BufferLen,
			int ulFlags = 0
		);

		[DllImport("setupapi.dll")]
		internal static extern bool SetupDiOpenDeviceInfo(
			IntPtr deviceInfoSet,
			string deviceInstanceId,
			IntPtr hwndParent,
			int openFlags,
			ref DevInfoData deviceInfoData
		);

		[DllImport("setupapi.dll", SetLastError = true)]
		internal static extern bool SetupDiGetDevicePropertyW(
			IntPtr deviceInfoSet,
			[In] ref DevInfoData deviceInfoData,
			[In] ref DEVPROPKEY propertyKey,
			[Out] out UInt32 propertyType,
			byte[] propertyBuffer,
			UInt32 propertyBufferSize,
			out UInt32 requiredSize,
			UInt32 flags
		);

		[DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern int StrCmpLogicalW(string x, string y);
	}
}
