// Decompiled with JetBrains decompiler
// Type: SmokeDetectorHandler.InterlinkedDeviceStatus
// Assembly: SmokeDetectorHandler, Version=2.20.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E8970E7-4D1B-41F1-9589-E7C5C5D80A7B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmokeDetectorHandler.dll

#nullable disable
namespace SmokeDetectorHandler
{
  public enum InterlinkedDeviceStatus : byte
  {
    NormalOperationNoSignal,
    SmokeAlarmCondition,
    TestAlarmCondition,
    BatteryFaultCondition,
    SmokeAlarmOff,
    BatteryFaultOff,
  }
}
