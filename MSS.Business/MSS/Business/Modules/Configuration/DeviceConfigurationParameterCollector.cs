// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Configuration.DeviceConfigurationParameterCollector
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Modules.GMMWrapper;
using System.Collections.Generic;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS.Business.Modules.Configuration
{
  public class DeviceConfigurationParameterCollector : IDeviceConfigurationParameterCollector
  {
    public const int MaxChannels = 4;
    private readonly IConfiguratorManager _configuratorManager;
    private readonly IConfigurationValuesToListConfigConverter _converter;

    public DeviceConfigurationParameterCollector(
      IConfiguratorManager configuratorManager,
      IConfigurationValuesToListConfigConverter configurationValuesToListConfigConverter)
    {
      this._configuratorManager = configuratorManager;
      this._converter = configurationValuesToListConfigConverter;
    }

    public List<ConfigurationPerChannel> Collect(ConnectionAdjuster connectionAdjuster)
    {
      try
      {
        return this.GetConfigurationParameters(this._configuratorManager.ReadDevice(connectionAdjuster));
      }
      finally
      {
        this._configuratorManager.CloseConnection();
      }
    }

    private List<ConfigurationPerChannel> GetConfigurationParameters(int count)
    {
      List<ConfigurationPerChannel> dictConfigParams = new List<ConfigurationPerChannel>();
      for (int inputType = 0; inputType < 4; ++inputType)
      {
        if (count > inputType)
          this.AddConfigParams(dictConfigParams, inputType);
      }
      return dictConfigParams;
    }

    private void AddConfigParams(List<ConfigurationPerChannel> dictConfigParams, int inputType)
    {
      List<Config> configurationValues = this._converter.GetConfigurationValues(this._configuratorManager.GetConfigurationParameters(inputType));
      dictConfigParams.Add(new ConfigurationPerChannel()
      {
        ChannelNr = inputType,
        ConfigValues = configurationValues
      });
    }
  }
}
