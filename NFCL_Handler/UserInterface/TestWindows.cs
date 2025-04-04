// Decompiled with JetBrains decompiler
// Type: NFCL_Handler.UserInterface.TestWindows
// Assembly: NFCL_Handler, Version=2.3.2.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 887E21A2-7448-48CC-AF3E-C39E4C7B3AFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NFCL_Handler.dll

using CommunicationPort;
using CommunicationPort.Functions;
using GmmDbLib;
using HandlerLib;
using HandlerLib.NFC;
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
using ZENNER.CommonLibrary.Exceptions;
using ZR_ClassLibrary;

#nullable disable
namespace NFCL_Handler.UserInterface
{
  public class TestWindows : Window, IComponentConnector
  {
    private static Logger logger = LogManager.GetLogger(nameof (TestWindows));
    private NFCL_HandlerWindowFunctions windowFunctions;
    private ProgressHandler progress;
    private CancellationTokenSource tokenSource;
    private bool exitSendNext;
    private byte[] ConfigChangedCounterAdr = new byte[4]
    {
      (byte) 32,
      (byte) 0,
      (byte) 29,
      (byte) 121
    };
    private byte[] CurrentValueAdr = new byte[4]
    {
      (byte) 32,
      (byte) 0,
      (byte) 0,
      (byte) 16
    };
    private const byte Data_Send = 0;
    private const byte Data_not_Send = 1;
    private const byte Data_later_send = 2;
    private const byte READ_FAULT = 0;
    private const byte READ_SUCCESS = 1;
    private const byte NOT_NEED_TO_READ = 2;
    private const byte READY_TO_READ = 3;
    private const byte NDC_CONNECTED = 5;
    private const byte NDC_ERROR_HANDLING = 6;
    private const byte NDC_MODULE_CONNECTION_LOST = 1;
    private byte repetition_counter;
    private byte fault_counter;
    private ushort CommunicationScenario;
    private byte MeterState;
    private byte value_reading_status;
    private TestWindows.cradio_transmit radio_transmit = new TestWindows.cradio_transmit();
    private byte con_work_state;
    private byte prepare_values_state;
    private bool time_read_parameter;
    internal TextBlock TextBlockStatus;
    internal System.Windows.Controls.ProgressBar ProgressBar1;
    internal System.Windows.Controls.TextBox TextBoxResult;
    internal StackPanel StackPanalButtons;
    internal System.Windows.Controls.Button ButtonReadMBusState;
    internal System.Windows.Controls.Button ButtonReadIUW;
    internal System.Windows.Controls.Button ButtonRadioTest;
    internal System.Windows.Controls.Button ButtonSendNextLoRa;
    internal System.Windows.Controls.Button ButtonSendSP9T1;
    internal System.Windows.Controls.Button ButtonReadWrite_SerialNumberFull_DevEUI_JoinEUI_DevKey;
    internal System.Windows.Controls.Button ButtonGetConfigurationParameter;
    internal System.Windows.Controls.Button ButtonSetMode;
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
    internal System.Windows.Controls.Button ButtonSetDeviceIdentification;
    internal System.Windows.Controls.Button ButtonReadSystemTime;
    internal System.Windows.Controls.Button ButtonWriteSystemTime;
    internal System.Windows.Controls.Button ButtonCompareConnectedAndWork;
    internal System.Windows.Controls.Button ButtonDisableIrDa;
    internal System.Windows.Controls.Button ButtonAccessRadioKey;
    internal System.Windows.Controls.Button ButtonGetMbus_interval;
    internal System.Windows.Controls.Button ButtonBreak;
    private bool _contentLoaded;

    public TestWindows(Window owner, NFCL_HandlerWindowFunctions windowFunctions)
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

    private string GetPacketString(byte packet)
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
          packetString = " SP9." + this.windowFunctions.MyFunctions.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(NFCL_Params.sp9_subtype).ToString();
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

    private ushort CRC16_Calculate(ushort initialValue, byte[] bytes, ushort size)
    {
      ushort[] numArray = new ushort[256];
      ushort num1 = initialValue;
      for (int index1 = 0; index1 < numArray.Length; ++index1)
      {
        ushort num2 = 0;
        ushort num3 = (ushort) (index1 << 8);
        for (int index2 = 0; index2 < 8; ++index2)
        {
          if ((((int) num2 ^ (int) num3) & 32768) != 0)
            num2 = (ushort) ((int) num2 << 1 ^ 4129);
          else
            num2 <<= 1;
          num3 <<= 1;
        }
        numArray[index1] = num2;
      }
      for (int index = 0; index < (int) size; ++index)
        num1 = (ushort) ((uint) num1 << 8 ^ (uint) numArray[(int) num1 >> 8 ^ (int) byte.MaxValue & (int) bytes[index]]);
      return num1;
    }

    private byte[] createNDCSetTimerData(DateTime timepoint)
    {
      return new byte[7]
      {
        (byte) (timepoint.Year % 100),
        (byte) timepoint.Month,
        (byte) timepoint.Day,
        (byte) timepoint.Hour,
        (byte) timepoint.Minute,
        (byte) timepoint.Second,
        (byte) 0
      };
    }

    private byte[] createNDCGetConfigChangedCounter()
    {
      return new byte[6]
      {
        this.ConfigChangedCounterAdr[3],
        this.ConfigChangedCounterAdr[2],
        this.ConfigChangedCounterAdr[1],
        this.ConfigChangedCounterAdr[0],
        (byte) 1,
        (byte) 0
      };
    }

    private byte[] createNDCSetConfigChangedCounter(byte configChangedCounter)
    {
      return new byte[5]
      {
        this.ConfigChangedCounterAdr[3],
        this.ConfigChangedCounterAdr[2],
        this.ConfigChangedCounterAdr[1],
        this.ConfigChangedCounterAdr[0],
        configChangedCounter
      };
    }

