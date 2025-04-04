// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Skip`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Skip<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly int _count;
    private readonly TimeSpan _duration;
    internal readonly IScheduler _scheduler;

    public Skip(IObservable<TSource> source, int count)
    {
      this._source = source;
      this._count = count;
    }

    public Skip(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler)
    {
      this._source = source;
      this._duration = duration;
      this._scheduler = scheduler;
    }

    public IObservable<TSource> Omega(int count)
    {
      return (IObservable<TSource>) new Skip<TSource>(this._source, this._count + count);
    }

    public IObservable<TSource> Omega(TimeSpan duration)
    {
      return duration <= this._duration ? (IObservable<TSource>) this : (IObservable<TSource>) new Skip<TSource>(this._source, duration, this._scheduler);
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._scheduler == null)
      {
        Skip<TSource>._ obj = new Skip<TSource>._(this, observer, cancel);
        setSink((IDisposable) obj);
        return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
      }
      Skip<TSource>.SkipImpl skipImpl = new Skip<TSource>.SkipImpl(this, observer, cancel);
      setSink((IDisposable) skipImpl);
      return skipImpl.Run();
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Skip<TSource> _parent;
      private int _remaining;

      public _(Skip<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._remaining = this._parent._count;
      }

      public void OnNext(TSource value)
      {
        if (this._remaining <= 0)
          this._observer.OnNext(value);
        else
          --this._remaining;
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

    private class SkipImpl : Sink<TSource>, IObserver<TSource>
    {
      private readonly Skip<TSource> _parent;
      private volatile bool _open;

      public SkipImpl(Skip<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        return (IDisposable) StableCompositeDisposable.Create(this._parent._scheduler.Schedule(this._parent._duration, new Action(this.Tick)), this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this));
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
