// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.ArgumentChecker
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using EQATEC.Analytics.Monitor.Policy;
using System;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  internal class ArgumentChecker : ILogAnalyticsMonitor
  {
    private readonly IStatisticsMonitor m_statisticsMonitor;
    private readonly LogAnalyticsMonitorImpl m_log;
    private readonly MonitorPolicy m_policy;

    public ArgumentChecker()
    {
      this.m_statisticsMonitor = (IStatisticsMonitor) null;
      this.m_log = new LogAnalyticsMonitorImpl((ILogAnalyticsMonitor) this);
      this.m_policy = (MonitorPolicy) null;
    }

    public ArgumentChecker(
      IStatisticsMonitor statisticsMonitor,
      LogAnalyticsMonitorImpl log,
      MonitorPolicy policy)
    {
      this.m_statisticsMonitor = Guard.IsNotNull<IStatisticsMonitor>(statisticsMonitor, nameof (statisticsMonitor));
      this.m_log = Guard.IsNotNull<LogAnalyticsMonitorImpl>(log, nameof (log));
      this.m_policy = Guard.IsNotNull<MonitorPolicy>(policy, nameof (policy));
    }

    public void LogInternalError(string callerId, Exception ex)
    {
      try
      {
        string str = !string.IsNullOrEmpty(ex.Message) ? ex.Message : "unknown reason";
        this.m_log.LogErrorF("{0} failed with an internal exception: {1}", (object) callerId, (object) str);
        if (this.m_statisticsMonitor == null)
          return;
        this.m_statisticsMonitor.SelfAnalyticsTrackException(callerId, ex);
      }
      catch
      {
      }
    }

    public void LogInternalErrorMessage(string callerId, string info)
    {
      try
      {
        this.m_log.LogErrorF("{0} failed with an internal error: {1}", (object) callerId, (object) info);
        if (this.m_statisticsMonitor == null)
          return;
        this.m_statisticsMonitor.SelfAnalyticsTrackExceptionMessage(callerId, info);
      }
      catch
      {
      }
    }

    public string Truncate(string callerId, string argName, string str, int maxLength)
    {
      string input = str ?? "";
      int length = input.Length;
      if (length > maxLength)
      {
        this.m_log.LogMessageF("{0}: {1} exceeds limit and will be truncated; length is {2} chars and limit is {3} chars", (object) callerId, (object) argName, (object) length, (object) maxLength);
        input = StringUtil.Chop(input, maxLength);
      }
      return input;
    }

    public string Format(string callerId, string argName, string message, params object[] args)
    {
      try
      {
        if (args == null || args.Length == 0)
          return message;
        object[] objArray = new object[args.Length];
        for (int index = 0; index < args.Length; ++index)
        {
          try
          {
            objArray[index] = args[index] == null ? (object) "" : (object) args[index].ToString();
          }
          catch
          {
            objArray[index] = args[index] == null ? (object) "" : (object) args[index].GetType().FullName;
          }
        }
        message = string.Format(message, objArray);
      }
      catch (Exception ex)
      {
        this.m_log.LogErrorF("{0} failed: format \"{1}\" was invalid: {2}", (object) callerId, (object) (message ?? "(null)"), !string.IsNullOrEmpty(ex.Message) ? (object) ex.Message : (object) "unknown reason");
        message = (string) null;
      }
      return message;
    }

    public bool IsNullOrInvalid(string callerId, string argName, string str)
    {
      if (str == null)
      {
        this.m_log.LogErrorF("{0} failed: argument {1} may not be null", (object) callerId, (object) argName);
        return true;
      }
      return this.IsInvalidObject(callerId, argName, (object) str);
    }

    public bool IsNullOrInvalidOrEmpty(string callerId, string argName, string str)
    {
      if (this.IsNullOrInvalid(callerId, argName, str))
        return true;
      if (str.Length != 0)
        return false;
      this.m_log.LogErrorF("{0} failed: argument {1} may not be an empty string", (object) callerId, (object) argName);
      return true;
    }

    public bool IsNullOrInvalidObject(string callerId, string argName, object obj)
    {
      if (obj != null)
        return this.IsInvalidObject(callerId, argName, obj);
      this.m_log.LogErrorF("{0} failed: argument {1} may not be null", (object) callerId, (object) argName);
      return true;
    }

    public bool IsInvalidObject(string callerId, string argName, object obj) => false;

    public bool IsInvalidFeatureName(string callerId, string featureName)
    {
      if (string.IsNullOrEmpty(featureName))
        return true;
      if (this.m_policy == null)
        return false;
      int length = featureName.Length;
      int num = this.m_policy.SettingsRestrictions.MaxFeatureIdSize.Value;
      if (length <= num)
        return false;
      this.m_log.LogErrorF("{0} failed: feature name \"{1}\" is too long; length is {2} and limit is {3} chars", (object) callerId, (object) featureName, (object) length, (object) num);
      return true;
    }

    public bool IsBlocked(string callerId, string dataTypeName, BlockingRestriction restriction)
    {
      if (!restriction.IsBlocking(Timing.Uptime))
        return false;
      string blockingDescription = restriction.GetBlockingDescription();
      this.m_log.LogMessageF("{0} failed: {1} is blocked; {2}", (object) callerId, (object) dataTypeName, (object) blockingDescription);
      return true;
    }

    public bool IsInvalidProductId(string callerId, string argName, string productId)
    {
      if (this.IsNullOrInvalidOrEmpty(callerId, argName, productId))
        return true;
      try
      {
        Guid guid = new Guid(productId);
        return false;
      }
      catch
      {
      }
      this.m_log.LogErrorF("{0} failed: {1} \"{2}\" is not correctly formatted. It must consist of alphanumeric characters only, like e.g. \"B8A318C11AE34C43A1984733EF5001E1\". Please ensure that you have your valid productId as received from http://analytics.eqatec.com. If you do not have a productId for this product then you must login and retrieve it.", (object) callerId, (object) argName, (object) productId);
      return true;
    }

    void ILogAnalyticsMonitor.LogMessage(string message)
    {
    }

    void ILogAnalyticsMonitor.LogError(string errorMessage) => throw new Exception(errorMessage);

    public bool IsInvalidWaypoint(string waypoint)
    {
      if (string.IsNullOrEmpty(waypoint))
        return true;
      if (this.m_policy == null)
        return false;
      int num = this.m_policy.SettingsRestrictions.MaxWaypointNameLength.Value;
      return waypoint.Length > num;
    }

    public bool IsInvalidFlowName(string flowname)
    {
      if (string.IsNullOrEmpty(flowname))
        return true;
      if (this.m_policy == null)
        return false;
      int num = this.m_policy.SettingsRestrictions.MaxFlowNameLength.Value;
      return flowname.Length > num;
    }
  }
}
