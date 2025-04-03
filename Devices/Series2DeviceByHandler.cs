// Decompiled with JetBrains decompiler
// Type: Devices.Series2DeviceByHandler
// Assembly: Devices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 793FC2DA-FF88-4FD5-BDE9-C00C0310F1EC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Devices.dll

using DeviceCollector;
using GMM_Handler;
using System;
using System.Collections.Generic;
using System.Globalization;
using ZR_ClassLibrary;

#nullable disable
namespace Devices
{
  public class Series2DeviceByHandler : MBusDeviceHandler
  {
    private string strSoftwareversion;
    private ZR_MeterIdent DeviceIdentity;
    private IZR_HandlerFunctions MyS2Handler = (IZR_HandlerFunctions) null;

    public Series2DeviceByHandler(DeviceManager MyDeviceManager)
      : base(MyDeviceManager)
    {
      if (SystemValues.ZRDezimalSeparator != null)
        return;
      SystemValues.ZRDezimalSeparator = new CultureInfo(CultureInfo.CurrentCulture.Name).NumberFormat.NumberDecimalSeparator;
    }

    private void GarantS2HandlerLoaded()
    {
      if (this.MyS2Handler != null)
        return;
      if (ZR_Component.CommonGmmInterface != null)
      {
        ZR_Component.CommonGmmInterface.GarantComponentLoaded(GMM_Components.GMM_Handler);
        this.MyS2Handler = (IZR_HandlerFunctions) ZR_Component.CommonGmmInterface.LoadedComponentsList[GMM_Components.GMM_Handler];
      }
      else
        this.MyS2Handler = (IZR_HandlerFunctions) new ZR_HandlerFunctions((IDeviceCollector) this.MyDeviceManager.MyBus, this.MyDeviceManager.MyBus.AsyncCom);
      this.MyS2Handler.BackupForEachRead = false;
    }

    public override object GetHandler() => (object) this.MyS2Handler;

    public override List<GlobalDeviceId> GetGlobalDeviceIdList()
    {
      if (this.MyS2Handler == null)
        return (List<GlobalDeviceId>) null;
      GlobalDeviceId deviceIdentification = this.MyS2Handler.GetDeviceIdentification();
      if (deviceIdentification == null)
        return (List<GlobalDeviceId>) null;
      return new List<GlobalDeviceId>()
      {
        deviceIdentification
      };
    }

    public override bool ReadConfigurationParameters(out GlobalDeviceId UpdatedDeviceIdentification)
    {
      UpdatedDeviceIdentification = (GlobalDeviceId) null;
      this.GarantS2HandlerLoaded();
      this.MyDeviceManager.MyBus.IsDeviceModified();
      if (this.MyS2Handler.checkConnection(out this.strSoftwareversion) != 0)
        return false;
      this.MyS2Handler.ReadConnectedDevice(out this.DeviceIdentity);
      UpdatedDeviceIdentification = this.MyS2Handler.GetDeviceIdentification();
      return true;
    }

    public override SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      ConfigurationParameter.ValueType ConfigurationType,
      int SubDevice)
    {
      this.MyDeviceManager.ParameterType = ConfigurationType;
      return this.MyS2Handler.GetConfigurationParameters(ConfigurationParameter.ValueType.Complete, SubDevice);
    }

    public override bool SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> ConfigParameterList,
      int SubDevice)
    {
      this.SetConfigParamList = ConfigParameterList;
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented);
      return false;
    }

    public override bool WriteChangedConfigurationParametersToDevice()
    {
      return this.MyS2Handler.progDevice(!DateTime.Now.IsDaylightSavingTime() ? DateTime.Now : DateTime.Now.AddHours(1.0));
    }
  }
}
