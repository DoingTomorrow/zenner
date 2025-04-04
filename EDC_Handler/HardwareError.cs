// Decompiled with JetBrains decompiler
// Type: EDC_Handler.HardwareError
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using System;

#nullable disable
namespace EDC_Handler
{
  [Flags]
  public enum HardwareError : ushort
  {
    ERROR_OSCILLATOR = 1,
    ERROR_RADIOCAL = 2,
    ERROR_CALLBACK = 4,
    COILFAIL = 16, // 0x0010
    COILWARNING = 32, // 0x0020
    OVERSIZE = 64, // 0x0040
    UNDERSIZE = 128, // 0x0080
    TAMPER = 256, // 0x0100
    REMOVAL = 512, // 0x0200
  }
}
