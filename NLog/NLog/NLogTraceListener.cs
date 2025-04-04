﻿// Decompiled with JetBrains decompiler
// Type: NLog.NLogTraceListener
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Internal;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Xml;

#nullable disable
namespace NLog
{
  public class NLogTraceListener : TraceListener
  {
    private LogFactory _logFactory;
    private LogLevel _defaultLogLevel = LogLevel.Debug;
    private bool _attributesLoaded;
    private bool _autoLoggerName;
    private LogLevel _forceLogLevel;
    private bool _disableFlush;

    public LogFactory LogFactory
    {
      get
      {
        this.InitAttributes();
        return this._logFactory;
      }
      set
      {
        this._attributesLoaded = true;
        this._logFactory = value;
      }
    }

    public LogLevel DefaultLogLevel
    {
      get
      {
        this.InitAttributes();
        return this._defaultLogLevel;
      }
      set
      {
        this._attributesLoaded = true;
        this._defaultLogLevel = value;
      }
    }

    public LogLevel ForceLogLevel
    {
      get
      {
        this.InitAttributes();
        return this._forceLogLevel;
      }
      set
      {
        this._attributesLoaded = true;
        this._forceLogLevel = value;
      }
    }

    public bool DisableFlush
    {
      get
      {
        this.InitAttributes();
        return this._disableFlush;
      }
      set
      {
        this._attributesLoaded = true;
        this._disableFlush = value;
      }
    }

    public override bool IsThreadSafe => true;

    public bool AutoLoggerName
    {
      get
      {
        this.InitAttributes();
        return this._autoLoggerName;
      }
      set
      {
        this._attributesLoaded = true;
        this._autoLoggerName = value;
      }
    }

    public override void Write(string message)
    {
      this.ProcessLogEventInfo(this.DefaultLogLevel, (string) null, message, (object[]) null, new int?(), new TraceEventType?(TraceEventType.Resume), new Guid?());
    }

    public override void WriteLine(string message)
    {
      this.ProcessLogEventInfo(this.DefaultLogLevel, (string) null, message, (object[]) null, new int?(), new TraceEventType?(TraceEventType.Resume), new Guid?());
    }

    public override void Close()
    {
    }

    public override void Fail(string message)
    {
      this.ProcessLogEventInfo(LogLevel.Error, (string) null, message, (object[]) null, new int?(), new TraceEventType?(TraceEventType.Error), new Guid?());
    }

    public override void Fail(string message, string detailMessage)
    {
      this.ProcessLogEventInfo(LogLevel.Error, (string) null, message + " " + detailMessage, (object[]) null, new int?(), new TraceEventType?(TraceEventType.Error), new Guid?());
    }

    public override void Flush()
    {
      if (this.DisableFlush)
        return;
      if (this.LogFactory != null)
        this.LogFactory.Flush();
      else
        LogManager.Flush();
    }

    public override void TraceData(
      TraceEventCache eventCache,
      string source,
      TraceEventType eventType,
      int id,
      object data)
    {
      if (this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, string.Empty, (object[]) null, data, (object[]) null))
        return;
      this.TraceData(eventCache, source, eventType, id, new object[1]
      {
        data
      });
    }

