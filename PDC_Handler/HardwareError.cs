// Decompiled with JetBrains decompiler
// Type: PDC_Handler.HardwareError
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using System;

#nullable disable
namespace PDC_Handler
{
  [Flags]
  public enum HardwareError : ushort
  {
    HW_ERROR_OSCILLATOR = 1,
    HW_ERROR_RADIOCAL = 2,
    HW_ERROR_CALLBACK = 4,
    HW_ERROR_BOR = 8,
    HW_ERROR_RAM = 16, // 0x0010
    HW_ERROR_OVERSIZE_A = 256, // 0x0100
    HW_ERROR_OVERSIZE_B = 512, // 0x0200
    HW_ERROR_UNDERSIZE_A = 1024, // 0x0400
    HW_ERROR_UNDERSIZE_B = 2048, // 0x0800
  }
}
