// Decompiled with JetBrains decompiler
// Type: Excel.Log.LogExtensions
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System;
using System.Collections.Concurrent;

#nullable disable
namespace Excel.Log
{
  public static class LogExtensions
  {
    private static readonly Lazy<ConcurrentDictionary<string, ILog>> _dictionary = new Lazy<ConcurrentDictionary<string, ILog>>((Func<ConcurrentDictionary<string, ILog>>) (() => new ConcurrentDictionary<string, ILog>()));

    public static ILog Log<T>(this T type) => typeof (T).FullName.Log();

    public static ILog Log(this string objectName)
    {
      return LogExtensions._dictionary.Value.GetOrAdd(objectName, new Func<string, ILog>(Excel.Log.Log.GetLoggerFor));
    }
  }
}
