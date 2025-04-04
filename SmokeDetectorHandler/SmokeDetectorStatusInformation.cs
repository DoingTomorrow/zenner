// Decompiled with JetBrains decompiler
// Type: SmokeDetectorHandler.SmokeDetectorStatusInformation
// Assembly: SmokeDetectorHandler, Version=2.20.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E8970E7-4D1B-41F1-9589-E7C5C5D80A7B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmokeDetectorHandler.dll

using System;

#nullable disable
namespace SmokeDetectorHandler
{
  [Flags]
  public enum SmokeDetectorStatusInformation : byte
  {
    None = 0,
    EventDataIsBinaryCoded = 1,
    EventDataIsStoredAtSpecificTime = 2,
    BatteryFault = 4,
    PermanentDeviceError = 8,
    TemporaryDeviceError = 16, // 0x10
    Bit5 = 32, // 0x20
    Bit6 = 64, // 0x40
    Bit7 = 128, // 0x80
  }
}
