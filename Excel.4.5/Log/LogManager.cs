// Decompiled with JetBrains decompiler
// Type: Excel.Log.LogManager
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System.Collections.Generic;

#nullable disable
namespace Excel.Log
{
  public static class LogManager
  {
    private static readonly Dictionary<string, ILog> _dictionary = new Dictionary<string, ILog>();
    private static object _sync = new object();

    public static ILog Log<T>(T type) => LogManager.Log(typeof (T).FullName);

    public static ILog Log(string objectName)
    {
      ILog log = (ILog) null;
      if (LogManager._dictionary.ContainsKey(objectName))
        log = LogManager._dictionary[objectName];
      if (log == null)
      {
        lock (LogManager._sync)
        {
          log = Excel.Log.Log.GetLoggerFor(objectName);
          LogManager._dictionary.Add(objectName, log);
        }
      }
      return log;
    }
  }
}
