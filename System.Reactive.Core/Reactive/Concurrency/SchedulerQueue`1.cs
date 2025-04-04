// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.SchedulerQueue`1
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

#nullable disable
namespace System.Reactive.Concurrency
{
  public class SchedulerQueue<TAbsolute> where TAbsolute : IComparable<TAbsolute>
  {
    private readonly PriorityQueue<ScheduledItem<TAbsolute>> _queue;

    public SchedulerQueue()
      : this(1024)
    {
    }

    public SchedulerQueue(int capacity)
    {
      this._queue = capacity >= 0 ? new PriorityQueue<ScheduledItem<TAbsolute>>(capacity) : throw new ArgumentOutOfRangeException(nameof (capacity));
    }

    public int Count => this._queue.Count;

    public void Enqueue(ScheduledItem<TAbsolute> scheduledItem)
    {
      this._queue.Enqueue(scheduledItem);
    }

    public bool Remove(ScheduledItem<TAbsolute> scheduledItem) => this._queue.Remove(scheduledItem);

    public ScheduledItem<TAbsolute> Dequeue() => this._queue.Dequeue();

    public ScheduledItem<TAbsolute> Peek() => this._queue.Peek();
  }
}
