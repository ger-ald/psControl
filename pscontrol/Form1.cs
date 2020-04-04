using System;
using System.Windows.Forms;
using System.IO.Ports;
using System.Globalization;
using System.Collections.Generic;

namespace pscontrol
{
	public partial class Form1 : Form
	{
		private const int SERIAL_RECV_TIMEOUT_KEEP = 250;//ms. time to wait for a reply after sending a cmd
		private const int SERIAL_RECV_TIMEOUT_TOSS = 45;//ms. time to wait (for a reply) after sending a cmd to give psu time to process
		private const int SERIAL_MAXEMPTYREPLIES = 4;//after thismany unexpected empty replies, the psu is considered lost. (if psu is disconnected the serial cmd's will timeout and thus come back empty even when we expected a reply)


		//public static Form1 mainformref;
		private readonly SerialPortHandler serport1;


		private const int SERIALMSG_INITOUTENA = 0;
		private const int SERIALMSG_STATUS     = 1;
		private const int SERIALMSG_VSET       = 2;
		private const int SERIALMSG_ISET       = 3;
		private const int SERIALMSG_VOUT       = 4;
		private const int SERIALMSG_IOUT       = 5;
		private const int SERIALMSG_VGET       = 6;
		private const int SERIALMSG_IGET       = 7;
		private const int SERIALMSG_DONTCARE   = 8;

		private int setVoltage = -1;
		private int setCurrent = -1;
		private int prevSetVoltage = -1;// = volt* 100
		private int prevSetCurrent = -1;// = amp *1000
		private double currOutVoltage = 0.0;
		private double currOutCurrent = 0.0;
		private int consecutiveEmptyReplies = 0;
		private int repliesCounter = 0;
		private int voltRepliesCounter = 0;

		public Form1()
		{
			InitializeComponent();
			//set voltage numupdowns:
			customNumericUpDown3.SetOverflow(customNumericUpDown2);
			customNumericUpDown2.SetOverflow(customNumericUpDown1);

			//set current numupdowns:
			customNumericUpDown7.SetOverflow(customNumericUpDown6);
			customNumericUpDown6.SetOverflow(customNumericUpDown5);
			customNumericUpDown5.SetOverflow(customNumericUpDown4);

			//set calcwatt callback
			customNumericUpDown1.ValueChanged += CustomNumericUpDowns_ValueChangedVoltage;
			customNumericUpDown2.ValueChanged += CustomNumericUpDowns_ValueChangedVoltage;
			customNumericUpDown3.ValueChanged += CustomNumericUpDowns_ValueChangedVoltage;
			customNumericUpDown4.ValueChanged += CustomNumericUpDowns_ValueChangedCurrent;
			customNumericUpDown5.ValueChanged += CustomNumericUpDowns_ValueChangedCurrent;
			customNumericUpDown6.ValueChanged += CustomNumericUpDowns_ValueChangedCurrent;
			customNumericUpDown7.ValueChanged += CustomNumericUpDowns_ValueChangedCurrent;

			toolStripStatusLabel1.Text = "";
			//mainformref = this;
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
			EnableInputs(false);
		}

		private void Serport1_SerialPortBroke()
		{
			btnComConnect.BeginInvoke((MethodInvoker)delegate ()
			{
				toolStripStatusLabel1.Text = "lost comport";
				StopComms();
			});
		}

		private void RefreshDropdown()
		{
			SerialPortHandler.ComPortEntry[] comPorts = SerialPortHandler.WmiDetectComs();
			List<string> descriptions = new List<string>();
			foreach (SerialPortHandler.ComPortEntry comPortEntry in comPorts)
				descriptions.Add(comPortEntry.port + " : " + comPortEntry.description);
			string prevSelection = cmbbxComList.Text;
			cmbbxComList.Items.Clear();
			cmbbxComList.Items.AddRange(descriptions.ToArray());
			if (cmbbxComList.Items.Count > 0)
			{
				//combobox isn't empty (there are comports on the pc)
				if (prevSelection.Length > 0)
				{
					//something was selected so try to put it back (combobox.SelectedIndex will go back to -1 if not found)
					cmbbxComList.SelectedItem = prevSelection;
				}
				if (-1 == cmbbxComList.SelectedIndex)
				{
					//nothing selected, so just select the first entry
					cmbbxComList.SelectedIndex = 0;
				}
			}
		}

