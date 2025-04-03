// Decompiled with JetBrains decompiler
// Type: HandlerLib.DeviceStateCounterID
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib
{
  public enum DeviceStateCounterID : byte
  {
    OperationTime = 1,
    ZeroFlowTime = 2,
    FlowTime = 3,
    BackFlowTime = 4,
    AirInTubeTime = 5,
    OverloadTime = 6,
    NFC_ActivationTime = 7,
    RadioActivationCounts = 8,
    TemperatureSensorErrorTime = 9,
    OneSensorPairErrorTime = 10, // 0x0A
    ResetCounts = 11, // 0x0B
    WatchDogCounts = 12, // 0x0C
    TDCResetCounts = 13, // 0x0D
    SmartFunctionsCpuMsTime = 14, // 0x0E
  }
}
