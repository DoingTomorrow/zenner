// Decompiled with JetBrains decompiler
// Type: Castle.Core.Logging.TraceLogger
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Castle.Core.Logging
{
  public class TraceLogger : LevelFilteredLogger
  {
    private static readonly Dictionary<string, TraceSource> cache = new Dictionary<string, TraceSource>();
    private TraceSource traceSource;

    public TraceLogger(string name)
      : base(name)
    {
      this.Initialize();
      this.Level = TraceLogger.MapLoggerLevel(this.traceSource.Switch.Level);
    }

    public TraceLogger(string name, LoggerLevel level)
      : base(name, level)
    {
      this.Initialize();
      this.Level = TraceLogger.MapLoggerLevel(this.traceSource.Switch.Level);
    }

    public override ILogger CreateChildLogger(string loggerName)
    {
      return this.InternalCreateChildLogger(loggerName);
    }

    private ILogger InternalCreateChildLogger(string loggerName)
    {
      return (ILogger) new TraceLogger(this.Name + "." + loggerName, this.Level);
    }

    protected override void Log(
      LoggerLevel loggerLevel,
      string loggerName,
      string message,
      Exception exception)
    {
      if (exception == null)
        this.traceSource.TraceEvent(TraceLogger.MapTraceEventType(loggerLevel), 0, message);
      else
        this.traceSource.TraceData(TraceLogger.MapTraceEventType(loggerLevel), 0, (object) message, (object) exception);
    }

    private void Initialize()
    {
      lock (TraceLogger.cache)
      {
        if (TraceLogger.cache.TryGetValue(this.Name, out this.traceSource))
          return;
        SourceLevels defaultLevel = TraceLogger.MapSourceLevels(this.Level);
        this.traceSource = new TraceSource(this.Name, defaultLevel);
        if (TraceLogger.IsSourceConfigured(this.traceSource))
        {
          TraceLogger.cache.Add(this.Name, this.traceSource);
        }
        else
        {
          TraceSource traceSource = new TraceSource("Default", defaultLevel);
          for (string name = TraceLogger.ShortenName(this.Name); !string.IsNullOrEmpty(name); name = TraceLogger.ShortenName(name))
          {
            TraceSource source = new TraceSource(name, defaultLevel);
            if (TraceLogger.IsSourceConfigured(source))
            {
              traceSource = source;
              break;
            }
          }
          this.traceSource.Switch = traceSource.Switch;
          this.traceSource.Listeners.Clear();
          foreach (TraceListener listener in traceSource.Listeners)
            this.traceSource.Listeners.Add(listener);
          TraceLogger.cache.Add(this.Name, this.traceSource);
        }
      }
    }

    private static string ShortenName(string name)
    {
      int length = name.LastIndexOf('.');
      return length != -1 ? name.Substring(0, length) : (string) null;
    }

    private static bool IsSourceConfigured(TraceSource source)
    {
      return source.Listeners.Count != 1 || !(source.Listeners[0] is DefaultTraceListener) || !(source.Listeners[0].Name == "Default");
    }

    private static LoggerLevel MapLoggerLevel(SourceLevels level)
    {
      switch (level)
      {
        case SourceLevels.All:
          return LoggerLevel.Debug;
        case SourceLevels.Critical:
          return LoggerLevel.Fatal;
        case SourceLevels.Error:
          return LoggerLevel.Error;
        case SourceLevels.Warning:
          return LoggerLevel.Warn;
        case SourceLevels.Information:
          return LoggerLevel.Info;
        case SourceLevels.Verbose:
          return LoggerLevel.Debug;
        default:
          return LoggerLevel.Off;
      }
    }

    private static SourceLevels MapSourceLevels(LoggerLevel level)
    {
      switch (level)
      {
        case LoggerLevel.Fatal:
          return SourceLevels.Critical;
        case LoggerLevel.Error:
          return SourceLevels.Error;
        case LoggerLevel.Warn:
          return SourceLevels.Warning;
        case LoggerLevel.Info:
          return SourceLevels.Information;
        case LoggerLevel.Debug:
          return SourceLevels.Verbose;
        default:
          return SourceLevels.Off;
      }
    }

    private static TraceEventType MapTraceEventType(LoggerLevel level)
    {
      switch (level)
      {
        case LoggerLevel.Fatal:
          return TraceEventType.Critical;
        case LoggerLevel.Error:
          return TraceEventType.Error;
        case LoggerLevel.Warn:
          return TraceEventType.Warning;
        case LoggerLevel.Info:
          return TraceEventType.Information;
        case LoggerLevel.Debug:
          return TraceEventType.Verbose;
        default:
          return TraceEventType.Verbose;
      }
    }
  }
}
