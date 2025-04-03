// Decompiled with JetBrains decompiler
// Type: HandlerLib.EDC_Warning
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;

#nullable disable
namespace HandlerLib
{
  [Flags]
  internal enum EDC_Warning : ushort
  {
    OK = 0,
    ABNORMAL = 2,
    BATT_LOW = 4,
    PERMANENT_ERROR = 8,
    INTERFERE = 16384, // 0x4000
    TAMPER_A = 64, // 0x0040
    REMOVAL_A = 128, // 0x0080
    LEAK = 8192, // 0x2000
    LEAK_A = 32, // 0x0020
    BLOCK_A = 1024, // 0x0400
    BACKFLOW = 2048, // 0x0800
    BACKFLOW_A = 4096, // 0x1000
    UNDERSIZE = 512, // 0x0200
    OVERSIZE = 256, // 0x0100
    BURST = 32768, // 0x8000
    TEMPORARY = 16, // 0x0010
  }
}
