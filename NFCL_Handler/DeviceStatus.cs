// Decompiled with JetBrains decompiler
// Type: NFCL_Handler.DeviceStatus
// Assembly: NFCL_Handler, Version=2.3.2.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 887E21A2-7448-48CC-AF3E-C39E4C7B3AFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NFCL_Handler.dll

using System;

#nullable disable
namespace NFCL_Handler
{
  [Flags]
  public enum DeviceStatus : ushort
  {
    OK = 0,
    Battery = 2,
    BatteryEndLife = 4,
    Hardware = 8,
  }
}
