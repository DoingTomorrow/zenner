// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.TimeInterval`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Concurrency;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class TimeInterval<TSource> : Producer<System.Reactive.TimeInterval<TSource>>
  {
    private readonly IObservable<TSource> _source;
    private readonly IScheduler _scheduler;

    public TimeInterval(IObservable<TSource> source, IScheduler scheduler)
    {
      this._source = source;
      this._scheduler = scheduler;
    }

    protected override IDisposable Run(
      IObserver<System.Reactive.TimeInterval<TSource>> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      TimeInterval<TSource>._ obj = new TimeInterval<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<System.Reactive.TimeInterval<TSource>>, IObserver<TSource>
    {
      private readonly TimeInterval<TSource> _parent;
      private IStopwatch _watch;
      private TimeSpan _last;

      public _(
        TimeInterval<TSource> parent,
        IObserver<System.Reactive.TimeInterval<TSource>> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._watch = this._parent._scheduler.StartStopwatch();
        this._last = TimeSpan.Zero;
        return this._parent._source.Subscribe((IObserver<TSource>) this);
      }

      public void OnNext(TSource value)
      {
        TimeSpan elapsed = this._watch.Elapsed;
        TimeSpan interval = elapsed.Subtract(this._last);
        this._last = elapsed;
        this._observer.OnNext(new System.Reactive.TimeInterval<TSource>(value, interval));
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
