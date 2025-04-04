// Decompiled with JetBrains decompiler
// Type: EDCL_Handler.HardwareStatus
// Assembly: EDCL_Handler, Version=2.2.10.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F3010E47-8885-4BE8-8551-D37B09710D3C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDCL_Handler.dll

using System;

#nullable disable
namespace EDCL_Handler
{
  [Flags]
  public enum HardwareStatus : ushort
  {
    OK = 0,
    COILFAIL = 16, // 0x0010
    COILWARNING = 32, // 0x0020
    OVERSIZE = 64, // 0x0040
    UNDERSIZE = 128, // 0x0080
    TAMPER_ACTIVE = 256, // 0x0100
    TAMPER = 1024, // 0x0400
    REMOVAL = 2048, // 0x0800
    REMOVAL_ACTIVE = 512, // 0x0200
    BATTERY_END_LIFE = 32768, // 0x8000
    INIT_DAC_COIL_FAULT = 4096, // 0x1000
    INIT_DAC_OVERFLOW = 8192, // 0x2000
    INIT_DAC_UNDERFLOW = 16384, // 0x4000
  }
}
