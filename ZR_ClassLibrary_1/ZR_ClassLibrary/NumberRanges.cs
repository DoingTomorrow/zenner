// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.NumberRanges
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;

#nullable disable
namespace ZR_ClassLibrary
{
  public static class NumberRanges
  {
    public static DeviceTypes GetTypeOfMinolDevice(string funkId)
    {
      long funkId1;
      return string.IsNullOrEmpty(funkId) || !Util.TryParseToInt64(funkId, out funkId1) ? DeviceTypes.None : NumberRanges.GetTypeOfMinolDevice(funkId1);
    }

    public static DeviceTypes GetTypeOfMinolDevice(long funkId)
    {
      if (funkId >= 0L && funkId <= 9999999L)
        return DeviceTypes.SmokeDetector;
      if (funkId >= 30000000L && funkId <= 32999999L)
        return funkId % 2L == 0L ? DeviceTypes.TemperatureSensor : DeviceTypes.HumiditySensor;
      if (funkId >= 33000000L && funkId <= 36999999L)
        return DeviceTypes.TemperatureSensor;
      if (funkId >= 40000000L && funkId <= 40002759L || funkId >= 40002760L && funkId <= 40007000L || funkId >= 40007001L && funkId <= 41099999L || funkId >= 41100000L && funkId <= 41999999L)
        return DeviceTypes.MinotelContact;
      if (funkId >= 42000000L && funkId <= 42899999L || funkId >= 42900000L && funkId <= 42999999L)
        return DeviceTypes.MinotelContactRadio3;
      if (funkId >= 50000000L && funkId <= 50999999L || funkId >= 51000000L && funkId <= 51099999L || funkId >= 51100000L && funkId <= 51999999L || funkId >= 52000000L && funkId <= 52099999L)
        return DeviceTypes.Aqua;
      if (funkId >= 52100000L && funkId <= 52199999L || funkId >= 52200000L && funkId <= 52999999L || funkId >= 53000000L && funkId <= 53000999L)
        return DeviceTypes.AquaMicro;
      if (funkId >= 53001000L && funkId <= 53099999L || funkId >= 53100000L && funkId <= 53999999L || funkId >= 54000000L && funkId <= 54999999L)
        return DeviceTypes.AquaMicroRadio3;
      if (funkId >= 56000000L && funkId <= 56999999L)
        return DeviceTypes.EDC;
      if (funkId >= 73000000L && funkId <= 73999999L)
        return DeviceTypes.ZR_Serie3;
      if (funkId >= 80000000L && funkId <= 80020233L || funkId >= 80020234L && funkId <= 80022450L)
        return DeviceTypes.EHCA_M6;
      if (funkId >= 80022451L && funkId <= 80999999L)
        return DeviceTypes.EHCA_M6_Radio3;
      if (funkId >= 81000000L && funkId <= 81099999L || funkId >= 81100000L && funkId <= 82999999L)
        return DeviceTypes.EHCA_M6;
      if (funkId >= 83000000L && funkId <= 89999999L)
        return DeviceTypes.EHCA_M6_Radio3;
      return funkId >= 90000000L && funkId <= 99999999L ? DeviceTypes.SmokeDetector : DeviceTypes.None;
    }

    public static ValueIdent.ValueIdPart_MeterType GetValueIdPart_MeterTypeOfMinolDevice(
      string funkId)
    {
      uint funkId1 = 0;
      if (!Util.TryParseToUInt32(funkId, out funkId1))
        throw new ArgumentException("Can not parse input parameter 'serialnumber' to UInt32! Value: " + funkId);
      return NumberRanges.GetValueIdPart_MeterTypeOfMinolDevice((long) funkId1);
    }

    public static ValueIdent.ValueIdPart_MeterType GetValueIdPart_MeterTypeOfMinolDevice(long funkId)
    {
      ValueIdent.ValueIdPart_MeterType typeOfMinolDevice;
      switch (NumberRanges.GetTypeOfMinolDevice(funkId))
      {
        case DeviceTypes.ZR_Serie1:
        case DeviceTypes.ZR_Serie2:
        case DeviceTypes.ZR_Serie3:
          typeOfMinolDevice = ValueIdent.ValueIdPart_MeterType.Heat;
          break;
        case DeviceTypes.EHCA_M5:
        case DeviceTypes.EHCA_M5p:
        case DeviceTypes.EHCA_M6:
        case DeviceTypes.EHCA_M6_Radio3:
          typeOfMinolDevice = ValueIdent.ValueIdPart_MeterType.HeatCostAllocator;
          break;
        case DeviceTypes.MinotelContact:
        case DeviceTypes.MinotelContactRadio3:
        case DeviceTypes.PDC:
          typeOfMinolDevice = ValueIdent.ValueIdPart_MeterType.PulseCounter;
          break;
        case DeviceTypes.Aqua:
        case DeviceTypes.AquaMicro:
        case DeviceTypes.AquaMicroRadio3:
        case DeviceTypes.ISF:
        case DeviceTypes.EDC:
          typeOfMinolDevice = ValueIdent.ValueIdPart_MeterType.Water;
          break;
        case DeviceTypes.SmokeDetector:
          typeOfMinolDevice = ValueIdent.ValueIdPart_MeterType.SmokeDetector;
          break;
        case DeviceTypes.TemperatureSensor:
          typeOfMinolDevice = ValueIdent.ValueIdPart_MeterType.Thermometer;
          break;
        case DeviceTypes.HumiditySensor:
          typeOfMinolDevice = ValueIdent.ValueIdPart_MeterType.Hygrometer;
          break;
        default:
          typeOfMinolDevice = ValueIdent.ValueIdPart_MeterType.Any;
          break;
      }
      return typeOfMinolDevice;
    }

    public static string GetManufacturer(DeviceTypes t)
    {
      switch (t)
      {
        case DeviceTypes.ZR_Serie1:
        case DeviceTypes.ZR_Serie2:
        case DeviceTypes.ZR_EHCA:
        case DeviceTypes.ZR_RDM:
        case DeviceTypes.ZR_Serie3:
        case DeviceTypes.MinoConnect:
        case DeviceTypes.EDC:
        case DeviceTypes.PDC:
          return "ZENNER";
        case DeviceTypes.WaveFlowDevice:
          return "Wavenis";
        case DeviceTypes.Minol_Device:
        case DeviceTypes.MinomatDevice:
        case DeviceTypes.EHCA_M5:
        case DeviceTypes.EHCA_M5p:
        case DeviceTypes.EHCA_M6:
        case DeviceTypes.EHCA_M6_Radio3:
        case DeviceTypes.MinotelContact:
        case DeviceTypes.MinotelContactRadio3:
        case DeviceTypes.Aqua:
        case DeviceTypes.AquaMicro:
        case DeviceTypes.AquaMicroRadio3:
        case DeviceTypes.ISF:
        case DeviceTypes.SmokeDetector:
        case DeviceTypes.TemperatureSensor:
        case DeviceTypes.HumiditySensor:
          return "Minol";
        case DeviceTypes.RelayDevice:
          return "Relay";
        default:
          return "Unknown";
      }
    }
  }
}
