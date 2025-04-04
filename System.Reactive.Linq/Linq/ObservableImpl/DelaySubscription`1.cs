// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.DelaySubscription`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Concurrency;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class DelaySubscription<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly DateTimeOffset? _dueTimeA;
    private readonly TimeSpan? _dueTimeR;
    private readonly IScheduler _scheduler;

    public DelaySubscription(
      IObservable<TSource> source,
      DateTimeOffset dueTime,
      IScheduler scheduler)
    {
      this._source = source;
      this._dueTimeA = new DateTimeOffset?(dueTime);
      this._scheduler = scheduler;
    }

    public DelaySubscription(IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
    {
      this._source = source;
      this._dueTimeR = new TimeSpan?(dueTime);
      this._scheduler = scheduler;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      DelaySubscription<TSource>._ state = new DelaySubscription<TSource>._(observer, cancel);
      setSink((IDisposable) state);
      return this._dueTimeA.HasValue ? this._scheduler.Schedule<DelaySubscription<TSource>._>(state, this._dueTimeA.Value, new Func<IScheduler, DelaySubscription<TSource>._, IDisposable>(this.Subscribe)) : this._scheduler.Schedule<DelaySubscription<TSource>._>(state, this._dueTimeR.Value, new Func<IScheduler, DelaySubscription<TSource>._, IDisposable>(this.Subscribe));
    }

    private IDisposable Subscribe(IScheduler _, DelaySubscription<TSource>._ sink)
    {
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) sink);
    }

    private class _(IObserver<TSource> observer, IDisposable cancel) : 
      Sink<TSource>(observer, cancel),
      IObserver<TSource>
    {
      public void OnNext(TSource value) => this._observer.OnNext(value);

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
  }
}
