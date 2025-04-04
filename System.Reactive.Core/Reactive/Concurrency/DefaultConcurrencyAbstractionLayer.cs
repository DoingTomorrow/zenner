// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.DefaultConcurrencyAbstractionLayer
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Reactive.Disposables;
using System.Threading;

#nullable disable
namespace System.Reactive.Concurrency
{
  internal class DefaultConcurrencyAbstractionLayer : IConcurrencyAbstractionLayer
  {
    public IDisposable StartTimer(Action<object> action, object state, TimeSpan dueTime)
    {
      return (IDisposable) new DefaultConcurrencyAbstractionLayer.Timer(action, state, DefaultConcurrencyAbstractionLayer.Normalize(dueTime));
    }

    public IDisposable StartPeriodicTimer(Action action, TimeSpan period)
    {
      if (period < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (period));
      return period == TimeSpan.Zero ? (IDisposable) new DefaultConcurrencyAbstractionLayer.FastPeriodicTimer(action) : (IDisposable) new DefaultConcurrencyAbstractionLayer.PeriodicTimer(action, period);
    }

    public IDisposable QueueUserWorkItem(Action<object> action, object state)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) (_ => action(_)), state);
      return Disposable.Empty;
    }

    public void Sleep(TimeSpan timeout)
    {
      Thread.Sleep(DefaultConcurrencyAbstractionLayer.Normalize(timeout));
    }

    public IStopwatch StartStopwatch() => (IStopwatch) new DefaultStopwatch();

    public bool SupportsLongRunning => true;

    public void StartThread(Action<object> action, object state)
    {
      new Thread((ThreadStart) (() => action(state)))
      {
        IsBackground = true
      }.Start();
    }

    private static TimeSpan Normalize(TimeSpan dueTime)
    {
      return dueTime < TimeSpan.Zero ? TimeSpan.Zero : dueTime;
    }

    private class Timer : IDisposable
    {
      private Action<object> _action;
      private volatile System.Threading.Timer _timer;

      public Timer(Action<object> action, object state, TimeSpan dueTime)
      {
        this._action = action;
        try
        {
        }
        finally
        {
          this._timer = new System.Threading.Timer(new TimerCallback(this.Tick), state, dueTime, TimeSpan.FromMilliseconds(-1.0));
        }
      }

      private void Tick(object state)
      {
        try
        {
          this._action(state);
        }
        finally
        {
          SpinWait.SpinUntil(new Func<bool>(this.IsTimerAssigned));
          this.Dispose();
        }
      }

      private bool IsTimerAssigned() => this._timer != null;

      public void Dispose()
      {
        System.Threading.Timer timer = this._timer;
        if (timer == TimerStubs.Never)
          return;
        this._action = Stubs<object>.Ignore;
        this._timer = TimerStubs.Never;
        timer.Dispose();
      }
    }

    private class PeriodicTimer : IDisposable
    {
      private Action _action;
      private volatile System.Threading.Timer _timer;

      public PeriodicTimer(Action action, TimeSpan period)
      {
        this._action = action;
        this._timer = new System.Threading.Timer(new TimerCallback(this.Tick), (object) null, period, period);
      }

      private void Tick(object state) => this._action();

      public void Dispose()
      {
        System.Threading.Timer timer = this._timer;
        if (timer == null)
          return;
        this._action = Stubs.Nop;
        this._timer = (System.Threading.Timer) null;
        timer.Dispose();
      }
    }

    private class FastPeriodicTimer : IDisposable
    {
      private readonly Action _action;
      private volatile bool disposed;

      public FastPeriodicTimer(Action action)
      {
        this._action = action;
        new Thread(new ThreadStart(this.Loop))
        {
          Name = "Rx-FastPeriodicTimer",
          IsBackground = true
        }.Start();
      }

      private void Loop()
      {
        while (!this.disposed)
          this._action();
      }

      public void Dispose() => this.disposed = true;
    }
  }
}
