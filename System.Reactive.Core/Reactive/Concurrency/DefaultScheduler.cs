// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.DefaultScheduler
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Concurrency
{
  public sealed class DefaultScheduler : LocalScheduler, ISchedulerPeriodic
  {
    private static readonly Lazy<DefaultScheduler> s_instance = new Lazy<DefaultScheduler>((Func<DefaultScheduler>) (() => new DefaultScheduler()));
    private static IConcurrencyAbstractionLayer s_cal = ConcurrencyAbstractionLayer.Current;

    public static DefaultScheduler Instance => DefaultScheduler.s_instance.Value;

    private DefaultScheduler()
    {
    }

    public override IDisposable Schedule<TState>(
      TState state,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      SingleAssignmentDisposable d = new SingleAssignmentDisposable();
      IDisposable disposable2 = DefaultScheduler.s_cal.QueueUserWorkItem((Action<object>) (_ =>
      {
        if (d.IsDisposed)
          return;
        d.Disposable = action((IScheduler) this, state);
      }), (object) null);
      return (IDisposable) StableCompositeDisposable.Create((IDisposable) d, disposable2);
    }

    public override IDisposable Schedule<TState>(
      TState state,
      TimeSpan dueTime,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      TimeSpan dueTime1 = Scheduler.Normalize(dueTime);
      if (dueTime1.Ticks == 0L)
        return this.Schedule<TState>(state, action);
      SingleAssignmentDisposable d = new SingleAssignmentDisposable();
      IDisposable disposable2 = DefaultScheduler.s_cal.StartTimer((Action<object>) (_ =>
      {
        if (d.IsDisposed)
          return;
        d.Disposable = action((IScheduler) this, state);
      }), (object) null, dueTime1);
      return (IDisposable) StableCompositeDisposable.Create((IDisposable) d, disposable2);
    }

    public IDisposable SchedulePeriodic<TState>(
      TState state,
      TimeSpan period,
      Func<TState, TState> action)
    {
      if (period < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (period));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      TState state1 = state;
      AsyncLock gate = new AsyncLock();
      IDisposable cancel = DefaultScheduler.s_cal.StartPeriodicTimer((Action) (() => gate.Wait((Action) (() => state1 = action(state1)))), period);
      return Disposable.Create((Action) (() =>
      {
        cancel.Dispose();
        gate.Dispose();
        action = Stubs<TState>.I;
      }));
    }

    protected override object GetService(Type serviceType)
    {
      return serviceType == typeof (ISchedulerLongRunning) && DefaultScheduler.s_cal.SupportsLongRunning ? (object) DefaultScheduler.LongRunning.Instance : base.GetService(serviceType);
    }

    private class LongRunning : ISchedulerLongRunning
    {
      public static ISchedulerLongRunning Instance = (ISchedulerLongRunning) new DefaultScheduler.LongRunning();

      public IDisposable ScheduleLongRunning<TState>(
        TState state,
        Action<TState, ICancelable> action)
      {
        if (action == null)
          throw new ArgumentNullException(nameof (action));
        BooleanDisposable state1 = new BooleanDisposable();
        DefaultScheduler.s_cal.StartThread((Action<object>) (arg => action(state, (ICancelable) arg)), (object) state1);
        return (IDisposable) state1;
      }
    }
  }
}
