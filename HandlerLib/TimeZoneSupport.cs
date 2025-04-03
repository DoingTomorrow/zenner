// Decompiled with JetBrains decompiler
// Type: HandlerLib.TimeZoneSupport
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;

#nullable disable
namespace HandlerLib
{
  public class TimeZoneSupport
  {
    public DateTimeOffset TimeZoneTime;

    public TimeZoneSupport(DateTimeOffset timeZoneTime) => this.TimeZoneTime = timeZoneTime;

    public TimeZoneSupport(DateTime theTime, byte timeZoneByte)
    {
      int num = (int) (sbyte) timeZoneByte;
      int hours = num / 4;
      int minutes = (num - hours * 4) * 15;
      TimeSpan offset = new TimeSpan(hours, minutes, 0);
      this.TimeZoneTime = new DateTimeOffset(theTime, offset);
    }

    public TimeZoneSupport(DateTime theTime, Decimal timeZone)
    {
      int hours = (int) timeZone;
      int minutes = (int) ((timeZone - (Decimal) hours) * 60M);
      TimeSpan offset = new TimeSpan(hours, minutes, 0);
      this.TimeZoneTime = new DateTimeOffset(theTime, offset);
    }

    public TimeZoneSupport(Decimal timeZone)
    {
      DateTime dateTime = new DateTime(DateTime.Now.ToUniversalTime().Ticks);
      int hours = (int) timeZone;
      int minutes = (int) ((timeZone - (Decimal) hours) * 60M);
      TimeSpan offset = new TimeSpan(hours, minutes, 0);
      this.TimeZoneTime = new DateTimeOffset(dateTime + offset, offset);
    }

    public byte GetTimeZoneAsByte() => (byte) (sbyte) (this.TimeZoneTime.Offset.TotalHours * 4.0);

    public Decimal GetTimeZoneAsDecimal() => (Decimal) this.TimeZoneTime.Offset.TotalHours;

    public override string ToString() => this.TimeZoneTime.ToString();
  }
}
