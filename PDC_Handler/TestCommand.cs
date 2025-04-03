// Decompiled with JetBrains decompiler
// Type: PDC_Handler.TestCommand
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using AsyncCom;
using DeviceCollector;
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
namespace PDC_Handler
{
  public class TestCommand : Form
  {
    private PDC_HandlerFunctions pdcHandler;
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
    private NumericUpDown txtSerialnumberInputA;
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
    private Button btnSetCurrentTime;
    private TabPage tabChannels;
    private Button btnReadMeterValueA;
    private Label label16;
    private NumericUpDown txtMeterValueA;
    private Button btnWriteMeterValueA;
    private TabPage tabConfig;
    private Label label19;
    private Label label15;
    private Button btnRadioFlagsClear;
    private Button btnConfigFlagsClear;
    private Button btnRadioFlagsSet;
    private Button btnConfigFlagsSet;
    private Button btnRadioFlagsWrite;
    private Button btnConfigFlagsWrite;
    private Button btnRadioFlagsRead;
    private Button btnConfigFlagsRead;
    private TextBox txtRadioFlags;
    private TextBox txtConfigFlags;
    private Button btnSerialWrite;
    private Button btnSerialRead;
    private TextBox txtSerialABC;
    private Label label21;
    private ComboBox cboxConfigChannel;
    private Label label25;
    private Button btnMBusIdWrite;
    private Button btnManIdRead;
    private TextBox txtManIdABC;
    private Label label24;
    private Button btnMBusTypeWrite;
    private Button btnMBusTypeRead;
    private TextBox txtMBusTypeABC;
    private Label label23;
    private Button btnMBusVersionWrite;
    private Button btnMBusVersionRead;
    private TextBox txtMBusVersionABC;
    private Label label22;
    private Button btnMBusAddressWrite;
    private Button btnMBusAddressRead;
    private TextBox txtMBusAddressABC;
    private Label label20;
    private NumericUpDown txtKeyMonth;
    private NumericUpDown txtKeyDay;
    private Label label26;
    private Button btnKeydateWrite;
    private Button btnKeydateRead;
    private NumericUpDown txtRadioList;
    private Label label27;
    private Button btnRadioListWrite;
    private Button btnRadioListRead;
    private Button btnRadioListQuery;
    private Button btnResetDelivery;
    private Button btnStatusFlagsARead;
    private Label label28;
    private Button btnStatusFlagsAClear;
    private TextBox txtStatusFlagsA;
    private Label label31;
    private Button btnObisWrite;
    private Button btnObisRead;
    private TextBox txtObisCode;
    private NumericUpDown txtExponentA;
    private NumericUpDown txtMantissaA;
    private Button btnExponentARead;
    private Label label33;
    private Button btnExponentAWrite;
    private Button btnMantissaARead;
    private Label label32;
    private Button btnMantissaAWrite;
    private TextBox txtVIFA;
    private Label label37;
    private NumericUpDown txtPulseOn;
    private NumericUpDown txtPulsePeriod;
    private Label label38;
    private Button btnPulseWrite;
    private Button btnPulseRead;
    private NumericUpDown txtLower;
    private Button btnLeakRead;
    private Label label42;
    private Button btnLeakWrite;
    private NumericUpDown txtUpper;
    private Label label41;
    private NumericUpDown txtUnleak;
    private Label label40;
    private NumericUpDown txtLeak;
    private Label label39;
    private NumericUpDown txtUndersizeLimit;
    private Button btnUndersizeRead;
    private Label label36;
    private Button btnUndersizeWrite;
    private NumericUpDown txtOversizeLimit;
    private Button btnOversizeRead;
    private Label label35;
    private Button btnOversizeWrite;
    private NumericUpDown txtBurstDiff;
    private Button btnBurstRead;
    private Label label30;
    private Button btnBurstWrite;
    private NumericUpDown txtBlock;
    private Button btnBlockRead;
    private Label label29;
    private Button btnBlockWrite;
    private Label label18;
    private ComboBox comboChannel;
    private Button btnVIFARead;
    private Button btnVIFAWrite;
    private NumericUpDown txtBurstLimit;
    private Label label43;
    private NumericUpDown txtUndersizeDiff;
    private Label label45;
    private NumericUpDown txtOversizeDiff;
    private Label label44;
    private TabPage tabMBus;
    private Button btnMBusStatusQuery;
    private Button btnDepassNow;
    private NumericUpDown txtDepassPeriod;
    private NumericUpDown txtDepassTimeout;
    private Label label46;
    private Button btnDepassWrite;
    private Button btnDepassRead;

    public TestCommand()
    {
      this.InitializeComponent();
      this.stopwatch = new Stopwatch();
      this.isCanceled = false;
      this.ResetUI();
    }

    public TestCommand(PDC_HandlerFunctions MyFunctions)
      : this()
    {
      this.pdcHandler = MyFunctions;
    }

    internal static void ShowDialog(Form owner, PDC_HandlerFunctions pdcHandler)
    {
      if (pdcHandler == null)
        return;
      using (TestCommand testCommand = new TestCommand())
      {
        testCommand.pdcHandler = pdcHandler;
        int num = (int) testCommand.ShowDialog((IWin32Window) owner);
      }
      ZR_ClassLibMessages.ClearErrors();
    }

    private void TestCommand_Load(object sender, EventArgs e)
    {
      this.pdcHandler.MyDeviceCollector.OnMessage += new EventHandler<GMM_EventArgs>(this.MyDeviceCollector_OnMessage);
      string[] namesOfEnum = ZR_ClassLibrary.Util.GetNamesOfEnum(typeof (RadioMode));
      this.cboxRadioMode.DataSource = (object) namesOfEnum;
      this.cboxRadioModeForTest.DataSource = (object) namesOfEnum;
      this.cboxRadioModeForMiConTestPacket.DataSource = (object) namesOfEnum;
      this.txtTimeout.Value = 180M;
    }

    private void TestCommand_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.pdcHandler.MyDeviceCollector.OnMessage -= new EventHandler<GMM_EventArgs>(this.MyDeviceCollector_OnMessage);
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
          this.pdcHandler.MyDeviceCollector.BreakRequest = true;
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
      if (this.settingsForSecondMinoConnect == null)
        this.settingsForSecondMinoConnect = GMMSettings.Default_Radio2_MinoConnect;
      if (TestCommand.secondDeviceCollector == null)
      {
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
      PDC_Meter meter = this.pdcHandler.Meter;
      if (meter != null)
      {
        try
        {
          ushort? transmitInterval = meter.GetRadioTransmitInterval();
          this.txtTimeout.Value = (Decimal) (transmitInterval.HasValue ? (int) transmitInterval.Value : 180);
          uint? serialMbusInputA = meter.GetSerialMBusInputA();
          this.txtSerialnumberInputA.Value = (Decimal) (serialMbusInputA.HasValue ? serialMbusInputA.Value : 0U);
          RadioMode? radioMode = meter.GetRadioMode();
          this.cboxRadioMode.SelectedItem = radioMode.HasValue ? (object) radioMode.Value.ToString() : (object) RadioMode.Radio2.ToString();
          this.cboxRadioModeForTest.SelectedItem = radioMode.HasValue ? (object) radioMode.Value.ToString() : (object) RadioMode.Radio2.ToString();
          this.cboxRadioModeForMiConTestPacket.SelectedItem = radioMode.HasValue ? (object) radioMode.Value.ToString() : (object) RadioMode.Radio2.ToString();
          short? frequencyOffset = meter.GetFrequencyOffset();
          this.txtRadioFrequencyOffset.Value = (Decimal) (frequencyOffset.HasValue ? (int) frequencyOffset.Value : 0);
        }
        catch
        {
        }
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
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.pdcHandler.ResetDevice));
    }

