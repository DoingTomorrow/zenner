// Decompiled with JetBrains decompiler
// Type: MBusLib.Entities.DeviceIdentity
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using System;

#nullable disable
namespace MBusLib.Entities
{
  [Serializable]
  public enum DeviceIdentity : ushort
  {
    None = 0,
    C2 = 1,
    C2a = 2,
    Pulse = 3,
    C5 = 5,
    IUW = 6,
    WR3 = 8,
    WR4 = 9,
    Minoprotect_II = 14, // 0x000E
    Smoke_detector = 15, // 0x000F
    EDC_radio = 17, // 0x0011
    EDC_MBus = 18, // 0x0012
    PDC_wMBus = 19, // 0x0013
    PDC_MBus = 20, // 0x0014
    PDC_Radio3 = 21, // 0x0015
    TH_sensor = 22, // 0x0016
    EDC_SIGFOX = 23, // 0x0017
    PDC_SIGFOX = 24, // 0x0018
    EDC_LoRa = 25, // 0x0019
    PDC_LoRa = 26, // 0x001A
    M8 = 27, // 0x001B
    SD_LoRa = 28, // 0x001C
    micro_LoRa = 29, // 0x001D
    micro_wMBus = 30, // 0x001E
    EDC_wMBus = 31, // 0x001F
    EDC_LoRa_470Mhz = 32, // 0x0020
    EDC_ModBus = 33, // 0x0021
    Bootloader = 34, // 0x0022
    C5_LoRa = 35, // 0x0023
    WR4_LoRa = 36, // 0x0024
    micro_LoRa_LL = 37, // 0x0025
    NFC_SENSUS = 38, // 0x0026
    NDC = 39, // 0x0027
    TH_LoRa = 40, // 0x0028
    EDC_LoRa_915 = 41, // 0x0029
    EDC_NB_IoT = 42, // 0x002A
    IDU = 43, // 0x002B
    micro_wMBus_LL = 44, // 0x002C
    TH_sensor_wMBus = 45, // 0x002D
    PDC_LoRa_915 = 46, // 0x002E
    UDC_LoRa_915 = 47, // 0x002F
    EDC_MBus_CN = 48, // 0x0030
    IDUv1 = 49, // 0x0031
    ODU = 50, // 0x0032
    EDC_MBus_CJ188 = 51, // 0x0033
    EDC_RS485_Modbus = 52, // 0x0034
    EDC_RS485_CJ188 = 53, // 0x0035
    EDC_NB_IoT_LCSW = 54, // 0x0036
    NFC_wMBus = 55, // 0x0037
    NFC_LoRa_wMBus = 56, // 0x0038
    EDC_NB_IoT_YJSW = 57, // 0x0039
    PDC_LoRa_868MHz_Darlington = 58, // 0x003A
    EDC_MBus_STM32 = 59, // 0x003B
    EDC_RS485_STM32 = 60, // 0x003C
    EDC_NB_IoT_FSNH = 61, // 0x003D
    EDC_NB_IoT_XM = 62, // 0x003E
    SD_wM_Bus = 63, // 0x003F
    CO2 = 64, // 0x0040
    TH_sensor_radio = 65, // 0x0041
    Water_8E_radio = 66, // 0x0042
    Water_8E_MBus = 67, // 0x0043
    EDC_NB_IoT_Israel = 68, // 0x0044
    EDC_NB_IoT_TaiWan = 69, // 0x0045
    PDC_GAS = 70, // 0x0046
    M7plus = 71, // 0x0047
    micro_radio3 = 72, // 0x0048
    MinoConnect = 73, // 0x0049
    EDC_LoRa_868_v2 = 74, // 0x004A
    EDC_LoRa_915_v2 = 75, // 0x004B
    GAS = 4064, // 0x0FE0
    C2e = 4065, // 0x0FE1
  }
}
