// Decompiled with JetBrains decompiler
// Type: InTheHand.Win32.SYSTEMTIME
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;

#nullable disable
namespace InTheHand.Win32
{
  internal struct SYSTEMTIME
  {
    private ushort year;
    private short month;
    private short dayOfWeek;
    private short day;
    private short hour;
    private short minute;
    private short second;
    private short millisecond;

    public static SYSTEMTIME FromByteArray(byte[] array, int offset)
    {
      return new SYSTEMTIME()
      {
        year = BitConverter.ToUInt16(array, offset),
        month = BitConverter.ToInt16(array, offset + 2),
        day = BitConverter.ToInt16(array, offset + 6),
        hour = BitConverter.ToInt16(array, offset + 8),
        minute = BitConverter.ToInt16(array, offset + 10),
        second = BitConverter.ToInt16(array, offset + 12)
      };
    }

    public static SYSTEMTIME FromDateTime(DateTime dt)
    {
      return new SYSTEMTIME()
      {
        year = (ushort) dt.Year,
        month = (short) dt.Month,
        dayOfWeek = (short) dt.DayOfWeek,
        day = (short) dt.Day,
        hour = (short) dt.Hour,
        minute = (short) dt.Minute,
        second = (short) dt.Second,
        millisecond = (short) dt.Millisecond
      };
    }

    public DateTime ToDateTime(DateTimeKind kind)
    {
      return this.year == (ushort) 0 && this.month == (short) 0 && this.day == (short) 0 && this.hour == (short) 0 && this.minute == (short) 0 && this.second == (short) 0 ? DateTime.MinValue : new DateTime((int) this.year, (int) this.month, (int) this.day, (int) this.hour, (int) this.minute, (int) this.second, (int) this.millisecond, kind);
    }
  }
}
