// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Timer
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Timer : Producer<long>
  {
    private readonly DateTimeOffset? _dueTimeA;
    private readonly TimeSpan? _dueTimeR;
    private readonly TimeSpan? _period;
    private readonly IScheduler _scheduler;

    public Timer(DateTimeOffset dueTime, TimeSpan? period, IScheduler scheduler)
    {
      this._dueTimeA = new DateTimeOffset?(dueTime);
      this._period = period;
      this._scheduler = scheduler;
    }

    public Timer(TimeSpan dueTime, TimeSpan? period, IScheduler scheduler)
    {
      this._dueTimeR = new TimeSpan?(dueTime);
      this._period = period;
      this._scheduler = scheduler;
    }

    protected override IDisposable Run(
      IObserver<long> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._period.HasValue)
      {
        Timer.TimerImpl timerImpl = new Timer.TimerImpl(this, observer, cancel);
        setSink((IDisposable) timerImpl);
        return timerImpl.Run();
      }
      Timer._ obj = new Timer._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<long>
    {
      private readonly Timer _parent;

      public _(Timer parent, IObserver<long> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        return this._parent._dueTimeA.HasValue ? this._parent._scheduler.Schedule(this._parent._dueTimeA.Value, new Action(this.Invoke)) : this._parent._scheduler.Schedule(this._parent._dueTimeR.Value, new Action(this.Invoke));
      }

      private void Invoke()
      {
        this._observer.OnNext(0L);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }

    private class TimerImpl : Sink<long>
    {
      private readonly Timer _parent;
      private readonly TimeSpan _period;
      private int _pendingTickCount;
      private IDisposable _periodic;

      public TimerImpl(Timer parent, IObserver<long> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._period = this._parent._period.Value;
      }

      public IDisposable Run()
      {
        if (this._parent._dueTimeA.HasValue)
          return this._parent._scheduler.Schedule<object>((object) null, this._parent._dueTimeA.Value, new Func<IScheduler, object, IDisposable>(this.InvokeStart));
        TimeSpan dueTime = this._parent._dueTimeR.Value;
        return dueTime == this._period ? this._parent._scheduler.SchedulePeriodic<long>(0L, this._period, new Func<long, long>(this.Tick)) : this._parent._scheduler.Schedule<object>((object) null, dueTime, new Func<IScheduler, object, IDisposable>(this.InvokeStart));
      }

      private long Tick(long count)
      {
        this._observer.OnNext(count);
        return count + 1L;
      }

      private IDisposable InvokeStart(IScheduler self, object state)
      {
        this._pendingTickCount = 1;
        SingleAssignmentDisposable disposable1 = new SingleAssignmentDisposable();
        this._periodic = (IDisposable) disposable1;
        disposable1.Disposable = self.SchedulePeriodic<long>(1L, this._period, new Func<long, long>(this.Tock));
        try
        {
          this._observer.OnNext(0L);
        }
        catch (Exception ex)
        {
          disposable1.Dispose();
          ex.Throw();
        }
        if (Interlocked.Decrement(ref this._pendingTickCount) <= 0)
          return (IDisposable) disposable1;
        return (IDisposable) StableCompositeDisposable.Create((IDisposable) disposable1, (IDisposable) new SingleAssignmentDisposable()
        {
          Disposable = self.Schedule<long>(1L, new Action<long, Action<long>>(this.CatchUp))
        });
      }

      private long Tock(long count)
      {
        if (Interlocked.Increment(ref this._pendingTickCount) == 1)
        {
          this._observer.OnNext(count);
          Interlocked.Decrement(ref this._pendingTickCount);
        }
        return count + 1L;
      }

      private void CatchUp(long count, Action<long> recurse)
      {
        try
        {
          this._observer.OnNext(count);
        }
        catch (Exception ex)
        {
          this._periodic.Dispose();
          ex.Throw();
        }
        if (Interlocked.Decrement(ref this._pendingTickCount) <= 0)
          return;
        recurse(count + 1L);
      }
    }
  }
}
