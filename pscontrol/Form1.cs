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

namespace pscontrol
{
	public partial class Form1 : Form
	{
		private KA3005P psu;

		private int updatesCounter = 0;

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

			psu = new KA3005P();
			psu.OnSurpriseDisconnect += psu_OnSurpriseDisconnect;
			psu.OnSetpointUpdate += psu_OnSetpointUpdate;
			psu.OnOutputUpdate += psu_OnOutputUpdate;

			toolStripStatusLabel1.Text = "";
			EnableInputs(false);
		}

		private void psu_OnSurpriseDisconnect()
		{
			btnComConnect.BeginInvoke((MethodInvoker)delegate ()
			{
				toolStripStatusLabel1.Text = "lost comport";
				StopComms();
			});
		}

		private void RefreshDropdown()
		{
			List<SerialPortDevice> devices = new List<SerialPortDevice>();
			//try
			//{
				devices = SerialPortEnumerator.Enumerate().ToList();
			/*}
			catch (Exception ex)
			{
				Console.WriteLine($"{ex.Message}\n\n{ex.StackTrace}");
			}*/
			devices.Sort((x, y) => NativeMethods.StrCmpLogicalW(x.Port, y.Port));
			//devices.ForEach(device => Console.WriteLine($"Found port:\n  Port:         {device.Port}\n  Description:  {device.Description}\n  FriendlyName: {device.FriendlyName}\n  BusReported:  {device.BusReportedDeviceDescription}\n  deviceID:     {device.DeviceID}"));

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

		private void UpdateStatusLeds()
		{
			cbIndicatorOutEnabled.CheckState = psu.OutputEnabled ? CheckState.Indeterminate : CheckState.Unchecked;
			cbIndicatorCV.CheckState = psu.IsInCVmode ? CheckState.Indeterminate : CheckState.Unchecked;
			cbIndicatorCC.CheckState = psu.IsInCCmode ? CheckState.Indeterminate : CheckState.Unchecked;
		}

		private void UpdateWattOhm(double voltage, double current)
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

		private void psu_OnSetpointUpdate()
		{
			customNumericUpDown1.BeginInvoke((MethodInvoker)delegate ()
			{
				int spV = (int)((psu.SetpointV * 100) + 0.5);
				int spI = (int)((psu.SetpointI * 1000) + 0.5);
				//voltage:
				if (spV >= 0)
				{
					customNumericUpDown1.ValueNoOnValueChanged = (spV / 100);
					customNumericUpDown2.ValueNoOnValueChanged = (decimal)(spV / 10) % 10;
					customNumericUpDown3.ValueNoOnValueChanged = (decimal)spV % 10;
				}
				//current:
				if (spI >= 0)
				{
					customNumericUpDown4.ValueNoOnValueChanged = (spI / 1000);
					customNumericUpDown5.ValueNoOnValueChanged = (decimal)(spI / 100) % 10;
					customNumericUpDown6.ValueNoOnValueChanged = (decimal)(spI / 10) % 10;
					customNumericUpDown7.ValueNoOnValueChanged = (decimal)spI % 10;
				}

				lblSetpointWatt.Text = (psu.SetpointV * psu.SetpointI).ToString("#0.000' W'", new CultureInfo("en-US")).PadLeft(9);
			});
		}

		private void psu_OnOutputUpdate()
		{
			lblOutVolt.BeginInvoke((MethodInvoker)delegate ()
			{
				UpdateStatusLeds();
				lblOutVolt.Text = psu.OutputV.ToString("#0.00", new CultureInfo("en-US")).PadLeft(6) + "  V";//can go up to 999.99
				lblOutAmp.Text = psu.OutputI.ToString("#0.000", new CultureInfo("en-US")).PadLeft(7) + " A";//can go up to 99.999
				UpdateWattOhm(psu.OutputV, psu.OutputI);
				updatesCounter++;
			});
		}



		private void StartComms()
		{
			bool connected;
			try
			{
				connected = psu.Connect((cmbbxComList.SelectedItem as SerialPortDevice).Port);
			}
			catch (Exception ex)
			{
				toolStripStatusLabel1.Text = "error opening port " + ex.Message;
				return;
			}

			if (!connected)
			{
				toolStripStatusLabel1.Text = "couldn't find device";
				return;
			}
			cbOutEnable.Checked = psu.OutputEnabled;
			UpdateStatusLeds();

			//set prev values to prevent sending if user changed values from 0 before connecting (the reply from the commands above will be used to mirror the setpoint values from the psu in the numupdowns)
			cmbbxComList.Enabled = false;
			btnComRefresh.Enabled = false;
			btnComConnect.Text = "Disconnect";
			toolStripStatusLabel1.Text = "connected";

			//enable stats:
			updatesCounter = 0;
			tmrRateTimer.Enabled = true;

			if (!cbLockInputs.Checked) EnableInputs(true);
		}

		private void StopComms()
		{
			EnableInputs(false);
			//disable comms processing:
			btnComConnect.Text = "Connect";
			psu.Disconnect();
			cmbbxComList.Enabled = true;
			btnComRefresh.Enabled = true;
			//disable stats:
			tmrRateTimer.Enabled = false;
			lblOutRateDisplay.Text = " - updates/sec";

			RefreshDropdown();
		}



		private void Form1_Load(object sender, EventArgs e)
		{
			RefreshDropdown();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			psu.Disconnect();
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

		private void CustomNumericUpDowns_ValueChangedVoltage(object sender, EventArgs e)
		{
			double voltage = (double)customNumericUpDown1.Value + (double)customNumericUpDown2.Value / 10 + (double)customNumericUpDown3.Value / 100;
			psu.SetpointV = voltage;
			lblSetpointWatt.Text = (psu.SetpointV * psu.SetpointI).ToString("#0.000' W'", new CultureInfo("en-US")).PadLeft(9);
		}

		private void CustomNumericUpDowns_ValueChangedCurrent(object sender, EventArgs e)
		{
			double current = (double)customNumericUpDown4.Value + (double)customNumericUpDown5.Value / 10 + (double)customNumericUpDown6.Value / 100 + (double)customNumericUpDown7.Value / 1000;
			psu.SetpointI = current;
			lblSetpointWatt.Text = (psu.SetpointV * psu.SetpointI).ToString("#0.000' W'", new CultureInfo("en-US")).PadLeft(9);
		}

		private void CbOutEnable_CheckedChanged(object sender, EventArgs e)
		{
			psu.OutputEnabled = cbOutEnable.Checked;
		}

		private void CbLockInputs_CheckedChanged(object sender, EventArgs e)
		{
			EnableInputs((!cbLockInputs.Checked) && psu.IsConnected);
		}

		private void TmrRateTimer_Tick(object sender, EventArgs e)
		{
			lblOutRateDisplay.Text = ((double)updatesCounter / ((double)tmrRateTimer.Interval / 1000.0)).ToString("#0", new CultureInfo("en-US")).PadLeft(2) + " updates/sec";
			updatesCounter = 0;
		}

		private void btnLoad_Click(object sender, EventArgs e)
		{
			byte index = byte.Parse(((Button)sender).Text.Split(' ')[1]);
			psu.Recall(index);
			cbOutEnable.Checked = false;
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			byte index = byte.Parse(((Button)sender).Text.Split(' ')[1]);
			psu.Save(index);
			cbOutEnable.Checked = false;
		}
	}
}
