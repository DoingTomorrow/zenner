﻿// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Zip`3
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Zip<TFirst, TSecond, TResult> : Producer<TResult>
  {
    private readonly IObservable<TFirst> _first;
    private readonly IObservable<TSecond> _second;
    private readonly IEnumerable<TSecond> _secondE;
    private readonly Func<TFirst, TSecond, TResult> _resultSelector;

    public Zip(
      IObservable<TFirst> first,
      IObservable<TSecond> second,
      Func<TFirst, TSecond, TResult> resultSelector)
    {
      this._first = first;
      this._second = second;
      this._resultSelector = resultSelector;
    }

    public Zip(
      IObservable<TFirst> first,
      IEnumerable<TSecond> second,
      Func<TFirst, TSecond, TResult> resultSelector)
    {
      this._first = first;
      this._secondE = second;
      this._resultSelector = resultSelector;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._second != null)
      {
        Zip<TFirst, TSecond, TResult>._ obj = new Zip<TFirst, TSecond, TResult>._(this, observer, cancel);
        setSink((IDisposable) obj);
        return obj.Run();
      }
      Zip<TFirst, TSecond, TResult>.ZipImpl zipImpl = new Zip<TFirst, TSecond, TResult>.ZipImpl(this, observer, cancel);
      setSink((IDisposable) zipImpl);
      return zipImpl.Run();
    }

    private class _ : Sink<TResult>
    {
      private readonly Zip<TFirst, TSecond, TResult> _parent;
      private object _gate;

      public _(
        Zip<TFirst, TSecond, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        SingleAssignmentDisposable self1 = new SingleAssignmentDisposable();
        SingleAssignmentDisposable self2 = new SingleAssignmentDisposable();
        Zip<TFirst, TSecond, TResult>._.F f = new Zip<TFirst, TSecond, TResult>._.F(this, (IDisposable) self1);
        Zip<TFirst, TSecond, TResult>._.S s = new Zip<TFirst, TSecond, TResult>._.S(this, (IDisposable) self2);
        f.Other = s;
        s.Other = f;
        self1.Disposable = this._parent._first.SubscribeSafe<TFirst>((IObserver<TFirst>) f);
        self2.Disposable = this._parent._second.SubscribeSafe<TSecond>((IObserver<TSecond>) s);
        return (IDisposable) StableCompositeDisposable.Create((IDisposable) self1, (IDisposable) self2, (IDisposable) f, (IDisposable) s);
      }

      private class F : IObserver<TFirst>, IDisposable
      {
        private readonly Zip<TFirst, TSecond, TResult>._ _parent;
        private readonly IDisposable _self;
        private Zip<TFirst, TSecond, TResult>._.S _other;
        private Queue<TFirst> _queue;

        public F(Zip<TFirst, TSecond, TResult>._ parent, IDisposable self)
        {
          this._parent = parent;
          this._self = self;
          this._queue = new Queue<TFirst>();
        }

        public Zip<TFirst, TSecond, TResult>._.S Other
        {
          set => this._other = value;
        }

        public Queue<TFirst> Queue => this._queue;

        public bool Done { get; private set; }

        public void OnNext(TFirst value)
        {
          lock (this._parent._gate)
          {
            if (this._other.Queue.Count > 0)
            {
              TSecond second = this._other.Queue.Dequeue();
              TResult result1 = default (TResult);
              TResult result2;
              try
              {
                result2 = this._parent._parent._resultSelector(value, second);
              }
              catch (Exception ex)
              {
                this._parent._observer.OnError(ex);
                this._parent.Dispose();
                return;
              }
              this._parent._observer.OnNext(result2);
            }
            else if (this._other.Done)
            {
              this._parent._observer.OnCompleted();
              this._parent.Dispose();
            }
            else
              this._queue.Enqueue(value);
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

        public void Dispose() => this._queue.Clear();
      }

      private class S : IObserver<TSecond>, IDisposable
      {
        private readonly Zip<TFirst, TSecond, TResult>._ _parent;
        private readonly IDisposable _self;
        private Zip<TFirst, TSecond, TResult>._.F _other;
        private Queue<TSecond> _queue;

        public S(Zip<TFirst, TSecond, TResult>._ parent, IDisposable self)
        {
          this._parent = parent;
          this._self = self;
          this._queue = new Queue<TSecond>();
        }

        public Zip<TFirst, TSecond, TResult>._.F Other
        {
          set => this._other = value;
        }

        public Queue<TSecond> Queue => this._queue;

        public bool Done { get; private set; }

        public void OnNext(TSecond value)
        {
          lock (this._parent._gate)
          {
            if (this._other.Queue.Count > 0)
            {
              TFirst first = this._other.Queue.Dequeue();
              TResult result1 = default (TResult);
              TResult result2;
              try
              {
                result2 = this._parent._parent._resultSelector(first, value);
              }
              catch (Exception ex)
              {
                this._parent._observer.OnError(ex);
                this._parent.Dispose();
                return;
              }
              this._parent._observer.OnNext(result2);
            }
            else if (this._other.Done)
            {
              this._parent._observer.OnCompleted();
              this._parent.Dispose();
            }
            else
              this._queue.Enqueue(value);
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

        public void Dispose() => this._queue.Clear();
      }
    }

    private class ZipImpl : Sink<TResult>, IObserver<TFirst>
    {
      private readonly Zip<TFirst, TSecond, TResult> _parent;
      private IEnumerator<TSecond> _rightEnumerator;

      public ZipImpl(
        Zip<TFirst, TSecond, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        try
        {
          this._rightEnumerator = this._parent._secondE.GetEnumerator();
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return Disposable.Empty;
        }
        return (IDisposable) StableCompositeDisposable.Create(this._parent._first.SubscribeSafe<TFirst>((IObserver<TFirst>) this), (IDisposable) this._rightEnumerator);
      }

      public void OnNext(TFirst value)
      {
        bool flag;
        try
        {
          flag = this._rightEnumerator.MoveNext();
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        if (flag)
        {
          TSecond second = default (TSecond);
          TSecond current;
          try
          {
            current = this._rightEnumerator.Current;
          }
          catch (Exception ex)
          {
            this._observer.OnError(ex);
            this.Dispose();
            return;
          }
          TResult result;
          try
          {
            result = this._parent._resultSelector(value, current);
          }
          catch (Exception ex)
          {
            this._observer.OnError(ex);
            this.Dispose();
            return;
          }
          this._observer.OnNext(result);
        }
        else
        {
          this._observer.OnCompleted();
          this.Dispose();
        }
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
