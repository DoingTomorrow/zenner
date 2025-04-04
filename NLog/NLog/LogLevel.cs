// Decompiled with JetBrains decompiler
// Type: NLog.LogLevel
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace NLog
{
  public sealed class LogLevel : IComparable, IEquatable<LogLevel>
  {
    public static readonly LogLevel Trace = new LogLevel(nameof (Trace), 0);
    public static readonly LogLevel Debug = new LogLevel(nameof (Debug), 1);
    public static readonly LogLevel Info = new LogLevel(nameof (Info), 2);
    public static readonly LogLevel Warn = new LogLevel(nameof (Warn), 3);
    public static readonly LogLevel Error = new LogLevel(nameof (Error), 4);
    public static readonly LogLevel Fatal = new LogLevel(nameof (Fatal), 5);
    public static readonly LogLevel Off = new LogLevel(nameof (Off), 6);
    private static readonly IList<LogLevel> allLevels = (IList<LogLevel>) new List<LogLevel>()
    {
      LogLevel.Trace,
      LogLevel.Debug,
      LogLevel.Info,
      LogLevel.Warn,
      LogLevel.Error,
      LogLevel.Fatal,
      LogLevel.Off
    }.AsReadOnly();
    private static readonly IList<LogLevel> allLoggingLevels = (IList<LogLevel>) new List<LogLevel>()
    {
      LogLevel.Trace,
      LogLevel.Debug,
      LogLevel.Info,
      LogLevel.Warn,
      LogLevel.Error,
      LogLevel.Fatal
    }.AsReadOnly();
    private readonly int _ordinal;
    private readonly string _name;

    public static IEnumerable<LogLevel> AllLevels => (IEnumerable<LogLevel>) LogLevel.allLevels;

    public static IEnumerable<LogLevel> AllLoggingLevels
    {
      get => (IEnumerable<LogLevel>) LogLevel.allLoggingLevels;
    }

    private LogLevel(string name, int ordinal)
    {
      this._name = name;
      this._ordinal = ordinal;
    }

    public string Name => this._name;

    internal static LogLevel MaxLevel => LogLevel.Fatal;

    internal static LogLevel MinLevel => LogLevel.Trace;

    public int Ordinal => this._ordinal;

    public static bool operator ==(LogLevel level1, LogLevel level2)
    {
      if ((object) level1 == null)
        return (object) level2 == null;
      return (object) level2 != null && level1.Ordinal == level2.Ordinal;
    }

    public static bool operator !=(LogLevel level1, LogLevel level2)
    {
      if ((object) level1 == null)
        return level2 != null;
      return (object) level2 == null || level1.Ordinal != level2.Ordinal;
    }

    public static bool operator >(LogLevel level1, LogLevel level2)
    {
      if (level1 == (LogLevel) null)
        throw new ArgumentNullException(nameof (level1));
      if (level2 == (LogLevel) null)
        throw new ArgumentNullException(nameof (level2));
      return level1.Ordinal > level2.Ordinal;
    }

    public static bool operator >=(LogLevel level1, LogLevel level2)
    {
      if (level1 == (LogLevel) null)
        throw new ArgumentNullException(nameof (level1));
      if (level2 == (LogLevel) null)
        throw new ArgumentNullException(nameof (level2));
      return level1.Ordinal >= level2.Ordinal;
    }

    public static bool operator <(LogLevel level1, LogLevel level2)
    {
      if (level1 == (LogLevel) null)
        throw new ArgumentNullException(nameof (level1));
      if (level2 == (LogLevel) null)
        throw new ArgumentNullException(nameof (level2));
      return level1.Ordinal < level2.Ordinal;
    }

    public static bool operator <=(LogLevel level1, LogLevel level2)
    {
      if (level1 == (LogLevel) null)
        throw new ArgumentNullException(nameof (level1));
      if (level2 == (LogLevel) null)
        throw new ArgumentNullException(nameof (level2));
      return level1.Ordinal <= level2.Ordinal;
    }

    public static LogLevel FromOrdinal(int ordinal)
    {
      switch (ordinal)
      {
        case 0:
          return LogLevel.Trace;
        case 1:
          return LogLevel.Debug;
        case 2:
          return LogLevel.Info;
        case 3:
          return LogLevel.Warn;
        case 4:
          return LogLevel.Error;
        case 5:
          return LogLevel.Fatal;
        case 6:
          return LogLevel.Off;
        default:
          throw new ArgumentException("Invalid ordinal.");
      }
    }

    public static LogLevel FromString(string levelName)
    {
      if (levelName == null)
        throw new ArgumentNullException(nameof (levelName));
      if (levelName.Equals("Trace", StringComparison.OrdinalIgnoreCase))
        return LogLevel.Trace;
      if (levelName.Equals("Debug", StringComparison.OrdinalIgnoreCase))
        return LogLevel.Debug;
      if (levelName.Equals("Info", StringComparison.OrdinalIgnoreCase))
        return LogLevel.Info;
      if (levelName.Equals("Warn", StringComparison.OrdinalIgnoreCase))
        return LogLevel.Warn;
      if (levelName.Equals("Error", StringComparison.OrdinalIgnoreCase))
        return LogLevel.Error;
      if (levelName.Equals("Fatal", StringComparison.OrdinalIgnoreCase))
        return LogLevel.Fatal;
      if (levelName.Equals("Off", StringComparison.OrdinalIgnoreCase))
        return LogLevel.Off;
      throw new ArgumentException(string.Format("Unknown log level: {0}", (object) levelName));
    }

    public override string ToString() => this.Name;

    public override int GetHashCode() => this.Ordinal;

    public override bool Equals(object obj)
    {
      LogLevel logLevel = obj as LogLevel;
      return (object) logLevel != null && this.Ordinal == logLevel.Ordinal;
    }

    public bool Equals(LogLevel other) => other != (LogLevel) null && this.Ordinal == other.Ordinal;

    public int CompareTo(object obj)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj));
      return this.Ordinal - ((LogLevel) obj).Ordinal;
    }
  }
}
