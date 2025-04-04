// Decompiled with JetBrains decompiler
// Type: M8_Handler.DeviceStatus
// Assembly: M8_Handler, Version=2.0.6.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 582F1296-F274-42DF-B72B-4C0B4D92AA72
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\M8_Handler.dll

using System;

#nullable disable
namespace M8_Handler
{
  [Flags]
  public enum DeviceStatus : ushort
  {
    TAMPER = 1,
    BATTERY = 2,
    BATTERY_END_LIFE = 4,
    HARDWARE = 8,
    HCA = 16, // 0x0010
    VERSION_LUT = 32, // 0x0020
  }
}
