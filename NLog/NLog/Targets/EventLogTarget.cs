// Decompiled with JetBrains decompiler
// Type: NLog.Targets.EventLogTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using NLog.Internal.Fakeables;
using NLog.Layouts;
using System;
using System.ComponentModel;
using System.Diagnostics;

#nullable disable
namespace NLog.Targets
{
  [Target("EventLog")]
  public class EventLogTarget : TargetWithLayout, IInstallable
  {
    private EventLog eventLogInstance;
    private int maxMessageLength;
    private long? maxKilobytes;

    public EventLogTarget()
      : this(LogFactory.CurrentAppDomain)
    {
    }

    public EventLogTarget(IAppDomain appDomain)
    {
      this.Source = (Layout) appDomain.FriendlyName;
      this.Log = "Application";
      this.MachineName = ".";
      this.MaxMessageLength = 16384;
      this.OptimizeBufferReuse = this.GetType() == typeof (EventLogTarget);
    }

    public EventLogTarget(string name)
      : this(LogFactory.CurrentAppDomain)
    {
      this.Name = name;
    }

    [DefaultValue(".")]
    public string MachineName { get; set; }

    public Layout EventId { get; set; }

    public Layout Category { get; set; }

    public Layout EntryType { get; set; }

    public Layout Source { get; set; }

    [DefaultValue("Application")]
    public string Log { get; set; }

    [DefaultValue(16384)]
    public int MaxMessageLength
    {
      get => this.maxMessageLength;
      set
      {
        this.maxMessageLength = value > 0 ? value : throw new ArgumentException("MaxMessageLength cannot be zero or negative.");
      }
    }

    [DefaultValue(null)]
    public long? MaxKilobytes
    {
      get => this.maxKilobytes;
      set
      {
        if (value.HasValue)
        {
          long? nullable1 = value;
          long num1 = 64;
          if ((nullable1.GetValueOrDefault() < num1 ? (nullable1.HasValue ? 1 : 0) : 0) == 0)
          {
            nullable1 = value;
            long num2 = 4194240;
            if ((nullable1.GetValueOrDefault() > num2 ? (nullable1.HasValue ? 1 : 0) : 0) == 0)
            {
              long? nullable2 = value;
              long num3 = 64;
              nullable1 = nullable2.HasValue ? new long?(nullable2.GetValueOrDefault() % num3) : new long?();
              long num4 = 0;
              if ((nullable1.GetValueOrDefault() == num4 ? (!nullable1.HasValue ? 1 : 0) : 1) == 0)
                goto label_5;
            }
          }
          throw new ArgumentException("MaxKilobytes must be a multitude of 64, and between 64 and 4194240");
        }
label_5:
        this.maxKilobytes = value;
      }
    }

    [DefaultValue(EventLogTargetOverflowAction.Truncate)]
    public EventLogTargetOverflowAction OnOverflow { get; set; }

    public void Install(InstallationContext installationContext)
    {
      this.CreateEventSourceIfNeeded(this.GetFixedSource(), true);
    }

    public void Uninstall(InstallationContext installationContext)
    {
      string fixedSource = this.GetFixedSource();
      if (string.IsNullOrEmpty(fixedSource))
        InternalLogger.Debug<string>("EventLogTarget(Name={0}): Skipping removing of event source because it contains layout renderers", this.Name);
      else
        EventLog.DeleteEventSource(fixedSource, this.MachineName);
    }

    public bool? IsInstalled(InstallationContext installationContext)
    {
      string fixedSource = this.GetFixedSource();
      if (!string.IsNullOrEmpty(fixedSource))
        return new bool?(EventLog.SourceExists(fixedSource, this.MachineName));
      InternalLogger.Debug<string>("EventLogTarget(Name={0}): Unclear if event source exists because it contains layout renderers", this.Name);
      return new bool?();
    }

    protected override void InitializeTarget()
    {
      base.InitializeTarget();
      string fixedSource = this.GetFixedSource();
      if (string.IsNullOrEmpty(fixedSource))
      {
        InternalLogger.Debug<string>("EventLogTarget(Name={0}): Skipping creation of event source because it contains layout renderers", this.Name);
      }
      else
      {
        if (EventLog.LogNameFromSourceName(fixedSource, this.MachineName).Equals(this.Log, StringComparison.CurrentCultureIgnoreCase))
          return;
        this.CreateEventSourceIfNeeded(fixedSource, false);
      }
    }

