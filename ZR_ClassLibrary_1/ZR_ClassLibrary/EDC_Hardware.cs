// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.EDC_Hardware
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

#nullable disable
namespace ZR_ClassLibrary
{
  public enum EDC_Hardware : ushort
  {
    Unknown = 0,
    EDC_Radio = 17, // 0x0011
    EDC_mBus = 18, // 0x0012
    EDC_LoRa868 = 25, // 0x0019
    Micro_LoRa = 29, // 0x001D
    Micro_WmBus = 30, // 0x001E
    EDC_wMBus = 31, // 0x001F
    EDC_LoRa470 = 32, // 0x0020
    EDC_ModBus = 33, // 0x0021
    EDC_LoRa915 = 41, // 0x0029
    EDC_NBIoT = 42, // 0x002A
    EDC_mBus_Modbus = 48, // 0x0030
    EDC_mBus_CJ188 = 51, // 0x0033
    EDC_RS485_Modbus = 52, // 0x0034
    EDC_RS485_CJ188 = 53, // 0x0035
    EDC_NBIoT_LCSW = 54, // 0x0036
    EDC_NBIoT_YJSW = 57, // 0x0039
    EDC_NBIoT_FSNH = 61, // 0x003D
    EDC_NBIoT_XM = 62, // 0x003E
    EDC_NBIoT_Israel = 68, // 0x0044
    EDC_NBIoT_TaiWan = 69, // 0x0045
  }
}
