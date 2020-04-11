namespace Win32PortEnumerate
{
	public class SerialportDevice
	{
		//public int? Vid { get; set; }
		//public int? Pid { get; set; }
		//public int? Rev { get; set; }
		public string Port { get; set; }
		public string Description { get; set; }
		public string FriendlyName { get; set; }
		public string BusReportedDeviceDescription { get; set; }
		public string DeviceID { get; set; }


		public override string ToString()
		{
			if (BusReportedDeviceDescription.Length > 0)
				return $"{Port} : {BusReportedDeviceDescription}";
			return $"{Port} : {Description}";
		}
	}
}
