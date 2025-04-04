// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.AnalyticsMonitor
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using EQATEC.Analytics.Monitor.Policy;
using System;
using System.Collections.Generic;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  internal class AnalyticsMonitor : IAnalyticsMonitor, IDisposable
  {
    private readonly IMonitorCoordinator m_coordinator;
    private readonly object m_lock = new object();
    private readonly IStatisticsMonitor m_statisticsMonitor;
    private readonly MonitorPolicy m_policy;
    private readonly LogAnalyticsMonitorImpl m_log;
    private readonly ArgumentChecker m_argChecker;
    private readonly AnalyticsMonitorStatus m_status;
    private bool m_isStarted;
    private TimeSpan m_uptimeForStart;

    internal TimeSpan UptimeForStart => this.m_uptimeForStart;

    internal bool IsStarted => this.m_isStarted;

    internal AnalyticsMonitor(
      IStatisticsMonitor statisticsMonitor,
      IMonitorCoordinator coordinator,
      ILogAnalyticsMonitor log,
      MonitorPolicy policy)
    {
      this.m_coordinator = Guard.IsNotNull<IMonitorCoordinator>(coordinator, nameof (coordinator));
      this.m_policy = Guard.IsNotNull<MonitorPolicy>(policy, nameof (policy));
      this.m_statisticsMonitor = Guard.IsNotNull<IStatisticsMonitor>(statisticsMonitor, nameof (statisticsMonitor));
      this.m_log = new LogAnalyticsMonitorImpl(Guard.IsNotNull<ILogAnalyticsMonitor>(log, nameof (log)));
      this.m_argChecker = new ArgumentChecker(this.m_statisticsMonitor, this.m_log, this.m_policy);
      this.m_status = new AnalyticsMonitorStatus(this, log, policy, this.m_argChecker);
    }

    public void Start()
    {
      try
      {
        if (this.m_isStarted)
        {
          this.m_log.LogMessageF("{0} ignored: monitor has already been started", (object) "IAnalyticsMonitor.Start");
        }
        else
        {
          this.m_coordinator.Start();
          this.m_isStarted = true;
          this.m_uptimeForStart = Timing.Uptime;
          this.m_log.LogMessageF("{0}: monitor started", (object) "IAnalyticsMonitor.Start");
        }
      }
      catch (Exception ex)
      {
        this.m_argChecker.LogInternalError("IAnalyticsMonitor.Start", ex);
      }
    }

    public void Stop() => this.Stop(TimeSpan.FromSeconds(2.0));

    public void Stop(TimeSpan waitForCompletion)
    {
      try
      {
        if (this.IsNotStarted("IAnalyticsMonitor.Stop"))
          return;
        int waitInMilliseconds = Math.Min((int) TimeSpan.FromSeconds(30.0).TotalMilliseconds, Math.Max(0, (int) waitForCompletion.TotalMilliseconds));
        this.m_isStarted = false;
        this.m_coordinator.Stop(waitInMilliseconds);
        this.m_log.LogMessageF("{0}: monitor stopped", (object) "IAnalyticsMonitor.Stop");
      }
      catch (Exception ex)
      {
        this.m_argChecker.LogInternalError("IAnalyticsMonitor.Stop", ex);
      }
    }

    public void TrackException(Exception exception)
    {
      try
      {
        if (this.m_argChecker.IsNullOrInvalidObject("IAnalyticsMonitor.TrackException", nameof (exception), (object) exception))
          return;
        this.TrackException(exception, (string) null);
      }
      catch (Exception ex)
      {
        this.m_argChecker.LogInternalError("IAnalyticsMonitor.TrackException", ex);
      }
    }

    public void TrackException(Exception exception, string contextMessage)
    {
      try
      {
        if (this.IsNotStarted("IAnalyticsMonitor.TrackException") || this.m_argChecker.IsBlocked("IAnalyticsMonitor.TrackException", "Exceptions", this.m_policy.DataTypeRestrictions.Exceptions) || this.m_argChecker.IsNullOrInvalidObject("IAnalyticsMonitor.TrackException", nameof (exception), (object) exception) || this.m_argChecker.IsInvalidObject("IAnalyticsMonitor.TrackException", nameof (contextMessage), (object) contextMessage))
          return;
        string message = this.m_argChecker.Truncate("IAnalyticsMonitor.TrackException", nameof (contextMessage), contextMessage, this.m_policy.SettingsRestrictions.MaxExceptionExtraInfo.Value);
        this.m_statisticsMonitor.AddException(exception, message);
      }
      catch (Exception ex)
      {
        this.m_argChecker.LogInternalError("IAnalyticsMonitor.TrackException", ex);
      }
    }

    public void TrackException(
      Exception exception,
      string contextMessageFormat,
      params object[] args)
    {
      try
      {
        contextMessageFormat = this.m_argChecker.Format("IAnalyticsMonitor.TrackException", nameof (contextMessageFormat), contextMessageFormat, args);
        contextMessageFormat = this.m_argChecker.Truncate("IAnalyticsMonitor.TrackException", nameof (contextMessageFormat), contextMessageFormat, this.m_policy.SettingsRestrictions.MaxExceptionExtraInfo.Value);
        this.TrackException(exception, contextMessageFormat);
      }
      catch (Exception ex)
      {
        this.m_argChecker.LogInternalError("IAnalyticsMonitor.TrackException", ex);
      }
    }

    public void TrackFeature(string featureName)
    {
      try
      {
        if (this.IsNotStarted("IAnalyticsMonitor.TrackFeature") || this.m_argChecker.IsBlocked("IAnalyticsMonitor.TrackFeature", "FeatureUsage", this.m_policy.DataTypeRestrictions.FeatureUsages) || this.m_argChecker.IsInvalidFeatureName("IAnalyticsMonitor.TrackFeature", featureName))
          return;
        this.m_statisticsMonitor.AddFeatures(new string[1]
        {
          featureName
        });
      }
      catch (Exception ex)
      {
        this.m_argChecker.LogInternalError("IAnalyticsMonitor.TrackFeature", ex);
      }
    }

    public void TrackFeatures(string[] featureNames)
    {
      try
      {
        if (this.IsNotStarted("IAnalyticsMonitor.TrackFeatures") || featureNames == null || this.m_argChecker.IsBlocked("IAnalyticsMonitor.TrackFeatures", "FeatureUsage", this.m_policy.DataTypeRestrictions.FeatureUsages))
          return;
        List<string> stringList = new List<string>();
        foreach (string featureName in featureNames)
        {
          if (!this.m_argChecker.IsInvalidFeatureName("IAnalyticsMonitor.TrackFeatures", featureName))
            stringList.Add(featureName);
        }
        this.m_statisticsMonitor.AddFeatures(stringList.ToArray());
      }
      catch (Exception ex)
      {
        this.m_argChecker.LogInternalError("IAnalyticsMonitor.TrackFeatures", ex);
      }
    }

    public TimingScope TrackFeatureStart(string featureName)
    {
      try
      {
        if (this.IsNotStarted("IAnalyticsMonitor.TrackFeatureStart"))
          return new TimingScope(featureName, (IAnalyticsMonitor) null);
        if (this.m_argChecker.IsBlocked("IAnalyticsMonitor.TrackFeatureStart", "FeatureTiming", this.m_policy.DataTypeRestrictions.FeatureTiming))
          return new TimingScope(featureName, (IAnalyticsMonitor) null);
        if (this.m_argChecker.IsInvalidFeatureName("IAnalyticsMonitor.TrackFeatureStart", featureName))
          return new TimingScope(featureName, (IAnalyticsMonitor) null);
        if (this.m_statisticsMonitor.AddFeatureStart(featureName))
          return new TimingScope(featureName, (IAnalyticsMonitor) this);
        this.m_log.LogMessageF("{0}: unable to start feature '{1}'", (object) "IAnalyticsMonitor.TrackFeatureStart", (object) featureName);
      }
      catch (Exception ex)
      {
        this.m_argChecker.LogInternalError("IAnalyticsMonitor.TrackFeatureStart", ex);
      }
      return new TimingScope(featureName, (IAnalyticsMonitor) null);
    }

    public TimeSpan TrackFeatureStop(string featureName)
    {
      try
      {
        if (this.IsNotStarted("IAnalyticsMonitor.TrackFeatureStop") || this.m_argChecker.IsBlocked("IAnalyticsMonitor.TrackFeatureStop", "FeatureTiming", this.m_policy.DataTypeRestrictions.FeatureTiming) || this.m_argChecker.IsInvalidFeatureName("IAnalyticsMonitor.TrackFeatureStop", featureName))
          return TimeSpan.Zero;
        TimeSpan timeSpent;
        if (!this.m_statisticsMonitor.AddFeatureStop(featureName, out timeSpent))
          this.m_log.LogMessageF("{0}: unable to stop feature '{1}'", (object) "IAnalyticsMonitor.TrackFeatureStop", (object) featureName);
        return timeSpent;
      }
      catch (Exception ex)
      {
        this.m_argChecker.LogInternalError("IAnalyticsMonitor.TrackFeatureStop", ex);
        return TimeSpan.Zero;
      }
    }

    public void TrackFeatureCancel(string featureName)
    {
      try
      {
        if (this.IsNotStarted("IAnalyticsMonitor.TrackFeatureCancel") || this.m_argChecker.IsBlocked("IAnalyticsMonitor.TrackFeatureCancel", "FeatureTiming", this.m_policy.DataTypeRestrictions.FeatureTiming) || this.m_argChecker.IsInvalidFeatureName("IAnalyticsMonitor.TrackFeatureCancel", featureName))
          return;
        this.m_statisticsMonitor.FeatureCancel(featureName);
      }
      catch (Exception ex)
      {
        this.m_argChecker.LogInternalError("IAnalyticsMonitor.TrackFeatureCancel", ex);
      }
    }

    public void TrackFeatureValue(string featureName, long trackedValue)
    {
      try
      {
        if (this.IsNotStarted("IAnalyticsMonitor.TrackFeatureValue") || this.m_argChecker.IsBlocked("IAnalyticsMonitor.TrackFeatureValue", "FeatureValues", this.m_policy.DataTypeRestrictions.FeatureValues) || this.m_argChecker.IsInvalidFeatureName("IAnalyticsMonitor.TrackFeatureValue", featureName))
          return;
        this.m_statisticsMonitor.AddFeatureValue(featureName, trackedValue);
      }
      catch (Exception ex)
      {
        this.m_argChecker.LogInternalError("IAnalyticsMonitor.TrackFeatureValue", ex);
      }
    }

    public void ForceSync()
    {
      try
      {
        if (this.IsNotStarted("IAnalyticsMonitor.ForceSync") || this.m_argChecker.IsBlocked("IAnalyticsMonitor.ForceSync", "Session", this.m_policy.DataTypeRestrictions.Sessions))
          return;
        this.m_statisticsMonitor.Sync();
      }
      catch (Exception ex)
      {
        this.m_argChecker.LogInternalError("IAnalyticsMonitor.ForceSync", ex);
      }
    }

    public void SetInstallationInfo(string installationId)
    {
      try
      {
        this.SetInstallationInfo(installationId, (IDictionary<string, string>) null);
      }
      catch (Exception ex)
      {
        this.m_argChecker.LogInternalError("IAnalyticsMonitor.SetInstallationInfo", ex);
      }
    }

    public void SetInstallationInfo(
      string installationId,
      IDictionary<string, string> installationProperties)
    {
      try
      {
        if (this.m_argChecker.IsNullOrInvalidOrEmpty("IAnalyticsMonitor.SetInstallationInfo", nameof (installationId), installationId))
          return;
        string installationID = this.m_argChecker.Truncate("IAnalyticsMonitor.SetInstallationInfo", nameof (installationId), installationId, this.m_policy.SettingsRestrictions.MaxInstallationIDSize.Value);
        IDictionary<string, string> installationProperties1 = (IDictionary<string, string>) new Dictionary<string, string>();
        if (installationProperties != null)
        {
          int num1 = installationProperties.Count;
          if (num1 > 0)
          {
            int num2 = this.m_policy.SettingsRestrictions.MaxInstallationProperties.Value;
            if (num1 > num2)
            {
              this.m_log.LogErrorF("{0}: {1} installation properties passed but limit is {2}; remaining will be ignored", (object) "IAnalyticsMonitor.SetInstallationInfo", (object) num1, (object) num2);
              num1 = num2;
            }
            IList<string> stringList = (IList<string>) new List<string>((IEnumerable<string>) installationProperties.Keys);
            for (int index = 0; index < num1; ++index)
            {
              string key = this.m_argChecker.Truncate("IAnalyticsMonitor.SetInstallationInfo", "key", stringList[index], this.m_policy.SettingsRestrictions.MaxInstallationPropertyKeySize.Value);
              string str = installationProperties[key] ?? "";
              installationProperties1[key] = str;
            }
          }
        }
        this.m_statisticsMonitor.SetInstallationInfo(installationID, installationProperties1);
      }
      catch (Exception ex)
      {
        this.m_argChecker.LogInternalError("IAnalyticsMonitor.SetInstallationInfo", ex);
      }
    }

    public void SetLicenseInfo(LicenseInfo[] licenseInfos)
    {
      try
      {
        if (licenseInfos == null || licenseInfos.Length == 0)
          return;
        List<LicenseInfo> licenseInfoList = new List<LicenseInfo>();
        foreach (LicenseInfo licenseInfo in licenseInfos)
        {
          if (!this.m_argChecker.IsNullOrInvalidOrEmpty("IAnalyticsMonitor.SetLicenseInfo", "licenseId", licenseInfo.LicenseIdentifier))
          {
            string licenseIdentifier = this.m_argChecker.Truncate("IAnalyticsMonitor.SetLicenseInfo", "licenseId", licenseInfo.LicenseIdentifier, this.m_policy.SettingsRestrictions.MaxLicenseIDSize.Value);
            string licenseType = this.m_argChecker.Truncate("IAnalyticsMonitor.SetLicenseInfo", "licenseType", licenseInfo.LicenseType, this.m_policy.SettingsRestrictions.MaxLicenseTypeSize.Value);
            licenseInfoList.Add(new LicenseInfo(licenseIdentifier, licenseType));
          }
        }
        this.m_statisticsMonitor.SetLicenseInfo(licenseInfoList.ToArray());
      }
      catch (Exception ex)
      {
        this.m_argChecker.LogInternalError("IAnalyticsMonitor.SetLicenseInfo", ex);
      }
    }

    public AnalyticsMonitorStatus Status => this.m_status;

    public void TrackFlow(string flowName, string waypoint)
    {
      try
      {
        if (this.m_argChecker.IsInvalidFlowName(flowName) || this.m_argChecker.IsInvalidWaypoint(waypoint))
          return;
        this.m_statisticsMonitor.AddFlowWaypoint(flowName, waypoint);
      }
      catch (Exception ex)
      {
        this.m_argChecker.LogInternalError("IAnalyticsMonitor.TrackFlow", ex);
      }
    }

    public void TrackFlowGoal(string flowName, string goalName)
    {
      try
      {
        if (this.m_argChecker.IsInvalidFlowName(flowName))
          return;
        this.m_statisticsMonitor.AddFlowGoal(flowName, goalName ?? "");
      }
      catch (Exception ex)
      {
        this.m_argChecker.LogInternalError("IAnalyticsMonitor.TrackFlowGoal", ex);
      }
    }

    public void ResetFlow(string flowName)
    {
      try
      {
        if (this.m_argChecker.IsInvalidFlowName(flowName))
          return;
        this.m_statisticsMonitor.ResetFlow(flowName);
      }
      catch (Exception ex)
      {
        this.m_argChecker.LogInternalError("IAnalyticsMonitor.ResetFlow", ex);
      }
    }

    void IDisposable.Dispose()
    {
      try
      {
        if (this.m_isStarted)
          this.Stop();
        this.m_coordinator.Dispose();
        this.m_statisticsMonitor.Dispose();
      }
      catch (Exception ex)
      {
        this.m_log.LogMessageF("{0}: Encountered errors during dispose: {1}", (object) "IAnalyticsMonitor.Dispose", (object) ex.Message);
      }
    }

    private bool IsNotStarted(string callerId)
    {
      if (this.m_isStarted)
        return false;
      this.m_log.LogMessageF("{0} is ignored: IAnalyticsMonitor has not yet been started", (object) callerId);
      return true;
    }
  }
}
