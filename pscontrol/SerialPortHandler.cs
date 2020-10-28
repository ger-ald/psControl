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
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
using Win32SerialPort;

namespace pscontrol
{
	class SerialPortHandler
	{
		private const int SERIAL_RECV_LEN_HINT = 20;//aproximate size of most rx messages
		private const int SERIAL_RECV_LEN_MAX = 1000;//max size of any rx messages

		private int serialPortByteTimeout;

		private readonly BetterSerialPort serialPort;
		private Thread serThread = null;

		private CancellationTokenSource stopSerThread_Token;

		public class SerialTask
		{
			[System.ComponentModel.Bindable(true)]
			[System.ComponentModel.TypeConverter(typeof(System.ComponentModel.StringConverter))]
			public object Tag;

			public string Send;
			public int WaitTime;
			public string Recv;

			public SerialTask(object tag, string toSend, int waitTime)
			{
				this.Tag = tag;
				this.Send = toSend;
				this.WaitTime = waitTime;
				this.Recv = null;
			}
		}

		private BlockingCollection<SerialTask> toSerThreadQueue;
		private BlockingCollection<SerialTask> fromSerThreadQueue;

		public event EventHandler SerialPortBroke;


		


		public SerialPortHandler(BetterSerialPort serialPort, int byteTimeout = 1)
		{
			this.serialPort = serialPort;
			serialPortByteTimeout = byteTimeout;
			toSerThreadQueue = new BlockingCollection<SerialTask>();
			fromSerThreadQueue = new BlockingCollection<SerialTask>();
		}




		private void NotifySerPortError()
		{
			//error, dont stop thread (it already stopped)
			try
			{
				serialPort.Close();
			}
			catch
			{
				// ...
			}

			//notify eventhandlers
			SerialPortBroke?.Invoke(this, EventArgs.Empty);
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
			serThread = null;
		}

		private void SerThread()
		{
			while (!stopSerThread_Token.Token.IsCancellationRequested)
			{
				SerialTask task;
				try
				{
					task = toSerThreadQueue.Take(stopSerThread_Token.Token);
				}
				catch
				{
					break;
				}
				if (!stopSerThread_Token.Token.IsCancellationRequested)
				{
					int bytesWritten = serialPort.Write(task.Send);
					if (bytesWritten < 0)
					{
						break;
					}
					if (task.WaitTime > 0)
					{
						task.Recv = "";
						string temp = serialPort.ReadString(SERIAL_RECV_LEN_HINT, (uint)task.WaitTime, 2);
						if (temp != null)
						{
							//no serport error ocurred
							task.Recv = temp;
						}
						


						fromSerThreadQueue.Add(task);
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






		public void Open(string portname, uint baudRate)
		{
			try
			{
				serialPort.Open(portname, baudRate);
			}
			catch (Exception ex)
			{
				//port is already opened by another application OR
				//tried to open a removed port
				throw ex;
			}
			toSerThreadQueue.ClearQueue();
			fromSerThreadQueue.ClearQueue();
			StartSerThread();
		}

		public bool IsOpen
		{
			get { return serialPort.IsOpen; }
		}

		public void Close()
		{
			StopSerThread();
			serialPort.Close();
		}

		public int SendCount
		{
			get { return toSerThreadQueue.Count; }
		}

		public void SendResetCount()
		{
			toSerThreadQueue.ClearQueue();
		}

		public void Send(SerialTask task)
		{
			if (IsOpen) toSerThreadQueue.Add(task);
		}

		public int RecvCount
		{
			get { return fromSerThreadQueue.Count; }
		}

		public bool TryRecv(out SerialTask task)
		{
			return fromSerThreadQueue.TryTake(out task);
		}

		public bool TryRecv(out SerialTask task, int millisecondsTimeout, CancellationToken cancellationToken)
		{
			return fromSerThreadQueue.TryTake(out task, millisecondsTimeout, cancellationToken);
		}
	}
}
