// Decompiled with JetBrains decompiler
// Type: Castle.Core.Logging.ConsoleLogger
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Globalization;

#nullable disable
namespace Castle.Core.Logging
{
  [Serializable]
  public class ConsoleLogger(string name, LoggerLevel logLevel) : LevelFilteredLogger(name, logLevel)
  {
    public ConsoleLogger()
      : this(string.Empty, LoggerLevel.Debug)
    {
    }

    public ConsoleLogger(LoggerLevel logLevel)
      : this(string.Empty, logLevel)
    {
    }

    public ConsoleLogger(string name)
      : this(name, LoggerLevel.Debug)
    {
    }

    protected override void Log(
      LoggerLevel loggerLevel,
      string loggerName,
      string message,
      Exception exception)
    {
      Console.Out.WriteLine("[{0}] '{1}' {2}", (object) loggerLevel, (object) loggerName, (object) message);
      if (exception == null)
        return;
      Console.Out.WriteLine("[{0}] '{1}' {2}: {3} {4}", (object) loggerLevel.ToString(), (object) loggerName, (object) exception.GetType().FullName, (object) exception.Message, (object) exception.StackTrace);
    }

    public override ILogger CreateChildLogger(string loggerName)
    {
      if (loggerName == null)
        throw new ArgumentNullException(nameof (loggerName), "To create a child logger you must supply a non null name");
      return (ILogger) new ConsoleLogger(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0}.{1}", (object) this.Name, (object) loggerName), this.Level);
    }
  }
}
