// Decompiled with JetBrains decompiler
// Type: System.Reactive.PlatformServices.SystemClock
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Threading;

#nullable disable
namespace System.Reactive.PlatformServices
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static class SystemClock
  {
    private static Lazy<ISystemClock> s_serviceSystemClock = new Lazy<ISystemClock>(new Func<ISystemClock>(SystemClock.InitializeSystemClock));
    private static Lazy<INotifySystemClockChanged> s_serviceSystemClockChanged = new Lazy<INotifySystemClockChanged>(new Func<INotifySystemClockChanged>(SystemClock.InitializeSystemClockChanged));
    private static readonly HashSet<WeakReference<LocalScheduler>> s_systemClockChanged = new HashSet<WeakReference<LocalScheduler>>();
    private static IDisposable s_systemClockChangedHandlerCollector;
    private static int _refCount;

    public static DateTimeOffset UtcNow => SystemClock.s_serviceSystemClock.Value.UtcNow;

    public static void AddRef()
    {
      if (Interlocked.Increment(ref SystemClock._refCount) != 1)
        return;
      SystemClock.s_serviceSystemClockChanged.Value.SystemClockChanged += new EventHandler<SystemClockChangedEventArgs>(SystemClock.OnSystemClockChanged);
    }

    public static void Release()
    {
      if (Interlocked.Decrement(ref SystemClock._refCount) != 0)
        return;
      SystemClock.s_serviceSystemClockChanged.Value.SystemClockChanged -= new EventHandler<SystemClockChangedEventArgs>(SystemClock.OnSystemClockChanged);
    }

    private static void OnSystemClockChanged(object sender, SystemClockChangedEventArgs e)
    {
      lock (SystemClock.s_systemClockChanged)
      {
        foreach (WeakReference<LocalScheduler> weakReference in SystemClock.s_systemClockChanged)
        {
          LocalScheduler localScheduler = (LocalScheduler) null;
          ref LocalScheduler local = ref localScheduler;
          if (weakReference.TryGetTarget(out local))
            localScheduler.SystemClockChanged(sender, e);
        }
      }
    }

    private static ISystemClock InitializeSystemClock()
    {
      return PlatformEnlightenmentProvider.Current.GetService<ISystemClock>() ?? (ISystemClock) new DefaultSystemClock();
    }

    private static INotifySystemClockChanged InitializeSystemClockChanged()
    {
      return PlatformEnlightenmentProvider.Current.GetService<INotifySystemClockChanged>() ?? (INotifySystemClockChanged) new DefaultSystemClockMonitor();
    }

    internal static void Register(LocalScheduler scheduler)
    {
      lock (SystemClock.s_systemClockChanged)
      {
        SystemClock.s_systemClockChanged.Add(new WeakReference<LocalScheduler>(scheduler));
        if (SystemClock.s_systemClockChanged.Count == 1)
        {
          SystemClock.s_systemClockChangedHandlerCollector = ConcurrencyAbstractionLayer.Current.StartPeriodicTimer(new Action(SystemClock.CollectHandlers), TimeSpan.FromSeconds(30.0));
        }
        else
        {
          if (SystemClock.s_systemClockChanged.Count % 64 != 0)
            return;
          SystemClock.CollectHandlers();
        }
      }
    }

    private static void CollectHandlers()
    {
      lock (SystemClock.s_systemClockChanged)
      {
        HashSet<WeakReference<LocalScheduler>> weakReferenceSet = (HashSet<WeakReference<LocalScheduler>>) null;
        foreach (WeakReference<LocalScheduler> weakReference in SystemClock.s_systemClockChanged)
        {
          LocalScheduler target = (LocalScheduler) null;
          if (!weakReference.TryGetTarget(out target))
          {
            if (weakReferenceSet == null)
              weakReferenceSet = new HashSet<WeakReference<LocalScheduler>>();
            weakReferenceSet.Add(weakReference);
          }
        }
        if (weakReferenceSet != null)
        {
          foreach (WeakReference<LocalScheduler> weakReference in weakReferenceSet)
            SystemClock.s_systemClockChanged.Remove(weakReference);
        }
        if (SystemClock.s_systemClockChanged.Count != 0)
          return;
        SystemClock.s_systemClockChangedHandlerCollector.Dispose();
        SystemClock.s_systemClockChangedHandlerCollector = (IDisposable) null;
      }
    }
  }
}
