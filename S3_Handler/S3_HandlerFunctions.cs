// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_HandlerFunctions
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using AsyncCom;
using CommunicationPort.UserInterface;
using DeviceCollector;
using GmmDbLib;
using HandlerLib;
using NLog;
using ReadoutConfiguration;
using StartupLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  public class S3_HandlerFunctions : 
    HandlerFunctionsForProduction,
    IHandler,
    IReadoutConfig,
    I_ZR_Component,
    IElectronicDeliveryNote,
    IWindowFunctions
  {
    internal const string S3_HandlerComponentName = "S3_Handler";
    internal const int MaxUndoSteps = 10;
    internal static Logger S3_HandlerFunctionsLogger = LogManager.GetLogger(nameof (S3_HandlerFunctions));
    internal static string Right_AllHandlersEnabled = "Right\\AllHandlersEnabled";
    internal static string Right_S3_HandlerEnabled = "Right\\S3_HandlerEnabled";
    internal static string Right_DeviceCollector = "Plugin\\DeviceCollector";
    internal static string Right_DesignerChangeMenu = "Right\\DesignerChangeMenu";
    internal static string Right_ProfessionalConfig = "Right\\ProfessionalConfig";
    internal static string Role_Developer = "Role\\Developer";
    internal static string Right_ReadOnly = "Right\\ReadOnly";
    internal static string Right_VolumeCalibration = "Right\\ConfiguratorFunction\\Calibration\\VolumeCalibration";
    internal static string Right_TemperatureCalibration = "Right\\ConfiguratorFunction\\Calibration\\TemperatureCalibration";
    internal static string Right_ChangeIdentification = "Right\\ConfiguratorFunction\\ChangeIdentification";
    public static bool[] OverwriteFromType_On_ProductionTypeProgramming;
    public static string[] OverwriteSelectionShortcuts;
    internal bool IsPlugin = false;
    internal string HandlerInstanceName;
    internal int HandlerThreadId;
    internal bool lockLoadedTypeAsBaseType;
    internal bool noAdditionalBaseTypeMissingMessages;
    internal bool BreakRequest = false;
    private bool writeEnabled = false;
    private AsyncOperation asyncOperation = (AsyncOperation) null;
    internal bool useDevelopmentFunctions = false;
    internal bool baseTypeEditMode = false;
    internal bool checkWriteByRead = false;
    internal bool holdBaseBlockAddresses = false;
    internal string PrintConfigPath = SystemValues.LoggDataPath;
    internal bool PrintConfigUseDateTimeFileToken = true;
    internal DataTable SaveMeterDataTabel;
    internal int SelectedSaveMeterDataTableRowIndex;
    internal int MeterInfoIdOfType;
    internal bool _loadLastSettingsOnStart = false;
    internal bool _saveLastSettingsOnExit = false;
    internal bool _usePcTime = false;
    internal bool _meterBackupOnWrite = true;
    internal bool _meterBackupOnRead = true;
    internal bool _SuppressCloneOptimization = false;
    internal bool _onlyOneReadBackupPerDay = true;
    internal bool _useBaseTypeByConfig = true;
    private S3_HandlerWindow HandlerMainWindow;
    internal ConfigList ConfigList;
    internal ADC_Measurement adc_Measurement;
    internal ADC_Calibration adc_Calibration;
    internal TDC_Measurement tdc_Measurement;
    internal VolumeGraphCalibration volumeGraphCalibration;
    internal S3_DatabaseAccess MyDatabase;
    internal S3_AllMeters MyMeters;
    internal LCD_Checker MyLcdChecker;
    internal S3_handlerLoadResult LoadResult;
    internal ProgressHandler progress;
    internal CancellationToken cancelToken;
    public S3_CommandsBase MyCommands;
    private ReadoutConfigFunctions myReadoutConfig;
    private string MessageBaseInfo = string.Empty;

    public static void InitS3HandlerForTestbenches(S3_HandlerFunctions TheHandler)
    {
      TheHandler.loadLastSettingsOnStart = false;
      TheHandler.saveLastSettingsOnExit = false;
      TheHandler.usePcTime = false;
      TheHandler.meterBackupOnRead = false;
      TheHandler.meterBackupOnWrite = false;
      TheHandler.onlyOneReadBackupPerDay = false;
    }

    public static void InitS3HandlerForExternalTestBenches(S3_HandlerFunctions TheHandler)
    {
      TheHandler.loadLastSettingsOnStart = false;
      TheHandler.saveLastSettingsOnExit = false;
      TheHandler.usePcTime = false;
      TheHandler.meterBackupOnRead = true;
      TheHandler.meterBackupOnWrite = false;
      TheHandler.onlyOneReadBackupPerDay = true;
    }

    internal bool WriteEnabled
    {
      get => this.writeEnabled;
      set
      {
        if (value)
        {
          if (UserManager.CheckPermission(S3_HandlerFunctions.Right_ReadOnly))
            this.writeEnabled = false;
          else
            this.writeEnabled = true;
        }
        else
          this.writeEnabled = false;
      }
    }

    public bool NewConfigurationDataAvailable { get; set; }

    public bool loadLastSettingsOnStart
    {
      get => this._loadLastSettingsOnStart;
      set => this._loadLastSettingsOnStart = value;
    }

    public bool saveLastSettingsOnExit
    {
      get => this._saveLastSettingsOnExit;
      set => this._saveLastSettingsOnExit = value;
    }

    public bool usePcTime
    {
      get => this._usePcTime;
      set => this._usePcTime = value;
    }

    public bool meterBackupOnWrite
    {
      get => this._meterBackupOnWrite;
      set => this._meterBackupOnWrite = value;
    }

    public bool meterBackupOnRead
    {
      get => this._meterBackupOnRead;
      set => this._meterBackupOnRead = value;
    }

    public bool SuppressCloneOptimization
    {
      get => this._SuppressCloneOptimization;
      set => this._SuppressCloneOptimization = value;
    }

    public bool onlyOneReadBackupPerDay
    {
      get => this._onlyOneReadBackupPerDay;
      set => this._onlyOneReadBackupPerDay = value;
    }

    public string InstanceName { get; private set; }

    public bool useBaseTypeByConfig
    {
      get => this._useBaseTypeByConfig;
      set => this._useBaseTypeByConfig = value;
    }

    public int UndoCount => this.MyMeters.UndoCount;

    internal ReadoutConfigFunctions MyReadoutConfig
    {
      get
      {
        if (this.myReadoutConfig == null)
          this.myReadoutConfig = (ReadoutConfigFunctions) PlugInLoader.GetPlugIn("ReadoutConfiguration").GetPluginInfo().Interface;
        return this.myReadoutConfig;
      }
    }

    public S3_HandlerFunctions()
    {
      this.BaseConstructor(DbBasis.PrimaryDB, (IDeviceCollector) null, true);
    }

    public S3_HandlerFunctions(ConfigList configList, string InstanceName = null)
    {
      if (InstanceName != null)
        this.InstanceName = InstanceName;
      this.SetReadoutConfiguration(configList);
      this.BaseConstructor(DbBasis.PrimaryDB, (IDeviceCollector) null, true);
    }

    public S3_HandlerFunctions(ConfigList configList, DbBasis databaseToUse, string InstanceName = null)
    {
      if (InstanceName != null)
        this.InstanceName = InstanceName;
      this.SetReadoutConfiguration(configList);
      if (databaseToUse == null)
        throw new Exception("Database == null");
      if (databaseToUse != DbBasis.PrimaryDB && databaseToUse != DbBasis.SecondaryDB)
        throw new Exception("Illegal Database");
      this.BaseConstructor(databaseToUse, (IDeviceCollector) null, true);
    }

    public S3_HandlerFunctions(DbBasis databaseToUse)
    {
      if (databaseToUse == DbBasis.PrimaryDB)
      {
        this.BaseConstructor(databaseToUse, (IDeviceCollector) null, true);
      }
      else
      {
        if (databaseToUse != DbBasis.SecondaryDB)
          throw new Exception("Illegal DbBasis object");
        if (databaseToUse == null)
          throw new Exception("DbBasis.SecondaryDB == null");
        this.BaseConstructor(databaseToUse, (IDeviceCollector) null, true);
      }
    }

    public S3_HandlerFunctions(string InstanceName)
    {
      this.InstanceName = InstanceName;
      if (InstanceName == "PlugIn")
        this.IsPlugin = true;
      this.BaseConstructor(DbBasis.PrimaryDB, (IDeviceCollector) null, true);
    }

    public S3_HandlerFunctions(bool generateCommunicationObjects)
    {
      this.BaseConstructor(DbBasis.PrimaryDB, (IDeviceCollector) null, generateCommunicationObjects);
    }

    public S3_HandlerFunctions(IDeviceCollector deviceCollector)
    {
      this.BaseConstructor(DbBasis.PrimaryDB, deviceCollector, true);
    }

    private void BaseConstructor(
      DbBasis databaseToUse,
      IDeviceCollector deviceCollector,
      bool generateCommunicationObjects)
    {
      if (this.InstanceName == null)
        this.InstanceName = "NameNotDefined";
      try
      {
        this.asyncOperation = AsyncOperationManager.CreateOperation((object) null);
        this.HandlerThreadId = Thread.CurrentThread.ManagedThreadId;
        this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgressHandler));
        this.cancelToken = new CancellationToken();
        if (deviceCollector != null)
          this.MyCommands = (S3_CommandsBase) new S3_CommandsDCAC(deviceCollector as DeviceCollectorFunctions);
        else if (this.IsPlugin | generateCommunicationObjects)
          this.SetCommunicationStructure(this.SelectedCommunicationStructure);
        this.MyDatabase = new S3_DatabaseAccess(this, databaseToUse);
        this.MyMeters = new S3_AllMeters(this);
        this.MyMeters.OnProgress += new EventHandlerEx<int>(this.MyMeters_OnProgress);
        this.ReadConfig();
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Error on load S3_Handler");
        ZR_ClassLibMessages.ShowAndClearErrors();
      }
    }

    public object GetFunctions() => (object) this;

    public string ShowMainWindow() => this.ShowS3_HandlerMainWindow("");

    public S3_HandlerFunctions.CommunicationStructure SelectedCommunicationStructure
    {
      get
      {
        if (this.MyCommands == null)
          return S3_HandlerFunctions.CommunicationStructure.compatible;
        if (this.MyCommands is S3_CommandsDCAC)
          return S3_HandlerFunctions.CommunicationStructure.classicDeviceCollector;
        if (this.MyCommands is S3_CommandsCHANGED)
          return S3_HandlerFunctions.CommunicationStructure.commonDefined;
        if (this.MyCommands is S3_CommandsCOMPATIBLE)
          return S3_HandlerFunctions.CommunicationStructure.compatible;
        throw new Exception("Illegal CommunicationStructure");
      }
    }

    public void SetCommunicationStructure(
      S3_HandlerFunctions.CommunicationStructure communicationStructure)
    {
      if (!this.IsPlugin && this.ConfigList == null)
        throw new Exception("ConfigList not available");
      if (this.MyCommands != null)
      {
        if (communicationStructure == this.SelectedCommunicationStructure)
          return;
        this.MyCommands.Close();
      }
      switch (communicationStructure)
      {
        case S3_HandlerFunctions.CommunicationStructure.classicDeviceCollector:
          if (this.IsPlugin)
          {
            AsyncFunctions asyncFunc = (AsyncFunctions) PlugInLoader.GetPlugIn("AsyncCom").GetPluginInfo().Interface;
            DeviceCollectorFunctions devColFunc = (DeviceCollectorFunctions) PlugInLoader.GetPlugIn("DeviceCollector").GetPluginInfo().Interface;
            devColFunc.OnMessage += new EventHandler<GMM_EventArgs>(this.S3_CommunicationMessage);
            this.MyCommands = (S3_CommandsBase) new S3_CommandsDCAC(devColFunc, asyncFunc);
            this.SetCommunicationMessage(this.MyCommands);
            break;
          }
          AsyncFunctions asyncFunctions = new AsyncFunctions();
          DeviceCollectorFunctions devColFunc1 = new DeviceCollectorFunctions((IAsyncFunctions) asyncFunctions, false);
          devColFunc1.OnMessage += new EventHandler<GMM_EventArgs>(this.S3_CommunicationMessage);
          this.MyCommands = (S3_CommandsBase) new S3_CommandsDCAC(devColFunc1, asyncFunctions);
          this.MyCommands.SetReadoutConfiguration(this.ConfigList);
          break;
        case S3_HandlerFunctions.CommunicationStructure.compatible:
          if (this.IsPlugin)
          {
            this.MyCommands = (S3_CommandsBase) new S3_CommandsCOMPATIBLE(this.progress, this.cancelToken, (CommunicationPortWindowFunctions) PlugInLoader.GetPlugIn("CommunicationPort").GetPluginInfo().Interface);
            this.SetCommunicationMessage(this.MyCommands);
            break;
          }
          CommunicationPortWindowFunctions myPortWinFunction = new CommunicationPortWindowFunctions();
          myPortWinFunction.SetReadoutConfiguration(this.ConfigList);
          this.MyCommands = (S3_CommandsBase) new S3_CommandsCOMPATIBLE(this.progress, this.cancelToken, myPortWinFunction);
          break;
        case S3_HandlerFunctions.CommunicationStructure.commonDefined:
          if (this.IsPlugin)
          {
            this.MyCommands = (S3_CommandsBase) new S3_CommandsCHANGED(this.progress, this.cancelToken, (CommunicationPortWindowFunctions) PlugInLoader.GetPlugIn("CommunicationPort").GetPluginInfo().Interface);
            this.SetCommunicationMessage(this.MyCommands);
            break;
          }
          CommunicationPortWindowFunctions myPortWinFunctions = new CommunicationPortWindowFunctions();
          myPortWinFunctions.SetReadoutConfiguration(this.ConfigList);
          this.MyCommands = (S3_CommandsBase) new S3_CommandsCHANGED(this.progress, this.cancelToken, myPortWinFunctions);
          break;
      }
    }

    private void OnProgressHandler(ProgressArg obj)
    {
      if (this.OnS3_HandlerMessage == null)
        return;
      GMM_EventArgs e = new GMM_EventArgs(GMM_EventArgs.MessageType.MessageAndProgressPercentage);
      if (obj.Tag != null && (GMM_EventArgs.MessageType) obj.Tag == GMM_EventArgs.MessageType.EndMessage)
        e = new GMM_EventArgs(GMM_EventArgs.MessageType.EndMessage);
      e.InfoText = obj.Message;
      e.ProgressPercentage = (int) obj.ProgressPercentage;
      e.InfoNumber = (int) obj.ProgressPercentage;
      this.OnS3_HandlerMessage((object) this, e);
      this.OnS3_HandlerMessage((object) this, new GMM_EventArgs(GMM_EventArgs.MessageType.Alive));
      if (e.Cancel)
        this.BreakRequest = true;
    }

    public void SetCommunicationMessage(S3_CommandsBase theCommands)
    {
      if (theCommands.GetType() == typeof (S3_CommandsDCAC))
        ((S3_CommandsDCAC) this.MyCommands).MyDeviceCollector.OnMessage += new EventHandler<GMM_EventArgs>(this.S3_CommunicationMessage);
      if (theCommands.GetType() == typeof (S3_CommandsCOMPATIBLE))
        ((S3_CommandsCOMPATIBLE) this.MyCommands).myPort.OnMessageEvent += new EventHandler<string>(this.myPort_OnMessageEvent);
      if (!(theCommands.GetType() == typeof (S3_CommandsCHANGED)))
        return;
      ((S3_CommandsCOMPATIBLE) this.MyCommands).myPort.OnMessageEvent += new EventHandler<string>(this.myPort_OnMessageEvent);
    }

    private void myPort_OnMessageEvent(object sender, string TheMessage)
    {
      if (this.OnS3_HandlerMessage == null)
        return;
      GMM_EventArgs e = new GMM_EventArgs(GMM_EventArgs.MessageType.Alive);
      if (TheMessage.Contains("Read:"))
      {
        e = new GMM_EventArgs(GMM_EventArgs.MessageType.PrimaryAddressMessage);
        e.InfoText = TheMessage;
      }
      else if (!TheMessage.Contains("OK"))
      {
        e = new GMM_EventArgs(GMM_EventArgs.MessageType.MessageAndProgressPercentage);
        if (sender.GetType() == typeof (ProgressArg))
          e.ProgressPercentage = Convert.ToInt32(((ProgressArg) sender).ProgressPercentage);
      }
      this.OnS3_HandlerMessage((object) this, e);
      if (e.Cancel)
        this.BreakRequest = true;
    }

    public void GMM_Dispose()
    {
      this.MyCommands.Connection_Dispose();
      if (this.HandlerMainWindow != null)
        this.HandlerMainWindow.Dispose();
      this.WriteConfig();
      this.MyMeters.OnProgress -= new EventHandlerEx<int>(this.MyMeters_OnProgress);
    }

    public override void SetReadoutConfiguration(ConfigList configList)
    {
      if (configList == null)
        throw new ArgumentNullException("ConfigList not defined");
      if (this.ConfigList == null)
      {
        this.ConfigList = configList;
        this.ConfigList.PropertyChanged += new PropertyChangedEventHandler(this.ConfigList_PropertyChanged);
      }
      else if (this.ConfigList != configList)
        throw new ArgumentException("this.configList != configList");
      if (this.MyCommands == null)
        return;
      this.MyCommands.SetReadoutConfiguration(configList);
    }

    private void ConfigList_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
    }

    public override ConfigList GetReadoutConfiguration() => this.ConfigList;

    private void MyMeters_OnProgress(object sender, int e)
    {
      if (this.OnProgress == null)
        return;
      this.OnProgress(sender, e);
    }

    private void ReadConfig()
    {
      if (!this.IsPlugin)
        return;
      try
      {
        GMMConfig gmmConfiguration1 = PlugInLoader.GmmConfiguration;
        S3_HandlerFunctions.S3_HandlerConfigVars handlerConfigVars = S3_HandlerFunctions.S3_HandlerConfigVars.LoadLastSettings;
        string strVariable1 = handlerConfigVars.ToString();
        string str1 = gmmConfiguration1.GetValue("S3_Handler", strVariable1);
        if (str1 != "")
          this._loadLastSettingsOnStart = bool.Parse(str1);
        GMMConfig gmmConfiguration2 = PlugInLoader.GmmConfiguration;
        handlerConfigVars = S3_HandlerFunctions.S3_HandlerConfigVars.PrintConfigPath;
        string strVariable2 = handlerConfigVars.ToString();
        string str2 = gmmConfiguration2.GetValue("S3_Handler", strVariable2);
        if (str2 != "")
          this.PrintConfigPath = str2;
        GMMConfig gmmConfiguration3 = PlugInLoader.GmmConfiguration;
        handlerConfigVars = S3_HandlerFunctions.S3_HandlerConfigVars.PrintConfigUseDateTimeFileToken;
        string strVariable3 = handlerConfigVars.ToString();
        string str3 = gmmConfiguration3.GetValue("S3_Handler", strVariable3);
        if (str3 != "")
          this.PrintConfigUseDateTimeFileToken = bool.Parse(str3);
        if (!this._loadLastSettingsOnStart)
          return;
        GMMConfig gmmConfiguration4 = PlugInLoader.GmmConfiguration;
        handlerConfigVars = S3_HandlerFunctions.S3_HandlerConfigVars.SaveSettings;
        string strVariable4 = handlerConfigVars.ToString();
        string str4 = gmmConfiguration4.GetValue("S3_Handler", strVariable4);
        if (str4 != "")
          this._saveLastSettingsOnExit = bool.Parse(str4);
        GMMConfig gmmConfiguration5 = PlugInLoader.GmmConfiguration;
        handlerConfigVars = S3_HandlerFunctions.S3_HandlerConfigVars.UsePcTime;
        string strVariable5 = handlerConfigVars.ToString();
        string str5 = gmmConfiguration5.GetValue("S3_Handler", strVariable5);
        if (str5 != "")
          this._usePcTime = bool.Parse(str5);
        GMMConfig gmmConfiguration6 = PlugInLoader.GmmConfiguration;
        handlerConfigVars = S3_HandlerFunctions.S3_HandlerConfigVars.MeterBackupOnWrite;
        string strVariable6 = handlerConfigVars.ToString();
        string str6 = gmmConfiguration6.GetValue("S3_Handler", strVariable6);
        if (str6 != "")
          this._meterBackupOnWrite = bool.Parse(str6);
        GMMConfig gmmConfiguration7 = PlugInLoader.GmmConfiguration;
        handlerConfigVars = S3_HandlerFunctions.S3_HandlerConfigVars.MeterBackupOnRead;
        string strVariable7 = handlerConfigVars.ToString();
        string str7 = gmmConfiguration7.GetValue("S3_Handler", strVariable7);
        if (str7 != "")
          this._meterBackupOnRead = bool.Parse(str7);
        GMMConfig gmmConfiguration8 = PlugInLoader.GmmConfiguration;
        handlerConfigVars = S3_HandlerFunctions.S3_HandlerConfigVars.OnlyOneReadBackupPerDay;
        string strVariable8 = handlerConfigVars.ToString();
        string str8 = gmmConfiguration8.GetValue("S3_Handler", strVariable8);
        if (str8 != "")
          this._onlyOneReadBackupPerDay = bool.Parse(str8);
        GMMConfig gmmConfiguration9 = PlugInLoader.GmmConfiguration;
        handlerConfigVars = S3_HandlerFunctions.S3_HandlerConfigVars.CheckWriteByRead;
        string strVariable9 = handlerConfigVars.ToString();
        string str9 = gmmConfiguration9.GetValue("S3_Handler", strVariable9);
        if (str9 != "")
          this.checkWriteByRead = bool.Parse(str9);
        GMMConfig gmmConfiguration10 = PlugInLoader.GmmConfiguration;
        handlerConfigVars = S3_HandlerFunctions.S3_HandlerConfigVars.UseBaseTypeByConfig;
        string strVariable10 = handlerConfigVars.ToString();
        string str10 = gmmConfiguration10.GetValue("S3_Handler", strVariable10);
        if (str10 != "")
          this._useBaseTypeByConfig = bool.Parse(str10);
        GMMConfig gmmConfiguration11 = PlugInLoader.GmmConfiguration;
        handlerConfigVars = S3_HandlerFunctions.S3_HandlerConfigVars.LastTypeMeterInfoId;
        string strVariable11 = handlerConfigVars.ToString();
        string s = gmmConfiguration11.GetValue("S3_Handler", strVariable11);
        if (s != "")
          this.MeterInfoIdOfType = int.Parse(s);
        if (this.IsHandlerCompleteEnabled())
        {
          GMMConfig gmmConfiguration12 = PlugInLoader.GmmConfiguration;
          handlerConfigVars = S3_HandlerFunctions.S3_HandlerConfigVars.UseDevelopmentFunctions;
          string strVariable12 = handlerConfigVars.ToString();
          string str11 = gmmConfiguration12.GetValue("S3_Handler", strVariable12);
          if (str11 != "")
            this.useDevelopmentFunctions = bool.Parse(str11);
        }
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    internal void WriteConfig()
    {
      if (!this.IsPlugin)
        return;
      PlugInLoader.GmmConfiguration.SetOrUpdateValue("S3_Handler", S3_HandlerFunctions.S3_HandlerConfigVars.LoadLastSettings.ToString(), this._loadLastSettingsOnStart.ToString());
      PlugInLoader.GmmConfiguration.SetOrUpdateValue("S3_Handler", S3_HandlerFunctions.S3_HandlerConfigVars.SaveSettings.ToString(), this._saveLastSettingsOnExit.ToString());
      PlugInLoader.GmmConfiguration.SetOrUpdateValue("S3_Handler", S3_HandlerFunctions.S3_HandlerConfigVars.PrintConfigPath.ToString(), this.PrintConfigPath);
      PlugInLoader.GmmConfiguration.SetOrUpdateValue("S3_Handler", S3_HandlerFunctions.S3_HandlerConfigVars.PrintConfigUseDateTimeFileToken.ToString(), this.PrintConfigUseDateTimeFileToken.ToString());
      PlugInLoader.GmmConfiguration.SetOrUpdateValue("S3_Handler", S3_HandlerFunctions.S3_HandlerConfigVars.CommunicationStructure.ToString(), this.SelectedCommunicationStructure.ToString());
      if (!this._saveLastSettingsOnExit)
        return;
      PlugInLoader.GmmConfiguration.SetOrUpdateValue("S3_Handler", S3_HandlerFunctions.S3_HandlerConfigVars.UsePcTime.ToString(), this._usePcTime.ToString());
      PlugInLoader.GmmConfiguration.SetOrUpdateValue("S3_Handler", S3_HandlerFunctions.S3_HandlerConfigVars.MeterBackupOnWrite.ToString(), this._meterBackupOnWrite.ToString());
      PlugInLoader.GmmConfiguration.SetOrUpdateValue("S3_Handler", S3_HandlerFunctions.S3_HandlerConfigVars.MeterBackupOnRead.ToString(), this._meterBackupOnRead.ToString());
      PlugInLoader.GmmConfiguration.SetOrUpdateValue("S3_Handler", S3_HandlerFunctions.S3_HandlerConfigVars.OnlyOneReadBackupPerDay.ToString(), this._onlyOneReadBackupPerDay.ToString());
      PlugInLoader.GmmConfiguration.SetOrUpdateValue("S3_Handler", S3_HandlerFunctions.S3_HandlerConfigVars.CheckWriteByRead.ToString(), this.checkWriteByRead.ToString());
      PlugInLoader.GmmConfiguration.SetOrUpdateValue("S3_Handler", S3_HandlerFunctions.S3_HandlerConfigVars.UseBaseTypeByConfig.ToString(), this._useBaseTypeByConfig.ToString());
      PlugInLoader.GmmConfiguration.SetOrUpdateValue("S3_Handler", S3_HandlerFunctions.S3_HandlerConfigVars.LastTypeMeterInfoId.ToString(), this.MeterInfoIdOfType.ToString());
      if (!this.IsHandlerCompleteEnabled())
        return;
      PlugInLoader.GmmConfiguration.SetOrUpdateValue("S3_Handler", S3_HandlerFunctions.S3_HandlerConfigVars.UseDevelopmentFunctions.ToString(), this.useDevelopmentFunctions.ToString());
    }

    internal bool IsHandlerCompleteEnabled()
    {
      return UserManager.IsNewLicenseModel() && (UserManager.CheckPermission("Right\\S3_HandlerEnabled") || UserManager.CheckPermission("Right\\AllHandlersEnabled"));
    }

    public bool Undo() => this.MyMeters.Undo();

    public string OpenTypeByWindow()
    {
      return new S3_Handler.OpenType(this).ShowDialog() == DialogResult.OK ? this.MyMeters.TypeMeter.MyIdentification.GetDeviceIdInfo() : string.Empty;
    }

    public bool ChangeTypeAndWrite() => this.MyMeters.OverwriteWorkFromType();

    public void ShowS3_HandlerMainWindow() => this.ShowS3_HandlerMainWindow("");

    public string ShowS3_HandlerMainWindow(string dummy)
    {
      if (this.HandlerMainWindow == null)
        this.HandlerMainWindow = new S3_HandlerWindow(this);
      ZR_ClassLibMessages.ClearErrors();
      this.HandlerMainWindow.StartComponentName = string.Empty;
      int num = (int) this.HandlerMainWindow.ShowDialog();
      return this.HandlerMainWindow.StartComponentName;
    }

    public event EventHandler<GMM_EventArgs> OnS3_HandlerMessage;

    public event EventHandlerEx<int> OnProgress;

    public void SetMessageInfo(string info) => this.MessageBaseInfo = info;

    internal void SendMessage(GMM_EventArgs.MessageType MessageType)
    {
      if (S3_HandlerFunctions.S3_HandlerFunctionsLogger.IsTraceEnabled)
        S3_HandlerFunctions.S3_HandlerFunctionsLogger.Trace("Message: " + MessageType.ToString());
      if (this.OnS3_HandlerMessage == null)
        return;
      GMM_EventArgs e = new GMM_EventArgs(this.MessageBaseInfo, MessageType);
      this.OnS3_HandlerMessage((object) this, e);
      if (e.Cancel)
        this.BreakRequest = true;
    }

    internal void SendMessage(int MessageInt, GMM_EventArgs.MessageType MessageType)
    {
      if (S3_HandlerFunctions.S3_HandlerFunctionsLogger.IsTraceEnabled)
        S3_HandlerFunctions.S3_HandlerFunctionsLogger.Trace("Message: " + MessageType.ToString(), MessageInt);
      if (this.OnS3_HandlerMessage == null)
        return;
      GMM_EventArgs e = new GMM_EventArgs(this.MessageBaseInfo, MessageInt, MessageType);
      this.OnS3_HandlerMessage((object) this, e);
      if (e.Cancel)
        this.BreakRequest = true;
    }

    internal void SendMessage(object sender, int MessageInt, GMM_EventArgs.MessageType MessageType)
    {
      if (S3_HandlerFunctions.S3_HandlerFunctionsLogger.IsTraceEnabled)
        S3_HandlerFunctions.S3_HandlerFunctionsLogger.Trace("Message: " + MessageType.ToString() + "; Value: " + MessageInt.ToString());
      if (this.OnS3_HandlerMessage == null)
        return;
      GMM_EventArgs e = new GMM_EventArgs(this.MessageBaseInfo, MessageInt, MessageType);
      this.OnS3_HandlerMessage(sender, e);
      if (e.Cancel)
        this.BreakRequest = true;
    }

    internal void SendMessageSwitchThread(int MessageInt, GMM_EventArgs.MessageType MessageType)
    {
      this.asyncOperation.Post((SendOrPostCallback) (state =>
      {
        try
        {
          this.SendMessage(MessageInt, MessageType);
        }
        catch (Exception ex)
        {
          string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        }
      }), (object) null);
    }

    internal void S3_CommunicationMessage(object sender, GMM_EventArgs TheMessage)
    {
      if (this.OnS3_HandlerMessage == null)
        return;
      this.OnS3_HandlerMessage(sender, TheMessage);
      if (TheMessage.Cancel)
        this.BreakRequest = true;
    }

    public bool ConnectDevice()
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info("Connect device");
      this.MyMeters.ClearWorkLine();
      bool flag = this.MyMeters.ConnectDevice();
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Debug("Connect device done", flag);
      return flag;
    }

    public override void Clear()
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info("Clear all meters");
      this.SaveMeterDataTabel = (DataTable) null;
      this.noAdditionalBaseTypeMissingMessages = false;
      this.MyMeters.Clear();
    }

    public void ClearMeterObjects()
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info("Clear meter objectes");
      this.SaveMeterDataTabel = (DataTable) null;
      this.noAdditionalBaseTypeMissingMessages = false;
      this.MyMeters.ClearWorkLine();
    }

    public bool ReadDeviceId(out S3_DeviceId DeviceId)
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info("Read device identification");
      bool flag = this.MyMeters.ReadHardwareIdentification(out DeviceId);
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Debug("Read device identification done", flag);
      return flag;
    }

    public override async Task<int> ReadDeviceAsync(
      ProgressHandler progress,
      CancellationToken token,
      ReadPartsSelection readPartsSelection)
    {
      this.progress = progress;
      Task<bool> theTask = Task.Run<bool>((Func<bool>) (() => this.ReadConnectedDevice(readPartsSelection)));
      int num1 = await theTask ? 1 : 0;
      bool result = theTask.Result;
      if (!result)
        throw new Exception(ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription);
      int num2 = 1;
      theTask = (Task<bool>) null;
      return num2;
    }

    public override void AddEventHandler(EventHandler<GMM_EventArgs> TheEventHandlerArgs)
    {
      this.OnS3_HandlerMessage += TheEventHandlerArgs;
    }

    public override void RemoveEventHandler(EventHandler<GMM_EventArgs> TheEventHandlerArgs)
    {
      this.OnS3_HandlerMessage -= TheEventHandlerArgs;
    }

    public override void SetCompatibleCommunicationStructure()
    {
      this.SetCommunicationStructure(S3_HandlerFunctions.CommunicationStructure.compatible);
    }

    public bool ReadConnectedDevice(ReadPartsSelection readPartsSelection = ReadPartsSelection.Dump)
    {
      if (this.NewConfigurationDataAvailable)
      {
        this.NewConfigurationDataAvailable = false;
        if (this.MyMeters.ConnectedMeter != null)
          return true;
      }
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info("Read connected device");
      bool flag = this.MyMeters.ReadConnectedDevice(readPartsSelection);
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Debug("Read connected device done", flag);
      return flag;
    }

    public bool RefreshDynamicRamData()
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info("Refresh dynamic RAM data from device");
      bool flag = this.MyMeters.RefreshDynamicParameterFromRAM();
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Debug("Refresh dynamic RAM data from device done", flag);
      return flag;
    }

    public bool IsConnectedMeterUnchanged() => this.MyMeters.IsConnectedMeterUnchanged();

    public bool WriteClone() => this.WriteClone(true);

    public bool WriteClone(bool ShowMessageWindow)
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info("Start write clone");
      bool flag = this.MyMeters.WriteClone(ShowMessageWindow);
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Debug("End clone", flag);
      return flag;
    }

    public override async Task WriteDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      Task<bool> theTask = Task.Run<bool>((Func<bool>) (() => this.WriteChangesToConnectedDevice()));
      int num = await theTask ? 1 : 0;
      bool result = theTask.Result;
      if (!result)
        throw new Exception(ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription);
      theTask = (Task<bool>) null;
    }

    public bool WriteChangesToConnectedDevice()
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info("Start write changes to connected device");
      bool connectedDevice = this.MyMeters.WriteChangesToConnectedDevice();
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Debug("End write changes to connected device", connectedDevice);
      return connectedDevice;
    }

    public bool WriteAllToCompatibledDevice()
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info("Start write all to compatible device");
      bool compatibledDevice = this.MyMeters.WriteConnectedMeterToConnectedCompatibledDevice();
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Debug("End write all to compatible device", compatibledDevice);
      return compatibledDevice;
    }

    public bool WriteChangesToConnectedDevice(DateTime newMeterTime, int timeZone)
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info("Start write changes to connected device. Set new meter time");
      bool connectedDevice = this.MyMeters.WriteChangesToConnectedDevice();
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Debug("End write changes to connected device", connectedDevice);
      return connectedDevice;
    }

    public bool SetTestModeOff(long ValueIdOfLcd)
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info("Start set DeviceTestMode off");
      this.MyMeters.MeterJobStart(MeterJob.DeviceTestModeRelease);
      bool flag = this.MyCommands.TestDone(ValueIdOfLcd);
      this.MyMeters.MeterJobFinished(MeterJob.DeviceTestModeRelease);
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Debug("Set DeviceTestMode off done", flag);
      return flag;
    }

    public bool GetActiveLcdDisplay(out LCD_Display theDisplay)
    {
      theDisplay = (LCD_Display) null;
      if (this.MyLcdChecker == null)
        this.MyLcdChecker = new LCD_Checker(this);
      return this.MyLcdChecker.GetLcdDisplay(out theDisplay);
    }

    public bool SetTestMode(DeviceTestMode TestMode)
    {
      bool flag1 = false;
      switch (TestMode)
      {
        case DeviceTestMode.Off:
          S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info("Device test modes off");
          bool flag2 = this.SetTestModeOff(272769346L);
          if (flag2 && this.MyMeters.ConnectedMeter.MyIdentification.IsLoRa)
          {
            S3_CommandsCOMPATIBLE newCommands = this.checkedNewCommands;
            AsyncHelpers.RunSync((Func<Task>) (async () => await newCommands.myCommandsRadio.StopRadioTests(this.progress, this.cancelToken)));
            flag1 = true;
            break;
          }
          flag1 = flag2;
          break;
        case DeviceTestMode.Temperature_Tests:
          S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info("Start set DeviceTestMode temperature test");
          if (this.adc_Calibration == null)
            this.adc_Calibration = new ADC_Calibration(this);
          flag1 = this.adc_Calibration.SendAdcTestActivate();
          S3_HandlerFunctions.S3_HandlerFunctionsLogger.Debug("Set DeviceTestMode temperature test done", flag1);
          break;
        case DeviceTestMode.Volume_Tests:
          S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info("Start set DeviceTestMode volume test");
          VolumeGraphCalibration.GarantObjectAvailable(this);
          flag1 = this.volumeGraphCalibration == null ? this.MyCommands.FlyingTestActivate() : this.volumeGraphCalibration.SendVolumeTestActivate();
          S3_HandlerFunctions.S3_HandlerFunctionsLogger.Debug("Set DeviceTestMode volume test done", flag1);
          break;
        case DeviceTestMode.Radio_PacketTest:
          S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info("Start set DeviceTestMode radio test");
          if (this.MyMeters.ConnectedMeter.MyIdentification.IsLoRa)
            throw new ApplicationException("Invalid mode for LoRa devices");
          flag1 = this.MyCommands.RadioTestActivate(RadioTestMode.Radio3_Packet);
          break;
        case DeviceTestMode.Radio_CenterTest:
          S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info("Start set DeviceTestMode radio test");
          if (!this.MyMeters.ConnectedMeter.MyIdentification.IsLoRa)
          {
            flag1 = this.MyCommands.RadioTestActivate(RadioTestMode.Radio3Center);
            break;
          }
          S3_CommandsCOMPATIBLE newCommands1 = this.checkedNewCommands;
          AsyncHelpers.RunSync((Func<Task>) (async () => await newCommands1.myCommandsRadio.TransmitUnmodulatedCarrierAsync((ushort) 120, this.progress, this.cancelToken)));
          flag1 = true;
          break;
        default:
          string str = "Test mode not availabel";
          S3_HandlerFunctions.S3_HandlerFunctionsLogger.Fatal(str);
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, str);
          break;
      }
      return flag1;
    }

    internal void SimulateVolume(double volumeInQm)
    {
      if (!this.SetTestMode(DeviceTestMode.Temperature_Tests))
        throw new Exception("Set DeviceTestMode.Temperature_Tests error");
      Thread.Sleep(2000);
      if (!this.RunEnergyCycle((Decimal) volumeInQm))
        throw new Exception("Write SimulatedVolume error");
      Thread.Sleep(2000);
      if (!this.SetTestMode(DeviceTestMode.Off))
        throw new Exception("Set DeviceTestMode.Off error");
    }

    public bool GetTestVolumeQm(out float Volume)
    {
      Volume = 0.0f;
      float Volume1;
      if (!this.GetTestVolume(out Volume1))
        return false;
      Volume = Volume1 * (float) (double) this.MyMeters.WorkMeter.MyMeterScaling.volumeLcdToQmFactor;
      return true;
    }

    public bool GetTestVolume(out float Volume)
    {
      Volume = 0.0f;
      if (this.volumeGraphCalibration == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Read test volume without test state!");
        return false;
      }
      bool testVolume = false;
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info("Start GetTestVolume");
      try
      {
        testVolume = this.volumeGraphCalibration.ReadTestVolume(out Volume);
        if (S3_HandlerFunctions.S3_HandlerFunctionsLogger.IsTraceEnabled)
          S3_HandlerFunctions.S3_HandlerFunctionsLogger.Debug("GetTestVolume done. Received value: " + Volume.ToString(), testVolume);
      }
      catch (Exception ex)
      {
        string str = "Exception: GetTestVolume" + ZR_Constants.SystemNewLine + ex.ToString();
        S3_HandlerFunctions.S3_HandlerFunctionsLogger.Fatal(str);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, str);
      }
      return testVolume;
    }

    public bool RunAdcCalibration(
      ADC_CalibrationSteps calibrationStep,
      float flowTemp,
      float returnTemp,
      int loops)
    {
      if (S3_HandlerFunctions.S3_HandlerFunctionsLogger.IsDebugEnabled)
        S3_HandlerFunctions.S3_HandlerFunctionsLogger.Debug("Start RunAdcCalibration.; TempFlow:" + flowTemp.ToString() + "; TempRet:" + returnTemp.ToString() + "; Loops:" + loops.ToString());
      bool flag = this.adc_Calibration.RunAdcCalibration(calibrationStep, flowTemp, returnTemp, loops);
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Debug("RunAdcCalibration done", flag);
      return flag;
    }

    public bool RunEnergyCycle(Decimal simulatedVolume)
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Debug(nameof (RunEnergyCycle));
      if (this.adc_Calibration == null)
        this.adc_Calibration = new ADC_Calibration(this);
      this.adc_Calibration.RunSingleADC_Cycle(simulatedVolume);
      return true;
    }

    public bool SetOpticalInterfaceMode(OpticalInterfaceMode TestMode)
    {
      bool flag = false;
      switch (TestMode)
      {
        case OpticalInterfaceMode.Wakeup:
          S3_HandlerFunctions.S3_HandlerFunctionsLogger.Debug("Start OptoReleaseStatic");
          this.MyMeters.MeterJobStart(MeterJob.OptoReleaseStatic);
          flag = this.MyCommands.SetOptoTimeoutSeconds(0);
          this.MyMeters.MeterJobFinished(MeterJob.OptoReleaseStatic);
          S3_HandlerFunctions.S3_HandlerFunctionsLogger.Debug("OptoReleaseStatic done", flag);
          break;
        case OpticalInterfaceMode.Static:
          S3_HandlerFunctions.S3_HandlerFunctionsLogger.Debug("Start OptoSetStatic");
          this.MyMeters.MeterJobStart(MeterJob.OptoSetStatic);
          flag = this.MyCommands.SetOptoTimeoutSeconds(3600);
          this.MyMeters.MeterJobFinished(MeterJob.OptoSetStatic);
          S3_HandlerFunctions.S3_HandlerFunctionsLogger.Debug("OptoSetStatic done", flag);
          break;
        default:
          string str = "Optical intrerface mode not availabel";
          S3_HandlerFunctions.S3_HandlerFunctionsLogger.Fatal(str);
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, str);
          break;
      }
      return flag;
    }

    public bool AdjustVolumeFactor(float ErrorInPercent)
    {
      try
      {
        VolumeGraphCalibration.GarantObjectAvailable(this);
        return this.volumeGraphCalibration.AdjustVolumeFactor(ErrorInPercent);
      }
      catch (Exception ex)
      {
        string str = "Exception: Adjust volume factor" + ZR_Constants.SystemNewLine + ex.ToString();
        S3_HandlerFunctions.S3_HandlerFunctionsLogger.Fatal(str);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, str);
        return false;
      }
    }

    public bool AdjustVolumeFactorQi(float Qi, float Q_UpperLimit, float ErrorInPercent)
    {
      try
      {
        VolumeGraphCalibration.GarantObjectAvailable(this);
        return this.volumeGraphCalibration.AdjustVolumeFactorQi(Qi, Q_UpperLimit, ErrorInPercent);
      }
      catch (Exception ex)
      {
        string str = "Exception: Adjust volume factor Qi" + ZR_Constants.SystemNewLine + ex.ToString();
        S3_HandlerFunctions.S3_HandlerFunctionsLogger.Fatal(str);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, str);
        return false;
      }
    }

    public bool AdjustVolumeCalibration(
      float Qi,
      float QiErrorInPercent,
      float Q,
      float QErrorInPercent,
      float Qp,
      float QpErrorInPercent)
    {
      try
      {
        VolumeGraphCalibration.GarantObjectAvailable(this);
        return this.volumeGraphCalibration.AdjustVolumeCalibration(Qi, QiErrorInPercent, Q, QErrorInPercent, Qp, QpErrorInPercent);
      }
      catch (Exception ex)
      {
        string str = "Exception: Adjust volume calibration" + ZR_Constants.SystemNewLine + ex.ToString();
        S3_HandlerFunctions.S3_HandlerFunctionsLogger.Fatal(str);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, str);
        return false;
      }
    }

    public bool CalibrateClock(double error_ppm)
    {
      if (this.MyMeters.WorkMeter == null)
        throw new Exception("WorkMeter not available");
      return this.MyMeters.WorkMeter.CalibrateClock(error_ppm);
    }

    public bool CalibrateRadioFrequency(double error_Hz)
    {
      if (this.MyMeters.WorkMeter == null)
        throw new Exception("WorkMeter not available");
      return this.MyMeters.WorkMeter.CalibrateRadioFrequency(error_Hz);
    }

    public bool SetReducedRadioPowerBy_dBm(int ppm)
    {
      if (this.MyMeters.WorkMeter == null)
        throw new Exception("WorkMeter not available");
      return this.MyMeters.WorkMeter.SetReducedRadioPowerBy_dBm(ppm);
    }

    public DeviceState GetDeviceState()
    {
      return this.MyMeters.ConnectedMeter != null ? this.MyMeters.ConnectedMeter.GetDeviceState() : (DeviceState) null;
    }

    public bool SetConnectedMeterFromWorkMeter() => this.MyMeters.SetConnectedMeterFromWorkMeter();

    public bool RunMeterSampling(out MeterSamplingData samplingData)
    {
      samplingData = (MeterSamplingData) null;
      if (this.MyMeters.ConnectedMeter == null && !this.MyMeters.ConnectDevice())
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Run meter sampling error. Meter not connected");
      return new MeterSampling(this).RunSampling(out samplingData);
    }

    public bool SaveMeterObject(MeterObjects meterSelect)
    {
      return this.MyMeters.SaveMeter(this.MyMeters.GetMeterObject(meterSelect));
    }

    public override DateTime? SaveMeter()
    {
      this.SaveMeterObject(MeterObjects.WorkMeter);
      return new DateTime?(DateTime.Now);
    }

    public bool CompareMeterObjects(
      string compareInfo,
      MeterObjects srcMeterObject,
      MeterObjects destMeterObject)
    {
      return MeterObjectComparer.CompareObjects(compareInfo, this, srcMeterObject, destMeterObject);
    }

    internal bool CompareMeterObjects(string compareInfo, S3_Meter srcMeter, S3_Meter destMeter)
    {
      return MeterObjectComparer.CompareObjects(compareInfo, this, srcMeter, destMeter);
    }

    public void ShowComparerWindow() => MeterObjectComparer.ShowComparerWindow(this);

    public void C5_FirmwareUpdate_To_FW_5_2_5()
    {
      new C5_FirmwareUpdate(this).StartUpdate_To_FW_5_2_5();
    }

    public void C5_firmwareUpdate(int mapID, string firmwarefile)
    {
      this.C5_firmwareUpdate(mapID, firmwarefile, true, false);
    }

    public void C5_firmwareUpdate(int mapID, string firmwarefile, bool ignoreOriginalMeter)
    {
      this.C5_firmwareUpdate(mapID, firmwarefile, ignoreOriginalMeter, false);
    }

    public void C5_firmwareUpdate(
      int mapID,
      string firmwarefile,
      bool ignoreOriginalMeter,
      bool firmwareOnly)
    {
      ZR_ClassLibMessages.ClearErrors();
      FirmwareUpdateSettings settings = new FirmwareUpdateSettings();
      settings.mapID = mapID;
      settings.ignoreOriginalMeter = ignoreOriginalMeter;
      settings.firmwareFile = firmwarefile;
      settings.eraseInfoMemory = true;
      settings.firmwareOnly = firmwareOnly;
      if (firmwareOnly)
        settings.eraseInfoMemory = false;
      new C5_FirmwareUpdate(this, settings).StartUpdate();
    }

    public bool GetLoadResult(out S3_handlerLoadResult LoadResult)
    {
      LoadResult = (S3_handlerLoadResult) null;
      if (this.LoadResult == null)
        return false;
      LoadResult = this.LoadResult;
      return true;
    }

    public override DeviceIdentification GetDeviceIdentification()
    {
      return (DeviceIdentification) new S3_CommonDeviceIdentification(this.MyMeters.CheckedWorkMeter);
    }

    public S3_DeviceId GetDeviceId()
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info("Get device identification");
      if (this.MyMeters.WorkMeter != null)
        return this.MyMeters.WorkMeter.MyIdentification.GetDeviceId();
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Error("Device identification not availabel!");
      return (S3_DeviceId) null;
    }

    public override bool IsProtected() => this.MyCommands.DeviceProtectionGet();

    public GlobalDeviceId GetGlobalDeviceIds()
    {
      this.NewConfigurationDataAvailable = false;
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info("Get global device identifications");
      S3_Meter activeMeter = this.MyMeters.GetActiveMeter();
      if (activeMeter != null)
        return activeMeter.GetGlobalDeviceIds();
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Error("Global device identifications not availabel!");
      return (GlobalDeviceId) null;
    }

    public bool CorrectInputIds(S3_DeviceId DeviceId)
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info("Correct device identification");
      if (this.MyMeters.WorkMeter != null)
        return this.MyMeters.WorkMeter.MyIdentification.CorrectInputIds(DeviceId);
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Error("WorkMeter not availabel!");
      return false;
    }

    public bool SetDeviceId(S3_DeviceId newDeviceId)
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info("Set device identification");
      if (this.MyMeters.WorkMeter != null)
        return this.MyMeters.WorkMeter.MyIdentification.SetDeviceId(newDeviceId);
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Error("WorkMeter not availabel!");
      return false;
    }

    public bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList,
      int SubDevice)
    {
      if (this.MyMeters.WorkMeter == null || !this.MyMeters.WorkMeter.GetValues(ref ValueList, SubDevice))
        return false;
      ValueIdent.CleanUpEmptyValueIdents(ValueList);
      return true;
    }

    public MeterDisplayValues GetDisplayValues()
    {
      return this.MyMeters.ConnectedMeter == null ? (MeterDisplayValues) null : this.MyMeters.ConnectedMeter.GetDisplayValues();
    }

    public override SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      int SubDevice = 0)
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info(nameof (GetConfigurationParameters));
      return this.MyMeters.GetConfigurationParameters(SubDevice);
    }

    public override void SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameterList,
      int subDevice = 0)
    {
      ZR_ClassLibMessages.ClearErrors();
      if (!this.SetConfigurationParameter(parameterList, subDevice))
        throw new Exception(ZR_ClassLibMessages.GetLastErrorMessageAndClearError());
    }

    public override BatteryEnergyManagement GetBatteryCalculations()
    {
      if (this.MyMeters.CheckedWorkMeter.BatteryManager == null)
        throw new Exception("Battery properties not calculated");
      return (BatteryEnergyManagement) this.MyMeters.WorkMeter.BatteryManager;
    }

    public bool SetConfigurationParameterNoClone(
      SortedList<OverrideID, ConfigurationParameter> parameterList,
      int SubDevice = 0)
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info(nameof (SetConfigurationParameterNoClone));
      return this.MyMeters.SetConfigurationParameterNoClone(parameterList, SubDevice);
    }

    public bool SetConfigurationParameter(
      SortedList<OverrideID, ConfigurationParameter> parameterList,
      int SubDevice = 0)
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info("SetConfigurationParameterNoClone");
      return this.MyMeters.SetConfigurationParameter(parameterList, SubDevice);
    }

    public bool SetAllConfigurationParameter(
      SortedList<OverrideID, ConfigurationParameter>[] parameterLists)
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info(nameof (SetAllConfigurationParameter));
      return parameterLists == null || parameterLists.Length != 4 ? ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Number of configuration lists != 4", S3_HandlerFunctions.S3_HandlerFunctionsLogger) : this.MyMeters.SetAllConfigurationParameter(parameterLists, true);
    }

    public bool ChangeHardwareType(
      DeviceHardwareIdentification hardwareIdentification)
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info(nameof (ChangeHardwareType));
      return this.MyMeters.ChangeHardwareType(hardwareIdentification);
    }

    public bool SetTimes(
      DateTime meterTimeFromPcTimeZone,
      double timeZoneUTC,
      int endOfBatteryYear,
      int endOfCalibrationYear)
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info(nameof (SetTimes));
      return this.MyMeters.WorkMeter.SetTimes(meterTimeFromPcTimeZone, timeZoneUTC, endOfBatteryYear, endOfCalibrationYear);
    }

    public bool SetMeterKey(uint password)
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info(nameof (SetMeterKey));
      return this.MyMeters.WorkMeter.SetMeterKey(password, true);
    }

    internal S3_Meter checkedTestMeter
    {
      get
      {
        if (this.MyMeters.WorkMeter == null)
          throw new HandlerMessageException("WorkMeter object not available");
        return this.MyMeters.ConnectedMeter != null ? this.MyMeters.WorkMeter : throw new HandlerMessageException("ConnectedMeter object not available");
      }
    }

    internal S3_CommandsCOMPATIBLE checkedNewCommands
    {
      get
      {
        return this.MyCommands is S3_CommandsCOMPATIBLE ? (S3_CommandsCOMPATIBLE) this.MyCommands : throw new Exception("Illegal reading system selected");
      }
    }

    public bool SetWriteProtection() => this.MyMeters.WorkMeter.SetWriteProtection();

    public bool ClearWriteProtection(uint password)
    {
      if (this.MyMeters.WorkMeter == null)
        this.MyMeters.WorkMeter = this.MyMeters.ConnectedMeter;
      return this.MyMeters.WorkMeter.ClearWriteProtection(password);
    }

    public bool GetIUFcalibrationCurve(out SortedList<double, double> calibrationCurve)
    {
      calibrationCurve = (SortedList<double, double>) null;
      VolumeGraphCalibration.GarantObjectAvailable(this);
      if (this.volumeGraphCalibration != null && this.volumeGraphCalibration is TDC_Calibration)
        return ((TDC_Calibration) this.volumeGraphCalibration).GetIUFcalibrationCurve(out calibrationCurve);
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Ultasonic calibration with illegal handler state!");
      return false;
    }

    public string ReadAndGetTdcStatusReport()
    {
      FlyingTestDiagnostic flyingTestDiagnostic = new FlyingTestDiagnostic(this);
      flyingTestDiagnostic.ReadTdcStatusData();
      return flyingTestDiagnostic.GetTdcStatusReport();
    }

    public string ReadAndGetFlyingTestReport()
    {
      FlyingTestDiagnostic flyingTestDiagnostic = new FlyingTestDiagnostic(this);
      flyingTestDiagnostic.ReadFlyingTestData();
      return flyingTestDiagnostic.GetFlyingTestReport();
    }

    public FlyingTestData Read_FlyingTestDiagnostic()
    {
      FlyingTestDiagnostic flyingTestDiagnostic = new FlyingTestDiagnostic(this);
      flyingTestDiagnostic.ReadFlyingTestData();
      return flyingTestDiagnostic.flyingTestData;
    }

    public VMCP_Diagnostic Read_VMCP_Diagnostic()
    {
      return new WR4_VMCP_Diagnostic(this).ReadVMCP_Diagnostic();
    }

    public ulong Read_VMCP_Volume() => new WR4_VMCP_Diagnostic(this).ReadVMCP_Volume();

    public string Read_VMCP_VolumeString()
    {
      return "VMCP volume: " + this.Read_VMCP_Volume().ToString("d014").Insert(8, ".") + "m\u00B3";
    }

    public void CalibrateConnectedSensors(
      TemperatureSensorParameters flowSensor,
      TemperatureSensorParameters returnSensor)
    {
      if (this.adc_Calibration == null)
        this.adc_Calibration = new ADC_Calibration(this);
      this.adc_Calibration.CalibrateConnectedSensors(flowSensor, returnSensor);
    }

    public SortedList<string, string> GetAdcDiagnosticValues()
    {
      if (this.adc_Calibration == null)
        this.adc_Calibration = new ADC_Calibration(this);
      return this.adc_Calibration.GetDiagnosticValues();
    }

    public int GetCheckupState() => this.checkedTestMeter.ReadCheckupState();

    public S3_DeviceDescriptor ReadDeviceDescriptor()
    {
      if (this.MyMeters.ConnectedMeter == null)
        throw new InvalidOperationException("ConnectedMeter object not available.");
      ByteField MemoryData;
      this.MyCommands.ReadMemory(MemoryLocation.NONE, 6656, 19, out MemoryData);
      return new S3_DeviceDescriptor(MemoryData);
    }

    public bool GetVolumeCounterValue(out byte volumeCounterValue)
    {
      volumeCounterValue = (byte) 0;
      return this.MyMeters.ConnectedMeter != null && this.MyMeters.ConnectedMeter.MyDeviceMemory.GetVolumeCounterValue(out volumeCounterValue);
    }

    public void SetRemainingCycleTime(byte remainingSeconds)
    {
      S3_Meter checkedTestMeter = this.checkedTestMeter;
      ZR_ClassLibMessages.ClearErrors();
      checkedTestMeter.MyParameters.ParameterByName[S3_ParameterNames.temperaturCycleTimeCounter.ToString()].SetByteValue(remainingSeconds);
      checkedTestMeter.MyParameters.ParameterByName[S3_ParameterNames.temperaturCycleTimeCounter.ToString()].WriteParameterToConnectedDevice();
      if (checkedTestMeter.MyIdentification.IsWR4)
        return;
      SortedList<string, S3_Parameter> parameterByName1 = checkedTestMeter.MyParameters.ParameterByName;
      S3_ParameterNames s3ParameterNames = S3_ParameterNames.volumeCycleTimeCounter;
      string key1 = s3ParameterNames.ToString();
      parameterByName1[key1].SetByteValue(remainingSeconds);
      SortedList<string, S3_Parameter> parameterByName2 = checkedTestMeter.MyParameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.volumeCycleTimeCounter;
      string key2 = s3ParameterNames.ToString();
      parameterByName2[key2].WriteParameterToConnectedDevice();
    }

    public ImpulseInputCounters ReadImpulseInputCounters()
    {
      return this.MyCommands.ReadInputCounters() ?? throw new HandlerMessageException("ReadImpulseInputCounters read from device error." + Environment.NewLine + ZR_ClassLibMessages.GetLastErrorAndClearError()?.ToString());
    }

    public override bool LoadLastBackup(int MeterId)
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info(nameof (LoadLastBackup));
      return this.OpenDeviceLastBackup(MeterId);
    }

    public override bool LoadBackup(int MeterId, DateTime TimePoint)
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info(nameof (LoadBackup));
      return this.OpenDevice(MeterId, TimePoint);
    }

    public bool OpenDeviceLastBackup(int MeterId)
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info(nameof (OpenDeviceLastBackup));
      if (!this.MyMeters.OpenDeviceLastBackup(MeterId))
        return false;
      this.LoadResult = new S3_handlerLoadResult();
      this.LoadResult.BasisTypeMeterInfoId = this.MyMeters.DbMeter.MyIdentification.BaseTypeId;
      this.LoadResult.HardwareTypeId = this.MyMeters.DbMeter.MyIdentification.HardwareTypeId;
      this.LoadResult.FirmwareVersion = this.MyMeters.DbMeter.MyIdentification.FirmwareVersionString;
      return true;
    }

    public bool OpenLastBackupFromTimeRange(
      DateTime rangeStartTime,
      DateTime rangeEndTime,
      int MeterID)
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info(nameof (OpenLastBackupFromTimeRange));
      if (!this.MyMeters.OpenLastBackupFromTimeRange(rangeStartTime, rangeEndTime, MeterID))
        return false;
      this.LoadResult = new S3_handlerLoadResult();
      this.LoadResult.BasisTypeMeterInfoId = this.MyMeters.DbMeter.MyIdentification.BaseTypeId;
      this.LoadResult.HardwareTypeId = this.MyMeters.DbMeter.MyIdentification.HardwareTypeId;
      this.LoadResult.FirmwareVersion = this.MyMeters.DbMeter.MyIdentification.FirmwareVersionString;
      return true;
    }

    public bool OverwriteFromBackup(bool[] overwriteSelection)
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info("OverwriteFromBackup + overwriteSelection");
      return this.MyMeters.OverwriteFromBackup(overwriteSelection);
    }

    public bool OpenDevice(int MeterId, DateTime TimePoint)
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info("OpenDevice at TimePoint");
      return this.MyMeters.OpenDevice(MeterId, TimePoint);
    }

    public bool SaveDevice()
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info(nameof (SaveDevice));
      return this.MyMeters.SaveDevice();
    }

    public override void OpenType(int MeterInfoId)
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info("OpenType by MeterInfoId: " + MeterInfoId.ToString());
      if (!this.MyMeters.OpenType(MeterInfoId))
        throw new Exception(ZR_ClassLibMessages.GetLastErrorMessageAndClearError());
    }

    public override void OpenType(string TypeCreationString)
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info("OpenType by TypeCreationString: " + TypeCreationString);
      if (!this.MyMeters.OpenType(TypeCreationString))
        throw new Exception(ZR_ClassLibMessages.GetLastErrorMessageAndClearError());
    }

    public bool SaveType(
      ref uint meterInfoId,
      string sapNumber,
      DeviceHardwareIdentification hardwareIdentification,
      SaveOptions saveOption,
      string meterInfoDescription,
      string meterTypeDescription)
    {
      return this.MyMeters.SaveType(ref meterInfoId, sapNumber, hardwareIdentification, saveOption, meterInfoDescription, meterTypeDescription, (string) null);
    }

    public bool OverwriteWorkFromType(bool[] OverwriteSelection)
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info(nameof (OverwriteWorkFromType));
      return this.MyMeters.OverwriteWorkFromType(OverwriteSelection);
    }

    public override void OverwriteFromType(CommonOverwriteGroups[] overwriteGroups)
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info(nameof (OverwriteFromType));
      this.MyMeters.OverwriteSrcToWork(HandlerMeterObjects.TypeMeter, overwriteGroups);
    }

    public override void OverwriteSrcToDest(
      HandlerMeterObjects sourceObject,
      HandlerMeterObjects destinationObject,
      CommonOverwriteGroups[] overwriteGroups)
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info(nameof (OverwriteSrcToDest));
      if (destinationObject != 0)
        throw new Exception("Only WorkMeter supported as destination!");
      this.MyMeters.OverwriteSrcToWork(sourceObject, overwriteGroups);
    }

    public bool IsTypeOverwritePossible(bool[] OverwriteSelection)
    {
      return this.MyMeters.IsTypeOverwritePossible(OverwriteSelection);
    }

    public bool IsTypeOverwritePerfect()
    {
      return this.MyMeters.OverwriteFromType.IsTypeOverwritePerfect();
    }

    public bool SetTypeInfos(uint SAP_MaterialNumber, uint MeterInfoId, uint MeterTypeId)
    {
      S3_HandlerFunctions.S3_HandlerFunctionsLogger.Info(nameof (SetTypeInfos));
      return this.MyMeters.WorkMeter.SetTypeInfos(SAP_MaterialNumber, MeterInfoId, MeterTypeId);
    }

    public bool SaveMeterObjectFromDifferentHandlerInstance(S3_HandlerFunctions sourceHandler)
    {
      this.MyMeters.SavedMeter = sourceHandler.MyMeters.WorkMeter;
      return this.MyMeters.CreateWorkMeterFromObjectMemory(this.MyMeters.SavedMeter);
    }

    public bool IsUltrasonicVolumeMeterReady(out IufCheckResults checkResultes)
    {
      checkResultes = (IufCheckResults) null;
      VolumeGraphCalibration.GarantObjectAvailable(this);
      if (this.volumeGraphCalibration != null && this.volumeGraphCalibration is TDC_Calibration)
        return (this.volumeGraphCalibration as TDC_Calibration).IsUltrasonicVolumeMeterReady(out checkResultes);
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Ultasonic check with illegal handler state!");
      return false;
    }

    public bool RunUltrasonicZeroFlowCalibration(
      out float volumeMeterTemp,
      out int tdcZeroCalibrationValue,
      out int changeOfTdcZeroCalibrationValue)
    {
      volumeMeterTemp = 0.0f;
      tdcZeroCalibrationValue = 0;
      changeOfTdcZeroCalibrationValue = 0;
      VolumeGraphCalibration.GarantObjectAvailable(this);
      if (this.volumeGraphCalibration == null || !(this.volumeGraphCalibration is TDC_Calibration))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Ultasonic calibration with illegal handler state!");
        return false;
      }
      TDC_Calibration graphCalibration = this.volumeGraphCalibration as TDC_Calibration;
      float flowTemp;
      float returnTemp;
      if (!this.MyMeters.WorkMeter.MyParameters.GetRefreshedTemperatures(out flowTemp, out returnTemp))
        return false;
      volumeMeterTemp = ((uint) this.MyMeters.WorkMeter.MyParameters.ParameterByName["Device_Setup_2"].GetUshortValue() & 1U) <= 0U ? returnTemp : flowTemp;
      if (!this.MyCommands.TestDone(272769346L))
        return false;
      int numberOfLoops = 50;
      int mediaFilterRange = 3;
      TDC_Calibration.CalibrationInfo calibrationInfo;
      if (!graphCalibration.CalibrateZeroFlow(numberOfLoops, mediaFilterRange, volumeMeterTemp, out calibrationInfo))
        return false;
      tdcZeroCalibrationValue = calibrationInfo.tdcZeroCalibrationValue;
      changeOfTdcZeroCalibrationValue = calibrationInfo.changeOfTdcZeroCalibrationValue;
      return true;
    }

    public byte[] GetAesKey()
    {
      if (this.MyMeters.WorkMeter == null)
        throw new Exception("Workmeter doesn't exist. Please read or open a device first!");
      if (this.MyMeters.WorkMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.encryptionKey.ToString()) && this.MyMeters.WorkMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.encryptionKey1.ToString()) && this.MyMeters.WorkMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.encryptionKey2.ToString()) && this.MyMeters.WorkMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.encryptionKey3.ToString()))
        throw new Exception("AES-Encryption-Key is not supported by this device!!!");
      byte[] bytes1 = BitConverter.GetBytes(this.MyMeters.WorkMeter.MyParameters.ParameterByName[S3_ParameterNames.encryptionKey.ToString()].GetUintValue());
      SortedList<string, S3_Parameter> parameterByName1 = this.MyMeters.WorkMeter.MyParameters.ParameterByName;
      S3_ParameterNames s3ParameterNames = S3_ParameterNames.encryptionKey1;
      string key1 = s3ParameterNames.ToString();
      byte[] bytes2 = BitConverter.GetBytes(parameterByName1[key1].GetUintValue());
      SortedList<string, S3_Parameter> parameterByName2 = this.MyMeters.WorkMeter.MyParameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.encryptionKey2;
      string key2 = s3ParameterNames.ToString();
      byte[] bytes3 = BitConverter.GetBytes(parameterByName2[key2].GetUintValue());
      SortedList<string, S3_Parameter> parameterByName3 = this.MyMeters.WorkMeter.MyParameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.encryptionKey3;
      string key3 = s3ParameterNames.ToString();
      byte[] bytes4 = BitConverter.GetBytes(parameterByName3[key3].GetUintValue());
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) bytes1);
      byteList.AddRange((IEnumerable<byte>) bytes2);
      byteList.AddRange((IEnumerable<byte>) bytes3);
      byteList.AddRange((IEnumerable<byte>) bytes4);
      return byteList.ToArray();
    }

    public bool Print(string options)
    {
      S3_Handler.Print print = new S3_Handler.Print(this);
      if (!print.IsDisposed)
      {
        int num = (int) print.ShowDialog();
      }
      return true;
    }

    internal enum S3_HandlerConfigVars
    {
      LoadLastSettings,
      SaveSettings,
      UsePcTime,
      MeterBackupOnWrite,
      MeterBackupOnRead,
      OnlyOneReadBackupPerDay,
      UseDevelopmentFunctions,
      CheckWriteByRead,
      UseBaseTypeByConfig,
      BaseTypeEditMode,
      PrintConfigPath,
      PrintConfigUseDateTimeFileToken,
      LastTypeMeterInfoId,
      ConnectionProfileID,
      LastCommunicationProfileID,
      LastComPort,
      CommunicationStructure,
    }

    public enum CommunicationStructure
    {
      classicDeviceCollector,
      compatible,
      commonDefined,
    }
  }
}
