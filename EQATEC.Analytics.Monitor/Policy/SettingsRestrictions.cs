// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Policy.SettingsRestrictions
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;

#nullable disable
namespace EQATEC.Analytics.Monitor.Policy
{
  internal class SettingsRestrictions : IEquatable<SettingsRestrictions>
  {
    internal SettingsValue<int> MaxExceptionStackTrace = new SettingsValue<int>(10000);
    internal SettingsValue<int> MaxExceptionExtraInfo = new SettingsValue<int>(1000);
    internal SettingsValue<int> MaxExceptionMessage = new SettingsValue<int>(1000);
    internal SettingsValue<int> MaxNestedExceptions = new SettingsValue<int>(10);
    internal SettingsValue<int> MaxFeatureIdSize = new SettingsValue<int>(1000);
    internal SettingsValue<int> MaxSessions = new SettingsValue<int>(10);
    internal SettingsValue<int> MaxSessionExceptions = new SettingsValue<int>(10);
    internal SettingsValue<int> MaxSessionFeatureValues = new SettingsValue<int>(20000);
    internal SettingsValue<int> MaxInstallationIDSize = new SettingsValue<int>(50);
    internal SettingsValue<int> MaxInstallationPropertyKeySize = new SettingsValue<int>(50);
    internal SettingsValue<int> MaxInstallationProperties = new SettingsValue<int>(10);
    internal SettingsValue<int> MaxInstallationSettingsHashMapToStore = new SettingsValue<int>(50);
    internal SettingsValue<int> MaxStorageSizeInKB = new SettingsValue<int>(int.MaxValue);
    internal SettingsValue<int> MaxMessageAttempts = new SettingsValue<int>(3);
    internal SettingsValue<TimeSpan> RetrySendInterval = new SettingsValue<TimeSpan>(TimeSpan.FromSeconds(30.0));
    internal SettingsValue<TimeSpan> AutoSendInterval = new SettingsValue<TimeSpan>(TimeSpan.FromDays(1.0));
    internal SettingsValue<int> MaxBandwidthUsagePerDayInKB = new SettingsValue<int>(int.MaxValue);
    internal SettingsValue<int> MaxDataPayloadSizeKB = new SettingsValue<int>(3072);
    internal SettingsValue<int> MaxFlowNameLength = new SettingsValue<int>(100);
    internal SettingsValue<int> MaxWaypointNameLength = new SettingsValue<int>(200);
    internal SettingsValue<int> MaxLicenseIDSize = new SettingsValue<int>(50);
    internal SettingsValue<int> MaxLicenseTypeSize = new SettingsValue<int>(50);
    internal SettingsValue<int> MaxNumberOfLicenseInfos = new SettingsValue<int>(200);
    internal SettingsValue<int> MaxFlowQueueLength = new SettingsValue<int>(5);
    internal SettingsValue<int> MaxFlowsInSession = new SettingsValue<int>(100);
    internal SettingsValue<int> MaxGoalsInFlow = new SettingsValue<int>(20000);
    internal SettingsValue<int> MaxTransitionsInFlow = new SettingsValue<int>(20000);
    internal SettingsValue<int> MinSecondsBetweenForceSync = new SettingsValue<int>(600);

    internal int SettingsVersion { get; set; }

    public SettingsRestrictions() => this.SettingsVersion = 0;

