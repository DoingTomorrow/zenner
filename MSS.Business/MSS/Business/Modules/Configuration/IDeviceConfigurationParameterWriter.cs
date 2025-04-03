// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Configuration.IDeviceConfigurationParameterWriter
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.Business.Modules.Configuration
{
  public interface IDeviceConfigurationParameterWriter
  {
    void Write(
      SortedList<OverrideID, ConfigurationParameter> paramsChannel1List,
      SortedList<OverrideID, ConfigurationParameter> paramsChannel2List,
      SortedList<OverrideID, ConfigurationParameter> paramsChannel3List,
      SortedList<OverrideID, ConfigurationParameter> paramsList);
  }
}
