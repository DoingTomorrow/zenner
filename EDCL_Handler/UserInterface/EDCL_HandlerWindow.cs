// Decompiled with JetBrains decompiler
// Type: EDCL_Handler.UserInterface.EDCL_HandlerWindow
// Assembly: EDCL_Handler, Version=2.2.10.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F3010E47-8885-4BE8-8551-D37B09710D3C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDCL_Handler.dll

using CommonWPF;
using GmmDbLib;
using HandlerLib;
using HandlerLib.MapManagement;
using ReadoutConfiguration;
using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace EDCL_Handler.UserInterface
{
  public class EDCL_HandlerWindow : Window, IComponentConnector
  {
    private Cursor defaultCursor;
    private CancellationTokenSource cancelTokenSource;
    private ProgressHandler progress;
    private const string translaterBaseKey = "EDCLoRaH_";
    private EDCL_HandlerWindowFunctions myWindowFunctions;
    private EDCL_HandlerFunctions myFunctions;
    private EDCL_DeviceCommands myCommands;
    private MenuItem MenuItemGetVersion;
    internal bool MapFileAvailable = false;
    internal string NextPlugin = "";
    internal Menu menuMain;
    internal MenuItem MenuItemComponents;
    internal StartupLib.GmmCorporateControl gmmCorporateControl1;
    internal TextBlock TextBlockStatus;
    internal ProgressBar ProgressBar1;
    internal GroupBox GroupBoxType;
    internal TextBox TextBoxTypeInfo;
    internal Button ButtonLoadType;
    internal Button ButtonSaveType;
    internal Button ButtonOverride;
    internal GroupBox GroupBoxBackup;
    internal TextBox TextBoxBackupInfo;
    internal Button ButtonLoadBackup;
    internal Button ButtonSaveBackup;
    internal GroupBox GroupBoxConnected;
    internal TextBox TextBoxConnectedInfo;
    internal Button ButtonConnect;
    internal Button ButtonRead;
    internal CheckBox CheckBoxReadAllBytes;
    internal GroupBox GroupBoxWork;
    internal TextBox TextBoxWorkInfo;
    internal Button ButtonWrite;
    internal StackPanel StackPanalButtons;
    internal Button ButtonReadoutConfig;
    internal Button ButtonCommunicationPort;
    internal Button ButtonShowParameter;
    internal Button ButtonShowIdentification;
    internal Button ButtonShowIdentification1;
    internal Button ButtonShowMemory;
    internal Button ButtonTestWindow1;
    internal Button ButtonTestWindowRadioCMD;
    internal Button ButtonTestWindowLoraCMD;
    internal Button ButtonTestWindowNBIoTCMD;
    internal Button ButtonTestWindowMBusCMD;
    internal Button ButtonTestWindowCommonCMD;
    internal Button ButtonTestWindowSpecialCMD;
    internal Button ButtonResetDevice;
    internal Button ButtonBackupDevice;
    internal Button ButtonRadioTest;
    internal Button ButtonConfigurator;
    internal Button ButtonEncabulator;
    internal Button ButtonClear;
    internal Button ButtonBreak;
    private bool _contentLoaded;

    public EDCL_HandlerWindow(EDCL_HandlerWindowFunctions myWindowFunctions, bool isPlugin = false)
    {
      this.myWindowFunctions = myWindowFunctions;
      this.myFunctions = myWindowFunctions.MyFunctions;
      this.myCommands = this.myFunctions.cmd;
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.InitializeComponent();
      if (isPlugin)
      {
        MenuItem componentsMenuItem = (MenuItem) this.menuMain.Items[0];
        componentsMenuItem.IsEnabled = true;
        UserInterfaceServices.AddDefaultMenu(componentsMenuItem, new RoutedEventHandler(this.componentsClick));
        componentsMenuItem.Items.Add((object) new Separator());
        UserInterfaceServices.AddMenuItem("ReadoutConfiguration", componentsMenuItem, new RoutedEventHandler(this.componentsClick));
      }
      MenuItem newItem = new MenuItem();
      newItem.Header = (object) Ot.Gtt(Tg.Handler_UI, "EDCLoRaH_SF_About", "Info");
      newItem.Click += new RoutedEventHandler(this.MenuClickShowInformationBox);
      this.menuMain.Items.Add((object) newItem);
      MenuItem insertItem = new MenuItem();
      insertItem.Header = (object) Ot.Gtt(Tg.Handler_UI, "EDCLoRaH_SF_Menu", "Special functions");
      this.menuMain.Items.Insert(0, (object) insertItem);
      this.MenuItemGetVersion = new MenuItem();
      this.MenuItemGetVersion.Header = (object) Ot.Gtt(Tg.Handler_UI, "EDCLoRaH_SF_MenuVersion", "Read device identification");
      this.MenuItemGetVersion.Click += new RoutedEventHandler(this.RunCommand_Click);
      insertItem.Items.Add((object) this.MenuItemGetVersion);
      this.Title = this.Title + " " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
      this.UpdateStatus(true);
      this.SetStopState();
      WpfTranslatorSupport.TranslateWindow(Tg.EDCL_Handler, (Window) this);
    }

    private void componentsClick(object sender, RoutedEventArgs e)
    {
      this.NextPlugin = ((HeaderedItemsControl) sender).Header.ToString();
      this.Close();
    }

    private void MenuClickShowInformationBox(object sender, RoutedEventArgs e)
    {
      InfoWindow.ShowDialog((Window) this);
    }

    private void SetRunState()
    {
      this.progress.Reset("connecting...");
      this.cancelTokenSource = new CancellationTokenSource();
      this.progress.Reset();
      this.progress.Split(new double[2]{ 2.0, 98.0 });
      this.progress.Report("Run");
      this.GroupBoxType.IsEnabled = false;
      this.GroupBoxBackup.IsEnabled = false;
      this.GroupBoxConnected.IsEnabled = false;
      this.GroupBoxWork.IsEnabled = false;
      this.StackPanalButtons.IsEnabled = false;
      this.ButtonBreak.IsEnabled = true;
      this.defaultCursor = this.Cursor;
      this.Cursor = Cursors.Wait;
    }

    private void SetStopState()
    {
      this.GroupBoxType.IsEnabled = true;
      this.GroupBoxBackup.IsEnabled = true;
      this.GroupBoxConnected.IsEnabled = true;
      this.GroupBoxWork.IsEnabled = this.MapFileAvailable;
      this.StackPanalButtons.IsEnabled = true;
      this.ButtonBreak.IsEnabled = false;
      this.Cursor = this.defaultCursor;
      this.ButtonCommunicationPort.IsEnabled = true;
      this.ButtonReadoutConfig.IsEnabled = true;
      this.ButtonLoadBackup.IsEnabled = true;
      this.ButtonLoadType.IsEnabled = true;
      this.ButtonSaveType.IsEnabled = this.MapFileAvailable;
      this.ButtonOverride.IsEnabled = this.MapFileAvailable;
      if (this.myFunctions.myMeters.WorkMeter != null)
        this.ButtonWrite.IsEnabled = true;
      bool flag1 = false;
      if (this.myFunctions.myMeters.WorkMeter != null && this.myFunctions.myMeters.BackupMeter != null)
        flag1 = true;
      bool flag2 = this.myCommands.IsDeviceIdentified && this.myFunctions.cmd.ConnectedDeviceVersion.VersionProtocolType == VersionProtocolTypes.Series4;
      this.ButtonRadioTest.IsEnabled = flag2 && this.MapFileAvailable;
      this.ButtonShowMemory.IsEnabled = ((!flag2 ? 0 : (this.MapFileAvailable ? 1 : 0)) | (flag1 ? 1 : 0)) != 0;
      this.ButtonShowParameter.IsEnabled = ((!flag2 ? 0 : (this.MapFileAvailable ? 1 : 0)) | (flag1 ? 1 : 0)) != 0;
      this.ButtonResetDevice.IsEnabled = flag2 && this.MapFileAvailable;
      this.ButtonBackupDevice.IsEnabled = flag2 && this.MapFileAvailable;
      this.ButtonShowIdentification.IsEnabled = ((!flag2 ? 0 : (this.MapFileAvailable ? 1 : 0)) | (flag1 ? 1 : 0)) != 0;
      this.ButtonShowIdentification1.IsEnabled = ((!flag2 ? 0 : (this.MapFileAvailable ? 1 : 0)) | (flag1 ? 1 : 0)) != 0;
      this.ButtonConfigurator.IsEnabled = ((!flag2 ? 0 : (this.MapFileAvailable ? 1 : 0)) | (flag1 ? 1 : 0)) != 0;
      this.ButtonEncabulator.IsEnabled = flag2 && this.MapFileAvailable;
      this.ButtonSaveBackup.IsEnabled = flag2 && this.MapFileAvailable;
      this.ButtonTestWindowRadioCMD.IsEnabled = flag2;
      this.ButtonTestWindowLoraCMD.IsEnabled = flag2;
      this.ButtonTestWindowNBIoTCMD.IsEnabled = flag2;
      this.ButtonTestWindowMBusCMD.IsEnabled = flag2;
      this.ButtonTestWindowCommonCMD.IsEnabled = flag2;
      this.ButtonTestWindowSpecialCMD.IsEnabled = flag2;
      this.progress.Reset();
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
      this.cancelTokenSource.Cancel();
    }

    private async void RunCommand_Click(object sender, RoutedEventArgs e)
    {
      this.SetRunState();
      try
      {
        if (sender == this.MenuItemGetVersion)
        {
          DeviceVersionMBus deviceVersion = await this.myCommands.ReadVersionAsync(this.progress, this.cancelTokenSource.Token);
          if (deviceVersion != null)
          {
            int num = (int) MessageBox.Show(deviceVersion.ToString());
          }
          deviceVersion = (DeviceVersionMBus) null;
        }
        else if (sender == this.ButtonConnect)
        {
          this.TextBoxConnectedInfo.Clear();
          await this.myFunctions.ConnectDeviceAsync(this.progress, this.cancelTokenSource.Token);
          this.MapFileAvailable = true;
          this.UpdateStatus();
        }
        else if (sender == this.ButtonRead)
        {
          bool isDump = this.CheckBoxReadAllBytes.IsChecked.Value;
          ReadPartsSelection parts = isDump ? ReadPartsSelection.Dump : ReadPartsSelection.AllWithoutLogger;
          int num = await this.myFunctions.ReadDeviceAsync(this.progress, this.cancelTokenSource.Token, parts);
          this.MapFileAvailable = true;
          this.UpdateStatus();
        }
        else if (sender == this.ButtonWrite)
          await this.myFunctions.WriteDeviceAsync(this.progress, this.cancelTokenSource.Token);
        else if (sender == this.ButtonResetDevice)
          await this.myCommands.Device.ResetDeviceAsync(this.progress, this.cancelTokenSource.Token);
        else if (sender == this.ButtonLoadType)
        {
          MeterInfo meterInfo = TypeWindow.ShowDialog((Window) this, new string[17]
          {
            "EDC_LoRa868",
            "EDC_LoRa470",
            "Micro_LoRa",
            "Micro_wMBus",
            "EDC_NBIoT",
            "Micro_wMBus_LL",
            "Micro_LoRa_LL",
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
          });
          if (meterInfo != null)
          {
            this.myWindowFunctions.MyFunctions.OpenType(meterInfo.MeterInfoID);
            this.UpdateStatus();
          }
          meterInfo = (MeterInfo) null;
        }
        else if (sender == this.ButtonSaveType)
        {
          bool existWorkMeter = this.myFunctions.myMeters.WorkMeter != null;
          byte[] compressedData = existWorkMeter ? this.myFunctions.myMeters.WorkMeter.meterMemory.GetCompressedData() : (byte[]) null;
          BaseType baseType = existWorkMeter ? this.myFunctions.myMeters.WorkMeter.BaseType : (BaseType) null;
          int? meterInfoID = TypeEditorWindow.ShowDialog((Window) this, new string[17]
          {
            "EDC_LoRa868",
            "EDC_LoRa470",
            "Micro_LoRa",
            "Micro_wMBus",
            "EDC_NBIoT",
            "Micro_wMBus_LL",
            "Micro_LoRa_LL",
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
          }, baseType, compressedData);
          if (meterInfoID.HasValue)
          {
            this.myWindowFunctions.MyFunctions.OpenType(meterInfoID.Value);
            this.UpdateStatus();
          }
          compressedData = (byte[]) null;
          baseType = (BaseType) null;
          meterInfoID = new int?();
        }
        else if (sender == this.ButtonBackupDevice)
          await this.myCommands.Device.BackupDeviceAsync(this.progress, this.cancelTokenSource.Token);
        else if (sender == this.ButtonLoadBackup)
        {
          this.myWindowFunctions.ShowBackUpWindow();
          this.ButtonShowMemory.IsEnabled = this.myFunctions.myMeters.WorkMeter != null;
          this.ButtonShowParameter.IsEnabled = this.myFunctions.myMeters.WorkMeter != null;
          this.UpdateStatus();
        }
        else if (sender == this.ButtonSaveBackup)
        {
          this.myFunctions.SaveMeter();
          this.UpdateStatus();
        }
        else if (sender == this.ButtonTestWindowLoraCMD)
        {
          LoRaCommandWindow window = new LoRaCommandWindow(this.myCommands.LoRa);
          window.ShowDialog();
          window = (LoRaCommandWindow) null;
        }
        else if (sender == this.ButtonTestWindowNBIoTCMD)
        {
          NBIoTCommandWindow window = new NBIoTCommandWindow(this.myCommands.NBIoT);
          window.ShowDialog();
          window = (NBIoTCommandWindow) null;
        }
        else if (sender == this.ButtonTestWindowMBusCMD)
        {
          MBusCommandWindow window = new MBusCommandWindow(this.myCommands.MBusCmd);
          window.ShowDialog();
          window = (MBusCommandWindow) null;
        }
        else if (sender == this.ButtonTestWindowRadioCMD)
        {
          RadioCommandWindow window = new RadioCommandWindow(this.myCommands.Radio, (IPort) this.myWindowFunctions.myPort.portFunctions);
          window.ShowDialog();
          window = (RadioCommandWindow) null;
        }
        else if (sender == this.ButtonShowParameter)
        {
          if (this.myFunctions.myMeters.WorkMeter == null)
            this.myFunctions.myMeters.WorkMeter = new EDCL_Meter();
          this.myWindowFunctions.ShowParameterForDevice();
        }
        else
        {
          if (sender != this.ButtonShowMemory)
            throw new HandlerMessageException("Not implemented RunCommand: " + sender.ToString());
          if (this.myFunctions.myMeters.WorkMeter == null)
            this.myFunctions.myMeters.WorkMeter = new EDCL_Meter();
          this.myWindowFunctions.ShowMemoryViewWindow();
        }
      }
      catch (OperationCanceledException ex)
      {
      }
      catch (MapExceptionClass ex)
      {
        this.MapFileAvailable = false;
        ExceptionViewer.Show((Exception) ex, "EDCL_Handler");
        this.UpdateStatus();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "EDCL_Handler");
      }
      this.SetStopState();
    }

    private void UpdateStatus(bool isInit = false)
    {
      this.TextBoxConnectedInfo.Clear();
      if (this.myFunctions.myMeters.ConnectedMeter != null)
        this.TextBoxConnectedInfo.Text = this.myFunctions.myMeters.ConnectedMeter.deviceIdentification.ToString();
      if (!this.MapFileAvailable && !isInit && this.myFunctions.cmd.ConnectedDeviceVersion != null)
        this.TextBoxConnectedInfo.Text = "Device: " + this.myFunctions.cmd.ConnectedDeviceVersion.FirmwareVersionObj.ToString() + "\r\rMapfile for this firmware not available!!!\rFunctional mode only!!!";
      this.TextBoxWorkInfo.Clear();
      if (this.myFunctions.myMeters.WorkMeter != null)
        this.TextBoxWorkInfo.Text = this.myFunctions.myMeters.WorkMeter.deviceIdentification.ToString();
      this.TextBoxBackupInfo.Clear();
      if (this.myFunctions.myMeters.BackupMeter != null)
        this.TextBoxBackupInfo.Text = this.myFunctions.myMeters.BackupMeter.deviceIdentification.ToString();
      this.TextBoxTypeInfo.Clear();
      if (this.myFunctions.myMeters.TypeMeter == null)
        return;
      this.TextBoxTypeInfo.Text = this.myFunctions.myMeters.TypeMeter.deviceIdentification.ToString();
    }

    private void ButtonClear_Click(object sender, RoutedEventArgs e)
    {
      this.myFunctions.Clear();
      this.TextBoxTypeInfo.Clear();
      this.TextBoxBackupInfo.Clear();
      this.TextBoxConnectedInfo.Clear();
      this.TextBoxWorkInfo.Clear();
    }

    private void ButtonCommunicationPort_Click(object sender, RoutedEventArgs e)
    {
      this.myWindowFunctions.myPort.ShowMainWindow();
    }

    private void ButtonReadoutConfig_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        ReadoutConfigFunctions.ChooseConfiguration(new ReadoutPreferences(this.myFunctions.GetReadoutConfiguration(), ConnectionProfileFilter.CreateHandlerFilter("EDCL_Handler")));
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonTestWindow1_Click(object sender, RoutedEventArgs e)
    {
      new TestWindows((Window) this, this.myWindowFunctions).ShowDialog();
    }

    private void ButtonRadioCommandTestWindow_Click(object sender, RoutedEventArgs e)
    {
      new MBusCommandWindow(this.myCommands.MBusCmd).ShowDialog();
    }

    private void ButtonCommonCommandTestWindow_Click(object sender, RoutedEventArgs e)
    {
      new CommandWindowCommon(this.myCommands.Device).ShowDialog();
    }

    private void ButtonSpecialCommandTestWindow_Click(object sender, RoutedEventArgs e)
    {
      new SpecialCommandWindow(this.myCommands.Special).ShowDialog();
    }

    private void ButtonRadioTest_Click(object sender, RoutedEventArgs e)
    {
      ReceiveTestPacketWindow.ShowDialog((Window) this, (DeviceCommandsMBus) this.myWindowFunctions.MyFunctions.cmd, this.myWindowFunctions.MyFunctions.cmd.Radio);
    }

    private void ButtonEncabulator_Click(object sender, RoutedEventArgs e)
    {
      VolumeMonitor.ShowDialog(this.myWindowFunctions.MyFunctions);
    }

    private void ButtonOverride_Click(object sender, RoutedEventArgs e)
    {
      OverwriteAndCompare overwriteAndCompare;
      do
      {
        overwriteAndCompare = new OverwriteAndCompare("EDCL_Handler", this.myFunctions.myMeters.GetAllMeterMemories(), (HandlerFunctionsForProduction) this.myFunctions);
        overwriteAndCompare.ShowDialog();
      }
      while (overwriteAndCompare.restartWindow);
    }

    private void ButtonConfigurator_Click(object sender, RoutedEventArgs e)
    {
      if (this.myFunctions.myMeters.WorkMeter == null)
        return;
      ConfiguratorWindow.ShowDialog((Window) this, (IHandler) this.myFunctions);
    }

    private void ButtonShowIdentification_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        new ChangeIdentWindow((DeviceIdentification) this.myFunctions.myMeters.WorkMeter.deviceIdentification).ShowDialog();
        this.UpdateStatus();
      }
      catch (Exception ex)
      {
        ErrorMessageBox.ShowDialog((Window) this, "EDCL_Handler", ex);
      }
    }

    private void ButtonShowIdentification1_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        new ChangeIdentWindow((DeviceIdentification) this.myFunctions.myMeters.WorkMeter.deviceIdentification1).ShowDialog();
        this.UpdateStatus();
      }
      catch (Exception ex)
      {
        ErrorMessageBox.ShowDialog((Window) this, "EDCL_Handler", ex);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EDCL_Handler;component/ui/edcl_handlerwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.menuMain = (Menu) target;
          break;
        case 2:
          this.MenuItemComponents = (MenuItem) target;
          break;
        case 3:
          this.gmmCorporateControl1 = (StartupLib.GmmCorporateControl) target;
          break;
        case 4:
          this.TextBlockStatus = (TextBlock) target;
          break;
        case 5:
          this.ProgressBar1 = (ProgressBar) target;
          break;
        case 6:
          this.GroupBoxType = (GroupBox) target;
          break;
        case 7:
          this.TextBoxTypeInfo = (TextBox) target;
          break;
        case 8:
          this.ButtonLoadType = (Button) target;
          this.ButtonLoadType.Click += new RoutedEventHandler(this.RunCommand_Click);
          break;
        case 9:
          this.ButtonSaveType = (Button) target;
          this.ButtonSaveType.Click += new RoutedEventHandler(this.RunCommand_Click);
          break;
        case 10:
          this.ButtonOverride = (Button) target;
          this.ButtonOverride.Click += new RoutedEventHandler(this.ButtonOverride_Click);
          break;
        case 11:
          this.GroupBoxBackup = (GroupBox) target;
          break;
        case 12:
          this.TextBoxBackupInfo = (TextBox) target;
          break;
        case 13:
          this.ButtonLoadBackup = (Button) target;
          this.ButtonLoadBackup.Click += new RoutedEventHandler(this.RunCommand_Click);
          break;
        case 14:
          this.ButtonSaveBackup = (Button) target;
          this.ButtonSaveBackup.Click += new RoutedEventHandler(this.RunCommand_Click);
          break;
        case 15:
          this.GroupBoxConnected = (GroupBox) target;
          break;
        case 16:
          this.TextBoxConnectedInfo = (TextBox) target;
          break;
        case 17:
          this.ButtonConnect = (Button) target;
          this.ButtonConnect.Click += new RoutedEventHandler(this.RunCommand_Click);
          break;
        case 18:
          this.ButtonRead = (Button) target;
          this.ButtonRead.Click += new RoutedEventHandler(this.RunCommand_Click);
          break;
        case 19:
          this.CheckBoxReadAllBytes = (CheckBox) target;
          break;
        case 20:
          this.GroupBoxWork = (GroupBox) target;
          break;
        case 21:
          this.TextBoxWorkInfo = (TextBox) target;
          break;
        case 22:
          this.ButtonWrite = (Button) target;
          this.ButtonWrite.Click += new RoutedEventHandler(this.RunCommand_Click);
          break;
        case 23:
          this.StackPanalButtons = (StackPanel) target;
          break;
        case 24:
          this.ButtonReadoutConfig = (Button) target;
          this.ButtonReadoutConfig.Click += new RoutedEventHandler(this.ButtonReadoutConfig_Click);
          break;
        case 25:
          this.ButtonCommunicationPort = (Button) target;
          this.ButtonCommunicationPort.Click += new RoutedEventHandler(this.ButtonCommunicationPort_Click);
          break;
        case 26:
          this.ButtonShowParameter = (Button) target;
          this.ButtonShowParameter.Click += new RoutedEventHandler(this.RunCommand_Click);
          break;
        case 27:
          this.ButtonShowIdentification = (Button) target;
          this.ButtonShowIdentification.Click += new RoutedEventHandler(this.ButtonShowIdentification_Click);
          break;
        case 28:
          this.ButtonShowIdentification1 = (Button) target;
          this.ButtonShowIdentification1.Click += new RoutedEventHandler(this.ButtonShowIdentification1_Click);
          break;
        case 29:
          this.ButtonShowMemory = (Button) target;
          this.ButtonShowMemory.Click += new RoutedEventHandler(this.RunCommand_Click);
          break;
        case 30:
          this.ButtonTestWindow1 = (Button) target;
          this.ButtonTestWindow1.Click += new RoutedEventHandler(this.ButtonTestWindow1_Click);
          break;
        case 31:
          this.ButtonTestWindowRadioCMD = (Button) target;
          this.ButtonTestWindowRadioCMD.Click += new RoutedEventHandler(this.RunCommand_Click);
          break;
        case 32:
          this.ButtonTestWindowLoraCMD = (Button) target;
          this.ButtonTestWindowLoraCMD.Click += new RoutedEventHandler(this.RunCommand_Click);
          break;
        case 33:
          this.ButtonTestWindowNBIoTCMD = (Button) target;
          this.ButtonTestWindowNBIoTCMD.Click += new RoutedEventHandler(this.RunCommand_Click);
          break;
        case 34:
          this.ButtonTestWindowMBusCMD = (Button) target;
          this.ButtonTestWindowMBusCMD.Click += new RoutedEventHandler(this.RunCommand_Click);
          break;
        case 35:
          this.ButtonTestWindowCommonCMD = (Button) target;
          this.ButtonTestWindowCommonCMD.Click += new RoutedEventHandler(this.ButtonCommonCommandTestWindow_Click);
          break;
        case 36:
          this.ButtonTestWindowSpecialCMD = (Button) target;
          this.ButtonTestWindowSpecialCMD.Click += new RoutedEventHandler(this.ButtonSpecialCommandTestWindow_Click);
          break;
        case 37:
          this.ButtonResetDevice = (Button) target;
          this.ButtonResetDevice.Click += new RoutedEventHandler(this.RunCommand_Click);
          break;
        case 38:
          this.ButtonBackupDevice = (Button) target;
          this.ButtonBackupDevice.Click += new RoutedEventHandler(this.RunCommand_Click);
          break;
        case 39:
          this.ButtonRadioTest = (Button) target;
          this.ButtonRadioTest.Click += new RoutedEventHandler(this.ButtonRadioTest_Click);
          break;
        case 40:
          this.ButtonConfigurator = (Button) target;
          this.ButtonConfigurator.Click += new RoutedEventHandler(this.ButtonConfigurator_Click);
          break;
        case 41:
          this.ButtonEncabulator = (Button) target;
          this.ButtonEncabulator.Click += new RoutedEventHandler(this.ButtonEncabulator_Click);
          break;
        case 42:
          this.ButtonClear = (Button) target;
          this.ButtonClear.Click += new RoutedEventHandler(this.ButtonClear_Click);
          break;
        case 43:
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
