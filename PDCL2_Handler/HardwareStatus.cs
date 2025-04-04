// Decompiled with JetBrains decompiler
// Type: PDCL2_Handler.HardwareStatus
// Assembly: PDCL2_Handler, Version=2.22.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 03BA4C2D-69FE-4DA6-9C3F-B3D5471C4058
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDCL2_Handler.dll

using System;

#nullable disable
namespace PDCL2_Handler
{
  [Flags]
  public enum HardwareStatus : ushort
  {
    HW_NO_ERROR = 0,
    HW_ERROR_COILFAIL = 16, // 0x0010
    HW_ERROR_COILWARNING = 32, // 0x0020
    HW_ERROR_OVERSIZE = 64, // 0x0040
    HW_ERROR_UNDERSIZE = 128, // 0x0080
    HW_ERROR_TAMPER_ACTIVE = 256, // 0x0100
    HW_ERROR_TAMPER = 1024, // 0x0400
    HW_ERROR_REMOVAL = 512, // 0x0200
  }
}