    private void btnEventLogClear_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.pdcHandler.EventLogClear));
    }

    private void btnSystemLogClear_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.pdcHandler.SystemLogClear));
    }

    private void btnRunRamBackup_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.pdcHandler.RunRAMBackup));
    }

    private void btnPulseDisable_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.pdcHandler.PulseDisable));
    }

    private void btnPulseEnable_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.pdcHandler.PulseEnable));
    }

    private void btnRadioDisable_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.pdcHandler.RadioDisable));
    }

    private void btnRadioNormal_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.pdcHandler.RadioNormal));
    }

    private void btnSendAck_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.pdcHandler.MyDeviceCollector.EDCHandler.StopVolumeMonitor_SendE5));
    }

    private void btnSND_NKE_IrDaOff_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod(new TestCommand.ActionSimpleMethod(this.pdcHandler.SendSND_NKE));
    }

    private void btnReadVersion_Click(object sender, EventArgs e)
    {
      this.ResetUI();
      DeviceVersion deviceVersion = this.pdcHandler.ReadVersion();
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
        if (this.pdcHandler.ReadMemory(uint16_1, (int) uint16_2 - (int) uint16_1, out buffer))
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = ZR_ClassLibrary.Util.ByteArrayToHexString(buffer);
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
        if (this.pdcHandler.StartRadioReceiver())
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          Thread.Sleep(1000);
          byte[] buffer;
          if (this.pdcHandler.MyDeviceCollector.AsyncCom.TryReceiveBlock(out buffer) && buffer != null)
            this.txtOutput.Text = ZR_ClassLibrary.Util.ByteArrayToHexString(buffer);
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
        if (this.pdcHandler.RadioOOK((RadioMode) Enum.Parse(typeof (RadioMode), this.cboxRadioModeForTest.SelectedItem.ToString(), true), Convert.ToInt16(this.txtRadioFrequencyOffset.Value), Convert.ToUInt16(this.txtRadioTimeout.Value)))
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
        if (this.pdcHandler.RadioPN9((RadioMode) Enum.Parse(typeof (RadioMode), this.cboxRadioModeForTest.SelectedItem.ToString(), true), Convert.ToInt16(this.txtRadioFrequencyOffset.Value), Convert.ToUInt16(this.txtRadioTimeout.Value)))
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
        if (this.pdcHandler.RadioReceive(out packet, out buffer, out rssi_dBm, out lqi, (uint) this.txtTimeoutRadioReceive.Value))
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "RSSI: " + rssi_dBm.ToString() + Environment.NewLine;
          TextBox txtOutput1 = this.txtOutput;
          txtOutput1.Text = txtOutput1.Text + "LQI: 0x" + lqi.ToString("X2") + Environment.NewLine;
          TextBox txtOutput2 = this.txtOutput;
          txtOutput2.Text = txtOutput2.Text + "Buffer: " + ZR_ClassLibrary.Util.ByteArrayToHexString(buffer) + Environment.NewLine;
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
        DateTime? nullable = this.pdcHandler.ReadSystemTime();
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
        if (this.pdcHandler.WriteSystemTime(this.txtSystemTime.Value))
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
        int funkId = (int) this.txtSerialnumberInputA.Value;
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
              this.txtOutput.Text = "Failed! Timeout";
            else
              this.txtOutput.Text = "Failed! Error: " + errorDescription;
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
        if (this.pdcHandler.WriteMeterValue((byte) this.comboChannel.SelectedIndex, Convert.ToUInt32(this.txtMeterValueA.Value)))
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
        }
        else
          this.txtOutput.Text = "Failed! " + ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        this.lblStatus.Text = string.Empty;
        this.lblProgress.Text = string.Empty;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnReadMeterValueA_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        int? nullable = this.pdcHandler.ReadMeterValue((byte) this.comboChannel.SelectedIndex);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtMeterValueA.Value = (Decimal) nullable.Value;
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

    private void btnConfigFlagsRead_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        ushort? nullable = this.pdcHandler.ReadConfigFlags();
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtConfigFlags.Text = nullable.Value.ToString("X4");
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

    private void btnConfigFlagsWrite_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        ushort? nullable = this.pdcHandler.WriteConfigFlags(Convert.ToUInt16(this.txtConfigFlags.Text, 16));
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtConfigFlags.Text = nullable.Value.ToString("X4");
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

    private void btnConfigFlagsSet_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        ushort? nullable = this.pdcHandler.ModifyConfigFlags(Convert.ToUInt16(this.txtConfigFlags.Text, 16), (ushort) 0);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtConfigFlags.Text = nullable.Value.ToString("X4");
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

    private void btnConfigFlagsClear_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        ushort? nullable = this.pdcHandler.ModifyConfigFlags((ushort) 0, Convert.ToUInt16(this.txtConfigFlags.Text, 16));
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtConfigFlags.Text = nullable.Value.ToString("X4");
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

    private void btnRadioFlagsRead_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        byte? nullable = this.pdcHandler.ReadRadioFlags();
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtRadioFlags.Text = nullable.Value.ToString("X2");
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

    private void btnRadioFlagsWrite_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        byte? nullable = this.pdcHandler.WriteRadioFlags(Convert.ToByte(this.txtRadioFlags.Text, 16));
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtRadioFlags.Text = nullable.Value.ToString("X2");
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

    private void btnRadioFlagsSet_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        byte? nullable = this.pdcHandler.ModifyRadioFlags(Convert.ToByte(this.txtRadioFlags.Text, 16), (byte) 0);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtRadioFlags.Text = nullable.Value.ToString("X2");
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

    private void btnRadioFlagsClear_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        byte? nullable = this.pdcHandler.ModifyRadioFlags((byte) 0, Convert.ToByte(this.txtRadioFlags.Text, 16));
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtRadioFlags.Text = nullable.Value.ToString("X2");
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

    private void btnSerialRead_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        uint? nullable = this.pdcHandler.ReadSerialNumber((byte) this.cboxConfigChannel.SelectedIndex);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtSerialABC.Text = nullable.Value.ToString("X8");
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

    private void btnSerialWrite_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        uint? nullable = this.pdcHandler.WriteSerialNumber((byte) this.cboxConfigChannel.SelectedIndex, Convert.ToUInt32(this.txtSerialABC.Text, 16));
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtSerialABC.Text = nullable.Value.ToString("X8");
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

    private void btnMBusAddressRead_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        byte? nullable = this.pdcHandler.ReadMBusAddress((byte) this.cboxConfigChannel.SelectedIndex);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtMBusAddressABC.Text = nullable.Value.ToString("X4");
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

    private void btnMBusAddressWrite_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        byte? nullable = this.pdcHandler.WriteMBusAddress((byte) this.cboxConfigChannel.SelectedIndex, Convert.ToByte(this.txtMBusAddressABC.Text, 16));
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtMBusAddressABC.Text = nullable.Value.ToString("X4");
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

    private void btnMBusVersionRead_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        byte? nullable = this.pdcHandler.ReadMBusVersion((byte) this.cboxConfigChannel.SelectedIndex);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtMBusVersionABC.Text = nullable.Value.ToString("X4");
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

    private void btnMBusVersionWrite_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        byte? nullable = this.pdcHandler.WriteMBusVersion((byte) this.cboxConfigChannel.SelectedIndex, Convert.ToByte(this.txtMBusVersionABC.Text, 16));
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtMBusVersionABC.Text = nullable.Value.ToString("X4");
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

    private void btnMBusTypeRead_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        byte? nullable = this.pdcHandler.ReadMBusType((byte) this.cboxConfigChannel.SelectedIndex);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtMBusTypeABC.Text = nullable.Value.ToString("X2");
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

    private void btnMBusTypeWrite_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        byte? nullable = this.pdcHandler.WriteMBusType((byte) this.cboxConfigChannel.SelectedIndex, Convert.ToByte(this.txtMBusTypeABC.Text, 16));
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtMBusTypeABC.Text = nullable.Value.ToString("X2");
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

    private void btnManIdRead_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        ushort? nullable = this.pdcHandler.ReadMBusManId((byte) this.cboxConfigChannel.SelectedIndex);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtManIdABC.Text = nullable.Value.ToString("X4");
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

    private void btnMBusIdWrite_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        ushort? nullable = this.pdcHandler.WriteMBusManId((byte) this.cboxConfigChannel.SelectedIndex, Convert.ToUInt16(this.txtManIdABC.Text, 16));
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtManIdABC.Text = nullable.Value.ToString("X4");
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

    private void btnObisRead_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        byte? nullable = this.pdcHandler.ReadObisCode((byte) this.cboxConfigChannel.SelectedIndex);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtObisCode.Text = nullable.Value.ToString("X2");
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

    private void btnObisWrite_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        byte? nullable = this.pdcHandler.WriteObisCode((byte) this.cboxConfigChannel.SelectedIndex, Convert.ToByte(this.txtObisCode.Text, 16));
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtObisCode.Text = nullable.Value.ToString("X2");
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

    private void btnKeydateRead_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        ushort? nullable = this.pdcHandler.ReadKeydate();
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          byte num1 = (byte) ((uint) nullable.Value >> 8);
          byte num2 = (byte) ((uint) nullable.Value & (uint) byte.MaxValue);
          this.txtKeyDay.Text = num1.ToString("X2");
          this.txtKeyMonth.Text = num2.ToString("X2");
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

    private void btnKeydateWrite_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        ushort? nullable = this.pdcHandler.WriteKeydate(Convert.ToByte(this.txtKeyMonth.Text, 16), Convert.ToByte(this.txtKeyDay.Text, 16));
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          byte num1 = (byte) ((uint) nullable.Value >> 8);
          byte num2 = (byte) ((uint) nullable.Value & (uint) byte.MaxValue);
          this.txtKeyDay.Text = num1.ToString("X2");
          this.txtKeyMonth.Text = num2.ToString("X2");
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

    private void btnRadioListRead_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        byte? nullable = this.pdcHandler.ReadRadioList();
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtRadioList.Text = nullable.Value.ToString();
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

    private void btnRadioListWrite_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        byte? nullable = this.pdcHandler.WriteRadioList(Convert.ToByte(this.txtRadioList.Text));
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtRadioList.Text = nullable.Value.ToString();
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

    private void btnRadioListQuery_Click(object sender, EventArgs e)
    {
      uint? nullable1 = this.pdcHandler.QueryRadioList();
      if (nullable1.HasValue)
      {
        uint? nullable2 = nullable1;
        uint maxValue1 = (uint) byte.MaxValue;
        byte num1 = (byte) (nullable2.HasValue ? new uint?(nullable2.GetValueOrDefault() & maxValue1) : new uint?()).Value;
        uint? nullable3 = nullable1;
        nullable2 = nullable3.HasValue ? new uint?(nullable3.GetValueOrDefault() >> 8) : new uint?();
        uint maxValue2 = (uint) byte.MaxValue;
        uint? nullable4;
        if (!nullable2.HasValue)
        {
          nullable3 = new uint?();
          nullable4 = nullable3;
        }
        else
          nullable4 = new uint?(nullable2.GetValueOrDefault() & maxValue2);
        nullable3 = nullable4;
        byte num2 = (byte) nullable3.Value;
        nullable3 = nullable1;
        nullable2 = nullable3.HasValue ? new uint?(nullable3.GetValueOrDefault() >> 16) : new uint?();
        uint maxValue3 = (uint) byte.MaxValue;
        uint? nullable5;
        if (!nullable2.HasValue)
        {
          nullable3 = new uint?();
          nullable5 = nullable3;
        }
        else
          nullable5 = new uint?(nullable2.GetValueOrDefault() & maxValue3);
        nullable3 = nullable5;
        byte num3 = (byte) nullable3.Value;
        this.txtOutput.Text = "List 1:\r\n";
        TextBox txtOutput1 = this.txtOutput;
        txtOutput1.Text = txtOutput1.Text + "  Install: " + (((int) num1 & 8) == 0 ? "No" : "Yes") + "\r\n";
        TextBox txtOutput2 = this.txtOutput;
        txtOutput2.Text = txtOutput2.Text + "  A: " + (((int) num1 & 1) == 0 ? "No" : "Yes") + "\r\n";
        TextBox txtOutput3 = this.txtOutput;
        txtOutput3.Text = txtOutput3.Text + "  B: " + (((int) num1 & 2) == 0 ? "No" : "Yes") + "\r\n";
        TextBox txtOutput4 = this.txtOutput;
        txtOutput4.Text = txtOutput4.Text + "  C: " + (((int) num1 & 4) == 0 ? "No" : "Yes") + "\r\n";
        this.txtOutput.Text += "List 2:\r\n";
        TextBox txtOutput5 = this.txtOutput;
        txtOutput5.Text = txtOutput5.Text + "  Install: " + (((int) num2 & 8) == 0 ? "No" : "Yes") + "\r\n";
        TextBox txtOutput6 = this.txtOutput;
        txtOutput6.Text = txtOutput6.Text + "  A: " + (((int) num2 & 1) == 0 ? "No" : "Yes") + "\r\n";
        TextBox txtOutput7 = this.txtOutput;
        txtOutput7.Text = txtOutput7.Text + "  B: " + (((int) num2 & 2) == 0 ? "No" : "Yes") + "\r\n";
        TextBox txtOutput8 = this.txtOutput;
        txtOutput8.Text = txtOutput8.Text + "  C: " + (((int) num2 & 4) == 0 ? "No" : "Yes") + "\r\n";
        this.txtOutput.Text += "List 3:\r\n";
        TextBox txtOutput9 = this.txtOutput;
        txtOutput9.Text = txtOutput9.Text + "  Install: " + (((int) num3 & 8) == 0 ? "No" : "Yes") + "\r\n";
        TextBox txtOutput10 = this.txtOutput;
        txtOutput10.Text = txtOutput10.Text + "  A: " + (((int) num3 & 1) == 0 ? "No" : "Yes") + "\r\n";
        TextBox txtOutput11 = this.txtOutput;
        txtOutput11.Text = txtOutput11.Text + "  B: " + (((int) num3 & 2) == 0 ? "No" : "Yes") + "\r\n";
        TextBox txtOutput12 = this.txtOutput;
        txtOutput12.Text = txtOutput12.Text + "  C: " + (((int) num3 & 4) == 0 ? "No" : "Yes") + "\r\n";
      }
      ushort? nullable6 = this.pdcHandler.ReadConfigFlags();
      if (!nullable6.HasValue)
        return;
      byte? nullable7 = this.pdcHandler.ReadRadioFlags();
      if (nullable7.HasValue)
      {
        this.txtOutput.Text += "For current configuration settings:\r\n";
        if (((int) nullable6.Value & 1024) != 0 || ((uint) nullable7.Value & 16U) > 0U)
          this.txtOutput.Text += "List 1 looks like it might be used.\r\n";
        else
          this.txtOutput.Text += "List 1 looks like it will not be used.\r\n";
        if (((int) nullable6.Value & 2048) != 0 || ((uint) nullable7.Value & 32U) > 0U)
          this.txtOutput.Text += "List 2 looks like it might be used.\r\n";
        else
          this.txtOutput.Text += "List 2 looks like it will not be used.\r\n";
        if (((int) nullable6.Value & 3072) == 0 && ((int) nullable7.Value & 48) == 0)
          this.txtOutput.Text += "List 3 looks like it will be used.\r\n";
        else
          this.txtOutput.Text += "List 3 looks like it will not be used.\r\n";
      }
    }

    private void btnResetDelivery_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        if (this.pdcHandler.ResetToDelivery())
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

    private void btnStatusFlagsARead_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        ushort? nullable = this.pdcHandler.ReadStatusFlags((byte) this.comboChannel.SelectedIndex);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtStatusFlagsA.Text = nullable.Value.ToString("X4");
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

    private void btnStatusFlagsAClear_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        ushort? nullable = this.pdcHandler.ClearStatusFlags((byte) this.comboChannel.SelectedIndex, Convert.ToUInt16(this.txtStatusFlagsA.Text, 16));
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtStatusFlagsA.Text = nullable.Value.ToString("X4");
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

    private void btnMantissaARead_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        ushort? nullable = this.pdcHandler.ReadMantissa((byte) this.comboChannel.SelectedIndex);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtMantissaA.Value = (Decimal) nullable.Value;
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

    private void btnMantissaAWrite_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        ushort? nullable = this.pdcHandler.WriteMantissa((byte) this.comboChannel.SelectedIndex, (ushort) this.txtMantissaA.Value);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtMantissaA.Value = (Decimal) nullable.Value;
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

    private void btnExponentARead_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        sbyte? nullable = this.pdcHandler.ReadExponent((byte) this.comboChannel.SelectedIndex);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtExponentA.Value = (Decimal) nullable.Value;
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

    private void btnExponentAWrite_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        sbyte? nullable = this.pdcHandler.WriteExponent((byte) this.comboChannel.SelectedIndex, (sbyte) this.txtExponentA.Value);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtExponentA.Value = (Decimal) nullable.Value;
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

    private void btnVIFARead_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        byte? nullable = this.pdcHandler.ReadVIF((byte) this.comboChannel.SelectedIndex);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtVIFA.Text = nullable.Value.ToString("X2");
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

    private void btnVIFAWrite_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        byte? nullable = this.pdcHandler.WriteVIF((byte) this.comboChannel.SelectedIndex, Convert.ToByte(this.txtVIFA.Text, 16));
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtVIFA.Text = nullable.Value.ToString("X2");
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

    private void btnPulseRead_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        uint? nullable = this.pdcHandler.ReadPulseSettings();
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtPulsePeriod.Value = (Decimal) (ushort) (nullable.Value & (uint) ushort.MaxValue);
          this.txtPulseOn.Value = (Decimal) (byte) (nullable.Value >> 16 & (uint) byte.MaxValue);
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

    private void btnPulseWrite_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        uint? nullable = this.pdcHandler.WritePulseSettings((ushort) this.txtPulsePeriod.Value, (byte) this.txtPulseOn.Value);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtPulsePeriod.Value = (Decimal) (ushort) (nullable.Value & (uint) ushort.MaxValue);
          this.txtPulseOn.Value = (Decimal) (byte) (nullable.Value >> 16 & (uint) byte.MaxValue);
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

    private void btnBlockRead_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        ushort? nullable = this.pdcHandler.ReadFlowBlock((byte) this.comboChannel.SelectedIndex);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtBlock.Value = (Decimal) nullable.Value;
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

    private void btnBlockWrite_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        ushort? nullable = this.pdcHandler.WriteFlowBlock((byte) this.comboChannel.SelectedIndex, (ushort) this.txtBlock.Value);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtBlock.Value = (Decimal) nullable.Value;
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

    private void btnBurstRead_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        uint? nullable = this.pdcHandler.ReadFlowBurst((byte) this.comboChannel.SelectedIndex);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtBurstDiff.Value = (Decimal) (nullable.Value & (uint) ushort.MaxValue);
          this.txtBurstLimit.Value = (Decimal) (nullable.Value >> 16 & (uint) ushort.MaxValue);
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

    private void btnBurstWrite_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        uint? nullable = this.pdcHandler.WriteFlowBurst((byte) this.comboChannel.SelectedIndex, (ushort) this.txtBurstDiff.Value, (ushort) this.txtBurstLimit.Value);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtBurstDiff.Value = (Decimal) (nullable.Value & (uint) ushort.MaxValue);
          this.txtBurstLimit.Value = (Decimal) (nullable.Value >> 16 & (uint) ushort.MaxValue);
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

    private void btnOversizeRead_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        uint? nullable = this.pdcHandler.ReadFlowOversize((byte) this.comboChannel.SelectedIndex);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtOversizeDiff.Value = (Decimal) (nullable.Value & (uint) ushort.MaxValue);
          this.txtOversizeLimit.Value = (Decimal) (nullable.Value >> 16 & (uint) ushort.MaxValue);
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

    private void btnOversizeWrite_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        uint? nullable = this.pdcHandler.WriteFlowOversize((byte) this.comboChannel.SelectedIndex, (ushort) this.txtOversizeDiff.Value, (ushort) this.txtOversizeLimit.Value);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtOversizeDiff.Value = (Decimal) (nullable.Value & (uint) ushort.MaxValue);
          this.txtOversizeLimit.Value = (Decimal) (nullable.Value >> 16 & (uint) ushort.MaxValue);
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

    private void btnUndersizeRead_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        uint? nullable = this.pdcHandler.ReadFlowUndersize((byte) this.comboChannel.SelectedIndex);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtUndersizeDiff.Value = (Decimal) (nullable.Value & (uint) ushort.MaxValue);
          this.txtUndersizeLimit.Value = (Decimal) (nullable.Value >> 16 & (uint) ushort.MaxValue);
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

    private void btnUndersizeWrite_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        uint? nullable = this.pdcHandler.WriteFlowUndersize((byte) this.comboChannel.SelectedIndex, (ushort) this.txtUndersizeDiff.Value, (ushort) this.txtUndersizeLimit.Value);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtUndersizeDiff.Value = (Decimal) (nullable.Value & (uint) ushort.MaxValue);
          this.txtUndersizeLimit.Value = (Decimal) (nullable.Value >> 16 & (uint) ushort.MaxValue);
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

    private void btnLeakRead_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        ulong? nullable = this.pdcHandler.ReadFlowLeak((byte) this.comboChannel.SelectedIndex);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtLeak.Value = (Decimal) (ushort) (nullable.Value & (ulong) ushort.MaxValue);
          this.txtUnleak.Value = (Decimal) (ushort) (nullable.Value >> 16 & (ulong) ushort.MaxValue);
          this.txtUpper.Value = (Decimal) (ushort) (nullable.Value >> 32 & (ulong) ushort.MaxValue);
          this.txtLower.Value = (Decimal) (ushort) (nullable.Value >> 48 & (ulong) ushort.MaxValue);
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

    private void btnLeakWrite_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        ulong? nullable = this.pdcHandler.WriteFlowLeak((byte) this.comboChannel.SelectedIndex, (ushort) this.txtLeak.Value, (ushort) this.txtUnleak.Value, (ushort) this.txtUpper.Value, (ushort) this.txtLower.Value);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtLeak.Value = (Decimal) (ushort) (nullable.Value & (ulong) ushort.MaxValue);
          this.txtUnleak.Value = (Decimal) (ushort) (nullable.Value >> 16 & (ulong) ushort.MaxValue);
          this.txtUpper.Value = (Decimal) (ushort) (nullable.Value >> 32 & (ulong) ushort.MaxValue);
          this.txtLower.Value = (Decimal) (ushort) (nullable.Value >> 48 & (ulong) ushort.MaxValue);
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

    private void btnDepassRead_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        uint? nullable = this.pdcHandler.ReadDepass();
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtDepassTimeout.Value = (Decimal) (ushort) (nullable.Value & (uint) ushort.MaxValue);
          this.txtDepassPeriod.Value = (Decimal) (ushort) (nullable.Value >> 16 & (uint) ushort.MaxValue);
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

    private void btnDepassWrite_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        uint? nullable = this.pdcHandler.WriteDepass((ushort) this.txtDepassTimeout.Value, (ushort) this.txtDepassPeriod.Value);
        if (nullable.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          this.txtDepassTimeout.Value = (Decimal) (ushort) (nullable.Value & (uint) ushort.MaxValue);
          this.txtDepassPeriod.Value = (Decimal) (ushort) (nullable.Value >> 16 & (uint) ushort.MaxValue);
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

    private void btnDepassNow_Click(object sender, EventArgs e) => this.pdcHandler.Depassivate();

    private void btnMBusStatusQuery_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResetUI();
        byte? nullable1 = this.pdcHandler.QueryMBusState();
        if (nullable1.HasValue)
        {
          this.stopwatch.Stop();
          this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
          this.txtOutput.Text = "OK";
          byte? nullable2 = nullable1;
          int? nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault() & 8) : new int?();
          int num1 = 0;
          if (!(nullable3.GetValueOrDefault() == num1 & nullable3.HasValue))
            this.txtOutput.Text += "\n\tMBus UART acquired.";
          nullable2 = nullable1;
          nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault() & 4) : new int?();
          int num2 = 0;
          if (!(nullable3.GetValueOrDefault() == num2 & nullable3.HasValue))
            this.txtOutput.Text += "\n\tMBus powered.";
          nullable2 = nullable1;
          nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault() & 2) : new int?();
          int num3 = 0;
          if (!(nullable3.GetValueOrDefault() == num3 & nullable3.HasValue))
            this.txtOutput.Text += "\n\tMBus hardware missing.";
          nullable2 = nullable1;
          nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault() & 1) : new int?();
          int num4 = 0;
          if (!(nullable3.GetValueOrDefault() == num4 & nullable3.HasValue))
            this.txtOutput.Text += "\n\tMBus harware powerfail.";
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
      this.btnResetDelivery = new Button();
      this.btnSystemLogClear = new Button();
      this.btnEventLogClear = new Button();
      this.btnSND_NKE_IrDaOff = new Button();
      this.btnStartRadioReceiver = new Button();
      this.btnRadioNormal = new Button();
      this.groupBox6 = new GroupBox();
      this.label11 = new Label();
      this.txtTimeoutRadioReceive = new NumericUpDown();
      this.btnRadioReceive = new Button();
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
      this.tabChannels = new TabPage();
      this.txtUndersizeDiff = new NumericUpDown();
      this.label45 = new Label();
      this.txtOversizeDiff = new NumericUpDown();
      this.label44 = new Label();
      this.txtBurstLimit = new NumericUpDown();
      this.label43 = new Label();
      this.txtLower = new NumericUpDown();
      this.btnLeakRead = new Button();
      this.label42 = new Label();
      this.btnLeakWrite = new Button();
      this.txtUpper = new NumericUpDown();
      this.label41 = new Label();
      this.txtUnleak = new NumericUpDown();
      this.label40 = new Label();
      this.txtLeak = new NumericUpDown();
      this.label39 = new Label();
      this.txtUndersizeLimit = new NumericUpDown();
      this.btnUndersizeRead = new Button();
      this.label36 = new Label();
      this.btnUndersizeWrite = new Button();
      this.txtOversizeLimit = new NumericUpDown();
      this.btnOversizeRead = new Button();
      this.label35 = new Label();
      this.btnOversizeWrite = new Button();
      this.txtBurstDiff = new NumericUpDown();
      this.btnBurstRead = new Button();
      this.label30 = new Label();
      this.btnBurstWrite = new Button();
      this.txtBlock = new NumericUpDown();
      this.btnBlockRead = new Button();
      this.label29 = new Label();
      this.btnBlockWrite = new Button();
      this.label18 = new Label();
      this.comboChannel = new ComboBox();
      this.txtVIFA = new TextBox();
      this.btnVIFARead = new Button();
      this.label37 = new Label();
      this.btnVIFAWrite = new Button();
      this.txtExponentA = new NumericUpDown();
      this.txtMantissaA = new NumericUpDown();
      this.btnExponentARead = new Button();
      this.label33 = new Label();
      this.btnExponentAWrite = new Button();
      this.btnMantissaARead = new Button();
      this.label32 = new Label();
      this.btnMantissaAWrite = new Button();
      this.txtStatusFlagsA = new TextBox();
      this.btnStatusFlagsARead = new Button();
      this.label28 = new Label();
      this.btnStatusFlagsAClear = new Button();
      this.btnReadMeterValueA = new Button();
      this.label16 = new Label();
      this.txtMeterValueA = new NumericUpDown();
      this.btnWriteMeterValueA = new Button();
      this.tabConfig = new TabPage();
      this.txtPulseOn = new NumericUpDown();
      this.txtPulsePeriod = new NumericUpDown();
      this.label38 = new Label();
      this.btnPulseWrite = new Button();
      this.btnPulseRead = new Button();
      this.label31 = new Label();
      this.btnObisWrite = new Button();
      this.btnObisRead = new Button();
      this.txtObisCode = new TextBox();
      this.btnRadioListQuery = new Button();
      this.txtRadioList = new NumericUpDown();
      this.label27 = new Label();
      this.btnRadioListWrite = new Button();
      this.btnRadioListRead = new Button();
      this.txtKeyMonth = new NumericUpDown();
      this.txtKeyDay = new NumericUpDown();
      this.label26 = new Label();
      this.btnKeydateWrite = new Button();
      this.btnKeydateRead = new Button();
      this.label25 = new Label();
      this.btnMBusIdWrite = new Button();
      this.btnManIdRead = new Button();
      this.txtManIdABC = new TextBox();
      this.label24 = new Label();
      this.btnMBusTypeWrite = new Button();
      this.btnMBusTypeRead = new Button();
      this.txtMBusTypeABC = new TextBox();
      this.label23 = new Label();
      this.btnMBusVersionWrite = new Button();
      this.btnMBusVersionRead = new Button();
      this.txtMBusVersionABC = new TextBox();
      this.label22 = new Label();
      this.btnMBusAddressWrite = new Button();
      this.btnMBusAddressRead = new Button();
      this.txtMBusAddressABC = new TextBox();
      this.label20 = new Label();
      this.cboxConfigChannel = new ComboBox();
      this.label21 = new Label();
      this.btnSerialWrite = new Button();
      this.btnSerialRead = new Button();
      this.txtSerialABC = new TextBox();
      this.btnRadioFlagsClear = new Button();
      this.btnConfigFlagsClear = new Button();
      this.btnRadioFlagsSet = new Button();
      this.btnConfigFlagsSet = new Button();
      this.btnRadioFlagsWrite = new Button();
      this.btnConfigFlagsWrite = new Button();
      this.btnRadioFlagsRead = new Button();
      this.btnConfigFlagsRead = new Button();
      this.txtRadioFlags = new TextBox();
      this.txtConfigFlags = new TextBox();
      this.label19 = new Label();
      this.label15 = new Label();
      this.tabMBus = new TabPage();
      this.btnMBusStatusQuery = new Button();
      this.btnDepassNow = new Button();
      this.txtDepassPeriod = new NumericUpDown();
      this.txtDepassTimeout = new NumericUpDown();
      this.label46 = new Label();
      this.btnDepassWrite = new Button();
      this.btnDepassRead = new Button();
      this.tabMemory = new TabPage();
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
      this.txtSerialnumberInputA = new NumericUpDown();
      this.btnReceiveRadioPacket = new Button();
      this.label6 = new Label();
      this.statusStrip.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.tabGenerally.SuspendLayout();
      this.groupBox6.SuspendLayout();
      this.txtTimeoutRadioReceive.BeginInit();
      this.groupBox7.SuspendLayout();
      this.txtRadioTimeout.BeginInit();
      this.txtRadioFrequencyOffset.BeginInit();
      this.groupBox3.SuspendLayout();
      this.tabs.SuspendLayout();
      this.tabChannels.SuspendLayout();
      this.txtUndersizeDiff.BeginInit();
      this.txtOversizeDiff.BeginInit();
      this.txtBurstLimit.BeginInit();
      this.txtLower.BeginInit();
      this.txtUpper.BeginInit();
      this.txtUnleak.BeginInit();
      this.txtLeak.BeginInit();
      this.txtUndersizeLimit.BeginInit();
      this.txtOversizeLimit.BeginInit();
      this.txtBurstDiff.BeginInit();
      this.txtBlock.BeginInit();
      this.txtExponentA.BeginInit();
      this.txtMantissaA.BeginInit();
      this.txtMeterValueA.BeginInit();
      this.tabConfig.SuspendLayout();
      this.txtPulseOn.BeginInit();
      this.txtPulsePeriod.BeginInit();
      this.txtRadioList.BeginInit();
      this.txtKeyMonth.BeginInit();
      this.txtKeyDay.BeginInit();
      this.tabMBus.SuspendLayout();
      this.txtDepassPeriod.BeginInit();
      this.txtDepassTimeout.BeginInit();
      this.tabMemory.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.tabMinoConnect.SuspendLayout();
      this.groupBox9.SuspendLayout();
      this.txtSendTestPacketPower.BeginInit();
      this.groupBox5.SuspendLayout();
      this.txtTimeout.BeginInit();
      this.txtSerialnumberInputA.BeginInit();
      this.SuspendLayout();
      this.txtOutput.Dock = DockStyle.Fill;
      this.txtOutput.Font = new Font("Consolas", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtOutput.Location = new Point(3, 16);
      this.txtOutput.Multiline = true;
      this.txtOutput.Name = "txtOutput";
      this.txtOutput.ScrollBars = ScrollBars.Vertical;
      this.txtOutput.Size = new Size(763, 157);
      this.txtOutput.TabIndex = 20;
      this.statusStrip.ImageScalingSize = new Size(20, 20);
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
      this.groupBox2.Location = new Point(3, 361);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(769, 176);
      this.groupBox2.TabIndex = 32;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Result";
      this.tabGenerally.Controls.Add((Control) this.btnResetDelivery);
      this.tabGenerally.Controls.Add((Control) this.btnSystemLogClear);
      this.tabGenerally.Controls.Add((Control) this.btnEventLogClear);
      this.tabGenerally.Controls.Add((Control) this.btnSND_NKE_IrDaOff);
      this.tabGenerally.Controls.Add((Control) this.btnStartRadioReceiver);
      this.tabGenerally.Controls.Add((Control) this.btnRadioNormal);
      this.tabGenerally.Controls.Add((Control) this.groupBox6);
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
      this.tabGenerally.Padding = new Padding(3, 3, 3, 3);
      this.tabGenerally.Size = new Size(761, 326);
      this.tabGenerally.TabIndex = 0;
      this.tabGenerally.Text = "Generally";
      this.tabGenerally.UseVisualStyleBackColor = true;
      this.btnResetDelivery.Location = new Point(160, 64);
      this.btnResetDelivery.Name = "btnResetDelivery";
      this.btnResetDelivery.Size = new Size(147, 23);
      this.btnResetDelivery.TabIndex = 65;
      this.btnResetDelivery.Text = "Reset To Delivery";
      this.btnResetDelivery.UseVisualStyleBackColor = true;
      this.btnResetDelivery.Click += new System.EventHandler(this.btnResetDelivery_Click);
      this.btnSystemLogClear.Location = new Point(160, 35);
      this.btnSystemLogClear.Name = "btnSystemLogClear";
      this.btnSystemLogClear.Size = new Size(147, 23);
      this.btnSystemLogClear.TabIndex = 64;
      this.btnSystemLogClear.Text = "System Log Clear";
      this.btnSystemLogClear.UseVisualStyleBackColor = true;
      this.btnSystemLogClear.Click += new System.EventHandler(this.btnSystemLogClear_Click);
      this.btnEventLogClear.Location = new Point(160, 6);
      this.btnEventLogClear.Name = "btnEventLogClear";
      this.btnEventLogClear.Size = new Size(147, 23);
      this.btnEventLogClear.TabIndex = 63;
      this.btnEventLogClear.Text = "Event Log Clear";
      this.btnEventLogClear.UseVisualStyleBackColor = true;
      this.btnEventLogClear.Click += new System.EventHandler(this.btnEventLogClear_Click);
      this.btnSND_NKE_IrDaOff.Location = new Point(5, 267);
      this.btnSND_NKE_IrDaOff.Name = "btnSND_NKE_IrDaOff";
      this.btnSND_NKE_IrDaOff.Size = new Size(147, 23);
      this.btnSND_NKE_IrDaOff.TabIndex = 62;
      this.btnSND_NKE_IrDaOff.Text = "IrDa Off (SND_NKE)";
      this.btnSND_NKE_IrDaOff.UseVisualStyleBackColor = true;
      this.btnSND_NKE_IrDaOff.Click += new System.EventHandler(this.btnSND_NKE_IrDaOff_Click);
      this.btnStartRadioReceiver.Location = new Point(7, 209);
      this.btnStartRadioReceiver.Name = "btnStartRadioReceiver";
      this.btnStartRadioReceiver.Size = new Size(147, 23);
      this.btnStartRadioReceiver.TabIndex = 7;
      this.btnStartRadioReceiver.Text = "Start Radio Receiver";
      this.btnStartRadioReceiver.UseVisualStyleBackColor = true;
      this.btnStartRadioReceiver.Click += new System.EventHandler(this.btnStartRadioReceiver_Click);
      this.btnRadioNormal.Location = new Point(7, 150);
      this.btnRadioNormal.Name = "btnRadioNormal";
      this.btnRadioNormal.Size = new Size(147, 23);
      this.btnRadioNormal.TabIndex = 4;
      this.btnRadioNormal.Text = "Radio Normal";
      this.btnRadioNormal.UseVisualStyleBackColor = true;
      this.btnRadioNormal.Click += new System.EventHandler(this.btnRadioNormal_Click);
      this.groupBox6.Controls.Add((Control) this.label11);
      this.groupBox6.Controls.Add((Control) this.txtTimeoutRadioReceive);
      this.groupBox6.Controls.Add((Control) this.btnRadioReceive);
      this.groupBox6.Location = new Point(469, 167);
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
      this.groupBox7.Controls.Add((Control) this.label34);
      this.groupBox7.Controls.Add((Control) this.txtRadioTimeout);
      this.groupBox7.Controls.Add((Control) this.label17);
      this.groupBox7.Controls.Add((Control) this.txtRadioFrequencyOffset);
      this.groupBox7.Controls.Add((Control) this.label10);
      this.groupBox7.Controls.Add((Control) this.btnRadioOOK);
      this.groupBox7.Controls.Add((Control) this.btnRadioPN9);
      this.groupBox7.Controls.Add((Control) this.cboxRadioModeForTest);
      this.groupBox7.Location = new Point(469, 4);
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
      this.groupBox3.Location = new Point(469, 93);
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
      this.btnResetDevice.Location = new Point(7, 35);
      this.btnResetDevice.Name = "btnResetDevice";
      this.btnResetDevice.Size = new Size(147, 23);
      this.btnResetDevice.TabIndex = 1;
      this.btnResetDevice.Text = "Reset Device";
      this.btnResetDevice.UseVisualStyleBackColor = true;
      this.btnResetDevice.Click += new System.EventHandler(this.btnResetDevice_Click);
      this.btnRadioDisable.Location = new Point(7, 179);
      this.btnRadioDisable.Name = "btnRadioDisable";
      this.btnRadioDisable.Size = new Size(147, 23);
      this.btnRadioDisable.TabIndex = 8;
      this.btnRadioDisable.Text = "Radio Disable";
      this.btnRadioDisable.UseVisualStyleBackColor = true;
      this.btnRadioDisable.Click += new System.EventHandler(this.btnRadioDisable_Click);
      this.btnReadVersion.Location = new Point(7, 6);
      this.btnReadVersion.Name = "btnReadVersion";
      this.btnReadVersion.Size = new Size(147, 23);
      this.btnReadVersion.TabIndex = 2;
      this.btnReadVersion.Text = "Read Version";
      this.btnReadVersion.UseVisualStyleBackColor = true;
      this.btnReadVersion.Click += new System.EventHandler(this.btnReadVersion_Click);
      this.btnPulseDisable.Location = new Point(7, 92);
      this.btnPulseDisable.Name = "btnPulseDisable";
      this.btnPulseDisable.Size = new Size(147, 23);
      this.btnPulseDisable.TabIndex = 5;
      this.btnPulseDisable.Text = "Pulse Disable";
      this.btnPulseDisable.UseVisualStyleBackColor = true;
      this.btnPulseDisable.Click += new System.EventHandler(this.btnPulseDisable_Click);
      this.btnPulseEnable.Location = new Point(7, 121);
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
      this.tabs.Controls.Add((Control) this.tabChannels);
      this.tabs.Controls.Add((Control) this.tabConfig);
      this.tabs.Controls.Add((Control) this.tabMBus);
      this.tabs.Controls.Add((Control) this.tabMemory);
      this.tabs.Controls.Add((Control) this.tabMinoConnect);
      this.tabs.Location = new Point(3, 3);
      this.tabs.Name = "tabs";
      this.tabs.SelectedIndex = 0;
      this.tabs.Size = new Size(769, 352);
      this.tabs.TabIndex = 33;
      this.tabs.Selected += new TabControlEventHandler(this.tabs_Selected);
      this.tabChannels.Controls.Add((Control) this.txtUndersizeDiff);
      this.tabChannels.Controls.Add((Control) this.label45);
      this.tabChannels.Controls.Add((Control) this.txtOversizeDiff);
      this.tabChannels.Controls.Add((Control) this.label44);
      this.tabChannels.Controls.Add((Control) this.txtBurstLimit);
      this.tabChannels.Controls.Add((Control) this.label43);
      this.tabChannels.Controls.Add((Control) this.txtLower);
      this.tabChannels.Controls.Add((Control) this.btnLeakRead);
      this.tabChannels.Controls.Add((Control) this.label42);
      this.tabChannels.Controls.Add((Control) this.btnLeakWrite);
      this.tabChannels.Controls.Add((Control) this.txtUpper);
      this.tabChannels.Controls.Add((Control) this.label41);
      this.tabChannels.Controls.Add((Control) this.txtUnleak);
      this.tabChannels.Controls.Add((Control) this.label40);
      this.tabChannels.Controls.Add((Control) this.txtLeak);
      this.tabChannels.Controls.Add((Control) this.label39);
      this.tabChannels.Controls.Add((Control) this.txtUndersizeLimit);
      this.tabChannels.Controls.Add((Control) this.btnUndersizeRead);
      this.tabChannels.Controls.Add((Control) this.label36);
      this.tabChannels.Controls.Add((Control) this.btnUndersizeWrite);
      this.tabChannels.Controls.Add((Control) this.txtOversizeLimit);
      this.tabChannels.Controls.Add((Control) this.btnOversizeRead);
      this.tabChannels.Controls.Add((Control) this.label35);
      this.tabChannels.Controls.Add((Control) this.btnOversizeWrite);
      this.tabChannels.Controls.Add((Control) this.txtBurstDiff);
      this.tabChannels.Controls.Add((Control) this.btnBurstRead);
      this.tabChannels.Controls.Add((Control) this.label30);
      this.tabChannels.Controls.Add((Control) this.btnBurstWrite);
      this.tabChannels.Controls.Add((Control) this.txtBlock);
      this.tabChannels.Controls.Add((Control) this.btnBlockRead);
      this.tabChannels.Controls.Add((Control) this.label29);
      this.tabChannels.Controls.Add((Control) this.btnBlockWrite);
      this.tabChannels.Controls.Add((Control) this.label18);
      this.tabChannels.Controls.Add((Control) this.comboChannel);
      this.tabChannels.Controls.Add((Control) this.txtVIFA);
      this.tabChannels.Controls.Add((Control) this.btnVIFARead);
      this.tabChannels.Controls.Add((Control) this.label37);
      this.tabChannels.Controls.Add((Control) this.btnVIFAWrite);
      this.tabChannels.Controls.Add((Control) this.txtExponentA);
      this.tabChannels.Controls.Add((Control) this.txtMantissaA);
      this.tabChannels.Controls.Add((Control) this.btnExponentARead);
      this.tabChannels.Controls.Add((Control) this.label33);
      this.tabChannels.Controls.Add((Control) this.btnExponentAWrite);
      this.tabChannels.Controls.Add((Control) this.btnMantissaARead);
      this.tabChannels.Controls.Add((Control) this.label32);
      this.tabChannels.Controls.Add((Control) this.btnMantissaAWrite);
      this.tabChannels.Controls.Add((Control) this.txtStatusFlagsA);
      this.tabChannels.Controls.Add((Control) this.btnStatusFlagsARead);
      this.tabChannels.Controls.Add((Control) this.label28);
      this.tabChannels.Controls.Add((Control) this.btnStatusFlagsAClear);
      this.tabChannels.Controls.Add((Control) this.btnReadMeterValueA);
      this.tabChannels.Controls.Add((Control) this.label16);
      this.tabChannels.Controls.Add((Control) this.txtMeterValueA);
      this.tabChannels.Controls.Add((Control) this.btnWriteMeterValueA);
      this.tabChannels.Location = new Point(4, 22);
      this.tabChannels.Name = "tabChannels";
      this.tabChannels.Size = new Size(761, 326);
      this.tabChannels.TabIndex = 3;
      this.tabChannels.Text = "Channels";
      this.tabChannels.UseVisualStyleBackColor = true;
      this.txtUndersizeDiff.Location = new Point(538, 154);
      this.txtUndersizeDiff.Maximum = new Decimal(new int[4]
      {
        int.MaxValue,
        0,
        0,
        0
      });
      this.txtUndersizeDiff.Minimum = new Decimal(new int[4]
      {
        int.MinValue,
        0,
        0,
        int.MinValue
      });
      this.txtUndersizeDiff.Name = "txtUndersizeDiff";
      this.txtUndersizeDiff.Size = new Size(89, 20);
      this.txtUndersizeDiff.TabIndex = 143;
      this.label45.Location = new Point(435, 154);
      this.label45.Name = "label45";
      this.label45.Size = new Size(95, 15);
      this.label45.TabIndex = 142;
      this.label45.Text = "Undersize Diff";
      this.label45.TextAlign = ContentAlignment.MiddleRight;
      this.txtOversizeDiff.Location = new Point(538, 96);
      this.txtOversizeDiff.Maximum = new Decimal(new int[4]
      {
        int.MaxValue,
        0,
        0,
        0
      });
      this.txtOversizeDiff.Minimum = new Decimal(new int[4]
      {
        int.MinValue,
        0,
        0,
        int.MinValue
      });
      this.txtOversizeDiff.Name = "txtOversizeDiff";
      this.txtOversizeDiff.Size = new Size(89, 20);
      this.txtOversizeDiff.TabIndex = 141;
      this.label44.Location = new Point(435, 97);
      this.label44.Name = "label44";
      this.label44.Size = new Size(95, 15);
      this.label44.TabIndex = 140;
      this.label44.Text = "Oversize Diff";
      this.label44.TextAlign = ContentAlignment.MiddleRight;
      this.txtBurstLimit.Location = new Point(538, 63);
      this.txtBurstLimit.Maximum = new Decimal(new int[4]
      {
        int.MaxValue,
        0,
        0,
        0
      });
      this.txtBurstLimit.Minimum = new Decimal(new int[4]
      {
        int.MinValue,
        0,
        0,
        int.MinValue
      });
      this.txtBurstLimit.Name = "txtBurstLimit";
      this.txtBurstLimit.Size = new Size(89, 20);
      this.txtBurstLimit.TabIndex = 139;
      this.label43.Location = new Point(435, 63);
      this.label43.Name = "label43";
      this.label43.Size = new Size(95, 15);
      this.label43.TabIndex = 138;
      this.label43.Text = "Burst Limit";
      this.label43.TextAlign = ContentAlignment.MiddleRight;
      this.txtLower.Location = new Point(538, 301);
      this.txtLower.Maximum = new Decimal(new int[4]
      {
        int.MaxValue,
        0,
        0,
        0
      });
      this.txtLower.Minimum = new Decimal(new int[4]
      {
        int.MinValue,
        0,
        0,
        int.MinValue
      });
      this.txtLower.Name = "txtLower";
      this.txtLower.Size = new Size(89, 20);
      this.txtLower.TabIndex = 137;
      this.btnLeakRead.Location = new Point(635, 298);
      this.btnLeakRead.Name = "btnLeakRead";
      this.btnLeakRead.Size = new Size(60, 23);
      this.btnLeakRead.TabIndex = 136;
      this.btnLeakRead.Text = "Read";
      this.btnLeakRead.UseVisualStyleBackColor = true;
      this.btnLeakRead.Click += new System.EventHandler(this.btnLeakRead_Click);
      this.label42.Location = new Point(435, 302);
      this.label42.Name = "label42";
      this.label42.Size = new Size(95, 15);
      this.label42.TabIndex = 135;
      this.label42.Text = "Leak Lower Limit";
      this.label42.TextAlign = ContentAlignment.MiddleRight;
      this.btnLeakWrite.Location = new Point(701, 298);
      this.btnLeakWrite.Name = "btnLeakWrite";
      this.btnLeakWrite.Size = new Size(60, 23);
      this.btnLeakWrite.TabIndex = 134;
      this.btnLeakWrite.Text = "Write";
      this.btnLeakWrite.UseVisualStyleBackColor = true;
      this.btnLeakWrite.Click += new System.EventHandler(this.btnLeakWrite_Click);
      this.txtUpper.Location = new Point(538, 272);
      this.txtUpper.Maximum = new Decimal(new int[4]
      {
        int.MaxValue,
        0,
        0,
        0
      });
      this.txtUpper.Minimum = new Decimal(new int[4]
      {
        int.MinValue,
        0,
        0,
        int.MinValue
      });
      this.txtUpper.Name = "txtUpper";
      this.txtUpper.Size = new Size(89, 20);
      this.txtUpper.TabIndex = 133;
      this.label41.Location = new Point(435, 273);
      this.label41.Name = "label41";
      this.label41.Size = new Size(95, 15);
      this.label41.TabIndex = 131;
      this.label41.Text = "Leak Upper Limit";
      this.label41.TextAlign = ContentAlignment.MiddleRight;
      this.txtUnleak.Location = new Point(538, 243);
      this.txtUnleak.Maximum = new Decimal(new int[4]
      {
        int.MaxValue,
        0,
        0,
        0
      });
      this.txtUnleak.Minimum = new Decimal(new int[4]
      {
        int.MinValue,
        0,
        0,
        int.MinValue
      });
      this.txtUnleak.Name = "txtUnleak";
      this.txtUnleak.Size = new Size(89, 20);
      this.txtUnleak.TabIndex = 129;
      this.label40.Location = new Point(435, 244);
      this.label40.Name = "label40";
      this.label40.Size = new Size(95, 15);
      this.label40.TabIndex = (int) sbyte.MaxValue;
      this.label40.Text = "Unleak Limit";
      this.label40.TextAlign = ContentAlignment.MiddleRight;
      this.txtLeak.Location = new Point(538, 215);
      this.txtLeak.Maximum = new Decimal(new int[4]
      {
        int.MaxValue,
        0,
        0,
        0
      });
      this.txtLeak.Minimum = new Decimal(new int[4]
      {
        int.MinValue,
        0,
        0,
        int.MinValue
      });
      this.txtLeak.Name = "txtLeak";
      this.txtLeak.Size = new Size(89, 20);
      this.txtLeak.TabIndex = 125;
      this.label39.Location = new Point(435, 216);
      this.label39.Name = "label39";
      this.label39.Size = new Size(95, 15);
      this.label39.TabIndex = 123;
      this.label39.Text = "Leak Limit";
      this.label39.TextAlign = ContentAlignment.MiddleRight;
      this.txtUndersizeLimit.Location = new Point(538, 178);
      this.txtUndersizeLimit.Maximum = new Decimal(new int[4]
      {
        int.MaxValue,
        0,
        0,
        0
      });
      this.txtUndersizeLimit.Minimum = new Decimal(new int[4]
      {
        int.MinValue,
        0,
        0,
        int.MinValue
      });
      this.txtUndersizeLimit.Name = "txtUndersizeLimit";
      this.txtUndersizeLimit.Size = new Size(89, 20);
      this.txtUndersizeLimit.TabIndex = 121;
      this.btnUndersizeRead.Location = new Point(635, 175);
      this.btnUndersizeRead.Name = "btnUndersizeRead";
      this.btnUndersizeRead.Size = new Size(60, 23);
      this.btnUndersizeRead.TabIndex = 120;
      this.btnUndersizeRead.Text = "Read";
      this.btnUndersizeRead.UseVisualStyleBackColor = true;
      this.btnUndersizeRead.Click += new System.EventHandler(this.btnUndersizeRead_Click);
      this.label36.Location = new Point(435, 179);
      this.label36.Name = "label36";
      this.label36.Size = new Size(95, 15);
      this.label36.TabIndex = 119;
      this.label36.Text = "Undersize Limit";
      this.label36.TextAlign = ContentAlignment.MiddleRight;
      this.btnUndersizeWrite.Location = new Point(701, 175);
      this.btnUndersizeWrite.Name = "btnUndersizeWrite";
      this.btnUndersizeWrite.Size = new Size(60, 23);
      this.btnUndersizeWrite.TabIndex = 118;
      this.btnUndersizeWrite.Text = "Write";
      this.btnUndersizeWrite.UseVisualStyleBackColor = true;
      this.btnUndersizeWrite.Click += new System.EventHandler(this.btnUndersizeWrite_Click);
      this.txtOversizeLimit.Location = new Point(538, 120);
      this.txtOversizeLimit.Maximum = new Decimal(new int[4]
      {
        int.MaxValue,
        0,
        0,
        0
      });
      this.txtOversizeLimit.Minimum = new Decimal(new int[4]
      {
        int.MinValue,
        0,
        0,
        int.MinValue
      });
      this.txtOversizeLimit.Name = "txtOversizeLimit";
      this.txtOversizeLimit.Size = new Size(89, 20);
      this.txtOversizeLimit.TabIndex = 117;
      this.btnOversizeRead.Location = new Point(635, 117);
      this.btnOversizeRead.Name = "btnOversizeRead";
      this.btnOversizeRead.Size = new Size(60, 23);
      this.btnOversizeRead.TabIndex = 116;
      this.btnOversizeRead.Text = "Read";
      this.btnOversizeRead.UseVisualStyleBackColor = true;
      this.btnOversizeRead.Click += new System.EventHandler(this.btnOversizeRead_Click);
      this.label35.Location = new Point(435, 121);
      this.label35.Name = "label35";
      this.label35.Size = new Size(95, 15);
      this.label35.TabIndex = 115;
      this.label35.Text = "Oversize Limit";
      this.label35.TextAlign = ContentAlignment.MiddleRight;
      this.btnOversizeWrite.Location = new Point(701, 117);
      this.btnOversizeWrite.Name = "btnOversizeWrite";
      this.btnOversizeWrite.Size = new Size(60, 23);
      this.btnOversizeWrite.TabIndex = 114;
      this.btnOversizeWrite.Text = "Write";
      this.btnOversizeWrite.UseVisualStyleBackColor = true;
      this.btnOversizeWrite.Click += new System.EventHandler(this.btnOversizeWrite_Click);
      this.txtBurstDiff.Location = new Point(538, 38);
      this.txtBurstDiff.Maximum = new Decimal(new int[4]
      {
        int.MaxValue,
        0,
        0,
        0
      });
      this.txtBurstDiff.Minimum = new Decimal(new int[4]
      {
        int.MinValue,
        0,
        0,
        int.MinValue
      });
      this.txtBurstDiff.Name = "txtBurstDiff";
      this.txtBurstDiff.Size = new Size(89, 20);
      this.txtBurstDiff.TabIndex = 113;
      this.btnBurstRead.Location = new Point(635, 59);
      this.btnBurstRead.Name = "btnBurstRead";
      this.btnBurstRead.Size = new Size(60, 23);
      this.btnBurstRead.TabIndex = 112;
      this.btnBurstRead.Text = "Read";
      this.btnBurstRead.UseVisualStyleBackColor = true;
      this.btnBurstRead.Click += new System.EventHandler(this.btnBurstRead_Click);
      this.label30.Location = new Point(435, 39);
      this.label30.Name = "label30";
      this.label30.Size = new Size(95, 15);
      this.label30.TabIndex = 111;
      this.label30.Text = "Burst Diff";
      this.label30.TextAlign = ContentAlignment.MiddleRight;
      this.btnBurstWrite.Location = new Point(701, 59);
      this.btnBurstWrite.Name = "btnBurstWrite";
      this.btnBurstWrite.Size = new Size(60, 23);
      this.btnBurstWrite.TabIndex = 110;
      this.btnBurstWrite.Text = "Write";
      this.btnBurstWrite.UseVisualStyleBackColor = true;
      this.btnBurstWrite.Click += new System.EventHandler(this.btnBurstWrite_Click);
      this.txtBlock.Location = new Point(538, 8);
      this.txtBlock.Maximum = new Decimal(new int[4]
      {
        int.MaxValue,
        0,
        0,
        0
      });
      this.txtBlock.Minimum = new Decimal(new int[4]
      {
        int.MinValue,
        0,
        0,
        int.MinValue
      });
      this.txtBlock.Name = "txtBlock";
      this.txtBlock.Size = new Size(89, 20);
      this.txtBlock.TabIndex = 109;
      this.btnBlockRead.Location = new Point(635, 5);
      this.btnBlockRead.Name = "btnBlockRead";
      this.btnBlockRead.Size = new Size(60, 23);
      this.btnBlockRead.TabIndex = 108;
      this.btnBlockRead.Text = "Read";
      this.btnBlockRead.UseVisualStyleBackColor = true;
      this.btnBlockRead.Click += new System.EventHandler(this.btnBlockRead_Click);
      this.label29.Location = new Point(435, 9);
      this.label29.Name = "label29";
      this.label29.Size = new Size(95, 15);
      this.label29.TabIndex = 107;
      this.label29.Text = "Block Limit";
      this.label29.TextAlign = ContentAlignment.MiddleRight;
      this.btnBlockWrite.Location = new Point(701, 5);
      this.btnBlockWrite.Name = "btnBlockWrite";
      this.btnBlockWrite.Size = new Size(60, 23);
      this.btnBlockWrite.TabIndex = 106;
      this.btnBlockWrite.Text = "Write";
      this.btnBlockWrite.UseVisualStyleBackColor = true;
      this.btnBlockWrite.Click += new System.EventHandler(this.btnBlockWrite_Click);
      this.label18.Location = new Point(12, 8);
      this.label18.Name = "label18";
      this.label18.Size = new Size(87, 15);
      this.label18.TabIndex = 105;
      this.label18.Text = "Channel:";
      this.label18.TextAlign = ContentAlignment.MiddleRight;
      this.comboChannel.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboChannel.FormattingEnabled = true;
      this.comboChannel.Items.AddRange(new object[2]
      {
        (object) "A",
        (object) "B"
      });
      this.comboChannel.Location = new Point(105, 6);
      this.comboChannel.Name = "comboChannel";
      this.comboChannel.Size = new Size(93, 21);
      this.comboChannel.TabIndex = 104;
      this.txtVIFA.Location = new Point(106, 165);
      this.txtVIFA.Name = "txtVIFA";
      this.txtVIFA.Size = new Size(93, 20);
      this.txtVIFA.TabIndex = 102;
      this.btnVIFARead.Location = new Point(205, 163);
      this.btnVIFARead.Name = "btnVIFARead";
      this.btnVIFARead.Size = new Size(60, 23);
      this.btnVIFARead.TabIndex = 98;
      this.btnVIFARead.Text = "Read";
      this.btnVIFARead.UseVisualStyleBackColor = true;
      this.btnVIFARead.Click += new System.EventHandler(this.btnVIFARead_Click);
      this.label37.Location = new Point(13, 167);
      this.label37.Name = "label37";
      this.label37.Size = new Size(87, 15);
      this.label37.TabIndex = 97;
      this.label37.Text = "VIF";
      this.label37.TextAlign = ContentAlignment.MiddleRight;
      this.btnVIFAWrite.Location = new Point(271, 163);
      this.btnVIFAWrite.Name = "btnVIFAWrite";
      this.btnVIFAWrite.Size = new Size(60, 23);
      this.btnVIFAWrite.TabIndex = 96;
      this.btnVIFAWrite.Text = "Write";
      this.btnVIFAWrite.UseVisualStyleBackColor = true;
      this.btnVIFAWrite.Click += new System.EventHandler(this.btnVIFAWrite_Click);
      this.txtExponentA.Location = new Point(107, 141);
      this.txtExponentA.Maximum = new Decimal(new int[4]
      {
        int.MaxValue,
        0,
        0,
        0
      });
      this.txtExponentA.Minimum = new Decimal(new int[4]
      {
        int.MinValue,
        0,
        0,
        int.MinValue
      });
      this.txtExponentA.Name = "txtExponentA";
      this.txtExponentA.Size = new Size(89, 20);
      this.txtExponentA.TabIndex = 93;
      this.txtMantissaA.Location = new Point(108, 115);
      this.txtMantissaA.Maximum = new Decimal(new int[4]
      {
        int.MaxValue,
        0,
        0,
        0
      });
      this.txtMantissaA.Minimum = new Decimal(new int[4]
      {
        int.MinValue,
        0,
        0,
        int.MinValue
      });
      this.txtMantissaA.Name = "txtMantissaA";
      this.txtMantissaA.Size = new Size(89, 20);
      this.txtMantissaA.TabIndex = 92;
      this.btnExponentARead.Location = new Point(204, 137);
      this.btnExponentARead.Name = "btnExponentARead";
      this.btnExponentARead.Size = new Size(60, 23);
      this.btnExponentARead.TabIndex = 83;
      this.btnExponentARead.Text = "Read";
      this.btnExponentARead.UseVisualStyleBackColor = true;
      this.btnExponentARead.Click += new System.EventHandler(this.btnExponentARead_Click);
      this.label33.Location = new Point(4, 141);
      this.label33.Name = "label33";
      this.label33.Size = new Size(95, 15);
      this.label33.TabIndex = 82;
      this.label33.Text = "Scale Exponent";
      this.label33.TextAlign = ContentAlignment.MiddleRight;
      this.btnExponentAWrite.Location = new Point(270, 137);
      this.btnExponentAWrite.Name = "btnExponentAWrite";
      this.btnExponentAWrite.Size = new Size(60, 23);
      this.btnExponentAWrite.TabIndex = 81;
      this.btnExponentAWrite.Text = "Write";
      this.btnExponentAWrite.UseVisualStyleBackColor = true;
      this.btnExponentAWrite.Click += new System.EventHandler(this.btnExponentAWrite_Click);
      this.btnMantissaARead.Location = new Point(204, 112);
      this.btnMantissaARead.Name = "btnMantissaARead";
      this.btnMantissaARead.Size = new Size(60, 23);
      this.btnMantissaARead.TabIndex = 79;
      this.btnMantissaARead.Text = "Read";
      this.btnMantissaARead.UseVisualStyleBackColor = true;
      this.btnMantissaARead.Click += new System.EventHandler(this.btnMantissaARead_Click);
      this.label32.Location = new Point(12, 116);
      this.label32.Name = "label32";
      this.label32.Size = new Size(87, 15);
      this.label32.TabIndex = 78;
      this.label32.Text = "Scale Mantissa";
      this.label32.TextAlign = ContentAlignment.MiddleRight;
      this.btnMantissaAWrite.Location = new Point(270, 112);
      this.btnMantissaAWrite.Name = "btnMantissaAWrite";
      this.btnMantissaAWrite.Size = new Size(60, 23);
      this.btnMantissaAWrite.TabIndex = 77;
      this.btnMantissaAWrite.Text = "Write";
      this.btnMantissaAWrite.UseVisualStyleBackColor = true;
      this.btnMantissaAWrite.Click += new System.EventHandler(this.btnMantissaAWrite_Click);
      this.txtStatusFlagsA.Location = new Point(105, 66);
      this.txtStatusFlagsA.Name = "txtStatusFlagsA";
      this.txtStatusFlagsA.Size = new Size(93, 20);
      this.txtStatusFlagsA.TabIndex = 75;
      this.btnStatusFlagsARead.Location = new Point(204, 64);
      this.btnStatusFlagsARead.Name = "btnStatusFlagsARead";
      this.btnStatusFlagsARead.Size = new Size(60, 23);
      this.btnStatusFlagsARead.TabIndex = 70;
      this.btnStatusFlagsARead.Text = "Read";
      this.btnStatusFlagsARead.UseVisualStyleBackColor = true;
      this.btnStatusFlagsARead.Click += new System.EventHandler(this.btnStatusFlagsARead_Click);
      this.label28.Location = new Point(12, 68);
      this.label28.Name = "label28";
      this.label28.Size = new Size(87, 15);
      this.label28.TabIndex = 68;
      this.label28.Text = "Status Flags";
      this.label28.TextAlign = ContentAlignment.MiddleRight;
      this.btnStatusFlagsAClear.Location = new Point(270, 64);
      this.btnStatusFlagsAClear.Name = "btnStatusFlagsAClear";
      this.btnStatusFlagsAClear.Size = new Size(60, 23);
      this.btnStatusFlagsAClear.TabIndex = 67;
      this.btnStatusFlagsAClear.Text = "Clear";
      this.btnStatusFlagsAClear.UseVisualStyleBackColor = true;
      this.btnStatusFlagsAClear.Click += new System.EventHandler(this.btnStatusFlagsAClear_Click);
      this.btnReadMeterValueA.Location = new Point(204, 38);
      this.btnReadMeterValueA.Name = "btnReadMeterValueA";
      this.btnReadMeterValueA.Size = new Size(60, 23);
      this.btnReadMeterValueA.TabIndex = 62;
      this.btnReadMeterValueA.Text = "Read";
      this.btnReadMeterValueA.UseVisualStyleBackColor = true;
      this.btnReadMeterValueA.Click += new System.EventHandler(this.btnReadMeterValueA_Click);
      this.label16.Location = new Point(12, 42);
      this.label16.Name = "label16";
      this.label16.Size = new Size(87, 15);
      this.label16.TabIndex = 60;
      this.label16.Text = "Meter value";
      this.label16.TextAlign = ContentAlignment.MiddleRight;
      this.txtMeterValueA.Location = new Point(108, 41);
      this.txtMeterValueA.Maximum = new Decimal(new int[4]
      {
        int.MaxValue,
        0,
        0,
        0
      });
      this.txtMeterValueA.Minimum = new Decimal(new int[4]
      {
        int.MinValue,
        0,
        0,
        int.MinValue
      });
      this.txtMeterValueA.Name = "txtMeterValueA";
      this.txtMeterValueA.Size = new Size(89, 20);
      this.txtMeterValueA.TabIndex = 61;
      this.btnWriteMeterValueA.Location = new Point(270, 38);
      this.btnWriteMeterValueA.Name = "btnWriteMeterValueA";
      this.btnWriteMeterValueA.Size = new Size(60, 23);
      this.btnWriteMeterValueA.TabIndex = 59;
      this.btnWriteMeterValueA.Text = "Write";
      this.btnWriteMeterValueA.UseVisualStyleBackColor = true;
      this.btnWriteMeterValueA.Click += new System.EventHandler(this.btnWriteMeterValue_Click);
      this.tabConfig.Controls.Add((Control) this.txtPulseOn);
      this.tabConfig.Controls.Add((Control) this.txtPulsePeriod);
      this.tabConfig.Controls.Add((Control) this.label38);
      this.tabConfig.Controls.Add((Control) this.btnPulseWrite);
      this.tabConfig.Controls.Add((Control) this.btnPulseRead);
      this.tabConfig.Controls.Add((Control) this.label31);
      this.tabConfig.Controls.Add((Control) this.btnObisWrite);
      this.tabConfig.Controls.Add((Control) this.btnObisRead);
      this.tabConfig.Controls.Add((Control) this.txtObisCode);
      this.tabConfig.Controls.Add((Control) this.btnRadioListQuery);
      this.tabConfig.Controls.Add((Control) this.txtRadioList);
      this.tabConfig.Controls.Add((Control) this.label27);
      this.tabConfig.Controls.Add((Control) this.btnRadioListWrite);
      this.tabConfig.Controls.Add((Control) this.btnRadioListRead);
      this.tabConfig.Controls.Add((Control) this.txtKeyMonth);
      this.tabConfig.Controls.Add((Control) this.txtKeyDay);
      this.tabConfig.Controls.Add((Control) this.label26);
      this.tabConfig.Controls.Add((Control) this.btnKeydateWrite);
      this.tabConfig.Controls.Add((Control) this.btnKeydateRead);
      this.tabConfig.Controls.Add((Control) this.label25);
      this.tabConfig.Controls.Add((Control) this.btnMBusIdWrite);
      this.tabConfig.Controls.Add((Control) this.btnManIdRead);
      this.tabConfig.Controls.Add((Control) this.txtManIdABC);
      this.tabConfig.Controls.Add((Control) this.label24);
      this.tabConfig.Controls.Add((Control) this.btnMBusTypeWrite);
      this.tabConfig.Controls.Add((Control) this.btnMBusTypeRead);
      this.tabConfig.Controls.Add((Control) this.txtMBusTypeABC);
      this.tabConfig.Controls.Add((Control) this.label23);
      this.tabConfig.Controls.Add((Control) this.btnMBusVersionWrite);
      this.tabConfig.Controls.Add((Control) this.btnMBusVersionRead);
      this.tabConfig.Controls.Add((Control) this.txtMBusVersionABC);
      this.tabConfig.Controls.Add((Control) this.label22);
      this.tabConfig.Controls.Add((Control) this.btnMBusAddressWrite);
      this.tabConfig.Controls.Add((Control) this.btnMBusAddressRead);
      this.tabConfig.Controls.Add((Control) this.txtMBusAddressABC);
      this.tabConfig.Controls.Add((Control) this.label20);
      this.tabConfig.Controls.Add((Control) this.cboxConfigChannel);
      this.tabConfig.Controls.Add((Control) this.label21);
      this.tabConfig.Controls.Add((Control) this.btnSerialWrite);
      this.tabConfig.Controls.Add((Control) this.btnSerialRead);
      this.tabConfig.Controls.Add((Control) this.txtSerialABC);
      this.tabConfig.Controls.Add((Control) this.btnRadioFlagsClear);
      this.tabConfig.Controls.Add((Control) this.btnConfigFlagsClear);
      this.tabConfig.Controls.Add((Control) this.btnRadioFlagsSet);
      this.tabConfig.Controls.Add((Control) this.btnConfigFlagsSet);
      this.tabConfig.Controls.Add((Control) this.btnRadioFlagsWrite);
      this.tabConfig.Controls.Add((Control) this.btnConfigFlagsWrite);
      this.tabConfig.Controls.Add((Control) this.btnRadioFlagsRead);
      this.tabConfig.Controls.Add((Control) this.btnConfigFlagsRead);
      this.tabConfig.Controls.Add((Control) this.txtRadioFlags);
      this.tabConfig.Controls.Add((Control) this.txtConfigFlags);
      this.tabConfig.Controls.Add((Control) this.label19);
      this.tabConfig.Controls.Add((Control) this.label15);
      this.tabConfig.Location = new Point(4, 22);
      this.tabConfig.Name = "tabConfig";
      this.tabConfig.Size = new Size(761, 326);
      this.tabConfig.TabIndex = 4;
      this.tabConfig.Text = "Config";
      this.tabConfig.UseVisualStyleBackColor = true;
      this.txtPulseOn.Location = new Point(536, 234);
      this.txtPulseOn.Maximum = new Decimal(new int[4]
      {
        int.MaxValue,
        0,
        0,
        0
      });
      this.txtPulseOn.Minimum = new Decimal(new int[4]
      {
        int.MinValue,
        0,
        0,
        int.MinValue
      });
      this.txtPulseOn.Name = "txtPulseOn";
      this.txtPulseOn.Size = new Size(48, 20);
      this.txtPulseOn.TabIndex = 130;
      this.txtPulsePeriod.Location = new Point(482, 234);
      this.txtPulsePeriod.Maximum = new Decimal(new int[4]
      {
        int.MaxValue,
        0,
        0,
        0
      });
      this.txtPulsePeriod.Minimum = new Decimal(new int[4]
      {
        int.MinValue,
        0,
        0,
        int.MinValue
      });
      this.txtPulsePeriod.Name = "txtPulsePeriod";
      this.txtPulsePeriod.Size = new Size(48, 20);
      this.txtPulsePeriod.TabIndex = 129;
      this.label38.Location = new Point(388, 236);
      this.label38.Name = "label38";
      this.label38.Size = new Size(87, 15);
      this.label38.TabIndex = 128;
      this.label38.Text = "Pulse Testing";
      this.label38.TextAlign = ContentAlignment.MiddleRight;
      this.btnPulseWrite.Location = new Point(656, 232);
      this.btnPulseWrite.Name = "btnPulseWrite";
      this.btnPulseWrite.Size = new Size(60, 23);
      this.btnPulseWrite.TabIndex = (int) sbyte.MaxValue;
      this.btnPulseWrite.Text = "Write";
      this.btnPulseWrite.UseVisualStyleBackColor = true;
      this.btnPulseWrite.Click += new System.EventHandler(this.btnPulseWrite_Click);
      this.btnPulseRead.Location = new Point(590, 232);
      this.btnPulseRead.Name = "btnPulseRead";
      this.btnPulseRead.Size = new Size(60, 23);
      this.btnPulseRead.TabIndex = 126;
      this.btnPulseRead.Text = "Read";
      this.btnPulseRead.UseVisualStyleBackColor = true;
      this.btnPulseRead.Click += new System.EventHandler(this.btnPulseRead_Click);
      this.label31.Location = new Point(389, 94);
      this.label31.Name = "label31";
      this.label31.Size = new Size(87, 15);
      this.label31.TabIndex = 125;
      this.label31.Text = "Obis Code";
      this.label31.TextAlign = ContentAlignment.MiddleRight;
      this.btnObisWrite.Location = new Point(656, 90);
      this.btnObisWrite.Name = "btnObisWrite";
      this.btnObisWrite.Size = new Size(60, 23);
      this.btnObisWrite.TabIndex = 124;
      this.btnObisWrite.Text = "Write";
      this.btnObisWrite.UseVisualStyleBackColor = true;
      this.btnObisWrite.Click += new System.EventHandler(this.btnObisWrite_Click);
      this.btnObisRead.Location = new Point(590, 90);
      this.btnObisRead.Name = "btnObisRead";
      this.btnObisRead.Size = new Size(60, 23);
      this.btnObisRead.TabIndex = 123;
      this.btnObisRead.Text = "Read";
      this.btnObisRead.UseVisualStyleBackColor = true;
      this.btnObisRead.Click += new System.EventHandler(this.btnObisRead_Click);
      this.txtObisCode.Location = new Point(482, 91);
      this.txtObisCode.Name = "txtObisCode";
      this.txtObisCode.Size = new Size(102, 20);
      this.txtObisCode.TabIndex = 122;
      this.btnRadioListQuery.Location = new Point(350, 280);
      this.btnRadioListQuery.Name = "btnRadioListQuery";
      this.btnRadioListQuery.Size = new Size(60, 23);
      this.btnRadioListQuery.TabIndex = 117;
      this.btnRadioListQuery.Text = "Query";
      this.btnRadioListQuery.UseVisualStyleBackColor = true;
      this.btnRadioListQuery.Click += new System.EventHandler(this.btnRadioListQuery_Click);
      this.txtRadioList.Location = new Point(110, 282);
      this.txtRadioList.Maximum = new Decimal(new int[4]
      {
        int.MaxValue,
        0,
        0,
        0
      });
      this.txtRadioList.Minimum = new Decimal(new int[4]
      {
        int.MinValue,
        0,
        0,
        int.MinValue
      });
      this.txtRadioList.Name = "txtRadioList";
      this.txtRadioList.Size = new Size(102, 20);
      this.txtRadioList.TabIndex = 116;
      this.label27.Location = new Point(17, 284);
      this.label27.Name = "label27";
      this.label27.Size = new Size(87, 15);
      this.label27.TabIndex = 115;
      this.label27.Text = "Radio List";
      this.label27.TextAlign = ContentAlignment.MiddleRight;
      this.btnRadioListWrite.Location = new Point(284, 280);
      this.btnRadioListWrite.Name = "btnRadioListWrite";
      this.btnRadioListWrite.Size = new Size(60, 23);
      this.btnRadioListWrite.TabIndex = 114;
      this.btnRadioListWrite.Text = "Write";
      this.btnRadioListWrite.UseVisualStyleBackColor = true;
      this.btnRadioListWrite.Click += new System.EventHandler(this.btnRadioListWrite_Click);
      this.btnRadioListRead.Location = new Point(218, 280);
      this.btnRadioListRead.Name = "btnRadioListRead";
      this.btnRadioListRead.Size = new Size(60, 23);
      this.btnRadioListRead.TabIndex = 113;
      this.btnRadioListRead.Text = "Read";
      this.btnRadioListRead.UseVisualStyleBackColor = true;
      this.btnRadioListRead.Click += new System.EventHandler(this.btnRadioListRead_Click);
      this.txtKeyMonth.Location = new Point(164, 234);
      this.txtKeyMonth.Maximum = new Decimal(new int[4]
      {
        int.MaxValue,
        0,
        0,
        0
      });
      this.txtKeyMonth.Minimum = new Decimal(new int[4]
      {
        int.MinValue,
        0,
        0,
        int.MinValue
      });
      this.txtKeyMonth.Name = "txtKeyMonth";
      this.txtKeyMonth.Size = new Size(48, 20);
      this.txtKeyMonth.TabIndex = 111;
      this.txtKeyDay.Location = new Point(110, 234);
      this.txtKeyDay.Maximum = new Decimal(new int[4]
      {
        int.MaxValue,
        0,
        0,
        0
      });
      this.txtKeyDay.Minimum = new Decimal(new int[4]
      {
        int.MinValue,
        0,
        0,
        int.MinValue
      });
      this.txtKeyDay.Name = "txtKeyDay";
      this.txtKeyDay.Size = new Size(48, 20);
      this.txtKeyDay.TabIndex = 110;
      this.label26.Location = new Point(17, 236);
      this.label26.Name = "label26";
      this.label26.Size = new Size(87, 15);
      this.label26.TabIndex = 109;
      this.label26.Text = "KeyDate DD MM";
      this.label26.TextAlign = ContentAlignment.MiddleRight;
      this.btnKeydateWrite.Location = new Point(284, 232);
      this.btnKeydateWrite.Name = "btnKeydateWrite";
      this.btnKeydateWrite.Size = new Size(60, 23);
      this.btnKeydateWrite.TabIndex = 108;
      this.btnKeydateWrite.Text = "Write";
      this.btnKeydateWrite.UseVisualStyleBackColor = true;
      this.btnKeydateWrite.Click += new System.EventHandler(this.btnKeydateWrite_Click);
      this.btnKeydateRead.Location = new Point(218, 232);
      this.btnKeydateRead.Name = "btnKeydateRead";
      this.btnKeydateRead.Size = new Size(60, 23);
      this.btnKeydateRead.TabIndex = 107;
      this.btnKeydateRead.Text = "Read";
      this.btnKeydateRead.UseVisualStyleBackColor = true;
      this.btnKeydateRead.Click += new System.EventHandler(this.btnKeydateRead_Click);
      this.label25.Location = new Point(17, 198);
      this.label25.Name = "label25";
      this.label25.Size = new Size(87, 15);
      this.label25.TabIndex = 105;
      this.label25.Text = "Manufacturer ID";
      this.label25.TextAlign = ContentAlignment.MiddleRight;
      this.btnMBusIdWrite.Location = new Point(284, 194);
      this.btnMBusIdWrite.Name = "btnMBusIdWrite";
      this.btnMBusIdWrite.Size = new Size(60, 23);
      this.btnMBusIdWrite.TabIndex = 104;
      this.btnMBusIdWrite.Text = "Write";
      this.btnMBusIdWrite.UseVisualStyleBackColor = true;
      this.btnMBusIdWrite.Click += new System.EventHandler(this.btnMBusIdWrite_Click);
      this.btnManIdRead.Location = new Point(218, 194);
      this.btnManIdRead.Name = "btnManIdRead";
      this.btnManIdRead.Size = new Size(60, 23);
      this.btnManIdRead.TabIndex = 103;
      this.btnManIdRead.Text = "Read";
      this.btnManIdRead.UseVisualStyleBackColor = true;
      this.btnManIdRead.Click += new System.EventHandler(this.btnManIdRead_Click);
      this.txtManIdABC.Location = new Point(110, 195);
      this.txtManIdABC.Name = "txtManIdABC";
      this.txtManIdABC.Size = new Size(102, 20);
      this.txtManIdABC.TabIndex = 102;
      this.label24.Location = new Point(17, 172);
      this.label24.Name = "label24";
      this.label24.Size = new Size(87, 15);
      this.label24.TabIndex = 101;
      this.label24.Text = "MBus Type";
      this.label24.TextAlign = ContentAlignment.MiddleRight;
      this.btnMBusTypeWrite.Location = new Point(284, 168);
      this.btnMBusTypeWrite.Name = "btnMBusTypeWrite";
      this.btnMBusTypeWrite.Size = new Size(60, 23);
      this.btnMBusTypeWrite.TabIndex = 100;
      this.btnMBusTypeWrite.Text = "Write";
      this.btnMBusTypeWrite.UseVisualStyleBackColor = true;
      this.btnMBusTypeWrite.Click += new System.EventHandler(this.btnMBusTypeWrite_Click);
      this.btnMBusTypeRead.Location = new Point(218, 168);
      this.btnMBusTypeRead.Name = "btnMBusTypeRead";
      this.btnMBusTypeRead.Size = new Size(60, 23);
      this.btnMBusTypeRead.TabIndex = 99;
      this.btnMBusTypeRead.Text = "Read";
      this.btnMBusTypeRead.UseVisualStyleBackColor = true;
      this.btnMBusTypeRead.Click += new System.EventHandler(this.btnMBusTypeRead_Click);
      this.txtMBusTypeABC.Location = new Point(110, 169);
      this.txtMBusTypeABC.Name = "txtMBusTypeABC";
      this.txtMBusTypeABC.Size = new Size(102, 20);
      this.txtMBusTypeABC.TabIndex = 98;
      this.label23.Location = new Point(17, 146);
      this.label23.Name = "label23";
      this.label23.Size = new Size(87, 15);
      this.label23.TabIndex = 97;
      this.label23.Text = "MBus Version";
      this.label23.TextAlign = ContentAlignment.MiddleRight;
      this.btnMBusVersionWrite.Location = new Point(284, 142);
      this.btnMBusVersionWrite.Name = "btnMBusVersionWrite";
      this.btnMBusVersionWrite.Size = new Size(60, 23);
      this.btnMBusVersionWrite.TabIndex = 96;
      this.btnMBusVersionWrite.Text = "Write";
      this.btnMBusVersionWrite.UseVisualStyleBackColor = true;
      this.btnMBusVersionWrite.Click += new System.EventHandler(this.btnMBusVersionWrite_Click);
      this.btnMBusVersionRead.Location = new Point(218, 142);
      this.btnMBusVersionRead.Name = "btnMBusVersionRead";
      this.btnMBusVersionRead.Size = new Size(60, 23);
      this.btnMBusVersionRead.TabIndex = 95;
      this.btnMBusVersionRead.Text = "Read";
      this.btnMBusVersionRead.UseVisualStyleBackColor = true;
      this.btnMBusVersionRead.Click += new System.EventHandler(this.btnMBusVersionRead_Click);
      this.txtMBusVersionABC.Location = new Point(110, 143);
      this.txtMBusVersionABC.Name = "txtMBusVersionABC";
      this.txtMBusVersionABC.Size = new Size(102, 20);
      this.txtMBusVersionABC.TabIndex = 94;
      this.label22.Location = new Point(17, 120);
      this.label22.Name = "label22";
      this.label22.Size = new Size(87, 15);
      this.label22.TabIndex = 93;
      this.label22.Text = "MBus Address";
      this.label22.TextAlign = ContentAlignment.MiddleRight;
      this.btnMBusAddressWrite.Location = new Point(284, 116);
      this.btnMBusAddressWrite.Name = "btnMBusAddressWrite";
      this.btnMBusAddressWrite.Size = new Size(60, 23);
      this.btnMBusAddressWrite.TabIndex = 92;
      this.btnMBusAddressWrite.Text = "Write";
      this.btnMBusAddressWrite.UseVisualStyleBackColor = true;
      this.btnMBusAddressWrite.Click += new System.EventHandler(this.btnMBusAddressWrite_Click);
      this.btnMBusAddressRead.Location = new Point(218, 116);
      this.btnMBusAddressRead.Name = "btnMBusAddressRead";
      this.btnMBusAddressRead.Size = new Size(60, 23);
      this.btnMBusAddressRead.TabIndex = 91;
      this.btnMBusAddressRead.Text = "Read";
      this.btnMBusAddressRead.UseVisualStyleBackColor = true;
      this.btnMBusAddressRead.Click += new System.EventHandler(this.btnMBusAddressRead_Click);
      this.txtMBusAddressABC.Location = new Point(110, 117);
      this.txtMBusAddressABC.Name = "txtMBusAddressABC";
      this.txtMBusAddressABC.Size = new Size(102, 20);
      this.txtMBusAddressABC.TabIndex = 90;
      this.label20.Location = new Point(17, 67);
      this.label20.Name = "label20";
      this.label20.Size = new Size(87, 15);
      this.label20.TabIndex = 89;
      this.label20.Text = "Config Group";
      this.label20.TextAlign = ContentAlignment.MiddleRight;
      this.cboxConfigChannel.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxConfigChannel.FormattingEnabled = true;
      this.cboxConfigChannel.Items.AddRange(new object[3]
      {
        (object) "A",
        (object) "B",
        (object) "C"
      });
      this.cboxConfigChannel.Location = new Point(110, 65);
      this.cboxConfigChannel.Name = "cboxConfigChannel";
      this.cboxConfigChannel.Size = new Size(102, 21);
      this.cboxConfigChannel.TabIndex = 88;
      this.label21.Location = new Point(17, 94);
      this.label21.Name = "label21";
      this.label21.Size = new Size(87, 15);
      this.label21.TabIndex = 87;
      this.label21.Text = "Serial Number";
      this.label21.TextAlign = ContentAlignment.MiddleRight;
      this.btnSerialWrite.Location = new Point(284, 90);
      this.btnSerialWrite.Name = "btnSerialWrite";
      this.btnSerialWrite.Size = new Size(60, 23);
      this.btnSerialWrite.TabIndex = 83;
      this.btnSerialWrite.Text = "Write";
      this.btnSerialWrite.UseVisualStyleBackColor = true;
      this.btnSerialWrite.Click += new System.EventHandler(this.btnSerialWrite_Click);
      this.btnSerialRead.Location = new Point(218, 90);
      this.btnSerialRead.Name = "btnSerialRead";
      this.btnSerialRead.Size = new Size(60, 23);
      this.btnSerialRead.TabIndex = 82;
      this.btnSerialRead.Text = "Read";
      this.btnSerialRead.UseVisualStyleBackColor = true;
      this.btnSerialRead.Click += new System.EventHandler(this.btnSerialRead_Click);
      this.txtSerialABC.Location = new Point(110, 91);
      this.txtSerialABC.Name = "txtSerialABC";
      this.txtSerialABC.Size = new Size(102, 20);
      this.txtSerialABC.TabIndex = 77;
      this.btnRadioFlagsClear.Location = new Point(416, 38);
      this.btnRadioFlagsClear.Name = "btnRadioFlagsClear";
      this.btnRadioFlagsClear.Size = new Size(60, 23);
      this.btnRadioFlagsClear.TabIndex = 75;
      this.btnRadioFlagsClear.Text = "Clear";
      this.btnRadioFlagsClear.UseVisualStyleBackColor = true;
      this.btnRadioFlagsClear.Click += new System.EventHandler(this.btnRadioFlagsClear_Click);
      this.btnConfigFlagsClear.Location = new Point(416, 11);
      this.btnConfigFlagsClear.Name = "btnConfigFlagsClear";
      this.btnConfigFlagsClear.Size = new Size(60, 23);
      this.btnConfigFlagsClear.TabIndex = 74;
      this.btnConfigFlagsClear.Text = "Clear";
      this.btnConfigFlagsClear.UseVisualStyleBackColor = true;
      this.btnConfigFlagsClear.Click += new System.EventHandler(this.btnConfigFlagsClear_Click);
      this.btnRadioFlagsSet.Location = new Point(350, 38);
      this.btnRadioFlagsSet.Name = "btnRadioFlagsSet";
      this.btnRadioFlagsSet.Size = new Size(60, 23);
      this.btnRadioFlagsSet.TabIndex = 73;
      this.btnRadioFlagsSet.Text = "Set";
      this.btnRadioFlagsSet.UseVisualStyleBackColor = true;
      this.btnRadioFlagsSet.Click += new System.EventHandler(this.btnRadioFlagsSet_Click);
      this.btnConfigFlagsSet.Location = new Point(350, 11);
      this.btnConfigFlagsSet.Name = "btnConfigFlagsSet";
      this.btnConfigFlagsSet.Size = new Size(60, 23);
      this.btnConfigFlagsSet.TabIndex = 72;
      this.btnConfigFlagsSet.Text = "Set";
      this.btnConfigFlagsSet.UseVisualStyleBackColor = true;
      this.btnConfigFlagsSet.Click += new System.EventHandler(this.btnConfigFlagsSet_Click);
      this.btnRadioFlagsWrite.Location = new Point(284, 38);
      this.btnRadioFlagsWrite.Name = "btnRadioFlagsWrite";
      this.btnRadioFlagsWrite.Size = new Size(60, 23);
      this.btnRadioFlagsWrite.TabIndex = 71;
      this.btnRadioFlagsWrite.Text = "Write";
      this.btnRadioFlagsWrite.UseVisualStyleBackColor = true;
      this.btnRadioFlagsWrite.Click += new System.EventHandler(this.btnRadioFlagsWrite_Click);
      this.btnConfigFlagsWrite.Location = new Point(284, 11);
      this.btnConfigFlagsWrite.Name = "btnConfigFlagsWrite";
      this.btnConfigFlagsWrite.Size = new Size(60, 23);
      this.btnConfigFlagsWrite.TabIndex = 70;
      this.btnConfigFlagsWrite.Text = "Write";
      this.btnConfigFlagsWrite.UseVisualStyleBackColor = true;
      this.btnConfigFlagsWrite.Click += new System.EventHandler(this.btnConfigFlagsWrite_Click);
      this.btnRadioFlagsRead.Location = new Point(218, 38);
      this.btnRadioFlagsRead.Name = "btnRadioFlagsRead";
      this.btnRadioFlagsRead.Size = new Size(60, 23);
      this.btnRadioFlagsRead.TabIndex = 69;
      this.btnRadioFlagsRead.Text = "Read";
      this.btnRadioFlagsRead.UseVisualStyleBackColor = true;
      this.btnRadioFlagsRead.Click += new System.EventHandler(this.btnRadioFlagsRead_Click);
      this.btnConfigFlagsRead.Location = new Point(218, 11);
      this.btnConfigFlagsRead.Name = "btnConfigFlagsRead";
      this.btnConfigFlagsRead.Size = new Size(60, 23);
      this.btnConfigFlagsRead.TabIndex = 68;
      this.btnConfigFlagsRead.Text = "Read";
      this.btnConfigFlagsRead.UseVisualStyleBackColor = true;
      this.btnConfigFlagsRead.Click += new System.EventHandler(this.btnConfigFlagsRead_Click);
      this.txtRadioFlags.Location = new Point(110, 39);
      this.txtRadioFlags.Name = "txtRadioFlags";
      this.txtRadioFlags.Size = new Size(102, 20);
      this.txtRadioFlags.TabIndex = 67;
      this.txtConfigFlags.Location = new Point(110, 13);
      this.txtConfigFlags.Name = "txtConfigFlags";
      this.txtConfigFlags.Size = new Size(102, 20);
      this.txtConfigFlags.TabIndex = 66;
      this.label19.Location = new Point(17, 42);
      this.label19.Name = "label19";
      this.label19.Size = new Size(87, 15);
      this.label19.TabIndex = 62;
      this.label19.Text = "cfg_radio_flags";
      this.label19.TextAlign = ContentAlignment.MiddleRight;
      this.label15.Location = new Point(17, 13);
      this.label15.Name = "label15";
      this.label15.Size = new Size(87, 15);
      this.label15.TabIndex = 61;
      this.label15.Text = "cfg_config_flags";
      this.label15.TextAlign = ContentAlignment.MiddleRight;
      this.tabMBus.Controls.Add((Control) this.btnMBusStatusQuery);
      this.tabMBus.Controls.Add((Control) this.btnDepassNow);
      this.tabMBus.Controls.Add((Control) this.txtDepassPeriod);
      this.tabMBus.Controls.Add((Control) this.txtDepassTimeout);
      this.tabMBus.Controls.Add((Control) this.label46);
      this.tabMBus.Controls.Add((Control) this.btnDepassWrite);
      this.tabMBus.Controls.Add((Control) this.btnDepassRead);
      this.tabMBus.Location = new Point(4, 22);
      this.tabMBus.Margin = new Padding(2, 2, 2, 2);
      this.tabMBus.Name = "tabMBus";
      this.tabMBus.Padding = new Padding(2, 2, 2, 2);
      this.tabMBus.Size = new Size(761, 326);
      this.tabMBus.TabIndex = 5;
      this.tabMBus.Text = "MBus";
      this.tabMBus.UseVisualStyleBackColor = true;
      this.btnMBusStatusQuery.Location = new Point(20, 67);
      this.btnMBusStatusQuery.Name = "btnMBusStatusQuery";
      this.btnMBusStatusQuery.Size = new Size(116, 23);
      this.btnMBusStatusQuery.TabIndex = 118;
      this.btnMBusStatusQuery.Text = "MBus Status Query";
      this.btnMBusStatusQuery.UseVisualStyleBackColor = true;
      this.btnMBusStatusQuery.Click += new System.EventHandler(this.btnMBusStatusQuery_Click);
      this.btnDepassNow.Location = new Point(409, 17);
      this.btnDepassNow.Name = "btnDepassNow";
      this.btnDepassNow.Size = new Size(60, 23);
      this.btnDepassNow.TabIndex = 117;
      this.btnDepassNow.Text = "Now";
      this.btnDepassNow.UseVisualStyleBackColor = true;
      this.btnDepassNow.Click += new System.EventHandler(this.btnDepassNow_Click);
      this.txtDepassPeriod.Location = new Point(195, 19);
      this.txtDepassPeriod.Maximum = new Decimal(new int[4]
      {
        int.MaxValue,
        0,
        0,
        0
      });
      this.txtDepassPeriod.Minimum = new Decimal(new int[4]
      {
        int.MinValue,
        0,
        0,
        int.MinValue
      });
      this.txtDepassPeriod.Name = "txtDepassPeriod";
      this.txtDepassPeriod.Size = new Size(48, 20);
      this.txtDepassPeriod.TabIndex = 116;
      this.txtDepassTimeout.Location = new Point(141, 19);
      this.txtDepassTimeout.Maximum = new Decimal(new int[4]
      {
        int.MaxValue,
        0,
        0,
        0
      });
      this.txtDepassTimeout.Minimum = new Decimal(new int[4]
      {
        int.MinValue,
        0,
        0,
        int.MinValue
      });
      this.txtDepassTimeout.Name = "txtDepassTimeout";
      this.txtDepassTimeout.Size = new Size(48, 20);
      this.txtDepassTimeout.TabIndex = 115;
      this.label46.Location = new Point(17, 20);
      this.label46.Name = "label46";
      this.label46.Size = new Size(118, 15);
      this.label46.TabIndex = 114;
      this.label46.Text = "Depass Timeout/Period";
      this.label46.TextAlign = ContentAlignment.MiddleRight;
      this.btnDepassWrite.Location = new Point(315, 17);
      this.btnDepassWrite.Name = "btnDepassWrite";
      this.btnDepassWrite.Size = new Size(60, 23);
      this.btnDepassWrite.TabIndex = 113;
      this.btnDepassWrite.Text = "Write";
      this.btnDepassWrite.UseVisualStyleBackColor = true;
      this.btnDepassWrite.Click += new System.EventHandler(this.btnDepassWrite_Click);
      this.btnDepassRead.Location = new Point(249, 17);
      this.btnDepassRead.Name = "btnDepassRead";
      this.btnDepassRead.Size = new Size(60, 23);
      this.btnDepassRead.TabIndex = 112;
      this.btnDepassRead.Text = "Read";
      this.btnDepassRead.UseVisualStyleBackColor = true;
      this.btnDepassRead.Click += new System.EventHandler(this.btnDepassRead_Click);
      this.tabMemory.Controls.Add((Control) this.groupBox1);
      this.tabMemory.Location = new Point(4, 22);
      this.tabMemory.Name = "tabMemory";
      this.tabMemory.Size = new Size(761, 326);
      this.tabMemory.TabIndex = 2;
      this.tabMemory.Text = "Memory";
      this.tabMemory.UseVisualStyleBackColor = true;
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
      this.tabMinoConnect.Size = new Size(761, 326);
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
      this.groupBox9.Location = new Point(300, 44);
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
      this.groupBox5.Controls.Add((Control) this.txtSerialnumberInputA);
      this.groupBox5.Controls.Add((Control) this.btnReceiveRadioPacket);
      this.groupBox5.Controls.Add((Control) this.label6);
      this.groupBox5.Location = new Point(11, 44);
      this.groupBox5.Name = "groupBox5";
      this.groupBox5.Size = new Size(283, 148);
      this.groupBox5.TabIndex = 28;
      this.groupBox5.TabStop = false;
      this.groupBox5.Text = "Receive";
      this.label9.Location = new Point(229, 71);
      this.label9.Name = "label9";
      this.label9.Size = new Size(49, 15);
      this.label9.TabIndex = 29;
      this.label9.Text = "sec";
      this.label9.TextAlign = ContentAlignment.MiddleLeft;
      this.label8.Location = new Point(54, 18);
      this.label8.Name = "label8";
      this.label8.Size = new Size(42, 15);
      this.label8.TabIndex = 24;
      this.label8.Text = "Mode:";
      this.label8.TextAlign = ContentAlignment.MiddleRight;
      this.txtTimeout.Location = new Point(102, 70);
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
      this.cboxRadioMode.Location = new Point(102, 17);
      this.cboxRadioMode.Name = "cboxRadioMode";
      this.cboxRadioMode.Size = new Size(121, 21);
      this.cboxRadioMode.TabIndex = 23;
      this.label7.Location = new Point(21, 70);
      this.label7.Name = "label7";
      this.label7.Size = new Size(75, 15);
      this.label7.TabIndex = 27;
      this.label7.Text = "Timeout:";
      this.label7.TextAlign = ContentAlignment.MiddleRight;
      this.txtSerialnumberInputA.Location = new Point(102, 44);
      this.txtSerialnumberInputA.Maximum = new Decimal(new int[4]
      {
        99999999,
        0,
        0,
        0
      });
      this.txtSerialnumberInputA.Name = "txtSerialnumberInputA";
      this.txtSerialnumberInputA.Size = new Size(120, 20);
      this.txtSerialnumberInputA.TabIndex = 26;
      this.btnReceiveRadioPacket.Location = new Point(35, 107);
      this.btnReceiveRadioPacket.Name = "btnReceiveRadioPacket";
      this.btnReceiveRadioPacket.Size = new Size(199, 23);
      this.btnReceiveRadioPacket.TabIndex = 0;
      this.btnReceiveRadioPacket.Text = "Receive radio packet";
      this.btnReceiveRadioPacket.UseVisualStyleBackColor = true;
      this.btnReceiveRadioPacket.Click += new System.EventHandler(this.btnReceiveRadioPacket_Click);
      this.label6.Location = new Point(0, 44);
      this.label6.Name = "label6";
      this.label6.Size = new Size(96, 15);
      this.label6.TabIndex = 25;
      this.label6.Text = "Serialnumber A:";
      this.label6.TextAlign = ContentAlignment.MiddleRight;
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
      this.groupBox6.ResumeLayout(false);
      this.txtTimeoutRadioReceive.EndInit();
      this.groupBox7.ResumeLayout(false);
      this.txtRadioTimeout.EndInit();
      this.txtRadioFrequencyOffset.EndInit();
      this.groupBox3.ResumeLayout(false);
      this.tabs.ResumeLayout(false);
      this.tabChannels.ResumeLayout(false);
      this.tabChannels.PerformLayout();
      this.txtUndersizeDiff.EndInit();
      this.txtOversizeDiff.EndInit();
      this.txtBurstLimit.EndInit();
      this.txtLower.EndInit();
      this.txtUpper.EndInit();
      this.txtUnleak.EndInit();
      this.txtLeak.EndInit();
      this.txtUndersizeLimit.EndInit();
      this.txtOversizeLimit.EndInit();
      this.txtBurstDiff.EndInit();
      this.txtBlock.EndInit();
      this.txtExponentA.EndInit();
      this.txtMantissaA.EndInit();
      this.txtMeterValueA.EndInit();
      this.tabConfig.ResumeLayout(false);
      this.tabConfig.PerformLayout();
      this.txtPulseOn.EndInit();
      this.txtPulsePeriod.EndInit();
      this.txtRadioList.EndInit();
      this.txtKeyMonth.EndInit();
      this.txtKeyDay.EndInit();
      this.tabMBus.ResumeLayout(false);
      this.txtDepassPeriod.EndInit();
      this.txtDepassTimeout.EndInit();
      this.tabMemory.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.tabMinoConnect.ResumeLayout(false);
      this.tabMinoConnect.PerformLayout();
      this.groupBox9.ResumeLayout(false);
      this.txtSendTestPacketPower.EndInit();
      this.groupBox5.ResumeLayout(false);
      this.txtTimeout.EndInit();
      this.txtSerialnumberInputA.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private delegate bool ActionSimpleMethod();
  }
}
