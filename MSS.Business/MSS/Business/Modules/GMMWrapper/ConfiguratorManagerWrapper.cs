// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.GMMWrapper.ConfiguratorManagerWrapper
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using System.Collections.Generic;
using ZENNER;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.Business.Modules.GMMWrapper
{
  public class ConfiguratorManagerWrapper : IConfiguratorManager
  {
    public int ReadDevice(ConnectionAdjuster connectionAdjuster)
    {
      return GmmInterface.ConfiguratorManager.ReadDevice(connectionAdjuster);
    }

    public SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(int channel)
    {
      return GmmInterface.ConfiguratorManager.GetConfigurationParameters(channel);
    }

    public void CloseConnection() => GmmInterface.ConfiguratorManager.CloseConnection();

    public void SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameter)
    {
      GmmInterface.ConfiguratorManager.SetConfigurationParameters(parameter);
    }

    public void SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameter,
      int channel)
    {
      GmmInterface.ConfiguratorManager.SetConfigurationParameters(parameter, channel);
    }

    public void WriteDevice() => GmmInterface.ConfiguratorManager.WriteDevice();
  }
}
