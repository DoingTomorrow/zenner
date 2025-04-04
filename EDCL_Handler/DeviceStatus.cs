// Decompiled with JetBrains decompiler
// Type: EDCL_Handler.DeviceStatus
// Assembly: EDCL_Handler, Version=2.2.10.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F3010E47-8885-4BE8-8551-D37B09710D3C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDCL_Handler.dll

using System;

#nullable disable
namespace EDCL_Handler
{
  [Flags]
  public enum DeviceStatus : ushort
  {
    OK = 0,
    REMOVAL = 1,
    BATTERY = 2,
    BATTERY_END_LIFE = 4,
    HARDWARE = 8,
    TAMPER = 32, // 0x0020
    RADIO_ERROR = 64, // 0x0040
    TRANSCEIVER = 128, // 0x0080
    OVERSIZE = 256, // 0x0100
    UNDERSIZE = 512, // 0x0200
    BLOCK = 1024, // 0x0400
    BACKFLOW = 2048, // 0x0800
    LEAK = 8192, // 0x2000
    BURST = 32768, // 0x8000
  }
}
