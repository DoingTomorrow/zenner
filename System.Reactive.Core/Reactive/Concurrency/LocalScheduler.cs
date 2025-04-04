// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.LocalScheduler
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.PlatformServices;
using System.Threading;

#nullable disable
namespace System.Reactive.Concurrency
{
  public abstract class LocalScheduler : IScheduler, IStopwatchProvider, IServiceProvider
  {
    private static readonly object _gate = new object();
    private static readonly object s_gate = new object();
    private static readonly PriorityQueue<LocalScheduler.WorkItem> s_longTerm = new PriorityQueue<LocalScheduler.WorkItem>();
    private static readonly SerialDisposable s_nextLongTermTimer = new SerialDisposable();
    private static LocalScheduler.WorkItem s_nextLongTermWorkItem = (LocalScheduler.WorkItem) null;
    private readonly PriorityQueue<LocalScheduler.WorkItem> _shortTerm = new PriorityQueue<LocalScheduler.WorkItem>();
    private readonly HashSet<IDisposable> _shortTermWork = new HashSet<IDisposable>();
    private static readonly TimeSpan SHORTTERM = TimeSpan.FromSeconds(10.0);
    private const int MAXERRORRATIO = 1000;
    private static readonly TimeSpan LONGTOSHORT = TimeSpan.FromSeconds(5.0);
    private static readonly TimeSpan RETRYSHORT = TimeSpan.FromMilliseconds(50.0);
    private static readonly TimeSpan MAXSUPPORTEDTIMER = TimeSpan.FromMilliseconds(4294967294.0);

    public virtual DateTimeOffset Now => Scheduler.Now;

    public virtual IDisposable Schedule<TState>(
      TState state,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return this.Schedule<TState>(state, TimeSpan.Zero, action);
    }

    public abstract IDisposable Schedule<TState>(
      TState state,
      TimeSpan dueTime,
      Func<IScheduler, TState, IDisposable> action);

    public virtual IDisposable Schedule<TState>(
      TState state,
      DateTimeOffset dueTime,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return this.Enqueue<TState>(state, dueTime, action);
    }

    public virtual IStopwatch StartStopwatch()
    {
      return ConcurrencyAbstractionLayer.Current.StartStopwatch();
    }

    object IServiceProvider.GetService(Type serviceType) => this.GetService(serviceType);

    protected virtual object GetService(Type serviceType)
    {
      if (serviceType == typeof (IStopwatchProvider))
        return (object) this;
      if (serviceType == typeof (ISchedulerLongRunning))
        return (object) (this as ISchedulerLongRunning);
      return serviceType == typeof (ISchedulerPeriodic) ? (object) (this as ISchedulerPeriodic) : (object) null;
    }

    protected LocalScheduler() => SystemClock.Register(this);

    private IDisposable Enqueue<TState>(
      TState state,
      DateTimeOffset dueTime,
      Func<IScheduler, TState, IDisposable> action)
    {
      TimeSpan timeSpan = Scheduler.Normalize(dueTime - this.Now);
      if (timeSpan == TimeSpan.Zero)
        return this.Schedule<TState>(state, TimeSpan.Zero, action);
      SystemClock.AddRef();
      LocalScheduler.WorkItem<TState> workItem = new LocalScheduler.WorkItem<TState>(this, state, dueTime, action);
      if (timeSpan <= LocalScheduler.SHORTTERM)
        this.ScheduleShortTermWork((LocalScheduler.WorkItem) workItem);
      else
        LocalScheduler.ScheduleLongTermWork((LocalScheduler.WorkItem) workItem);
      return (IDisposable) workItem;
    }

    private void ScheduleShortTermWork(LocalScheduler.WorkItem item)
    {
      lock (LocalScheduler._gate)
      {
        this._shortTerm.Enqueue(item);
        SingleAssignmentDisposable state = new SingleAssignmentDisposable();
        this._shortTermWork.Add((IDisposable) state);
        TimeSpan dueTime = Scheduler.Normalize(item.DueTime - item.Scheduler.Now);
        state.Disposable = item.Scheduler.Schedule<SingleAssignmentDisposable>(state, dueTime, new Func<IScheduler, SingleAssignmentDisposable, IDisposable>(this.ExecuteNextShortTermWorkItem));
      }
    }

    private IDisposable ExecuteNextShortTermWorkItem(IScheduler scheduler, IDisposable cancel)
    {
      LocalScheduler.WorkItem workItem = (LocalScheduler.WorkItem) null;
      lock (LocalScheduler._gate)
      {
        if (this._shortTermWork.Remove(cancel))
        {
          if (this._shortTerm.Count > 0)
            workItem = this._shortTerm.Dequeue();
        }
      }
      if (workItem != null)
      {
        if (workItem.DueTime - workItem.Scheduler.Now >= LocalScheduler.RETRYSHORT)
          this.ScheduleShortTermWork(workItem);
        else
          workItem.Invoke(scheduler);
      }
      return Disposable.Empty;
    }

