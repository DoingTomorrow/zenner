﻿// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Buffer`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Buffer<TSource> : Producer<IList<TSource>>
  {
    private readonly IObservable<TSource> _source;
    private readonly int _count;
    private readonly int _skip;
    private readonly TimeSpan _timeSpan;
    private readonly TimeSpan _timeShift;
    private readonly IScheduler _scheduler;

    public Buffer(IObservable<TSource> source, int count, int skip)
    {
      this._source = source;
      this._count = count;
      this._skip = skip;
    }

    public Buffer(
      IObservable<TSource> source,
      TimeSpan timeSpan,
      TimeSpan timeShift,
      IScheduler scheduler)
    {
      this._source = source;
      this._timeSpan = timeSpan;
      this._timeShift = timeShift;
      this._scheduler = scheduler;
    }

    public Buffer(IObservable<TSource> source, TimeSpan timeSpan, int count, IScheduler scheduler)
    {
      this._source = source;
      this._timeSpan = timeSpan;
      this._count = count;
      this._scheduler = scheduler;
    }

    protected override IDisposable Run(
      IObserver<IList<TSource>> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._scheduler == null)
      {
        Buffer<TSource>._ obj = new Buffer<TSource>._(this, observer, cancel);
        setSink((IDisposable) obj);
        return obj.Run();
      }
      if (this._count > 0)
      {
        Buffer<TSource>.Impl impl = new Buffer<TSource>.Impl(this, observer, cancel);
        setSink((IDisposable) impl);
        return impl.Run();
      }
      if (this._timeSpan == this._timeShift)
      {
        Buffer<TSource>.BufferTimeShift bufferTimeShift = new Buffer<TSource>.BufferTimeShift(this, observer, cancel);
        setSink((IDisposable) bufferTimeShift);
        return bufferTimeShift.Run();
      }
      Buffer<TSource>.BufferImpl bufferImpl = new Buffer<TSource>.BufferImpl(this, observer, cancel);
      setSink((IDisposable) bufferImpl);
      return bufferImpl.Run();
    }

    private class _ : Sink<IList<TSource>>, IObserver<TSource>
    {
      private readonly Buffer<TSource> _parent;
      private Queue<IList<TSource>> _queue;
      private int _n;

      public _(Buffer<TSource> parent, IObserver<IList<TSource>> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._queue = new Queue<IList<TSource>>();
        this._n = 0;
        this.CreateWindow();
        return this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this);
      }

      private void CreateWindow() => this._queue.Enqueue((IList<TSource>) new List<TSource>());

      public void OnNext(TSource value)
      {
        foreach (ICollection<TSource> sources in this._queue)
          sources.Add(value);
        int num = this._n - this._parent._count + 1;
        if (num >= 0 && num % this._parent._skip == 0)
        {
          IList<TSource> sourceList = this._queue.Dequeue();
          if (sourceList.Count > 0)
            this._observer.OnNext(sourceList);
        }
        ++this._n;
        if (this._n % this._parent._skip != 0)
          return;
        this.CreateWindow();
      }

      public void OnError(Exception error)
      {
        while (this._queue.Count > 0)
          this._queue.Dequeue().Clear();
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        while (this._queue.Count > 0)
        {
          IList<TSource> sourceList = this._queue.Dequeue();
          if (sourceList.Count > 0)
            this._observer.OnNext(sourceList);
        }
        this._observer.OnCompleted();
        this.Dispose();
      }
    }

    private class BufferImpl : Sink<IList<TSource>>, IObserver<TSource>
    {
      private readonly Buffer<TSource> _parent;
      private TimeSpan _totalTime;
      private TimeSpan _nextShift;
      private TimeSpan _nextSpan;
      private object _gate;
      private Queue<List<TSource>> _q;
      private SerialDisposable _timerD;

      public BufferImpl(
        Buffer<TSource> parent,
        IObserver<IList<TSource>> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._totalTime = TimeSpan.Zero;
        this._nextShift = this._parent._timeShift;
        this._nextSpan = this._parent._timeSpan;
        this._gate = new object();
        this._q = new Queue<List<TSource>>();
        this._timerD = new SerialDisposable();
        this.CreateWindow();
        this.CreateTimer();
        return (IDisposable) StableCompositeDisposable.Create((IDisposable) this._timerD, this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this));
      }

      private void CreateWindow() => this._q.Enqueue(new List<TSource>());

      private void CreateTimer()
      {
        SingleAssignmentDisposable assignmentDisposable = new SingleAssignmentDisposable();
        this._timerD.Disposable = (IDisposable) assignmentDisposable;
        bool flag1 = false;
        bool flag2 = false;
        if (this._nextSpan == this._nextShift)
        {
          flag1 = true;
          flag2 = true;
        }
        else if (this._nextSpan < this._nextShift)
          flag1 = true;
        else
          flag2 = true;
        TimeSpan timeSpan = flag1 ? this._nextSpan : this._nextShift;
        TimeSpan dueTime = timeSpan - this._totalTime;
        this._totalTime = timeSpan;
        if (flag1)
          this._nextSpan += this._parent._timeShift;
        if (flag2)
          this._nextShift += this._parent._timeShift;
        assignmentDisposable.Disposable = this._parent._scheduler.Schedule<Buffer<TSource>.BufferImpl.State>(new Buffer<TSource>.BufferImpl.State()
        {
          isSpan = flag1,
          isShift = flag2
        }, dueTime, new Func<IScheduler, Buffer<TSource>.BufferImpl.State, IDisposable>(this.Tick));
      }

      private IDisposable Tick(IScheduler self, Buffer<TSource>.BufferImpl.State state)
      {
        lock (this._gate)
        {
          if (state.isSpan)
            this._observer.OnNext((IList<TSource>) this._q.Dequeue());
          if (state.isShift)
            this.CreateWindow();
        }
        this.CreateTimer();
        return Disposable.Empty;
      }

      public void OnNext(TSource value)
      {
        lock (this._gate)
        {
          foreach (List<TSource> sourceList in this._q)
            sourceList.Add(value);
        }
      }

      public void OnError(Exception error)
      {
        lock (this._gate)
        {
          while (this._q.Count > 0)
            this._q.Dequeue().Clear();
          this._observer.OnError(error);
          this.Dispose();
        }
      }

      public void OnCompleted()
      {
        lock (this._gate)
        {
          while (this._q.Count > 0)
            this._observer.OnNext((IList<TSource>) this._q.Dequeue());
          this._observer.OnCompleted();
          this.Dispose();
        }
      }

      private struct State
      {
        public bool isSpan;
        public bool isShift;
      }
    }

    private class BufferTimeShift : Sink<IList<TSource>>, IObserver<TSource>
    {
      private readonly Buffer<TSource> _parent;
      private object _gate;
      private List<TSource> _list;

      public BufferTimeShift(
        Buffer<TSource> parent,
        IObserver<IList<TSource>> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        this._list = new List<TSource>();
        return (IDisposable) StableCompositeDisposable.Create(this._parent._scheduler.SchedulePeriodic(this._parent._timeSpan, new Action(this.Tick)), this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this));
      }

      private void Tick()
      {
        lock (this._gate)
        {
          this._observer.OnNext((IList<TSource>) this._list);
          this._list = new List<TSource>();
        }
      }

      public void OnNext(TSource value)
      {
        lock (this._gate)
          this._list.Add(value);
      }

      public void OnError(Exception error)
      {
        lock (this._gate)
        {
          this._list.Clear();
          this._observer.OnError(error);
          this.Dispose();
        }
      }

      public void OnCompleted()
      {
        lock (this._gate)
        {
          this._observer.OnNext((IList<TSource>) this._list);
          this._observer.OnCompleted();
          this.Dispose();
        }
      }
    }

    private class Impl : Sink<IList<TSource>>, IObserver<TSource>
    {
      private readonly Buffer<TSource> _parent;
      private object _gate;
      private IList<TSource> _s;
      private int _n;
      private int _windowId;
      private SerialDisposable _timerD;

      public Impl(Buffer<TSource> parent, IObserver<IList<TSource>> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        this._s = (IList<TSource>) null;
        this._n = 0;
        this._windowId = 0;
        this._timerD = new SerialDisposable();
        this._s = (IList<TSource>) new List<TSource>();
        this.CreateTimer(0);
        return (IDisposable) StableCompositeDisposable.Create((IDisposable) this._timerD, this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this));
      }

      private void CreateTimer(int id)
      {
        SingleAssignmentDisposable assignmentDisposable = new SingleAssignmentDisposable();
        this._timerD.Disposable = (IDisposable) assignmentDisposable;
        assignmentDisposable.Disposable = this._parent._scheduler.Schedule<int>(id, this._parent._timeSpan, new Func<IScheduler, int, IDisposable>(this.Tick));
      }

      private IDisposable Tick(IScheduler self, int id)
      {
        IDisposable empty = Disposable.Empty;
        lock (this._gate)
        {
          if (id != this._windowId)
            return empty;
          this._n = 0;
          int id1 = ++this._windowId;
          IList<TSource> s = this._s;
          this._s = (IList<TSource>) new List<TSource>();
          this._observer.OnNext(s);
          this.CreateTimer(id1);
        }
        return empty;
      }

      public void OnNext(TSource value)
      {
        bool flag = false;
        int id = 0;
        lock (this._gate)
        {
          this._s.Add(value);
          ++this._n;
          if (this._n == this._parent._count)
          {
            flag = true;
            this._n = 0;
            id = ++this._windowId;
            IList<TSource> s = this._s;
            this._s = (IList<TSource>) new List<TSource>();
            this._observer.OnNext(s);
          }
          if (!flag)
            return;
          this.CreateTimer(id);
        }
      }

      public void OnError(Exception error)
      {
        lock (this._gate)
        {
          this._s.Clear();
          this._observer.OnError(error);
          this.Dispose();
        }
      }

      public void OnCompleted()
      {
        lock (this._gate)
        {
          this._observer.OnNext(this._s);
          this._observer.OnCompleted();
          this.Dispose();
        }
      }
    }
  }
}
