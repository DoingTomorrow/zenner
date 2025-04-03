// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.GMMWrapper.IConfiguratorManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using System.Collections.Generic;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.Business.Modules.GMMWrapper
{
  public interface IConfiguratorManager
  {
    int ReadDevice(ConnectionAdjuster connectionAdjuster);

    SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(int channel);

    void SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameter);

    void SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameter,
      int channel);

    void CloseConnection();

    void WriteDevice();
  }
}
