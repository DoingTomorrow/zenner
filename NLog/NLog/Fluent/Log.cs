// Decompiled with JetBrains decompiler
// Type: NLog.Fluent.Log
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System.IO;
using System.Runtime.CompilerServices;

#nullable disable
namespace NLog.Fluent
{
  public static class Log
  {
    private static readonly ILogger _logger = (ILogger) LogManager.GetCurrentClassLogger();

    public static LogBuilder Level(LogLevel logLevel, [CallerFilePath] string callerFilePath = null)
    {
      return Log.Create(logLevel, callerFilePath);
    }

    public static LogBuilder Trace([CallerFilePath] string callerFilePath = null)
    {
      return Log.Create(LogLevel.Trace, callerFilePath);
    }

    public static LogBuilder Debug([CallerFilePath] string callerFilePath = null)
    {
      return Log.Create(LogLevel.Debug, callerFilePath);
    }

    public static LogBuilder Info([CallerFilePath] string callerFilePath = null)
    {
      return Log.Create(LogLevel.Info, callerFilePath);
    }

    public static LogBuilder Warn([CallerFilePath] string callerFilePath = null)
    {
      return Log.Create(LogLevel.Warn, callerFilePath);
    }

    public static LogBuilder Error([CallerFilePath] string callerFilePath = null)
    {
      return Log.Create(LogLevel.Error, callerFilePath);
    }

    public static LogBuilder Fatal([CallerFilePath] string callerFilePath = null)
    {
      return Log.Create(LogLevel.Fatal, callerFilePath);
    }

    private static LogBuilder Create(LogLevel logLevel, string callerFilePath)
    {
      string withoutExtension = Path.GetFileNameWithoutExtension(callerFilePath ?? string.Empty);
      LogBuilder logBuilder = new LogBuilder(string.IsNullOrWhiteSpace(withoutExtension) ? Log._logger : (ILogger) LogManager.GetLogger(withoutExtension), logLevel);
      if (callerFilePath != null)
        logBuilder.Property((object) "CallerFilePath", (object) callerFilePath);
      return logBuilder;
    }
  }
}
