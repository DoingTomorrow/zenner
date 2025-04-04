// Decompiled with JetBrains decompiler
// Type: M8_Handler.UserInterface.TestWindows
// Assembly: M8_Handler, Version=2.0.6.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 582F1296-F274-42DF-B72B-4C0B4D92AA72
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\M8_Handler.dll

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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace M8_Handler.UserInterface
{
  public class TestWindows : Window, IComponentConnector
  {
    private static Logger logger = LogManager.GetLogger(nameof (TestWindows));
    private M8_HandlerWindowFunctions windowFunctions;
    private ProgressHandler progress;
    private CancellationTokenSource tokenSource;
    internal TextBlock TextBlockStatus;
    internal ProgressBar ProgressBar1;
    internal TextBox TextBoxResult;
    internal StackPanel StackPanalButtons;
    internal Button ButtonRadioTest;
    internal Button ButtonSendNextLoRa;
    internal Button ButtonSendSP9T1;
    internal Button ButtonReadWrite_SerialNumberFull_DevEUI_JoinEUI_DevKey;
    internal Button ButtonGetConfigurationParameter;
    internal Button ButtonSetMode;
    internal Button ButtonHardwareTypeManager;
    internal Button ButtonAddNewMap;
    internal Button ButtonProtection;
    internal Button ButtonReadVersion;
    internal Button ButtonGetRadioVersion;
    internal Button ButtonReadTemperature;
    internal Button ButtonReadTamperSwitch;
    internal Button ButtonTransmitModulatedCarrier;
    internal Button ButtonTransmitUnmodulatedCarrier;
    internal Button ButtonSetFrequencyIncrement;
    internal Button ButtonSendTestPacket;
    internal Button ButtonReceiveTestPacket;
    internal Button ButtonReceiveTestPacketViaMiCon;
    internal Button ButtonSendTestPacketViaMiCon;
    internal Button ButtonSetLcdTestState;
    internal Button ButtonSetDeviceIdentification;
    internal Button ButtonReadSystemTime;
    internal Button ButtonWriteSystemTime;
    internal Button ButtonCompareConnectedAndWork;
    internal Button ButtonDisableIrDa;
    internal Button ButtonAccessRadioKey;
    internal Button ButtonGetMbus_interval;
    internal Button ButtonBreak;
    private bool _contentLoaded;

    public TestWindows(Window owner, M8_HandlerWindowFunctions windowFunctions)
    {
      this.InitializeComponent();
      this.Owner = owner;
      this.windowFunctions = windowFunctions;
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
    }

    private void SetRunState()
    {
      this.tokenSource = new CancellationTokenSource();
      this.progress.Reset();
      this.ButtonBreak.IsEnabled = true;
      this.Cursor = Cursors.Wait;
      this.TextBoxResult.Text = string.Empty;
    }

    private void SetStopState()
    {
      this.ButtonBreak.IsEnabled = false;
      this.Cursor = (Cursor) null;
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

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        object result = (object) null;
        M8_HandlerFunctions handler = this.windowFunctions.MyFunctions;
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
              "HCA_LoRa",
              "M7plus"
            }, new uint?(handler.myMeters.WorkMeter.meterMemory.FirmwareVersion));
          else
            dlg = new HardwareTypeEditor(new string[2]
            {
              "HCA_LoRa",
              "M7plus"
            });
          dlg.ShowDialog();
          dlg = (HardwareTypeEditor) null;
        }
        else if (sender == this.ButtonReadWrite_SerialNumberFull_DevEUI_JoinEUI_DevKey)
        {
          int numberOfIds = 1;
          IdContainer idContainer = DbBasis.PrimaryDB.BaseDbConnection.GetNewIds("SerialNumber_HCA_LoRa", numberOfIds);
          int serialnumber = idContainer.GetNextID();
          int num = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
          SortedList<OverrideID, ConfigurationParameter> configParameters = handler.GetConfigurationParameters(0);
          configParameters[OverrideID.SerialNumberFull].ParameterValue = (object) Utility.ConvertSerialNumberToFullserialNumber(Utility.LoraDevice.M8, serialnumber);
          configParameters[OverrideID.DevEUI].ParameterValue = (object) Utility.ConvertSerialNumberToEUI64(Utility.LoraDevice.M8, serialnumber);
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
          result = (object) handler.AccessRadioKey_IsOK();
        else if (sender == this.ButtonReadTemperature)
        {
          Temperature temperature = await handler.ReadTemperatureAsync(this.progress, this.tokenSource.Token);
          result = (object) temperature;
          temperature = (Temperature) null;
        }
        else if (sender == this.ButtonReadTamperSwitch)
        {
          Tamper tamper = await handler.ReadTamperSwitchAsync(this.progress, this.tokenSource.Token);
          result = (object) tamper;
        }
        else
        {
          if (sender == this.ButtonSendNextLoRa)
          {
            bool not_ready = true;
            int day_to_check = 0;
            int max_days_to_check = 1000;
            do
            {
              try
              {
                int num = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
                result = (object) num;
                Common32BitCommands.SystemTime _currentTime = await handler.cmd.Device.GetSystemTimeAsync(this.progress, this.tokenSource.Token);
                DateTime currentTime = new DateTime(2000 + (int) _currentTime.Year, (int) _currentTime.Month, (int) _currentTime.Day, (int) _currentTime.Hour, (int) _currentTime.Min, (int) _currentTime.Sec);
                byte[] time_to_send_data = handler.myMeters.WorkMeter.meterMemory.GetData(M8_Params.time_to_send_data);
                DateTime localDate = DateTime.Now;
                TextBox textBoxResult1 = this.TextBoxResult;
                textBoxResult1.Text = textBoxResult1.Text + "Time: " + localDate.ToString() + Environment.NewLine;
                TextBox textBoxResult2 = this.TextBoxResult;
                textBoxResult2.Text = textBoxResult2.Text + "time_to_send_data = " + Util.ByteArrayToHexString(time_to_send_data) + Environment.NewLine;
                TextBox textBoxResult3 = this.TextBoxResult;
                textBoxResult3.Text = textBoxResult3.Text + "time_to_send_data.day  " + time_to_send_data[0].ToString() + ", time " + time_to_send_data[1].ToString() + ":" + time_to_send_data[2].ToString() + ":" + time_to_send_data[3].ToString() + Environment.NewLine;
                TextBox textBoxResult4 = this.TextBoxResult;
                textBoxResult4.Text = textBoxResult4.Text + "time_to_send_data.packet: " + time_to_send_data[15].ToString() + Environment.NewLine;
                TextBox textBoxResult5 = this.TextBoxResult;
                TextBox textBox = textBoxResult5;
                string[] strArray = new string[12];
                strArray[0] = textBoxResult5.Text;
                strArray[1] = "current device time day ";
                strArray[2] = currentTime.Day.ToString();
                strArray[3] = ".";
                int num3 = currentTime.Month;
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
                string str = string.Concat(strArray);
                textBox.Text = str;
                byte day = time_to_send_data[0];
                if (day == (byte) 0)
                {
                  currentTime = currentTime.AddDays(-1.0);
                  day = Convert.ToByte(currentTime.Day);
                  currentTime = currentTime.AddDays(1.0);
                }
                DateTime timepoint2 = new DateTime(currentTime.Year, currentTime.Month, (int) day, (int) time_to_send_data[1], (int) time_to_send_data[2], (int) time_to_send_data[3]);
                if (currentTime.Day != timepoint2.Day || currentTime > timepoint2)
                {
                  TextBox textBoxResult6 = this.TextBoxResult;
                  textBoxResult6.Text = textBoxResult6.Text + "set midnight" + Environment.NewLine;
                  timepoint2 = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 23, 59, 59);
                  currentTime = currentTime.AddDays(1.0);
                  timepoint2 = timepoint2.AddSeconds(-5.0);
                  TextBox textBoxResult7 = this.TextBoxResult;
                  textBoxResult7.Text = textBoxResult7.Text + "set system time = " + timepoint2.ToString("s") + Environment.NewLine;
                  await handler.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(timepoint2, (sbyte) 1), this.progress, this.tokenSource.Token);
                  this.TextBoxResult.CaretIndex = this.TextBoxResult.Text.Length;
                  Rect rect = this.TextBoxResult.GetRectFromCharacterIndex(this.TextBoxResult.CaretIndex);
                  this.TextBoxResult.ScrollToHorizontalOffset(rect.Right);
                  await Task.Delay(10000);
                  rect = new Rect();
                }
                else
                {
                  TextBox textBoxResult8 = this.TextBoxResult;
                  textBoxResult8.Text = textBoxResult8.Text + "set time to send" + Environment.NewLine;
                  not_ready = false;
                  timepoint2 = timepoint2.AddSeconds(-5.0);
                  TextBox textBoxResult9 = this.TextBoxResult;
                  textBoxResult9.Text = textBoxResult9.Text + "set system time = " + timepoint2.ToString("s") + Environment.NewLine;
                  await handler.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(timepoint2, (sbyte) 1), this.progress, this.tokenSource.Token);
                  this.TextBoxResult.CaretIndex = this.TextBoxResult.Text.Length;
                  Rect rect = this.TextBoxResult.GetRectFromCharacterIndex(this.TextBoxResult.CaretIndex);
                  this.TextBoxResult.ScrollToHorizontalOffset(rect.Right);
                  await Task.Delay(20000);
                  rect = new Rect();
                }
                this.TextBoxResult.Text += Environment.NewLine;
                DateTime timepoint = timepoint2.AddSeconds(10.0);
                DateTime timepointOld = timepoint2;
                DateTime timepointNew = timepoint2;
                _currentTime = (Common32BitCommands.SystemTime) null;
                time_to_send_data = (byte[]) null;
              }
              catch (Exception ex)
              {
                TextBox textBoxResult = this.TextBoxResult;
                textBoxResult.Text = textBoxResult.Text + ex.Message + Environment.NewLine;
              }
            }
            while (not_ready || ++day_to_check < max_days_to_check);
            return;
          }
          if (sender == this.ButtonSendSP9T1)
          {
            int num1 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
            result = (object) num1;
            byte SP9_send_day = handler.myMeters.WorkMeter.meterMemory.GetParameterValue<byte>(M8_Params.SP9_send_day);
            this.TextBoxResult.Text = "SP9_send_day = " + SP9_send_day.ToString() + Environment.NewLine;
            byte[] lora_start_date = handler.myMeters.WorkMeter.meterMemory.GetData(M8_Params.lora_start_date);
            DateTime? startDate1 = Util.ConvertToDate_MBus_CP16_TypeG(lora_start_date, 0);
            DateTime? startDate2 = Util.ConvertToDate_MBus_CP16_TypeG(lora_start_date, 2);
            TextBox textBoxResult10 = this.TextBoxResult;
            string text1 = textBoxResult10.Text;
            DateTime? nullable = startDate1;
            string str1 = nullable.ToString();
            string newLine1 = Environment.NewLine;
            textBoxResult10.Text = text1 + "lora_start_date 1 = " + str1 + newLine1;
            TextBox textBoxResult11 = this.TextBoxResult;
            string text2 = textBoxResult11.Text;
            nullable = startDate2;
            string str2 = nullable.ToString();
            string newLine2 = Environment.NewLine;
            textBoxResult11.Text = text2 + "lora_start_date 2 = " + str2 + newLine2;
            DateTime timepoint = new DateTime(2018, startDate1.Value.Month, (int) SP9_send_day, 23, 59, 55);
            timepoint = timepoint.AddMonths(1);
            timepoint = timepoint.AddDays(-1.0);
            TextBox textBoxResult12 = this.TextBoxResult;
            textBoxResult12.Text = textBoxResult12.Text + "set system time = " + timepoint.ToString("s") + Environment.NewLine;
            await handler.cmd.Device.SetSystemTimeAsync(new Common32BitCommands.SystemTime(timepoint, (sbyte) 1), this.progress, this.tokenSource.Token);
            await Task.Delay(14000);
            int num2 = await handler.ReadDeviceAsync(this.progress, this.tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
            result = (object) num2;
            byte[] time_to_send_data = handler.myMeters.WorkMeter.meterMemory.GetData(M8_Params.time_to_send_data);
            TextBox textBoxResult13 = this.TextBoxResult;
            textBoxResult13.Text = textBoxResult13.Text + "time_to_send_data = " + Util.ByteArrayToHexString(time_to_send_data) + Environment.NewLine;
            TextBox textBoxResult14 = this.TextBoxResult;
            textBoxResult14.Text = textBoxResult14.Text + "time_to_send_data.day = " + time_to_send_data[0].ToString() + Environment.NewLine;
            TextBox textBoxResult15 = this.TextBoxResult;
            textBoxResult15.Text = textBoxResult15.Text + "time_to_send_data.hour = " + time_to_send_data[1].ToString() + Environment.NewLine;
            TextBox textBoxResult16 = this.TextBoxResult;
            textBoxResult16.Text = textBoxResult16.Text + "time_to_send_data.minutes = " + time_to_send_data[2].ToString() + Environment.NewLine;
            TextBox textBoxResult17 = this.TextBoxResult;
            textBoxResult17.Text = textBoxResult17.Text + "time_to_send_data.seconds = " + time_to_send_data[3].ToString() + Environment.NewLine;
            timepoint = timepoint.AddSeconds(5.0);
            DateTime timepoint2 = new DateTime(timepoint.Year, timepoint.Month, (int) time_to_send_data[0], (int) time_to_send_data[1], (int) time_to_send_data[2], (int) time_to_send_data[3]);
            timepoint2 = timepoint2.AddSeconds(-4.0);
            TextBox textBoxResult18 = this.TextBoxResult;
            textBoxResult18.Text = textBoxResult18.Text + "set system time = " + timepoint2.ToString("s") + Environment.NewLine;
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
            await handler.SendTestPacketAsync(this.progress, this.tokenSource.Token, (ushort) 1, (ushort) 5, 11111111U, arbitraryData);
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
          else if (sender == this.ButtonSetLcdTestState)
            await handler.SetLcdTestStateAsync(this.progress, this.tokenSource.Token, LcdTest.PATTERN_EVEN_1_3);
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
          else if (sender == this.ButtonDisableIrDa)
            await handler.SetModeAsync(this.progress, this.tokenSource.Token, (Enum) Mode.StandbyCurrentMode);
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
        handler = (M8_HandlerFunctions) null;
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
      M8_HandlerFunctions handler = this.windowFunctions.MyFunctions;
      RadioMode mode = RadioMode.Radio3_868_95;
      uint serialnumber = 11111111;
      byte[] arbitraryData = new byte[28];
      TestWindows.logger.Info("TEST: M8 = > MiCon");
      ConnectionProfile profile = ReadoutConfigFunctions.Manager.GetConnectionProfile(32);
      profile.EquipmentModel.ChangeableParameters.Find((Predicate<ChangeableParameter>) (x => x.Key == "Port")).Value = "COM5";
      CommunicationPortFunctions port = new CommunicationPortFunctions();
      port.SetReadoutConfiguration(profile.GetSettingsList());
      TextBox textBoxResult1 = this.TextBoxResult;
      textBoxResult1.Text = textBoxResult1.Text + "Open MiCon " + profile.EquipmentModel.ChangeableParameters.Find((Predicate<ChangeableParameter>) (x => x.Key == "Port")).Value + Environment.NewLine + Environment.NewLine;
      port.Open();
      await Task.Delay(500);
      CommunicationByMinoConnect micon = port.GetCommunicationByMinoConnect();
      try
      {
        TextBox textBoxResult2 = this.TextBoxResult;
        textBoxResult2.Text = textBoxResult2.Text + "Set frequency to 868.95 MHz" + Environment.NewLine;
        await handler.SetCenterFrequencyAsync(progress, cancelToken, 868950000U);
        TestWindows.logger.Info("TEST: MiCon => M8");
        TextBox textBoxResult3 = this.TextBoxResult;
        textBoxResult3.Text = textBoxResult3.Text + Environment.NewLine + "Start M8 receiver (" + (ushort) 10.ToString() + " sec)" + Environment.NewLine;
        Task<double?> resultReceiver2 = handler.ReceiveTestPacketAsync(progress, cancelToken, (byte) 10, serialnumber, "D696");
        TextBox textBoxResult4 = this.TextBoxResult;
        textBoxResult4.Text = textBoxResult4.Text + "Send test packet via MiCon" + Environment.NewLine;
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
        TextBox textBoxResult5 = this.TextBoxResult;
        textBoxResult5.Text = textBoxResult5.Text + countOfSendPackets.ToString() + " packets are sends form MiCon" + Environment.NewLine;
        try
        {
          if (resultReceiver2.Result.HasValue)
          {
            TestWindows.logger.Info("Successfully send/receive a TEST packet from MinoConnect to M8");
            TextBox textBoxResult6 = this.TextBoxResult;
            textBoxResult6.Text = textBoxResult6.Text + "MiCon = > M8 OK RSSI: " + resultReceiver2.Result.ToString() + Environment.NewLine;
          }
          else
          {
            TestWindows.logger.Error("Failed send/receive a TEST packet from MinoConnect to M8");
            TextBox textBoxResult7 = this.TextBoxResult;
            textBoxResult7.Text = textBoxResult7.Text + "MiCon = > M8 Error" + Environment.NewLine;
          }
        }
        catch (Exception ex)
        {
          TestWindows.logger.Error("Failed send/receive a TEST packet from MinoConnect to M8");
          TextBox textBoxResult8 = this.TextBoxResult;
          textBoxResult8.Text = textBoxResult8.Text + "MiCon = > M8 Error: " + ex.Message + Environment.NewLine;
        }
        await Task.Delay(500);
        bool done = false;
        RadioTestResult resultReceiver = (RadioTestResult) null;
        TextBox textBoxResult9 = this.TextBoxResult;
        textBoxResult9.Text = textBoxResult9.Text + "Start MiCon receiver (" + (ushort) 10.ToString() + " sec)" + Environment.NewLine;
        Task taskReceiver = Task.Run((Action) (() =>
        {
          resultReceiver = micon.ReceiveOnePacket(mode, (int) serialnumber, (ushort) 10, "D696");
          done = true;
        }));
        TextBox textBoxResult10 = this.TextBoxResult;
        textBoxResult10.Text = textBoxResult10.Text + "Send test packet via M8" + Environment.NewLine;
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
          TestWindows.logger.Info("Successfully send/receive a TEST packet from M8 to MinoConnect. RSSI: " + resultReceiver.RSSI.ToString() + " dBm");
          TextBox textBoxResult11 = this.TextBoxResult;
          textBoxResult11.Text = textBoxResult11.Text + "M8 = > MiCon OK RSSI:" + resultReceiver.RSSI.ToString() + Environment.NewLine;
        }
        else
        {
          TestWindows.logger.Error("Failed send/receive a TEST packet from M8 to MinoConnect");
          TextBox textBoxResult12 = this.TextBoxResult;
          textBoxResult12.Text = textBoxResult12.Text + "M8 = > MiCon Error" + Environment.NewLine;
        }
        TextBox textBoxResult13 = this.TextBoxResult;
        textBoxResult13.Text = textBoxResult13.Text + "Set frequency to 868.30 MHz" + Environment.NewLine;
        await handler.SetCenterFrequencyAsync(progress, cancelToken, 868300000U);
        taskReceiver = (Task) null;
      }
      finally
      {
        TextBox textBoxResult14 = this.TextBoxResult;
        textBoxResult14.Text = textBoxResult14.Text + Environment.NewLine + "Close MiCon " + profile.EquipmentModel.ChangeableParameters.Find((Predicate<ChangeableParameter>) (x => x.Key == "Port")).Value + Environment.NewLine;
        port.Close();
      }
      watch.Stop();
      TextBox textBoxResult15 = this.TextBoxResult;
      textBoxResult15.Text = textBoxResult15.Text + "Elapsed: " + Util.ElapsedToString(watch.Elapsed) + Environment.NewLine;
      bool flag = true;
      watch = (Stopwatch) null;
      handler = (M8_HandlerFunctions) null;
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

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/M8_Handler;component/ui/testwindows.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.TextBlockStatus = (TextBlock) target;
          break;
        case 2:
          this.ProgressBar1 = (ProgressBar) target;
          break;
        case 3:
          this.TextBoxResult = (TextBox) target;
          break;
        case 4:
          this.StackPanalButtons = (StackPanel) target;
          break;
        case 5:
          this.ButtonRadioTest = (Button) target;
          this.ButtonRadioTest.Click += new RoutedEventHandler(this.ButtonRadioTest_Click);
          break;
        case 6:
          this.ButtonSendNextLoRa = (Button) target;
          this.ButtonSendNextLoRa.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 7:
          this.ButtonSendSP9T1 = (Button) target;
          this.ButtonSendSP9T1.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 8:
          this.ButtonReadWrite_SerialNumberFull_DevEUI_JoinEUI_DevKey = (Button) target;
          this.ButtonReadWrite_SerialNumberFull_DevEUI_JoinEUI_DevKey.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 9:
          this.ButtonGetConfigurationParameter = (Button) target;
          this.ButtonGetConfigurationParameter.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 10:
          this.ButtonSetMode = (Button) target;
          this.ButtonSetMode.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 11:
          this.ButtonHardwareTypeManager = (Button) target;
          this.ButtonHardwareTypeManager.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 12:
          this.ButtonAddNewMap = (Button) target;
          this.ButtonAddNewMap.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 13:
          this.ButtonProtection = (Button) target;
          this.ButtonProtection.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 14:
          this.ButtonReadVersion = (Button) target;
          this.ButtonReadVersion.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 15:
          this.ButtonGetRadioVersion = (Button) target;
          this.ButtonGetRadioVersion.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 16:
          this.ButtonReadTemperature = (Button) target;
          this.ButtonReadTemperature.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 17:
          this.ButtonReadTamperSwitch = (Button) target;
          this.ButtonReadTamperSwitch.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 18:
          this.ButtonTransmitModulatedCarrier = (Button) target;
          this.ButtonTransmitModulatedCarrier.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 19:
          this.ButtonTransmitUnmodulatedCarrier = (Button) target;
          this.ButtonTransmitUnmodulatedCarrier.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 20:
          this.ButtonSetFrequencyIncrement = (Button) target;
          this.ButtonSetFrequencyIncrement.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 21:
          this.ButtonSendTestPacket = (Button) target;
          this.ButtonSendTestPacket.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 22:
          this.ButtonReceiveTestPacket = (Button) target;
          this.ButtonReceiveTestPacket.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 23:
          this.ButtonReceiveTestPacketViaMiCon = (Button) target;
          this.ButtonReceiveTestPacketViaMiCon.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 24:
          this.ButtonSendTestPacketViaMiCon = (Button) target;
          this.ButtonSendTestPacketViaMiCon.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 25:
          this.ButtonSetLcdTestState = (Button) target;
          this.ButtonSetLcdTestState.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 26:
          this.ButtonSetDeviceIdentification = (Button) target;
          this.ButtonSetDeviceIdentification.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 27:
          this.ButtonReadSystemTime = (Button) target;
          this.ButtonReadSystemTime.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 28:
          this.ButtonWriteSystemTime = (Button) target;
          this.ButtonWriteSystemTime.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 29:
          this.ButtonCompareConnectedAndWork = (Button) target;
          this.ButtonCompareConnectedAndWork.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 30:
          this.ButtonDisableIrDa = (Button) target;
          this.ButtonDisableIrDa.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 31:
          this.ButtonAccessRadioKey = (Button) target;
          this.ButtonAccessRadioKey.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 32:
          this.ButtonGetMbus_interval = (Button) target;
          this.ButtonGetMbus_interval.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 33:
          this.ButtonBreak = (Button) target;
          this.ButtonBreak.Click += new RoutedEventHandler(this.ButtonBreak_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
