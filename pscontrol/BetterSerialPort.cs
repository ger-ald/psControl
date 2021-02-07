using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Win32SerialPort
{
	class BetterSerialPort : IDisposable
	{
		private const long INVALID_HANDLE_VALUE = -1;

		#region Enum
		public enum StopBits
		{
			None,
			One,
			Two,
			OnePointFive,
		}

		public enum Parity
		{
			None,
			Odd,
			Even,
			Mark,
			Space,
		}
		#endregion
		#region Fields
		/// <summary>
		/// The baud rate at which the communications device operates.
		/// </summary>
		private uint iBaudRate;

		/// <summary>
		/// The number of bits in the bytes to be transmitted and received.
		/// </summary>
		private readonly byte byteSize;

		/// <summary>
		/// The system handle to the serial port connection ('file' handle).
		/// </summary>
		private IntPtr pHandle = (IntPtr)INVALID_HANDLE_VALUE;

		/// <summary>
		/// The parity scheme to be used.
		/// </summary>
		private readonly Parity parity;

		/// <summary>
		/// The name of the serial port to connect to.
		/// </summary>
		private string sPortName;

		/// <summary>
		/// The number of bits in the bytes to be transmitted and received.
		/// </summary>
		private readonly StopBits stopBits;


		COMMTIMEOUTS timeouts = new COMMTIMEOUTS();
		#endregion

		#region Constructor
		/// <summary>
		/// Creates a new instance of SerialCom.
		/// </summary>
		/// <param>The number of stop bits to be used</param>
		/// <param>The parity scheme to be used</param>
		/// <param>The number of bits in the bytes to be transmitted and received</param>
		public BetterSerialPort(StopBits stopBits, Parity parity, byte byteSize = 8)
		{
			if (stopBits == StopBits.None)
				throw new ArgumentException("stopBits cannot be StopBits.None", "stopBits");
			if (byteSize < 5 || byteSize > 8)
				throw new ArgumentOutOfRangeException("The number of data bits must be 5 to 8 bits.", "byteSize");
			//if (baudRate < 110 || baudRate > 256000)
			//	throw new ArgumentOutOfRangeException("Invalid baud rate specified.", "baudRate");
			if ((byteSize == 5 && stopBits == StopBits.Two) || (stopBits == StopBits.OnePointFive && byteSize > 5))
				throw new ArgumentException("The use of 5 data bits with 2 stop bits is an invalid combination, " +
					"as is 6, 7, or 8 data bits with 1.5 stop bits.");

			this.sPortName = "";
			this.iBaudRate = 9600;
			this.byteSize = byteSize;
			this.stopBits = stopBits;
			this.parity = parity;
		}
		#endregion

		#region Open
		/// <summary>
		/// Opens and initializes the serial connection.
		/// </summary>
		/// <param>The name of the serial port to connect to</param>
		/// <param>The baud rate at which the communications device operates</param>
		/// <returns>Whether or not the operation succeeded</returns>
		public void Open(string portName, uint baudRate)
		{
			if (portName.StartsWith(@"\\"))
				this.sPortName = portName;
			else
				this.sPortName = @"\\.\" + portName;
			this.iBaudRate = baudRate;

			pHandle = CreateFile(this.sPortName, FileAccess.ReadWrite, FileShare.None,
				IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
			if (pHandle == (IntPtr)INVALID_HANDLE_VALUE)
			{
				string lastError = GetLastErrorAsString();
				throw new Exception("BetterSerialPort.Open(); " + lastError);
			}

			ConfigureSerialPort();
		}
		#endregion

		public bool IsOpen
		{
			get { return (pHandle != (IntPtr)INVALID_HANDLE_VALUE); }
		}

		#region Write
		/// <summary>
		/// Transmits the specified array of bytes.
		/// </summary>
		/// <param>The bytes to write</param>
		/// <returns>The number of bytes written (-1 if error)</returns>
		public int Write(byte[] data)
		{
			FailIfNotConnected();
			if (data == null) return 0;

			if (WriteFile(pHandle, data, data.Length, out int bytesWritten, 0))
				return bytesWritten;
			return -1;
		}

		/// <summary>
		/// Transmits the specified string.
		/// </summary>
		/// <param>The string to write</param>
		/// <returns>The number of bytes written (-1 if error)</returns>
		public int Write(string data)
		{
			FailIfNotConnected();

			// convert the string to bytes
			byte[] bytes;
			if (data == null)
			{
				bytes = null;
			}
			else
			{
				bytes = Encoding.UTF8.GetBytes(data);
			}

			return Write(bytes);
		}

		/// <summary>
		/// Transmits the specified string and appends the carriage return to the end
		/// if it does not exist.
		/// </summary>
		/// <remarks>
		/// Note that the string must end in '\r\n' before any serial device will interpret the data
		/// sent. For ease of programmability, this method should be used instead of Write() when you
		/// want to automatically execute the specified command string.
		/// </remarks>
		/// <param>The string to write</param>
		/// <returns>The number of bytes written (-1 if error)</returns>
		public int WriteLine(string data)
		{
			if (data != null && !data.EndsWith("\r\n"))
				data += "\r\n";
			return Write(data);
		}
		#endregion

		#region Read
		/// <summary>
		/// Reads any bytes that have been received and writes them to the specified array.
		/// </summary>
		/// <param>The array to write the read data to</param>
		/// <returns>The number of bytes read (-1 if error)</returns>
		public int Read(int count, out byte[] data, uint msgTimeout, uint byteTimeout)
		{
			FailIfNotConnected();
			data = null;

			// set the serial connection timeouts
			timeouts.ReadIntervalTimeout = byteTimeout;
			timeouts.ReadTotalTimeoutMultiplier = 0;
			timeouts.ReadTotalTimeoutConstant = msgTimeout;
			timeouts.WriteTotalTimeoutMultiplier = 0;
			timeouts.WriteTotalTimeoutConstant = 1000;
			if (!SetCommTimeouts(pHandle, ref timeouts))
			{
				return -1;
			}

			data = new byte[count];
			if (ReadFile(pHandle, data, data.Length, out int bytesRead, 0))
				return bytesRead;
			return -1;
		}

		/// <summary>
		/// Reads any data that has been received as a string.
		/// </summary>
		/// <param>The maximum number of bytes to read</param>
		/// <returns>The data received (null if no data)</returns>
		public string ReadString(int maxBytesToRead, uint msgTimeout, uint byteTimeout)
		{
			if (maxBytesToRead < 1) throw new ArgumentOutOfRangeException("maxBytesToRead");

			int numBytes = Read(maxBytesToRead, out byte[] bytes, msgTimeout, byteTimeout);
			if (numBytes < 0)
			{
				return null;
			}
			return Encoding.UTF8.GetString(bytes, 0, numBytes);
		}
		#endregion

		#region Dispose Utils
		/// <summary>
		/// Disconnects and disposes of the SerialCom instance.
		/// </summary>
		public void Close()
		{
			Dispose();
		}

		public void Dispose()
		{
			if (pHandle != (IntPtr)INVALID_HANDLE_VALUE)
			{
				CloseHandle(pHandle);
				pHandle = (IntPtr)INVALID_HANDLE_VALUE;
			}
		}

		/// <summary>
		/// Flushes the serial I/O buffers.
		/// </summary>
		/// <returns>Whether or not the operation succeeded</returns>
		public bool Flush()
		{
			FailIfNotConnected();

			const int PURGE_RXCLEAR = 0x0008; // input buffer
			const int PURGE_TXCLEAR = 0x0004; // output buffer
			return PurgeComm(pHandle, PURGE_RXCLEAR | PURGE_TXCLEAR);
		}
		#endregion

		#region Private Helpers
		/// <summary>
		/// Configures the serial device based on the connection parameters pased in by the user.
		/// </summary>
		/// <returns>Whether or not the operation succeeded</returns>
		private void ConfigureSerialPort()
		{
			DCB serialConfig = new DCB();
			if (!GetCommState(pHandle, ref serialConfig))
			{
				Close();
				string lastError = GetLastErrorAsString();
				throw new Exception("BetterSerialPort.Open():GetCommState " + lastError);
			}

			// setup the DCB struct with the serial settings we need
			serialConfig.BaudRate = this.iBaudRate;
			serialConfig.ByteSize = this.byteSize;
			serialConfig.fBinary = 1; // must be true
			serialConfig.fDtrControl = 1; // DTR_CONTROL_ENABLE "Enables the DTR line when the device is opened and leaves it on."
			serialConfig.fAbortOnError = 0; // false
			serialConfig.fTXContinueOnXoff = 0; // false

			serialConfig.fParity = 1; // true so that the Parity member is looked at
			switch (this.parity)
			{
				case Parity.Even:
					serialConfig.Parity = 2;
					break;
				case Parity.Mark:
					serialConfig.Parity = 3;
					break;
				case Parity.Odd:
					serialConfig.Parity = 1;
					break;
				case Parity.Space:
					serialConfig.Parity = 4;
					break;
				case Parity.None:
				default:
					serialConfig.Parity = 0;
					break;
			}
			switch (this.stopBits)
			{
				case StopBits.One:
					serialConfig.StopBits = 0;
					break;
				case StopBits.OnePointFive:
					serialConfig.StopBits = 1;
					break;
				case StopBits.Two:
					serialConfig.StopBits = 2;
					break;
				case StopBits.None:
				default:
					throw new ArgumentException("stopBits cannot be StopBits.None");
			}

			if (!SetCommState(pHandle, ref serialConfig))
			{
				//this might happen because of the baudrate (the comport driver says if it supports it or not)
				Close();
				int lastErrorInt = Marshal.GetLastWin32Error();
				if (lastErrorInt == 87)
				{
					throw new Exception("BetterSerialPort.Open() Invalid baudrate for this comport");
				}
				string lastErrorString = GetLastErrorAsString();
				throw new Exception("BetterSerialPort.Open():SetCommState " + lastErrorString);
			}

			// set the serial connection timeouts
			timeouts.ReadIntervalTimeout = 1;
			timeouts.ReadTotalTimeoutMultiplier = 0;
			timeouts.ReadTotalTimeoutConstant = 1000;
			timeouts.WriteTotalTimeoutMultiplier = 0;
			timeouts.WriteTotalTimeoutConstant = 1000;
			if (!SetCommTimeouts(pHandle, ref timeouts))
			{
				Close();
				string lastError = GetLastErrorAsString();
				throw new Exception("BetterSerialPort.Open():SetCommTimeouts " + lastError);
			}
		}

		/// <summary>
		/// Helper that throws a InvalidOperationException if we don't have a serial connection.
		/// </summary>
		private void FailIfNotConnected()
		{
			if (pHandle == (IntPtr)INVALID_HANDLE_VALUE)
				throw new InvalidOperationException("You must be connected to the serial port before performing this operation.");
		}

		/// <summary>
		/// Helper that throws a InvalidOperationException if we don't have a serial connection.
		/// </summary>
		private string GetLastErrorAsString()
		{
			return new Win32Exception(Marshal.GetLastWin32Error()).Message;
		}
		#endregion

		#region Native Helpers
		#region Native structures
		/// <summary>
		/// Contains the time-out parameters for a communications device.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		struct COMMTIMEOUTS
		{
			public uint ReadIntervalTimeout;
			public uint ReadTotalTimeoutMultiplier;
			public uint ReadTotalTimeoutConstant;
			public uint WriteTotalTimeoutMultiplier;
			public uint WriteTotalTimeoutConstant;
		}

		/// <summary>
		/// Defines the control setting for a serial communications device.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		struct DCB
		{
			public int DCBlength;
			public uint BaudRate;
			public uint Flags;
			public ushort wReserved;
			public ushort XonLim;
			public ushort XoffLim;
			public byte ByteSize;
			public byte Parity;
			public byte StopBits;
			public sbyte XonChar;
			public sbyte XoffChar;
			public sbyte ErrorChar;
			public sbyte EofChar;
			public sbyte EvtChar;
			public ushort wReserved1;
			public uint fBinary;
			public uint fParity;
			public uint fOutxCtsFlow;
			public uint fOutxDsrFlow;
			public uint fDtrControl;
			public uint fDsrSensitivity;
			public uint fTXContinueOnXoff;
			public uint fOutX;
			public uint fInX;
			public uint fErrorChar;
			public uint fNull;
			public uint fRtsControl;
			public uint fAbortOnError;
		}
		#endregion

		#region Native Methods
		// Used to get a handle to the serial port so that we can read/write to it.
		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		static extern IntPtr CreateFile(string fileName,
		   [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
		   [MarshalAs(UnmanagedType.U4)] FileShare fileShare,
		   IntPtr securityAttributes,
		   [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
		   int flags,
		   IntPtr template);

		// Used to close the handle to the serial port.
		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool CloseHandle(IntPtr hObject);

		// Used to get the state of the serial port so that we can configure it.
		[DllImport("kernel32.dll")]
		static extern bool GetCommState(IntPtr hFile, ref DCB lpDCB);

		// Used to configure the serial port.
		[DllImport("kernel32.dll")]
		static extern bool SetCommState(IntPtr hFile, [In] ref DCB lpDCB);

		// Used to set the connection timeouts on our serial connection.
		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool SetCommTimeouts(IntPtr hFile, ref COMMTIMEOUTS lpCommTimeouts);

		// Used to read bytes from the serial connection.
		[DllImport("kernel32.dll")]
		static extern bool ReadFile(IntPtr hFile, byte[] lpBuffer,
		   int nNumberOfBytesToRead, out int lpNumberOfBytesRead, int lpOverlapped);

		// Used to write bytes to the serial connection.
		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer,
			int nNumberOfBytesToWrite, out int lpNumberOfBytesWritten, int lpOverlapped);

		// Used to flush the I/O buffers.
		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool PurgeComm(IntPtr hFile, int dwFlags);


		//used to get the win32 errors as a string
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern int FormatMessageW(
			uint dwFormatFlags, IntPtr lpSource, int dwMessageId,
			int dwLanguageId, out IntPtr MsgBuffer, int nSize, IntPtr Arguments);

		#endregion
		#endregion
	}
}
