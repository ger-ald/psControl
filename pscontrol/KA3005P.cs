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
using System.Globalization;
using System.IO.Ports;
using System.Threading;

namespace pscontrol
{
	public class KA3005P
	{
		private const int SERIAL_RECV_TIMEOUT_KEEP = 250;//ms. time to wait for a reply after sending a cmd
		private const int SERIAL_RECV_TIMEOUT_TOSS = 45;//ms. time to wait (for a reply) after sending a cmd to give psu time to process
		private const int SERIAL_MAXEMPTYREPLIES = 4;//after thismany unexpected empty replies, the psu is considered lost. (if psu is disconnected the serial cmd's will timeout and thus come back empty even when we expected a reply)

		private enum SerialMsgType
		{
			INITOUTENA,
			STATUS,
			VSET,
			ISET,
			VOUT,
			IOUT,
			VGET,
			IGET,
			DONTCARE
		};

		private readonly SerialPortHandler serport1;

		private int setpointVoltage = -1;// = volt* 100
		private int setpointCurrent = -1;// = amp *1000
		private int prevSetVoltage = -1;// = volt* 100
		private int prevSetCurrent = -1;// = amp *1000
		private double currentVoltage = 0.0;
		private double currentCurrent = 0.0;
		private bool currentOutEnabled = false;
		private int consecutiveEmptyReplies = 0;


		/// <summary>
		/// This event is called when the setpoints are changed by the psu, for example: right after connecting or recalling from psu memory.
		/// </summary>
		public event EventHandler OnSetpointUpdate;
		public event EventHandler OnOutputUpdate;
		public event EventHandler OnSurpriseDisconnect;

		private Thread otherThread = null;
		private CancellationTokenSource stopOtherThread_CancelToken;

		/// <summary>
		/// Voltage setpoint of the psu.
		/// </summary>
		public double SetpointV
		{
			get
			{
				return (double)setpointVoltage / 100;
			}

			set
			{
				setpointVoltage = (int)((value + 0.005) * 100);
			}
		}

		/// <summary>
		/// Current setpoint of the psu.
		/// </summary>
		public double SetpointI
		{
			get
			{
				return (double)setpointCurrent / 1000;
			}

			set
			{
				setpointCurrent = (int)((value + 0.0005) * 1000);
			}
		}

		/// <summary>
		/// Actual output voltage reported by the psu.
		/// </summary>
		public double OutputV
		{
			get
			{
				return currentVoltage;
			}
		}
	
		/// <summary>
		/// Actual output current reported by the psu.
		/// </summary>
		public double OutputI
		{
			get
			{
				return currentCurrent;
			}
		}
	
		/// <summary>
		/// Set: enable or disable psu output, Get: psu reported output state.
		/// </summary>
		public bool OutputEnabled
		{
			get
			{
				return currentOutEnabled;
			}

			set
			{
				serport1.Send(new SerialPortHandler.SerialTask(SerialMsgType.DONTCARE, "OUT" + (value ? "1" : "0"), SERIAL_RECV_TIMEOUT_TOSS));
			}
		}
		public bool IsInCVmode { get; private set; } = false;
		public bool IsInCCmode { get; private set; } = false;


		public KA3005P()
		{
			SerialPort serialPort1 = new SerialPort
			{
				BaudRate = 9600,
				DataBits = 8,
				Parity = Parity.None,
				StopBits = StopBits.One,
				Handshake = Handshake.None,
				ReadTimeout = 20
			};
			serport1 = new SerialPortHandler(serialPort1);
			serport1.SerialPortBroke += Serport1_SerialPortBroke;
			SetStatusIndicators(0);
		}

		private void StartOtherThread()
		{
			if ((otherThread != null) && otherThread.IsAlive) return;
			stopOtherThread_CancelToken?.Dispose();
			stopOtherThread_CancelToken = new CancellationTokenSource();
			otherThread = new Thread(OtherThread);
			otherThread.Start();
		}

		private void StopOtherThread()
		{
			if (otherThread == null) return;
			if (!otherThread.IsAlive) return;
			stopOtherThread_CancelToken.Cancel();
			otherThread.Join();
			otherThread = null;
		}

