// Decompiled with JetBrains decompiler
// Type: THL_Handler.UserInterface.TestWindows
// Assembly: THL_Handler, Version=1.0.5.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: C9669406-A704-45DE-B726-D8A41F27FFB8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\THL_Handler.dll

using CommunicationPort;
using CommunicationPort.Functions;
using GmmDbLib;
using HandlerLib;
using NLog;
using ReadoutConfiguration;
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
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace THL_Handler.UserInterface
{
  public class TestWindows : Window, IComponentConnector
  {
    private static Logger logger = LogManager.GetLogger(nameof (TestWindows));
    private THL_HandlerWindowFunctions windowFunctions;
    private ProgressHandler progress;
    private CancellationTokenSource tokenSource;
    private bool exitSendNext;
    internal TextBlock TextBlockStatus;
    internal System.Windows.Controls.ProgressBar ProgressBar1;
    internal System.Windows.Controls.TextBox TextBoxResult;
    internal StackPanel StackPanalButtons;
    internal System.Windows.Controls.Button ButtonReadTemperature;
    internal System.Windows.Controls.Button ButtonReadHumidity;
    internal System.Windows.Controls.Button ButtonReadLogger;
    internal System.Windows.Controls.Button ButtonRadioTest;
    internal System.Windows.Controls.Button ButtonSendNextLoRa;
    internal System.Windows.Controls.Button ButtonSendSP9T1;
    internal System.Windows.Controls.Button ButtonReadWrite_SerialNumberFull_DevEUI_JoinEUI_DevKey;
    internal System.Windows.Controls.Button ButtonGetConfigurationParameter;
    internal System.Windows.Controls.Button ButtonSetMode;
    internal System.Windows.Controls.Button ButtonSwitchState;
    internal System.Windows.Controls.Button ButtonHardwareTypeManager;
    internal System.Windows.Controls.Button ButtonAddNewMap;
    internal System.Windows.Controls.Button ButtonProtection;
    internal System.Windows.Controls.Button ButtonReadVersion;
    internal System.Windows.Controls.Button ButtonGetRadioVersion;
    internal System.Windows.Controls.Button ButtonTransmitModulatedCarrier;
    internal System.Windows.Controls.Button ButtonTransmitUnmodulatedCarrier;
    internal System.Windows.Controls.Button ButtonSetFrequencyIncrement;
    internal System.Windows.Controls.Button ButtonSendTestPacket;
    internal System.Windows.Controls.Button ButtonReceiveTestPacket;
    internal System.Windows.Controls.Button ButtonReceiveTestPacketViaMiCon;
    internal System.Windows.Controls.Button ButtonSendTestPacketViaMiCon;
    internal System.Windows.Controls.Button ButtonSetLcdTestState;
    internal System.Windows.Controls.Button ButtonSetDeviceIdentification;
    internal System.Windows.Controls.Button ButtonReadSystemTime;
    internal System.Windows.Controls.Button ButtonWriteSystemTime;
    internal System.Windows.Controls.Button ButtonCompareConnectedAndWork;
    internal System.Windows.Controls.Button ButtonAccessRadioKey;
    internal System.Windows.Controls.Button ButtonGetMbus_interval;
    internal System.Windows.Controls.Button ButtonBreak;
    private bool _contentLoaded;

    public TestWindows(Window owner, THL_HandlerWindowFunctions windowFunctions)
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
      this.ButtonBreak.IsEnabled = true;
      this.Cursor = System.Windows.Input.Cursors.Wait;
      this.TextBoxResult.Text = string.Empty;
    }

    private void SetStopState()
    {
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
        THL_HandlerFunctions handler = this.windowFunctions.MyFunctions;
        if (sender == this.ButtonReadVersion)
        {
          FirmwareVersion firmwareVersion = await handler.ReadVersionAsync(this.progress, this.tokenSource.Token);
          result = (object) firmwareVersion;
          firmwareVersion = new FirmwareVersion();
        }
        else if (sender == this.ButtonHardwareTypeManager)
        {
          string[] hardwareNames = new string[2]
          {
            "TH_LoRa",
            "TH_sensor_wMBus"
          };
          HardwareTypeEditor dlg = handler.myMeters.WorkMeter == null ? new HardwareTypeEditor(hardwareNames) : new HardwareTypeEditor(hardwareNames, new uint?(handler.myMeters.WorkMeter.meterMemory.FirmwareVersion));
          dlg.ShowDialog();
          hardwareNames = (string[]) null;
          dlg = (HardwareTypeEditor) null;
        }
        else if (sender == this.ButtonReadWrite_SerialNumberFull_DevEUI_JoinEUI_DevKey)
        {
          int numberOfIds = 1;
          IdContainer idContainer = DbBasis.PrimaryDB.BaseDbConnection.GetNewIds("SerialNumber_HuT_LoRa", numberOfIds);
          int serialnumber = idContainer.GetNextID();
          int num = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
          SortedList<OverrideID, ConfigurationParameter> configParameters = handler.GetConfigurationParameters(0);
          configParameters[OverrideID.SerialNumberFull].ParameterValue = (object) Utility.ConvertSerialNumberToFullserialNumber(Utility.LoraDevice.THL, serialnumber);
          configParameters[OverrideID.DevEUI].ParameterValue = (object) Utility.ConvertSerialNumberToEUI64(Utility.LoraDevice.THL, serialnumber);
          configParameters[OverrideID.JoinEUI].ParameterValue = (object) "04B6480000000001";
          configParameters[OverrideID.AppKey].ParameterValue = (object) Utility.GetRandomAppKey();
          handler.SetConfigurationParameters(configParameters, 0);
          await handler.WriteDeviceAsync(this.progress, this.tokenSource.Token);
          idContainer = (IdContainer) null;
          configParameters = (SortedList<OverrideID, ConfigurationParameter>) null;
        }
        else if (sender == this.ButtonGetRadioVersion)
        {
          ushort radioVersionAsync = await handler.cmd.Radio.GetRadioVersionAsync(this.progress, this.tokenSource.Token);
          result = (object) radioVersionAsync;
        }
        else if (sender == this.ButtonAccessRadioKey)
        {
          result = (object) handler.AccessRadioKey_IsOK();
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
            AddressRange addressRange_radio_transmit = handler.myMeters.WorkMeter.meterMemory.GetParameterAddressRange(THL_Params.radio_transmit.ToString());
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
                textBoxResult5.Text = textBoxResult5.Text + "current device time day " + currentTime.Day.ToString() + "." + currentTime.Month.ToString() + ", time " + currentTime.Hour.ToString() + ":" + currentTime.Minute.ToString() + ":" + currentTime.Second.ToString() + Environment.NewLine;
                if (day == (byte) 0)
                  day = Convert.ToByte(currentTime.Day);
                DateTime timepoint2 = new DateTime(currentTime.Year, currentTime.Month, (int) day, (int) hour, (int) minutes, (int) seconds);
                DateTime timeToSendTest = timepoint2;
                timeToSendTest.AddSeconds(8.0);
                byte transmission_scenario = handler.myMeters.WorkMeter.meterMemory.GetData(THL_Params.cfg_communication_scenario)[0];
                int SCENARIO3 = 203;
                int SCENARIO4 = 204;
                if ((int) transmission_scenario == SCENARIO3 || (int) transmission_scenario == SCENARIO4)
                {
                  if (currentTime.Hour != timeToSendTest.Hour || currentTime.Hour == timeToSendTest.Hour && currentTime > timeToSendTest)
                  {
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
                      ++packets_to_send;
                    System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
                    textBoxResult7.Text = textBoxResult7.Text + "set time to send" + Environment.NewLine;
                    not_ready = false;
                    timepoint2 = timepoint2.AddSeconds(-5.0);
                    System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
                    textBoxResult8.Text = textBoxResult8.Text + "set system time = " + timepoint2.ToString("s") + Environment.NewLine;
                    await handler.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(timepoint2, (sbyte) 1), this.progress, this.tokenSource.Token);
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
                else if (currentTime > timeToSendTest)
                {
                  if (currentTime.Day != timepoint2.Day || currentTime > timepoint2)
                  {
                    if (timepointOld <= timepoint2 && timepointOld.Day == timepoint2.Day)
                    {
                      if (data == (byte) 1)
                        ++packets_to_send;
                      System.Windows.Controls.TextBox textBoxResult10 = this.TextBoxResult;
                      textBoxResult10.Text = textBoxResult10.Text + "LoRa send timing is missed -> do not set midnight but wait to get the device time to end sending" + Environment.NewLine;
                      await Task.Delay(4000);
                      timepoint2 = timepoint2.AddSeconds(-5.0);
                      System.Windows.Controls.TextBox textBoxResult11 = this.TextBoxResult;
                      textBoxResult11.Text = textBoxResult11.Text + "set system time = " + timepoint2.ToString("s") + Environment.NewLine;
                      System.Windows.Controls.TextBox textBoxResult12 = this.TextBoxResult;
                      textBoxResult12.Text = textBoxResult12.Text + "packet Nr " + packets_to_send.ToString() + Environment.NewLine;
                      timepointOld = timepoint2;
                    }
                    else
                    {
                      timepointOld = timepoint2;
                      System.Windows.Controls.TextBox textBoxResult13 = this.TextBoxResult;
                      textBoxResult13.Text = textBoxResult13.Text + "set midnight" + Environment.NewLine;
                      timepoint2 = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 23, 59, 59);
                      currentTime = currentTime.AddDays(1.0);
                      timepoint2 = timepoint2.AddSeconds(-5.0);
                      System.Windows.Controls.TextBox textBoxResult14 = this.TextBoxResult;
                      textBoxResult14.Text = textBoxResult14.Text + "set system time = " + timepoint2.ToString("s") + Environment.NewLine;
                      await handler.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(timepoint2, (sbyte) 1), this.progress, this.tokenSource.Token);
                      this.TextBoxResult.CaretIndex = this.TextBoxResult.Text.Length;
                      Rect rect = this.TextBoxResult.GetRectFromCharacterIndex(this.TextBoxResult.CaretIndex);
                      this.TextBoxResult.ScrollToHorizontalOffset(rect.Right);
                      rect = new Rect();
                    }
                  }
                  else
                  {
                    timepointOld = timepoint2;
                    if (data == (byte) 1)
                      ++packets_to_send;
                    System.Windows.Controls.TextBox textBoxResult15 = this.TextBoxResult;
                    textBoxResult15.Text = textBoxResult15.Text + "set time to send" + Environment.NewLine;
                    not_ready = false;
                    timepoint2 = timepoint2.AddSeconds(-10.0);
                    System.Windows.Controls.TextBox textBoxResult16 = this.TextBoxResult;
                    textBoxResult16.Text = textBoxResult16.Text + "set system time = " + timepoint2.ToString("s") + Environment.NewLine;
                    await handler.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(timepoint2, (sbyte) 1), this.progress, this.tokenSource.Token);
                    this.TextBoxResult.CaretIndex = this.TextBoxResult.Text.Length;
                    Rect rect = this.TextBoxResult.GetRectFromCharacterIndex(this.TextBoxResult.CaretIndex);
                    this.TextBoxResult.ScrollToHorizontalOffset(rect.Right);
                    System.Windows.Controls.TextBox textBoxResult17 = this.TextBoxResult;
                    textBoxResult17.Text = textBoxResult17.Text + "packet Nr " + packets_to_send.ToString() + Environment.NewLine;
                    rect = new Rect();
                  }
                }
                else
                {
                  System.Windows.Controls.TextBox textBoxResult18 = this.TextBoxResult;
                  textBoxResult18.Text = textBoxResult18.Text + "set time to send" + Environment.NewLine;
                  not_ready = false;
                  timepoint2 = timepoint2.AddSeconds(-2.0);
                  System.Windows.Controls.TextBox textBoxResult19 = this.TextBoxResult;
                  textBoxResult19.Text = textBoxResult19.Text + "set system time = " + timepoint2.ToString("s") + Environment.NewLine;
                  await handler.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(timepoint2, (sbyte) 1), this.progress, this.tokenSource.Token);
                  this.TextBoxResult.CaretIndex = this.TextBoxResult.Text.Length;
                  Rect rect = this.TextBoxResult.GetRectFromCharacterIndex(this.TextBoxResult.CaretIndex);
                  this.TextBoxResult.ScrollToHorizontalOffset(rect.Right);
                  await Task.Delay(4000);
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
            byte SP9_send_day = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(THL_Params.SP9_send_day);
            this.TextBoxResult.Text = "SP9_send_day = " + SP9_send_day.ToString() + Environment.NewLine;
            byte[] lora_start_date = handler.myMeters.WorkMeter.meterMemory.GetData(THL_Params.lora_start_date);
            DateTime? startDate1 = Util.ConvertToDate_MBus_CP16_TypeG(lora_start_date, 0);
            DateTime? startDate2 = Util.ConvertToDate_MBus_CP16_TypeG(lora_start_date, 2);
            System.Windows.Controls.TextBox textBoxResult20 = this.TextBoxResult;
            textBoxResult20.Text = textBoxResult20.Text + "lora_start_date 1 = " + startDate1.ToString() + Environment.NewLine;
            System.Windows.Controls.TextBox textBoxResult21 = this.TextBoxResult;
            textBoxResult21.Text = textBoxResult21.Text + "lora_start_date 2 = " + startDate2.ToString() + Environment.NewLine;
            DateTime timepoint = new DateTime(2018, startDate1.Value.Month, (int) SP9_send_day, 23, 59, 55);
            timepoint = timepoint.AddMonths(1);
            timepoint = timepoint.AddDays(-1.0);
            System.Windows.Controls.TextBox textBoxResult22 = this.TextBoxResult;
            textBoxResult22.Text = textBoxResult22.Text + "set system time = " + timepoint.ToString("s") + Environment.NewLine;
            await handler.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(timepoint, (sbyte) 1), this.progress, this.tokenSource.Token);
            await Task.Delay(14000);
            int num2 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
            result = (object) num2;
            byte[] time_to_send_data = handler.myMeters.WorkMeter.meterMemory.GetData(THL_Params.time_to_send_data);
            System.Windows.Controls.TextBox textBoxResult23 = this.TextBoxResult;
            textBoxResult23.Text = textBoxResult23.Text + "time_to_send_data = " + Util.ByteArrayToHexString(time_to_send_data) + Environment.NewLine;
            System.Windows.Controls.TextBox textBoxResult24 = this.TextBoxResult;
            textBoxResult24.Text = textBoxResult24.Text + "time_to_send_data.day = " + time_to_send_data[0].ToString() + Environment.NewLine;
            System.Windows.Controls.TextBox textBoxResult25 = this.TextBoxResult;
            textBoxResult25.Text = textBoxResult25.Text + "time_to_send_data.hour = " + time_to_send_data[1].ToString() + Environment.NewLine;
            System.Windows.Controls.TextBox textBoxResult26 = this.TextBoxResult;
            textBoxResult26.Text = textBoxResult26.Text + "time_to_send_data.minutes = " + time_to_send_data[2].ToString() + Environment.NewLine;
            System.Windows.Controls.TextBox textBoxResult27 = this.TextBoxResult;
            textBoxResult27.Text = textBoxResult27.Text + "time_to_send_data.seconds = " + time_to_send_data[3].ToString() + Environment.NewLine;
            timepoint = timepoint.AddSeconds(5.0);
            DateTime timepoint2 = new DateTime(timepoint.Year, timepoint.Month, (int) time_to_send_data[0], (int) time_to_send_data[1], (int) time_to_send_data[2], (int) time_to_send_data[3]);
            timepoint2 = timepoint2.AddSeconds(-4.0);
            System.Windows.Controls.TextBox textBoxResult28 = this.TextBoxResult;
            textBoxResult28.Text = textBoxResult28.Text + "set system time = " + timepoint2.ToString("s") + Environment.NewLine;
            await handler.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(timepoint2, (sbyte) 1), this.progress, this.tokenSource.Token);
            return;
          }
          if (sender == this.ButtonTransmitModulatedCarrier)
            await handler.TransmitModulatedCarrierAsync((ushort) 5, this.progress, this.tokenSource.Token);
          else if (sender == this.ButtonTransmitUnmodulatedCarrier)
            await handler.TransmitUnmodulatedCarrierAsync((ushort) 5, this.progress, this.tokenSource.Token);
          else if (sender == this.ButtonSetFrequencyIncrement)
            await handler.SetFrequencyIncrementAsync(this.progress, this.tokenSource.Token, -55);
          else if (sender == this.ButtonGetMbus_interval)
          {
            ushort Mbus_interval = handler.GetMbus_interval();
            ushort FD_Mbus_interval = handler.GetFD_Mbus_interval();
            byte Mbus_nighttime_start = handler.GetMbus_nighttime_start();
            byte Mbus_nighttime_stop = handler.GetMbus_nighttime_stop();
            byte Mbus_radio_suppression_days = handler.GetMbus_radio_suppression_days();
            string str = "Mbus_interval: " + Mbus_interval.ToString() + Environment.NewLine;
            str = str + "FD_Mbus_interval: " + FD_Mbus_interval.ToString() + Environment.NewLine;
            handler.SetFD_Mbus_interval(Mbus_interval);
            FD_Mbus_interval = handler.GetFD_Mbus_interval();
            str = str + "after set FD_Mbus_interval: " + FD_Mbus_interval.ToString() + Environment.NewLine;
            result = (object) str;
            str = (string) null;
          }
          else if (sender == this.ButtonSendTestPacket)
          {
            byte[] arbitraryData = new byte[28];
            for (int i = 0; i < arbitraryData.Length; ++i)
              arbitraryData[i] = (byte) 85;
            await handler.SendTestPacketAsync(this.progress, this.tokenSource.Token, (ushort) 1, (ushort) 5, 11111111U, arbitraryData, "0FF0");
            arbitraryData = (byte[]) null;
          }
          else if (sender == this.ButtonSwitchState)
          {
            int num = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
            result = (object) num;
            SwitchState state = handler.Get_mount_switch_state();
            result = (object) ("Switch state: " + state.ToString());
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
          else if (sender == this.ButtonSetLcdTestState)
            await handler.SetLcdTestStateAsync(this.progress, this.tokenSource.Token, LcdTest.SegmentFillandClear);
          else if (sender == this.ButtonSetDeviceIdentification)
          {
            DeviceIdentification di = handler.GetDeviceIdentification();
            di.MeterID = new uint?(100000000U);
            di.HardwareTypeID = new uint?(317U);
            di.MeterInfoID = new uint?(2U);
            di.MeterTypeID = new uint?(3U);
            di.BaseTypeID = new uint?(4U);
            di.SAP_MaterialNumber = new uint?(5U);
            di.SAP_ProductionOrderNumber = "6";
            di.PrintedSerialNumberAsString = "Hallo World!";
            di = (DeviceIdentification) null;
          }
          else if (sender == this.ButtonReadSystemTime)
          {
            Common32BitCommands.SystemTime systemTime = await handler.ReadSystemTimeAsync(this.progress, this.tokenSource.Token);
            result = (object) systemTime;
            systemTime = (Common32BitCommands.SystemTime) null;
          }
          else if (sender == this.ButtonSetMode)
            await handler.SetModeAsync(this.progress, this.tokenSource.Token, (Enum) Mode.OperationMode);
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
          else
            result = (object) "Not supported button";
        }
        if (result != null)
          this.TextBoxResult.Text = !(result is byte[]) ? result.ToString() : Utility.ByteArrayToHexString((byte[]) result);
        result = (object) null;
        handler = (THL_HandlerFunctions) null;
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

    internal async Task<bool> RunRadioPacketTest(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.TextBoxResult.Text = string.Empty;
      Stopwatch watch = Stopwatch.StartNew();
      THL_HandlerFunctions handler = this.windowFunctions.MyFunctions;
      RadioMode mode = RadioMode.Radio3;
      uint serialnumber = 11111111;
      byte[] arbitraryData = new byte[28];
      TestWindows.logger.Info("TEST: TH = > MiCon");
      ConnectionProfile profile = ReadoutConfigFunctions.Manager.GetConnectionProfile(32);
      profile.EquipmentModel.ChangeableParameters.Find((Predicate<ChangeableParameter>) (x => x.Key == "Port")).Value = "COM25";
      CommunicationPortFunctions port = new CommunicationPortFunctions();
      port.SetReadoutConfiguration(profile.GetSettingsList());
      System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
      textBoxResult1.Text = textBoxResult1.Text + "Open MiCon " + profile.EquipmentModel.ChangeableParameters.Find((Predicate<ChangeableParameter>) (x => x.Key == "Port")).Value + Environment.NewLine + Environment.NewLine;
      port.Open();
      await Task.Delay(500);
      CommunicationByMinoConnect micon = port.GetCommunicationByMinoConnect();
      try
      {
        TestWindows.logger.Info("TEST: MiCon => TH");
        System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
        textBoxResult2.Text = textBoxResult2.Text + Environment.NewLine + "Start TH receiver (" + (ushort) 10.ToString() + " sec)" + Environment.NewLine;
        Task<double?> resultReceiver2 = handler.ReceiveTestPacketAsync(progress, cancelToken, (byte) 10, serialnumber, "D696");
        System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
        textBoxResult3.Text = textBoxResult3.Text + "Send test packet via MiCon" + Environment.NewLine;
        int countOfSendPackets = 0;
        await Task.Run((Action) (() =>
        {
          while (!resultReceiver2.IsCompleted)
          {
            try
            {
              micon.SendTestPacket(serialnumber, RadioMode.Radio3, (byte) 7, "D696");
              ++countOfSendPackets;
              Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
              TestWindows.logger.Error(ex.Message);
            }
          }
        }));
        System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
        textBoxResult4.Text = textBoxResult4.Text + countOfSendPackets.ToString() + " packets are sends form MiCon" + Environment.NewLine;
        try
        {
          if (resultReceiver2.Result.HasValue)
          {
            TestWindows.logger.Info("Successfully send/receive a TEST packet from MinoConnect to THL");
            System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
            textBoxResult5.Text = textBoxResult5.Text + "MiCon = > TH OK RSSI: " + resultReceiver2.Result.ToString() + Environment.NewLine;
          }
          else
          {
            TestWindows.logger.Error("Failed send/receive a TEST packet from MinoConnect to THL");
            System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
            textBoxResult6.Text = textBoxResult6.Text + "MiCon = > TH Error" + Environment.NewLine;
          }
        }
        catch (Exception ex)
        {
          TestWindows.logger.Error("Failed send/receive a TEST packet from MinoConnect to THL");
          System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
          textBoxResult7.Text = textBoxResult7.Text + "MiCon = > TH Error: " + ex.Message + Environment.NewLine;
        }
        await Task.Delay(500);
        bool done = false;
        RadioTestResult resultReceiver = (RadioTestResult) null;
        System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
        textBoxResult8.Text = textBoxResult8.Text + "Start MiCon receiver (" + (ushort) 10.ToString() + " sec)" + Environment.NewLine;
        Task taskReceiver = Task.Run((Action) (() =>
        {
          resultReceiver = micon.ReceiveOnePacket(mode, (int) serialnumber, (ushort) 10, "D696");
          done = true;
        }));
        System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
        textBoxResult9.Text = textBoxResult9.Text + "Send test packet via TH" + Environment.NewLine;
        while (!done)
        {
          await Task.Delay(500);
          try
          {
            await handler.SendTestPacketAsync(progress, cancelToken, (ushort) 1, (ushort) 1, serialnumber, arbitraryData, "D696");
          }
          catch (Exception ex)
          {
            TestWindows.logger.Error(ex.Message);
          }
        }
        if (resultReceiver != null)
        {
          TestWindows.logger.Info("Successfully send/receive a TEST packet from THL to MinoConnect. RSSI: " + resultReceiver.RSSI.ToString() + " dBm");
          System.Windows.Controls.TextBox textBoxResult10 = this.TextBoxResult;
          textBoxResult10.Text = textBoxResult10.Text + "TH = > MiCon OK RSSI:" + resultReceiver.RSSI.ToString() + Environment.NewLine;
        }
        else
        {
          TestWindows.logger.Error("Failed send/receive a TEST packet from THL to MinoConnect");
          System.Windows.Controls.TextBox textBoxResult11 = this.TextBoxResult;
          textBoxResult11.Text = textBoxResult11.Text + "TH = > MiCon Error" + Environment.NewLine;
        }
        taskReceiver = (Task) null;
      }
      finally
      {
        System.Windows.Controls.TextBox textBoxResult12 = this.TextBoxResult;
        textBoxResult12.Text = textBoxResult12.Text + Environment.NewLine + "Close MiCon " + profile.EquipmentModel.ChangeableParameters.Find((Predicate<ChangeableParameter>) (x => x.Key == "Port")).Value + Environment.NewLine;
        port.Close();
      }
      watch.Stop();
      System.Windows.Controls.TextBox textBoxResult13 = this.TextBoxResult;
      textBoxResult13.Text = textBoxResult13.Text + "Elapsed: " + Util.ElapsedToString(watch.Elapsed) + Environment.NewLine;
      bool flag = true;
      watch = (Stopwatch) null;
      handler = (THL_HandlerFunctions) null;
      arbitraryData = (byte[]) null;
      profile = (ConnectionProfile) null;
      port = (CommunicationPortFunctions) null;
      return flag;
    }

    private async void ButtonRadioTest_Click(object sender, RoutedEventArgs e)
    {
      this.tokenSource = new CancellationTokenSource();
      try
      {
        int num = await this.RunRadioPacketTest(this.progress, this.tokenSource.Token) ? 1 : 0;
      }
      catch (Exception ex)
      {
        TestWindows.logger.Error(ex.Message);
      }
    }

    private void Window_Closing(object sender, CancelEventArgs e) => this.exitSendNext = true;

    private async void ButtonReadLogger_Click(object sender, RoutedEventArgs e)
    {
      this.tokenSource = new CancellationTokenSource();
      try
      {
        THL_HandlerFunctions handler = this.windowFunctions.MyFunctions;
        FirmwareVersion firmwareVersion = await handler.ReadVersionAsync(this.progress, this.tokenSource.Token);
        FirmwareVersion ver = firmwareVersion;
        firmwareVersion = new FirmwareVersion();
        THL_DeviceMemory mem = new THL_DeviceMemory(ver.Version);
        AddressRange prm = mem.GetParameterAddressRange("th_reading");
        byte[] buffer = await handler.cmd.Device.ReadMemoryAsync(this.progress, this.tokenSource.Token, prm, (byte) 232);
        Reading logger = Reading.Parse(buffer, 0);
        StringBuilder sb = new StringBuilder();
        this.TextBoxResult.Text = string.Empty;
        for (int i = 0; i < logger.measurement_samples.Length; ++i)
          this.AddItem(sb, "3 minutes[" + i.ToString() + "]   ", logger.measurement_samples[i].ToString());
        sb.AppendLine();
        for (int i = 0; i < logger.daily_samples.Length; ++i)
        {
          if (logger.daily_samples[i].SHT2x_RH_Valid || logger.daily_samples[i].SHT2x_T_Valid)
            this.AddItem(sb, "15 minutes[" + i.ToString() + "]   ", logger.daily_samples[i].ToString());
        }
        sb.AppendLine();
        this.AddItem(sb, "daily_average", logger.daily_average.ToString());
        this.AddItem(sb, "daily_sample_count", logger.daily_sample_count.ToString());
        this.AddItem(sb, "quarter_hour_sample_count", logger.quarter_hour_sample_count.ToString());
        this.AddItem(sb, "measurement_countdown", logger.measurement_countdown.ToString());
        sb.AppendLine();
        this.AddItem(sb, "dailyBucket.count", logger.dailyBucket.count.ToString());
        this.AddItem(sb, "dailyBucket.temperatureDistribution    ", logger.dailyBucket.temperatureDistribution.ToString());
        this.AddItem(sb, "dailyBucket.humidityDistribution       ", logger.dailyBucket.humidityDistribution.ToString());
        sb.AppendLine();
        this.AddItem(sb, "current", logger.current.ToString());
        sb.AppendLine();
        for (int i = 0; i < logger.logger.daily.Length; ++i)
          this.AddItem(sb, "daily[" + i.ToString() + "]", logger.logger.daily[i].ToString());
        for (int i = 0; i < logger.logger.monthly.Length; ++i)
          this.AddItem(sb, "monthly[" + i.ToString() + "]", logger.logger.monthly[i].ToString());
        for (int i = 0; i < logger.logger.half_monthly.Length; ++i)
          this.AddItem(sb, "h monthly[" + i.ToString() + "]", logger.logger.half_monthly[i].ToString());
        this.AddItem(sb, "last daily", logger.logger.last_daily.ToString());
        this.AddItem(sb, "last monthly", logger.logger.last_monthly.ToString());
        this.AddItem(sb, "last h monthly", logger.logger.last_half_monthly.ToString());
        this.AddItem(sb, "crc", logger.logger.crc.ToString());
        this.TextBoxResult.Text += sb.ToString();
        handler = (THL_HandlerFunctions) null;
        ver = new FirmwareVersion();
        mem = (THL_DeviceMemory) null;
        prm = (AddressRange) null;
        buffer = (byte[]) null;
        logger = (Reading) null;
        sb = (StringBuilder) null;
      }
      catch (Exception ex)
      {
        System.Windows.Controls.TextBox textBoxResult = this.TextBoxResult;
        textBoxResult.Text = textBoxResult.Text + ex.Message + Environment.NewLine;
      }
    }

    private void AddItem(StringBuilder sb, string key, string value)
    {
      sb.Append(key).Append(" ").AppendLine(value);
    }

    private async void ButtonReadTemperature_Click(object sender, RoutedEventArgs e)
    {
      this.tokenSource = new CancellationTokenSource();
      try
      {
        double temperature = await this.windowFunctions.MyFunctions.ReadTemperatureAsync(this.progress, this.tokenSource.Token);
        this.TextBoxResult.Text = temperature.ToString() + " °C";
      }
      catch (Exception ex)
      {
        this.TextBoxResult.Text = ex.Message + Environment.NewLine;
      }
    }

    private async void ButtonReadHumidity_Click(object sender, RoutedEventArgs e)
    {
      this.tokenSource = new CancellationTokenSource();
      try
      {
        double humidity = await this.windowFunctions.MyFunctions.ReadHumidityAsync(this.progress, this.tokenSource.Token);
        this.TextBoxResult.Text = humidity.ToString() + " %";
      }
      catch (Exception ex)
      {
        this.TextBoxResult.Text = ex.Message + Environment.NewLine;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      System.Windows.Application.LoadComponent((object) this, new Uri("/THL_Handler;component/ui/testwindows.xaml", UriKind.Relative));
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
          this.ButtonReadTemperature = (System.Windows.Controls.Button) target;
          this.ButtonReadTemperature.Click += new RoutedEventHandler(this.ButtonReadTemperature_Click);
          break;
        case 7:
          this.ButtonReadHumidity = (System.Windows.Controls.Button) target;
          this.ButtonReadHumidity.Click += new RoutedEventHandler(this.ButtonReadHumidity_Click);
          break;
        case 8:
          this.ButtonReadLogger = (System.Windows.Controls.Button) target;
          this.ButtonReadLogger.Click += new RoutedEventHandler(this.ButtonReadLogger_Click);
          break;
        case 9:
          this.ButtonRadioTest = (System.Windows.Controls.Button) target;
          this.ButtonRadioTest.Click += new RoutedEventHandler(this.ButtonRadioTest_Click);
          break;
        case 10:
          this.ButtonSendNextLoRa = (System.Windows.Controls.Button) target;
          this.ButtonSendNextLoRa.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 11:
          this.ButtonSendSP9T1 = (System.Windows.Controls.Button) target;
          this.ButtonSendSP9T1.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 12:
          this.ButtonReadWrite_SerialNumberFull_DevEUI_JoinEUI_DevKey = (System.Windows.Controls.Button) target;
          this.ButtonReadWrite_SerialNumberFull_DevEUI_JoinEUI_DevKey.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 13:
          this.ButtonGetConfigurationParameter = (System.Windows.Controls.Button) target;
          this.ButtonGetConfigurationParameter.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 14:
          this.ButtonSetMode = (System.Windows.Controls.Button) target;
          this.ButtonSetMode.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 15:
          this.ButtonSwitchState = (System.Windows.Controls.Button) target;
          this.ButtonSwitchState.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 16:
          this.ButtonHardwareTypeManager = (System.Windows.Controls.Button) target;
          this.ButtonHardwareTypeManager.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 17:
          this.ButtonAddNewMap = (System.Windows.Controls.Button) target;
          this.ButtonAddNewMap.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 18:
          this.ButtonProtection = (System.Windows.Controls.Button) target;
          this.ButtonProtection.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 19:
          this.ButtonReadVersion = (System.Windows.Controls.Button) target;
          this.ButtonReadVersion.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 20:
          this.ButtonGetRadioVersion = (System.Windows.Controls.Button) target;
          this.ButtonGetRadioVersion.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 21:
          this.ButtonTransmitModulatedCarrier = (System.Windows.Controls.Button) target;
          this.ButtonTransmitModulatedCarrier.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 22:
          this.ButtonTransmitUnmodulatedCarrier = (System.Windows.Controls.Button) target;
          this.ButtonTransmitUnmodulatedCarrier.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 23:
          this.ButtonSetFrequencyIncrement = (System.Windows.Controls.Button) target;
          this.ButtonSetFrequencyIncrement.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 24:
          this.ButtonSendTestPacket = (System.Windows.Controls.Button) target;
          this.ButtonSendTestPacket.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 25:
          this.ButtonReceiveTestPacket = (System.Windows.Controls.Button) target;
          this.ButtonReceiveTestPacket.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 26:
          this.ButtonReceiveTestPacketViaMiCon = (System.Windows.Controls.Button) target;
          this.ButtonReceiveTestPacketViaMiCon.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 27:
          this.ButtonSendTestPacketViaMiCon = (System.Windows.Controls.Button) target;
          this.ButtonSendTestPacketViaMiCon.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 28:
          this.ButtonSetLcdTestState = (System.Windows.Controls.Button) target;
          this.ButtonSetLcdTestState.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 29:
          this.ButtonSetDeviceIdentification = (System.Windows.Controls.Button) target;
          this.ButtonSetDeviceIdentification.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 30:
          this.ButtonReadSystemTime = (System.Windows.Controls.Button) target;
          this.ButtonReadSystemTime.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 31:
          this.ButtonWriteSystemTime = (System.Windows.Controls.Button) target;
          this.ButtonWriteSystemTime.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 32:
          this.ButtonCompareConnectedAndWork = (System.Windows.Controls.Button) target;
          this.ButtonCompareConnectedAndWork.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 33:
          this.ButtonAccessRadioKey = (System.Windows.Controls.Button) target;
          this.ButtonAccessRadioKey.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 34:
          this.ButtonGetMbus_interval = (System.Windows.Controls.Button) target;
          this.ButtonGetMbus_interval.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 35:
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
