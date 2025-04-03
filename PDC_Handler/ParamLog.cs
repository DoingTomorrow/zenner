// Decompiled with JetBrains decompiler
// Type: PDC_Handler.ParamLog
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

#nullable disable
namespace PDC_Handler
{
  public enum ParamLog : ushort
  {
    LOG_NULL = 0,
    LOG_RESET = 4096, // 0x1000
    LOG_STICHTAG_RESET = 8192, // 0x2000
    LOG_NEXT = 12288, // 0x3000
    LOG_TABLE_SELECT = 16384, // 0x4000
    LOG_HISTORY_INDEX = 20480, // 0x5000
    LOG_TABLE_INDEX = 24576, // 0x6000
    LOG_CHANNEL = 28672, // 0x7000
  }
}
