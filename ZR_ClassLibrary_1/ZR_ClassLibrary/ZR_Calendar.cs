// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.ZR_Calendar
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;

#nullable disable
namespace ZR_ClassLibrary
{
  public class ZR_Calendar
  {
    public const uint Cal_SecoundsPerDay = 86400;
    public const uint Cal_SecoundsPerMonth = 2629800;
    public const uint Cal_SecoundsPerYear = 31557600;
    private static uint[] MonthBeginTabel = new uint[26]
    {
      0U,
      31U,
      59U,
      90U,
      120U,
      151U,
      181U,
      212U,
      243U,
      273U,
      304U,
      334U,
      365U,
      0U,
      31U,
      60U,
      91U,
      121U,
      152U,
      182U,
      213U,
      244U,
      274U,
      305U,
      335U,
      366U
    };

    public static uint Cal_GetNow() => ZR_Calendar.Cal_GetMeterTime(DateTime.Now);

    public static uint Cal_GetMeterTime(DateTime TheTime)
    {
      DateTime dateTime = new DateTime(1980, 1, 1);
      return (uint) (TheTime - dateTime).TotalSeconds;
    }

    public static DateTime Cal_GetDateTime(uint TheTime)
    {
      return TheTime == uint.MaxValue ? DateTime.MinValue : new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Unspecified).Add(new TimeSpan(0, 0, (int) TheTime));
    }

    public static CalStruct Cal_Sec80ToStruct(uint Sec80)
    {
      CalTime time = ZR_Calendar.Cal_Sec80ToTime(Sec80);
      CalDate date = ZR_Calendar.Cal_Sec80ToDate(Sec80);
      return new CalStruct()
      {
        Secound = time.Secound,
        Minute = time.Minute,
        Hour = time.Hour,
        Day = date.Day,
        Month = date.Month,
        Year = date.Year
      };
    }

    public static uint Cal_StructToSec80(CalStruct DateTime)
    {
      CalTime Time = new CalTime();
      CalDate Date = new CalDate();
      Time.Secound = DateTime.Secound;
      Time.Minute = DateTime.Minute;
      Time.Hour = DateTime.Hour;
      Date.Day = DateTime.Day;
      Date.Month = DateTime.Month;
      Date.Year = DateTime.Year;
      return ZR_Calendar.Cal_TimeToSec80(Time) + ZR_Calendar.Cal_DateToSec80(Date);
    }

    public static CalTime Cal_Sec80ToTime(uint Sec80)
    {
      return new CalTime()
      {
        Secound = Sec80 % 60U,
        Minute = Sec80 % 3600U / 60U,
        Hour = Sec80 % 86400U / 3600U
      };
    }

    public static uint Cal_TimeToSec80(CalTime Time)
    {
      if (Time.Secound > 59U)
        Time.Secound = 59U;
      if (Time.Minute > 59U)
        Time.Minute = 59U;
      if (Time.Hour > 23U)
        Time.Hour = 23U;
      return Time.Secound + Time.Minute * 60U + Time.Hour * 3600U;
    }

    public static CalDate Cal_Sec80ToDate(uint Sec80)
    {
      CalDate date = new CalDate();
      uint num1 = Sec80 / 86400U;
      uint num2 = (num1 << 2) / 1461U;
      date.Year = 1980U + num2;
      uint num3 = num1 - (num2 * 365U + (num2 + 3U >> 2));
      uint index = num2 % 4U <= 0U ? 13U : 0U;
      while (num3 >= ZR_Calendar.MonthBeginTabel[(int) index + 1])
        ++index;
      date.Month = index <= 12U ? index + 1U : index - 12U;
      date.Day = (uint) ((int) num3 - (int) ZR_Calendar.MonthBeginTabel[(int) index] + 1);
      return date;
    }

    public static uint Cal_DateToSec80(CalDate Date)
    {
      while (Date.Year < 1980U)
        Date.Year += 100U;
      while (Date.Year > 2099U)
        Date.Year -= 100U;
      if (Date.Month < 1U)
      {
        Date.Month = 12U;
        --Date.Year;
      }
      if (Date.Month > 12U)
        Date.Month = 12U;
      if (Date.Day < 1U)
        Date.Day = 1U;
      uint num1 = Date.Year - 1980U;
      uint num2 = num1 % 4U <= 0U ? 13U : 0U;
      uint num3 = ZR_Calendar.MonthBeginTabel[(int) num2 + (int) Date.Month] - ZR_Calendar.MonthBeginTabel[(int) num2 + (int) Date.Month - 1];
      if (Date.Day > num3)
        Date.Day = num3;
      uint index = (uint) ((int) num2 + (int) Date.Month - 1);
      return ((uint) ((int) ZR_Calendar.MonthBeginTabel[(int) index] + (int) Date.Day - 1) + (num1 * 365U + (num1 + 3U >> 2))) * 86400U;
    }

    public static byte Cal_GetWeekdayBit(uint TimePoint)
    {
      return (byte) (1U << (int) ((TimePoint / 86400U + 1U) % 7U));
    }

    public static uint Cal_SetTimeToNextYear(uint Sec80)
    {
      bool flag = true;
      CalDate date = ZR_Calendar.Cal_Sec80ToDate(Sec80);
      while (true)
      {
        if (flag)
        {
          if (Sec80 > ZR_Calendar.Cal_GetNow())
          {
            --date.Year;
          }
          else
          {
            flag = false;
            continue;
          }
        }
        else if (Sec80 < ZR_Calendar.Cal_GetNow())
          ++date.Year;
        else
          break;
        Sec80 = ZR_Calendar.Cal_DateToSec80(date) + Sec80;
      }
      return Sec80;
    }

    public static DateTime Cal_GetWintertime(DateTime AktTime, int WinterUTCVerschiebungInMinuten)
    {
      TimeSpan utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(AktTime);
      double num1 = (double) (utcOffset.Hours * 60 + utcOffset.Minutes);
      double num2 = (double) WinterUTCVerschiebungInMinuten - num1;
      return AktTime.AddMinutes(num2);
    }

    public static int Cal_UTCVerschiebungInMinuten(int Stunden, ushort Minuten)
    {
      return Stunden >= 0 ? Stunden * 60 + (int) Minuten : Stunden * 60 - (int) Minuten;
    }

    public static string Cal_GetIntervalInfo(uint Sec80)
    {
      switch (Sec80)
      {
        case 3600:
          return "one hour";
        case 86400:
          return "one day";
        case 2629800:
          return "one month";
        case 31557600:
          return "one year";
        default:
          return Sec80.ToString() + " sec";
      }
    }
  }
}
