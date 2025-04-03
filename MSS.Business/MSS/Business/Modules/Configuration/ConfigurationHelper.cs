// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Configuration.ConfigurationHelper
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS.Business.Modules.Configuration
{
  public static class ConfigurationHelper
  {
    public static byte[] SerializeConfigurationParameters(
      List<ConfigurationPerChannel> configurationParametersPerChannel)
    {
      foreach (ConfigurationPerChannel configurationPerChannel in configurationParametersPerChannel)
      {
        foreach (Config configValue in configurationPerChannel.ConfigValues)
        {
          configValue.Parameter = (object) null;
          configValue.IsReadOnly = true;
        }
      }
      XmlSerializer xmlSerializer = new XmlSerializer(configurationParametersPerChannel.GetType());
      MemoryStream memoryStream = new MemoryStream();
      xmlSerializer.Serialize((Stream) memoryStream, (object) configurationParametersPerChannel);
      return memoryStream.ToArray();
    }

    public static List<ConfigurationPerChannel> DeserializeConfigurationParameters(
      byte[] gmmParameters)
    {
      return new XmlSerializer(typeof (List<ConfigurationPerChannel>)).Deserialize((Stream) new MemoryStream(gmmParameters)) as List<ConfigurationPerChannel>;
    }

    public static bool HasChangeableParameters(DeviceModel deviceModel)
    {
      return deviceModel?.ChangeableParameters != null && deviceModel.ChangeableParameters.Any<ChangeableParameter>();
    }
  }
}
