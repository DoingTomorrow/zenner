// Decompiled with JetBrains decompiler
// Type: S4_Handler.UserInterface.S4_RadioTestWindow
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommonWPF;
using CommunicationPort;
using HandlerLib;
using NLog;
using S4_Handler.Functions;
using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace S4_Handler.UserInterface
{
  public partial class S4_RadioTestWindow : Window, IComponentConnector
  {
    internal static Logger S4_RadioTestWindowLogger = LogManager.GetLogger(nameof (S4_RadioTestWindow));
    private S4_HandlerWindowFunctions MyWindowFunctions;
    private S4_HandlerFunctions MyFunctions;
    private S4_DeviceCommandsNFC DeviceCommands;
    private string MainComPort;
    private string RadioTestComPort;
    private RadioTestByDevice RadioTestDevice;
    private bool BlockRadioComPortSetup;
    private Cursor defaultCursor;
    private CancellationTokenSource CancelTokenSource;
    private CancellationToken CancelToken;
    private ProgressHandler progress;
    private RadioTestLoopResults RadioTestLoopRepeatResults;
    private uint CycleMilliSeconds;
    private int RequiredReceiveCount;
    private ushort TimoutSeconds;
    private ushort SyncWord;
    private uint DeviceID;
    private DateTime StartTime;
    private DateTime EndTime;
    private int SendCount;
    private bool Done;
    private Brush DefaultLabelBrush;
    private string DefaultOpenText;
    private int LoopCount;
    private int LoopRepeatCount;
    internal StartupLib.GmmCorporateControl gmmCorporateControl1;
    internal Button ButtonSetDefault;
    internal Button ButtonClear;
    internal Button ButtonBreak;
    internal StackPanel StackPanelButtons;
    internal Button ButtonOpenRadioMinoConnect;
    internal Button ButtonSendUnmodulatedCarrier;
    internal Button ButtonSendModulatedCarrier;
    internal Button ButtonSendRadioPackets;
    internal Button ButtonTransmitRadioPacketsByCycle;
    internal StackPanel StackPanelTests;
    internal Button ButtonTransmitTest;
    internal Button ButtonReceiveTest;
    internal CheckBox CheckBoxLoop;
    internal TextBlock TextBlockLoopCounts;
    internal StackPanel StackPanelManualReceive;
    internal TextBox TextBoxReceiveBytes;
    internal Button ButtonSetModePackageReceive;
    internal Button ButtonSendPacketByMinoConnect;
    internal Button ButtonExitModeAndGetResult;
    internal StackPanel StackPanelFrequency;
    internal TextBox TextBoxFrequencyIncrement;
    internal Button ButtonReadFrequencyIncrement;
    internal Button ButtonWriteFrequencyIncrement;
    internal StackPanel StackRadioPower;
    internal TextBox TextBoxRadioPower;
    internal Button ButtonReadRadioPower;
    internal Button ButtonWriteRadioPower;
    internal Button ButtonStartLogToExcel;
    internal Button ButtonShowExcelLog;
    internal DockPanel DockPanelInput;
    internal ProgressBar ProgressBarRun;
    internal RadioButton RadioButtonMinoConnect;
    internal RadioButton RadioButtonIUWS;
    internal Label LabelMiConComPort;
    internal ComboBox ComboBoxMinoConnectComPort;
    internal TextBox TextBoxCycleTime;
    internal TextBox TextBoxTimeoutTime;
    internal TextBox TextBoxSyncWord;
    internal TextBox TextBoxDeviceID;
    internal TextBox TextBoxRequiredReceiveCount;
    internal TextBox TextBoxFrequency;
    internal TextBox TextBoxActiveTest;
    internal TextBlock TextBlockSendCount;
    internal TextBlock TextBlockReceiveCount;
    internal TextBlock TextBlockPollingCount;
    internal TextBox TextBoxStatus;
    private bool _contentLoaded;

    internal S4_RadioTestWindow(
      S4_HandlerWindowFunctions myWindowFunctions,
      string mainComPort,
      uint DeviceID,
      string radioMiConComPort = null)
    {
      this.MyWindowFunctions = myWindowFunctions;
      this.MyFunctions = this.MyWindowFunctions.MyFunctions;
      this.DeviceCommands = this.MyFunctions.myDeviceCommands;
      this.MainComPort = mainComPort;
      this.DeviceID = DeviceID;
      this.InitializeComponent();
      this.defaultCursor = this.Cursor;
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.ProgressBarRun.Minimum = 0.0;
      this.DefaultOpenText = (string) this.ButtonOpenRadioMinoConnect.Content;
      this.DefaultLabelBrush = this.LabelMiConComPort.Background;
      this.PrepareStopState();
      if (radioMiConComPort != null)
      {
        this.RadioTestComPort = radioMiConComPort;
        this.BlockRadioComPortSetup = true;
        this.ComboBoxMinoConnectComPort.Items.Add((object) radioMiConComPort);
        this.ComboBoxMinoConnectComPort.SelectedIndex = 0;
      }
      else
      {
        this.InitValues(false);
        this.SetComPortList();
      }
    }

    private void InitValues(bool setDefault)
    {
      this.TextBoxFrequency.Text = 868.3.ToString("0.00");
      this.TextBoxSyncWord.Text = (ushort) 37331.ToString("X04");
      this.TextBoxDeviceID.Text = this.DeviceID.ToString("d08");
      this.TextBoxCycleTime.Text = "700";
      this.TextBoxTimeoutTime.Text = "20";
      this.TextBoxRequiredReceiveCount.Text = "10";
      if (setDefault || !this.MyWindowFunctions.IsPluginObject)
        return;
      try
      {
        string str1 = PlugInLoader.GmmConfiguration.GetValue("S4_Handler", S4_HandlerWindowFunctions.ConfigVariables.RadioMinoConnectComPort.ToString());
        if (!string.IsNullOrEmpty(str1))
          this.RadioTestComPort = str1;
        string str2 = PlugInLoader.GmmConfiguration.GetValue("S4_Handler", S4_HandlerWindowFunctions.ConfigVariables.RadioTestDevice.ToString());
        if (!string.IsNullOrEmpty(str2))
        {
          if (str2 == RadioTestByDevice.RadioTestDevice.MinoConnect.ToString())
            this.RadioButtonMinoConnect.IsChecked = new bool?(true);
          else if (str2 == RadioTestByDevice.RadioTestDevice.IUWS.ToString())
            this.RadioButtonIUWS.IsChecked = new bool?(true);
        }
        string str3 = PlugInLoader.GmmConfiguration.GetValue("S4_Handler", S4_HandlerWindowFunctions.ConfigVariables.RadioTimeoutTime.ToString());
        if (!string.IsNullOrEmpty(str3))
          this.TextBoxTimeoutTime.Text = str3;
        string str4 = PlugInLoader.GmmConfiguration.GetValue("S4_Handler", S4_HandlerWindowFunctions.ConfigVariables.RadioCycleTime.ToString());
        if (!string.IsNullOrEmpty(str4))
          this.TextBoxCycleTime.Text = str4;
        string str5 = PlugInLoader.GmmConfiguration.GetValue("S4_Handler", S4_HandlerWindowFunctions.ConfigVariables.RadioSyncWord.ToString());
        if (!string.IsNullOrEmpty(str5))
          this.TextBoxSyncWord.Text = str5;
        string s = PlugInLoader.GmmConfiguration.GetValue("S4_Handler", S4_HandlerWindowFunctions.ConfigVariables.RadioTestFrequencyHz.ToString());
        if (!string.IsNullOrEmpty(s))
          this.TextBoxFrequency.Text = ((double) int.Parse(s) / 1000000.0).ToString();
      }
      catch
      {
      }
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      this.CloseRadioMinoConnect();
      if (!this.MyWindowFunctions.IsPluginObject)
        return;
      try
      {
        string strInhalt = this.RadioButtonIUWS.IsChecked.Value ? RadioTestByDevice.RadioTestDevice.IUWS.ToString() : RadioTestByDevice.RadioTestDevice.MinoConnect.ToString();
        PlugInLoader.GmmConfiguration.SetOrUpdateValue("S4_Handler", S4_HandlerWindowFunctions.ConfigVariables.RadioTestDevice.ToString(), strInhalt);
        PlugInLoader.GmmConfiguration.SetOrUpdateValue("S4_Handler", S4_HandlerWindowFunctions.ConfigVariables.RadioTimeoutTime.ToString(), this.TextBoxTimeoutTime.Text);
        PlugInLoader.GmmConfiguration.SetOrUpdateValue("S4_Handler", S4_HandlerWindowFunctions.ConfigVariables.RadioCycleTime.ToString(), this.TextBoxCycleTime.Text);
        PlugInLoader.GmmConfiguration.SetOrUpdateValue("S4_Handler", S4_HandlerWindowFunctions.ConfigVariables.RadioSyncWord.ToString(), this.TextBoxSyncWord.Text);
        PlugInLoader.GmmConfiguration.SetOrUpdateValue("S4_Handler", S4_HandlerWindowFunctions.ConfigVariables.RadioTestFrequencyHz.ToString(), ((int) (double.Parse(this.TextBoxFrequency.Text) * 1000000.0)).ToString());
      }
      catch
      {
      }
    }

    private void OnProgress(ProgressArg obj)
    {
      if (obj.Tag != null && obj.Tag is RadioTestLoopResults)
      {
        RadioTestLoopResults tag = obj.Tag as RadioTestLoopResults;
        this.TextBlockSendCount.Text = tag.SendCount.ToString();
        this.TextBlockReceiveCount.Text = tag.ReceiveCount.ToString();
        this.TextBlockPollingCount.Text = tag.PollingCount.ToString();
      }
      if (!string.IsNullOrEmpty(obj.Message) && obj.Message != "0")
        this.AddStatusLine(obj.Message);
      this.ProgressBarRun.Value = DateTime.Now.Subtract(this.StartTime).TotalSeconds;
    }

    private void AddStatusLine(string theLine)
    {
      if (this.TextBoxStatus.Text.Length > 0)
      {
        this.TextBoxStatus.AppendText(Environment.NewLine + theLine);
        this.TextBoxStatus.ScrollToEnd();
      }
      else
        this.TextBoxStatus.Text = theLine;
    }

    private void ButtonBreak_Click(object sender, RoutedEventArgs e)
    {
      this.CancelTokenSource.Cancel();
    }

    private void SetRunState(string functionText, bool isManualStop = false)
    {
      S4_RadioTestWindow.S4_RadioTestWindowLogger.Trace(functionText);
      this.TimoutSeconds = ushort.Parse(this.TextBoxTimeoutTime.Text);
      this.CycleMilliSeconds = uint.Parse(this.TextBoxCycleTime.Text);
      this.RequiredReceiveCount = int.Parse(this.TextBoxRequiredReceiveCount.Text);
      this.SyncWord = ushort.Parse(this.TextBoxSyncWord.Text, NumberStyles.HexNumber);
      this.DeviceID = uint.Parse(this.TextBoxDeviceID.Text);
      this.RadioTestLoopRepeatResults = (RadioTestLoopResults) null;
      this.TextBlockLoopCounts.Text = "";
      this.SendCount = 0;
      this.LoopCount = 0;
      this.LoopRepeatCount = 0;
      this.Done = false;
      this.TextBoxActiveTest.Text = functionText;
      if (!isManualStop)
        this.TextBoxStatus.Clear();
      this.AddStatusLine(functionText);
      if (!isManualStop)
      {
        this.AddStatusLine("Timeout[s]: " + this.TimoutSeconds.ToString());
        this.StartTime = DateTime.Now;
        this.EndTime = this.StartTime.AddSeconds((double) ((int) this.TimoutSeconds + 5));
      }
      this.CancelTokenSource = new CancellationTokenSource();
      this.CancelToken = this.CancelTokenSource.Token;
      this.ProgressBarRun.Maximum = (double) this.TimoutSeconds;
      this.ProgressBarRun.Value = 0.0;
      this.progress.Reset();
      if (this.Cursor != Cursors.Wait)
        this.Cursor = Cursors.Wait;
      this.ButtonBreak.IsEnabled = true;
      this.DockPanelInput.IsEnabled = false;
      this.StackPanelButtons.IsEnabled = false;
      this.StackPanelTests.IsEnabled = false;
      this.StackPanelFrequency.IsEnabled = false;
    }

    private async Task WaitNextSendTime()
    {
      DateTime nextSendTime = this.StartTime.AddMilliseconds((double) ((long) this.SendCount * (long) this.CycleMilliSeconds));
      do
      {
        int waitMilliSeconds = (int) nextSendTime.Subtract(DateTime.Now).TotalMilliseconds;
        if (waitMilliSeconds > 0)
        {
          if (waitMilliSeconds > 200)
            waitMilliSeconds = 200;
          await Task.Delay(waitMilliSeconds, this.CancelToken);
          if (this.Done)
            goto label_8;
        }
      }
      while (DateTime.Now < nextSendTime);
      goto label_5;
label_8:
      return;
label_5:;
    }

    private async Task SetStopState()
    {
      try
      {
        await this.MyFunctions.SetModeAsync(this.progress, new CancellationTokenSource().Token, (Enum) HandlerFunctionsForProduction.CommonDeviceModes.OperationMode, (byte) 0, (ushort) 10, (ushort) 0, 305419896U, (ushort) 37331, (byte[]) null);
        this.progress.Report("Reset to OperationMode");
        double testSeconds = DateTime.Now.Subtract(this.StartTime).TotalSeconds;
        this.progress.Report("Test time [s]: " + testSeconds.ToString("0.###"));
      }
      catch
      {
      }
      this.PrepareStopState();
    }

    private void PrepareStopState()
    {
      this.Cursor = this.defaultCursor;
      this.progress.Reset();
      this.TextBoxActiveTest.Text = "All tests stopped.";
      this.ButtonBreak.IsEnabled = false;
      this.DockPanelInput.IsEnabled = true;
      this.StackPanelButtons.IsEnabled = true;
      this.StackPanelTests.IsEnabled = true;
      this.StackPanelFrequency.IsEnabled = true;
      this.StartTime = DateTime.Now;
      this.ProgressBarRun.Value = 0.0;
      this.CheckBoxLoop.IsChecked = new bool?(false);
      S4_RadioTestWindow.S4_RadioTestWindowLogger.Trace("Test finished");
    }

    private void ComboBoxMinoConnectComPort_DropDownOpened(object sender, EventArgs e)
    {
      this.SetComPortList();
    }

    private void SetComPortList()
    {
      if (this.BlockRadioComPortSetup)
        return;
      List<ValueItem> availableComPorts = Constants.GetAvailableComPorts();
      int index1 = availableComPorts.FindIndex((Predicate<ValueItem>) (item => item.Value == this.MainComPort));
      if (index1 >= 0)
        availableComPorts.RemoveAt(index1);
      if (availableComPorts.Count == 0)
      {
        this.ComboBoxMinoConnectComPort.Items.Clear();
        this.ComboBoxMinoConnectComPort.Items.Add((object) "MinoConnectUsbRadio not available");
        this.StackPanelButtons.IsEnabled = false;
      }
      else if (this.ComboBoxMinoConnectComPort.SelectedItem == null || !(this.ComboBoxMinoConnectComPort.SelectedItem is ValueItem))
      {
        this.ComboBoxMinoConnectComPort.ItemsSource = (IEnumerable) availableComPorts;
        if (this.RadioTestComPort != null)
        {
          int index2 = availableComPorts.FindIndex((Predicate<ValueItem>) (item => item.Value == this.RadioTestComPort));
          if (index2 >= 0)
            this.ComboBoxMinoConnectComPort.SelectedIndex = index2;
        }
        if (this.ComboBoxMinoConnectComPort.SelectedIndex >= 0)
          return;
        this.ComboBoxMinoConnectComPort.SelectedIndex = 0;
      }
      else
      {
        ValueItem selectedItemBefore = this.ComboBoxMinoConnectComPort.SelectedItem as ValueItem;
        this.ComboBoxMinoConnectComPort.ItemsSource = (IEnumerable) availableComPorts;
        int index3 = availableComPorts.FindIndex((Predicate<ValueItem>) (item => item.Value == selectedItemBefore.Value));
        if (index3 >= 0)
          this.ComboBoxMinoConnectComPort.SelectedIndex = index3;
      }
    }

    private void ComboBoxMinoConnectComPort_SelectionChanged(
      object sender,
      SelectionChangedEventArgs e)
    {
      if (this.BlockRadioComPortSetup)
        return;
      this.RadioTestComPort = (string) null;
      if (this.ComboBoxMinoConnectComPort.SelectedIndex < 0 || !(this.ComboBoxMinoConnectComPort.SelectedItem is ValueItem))
        return;
      this.RadioTestComPort = ((ValueItem) this.ComboBoxMinoConnectComPort.SelectedItem).Value;
      if (!this.MyWindowFunctions.IsPluginObject)
        return;
      PlugInLoader.GmmConfiguration.SetOrUpdateValue("S4_Handler", S4_HandlerWindowFunctions.ConfigVariables.RadioMinoConnectComPort.ToString(), this.RadioTestComPort);
    }

    private async void ButtonOpenRadioMinoConnect_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState("Open radio MinoConnect");
        if (this.RadioTestDevice != null)
          this.CloseRadioMinoConnect();
        else
          await this.OpenRadioMinoConnectAsync();
      }
      catch (Exception ex)
      {
        this.RadioTestDevice = (RadioTestByDevice) null;
        ExceptionViewer.Show(ex);
      }
      await this.SetStopState();
    }

    private async Task OpenRadioMinoConnectAsync()
    {
      RadioTestParameters newRadioTestParameters = (RadioTestParameters) null;
      ushort syncWord = ushort.Parse(this.TextBoxSyncWord.Text, NumberStyles.HexNumber);
      double frequency = double.Parse(this.TextBoxFrequency.Text);
      if (this.RadioTestDevice == null)
      {
        if (this.RadioButtonMinoConnect.IsChecked.Value)
        {
          newRadioTestParameters = new RadioTestParameters(RadioTestByDevice.RadioTestDevice.MinoConnect, frequency, syncWord);
          this.AddStatusLine("MinoConnect prepared for radio communication");
        }
        else
        {
          newRadioTestParameters = new RadioTestParameters(RadioTestByDevice.RadioTestDevice.IUWS, frequency, syncWord);
          this.AddStatusLine("IUWS prepared for radio communication");
        }
      }
      else if (this.RadioButtonMinoConnect.IsChecked.Value != (this.RadioTestDevice.TestParameters.TestDevice == RadioTestByDevice.RadioTestDevice.MinoConnect) || (int) syncWord != (int) this.RadioTestDevice.TestParameters.SyncWord || frequency != this.RadioTestDevice.TestParameters.TestFrequency)
      {
        this.RadioTestDevice.CloseMinoConnect();
        await Task.Delay(500);
        if (this.RadioButtonMinoConnect.IsChecked.Value)
        {
          newRadioTestParameters = new RadioTestParameters(RadioTestByDevice.RadioTestDevice.MinoConnect, frequency, syncWord);
          this.AddStatusLine("Changed to MinoConnect for radio communication");
        }
        else
        {
          newRadioTestParameters = new RadioTestParameters(RadioTestByDevice.RadioTestDevice.IUWS, frequency, syncWord);
          this.AddStatusLine("Changed to IUWS for radio communication");
        }
      }
      if (newRadioTestParameters != null)
      {
        this.RadioTestDevice = new RadioTestByDevice(this.RadioTestComPort, newRadioTestParameters);
        this.RadioTestDevice.SetChannelName("RTD");
        this.MyFunctions.configList.ReadingChannelIdentification = "IUWS";
        this.RadioTestDevice.OpenMinoConnect();
        this.LabelMiConComPort.Background = (Brush) Brushes.LightGreen;
        this.ButtonOpenRadioMinoConnect.Content = (object) "Close radio MinoDevice";
        await Task.Delay(500);
      }
      this.StartTime = DateTime.Now;
      newRadioTestParameters = (RadioTestParameters) null;
    }

    private void CloseRadioMinoConnect()
    {
      if (this.RadioTestDevice == null)
        return;
      if (this.CancelTokenSource != null)
        this.CancelTokenSource.Cancel();
      this.LabelMiConComPort.Background = this.DefaultLabelBrush;
      this.ButtonOpenRadioMinoConnect.Content = (object) this.DefaultOpenText;
      this.RadioTestDevice.CloseMinoConnect();
      this.RadioTestDevice = (RadioTestByDevice) null;
      this.TextBoxStatus.Text = "MinoConnect for radio communication closed";
    }

    private async void ButtonSendUnmodulatedCarrier_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState("Send unmodulated carrier");
        await this.MyFunctions.RadioTests.SendUnmodulatedCarrier(this.progress, this.CancelToken, this.TimoutSeconds);
        this.progress.Report("UnmodulatedCarrierMode activated");
        if (this.TimoutSeconds > (ushort) 0)
        {
          while (true)
          {
            if (await this.MyFunctions.IsTestActive(HandlerFunctionsForProduction.CommonDeviceModes.RadioTestTransmitUnmodulatedCarrier, this.progress, this.CancelToken))
            {
              if (!(DateTime.Now > this.EndTime))
              {
                await Task.Delay(500, this.CancelToken);
                this.progress.Report();
              }
              else
                break;
            }
            else
              goto label_8;
          }
          throw new TimeoutException("Unexpected test time");
label_8:
          this.progress.Report("UnmodulatedCarrierMode stopped by device");
        }
        else
        {
          this.AddStatusLine("No timeout !!! -> Stop by 'Exit mode and get result'");
          this.PrepareStopState();
          return;
        }
      }
      catch (OperationCanceledException ex)
      {
        this.progress.Report("Operation canceled");
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
        this.progress.Report("Operation stopped by exception");
      }
      await this.SetStopState();
    }

    private async void ButtonSendModulatedCarrier_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState("Send modulated carrier");
        await this.MyFunctions.RadioTests.SendModulatedCarrier(this.progress, this.CancelToken, this.TimoutSeconds);
        this.progress.Report("ModulatedCarrierMode activated");
        if (this.TimoutSeconds > (ushort) 0)
        {
          while (true)
          {
            if (await this.MyFunctions.IsTestActive(HandlerFunctionsForProduction.CommonDeviceModes.RadioTestTransmitModulatedCarrier, this.progress, this.CancelToken))
            {
              if (!(DateTime.Now > this.EndTime))
              {
                await Task.Delay(500, this.CancelToken);
                this.progress.Report();
              }
              else
                break;
            }
            else
              goto label_8;
          }
          throw new TimeoutException("Unexpected test time");
label_8:
          this.progress.Report("ModulatedCarrierMode stopped by device");
        }
        else
        {
          this.AddStatusLine("No timeout !!! -> Stop by 'Exit mode and get result'");
          this.PrepareStopState();
          return;
        }
      }
      catch (OperationCanceledException ex)
      {
        this.progress.Report("Operation canceled");
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
        this.progress.Report("Operation stopped by exception");
      }
      await this.SetStopState();
    }

    private async void ButtonSendRadioPackets_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState("Send radio packets");
        this.EndTime = this.StartTime.AddSeconds((double) this.TimoutSeconds);
        this.AddStatusLine("DeviceID:           " + this.DeviceID.ToString());
        this.AddStatusLine("SyncWord:         0x" + this.SyncWord.ToString("X04"));
        this.AddStatusLine("Cycle[ms]:          " + this.CycleMilliSeconds.ToString("X04"));
        this.AddStatusLine("ArbitraryData:    0x" + Util.ByteArrayToHexString(CommonRadioCommands.DefaultAbitraryData));
        RadioTestLoopResults reportMessage = new RadioTestLoopResults();
        byte[] sendPacketBytes = CommonRadioCommands.GetSendTestPacketData((ushort) this.CycleMilliSeconds, (ushort) 1, this.DeviceID, CommonRadioCommands.DefaultAbitraryData, this.SyncWord);
        do
        {
          DateTime nextSendTime = this.StartTime.AddMilliseconds((double) ((long) reportMessage.SendCount * (long) this.CycleMilliSeconds));
          int waitMilliSeconds = (int) nextSendTime.Subtract(DateTime.Now).TotalMilliseconds;
          if (waitMilliSeconds > 0)
            await Task.Delay(waitMilliSeconds, this.CancelToken);
          S4_RadioTestWindow.S4_RadioTestWindowLogger.Trace("Send test packet");
          S4_HandlerFunctions functions = this.MyFunctions;
          ProgressHandler progress = this.progress;
          CancellationToken cancelToken = this.CancelToken;
          // ISSUE: variable of a boxed type
          __Boxed<HandlerFunctionsForProduction.CommonDeviceModes> mode = (Enum) HandlerFunctionsForProduction.CommonDeviceModes.RadioTestSendTestPacket;
          ushort syncWord1 = this.SyncWord;
          int deviceId = (int) this.DeviceID;
          int syncWord2 = (int) syncWord1;
          byte[] arbitraryData = sendPacketBytes;
          await functions.SetModeAsync(progress, cancelToken, (Enum) mode, deviceID: (uint) deviceId, syncWord: (ushort) syncWord2, arbitraryData: arbitraryData);
          ++reportMessage.SendCount;
          this.progress.Report(tag: (object) reportMessage);
        }
        while (!(DateTime.Now > this.EndTime));
        this.progress.Report("Send packet loop stopped");
        reportMessage = (RadioTestLoopResults) null;
        sendPacketBytes = (byte[]) null;
      }
      catch (OperationCanceledException ex)
      {
        this.progress.Report("Operation canceled");
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
        this.progress.Report("Operation stopped by exception");
      }
      await this.SetStopState();
    }

    private async void ButtonTransmitTest_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState("Transmit test");
        await this.OpenRadioMinoConnectAsync();
        if (this.TimoutSeconds > (ushort) byte.MaxValue)
          throw new Exception("Timeout > 8 bits not allowed");
        this.RadioTestLoopRepeatResults = new RadioTestLoopResults();
        while (!this.CancelToken.IsCancellationRequested)
        {
          RadioTestLoopResults radioTestLoopResults = (RadioTestLoopResults) null;
          try
          {
            radioTestLoopResults = await this.MyFunctions.RadioTests.RadioSendTestAsync(this.progress, this.CancelToken, this.RadioTestDevice, this.TimoutSeconds, this.CycleMilliSeconds, this.RequiredReceiveCount);
            this.AddStatusLine(radioTestLoopResults.ToString());
          }
          catch (Exception ex)
          {
            if (this.IsCanceledException(ex))
            {
              this.progress.Report("Operation canceled");
            }
            else
            {
              this.progress.Report("Operation stopped by exception");
              if (!this.CheckBoxLoop.IsChecked.Value)
                ExceptionViewer.Show(ex);
            }
          }
          if (!this.IsLoopBreakRequired(radioTestLoopResults))
            radioTestLoopResults = (RadioTestLoopResults) null;
          else
            break;
        }
        this.ReportLoopRepeatResults();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      await this.SetStopState();
    }

    private bool IsCanceledException(Exception ex)
    {
      if (ex is OperationCanceledException)
        return true;
      return ex.InnerException != null && this.IsCanceledException(ex.InnerException);
    }

    private async void ButtonTransmitRadioPacketsByCycle_Click(object sender, RoutedEventArgs e)
    {
      this.SetRunState("Transmit radio packets by device cycle");
      ushort intervalSeconds = (ushort) (this.CycleMilliSeconds / 1000U);
      if (intervalSeconds == (ushort) 0)
        intervalSeconds = (ushort) 1;
      await this.OpenRadioMinoConnectAsync();
      byte[] arbitraryData = CommonRadioCommands.DefaultAbitraryData;
      this.AddStatusLine("Interval[s]: " + intervalSeconds.ToString());
      this.AddStatusLine("DeviceID:           " + this.DeviceID.ToString());
      this.AddStatusLine("SyncWord:         0x" + this.SyncWord.ToString("X04"));
      this.AddStatusLine("ArbitraryData:    0x" + Util.ByteArrayToHexString(arbitraryData));
      RadioTestLoopResults reportMessage = new RadioTestLoopResults();
      ushort MiConTimeout = this.TimoutSeconds;
      if (MiConTimeout > (ushort) byte.MaxValue)
        MiConTimeout = (ushort) 100;
      byte[] sendPacketBytes = CommonRadioCommands.GetSendTestPacketData((ushort) this.CycleMilliSeconds, (ushort) 1, this.DeviceID, CommonRadioCommands.DefaultAbitraryData, this.SyncWord);
      try
      {
        Task taskReceiver = (Task) null;
        while (DateTime.Now < this.EndTime)
        {
          bool done = false;
          RadioTestResult resultReceiver = (RadioTestResult) null;
          taskReceiver = Task.Run((Action) (() =>
          {
            resultReceiver = this.RadioTestDevice.ReceiveOnePacket((int) this.DeviceID, MiConTimeout, this.SyncWord.ToString("x04"));
            done = true;
          }));
          S4_RadioTestWindow.S4_RadioTestWindowLogger.Trace("MinoConnect receiver active");
          if (reportMessage.SendCount == 0)
          {
            S4_RadioTestWindow.S4_RadioTestWindowLogger.Trace("Set IUW mode SendTestPacket by cycle");
            DateTime sendErrorTime = DateTime.Now.AddSeconds(4.0);
            S4_HandlerFunctions functions = this.MyFunctions;
            ProgressHandler progress = this.progress;
            CancellationToken cancelToken = this.CancelToken;
            // ISSUE: variable of a boxed type
            __Boxed<HandlerFunctionsForProduction.CommonDeviceModes> mode = (Enum) HandlerFunctionsForProduction.CommonDeviceModes.RadioTestSendTestPacket;
            ushort num = intervalSeconds;
            int timoutSeconds = (int) this.TimoutSeconds;
            int interval = (int) num;
            byte[] arbitraryData1 = sendPacketBytes;
            await functions.SetModeAsync(progress, cancelToken, (Enum) mode, timeoutSeconds: (ushort) timoutSeconds, interval: (ushort) interval, arbitraryData: arbitraryData1);
            ++reportMessage.SendCount;
          }
          while (!done)
          {
            await Task.Delay(500, this.CancelToken);
            if (DateTime.Now > this.EndTime)
              break;
          }
          if (resultReceiver != null)
          {
            reportMessage.AddRSSI(resultReceiver.RSSI, new int?((int) resultReceiver.LQI));
            S4_RadioTestWindow.S4_RadioTestWindowLogger.Trace("MinoConnect receive ok.");
            if (resultReceiver.ReceiveBuffer != null)
            {
              int i = 0;
              while (i < resultReceiver.ReceiveBuffer.Length)
                ++i;
            }
          }
          else if (DateTime.Now < this.EndTime)
          {
            S4_RadioTestWindow.S4_RadioTestWindowLogger.Trace("No data received");
            this.progress.Report("No data received");
          }
          this.progress.Report(tag: (object) reportMessage);
        }
        this.TextBoxStatus.AppendText(Environment.NewLine + reportMessage.ToString());
        taskReceiver = (Task) null;
      }
      catch (Exception ex)
      {
        if (this.IsCanceledException(ex))
        {
          this.progress.Report("Operation canceled");
        }
        else
        {
          this.progress.Report("Operation stopped by exception");
          if (!this.CheckBoxLoop.IsChecked.Value)
            ExceptionViewer.Show(ex);
        }
      }
      await this.RadioTestDevice.StopRadioAsync(this.progress);
      await this.SetStopState();
      arbitraryData = (byte[]) null;
      reportMessage = (RadioTestLoopResults) null;
      sendPacketBytes = (byte[]) null;
    }

    private async void ButtonReceiveTest_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState("Receive test");
        await this.OpenRadioMinoConnectAsync();
        if (this.TimoutSeconds > (ushort) byte.MaxValue)
          throw new Exception("Timeout > 8 bits not allowed");
        this.RadioTestLoopRepeatResults = new RadioTestLoopResults();
        while (!this.CancelToken.IsCancellationRequested)
        {
          RadioTestLoopResults radioTestLoopResults = (RadioTestLoopResults) null;
          try
          {
            radioTestLoopResults = await this.MyFunctions.RadioTests.RadioReceiveTestAsync(this.progress, this.CancelToken, this.RadioTestDevice, this.TimoutSeconds, this.CycleMilliSeconds, this.RequiredReceiveCount);
            this.AddStatusLine(radioTestLoopResults.ToString());
          }
          catch (Exception ex)
          {
            if (this.IsCanceledException(ex))
            {
              this.progress.Report("Operation canceled");
            }
            else
            {
              this.progress.Report("Operation stopped by exception");
              if (!this.CheckBoxLoop.IsChecked.Value)
                ExceptionViewer.Show(ex);
            }
          }
          if (!this.IsLoopBreakRequired(radioTestLoopResults))
            radioTestLoopResults = (RadioTestLoopResults) null;
          else
            break;
        }
        this.ReportLoopRepeatResults();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      await this.SetStopState();
    }

    private bool IsLoopBreakRequired(RadioTestLoopResults radioTestLoopResults)
    {
      ++this.LoopCount;
      this.TextBlockLoopCounts.Text = this.LoopCount.ToString();
      this.progress.Report("*" + Environment.NewLine + "********** Loop count: " + this.LoopCount.ToString() + " **********" + Environment.NewLine + "*" + Environment.NewLine);
      ++this.LoopRepeatCount;
      this.RadioTestLoopRepeatResults.AddLoopResults(radioTestLoopResults);
      return !this.CheckBoxLoop.IsChecked.Value;
    }

    private void ReportLoopRepeatResults()
    {
      if (this.LoopRepeatCount <= 1 || this.RadioTestLoopRepeatResults == null || this.RadioTestLoopRepeatResults.TestCount <= 0)
        return;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("#");
      stringBuilder.AppendLine("############ Loop repeat results ############");
      stringBuilder.AppendLine("# Number of loops: " + this.LoopCount.ToString());
      stringBuilder.AppendLine("#");
      stringBuilder.AppendLine(this.RadioTestLoopRepeatResults.ToString());
      this.progress.Report(stringBuilder.ToString());
    }

    private async void ButtonReadFrequencyIncrement_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        int frequencyIncrement = await this.MyFunctions.RadioTests.GetFrequencyIncrementAsync(this.progress, this.CancelToken);
        this.progress.Report("FrequencyIncrement from device[Hz]: " + frequencyIncrement.ToString());
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
        this.progress.Report("Operation stopped by exception");
      }
    }

    private async void ButtonWriteFrequencyIncrement_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        int frequencyIncrement = int.Parse(this.TextBoxFrequencyIncrement.Text);
        await this.MyFunctions.RadioTests.SetFrequencyIncrementAsync(this.progress, this.CancelToken, frequencyIncrement);
        this.progress.Report("FrequencyIncrement set to[Hz]: " + frequencyIncrement.ToString());
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
        this.progress.Report("Operation stopped by exception");
      }
    }

    private async void ButtonReadRadioPower_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        uint power = await this.MyFunctions.RadioTests.GetRadioPowerAsync(this.progress, this.CancelToken);
        this.TextBoxRadioPower.Text = power.ToString();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
        this.progress.Report("Operation stopped by exception");
      }
    }

    private async void ButtonWriteRadioPower_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        uint power = uint.Parse(this.TextBoxRadioPower.Text);
        await this.MyFunctions.RadioTests.SetRadioPowerAsync(this.progress, this.CancelToken, power);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
        this.progress.Report("Operation stopped by exception");
      }
    }

    private void ButtonClear_Click(object sender, RoutedEventArgs e)
    {
      this.TextBoxStatus.Clear();
      this.TextBlockLoopCounts.Text = "";
    }

    private void ButtonSetDefault_Click(object sender, RoutedEventArgs e) => this.InitValues(true);

    private void ButtonStartLogToExcel_Click(object sender, RoutedEventArgs e)
    {
      this.MyFunctions.RadioTests.ClearTestResultLog();
      this.TextBoxStatus.Text = "Excel logger cleared and activated";
    }

    private void ButtonShowExcelLog_Click(object sender, RoutedEventArgs e)
    {
      this.MyFunctions.RadioTests.ShowLog();
    }

    private async void ButtonSetModePackageReceive_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        int receiveBytes = int.Parse(this.TextBoxReceiveBytes.Text);
        this.SetRunState("Set mode -> Receicve one packet");
        this.AddStatusLine("Required byte count: " + receiveBytes.ToString());
        this.AddStatusLine("Required sync word: " + this.SyncWord.ToString("x04"));
        this.AddStatusLine("");
        this.AddStatusLine("LCD: tE69");
        this.AddStatusLine("LCD change to tE6A -> package received");
        this.AddStatusLine("LCD change to tE6b -> receive timout");
        await this.MyFunctions.RadioTests.SetModeReceiveOnePacketAsync(this.progress, this.CancelToken, this.SyncWord, receiveBytes, this.TimoutSeconds);
        this.AddStatusLine("Mode activated");
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.PrepareStopState();
    }

    private async void ButtonExitModeAndGetResult_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.AddStatusLine("");
        this.SetRunState("Exit mode and get results", true);
        string result = await this.MyFunctions.RadioTests.GetReceiveOnePacketResultAsync(this.progress, this.CancelToken);
        this.AddStatusLine(result);
        result = (string) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      await this.SetStopState();
    }

    private async void ButtonSendPacketByMinoConnect_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.AddStatusLine("");
        this.SetRunState("Send test packet by MinoConnect", true);
        await this.OpenRadioMinoConnectAsync();
        string result = this.MyFunctions.RadioTests.SendOnePacketByMiCon(this.progress, this.CancelToken, this.RadioTestDevice);
        this.AddStatusLine(result);
        result = (string) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.PrepareStopState();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/S4_Handler;component/userinterface/s4_radiotestwindow.xaml", UriKind.Relative));
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
          this.gmmCorporateControl1 = (StartupLib.GmmCorporateControl) target;
          break;
        case 3:
          this.ButtonSetDefault = (Button) target;
          this.ButtonSetDefault.Click += new RoutedEventHandler(this.ButtonSetDefault_Click);
          break;
        case 4:
          this.ButtonClear = (Button) target;
          this.ButtonClear.Click += new RoutedEventHandler(this.ButtonClear_Click);
          break;
        case 5:
          this.ButtonBreak = (Button) target;
          this.ButtonBreak.Click += new RoutedEventHandler(this.ButtonBreak_Click);
          break;
        case 6:
          this.StackPanelButtons = (StackPanel) target;
          break;
        case 7:
          this.ButtonOpenRadioMinoConnect = (Button) target;
          this.ButtonOpenRadioMinoConnect.Click += new RoutedEventHandler(this.ButtonOpenRadioMinoConnect_Click);
          break;
        case 8:
          this.ButtonSendUnmodulatedCarrier = (Button) target;
          this.ButtonSendUnmodulatedCarrier.Click += new RoutedEventHandler(this.ButtonSendUnmodulatedCarrier_Click);
          break;
        case 9:
          this.ButtonSendModulatedCarrier = (Button) target;
          this.ButtonSendModulatedCarrier.Click += new RoutedEventHandler(this.ButtonSendModulatedCarrier_Click);
          break;
        case 10:
          this.ButtonSendRadioPackets = (Button) target;
          this.ButtonSendRadioPackets.Click += new RoutedEventHandler(this.ButtonSendRadioPackets_Click);
          break;
        case 11:
          this.ButtonTransmitRadioPacketsByCycle = (Button) target;
          this.ButtonTransmitRadioPacketsByCycle.Click += new RoutedEventHandler(this.ButtonTransmitRadioPacketsByCycle_Click);
          break;
        case 12:
          this.StackPanelTests = (StackPanel) target;
          break;
        case 13:
          this.ButtonTransmitTest = (Button) target;
          this.ButtonTransmitTest.Click += new RoutedEventHandler(this.ButtonTransmitTest_Click);
          break;
        case 14:
          this.ButtonReceiveTest = (Button) target;
          this.ButtonReceiveTest.Click += new RoutedEventHandler(this.ButtonReceiveTest_Click);
          break;
        case 15:
          this.CheckBoxLoop = (CheckBox) target;
          break;
        case 16:
          this.TextBlockLoopCounts = (TextBlock) target;
          break;
        case 17:
          this.StackPanelManualReceive = (StackPanel) target;
          break;
        case 18:
          this.TextBoxReceiveBytes = (TextBox) target;
          break;
        case 19:
          this.ButtonSetModePackageReceive = (Button) target;
          this.ButtonSetModePackageReceive.Click += new RoutedEventHandler(this.ButtonSetModePackageReceive_Click);
          break;
        case 20:
          this.ButtonSendPacketByMinoConnect = (Button) target;
          this.ButtonSendPacketByMinoConnect.Click += new RoutedEventHandler(this.ButtonSendPacketByMinoConnect_Click);
          break;
        case 21:
          this.ButtonExitModeAndGetResult = (Button) target;
          this.ButtonExitModeAndGetResult.Click += new RoutedEventHandler(this.ButtonExitModeAndGetResult_Click);
          break;
        case 22:
          this.StackPanelFrequency = (StackPanel) target;
          break;
        case 23:
          this.TextBoxFrequencyIncrement = (TextBox) target;
          break;
        case 24:
          this.ButtonReadFrequencyIncrement = (Button) target;
          this.ButtonReadFrequencyIncrement.Click += new RoutedEventHandler(this.ButtonReadFrequencyIncrement_Click);
          break;
        case 25:
          this.ButtonWriteFrequencyIncrement = (Button) target;
          this.ButtonWriteFrequencyIncrement.Click += new RoutedEventHandler(this.ButtonWriteFrequencyIncrement_Click);
          break;
        case 26:
          this.StackRadioPower = (StackPanel) target;
          break;
        case 27:
          this.TextBoxRadioPower = (TextBox) target;
          break;
        case 28:
          this.ButtonReadRadioPower = (Button) target;
          this.ButtonReadRadioPower.Click += new RoutedEventHandler(this.ButtonReadRadioPower_Click);
          break;
        case 29:
          this.ButtonWriteRadioPower = (Button) target;
          this.ButtonWriteRadioPower.Click += new RoutedEventHandler(this.ButtonWriteRadioPower_Click);
          break;
        case 30:
          this.ButtonStartLogToExcel = (Button) target;
          this.ButtonStartLogToExcel.Click += new RoutedEventHandler(this.ButtonStartLogToExcel_Click);
          break;
        case 31:
          this.ButtonShowExcelLog = (Button) target;
          this.ButtonShowExcelLog.Click += new RoutedEventHandler(this.ButtonShowExcelLog_Click);
          break;
        case 32:
          this.DockPanelInput = (DockPanel) target;
          break;
        case 33:
          this.ProgressBarRun = (ProgressBar) target;
          break;
        case 34:
          this.RadioButtonMinoConnect = (RadioButton) target;
          break;
        case 35:
          this.RadioButtonIUWS = (RadioButton) target;
          break;
        case 36:
          this.LabelMiConComPort = (Label) target;
          break;
        case 37:
          this.ComboBoxMinoConnectComPort = (ComboBox) target;
          this.ComboBoxMinoConnectComPort.DropDownOpened += new System.EventHandler(this.ComboBoxMinoConnectComPort_DropDownOpened);
          this.ComboBoxMinoConnectComPort.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxMinoConnectComPort_SelectionChanged);
          break;
        case 38:
          this.TextBoxCycleTime = (TextBox) target;
          break;
        case 39:
          this.TextBoxTimeoutTime = (TextBox) target;
          break;
        case 40:
          this.TextBoxSyncWord = (TextBox) target;
          break;
        case 41:
          this.TextBoxDeviceID = (TextBox) target;
          break;
        case 42:
          this.TextBoxRequiredReceiveCount = (TextBox) target;
          break;
        case 43:
          this.TextBoxFrequency = (TextBox) target;
          break;
        case 44:
          this.TextBoxActiveTest = (TextBox) target;
          break;
        case 45:
          this.TextBlockSendCount = (TextBlock) target;
          break;
        case 46:
          this.TextBlockReceiveCount = (TextBlock) target;
          break;
        case 47:
          this.TextBlockPollingCount = (TextBlock) target;
          break;
        case 48:
          this.TextBoxStatus = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
