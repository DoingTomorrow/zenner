// Decompiled with JetBrains decompiler
// Type: EDC_Handler.Warning
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using System;

#nullable disable
namespace EDC_Handler
{
  [Flags]
  public enum Warning : ushort
  {
    APP_BUSY = 1,
    ABNORMAL = 2,
    BATT_LOW = 4,
    PERMANENT_ERROR = 8,
    TEMPORARY_ERROR = 16, // 0x0010
    TAMPER_A = 64, // 0x0040
    REMOVAL_A = 128, // 0x0080
    LEAK = 8192, // 0x2000
    LEAK_A = 32, // 0x0020
    UNDERSIZE = 512, // 0x0200
    BLOCK_A = 1024, // 0x0400
    BACKFLOW = 2048, // 0x0800
    BACKFLOW_A = 4096, // 0x1000
    INTERFERE = 16384, // 0x4000
    OVERSIZE = 256, // 0x0100
    BURST = 32768, // 0x8000
  }
}
