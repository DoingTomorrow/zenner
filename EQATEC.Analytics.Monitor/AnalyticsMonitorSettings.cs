// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.AnalyticsMonitorSettings
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using EQATEC.Analytics.Monitor.Storage;
using System;
using System.Reflection;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  public class AnalyticsMonitorSettings : IAnalyticsMonitorSettings
  {
    private Uri m_serverUri;
    private Uri m_explictUri;
    private bool m_useSsl;

    internal AnalyticsMonitorSettings(string productId, Version version)
    {
      if (new ArgumentChecker().IsInvalidProductId("AnalyticsMonitorSettings::Create", nameof (productId), productId))
        return;
      this.ResetSettings(productId);
      this.Version = version;
      if (!(version == (Version) null))
        return;
      try
      {
        this.Version = (Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly()).GetName().Version;
      }
      catch
      {
        this.Version = new Version(0, 0, 0, 0);
      }
    }

    [Obsolete("Use the AnalyticsMonitorFactory.CreateSettings method instead")]
    public AnalyticsMonitorSettings(string productId)
    {
      if (new ArgumentChecker().IsInvalidProductId("AnalyticsMonitorSettings::Create", nameof (productId), productId))
        return;
      this.ResetSettings(productId);
      this.Version = new Version(0, 0, 0, 0);
      try
      {
        this.Version = (Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly()).GetName().Version;
      }
      catch
      {
      }
    }

    private void ResetSettings(string productId)
    {
      this.ProductId = new Guid(productId);
      this.SynchronizeAutomatically = true;
      this.LoggingInterface = (ILogAnalyticsMonitor) new VoidLog();
      this.StorageInterface = AnalyticsMonitorSettings.GetDefaultStorage();
      this.StorageSaveInterval = TimeSpan.FromMinutes(1.0);
      this.UseSSL = false;
      this.ServerUri = (Uri) null;
      this.DailyNetworkUtilizationInKB = int.MaxValue;
      this.MaxStorageSizeInKB = int.MaxValue;
      this.Location = new LocationCoordinates();
      this.ProxyConfig = new ProxyConfiguration();
    }

    public Guid ProductId { get; internal set; }

    public Version Version { get; set; }

    public ILogAnalyticsMonitor LoggingInterface { get; set; }

    public IStorage StorageInterface { get; set; }

    public TimeSpan StorageSaveInterval { get; set; }

    public Uri ServerUri
    {
      get => this.m_serverUri;
      set
      {
        this.m_explictUri = value;
        this.InitializeServerUri();
      }
    }

    private void InitializeServerUri()
    {
      Uri uri;
      if (this.m_explictUri != (Uri) null)
      {
        if (this.m_explictUri.AbsoluteUri.TrimEnd('/') != "http://analytics-monitor.eqatec.com")
        {
          if (this.m_explictUri.AbsoluteUri.TrimEnd('/') != "http://tools.eqatec.com")
          {
            uri = this.m_explictUri;
            goto label_5;
          }
        }
      }
      uri = AnalyticsMonitorSettings.GetServerUri(this.ProductId, this.UseSSL);
label_5:
      this.m_serverUri = uri;
    }

    public bool TestMode { get; set; }

    public bool SynchronizeAutomatically { get; set; }

    public int DailyNetworkUtilizationInKB { get; set; }

    public int MaxStorageSizeInKB { get; set; }

    public bool UseSSL
    {
      get => this.m_useSsl;
      set
      {
        this.m_useSsl = value;
        this.InitializeServerUri();
      }
    }

    internal static IStorage GetDefaultStorage()
    {
      try
      {
        return AnalyticsMonitorFactory.CreateStorage();
      }
      catch
      {
        return (IStorage) new VoidStorage();
      }
    }

    private static Uri GetServerUri(Guid productId, bool useSSL)
    {
      string str = productId.ToString("N");
      return new Uri(string.Format("{0}://{1}.monitor-eqatec.com/", useSSL ? (object) "https" : (object) "http", (object) str));
    }

    public ProxyConfiguration ProxyConfig { get; private set; }

    public LocationCoordinates Location { get; private set; }
  }
}
