// Decompiled with JetBrains decompiler
// Type: ZENNER.EDC2_Warning
// Assembly: GmmInterface, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 25F1E48F-52B7-4A4F-B66A-62C91CCF5C52
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmInterface.dll

using System;

#nullable disable
namespace ZENNER
{
  [Flags]
  [Serializable]
  public enum EDC2_Warning : ushort
  {
    OK = 0,
    APP_BUSY = 1,
    ABNORMAL = 2,
    BATT_LOW = 4,
    PERMANENT_ERROR = 8,
    TEMPORARY_ERROR = 16, // 0x0010
    LEAK_A = 32, // 0x0020
    TAMPER_A = 64, // 0x0040
    REMOVAL_A = 128, // 0x0080
    OVERSIZE = 256, // 0x0100
    UNDERSIZE = 512, // 0x0200
    BLOCK_A = 1024, // 0x0400
    BACKFLOW = 2048, // 0x0800
    BACKFLOW_A = 4096, // 0x1000
    LEAK = 8192, // 0x2000
    INTERFERE = 16384, // 0x4000
    BURST = 32768, // 0x8000
  }
}
