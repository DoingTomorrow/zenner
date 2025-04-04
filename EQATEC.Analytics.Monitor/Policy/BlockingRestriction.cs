// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Policy.BlockingRestriction
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;

#nullable disable
namespace EQATEC.Analytics.Monitor.Policy
{
  internal class BlockingRestriction : IEquatable<BlockingRestriction>
  {
    internal BlockingRestriction() => this.Clear();

    internal void Clear()
    {
      this.BlockingStart = TimeSpan.MinValue;
      this.BlockingUntil = TimeSpan.MinValue;
      this.BlockingType = BlockingRestriction.BlockingTypes.Unknown;
    }

    internal TimeSpan BlockingStart { get; set; }

    internal TimeSpan BlockingUntil { get; set; }

    internal BlockingRestriction.BlockingTypes BlockingType { get; set; }

    internal bool IsBlocking(TimeSpan uptime)
    {
      if (this.BlockingStart == TimeSpan.MinValue && this.BlockingUntil == TimeSpan.MinValue)
        return false;
      if (this.BlockingStart <= uptime && uptime <= this.BlockingUntil)
        return true;
      this.Clear();
      return false;
    }

    internal void SetBlockingTime(
      TimeSpan uptime,
      TimeSpan blockingTime,
      BlockingRestriction.BlockingTypes type)
    {
      this.BlockingStart = uptime;
      this.BlockingType = type;
      TimeSpan timeSpan = new TimeSpan(Math.Min(TimeSpan.FromDays(14.0).Ticks, blockingTime.Ticks));
      this.BlockingUntil = uptime + timeSpan;
    }

    public bool Equals(BlockingRestriction other)
    {
      return other != null && this.BlockingStart == other.BlockingStart && this.BlockingUntil == other.BlockingUntil && this.BlockingType == other.BlockingType;
    }

    internal string GetBlockingDescription()
    {
      switch (this.BlockingType)
      {
        case BlockingRestriction.BlockingTypes.Unknown:
          return "Unknown blocking reason";
        case BlockingRestriction.BlockingTypes.Subscription:
          return "Subscription has expired";
        case BlockingRestriction.BlockingTypes.SamplesSelection:
          return "Monitor not selected for sampling";
        case BlockingRestriction.BlockingTypes.ByUser:
          return "Product owner has blocked";
        case BlockingRestriction.BlockingTypes.Other:
          return "Unspecified blocking reason";
        default:
          return "Unknown blocking reason " + (object) (int) this.BlockingType;
      }
    }

    public BlockingRestriction Copy()
    {
      return new BlockingRestriction()
      {
        BlockingStart = this.BlockingStart,
        BlockingType = this.BlockingType,
        BlockingUntil = this.BlockingUntil
      };
    }

    internal enum BlockingTypes
    {
      Unknown,
      Subscription,
      SamplesSelection,
      ByUser,
      Other,
    }
  }
}
