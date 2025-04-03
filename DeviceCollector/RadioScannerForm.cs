// Decompiled with JetBrains decompiler
// Type: DeviceCollector.RadioScannerForm
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using CorporateDesign;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class RadioScannerForm : Form
  {
    private DeviceCollectorFunctions MyFunctions;
    private bool isStoped;
    private Stopwatch stopwatch;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private Panel panel1;
    private Label label1;
    private Button btnStartScanning;
    private Label label3;
    private TextBox txtTransceiver;
    private Button btnStopScanning;
    private TextBox txtBusMode;
    private StatusStrip statusStrip1;
    private ToolStripStatusLabel txtStatus;
    private RichTextBox txtOutput;
    private Button btnClear;
    private Timer timer;
    private Label label5;
    private NumericUpDown txtInterval;
    private CheckBox ckbTimeInfo;
    private GroupBox groupBox2;
    private GroupBox groupBox1;
    private GroupBox groupBox3;
    private Label label9;
    private Label label8;
    private TextBox txtbxTransmitterQuarzError;
    private Label label6;
    private TextBox txtbxReceiverQuarzError;
    private Label label7;
    private Label label4;
    private Label label2;
    private TextBox txtFilterBySerialNumber;

    public RadioScannerForm(DeviceCollectorFunctions MyFunctions)
    {
      this.MyFunctions = MyFunctions;
      this.isStoped = false;
      this.stopwatch = new Stopwatch();
      this.InitializeComponent();
    }

    private void RadioScannerForm_Load(object sender, EventArgs e)
    {
      this.txtTransceiver.Text = this.MyFunctions.MyCom.Transceiver.ToString();
      this.txtBusMode.Text = this.MyFunctions.GetBaseMode().ToString();
      this.MyFunctions.OnMessage += new EventHandler<GMM_EventArgs>(this.DeviceCollector_OnMessage);
      this.MyFunctions.RadioReader.OnPacketReceived += new EventHandler<RadioPacket>(this.RadioReader_OnPacketReceived);
    }

    private void RadioScannerForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.isStoped = true;
    }

    private void RadioScannerForm_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.timer.Enabled = false;
      this.MyFunctions.OnMessage -= new EventHandler<GMM_EventArgs>(this.DeviceCollector_OnMessage);
      this.MyFunctions.RadioReader.OnPacketReceived -= new EventHandler<RadioPacket>(this.RadioReader_OnPacketReceived);
    }

    private void btnStartScanning_Click(object sender, EventArgs e)
    {
      this.isStoped = false;
      this.btnStartScanning.Enabled = false;
      this.btnStopScanning.Enabled = true;
      this.timer.Enabled = true;
      this.stopwatch.Start();
      if (this.MyFunctions.RadioReader.Read() != null)
        return;
      ZR_ClassLibMessages.ShowAndClearErrors(this.Name, (string) null);
      this.btnStopScanning_Click((object) null, (EventArgs) null);
    }

    private void btnStopScanning_Click(object sender, EventArgs e)
    {
      this.timer.Enabled = false;
      this.isStoped = true;
      this.btnStartScanning.Enabled = true;
      this.btnStopScanning.Enabled = false;
    }

    private void btnClear_Click(object sender, EventArgs e) => this.txtOutput.Clear();

    private void DeviceCollector_OnMessage(object sender, GMM_EventArgs e)
    {
      e.Cancel = this.isStoped;
      if (string.IsNullOrEmpty(e.EventMessage))
        return;
      this.txtStatus.Text = e.EventMessage;
    }

    private void RadioReader_OnPacketReceived(object sender, RadioPacket e)
    {
      if (!(e is RadioDevicePacket))
        return;
      RadioDevicePacket radioDevicePacket = e as RadioDevicePacket;
      int? rssiDBm = radioDevicePacket.RSSI_dBm;
      int num1 = 0;
      if (rssiDBm.GetValueOrDefault() == num1 & rssiDBm.HasValue && this.MyFunctions.MyCom.Transceiver != TransceiverDevice.MinoHead || !string.IsNullOrEmpty(this.txtFilterBySerialNumber.Text) && !radioDevicePacket.FunkId.ToString().StartsWith(this.txtFilterBySerialNumber.Text.Trim()))
        return;
      this.stopwatch.Stop();
      string text1 = string.Empty;
      string text2 = string.Empty;
      if (this.MyFunctions.RadioReader.ReceivedData != null && this.MyFunctions.RadioReader.ReceivedData.ContainsKey(radioDevicePacket.FunkId))
      {
        RadioDataSet radioDataSet = this.MyFunctions.RadioReader.ReceivedData[radioDevicePacket.FunkId];
        if (radioDataSet.LastRadioPacket != null)
        {
          if (!(radioDataSet.LastRadioPacket is RadioDevicePacket lastRadioPacket))
            return;
          double num2 = (double) (radioDevicePacket.MCT - lastRadioPacket.MCT) / 1800.0 / ((1.0 + (double) Convert.ToInt16(this.txtbxTransmitterQuarzError.Text) / 1000000.0) * (1.0 + (double) Convert.ToInt16(this.txtbxReceiverQuarzError.Text) / 1000000.0));
          double totalSeconds = (radioDevicePacket.ReceivedAt - lastRadioPacket.ReceivedAt).TotalSeconds;
          byte? acc;
          int num3;
          if (radioDevicePacket is RadioPacketWirelessMBus)
          {
            acc = ((RadioPacketWirelessMBus) radioDevicePacket).ACC;
            num3 = acc.HasValue ? 1 : 0;
          }
          else
            num3 = 0;
          if (num3 != 0)
          {
            double num4 = (double) lastRadioPacket.MCT / 1800.0;
            double num5 = (double) radioDevicePacket.MCT / 1800.0;
            acc = ((RadioPacketWirelessMBus) radioDevicePacket).ACC;
            double num6 = (double) acc.Value;
            acc = ((RadioPacketWirelessMBus) lastRadioPacket).ACC;
            double num7 = (double) acc.Value;
            double num8 = Convert.ToDouble(this.txtInterval.Value);
            double num9 = (1.0 + (Math.Abs(num7 - 128.0) - 64.0) / 2048.0) * num8;
            double num10 = num2 - num9;
            text1 = string.Format("ACC: {0} Tnom: {1} Actual MiCon: {2:0.0000} Actual PC: {3:0.0000} Expected interval: {4:0.0000} Difference: {5:0.0000} sec -> {6:0.0000} % ", (object) num6, (object) num8, (object) num2, (object) totalSeconds, (object) num9, (object) num10, (object) (num2 * 100.0 / num9 - 100.0));
            if (this.ckbTimeInfo.Checked)
              text2 = string.Format("ID: {0:00000000} | ACC: {1:000} | Sync.: {2} | Tnom: {3} | MiConDiff: {4:0.00000} | Expected: {5:0.00000} | Diff: {6:0.00000}", (object) radioDevicePacket.FunkId, (object) num6, (object) ((RadioPacketWirelessMBus) radioDevicePacket).Synchronous, (object) num8, (object) num2, (object) num9, (object) (num2 - num9));
          }
        }
      }
      if (!this.ckbTimeInfo.Checked)
      {
        this.txtOutput.AppendText(SystemValues.DateTimeNow.ToString("g"));
        this.txtOutput.AppendText(" ");
        this.txtOutput.AppendText(radioDevicePacket.ToString());
        this.txtOutput.AppendText(text1);
        this.txtOutput.AppendText("\n");
      }
      else
      {
        this.txtOutput.AppendText(SystemValues.DateTimeNow.ToString("g"));
        this.txtOutput.AppendText(" ");
        this.txtOutput.AppendText(text2);
        this.txtOutput.AppendText("\n");
      }
      this.stopwatch.Reset();
      this.stopwatch.Start();
    }

    private void timer_Tick(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.txtFilterBySerialNumber.Text) || !this.stopwatch.IsRunning || this.stopwatch.Elapsed.TotalSeconds <= (double) this.txtInterval.Value)
        return;
      this.txtOutput.AppendText(SystemValues.DateTimeNow.ToString("HH:mm:ss"));
      this.txtOutput.AppendText("   ");
      this.txtOutput.AppendText("Kein_Empfang\n");
      this.stopwatch.Reset();
      this.stopwatch.Start();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (RadioScannerForm));
      this.panel1 = new Panel();
      this.groupBox2 = new GroupBox();
      this.ckbTimeInfo = new CheckBox();
      this.txtInterval = new NumericUpDown();
      this.label5 = new Label();
      this.btnClear = new Button();
      this.txtOutput = new RichTextBox();
      this.btnStopScanning = new Button();
      this.btnStartScanning = new Button();
      this.txtBusMode = new TextBox();
      this.label3 = new Label();
      this.txtTransceiver = new TextBox();
      this.label1 = new Label();
      this.statusStrip1 = new StatusStrip();
      this.txtStatus = new ToolStripStatusLabel();
      this.timer = new Timer(this.components);
      this.groupBox1 = new GroupBox();
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.txtFilterBySerialNumber = new TextBox();
      this.label2 = new Label();
      this.label7 = new Label();
      this.txtbxReceiverQuarzError = new TextBox();
      this.label6 = new Label();
      this.txtbxTransmitterQuarzError = new TextBox();
      this.label8 = new Label();
      this.label9 = new Label();
      this.groupBox3 = new GroupBox();
      this.label4 = new Label();
      this.panel1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.txtInterval.BeginInit();
      this.statusStrip1.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.SuspendLayout();
      this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel1.Controls.Add((Control) this.groupBox2);
      this.panel1.Controls.Add((Control) this.btnClear);
      this.panel1.Controls.Add((Control) this.txtOutput);
      this.panel1.Controls.Add((Control) this.btnStopScanning);
      this.panel1.Controls.Add((Control) this.btnStartScanning);
      this.panel1.Location = new Point(1, 41);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(886, 483);
      this.panel1.TabIndex = 2;
      this.groupBox2.Controls.Add((Control) this.groupBox3);
      this.groupBox2.Controls.Add((Control) this.label4);
      this.groupBox2.Controls.Add((Control) this.label2);
      this.groupBox2.Controls.Add((Control) this.txtFilterBySerialNumber);
      this.groupBox2.Controls.Add((Control) this.txtInterval);
      this.groupBox2.Controls.Add((Control) this.label5);
      this.groupBox2.Location = new Point(274, 0);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(411, 94);
      this.groupBox2.TabIndex = 22;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Settings";
      this.ckbTimeInfo.AutoSize = true;
      this.ckbTimeInfo.Location = new Point(8, 68);
      this.ckbTimeInfo.Name = "ckbTimeInfo";
      this.ckbTimeInfo.RightToLeft = RightToLeft.Yes;
      this.ckbTimeInfo.Size = new Size(130, 17);
      this.ckbTimeInfo.TabIndex = 17;
      this.ckbTimeInfo.Text = "Time informations only";
      this.ckbTimeInfo.UseVisualStyleBackColor = true;
      this.txtInterval.Location = new Point(86, 59);
      this.txtInterval.Maximum = new Decimal(new int[4]
      {
        10000,
        0,
        0,
        0
      });
      this.txtInterval.Name = "txtInterval";
      this.txtInterval.Size = new Size(46, 20);
      this.txtInterval.TabIndex = 14;
      this.txtInterval.Value = new Decimal(new int[4]
      {
        180,
        0,
        0,
        0
      });
      this.label5.AutoSize = true;
      this.label5.Location = new Point(138, 63);
      this.label5.Name = "label5";
      this.label5.Size = new Size(24, 13);
      this.label5.TabIndex = 16;
      this.label5.Text = "sec";
      this.btnClear.Location = new Point(11, 65);
      this.btnClear.Name = "btnClear";
      this.btnClear.Size = new Size(67, 23);
      this.btnClear.TabIndex = 13;
      this.btnClear.Text = "Clear";
      this.btnClear.UseVisualStyleBackColor = true;
      this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
      this.txtOutput.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtOutput.Font = new Font("Courier New", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtOutput.Location = new Point(11, 94);
      this.txtOutput.Name = "txtOutput";
      this.txtOutput.Size = new Size(863, 386);
      this.txtOutput.TabIndex = 12;
      this.txtOutput.Text = "";
      this.btnStopScanning.Enabled = false;
      this.btnStopScanning.Location = new Point(11, 39);
      this.btnStopScanning.Name = "btnStopScanning";
      this.btnStopScanning.Size = new Size(67, 23);
      this.btnStopScanning.TabIndex = 8;
      this.btnStopScanning.Text = "Stop";
      this.btnStopScanning.UseVisualStyleBackColor = true;
      this.btnStopScanning.Click += new System.EventHandler(this.btnStopScanning_Click);
      this.btnStartScanning.Location = new Point(11, 13);
      this.btnStartScanning.Name = "btnStartScanning";
      this.btnStartScanning.Size = new Size(67, 23);
      this.btnStartScanning.TabIndex = 1;
      this.btnStartScanning.Text = "Start";
      this.btnStartScanning.UseVisualStyleBackColor = true;
      this.btnStartScanning.Click += new System.EventHandler(this.btnStartScanning_Click);
      this.txtBusMode.Location = new Point(77, 16);
      this.txtBusMode.Name = "txtBusMode";
      this.txtBusMode.ReadOnly = true;
      this.txtBusMode.Size = new Size(94, 20);
      this.txtBusMode.TabIndex = 9;
      this.txtBusMode.TabStop = false;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(5, 43);
      this.label3.Name = "label3";
      this.label3.Size = new Size(66, 13);
      this.label3.TabIndex = 7;
      this.label3.Text = "Transceiver:";
      this.txtTransceiver.Location = new Point(77, 42);
      this.txtTransceiver.Name = "txtTransceiver";
      this.txtTransceiver.ReadOnly = true;
      this.txtTransceiver.Size = new Size(94, 20);
      this.txtTransceiver.TabIndex = 6;
      this.txtTransceiver.TabStop = false;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(5, 18);
      this.label1.Name = "label1";
      this.label1.Size = new Size(57, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Bus mode:";
      this.statusStrip1.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.txtStatus
      });
      this.statusStrip1.Location = new Point(0, 527);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new Size(887, 22);
      this.statusStrip1.TabIndex = 3;
      this.statusStrip1.Text = "statusStrip1";
      this.txtStatus.Name = "txtStatus";
      this.txtStatus.Size = new Size(0, 17);
      this.timer.Tick += new System.EventHandler(this.timer_Tick);
      this.groupBox1.Controls.Add((Control) this.txtTransceiver);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.txtBusMode);
      this.groupBox1.Controls.Add((Control) this.ckbTimeInfo);
      this.groupBox1.Location = new Point(85, 41);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(184, 94);
      this.groupBox1.TabIndex = 22;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Info";
      this.zennerCoroprateDesign1.Dock = DockStyle.Fill;
      this.zennerCoroprateDesign1.Location = new Point(0, 0);
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      this.zennerCoroprateDesign1.Size = new Size(887, 549);
      this.zennerCoroprateDesign1.TabIndex = 1;
      this.txtFilterBySerialNumber.Location = new Point(86, 32);
      this.txtFilterBySerialNumber.Name = "txtFilterBySerialNumber";
      this.txtFilterBySerialNumber.Size = new Size(120, 20);
      this.txtFilterBySerialNumber.TabIndex = 11;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(9, 35);
      this.label2.Name = "label2";
      this.label2.Size = new Size(71, 13);
      this.label2.TabIndex = 10;
      this.label2.Text = "Serialnumber:";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(7, 47);
      this.label7.Name = "label7";
      this.label7.Size = new Size(62, 13);
      this.label7.TabIndex = 19;
      this.label7.Text = "Transmitter:";
      this.txtbxReceiverQuarzError.Location = new Point(72, 16);
      this.txtbxReceiverQuarzError.Name = "txtbxReceiverQuarzError";
      this.txtbxReceiverQuarzError.Size = new Size(49, 20);
      this.txtbxReceiverQuarzError.TabIndex = 20;
      this.txtbxReceiverQuarzError.Text = "0";
      this.txtbxReceiverQuarzError.TextAlign = HorizontalAlignment.Right;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(7, 19);
      this.label6.Name = "label6";
      this.label6.Size = new Size(53, 13);
      this.label6.TabIndex = 18;
      this.label6.Text = "Receiver:";
      this.txtbxTransmitterQuarzError.Location = new Point(72, 43);
      this.txtbxTransmitterQuarzError.Name = "txtbxTransmitterQuarzError";
      this.txtbxTransmitterQuarzError.Size = new Size(49, 20);
      this.txtbxTransmitterQuarzError.TabIndex = 21;
      this.txtbxTransmitterQuarzError.Text = "0";
      this.txtbxTransmitterQuarzError.TextAlign = HorizontalAlignment.Right;
      this.label8.AutoSize = true;
      this.label8.Location = new Point((int) sbyte.MaxValue, 19);
      this.label8.Name = "label8";
      this.label8.Size = new Size(27, 13);
      this.label8.TabIndex = 22;
      this.label8.Text = "ppm";
      this.label9.AutoSize = true;
      this.label9.Location = new Point((int) sbyte.MaxValue, 46);
      this.label9.Name = "label9";
      this.label9.Size = new Size(27, 13);
      this.label9.TabIndex = 23;
      this.label9.Text = "ppm";
      this.groupBox3.Controls.Add((Control) this.label9);
      this.groupBox3.Controls.Add((Control) this.label8);
      this.groupBox3.Controls.Add((Control) this.txtbxTransmitterQuarzError);
      this.groupBox3.Controls.Add((Control) this.label6);
      this.groupBox3.Controls.Add((Control) this.txtbxReceiverQuarzError);
      this.groupBox3.Controls.Add((Control) this.label7);
      this.groupBox3.Location = new Point(212, 16);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new Size(189, 70);
      this.groupBox3.TabIndex = 23;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Quarz offset error";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(9, 63);
      this.label4.Name = "label4";
      this.label4.Size = new Size(45, 13);
      this.label4.TabIndex = 15;
      this.label4.Text = "Interval:";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(887, 549);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.statusStrip1);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (RadioScannerForm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Radio Scanner";
      this.FormClosing += new FormClosingEventHandler(this.RadioScannerForm_FormClosing);
      this.FormClosed += new FormClosedEventHandler(this.RadioScannerForm_FormClosed);
      this.Load += new System.EventHandler(this.RadioScannerForm_Load);
      this.panel1.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.txtInterval.EndInit();
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
