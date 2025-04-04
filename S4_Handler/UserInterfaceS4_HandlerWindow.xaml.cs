// Decompiled with JetBrains decompiler
// Type: S4_Handler.UserInterface.S4_HandlerWindow
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommonWPF;
using GmmDbLib;
using HandlerLib;
using HandlerLib.NFC;
using Microsoft.Win32;
using ReadoutConfiguration;
using S4_Handler.Functions;
using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
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
  public partial class S4_HandlerWindow : Window, INotifyPropertyChanged, IComponentConnector
  {
    private const string translaterBaseKey = "S4H_";
    private bool IsPlugin;
    private S4_HandlerWindowFunctions myWindowFunctions;
    private S4_HandlerFunctions myFunctions;
    internal string NextPlugin = "";
    private int LcdSwitchCount;
    private const ReadPartsSelection supportedSelections = ReadPartsSelection.LoggersMask | ReadPartsSelection.FirmwareVersion | ReadPartsSelection.Identification | ReadPartsSelection.EnhancedIdentification | ReadPartsSelection.Calibration | ReadPartsSelection.SmartFunctions | ReadPartsSelection.ScenarioConfiguration | ReadPartsSelection.BackupBlocks | ReadPartsSelection.ProtocolOnlyMode;
    private ReadPartsSelection ReadSelectionBits = ReadPartsSelection.All;
    private MenuItem MenuItemProgressDefineMode;
    private MenuItem MenuItemRepeatEndless;
    private MenuItem MenuItemUseNFC_BlockMode;
    private MenuItem MenuItemMapCheckDisabled;
    private MenuItem MenuItemSendNfcCommand;
    private MenuItem MenuItemSendToNfcDevice;
    private MenuItem MenuItemDownlinkTest;
    private MenuItem MenuItemNLogTraceActive;
    private Cursor defaultCursor;
    private CancellationTokenSource cancelTokenSource;
    private ProgressHandler progress;
    private Brush defaultLoadBrush;
    internal Grid GridBackground;
    internal Menu menuMain;
    internal StartupLib.GmmCorporateControl gmmCorporateControl1;
    internal TextBox TextBoxChannel;
    internal TextBlock TextBlockRepeats;
    internal TextBlock TextBlockStatus;
    internal ProgressBar ProgressBar1;
    internal GroupBox GroupBoxType;
    internal TextBox TextBoxTypeInfo;
    internal Button ButtonLoadType;
    internal Button ButtonLoadCompareType;
    internal Button ButtonSaveType;
    internal GroupBox GroupBoxBackup;
    internal TextBox TextBoxBackupInfo;
    internal Button ButtonLoadBackup;
    internal Button ButtonSaveBackup;
    internal Button ButtonWriteClone;
    internal CheckBox CheckBoxUseSecondDB;
    internal Button ButtonLoadLastBackup;
    internal TextBox TextBoxBackupMeterID;
    internal GroupBox GroupBoxConnected;
    internal TextBox TextBoxConnectedInfo;
    internal Button ButtonNextLcdView;
    internal Button ButtonConnect;
    internal Button ButtonReadRange;
    internal Button ButtonRead;
    internal GroupBox GroupBoxWork;
    internal TextBox TextBoxWorkInfo;
    internal Button ButtonWrite;
    internal StackPanel StackPanalButtons2;
    internal Button ButtonConfiguration;
    internal GroupBox GroupBoxMapBased;
    internal Button ButtonDeviceIdentification;
    internal Button ButtonMapData;
    internal Button ButtonOverwriteAndDiagnostic;
    internal Button ButtonUltrasonicWindow;
    internal Button ButtonTimeElapseSimulator;
    internal Button ButtonCurrentMeasurement;
    internal Button ButtonShowSummary;
    internal Button ButtonDeviceHistory;
    internal Button ButtonPrepareReleaseInfo;
    internal GroupBox GroupBoxManagedMap;
    internal Button ButtonShowParameter;
    internal Button ButtonShowMemory;
    internal GroupBox GroupBoxModules;
    internal ListBox ListBoxModules;
    internal Button ButtonReloadModuleList;
    internal StackPanel StackPanalButtons;
    internal Button ButtonReadoutConfig;
    internal Button ButtonCommunicationPort;
    internal Button ButtonCommunicationTest;
    internal GroupBox GroupBoxByProtocol;
    internal StackPanel StackPanelOnlyProtocol;
    internal Button ButtonDeviceData;
    internal Button ButtonMeterMonitor;
    internal Button ButtonHardwareSetupWindow;
    internal Button ButtonTestWindowCommunication;
    internal Button ButtonRadioTests;
    internal GroupBox GroupBoxSingleCommands;
    internal Button Button_CommonCMD;
    internal Button Button_LoraCMD;
    internal Button Button_RadioCMD;
    internal Button Button_MBusCMD;
    internal Button Button_SpecialCMD;
    internal Button ButtonSmartFunctions;
    internal Button ButtonCommunicationScenarios;
    internal Button ButtonTestWindow2;
    internal Button ButtonClear;
    internal Button ButtonBreak;
    private bool _contentLoaded;

    public string ReadingChannelIdentification
    {
      get
      {
        return this.myFunctions.configList != null ? this.myFunctions.configList.ReadingChannelIdentification : "NoCom";
      }
      set
      {
        if (this.myFunctions.configList == null)
          return;
        this.myFunctions.configList.ReadingChannelIdentification = value;
      }
    }

    public S4_HandlerWindow(S4_HandlerWindowFunctions myWindowFunctions, bool isPlugin = false)
    {
      this.myWindowFunctions = myWindowFunctions;
      this.myFunctions = myWindowFunctions.MyFunctions;
      this.IsPlugin = isPlugin;
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.InitializeComponent();
      if (isPlugin)
      {
        try
        {
          GMMConfig gmmConfiguration1 = PlugInLoader.GmmConfiguration;
          S4_HandlerWindowFunctions.ConfigVariables configVariables = S4_HandlerWindowFunctions.ConfigVariables.ReadPartSelection;
          string strVariable1 = configVariables.ToString();
          string s = gmmConfiguration1.GetValue("S4_Handler", strVariable1);
          if (!string.IsNullOrEmpty(s))
            this.ReadSelectionBits = (ReadPartsSelection) uint.Parse(s);
          GMMConfig gmmConfiguration2 = PlugInLoader.GmmConfiguration;
          configVariables = S4_HandlerWindowFunctions.ConfigVariables.MapCheckDisabled;
          string strVariable2 = configVariables.ToString();
          string str = gmmConfiguration2.GetValue("S4_Handler", strVariable2);
          if (str != null)
            this.myFunctions.MapCheckDisabled = bool.Parse(str);
        }
        catch
        {
        }
      }
      MenuItem newItem1 = new MenuItem();
      newItem1.Header = (object) Ot.Gtt(Tg.Handler_UI, "S4H_SF_Config", "Configuration");
      this.menuMain.Items.Add((object) newItem1);
      this.MenuItemMapCheckDisabled = new MenuItem();
      this.MenuItemMapCheckDisabled.Header = (object) "Map check disabled";
      this.MenuItemMapCheckDisabled.Click += new RoutedEventHandler(this.MenuClickMapCheckDisabled);
      this.MenuItemMapCheckDisabled.IsChecked = this.myFunctions.MapCheckDisabled;
      newItem1.Items.Add((object) this.MenuItemMapCheckDisabled);
      newItem1.Items.Add((object) new Separator());
      this.MenuItemProgressDefineMode = new MenuItem();
      this.MenuItemProgressDefineMode.Header = (object) "Progress definition mode";
      this.MenuItemProgressDefineMode.IsCheckable = true;
      newItem1.Items.Add((object) this.MenuItemProgressDefineMode);
      this.MenuItemRepeatEndless = new MenuItem();
      this.MenuItemRepeatEndless.Header = (object) "Repeat endless";
      this.MenuItemRepeatEndless.IsCheckable = true;
      newItem1.Items.Add((object) this.MenuItemRepeatEndless);
      newItem1.Items.Add((object) new Separator());
      this.MenuItemUseNFC_BlockMode = new MenuItem();
      this.MenuItemUseNFC_BlockMode.Header = (object) "Use NFC block mode";
      this.MenuItemUseNFC_BlockMode.IsCheckable = true;
      this.MenuItemUseNFC_BlockMode.IsChecked = this.myFunctions.NFC_BlockMode;
      this.MenuItemUseNFC_BlockMode.Click += new RoutedEventHandler(this.MenuClickUseNFC_BlockMode);
      newItem1.Items.Add((object) this.MenuItemUseNFC_BlockMode);
      newItem1.Items.Add((object) new Separator());
      this.MenuItemSendNfcCommand = new MenuItem();
      this.MenuItemSendNfcCommand.Header = (object) "SendNfcCommand (MBus, no NFC frame)";
      this.MenuItemSendNfcCommand.IsCheckable = true;
      this.MenuItemSendNfcCommand.Click += new RoutedEventHandler(this.MenuClickSendNfcCommand);
      newItem1.Items.Add((object) this.MenuItemSendNfcCommand);
      newItem1.Items.Add((object) new Separator());
      this.MenuItemSendToNfcDevice = new MenuItem();
      this.MenuItemSendToNfcDevice.Header = (object) "SendToNfcDevice (NFC frame in MBus)";
      this.MenuItemSendToNfcDevice.IsCheckable = true;
      this.MenuItemSendToNfcDevice.Click += new RoutedEventHandler(this.MenuClickSendToNfcDevice);
      newItem1.Items.Add((object) this.MenuItemSendToNfcDevice);
      newItem1.Items.Add((object) new Separator());
      this.MenuItemDownlinkTest = new MenuItem();
      this.MenuItemDownlinkTest.Header = (object) "Downlink test";
      this.MenuItemDownlinkTest.IsCheckable = false;
      this.MenuItemDownlinkTest.Click += new RoutedEventHandler(this.MenuClickDownlinkTest);
      newItem1.Items.Add((object) this.MenuItemDownlinkTest);
      this.MenuItemNLogTraceActive = new MenuItem();
      this.MenuItemNLogTraceActive.Header = (object) "NLog trace active for one test";
      this.MenuItemNLogTraceActive.Click += new RoutedEventHandler(this.MenuItemNLogTraceActive_Click);
      this.MenuItemNLogTraceActive.Checked += new RoutedEventHandler(this.MenuItemNLogTraceActive_Checked);
      this.MenuItemNLogTraceActive.Unchecked += new RoutedEventHandler(this.MenuItemNLogTraceActive_Unchecked);
      newItem1.Items.Add((object) this.MenuItemNLogTraceActive);
      MenuItem newItem2 = new MenuItem();
      newItem2.Header = (object) Ot.Gtt(Tg.Handler_UI, "S4H_SF_Menu", "Special functions");
      this.menuMain.Items.Add((object) newItem2);
      MenuItem newItem3 = new MenuItem();
      newItem3.Header = (object) Ot.Gtt(Tg.Handler_UI, "S4H_SF_MenuVersion", "Read device identification");
      newItem3.Click += new RoutedEventHandler(this.MenuClickReadIdentification);
      newItem2.Items.Add((object) newItem3);
      MenuItem newItem4 = new MenuItem();
      newItem4.Header = (object) "Show progress log";
      newItem4.Click += new RoutedEventHandler(this.MenuClickMenuItemShowProgressLog);
      newItem2.Items.Add((object) newItem4);
      newItem2.Items.Add((object) new Separator());
      MenuItem newItem5 = new MenuItem();
      newItem5.Header = (object) "Show read ranges";
      newItem5.Click += new RoutedEventHandler(this.MenuClickMenuItemShowReadRanges);
      newItem2.Items.Add((object) newItem5);
      MenuItem newItem6 = new MenuItem();
      newItem6.Header = (object) "Show read sub-ranges";
      newItem6.Click += new RoutedEventHandler(this.MenuClickMenuItemShowReadSubRanges);
      newItem2.Items.Add((object) newItem6);
      MenuItem newItem7 = new MenuItem();
      newItem7.Header = (object) "Show read sub-ranges and gaps";
      newItem7.Click += new RoutedEventHandler(this.MenuClickMenuItemShowReadSubRangesAndGaps);
      newItem2.Items.Add((object) newItem7);
      newItem2.Items.Add((object) new Separator());
      MenuItem newItem8 = new MenuItem();
      newItem8.Header = (object) "Show write ranges";
      newItem8.Click += new RoutedEventHandler(this.MenuClickMenuItemShowWriteRanges);
      newItem2.Items.Add((object) newItem8);
      newItem2.Items.Add((object) new Separator());
      MenuItem newItem9 = new MenuItem();
      newItem9.Header = (object) "Show available map data ranges";
      newItem9.Click += new RoutedEventHandler(this.MenuClickMenuItemShowAvailableMapDataRanges);
      newItem2.Items.Add((object) newItem9);
      if (!UserManager.CheckPermission(UserManager.Role_Developer))
        this.ButtonTestWindow2.Visibility = Visibility.Collapsed;
      if (isPlugin)
      {
        MenuItem menuItem1 = new MenuItem();
        menuItem1.Header = (object) "Components";
        MenuItem menuItem2 = menuItem1;
        int insertIndex = UserInterfaceServices.AddDefaultMenu(menuItem2, new RoutedEventHandler(this.componentsClick));
        UserInterfaceServices.AddMenuItem("CommunicationPort", menuItem2, new RoutedEventHandler(this.componentsClick));
        UserInterfaceServices.AddMenuItem("ReadoutConfiguration", menuItem2, new RoutedEventHandler(this.componentsClick));
        if (menuItem2.Items.Count > 0)
        {
          if (insertIndex > 0 && menuItem2.Items.Count > insertIndex)
            menuItem2.Items.Insert(insertIndex, (object) new Separator());
          this.menuMain.Items.Add((object) menuItem2);
        }
      }
      MenuItem newItem10 = new MenuItem();
      newItem10.Header = (object) Ot.Gtt(Tg.Handler_UI, "S4H_SF_About", "Info");
      newItem10.Click += new RoutedEventHandler(this.MenuClickShowInformationBox);
      this.menuMain.Items.Add((object) newItem10);
      this.Title = this.Title + " -  Version:" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
      this.SetStopState();
      this.UpdateStatus();
    }

    private void MenuItemNLogTraceActive_Click(object sender, RoutedEventArgs e)
    {
      if (this.MenuItemNLogTraceActive.IsChecked)
        this.MenuItemNLogTraceActive.IsChecked = false;
      else
        this.MenuItemNLogTraceActive.IsChecked = true;
    }

    private void MenuItemNLogTraceActive_Checked(object sender, RoutedEventArgs e)
    {
      NLogSupport.TestBenchTraceActivate();
    }

    private void MenuItemNLogTraceActive_Unchecked(object sender, RoutedEventArgs e)
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
      saveFileDialog.FileName = "NLogOut_" + DateTime.Now.ToString("yyMMdd_HHmm") + ".json";
      bool? nullable = saveFileDialog.ShowDialog();
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return;
      NLogSupport.TestBenchTraceStop(saveFileDialog.FileName);
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      try
      {
        PlugInLoader.GmmConfiguration.SetOrUpdateValue("S4_Handler", S4_HandlerWindowFunctions.ConfigVariables.MapCheckDisabled.ToString(), this.myFunctions.MapCheckDisabled.ToString());
      }
      catch
      {
      }
    }

    protected override void OnActivated(EventArgs e)
    {
      this.UpdateStatus();
      base.OnActivated(e);
    }

    private void componentsClick(object sender, RoutedEventArgs e)
    {
      this.NextPlugin = ((HeaderedItemsControl) sender).Header.ToString();
      if (this.NextPlugin == "Configurator")
        PlugInLoader.GmmConfiguration.SetOrUpdateValue("ConfiguratorSetup", "DeviceName", "IUW");
      this.Close();
    }

    private void MenuClickShowInformationBox(object sender, RoutedEventArgs e)
    {
      S4_InfoWindow s4InfoWindow = new S4_InfoWindow();
      s4InfoWindow.Height = 600.0;
      s4InfoWindow.Width = 1000.0;
      s4InfoWindow.ShowDialog();
    }

    private void SetRunState()
    {
      this.cancelTokenSource = new CancellationTokenSource();
      this.progress.Reset();
      this.progress.Split(new double[2]{ 2.0, 98.0 });
      this.progress.Report("Run");
      this.GroupBoxType.IsEnabled = false;
      this.GroupBoxBackup.IsEnabled = false;
      this.GroupBoxWork.IsEnabled = false;
      this.StackPanalButtons.IsEnabled = false;
      this.ButtonNextLcdView.IsEnabled = false;
      this.ButtonConnect.IsEnabled = false;
      this.ButtonRead.IsEnabled = false;
      this.ButtonReadRange.IsEnabled = false;
      this.ButtonClear.IsEnabled = false;
      this.ButtonBreak.IsEnabled = true;
      if (this.Cursor == Cursors.Wait)
        return;
      this.defaultCursor = this.Cursor;
      this.Cursor = Cursors.Wait;
    }

    private void SetStopState()
    {
      if (this.LcdSwitchCount > 0)
        return;
      this.GroupBoxType.IsEnabled = true;
      this.GroupBoxBackup.IsEnabled = true;
      this.GroupBoxWork.IsEnabled = true;
      this.StackPanalButtons.IsEnabled = true;
      this.ButtonClear.IsEnabled = true;
      this.ButtonBreak.IsEnabled = false;
      this.Cursor = this.defaultCursor;
      this.UpdateStatus();
      if (this.myFunctions.myPort == null)
      {
        this.ButtonReadoutConfig.IsEnabled = false;
        this.ButtonCommunicationPort.IsEnabled = false;
        this.ButtonCommunicationTest.IsEnabled = false;
        this.GroupBoxConnected.IsEnabled = false;
        this.StackPanelOnlyProtocol.IsEnabled = false;
        this.GroupBoxManagedMap.IsEnabled = false;
        this.GroupBoxSingleCommands.IsEnabled = false;
      }
      else
      {
        this.ButtonReadoutConfig.IsEnabled = true;
        this.ButtonCommunicationPort.IsEnabled = true;
        this.ButtonCommunicationTest.IsEnabled = true;
        this.ButtonNextLcdView.IsEnabled = this.myFunctions.myMeters.ConnectedMeter != null || this.myFunctions.myProtocolConfiguration != null;
        if ((this.ReadSelectionBits & ReadPartsSelection.ProtocolOnlyMode) > ~(ReadPartsSelection.Dump | ReadPartsSelection.ProtocolOnlyMode))
        {
          this.TextBoxConnectedInfo.Background = (Brush) Brushes.LightCyan;
          this.ButtonConnect.IsEnabled = false;
        }
        else
        {
          this.TextBoxConnectedInfo.Background = this.Background;
          this.ButtonConnect.IsEnabled = true;
        }
        this.ButtonRead.IsEnabled = true;
        this.ButtonReadRange.IsEnabled = true;
        bool flag = UserManager.CheckPermission(UserManager.Right_ReadOnly);
        this.ButtonWrite.IsEnabled = this.myFunctions.myMeters.ConnectedMeter != null && this.myFunctions.myMeters.WorkMeter != null && !flag || this.myFunctions.myProtocolConfiguration != null && this.myFunctions.myProtocolConfiguration.IsWritePrepared;
        this.ButtonWriteClone.IsEnabled = this.myFunctions.myMeters.ConnectedMeter != null && this.myFunctions.myMeters.BackupMeter != null && !flag;
      }
    }

    private void UpdateStatus()
    {
      this.TextBoxWorkInfo.Clear();
      if (this.myFunctions.myMeters.WorkMeter != null)
      {
        this.GroupBoxMapBased.IsEnabled = true;
        this.ButtonConfiguration.IsEnabled = true;
        this.TextBoxWorkInfo.Text = this.myFunctions.myMeters.WorkMeter.GetStatusText();
      }
      else
      {
        this.GroupBoxMapBased.IsEnabled = false;
        this.ButtonConfiguration.IsEnabled = false;
      }
      this.TextBoxConnectedInfo.Clear();
      if (this.myFunctions.myMeters.ConnectedMeter != null)
      {
        this.StackPanelOnlyProtocol.IsEnabled = true;
        this.TextBoxConnectedInfo.Text = this.myFunctions.myMeters.ConnectedMeter.GetStatusText();
      }
      else
        this.StackPanelOnlyProtocol.IsEnabled = false;
      this.TextBoxBackupInfo.Clear();
      if (this.myFunctions.myMeters.BackupMeter != null)
        this.TextBoxBackupInfo.Text = this.myFunctions.myMeters.BackupMeter.GetStatusText();
      this.TextBoxTypeInfo.Clear();
      if (this.myFunctions.myMeters.TypeMeter != null)
        this.TextBoxTypeInfo.Text = this.myFunctions.myMeters.TypeMeter.GetStatusText();
      if (this.myFunctions.myMeters.ConnectedMeter != null || this.myFunctions.myMeters.WorkMeter != null)
        this.GroupBoxManagedMap.IsEnabled = true;
      else
        this.GroupBoxManagedMap.IsEnabled = false;
      if (this.myFunctions.myProtocolConfiguration != null && this.myFunctions.myProtocolConfiguration.IsReadDone)
      {
        this.ButtonConfiguration.IsEnabled = true;
        this.StackPanelOnlyProtocol.IsEnabled = true;
      }
      bool flag = false;
      try
      {
        ConnectionProfile partialProfile = ReadoutConfigFunctions.GetPartialProfile(this.myFunctions.configList.ConnectionProfileID);
        if (partialProfile != null)
        {
          if (partialProfile.DeviceModel.Parameters != null)
          {
            if (partialProfile.DeviceModel.Parameters[ConnectionProfileParameter.Handler] == "S4_Handler")
              flag = true;
            else if (partialProfile.DeviceModel.Parameters[ConnectionProfileParameter.Handler] == "NFCL_Handler")
              flag = true;
          }
        }
      }
      catch
      {
      }
      if (!flag)
      {
        this.GroupBoxConnected.IsEnabled = false;
        this.TextBoxConnectedInfo.Text = "Illegal readout configuration";
      }
      else
        this.GroupBoxConnected.IsEnabled = true;
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
      this.LcdSwitchCount = 0;
      this.cancelTokenSource.Cancel();
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
      this.SetRunState();
      try
      {
        if (sender == this.ButtonNextLcdView)
        {
          this.ButtonNextLcdView.IsEnabled = true;
          ++this.LcdSwitchCount;
          if (this.LcdSwitchCount > 1)
            return;
          do
          {
            await this.myWindowFunctions.MyFunctions.checkedCommands.SwitchLcd(this.progress, this.cancelTokenSource.Token);
            if (this.LcdSwitchCount > 0)
              --this.LcdSwitchCount;
          }
          while (this.LcdSwitchCount > 0);
        }
        else if (sender == this.ButtonConnect)
        {
          this.TextBoxWorkInfo.Clear();
          this.TextBoxConnectedInfo.Clear();
          int num = await this.myFunctions.ReadDeviceAsync(this.progress, this.cancelTokenSource.Token, ReadPartsSelection.FirmwareVersion);
          this.UpdateStatus();
        }
        else if (sender == this.ButtonRead)
        {
          int repeats = 0;
          do
          {
            if (this.MenuItemRepeatEndless.IsChecked)
            {
              ++repeats;
              this.TextBlockRepeats.Text = "Repeates: " + repeats.ToString();
              this.TextBlockRepeats.Visibility = Visibility.Visible;
            }
            else
              this.TextBlockRepeats.Visibility = Visibility.Collapsed;
            if (this.MenuItemProgressDefineMode.IsChecked)
              this.progress.SplitByReportLoggerTimesString();
            else
              this.progress.SplitByReportLoggerTimesString("M1;1;M2;290;179;M3;148;148;129;M4;509;464;463;466;505;484;487;484;465;463;464;379;M5;486;255;150;149;M6;484;466;463;149;464;464;147;148;M7;150;464;464;463;464;M8;148;128;193;M9;462;465;463;147;M10;130;277;148;212;462;463;464;512;482;485;483;466;463;464;462;149;128;M11;158;486;485;M12;1");
            this.TextBoxWorkInfo.Clear();
            this.TextBoxConnectedInfo.Clear();
            int protocolRepeatCount = await this.myFunctions.ReadDeviceAsync(this.progress, this.cancelTokenSource.Token, this.ReadSelectionBits);
            if (this.MenuItemProgressDefineMode.IsChecked)
            {
              string timeString = this.progress.GetReportLoggerTimesString();
              GmmMessage.Show("Progress time string: " + Environment.NewLine + timeString, "Progress time string generation");
              timeString = (string) null;
            }
            this.UpdateStatus();
            this.TextBlockStatus.Text = "Read done. Protocol repeat count: " + protocolRepeatCount.ToString();
          }
          while (this.MenuItemRepeatEndless.IsChecked);
        }
        else if (sender == this.ButtonWrite)
        {
          int repeats = 0;
          do
          {
            if (this.MenuItemRepeatEndless.IsChecked)
            {
              ++repeats;
              this.TextBlockRepeats.Text = "Repeates: " + repeats.ToString();
              this.TextBlockRepeats.Visibility = Visibility.Visible;
            }
            else
              this.TextBlockRepeats.Visibility = Visibility.Collapsed;
            await this.myFunctions.WriteDeviceAsync(this.progress, this.cancelTokenSource.Token);
          }
          while (this.MenuItemRepeatEndless.IsChecked);
        }
        else if (sender == this.ButtonWriteClone)
          await this.myFunctions.myMeters.WriteClone(this.progress, this.cancelTokenSource.Token);
        else if (sender == this.ButtonLoadType)
        {
          GmmDbLib.MeterInfo meterInfo = TypeWindow.ShowDialog((Window) this, "IUW");
          if (meterInfo != null)
            this.myWindowFunctions.MyFunctions.OpenType(meterInfo.MeterInfoID);
          this.UpdateStatus();
          meterInfo = (GmmDbLib.MeterInfo) null;
        }
        else if (sender == this.ButtonLoadCompareType)
        {
          GmmDbLib.MeterInfo meterInfo = TypeWindow.ShowDialog((Window) this, "IUW");
          if (meterInfo != null)
            this.myWindowFunctions.MyFunctions.OpenCompareType(meterInfo.MeterInfoID);
          this.UpdateStatus();
          meterInfo = (GmmDbLib.MeterInfo) null;
        }
        else if (sender == this.ButtonSaveType)
        {
          if (this.myFunctions.myMeters.WorkMeter != null)
          {
            byte[] compressedData = this.myFunctions.myMeters.WorkMeter.meterMemory.GetCompressedData();
            BaseType baseType = this.myFunctions.myMeters.WorkMeter.BaseType;
            if (baseType == null && this.myFunctions.myMeters.TypeMeter != null)
              baseType = this.myFunctions.myMeters.TypeMeter.BaseType;
            TypeEditorWindow.ShowDialog((Window) this, "IUW", baseType, compressedData, this.myFunctions.myMeters.WorkMeter.DatabaseTypeCreationString);
            compressedData = (byte[]) null;
            baseType = (BaseType) null;
          }
        }
        else
        {
          int num1 = (int) MessageBox.Show("Not implemented command");
        }
      }
      catch (OperationCanceledException ex)
      {
      }
      catch (TimeoutException ex)
      {
        int num = (int) MessageBox.Show("Timeout !!!\nIs your device connected correctly?");
      }
      catch (NfcFrameException ex)
      {
        ExceptionViewer.Show((Exception) ex, "NFC exception");
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.SetStopState();
    }

    private void ButtonClear_Click(object sender, RoutedEventArgs e)
    {
      this.myFunctions.Clear();
      this.TextBoxTypeInfo.Clear();
      this.TextBoxBackupInfo.Clear();
      this.TextBoxConnectedInfo.Clear();
      this.TextBoxWorkInfo.Clear();
      this.UpdateStatus();
    }

    private void ButtonReadoutConfig_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        ReadoutConfigFunctions.ChooseConfiguration(new ReadoutPreferences(this.myFunctions.GetReadoutConfiguration(), ConnectionProfileFilter.CreateHandlerFilter("S4_Handler")));
        this.UpdateStatus();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonCommunicationPort_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.myWindowFunctions.myPort.ShowMainWindow();
        this.onPropertyChanged("ReadingChannelIdentification");
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonCommunicationTest_Click(object sender, RoutedEventArgs e)
    {
      S4_CommunicationTest communicationTest = new S4_CommunicationTest(this.myFunctions);
      communicationTest.Owner = (Window) this;
      communicationTest.Show();
    }

    private void MenuClickSetConfigurationNFC(object sender, RoutedEventArgs e)
    {
      try
      {
        ConfigList configList = this.myFunctions.myPort != null ? this.myFunctions.GetReadoutConfiguration() : throw new Exception("CommunicationPort object not available.");
        string port = configList.Port;
        ConnectionProfile connectionProfile = ReadoutConfigFunctions.Manager.GetConnectionProfile(17);
        configList.Reset(connectionProfile.GetSettingsList());
        configList.Port = port;
        this.myFunctions.myPort.SetReadoutConfiguration(configList);
        this.myFunctions.SetReadoutConfiguration(configList);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void MenuClickSetConfigurationMBus(object sender, RoutedEventArgs e)
    {
      ConfigList configList = this.myFunctions.myPort != null ? this.myFunctions.GetReadoutConfiguration() : throw new Exception("CommunicationPort object not available.");
      string port = configList.Port;
      ConnectionProfile connectionProfile = ReadoutConfigFunctions.Manager.GetConnectionProfile(209);
      configList.Reset(connectionProfile.GetSettingsList());
      configList.Port = port;
      this.myFunctions.myPort.SetReadoutConfiguration(configList);
      this.myFunctions.SetReadoutConfiguration(configList);
    }

    private void MenuClickUseNFC_BlockMode(object sender, RoutedEventArgs e)
    {
      this.myFunctions.NFC_BlockMode = this.MenuItemUseNFC_BlockMode.IsChecked;
    }

    private void MenuClickMapCheckDisabled(object sender, RoutedEventArgs e)
    {
      this.MenuItemMapCheckDisabled.IsChecked = !this.MenuItemMapCheckDisabled.IsChecked;
      this.myFunctions.MapCheckDisabled = this.MenuItemMapCheckDisabled.IsChecked;
    }

    private async void MenuClickSendNfcCommand(object sender, RoutedEventArgs e)
    {
      if (this.myFunctions.myDeviceCommands.CommonNfcCommands.myNfcRepeater.IrDaCommands == null)
      {
        if (this.myFunctions.myDeviceCommands.CommonNfcCommands.myNfcRepeater.IrDaWrapper != NfcRepeater.IrDaCommandWrapper.SendNfcCommand)
        {
          this.myFunctions.myDeviceCommands.CommonNfcCommands.myNfcRepeater.IrDaWrapper = NfcRepeater.IrDaCommandWrapper.Non;
          this.SetRunState();
          DeviceIdentification deviceIdentification = await this.myFunctions.myDeviceCommands.CommonNfcCommands.ReadVersionAsync(this.progress, this.cancelTokenSource.Token);
          this.SetStopState();
          this.myFunctions.myDeviceCommands.CommonNfcCommands.myNfcRepeater.IrDaWrapper = NfcRepeater.IrDaCommandWrapper.SendNfcCommand;
          this.MenuItemSendNfcCommand.IsChecked = true;
          this.GridBackground.Background = (Brush) Brushes.LightPink;
        }
        else
        {
          this.myFunctions.myDeviceCommands.CommonNfcCommands.myNfcRepeater.IrDaWrapper = NfcRepeater.IrDaCommandWrapper.Non;
          this.MenuItemSendNfcCommand.IsChecked = false;
          this.GridBackground.Background = this.Background;
        }
      }
      else
      {
        this.MenuItemSendNfcCommand.IsChecked = false;
        this.GridBackground.Background = this.Background;
      }
      this.MenuItemSendToNfcDevice.IsChecked = false;
      this.MenuItemDownlinkTest.IsChecked = false;
      this.SetStopState();
    }

    private void MenuClickSendToNfcDevice(object sender, RoutedEventArgs e)
    {
      if (this.myFunctions.myDeviceCommands.CommonNfcCommands.myNfcRepeater.IrDaCommands == null)
      {
        if (this.myFunctions.myDeviceCommands.CommonNfcCommands.myNfcRepeater.IrDaWrapper != NfcRepeater.IrDaCommandWrapper.SendToNfcDevice)
        {
          this.myFunctions.myDeviceCommands.CommonNfcCommands.myNfcRepeater.IrDaWrapper = NfcRepeater.IrDaCommandWrapper.SendToNfcDevice;
          this.MenuItemSendToNfcDevice.IsChecked = true;
          this.GridBackground.Background = (Brush) Brushes.LightYellow;
        }
        else
        {
          this.myFunctions.myDeviceCommands.CommonNfcCommands.myNfcRepeater.IrDaWrapper = NfcRepeater.IrDaCommandWrapper.Non;
          this.MenuItemSendToNfcDevice.IsChecked = false;
          this.GridBackground.Background = this.Background;
        }
      }
      else
      {
        this.MenuItemSendToNfcDevice.IsChecked = false;
        this.GridBackground.Background = this.Background;
      }
      this.MenuItemSendNfcCommand.IsChecked = false;
      this.MenuItemDownlinkTest.IsChecked = false;
      this.SetStopState();
    }

    private async void MenuClickDownlinkTest(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.myFunctions.myDeviceCommands.CommonNfcCommands.myNfcRepeater.IrDaCommands == null)
        {
          if (this.myFunctions.myDeviceCommands.CommonNfcCommands.myNfcRepeater.IrDaWrapper != NfcRepeater.IrDaCommandWrapper.DownlinkTest)
          {
            this.myFunctions.myDeviceCommands.CommonNfcCommands.myNfcRepeater.IrDaWrapper = NfcRepeater.IrDaCommandWrapper.Non;
            this.SetRunState();
            DeviceIdentification deviceIdentification = await this.myFunctions.myDeviceCommands.CommonNfcCommands.ReadVersionAsync(this.progress, this.cancelTokenSource.Token);
            byte[] devEUI_Bytes = await this.myFunctions.myDeviceCommands.CMDs_LoRa.GetDevEUIAsync(this.progress, this.cancelTokenSource.Token);
            ulong devEUI = BitConverter.ToUInt64(devEUI_Bytes, 0);
            this.SetStopState();
            this.myFunctions.myDeviceCommands.CommonNfcCommands.myNfcRepeater.IrDaWrapper = NfcRepeater.IrDaCommandWrapper.DownlinkTest;
            this.MenuItemDownlinkTest.IsChecked = true;
            this.myFunctions.myDeviceCommands.CommonNfcCommands.myNfcRepeater.DownLinkWindow = new DownLinkTestWindow(devEUI, this.IsPlugin);
            this.myFunctions.myDeviceCommands.CommonNfcCommands.myNfcRepeater.DownLinkWindow.Owner = (Window) this;
            this.myFunctions.myDeviceCommands.CommonNfcCommands.myNfcRepeater.DownLinkWindow.ShowDialog();
            this.GridBackground.Background = (Brush) Brushes.Azure;
            devEUI_Bytes = (byte[]) null;
          }
          else
          {
            this.myFunctions.myDeviceCommands.CommonNfcCommands.myNfcRepeater.IrDaWrapper = NfcRepeater.IrDaCommandWrapper.Non;
            this.MenuItemDownlinkTest.IsChecked = false;
            this.GridBackground.Background = this.Background;
          }
        }
        else
        {
          this.MenuItemDownlinkTest.IsChecked = false;
          this.GridBackground.Background = this.Background;
        }
        this.MenuItemSendNfcCommand.IsChecked = false;
        this.MenuItemSendToNfcDevice.IsChecked = false;
        this.SetStopState();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Down link test initialisation error.");
        this.myFunctions.myDeviceCommands.CommonNfcCommands.myNfcRepeater.IrDaWrapper = NfcRepeater.IrDaCommandWrapper.Non;
        this.MenuItemDownlinkTest.IsChecked = false;
        this.GridBackground.Background = this.Background;
      }
    }

    private void MenuClickReadIdentification(object sender, RoutedEventArgs e)
    {
      DeviceIdentification deviceIdentification = this.myFunctions.GetDeviceIdentification();
      if (deviceIdentification == null)
        return;
      int num = (int) MessageBox.Show(deviceIdentification.ToString());
    }

    private void MenuClickMenuItemShowProgressLog(object sender, RoutedEventArgs e)
    {
      if (this.progress == null)
        return;
      GmmMessage.Show_Ok(this.progress.ToString());
    }

    private void MenuClickMenuItemShowReadRanges(object sender, RoutedEventArgs e)
    {
      if (this.myFunctions.myMeters == null)
        return;
      List<AddressRangeInfo> readRangesList = this.myFunctions.myMeters.ReadRangesList;
      readRangesList.Sort();
      RangeListVisualisation listVisualisation = new RangeListVisualisation("Read memory ranges", readRangesList);
    }

    private void MenuClickMenuItemShowReadSubRanges(object sender, RoutedEventArgs e)
    {
      if (this.myFunctions.myMeters.WorkMeter == null)
        return;
      List<AddressRangeInfo> readRangesList = this.myFunctions.myMeters.ReadRangesList;
      readRangesList.Sort();
      RangeListVisualisation listVisualisation = new RangeListVisualisation("Read memory sub-ranges", this.myFunctions.myMeters.WorkMeter.meterMemory.MapDef.GetUsedSubRangesInfos(readRangesList));
    }

    private void MenuClickMenuItemShowReadSubRangesAndGaps(object sender, RoutedEventArgs e)
    {
      if (this.myFunctions.myMeters.WorkMeter == null)
        return;
      List<AddressRangeInfo> readRangesList = this.myFunctions.myMeters.ReadRangesList;
      readRangesList.Sort();
      List<AddressRangeInfo> usedSubRangesInfos = this.myFunctions.myMeters.WorkMeter.meterMemory.MapDef.GetUsedSubRangesInfos(readRangesList);
      AddressRangeInfo.AddGaps(usedSubRangesInfos);
      RangeListVisualisation listVisualisation = new RangeListVisualisation("Read memory sub-ranges and gaps", usedSubRangesInfos);
    }

    private void MenuClickMenuItemShowWriteRanges(object sender, RoutedEventArgs e)
    {
      if (this.myFunctions.myMeters.WorkMeter == null)
        return;
      List<AddressRangeInfo> writeRangesList = this.myFunctions.myMeters.WriteRangesList;
      writeRangesList.Sort();
      RangeListVisualisation listVisualisation = new RangeListVisualisation("Write memory ranges", this.myFunctions.myMeters.WorkMeter.meterMemory.MapDef.GetUsedSubRangesInfos(writeRangesList));
    }

    private void MenuClickMenuItemShowAvailableMapDataRanges(object sender, RoutedEventArgs e)
    {
      if (this.myFunctions.myMeters.WorkMeter == null)
        return;
      List<AddressRangeInfo> addressRangeInfos = this.myFunctions.myMeters.WorkMeter.meterMemory.GetAllAddressRangeInfos();
      addressRangeInfos.Sort();
      List<AddressRangeInfo> ranges = new List<AddressRangeInfo>();
      using (List<AddressRangeInfo>.Enumerator enumerator = addressRangeInfos.GetEnumerator())
      {
label_6:
        while (enumerator.MoveNext())
        {
          AddressRangeInfo current = enumerator.Current;
          ranges.Add(current);
          uint address = current.StartAddress;
          while (true)
          {
            AddressRange availableDataRange = this.myFunctions.myMeters.WorkMeter.meterMemory.GetNextAvailableDataRange(address);
            if (availableDataRange != null)
            {
              address = availableDataRange.EndAddress + 1U;
              ranges.Add(new AddressRangeInfo("   available", availableDataRange));
            }
            else
              goto label_6;
          }
        }
      }
      RangeListVisualisation listVisualisation = new RangeListVisualisation("Available memory data", ranges);
    }

    private void ButtonUltrasonicWindow_Click(object sender, RoutedEventArgs e)
    {
      S4_UltrasonicWindows ultrasonicWindows = new S4_UltrasonicWindows(this.myWindowFunctions);
      ultrasonicWindows.Owner = (Window) this;
      ultrasonicWindows.ShowDialog();
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

    private void ButtonTestWindow2_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        new S4_TestWindows2(this.myWindowFunctions).ShowDialog();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private async void ButtonShowParameter_Click(object sender, RoutedEventArgs e)
    {
      bool writeToDevice = true;
      try
      {
        if (this.myFunctions.myMeters.WorkMeter == null)
        {
          int num = await this.myFunctions.ReadDeviceAsync(this.progress, this.cancelTokenSource.Token, ReadPartsSelection.EnhancedIdentification);
        }
        if (this.myFunctions.myMeters.ConnectedMeter == null)
          writeToDevice = false;
        if (this.myFunctions.myMeters.WorkMeter == null)
          throw new Exception("Please connect to a device first.");
        ParameterWindow localParameterWindow = new ParameterWindow((DeviceMemory) this.myFunctions.myMeters.WorkMeter.meterMemory, (BaseMemoryAccess) this.myFunctions.myDeviceCommands, Assembly.GetExecutingAssembly(), this.myFunctions.myMeters.ConnectedMeter != null, writeToDevice);
        localParameterWindow.Owner = (Window) this;
        localParameterWindow.ShowDialog();
        localParameterWindow = (ParameterWindow) null;
        this.UpdateStatus();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private async void ButtonShowMemory_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.myFunctions.myMeters.WorkMeter == null)
        {
          int num = await this.myFunctions.ReadDeviceAsync(this.progress, this.cancelTokenSource.Token, ReadPartsSelection.EnhancedIdentification);
        }
        if (this.myFunctions.myMeters.WorkMeter == null || this.myFunctions.myMeters.WorkMeter.meterMemory == null)
          throw new Exception("Work meter memory not available");
        MemoryViewer localMemoryViewerWindow = new MemoryViewer((DeviceMemory) this.myFunctions.myMeters.WorkMeter.meterMemory, (BaseMemoryAccess) this.myFunctions.myDeviceCommands);
        localMemoryViewerWindow.Owner = (Window) this;
        localMemoryViewerWindow.ShowDialog();
        this.UpdateStatus();
        localMemoryViewerWindow = (MemoryViewer) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonDeviceIdentification_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        new ChangeIdentWindow((DeviceIdentification) this.myFunctions.myMeters.WorkMeter.deviceIdentification).ShowDialog();
        this.UpdateStatus();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonLoadLastBackup_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        int meterID = int.Parse(this.TextBoxBackupMeterID.Text);
        if (this.CheckBoxUseSecondDB.IsChecked.Value)
          this.myFunctions.myMeters.SetBackupMeter(GmmDbLib.MeterData.LoadLastBackup(DbBasis.SecondaryDB.BaseDbConnection, meterID) ?? throw new Exception("No data from second data base"));
        else
          this.myWindowFunctions.MyFunctions.LoadLastBackup(meterID);
        this.UpdateStatus();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error while loading last backup");
      }
    }

    private void ButtonLoadBackup_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.myWindowFunctions.ShowBackUpWindow(this.CheckBoxUseSecondDB.IsChecked.Value);
        this.UpdateStatus();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error while loading Backup");
      }
    }

    private void CheckBoxUseSecondDB_Checked(object sender, RoutedEventArgs e)
    {
      if (!PlugInLoader.InitSecundaryDatabase())
      {
        this.CheckBoxUseSecondDB.IsChecked = new bool?(false);
        GmmMessage.Show("Secondary database not available");
      }
      else
      {
        this.defaultLoadBrush = this.ButtonLoadBackup.Background;
        this.ButtonLoadBackup.Background = (Brush) new SolidColorBrush(Colors.LightBlue);
        this.ButtonLoadLastBackup.Background = this.ButtonLoadBackup.Background;
      }
    }

    private void CheckBoxUseSecondDB_Unchecked(object sender, RoutedEventArgs e)
    {
      if (this.defaultLoadBrush == null)
        return;
      this.ButtonLoadBackup.Background = this.defaultLoadBrush;
      this.ButtonLoadLastBackup.Background = this.defaultLoadBrush;
    }

    private void ButtonSaveBackup_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.myFunctions.myMeters.WorkMeter == null)
          throw new Exception("There is no Meterobject to store to database.");
        this.myFunctions.SaveMeter();
        this.UpdateStatus();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error while saving Backup");
      }
    }

    private void ButtonOverwriteAndCompare_Click(object sender, RoutedEventArgs e)
    {
      OverwriteAndCompare overwriteAndCompare;
      do
      {
        overwriteAndCompare = new OverwriteAndCompare("S4_Handler", this.myFunctions.myMeters.GetAllMeterMemories(), (HandlerFunctionsForProduction) this.myFunctions, true);
        overwriteAndCompare.ShowDialog();
      }
      while (overwriteAndCompare.restartWindow);
    }

    private void ButtonConfiguration_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        ConfiguratorWindow.ShowDialog((Window) this, (IHandler) this.myFunctions);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.SetStopState();
    }

    private void ButtonShowSummary_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        GmmMessage.Show_Ok(this.myFunctions.myMeters.WorkMeter.ToString(), useCourier: true);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private async void ButtonPrepareDeviceHistory_Click(object sender, RoutedEventArgs e)
    {
      this.SetRunState();
      S4_DeviceHistory history = new S4_DeviceHistory(this.myFunctions);
      try
      {
        await history.PrepareHistory(this.progress, this.cancelTokenSource.Token);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      GmmMessage.Show_Ok(history.ToString(), useCourier: true);
      this.SetStopState();
      history = (S4_DeviceHistory) null;
    }

    private void ButtonPrepareReleaseInfo_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        GMMConfig gmmConfig = (GMMConfig) null;
        if (this.myWindowFunctions.IsPluginObject)
          gmmConfig = PlugInLoader.GmmConfiguration;
        S4_ReleaseInfo s4ReleaseInfo = new S4_ReleaseInfo(this.myFunctions.myMeters.WorkMeter, gmmConfig);
        s4ReleaseInfo.Owner = (Window) this;
        s4ReleaseInfo.ShowDialog();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonTestWindowCommunication_Click(object sender, RoutedEventArgs e)
    {
      new S4_TestWindowCommunication(this.myWindowFunctions).ShowDialog();
    }

    private void ButtonReadRange_Click(object sender, RoutedEventArgs e)
    {
      DeviceReadRangeSelection.DefineReadPartsSelections(ReadPartsSelection.LoggersMask | ReadPartsSelection.FirmwareVersion | ReadPartsSelection.Identification | ReadPartsSelection.EnhancedIdentification | ReadPartsSelection.Calibration | ReadPartsSelection.SmartFunctions | ReadPartsSelection.ScenarioConfiguration | ReadPartsSelection.BackupBlocks | ReadPartsSelection.ProtocolOnlyMode, ref this.ReadSelectionBits, (Window) this);
      if (this.myWindowFunctions.IsPluginObject)
        PlugInLoader.GmmConfiguration.SetOrUpdateValue("S4_Handler", S4_HandlerWindowFunctions.ConfigVariables.ReadPartSelection.ToString(), ((uint) this.ReadSelectionBits).ToString());
      this.myFunctions.Clear();
      this.SetStopState();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
    {
      add => this.PropertyChanged += value;
      remove => this.PropertyChanged -= value;
    }

    protected void onPropertyChanged(string PropertyName)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(PropertyName));
    }

    private void ButtonSmartFunctions_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        S4_SmartFunctions s4SmartFunctions = this.myFunctions.myMeters.ConnectedMeter == null ? new S4_SmartFunctions((NfcDeviceCommands) null, this.IsPlugin) : new S4_SmartFunctions(this.myFunctions.checkedNfcCommands, this.IsPlugin);
        s4SmartFunctions.Owner = (Window) this;
        s4SmartFunctions.ShowDialog();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonCommunicationScenarios_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        new S4_ScenarioWindow(this.myFunctions.myMeters.WorkMeter == null ? new S4_ScenarioManager(this.myFunctions.myDeviceCommands) : new S4_ScenarioManager(this.myFunctions.myDeviceCommands, this.myFunctions.myMeters.WorkMeter.meterMemory), this.myFunctions.myMeters.ConnectedMeter != null, this.myWindowFunctions.IsPluginObject).ShowDialog();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonMeterMonitor_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        S4_MeterMonitor s4MeterMonitor = new S4_MeterMonitor(this.myFunctions.checkedCommands);
        s4MeterMonitor.Owner = (Window) this;
        s4MeterMonitor.ShowDialog();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonHardwareSetupWindow_Click(object sender, RoutedEventArgs e)
    {
      S4_HardwareSetupWindow hardwareSetupWindow = new S4_HardwareSetupWindow(this.myWindowFunctions);
      hardwareSetupWindow.Owner = (Window) this;
      hardwareSetupWindow.ShowDialog();
    }

    private void ButtonMapData_Click(object sender, RoutedEventArgs e)
    {
      S4_MapDataWindow s4MapDataWindow = new S4_MapDataWindow(this.myWindowFunctions);
      s4MapDataWindow.Owner = (Window) this;
      s4MapDataWindow.ShowDialog();
    }

    private void ButtonDeviceData_Click(object sender, RoutedEventArgs e)
    {
      S4_DeviceDataWindow deviceDataWindow = new S4_DeviceDataWindow(this.myWindowFunctions);
      deviceDataWindow.Owner = (Window) this;
      deviceDataWindow.ShowDialog();
    }

    private void ButtonRadioTests_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        S4_RadioTestWindow s4RadioTestWindow = new S4_RadioTestWindow(this.myWindowFunctions, this.myFunctions.myPort.GetReadoutConfiguration().Port, this.myWindowFunctions.MyFunctions.myMeters.ConnectedMeter.deviceIdentification.MeterID.Value);
        s4RadioTestWindow.Owner = (Window) this;
        s4RadioTestWindow.ShowDialog();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void Button_CommonCMD_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.myFunctions.myDeviceCommands.CMDs_Device != null)
        {
          CommandWindowCommon commandWindowCommon = new CommandWindowCommon(this.myFunctions.myDeviceCommands.CMDs_Device);
          commandWindowCommon.Owner = (Window) this;
          commandWindowCommon.ShowDialog();
        }
        else
        {
          int num = (int) MessageBox.Show("These commands are not available");
        }
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void Button_LoraCMD_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.myFunctions.myDeviceCommands.CMDs_LoRa != null)
        {
          LoRaCommandWindow loRaCommandWindow = new LoRaCommandWindow(this.myFunctions.myDeviceCommands.CMDs_LoRa);
          loRaCommandWindow.Owner = (Window) this;
          loRaCommandWindow.ShowDialog();
        }
        else
        {
          int num = (int) MessageBox.Show("These commands are not available");
        }
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void Button_RadioCMD_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.myFunctions.myDeviceCommands.CMDs_Radio != null)
        {
          RadioCommandWindow radioCommandWindow = new RadioCommandWindow(this.myFunctions.myDeviceCommands.CMDs_Radio, (IPort) this.myFunctions.myPort);
          radioCommandWindow.Owner = (Window) this;
          radioCommandWindow.ShowDialog();
        }
        else
        {
          int num = (int) MessageBox.Show("These commands are not available");
        }
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void Button_MBusCMD_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.myFunctions.myDeviceCommands.CMDs_MBus != null)
        {
          MBusCommandWindow mbusCommandWindow = new MBusCommandWindow(this.myFunctions.myDeviceCommands.CMDs_MBus);
          mbusCommandWindow.Owner = (Window) this;
          mbusCommandWindow.ShowDialog();
        }
        else
        {
          int num = (int) MessageBox.Show("These commands are not available");
        }
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void Button_SpecialCMD_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.myFunctions.myDeviceCommands.CMDs_Special != null)
        {
          SpecialCommandWindow specialCommandWindow = new SpecialCommandWindow(this.myFunctions.myDeviceCommands.CMDs_Special);
          specialCommandWindow.Owner = (Window) this;
          specialCommandWindow.ShowDialog();
        }
        else
        {
          int num = (int) MessageBox.Show("These commands are not available");
        }
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonCurrentMeasurement_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        S4_CurrentMeasureWindow currentMeasureWindow = new S4_CurrentMeasureWindow(this.myFunctions);
        currentMeasureWindow.Owner = (Window) this;
        currentMeasureWindow.ShowDialog();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ListBoxItemModules_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      try
      {
        ListBoxItem listBoxItem = (ListBoxItem) sender;
        string content = listBoxItem.Content as string;
        S4_ModuleHandlerWindow moduleHandlerWindow = new S4_ModuleHandlerWindow(this.myFunctions, listBoxItem.Tag != null ? (BusModuleInfo) listBoxItem.Tag : new BusModuleInfo());
        moduleHandlerWindow.Owner = (Window) this;
        moduleHandlerWindow.ShowDialog();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private async void ButtonReloadModuleList_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        List<BusModuleInfo> infoList = await this.myFunctions.myDeviceCommands.CommonNfcCommands.ReadBusModuleListAsync(this.progress, this.cancelTokenSource.Token);
        this.ListBoxModules.Items.Clear();
        foreach (BusModuleInfo info in infoList)
        {
          ListBoxItem newItem = new ListBoxItem();
          newItem.Content = (object) info.ToString();
          newItem.Tag = (object) info;
          newItem.MouseDoubleClick += new MouseButtonEventHandler(this.ListBoxItemModules_MouseDoubleClick);
          this.ListBoxModules.Items.Add((object) newItem);
          newItem = (ListBoxItem) null;
        }
        await Task.Delay(0);
        infoList = (List<BusModuleInfo>) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.SetStopState();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/S4_Handler;component/userinterface/s4_handlerwindow.xaml", UriKind.Relative));
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
          this.GridBackground = (Grid) target;
          break;
        case 3:
          this.menuMain = (Menu) target;
          break;
        case 4:
          this.gmmCorporateControl1 = (StartupLib.GmmCorporateControl) target;
          break;
        case 5:
          this.TextBoxChannel = (TextBox) target;
          break;
        case 6:
          this.TextBlockRepeats = (TextBlock) target;
          break;
        case 7:
          this.TextBlockStatus = (TextBlock) target;
          break;
        case 8:
          this.ProgressBar1 = (ProgressBar) target;
          break;
        case 9:
          this.GroupBoxType = (GroupBox) target;
          break;
        case 10:
          this.TextBoxTypeInfo = (TextBox) target;
          break;
        case 11:
          this.ButtonLoadType = (Button) target;
          this.ButtonLoadType.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 12:
          this.ButtonLoadCompareType = (Button) target;
          this.ButtonLoadCompareType.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 13:
          this.ButtonSaveType = (Button) target;
          this.ButtonSaveType.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 14:
          this.GroupBoxBackup = (GroupBox) target;
          break;
        case 15:
          this.TextBoxBackupInfo = (TextBox) target;
          break;
        case 16:
          this.ButtonLoadBackup = (Button) target;
          this.ButtonLoadBackup.Click += new RoutedEventHandler(this.ButtonLoadBackup_Click);
          break;
        case 17:
          this.ButtonSaveBackup = (Button) target;
          this.ButtonSaveBackup.Click += new RoutedEventHandler(this.ButtonSaveBackup_Click);
          break;
        case 18:
          this.ButtonWriteClone = (Button) target;
          this.ButtonWriteClone.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 19:
          this.CheckBoxUseSecondDB = (CheckBox) target;
          this.CheckBoxUseSecondDB.Checked += new RoutedEventHandler(this.CheckBoxUseSecondDB_Checked);
          this.CheckBoxUseSecondDB.Unchecked += new RoutedEventHandler(this.CheckBoxUseSecondDB_Unchecked);
          break;
        case 20:
          this.ButtonLoadLastBackup = (Button) target;
          this.ButtonLoadLastBackup.Click += new RoutedEventHandler(this.ButtonLoadLastBackup_Click);
          break;
        case 21:
          this.TextBoxBackupMeterID = (TextBox) target;
          break;
        case 22:
          this.GroupBoxConnected = (GroupBox) target;
          break;
        case 23:
          this.TextBoxConnectedInfo = (TextBox) target;
          break;
        case 24:
          this.ButtonNextLcdView = (Button) target;
          this.ButtonNextLcdView.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 25:
          this.ButtonConnect = (Button) target;
          this.ButtonConnect.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 26:
          this.ButtonReadRange = (Button) target;
          this.ButtonReadRange.Click += new RoutedEventHandler(this.ButtonReadRange_Click);
          break;
        case 27:
          this.ButtonRead = (Button) target;
          this.ButtonRead.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 28:
          this.GroupBoxWork = (GroupBox) target;
          break;
        case 29:
          this.TextBoxWorkInfo = (TextBox) target;
          break;
        case 30:
          this.ButtonWrite = (Button) target;
          this.ButtonWrite.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 31:
          this.StackPanalButtons2 = (StackPanel) target;
          break;
        case 32:
          this.ButtonConfiguration = (Button) target;
          this.ButtonConfiguration.Click += new RoutedEventHandler(this.ButtonConfiguration_Click);
          break;
        case 33:
          this.GroupBoxMapBased = (GroupBox) target;
          break;
        case 34:
          this.ButtonDeviceIdentification = (Button) target;
          this.ButtonDeviceIdentification.Click += new RoutedEventHandler(this.ButtonDeviceIdentification_Click);
          break;
        case 35:
          this.ButtonMapData = (Button) target;
          this.ButtonMapData.Click += new RoutedEventHandler(this.ButtonMapData_Click);
          break;
        case 36:
          this.ButtonOverwriteAndDiagnostic = (Button) target;
          this.ButtonOverwriteAndDiagnostic.Click += new RoutedEventHandler(this.ButtonOverwriteAndCompare_Click);
          break;
        case 37:
          this.ButtonUltrasonicWindow = (Button) target;
          this.ButtonUltrasonicWindow.Click += new RoutedEventHandler(this.ButtonUltrasonicWindow_Click);
          break;
        case 38:
          this.ButtonTimeElapseSimulator = (Button) target;
          this.ButtonTimeElapseSimulator.Click += new RoutedEventHandler(this.ButtonTimeElapseSimulator_Click);
          break;
        case 39:
          this.ButtonCurrentMeasurement = (Button) target;
          this.ButtonCurrentMeasurement.Click += new RoutedEventHandler(this.ButtonCurrentMeasurement_Click);
          break;
        case 40:
          this.ButtonShowSummary = (Button) target;
          this.ButtonShowSummary.Click += new RoutedEventHandler(this.ButtonShowSummary_Click);
          break;
        case 41:
          this.ButtonDeviceHistory = (Button) target;
          this.ButtonDeviceHistory.Click += new RoutedEventHandler(this.ButtonPrepareDeviceHistory_Click);
          break;
        case 42:
          this.ButtonPrepareReleaseInfo = (Button) target;
          this.ButtonPrepareReleaseInfo.Click += new RoutedEventHandler(this.ButtonPrepareReleaseInfo_Click);
          break;
        case 43:
          this.GroupBoxManagedMap = (GroupBox) target;
          break;
        case 44:
          this.ButtonShowParameter = (Button) target;
          this.ButtonShowParameter.Click += new RoutedEventHandler(this.ButtonShowParameter_Click);
          break;
        case 45:
          this.ButtonShowMemory = (Button) target;
          this.ButtonShowMemory.Click += new RoutedEventHandler(this.ButtonShowMemory_Click);
          break;
        case 46:
          this.GroupBoxModules = (GroupBox) target;
          break;
        case 47:
          this.ListBoxModules = (ListBox) target;
          break;
        case 48:
          ((Control) target).MouseDoubleClick += new MouseButtonEventHandler(this.ListBoxItemModules_MouseDoubleClick);
          break;
        case 49:
          ((Control) target).MouseDoubleClick += new MouseButtonEventHandler(this.ListBoxItemModules_MouseDoubleClick);
          break;
        case 50:
          this.ButtonReloadModuleList = (Button) target;
          this.ButtonReloadModuleList.Click += new RoutedEventHandler(this.ButtonReloadModuleList_Click);
          break;
        case 51:
          this.StackPanalButtons = (StackPanel) target;
          break;
        case 52:
          this.ButtonReadoutConfig = (Button) target;
          this.ButtonReadoutConfig.Click += new RoutedEventHandler(this.ButtonReadoutConfig_Click);
          break;
        case 53:
          this.ButtonCommunicationPort = (Button) target;
          this.ButtonCommunicationPort.Click += new RoutedEventHandler(this.ButtonCommunicationPort_Click);
          break;
        case 54:
          this.ButtonCommunicationTest = (Button) target;
          this.ButtonCommunicationTest.Click += new RoutedEventHandler(this.ButtonCommunicationTest_Click);
          break;
        case 55:
          this.GroupBoxByProtocol = (GroupBox) target;
          break;
        case 56:
          this.StackPanelOnlyProtocol = (StackPanel) target;
          break;
        case 57:
          this.ButtonDeviceData = (Button) target;
          this.ButtonDeviceData.Click += new RoutedEventHandler(this.ButtonDeviceData_Click);
          break;
        case 58:
          this.ButtonMeterMonitor = (Button) target;
          this.ButtonMeterMonitor.Click += new RoutedEventHandler(this.ButtonMeterMonitor_Click);
          break;
        case 59:
          this.ButtonHardwareSetupWindow = (Button) target;
          this.ButtonHardwareSetupWindow.Click += new RoutedEventHandler(this.ButtonHardwareSetupWindow_Click);
          break;
        case 60:
          this.ButtonTestWindowCommunication = (Button) target;
          this.ButtonTestWindowCommunication.Click += new RoutedEventHandler(this.ButtonTestWindowCommunication_Click);
          break;
        case 61:
          this.ButtonRadioTests = (Button) target;
          this.ButtonRadioTests.Click += new RoutedEventHandler(this.ButtonRadioTests_Click);
          break;
        case 62:
          this.GroupBoxSingleCommands = (GroupBox) target;
          break;
        case 63:
          this.Button_CommonCMD = (Button) target;
          this.Button_CommonCMD.Click += new RoutedEventHandler(this.Button_CommonCMD_Click);
          break;
        case 64:
          this.Button_LoraCMD = (Button) target;
          this.Button_LoraCMD.Click += new RoutedEventHandler(this.Button_LoraCMD_Click);
          break;
        case 65:
          this.Button_RadioCMD = (Button) target;
          this.Button_RadioCMD.Click += new RoutedEventHandler(this.Button_RadioCMD_Click);
          break;
        case 66:
          this.Button_MBusCMD = (Button) target;
          this.Button_MBusCMD.Click += new RoutedEventHandler(this.Button_MBusCMD_Click);
          break;
        case 67:
          this.Button_SpecialCMD = (Button) target;
          this.Button_SpecialCMD.Click += new RoutedEventHandler(this.Button_SpecialCMD_Click);
          break;
        case 68:
          this.ButtonSmartFunctions = (Button) target;
          this.ButtonSmartFunctions.Click += new RoutedEventHandler(this.ButtonSmartFunctions_Click);
          break;
        case 69:
          this.ButtonCommunicationScenarios = (Button) target;
          this.ButtonCommunicationScenarios.Click += new RoutedEventHandler(this.ButtonCommunicationScenarios_Click);
          break;
        case 70:
          this.ButtonTestWindow2 = (Button) target;
          this.ButtonTestWindow2.Click += new RoutedEventHandler(this.ButtonTestWindow2_Click);
          break;
        case 71:
          this.ButtonClear = (Button) target;
          this.ButtonClear.Click += new RoutedEventHandler(this.ButtonClear_Click);
          break;
        case 72:
          this.ButtonBreak = (Button) target;
          this.ButtonBreak.Click += new RoutedEventHandler(this.ButtonBreak_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    private enum LocalDeviceCommands
    {
      ConnectDeviceAsync,
      ReadDeviceAsync,
      WriteDeviceAsync,
      ResetDeviceAsync,
      BackupDeviceAsync,
    }
  }
}
