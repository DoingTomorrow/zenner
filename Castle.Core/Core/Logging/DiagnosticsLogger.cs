// Decompiled with JetBrains decompiler
// Type: Castle.Core.Logging.DiagnosticsLogger
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Diagnostics;
using System.Globalization;

#nullable disable
namespace Castle.Core.Logging
{
  [Serializable]
  public class DiagnosticsLogger : LevelFilteredLogger, IDisposable
  {
    [NonSerialized]
    private EventLog eventLog;

    public DiagnosticsLogger(string logName)
      : this(logName, "default")
    {
    }

    public DiagnosticsLogger(string logName, string source)
      : base(LoggerLevel.Debug)
    {
      if (!EventLog.SourceExists(source))
        EventLog.CreateEventSource(source, logName);
      this.eventLog = new EventLog(logName);
      this.eventLog.Source = source;
    }

    public DiagnosticsLogger(string logName, string machineName, string source)
    {
      if (!EventLog.SourceExists(source, machineName))
        EventLog.CreateEventSource(new EventSourceCreationData(source, logName)
        {
          MachineName = machineName
        });
      this.eventLog = new EventLog(logName, machineName, source);
    }

    ~DiagnosticsLogger() => this.Dispose(false);

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing || this.eventLog == null)
        return;
      this.eventLog.Close();
      this.eventLog = (EventLog) null;
    }

    public override ILogger CreateChildLogger(string loggerName)
    {
      return (ILogger) new DiagnosticsLogger(this.eventLog.Log, this.eventLog.MachineName, this.eventLog.Source);
    }

    protected override void Log(
      LoggerLevel loggerLevel,
      string loggerName,
      string message,
      Exception exception)
    {
      if (this.eventLog == null)
        return;
      EventLogEntryType type = DiagnosticsLogger.TranslateLevel(loggerLevel);
      string message1;
      if (exception == null)
        message1 = string.Format((IFormatProvider) CultureInfo.CurrentCulture, "[{0}] '{1}' message: {2}", (object) loggerLevel.ToString(), (object) loggerName, (object) message);
      else
        message1 = string.Format((IFormatProvider) CultureInfo.CurrentCulture, "[{0}] '{1}' message: {2} exception: {3} {4} {5}", (object) loggerLevel.ToString(), (object) loggerName, (object) message, (object) exception.GetType(), (object) exception.Message, (object) exception.StackTrace);
      this.eventLog.WriteEntry(message1, type);
    }

    private static EventLogEntryType TranslateLevel(LoggerLevel level)
    {
      switch (level)
      {
        case LoggerLevel.Fatal:
        case LoggerLevel.Error:
          return EventLogEntryType.Error;
        case LoggerLevel.Warn:
          return EventLogEntryType.Warning;
        default:
          return EventLogEntryType.Information;
      }
    }
  }
}
