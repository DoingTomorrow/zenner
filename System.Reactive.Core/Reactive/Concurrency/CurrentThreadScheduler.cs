// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.CurrentThreadScheduler
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.ComponentModel;
using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Concurrency
{
  public sealed class CurrentThreadScheduler : LocalScheduler
  {
    private static readonly Lazy<CurrentThreadScheduler> s_instance = new Lazy<CurrentThreadScheduler>((Func<CurrentThreadScheduler>) (() => new CurrentThreadScheduler()));
    [ThreadStatic]
    private static SchedulerQueue<TimeSpan> s_threadLocalQueue;
    [ThreadStatic]
    private static IStopwatch s_clock;

    private CurrentThreadScheduler()
    {
    }

    public static CurrentThreadScheduler Instance => CurrentThreadScheduler.s_instance.Value;

    private static SchedulerQueue<TimeSpan> GetQueue() => CurrentThreadScheduler.s_threadLocalQueue;

    private static void SetQueue(SchedulerQueue<TimeSpan> newQueue)
    {
      CurrentThreadScheduler.s_threadLocalQueue = newQueue;
    }

    private static TimeSpan Time
    {
      get
      {
        if (CurrentThreadScheduler.s_clock == null)
          CurrentThreadScheduler.s_clock = ConcurrencyAbstractionLayer.Current.StartStopwatch();
        return CurrentThreadScheduler.s_clock.Elapsed;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This instance property is no longer supported. Use CurrentThreadScheduler.IsScheduleRequired instead.")]
    public bool ScheduleRequired => CurrentThreadScheduler.IsScheduleRequired;

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static bool IsScheduleRequired => CurrentThreadScheduler.GetQueue() == null;

    public override IDisposable Schedule<TState>(
      TState state,
      TimeSpan dueTime,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      TimeSpan dueTime1 = CurrentThreadScheduler.Time + Scheduler.Normalize(dueTime);
      ScheduledItem<TimeSpan, TState> scheduledItem = new ScheduledItem<TimeSpan, TState>((IScheduler) this, state, action, dueTime1);
      SchedulerQueue<TimeSpan> queue = CurrentThreadScheduler.GetQueue();
      if (queue == null)
      {
        SchedulerQueue<TimeSpan> schedulerQueue = new SchedulerQueue<TimeSpan>(4);
        schedulerQueue.Enqueue((ScheduledItem<TimeSpan>) scheduledItem);
        CurrentThreadScheduler.SetQueue(schedulerQueue);
        try
        {
          CurrentThreadScheduler.Trampoline.Run(schedulerQueue);
        }
        finally
        {
          CurrentThreadScheduler.SetQueue((SchedulerQueue<TimeSpan>) null);
        }
      }
      else
        queue.Enqueue((ScheduledItem<TimeSpan>) scheduledItem);
      return Disposable.Create(new Action(((ScheduledItem<TimeSpan>) scheduledItem).Cancel));
    }

    private static class Trampoline
    {
      public static void Run(SchedulerQueue<TimeSpan> queue)
      {
        while (queue.Count > 0)
        {
          ScheduledItem<TimeSpan> scheduledItem = queue.Dequeue();
          if (!scheduledItem.IsCanceled)
          {
            TimeSpan timeout = scheduledItem.DueTime - CurrentThreadScheduler.Time;
            if (timeout.Ticks > 0L)
              ConcurrencyAbstractionLayer.Current.Sleep(timeout);
            if (!scheduledItem.IsCanceled)
              scheduledItem.Invoke();
          }
        }
      }
    }
  }
}
