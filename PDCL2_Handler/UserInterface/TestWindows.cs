// Decompiled with JetBrains decompiler
// Type: PDCL2_Handler.UserInterface.TestWindows
// Assembly: PDCL2_Handler, Version=2.22.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 03BA4C2D-69FE-4DA6-9C3F-B3D5471C4058
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDCL2_Handler.dll

using CommunicationPort;
using CommunicationPort.Functions;
using HandlerLib;
using NLog;
using ReadoutConfiguration;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
namespace PDCL2_Handler.UserInterface
{
  public class TestWindows : Window, IComponentConnector
  {
    private static Logger logger = LogManager.GetLogger(nameof (TestWindows));
    private PDCL2_HandlerWindowFunctions windowFunctions;
    private ProgressHandler progress;
    private CancellationTokenSource tokenSource;
    private bool exitSendNext;
    private byte led = 0;
    internal TextBlock TextBlockStatus;
    internal System.Windows.Controls.ProgressBar ProgressBar1;
    internal System.Windows.Controls.TextBox TextBoxResult;
    internal StackPanel StackPanalButtons;
    internal System.Windows.Controls.Button ButtonRadioTest;
    internal System.Windows.Controls.Button ButtonAddNewMap;
    internal System.Windows.Controls.Button ButtonSendNextLoRa;
    internal System.Windows.Controls.Button ButtonSendSP9T1;
    internal System.Windows.Controls.Button ButtonGetConfigurationParameter;
    internal System.Windows.Controls.Button ButtonSetMode;
    internal System.Windows.Controls.Button ButtonHardwareTypeEditor;
    internal System.Windows.Controls.Button ButtonProtection;
    internal System.Windows.Controls.Button ButtonReadVersion;
    internal System.Windows.Controls.Button ButtonGetRadioVersion;
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
    internal System.Windows.Controls.Button ButtonWriteAccessRadioKey;
    internal System.Windows.Controls.Button ButtonSwitchLoRaLED;
    internal System.Windows.Controls.Button ButtonFillLogger;
    internal System.Windows.Controls.Button ButtonBreak;
    private bool _contentLoaded;

    public TestWindows(Window owner, PDCL2_HandlerWindowFunctions windowFunctions)
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

