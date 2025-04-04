// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.AnalyticsMonitorStatus
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using EQATEC.Analytics.Monitor.Policy;
using System;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  public class AnalyticsMonitorStatus
  {
    private readonly AnalyticsMonitor m_monitor;
    private readonly MonitorPolicy m_policy;
    private readonly ArgumentChecker m_argsChecker;
    private readonly LogAnalyticsMonitorImpl m_log;
    private ConnectivityStatus m_connectivity;
    private readonly object m_lock = new object();

    internal AnalyticsMonitorStatus(
      AnalyticsMonitor monitor,
      ILogAnalyticsMonitor log,
      MonitorPolicy policy,
      ArgumentChecker argsChecker)
    {
      this.Capabilities = new AnalyticsMonitorCapabilities(policy);
      this.m_monitor = monitor;
      this.m_policy = policy;
      this.m_argsChecker = argsChecker;
      this.m_log = new LogAnalyticsMonitorImpl(log);
    }

    public AnalyticsMonitorCapabilities Capabilities { get; private set; }

    public bool IsStarted => this.m_monitor.IsStarted;

    public TimeSpan RunTime
    {
      get => !this.IsStarted ? TimeSpan.Zero : Timing.Uptime - this.m_monitor.UptimeForStart;
    }

    public string CookieId => this.m_policy.Info.Cookie;

    public ConnectivityStatus Connectivity => this.m_connectivity;
  }
}
