// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.IAnalyticsMonitor
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  public interface IAnalyticsMonitor : IDisposable
  {
    void Start();

    void TrackException(Exception exception);

    void TrackException(Exception exception, string contextMessage);

    void TrackException(Exception exception, string contextMessageFormat, params object[] args);

    void TrackFeature(string featureName);

    void TrackFeatures(string[] featureNames);

    TimingScope TrackFeatureStart(string featureName);

    TimeSpan TrackFeatureStop(string featureName);

    void TrackFeatureCancel(string featureName);

    void TrackFeatureValue(string featureName, long trackedValue);

    void Stop();

    void Stop(TimeSpan waitForCompletion);

    void ForceSync();

    void SetInstallationInfo(string installationId, IDictionary<string, string> propertyDictionary);

    void SetInstallationInfo(string installationId);

    AnalyticsMonitorStatus Status { get; }
  }
}
