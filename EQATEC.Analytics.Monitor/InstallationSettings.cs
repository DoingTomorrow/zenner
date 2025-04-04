// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.InstallationSettings
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using EQATEC.Analytics.Monitor.Policy;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  internal class InstallationSettings
  {
    public string InstallationId { get; set; }

    internal Dictionary<string, EQATEC.Analytics.Monitor.LicenseInfo> LicenseInfo { get; private set; }

    private string HashMapSent { get; set; }

    public Dictionary<string, string> InstallationProperties { get; private set; }

    internal bool HasChanged => this.HashMapSent != this.HashMapOfCustomproperties();

    internal InstallationSettings()
    {
      this.HashMapSent = string.Empty;
      this.InstallationId = string.Empty;
      this.InstallationProperties = new Dictionary<string, string>();
      this.LicenseInfo = new Dictionary<string, EQATEC.Analytics.Monitor.LicenseInfo>();
    }

    private string HashMapOfCustomproperties()
    {
      StringBuilder stringBuilder = new StringBuilder(this.InstallationId);
      foreach (KeyValuePair<string, string> installationProperty in this.InstallationProperties)
      {
        stringBuilder.Append(installationProperty.Key);
        stringBuilder.Append(installationProperty.Value);
      }
      return stringBuilder.ToString();
    }

    internal InstallationSettings Copy()
    {
      return new InstallationSettings()
      {
        InstallationId = this.InstallationId,
        InstallationProperties = new Dictionary<string, string>((IDictionary<string, string>) this.InstallationProperties),
        LicenseInfo = new Dictionary<string, EQATEC.Analytics.Monitor.LicenseInfo>((IDictionary<string, EQATEC.Analytics.Monitor.LicenseInfo>) this.LicenseInfo),
        HashMapSent = this.HashMapSent
      };
    }

    internal void Reset() => this.HashMapSent = string.Empty;

    public void SubtractCopy(InstallationSettings installationSettings)
    {
      foreach (KeyValuePair<string, EQATEC.Analytics.Monitor.LicenseInfo> keyValuePair in installationSettings.LicenseInfo)
        this.LicenseInfo.Remove(keyValuePair.Key);
      this.HashMapSent = installationSettings.HashMapOfCustomproperties();
    }

    public void Update(
      string installationId,
      IDictionary<string, string> installationProperties,
      MonitorPolicy policy,
      ILogAnalyticsMonitor log)
    {
      this.InstallationId = installationId ?? "";
      this.InstallationProperties.Clear();
      if (installationProperties == null)
        return;
      foreach (KeyValuePair<string, string> installationProperty in (IEnumerable<KeyValuePair<string, string>>) installationProperties)
      {
        if (string.IsNullOrEmpty(installationProperty.Key))
        {
          log.LogMessage("Empty property name is ignored");
        }
        else
        {
          int length = installationProperty.Key.Length;
          int num1 = policy.SettingsRestrictions.MaxInstallationPropertyKeySize.Value;
          if (length > num1)
            log.LogMessage(string.Format("Property name '{0}' is too long; length is {1} and limit is {2} chars", (object) installationProperty.Key, (object) length, (object) num1));
          else if (this.InstallationProperties.ContainsKey(installationProperty.Key))
          {
            this.InstallationProperties[installationProperty.Key] = installationProperty.Value;
          }
          else
          {
            int num2 = policy.SettingsRestrictions.MaxInstallationProperties.Value;
            if (this.InstallationProperties.Count >= num2)
            {
              log.LogError(string.Format("No more than {0} installation properties are allowed", (object) num2));
              break;
            }
            this.InstallationProperties[installationProperty.Key] = installationProperty.Value;
          }
        }
      }
    }
  }
}