    private byte[] createNDCSetCurrentValue(double curValue)
    {
      byte[] bytes = BitConverter.GetBytes(curValue);
      return new byte[12]
      {
        this.CurrentValueAdr[3],
        this.CurrentValueAdr[2],
        this.CurrentValueAdr[1],
        this.CurrentValueAdr[0],
        bytes[0],
        bytes[1],
        bytes[2],
        bytes[3],
        bytes[4],
        bytes[5],
        bytes[6],
        bytes[7]
      };
    }

    private bool checkTimeDif(byte minute1, byte minute2, byte minute_delta)
    {
      bool flag = false;
      if ((int) minute1 <= (int) minute_delta)
        minute1 += (byte) 60;
      if ((int) minute2 <= (int) minute_delta)
        minute2 += (byte) 60;
      byte num = (int) minute1 <= (int) minute2 ? (byte) ((uint) minute2 - (uint) minute1) : (byte) ((uint) minute1 - (uint) minute2);
      if ((int) num >= (int) minute_delta && num < (byte) 59)
        flag = true;
      return flag;
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        object result = (object) null;
        NFCL_HandlerFunctions handler = this.windowFunctions.MyFunctions;
        if (sender == this.ButtonReadVersion)
        {
          FirmwareVersion firmwareVersion = await handler.ReadVersionAsync(this.progress, this.tokenSource.Token);
          result = (object) firmwareVersion;
          firmwareVersion = new FirmwareVersion();
        }
        else if (sender == this.ButtonHardwareTypeManager)
        {
          HardwareTypeEditor dlg;
          if (handler.myMeters.WorkMeter != null)
            dlg = new HardwareTypeEditor(new string[2]
            {
              "NFC_LoRa",
              "NFC_wMBus"
            }, new uint?(handler.myMeters.WorkMeter.meterMemory.FirmwareVersion));
          else
            dlg = new HardwareTypeEditor(new string[2]
            {
              "NFC_LoRa",
              "NFC_wMBus"
            });
          dlg.ShowDialog();
          dlg = (HardwareTypeEditor) null;
        }
        else if (sender == this.ButtonReadWrite_SerialNumberFull_DevEUI_JoinEUI_DevKey)
        {
          int numberOfIds = 1;
          IdContainer idContainer = DbBasis.PrimaryDB.BaseDbConnection.GetNewIds("SerialNumber_NFC_LoRa", numberOfIds);
          int serialnumber = idContainer.GetNextID();
          int num = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
          SortedList<OverrideID, ConfigurationParameter> configParameters = handler.GetConfigurationParameters(0);
          configParameters[OverrideID.SerialNumberFull].ParameterValue = (object) Utility.ConvertSerialNumberToFullserialNumber(Utility.LoraDevice.NFCL, serialnumber);
          configParameters[OverrideID.DevEUI].ParameterValue = (object) Utility.ConvertSerialNumberToEUI64(Utility.LoraDevice.NFCL, serialnumber);
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
            DateTime previousTimeToSendLoRa = new DateTime(2000, 1, 1, 0, 0, 0);
            double newCurValue = 10.0;
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
              dlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
              if (dlg.ShowDialog().ToString().Equals("OK"))
                Pfad = dlg.FileName;
            }
            do
            {
              try
              {
                int num = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
                result = (object) num;
                Common32BitCommands.SystemTime currentDeviceTime = await handler.cmd.Device.GetSystemTimeAsync(this.progress, this.tokenSource.Token);
                DateTime currentDeviceTimeTMP = new DateTime(2000 + (int) currentDeviceTime.Year, (int) currentDeviceTime.Month, (int) currentDeviceTime.Day, (int) currentDeviceTime.Hour, (int) currentDeviceTime.Min, (int) currentDeviceTime.Sec);
                byte[] time_to_send_data = handler.myMeters.WorkMeter.meterMemory.GetData(NFCL_Params.time_to_send_data);
                DateTime localDate = DateTime.Now;
                System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
                textBoxResult1.Text = textBoxResult1.Text + "Time: " + localDate.ToString();
                this.TextBoxResult.Text += Environment.NewLine;
                System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
                textBoxResult2.Text = textBoxResult2.Text + "time_to_send_data = " + Util.ByteArrayToHexString(time_to_send_data) + Environment.NewLine;
                System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
                textBoxResult3.Text = textBoxResult3.Text + "time_to_send_data.day  " + time_to_send_data[0].ToString() + ", time " + time_to_send_data[1].ToString() + ":" + time_to_send_data[2].ToString() + ":" + time_to_send_data[3].ToString() + Environment.NewLine;
                System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
                textBoxResult4.Text = textBoxResult4.Text + "time_to_send_data.packet: " + time_to_send_data[20].ToString() + this.GetPacketString(time_to_send_data[20]) + Environment.NewLine;
                System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
                System.Windows.Controls.TextBox textBox = textBoxResult5;
                string[] strArray = new string[12]
                {
                  textBoxResult5.Text,
                  "current device time day ",
                  currentDeviceTimeTMP.Day.ToString(),
                  ".",
                  currentDeviceTimeTMP.Month.ToString(),
                  ", time ",
                  currentDeviceTimeTMP.Hour.ToString(),
                  ":",
                  null,
                  null,
                  null,
                  null
                };
                int num1 = currentDeviceTimeTMP.Minute;
                strArray[8] = num1.ToString();
                strArray[9] = ":";
                num1 = currentDeviceTimeTMP.Second;
                strArray[10] = num1.ToString();
                strArray[11] = Environment.NewLine;
                string str1 = string.Concat(strArray);
                textBox.Text = str1;
                byte day = time_to_send_data[0];
                DateTime timeToSendLoRa = new DateTime(2000, 1, 1, (int) time_to_send_data[1], (int) time_to_send_data[2], (int) time_to_send_data[3]);
                if (day > (byte) 0)
                  timeToSendLoRa = new DateTime(currentDeviceTimeTMP.Year, currentDeviceTimeTMP.Month, (int) day, (int) time_to_send_data[1], (int) time_to_send_data[2], (int) time_to_send_data[3]);
                DateTime timeToSendTest = timeToSendLoRa;
                timeToSendTest.AddSeconds(12.0);
                byte con_work_state = handler.myMeters.WorkMeter.meterMemory.GetData(NFCL_Params.con_work_state)[0];
                byte con_fast_check = handler.myMeters.WorkMeter.meterMemory.GetData(NFCL_Params.con_fast_check)[0];
                System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
                textBoxResult6.Text = textBoxResult6.Text + "NFC connector state " + con_work_state.ToString() + " con_fast_check = " + con_fast_check.ToString();
                bool ok = false;
                if ((con_work_state == (byte) 5 || con_work_state == (byte) 6) && con_fast_check == (byte) 0)
                {
                  byte CommunicationScenario = handler.myMeters.WorkMeter.meterMemory.GetData(NFCL_Params.CommunicationScenario)[0];
                  byte[] nfc_ident = handler.myMeters.WorkMeter.meterMemory.GetData(NFCL_Params.nfc_ident);
                  uint meterID = (uint) ((int) nfc_ident[27] << 24 | (int) nfc_ident[26] << 16 | (int) nfc_ident[25] << 8) | (uint) nfc_ident[24];
                  ushort crcInitValue = (ushort) ((uint) (ushort) (meterID >> 16) ^ (uint) (ushort) meterID);
                  if (CommunicationScenario == (byte) 203 || CommunicationScenario == (byte) 204)
                  {
                    if (currentDeviceTimeTMP.Hour != timeToSendTest.Hour || currentDeviceTimeTMP.Hour == timeToSendTest.Hour && currentDeviceTimeTMP > timeToSendTest && (time_to_send_data[20] == (byte) 17 || time_to_send_data[20] == (byte) 0))
                    {
                      int prev_hour = currentDeviceTimeTMP.Hour;
                      int prev_day = currentDeviceTimeTMP.Day;
                      int prev_month = currentDeviceTimeTMP.Month;
                      currentDeviceTimeTMP.AddHours(1.0);
                      if (currentDeviceTimeTMP.Hour < prev_hour)
                      {
                        prev_day = currentDeviceTimeTMP.Day;
                        currentDeviceTimeTMP.AddDays(1.0);
                      }
                      if (currentDeviceTimeTMP.Day < prev_day)
                      {
                        prev_month = currentDeviceTimeTMP.Month;
                        currentDeviceTimeTMP.AddMonths(1);
                      }
                      if (currentDeviceTimeTMP.Month < prev_month)
                        currentDeviceTimeTMP.AddYears(1);
                      currentDeviceTimeTMP = new DateTime(currentDeviceTimeTMP.Year, currentDeviceTimeTMP.Month, currentDeviceTimeTMP.Day, currentDeviceTimeTMP.Hour, 59, 55);
                      byte ConfigChangedCounter = 0;
                      byte[] dataGetConfigChangedCounter = this.createNDCGetConfigChangedCounter();
                      NfcFrame GetConfigChangedCounterFrame = new NfcFrame(NfcCommands.ReadMemory, dataGetConfigChangedCounter, "ReadMemory", new ushort?(crcInitValue));
                      do
                      {
                        try
                        {
                          byte[] retValGetChangedCounter = await handler.cmd.Special.SendToNfcDeviceAsync(GetConfigChangedCounterFrame.NfcRequestFrame, this.progress, this.tokenSource.Token);
                          Debug.WriteLine(Util.ByteArrayToHexString(retValGetChangedCounter));
                          ok = true;
                          if (retValGetChangedCounter[0] != (byte) 254)
                            ConfigChangedCounter = retValGetChangedCounter[6];
                          retValGetChangedCounter = (byte[]) null;
                        }
                        catch (NACK_Exception ex)
                        {
                          ok = false;
                        }
                      }
                      while (!ok);
                      --ConfigChangedCounter;
                      ++newCurValue;
                      byte[] data = this.createNDCSetCurrentValue(newCurValue);
                      NfcFrame SetCurValueFrame = new NfcFrame(NfcCommands.WriteMemory, data, "WriteMemory", new ushort?(crcInitValue));
                      do
                      {
                        try
                        {
                          byte[] retValSetCurVal = await handler.cmd.Special.SendToNfcDeviceAsync(SetCurValueFrame.NfcRequestFrame, this.progress, this.tokenSource.Token);
                          Debug.WriteLine(Util.ByteArrayToHexString(retValSetCurVal));
                          ok = true;
                          retValSetCurVal = (byte[]) null;
                        }
                        catch (NACK_Exception ex)
                        {
                          ok = false;
                        }
                      }
                      while (!ok);
                      byte[] dataSetConfigChangedCounter = this.createNDCSetConfigChangedCounter(ConfigChangedCounter);
                      NfcFrame SetConfigChangedCounterFrame = new NfcFrame(NfcCommands.WriteMemory, dataSetConfigChangedCounter, "WriteMemory", new ushort?(crcInitValue));
                      do
                      {
                        try
                        {
                          byte[] retValSetChangedCounter = await handler.cmd.Special.SendToNfcDeviceAsync(SetConfigChangedCounterFrame.NfcRequestFrame, this.progress, this.tokenSource.Token);
                          Debug.WriteLine(Util.ByteArrayToHexString(retValSetChangedCounter));
                          ok = true;
                          retValSetChangedCounter = (byte[]) null;
                        }
                        catch (NACK_Exception ex)
                        {
                          ok = false;
                        }
                      }
                      while (!ok);
                      System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
                      textBoxResult7.Text = textBoxResult7.Text + " newCurValue = " + newCurValue.ToString() + Environment.NewLine;
                      await Task.Delay(5000);
                      previousTimeToSendLoRa = timeToSendLoRa;
                      System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
                      textBoxResult8.Text = textBoxResult8.Text + "next hour" + Environment.NewLine;
                      System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
                      textBoxResult9.Text = textBoxResult9.Text + "set system time = " + currentDeviceTimeTMP.ToString("s") + Environment.NewLine;
                      byte[] dataForSetTime = this.createNDCSetTimerData(currentDeviceTimeTMP);
                      NfcFrame SetTimeFrame = new NfcFrame(NfcCommands.SetSystemDateTime, dataForSetTime, "SetSystemDateTime", new ushort?(crcInitValue));
                      do
                      {
                        try
                        {
                          byte[] retValSetTime = await handler.cmd.Special.SendToNfcDeviceAsync(SetTimeFrame.NfcRequestFrame, this.progress, this.tokenSource.Token);
                          Debug.WriteLine(Util.ByteArrayToHexString(retValSetTime));
                          ok = true;
                          retValSetTime = (byte[]) null;
                        }
                        catch (NACK_Exception ex)
                        {
                          ok = false;
                        }
                      }
                      while (!ok);
                      do
                      {
                        try
                        {
                          await handler.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(currentDeviceTimeTMP, (sbyte) 1), this.progress, this.tokenSource.Token);
                          ok = true;
                        }
                        catch (NACK_Exception ex)
                        {
                          ok = false;
                        }
                      }
                      while (!ok);
                      this.TextBoxResult.CaretIndex = this.TextBoxResult.Text.Length;
                      Rect rect = this.TextBoxResult.GetRectFromCharacterIndex(this.TextBoxResult.CaretIndex);
                      this.TextBoxResult.ScrollToHorizontalOffset(rect.Right);
                      if (!this.exitSendNext)
                        await Task.Delay(14000);
                      dataGetConfigChangedCounter = (byte[]) null;
                      GetConfigChangedCounterFrame = (NfcFrame) null;
                      data = (byte[]) null;
                      SetCurValueFrame = (NfcFrame) null;
                      dataSetConfigChangedCounter = (byte[]) null;
                      SetConfigChangedCounterFrame = (NfcFrame) null;
                      dataForSetTime = (byte[]) null;
                      SetTimeFrame = (NfcFrame) null;
                      rect = new Rect();
                    }
                    else
                    {
                      this.TextBoxResult.Text += Environment.NewLine;
                      previousTimeToSendLoRa = timeToSendLoRa;
                      System.Windows.Controls.TextBox textBoxResult10 = this.TextBoxResult;
                      textBoxResult10.Text = textBoxResult10.Text + "set time to send" + Environment.NewLine;
                      not_ready = false;
                      timeToSendLoRa = timeToSendLoRa.AddSeconds(-5.0);
                      System.Windows.Controls.TextBox textBoxResult11 = this.TextBoxResult;
                      textBoxResult11.Text = textBoxResult11.Text + "set system time = " + timeToSendLoRa.ToString("s") + Environment.NewLine;
                      byte[] dataForSetTime = this.createNDCSetTimerData(timeToSendLoRa);
                      NfcFrame SetTimeFrame = new NfcFrame(NfcCommands.SetSystemDateTime, dataForSetTime, "SetSystemDateTime", new ushort?(crcInitValue));
                      do
                      {
                        try
                        {
                          byte[] retValSetTime = await handler.cmd.Special.SendToNfcDeviceAsync(SetTimeFrame.NfcRequestFrame, this.progress, this.tokenSource.Token);
                          Debug.WriteLine(Util.ByteArrayToHexString(retValSetTime));
                          ok = true;
                          retValSetTime = (byte[]) null;
                        }
                        catch (NACK_Exception ex)
                        {
                          ok = false;
                        }
                      }
                      while (!ok);
                      do
                      {
                        try
                        {
                          await handler.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(timeToSendLoRa, (sbyte) 1), this.progress, this.tokenSource.Token);
                          ok = true;
                        }
                        catch (NACK_Exception ex)
                        {
                          ok = false;
                        }
                      }
                      while (!ok);
                      this.TextBoxResult.CaretIndex = this.TextBoxResult.Text.Length;
                      Rect rect = this.TextBoxResult.GetRectFromCharacterIndex(this.TextBoxResult.CaretIndex);
                      this.TextBoxResult.ScrollToHorizontalOffset(rect.Right);
                      ++packets_to_send;
                      System.Windows.Controls.TextBox textBoxResult12 = this.TextBoxResult;
                      textBoxResult12.Text = textBoxResult12.Text + "packet Nr " + packets_to_send.ToString() + Environment.NewLine;
                      if (!this.exitSendNext)
                        await Task.Delay(8000);
                      dataForSetTime = (byte[]) null;
                      SetTimeFrame = (NfcFrame) null;
                      rect = new Rect();
                    }
                  }
                  else if (currentDeviceTimeTMP.Day != timeToSendLoRa.Day || currentDeviceTimeTMP > timeToSendLoRa)
                  {
                    if (previousTimeToSendLoRa < timeToSendLoRa && currentDeviceTimeTMP.Day == timeToSendLoRa.Day)
                    {
                      this.TextBoxResult.Text += Environment.NewLine;
                      System.Windows.Controls.TextBox textBoxResult13 = this.TextBoxResult;
                      textBoxResult13.Text = textBoxResult13.Text + "LoRa send timing is missed -> do not set midnight but wait to get the device time to end sending" + Environment.NewLine;
                      await Task.Delay(40000);
                      previousTimeToSendLoRa = timeToSendLoRa;
                    }
                    else
                    {
                      byte[] dataGetConfigChangedCounter = this.createNDCGetConfigChangedCounter();
                      NfcFrame GetConfigChangedCounterFrame = new NfcFrame(NfcCommands.ReadMemory, dataGetConfigChangedCounter, "ReadMemory", new ushort?(crcInitValue));
                      byte ConfigChangedCounter = 0;
                      do
                      {
                        try
                        {
                          byte[] retValGetChangedCounter = await handler.cmd.Special.SendToNfcDeviceAsync(GetConfigChangedCounterFrame.NfcRequestFrame, this.progress, this.tokenSource.Token);
                          Debug.WriteLine(Util.ByteArrayToHexString(retValGetChangedCounter));
                          ok = true;
                          if (retValGetChangedCounter[0] != (byte) 254)
                            ConfigChangedCounter = retValGetChangedCounter[6];
                          retValGetChangedCounter = (byte[]) null;
                        }
                        catch (NACK_Exception ex)
                        {
                          ok = false;
                        }
                      }
                      while (!ok);
                      --ConfigChangedCounter;
                      ++newCurValue;
                      byte[] data = this.createNDCSetCurrentValue(newCurValue);
                      NfcFrame SetCurValueFrame = new NfcFrame(NfcCommands.WriteMemory, data, "WriteMemory", new ushort?(crcInitValue));
                      do
                      {
                        try
                        {
                          byte[] retValSetCurVal = await handler.cmd.Special.SendToNfcDeviceAsync(SetCurValueFrame.NfcRequestFrame, this.progress, this.tokenSource.Token);
                          Debug.WriteLine(Util.ByteArrayToHexString(retValSetCurVal));
                          ok = true;
                          retValSetCurVal = (byte[]) null;
                        }
                        catch (NACK_Exception ex)
                        {
                          ok = false;
                        }
                      }
                      while (!ok);
                      byte[] dataSetConfigChangedCounter = this.createNDCSetConfigChangedCounter(ConfigChangedCounter);
                      NfcFrame SetConfigChangedCounterFrame = new NfcFrame(NfcCommands.WriteMemory, dataSetConfigChangedCounter, "WriteMemory", new ushort?(crcInitValue));
                      do
                      {
                        try
                        {
                          byte[] retValSetChangedCounter = await handler.cmd.Special.SendToNfcDeviceAsync(SetConfigChangedCounterFrame.NfcRequestFrame, this.progress, this.tokenSource.Token);
                          Debug.WriteLine(Util.ByteArrayToHexString(retValSetChangedCounter));
                          ok = true;
                          retValSetChangedCounter = (byte[]) null;
                        }
                        catch (NACK_Exception ex)
                        {
                          ok = false;
                        }
                      }
                      while (!ok);
                      System.Windows.Controls.TextBox textBoxResult14 = this.TextBoxResult;
                      textBoxResult14.Text = textBoxResult14.Text + " newCurValue = " + newCurValue.ToString() + Environment.NewLine;
                      await Task.Delay(5000);
                      previousTimeToSendLoRa = timeToSendLoRa;
                      System.Windows.Controls.TextBox textBoxResult15 = this.TextBoxResult;
                      textBoxResult15.Text = textBoxResult15.Text + "set midnight" + Environment.NewLine;
                      DateTime timeMidnight = new DateTime(currentDeviceTimeTMP.Year, currentDeviceTimeTMP.Month, currentDeviceTimeTMP.Day, 23, 59, 59);
                      currentDeviceTimeTMP = currentDeviceTimeTMP.AddDays(1.0);
                      timeMidnight = timeMidnight.AddSeconds(-5.0);
                      System.Windows.Controls.TextBox textBoxResult16 = this.TextBoxResult;
                      textBoxResult16.Text = textBoxResult16.Text + "set system time = " + timeMidnight.ToString("s") + Environment.NewLine;
                      byte[] dataForSetTime = this.createNDCSetTimerData(timeMidnight);
                      NfcFrame SetTimeFrame = new NfcFrame(NfcCommands.SetSystemDateTime, dataForSetTime, "SetSystemDateTime", new ushort?(crcInitValue));
                      do
                      {
                        try
                        {
                          byte[] retValSetTime = await handler.cmd.Special.SendToNfcDeviceAsync(SetTimeFrame.NfcRequestFrame, this.progress, this.tokenSource.Token);
                          Debug.WriteLine(Util.ByteArrayToHexString(retValSetTime));
                          ok = true;
                          retValSetTime = (byte[]) null;
                        }
                        catch (NACK_Exception ex)
                        {
                          ok = false;
                        }
                      }
                      while (!ok);
                      await handler.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(timeMidnight, (sbyte) 1), this.progress, this.tokenSource.Token);
                      this.TextBoxResult.CaretIndex = this.TextBoxResult.Text.Length;
                      Rect rect = this.TextBoxResult.GetRectFromCharacterIndex(this.TextBoxResult.CaretIndex);
                      this.TextBoxResult.ScrollToHorizontalOffset(rect.Right);
                      if (!this.exitSendNext)
                        await Task.Delay(14000);
                      dataGetConfigChangedCounter = (byte[]) null;
                      GetConfigChangedCounterFrame = (NfcFrame) null;
                      data = (byte[]) null;
                      SetCurValueFrame = (NfcFrame) null;
                      dataSetConfigChangedCounter = (byte[]) null;
                      SetConfigChangedCounterFrame = (NfcFrame) null;
                      dataForSetTime = (byte[]) null;
                      SetTimeFrame = (NfcFrame) null;
                      rect = new Rect();
                    }
                  }
                  else
                  {
                    this.TextBoxResult.Text += Environment.NewLine;
                    previousTimeToSendLoRa = timeToSendLoRa;
                    System.Windows.Controls.TextBox textBoxResult17 = this.TextBoxResult;
                    textBoxResult17.Text = textBoxResult17.Text + "set time to send" + Environment.NewLine;
                    not_ready = false;
                    timeToSendLoRa = timeToSendLoRa.AddSeconds(-15.0);
                    System.Windows.Controls.TextBox textBoxResult18 = this.TextBoxResult;
                    textBoxResult18.Text = textBoxResult18.Text + "set system time = " + timeToSendLoRa.ToString("s") + Environment.NewLine;
                    byte[] dataForSetTime = this.createNDCSetTimerData(timeToSendLoRa);
                    NfcFrame SetTimeFrame = new NfcFrame(NfcCommands.SetSystemDateTime, dataForSetTime, "SetSystemDateTime", new ushort?(crcInitValue));
                    do
                    {
                      try
                      {
                        byte[] retValSetTime = await handler.cmd.Special.SendToNfcDeviceAsync(SetTimeFrame.NfcRequestFrame, this.progress, this.tokenSource.Token);
                        Debug.WriteLine(Util.ByteArrayToHexString(retValSetTime));
                        ok = true;
                        retValSetTime = (byte[]) null;
                      }
                      catch (NACK_Exception ex)
                      {
                        ok = false;
                      }
                    }
                    while (!ok);
                    do
                    {
                      try
                      {
                        await handler.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(timeToSendLoRa, (sbyte) 1), this.progress, this.tokenSource.Token);
                        ok = true;
                      }
                      catch (NACK_Exception ex)
                      {
                        ok = false;
                      }
                    }
                    while (!ok);
                    this.TextBoxResult.CaretIndex = this.TextBoxResult.Text.Length;
                    Rect rect = this.TextBoxResult.GetRectFromCharacterIndex(this.TextBoxResult.CaretIndex);
                    this.TextBoxResult.ScrollToHorizontalOffset(rect.Right);
                    ++packets_to_send;
                    System.Windows.Controls.TextBox textBoxResult19 = this.TextBoxResult;
                    textBoxResult19.Text = textBoxResult19.Text + "packet Nr " + packets_to_send.ToString() + Environment.NewLine;
                    if (!this.exitSendNext)
                      await Task.Delay(30000);
                    dataForSetTime = (byte[]) null;
                    SetTimeFrame = (NfcFrame) null;
                    rect = new Rect();
                  }
                  nfc_ident = (byte[]) null;
                }
                else
                {
                  this.TextBoxResult.Text += Environment.NewLine;
                  await Task.Delay(4000);
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
                currentDeviceTime = (Common32BitCommands.SystemTime) null;
                time_to_send_data = (byte[]) null;
              }
              catch (Exception ex)
              {
                this.TextBoxResult.Text += Environment.NewLine;
                System.Windows.Controls.TextBox textBoxResult = this.TextBoxResult;
                textBoxResult.Text = textBoxResult.Text + ex.Message + Environment.NewLine + Environment.NewLine;
              }
            }
            while ((not_ready || packets_to_send < max_packets_to_send) && !this.exitSendNext);
            return;
          }
          if (sender == this.ButtonSendSP9T1)
          {
            this.test_check_parameter_to_send();
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
          else if (sender == this.ButtonReceiveTestPacket)
          {
            this.ButtonReceiveTestPacket.IsEnabled = false;
            try
            {
              double? nullable = await handler.ReceiveTestPacketAsync(this.progress, this.tokenSource.Token, (byte) 200, 11111111U, "0FF0");
              result = (object) nullable;
              nullable = new double?();
            }
            finally
            {
              this.ButtonReceiveTestPacket.IsEnabled = true;
            }
          }
          else if (sender == this.ButtonReceiveTestPacketViaMiCon)
            ReceiveTestPacketMiConWindow.Show((Window) this);
          else if (sender == this.ButtonSendTestPacketViaMiCon)
            SendTestPacketMiConWindow.Show((Window) this);
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
        handler = (NFCL_HandlerFunctions) null;
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
      NFCL_HandlerFunctions handler = this.windowFunctions.MyFunctions;
      RadioMode mode = RadioMode.Radio3;
      uint serialnumber = 11111111;
      byte[] arbitraryData = new byte[28];
      TestWindows.logger.Info("TEST: NFCL = > MiCon");
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
        TestWindows.logger.Info("TEST: MiCon => NFCL");
        System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
        textBoxResult2.Text = textBoxResult2.Text + Environment.NewLine + "Start NFCL receiver (" + (ushort) 10.ToString() + " sec)" + Environment.NewLine;
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
            TestWindows.logger.Info("Successfully send/receive a TEST packet from MinoConnect to NFCL");
            System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
            textBoxResult5.Text = textBoxResult5.Text + "MiCon = > NFCL OK RSSI: " + resultReceiver2.Result.ToString() + Environment.NewLine;
          }
          else
          {
            TestWindows.logger.Error("Failed send/receive a TEST packet from MinoConnect to NFCL");
            System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
            textBoxResult6.Text = textBoxResult6.Text + "MiCon = > NFCL Error" + Environment.NewLine;
          }
        }
        catch (Exception ex)
        {
          TestWindows.logger.Error("Failed send/receive a TEST packet from MinoConnect to NFCL");
          System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
          textBoxResult7.Text = textBoxResult7.Text + "MiCon = > NFCL Error: " + ex.Message + Environment.NewLine;
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
        textBoxResult9.Text = textBoxResult9.Text + "Send test packet via NFCL" + Environment.NewLine;
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
          TestWindows.logger.Info("Successfully send/receive a TEST packet from NFCL to MinoConnect. RSSI: " + resultReceiver.RSSI.ToString() + " dBm");
          System.Windows.Controls.TextBox textBoxResult10 = this.TextBoxResult;
          textBoxResult10.Text = textBoxResult10.Text + "NFCL = > MiCon OK RSSI:" + resultReceiver.RSSI.ToString() + Environment.NewLine;
        }
        else
        {
          TestWindows.logger.Error("Failed send/receive a TEST packet from NFCL to MinoConnect");
          System.Windows.Controls.TextBox textBoxResult11 = this.TextBoxResult;
          textBoxResult11.Text = textBoxResult11.Text + "NFCL = > MiCon Error" + Environment.NewLine;
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
      handler = (NFCL_HandlerFunctions) null;
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

    private async void ButtonReadIUW_Click(object sender, RoutedEventArgs e)
    {
      this.tokenSource = new CancellationTokenSource();
      try
      {
        NfcDeviceIdentification iuwIdent = await this.windowFunctions.MyFunctions.GetIdentificationIUW(this.progress, this.tokenSource.Token);
        this.TextBoxResult.Text = iuwIdent.GetFirmwareVersionString();
        iuwIdent = (NfcDeviceIdentification) null;
      }
      catch (Exception ex)
      {
        this.TextBoxResult.Text = ex.Message;
        TestWindows.logger.Error(ex.Message);
      }
    }

    private async void ButtonReadMBusState_Click(object sender, RoutedEventArgs e)
    {
      this.tokenSource = new CancellationTokenSource();
      try
      {
        byte state = await this.windowFunctions.MyFunctions.GetMBusStatusByteAsync(this.progress, this.tokenSource.Token);
        this.TextBoxResult.Text = "M-Bus state: " + state.ToString();
      }
      catch (Exception ex)
      {
        this.TextBoxResult.Text = ex.Message;
        TestWindows.logger.Error(ex.Message);
      }
    }

    private bool HAL_IS_BIT_SET(byte state, byte flag) => (int) state == (int) flag;

    private bool is_time_read_parameter() => this.time_read_parameter;

    private byte Lora_packet_prepare_values()
    {
      System.Windows.Controls.TextBox textBoxResult = this.TextBoxResult;
      textBoxResult.Text = textBoxResult.Text + "Lora_packet_prepare_values = " + this.prepare_values_state.ToString() + Environment.NewLine;
      if (this.prepare_values_state == (byte) 0)
      {
        ++this.repetition_counter;
        if (this.repetition_counter >= (byte) 2)
          this.con_work_state = (byte) 6;
        if (this.con_work_state == (byte) 6)
        {
          ++this.fault_counter;
          if (this.fault_counter == (byte) 3)
            this.MeterState = (byte) 1;
        }
      }
      else
      {
        this.con_work_state = (byte) 5;
        this.MeterState = (byte) 0;
        this.fault_counter = (byte) 0;
        this.repetition_counter = (byte) 0;
      }
      return this.prepare_values_state;
    }

    private byte con_prepare_wmbus_values_reading() => 1;

    private async void test_check_parameter_to_send()
    {
      this.repetition_counter = (byte) 0;
      this.fault_counter = (byte) 0;
      this.CommunicationScenario = (ushort) 204;
      this.MeterState = (byte) 0;
      this.value_reading_status = (byte) 3;
      this.radio_transmit.Command = (byte) 0;
      this.radio_transmit.Time_shift = (byte) 0;
      this.con_work_state = (byte) 5;
      this.prepare_values_state = (byte) 0;
      for (int i = 0; i <= 100; ++i)
      {
        for (int j = 0; j <= 60; ++j)
        {
          this.time_read_parameter = false;
          if (j > 40)
            this.time_read_parameter = true;
          System.Windows.Controls.TextBox textBoxResult1 = this.TextBoxResult;
          textBoxResult1.Text = textBoxResult1.Text + "IN" + Environment.NewLine;
          System.Windows.Controls.TextBox textBoxResult2 = this.TextBoxResult;
          textBoxResult2.Text = textBoxResult2.Text + "CommunicationScenario = " + this.CommunicationScenario.ToString() + Environment.NewLine;
          System.Windows.Controls.TextBox textBoxResult3 = this.TextBoxResult;
          textBoxResult3.Text = textBoxResult3.Text + "MeterState = " + this.MeterState.ToString() + Environment.NewLine;
          System.Windows.Controls.TextBox textBoxResult4 = this.TextBoxResult;
          textBoxResult4.Text = textBoxResult4.Text + "value_reading_status = " + this.value_reading_status.ToString() + Environment.NewLine;
          System.Windows.Controls.TextBox textBoxResult5 = this.TextBoxResult;
          textBoxResult5.Text = textBoxResult5.Text + "radio_transmit.Command = " + this.radio_transmit.Command.ToString() + Environment.NewLine;
          System.Windows.Controls.TextBox textBoxResult6 = this.TextBoxResult;
          textBoxResult6.Text = textBoxResult6.Text + "radio_transmit.Time_shift = " + this.radio_transmit.Time_shift.ToString() + Environment.NewLine;
          System.Windows.Controls.TextBox textBoxResult7 = this.TextBoxResult;
          textBoxResult7.Text = textBoxResult7.Text + "con_work_state = " + this.con_work_state.ToString() + Environment.NewLine;
          System.Windows.Controls.TextBox textBoxResult8 = this.TextBoxResult;
          textBoxResult8.Text = textBoxResult8.Text + "prepare_values_state = " + this.prepare_values_state.ToString() + Environment.NewLine;
          System.Windows.Controls.TextBox textBoxResult9 = this.TextBoxResult;
          textBoxResult9.Text = textBoxResult9.Text + "time_read_parameter = " + this.time_read_parameter.ToString() + Environment.NewLine;
          this.check_parameter_to_send();
          if (this.con_work_state != (byte) 6)
            ;
          System.Windows.Controls.TextBox textBoxResult10 = this.TextBoxResult;
          textBoxResult10.Text = textBoxResult10.Text + "OUT" + Environment.NewLine;
          System.Windows.Controls.TextBox textBoxResult11 = this.TextBoxResult;
          textBoxResult11.Text = textBoxResult11.Text + "MeterState = " + this.MeterState.ToString() + Environment.NewLine;
          System.Windows.Controls.TextBox textBoxResult12 = this.TextBoxResult;
          textBoxResult12.Text = textBoxResult12.Text + "value_reading_status = " + this.value_reading_status.ToString() + Environment.NewLine;
          System.Windows.Controls.TextBox textBoxResult13 = this.TextBoxResult;
          textBoxResult13.Text = textBoxResult13.Text + "radio_transmit.Command = " + this.radio_transmit.Command.ToString() + Environment.NewLine;
          System.Windows.Controls.TextBox textBoxResult14 = this.TextBoxResult;
          textBoxResult14.Text = textBoxResult14.Text + "radio_transmit.Time_shift = " + this.radio_transmit.Time_shift.ToString() + Environment.NewLine;
          System.Windows.Controls.TextBox textBoxResult15 = this.TextBoxResult;
          textBoxResult15.Text = textBoxResult15.Text + "con_work_state = " + this.con_work_state.ToString() + Environment.NewLine;
          System.Windows.Controls.TextBox textBoxResult16 = this.TextBoxResult;
          textBoxResult16.Text = textBoxResult16.Text + "prepare_values_state = " + this.prepare_values_state.ToString() + Environment.NewLine;
          System.Windows.Controls.TextBox textBoxResult17 = this.TextBoxResult;
          textBoxResult17.Text = textBoxResult17.Text + "time_read_parameter = " + this.time_read_parameter.ToString() + Environment.NewLine;
          System.Windows.Controls.TextBox textBoxResult18 = this.TextBoxResult;
          textBoxResult18.Text = textBoxResult18.Text + Environment.NewLine + Environment.NewLine;
          await Task.Delay(1000);
        }
      }
    }

    private void check_parameter_to_send()
    {
      if (this.value_reading_status == (byte) 1 || !this.is_time_read_parameter())
        return;
      this.radio_transmit.Command = (byte) 0;
      this.radio_transmit.Time_shift = (byte) 0;
      if (this.CommunicationScenario >= (ushort) 200 && this.CommunicationScenario < (ushort) 300)
      {
        if (this.value_reading_status != (byte) 3 && this.HAL_IS_BIT_SET(this.MeterState, (byte) 1) && this.value_reading_status != (byte) 1)
        {
          this.radio_transmit.Command = (byte) 1;
          this.value_reading_status = (byte) 3;
        }
        else
        {
          if (this.con_work_state == (byte) 5 || this.value_reading_status == (byte) 3)
            this.value_reading_status = this.Lora_packet_prepare_values();
          if (this.value_reading_status == (byte) 0)
          {
            this.radio_transmit.Command = (byte) 2;
            this.radio_transmit.Time_shift = (byte) 60;
          }
        }
      }
      if (this.CommunicationScenario >= (ushort) 300 && this.CommunicationScenario < (ushort) 400)
      {
        byte num = this.con_prepare_wmbus_values_reading();
        if (num == (byte) 0 || this.value_reading_status == (byte) 0 && num == (byte) 2)
        {
          this.radio_transmit.Command = (byte) 1;
          this.value_reading_status = (byte) 0;
        }
        else
          this.value_reading_status = num;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      System.Windows.Application.LoadComponent((object) this, new Uri("/NFCL_Handler;component/ui/testwindows.xaml", UriKind.Relative));
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
          this.ButtonReadMBusState = (System.Windows.Controls.Button) target;
          this.ButtonReadMBusState.Click += new RoutedEventHandler(this.ButtonReadMBusState_Click);
          break;
        case 7:
          this.ButtonReadIUW = (System.Windows.Controls.Button) target;
          this.ButtonReadIUW.Click += new RoutedEventHandler(this.ButtonReadIUW_Click);
          break;
        case 8:
          this.ButtonRadioTest = (System.Windows.Controls.Button) target;
          this.ButtonRadioTest.Click += new RoutedEventHandler(this.ButtonRadioTest_Click);
          break;
        case 9:
          this.ButtonSendNextLoRa = (System.Windows.Controls.Button) target;
          this.ButtonSendNextLoRa.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 10:
          this.ButtonSendSP9T1 = (System.Windows.Controls.Button) target;
          this.ButtonSendSP9T1.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 11:
          this.ButtonReadWrite_SerialNumberFull_DevEUI_JoinEUI_DevKey = (System.Windows.Controls.Button) target;
          this.ButtonReadWrite_SerialNumberFull_DevEUI_JoinEUI_DevKey.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 12:
          this.ButtonGetConfigurationParameter = (System.Windows.Controls.Button) target;
          this.ButtonGetConfigurationParameter.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 13:
          this.ButtonSetMode = (System.Windows.Controls.Button) target;
          this.ButtonSetMode.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 14:
          this.ButtonHardwareTypeManager = (System.Windows.Controls.Button) target;
          this.ButtonHardwareTypeManager.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 15:
          this.ButtonAddNewMap = (System.Windows.Controls.Button) target;
          this.ButtonAddNewMap.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 16:
          this.ButtonProtection = (System.Windows.Controls.Button) target;
          this.ButtonProtection.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 17:
          this.ButtonReadVersion = (System.Windows.Controls.Button) target;
          this.ButtonReadVersion.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 18:
          this.ButtonGetRadioVersion = (System.Windows.Controls.Button) target;
          this.ButtonGetRadioVersion.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 19:
          this.ButtonTransmitModulatedCarrier = (System.Windows.Controls.Button) target;
          this.ButtonTransmitModulatedCarrier.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 20:
          this.ButtonTransmitUnmodulatedCarrier = (System.Windows.Controls.Button) target;
          this.ButtonTransmitUnmodulatedCarrier.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 21:
          this.ButtonSetFrequencyIncrement = (System.Windows.Controls.Button) target;
          this.ButtonSetFrequencyIncrement.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 22:
          this.ButtonSendTestPacket = (System.Windows.Controls.Button) target;
          this.ButtonSendTestPacket.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 23:
          this.ButtonReceiveTestPacket = (System.Windows.Controls.Button) target;
          this.ButtonReceiveTestPacket.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 24:
          this.ButtonReceiveTestPacketViaMiCon = (System.Windows.Controls.Button) target;
          this.ButtonReceiveTestPacketViaMiCon.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 25:
          this.ButtonSendTestPacketViaMiCon = (System.Windows.Controls.Button) target;
          this.ButtonSendTestPacketViaMiCon.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 26:
          this.ButtonSetDeviceIdentification = (System.Windows.Controls.Button) target;
          this.ButtonSetDeviceIdentification.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 27:
          this.ButtonReadSystemTime = (System.Windows.Controls.Button) target;
          this.ButtonReadSystemTime.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 28:
          this.ButtonWriteSystemTime = (System.Windows.Controls.Button) target;
          this.ButtonWriteSystemTime.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 29:
          this.ButtonCompareConnectedAndWork = (System.Windows.Controls.Button) target;
          this.ButtonCompareConnectedAndWork.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 30:
          this.ButtonDisableIrDa = (System.Windows.Controls.Button) target;
          this.ButtonDisableIrDa.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 31:
          this.ButtonAccessRadioKey = (System.Windows.Controls.Button) target;
          this.ButtonAccessRadioKey.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 32:
          this.ButtonGetMbus_interval = (System.Windows.Controls.Button) target;
          this.ButtonGetMbus_interval.Click += new RoutedEventHandler(this.Button_Click);
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

    private class cradio_transmit
    {
      public byte Command;
      public byte Time_shift;
    }
  }
}
