// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Policy.MonitorInfo
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;

#nullable disable
namespace EQATEC.Analytics.Monitor.Policy
{
  internal class MonitorInfo : IEquatable<MonitorInfo>
  {
    internal string Cookie;
    internal string LocalIdentifier;
    internal Uri AlternativeUri;
    internal DateTime BandwidthUtilizationDate;
    internal long BandwidthUtilization;

    internal MonitorInfo() => this.LocalIdentifier = Guid.NewGuid().ToString("N");

    public bool Equals(MonitorInfo other)
    {
      return other != null && this.Cookie == other.Cookie && this.AlternativeUri == other.AlternativeUri && this.BandwidthUtilization == other.BandwidthUtilization && this.BandwidthUtilizationDate == other.BandwidthUtilizationDate && this.LocalIdentifier == other.LocalIdentifier;
    }

    public MonitorInfo Copy()
    {
      return new MonitorInfo()
      {
        AlternativeUri = this.AlternativeUri == (Uri) null ? (Uri) null : new Uri(this.AlternativeUri.ToString()),
        Cookie = this.Cookie,
        LocalIdentifier = this.LocalIdentifier,
        BandwidthUtilization = this.BandwidthUtilization,
        BandwidthUtilizationDate = this.BandwidthUtilizationDate
      };
    }
  }
}
