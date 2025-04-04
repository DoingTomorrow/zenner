// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.AnalyticsMonitorCapabilities
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using EQATEC.Analytics.Monitor.Policy;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  public class AnalyticsMonitorCapabilities
  {
    private readonly MonitorPolicy m_policy;

    internal AnalyticsMonitorCapabilities(MonitorPolicy policy) => this.m_policy = policy;

    public int MaxLengthOfExceptionContextMessage
    {
      get => this.m_policy.SettingsRestrictions.MaxExceptionExtraInfo.Value;
    }

    public int MaxAllowedBandwidthUsagePerDayInKB
    {
      get => this.m_policy.SettingsRestrictions.MaxBandwidthUsagePerDayInKB.Value;
    }

    public int MaxLengthOfFeatureName => this.m_policy.SettingsRestrictions.MaxFeatureIdSize.Value;

    public int MaxNumberOfInstallationProperties
    {
      get => this.m_policy.SettingsRestrictions.MaxInstallationProperties.Value;
    }

    public int MaxKeySizeOfInstallationPropertyKey
    {
      get => this.m_policy.SettingsRestrictions.MaxInstallationPropertyKeySize.Value;
    }

    public int MaxStorageSizeInKB => this.m_policy.SettingsRestrictions.MaxStorageSizeInKB.Value;

    public int MaxInstallationIDSize
    {
      get => this.m_policy.SettingsRestrictions.MaxInstallationIDSize.Value;
    }
  }
}
