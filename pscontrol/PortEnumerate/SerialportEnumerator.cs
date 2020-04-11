using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
//using System.Text.RegularExpressions;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace Win32PortEnumerate
{
	public static class SerialportEnumerator
	{
		const int utf16terminatorSize_bytes = 2;

		public static IEnumerable<SerialportDevice> Enumerate()
		{
			Guid[] guids = GetClassGUIDs("Ports");
			Guid[] addGuids = GetClassGUIDs("Modem");
			int oldSize = guids.Length;
			Array.Resize<Guid>(ref guids, oldSize + addGuids.Length);
			addGuids.CopyTo(guids, oldSize);

			string port;
			string description;
			string busDescription;
			string deviceId;
			int size;
			for (int guidIndex = 0; guidIndex < guids.Length; guidIndex++)
			{
				IntPtr hDevInfoSet = NativeMethods.SetupDiGetClassDevs(ref guids[guidIndex], null, IntPtr.Zero, NativeMethods.DiGetClassFlags.DIGCF_PRESENT);
				if (hDevInfoSet.ToInt64() == NativeMethods.INVALID_HANDLE_VALUE)
				{
					yield break;
				}
				IntPtr hParentsDevInfoSet = NativeMethods.SetupDiCreateDeviceInfoList(IntPtr.Zero, IntPtr.Zero);
				if (hParentsDevInfoSet.ToInt64() == NativeMethods.INVALID_HANDLE_VALUE)
				{
					yield break;
				}

				NativeMethods.DevInfoData devInfoData = new NativeMethods.DevInfoData();
				NativeMethods.DevInfoData parentDevInfoData = new NativeMethods.DevInfoData();
				devInfoData.CbSize = (uint)Marshal.SizeOf(devInfoData);
				parentDevInfoData.CbSize = (uint)Marshal.SizeOf(parentDevInfoData);

				try
				{
					for (uint i = 0; NativeMethods.SetupDiEnumDeviceInfo(hDevInfoSet, i, ref devInfoData); i++)
					{
						port = GetPortName(hDevInfoSet, devInfoData);
						if (!port.StartsWith("COM")) continue;
						busDescription = "";
						deviceId = "";
						description = GetDescription(hDevInfoSet, devInfoData);
						NativeMethods.CM_Get_Device_ID_Size(out size, devInfoData.DevInst);
						size++;//account for '\0'
						StringBuilder IDBuffer = new StringBuilder(size);
						int getDevIdRet = NativeMethods.CM_Get_Device_ID(devInfoData.DevInst, IDBuffer, size);
						if (getDevIdRet != (int)NativeMethods.GetDeviceIdResult.CR_SUCCESS)
						{
							//throw new Win32Exception(Marshal.GetLastWin32Error(), $"Failed to get parent device ID. With error: {getDevIdRet}]");
						}
						else
						{
							deviceId = IDBuffer.ToString();
							busDescription = GetDeviceBusDescription(hDevInfoSet, devInfoData);
						}


						if ((busDescription.Length == 0) || (busDescription == description))
						{
							NativeMethods.CM_Get_Parent(out parentDevInfoData.DevInst, devInfoData.DevInst);
							NativeMethods.CM_Get_Device_ID_Size(out size, parentDevInfoData.DevInst);
							size++;//account for '\0'
							IDBuffer = new StringBuilder(size);
							getDevIdRet = NativeMethods.CM_Get_Device_ID(parentDevInfoData.DevInst, IDBuffer, size);
							if (getDevIdRet != (int)NativeMethods.GetDeviceIdResult.CR_SUCCESS)
							{
								//throw new Win32Exception(Marshal.GetLastWin32Error(), $"Failed to get parent device ID. With error: {getDevIdRet}]");
							}
							else
							{
								deviceId = IDBuffer.ToString();
								bool result = NativeMethods.SetupDiOpenDeviceInfo(hParentsDevInfoSet, deviceId, IntPtr.Zero, 0, ref parentDevInfoData);
								if (!result)
								{
									//throw new Win32Exception(Marshal.GetLastWin32Error(), $"SetupDiOpenDeviceInfo failed Error: [{Marshal.GetLastWin32Error()}] HR: [{Marshal.GetHRForLastWin32Error()}]");
									//busDescription = "";
								}
								else
								{
									busDescription = GetDeviceBusDescription(hParentsDevInfoSet, parentDevInfoData);
								}
							}
						}


						//Dictionary<string, string> id = GetDeviceVidPid(hDevInfoSet, devInfoData);

						var device = new SerialportDevice
						{
							Port = port,
							Description = description,
							FriendlyName = GetFriendlyName(hDevInfoSet, devInfoData),
							BusReportedDeviceDescription = busDescription,
							DeviceID = deviceId
							//Vid = id.ContainsKey("VID") ? (int?)int.Parse(id["VID"], System.Globalization.NumberStyles.HexNumber) : null,
							//Pid = id.ContainsKey("PID") ? (int?)int.Parse(id["PID"], System.Globalization.NumberStyles.HexNumber) : null,
							//Rev = id.ContainsKey("REV") ? (int?)int.Parse(id["REV"], System.Globalization.NumberStyles.HexNumber) : null
						};

						yield return device;
					}

					if (Marshal.GetLastWin32Error() != NativeMethods.NO_ERROR &&
						Marshal.GetLastWin32Error() != NativeMethods.ERROR_NO_MORE_ITEMS)
					{
						throw new Win32Exception(Marshal.GetLastWin32Error(), $"Failed to enumerate USB serial devices. Error: [{Marshal.GetLastWin32Error()}] HR: [{Marshal.GetHRForLastWin32Error()}]");
					}
				}
				finally
				{
					NativeMethods.SetupDiDestroyDeviceInfoList(hParentsDevInfoSet);
					NativeMethods.SetupDiDestroyDeviceInfoList(hDevInfoSet);
				}
			}
		}

		private static Guid[] GetClassGUIDs(string className)
		{
			uint requiredSize;
			Guid[] guidArray = new Guid[1];

			bool status = NativeMethods.SetupDiClassGuidsFromName(className, ref guidArray[0], 1, out requiredSize);
			if (true == status)
			{
				if (1 < requiredSize)
				{
					guidArray = new Guid[requiredSize];
					NativeMethods.SetupDiClassGuidsFromName(className, ref guidArray[0], requiredSize, out requiredSize);
				}
			}
			else
				throw new System.ComponentModel.Win32Exception();

			return guidArray;
		}

		private static string GetPortName(IntPtr hDevInfoSet, NativeMethods.DevInfoData devInfoData)
		{
			var hRegKey = NativeMethods.SetupDiOpenDevRegKey(
				hDevInfoSet,
				ref devInfoData,
				NativeMethods.DeviceInfoPropertyScope.DICS_FLAG_GLOBAL,
				0,
				NativeMethods.DeviceInfoRegistryKeyType.DIREG_DEV,
				NativeMethods.RegistrySpecificAccessRights.KEY_QUERY_VALUE);

			if (hRegKey == IntPtr.Zero) return string.Empty;

			var safeHandle = new SafeRegistryHandle(hRegKey, true);

			var key = RegistryKey.FromHandle(safeHandle);
			return key.GetValue(@"PortName") as string;
		}

		private static string GetDescription(IntPtr hDevInfoSet, NativeMethods.DevInfoData devInfoData)
		{
			var buffer = new StringBuilder(256);
			var length = (uint)buffer.Capacity;
			NativeMethods.SetupDiGetDeviceRegistryProperty(hDevInfoSet, ref devInfoData, NativeMethods.DeviceInfoRegistryProperty.SPDRP_DEVICEDESC, out _, buffer, length, out _);

			return buffer.ToString();
		}

		private static string GetFriendlyName(IntPtr hDevInfoSet, NativeMethods.DevInfoData devInfoData)
		{
			var buffer = new StringBuilder(256);
			var length = (uint)buffer.Capacity;
			NativeMethods.SetupDiGetDeviceRegistryProperty(hDevInfoSet, ref devInfoData, NativeMethods.DeviceInfoRegistryProperty.SPDRP_FRIENDLYNAME, out _, buffer, length, out _);

			return buffer.ToString();
		}

		private static string GetDeviceBusDescription(IntPtr hDeviceInfoSet, NativeMethods.DevInfoData deviceInfoData)
		{
			NativeMethods.DEVPROPKEY DEVPKEY_Device_BusReportedDeviceDesc = new NativeMethods.DEVPROPKEY() { Fmtid = new Guid(0x540b947e, 0x8b40, 0x45bc, 0xa8, 0xa2, 0x6a, 0x0b, 0x89, 0x4c, 0xbd, 0xa2), Pid = 4 };
			uint propRegDataType;
			byte[] ptrBuf = new byte[256];
			uint RequiredSize;
			bool success;
			try
			{
				success = NativeMethods.SetupDiGetDevicePropertyW(hDeviceInfoSet, ref deviceInfoData, ref DEVPKEY_Device_BusReportedDeviceDesc, out propRegDataType, ptrBuf, 256, out RequiredSize, 0);
			}
			catch (EntryPointNotFoundException)
			{
				return "";// windows < windows 7 go here (the bus reported device description just isn't available in winxp and below)
			}
			if (!success)
			{
				return "";
			}
			return System.Text.UnicodeEncoding.Unicode.GetString(ptrBuf, 0, (int)RequiredSize - utf16terminatorSize_bytes);
		}

		/*private static Dictionary<string, string> GetDeviceVidPid(IntPtr hDevInfoSet, NativeMethods.DevInfoData devInfoData)
		{
			var buffer = new StringBuilder(256);
			var length = (uint)buffer.Capacity;
			NativeMethods.SetupDiGetDeviceRegistryProperty(hDevInfoSet, ref devInfoData, NativeMethods.DeviceInfoRegistryProperty.SPDRP_HARDWAREID, out _, buffer, length, out _);


			var result = new Dictionary<string, string>();

			var regex = new Regex(@"(?<Enum>[^\\]*)\\((?<ID>[^&]+)&?)+"); //Matches 'USB\VID_123&PID_456&REV_001' or 'root\GenericDevice'

			var match = regex.Match(buffer.ToString());
			if (!match.Success || !match.Groups["ID"].Success) return result; //empty result. The ID group should always match if the match succeeded. But testing here for completeness.

			foreach (var id in match.Groups["ID"].Captures)
			{
				var splitIndex = id.ToString().IndexOf('_');
				if (splitIndex < 0) result.Add("GENERIC", id.ToString());
				else result.Add(id.ToString().Substring(0, splitIndex), id.ToString().Substring(splitIndex+1));
			} 

			return result;
		}*/

		/*
			//NativeMethods.DEVPROPKEY DEVPKEY_Device_LocationPaths = new NativeMethods.DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 37 };
https://github.com/vurdalakov/usbdevices/blob/master/src/UsbDevicesDotNet/UsbDeviceWinApi.DevicePropertyKeys.cs
		 */
	}
}
