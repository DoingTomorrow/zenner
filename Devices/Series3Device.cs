// Decompiled with JetBrains decompiler
// Type: Devices.Series3Device
// Assembly: Devices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 793FC2DA-FF88-4FD5-BDE9-C00C0310F1EC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Devices.dll

using DeviceCollector;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace Devices
{
  internal class Series3Device : MBusDeviceHandler
  {
    internal Series3Device(DeviceManager MyDeviceManager)
      : base(MyDeviceManager)
    {
    }

    public override bool ReadConfigurationParameters(out GlobalDeviceId UpdatedDeviceIdentification)
    {
      bool telegrammEnabled = this.MyDeviceManager.MyBus.IsMultiTelegrammEnabled;
      this.MyDeviceManager.MyBus.IsMultiTelegrammEnabled = false;
      try
      {
        return base.ReadConfigurationParameters(out UpdatedDeviceIdentification);
      }
      finally
      {
        this.MyDeviceManager.MyBus.IsMultiTelegrammEnabled = telegrammEnabled;
      }
    }

    public override SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      ConfigurationParameter.ValueType ConfigurationType,
      int SubDevice)
    {
      SortedList<OverrideID, ConfigurationParameter> configurationParameters = base.GetConfigurationParameters(ConfigurationType, SubDevice) ?? new SortedList<OverrideID, ConfigurationParameter>();
      configurationParameters.Add(OverrideID.RadioProtocol, new ConfigurationParameter(OverrideID.RadioProtocol, (object) RadioProtocol.Undefined)
      {
        HasWritePermission = true
      });
      configurationParameters.Add(OverrideID.DueDateMonth, new ConfigurationParameter(OverrideID.DueDateMonth, (object) 0)
      {
        HasWritePermission = true
      });
      return configurationParameters;
    }

    public override bool WriteChangedConfigurationParametersToDevice()
    {
      if (!base.WriteChangedConfigurationParametersToDevice() || this.SetConfigParamList == null)
        return false;
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      if (this.SetConfigParamList.ContainsKey(OverrideID.RadioProtocol))
      {
        ConfigurationParameter setConfigParam = this.SetConfigParamList[OverrideID.RadioProtocol];
        DeviceCollectorFunctions myBus = this.MyDeviceManager.MyBus;
        ReadVersionData versionData;
        if (!myBus.ReadVersion(out versionData))
          return false;
        bool flag = versionData.Version < 83963909U;
        switch ((RadioProtocol) Enum.Parse(typeof (RadioProtocol), setConfigParam.ParameterValue.ToString(), true))
        {
          case RadioProtocol.Scenario1:
            if (flag)
            {
              if (!myBus.SelectParameterList(2, 1))
                return false;
              break;
            }
            if (!myBus.SelectParameterList(1, 1))
              return false;
            break;
          case RadioProtocol.Scenario3:
            if (flag)
            {
              if (!myBus.SelectParameterList(1, 1))
                return false;
              break;
            }
            if (!myBus.SelectParameterList(3, 1))
              return false;
            break;
          default:
            return false;
        }
      }
      return !this.SetConfigParamList.ContainsKey(OverrideID.DueDateMonth) || this.MyDeviceManager.MyBus.WriteDueDateMonth(Convert.ToUInt16(this.SetConfigParamList[OverrideID.DueDateMonth].ParameterValue));
    }
  }
}
