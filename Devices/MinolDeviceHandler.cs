// Decompiled with JetBrains decompiler
// Type: Devices.MinolDeviceHandler
// Assembly: Devices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 793FC2DA-FF88-4FD5-BDE9-C00C0310F1EC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Devices.dll

using DeviceCollector;
using MinolHandler;
using NLog;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace Devices
{
  public class MinolDeviceHandler(DeviceManager MyDeviceManager) : BaseDevice(MyDeviceManager)
  {
    private static Logger logger = LogManager.GetLogger("MinolDeviceByHandler");
    private MinolHandlerFunctions minolHandler;

    public override event EventHandlerEx<int> OnProgress;

    private void LoadMinolHandler()
    {
      if (this.minolHandler != null)
        return;
      BusMode? currentBusMode = this.MyDeviceManager.GetCurrentBusMode();
      BusMode? nullable = currentBusMode;
      BusMode busMode = BusMode.Minol_Device;
      if (!(nullable.GetValueOrDefault() == busMode & nullable.HasValue))
        throw new ArgumentException("Can not load MinolHandlerFunctions! Reason: Wrong BusMode: " + currentBusMode.ToString());
      this.minolHandler = new MinolHandlerFunctions((IDeviceCollector) this.MyDeviceManager.MyBus);
      this.minolHandler.OnProgress += new EventHandlerEx<int>(this.minolHandler_OnProgress);
    }

    private void minolHandler_OnProgress(object sender, int e)
    {
      if (this.OnProgress == null)
        return;
      this.OnProgress(sender, e);
    }

    public override object GetHandler()
    {
      this.LoadMinolHandler();
      return (object) this.minolHandler;
    }

    internal override void ShowHandlerWindow()
    {
      this.LoadMinolHandler();
      this.minolHandler.ShowMinolHandlerWindow();
    }

    public override void Dispose()
    {
      if (this.minolHandler == null)
        return;
      this.minolHandler.OnProgress -= new EventHandlerEx<int>(this.minolHandler_OnProgress);
      this.minolHandler.GMM_Dispose();
      this.minolHandler = (MinolHandlerFunctions) null;
    }

    public override bool BeginSearchDevices()
    {
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      BusMode? currentBusMode = this.MyDeviceManager.GetCurrentBusMode();
      BusMode? nullable = currentBusMode;
      BusMode busMode = BusMode.Minol_Device;
      if (!(nullable.GetValueOrDefault() == busMode & nullable.HasValue))
      {
        nullable = currentBusMode;
        throw new Exception("DeviceCollector settings are wrong! Invalid bus mode: " + nullable.ToString());
      }
      this.LoadMinolHandler();
      GlobalDeviceId UpdatedDeviceIdentification;
      if (!this.minolHandler.ReadValues(out UpdatedDeviceIdentification))
        return false;
      this.FireEventOnValueIdentSetReceived(UpdatedDeviceIdentification, 0);
      return true;
    }

    public override bool Read(StructureTreeNode structureTreeNode, List<long> filter)
    {
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      BusMode? currentBusMode = this.MyDeviceManager.GetCurrentBusMode();
      BusMode busMode = BusMode.Minol_Device;
      if (!(currentBusMode.GetValueOrDefault() == busMode & currentBusMode.HasValue))
        return false;
      this.LoadMinolHandler();
      if (!this.minolHandler.ReadValues(out GlobalDeviceId _))
        return false;
      GlobalDeviceId globalDeviceId = this.minolHandler.GetGlobalDeviceId();
      return globalDeviceId != null && globalDeviceId.Serialnumber == structureTreeNode.SerialNumber;
    }

    public override bool ReadAll(List<long> filter)
    {
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      BusMode? currentBusMode = this.MyDeviceManager.GetCurrentBusMode();
      BusMode? nullable = currentBusMode;
      BusMode busMode = BusMode.Minol_Device;
      if (!(nullable.GetValueOrDefault() == busMode & nullable.HasValue))
      {
        nullable = currentBusMode;
        throw new Exception("DeviceCollector settings are wrong! Invalid bus mode: " + nullable.ToString());
      }
      this.LoadMinolHandler();
      GlobalDeviceId UpdatedDeviceIdentification;
      bool flag = this.minolHandler.ReadValues(out UpdatedDeviceIdentification);
      if (flag)
        this.FireEventOnValueIdentSetReceived(UpdatedDeviceIdentification, 0);
      return flag;
    }

    public override bool Connect(ref GlobalDeviceId Device)
    {
      this.LoadMinolHandler();
      Device = this.minolHandler.GetGlobalDeviceId();
      return Device != null;
    }

    public override List<GlobalDeviceId> GetGlobalDeviceIdList()
    {
      this.LoadMinolHandler();
      List<GlobalDeviceId> globalDeviceIdList = new List<GlobalDeviceId>();
      GlobalDeviceId globalDeviceId = this.minolHandler.GetGlobalDeviceId();
      if (globalDeviceId != null)
        globalDeviceIdList.Add(globalDeviceId);
      return globalDeviceIdList;
    }

    public override bool SelectDevice(GlobalDeviceId device)
    {
      this.LoadMinolHandler();
      return this.minolHandler.SelectDevice(device);
    }

    public override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList,
      string serialnumber)
    {
      BusMode? currentBusMode = this.MyDeviceManager.GetCurrentBusMode();
      BusMode? nullable = currentBusMode;
      BusMode busMode = BusMode.Minol_Device;
      if (!(nullable.GetValueOrDefault() == busMode & nullable.HasValue))
        throw new Exception("DeviceCollector settings are wrong! Invalid bus mode: " + currentBusMode.ToString());
      this.LoadMinolHandler();
      GlobalDeviceId globalDeviceId = this.minolHandler.GetGlobalDeviceId();
      if (!this.minolHandler.GetValues(ref ValueList))
        return false;
      ValueIdent.CleanUpEmptyValueIdents(ValueList);
      return globalDeviceId.Serialnumber == serialnumber;
    }

    public override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList)
    {
      this.LoadMinolHandler();
      if (!this.minolHandler.GetValues(ref ValueList))
        return false;
      ValueIdent.CleanUpEmptyValueIdents(ValueList);
      return true;
    }

    public override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList,
      int SubDevice)
    {
      this.LoadMinolHandler();
      if (!this.minolHandler.GetValues(ref ValueList, SubDevice))
        return false;
      ValueIdent.CleanUpEmptyValueIdents(ValueList);
      return true;
    }

    public override SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      ConfigurationParameter.ValueType ConfigurationType,
      int SubDevice)
    {
      this.LoadMinolHandler();
      return this.minolHandler.GetConfigurationParameters(ConfigurationType, SubDevice);
    }

    public override bool ReadConfigurationParameters(out GlobalDeviceId UpdatedDeviceIdentification)
    {
      UpdatedDeviceIdentification = (GlobalDeviceId) null;
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      this.LoadMinolHandler();
      return this.minolHandler.ReadConfigurationParameters(out UpdatedDeviceIdentification);
    }

    public override bool SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameterList,
      int SubDevice)
    {
      this.LoadMinolHandler();
      return this.minolHandler.SetConfigurationParameters(parameterList, SubDevice);
    }

    public override bool SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameterList)
    {
      this.LoadMinolHandler();
      return this.minolHandler.SetConfigurationParameters(parameterList);
    }

    public override bool WriteChangedConfigurationParametersToDevice()
    {
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      this.LoadMinolHandler();
      return this.minolHandler.WriteChangesToDevice();
    }

    private void FireEventOnValueIdentSetReceived(GlobalDeviceId device, int subIndex)
    {
      if (!this.MyDeviceManager.IsValueIdentSetReceivedEventEnabled)
        return;
      DeviceTypes typeOfMinolDevice = NumberRanges.GetTypeOfMinolDevice(device.Serialnumber);
      ValueIdentSet e = new ValueIdentSet()
      {
        Version = device.Generation,
        SerialNumber = device.Serialnumber,
        Manufacturer = "MINOL",
        DeviceType = typeOfMinolDevice.ToString()
      };
      e.Manufacturer = NumberRanges.GetManufacturer(typeOfMinolDevice);
      e.ZDF = "SID;" + device.Serialnumber + ";MAN;" + e.Manufacturer + ";MED;" + e.DeviceType;
      SortedList<long, SortedList<DateTime, ReadingValue>> ValueList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
      if (this.GetValues(ref ValueList, subIndex))
        e.AvailableValues = ValueList;
      this.MyDeviceManager.OnValueIdentSetReceived((object) this, e);
      if (device.SubDevices != null)
      {
        foreach (GlobalDeviceId subDevice in device.SubDevices)
          this.FireEventOnValueIdentSetReceived(subDevice, device.SubDevices.IndexOf(subDevice) + 1);
      }
    }
  }
}
