// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.ImmediateScheduler
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Concurrency
{
  public sealed class ImmediateScheduler : LocalScheduler
  {
    private static readonly Lazy<ImmediateScheduler> s_instance = new Lazy<ImmediateScheduler>((Func<ImmediateScheduler>) (() => new ImmediateScheduler()));

    private ImmediateScheduler()
    {
    }

    public static ImmediateScheduler Instance => ImmediateScheduler.s_instance.Value;

    public override IDisposable Schedule<TState>(
      TState state,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return action((IScheduler) new ImmediateScheduler.AsyncLockScheduler(), state);
    }

    public override IDisposable Schedule<TState>(
      TState state,
      TimeSpan dueTime,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      TimeSpan timeout = Scheduler.Normalize(dueTime);
      if (timeout.Ticks > 0L)
        ConcurrencyAbstractionLayer.Current.Sleep(timeout);
      return action((IScheduler) new ImmediateScheduler.AsyncLockScheduler(), state);
    }

    private class AsyncLockScheduler : LocalScheduler
    {
      private AsyncLock asyncLock;

      public override IDisposable Schedule<TState>(
        TState state,
        Func<IScheduler, TState, IDisposable> action)
      {
        if (action == null)
          throw new ArgumentNullException(nameof (action));
        SingleAssignmentDisposable m = new SingleAssignmentDisposable();
        if (this.asyncLock == null)
          this.asyncLock = new AsyncLock();
        this.asyncLock.Wait((Action) (() =>
        {
          if (m.IsDisposed)
            return;
          m.Disposable = action((IScheduler) this, state);
        }));
        return (IDisposable) m;
      }

      public override IDisposable Schedule<TState>(
        TState state,
        TimeSpan dueTime,
        Func<IScheduler, TState, IDisposable> action)
      {
        if (action == null)
          throw new ArgumentNullException(nameof (action));
        if (dueTime.Ticks <= 0L)
          return this.Schedule<TState>(state, action);
        IStopwatch timer = ConcurrencyAbstractionLayer.Current.StartStopwatch();
        SingleAssignmentDisposable m = new SingleAssignmentDisposable();
        if (this.asyncLock == null)
          this.asyncLock = new AsyncLock();
        this.asyncLock.Wait((Action) (() =>
        {
          if (m.IsDisposed)
            return;
          TimeSpan timeout = dueTime - timer.Elapsed;
          if (timeout.Ticks > 0L)
            ConcurrencyAbstractionLayer.Current.Sleep(timeout);
          if (m.IsDisposed)
            return;
          m.Disposable = action((IScheduler) this, state);
        }));
        return (IDisposable) m;
      }
    }
  }
}
