// Decompiled with JetBrains decompiler
// Type: EDC_Handler.ControlLogger
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using System;

#nullable disable
namespace EDC_Handler
{
  [Flags]
  public enum ControlLogger
  {
    LOG_NULL = 0,
    LOG_RESET = 4096, // 0x00001000
    LOG_STICHTAG_RESET = 8192, // 0x00002000
    LOG_NEXT = LOG_STICHTAG_RESET | LOG_RESET, // 0x00003000
    LOG_TABLE_SELECT = 16384, // 0x00004000
    LOG_HISTORY_INDEX = LOG_TABLE_SELECT | LOG_RESET, // 0x00005000
    LOG_TABLE_INDEX = LOG_TABLE_SELECT | LOG_STICHTAG_RESET, // 0x00006000
  }
}
