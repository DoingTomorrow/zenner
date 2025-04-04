// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.TimingScope
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  public class TimingScope : IDisposable
  {
    private readonly string m_featureName;
    private readonly IAnalyticsMonitor m_monitor;
    private bool m_scopeEnded;
    private readonly object m_lock = new object();

    public string FeatureName => this.m_featureName;

    internal TimingScope(string featureName, IAnalyticsMonitor monitor)
    {
      this.m_featureName = featureName;
      this.m_monitor = monitor;
    }

    public void Complete()
    {
      if (this.m_scopeEnded)
        return;
      lock (this.m_lock)
      {
        if (this.m_scopeEnded)
          return;
        this.m_scopeEnded = true;
      }
      if (this.m_monitor == null)
        return;
      this.m_monitor.TrackFeatureStop(this.m_featureName);
    }

    public void Cancel()
    {
      if (this.m_scopeEnded)
        return;
      lock (this.m_lock)
      {
        if (this.m_scopeEnded)
          return;
        this.m_scopeEnded = true;
      }
      if (this.m_monitor == null)
        return;
      this.m_monitor.TrackFeatureCancel(this.m_featureName);
    }

    public void Dispose() => this.Cancel();
  }
}