    private async Task<object> fillLogger(object result, PDCL2_HandlerFunctions handler)
    {
      CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
      DateTime currentTime = new DateTime(2019, 1, 1, 0, 0, 0);
      uint counterCH1 = 0;
      uint counterCH2 = 0;
      uint i = 0;
      while (currentTime < DateTime.Now)
      {
        currentTime = currentTime.AddMonths(1);
        currentTime = new DateTime(currentTime.Year, currentTime.Month, 1, 0, 0, 0);
        currentTime = currentTime.AddSeconds(-2.0);
        ++counterCH1;
        counterCH2 += 2U;
        System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
        textBoxResult1.Text = textBoxResult1.Text + "set counter ch1 " + counterCH1.ToString() + Environment.NewLine;
        await handler.cmd.MBusCmd.SetChannelValueAsync((byte) 1, counterCH1, this.progress, cancelTokenSource.Token);
        System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
        textBoxResult2.Text = textBoxResult2.Text + "set counter ch2 " + counterCH2.ToString() + Environment.NewLine;
        await handler.cmd.MBusCmd.SetChannelValueAsync((byte) 2, counterCH2, this.progress, cancelTokenSource.Token);
        await Task.Delay(1000);
        System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
        textBoxResult3.Text = textBoxResult3.Text + "set time  " + currentTime.ToString() + Environment.NewLine;
        await handler.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(currentTime, (sbyte) 1), this.progress, this.tokenSource.Token);
        await Task.Delay(10000);
        currentTime = currentTime.AddMonths(1);
        ++i;
      }
      object obj = result;
      cancelTokenSource = (CancellationTokenSource) null;
      return obj;
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        object result = (object) null;
        PDCL2_HandlerFunctions handler = this.windowFunctions.MyFunctions;
        if (sender == this.ButtonReadVersion)
        {
          FirmwareVersion firmwareVersion = await handler.ReadVersionAsync(this.progress, this.tokenSource.Token);
          result = (object) firmwareVersion;
          firmwareVersion = new FirmwareVersion();
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
            int readingA = 0;
            int readingB = 0;
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
            AddressRange addressRange_time_to_send_data = handler.myMeters.WorkMeter.meterMemory.GetParameterAddressRange(PDCL2_Params.radio_transmit.ToString());
            do
            {
              try
              {
                byte[] time_to_send_data = await handler.cmd.Device.ReadMemoryAsync(this.progress, this.tokenSource.Token, addressRange_time_to_send_data, (byte) 20);
                Common32BitCommands.SystemTime _currentTime = await handler.cmd.Device.GetSystemTimeAsync(this.progress, this.tokenSource.Token);
                DateTime currentTime = new DateTime(2000 + (int) _currentTime.Year, (int) _currentTime.Month, (int) _currentTime.Day, (int) _currentTime.Hour, (int) _currentTime.Min, (int) _currentTime.Sec);
                DateTime localDate = DateTime.Now;
                for (int i = 0; i <= 40 && send_hour == (int) time_to_send_data[1] && send_min == (int) time_to_send_data[2] && send_sec == (int) time_to_send_data[3]; ++i)
                {
                  _currentTime = await handler.cmd.Device.GetSystemTimeAsync(this.progress, this.tokenSource.Token);
                  time_to_send_data = await handler.cmd.Device.ReadMemoryAsync(this.progress, this.tokenSource.Token, addressRange_time_to_send_data, (byte) 10);
                  currentTime = new DateTime(2000 + (int) _currentTime.Year, (int) _currentTime.Month, (int) _currentTime.Day, (int) _currentTime.Hour, (int) _currentTime.Min, (int) _currentTime.Sec);
                  localDate = DateTime.Now;
                }
                _currentTime = await handler.cmd.Device.GetSystemTimeAsync(this.progress, this.tokenSource.Token);
                currentTime = new DateTime(2000 + (int) _currentTime.Year, (int) _currentTime.Month, (int) _currentTime.Day, (int) _currentTime.Hour, (int) _currentTime.Min, (int) _currentTime.Sec);
                localDate = DateTime.Now;
                System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
                textBoxResult1.Text = textBoxResult1.Text + "Time: " + localDate.ToString();
                this.TextBoxResult.Text += Environment.NewLine;
                System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
                textBoxResult2.Text = textBoxResult2.Text + "time_to_send_data = " + Util.ByteArrayToHexString(time_to_send_data) + Environment.NewLine;
                System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
                textBoxResult3.Text = textBoxResult3.Text + "time_to_send_data.day  " + time_to_send_data[6].ToString() + ", time " + time_to_send_data[7].ToString() + ":" + time_to_send_data[8].ToString() + ":" + time_to_send_data[9].ToString() + Environment.NewLine;
                System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
                textBoxResult4.Text = textBoxResult4.Text + "time_to_send_data.packet: " + time_to_send_data[1].ToString() + this.GetPacketString(time_to_send_data[1], time_to_send_data[2]) + Environment.NewLine;
                System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
                System.Windows.Controls.TextBox textBox = textBoxResult5;
                string[] strArray = new string[12];
                strArray[0] = textBoxResult5.Text;
                strArray[1] = "current device time day ";
                int num3 = currentTime.Day;
                strArray[2] = num3.ToString();
                strArray[3] = ".";
                num3 = currentTime.Month;
                strArray[4] = num3.ToString();
                strArray[5] = ", time ";
                num3 = currentTime.Hour;
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
                byte day = time_to_send_data[0];
                if (day == (byte) 0)
                {
                  currentTime = currentTime.AddDays(-1.0);
                  day = Convert.ToByte(currentTime.Day);
                  currentTime = currentTime.AddDays(1.0);
                }
                DateTime timepoint2 = new DateTime(currentTime.Year, currentTime.Month, (int) day, (int) time_to_send_data[7], (int) time_to_send_data[8], (int) time_to_send_data[9]);
                DateTime timeToSendTest = timepoint2;
                timeToSendTest.AddSeconds(8.0);
                byte transmission_scenario = handler.myMeters.WorkMeter.meterMemory.GetData(PDCL2_Params.cfg_transmission_scenario)[0];
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
                      await Task.Delay(4000);
                    rect = new Rect();
                  }
                  else
                  {
                    num3 = packets_to_send++;
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
                      num3 = packets_to_send++;
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
                      ++readingA;
                      readingB += 2;
                      await handler.WriteMeterValueA(readingA, this.progress, this.tokenSource.Token);
                      await handler.WriteMeterValueB(readingB, this.progress, this.tokenSource.Token);
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
                    num3 = packets_to_send++;
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
                send_hour = (int) time_to_send_data[1];
                send_min = (int) time_to_send_data[2];
                send_sec = (int) time_to_send_data[3];
                time_to_send_data = (byte[]) null;
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
            byte SP9_send_day = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(PDCL2_Params.SP9_send_day);
            this.TextBoxResult.Text = "SP9_send_day = " + SP9_send_day.ToString() + Environment.NewLine;
            byte[] lora_start_date = handler.myMeters.WorkMeter.meterMemory.GetData(PDCL2_Params.lora_start_date);
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
            byte[] time_to_send_data = handler.myMeters.WorkMeter.meterMemory.GetData(PDCL2_Params.time_to_send_data);
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
          if (sender == this.ButtonHardwareTypeEditor)
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
            HardwareTypeEditor dlg = new HardwareTypeEditor(new string[4]
            {
              "PDC_LoRa",
              "PDC_LoRa_868MHz_SD",
              "PDC_LoRa_915",
              "UDC_LoRa_915"
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
          else if (sender == this.ButtonRadioTest)
          {
            int num4 = await this.RunRadioPacketTest(this.progress, this.tokenSource.Token) ? 1 : 0;
          }
          else if (sender == this.ButtonAccessRadioKey)
            result = (object) handler.AccessRadioKey_IsOK();
          else if (sender == this.ButtonWriteAccessRadioKey)
          {
            await handler.WriteAccessRadioKeyAsync(this.progress, this.tokenSource.Token);
            result = (object) "ok";
          }
          else if (sender == this.ButtonSwitchLoRaLED)
          {
            this.led = this.led == (byte) 0 ? (byte) 1 : (byte) 0;
            await handler.SwitchLoRaLEDAsync(this.led, this.progress, this.tokenSource.Token);
          }
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
            int num5 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
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
              int num6 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
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
        if (result != null)
          this.TextBoxResult.Text = !(result is byte[]) ? result.ToString() : Utility.ByteArrayToHexString((byte[]) result);
        result = (object) null;
        handler = (PDCL2_HandlerFunctions) null;
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

    private void Window_Closing(object sender, CancelEventArgs e) => this.exitSendNext = true;

    internal async Task<bool> RunRadioPacketTest(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.TextBoxResult.Text = string.Empty;
      Stopwatch watch = Stopwatch.StartNew();
      PDCL2_HandlerFunctions handler = this.windowFunctions.MyFunctions;
      await handler.SetRadioMode((byte) 1, progress, cancelToken);
      RadioMode mode = RadioMode.Radio3;
      uint serialnumber = 11111111;
      byte[] arbitraryData = new byte[28];
      TestWindows.logger.Info("TEST: PDCL = > MiCon");
      ConnectionProfile profile = ReadoutConfigFunctions.Manager.GetConnectionProfile(32);
      profile.EquipmentModel.ChangeableParameters.Find((Predicate<ChangeableParameter>) (x => x.Key == "Port")).Value = "COM6";
      CommunicationPortFunctions port = new CommunicationPortFunctions();
      port.SetReadoutConfiguration(profile.GetSettingsList());
      System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
      textBoxResult1.Text = textBoxResult1.Text + "Open MiCon " + profile.EquipmentModel.ChangeableParameters.Find((Predicate<ChangeableParameter>) (x => x.Key == "Port")).Value + Environment.NewLine + Environment.NewLine;
      port.Open();
      await Task.Delay(500);
      CommunicationByMinoConnect micon = port.GetCommunicationByMinoConnect();
      try
      {
        TestWindows.logger.Info("TEST: MiCon => PDCL");
        System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
        textBoxResult2.Text = textBoxResult2.Text + Environment.NewLine + "Start PDCL receiver (" + (ushort) 10.ToString() + " sec)" + Environment.NewLine;
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
              micon.SendTestPacket(serialnumber, mode, (byte) 7, "D696");
              ++countOfSendPackets;
              Thread.Sleep(1500);
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
          double? result = resultReceiver2.Result;
          if (result.HasValue)
          {
            TestWindows.logger.Info("Successfully send/receive a TEST packet from MinoConnect to PDCL");
            System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
            string text = textBoxResult5.Text;
            result = resultReceiver2.Result;
            string str = result.ToString();
            string newLine = Environment.NewLine;
            textBoxResult5.Text = text + "MiCon = > PDCL OK RSSI: " + str + newLine;
          }
          else
          {
            TestWindows.logger.Error("Failed send/receive a TEST packet from MinoConnect to PDCL");
            System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
            textBoxResult6.Text = textBoxResult6.Text + "MiCon = > PDCL Error" + Environment.NewLine;
          }
        }
        catch (Exception ex)
        {
          TestWindows.logger.Error("Failed send/receive a TEST packet from MinoConnect to PDCL");
          System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
          textBoxResult7.Text = textBoxResult7.Text + "MiCon = > PDCL Error: " + ex.Message + Environment.NewLine;
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
        textBoxResult9.Text = textBoxResult9.Text + "Send test packet via PDCL" + Environment.NewLine;
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
          Logger logger = TestWindows.logger;
          int rssi = resultReceiver.RSSI;
          string message = "Successfully send/receive a TEST packet from PDCL to MinoConnect. RSSI: " + rssi.ToString() + " dBm";
          logger.Info(message);
          System.Windows.Controls.TextBox textBoxResult10 = this.TextBoxResult;
          string text = textBoxResult10.Text;
          rssi = resultReceiver.RSSI;
          string str = rssi.ToString();
          string newLine = Environment.NewLine;
          textBoxResult10.Text = text + "PDCL = > MiCon OK RSSI:" + str + newLine;
        }
        else
        {
          TestWindows.logger.Error("Failed send/receive a TEST packet from PDCL to MinoConnect");
          System.Windows.Controls.TextBox textBoxResult11 = this.TextBoxResult;
          textBoxResult11.Text = textBoxResult11.Text + "PDCL = > MiCon Error" + Environment.NewLine;
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
      handler = (PDCL2_HandlerFunctions) null;
      arbitraryData = (byte[]) null;
      profile = (ConnectionProfile) null;
      port = (CommunicationPortFunctions) null;
      return flag;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      System.Windows.Application.LoadComponent((object) this, new Uri("/PDCL2_Handler;component/ui/testwindows.xaml", UriKind.Relative));
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
          this.ButtonRadioTest = (System.Windows.Controls.Button) target;
          this.ButtonRadioTest.Click += new RoutedEventHandler(this.Button_Click);
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
          this.ButtonGetConfigurationParameter = (System.Windows.Controls.Button) target;
          this.ButtonGetConfigurationParameter.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 11:
          this.ButtonSetMode = (System.Windows.Controls.Button) target;
          this.ButtonSetMode.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 12:
          this.ButtonHardwareTypeEditor = (System.Windows.Controls.Button) target;
          this.ButtonHardwareTypeEditor.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 13:
          this.ButtonProtection = (System.Windows.Controls.Button) target;
          this.ButtonProtection.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 14:
          this.ButtonReadVersion = (System.Windows.Controls.Button) target;
          this.ButtonReadVersion.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 15:
          this.ButtonGetRadioVersion = (System.Windows.Controls.Button) target;
          this.ButtonGetRadioVersion.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 16:
          this.ButtonTransmitModulatedCarrier = (System.Windows.Controls.Button) target;
          this.ButtonTransmitModulatedCarrier.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 17:
          this.ButtonTransmitUnmodulatedCarrier = (System.Windows.Controls.Button) target;
          this.ButtonTransmitUnmodulatedCarrier.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 18:
          this.ButtonSetFrequencyIncrement = (System.Windows.Controls.Button) target;
          this.ButtonSetFrequencyIncrement.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 19:
          this.ButtonSendTestPacket = (System.Windows.Controls.Button) target;
          this.ButtonSendTestPacket.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 20:
          this.ButtonReceiveTestPacket = (System.Windows.Controls.Button) target;
          this.ButtonReceiveTestPacket.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 21:
          this.ButtonSendTestPacketViaMiCon = (System.Windows.Controls.Button) target;
          this.ButtonSendTestPacketViaMiCon.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 22:
          this.ButtonReceiveTestPacketViaMiCon = (System.Windows.Controls.Button) target;
          this.ButtonReceiveTestPacketViaMiCon.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 23:
          this.ButtonSetDeviceIdentification = (System.Windows.Controls.Button) target;
          this.ButtonSetDeviceIdentification.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 24:
          this.ButtonReadSystemTime = (System.Windows.Controls.Button) target;
          this.ButtonReadSystemTime.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 25:
          this.ButtonWriteSystemTime = (System.Windows.Controls.Button) target;
          this.ButtonWriteSystemTime.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 26:
          this.ButtonDisableIrDa = (System.Windows.Controls.Button) target;
          this.ButtonDisableIrDa.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 27:
          this.ButtonCompareConnectedAndWork = (System.Windows.Controls.Button) target;
          this.ButtonCompareConnectedAndWork.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 28:
          this.ButtonShowPublicKey = (System.Windows.Controls.Button) target;
          this.ButtonShowPublicKey.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 29:
          this.ButtonAccessRadioKey = (System.Windows.Controls.Button) target;
          this.ButtonAccessRadioKey.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 30:
          this.ButtonWriteAccessRadioKey = (System.Windows.Controls.Button) target;
          this.ButtonWriteAccessRadioKey.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 31:
          this.ButtonSwitchLoRaLED = (System.Windows.Controls.Button) target;
          this.ButtonSwitchLoRaLED.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 32:
          this.ButtonFillLogger = (System.Windows.Controls.Button) target;
          this.ButtonFillLogger.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 33:
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
