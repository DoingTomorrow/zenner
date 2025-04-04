// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.LogAnalyticsMonitorImpl
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  internal class LogAnalyticsMonitorImpl : ILogAnalyticsMonitor
  {
    private readonly ILogAnalyticsMonitor m_logger;

    public LogAnalyticsMonitorImpl(ILogAnalyticsMonitor logger) => this.m_logger = logger;

    public void LogMessage(string message) => this.m_logger.LogMessage(message);

    public void LogMessageF(string format, params object[] args)
    {
      this.m_logger.LogMessage(string.Format(format, args));
    }

    public void LogError(string errorMessage) => this.m_logger.LogError(errorMessage);

    public void LogErrorF(string format, params object[] args)
    {
      this.m_logger.LogError(string.Format(format, args));
    }
  }
}
