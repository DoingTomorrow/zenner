// Decompiled with JetBrains decompiler
// Type: PDC_Handler.Warning
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using System;

#nullable disable
namespace PDC_Handler
{
  [Flags]
  public enum Warning : ushort
  {
    WARNING_APP_BUSY = 1,
    WARNING_ABNORMAL = 2,
    WARNING_BATT_LOW = 4,
    WARNING_PERMANENT_ERROR = 8,
    WARNING_TEMPORARY = 16, // 0x0010
    WARNING_LEAK = 256, // 0x0100
    WARNING_LEAK_H = 32, // 0x0020
    WARNING_BLOCK = 512, // 0x0200
    WARNING_BLOCK_H = 64, // 0x0040
    WARNING_UNDERSIZE = 1024, // 0x0400
    WARNING_OVERSIZE = 2048, // 0x0800
    WARNING_BURST = 128, // 0x0080
  }
}