    public bool Equals(SettingsRestrictions other)
    {
      return other != null && this.SettingsVersion == other.SettingsVersion && this.MaxExceptionStackTrace.Equals((object) other.MaxExceptionStackTrace) && this.MaxExceptionExtraInfo.Equals((object) other.MaxExceptionExtraInfo) && this.MaxExceptionMessage.Equals((object) other.MaxExceptionMessage) && this.MaxNestedExceptions.Equals((object) other.MaxNestedExceptions) && this.MaxFeatureIdSize.Equals((object) other.MaxFeatureIdSize) && this.MaxSessions.Equals((object) other.MaxSessions) && this.MaxSessionExceptions.Equals((object) other.MaxSessionExceptions) && this.MaxSessionFeatureValues.Equals((object) other.MaxSessionFeatureValues) && this.MaxInstallationPropertyKeySize.Equals((object) other.MaxInstallationPropertyKeySize) && this.MaxInstallationProperties.Equals((object) other.MaxInstallationProperties) && this.MaxInstallationSettingsHashMapToStore.Equals((object) other.MaxInstallationSettingsHashMapToStore) && this.MaxStorageSizeInKB.Equals((object) other.MaxStorageSizeInKB) && this.MaxMessageAttempts.Equals((object) other.MaxMessageAttempts) && this.RetrySendInterval.Equals((object) other.RetrySendInterval) && this.AutoSendInterval.Equals((object) other.AutoSendInterval) && this.MaxBandwidthUsagePerDayInKB.Equals((object) other.MaxBandwidthUsagePerDayInKB) && this.MaxInstallationIDSize.Equals((object) other.MaxInstallationIDSize) && this.MinSecondsBetweenForceSync.Equals((object) other.MinSecondsBetweenForceSync) && this.MaxDataPayloadSizeKB.Equals((object) other.MaxDataPayloadSizeKB) && this.MaxLicenseIDSize.Equals((object) other.MaxLicenseIDSize) && this.MaxFlowNameLength.Equals((object) other.MaxFlowNameLength) && this.MaxWaypointNameLength.Equals((object) other.MaxWaypointNameLength) && this.MaxLicenseIDSize.Equals((object) other.MaxLicenseIDSize) && this.MaxLicenseTypeSize.Equals((object) other.MaxLicenseTypeSize) && this.MaxNumberOfLicenseInfos.Equals((object) other.MaxNumberOfLicenseInfos) && this.MaxFlowQueueLength.Equals((object) other.MaxFlowQueueLength) && this.MaxFlowsInSession.Equals((object) other.MaxFlowsInSession) && this.MaxGoalsInFlow.Equals((object) other.MaxGoalsInFlow) && this.MaxTransitionsInFlow.Equals((object) other.MaxTransitionsInFlow);
    }

    public SettingsRestrictions Copy()
    {
      return new SettingsRestrictions()
      {
        SettingsVersion = this.SettingsVersion,
        MaxExceptionStackTrace = this.MaxExceptionStackTrace,
        MaxExceptionExtraInfo = this.MaxExceptionExtraInfo,
        MaxExceptionMessage = this.MaxExceptionMessage,
        MaxNestedExceptions = this.MaxNestedExceptions,
        MaxFeatureIdSize = this.MaxFeatureIdSize,
        MaxSessions = this.MaxSessions,
        MaxSessionExceptions = this.MaxSessionExceptions,
        MaxSessionFeatureValues = this.MaxSessionFeatureValues,
        MaxInstallationPropertyKeySize = this.MaxInstallationPropertyKeySize,
        MaxInstallationProperties = this.MaxInstallationProperties,
        MaxInstallationSettingsHashMapToStore = this.MaxInstallationSettingsHashMapToStore,
        MaxStorageSizeInKB = this.MaxStorageSizeInKB,
        MaxMessageAttempts = this.MaxMessageAttempts,
        RetrySendInterval = this.RetrySendInterval,
        AutoSendInterval = this.AutoSendInterval,
        MaxBandwidthUsagePerDayInKB = this.MaxBandwidthUsagePerDayInKB,
        MaxInstallationIDSize = this.MaxInstallationIDSize,
        MinSecondsBetweenForceSync = this.MinSecondsBetweenForceSync,
        MaxDataPayloadSizeKB = this.MaxDataPayloadSizeKB,
        MaxLicenseIDSize = this.MaxLicenseIDSize,
        MaxFlowNameLength = this.MaxFlowNameLength,
        MaxWaypointNameLength = this.MaxWaypointNameLength,
        MaxLicenseTypeSize = this.MaxLicenseTypeSize,
        MaxNumberOfLicenseInfos = this.MaxNumberOfLicenseInfos,
        MaxFlowQueueLength = this.MaxFlowQueueLength,
        MaxFlowsInSession = this.MaxFlowsInSession,
        MaxGoalsInFlow = this.MaxGoalsInFlow,
        MaxTransitionsInFlow = this.MaxTransitionsInFlow
      };
    }
  }
}
