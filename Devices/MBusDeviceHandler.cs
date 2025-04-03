// Decompiled with JetBrains decompiler
// Type: Devices.MBusDeviceHandler
// Assembly: Devices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 793FC2DA-FF88-4FD5-BDE9-C00C0310F1EC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Devices.dll

using DeviceCollector;
using System;
using System.Collections.Generic;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace Devices
{
  public class MBusDeviceHandler(DeviceManager MyDeviceManager) : BaseDevice(MyDeviceManager)
  {
    private SortedList<string, DateTime> valuesCanNotReadFromDevice;
    protected SortedList<OverrideID, ConfigurationParameter> ReadConfigParamList;
    protected SortedList<OverrideID, ConfigurationParameter> SetConfigParamList;

    public override event EventHandlerEx<int> OnProgress;

    public override event EventHandlerEx<string> OnProgressMessage;

    public override void Dispose()
    {
      this.valuesCanNotReadFromDevice = (SortedList<string, DateTime>) null;
      this.ReadConfigParamList = (SortedList<OverrideID, ConfigurationParameter>) null;
      this.SetConfigParamList = (SortedList<OverrideID, ConfigurationParameter>) null;
    }

    internal override void ShowHandlerWindow() => this.MyDeviceManager.MyBus.ShowBusWindow();

    public override object GetHandler() => (object) this.MyDeviceManager.MyBus;

    public override bool ReadAll(List<long> filter)
    {
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      if (this.valuesCanNotReadFromDevice == null)
        this.valuesCanNotReadFromDevice = new SortedList<string, DateTime>();
      this.valuesCanNotReadFromDevice.Clear();
      ZR_ClassLibrary.BusMode? currentBusMode = this.MyDeviceManager.GetCurrentBusMode();
      ZR_ClassLibrary.BusMode? nullable1 = currentBusMode;
      ZR_ClassLibrary.BusMode busMode1 = ZR_ClassLibrary.BusMode.MBus;
      if (nullable1.GetValueOrDefault() == busMode1 & nullable1.HasValue)
      {
        List<GlobalDeviceId> list = this.MyDeviceManager.DeviceList_GetList();
        if (list == null)
          return false;
        if (list.Count == 1 && string.IsNullOrEmpty(list[0].Serialnumber))
          return this.BeginSearchDevices();
        int count = list.Count;
        GMM_EventArgs eventMessage = new GMM_EventArgs(count.ToString() + " devices available");
        this.MyDeviceManager.RaiseEvent(eventMessage);
        if (eventMessage.Cancel)
          return false;
        int num = 1;
        foreach (GlobalDeviceId device in list)
        {
          if (!this.SelectDevice(device))
            return false;
          string serialnumber = device.Serialnumber;
          GMM_EventArgs gmmEventArgs = eventMessage;
          string[] strArray = new string[7]
          {
            "Read ",
            num.ToString(),
            " of ",
            null,
            null,
            null,
            null
          };
          count = list.Count;
          strArray[3] = count.ToString();
          strArray[4] = " (SN: ";
          strArray[5] = serialnumber;
          strArray[6] = ")";
          string str = string.Concat(strArray);
          gmmEventArgs.EventMessage = str;
          this.MyDeviceManager.RaiseEvent(eventMessage);
          if (eventMessage.Cancel)
            return false;
          DeviceInfo Info;
          if (!this.MyDeviceManager.MyBus.ReadParameter(out Info))
          {
            if (!this.valuesCanNotReadFromDevice.ContainsKey(serialnumber))
              this.valuesCanNotReadFromDevice.Add(serialnumber, DateTime.Now);
            else
              this.valuesCanNotReadFromDevice[serialnumber] = DateTime.Now;
            Info = new DeviceInfo();
            Info.MeterNumber = device.Serialnumber;
          }
          if (Info != null)
            this.FireEventOnValueIdentSetReceived(Info, (DeviceInfo) null);
          ++num;
        }
        return true;
      }
      ZR_ClassLibrary.BusMode? nullable2 = currentBusMode;
      ZR_ClassLibrary.BusMode busMode2 = ZR_ClassLibrary.BusMode.MBusPointToPoint;
      if (!(nullable2.GetValueOrDefault() == busMode2 & nullable2.HasValue) || !this.MyDeviceManager.MyBus.ScanFromAddress(0))
        return false;
      DeviceInfo Info1;
      bool flag = this.MyDeviceManager.MyBus.ReadParameter(out Info1);
      if (flag)
        this.FireEventOnValueIdentSetReceived(Info1, (DeviceInfo) null);
      return flag;
    }

    public override bool Read(StructureTreeNode structureTreeNode, List<long> filter)
    {
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      SortedList<DeviceCollectorSettings, object> collectorSettings = this.MyDeviceManager.MyBus.GetDeviceCollectorSettings();
      bool flag = true;
      if (collectorSettings.ContainsKey(DeviceCollectorSettings.OnlySecondaryAddressing))
        flag = Convert.ToBoolean(collectorSettings[DeviceCollectorSettings.OnlySecondaryAddressing]);
      ZR_ClassLibrary.BusMode? currentBusMode = this.MyDeviceManager.GetCurrentBusMode();
      ZR_ClassLibrary.BusMode? nullable = currentBusMode;
      ZR_ClassLibrary.BusMode busMode1 = ZR_ClassLibrary.BusMode.MBus;
      int num;
      if (!(nullable.GetValueOrDefault() == busMode1 & nullable.HasValue))
      {
        nullable = currentBusMode;
        ZR_ClassLibrary.BusMode busMode2 = ZR_ClassLibrary.BusMode.MBusPointToPoint;
        num = nullable.GetValueOrDefault() == busMode2 & nullable.HasValue ? 1 : 0;
      }
      else
        num = 1;
      if (num == 0)
        throw new ArgumentNullException("Wrong 'BusMode'! Please check the DeveiceCollector settings.");
      string nodeSettingsValue = structureTreeNode.GetNodeSettingsValue("RADR");
      if (string.IsNullOrEmpty(nodeSettingsValue))
        throw new ArgumentNullException("Parameter 'RADR' can not be null!");
      if (string.IsNullOrEmpty(structureTreeNode.SerialNumber))
        throw new ArgumentNullException("Parameter 'serailnumber' can not be null!");
      if (this.valuesCanNotReadFromDevice == null)
        this.valuesCanNotReadFromDevice = new SortedList<string, DateTime>();
      this.ClearDeviceList();
      int int32 = Convert.ToInt32(nodeSettingsValue);
      if (!flag ? this.MyDeviceManager.MyBus.SearchSingleDeviceByPrimaryAddress(int32) : (structureTreeNode.SubDeviceIndex != 0 ? this.MyDeviceManager.MyBus.SearchSingleDeviceBySerialNumber(structureTreeNode.Parent.SerialNumber) : this.MyDeviceManager.MyBus.SearchSingleDeviceBySerialNumber(structureTreeNode.SerialNumber)))
        return true;
      if (!this.valuesCanNotReadFromDevice.ContainsKey(structureTreeNode.SerialNumber))
        this.valuesCanNotReadFromDevice.Add(structureTreeNode.SerialNumber, ParameterService.GetNow());
      else
        this.valuesCanNotReadFromDevice[structureTreeNode.SerialNumber] = ParameterService.GetNow();
      return false;
    }

    public override bool SelectDevice(GlobalDeviceId device)
    {
      if (this.MyDeviceManager == null)
        throw new ArgumentNullException("Class member 'MyDeviceManager' can not be null!");
      return !string.IsNullOrEmpty(device.Serialnumber) ? this.MyDeviceManager.MyBus.SetSelectedDeviceBySerialNumber(device.Serialnumber) : this.MyDeviceManager.MyBus.SetSelectedDeviceByPrimaryAddress((int) byte.Parse(device.MeterNumber));
    }

    public override string GetZdfValues()
    {
      if (this.MyDeviceManager == null)
        throw new ArgumentNullException("Class member 'MyDeviceManager' can not be null!");
      DeviceInfo Info;
      return !this.MyDeviceManager.MyBus.GetParameter(out Info) ? string.Empty : Info.GetZDFParameterString();
    }

    public override UniqueIdentification GetUniqueIdentification()
    {
      return this.MyDeviceManager != null ? this.MyDeviceManager.MyBus.GetUniqueIdentificationOfSelectedDevice() : throw new ArgumentNullException("Class member 'MyDeviceManager' can not be null!");
    }

    public override bool BeginSearchDevices()
    {
      if (this.MyDeviceManager == null)
        throw new ArgumentNullException("Class member 'MyDeviceManager' can not be null!");
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      this.MyDeviceManager.MyBus.OnDeviceListChanged += new EventHandler<BusDevice>(this.MyBus_OnDeviceListChanged);
      this.MyDeviceManager.MyBus.OnProgress += new EventHandlerEx<int>(this.MyBus_OnProgress);
      this.MyDeviceManager.MyBus.OnProgressMessage += new EventHandlerEx<string>(this.MyBus_OnProgressMessage);
      try
      {
        this.MyDeviceManager.MyBus.DeleteBusInfo();
        SortedList<DeviceCollectorSettings, object> collectorSettings = this.MyDeviceManager.MyBus.GetDeviceCollectorSettings();
        if (!collectorSettings.ContainsKey(DeviceCollectorSettings.OnlySecondaryAddressing) || !Convert.ToBoolean(collectorSettings[DeviceCollectorSettings.OnlySecondaryAddressing]))
          return this.MyDeviceManager.MyBus.ScanFromAddress(0);
        string StartSerialnumber = "fffffff0";
        if (collectorSettings.ContainsKey(DeviceCollectorSettings.ScanStartSerialnumber))
          StartSerialnumber = collectorSettings[DeviceCollectorSettings.ScanStartSerialnumber].ToString();
        return this.MyDeviceManager.MyBus.ScanFromSerialNumber(StartSerialnumber);
      }
      finally
      {
        this.MyDeviceManager.MyBus.OnDeviceListChanged -= new EventHandler<BusDevice>(this.MyBus_OnDeviceListChanged);
        this.MyDeviceManager.MyBus.OnProgress -= new EventHandlerEx<int>(this.MyBus_OnProgress);
        this.MyDeviceManager.MyBus.OnProgressMessage -= new EventHandlerEx<string>(this.MyBus_OnProgressMessage);
      }
    }

    private void MyBus_OnProgress(object sender, int e)
    {
      if (this.OnProgress == null)
        return;
      this.OnProgress(sender, e);
    }

    private void MyBus_OnProgressMessage(object sender, string e)
    {
      if (this.OnProgressMessage == null)
        return;
      this.OnProgressMessage(sender, e);
    }

    private void MyBus_OnDeviceListChanged(object sender, BusDevice e)
    {
      if (e == null)
        return;
      this.FireEventOnValueIdentSetReceived(e.Info, (DeviceInfo) null);
    }

    private DeviceTypes ConvertToDeviceType(string type)
    {
      return string.IsNullOrEmpty(type) || !Enum.IsDefined(typeof (DeviceTypes), (object) type) ? DeviceTypes.None : (DeviceTypes) Enum.Parse(typeof (DeviceTypes), type, true);
    }

    public override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> valueList,
      string serialnumber)
    {
      if (valueList == null)
        valueList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
      if (string.IsNullOrEmpty(serialnumber))
        return false;
      ZR_ClassLibrary.BusMode? currentBusMode = this.MyDeviceManager.GetCurrentBusMode();
      ZR_ClassLibrary.BusMode? nullable1 = currentBusMode;
      ZR_ClassLibrary.BusMode busMode1 = ZR_ClassLibrary.BusMode.MBus;
      int num;
      if (!(nullable1.GetValueOrDefault() == busMode1 & nullable1.HasValue))
      {
        ZR_ClassLibrary.BusMode? nullable2 = currentBusMode;
        ZR_ClassLibrary.BusMode busMode2 = ZR_ClassLibrary.BusMode.MBusPointToPoint;
        num = nullable2.GetValueOrDefault() == busMode2 & nullable2.HasValue ? 1 : 0;
      }
      else
        num = 1;
      if (num == 0)
        throw new ArgumentNullException("Wrong 'BusMode'! Please check the DeveiceCollector settings.");
      bool flag = this.MyDeviceManager.MyBus.SetSelectedDeviceBySerialNumber(serialnumber);
      DeviceInfo Info;
      bool parameter = this.MyDeviceManager.MyBus.GetParameter(out Info);
      if (!flag || !parameter)
      {
        if (this.valuesCanNotReadFromDevice == null)
          this.valuesCanNotReadFromDevice = new SortedList<string, DateTime>();
        if (!this.valuesCanNotReadFromDevice.ContainsKey(serialnumber))
          return false;
        long valueIdentOfWarninig = ValueIdent.GetValueIdentOfWarninig(ValueIdent.ValueIdPart_MeterType.Any, ValueIdent.ValueIdentWarning.FailedToRead, ValueIdent.ValueIdPart_Creation.ReadingSystem);
        valueList.Add(valueIdentOfWarninig, new SortedList<DateTime, ReadingValue>()
        {
          {
            this.valuesCanNotReadFromDevice[serialnumber],
            new ReadingValue() { value = 1.0 }
          }
        });
        return true;
      }
      if (!TranslationRulesManager.Instance.TryParse(Info.GetZDFParameterString(), 0, ref valueList))
        return false;
      ValueIdent.CleanUpEmptyValueIdents(valueList);
      return true;
    }

    public override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> valueList)
    {
      BusDevice selectedDevice = this.MyDeviceManager.MyBus.GetSelectedDevice();
      return selectedDevice != null && selectedDevice.Info != null && selectedDevice.Info.MeterNumber != null && this.GetValues(ref valueList, selectedDevice.Info.MeterNumber);
    }

    public override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> valueList,
      int subDeviceIndex)
    {
      string zdfValues = this.GetZdfValues();
      return !string.IsNullOrEmpty(zdfValues) && TranslationRulesManager.Instance.TryParse(zdfValues, subDeviceIndex, ref valueList);
    }

    public override bool GetValues(
      string zdf,
      ref SortedList<long, SortedList<DateTime, ReadingValue>> valueList,
      int subDeviceIndex)
    {
      return !string.IsNullOrEmpty(zdf) && TranslationRulesManager.Instance.TryParse(zdf, subDeviceIndex, ref valueList);
    }

    public override bool ReadConfigurationParameters(out GlobalDeviceId UpdatedDeviceIdentification)
    {
      UpdatedDeviceIdentification = (GlobalDeviceId) null;
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      this.ReadConfigParamList = new SortedList<OverrideID, ConfigurationParameter>();
      DeviceInfo Info;
      if (!this.MyDeviceManager.MyBus.ReadParameter(out Info) || Info == null)
        return false;
      ConfigurationParameter configurationParameter1 = new ConfigurationParameter(OverrideID.DeviceName, (object) (Info.Manufacturer + " " + Info.DeviceType.ToString()));
      this.ReadConfigParamList.Add(configurationParameter1.ParameterID, configurationParameter1);
      ConfigurationParameter configurationParameter2 = new ConfigurationParameter(OverrideID.SerialNumber, (object) Info.MeterNumber);
      this.ReadConfigParamList.Add(configurationParameter2.ParameterID, configurationParameter2);
      ConfigurationParameter configurationParameter3 = new ConfigurationParameter(OverrideID.Medium, (object) Info.MediumString);
      this.ReadConfigParamList.Add(configurationParameter3.ParameterID, configurationParameter3);
      ConfigurationParameter configurationParameter4 = new ConfigurationParameter(OverrideID.FirmwareVersion, (object) Info.Version);
      this.ReadConfigParamList.Add(configurationParameter4.ParameterID, configurationParameter4);
      ConfigurationParameter configurationParameter5 = new ConfigurationParameter(OverrideID.DiagnosticString, (object) Info.GetZDFParameterString());
      this.ReadConfigParamList.Add(configurationParameter5.ParameterID, configurationParameter5);
      ConfigurationParameter configurationParameter6 = new ConfigurationParameter(OverrideID.MBusAddress, (object) (ulong) Info.A_Field);
      configurationParameter6.HasWritePermission = true;
      configurationParameter6.MaxParameterValue = (object) 249UL;
      configurationParameter6.MinParameterValue = (object) 0UL;
      this.ReadConfigParamList.Add(configurationParameter6.ParameterID, configurationParameter6);
      ConfigurationParameter configurationParameter7 = new ConfigurationParameter(OverrideID.Baudrate, (object) this.MyDeviceManager.GetAsyncComSettings()["Baudrate"]);
      configurationParameter7.HasWritePermission = true;
      configurationParameter7.IsEditable = true;
      configurationParameter7.AllowedValues = Constants.GetAvailableBaudrates().ConvertAll<string>((Converter<ValueItem, string>) (x => x.Value)).ToArray();
      this.ReadConfigParamList.Add(configurationParameter7.ParameterID, configurationParameter7);
      List<GlobalDeviceId> globalDeviceIdList = this.GetGlobalDeviceIdList();
      if (globalDeviceIdList == null)
        return false;
      UpdatedDeviceIdentification = globalDeviceIdList[0];
      return true;
    }

    public override SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      ConfigurationParameter.ValueType ConfigurationType,
      int SubDevice)
    {
      this.MyDeviceManager.ParameterType = ConfigurationType;
      if (this.ReadConfigParamList == null)
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      SortedList<OverrideID, ConfigurationParameter> configurationParameters = new SortedList<OverrideID, ConfigurationParameter>();
      foreach (KeyValuePair<OverrideID, ConfigurationParameter> readConfigParam in this.ReadConfigParamList)
        configurationParameters.Add(readConfigParam.Key, new ConfigurationParameter(readConfigParam.Value));
      return configurationParameters;
    }

    public override bool SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> ConfigParameterList,
      int SubDevice)
    {
      this.SetConfigParamList = ConfigParameterList;
      return true;
    }

    public override bool WriteChangedConfigurationParametersToDevice()
    {
      if (this.SetConfigParamList == null)
        return false;
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      if (this.SetConfigParamList.ContainsKey(OverrideID.MBusAddress))
      {
        ConfigurationParameter readConfigParam = this.ReadConfigParamList[OverrideID.MBusAddress];
        ConfigurationParameter setConfigParam = this.SetConfigParamList[OverrideID.MBusAddress];
        if (readConfigParam.ParameterValue != setConfigParam.ParameterValue && !this.MyDeviceManager.MyBus.SetPrimaryAddress(Convert.ToInt32(setConfigParam.ParameterValue)))
          return false;
      }
      if (this.SetConfigParamList.ContainsKey(OverrideID.Baudrate))
      {
        ConfigurationParameter readConfigParam = this.ReadConfigParamList[OverrideID.Baudrate];
        ConfigurationParameter setConfigParam = this.SetConfigParamList[OverrideID.Baudrate];
        if (readConfigParam.ParameterValue != setConfigParam.ParameterValue && !this.MyDeviceManager.MyBus.SetBaudrate(Convert.ToInt32(setConfigParam.ParameterValue)))
          return false;
      }
      return true;
    }

    public override List<GlobalDeviceId> GetGlobalDeviceIdList()
    {
      List<GlobalDeviceId> globalDeviceIdList = new List<GlobalDeviceId>();
      int numberOfDevices = this.MyDeviceManager.MyBus.GetNumberOfDevices();
      for (int index = 0; index < numberOfDevices && this.MyDeviceManager.MyBus.SetSelectedDeviceByIndex(index); ++index)
      {
        GlobalDeviceId globalDeviceId = this.GetGlobalDeviceId();
        if (globalDeviceId == null)
          return globalDeviceIdList;
        globalDeviceIdList.Add(globalDeviceId);
      }
      return globalDeviceIdList;
    }

    private GlobalDeviceId GetGlobalDeviceId()
    {
      DeviceInfo Info;
      if (!this.MyDeviceManager.MyBus.GetParameter(out Info) || Info == null)
        return (GlobalDeviceId) null;
      return new GlobalDeviceId()
      {
        DeviceTypeName = Info.DeviceType.ToString(),
        Manufacturer = Info.Manufacturer,
        MeterNumber = Convert.ToString(Info.A_Field),
        Generation = Info.Version.ToString(),
        FirmwareVersion = Info.Signature.ToString(),
        Serialnumber = Info.MeterNumber,
        MeterType = ValueIdent.ConvertToMeterType(Info.Medium)
      };
    }

    private void FireEventOnValueIdentSetReceived(DeviceInfo device, DeviceInfo mainDevice)
    {
      if (device == null || !this.MyDeviceManager.IsValueIdentSetReceivedEventEnabled)
        return;
      ValueIdentSet e1 = new ValueIdentSet();
      if (mainDevice != null && device.SubDevices != null)
      {
        e1.MainDeviceSerialNumber = mainDevice.MeterNumber;
        e1.Channel = device.SubDevices.IndexOf(mainDevice);
      }
      e1.Manufacturer = device.Manufacturer;
      e1.Version = device.Version.ToString();
      e1.SerialNumber = device.MeterNumber;
      e1.DeviceType = device.MediumString;
      e1.ZDF = device.GetZDFParameterString();
      e1.PrimaryAddress = device.A_Field.ToString();
      this.GetValues(ref e1.AvailableValues, e1.SerialNumber);
      this.MyDeviceManager.OnValueIdentSetReceived((object) this, e1);
      if (device.SubDevices != null)
      {
        foreach (DeviceInfo subDevice in device.SubDevices)
          this.FireEventOnValueIdentSetReceived(subDevice, device);
      }
      List<GlobalDeviceId> subDevices = TranslationRulesManager.GetSubDevices(e1.ZDF);
      if (subDevices != null)
      {
        int num = 1;
        foreach (GlobalDeviceId globalDeviceId in subDevices)
        {
          ValueIdentSet e2 = new ValueIdentSet();
          e1.Manufacturer = globalDeviceId.Manufacturer;
          e1.Version = globalDeviceId.Generation;
          e2.SerialNumber = globalDeviceId.Serialnumber;
          e2.DeviceType = globalDeviceId.MeterType.ToString();
          e2.ZDF = e1.ZDF;
          e2.MainDeviceSerialNumber = device.MeterNumber;
          e2.PrimaryAddress = device.A_Field.ToString();
          e2.Channel = num++;
          SortedList<long, SortedList<DateTime, ReadingValue>> ValueList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
          if (this.GetValues(e1.ZDF, ref ValueList, subDevices.IndexOf(globalDeviceId) + 1))
            e2.AvailableValues = ValueList;
          this.MyDeviceManager.OnValueIdentSetReceived((object) this, e2);
        }
      }
    }
  }
}
