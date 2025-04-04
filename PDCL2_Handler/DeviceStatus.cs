// Decompiled with JetBrains decompiler
// Type: PDCL2_Handler.DeviceStatus
// Assembly: PDCL2_Handler, Version=2.22.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 03BA4C2D-69FE-4DA6-9C3F-B3D5471C4058
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDCL2_Handler.dll

using System;

#nullable disable
namespace PDCL2_Handler
{
  [Flags]
  public enum DeviceStatus : ushort
  {
    OK = 0,
    Tamper = 1,
    Battery = 2,
    BatteryEndLife = 4,
    Hardware = 8,
  }
}
