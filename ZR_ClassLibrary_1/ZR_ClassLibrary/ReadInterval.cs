// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.ReadInterval
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class ReadInterval
  {
    public DateTime StartTime { get; set; }

    public int Repetitions { get; set; }

    public TimeSpan Interval { get; set; }

    public string ReadIntervalString
    {
      get
      {
        return string.Format("{0:u}|{1}|{2}", (object) this.StartTime, (object) this.Repetitions, (object) this.Interval);
      }
    }

    public ReadInterval(int minutes)
      : this(minutes, DateTime.Now)
    {
    }

    public ReadInterval(int minutes, DateTime startDate)
      : this(new TimeSpan(0, minutes, 0), startDate)
    {
    }

    public ReadInterval(TimeSpan interval, DateTime startDate)
    {
      this.StartTime = startDate;
      this.Interval = interval;
    }

    public static ReadInterval TryParse(string readIntervalString) => (ReadInterval) null;

    public object GetNextReadDateTime(DateTime now) => throw new NotImplementedException();
  }
}