    private static void ScheduleLongTermWork(LocalScheduler.WorkItem item)
    {
      lock (LocalScheduler.s_gate)
      {
        LocalScheduler.s_longTerm.Enqueue(item);
        LocalScheduler.UpdateLongTermProcessingTimer();
      }
    }

    private static void UpdateLongTermProcessingTimer()
    {
      if (LocalScheduler.s_longTerm.Count == 0)
        return;
      LocalScheduler.WorkItem workItem = LocalScheduler.s_longTerm.Peek();
      if (workItem == LocalScheduler.s_nextLongTermWorkItem)
        return;
      TimeSpan timeSpan1 = Scheduler.Normalize(workItem.DueTime - workItem.Scheduler.Now);
      TimeSpan timeSpan2 = TimeSpan.FromTicks(Math.Max(timeSpan1.Ticks / 1000L, LocalScheduler.LONGTOSHORT.Ticks));
      TimeSpan dueTime = TimeSpan.FromTicks(Math.Min((timeSpan1 - timeSpan2).Ticks, LocalScheduler.MAXSUPPORTEDTIMER.Ticks));
      LocalScheduler.s_nextLongTermWorkItem = workItem;
      LocalScheduler.s_nextLongTermTimer.Disposable = ConcurrencyAbstractionLayer.Current.StartTimer(new Action<object>(LocalScheduler.EvaluateLongTermQueue), (object) null, dueTime);
    }

    private static void EvaluateLongTermQueue(object state)
    {
      lock (LocalScheduler.s_gate)
      {
        while (LocalScheduler.s_longTerm.Count > 0)
        {
          LocalScheduler.WorkItem workItem1 = LocalScheduler.s_longTerm.Peek();
          if (!(Scheduler.Normalize(workItem1.DueTime - workItem1.Scheduler.Now) >= LocalScheduler.SHORTTERM))
          {
            LocalScheduler.WorkItem workItem2 = LocalScheduler.s_longTerm.Dequeue();
            workItem2.Scheduler.ScheduleShortTermWork(workItem2);
          }
          else
            break;
        }
        LocalScheduler.s_nextLongTermWorkItem = (LocalScheduler.WorkItem) null;
        LocalScheduler.UpdateLongTermProcessingTimer();
      }
    }

    internal void SystemClockChanged(object sender, SystemClockChangedEventArgs args)
    {
      lock (LocalScheduler._gate)
      {
        lock (LocalScheduler.s_gate)
        {
          foreach (IDisposable disposable in this._shortTermWork)
            disposable.Dispose();
          this._shortTermWork.Clear();
          while (this._shortTerm.Count > 0)
          {
            LocalScheduler.WorkItem workItem = this._shortTerm.Dequeue();
            LocalScheduler.s_longTerm.Enqueue(workItem);
          }
          LocalScheduler.s_nextLongTermWorkItem = (LocalScheduler.WorkItem) null;
          LocalScheduler.EvaluateLongTermQueue((object) null);
        }
      }
    }

    private abstract class WorkItem : IComparable<LocalScheduler.WorkItem>, IDisposable
    {
      private readonly LocalScheduler _scheduler;
      private readonly DateTimeOffset _dueTime;
      private readonly SingleAssignmentDisposable _disposable;
      private int _hasRun;

      public WorkItem(LocalScheduler scheduler, DateTimeOffset dueTime)
      {
        this._scheduler = scheduler;
        this._dueTime = dueTime;
        this._disposable = new SingleAssignmentDisposable();
        this._hasRun = 0;
      }

      public LocalScheduler Scheduler => this._scheduler;

      public DateTimeOffset DueTime => this._dueTime;

      public void Invoke(IScheduler scheduler)
      {
        if (Interlocked.Exchange(ref this._hasRun, 1) != 0)
          return;
        try
        {
          if (this._disposable.IsDisposed)
            return;
          this._disposable.Disposable = this.InvokeCore(scheduler);
        }
        finally
        {
          SystemClock.Release();
        }
      }

      protected abstract IDisposable InvokeCore(IScheduler scheduler);

      public int CompareTo(LocalScheduler.WorkItem other)
      {
        return Comparer<DateTimeOffset>.Default.Compare(this._dueTime, other._dueTime);
      }

      public void Dispose() => this._disposable.Dispose();
    }

    private sealed class WorkItem<TState> : LocalScheduler.WorkItem
    {
      private readonly TState _state;
      private readonly Func<IScheduler, TState, IDisposable> _action;

      public WorkItem(
        LocalScheduler scheduler,
        TState state,
        DateTimeOffset dueTime,
        Func<IScheduler, TState, IDisposable> action)
        : base(scheduler, dueTime)
      {
        this._state = state;
        this._action = action;
      }

      protected override IDisposable InvokeCore(IScheduler scheduler)
      {
        return this._action(scheduler, this._state);
      }
    }
  }
}
