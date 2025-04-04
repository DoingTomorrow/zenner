// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.AnalyticsMonitorFactory
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using EQATEC.Analytics.Monitor.Messaging;
using EQATEC.Analytics.Monitor.Policy;
using EQATEC.Analytics.Monitor.Storage;
using System;
using System.IO.IsolatedStorage;
using System.Reflection;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  public static class AnalyticsMonitorFactory
  {
    public static IAnalyticsMonitor CreateMonitor(string productId)
    {
      if (new ArgumentChecker().IsInvalidProductId("AnalyticsMonitorFactory::CreateMonitor", nameof (productId), productId))
        return (IAnalyticsMonitor) null;
      Version version = new Version(0, 0, 0, 0);
      try
      {
        version = (Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly()).GetName().Version;
      }
      catch
      {
      }
      IAnalyticsMonitorSettings settings = AnalyticsMonitorFactory.CreateSettings(productId);
      settings.Version = version;
      return AnalyticsMonitorFactory.CreateMonitor(settings);
    }

    public static IAnalyticsMonitor Create(IAnalyticsMonitorSettings settings)
    {
      return AnalyticsMonitorFactory.CreateMonitor(settings);
    }

    public static IAnalyticsMonitor CreateMonitor(IAnalyticsMonitorSettings settings)
    {
      if (new ArgumentChecker().IsNullOrInvalidObject("AnalyticsMonitorFactory::CreateMonitor", nameof (settings), (object) settings))
        return (IAnalyticsMonitor) null;
      ILogAnalyticsMonitor log = settings.LoggingInterface ?? (ILogAnalyticsMonitor) new VoidLog();
      MonitorPolicy monitorPolicy = new MonitorPolicy();
      monitorPolicy.RuntimeStatus.AutoSync = settings.SynchronizeAutomatically;
      monitorPolicy.RuntimeStatus.StorageSaveInterval = settings.StorageSaveInterval <= TimeSpan.Zero ? TimeSpan.FromHours(24.0) : settings.StorageSaveInterval;
      monitorPolicy.RuntimeStatus.TestMode = settings.TestMode;
      monitorPolicy.RuntimeStatus.CurrentApplicationVersion = settings.Version;
      if (settings.Location != null)
      {
        if (!settings.Location.IsValid())
          log.LogError(string.Format("The location coordinates ({0}) are invalid and are not accepted", (object) settings.Location.ToString()));
        else
          monitorPolicy.RuntimeStatus.Location = settings.Location.Copy();
      }
      monitorPolicy.SettingsRestrictions.MaxStorageSizeInKB.SetExplictValue(Math.Max(1, settings.MaxStorageSizeInKB));
      monitorPolicy.SettingsRestrictions.MaxBandwidthUsagePerDayInKB.SetExplictValue(Math.Max(10, settings.DailyNetworkUtilizationInKB));
      ProxyConfiguration proxyConfiguration = new ProxyConfiguration();
      ProxyConfiguration proxyConfig = settings.ProxyConfig;
      IMessageSender sender = (IMessageSender) new MessageSender(log, proxyConfig, monitorPolicy);
      OSInfoObject osInfo = OSInfo.GetOSInfo(log);
      IMessageFactory messageFactory = (IMessageFactory) new MessageFactory(settings.ProductId, log, osInfo);
      IMessagingSubSystem messagingSubSystem = (IMessagingSubSystem) new MessagingSubSystem((IMessageReceiver) new MessageReceiver(messageFactory.MessageFactoryVersion, log, monitorPolicy), sender, monitorPolicy, settings.ServerUri, log);
      ThreadPoolTimer threadPoolTimer = new ThreadPoolTimer();
      IStorage storage = settings.StorageInterface ?? (IStorage) new VoidStorage();
      StatisticsContainer statisticsContainer = new StatisticsContainer((IStorageFactory) new StorageFactory(log, settings.ProductId, storage), log, monitorPolicy);
      Transmitter transmitter = new Transmitter(messagingSubSystem, messageFactory, log, monitorPolicy);
      MonitorCoordinator coordinator = new MonitorCoordinator((IStatisticsContainer) statisticsContainer, (ITimer) threadPoolTimer, (ITransmitter) transmitter, log, monitorPolicy);
      return (IAnalyticsMonitor) new AnalyticsMonitor((IStatisticsMonitor) statisticsContainer, (IMonitorCoordinator) coordinator, log, monitorPolicy);
    }

    public static IStorage CreateStorage(string folder)
    {
      try
      {
        return (IStorage) new FileStorage(folder);
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException("Could not create file storage at " + folder, ex);
      }
    }

    public static IStorage CreateStorage(string folder, string identifier)
    {
      try
      {
        return (IStorage) new FileStorage((FileSystemAdapter) new FilePathStorageAdapter(folder, identifier));
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException("Could not create file storage at " + folder, ex);
      }
    }

    public static IStorage CreateStorage(IsolatedStorageScope scope)
    {
      try
      {
        return (IStorage) new FileStorage(scope);
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException("Could not create isolated storage for scope " + scope.ToString(), ex);
      }
    }

    public static IStorage CreateStorage()
    {
      try
      {
        return (IStorage) new FileStorage((FileSystemAdapter) new IsolatedStorageAdapter());
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException("Could not create storage for isolatedstorage", ex);
      }
    }

    public static IAnalyticsMonitorSettings CreateSettings(string productId)
    {
      if (new ArgumentChecker().IsInvalidProductId("AnalyticsMonitorFactory::CreateSettings", nameof (productId), productId))
        return (IAnalyticsMonitorSettings) null;
      Version version = new Version(0, 0, 0, 0);
      try
      {
        version = (Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly()).GetName().Version;
      }
      catch
      {
      }
      return (IAnalyticsMonitorSettings) new AnalyticsMonitorSettings(productId, version);
    }

    public static ILogAnalyticsMonitor CreateTraceMonitor()
    {
      return (ILogAnalyticsMonitor) new TraceLog();
    }
  }
}
