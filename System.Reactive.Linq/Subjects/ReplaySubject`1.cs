// Decompiled with JetBrains decompiler
// Type: System.Reactive.Subjects.ReplaySubject`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Subjects
{
  public sealed class ReplaySubject<T> : SubjectBase<T>, IDisposable
  {
    private readonly SubjectBase<T> _implementation;

    public ReplaySubject()
      : this(int.MaxValue)
    {
    }

    public ReplaySubject(IScheduler scheduler)
    {
      this._implementation = (SubjectBase<T>) new ReplaySubject<T>.ReplayByTime(scheduler);
    }

    public ReplaySubject(int bufferSize)
    {
      if (bufferSize != 1)
      {
        if (bufferSize == int.MaxValue)
          this._implementation = (SubjectBase<T>) new ReplaySubject<T>.ReplayAll();
        else
          this._implementation = (SubjectBase<T>) new ReplaySubject<T>.ReplayMany(bufferSize);
      }
      else
        this._implementation = (SubjectBase<T>) new ReplaySubject<T>.ReplayOne();
    }

    public ReplaySubject(int bufferSize, IScheduler scheduler)
    {
      this._implementation = (SubjectBase<T>) new ReplaySubject<T>.ReplayByTime(bufferSize, scheduler);
    }

    public ReplaySubject(TimeSpan window)
    {
      this._implementation = (SubjectBase<T>) new ReplaySubject<T>.ReplayByTime(window);
    }

    public ReplaySubject(TimeSpan window, IScheduler scheduler)
    {
      this._implementation = (SubjectBase<T>) new ReplaySubject<T>.ReplayByTime(window, scheduler);
    }

    public ReplaySubject(int bufferSize, TimeSpan window)
    {
      this._implementation = (SubjectBase<T>) new ReplaySubject<T>.ReplayByTime(bufferSize, window);
    }

    public ReplaySubject(int bufferSize, TimeSpan window, IScheduler scheduler)
    {
      this._implementation = (SubjectBase<T>) new ReplaySubject<T>.ReplayByTime(bufferSize, window, scheduler);
    }

    public override bool HasObservers => this._implementation.HasObservers;

    public override bool IsDisposed => this._implementation.IsDisposed;

    public override void OnNext(T value) => this._implementation.OnNext(value);

    public override void OnError(Exception error)
    {
      if (error == null)
        throw new ArgumentNullException(nameof (error));
      this._implementation.OnError(error);
    }

    public override void OnCompleted() => this._implementation.OnCompleted();

    public override IDisposable Subscribe(IObserver<T> observer)
    {
      return observer != null ? this._implementation.Subscribe(observer) : throw new ArgumentNullException(nameof (observer));
    }

    public override void Dispose() => this._implementation.Dispose();

    private abstract class ReplayBase : SubjectBase<T>
    {
      private readonly object _gate = new object();
      private ImmutableList<IScheduledObserver<T>> _observers;
      private bool _isStopped;
      private Exception _error;
      private bool _isDisposed;

      public ReplayBase()
      {
        this._observers = ImmutableList<IScheduledObserver<T>>.Empty;
        this._isStopped = false;
        this._error = (Exception) null;
      }

      public override bool HasObservers
      {
        get
        {
          ImmutableList<IScheduledObserver<T>> observers = this._observers;
          return observers != null && observers.Data.Length != 0;
        }
      }

      public override bool IsDisposed
      {
        get
        {
          lock (this._gate)
            return this._isDisposed;
        }
      }

      public override void OnNext(T value)
      {
        IScheduledObserver<T>[] scheduledObserverArray = (IScheduledObserver<T>[]) null;
        lock (this._gate)
        {
          this.CheckDisposed();
          if (!this._isStopped)
          {
            this.Next(value);
            this.Trim();
            scheduledObserverArray = this._observers.Data;
            foreach (IObserver<T> observer in scheduledObserverArray)
              observer.OnNext(value);
          }
        }
        if (scheduledObserverArray == null)
          return;
        foreach (IScheduledObserver<T> scheduledObserver in scheduledObserverArray)
          scheduledObserver.EnsureActive();
      }

      public override void OnError(Exception error)
      {
        IScheduledObserver<T>[] scheduledObserverArray = (IScheduledObserver<T>[]) null;
        lock (this._gate)
        {
          this.CheckDisposed();
          if (!this._isStopped)
          {
            this._isStopped = true;
            this._error = error;
            this.Trim();
            scheduledObserverArray = this._observers.Data;
            foreach (IObserver<T> observer in scheduledObserverArray)
              observer.OnError(error);
            this._observers = ImmutableList<IScheduledObserver<T>>.Empty;
          }
        }
        if (scheduledObserverArray == null)
          return;
        foreach (IScheduledObserver<T> scheduledObserver in scheduledObserverArray)
          scheduledObserver.EnsureActive();
      }

      public override void OnCompleted()
      {
        IScheduledObserver<T>[] scheduledObserverArray = (IScheduledObserver<T>[]) null;
        lock (this._gate)
        {
          this.CheckDisposed();
          if (!this._isStopped)
          {
            this._isStopped = true;
            this.Trim();
            scheduledObserverArray = this._observers.Data;
            foreach (IObserver<T> observer in scheduledObserverArray)
              observer.OnCompleted();
            this._observers = ImmutableList<IScheduledObserver<T>>.Empty;
          }
        }
        if (scheduledObserverArray == null)
          return;
        foreach (IScheduledObserver<T> scheduledObserver in scheduledObserverArray)
          scheduledObserver.EnsureActive();
      }

      public override IDisposable Subscribe(IObserver<T> observer)
      {
        IScheduledObserver<T> scheduledObserver = this.CreateScheduledObserver(observer);
        int count = 0;
        IDisposable disposable = Disposable.Empty;
        lock (this._gate)
        {
          this.CheckDisposed();
          this.Trim();
          count = this.Replay((IObserver<T>) scheduledObserver);
          if (this._error != null)
          {
            ++count;
            scheduledObserver.OnError(this._error);
          }
          else if (this._isStopped)
          {
            ++count;
            scheduledObserver.OnCompleted();
          }
          if (!this._isStopped)
          {
            disposable = (IDisposable) new ReplaySubject<T>.ReplayBase.Subscription(this, scheduledObserver);
            this._observers = this._observers.Add(scheduledObserver);
          }
        }
        scheduledObserver.EnsureActive(count);
        return disposable;
      }

      public override void Dispose()
      {
        lock (this._gate)
        {
          this._isDisposed = true;
          this._observers = (ImmutableList<IScheduledObserver<T>>) null;
          this.DisposeCore();
        }
      }

      protected abstract void DisposeCore();

      protected abstract void Next(T value);

      protected abstract int Replay(IObserver<T> observer);

      protected abstract void Trim();

      protected abstract IScheduledObserver<T> CreateScheduledObserver(IObserver<T> observer);

      private void CheckDisposed()
      {
        if (this._isDisposed)
          throw new ObjectDisposedException(string.Empty);
      }

      private void Unsubscribe(IScheduledObserver<T> observer)
      {
        lock (this._gate)
        {
          if (this._isDisposed)
            return;
          this._observers = this._observers.Remove(observer);
        }
      }

      private sealed class Subscription : IDisposable
      {
        private readonly ReplaySubject<T>.ReplayBase _subject;
        private readonly IScheduledObserver<T> _observer;

        public Subscription(ReplaySubject<T>.ReplayBase subject, IScheduledObserver<T> observer)
        {
          this._subject = subject;
          this._observer = observer;
        }

        public void Dispose()
        {
          this._observer.Dispose();
          this._subject.Unsubscribe(this._observer);
        }
      }
    }

    private sealed class ReplayByTime : ReplaySubject<T>.ReplayBase
    {
      private const int InfiniteBufferSize = 2147483647;
      private readonly int _bufferSize;
      private readonly TimeSpan _window;
      private readonly IScheduler _scheduler;
      private readonly IStopwatch _stopwatch;
      private readonly Queue<TimeInterval<T>> _queue;

      public ReplayByTime(int bufferSize, TimeSpan window, IScheduler scheduler)
      {
        if (bufferSize < 0)
          throw new ArgumentOutOfRangeException(nameof (bufferSize));
        if (window < TimeSpan.Zero)
          throw new ArgumentOutOfRangeException(nameof (window));
        if (scheduler == null)
          throw new ArgumentNullException(nameof (scheduler));
        this._bufferSize = bufferSize;
        this._window = window;
        this._scheduler = scheduler;
        this._stopwatch = this._scheduler.StartStopwatch();
        this._queue = new Queue<TimeInterval<T>>();
      }

      public ReplayByTime(int bufferSize, TimeSpan window)
        : this(bufferSize, window, SchedulerDefaults.Iteration)
      {
      }

      public ReplayByTime(IScheduler scheduler)
        : this(int.MaxValue, TimeSpan.MaxValue, scheduler)
      {
      }

      public ReplayByTime(int bufferSize, IScheduler scheduler)
        : this(bufferSize, TimeSpan.MaxValue, scheduler)
      {
      }

      public ReplayByTime(TimeSpan window, IScheduler scheduler)
        : this(int.MaxValue, window, scheduler)
      {
      }

      public ReplayByTime(TimeSpan window)
        : this(int.MaxValue, window, SchedulerDefaults.Iteration)
      {
      }

      protected override IScheduledObserver<T> CreateScheduledObserver(IObserver<T> observer)
      {
        return (IScheduledObserver<T>) new ScheduledObserver<T>(this._scheduler, observer);
      }

      protected override void DisposeCore() => this._queue.Clear();

      protected override void Next(T value)
      {
        TimeSpan elapsed = this._stopwatch.Elapsed;
        this._queue.Enqueue(new TimeInterval<T>(value, elapsed));
      }

      protected override int Replay(IObserver<T> observer)
      {
        int count = this._queue.Count;
        foreach (TimeInterval<T> timeInterval in this._queue)
          observer.OnNext(timeInterval.Value);
        return count;
      }

      protected override void Trim()
      {
        TimeSpan elapsed = this._stopwatch.Elapsed;
        while (this._queue.Count > this._bufferSize)
          this._queue.Dequeue();
        while (this._queue.Count > 0 && elapsed.Subtract(this._queue.Peek().Interval).CompareTo(this._window) > 0)
          this._queue.Dequeue();
      }
    }

    private sealed class ReplayOne : ReplaySubject<T>.ReplayBufferBase
    {
      private bool _hasValue;
      private T _value;

      protected override void Trim()
      {
      }

      protected override void Next(T value)
      {
        this._hasValue = true;
        this._value = value;
      }

      protected override int Replay(IObserver<T> observer)
      {
        int num = 0;
        if (this._hasValue)
        {
          num = 1;
          observer.OnNext(this._value);
        }
        return num;
      }

      protected override void DisposeCore() => this._value = default (T);
    }

    private sealed class ReplayMany : ReplaySubject<T>.ReplayManyBase
    {
      private readonly int _bufferSize;

      public ReplayMany(int bufferSize)
        : base(bufferSize)
      {
        this._bufferSize = bufferSize;
      }

      protected override void Trim()
      {
        while (this._queue.Count > this._bufferSize)
          this._queue.Dequeue();
      }
    }

    private sealed class ReplayAll : ReplaySubject<T>.ReplayManyBase
    {
      public ReplayAll()
        : base(0)
      {
      }

      protected override void Trim()
      {
      }
    }

    private abstract class ReplayBufferBase : ReplaySubject<T>.ReplayBase
    {
      protected override IScheduledObserver<T> CreateScheduledObserver(IObserver<T> observer)
      {
        return (IScheduledObserver<T>) new FastImmediateObserver<T>(observer);
      }

      protected override void DisposeCore()
      {
      }
    }

    private abstract class ReplayManyBase : ReplaySubject<T>.ReplayBufferBase
    {
      protected readonly Queue<T> _queue;

      protected ReplayManyBase(int queueSize)
      {
        this._queue = new Queue<T>(Math.Min(queueSize, 64));
      }

      protected override void Next(T value) => this._queue.Enqueue(value);

      protected override int Replay(IObserver<T> observer)
      {
        int count = this._queue.Count;
        foreach (T obj in this._queue)
          observer.OnNext(obj);
        return count;
      }

      protected override void DisposeCore() => this._queue.Clear();
    }
  }
}
