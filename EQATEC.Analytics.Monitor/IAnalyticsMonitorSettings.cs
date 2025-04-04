// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.IAnalyticsMonitorSettings
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  public interface IAnalyticsMonitorSettings
  {
    Guid ProductId { get; }

    Version Version { get; set; }

    ILogAnalyticsMonitor LoggingInterface { get; set; }

    IStorage StorageInterface { get; set; }

    TimeSpan StorageSaveInterval { get; set; }

    Uri ServerUri { get; set; }

    bool TestMode { get; set; }

    bool SynchronizeAutomatically { get; set; }

    int DailyNetworkUtilizationInKB { get; set; }

    int MaxStorageSizeInKB { get; set; }

    bool UseSSL { get; set; }

    ProxyConfiguration ProxyConfig { get; }

    LocationCoordinates Location { get; }
  }
}