		private void EnableInputs(bool newState)
		{
			cbOutEnable.Enabled = newState;
			groupBox1.Enabled = newState;
			groupBox4.Enabled = newState;
		}

		/// <summary>
		/// Makes an escaped printable string. For example, the char '\x00' becomes "\0"
		/// </summary>
		/*private static string ToLiteral(string input)
		{
			using (var writer = new StringWriter())
			{
				using (var provider = CodeDomProvider.CreateProvider("CSharp"))
				{
					provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
					return writer.ToString();
				}
			}
		}*/

		private void SetStatLeds(byte status)
		{
			cbIndicatorOutEnabled.CheckState = ((status & 0x40) != 0) ? CheckState.Indeterminate : CheckState.Unchecked;
			cbIndicatorCV.CheckState = (((status & 0x01) != 0) && ((status & 0x40) != 0)) ? CheckState.Indeterminate : CheckState.Unchecked;
			cbIndicatorCC.CheckState = ((!cbIndicatorCV.Checked) && ((status & 0x40) != 0)) ? CheckState.Indeterminate : CheckState.Unchecked;
		}

		private void CalcWattOhm(double voltage, double current)
		{
			double power = voltage * current;
			double resistance = voltage / current;
			lblOutWatt.Text = power.ToString("#0.000", new CultureInfo("en-US")).PadLeft(7) + " W";//can go up to 999.999
			if (double.IsInfinity(resistance))
				lblOutOhm.Text = "∞".PadLeft(7) + " Ω";//on windows xp it prints 'Infinity' instead of the infinity sign if you run it through ToString()
			else if (resistance < 1000.000 || double.IsNaN(resistance))
				lblOutOhm.Text = resistance.ToString("#0.000", new CultureInfo("en-US")).PadLeft(7) + " Ω";//can go up to 999.999
			else
				lblOutOhm.Text = (resistance / 1000.0).ToString("#0.00", new CultureInfo("en-US")).PadLeft(6) + " kΩ";//can go up to 999.999
		}

		private void UpdateInputs()
		{
			//voltage:
			if (setVoltage >= 0)
			{
				customNumericUpDown1.ValueNoOnValueChanged = (setVoltage / 100);
				customNumericUpDown2.ValueNoOnValueChanged = (decimal)(setVoltage / 10) % 10;
				customNumericUpDown3.ValueNoOnValueChanged = (decimal)setVoltage % 10;
			}
			//current:
			if (setCurrent >= 0)
			{
				customNumericUpDown4.ValueNoOnValueChanged = (setCurrent / 1000);
				customNumericUpDown5.ValueNoOnValueChanged = (decimal)(setCurrent / 100) % 10;
				customNumericUpDown6.ValueNoOnValueChanged = (decimal)(setCurrent / 10) % 10;
				customNumericUpDown7.ValueNoOnValueChanged = (decimal)setCurrent % 10;
			}

			lblSetpointWatt.Text = ((setVoltage / 100.0) * (setCurrent / 1000.0)).ToString("#0.000' W'", new CultureInfo("en-US")).PadLeft(9);
		}


