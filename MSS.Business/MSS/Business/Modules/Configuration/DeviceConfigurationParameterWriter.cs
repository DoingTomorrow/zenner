// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Configuration.DeviceConfigurationParameterWriter
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Modules.GMMWrapper;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.Business.Modules.Configuration
{
  public class DeviceConfigurationParameterWriter : IDeviceConfigurationParameterWriter
  {
    private readonly IConfiguratorManager _configuratorManager;

    public DeviceConfigurationParameterWriter(IConfiguratorManager configuratorManager)
    {
      this._configuratorManager = configuratorManager;
    }

    public void Write(
      SortedList<OverrideID, ConfigurationParameter> paramsChannel1List,
      SortedList<OverrideID, ConfigurationParameter> paramsChannel2List,
      SortedList<OverrideID, ConfigurationParameter> paramsChannel3List,
      SortedList<OverrideID, ConfigurationParameter> paramsList)
    {
      try
      {
        this.SetConfigParams(paramsChannel1List, paramsChannel2List, paramsChannel3List, paramsList);
        this._configuratorManager.WriteDevice();
      }
      finally
      {
        this._configuratorManager.CloseConnection();
      }
    }

    private void SetConfigParams(
      SortedList<OverrideID, ConfigurationParameter> paramsChannel1List,
      SortedList<OverrideID, ConfigurationParameter> paramsChannel2List,
      SortedList<OverrideID, ConfigurationParameter> paramsChannel3List,
      SortedList<OverrideID, ConfigurationParameter> paramsList)
    {
      this._configuratorManager.SetConfigurationParameters(paramsChannel1List, 1);
      this._configuratorManager.SetConfigurationParameters(paramsChannel2List, 2);
      this._configuratorManager.SetConfigurationParameters(paramsChannel3List, 3);
      this._configuratorManager.SetConfigurationParameters(paramsList);
    }
  }
}
