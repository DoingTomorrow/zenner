// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.SkipLast`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Concurrency;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class SkipLast<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly int _count;
    private readonly TimeSpan _duration;
    private readonly IScheduler _scheduler;

    public SkipLast(IObservable<TSource> source, int count)
    {
      this._source = source;
      this._count = count;
    }

    public SkipLast(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler)
    {
      this._source = source;
      this._duration = duration;
      this._scheduler = scheduler;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._scheduler == null)
      {
        SkipLast<TSource>._ obj = new SkipLast<TSource>._(this, observer, cancel);
        setSink((IDisposable) obj);
        return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
      }
      SkipLast<TSource>.SkipLastImpl skipLastImpl = new SkipLast<TSource>.SkipLastImpl(this, observer, cancel);
      setSink((IDisposable) skipLastImpl);
      return skipLastImpl.Run();
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly SkipLast<TSource> _parent;
      private Queue<TSource> _queue;

      public _(SkipLast<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._queue = new Queue<TSource>();
      }

      public void OnNext(TSource value)
      {
        this._queue.Enqueue(value);
        if (this._queue.Count <= this._parent._count)
          return;
        this._observer.OnNext(this._queue.Dequeue());
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnCompleted();
        this.Dispose();
      }
    }

    private class SkipLastImpl : Sink<TSource>, IObserver<TSource>
    {
      private readonly SkipLast<TSource> _parent;
      private Queue<System.Reactive.TimeInterval<TSource>> _queue;
      private IStopwatch _watch;

      public SkipLastImpl(
        SkipLast<TSource> parent,
        IObserver<TSource> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._queue = new Queue<System.Reactive.TimeInterval<TSource>>();
      }

      public IDisposable Run()
      {
        this._watch = this._parent._scheduler.StartStopwatch();
        return this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this);
      }

      public void OnNext(TSource value)
      {
        TimeSpan elapsed = this._watch.Elapsed;
        this._queue.Enqueue(new System.Reactive.TimeInterval<TSource>(value, elapsed));
        while (this._queue.Count > 0 && elapsed - this._queue.Peek().Interval >= this._parent._duration)
          this._observer.OnNext(this._queue.Dequeue().Value);
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        TimeSpan elapsed = this._watch.Elapsed;
        while (this._queue.Count > 0)
        {
          TimeSpan timeSpan = elapsed;
          System.Reactive.TimeInterval<TSource> timeInterval = this._queue.Peek();
          TimeSpan interval = timeInterval.Interval;
          if (timeSpan - interval >= this._parent._duration)
          {
            IObserver<TSource> observer = this._observer;
            timeInterval = this._queue.Dequeue();
            TSource source = timeInterval.Value;
            observer.OnNext(source);
          }
          else
            break;
        }
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
