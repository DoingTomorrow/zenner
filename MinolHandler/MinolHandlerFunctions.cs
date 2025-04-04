// Decompiled with JetBrains decompiler
// Type: MinolHandler.MinolHandlerFunctions
// Assembly: MinolHandler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: A1A42975-0CFC-4FCB-838E-3BA18C5EABDC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinolHandler.dll

using AsyncCom;
using DeviceCollector;
using NLog;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace MinolHandler
{
  public class MinolHandlerFunctions
  {
    private static Logger logger = LogManager.GetLogger(nameof (MinolHandlerFunctions));
    private GMMConfig MyConfig;
    private MinolHandlerWindow MyWindow;

    public event EventHandlerEx<int> OnProgress;

    public MinolHandlerFunctions()
    {
      ZR_Component.CommonGmmInterface.GarantComponentLoaded(GMM_Components.AsyncCom);
      ZR_Component.CommonGmmInterface.GarantComponentLoaded(GMM_Components.DeviceCollector);
      this.MyCom = (IAsyncFunctions) ZR_Component.CommonGmmInterface.LoadedComponentsList[GMM_Components.AsyncCom];
      this.MyBus = (IDeviceCollector) ZR_Component.CommonGmmInterface.LoadedComponentsList[GMM_Components.DeviceCollector];
      this.MyConfig = (GMMConfig) ZR_Component.CommonGmmInterface.LoadedComponentsList[GMM_Components.KonfigGroup];
      this.MyDatabaseAccess = new DatabaseAccess(this);
      this.MyDevices = new AllDevices(this);
      string str = this.MyConfig.GetValue(GMM_Components.MinolHandler.ToString(), nameof (DailySave));
      if (str != "")
        this.DailySave = bool.Parse(str);
      this.IsMinoConnectKeyEventEnabled = false;
      this.IsBusy = false;
      this.MyDatabaseAccess.GetTelegramParameters(20359, 6, 156, out string _, out int _, out int _, out SortedList<string, List<TelegramParameter>> _);
    }

    public MinolHandlerFunctions(IDeviceCollector deviceCollector)
    {
      this.MyCom = deviceCollector.AsyncCom;
      this.MyBus = deviceCollector;
      this.MyDatabaseAccess = new DatabaseAccess(this);
      this.MyDevices = new AllDevices(this);
      this.DailySave = false;
      this.IsMinoConnectKeyEventEnabled = false;
      this.IsBusy = false;
      this.MyDatabaseAccess.GetTelegramParameters(20359, 6, 156, out string _, out int _, out int _, out SortedList<string, List<TelegramParameter>> _);
    }

    public void GMM_Dispose()
    {
      this.MyBus.ComClose();
      this.MyBus.Dispose();
      if (this.MyConfig == null)
        return;
      this.MyConfig.SetOrUpdateValue(GMM_Components.MinolHandler.ToString(), "DailySave", this.DailySave.ToString());
    }

    internal IDeviceCollector MyBus { get; private set; }

    internal IAsyncFunctions MyCom { get; private set; }

    public AllDevices MyDevices { get; private set; }

    internal DatabaseAccess MyDatabaseAccess { get; private set; }

    public bool IsBusy { get; private set; }

    public bool IsMinoConnectKeyEventEnabled { get; set; }

    internal bool DailySave { get; set; }

    public string ShowMinolHandlerWindow()
    {
      if (this.MyWindow == null)
        this.MyWindow = new MinolHandlerWindow(this);
      int num = (int) this.MyWindow.ShowDialog();
      return this.MyWindow.StartComponentName;
    }

    public bool WriteChangesToDevice() => this.WriteChangesToDevice(false, out DateTime _);

    public bool WriteChangesToDevice(bool saveToDatabase, out DateTime backupTimePoint)
    {
      return this.MyDevices.WriteDevice(saveToDatabase, out backupTimePoint);
    }

    public bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList)
    {
      if (this.MyDevices.WorkDevice == null)
      {
        ValueList = (SortedList<long, SortedList<DateTime, ReadingValue>>) null;
        return false;
      }
      if (!this.MyDevices.WorkDevice.GetValues(ref ValueList))
        return false;
      ValueIdent.CleanUpEmptyValueIdents(ValueList);
      return true;
    }

    public bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList,
      int SubDevice)
    {
      if (this.MyDevices.WorkDevice == null)
      {
        ValueList = (SortedList<long, SortedList<DateTime, ReadingValue>>) null;
        return false;
      }
      if (!this.MyDevices.WorkDevice.GetValues(ref ValueList, SubDevice))
        return false;
      ValueIdent.CleanUpEmptyValueIdents(ValueList);
      return true;
    }

    public bool ReadValues(out GlobalDeviceId UpdatedDeviceIdentification)
    {
      UpdatedDeviceIdentification = (GlobalDeviceId) null;
      if (this.IsBusy)
      {
        MinolHandlerFunctions.logger.Error("Abort an asynchronous method call! The MinolHandler is busy.");
        return false;
      }
      try
      {
        this.IsBusy = true;
        this.MyDevices.OnProgress += new EventHandlerEx<int>(this.MyDevices_OnProgress);
        if (!this.ReadDevice(ReadMode.Complete))
          return false;
        UpdatedDeviceIdentification = this.GetGlobalDeviceId();
        return true;
      }
      finally
      {
        this.IsBusy = false;
        this.MyDevices.OnProgress -= new EventHandlerEx<int>(this.MyDevices_OnProgress);
      }
    }

    public bool ReadConfigurationParameters(out GlobalDeviceId UpdatedDeviceIdentification)
    {
      return this.ReadConfigurationParameters(out UpdatedDeviceIdentification, ReadMode.Ident);
    }

    public bool ReadConfigurationParameters(
      out GlobalDeviceId UpdatedDeviceIdentification,
      ReadMode mode)
    {
      UpdatedDeviceIdentification = (GlobalDeviceId) null;
      if (this.IsBusy)
      {
        MinolHandlerFunctions.logger.Error("Abort an asynchronous method call! The MinolHandler is busy.");
        return false;
      }
      try
      {
        this.IsBusy = true;
        this.MyDevices.OnProgress += new EventHandlerEx<int>(this.MyDevices_OnProgress);
        if (!this.ReadDevice(mode))
          return false;
        UpdatedDeviceIdentification = this.GetGlobalDeviceId();
        return true;
      }
      finally
      {
        this.IsBusy = false;
        this.MyDevices.OnProgress -= new EventHandlerEx<int>(this.MyDevices_OnProgress);
      }
    }

    public object GetConfigurationParameter(string key)
    {
      return this.MyDevices.WorkDevice.GetReadValue(key);
    }

    public SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      ConfigurationParameter.ValueType ConfigurationType)
    {
      if (this.MyDevices.WorkDevice != null)
        return this.MyDevices.WorkDevice.GetConfigurationParameters(ConfigurationType);
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.OperationCancelled, "No configuration parameter found! Please use ReadConfigurationParameters function first!");
      return (SortedList<OverrideID, ConfigurationParameter>) null;
    }

    public SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      ConfigurationParameter.ValueType ConfigurationType,
      int SubDevice)
    {
      if (this.MyDevices.WorkDevice != null)
        return this.MyDevices.WorkDevice.GetConfigurationParameters(ConfigurationType, SubDevice);
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.OperationCancelled, "No configuration parameter! Please use ReadConfigurationParameters function first!");
      return (SortedList<OverrideID, ConfigurationParameter>) null;
    }

    public bool SetConfigurationParameters(OverrideID parameterKey, object parameterValue)
    {
      if (this.MyDevices.WorkDevice == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "No MinolDevice.WorkDevice found!");
        return false;
      }
      SortedList<OverrideID, ConfigurationParameter> configurationParameters = this.MyDevices.WorkDevice.GetConfigurationParameters(ConfigurationParameter.ValueType.Complete);
      if (configurationParameters == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "No configuration parameter found!");
        return false;
      }
      if (!configurationParameters.ContainsKey(parameterKey))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, parameterKey.ToString() + " configuration parameter is not supported!");
        return false;
      }
      ConfigurationParameter configurationParameter = configurationParameters[parameterKey];
      SortedList<OverrideID, ConfigurationParameter> parameterList = new SortedList<OverrideID, ConfigurationParameter>();
      configurationParameter.ParameterValue = parameterValue;
      parameterList.Add(parameterKey, configurationParameter);
      return this.SetConfigurationParameters(parameterList);
    }

    public bool SetConfigurationParameter(string key, object value)
    {
      return this.MyDevices.WorkDevice.SetReadValue(key, value);
    }

    public bool SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameterList)
    {
      return this.SetConfigurationParameters(parameterList, 0);
    }

    public bool SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> ParameterList,
      int SubDevice)
    {
      if (ParameterList == null)
        return false;
      if (this.MyDevices.WorkDevice == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.OperationCancelled, "No device found! Please use ReadConfigurationParameters function first!");
        return false;
      }
      try
      {
        return this.MyDevices.WorkDevice.SetConfigurationParameters(ParameterList, SubDevice);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, ex.Message);
        return false;
      }
    }

    public GlobalDeviceId GetGlobalDeviceId()
    {
      return this.MyDevices.WorkDevice == null ? (GlobalDeviceId) null : this.MyDevices.WorkDevice.GetGlobalDeviceId();
    }

    public bool SelectDevice(GlobalDeviceId device)
    {
      if (this.MyDevices.WorkDevice == null)
        return false;
      GlobalDeviceId globalDeviceId = this.MyDevices.WorkDevice.GetGlobalDeviceId();
      if (globalDeviceId.Serialnumber == device.Serialnumber)
      {
        this.MyDevices.WorkDevice.SelectedDevice = device;
        return true;
      }
      if (globalDeviceId.SubDevices != null)
      {
        foreach (GlobalDeviceId subDevice in globalDeviceId.SubDevices)
        {
          if (subDevice.Serialnumber == device.Serialnumber)
          {
            this.MyDevices.WorkDevice.SelectedDevice = device;
            return true;
          }
        }
      }
      return false;
    }

    public bool ReadDevice() => this.ReadDevice(false, out DateTime _);

    public bool ReadDevice(bool saveToDatabase, out DateTime backupTimePoint)
    {
      return this.MyDevices.ReadAndCreateDevice(saveToDatabase, out backupTimePoint);
    }

    public bool ReadDevice(ReadMode mode, bool saveToDatabase, out DateTime backupTimePoint)
    {
      return this.MyDevices.ReadAndCreateDevice(mode, saveToDatabase, out backupTimePoint);
    }

    public bool ReadDevice(ReadMode mode)
    {
      try
      {
        this.MyDevices.OnProgress += new EventHandlerEx<int>(this.MyDevices_OnProgress);
        return this.MyDevices.ReadAndCreateDevice(mode);
      }
      finally
      {
        this.MyDevices.OnProgress -= new EventHandlerEx<int>(this.MyDevices_OnProgress);
      }
    }

    private void MyDevices_OnProgress(object sender, int e)
    {
      if (this.OnProgress == null)
        return;
      this.OnProgress(sender, e);
    }

    public DeviceTypes GetDeviceType() => this.MyDevices.WorkDevice.DeviceType;

    public ISF_TestStationData GetISFTestStationData()
    {
      return ISF_TestStationData.GetTestStationData(this.MyDevices.WorkDevice.Map);
    }

    public void SetISFTestStationData(ISF_TestStationData data)
    {
      ISF_TestStationData.SetTestStationData(this.MyDevices.WorkDevice, data);
    }

    public bool Merge(AddressRange addressRange)
    {
      if (addressRange == AddressRange.BaseType)
        return this.MyDevices.Merge(addressRange, this.MyDevices.TypeDevice);
      throw new NotImplementedException("Unknown address range: " + addressRange.ToString());
    }

    public bool LoadTypeFromDatabase(int MeterInfoID)
    {
      return this.MyDevices.LoadTypeFromDatabase(MeterInfoID);
    }

    public bool LoadFromDatabase(int MeterID, DateTime timePoint)
    {
      return this.MyDevices.LoadFromDatabase(MeterID, timePoint);
    }

    public SortedList<int, short> GetMemoryMap() => this.MyDevices.WorkDevice.GetMemoryMap();

    public bool SetTestParameter(TestParameter testParameter)
    {
      return this.MyDevices.WorkDevice.SetTestParameter(testParameter);
    }

    public bool IsMemoryMapEqual(SortedList<int, short> expectedMemoryMap) => true;

    public bool SaveToDatabase(out DateTime backupTimePoint)
    {
      return this.MyDevices.SaveToDatabase(out backupTimePoint);
    }

    public bool SaveToDatabase(
      int meterInfoID,
      string OrderNumber,
      string TheSerialNumber,
      out DateTime backupTimePoint)
    {
      return this.MyDevices.SaveToDatabase(meterInfoID, OrderNumber, TheSerialNumber, out backupTimePoint);
    }
  }
}
