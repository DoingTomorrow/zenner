// Decompiled with JetBrains decompiler
// Type: S4_Handler.UserInterface.S4_MapDataWindow
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommonWPF;
using HandlerLib;
using S4_Handler.Functions;
using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler.UserInterface
{
  public partial class S4_MapDataWindow : Window, IComponentConnector
  {
    private S4_HandlerWindowFunctions myWindowFunctions;
    private S4_HandlerFunctions myFunctions;
    private S4_DeviceCommandsNFC myCommands;
    private ProgressHandler progress;
    private Cursor defaultCursor;
    private CancellationTokenSource cancelTokenSource;
    internal StartupLib.GmmCorporateControl gmmCorporateControl1;
    internal TextBlock TextBlockStatus;
    internal ProgressBar ProgressBar1;
    internal Button ButtonBreak;
    internal Button ButtonShowDeviceDate;
    internal Button ButtonShowEventLoggerData;
    internal Button ButtonClearEventLoggerData;
    internal Button ButtonShowMonthLoggerData;
    internal Button ButtonClearMonthLoggerData;
    internal Button ButtonShowDiagnosticData;
    internal Button ButtonReadAndShowDiagnosticData;
    internal Button ButtonShowDiagnosticDataInternal;
    internal Button ButtonReadAndShowDiagnosticDataInternal;
    internal Button ButtonInitDiagnosticAndLoggers;
    internal Button ButtonSetDiagnosticAndLoggers;
    internal Button ButtonShowBackup;
    internal Button ButtonShowCalibrationsFromBackup;
    internal Button ButtonShowCalibrationsFromType;
    internal Button ButtonShowPCB_Assembly;
    internal TextBox TextBoxCommandResult;
    private bool _contentLoaded;

    public S4_MapDataWindow(S4_HandlerWindowFunctions myWindowFunctions)
    {
      this.myWindowFunctions = myWindowFunctions;
      this.myFunctions = myWindowFunctions.MyFunctions;
      this.myCommands = this.myFunctions.myDeviceCommands;
      this.InitializeComponent();
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      if (this.myFunctions.myMeters.BackupMeter == null)
        this.ButtonShowCalibrationsFromBackup.IsEnabled = false;
      if (this.myFunctions.myMeters.TypeMeter != null)
        return;
      this.ButtonShowCalibrationsFromType.IsEnabled = false;
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
        if (obj.Tag != null && obj.Tag.GetType() == typeof (string))
        {
          string tag = (string) obj.Tag;
          if (this.TextBoxCommandResult.Text.Length == 0)
            this.TextBoxCommandResult.AppendText(tag);
          else
            this.TextBoxCommandResult.AppendText(Environment.NewLine + tag);
          if (this.TextBoxCommandResult.Text.Length > 10000)
          {
            int start = this.TextBoxCommandResult.Text.IndexOf(Environment.NewLine);
            int num = this.TextBoxCommandResult.Text.IndexOf(Environment.NewLine, 1000);
            if (start > 0 && num > 0)
            {
              int length = num - start;
              this.TextBoxCommandResult.Select(start, length);
              this.TextBoxCommandResult.SelectedText = "";
            }
          }
          this.TextBoxCommandResult.ScrollToEnd();
        }
        else
          this.TextBlockStatus.Text = obj.Message;
      }
    }

    private void SetRunState()
    {
      this.cancelTokenSource = new CancellationTokenSource();
      this.progress.Reset();
      this.progress.Split(new double[2]{ 2.0, 98.0 });
      this.progress.Report("Run");
      this.ButtonBreak.IsEnabled = true;
      if (this.Cursor == Cursors.Wait)
        return;
      this.defaultCursor = this.Cursor;
      this.Cursor = Cursors.Wait;
    }

    private void SetStopState()
    {
      this.ButtonBreak.IsEnabled = false;
      this.Cursor = this.defaultCursor;
      this.progress.Reset();
      this.TextBlockStatus.Text = "";
      if (!UserManager.CheckPermission(UserManager.Right_ReadOnly))
        return;
      this.ButtonInitDiagnosticAndLoggers.IsEnabled = false;
      this.ButtonSetDiagnosticAndLoggers.IsEnabled = false;
    }

    private void ButtonBreak_Click(object sender, RoutedEventArgs e)
    {
      this.cancelTokenSource.Cancel();
    }

    private async void ButtonX_Click(object sender, RoutedEventArgs e)
    {
      this.SetRunState();
      try
      {
        if (sender == this.ButtonShowDeviceDate)
        {
          StringBuilder message = new StringBuilder();
          DateTimeOffset currentTime = DateTimeOffset.MinValue;
          if (this.myFunctions.myMeters.WorkMeter != null && this.myFunctions.myMeters.WorkMeter.meterMemory.IsParameterAvailable(S4_Params.sysDateTime))
          {
            DateTimeOffset mapTime = FirmwareDateTimeSupport.ToDateTimeOffsetBCD(this.myFunctions.myMeters.WorkMeter.meterMemory.GetData(S4_Params.sysDateTime));
            message.AppendLine("Device time from map:");
            message.Append("    ");
            message.AppendLine(mapTime.ToString());
            this.TextBoxCommandResult.Text = message.ToString();
            mapTime = new DateTimeOffset();
          }
          message = (StringBuilder) null;
          currentTime = new DateTimeOffset();
        }
        else if (sender == this.ButtonShowDiagnosticData)
        {
          S4_DiagnosticData diagData = new S4_DiagnosticData();
          diagData.LoadData(this.myFunctions.myMeters.WorkMeter.meterMemory);
          DeviceStateCounter stateConters = DeviceStateCounter.CreateObjectFromMemory((DeviceMemory) this.myFunctions.myMeters.WorkMeter.meterMemory, this.myFunctions.myMeters.WorkMeter.GetStateCounterRanges());
          this.TextBoxCommandResult.Text = diagData.ToString(DiagnosticEntry.ToStringUnits.InternalUnits) + Environment.NewLine + stateConters.ToTextBlock();
          diagData = (S4_DiagnosticData) null;
          stateConters = (DeviceStateCounter) null;
        }
        else if (sender == this.ButtonReadAndShowDiagnosticData)
        {
          S4_DiagnosticData diagData = new S4_DiagnosticData();
          await diagData.ReadAndLoadDataAsync(this.myFunctions.myMeters.WorkMeter.meterMemory, this.myFunctions.myDeviceCommands, this.progress, this.cancelTokenSource.Token);
          this.TextBoxCommandResult.Text = diagData.ToString();
          diagData = (S4_DiagnosticData) null;
        }
        else if (sender == this.ButtonShowDiagnosticDataInternal)
        {
          S4_DiagnosticData diagData = new S4_DiagnosticData();
          diagData.LoadData(this.myFunctions.myMeters.WorkMeter.meterMemory);
          DeviceStateCounter stateConters = DeviceStateCounter.CreateObjectFromMemory((DeviceMemory) this.myFunctions.myMeters.WorkMeter.meterMemory, this.myFunctions.myMeters.WorkMeter.GetStateCounterRanges());
          this.TextBoxCommandResult.Text = diagData.ToString(DiagnosticEntry.ToStringUnits.InternalUnits) + Environment.NewLine + stateConters.ToTextBlock();
          diagData = (S4_DiagnosticData) null;
          stateConters = (DeviceStateCounter) null;
        }
        else if (sender == this.ButtonReadAndShowDiagnosticDataInternal)
        {
          S4_DiagnosticData diagData = new S4_DiagnosticData();
          await diagData.ReadAndLoadDataAsync(this.myFunctions.myMeters.WorkMeter.meterMemory, this.myFunctions.myDeviceCommands, this.progress, this.cancelTokenSource.Token);
          this.TextBoxCommandResult.Text = diagData.ToString(DiagnosticEntry.ToStringUnits.InternalUnits);
          diagData = (S4_DiagnosticData) null;
        }
        else if (sender == this.ButtonInitDiagnosticAndLoggers)
        {
          this.myFunctions.myLoggerManager.Clear();
          this.myFunctions.myMeters.WorkMeter.InitMeterData();
          this.TextBoxCommandResult.Text = "Diagnostic and loggers initialised";
        }
        else if (sender == this.ButtonSetDiagnosticAndLoggers)
        {
          this.myFunctions.myLoggerManager.Clear();
          S4_DiagnosticData.InitDiagnosticData(this.myFunctions.myMeters.WorkMeter.meterMemory, new DateTime(2022, 2, 11, 10, 49, 12), 40000.0, true);
          S4_DiagnosticData.InitKeyDateData(this.myFunctions.myMeters.WorkMeter.meterMemory, new DateTime(2022, 4, 17), 2000.0, true);
          S4_LoggerManager.FillMonthLoggerMemory(this.myFunctions.myMeters.WorkMeter.meterMemory, new DateTime(2023, 1, 1), 60000.0, 10.0);
          this.TextBoxCommandResult.Text = "Diagnostic and loggers initialised with unique numbers";
        }
        else if (sender == this.ButtonShowEventLoggerData)
        {
          S4_DeviceMemory memory = this.myFunctions.myMeters.checkedWorkMeter.meterMemory;
          List<EventLoggerData> loggerData = S4_LoggerManager.GetEventLoggerDataListFromMemory(memory);
          this.TextBoxCommandResult.Text = S4_LoggerManager.GetEventLoggerTextFromList(loggerData);
          memory = (S4_DeviceMemory) null;
          loggerData = (List<EventLoggerData>) null;
        }
        else if (sender == this.ButtonClearEventLoggerData)
        {
          S4_DeviceMemory deviceMemory = this.myFunctions.myMeters.checkedWorkMeter.meterMemory;
          await S4_LoggerManager.ReadEventLoggerMemory(deviceMemory, this.myFunctions.checkedNfcCommands, this.progress, this.cancelTokenSource.Token);
          S4_LoggerManager.ClearEventLoggerMemory(deviceMemory);
          await S4_LoggerManager.WriteEventLoggerMemory(deviceMemory, this.myFunctions.checkedNfcCommands, this.progress, this.cancelTokenSource.Token);
          await S4_LoggerManager.AddEventAsync(DateTime.Now, LoggerEventTypes.NotDefined, (byte) 0, this.myFunctions.checkedNfcCommands, this.progress, this.cancelTokenSource.Token);
          this.TextBoxCommandResult.Text = "Event logger cleard";
          deviceMemory = (S4_DeviceMemory) null;
        }
        else if (sender == this.ButtonShowMonthLoggerData)
        {
          _UNIT_SCALE_ defaultUnitScale = (_UNIT_SCALE_) this.myFunctions.myMeters.checkedWorkMeter.meterMemory.GetParameterValue<ushort>(S4_Params.DeviceDefaultUnitScale);
          S4_BaseUnitSupport unitSupport = new S4_BaseUnitSupport(S4_MenuManager.GetBaseUnit(defaultUnitScale));
          string loggerData = S4_LoggerManager.GetMonthLoggerDataTextFromMemory(this.myFunctions.myMeters.checkedWorkMeter.meterMemory, unitSupport);
          this.TextBoxCommandResult.Text = loggerData;
          unitSupport = (S4_BaseUnitSupport) null;
          loggerData = (string) null;
        }
        else if (sender == this.ButtonClearMonthLoggerData)
        {
          await S4_LoggerManager.ReadMonthLoggerMemory(this.myFunctions.myMeters.checkedWorkMeter.meterMemory, this.myFunctions.checkedNfcCommands, this.progress, this.cancelTokenSource.Token);
          S4_LoggerManager.ClearMonthLoggerInMemory(this.myFunctions.myMeters.checkedWorkMeter.meterMemory);
          await S4_LoggerManager.WriteMonthLoggerMemory(this.myFunctions.myMeters.checkedWorkMeter.meterMemory, this.myFunctions.checkedNfcCommands, this.progress, this.cancelTokenSource.Token);
          this.TextBoxCommandResult.Text = "Month logger cleared";
        }
        else if (sender == this.ButtonShowBackup)
          this.TextBoxCommandResult.Text = this.myFunctions.myMeters.checkedWorkMeter.GetBackupData();
        else if (sender == this.ButtonShowCalibrationsFromBackup)
          this.ShowCalibrationChanges(this.myFunctions.myMeters.BackupMeter, "Compare backup to work");
        else if (sender == this.ButtonShowCalibrationsFromType)
          this.ShowCalibrationChanges(this.myFunctions.myMeters.TypeMeter, "Compare type to work");
        else
          this.TextBoxCommandResult.Text = sender != this.ButtonShowPCB_Assembly ? "Not supported button" : (this.myFunctions.myMeters.WorkMeter != null && this.myFunctions.myMeters.WorkMeter.deviceIdentification.PCB_Assembly != null ? this.myFunctions.myMeters.WorkMeter.deviceIdentification.PCB_Assembly.ToString() : "Data not available");
      }
      catch (OperationCanceledException ex)
      {
      }
      catch (TimeoutException ex)
      {
        this.TextBoxCommandResult.Text = "*** Timeout ***" + Environment.NewLine + ex.Message;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
        this.TextBoxCommandResult.Text = "*** Exception ***" + Environment.NewLine + ex.Message;
      }
      finally
      {
        this.SetStopState();
      }
    }

    private void ShowCalibrationChanges(S4_Meter sourceMeter, string comment)
    {
      S4_DeviceMemory meterMemory1 = sourceMeter.meterMemory;
      S4_DeviceMemory meterMemory2 = this.myFunctions.myMeters.WorkMeter.meterMemory;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(comment);
      stringBuilder.AppendLine();
      if (meterMemory2.IsParameterAvailable(S4_Params.rtcCalibrationValue) && meterMemory1.IsParameterAvailable(S4_Params.rtcCalibrationValue))
      {
        stringBuilder.AppendLine("*** Real time clock calibration ***");
        short parameterValue = meterMemory1.GetParameterValue<short>(S4_Params.rtcCalibrationValue);
        string str1 = parameterValue.ToString();
        parameterValue = meterMemory2.GetParameterValue<short>(S4_Params.rtcCalibrationValue);
        string str2 = parameterValue.ToString();
        stringBuilder.AppendLine("rtcCalibrationValue changed from " + str1.ToString() + " to " + str2.ToString());
        stringBuilder.AppendLine();
      }
      int parameterValue1;
      if (meterMemory2.IsParameterAvailable(S4_Params.cfg_radio_frequency_inc_dec) && meterMemory1.IsParameterAvailable(S4_Params.cfg_radio_frequency_inc_dec))
      {
        stringBuilder.AppendLine("*** Carrier frequency calibration ***");
        parameterValue1 = meterMemory1.GetParameterValue<int>(S4_Params.cfg_radio_frequency_inc_dec);
        string str3 = parameterValue1.ToString();
        parameterValue1 = meterMemory2.GetParameterValue<int>(S4_Params.cfg_radio_frequency_inc_dec);
        string str4 = parameterValue1.ToString();
        stringBuilder.AppendLine("cfg_radio_frequency_inc_dec changed from " + str3.ToString() + " to " + str4.ToString());
        stringBuilder.AppendLine();
      }
      stringBuilder.AppendLine("*** Ultransonic zero flow calibration ***");
      bool? transducerChannels = this.myFunctions.myMeters.WorkMeter.deviceIdentification.IsTwoTransducerChannels;
      int num;
      if (transducerChannels.HasValue)
      {
        transducerChannels = this.myFunctions.myMeters.WorkMeter.deviceIdentification.IsTwoTransducerChannels;
        num = transducerChannels.Value ? 1 : 0;
      }
      else
        num = 0;
      if (num != 0)
      {
        if (meterMemory2.IsParameterAvailable(S4_Params.zeroOffsetUnit1) && meterMemory1.IsParameterAvailable(S4_Params.zeroOffsetUnit1))
        {
          parameterValue1 = meterMemory1.GetParameterValue<int>(S4_Params.zeroOffsetUnit1);
          string str5 = parameterValue1.ToString();
          parameterValue1 = meterMemory2.GetParameterValue<int>(S4_Params.zeroOffsetUnit1);
          string str6 = parameterValue1.ToString();
          stringBuilder.AppendLine("zeroOffsetUnit1 changed from " + str5.ToString() + " to " + str6.ToString());
        }
        if (meterMemory2.IsParameterAvailable(S4_Params.zeroOffsetMeasUnit1) && meterMemory1.IsParameterAvailable(S4_Params.zeroOffsetMeasUnit1))
        {
          parameterValue1 = meterMemory1.GetParameterValue<int>(S4_Params.zeroOffsetUnit1);
          string str7 = parameterValue1.ToString();
          parameterValue1 = meterMemory2.GetParameterValue<int>(S4_Params.zeroOffsetUnit1);
          string str8 = parameterValue1.ToString();
          stringBuilder.AppendLine("zeroOffsetMeasUnit1 changed from " + str7.ToString() + " to " + str8.ToString());
        }
      }
      if (meterMemory2.IsParameterAvailable(S4_Params.zeroOffsetUnit2) && meterMemory1.IsParameterAvailable(S4_Params.zeroOffsetUnit2))
      {
        parameterValue1 = meterMemory1.GetParameterValue<int>(S4_Params.zeroOffsetUnit2);
        string str9 = parameterValue1.ToString();
        parameterValue1 = meterMemory2.GetParameterValue<int>(S4_Params.zeroOffsetUnit2);
        string str10 = parameterValue1.ToString();
        stringBuilder.AppendLine("zeroOffsetUnit2 changed from " + str9.ToString() + " to " + str10.ToString());
      }
      if (meterMemory2.IsParameterAvailable(S4_Params.zeroOffsetMeasUnit2) && meterMemory1.IsParameterAvailable(S4_Params.zeroOffsetMeasUnit2))
      {
        parameterValue1 = meterMemory1.GetParameterValue<int>(S4_Params.zeroOffsetUnit2);
        string str11 = parameterValue1.ToString();
        parameterValue1 = meterMemory2.GetParameterValue<int>(S4_Params.zeroOffsetUnit2);
        string str12 = parameterValue1.ToString();
        stringBuilder.AppendLine("zeroOffsetMeasUnit2 changed from " + str11.ToString() + " to " + str12.ToString());
      }
      stringBuilder.AppendLine();
      S4_TDC_Calibration s4TdcCalibration = new S4_TDC_Calibration();
      s4TdcCalibration.LoadCalibrationFromMemory(meterMemory2);
      stringBuilder.Append(s4TdcCalibration.GetCalibrationChanges(meterMemory1, (string) null));
      this.TextBoxCommandResult.Text = stringBuilder.ToString();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/S4_Handler;component/userinterface/s4_mapdatawindow.xaml", UriKind.Relative));
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
          this.TextBlockStatus = (TextBlock) target;
          break;
        case 3:
          this.ProgressBar1 = (ProgressBar) target;
          break;
        case 4:
          this.ButtonBreak = (Button) target;
          this.ButtonBreak.Click += new RoutedEventHandler(this.ButtonBreak_Click);
          break;
        case 5:
          this.ButtonShowDeviceDate = (Button) target;
          this.ButtonShowDeviceDate.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 6:
          this.ButtonShowEventLoggerData = (Button) target;
          this.ButtonShowEventLoggerData.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 7:
          this.ButtonClearEventLoggerData = (Button) target;
          this.ButtonClearEventLoggerData.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 8:
          this.ButtonShowMonthLoggerData = (Button) target;
          this.ButtonShowMonthLoggerData.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 9:
          this.ButtonClearMonthLoggerData = (Button) target;
          this.ButtonClearMonthLoggerData.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 10:
          this.ButtonShowDiagnosticData = (Button) target;
          this.ButtonShowDiagnosticData.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 11:
          this.ButtonReadAndShowDiagnosticData = (Button) target;
          this.ButtonReadAndShowDiagnosticData.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 12:
          this.ButtonShowDiagnosticDataInternal = (Button) target;
          this.ButtonShowDiagnosticDataInternal.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 13:
          this.ButtonReadAndShowDiagnosticDataInternal = (Button) target;
          this.ButtonReadAndShowDiagnosticDataInternal.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 14:
          this.ButtonInitDiagnosticAndLoggers = (Button) target;
          this.ButtonInitDiagnosticAndLoggers.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 15:
          this.ButtonSetDiagnosticAndLoggers = (Button) target;
          this.ButtonSetDiagnosticAndLoggers.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 16:
          this.ButtonShowBackup = (Button) target;
          this.ButtonShowBackup.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 17:
          this.ButtonShowCalibrationsFromBackup = (Button) target;
          this.ButtonShowCalibrationsFromBackup.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 18:
          this.ButtonShowCalibrationsFromType = (Button) target;
          this.ButtonShowCalibrationsFromType.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 19:
          this.ButtonShowPCB_Assembly = (Button) target;
          this.ButtonShowPCB_Assembly.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 20:
          this.TextBoxCommandResult = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
