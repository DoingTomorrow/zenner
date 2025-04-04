// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Transmitter
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using EQATEC.Analytics.Monitor.Messaging;
using EQATEC.Analytics.Monitor.Policy;
using System;
using System.Threading;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  internal class Transmitter : ITransmitter
  {
    private readonly IMessagingSubSystem m_messagingSubSystem;
    private readonly IMessageFactory m_messageFactory;
    private readonly ILogAnalyticsMonitor m_log;
    private readonly MonitorPolicy m_monitorPolicy;
    private readonly object m_lock = new object();

    internal Transmitter(
      IMessagingSubSystem messagingSubSystem,
      IMessageFactory messageFactory,
      ILogAnalyticsMonitor log,
      MonitorPolicy monitorPolicy)
    {
      this.m_messagingSubSystem = messagingSubSystem;
      this.m_messageFactory = messageFactory;
      this.m_log = log;
      this.m_monitorPolicy = monitorPolicy;
    }

    public void Send(
      StatisticsData statistics,
      int waitInMilliseconds,
      Action<SendResult> callback)
    {
      MessagePayload payload = this.m_messageFactory.BuildStatisticsMessage(new MessageContext(statistics, this.m_monitorPolicy));
      if (payload == null)
      {
        callback(SendResult.Failure);
      }
      else
      {
        AutoResetEvent evt = (AutoResetEvent) null;
        if (waitInMilliseconds > 0)
          evt = Timing.CreateWaitHandle();
        this.m_messagingSubSystem.SendMessage(payload, (Action<SendMessageResult>) (sendResult =>
        {
          SendResult result = sendResult.Result;
          try
          {
            lock (this.m_lock)
            {
              switch (sendResult.Result)
              {
                case SendResult.Success:
                  this.m_monitorPolicy.RuntimeStatus.SyncedVersion = statistics.Statistics.Version;
                  this.m_monitorPolicy.RuntimeStatus.HasSentOSInfo = true;
                  break;
                case SendResult.Failure:
                  break;
                default:
                  this.m_log.LogError("Failed to determine the SendResult status: result=" + (object) sendResult.Result);
                  break;
              }
            }
          }
          catch (Exception ex)
          {
            this.m_log.LogError("Error while receiving statistics reply: " + ex.Message);
          }
          finally
          {
            callback(result);
            if (sendResult.Result == SendResult.Success)
              this.m_log.LogMessage("Statistics was sent successfully");
            else
              this.m_log.LogMessage("Statistics failed to be sent: " + sendResult.Message);
            if (evt != null)
            {
              evt.Set();
              evt.Close();
              evt = (AutoResetEvent) null;
            }
          }
        }));
        if (waitInMilliseconds <= 0 || evt == null)
          return;
        PlatformUtil.Wait(evt, waitInMilliseconds);
      }
    }
  }
}
