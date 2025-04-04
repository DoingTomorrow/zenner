// Decompiled with JetBrains decompiler
// Type: THL_Handler.DeviceStatus
// Assembly: THL_Handler, Version=1.0.5.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: C9669406-A704-45DE-B726-D8A41F27FFB8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\THL_Handler.dll

using System;

#nullable disable
namespace THL_Handler
{
  [Flags]
  public enum DeviceStatus : ushort
  {
    OK = 0,
    Removal = 1,
    Battery = 2,
    BatteryEndLife = 4,
    Hardware = 8,
    Tamper = 32, // 0x0020
  }
}
