#region License
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

namespace pscontrol
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.cmbbxComList = new System.Windows.Forms.ComboBox();
			this.btnComRefresh = new System.Windows.Forms.Button();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.btnComConnect = new System.Windows.Forms.Button();
			this.lblSetpointWatt = new System.Windows.Forms.Label();
			this.cbOutEnable = new System.Windows.Forms.CheckBox();
			this.lblOutVolt = new System.Windows.Forms.Label();
			this.lblOutAmp = new System.Windows.Forms.Label();
			this.lblOutWatt = new System.Windows.Forms.Label();
			this.lblOutOhm = new System.Windows.Forms.Label();
			this.cbIndicatorOutEnabled = new System.Windows.Forms.CheckBox();
			this.cbIndicatorCV = new System.Windows.Forms.CheckBox();
			this.cbIndicatorCC = new System.Windows.Forms.CheckBox();
			this.cbLockInputs = new System.Windows.Forms.CheckBox();
			this.gbPsuSetpoints = new System.Windows.Forms.GroupBox();
			this.cnudVoltSetpoint1 = new pscontrol.CustomNumericUpDown();
			this.cnudVoltSetpoint2 = new pscontrol.CustomNumericUpDown();
			this.cnudVoltSetpoint3 = new pscontrol.CustomNumericUpDown();
			this.cnudAmpSetpoint1 = new pscontrol.CustomNumericUpDown();
			this.cnudAmpSetpoint2 = new pscontrol.CustomNumericUpDown();
			this.cnudAmpSetpoint3 = new pscontrol.CustomNumericUpDown();
			this.cnudAmpSetpoint4 = new pscontrol.CustomNumericUpDown();
			this.tmrSecondTimer = new System.Windows.Forms.Timer(this.components);
			this.lblOutRateDisplay = new System.Windows.Forms.Label();
			this.gbCommsSetup = new System.Windows.Forms.GroupBox();
			this.gbPsuOutputs = new System.Windows.Forms.GroupBox();
			this.gbMemoryButtons = new System.Windows.Forms.GroupBox();
			this.btnSave5 = new System.Windows.Forms.Button();
			this.btnLoad5 = new System.Windows.Forms.Button();
			this.btnSave4 = new System.Windows.Forms.Button();
			this.btnLoad4 = new System.Windows.Forms.Button();
			this.btnSave3 = new System.Windows.Forms.Button();
			this.btnLoad3 = new System.Windows.Forms.Button();
			this.btnSave2 = new System.Windows.Forms.Button();
			this.btnLoad2 = new System.Windows.Forms.Button();
			this.btnSave1 = new System.Windows.Forms.Button();
			this.btnLoad1 = new System.Windows.Forms.Button();
			this.gbRecordPlayback = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cnudLogIntervalSeconds = new pscontrol.CustomNumericUpDown();
			this.cnudLogIntervalMinutes = new pscontrol.CustomNumericUpDown();
			this.btnStartStopLog = new System.Windows.Forms.Button();
			this.lblShowMore = new System.Windows.Forms.Label();
			this.ofdLogToFile = new System.Windows.Forms.OpenFileDialog();
			this.statusStrip1.SuspendLayout();
			this.gbPsuSetpoints.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cnudVoltSetpoint1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cnudVoltSetpoint2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cnudVoltSetpoint3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cnudAmpSetpoint1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cnudAmpSetpoint2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cnudAmpSetpoint3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cnudAmpSetpoint4)).BeginInit();
			this.gbCommsSetup.SuspendLayout();
			this.gbPsuOutputs.SuspendLayout();
			this.gbMemoryButtons.SuspendLayout();
			this.gbRecordPlayback.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cnudLogIntervalSeconds)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cnudLogIntervalMinutes)).BeginInit();
			this.SuspendLayout();
			// 
			// cmbbxComList
			// 
			this.cmbbxComList.DropDownWidth = 245;
			this.cmbbxComList.FormattingEnabled = true;
			this.cmbbxComList.Location = new System.Drawing.Point(9, 17);
			this.cmbbxComList.MaxDropDownItems = 10;
			this.cmbbxComList.Name = "cmbbxComList";
			this.cmbbxComList.Size = new System.Drawing.Size(147, 21);
			this.cmbbxComList.TabIndex = 1;
			// 
			// btnComRefresh
			// 
			this.btnComRefresh.Location = new System.Drawing.Point(160, 16);
			this.btnComRefresh.Name = "btnComRefresh";
			this.btnComRefresh.Size = new System.Drawing.Size(21, 23);
			this.btnComRefresh.TabIndex = 2;
			this.btnComRefresh.Text = "R";
			this.btnComRefresh.UseVisualStyleBackColor = true;
			this.btnComRefresh.Click += new System.EventHandler(this.BtnComRefreshList_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 170);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(479, 22);
			this.statusStrip1.TabIndex = 3;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(13, 17);
			this.toolStripStatusLabel1.Text = "1";
			// 
			// btnComConnect
			// 
			this.btnComConnect.Location = new System.Drawing.Point(185, 16);
			this.btnComConnect.Name = "btnComConnect";
			this.btnComConnect.Size = new System.Drawing.Size(70, 23);
			this.btnComConnect.TabIndex = 0;
			this.btnComConnect.Text = "Connect";
			this.btnComConnect.UseVisualStyleBackColor = true;
			this.btnComConnect.Click += new System.EventHandler(this.BtnComConnect_Click);
			// 
			// lblSetpointWatt
			// 
			this.lblSetpointWatt.AutoSize = true;
			this.lblSetpointWatt.BackColor = System.Drawing.SystemColors.Window;
			this.lblSetpointWatt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblSetpointWatt.Font = new System.Drawing.Font("Lucida Console", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSetpointWatt.Location = new System.Drawing.Point(9, 74);
			this.lblSetpointWatt.Name = "lblSetpointWatt";
			this.lblSetpointWatt.Size = new System.Drawing.Size(90, 17);
			this.lblSetpointWatt.TabIndex = 16;
			this.lblSetpointWatt.Text = "  0.000 W";
			// 
			// cbOutEnable
			// 
			this.cbOutEnable.AutoSize = true;
			this.cbOutEnable.Location = new System.Drawing.Point(158, 58);
			this.cbOutEnable.Name = "cbOutEnable";
			this.cbOutEnable.Size = new System.Drawing.Size(91, 17);
			this.cbOutEnable.TabIndex = 10;
			this.cbOutEnable.Text = "enable output";
			this.cbOutEnable.UseVisualStyleBackColor = true;
			this.cbOutEnable.CheckedChanged += new System.EventHandler(this.CbOutEnable_CheckedChanged);
			// 
			// lblOutVolt
			// 
			this.lblOutVolt.AutoSize = true;
			this.lblOutVolt.Font = new System.Drawing.Font("Lucida Console", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblOutVolt.Location = new System.Drawing.Point(42, 11);
			this.lblOutVolt.Name = "lblOutVolt";
			this.lblOutVolt.Size = new System.Drawing.Size(156, 27);
			this.lblOutVolt.TabIndex = 17;
			this.lblOutVolt.Text = "  -.--  V";
			// 
			// lblOutAmp
			// 
			this.lblOutAmp.AutoSize = true;
			this.lblOutAmp.Font = new System.Drawing.Font("Lucida Console", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblOutAmp.Location = new System.Drawing.Point(42, 51);
			this.lblOutAmp.Name = "lblOutAmp";
			this.lblOutAmp.Size = new System.Drawing.Size(156, 27);
			this.lblOutAmp.TabIndex = 18;
			this.lblOutAmp.Text = "  -.--- A";
			// 
			// lblOutWatt
			// 
			this.lblOutWatt.AutoSize = true;
			this.lblOutWatt.Font = new System.Drawing.Font("Lucida Console", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblOutWatt.Location = new System.Drawing.Point(42, 91);
			this.lblOutWatt.Name = "lblOutWatt";
			this.lblOutWatt.Size = new System.Drawing.Size(156, 27);
			this.lblOutWatt.TabIndex = 19;
			this.lblOutWatt.Text = "  -.--- W";
			// 
			// lblOutOhm
			// 
			this.lblOutOhm.AutoSize = true;
			this.lblOutOhm.Font = new System.Drawing.Font("Lucida Console", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblOutOhm.Location = new System.Drawing.Point(42, 131);
			this.lblOutOhm.Name = "lblOutOhm";
			this.lblOutOhm.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblOutOhm.Size = new System.Drawing.Size(156, 27);
			this.lblOutOhm.TabIndex = 20;
			this.lblOutOhm.Text = "  -.--- Ω";
			// 
			// cbIndicatorOutEnabled
			// 
			this.cbIndicatorOutEnabled.AutoCheck = false;
			this.cbIndicatorOutEnabled.AutoSize = true;
			this.cbIndicatorOutEnabled.Location = new System.Drawing.Point(6, 66);
			this.cbIndicatorOutEnabled.Name = "cbIndicatorOutEnabled";
			this.cbIndicatorOutEnabled.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cbIndicatorOutEnabled.Size = new System.Drawing.Size(49, 17);
			this.cbIndicatorOutEnabled.TabIndex = 14;
			this.cbIndicatorOutEnabled.Text = "OUT";
			this.cbIndicatorOutEnabled.UseVisualStyleBackColor = true;
			// 
			// cbIndicatorCV
			// 
			this.cbIndicatorCV.AutoCheck = false;
			this.cbIndicatorCV.AutoSize = true;
			this.cbIndicatorCV.Location = new System.Drawing.Point(6, 43);
			this.cbIndicatorCV.Name = "cbIndicatorCV";
			this.cbIndicatorCV.Size = new System.Drawing.Size(43, 17);
			this.cbIndicatorCV.TabIndex = 13;
			this.cbIndicatorCV.Text = "C.V";
			this.cbIndicatorCV.UseVisualStyleBackColor = true;
			// 
			// cbIndicatorCC
			// 
			this.cbIndicatorCC.AutoCheck = false;
			this.cbIndicatorCC.AutoSize = true;
			this.cbIndicatorCC.Location = new System.Drawing.Point(6, 20);
			this.cbIndicatorCC.Name = "cbIndicatorCC";
			this.cbIndicatorCC.Size = new System.Drawing.Size(43, 17);
			this.cbIndicatorCC.TabIndex = 12;
			this.cbIndicatorCC.Text = "C.C";
			this.cbIndicatorCC.UseVisualStyleBackColor = true;
			// 
			// cbLockInputs
			// 
			this.cbLockInputs.AutoSize = true;
			this.cbLockInputs.Location = new System.Drawing.Point(158, 35);
			this.cbLockInputs.Name = "cbLockInputs";
			this.cbLockInputs.Size = new System.Drawing.Size(46, 17);
			this.cbLockInputs.TabIndex = 11;
			this.cbLockInputs.Text = "lock";
			this.cbLockInputs.UseVisualStyleBackColor = true;
			this.cbLockInputs.CheckedChanged += new System.EventHandler(this.CbLockInputs_CheckedChanged);
			// 
			// gbPsuSetpoints
			// 
			this.gbPsuSetpoints.Controls.Add(this.cnudVoltSetpoint1);
			this.gbPsuSetpoints.Controls.Add(this.cnudVoltSetpoint2);
			this.gbPsuSetpoints.Controls.Add(this.cnudVoltSetpoint3);
			this.gbPsuSetpoints.Controls.Add(this.lblSetpointWatt);
			this.gbPsuSetpoints.Controls.Add(this.cnudAmpSetpoint1);
			this.gbPsuSetpoints.Controls.Add(this.cnudAmpSetpoint2);
			this.gbPsuSetpoints.Controls.Add(this.cnudAmpSetpoint3);
			this.gbPsuSetpoints.Controls.Add(this.cnudAmpSetpoint4);
			this.gbPsuSetpoints.Location = new System.Drawing.Point(5, 1);
			this.gbPsuSetpoints.Name = "gbPsuSetpoints";
			this.gbPsuSetpoints.Size = new System.Drawing.Size(160, 100);
			this.gbPsuSetpoints.TabIndex = 1;
			this.gbPsuSetpoints.TabStop = false;
			this.gbPsuSetpoints.Text = "Setpoints";
			// 
			// cnudVoltSetpoint1
			// 
			this.cnudVoltSetpoint1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cnudVoltSetpoint1.Location = new System.Drawing.Point(9, 16);
			this.cnudVoltSetpoint1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.cnudVoltSetpoint1.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
			this.cnudVoltSetpoint1.Name = "cnudVoltSetpoint1";
			this.cnudVoltSetpoint1.Size = new System.Drawing.Size(42, 22);
			this.cnudVoltSetpoint1.TabIndex = 3;
			this.cnudVoltSetpoint1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.cnudVoltSetpoint1.ValueNoOnValueChanged = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.cnudVoltSetpoint1.ValueChanged += new System.EventHandler(this.CnudVoltSetpoint_ValueChangeds);
			// 
			// cnudVoltSetpoint2
			// 
			this.cnudVoltSetpoint2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cnudVoltSetpoint2.Location = new System.Drawing.Point(57, 16);
			this.cnudVoltSetpoint2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.cnudVoltSetpoint2.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
			this.cnudVoltSetpoint2.Name = "cnudVoltSetpoint2";
			this.cnudVoltSetpoint2.Size = new System.Drawing.Size(29, 22);
			this.cnudVoltSetpoint2.TabIndex = 4;
			this.cnudVoltSetpoint2.ValueNoOnValueChanged = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.cnudVoltSetpoint2.ValueChanged += new System.EventHandler(this.CnudVoltSetpoint_ValueChangeds);
			// 
			// cnudVoltSetpoint3
			// 
			this.cnudVoltSetpoint3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cnudVoltSetpoint3.Location = new System.Drawing.Point(87, 16);
			this.cnudVoltSetpoint3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.cnudVoltSetpoint3.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
			this.cnudVoltSetpoint3.Name = "cnudVoltSetpoint3";
			this.cnudVoltSetpoint3.Size = new System.Drawing.Size(29, 22);
			this.cnudVoltSetpoint3.TabIndex = 5;
			this.cnudVoltSetpoint3.ValueNoOnValueChanged = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.cnudVoltSetpoint3.ValueChanged += new System.EventHandler(this.CnudVoltSetpoint_ValueChangeds);
			// 
			// cnudAmpSetpoint1
			// 
			this.cnudAmpSetpoint1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cnudAmpSetpoint1.Location = new System.Drawing.Point(22, 46);
			this.cnudAmpSetpoint1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.cnudAmpSetpoint1.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.cnudAmpSetpoint1.Name = "cnudAmpSetpoint1";
			this.cnudAmpSetpoint1.Size = new System.Drawing.Size(29, 22);
			this.cnudAmpSetpoint1.TabIndex = 6;
			this.cnudAmpSetpoint1.ValueNoOnValueChanged = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.cnudAmpSetpoint1.ValueChanged += new System.EventHandler(this.CnudAmpSetpoint_ValueChangeds);
			// 
			// cnudAmpSetpoint2
			// 
			this.cnudAmpSetpoint2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cnudAmpSetpoint2.Location = new System.Drawing.Point(57, 46);
			this.cnudAmpSetpoint2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.cnudAmpSetpoint2.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
			this.cnudAmpSetpoint2.Name = "cnudAmpSetpoint2";
			this.cnudAmpSetpoint2.Size = new System.Drawing.Size(29, 22);
			this.cnudAmpSetpoint2.TabIndex = 7;
			this.cnudAmpSetpoint2.ValueNoOnValueChanged = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.cnudAmpSetpoint2.ValueChanged += new System.EventHandler(this.CnudAmpSetpoint_ValueChangeds);
			// 
			// cnudAmpSetpoint3
			// 
			this.cnudAmpSetpoint3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cnudAmpSetpoint3.Location = new System.Drawing.Point(117, 46);
			this.cnudAmpSetpoint3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.cnudAmpSetpoint3.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
			this.cnudAmpSetpoint3.Name = "cnudAmpSetpoint3";
			this.cnudAmpSetpoint3.Size = new System.Drawing.Size(29, 22);
			this.cnudAmpSetpoint3.TabIndex = 9;
			this.cnudAmpSetpoint3.ValueNoOnValueChanged = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.cnudAmpSetpoint3.ValueChanged += new System.EventHandler(this.CnudAmpSetpoint_ValueChangeds);
			// 
			// cnudAmpSetpoint4
			// 
			this.cnudAmpSetpoint4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cnudAmpSetpoint4.Location = new System.Drawing.Point(87, 46);
			this.cnudAmpSetpoint4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.cnudAmpSetpoint4.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
			this.cnudAmpSetpoint4.Name = "cnudAmpSetpoint4";
			this.cnudAmpSetpoint4.Size = new System.Drawing.Size(29, 22);
			this.cnudAmpSetpoint4.TabIndex = 8;
			this.cnudAmpSetpoint4.ValueNoOnValueChanged = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.cnudAmpSetpoint4.ValueChanged += new System.EventHandler(this.CnudAmpSetpoint_ValueChangeds);
			// 
			// tmrSecondTimer
			// 
			this.tmrSecondTimer.Interval = 1000;
			this.tmrSecondTimer.Tick += new System.EventHandler(this.TmrSecondTimer_Tick);
			// 
			// lblOutRateDisplay
			// 
			this.lblOutRateDisplay.AutoSize = true;
			this.lblOutRateDisplay.Font = new System.Drawing.Font("Lucida Console", 7F);
			this.lblOutRateDisplay.Location = new System.Drawing.Point(7, 42);
			this.lblOutRateDisplay.Name = "lblOutRateDisplay";
			this.lblOutRateDisplay.Size = new System.Drawing.Size(89, 10);
			this.lblOutRateDisplay.TabIndex = 22;
			this.lblOutRateDisplay.Text = " - updates/sec";
			// 
			// gbCommsSetup
			// 
			this.gbCommsSetup.Controls.Add(this.lblOutRateDisplay);
			this.gbCommsSetup.Controls.Add(this.cmbbxComList);
			this.gbCommsSetup.Controls.Add(this.btnComRefresh);
			this.gbCommsSetup.Controls.Add(this.btnComConnect);
			this.gbCommsSetup.Location = new System.Drawing.Point(5, 107);
			this.gbCommsSetup.Name = "gbCommsSetup";
			this.gbCommsSetup.Size = new System.Drawing.Size(261, 59);
			this.gbCommsSetup.TabIndex = 0;
			this.gbCommsSetup.TabStop = false;
			this.gbCommsSetup.Text = "Comms";
			// 
			// gbPsuOutputs
			// 
			this.gbPsuOutputs.Controls.Add(this.cbIndicatorOutEnabled);
			this.gbPsuOutputs.Controls.Add(this.cbIndicatorCC);
			this.gbPsuOutputs.Controls.Add(this.lblOutOhm);
			this.gbPsuOutputs.Controls.Add(this.cbIndicatorCV);
			this.gbPsuOutputs.Controls.Add(this.lblOutWatt);
			this.gbPsuOutputs.Controls.Add(this.lblOutVolt);
			this.gbPsuOutputs.Controls.Add(this.lblOutAmp);
			this.gbPsuOutputs.Location = new System.Drawing.Point(272, 1);
			this.gbPsuOutputs.Name = "gbPsuOutputs";
			this.gbPsuOutputs.Size = new System.Drawing.Size(202, 165);
			this.gbPsuOutputs.TabIndex = 2;
			this.gbPsuOutputs.TabStop = false;
			this.gbPsuOutputs.Text = "Output";
			// 
			// gbMemoryButtons
			// 
			this.gbMemoryButtons.Controls.Add(this.btnSave5);
			this.gbMemoryButtons.Controls.Add(this.btnLoad5);
			this.gbMemoryButtons.Controls.Add(this.btnSave4);
			this.gbMemoryButtons.Controls.Add(this.btnLoad4);
			this.gbMemoryButtons.Controls.Add(this.btnSave3);
			this.gbMemoryButtons.Controls.Add(this.btnLoad3);
			this.gbMemoryButtons.Controls.Add(this.btnSave2);
			this.gbMemoryButtons.Controls.Add(this.btnLoad2);
			this.gbMemoryButtons.Controls.Add(this.btnSave1);
			this.gbMemoryButtons.Controls.Add(this.btnLoad1);
			this.gbMemoryButtons.Location = new System.Drawing.Point(480, 1);
			this.gbMemoryButtons.Name = "gbMemoryButtons";
			this.gbMemoryButtons.Size = new System.Drawing.Size(120, 165);
			this.gbMemoryButtons.TabIndex = 4;
			this.gbMemoryButtons.TabStop = false;
			this.gbMemoryButtons.Text = "Memory";
			// 
			// btnSave5
			// 
			this.btnSave5.Location = new System.Drawing.Point(62, 136);
			this.btnSave5.Name = "btnSave5";
			this.btnSave5.Size = new System.Drawing.Size(52, 23);
			this.btnSave5.TabIndex = 9;
			this.btnSave5.Text = "Save 5";
			this.btnSave5.UseVisualStyleBackColor = true;
			this.btnSave5.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnLoad5
			// 
			this.btnLoad5.Location = new System.Drawing.Point(6, 136);
			this.btnLoad5.Name = "btnLoad5";
			this.btnLoad5.Size = new System.Drawing.Size(52, 23);
			this.btnLoad5.TabIndex = 8;
			this.btnLoad5.Text = "Load 5";
			this.btnLoad5.UseVisualStyleBackColor = true;
			this.btnLoad5.Click += new System.EventHandler(this.btnLoad_Click);
			// 
			// btnSave4
			// 
			this.btnSave4.Location = new System.Drawing.Point(62, 107);
			this.btnSave4.Name = "btnSave4";
			this.btnSave4.Size = new System.Drawing.Size(52, 23);
			this.btnSave4.TabIndex = 7;
			this.btnSave4.Text = "Save 4";
			this.btnSave4.UseVisualStyleBackColor = true;
			this.btnSave4.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnLoad4
			// 
			this.btnLoad4.Location = new System.Drawing.Point(6, 107);
			this.btnLoad4.Name = "btnLoad4";
			this.btnLoad4.Size = new System.Drawing.Size(52, 23);
			this.btnLoad4.TabIndex = 6;
			this.btnLoad4.Text = "Load 4";
			this.btnLoad4.UseVisualStyleBackColor = true;
			this.btnLoad4.Click += new System.EventHandler(this.btnLoad_Click);
			// 
			// btnSave3
			// 
			this.btnSave3.Location = new System.Drawing.Point(62, 78);
			this.btnSave3.Name = "btnSave3";
			this.btnSave3.Size = new System.Drawing.Size(52, 23);
			this.btnSave3.TabIndex = 5;
			this.btnSave3.Text = "Save 3";
			this.btnSave3.UseVisualStyleBackColor = true;
			this.btnSave3.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnLoad3
			// 
			this.btnLoad3.Location = new System.Drawing.Point(6, 78);
			this.btnLoad3.Name = "btnLoad3";
			this.btnLoad3.Size = new System.Drawing.Size(52, 23);
			this.btnLoad3.TabIndex = 4;
			this.btnLoad3.Text = "Load 3";
			this.btnLoad3.UseVisualStyleBackColor = true;
			this.btnLoad3.Click += new System.EventHandler(this.btnLoad_Click);
			// 
			// btnSave2
			// 
			this.btnSave2.Location = new System.Drawing.Point(62, 49);
			this.btnSave2.Name = "btnSave2";
			this.btnSave2.Size = new System.Drawing.Size(52, 23);
			this.btnSave2.TabIndex = 3;
			this.btnSave2.Text = "Save 2";
			this.btnSave2.UseVisualStyleBackColor = true;
			this.btnSave2.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnLoad2
			// 
			this.btnLoad2.Location = new System.Drawing.Point(6, 49);
			this.btnLoad2.Name = "btnLoad2";
			this.btnLoad2.Size = new System.Drawing.Size(52, 23);
			this.btnLoad2.TabIndex = 2;
			this.btnLoad2.Text = "Load 2";
			this.btnLoad2.UseVisualStyleBackColor = true;
			this.btnLoad2.Click += new System.EventHandler(this.btnLoad_Click);
			// 
			// btnSave1
			// 
			this.btnSave1.Location = new System.Drawing.Point(62, 20);
			this.btnSave1.Name = "btnSave1";
			this.btnSave1.Size = new System.Drawing.Size(52, 23);
			this.btnSave1.TabIndex = 1;
			this.btnSave1.Text = "Save 1";
			this.btnSave1.UseVisualStyleBackColor = true;
			this.btnSave1.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnLoad1
			// 
			this.btnLoad1.Location = new System.Drawing.Point(6, 20);
			this.btnLoad1.Name = "btnLoad1";
			this.btnLoad1.Size = new System.Drawing.Size(52, 23);
			this.btnLoad1.TabIndex = 0;
			this.btnLoad1.Text = "Load 1";
			this.btnLoad1.UseVisualStyleBackColor = true;
			this.btnLoad1.Click += new System.EventHandler(this.btnLoad_Click);
			// 
			// gbRecordPlayback
			// 
			this.gbRecordPlayback.Controls.Add(this.label1);
			this.gbRecordPlayback.Controls.Add(this.cnudLogIntervalSeconds);
			this.gbRecordPlayback.Controls.Add(this.cnudLogIntervalMinutes);
			this.gbRecordPlayback.Controls.Add(this.btnStartStopLog);
			this.gbRecordPlayback.Location = new System.Drawing.Point(606, 1);
			this.gbRecordPlayback.Name = "gbRecordPlayback";
			this.gbRecordPlayback.Size = new System.Drawing.Size(110, 165);
			this.gbRecordPlayback.TabIndex = 12;
			this.gbRecordPlayback.TabStop = false;
			this.gbRecordPlayback.Text = "Record/Playback";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 46);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(90, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "Log interval: (m s)";
			// 
			// cnudLogIntervalSeconds
			// 
			this.cnudLogIntervalSeconds.Location = new System.Drawing.Point(39, 62);
			this.cnudLogIntervalSeconds.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
			this.cnudLogIntervalSeconds.Name = "cnudLogIntervalSeconds";
			this.cnudLogIntervalSeconds.Size = new System.Drawing.Size(32, 20);
			this.cnudLogIntervalSeconds.TabIndex = 4;
			this.cnudLogIntervalSeconds.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.cnudLogIntervalSeconds.ValueNoOnValueChanged = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.cnudLogIntervalSeconds.ValueChanged += new System.EventHandler(this.cnudLogIntervals_ValueChanged);
			// 
			// cnudLogIntervalMinutes
			// 
			this.cnudLogIntervalMinutes.Location = new System.Drawing.Point(6, 62);
			this.cnudLogIntervalMinutes.Name = "cnudLogIntervalMinutes";
			this.cnudLogIntervalMinutes.Size = new System.Drawing.Size(32, 20);
			this.cnudLogIntervalMinutes.TabIndex = 3;
			this.cnudLogIntervalMinutes.ValueNoOnValueChanged = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.cnudLogIntervalMinutes.ValueChanged += new System.EventHandler(this.cnudLogIntervals_ValueChanged);
			// 
			// btnStartStopLog
			// 
			this.btnStartStopLog.Location = new System.Drawing.Point(6, 20);
			this.btnStartStopLog.Name = "btnStartStopLog";
			this.btnStartStopLog.Size = new System.Drawing.Size(75, 23);
			this.btnStartStopLog.TabIndex = 1;
			this.btnStartStopLog.Text = "Start Log";
			this.btnStartStopLog.UseVisualStyleBackColor = true;
			this.btnStartStopLog.Click += new System.EventHandler(this.btnStartStopLog_Click);
			// 
			// lblShowMore
			// 
			this.lblShowMore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lblShowMore.AutoSize = true;
			this.lblShowMore.BackColor = System.Drawing.Color.Transparent;
			this.lblShowMore.Cursor = System.Windows.Forms.Cursors.Hand;
			this.lblShowMore.Location = new System.Drawing.Point(430, 173);
			this.lblShowMore.Name = "lblShowMore";
			this.lblShowMore.Size = new System.Drawing.Size(43, 13);
			this.lblShowMore.TabIndex = 13;
			this.lblShowMore.Text = "More>>";
			this.lblShowMore.Click += new System.EventHandler(this.lblShowMore_Click);
			// 
			// ofdLogToFile
			// 
			this.ofdLogToFile.CheckFileExists = false;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(479, 192);
			this.Controls.Add(this.lblShowMore);
			this.Controls.Add(this.gbRecordPlayback);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.cbLockInputs);
			this.Controls.Add(this.cbOutEnable);
			this.Controls.Add(this.gbPsuSetpoints);
			this.Controls.Add(this.gbCommsSetup);
			this.Controls.Add(this.gbPsuOutputs);
			this.Controls.Add(this.gbMemoryButtons);
			this.MaximumSize = new System.Drawing.Size(738, 231);
			this.MinimumSize = new System.Drawing.Size(287, 60);
			this.Name = "Form1";
			this.ShowIcon = false;
			this.Text = "psControl   v";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.gbPsuSetpoints.ResumeLayout(false);
			this.gbPsuSetpoints.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.cnudVoltSetpoint1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cnudVoltSetpoint2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cnudVoltSetpoint3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cnudAmpSetpoint1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cnudAmpSetpoint2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cnudAmpSetpoint3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cnudAmpSetpoint4)).EndInit();
			this.gbCommsSetup.ResumeLayout(false);
			this.gbCommsSetup.PerformLayout();
			this.gbPsuOutputs.ResumeLayout(false);
			this.gbPsuOutputs.PerformLayout();
			this.gbMemoryButtons.ResumeLayout(false);
			this.gbRecordPlayback.ResumeLayout(false);
			this.gbRecordPlayback.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.cnudLogIntervalSeconds)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cnudLogIntervalMinutes)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.ComboBox cmbbxComList;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private CustomNumericUpDown cnudVoltSetpoint1;
		private CustomNumericUpDown cnudVoltSetpoint2;
		private CustomNumericUpDown cnudVoltSetpoint3;
		private CustomNumericUpDown cnudAmpSetpoint1;
		private CustomNumericUpDown cnudAmpSetpoint2;
		private CustomNumericUpDown cnudAmpSetpoint4;
		private CustomNumericUpDown cnudAmpSetpoint3;
		private System.Windows.Forms.Label lblSetpointWatt;
		private System.Windows.Forms.Label lblOutVolt;
		private System.Windows.Forms.Label lblOutAmp;
		private System.Windows.Forms.Label lblOutWatt;
		private System.Windows.Forms.Label lblOutOhm;
		private System.Windows.Forms.CheckBox cbOutEnable;
		private System.Windows.Forms.CheckBox cbIndicatorOutEnabled;
		private System.Windows.Forms.CheckBox cbIndicatorCV;
		private System.Windows.Forms.CheckBox cbIndicatorCC;
		private System.Windows.Forms.CheckBox cbLockInputs;
		private System.Windows.Forms.Timer tmrSecondTimer;
		private System.Windows.Forms.Label lblOutRateDisplay;
		private System.Windows.Forms.GroupBox gbPsuSetpoints;
		private System.Windows.Forms.GroupBox gbCommsSetup;
		private System.Windows.Forms.GroupBox gbPsuOutputs;
		private System.Windows.Forms.GroupBox gbMemoryButtons;
		private System.Windows.Forms.Button btnComRefresh;
		private System.Windows.Forms.Button btnComConnect;
		private System.Windows.Forms.Button btnLoad1;
		private System.Windows.Forms.Button btnSave1;
		private System.Windows.Forms.Button btnSave2;
		private System.Windows.Forms.Button btnLoad2;
		private System.Windows.Forms.Button btnSave3;
		private System.Windows.Forms.Button btnLoad3;
		private System.Windows.Forms.Button btnSave4;
		private System.Windows.Forms.Button btnLoad4;
		private System.Windows.Forms.Button btnSave5;
		private System.Windows.Forms.Button btnLoad5;
		private System.Windows.Forms.GroupBox gbRecordPlayback;
		private System.Windows.Forms.Button btnStartStopLog;
		private System.Windows.Forms.Label lblShowMore;
		private System.Windows.Forms.OpenFileDialog ofdLogToFile;
		private CustomNumericUpDown cnudLogIntervalSeconds;
		private CustomNumericUpDown cnudLogIntervalMinutes;
		private System.Windows.Forms.Label label1;
	}
}