		private void OtherThread()
		{
			SerialPortHandler.SerialTask task = null;
			while (serport1.IsOpen && !stopOtherThread_CancelToken.Token.IsCancellationRequested)
			{
				//send new commands if the transmit queue is empty: (avoids creating a large backlog because of differing transfer rates)
				if (serport1.SendCount == 0)
				{
					if (setpointVoltage != prevSetVoltage)
					{
						prevSetVoltage = setpointVoltage;
						serport1.Send(new SerialPortHandler.SerialTask(SerialMsgType.VSET, "VSET1:" + ((double)setpointVoltage / 100).ToString("00.00", new CultureInfo("en-US")), SERIAL_RECV_TIMEOUT_TOSS));
					}
					if (setpointCurrent != prevSetCurrent)
					{
						prevSetCurrent = setpointCurrent;
						serport1.Send(new SerialPortHandler.SerialTask(SerialMsgType.ISET, "ISET1:" + ((double)setpointCurrent / 1000).ToString("0.000", new CultureInfo("en-US")), SERIAL_RECV_TIMEOUT_TOSS));
					}
					serport1.Send(new SerialPortHandler.SerialTask(SerialMsgType.STATUS, "STATUS?", SERIAL_RECV_TIMEOUT_KEEP));
					serport1.Send(new SerialPortHandler.SerialTask(SerialMsgType.VOUT, "VOUT1?", SERIAL_RECV_TIMEOUT_KEEP));
					serport1.Send(new SerialPortHandler.SerialTask(SerialMsgType.IOUT, "IOUT1?", SERIAL_RECV_TIMEOUT_KEEP));
				}

				bool msgReceived = false;
				try
				{
					msgReceived = serport1.TryRecv(out task, 100, stopOtherThread_CancelToken.Token);
				}
				catch
				{
					// OperationCanceledException
				}
				if (msgReceived)
				{
					//handle all command replies:
					//Console.WriteLine("{0}: {1} -> {2}     {3} : {4}", task.Tag, ToLiteral(task.Send), ToLiteral(task.Recv), serport1.SendCount, serport1.RecvCount);

					if (task.Recv.Length == 0)
					{
						if (task.WaitTime != SERIAL_RECV_TIMEOUT_TOSS)
						{
							//we expected a reply but didn't get one
							consecutiveEmptyReplies++;
							if (consecutiveEmptyReplies >= SERIAL_MAXEMPTYREPLIES)
							{
								//psu is considered not connected anymore
								break;
							}
						}
						continue;
					}
					else if (task.WaitTime != SERIAL_RECV_TIMEOUT_TOSS)
					{
						consecutiveEmptyReplies = 0;
					}
					if (((SerialMsgType)task.Tag != SerialMsgType.STATUS) && (!char.IsDigit(task.Recv[0]))) continue;//filter misformed replies
					double tempvalue;
					switch (task.Tag)
					{
						//received cc/cv and output enabled state from psu (happens frequent)
						case SerialMsgType.STATUS:
							SetStatusIndicators((byte)task.Recv[0]);
							break;
						//received actual v/i output from psu (happens frequent)
						case SerialMsgType.VOUT:
							if (double.TryParse(task.Recv, NumberStyles.Any, new CultureInfo("en-US"), out currentVoltage))
							{
								// nothing...
							}
							break;
						case SerialMsgType.IOUT:
							if (double.TryParse(task.Recv, NumberStyles.Any, new CultureInfo("en-US"), out currentCurrent))
							{
								//v and i received, so notify eventhandlers: (when we send requests the V is asked first so here we should have received V already)
								OnOutputUpdate?.Invoke(this, EventArgs.Empty);
							}
							break;

						//received setpoint v/i output from psu (happens at connect only)
						case SerialMsgType.VGET:
							if (double.TryParse(task.Recv, NumberStyles.Any, new CultureInfo("en-US"), out tempvalue))
							{
								this.SetpointV = tempvalue;
								prevSetVoltage = setpointVoltage;
							}
							break;
						case SerialMsgType.IGET:
							if (double.TryParse(task.Recv, NumberStyles.Any, new CultureInfo("en-US"), out tempvalue))
							{
								this.SetpointI = tempvalue;
								prevSetCurrent = setpointCurrent;
								//v and i received, so notify eventhandlers:
								OnSetpointUpdate?.Invoke(this, EventArgs.Empty);
							}
							break;

						default:
							break;
					}
				}
			}
			if (!stopOtherThread_CancelToken.Token.IsCancellationRequested)
			{
				//if we're not volantarily stopping, notify the error
				Console.WriteLine("[OtherThread] error");

				if (serport1.IsOpen) serport1.Close();
				NotifyOnSurpriseDisconnect();
			}
			Console.WriteLine("[OtherThread] exit");
		}




		private void NotifyOnSurpriseDisconnect()
		{
			//notify eventhandlers
			OnSurpriseDisconnect?.Invoke(this, EventArgs.Empty);
		}

		private void Serport1_SerialPortBroke(object sender, EventArgs e)
		{
			//this will cause otherThread to stop and call NotifyOnDisconnect()
			if (serport1.IsOpen) serport1.Close();
		}

		private void SetStatusIndicators(byte status)
		{
			currentOutEnabled = (status & 0x40) != 0;
			IsInCVmode = ((status & 0x01) != 0) && currentOutEnabled;
			IsInCCmode = (!IsInCVmode) && currentOutEnabled;
		}




