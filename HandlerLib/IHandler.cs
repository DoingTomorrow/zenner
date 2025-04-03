// Decompiled with JetBrains decompiler
// Type: HandlerLib.IHandler
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommunicationPort.Functions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public interface IHandler : IReadoutConfig
  {
    void Open();

    void Close();

    DateTime? SaveMeter();

    Task<int> ReadDeviceAsync(
      ProgressHandler progress,
      CancellationToken token,
      ReadPartsSelection readPartsSelection);

    Task WriteDeviceAsync(ProgressHandler progress, CancellationToken token);

    SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(int subDevice = 0);

    void SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameter,
      int subDevice = 0);

    SortedList<long, SortedList<DateTime, double>> GetValues(int subDevice = 0);

    void SetCommunicationPort(CommunicationPortFunctions myPort = null);
  }
}
