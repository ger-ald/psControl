using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Management;
using System.Collections.Generic;

namespace pscontrol
{
	class SerialPortHandler
	{
		private const int SERIAL_RECV_LEN = 20;

		private readonly SerialPort serialPort;
		private Thread serThread = null;

		private CancellationTokenSource stopSerThread_Token;

		public struct ComPortEntry
		{
			public string port;
			public string description;

			public ComPortEntry(string port, string description)
			{
				this.port = port;
				this.description = description;
			}
		}
		public struct Serialtask
		{
			public int type;
			public string send;
			public int waittime;
			public string recv;

			public Serialtask(int type, string tosend, int waittime)
			{
				this.type = type;
				send = tosend;
				this.waittime = waittime;
				recv = null;
			}
		}

		private BlockingCollection<Serialtask> toserthreadqueue;
		private BlockingCollection<Serialtask> fromserthreadqueue;

		public delegate void SerialBroke_Delegate();
		public event SerialBroke_Delegate SerialPortBroke;


		


		public SerialPortHandler(SerialPort serialPort)
		{
			this.serialPort = serialPort;
			toserthreadqueue = new BlockingCollection<Serialtask>();
			fromserthreadqueue = new BlockingCollection<Serialtask>();
		}



		public static ComPortEntry[] WmiDetectComs()
		{
			using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE ClassGuid='{4d36e978-e325-11ce-bfc1-08002be10318}'"))
			{
				List<ComPortEntry> comlist = new List<ComPortEntry>();
				string caption = "";
				string port = "";
				int portnameindex;
				foreach (ManagementObject obj in searcher.Get())
				{
					if (obj != null)
					{
						object captionObj = obj["Caption"];
						if (captionObj != null)
						{
							caption = captionObj.ToString();
							portnameindex = caption.LastIndexOf(" (COM");
							if (portnameindex > 0)
							{
								port = caption.Substring(portnameindex + 2).Replace(")", string.Empty);
								comlist.Add(new ComPortEntry(port, caption.Substring(0, portnameindex)));
							}
						}
					}
				}
				//comlist.Sort();
				return comlist.ToArray();
			}
		}




		private void NotifySerPortError()
		{
			//error, dont stop thread (it already stopped)
			try
			{
				if (serialPort.IsOpen) serialPort.Close();
			}
			catch
			{
				// ...
			}

			//notify eventhandlers
			SerialPortBroke?.Invoke();
		}

		private void StartSerThread()
		{
			if ((serThread != null) && serThread.IsAlive) return;
			stopSerThread_Token?.Dispose();
			stopSerThread_Token = new CancellationTokenSource();
			serThread = new Thread(SerThread);
			serThread.Start();
		}

		private void StopSerThread()
		{
			if (serThread == null) return;
			if (!serThread.IsAlive) return;
			stopSerThread_Token.Cancel();
			serThread.Join();
		}

		private void SerThread()
		{
			while (serialPort.IsOpen)
			{
				Serialtask task;
				try
				{
					task = toserthreadqueue.Take(stopSerThread_Token.Token);
				}
				catch
				{
					break;
				}
				if (!stopSerThread_Token.Token.IsCancellationRequested)
				{
					try
					{
						serialPort.Write(task.send);
					}
					catch
					{
						break;
					}
					if (task.waittime > 0)
					{
						Stopwatch noAnswerTimeout = new Stopwatch();
						noAnswerTimeout.Start();
						task.recv = "";
						while ((task.recv.Length == 0) && (noAnswerTimeout.ElapsedMilliseconds < task.waittime))
						{
							string temp = SerialPortBlockingRead(serialPort);
							if (temp == null) break;//serport error ocurred
							task.recv += temp;
						}
						noAnswerTimeout.Stop();
						fromserthreadqueue.Add(task);
					}
				}
			}
			if (!stopSerThread_Token.Token.IsCancellationRequested)
			{
				//if we're not volantarily stopping, notify the error
				Console.WriteLine("[SerThread] port error");

				NotifySerPortError();
			}
			Console.WriteLine("[SerThread] exit");
		}

		/// <summary>
		/// blocking receive bytes from the serialport
		/// </summary>
		/// <param name="port">what port to use</param>
		/// <returns>null on serialport error, received string otherwise</returns>
		private static string SerialPortBlockingRead(SerialPort port)
		{
			var recv = new StringBuilder(SERIAL_RECV_LEN);
			try
			{
				for (int i = 0; i < SERIAL_RECV_LEN; i++)
				{
					recv.Append((char)port.ReadChar());
				}
			}
			catch (TimeoutException)
			{
				//this means the receive 'packet' is complete
			}
			catch (System.IO.IOException)
			{
				//comport got disconnected
				return null;
			}
			//catch (UnauthorizedAccessException)
			//{
				//i can't remember what this was for, so i'll leave it out for now
			//	return null;
			//}
			return recv.ToString();
		}






		public void Open(string portname)
		{
			serialPort.PortName = portname;
			serialPort.Open();
			StartSerThread();
		}

		public bool IsOpen
		{
			get { return serialPort.IsOpen; }
		}

		public void Close()
		{
			StopSerThread();
			if (serialPort.IsOpen) serialPort.Close();	//now empty the queues
			toserthreadqueue.ClearQueue();
			fromserthreadqueue.ClearQueue();
		}

		public int SendCount
		{
			get { return toserthreadqueue.Count; }
		}

		public void Send(Serialtask task)
		{
			if (serialPort.IsOpen) toserthreadqueue.Add(task);
		}

		public int RecvCount
		{
			get { return fromserthreadqueue.Count; }
		}

		public bool TryRecv(out Serialtask task)
		{
			return fromserthreadqueue.TryTake(out task);
		}
	}
}
