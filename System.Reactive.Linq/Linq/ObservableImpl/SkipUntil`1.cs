// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.SkipUntil`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class SkipUntil<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly DateTimeOffset _startTime;
    internal readonly IScheduler _scheduler;

    public SkipUntil(IObservable<TSource> source, DateTimeOffset startTime, IScheduler scheduler)
    {
      this._source = source;
      this._startTime = startTime;
      this._scheduler = scheduler;
    }

    public IObservable<TSource> Omega(DateTimeOffset startTime)
    {
      return startTime <= this._startTime ? (IObservable<TSource>) this : (IObservable<TSource>) new SkipUntil<TSource>(this._source, startTime, this._scheduler);
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      SkipUntil<TSource>._ obj = new SkipUntil<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly SkipUntil<TSource> _parent;
      private volatile bool _open;

      public _(SkipUntil<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        return (IDisposable) StableCompositeDisposable.Create(this._parent._scheduler.Schedule(this._parent._startTime, new Action(this.Tick)), this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this));
      }

      private void Tick() => this._open = true;

      public void OnNext(TSource value)
      {
        if (!this._open)
          return;
        this._observer.OnNext(value);
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
