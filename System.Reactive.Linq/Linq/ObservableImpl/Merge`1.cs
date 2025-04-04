// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Merge`1
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
  internal class Merge<TSource> : Producer<TSource>
  {
    private readonly IObservable<IObservable<TSource>> _sources;
    private readonly int _maxConcurrent;
    private readonly IObservable<Task<TSource>> _sourcesT;

    public Merge(IObservable<IObservable<TSource>> sources) => this._sources = sources;

    public Merge(IObservable<IObservable<TSource>> sources, int maxConcurrent)
    {
      this._sources = sources;
      this._maxConcurrent = maxConcurrent;
    }

    public Merge(IObservable<Task<TSource>> sources) => this._sourcesT = sources;

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._maxConcurrent > 0)
      {
        Merge<TSource>.MergeConcurrent mergeConcurrent = new Merge<TSource>.MergeConcurrent(this, observer, cancel);
        setSink((IDisposable) mergeConcurrent);
        return mergeConcurrent.Run();
      }
      if (this._sourcesT != null)
      {
        Merge<TSource>.MergeImpl mergeImpl = new Merge<TSource>.MergeImpl(this, observer, cancel);
        setSink((IDisposable) mergeImpl);
        return mergeImpl.Run();
      }
      Merge<TSource>._ obj = new Merge<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TSource>, IObserver<IObservable<TSource>>
    {
      private readonly Merge<TSource> _parent;
      private object _gate;
      private bool _isStopped;
      private CompositeDisposable _group;
      private SingleAssignmentDisposable _sourceSubscription;

      public _(Merge<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
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
        this._sourceSubscription.Disposable = this._parent._sources.SubscribeSafe<IObservable<TSource>>((IObserver<IObservable<TSource>>) this);
        return (IDisposable) this._group;
      }

      public void OnNext(IObservable<TSource> value)
      {
        SingleAssignmentDisposable self = new SingleAssignmentDisposable();
        this._group.Add((IDisposable) self);
        self.Disposable = value.SubscribeSafe<TSource>((IObserver<TSource>) new Merge<TSource>._.Iter(this, (IDisposable) self));
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

      private class Iter : IObserver<TSource>
      {
        private readonly Merge<TSource>._ _parent;
        private readonly IDisposable _self;

        public Iter(Merge<TSource>._ parent, IDisposable self)
        {
          this._parent = parent;
          this._self = self;
        }

        public void OnNext(TSource value)
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

    private class MergeConcurrent : Sink<TSource>, IObserver<IObservable<TSource>>
    {
      private readonly Merge<TSource> _parent;
      private object _gate;
      private Queue<IObservable<TSource>> _q;
      private bool _isStopped;
      private SingleAssignmentDisposable _sourceSubscription;
      private CompositeDisposable _group;
      private int _activeCount;

      public MergeConcurrent(
        Merge<TSource> parent,
        IObserver<TSource> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        this._q = new Queue<IObservable<TSource>>();
        this._isStopped = false;
        this._activeCount = 0;
        this._group = new CompositeDisposable();
        this._sourceSubscription = new SingleAssignmentDisposable();
        this._sourceSubscription.Disposable = this._parent._sources.SubscribeSafe<IObservable<TSource>>((IObserver<IObservable<TSource>>) this);
        this._group.Add((IDisposable) this._sourceSubscription);
        return (IDisposable) this._group;
      }

      public void OnNext(IObservable<TSource> value)
      {
        lock (this._gate)
        {
          if (this._activeCount < this._parent._maxConcurrent)
          {
            ++this._activeCount;
            this.Subscribe(value);
          }
          else
            this._q.Enqueue(value);
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
          this._isStopped = true;
          if (this._activeCount == 0)
          {
            this._observer.OnCompleted();
            this.Dispose();
          }
          else
            this._sourceSubscription.Dispose();
        }
      }

      private void Subscribe(IObservable<TSource> innerSource)
      {
        SingleAssignmentDisposable self = new SingleAssignmentDisposable();
        this._group.Add((IDisposable) self);
        self.Disposable = innerSource.SubscribeSafe<TSource>((IObserver<TSource>) new Merge<TSource>.MergeConcurrent.Iter(this, (IDisposable) self));
      }

      private class Iter : IObserver<TSource>
      {
        private readonly Merge<TSource>.MergeConcurrent _parent;
        private readonly IDisposable _self;

        public Iter(Merge<TSource>.MergeConcurrent parent, IDisposable self)
        {
          this._parent = parent;
          this._self = self;
        }

        public void OnNext(TSource value)
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
          lock (this._parent._gate)
          {
            if (this._parent._q.Count > 0)
            {
              this._parent.Subscribe(this._parent._q.Dequeue());
            }
            else
            {
              --this._parent._activeCount;
              if (!this._parent._isStopped || this._parent._activeCount != 0)
                return;
              this._parent._observer.OnCompleted();
              this._parent.Dispose();
            }
          }
        }
      }
    }

    private class MergeImpl : Sink<TSource>, IObserver<Task<TSource>>
    {
      private readonly Merge<TSource> _parent;
      private object _gate;
      private volatile int _count;

      public MergeImpl(Merge<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        this._count = 1;
        return this._parent._sourcesT.SubscribeSafe<Task<TSource>>((IObserver<Task<TSource>>) this);
      }

      public void OnNext(Task<TSource> value)
      {
        Interlocked.Increment(ref this._count);
        if (value.IsCompleted)
          this.OnCompletedTask(value);
        else
          value.ContinueWith(new Action<Task<TSource>>(this.OnCompletedTask));
      }

      private void OnCompletedTask(Task<TSource> task)
      {
        switch (task.Status)
        {
          case TaskStatus.RanToCompletion:
            lock (this._gate)
              this._observer.OnNext(task.Result);
            this.OnCompleted();
            break;
          case TaskStatus.Canceled:
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
