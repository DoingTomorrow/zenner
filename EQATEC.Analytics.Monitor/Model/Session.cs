// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Model.Session
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EQATEC.Analytics.Monitor.Model
{
  internal class Session : IEquatable<Session>
  {
    public Guid Id { get; private set; }

    public Version ApplicationVersion { get; private set; }

    public int StartCount { get; internal set; }

    public DateTime UtcStartTime { get; internal set; }

    public TimeSpan StartTime { get; internal set; }

    public TimeSpan Runtime { get; internal set; }

    public InstallationSettings InstallationSettings { get; internal set; }

    public bool Stopped { get; set; }

    public bool HasSendData { get; set; }

    public Dictionary<string, Feature> Features { get; private set; }

    public Dictionary<string, Flow> Flows { get; private set; }

    public List<ExceptionEntry> Exceptions { get; private set; }

    public List<FeatureValue> FeatureValues { get; private set; }

    public SelfAnalytics InternalAnalytics { get; private set; }

    public Session(
      Guid sessionID,
      InstallationSettings installationSettings,
      Version applicationVersion)
    {
      this.ApplicationVersion = applicationVersion;
      this.InstallationSettings = installationSettings;
      this.Id = sessionID;
      this.Features = new Dictionary<string, Feature>();
      this.Flows = new Dictionary<string, Flow>();
      this.Exceptions = new List<ExceptionEntry>();
      this.FeatureValues = new List<FeatureValue>();
      this.UtcStartTime = DateTime.MinValue;
      this.InternalAnalytics = new SelfAnalytics();
    }

    public Session CreateSnapshotCopy()
    {
      Session snapshotCopy = new Session(this.Id, this.InstallationSettings.Copy(), this.ApplicationVersion)
      {
        Id = this.Id,
        StartCount = this.StartCount,
        StartTime = this.StartTime,
        Runtime = this.Runtime,
        Stopped = this.Stopped,
        HasSendData = this.HasSendData,
        UtcStartTime = this.UtcStartTime,
        InternalAnalytics = this.InternalAnalytics.Copy()
      };
      foreach (KeyValuePair<string, Feature> feature in this.Features)
        snapshotCopy.Features.Add(feature.Key, feature.Value.Copy());
      foreach (KeyValuePair<string, Flow> flow in this.Flows)
        snapshotCopy.Flows.Add(flow.Key, flow.Value.Copy());
      foreach (ExceptionEntry exception in this.Exceptions)
        snapshotCopy.Exceptions.Add(exception);
      foreach (FeatureValue featureValue in this.FeatureValues)
        snapshotCopy.FeatureValues.Add(featureValue);
      return snapshotCopy;
    }

    public void SubtractSnapshotCopy(Session session)
    {
      this.InternalAnalytics.SubtractCopy(session.InternalAnalytics);
      this.HasSendData = true;
      foreach (KeyValuePair<string, Feature> feature1 in session.Features)
      {
        Feature feature2;
        if (this.Features.TryGetValue(feature1.Key, out feature2))
          feature2.SyncedHits = feature1.Value.SessionHit;
      }
      foreach (KeyValuePair<string, Flow> flow1 in session.Flows)
      {
        Flow flow2;
        if (this.Flows.TryGetValue(flow1.Key, out flow2))
          flow2.SubtractCopy(flow1.Value);
      }
      foreach (ExceptionEntry exception in session.Exceptions)
        this.Exceptions.Remove(exception);
      foreach (FeatureValue featureValue in session.FeatureValues)
        this.FeatureValues.Remove(featureValue);
      this.InstallationSettings.SubtractCopy(session.InstallationSettings);
    }

    public Session Copy()
    {
      Session session = new Session(this.Id, this.InstallationSettings.Copy(), this.ApplicationVersion)
      {
        StartCount = this.StartCount,
        StartTime = this.StartTime,
        Runtime = this.Runtime,
        Stopped = this.Stopped,
        UtcStartTime = this.UtcStartTime
      };
      foreach (KeyValuePair<string, Feature> feature in this.Features)
        session.Features.Add(feature.Key, feature.Value.Copy());
      foreach (KeyValuePair<string, Flow> flow in this.Flows)
        session.Flows.Add(flow.Key, flow.Value.Copy());
      foreach (ExceptionEntry exception in this.Exceptions)
        session.Exceptions.Add(exception.Copy());
      foreach (FeatureValue featureValue in this.FeatureValues)
        session.FeatureValues.Add(featureValue.Copy());
      return session;
    }

    public bool Equals(Session other)
    {
      if (other == null || !(this.Id == other.Id) || this.StartCount != other.StartCount || !(this.StartTime == other.StartTime) || !(this.Runtime == other.Runtime) || this.Stopped != other.Stopped || !(this.UtcStartTime == other.UtcStartTime) || !(this.InstallationSettings.InstallationId == other.InstallationSettings.InstallationId) || this.Features.Count != other.Features.Count || this.Flows.Count != other.Flows.Count || this.Exceptions.Count != other.Exceptions.Count || this.FeatureValues.Count != other.FeatureValues.Count || !(this.ApplicationVersion == other.ApplicationVersion))
        return false;
      foreach (KeyValuePair<string, string> installationProperty in this.InstallationSettings.InstallationProperties)
      {
        if (!other.InstallationSettings.InstallationProperties.ContainsKey(installationProperty.Key) || !installationProperty.Value.Equals(other.InstallationSettings.InstallationProperties[installationProperty.Key]))
          return false;
      }
      foreach (KeyValuePair<string, Feature> feature in this.Features)
      {
        if (!other.Features.ContainsKey(feature.Key) || !feature.Value.Equals(other.Features[feature.Key]))
          return false;
      }
      foreach (KeyValuePair<string, Flow> flow in this.Flows)
      {
        if (!other.Flows.ContainsKey(flow.Key) || !flow.Value.Equals(other.Flows[flow.Key]))
          return false;
      }
      for (int index = 0; index < this.Exceptions.Count; ++index)
      {
        if (!this.Exceptions[index].Equals(other.Exceptions[index]))
          return false;
      }
      for (int index = 0; index < this.FeatureValues.Count; ++index)
      {
        if (!this.FeatureValues[index].Equals(other.FeatureValues[index]))
          return false;
      }
      return true;
    }

    public void End()
    {
      this.InstallationSettings = this.InstallationSettings.Copy();
      this.Stopped = true;
    }
  }
}
