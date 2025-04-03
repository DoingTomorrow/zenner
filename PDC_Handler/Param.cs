// Decompiled with JetBrains decompiler
// Type: PDC_Handler.Param
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

#nullable disable
namespace PDC_Handler
{
  public enum Param : ushort
  {
    NULL = 0,
    DATE = 16, // 0x0010
    DATE_TIME = 32, // 0x0020
    LOG_DATE = 48, // 0x0030
    LOG_DATE_TIME = 64, // 0x0040
    LOG_VALUE = 80, // 0x0050
    LOG_VALUE_BCD = 96, // 0x0060
    VALUE_BCD = 112, // 0x0070
    VALUE = 128, // 0x0080
    STORE_SAVE = 144, // 0x0090
    STORE_DIFF = 160, // 0x00A0
    STORE_DIFF_BCD = 176, // 0x00B0
    STORE2_BYTE = 192, // 0x00C0
    STORE2_SHORT = 208, // 0x00D0
  }
}
