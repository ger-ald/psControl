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
			this.tmrComTimer = new System.Windows.Forms.Timer(this.components);
			this.lblSetpointWatt = new System.Windows.Forms.Label();
			this.cbOutEnable = new System.Windows.Forms.CheckBox();
			this.lblOutVolt = new System.Windows.Forms.Label();
			this.lblOutAmp = new System.Windows.Forms.Label();
			this.lblOutWatt = new System.Windows.Forms.Label();
			this.lblOutOhm = new System.Windows.Forms.Label();
			this.cbIndicatorOutEnabled = new System.Windows.Forms.CheckBox();
			this.cbIndicatorCV = new System.Windows.Forms.CheckBox();
			this.cbIndicatorCC = new System.Windows.Forms.CheckBox();
			this.lockInputs = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tmrRateTimer = new System.Windows.Forms.Timer(this.components);
			this.lblReplyRateDisplay = new System.Windows.Forms.Label();
			this.lblOutRateDisplay = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
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
			this.customNumericUpDown1 = new pscontrol.CustomNumericUpDown();
			this.customNumericUpDown2 = new pscontrol.CustomNumericUpDown();
			this.customNumericUpDown3 = new pscontrol.CustomNumericUpDown();
			this.customNumericUpDown4 = new pscontrol.CustomNumericUpDown();
			this.customNumericUpDown5 = new pscontrol.CustomNumericUpDown();
			this.customNumericUpDown7 = new pscontrol.CustomNumericUpDown();
			this.customNumericUpDown6 = new pscontrol.CustomNumericUpDown();
			this.statusStrip1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.customNumericUpDown1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.customNumericUpDown2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.customNumericUpDown3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.customNumericUpDown4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.customNumericUpDown5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.customNumericUpDown7)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.customNumericUpDown6)).BeginInit();
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
			this.cmbbxComList.Sorted = true;
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
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 170);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(605, 22);
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
			// tmrComTimer
			// 
			this.tmrComTimer.Interval = 50;
			this.tmrComTimer.Tick += new System.EventHandler(this.TmrComTimer_Tick);
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
			// lockInputs
			// 
			this.lockInputs.AutoSize = true;
			this.lockInputs.Location = new System.Drawing.Point(158, 35);
			this.lockInputs.Name = "lockInputs";
			this.lockInputs.Size = new System.Drawing.Size(46, 17);
			this.lockInputs.TabIndex = 11;
			this.lockInputs.Text = "lock";
			this.lockInputs.UseVisualStyleBackColor = true;
			this.lockInputs.CheckedChanged += new System.EventHandler(this.LockInputs_CheckedChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.customNumericUpDown1);
			this.groupBox1.Controls.Add(this.customNumericUpDown2);
			this.groupBox1.Controls.Add(this.customNumericUpDown3);
			this.groupBox1.Controls.Add(this.lblSetpointWatt);
			this.groupBox1.Controls.Add(this.customNumericUpDown4);
			this.groupBox1.Controls.Add(this.customNumericUpDown5);
			this.groupBox1.Controls.Add(this.customNumericUpDown7);
			this.groupBox1.Controls.Add(this.customNumericUpDown6);
			this.groupBox1.Location = new System.Drawing.Point(5, 1);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(160, 100);
			this.groupBox1.TabIndex = 15;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Setpoints";
			// 
			// tmrRateTimer
			// 
			this.tmrRateTimer.Interval = 1000;
			this.tmrRateTimer.Tick += new System.EventHandler(this.TmrRateTimer_Tick);
			// 
			// lblReplyRateDisplay
			// 
			this.lblReplyRateDisplay.AutoSize = true;
			this.lblReplyRateDisplay.Font = new System.Drawing.Font("Lucida Console", 7F);
			this.lblReplyRateDisplay.Location = new System.Drawing.Point(7, 43);
			this.lblReplyRateDisplay.Name = "lblReplyRateDisplay";
			this.lblReplyRateDisplay.Size = new System.Drawing.Size(89, 10);
			this.lblReplyRateDisplay.TabIndex = 21;
			this.lblReplyRateDisplay.Text = " - replies/sec";
			// 
			// lblOutRateDisplay
			// 
			this.lblOutRateDisplay.AutoSize = true;
			this.lblOutRateDisplay.Font = new System.Drawing.Font("Lucida Console", 7F);
			this.lblOutRateDisplay.Location = new System.Drawing.Point(166, 43);
			this.lblOutRateDisplay.Name = "lblOutRateDisplay";
			this.lblOutRateDisplay.Size = new System.Drawing.Size(89, 10);
			this.lblOutRateDisplay.TabIndex = 22;
			this.lblOutRateDisplay.Text = " - updates/sec";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.lblReplyRateDisplay);
			this.groupBox2.Controls.Add(this.lblOutRateDisplay);
			this.groupBox2.Controls.Add(this.cmbbxComList);
			this.groupBox2.Controls.Add(this.btnComRefresh);
			this.groupBox2.Controls.Add(this.btnComConnect);
			this.groupBox2.Location = new System.Drawing.Point(5, 107);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(261, 59);
			this.groupBox2.TabIndex = 0;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Comms";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.cbIndicatorOutEnabled);
			this.groupBox3.Controls.Add(this.cbIndicatorCC);
			this.groupBox3.Controls.Add(this.lblOutOhm);
			this.groupBox3.Controls.Add(this.cbIndicatorCV);
			this.groupBox3.Controls.Add(this.lblOutWatt);
			this.groupBox3.Controls.Add(this.lblOutVolt);
			this.groupBox3.Controls.Add(this.lblOutAmp);
			this.groupBox3.Location = new System.Drawing.Point(272, 1);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(202, 165);
			this.groupBox3.TabIndex = 24;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Output";
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.btnSave5);
			this.groupBox4.Controls.Add(this.btnLoad5);
			this.groupBox4.Controls.Add(this.btnSave4);
			this.groupBox4.Controls.Add(this.btnLoad4);
			this.groupBox4.Controls.Add(this.btnSave3);
			this.groupBox4.Controls.Add(this.btnLoad3);
			this.groupBox4.Controls.Add(this.btnSave2);
			this.groupBox4.Controls.Add(this.btnLoad2);
			this.groupBox4.Controls.Add(this.btnSave1);
			this.groupBox4.Controls.Add(this.btnLoad1);
			this.groupBox4.Location = new System.Drawing.Point(480, 1);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(120, 165);
			this.groupBox4.TabIndex = 25;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Memory";
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
			// customNumericUpDown1
			// 
			this.customNumericUpDown1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.customNumericUpDown1.Location = new System.Drawing.Point(9, 16);
			this.customNumericUpDown1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.customNumericUpDown1.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
			this.customNumericUpDown1.Name = "customNumericUpDown1";
			this.customNumericUpDown1.Size = new System.Drawing.Size(42, 22);
			this.customNumericUpDown1.TabIndex = 3;
			this.customNumericUpDown1.ValueNoOnValueChanged = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// customNumericUpDown2
			// 
			this.customNumericUpDown2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.customNumericUpDown2.Location = new System.Drawing.Point(57, 16);
			this.customNumericUpDown2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.customNumericUpDown2.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
			this.customNumericUpDown2.Name = "customNumericUpDown2";
			this.customNumericUpDown2.Size = new System.Drawing.Size(29, 22);
			this.customNumericUpDown2.TabIndex = 4;
			this.customNumericUpDown2.ValueNoOnValueChanged = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// customNumericUpDown3
			// 
			this.customNumericUpDown3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.customNumericUpDown3.Location = new System.Drawing.Point(87, 16);
			this.customNumericUpDown3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.customNumericUpDown3.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
			this.customNumericUpDown3.Name = "customNumericUpDown3";
			this.customNumericUpDown3.Size = new System.Drawing.Size(29, 22);
			this.customNumericUpDown3.TabIndex = 5;
			this.customNumericUpDown3.ValueNoOnValueChanged = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// customNumericUpDown4
			// 
			this.customNumericUpDown4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.customNumericUpDown4.Location = new System.Drawing.Point(22, 46);
			this.customNumericUpDown4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.customNumericUpDown4.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.customNumericUpDown4.Name = "customNumericUpDown4";
			this.customNumericUpDown4.Size = new System.Drawing.Size(29, 22);
			this.customNumericUpDown4.TabIndex = 6;
			this.customNumericUpDown4.ValueNoOnValueChanged = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// customNumericUpDown5
			// 
			this.customNumericUpDown5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.customNumericUpDown5.Location = new System.Drawing.Point(57, 46);
			this.customNumericUpDown5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.customNumericUpDown5.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
			this.customNumericUpDown5.Name = "customNumericUpDown5";
			this.customNumericUpDown5.Size = new System.Drawing.Size(29, 22);
			this.customNumericUpDown5.TabIndex = 7;
			this.customNumericUpDown5.ValueNoOnValueChanged = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// customNumericUpDown7
			// 
			this.customNumericUpDown7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.customNumericUpDown7.Location = new System.Drawing.Point(117, 46);
			this.customNumericUpDown7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.customNumericUpDown7.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
			this.customNumericUpDown7.Name = "customNumericUpDown7";
			this.customNumericUpDown7.Size = new System.Drawing.Size(29, 22);
			this.customNumericUpDown7.TabIndex = 9;
			this.customNumericUpDown7.ValueNoOnValueChanged = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// customNumericUpDown6
			// 
			this.customNumericUpDown6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.customNumericUpDown6.Location = new System.Drawing.Point(87, 46);
			this.customNumericUpDown6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.customNumericUpDown6.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
			this.customNumericUpDown6.Name = "customNumericUpDown6";
			this.customNumericUpDown6.Size = new System.Drawing.Size(29, 22);
			this.customNumericUpDown6.TabIndex = 8;
			this.customNumericUpDown6.ValueNoOnValueChanged = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(605, 192);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.lockInputs);
			this.Controls.Add(this.cbOutEnable);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox4);
			this.MaximumSize = new System.Drawing.Size(629, 232);
			this.MinimumSize = new System.Drawing.Size(287, 60);
			this.Name = "Form1";
			this.ShowIcon = false;
			this.Text = "PScontrol";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.customNumericUpDown1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.customNumericUpDown2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.customNumericUpDown3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.customNumericUpDown4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.customNumericUpDown5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.customNumericUpDown7)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.customNumericUpDown6)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.ComboBox cmbbxComList;
		private System.Windows.Forms.Button btnComRefresh;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.Button btnComConnect;
		private System.Windows.Forms.Timer tmrComTimer;
		private CustomNumericUpDown customNumericUpDown1;
		private CustomNumericUpDown customNumericUpDown2;
		private CustomNumericUpDown customNumericUpDown3;
		private System.Windows.Forms.Label lblSetpointWatt;
		private System.Windows.Forms.CheckBox cbOutEnable;
		private CustomNumericUpDown customNumericUpDown4;
		private CustomNumericUpDown customNumericUpDown5;
		private CustomNumericUpDown customNumericUpDown6;
		private CustomNumericUpDown customNumericUpDown7;
		private System.Windows.Forms.Label lblOutVolt;
		private System.Windows.Forms.Label lblOutAmp;
		private System.Windows.Forms.Label lblOutWatt;
		private System.Windows.Forms.Label lblOutOhm;
		private System.Windows.Forms.CheckBox cbIndicatorOutEnabled;
		private System.Windows.Forms.CheckBox cbIndicatorCV;
		private System.Windows.Forms.CheckBox cbIndicatorCC;
		private System.Windows.Forms.CheckBox lockInputs;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Timer tmrRateTimer;
		private System.Windows.Forms.Label lblReplyRateDisplay;
		private System.Windows.Forms.Label lblOutRateDisplay;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Button btnLoad1;
		private System.Windows.Forms.Button btnSave1;
		private System.Windows.Forms.Button btnSave5;
		private System.Windows.Forms.Button btnLoad5;
		private System.Windows.Forms.Button btnSave4;
		private System.Windows.Forms.Button btnLoad4;
		private System.Windows.Forms.Button btnSave3;
		private System.Windows.Forms.Button btnLoad3;
		private System.Windows.Forms.Button btnSave2;
		private System.Windows.Forms.Button btnLoad2;
	}
}

