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
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace pscontrol
{
	class SerialPortHandler
	{
		private const int SERIAL_RECV_LEN = 20;

		private readonly SerialPort serialPort;
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

		public delegate void SerialBroke_Delegate();
		public event SerialBroke_Delegate SerialPortBroke;


		


		public SerialPortHandler(SerialPort serialPort)
		{
			this.serialPort = serialPort;
			toSerThreadQueue = new BlockingCollection<SerialTask>();
			fromSerThreadQueue = new BlockingCollection<SerialTask>();
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
			serThread = null;
		}

		private void SerThread()
		{
			while (serialPort.IsOpen)
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
					try
					{
						serialPort.Write(task.Send);
					}
					catch
					{
						break;
					}
					if (task.WaitTime > 0)
					{
						Stopwatch noAnswerTimeout = new Stopwatch();
						noAnswerTimeout.Start();
						task.Recv = "";
						while ((task.Recv.Length == 0) && (noAnswerTimeout.ElapsedMilliseconds < task.WaitTime))
						{
							string temp = SerialPortBlockingRead(serialPort);
							if (temp == null) break;//serport error ocurred
							task.Recv += temp;
						}
						noAnswerTimeout.Stop();
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
			catch (InvalidOperationException)
			{
				//comport is closed
				return null;
			}
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
			toSerThreadQueue.ClearQueue();
			fromSerThreadQueue.ClearQueue();
		}

		public int SendCount
		{
			get { return toSerThreadQueue.Count; }
		}

		public void Send(SerialTask task)
		{
			if (serialPort.IsOpen) toSerThreadQueue.Add(task);
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
