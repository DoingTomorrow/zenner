// Decompiled with JetBrains decompiler
// Type: Ninject.Activation.Caching.GarbageCollectionCachePruner
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Language;
using System;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace Ninject.Activation.Caching
{
  public class GarbageCollectionCachePruner : 
    NinjectComponent,
    ICachePruner,
    INinjectComponent,
    IDisposable
  {
    private readonly WeakReference indicator = new WeakReference(new object());
    private readonly List<IPruneable> caches = new List<IPruneable>();
    private Timer timer;
    private bool stop;

    public override void Dispose(bool disposing)
    {
      if (disposing && !this.IsDisposed && this.timer != null)
        this.Stop();
      base.Dispose(disposing);
    }

    public void Start(IPruneable pruneable)
    {
      Ensure.ArgumentNotNull((object) pruneable, nameof (pruneable));
      this.caches.Add(pruneable);
      if (this.timer != null)
        return;
      this.timer = new Timer(new TimerCallback(this.PruneCacheIfGarbageCollectorHasRun), (object) null, this.GetTimeoutInMilliseconds(), -1);
    }

    public void Stop()
    {
      lock (this)
        this.stop = true;
      using (ManualResetEvent notifyObject = new ManualResetEvent(false))
      {
        this.timer.Dispose((WaitHandle) notifyObject);
        notifyObject.WaitOne();
        this.timer = (Timer) null;
        this.caches.Clear();
      }
    }

    private void PruneCacheIfGarbageCollectorHasRun(object state)
    {
      lock (this)
      {
        if (this.stop)
          return;
        try
        {
          if (this.indicator.IsAlive)
            return;
          this.caches.Map<IPruneable>((Action<IPruneable>) (cache => cache.Prune()));
          this.indicator.Target = new object();
        }
        finally
        {
          this.timer.Change(this.GetTimeoutInMilliseconds(), -1);
        }
      }
    }

    private int GetTimeoutInMilliseconds()
    {
      TimeSpan cachePruningInterval = this.Settings.CachePruningInterval;
      return !(cachePruningInterval == TimeSpan.MaxValue) ? (int) cachePruningInterval.TotalMilliseconds : -1;
    }
  }
}
