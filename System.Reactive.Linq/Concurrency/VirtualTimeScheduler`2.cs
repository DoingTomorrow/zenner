// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.VirtualTimeScheduler`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Concurrency
{
  public abstract class VirtualTimeScheduler<TAbsolute, TRelative> : 
    VirtualTimeSchedulerBase<TAbsolute, TRelative>
    where TAbsolute : IComparable<TAbsolute>
  {
    private readonly SchedulerQueue<TAbsolute> queue = new SchedulerQueue<TAbsolute>();

    protected VirtualTimeScheduler()
    {
    }

    protected VirtualTimeScheduler(TAbsolute initialClock, IComparer<TAbsolute> comparer)
      : base(initialClock, comparer)
    {
    }

    protected override IScheduledItem<TAbsolute> GetNext()
    {
      lock (this.queue)
      {
        while (this.queue.Count > 0)
        {
          ScheduledItem<TAbsolute> next = this.queue.Peek();
          if (!next.IsCanceled)
            return (IScheduledItem<TAbsolute>) next;
          this.queue.Dequeue();
        }
      }
      return (IScheduledItem<TAbsolute>) null;
    }

    public override IDisposable ScheduleAbsolute<TState>(
      TState state,
      TAbsolute dueTime,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      ScheduledItem<TAbsolute, TState> si = (ScheduledItem<TAbsolute, TState>) null;
      Func<IScheduler, TState, IDisposable> action1 = (Func<IScheduler, TState, IDisposable>) ((scheduler, state1) =>
      {
        lock (this.queue)
          this.queue.Remove((ScheduledItem<TAbsolute>) si);
        return action(scheduler, state1);
      });
      si = new ScheduledItem<TAbsolute, TState>((IScheduler) this, state, action1, dueTime, this.Comparer);
      lock (this.queue)
        this.queue.Enqueue((ScheduledItem<TAbsolute>) si);
      return Disposable.Create(new Action(((ScheduledItem<TAbsolute>) si).Cancel));
    }
  }
}
