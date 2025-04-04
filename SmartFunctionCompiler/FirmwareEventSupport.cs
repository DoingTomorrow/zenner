// Decompiled with JetBrains decompiler
// Type: SmartFunctionCompiler.FirmwareEventSupport
// Assembly: SmartFunctionCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: E49EBEEE-4E03-4F25-A9DE-0F245CFB9A90
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmartFunctionCompiler.exe

using HandlerLib;
using System;

#nullable disable
namespace SmartFunctionCompiler
{
  public static class FirmwareEventSupport
  {
    private const uint yearSeconds = 31557600;
    public static uint[] EventTimeSeconds = new uint[19]
    {
      0U,
      60U,
      300U,
      600U,
      900U,
      1800U,
      3600U,
      10800U,
      21600U,
      43200U,
      86400U,
      259200U,
      604800U,
      1209600U,
      1314900U,
      2629800U,
      7889400U,
      15778800U,
      31557600U
    };

    public static bool IsTimeEvent(FirmwareEvents theEvent)
    {
      return theEvent >= FirmwareEvents.Minute || theEvent <= FirmwareEvents.Year;
    }

    public static bool IsLinearTimeEvent(FirmwareEvents theEvent)
    {
      return theEvent >= FirmwareEvents.Minute || theEvent <= FirmwareEvents.Weeks2;
    }

    public static uint GetEventTimeSeconds(FirmwareEvents theEvent)
    {
      return FirmwareEventSupport.IsTimeEvent(theEvent) ? FirmwareEventSupport.EventTimeSeconds[(int) theEvent] : 0U;
    }

    public static uint GetNextEventCycleTime2000(DateTime referenceTime, FirmwareEvents theEvent)
    {
      uint meterTime = CalendarBase2000.Cal_GetMeterTime(referenceTime);
      if (FirmwareEventSupport.IsLinearTimeEvent(theEvent))
      {
        uint eventTimeSeconds = FirmwareEventSupport.GetEventTimeSeconds(theEvent);
        return meterTime / eventTimeSeconds * eventTimeSeconds + eventTimeSeconds;
      }
      switch (theEvent)
      {
        case FirmwareEvents.HalfMonth:
          return referenceTime.Day < 16 ? CalendarBase2000.Cal_GetMeterTime(new DateTime(referenceTime.Year, referenceTime.Month, 1).AddMonths(1)) : CalendarBase2000.Cal_GetMeterTime(new DateTime(referenceTime.Year, referenceTime.Month, 16));
        case FirmwareEvents.Month:
          return CalendarBase2000.Cal_GetMeterTime(new DateTime(referenceTime.Year, referenceTime.Month, 1).AddMonths(1));
        case FirmwareEvents.Month3:
          DateTime TheTime1 = new DateTime(referenceTime.Year, referenceTime.Month, 1);
          TheTime1 = TheTime1.AddMonths(1);
          while ((TheTime1.Month - 1) % 3 != 0)
            TheTime1 = TheTime1.AddMonths(1);
          return CalendarBase2000.Cal_GetMeterTime(TheTime1);
        case FirmwareEvents.HalfYear:
          DateTime TheTime2 = new DateTime(referenceTime.Year, referenceTime.Month, 1);
          TheTime2 = TheTime2.AddMonths(1);
          while ((TheTime2.Month - 1) % 6 != 0)
            TheTime2 = TheTime2.AddMonths(1);
          return CalendarBase2000.Cal_GetMeterTime(TheTime2);
        case FirmwareEvents.Year:
          return CalendarBase2000.Cal_GetMeterTime(new DateTime(referenceTime.Year, 1, 1).AddYears(1));
        default:
          return 0;
      }
    }
  }
}
