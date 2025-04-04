// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.AnalyticsMonitorFacade
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  [Obsolete("This static class is no longer support and will be removed in future versions. Please use the IAnalyticsMonitor directly by creating instances with the AnalyticsMonitorFactory methods")]
  public static class AnalyticsMonitorFacade
  {
    private static IAnalyticsMonitor s_instance;
    private static Guid s_productId;
    private static readonly object s_lock = new object();

    public static void Create(string productId)
    {
      AnalyticsMonitorFacade.Create(AnalyticsMonitorFactory.CreateSettings(productId));
    }

    public static void Create(IAnalyticsMonitorSettings settings)
    {
      lock (AnalyticsMonitorFacade.s_lock)
      {
        if (AnalyticsMonitorFacade.s_instance != null)
          throw new InvalidOperationException(string.Format("The AnalyticsMonitorFacade has already registered a facade instance. The existing facade instance has product id {0}. That can happen if multiple parts of your application utilizes the facade and uses the Create call. Consider using the AnalyticsMonitorFactory.Create calls instead if this is the case.", (object) AnalyticsMonitorFacade.s_productId));
        AnalyticsMonitorFacade.Create(AnalyticsMonitorFactory.CreateMonitor(settings), settings.ProductId);
      }
    }

    internal static void Create(IAnalyticsMonitor monitor, Guid productID)
    {
      AnalyticsMonitorFacade.s_instance = monitor;
      AnalyticsMonitorFacade.s_productId = productID;
    }

    private static void Do(Action<IAnalyticsMonitor> action)
    {
      IAnalyticsMonitor instance = AnalyticsMonitorFacade.s_instance;
      if (instance == null)
        return;
      action(instance);
    }

    private static T Do<T>(AnalyticsMonitorFacade.ReturnCall<T> action, T defaultValue)
    {
      IAnalyticsMonitor instance = AnalyticsMonitorFacade.s_instance;
      return instance != null ? action(instance) : defaultValue;
    }

    public static void Start()
    {
      AnalyticsMonitorFacade.Do((Action<IAnalyticsMonitor>) (m => m.Start()));
    }

    public static void Stop()
    {
      AnalyticsMonitorFacade.Do((Action<IAnalyticsMonitor>) (m => m.Stop()));
    }

    public static void Stop(TimeSpan waitForCompletion)
    {
      AnalyticsMonitorFacade.Do((Action<IAnalyticsMonitor>) (m => m.Stop(waitForCompletion)));
    }

    public static void TrackException(Exception exception)
    {
      AnalyticsMonitorFacade.Do((Action<IAnalyticsMonitor>) (m => m.TrackException(exception)));
    }

    public static void TrackException(Exception exception, string message)
    {
      AnalyticsMonitorFacade.Do((Action<IAnalyticsMonitor>) (m => m.TrackException(exception, message)));
    }

    public static void TrackException(Exception exception, string format, params object[] args)
    {
      AnalyticsMonitorFacade.Do((Action<IAnalyticsMonitor>) (m => m.TrackException(exception, format, args)));
    }

    public static void TrackFeature(string featureName)
    {
      AnalyticsMonitorFacade.Do((Action<IAnalyticsMonitor>) (m => m.TrackFeature(featureName)));
    }

    public static void TrackFeatures(string[] featureNames)
    {
      AnalyticsMonitorFacade.Do((Action<IAnalyticsMonitor>) (m => m.TrackFeatures(featureNames)));
    }

    public static TimingScope TrackFeatureStart(string featureName)
    {
      return AnalyticsMonitorFacade.Do<TimingScope>((AnalyticsMonitorFacade.ReturnCall<TimingScope>) (m => m.TrackFeatureStart(featureName)), new TimingScope(featureName, (IAnalyticsMonitor) null));
    }

    public static TimeSpan TrackFeatureStop(string featureName)
    {
      return AnalyticsMonitorFacade.Do<TimeSpan>((AnalyticsMonitorFacade.ReturnCall<TimeSpan>) (m => m.TrackFeatureStop(featureName)), TimeSpan.Zero);
    }

    public static void TrackFeatureCancel(string featureName)
    {
      AnalyticsMonitorFacade.Do((Action<IAnalyticsMonitor>) (m => m.TrackFeatureCancel(featureName)));
    }

    public static void TrackFeatureValue(string featureName, long value)
    {
      AnalyticsMonitorFacade.Do((Action<IAnalyticsMonitor>) (m => m.TrackFeatureValue(featureName, value)));
    }

    public static void ForceSync()
    {
      AnalyticsMonitorFacade.Do((Action<IAnalyticsMonitor>) (m => m.ForceSync()));
    }

    public static void SetInstallationInfo(
      string installationID,
      IDictionary<string, string> installationProperties)
    {
      AnalyticsMonitorFacade.Do((Action<IAnalyticsMonitor>) (x => x.SetInstallationInfo(installationID, installationProperties)));
    }

    public static void SetInstallationInfo(string installationID)
    {
      AnalyticsMonitorFacade.Do((Action<IAnalyticsMonitor>) (x => x.SetInstallationInfo(installationID)));
    }

    public static AnalyticsMonitorStatus Status
    {
      get
      {
        return AnalyticsMonitorFacade.s_instance == null ? (AnalyticsMonitorStatus) null : AnalyticsMonitorFacade.s_instance.Status;
      }
    }

    private delegate T ReturnCall<T>(IAnalyticsMonitor monitor);
  }
}
