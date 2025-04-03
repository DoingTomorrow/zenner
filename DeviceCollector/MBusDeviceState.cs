// Decompiled with JetBrains decompiler
// Type: DeviceCollector.MBusDeviceState
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

#nullable disable
namespace DeviceCollector
{
  public enum MBusDeviceState
  {
    NoError = 0,
    Busy = 1,
    AnyError = 2,
    PowerLow = 4,
    PowerLowError = 6,
    PermanentError = 10, // 0x0000000A
    TemporaryError = 18, // 0x00000012
  }
}