		private void StartComms()
		{
			try
			{
				serport1.Open(cmbbxComList.Text.Split(':')[0]);
			}
			catch (Exception ex)
			{
				toolStripStatusLabel1.Text = "error opening port " + ex.Message;
				return;
			}

			//send one cmd to check if the psu is there
			serport1.Send(new SerialPortHandler.Serialtask(SERIALMSG_INITOUTENA, "STATUS?", SERIAL_RECV_TIMEOUT_KEEP));//send a status request to see if the psu is present and set the outputenabled state
			while (serport1.RecvCount == 0) ;
			SerialPortHandler.Serialtask task;
			while (!serport1.TryRecv(out task)) ;
			if (task.recv.Length != 1)
			{
				//we didnt get the expected single byte back
				toolStripStatusLabel1.Text = "couldn't find device";
				serport1.Close();
				return;
			}
			SetStatLeds((byte)task.recv[0]);
			cbOutEnable.Checked = ((byte)task.recv[0] & 0x40) != 0;
			serport1.Send(new SerialPortHandler.Serialtask(SERIALMSG_VGET, "VSET1?", SERIAL_RECV_TIMEOUT_KEEP));
			serport1.Send(new SerialPortHandler.Serialtask(SERIALMSG_IGET, "ISET1?", SERIAL_RECV_TIMEOUT_KEEP));
			serport1.Send(new SerialPortHandler.Serialtask(SERIALMSG_VOUT, "VOUT1?", SERIAL_RECV_TIMEOUT_KEEP));
			serport1.Send(new SerialPortHandler.Serialtask(SERIALMSG_IOUT, "IOUT1?", SERIAL_RECV_TIMEOUT_KEEP));

			//set prev values to prevent sending if user changed values from 0 before connecting (the reply from the commands above will be used to mirror the setpoint values from the psu in the numupdowns)
			prevSetVoltage = (int)customNumericUpDown1.Value * 100 + (int)customNumericUpDown2.Value * 10 + (int)customNumericUpDown3.Value;
			prevSetCurrent = (int)customNumericUpDown4.Value * 1000 + (int)customNumericUpDown5.Value * 100 + (int)customNumericUpDown6.Value * 10 + (int)customNumericUpDown7.Value;
			cmbbxComList.Enabled = false;
			btnComRefresh.Enabled = false;
			btnComConnect.Text = "Disconnect";
			toolStripStatusLabel1.Text = "connected";
			//enable comms processing:
			consecutiveEmptyReplies = 0;
			tmrComTimer.Enabled = true;
			//enable stats:
			repliesCounter = 0;
			voltRepliesCounter = 0;
			tmrRateTimer.Enabled = true;

			if (!lockInputs.Checked) EnableInputs(true);
		}

		private void StopComms()
		{
			EnableInputs(false);
			//disable comms processing:
			btnComConnect.Text = "Connect";
			serport1.Close();
			cmbbxComList.Enabled = true;
			btnComRefresh.Enabled = true;
			tmrComTimer.Enabled = false;
			//disable stats:
			tmrRateTimer.Enabled = false;
			lblReplyRateDisplay.Text = " - replies/sec";
			lblOutRateDisplay.Text = " - updates/sec";

			RefreshDropdown();
		}

		private void PsuSetVoltage(double newVoltage)
		{
			setVoltage = (int)((newVoltage + 0.005) * 100);
		}

		private void PsuSetCurrent(double newCurrent)
		{
			setCurrent = (int)((newCurrent + 0.0005) * 1000);
		}

		private void PsuRecall(byte index)
		{
			if ((0 == index) || (index > 5))
			{
				//psu only has 1-5
				throw new IndexOutOfRangeException();
			}
			serport1.Send(new SerialPortHandler.Serialtask(SERIALMSG_DONTCARE, "RCL" + index.ToString(), SERIAL_RECV_TIMEOUT_TOSS));
			serport1.Send(new SerialPortHandler.Serialtask(SERIALMSG_VGET, "VSET1?", SERIAL_RECV_TIMEOUT_KEEP));
			serport1.Send(new SerialPortHandler.Serialtask(SERIALMSG_IGET, "ISET1?", SERIAL_RECV_TIMEOUT_KEEP));
			cbOutEnable.Checked = false;//recall always turns off the output
		}

