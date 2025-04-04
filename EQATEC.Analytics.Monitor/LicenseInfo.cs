// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.LicenseInfo
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  internal class LicenseInfo
  {
    public LicenseInfo(string licenseIdentifier)
      : this(licenseIdentifier, (string) null)
    {
    }

    public LicenseInfo(string licenseIdentifier, string licenseType)
    {
      this.LicenseIdentifier = licenseIdentifier;
      this.LicenseType = licenseType;
    }

    public string LicenseIdentifier { get; internal set; }

    public string LicenseType { get; internal set; }

    internal LicenseInfo Copy() => new LicenseInfo(this.LicenseIdentifier, this.LicenseType);
  }
}
