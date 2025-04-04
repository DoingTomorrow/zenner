// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Join`5
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult> : Producer<TResult>
  {
    private readonly IObservable<TLeft> _left;
    private readonly IObservable<TRight> _right;
    private readonly Func<TLeft, IObservable<TLeftDuration>> _leftDurationSelector;
    private readonly Func<TRight, IObservable<TRightDuration>> _rightDurationSelector;
    private readonly Func<TLeft, TRight, TResult> _resultSelector;

    public Join(
      IObservable<TLeft> left,
      IObservable<TRight> right,
      Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector,
      Func<TRight, IObservable<TRightDuration>> rightDurationSelector,
      Func<TLeft, TRight, TResult> resultSelector)
    {
      this._left = left;
      this._right = right;
      this._leftDurationSelector = leftDurationSelector;
      this._rightDurationSelector = rightDurationSelector;
      this._resultSelector = resultSelector;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._ obj = new Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TResult>
    {
      private readonly Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult> _parent;
      private object _gate;
      private CompositeDisposable _group;
      private bool _leftDone;
      private int _leftID;
      private SortedDictionary<int, TLeft> _leftMap;
      private bool _rightDone;
      private int _rightID;
      private SortedDictionary<int, TRight> _rightMap;

      public _(
        Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        this._group = new CompositeDisposable();
        SingleAssignmentDisposable self1 = new SingleAssignmentDisposable();
        this._group.Add((IDisposable) self1);
        this._leftDone = false;
        this._leftID = 0;
        this._leftMap = new SortedDictionary<int, TLeft>();
        SingleAssignmentDisposable self2 = new SingleAssignmentDisposable();
        this._group.Add((IDisposable) self2);
        this._rightDone = false;
        this._rightID = 0;
        this._rightMap = new SortedDictionary<int, TRight>();
        self1.Disposable = this._parent._left.SubscribeSafe<TLeft>((IObserver<TLeft>) new Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._.LeftObserver(this, (IDisposable) self1));
        self2.Disposable = this._parent._right.SubscribeSafe<TRight>((IObserver<TRight>) new Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._.RightObserver(this, (IDisposable) self2));
        return (IDisposable) this._group;
      }

      private class LeftObserver : IObserver<TLeft>
      {
        private readonly Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._ _parent;
        private readonly IDisposable _self;

        public LeftObserver(
          Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._ parent,
          IDisposable self)
        {
          this._parent = parent;
          this._self = self;
        }

        private void Expire(int id, IDisposable resource)
        {
          lock (this._parent._gate)
          {
            if (this._parent._leftMap.Remove(id))
            {
              if (this._parent._leftMap.Count == 0)
              {
                if (this._parent._leftDone)
                {
                  this._parent._observer.OnCompleted();
                  this._parent.Dispose();
                }
              }
            }
          }
          this._parent._group.Remove(resource);
        }

        public void OnNext(TLeft value)
        {
          int num1 = 0;
          int num2 = 0;
          lock (this._parent._gate)
          {
            num1 = this._parent._leftID++;
            num2 = this._parent._rightID;
            this._parent._leftMap.Add(num1, value);
          }
          SingleAssignmentDisposable self = new SingleAssignmentDisposable();
          this._parent._group.Add((IDisposable) self);
          IObservable<TLeftDuration> source;
          try
          {
            source = this._parent._parent._leftDurationSelector(value);
          }
          catch (Exception ex)
          {
            this._parent._observer.OnError(ex);
            this._parent.Dispose();
            return;
          }
          self.Disposable = source.SubscribeSafe<TLeftDuration>((IObserver<TLeftDuration>) new Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._.LeftObserver.Delta(this, num1, (IDisposable) self));
          lock (this._parent._gate)
          {
            foreach (KeyValuePair<int, TRight> right in this._parent._rightMap)
            {
              if (right.Key < num2)
              {
                TResult result1 = default (TResult);
                TResult result2;
                try
                {
                  result2 = this._parent._parent._resultSelector(value, right.Value);
                }
                catch (Exception ex)
                {
                  this._parent._observer.OnError(ex);
                  this._parent.Dispose();
                  break;
                }
                this._parent._observer.OnNext(result2);
              }
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
            this._parent._leftDone = true;
            if (this._parent._rightDone || this._parent._leftMap.Count == 0)
            {
              this._parent._observer.OnCompleted();
              this._parent.Dispose();
            }
            else
              this._self.Dispose();
          }
        }

        private class Delta : IObserver<TLeftDuration>
        {
          private readonly Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._.LeftObserver _parent;
          private readonly int _id;
          private readonly IDisposable _self;

          public Delta(
            Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._.LeftObserver parent,
            int id,
            IDisposable self)
          {
            this._parent = parent;
            this._id = id;
            this._self = self;
          }

          public void OnNext(TLeftDuration value) => this._parent.Expire(this._id, this._self);

          public void OnError(Exception error) => this._parent.OnError(error);

          public void OnCompleted() => this._parent.Expire(this._id, this._self);
        }
      }

      private class RightObserver : IObserver<TRight>
      {
        private readonly Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._ _parent;
        private readonly IDisposable _self;

        public RightObserver(
          Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._ parent,
          IDisposable self)
        {
          this._parent = parent;
          this._self = self;
        }

        private void Expire(int id, IDisposable resource)
        {
          lock (this._parent._gate)
          {
            if (this._parent._rightMap.Remove(id))
            {
              if (this._parent._rightMap.Count == 0)
              {
                if (this._parent._rightDone)
                {
                  this._parent._observer.OnCompleted();
                  this._parent.Dispose();
                }
              }
            }
          }
          this._parent._group.Remove(resource);
        }

        public void OnNext(TRight value)
        {
          int num1 = 0;
          int num2 = 0;
          lock (this._parent._gate)
          {
            num1 = this._parent._rightID++;
            num2 = this._parent._leftID;
            this._parent._rightMap.Add(num1, value);
          }
          SingleAssignmentDisposable self = new SingleAssignmentDisposable();
          this._parent._group.Add((IDisposable) self);
          IObservable<TRightDuration> source;
          try
          {
            source = this._parent._parent._rightDurationSelector(value);
          }
          catch (Exception ex)
          {
            this._parent._observer.OnError(ex);
            this._parent.Dispose();
            return;
          }
          self.Disposable = source.SubscribeSafe<TRightDuration>((IObserver<TRightDuration>) new Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._.RightObserver.Delta(this, num1, (IDisposable) self));
          lock (this._parent._gate)
          {
            foreach (KeyValuePair<int, TLeft> left in this._parent._leftMap)
            {
              if (left.Key < num2)
              {
                TResult result1 = default (TResult);
                TResult result2;
                try
                {
                  result2 = this._parent._parent._resultSelector(left.Value, value);
                }
                catch (Exception ex)
                {
                  this._parent._observer.OnError(ex);
                  this._parent.Dispose();
                  break;
                }
                this._parent._observer.OnNext(result2);
              }
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
            this._parent._rightDone = true;
            if (this._parent._leftDone || this._parent._rightMap.Count == 0)
            {
              this._parent._observer.OnCompleted();
              this._parent.Dispose();
            }
            else
              this._self.Dispose();
          }
        }

        private class Delta : IObserver<TRightDuration>
        {
          private readonly Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._.RightObserver _parent;
          private readonly int _id;
          private readonly IDisposable _self;

          public Delta(
            Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._.RightObserver parent,
            int id,
            IDisposable self)
          {
            this._parent = parent;
            this._id = id;
            this._self = self;
          }

          public void OnNext(TRightDuration value) => this._parent.Expire(this._id, this._self);

          public void OnError(Exception error) => this._parent.OnError(error);

          public void OnCompleted() => this._parent.Expire(this._id, this._self);
        }
      }
    }
  }
}
