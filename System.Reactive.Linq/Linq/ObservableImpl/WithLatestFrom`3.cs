// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.WithLatestFrom`3
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class WithLatestFrom<TFirst, TSecond, TResult> : Producer<TResult>
  {
    private readonly IObservable<TFirst> _first;
    private readonly IObservable<TSecond> _second;
    private readonly Func<TFirst, TSecond, TResult> _resultSelector;

    public WithLatestFrom(
      IObservable<TFirst> first,
      IObservable<TSecond> second,
      Func<TFirst, TSecond, TResult> resultSelector)
    {
      this._first = first;
      this._second = second;
      this._resultSelector = resultSelector;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      WithLatestFrom<TFirst, TSecond, TResult>._ obj = new WithLatestFrom<TFirst, TSecond, TResult>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TResult>
    {
      private readonly WithLatestFrom<TFirst, TSecond, TResult> _parent;
      private object _gate;
      private volatile bool _hasLatest;
      private TSecond _latest;

      public _(
        WithLatestFrom<TFirst, TSecond, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        SingleAssignmentDisposable self = new SingleAssignmentDisposable();
        WithLatestFrom<TFirst, TSecond, TResult>._.F f = new WithLatestFrom<TFirst, TSecond, TResult>._.F(this);
        WithLatestFrom<TFirst, TSecond, TResult>._.S s = new WithLatestFrom<TFirst, TSecond, TResult>._.S(this, (IDisposable) self);
        IDisposable disposable1 = this._parent._first.SubscribeSafe<TFirst>((IObserver<TFirst>) f);
        self.Disposable = this._parent._second.SubscribeSafe<TSecond>((IObserver<TSecond>) s);
        SingleAssignmentDisposable disposable2 = self;
        return (IDisposable) StableCompositeDisposable.Create(disposable1, (IDisposable) disposable2);
      }

      private class F : IObserver<TFirst>
      {
        private readonly WithLatestFrom<TFirst, TSecond, TResult>._ _parent;

        public F(WithLatestFrom<TFirst, TSecond, TResult>._ parent) => this._parent = parent;

        public void OnCompleted()
        {
          lock (this._parent._gate)
          {
            this._parent._observer.OnCompleted();
            this._parent.Dispose();
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

        public void OnNext(TFirst value)
        {
          if (!this._parent._hasLatest)
            return;
          TResult result1 = default (TResult);
          TResult result2;
          try
          {
            result2 = this._parent._parent._resultSelector(value, this._parent._latest);
          }
          catch (Exception ex)
          {
            lock (this._parent._gate)
            {
              this._parent._observer.OnError(ex);
              this._parent.Dispose();
              return;
            }
          }
          lock (this._parent._gate)
            this._parent._observer.OnNext(result2);
        }
      }

      private class S : IObserver<TSecond>
      {
        private readonly WithLatestFrom<TFirst, TSecond, TResult>._ _parent;
        private readonly IDisposable _self;

        public S(WithLatestFrom<TFirst, TSecond, TResult>._ parent, IDisposable self)
        {
          this._parent = parent;
          this._self = self;
        }

        public void OnCompleted() => this._self.Dispose();

        public void OnError(Exception error)
        {
          lock (this._parent._gate)
          {
            this._parent._observer.OnError(error);
            this._parent.Dispose();
          }
        }

        public void OnNext(TSecond value)
        {
          this._parent._latest = value;
          this._parent._hasLatest = true;
        }
      }
    }
  }
}
