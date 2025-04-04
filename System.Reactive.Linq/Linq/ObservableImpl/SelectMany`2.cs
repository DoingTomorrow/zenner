// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.SelectMany`2
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
  internal class SelectMany<TSource, TResult> : Producer<TResult>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, IObservable<TResult>> _selector;
    private readonly Func<TSource, int, IObservable<TResult>> _selectorI;
    private readonly Func<Exception, IObservable<TResult>> _selectorOnError;
    private readonly Func<IObservable<TResult>> _selectorOnCompleted;
    private readonly Func<TSource, IEnumerable<TResult>> _selectorE;
    private readonly Func<TSource, int, IEnumerable<TResult>> _selectorEI;
    private readonly Func<TSource, CancellationToken, Task<TResult>> _selectorT;
    private readonly Func<TSource, int, CancellationToken, Task<TResult>> _selectorTI;

    public SelectMany(IObservable<TSource> source, Func<TSource, IObservable<TResult>> selector)
    {
      this._source = source;
      this._selector = selector;
    }

    public SelectMany(
      IObservable<TSource> source,
      Func<TSource, int, IObservable<TResult>> selector)
    {
      this._source = source;
      this._selectorI = selector;
    }

    public SelectMany(
      IObservable<TSource> source,
      Func<TSource, IObservable<TResult>> selector,
      Func<Exception, IObservable<TResult>> selectorOnError,
      Func<IObservable<TResult>> selectorOnCompleted)
    {
      this._source = source;
      this._selector = selector;
      this._selectorOnError = selectorOnError;
      this._selectorOnCompleted = selectorOnCompleted;
    }

    public SelectMany(
      IObservable<TSource> source,
      Func<TSource, int, IObservable<TResult>> selector,
      Func<Exception, IObservable<TResult>> selectorOnError,
      Func<IObservable<TResult>> selectorOnCompleted)
    {
      this._source = source;
      this._selectorI = selector;
      this._selectorOnError = selectorOnError;
      this._selectorOnCompleted = selectorOnCompleted;
    }

    public SelectMany(IObservable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
    {
      this._source = source;
      this._selectorE = selector;
    }

    public SelectMany(
      IObservable<TSource> source,
      Func<TSource, int, IEnumerable<TResult>> selector)
    {
      this._source = source;
      this._selectorEI = selector;
    }

    public SelectMany(
      IObservable<TSource> source,
      Func<TSource, CancellationToken, Task<TResult>> selector)
    {
      this._source = source;
      this._selectorT = selector;
    }

    public SelectMany(
      IObservable<TSource> source,
      Func<TSource, int, CancellationToken, Task<TResult>> selector)
    {
      this._source = source;
      this._selectorTI = selector;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._selector != null)
      {
        SelectMany<TSource, TResult>._ obj = new SelectMany<TSource, TResult>._(this, observer, cancel);
        setSink((IDisposable) obj);
        return obj.Run();
      }
      if (this._selectorI != null)
      {
        SelectMany<TSource, TResult>.IndexSelectorImpl indexSelectorImpl = new SelectMany<TSource, TResult>.IndexSelectorImpl(this, observer, cancel);
        setSink((IDisposable) indexSelectorImpl);
        return indexSelectorImpl.Run();
      }
      if (this._selectorT != null)
      {
        SelectMany<TSource, TResult>.SelectManyImpl selectManyImpl = new SelectMany<TSource, TResult>.SelectManyImpl(this, observer, cancel);
        setSink((IDisposable) selectManyImpl);
        return selectManyImpl.Run();
      }
      if (this._selectorTI != null)
      {
        SelectMany<TSource, TResult>.Sigma sigma = new SelectMany<TSource, TResult>.Sigma(this, observer, cancel);
        setSink((IDisposable) sigma);
        return sigma.Run();
      }
      if (this._selectorE != null)
      {
        SelectMany<TSource, TResult>.NoSelectorImpl noSelectorImpl = new SelectMany<TSource, TResult>.NoSelectorImpl(this, observer, cancel);
        setSink((IDisposable) noSelectorImpl);
        return this._source.SubscribeSafe<TSource>((IObserver<TSource>) noSelectorImpl);
      }
      SelectMany<TSource, TResult>.Omega omega = new SelectMany<TSource, TResult>.Omega(this, observer, cancel);
      setSink((IDisposable) omega);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) omega);
    }

    private class _ : Sink<TResult>, IObserver<TSource>
    {
      private readonly SelectMany<TSource, TResult> _parent;
      private object _gate;
      private bool _isStopped;
      private CompositeDisposable _group;
      private SingleAssignmentDisposable _sourceSubscription;

      public _(
        SelectMany<TSource, TResult> parent,
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
        IObservable<TResult> inner;
        try
        {
          inner = this._parent._selector(value);
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
        this.SubscribeInner(inner);
      }

      public void OnError(Exception error)
      {
        if (this._parent._selectorOnError != null)
        {
          IObservable<TResult> inner;
          try
          {
            inner = this._parent._selectorOnError(error);
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
          this.SubscribeInner(inner);
          this.Final();
        }
        else
        {
          lock (this._gate)
          {
            this._observer.OnError(error);
            this.Dispose();
          }
        }
      }

      public void OnCompleted()
      {
        if (this._parent._selectorOnCompleted != null)
        {
          IObservable<TResult> inner;
          try
          {
            inner = this._parent._selectorOnCompleted();
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
          this.SubscribeInner(inner);
        }
        this.Final();
      }

      private void Final()
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

      private void SubscribeInner(IObservable<TResult> inner)
      {
        SingleAssignmentDisposable self = new SingleAssignmentDisposable();
        this._group.Add((IDisposable) self);
        self.Disposable = inner.SubscribeSafe<TResult>((IObserver<TResult>) new SelectMany<TSource, TResult>._.Iter(this, (IDisposable) self));
      }

      private class Iter : IObserver<TResult>
      {
        private readonly SelectMany<TSource, TResult>._ _parent;
        private readonly IDisposable _self;

        public Iter(SelectMany<TSource, TResult>._ parent, IDisposable self)
        {
          this._parent = parent;
          this._self = self;
        }

        public void OnNext(TResult value)
        {
          lock (this._parent._gate)
            this._parent._observer.OnNext(value);
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
      private readonly SelectMany<TSource, TResult> _parent;
      private object _gate;
      private bool _isStopped;
      private CompositeDisposable _group;
      private SingleAssignmentDisposable _sourceSubscription;
      private int _index;

      public IndexSelectorImpl(
        SelectMany<TSource, TResult> parent,
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
        IObservable<TResult> inner;
        try
        {
          inner = this._parent._selectorI(value, checked (this._index++));
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
        this.SubscribeInner(inner);
      }

      public void OnError(Exception error)
      {
        if (this._parent._selectorOnError != null)
        {
          IObservable<TResult> inner;
          try
          {
            inner = this._parent._selectorOnError(error);
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
          this.SubscribeInner(inner);
          this.Final();
        }
        else
        {
          lock (this._gate)
          {
            this._observer.OnError(error);
            this.Dispose();
          }
        }
      }

      public void OnCompleted()
      {
        if (this._parent._selectorOnCompleted != null)
        {
          IObservable<TResult> inner;
          try
          {
            inner = this._parent._selectorOnCompleted();
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
          this.SubscribeInner(inner);
        }
        this.Final();
      }

      private void Final()
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

      private void SubscribeInner(IObservable<TResult> inner)
      {
        SingleAssignmentDisposable self = new SingleAssignmentDisposable();
        this._group.Add((IDisposable) self);
        self.Disposable = inner.SubscribeSafe<TResult>((IObserver<TResult>) new SelectMany<TSource, TResult>.IndexSelectorImpl.Iter(this, (IDisposable) self));
      }

      private class Iter : IObserver<TResult>
      {
        private readonly SelectMany<TSource, TResult>.IndexSelectorImpl _parent;
        private readonly IDisposable _self;

        public Iter(
          SelectMany<TSource, TResult>.IndexSelectorImpl parent,
          IDisposable self)
        {
          this._parent = parent;
          this._self = self;
        }

        public void OnNext(TResult value)
        {
          lock (this._parent._gate)
            this._parent._observer.OnNext(value);
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
      private readonly SelectMany<TSource, TResult> _parent;

      public NoSelectorImpl(
        SelectMany<TSource, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public void OnNext(TSource value)
      {
        IEnumerable<TResult> results;
        try
        {
          results = this._parent._selectorE(value);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        IEnumerator<TResult> enumerator;
        try
        {
          enumerator = results.GetEnumerator();
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
                result = enumerator.Current;
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
      private readonly SelectMany<TSource, TResult> _parent;
      private int _index;

      public Omega(
        SelectMany<TSource, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public void OnNext(TSource value)
      {
        IEnumerable<TResult> results;
        try
        {
          results = this._parent._selectorEI(value, checked (this._index++));
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        IEnumerator<TResult> enumerator;
        try
        {
          enumerator = results.GetEnumerator();
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
                result = enumerator.Current;
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
      private readonly SelectMany<TSource, TResult> _parent;
      private object _gate;
      private CancellationDisposable _cancel;
      private volatile int _count;

      public SelectManyImpl(
        SelectMany<TSource, TResult> parent,
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
        Task<TResult> task;
        try
        {
          Interlocked.Increment(ref this._count);
          task = this._parent._selectorT(value, this._cancel.Token);
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
          this.OnCompletedTask(task);
        else
          task.ContinueWith(new Action<Task<TResult>>(this.OnCompletedTask));
      }

      private void OnCompletedTask(Task<TResult> task)
      {
        switch (task.Status)
        {
          case TaskStatus.RanToCompletion:
            lock (this._gate)
              this._observer.OnNext(task.Result);
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
      private readonly SelectMany<TSource, TResult> _parent;
      private object _gate;
      private CancellationDisposable _cancel;
      private volatile int _count;
      private int _index;

      public Sigma(
        SelectMany<TSource, TResult> parent,
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
        Task<TResult> task;
        try
        {
          Interlocked.Increment(ref this._count);
          task = this._parent._selectorTI(value, checked (this._index++), this._cancel.Token);
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
          this.OnCompletedTask(task);
        else
          task.ContinueWith(new Action<Task<TResult>>(this.OnCompletedTask));
      }

      private void OnCompletedTask(Task<TResult> task)
      {
        switch (task.Status)
        {
          case TaskStatus.RanToCompletion:
            lock (this._gate)
              this._observer.OnNext(task.Result);
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
