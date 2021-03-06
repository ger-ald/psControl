﻿#region License
/* Copyright(c) 2020 Gerald Elzing
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

namespace Win32PortEnumerate
{
	public class SerialPortDevice
	{
		//public int? Vid { get; set; }
		//public int? Pid { get; set; }
		//public int? Rev { get; set; }
		public string Port { get; set; }
		public string Description { get; set; }
		public string FriendlyName { get; set; }
		public string BusReportedDeviceDescription { get; set; }
		public string DeviceID { get; set; }
		//public string DevicePath { get; set; }


		public override string ToString()
		{
			if (BusReportedDeviceDescription.Length > 0)
				return $"{Port} : {BusReportedDeviceDescription}";
			return $"{Port} : {Description}";
		}
	}
}
