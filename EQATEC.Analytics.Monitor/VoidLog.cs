// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.VoidLog
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  internal class VoidLog : ILogAnalyticsMonitor
  {
    public void LogMessage(string message) => VoidLog.RaiseMessage(message);

    public void LogError(string errorMessage) => VoidLog.RaiseError(errorMessage);

    private static void RaiseError(string msg)
    {
      EventHandler<VoidLog.MessageEventArgs> error = VoidLog.Error;
      if (error == null)
        return;
      error((object) null, new VoidLog.MessageEventArgs(msg));
    }

    private static void RaiseMessage(string msg)
    {
      EventHandler<VoidLog.MessageEventArgs> message = VoidLog.Message;
      if (message == null)
        return;
      message((object) null, new VoidLog.MessageEventArgs(msg));
    }

    internal static event EventHandler<VoidLog.MessageEventArgs> Message;

    internal static event EventHandler<VoidLog.MessageEventArgs> Error;

    internal class MessageEventArgs : EventArgs
    {
      public string Message { get; private set; }

      public MessageEventArgs(string message) => this.Message = message;
    }
  }
}
