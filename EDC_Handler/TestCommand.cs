// Decompiled with JetBrains decompiler
// Type: EDC_Handler.TestCommand
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using AsyncCom;
using DeviceCollector;
using HandlerLib;
using StartupLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace EDC_Handler
{
  public class TestCommand : Form
  {
    private EDC_HandlerFunctions edcHandler;
    private bool isCanceled;
    private Stopwatch stopwatch;
    private GMMSettings settingsForSecondMinoConnect;
    private static DeviceCollectorFunctions secondDeviceCollector;
    private IContainer components = (IContainer) null;
    private TextBox txtOutput;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel lblStatus;
    private ToolStripProgressBar progress;
    private ToolStripStatusLabel lblProgress;
    private ToolStripStatusLabel lblPerformance;
    private GroupBox groupBox2;
    private TabPage tabGenerally;
    private Button btnRadioNormal;
    private Button btnRadioPN9;
    private Button btnResetDevice;
    private Button btnRadioDisable;
    private Button btnRadioOOK;
    private Button btnReadVersion;
    private Button btnPulseDisable;
    private Button btnPulseEnable;
    private Button btnSendAck;
    private Button btnRunRamBackup;
    private TabControl tabs;
    private GroupBox groupBox3;
    private Button btnReadSystemTime;
    private Button btnWriteSystemTime;
    private DateTimePicker txtSystemTime;
    private TabPage tabMinoConnect;
    private Button btnReceiveRadioPacket;
    private NumericUpDown txtSerialnumber;
    private Label label6;
    private Label label8;
    private ComboBox cboxRadioMode;
    private GroupBox groupBox5;
    private Label label9;
    private NumericUpDown txtTimeout;
    private Label label7;
    private GroupBox groupBox7;
    private Label label34;
    private NumericUpDown txtRadioTimeout;
    private Label label17;
    private NumericUpDown txtRadioFrequencyOffset;
    private Label label10;
    private ComboBox cboxRadioModeForTest;
    private Button btnStartDepassivation;
    private Button btnRadioReceive;
    private GroupBox groupBox6;
    private Label label11;
    private NumericUpDown txtTimeoutRadioReceive;
    private GroupBox groupBox9;
    private Button btnStopSendMinoConnectTestPacket;
    private Label label13;
    private ComboBox cboxRadioModeForMiConTestPacket;
    private Button btnStartSendMinoConnectTestPacket;
    private NumericUpDown txtSendTestPacketPower;
    private Label label12;
    private Label label14;
    private ComboBox cboxPortNames;
    private Button btnCloseSecondaryComPort;
    private Button btnOpenSecondaryComPort;
    private Button btnStartRadioReceiver;
    private Button btnSND_NKE_IrDaOff;
    private TabPage tabMemory;
    private Button btnEventLogClear;
    private GroupBox groupBox4;
    private TextBox txtWriteMemoryBuffer;
    private TextBox txtStartAddressByWrite;
    private Label label15;
    private Button btnWriteMemory;
    private GroupBox groupBox1;
    private Label label5;
    private Label label4;
    private Label label3;
    private TextBox txtByteSize;
    private Label label2;
    private TextBox txtEndAddress;
    private TextBox txtStartAddress;
    private Label label1;
    private Label lblStartAddress;
    private Button btnReadMemory;
    private Button btnSystemLogClear;
    private Button btnRemovalFlagClear;
    private Button btnTamperFlagClear;
    private Button btnBackflowFlagClear;
    private Button btnLeakFlagClear;
    private Button btnBlockFlagClear;
    private Button btnOversizeFlagClear;
    private Button btnUndersizeFlagClear;
    private Button btnBurstFlagClear;
    private Button btnLogClearAndDisableLog;
    private GroupBox groupBox10;
    private Label label16;
    private NumericUpDown txtMeterValue;
    private Button btnWriteMeterValue;
    private Button btnReadMeterValue;
    private Button btnSetCurrentTime;
    private Button btnLogEnable;
    private Button btnLogDisable;
    private TabPage tabWiredMbus;
    private CheckBox ckboxClearQueue;
    private Label label18;
    private Button btnWritePulseoutQueue;
    private NumericUpDown txtPulseoutQueue;
    private GroupBox groupBox8;
    private Button btnReadManufacturer;
    private Label label23;
    private Button btnWriteManufacturer;
    private Button btnReadMedium;
    private Label label22;
    private Button btnWriteMedium;
    private Button btnReadGeneration;
    private Label label21;
    private NumericUpDown txtGeneration;
    private Button btnWriteGeneration;
    private Button btnReadAddress;
    private Label label20;
    private NumericUpDown txtAddress;
    private Button btnWriteAddress;
    private RadioButton rbtnPrimary;
    private RadioButton rbtnSecondary;
    private Button btnReadSerial;
    private Label label19;
    private NumericUpDown txtSerial;
    private Button btnWriteSerial;
    private ComboBox cboxMBusMedium;
    private TextBox txtManufacturer;
    private Button btnReadObis;
    private Label label24;
    private Button btnWriteObis;
    private TextBox txtObis;
    private Button buttonHardwareTypeManager;

    public TestCommand()
    {
      this.InitializeComponent();
      List<string> stringList = new List<string>((IEnumerable<string>) Util.GetNamesOfEnum(typeof (MBusDeviceType)));
      stringList.Insert(0, string.Empty);
      this.cboxMBusMedium.DataSource = (object) stringList;
      this.stopwatch = new Stopwatch();
      this.isCanceled = false;
      this.ResetUI();
    }

    public TestCommand(EDC_HandlerFunctions MyFunctions)
      : this()
    {
      this.edcHandler = MyFunctions;
    }

    internal static void ShowDialog(Form owner, EDC_HandlerFunctions edcHandler)
    {
      if (edcHandler == null)
        return;
      using (TestCommand testCommand = new TestCommand())
      {
        testCommand.edcHandler = edcHandler;
        int num = (int) testCommand.ShowDialog((IWin32Window) owner);
      }
    }

    private void TestCommand_Load(object sender, EventArgs e)
    {
      this.edcHandler.MyDeviceCollector.OnMessage += new EventHandler<GMM_EventArgs>(this.MyDeviceCollector_OnMessage);
      string[] namesOfEnum = Util.GetNamesOfEnum(typeof (RadioMode));
      this.cboxRadioMode.DataSource = (object) namesOfEnum;
      this.cboxRadioModeForTest.DataSource = (object) namesOfEnum;
      this.cboxRadioModeForMiConTestPacket.DataSource = (object) namesOfEnum;
      this.txtTimeout.Value = 180M;
      if (!UserManager.CheckPermission("EDC_Handler.View.TestCommandos"))
        this.Close();
      if (UserManager.CheckPermission("EDC_Handler.View.Expert"))
        return;
      this.tabs.TabPages.Remove(this.tabMemory);
      this.tabs.TabPages.Remove(this.tabMinoConnect);
    }

    private void TestCommand_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.edcHandler.MyDeviceCollector.OnMessage -= new EventHandler<GMM_EventArgs>(this.MyDeviceCollector_OnMessage);
    }

    private void MyDeviceCollector_OnMessage(object sender, GMM_EventArgs e)
    {
      if (this.InvokeRequired)
      {
        try
        {
          this.BeginInvoke((Delegate) new EventHandler<GMM_EventArgs>(this.MyDeviceCollector_OnMessage), sender, (object) e);
        }
        catch
        {
        }
      }
      else
      {
        e.Cancel = this.isCanceled;
        if (e.Cancel)
        {
          this.edcHandler.MyDeviceCollector.BreakRequest = true;
        }
        else
        {
          this.progress.Visible = e.ProgressPercentage > 0;
          this.lblProgress.Visible = this.progress.Visible;
          if (this.progress.Visible)
          {
            this.progress.Value = e.ProgressPercentage;
            this.lblProgress.Text = string.Format("{0}%", (object) e.ProgressPercentage);
          }
          if (string.IsNullOrEmpty(e.EventMessage) || e.TheMessageType == GMM_EventArgs.MessageType.Alive)
            return;
          this.lblStatus.Text = e.EventMessage;
        }
      }
    }

    private void tabs_Selected(object sender, TabControlEventArgs e)
    {
      this.txtOutput.Text = string.Empty;
      if (e.TabPage != this.tabMinoConnect || this.cboxPortNames.Items.Count != 0)
        return;
      string[] portNames = SerialPort.GetPortNames();
      if (portNames != null)
      {
        foreach (object obj in portNames)
          this.cboxPortNames.Items.Add(obj);
      }
      if (this.cboxPortNames.Items.Count > 0)
        this.cboxPortNames.SelectedIndex = 0;
      if (TestCommand.secondDeviceCollector == null)
      {
        this.settingsForSecondMinoConnect = GMMSettings.Default_Radio2_MinoConnect;
        TestCommand.secondDeviceCollector = new DeviceCollectorFunctions((IAsyncFunctions) new AsyncFunctions(true), false);
        TestCommand.secondDeviceCollector.SetDeviceCollectorSettings(this.settingsForSecondMinoConnect.DeviceCollectorSettings);
        TestCommand.secondDeviceCollector.SetAsyncComSettings(this.settingsForSecondMinoConnect.AsyncComSettings);
      }
      else
      {
        string asyncComSettings = TestCommand.secondDeviceCollector.GetAsyncComSettings(AsyncComSettings.Port);
        if (!string.IsNullOrEmpty(asyncComSettings) && this.cboxPortNames.Items.Count > 0)
          this.cboxPortNames.Text = asyncComSettings;
      }
      EDC_Meter meter = this.edcHandler.Meter;
      if (meter != null && meter.Version.Type == EDC_Hardware.EDC_Radio)
      {
        ushort? transmitInterval = meter.GetRadioTransmitInterval();
        this.txtTimeout.Value = (Decimal) (transmitInterval.HasValue ? (int) transmitInterval.Value : 180);
        RadioMode? radioMode = meter.GetRadioMode();
        this.cboxRadioMode.SelectedItem = radioMode.HasValue ? (object) radioMode.Value.ToString() : (object) RadioMode.Radio2.ToString();
        this.cboxRadioModeForTest.SelectedItem = radioMode.HasValue ? (object) radioMode.Value.ToString() : (object) RadioMode.Radio2.ToString();
        this.cboxRadioModeForMiConTestPacket.SelectedItem = radioMode.HasValue ? (object) radioMode.Value.ToString() : (object) RadioMode.Radio2.ToString();
        short? frequencyOffset = meter.GetFrequencyOffset();
        this.txtRadioFrequencyOffset.Value = (Decimal) (frequencyOffset.HasValue ? (int) frequencyOffset.Value : 0);
        uint? nullable1 = new uint?();
        uint? nullable2 = !radioMode.HasValue || radioMode.Value != RadioMode.Radio2 && radioMode.Value != RadioMode.Radio3 ? meter.GetSerialnumberPrimary() : meter.GetSerialnumberRadioMinol();
        this.txtSerialnumber.Value = (Decimal) (nullable2.HasValue ? nullable2.Value : 0U);
      }
    }

    private void txtStartEnd_TextChanged(object sender, EventArgs e)
    {
      try
      {
        ushort uint16 = Convert.ToUInt16(this.txtStartAddress.Text.Replace("0x", string.Empty), 16);
        this.txtByteSize.Text = ((int) Convert.ToUInt16(this.txtEndAddress.Text.Replace("0x", string.Empty), 16) - (int) uint16).ToString();
      }
      catch
      {
      }
    }

    private void txtByteSize_TextChanged(object sender, EventArgs e)
    {
      try
      {
        ushort uint16 = Convert.ToUInt16(this.txtStartAddress.Text.Replace("0x", string.Empty), 16);
        ushort result;
        if (!ushort.TryParse(this.txtByteSize.Text, out result))
          return;
        this.txtEndAddress.Text = ((int) uint16 + (int) result).ToString("X4");
      }
      catch
      {
      }
    }

    private void btnOpenSecondaryComPort_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.cboxPortNames.Text))
        return;
      this.settingsForSecondMinoConnect.SetAsyncComSettings(AsyncComSettings.Port, this.cboxPortNames.Text);
      TestCommand.secondDeviceCollector.SetAsyncComSettings(this.settingsForSecondMinoConnect.AsyncComSettings);
      TestCommand.secondDeviceCollector.ComOpen();
    }

    private void btnCloseSecondaryComPort_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.cboxPortNames.Text))
        return;
      this.settingsForSecondMinoConnect.SetAsyncComSettings(AsyncComSettings.Port, this.cboxPortNames.Text);
      TestCommand.secondDeviceCollector.SetAsyncComSettings(this.settingsForSecondMinoConnect.AsyncComSettings);
      TestCommand.secondDeviceCollector.ComClose();
    }

    private void btnSetCurrentTime_Click(object sender, EventArgs e)
    {
      this.txtSystemTime.Value = DateTime.Now;
    }

    private void btnResetDevice_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.edcHandler.ResetDevice));
    }

    private void btnEventLogClear_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.edcHandler.EventLogClear));
    }

    private void btnSystemLogClear_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.edcHandler.SystemLogClear));
    }

    private void btnRemovalFlagClear_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.edcHandler.RemovalFlagClear));
    }

    private void btnTamperFlagClear_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.edcHandler.TamperFlagClear));
    }

    private void btnBackflowFlagClear_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.edcHandler.BackflowFlagClear));
    }

    private void btnLeakFlagClear_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.edcHandler.LeakFlagClear));
    }

    private void btnBlockFlagClear_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.edcHandler.BlockFlagClear));
    }

    private void btnOversizeFlagClear_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.edcHandler.OversizeFlagClear));
    }

    private void btnUndersizeFlagClear_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.edcHandler.UndersizeFlagClear));
    }

    private void btnBurstFlagClear_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.edcHandler.BurstFlagClear));
    }

    private void btnLogClearAndDisableLog_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.edcHandler.LogClearAndDisableLog));
    }

    private void btnRunRamBackup_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.edcHandler.RunRAMBackup));
    }

    private void btnPulseDisable_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.edcHandler.PulseDisable));
    }

    private void btnPulseEnable_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.edcHandler.PulseEnable));
    }

    private void btnRadioDisable_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.edcHandler.RadioDisable));
    }

    private void btnRadioNormal_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.edcHandler.RadioNormal));
    }

    private void btnStartDepassivation_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.edcHandler.StartDepassivation));
    }

    private void btnSendAck_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.edcHandler.MyDeviceCollector.EDCHandler.StopVolumeMonitor_SendE5));
    }

    private void btnSND_NKE_IrDaOff_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.edcHandler.SendSND_NKE));
    }

    private void btnLogEnable_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.edcHandler.LogEnable));
    }

    private void btnLogDisable_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.edcHandler.LogDisable));
    }

    private void btnReadVersion_Click(object sender, EventArgs e)
    {
      this.ResetUI();
      DeviceVersion deviceVersion = this.edcHandler.ReadVersion();
      if (deviceVersion != null)
      {
        this.stopwatch.Stop();
        this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
        this.txtOutput.Text = deviceVersion.ToString(20);
      }
      else
        this.txtOutput.Text = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
      this.lblStatus.Text = string.Empty;
      this.lblProgress.Text = string.Empty;
    }

    private void btnReadMemory_Click(object sender, EventArgs e)
    {
      try
      {
        ushort uint16_1 = Convert.ToUInt16(this.txtStartAddress.Text.Replace("0x", string.Empty), 16);
        ushort uint16_2 = Convert.ToUInt16(this.txtEndAddress.Text.Replace("0x", string.Empty), 16);
        this.ResetUI();
        byte[] buffer;
        if (this.edcHandler.ReadMemory(uint16_1, (int) uint16_2 - (int) uint16_1, out buffer))
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = Util.ByteArrayToHexString(buffer);
        }
        else
          this.txtOutput.Text = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        this.lblStatus.Text = string.Empty;
        this.lblProgress.Text = string.Empty;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void btnWriteMemory_Click(object sender, EventArgs e)
    {
      try
      {
        ushort uint16 = Convert.ToUInt16(this.txtStartAddressByWrite.Text.Replace("0x", string.Empty), 16);
        byte[] byteArray = Util.HexStringToByteArray(this.txtWriteMemoryBuffer.Text);
        this.ResetUI();
        if (this.edcHandler.WriteMemory(uint16, byteArray))
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
        }
        else
          this.txtOutput.Text = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        this.lblStatus.Text = string.Empty;
        this.lblProgress.Text = string.Empty;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void btnStartRadioReceiver_Click(object sender, EventArgs e)
    {
      this.ResetUI();
      try
      {
        if (this.edcHandler.StartRadioReceiver())
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          Thread.Sleep(1000);
          byte[] buffer;
          if (this.edcHandler.MyDeviceCollector.AsyncCom.TryReceiveBlock(out buffer) && buffer != null)
            this.txtOutput.Text = Util.ByteArrayToHexString(buffer);
          else
            this.txtOutput.Text = "OK";
        }
        else
          this.txtOutput.Text = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
      }
      catch (Exception ex)
      {
        this.txtOutput.Text = ex.Message;
      }
      this.lblStatus.Text = string.Empty;
      this.lblProgress.Text = string.Empty;
    }

    private void btnRadioOOK_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        if (this.edcHandler.RadioOOK((RadioMode) Enum.Parse(typeof (RadioMode), this.cboxRadioModeForTest.SelectedItem.ToString(), true), Convert.ToInt16(this.txtRadioFrequencyOffset.Value), Convert.ToUInt16(this.txtRadioTimeout.Value)))
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
        }
        else
          this.txtOutput.Text = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        this.lblStatus.Text = string.Empty;
        this.lblProgress.Text = string.Empty;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnRadioPN9_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        if (this.edcHandler.RadioPN9((RadioMode) Enum.Parse(typeof (RadioMode), this.cboxRadioModeForTest.SelectedItem.ToString(), true), Convert.ToInt16(this.txtRadioFrequencyOffset.Value), Convert.ToUInt16(this.txtRadioTimeout.Value)))
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
        }
        else
          this.txtOutput.Text = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        this.lblStatus.Text = string.Empty;
        this.lblProgress.Text = string.Empty;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnRadioReceive_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        RadioPacket packet;
        byte[] buffer;
        int rssi_dBm;
        int lqi;
        if (this.edcHandler.RadioReceive(out packet, out buffer, out rssi_dBm, out lqi, (uint) this.txtTimeoutRadioReceive.Value))
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "RSSI: " + rssi_dBm.ToString() + Environment.NewLine;
          TextBox txtOutput1 = this.txtOutput;
          txtOutput1.Text = txtOutput1.Text + "LQI: 0x" + lqi.ToString("X2") + Environment.NewLine;
          TextBox txtOutput2 = this.txtOutput;
          txtOutput2.Text = txtOutput2.Text + "Buffer: " + Util.ByteArrayToHexString(buffer) + Environment.NewLine;
          if (packet != null)
          {
            TextBox txtOutput3 = this.txtOutput;
            txtOutput3.Text = txtOutput3.Text + "Packet type: " + packet.GetType()?.ToString() + Environment.NewLine;
            TextBox txtOutput4 = this.txtOutput;
            txtOutput4.Text = txtOutput4.Text + "FunkId: " + packet.FunkId.ToString() + Environment.NewLine;
            TextBox txtOutput5 = this.txtOutput;
            txtOutput5.Text = txtOutput5.Text + Environment.NewLine + "ATTENTION: The radio receiver is deactivated now!!! To activate receiver please press the 'Radio normal' button.";
          }
        }
        else
        {
          this.txtOutput.Text = "Failed to receive radio packet!" + Environment.NewLine;
          TextBox txtOutput = this.txtOutput;
          txtOutput.Text = txtOutput.Text + "Reason: " + ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        }
        this.lblStatus.Text = string.Empty;
        this.lblProgress.Text = string.Empty;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnReadSystemTime_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        DateTime? nullable = this.edcHandler.ReadSystemTime();
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtSystemTime.Value = nullable.Value;
        }
        else
          this.txtOutput.Text = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        this.lblStatus.Text = string.Empty;
        this.lblProgress.Text = string.Empty;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnWriteSystemTime_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        if (this.edcHandler.WriteSystemTime(this.txtSystemTime.Value))
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
        }
        else
          this.txtOutput.Text = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        this.lblStatus.Text = string.Empty;
        this.lblProgress.Text = string.Empty;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnReceiveRadioPacket_Click(object sender, EventArgs e)
    {
      this.settingsForSecondMinoConnect.SetAsyncComSettings(AsyncComSettings.Port, this.cboxPortNames.Text);
      TestCommand.secondDeviceCollector.SetAsyncComSettings(this.settingsForSecondMinoConnect.AsyncComSettings);
      Cursor.Current = Cursors.WaitCursor;
      Application.DoEvents();
      try
      {
        this.ResetUI();
        SortedList<DeviceCollectorSettings, object> collectorSettings = TestCommand.secondDeviceCollector.GetDeviceCollectorSettings();
        RadioMode radioMode = (RadioMode) Enum.Parse(typeof (RadioMode), this.cboxRadioMode.Text, true);
        switch (radioMode)
        {
          case RadioMode.Radio2:
            collectorSettings[DeviceCollectorSettings.BusMode] = (object) BusMode.Radio2;
            break;
          case RadioMode.Radio3:
            collectorSettings[DeviceCollectorSettings.BusMode] = (object) BusMode.Radio3;
            break;
          case RadioMode.wMBusS1:
            collectorSettings[DeviceCollectorSettings.BusMode] = (object) BusMode.wMBusS1;
            break;
          case RadioMode.wMBusS1M:
            collectorSettings[DeviceCollectorSettings.BusMode] = (object) BusMode.wMBusS1M;
            break;
          case RadioMode.wMBusS2:
            collectorSettings[DeviceCollectorSettings.BusMode] = (object) BusMode.wMBusS2;
            break;
          case RadioMode.wMBusT1:
            collectorSettings[DeviceCollectorSettings.BusMode] = (object) BusMode.wMBusT1;
            break;
          case RadioMode.wMBusT2_meter:
            collectorSettings[DeviceCollectorSettings.BusMode] = (object) BusMode.wMBusT2_meter;
            break;
          case RadioMode.wMBusT2_other:
            collectorSettings[DeviceCollectorSettings.BusMode] = (object) BusMode.wMBusT2_other;
            break;
          case RadioMode.wMBusC1A:
            collectorSettings[DeviceCollectorSettings.BusMode] = (object) BusMode.wMBusC1A;
            break;
          case RadioMode.wMBusC1B:
            collectorSettings[DeviceCollectorSettings.BusMode] = (object) BusMode.wMBusC1B;
            break;
          default:
            int num = (int) MessageBox.Show("The radio mode " + radioMode.ToString() + " is not implemented! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
        }
        int funkId = (int) this.txtSerialnumber.Value;
        int timeout = (int) this.txtTimeout.Value;
        TestCommand.secondDeviceCollector.SetDeviceCollectorSettings(collectorSettings);
        if (!TestCommand.secondDeviceCollector.ComOpen())
        {
          string errorDescription = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
          if (string.IsNullOrEmpty(errorDescription))
            this.txtOutput.Text = "COM open error!";
          else
            this.txtOutput.Text = "COM open error! Error: " + errorDescription;
        }
        else
        {
          RadioPacket onePacket = TestCommand.secondDeviceCollector.RadioReader.ReceiveOnePacket((long) funkId, timeout);
          if (onePacket != null)
          {
            this.stopwatch.Stop();
            this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
            this.txtOutput.Text = onePacket.ToString();
          }
          else
          {
            string errorDescription = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
            if (string.IsNullOrEmpty(errorDescription))
              this.txtOutput.Text = "Faled! Timeout";
            else
              this.txtOutput.Text = "Faled! Error: " + errorDescription;
          }
          this.lblStatus.Text = string.Empty;
          this.lblProgress.Text = string.Empty;
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void btnStartSendMinoConnectTestPacket_Click(object sender, EventArgs e)
    {
      this.settingsForSecondMinoConnect.SetAsyncComSettings(AsyncComSettings.Port, this.cboxPortNames.Text);
      TestCommand.secondDeviceCollector.SetAsyncComSettings(this.settingsForSecondMinoConnect.AsyncComSettings);
      if (!TestCommand.secondDeviceCollector.ComOpen())
      {
        string errorDescription = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        if (string.IsNullOrEmpty(errorDescription))
          this.txtOutput.Text = "COM open error!";
        else
          this.txtOutput.Text = "COM open error! Error: " + errorDescription;
      }
      else
      {
        Cursor.Current = Cursors.WaitCursor;
        Application.DoEvents();
        try
        {
          this.ResetUI();
          RadioMode radioMode = (RadioMode) Enum.Parse(typeof (RadioMode), this.cboxRadioModeForMiConTestPacket.Text, true);
          byte num = Convert.ToByte(this.txtSendTestPacketPower.Value);
          if (TestCommand.secondDeviceCollector.AsyncCom.CallTransceiverFunction(TransceiverDeviceFunction.StartSendTestPacket, (object) radioMode, (object) num))
          {
            this.stopwatch.Stop();
            this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
            this.txtOutput.Text = "OK";
          }
          else
          {
            string errorDescription = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
            if (string.IsNullOrEmpty(errorDescription))
              this.txtOutput.Text = "Faled! Timeout";
            else
              this.txtOutput.Text = "Faled! Error: " + errorDescription;
          }
          this.lblStatus.Text = string.Empty;
          this.lblProgress.Text = string.Empty;
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        finally
        {
          Cursor.Current = Cursors.Default;
        }
      }
    }

    private void btnStopSendMinoConnectTestPacket_Click(object sender, EventArgs e)
    {
      this.settingsForSecondMinoConnect.SetAsyncComSettings(AsyncComSettings.Port, this.cboxPortNames.Text);
      TestCommand.secondDeviceCollector.SetAsyncComSettings(this.settingsForSecondMinoConnect.AsyncComSettings);
      if (!TestCommand.secondDeviceCollector.ComOpen())
      {
        string errorDescription = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        if (string.IsNullOrEmpty(errorDescription))
          this.txtOutput.Text = "COM open error!";
        else
          this.txtOutput.Text = "COM open error! Error: " + errorDescription;
      }
      else
      {
        Cursor.Current = Cursors.WaitCursor;
        Application.DoEvents();
        try
        {
          this.ResetUI();
          if (TestCommand.secondDeviceCollector.AsyncCom.CallTransceiverFunction(TransceiverDeviceFunction.StopSendTestPacket))
          {
            this.stopwatch.Stop();
            this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
            this.txtOutput.Text = "OK";
          }
          else
          {
            string errorDescription = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
            if (string.IsNullOrEmpty(errorDescription))
              this.txtOutput.Text = "Faled! Timeout";
            else
              this.txtOutput.Text = "Faled! Error: " + errorDescription;
          }
          this.lblStatus.Text = string.Empty;
          this.lblProgress.Text = string.Empty;
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        finally
        {
          TestCommand.secondDeviceCollector.ComClose();
          Cursor.Current = Cursors.Default;
        }
      }
    }

    private void btnWriteMeterValue_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        if (this.edcHandler.WriteMeterValue(Convert.ToUInt32(this.txtMeterValue.Value)))
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
        }
        else
          this.txtOutput.Text = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        this.lblStatus.Text = string.Empty;
        this.lblProgress.Text = string.Empty;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnReadMeterValue_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        uint? nullable = this.edcHandler.ReadMeterValue();
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtMeterValue.Value = (Decimal) nullable.Value;
        }
        else
          this.txtOutput.Text = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        this.lblStatus.Text = string.Empty;
        this.lblProgress.Text = string.Empty;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnWritePulseoutQueue_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        if (this.edcHandler.WritePulseoutQueue(Convert.ToInt16(this.txtPulseoutQueue.Value), this.ckboxClearQueue.Checked))
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
        }
        else
          this.txtOutput.Text = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        this.lblStatus.Text = string.Empty;
        this.lblProgress.Text = string.Empty;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnReadSerial_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        uint? nullable = this.edcHandler.ReadSerialnumber(this.rbtnPrimary.Checked ? ID.Primary : ID.Secondary);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtSerial.Value = (Decimal) nullable.Value;
        }
        else
          this.txtOutput.Text = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        this.lblStatus.Text = string.Empty;
        this.lblProgress.Text = string.Empty;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnWriteSerial_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        if (this.edcHandler.WriteSerialnumber(this.rbtnPrimary.Checked ? ID.Primary : ID.Secondary, Convert.ToUInt32(this.txtSerial.Value)))
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
        }
        else
          this.txtOutput.Text = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        this.lblStatus.Text = string.Empty;
        this.lblProgress.Text = string.Empty;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnReadAddress_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        byte? nullable = this.edcHandler.ReadAddress(this.rbtnPrimary.Checked ? ID.Primary : ID.Secondary);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtAddress.Value = (Decimal) nullable.Value;
        }
        else
          this.txtOutput.Text = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        this.lblStatus.Text = string.Empty;
        this.lblProgress.Text = string.Empty;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnWriteAddress_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        if (this.edcHandler.WriteAddress(this.rbtnPrimary.Checked ? ID.Primary : ID.Secondary, Convert.ToByte(this.txtAddress.Value)))
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
        }
        else
          this.txtOutput.Text = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        this.lblStatus.Text = string.Empty;
        this.lblProgress.Text = string.Empty;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnReadGeneration_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        byte? nullable = this.edcHandler.ReadGeneration(this.rbtnPrimary.Checked ? ID.Primary : ID.Secondary);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtGeneration.Value = (Decimal) nullable.Value;
        }
        else
          this.txtOutput.Text = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        this.lblStatus.Text = string.Empty;
        this.lblProgress.Text = string.Empty;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnWriteGeneration_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        if (this.edcHandler.WriteGeneration(this.rbtnPrimary.Checked ? ID.Primary : ID.Secondary, Convert.ToByte(this.txtGeneration.Value)))
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
        }
        else
          this.txtOutput.Text = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        this.lblStatus.Text = string.Empty;
        this.lblProgress.Text = string.Empty;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnReadMedium_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        MBusDeviceType? nullable = this.edcHandler.ReadMedium(this.rbtnPrimary.Checked ? ID.Primary : ID.Secondary);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.cboxMBusMedium.SelectedItem = (object) nullable.Value.ToString();
        }
        else
          this.txtOutput.Text = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        this.lblStatus.Text = string.Empty;
        this.lblProgress.Text = string.Empty;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnWriteMedium_Click(object sender, EventArgs e)
    {
      if (this.cboxMBusMedium.SelectedItem == null || string.IsNullOrEmpty(this.cboxMBusMedium.SelectedItem.ToString()))
        return;
      try
      {
        this.ResetUI();
        if (this.edcHandler.WriteMedium(this.rbtnPrimary.Checked ? ID.Primary : ID.Secondary, (MBusDeviceType) Enum.Parse(typeof (MBusDeviceType), this.cboxMBusMedium.SelectedItem.ToString())))
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
        }
        else
          this.txtOutput.Text = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        this.lblStatus.Text = string.Empty;
        this.lblProgress.Text = string.Empty;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnReadManufacturer_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        string str = this.edcHandler.ReadManufacturer(this.rbtnPrimary.Checked ? ID.Primary : ID.Secondary);
        if (!string.IsNullOrEmpty(str))
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtManufacturer.Text = str;
        }
        else
          this.txtOutput.Text = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        this.lblStatus.Text = string.Empty;
        this.lblProgress.Text = string.Empty;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnWriteManufacturer_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        if (this.edcHandler.WriteManufacturer(this.rbtnPrimary.Checked ? ID.Primary : ID.Secondary, this.txtManufacturer.Text))
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
        }
        else
          this.txtOutput.Text = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        this.lblStatus.Text = string.Empty;
        this.lblProgress.Text = string.Empty;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnReadObis_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        byte? nullable = this.edcHandler.ReadObis(this.rbtnPrimary.Checked ? ID.Primary : ID.Secondary);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtObis.Text = nullable.Value.ToString("X2");
        }
        else
          this.txtOutput.Text = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        this.lblStatus.Text = string.Empty;
        this.lblProgress.Text = string.Empty;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnWriteObis_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        if (this.edcHandler.WriteObis(this.rbtnPrimary.Checked ? ID.Primary : ID.Secondary, Convert.ToByte(this.txtObis.Text, 16)))
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
        }
        else
          this.txtOutput.Text = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        this.lblStatus.Text = string.Empty;
        this.lblProgress.Text = string.Empty;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void ResetUI()
    {
      this.isCanceled = false;
      this.lblProgress.Text = string.Empty;
      this.lblStatus.Text = string.Empty;
      this.lblPerformance.Text = string.Empty;
      this.txtOutput.Text = string.Empty;
      this.stopwatch.Reset();
      this.stopwatch.Start();
    }

    private void ExecuteSimpleMethod(TestCommand.ActionSimpleMethod act)
    {
      this.ResetUI();
      string str = string.Format("{0}{1}.{2}", (object) act.Method.ReturnParameter, act.Target, (object) act.Method.Name);
      try
      {
        if (act())
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
        }
        else
        {
          string errorDescription = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
          this.txtOutput.Text = "Failed to execute: " + str + Environment.NewLine + Environment.NewLine + errorDescription;
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      this.lblStatus.Text = string.Empty;
      this.lblProgress.Text = string.Empty;
    }

    private void buttonHardwareTypeManager_Click(object sender, EventArgs e)
    {
      new HardwareTypeEditor(new string[2]
      {
        "EDC_Radio",
        "EDC_mBus"
      }).ShowDialog();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (TestCommand));
      this.txtOutput = new TextBox();
      this.statusStrip = new StatusStrip();
      this.progress = new ToolStripProgressBar();
      this.lblStatus = new ToolStripStatusLabel();
      this.lblProgress = new ToolStripStatusLabel();
      this.lblPerformance = new ToolStripStatusLabel();
      this.groupBox2 = new GroupBox();
      this.tabGenerally = new TabPage();
      this.groupBox8 = new GroupBox();
      this.txtObis = new TextBox();
      this.btnReadObis = new Button();
      this.label24 = new Label();
      this.btnWriteObis = new Button();
      this.txtManufacturer = new TextBox();
      this.cboxMBusMedium = new ComboBox();
      this.btnReadManufacturer = new Button();
      this.label23 = new Label();
      this.btnWriteManufacturer = new Button();
      this.btnReadMedium = new Button();
      this.label22 = new Label();
      this.btnWriteMedium = new Button();
      this.btnReadGeneration = new Button();
      this.label21 = new Label();
      this.txtGeneration = new NumericUpDown();
      this.btnWriteGeneration = new Button();
      this.btnReadAddress = new Button();
      this.label20 = new Label();
      this.txtAddress = new NumericUpDown();
      this.btnWriteAddress = new Button();
      this.rbtnPrimary = new RadioButton();
      this.rbtnSecondary = new RadioButton();
      this.btnReadSerial = new Button();
      this.label19 = new Label();
      this.txtSerial = new NumericUpDown();
      this.btnWriteSerial = new Button();
      this.buttonHardwareTypeManager = new Button();
      this.btnLogDisable = new Button();
      this.btnLogEnable = new Button();
      this.groupBox10 = new GroupBox();
      this.btnReadMeterValue = new Button();
      this.label16 = new Label();
      this.txtMeterValue = new NumericUpDown();
      this.btnWriteMeterValue = new Button();
      this.btnLogClearAndDisableLog = new Button();
      this.btnBurstFlagClear = new Button();
      this.btnUndersizeFlagClear = new Button();
      this.btnOversizeFlagClear = new Button();
      this.btnBlockFlagClear = new Button();
      this.btnLeakFlagClear = new Button();
      this.btnBackflowFlagClear = new Button();
      this.btnTamperFlagClear = new Button();
      this.btnRemovalFlagClear = new Button();
      this.btnSystemLogClear = new Button();
      this.btnEventLogClear = new Button();
      this.btnSND_NKE_IrDaOff = new Button();
      this.btnStartRadioReceiver = new Button();
      this.btnRadioNormal = new Button();
      this.groupBox6 = new GroupBox();
      this.label11 = new Label();
      this.txtTimeoutRadioReceive = new NumericUpDown();
      this.btnRadioReceive = new Button();
      this.btnStartDepassivation = new Button();
      this.groupBox7 = new GroupBox();
      this.label34 = new Label();
      this.txtRadioTimeout = new NumericUpDown();
      this.label17 = new Label();
      this.txtRadioFrequencyOffset = new NumericUpDown();
      this.label10 = new Label();
      this.btnRadioOOK = new Button();
      this.btnRadioPN9 = new Button();
      this.cboxRadioModeForTest = new ComboBox();
      this.groupBox3 = new GroupBox();
      this.btnSetCurrentTime = new Button();
      this.btnReadSystemTime = new Button();
      this.btnWriteSystemTime = new Button();
      this.txtSystemTime = new DateTimePicker();
      this.btnResetDevice = new Button();
      this.btnRadioDisable = new Button();
      this.btnReadVersion = new Button();
      this.btnPulseDisable = new Button();
      this.btnPulseEnable = new Button();
      this.btnSendAck = new Button();
      this.btnRunRamBackup = new Button();
      this.tabs = new TabControl();
      this.tabMemory = new TabPage();
      this.groupBox4 = new GroupBox();
      this.txtWriteMemoryBuffer = new TextBox();
      this.txtStartAddressByWrite = new TextBox();
      this.label15 = new Label();
      this.btnWriteMemory = new Button();
      this.groupBox1 = new GroupBox();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.txtByteSize = new TextBox();
      this.label2 = new Label();
      this.txtEndAddress = new TextBox();
      this.txtStartAddress = new TextBox();
      this.label1 = new Label();
      this.lblStartAddress = new Label();
      this.btnReadMemory = new Button();
      this.tabMinoConnect = new TabPage();
      this.btnCloseSecondaryComPort = new Button();
      this.btnOpenSecondaryComPort = new Button();
      this.label14 = new Label();
      this.cboxPortNames = new ComboBox();
      this.groupBox9 = new GroupBox();
      this.txtSendTestPacketPower = new NumericUpDown();
      this.label12 = new Label();
      this.btnStopSendMinoConnectTestPacket = new Button();
      this.label13 = new Label();
      this.cboxRadioModeForMiConTestPacket = new ComboBox();
      this.btnStartSendMinoConnectTestPacket = new Button();
      this.groupBox5 = new GroupBox();
      this.label9 = new Label();
      this.label8 = new Label();
      this.txtTimeout = new NumericUpDown();
      this.cboxRadioMode = new ComboBox();
      this.label7 = new Label();
      this.txtSerialnumber = new NumericUpDown();
      this.btnReceiveRadioPacket = new Button();
      this.label6 = new Label();
      this.tabWiredMbus = new TabPage();
      this.ckboxClearQueue = new CheckBox();
      this.label18 = new Label();
      this.btnWritePulseoutQueue = new Button();
      this.txtPulseoutQueue = new NumericUpDown();
      this.statusStrip.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.tabGenerally.SuspendLayout();
      this.groupBox8.SuspendLayout();
      this.txtGeneration.BeginInit();
      this.txtAddress.BeginInit();
      this.txtSerial.BeginInit();
      this.groupBox10.SuspendLayout();
      this.txtMeterValue.BeginInit();
      this.groupBox6.SuspendLayout();
      this.txtTimeoutRadioReceive.BeginInit();
      this.groupBox7.SuspendLayout();
      this.txtRadioTimeout.BeginInit();
      this.txtRadioFrequencyOffset.BeginInit();
      this.groupBox3.SuspendLayout();
      this.tabs.SuspendLayout();
      this.tabMemory.SuspendLayout();
      this.groupBox4.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.tabMinoConnect.SuspendLayout();
      this.groupBox9.SuspendLayout();
      this.txtSendTestPacketPower.BeginInit();
      this.groupBox5.SuspendLayout();
      this.txtTimeout.BeginInit();
      this.txtSerialnumber.BeginInit();
      this.tabWiredMbus.SuspendLayout();
      this.txtPulseoutQueue.BeginInit();
      this.SuspendLayout();
      this.txtOutput.Dock = DockStyle.Fill;
      this.txtOutput.Font = new Font("Consolas", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtOutput.Location = new Point(3, 16);
      this.txtOutput.Multiline = true;
      this.txtOutput.Name = "txtOutput";
      this.txtOutput.ScrollBars = ScrollBars.Vertical;
      this.txtOutput.Size = new Size(763, 95);
      this.txtOutput.TabIndex = 20;
      this.statusStrip.Items.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.progress,
        (ToolStripItem) this.lblStatus,
        (ToolStripItem) this.lblProgress,
        (ToolStripItem) this.lblPerformance
      });
      this.statusStrip.Location = new Point(0, 540);
      this.statusStrip.Name = "statusStrip";
      this.statusStrip.Size = new Size(780, 22);
      this.statusStrip.TabIndex = 21;
      this.statusStrip.Text = "statusStrip1";
      this.progress.Name = "progress";
      this.progress.Size = new Size(100, 16);
      this.progress.Step = 1;
      this.progress.Visible = false;
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = new Size(46, 17);
      this.lblStatus.Text = "{status}";
      this.lblProgress.Name = "lblProgress";
      this.lblProgress.Size = new Size(60, 17);
      this.lblProgress.Text = "{progress}";
      this.lblPerformance.Name = "lblPerformance";
      this.lblPerformance.Size = new Size(83, 17);
      this.lblPerformance.Text = "{performance}";
      this.groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox2.Controls.Add((Control) this.txtOutput);
      this.groupBox2.Location = new Point(3, 423);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(769, 114);
      this.groupBox2.TabIndex = 32;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Result";
      this.tabGenerally.Controls.Add((Control) this.groupBox8);
      this.tabGenerally.Controls.Add((Control) this.buttonHardwareTypeManager);
      this.tabGenerally.Controls.Add((Control) this.btnLogDisable);
      this.tabGenerally.Controls.Add((Control) this.btnLogEnable);
      this.tabGenerally.Controls.Add((Control) this.groupBox10);
      this.tabGenerally.Controls.Add((Control) this.btnLogClearAndDisableLog);
      this.tabGenerally.Controls.Add((Control) this.btnBurstFlagClear);
      this.tabGenerally.Controls.Add((Control) this.btnUndersizeFlagClear);
      this.tabGenerally.Controls.Add((Control) this.btnOversizeFlagClear);
      this.tabGenerally.Controls.Add((Control) this.btnBlockFlagClear);
      this.tabGenerally.Controls.Add((Control) this.btnLeakFlagClear);
      this.tabGenerally.Controls.Add((Control) this.btnBackflowFlagClear);
      this.tabGenerally.Controls.Add((Control) this.btnTamperFlagClear);
      this.tabGenerally.Controls.Add((Control) this.btnRemovalFlagClear);
      this.tabGenerally.Controls.Add((Control) this.btnSystemLogClear);
      this.tabGenerally.Controls.Add((Control) this.btnEventLogClear);
      this.tabGenerally.Controls.Add((Control) this.btnSND_NKE_IrDaOff);
      this.tabGenerally.Controls.Add((Control) this.btnStartRadioReceiver);
      this.tabGenerally.Controls.Add((Control) this.btnRadioNormal);
      this.tabGenerally.Controls.Add((Control) this.groupBox6);
      this.tabGenerally.Controls.Add((Control) this.btnStartDepassivation);
      this.tabGenerally.Controls.Add((Control) this.groupBox7);
      this.tabGenerally.Controls.Add((Control) this.groupBox3);
      this.tabGenerally.Controls.Add((Control) this.btnResetDevice);
      this.tabGenerally.Controls.Add((Control) this.btnRadioDisable);
      this.tabGenerally.Controls.Add((Control) this.btnReadVersion);
      this.tabGenerally.Controls.Add((Control) this.btnPulseDisable);
      this.tabGenerally.Controls.Add((Control) this.btnPulseEnable);
      this.tabGenerally.Controls.Add((Control) this.btnSendAck);
      this.tabGenerally.Controls.Add((Control) this.btnRunRamBackup);
      this.tabGenerally.Location = new Point(4, 22);
      this.tabGenerally.Name = "tabGenerally";
      this.tabGenerally.Padding = new Padding(3);
      this.tabGenerally.Size = new Size(761, 388);
      this.tabGenerally.TabIndex = 0;
      this.tabGenerally.Text = "Generally";
      this.tabGenerally.UseVisualStyleBackColor = true;
      this.groupBox8.Controls.Add((Control) this.txtObis);
      this.groupBox8.Controls.Add((Control) this.btnReadObis);
      this.groupBox8.Controls.Add((Control) this.label24);
      this.groupBox8.Controls.Add((Control) this.btnWriteObis);
      this.groupBox8.Controls.Add((Control) this.txtManufacturer);
      this.groupBox8.Controls.Add((Control) this.cboxMBusMedium);
      this.groupBox8.Controls.Add((Control) this.btnReadManufacturer);
      this.groupBox8.Controls.Add((Control) this.label23);
      this.groupBox8.Controls.Add((Control) this.btnWriteManufacturer);
      this.groupBox8.Controls.Add((Control) this.btnReadMedium);
      this.groupBox8.Controls.Add((Control) this.label22);
      this.groupBox8.Controls.Add((Control) this.btnWriteMedium);
      this.groupBox8.Controls.Add((Control) this.btnReadGeneration);
      this.groupBox8.Controls.Add((Control) this.label21);
      this.groupBox8.Controls.Add((Control) this.txtGeneration);
      this.groupBox8.Controls.Add((Control) this.btnWriteGeneration);
      this.groupBox8.Controls.Add((Control) this.btnReadAddress);
      this.groupBox8.Controls.Add((Control) this.label20);
      this.groupBox8.Controls.Add((Control) this.txtAddress);
      this.groupBox8.Controls.Add((Control) this.btnWriteAddress);
      this.groupBox8.Controls.Add((Control) this.rbtnPrimary);
      this.groupBox8.Controls.Add((Control) this.rbtnSecondary);
      this.groupBox8.Controls.Add((Control) this.btnReadSerial);
      this.groupBox8.Controls.Add((Control) this.label19);
      this.groupBox8.Controls.Add((Control) this.txtSerial);
      this.groupBox8.Controls.Add((Control) this.btnWriteSerial);
      this.groupBox8.Location = new Point(431, 217);
      this.groupBox8.Name = "groupBox8";
      this.groupBox8.Size = new Size(320, 168);
      this.groupBox8.TabIndex = 77;
      this.groupBox8.TabStop = false;
      this.groupBox8.Text = "M-Bus ID";
      this.txtObis.Location = new Point(89, 144);
      this.txtObis.MaxLength = 2;
      this.txtObis.Name = "txtObis";
      this.txtObis.Size = new Size(89, 20);
      this.txtObis.TabIndex = 87;
      this.btnReadObis.Location = new Point(184, 142);
      this.btnReadObis.Name = "btnReadObis";
      this.btnReadObis.Size = new Size(60, 23);
      this.btnReadObis.TabIndex = 86;
      this.btnReadObis.Text = "Read";
      this.btnReadObis.UseVisualStyleBackColor = true;
      this.btnReadObis.Click += new System.EventHandler(this.btnReadObis_Click);
      this.label24.Location = new Point(4, 145);
      this.label24.Name = "label24";
      this.label24.Size = new Size(79, 15);
      this.label24.TabIndex = 84;
      this.label24.Text = "Obis : 0x";
      this.label24.TextAlign = ContentAlignment.MiddleRight;
      this.btnWriteObis.Location = new Point(243, 142);
      this.btnWriteObis.Name = "btnWriteObis";
      this.btnWriteObis.Size = new Size(60, 23);
      this.btnWriteObis.TabIndex = 83;
      this.btnWriteObis.Text = "Write";
      this.btnWriteObis.UseVisualStyleBackColor = true;
      this.btnWriteObis.Click += new System.EventHandler(this.btnWriteObis_Click);
      this.txtManufacturer.Location = new Point(89, 122);
      this.txtManufacturer.MaxLength = 3;
      this.txtManufacturer.Name = "txtManufacturer";
      this.txtManufacturer.Size = new Size(89, 20);
      this.txtManufacturer.TabIndex = 82;
      this.cboxMBusMedium.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxMBusMedium.FormattingEnabled = true;
      this.cboxMBusMedium.Location = new Point(89, 99);
      this.cboxMBusMedium.Name = "cboxMBusMedium";
      this.cboxMBusMedium.Size = new Size(89, 21);
      this.cboxMBusMedium.TabIndex = 81;
      this.btnReadManufacturer.Location = new Point(184, 120);
      this.btnReadManufacturer.Name = "btnReadManufacturer";
      this.btnReadManufacturer.Size = new Size(60, 23);
      this.btnReadManufacturer.TabIndex = 80;
      this.btnReadManufacturer.Text = "Read";
      this.btnReadManufacturer.UseVisualStyleBackColor = true;
      this.btnReadManufacturer.Click += new System.EventHandler(this.btnReadManufacturer_Click);
      this.label23.Location = new Point(4, 123);
      this.label23.Name = "label23";
      this.label23.Size = new Size(79, 15);
      this.label23.TabIndex = 78;
      this.label23.Text = "Manufacturer:";
      this.label23.TextAlign = ContentAlignment.MiddleRight;
      this.btnWriteManufacturer.Location = new Point(243, 120);
      this.btnWriteManufacturer.Name = "btnWriteManufacturer";
      this.btnWriteManufacturer.Size = new Size(60, 23);
      this.btnWriteManufacturer.TabIndex = 77;
      this.btnWriteManufacturer.Text = "Write";
      this.btnWriteManufacturer.UseVisualStyleBackColor = true;
      this.btnWriteManufacturer.Click += new System.EventHandler(this.btnWriteManufacturer_Click);
      this.btnReadMedium.Location = new Point(184, 98);
      this.btnReadMedium.Name = "btnReadMedium";
      this.btnReadMedium.Size = new Size(60, 23);
      this.btnReadMedium.TabIndex = 76;
      this.btnReadMedium.Text = "Read";
      this.btnReadMedium.UseVisualStyleBackColor = true;
      this.btnReadMedium.Click += new System.EventHandler(this.btnReadMedium_Click);
      this.label22.Location = new Point(4, 101);
      this.label22.Name = "label22";
      this.label22.Size = new Size(79, 15);
      this.label22.TabIndex = 74;
      this.label22.Text = "Medium:";
      this.label22.TextAlign = ContentAlignment.MiddleRight;
      this.btnWriteMedium.Location = new Point(243, 98);
      this.btnWriteMedium.Name = "btnWriteMedium";
      this.btnWriteMedium.Size = new Size(60, 23);
      this.btnWriteMedium.TabIndex = 73;
      this.btnWriteMedium.Text = "Write";
      this.btnWriteMedium.UseVisualStyleBackColor = true;
      this.btnWriteMedium.Click += new System.EventHandler(this.btnWriteMedium_Click);
      this.btnReadGeneration.Location = new Point(184, 76);
      this.btnReadGeneration.Name = "btnReadGeneration";
      this.btnReadGeneration.Size = new Size(60, 23);
      this.btnReadGeneration.TabIndex = 72;
      this.btnReadGeneration.Text = "Read";
      this.btnReadGeneration.UseVisualStyleBackColor = true;
      this.btnReadGeneration.Click += new System.EventHandler(this.btnReadGeneration_Click);
      this.label21.Location = new Point(4, 79);
      this.label21.Name = "label21";
      this.label21.Size = new Size(79, 15);
      this.label21.TabIndex = 70;
      this.label21.Text = "Generation:";
      this.label21.TextAlign = ContentAlignment.MiddleRight;
      this.txtGeneration.Location = new Point(89, 78);
      this.txtGeneration.Maximum = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.txtGeneration.Name = "txtGeneration";
      this.txtGeneration.Size = new Size(89, 20);
      this.txtGeneration.TabIndex = 71;
      this.btnWriteGeneration.Location = new Point(243, 76);
      this.btnWriteGeneration.Name = "btnWriteGeneration";
      this.btnWriteGeneration.Size = new Size(60, 23);
      this.btnWriteGeneration.TabIndex = 69;
      this.btnWriteGeneration.Text = "Write";
      this.btnWriteGeneration.UseVisualStyleBackColor = true;
      this.btnWriteGeneration.Click += new System.EventHandler(this.btnWriteGeneration_Click);
      this.btnReadAddress.Location = new Point(184, 54);
      this.btnReadAddress.Name = "btnReadAddress";
      this.btnReadAddress.Size = new Size(60, 23);
      this.btnReadAddress.TabIndex = 68;
      this.btnReadAddress.Text = "Read";
      this.btnReadAddress.UseVisualStyleBackColor = true;
      this.btnReadAddress.Click += new System.EventHandler(this.btnReadAddress_Click);
      this.label20.Location = new Point(4, 57);
      this.label20.Name = "label20";
      this.label20.Size = new Size(79, 15);
      this.label20.TabIndex = 66;
      this.label20.Text = "Address:";
      this.label20.TextAlign = ContentAlignment.MiddleRight;
      this.txtAddress.Location = new Point(89, 56);
      this.txtAddress.Maximum = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.txtAddress.Name = "txtAddress";
      this.txtAddress.Size = new Size(89, 20);
      this.txtAddress.TabIndex = 67;
      this.btnWriteAddress.Location = new Point(243, 54);
      this.btnWriteAddress.Name = "btnWriteAddress";
      this.btnWriteAddress.Size = new Size(60, 23);
      this.btnWriteAddress.TabIndex = 65;
      this.btnWriteAddress.Text = "Write";
      this.btnWriteAddress.UseVisualStyleBackColor = true;
      this.btnWriteAddress.Click += new System.EventHandler(this.btnWriteAddress_Click);
      this.rbtnPrimary.AutoSize = true;
      this.rbtnPrimary.Location = new Point(33, 13);
      this.rbtnPrimary.Name = "rbtnPrimary";
      this.rbtnPrimary.Size = new Size((int) sbyte.MaxValue, 17);
      this.rbtnPrimary.TabIndex = 64;
      this.rbtnPrimary.Text = "Primary (EDC module)";
      this.rbtnPrimary.UseVisualStyleBackColor = true;
      this.rbtnSecondary.AutoSize = true;
      this.rbtnSecondary.Checked = true;
      this.rbtnSecondary.Location = new Point(174, 13);
      this.rbtnSecondary.Name = "rbtnSecondary";
      this.rbtnSecondary.Size = new Size(140, 17);
      this.rbtnSecondary.TabIndex = 63;
      this.rbtnSecondary.TabStop = true;
      this.rbtnSecondary.Text = "Secondary (water meter)";
      this.rbtnSecondary.UseVisualStyleBackColor = true;
      this.btnReadSerial.Location = new Point(184, 32);
      this.btnReadSerial.Name = "btnReadSerial";
      this.btnReadSerial.Size = new Size(60, 23);
      this.btnReadSerial.TabIndex = 62;
      this.btnReadSerial.Text = "Read";
      this.btnReadSerial.UseVisualStyleBackColor = true;
      this.btnReadSerial.Click += new System.EventHandler(this.btnReadSerial_Click);
      this.label19.Location = new Point(4, 35);
      this.label19.Name = "label19";
      this.label19.Size = new Size(79, 15);
      this.label19.TabIndex = 60;
      this.label19.Text = "Serial:";
      this.label19.TextAlign = ContentAlignment.MiddleRight;
      this.txtSerial.Location = new Point(89, 34);
      this.txtSerial.Maximum = new Decimal(new int[4]
      {
        99999999,
        0,
        0,
        0
      });
      this.txtSerial.Name = "txtSerial";
      this.txtSerial.Size = new Size(89, 20);
      this.txtSerial.TabIndex = 61;
      this.btnWriteSerial.Location = new Point(243, 32);
      this.btnWriteSerial.Name = "btnWriteSerial";
      this.btnWriteSerial.Size = new Size(60, 23);
      this.btnWriteSerial.TabIndex = 59;
      this.btnWriteSerial.Text = "Write";
      this.btnWriteSerial.UseVisualStyleBackColor = true;
      this.btnWriteSerial.Click += new System.EventHandler(this.btnWriteSerial_Click);
      this.buttonHardwareTypeManager.Location = new Point(313, 65);
      this.buttonHardwareTypeManager.Name = "buttonHardwareTypeManager";
      this.buttonHardwareTypeManager.Size = new Size(147, 23);
      this.buttonHardwareTypeManager.TabIndex = 76;
      this.buttonHardwareTypeManager.Text = "Hardware type editor";
      this.buttonHardwareTypeManager.UseVisualStyleBackColor = true;
      this.buttonHardwareTypeManager.Click += new System.EventHandler(this.buttonHardwareTypeManager_Click);
      this.btnLogDisable.Location = new Point(313, 35);
      this.btnLogDisable.Name = "btnLogDisable";
      this.btnLogDisable.Size = new Size(147, 23);
      this.btnLogDisable.TabIndex = 76;
      this.btnLogDisable.Text = "Log Disable";
      this.btnLogDisable.UseVisualStyleBackColor = true;
      this.btnLogDisable.Click += new System.EventHandler(this.btnLogDisable_Click);
      this.btnLogEnable.Location = new Point(313, 6);
      this.btnLogEnable.Name = "btnLogEnable";
      this.btnLogEnable.Size = new Size(147, 23);
      this.btnLogEnable.TabIndex = 75;
      this.btnLogEnable.Text = "Log Enable";
      this.btnLogEnable.UseVisualStyleBackColor = true;
      this.btnLogEnable.Click += new System.EventHandler(this.btnLogEnable_Click);
      this.groupBox10.Controls.Add((Control) this.btnReadMeterValue);
      this.groupBox10.Controls.Add((Control) this.label16);
      this.groupBox10.Controls.Add((Control) this.txtMeterValue);
      this.groupBox10.Controls.Add((Control) this.btnWriteMeterValue);
      this.groupBox10.Location = new Point(469, 4);
      this.groupBox10.Name = "groupBox10";
      this.groupBox10.Size = new Size(282, 45);
      this.groupBox10.TabIndex = 74;
      this.groupBox10.TabStop = false;
      this.groupBox10.Text = "Meter value";
      this.btnReadMeterValue.Location = new Point(213, 16);
      this.btnReadMeterValue.Name = "btnReadMeterValue";
      this.btnReadMeterValue.Size = new Size(60, 23);
      this.btnReadMeterValue.TabIndex = 62;
      this.btnReadMeterValue.Text = "Read";
      this.btnReadMeterValue.UseVisualStyleBackColor = true;
      this.btnReadMeterValue.Click += new System.EventHandler(this.btnReadMeterValue_Click);
      this.label16.Location = new Point(4, 18);
      this.label16.Name = "label16";
      this.label16.Size = new Size(38, 15);
      this.label16.TabIndex = 60;
      this.label16.Text = "Pulse:";
      this.label16.TextAlign = ContentAlignment.MiddleRight;
      this.txtMeterValue.Location = new Point(48, 17);
      this.txtMeterValue.Maximum = new Decimal(new int[4]
      {
        int.MaxValue,
        0,
        0,
        0
      });
      this.txtMeterValue.Minimum = new Decimal(new int[4]
      {
        int.MinValue,
        0,
        0,
        int.MinValue
      });
      this.txtMeterValue.Name = "txtMeterValue";
      this.txtMeterValue.Size = new Size(92, 20);
      this.txtMeterValue.TabIndex = 61;
      this.btnWriteMeterValue.Location = new Point(147, 16);
      this.btnWriteMeterValue.Name = "btnWriteMeterValue";
      this.btnWriteMeterValue.Size = new Size(60, 23);
      this.btnWriteMeterValue.TabIndex = 59;
      this.btnWriteMeterValue.Text = "Write";
      this.btnWriteMeterValue.UseVisualStyleBackColor = true;
      this.btnWriteMeterValue.Click += new System.EventHandler(this.btnWriteMeterValue_Click);
      this.btnLogClearAndDisableLog.Location = new Point(160, 6);
      this.btnLogClearAndDisableLog.Name = "btnLogClearAndDisableLog";
      this.btnLogClearAndDisableLog.Size = new Size(147, 23);
      this.btnLogClearAndDisableLog.TabIndex = 73;
      this.btnLogClearAndDisableLog.Text = "Log Clear && Disable Log";
      this.btnLogClearAndDisableLog.UseVisualStyleBackColor = true;
      this.btnLogClearAndDisableLog.Click += new System.EventHandler(this.btnLogClearAndDisableLog_Click);
      this.btnBurstFlagClear.Location = new Point(160, 296);
      this.btnBurstFlagClear.Name = "btnBurstFlagClear";
      this.btnBurstFlagClear.Size = new Size(147, 23);
      this.btnBurstFlagClear.TabIndex = 72;
      this.btnBurstFlagClear.Text = "Burst Flag Clear";
      this.btnBurstFlagClear.UseVisualStyleBackColor = true;
      this.btnBurstFlagClear.Click += new System.EventHandler(this.btnBurstFlagClear_Click);
      this.btnUndersizeFlagClear.Location = new Point(160, 267);
      this.btnUndersizeFlagClear.Name = "btnUndersizeFlagClear";
      this.btnUndersizeFlagClear.Size = new Size(147, 23);
      this.btnUndersizeFlagClear.TabIndex = 71;
      this.btnUndersizeFlagClear.Text = "Undersize Flag Clear";
      this.btnUndersizeFlagClear.UseVisualStyleBackColor = true;
      this.btnUndersizeFlagClear.Click += new System.EventHandler(this.btnUndersizeFlagClear_Click);
      this.btnOversizeFlagClear.Location = new Point(160, 238);
      this.btnOversizeFlagClear.Name = "btnOversizeFlagClear";
      this.btnOversizeFlagClear.Size = new Size(147, 23);
      this.btnOversizeFlagClear.TabIndex = 70;
      this.btnOversizeFlagClear.Text = "Oversize Flag Clear";
      this.btnOversizeFlagClear.UseVisualStyleBackColor = true;
      this.btnOversizeFlagClear.Click += new System.EventHandler(this.btnOversizeFlagClear_Click);
      this.btnBlockFlagClear.Location = new Point(160, 208);
      this.btnBlockFlagClear.Name = "btnBlockFlagClear";
      this.btnBlockFlagClear.Size = new Size(147, 23);
      this.btnBlockFlagClear.TabIndex = 69;
      this.btnBlockFlagClear.Text = "Block Flag Clear";
      this.btnBlockFlagClear.UseVisualStyleBackColor = true;
      this.btnBlockFlagClear.Click += new System.EventHandler(this.btnBlockFlagClear_Click);
      this.btnLeakFlagClear.Location = new Point(160, 180);
      this.btnLeakFlagClear.Name = "btnLeakFlagClear";
      this.btnLeakFlagClear.Size = new Size(147, 23);
      this.btnLeakFlagClear.TabIndex = 68;
      this.btnLeakFlagClear.Text = "Leak Flag Clear";
      this.btnLeakFlagClear.UseVisualStyleBackColor = true;
      this.btnLeakFlagClear.Click += new System.EventHandler(this.btnLeakFlagClear_Click);
      this.btnBackflowFlagClear.Location = new Point(160, 151);
      this.btnBackflowFlagClear.Name = "btnBackflowFlagClear";
      this.btnBackflowFlagClear.Size = new Size(147, 23);
      this.btnBackflowFlagClear.TabIndex = 67;
      this.btnBackflowFlagClear.Text = "Backflow Flag Clear";
      this.btnBackflowFlagClear.UseVisualStyleBackColor = true;
      this.btnBackflowFlagClear.Click += new System.EventHandler(this.btnBackflowFlagClear_Click);
      this.btnTamperFlagClear.Location = new Point(160, 122);
      this.btnTamperFlagClear.Name = "btnTamperFlagClear";
      this.btnTamperFlagClear.Size = new Size(147, 23);
      this.btnTamperFlagClear.TabIndex = 66;
      this.btnTamperFlagClear.Text = "Tamper Flag Clear";
      this.btnTamperFlagClear.UseVisualStyleBackColor = true;
      this.btnTamperFlagClear.Click += new System.EventHandler(this.btnTamperFlagClear_Click);
      this.btnRemovalFlagClear.Location = new Point(160, 93);
      this.btnRemovalFlagClear.Name = "btnRemovalFlagClear";
      this.btnRemovalFlagClear.Size = new Size(147, 23);
      this.btnRemovalFlagClear.TabIndex = 65;
      this.btnRemovalFlagClear.Text = "Removal Flag Clear";
      this.btnRemovalFlagClear.UseVisualStyleBackColor = true;
      this.btnRemovalFlagClear.Click += new System.EventHandler(this.btnRemovalFlagClear_Click);
      this.btnSystemLogClear.Location = new Point(160, 64);
      this.btnSystemLogClear.Name = "btnSystemLogClear";
      this.btnSystemLogClear.Size = new Size(147, 23);
      this.btnSystemLogClear.TabIndex = 64;
      this.btnSystemLogClear.Text = "System Log Clear";
      this.btnSystemLogClear.UseVisualStyleBackColor = true;
      this.btnSystemLogClear.Click += new System.EventHandler(this.btnSystemLogClear_Click);
      this.btnEventLogClear.Location = new Point(160, 35);
      this.btnEventLogClear.Name = "btnEventLogClear";
      this.btnEventLogClear.Size = new Size(147, 23);
      this.btnEventLogClear.TabIndex = 63;
      this.btnEventLogClear.Text = "Event Log Clear";
      this.btnEventLogClear.UseVisualStyleBackColor = true;
      this.btnEventLogClear.Click += new System.EventHandler(this.btnEventLogClear_Click);
      this.btnSND_NKE_IrDaOff.Location = new Point(7, 296);
      this.btnSND_NKE_IrDaOff.Name = "btnSND_NKE_IrDaOff";
      this.btnSND_NKE_IrDaOff.Size = new Size(147, 23);
      this.btnSND_NKE_IrDaOff.TabIndex = 62;
      this.btnSND_NKE_IrDaOff.Text = "IrDa Off (SND_NKE)";
      this.btnSND_NKE_IrDaOff.UseVisualStyleBackColor = true;
      this.btnSND_NKE_IrDaOff.Click += new System.EventHandler(this.btnSND_NKE_IrDaOff_Click);
      this.btnStartRadioReceiver.Location = new Point(7, 179);
      this.btnStartRadioReceiver.Name = "btnStartRadioReceiver";
      this.btnStartRadioReceiver.Size = new Size(147, 23);
      this.btnStartRadioReceiver.TabIndex = 7;
      this.btnStartRadioReceiver.Text = "Start Radio Receiver";
      this.btnStartRadioReceiver.UseVisualStyleBackColor = true;
      this.btnStartRadioReceiver.Click += new System.EventHandler(this.btnStartRadioReceiver_Click);
      this.btnRadioNormal.Location = new Point(7, 93);
      this.btnRadioNormal.Name = "btnRadioNormal";
      this.btnRadioNormal.Size = new Size(147, 23);
      this.btnRadioNormal.TabIndex = 4;
      this.btnRadioNormal.Text = "Radio Normal";
      this.btnRadioNormal.UseVisualStyleBackColor = true;
      this.btnRadioNormal.Click += new System.EventHandler(this.btnRadioNormal_Click);
      this.groupBox6.Controls.Add((Control) this.label11);
      this.groupBox6.Controls.Add((Control) this.txtTimeoutRadioReceive);
      this.groupBox6.Controls.Add((Control) this.btnRadioReceive);
      this.groupBox6.Location = new Point(7, 322);
      this.groupBox6.Name = "groupBox6";
      this.groupBox6.Size = new Size(282, 45);
      this.groupBox6.TabIndex = 60;
      this.groupBox6.TabStop = false;
      this.groupBox6.Text = "Radio";
      this.label11.Location = new Point(5, 18);
      this.label11.Name = "label11";
      this.label11.Size = new Size(75, 15);
      this.label11.TabIndex = 60;
      this.label11.Text = "Timeout (ms):";
      this.label11.TextAlign = ContentAlignment.MiddleRight;
      this.txtTimeoutRadioReceive.Location = new Point(85, 17);
      this.txtTimeoutRadioReceive.Maximum = new Decimal(new int[4]
      {
        int.MaxValue,
        0,
        0,
        0
      });
      this.txtTimeoutRadioReceive.Name = "txtTimeoutRadioReceive";
      this.txtTimeoutRadioReceive.Size = new Size(67, 20);
      this.txtTimeoutRadioReceive.TabIndex = 61;
      this.txtTimeoutRadioReceive.Value = new Decimal(new int[4]
      {
        10000,
        0,
        0,
        0
      });
      this.btnRadioReceive.Location = new Point(159, 16);
      this.btnRadioReceive.Name = "btnRadioReceive";
      this.btnRadioReceive.Size = new Size(106, 23);
      this.btnRadioReceive.TabIndex = 59;
      this.btnRadioReceive.Text = "Radio Receive";
      this.btnRadioReceive.UseVisualStyleBackColor = true;
      this.btnRadioReceive.Click += new System.EventHandler(this.btnRadioReceive_Click);
      this.btnStartDepassivation.Location = new Point(7, 267);
      this.btnStartDepassivation.Name = "btnStartDepassivation";
      this.btnStartDepassivation.Size = new Size(147, 23);
      this.btnStartDepassivation.TabIndex = 46;
      this.btnStartDepassivation.Text = "Start Depassivation";
      this.btnStartDepassivation.UseVisualStyleBackColor = true;
      this.btnStartDepassivation.Click += new System.EventHandler(this.btnStartDepassivation_Click);
      this.groupBox7.Controls.Add((Control) this.label34);
      this.groupBox7.Controls.Add((Control) this.txtRadioTimeout);
      this.groupBox7.Controls.Add((Control) this.label17);
      this.groupBox7.Controls.Add((Control) this.txtRadioFrequencyOffset);
      this.groupBox7.Controls.Add((Control) this.label10);
      this.groupBox7.Controls.Add((Control) this.btnRadioOOK);
      this.groupBox7.Controls.Add((Control) this.btnRadioPN9);
      this.groupBox7.Controls.Add((Control) this.cboxRadioModeForTest);
      this.groupBox7.Location = new Point(469, 51);
      this.groupBox7.Name = "groupBox7";
      this.groupBox7.Size = new Size(282, 86);
      this.groupBox7.TabIndex = 44;
      this.groupBox7.TabStop = false;
      this.groupBox7.Text = "Radio test";
      this.label34.Location = new Point(5, 62);
      this.label34.Name = "label34";
      this.label34.Size = new Size(106, 15);
      this.label34.TabIndex = 57;
      this.label34.Text = "Timeout (seconds):";
      this.label34.TextAlign = ContentAlignment.MiddleRight;
      this.txtRadioTimeout.Location = new Point(119, 60);
      this.txtRadioTimeout.Maximum = new Decimal(new int[4]
      {
        65536,
        0,
        0,
        0
      });
      this.txtRadioTimeout.Name = "txtRadioTimeout";
      this.txtRadioTimeout.Size = new Size(94, 20);
      this.txtRadioTimeout.TabIndex = 58;
      this.txtRadioTimeout.Value = new Decimal(new int[4]
      {
        10,
        0,
        0,
        0
      });
      this.label17.Location = new Point(5, 39);
      this.label17.Name = "label17";
      this.label17.Size = new Size(106, 15);
      this.label17.TabIndex = 55;
      this.label17.Text = "Frequency offset:";
      this.label17.TextAlign = ContentAlignment.MiddleRight;
      this.txtRadioFrequencyOffset.Location = new Point(119, 37);
      this.txtRadioFrequencyOffset.Maximum = new Decimal(new int[4]
      {
        (int) short.MaxValue,
        0,
        0,
        0
      });
      this.txtRadioFrequencyOffset.Minimum = new Decimal(new int[4]
      {
        32768,
        0,
        0,
        int.MinValue
      });
      this.txtRadioFrequencyOffset.Name = "txtRadioFrequencyOffset";
      this.txtRadioFrequencyOffset.Size = new Size(94, 20);
      this.txtRadioFrequencyOffset.TabIndex = 56;
      this.label10.Location = new Point(5, 16);
      this.label10.Name = "label10";
      this.label10.Size = new Size(106, 15);
      this.label10.TabIndex = 54;
      this.label10.Text = "Mode:";
      this.label10.TextAlign = ContentAlignment.MiddleRight;
      this.btnRadioOOK.Location = new Point(220, 12);
      this.btnRadioOOK.Name = "btnRadioOOK";
      this.btnRadioOOK.Size = new Size(56, 23);
      this.btnRadioOOK.TabIndex = 41;
      this.btnRadioOOK.Text = "OOK";
      this.btnRadioOOK.UseVisualStyleBackColor = true;
      this.btnRadioOOK.Click += new System.EventHandler(this.btnRadioOOK_Click);
      this.btnRadioPN9.Location = new Point(220, 41);
      this.btnRadioPN9.Name = "btnRadioPN9";
      this.btnRadioPN9.Size = new Size(56, 23);
      this.btnRadioPN9.TabIndex = 42;
      this.btnRadioPN9.Text = "PN9";
      this.btnRadioPN9.UseVisualStyleBackColor = true;
      this.btnRadioPN9.Click += new System.EventHandler(this.btnRadioPN9_Click);
      this.cboxRadioModeForTest.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxRadioModeForTest.FormattingEnabled = true;
      this.cboxRadioModeForTest.Location = new Point(119, 14);
      this.cboxRadioModeForTest.Name = "cboxRadioModeForTest";
      this.cboxRadioModeForTest.Size = new Size(95, 21);
      this.cboxRadioModeForTest.TabIndex = 53;
      this.groupBox3.Controls.Add((Control) this.btnSetCurrentTime);
      this.groupBox3.Controls.Add((Control) this.btnReadSystemTime);
      this.groupBox3.Controls.Add((Control) this.btnWriteSystemTime);
      this.groupBox3.Controls.Add((Control) this.txtSystemTime);
      this.groupBox3.Location = new Point(469, 140);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new Size(282, 71);
      this.groupBox3.TabIndex = 43;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "System time";
      this.btnSetCurrentTime.Location = new Point(154, 19);
      this.btnSetCurrentTime.Name = "btnSetCurrentTime";
      this.btnSetCurrentTime.Size = new Size(60, 23);
      this.btnSetCurrentTime.TabIndex = 47;
      this.btnSetCurrentTime.Text = "Current";
      this.btnSetCurrentTime.UseVisualStyleBackColor = true;
      this.btnSetCurrentTime.Click += new System.EventHandler(this.btnSetCurrentTime_Click);
      this.btnReadSystemTime.Location = new Point(18, 42);
      this.btnReadSystemTime.Name = "btnReadSystemTime";
      this.btnReadSystemTime.Size = new Size(60, 23);
      this.btnReadSystemTime.TabIndex = 45;
      this.btnReadSystemTime.Text = "Read";
      this.btnReadSystemTime.UseVisualStyleBackColor = true;
      this.btnReadSystemTime.Click += new System.EventHandler(this.btnReadSystemTime_Click);
      this.btnWriteSystemTime.Location = new Point(85, 42);
      this.btnWriteSystemTime.Name = "btnWriteSystemTime";
      this.btnWriteSystemTime.Size = new Size(60, 23);
      this.btnWriteSystemTime.TabIndex = 46;
      this.btnWriteSystemTime.Text = "Write";
      this.btnWriteSystemTime.UseVisualStyleBackColor = true;
      this.btnWriteSystemTime.Click += new System.EventHandler(this.btnWriteSystemTime_Click);
      this.txtSystemTime.CustomFormat = "dd.MM.yyyy HH:mm:ss";
      this.txtSystemTime.Format = DateTimePickerFormat.Custom;
      this.txtSystemTime.Location = new Point(18, 19);
      this.txtSystemTime.Name = "txtSystemTime";
      this.txtSystemTime.ShowUpDown = true;
      this.txtSystemTime.Size = new Size((int) sbyte.MaxValue, 20);
      this.txtSystemTime.TabIndex = 44;
      this.btnResetDevice.Location = new Point(7, 6);
      this.btnResetDevice.Name = "btnResetDevice";
      this.btnResetDevice.Size = new Size(147, 23);
      this.btnResetDevice.TabIndex = 1;
      this.btnResetDevice.Text = "Reset Device";
      this.btnResetDevice.UseVisualStyleBackColor = true;
      this.btnResetDevice.Click += new System.EventHandler(this.btnResetDevice_Click);
      this.btnRadioDisable.Location = new Point(7, 209);
      this.btnRadioDisable.Name = "btnRadioDisable";
      this.btnRadioDisable.Size = new Size(147, 23);
      this.btnRadioDisable.TabIndex = 8;
      this.btnRadioDisable.Text = "Radio Disable";
      this.btnRadioDisable.UseVisualStyleBackColor = true;
      this.btnRadioDisable.Click += new System.EventHandler(this.btnRadioDisable_Click);
      this.btnReadVersion.Location = new Point(7, 35);
      this.btnReadVersion.Name = "btnReadVersion";
      this.btnReadVersion.Size = new Size(147, 23);
      this.btnReadVersion.TabIndex = 2;
      this.btnReadVersion.Text = "Read Version";
      this.btnReadVersion.UseVisualStyleBackColor = true;
      this.btnReadVersion.Click += new System.EventHandler(this.btnReadVersion_Click);
      this.btnPulseDisable.Location = new Point(7, 122);
      this.btnPulseDisable.Name = "btnPulseDisable";
      this.btnPulseDisable.Size = new Size(147, 23);
      this.btnPulseDisable.TabIndex = 5;
      this.btnPulseDisable.Text = "Pulse Disable";
      this.btnPulseDisable.UseVisualStyleBackColor = true;
      this.btnPulseDisable.Click += new System.EventHandler(this.btnPulseDisable_Click);
      this.btnPulseEnable.Location = new Point(7, 151);
      this.btnPulseEnable.Name = "btnPulseEnable";
      this.btnPulseEnable.Size = new Size(147, 23);
      this.btnPulseEnable.TabIndex = 6;
      this.btnPulseEnable.Text = "Pulse Enable";
      this.btnPulseEnable.UseVisualStyleBackColor = true;
      this.btnPulseEnable.Click += new System.EventHandler(this.btnPulseEnable_Click);
      this.btnSendAck.Location = new Point(7, 238);
      this.btnSendAck.Name = "btnSendAck";
      this.btnSendAck.Size = new Size(147, 23);
      this.btnSendAck.TabIndex = 9;
      this.btnSendAck.Text = "Send 0xE5";
      this.btnSendAck.UseVisualStyleBackColor = true;
      this.btnSendAck.Click += new System.EventHandler(this.btnSendAck_Click);
      this.btnRunRamBackup.Location = new Point(7, 64);
      this.btnRunRamBackup.Name = "btnRunRamBackup";
      this.btnRunRamBackup.Size = new Size(147, 23);
      this.btnRunRamBackup.TabIndex = 3;
      this.btnRunRamBackup.Text = "Run RAM Backup";
      this.btnRunRamBackup.UseVisualStyleBackColor = true;
      this.btnRunRamBackup.Click += new System.EventHandler(this.btnRunRamBackup_Click);
      this.tabs.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.tabs.Controls.Add((Control) this.tabGenerally);
      this.tabs.Controls.Add((Control) this.tabMemory);
      this.tabs.Controls.Add((Control) this.tabMinoConnect);
      this.tabs.Controls.Add((Control) this.tabWiredMbus);
      this.tabs.Location = new Point(3, 3);
      this.tabs.Name = "tabs";
      this.tabs.SelectedIndex = 0;
      this.tabs.Size = new Size(769, 414);
      this.tabs.TabIndex = 33;
      this.tabs.Selected += new TabControlEventHandler(this.tabs_Selected);
      this.tabMemory.Controls.Add((Control) this.groupBox4);
      this.tabMemory.Controls.Add((Control) this.groupBox1);
      this.tabMemory.Location = new Point(4, 22);
      this.tabMemory.Name = "tabMemory";
      this.tabMemory.Size = new Size(761, 388);
      this.tabMemory.TabIndex = 2;
      this.tabMemory.Text = "Memory";
      this.tabMemory.UseVisualStyleBackColor = true;
      this.groupBox4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox4.Controls.Add((Control) this.txtWriteMemoryBuffer);
      this.groupBox4.Controls.Add((Control) this.txtStartAddressByWrite);
      this.groupBox4.Controls.Add((Control) this.label15);
      this.groupBox4.Controls.Add((Control) this.btnWriteMemory);
      this.groupBox4.Location = new Point(179, 7);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new Size(579, 116);
      this.groupBox4.TabIndex = 35;
      this.groupBox4.TabStop = false;
      this.txtWriteMemoryBuffer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtWriteMemoryBuffer.Location = new Point(8, 42);
      this.txtWriteMemoryBuffer.Multiline = true;
      this.txtWriteMemoryBuffer.Name = "txtWriteMemoryBuffer";
      this.txtWriteMemoryBuffer.Size = new Size(565, 65);
      this.txtWriteMemoryBuffer.TabIndex = 31;
      this.txtStartAddressByWrite.Location = new Point(102, 13);
      this.txtStartAddressByWrite.Name = "txtStartAddressByWrite";
      this.txtStartAddressByWrite.Size = new Size(45, 20);
      this.txtStartAddressByWrite.TabIndex = 24;
      this.txtStartAddressByWrite.Text = "FFFF";
      this.label15.Location = new Point(6, 17);
      this.label15.Name = "label15";
      this.label15.Size = new Size(90, 13);
      this.label15.TabIndex = 25;
      this.label15.Text = "Start address: 0x";
      this.label15.TextAlign = ContentAlignment.MiddleRight;
      this.btnWriteMemory.Location = new Point(157, 11);
      this.btnWriteMemory.Name = "btnWriteMemory";
      this.btnWriteMemory.Size = new Size(147, 23);
      this.btnWriteMemory.TabIndex = 23;
      this.btnWriteMemory.Text = "Write Memory";
      this.btnWriteMemory.UseVisualStyleBackColor = true;
      this.btnWriteMemory.Click += new System.EventHandler(this.btnWriteMemory_Click);
      this.groupBox1.Controls.Add((Control) this.label5);
      this.groupBox1.Controls.Add((Control) this.label4);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.txtByteSize);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Controls.Add((Control) this.txtEndAddress);
      this.groupBox1.Controls.Add((Control) this.txtStartAddress);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Controls.Add((Control) this.lblStartAddress);
      this.groupBox1.Controls.Add((Control) this.btnReadMemory);
      this.groupBox1.Location = new Point(12, 7);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(161, 116);
      this.groupBox1.TabIndex = 34;
      this.groupBox1.TabStop = false;
      this.label5.Location = new Point(123, 94);
      this.label5.Name = "label5";
      this.label5.Size = new Size(32, 13);
      this.label5.TabIndex = 32;
      this.label5.Text = "bytes";
      this.label5.TextAlign = ContentAlignment.MiddleLeft;
      this.label4.Location = new Point(123, 69);
      this.label4.Name = "label4";
      this.label4.Size = new Size(28, 13);
      this.label4.TabIndex = 31;
      this.label4.Text = "hex";
      this.label4.TextAlign = ContentAlignment.MiddleLeft;
      this.label3.Location = new Point(123, 45);
      this.label3.Name = "label3";
      this.label3.Size = new Size(28, 13);
      this.label3.TabIndex = 30;
      this.label3.Text = "hex";
      this.label3.TextAlign = ContentAlignment.MiddleLeft;
      this.txtByteSize.Location = new Point(56, 90);
      this.txtByteSize.Name = "txtByteSize";
      this.txtByteSize.Size = new Size(65, 20);
      this.txtByteSize.TabIndex = 28;
      this.txtByteSize.Text = "512";
      this.txtByteSize.TextChanged += new System.EventHandler(this.txtByteSize_TextChanged);
      this.label2.Location = new Point(6, 94);
      this.label2.Name = "label2";
      this.label2.Size = new Size(51, 13);
      this.label2.TabIndex = 29;
      this.label2.Text = "Size:";
      this.label2.TextAlign = ContentAlignment.MiddleRight;
      this.txtEndAddress.Location = new Point(56, 66);
      this.txtEndAddress.Name = "txtEndAddress";
      this.txtEndAddress.Size = new Size(65, 20);
      this.txtEndAddress.TabIndex = 26;
      this.txtEndAddress.Text = "9800";
      this.txtEndAddress.TextChanged += new System.EventHandler(this.txtStartEnd_TextChanged);
      this.txtStartAddress.Location = new Point(56, 42);
      this.txtStartAddress.Name = "txtStartAddress";
      this.txtStartAddress.Size = new Size(65, 20);
      this.txtStartAddress.TabIndex = 24;
      this.txtStartAddress.Text = "9600";
      this.txtStartAddress.TextChanged += new System.EventHandler(this.txtStartEnd_TextChanged);
      this.label1.Location = new Point(5, 70);
      this.label1.Name = "label1";
      this.label1.Size = new Size(48, 13);
      this.label1.TabIndex = 27;
      this.label1.Text = "End: 0x";
      this.label1.TextAlign = ContentAlignment.MiddleRight;
      this.lblStartAddress.Location = new Point(5, 46);
      this.lblStartAddress.Name = "lblStartAddress";
      this.lblStartAddress.Size = new Size(48, 13);
      this.lblStartAddress.TabIndex = 25;
      this.lblStartAddress.Text = "Start: 0x";
      this.lblStartAddress.TextAlign = ContentAlignment.MiddleRight;
      this.btnReadMemory.Location = new Point(4, 13);
      this.btnReadMemory.Name = "btnReadMemory";
      this.btnReadMemory.Size = new Size(147, 23);
      this.btnReadMemory.TabIndex = 23;
      this.btnReadMemory.Text = "Read Memory";
      this.btnReadMemory.UseVisualStyleBackColor = true;
      this.btnReadMemory.Click += new System.EventHandler(this.btnReadMemory_Click);
      this.tabMinoConnect.Controls.Add((Control) this.btnCloseSecondaryComPort);
      this.tabMinoConnect.Controls.Add((Control) this.btnOpenSecondaryComPort);
      this.tabMinoConnect.Controls.Add((Control) this.label14);
      this.tabMinoConnect.Controls.Add((Control) this.cboxPortNames);
      this.tabMinoConnect.Controls.Add((Control) this.groupBox9);
      this.tabMinoConnect.Controls.Add((Control) this.groupBox5);
      this.tabMinoConnect.Location = new Point(4, 22);
      this.tabMinoConnect.Name = "tabMinoConnect";
      this.tabMinoConnect.Size = new Size(761, 388);
      this.tabMinoConnect.TabIndex = 1;
      this.tabMinoConnect.Text = "MinoConnect";
      this.tabMinoConnect.UseVisualStyleBackColor = true;
      this.btnCloseSecondaryComPort.Location = new Point(327, 15);
      this.btnCloseSecondaryComPort.Margin = new Padding(2, 3, 2, 3);
      this.btnCloseSecondaryComPort.Name = "btnCloseSecondaryComPort";
      this.btnCloseSecondaryComPort.Size = new Size(77, 23);
      this.btnCloseSecondaryComPort.TabIndex = 33;
      this.btnCloseSecondaryComPort.Text = "Close";
      this.btnCloseSecondaryComPort.UseVisualStyleBackColor = true;
      this.btnCloseSecondaryComPort.Click += new System.EventHandler(this.btnCloseSecondaryComPort_Click);
      this.btnOpenSecondaryComPort.Location = new Point(246, 15);
      this.btnOpenSecondaryComPort.Margin = new Padding(2, 3, 2, 3);
      this.btnOpenSecondaryComPort.Name = "btnOpenSecondaryComPort";
      this.btnOpenSecondaryComPort.Size = new Size(77, 23);
      this.btnOpenSecondaryComPort.TabIndex = 32;
      this.btnOpenSecondaryComPort.Text = "Open";
      this.btnOpenSecondaryComPort.UseVisualStyleBackColor = true;
      this.btnOpenSecondaryComPort.Click += new System.EventHandler(this.btnOpenSecondaryComPort_Click);
      this.label14.AutoSize = true;
      this.label14.Location = new Point(21, 20);
      this.label14.Name = "label14";
      this.label14.Size = new Size(134, 13);
      this.label14.TabIndex = 31;
      this.label14.Text = "Second MinoConnect port:";
      this.cboxPortNames.FormattingEnabled = true;
      this.cboxPortNames.Location = new Point(160, 17);
      this.cboxPortNames.Name = "cboxPortNames";
      this.cboxPortNames.Size = new Size(71, 21);
      this.cboxPortNames.TabIndex = 30;
      this.groupBox9.Controls.Add((Control) this.txtSendTestPacketPower);
      this.groupBox9.Controls.Add((Control) this.label12);
      this.groupBox9.Controls.Add((Control) this.btnStopSendMinoConnectTestPacket);
      this.groupBox9.Controls.Add((Control) this.label13);
      this.groupBox9.Controls.Add((Control) this.cboxRadioModeForMiConTestPacket);
      this.groupBox9.Controls.Add((Control) this.btnStartSendMinoConnectTestPacket);
      this.groupBox9.Location = new Point(281, 44);
      this.groupBox9.Name = "groupBox9";
      this.groupBox9.Size = new Size(252, 148);
      this.groupBox9.TabIndex = 29;
      this.groupBox9.TabStop = false;
      this.groupBox9.Text = "Transmit";
      this.txtSendTestPacketPower.Location = new Point(88, 44);
      this.txtSendTestPacketPower.Maximum = new Decimal(new int[4]
      {
        7,
        0,
        0,
        0
      });
      this.txtSendTestPacketPower.Name = "txtSendTestPacketPower";
      this.txtSendTestPacketPower.Size = new Size(121, 20);
      this.txtSendTestPacketPower.TabIndex = 30;
      this.txtSendTestPacketPower.Value = new Decimal(new int[4]
      {
        7,
        0,
        0,
        0
      });
      this.label12.Location = new Point(7, 44);
      this.label12.Name = "label12";
      this.label12.Size = new Size(75, 15);
      this.label12.TabIndex = 29;
      this.label12.Text = "Power (0-7):";
      this.label12.TextAlign = ContentAlignment.MiddleRight;
      this.btnStopSendMinoConnectTestPacket.Location = new Point(13, 107);
      this.btnStopSendMinoConnectTestPacket.Name = "btnStopSendMinoConnectTestPacket";
      this.btnStopSendMinoConnectTestPacket.Size = new Size(225, 23);
      this.btnStopSendMinoConnectTestPacket.TabIndex = 25;
      this.btnStopSendMinoConnectTestPacket.Text = "Stop";
      this.btnStopSendMinoConnectTestPacket.UseVisualStyleBackColor = true;
      this.btnStopSendMinoConnectTestPacket.Click += new System.EventHandler(this.btnStopSendMinoConnectTestPacket_Click);
      this.label13.Location = new Point(40, 18);
      this.label13.Name = "label13";
      this.label13.Size = new Size(42, 15);
      this.label13.TabIndex = 24;
      this.label13.Text = "Mode:";
      this.label13.TextAlign = ContentAlignment.MiddleRight;
      this.cboxRadioModeForMiConTestPacket.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxRadioModeForMiConTestPacket.FormattingEnabled = true;
      this.cboxRadioModeForMiConTestPacket.Location = new Point(88, 17);
      this.cboxRadioModeForMiConTestPacket.Name = "cboxRadioModeForMiConTestPacket";
      this.cboxRadioModeForMiConTestPacket.Size = new Size(121, 21);
      this.cboxRadioModeForMiConTestPacket.TabIndex = 23;
      this.btnStartSendMinoConnectTestPacket.Location = new Point(13, 70);
      this.btnStartSendMinoConnectTestPacket.Name = "btnStartSendMinoConnectTestPacket";
      this.btnStartSendMinoConnectTestPacket.Size = new Size(225, 23);
      this.btnStartSendMinoConnectTestPacket.TabIndex = 0;
      this.btnStartSendMinoConnectTestPacket.Text = "Start send MinoConnect test packet";
      this.btnStartSendMinoConnectTestPacket.UseVisualStyleBackColor = true;
      this.btnStartSendMinoConnectTestPacket.Click += new System.EventHandler(this.btnStartSendMinoConnectTestPacket_Click);
      this.groupBox5.Controls.Add((Control) this.label9);
      this.groupBox5.Controls.Add((Control) this.label8);
      this.groupBox5.Controls.Add((Control) this.txtTimeout);
      this.groupBox5.Controls.Add((Control) this.cboxRadioMode);
      this.groupBox5.Controls.Add((Control) this.label7);
      this.groupBox5.Controls.Add((Control) this.txtSerialnumber);
      this.groupBox5.Controls.Add((Control) this.btnReceiveRadioPacket);
      this.groupBox5.Controls.Add((Control) this.label6);
      this.groupBox5.Location = new Point(11, 44);
      this.groupBox5.Name = "groupBox5";
      this.groupBox5.Size = new Size(252, 148);
      this.groupBox5.TabIndex = 28;
      this.groupBox5.TabStop = false;
      this.groupBox5.Text = "Receive";
      this.label9.Location = new Point(215, 71);
      this.label9.Name = "label9";
      this.label9.Size = new Size(49, 15);
      this.label9.TabIndex = 29;
      this.label9.Text = "sec";
      this.label9.TextAlign = ContentAlignment.MiddleLeft;
      this.label8.Location = new Point(40, 18);
      this.label8.Name = "label8";
      this.label8.Size = new Size(42, 15);
      this.label8.TabIndex = 24;
      this.label8.Text = "Mode:";
      this.label8.TextAlign = ContentAlignment.MiddleRight;
      this.txtTimeout.Location = new Point(88, 70);
      this.txtTimeout.Maximum = new Decimal(new int[4]
      {
        10000,
        0,
        0,
        0
      });
      this.txtTimeout.Name = "txtTimeout";
      this.txtTimeout.Size = new Size(121, 20);
      this.txtTimeout.TabIndex = 28;
      this.txtTimeout.Value = new Decimal(new int[4]
      {
        180,
        0,
        0,
        0
      });
      this.cboxRadioMode.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxRadioMode.FormattingEnabled = true;
      this.cboxRadioMode.Location = new Point(88, 17);
      this.cboxRadioMode.Name = "cboxRadioMode";
      this.cboxRadioMode.Size = new Size(121, 21);
      this.cboxRadioMode.TabIndex = 23;
      this.label7.Location = new Point(7, 70);
      this.label7.Name = "label7";
      this.label7.Size = new Size(75, 15);
      this.label7.TabIndex = 27;
      this.label7.Text = "Timeout:";
      this.label7.TextAlign = ContentAlignment.MiddleRight;
      this.txtSerialnumber.Location = new Point(88, 44);
      this.txtSerialnumber.Maximum = new Decimal(new int[4]
      {
        99999999,
        0,
        0,
        0
      });
      this.txtSerialnumber.Name = "txtSerialnumber";
      this.txtSerialnumber.Size = new Size(120, 20);
      this.txtSerialnumber.TabIndex = 26;
      this.btnReceiveRadioPacket.Location = new Point(21, 107);
      this.btnReceiveRadioPacket.Name = "btnReceiveRadioPacket";
      this.btnReceiveRadioPacket.Size = new Size(199, 23);
      this.btnReceiveRadioPacket.TabIndex = 0;
      this.btnReceiveRadioPacket.Text = "Receive radio packet";
      this.btnReceiveRadioPacket.UseVisualStyleBackColor = true;
      this.btnReceiveRadioPacket.Click += new System.EventHandler(this.btnReceiveRadioPacket_Click);
      this.label6.Location = new Point(7, 44);
      this.label6.Name = "label6";
      this.label6.Size = new Size(75, 15);
      this.label6.TabIndex = 25;
      this.label6.Text = "Serialnumber:";
      this.label6.TextAlign = ContentAlignment.MiddleRight;
      this.tabWiredMbus.Controls.Add((Control) this.ckboxClearQueue);
      this.tabWiredMbus.Controls.Add((Control) this.label18);
      this.tabWiredMbus.Controls.Add((Control) this.btnWritePulseoutQueue);
      this.tabWiredMbus.Controls.Add((Control) this.txtPulseoutQueue);
      this.tabWiredMbus.Location = new Point(4, 22);
      this.tabWiredMbus.Name = "tabWiredMbus";
      this.tabWiredMbus.Size = new Size(761, 388);
      this.tabWiredMbus.TabIndex = 3;
      this.tabWiredMbus.Text = "wired M-Bus";
      this.tabWiredMbus.UseVisualStyleBackColor = true;
      this.ckboxClearQueue.AutoSize = true;
      this.ckboxClearQueue.Location = new Point(181, 20);
      this.ckboxClearQueue.Name = "ckboxClearQueue";
      this.ckboxClearQueue.Size = new Size(83, 17);
      this.ckboxClearQueue.TabIndex = 62;
      this.ckboxClearQueue.Text = "Clear queue";
      this.ckboxClearQueue.UseVisualStyleBackColor = true;
      this.label18.Location = new Point(14, 20);
      this.label18.Name = "label18";
      this.label18.Size = new Size(97, 15);
      this.label18.TabIndex = 60;
      this.label18.Text = "Pulseout queue:";
      this.label18.TextAlign = ContentAlignment.MiddleRight;
      this.btnWritePulseoutQueue.Location = new Point(270, 16);
      this.btnWritePulseoutQueue.Name = "btnWritePulseoutQueue";
      this.btnWritePulseoutQueue.Size = new Size(70, 23);
      this.btnWritePulseoutQueue.TabIndex = 59;
      this.btnWritePulseoutQueue.Text = "Process";
      this.btnWritePulseoutQueue.UseVisualStyleBackColor = true;
      this.btnWritePulseoutQueue.Click += new System.EventHandler(this.btnWritePulseoutQueue_Click);
      this.txtPulseoutQueue.Location = new Point(115, 19);
      this.txtPulseoutQueue.Maximum = new Decimal(new int[4]
      {
        (int) short.MaxValue,
        0,
        0,
        0
      });
      this.txtPulseoutQueue.Minimum = new Decimal(new int[4]
      {
        32768,
        0,
        0,
        int.MinValue
      });
      this.txtPulseoutQueue.Name = "txtPulseoutQueue";
      this.txtPulseoutQueue.Size = new Size(60, 20);
      this.txtPulseoutQueue.TabIndex = 61;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(780, 562);
      this.Controls.Add((Control) this.tabs);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.statusStrip);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (TestCommand);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Test commands";
      this.FormClosing += new FormClosingEventHandler(this.TestCommand_FormClosing);
      this.Load += new System.EventHandler(this.TestCommand_Load);
      this.statusStrip.ResumeLayout(false);
      this.statusStrip.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.tabGenerally.ResumeLayout(false);
      this.groupBox8.ResumeLayout(false);
      this.groupBox8.PerformLayout();
      this.txtGeneration.EndInit();
      this.txtAddress.EndInit();
      this.txtSerial.EndInit();
      this.groupBox10.ResumeLayout(false);
      this.txtMeterValue.EndInit();
      this.groupBox6.ResumeLayout(false);
      this.txtTimeoutRadioReceive.EndInit();
      this.groupBox7.ResumeLayout(false);
      this.txtRadioTimeout.EndInit();
      this.txtRadioFrequencyOffset.EndInit();
      this.groupBox3.ResumeLayout(false);
      this.tabs.ResumeLayout(false);
      this.tabMemory.ResumeLayout(false);
      this.groupBox4.ResumeLayout(false);
      this.groupBox4.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.tabMinoConnect.ResumeLayout(false);
      this.tabMinoConnect.PerformLayout();
      this.groupBox9.ResumeLayout(false);
      this.txtSendTestPacketPower.EndInit();
      this.groupBox5.ResumeLayout(false);
      this.txtTimeout.EndInit();
      this.txtSerialnumber.EndInit();
      this.tabWiredMbus.ResumeLayout(false);
      this.tabWiredMbus.PerformLayout();
      this.txtPulseoutQueue.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private delegate bool ActionSimpleMethod();
  }
}
