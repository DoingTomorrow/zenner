// Decompiled with JetBrains decompiler
// Type: Excel.Log.Log
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using Excel.Log.Logger;
using System;

#nullable disable
namespace Excel.Log
{
  public static class Log
  {
    private static Type _logType = typeof (NullLog);
    private static ILog _logger;

    public static void InitializeWith<T>() where T : ILog, new() => Excel.Log.Log._logType = typeof (T);

    public static void InitializeWith(ILog loggerType)
    {
      Excel.Log.Log._logType = loggerType.GetType();
      Excel.Log.Log._logger = loggerType;
    }

    public static ILog GetLoggerFor(string objectName)
    {
      loggerFor = Excel.Log.Log._logger;
      if (Excel.Log.Log._logger == null && Activator.CreateInstance(Excel.Log.Log._logType) is ILog loggerFor)
        loggerFor.InitializeFor(objectName);
      return loggerFor;
    }
  }
}