    public override void TraceData(
      TraceEventCache eventCache,
      string source,
      TraceEventType eventType,
      int id,
      params object[] data)
    {
      if (this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, string.Empty, (object[]) null, (object) null, data))
        return;
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < data.Length; ++index)
      {
        if (index > 0)
          stringBuilder.Append(", ");
        stringBuilder.Append("{");
        stringBuilder.Append(index);
        stringBuilder.Append("}");
      }
      this.ProcessLogEventInfo(NLogTraceListener.TranslateLogLevel(eventType), source, stringBuilder.ToString(), data, new int?(id), new TraceEventType?(eventType), new Guid?());
    }

    public override void TraceEvent(
      TraceEventCache eventCache,
      string source,
      TraceEventType eventType,
      int id)
    {
      if (this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, string.Empty, (object[]) null, (object) null, (object[]) null))
        return;
      this.ProcessLogEventInfo(NLogTraceListener.TranslateLogLevel(eventType), source, string.Empty, (object[]) null, new int?(id), new TraceEventType?(eventType), new Guid?());
    }

    public override void TraceEvent(
      TraceEventCache eventCache,
      string source,
      TraceEventType eventType,
      int id,
      string format,
      params object[] args)
    {
      if (this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, format, args, (object) null, (object[]) null))
        return;
      this.ProcessLogEventInfo(NLogTraceListener.TranslateLogLevel(eventType), source, format, args, new int?(id), new TraceEventType?(eventType), new Guid?());
    }

    public override void TraceEvent(
      TraceEventCache eventCache,
      string source,
      TraceEventType eventType,
      int id,
      string message)
    {
      if (this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, eventType, id, message, (object[]) null, (object) null, (object[]) null))
        return;
      this.ProcessLogEventInfo(NLogTraceListener.TranslateLogLevel(eventType), source, message, (object[]) null, new int?(id), new TraceEventType?(eventType), new Guid?());
    }

    public override void TraceTransfer(
      TraceEventCache eventCache,
      string source,
      int id,
      string message,
      Guid relatedActivityId)
    {
      if (this.Filter != null && !this.Filter.ShouldTrace(eventCache, source, TraceEventType.Transfer, id, message, (object[]) null, (object) null, (object[]) null))
        return;
      this.ProcessLogEventInfo(LogLevel.Debug, source, message, (object[]) null, new int?(id), new TraceEventType?(TraceEventType.Transfer), new Guid?(relatedActivityId));
    }

    protected override string[] GetSupportedAttributes()
    {
      return new string[4]
      {
        "defaultLogLevel",
        "autoLoggerName",
        "forceLogLevel",
        "disableFlush"
      };
    }

    private static LogLevel TranslateLogLevel(TraceEventType eventType)
    {
      switch (eventType)
      {
        case TraceEventType.Critical:
          return LogLevel.Fatal;
        case TraceEventType.Error:
          return LogLevel.Error;
        case TraceEventType.Warning:
          return LogLevel.Warn;
        case TraceEventType.Information:
          return LogLevel.Info;
        case TraceEventType.Verbose:
          return LogLevel.Trace;
        default:
          return LogLevel.Debug;
      }
    }

    protected virtual void ProcessLogEventInfo(
      LogLevel logLevel,
      string loggerName,
      [Localizable(false)] string message,
      object[] arguments,
      int? eventId,
      TraceEventType? eventType,
      Guid? relatedActiviyId)
    {
      loggerName = (loggerName ?? this.Name) ?? string.Empty;
      StackTrace stackTrace = (StackTrace) null;
      int userStackFrame = -1;
      if (this.AutoLoggerName)
      {
        stackTrace = new StackTrace();
        for (int index = 0; index < stackTrace.FrameCount; ++index)
        {
          loggerName = StackTraceUsageUtils.LookupClassNameFromStackFrame(stackTrace.GetFrame(index));
          if (!string.IsNullOrEmpty(loggerName))
          {
            userStackFrame = index;
            break;
          }
        }
      }
      ILogger logger = this.LogFactory == null ? (ILogger) LogManager.GetLogger(loggerName) : (ILogger) this.LogFactory.GetLogger(loggerName);
      LogLevel logLevel1 = this._forceLogLevel;
      if ((object) logLevel1 == null)
        logLevel1 = logLevel;
      logLevel = logLevel1;
      if (!logger.IsEnabled(logLevel))
        return;
      LogEventInfo logEvent = new LogEventInfo();
      logEvent.LoggerName = loggerName;
      logEvent.Level = logLevel;
      if (eventType.HasValue)
        logEvent.Properties.Add((object) "EventType", (object) eventType.Value);
      if (relatedActiviyId.HasValue)
        logEvent.Properties.Add((object) "RelatedActivityID", (object) relatedActiviyId.Value);
      logEvent.Message = message;
      logEvent.Parameters = arguments;
      LogEventInfo logEventInfo = logEvent;
      LogLevel logLevel2 = this._forceLogLevel;
      if ((object) logLevel2 == null)
        logLevel2 = logLevel;
      logEventInfo.Level = logLevel2;
      if (eventId.HasValue)
        logEvent.Properties.Add((object) "EventID", (object) eventId.Value);
      if (stackTrace != null && userStackFrame >= 0)
        logEvent.SetStackTrace(stackTrace, userStackFrame);
      logger.Log(logEvent);
    }

    private void InitAttributes()
    {
      if (this._attributesLoaded)
        return;
      this._attributesLoaded = true;
      if (Trace.AutoFlush)
        this._disableFlush = true;
      foreach (DictionaryEntry attribute in this.Attributes)
      {
        string key = (string) attribute.Key;
        string str = (string) attribute.Value;
        switch (key.ToUpperInvariant())
        {
          case "DEFAULTLOGLEVEL":
            this._defaultLogLevel = LogLevel.FromString(str);
            continue;
          case "FORCELOGLEVEL":
            this._forceLogLevel = LogLevel.FromString(str);
            continue;
          case "AUTOLOGGERNAME":
            this.AutoLoggerName = XmlConvert.ToBoolean(str);
            continue;
          case "DISABLEFLUSH":
            this._disableFlush = bool.Parse(str);
            continue;
          default:
            continue;
        }
      }
    }
  }
}
