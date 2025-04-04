// Decompiled with JetBrains decompiler
// Type: EDC_Handler.ParamCode
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

#nullable disable
namespace EDC_Handler
{
  public enum ParamCode
  {
    NULL = 0,
    DATE = 16, // 0x00000010
    DATE_TIME = 32, // 0x00000020
    LOG_DATE = 48, // 0x00000030
    LOG_DATE_TIME = 64, // 0x00000040
    LOG_VALUE = 80, // 0x00000050
    LOG_VALUE_BCD = 96, // 0x00000060
    VALUE_BCD = 112, // 0x00000070
    VALUE = 128, // 0x00000080
    STORE_SAVE = 144, // 0x00000090
    STORE_DIFF = 160, // 0x000000A0
    STORE_DIFF_BCD = 176, // 0x000000B0
  }
}
