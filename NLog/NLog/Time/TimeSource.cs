// Decompiled with JetBrains decompiler
// Type: NLog.Time.TimeSource
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;

#nullable disable
namespace NLog.Time
{
  [NLogConfigurationItem]
  public abstract class TimeSource
  {
    private static TimeSource currentSource = (TimeSource) new FastLocalTimeSource();

    public abstract DateTime Time { get; }

    public static TimeSource Current
    {
      get => TimeSource.currentSource;
      set => TimeSource.currentSource = value;
    }

    public override string ToString()
    {
      TimeSourceAttribute customAttribute = this.GetType().GetCustomAttribute<TimeSourceAttribute>();
      return customAttribute != null ? customAttribute.Name + " (time source)" : this.GetType().Name;
    }

    public abstract DateTime FromSystemTime(DateTime systemTime);
  }
}