    protected override void Write(LogEventInfo logEvent)
    {
      string message1 = this.RenderLogEvent(this.Layout, logEvent);
      EventLogEntryType entryType = this.GetEntryType(logEvent);
      int eventId = this.EventId.RenderInt(logEvent, 0, "EventLogTarget.EventId");
      short category = this.Category.RenderShort(logEvent, (short) 0, "EventLogTarget.Category");
      if (message1.Length > this.MaxMessageLength)
      {
        if (this.OnOverflow == EventLogTargetOverflowAction.Truncate)
        {
          string message2 = message1.Substring(0, this.MaxMessageLength);
          this.WriteEntry(logEvent, message2, entryType, eventId, category);
        }
        else if (this.OnOverflow == EventLogTargetOverflowAction.Split)
        {
          for (int startIndex = 0; startIndex < message1.Length; startIndex += this.MaxMessageLength)
          {
            string message3 = message1.Substring(startIndex, Math.Min(this.MaxMessageLength, message1.Length - startIndex));
            this.WriteEntry(logEvent, message3, entryType, eventId, category);
          }
        }
        else
        {
          int onOverflow = (int) this.OnOverflow;
        }
      }
      else
        this.WriteEntry(logEvent, message1, entryType, eventId, category);
    }

    internal virtual void WriteEntry(
      LogEventInfo logEventInfo,
      string message,
      EventLogEntryType entryType,
      int eventId,
      short category)
    {
      this.GetEventLog(logEventInfo).WriteEntry(message, entryType, eventId, category);
    }

    private EventLogEntryType GetEntryType(LogEventInfo logEvent)
    {
      EventLogEntryType result;
      if (this.EntryType != null && EnumHelpers.TryParse<EventLogEntryType>(this.RenderLogEvent(this.EntryType, logEvent), true, out result))
        return result;
      if (logEvent.Level >= NLog.LogLevel.Error)
        return EventLogEntryType.Error;
      return logEvent.Level >= NLog.LogLevel.Warn ? EventLogEntryType.Warning : EventLogEntryType.Information;
    }

    internal string GetFixedSource()
    {
      if (this.Source == null)
        return (string) null;
      return this.Source is SimpleLayout source && source.IsFixedText ? source.FixedText : (string) null;
    }

    private EventLog GetEventLog(LogEventInfo logEvent)
    {
      string source = this.RenderSource(logEvent);
      if ((this.eventLogInstance == null || !(source == this.eventLogInstance.Source) || !(this.eventLogInstance.Log == this.Log) ? 0 : (this.eventLogInstance.MachineName == this.MachineName ? 1 : 0)) == 0)
        this.eventLogInstance = new EventLog(this.Log, this.MachineName, source);
      long? maxKilobytes = this.MaxKilobytes;
      if (maxKilobytes.HasValue)
      {
        EventLog eventLogInstance = this.eventLogInstance;
        maxKilobytes = this.MaxKilobytes;
        long num = maxKilobytes.Value;
        eventLogInstance.MaximumKilobytes = num;
      }
      return this.eventLogInstance;
    }

    internal string RenderSource(LogEventInfo logEvent)
    {
      return this.Source == null ? (string) null : this.RenderLogEvent(this.Source, logEvent);
    }

    private void CreateEventSourceIfNeeded(string fixedSource, bool alwaysThrowError)
    {
      if (string.IsNullOrEmpty(fixedSource))
      {
        InternalLogger.Debug<string>("EventLogTarget(Name={0}): Skipping creation of event source because it contains layout renderers", this.Name);
      }
      else
      {
        try
        {
          if (EventLog.SourceExists(fixedSource, this.MachineName))
          {
            if (EventLog.LogNameFromSourceName(fixedSource, this.MachineName).Equals(this.Log, StringComparison.CurrentCultureIgnoreCase))
              return;
            EventLog.DeleteEventSource(fixedSource, this.MachineName);
            EventLog.CreateEventSource(new EventSourceCreationData(fixedSource, this.Log)
            {
              MachineName = this.MachineName
            });
          }
          else
            EventLog.CreateEventSource(new EventSourceCreationData(fixedSource, this.Log)
            {
              MachineName = this.MachineName
            });
        }
        catch (Exception ex)
        {
          InternalLogger.Error(ex, "EventLogTarget(Name={0}): Error when connecting to EventLog.", (object) this.Name);
          if (!alwaysThrowError && !ex.MustBeRethrown())
            return;
          throw;
        }
      }
    }
  }
}
