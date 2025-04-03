// Decompiled with JetBrains decompiler
// Type: Devices.MinomatHandler
// Assembly: Devices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 793FC2DA-FF88-4FD5-BDE9-C00C0310F1EC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Devices.dll

using DeviceCollector;
using MinomatHandler;
using NLog;
using StartupLib;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace Devices
{
  public class MinomatHandler(DeviceManager MyDeviceManager) : BaseDevice(MyDeviceManager)
  {
    private SortedList<OverrideID, ConfigurationParameter> changedParameters;
    private Minomat.MinomatInfo minomatV2Info;
    private static Logger logger = LogManager.GetLogger(nameof (MinomatHandler));
    private MinomatV4 minomatV4;
    private MeasurementSet readedValues;
    private string selectedDevice;
    private GMM_EventArgs message = new GMM_EventArgs("");

    public override event EventHandlerEx<int> OnProgress;

    public override void Dispose()
    {
      if (this.minomatV4 != null)
      {
        this.minomatV4.OnMessage -= new EventHandler<MinomatV4.StateEventArgs>(this.MinomatV4_OnMessage);
        this.minomatV4.OnError -= new EventHandlerEx<Exception>(this.minomatV4_OnError);
        this.minomatV4.Dispose();
        this.minomatV4 = (MinomatV4) null;
      }
      this.readedValues = (MeasurementSet) null;
    }

    public override object GetHandler()
    {
      this.LoadMinomatHandler();
      return (object) this.minomatV4;
    }

    public override List<GlobalDeviceId> GetGlobalDeviceIdList()
    {
      List<GlobalDeviceId> globalDeviceIdList = new List<GlobalDeviceId>();
      int numberOfDevices = this.MyDeviceManager.MyBus.GetNumberOfDevices();
      for (int index = 0; index < numberOfDevices && this.MyDeviceManager.MyBus.SetSelectedDeviceByIndex(index); ++index)
      {
        DeviceInfo Info;
        if (!this.MyDeviceManager.MyBus.GetParameter(out Info) || Info == null)
          return (List<GlobalDeviceId>) null;
        globalDeviceIdList.Add(new GlobalDeviceId()
        {
          Serialnumber = Info.MeterNumber,
          DeviceTypeName = NumberRanges.GetTypeOfMinolDevice(Info.MeterNumber).ToString(),
          Manufacturer = "MINOL"
        });
      }
      return globalDeviceIdList;
    }

    public override bool SelectDevice(GlobalDeviceId device)
    {
      if (device == null)
        return false;
      BusMode? currentBusMode = this.MyDeviceManager.GetCurrentBusMode();
      BusMode? nullable = currentBusMode;
      BusMode busMode1 = BusMode.MinomatV2;
      if (nullable.GetValueOrDefault() == busMode1 & nullable.HasValue)
        return this.MyDeviceManager.MyBus.SetSelectedDeviceBySerialNumber(device.Serialnumber);
      nullable = currentBusMode;
      BusMode busMode2 = BusMode.MinomatV3;
      int num;
      if (!(nullable.GetValueOrDefault() == busMode2 & nullable.HasValue))
      {
        nullable = currentBusMode;
        BusMode busMode3 = BusMode.MinomatV4;
        num = nullable.GetValueOrDefault() == busMode3 & nullable.HasValue ? 1 : 0;
      }
      else
        num = 1;
      if (num == 0)
        return false;
      this.selectedDevice = device.Serialnumber;
      return true;
    }

    public override bool Read(StructureTreeNode structureTreeNode, List<long> filter)
    {
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      BusMode? currentBusMode = this.MyDeviceManager.GetCurrentBusMode();
      BusMode? nullable = currentBusMode;
      BusMode busMode1 = BusMode.MinomatV3;
      int num;
      if (!(nullable.GetValueOrDefault() == busMode1 & nullable.HasValue))
      {
        nullable = currentBusMode;
        BusMode busMode2 = BusMode.MinomatV4;
        num = nullable.GetValueOrDefault() == busMode2 & nullable.HasValue ? 1 : 0;
      }
      else
        num = 1;
      if (num == 0)
        return false;
      if (this.readedValues == null)
        this.readedValues = new MeasurementSet();
      this.readedValues.Clear();
      return this.ReadValuesOfMinomatV4(structureTreeNode.SerialNumber, filter);
    }

    public override bool ReadAll(List<long> filter)
    {
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      this.LoadMinomatHandler();
      BusMode? currentBusMode = this.MyDeviceManager.GetCurrentBusMode();
      BusMode? nullable = currentBusMode;
      BusMode busMode1 = BusMode.MinomatV3;
      int num;
      if (!(nullable.GetValueOrDefault() == busMode1 & nullable.HasValue))
      {
        nullable = currentBusMode;
        BusMode busMode2 = BusMode.MinomatV4;
        num = nullable.GetValueOrDefault() == busMode2 & nullable.HasValue ? 1 : 0;
      }
      else
        num = 1;
      if (num != 0)
      {
        bool flag = this.ReadMinomatV4(filter);
        if (this.readedValues != null)
        {
          List<uint> uintList = new List<uint>((IEnumerable<uint>) this.readedValues.Keys);
          int count = uintList.Count;
          foreach (uint id in uintList)
            this.FireEventOnValueIdentSetReceived(id, count);
        }
        return flag;
      }
      nullable = currentBusMode;
      BusMode busMode3 = BusMode.MinomatV2;
      if (!(nullable.GetValueOrDefault() == busMode3 & nullable.HasValue) || !this.MyDeviceManager.MyBus.ScanFromAddress(0))
        return false;
      List<GlobalDeviceId> globalDeviceIdList = this.GetGlobalDeviceIdList();
      int count1 = globalDeviceIdList.Count;
      foreach (GlobalDeviceId id in globalDeviceIdList)
        this.FireEventOnValueIdentSetReceived(id, count1);
      return true;
    }

    public override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList,
      string serialnumber)
    {
      BusMode? currentBusMode = this.MyDeviceManager.GetCurrentBusMode();
      BusMode? nullable1 = currentBusMode;
      BusMode busMode1 = BusMode.MinomatV2;
      if (nullable1.GetValueOrDefault() == busMode1 & nullable1.HasValue)
        return this.GetValuesFromMinomatV2(ref ValueList, serialnumber);
      BusMode? nullable2 = currentBusMode;
      BusMode busMode2 = BusMode.MinomatV3;
      int num;
      if (!(nullable2.GetValueOrDefault() == busMode2 & nullable2.HasValue))
      {
        nullable2 = currentBusMode;
        BusMode busMode3 = BusMode.MinomatV4;
        num = nullable2.GetValueOrDefault() == busMode3 & nullable2.HasValue ? 1 : 0;
      }
      else
        num = 1;
      return num != 0 && this.GetValuesFromMinomatV4(ref ValueList, serialnumber);
    }

    public override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList)
    {
      BusMode? currentBusMode = this.MyDeviceManager.GetCurrentBusMode();
      BusMode? nullable1 = currentBusMode;
      BusMode busMode1 = BusMode.MinomatV2;
      if (nullable1.GetValueOrDefault() == busMode1 & nullable1.HasValue)
      {
        if (!this.GetValuesFromMinomatV2(ref ValueList))
          return false;
        ValueIdent.CleanUpEmptyValueIdents(ValueList);
        return true;
      }
      BusMode? nullable2 = currentBusMode;
      BusMode busMode2 = BusMode.MinomatV3;
      int num;
      if (!(nullable2.GetValueOrDefault() == busMode2 & nullable2.HasValue))
      {
        nullable2 = currentBusMode;
        BusMode busMode3 = BusMode.MinomatV4;
        num = nullable2.GetValueOrDefault() == busMode3 & nullable2.HasValue ? 1 : 0;
      }
      else
        num = 1;
      if (num == 0 || string.IsNullOrEmpty(this.selectedDevice) || !this.GetValuesFromMinomatV4(ref ValueList, this.selectedDevice))
        return false;
      ValueIdent.CleanUpEmptyValueIdents(ValueList);
      return true;
    }

    public override bool ReadConfigurationParameters(out GlobalDeviceId UpdatedDeviceIdentification)
    {
      UpdatedDeviceIdentification = (GlobalDeviceId) null;
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      string str = Convert.ToString(this.MyDeviceManager.MyBus.GetDeviceCollectorSettings()[DeviceCollectorSettings.DaKonId]);
      if (string.IsNullOrEmpty(str))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "The DeviceCollectorSettings.DaKonId is missing!");
        return false;
      }
      object InfoObject;
      this.MyDeviceManager.MyBus.GetDeviceCollectorInfo(out InfoObject);
      if (!(InfoObject is Minomat.MinomatInfo minomatInfo) || minomatInfo.configuration == null || minomatInfo.systemStatus == null || minomatInfo.systemTime == DateTime.MinValue)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Failed to read Minomat settings!");
        return false;
      }
      this.minomatV2Info = minomatInfo;
      UpdatedDeviceIdentification = new GlobalDeviceId();
      UpdatedDeviceIdentification.Serialnumber = str;
      UpdatedDeviceIdentification.MeterType = ValueIdent.ValueIdPart_MeterType.Collector;
      UpdatedDeviceIdentification.Manufacturer = "MINOL";
      if (minomatInfo.systemStatus != null && minomatInfo.systemStatus is MinomatV2.SystemStatus)
      {
        MinomatV2.SystemStatus systemStatus = (MinomatV2.SystemStatus) minomatInfo.systemStatus;
        if (!string.IsNullOrEmpty(systemStatus.FirmwareVersionAsString))
          UpdatedDeviceIdentification.FirmwareVersion = systemStatus.FirmwareVersionAsString;
      }
      return true;
    }

    public override SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      ConfigurationParameter.ValueType ConfigurationType,
      int SubDevice)
    {
      this.changedParameters = (SortedList<OverrideID, ConfigurationParameter>) null;
      BusMode? currentBusMode = this.MyDeviceManager.GetCurrentBusMode();
      BusMode busMode = BusMode.MinomatV2;
      if (!(currentBusMode.GetValueOrDefault() == busMode & currentBusMode.HasValue) || this.minomatV2Info == null)
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      string ParameterValue1 = Convert.ToString(this.MyDeviceManager.MyBus.GetDeviceCollectorSettings()[DeviceCollectorSettings.DaKonId]);
      if (!string.IsNullOrEmpty(ParameterValue1))
      {
        SortedList<OverrideID, ConfigurationParameter> r = new SortedList<OverrideID, ConfigurationParameter>();
        string ParameterValue2 = "Minomat";
        string ParameterValue3 = "";
        if (this.minomatV2Info.systemStatus != null && this.minomatV2Info.systemStatus is MinomatV2.SystemStatus)
          ParameterValue3 = ((MinomatV2.SystemStatus) this.minomatV2Info.systemStatus).FirmwareVersionAsString;
        if (UserManager.IsNewLicenseModel())
        {
          Devices.MinomatHandler.AddParam(false, r, OverrideID.SerialNumber, (object) ParameterValue1);
          Devices.MinomatHandler.AddParam(false, r, OverrideID.FirmwareVersion, (object) ParameterValue3);
          Devices.MinomatHandler.AddParam(false, r, OverrideID.DeviceName, (object) ParameterValue2);
          Devices.MinomatHandler.AddParam(false, r, OverrideID.InitDevice, (object) false, true, (string[]) null);
          Devices.MinomatHandler.AddParam(false, r, OverrideID.StartHKVEReceptionWindow, (object) false, true, (string[]) null);
          Devices.MinomatHandler.AddParam(false, r, OverrideID.DeviceClock, (object) this.minomatV2Info.systemTime);
        }
        else
        {
          r.Add(OverrideID.SerialNumber, new ConfigurationParameter(OverrideID.SerialNumber, (object) ParameterValue1));
          r.Add(OverrideID.FirmwareVersion, new ConfigurationParameter(OverrideID.FirmwareVersion, (object) ParameterValue3));
          r.Add(OverrideID.DeviceName, new ConfigurationParameter(OverrideID.DeviceName, (object) ParameterValue2));
          r.Add(OverrideID.InitDevice, new ConfigurationParameter(OverrideID.InitDevice, (object) false)
          {
            IsFunction = true,
            HasWritePermission = true
          });
          r.Add(OverrideID.StartHKVEReceptionWindow, new ConfigurationParameter(OverrideID.StartHKVEReceptionWindow, (object) false)
          {
            IsFunction = true,
            HasWritePermission = true
          });
          r.Add(OverrideID.DeviceClock, new ConfigurationParameter(OverrideID.DeviceClock, (object) this.minomatV2Info.systemTime)
          {
            HasWritePermission = true
          });
        }
        return r;
      }
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "The DeviceCollectorSettings.DaKonId is missing!");
      return (SortedList<OverrideID, ConfigurationParameter>) null;
    }

    private static void AddParam(
      bool canChanged,
      SortedList<OverrideID, ConfigurationParameter> r,
      OverrideID overrideID,
      object obj)
    {
      Devices.MinomatHandler.AddParam(canChanged, r, overrideID, obj, false, (string[]) null);
    }

    private static void AddParam(
      bool canChanged,
      SortedList<OverrideID, ConfigurationParameter> r,
      OverrideID overrideID,
      object obj,
      bool isFunction,
      string[] allowedValues)
    {
      if (!UserManager.IsConfigParamVisible(overrideID))
        return;
      bool flag = false;
      if (canChanged)
        flag = UserManager.IsConfigParamEditable(overrideID);
      r.Add(overrideID, new ConfigurationParameter(overrideID, obj)
      {
        HasWritePermission = flag,
        AllowedValues = allowedValues
      });
    }

    public override bool SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameterList,
      int SubDevice)
    {
      if (SubDevice != 0)
        return false;
      if (this.changedParameters == null)
        this.changedParameters = new SortedList<OverrideID, ConfigurationParameter>();
      if (parameterList == null || parameterList.Count <= 0)
        return false;
      foreach (KeyValuePair<OverrideID, ConfigurationParameter> parameter in parameterList)
      {
        if (this.changedParameters.ContainsKey(parameter.Key))
          this.changedParameters[parameter.Key] = parameter.Value;
        else
          this.changedParameters.Add(parameter.Key, parameter.Value);
      }
      return true;
    }

    public override bool SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameterList)
    {
      return this.SetConfigurationParameters(parameterList, 0);
    }

    public override bool WriteChangedConfigurationParametersToDevice()
    {
      if (this.changedParameters == null)
        return false;
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      try
      {
        bool flag = true;
        foreach (KeyValuePair<OverrideID, ConfigurationParameter> changedParameter in this.changedParameters)
        {
          if (changedParameter.Value.HasWritePermission)
          {
            switch (changedParameter.Key)
            {
              case OverrideID.DeviceClock:
                if (changedParameter.Value.ParameterValue.GetType() == typeof (DateTime))
                {
                  flag = this.MyDeviceManager.MyBus.SetTime((DateTime) changedParameter.Value.ParameterValue);
                  break;
                }
                break;
              case OverrideID.InitDevice:
                if (changedParameter.Value.ParameterValue.GetType() == typeof (bool) && (bool) changedParameter.Value.ParameterValue)
                {
                  flag = this.MyDeviceManager.MyBus.SystemInit();
                  break;
                }
                break;
              case OverrideID.StartHKVEReceptionWindow:
                if (changedParameter.Value.ParameterValue.GetType() == typeof (bool) && (bool) changedParameter.Value.ParameterValue)
                {
                  flag = this.MyDeviceManager.MyBus.StartHKVEReceptionWindow();
                  break;
                }
                break;
              case OverrideID.RegisterHKVE:
                if (changedParameter.Value.ParameterValue.GetType() == typeof (List<string>))
                {
                  flag = this.MyDeviceManager.MyBus.RegisterHKVE((List<string>) changedParameter.Value.ParameterValue);
                  break;
                }
                break;
              case OverrideID.DeregisterHKVE:
                if (changedParameter.Value.ParameterValue.GetType() == typeof (List<string>))
                {
                  flag = this.MyDeviceManager.MyBus.DeregisterHKVE((List<string>) changedParameter.Value.ParameterValue);
                  break;
                }
                break;
              default:
                ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented, "This OverrideID is not supported!");
                return false;
            }
            if (!flag)
              return false;
          }
        }
        this.changedParameters.Clear();
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        return false;
      }
    }

    public override bool ExecuteMethod(
      OverrideID overrideID,
      bool isSetMethod,
      out object result,
      object param1,
      object param2,
      object param3,
      object param4)
    {
      result = (object) null;
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      try
      {
        BusMode? currentBusMode = this.MyDeviceManager.GetCurrentBusMode();
        BusMode? nullable = currentBusMode;
        BusMode busMode1 = BusMode.MinomatV3;
        int num;
        if (!(nullable.GetValueOrDefault() == busMode1 & nullable.HasValue))
        {
          nullable = currentBusMode;
          BusMode busMode2 = BusMode.MinomatV4;
          num = nullable.GetValueOrDefault() == busMode2 & nullable.HasValue ? 1 : 0;
        }
        else
          num = 1;
        if (num != 0)
          return this.ExecuteMethodMinomatV4(overrideID, isSetMethod, out result, param1, param2, param3, param4);
        nullable = currentBusMode;
        BusMode busMode3 = BusMode.MinomatV2;
        return nullable.GetValueOrDefault() == busMode3 & nullable.HasValue && this.ExecuteMethodMinomatV2(overrideID, isSetMethod, out result, param1, param2, param3, param4);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddException(ex);
        return false;
      }
    }

    public override bool BeginSearchDevices()
    {
      BusMode? currentBusMode = this.MyDeviceManager.GetCurrentBusMode();
      BusMode? nullable1 = currentBusMode;
      BusMode busMode1 = BusMode.MinomatV3;
      BusMode? nullable2;
      int num;
      if (!(nullable1.GetValueOrDefault() == busMode1 & nullable1.HasValue))
      {
        nullable2 = currentBusMode;
        BusMode busMode2 = BusMode.MinomatV4;
        num = nullable2.GetValueOrDefault() == busMode2 & nullable2.HasValue ? 1 : 0;
      }
      else
        num = 1;
      if (num != 0)
        return this.BeginSearchDevicesOnMinomatV4();
      nullable2 = currentBusMode;
      BusMode busMode3 = BusMode.MinomatV2;
      return nullable2.GetValueOrDefault() == busMode3 & nullable2.HasValue && this.BeginSearchDevicesOnMinomatV2();
    }

    private void LoadMinomatHandler()
    {
      BusMode? currentBusMode = this.MyDeviceManager.GetCurrentBusMode();
      BusMode? nullable1 = currentBusMode;
      BusMode busMode1 = BusMode.MinomatV3;
      BusMode? nullable2;
      int num;
      if (!(nullable1.GetValueOrDefault() == busMode1 & nullable1.HasValue))
      {
        nullable2 = currentBusMode;
        BusMode busMode2 = BusMode.MinomatV4;
        num = nullable2.GetValueOrDefault() == busMode2 & nullable2.HasValue ? 1 : 0;
      }
      else
        num = 1;
      if (num != 0)
      {
        if (this.minomatV4 != null)
          return;
        this.minomatV4 = new MinomatV4(new SCGiConnection(this.MyDeviceManager.MyBus.AsyncCom));
        if (!string.IsNullOrEmpty(this.MyDeviceManager.MyBus.MinomatV4_SourceAddress) && Enum.IsDefined(typeof (SCGiAddress), (object) this.MyDeviceManager.MyBus.MinomatV4_SourceAddress))
        {
          this.minomatV4.Connection.SourceAddress = (SCGiAddress) Enum.Parse(typeof (SCGiAddress), this.MyDeviceManager.MyBus.MinomatV4_SourceAddress, true);
          if (this.minomatV4.Connection.SourceAddress == SCGiAddress.ServerHTTP || this.minomatV4.Connection.SourceAddress == SCGiAddress.ServerTCP)
            this.minomatV4.Authentication = (SCGiHeaderEx) null;
        }
        this.minomatV4.OnMessage += new EventHandler<MinomatV4.StateEventArgs>(this.MinomatV4_OnMessage);
        this.minomatV4.OnError += new EventHandlerEx<Exception>(this.minomatV4_OnError);
        SortedList<DeviceCollectorSettings, object> collectorSettings = this.MyDeviceManager.MyBus.GetDeviceCollectorSettings();
        if (collectorSettings != null && collectorSettings.ContainsKey(DeviceCollectorSettings.MaxRequestRepeat))
          this.minomatV4.MaxAttempt = Convert.ToInt32(collectorSettings[DeviceCollectorSettings.MaxRequestRepeat]);
      }
      else
      {
        nullable2 = currentBusMode;
        BusMode busMode3 = BusMode.MinomatV2;
        if (!(nullable2.GetValueOrDefault() == busMode3 & nullable2.HasValue))
        {
          nullable2 = currentBusMode;
          throw new ArgumentException("Can not load MinomatHandler! Reason: Wrong BusMode: " + nullable2.ToString());
        }
      }
    }

    private void minomatV4_OnError(object sender, Exception e)
    {
      this.MyDeviceManager.RaiseEventError(e);
    }

    private void MinomatV4_OnMessage(object sender, MinomatV4.StateEventArgs e)
    {
      this.message.EventMessage = e.Message;
      this.MyDeviceManager.RaiseEvent(this.message);
      this.minomatV4.CancelCurrentMethod = this.message.Cancel;
    }

    private bool BeginSearchDevicesOnMinomatV2()
    {
      if (this.OnProgress != null)
        this.OnProgress((object) this, 0);
      this.LoadMinomatHandler();
      List<string> registeredHkve = this.MyDeviceManager.MyBus.GetRegisteredHKVE();
      if (registeredHkve == null)
        return false;
      int num = 0;
      foreach (string funkId in registeredHkve)
      {
        if (this.OnProgress != null)
        {
          ++num;
          this.OnProgress((object) this, num * 100 / registeredHkve.Count);
        }
        ValueIdentSet e = new ValueIdentSet();
        e.SerialNumber = funkId.ToString();
        DeviceTypes typeOfMinolDevice = NumberRanges.GetTypeOfMinolDevice(funkId);
        e.DeviceType = typeOfMinolDevice.ToString();
        e.Manufacturer = NumberRanges.GetManufacturer(typeOfMinolDevice);
        e.ZDF = "SID;" + funkId + ";MAN;" + e.Manufacturer + ";MED;" + e.DeviceType;
        this.MyDeviceManager.OnValueIdentSetReceived((object) this, e);
      }
      return true;
    }

    private bool ExecuteMethodMinomatV2(
      OverrideID overrideID,
      bool isSetMethod,
      out object result,
      object param1,
      object param2,
      object param3,
      object param4)
    {
      result = (object) null;
      this.LoadMinomatHandler();
      switch (overrideID)
      {
        case OverrideID.InitDevice:
          return this.MyDeviceManager.MyBus.SystemInit();
        case OverrideID.StartHKVEReceptionWindow:
          return this.MyDeviceManager.MyBus.StartHKVEReceptionWindow();
        case OverrideID.RegisterHKVE:
          return this.MyDeviceManager.MyBus.RegisterHKVE((List<string>) param1);
        case OverrideID.DeregisterHKVE:
          return this.MyDeviceManager.MyBus.DeregisterHKVE((List<string>) param1);
        case OverrideID.RegisteredHKVE:
          result = (object) this.MyDeviceManager.MyBus.GetRegisteredHKVE();
          return result != null;
        case OverrideID.UnregisteredHKVE:
          result = (object) this.MyDeviceManager.MyBus.GetUnregisteredHKVE();
          return result != null;
        case OverrideID.MinomatV2Configuration:
          if (isSetMethod)
            return this.MyDeviceManager.MyBus.SetMinomatV2Configuration((MinomatV2.Configuration) param1);
          result = (object) this.MyDeviceManager.MyBus.GetMinomatV2Configuration();
          return result != null;
        case OverrideID.MinomatV2SystemStatus:
          result = (object) this.MyDeviceManager.MyBus.GetMinomatV2SystemStatus();
          return result != null;
        default:
          throw new ArgumentException("OverrideID is not supported! Value: " + overrideID.ToString());
      }
    }

    private bool GetValuesFromMinomatV2(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList)
    {
      return this.GetValuesFromMinomatV2(ref ValueList, (string) null);
    }

    private bool GetValuesFromMinomatV2(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList,
      string serialnumber)
    {
      if (!string.IsNullOrEmpty(serialnumber))
        this.MyDeviceManager.MyBus.SetSelectedDeviceBySerialNumber(serialnumber);
      if (ValueList == null || ValueList.Count == 0)
        ValueList = ValueIdent.GetDefaultValueListTemplate();
      DeviceInfo Info;
      if (!this.MyDeviceManager.MyBus.ReadParameter(out Info))
        return false;
      byte status = Info.Status;
      bool flag1 = ((int) status & 32) == 32;
      bool flag2 = ((int) status & 16) == 16;
      if (flag1)
      {
        ValueIdent.ValueIdPart_MeterType meterType = ValueIdent.ConvertToMeterType(Info.DeviceType);
        ValueList.Add(ValueIdent.GetValueIdentOfError(meterType, ValueIdent.ValueIdentError.Manipulation), new SortedList<DateTime, ReadingValue>()
        {
          {
            DateTime.Now,
            new ReadingValue() { value = 1.0 }
          }
        });
      }
      if (flag2)
      {
        ValueIdent.ValueIdPart_MeterType meterType = ValueIdent.ConvertToMeterType(Info.DeviceType);
        ValueList.Add(ValueIdent.GetValueIdentOfError(meterType, ValueIdent.ValueIdentError.DeviceError), new SortedList<DateTime, ReadingValue>()
        {
          {
            DateTime.Now,
            new ReadingValue() { value = 1.0 }
          }
        });
      }
      SortedList<string, SortedList<int, string>> sortedParameterList = this.GenerateSortedParameterList(Info);
      if (!sortedParameterList.ContainsKey("TIMP"))
        return true;
      SortedList<int, DateTime> sortedList1 = new SortedList<int, DateTime>();
      SortedList<int, string> sortedList2 = sortedParameterList["TIMP"];
      for (int index = 0; index < sortedList2.Count; ++index)
      {
        DateTime dateTime = DateTime.Parse(sortedList2.Values[index], (IFormatProvider) FixedFormates.TheFormates.DateTimeFormat);
        sortedList1.Add(sortedList2.Keys[index], dateTime);
      }
      for (int index1 = 0; index1 < ValueList.Keys.Count; ++index1)
      {
        if (ValueIdent.Contains(ValueList.Keys[index1], 281088648L))
        {
          int index2 = sortedParameterList.IndexOfKey("M_ST");
          if (index2 >= 0)
          {
            SortedList<int, string> sortedList3 = sortedParameterList.Values[index2];
            for (int index3 = 0; index3 < sortedList3.Count; ++index3)
            {
              if (!(sortedList3.Values[index3] == "NO_DATA"))
              {
                ReadingValue readingValue = new ReadingValue();
                switch (sortedParameterList["M_ST_STATUS"][sortedList3.Keys[index3]])
                {
                  case "READING_VALID":
                    readingValue.value = double.Parse(sortedList3.Values[index3], (IFormatProvider) FixedFormates.TheFormates.NumberFormat);
                    readingValue.state = ReadingValueState.ok;
                    readingValue.StateDetails = string.Empty;
                    break;
                  case "NOT_VALID":
                    readingValue.value = double.NaN;
                    readingValue.state = ReadingValueState.error;
                    readingValue.StateDetails = sortedParameterList["M_ST_STATUSDETAIL"][sortedList3.Keys[index3]];
                    break;
                  default:
                    continue;
                }
                ValueList.Values[index1].Add(sortedList1[sortedList3.Keys[index3]], readingValue);
              }
            }
          }
        }
        if (ValueIdent.Contains(ValueList.Keys[index1], 272703506L))
        {
          int index4 = sortedParameterList.IndexOfKey("M_RSSI");
          if (index4 >= 0)
          {
            SortedList<int, string> sortedList4 = sortedParameterList.Values[index4];
            for (int index5 = 0; index5 < sortedList4.Count; ++index5)
            {
              string s = sortedList4.Values[index5];
              if (!(s == "0"))
              {
                int rssiDBm = Util.RssiToRssi_dBm(byte.Parse(s));
                ValueList.Values[index1].Add(sortedList1[sortedList4.Keys[index5]], new ReadingValue()
                {
                  value = (double) rssiDBm,
                  state = ReadingValueState.ok,
                  StateDetails = string.Empty
                });
              }
            }
          }
        }
        if (ValueIdent.Contains(ValueList.Keys[index1], 289477256L))
        {
          int index6 = sortedParameterList.IndexOfKey("M_MO");
          if (index6 >= 0)
          {
            SortedList<int, string> sortedList5 = sortedParameterList.Values[index6];
            for (int index7 = 0; index7 < sortedList5.Count; ++index7)
            {
              if (!(sortedList5.Values[index7] == "NO_DATA"))
              {
                ReadingValue readingValue = new ReadingValue();
                switch (sortedParameterList["M_MO_STATUS"][sortedList5.Keys[index7]])
                {
                  case "READING_VALID":
                    readingValue.value = double.Parse(sortedList5.Values[index7], (IFormatProvider) FixedFormates.TheFormates.NumberFormat);
                    readingValue.state = ReadingValueState.ok;
                    readingValue.StateDetails = string.Empty;
                    break;
                  case "NOT_VALID":
                    readingValue.value = double.NaN;
                    readingValue.state = ReadingValueState.error;
                    readingValue.StateDetails = sortedParameterList["M_MO_STATUSDETAIL"][sortedList5.Keys[index7]];
                    break;
                  default:
                    continue;
                }
                ValueList.Values[index1].Add(sortedList1[sortedList5.Keys[index7]], readingValue);
              }
            }
          }
        }
        if (ValueIdent.Contains(ValueList.Keys[index1], 293671560L))
        {
          int index8 = sortedParameterList.IndexOfKey("M_HMO");
          if (index8 >= 0)
          {
            SortedList<int, string> sortedList6 = sortedParameterList.Values[index8];
            for (int index9 = 0; index9 < sortedList6.Count; ++index9)
            {
              if (!(sortedList6.Values[index9] == "NO_DATA"))
              {
                ReadingValue readingValue = new ReadingValue();
                switch (sortedParameterList["M_HMO_STATUS"][sortedList6.Keys[index9]])
                {
                  case "READING_VALID":
                    readingValue.value = double.Parse(sortedList6.Values[index9], (IFormatProvider) FixedFormates.TheFormates.NumberFormat);
                    readingValue.state = ReadingValueState.ok;
                    readingValue.StateDetails = string.Empty;
                    break;
                  case "NOT_VALID":
                    readingValue.value = double.NaN;
                    readingValue.state = ReadingValueState.error;
                    readingValue.StateDetails = sortedParameterList["M_HMO_STATUSDETAIL"][sortedList6.Keys[index9]];
                    break;
                  default:
                    continue;
                }
                ValueList.Values[index1].Add(sortedList1[sortedList6.Keys[index9]], readingValue);
              }
            }
          }
        }
      }
      return true;
    }

    private SortedList<string, SortedList<int, string>> GenerateSortedParameterList(
      DeviceInfo TheInfo)
    {
      SortedList<string, SortedList<int, string>> sortedParameterList = new SortedList<string, SortedList<int, string>>();
      try
      {
        for (int index1 = 0; index1 < TheInfo.ParameterList.Count; ++index1)
        {
          string defineString = TheInfo.ParameterList[index1].DefineString;
          string valueString = TheInfo.ParameterList[index1].ValueString;
          string[] strArray = defineString.Split('[');
          if (strArray.Length == 1)
            sortedParameterList.Add(defineString, new SortedList<int, string>()
            {
              {
                0,
                valueString
              }
            });
          else if (strArray.Length == 2)
          {
            int key = int.Parse(strArray[1].Replace("]", ""));
            int index2 = sortedParameterList.IndexOfKey(strArray[0]);
            if (index2 < 0)
              sortedParameterList.Add(strArray[0], new SortedList<int, string>()
              {
                {
                  key,
                  valueString
                }
              });
            else
              sortedParameterList.Values[index2].Add(key, valueString);
          }
          else
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Invalid Minomat V2 parameter: " + defineString);
        }
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Invalid Minomat V2 parameters. Reason: " + ex.Message);
      }
      return sortedParameterList;
    }

    private bool ReadMinomatV4(List<long> filter)
    {
      GMM_EventArgs eventMessage = new GMM_EventArgs(string.Empty);
      if (this.readedValues == null)
        this.readedValues = new MeasurementSet();
      this.readedValues.Clear();
      List<GlobalDeviceId> deviceList = this.MyDeviceManager.DeviceList_GetList();
      if (deviceList == null || deviceList.Count == 0)
      {
        try
        {
          eventMessage.EventMessage = "Read list of available devices...";
          this.MyDeviceManager.RaiseEvent(eventMessage);
          if (eventMessage.Cancel)
            return false;
          List<uint> registeredMessUnits = this.minomatV4.GetRegisteredMessUnits();
          if (registeredMessUnits == null)
            return false;
          deviceList = new List<GlobalDeviceId>();
          foreach (uint num in registeredMessUnits)
          {
            if (num > 0U && num < 99999999U)
              deviceList.Add(new GlobalDeviceId()
              {
                Serialnumber = num.ToString()
              });
          }
          this.MyDeviceManager.DeviceList_AddDeviceList(deviceList);
        }
        catch (Exception ex)
        {
          this.MyDeviceManager.RaiseEventError(ex);
          eventMessage.EventMessage = "Can not read the list of registered devices. " + ex.Message;
          this.MyDeviceManager.RaiseEvent(eventMessage);
          if (eventMessage.Cancel)
            return false;
        }
        eventMessage.EventMessage = deviceList.Count.ToString() + " devices available";
        this.MyDeviceManager.RaiseEvent(eventMessage);
        if (eventMessage.Cancel)
          return false;
      }
      return this.ReadValuesOfMinomatV4(filter);
    }

    private bool GetValuesFromMinomatV4(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList,
      string serialnumber)
    {
      List<GlobalDeviceId> list = this.MyDeviceManager.DeviceList_GetList();
      if (list == null || list.Count == 0)
        return true;
      if (string.IsNullOrEmpty(serialnumber))
        throw new ArgumentNullException("Input parameter 'serialnumber' can not be null!");
      uint num = 0;
      if (!Util.TryParseToUInt32(serialnumber, out num))
        throw new ArgumentException("Can not parse input parameter 'serialnumber' to UInt32! Value: " + serialnumber);
      ValueIdent.ValueIdPart_MeterType typeOfMinolDevice = NumberRanges.GetValueIdPart_MeterTypeOfMinolDevice((long) num);
      List<long> filter = (List<long>) null;
      if (ValueList != null)
      {
        if (ValueList.Count > 0)
          filter = new List<long>((IEnumerable<long>) ValueList.Keys);
      }
      else
        ValueList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
      if (!this.readedValues.ContainsKey(num))
      {
        if (list.Exists((Predicate<GlobalDeviceId>) (x => x.Serialnumber == serialnumber)))
        {
          long valueIdentOfWarninig = ValueIdent.GetValueIdentOfWarninig(typeOfMinolDevice, ValueIdent.ValueIdentWarning.FailedToRead, ValueIdent.ValueIdPart_Creation.ReadingSystem);
          ReadingValue readingValue = new ReadingValue()
          {
            value = 1.0
          };
          ValueList.Add(valueIdentOfWarninig, new SortedList<DateTime, ReadingValue>()
          {
            {
              DateTime.Now,
              readingValue
            }
          });
        }
        return true;
      }
      Dictionary<MeasurementDataType, MeasurementData> readedValue = this.readedValues[num];
      NumberRanges.GetTypeOfMinolDevice((long) num);
      MinomatV4.AddValues(ValueList, readedValue, typeOfMinolDevice, filter, ValueIdent.ValueIdPart_StorageInterval.Day, MeasurementDataType.Day);
      MinomatV4.AddValues(ValueList, readedValue, typeOfMinolDevice, filter, ValueIdent.ValueIdPart_StorageInterval.Month, MeasurementDataType.MonthAndHalfMonth);
      MinomatV4.AddValues(ValueList, readedValue, typeOfMinolDevice, filter, ValueIdent.ValueIdPart_StorageInterval.DueDate, MeasurementDataType.DueDate);
      MinomatV4.AddValues(ValueList, readedValue, typeOfMinolDevice, filter, ValueIdent.ValueIdPart_StorageInterval.QuarterHour, MeasurementDataType.Quarter);
      return true;
    }

    private void AddErrorValue(
      SortedList<long, SortedList<DateTime, ReadingValue>> ValueList,
      ValueIdent.ValueIdPart_MeterType meterType,
      ValueIdent.ValueIdentError error,
      DateTime? timepoint)
    {
      long valueIdentOfError = ValueIdent.GetValueIdentOfError(meterType, error);
      ReadingValue readingValue = new ReadingValue();
      readingValue.value = 1.0;
      SortedList<DateTime, ReadingValue> sortedList = new SortedList<DateTime, ReadingValue>();
      if (timepoint.HasValue)
      {
        sortedList.Add(timepoint.Value, readingValue);
      }
      else
      {
        DateTime now = DateTime.Now;
        sortedList.Add(new DateTime(now.Year, now.Month, now.Day), readingValue);
      }
      ValueList.Add(valueIdentOfError, sortedList);
    }

    private bool BeginSearchDevicesOnMinomatV4()
    {
      if (this.OnProgress != null)
        this.OnProgress((object) this, 0);
      this.LoadMinomatHandler();
      List<uint> registeredMessUnits = this.minomatV4.GetRegisteredMessUnits();
      if (registeredMessUnits == null)
        return false;
      int num = 0;
      foreach (uint funkId in registeredMessUnits)
      {
        if (this.OnProgress != null)
        {
          ++num;
          this.OnProgress((object) this, num * 100 / registeredMessUnits.Count);
        }
        ValueIdentSet e = new ValueIdentSet();
        e.SerialNumber = funkId.ToString();
        DeviceTypes typeOfMinolDevice = NumberRanges.GetTypeOfMinolDevice((long) funkId);
        e.DeviceType = typeOfMinolDevice.ToString();
        e.Manufacturer = NumberRanges.GetManufacturer(typeOfMinolDevice);
        e.ZDF = "SID;" + funkId.ToString() + ";MAN;" + e.Manufacturer + ";MED;" + e.DeviceType;
        this.MyDeviceManager.OnValueIdentSetReceived((object) this, e);
      }
      return true;
    }

    private bool ReadValuesOfMinomatV4(string serialnumber, List<long> filter)
    {
      uint num;
      if (string.IsNullOrEmpty(serialnumber) || !Util.TryParseToUInt32(serialnumber, out num))
        return false;
      this.LoadMinomatHandler();
      this.MyDeviceManager.DeviceList_AddDevice(new GlobalDeviceId()
      {
        Serialnumber = num.ToString()
      });
      return this.ReadValuesOfMinomatV4(filter);
    }

    private bool ReadValuesOfMinomatV4(List<long> filter)
    {
      List<GlobalDeviceId> list = this.MyDeviceManager.DeviceList_GetList();
      if (list == null || list.Count == 0)
        return true;
      this.MyDeviceManager.BreakRequest = false;
      this.minomatV4.CancelCurrentMethod = false;
      List<uint> ids = new List<uint>();
      foreach (GlobalDeviceId globalDeviceId in list)
        ids.Add(Convert.ToUInt32(globalDeviceId.Serialnumber));
      GMM_EventArgs eventMessage = new GMM_EventArgs("");
      MeasurementSet set = new MeasurementSet();
      this.LoadMinomatHandler();
      try
      {
        DateTime start = new DateTime(DateTime.Now.Year - 5, 12, 31);
        DateTime now = DateTime.Now;
        SortedList<DeviceCollectorSettings, object> collectorSettings = this.MyDeviceManager.MyBus.GetDeviceCollectorSettings();
        int count;
        if (filter == null || ValueIdent.Contains(filter, ValueIdent.ValueIdPart_StorageInterval.DueDate))
        {
          GMM_EventArgs gmmEventArgs = eventMessage;
          count = list.Count;
          string str = "Read due date values of " + count.ToString() + " devices...";
          gmmEventArgs.EventMessage = str;
          this.MyDeviceManager.RaiseEvent(eventMessage);
          if (eventMessage.Cancel)
            return false;
          TimeSpan result;
          if (collectorSettings.ContainsKey(DeviceCollectorSettings.MinomatV4_DurationDueDate) && collectorSettings[DeviceCollectorSettings.MinomatV4_DurationDueDate] != null && TimeSpan.TryParse(collectorSettings[DeviceCollectorSettings.MinomatV4_DurationDueDate].ToString(), out result))
            start = now.Add(result);
          MeasurementSet measurementData = this.minomatV4.GetMeasurementData(ids, MeasurementDataType.DueDate, start, now, false);
          if (measurementData != null && measurementData.Count > 0)
            set.Add(measurementData);
        }
        if (filter == null || ValueIdent.Contains(filter, ValueIdent.ValueIdPart_StorageInterval.Month))
        {
          GMM_EventArgs gmmEventArgs = eventMessage;
          count = list.Count;
          string str = "Read month values of " + count.ToString() + " devices...";
          gmmEventArgs.EventMessage = str;
          this.MyDeviceManager.RaiseEvent(eventMessage);
          if (eventMessage.Cancel)
            return false;
          TimeSpan result;
          if (collectorSettings.ContainsKey(DeviceCollectorSettings.MinomatV4_DurationMonth) && collectorSettings[DeviceCollectorSettings.MinomatV4_DurationMonth] != null && TimeSpan.TryParse(collectorSettings[DeviceCollectorSettings.MinomatV4_DurationMonth].ToString(), out result))
            start = now.Add(result);
          MeasurementSet measurementData = this.minomatV4.GetMeasurementData(ids, MeasurementDataType.MonthAndHalfMonth, start, now, false);
          if (measurementData != null && measurementData.Count > 0)
            set.Add(measurementData);
        }
        if (filter == null || ValueIdent.Contains(filter, ValueIdent.ValueIdPart_StorageInterval.Day))
        {
          GMM_EventArgs gmmEventArgs = eventMessage;
          count = list.Count;
          string str = "Read day values of " + count.ToString() + " devices...";
          gmmEventArgs.EventMessage = str;
          this.MyDeviceManager.RaiseEvent(eventMessage);
          if (eventMessage.Cancel)
            return false;
          TimeSpan result;
          if (collectorSettings.ContainsKey(DeviceCollectorSettings.MinomatV4_DurationDay) && collectorSettings[DeviceCollectorSettings.MinomatV4_DurationDay] != null && TimeSpan.TryParse(collectorSettings[DeviceCollectorSettings.MinomatV4_DurationDay].ToString(), out result))
            start = now.Add(result);
          MeasurementSet measurementData = this.minomatV4.GetMeasurementData(ids, MeasurementDataType.Day, start, now, false);
          if (measurementData != null && measurementData.Count > 0)
            set.Add(measurementData);
        }
        if (filter == null || ValueIdent.Contains(filter, ValueIdent.ValueIdPart_StorageInterval.QuarterHour))
        {
          GMM_EventArgs gmmEventArgs = eventMessage;
          count = list.Count;
          string str = "Read quarter values of " + count.ToString() + " devices...";
          gmmEventArgs.EventMessage = str;
          this.MyDeviceManager.RaiseEvent(eventMessage);
          if (eventMessage.Cancel)
            return false;
          TimeSpan result;
          if (collectorSettings.ContainsKey(DeviceCollectorSettings.MinomatV4_DurationQuarterHour) && collectorSettings[DeviceCollectorSettings.MinomatV4_DurationQuarterHour] != null && TimeSpan.TryParse(collectorSettings[DeviceCollectorSettings.MinomatV4_DurationQuarterHour].ToString(), out result))
            start = now.Add(result);
          MeasurementSet measurementData = this.minomatV4.GetMeasurementData(ids, MeasurementDataType.Quarter, start, now, false);
          if (measurementData != null && measurementData.Count > 0)
            set.Add(measurementData);
        }
        this.readedValues.Add(set);
      }
      catch (Exception ex)
      {
        Devices.MinomatHandler.logger.Error(ex.Message);
      }
      eventMessage.EventMessage = "Done!";
      this.MyDeviceManager.RaiseEvent(eventMessage);
      return true;
    }

    private bool ExecuteMethodMinomatV4(
      OverrideID overrideID,
      bool isSetMethod,
      out object result,
      object param1,
      object param2,
      object param3,
      object param4)
    {
      result = (object) null;
      this.LoadMinomatHandler();
      switch (overrideID)
      {
        case OverrideID.MinomatResetConfigurationState:
          if (isSetMethod)
            throw new ArgumentException("Such function is not available! Value: Set" + overrideID.ToString());
          result = (object) this.minomatV4.GetResetConfigurationState();
          break;
        case OverrideID.MinomatMinolId:
          if (isSetMethod)
            return this.minomatV4.SetMinolId(param1);
          result = (object) this.minomatV4.GetMinolId();
          break;
        case OverrideID.MinomatNodeId:
          if (isSetMethod)
            return this.minomatV4.SetNodeId(param1);
          result = (object) this.minomatV4.GetNodeId();
          break;
        case OverrideID.MinomatNetworkId:
          if (isSetMethod)
            return this.minomatV4.SetNetworkId(param1);
          result = (object) this.minomatV4.GetNetworkId();
          break;
        case OverrideID.MinomatSystemTime:
          if (isSetMethod)
            return this.minomatV4.SetSystemTime(param1);
          result = (object) this.minomatV4.GetSystemTime();
          break;
        case OverrideID.MinomatRadioChannel:
          if (isSetMethod)
            return this.minomatV4.SetRadioChannel(param1);
          result = (object) this.minomatV4.GetRadioChannel();
          break;
        case OverrideID.MinomatTransceiverChannelId:
          if (isSetMethod)
            return this.minomatV4.SetTransceiverChannelId(param1);
          result = (object) this.minomatV4.GetTransceiverChannelId();
          break;
        case OverrideID.MinomatRoutingTable:
          if (isSetMethod)
            throw new ArgumentException("Such function is not available! Value: Set" + overrideID.ToString());
          result = (object) this.minomatV4.GetRoutingTable();
          break;
        case OverrideID.MinomatFirmwareVersion:
          if (isSetMethod)
            throw new ArgumentException("Such function is not available! Value: Set" + overrideID.ToString());
          result = (object) this.minomatV4.GetFirmwareVersion();
          break;
        case OverrideID.MinomatUserappName:
          if (isSetMethod)
            throw new ArgumentException("Such function is not available! Value: Set" + overrideID.ToString());
          result = (object) this.minomatV4.GetUserappName();
          break;
        case OverrideID.MinomatFirmwareBuildTime:
          if (isSetMethod)
            throw new ArgumentException("Such function is not available! Value: Set" + overrideID.ToString());
          result = (object) this.minomatV4.GetFirmwareBuildTime();
          break;
        case OverrideID.MinomatUserappBuildTime:
          if (isSetMethod)
            throw new ArgumentException("Such function is not available! Value: Set" + overrideID.ToString());
          result = (object) this.minomatV4.GetUserappBuildTime();
          break;
        case OverrideID.MinomatErrorFlags:
          if (isSetMethod)
            throw new ArgumentException("Such function is not available! Value: Set" + overrideID.ToString());
          result = (object) this.minomatV4.GetErrorFlags();
          break;
        case OverrideID.MinomatTransmissionPower:
          if (isSetMethod)
            return this.minomatV4.SetTransmissionPower(param1);
          result = (object) this.minomatV4.GetTransmissionPower();
          break;
        case OverrideID.MinomatMultiChannelSettings:
          if (isSetMethod)
            return this.minomatV4.SetMultiChannelSettings(param1, param2, param3, param4);
          result = (object) this.minomatV4.GetMultiChannelSettings();
          break;
        case OverrideID.MinomatTransceiverFrequencyOffset:
          if (isSetMethod)
            return this.minomatV4.SetTransceiverFrequencyOffset(param1);
          result = (object) this.minomatV4.GetTransceiverFrequencyOffset();
          break;
        case OverrideID.MinomatTemperatureOffset:
          if (isSetMethod)
            return this.minomatV4.SetTemperatureOffset(param1);
          result = (object) this.minomatV4.GetTemperatureOffset();
          break;
        case OverrideID.MinomatPhaseDetailsBuffer:
          if (isSetMethod)
            return this.minomatV4.SetPhaseDetailsBuffer(param1);
          result = (object) this.minomatV4.GetPhaseDetailsBuffer();
          break;
        case OverrideID.MinomatPhaseDetails:
          if (isSetMethod)
            throw new ArgumentException("Such function is not available! Value: Set" + overrideID.ToString());
          result = (object) this.minomatV4.GetPhaseDetails();
          break;
        case OverrideID.MinomatRestartMinomat:
          return this.minomatV4.RestartMinomat();
        case OverrideID.MinomatMessUnitNumberMax:
          if (isSetMethod)
            throw new ArgumentException("Such function is not available! Value: Set" + overrideID.ToString());
          result = (object) this.minomatV4.GetMessUnitNumberMax();
          break;
        case OverrideID.MinomatMaxMessUnitNumberNotConfigured:
          if (isSetMethod)
            return this.minomatV4.SetMessUnitNumberNotConfiguredMax(param1);
          result = (object) this.minomatV4.GetMessUnitNumberNotConfiguredMax();
          break;
        case OverrideID.MinomatScenario:
          if (isSetMethod)
            return this.minomatV4.SetScenario(param1);
          result = (object) this.minomatV4.GetScenario();
          break;
        case OverrideID.MinomatStartTestReception:
          return this.minomatV4.StartTestReception(param1, param2);
        case OverrideID.MinomatTestReceptionResult:
          if (isSetMethod)
            throw new ArgumentException("Such function is not available! Value: Set" + overrideID.ToString());
          result = (object) this.minomatV4.GetTestReceptionResult();
          break;
        case OverrideID.MinomatRegisterMessUnit:
          result = (object) this.minomatV4.RegisterMessUnit(param1, param2, param3, param4);
          break;
        case OverrideID.MinomatResetConfiguration:
          return this.minomatV4.ResetConfiguration();
        case OverrideID.MinomatStartNetworkSetup:
          return this.minomatV4.StartNetworkSetup(param1);
        case OverrideID.MinomatDeleteMessUnit:
          result = (object) this.minomatV4.DeleteMessUnit(param1);
          break;
        case OverrideID.MinomatInfoOfRegisteredMessUnit:
          if (isSetMethod)
            throw new ArgumentException("Such function is not available! Value: Set" + overrideID.ToString());
          result = (object) this.minomatV4.GetInfoOfRegisteredMessUnit(param1);
          break;
        case OverrideID.MinomatRegisteredMessUnits:
          if (isSetMethod)
            throw new ArgumentException("Such function is not available! Value: Set" + overrideID.ToString());
          result = (object) this.minomatV4.GetRegisteredMessUnits();
          break;
        case OverrideID.MinomatSimPin:
          if (isSetMethod)
            return this.minomatV4.SetSimPin(param1);
          result = (object) this.minomatV4.GetSimPin();
          break;
        case OverrideID.MinomatAPN:
          if (isSetMethod)
            return this.minomatV4.SetAPN(param1);
          result = (object) this.minomatV4.GetAPN();
          break;
        case OverrideID.MinomatGPRSUserName:
          if (isSetMethod)
            return this.minomatV4.SetGPRSUserName(param1);
          result = (object) this.minomatV4.GetGPRSUserName();
          break;
        case OverrideID.MinomatGPRSPassword:
          if (isSetMethod)
            return this.minomatV4.SetGPRSPassword(param1);
          result = (object) this.minomatV4.GetGPRSPassword();
          break;
        case OverrideID.MinomatHttpServer:
          if (isSetMethod)
            return this.minomatV4.SetHttpServer(param1, param2);
          result = (object) this.minomatV4.GetHttpServer();
          break;
        case OverrideID.MinomatHttpResourceName:
          if (isSetMethod)
            return this.minomatV4.SetHttpResourceName(param1);
          result = (object) this.minomatV4.GetHttpResourceName();
          break;
        case OverrideID.MinomatStartGSMTestReception:
          return this.minomatV4.StartGSMTestReception();
        case OverrideID.MinomatGSMTestReceptionState:
          if (isSetMethod)
            throw new ArgumentException("Such function is not available! Value: Set" + overrideID.ToString());
          result = (object) this.minomatV4.GetGSMTestReceptionState();
          break;
        case OverrideID.MinomatForceNetworkOptimization:
          result = (object) this.minomatV4.ForceNetworkOptimisation();
          break;
        case OverrideID.MinomatStartNetworkOptimization:
          return this.minomatV4.StartNetworkOptimization();
        case OverrideID.MinomatRegisterSlave:
          result = (object) this.minomatV4.RegisterSlave(param1, param2);
          break;
        case OverrideID.MinomatDeregisterSlave:
          result = (object) this.minomatV4.DeregisterSlave(param1);
          break;
        case OverrideID.MinomatRegisteredSlaves:
          result = (object) this.minomatV4.GetRegisteredSlaves(param1);
          break;
        case OverrideID.MinomatFlash:
          result = (object) this.minomatV4.GetFlash(param1, param2, param3, param4);
          break;
        case OverrideID.MinomatEeprom:
          result = (object) this.minomatV4.GetEeprom(param1, param2, param3);
          break;
        case OverrideID.MinomatAppInitialSettings:
          if (isSetMethod)
            return this.minomatV4.SetAppInitialSettings(param1, param2, param3, param4);
          result = (object) this.minomatV4.GetAppInitialSettings();
          break;
        case OverrideID.MinomatActionTimepoint:
          result = (object) this.minomatV4.SetActionTimepoint(param1, param2, param3);
          break;
        case OverrideID.MinomatMeasurementData:
          result = (object) this.minomatV4.GetMeasurementData(param1.ToString(), param2.ToString(), param3.ToString(), param4.ToString(), false);
          break;
        case OverrideID.MinomatHttpState:
          if (isSetMethod)
            throw new ArgumentException("Such function is not available! Value: Set" + overrideID.ToString());
          result = (object) this.minomatV4.GetHttpState();
          break;
        case OverrideID.MinomatGsmState:
          if (isSetMethod)
            throw new ArgumentException("Such function is not available! Value: Set" + overrideID.ToString());
          result = (object) this.minomatV4.GetGsmState();
          break;
        case OverrideID.MinomatModemBuildDate:
          if (isSetMethod)
            throw new ArgumentException("Such function is not available! Value: Set" + overrideID.ToString());
          result = (object) this.minomatV4.GetModemBuildDate();
          break;
        case OverrideID.MinomatModemDueDate:
          if (isSetMethod)
            return this.minomatV4.SetModemDueDate(param1);
          result = (object) this.minomatV4.GetModemDueDate();
          break;
        case OverrideID.MinomatStartHttpConnection:
          return this.minomatV4.StartHttpConnection();
        case OverrideID.MinomatConfigurationString:
          if (isSetMethod)
            throw new ArgumentException("Such function is not available! Value: Set" + overrideID.ToString());
          result = (object) this.minomatV4.GetConfigurationString();
          break;
        case OverrideID.MinomatSwitchToNetworkModel:
          return this.minomatV4.SwitchToNetworkModel();
        case OverrideID.MinomatComServerName:
          if (isSetMethod)
            throw new ArgumentException("Such function is not available! Value: Set" + overrideID.ToString());
          result = (object) this.minomatV4.GetComServerFile(ComServerFileType.Name);
          break;
        case OverrideID.MinomatMessUnitMetadata:
          if (isSetMethod)
            throw new ArgumentException("Such function is not available! Value: Set" + overrideID.ToString());
          result = (object) this.minomatV4.GetMessUnitMetadata(param1);
          break;
        case OverrideID.MinomatLED:
          return this.minomatV4.SetLED(param1, param2, param3, param4);
        default:
          throw new ArgumentException("OverrideID is not supported! Value: " + overrideID.ToString());
      }
      return true;
    }

    private void FireEventOnValueIdentSetReceived(uint id, int total)
    {
      if (!this.MyDeviceManager.IsValueIdentSetReceivedEventEnabled)
        return;
      ValueIdentSet e = new ValueIdentSet();
      e.SerialNumber = id.ToString();
      DeviceTypes typeOfMinolDevice = NumberRanges.GetTypeOfMinolDevice((long) id);
      e.DeviceType = typeOfMinolDevice.ToString();
      e.Manufacturer = NumberRanges.GetManufacturer(typeOfMinolDevice);
      e.ZDF = "SID;" + id.ToString() + ";MAN;" + e.Manufacturer + ";MED;" + e.DeviceType;
      e.Total = total;
      SortedList<long, SortedList<DateTime, ReadingValue>> ValueList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
      if (this.GetValues(ref ValueList, e.SerialNumber))
        e.AvailableValues = ValueList;
      this.MyDeviceManager.OnValueIdentSetReceived((object) this, e);
    }

    private void FireEventOnValueIdentSetReceived(GlobalDeviceId id, int total)
    {
      if (!this.MyDeviceManager.IsValueIdentSetReceivedEventEnabled)
        return;
      ValueIdentSet e = new ValueIdentSet()
      {
        Manufacturer = id.Manufacturer,
        SerialNumber = id.Serialnumber,
        DeviceType = id.DeviceTypeName
      };
      e.ZDF = "SID;" + id.Serialnumber + ";MAN;" + id.Manufacturer + ";MED;" + e.DeviceType;
      e.Total = total;
      SortedList<long, SortedList<DateTime, ReadingValue>> ValueList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
      if (this.GetValues(ref ValueList, e.SerialNumber))
        e.AvailableValues = ValueList;
      this.MyDeviceManager.OnValueIdentSetReceived((object) this, e);
    }
  }
}
