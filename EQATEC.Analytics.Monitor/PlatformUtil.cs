// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.PlatformUtil
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;
using System.Threading;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  internal static class PlatformUtil
  {
    internal static void Wait(AutoResetEvent resetEvent, int timeout)
    {
      resetEvent.WaitOne(timeout, false);
    }

    internal static void DisposeSafely<T>(ILogAnalyticsMonitor log, T obj) where T : class, IDisposable
    {
      if ((object) obj == null)
        return;
      PlatformUtil.DoSafely(log, new PlatformUtil.Action(((IDisposable) obj).Dispose), string.Empty);
    }

    internal static void DoSafely(
      ILogAnalyticsMonitor log,
      PlatformUtil.Action action,
      string message)
    {
      try
      {
        if (action == null)
          return;
        action();
      }
      catch (Exception ex)
      {
        if (log == null || string.IsNullOrEmpty(message))
          return;
        log.LogError(message + ": " + ex.Message);
      }
    }

    internal delegate void Action();
  }
}
