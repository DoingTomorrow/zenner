// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Throttle`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Throttle<TSource, TThrottle> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, IObservable<TThrottle>> _throttleSelector;

    public Throttle(
      IObservable<TSource> source,
      Func<TSource, IObservable<TThrottle>> throttleSelector)
    {
      this._source = source;
      this._throttleSelector = throttleSelector;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Throttle<TSource, TThrottle>._ obj = new Throttle<TSource, TThrottle>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Throttle<TSource, TThrottle> _parent;
      private object _gate;
      private TSource _value;
      private bool _hasValue;
      private SerialDisposable _cancelable;
      private ulong _id;

      public _(
        Throttle<TSource, TThrottle> parent,
        IObserver<TSource> observer,
        IDisposable cancel)
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
        IObservable<TThrottle> source;
        try
        {
          source = this._parent._throttleSelector(value);
        }
        catch (Exception ex)
        {
          lock (this._gate)
          {
            this._observer.OnError(ex);
            this.Dispose();
            return;
          }
        }
        ulong id;
        lock (this._gate)
        {
          this._hasValue = true;
          this._value = value;
          ++this._id;
          id = this._id;
        }
        SingleAssignmentDisposable self = new SingleAssignmentDisposable();
        this._cancelable.Disposable = (IDisposable) self;
        self.Disposable = source.SubscribeSafe<TThrottle>((IObserver<TThrottle>) new Throttle<TSource, TThrottle>._.Delta(this, value, id, (IDisposable) self));
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

      private class Delta : IObserver<TThrottle>
      {
        private readonly Throttle<TSource, TThrottle>._ _parent;
        private readonly TSource _value;
        private readonly ulong _currentid;
        private readonly IDisposable _self;

        public Delta(
          Throttle<TSource, TThrottle>._ parent,
          TSource value,
          ulong currentid,
          IDisposable self)
        {
          this._parent = parent;
          this._value = value;
          this._currentid = currentid;
          this._self = self;
        }

        public void OnNext(TThrottle value)
        {
          lock (this._parent._gate)
          {
            if (this._parent._hasValue && (long) this._parent._id == (long) this._currentid)
              this._parent._observer.OnNext(this._value);
            this._parent._hasValue = false;
            this._self.Dispose();
          }
        }

        public void OnError(Exception error)
        {
          lock (this._parent._gate)
          {
            this._parent._observer.OnError(error);
            this._parent.Dispose();
          }
        }

        public void OnCompleted()
        {
          lock (this._parent._gate)
          {
            if (this._parent._hasValue && (long) this._parent._id == (long) this._currentid)
              this._parent._observer.OnNext(this._value);
            this._parent._hasValue = false;
            this._self.Dispose();
          }
        }
      }
    }
  }
}
