// Decompiled with JetBrains decompiler
// Type: EDCL_Handler.Calibration
// Assembly: EDCL_Handler, Version=2.2.10.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F3010E47-8885-4BE8-8551-D37B09710D3C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDCL_Handler.dll

using System;

#nullable disable
namespace EDCL_Handler
{
  [Flags]
  public enum Calibration : byte
  {
    No = 0,
    VCC2_IS_STABLE = 1,
    DAC_OFFSET_FOUND = 2,
    DAC_CHECKED = 4,
    DAC_OFFSET_FOUND_COILS_STABLE = 8,
    CHECK_DAC_OFFSET_NOM_READY = 16, // 0x10
    FINISHED = 32, // 0x20
    WATCHDOG_RESET = 64, // 0x40
    SUCCESSFUL = FINISHED | CHECK_DAC_OFFSET_NOM_READY | DAC_OFFSET_FOUND_COILS_STABLE | DAC_CHECKED | DAC_OFFSET_FOUND | VCC2_IS_STABLE, // 0x3F
  }
}
