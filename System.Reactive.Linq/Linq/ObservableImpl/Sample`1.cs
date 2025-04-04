// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Sample`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Sample<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly TimeSpan _interval;
    private readonly IScheduler _scheduler;

    public Sample(IObservable<TSource> source, TimeSpan interval, IScheduler scheduler)
    {
      this._source = source;
      this._interval = interval;
      this._scheduler = scheduler;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Sample<TSource>._ obj = new Sample<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Sample<TSource> _parent;
      private object _gate;
      private IDisposable _sourceSubscription;
      private bool _hasValue;
      private TSource _value;
      private bool _atEnd;

      public _(Sample<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        SingleAssignmentDisposable disposable1 = new SingleAssignmentDisposable();
        this._sourceSubscription = (IDisposable) disposable1;
        disposable1.Disposable = this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this);
        return (IDisposable) StableCompositeDisposable.Create((IDisposable) disposable1, this._parent._scheduler.SchedulePeriodic(this._parent._interval, new Action(this.Tick)));
      }

      private void Tick()
      {
        lock (this._gate)
        {
          if (this._hasValue)
          {
            this._hasValue = false;
            this._observer.OnNext(this._value);
          }
          if (!this._atEnd)
            return;
          this._observer.OnCompleted();
          this.Dispose();
        }
      }

      public void OnNext(TSource value)
      {
        lock (this._gate)
        {
          this._hasValue = true;
          this._value = value;
        }
      }

      public void OnError(Exception error)
      {
        lock (this._gate)
        {
          this._observer.OnError(error);
          this.Dispose();
        }
      }

      public void OnCompleted()
      {
        lock (this._gate)
        {
          this._atEnd = true;
          this._sourceSubscription.Dispose();
        }
      }
    }
  }
}
