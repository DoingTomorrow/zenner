// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Model.Feature
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;

#nullable disable
namespace EQATEC.Analytics.Monitor.Model
{
  internal class Feature : IEquatable<Feature>
  {
    public uint SessionHit { get; set; }

    public uint SyncedHits { get; set; }

    public TimeSpan ActiveStartTime { get; set; }

    public Feature()
    {
      this.SessionHit = 0U;
      this.SyncedHits = 0U;
      this.ActiveStartTime = TimeSpan.MinValue;
    }

    public Feature(uint sessionHit)
      : this()
    {
      this.SessionHit = sessionHit;
      this.ActiveStartTime = TimeSpan.MinValue;
    }

    public void StartTiming(TimeSpan startTime)
    {
      if (this.IsActive)
        return;
      this.ActiveStartTime = startTime;
    }

    public TimeSpan StopTiming(TimeSpan stopTime)
    {
      TimeSpan timeSpan = TimeSpan.Zero;
      if (this.IsActive)
      {
        timeSpan = stopTime.Subtract(this.ActiveStartTime);
        this.ActiveStartTime = TimeSpan.MinValue;
      }
      return timeSpan;
    }

    public bool IsActive => this.ActiveStartTime != TimeSpan.MinValue;

    public bool Equals(Feature other)
    {
      return other != null && (int) this.SessionHit == (int) other.SessionHit && this.ActiveStartTime == other.ActiveStartTime;
    }

    public Feature Copy()
    {
      return new Feature()
      {
        ActiveStartTime = this.ActiveStartTime,
        SessionHit = this.SessionHit,
        SyncedHits = this.SyncedHits
      };
    }
  }
}