		private void PsuSave(byte index)
		{
			if ((0 == index) || (index > 5))
			{
				//psu only has 1-5
				throw new IndexOutOfRangeException();
			}
			//when recalling one mem and saving it under another, the psu ignores the save. work-around: recall the one where we want to save and write our V and A again, then save. (write V and A only doesn't work)
			serport1.Send(new SerialPortHandler.Serialtask(SERIALMSG_DONTCARE, "RCL" + index.ToString(), SERIAL_RECV_TIMEOUT_TOSS));
			serport1.Send(new SerialPortHandler.Serialtask(SERIALMSG_VSET, "VSET1:" + ((double)setVoltage / 100).ToString("00.00", new CultureInfo("en-US")), SERIAL_RECV_TIMEOUT_TOSS));
			serport1.Send(new SerialPortHandler.Serialtask(SERIALMSG_ISET, "ISET1:" + ((double) setCurrent / 1000).ToString("0.000", new CultureInfo("en-US")), SERIAL_RECV_TIMEOUT_TOSS));
			serport1.Send(new SerialPortHandler.Serialtask(SERIALMSG_DONTCARE, "SAV" + index.ToString(), SERIAL_RECV_TIMEOUT_TOSS));

			//can't prevent output from turning off, but can update the outputenable checkbox to reflect it:
			cbOutEnable.Checked = false;
		}



		private void Form1_Load(object sender, EventArgs e)
		{
			RefreshDropdown();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			serport1.Close();
		}

		private void BtnComRefreshList_Click(object sender, EventArgs e)
		{
			RefreshDropdown();
		}

		private void BtnComConnect_Click(object sender, EventArgs e)
		{
			toolStripStatusLabel1.Text = "";
			if (!serport1.IsOpen)
			{
				StartComms();
			}
			else
			{
				StopComms();
			}
		}

		private void CustomNumericUpDowns_ValueChangedVoltage(object sender, EventArgs e)
		{
			double voltage = (double)customNumericUpDown1.Value + (double)customNumericUpDown2.Value / 10 + (double)customNumericUpDown3.Value / 100;
			PsuSetVoltage(voltage);
			lblSetpointWatt.Text = ((setVoltage / 100.0) * (setCurrent / 1000.0)).ToString("#0.000' W'", new CultureInfo("en-US")).PadLeft(9);
		}

		private void CustomNumericUpDowns_ValueChangedCurrent(object sender, EventArgs e)
		{
			double current = (double)customNumericUpDown4.Value + (double)customNumericUpDown5.Value / 10 + (double)customNumericUpDown6.Value / 100 + (double)customNumericUpDown7.Value / 1000;
			PsuSetCurrent(current);
			lblSetpointWatt.Text = ((setVoltage / 100.0) * (setCurrent / 1000.0)).ToString("#0.000' W'", new CultureInfo("en-US")).PadLeft(9);
		}

		private void CbOutEnable_CheckedChanged(object sender, EventArgs e)
		{
			serport1.Send(new SerialPortHandler.Serialtask(SERIALMSG_DONTCARE, "OUT" + (cbOutEnable.Checked ? "1" : "0"), SERIAL_RECV_TIMEOUT_TOSS));
		}

		private void LockInputs_CheckedChanged(object sender, EventArgs e)
		{
			EnableInputs((!lockInputs.Checked) && serport1.IsOpen);
		}

