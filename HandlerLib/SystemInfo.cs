// Decompiled with JetBrains decompiler
// Type: HandlerLib.SystemInfo
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;

#nullable disable
namespace HandlerLib
{
  [Flags]
  public enum SystemInfo : uint
  {
    SYS_INFO_ERROR_MASK = 4278190080, // 0xFF000000
    SYS_INFO_ALERT_MASK = 16711680, // 0x00FF0000
    SYS_INFO_WARNING_MASK = 65535, // 0x0000FFFF
    BatteryDown = 2147483648, // 0x80000000
    TDC_Error = 1073741824, // 0x40000000
    AccumulatedDataLost = 536870912, // 0x20000000
    NoWater = 8388608, // 0x00800000
    Bubbles = 4194304, // 0x00400000
    CRC_ErrorConfiguration = 131072, // 0x00020000
    CRC_ErrorFirmware = 65536, // 0x00010000
    FlowRateOutOfRange = 32768, // 0x00008000
    TemperatureOutOfRange = 16384, // 0x00004000
    ReverseFlow = 8192, // 0x00002000
    TestViewActive = 4096, // 0x00001000
    TemperatureSensorDefect = 2048, // 0x00000800
    UltrasonicChannel2_Corrupt = 1024, // 0x00000400
    UltrasonicChannel1_Corrupt = 512, // 0x00000200
    WriteProtectionNotActive = 256, // 0x00000100
    BatteryUnderVoltage = 128, // 0x00000080
    BatteryLiveTimeOver = 64, // 0x00000040
    DisplayInterpreterError = 32, // 0x00000020
    NFC_TAG_fault = 16, // 0x00000010
    Sleep = 8,
    ScenarioNotSupported = 2,
    SmartFunctionEventDisplayed = 1,
  }
}
