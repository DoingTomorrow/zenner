// Decompiled with JetBrains decompiler
// Type: DeviceCollector.SmokeDetectorEvent
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using System;

#nullable disable
namespace DeviceCollector
{
  [Flags]
  public enum SmokeDetectorEvent : ushort
  {
    BatteryForewarning = 1,
    BatteryFault = 2,
    BatteryWarningRadio = 4,
    SmokeChamberPollutionForewarning = 8,
    SmokeChamberPollutionWarning = 16, // 0x0010
    PushButtonFailure = 32, // 0x0020
    HornFailure = 64, // 0x0040
    RemovingDetection = 128, // 0x0080
    TestAlarmReleased = 256, // 0x0100
    SmokeAlarmReleased = 512, // 0x0200
    IngressAperturesObstructionDetected = 1024, // 0x0400
    ObjectInSurroundingAreaDetected = 2048, // 0x0800
    LED_Failure = 4096, // 0x1000
    Bit13_undefined = 8192, // 0x2000
    Bit14_undefined = 16384, // 0x4000
    Bit15_undefined = 32768, // 0x8000
  }
}
