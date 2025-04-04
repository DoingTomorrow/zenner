// Decompiled with JetBrains decompiler
// Type: System.Reactive.ScheduledObserver`1
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Collections.Concurrent;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

#nullable disable
namespace System.Reactive
{
  internal class ScheduledObserver<T> : 
    ObserverBase<T>,
    IScheduledObserver<T>,
    IObserver<T>,
    IDisposable
  {
    private volatile int _state;
    private const int STOPPED = 0;
    private const int RUNNING = 1;
    private const int PENDING = 2;
    private const int FAULTED = 9;
    private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
    private volatile bool _failed;
    private volatile Exception _error;
    private volatile bool _completed;
    private readonly IObserver<T> _observer;
    private readonly IScheduler _scheduler;
    private readonly ISchedulerLongRunning _longRunning;
    private readonly SerialDisposable _disposable = new SerialDisposable();
    private readonly object _dispatcherInitGate = new object();
    private readonly SemaphoreSlim _dispatcherEvent;
    private readonly IDisposable _dispatcherEventRelease;
    private IDisposable _dispatcherJob;

    public ScheduledObserver(IScheduler scheduler, IObserver<T> observer)
    {
      this._scheduler = scheduler;
      this._observer = observer;
      this._longRunning = this._scheduler.AsLongRunning();
      if (this._longRunning == null)
        return;
      this._dispatcherEvent = new SemaphoreSlim(0);
      this._dispatcherEventRelease = Disposable.Create((Action) (() => this._dispatcherEvent.Release()));
    }

    private void EnsureDispatcher()
    {
      if (this._dispatcherJob != null)
        return;
      lock (this._dispatcherInitGate)
      {
        if (this._dispatcherJob != null)
          return;
        this._dispatcherJob = this._longRunning.ScheduleLongRunning(new Action<ICancelable>(this.Dispatch));
        this._disposable.Disposable = (IDisposable) StableCompositeDisposable.Create(this._dispatcherJob, this._dispatcherEventRelease);
      }
    }

    private void Dispatch(ICancelable cancel)
    {
      do
      {
        this._dispatcherEvent.Wait();
        if (cancel.IsDisposed)
          return;
        T result1 = default (T);
        while (this._queue.TryDequeue(out result1))
        {
          try
          {
            this._observer.OnNext(result1);
          }
          catch
          {
            T result2 = default (T);
            do
              ;
            while (this._queue.TryDequeue(out result2));
            throw;
          }
          this._dispatcherEvent.Wait();
          if (cancel.IsDisposed)
            return;
        }
        if (this._failed)
        {
          this._observer.OnError(this._error);
          this.Dispose();
          return;
        }
      }
      while (!this._completed);
      this._observer.OnCompleted();
      this.Dispose();
    }

    public void EnsureActive() => this.EnsureActive(1);

    public void EnsureActive(int n)
    {
      if (this._longRunning != null)
      {
        if (n > 0)
          this._dispatcherEvent.Release(n);
        this.EnsureDispatcher();
      }
      else
        this.EnsureActiveSlow();
    }

    private void EnsureActiveSlow()
    {
      bool flag = false;
      int num;
      do
      {
        num = Interlocked.CompareExchange(ref this._state, 1, 0);
        switch (num)
        {
          case 0:
            flag = true;
            goto label_5;
          case 9:
            return;
          default:
            continue;
        }
      }
      while (num != 2 && (num != 1 || Interlocked.CompareExchange(ref this._state, 2, 1) != 1));
label_5:
      if (!flag)
        return;
      this._disposable.Disposable = this._scheduler.Schedule<object>((object) null, new Action<object, Action<object>>(this.Run));
    }

    private void Run(object state, Action<object> recurse)
    {
      T result1 = default (T);
      while (!this._queue.TryDequeue(out result1))
      {
        if (this._failed)
        {
          if (this._queue.IsEmpty)
          {
            Interlocked.Exchange(ref this._state, 0);
            this._observer.OnError(this._error);
            this.Dispose();
            return;
          }
        }
        else if (this._completed)
        {
          if (this._queue.IsEmpty)
          {
            Interlocked.Exchange(ref this._state, 0);
            this._observer.OnCompleted();
            this.Dispose();
            return;
          }
        }
        else
        {
          switch (Interlocked.CompareExchange(ref this._state, 0, 1))
          {
            case 1:
              return;
            case 9:
              return;
            default:
              this._state = 1;
              continue;
          }
        }
      }
      Interlocked.Exchange(ref this._state, 1);
      try
      {
        this._observer.OnNext(result1);
      }
      catch
      {
        Interlocked.Exchange(ref this._state, 9);
        T result2 = default (T);
        do
          ;
        while (this._queue.TryDequeue(out result2));
        throw;
      }
      recurse(state);
    }

    protected override void OnNextCore(T value) => this._queue.Enqueue(value);

    protected override void OnErrorCore(Exception exception)
    {
      this._error = exception;
      this._failed = true;
    }

    protected override void OnCompletedCore() => this._completed = true;

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (!disposing)
        return;
      this._disposable.Dispose();
    }
  }
}
