// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.ErrorNumber
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

#nullable disable
namespace ZR_ClassLibrary
{
  public enum ErrorNumber
  {
    None = 0,
    Input_1_frequencyToHigh = 8,
    Input_2_frequencyToHigh = 9,
    Input_3_frequencyToHigh = 10, // 0x0000000A
    SelfTestError = 12, // 0x0000000C
    ReadingDayDataError = 13, // 0x0000000D
    InternalLoopMemoryError = 14, // 0x0000000E
    ExternalLoopMemoryError = 16, // 0x00000010
    ExternalEEPHardwareError = 17, // 0x00000011
    InternalEEPHardwareError = 18, // 0x00000012
    ResetError = 19, // 0x00000013
    ShortCircuitReturnSensor = 34, // 0x00000022
    BrokenReturnSensor = 37, // 0x00000025
    ShortCircuitFlowSensor = 44, // 0x0000002C
    BrokenFlowSensor = 47, // 0x0000002F
    OtherTempMessuringError = 57, // 0x00000039
    ForwardSensorTooSmall = 62, // 0x0000003E
    ForwardSensorTooLarge = 63, // 0x0000003F
    ReturnSensorTooSmall = 64, // 0x00000040
    ReturnSensorTooLarge = 65, // 0x00000041
    FlowReturnSensorWrongWay = 71, // 0x00000047
    BatteryUndervoltage = 77, // 0x0000004D
  }
}
