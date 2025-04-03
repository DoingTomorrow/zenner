// Decompiled with JetBrains decompiler
// Type: Devices.DeviceManager
// Assembly: Devices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 793FC2DA-FF88-4FD5-BDE9-C00C0310F1EC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Devices.dll

using AsyncCom;
using CommunicationPort.UserInterface;
using DeviceCollector;
using NLog;
using ReadoutConfiguration;
using StartupLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace Devices
{
  public class DeviceManager : IDisposable
  {
    private static Logger logger = LogManager.GetLogger(nameof (DeviceManager));
    public bool IsPlugin = false;
    public string CommonDeviceName = (string) null;
    private SortedList<string, string> SavedSettings;
    public BaseDevice SelectedHandler;
    private object databaseMutex = new object();
    private List<int> DeviceCollectorDeviceIndexFromConfiguratorIndex;
    private List<GlobalDeviceId> ConfiguratorDeviceList;

    public DeviceCollectorFunctions MyBus { get; private set; }

    public AsyncFunctions MyAsyncCom { get; private set; }

    public CommunicationPortWindowFunctions MyCommunicationPort { get; set; }

    public ConfigList ActiveConfigList { get; private set; }

    public event EventHandler<GMM_EventArgs> OnMessage;

    public event EventHandlerEx<Exception> OnError;

    public event EventHandler<ValueIdentSet> ValueIdentSetReceived;

    public event EventHandlerEx<int> OnProgress;

    public event System.EventHandler BatterieLow;

    public event EventHandlerEx<string> OnProgressMessage;

    public event System.EventHandler ConnectionLost;

    internal bool IsValueIdentSetReceivedEventEnabled => this.ValueIdentSetReceived != null;

    public ConfigurationParameter.ValueType ParameterType { get; set; }

    public DeviceManagerModes DeviceManagerMode { get; private set; }

    public bool BreakRequest
    {
      get => this.MyBus != null && this.MyBus.BreakRequest;
      set
      {
        if (this.MyBus == null)
          return;
        this.MyBus.BreakRequest = value;
      }
    }

    public GmmComponentInfo LoadedComponentInfo { get; private set; }

    public DeviceManager()
    {
    }

    public DeviceManager(DeviceCollectorFunctions deviceCollector)
    {
      this.MyBus = deviceCollector;
      this.MyBus.OnProgress += new EventHandlerEx<int>(this.deviceCollector_OnProgress);
      this.MyBus.OnProgressMessage += new EventHandlerEx<string>(this.deviceCollector_OnProgressMessage);
      this.ParameterType = ConfigurationParameter.ValueType.Direct;
    }

    public void Dispose()
    {
      this.BreakRequest = true;
      this.Close();
      this.DisposeHandler();
      if (this.MyAsyncCom != null)
        this.MyAsyncCom.BatterieLow -= new System.EventHandler(this.RaiseBatterieLow);
      if (this.MyBus != null)
      {
        this.MyBus.OnProgress -= new EventHandlerEx<int>(this.deviceCollector_OnProgress);
        this.MyBus.OnProgressMessage -= new EventHandlerEx<string>(this.deviceCollector_OnProgressMessage);
        this.MyBus.Dispose();
        this.MyBus.GMM_Dispose();
      }
      GC.Collect();
      GC.WaitForPendingFinalizers();
    }

    private void DisposeHandler()
    {
      if (this.SelectedHandler == null)
        return;
      this.SelectedHandler.Dispose();
      this.SelectedHandler.OnProgress -= new EventHandlerEx<int>(this.SelectedHandler_OnProgress);
      this.SelectedHandler.OnProgressMessage -= new EventHandlerEx<string>(this.SelectedHandler_OnProgressMessage);
      this.SelectedHandler.ConnectionLost -= new System.EventHandler(this.SelectedHandler_ConnectionLost);
      this.SelectedHandler = (BaseDevice) null;
    }

    public void ClassicInitialisation()
    {
      ZR_Component.CommonGmmInterface.GarantComponentLoaded(GMM_Components.DeviceCollector);
      this.MyBus = (DeviceCollectorFunctions) ZR_Component.CommonGmmInterface.LoadedComponentsList[GMM_Components.DeviceCollector];
      this.MyBus.OnProgress += new EventHandlerEx<int>(this.deviceCollector_OnProgress);
      this.MyBus.OnProgressMessage += new EventHandlerEx<string>(this.deviceCollector_OnProgressMessage);
      this.ParameterType = ConfigurationParameter.ValueType.Direct;
    }

    public void PrepareCommunicationStructure(ConfigList configList)
    {
      this.ActiveConfigList = configList != null ? configList : throw new Exception("Configuration not defined");
      try
      {
        ConnectionProfile partialProfile = ReadoutConfigFunctions.GetPartialProfile(this.ActiveConfigList.ConnectionProfileID);
        if (partialProfile == null)
          throw new Exception("Profile with ProfileID = " + this.ActiveConfigList.ConnectionProfileID.ToString() + " not supported");
        string handlerName = "NoHandler";
        if (partialProfile.DeviceModel.Parameters != null)
        {
          if (partialProfile.DeviceModel.Parameters.ContainsKey(ConnectionProfileParameter.Handler))
          {
            handlerName = partialProfile.DeviceModel.Parameters[ConnectionProfileParameter.Handler];
          }
          else
          {
            string busMode1 = configList.BusMode;
            ZR_ClassLibrary.BusMode busMode2 = ZR_ClassLibrary.BusMode.MBus;
            string str1 = busMode2.ToString();
            int num;
            if (!(busMode1 == str1))
            {
              string busMode3 = configList.BusMode;
              busMode2 = ZR_ClassLibrary.BusMode.MBusPointToPoint;
              string str2 = busMode2.ToString();
              num = busMode3 == str2 ? 1 : 0;
            }
            else
              num = 1;
            if (num != 0)
              handlerName = configList.SelectedDeviceMBusType;
          }
        }
        GmmStructureInfo.Init(this.IsPlugin);
        this.LoadedComponentInfo = GmmStructureInfo.GetComponentInfo(handlerName);
        if (this.LoadedComponentInfo.CommunicationModel == CommunicationModels.DeviceCollector_AsyncCom)
        {
          if (this.MyCommunicationPort != null)
            this.MyCommunicationPort.Close();
          if (this.IsPlugin)
          {
            this.MyBus = (DeviceCollectorFunctions) PlugInLoader.GetPlugIn("DeviceCollector").GetPluginInfo().Interface;
            this.MyAsyncCom = (AsyncFunctions) PlugInLoader.GetPlugIn("AsyncCom").GetPluginInfo().Interface;
            this.MyAsyncCom.BatterieLow -= new System.EventHandler(this.RaiseBatterieLow);
            this.MyAsyncCom.BatterieLow += new System.EventHandler(this.RaiseBatterieLow);
          }
          else
          {
            if (this.MyAsyncCom == null)
            {
              this.MyAsyncCom = new AsyncFunctions();
              this.MyAsyncCom.BatterieLow += new System.EventHandler(this.RaiseBatterieLow);
            }
            if (this.MyBus == null)
            {
              this.MyBus = new DeviceCollectorFunctions((IAsyncFunctions) this.MyAsyncCom, false);
              this.MyBus.OnProgress += new EventHandlerEx<int>(this.deviceCollector_OnProgress);
              this.MyBus.OnProgressMessage += new EventHandlerEx<string>(this.deviceCollector_OnProgressMessage);
            }
          }
          SortedList<string, string> sortedList = this.ActiveConfigList.GetSortedList();
          this.MyBus.SetDeviceCollectorSettings(sortedList);
          this.MyAsyncCom.SetAsyncComSettings(sortedList);
          this.GarantDeviceCollectorObjectFromHandlerName(handlerName, this.ActiveConfigList);
          this.GarantHandlerLoaded();
        }
        else
        {
          if (this.MyAsyncCom != null)
            this.MyAsyncCom.Close();
          ConfigList configList1 = this.ActiveConfigList;
          if (this.IsPlugin)
          {
            this.MyCommunicationPort = (CommunicationPortWindowFunctions) PlugInLoader.GetPlugIn("CommunicationPort").GetPluginInfo().Interface;
            this.MyCommunicationPort.portFunctions.OnBatteryLow -= new System.EventHandler(this.RaiseBatterieLow);
            this.MyCommunicationPort.portFunctions.OnBatteryLow += new System.EventHandler(this.RaiseBatterieLow);
          }
          else if (this.MyCommunicationPort == null)
          {
            this.MyCommunicationPort = new CommunicationPortWindowFunctions();
            this.MyCommunicationPort.SetReadoutConfiguration(this.ActiveConfigList);
            this.MyCommunicationPort.portFunctions.OnBatteryLow += new System.EventHandler(this.RaiseBatterieLow);
          }
          else
          {
            configList1 = this.MyCommunicationPort.GetReadoutConfiguration();
            ConfigList configList2 = configList1;
            SortedList<string, string> sortedList = this.ActiveConfigList.GetSortedList();
            if (!configList2.Equal(sortedList))
              configList2.Reset(this.ActiveConfigList.GetSortedList());
          }
          if (this.SelectedHandler == null || this.SelectedHandler.GetType() != typeof (CommonHandlerWrapper) || ((CommonHandlerWrapper) this.SelectedHandler).HandlerName != handlerName)
          {
            CommonHandlerWrapper fromHandlerName = CommonHandlerWrapper.CreateFromHandlerName(this, configList1, handlerName, this.IsPlugin);
            fromHandlerName.OnProgress += new EventHandlerEx<int>(this.SelectedHandler_OnProgress);
            this.SelectedHandler = (BaseDevice) fromHandlerName;
          }
          else
          {
            ConfigList readoutConfiguration = ((CommonHandlerWrapper) this.SelectedHandler).HandlerInterface.GetReadoutConfiguration();
            SortedList<string, string> sortedList = this.ActiveConfigList.GetSortedList();
            if (!readoutConfiguration.Equal(sortedList))
              readoutConfiguration.Reset(sortedList);
          }
        }
      }
      catch (Exception ex)
      {
        this.LoadedComponentInfo = (GmmComponentInfo) null;
        throw ex;
      }
    }

    private void GarantDeviceCollectorObjectFromHandlerName(
      string handlerName,
      ConfigList configList)
    {
      this.MyBus.DeleteBusInfo();
      ZR_ClassLibMessages.ClearErrors();
      ZR_ClassLibrary.BusMode baseMode = this.MyBus.GetBaseMode();
      bool flag;
      switch (baseMode)
      {
        case ZR_ClassLibrary.BusMode.MBusPointToPoint:
        case ZR_ClassLibrary.BusMode.MBus:
          switch (handlerName)
          {
            case "EDC_Handler":
              flag = this.MyBus.AddDevice(DeviceTypes.EDC, (int) configList.PrimaryAddress, (long) configList.SecondaryAddress);
              break;
            case "TH_Handler":
              flag = this.MyBus.AddDevice(DeviceTypes.TemperatureSensor, (int) configList.PrimaryAddress, (long) configList.SecondaryAddress);
              break;
            case "MinolHandler":
              flag = this.MyBus.AddDevice(DeviceTypes.Minol_Device, (int) configList.PrimaryAddress, (long) configList.SecondaryAddress);
              break;
            case "PDC_Handler":
              flag = this.MyBus.AddDevice(DeviceTypes.PDC, (int) configList.PrimaryAddress, (long) configList.SecondaryAddress);
              break;
            case "GMM_Handler":
              flag = this.MyBus.AddDevice(DeviceTypes.ZR_Serie2, (int) configList.PrimaryAddress, (long) configList.SecondaryAddress);
              break;
            case "S3_Handler":
              flag = this.MyBus.AddDevice(DeviceTypes.ZR_Serie3, (int) configList.PrimaryAddress, (long) configList.SecondaryAddress);
              break;
            default:
              flag = this.MyBus.AddDevice(DeviceTypes.MBus, (int) configList.PrimaryAddress, (long) configList.SecondaryAddress);
              break;
          }
          break;
        case ZR_ClassLibrary.BusMode.MinomatV2:
          flag = true;
          break;
        case ZR_ClassLibrary.BusMode.MinomatV3:
        case ZR_ClassLibrary.BusMode.MinomatV4:
        case ZR_ClassLibrary.BusMode.Minol_Device:
        case ZR_ClassLibrary.BusMode.SmokeDetector:
          DeviceManager.logger.Debug("Expected device: " + configList.PrimaryAddress.ToString());
          flag = this.MyBus.AddDevice(DeviceTypes.Minol_Device, 0, (long) configList.PrimaryAddress);
          break;
        case ZR_ClassLibrary.BusMode.Radio2:
        case ZR_ClassLibrary.BusMode.Radio3:
        case ZR_ClassLibrary.BusMode.Radio4:
        case ZR_ClassLibrary.BusMode.wMBusS1:
        case ZR_ClassLibrary.BusMode.wMBusS1M:
        case ZR_ClassLibrary.BusMode.wMBusS2:
        case ZR_ClassLibrary.BusMode.wMBusT1:
        case ZR_ClassLibrary.BusMode.wMBusT2_meter:
        case ZR_ClassLibrary.BusMode.wMBusT2_other:
        case ZR_ClassLibrary.BusMode.wMBusC1A:
        case ZR_ClassLibrary.BusMode.wMBusC1B:
        case ZR_ClassLibrary.BusMode.Radio3_868_95_RUSSIA:
        case ZR_ClassLibrary.BusMode.RadioMS:
          flag = true;
          break;
        default:
          throw new Exception("The AddDevice method is not supported for this device! BusMode: " + baseMode.ToString());
      }
      if (!flag)
      {
        ZR_ClassLibMessages.LastErrorInfo errorAndClearError = ZR_ClassLibMessages.GetLastErrorAndClearError();
        DeviceManager.logger.Error("Failed add device to expected devices! Error: " + errorAndClearError.LastErrorDescription);
        throw new Exception("Error on add DeviceCollector device object!" + Environment.NewLine + "HandlerName: " + handlerName + "; BusMode: " + baseMode.ToString() + Environment.NewLine + errorAndClearError?.ToString());
      }
    }

    public void ShowHandlerWindow()
    {
      this.ParameterType = ConfigurationParameter.ValueType.Complete;
      if (!this.GarantHandlerLoaded() || this.SelectedHandler == null)
        return;
      Application.EnableVisualStyles();
      this.SelectedHandler.ShowHandlerWindow();
    }

    public static Dictionary<string, string> LoadAvailableCOMservers()
    {
      if (ZR_Component.CommonGmmInterface.DeviceManager == null)
      {
        DeviceManager deviceManager = new DeviceManager();
        deviceManager.ClassicInitialisation();
        ZR_Component.CommonGmmInterface.DeviceManager = (object) deviceManager;
      }
      return ((DeviceManager) ZR_Component.CommonGmmInterface.DeviceManager).MyBus.LoadAvailableCOMservers();
    }

    private void RaiseBatterieLow(object sender, EventArgs e)
    {
      if (this.BatterieLow == null)
        return;
      this.BatterieLow(sender, e);
    }

    private void deviceCollector_OnProgress(object sender, int e)
    {
      if (this.OnProgress == null)
        return;
      DeviceManager.logger.Trace("Start Event_OnProgress");
      this.OnProgress(sender, e);
      DeviceManager.logger.Trace("End Event_OnProgress");
    }

    private void deviceCollector_OnProgressMessage(object sender, string e)
    {
      if (this.OnProgressMessage == null)
        return;
      DeviceManager.logger.Trace("Start Event_OnProgressMessage");
      this.OnProgressMessage(sender, e);
      DeviceManager.logger.Trace("End Event_OnProgressMessage");
    }

    internal virtual void OnValueIdentSetReceived(object sender, ValueIdentSet e)
    {
      EventHandler<ValueIdentSet> identSetReceived = this.ValueIdentSetReceived;
      if (identSetReceived == null)
        return;
      DeviceManager.logger.Trace("Start Event_ValueIdentSetReceived");
      identSetReceived(sender, e);
      DeviceManager.logger.Trace("End Event_ValueIdentSetReceived");
    }

    internal void RaiseEvent(GMM_EventArgs eventMessage)
    {
      if (eventMessage == null || this.OnMessage == null)
        return;
      DeviceManager.logger.Trace("Start Event_OnMessage");
      this.OnMessage((object) this, eventMessage);
      DeviceManager.logger.Trace("End Event_OnMessage");
    }

    internal void RaiseEventError(Exception exception)
    {
      if (exception == null || this.OnError == null)
        return;
      DeviceManager.logger.Trace("Start Event_OnError");
      this.OnError((object) this, exception);
      DeviceManager.logger.Trace("End Event_OnError");
    }

    public bool SetDeviceCollectorSettings(string deviceCollectorSettings)
    {
      this.DisposeHandler();
      return this.MyBus == null || this.MyBus.SetDeviceCollectorSettings(deviceCollectorSettings);
    }

    public bool SetDeviceCollectorSettings(
      SortedList<DeviceCollectorSettings, object> deviceCollectorSettings)
    {
      this.Dispose();
      return this.MyBus == null || this.MyBus.SetDeviceCollectorSettings(deviceCollectorSettings);
    }

    public bool SetDeviceCollectorSettings(SortedList<string, string> deviceCollectorSettings)
    {
      this.DisposeHandler();
      if (deviceCollectorSettings.ContainsKey("UsedHandler"))
        this.SavedSettings = deviceCollectorSettings;
      else if (this.MyBus != null)
        return this.MyBus.SetDeviceCollectorSettings(deviceCollectorSettings);
      return true;
    }

    public void SetDeviceCollectorSettings2(SortedList<string, string> deviceCollectorSettings)
    {
      if (deviceCollectorSettings.ContainsKey("UsedHandler"))
      {
        this.SavedSettings = deviceCollectorSettings;
      }
      else
      {
        if (this.MyBus == null)
          return;
        this.MyBus.SetDeviceCollectorSettings(deviceCollectorSettings);
      }
    }

    public bool SetAsyncComSettings(string asyncComSettings)
    {
      if (string.IsNullOrEmpty(asyncComSettings))
        return false;
      string[] strArray = asyncComSettings.Split(';');
      if (strArray.Length == 0)
        return false;
      SortedList<string, string> asyncComSettings1 = new SortedList<string, string>();
      for (int index = 0; index + 1 < strArray.Length; index += 2)
      {
        if (!asyncComSettings1.ContainsKey(strArray[index]))
          asyncComSettings1.Add(strArray[index], strArray[index + 1]);
        else
          DeviceManager.logger.Error("Multiple AsyncComSettings detected: {0}={1} and {2}={3}", new object[4]
          {
            (object) strArray[index],
            (object) strArray[index + 1],
            (object) strArray[index],
            (object) asyncComSettings1[strArray[index]]
          });
      }
      return this.SetAsyncComSettings(asyncComSettings1);
    }

    public bool SetAsyncComSettings(
      SortedList<AsyncComSettings, object> asyncComSettings)
    {
      if (asyncComSettings == null)
        return false;
      SortedList<string, string> asyncComSettings1 = new SortedList<string, string>();
      foreach (KeyValuePair<AsyncComSettings, object> asyncComSetting in asyncComSettings)
        asyncComSettings1.Add(asyncComSetting.Key.ToString(), asyncComSetting.Value.ToString());
      return this.SetAsyncComSettings(asyncComSettings1);
    }

    public bool SetAsyncComSettings(SortedList<string, string> asyncComSettings)
    {
      try
      {
        this.DisposeHandler();
        if (this.MyBus == null)
        {
          ZR_Component.CommonGmmInterface.GarantComponentLoaded(GMM_Components.DeviceCollector);
          this.MyBus = (DeviceCollectorFunctions) ZR_Component.CommonGmmInterface.LoadedComponentsList[GMM_Components.DeviceCollector];
        }
        this.MyBus.SetAsyncComSettings(asyncComSettings);
      }
      catch (Exception ex)
      {
        string str = "SetAsyncComSettings error: " + ex.Message;
        DeviceManager.logger.Error(ex, str);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, str);
        return false;
      }
      return true;
    }

    public void SetAsyncComSettings2(SortedList<string, string> asyncComSettings)
    {
      this.MyBus.SetAsyncComSettings(asyncComSettings);
    }

    public SortedList<string, string> GetAsyncComSettings()
    {
      try
      {
        if (this.MyBus != null)
          return this.MyBus.GetAsyncComSettings();
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "No structure loaded");
        return (SortedList<string, string>) null;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Get structure error: " + ex.Message);
        return (SortedList<string, string>) null;
      }
    }

    public void DeviceList_Clear()
    {
      try
      {
        this.GarantHandlerLoaded();
        if (this.SelectedHandler == null)
          return;
        this.SelectedHandler.ClearDeviceList();
      }
      catch (Exception ex)
      {
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        DeviceManager.logger.Error(ex, str);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, str);
      }
    }

    public bool GarantHandlerLoaded() => this.GarantHandlerLoaded(true);

    public bool GarantHandlerLoaded(bool addDymmyMBusDeviceIfSelectedDeviceNULL)
    {
      if (this.MyBus == null)
        return false;
      this.DeviceManagerMode = DeviceManagerModes.DeviceListConfiguratorControlled;
      ZR_ClassLibrary.BusMode baseMode = this.MyBus.GetBaseMode();
      switch (baseMode)
      {
        case ZR_ClassLibrary.BusMode.MBusPointToPoint:
          return this.InitializeMBusDeviceHandler(addDymmyMBusDeviceIfSelectedDeviceNULL);
        case ZR_ClassLibrary.BusMode.MBus:
          this.DeviceManagerMode = DeviceManagerModes.DeviceListBusControlled;
          return this.InitializeMBusDeviceHandler(addDymmyMBusDeviceIfSelectedDeviceNULL);
        case ZR_ClassLibrary.BusMode.MinomatV2:
        case ZR_ClassLibrary.BusMode.MinomatV3:
        case ZR_ClassLibrary.BusMode.MinomatV4:
          this.InitializeMinomatHandler();
          return true;
        case ZR_ClassLibrary.BusMode.MinomatRadioTest:
        case ZR_ClassLibrary.BusMode.Radio2:
        case ZR_ClassLibrary.BusMode.Radio3:
        case ZR_ClassLibrary.BusMode.Radio4:
        case ZR_ClassLibrary.BusMode.wMBusS1:
        case ZR_ClassLibrary.BusMode.wMBusS1M:
        case ZR_ClassLibrary.BusMode.wMBusS2:
        case ZR_ClassLibrary.BusMode.wMBusT1:
        case ZR_ClassLibrary.BusMode.wMBusT2_meter:
        case ZR_ClassLibrary.BusMode.wMBusT2_other:
        case ZR_ClassLibrary.BusMode.wMBusC1A:
        case ZR_ClassLibrary.BusMode.wMBusC1B:
        case ZR_ClassLibrary.BusMode.Radio3_868_95_RUSSIA:
        case ZR_ClassLibrary.BusMode.RadioMS:
          this.InitializeWalkByHandler();
          return true;
        case ZR_ClassLibrary.BusMode.Minol_Device:
          this.InitializeMinolDeviceHandler();
          return true;
        case ZR_ClassLibrary.BusMode.RelayDevice:
          this.InitializeRelayDeviceHandler();
          return true;
        case ZR_ClassLibrary.BusMode.SmokeDetector:
          this.InitializeSmokeDetectorHandler();
          return true;
        default:
          throw new ArgumentException("Can not initialize handler! Unknown bus mode: " + baseMode.ToString());
      }
    }

    public bool IsDeviceModified()
    {
      if (this.MyBus.IsDeviceModified())
        return true;
      return this.SelectedHandler != null && this.SelectedHandler.IsDevicesModified();
    }

    private void InitializeSmokeDetectorHandler()
    {
      if (this.SelectedHandler != null && !(this.SelectedHandler.GetType() != typeof (SmokeDetectorHandler)))
        return;
      this.SelectedHandler = (BaseDevice) new SmokeDetectorHandler(this);
      this.SelectedHandler.OnProgress += new EventHandlerEx<int>(this.SelectedHandler_OnProgress);
      this.SelectedHandler.OnProgressMessage += new EventHandlerEx<string>(this.SelectedHandler_OnProgressMessage);
    }

    private void InitializeWalkByHandler()
    {
      if (this.SelectedHandler != null && !(this.SelectedHandler.GetType() != typeof (WalkByHandler)))
        return;
      this.SelectedHandler = (BaseDevice) new WalkByHandler(this);
      this.SelectedHandler.OnProgress += new EventHandlerEx<int>(this.SelectedHandler_OnProgress);
      this.SelectedHandler.OnProgressMessage += new EventHandlerEx<string>(this.SelectedHandler_OnProgressMessage);
      this.SelectedHandler.ConnectionLost += new System.EventHandler(this.SelectedHandler_ConnectionLost);
    }

    private void InitializeRelayDeviceHandler()
    {
      if (this.SelectedHandler != null && !(this.SelectedHandler.GetType() != typeof (RelayDeviceHandler)))
        return;
      this.SelectedHandler = (BaseDevice) new RelayDeviceHandler(this);
      this.SelectedHandler.OnProgress += new EventHandlerEx<int>(this.SelectedHandler_OnProgress);
      this.SelectedHandler.OnProgressMessage += new EventHandlerEx<string>(this.SelectedHandler_OnProgressMessage);
    }

    private void InitializeMinomatHandler()
    {
      if (this.SelectedHandler != null && !(this.SelectedHandler.GetType() != typeof (MinomatHandler)))
        return;
      this.SelectedHandler = (BaseDevice) new MinomatHandler(this);
      this.SelectedHandler.OnProgress += new EventHandlerEx<int>(this.SelectedHandler_OnProgress);
      this.SelectedHandler.OnProgressMessage += new EventHandlerEx<string>(this.SelectedHandler_OnProgressMessage);
    }

    private void InitializeMinolDeviceHandler()
    {
      if (this.SelectedHandler != null && !(this.SelectedHandler.GetType() != typeof (MinolDeviceHandler)))
        return;
      this.SelectedHandler = (BaseDevice) new MinolDeviceHandler(this);
      this.SelectedHandler.OnProgress += new EventHandlerEx<int>(this.SelectedHandler_OnProgress);
      this.SelectedHandler.OnProgressMessage += new EventHandlerEx<string>(this.SelectedHandler_OnProgressMessage);
    }

    private bool InitializeMBusDeviceHandler(bool addDymmyMBusDeviceIfSelectedDeviceNULL)
    {
      BusDevice selectedDevice = this.MyBus.GetSelectedDevice();
      if (selectedDevice == null)
      {
        if (addDymmyMBusDeviceIfSelectedDeviceNULL && !this.MyBus.AddDevice(DeviceTypes.MBus, 0))
          return false;
        selectedDevice = this.MyBus.GetSelectedDevice();
        if (selectedDevice == null)
          return false;
      }
      if (selectedDevice.Info != null && (selectedDevice.Info.DeviceType == DeviceTypes.HumiditySensor || selectedDevice.Info.DeviceType == DeviceTypes.TemperatureSensor))
      {
        if (this.SelectedHandler == null || this.SelectedHandler.GetType() != typeof (ThHandler))
        {
          this.SelectedHandler = (BaseDevice) new ThHandler(this);
          this.SelectedHandler.OnProgress += new EventHandlerEx<int>(this.SelectedHandler_OnProgress);
          this.SelectedHandler.OnProgressMessage += new EventHandlerEx<string>(this.SelectedHandler_OnProgressMessage);
        }
        return true;
      }
      switch (selectedDevice)
      {
        case EDC _:
          if (this.SelectedHandler == null || this.SelectedHandler.GetType() != typeof (EdcHandler))
          {
            this.SelectedHandler = (BaseDevice) new EdcHandler(this);
            this.SelectedHandler.OnProgress += new EventHandlerEx<int>(this.SelectedHandler_OnProgress);
            this.SelectedHandler.OnProgressMessage += new EventHandlerEx<string>(this.SelectedHandler_OnProgressMessage);
            break;
          }
          break;
        case PDC _:
          if (this.SelectedHandler == null || this.SelectedHandler.GetType() != typeof (PdcHandler))
          {
            this.SelectedHandler = (BaseDevice) new PdcHandler(this);
            this.SelectedHandler.OnProgress += new EventHandlerEx<int>(this.SelectedHandler_OnProgress);
            this.SelectedHandler.OnProgressMessage += new EventHandlerEx<string>(this.SelectedHandler_OnProgressMessage);
            break;
          }
          break;
        case Serie3MBus _:
          if (this.ParameterType == ConfigurationParameter.ValueType.Complete)
          {
            if (this.SelectedHandler == null || !(this.SelectedHandler is Series3DeviceByHandler))
            {
              this.SelectedHandler = (BaseDevice) new Series3DeviceByHandler(this);
              this.SelectedHandler.OnProgress += new EventHandlerEx<int>(this.SelectedHandler_OnProgress);
              this.SelectedHandler.OnProgressMessage += new EventHandlerEx<string>(this.SelectedHandler_OnProgressMessage);
              break;
            }
            break;
          }
          if (this.SelectedHandler == null || this.SelectedHandler.GetType() != typeof (Series3Device))
          {
            this.SelectedHandler = (BaseDevice) new Series3Device(this);
            this.SelectedHandler.OnProgress += new EventHandlerEx<int>(this.SelectedHandler_OnProgress);
            this.SelectedHandler.OnProgressMessage += new EventHandlerEx<string>(this.SelectedHandler_OnProgressMessage);
          }
          break;
        case Serie2MBus _:
          if (this.ParameterType == ConfigurationParameter.ValueType.Complete)
          {
            if (this.SelectedHandler == null || this.SelectedHandler.GetType() != typeof (Series2DeviceByHandler))
            {
              this.SelectedHandler = (BaseDevice) new Series2DeviceByHandler(this);
              this.SelectedHandler.OnProgress += new EventHandlerEx<int>(this.SelectedHandler_OnProgress);
              this.SelectedHandler.OnProgressMessage += new EventHandlerEx<string>(this.SelectedHandler_OnProgressMessage);
              break;
            }
            break;
          }
          if (this.SelectedHandler == null || this.SelectedHandler.GetType() != typeof (Series2Device))
          {
            this.SelectedHandler = (BaseDevice) new Series2Device(this);
            this.SelectedHandler.OnProgress += new EventHandlerEx<int>(this.SelectedHandler_OnProgress);
            this.SelectedHandler.OnProgressMessage += new EventHandlerEx<string>(this.SelectedHandler_OnProgressMessage);
          }
          break;
        default:
          if (this.SelectedHandler == null || this.SelectedHandler.GetType() != typeof (MBusDeviceHandler))
          {
            this.SelectedHandler = (BaseDevice) new MBusDeviceHandler(this);
            this.SelectedHandler.OnProgress += new EventHandlerEx<int>(this.SelectedHandler_OnProgress);
            this.SelectedHandler.OnProgressMessage += new EventHandlerEx<string>(this.SelectedHandler_OnProgressMessage);
          }
          break;
      }
      return true;
    }

    public bool Open() => this.SelectedHandler != null && this.SelectedHandler.Open();

    public bool Close() => this.SelectedHandler != null && this.SelectedHandler.Close();

    public bool DeviceList_Read(StructureTreeNode structureTreeNode, List<long> filter)
    {
      try
      {
        this.GarantHandlerLoaded();
        this.BreakRequest = false;
        return this.SelectedHandler.Read(structureTreeNode, filter);
      }
      catch (AccessDeniedException ex)
      {
        throw ex;
      }
      catch (IOException ex)
      {
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        DeviceManager.logger.Error((Exception) ex, str);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, str);
        return false;
      }
      catch (Exception ex)
      {
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        DeviceManager.logger.Error(ex, str);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, str);
        return false;
      }
    }

    public bool DeviceList_ReadAll(List<long> filter)
    {
      try
      {
        if (!this.GarantHandlerLoaded())
          return false;
        this.BreakRequest = false;
        this.ParameterType = ConfigurationParameter.ValueType.Direct;
        if (DeviceManager.logger.IsTraceEnabled)
          DeviceManager.logger.Trace("ReadAll (Filter:" + ZR_ClassLibrary.Util.ArrayToString(filter, " ") + ")");
        return this.SelectedHandler.ReadAll(filter);
      }
      catch (IOException ex)
      {
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        DeviceManager.logger.Error((Exception) ex, str);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, str);
        return false;
      }
      catch (Exception ex)
      {
        string message = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        DeviceManager.logger.Error(ex, message);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, ex.Message);
        return false;
      }
    }

    private void SelectedHandler_OnProgress(object sender, int e)
    {
      if (this.OnProgress == null)
        return;
      this.OnProgress(sender, e);
    }

    private void SelectedHandler_OnProgressMessage(object sender, string e)
    {
      if (this.OnProgressMessage == null)
        return;
      this.OnProgressMessage(sender, e);
    }

    private void SelectedHandler_ConnectionLost(object sender, EventArgs e)
    {
      if (this.ConnectionLost == null)
        return;
      DeviceManager.logger.Trace("Start Event_ConnectionLost");
      this.ConnectionLost(sender, e);
      DeviceManager.logger.Trace("End Event_ConnectionLost");
    }

    public List<GlobalDeviceId> DeviceList_ReadAllAndGetList(
      ChoiceDeviceList choiceDeviceList,
      List<long> filter)
    {
      return this.DeviceList_ReadAll(filter) ? this.DeviceList_GetList(choiceDeviceList) : (List<GlobalDeviceId>) null;
    }

    public bool DeviceList_StartHKVEReceptionWindow()
    {
      this.BreakRequest = false;
      return this.MyBus.GetBaseMode() == ZR_ClassLibrary.BusMode.MinomatV2 ? this.MyBus.StartHKVEReceptionWindow() : throw new NotImplementedException("Only avaliable for Minomat bus mode");
    }

    public bool BeginSearchDevices()
    {
      this.GarantHandlerLoaded();
      this.BreakRequest = false;
      return this.SelectedHandler.BeginSearchDevices();
    }

    public bool DeviceList_SystemInit()
    {
      this.BreakRequest = false;
      return this.MyBus.GetBaseMode() == ZR_ClassLibrary.BusMode.MinomatV2 ? this.MyBus.SystemInit() : throw new NotImplementedException("Only avaliable for Minomat bus mode");
    }

    public List<GlobalDeviceId> DeviceList_GetList()
    {
      return this.DeviceList_GetList(ChoiceDeviceList.Readout);
    }

    public List<GlobalDeviceId> DeviceList_GetList(ChoiceDeviceList choiceDeviceList)
    {
      this.GarantHandlerLoaded();
      ZR_ClassLibrary.BusMode baseMode = this.MyBus.GetBaseMode();
      switch (baseMode)
      {
        case ZR_ClassLibrary.BusMode.MBusPointToPoint:
        case ZR_ClassLibrary.BusMode.MinomatV3:
        case ZR_ClassLibrary.BusMode.MinomatV4:
        case ZR_ClassLibrary.BusMode.MinomatRadioTest:
        case ZR_ClassLibrary.BusMode.Minol_Device:
        case ZR_ClassLibrary.BusMode.Radio2:
        case ZR_ClassLibrary.BusMode.Radio3:
        case ZR_ClassLibrary.BusMode.Radio4:
        case ZR_ClassLibrary.BusMode.wMBusS1:
        case ZR_ClassLibrary.BusMode.wMBusS1M:
        case ZR_ClassLibrary.BusMode.wMBusS2:
        case ZR_ClassLibrary.BusMode.wMBusT1:
        case ZR_ClassLibrary.BusMode.wMBusT2_meter:
        case ZR_ClassLibrary.BusMode.wMBusT2_other:
        case ZR_ClassLibrary.BusMode.wMBusC1A:
        case ZR_ClassLibrary.BusMode.wMBusC1B:
        case ZR_ClassLibrary.BusMode.Radio3_868_95_RUSSIA:
        case ZR_ClassLibrary.BusMode.RadioMS:
        case ZR_ClassLibrary.BusMode.SmokeDetector:
          return this.SelectedHandler == null ? (List<GlobalDeviceId>) null : this.SelectedHandler.GetGlobalDeviceIdList();
        case ZR_ClassLibrary.BusMode.MBus:
          return this.DeviceManagerMode == DeviceManagerModes.DeviceListBusControlled ? this.GetMBusDeviceListForConfiguration(choiceDeviceList) : this.GetMBusDeviceList(choiceDeviceList);
        case ZR_ClassLibrary.BusMode.MinomatV2:
          if (choiceDeviceList == ChoiceDeviceList.Readout)
            return this.GetMBusDeviceList(choiceDeviceList);
          return new List<GlobalDeviceId>()
          {
            new GlobalDeviceId()
            {
              MeterType = ValueIdent.ValueIdPart_MeterType.Collector,
              DeviceTypeName = "MINOMAT",
              FirmwareVersion = "2",
              Manufacturer = "MINOL"
            }
          };
        case ZR_ClassLibrary.BusMode.RelayDevice:
          return this.GetMBusDeviceList(choiceDeviceList);
        default:
          throw new ArgumentException("Can not get list! Unknown bus mode: " + baseMode.ToString());
      }
    }

    private List<GlobalDeviceId> GetMBusDeviceListForConfiguration(ChoiceDeviceList choiceDeviceList)
    {
      this.ConfiguratorDeviceList = new List<GlobalDeviceId>();
      this.DeviceCollectorDeviceIndexFromConfiguratorIndex = new List<int>();
      int numberOfDevices = this.MyBus.GetNumberOfDevices();
      for (int DeviceListIndex = 0; DeviceListIndex < numberOfDevices; ++DeviceListIndex)
      {
        DeviceInfo Info;
        if (this.MyBus.GetParameter(out Info, DeviceListIndex))
        {
          ValueIdent.ValueIdPart_MeterType meterType = ValueIdent.ConvertToMeterType(Info.Medium);
          if (Info.DeviceType == DeviceTypes.ZR_Serie3)
          {
            if (meterType != ValueIdent.ValueIdPart_MeterType.Heat && meterType != ValueIdent.ValueIdPart_MeterType.ChangeOverHeat && meterType != ValueIdent.ValueIdPart_MeterType.Cooling)
              continue;
          }
          else if (Info.DeviceType != DeviceTypes.ZR_Serie2)
            continue;
          GlobalDeviceId globalDeviceId = new GlobalDeviceId();
          this.ConfiguratorDeviceList.Add(globalDeviceId);
          this.DeviceCollectorDeviceIndexFromConfiguratorIndex.Add(DeviceListIndex);
          globalDeviceId.Serialnumber = Info.MeterNumber;
          globalDeviceId.Address = (int) Info.A_Field;
          globalDeviceId.MeterType = meterType;
          globalDeviceId.DeviceTypeName = ValueIdent.GetTranslatedValueNameForPartOfValueId((Enum) globalDeviceId.MeterType);
          globalDeviceId.Manufacturer = Info.Manufacturer;
          globalDeviceId.MeterNumber = Convert.ToString(Info.A_Field);
          globalDeviceId.Generation = Info.Version.ToString();
          globalDeviceId.FirmwareVersion = Info.Signature.ToString();
        }
        else
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Device info not available.");
          return (List<GlobalDeviceId>) null;
        }
      }
      return this.ConfiguratorDeviceList;
    }

    [Obsolete]
    private List<GlobalDeviceId> GetMBusDeviceList(ChoiceDeviceList choiceDeviceList)
    {
      ZR_ClassLibrary.BusMode baseMode = this.MyBus.GetBaseMode();
      List<GlobalDeviceId> mbusDeviceList = new List<GlobalDeviceId>();
      int numberOfDevices = this.MyBus.GetNumberOfDevices();
      for (int DeviceListIndex = 0; DeviceListIndex < numberOfDevices; ++DeviceListIndex)
      {
        DeviceInfo Info;
        if (this.MyBus.GetParameter(out Info, DeviceListIndex))
        {
          GlobalDeviceId globalDeviceId = new GlobalDeviceId();
          globalDeviceId.Serialnumber = Info.MeterNumber;
          if (baseMode == ZR_ClassLibrary.BusMode.MBus || baseMode == ZR_ClassLibrary.BusMode.MBusPointToPoint || baseMode == ZR_ClassLibrary.BusMode.RelayDevice || baseMode == ZR_ClassLibrary.BusMode.wMBusC1A || baseMode == ZR_ClassLibrary.BusMode.wMBusC1B || baseMode == ZR_ClassLibrary.BusMode.wMBusS1 || baseMode == ZR_ClassLibrary.BusMode.wMBusS1M || baseMode == ZR_ClassLibrary.BusMode.wMBusS2 || baseMode == ZR_ClassLibrary.BusMode.wMBusT1 || baseMode == ZR_ClassLibrary.BusMode.wMBusT2_meter || baseMode == ZR_ClassLibrary.BusMode.wMBusT2_other)
          {
            globalDeviceId.MeterType = ValueIdent.ConvertToMeterType(Info.Medium);
          }
          else
          {
            int num;
            switch (baseMode)
            {
              case ZR_ClassLibrary.BusMode.MinomatV2:
                if (!string.IsNullOrEmpty(globalDeviceId.Serialnumber))
                {
                  long int64 = ZR_ClassLibrary.Util.ConvertBcdInt64ToInt64((long) ParameterService.ConvertHexStringToUInt32(Info.MeterNumber));
                  globalDeviceId.MeterType = int64 <= 0L ? ValueIdent.ValueIdPart_MeterType.Any : NumberRanges.GetValueIdPart_MeterTypeOfMinolDevice(int64);
                }
                globalDeviceId.IsRegistered = Info.A_Field != byte.MaxValue && Info.A_Field >= (byte) 0 && Info.A_Field < (byte) 100;
                goto label_11;
              case ZR_ClassLibrary.BusMode.Radio2:
              case ZR_ClassLibrary.BusMode.Radio3:
              case ZR_ClassLibrary.BusMode.Radio4:
                num = 1;
                break;
              default:
                num = baseMode == ZR_ClassLibrary.BusMode.Radio3_868_95_RUSSIA ? 1 : 0;
                break;
            }
            globalDeviceId.MeterType = num == 0 ? (baseMode != ZR_ClassLibrary.BusMode.MinomatRadioTest ? ValueIdent.ValueIdPart_MeterType.Any : ValueIdent.ValueIdPart_MeterType.Other) : ValueIdent.ConvertToMeterType(Info.DeviceType);
          }
label_11:
          globalDeviceId.DeviceTypeName = ValueIdent.GetTranslatedValueNameForPartOfValueId((Enum) globalDeviceId.MeterType);
          globalDeviceId.Manufacturer = Info.Manufacturer;
          globalDeviceId.MeterNumber = Convert.ToString(Info.A_Field);
          globalDeviceId.Generation = Info.Version.ToString();
          globalDeviceId.FirmwareVersion = Info.Signature.ToString();
          mbusDeviceList.Add(globalDeviceId);
          if (choiceDeviceList == ChoiceDeviceList.Readout)
          {
            List<GlobalDeviceId> subDevices = TranslationRulesManager.GetSubDevices(Info.GetZDFParameterString());
            if (subDevices != null)
              globalDeviceId.SubDevices.AddRange((IEnumerable<GlobalDeviceId>) subDevices);
          }
        }
        else
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Device info not available.");
          return (List<GlobalDeviceId>) null;
        }
      }
      return mbusDeviceList;
    }

    public bool DeviceList_AddDevice(GlobalDeviceId device)
    {
      return this.DeviceList_AddDeviceList(new List<GlobalDeviceId>()
      {
        device
      });
    }

    public bool DeviceList_AddDeviceList(List<GlobalDeviceId> deviceList)
    {
      if (this.LoadedComponentInfo.CommunicationModel != 0)
        return true;
      if (deviceList == null)
        return false;
      this.GarantHandlerLoaded(false);
      this.BreakRequest = false;
      ZR_ClassLibrary.BusMode baseMode = this.MyBus.GetBaseMode();
      switch (baseMode)
      {
        case ZR_ClassLibrary.BusMode.MBusPointToPoint:
        case ZR_ClassLibrary.BusMode.MBus:
          foreach (GlobalDeviceId device in deviceList)
          {
            bool flag1 = !string.IsNullOrEmpty(device.MeterNumber);
            bool flag2 = !string.IsNullOrEmpty(device.Serialnumber);
            DeviceTypes NewType = DeviceTypes.MBus;
            if (!string.IsNullOrEmpty(device.DeviceTypeName))
            {
              if (Enum.IsDefined(typeof (DeviceTypes), (object) device.DeviceTypeName))
                NewType = (DeviceTypes) Enum.Parse(typeof (DeviceTypes), device.DeviceTypeName, true);
              else
                DeviceManager.logger.Fatal("Invalid device type! Value: " + device.DeviceTypeName);
            }
            if (flag1 && !flag2)
            {
              if (!this.MyBus.AddDevice(NewType, int.Parse(device.MeterNumber)))
                return false;
            }
            else
            {
              int PrimaryAddress = string.IsNullOrEmpty(device.MeterNumber) ? 0 : int.Parse(device.MeterNumber);
              string SerialNumber = string.IsNullOrEmpty(device.Serialnumber) ? string.Empty : device.Serialnumber;
              if (!this.MyBus.AddDevice(NewType, PrimaryAddress, SerialNumber))
                return false;
            }
          }
          return true;
        case ZR_ClassLibrary.BusMode.MinomatV2:
          DeviceCollectorFunctions myBus = this.MyBus;
          List<DeviceCollector.MinomatDevice> deviceList1 = new List<DeviceCollector.MinomatDevice>();
          foreach (GlobalDeviceId device in deviceList)
          {
            DeviceCollector.MinomatDevice minomatDevice = new DeviceCollector.MinomatDevice(myBus);
            minomatDevice.Info.MeterNumber = device.Serialnumber;
            deviceList1.Add(minomatDevice);
          }
          return myBus.RegisterHKVE(deviceList1);
        case ZR_ClassLibrary.BusMode.MinomatV3:
        case ZR_ClassLibrary.BusMode.MinomatV4:
        case ZR_ClassLibrary.BusMode.Minol_Device:
        case ZR_ClassLibrary.BusMode.Radio2:
        case ZR_ClassLibrary.BusMode.Radio3:
        case ZR_ClassLibrary.BusMode.Radio4:
        case ZR_ClassLibrary.BusMode.wMBusS1:
        case ZR_ClassLibrary.BusMode.wMBusS1M:
        case ZR_ClassLibrary.BusMode.wMBusS2:
        case ZR_ClassLibrary.BusMode.wMBusT1:
        case ZR_ClassLibrary.BusMode.wMBusT2_meter:
        case ZR_ClassLibrary.BusMode.wMBusT2_other:
        case ZR_ClassLibrary.BusMode.wMBusC1A:
        case ZR_ClassLibrary.BusMode.wMBusC1B:
        case ZR_ClassLibrary.BusMode.Radio3_868_95_RUSSIA:
        case ZR_ClassLibrary.BusMode.RadioMS:
        case ZR_ClassLibrary.BusMode.SmokeDetector:
          foreach (GlobalDeviceId device in deviceList)
          {
            if (device.Serialnumber != null)
            {
              DeviceManager.logger.Debug("Expected device: " + device.Serialnumber);
              if (!this.MyBus.AddDevice(DeviceTypes.Minol_Device, 0, device.Serialnumber))
              {
                ZR_ClassLibMessages.LastErrorInfo errorAndClearError = ZR_ClassLibMessages.GetLastErrorAndClearError();
                DeviceManager.logger.Error("Failed add device to expected devices! Error: " + errorAndClearError.LastErrorDescription);
              }
            }
          }
          return true;
        default:
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented, "The AddDevice method is not supported for this device! BusMode: " + baseMode.ToString());
          return false;
      }
    }

    public bool DeviceList_DeleteSelectedDevice()
    {
      this.BreakRequest = false;
      return this.MyBus.DeleteSelectedDevice();
    }

    public int DeviceList_GetIndexOfSelectedDevice()
    {
      int ofSelectedDevice = this.MyBus.GetIndexOfSelectedDevice();
      if (this.DeviceManagerMode != DeviceManagerModes.DeviceListBusControlled)
        return ofSelectedDevice;
      if (this.DeviceCollectorDeviceIndexFromConfiguratorIndex != null)
      {
        for (int index = 0; index < this.DeviceCollectorDeviceIndexFromConfiguratorIndex.Count; ++index)
        {
          if (this.DeviceCollectorDeviceIndexFromConfiguratorIndex[index] == ofSelectedDevice)
            return index;
        }
      }
      return -1;
    }

    public bool DeviceList_SelectDevice(GlobalDeviceId device)
    {
      if (device == null || string.IsNullOrEmpty(device.Serialnumber))
        return false;
      this.GarantHandlerLoaded();
      this.BreakRequest = false;
      return this.SelectedHandler.SelectDevice(device);
    }

    public bool DeviceList_SelectDevice(int deviceIndex)
    {
      return this.DeviceManagerMode != DeviceManagerModes.DeviceListBusControlled ? this.MyBus.SetSelectedDeviceByIndex(deviceIndex) : this.MyBus.SetSelectedDeviceByIndex(this.DeviceCollectorDeviceIndexFromConfiguratorIndex[deviceIndex]);
    }

    internal ZR_ClassLibrary.BusMode? GetCurrentBusMode()
    {
      SortedList<DeviceCollectorSettings, object> collectorSettings = this.MyBus.GetDeviceCollectorSettings();
      if (collectorSettings == null)
        return new ZR_ClassLibrary.BusMode?();
      return !collectorSettings.ContainsKey(DeviceCollectorSettings.BusMode) ? new ZR_ClassLibrary.BusMode?() : new ZR_ClassLibrary.BusMode?((ZR_ClassLibrary.BusMode) collectorSettings[DeviceCollectorSettings.BusMode]);
    }

    public SortedList<long, SortedList<DateTime, ReadingValue>> SaveValuesToDatabase(
      int meterId,
      int nodeId,
      SortedList<long, SortedList<DateTime, ReadingValue>> values)
    {
      lock (this.databaseMutex)
        return MeterDatabase.SaveMeterValues(meterId, nodeId, values);
    }
  }
}
