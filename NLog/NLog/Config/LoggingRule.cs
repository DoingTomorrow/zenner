// Decompiled with JetBrains decompiler
// Type: NLog.Config.LoggingRule
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;

#nullable disable
namespace NLog.Config
{
  [NLogConfigurationItem]
  public class LoggingRule
  {
    private readonly bool[] _logLevels = new bool[NLog.LogLevel.MaxLevel.Ordinal + 1];
    private string _loggerNamePattern;
    private LoggingRule.MatchMode _loggerNameMatchMode;
    private string _loggerNameMatchArgument;

    public LoggingRule()
    {
      this.Filters = (IList<NLog.Filters.Filter>) new List<NLog.Filters.Filter>();
      this.ChildRules = (IList<LoggingRule>) new List<LoggingRule>();
      this.Targets = (IList<Target>) new List<Target>();
    }

    public LoggingRule(
      string loggerNamePattern,
      NLog.LogLevel minLevel,
      NLog.LogLevel maxLevel,
      Target target)
      : this()
    {
      this.LoggerNamePattern = loggerNamePattern;
      this.Targets.Add(target);
      this.EnableLoggingForLevels(minLevel, maxLevel);
    }

    public LoggingRule(string loggerNamePattern, NLog.LogLevel minLevel, Target target)
      : this()
    {
      this.LoggerNamePattern = loggerNamePattern;
      this.Targets.Add(target);
      this.EnableLoggingForLevels(minLevel, NLog.LogLevel.MaxLevel);
    }

    public LoggingRule(string loggerNamePattern, Target target)
      : this()
    {
      this.LoggerNamePattern = loggerNamePattern;
      this.Targets.Add(target);
    }

    public IList<Target> Targets { get; }

    public IList<LoggingRule> ChildRules { get; }

    internal List<LoggingRule> GetChildRulesThreadSafe()
    {
      lock (this.ChildRules)
        return this.ChildRules.ToList<LoggingRule>();
    }

    internal List<Target> GetTargetsThreadSafe()
    {
      lock (this.Targets)
        return this.Targets.ToList<Target>();
    }

    internal bool RemoveTargetThreadSafe(Target target)
    {
      lock (this.Targets)
        return this.Targets.Remove(target);
    }

    public IList<NLog.Filters.Filter> Filters { get; }

    public bool Final { get; set; }

    public string LoggerNamePattern
    {
      get => this._loggerNamePattern;
      set
      {
        this._loggerNamePattern = value;
        int length = this._loggerNamePattern.IndexOf('*');
        int num = this._loggerNamePattern.LastIndexOf('*');
        if (length < 0)
        {
          this._loggerNameMatchMode = LoggingRule.MatchMode.Equals;
          this._loggerNameMatchArgument = value;
        }
        else if (length == num)
        {
          string str1 = this.LoggerNamePattern.Substring(0, length);
          string str2 = this.LoggerNamePattern.Substring(length + 1);
          if (str1.Length > 0)
          {
            this._loggerNameMatchMode = LoggingRule.MatchMode.StartsWith;
            this._loggerNameMatchArgument = str1;
          }
          else
          {
            if (str2.Length <= 0)
              return;
            this._loggerNameMatchMode = LoggingRule.MatchMode.EndsWith;
            this._loggerNameMatchArgument = str2;
          }
        }
        else if (length == 0 && num == this.LoggerNamePattern.Length - 1)
        {
          string str = this.LoggerNamePattern.Substring(1, this.LoggerNamePattern.Length - 2);
          this._loggerNameMatchMode = LoggingRule.MatchMode.Contains;
          this._loggerNameMatchArgument = str;
        }
        else
        {
          this._loggerNameMatchMode = LoggingRule.MatchMode.None;
          this._loggerNameMatchArgument = string.Empty;
        }
      }
    }

    public ReadOnlyCollection<NLog.LogLevel> Levels
    {
      get
      {
        List<NLog.LogLevel> logLevelList = new List<NLog.LogLevel>();
        for (int ordinal = NLog.LogLevel.MinLevel.Ordinal; ordinal <= NLog.LogLevel.MaxLevel.Ordinal; ++ordinal)
        {
          if (this._logLevels[ordinal])
            logLevelList.Add(NLog.LogLevel.FromOrdinal(ordinal));
        }
        return logLevelList.AsReadOnly();
      }
    }

    public void EnableLoggingForLevel(NLog.LogLevel level)
    {
      if (level == NLog.LogLevel.Off)
        return;
      this._logLevels[level.Ordinal] = true;
    }

    public void EnableLoggingForLevels(NLog.LogLevel minLevel, NLog.LogLevel maxLevel)
    {
      for (int ordinal = minLevel.Ordinal; ordinal <= maxLevel.Ordinal; ++ordinal)
        this.EnableLoggingForLevel(NLog.LogLevel.FromOrdinal(ordinal));
    }

    public void DisableLoggingForLevel(NLog.LogLevel level)
    {
      if (level == NLog.LogLevel.Off)
        return;
      this._logLevels[level.Ordinal] = false;
    }

    public void DisableLoggingForLevels(NLog.LogLevel minLevel, NLog.LogLevel maxLevel)
    {
      for (int ordinal = minLevel.Ordinal; ordinal <= maxLevel.Ordinal; ++ordinal)
        this.DisableLoggingForLevel(NLog.LogLevel.FromOrdinal(ordinal));
    }

    public void SetLoggingLevels(NLog.LogLevel minLevel, NLog.LogLevel maxLevel)
    {
      this.DisableLoggingForLevels(NLog.LogLevel.MinLevel, NLog.LogLevel.MaxLevel);
      this.EnableLoggingForLevels(minLevel, maxLevel);
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "logNamePattern: ({0}:{1})", new object[2]
      {
        (object) this._loggerNameMatchArgument,
        (object) this._loggerNameMatchMode
      });
      stringBuilder.Append(" levels: [ ");
      for (int ordinal = 0; ordinal < this._logLevels.Length; ++ordinal)
      {
        if (this._logLevels[ordinal])
          stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0} ", new object[1]
          {
            (object) NLog.LogLevel.FromOrdinal(ordinal).ToString()
          });
      }
      stringBuilder.Append("] appendTo: [ ");
      foreach (Target target in this.GetTargetsThreadSafe())
        stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0} ", new object[1]
        {
          (object) target.Name
        });
      stringBuilder.Append("]");
      return stringBuilder.ToString();
    }

    public bool IsLoggingEnabledForLevel(NLog.LogLevel level)
    {
      return !(level == NLog.LogLevel.Off) && this._logLevels[level.Ordinal];
    }

    public bool NameMatches(string loggerName)
    {
      switch (this._loggerNameMatchMode)
      {
        case LoggingRule.MatchMode.All:
          return true;
        case LoggingRule.MatchMode.Equals:
          return loggerName.Equals(this._loggerNameMatchArgument, StringComparison.Ordinal);
        case LoggingRule.MatchMode.StartsWith:
          return loggerName.StartsWith(this._loggerNameMatchArgument, StringComparison.Ordinal);
        case LoggingRule.MatchMode.EndsWith:
          return loggerName.EndsWith(this._loggerNameMatchArgument, StringComparison.Ordinal);
        case LoggingRule.MatchMode.Contains:
          return loggerName.IndexOf(this._loggerNameMatchArgument, StringComparison.Ordinal) >= 0;
        default:
          return false;
      }
    }

    internal enum MatchMode
    {
      All,
      None,
      Equals,
      StartsWith,
      EndsWith,
      Contains,
    }
  }
}
