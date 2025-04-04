// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.CombineLatest`3
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class CombineLatest<TFirst, TSecond, TResult> : Producer<TResult>
  {
    private readonly IObservable<TFirst> _first;
    private readonly IObservable<TSecond> _second;
    private readonly Func<TFirst, TSecond, TResult> _resultSelector;

    public CombineLatest(
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
      CombineLatest<TFirst, TSecond, TResult>._ obj = new CombineLatest<TFirst, TSecond, TResult>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TResult>
    {
      private readonly CombineLatest<TFirst, TSecond, TResult> _parent;
      private object _gate;

      public _(
        CombineLatest<TFirst, TSecond, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        SingleAssignmentDisposable assignmentDisposable1 = new SingleAssignmentDisposable();
        SingleAssignmentDisposable assignmentDisposable2 = new SingleAssignmentDisposable();
        CombineLatest<TFirst, TSecond, TResult>._.F f = new CombineLatest<TFirst, TSecond, TResult>._.F(this, (IDisposable) assignmentDisposable1);
        CombineLatest<TFirst, TSecond, TResult>._.S s = new CombineLatest<TFirst, TSecond, TResult>._.S(this, (IDisposable) assignmentDisposable2);
        f.Other = s;
        s.Other = f;
        assignmentDisposable1.Disposable = this._parent._first.SubscribeSafe<TFirst>((IObserver<TFirst>) f);
        assignmentDisposable2.Disposable = this._parent._second.SubscribeSafe<TSecond>((IObserver<TSecond>) s);
        return (IDisposable) StableCompositeDisposable.Create((IDisposable) assignmentDisposable1, (IDisposable) assignmentDisposable2);
      }

      private class F : IObserver<TFirst>
      {
        private readonly CombineLatest<TFirst, TSecond, TResult>._ _parent;
        private readonly IDisposable _self;
        private CombineLatest<TFirst, TSecond, TResult>._.S _other;

        public F(CombineLatest<TFirst, TSecond, TResult>._ parent, IDisposable self)
        {
          this._parent = parent;
          this._self = self;
        }

        public CombineLatest<TFirst, TSecond, TResult>._.S Other
        {
          set => this._other = value;
        }

        public bool HasValue { get; private set; }

        public TFirst Value { get; private set; }

        public bool Done { get; private set; }

        public void OnNext(TFirst value)
        {
          lock (this._parent._gate)
          {
            this.HasValue = true;
            this.Value = value;
            if (this._other.HasValue)
            {
              TResult result1 = default (TResult);
              TResult result2;
              try
              {
                result2 = this._parent._parent._resultSelector(value, this._other.Value);
              }
              catch (Exception ex)
              {
                this._parent._observer.OnError(ex);
                this._parent.Dispose();
                return;
              }
              this._parent._observer.OnNext(result2);
            }
            else
            {
              if (!this._other.Done)
                return;
              this._parent._observer.OnCompleted();
              this._parent.Dispose();
            }
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
            this.Done = true;
            if (this._other.Done)
            {
              this._parent._observer.OnCompleted();
              this._parent.Dispose();
            }
            else
              this._self.Dispose();
          }
        }
      }

      private class S : IObserver<TSecond>
      {
        private readonly CombineLatest<TFirst, TSecond, TResult>._ _parent;
        private readonly IDisposable _self;
        private CombineLatest<TFirst, TSecond, TResult>._.F _other;

        public S(CombineLatest<TFirst, TSecond, TResult>._ parent, IDisposable self)
        {
          this._parent = parent;
          this._self = self;
        }

        public CombineLatest<TFirst, TSecond, TResult>._.F Other
        {
          set => this._other = value;
        }

        public bool HasValue { get; private set; }

        public TSecond Value { get; private set; }

        public bool Done { get; private set; }

        public void OnNext(TSecond value)
        {
          lock (this._parent._gate)
          {
            this.HasValue = true;
            this.Value = value;
            if (this._other.HasValue)
            {
              TResult result1 = default (TResult);
              TResult result2;
              try
              {
                result2 = this._parent._parent._resultSelector(this._other.Value, value);
              }
              catch (Exception ex)
              {
                this._parent._observer.OnError(ex);
                this._parent.Dispose();
                return;
              }
              this._parent._observer.OnNext(result2);
            }
            else
            {
              if (!this._other.Done)
                return;
              this._parent._observer.OnCompleted();
              this._parent.Dispose();
            }
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
            this.Done = true;
            if (this._other.Done)
            {
              this._parent._observer.OnCompleted();
              this._parent.Dispose();
            }
            else
              this._self.Dispose();
          }
        }
      }
    }
  }
}
