// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Model.FeatureValue
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;

#nullable disable
namespace EQATEC.Analytics.Monitor.Model
{
  internal class FeatureValue : IEquatable<FeatureValue>
  {
    internal string Name { get; private set; }

    internal FeatureValueType Type { get; private set; }

    internal long Value { get; private set; }

    internal long TimeStamp { get; private set; }

    public FeatureValue(string name, FeatureValueType type, long value, TimeSpan timespan)
    {
      this.Name = name;
      this.Type = type;
      this.Value = value;
      this.TimeStamp = timespan.Ticks;
    }

    public bool Equals(FeatureValue other)
    {
      return other.TimeStamp == this.TimeStamp && other.Value == this.Value && other.Type == this.Type && other.Name == this.Name;
    }

    public FeatureValue Copy()
    {
      return new FeatureValue(this.Name, this.Type, this.Value, new TimeSpan(this.TimeStamp));
    }
  }
}