		/// <summary>
		/// Tries to connect to the psu.
		/// </summary>
		/// <param name="portname">Name of the com port that has the psu attached</param>
		/// <returns>true if connected</returns>
		public bool Connect(string portname)
		{
			serport1.Open(portname);

			//send one cmd to check if the psu is there
			serport1.Send(new SerialPortHandler.SerialTask(SerialMsgType.INITOUTENA, "STATUS?", SERIAL_RECV_TIMEOUT_KEEP));//send a status request to see if the psu is present and set the outputenabled state
			while (serport1.RecvCount == 0) ;
			SerialPortHandler.SerialTask task;
			while (!serport1.TryRecv(out task)) ;
			if (task.Recv.Length != 1)
			{
				//we didnt get the expected single byte back
				serport1.Close();
				return false;
			}
			SetStatusIndicators((byte)task.Recv[0]);
			serport1.Send(new SerialPortHandler.SerialTask(SerialMsgType.VGET, "VSET1?", SERIAL_RECV_TIMEOUT_KEEP));
			serport1.Send(new SerialPortHandler.SerialTask(SerialMsgType.IGET, "ISET1?", SERIAL_RECV_TIMEOUT_KEEP));
			serport1.Send(new SerialPortHandler.SerialTask(SerialMsgType.VOUT, "VOUT1?", SERIAL_RECV_TIMEOUT_KEEP));
			serport1.Send(new SerialPortHandler.SerialTask(SerialMsgType.IOUT, "IOUT1?", SERIAL_RECV_TIMEOUT_KEEP));

			//set prev values to prevent sending if user changed values from 0 before connecting (the reply from the commands above will be used to mirror the setpoint values from the psu in the numupdowns)
			prevSetVoltage = setpointVoltage;
			prevSetCurrent = setpointCurrent;

			//enable comms processing:
			consecutiveEmptyReplies = 0;
			StartOtherThread();

			return true;
		}

		/// <summary>
		/// Returns if the psu is still connected.
		/// </summary>
		public bool IsConnected
		{
			get { return ((otherThread != null) && otherThread.IsAlive); }
		}

		/// <summary>
		/// Disconnects from the com port that has the psu attached.
		/// </summary>
		public void Disconnect()
		{
			StopOtherThread();
			if (serport1.IsOpen) serport1.Close();
		}

		/// <summary>
		/// Recalls SetpointV and SetpointI from the psu's memory. This will turn the output off!
		/// </summary>
		/// <param name="index">The memory index to recall. 1-5</param>
		public void Recall(byte index)
		{
			if ((0 == index) || (index > 5))
			{
				//psu only has 1-5
				throw new IndexOutOfRangeException();
			}
			serport1.Send(new SerialPortHandler.SerialTask(SerialMsgType.DONTCARE, "RCL" + index.ToString(), SERIAL_RECV_TIMEOUT_TOSS));
			serport1.Send(new SerialPortHandler.SerialTask(SerialMsgType.VGET, "VSET1?", SERIAL_RECV_TIMEOUT_KEEP));
			serport1.Send(new SerialPortHandler.SerialTask(SerialMsgType.IGET, "ISET1?", SERIAL_RECV_TIMEOUT_KEEP));
			//recall always turns off the output
		}

		/// <summary>
		/// Saves SetpointV and SetpointI to the psu's memory. This will turn the output off!
		/// </summary>
		/// <param name="index">The memory index to save to. 1-5</param>
		public void Save(byte index)
		{
			if ((0 == index) || (index > 5))
			{
				//psu only has 1-5
				throw new IndexOutOfRangeException();
			}
			//when recalling one mem and saving it under another, the psu ignores the save.
			//work-around: recall the one where we want to save and write our V and A again, then save.
			//(this selects the index we want to save to and makes it work but the output will be turned off because of the recall)
			serport1.Send(new SerialPortHandler.SerialTask(SerialMsgType.DONTCARE, "RCL" + index.ToString(), SERIAL_RECV_TIMEOUT_TOSS));
			serport1.Send(new SerialPortHandler.SerialTask(SerialMsgType.VSET, "VSET1:" + ((double)setpointVoltage / 100).ToString("00.00", new CultureInfo("en-US")), SERIAL_RECV_TIMEOUT_TOSS));
			serport1.Send(new SerialPortHandler.SerialTask(SerialMsgType.ISET, "ISET1:" + ((double)setpointCurrent / 1000).ToString("0.000", new CultureInfo("en-US")), SERIAL_RECV_TIMEOUT_TOSS));
			serport1.Send(new SerialPortHandler.SerialTask(SerialMsgType.DONTCARE, "SAV" + index.ToString(), SERIAL_RECV_TIMEOUT_TOSS));
		}
	}
}
