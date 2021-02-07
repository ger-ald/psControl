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
using System.Windows.Forms;
using System.IO.Ports;
using System.Globalization;
using System.Collections.Generic;
using Win32PortEnumerate;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Reflection;
using Microsoft.Win32;

namespace pscontrol
{
	public partial class Form1 : Form
	{
		private KA3005P psu;

		//logging vars:
		private bool logging = false;
		private string loggingFileName = "";
		private StreamWriter logFileStream;
		private int loggingInterval = 1;
		private int loggingIntervalCnt = 0;

		//scripting vars:
		private bool scripting = false;
		private string scriptingFileName = "";
		private List<string> scriptLines = null;
		private int scriptExeLine = 0;
		private int scriptExeDelay = 0;
		private List<int> scriptStack = null;

		private int updatesCounter = 0;
		private int form1WidthWhenShowingLess = 0;

		public Form1()
		{
			InitializeComponent();
			//append the version to our window title:
			Version version = Assembly.GetEntryAssembly().GetName().Version;
			this.Text += $"{version.Major}.{version.Minor}.{version.Build}{((version.Revision > 0) ? ("b" + version.Revision) : "")}";//append version and leave beta off if it is 0 (a release)

			//set voltage numupdowns:
			cnudVoltSetpoint3.SetOverflow(cnudVoltSetpoint2);
			cnudVoltSetpoint2.SetOverflow(cnudVoltSetpoint1);

			//set current numupdowns:
			cnudAmpSetpoint3.SetOverflow(cnudAmpSetpoint4);
			cnudAmpSetpoint4.SetOverflow(cnudAmpSetpoint2);
			cnudAmpSetpoint2.SetOverflow(cnudAmpSetpoint1);

			cnudLogIntervalSeconds.SetOverflow(cnudLogIntervalMinutes);

			btnStartPauseScript.Enabled = false;
			btnStopScript.Enabled = false;
			form1WidthWhenShowingLess = this.Width;

			psu = new KA3005P();
			psu.OnSurpriseDisconnect += psu_OnSurpriseDisconnect;
			psu.OnSetpointUpdate += psu_OnSetpointUpdate;
			psu.OnOutputUpdate += psu_OnOutputUpdate;

			toolStripStatusLabel1.Text = "";
			ConnectedChange(false);

			SystemEvents.PowerModeChanged += System_OnPowerChange;
		}

		private void RefreshDropdown()
		{
			List<SerialPortDevice> devices = new List<SerialPortDevice>();
			devices = SerialPortEnumerator.Enumerate();
			devices.Sort((x, y) => NativeMethods.StrCmpLogicalW(x.Port, y.Port));

			string prevSelection = cmbbxComList.Text;
			cmbbxComList.Items.Clear();
			cmbbxComList.Items.AddRange(devices.ToArray());
			if (cmbbxComList.Items.Count > 0)
			{
				//combobox isn't empty (there are comports on the pc)
				if (prevSelection.Length > 0)
				{
					//something was selected so try to put it back (combobox.SelectedIndex will go back to -1 if not found)
					cmbbxComList.Text = prevSelection;
				}
				if (-1 == cmbbxComList.SelectedIndex)
				{
					//nothing selected, so just select the first entry
					cmbbxComList.SelectedIndex = 0;
				}
			}
		}

		/// <summary>
		/// Enables and disables the controls that can change the output of the psu
		/// </summary>
		/// <param name="enabled">enable psu changable inputs or not</param>
		private void EnableInputs(bool enabled)
		{
			cbOutEnable.Enabled = enabled;
			gbPsuSetpoints.Enabled = enabled;
			gbMemoryButtons.Enabled = enabled;
		}

