// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.TraceLog
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System.Diagnostics;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  internal class TraceLog : ILogAnalyticsMonitor
  {
    public void LogMessage(string message)
    {
      Trace.WriteLine(string.Format("EQATEC Analytics Monitor: {0}", (object) message));
    }

    public void LogError(string errorMessage)
    {
      Trace.WriteLine(string.Format("EQATEC Analytics Monitor ***** ERROR *****: {0}", (object) errorMessage));
    }
  }
}