		private void TmrComTimer_Tick(object sender, EventArgs e)
		{
			//send new commands if the transmit queue is empty: (avoids creating a large backlog because of differing transfer rates)
			if (serport1.SendCount == 0)
			{
				if (setVoltage != prevSetVoltage)
				{
					prevSetVoltage = setVoltage;
					serport1.Send(new SerialPortHandler.Serialtask(SERIALMSG_VSET, "VSET1:" + ((double)setVoltage / 100).ToString("00.00", new CultureInfo("en-US")), SERIAL_RECV_TIMEOUT_TOSS));
				}
				if (setCurrent != prevSetCurrent)
				{
					prevSetCurrent = setCurrent;
					serport1.Send(new SerialPortHandler.Serialtask(SERIALMSG_ISET, "ISET1:" + ((double)setCurrent / 1000).ToString("0.000", new CultureInfo("en-US")), SERIAL_RECV_TIMEOUT_TOSS));
				}
				serport1.Send(new SerialPortHandler.Serialtask(SERIALMSG_STATUS, "STATUS?", SERIAL_RECV_TIMEOUT_KEEP));
				serport1.Send(new SerialPortHandler.Serialtask(SERIALMSG_VOUT, "VOUT1?", SERIAL_RECV_TIMEOUT_KEEP));
				serport1.Send(new SerialPortHandler.Serialtask(SERIALMSG_IOUT, "IOUT1?", SERIAL_RECV_TIMEOUT_KEEP));
			}

			//handle all command replies:
			while (serport1.RecvCount > 0)
			{
				SerialPortHandler.Serialtask task;
				while (!serport1.TryRecv(out task)) ;

				//Console.WriteLine("{0}: {1} -> {2}     {3} : {4}", task.type, ToLiteral(task.send), ToLiteral(task.recv), serport1.SendCount, serport1.RecvCount);
				repliesCounter++;

				if (task.recv.Length == 0)
				{
					if (task.waittime != SERIAL_RECV_TIMEOUT_TOSS)
					{
						//we expected a reply but didn't get one
						consecutiveEmptyReplies++;
						if (consecutiveEmptyReplies >= SERIAL_MAXEMPTYREPLIES)
						{
							//psu is considered not connected anymore
							StopComms();
							toolStripStatusLabel1.Text = "lost connection";
						}
					}
					return;
				}
				else if (task.waittime != SERIAL_RECV_TIMEOUT_TOSS)
				{
					consecutiveEmptyReplies = 0;
				}
				if ((task.type != SERIALMSG_STATUS) && (!char.IsDigit(task.recv[0]))) return;//filter misformed replies
				double tempvalue;
				switch (task.type)
				{
					//received cc/cv and output enabled state from psu ('SERIALMSG_STATUS' happens frequent)
					case SERIALMSG_STATUS:
						SetStatLeds((byte)task.recv[0]);
						break;

					//received actual v/i output from psu (happens frequent)
					case SERIALMSG_VOUT:
						if (double.TryParse(task.recv, NumberStyles.Any, new CultureInfo("en-US"), out currOutVoltage))
						{
							lblOutVolt.Text = currOutVoltage.ToString("#0.00", new CultureInfo("en-US")).PadLeft(6) + "  V";//can go up to 999.99
							CalcWattOhm(currOutVoltage, currOutCurrent);
							voltRepliesCounter++;
						}
						break;
					case SERIALMSG_IOUT:
						if (double.TryParse(task.recv, NumberStyles.Any, new CultureInfo("en-US"), out currOutCurrent))
						{
							lblOutAmp.Text = currOutCurrent.ToString("#0.000", new CultureInfo("en-US")).PadLeft(7) + " A";//can go up to 99.999
							CalcWattOhm(currOutVoltage, currOutCurrent);
						}
						break;

					//received setpoint v/i output from psu (happens at connect only)
					case SERIALMSG_VGET:
						if (double.TryParse(task.recv, NumberStyles.Any, new CultureInfo("en-US"), out tempvalue))
						{
							PsuSetVoltage(tempvalue);
							prevSetVoltage = setVoltage;
							UpdateInputs();
						}
						break;
					case SERIALMSG_IGET:
						if (double.TryParse(task.recv, NumberStyles.Any, new CultureInfo("en-US"), out tempvalue))
						{
							PsuSetCurrent(tempvalue);
							prevSetCurrent = setCurrent;
							UpdateInputs();
						}
						break;

					default:
						break;
				}
			}
		}

		private void TmrRateTimer_Tick(object sender, EventArgs e)
		{
			lblReplyRateDisplay.Text = ((double)repliesCounter / ((double)tmrRateTimer.Interval / 1000.0)).ToString("#0", new CultureInfo("en-US")).PadLeft(2) + " replies/sec";
			repliesCounter = 0;
			lblOutRateDisplay.Text = ((double)voltRepliesCounter / ((double)tmrRateTimer.Interval / 1000.0)).ToString("#0", new CultureInfo("en-US")).PadLeft(2) + " updates/sec";
			voltRepliesCounter = 0;
		}

		private void btnLoad_Click(object sender, EventArgs e)
		{
			byte index = byte.Parse(((Button)sender).Text.Split(' ')[1]);
			PsuRecall(index);
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			byte index = byte.Parse(((Button)sender).Text.Split(' ')[1]);
			PsuSave(index);
		}
	}
}
