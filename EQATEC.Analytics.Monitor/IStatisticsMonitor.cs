// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.IStatisticsMonitor
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  internal interface IStatisticsMonitor : IDisposable
  {
    void AddFeatures(string[] featureName);

    bool AddFeatureStart(string featureName);

    bool AddFeatureStop(string featureName, out TimeSpan timeSpent);

    void AddFeatureValue(string featureName, long trackedValue);

    void FeatureCancel(string featureName);

    void AddException(Exception exception, string message);

    void AddExceptionRawMessage(
      string type,
      string reason,
      string stacktrace,
      string contextMessage);

    void Sync();

    void SetInstallationInfo(
      string installationID,
      IDictionary<string, string> installationProperties);

    void SetLicenseInfo(LicenseInfo[] licenseInfos);

    void SelfAnalyticsTrackException(string callerId, Exception ex);

    void SelfAnalyticsTrackExceptionMessage(string callerId, string info);

    void AddFlowWaypoint(string flowName, string waypoint);

    void AddFlowGoal(string flowName, string goalName);

    void ResetFlow(string flowName);
  }
}
