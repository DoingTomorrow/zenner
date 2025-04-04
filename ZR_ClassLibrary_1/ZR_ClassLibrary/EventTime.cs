// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.EventTime
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;

#nullable disable
namespace ZR_ClassLibrary
{
  public class EventTime
  {
    public string GetEventTime(long Ticks)
    {
      return new DateTime(Ticks).ToString("dd.MM.yy HH:mm:ss.ffff");
    }

    public string GetEventTimeDifferenc(long DifferenceTicks)
    {
      string eventTimeDifferenc = this.GetTimeDifferenc(DifferenceTicks).ToString("d06");
      if (eventTimeDifferenc.Length > 6)
        eventTimeDifferenc = "..new.";
      return eventTimeDifferenc;
    }

    public long GetTimeDifferenc(long DifferenceTicks) => DifferenceTicks / 10000L;

    public long GetEndTicks(long WaitMilliSecounds)
    {
      return DateTime.Now.Ticks + WaitMilliSecounds * 10000L;
    }

    public long GetTicksPlusTime(long FromTicks, long AdditionalMilliSecounds)
    {
      return FromTicks + AdditionalMilliSecounds * 10000L;
    }

    public long GetTimeTicks() => DateTime.Now.Ticks;
  }
}
