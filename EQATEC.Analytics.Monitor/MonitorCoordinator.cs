// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.MonitorCoordinator
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using EQATEC.Analytics.Monitor.Messaging;
using EQATEC.Analytics.Monitor.Policy;
using System;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  internal class MonitorCoordinator : IMonitorCoordinator, IDisposable
  {
    private static readonly TimeSpan s_24Hours = TimeSpan.FromHours(24.0);
    private readonly IStatisticsContainer m_statisticsContainer;
    private readonly ITimer m_timer;
    private readonly ITransmitter m_transmitter;
    private readonly ILogAnalyticsMonitor m_log;
    private readonly MonitorPolicy m_policy;
    private bool m_isSending;
    private bool m_sendHasBeenQueued;
    private bool m_dataReceived;
    private ConnectivityStatus m_status;
    private int m_retryCount;
    private TimeSpan m_uptimeForNextSend;
    private TimeSpan m_uptimeForNextAutoSave;
    private TimeSpan m_uptimeForLastSend;
    private TimeSpan m_uptimeForLastSave;
    private bool m_isStarted;
    private readonly object m_lock = new object();

    internal MonitorCoordinator(
      IStatisticsContainer statisticsContainer,
      ITimer timer,
      ITransmitter transmitter,
      ILogAnalyticsMonitor log,
      MonitorPolicy policy)
    {
      this.m_statisticsContainer = statisticsContainer;
      this.m_timer = timer;
      this.m_transmitter = transmitter;
      this.m_log = log;
      this.m_policy = policy;
      this.Status = ConnectivityStatus.Unknown;
      this.m_statisticsContainer.NewDataAvailable += (EventHandler) ((s, e) => this.ScheduleSend());
      this.m_timer.Elapsed += new EventHandler(this.TimerElapsed);
    }

    private void TimerElapsed(object sender, EventArgs e)
    {
      TimeSpan uptime = Timing.Uptime;
      bool flag1;
      bool flag2;
      lock (this.m_lock)
      {
        if (!this.m_isStarted)
          return;
        flag1 = uptime >= this.m_uptimeForNextAutoSave;
        flag2 = this.m_dataReceived && uptime >= this.m_uptimeForNextSend;
      }
      if (flag1)
        this.PerformSave();
      if (flag2)
        this.PerformSend();
      this.SetupTimer(Timing.Uptime);
    }

    protected void ScheduleSend()
    {
      lock (this.m_lock)
      {
        if (!this.m_isStarted)
          return;
        this.m_dataReceived = true;
        if (this.m_sendHasBeenQueued)
          return;
        this.m_sendHasBeenQueued = true;
        if (this.m_isSending)
          return;
        this.SetupTimer(Timing.Uptime);
      }
    }

    private void SetupTimer(TimeSpan uptime)
    {
      TimeSpan storageSaveInterval = this.m_policy.RuntimeStatus.StorageSaveInterval;
      TimeSpan timeSpan1 = TimeSpan.FromMilliseconds(100.0);
      lock (this.m_lock)
      {
        this.m_uptimeForNextSend = !this.m_sendHasBeenQueued ? this.m_uptimeForLastSend.Add(MonitorCoordinator.s_24Hours) : (this.m_status == ConnectivityStatus.Disconnected ? this.m_uptimeForLastSend.Add(TimeSpan.FromSeconds((double) (int) Math.Min((double) (this.m_retryCount * this.m_retryCount) * this.m_policy.SettingsRestrictions.RetrySendInterval.Value.TotalSeconds, MonitorCoordinator.s_24Hours.TotalSeconds))) : uptime);
        this.m_uptimeForNextAutoSave = this.m_uptimeForLastSave.Add(storageSaveInterval);
        TimeSpan timeSpan2 = this.m_uptimeForNextSend.Subtract(uptime);
        if (timeSpan2 <= TimeSpan.Zero)
          timeSpan2 = timeSpan1;
        TimeSpan timeSpan3 = this.m_uptimeForNextAutoSave.Subtract(uptime);
        if (timeSpan3 <= TimeSpan.Zero)
          timeSpan3 = timeSpan1;
        this.m_timer.SetTimeout(timeSpan2 < timeSpan3 ? timeSpan2 : timeSpan3);
      }
    }

    protected void PerformSave()
    {
      this.m_uptimeForLastSave = Timing.Uptime;
      this.m_statisticsContainer.Save();
    }

    protected void PerformSend() => this.PerformSend(false, 0);

    private void PerformSend(bool isStopMessage, int waitInMilliseconds)
    {
      lock (this.m_lock)
      {
        int statisticsVersion = this.m_statisticsContainer.GetStatisticsVersion();
        if (statisticsVersion < 0)
          return;
        TimeSpan uptime = Timing.Uptime;
        if (!isStopMessage && this.m_isSending)
        {
          this.m_sendHasBeenQueued = true;
          return;
        }
        if (statisticsVersion <= this.m_policy.RuntimeStatus.SyncedVersion)
        {
          this.m_sendHasBeenQueued = false;
          return;
        }
        this.PerformSave();
        this.m_sendHasBeenQueued = false;
        this.m_isSending = true;
        this.m_uptimeForLastSend = uptime;
      }
      StatisticsData sendingStatistics = this.m_statisticsContainer.GetStatisticsToSend();
      this.m_transmitter.Send(sendingStatistics, waitInMilliseconds, (Action<SendResult>) (sendResult =>
      {
        lock (this.m_lock)
        {
          try
          {
            switch (sendResult)
            {
              case SendResult.Success:
                this.m_statisticsContainer.RegisterStatisticsSend(sendingStatistics);
                this.PerformSave();
                this.Status = ConnectivityStatus.Connected;
                this.m_retryCount = 0;
                break;
              case SendResult.Failure:
                this.m_sendHasBeenQueued = true;
                this.Status = ConnectivityStatus.Disconnected;
                ++this.m_retryCount;
                break;
            }
            this.m_isSending = false;
            this.SetupTimer(Timing.Uptime);
          }
          catch (Exception ex)
          {
            this.m_log.LogError("Failure to handle Send callback: " + ex.Message);
          }
        }
      }));
    }

    public void Start()
    {
      this.m_isStarted = true;
      this.m_isSending = false;
      this.m_sendHasBeenQueued = false;
      this.m_dataReceived = false;
      this.Status = ConnectivityStatus.Unknown;
      this.m_retryCount = 0;
      this.m_uptimeForLastSend = Timing.Uptime;
      this.m_uptimeForLastSave = Timing.Uptime;
      this.m_statisticsContainer.StartSession();
    }

    public void Stop(int waitInMilliseconds)
    {
      this.m_isStarted = false;
      this.m_dataReceived = false;
      this.m_statisticsContainer.EndSession();
      if (this.m_policy.RuntimeStatus.AutoSync)
        this.PerformSend(true, waitInMilliseconds);
      else
        this.PerformSave();
    }

    public ConnectivityStatus Status
    {
      get => this.m_status;
      private set => this.m_status = value;
    }

    public void Dispose()
    {
      if (this.m_timer == null)
        return;
      this.m_timer.Dispose();
    }
  }
}
