// Decompiled with JetBrains decompiler
// Type: S4_Handler.UserInterface.S4_DeviceDataWindow
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommonWPF;
using GmmDbLib;
using HandlerLib;
using HandlerLib.NFC;
using NLog;
using S4_Handler.Functions;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace S4_Handler.UserInterface
{
  public partial class S4_DeviceDataWindow : Window, IComponentConnector
  {
    private static Logger S4_HandlerTestWindowsLogger = LogManager.GetLogger("S4_DeviceDataWindows");
    private S4_HandlerWindowFunctions myWindowFunctions;
    private S4_HandlerFunctions myFunctions;
    private S4_DeviceCommandsNFC myCommands;
    private ProgressHandler progress;
    private Cursor defaultCursor;
    private CancellationTokenSource cancelTokenSource;
    private StringBuilder Warnings = new StringBuilder();
    internal ProgressBar ProgressBar1;
    internal Button ButtonBreak;
    internal StackPanel StackPanalData;
    internal Button ButtonReadCurrentData;
    internal Button ButtonReadAlive;
    internal Button ButtonReadNextAlive;
    internal Button ButtonReadLastAlive;
    internal Button ButtonReadLoggerList;
    internal Button ButtonReadSelectedLogger;
    internal Button ButtonReadDateTime;
    internal TextBox TextBoxTimeZone;
    internal TextBox TextBoxNewDate;
    internal TextBox TextBoxNewTime;
    internal Button ButtonSetDateTime;
    internal Button ButtonSetDateTimeFromPC;
    internal Button ButtonSetDateTimeFromTimeServer;
    internal Button ButtonCompareDeviceAndTimeServerTime;
    internal Button ButtonGetBatteryEndDate;
    internal TextBox TextBoxBatteryEndDate;
    internal TextBox TextBoxBatteryPriWarningMonth;
    internal TextBox TextBoxBatteryDurabilityMonth;
    internal Button ButtonSetBatteryEndDate;
    internal Button UpdateNdef;
    internal TextBlock TextBlockStatus;
    internal GroupBox GroupBoxResults;
    internal TextBox TextBoxCommandResult;
    internal GroupBox GroupBoxLoggers;
    internal DataGrid DataGridLoggers;
    internal GroupBox GroupBoxData;
    internal DataGrid DataGridData;
    private bool _contentLoaded;

    public S4_DeviceDataWindow(S4_HandlerWindowFunctions myWindowFunctions)
    {
      this.myWindowFunctions = myWindowFunctions;
      this.myFunctions = myWindowFunctions.MyFunctions;
      this.myCommands = this.myFunctions.myDeviceCommands;
      this.InitializeComponent();
      this.DataGridLoggers.ItemsSource = (IEnumerable) this.myWindowFunctions.MyFunctions.myLoggerManager.LoggerList;
      this.SetLoggerLIstInfo();
      this.SetDateTime(new DateTimeOffset(DateTime.Now));
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.SetStopState();
    }

    private void SetLoggerLIstInfo()
    {
      string str = this.GroupBoxLoggers.Header.ToString();
      int num = str.IndexOf(": ");
      if (num <= 0)
        return;
      this.GroupBoxLoggers.Header = (object) (str.Substring(0, num + 2) + "(Read from device)");
    }

    private void SetDateTime(DateTimeOffset currentTime)
    {
      TimeZoneSupport timeZoneSupport = new TimeZoneSupport(currentTime);
      this.TextBoxTimeZone.Text = timeZoneSupport.GetTimeZoneAsDecimal().ToString();
      this.TextBoxNewDate.Text = timeZoneSupport.TimeZoneTime.Date.ToShortDateString();
      this.TextBoxNewTime.Text = timeZoneSupport.TimeZoneTime.DateTime.ToLongTimeString();
    }

    private void SetRunState()
    {
      this.cancelTokenSource = new CancellationTokenSource();
      this.progress.Reset();
      this.progress.Split(new double[2]{ 2.0, 98.0 });
      this.progress.Report("Run");
      this.ButtonBreak.IsEnabled = true;
      if (this.Cursor != Cursors.Wait)
      {
        this.defaultCursor = this.Cursor;
        this.Cursor = Cursors.Wait;
      }
      this.Warnings.Clear();
    }

    private void SetStopState()
    {
      this.ButtonBreak.IsEnabled = false;
      this.Cursor = this.defaultCursor;
      this.progress.Reset();
      this.TextBlockStatus.Text = "";
      if (this.Warnings.Length <= 0)
        return;
      this.TextBoxCommandResult.AppendText(Environment.NewLine);
      this.TextBoxCommandResult.AppendText(Environment.NewLine);
      this.TextBoxCommandResult.AppendText("---- Warnings ---" + Environment.NewLine);
      this.TextBoxCommandResult.AppendText(Environment.NewLine);
      this.TextBoxCommandResult.AppendText(this.Warnings.ToString());
      this.Warnings.Clear();
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
        if (obj.Tag != null && obj.Tag is ProgressWarning)
          this.Warnings.Append(obj.Message + ((ProgressWarning) obj.Tag).ToString());
        else
          this.TextBlockStatus.Text = obj.Message;
      }
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
        if (sender == this.ButtonReadDateTime)
        {
          this.TextBoxCommandResult.Clear();
          TextBox textBox = this.TextBoxCommandResult;
          string str = await this.ReadAndGetDeviceTime();
          textBox.Text = str;
          textBox = (TextBox) null;
          str = (string) null;
        }
        else if (sender == this.ButtonSetDateTime)
        {
          Decimal timeZone = Decimal.Parse(this.TextBoxTimeZone.Text);
          DateTime dateTime = DateTime.Parse(this.TextBoxNewDate.Text);
          TimeSpan timeOfDay = TimeSpan.Parse(this.TextBoxNewTime.Text);
          dateTime = dateTime.Add(timeOfDay);
          DateTimeOffset dateTimeOffset = await this.myFunctions.SetDeviceTime(dateTime, timeZone, this.progress, this.cancelTokenSource.Token);
          DateTimeOffset setTime = dateTimeOffset;
          dateTimeOffset = new DateTimeOffset();
          TextBox textBox = this.TextBoxCommandResult;
          string str1 = Environment.NewLine;
          string str2 = Environment.NewLine;
          string str3 = await this.ReadAndGetDeviceTime();
          textBox.Text = "New device time done." + str1 + str2 + str3;
          textBox = (TextBox) null;
          str1 = (string) null;
          str2 = (string) null;
          str3 = (string) null;
          timeOfDay = new TimeSpan();
          setTime = new DateTimeOffset();
        }
        else if (sender == this.ButtonSetDateTimeFromPC)
        {
          this.TextBoxCommandResult.Clear();
          Decimal timeZone = Decimal.Parse(this.TextBoxTimeZone.Text);
          DateTimeOffset dateTimeOffset = await this.myFunctions.SetTimeForTimeZoneFromPcTime(timeZone, this.progress, this.cancelTokenSource.Token);
          DateTimeOffset setTime = dateTimeOffset;
          dateTimeOffset = new DateTimeOffset();
          TextBox textBox = this.TextBoxCommandResult;
          string str4 = Environment.NewLine;
          string str5 = Environment.NewLine;
          string str6 = await this.ReadAndGetDeviceTime();
          textBox.Text = "New device time from PC time and time zone done." + str4 + str5 + str6;
          textBox = (TextBox) null;
          str4 = (string) null;
          str5 = (string) null;
          str6 = (string) null;
          setTime = new DateTimeOffset();
        }
        else if (sender == this.ButtonSetDateTimeFromTimeServer)
        {
          this.TextBoxCommandResult.Clear();
          Decimal timeZone = Decimal.Parse(this.TextBoxTimeZone.Text);
          string timeServer;
          try
          {
            timeServer = DbBasis.PrimaryDB.BaseDbConnection.DatabaseIdentification["TimeServer"];
          }
          catch
          {
            throw new ApplicationException("Time server not specified in database identification table!");
          }
          InternetTime MyTimeServer = new InternetTime(timeServer);
          DateTime TimeServerTime = MyTimeServer.Connect(false);
          DateTime dateTimeAtDeviceTimeZone = TimeServerTime.ToUniversalTime().AddHours((double) timeZone);
          DateTimeOffset dateTimeOffset1 = new DateTimeOffset(dateTimeAtDeviceTimeZone.Year, dateTimeAtDeviceTimeZone.Month, dateTimeAtDeviceTimeZone.Day, dateTimeAtDeviceTimeZone.Hour, dateTimeAtDeviceTimeZone.Minute, dateTimeAtDeviceTimeZone.Second, new TimeSpan(0, (int) (timeZone * 60M), 0));
          DateTimeOffset dateTimeOffset2 = await this.myFunctions.checkedCommands.CommonNfcCommands.SetSystemDateTime(dateTimeOffset1, this.progress, this.cancelTokenSource.Token);
          DateTimeOffset setTime = dateTimeOffset2;
          dateTimeOffset2 = new DateTimeOffset();
          TextBox textBox = this.TextBoxCommandResult;
          string str7 = Environment.NewLine;
          string str8 = Environment.NewLine;
          string str9 = await this.ReadAndGetDeviceTime();
          textBox.Text = "New device time from TimeServer and time zone: " + str7 + str8 + str9;
          textBox = (TextBox) null;
          str7 = (string) null;
          str8 = (string) null;
          str9 = (string) null;
          timeServer = (string) null;
          MyTimeServer = (InternetTime) null;
          dateTimeOffset1 = new DateTimeOffset();
          setTime = new DateTimeOffset();
        }
        else if (sender == this.ButtonCompareDeviceAndTimeServerTime)
        {
          this.TextBoxCommandResult.Clear();
          string timeServer;
          try
          {
            timeServer = DbBasis.PrimaryDB.BaseDbConnection.DatabaseIdentification["TimeServer"];
          }
          catch
          {
            throw new ApplicationException("Time server not specified in database identification table!");
          }
          if (this.myFunctions.myMeters.ConnectedMeter != null)
          {
            StringBuilder message = new StringBuilder();
            DateTimeOffset dateTimeOffset = await this.myFunctions.GetSystemDateTime(this.progress, this.cancelTokenSource.Token);
            DateTimeOffset systemTime = dateTimeOffset;
            dateTimeOffset = new DateTimeOffset();
            InternetTime MyTimeServer = new InternetTime(timeServer);
            DateTime TimeServerTime = MyTimeServer.Connect(false);
            TimeSpan DiffTime = systemTime.DateTime.Subtract(TimeServerTime);
            double Diff = DiffTime.TotalSeconds;
            int num = (int) MessageBox.Show("Difference = " + Diff.ToString() + "Seconds", "Compare TimeServerTime and device time", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            message = (StringBuilder) null;
            systemTime = new DateTimeOffset();
            MyTimeServer = (InternetTime) null;
            DiffTime = new TimeSpan();
          }
          timeServer = (string) null;
        }
        else if (sender == this.ButtonReadCurrentData)
        {
          if (new FirmwareVersion(this.myFunctions.myMeters.ConnectedMeter.deviceIdentification.FirmwareVersion.Value) < (object) "1.4.8 IUW")
          {
            this.TextBlockStatus.Text = "Current data protocols not available for this firmware";
          }
          else
          {
            StringBuilder cr = new StringBuilder();
            S4_CurrentData currentData = await this.myFunctions.ReadCurrentDataAsync(this.progress, this.cancelTokenSource.Token);
            cr.AppendLine("*** Current device data ***");
            cr.AppendLine(currentData.ToTextBlock());
            S4_FunctionalState functionalState = await this.myFunctions.ReadAlliveAndStateAsync(this.progress, this.cancelTokenSource.Token);
            cr.AppendLine("*** Functional state ***");
            cr.AppendLine(functionalState.ToTextBlock());
            DeviceStateCounter stateCounter = await this.myFunctions.ReadStateCounterAsync(this.progress, this.cancelTokenSource.Token);
            cr.AppendLine("*** Current state counter ***");
            cr.AppendLine(stateCounter.ToTextBlock());
            S4_SystemState deviceState = await this.myFunctions.GetDeviceState(this.progress, this.cancelTokenSource.Token);
            cr.AppendLine("*** Device state ***");
            cr.AppendLine(deviceState.ToTextBlock());
            if (this.myFunctions.myMeters.ConnectedMeter.meterMemory.SmartFunctionFlashRange != null)
            {
              S4_SmartFunctionInfo smartFunctionInfo = await this.myFunctions.ReadSmartFunctionInfoAsync(this.progress, this.cancelTokenSource.Token);
              cr.AppendLine(smartFunctionInfo.ToTextBlock());
              smartFunctionInfo = (S4_SmartFunctionInfo) null;
            }
            this.TextBoxCommandResult.Text = cr.ToString();
            this.TextBlockStatus.Text = "Read current data: OK";
            cr = (StringBuilder) null;
            currentData = (S4_CurrentData) null;
            functionalState = (S4_FunctionalState) null;
            stateCounter = (DeviceStateCounter) null;
            deviceState = (S4_SystemState) null;
          }
        }
        else if (sender == this.ButtonReadAlive)
        {
          S4_FunctionalState functionalState = await this.myFunctions.checkedCommands.ReadAlliveAndStateAsync(this.progress, this.cancelTokenSource.Token);
          this.TextBoxCommandResult.Text = "Read alive and state" + Environment.NewLine + functionalState.ToTextBlock();
          functionalState = (S4_FunctionalState) null;
        }
        else if (sender == this.ButtonReadNextAlive)
        {
          S4_FunctionalState functionalState = await this.myFunctions.checkedCommands.ReadAlliveAndStateAsync(this.progress, this.cancelTokenSource.Token, S4_DeviceCommandsNFC.FunctionalStateRequest.next);
          this.TextBoxCommandResult.Text = "Read next alive and state" + Environment.NewLine + functionalState.ToTextBlock();
          functionalState = (S4_FunctionalState) null;
        }
        else if (sender == this.ButtonReadLastAlive)
        {
          S4_FunctionalState functionalState = await this.myFunctions.checkedCommands.ReadAlliveAndStateAsync(this.progress, this.cancelTokenSource.Token, S4_DeviceCommandsNFC.FunctionalStateRequest.last);
          this.TextBoxCommandResult.Text = "Read last alive and state" + Environment.NewLine + functionalState.ToTextBlock();
          functionalState = (S4_FunctionalState) null;
        }
        else if (sender == this.ButtonReadLoggerList)
        {
          ObservableCollection<LoggerListItem> loggerList = await this.myFunctions.ReadLoggerListAsync(this.progress, this.cancelTokenSource.Token);
          this.DataGridLoggers.ItemsSource = (IEnumerable) loggerList;
          if (loggerList.Count > 0)
          {
            this.DataGridLoggers.SelectedIndex = 0;
            this.ButtonReadSelectedLogger.IsEnabled = true;
          }
          else
            this.ButtonReadSelectedLogger.IsEnabled = false;
          this.SetLoggerLIstInfo();
          this.TextBlockStatus.Text = "Read logger list: OK";
          loggerList = (ObservableCollection<LoggerListItem>) null;
        }
        else if (sender == this.ButtonReadSelectedLogger)
          await this.ReadLoggerAsync();
        else if (sender == this.UpdateNdef)
        {
          byte[] resultAsync = await this.myFunctions.myDeviceCommands.CommonNfcCommands.SendCommandAndGetResultAsync(this.progress, this.cancelTokenSource.Token, NfcCommands.UpdateNdef);
          this.TextBoxCommandResult.Text = "Update NFC tag data: done";
        }
        else if (sender == this.ButtonGetBatteryEndDate)
        {
          StringBuilder info = new StringBuilder();
          info.AppendLine("GetBatteryEndDate result");
          info.AppendLine();
          NfcDeviceCommands.BatteryEndDateData endDateData = await this.myFunctions.myDeviceCommands.CommonNfcCommands.GetBatteryEndDateAsync(this.progress, this.cancelTokenSource.Token);
          info.AppendLine("BatteryEndDate: " + endDateData.EndDate.ToShortDateString());
          if (endDateData.BatteryDurabilityMonths.HasValue)
          {
            info.AppendLine("DurabilityMonths: " + endDateData.BatteryDurabilityMonths.ToString());
            info.AppendLine("PreWaringMonths: " + endDateData.BatteryPreWaringMonths.ToString());
          }
          this.TextBoxCommandResult.Text = info.ToString();
          info = (StringBuilder) null;
          endDateData = (NfcDeviceCommands.BatteryEndDateData) null;
        }
        else if (sender == this.ButtonSetBatteryEndDate)
        {
          DateTime batteryEndDate = DateTime.Parse(this.TextBoxBatteryEndDate.Text);
          if (this.TextBoxBatteryDurabilityMonth.Text.Trim() == "" && this.TextBoxBatteryPriWarningMonth.Text.Trim() == "")
          {
            await this.myFunctions.myDeviceCommands.CommonNfcCommands.SetBatteryEndDateAsync(this.progress, this.cancelTokenSource.Token, batteryEndDate);
          }
          else
          {
            byte batteryDurabilityMonth = byte.Parse(this.TextBoxBatteryDurabilityMonth.Text);
            sbyte batteryPreWaringMonth = sbyte.Parse(this.TextBoxBatteryPriWarningMonth.Text);
            await this.myFunctions.myDeviceCommands.CommonNfcCommands.SetBatteryEndDateAsync(this.progress, this.cancelTokenSource.Token, batteryEndDate, new byte?(batteryDurabilityMonth), new sbyte?(batteryPreWaringMonth));
          }
          this.TextBoxCommandResult.Text = "SetBatteryEndDate done";
        }
        else
          this.TextBoxCommandResult.Text = "Not supported button";
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

    private async Task<string> ReadAndGetDeviceTime()
    {
      StringBuilder message = new StringBuilder();
      DateTimeOffset currentTime = DateTimeOffset.MinValue;
      if (this.myFunctions.myMeters.ConnectedMeter != null)
      {
        DateTimeOffset dateTimeOffset = await this.myFunctions.GetSystemDateTime(this.progress, this.cancelTokenSource.Token);
        DateTimeOffset systemTime = dateTimeOffset;
        dateTimeOffset = new DateTimeOffset();
        message.AppendLine("Device time by read protocol:");
        message.Append("    ");
        message.AppendLine(systemTime.ToString());
        currentTime = systemTime;
        systemTime = new DateTimeOffset();
      }
      if (currentTime == DateTimeOffset.MinValue)
      {
        message.AppendLine("No time available");
        currentTime = new DateTimeOffset(DateTime.Now);
      }
      this.SetDateTime(currentTime);
      string deviceTime = message.ToString();
      message = (StringBuilder) null;
      return deviceTime;
    }

    private void ButtonAddNewMap_Click(object sender, RoutedEventArgs e)
    {
      this.myWindowFunctions.OpenMapClassManagerToReadFromMapFile();
    }

    private void AddCommendResult(string result = "")
    {
      if (this.TextBoxCommandResult.Text.Length > 0)
        this.TextBoxCommandResult.AppendText(Environment.NewLine + result);
      else
        this.TextBoxCommandResult.AppendText(result);
    }

    private void DataGridData_AutoGeneratingColumn(
      object sender,
      DataGridAutoGeneratingColumnEventArgs e)
    {
      if (!(e.PropertyType == typeof (DateTime)))
        return;
      (e.Column as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy HH.mm.ss";
    }

    private async void DataGridLoggers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      this.SetRunState();
      try
      {
        await this.ReadLoggerAsync();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.SetStopState();
    }

    private async Task ReadLoggerAsync()
    {
      if (this.DataGridLoggers.SelectedItem == null)
        return;
      string LoggerName = ((LoggerListItem) this.DataGridLoggers.SelectedItem).LoggerName;
      IEnumerable loggerData = await this.myFunctions.ReadLoggerDataAsync(LoggerName, this.progress, this.cancelTokenSource.Token);
      this.DataGridData.Columns.Clear();
      if (!(loggerData is List<DynamicLoggerEntry>))
      {
        this.DataGridData.AutoGenerateColumns = true;
      }
      else
      {
        this.DataGridData.AutoGenerateColumns = false;
        List<DynamicLoggerEntry> collection = (List<DynamicLoggerEntry>) loggerData;
        if (collection.Count > 0)
        {
          foreach (KeyValuePair<string, object> dynamicProperty in collection[0].DynamicProperties)
          {
            KeyValuePair<string, object> property = dynamicProperty;
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = (object) property.Key;
            textColumn.Binding = (BindingBase) new Binding(property.Key);
            if (property.Value is DateTime)
              textColumn.Binding.StringFormat = "dd.MM.yyyy HH.mm.ss";
            this.DataGridData.Columns.Add((DataGridColumn) textColumn);
            textColumn = (DataGridTextColumn) null;
            property = new KeyValuePair<string, object>();
          }
        }
        collection = (List<DynamicLoggerEntry>) null;
      }
      this.DataGridData.ItemsSource = loggerData;
      string HeaderText = this.GroupBoxData.Header.ToString();
      int infoIndex = HeaderText.IndexOf(": ");
      if (infoIndex > 0)
      {
        HeaderText = HeaderText.Substring(0, infoIndex + 2);
        HeaderText = HeaderText + "(" + LoggerName + ")";
        this.GroupBoxData.Header = (object) HeaderText;
      }
      this.TextBlockStatus.Text = "Read logger: OK";
      LoggerName = (string) null;
      loggerData = (IEnumerable) null;
      HeaderText = (string) null;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/S4_Handler;component/userinterface/s4_devicedatawindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.ProgressBar1 = (ProgressBar) target;
          break;
        case 2:
          this.ButtonBreak = (Button) target;
          this.ButtonBreak.Click += new RoutedEventHandler(this.ButtonBreak_Click);
          break;
        case 3:
          this.StackPanalData = (StackPanel) target;
          break;
        case 4:
          this.ButtonReadCurrentData = (Button) target;
          this.ButtonReadCurrentData.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 5:
          this.ButtonReadAlive = (Button) target;
          this.ButtonReadAlive.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 6:
          this.ButtonReadNextAlive = (Button) target;
          this.ButtonReadNextAlive.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 7:
          this.ButtonReadLastAlive = (Button) target;
          this.ButtonReadLastAlive.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 8:
          this.ButtonReadLoggerList = (Button) target;
          this.ButtonReadLoggerList.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 9:
          this.ButtonReadSelectedLogger = (Button) target;
          this.ButtonReadSelectedLogger.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 10:
          this.ButtonReadDateTime = (Button) target;
          this.ButtonReadDateTime.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 11:
          this.TextBoxTimeZone = (TextBox) target;
          break;
        case 12:
          this.TextBoxNewDate = (TextBox) target;
          break;
        case 13:
          this.TextBoxNewTime = (TextBox) target;
          break;
        case 14:
          this.ButtonSetDateTime = (Button) target;
          this.ButtonSetDateTime.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 15:
          this.ButtonSetDateTimeFromPC = (Button) target;
          this.ButtonSetDateTimeFromPC.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 16:
          this.ButtonSetDateTimeFromTimeServer = (Button) target;
          this.ButtonSetDateTimeFromTimeServer.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 17:
          this.ButtonCompareDeviceAndTimeServerTime = (Button) target;
          this.ButtonCompareDeviceAndTimeServerTime.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 18:
          this.ButtonGetBatteryEndDate = (Button) target;
          this.ButtonGetBatteryEndDate.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 19:
          this.TextBoxBatteryEndDate = (TextBox) target;
          break;
        case 20:
          this.TextBoxBatteryPriWarningMonth = (TextBox) target;
          break;
        case 21:
          this.TextBoxBatteryDurabilityMonth = (TextBox) target;
          break;
        case 22:
          this.ButtonSetBatteryEndDate = (Button) target;
          this.ButtonSetBatteryEndDate.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 23:
          this.UpdateNdef = (Button) target;
          this.UpdateNdef.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 24:
          this.TextBlockStatus = (TextBlock) target;
          break;
        case 25:
          this.GroupBoxResults = (GroupBox) target;
          break;
        case 26:
          this.TextBoxCommandResult = (TextBox) target;
          break;
        case 27:
          this.GroupBoxLoggers = (GroupBox) target;
          break;
        case 28:
          this.DataGridLoggers = (DataGrid) target;
          this.DataGridLoggers.MouseDoubleClick += new MouseButtonEventHandler(this.DataGridLoggers_MouseDoubleClick);
          break;
        case 29:
          this.GroupBoxData = (GroupBox) target;
          break;
        case 30:
          this.DataGridData = (DataGrid) target;
          this.DataGridData.AutoGeneratingColumn += new EventHandler<DataGridAutoGeneratingColumnEventArgs>(this.DataGridData_AutoGeneratingColumn);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
