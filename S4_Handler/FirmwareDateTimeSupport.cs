// Decompiled with JetBrains decompiler
// Type: S4_Handler.FirmwareDateTimeSupport
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using S4_Handler.Functions;
using System;
using ZR_ClassLibrary;

#nullable disable
namespace S4_Handler
{
  internal static class FirmwareDateTimeSupport
  {
    internal static DateTime GetDateTimeFromMemoryBCD(DeviceMemory deviceMemory)
    {
      uint parameterAddress = deviceMemory.GetParameterAddress(S4_Params.sysDateTime.ToString());
      return FirmwareDateTimeSupport.ToDateTimeFromBCD(deviceMemory.GetData(parameterAddress, 12U));
    }

    internal static Decimal GetTimeZoneFromMemory(DeviceMemory deviceMemory)
    {
      uint parameterAddress = deviceMemory.GetParameterAddress(S4_Params.sysDateTime.ToString());
      return (Decimal) (sbyte) deviceMemory.GetData(parameterAddress + 12U, 1U)[0] / 4M;
    }

    internal static void SetTimeOffsetToMemoryBCD(
      DateTimeOffset timeOffset,
      DeviceMemory deviceMemory)
    {
      uint parameterAddress = deviceMemory.GetParameterAddress(S4_Params.sysDateTime.ToString());
      byte[] dateTimeOffsetBcd = FirmwareDateTimeSupport.ToFirmwareDateTimeOffsetBCD(timeOffset);
      deviceMemory.SetData(parameterAddress, dateTimeOffsetBcd);
    }

    internal static void SetTimeZoneToMemory(Decimal timeZone, DeviceMemory deviceMemory)
    {
      uint parameterAddress = deviceMemory.GetParameterAddress(S4_Params.sysDateTime.ToString());
      byte num = (byte) (sbyte) (timeZone * 4M);
      deviceMemory.SetData((uint) ((int) parameterAddress + 12), new byte[1]
      {
        num
      });
    }

    internal static DateTime GetDateTimeFromMemoryBCD(uint address, DeviceMemory deviceMemory)
    {
      return FirmwareDateTimeSupport.ToDateTimeFromBCD(deviceMemory.GetData(address, 12U));
    }

    internal static DateTimeOffset ToDateTimeOffsetBCD(byte[] data)
    {
      DateTime dateTimeFromBcd = FirmwareDateTimeSupport.ToDateTimeFromBCD(data);
      return data.Length > 12 ? new TimeZoneSupport(dateTimeFromBcd, data[12]).TimeZoneTime : new TimeZoneSupport(dateTimeFromBcd, (byte) 0).TimeZoneTime;
    }

    internal static DateTime? ToDateTimeFromFirmwareDateBCD(uint firmwareDateBCD)
    {
      try
      {
        uint day = (uint) (((int) firmwareDateBCD & 15) + (int) ((firmwareDateBCD & 48U) >> 4) * 10);
        uint month = ((firmwareDateBCD & 3840U) >> 8) + ((firmwareDateBCD & 4096U) >> 12) * 10U;
        return new DateTime?(new DateTime((int) ((firmwareDateBCD & 983040U) >> 16) + (int) ((firmwareDateBCD & 15728640U) >> 20) * 10 + 2000, (int) month, (int) day));
      }
      catch
      {
        return new DateTime?();
      }
    }

    internal static uint ToFirmwareDateBCD(DateTime dateTime)
    {
      return dateTime == DateTime.MinValue ? 0U : (uint) ((int) (byte) ((uint) (byte) (dateTime.Day % 10) | (uint) (byte) (dateTime.Day / 10 % 10) << 4) + ((int) (byte) ((uint) (byte) (dateTime.Month % 10) | (uint) (byte) (dateTime.Month / 10 % 10) << 4) << 8) + ((int) (byte) ((uint) (byte) (dateTime.Year % 10) | (uint) (byte) (dateTime.Year / 10 % 10) << 4) << 16) + 536870912);
    }

    internal static TimeSpan ToTimeSpanFromFirmwareTimeBCD(uint firmwareTimeBCD)
    {
      try
      {
        uint seconds = (uint) (((int) firmwareTimeBCD & 15) + (int) ((firmwareTimeBCD & 112U) >> 4) * 10);
        uint minutes = ((firmwareTimeBCD & 3840U) >> 8) + ((firmwareTimeBCD & 28672U) >> 12) * 10U;
        return new TimeSpan((int) (((firmwareTimeBCD & 983040U) >> 16) + ((firmwareTimeBCD & 3145728U) >> 20) * 10U), (int) minutes, (int) seconds);
      }
      catch
      {
        return new TimeSpan();
      }
    }

