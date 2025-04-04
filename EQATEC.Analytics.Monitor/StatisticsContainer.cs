// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.StatisticsContainer
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using EQATEC.Analytics.Monitor.Model;
using EQATEC.Analytics.Monitor.Policy;
using EQATEC.Analytics.Monitor.Storage;
using System;
using System.Collections.Generic;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  internal class StatisticsContainer : IStatisticsMonitor, IDisposable, IStatisticsContainer
  {
    private readonly IStorageFactory m_storageFactory;
    private readonly object m_lock = new object();
    private readonly ILogAnalyticsMonitor m_log;
    private readonly MonitorPolicy m_policy;
    private Statistics m_liveStatistics;
    private bool m_isBlocked;
    private bool m_isForceSync;

    public event EventHandler NewDataAvailable;

    internal StatisticsContainer(
      IStorageFactory storageFactory,
      ILogAnalyticsMonitor log,
      MonitorPolicy policy)
    {
      this.m_storageFactory = Guard.IsNotNull<IStorageFactory>(storageFactory, nameof (storageFactory));
      this.m_log = Guard.IsNotNull<ILogAnalyticsMonitor>(log, nameof (log));
      this.m_policy = Guard.IsNotNull<MonitorPolicy>(policy, nameof (policy));
      this.m_policy.Changed += (EventHandler) ((s, e) => this.SavePolicy());
      this.m_liveStatistics = new Statistics();
    }

    public void Dispose() => this.m_storageFactory.Dispose();

    public void StartSession()
    {
      lock (this.m_lock)
      {
        if (this.HasActiveSession)
        {
          this.m_log.LogError("StartSession ignored because a session is already active");
          return;
        }
        this.m_policy.RuntimeStatus.Reset();
        Exception ex = this.LoadFromStorage();
        List<Session> sessions = this.m_liveStatistics.Sessions;
        int num = 0;
        while (sessions.Count > 0 && sessions.Count >= this.m_policy.SettingsRestrictions.MaxSessions.Value)
        {
          sessions.RemoveAt(0);
          ++num;
        }
        if (num > 0)
          this.m_log.LogMessage(string.Format("Deleted {0} old {1} from the queue to make room for the new", (object) num, num == 1 ? (object) "session" : (object) "sessions"));
        this.m_isBlocked = this.m_policy.DataTypeRestrictions.Sessions.IsBlocking(Timing.Uptime);
        ++this.m_policy.RuntimeStatus.SessionStartCount;
        if (this.m_isBlocked)
        {
          this.m_log.LogMessage("Monitoring sessions are blocked. " + this.m_policy.DataTypeRestrictions.Sessions.GetBlockingDescription());
        }
        else
        {
          ++this.m_liveStatistics.Version;
          this.m_liveStatistics.StartSession(this.m_policy.RuntimeStatus.InstallationSettings, this.m_policy.RuntimeStatus.CurrentApplicationVersion);
          this.m_liveStatistics.CurrentSession.StartCount = this.m_policy.RuntimeStatus.SessionStartCount;
          this.m_liveStatistics.CurrentSession.InternalAnalytics.TrackException("LoadStorage", ex);
        }
        this.SavePolicy();
        this.SaveStatistics();
        this.Save();
      }
      this.OnNewDataAvailable(false);
    }

    private bool HasActiveSession
    {
      get => this.m_liveStatistics != null && this.m_liveStatistics.CurrentSession != null;
    }

    private void OnNewDataAvailable(bool isManualSync)
    {
      if (this.m_isBlocked || !isManualSync && !this.m_policy.RuntimeStatus.AutoSync)
        return;
      EventHandler newDataAvailable = this.NewDataAvailable;
      if (newDataAvailable == null)
        return;
      newDataAvailable((object) this, EventArgs.Empty);
    }

    public void EndSession()
    {
      lock (this.m_lock)
      {
        if (!this.HasActiveSession)
          return;
        this.UpdateRuntime(true);
        this.m_liveStatistics.EndSession();
        ++this.m_liveStatistics.Version;
        this.m_policy.RuntimeStatus.InstallationSettings.LicenseInfo.Clear();
      }
    }

    public void AddFeatures(string[] featureNames)
    {
      lock (this.m_lock)
      {
        if (!this.HasActiveSession || featureNames == null || featureNames.Length == 0)
          return;
        foreach (string featureName in featureNames)
          ++this.GetFeatureByName(featureName, true).SessionHit;
        ++this.m_liveStatistics.Version;
      }
    }

    public void AddFlowWaypoint(string flowName, string waypointName)
    {
      lock (this.m_lock)
      {
        if (!this.HasActiveSession)
          return;
        TimeSpan uptime = Timing.Uptime;
        Flow flow;
        if (!this.m_liveStatistics.CurrentSession.Flows.TryGetValue(flowName, out flow))
        {
          if (this.m_liveStatistics.CurrentSession.Flows.Count >= this.m_policy.SettingsRestrictions.MaxFlowsInSession.Value)
          {
            this.m_log.LogError(string.Format("Unable to add a new flow. Already have {0} uniquely named flows", (object) this.m_liveStatistics.CurrentSession.Flows.Count));
            return;
          }
          TimeSpan startTime = this.m_liveStatistics.CurrentSession.StartTime;
          flow = new Flow(flowName, startTime, this.m_policy.SettingsRestrictions);
          this.m_liveStatistics.CurrentSession.Flows.Add(flowName, flow);
        }
        flow.AddWayPoint(waypointName, uptime);
        ++this.m_liveStatistics.Version;
      }
    }

    public void AddFlowGoal(string flowName, string goalName)
    {
      lock (this.m_lock)
      {
        Flow flow;
        if (!this.HasActiveSession || !this.m_liveStatistics.CurrentSession.Flows.TryGetValue(flowName, out flow))
          return;
        flow.AddGoal(goalName, Timing.Uptime);
        ++this.m_liveStatistics.Version;
      }
    }

    public void ResetFlow(string flowName)
    {
      lock (this.m_lock)
      {
        if (!this.HasActiveSession)
          return;
        TimeSpan uptime = Timing.Uptime;
        Flow flow;
        if (!this.m_liveStatistics.CurrentSession.Flows.TryGetValue(flowName, out flow))
          return;
        flow.Reset(uptime);
      }
    }

    public bool AddFeatureStart(string featureName)
    {
      lock (this.m_lock)
      {
        if (!this.HasActiveSession)
          return false;
        Feature featureByName = this.GetFeatureByName(featureName, true);
        if (featureByName.IsActive)
          return false;
        featureByName.StartTiming(Timing.Uptime);
      }
      return true;
    }

    public bool AddFeatureStop(string featureName, out TimeSpan timeSpent)
    {
      timeSpent = TimeSpan.Zero;
      bool flag1 = false;
      bool flag2 = false;
      lock (this.m_lock)
      {
        if (!this.HasActiveSession)
          return false;
        Feature featureByName = this.GetFeatureByName(featureName, false);
        if (featureByName == null || !featureByName.IsActive)
          return false;
        TimeSpan timeSpan = featureByName.StopTiming(Timing.Uptime);
        if (timeSpan > TimeSpan.Zero)
        {
          flag1 = this.AddFeatureValueItem(new FeatureValue(featureName, FeatureValueType.Timing, (long) timeSpan.TotalMilliseconds, Timing.Uptime - this.m_liveStatistics.CurrentSession.StartTime));
          flag2 = true;
        }
        timeSpent = timeSpan;
      }
      if (flag1)
        this.OnNewDataAvailable(false);
      return flag2;
    }

    private bool AddFeatureValueItem(FeatureValue newFeatureValue)
    {
      int num = 0;
      List<FeatureValue> featureValues = this.m_liveStatistics.CurrentSession.FeatureValues;
      while (featureValues.Count > 0 && featureValues.Count >= this.m_policy.SettingsRestrictions.MaxSessionFeatureValues.Value)
      {
        featureValues.RemoveAt(0);
        ++num;
      }
      if (num > 0)
        this.m_log.LogMessage(string.Format("There has been deleted {0} old {1} from the queue to make room for the new", (object) num, num == 1 ? (object) "feature" : (object) "features"));
      featureValues.Add(newFeatureValue);
      ++this.m_liveStatistics.Version;
      return featureValues.Count > this.m_policy.SettingsRestrictions.MaxSessionFeatureValues.Value / 2;
    }

    public void FeatureCancel(string featureName)
    {
      lock (this.m_lock)
      {
        if (!this.HasActiveSession)
          return;
        Feature featureByName = this.GetFeatureByName(featureName, false);
        if (featureByName == null || !featureByName.IsActive)
          return;
        featureByName.StopTiming(Timing.Uptime);
      }
    }

    public void AddFeatureValue(string featureName, long value)
    {
      bool flag;
      lock (this.m_lock)
      {
        if (!this.HasActiveSession)
          return;
        flag = this.AddFeatureValueItem(new FeatureValue(featureName, FeatureValueType.Value, value, Timing.Uptime - this.m_liveStatistics.CurrentSession.StartTime));
      }
      if (!flag)
        return;
      this.OnNewDataAvailable(false);
    }

    public void AddException(Exception exception, string message)
    {
      lock (this.m_lock)
      {
        if (!this.HasActiveSession)
          return;
        int num = 0;
        List<ExceptionEntry> exceptions = this.m_liveStatistics.CurrentSession.Exceptions;
        while (exceptions.Count > 0 && exceptions.Count >= this.m_policy.SettingsRestrictions.MaxSessionExceptions.Value)
        {
          exceptions.RemoveAt(0);
          ++num;
        }
        if (num > 0)
          this.m_log.LogMessage(string.Format("There has been deleted {0} old {1} from the queue to make room for the new", (object) num, num == 1 ? (object) nameof (exception) : (object) "exceptions"));
        ExceptionEntry exceptionEntry = ExceptionEntry.Create(Timing.Uptime - this.m_liveStatistics.CurrentSession.StartTime, exception, message, this.m_policy.SettingsRestrictions, this.m_log);
        if (exceptionEntry != null)
        {
          exceptions.Add(exceptionEntry);
          ++this.m_liveStatistics.Version;
        }
      }
      this.OnNewDataAvailable(false);
    }

    public void AddExceptionRawMessage(
      string type,
      string reason,
      string stacktrace,
      string message)
    {
      lock (this.m_lock)
      {
        if (!this.HasActiveSession)
          return;
        int num = 0;
        List<ExceptionEntry> exceptions = this.m_liveStatistics.CurrentSession.Exceptions;
        while (exceptions.Count > 0 && exceptions.Count >= this.m_policy.SettingsRestrictions.MaxSessionExceptions.Value)
        {
          exceptions.RemoveAt(0);
          ++num;
        }
        if (num > 0)
          this.m_log.LogMessage(string.Format("There has been deleted {0} old {1} from the queue to make room for the new", (object) num, num == 1 ? (object) "exception" : (object) "exceptions"));
        exceptions.Add(new ExceptionEntry(Timing.Uptime - this.m_liveStatistics.CurrentSession.StartTime, reason, type, message, stacktrace, (ExceptionEntry) null, true));
        ++this.m_liveStatistics.Version;
      }
      this.OnNewDataAvailable(false);
    }

    public void SetInstallationInfo(
      string installationID,
      IDictionary<string, string> installationProperties)
    {
      lock (this.m_lock)
      {
        this.m_policy.RuntimeStatus.InstallationSettings.Update(installationID, installationProperties, this.m_policy, this.m_log);
        ++this.m_liveStatistics.Version;
      }
    }

    public void SetLicenseInfo(LicenseInfo[] licenseInfos)
    {
      lock (this.m_lock)
      {
        int num = this.m_policy.SettingsRestrictions.MaxNumberOfLicenseInfos.Value;
        foreach (LicenseInfo licenseInfo in licenseInfos)
        {
          if (this.m_policy.RuntimeStatus.InstallationSettings.LicenseInfo.Count >= num)
          {
            this.m_log.LogMessage(string.Format("Can only add {0} LicenseInfo instances to the monitor", (object) num));
            break;
          }
          this.m_policy.RuntimeStatus.InstallationSettings.LicenseInfo[licenseInfo.LicenseIdentifier] = licenseInfo;
        }
      }
    }

    public void Sync()
    {
      lock (this.m_lock)
      {
        if (!this.HasActiveSession)
          return;
      }
      TimeSpan uptime = Timing.Uptime;
      TimeSpan forLastForceSync = this.m_policy.RuntimeStatus.UptimeForLastForceSync;
      if (uptime >= forLastForceSync && uptime.Subtract(forLastForceSync).TotalSeconds < (double) this.m_policy.SettingsRestrictions.MinSecondsBetweenForceSync.Value)
      {
        this.m_log.LogMessage(string.Format("Cannot force a sync at this time. There needs to be at least {0} seconds between Sync calls", (object) this.m_policy.SettingsRestrictions.MinSecondsBetweenForceSync.Value));
      }
      else
      {
        this.m_policy.RuntimeStatus.UptimeForLastForceSync = uptime;
        this.m_isForceSync = true;
        this.OnNewDataAvailable(true);
      }
    }

    private Feature GetFeatureByName(string featureName, bool create)
    {
      if (this.m_liveStatistics.CurrentSession.Features.ContainsKey(featureName))
        return this.m_liveStatistics.CurrentSession.Features[featureName];
      return create ? (this.m_liveStatistics.CurrentSession.Features[featureName] = new Feature()) : (Feature) null;
    }

    private Exception LoadFromStorage()
    {
      Exception exception = (Exception) null;
      try
      {
        lock (this.m_lock)
        {
          Statistics statistics = this.m_storageFactory.LoadFromStorage(this.m_policy);
          if (statistics != null)
          {
            this.m_liveStatistics = statistics;
            return exception;
          }
        }
      }
      catch (Exception ex)
      {
        this.m_log.LogError("Failed to load the existing statistics, resetting monitor data: " + ex.Message);
        exception = ex;
      }
      this.m_liveStatistics = new Statistics();
      return exception;
    }

    public void Save()
    {
      try
      {
        lock (this.m_lock)
        {
          this.UpdateRuntime(false);
          this.m_storageFactory.SaveSessions(this.m_liveStatistics, this.m_policy.Copy());
        }
      }
      catch (Exception ex)
      {
        this.m_log.LogError("Failed to save the current state: " + ex.Message);
        if (this.m_liveStatistics == null || this.m_liveStatistics.CurrentSession == null)
          return;
        this.m_liveStatistics.CurrentSession.InternalAnalytics.TrackException("SaveStatistics", ex);
      }
    }

    public void SavePolicy()
    {
      try
      {
        lock (this.m_lock)
          this.m_storageFactory.SavePolicy(this.m_policy);
      }
      catch (Exception ex)
      {
        this.m_log.LogError("Failed to save the current policy state: " + ex.Message);
        if (this.m_liveStatistics == null || this.m_liveStatistics.CurrentSession == null)
          return;
        this.m_liveStatistics.CurrentSession.InternalAnalytics.TrackException(nameof (SavePolicy), ex);
      }
    }

    internal void SaveStatistics()
    {
      try
      {
        lock (this.m_lock)
        {
          this.UpdateRuntime(false);
          this.m_storageFactory.SaveStatistics(this.m_policy);
        }
      }
      catch (Exception ex)
      {
        this.m_log.LogError("Failed to save the current statistics state: " + ex.Message);
        if (this.m_liveStatistics == null || this.m_liveStatistics.CurrentSession == null)
          return;
        this.m_liveStatistics.CurrentSession.InternalAnalytics.TrackException("SaveStatisticsMetaData", ex);
      }
    }

    public int GetStatisticsVersion()
    {
      lock (this.m_lock)
        return this.m_liveStatistics == null || this.m_isBlocked ? -1 : this.m_liveStatistics.Version;
    }

    public StatisticsData GetStatisticsToSend()
    {
      lock (this.m_lock)
      {
        if (this.m_liveStatistics == null)
          return (StatisticsData) null;
        this.UpdateRuntime(false);
        return new StatisticsData(this.m_liveStatistics.CreateSnapshotCopy(), this.m_isForceSync);
      }
    }

    public void RegisterStatisticsSend(StatisticsData statisticsSend)
    {
      lock (this.m_lock)
      {
        if (this.m_liveStatistics == null)
          return;
        if (statisticsSend.IsForceSync)
          this.m_isForceSync = false;
        this.m_liveStatistics.SubtractSnapshotCopy(statisticsSend.Statistics);
      }
    }

    private void UpdateRuntime(bool endSession)
    {
      lock (this.m_lock)
      {
        Session currentSession = this.m_liveStatistics.CurrentSession;
        if (currentSession == null)
          return;
        TimeSpan timeSpan = Timing.Uptime - currentSession.StartTime;
        if (timeSpan.TotalMilliseconds > 0.0)
          currentSession.Runtime = timeSpan;
        if (!endSession)
          return;
        foreach (KeyValuePair<string, Feature> feature in currentSession.Features)
        {
          if (feature.Value.IsActive)
            this.FeatureCancel(feature.Key);
        }
        foreach (KeyValuePair<string, Flow> flow in currentSession.Flows)
          this.AddFlowWaypoint(flow.Key, "");
      }
    }

    public void SelfAnalyticsTrackException(string callerId, Exception ex)
    {
      lock (this.m_lock)
        this.m_liveStatistics.CurrentSession?.InternalAnalytics.TrackException(callerId, ex);
    }

    public void SelfAnalyticsTrackExceptionMessage(string callerId, string info)
    {
      lock (this.m_lock)
        this.m_liveStatistics.CurrentSession?.InternalAnalytics.TrackExceptionMessage(callerId, info);
    }
  }
}
