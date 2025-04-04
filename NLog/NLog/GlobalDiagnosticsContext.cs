// Decompiled with JetBrains decompiler
// Type: NLog.GlobalDiagnosticsContext
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Internal;
using System;
using System.Collections.Generic;

#nullable disable
namespace NLog
{
  public static class GlobalDiagnosticsContext
  {
    private static Dictionary<string, object> dict = new Dictionary<string, object>();

    public static void Set(string item, string value)
    {
      lock (GlobalDiagnosticsContext.dict)
        GlobalDiagnosticsContext.dict[item] = (object) value;
    }

    public static void Set(string item, object value)
    {
      lock (GlobalDiagnosticsContext.dict)
        GlobalDiagnosticsContext.dict[item] = value;
    }

    public static string Get(string item)
    {
      return GlobalDiagnosticsContext.Get(item, (IFormatProvider) null);
    }

    public static string Get(string item, IFormatProvider formatProvider)
    {
      return FormatHelper.ConvertToString(GlobalDiagnosticsContext.GetObject(item), formatProvider);
    }

    public static object GetObject(string item)
    {
      lock (GlobalDiagnosticsContext.dict)
      {
        object obj;
        if (!GlobalDiagnosticsContext.dict.TryGetValue(item, out obj))
          obj = (object) null;
        return obj;
      }
    }

    public static ICollection<string> GetNames()
    {
      lock (GlobalDiagnosticsContext.dict)
        return (ICollection<string>) GlobalDiagnosticsContext.dict.Keys;
    }

    public static bool Contains(string item)
    {
      lock (GlobalDiagnosticsContext.dict)
        return GlobalDiagnosticsContext.dict.ContainsKey(item);
    }

    public static void Remove(string item)
    {
      lock (GlobalDiagnosticsContext.dict)
        GlobalDiagnosticsContext.dict.Remove(item);
    }

    public static void Clear()
    {
      lock (GlobalDiagnosticsContext.dict)
        GlobalDiagnosticsContext.dict.Clear();
    }
  }
}