		/// <summary>
		/// This enables or disables all psu connection related form controls
		/// </summary>
		/// <param name="isConnected">what connectionstate changed to</param>
		private void ConnectedChange(bool isConnected)
		{
			EnableInputs(isConnected && !cbLockInputs.Checked);
			gbRecordPlayback.Enabled = isConnected;
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

		private void UpdateDispStatusLeds()
		{
			cbIndicatorOutEnabled.CheckState = psu.OutputEnabled ? CheckState.Indeterminate : CheckState.Unchecked;
			cbIndicatorOutEnabled.BackColor = psu.OutputEnabled ? Color.Red : SystemColors.Control;
			cbIndicatorCV.CheckState = psu.IsInCVmode ? CheckState.Indeterminate : CheckState.Unchecked;
			cbIndicatorCC.CheckState = psu.IsInCCmode ? CheckState.Indeterminate : CheckState.Unchecked;
		}

		private void UpdateDispOutput(double voltage, double current)
		{
			lblOutVolt.Text = voltage.ToString("#0.00", new CultureInfo("en-US")).PadLeft(6) + "  V";//can go up to 999.99
			lblOutAmp.Text = current.ToString("#0.000", new CultureInfo("en-US")).PadLeft(7) + " A";//can go up to 99.999

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

		private void UpdateDispSetpoint(double voltage, double current)
		{
			int spV = (int)((voltage * 100) + 0.5);
			int spI = (int)((current * 1000) + 0.5);

			//voltage:
			if (spV >= 0)
			{
				cnudVoltSetpoint1.ValueNoOnValueChanged = (spV / 100);
				cnudVoltSetpoint2.ValueNoOnValueChanged = (decimal)(spV / 10) % 10;
				cnudVoltSetpoint3.ValueNoOnValueChanged = (decimal)spV % 10;
			}
			//current:
			if (spI >= 0)
			{
				cnudAmpSetpoint1.ValueNoOnValueChanged = (spI / 1000);
				cnudAmpSetpoint2.ValueNoOnValueChanged = (decimal)(spI / 100) % 10;
				cnudAmpSetpoint4.ValueNoOnValueChanged = (decimal)(spI / 10) % 10;
				cnudAmpSetpoint3.ValueNoOnValueChanged = (decimal)spI % 10;
			}

			lblSetpointWatt.Text = (voltage * current).ToString("#0.000' W'", new CultureInfo("en-US")).PadLeft(9);
		}

		private void psu_OnSurpriseDisconnect(object sender, EventArgs e)
		{
			btnComConnect.BeginInvoke((MethodInvoker)delegate ()
			{
				toolStripStatusLabel1.Text = "Lost connection";
				StopComms();
			});
		}

		private void psu_OnSetpointUpdate(object sender, EventArgs e)
		{
			cnudVoltSetpoint1.BeginInvoke((MethodInvoker)delegate ()
			{
				UpdateDispSetpoint(psu.SetpointV, psu.SetpointI);
				cbOutEnable.Checked = psu.OutputEnabled;
			});
		}

		private void psu_OnOutputUpdate(object sender, EventArgs e)
		{
			lblOutVolt.BeginInvoke((MethodInvoker)delegate ()
			{
				UpdateDispStatusLeds();
				UpdateDispOutput(psu.OutputV, psu.OutputI);
				updatesCounter++;

				if (logging && (loggingInterval == 0))
				{
					//as fast as possible:
					loggingIntervalCnt = 0;
					WriteLogLine();
				}
			});
		}



		private void StartComms()
		{
			bool connected;
			if (cmbbxComList.SelectedItem == null)
			{
				toolStripStatusLabel1.Text = "Error: Port not found";
				return;
			}
			try
			{
				connected = psu.Connect(((SerialPortDevice)cmbbxComList.SelectedItem).Port);
			}
			catch (Exception ex)
			{
				//port is already open OR
				//tried to open a removed port
				toolStripStatusLabel1.Text = "Error: " + ex.Message;
				return;
			}

			if (!connected)
			{
				toolStripStatusLabel1.Text = "Error: Couldn't find device";
				return;
			}

			//set prev values to prevent sending if user changed values from 0 before connecting (the reply from the commands above will be used to mirror the setpoint values from the psu in the numupdowns)
			cmbbxComList.Enabled = false;
			btnComRefresh.Enabled = false;
			btnComConnect.Text = "Disconnect";
			toolStripStatusLabel1.Text = "Connected";

			//enable updateStats:
			updatesCounter = 0;
			tmrSecondTimer.Enabled = true;

			ConnectedChange(true);
		}

		private void StopComms()
		{
			if (scripting)
			{
				StopScripting();
			}
			if (logging)
			{
				StopLogging();
			}
			ConnectedChange(false);
			//disable comms processing:
			btnComConnect.Text = "Connect";
			psu.Disconnect();
			cmbbxComList.Enabled = true;
			btnComRefresh.Enabled = true;
			//disable updateStats:
			tmrSecondTimer.Enabled = false;
			lblOutRateDisplay.Text = " - updates/sec";

			RefreshDropdown();
		}



		private void System_OnPowerChange(object s, PowerModeChangedEventArgs e)
		{
			switch (e.Mode)
			{
				case PowerModes.Resume:
					psu.Resync();
					break;
				case PowerModes.Suspend:
					PauseScripting();
					break;
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			RefreshDropdown();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (logging)
			{
				StopLogging();
			}
			psu.Disconnect();
			SystemEvents.PowerModeChanged -= System_OnPowerChange;
		}

		private void BtnComRefreshList_Click(object sender, EventArgs e)
		{
			RefreshDropdown();
		}

		private void BtnComConnect_Click(object sender, EventArgs e)
		{
			toolStripStatusLabel1.Text = "";
			if (!psu.IsConnected)
			{
				StartComms();
			}
			else
			{
				StopComms();
			}
		}

		private void cnudVoltSetpoint_ValueChangeds(object sender, EventArgs e)
		{
			double voltage = (double)cnudVoltSetpoint1.Value + (double)cnudVoltSetpoint2.Value / 10 + (double)cnudVoltSetpoint3.Value / 100;
			psu.SetpointV = voltage;
			lblSetpointWatt.Text = (psu.SetpointV * psu.SetpointI).ToString("#0.000' W'", new CultureInfo("en-US")).PadLeft(9);
		}

		private void cnudAmpSetpoint_ValueChangeds(object sender, EventArgs e)
		{
			double current = (double)cnudAmpSetpoint1.Value + (double)cnudAmpSetpoint2.Value / 10 + (double)cnudAmpSetpoint4.Value / 100 + (double)cnudAmpSetpoint3.Value / 1000;
			psu.SetpointI = current;
			lblSetpointWatt.Text = (psu.SetpointV * psu.SetpointI).ToString("#0.000' W'", new CultureInfo("en-US")).PadLeft(9);
		}

		private void CbOutEnable_CheckedChanged(object sender, EventArgs e)
		{
			psu.SetpointOutputEnabled = cbOutEnable.Checked;
		}

		private void CbLockInputs_CheckedChanged(object sender, EventArgs e)
		{
			EnableInputs((!cbLockInputs.Checked) && psu.IsConnected);
		}

		private void btnLoad_Click(object sender, EventArgs e)
		{
			byte index = byte.Parse(((Button)sender).Text.Split(' ')[1]);
			psu.Recall(index);
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			byte index = byte.Parse(((Button)sender).Text.Split(' ')[1]);
			psu.Save(index);
			//cbOutEnable.Checked = false;
		}

		private void lblShowMore_Click(object sender, EventArgs e)
		{
			this.Width = lblShowMore.Text.StartsWith("More")? this.MaximumSize.Width : form1WidthWhenShowingLess;
		}

		private void Form1_SizeChanged(object sender, EventArgs e)
		{
			lblShowMore.Text = (this.Width < this.MaximumSize.Width)? "More>>" : "Less<<";
		}

		private void TmrSecondTimer_Tick(object sender, EventArgs e)
		{
			lblOutRateDisplay.Text = ((double)updatesCounter / ((double)tmrSecondTimer.Interval / 1000.0)).ToString("#0", new CultureInfo("en-US")).PadLeft(2) + " updates/sec";
			updatesCounter = 0;

			if (scripting)
			{
				ExecScript();
			}
			if (logging && (loggingInterval > 0))
			{
				loggingIntervalCnt++;
				if (loggingIntervalCnt >= loggingInterval)
				{
					loggingIntervalCnt = 0;
					WriteLogLine();
				}
			}
		}



		private void StartLogging()
		{
			ofdLogAndScript.Title = "Log to file";
			ofdLogAndScript.FileName = "psuLog_" + DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss") + ".csv";
			ofdLogAndScript.CheckFileExists = false;

			//ShowDialog() is blocking but thats ok
			DialogResult res = ofdLogAndScript.ShowDialog();
			if (res == DialogResult.OK)
			{
				btnStartStopLog.Text = "Stop log";
				loggingFileName = ofdLogAndScript.FileName;
				try
				{
					FileStream logFile = new FileStream(loggingFileName, FileMode.Append, FileAccess.Write, FileShare.Read);
					logFileStream = new StreamWriter(logFile);
				}
				catch (UnauthorizedAccessException ex)
				{
					toolStripStatusLabel1.Text = "Error: " + ex.Message;
					return;
				}
				toolStripStatusLabel1.Text = "";
				logFileStream.WriteLine("Time;CC Mode;spVolts;spAmps;Volts;Amps");
				logFileStream.Flush();

				loggingInterval = ((int)cnudLogIntervalMinutes.Value * 60) + (int)cnudLogIntervalSeconds.Value;
				loggingIntervalCnt = 0;
				logging = true;
			}
		}

		private void WriteLogLine()
		{
			//logFileStream.WriteLine($"{(UInt64)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds};{(psu.IsInCCmode ? 1 : 0)};{(psu.OutputEnabled ? psu.SetpointV : 0)};{psu.SetpointI};{psu.OutputV};{psu.OutputI}");//log as utc timestamp
			//logFileStream.WriteLine($"{DateTime.UtcNow.ToString("O")};{(psu.IsInCCmode ? 1 : 0)};{(psu.OutputEnabled ? psu.SetpointV : 0)};{psu.SetpointI};{psu.OutputV};{psu.OutputI}");//log as utc standard string
			logFileStream.WriteLine($"{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")};{(psu.IsInCCmode ? 1 : 0)};{(psu.OutputEnabled ? psu.SetpointV : 0)};{psu.SetpointI};{psu.OutputV};{psu.OutputI}");//log as local formatted string (also localized '.' or ',' use for decimal seperator)(tested compatible with dutch m. excel)
			logFileStream.Flush();
		}

		private void StopLogging()
		{
			logging = false;
			logFileStream.Close();
			logFileStream = null;
			btnStartStopLog.Text = "Start log";
		}

		private void btnStartStopLog_Click(object sender, EventArgs e)
		{
			if (!logging)
			{
				//logging is off so start logging
				StartLogging();
			}
			else
			{
				//we are logging so stop logging
				StopLogging();
			}
		}

		private void cnudLogIntervals_ValueChanged(object sender, EventArgs e)
		{
			loggingInterval = ((int)cnudLogIntervalMinutes.Value * 60) + (int)cnudLogIntervalSeconds.Value;
		}

		private void StartResumeScripting()
		{
			if (scriptStack != null)
			{
				//resume:
				scripting = true;
				btnStartPauseScript.Image = Properties.Resources.PauseIcon;
				toolStripStatusLabel1.Text = "Script is running!";
			}
			else
			{
				//start:
				try
				{
					scriptLines = new List<string>();
					using (var fileStream = new FileStream(scriptingFileName, FileMode.Open, FileAccess.Read, FileShare.None))
					using (var sr = new StreamReader(fileStream))
					{
						string line;
						while ((line = sr.ReadLine()) != null)
						{
							scriptLines.Add(line);
						}
						sr.Close();
						fileStream.Close();
					}

					scriptExeLine = 0;
					scriptExeDelay = 0;
					scriptStack = new List<int>();

					scripting = true;
					btnStartPauseScript.Image = Properties.Resources.PauseIcon;
					toolStripStatusLabel1.Text = "Script is running!";
				}
				catch (Exception ex)
				{
					toolStripStatusLabel1.Text = $"Error: {ex.Message}";
				}
			}
		}

		private void PauseScripting()
		{
			if (scripting)
			{
				scripting = false;
				//leave scriptStack as not null
				btnStartPauseScript.Image = Properties.Resources.PlayIcon;
			}
		}

		private void ExecScript()
		{
			if (scriptExeDelay > 0) scriptExeDelay--;

			bool setpointsUpdated = false;
			while (scriptExeDelay == 0)
			{
				if (scriptExeLine < scriptLines.Count)
				{
					//not yet at end of script

					Console.WriteLine($"{scriptExeLine} '{scriptLines[scriptExeLine]}'");
					string line = scriptLines[scriptExeLine].Replace('.', ',');
					line = line.TrimStart(new char[] { ' ', '\t' });
					line = line.ToLower(); ;
					string[] lineArgs = line.Split(new char[] { ' ', '\t' });
					if (lineArgs.Length > 0)
					{
						bool outputEnable = cbOutEnable.Checked;
						double voltage = psu.SetpointV;
						double current = psu.SetpointI;

						switch (lineArgs[0])
						{
							//psu control:
							case "v":
								if (lineArgs.Length > 1)
								{
									if (double.TryParse(lineArgs[1], out double value))
									{
										voltage = value;
									}
								}
								break;
							case "i":
								if (lineArgs.Length > 1)
								{
									if (double.TryParse(lineArgs[1], out double value))
									{
										current = value;
									}
								}
								break;
							case "vi":
								if (lineArgs.Length > 2)
								{
									double value;
									if (double.TryParse(lineArgs[1], out value))
									{
										voltage = value;
									}
									if (double.TryParse(lineArgs[2], out value))
									{
										current = value;
									}
								}
								break;

							case "v++":
								if (lineArgs.Length > 1)
								{
									if (double.TryParse(lineArgs[1], out double value))
									{
										voltage += value;
									}
								}
								else
								{
									voltage += 1;
								}
								break;
							case "v--":
								if (lineArgs.Length > 1)
								{
									if (double.TryParse(lineArgs[1], out double value))
									{
										voltage -= value;
									}
								}
								else
								{
									voltage -= 1;
								}
								break;
							case "i++":
								if (lineArgs.Length > 1)
								{
									if (double.TryParse(lineArgs[1], out double value))
									{
										current += value;
									}
								}
								else
								{
									current += 1;
								}
								break;
							case "i--":
								if (lineArgs.Length > 1)
								{
									if (double.TryParse(lineArgs[1], out double value))
									{
										current -= value;
									}
								}
								else
								{
									current -= 1;
								}
								break;

							case "out0":
								outputEnable = false;
								break;
							case "out1":
								outputEnable = true;
								break;



							//execution control:
							case "wait":
								if (lineArgs.Length > 1)
								{
									int value;
									if (int.TryParse(lineArgs[1], out value))
									{
										scriptExeDelay = (value > 0) ? value : 0;
									}
								}
								break;
							case "loop":
								if (lineArgs.Length > 1)
								{
									if (scriptStack.Count < 42)
									{
										if (int.TryParse(lineArgs[1], out int value))
										{
											scriptStack.Add(scriptExeLine);//scriptExeLine will be incremented after popping it from the stack
											scriptStack.Add((value > 1) ? value : 1);
										}
									}
									else
									{
										toolStripStatusLabel1.Text = "script stack overflow (loop nesting)";
									}
								}
								break;
							case "pool":
								if (scriptStack.Count > 1)
								{
									int value = --scriptStack[scriptStack.Count - 1];
									if (value == 0)
									{
										//loop is over
										scriptStack.RemoveAt(scriptStack.Count - 1);
										scriptStack.RemoveAt(scriptStack.Count - 1);
									}
									else
									{
										//one more time!
										scriptExeLine = scriptStack[scriptStack.Count - 2];
									}
								}
								break;

							default:
								break;
						}
						if (voltage < 0.0)
						{
							voltage = 0;
						}
						else if (voltage > 31.0)
						{
							voltage = 31;
						}
						if (current < 0.0)
						{
							current = 0;
						}
						else if (current > 5.1)
						{
							current = 31;
						}
						if ((voltage != psu.SetpointV) ||
							(current != psu.SetpointI))
						{
							psu.SetpointV = voltage;
							psu.SetpointI = current;
							setpointsUpdated = true;
						}
						if (outputEnable != cbOutEnable.Checked)
						{
							cbOutEnable.Checked = outputEnable;//set checkbox instead of psu because checkbox has a valuechanged that also sets the psu
							setpointsUpdated = true;
						}
					}
					else
					{
						//empty line
					}
					scriptExeLine++;
				}
				else
				{
					//end of script reached
					StopScripting();
					break;
				}
			}
			if (setpointsUpdated)
			{
				UpdateDispSetpoint(psu.SetpointV, psu.SetpointI);
			}
		}

		private void StopScripting()
		{
			scripting = false;
			scriptStack = null;
			btnStartPauseScript.Image = Properties.Resources.PlayIcon;
		}

		private void btnStartPauseScript_Click(object sender, EventArgs e)
		{
			toolStripStatusLabel1.Text = "";
			if (scripting)
			{
				PauseScripting();
			}
			else
			{
				StartResumeScripting();
			}
		}

		private void btnStopScript_Click(object sender, EventArgs e)
		{
			toolStripStatusLabel1.Text = "";
			if (scripting)
			{
				StopScripting();
			}
		}

		private void btnBrowseScript_Click(object sender, EventArgs e)
		{
			toolStripStatusLabel1.Text = "";
			ofdLogAndScript.Title = "Open script file";
			ofdLogAndScript.FileName = "psuScript.txt";
			ofdLogAndScript.CheckFileExists = true;

			//ShowDialog() is blocking but thats ok
			DialogResult res = ofdLogAndScript.ShowDialog();
			if (res == DialogResult.OK)
			{
				scriptingFileName = ofdLogAndScript.FileName;
				lblScriptName.Text = Path.GetFileName(scriptingFileName);
				btnStartPauseScript.Enabled = true;
				btnStopScript.Enabled = true;
			}
		}
	}
}
