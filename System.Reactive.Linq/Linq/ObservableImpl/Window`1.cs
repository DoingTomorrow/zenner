// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Window`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Window<TSource> : Producer<IObservable<TSource>>
  {
    private readonly IObservable<TSource> _source;
    private readonly int _count;
    private readonly int _skip;
    private readonly TimeSpan _timeSpan;
    private readonly TimeSpan _timeShift;
    private readonly IScheduler _scheduler;

    public Window(IObservable<TSource> source, int count, int skip)
    {
      this._source = source;
      this._count = count;
      this._skip = skip;
    }

    public Window(
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

    public Window(IObservable<TSource> source, TimeSpan timeSpan, int count, IScheduler scheduler)
    {
      this._source = source;
      this._timeSpan = timeSpan;
      this._count = count;
      this._scheduler = scheduler;
    }

    protected override IDisposable Run(
      IObserver<IObservable<TSource>> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._scheduler == null)
      {
        Window<TSource>._ obj = new Window<TSource>._(this, observer, cancel);
        setSink((IDisposable) obj);
        return obj.Run();
      }
      if (this._count > 0)
      {
        Window<TSource>.BoundedWindowImpl boundedWindowImpl = new Window<TSource>.BoundedWindowImpl(this, observer, cancel);
        setSink((IDisposable) boundedWindowImpl);
        return boundedWindowImpl.Run();
      }
      if (this._timeSpan == this._timeShift)
      {
        Window<TSource>.TimeShiftImpl timeShiftImpl = new Window<TSource>.TimeShiftImpl(this, observer, cancel);
        setSink((IDisposable) timeShiftImpl);
        return timeShiftImpl.Run();
      }
      Window<TSource>.WindowImpl windowImpl = new Window<TSource>.WindowImpl(this, observer, cancel);
      setSink((IDisposable) windowImpl);
      return windowImpl.Run();
    }

    private class _ : Sink<IObservable<TSource>>, IObserver<TSource>
    {
      private readonly Window<TSource> _parent;
      private Queue<ISubject<TSource>> _queue;
      private int _n;
      private SingleAssignmentDisposable _m;
      private RefCountDisposable _refCountDisposable;

      public _(
        Window<TSource> parent,
        IObserver<IObservable<TSource>> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._queue = new Queue<ISubject<TSource>>();
        this._n = 0;
        this._m = new SingleAssignmentDisposable();
        this._refCountDisposable = new RefCountDisposable((IDisposable) this._m);
        this._observer.OnNext(this.CreateWindow());
        this._m.Disposable = this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this);
        return (IDisposable) this._refCountDisposable;
      }

      private IObservable<TSource> CreateWindow()
      {
        Subject<TSource> source = new Subject<TSource>();
        this._queue.Enqueue((ISubject<TSource>) source);
        return (IObservable<TSource>) new WindowObservable<TSource>((IObservable<TSource>) source, this._refCountDisposable);
      }

      public void OnNext(TSource value)
      {
        foreach (IObserver<TSource> observer in this._queue)
          observer.OnNext(value);
        int num = this._n - this._parent._count + 1;
        if (num >= 0 && num % this._parent._skip == 0)
          this._queue.Dequeue().OnCompleted();
        ++this._n;
        if (this._n % this._parent._skip != 0)
          return;
        this._observer.OnNext(this.CreateWindow());
      }

      public void OnError(Exception error)
      {
        while (this._queue.Count > 0)
          this._queue.Dequeue().OnError(error);
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        while (this._queue.Count > 0)
          this._queue.Dequeue().OnCompleted();
        this._observer.OnCompleted();
        this.Dispose();
      }
    }

    private class WindowImpl : Sink<IObservable<TSource>>, IObserver<TSource>
    {
      private readonly Window<TSource> _parent;
      private TimeSpan _totalTime;
      private TimeSpan _nextShift;
      private TimeSpan _nextSpan;
      private object _gate;
      private Queue<ISubject<TSource>> _q;
      private SerialDisposable _timerD;
      private RefCountDisposable _refCountDisposable;

      public WindowImpl(
        Window<TSource> parent,
        IObserver<IObservable<TSource>> observer,
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
        this._q = new Queue<ISubject<TSource>>();
        this._timerD = new SerialDisposable();
        CompositeDisposable compositeDisposable = new CompositeDisposable(2)
        {
          (IDisposable) this._timerD
        };
        this._refCountDisposable = new RefCountDisposable((IDisposable) compositeDisposable);
        this.CreateWindow();
        this.CreateTimer();
        compositeDisposable.Add(this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this));
        return (IDisposable) this._refCountDisposable;
      }

      private void CreateWindow()
      {
        Subject<TSource> source = new Subject<TSource>();
        this._q.Enqueue((ISubject<TSource>) source);
        this._observer.OnNext((IObservable<TSource>) new WindowObservable<TSource>((IObservable<TSource>) source, this._refCountDisposable));
      }

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
        assignmentDisposable.Disposable = this._parent._scheduler.Schedule<Window<TSource>.WindowImpl.State>(new Window<TSource>.WindowImpl.State()
        {
          isSpan = flag1,
          isShift = flag2
        }, dueTime, new Func<IScheduler, Window<TSource>.WindowImpl.State, IDisposable>(this.Tick));
      }

      private IDisposable Tick(IScheduler self, Window<TSource>.WindowImpl.State state)
      {
        lock (this._gate)
        {
          if (state.isSpan)
            this._q.Dequeue().OnCompleted();
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
          foreach (IObserver<TSource> observer in this._q)
            observer.OnNext(value);
        }
      }

      public void OnError(Exception error)
      {
        lock (this._gate)
        {
          foreach (IObserver<TSource> observer in this._q)
            observer.OnError(error);
          this._observer.OnError(error);
          this.Dispose();
        }
      }

      public void OnCompleted()
      {
        lock (this._gate)
        {
          foreach (IObserver<TSource> observer in this._q)
            observer.OnCompleted();
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

    private class TimeShiftImpl : Sink<IObservable<TSource>>, IObserver<TSource>
    {
      private readonly Window<TSource> _parent;
      private object _gate;
      private Subject<TSource> _subject;
      private RefCountDisposable _refCountDisposable;

      public TimeShiftImpl(
        Window<TSource> parent,
        IObserver<IObservable<TSource>> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        CompositeDisposable compositeDisposable = new CompositeDisposable(2);
        this._refCountDisposable = new RefCountDisposable((IDisposable) compositeDisposable);
        this.CreateWindow();
        compositeDisposable.Add(this._parent._scheduler.SchedulePeriodic(this._parent._timeSpan, new Action(this.Tick)));
        compositeDisposable.Add(this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this));
        return (IDisposable) this._refCountDisposable;
      }

      private void Tick()
      {
        lock (this._gate)
        {
          this._subject.OnCompleted();
          this.CreateWindow();
        }
      }

      private void CreateWindow()
      {
        this._subject = new Subject<TSource>();
        this._observer.OnNext((IObservable<TSource>) new WindowObservable<TSource>((IObservable<TSource>) this._subject, this._refCountDisposable));
      }

      public void OnNext(TSource value)
      {
        lock (this._gate)
          this._subject.OnNext(value);
      }

      public void OnError(Exception error)
      {
        lock (this._gate)
        {
          this._subject.OnError(error);
          this._observer.OnError(error);
          this.Dispose();
        }
      }

      public void OnCompleted()
      {
        lock (this._gate)
        {
          this._subject.OnCompleted();
          this._observer.OnCompleted();
          this.Dispose();
        }
      }
    }

    private class BoundedWindowImpl : Sink<IObservable<TSource>>, IObserver<TSource>
    {
      private readonly Window<TSource> _parent;
      private object _gate;
      private ISubject<TSource> _s;
      private int _n;
      private int _windowId;
      private SerialDisposable _timerD;
      private RefCountDisposable _refCountDisposable;

      public BoundedWindowImpl(
        Window<TSource> parent,
        IObserver<IObservable<TSource>> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        this._s = (ISubject<TSource>) null;
        this._n = 0;
        this._windowId = 0;
        this._timerD = new SerialDisposable();
        CompositeDisposable compositeDisposable = new CompositeDisposable(2)
        {
          (IDisposable) this._timerD
        };
        this._refCountDisposable = new RefCountDisposable((IDisposable) compositeDisposable);
        this._s = (ISubject<TSource>) new Subject<TSource>();
        this._observer.OnNext((IObservable<TSource>) new WindowObservable<TSource>((IObservable<TSource>) this._s, this._refCountDisposable));
        this.CreateTimer(0);
        compositeDisposable.Add(this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this));
        return (IDisposable) this._refCountDisposable;
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
        int id1 = 0;
        lock (this._gate)
        {
          if (id != this._windowId)
            return empty;
          this._n = 0;
          id1 = ++this._windowId;
          this._s.OnCompleted();
          this._s = (ISubject<TSource>) new Subject<TSource>();
          this._observer.OnNext((IObservable<TSource>) new WindowObservable<TSource>((IObservable<TSource>) this._s, this._refCountDisposable));
        }
        this.CreateTimer(id1);
        return empty;
      }

      public void OnNext(TSource value)
      {
        bool flag = false;
        int id = 0;
        lock (this._gate)
        {
          this._s.OnNext(value);
          ++this._n;
          if (this._n == this._parent._count)
          {
            flag = true;
            this._n = 0;
            id = ++this._windowId;
            this._s.OnCompleted();
            this._s = (ISubject<TSource>) new Subject<TSource>();
            this._observer.OnNext((IObservable<TSource>) new WindowObservable<TSource>((IObservable<TSource>) this._s, this._refCountDisposable));
          }
        }
        if (!flag)
          return;
        this.CreateTimer(id);
      }

      public void OnError(Exception error)
      {
        lock (this._gate)
        {
          this._s.OnError(error);
          this._observer.OnError(error);
          this.Dispose();
        }
      }

      public void OnCompleted()
      {
        lock (this._gate)
        {
          this._s.OnCompleted();
          this._observer.OnCompleted();
          this.Dispose();
        }
      }
    }
  }
}
