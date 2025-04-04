// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.HistoricalScheduler
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Concurrency
{
  public class HistoricalScheduler : HistoricalSchedulerBase
  {
    private readonly SchedulerQueue<DateTimeOffset> queue = new SchedulerQueue<DateTimeOffset>();

    public HistoricalScheduler()
    {
    }

    public HistoricalScheduler(DateTimeOffset initialClock)
      : base(initialClock)
    {
    }

    public HistoricalScheduler(DateTimeOffset initialClock, IComparer<DateTimeOffset> comparer)
      : base(initialClock, comparer)
    {
    }

    protected override IScheduledItem<DateTimeOffset> GetNext()
    {
      while (this.queue.Count > 0)
      {
        ScheduledItem<DateTimeOffset> next = this.queue.Peek();
        if (!next.IsCanceled)
          return (IScheduledItem<DateTimeOffset>) next;
        this.queue.Dequeue();
      }
      return (IScheduledItem<DateTimeOffset>) null;
    }

    public override IDisposable ScheduleAbsolute<TState>(
      TState state,
      DateTimeOffset dueTime,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      ScheduledItem<DateTimeOffset, TState> si = (ScheduledItem<DateTimeOffset, TState>) null;
      Func<IScheduler, TState, IDisposable> action1 = (Func<IScheduler, TState, IDisposable>) ((scheduler, state1) =>
      {
        this.queue.Remove((ScheduledItem<DateTimeOffset>) si);
        return action(scheduler, state1);
      });
      si = new ScheduledItem<DateTimeOffset, TState>((IScheduler) this, state, action1, dueTime, this.Comparer);
      this.queue.Enqueue((ScheduledItem<DateTimeOffset>) si);
      return Disposable.Create(new Action(((ScheduledItem<DateTimeOffset>) si).Cancel));
    }
  }
}
