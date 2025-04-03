// Decompiled with JetBrains decompiler
// Type: ZENNER.ICommand
// Assembly: GmmInterface, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 25F1E48F-52B7-4A4F-B66A-62C91CCF5C52
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmInterface.dll

using HandlerLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace ZENNER
{
  public interface ICommand
  {
    Task<DeviceVersionMBus> ReadVersionAsync(ProgressHandler progress, CancellationToken token);

    Task<DateTime?> GetSystemTimeAsync(ProgressHandler progress, CancellationToken token);

    Task<DateTime?> GetKeyDateAsync(ProgressHandler progress, CancellationToken token);

    Task<string> GetDevEUIAsync(ProgressHandler progress, CancellationToken token);

    Task<DeviceMode> GetModeAsync(ProgressHandler progress, CancellationToken token);

    Task<TransmissionScenario> GetTransmissionScenarioAsync(
      ProgressHandler progress,
      CancellationToken token);

    Task<double> GetProductFactorAsync(ProgressHandler progress, CancellationToken token);

    Task<uint> GetChannelValueAsync(
      byte channel,
      ProgressHandler progress,
      CancellationToken token);

    Task<ActivationMode> GetActivationModeAsync(ProgressHandler progress, CancellationToken token);

    Task<long> GetRadio3_IDAsync(ProgressHandler progress, CancellationToken token);

    Task SetKeyDateAsync(DateTime date, ProgressHandler progress, CancellationToken token);

    Task SetModeAsync(DeviceMode mode, ProgressHandler progress, CancellationToken token);

    Task SetTransmissionScenarioAsync(
      TransmissionScenario scenario,
      ProgressHandler progress,
      CancellationToken token);

    Task SetProductFactorAsync(double factor, ProgressHandler progress, CancellationToken token);

    Task SetChannelValueAsync(
      byte channel,
      uint value,
      ProgressHandler progress,
      CancellationToken token);

    Task<List<Device>> ReadValuesAsync(ProgressHandler progress, CancellationToken token);

    Task<IEnumerable> ReadEventsAsync(ProgressHandler progress, CancellationToken token);

    Task SendJoinRequestAsync(ProgressHandler progress, CancellationToken token);

    Task BackupDeviceAsync(ProgressHandler progress, CancellationToken token);

    Task ResetDeviceAsync(ProgressHandler progress, CancellationToken token);

    void Disconnect();
  }
}
