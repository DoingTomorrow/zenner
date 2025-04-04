// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.Scheduler
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Globalization;
using System.Reactive.Disposables;
using System.Reactive.PlatformServices;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace System.Reactive.Concurrency
{
  public static class Scheduler
  {
    private static Lazy<IScheduler> s_threadPool = new Lazy<IScheduler>((Func<IScheduler>) (() => Scheduler.Initialize(nameof (ThreadPool))));
    private static Lazy<IScheduler> s_newThread = new Lazy<IScheduler>((Func<IScheduler>) (() => Scheduler.Initialize(nameof (NewThread))));
    private static Lazy<IScheduler> s_taskPool = new Lazy<IScheduler>((Func<IScheduler>) (() => Scheduler.Initialize(nameof (TaskPool))));
    internal static Type[] OPTIMIZATIONS = new Type[3]
    {
      typeof (ISchedulerLongRunning),
      typeof (IStopwatchProvider),
      typeof (ISchedulerPeriodic)
    };

    public static SchedulerOperation Yield(this IScheduler scheduler)
    {
      return scheduler != null ? new SchedulerOperation((Func<Action, IDisposable>) (a => scheduler.Schedule(a)), scheduler.GetCancellationToken()) : throw new ArgumentNullException(nameof (scheduler));
    }

    public static SchedulerOperation Yield(
      this IScheduler scheduler,
      CancellationToken cancellationToken)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return new SchedulerOperation((Func<Action, IDisposable>) (a => scheduler.Schedule(a)), cancellationToken);
    }

    public static SchedulerOperation Sleep(this IScheduler scheduler, TimeSpan dueTime)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return new SchedulerOperation((Func<Action, IDisposable>) (a => scheduler.Schedule(dueTime, a)), scheduler.GetCancellationToken());
    }

    public static SchedulerOperation Sleep(
      this IScheduler scheduler,
      TimeSpan dueTime,
      CancellationToken cancellationToken)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return new SchedulerOperation((Func<Action, IDisposable>) (a => scheduler.Schedule(dueTime, a)), cancellationToken);
    }

    public static SchedulerOperation Sleep(this IScheduler scheduler, DateTimeOffset dueTime)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return new SchedulerOperation((Func<Action, IDisposable>) (a => scheduler.Schedule(dueTime, a)), scheduler.GetCancellationToken());
    }

    public static SchedulerOperation Sleep(
      this IScheduler scheduler,
      DateTimeOffset dueTime,
      CancellationToken cancellationToken)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return new SchedulerOperation((Func<Action, IDisposable>) (a => scheduler.Schedule(dueTime, a)), cancellationToken);
    }

    public static IDisposable ScheduleAsync<TState>(
      this IScheduler scheduler,
      TState state,
      Func<IScheduler, TState, CancellationToken, Task> action)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return Scheduler.ScheduleAsync_<TState>(scheduler, state, action);
    }

    public static IDisposable ScheduleAsync<TState>(
      this IScheduler scheduler,
      TState state,
      Func<IScheduler, TState, CancellationToken, Task<IDisposable>> action)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return Scheduler.ScheduleAsync_<TState>(scheduler, state, action);
    }

    public static IDisposable ScheduleAsync(
      this IScheduler scheduler,
      Func<IScheduler, CancellationToken, Task> action)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return Scheduler.ScheduleAsync_<object>(scheduler, (object) null, (Func<IScheduler, object, CancellationToken, Task>) ((self, o, ct) => action(self, ct)));
    }

    public static IDisposable ScheduleAsync(
      this IScheduler scheduler,
      Func<IScheduler, CancellationToken, Task<IDisposable>> action)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return Scheduler.ScheduleAsync_<object>(scheduler, (object) null, (Func<IScheduler, object, CancellationToken, Task<IDisposable>>) ((self, o, ct) => action(self, ct)));
    }

    public static IDisposable ScheduleAsync<TState>(
      this IScheduler scheduler,
      TState state,
      TimeSpan dueTime,
      Func<IScheduler, TState, CancellationToken, Task> action)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return Scheduler.ScheduleAsync_<TState>(scheduler, state, dueTime, action);
    }

    public static IDisposable ScheduleAsync<TState>(
      this IScheduler scheduler,
      TState state,
      TimeSpan dueTime,
      Func<IScheduler, TState, CancellationToken, Task<IDisposable>> action)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return Scheduler.ScheduleAsync_<TState>(scheduler, state, dueTime, action);
    }

    public static IDisposable ScheduleAsync(
      this IScheduler scheduler,
      TimeSpan dueTime,
      Func<IScheduler, CancellationToken, Task> action)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return Scheduler.ScheduleAsync_<object>(scheduler, (object) null, dueTime, (Func<IScheduler, object, CancellationToken, Task>) ((self, o, ct) => action(self, ct)));
    }

    public static IDisposable ScheduleAsync(
      this IScheduler scheduler,
      TimeSpan dueTime,
      Func<IScheduler, CancellationToken, Task<IDisposable>> action)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return Scheduler.ScheduleAsync_<object>(scheduler, (object) null, dueTime, (Func<IScheduler, object, CancellationToken, Task<IDisposable>>) ((self, o, ct) => action(self, ct)));
    }

    public static IDisposable ScheduleAsync<TState>(
      this IScheduler scheduler,
      TState state,
      DateTimeOffset dueTime,
      Func<IScheduler, TState, CancellationToken, Task> action)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return Scheduler.ScheduleAsync_<TState>(scheduler, state, dueTime, action);
    }

    public static IDisposable ScheduleAsync<TState>(
      this IScheduler scheduler,
      TState state,
      DateTimeOffset dueTime,
      Func<IScheduler, TState, CancellationToken, Task<IDisposable>> action)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return Scheduler.ScheduleAsync_<TState>(scheduler, state, dueTime, action);
    }

    public static IDisposable ScheduleAsync(
      this IScheduler scheduler,
      DateTimeOffset dueTime,
      Func<IScheduler, CancellationToken, Task> action)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return Scheduler.ScheduleAsync_<object>(scheduler, (object) null, dueTime, (Func<IScheduler, object, CancellationToken, Task>) ((self, o, ct) => action(self, ct)));
    }

    public static IDisposable ScheduleAsync(
      this IScheduler scheduler,
      DateTimeOffset dueTime,
      Func<IScheduler, CancellationToken, Task<IDisposable>> action)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return Scheduler.ScheduleAsync_<object>(scheduler, (object) null, dueTime, (Func<IScheduler, object, CancellationToken, Task<IDisposable>>) ((self, o, ct) => action(self, ct)));
    }

    private static IDisposable ScheduleAsync_<TState>(
      IScheduler scheduler,
      TState state,
      Func<IScheduler, TState, CancellationToken, Task> action)
    {
      return scheduler.Schedule<TState>(state, (Func<IScheduler, TState, IDisposable>) ((self, s) => Scheduler.InvokeAsync<TState>(self, s, action)));
    }

    private static IDisposable ScheduleAsync_<TState>(
      IScheduler scheduler,
      TState state,
      Func<IScheduler, TState, CancellationToken, Task<IDisposable>> action)
    {
      return scheduler.Schedule<TState>(state, (Func<IScheduler, TState, IDisposable>) ((self, s) => Scheduler.InvokeAsync<TState>(self, s, action)));
    }

    private static IDisposable ScheduleAsync_<TState>(
      IScheduler scheduler,
      TState state,
      TimeSpan dueTime,
      Func<IScheduler, TState, CancellationToken, Task> action)
    {
      return scheduler.Schedule<TState>(state, dueTime, (Func<IScheduler, TState, IDisposable>) ((self, s) => Scheduler.InvokeAsync<TState>(self, s, action)));
    }

    private static IDisposable ScheduleAsync_<TState>(
      IScheduler scheduler,
      TState state,
      TimeSpan dueTime,
      Func<IScheduler, TState, CancellationToken, Task<IDisposable>> action)
    {
      return scheduler.Schedule<TState>(state, dueTime, (Func<IScheduler, TState, IDisposable>) ((self, s) => Scheduler.InvokeAsync<TState>(self, s, action)));
    }

    private static IDisposable ScheduleAsync_<TState>(
      IScheduler scheduler,
      TState state,
      DateTimeOffset dueTime,
      Func<IScheduler, TState, CancellationToken, Task> action)
    {
      return scheduler.Schedule<TState>(state, dueTime, (Func<IScheduler, TState, IDisposable>) ((self, s) => Scheduler.InvokeAsync<TState>(self, s, action)));
    }

    private static IDisposable ScheduleAsync_<TState>(
      IScheduler scheduler,
      TState state,
      DateTimeOffset dueTime,
      Func<IScheduler, TState, CancellationToken, Task<IDisposable>> action)
    {
      return scheduler.Schedule<TState>(state, dueTime, (Func<IScheduler, TState, IDisposable>) ((self, s) => Scheduler.InvokeAsync<TState>(self, s, action)));
    }

    private static IDisposable InvokeAsync<TState>(
      IScheduler self,
      TState s,
      Func<IScheduler, TState, CancellationToken, Task<IDisposable>> action)
    {
      CancellationDisposable disposable1 = new CancellationDisposable();
      SingleAssignmentDisposable d = new SingleAssignmentDisposable();
      action((IScheduler) new Scheduler.CancelableScheduler(self, disposable1.Token), s, disposable1.Token).ContinueWith((Action<Task<IDisposable>>) (t =>
      {
        if (t.IsCanceled)
          return;
        if (t.Exception != null)
          t.Exception.Handle((Func<Exception, bool>) (e => e is OperationCanceledException));
        d.Disposable = t.Result;
      }), TaskContinuationOptions.ExecuteSynchronously);
      return (IDisposable) StableCompositeDisposable.Create((IDisposable) disposable1, (IDisposable) d);
    }

    private static IDisposable InvokeAsync<TState>(
      IScheduler self,
      TState s,
      Func<IScheduler, TState, CancellationToken, Task> action)
    {
      return Scheduler.InvokeAsync<TState>(self, s, (Func<IScheduler, TState, CancellationToken, Task<IDisposable>>) ((self_, state, ct) => action(self_, state, ct).ContinueWith<IDisposable>((Func<Task, IDisposable>) (_ => Disposable.Empty))));
    }

    private static CancellationToken GetCancellationToken(this IScheduler scheduler)
    {
      return !(scheduler is Scheduler.CancelableScheduler cancelableScheduler) ? CancellationToken.None : cancelableScheduler.Token;
    }

    public static DateTimeOffset Now => SystemClock.UtcNow;

    public static TimeSpan Normalize(TimeSpan timeSpan)
    {
      return timeSpan.Ticks < 0L ? TimeSpan.Zero : timeSpan;
    }

    public static ImmediateScheduler Immediate => ImmediateScheduler.Instance;

    public static CurrentThreadScheduler CurrentThread => CurrentThreadScheduler.Instance;

    public static DefaultScheduler Default => DefaultScheduler.Instance;

    [Obsolete("This property is no longer supported due to refactoring of the API surface and elimination of platform-specific dependencies. Consider using Scheduler.Default to obtain the platform's most appropriate pool-based scheduler. In order to access a specific pool-based scheduler, please add a reference to the System.Reactive.PlatformServices assembly for your target platform and use the appropriate scheduler in the System.Reactive.Concurrency namespace.")]
    public static IScheduler ThreadPool => Scheduler.s_threadPool.Value;

    [Obsolete("This property is no longer supported due to refactoring of the API surface and elimination of platform-specific dependencies. Please add a reference to the System.Reactive.PlatformServices assembly for your target platform and use NewThreadScheduler.Default to obtain an instance of this scheduler type.")]
    public static IScheduler NewThread => Scheduler.s_newThread.Value;

    [Obsolete("This property is no longer supported due to refactoring of the API surface and elimination of platform-specific dependencies. Please add a reference to the System.Reactive.PlatformServices assembly for your target platform and use TaskPoolScheduler.Default to obtain an instance of this scheduler type.")]
    public static IScheduler TaskPool => Scheduler.s_taskPool.Value;

    private static IScheduler Initialize(string name)
    {
      return PlatformEnlightenmentProvider.Current.GetService<IScheduler>((object) name) ?? throw new NotSupportedException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Strings_Core.CANT_OBTAIN_SCHEDULER, new object[1]
      {
        (object) name
      }));
    }

    public static IDisposable Schedule(this IScheduler scheduler, Action<Action> action)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return scheduler.Schedule<Action<Action>>(action, (Action<Action<Action>, Action<Action<Action>>>) ((_action, self) => _action((Action) (() => self(_action)))));
    }

    public static IDisposable Schedule<TState>(
      this IScheduler scheduler,
      TState state,
      Action<TState, Action<TState>> action)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return scheduler.Schedule<Scheduler.Pair<TState, Action<TState, Action<TState>>>>(new Scheduler.Pair<TState, Action<TState, Action<TState>>>()
      {
        First = state,
        Second = action
      }, new Func<IScheduler, Scheduler.Pair<TState, Action<TState, Action<TState>>>, IDisposable>(Scheduler.InvokeRec1<TState>));
    }

    private static IDisposable InvokeRec1<TState>(
      IScheduler scheduler,
      Scheduler.Pair<TState, Action<TState, Action<TState>>> pair)
    {
      CompositeDisposable group = new CompositeDisposable(1);
      object gate = new object();
      TState first = pair.First;
      Action<TState, Action<TState>> action = pair.Second;
      Action<TState> recursiveAction = (Action<TState>) null;
      recursiveAction = (Action<TState>) (state1 => action(state1, (Action<TState>) (state2 =>
      {
        bool isAdded = false;
        bool isDone = false;
        IDisposable d = (IDisposable) null;
        d = scheduler.Schedule<TState>(state2, (Func<IScheduler, TState, IDisposable>) ((scheduler1, state3) =>
        {
          lock (gate)
          {
            if (isAdded)
              group.Remove(d);
            else
              isDone = true;
          }
          recursiveAction(state3);
          return Disposable.Empty;
        }));
        lock (gate)
        {
          if (isDone)
            return;
          group.Add(d);
          isAdded = true;
        }
      })));
      recursiveAction(first);
      return (IDisposable) group;
    }

    public static IDisposable Schedule(
      this IScheduler scheduler,
      TimeSpan dueTime,
      Action<Action<TimeSpan>> action)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return scheduler.Schedule<Action<Action<TimeSpan>>>(action, dueTime, (Action<Action<Action<TimeSpan>>, Action<Action<Action<TimeSpan>>, TimeSpan>>) ((_action, self) => _action((Action<TimeSpan>) (dt => self(_action, dt)))));
    }

    public static IDisposable Schedule<TState>(
      this IScheduler scheduler,
      TState state,
      TimeSpan dueTime,
      Action<TState, Action<TState, TimeSpan>> action)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return scheduler.Schedule<Scheduler.Pair<TState, Action<TState, Action<TState, TimeSpan>>>>(new Scheduler.Pair<TState, Action<TState, Action<TState, TimeSpan>>>()
      {
        First = state,
        Second = action
      }, dueTime, new Func<IScheduler, Scheduler.Pair<TState, Action<TState, Action<TState, TimeSpan>>>, IDisposable>(Scheduler.InvokeRec2<TState>));
    }

    private static IDisposable InvokeRec2<TState>(
      IScheduler scheduler,
      Scheduler.Pair<TState, Action<TState, Action<TState, TimeSpan>>> pair)
    {
      CompositeDisposable group = new CompositeDisposable(1);
      object gate = new object();
      TState first = pair.First;
      Action<TState, Action<TState, TimeSpan>> action = pair.Second;
      Action<TState> recursiveAction = (Action<TState>) null;
      recursiveAction = (Action<TState>) (state1 => action(state1, (Action<TState, TimeSpan>) ((state2, dueTime1) =>
      {
        bool isAdded = false;
        bool isDone = false;
        IDisposable d = (IDisposable) null;
        d = scheduler.Schedule<TState>(state2, dueTime1, (Func<IScheduler, TState, IDisposable>) ((scheduler1, state3) =>
        {
          lock (gate)
          {
            if (isAdded)
              group.Remove(d);
            else
              isDone = true;
          }
          recursiveAction(state3);
          return Disposable.Empty;
        }));
        lock (gate)
        {
          if (isDone)
            return;
          group.Add(d);
          isAdded = true;
        }
      })));
      recursiveAction(first);
      return (IDisposable) group;
    }

    public static IDisposable Schedule(
      this IScheduler scheduler,
      DateTimeOffset dueTime,
      Action<Action<DateTimeOffset>> action)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return scheduler.Schedule<Action<Action<DateTimeOffset>>>(action, dueTime, (Action<Action<Action<DateTimeOffset>>, Action<Action<Action<DateTimeOffset>>, DateTimeOffset>>) ((_action, self) => _action((Action<DateTimeOffset>) (dt => self(_action, dt)))));
    }

    public static IDisposable Schedule<TState>(
      this IScheduler scheduler,
      TState state,
      DateTimeOffset dueTime,
      Action<TState, Action<TState, DateTimeOffset>> action)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return scheduler.Schedule<Scheduler.Pair<TState, Action<TState, Action<TState, DateTimeOffset>>>>(new Scheduler.Pair<TState, Action<TState, Action<TState, DateTimeOffset>>>()
      {
        First = state,
        Second = action
      }, dueTime, new Func<IScheduler, Scheduler.Pair<TState, Action<TState, Action<TState, DateTimeOffset>>>, IDisposable>(Scheduler.InvokeRec3<TState>));
    }

    private static IDisposable InvokeRec3<TState>(
      IScheduler scheduler,
      Scheduler.Pair<TState, Action<TState, Action<TState, DateTimeOffset>>> pair)
    {
      CompositeDisposable group = new CompositeDisposable(1);
      object gate = new object();
      TState first = pair.First;
      Action<TState, Action<TState, DateTimeOffset>> action = pair.Second;
      Action<TState> recursiveAction = (Action<TState>) null;
      recursiveAction = (Action<TState>) (state1 => action(state1, (Action<TState, DateTimeOffset>) ((state2, dueTime1) =>
      {
        bool isAdded = false;
        bool isDone = false;
        IDisposable d = (IDisposable) null;
        d = scheduler.Schedule<TState>(state2, dueTime1, (Func<IScheduler, TState, IDisposable>) ((scheduler1, state3) =>
        {
          lock (gate)
          {
            if (isAdded)
              group.Remove(d);
            else
              isDone = true;
          }
          recursiveAction(state3);
          return Disposable.Empty;
        }));
        lock (gate)
        {
          if (isDone)
            return;
          group.Add(d);
          isAdded = true;
        }
      })));
      recursiveAction(first);
      return (IDisposable) group;
    }

    public static ISchedulerLongRunning AsLongRunning(this IScheduler scheduler)
    {
      return scheduler is IServiceProvider serviceProvider ? (ISchedulerLongRunning) serviceProvider.GetService(typeof (ISchedulerLongRunning)) : (ISchedulerLongRunning) null;
    }

    public static IStopwatchProvider AsStopwatchProvider(this IScheduler scheduler)
    {
      return scheduler is IServiceProvider serviceProvider ? (IStopwatchProvider) serviceProvider.GetService(typeof (IStopwatchProvider)) : (IStopwatchProvider) null;
    }

    public static ISchedulerPeriodic AsPeriodic(this IScheduler scheduler)
    {
      return scheduler is IServiceProvider serviceProvider ? (ISchedulerPeriodic) serviceProvider.GetService(typeof (ISchedulerPeriodic)) : (ISchedulerPeriodic) null;
    }

    public static IDisposable SchedulePeriodic<TState>(
      this IScheduler scheduler,
      TState state,
      TimeSpan period,
      Func<TState, TState> action)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (period < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (period));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return Scheduler.SchedulePeriodic_<TState>(scheduler, state, period, action);
    }

    public static IDisposable SchedulePeriodic<TState>(
      this IScheduler scheduler,
      TState state,
      TimeSpan period,
      Action<TState> action)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (period < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (period));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return Scheduler.SchedulePeriodic_<TState>(scheduler, state, period, (Func<TState, TState>) (state_ =>
      {
        action(state_);
        return state_;
      }));
    }

    public static IDisposable SchedulePeriodic(
      this IScheduler scheduler,
      TimeSpan period,
      Action action)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (period < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException(nameof (period));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return Scheduler.SchedulePeriodic_<Action>(scheduler, action, period, (Func<Action, Action>) (a =>
      {
        a();
        return a;
      }));
    }

    public static IStopwatch StartStopwatch(this IScheduler scheduler)
    {
      IStopwatchProvider stopwatchProvider = scheduler != null ? scheduler.AsStopwatchProvider() : throw new ArgumentNullException(nameof (scheduler));
      return stopwatchProvider != null ? stopwatchProvider.StartStopwatch() : (IStopwatch) new Scheduler.EmulatedStopwatch(scheduler);
    }

    private static IDisposable SchedulePeriodic_<TState>(
      IScheduler scheduler,
      TState state,
      TimeSpan period,
      Func<TState, TState> action)
    {
      ISchedulerPeriodic schedulerPeriodic = scheduler.AsPeriodic();
      if (schedulerPeriodic != null)
        return schedulerPeriodic.SchedulePeriodic<TState>(state, period, action);
      IStopwatchProvider stopwatchProvider = scheduler.AsStopwatchProvider();
      return stopwatchProvider != null ? new Scheduler.SchedulePeriodicStopwatch<TState>(scheduler, state, period, action, stopwatchProvider).Start() : new Scheduler.SchedulePeriodicRecursive<TState>(scheduler, state, period, action).Start();
    }

    public static IDisposable Schedule(this IScheduler scheduler, Action action)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return action != null ? scheduler.Schedule<Action>(action, new Func<IScheduler, Action, IDisposable>(Scheduler.Invoke)) : throw new ArgumentNullException(nameof (action));
    }

    public static IDisposable Schedule(this IScheduler scheduler, TimeSpan dueTime, Action action)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return scheduler.Schedule<Action>(action, dueTime, new Func<IScheduler, Action, IDisposable>(Scheduler.Invoke));
    }

    public static IDisposable Schedule(
      this IScheduler scheduler,
      DateTimeOffset dueTime,
      Action action)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return scheduler.Schedule<Action>(action, dueTime, new Func<IScheduler, Action, IDisposable>(Scheduler.Invoke));
    }

    public static IDisposable ScheduleLongRunning(
      this ISchedulerLongRunning scheduler,
      Action<ICancelable> action)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return scheduler.ScheduleLongRunning<Action<ICancelable>>(action, (Action<Action<ICancelable>, ICancelable>) ((a, c) => a(c)));
    }

    private static IDisposable Invoke(IScheduler scheduler, Action action)
    {
      action();
      return Disposable.Empty;
    }

    public static IScheduler DisableOptimizations(this IScheduler scheduler)
    {
      return scheduler != null ? (IScheduler) new DisableOptimizationsScheduler(scheduler) : throw new ArgumentNullException(nameof (scheduler));
    }

    public static IScheduler DisableOptimizations(
      this IScheduler scheduler,
      params Type[] optimizationInterfaces)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return optimizationInterfaces != null ? (IScheduler) new DisableOptimizationsScheduler(scheduler, optimizationInterfaces) : throw new ArgumentNullException(nameof (optimizationInterfaces));
    }

    public static IScheduler Catch<TException>(
      this IScheduler scheduler,
      Func<TException, bool> handler)
      where TException : Exception
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return handler != null ? (IScheduler) new CatchScheduler<TException>(scheduler, handler) : throw new ArgumentNullException(nameof (handler));
    }

    private class CancelableScheduler : IScheduler
    {
      private readonly IScheduler _scheduler;
      private readonly CancellationToken _cancellationToken;

      public CancelableScheduler(IScheduler scheduler, CancellationToken cancellationToken)
      {
        this._scheduler = scheduler;
        this._cancellationToken = cancellationToken;
      }

      public CancellationToken Token => this._cancellationToken;

      public DateTimeOffset Now => this._scheduler.Now;

      public IDisposable Schedule<TState>(
        TState state,
        Func<IScheduler, TState, IDisposable> action)
      {
        return this._scheduler.Schedule<TState>(state, action);
      }

      public IDisposable Schedule<TState>(
        TState state,
        TimeSpan dueTime,
        Func<IScheduler, TState, IDisposable> action)
      {
        return this._scheduler.Schedule<TState>(state, dueTime, action);
      }

      public IDisposable Schedule<TState>(
        TState state,
        DateTimeOffset dueTime,
        Func<IScheduler, TState, IDisposable> action)
      {
        return this._scheduler.Schedule<TState>(state, dueTime, action);
      }
    }

    [Serializable]
    private struct Pair<T1, T2>
    {
      public T1 First;
      public T2 Second;
    }

    private class SchedulePeriodicStopwatch<TState>
    {
      private readonly IScheduler _scheduler;
      private readonly TimeSpan _period;
      private readonly Func<TState, TState> _action;
      private readonly IStopwatchProvider _stopwatchProvider;
      private TState _state;
      private readonly object _gate = new object();
      private readonly AutoResetEvent _resumeEvent = new AutoResetEvent(false);
      private volatile int _runState;
      private IStopwatch _stopwatch;
      private TimeSpan _nextDue;
      private TimeSpan _suspendedAt;
      private TimeSpan _inactiveTime;
      private const int STOPPED = 0;
      private const int RUNNING = 1;
      private const int SUSPENDED = 2;
      private const int DISPOSED = 3;

      public SchedulePeriodicStopwatch(
        IScheduler scheduler,
        TState state,
        TimeSpan period,
        Func<TState, TState> action,
        IStopwatchProvider stopwatchProvider)
      {
        this._scheduler = scheduler;
        this._period = period;
        this._action = action;
        this._stopwatchProvider = stopwatchProvider;
        this._state = state;
        this._runState = 0;
      }

      public IDisposable Start()
      {
        this.RegisterHostLifecycleEventHandlers();
        this._stopwatch = this._stopwatchProvider.StartStopwatch();
        this._nextDue = this._period;
        this._runState = 1;
        return (IDisposable) StableCompositeDisposable.Create(this._scheduler.Schedule(this._nextDue, new Action<Action<TimeSpan>>(this.Tick)), Disposable.Create(new Action(this.Cancel)));
      }

      private void Tick(Action<TimeSpan> recurse)
      {
        this._nextDue += this._period;
        this._state = this._action(this._state);
        TimeSpan timeSpan = new TimeSpan();
        while (true)
        {
          bool flag;
          do
          {
            flag = false;
            lock (this._gate)
            {
              if (this._runState == 1)
              {
                timeSpan = Scheduler.Normalize(this._nextDue - (this._stopwatch.Elapsed - this._inactiveTime));
                goto label_11;
              }
              else
              {
                if (this._runState == 3)
                  return;
                flag = true;
              }
            }
          }
          while (!flag);
          this._resumeEvent.WaitOne();
        }
label_11:
        recurse(timeSpan);
      }

      private void Cancel()
      {
        this.UnregisterHostLifecycleEventHandlers();
        lock (this._gate)
        {
          this._runState = 3;
          if (Environment.HasShutdownStarted)
            return;
          this._resumeEvent.Set();
        }
      }

      private void Suspending(object sender, HostSuspendingEventArgs args)
      {
        lock (this._gate)
        {
          if (this._runState != 1)
            return;
          this._suspendedAt = this._stopwatch.Elapsed;
          this._runState = 2;
          if (Environment.HasShutdownStarted)
            return;
          this._resumeEvent.Reset();
        }
      }

      private void Resuming(object sender, HostResumingEventArgs args)
      {
        lock (this._gate)
        {
          if (this._runState != 2)
            return;
          this._inactiveTime += this._stopwatch.Elapsed - this._suspendedAt;
          this._runState = 1;
          if (Environment.HasShutdownStarted)
            return;
          this._resumeEvent.Set();
        }
      }

      private void RegisterHostLifecycleEventHandlers()
      {
        HostLifecycleService.Suspending += new EventHandler<HostSuspendingEventArgs>(this.Suspending);
        HostLifecycleService.Resuming += new EventHandler<HostResumingEventArgs>(this.Resuming);
        HostLifecycleService.AddRef();
      }

      private void UnregisterHostLifecycleEventHandlers()
      {
        HostLifecycleService.Suspending -= new EventHandler<HostSuspendingEventArgs>(this.Suspending);
        HostLifecycleService.Resuming -= new EventHandler<HostResumingEventArgs>(this.Resuming);
        HostLifecycleService.Release();
      }
    }

    private class SchedulePeriodicRecursive<TState>
    {
      private readonly IScheduler _scheduler;
      private readonly TimeSpan _period;
      private readonly Func<TState, TState> _action;
      private TState _state;
      private int _pendingTickCount;
      private IDisposable _cancel;
      private const int TICK = 0;
      private const int DISPATCH_START = 1;
      private const int DISPATCH_END = 2;

      public SchedulePeriodicRecursive(
        IScheduler scheduler,
        TState state,
        TimeSpan period,
        Func<TState, TState> action)
      {
        this._scheduler = scheduler;
        this._period = period;
        this._action = action;
        this._state = state;
      }

      public IDisposable Start()
      {
        this._pendingTickCount = 0;
        SingleAssignmentDisposable assignmentDisposable = new SingleAssignmentDisposable();
        this._cancel = (IDisposable) assignmentDisposable;
        assignmentDisposable.Disposable = this._scheduler.Schedule<int>(0, this._period, new Action<int, Action<int, TimeSpan>>(this.Tick));
        return (IDisposable) assignmentDisposable;
      }

      private void Tick(int command, Action<int, TimeSpan> recurse)
      {
        switch (command)
        {
          case 0:
            recurse(0, this._period);
            if (Interlocked.Increment(ref this._pendingTickCount) != 1)
              break;
            goto case 1;
          case 1:
            try
            {
              this._state = this._action(this._state);
            }
            catch (Exception ex)
            {
              this._cancel.Dispose();
              ex.Throw();
            }
            recurse(2, TimeSpan.Zero);
            break;
          case 2:
            if (Interlocked.Decrement(ref this._pendingTickCount) <= 0)
              break;
            recurse(1, TimeSpan.Zero);
            break;
        }
      }
    }

    private class EmulatedStopwatch : IStopwatch
    {
      private readonly IScheduler _scheduler;
      private readonly DateTimeOffset _start;

      public EmulatedStopwatch(IScheduler scheduler)
      {
        this._scheduler = scheduler;
        this._start = this._scheduler.Now;
      }

      public TimeSpan Elapsed => Scheduler.Normalize(this._scheduler.Now - this._start);
    }
  }
}
