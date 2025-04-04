// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Timing
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;
using System.Threading;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  internal static class Timing
  {
    private static Timing.SystemTimeCall s_systemTime = new Timing.SystemTimeCall(Timing.DefaultSystemTime);
    private static Timing.SystemUptimeCall s_systemUptime = new Timing.SystemUptimeCall(Timing.DefaultSystemUptime);
    private static Timing.WaitHandleCall s_waitHandle = new Timing.WaitHandleCall(Timing.DefaultWaitHandle);

    internal static void SetSystemTimeCall(Timing.SystemTimeCall call)
    {
      Timing.s_systemTime = call ?? new Timing.SystemTimeCall(Timing.DefaultSystemTime);
    }

    internal static void SetSystemUptimeCall(Timing.SystemUptimeCall call)
    {
      Timing.s_systemUptime = call ?? new Timing.SystemUptimeCall(Timing.DefaultSystemUptime);
    }

    internal static void ResetTiming()
    {
      Timing.SetSystemTimeCall((Timing.SystemTimeCall) null);
      Timing.SetSystemUptimeCall((Timing.SystemUptimeCall) null);
    }

    private static DateTime DefaultSystemTime() => DateTime.Now;

    private static AutoResetEvent DefaultWaitHandle() => new AutoResetEvent(false);

    private static TimeSpan DefaultSystemUptime() => new TimeSpan(DateTime.UtcNow.Ticks);

    internal static DateTime Now => Timing.s_systemTime();

    internal static DateTime UtcNow => TimeZone.CurrentTimeZone.ToUniversalTime(Timing.Now);

    internal static TimeSpan Uptime => Timing.s_systemUptime();

    internal static AutoResetEvent CreateWaitHandle() => Timing.s_waitHandle();

    internal delegate DateTime SystemTimeCall();

    internal delegate TimeSpan SystemUptimeCall();

    internal delegate AutoResetEvent WaitHandleCall();
  }
}
