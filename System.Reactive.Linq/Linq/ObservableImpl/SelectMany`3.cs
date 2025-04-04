// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.SelectMany`3
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class SelectMany<TSource, TCollection, TResult> : Producer<TResult>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, IObservable<TCollection>> _collectionSelector;
    private readonly Func<TSource, int, IObservable<TCollection>> _collectionSelectorI;
    private readonly Func<TSource, IEnumerable<TCollection>> _collectionSelectorE;
    private readonly Func<TSource, int, IEnumerable<TCollection>> _collectionSelectorEI;
    private readonly Func<TSource, TCollection, TResult> _resultSelector;
    private readonly Func<TSource, int, TCollection, int, TResult> _resultSelectorI;
    private readonly Func<TSource, CancellationToken, Task<TCollection>> _collectionSelectorT;
    private readonly Func<TSource, int, CancellationToken, Task<TCollection>> _collectionSelectorTI;
    private readonly Func<TSource, int, TCollection, TResult> _resultSelectorTI;

    public SelectMany(
      IObservable<TSource> source,
      Func<TSource, IObservable<TCollection>> collectionSelector,
      Func<TSource, TCollection, TResult> resultSelector)
    {
      this._source = source;
      this._collectionSelector = collectionSelector;
      this._resultSelector = resultSelector;
    }

    public SelectMany(
      IObservable<TSource> source,
      Func<TSource, int, IObservable<TCollection>> collectionSelector,
      Func<TSource, int, TCollection, int, TResult> resultSelector)
    {
      this._source = source;
      this._collectionSelectorI = collectionSelector;
      this._resultSelectorI = resultSelector;
    }

    public SelectMany(
      IObservable<TSource> source,
      Func<TSource, IEnumerable<TCollection>> collectionSelector,
      Func<TSource, TCollection, TResult> resultSelector)
    {
      this._source = source;
      this._collectionSelectorE = collectionSelector;
      this._resultSelector = resultSelector;
    }

    public SelectMany(
      IObservable<TSource> source,
      Func<TSource, int, IEnumerable<TCollection>> collectionSelector,
      Func<TSource, int, TCollection, int, TResult> resultSelector)
    {
      this._source = source;
      this._collectionSelectorEI = collectionSelector;
      this._resultSelectorI = resultSelector;
    }

    public SelectMany(
      IObservable<TSource> source,
      Func<TSource, CancellationToken, Task<TCollection>> collectionSelector,
      Func<TSource, TCollection, TResult> resultSelector)
    {
      this._source = source;
      this._collectionSelectorT = collectionSelector;
      this._resultSelector = resultSelector;
    }

    public SelectMany(
      IObservable<TSource> source,
      Func<TSource, int, CancellationToken, Task<TCollection>> collectionSelector,
      Func<TSource, int, TCollection, TResult> resultSelector)
    {
      this._source = source;
      this._collectionSelectorTI = collectionSelector;
      this._resultSelectorTI = resultSelector;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._collectionSelector != null)
      {
        SelectMany<TSource, TCollection, TResult>._ obj = new SelectMany<TSource, TCollection, TResult>._(this, observer, cancel);
        setSink((IDisposable) obj);
        return obj.Run();
      }
      if (this._collectionSelectorI != null)
      {
        SelectMany<TSource, TCollection, TResult>.IndexSelectorImpl indexSelectorImpl = new SelectMany<TSource, TCollection, TResult>.IndexSelectorImpl(this, observer, cancel);
        setSink((IDisposable) indexSelectorImpl);
        return indexSelectorImpl.Run();
      }
      if (this._collectionSelectorT != null)
      {
        SelectMany<TSource, TCollection, TResult>.SelectManyImpl selectManyImpl = new SelectMany<TSource, TCollection, TResult>.SelectManyImpl(this, observer, cancel);
        setSink((IDisposable) selectManyImpl);
        return selectManyImpl.Run();
      }
      if (this._collectionSelectorTI != null)
      {
        SelectMany<TSource, TCollection, TResult>.Sigma sigma = new SelectMany<TSource, TCollection, TResult>.Sigma(this, observer, cancel);
        setSink((IDisposable) sigma);
        return sigma.Run();
      }
      if (this._collectionSelectorE != null)
      {
        SelectMany<TSource, TCollection, TResult>.NoSelectorImpl noSelectorImpl = new SelectMany<TSource, TCollection, TResult>.NoSelectorImpl(this, observer, cancel);
        setSink((IDisposable) noSelectorImpl);
        return this._source.SubscribeSafe<TSource>((IObserver<TSource>) noSelectorImpl);
      }
      SelectMany<TSource, TCollection, TResult>.Omega omega = new SelectMany<TSource, TCollection, TResult>.Omega(this, observer, cancel);
      setSink((IDisposable) omega);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) omega);
    }

    private class _ : Sink<TResult>, IObserver<TSource>
    {
      private readonly SelectMany<TSource, TCollection, TResult> _parent;
      private object _gate;
      private bool _isStopped;
      private CompositeDisposable _group;
      private SingleAssignmentDisposable _sourceSubscription;

      public _(
        SelectMany<TSource, TCollection, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        this._isStopped = false;
        this._group = new CompositeDisposable();
        this._sourceSubscription = new SingleAssignmentDisposable();
        this._group.Add((IDisposable) this._sourceSubscription);
        this._sourceSubscription.Disposable = this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this);
        return (IDisposable) this._group;
      }

      public void OnNext(TSource value)
      {
        IObservable<TCollection> source;
        try
        {
          source = this._parent._collectionSelector(value);
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
        SingleAssignmentDisposable self = new SingleAssignmentDisposable();
        this._group.Add((IDisposable) self);
        self.Disposable = source.SubscribeSafe<TCollection>((IObserver<TCollection>) new SelectMany<TSource, TCollection, TResult>._.Iter(this, value, (IDisposable) self));
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
        this._isStopped = true;
        if (this._group.Count == 1)
        {
          lock (this._gate)
          {
            this._observer.OnCompleted();
            this.Dispose();
          }
        }
        else
          this._sourceSubscription.Dispose();
      }

      private class Iter : IObserver<TCollection>
      {
        private readonly SelectMany<TSource, TCollection, TResult>._ _parent;
        private readonly TSource _value;
        private readonly IDisposable _self;

        public Iter(
          SelectMany<TSource, TCollection, TResult>._ parent,
          TSource value,
          IDisposable self)
        {
          this._parent = parent;
          this._value = value;
          this._self = self;
        }

        public void OnNext(TCollection value)
        {
          TResult result1 = default (TResult);
          TResult result2;
          try
          {
            result2 = this._parent._parent._resultSelector(this._value, value);
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
          this._parent._group.Remove(this._self);
          if (!this._parent._isStopped || this._parent._group.Count != 1)
            return;
          lock (this._parent._gate)
          {
            this._parent._observer.OnCompleted();
            this._parent.Dispose();
          }
        }
      }
    }

    private class IndexSelectorImpl : Sink<TResult>, IObserver<TSource>
    {
      private readonly SelectMany<TSource, TCollection, TResult> _parent;
      private object _gate;
      private bool _isStopped;
      private CompositeDisposable _group;
      private SingleAssignmentDisposable _sourceSubscription;
      private int _index;

      public IndexSelectorImpl(
        SelectMany<TSource, TCollection, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        this._isStopped = false;
        this._group = new CompositeDisposable();
        this._sourceSubscription = new SingleAssignmentDisposable();
        this._group.Add((IDisposable) this._sourceSubscription);
        this._sourceSubscription.Disposable = this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this);
        return (IDisposable) this._group;
      }

      public void OnNext(TSource value)
      {
        int index = checked (this._index++);
        IObservable<TCollection> source;
        try
        {
          source = this._parent._collectionSelectorI(value, index);
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
        SingleAssignmentDisposable self = new SingleAssignmentDisposable();
        this._group.Add((IDisposable) self);
        self.Disposable = source.SubscribeSafe<TCollection>((IObserver<TCollection>) new SelectMany<TSource, TCollection, TResult>.IndexSelectorImpl.Iter(this, value, index, (IDisposable) self));
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
        this._isStopped = true;
        if (this._group.Count == 1)
        {
          lock (this._gate)
          {
            this._observer.OnCompleted();
            this.Dispose();
          }
        }
        else
          this._sourceSubscription.Dispose();
      }

      private class Iter : IObserver<TCollection>
      {
        private readonly SelectMany<TSource, TCollection, TResult>.IndexSelectorImpl _parent;
        private readonly TSource _value;
        private readonly int _valueIndex;
        private readonly IDisposable _self;
        private int _index;

        public Iter(
          SelectMany<TSource, TCollection, TResult>.IndexSelectorImpl parent,
          TSource value,
          int index,
          IDisposable self)
        {
          this._parent = parent;
          this._value = value;
          this._valueIndex = index;
          this._self = self;
        }

        public void OnNext(TCollection value)
        {
          TResult result1 = default (TResult);
          TResult result2;
          try
          {
            result2 = this._parent._parent._resultSelectorI(this._value, this._valueIndex, value, checked (this._index++));
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
          this._parent._group.Remove(this._self);
          if (!this._parent._isStopped || this._parent._group.Count != 1)
            return;
          lock (this._parent._gate)
          {
            this._parent._observer.OnCompleted();
            this._parent.Dispose();
          }
        }
      }
    }

    private class NoSelectorImpl : Sink<TResult>, IObserver<TSource>
    {
      private readonly SelectMany<TSource, TCollection, TResult> _parent;

      public NoSelectorImpl(
        SelectMany<TSource, TCollection, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public void OnNext(TSource value)
      {
        IEnumerable<TCollection> collections;
        try
        {
          collections = this._parent._collectionSelectorE(value);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        IEnumerator<TCollection> enumerator;
        try
        {
          enumerator = collections.GetEnumerator();
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        try
        {
          bool flag = true;
          while (flag)
          {
            TResult result = default (TResult);
            try
            {
              flag = enumerator.MoveNext();
              if (flag)
                result = this._parent._resultSelector(value, enumerator.Current);
            }
            catch (Exception ex)
            {
              this._observer.OnError(ex);
              this.Dispose();
              break;
            }
            if (flag)
              this._observer.OnNext(result);
          }
        }
        finally
        {
          enumerator?.Dispose();
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

    private class Omega : Sink<TResult>, IObserver<TSource>
    {
      private readonly SelectMany<TSource, TCollection, TResult> _parent;
      private int _index;

      public Omega(
        SelectMany<TSource, TCollection, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public void OnNext(TSource value)
      {
        int num1 = checked (this._index++);
        IEnumerable<TCollection> collections;
        try
        {
          collections = this._parent._collectionSelectorEI(value, num1);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        IEnumerator<TCollection> enumerator;
        try
        {
          enumerator = collections.GetEnumerator();
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        try
        {
          int num2 = 0;
          bool flag = true;
          while (flag)
          {
            TResult result = default (TResult);
            try
            {
              flag = enumerator.MoveNext();
              if (flag)
                result = this._parent._resultSelectorI(value, num1, enumerator.Current, checked (num2++));
            }
            catch (Exception ex)
            {
              this._observer.OnError(ex);
              this.Dispose();
              break;
            }
            if (flag)
              this._observer.OnNext(result);
          }
        }
        finally
        {
          enumerator?.Dispose();
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

    private class SelectManyImpl : Sink<TResult>, IObserver<TSource>
    {
      private readonly SelectMany<TSource, TCollection, TResult> _parent;
      private object _gate;
      private CancellationDisposable _cancel;
      private volatile int _count;

      public SelectManyImpl(
        SelectMany<TSource, TCollection, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        this._cancel = new CancellationDisposable();
        this._count = 1;
        return (IDisposable) StableCompositeDisposable.Create(this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this), (IDisposable) this._cancel);
      }

      public void OnNext(TSource value)
      {
        Task<TCollection> task;
        try
        {
          Interlocked.Increment(ref this._count);
          task = this._parent._collectionSelectorT(value, this._cancel.Token);
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
        if (task.IsCompleted)
          this.OnCompletedTask(value, task);
        else
          this.AttachContinuation(value, task);
      }

      private void AttachContinuation(TSource value, Task<TCollection> task)
      {
        task.ContinueWith((Action<Task<TCollection>>) (t => this.OnCompletedTask(value, t)));
      }

      private void OnCompletedTask(TSource value, Task<TCollection> task)
      {
        switch (task.Status)
        {
          case TaskStatus.RanToCompletion:
            TResult result1 = default (TResult);
            TResult result2;
            try
            {
              result2 = this._parent._resultSelector(value, task.Result);
            }
            catch (Exception ex)
            {
              lock (this._gate)
              {
                this._observer.OnError(ex);
                this.Dispose();
                break;
              }
            }
            lock (this._gate)
              this._observer.OnNext(result2);
            this.OnCompleted();
            break;
          case TaskStatus.Canceled:
            if (this._cancel.IsDisposed)
              break;
            lock (this._gate)
            {
              this._observer.OnError((Exception) new TaskCanceledException((Task) task));
              this.Dispose();
              break;
            }
          case TaskStatus.Faulted:
            lock (this._gate)
            {
              this._observer.OnError(task.Exception.InnerException);
              this.Dispose();
              break;
            }
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
        if (Interlocked.Decrement(ref this._count) != 0)
          return;
        lock (this._gate)
        {
          this._observer.OnCompleted();
          this.Dispose();
        }
      }
    }

    private class Sigma : Sink<TResult>, IObserver<TSource>
    {
      private readonly SelectMany<TSource, TCollection, TResult> _parent;
      private object _gate;
      private CancellationDisposable _cancel;
      private volatile int _count;
      private int _index;

      public Sigma(
        SelectMany<TSource, TCollection, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        this._cancel = new CancellationDisposable();
        this._count = 1;
        return (IDisposable) StableCompositeDisposable.Create(this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this), (IDisposable) this._cancel);
      }

      public void OnNext(TSource value)
      {
        int index = checked (this._index++);
        Task<TCollection> task;
        try
        {
          Interlocked.Increment(ref this._count);
          task = this._parent._collectionSelectorTI(value, index, this._cancel.Token);
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
        if (task.IsCompleted)
          this.OnCompletedTask(value, index, task);
        else
          this.AttachContinuation(value, index, task);
      }

      private void AttachContinuation(TSource value, int index, Task<TCollection> task)
      {
        task.ContinueWith((Action<Task<TCollection>>) (t => this.OnCompletedTask(value, index, t)));
      }

      private void OnCompletedTask(TSource value, int index, Task<TCollection> task)
      {
        switch (task.Status)
        {
          case TaskStatus.RanToCompletion:
            TResult result1 = default (TResult);
            TResult result2;
            try
            {
              result2 = this._parent._resultSelectorTI(value, index, task.Result);
            }
            catch (Exception ex)
            {
              lock (this._gate)
              {
                this._observer.OnError(ex);
                this.Dispose();
                break;
              }
            }
            lock (this._gate)
              this._observer.OnNext(result2);
            this.OnCompleted();
            break;
          case TaskStatus.Canceled:
            if (this._cancel.IsDisposed)
              break;
            lock (this._gate)
            {
              this._observer.OnError((Exception) new TaskCanceledException((Task) task));
              this.Dispose();
              break;
            }
          case TaskStatus.Faulted:
            lock (this._gate)
            {
              this._observer.OnError(task.Exception.InnerException);
              this.Dispose();
              break;
            }
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
        if (Interlocked.Decrement(ref this._count) != 0)
          return;
        lock (this._gate)
        {
          this._observer.OnCompleted();
          this.Dispose();
        }
      }
    }
  }
}
