// Decompiled with JetBrains decompiler
// Type: System.Reactive.PlatformServices.HostLifecycleService
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.ComponentModel;
using System.Threading;

#nullable disable
namespace System.Reactive.PlatformServices
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static class HostLifecycleService
  {
    private static Lazy<IHostLifecycleNotifications> s_notifications = new Lazy<IHostLifecycleNotifications>(new Func<IHostLifecycleNotifications>(HostLifecycleService.InitializeNotifications));
    private static int _refCount;

    public static event EventHandler<HostSuspendingEventArgs> Suspending;

    public static event EventHandler<HostResumingEventArgs> Resuming;

    public static void AddRef()
    {
      if (Interlocked.Increment(ref HostLifecycleService._refCount) != 1)
        return;
      IHostLifecycleNotifications lifecycleNotifications = HostLifecycleService.s_notifications.Value;
      if (lifecycleNotifications == null)
        return;
      lifecycleNotifications.Suspending += new EventHandler<HostSuspendingEventArgs>(HostLifecycleService.OnSuspending);
      lifecycleNotifications.Resuming += new EventHandler<HostResumingEventArgs>(HostLifecycleService.OnResuming);
    }

    public static void Release()
    {
      if (Interlocked.Decrement(ref HostLifecycleService._refCount) != 0)
        return;
      IHostLifecycleNotifications lifecycleNotifications = HostLifecycleService.s_notifications.Value;
      if (lifecycleNotifications == null)
        return;
      lifecycleNotifications.Suspending -= new EventHandler<HostSuspendingEventArgs>(HostLifecycleService.OnSuspending);
      lifecycleNotifications.Resuming -= new EventHandler<HostResumingEventArgs>(HostLifecycleService.OnResuming);
    }

    private static void OnSuspending(object sender, HostSuspendingEventArgs e)
    {
      EventHandler<HostSuspendingEventArgs> suspending = HostLifecycleService.Suspending;
      if (suspending == null)
        return;
      suspending(sender, e);
    }

    private static void OnResuming(object sender, HostResumingEventArgs e)
    {
      EventHandler<HostResumingEventArgs> resuming = HostLifecycleService.Resuming;
      if (resuming == null)
        return;
      resuming(sender, e);
    }

    private static IHostLifecycleNotifications InitializeNotifications()
    {
      return PlatformEnlightenmentProvider.Current.GetService<IHostLifecycleNotifications>();
    }
  }
}
