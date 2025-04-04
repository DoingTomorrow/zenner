// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Throttle`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Throttle<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly TimeSpan _dueTime;
    private readonly IScheduler _scheduler;

    public Throttle(IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
    {
      this._source = source;
      this._dueTime = dueTime;
      this._scheduler = scheduler;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Throttle<TSource>._ obj = new Throttle<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Throttle<TSource> _parent;
      private object _gate;
      private TSource _value;
      private bool _hasValue;
      private SerialDisposable _cancelable;
      private ulong _id;

      public _(Throttle<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        this._value = default (TSource);
        this._hasValue = false;
        this._cancelable = new SerialDisposable();
        this._id = 0UL;
        return (IDisposable) StableCompositeDisposable.Create(this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this), (IDisposable) this._cancelable);
      }

      public void OnNext(TSource value)
      {
        ulong state = 0;
        lock (this._gate)
        {
          this._hasValue = true;
          this._value = value;
          ++this._id;
          state = this._id;
        }
        SingleAssignmentDisposable assignmentDisposable = new SingleAssignmentDisposable();
        this._cancelable.Disposable = (IDisposable) assignmentDisposable;
        assignmentDisposable.Disposable = this._parent._scheduler.Schedule<ulong>(state, this._parent._dueTime, new Func<IScheduler, ulong, IDisposable>(this.Propagate));
      }

      private IDisposable Propagate(IScheduler self, ulong currentid)
      {
        lock (this._gate)
        {
          if (this._hasValue && (long) this._id == (long) currentid)
            this._observer.OnNext(this._value);
          this._hasValue = false;
        }
        return Disposable.Empty;
      }

      public void OnError(Exception error)
      {
        this._cancelable.Dispose();
        lock (this._gate)
        {
          this._observer.OnError(error);
          this.Dispose();
          this._hasValue = false;
          ++this._id;
        }
      }

      public void OnCompleted()
      {
        this._cancelable.Dispose();
        lock (this._gate)
        {
          if (this._hasValue)
            this._observer.OnNext(this._value);
          this._observer.OnCompleted();
          this.Dispose();
          this._hasValue = false;
          ++this._id;
        }
      }
    }
  }
}
