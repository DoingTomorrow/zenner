// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Policy.RuntimeStatus
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;
using System.Net;

#nullable disable
namespace EQATEC.Analytics.Monitor.Policy
{
  internal class RuntimeStatus
  {
    internal bool HasSentOSInfo;
    internal Version CurrentApplicationVersion;
    internal int SyncedVersion;
    internal TimeSpan StorageSaveInterval;
    internal bool AutoSync;
    internal bool TestMode;
    internal LocationCoordinates Location;
    internal TimeSpan UptimeForLastForceSync;
    internal int SessionStartCount;

    internal InstallationSettings InstallationSettings { get; private set; }

    internal CookieContainer CookieContainer { get; private set; }

    internal RuntimeStatus()
    {
      this.StorageSaveInterval = TimeSpan.FromMinutes(1.0);
      this.TestMode = false;
      this.AutoSync = true;
      this.CurrentApplicationVersion = new Version(0, 0, 0, 0);
      this.InstallationSettings = new InstallationSettings();
      this.SyncedVersion = -1;
      this.UptimeForLastForceSync = TimeSpan.Zero;
      this.SessionStartCount = 0;
      this.CookieContainer = new CookieContainer();
    }

    internal void Reset()
    {
      this.HasSentOSInfo = false;
      this.SyncedVersion = -1;
      this.UptimeForLastForceSync = TimeSpan.Zero;
      this.SessionStartCount = 0;
      this.InstallationSettings.Reset();
    }

    internal RuntimeStatus Copy()
    {
      return new RuntimeStatus()
      {
        StorageSaveInterval = this.StorageSaveInterval,
        AutoSync = this.AutoSync,
        TestMode = this.TestMode,
        CurrentApplicationVersion = this.CurrentApplicationVersion,
        HasSentOSInfo = this.HasSentOSInfo,
        SyncedVersion = this.SyncedVersion,
        InstallationSettings = this.InstallationSettings.Copy(),
        UptimeForLastForceSync = this.UptimeForLastForceSync,
        SessionStartCount = this.SessionStartCount
      };
    }
  }
}
