#region License
/* Copyright(c) 2020 Gerald Elzing (gerald.elzing+pscontrol@gmail.com)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
**/
#endregion

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
	public static class SerialPortEnumerator
	{
		const int utf16terminatorSize_bytes = 2;

#if false
		public static List<SerialPortDevice> Enumerate()
		{
			List<SerialPortDevice> devices = new List<SerialPortDevice>();
			Guid[] guids = GetClassGUIDs("Ports");
			Guid[] addGuids = new Guid[] { new Guid(0x86e0d1e0, 0x8089, 0x11d0, 0x9c, 0xe4, 0x08, 0x00, 0x3e, 0x30, 0x1f, 0x73) };
			//GetClassGUIDs("Modem");
			int oldSize = guids.Length;
			Array.Resize<Guid>(ref guids, oldSize + addGuids.Length);
			addGuids.CopyTo(guids, oldSize);

			string port;
			string description;
			string busDescription;
			string deviceId;
			string devicePath;
			int size;
			for (int guidIndex = 0; guidIndex < guids.Length; guidIndex++)
			{
				Console.WriteLine("l1");
				IntPtr hDevInfoSet = NativeMethods.SetupDiGetClassDevs(ref guids[guidIndex], null, IntPtr.Zero, NativeMethods.DiGetClassFlags.DIGCF_PRESENT | NativeMethods.DiGetClassFlags.DIGCF_DEVICEINTERFACE);
				if (hDevInfoSet.ToInt64() == NativeMethods.INVALID_HANDLE_VALUE)
				{
					break;
				}
				Console.WriteLine("l2");
				IntPtr hParentsDevInfoSet = NativeMethods.SetupDiCreateDeviceInfoList(IntPtr.Zero, IntPtr.Zero);
				if (hParentsDevInfoSet.ToInt64() == NativeMethods.INVALID_HANDLE_VALUE)
				{
					break;
				}

				Console.WriteLine("l3");
				NativeMethods.SP_DEVICE_INTERFACE_DATA devInterfaceData = new NativeMethods.SP_DEVICE_INTERFACE_DATA();
				devInterfaceData.CbSize = (uint)Marshal.SizeOf(devInterfaceData);

				NativeMethods.SP_DEVINFO_DATA devInfoData = new NativeMethods.SP_DEVINFO_DATA();
				NativeMethods.SP_DEVINFO_DATA parentDevInfoData = new NativeMethods.SP_DEVINFO_DATA();
				devInfoData.CbSize = (uint)Marshal.SizeOf(devInfoData);
				parentDevInfoData.CbSize = (uint)Marshal.SizeOf(parentDevInfoData);

				Console.WriteLine("l4");
				try
				{
					bool ok = true;
					for (uint i = 0; ok; i++)
					{
						ok = NativeMethods.SetupDiEnumDeviceInfo(hDevInfoSet, i, ref devInfoData);
						if (!ok)
							break;

						Console.WriteLine("l5");
						port = GetPortName(hDevInfoSet, devInfoData);
						if (!port.StartsWith("COM")) continue;

						Console.WriteLine(port);
						busDescription = "";
						deviceId = "";
						devicePath = "";

						//get device path:
						try
						{
							Console.WriteLine("l6");
							ok = NativeMethods.SetupDiEnumDeviceInterfaces(hDevInfoSet, IntPtr.Zero, ref guids[guidIndex], i, ref devInterfaceData);
							if (ok)
							{
								//call with 0 to get the size:
								Console.WriteLine("l7");
								uint nRequiredSize;
								ok = NativeMethods.SetupDiGetDeviceInterfaceDetail(hDevInfoSet, devInterfaceData, IntPtr.Zero, 0, out nRequiredSize, devInfoData);
								Console.WriteLine("l8");
								if (ok == false)
								{
									ok = true;
									int error = Marshal.GetLastWin32Error();
									// expect that ok is false and that error is ERROR_INSUFFICIENT_BUFFER = 122, 
									// and nRequiredSize is the required size
									if (error != 122)
									{
										//throw new Win32Exception(error);
										Console.WriteLine(error);
										//goto skipDevicepath;
									}
								}

								Console.WriteLine("l9");
								// build a Device Interface Detail Data structure
								IntPtr didd = Marshal.AllocHGlobal((int)nRequiredSize);
								try
								{
									uint nBytes = nRequiredSize;
									Marshal.WriteInt32(didd, (IntPtr.Size == 4) ? (4 + Marshal.SystemDefaultCharSize) : 8);

									// now we can get some more detailed information
									if (NativeMethods.SetupDiGetDeviceInterfaceDetail(hDevInfoSet, devInterfaceData, didd, nBytes, out nRequiredSize, devInfoData))
									{
										devicePath = Marshal.PtrToStringAuto(new IntPtr(didd.ToInt32() + 4));
									}
								}
								finally
								{
									Marshal.FreeHGlobal(didd);
								}
							}
						}
						catch (Exception ex)
						{
							Console.WriteLine($"Failed to {ex.Message} enumerate USB serial devices. Error: [{Marshal.GetLastWin32Error()}] HR: [{Marshal.GetHRForLastWin32Error()}]");
						}
						//skipDevicepath:

						Console.WriteLine("l10");
						//get other info:
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

						var device = new SerialPortDevice
						{
							Port = port,
							Description = description,
							FriendlyName = GetFriendlyName(hDevInfoSet, devInfoData),
							BusReportedDeviceDescription = busDescription,
							DeviceID = deviceId,
							DevicePath = devicePath
							//Vid = id.ContainsKey("VID") ? (int?)int.Parse(id["VID"], System.Globalization.NumberStyles.HexNumber) : null,
							//Pid = id.ContainsKey("PID") ? (int?)int.Parse(id["PID"], System.Globalization.NumberStyles.HexNumber) : null,
							//Rev = id.ContainsKey("REV") ? (int?)int.Parse(id["REV"], System.Globalization.NumberStyles.HexNumber) : null
						};

						devices.Add(device);
					}

					if (!ok &&
						Marshal.GetLastWin32Error() != NativeMethods.NO_ERROR &&
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
			return devices;
		}
#else
		public static List<SerialPortDevice> Enumerate()
		{
			List<SerialPortDevice> devices = new List<SerialPortDevice>();

			Guid[] guids = GetClassGUIDs("Ports");
			Guid[] addGuids = GetClassGUIDs("Modem");
			int oldSize = guids.Length;
			Array.Resize<Guid>(ref guids, oldSize + addGuids.Length);
			addGuids.CopyTo(guids, oldSize);

			string port;
			string description;
			string busDescription;
			string deviceId;
			for (int guidIndex = 0; guidIndex < guids.Length; guidIndex++)
			{
				IntPtr hDevInfoSet = NativeMethods.SetupDiGetClassDevs(ref guids[guidIndex], null, IntPtr.Zero, NativeMethods.DiGetClassFlags.DIGCF_PRESENT);
				if (hDevInfoSet.ToInt64() == NativeMethods.INVALID_HANDLE_VALUE)
				{
					break;
				}
				IntPtr hParentsDevInfoSet = NativeMethods.SetupDiCreateDeviceInfoList(IntPtr.Zero, IntPtr.Zero);
				if (hParentsDevInfoSet.ToInt64() == NativeMethods.INVALID_HANDLE_VALUE)
				{
					break;
				}

				NativeMethods.SP_DEVINFO_DATA devInfoData = new NativeMethods.SP_DEVINFO_DATA();
				NativeMethods.SP_DEVINFO_DATA parentDevInfoData = new NativeMethods.SP_DEVINFO_DATA();
				devInfoData.CbSize = (uint)Marshal.SizeOf(devInfoData);
				parentDevInfoData.CbSize = (uint)Marshal.SizeOf(parentDevInfoData);

				try
				{
					for (uint i = 0; NativeMethods.SetupDiEnumDeviceInfo(hDevInfoSet, i, ref devInfoData); i++)
					{
						port = GetPortName(hDevInfoSet, devInfoData);
						if (port == null) continue;
						if (!port.StartsWith("COM")) continue;
						busDescription = "";
						deviceId = "";
						description = GetDescription(hDevInfoSet, devInfoData);
						NativeMethods.CM_Get_Device_ID_Size(out int size, devInfoData.DevInst);
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

						var device = new SerialPortDevice
						{
							Port = port,
							Description = description,
							FriendlyName = GetFriendlyName(hDevInfoSet, devInfoData),
							BusReportedDeviceDescription = busDescription,
							DeviceID = deviceId
						};

						devices.Add(device);
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
			return devices;
		}
#endif

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
					NativeMethods.SetupDiClassGuidsFromName(className, ref guidArray[0], requiredSize, out _);
				}
			}
			else
				throw new System.ComponentModel.Win32Exception();

			return guidArray;
		}

		private static string GetPortName(IntPtr hDevInfoSet, NativeMethods.SP_DEVINFO_DATA devInfoData)
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

		private static string GetDescription(IntPtr hDevInfoSet, NativeMethods.SP_DEVINFO_DATA devInfoData)
		{
			var buffer = new StringBuilder(256);
			var length = (uint)buffer.Capacity;
			NativeMethods.SetupDiGetDeviceRegistryProperty(hDevInfoSet, ref devInfoData, NativeMethods.DeviceInfoRegistryProperty.SPDRP_DEVICEDESC, out _, buffer, length, out _);

			return buffer.ToString();
		}

		private static string GetFriendlyName(IntPtr hDevInfoSet, NativeMethods.SP_DEVINFO_DATA devInfoData)
		{
			var buffer = new StringBuilder(256);
			var length = (uint)buffer.Capacity;
			NativeMethods.SetupDiGetDeviceRegistryProperty(hDevInfoSet, ref devInfoData, NativeMethods.DeviceInfoRegistryProperty.SPDRP_FRIENDLYNAME, out _, buffer, length, out _);

			return buffer.ToString();
		}

		private static string GetDeviceBusDescription(IntPtr hDeviceInfoSet, NativeMethods.SP_DEVINFO_DATA deviceInfoData)
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

		/*
			//NativeMethods.DEVPROPKEY DEVPKEY_Device_LocationPaths = new NativeMethods.DEVPROPKEY() { Fmtid = new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), Pid = 37 };
https://github.com/vurdalakov/usbdevices/blob/master/src/UsbDevicesDotNet/UsbDeviceWinApi.DevicePropertyKeys.cs
		 */
	}
}
