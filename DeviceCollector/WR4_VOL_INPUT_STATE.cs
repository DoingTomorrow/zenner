// Decompiled with JetBrains decompiler
// Type: DeviceCollector.WR4_VOL_INPUT_STATE
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

#nullable disable
namespace DeviceCollector
{
  public enum WR4_VOL_INPUT_STATE
  {
    Unknown,
    WaitSetCycle,
    SetCycle,
    WaitReceiveVolume,
    ReceiveVolume,
    WaitRequestID,
    RequestID,
  }
}
