// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Timestamp`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Concurrency;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Timestamp<TSource> : Producer<Timestamped<TSource>>
  {
    private readonly IObservable<TSource> _source;
    private readonly IScheduler _scheduler;

    public Timestamp(IObservable<TSource> source, IScheduler scheduler)
    {
      this._source = source;
      this._scheduler = scheduler;
    }

    protected override IDisposable Run(
      IObserver<Timestamped<TSource>> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Timestamp<TSource>._ obj = new Timestamp<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
    }

    private class _ : Sink<Timestamped<TSource>>, IObserver<TSource>
    {
      private readonly Timestamp<TSource> _parent;

      public _(
        Timestamp<TSource> parent,
        IObserver<Timestamped<TSource>> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public void OnNext(TSource value)
      {
        this._observer.OnNext(new Timestamped<TSource>(value, this._parent._scheduler.Now));
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
  }
}