    internal static uint ToFirmwareTimeBCD(DateTime time)
    {
      if (time == DateTime.MinValue)
        return 0;
      uint bcdUint32_1 = Util.ConvertUnt32ToBcdUInt32((uint) time.Second);
      uint bcdUint32_2 = Util.ConvertUnt32ToBcdUInt32((uint) time.Minute);
      return (Util.ConvertUnt32ToBcdUInt32((uint) time.Hour) << 16) + (bcdUint32_2 << 8) + bcdUint32_1;
    }

    internal static DateTime ToDateTimeFromBCD(byte[] data)
    {
      if (data == null || data.Length < 12)
        throw new Exception("Illegal byte[] array for ToDateTime function");
      uint millisecond = (uint) (125.0 / 512.0 * (4095.0 - (double) BitConverter.ToUInt32(data, 0)));
      uint uint32_1 = BitConverter.ToUInt32(data, 4);
      uint uint32_2 = BitConverter.ToUInt32(data, 8);
      uint second = (uint) (((int) uint32_1 & 15) + (int) ((uint32_1 & 112U) >> 4) * 10);
      uint minute = ((uint32_1 & 3840U) >> 8) + ((uint32_1 & 28672U) >> 12) * 10U;
      uint hour = ((uint32_1 & 983040U) >> 16) + ((uint32_1 & 3145728U) >> 20) * 10U;
      uint day = (uint) (((int) uint32_2 & 15) + (int) ((uint32_2 & 48U) >> 4) * 10);
      uint month = ((uint32_2 & 3840U) >> 8) + ((uint32_2 & 4096U) >> 12) * 10U;
      uint year = (uint) ((int) ((uint32_2 & 983040U) >> 16) + (int) ((uint32_2 & 15728640U) >> 20) * 10 + 2000);
      try
      {
        return new DateTime((int) year, (int) month, (int) day, (int) hour, (int) minute, (int) second, (int) millisecond);
      }
      catch
      {
        return DateTime.MinValue;
      }
    }

    internal static byte[] ToFirmwareDateTimeBCD(DateTime dateTime)
    {
      byte[] firmwareDateTimeBcd = new byte[12];
      if (dateTime > DateTime.MinValue)
      {
        uint num = (uint) (4095 - (int) ((uint) (dateTime.Millisecond * 4096) / 1000U) & 4095);
        firmwareDateTimeBcd[0] = (byte) num;
        firmwareDateTimeBcd[1] = (byte) (num >> 8);
        firmwareDateTimeBcd[2] = (byte) (num >> 16);
        firmwareDateTimeBcd[3] = (byte) (num >> 24);
        firmwareDateTimeBcd[4] = (byte) ((uint) (byte) (dateTime.Second % 10) | (uint) (byte) (dateTime.Second / 10 % 10) << 4);
        firmwareDateTimeBcd[5] = (byte) ((uint) (byte) (dateTime.Minute % 10) | (uint) (byte) (dateTime.Minute / 10 % 10) << 4);
        firmwareDateTimeBcd[6] = (byte) ((uint) (byte) (dateTime.Hour % 10) | (uint) (byte) (dateTime.Hour / 10 % 10) << 4);
        firmwareDateTimeBcd[7] = (byte) 0;
        firmwareDateTimeBcd[8] = (byte) ((uint) (byte) (dateTime.Day % 10) | (uint) (byte) (dateTime.Day / 10 % 10) << 4);
        firmwareDateTimeBcd[9] = (byte) ((uint) (byte) (dateTime.Month % 10) | (uint) (byte) (dateTime.Month / 10 % 10) << 4);
        firmwareDateTimeBcd[10] = (byte) ((uint) (byte) (dateTime.Year % 10) | (uint) (byte) (dateTime.Year / 10 % 10) << 4);
        firmwareDateTimeBcd[11] = (byte) 32;
      }
      return firmwareDateTimeBcd;
    }

    internal static byte[] ToFirmwareDateTimeOffsetBCD(DateTimeOffset dateTimeOffset)
    {
      TimeZoneSupport timeZoneSupport = new TimeZoneSupport(dateTimeOffset);
      return FirmwareDateTimeSupport.ToFirmwareDateTimeBCD(timeZoneSupport.TimeZoneTime.DateTime, new Decimal?(timeZoneSupport.GetTimeZoneAsDecimal()));
    }

    internal static byte[] ToFirmwareDateTimeBCD(DateTime dateTime, Decimal? timeZone)
    {
      byte[] firmwareDateTimeBcd = FirmwareDateTimeSupport.ToFirmwareDateTimeBCD(dateTime);
      if (timeZone.HasValue)
      {
        Array.Resize<byte>(ref firmwareDateTimeBcd, 13);
        firmwareDateTimeBcd[12] = new TimeZoneSupport(dateTime, timeZone.Value).GetTimeZoneAsByte();
      }
      return firmwareDateTimeBcd;
    }
  }
}
