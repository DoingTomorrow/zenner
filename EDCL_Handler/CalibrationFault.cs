// Decompiled with JetBrains decompiler
// Type: EDCL_Handler.CalibrationFault
// Assembly: EDCL_Handler, Version=2.2.10.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F3010E47-8885-4BE8-8551-D37B09710D3C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDCL_Handler.dll

using System;

#nullable disable
namespace EDCL_Handler
{
  [Flags]
  public enum CalibrationFault : byte
  {
    OK = 0,
    COIL_A = 1,
    COIL_B = 2,
    LOW_COILS_COUNTER = 4,
    DAC_CHECK = 8,
    DAC_PULS_COUNT_CONDITION1_HURT = 16, // 0x10
    DAC_PULS_COUNT_CONDITION2_HURT = 32, // 0x20
    DAC_OVERFLOW = 64, // 0x40
    DAC_UNDERFLOW = 128, // 0x80
  }
}
