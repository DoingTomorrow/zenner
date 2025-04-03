// Decompiled with JetBrains decompiler
// Type: HandlerLib.CalendarBase2000
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class CalendarBase2000
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

    public static uint Cal_GetNow() => CalendarBase2000.Cal_GetMeterTime(DateTime.Now);

    public static uint Cal_GetMeterTime(DateTime TheTime)
    {
      DateTime dateTime = new DateTime(2000, 1, 1);
      return (uint) (TheTime - dateTime).TotalSeconds;
    }

    public static DateTime Cal_GetDateTime(uint TheTime)
    {
      return new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Unspecified).Add(new TimeSpan(0, 0, (int) TheTime));
    }

    public static CalStruct Cal_Sec2000ToStruct(uint Sec2000)
    {
      CalTime time = CalendarBase2000.Cal_Sec2000ToTime(Sec2000);
      CalDate date = CalendarBase2000.Cal_Sec2000ToDate(Sec2000);
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

    public static uint Cal_StructToSec2000(CalStruct DateTime)
    {
      CalTime Time = new CalTime();
      CalDate Date = new CalDate();
      Time.Secound = DateTime.Secound;
      Time.Minute = DateTime.Minute;
      Time.Hour = DateTime.Hour;
      Date.Day = DateTime.Day;
      Date.Month = DateTime.Month;
      Date.Year = DateTime.Year;
      return CalendarBase2000.Cal_TimeToSec2000(Time) + CalendarBase2000.Cal_DateToSec2000(Date);
    }

    public static CalTime Cal_Sec2000ToTime(uint Sec2000)
    {
      return new CalTime()
      {
        Secound = Sec2000 % 60U,
        Minute = Sec2000 % 3600U / 60U,
        Hour = Sec2000 % 86400U / 3600U
      };
    }

    public static uint Cal_TimeToSec2000(CalTime Time)
    {
      if (Time.Secound > 59U)
        Time.Secound = 59U;
      if (Time.Minute > 59U)
        Time.Minute = 59U;
      if (Time.Hour > 23U)
        Time.Hour = 23U;
      return Time.Secound + Time.Minute * 60U + Time.Hour * 3600U;
    }

    public static CalDate Cal_Sec2000ToDate(uint Sec2000)
    {
      CalDate date = new CalDate();
      uint num1 = Sec2000 / 86400U;
      uint num2 = (num1 << 2) / 1461U;
      date.Year = 2000U + num2;
      uint num3 = num1 - (num2 * 365U + (num2 + 3U >> 2));
      uint index = num2 % 4U <= 0U ? 13U : 0U;
      while (num3 >= CalendarBase2000.MonthBeginTabel[(int) index + 1])
        ++index;
      date.Month = index <= 12U ? index + 1U : index - 12U;
      date.Day = (uint) ((int) num3 - (int) CalendarBase2000.MonthBeginTabel[(int) index] + 1);
      return date;
    }

    public static uint Cal_DateToSec2000(CalDate Date)
    {
      while (Date.Year < 2000U)
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
      uint num1 = Date.Year - 2000U;
      uint num2 = num1 % 4U <= 0U ? 13U : 0U;
      uint num3 = CalendarBase2000.MonthBeginTabel[(int) num2 + (int) Date.Month] - CalendarBase2000.MonthBeginTabel[(int) num2 + (int) Date.Month - 1];
      if (Date.Day > num3)
        Date.Day = num3;
      uint index = (uint) ((int) num2 + (int) Date.Month - 1);
      return ((uint) ((int) CalendarBase2000.MonthBeginTabel[(int) index] + (int) Date.Day - 1) + (num1 * 365U + (num1 + 3U >> 2))) * 86400U;
    }

    public static byte Cal_GetWeekdayBit(uint TimePoint)
    {
      return (byte) (1U << (int) ((TimePoint / 86400U + 5U) % 7U));
    }

    public static uint Cal_SetTimeToNextYear(uint Sec2000)
    {
      bool flag = true;
      CalDate date = CalendarBase2000.Cal_Sec2000ToDate(Sec2000);
      while (true)
      {
        if (flag)
        {
          if (Sec2000 > CalendarBase2000.Cal_GetNow())
          {
            --date.Year;
          }
          else
          {
            flag = false;
            continue;
          }
        }
        else if (Sec2000 < CalendarBase2000.Cal_GetNow())
          ++date.Year;
        else
          break;
        Sec2000 = CalendarBase2000.Cal_DateToSec2000(date) + Sec2000;
      }
      return Sec2000;
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

    public static string Cal_GetIntervalInfo(uint Sec2000)
    {
      switch (Sec2000)
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
          return Sec2000.ToString() + " sec";
      }
    }
  }
}
