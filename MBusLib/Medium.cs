// Decompiled with JetBrains decompiler
// Type: MBusLib.Medium
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

#nullable disable
namespace MBusLib
{
  public enum Medium : byte
  {
    OTHER = 0,
    OIL = 1,
    ELECTRICITY = 2,
    GAS = 3,
    HEAT_OUTLET = 4,
    STEAM = 5,
    HOT_WATER = 6,
    WATER = 7,
    HCA = 8,
    COMPRESSED_AIR = 9,
    COOL_OUTLET = 10, // 0x0A
    COOL_INLET = 11, // 0x0B
    HEAT_INLET = 12, // 0x0C
    HEAT_AND_COOL = 13, // 0x0D
    BUS_SYSTEM = 14, // 0x0E
    UNKNOWN = 15, // 0x0F
    HEAT_ENERGY_VALUE = 20, // 0x14
    HOT_WATER_90 = 21, // 0x15
    COLD_WATER = 22, // 0x16
    HOT_AND_COLD_WATER = 23, // 0x17
    PRESSURE = 24, // 0x18
    AD_CONVERTER = 25, // 0x19
    SMOKE_DETECTOR = 26, // 0x1A
    ROOM_SENSOR = 27, // 0x1B
    GAS_DETECTOR = 28, // 0x1C
    CIRCUIT_BREAKER = 32, // 0x20
    GAS_WATER_OUTLET = 33, // 0x21
    CUSTOMER_DISPLAY = 37, // 0x25
    EFFLUENT_WATER = 40, // 0x28
    WASTE = 41, // 0x29
    CARBON_MONOXIDE = 42, // 0x2A
    RF_Adapter = 55, // 0x37
  }
}
