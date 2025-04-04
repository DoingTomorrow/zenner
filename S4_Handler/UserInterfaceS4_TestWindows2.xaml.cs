// Decompiled with JetBrains decompiler
// Type: S4_Handler.UserInterface.S4_TestWindows2
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommonWPF;
using HandlerLib;
using HandlerLib.NFC;
using S4_Handler.Functions;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace S4_Handler.UserInterface
{
  public partial class S4_TestWindows2 : Window, IComponentConnector
  {
    private CancellationTokenSource cancelTokenSource;
    private ProgressHandler progress;
    private S4_HandlerWindowFunctions myWindowFunctions;
    private S4_HandlerFunctions myFunctions;
    private S4_DeviceCommandsNFC myCommands;
    private Cursor defaultCursor;
    private string AddRandomEventButtonText;
    private string AddSelectedEventButtonText;
    private Random MyRandom;
    private List<EventLoggerData> LoggerData = (List<EventLoggerData>) null;
    private bool breakLoop = false;
    private string lastLoggerState;
    internal StartupLib.GmmCorporateControl gmmCorporateControl1;
    internal Button ButtonTestWindow;
    internal Button Button_GetVersion;
    internal Button Button_ReadDeviceLoop;
    internal Button Button_Display;
    internal Button Button_NFC_Test;
    internal Button Button_IUW_Test;
    internal Button Button_CommunicationTest;
    internal Button ButtonRunCurrentSelftest;
    internal Button ButtonShowFirmwareTests;
    internal Button ButtonShowUsedHandlerVariables;
    internal Button ButtonRadioOffTimes;
    internal Button ButtonDeleteAllSmartFunctions;
    internal Button ButtonSetAllSmartFunctions;
    internal ComboBox ComboBoxNdcModuleState;
    internal CheckBox CheckBoxLoopCommand;
    internal TextBlock TextBlockLoops;
    internal Button ButtonWriteAndReset;
    internal Button ButtonWriteAndSleep;
    internal Button ButtonReInitMeasurement;
    internal Button Button_TdcInternals;
    internal Button Button_LoadDefaultCalibration;
    internal TextBox TextBoxSimulatedVolume;
    internal Button ButtonSetSimulatedVolume;
    internal Button ButtonShowMonthLogger;
    internal Button ButtonClearMonthLogger;
    internal TextBox TextBoxSimMonthVolInc;
    internal TextBox TextBoxSimMonthDateTime;
    internal Button ButtonFillMonthLogger;
    internal Button ButtonAddValueToMonthLogger;
    internal Button ButtonShowCommandProtection;
    internal Button ButtonShowEventLogger;
    internal Button ButtonReadAndShowEventLoggerFromMemory;
    internal Button ButtonShowEventLoggerFromMemory;
    internal Button ButtonShowEventLoggerState;
    internal Button ButtonShowEventLoggerStateDiff;
    internal Button ButtonClearEventLogger;
    internal TextBox TextBoxNewEventTime;
    internal TextBox TextBoxNumberOfEvents;
    internal Button ButtonAddRamdomEvent;
    internal ComboBox ComboBoxEvents;
    internal TextBox TextBoxEventParam;
    internal Button ButtonAddSelectedEvent;
    internal Button Button_AddNewMap;
    internal Button Button_HardwareTypeManagerIUW;
    internal Button Button_HardwareTypeManagerMiCon;
    internal Button Button_HardwareTypeManagerMinoConnect;
    internal Button Button_HardwareTypeManagerNDC;
    private bool _contentLoaded;

    public S4_TestWindows2(S4_HandlerWindowFunctions myWindowFunctions)
    {
      this.myWindowFunctions = myWindowFunctions;
      this.myFunctions = myWindowFunctions.MyFunctions;
      this.myCommands = this.myFunctions.myDeviceCommands;
      this.cancelTokenSource = new CancellationTokenSource();
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.InitializeComponent();
      this.ComboBoxEvents.ItemsSource = (IEnumerable) Enum.GetNames(typeof (LoggerEventTypes));
      this.ComboBoxEvents.SelectedIndex = 0;
      if (this.myFunctions.myMeters.WorkMeter != null && this.myFunctions.myMeters.WorkMeter.meterMemory != null)
      {
        S4_DeviceMemory meterMemory = this.myFunctions.myMeters.checkedWorkMeter.meterMemory;
        if (meterMemory.IsParameterAvailable(S4_Params.devFlowSimulation))
        {
          float parameterValue = meterMemory.GetParameterValue<float>(S4_Params.devFlowSimulation);
          if ((double) parameterValue != double.NaN)
            this.TextBoxSimulatedVolume.Text = parameterValue.ToString();
        }
      }
      this.TextBoxNewEventTime.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
      this.AddRandomEventButtonText = this.ButtonAddRamdomEvent.Content.ToString();
      this.AddSelectedEventButtonText = this.ButtonAddSelectedEvent.Content.ToString();
    }

    private void OnProgress(ProgressArg obj)
    {
      if (this.CheckAccess())
        return;
      this.Dispatcher.Invoke((Action) (() => this.OnProgress(obj)));
    }

    private void SetRunState()
    {
      this.cancelTokenSource = new CancellationTokenSource();
      this.progress.Reset();
      this.progress.Split(new double[2]{ 2.0, 98.0 });
      this.progress.Report("Run");
      this.defaultCursor = this.Cursor;
      this.Cursor = Cursors.Wait;
    }

    private void SetStopState()
    {
      this.Cursor = this.defaultCursor;
      this.progress.Reset();
    }

    private void ButtonAddNewMap_Click(object sender, RoutedEventArgs e)
    {
      this.myWindowFunctions.OpenMapClassManagerToReadFromMapFile();
    }

    private void Button_HardwareTypeEditor_Click(object sender, RoutedEventArgs e)
    {
      uint? firmwareVersion = new uint?();
      uint? hardwareType = new uint?();
      bool isChangingAllowed = false;
      try
      {
        string[] HardwareNames = new string[4]
        {
          "IUW",
          "NFC_MiCon_Connector",
          "SENSUSConnector",
          "MinoConnect"
        };
        if (((FrameworkElement) sender).Name.Contains("IUW"))
        {
          HardwareNames = new string[1]{ "IUW" };
          if (this.myFunctions.myMeters.WorkMeter != null)
          {
            firmwareVersion = new uint?(this.myFunctions.myMeters.WorkMeter.meterMemory.FirmwareVersion);
            hardwareType = this.myFunctions.myMeters.WorkMeter.deviceIdentification.HardwareID;
          }
        }
        else if (((FrameworkElement) sender).Name.Contains("MiCon"))
        {
          HardwareNames = new string[1]
          {
            "NFC_MiCon_Connector"
          };
          isChangingAllowed = true;
        }
        else if (((FrameworkElement) sender).Name.Contains("NDC"))
        {
          HardwareNames = new string[1]{ "SENSUSConnector" };
          isChangingAllowed = true;
        }
        else if (((FrameworkElement) sender).Name.Contains("MinoConnect"))
        {
          HardwareNames = new string[1]{ "MinoConnect" };
          isChangingAllowed = true;
        }
        new HardwareTypeEditor(HardwareNames, firmwareVersion, hardwareType, isChangingAllowed).ShowDialog();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "HardwareTypeEditor error");
      }
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (sender == this.Button_GetVersion)
          this.myWindowFunctions.ShowTestConnectionWindow();
        if (sender == this.Button_ReadDeviceLoop)
          new S4_ReadDeviceLoop(this.myFunctions).Show();
        else if (sender == this.Button_Display)
          new S4_TestDisplay(this.myWindowFunctions).Show();
        else if (sender == this.Button_NFC_Test)
        {
          NFC_Test window = new NFC_Test(this.myFunctions.myPort);
          window.Show();
          window = (NFC_Test) null;
        }
        else if (sender == this.Button_TdcInternals)
        {
          if (this.myFunctions.myMeters.WorkMeter == null)
          {
            if (this.myFunctions.myMeters.ConnectedMeter == null)
              throw new Exception("Meter objects not available");
            this.myFunctions.myMeters.WorkMeter = this.myFunctions.myMeters.ConnectedMeter.Clone();
          }
          S4_TDCinternals window = new S4_TDCinternals(this.myWindowFunctions);
          window.ShowDialog();
          window = (S4_TDCinternals) null;
        }
        else if (sender == this.Button_LoadDefaultCalibration)
        {
          S4_TDC_Calibration theCalibration = new S4_TDC_Calibration();
          theCalibration.CopyCalibrationToMemory(this.myFunctions.myMeters.checkedWorkMeter.meterMemory);
          int num = (int) MessageBox.Show("Default calibration loaded");
          theCalibration = (S4_TDC_Calibration) null;
        }
        else if (sender == this.Button_IUW_Test)
        {
          S4_IUW_TestModes window = new S4_IUW_TestModes(this.myWindowFunctions);
          window.Show();
          window = (S4_IUW_TestModes) null;
        }
        else if (sender == this.Button_CommunicationTest)
        {
          S4_CommunicationTest window = new S4_CommunicationTest(this.myFunctions);
          window.Show();
          window = (S4_CommunicationTest) null;
        }
        else if (sender == this.ButtonWriteAndReset)
        {
          int loopCounter = 0;
          do
          {
            this.TextBlockLoops.Text = loopCounter.ToString();
            ++loopCounter;
            await this.myFunctions.WriteDeviceAsync(this.progress, this.cancelTokenSource.Token);
            await this.RandomDelay(500);
            await this.myFunctions.ResetDeviceAsync(this.progress, this.cancelTokenSource.Token);
          }
          while (this.CheckBoxLoopCommand.IsChecked.Value);
        }
        else if (sender == this.ButtonWriteAndSleep)
        {
          int loopCounter = 0;
          do
          {
            this.TextBlockLoops.Text = loopCounter.ToString();
            ++loopCounter;
            await this.myFunctions.WriteDeviceAsync(this.progress, this.cancelTokenSource.Token);
            await this.RandomDelay(500);
            await this.myFunctions.SetModeAsync(this.progress, this.cancelTokenSource.Token, (Enum) HandlerFunctionsForProduction.CommonDeviceModes.DeliveryMode, (byte) 0, (ushort) 10, (ushort) 0, 305419896U, (ushort) 37331, (byte[]) null);
            await Task.Delay(3000);
            await this.myFunctions.SetModeAsync(this.progress, this.cancelTokenSource.Token, (Enum) HandlerFunctionsForProduction.CommonDeviceModes.OperationMode, (byte) 0, (ushort) 10, (ushort) 0, 305419896U, (ushort) 37331, (byte[]) null);
            await Task.Delay(9000);
          }
          while (this.CheckBoxLoopCommand.IsChecked.Value);
        }
        else if (sender == this.ButtonReInitMeasurement)
        {
          int loopCounter = 0;
          while (true)
          {
            this.TextBlockLoops.Text = loopCounter.ToString();
            ++loopCounter;
            await this.myFunctions.ReInitMeasurementAsync(S4_HandlerFunctions.MeasurementParts.All, this.progress, this.cancelTokenSource.Token);
            if (this.CheckBoxLoopCommand.IsChecked.Value)
              await Task.Delay(2000);
            else
              break;
          }
        }
        else if (sender == this.ButtonShowCommandProtection)
        {
          S4_ProtectionManager protectionManager = new S4_ProtectionManager(this.myFunctions.myMeters.checkedWorkMeter.meterMemory, this.myFunctions.checkedNfcCommands);
          await protectionManager.ReadCommandProtectionAsync(this.progress, this.cancelTokenSource.Token);
          string info = protectionManager.GetCommandProtectionText();
          GmmMessage.Show_Ok(info, "Command protection", true);
          protectionManager = (S4_ProtectionManager) null;
          info = (string) null;
        }
        else
        {
          if (sender == this.ButtonRunCurrentSelftest)
            return;
          if (sender != this.ButtonShowFirmwareTests)
            return;
          try
          {
            S4_FirmwareTests theWindow = new S4_FirmwareTests(this.myWindowFunctions);
            theWindow.Owner = (Window) this;
            theWindow.ShowDialog();
            theWindow = (S4_FirmwareTests) null;
          }
          catch (Exception ex)
          {
            ExceptionViewer.Show(ex);
          }
        }
      }
      catch (TaskCanceledException ex)
      {
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private async Task RandomDelay(int maxMilliSecondes)
    {
      if (this.MyRandom == null)
        this.MyRandom = new Random(DateTime.Now.Millisecond);
      await Task.Delay(this.MyRandom.Next(maxMilliSecondes));
    }

    private async void ButtonSetSimulatedVolume_Click(object sender, RoutedEventArgs e)
    {
      this.SetRunState();
      try
      {
        if (sender != this.ButtonSetSimulatedVolume)
          return;
        float simulatedVolume;
        if (!float.TryParse(this.TextBoxSimulatedVolume.Text, out simulatedVolume))
          throw new Exception("Illegal volume simulation value. (not a float)");
        S4_DeviceMemory mem = this.myFunctions.myMeters.checkedConnectedMeter.meterMemory;
        if (!mem.IsParameterInMap(S4_Params.devFlowSimulation))
          throw new Exception("Volume simulation not supported in this firmware");
        if (this.myFunctions.myMeters.WorkMeter != null && this.myFunctions.myMeters.WorkMeter.meterMemory != null && this.myFunctions.myMeters.WorkMeter.meterMemory.IsParameterAvailable(S4_Params.devFlowSimulation))
        {
          S4_DeviceMemory mem_work = this.myFunctions.myMeters.WorkMeter.meterMemory;
          mem_work.SetParameterValue<float>(S4_Params.devFlowSimulation, simulatedVolume);
          await this.myCommands.CommonNfcCommands.WriteMemoryAsync(mem_work.GetParameterAddressRange(S4_Params.devFlowSimulation), (DeviceMemory) mem_work, this.progress, this.cancelTokenSource.Token);
          mem_work = (S4_DeviceMemory) null;
        }
        else
          await this.myCommands.CommonNfcCommands.WriteMemoryAsync(this.progress, this.cancelTokenSource.Token, mem.GetParameterAddress(S4_Params.devFlowSimulation), BitConverter.GetBytes(simulatedVolume));
        mem = (S4_DeviceMemory) null;
      }
      catch (TimeoutException ex)
      {
        ExceptionViewer.Show((Exception) ex);
      }
      catch (HandlerMessageException ex)
      {
        ExceptionViewer.Show((Exception) ex);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      finally
      {
        this.SetStopState();
      }
    }

    private void ButtonShowUsedHandlerVariables_Click(object sender, RoutedEventArgs e)
    {
      this.myFunctions.myMeters.WorkMeter.meterMemory.ShowUsedParameterRanges();
    }

    private void ButtonTimeElapseSimulator_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        new S4_TimeLapseSimulator(this.myWindowFunctions).Show();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private async void ButtonFillMonthLogger_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        S4_CurrentData currentData = await this.myFunctions.ReadCurrentDataAsync(this.progress, this.cancelTokenSource.Token);
        double volumeIncrement;
        if (!double.TryParse(this.TextBoxSimMonthVolInc.Text, out volumeIncrement))
          throw new Exception("Illegel volume increment number");
        DateTime setTime;
        if (!DateTime.TryParse(this.TextBoxSimMonthDateTime.Text, out setTime))
          setTime = currentData.DeviceTime;
        await S4_LoggerManager.ReadMonthLoggerMemory(this.myFunctions.myMeters.checkedWorkMeter.meterMemory, this.myFunctions.checkedNfcCommands, this.progress, this.cancelTokenSource.Token);
        S4_LoggerManager.FillMonthLoggerMemory(this.myFunctions.myMeters.checkedWorkMeter.meterMemory, setTime, currentData.Volume, volumeIncrement);
        await S4_LoggerManager.WriteMonthLoggerMemory(this.myFunctions.myMeters.checkedWorkMeter.meterMemory, this.myFunctions.checkedNfcCommands, this.progress, this.cancelTokenSource.Token);
        string loggerData = S4_LoggerManager.GetMonthLoggerDataTextFromMemory(this.myFunctions.myMeters.checkedWorkMeter.meterMemory, currentData.Units);
        GmmMessage.Show_Ok(loggerData, "Month logger is filled");
        currentData = (S4_CurrentData) null;
        loggerData = (string) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private async void ButtonClearMonthLogger_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        await S4_LoggerManager.ReadMonthLoggerMemory(this.myFunctions.myMeters.checkedWorkMeter.meterMemory, this.myFunctions.checkedNfcCommands, this.progress, this.cancelTokenSource.Token);
        S4_LoggerManager.ClearMonthLoggerInMemory(this.myFunctions.myMeters.checkedWorkMeter.meterMemory);
        await S4_LoggerManager.WriteMonthLoggerMemory(this.myFunctions.myMeters.checkedWorkMeter.meterMemory, this.myFunctions.checkedNfcCommands, this.progress, this.cancelTokenSource.Token);
        GmmMessage.Show_Ok("Month logger cleared");
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private async void ButtonAddValueToMonthLogger_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        S4_CurrentData currentData = await this.myFunctions.ReadCurrentDataAsync(this.progress, this.cancelTokenSource.Token);
        await S4_LoggerManager.ReadMonthLoggerMemory(this.myFunctions.myMeters.checkedWorkMeter.meterMemory, this.myFunctions.checkedNfcCommands, this.progress, this.cancelTokenSource.Token);
        List<VolumeLoggerData> loggerDataList = S4_LoggerManager.GetMonthLoggerDataListFromMemory(this.myFunctions.myMeters.checkedWorkMeter.meterMemory, currentData.Units);
        double volumeIncrement;
        if (!double.TryParse(this.TextBoxSimMonthVolInc.Text, out volumeIncrement))
          throw new Exception("Illegel volume increment number");
        DateTime? nextTimeStamp = new DateTime?();
        DateTime setTime;
        if (DateTime.TryParse(this.TextBoxSimMonthDateTime.Text, out setTime))
          nextTimeStamp = new DateTime?(setTime);
        double? nextVolume = new double?();
        if (loggerDataList.Count > 0)
        {
          double lastVolume = loggerDataList[0].Volume;
          DateTime lastTime = loggerDataList[0].LoggerTime;
          nextVolume = new double?(lastVolume + volumeIncrement);
          if (!nextTimeStamp.HasValue)
            nextTimeStamp = new DateTime?(S4_LoggerManager.GetNextMonthTime(lastTime));
        }
        else
        {
          if (!nextTimeStamp.HasValue)
            nextTimeStamp = new DateTime?(S4_LoggerManager.GetNextMonthTime(currentData.DeviceTime));
          nextVolume = new double?(volumeIncrement);
        }
        S4_LoggerManager.AddMonthLoggerEntryToMemory(this.myFunctions.myMeters.checkedWorkMeter.meterMemory, nextTimeStamp.Value, nextVolume.Value);
        loggerDataList = S4_LoggerManager.GetMonthLoggerDataListFromMemory(this.myFunctions.myMeters.checkedWorkMeter.meterMemory, currentData.Units);
        await S4_LoggerManager.WriteMonthLoggerMemory(this.myFunctions.myMeters.checkedWorkMeter.meterMemory, this.myFunctions.checkedNfcCommands, this.progress, this.cancelTokenSource.Token);
        nextTimeStamp = new DateTime?(S4_LoggerManager.GetNextMonthTime(nextTimeStamp.Value));
        this.TextBoxSimMonthDateTime.Text = nextTimeStamp.Value.ToString("dd.MM.yyyy HH:mm:ss");
        string loggerData = S4_LoggerManager.GetMonthLoggerDataTextFromMemory(this.myFunctions.myMeters.checkedWorkMeter.meterMemory, currentData.Units);
        GmmMessage.Show_Ok(loggerData, "Month logger value added");
        currentData = (S4_CurrentData) null;
        loggerDataList = (List<VolumeLoggerData>) null;
        nextTimeStamp = new DateTime?();
        nextVolume = new double?();
        loggerData = (string) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonShowMonthLogger_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        GmmMessage.Show_Ok(S4_LoggerManager.GetMonthLoggerDataTextFromMemory(this.myFunctions.myMeters.checkedWorkMeter.meterMemory, new S4_BaseUnitSupport(S4_MenuManager.GetBaseUnit((_UNIT_SCALE_) this.myFunctions.myMeters.checkedWorkMeter.meterMemory.GetParameterValue<ushort>(S4_Params.DeviceDefaultUnitScale)))), "Month logger data");
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private async void ButtonShowEventLogger_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        IEnumerable enumerable = await this.myFunctions.ReadLoggerDataAsync(LoggerNames.Event.ToString(), this.progress, this.cancelTokenSource.Token);
        this.LoggerData = (List<EventLoggerData>) enumerable;
        enumerable = (IEnumerable) null;
        string eventLoggerText = S4_LoggerManager.GetEventLoggerTextFromList(this.LoggerData);
        GmmMessage.Show_Ok(eventLoggerText, "Event logger data", true);
        eventLoggerText = (string) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
        this.LoggerData = (List<EventLoggerData>) null;
      }
    }

    private async void ButtonReadAndShowEventLoggerFromMemory_Click(
      object sender,
      RoutedEventArgs e)
    {
      try
      {
        S4_DeviceMemory memory = this.myFunctions.myMeters.checkedWorkMeter.meterMemory;
        await S4_LoggerManager.ReadEventLoggerMemory(memory, this.myFunctions.checkedNfcCommands, this.progress, this.cancelTokenSource.Token);
        this.LoggerData = S4_LoggerManager.GetEventLoggerDataListFromMemory(memory);
        string eventLoggerText = S4_LoggerManager.GetEventLoggerTextFromList(this.LoggerData);
        GmmMessage.Show_Ok(eventLoggerText, "Event logger data", true);
        memory = (S4_DeviceMemory) null;
        eventLoggerText = (string) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
        this.LoggerData = (List<EventLoggerData>) null;
      }
    }

    private void ButtonShowEventLoggerFromMemory_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.LoggerData = S4_LoggerManager.GetEventLoggerDataListFromMemory(this.myFunctions.myMeters.checkedWorkMeter.meterMemory);
        GmmMessage.Show_Ok(S4_LoggerManager.GetEventLoggerTextFromList(this.LoggerData), "Event logger data", true);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
        this.LoggerData = (List<EventLoggerData>) null;
      }
    }

    private async void ButtonClearEventLogger_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        S4_DeviceMemory deviceMemory = this.myFunctions.myMeters.checkedWorkMeter.meterMemory;
        await S4_LoggerManager.ReadEventLoggerMemory(deviceMemory, this.myFunctions.checkedNfcCommands, this.progress, this.cancelTokenSource.Token);
        S4_LoggerManager.ClearEventLoggerMemory(deviceMemory);
        await S4_LoggerManager.WriteEventLoggerMemory(deviceMemory, this.myFunctions.checkedNfcCommands, this.progress, this.cancelTokenSource.Token);
        await S4_LoggerManager.AddEventAsync(DateTime.Now, LoggerEventTypes.NotDefined, (byte) 0, this.myFunctions.checkedNfcCommands, this.progress, this.cancelTokenSource.Token);
        GmmMessage.Show_Ok("Event logger cleard", "Event logger");
        deviceMemory = (S4_DeviceMemory) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
        this.LoggerData = (List<EventLoggerData>) null;
      }
    }

    private async void ButtonAddRamdomEvent_Click(object sender, RoutedEventArgs e)
    {
      if (this.ButtonAddRamdomEvent.Content.ToString() != this.AddRandomEventButtonText)
      {
        this.breakLoop = true;
      }
      else
      {
        try
        {
          this.ButtonAddRamdomEvent.Content = (object) "Break";
          DateTime eventTime;
          if (!DateTime.TryParse(this.TextBoxNewEventTime.Text, out eventTime))
            throw new Exception("Illegal event time.");
          int numberOfEvents;
          if (!int.TryParse(this.TextBoxNumberOfEvents.Text, out numberOfEvents))
            throw new Exception("Illegal number of events.");
          S4_HandlerFunctions functions1 = this.myFunctions;
          LoggerNames loggerNames = LoggerNames.Event;
          string LoggerName1 = loggerNames.ToString();
          ProgressHandler progress1 = this.progress;
          CancellationToken token1 = this.cancelTokenSource.Token;
          IEnumerable enumerable1 = await functions1.ReadLoggerDataAsync(LoggerName1, progress1, token1);
          this.LoggerData = (List<EventLoggerData>) enumerable1;
          enumerable1 = (IEnumerable) null;
          for (int i = 0; i < numberOfEvents && !this.breakLoop; ++i)
          {
            LoggerEventTypes randomEvent = this.myFunctions.myLoggerManager.GetRandomEvent();
            byte randomEventParam = this.myFunctions.myLoggerManager.GetRandomEventParam();
            await S4_LoggerManager.AddEventAsync(eventTime, randomEvent, randomEventParam, this.myFunctions.checkedNfcCommands, this.progress, this.cancelTokenSource.Token);
            eventTime = eventTime.AddSeconds(1.0);
          }
          this.TextBoxNewEventTime.Text = eventTime.ToString("dd.MM.yyyy HH:mm:ss");
          S4_HandlerFunctions functions2 = this.myFunctions;
          loggerNames = LoggerNames.Event;
          string LoggerName2 = loggerNames.ToString();
          ProgressHandler progress2 = this.progress;
          CancellationToken token2 = this.cancelTokenSource.Token;
          IEnumerable enumerable2 = await functions2.ReadLoggerDataAsync(LoggerName2, progress2, token2);
          List<EventLoggerData> afterLoggerData = (List<EventLoggerData>) enumerable2;
          enumerable2 = (IEnumerable) null;
          string eventLoggerChangeText = S4_LoggerManager.GetEventLoggerChangeTextFromLists(this.LoggerData, afterLoggerData);
          GmmMessage.Show_Ok(eventLoggerChangeText, "Events added", true);
          afterLoggerData = (List<EventLoggerData>) null;
          eventLoggerChangeText = (string) null;
        }
        catch (Exception ex)
        {
          ExceptionViewer.Show(ex);
          this.LoggerData = (List<EventLoggerData>) null;
        }
        finally
        {
          this.breakLoop = false;
          this.ButtonAddRamdomEvent.Content = (object) this.AddRandomEventButtonText;
        }
      }
    }

    private async void ButtonAddSelectedEvent_Click(object sender, RoutedEventArgs e)
    {
      if (this.ButtonAddSelectedEvent.Content.ToString() != this.AddSelectedEventButtonText)
      {
        this.breakLoop = true;
      }
      else
      {
        try
        {
          this.ButtonAddSelectedEvent.Content = (object) "Break";
          DateTime eventTime;
          if (!DateTime.TryParse(this.TextBoxNewEventTime.Text, out eventTime))
            throw new Exception("Illegal event time");
          LoggerEventTypes theEvent;
          if (!Enum.TryParse<LoggerEventTypes>(this.ComboBoxEvents.SelectedItem.ToString(), out theEvent))
            throw new Exception("Illegal event type");
          byte eventParam;
          if (!byte.TryParse(this.TextBoxEventParam.Text, out eventParam))
            throw new Exception("Illegal event parameter");
          int numberOfEvents;
          if (!int.TryParse(this.TextBoxNumberOfEvents.Text, out numberOfEvents))
            throw new Exception("Illegal number of events.");
          S4_HandlerFunctions functions1 = this.myFunctions;
          LoggerNames loggerNames = LoggerNames.Event;
          string LoggerName1 = loggerNames.ToString();
          ProgressHandler progress1 = this.progress;
          CancellationToken token1 = this.cancelTokenSource.Token;
          IEnumerable enumerable1 = await functions1.ReadLoggerDataAsync(LoggerName1, progress1, token1);
          this.LoggerData = (List<EventLoggerData>) enumerable1;
          enumerable1 = (IEnumerable) null;
          for (int i = 0; i < numberOfEvents && !this.breakLoop; ++i)
          {
            await S4_LoggerManager.AddEventAsync(eventTime, theEvent, eventParam, this.myFunctions.checkedNfcCommands, this.progress, this.cancelTokenSource.Token);
            eventTime = eventTime.AddSeconds(1.0);
          }
          this.TextBoxNewEventTime.Text = eventTime.ToString("dd.MM.yyyy HH:mm:ss");
          S4_HandlerFunctions functions2 = this.myFunctions;
          loggerNames = LoggerNames.Event;
          string LoggerName2 = loggerNames.ToString();
          ProgressHandler progress2 = this.progress;
          CancellationToken token2 = this.cancelTokenSource.Token;
          IEnumerable enumerable2 = await functions2.ReadLoggerDataAsync(LoggerName2, progress2, token2);
          List<EventLoggerData> afterLoggerData = (List<EventLoggerData>) enumerable2;
          enumerable2 = (IEnumerable) null;
          string eventLoggerChangeText = S4_LoggerManager.GetEventLoggerChangeTextFromLists(this.LoggerData, afterLoggerData);
          GmmMessage.Show_Ok(eventLoggerChangeText, "Events added", true);
          afterLoggerData = (List<EventLoggerData>) null;
          eventLoggerChangeText = (string) null;
        }
        catch (Exception ex)
        {
          ExceptionViewer.Show(ex);
          this.LoggerData = (List<EventLoggerData>) null;
        }
        finally
        {
          this.breakLoop = false;
          this.ButtonAddSelectedEvent.Content = (object) this.AddSelectedEventButtonText;
        }
      }
    }

    private async Task AddEventAsync(
      DateTime eventTime,
      LoggerEventTypes theEvent,
      byte eventParam)
    {
      try
      {
        LoggerNames loggerNames;
        if (this.LoggerData == null)
        {
          S4_HandlerFunctions functions = this.myFunctions;
          loggerNames = LoggerNames.Event;
          string LoggerName = loggerNames.ToString();
          ProgressHandler progress = this.progress;
          CancellationToken token = this.cancelTokenSource.Token;
          IEnumerable enumerable = await functions.ReadLoggerDataAsync(LoggerName, progress, token);
          this.LoggerData = (List<EventLoggerData>) enumerable;
          enumerable = (IEnumerable) null;
        }
        await S4_LoggerManager.AddEventAsync(eventTime, theEvent, eventParam, this.myFunctions.checkedNfcCommands, this.progress, this.cancelTokenSource.Token);
        S4_HandlerFunctions functions1 = this.myFunctions;
        loggerNames = LoggerNames.Event;
        string LoggerName1 = loggerNames.ToString();
        ProgressHandler progress1 = this.progress;
        CancellationToken token1 = this.cancelTokenSource.Token;
        IEnumerable enumerable1 = await functions1.ReadLoggerDataAsync(LoggerName1, progress1, token1);
        List<EventLoggerData> afterLoggerData = (List<EventLoggerData>) enumerable1;
        enumerable1 = (IEnumerable) null;
        string eventLoggerChangeText = S4_LoggerManager.GetEventLoggerChangeTextFromLists(this.LoggerData, afterLoggerData);
        this.LoggerData = afterLoggerData;
        afterLoggerData = (List<EventLoggerData>) null;
        eventLoggerChangeText = (string) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
        this.LoggerData = (List<EventLoggerData>) null;
      }
    }

    private async void ButtonShowEventLoggerState_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        S4_DeviceMemory deviceMemory = this.myFunctions.myMeters.checkedWorkMeter.meterMemory;
        string theState = await S4_LoggerManager.ReadEventLoggerStateMemory(deviceMemory, this.myFunctions.checkedNfcCommands, this.progress, this.cancelTokenSource.Token);
        GmmMessage.Show(theState, "Event logger state");
        this.ButtonShowEventLoggerStateDiff.IsEnabled = true;
        this.lastLoggerState = theState;
        deviceMemory = (S4_DeviceMemory) null;
        theState = (string) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
        this.LoggerData = (List<EventLoggerData>) null;
      }
    }

    private async void ButtonShowEventLoggerStateDiff_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        S4_DeviceMemory deviceMemory = this.myFunctions.myMeters.checkedWorkMeter.meterMemory;
        string theState = await S4_LoggerManager.ReadEventLoggerStateMemory(deviceMemory, this.myFunctions.checkedNfcCommands, this.progress, this.cancelTokenSource.Token);
        string filePathBefore = this.WriteEventStateLogFile("EventLogBefore", this.lastLoggerState);
        string filePathNow = this.WriteEventStateLogFile("EventLogNow", theState);
        Process myProcess = new Process();
        myProcess.StartInfo.FileName = "TortoiseMerge";
        myProcess.StartInfo.Arguments = "/base:\"" + filePathBefore + "\" /theirs:\"" + filePathNow + "\"";
        myProcess.Start();
        this.lastLoggerState = theState;
        deviceMemory = (S4_DeviceMemory) null;
        theState = (string) null;
        filePathBefore = (string) null;
        filePathNow = (string) null;
        myProcess = (Process) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
        this.LoggerData = (List<EventLoggerData>) null;
      }
    }

    public string WriteEventStateLogFile(string fileTitel, string fileContent)
    {
      string path2 = DateTime.Now.ToString("yyyyMMdd_HHmmss_FFFF_") + fileTitel + ".txt";
      string path = Path.Combine(SystemValues.LoggDataPath, path2);
      using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.ASCII))
      {
        streamWriter.Write(fileContent);
        streamWriter.Flush();
        streamWriter.Close();
      }
      return path;
    }

    private void ButtonTestWindow_Click(object sender, RoutedEventArgs e)
    {
      new S4_UltrasonicWindows(this.myWindowFunctions).ShowDialog();
    }

    private void ButtonRadioOffTimes_Click(object sender, RoutedEventArgs e)
    {
      new RadioOffTimes().ShowDialog();
    }

    private async void ComboBoxNdcModuleState_SelectionChanged(
      object sender,
      SelectionChangedEventArgs e)
    {
      try
      {
        int indexAndState = this.ComboBoxNdcModuleState.SelectedIndex;
        if (indexAndState < 0)
          return;
        byte[] data = new byte[1]{ (byte) indexAndState };
        byte[] resultAsync = await this.myFunctions.checkedNfcCommands.SendCommandAndGetResultAsync(this.progress, this.cancelTokenSource.Token, NfcCommands.SetNDC_ModuleState, data);
        GmmMessage.Show_Ok("NDC_ModuleState changed");
        data = (byte[]) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "NDC_ModuleState change error");
      }
    }

    private async void ButtonDeleteAllSmartFunctions_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        await this.myFunctions.DeleteAllSmartFunctionsAsync(this.progress, this.cancelTokenSource.Token);
        GmmMessage.Show_Ok("All smart functions deleted");
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "DeleteAllSmartFunctions error");
      }
    }

    private void ButtonSetAllSmartFunctions_Click(object sender, RoutedEventArgs e)
    {
      string[] strArray = new string[12]
      {
        "Backflow",
        "BackflowLog",
        "Burst",
        "Dry",
        "Frost",
        "Leakage",
        "NoConsumption",
        "Oversized",
        "ReverseInstallation",
        "VolAnalyseLog",
        "DailyVolumeLog",
        "Undersized"
      };
      try
      {
        this.myFunctions.SetConfigurationParameters(new SortedList<OverrideID, ConfigurationParameter>()
        {
          {
            OverrideID.SmartFunctions,
            new ConfigurationParameter(OverrideID.SmartFunctions)
            {
              ParameterValue = (object) strArray
            }
          }
        }, 0);
        GmmMessage.Show_Ok("All smart functions activated by SetConfigurationParameters. Write required.");
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "SetAllSmartFunctions error");
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/S4_Handler;component/userinterface/s4_testwindows2.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.gmmCorporateControl1 = (StartupLib.GmmCorporateControl) target;
          break;
        case 2:
          this.ButtonTestWindow = (Button) target;
          this.ButtonTestWindow.Click += new RoutedEventHandler(this.ButtonTestWindow_Click);
          break;
        case 3:
          this.Button_GetVersion = (Button) target;
          this.Button_GetVersion.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 4:
          this.Button_ReadDeviceLoop = (Button) target;
          this.Button_ReadDeviceLoop.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 5:
          this.Button_Display = (Button) target;
          this.Button_Display.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 6:
          this.Button_NFC_Test = (Button) target;
          this.Button_NFC_Test.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 7:
          this.Button_IUW_Test = (Button) target;
          this.Button_IUW_Test.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 8:
          this.Button_CommunicationTest = (Button) target;
          this.Button_CommunicationTest.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 9:
          this.ButtonRunCurrentSelftest = (Button) target;
          this.ButtonRunCurrentSelftest.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 10:
          this.ButtonShowFirmwareTests = (Button) target;
          this.ButtonShowFirmwareTests.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 11:
          this.ButtonShowUsedHandlerVariables = (Button) target;
          this.ButtonShowUsedHandlerVariables.Click += new RoutedEventHandler(this.ButtonShowUsedHandlerVariables_Click);
          break;
        case 12:
          this.ButtonRadioOffTimes = (Button) target;
          this.ButtonRadioOffTimes.Click += new RoutedEventHandler(this.ButtonRadioOffTimes_Click);
          break;
        case 13:
          this.ButtonDeleteAllSmartFunctions = (Button) target;
          this.ButtonDeleteAllSmartFunctions.Click += new RoutedEventHandler(this.ButtonDeleteAllSmartFunctions_Click);
          break;
        case 14:
          this.ButtonSetAllSmartFunctions = (Button) target;
          this.ButtonSetAllSmartFunctions.Click += new RoutedEventHandler(this.ButtonSetAllSmartFunctions_Click);
          break;
        case 15:
          this.ComboBoxNdcModuleState = (ComboBox) target;
          this.ComboBoxNdcModuleState.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxNdcModuleState_SelectionChanged);
          break;
        case 16:
          this.CheckBoxLoopCommand = (CheckBox) target;
          break;
        case 17:
          this.TextBlockLoops = (TextBlock) target;
          break;
        case 18:
          this.ButtonWriteAndReset = (Button) target;
          this.ButtonWriteAndReset.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 19:
          this.ButtonWriteAndSleep = (Button) target;
          this.ButtonWriteAndSleep.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 20:
          this.ButtonReInitMeasurement = (Button) target;
          this.ButtonReInitMeasurement.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 21:
          this.Button_TdcInternals = (Button) target;
          this.Button_TdcInternals.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 22:
          this.Button_LoadDefaultCalibration = (Button) target;
          this.Button_LoadDefaultCalibration.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 23:
          this.TextBoxSimulatedVolume = (TextBox) target;
          break;
        case 24:
          this.ButtonSetSimulatedVolume = (Button) target;
          this.ButtonSetSimulatedVolume.Click += new RoutedEventHandler(this.ButtonSetSimulatedVolume_Click);
          break;
        case 25:
          this.ButtonShowMonthLogger = (Button) target;
          this.ButtonShowMonthLogger.Click += new RoutedEventHandler(this.ButtonShowMonthLogger_Click);
          break;
        case 26:
          this.ButtonClearMonthLogger = (Button) target;
          this.ButtonClearMonthLogger.Click += new RoutedEventHandler(this.ButtonClearMonthLogger_Click);
          break;
        case 27:
          this.TextBoxSimMonthVolInc = (TextBox) target;
          break;
        case 28:
          this.TextBoxSimMonthDateTime = (TextBox) target;
          break;
        case 29:
          this.ButtonFillMonthLogger = (Button) target;
          this.ButtonFillMonthLogger.Click += new RoutedEventHandler(this.ButtonFillMonthLogger_Click);
          break;
        case 30:
          this.ButtonAddValueToMonthLogger = (Button) target;
          this.ButtonAddValueToMonthLogger.Click += new RoutedEventHandler(this.ButtonAddValueToMonthLogger_Click);
          break;
        case 31:
          this.ButtonShowCommandProtection = (Button) target;
          this.ButtonShowCommandProtection.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 32:
          this.ButtonShowEventLogger = (Button) target;
          this.ButtonShowEventLogger.Click += new RoutedEventHandler(this.ButtonShowEventLogger_Click);
          break;
        case 33:
          this.ButtonReadAndShowEventLoggerFromMemory = (Button) target;
          this.ButtonReadAndShowEventLoggerFromMemory.Click += new RoutedEventHandler(this.ButtonReadAndShowEventLoggerFromMemory_Click);
          break;
        case 34:
          this.ButtonShowEventLoggerFromMemory = (Button) target;
          this.ButtonShowEventLoggerFromMemory.Click += new RoutedEventHandler(this.ButtonShowEventLoggerFromMemory_Click);
          break;
        case 35:
          this.ButtonShowEventLoggerState = (Button) target;
          this.ButtonShowEventLoggerState.Click += new RoutedEventHandler(this.ButtonShowEventLoggerState_Click);
          break;
        case 36:
          this.ButtonShowEventLoggerStateDiff = (Button) target;
          this.ButtonShowEventLoggerStateDiff.Click += new RoutedEventHandler(this.ButtonShowEventLoggerStateDiff_Click);
          break;
        case 37:
          this.ButtonClearEventLogger = (Button) target;
          this.ButtonClearEventLogger.Click += new RoutedEventHandler(this.ButtonClearEventLogger_Click);
          break;
        case 38:
          this.TextBoxNewEventTime = (TextBox) target;
          break;
        case 39:
          this.TextBoxNumberOfEvents = (TextBox) target;
          break;
        case 40:
          this.ButtonAddRamdomEvent = (Button) target;
          this.ButtonAddRamdomEvent.Click += new RoutedEventHandler(this.ButtonAddRamdomEvent_Click);
          break;
        case 41:
          this.ComboBoxEvents = (ComboBox) target;
          break;
        case 42:
          this.TextBoxEventParam = (TextBox) target;
          break;
        case 43:
          this.ButtonAddSelectedEvent = (Button) target;
          this.ButtonAddSelectedEvent.Click += new RoutedEventHandler(this.ButtonAddSelectedEvent_Click);
          break;
        case 44:
          this.Button_AddNewMap = (Button) target;
          this.Button_AddNewMap.Click += new RoutedEventHandler(this.ButtonAddNewMap_Click);
          break;
        case 45:
          this.Button_HardwareTypeManagerIUW = (Button) target;
          this.Button_HardwareTypeManagerIUW.Click += new RoutedEventHandler(this.Button_HardwareTypeEditor_Click);
          break;
        case 46:
          this.Button_HardwareTypeManagerMiCon = (Button) target;
          this.Button_HardwareTypeManagerMiCon.Click += new RoutedEventHandler(this.Button_HardwareTypeEditor_Click);
          break;
        case 47:
          this.Button_HardwareTypeManagerMinoConnect = (Button) target;
          this.Button_HardwareTypeManagerMinoConnect.Click += new RoutedEventHandler(this.Button_HardwareTypeEditor_Click);
          break;
        case 48:
          this.Button_HardwareTypeManagerNDC = (Button) target;
          this.Button_HardwareTypeManagerNDC.Click += new RoutedEventHandler(this.Button_HardwareTypeEditor_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
