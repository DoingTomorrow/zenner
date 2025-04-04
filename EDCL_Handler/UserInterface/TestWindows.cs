// Decompiled with JetBrains decompiler
// Type: EDCL_Handler.UserInterface.TestWindows
// Assembly: EDCL_Handler, Version=2.2.10.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F3010E47-8885-4BE8-8551-D37B09710D3C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDCL_Handler.dll

using HandlerLib;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Markup;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace EDCL_Handler.UserInterface
{
  public class TestWindows : Window, IComponentConnector
  {
    private EDCL_HandlerWindowFunctions windowFunctions;
    private ProgressHandler progress;
    private CancellationTokenSource tokenSource;
    private ushort project_type;
    private byte cfg_device_mode;
    private ushort persistent_warning_flags;
    private ushort device_status;
    private ushort pulseFlowState;
    private long pulseTotalForwardCount64;
    private long pulseTotalCompare;
    private ushort cfg_pulse_leak_limit;
    private ushort cfg_pulse_leak_limit_save;
    private ushort cfg_pulse_unleak_limit;
    private ushort cfg_pulse_unleak_limit_save;
    private short cfg_pulse_leak_lower;
    private short cfg_pulse_leak_lower_save;
    private short cfg_pulse_leak_upper;
    private short cfg_pulse_leak_upper_save;
    private ushort pulseLeakageCounter;
    private ushort flow_check_counter;
    private ushort flow_check_counter_save;
    private ushort cfg_pulse_back_limit;
    private ushort cfg_pulse_back_limit_save;
    private ushort cfg_pulse_unback_limit;
    private ushort cfg_pulse_unback_limit_save;
    private ushort pulseBackflowCounter;
    private ushort cfg_pulse_block_limit;
    private ushort cfg_pulse_block_limit_save;
    private ushort pulseBlockageCounter;
    private ushort cfg_burst_limit;
    private ushort pulseBurstCounter;
    private ushort cfg_burst_diff;
    private ushort cfg_oversize_limit;
    private ushort cfg_oversize_limit_save;
    private ushort cfg_oversize_diff;
    private ushort pulseOversizeCounter;
    private ushort cfg_undersize_limit;
    private ushort cfg_undersize_limit_save;
    private ushort cfg_undersize_diff;
    private ushort pulseUndersizeCounter;
    private bool exitSendNext;
    internal TextBlock TextBlockStatus;
    internal System.Windows.Controls.ProgressBar ProgressBar1;
    internal System.Windows.Controls.TextBox TextBoxResult;
    internal StackPanel StackPanalButtons;
    internal System.Windows.Controls.Button ButtonCalibrate_VCC2;
    internal System.Windows.Controls.Button ButtonAddNewMap;
    internal System.Windows.Controls.Button ButtonSendNextLoRa;
    internal System.Windows.Controls.Button ButtonSendSP9T1;
    internal System.Windows.Controls.Button ButtonShowPersistWarnings;
    internal System.Windows.Controls.Button ButtonTestLeak;
    internal System.Windows.Controls.Button ButtonTestReverseFlow;
    internal System.Windows.Controls.Button ButtonTestBlockage;
    internal System.Windows.Controls.Button ButtonTestBurst;
    internal System.Windows.Controls.Button ButtonMeterIsOversized;
    internal System.Windows.Controls.Button ButtonMeterIsUndersized;
    internal System.Windows.Controls.Button ButtonGetConfigurationParameter;
    internal System.Windows.Controls.Button ButtonSetMode;
    internal System.Windows.Controls.Button ButtonHardwareTypeEditor;
    internal System.Windows.Controls.Button ButtonProtection;
    internal System.Windows.Controls.Button ButtonReadVersion;
    internal System.Windows.Controls.Button ButtonGetRadioVersion;
    internal System.Windows.Controls.Button ButtonSAP;
    internal System.Windows.Controls.Button ButtonTransmitModulatedCarrier;
    internal System.Windows.Controls.Button ButtonTransmitUnmodulatedCarrier;
    internal System.Windows.Controls.Button ButtonSetFrequencyIncrement;
    internal System.Windows.Controls.Button ButtonSendTestPacket;
    internal System.Windows.Controls.Button ButtonReceiveTestPacket;
    internal System.Windows.Controls.Button ButtonSendTestPacketViaMiCon;
    internal System.Windows.Controls.Button ButtonReceiveTestPacketViaMiCon;
    internal System.Windows.Controls.Button ButtonSetDeviceIdentification;
    internal System.Windows.Controls.Button ButtonReadSystemTime;
    internal System.Windows.Controls.Button ButtonWriteSystemTime;
    internal System.Windows.Controls.Button ButtonDisableIrDa;
    internal System.Windows.Controls.Button ButtonCompareConnectedAndWork;
    internal System.Windows.Controls.Button ButtonShowPublicKey;
    internal System.Windows.Controls.Button ButtonAccessRadioKey;
    internal System.Windows.Controls.Button ButtonGetCalibration;
    internal System.Windows.Controls.Button ButtonFillLogger;
    internal System.Windows.Controls.Button ButtonBatchFirmwareUpdate;
    internal System.Windows.Controls.Button ButtonBreak;
    private bool _contentLoaded;

    public TestWindows(Window owner, EDCL_HandlerWindowFunctions windowFunctions)
    {
      this.InitializeComponent();
      this.Owner = owner;
      this.windowFunctions = windowFunctions;
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.exitSendNext = false;
    }

    private void SetRunState()
    {
      this.tokenSource = new CancellationTokenSource();
      this.progress.Reset();
      this.StackPanalButtons.IsEnabled = false;
      this.ButtonBreak.IsEnabled = true;
      this.Cursor = System.Windows.Input.Cursors.Wait;
      this.TextBoxResult.Text = string.Empty;
    }

    private void SetStopState()
    {
      this.StackPanalButtons.IsEnabled = true;
      this.ButtonBreak.IsEnabled = false;
      this.Cursor = (System.Windows.Input.Cursor) null;
    }

    private void OnProgress(ProgressArg obj)
    {
      if (!this.CheckAccess())
      {
        this.Dispatcher.Invoke((Action) (() => this.OnProgress(obj)));
      }
      else
      {
        this.ProgressBar1.Value = obj.ProgressPercentage;
        this.TextBlockStatus.Text = obj.Message;
      }
    }

    private void ButtonBreak_Click(object sender, RoutedEventArgs e)
    {
      if (this.tokenSource == null)
        return;
      this.tokenSource.Cancel();
    }

    public async Task WriteDeviceAsyncWithoutReset(
      ProgressHandler progress,
      CancellationToken token)
    {
      if (this.windowFunctions.MyFunctions.myMeters.ConnectedMeter == null)
        throw new ArgumentNullException("ConnectedMeter");
      if (this.windowFunctions.MyFunctions.myMeters == null)
        throw new ArgumentNullException("WorkMeter");
      List<AddressRange> ranges = this.windowFunctions.MyFunctions.myMeters.WorkMeter.meterMemory.GetChangedDataRanges((DeviceMemory) this.windowFunctions.MyFunctions.myMeters.ConnectedMeter.meterMemory);
      if (ranges == null || ranges.Count <= 0)
      {
        ranges = (List<AddressRange>) null;
      }
      else
      {
        progress.Split(ranges.Count + 1);
        foreach (AddressRange range in ranges)
        {
          this.windowFunctions.MyFunctions.myMeters.WorkMeter.meterMemory.GarantMemoryAvailable(range);
          await this.windowFunctions.MyFunctions.cmd.Device.WriteMemoryAsync(range, (DeviceMemory) this.windowFunctions.MyFunctions.myMeters.WorkMeter.meterMemory, progress, token);
        }
        this.windowFunctions.MyFunctions.Clear();
        ranges = (List<AddressRange>) null;
      }
    }

    private void ShowFlowState(ushort project_type, ushort FlowState)
    {
      this.TextBoxResult.Text += "in FlowState set flags: ";
      if (project_type == (ushort) 25 || project_type == (ushort) 29)
      {
        if (FlowState == (ushort) 0)
        {
          System.Windows.Controls.TextBox textBoxResult = this.TextBoxResult;
          textBoxResult.Text = textBoxResult.Text + "none" + Environment.NewLine;
        }
        else
        {
          if (((uint) FlowState & 1U) > 0U)
            this.TextBoxResult.Text += "FLOW_STATE_BLOCK ";
          if (((uint) FlowState & 2U) > 0U)
            this.TextBoxResult.Text += "FLOW_STATE_LEAK ";
          if (((uint) FlowState & 4U) > 0U)
            this.TextBoxResult.Text += "FLOW_STATE_BACK ";
        }
      }
      this.TextBoxResult.Text += Environment.NewLine;
    }

    private void ShowPersistentWarningFlags(ushort project_type, ushort persistent_warning_flags)
    {
      if (project_type != (ushort) 25 && project_type != (ushort) 29 && project_type != (ushort) 31 && project_type != (ushort) 30)
      {
        System.Windows.Controls.TextBox textBoxResult = this.TextBoxResult;
        textBoxResult.Text = textBoxResult.Text + "wrong project number " + project_type.ToString();
      }
      else
      {
        System.Windows.Controls.TextBox textBoxResult = this.TextBoxResult;
        textBoxResult.Text = textBoxResult.Text + "in persistent_warning_flags set flags:(" + persistent_warning_flags.ToString() + ")";
        if (project_type == (ushort) 25 || project_type == (ushort) 29)
        {
          if (persistent_warning_flags == (ushort) 0)
          {
            this.TextBoxResult.Text += "none";
          }
          else
          {
            if (((uint) persistent_warning_flags & 1U) > 0U)
              this.TextBoxResult.Text += "WARNING_APP_BUSY ";
            if (((uint) persistent_warning_flags & 2U) > 0U)
              this.TextBoxResult.Text += "WARNING_ABNORMAL ";
            if (((uint) persistent_warning_flags & 4U) > 0U)
              this.TextBoxResult.Text += "WARNING_BATT_LOW ";
            if (((uint) persistent_warning_flags & 8U) > 0U)
              this.TextBoxResult.Text += "WARNING_PERMANENT_ERROR ";
            if (((uint) persistent_warning_flags & 16U) > 0U)
              this.TextBoxResult.Text += "WARNING_TEMPORARY_ERROR ";
            if (((uint) persistent_warning_flags & 32U) > 0U)
              this.TextBoxResult.Text += "WARNING_LEAK_A ";
            if (((uint) persistent_warning_flags & 64U) > 0U)
              this.TextBoxResult.Text += "WARNING_TAMPER_A ";
            if (((uint) persistent_warning_flags & 128U) > 0U)
              this.TextBoxResult.Text += "WARNING_REMOVAL_A ";
            if (((uint) persistent_warning_flags & 256U) > 0U)
              this.TextBoxResult.Text += "WARNING_OVERSIZE ";
            if (((uint) persistent_warning_flags & 512U) > 0U)
              this.TextBoxResult.Text += "WARNING_UNDERSIZE ";
            if (((uint) persistent_warning_flags & 1024U) > 0U)
              this.TextBoxResult.Text += "WARNING_BLOCK_A ";
            if (((uint) persistent_warning_flags & 2048U) > 0U)
              this.TextBoxResult.Text += "WARNING_BACKFLOW ";
            if (((uint) persistent_warning_flags & 4096U) > 0U)
              this.TextBoxResult.Text += "WARNING_BACKFLOW_A ";
            if (((uint) persistent_warning_flags & 8192U) > 0U)
              this.TextBoxResult.Text += "WARNING_LEAK ";
            if (((uint) persistent_warning_flags & 16384U) > 0U)
              this.TextBoxResult.Text += "WARNING_INTERFERE ";
            if (((uint) persistent_warning_flags & 32768U) > 0U)
              this.TextBoxResult.Text += "WARNING_BURST ";
          }
        }
        this.TextBoxResult.Text += Environment.NewLine;
      }
    }

    private void ShowDeviceStatus(ushort project_type, ushort device_status)
    {
      if (project_type != (ushort) 25 && project_type != (ushort) 29 && project_type != (ushort) 31 && project_type != (ushort) 30)
      {
        System.Windows.Controls.TextBox textBoxResult = this.TextBoxResult;
        textBoxResult.Text = textBoxResult.Text + "wrong project number " + project_type.ToString();
      }
      else
      {
        System.Windows.Controls.TextBox textBoxResult = this.TextBoxResult;
        textBoxResult.Text = textBoxResult.Text + "in device_status set flags:(" + device_status.ToString() + ")";
        if (project_type == (ushort) 25 || project_type == (ushort) 29)
        {
          if (device_status == (ushort) 0)
          {
            this.TextBoxResult.Text += "none";
          }
          else
          {
            if (((uint) device_status & 1U) > 0U)
              this.TextBoxResult.Text += "WARNING_APP_BUSY ";
            if (((uint) device_status & 2U) > 0U)
              this.TextBoxResult.Text += "WARNING_ABNORMAL ";
            if (((uint) device_status & 4U) > 0U)
              this.TextBoxResult.Text += "WARNING_BATT_LOW ";
            if (((uint) device_status & 8U) > 0U)
              this.TextBoxResult.Text += "WARNING_PERMANENT_ERROR ";
            if (((uint) device_status & 16U) > 0U)
              this.TextBoxResult.Text += " UNKNOWN_ERROR";
            if (((uint) device_status & 32U) > 0U)
              this.TextBoxResult.Text += "WARNING_LEAK_A ";
            if (((uint) device_status & 64U) > 0U)
              this.TextBoxResult.Text += "WARNING_TAMPER_A ";
            if (((uint) device_status & 128U) > 0U)
              this.TextBoxResult.Text += "WARNING_REMOVAL_A ";
            if (((uint) device_status & 256U) > 0U)
              this.TextBoxResult.Text += "WARNING_OVERSIZE ";
            if (((uint) device_status & 512U) > 0U)
              this.TextBoxResult.Text += "WARNING_UNDERSIZE ";
            if (((uint) device_status & 1024U) > 0U)
              this.TextBoxResult.Text += "WARNING_BLOCK_A ";
            if (((uint) device_status & 2048U) > 0U)
              this.TextBoxResult.Text += "WARNING_BACKFLOW ";
            if (((uint) device_status & 4096U) > 0U)
              this.TextBoxResult.Text += "WARNING_BACKFLOW_A ";
            if (((uint) device_status & 8192U) > 0U)
              this.TextBoxResult.Text += "WARNING_LEAK ";
            if (((uint) device_status & 16384U) > 0U)
              this.TextBoxResult.Text += "WARNING_INTERFERE ";
            if (((uint) device_status & 32768U) > 0U)
              this.TextBoxResult.Text += "WARNING_BURST ";
          }
        }
        this.TextBoxResult.Text += Environment.NewLine;
      }
    }

    private string GetPacketString(byte packet, byte sp9_subtype)
    {
      string packetString;
      switch (packet)
      {
        case 1:
          packetString = " SP1";
          break;
        case 2:
          packetString = " SP2";
          break;
        case 3:
          packetString = " SP3";
          break;
        case 4:
          packetString = " SP4";
          break;
        case 5:
          packetString = " START_JOINING";
          break;
        case 6:
          packetString = " SP0";
          break;
        case 7:
          packetString = " AP1";
          break;
        case 8:
          packetString = " SP9." + sp9_subtype.ToString();
          break;
        case 11:
          packetString = " SP0";
          break;
        case 12:
          packetString = " SP5";
          break;
        case 13:
          packetString = " SP6";
          break;
        case 14:
          packetString = " SP7";
          break;
        case 15:
          packetString = " SP8";
          break;
        case 16:
          packetString = " PACKET_ASYNC_TELEGRAM";
          break;
        case 17:
          packetString = " SP12";
          break;
        case 18:
          packetString = " PACKET_STRESS";
          break;
        default:
          packetString = " unknown";
          break;
      }
      return packetString;
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        object result = (object) null;
        EDCL_HandlerFunctions handler = this.windowFunctions.MyFunctions;
        if (sender == this.ButtonReadVersion)
        {
          FirmwareVersion firmwareVersion = await handler.ReadVersionAsync(this.progress, this.tokenSource.Token);
          result = (object) firmwareVersion;
          firmwareVersion = new FirmwareVersion();
        }
        else if (sender == this.ButtonCalibrate_VCC2)
        {
          DateTime start = DateTime.Now;
          await handler.Calibrate_VCC2(this.progress, this.tokenSource.Token);
          result = (object) ("Successfully calibrated! Expired: " + (DateTime.Now - start).TotalSeconds.ToString() + " sec");
        }
        else if (sender == this.ButtonShowPersistWarnings)
        {
          int num = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
          result = (object) num;
          ushort pulseFlowState = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseFlowState);
          ushort persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
          ushort project_type = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.project_type);
          this.ShowFlowState(project_type, pulseFlowState);
          this.ShowPersistentWarningFlags(project_type, persistent_warning_flags);
        }
        else
        {
          if (sender == this.ButtonTestLeak)
          {
            result = await this.TestLeak(result, handler);
            return;
          }
          if (sender == this.ButtonTestReverseFlow)
            result = await this.TestReverseFlow(result, handler);
          else if (sender == this.ButtonTestBlockage)
            result = await this.TestBlockage(result, handler);
          else if (sender == this.ButtonTestBurst)
            result = await this.TestBurst(result, handler);
          else if (sender == this.ButtonMeterIsOversized)
            result = await this.TestOversized(result, handler);
          else if (sender == this.ButtonMeterIsUndersized)
          {
            result = await this.TestUndersized(result, handler);
          }
          else
          {
            if (sender == this.ButtonSendNextLoRa)
            {
              bool not_ready = true;
              int packets_to_send = 0;
              int packets_to_send_old = 0;
              int max_packets_to_send = 3000;
              string Pfad = string.Empty;
              DateTime timepointOld = new DateTime(2000, 1, 1, 0, 0, 0);
              int send_hour = 0;
              int send_min = 0;
              int send_sec = 0;
              using (SaveFileDialog dlg = new SaveFileDialog())
              {
                dlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                if (dlg.ShowDialog().ToString().Equals("OK"))
                  Pfad = dlg.FileName;
              }
              int num = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
              result = (object) num;
              uint curr_value = await handler.cmd.MBusCmd.GetChannelValueAsync((byte) 0, this.progress, this.tokenSource.Token);
              AddressRange addressRange_radio_transmit = handler.myMeters.WorkMeter.meterMemory.GetParameterAddressRange(EDCL_Params.radio_transmit.ToString());
              do
              {
                try
                {
                  byte[] radio_transmit = await handler.cmd.Device.ReadMemoryAsync(this.progress, this.tokenSource.Token, addressRange_radio_transmit, (byte) 20);
                  byte data = radio_transmit[0];
                  byte packet = radio_transmit[1];
                  byte Sp9_subtyp = radio_transmit[2];
                  byte day = radio_transmit[6];
                  byte hour = radio_transmit[7];
                  byte minutes = radio_transmit[8];
                  byte seconds = radio_transmit[9];
                  Common32BitCommands.SystemTime _currentTime = await handler.cmd.Device.GetSystemTimeAsync(this.progress, this.tokenSource.Token);
                  DateTime currentTime = new DateTime(2000 + (int) _currentTime.Year, (int) _currentTime.Month, (int) _currentTime.Day, (int) _currentTime.Hour, (int) _currentTime.Min, (int) _currentTime.Sec);
                  DateTime localDate = DateTime.Now;
                  for (int i = 0; i <= 40 && send_hour == (int) hour && send_min == (int) minutes && send_sec == (int) seconds; ++i)
                  {
                    _currentTime = await handler.cmd.Device.GetSystemTimeAsync(this.progress, this.tokenSource.Token);
                    radio_transmit = await handler.cmd.Device.ReadMemoryAsync(this.progress, this.tokenSource.Token, addressRange_radio_transmit, (byte) 10);
                    currentTime = new DateTime(2000 + (int) _currentTime.Year, (int) _currentTime.Month, (int) _currentTime.Day, (int) _currentTime.Hour, (int) _currentTime.Min, (int) _currentTime.Sec);
                    localDate = DateTime.Now;
                  }
                  _currentTime = await handler.cmd.Device.GetSystemTimeAsync(this.progress, this.tokenSource.Token);
                  currentTime = new DateTime(2000 + (int) _currentTime.Year, (int) _currentTime.Month, (int) _currentTime.Day, (int) _currentTime.Hour, (int) _currentTime.Min, (int) _currentTime.Sec);
                  localDate = DateTime.Now;
                  System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
                  textBoxResult1.Text = textBoxResult1.Text + "Time: " + localDate.ToString() + " send data:" + data.ToString();
                  this.TextBoxResult.Text += Environment.NewLine;
                  System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
                  textBoxResult2.Text = textBoxResult2.Text + "radio_transmit = " + Util.ByteArrayToHexString(radio_transmit) + Environment.NewLine;
                  System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
                  textBoxResult3.Text = textBoxResult3.Text + "radio_transmit.day  " + day.ToString() + ", time " + hour.ToString() + ":" + minutes.ToString() + ":" + seconds.ToString() + Environment.NewLine;
                  System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
                  textBoxResult4.Text = textBoxResult4.Text + "radio_transmit.packet: " + packet.ToString() + this.GetPacketString(packet, Sp9_subtyp) + Environment.NewLine;
                  System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
                  System.Windows.Controls.TextBox textBox = textBoxResult5;
                  string[] strArray = new string[12]
                  {
                    textBoxResult5.Text,
                    "current device time day ",
                    currentTime.Day.ToString(),
                    ".",
                    currentTime.Month.ToString(),
                    ", time ",
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                  };
                  int num3 = currentTime.Hour;
                  strArray[6] = num3.ToString();
                  strArray[7] = ":";
                  num3 = currentTime.Minute;
                  strArray[8] = num3.ToString();
                  strArray[9] = ":";
                  num3 = currentTime.Second;
                  strArray[10] = num3.ToString();
                  strArray[11] = Environment.NewLine;
                  string str1 = string.Concat(strArray);
                  textBox.Text = str1;
                  if (day == (byte) 0)
                    day = Convert.ToByte(currentTime.Day);
                  DateTime sendTime = new DateTime(currentTime.Year, currentTime.Month, (int) day, (int) hour, (int) minutes, (int) seconds);
                  DateTime timeAfterSend = sendTime;
                  timeAfterSend.AddSeconds(8.0);
                  byte transmission_scenario = handler.myMeters.WorkMeter.meterMemory.GetData(EDCL_Params.cfg_communication_scenario)[0];
                  int SCENARIO3 = 203;
                  int SCENARIO4 = 204;
                  if ((int) transmission_scenario == SCENARIO3 || (int) transmission_scenario == SCENARIO4)
                  {
                    if (currentTime.Hour != timeAfterSend.Hour || currentTime.Hour == timeAfterSend.Hour && currentTime > timeAfterSend)
                    {
                      ++curr_value;
                      await handler.cmd.MBusCmd.SetChannelValueAsync((byte) 0, curr_value, this.progress, this.tokenSource.Token);
                      int prev_hour = currentTime.Hour;
                      int prev_day = currentTime.Day;
                      int prev_month = currentTime.Month;
                      currentTime.AddHours(1.0);
                      if (currentTime.Hour < prev_hour)
                      {
                        prev_day = currentTime.Day;
                        currentTime.AddDays(1.0);
                      }
                      if (currentTime.Day < prev_day)
                      {
                        prev_month = currentTime.Month;
                        currentTime.AddMonths(1);
                      }
                      if (currentTime.Month < prev_month)
                        currentTime.AddYears(1);
                      currentTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, 59, 59);
                      System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
                      textBoxResult6.Text = textBoxResult6.Text + "set system time = " + currentTime.ToString("s") + Environment.NewLine;
                      await handler.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(currentTime, (sbyte) 1), this.progress, this.tokenSource.Token);
                      this.TextBoxResult.CaretIndex = this.TextBoxResult.Text.Length;
                      Rect rect = this.TextBoxResult.GetRectFromCharacterIndex(this.TextBoxResult.CaretIndex);
                      this.TextBoxResult.ScrollToHorizontalOffset(rect.Right);
                      if (!this.exitSendNext)
                        await Task.Delay(10000);
                      rect = new Rect();
                    }
                    else
                    {
                      if (data == (byte) 1)
                        num3 = packets_to_send++;
                      System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
                      textBoxResult7.Text = textBoxResult7.Text + "set time to send" + Environment.NewLine;
                      not_ready = false;
                      sendTime = sendTime.AddSeconds(-5.0);
                      System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
                      textBoxResult8.Text = textBoxResult8.Text + "set system time = " + sendTime.ToString("s") + Environment.NewLine;
                      await handler.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(sendTime, (sbyte) 1), this.progress, this.tokenSource.Token);
                      this.TextBoxResult.CaretIndex = this.TextBoxResult.Text.Length;
                      Rect rect = this.TextBoxResult.GetRectFromCharacterIndex(this.TextBoxResult.CaretIndex);
                      this.TextBoxResult.ScrollToHorizontalOffset(rect.Right);
                      System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
                      textBoxResult9.Text = textBoxResult9.Text + "packet Nr " + packets_to_send.ToString() + Environment.NewLine;
                      if (!this.exitSendNext)
                        await Task.Delay(14000);
                      rect = new Rect();
                    }
                  }
                  else if (currentTime < sendTime && packet == (byte) 0)
                  {
                    System.Windows.Controls.TextBox textBoxResult10 = this.TextBoxResult;
                    textBoxResult10.Text = textBoxResult10.Text + "set time to send" + Environment.NewLine;
                    not_ready = false;
                    sendTime = sendTime.AddSeconds(-1.0);
                    System.Windows.Controls.TextBox textBoxResult11 = this.TextBoxResult;
                    textBoxResult11.Text = textBoxResult11.Text + "set system time = " + sendTime.ToString("s") + Environment.NewLine;
                    await handler.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(sendTime, (sbyte) 1), this.progress, this.tokenSource.Token);
                    this.TextBoxResult.CaretIndex = this.TextBoxResult.Text.Length;
                    Rect rect = this.TextBoxResult.GetRectFromCharacterIndex(this.TextBoxResult.CaretIndex);
                    this.TextBoxResult.ScrollToHorizontalOffset(rect.Right);
                    System.Windows.Controls.TextBox textBoxResult12 = this.TextBoxResult;
                    textBoxResult12.Text = textBoxResult12.Text + "packet Nr " + packets_to_send.ToString() + Environment.NewLine;
                    await Task.Delay(4000);
                    rect = new Rect();
                  }
                  else if (currentTime > timeAfterSend)
                  {
                    if (currentTime.Day != sendTime.Day || currentTime > sendTime)
                    {
                      if (timepointOld <= sendTime && timepointOld.Day == sendTime.Day)
                      {
                        if (data == (byte) 1)
                          num3 = packets_to_send++;
                        System.Windows.Controls.TextBox textBoxResult13 = this.TextBoxResult;
                        textBoxResult13.Text = textBoxResult13.Text + "LoRa send timing is missed -> do not set midnight but wait to get the device time to end sending" + Environment.NewLine;
                        await Task.Delay(4000);
                        sendTime = sendTime.AddSeconds(5.0);
                        System.Windows.Controls.TextBox textBoxResult14 = this.TextBoxResult;
                        textBoxResult14.Text = textBoxResult14.Text + "set system time = " + sendTime.ToString("s") + Environment.NewLine;
                        System.Windows.Controls.TextBox textBoxResult15 = this.TextBoxResult;
                        textBoxResult15.Text = textBoxResult15.Text + "packet Nr " + packets_to_send.ToString() + Environment.NewLine;
                        timepointOld = sendTime;
                      }
                      else
                      {
                        ++curr_value;
                        await handler.cmd.MBusCmd.SetChannelValueAsync((byte) 0, curr_value, this.progress, this.tokenSource.Token);
                        timepointOld = sendTime;
                        System.Windows.Controls.TextBox textBoxResult16 = this.TextBoxResult;
                        textBoxResult16.Text = textBoxResult16.Text + "set midnight" + Environment.NewLine;
                        sendTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 23, 59, 59);
                        currentTime = currentTime.AddDays(1.0);
                        System.Windows.Controls.TextBox textBoxResult17 = this.TextBoxResult;
                        textBoxResult17.Text = textBoxResult17.Text + "set system time = " + sendTime.ToString("s") + Environment.NewLine;
                        await handler.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(sendTime, (sbyte) 1), this.progress, this.tokenSource.Token);
                        this.TextBoxResult.CaretIndex = this.TextBoxResult.Text.Length;
                        Rect rect = this.TextBoxResult.GetRectFromCharacterIndex(this.TextBoxResult.CaretIndex);
                        this.TextBoxResult.ScrollToHorizontalOffset(rect.Right);
                        await Task.Delay(14000);
                        rect = new Rect();
                      }
                    }
                    else
                    {
                      timepointOld = sendTime;
                      if (data == (byte) 1)
                        num3 = packets_to_send++;
                      System.Windows.Controls.TextBox textBoxResult18 = this.TextBoxResult;
                      textBoxResult18.Text = textBoxResult18.Text + "set time to send" + Environment.NewLine;
                      not_ready = false;
                      sendTime = sendTime.AddSeconds(-10.0);
                      System.Windows.Controls.TextBox textBoxResult19 = this.TextBoxResult;
                      textBoxResult19.Text = textBoxResult19.Text + "set system time = " + sendTime.ToString("s") + Environment.NewLine;
                      await handler.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(sendTime, (sbyte) 1), this.progress, this.tokenSource.Token);
                      this.TextBoxResult.CaretIndex = this.TextBoxResult.Text.Length;
                      Rect rect = this.TextBoxResult.GetRectFromCharacterIndex(this.TextBoxResult.CaretIndex);
                      this.TextBoxResult.ScrollToHorizontalOffset(rect.Right);
                      System.Windows.Controls.TextBox textBoxResult20 = this.TextBoxResult;
                      textBoxResult20.Text = textBoxResult20.Text + "packet Nr " + packets_to_send.ToString() + Environment.NewLine;
                      rect = new Rect();
                    }
                  }
                  else
                  {
                    if (data == (byte) 1)
                      num3 = packets_to_send++;
                    System.Windows.Controls.TextBox textBoxResult21 = this.TextBoxResult;
                    textBoxResult21.Text = textBoxResult21.Text + "set time to send" + Environment.NewLine;
                    not_ready = false;
                    sendTime = sendTime.AddSeconds(-2.0);
                    System.Windows.Controls.TextBox textBoxResult22 = this.TextBoxResult;
                    textBoxResult22.Text = textBoxResult22.Text + "set system time = " + sendTime.ToString("s") + Environment.NewLine;
                    await handler.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(sendTime, (sbyte) 1), this.progress, this.tokenSource.Token);
                    this.TextBoxResult.CaretIndex = this.TextBoxResult.Text.Length;
                    Rect rect = this.TextBoxResult.GetRectFromCharacterIndex(this.TextBoxResult.CaretIndex);
                    this.TextBoxResult.ScrollToHorizontalOffset(rect.Right);
                    System.Windows.Controls.TextBox textBoxResult23 = this.TextBoxResult;
                    textBoxResult23.Text = textBoxResult23.Text + "packet Nr " + packets_to_send.ToString() + Environment.NewLine;
                    await Task.Delay(10000);
                    rect = new Rect();
                  }
                  this.TextBoxResult.Text += Environment.NewLine;
                  if (packets_to_send != 0 && packets_to_send % 10 == 0 && packets_to_send_old != packets_to_send || this.exitSendNext)
                  {
                    packets_to_send_old = packets_to_send;
                    try
                    {
                      string str = this.TextBoxResult.ToString();
                      FileStream fileStream = File.Open(Pfad, FileMode.Append, FileAccess.Write);
                      StreamWriter fileWriter = new StreamWriter((Stream) fileStream);
                      fileWriter.WriteLine(str);
                      fileWriter.Flush();
                      fileWriter.Close();
                      str = (string) null;
                      fileStream = (FileStream) null;
                      fileWriter = (StreamWriter) null;
                    }
                    catch (IOException ex)
                    {
                      Console.WriteLine((object) ex);
                    }
                    this.TextBoxResult.Text = "";
                  }
                  send_hour = (int) day;
                  send_min = (int) hour;
                  send_sec = (int) minutes;
                  radio_transmit = (byte[]) null;
                  _currentTime = (Common32BitCommands.SystemTime) null;
                }
                catch (Exception ex)
                {
                  System.Windows.Controls.TextBox textBoxResult = this.TextBoxResult;
                  textBoxResult.Text = textBoxResult.Text + ex.Message + Environment.NewLine;
                }
              }
              while ((not_ready || packets_to_send < max_packets_to_send) && !this.exitSendNext);
              return;
            }
            if (sender == this.ButtonSendSP9T1)
            {
              int num1 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
              result = (object) num1;
              byte SP9_send_day = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.SP9_send_day);
              this.TextBoxResult.Text = "SP9_send_day = " + SP9_send_day.ToString() + Environment.NewLine;
              byte[] lora_start_date = handler.myMeters.WorkMeter.meterMemory.GetData(EDCL_Params.lora_start_date);
              DateTime? startDate1 = Util.ConvertToDate_MBus_CP16_TypeG(lora_start_date, 0);
              DateTime? startDate2 = Util.ConvertToDate_MBus_CP16_TypeG(lora_start_date, 2);
              System.Windows.Controls.TextBox textBoxResult24 = this.TextBoxResult;
              textBoxResult24.Text = textBoxResult24.Text + "lora_start_date 1 = " + startDate1.ToString() + Environment.NewLine;
              System.Windows.Controls.TextBox textBoxResult25 = this.TextBoxResult;
              textBoxResult25.Text = textBoxResult25.Text + "lora_start_date 2 = " + startDate2.ToString() + Environment.NewLine;
              DateTime timepoint = new DateTime(2018, startDate1.Value.Month, (int) SP9_send_day, 23, 59, 55);
              timepoint = timepoint.AddMonths(1);
              timepoint = timepoint.AddDays(-1.0);
              System.Windows.Controls.TextBox textBoxResult26 = this.TextBoxResult;
              textBoxResult26.Text = textBoxResult26.Text + "set system time = " + timepoint.ToString("s") + Environment.NewLine;
              await handler.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(timepoint, (sbyte) 1), this.progress, this.tokenSource.Token);
              await Task.Delay(14000);
              int num2 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
              result = (object) num2;
              byte[] time_to_send_data = handler.myMeters.WorkMeter.meterMemory.GetData(EDCL_Params.time_to_send_data);
              System.Windows.Controls.TextBox textBoxResult27 = this.TextBoxResult;
              textBoxResult27.Text = textBoxResult27.Text + "time_to_send_data = " + Util.ByteArrayToHexString(time_to_send_data) + Environment.NewLine;
              System.Windows.Controls.TextBox textBoxResult28 = this.TextBoxResult;
              textBoxResult28.Text = textBoxResult28.Text + "time_to_send_data.day = " + time_to_send_data[0].ToString() + Environment.NewLine;
              System.Windows.Controls.TextBox textBoxResult29 = this.TextBoxResult;
              textBoxResult29.Text = textBoxResult29.Text + "time_to_send_data.hour = " + time_to_send_data[1].ToString() + Environment.NewLine;
              System.Windows.Controls.TextBox textBoxResult30 = this.TextBoxResult;
              textBoxResult30.Text = textBoxResult30.Text + "time_to_send_data.minutes = " + time_to_send_data[2].ToString() + Environment.NewLine;
              System.Windows.Controls.TextBox textBoxResult31 = this.TextBoxResult;
              textBoxResult31.Text = textBoxResult31.Text + "time_to_send_data.seconds = " + time_to_send_data[3].ToString() + Environment.NewLine;
              timepoint = timepoint.AddSeconds(5.0);
              DateTime timepoint2 = new DateTime(timepoint.Year, timepoint.Month, (int) time_to_send_data[0], (int) time_to_send_data[1], (int) time_to_send_data[2], (int) time_to_send_data[3]);
              timepoint2 = timepoint2.AddSeconds(-4.0);
              System.Windows.Controls.TextBox textBoxResult32 = this.TextBoxResult;
              textBoxResult32.Text = textBoxResult32.Text + "set system time = " + timepoint2.ToString("s") + Environment.NewLine;
              await handler.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(timepoint2, (sbyte) 1), this.progress, this.tokenSource.Token);
              lora_start_date = (byte[]) null;
              startDate1 = new DateTime?();
              startDate2 = new DateTime?();
              time_to_send_data = (byte[]) null;
            }
            else if (sender == this.ButtonGetCalibration)
            {
              StringBuilder sb = new StringBuilder();
              Calibration calib = handler.GetCalibration();
              sb.AppendLine(calib.ToString());
              if (calib != Calibration.SUCCESSFUL)
              {
                CalibrationFault calibFault = handler.GetCalibrationFault();
                sb.AppendLine(calibFault.ToString());
              }
              result = (object) sb.ToString();
              sb = (StringBuilder) null;
            }
            else if (sender == this.ButtonHardwareTypeEditor)
            {
              uint? firmwareVersion1 = new uint?();
              try
              {
                FirmwareVersion firmwareVersion2 = await handler.ReadVersionAsync(this.progress, this.tokenSource.Token);
                FirmwareVersion version = firmwareVersion2;
                firmwareVersion2 = new FirmwareVersion();
                firmwareVersion1 = new uint?(version.Version);
                version = new FirmwareVersion();
              }
              catch
              {
              }
              HardwareTypeEditor dlg = new HardwareTypeEditor(new string[17]
              {
                "EDC_LoRa868",
                "EDC_LoRa470",
                "Micro_LoRa",
                "Micro_wMBus",
                "EDC_NBIoT",
                "Micro_LoRa_LL",
                "Micro_wMBus_LL",
                "Micro_radio3_LoRa",
                "EDC_wMBus",
                "EDC_NBIoT_LCSW",
                "EDC_NBIoT_YJSW",
                "EDC_NBIoT_FSNH",
                "EDC_NBIoT_XM",
                "EDC_NBIoT_Israel",
                "EDC_NBIoT_TaiWan",
                "EDC_LoRa915_US",
                "EDC_LoRa915_BR"
              }, firmwareVersion1);
              dlg.ShowDialog();
              firmwareVersion1 = new uint?();
              dlg = (HardwareTypeEditor) null;
            }
            else if (sender == this.ButtonGetRadioVersion)
            {
              ushort radioVersionAsync = await handler.cmd.Radio.GetRadioVersionAsync(this.progress, this.tokenSource.Token);
              result = (object) radioVersionAsync;
            }
            else if (sender == this.ButtonAccessRadioKey)
              result = (object) handler.AccessRadioKey_IsOK();
            else if (sender == this.ButtonTransmitModulatedCarrier)
              await handler.TransmitModulatedCarrierAsync((ushort) 5, this.progress, this.tokenSource.Token);
            else if (sender == this.ButtonTransmitUnmodulatedCarrier)
              await handler.TransmitUnmodulatedCarrierAsync((ushort) 5, this.progress, this.tokenSource.Token);
            else if (sender == this.ButtonSetFrequencyIncrement)
              await handler.SetFrequencyIncrementAsync(this.progress, this.tokenSource.Token, -55);
            else if (sender == this.ButtonDisableIrDa)
              await handler.DisableIrDaAsync(this.progress, this.tokenSource.Token);
            else if (sender == this.ButtonSendTestPacket)
            {
              byte[] arbitraryData = new byte[28];
              for (int i = 0; i < arbitraryData.Length; ++i)
                arbitraryData[i] = (byte) 85;
              await handler.SendTestPacketAsync(this.progress, this.tokenSource.Token, (ushort) 1, (ushort) 5, 11111111U, arbitraryData, "0FF0");
              arbitraryData = (byte[]) null;
            }
            else if (sender == this.ButtonReceiveTestPacket)
            {
              double? nullable = await handler.ReceiveTestPacketAsync(this.progress, this.tokenSource.Token, (byte) 200, 11111111U, "0FF0");
              result = (object) nullable;
              nullable = new double?();
            }
            else if (sender == this.ButtonReceiveTestPacketViaMiCon)
              ReceiveTestPacketMiConWindow.Show((Window) this);
            else if (sender == this.ButtonSendTestPacketViaMiCon)
              SendTestPacketMiConWindow.Show((Window) this);
            else if (sender == this.ButtonSetDeviceIdentification)
            {
              int num = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
              DeviceIdentification di = handler.GetDeviceIdentification();
              di.MeterID = new uint?(100000009U);
              di.HardwareTypeID = new uint?(312U);
              di.MeterInfoID = new uint?(2U);
              di.MeterTypeID = new uint?(3U);
              di.BaseTypeID = new uint?(4U);
              di.SAP_MaterialNumber = new uint?(5U);
              di.SAP_ProductionOrderNumber = "6";
              di.FullSerialNumber = "6ZRI8801234567";
              di.ID_BCD = new uint?(19088743U);
              di = (DeviceIdentification) null;
            }
            else if (sender == this.ButtonReadSystemTime)
            {
              Common32BitCommands.SystemTime systemTime = await handler.ReadSystemTimeAsync(this.progress, this.tokenSource.Token);
              result = (object) systemTime;
              systemTime = (Common32BitCommands.SystemTime) null;
            }
            else if (sender == this.ButtonSetMode)
              await handler.SetModeAsync(this.progress, this.tokenSource.Token, Mode.OPERATION_MODE);
            else if (sender == this.ButtonWriteSystemTime)
            {
              Common32BitCommands.SystemTime time = new Common32BitCommands.SystemTime(DateTime.Now, (sbyte) 4);
              await handler.WriteSystemTimeAsync(this.progress, this.tokenSource.Token, time);
              time = (Common32BitCommands.SystemTime) null;
            }
            else if (sender == this.ButtonGetConfigurationParameter)
            {
              if (handler.myMeters.WorkMeter == null)
              {
                int num = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
              }
              SortedList<OverrideID, ConfigurationParameter> cfg = handler.GetConfigurationParameters(0);
              if (cfg != null)
              {
                foreach (KeyValuePair<OverrideID, ConfigurationParameter> keyValuePair in cfg)
                {
                  KeyValuePair<OverrideID, ConfigurationParameter> p = keyValuePair;
                  this.TextBoxResult.Text += p.ToString();
                  this.TextBoxResult.Text += Environment.NewLine;
                  p = new KeyValuePair<OverrideID, ConfigurationParameter>();
                }
              }
              cfg = (SortedList<OverrideID, ConfigurationParameter>) null;
            }
            else if (sender == this.ButtonAddNewMap)
              this.windowFunctions.OpenMapClassManagerToReadFromMapFile();
            else if (sender == this.ButtonProtection)
              this.windowFunctions.ShowProtectionWindow();
            else if (sender == this.ButtonCompareConnectedAndWork)
              handler.myMeters.CompareConnectedAndWork();
            else if (sender != this.ButtonShowPublicKey)
            {
              if (sender == this.ButtonFillLogger)
                result = await this.fillLogger(result, handler);
              else
                result = (object) "Not supported button";
            }
          }
        }
        if (result != null)
          this.TextBoxResult.Text = !(result is byte[]) ? result.ToString() : Utility.ByteArrayToHexString((byte[]) result);
        result = (object) null;
        handler = (EDCL_HandlerFunctions) null;
      }
      catch (Exception ex)
      {
        this.TextBoxResult.Text = ex.Message;
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async Task<object> TestUndersized(object result, EDCL_HandlerFunctions handler)
    {
      uint WMBUS_STATUS_UNDERSIZE = 512;
      this.TextBoxResult.Text = "Test undersized" + Environment.NewLine;
      object obj1 = await this.ReadUndersizedParameter(result, handler);
      result = obj1;
      obj1 = (object) null;
      this.flow_check_counter_save = (ushort) 450;
      this.cfg_oversize_limit_save = this.cfg_oversize_limit;
      if (this.cfg_device_mode != (byte) 8 && this.cfg_device_mode != (byte) 9)
      {
        System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
        textBoxResult1.Text = textBoxResult1.Text + "1. Current values" + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
        textBoxResult2.Text = textBoxResult2.Text + "cfg_undersize_diff = " + this.cfg_undersize_diff.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
        textBoxResult3.Text = textBoxResult3.Text + "cfg_undersize_limit = " + this.cfg_undersize_limit.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
        textBoxResult4.Text = textBoxResult4.Text + "pulseUndersizeCounter = " + this.pulseUndersizeCounter.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
        textBoxResult5.Text = textBoxResult5.Text + "flow_check_counter = " + this.flow_check_counter.ToString() + Environment.NewLine;
        this.TextBoxResult.Text += Environment.NewLine;
        this.ShowFlowState(this.project_type, this.pulseFlowState);
        this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
        this.ShowDeviceStatus(this.project_type, this.device_status);
        this.TextBoxResult.Text += Environment.NewLine;
        object obj2 = await this.ReadUndersizedParameter(result, handler);
        result = obj2;
        obj2 = (object) null;
        System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
        textBoxResult6.Text = textBoxResult6.Text + "2. Pre condition" + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
        textBoxResult7.Text = textBoxResult7.Text + "pulseUndersizeCounter = " + this.pulseUndersizeCounter.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
        textBoxResult8.Text = textBoxResult8.Text + "flow_check_counter = " + this.flow_check_counter.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
        textBoxResult9.Text = textBoxResult9.Text + "pulseTotalForwardCount64 = " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult10 = this.TextBoxResult;
        textBoxResult10.Text = textBoxResult10.Text + "pulseTotalCompare = " + this.pulseTotalCompare.ToString() + Environment.NewLine;
        this.ShowFlowState(this.project_type, this.pulseFlowState);
        this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
        this.ShowDeviceStatus(this.project_type, this.device_status);
        this.TextBoxResult.Text += Environment.NewLine;
        string Title = string.Format("set pulse difference x (x > " + this.cfg_undersize_diff.ToString() + "(undersize test) x <= " + this.cfg_undersize_diff.ToString() + "(not undersize)");
        string value = InputWindow.Show(Title, "");
        long flowdiff = Convert.ToInt64(value);
        if (flowdiff > (long) this.cfg_undersize_diff)
        {
          if (((int) this.persistent_warning_flags & (int) WMBUS_STATUS_UNDERSIZE) != (int) WMBUS_STATUS_UNDERSIZE)
          {
            object obj3 = await this.undersized_check(result, handler, WMBUS_STATUS_UNDERSIZE, flowdiff);
            result = obj3;
            obj3 = (object) null;
          }
          else
          {
            object obj4 = await this.undersized_no_change(result, handler, WMBUS_STATUS_UNDERSIZE, flowdiff);
            result = obj4;
            obj4 = (object) null;
          }
        }
        else if (((int) this.persistent_warning_flags & (int) WMBUS_STATUS_UNDERSIZE) != (int) WMBUS_STATUS_UNDERSIZE)
        {
          object obj5 = await this.undersized_not_detected(result, handler, WMBUS_STATUS_UNDERSIZE, flowdiff);
          result = obj5;
          obj5 = (object) null;
        }
        else
        {
          object obj6 = await this.undersized_no_change(result, handler, WMBUS_STATUS_UNDERSIZE, flowdiff);
          result = obj6;
          obj6 = (object) null;
        }
        System.Windows.Controls.TextBox textBoxResult11 = this.TextBoxResult;
        textBoxResult11.Text = textBoxResult11.Text + "write parameter back " + Environment.NewLine;
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter_save);
        await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
        Title = (string) null;
        value = (string) null;
      }
      else
        this.TextBoxResult.Text = "in Mode " + this.cfg_device_mode.ToString() + " smart funktions are not available, please change mode to 1 or 0" + Environment.NewLine;
      return result;
    }

    private async Task<object> ReadUndersizedParameter(object result, EDCL_HandlerFunctions handler)
    {
      int num = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num;
      System.Windows.Controls.TextBox textBoxResult = this.TextBoxResult;
      textBoxResult.Text = textBoxResult.Text + "Test Undersized" + Environment.NewLine + Environment.NewLine;
      this.cfg_undersize_limit = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_undersize_limit);
      this.cfg_undersize_limit_save = this.cfg_undersize_limit;
      this.cfg_undersize_diff = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_undersize_diff);
      this.pulseUndersizeCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseUndersizeCounter);
      this.flow_check_counter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.flow_check_counter);
      this.flow_check_counter_save = this.flow_check_counter;
      this.pulseFlowState = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseFlowState);
      this.persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
      this.device_status = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.device_status);
      this.project_type = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.project_type);
      this.cfg_device_mode = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.cfg_device_mode);
      return result;
    }

    private async Task<object> undersized_no_change(
      object result,
      EDCL_HandlerFunctions handler,
      uint WMBUS_STATUS_UNDERSIZE,
      long flowdiff)
    {
      this.flow_check_counter = (ushort) 1;
      ushort pulseUndersizeCounterSave = this.pulseUndersizeCounter;
      ushort persistent_warning_flagsSave = this.persistent_warning_flags;
      System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
      textBoxResult1.Text = textBoxResult1.Text + "3. Test no change" + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
      textBoxResult2.Text = textBoxResult2.Text + "flow_check_counter set to " + this.flow_check_counter.ToString() + Environment.NewLine + Environment.NewLine;
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
      await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
      await Task.Delay(((int) this.flow_check_counter + 1) * 2000);
      System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
      textBoxResult3.Text = textBoxResult3.Text + "Test no change" + Environment.NewLine;
      int num1 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num1;
      for (int i = 0; i <= 3; ++i)
      {
        this.pulseUndersizeCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseUndersizeCounter);
        this.flow_check_counter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.flow_check_counter);
        this.pulseTotalForwardCount64 = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64);
        System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
        textBoxResult4.Text = textBoxResult4.Text + "pulseUndersizeCounter = " + this.pulseUndersizeCounter.ToString() + " cfg_undersize_limit = " + this.cfg_undersize_limit.ToString() + Environment.NewLine;
        this.ShowFlowState(this.project_type, this.pulseFlowState);
        this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
        this.ShowDeviceStatus(this.project_type, this.device_status);
        this.flow_check_counter = (ushort) 1;
        this.pulseTotalForwardCount64 = flowdiff * 4L + this.pulseTotalCompare;
        System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
        textBoxResult5.Text = textBoxResult5.Text + "pulseTotalForwardCount64 set to " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
        textBoxResult6.Text = textBoxResult6.Text + "Change in Meter Reading: " + ((this.pulseTotalForwardCount64 - this.pulseTotalCompare) / 4L).ToString() + Environment.NewLine;
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64, this.pulseTotalForwardCount64);
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
        await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
        await Task.Delay(((int) this.flow_check_counter + 1) * 4000);
        int num2 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
        result = (object) num2;
        this.pulseTotalCompare = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalCompare);
        this.pulseUndersizeCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseUndersizeCounter);
        this.persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
        this.device_status = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.device_status);
        System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
        textBoxResult7.Text = textBoxResult7.Text + "after flow check pulseTotalCompare =  " + this.pulseTotalCompare.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
        textBoxResult8.Text = textBoxResult8.Text + "after flow check pulseUndersizeCounter = " + this.pulseUndersizeCounter.ToString() + " cfg_undersize_limit = " + this.cfg_undersize_limit.ToString() + Environment.NewLine;
        this.TextBoxResult.Text += Environment.NewLine;
      }
      System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
      textBoxResult9.Text = textBoxResult9.Text + "after flow check pulseUndersizeCounter = " + this.pulseUndersizeCounter.ToString() + " cfg_undersize_limit = " + this.cfg_undersize_limit.ToString() + Environment.NewLine;
      this.ShowFlowState(this.project_type, this.pulseFlowState);
      this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
      this.ShowDeviceStatus(this.project_type, this.device_status);
      string ResultText = "";
      ResultText = (int) this.persistent_warning_flags != (int) persistent_warning_flagsSave || (int) this.pulseUndersizeCounter != (int) pulseUndersizeCounterSave ? "test FAILED" : "test PASSED";
      System.Windows.Controls.TextBox textBoxResult10 = this.TextBoxResult;
      textBoxResult10.Text = textBoxResult10.Text + Environment.NewLine + ResultText + Environment.NewLine;
      object obj = result;
      ResultText = (string) null;
      return obj;
    }

    private async Task<object> undersized_not_detected(
      object result,
      EDCL_HandlerFunctions handler,
      uint WMBUS_STATUS_UNDERSIZE,
      long flowdiff)
    {
      this.pulseUndersizeCounter = (ushort) ((uint) this.cfg_undersize_limit - 1U);
      System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
      textBoxResult1.Text = textBoxResult1.Text + "3. Test not Undersized" + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
      textBoxResult2.Text = textBoxResult2.Text + "set pulseUndersizeCounter to a value != cfg_undersize_limit." + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
      textBoxResult3.Text = textBoxResult3.Text + "pulseUndersizeCounter set to " + this.pulseUndersizeCounter.ToString() + Environment.NewLine;
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.pulseUndersizeCounter, this.pulseUndersizeCounter);
      await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
      System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
      textBoxResult4.Text = textBoxResult4.Text + "Test not Undersized" + Environment.NewLine;
      int num1 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num1;
      this.pulseUndersizeCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseUndersizeCounter);
      this.flow_check_counter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.flow_check_counter);
      this.pulseTotalForwardCount64 = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64);
      this.pulseTotalCompare = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalCompare);
      System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
      textBoxResult5.Text = textBoxResult5.Text + "pulseUndersizeCounter = " + this.pulseUndersizeCounter.ToString() + " cfg_undersize_limit = " + this.cfg_undersize_limit.ToString() + Environment.NewLine;
      this.ShowFlowState(this.project_type, this.pulseFlowState);
      this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
      this.ShowDeviceStatus(this.project_type, this.device_status);
      this.pulseTotalForwardCount64 = flowdiff * 4L + this.pulseTotalCompare;
      System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
      textBoxResult6.Text = textBoxResult6.Text + "pulseTotalForwardCount64 set to " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
      textBoxResult7.Text = textBoxResult7.Text + "Change in Meter Reading: " + ((this.pulseTotalForwardCount64 - this.pulseTotalCompare) / 4L).ToString() + Environment.NewLine;
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64, this.pulseTotalForwardCount64);
      await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
      int num2 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num2;
      this.flow_check_counter = (ushort) 1;
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
      await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
      await Task.Delay(((int) this.flow_check_counter + 1) * 4000);
      int num3 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num3;
      this.pulseTotalCompare = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalCompare);
      this.cfg_undersize_limit = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_undersize_limit);
      this.pulseUndersizeCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseUndersizeCounter);
      this.persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
      this.device_status = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.device_status);
      System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
      textBoxResult8.Text = textBoxResult8.Text + "after flow check pulseTotalCompare =  " + this.pulseTotalCompare.ToString() + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
      textBoxResult9.Text = textBoxResult9.Text + "after flow check pulseUndersizeCounter = " + this.pulseUndersizeCounter.ToString() + " cfg_undersize_limit = " + this.cfg_undersize_limit.ToString() + Environment.NewLine;
      this.TextBoxResult.Text += Environment.NewLine;
      if ((int) this.pulseUndersizeCounter == (int) this.cfg_undersize_limit)
      {
        System.Windows.Controls.TextBox textBoxResult10 = this.TextBoxResult;
        textBoxResult10.Text = textBoxResult10.Text + "pulseUndersizeCounter = cfg_undersize_limit => Success UNDERSIZE NOT DETECTED!!!" + Environment.NewLine;
      }
      else
      {
        System.Windows.Controls.TextBox textBoxResult11 = this.TextBoxResult;
        textBoxResult11.Text = textBoxResult11.Text + "pulseUndersizeCounter != cfg_undersize_limit => fail UNDERSIZE NOT DETECTED" + Environment.NewLine;
      }
      this.ShowFlowState(this.project_type, this.pulseFlowState);
      this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
      this.ShowDeviceStatus(this.project_type, this.device_status);
      string ResultText = "";
      ResultText = (int) this.pulseUndersizeCounter != (int) this.cfg_undersize_limit ? "test FAILED pulseUndersizeCounter != cfg_undersize_limit" : "test PASSED pulseUndersizeCounter = cfg_undersize_limit";
      System.Windows.Controls.TextBox textBoxResult12 = this.TextBoxResult;
      textBoxResult12.Text = textBoxResult12.Text + Environment.NewLine + ResultText + Environment.NewLine;
      object obj = result;
      ResultText = (string) null;
      return obj;
    }

    private async Task<object> undersized_check(
      object result,
      EDCL_HandlerFunctions handler,
      uint WMBUS_STATUS_UNDERSIZE,
      long flowdiff)
    {
      this.pulseUndersizeCounter = (ushort) 3;
      this.cfg_undersize_limit = (ushort) 3;
      this.flow_check_counter = (ushort) 1;
      System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
      textBoxResult1.Text = textBoxResult1.Text + "3. Test undersize" + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
      textBoxResult2.Text = textBoxResult2.Text + "cfg_undersize_limit set to " + this.cfg_undersize_limit.ToString() + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
      textBoxResult3.Text = textBoxResult3.Text + "pulseUndersizeCounter set to " + this.pulseUndersizeCounter.ToString() + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
      textBoxResult4.Text = textBoxResult4.Text + "flow_check_counter set to " + this.flow_check_counter.ToString() + Environment.NewLine + Environment.NewLine;
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_undersize_limit, this.cfg_undersize_limit);
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.pulseUndersizeCounter, this.pulseUndersizeCounter);
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
      await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
      await Task.Delay(((int) this.flow_check_counter + 1) * 2000);
      System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
      textBoxResult5.Text = textBoxResult5.Text + "Test undersize" + Environment.NewLine;
      int num1 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num1;
      do
      {
        this.pulseUndersizeCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseUndersizeCounter);
        this.flow_check_counter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.flow_check_counter);
        this.pulseTotalForwardCount64 = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64);
        System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
        textBoxResult6.Text = textBoxResult6.Text + "pulseUndersizeCounter = " + this.pulseUndersizeCounter.ToString() + " cfg_undersize_limit = " + this.cfg_undersize_limit.ToString() + Environment.NewLine;
        this.ShowFlowState(this.project_type, this.pulseFlowState);
        this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
        this.ShowDeviceStatus(this.project_type, this.device_status);
        this.flow_check_counter = (ushort) 1;
        this.pulseTotalForwardCount64 = flowdiff * 4L + this.pulseTotalCompare;
        System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
        textBoxResult7.Text = textBoxResult7.Text + "pulseTotalForwardCount64 set to " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
        textBoxResult8.Text = textBoxResult8.Text + "Change in Meter Reading: " + ((this.pulseTotalForwardCount64 - this.pulseTotalCompare) / 4L).ToString() + Environment.NewLine;
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64, this.pulseTotalForwardCount64);
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
        await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
        await Task.Delay(((int) this.flow_check_counter + 1) * 4000);
        int num2 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
        result = (object) num2;
        this.pulseTotalCompare = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalCompare);
        this.pulseUndersizeCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseUndersizeCounter);
        this.cfg_undersize_limit = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_undersize_limit);
        this.persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
        this.device_status = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.device_status);
        System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
        textBoxResult9.Text = textBoxResult9.Text + "after flow check pulseTotalCompare =  " + this.pulseTotalCompare.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult10 = this.TextBoxResult;
        textBoxResult10.Text = textBoxResult10.Text + "after flow check pulseUndersizeCounter = " + this.pulseUndersizeCounter.ToString() + " cfg_undersize_limit = " + this.cfg_undersize_limit.ToString() + Environment.NewLine;
        this.TextBoxResult.Text += Environment.NewLine;
      }
      while (this.pulseUndersizeCounter != (ushort) 0 && ((int) this.persistent_warning_flags & (int) WMBUS_STATUS_UNDERSIZE) != (int) WMBUS_STATUS_UNDERSIZE);
      System.Windows.Controls.TextBox textBoxResult11 = this.TextBoxResult;
      textBoxResult11.Text = textBoxResult11.Text + "after flow check pulseUndersizeCounter = " + this.pulseUndersizeCounter.ToString() + " cfg_undersize_limit = " + this.cfg_undersize_limit.ToString() + Environment.NewLine;
      this.ShowFlowState(this.project_type, this.pulseFlowState);
      this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
      this.ShowDeviceStatus(this.project_type, this.device_status);
      string ResultText = "";
      ResultText = ((int) this.persistent_warning_flags & (int) WMBUS_STATUS_UNDERSIZE) != (int) WMBUS_STATUS_UNDERSIZE || this.pulseUndersizeCounter != (ushort) 0 ? "test FAILED" : "test PASSED";
      System.Windows.Controls.TextBox textBoxResult12 = this.TextBoxResult;
      textBoxResult12.Text = textBoxResult12.Text + Environment.NewLine + ResultText + Environment.NewLine;
      object obj = result;
      ResultText = (string) null;
      return obj;
    }

    private async Task<object> TestOversized(object result, EDCL_HandlerFunctions handler)
    {
      uint WMBUS_STATUS_OVERSIZE = 256;
      this.TextBoxResult.Text = "Test Oversized" + Environment.NewLine;
      object obj1 = await this.ReadOversizedParameter(result, handler);
      result = obj1;
      obj1 = (object) null;
      this.flow_check_counter_save = (ushort) 450;
      this.cfg_oversize_limit_save = this.cfg_oversize_limit;
      if (this.cfg_device_mode != (byte) 8 && this.cfg_device_mode != (byte) 9)
      {
        System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
        textBoxResult1.Text = textBoxResult1.Text + "1. Current values" + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
        textBoxResult2.Text = textBoxResult2.Text + "cfg_oversize_diff = " + this.cfg_oversize_diff.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
        textBoxResult3.Text = textBoxResult3.Text + "cfg_oversize_limit = " + this.cfg_oversize_limit.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
        textBoxResult4.Text = textBoxResult4.Text + "pulseOversizeCounter = " + this.pulseOversizeCounter.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
        textBoxResult5.Text = textBoxResult5.Text + "flow_check_counter = " + this.flow_check_counter.ToString() + Environment.NewLine;
        this.TextBoxResult.Text += Environment.NewLine;
        this.ShowFlowState(this.project_type, this.pulseFlowState);
        this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
        this.ShowDeviceStatus(this.project_type, this.device_status);
        this.TextBoxResult.Text += Environment.NewLine;
        object obj2 = await this.ReadOversizedParameter(result, handler);
        result = obj2;
        obj2 = (object) null;
        System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
        textBoxResult6.Text = textBoxResult6.Text + "2. Pre condition" + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
        textBoxResult7.Text = textBoxResult7.Text + "pulseOversizeCounter = " + this.pulseOversizeCounter.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
        textBoxResult8.Text = textBoxResult8.Text + "flow_check_counter = " + this.flow_check_counter.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
        textBoxResult9.Text = textBoxResult9.Text + "pulseTotalForwardCount64 = " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult10 = this.TextBoxResult;
        textBoxResult10.Text = textBoxResult10.Text + "pulseTotalCompare = " + this.pulseTotalCompare.ToString() + Environment.NewLine;
        this.ShowFlowState(this.project_type, this.pulseFlowState);
        this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
        this.ShowDeviceStatus(this.project_type, this.device_status);
        this.TextBoxResult.Text += Environment.NewLine;
        string Title = string.Format("set pulse difference x (x< " + this.cfg_oversize_diff.ToString() + "(oversize test if x!=0)  x==0(do nothing) x >= " + this.cfg_oversize_diff.ToString() + "(not oversized if x!=0)");
        string value = InputWindow.Show(Title, "");
        long flowdiff = Convert.ToInt64(value);
        if (flowdiff == 0L)
        {
          object obj3 = await this.oversize_no_change(result, handler, WMBUS_STATUS_OVERSIZE, flowdiff);
          result = obj3;
          obj3 = (object) null;
        }
        else if (flowdiff < (long) this.cfg_oversize_diff)
        {
          if (((int) this.persistent_warning_flags & (int) WMBUS_STATUS_OVERSIZE) != (int) WMBUS_STATUS_OVERSIZE && this.pulseOversizeCounter > (ushort) 0)
          {
            object obj4 = await this.oversize_check(result, handler, WMBUS_STATUS_OVERSIZE, flowdiff);
            result = obj4;
            obj4 = (object) null;
          }
          else
          {
            object obj5 = await this.oversize_no_change(result, handler, WMBUS_STATUS_OVERSIZE, flowdiff);
            result = obj5;
            obj5 = (object) null;
          }
        }
        else if (((int) this.persistent_warning_flags & (int) WMBUS_STATUS_OVERSIZE) != (int) WMBUS_STATUS_OVERSIZE && this.cfg_oversize_limit != (ushort) 0 && this.pulseOversizeCounter > (ushort) 0)
        {
          object obj6 = await this.oversize_not_oversized(result, handler, WMBUS_STATUS_OVERSIZE, flowdiff);
          result = obj6;
          obj6 = (object) null;
        }
        else
        {
          object obj7 = await this.oversize_no_change(result, handler, WMBUS_STATUS_OVERSIZE, flowdiff);
          result = obj7;
          obj7 = (object) null;
        }
        System.Windows.Controls.TextBox textBoxResult11 = this.TextBoxResult;
        textBoxResult11.Text = textBoxResult11.Text + "write parameter back " + Environment.NewLine;
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter_save);
        await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
        Title = (string) null;
        value = (string) null;
      }
      else
        this.TextBoxResult.Text = "in Mode " + this.cfg_device_mode.ToString() + " smart funktions are not available, please change mode to 1 or 0" + Environment.NewLine;
      return result;
    }

    private async Task<object> ReadOversizedParameter(object result, EDCL_HandlerFunctions handler)
    {
      int num = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num;
      System.Windows.Controls.TextBox textBoxResult = this.TextBoxResult;
      textBoxResult.Text = textBoxResult.Text + "Test Oversized" + Environment.NewLine + Environment.NewLine;
      this.cfg_oversize_limit = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_oversize_limit);
      this.cfg_oversize_limit_save = this.cfg_oversize_limit;
      this.cfg_oversize_diff = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_oversize_diff);
      this.pulseOversizeCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseOversizeCounter);
      this.flow_check_counter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.flow_check_counter);
      this.flow_check_counter_save = this.flow_check_counter;
      this.pulseFlowState = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseFlowState);
      this.persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
      this.project_type = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.project_type);
      this.cfg_device_mode = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.cfg_device_mode);
      return result;
    }

    private async Task<object> oversize_no_change(
      object result,
      EDCL_HandlerFunctions handler,
      uint WMBUS_STATUS_OVERSIZE,
      long flowdiff)
    {
      this.flow_check_counter = (ushort) 1;
      ushort persistent_warning_flagsSave = this.persistent_warning_flags;
      ushort pulseOversizeCounterSave = this.pulseOversizeCounter;
      System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
      textBoxResult1.Text = textBoxResult1.Text + "3. Test no change" + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
      textBoxResult2.Text = textBoxResult2.Text + "Test not oversized" + Environment.NewLine;
      int num1 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num1;
      int loops = 0;
      for (int i = 0; i <= 3; ++i)
      {
        this.pulseOversizeCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseOversizeCounter);
        this.flow_check_counter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.flow_check_counter);
        this.pulseTotalForwardCount64 = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64);
        System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
        textBoxResult3.Text = textBoxResult3.Text + "pulseOversizeCounter = " + this.pulseOversizeCounter.ToString() + " cfg_oversize_limit = " + this.cfg_oversize_limit.ToString() + Environment.NewLine;
        this.ShowFlowState(this.project_type, this.pulseFlowState);
        this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
        this.ShowDeviceStatus(this.project_type, this.device_status);
        this.flow_check_counter = (ushort) 1;
        this.pulseTotalForwardCount64 = flowdiff * 4L + this.pulseTotalCompare;
        System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
        textBoxResult4.Text = textBoxResult4.Text + "pulseTotalForwardCount64 set to " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
        textBoxResult5.Text = textBoxResult5.Text + "Change in Meter Reading: " + ((this.pulseTotalForwardCount64 - this.pulseTotalCompare) / 4L).ToString() + Environment.NewLine;
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64, this.pulseTotalForwardCount64);
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
        await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
        await Task.Delay(((int) this.flow_check_counter + 1) * 4000);
        int num2 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
        result = (object) num2;
        this.pulseTotalCompare = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalCompare);
        this.pulseOversizeCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseOversizeCounter);
        this.persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
        this.device_status = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.device_status);
        System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
        textBoxResult6.Text = textBoxResult6.Text + "after flow check pulseTotalCompare =  " + this.pulseTotalCompare.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
        textBoxResult7.Text = textBoxResult7.Text + "after flow check pulseOversizeCounter = " + this.pulseOversizeCounter.ToString() + " cfg_oversize_limit = " + this.cfg_oversize_limit.ToString() + Environment.NewLine;
        this.TextBoxResult.Text += Environment.NewLine;
        ++loops;
      }
      System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
      textBoxResult8.Text = textBoxResult8.Text + "after flow check pulseOversizeCounter = " + this.pulseOversizeCounter.ToString() + " cfg_oversize_limit = " + this.cfg_oversize_limit.ToString() + Environment.NewLine;
      this.ShowFlowState(this.project_type, this.pulseFlowState);
      this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
      this.ShowDeviceStatus(this.project_type, this.device_status);
      ushort WMBUS_STATUS_BURST = 32768;
      string ResultText = "";
      ResultText = (int) this.persistent_warning_flags == (int) persistent_warning_flagsSave || flowdiff <= (long) this.cfg_burst_diff || ((int) this.persistent_warning_flags & (int) ~WMBUS_STATUS_BURST) == (int) persistent_warning_flagsSave ? (flowdiff != 0L ? ((int) this.persistent_warning_flags != (int) persistent_warning_flagsSave || ((int) this.persistent_warning_flags & (int) WMBUS_STATUS_OVERSIZE) != (int) WMBUS_STATUS_OVERSIZE || (int) this.pulseBackflowCounter != (int) this.cfg_pulse_unback_limit && (int) this.pulseBackflowCounter != (int) pulseOversizeCounterSave + loops ? ((int) this.persistent_warning_flags != (int) persistent_warning_flagsSave || ((int) this.persistent_warning_flags & (int) WMBUS_STATUS_OVERSIZE) == (int) WMBUS_STATUS_OVERSIZE || (int) this.pulseBackflowCounter != (int) this.cfg_pulse_back_limit && (int) this.pulseBackflowCounter != (int) pulseOversizeCounterSave + loops ? "test FAILED" : "test PASSED") : "test PASSED") : ((int) this.persistent_warning_flags != (int) persistent_warning_flagsSave || (int) this.pulseOversizeCounter != (int) pulseOversizeCounterSave ? "test FAILED" : "test PASSED")) : "test PASSED (BURST IS SET)";
      System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
      textBoxResult9.Text = textBoxResult9.Text + Environment.NewLine + ResultText + Environment.NewLine;
      object obj = result;
      ResultText = (string) null;
      return obj;
    }

    private async Task<object> oversize_not_oversized(
      object result,
      EDCL_HandlerFunctions handler,
      uint WMBUS_STATUS_OVERSIZE,
      long flowdiff)
    {
      System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
      textBoxResult1.Text = textBoxResult1.Text + "3. Test not oversized" + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
      textBoxResult2.Text = textBoxResult2.Text + "Test not oversized" + Environment.NewLine;
      int num1 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num1;
      this.pulseOversizeCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseOversizeCounter);
      this.flow_check_counter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.flow_check_counter);
      this.pulseTotalForwardCount64 = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64);
      this.pulseTotalCompare = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalCompare);
      System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
      textBoxResult3.Text = textBoxResult3.Text + "pulseOversizeCounter = " + this.pulseOversizeCounter.ToString() + " cfg_oversize_limit = " + this.cfg_oversize_limit.ToString() + Environment.NewLine;
      this.ShowFlowState(this.project_type, this.pulseFlowState);
      this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
      this.ShowDeviceStatus(this.project_type, this.device_status);
      this.flow_check_counter = (ushort) 1;
      this.pulseTotalForwardCount64 = flowdiff * 4L + this.pulseTotalCompare;
      System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
      textBoxResult4.Text = textBoxResult4.Text + "pulseTotalForwardCount64 set to " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
      textBoxResult5.Text = textBoxResult5.Text + "Change in Meter Reading: " + ((this.pulseTotalForwardCount64 - this.pulseTotalCompare) / 4L).ToString() + Environment.NewLine;
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64, this.pulseTotalForwardCount64);
      await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
      await Task.Delay(((int) this.flow_check_counter + 1) * 4000);
      int num2 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num2;
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
      await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
      await Task.Delay(((int) this.flow_check_counter + 1) * 4000);
      int num3 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num3;
      this.pulseTotalCompare = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalCompare);
      this.cfg_oversize_limit = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_oversize_limit);
      this.pulseOversizeCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseOversizeCounter);
      this.persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
      this.device_status = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.device_status);
      System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
      textBoxResult6.Text = textBoxResult6.Text + "after flow check pulseTotalCompare =  " + this.pulseTotalCompare.ToString() + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
      textBoxResult7.Text = textBoxResult7.Text + "after flow check pulseOversizeCounter = " + this.pulseOversizeCounter.ToString() + " cfg_oversize_limit = " + this.cfg_oversize_limit.ToString() + Environment.NewLine;
      this.TextBoxResult.Text += Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
      textBoxResult8.Text = textBoxResult8.Text + "after flow check pulseOversizeCounter = " + this.pulseOversizeCounter.ToString() + " cfg_oversize_limit = " + this.cfg_oversize_limit.ToString() + Environment.NewLine;
      string ResultText = "";
      ResultText = this.pulseOversizeCounter != (ushort) 0 ? "test FAILED pulseOversizeCounter != 0" + Environment.NewLine : "test PASSED OVERSIZE NOT DETECTED!!! pulseOversizeCounter == 0" + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
      textBoxResult9.Text = textBoxResult9.Text + Environment.NewLine + ResultText + Environment.NewLine;
      this.ShowFlowState(this.project_type, this.pulseFlowState);
      this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
      this.ShowDeviceStatus(this.project_type, this.device_status);
      object obj = result;
      ResultText = (string) null;
      return obj;
    }

    private async Task<object> oversize_check(
      object result,
      EDCL_HandlerFunctions handler,
      uint WMBUS_STATUS_OVERSIZE,
      long flowdiff)
    {
      this.pulseOversizeCounter = (ushort) 3;
      this.flow_check_counter = (ushort) 1;
      System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
      textBoxResult1.Text = textBoxResult1.Text + "3. Test oversize" + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
      textBoxResult2.Text = textBoxResult2.Text + "cfg_oversize_limit set to " + this.cfg_oversize_limit.ToString() + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
      textBoxResult3.Text = textBoxResult3.Text + "pulseOversizeCounter set to " + this.pulseOversizeCounter.ToString() + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
      textBoxResult4.Text = textBoxResult4.Text + "flow_check_counter set to " + this.flow_check_counter.ToString() + Environment.NewLine + Environment.NewLine;
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.pulseOversizeCounter, this.pulseOversizeCounter);
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
      await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
      await Task.Delay(((int) this.flow_check_counter + 1) * 2000);
      System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
      textBoxResult5.Text = textBoxResult5.Text + "Test oversize" + Environment.NewLine;
      int num1 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num1;
      do
      {
        this.pulseOversizeCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseOversizeCounter);
        this.flow_check_counter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.flow_check_counter);
        this.pulseTotalForwardCount64 = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64);
        System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
        textBoxResult6.Text = textBoxResult6.Text + "pulseOversizeCounter = " + this.pulseOversizeCounter.ToString() + " cfg_oversize_limit = " + this.cfg_oversize_limit.ToString() + Environment.NewLine;
        this.ShowFlowState(this.project_type, this.pulseFlowState);
        this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
        this.ShowDeviceStatus(this.project_type, this.device_status);
        this.flow_check_counter = (ushort) 1;
        this.pulseTotalForwardCount64 = flowdiff * 4L + this.pulseTotalCompare;
        System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
        textBoxResult7.Text = textBoxResult7.Text + "pulseTotalForwardCount64 set to " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
        textBoxResult8.Text = textBoxResult8.Text + "Change in Meter Reading: " + ((this.pulseTotalForwardCount64 - this.pulseTotalCompare) / 4L).ToString() + Environment.NewLine;
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64, this.pulseTotalForwardCount64);
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
        await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
        await Task.Delay(((int) this.flow_check_counter + 1) * 4000);
        int num2 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
        result = (object) num2;
        this.pulseTotalCompare = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalCompare);
        this.pulseOversizeCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseOversizeCounter);
        this.persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
        this.device_status = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.device_status);
        System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
        textBoxResult9.Text = textBoxResult9.Text + "after flow check pulseTotalCompare =  " + this.pulseTotalCompare.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult10 = this.TextBoxResult;
        textBoxResult10.Text = textBoxResult10.Text + "after flow check pulseOversizeCounter = " + this.pulseOversizeCounter.ToString() + " cfg_oversize_limit = " + this.cfg_oversize_limit.ToString() + Environment.NewLine;
        this.TextBoxResult.Text += Environment.NewLine;
      }
      while (this.pulseOversizeCounter != (ushort) 0 && ((int) this.persistent_warning_flags & (int) WMBUS_STATUS_OVERSIZE) != (int) WMBUS_STATUS_OVERSIZE);
      System.Windows.Controls.TextBox textBoxResult11 = this.TextBoxResult;
      textBoxResult11.Text = textBoxResult11.Text + "after flow check pulseOversizeCounter = " + this.pulseOversizeCounter.ToString() + " cfg_oversize_limit = " + this.cfg_oversize_limit.ToString() + Environment.NewLine;
      this.ShowFlowState(this.project_type, this.pulseFlowState);
      this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
      this.ShowDeviceStatus(this.project_type, this.device_status);
      string ResultText = "";
      ResultText = ((int) this.persistent_warning_flags & (int) WMBUS_STATUS_OVERSIZE) != (int) WMBUS_STATUS_OVERSIZE || (int) this.pulseBackflowCounter != (int) this.cfg_pulse_back_limit ? "test FAILED" : "test PASSED";
      System.Windows.Controls.TextBox textBoxResult12 = this.TextBoxResult;
      textBoxResult12.Text = textBoxResult12.Text + Environment.NewLine + ResultText + Environment.NewLine;
      object obj = result;
      ResultText = (string) null;
      return obj;
    }

    private async Task<object> TestBurst(object result, EDCL_HandlerFunctions handler)
    {
      uint WMBUS_STATUS_BURST = 32768;
      this.TextBoxResult.Text = "Test Burst" + Environment.NewLine;
      object obj1 = await this.ReadBurstParameter(result, handler);
      result = obj1;
      obj1 = (object) null;
      this.flow_check_counter_save = (ushort) 450;
      this.cfg_pulse_block_limit_save = this.cfg_pulse_block_limit;
      if (this.cfg_device_mode != (byte) 8 && this.cfg_device_mode != (byte) 9)
      {
        System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
        textBoxResult1.Text = textBoxResult1.Text + "1. Current values" + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
        textBoxResult2.Text = textBoxResult2.Text + "cfg_burst_diff = " + this.cfg_burst_diff.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
        textBoxResult3.Text = textBoxResult3.Text + "cfg_burst_limit = " + this.cfg_burst_limit.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
        textBoxResult4.Text = textBoxResult4.Text + "pulseBurstCounter = " + this.pulseBurstCounter.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
        textBoxResult5.Text = textBoxResult5.Text + "flow_check_counter = " + this.flow_check_counter.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
        textBoxResult6.Text = textBoxResult6.Text + "pulseTotalForwardCount64 = " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
        textBoxResult7.Text = textBoxResult7.Text + "pulseTotalCompare = " + this.pulseTotalCompare.ToString() + Environment.NewLine;
        this.ShowFlowState(this.project_type, this.pulseFlowState);
        this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
        this.ShowDeviceStatus(this.project_type, this.device_status);
        this.TextBoxResult.Text += Environment.NewLine;
        object obj2 = await this.ReadBurstParameter(result, handler);
        result = obj2;
        obj2 = (object) null;
        System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
        textBoxResult8.Text = textBoxResult8.Text + "2. Pre condition" + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
        textBoxResult9.Text = textBoxResult9.Text + "pulseBurstCounter = " + this.pulseBurstCounter.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult10 = this.TextBoxResult;
        textBoxResult10.Text = textBoxResult10.Text + "flow_check_counter = " + this.flow_check_counter.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult11 = this.TextBoxResult;
        textBoxResult11.Text = textBoxResult11.Text + "pulseTotalForwardCount64 = " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult12 = this.TextBoxResult;
        textBoxResult12.Text = textBoxResult12.Text + "pulseTotalCompare = " + this.pulseTotalCompare.ToString() + Environment.NewLine;
        this.ShowFlowState(this.project_type, this.pulseFlowState);
        this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
        this.ShowDeviceStatus(this.project_type, this.device_status);
        this.TextBoxResult.Text += Environment.NewLine;
        string Title = string.Format("set pulse difference x (x> " + this.cfg_burst_diff.ToString() + "(burst test) x<=" + this.cfg_burst_diff.ToString() + "(burst withdraw))");
        string value = InputWindow.Show(Title, "");
        long flowdiff = Convert.ToInt64(value);
        if (flowdiff > (long) this.cfg_burst_diff)
        {
          if (((int) this.persistent_warning_flags & (int) WMBUS_STATUS_BURST) != (int) WMBUS_STATUS_BURST)
          {
            object obj3 = await this.burst_check(result, handler, WMBUS_STATUS_BURST, flowdiff);
            result = obj3;
            obj3 = (object) null;
          }
          else
          {
            object obj4 = await this.burst_no_change(result, handler, WMBUS_STATUS_BURST, flowdiff);
            result = obj4;
            obj4 = (object) null;
          }
        }
        else if (((int) this.persistent_warning_flags & (int) WMBUS_STATUS_BURST) == (int) WMBUS_STATUS_BURST)
        {
          object obj5 = await this.burst_withdraw(result, handler, WMBUS_STATUS_BURST, flowdiff);
          result = obj5;
          obj5 = (object) null;
        }
        else
        {
          object obj6 = await this.burst_no_change(result, handler, WMBUS_STATUS_BURST, flowdiff);
          result = obj6;
          obj6 = (object) null;
        }
        System.Windows.Controls.TextBox textBoxResult13 = this.TextBoxResult;
        textBoxResult13.Text = textBoxResult13.Text + "write parameter back " + Environment.NewLine;
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_burst_limit, this.cfg_burst_limit);
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter_save);
        await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
        Title = (string) null;
        value = (string) null;
      }
      else
        this.TextBoxResult.Text = "in Mode " + this.cfg_device_mode.ToString() + " smart funktions are not available, please change mode to 1 or 0" + Environment.NewLine;
      return result;
    }

    private async Task<object> burst_withdraw(
      object result,
      EDCL_HandlerFunctions handler,
      uint WMBUS_STATUS_BURST,
      long flowdiff)
    {
      this.flow_check_counter = (ushort) 1;
      System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
      textBoxResult1.Text = textBoxResult1.Text + "3. Test burst withdraw" + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
      textBoxResult2.Text = textBoxResult2.Text + "Test burst withdraw" + Environment.NewLine;
      int num1 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num1;
      this.pulseBurstCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseBurstCounter);
      this.flow_check_counter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.flow_check_counter);
      this.pulseTotalForwardCount64 = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64);
      System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
      textBoxResult3.Text = textBoxResult3.Text + "pulseBurstCounter = " + this.pulseBurstCounter.ToString() + " cfg_burst_limit = " + this.cfg_burst_limit.ToString() + Environment.NewLine;
      this.ShowFlowState(this.project_type, this.pulseFlowState);
      this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
      this.ShowDeviceStatus(this.project_type, this.device_status);
      this.flow_check_counter = (ushort) 1;
      this.pulseTotalForwardCount64 = flowdiff * 4L + this.pulseTotalCompare;
      System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
      textBoxResult4.Text = textBoxResult4.Text + "pulseTotalForwardCount64 set to " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
      textBoxResult5.Text = textBoxResult5.Text + "Change in Meter Reading: " + ((this.pulseTotalForwardCount64 - this.pulseTotalCompare) / 4L).ToString() + Environment.NewLine;
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64, this.pulseTotalForwardCount64);
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
      await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
      await Task.Delay(((int) this.flow_check_counter + 1) * 4000);
      int num2 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num2;
      this.pulseTotalCompare = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalCompare);
      this.pulseBurstCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseBurstCounter);
      this.persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
      this.device_status = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.device_status);
      System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
      textBoxResult6.Text = textBoxResult6.Text + "after flow check pulseTotalCompare =  " + this.pulseTotalCompare.ToString() + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
      textBoxResult7.Text = textBoxResult7.Text + "after flow check pulseBurstCounter = " + this.pulseBurstCounter.ToString() + " cfg_burst_limit = " + this.cfg_burst_limit.ToString() + Environment.NewLine;
      this.TextBoxResult.Text += Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
      textBoxResult8.Text = textBoxResult8.Text + "after flow check pulseBurstCounter = " + this.pulseBurstCounter.ToString() + " cfg_burst_limit = " + this.cfg_burst_limit.ToString() + Environment.NewLine;
      this.ShowFlowState(this.project_type, this.pulseFlowState);
      this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
      this.ShowDeviceStatus(this.project_type, this.device_status);
      uint WMBUS_STATUS_UNDERSIZE = 512;
      string ResultText = "";
      ResultText = ((int) this.persistent_warning_flags & (int) WMBUS_STATUS_BURST) != 0 && (((int) this.persistent_warning_flags & (int) WMBUS_STATUS_BURST) != (int) WMBUS_STATUS_BURST || ((int) this.persistent_warning_flags & (int) WMBUS_STATUS_UNDERSIZE) != (int) WMBUS_STATUS_UNDERSIZE) || (int) this.pulseBurstCounter != (int) this.cfg_burst_limit ? "test FAILED" : "test PASSED";
      System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
      textBoxResult9.Text = textBoxResult9.Text + Environment.NewLine + ResultText + Environment.NewLine;
      object obj = result;
      ResultText = (string) null;
      return obj;
    }

    private async Task<object> burst_no_change(
      object result,
      EDCL_HandlerFunctions handler,
      uint WMBUS_STATUS_BURST,
      long flowdiff)
    {
      ushort pulseBurstCounterSave = this.pulseBurstCounter;
      ushort persistent_warning_flagsSave = this.persistent_warning_flags;
      System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
      textBoxResult1.Text = textBoxResult1.Text + "3. Test burst" + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
      textBoxResult2.Text = textBoxResult2.Text + "Test burst no change" + Environment.NewLine;
      int num1 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num1;
      for (int i = 0; i <= 3; ++i)
      {
        this.pulseBurstCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseBurstCounter);
        this.flow_check_counter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.flow_check_counter);
        this.pulseTotalForwardCount64 = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64);
        System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
        textBoxResult3.Text = textBoxResult3.Text + "pulseBurstCounter = " + this.pulseBurstCounter.ToString() + " cfg_burst_limit = " + this.cfg_burst_limit.ToString() + Environment.NewLine;
        this.ShowFlowState(this.project_type, this.pulseFlowState);
        this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
        this.ShowDeviceStatus(this.project_type, this.device_status);
        this.flow_check_counter = (ushort) 1;
        this.pulseTotalForwardCount64 = flowdiff * 4L + this.pulseTotalCompare;
        System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
        textBoxResult4.Text = textBoxResult4.Text + "pulseTotalForwardCount64 set to " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
        textBoxResult5.Text = textBoxResult5.Text + "Change in Meter Reading: " + ((this.pulseTotalForwardCount64 - this.pulseTotalCompare) / 4L).ToString() + Environment.NewLine;
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64, this.pulseTotalForwardCount64);
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
        await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
        await Task.Delay(((int) this.flow_check_counter + 1) * 4000);
        int num2 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
        result = (object) num2;
        this.pulseTotalCompare = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalCompare);
        this.pulseBurstCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseBurstCounter);
        this.persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
        this.device_status = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.device_status);
        System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
        textBoxResult6.Text = textBoxResult6.Text + "after flow check pulseTotalCompare =  " + this.pulseTotalCompare.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
        textBoxResult7.Text = textBoxResult7.Text + "after flow check pulseBurstCounter = " + this.pulseBurstCounter.ToString() + " cfg_burst_limit = " + this.cfg_burst_limit.ToString() + Environment.NewLine;
        this.TextBoxResult.Text += Environment.NewLine;
      }
      System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
      textBoxResult8.Text = textBoxResult8.Text + "after flow check pulseBurstCounter = " + this.pulseBurstCounter.ToString() + " cfg_burst_limit = " + this.cfg_burst_limit.ToString() + Environment.NewLine;
      this.ShowFlowState(this.project_type, this.pulseFlowState);
      this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
      this.ShowDeviceStatus(this.project_type, this.device_status);
      string ResultText = "";
      ResultText = (int) this.persistent_warning_flags != (int) persistent_warning_flagsSave || (int) this.pulseBurstCounter != (int) pulseBurstCounterSave ? "test FAILED" : "test PASSED";
      System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
      textBoxResult9.Text = textBoxResult9.Text + Environment.NewLine + ResultText + Environment.NewLine;
      object obj = result;
      ResultText = (string) null;
      return obj;
    }

    private async Task<object> burst_check(
      object result,
      EDCL_HandlerFunctions handler,
      uint WMBUS_STATUS_BURST,
      long flowdiff)
    {
      this.pulseBurstCounter = this.cfg_burst_limit;
      this.flow_check_counter = (ushort) 1;
      System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
      textBoxResult1.Text = textBoxResult1.Text + "3. Test burst" + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
      textBoxResult2.Text = textBoxResult2.Text + "pulseBurstCounter set to " + this.pulseBurstCounter.ToString() + Environment.NewLine;
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.pulseBurstCounter, this.pulseBurstCounter);
      await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
      System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
      textBoxResult3.Text = textBoxResult3.Text + "Test burst" + Environment.NewLine;
      int num1 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num1;
      do
      {
        this.pulseBurstCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseBurstCounter);
        this.flow_check_counter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.flow_check_counter);
        this.pulseTotalForwardCount64 = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64);
        System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
        textBoxResult4.Text = textBoxResult4.Text + "pulseBurstCounter = " + this.pulseBurstCounter.ToString() + " cfg_burst_limit = " + this.cfg_burst_limit.ToString() + Environment.NewLine;
        this.ShowFlowState(this.project_type, this.pulseFlowState);
        this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
        this.ShowDeviceStatus(this.project_type, this.device_status);
        this.flow_check_counter = (ushort) 1;
        this.pulseTotalForwardCount64 = flowdiff * 4L + this.pulseTotalCompare;
        System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
        textBoxResult5.Text = textBoxResult5.Text + "pulseTotalForwardCount64 set to " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
        textBoxResult6.Text = textBoxResult6.Text + "Change in Meter Reading: " + ((this.pulseTotalForwardCount64 - this.pulseTotalCompare) / 4L).ToString() + Environment.NewLine;
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64, this.pulseTotalForwardCount64);
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
        await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
        await Task.Delay(((int) this.flow_check_counter + 1) * 4000);
        int num2 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
        result = (object) num2;
        this.pulseTotalCompare = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalCompare);
        this.pulseBurstCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseBurstCounter);
        this.persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
        this.device_status = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.device_status);
        System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
        textBoxResult7.Text = textBoxResult7.Text + "after flow check pulseTotalCompare =  " + this.pulseTotalCompare.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
        textBoxResult8.Text = textBoxResult8.Text + "after flow check pulseBurstCounter = " + this.pulseBlockageCounter.ToString() + " cfg_burst_limit = " + this.cfg_burst_limit.ToString() + Environment.NewLine;
        this.TextBoxResult.Text += Environment.NewLine;
      }
      while (this.pulseBurstCounter != (ushort) 0 && ((int) this.persistent_warning_flags & (int) WMBUS_STATUS_BURST) != (int) WMBUS_STATUS_BURST);
      System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
      textBoxResult9.Text = textBoxResult9.Text + "after flow check pulseBurstCounter = " + this.pulseBurstCounter.ToString() + " cfg_burst_limit = " + this.cfg_burst_limit.ToString() + Environment.NewLine;
      this.ShowFlowState(this.project_type, this.pulseFlowState);
      this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
      this.ShowDeviceStatus(this.project_type, this.device_status);
      string ResultText = "";
      ResultText = ((int) this.persistent_warning_flags & (int) WMBUS_STATUS_BURST) != (int) WMBUS_STATUS_BURST || this.pulseBurstCounter != (ushort) 0 ? "test FAILED" : "test PASSED";
      System.Windows.Controls.TextBox textBoxResult10 = this.TextBoxResult;
      textBoxResult10.Text = textBoxResult10.Text + Environment.NewLine + ResultText + Environment.NewLine;
      object obj = result;
      ResultText = (string) null;
      return obj;
    }

    private async Task<object> ReadBurstParameter(object result, EDCL_HandlerFunctions handler)
    {
      int num = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num;
      this.cfg_burst_limit = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_burst_limit);
      this.cfg_burst_diff = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_burst_diff);
      this.pulseBurstCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseBurstCounter);
      this.flow_check_counter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.flow_check_counter);
      this.pulseBackflowCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseBackflowCounter);
      this.pulseFlowState = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseFlowState);
      this.persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
      this.device_status = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.device_status);
      this.project_type = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.project_type);
      this.pulseTotalForwardCount64 = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64);
      this.pulseTotalCompare = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalCompare);
      this.cfg_device_mode = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.cfg_device_mode);
      return result;
    }

    private async Task<object> TestBlockage(object result, EDCL_HandlerFunctions handler)
    {
      uint WMBUS_STATUS_BLOCK_A = 1024;
      uint WMBUS_STATUS_TEMPORARY_ERROR = 16;
      object obj1 = await this.ReadBlockageParameter(result, handler);
      result = obj1;
      obj1 = (object) null;
      if (this.cfg_device_mode != (byte) 8 && this.cfg_device_mode != (byte) 9)
      {
        this.TextBoxResult.Text = "1. Current values" + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
        textBoxResult1.Text = textBoxResult1.Text + "cfg_pulse_block_limit = " + this.cfg_pulse_block_limit.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
        textBoxResult2.Text = textBoxResult2.Text + "pulseBlockageCounter = " + this.pulseBackflowCounter.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
        textBoxResult3.Text = textBoxResult3.Text + "flow_check_counter = " + this.flow_check_counter.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
        textBoxResult4.Text = textBoxResult4.Text + "pulseTotalForwardCount64 = " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
        textBoxResult5.Text = textBoxResult5.Text + "pulseTotalCompare = " + this.pulseTotalCompare.ToString() + Environment.NewLine;
        this.ShowFlowState(this.project_type, this.pulseFlowState);
        this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
        this.ShowDeviceStatus(this.project_type, this.device_status);
        this.TextBoxResult.Text += Environment.NewLine;
        this.flow_check_counter = (ushort) 1;
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
        await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
        await Task.Delay(((int) this.flow_check_counter + 1) * 2000);
        int num = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
        result = (object) num;
        this.pulseBlockageCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseBlockageCounter);
        this.flow_check_counter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.flow_check_counter);
        this.pulseFlowState = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseFlowState);
        this.persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
        this.device_status = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.device_status);
        this.project_type = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.project_type);
        this.pulseTotalForwardCount64 = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64);
        this.pulseTotalCompare = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalCompare);
        System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
        textBoxResult6.Text = textBoxResult6.Text + "2. Pre condition" + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
        textBoxResult7.Text = textBoxResult7.Text + "pulseBlockageCounter = " + this.pulseBlockageCounter.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
        textBoxResult8.Text = textBoxResult8.Text + "flow_check_counter = " + this.flow_check_counter.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
        textBoxResult9.Text = textBoxResult9.Text + "pulseTotalForwardCount64 = " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult10 = this.TextBoxResult;
        textBoxResult10.Text = textBoxResult10.Text + "pulseTotalCompare = " + this.pulseTotalCompare.ToString() + Environment.NewLine;
        this.ShowFlowState(this.project_type, this.pulseFlowState);
        this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
        this.ShowDeviceStatus(this.project_type, this.device_status);
        this.TextBoxResult.Text += Environment.NewLine;
        string value = InputWindow.Show("set pulse difference x (x<-2, -2<=x<0, x=0, 0<x<=2, x>2)", "");
        long flowdiff = Convert.ToInt64(value);
        if (flowdiff == 0L)
        {
          if (((int) this.persistent_warning_flags & ((int) WMBUS_STATUS_BLOCK_A | (int) WMBUS_STATUS_TEMPORARY_ERROR)) != ((int) WMBUS_STATUS_BLOCK_A | (int) WMBUS_STATUS_TEMPORARY_ERROR))
          {
            object obj2 = await this.blockage_check(result, handler, WMBUS_STATUS_BLOCK_A | WMBUS_STATUS_TEMPORARY_ERROR, flowdiff);
            result = obj2;
            obj2 = (object) null;
          }
          else
          {
            object obj3 = await this.blockage_no_change(result, handler, WMBUS_STATUS_BLOCK_A | WMBUS_STATUS_TEMPORARY_ERROR, flowdiff);
            result = obj3;
            obj3 = (object) null;
          }
        }
        else if (((int) this.persistent_warning_flags & ((int) WMBUS_STATUS_BLOCK_A | (int) WMBUS_STATUS_TEMPORARY_ERROR)) == ((int) WMBUS_STATUS_BLOCK_A | (int) WMBUS_STATUS_TEMPORARY_ERROR))
        {
          if (flowdiff <= 2L && flowdiff >= -2L)
          {
            object obj4 = await this.blockage_no_change(result, handler, WMBUS_STATUS_BLOCK_A | WMBUS_STATUS_TEMPORARY_ERROR, flowdiff);
            result = obj4;
            obj4 = (object) null;
          }
          else
          {
            object obj5 = await this.blockage_withdraw(result, handler, WMBUS_STATUS_BLOCK_A | WMBUS_STATUS_TEMPORARY_ERROR, flowdiff);
            result = obj5;
            obj5 = (object) null;
          }
        }
        else
        {
          object obj6 = await this.blockage_no_change(result, handler, WMBUS_STATUS_BLOCK_A | WMBUS_STATUS_TEMPORARY_ERROR, flowdiff);
          result = obj6;
          obj6 = (object) null;
        }
        System.Windows.Controls.TextBox textBoxResult11 = this.TextBoxResult;
        textBoxResult11.Text = textBoxResult11.Text + "write parameter back " + Environment.NewLine;
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_pulse_block_limit, this.cfg_pulse_block_limit_save);
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter_save);
        await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
        value = (string) null;
      }
      else
        this.TextBoxResult.Text = "in Mode " + this.cfg_device_mode.ToString() + " smart funktions are not available, please change mode to 1 or 0" + Environment.NewLine;
      return result;
    }

    private async Task<object> blockage_check(
      object result,
      EDCL_HandlerFunctions handler,
      uint WMBUS_STATUS_BLOCK,
      long flowdiff)
    {
      this.pulseBlockageCounter = (ushort) 3;
      this.cfg_pulse_block_limit = (ushort) 3;
      this.flow_check_counter = (ushort) 1;
      System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
      textBoxResult1.Text = textBoxResult1.Text + "3. Test blockage" + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
      textBoxResult2.Text = textBoxResult2.Text + "cfg_pulse_block_limit set to " + this.cfg_pulse_block_limit.ToString() + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
      textBoxResult3.Text = textBoxResult3.Text + "pulseBlockageCounter set to " + this.pulseBlockageCounter.ToString() + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
      textBoxResult4.Text = textBoxResult4.Text + "flow_check_counter set to " + this.flow_check_counter.ToString() + Environment.NewLine + Environment.NewLine;
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_pulse_block_limit, this.cfg_pulse_block_limit);
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.pulseBlockageCounter, this.pulseBlockageCounter);
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
      await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
      await Task.Delay(((int) this.flow_check_counter + 1) * 2000);
      System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
      textBoxResult5.Text = textBoxResult5.Text + "Test blockage" + Environment.NewLine;
      int num1 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num1;
      do
      {
        this.pulseBlockageCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseBlockageCounter);
        this.flow_check_counter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.flow_check_counter);
        this.pulseTotalForwardCount64 = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64);
        System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
        textBoxResult6.Text = textBoxResult6.Text + "pulseBlockageCounter = " + this.pulseBlockageCounter.ToString() + " cfg_pulse_block_limit = " + this.cfg_pulse_block_limit.ToString() + Environment.NewLine;
        this.ShowFlowState(this.project_type, this.pulseFlowState);
        this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
        this.ShowDeviceStatus(this.project_type, this.device_status);
        this.flow_check_counter = (ushort) 1;
        this.pulseTotalForwardCount64 = flowdiff * 4L + this.pulseTotalCompare;
        System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
        textBoxResult7.Text = textBoxResult7.Text + "pulseTotalForwardCount64 set to " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
        textBoxResult8.Text = textBoxResult8.Text + "Change in Meter Reading: " + ((this.pulseTotalForwardCount64 - this.pulseTotalCompare) / 4L).ToString() + Environment.NewLine;
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64, this.pulseTotalForwardCount64);
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
        await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
        await Task.Delay(((int) this.flow_check_counter + 1) * 4000);
        int num2 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
        result = (object) num2;
        this.pulseTotalCompare = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalCompare);
        this.pulseBlockageCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseBlockageCounter);
        this.persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
        this.device_status = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.device_status);
        System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
        textBoxResult9.Text = textBoxResult9.Text + "after flow check pulseTotalCompare =  " + this.pulseTotalCompare.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult10 = this.TextBoxResult;
        textBoxResult10.Text = textBoxResult10.Text + "after flow check pulseBlockageCounter = " + this.pulseBlockageCounter.ToString() + " cfg_pulse_block_limit = " + this.cfg_pulse_block_limit.ToString() + Environment.NewLine;
        this.TextBoxResult.Text += Environment.NewLine;
      }
      while (this.pulseBlockageCounter != (ushort) 0 && ((int) this.persistent_warning_flags & (int) WMBUS_STATUS_BLOCK) != (int) WMBUS_STATUS_BLOCK);
      System.Windows.Controls.TextBox textBoxResult11 = this.TextBoxResult;
      textBoxResult11.Text = textBoxResult11.Text + "after flow check pulseBlockageCounter = " + this.pulseBlockageCounter.ToString() + " cfg_pulse_block_limit = " + this.cfg_pulse_block_limit.ToString() + Environment.NewLine;
      this.ShowFlowState(this.project_type, this.pulseFlowState);
      this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
      this.ShowDeviceStatus(this.project_type, this.device_status);
      string ResultText = "";
      ResultText = ((int) this.persistent_warning_flags & (int) WMBUS_STATUS_BLOCK) != (int) WMBUS_STATUS_BLOCK ? "test FAILED" : "test PASSED";
      System.Windows.Controls.TextBox textBoxResult12 = this.TextBoxResult;
      textBoxResult12.Text = textBoxResult12.Text + Environment.NewLine + ResultText + Environment.NewLine;
      object obj = result;
      ResultText = (string) null;
      return obj;
    }

    private async Task<object> blockage_withdraw(
      object result,
      EDCL_HandlerFunctions handler,
      uint WMBUS_STATUS_BLOCK,
      long flowdiff)
    {
      this.flow_check_counter = (ushort) 1;
      System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
      textBoxResult1.Text = textBoxResult1.Text + "3. Test blockage withdraw" + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
      textBoxResult2.Text = textBoxResult2.Text + "flow_check_counter set to " + this.flow_check_counter.ToString() + Environment.NewLine + Environment.NewLine;
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
      await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
      await Task.Delay(((int) this.flow_check_counter + 1) * 2000);
      System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
      textBoxResult3.Text = textBoxResult3.Text + "Test results" + Environment.NewLine;
      int num1 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num1;
      this.pulseBlockageCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseBlockageCounter);
      this.flow_check_counter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.flow_check_counter);
      this.pulseTotalForwardCount64 = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64);
      System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
      textBoxResult4.Text = textBoxResult4.Text + "pulseBlockageCounter = " + this.pulseBlockageCounter.ToString() + " cfg_pulse_block_limit = " + this.cfg_pulse_block_limit.ToString() + Environment.NewLine;
      this.ShowFlowState(this.project_type, this.pulseFlowState);
      this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
      this.ShowDeviceStatus(this.project_type, this.device_status);
      this.flow_check_counter = (ushort) 1;
      this.pulseTotalForwardCount64 = flowdiff * 4L + this.pulseTotalCompare;
      System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
      textBoxResult5.Text = textBoxResult5.Text + "pulseTotalForwardCount64 set to " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
      textBoxResult6.Text = textBoxResult6.Text + "Change in Meter Reading: " + ((this.pulseTotalForwardCount64 - this.pulseTotalCompare) / 4L).ToString() + Environment.NewLine;
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64, this.pulseTotalForwardCount64);
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
      await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
      await Task.Delay(((int) this.flow_check_counter + 1) * 4000);
      int num2 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num2;
      this.pulseTotalCompare = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalCompare);
      this.pulseBlockageCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseBlockageCounter);
      this.persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
      this.pulseFlowState = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseFlowState);
      this.device_status = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.device_status);
      System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
      textBoxResult7.Text = textBoxResult7.Text + "after flow check pulseTotalCompare =  " + this.pulseTotalCompare.ToString() + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
      textBoxResult8.Text = textBoxResult8.Text + "after flow check pulseBlockageCounter = " + this.pulseBlockageCounter.ToString() + " cfg_pulse_block_limit = " + this.cfg_pulse_block_limit.ToString() + Environment.NewLine;
      this.TextBoxResult.Text += Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
      textBoxResult9.Text = textBoxResult9.Text + "after flow check pulseBlockageCounter = " + this.pulseBlockageCounter.ToString() + " cfg_pulse_block_limit = " + this.cfg_pulse_block_limit.ToString() + Environment.NewLine;
      this.ShowFlowState(this.project_type, this.pulseFlowState);
      this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
      this.ShowDeviceStatus(this.project_type, this.device_status);
      string ResultText = "";
      ResultText = ((int) this.persistent_warning_flags & (int) WMBUS_STATUS_BLOCK) == (int) WMBUS_STATUS_BLOCK || (int) this.pulseBlockageCounter != (int) this.cfg_pulse_block_limit ? "test FAILED" : "test PASSED";
      System.Windows.Controls.TextBox textBoxResult10 = this.TextBoxResult;
      textBoxResult10.Text = textBoxResult10.Text + Environment.NewLine + ResultText + Environment.NewLine;
      object obj = result;
      ResultText = (string) null;
      return obj;
    }

    private async Task<object> blockage_no_change(
      object result,
      EDCL_HandlerFunctions handler,
      uint WMBUS_STATUS_BLOCK,
      long flowdiff)
    {
      this.pulseBlockageCounter = (ushort) 3;
      this.flow_check_counter = (ushort) 1;
      ushort persistent_warning_flagsSave = this.persistent_warning_flags;
      System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
      textBoxResult1.Text = textBoxResult1.Text + "3. Test blockage no change" + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
      textBoxResult2.Text = textBoxResult2.Text + "pulseBlockageCounter set to " + this.pulseBlockageCounter.ToString() + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
      textBoxResult3.Text = textBoxResult3.Text + "flow_check_counter set to " + this.flow_check_counter.ToString() + Environment.NewLine + Environment.NewLine;
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.pulseBlockageCounter, this.pulseBlockageCounter);
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
      await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
      await Task.Delay(((int) this.flow_check_counter + 1) * 2000);
      System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
      textBoxResult4.Text = textBoxResult4.Text + "Test results" + Environment.NewLine;
      int num1 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num1;
      for (int i = 0; i <= 3; ++i)
      {
        this.pulseBlockageCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseBlockageCounter);
        this.flow_check_counter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.flow_check_counter);
        this.pulseTotalForwardCount64 = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64);
        System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
        textBoxResult5.Text = textBoxResult5.Text + "pulseBlockageCounter = " + this.pulseBlockageCounter.ToString() + " cfg_pulse_block_limit = " + this.cfg_pulse_block_limit.ToString() + Environment.NewLine;
        this.ShowFlowState(this.project_type, this.pulseFlowState);
        this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
        this.ShowDeviceStatus(this.project_type, this.device_status);
        this.flow_check_counter = (ushort) 1;
        this.pulseTotalForwardCount64 = flowdiff * 4L + this.pulseTotalCompare;
        System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
        textBoxResult6.Text = textBoxResult6.Text + "pulseTotalForwardCount64 set to " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
        textBoxResult7.Text = textBoxResult7.Text + "Change in Meter Reading: " + ((this.pulseTotalForwardCount64 - this.pulseTotalCompare) / 4L).ToString() + Environment.NewLine;
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64, this.pulseTotalForwardCount64);
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
        await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
        await Task.Delay(((int) this.flow_check_counter + 1) * 4000);
        int num2 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
        result = (object) num2;
        this.pulseTotalCompare = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalCompare);
        this.pulseBlockageCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseBlockageCounter);
        this.persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
        this.device_status = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.device_status);
        System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
        textBoxResult8.Text = textBoxResult8.Text + "after flow check pulseTotalCompare =  " + this.pulseTotalCompare.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
        textBoxResult9.Text = textBoxResult9.Text + "after flow check pulseBlockageCounter = " + this.pulseBlockageCounter.ToString() + " cfg_pulse_block_limit = " + this.cfg_pulse_block_limit.ToString() + Environment.NewLine;
        this.TextBoxResult.Text += Environment.NewLine;
      }
      System.Windows.Controls.TextBox textBoxResult10 = this.TextBoxResult;
      textBoxResult10.Text = textBoxResult10.Text + "after flow check pulseBlockageCounter = " + this.pulseBlockageCounter.ToString() + " cfg_pulse_block_limit = " + this.cfg_pulse_block_limit.ToString() + Environment.NewLine;
      this.ShowFlowState(this.project_type, this.pulseFlowState);
      this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
      this.ShowDeviceStatus(this.project_type, this.device_status);
      string ResultText = "";
      ResultText = ((int) this.persistent_warning_flags & (int) WMBUS_STATUS_BLOCK) != (int) WMBUS_STATUS_BLOCK ? ((int) this.persistent_warning_flags != (int) persistent_warning_flagsSave || (int) this.pulseBlockageCounter != (int) this.cfg_pulse_block_limit ? "test FAILED" : "test PASSED") : ((int) this.persistent_warning_flags != (int) persistent_warning_flagsSave ? "test FAILED" : "test PASSED");
      System.Windows.Controls.TextBox textBoxResult11 = this.TextBoxResult;
      textBoxResult11.Text = textBoxResult11.Text + Environment.NewLine + ResultText + Environment.NewLine;
      object obj = result;
      ResultText = (string) null;
      return obj;
    }

    private async Task<object> ReadBlockageParameter(object result, EDCL_HandlerFunctions handler)
    {
      int num = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num;
      this.TextBoxResult.Text = "Test Blockage" + Environment.NewLine;
      this.cfg_pulse_block_limit = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_pulse_block_limit);
      this.cfg_pulse_block_limit_save = this.cfg_pulse_block_limit;
      this.pulseBlockageCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseBlockageCounter);
      this.flow_check_counter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.flow_check_counter);
      this.flow_check_counter_save = this.flow_check_counter;
      this.pulseBackflowCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseBackflowCounter);
      this.pulseFlowState = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseFlowState);
      this.persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
      this.device_status = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.device_status);
      this.project_type = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.project_type);
      this.pulseTotalForwardCount64 = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64);
      this.pulseTotalCompare = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalCompare);
      this.cfg_device_mode = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.cfg_device_mode);
      return result;
    }

    private async Task<object> TestReverseFlow(object result, EDCL_HandlerFunctions handler)
    {
      uint WMBUS_STATUS_BACKFLOW = 2048;
      object obj1 = await this.ReadReverseFlowParameter(result, handler);
      result = obj1;
      obj1 = (object) null;
      this.cfg_pulse_back_limit_save = this.cfg_pulse_back_limit;
      this.cfg_pulse_unback_limit_save = this.cfg_pulse_unback_limit;
      if (this.cfg_device_mode != (byte) 8 && this.cfg_device_mode != (byte) 9)
      {
        this.TextBoxResult.Text = "1. Current values" + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
        textBoxResult1.Text = textBoxResult1.Text + "cfg_pulse_back_limit = " + this.cfg_pulse_back_limit.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
        textBoxResult2.Text = textBoxResult2.Text + "cfg_pulse_unback_limit = " + this.cfg_pulse_unback_limit.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
        textBoxResult3.Text = textBoxResult3.Text + "pulseBackflowCounter = " + this.pulseBackflowCounter.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
        textBoxResult4.Text = textBoxResult4.Text + "flow_check_counter = " + this.flow_check_counter.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
        textBoxResult5.Text = textBoxResult5.Text + "pulseTotalForwardCount64 = " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
        textBoxResult6.Text = textBoxResult6.Text + "pulseTotalCompare = " + this.pulseTotalCompare.ToString() + Environment.NewLine;
        this.ShowFlowState(this.project_type, this.pulseFlowState);
        this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
        this.ShowDeviceStatus(this.project_type, this.device_status);
        this.TextBoxResult.Text += Environment.NewLine;
        this.flow_check_counter = (ushort) 1;
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
        await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
        await Task.Delay(((int) this.flow_check_counter + 1) * 2000);
        int num = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
        result = (object) num;
        this.pulseBackflowCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseBackflowCounter);
        this.flow_check_counter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.flow_check_counter);
        this.pulseFlowState = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseFlowState);
        this.persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
        this.device_status = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.device_status);
        this.project_type = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.project_type);
        this.pulseTotalForwardCount64 = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64);
        this.pulseTotalCompare = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalCompare);
        System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
        textBoxResult7.Text = textBoxResult7.Text + "2. Pre condition" + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
        textBoxResult8.Text = textBoxResult8.Text + "pulseBackflowCounter = " + this.pulseBackflowCounter.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
        textBoxResult9.Text = textBoxResult9.Text + "flow_check_counter = " + this.flow_check_counter.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult10 = this.TextBoxResult;
        textBoxResult10.Text = textBoxResult10.Text + "pulseTotalForwardCount64 = " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult11 = this.TextBoxResult;
        textBoxResult11.Text = textBoxResult11.Text + "pulseTotalCompare = " + this.pulseTotalCompare.ToString() + Environment.NewLine;
        this.ShowFlowState(this.project_type, this.pulseFlowState);
        this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
        this.ShowDeviceStatus(this.project_type, this.device_status);
        this.TextBoxResult.Text += Environment.NewLine;
        string value = InputWindow.Show("set pulse difference x(x<0, x=0, x>0 )", "");
        long flowdiff = Convert.ToInt64(value);
        if (flowdiff < 0L)
        {
          if (((int) this.persistent_warning_flags & (int) WMBUS_STATUS_BACKFLOW) != (int) WMBUS_STATUS_BACKFLOW)
          {
            object obj2 = await this.reverse_flow_check(result, handler, WMBUS_STATUS_BACKFLOW, flowdiff);
            result = obj2;
            obj2 = (object) null;
          }
          else
          {
            object obj3 = await this.reverse_flow_no_change(result, handler, WMBUS_STATUS_BACKFLOW, flowdiff);
            result = obj3;
            obj3 = (object) null;
          }
        }
        else if (flowdiff > 0L)
        {
          if (((int) this.persistent_warning_flags & (int) WMBUS_STATUS_BACKFLOW) == (int) WMBUS_STATUS_BACKFLOW)
          {
            object obj4 = await this.reverse_flow_withdraw(result, handler, WMBUS_STATUS_BACKFLOW, flowdiff);
            result = obj4;
            obj4 = (object) null;
          }
          else
          {
            object obj5 = await this.reverse_flow_no_change(result, handler, WMBUS_STATUS_BACKFLOW, flowdiff);
            result = obj5;
            obj5 = (object) null;
          }
        }
        else
        {
          object obj6 = await this.reverse_flow_no_change(result, handler, WMBUS_STATUS_BACKFLOW, flowdiff);
          result = obj6;
          obj6 = (object) null;
        }
        System.Windows.Controls.TextBox textBoxResult12 = this.TextBoxResult;
        textBoxResult12.Text = textBoxResult12.Text + "write parameter back " + Environment.NewLine;
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_pulse_back_limit, this.cfg_pulse_back_limit_save);
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_pulse_unback_limit, this.cfg_pulse_unback_limit_save);
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter_save);
        await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
        value = (string) null;
      }
      else
        this.TextBoxResult.Text = "in Mode " + this.cfg_device_mode.ToString() + " smart funktions are not available, please change mode to 1 or 0" + Environment.NewLine;
      return result;
    }

    private async Task<object> reverse_flow_check(
      object result,
      EDCL_HandlerFunctions handler,
      uint WMBUS_STATUS_BACKFLOW,
      long flowdiff)
    {
      this.pulseBackflowCounter = (ushort) 3;
      this.flow_check_counter = (ushort) 1;
      System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
      textBoxResult1.Text = textBoxResult1.Text + "3. Test reverse flow" + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
      textBoxResult2.Text = textBoxResult2.Text + "pulseBackflowCounter set to " + this.pulseBackflowCounter.ToString() + Environment.NewLine;
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_pulse_back_limit, this.cfg_pulse_back_limit);
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.pulseBackflowCounter, this.pulseBackflowCounter);
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
      await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
      System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
      textBoxResult3.Text = textBoxResult3.Text + "Test results" + Environment.NewLine;
      int num1 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num1;
      do
      {
        this.pulseBackflowCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseBackflowCounter);
        this.flow_check_counter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.flow_check_counter);
        this.pulseTotalForwardCount64 = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64);
        System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
        textBoxResult4.Text = textBoxResult4.Text + "pulseBackflowCounter = " + this.pulseBackflowCounter.ToString() + " cfg_pulse_back_limit = " + this.cfg_pulse_back_limit.ToString() + Environment.NewLine;
        this.ShowFlowState(this.project_type, this.pulseFlowState);
        this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
        this.ShowDeviceStatus(this.project_type, this.device_status);
        this.flow_check_counter = (ushort) 1;
        this.pulseTotalForwardCount64 = flowdiff * 4L + this.pulseTotalCompare;
        System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
        textBoxResult5.Text = textBoxResult5.Text + "pulseTotalForwardCount64 set to " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
        textBoxResult6.Text = textBoxResult6.Text + "Change in Meter Reading: " + ((this.pulseTotalForwardCount64 - this.pulseTotalCompare) / 4L).ToString() + Environment.NewLine;
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64, this.pulseTotalForwardCount64);
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
        await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
        await Task.Delay(((int) this.flow_check_counter + 1) * 4000);
        int num2 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
        result = (object) num2;
        this.pulseTotalCompare = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalCompare);
        this.pulseBackflowCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseBackflowCounter);
        this.persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
        this.device_status = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.device_status);
        System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
        textBoxResult7.Text = textBoxResult7.Text + "after flow check pulseTotalCompare =  " + this.pulseTotalCompare.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
        textBoxResult8.Text = textBoxResult8.Text + "after flow check pulseBackflowCounter = " + this.pulseBackflowCounter.ToString() + " cfg_pulse_back_limit = " + this.cfg_pulse_back_limit.ToString() + Environment.NewLine;
        this.TextBoxResult.Text += Environment.NewLine;
      }
      while (this.pulseBackflowCounter != (ushort) 0 && ((int) this.persistent_warning_flags & (int) WMBUS_STATUS_BACKFLOW) != (int) WMBUS_STATUS_BACKFLOW);
      System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
      textBoxResult9.Text = textBoxResult9.Text + "after flow check pulseBackflowCounter = " + this.pulseBackflowCounter.ToString() + " cfg_pulse_unback_limit = " + this.cfg_pulse_unback_limit.ToString() + Environment.NewLine;
      this.ShowFlowState(this.project_type, this.pulseFlowState);
      this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
      this.ShowDeviceStatus(this.project_type, this.device_status);
      if (((int) this.persistent_warning_flags & (int) WMBUS_STATUS_BACKFLOW) == (int) WMBUS_STATUS_BACKFLOW && (int) this.pulseBackflowCounter == (int) this.cfg_pulse_unback_limit)
      {
        this.TextBoxResult.Text += Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult10 = this.TextBoxResult;
        textBoxResult10.Text = textBoxResult10.Text + "test PASSED" + Environment.NewLine;
        this.TextBoxResult.Text += Environment.NewLine;
      }
      else
      {
        this.TextBoxResult.Text += Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult11 = this.TextBoxResult;
        textBoxResult11.Text = textBoxResult11.Text + "test FAILED" + Environment.NewLine;
        this.TextBoxResult.Text += Environment.NewLine;
      }
      return result;
    }

    private async Task<object> reverse_flow_withdraw(
      object result,
      EDCL_HandlerFunctions handler,
      uint WMBUS_STATUS_BACKFLOW,
      long flowdiff)
    {
      this.pulseBackflowCounter = (ushort) 3;
      this.flow_check_counter = (ushort) 1;
      System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
      textBoxResult1.Text = textBoxResult1.Text + "3. Test reverse flow withdraw" + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
      textBoxResult2.Text = textBoxResult2.Text + "pulseBackflowCounter set to " + this.pulseBackflowCounter.ToString() + Environment.NewLine;
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_pulse_unback_limit, this.cfg_pulse_unback_limit);
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.pulseBackflowCounter, this.pulseBackflowCounter);
      await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
      System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
      textBoxResult3.Text = textBoxResult3.Text + "Test results" + Environment.NewLine;
      int num1 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num1;
      do
      {
        this.pulseBackflowCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseBackflowCounter);
        this.flow_check_counter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.flow_check_counter);
        this.pulseTotalForwardCount64 = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64);
        System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
        textBoxResult4.Text = textBoxResult4.Text + "pulseBackflowCounter = " + this.pulseBackflowCounter.ToString() + " cfg_pulse_unback_limit = " + this.cfg_pulse_unback_limit.ToString() + Environment.NewLine;
        this.ShowFlowState(this.project_type, this.pulseFlowState);
        this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
        this.ShowDeviceStatus(this.project_type, this.device_status);
        this.flow_check_counter = (ushort) 1;
        this.pulseTotalForwardCount64 = flowdiff * 4L + this.pulseTotalCompare;
        System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
        textBoxResult5.Text = textBoxResult5.Text + "pulseTotalForwardCount64 set to " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
        textBoxResult6.Text = textBoxResult6.Text + "Change in Meter Reading: " + ((this.pulseTotalForwardCount64 - this.pulseTotalCompare) / 4L).ToString() + Environment.NewLine;
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64, this.pulseTotalForwardCount64);
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
        await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
        await Task.Delay(((int) this.flow_check_counter + 1) * 4000);
        int num2 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
        result = (object) num2;
        this.pulseTotalCompare = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalCompare);
        this.pulseBackflowCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseBackflowCounter);
        this.persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
        this.device_status = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.device_status);
        System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
        textBoxResult7.Text = textBoxResult7.Text + "after flow check pulseTotalCompare =  " + this.pulseTotalCompare.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
        textBoxResult8.Text = textBoxResult8.Text + "after flow check pulseBackflowCounter = " + this.pulseBackflowCounter.ToString() + " cfg_pulse_unback_limit = " + this.cfg_pulse_unback_limit.ToString() + Environment.NewLine;
        this.TextBoxResult.Text += Environment.NewLine;
      }
      while (this.pulseBackflowCounter != (ushort) 0 && ((int) this.persistent_warning_flags & (int) WMBUS_STATUS_BACKFLOW) == (int) WMBUS_STATUS_BACKFLOW);
      System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
      textBoxResult9.Text = textBoxResult9.Text + "after flow check pulseBackflowCounter = " + this.pulseBackflowCounter.ToString() + " cfg_pulse_unback_limit = " + this.cfg_pulse_back_limit.ToString() + Environment.NewLine;
      this.ShowFlowState(this.project_type, this.pulseFlowState);
      this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
      this.ShowDeviceStatus(this.project_type, this.device_status);
      if (((int) this.persistent_warning_flags & (int) WMBUS_STATUS_BACKFLOW) != (int) WMBUS_STATUS_BACKFLOW && (int) this.pulseBackflowCounter == (int) this.cfg_pulse_back_limit)
      {
        this.TextBoxResult.Text += Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult10 = this.TextBoxResult;
        textBoxResult10.Text = textBoxResult10.Text + "test PASSED" + Environment.NewLine;
        this.TextBoxResult.Text += Environment.NewLine;
      }
      else
      {
        this.TextBoxResult.Text += Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult11 = this.TextBoxResult;
        textBoxResult11.Text = textBoxResult11.Text + "test FAILED" + Environment.NewLine;
        this.TextBoxResult.Text += Environment.NewLine;
      }
      return result;
    }

    private async Task<object> reverse_flow_no_change(
      object result,
      EDCL_HandlerFunctions handler,
      uint WMBUS_STATUS_BACKFLOW,
      long flowdiff)
    {
      string Title = ((int) this.persistent_warning_flags & (int) WMBUS_STATUS_BACKFLOW) != (int) WMBUS_STATUS_BACKFLOW ? string.Format("set pulseBackflowCounter x (x<= " + this.cfg_pulse_back_limit.ToString() + " x>0) 3 steps are tested") : string.Format("set pulseBackflowCounter x (x<= " + this.cfg_pulse_unback_limit.ToString() + " x>0)");
      string value = InputWindow.Show(Title, "");
      ushort pulseBackflowCounter = Convert.ToUInt16(value);
      ushort persistent_warning_flagsSave = this.persistent_warning_flags;
      ushort pulseBackflowCounterSave = pulseBackflowCounter;
      System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
      textBoxResult1.Text = textBoxResult1.Text + "3. Test reverse flow no change" + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
      textBoxResult2.Text = textBoxResult2.Text + "pulseBackflowCounter set to " + pulseBackflowCounter.ToString() + Environment.NewLine + Environment.NewLine;
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.pulseBackflowCounter, pulseBackflowCounter);
      await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
      System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
      textBoxResult3.Text = textBoxResult3.Text + "Test results" + Environment.NewLine;
      int num1 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num1;
      int loops = 0;
      for (int i = 0; i < 3; ++i)
      {
        pulseBackflowCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseBackflowCounter);
        this.flow_check_counter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.flow_check_counter);
        this.pulseTotalForwardCount64 = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64);
        System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
        textBoxResult4.Text = textBoxResult4.Text + "pulseBackflowCounter = " + pulseBackflowCounter.ToString() + " cfg_pulse_unback_limit = " + this.cfg_pulse_unback_limit.ToString() + Environment.NewLine;
        this.ShowFlowState(this.project_type, this.pulseFlowState);
        this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
        this.ShowDeviceStatus(this.project_type, this.device_status);
        this.flow_check_counter = (ushort) 1;
        this.pulseTotalForwardCount64 = flowdiff * 4L + this.pulseTotalCompare;
        System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
        textBoxResult5.Text = textBoxResult5.Text + "pulseTotalForwardCount64 set to " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
        textBoxResult6.Text = textBoxResult6.Text + "Change in Meter Reading: " + ((this.pulseTotalForwardCount64 - this.pulseTotalCompare) / 4L).ToString() + Environment.NewLine;
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64, this.pulseTotalForwardCount64);
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
        await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
        await Task.Delay(((int) this.flow_check_counter + 1) * 4000);
        int num2 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
        result = (object) num2;
        this.pulseTotalCompare = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalCompare);
        pulseBackflowCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseBackflowCounter);
        this.persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
        this.device_status = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.device_status);
        System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
        textBoxResult7.Text = textBoxResult7.Text + "after flow check pulseTotalCompare =  " + this.pulseTotalCompare.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
        textBoxResult8.Text = textBoxResult8.Text + "after flow check pulseBackflowCounter = " + pulseBackflowCounter.ToString() + " cfg_pulse_back_limit = " + this.cfg_pulse_back_limit.ToString() + " cfg_pulse_unback_limit = " + this.cfg_pulse_unback_limit.ToString() + Environment.NewLine;
        this.TextBoxResult.Text += Environment.NewLine;
        ++loops;
      }
      this.ShowFlowState(this.project_type, this.pulseFlowState);
      this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
      this.ShowDeviceStatus(this.project_type, this.device_status);
      string ResultText = "";
      ResultText = flowdiff != 0L ? ((int) this.persistent_warning_flags != (int) persistent_warning_flagsSave || ((int) this.persistent_warning_flags & (int) WMBUS_STATUS_BACKFLOW) != (int) WMBUS_STATUS_BACKFLOW || (int) pulseBackflowCounter != (int) this.cfg_pulse_unback_limit && (int) pulseBackflowCounter != (int) pulseBackflowCounterSave + loops ? ((int) this.persistent_warning_flags != (int) persistent_warning_flagsSave || ((int) this.persistent_warning_flags & (int) WMBUS_STATUS_BACKFLOW) == (int) WMBUS_STATUS_BACKFLOW || (int) pulseBackflowCounter != (int) this.cfg_pulse_back_limit && (int) pulseBackflowCounter != (int) pulseBackflowCounterSave + loops ? "test FAILED" : "test PASSED") : "test PASSED") : ((int) this.persistent_warning_flags != (int) persistent_warning_flagsSave || (int) pulseBackflowCounter != (int) pulseBackflowCounterSave ? "test FAILED" : "test PASSED");
      System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
      textBoxResult9.Text = textBoxResult9.Text + Environment.NewLine + ResultText + Environment.NewLine;
      pulseBackflowCounter = pulseBackflowCounterSave;
      object obj = result;
      Title = (string) null;
      value = (string) null;
      ResultText = (string) null;
      return obj;
    }

    private async Task<object> ReadReverseFlowParameter(
      object result,
      EDCL_HandlerFunctions handler)
    {
      int num = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num;
      this.TextBoxResult.Text = "Test leak" + Environment.NewLine + Environment.NewLine;
      this.cfg_pulse_back_limit = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_pulse_back_limit);
      this.cfg_pulse_back_limit_save = this.cfg_pulse_back_limit;
      this.cfg_pulse_unback_limit = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_pulse_unback_limit);
      this.cfg_pulse_unback_limit_save = this.cfg_pulse_back_limit;
      this.pulseBackflowCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseBackflowCounter);
      this.flow_check_counter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.flow_check_counter);
      this.flow_check_counter_save = this.flow_check_counter;
      this.pulseFlowState = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseFlowState);
      this.persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
      this.device_status = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.device_status);
      this.project_type = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.project_type);
      this.pulseTotalForwardCount64 = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64);
      this.pulseTotalCompare = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalCompare);
      this.cfg_device_mode = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.cfg_device_mode);
      return result;
    }

    private async Task<object> TestLeak(object result, EDCL_HandlerFunctions handler)
    {
      uint WMBUS_STATUS_LEAK = 8192;
      object obj1 = await this.ReadLeakParameter(result, handler);
      result = obj1;
      obj1 = (object) null;
      this.cfg_pulse_leak_limit_save = this.cfg_pulse_leak_limit;
      this.cfg_pulse_unleak_limit_save = this.cfg_pulse_unleak_limit;
      this.cfg_pulse_leak_lower_save = this.cfg_pulse_leak_lower;
      this.cfg_pulse_leak_upper_save = this.cfg_pulse_leak_upper;
      if (this.cfg_device_mode != (byte) 8 && this.cfg_device_mode != (byte) 9)
      {
        this.TextBoxResult.Text = "1. Current values" + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
        textBoxResult1.Text = textBoxResult1.Text + "cfg_pulse_leak_limit = " + this.cfg_pulse_leak_limit.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
        textBoxResult2.Text = textBoxResult2.Text + "cfg_pulse_leak_lower = " + this.cfg_pulse_leak_lower.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
        textBoxResult3.Text = textBoxResult3.Text + "cfg_pulse_leak_upper = " + this.cfg_pulse_leak_upper.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
        textBoxResult4.Text = textBoxResult4.Text + "pulseLeakageCounter = " + this.pulseLeakageCounter.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
        textBoxResult5.Text = textBoxResult5.Text + "flow_check_counter = " + this.flow_check_counter.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
        textBoxResult6.Text = textBoxResult6.Text + "pulseTotalForwardCount64 = " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
        textBoxResult7.Text = textBoxResult7.Text + "pulseTotalCompare = " + this.pulseTotalCompare.ToString() + Environment.NewLine;
        this.ShowFlowState(this.project_type, this.pulseFlowState);
        this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
        this.ShowDeviceStatus(this.project_type, this.device_status);
        this.TextBoxResult.Text += Environment.NewLine;
        this.cfg_pulse_leak_lower = (short) -10;
        this.cfg_pulse_leak_upper = (short) 10;
        this.flow_check_counter = (ushort) 1;
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<short>(EDCL_Params.cfg_pulse_leak_lower, this.cfg_pulse_leak_lower);
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<short>(EDCL_Params.cfg_pulse_leak_upper, this.cfg_pulse_leak_upper);
        await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
        await Task.Delay(((int) this.flow_check_counter + 1) * 2000);
        object obj2 = await this.ReadLeakParameter(result, handler);
        result = obj2;
        obj2 = (object) null;
        System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
        textBoxResult8.Text = textBoxResult8.Text + "2. Pre condition" + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
        textBoxResult9.Text = textBoxResult9.Text + "cfg_pulse_leak_limit = " + this.cfg_pulse_leak_limit.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult10 = this.TextBoxResult;
        textBoxResult10.Text = textBoxResult10.Text + "cfg_pulse_leak_lower = " + this.cfg_pulse_leak_lower.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult11 = this.TextBoxResult;
        textBoxResult11.Text = textBoxResult11.Text + "cfg_pulse_leak_upper = " + this.cfg_pulse_leak_upper.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult12 = this.TextBoxResult;
        textBoxResult12.Text = textBoxResult12.Text + "pulseLeakageCounter = " + this.pulseLeakageCounter.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult13 = this.TextBoxResult;
        textBoxResult13.Text = textBoxResult13.Text + "flow_check_counter = " + this.flow_check_counter.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult14 = this.TextBoxResult;
        textBoxResult14.Text = textBoxResult14.Text + "pulseTotalForwardCount64 = " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult15 = this.TextBoxResult;
        textBoxResult15.Text = textBoxResult15.Text + "pulseTotalCompare = " + this.pulseTotalCompare.ToString() + Environment.NewLine;
        this.ShowFlowState(this.project_type, this.pulseFlowState);
        this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
        this.ShowDeviceStatus(this.project_type, this.device_status);
        this.TextBoxResult.Text += Environment.NewLine;
        string value = InputWindow.Show("set pulse difference (-9, -20, 0, 10 or 100 )", "");
        long flowdiff = Convert.ToInt64(value);
        if (flowdiff == 0L)
        {
          if (((int) this.persistent_warning_flags & (int) WMBUS_STATUS_LEAK) == (int) WMBUS_STATUS_LEAK)
          {
            object obj3 = await this.leak_check_unleak(result, handler, WMBUS_STATUS_LEAK, flowdiff);
            result = obj3;
            obj3 = (object) null;
          }
          else
          {
            object obj4 = await this.leak_check_normal(result, handler, WMBUS_STATUS_LEAK, flowdiff);
            result = obj4;
            obj4 = (object) null;
          }
        }
        else if ((long) this.cfg_pulse_leak_upper >= flowdiff && flowdiff >= (long) this.cfg_pulse_leak_lower)
        {
          object obj5 = await this.leak_check_leak(result, handler, WMBUS_STATUS_LEAK, flowdiff);
          result = obj5;
          obj5 = (object) null;
        }
        else
        {
          object obj6 = await this.leak_check_normal(result, handler, WMBUS_STATUS_LEAK, flowdiff);
          result = obj6;
          obj6 = (object) null;
        }
        System.Windows.Controls.TextBox textBoxResult16 = this.TextBoxResult;
        textBoxResult16.Text = textBoxResult16.Text + "write parameter back " + Environment.NewLine;
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_pulse_leak_limit, this.cfg_pulse_leak_limit_save);
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter_save);
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<short>(EDCL_Params.cfg_pulse_leak_lower, this.cfg_pulse_leak_lower_save);
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<short>(EDCL_Params.cfg_pulse_leak_upper, this.cfg_pulse_leak_upper_save);
        await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
        value = (string) null;
      }
      else
        this.TextBoxResult.Text = "in Mode " + this.cfg_device_mode.ToString() + " smart funktions are not available, please change mode to 1 or 0" + Environment.NewLine;
      return result;
    }

    private async Task<object> ReadLeakParameter(object result, EDCL_HandlerFunctions handler)
    {
      int num = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num;
      System.Windows.Controls.TextBox textBoxResult = this.TextBoxResult;
      textBoxResult.Text = textBoxResult.Text + "Test leak" + Environment.NewLine + Environment.NewLine;
      this.cfg_pulse_leak_limit = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_pulse_leak_limit);
      this.cfg_pulse_unleak_limit = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_pulse_unleak_limit);
      this.cfg_pulse_leak_lower = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<short>(EDCL_Params.cfg_pulse_leak_lower);
      this.cfg_pulse_leak_upper = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<short>(EDCL_Params.cfg_pulse_leak_upper);
      this.pulseLeakageCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseLeakageCounter);
      this.flow_check_counter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.flow_check_counter);
      this.flow_check_counter_save = this.flow_check_counter;
      this.pulseFlowState = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseFlowState);
      this.persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
      this.device_status = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.device_status);
      this.project_type = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.project_type);
      this.pulseTotalForwardCount64 = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64);
      this.pulseTotalCompare = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalCompare);
      this.cfg_device_mode = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(EDCL_Params.cfg_device_mode);
      return result;
    }

    private async Task<object> leak_check_leak(
      object result,
      EDCL_HandlerFunctions handler,
      uint WMBUS_STATUS_LEAK,
      long flowdiff)
    {
      this.pulseLeakageCounter = (ushort) 3;
      this.cfg_pulse_leak_limit = (ushort) 3;
      this.flow_check_counter = (ushort) 1;
      System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
      textBoxResult1.Text = textBoxResult1.Text + "3. Test leak" + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
      textBoxResult2.Text = textBoxResult2.Text + "cfg_pulse_leak_limit set to " + this.cfg_pulse_leak_limit.ToString() + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
      textBoxResult3.Text = textBoxResult3.Text + "pulseLeakageCounter set to " + this.pulseLeakageCounter.ToString() + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
      textBoxResult4.Text = textBoxResult4.Text + "flow_check_counter set to " + this.flow_check_counter.ToString() + Environment.NewLine + Environment.NewLine;
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_pulse_leak_limit, this.cfg_pulse_leak_limit);
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.pulseLeakageCounter, this.pulseLeakageCounter);
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
      await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
      await Task.Delay(((int) this.flow_check_counter + 1) * 2000);
      System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
      textBoxResult5.Text = textBoxResult5.Text + "Test results" + Environment.NewLine;
      int num1 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num1;
      do
      {
        this.pulseLeakageCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseLeakageCounter);
        this.flow_check_counter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.flow_check_counter);
        this.pulseTotalForwardCount64 = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64);
        System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
        textBoxResult6.Text = textBoxResult6.Text + "pulseLeakageCounter = " + this.pulseLeakageCounter.ToString() + " cfg_pulse_leak_limit = " + this.cfg_pulse_leak_limit.ToString() + Environment.NewLine;
        this.ShowFlowState(this.project_type, this.pulseFlowState);
        this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
        this.ShowDeviceStatus(this.project_type, this.device_status);
        this.flow_check_counter = (ushort) 1;
        this.pulseTotalForwardCount64 = flowdiff * 4L + this.pulseTotalCompare;
        System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
        textBoxResult7.Text = textBoxResult7.Text + "pulseTotalForwardCount64 set to " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
        textBoxResult8.Text = textBoxResult8.Text + "Change in Meter Reading: " + ((this.pulseTotalForwardCount64 - this.pulseTotalCompare) / 4L).ToString() + Environment.NewLine;
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64, this.pulseTotalForwardCount64);
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
        await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
        await Task.Delay(((int) this.flow_check_counter + 1) * 4000);
        int num2 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
        result = (object) num2;
        this.pulseTotalCompare = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalCompare);
        this.pulseLeakageCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseLeakageCounter);
        this.persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
        this.device_status = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.device_status);
        System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
        textBoxResult9.Text = textBoxResult9.Text + "after flow check pulseTotalCompare =  " + this.pulseTotalCompare.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult10 = this.TextBoxResult;
        textBoxResult10.Text = textBoxResult10.Text + "after flow check pulseLeakageCounter = " + this.pulseLeakageCounter.ToString() + " cfg_pulse_leak_limit = " + this.cfg_pulse_leak_limit.ToString() + Environment.NewLine;
        this.TextBoxResult.Text += Environment.NewLine;
      }
      while (this.pulseLeakageCounter > (ushort) 0 && ((int) this.persistent_warning_flags & (int) WMBUS_STATUS_LEAK) != (int) WMBUS_STATUS_LEAK);
      System.Windows.Controls.TextBox textBoxResult11 = this.TextBoxResult;
      textBoxResult11.Text = textBoxResult11.Text + "after flow check pulseLeakageCounter = " + this.pulseLeakageCounter.ToString() + " cfg_pulse_unleak_limit = " + this.cfg_pulse_unleak_limit.ToString() + Environment.NewLine;
      this.ShowFlowState(this.project_type, this.pulseFlowState);
      this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
      this.ShowDeviceStatus(this.project_type, this.device_status);
      if (((int) this.persistent_warning_flags & (int) WMBUS_STATUS_LEAK) == (int) WMBUS_STATUS_LEAK && (int) this.pulseLeakageCounter == (int) this.cfg_pulse_unleak_limit)
      {
        System.Windows.Controls.TextBox textBoxResult12 = this.TextBoxResult;
        textBoxResult12.Text = textBoxResult12.Text + "test PASSED" + Environment.NewLine;
      }
      else
      {
        System.Windows.Controls.TextBox textBoxResult13 = this.TextBoxResult;
        textBoxResult13.Text = textBoxResult13.Text + "test FAILED" + Environment.NewLine;
      }
      return result;
    }

    private async Task<object> leak_check_unleak(
      object result,
      EDCL_HandlerFunctions handler,
      uint WMBUS_STATUS_LEAK,
      long flowdiff)
    {
      this.flow_check_counter = (ushort) 1;
      this.cfg_pulse_leak_limit = (ushort) 20;
      this.cfg_pulse_unleak_limit = (ushort) 50;
      this.pulseLeakageCounter = (ushort) 3;
      System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
      textBoxResult1.Text = textBoxResult1.Text + "3. Test unleak" + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
      textBoxResult2.Text = textBoxResult2.Text + "cfg_pulse_leak_limit set to " + this.cfg_pulse_leak_limit.ToString() + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
      textBoxResult3.Text = textBoxResult3.Text + "cfg_pulse_unleak_limit set to " + this.cfg_pulse_unleak_limit.ToString() + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
      textBoxResult4.Text = textBoxResult4.Text + "pulseLeakageCounter set to " + this.pulseLeakageCounter.ToString() + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
      textBoxResult5.Text = textBoxResult5.Text + "flow_check_counter set to " + this.flow_check_counter.ToString() + Environment.NewLine + Environment.NewLine;
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_pulse_leak_limit, this.cfg_pulse_leak_limit);
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_pulse_unleak_limit, this.cfg_pulse_unleak_limit);
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.pulseLeakageCounter, this.pulseLeakageCounter);
      await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
      await Task.Delay(((int) this.flow_check_counter + 1) * 2000);
      System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
      textBoxResult6.Text = textBoxResult6.Text + "Test results" + Environment.NewLine;
      int num1 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num1;
      do
      {
        this.pulseLeakageCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseLeakageCounter);
        this.flow_check_counter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.flow_check_counter);
        this.pulseTotalForwardCount64 = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64);
        System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
        textBoxResult7.Text = textBoxResult7.Text + "pulseLeakageCounter = " + this.pulseLeakageCounter.ToString() + " cfg_pulse_unleak_limit = " + this.cfg_pulse_unleak_limit.ToString() + Environment.NewLine;
        this.ShowFlowState(this.project_type, this.pulseFlowState);
        this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
        this.ShowDeviceStatus(this.project_type, this.device_status);
        this.flow_check_counter = (ushort) 1;
        this.pulseTotalForwardCount64 = flowdiff * 4L + this.pulseTotalCompare;
        System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
        textBoxResult8.Text = textBoxResult8.Text + "pulseTotalForwardCount64 set to " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
        textBoxResult9.Text = textBoxResult9.Text + "Change in Meter Reading: " + ((this.pulseTotalForwardCount64 - this.pulseTotalCompare) / 4L).ToString() + Environment.NewLine;
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64, this.pulseTotalForwardCount64);
        handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
        await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
        await Task.Delay(((int) this.flow_check_counter + 1) * 4000);
        int num2 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
        result = (object) num2;
        this.pulseTotalCompare = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalCompare);
        this.pulseLeakageCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseLeakageCounter);
        this.persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
        this.device_status = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.device_status);
        System.Windows.Controls.TextBox textBoxResult10 = this.TextBoxResult;
        textBoxResult10.Text = textBoxResult10.Text + "after flow check pulseTotalCompare =  " + this.pulseTotalCompare.ToString() + Environment.NewLine;
        System.Windows.Controls.TextBox textBoxResult11 = this.TextBoxResult;
        textBoxResult11.Text = textBoxResult11.Text + "after flow check pulseLeakageCounter = " + this.pulseLeakageCounter.ToString() + " cfg_pulse_unleak_limit = " + this.cfg_pulse_unleak_limit.ToString() + Environment.NewLine;
        this.TextBoxResult.Text += Environment.NewLine;
      }
      while (this.pulseLeakageCounter != (ushort) 0 && ((int) this.persistent_warning_flags & (int) WMBUS_STATUS_LEAK) == (int) WMBUS_STATUS_LEAK);
      System.Windows.Controls.TextBox textBoxResult12 = this.TextBoxResult;
      textBoxResult12.Text = textBoxResult12.Text + "after flow check pulseLeakageCounter = " + this.pulseLeakageCounter.ToString() + " cfg_pulse_leak_limit = " + this.cfg_pulse_leak_limit.ToString() + Environment.NewLine;
      if (((int) this.persistent_warning_flags & (int) WMBUS_STATUS_LEAK) != (int) WMBUS_STATUS_LEAK && (int) this.pulseLeakageCounter == (int) this.cfg_pulse_leak_limit)
      {
        System.Windows.Controls.TextBox textBoxResult13 = this.TextBoxResult;
        textBoxResult13.Text = textBoxResult13.Text + "test PASSED" + Environment.NewLine;
      }
      else
      {
        System.Windows.Controls.TextBox textBoxResult14 = this.TextBoxResult;
        textBoxResult14.Text = textBoxResult14.Text + "test FAILED" + Environment.NewLine;
      }
      this.ShowFlowState(this.project_type, this.pulseFlowState);
      this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
      this.ShowDeviceStatus(this.project_type, this.device_status);
      return result;
    }

    private async Task<object> leak_check_normal(
      object result,
      EDCL_HandlerFunctions handler,
      uint WMBUS_STATUS_LEAK,
      long flowdiff)
    {
      this.cfg_pulse_leak_limit = (ushort) 20;
      this.cfg_pulse_unleak_limit = (ushort) 50;
      this.pulseLeakageCounter = ((int) this.persistent_warning_flags & (int) WMBUS_STATUS_LEAK) != (int) WMBUS_STATUS_LEAK ? (ushort) ((uint) this.cfg_pulse_leak_limit - 1U) : (ushort) ((uint) this.cfg_pulse_unleak_limit - 1U);
      this.flow_check_counter = (ushort) 1;
      System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
      textBoxResult1.Text = textBoxResult1.Text + "3. Test normal" + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
      textBoxResult2.Text = textBoxResult2.Text + "cfg_pulse_leak_limit set to " + this.cfg_pulse_leak_limit.ToString() + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
      textBoxResult3.Text = textBoxResult3.Text + "cfg_pulse_unleak_limit set to " + this.cfg_pulse_unleak_limit.ToString() + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
      textBoxResult4.Text = textBoxResult4.Text + "pulseLeakageCounter set to " + this.pulseLeakageCounter.ToString() + Environment.NewLine;
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_pulse_leak_limit, this.cfg_pulse_leak_limit);
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.pulseLeakageCounter, this.pulseLeakageCounter);
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.cfg_pulse_unleak_limit, this.cfg_pulse_unleak_limit);
      await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
      System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
      textBoxResult5.Text = textBoxResult5.Text + "Test results" + Environment.NewLine;
      int num1 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num1;
      this.pulseLeakageCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseLeakageCounter);
      this.flow_check_counter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.flow_check_counter);
      this.pulseTotalForwardCount64 = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64);
      System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
      textBoxResult6.Text = textBoxResult6.Text + "pulseLeakageCounter = " + this.pulseLeakageCounter.ToString() + " cfg_pulse_leak_limit = " + this.cfg_pulse_leak_limit.ToString() + Environment.NewLine;
      this.ShowFlowState(this.project_type, this.pulseFlowState);
      this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
      this.ShowDeviceStatus(this.project_type, this.device_status);
      this.flow_check_counter = (ushort) 1;
      this.pulseTotalForwardCount64 = flowdiff * 4L + this.pulseTotalCompare;
      System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
      textBoxResult7.Text = textBoxResult7.Text + "pulseTotalForwardCount64 set to " + this.pulseTotalForwardCount64.ToString() + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
      textBoxResult8.Text = textBoxResult8.Text + "Change in Meter Reading: " + ((this.pulseTotalForwardCount64 - this.pulseTotalCompare) / 4L).ToString() + Environment.NewLine;
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<long>(EDCL_Params.pulseTotalForwardCount64, this.pulseTotalForwardCount64);
      await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
      int num2 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num2;
      handler.myMeters.WorkMeter.meterMemory.SetParameterValue<ushort>(EDCL_Params.flow_check_counter, this.flow_check_counter);
      await this.WriteDeviceAsyncWithoutReset(this.progress, this.tokenSource.Token);
      await Task.Delay(((int) this.flow_check_counter + 1) * 4000);
      int num3 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
      result = (object) num3;
      this.pulseTotalCompare = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<long>(EDCL_Params.pulseTotalCompare);
      this.pulseLeakageCounter = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.pulseLeakageCounter);
      this.cfg_pulse_leak_limit = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_pulse_leak_limit);
      this.cfg_pulse_unleak_limit = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.cfg_pulse_unleak_limit);
      this.persistent_warning_flags = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.persistent_warning_flags);
      this.device_status = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<ushort>(EDCL_Params.device_status);
      System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
      textBoxResult9.Text = textBoxResult9.Text + "after flow check pulseTotalCompare =  " + this.pulseTotalCompare.ToString() + Environment.NewLine;
      System.Windows.Controls.TextBox textBoxResult10 = this.TextBoxResult;
      textBoxResult10.Text = textBoxResult10.Text + "after flow check pulseLeakageCounter = " + this.pulseLeakageCounter.ToString() + " cfg_pulse_leak_limit = " + this.cfg_pulse_leak_limit.ToString() + Environment.NewLine;
      this.TextBoxResult.Text += Environment.NewLine;
      if (((int) this.persistent_warning_flags & (int) WMBUS_STATUS_LEAK) == (int) WMBUS_STATUS_LEAK)
      {
        System.Windows.Controls.TextBox textBoxResult11 = this.TextBoxResult;
        textBoxResult11.Text = textBoxResult11.Text + "after flow check pulseLeakageCounter = " + this.pulseLeakageCounter.ToString() + " cfg_pulse_unleak_limit = " + this.cfg_pulse_unleak_limit.ToString() + Environment.NewLine;
        if ((int) this.pulseLeakageCounter == (int) this.cfg_pulse_unleak_limit)
        {
          System.Windows.Controls.TextBox textBoxResult12 = this.TextBoxResult;
          textBoxResult12.Text = textBoxResult12.Text + Environment.NewLine + "test PASSED pulseLeakageCounter == cfg_pulse_unleak_limit" + Environment.NewLine;
        }
        else
        {
          System.Windows.Controls.TextBox textBoxResult13 = this.TextBoxResult;
          textBoxResult13.Text = textBoxResult13.Text + Environment.NewLine + "test FAILED pulseLeakageCounter != cfg_pulse_unleak_limit" + Environment.NewLine;
        }
      }
      else
      {
        System.Windows.Controls.TextBox textBoxResult14 = this.TextBoxResult;
        textBoxResult14.Text = textBoxResult14.Text + "after flow check pulseLeakageCounter = " + this.pulseLeakageCounter.ToString() + " cfg_pulse_leak_limit = " + this.cfg_pulse_leak_limit.ToString() + Environment.NewLine;
        if ((int) this.pulseLeakageCounter == (int) this.cfg_pulse_leak_limit)
        {
          System.Windows.Controls.TextBox textBoxResult15 = this.TextBoxResult;
          textBoxResult15.Text = textBoxResult15.Text + Environment.NewLine + "test PASSED pulseLeakageCounter == cfg_pulse_leak_limit" + Environment.NewLine;
        }
        else
        {
          System.Windows.Controls.TextBox textBoxResult16 = this.TextBoxResult;
          textBoxResult16.Text = textBoxResult16.Text + Environment.NewLine + "test FAILED pulseLeakageCounter != cfg_pulse_leak_limit" + Environment.NewLine;
        }
      }
      this.ShowFlowState(this.project_type, this.pulseFlowState);
      this.ShowPersistentWarningFlags(this.project_type, this.persistent_warning_flags);
      this.ShowDeviceStatus(this.project_type, this.device_status);
      return result;
    }

    private async Task<object> fillLogger(object result, EDCL_HandlerFunctions handler)
    {
      CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
      DateTime currentTime = DateTime.Now;
      currentTime = currentTime.AddYears(-2);
      DateTime timeToChangeToDayFilling = DateTime.Now;
      timeToChangeToDayFilling.AddDays(32.0);
      uint counterCH0 = 1234567;
      System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
      textBoxResult1.Text = textBoxResult1.Text + "fill months and key logger" + Environment.NewLine;
      uint i = 0;
      while (currentTime < DateTime.Now)
      {
        if (currentTime < timeToChangeToDayFilling)
        {
          currentTime = currentTime.AddMonths(1);
          currentTime = new DateTime(currentTime.Year, currentTime.Month, 1, 0, 0, 0);
          currentTime = currentTime.AddSeconds(-2.0);
          if (i % 2U == 0U)
            counterCH0 += i + 77U;
          else
            counterCH0 -= i + 33U;
          DateTime hmTime = new DateTime(currentTime.Year, currentTime.Month, 15, 23, 59, 57);
          ++counterCH0;
          System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
          textBoxResult2.Text = textBoxResult2.Text + "set counter ch0 " + counterCH0.ToString() + Environment.NewLine;
          await handler.cmd.MBusCmd.SetChannelValueAsync((byte) 0, counterCH0, this.progress, cancelTokenSource.Token);
          System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
          textBoxResult3.Text = textBoxResult3.Text + "set time half month " + hmTime.ToString() + Environment.NewLine;
          await handler.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(hmTime, (sbyte) 1), this.progress, this.tokenSource.Token);
          await Task.Delay(4000);
          System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
          textBoxResult4.Text = textBoxResult4.Text + "set counter ch0 " + counterCH0.ToString() + Environment.NewLine;
          await handler.cmd.MBusCmd.SetChannelValueAsync((byte) 0, counterCH0, this.progress, cancelTokenSource.Token);
          System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
          textBoxResult5.Text = textBoxResult5.Text + "set time full month " + currentTime.ToString() + Environment.NewLine;
          await handler.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(currentTime, (sbyte) 1), this.progress, this.tokenSource.Token);
          await Task.Delay(4000);
          currentTime = currentTime.AddMonths(1);
        }
        if (currentTime >= DateTime.Now)
        {
          currentTime = currentTime.AddMonths(-1);
          currentTime = currentTime.AddDays(1.0);
          this.TextBoxResult.Text += Environment.NewLine;
          System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
          textBoxResult6.Text = textBoxResult6.Text + "fill day logger" + Environment.NewLine;
          for (uint k = 0; k <= 1U; ++k)
          {
            uint loggervalues = 32;
            if (k == 1U)
            {
              this.TextBoxResult.Text += Environment.NewLine;
              System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
              textBoxResult7.Text = textBoxResult7.Text + "fill hour logger" + Environment.NewLine;
              loggervalues = 4U;
            }
            for (uint j = 0; j <= loggervalues; ++j)
            {
              if (j % 2U == 0U)
                counterCH0 += j + 77U;
              else
                counterCH0 -= j + 33U;
              System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
              textBoxResult8.Text = textBoxResult8.Text + "set counter ch0 " + counterCH0.ToString() + Environment.NewLine;
              await handler.cmd.MBusCmd.SetChannelValueAsync((byte) 0, counterCH0, this.progress, cancelTokenSource.Token);
              System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
              textBoxResult9.Text = textBoxResult9.Text + "set time  " + currentTime.ToString() + Environment.NewLine;
              await handler.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(currentTime, (sbyte) 1), this.progress, this.tokenSource.Token);
              await Task.Delay(4000);
              if (k == 0U)
              {
                currentTime = currentTime.AddDays(1.0);
              }
              else
              {
                currentTime = currentTime.AddHours(1.0);
                currentTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, 59, 57);
              }
            }
          }
        }
        ++i;
      }
      object obj = result;
      cancelTokenSource = (CancellationTokenSource) null;
      return obj;
    }

    private void Window_Closing(object sender, CancelEventArgs e) => this.exitSendNext = true;

    private void ButtonSAP_Click(object sender, RoutedEventArgs e)
    {
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      System.Windows.Application.LoadComponent((object) this, new Uri("/EDCL_Handler;component/ui/testwindows.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((Window) target).Closing += new CancelEventHandler(this.Window_Closing);
          break;
        case 2:
          this.TextBlockStatus = (TextBlock) target;
          break;
        case 3:
          this.ProgressBar1 = (System.Windows.Controls.ProgressBar) target;
          break;
        case 4:
          this.TextBoxResult = (System.Windows.Controls.TextBox) target;
          break;
        case 5:
          this.StackPanalButtons = (StackPanel) target;
          break;
        case 6:
          this.ButtonCalibrate_VCC2 = (System.Windows.Controls.Button) target;
          this.ButtonCalibrate_VCC2.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 7:
          this.ButtonAddNewMap = (System.Windows.Controls.Button) target;
          this.ButtonAddNewMap.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 8:
          this.ButtonSendNextLoRa = (System.Windows.Controls.Button) target;
          this.ButtonSendNextLoRa.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 9:
          this.ButtonSendSP9T1 = (System.Windows.Controls.Button) target;
          this.ButtonSendSP9T1.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 10:
          this.ButtonShowPersistWarnings = (System.Windows.Controls.Button) target;
          this.ButtonShowPersistWarnings.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 11:
          this.ButtonTestLeak = (System.Windows.Controls.Button) target;
          this.ButtonTestLeak.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 12:
          this.ButtonTestReverseFlow = (System.Windows.Controls.Button) target;
          this.ButtonTestReverseFlow.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 13:
          this.ButtonTestBlockage = (System.Windows.Controls.Button) target;
          this.ButtonTestBlockage.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 14:
          this.ButtonTestBurst = (System.Windows.Controls.Button) target;
          this.ButtonTestBurst.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 15:
          this.ButtonMeterIsOversized = (System.Windows.Controls.Button) target;
          this.ButtonMeterIsOversized.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 16:
          this.ButtonMeterIsUndersized = (System.Windows.Controls.Button) target;
          this.ButtonMeterIsUndersized.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 17:
          this.ButtonGetConfigurationParameter = (System.Windows.Controls.Button) target;
          this.ButtonGetConfigurationParameter.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 18:
          this.ButtonSetMode = (System.Windows.Controls.Button) target;
          this.ButtonSetMode.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 19:
          this.ButtonHardwareTypeEditor = (System.Windows.Controls.Button) target;
          this.ButtonHardwareTypeEditor.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 20:
          this.ButtonProtection = (System.Windows.Controls.Button) target;
          this.ButtonProtection.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 21:
          this.ButtonReadVersion = (System.Windows.Controls.Button) target;
          this.ButtonReadVersion.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 22:
          this.ButtonGetRadioVersion = (System.Windows.Controls.Button) target;
          this.ButtonGetRadioVersion.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 23:
          this.ButtonSAP = (System.Windows.Controls.Button) target;
          this.ButtonSAP.Click += new RoutedEventHandler(this.ButtonSAP_Click);
          break;
        case 24:
          this.ButtonTransmitModulatedCarrier = (System.Windows.Controls.Button) target;
          this.ButtonTransmitModulatedCarrier.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 25:
          this.ButtonTransmitUnmodulatedCarrier = (System.Windows.Controls.Button) target;
          this.ButtonTransmitUnmodulatedCarrier.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 26:
          this.ButtonSetFrequencyIncrement = (System.Windows.Controls.Button) target;
          this.ButtonSetFrequencyIncrement.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 27:
          this.ButtonSendTestPacket = (System.Windows.Controls.Button) target;
          this.ButtonSendTestPacket.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 28:
          this.ButtonReceiveTestPacket = (System.Windows.Controls.Button) target;
          this.ButtonReceiveTestPacket.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 29:
          this.ButtonSendTestPacketViaMiCon = (System.Windows.Controls.Button) target;
          this.ButtonSendTestPacketViaMiCon.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 30:
          this.ButtonReceiveTestPacketViaMiCon = (System.Windows.Controls.Button) target;
          this.ButtonReceiveTestPacketViaMiCon.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 31:
          this.ButtonSetDeviceIdentification = (System.Windows.Controls.Button) target;
          this.ButtonSetDeviceIdentification.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 32:
          this.ButtonReadSystemTime = (System.Windows.Controls.Button) target;
          this.ButtonReadSystemTime.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 33:
          this.ButtonWriteSystemTime = (System.Windows.Controls.Button) target;
          this.ButtonWriteSystemTime.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 34:
          this.ButtonDisableIrDa = (System.Windows.Controls.Button) target;
          this.ButtonDisableIrDa.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 35:
          this.ButtonCompareConnectedAndWork = (System.Windows.Controls.Button) target;
          this.ButtonCompareConnectedAndWork.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 36:
          this.ButtonShowPublicKey = (System.Windows.Controls.Button) target;
          this.ButtonShowPublicKey.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 37:
          this.ButtonAccessRadioKey = (System.Windows.Controls.Button) target;
          this.ButtonAccessRadioKey.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 38:
          this.ButtonGetCalibration = (System.Windows.Controls.Button) target;
          this.ButtonGetCalibration.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 39:
          this.ButtonFillLogger = (System.Windows.Controls.Button) target;
          this.ButtonFillLogger.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 40:
          this.ButtonBatchFirmwareUpdate = (System.Windows.Controls.Button) target;
          this.ButtonBatchFirmwareUpdate.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 41:
          this.ButtonBreak = (System.Windows.Controls.Button) target;
          this.ButtonBreak.Click += new RoutedEventHandler(this.ButtonBreak_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
