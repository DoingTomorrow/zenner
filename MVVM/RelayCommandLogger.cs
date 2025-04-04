// Decompiled with JetBrains decompiler
// Type: MVVM.RelayCommandLogger
// Assembly: MVVM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5D33A3C4-E333-437E-9AB2-FFDB9D6F32F8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MVVM.dll

using NLog;

#nullable disable
namespace MVVM
{
  public static class RelayCommandLogger
  {
    public static Logger GetLogger() => LogManager.GetCurrentClassLogger();

    public static void WriteMessageToLogger(this Logger logger, LogLevel logLevel, string message)
    {
      logger.Log(logLevel, message);
    }

    public static void LogDebug(string message) => RelayCommandLogger.GetLogger().Debug(message);
  }
}
