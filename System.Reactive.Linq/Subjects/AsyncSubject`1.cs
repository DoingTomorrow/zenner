// Decompiled with JetBrains decompiler
// Type: System.Reactive.Subjects.AsyncSubject`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using System.Threading;

#nullable disable
namespace System.Reactive.Subjects
{
  public sealed class AsyncSubject<T> : SubjectBase<T>, IDisposable, INotifyCompletion
  {
    private readonly object _gate = new object();
    private ImmutableList<IObserver<T>> _observers;
    private bool _isDisposed;
    private bool _isStopped;
    private T _value;
    private bool _hasValue;
    private Exception _exception;

    public AsyncSubject() => this._observers = ImmutableList<IObserver<T>>.Empty;

    public override bool HasObservers
    {
      get
      {
        ImmutableList<IObserver<T>> observers = this._observers;
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

    public override void OnCompleted()
    {
      IObserver<T>[] observerArray = (IObserver<T>[]) null;
      T obj = default (T);
      bool flag = false;
      lock (this._gate)
      {
        this.CheckDisposed();
        if (!this._isStopped)
        {
          observerArray = this._observers.Data;
          this._observers = ImmutableList<IObserver<T>>.Empty;
          this._isStopped = true;
          obj = this._value;
          flag = this._hasValue;
        }
      }
      if (observerArray == null)
        return;
      if (flag)
      {
        foreach (IObserver<T> observer in observerArray)
        {
          observer.OnNext(obj);
          observer.OnCompleted();
        }
      }
      else
      {
        foreach (IObserver<T> observer in observerArray)
          observer.OnCompleted();
      }
    }

    public override void OnError(Exception error)
    {
      if (error == null)
        throw new ArgumentNullException(nameof (error));
      IObserver<T>[] observerArray = (IObserver<T>[]) null;
      lock (this._gate)
      {
        this.CheckDisposed();
        if (!this._isStopped)
        {
          observerArray = this._observers.Data;
          this._observers = ImmutableList<IObserver<T>>.Empty;
          this._isStopped = true;
          this._exception = error;
        }
      }
      if (observerArray == null)
        return;
      foreach (IObserver<T> observer in observerArray)
        observer.OnError(error);
    }

    public override void OnNext(T value)
    {
      lock (this._gate)
      {
        this.CheckDisposed();
        if (this._isStopped)
          return;
        this._value = value;
        this._hasValue = true;
      }
    }

    public override IDisposable Subscribe(IObserver<T> observer)
    {
      if (observer == null)
        throw new ArgumentNullException(nameof (observer));
      Exception error = (Exception) null;
      T obj = default (T);
      bool flag = false;
      lock (this._gate)
      {
        this.CheckDisposed();
        if (!this._isStopped)
        {
          this._observers = this._observers.Add(observer);
          return (IDisposable) new AsyncSubject<T>.Subscription(this, observer);
        }
        error = this._exception;
        flag = this._hasValue;
        obj = this._value;
      }
      if (error != null)
        observer.OnError(error);
      else if (flag)
      {
        observer.OnNext(obj);
        observer.OnCompleted();
      }
      else
        observer.OnCompleted();
      return Disposable.Empty;
    }

    private void CheckDisposed()
    {
      if (this._isDisposed)
        throw new ObjectDisposedException(string.Empty);
    }

    public override void Dispose()
    {
      lock (this._gate)
      {
        this._isDisposed = true;
        this._observers = (ImmutableList<IObserver<T>>) null;
        this._exception = (Exception) null;
        this._value = default (T);
      }
    }

    public AsyncSubject<T> GetAwaiter() => this;

    public void OnCompleted(Action continuation)
    {
      if (continuation == null)
        throw new ArgumentNullException(nameof (continuation));
      this.OnCompleted(continuation, true);
    }

    private void OnCompleted(Action continuation, bool originalContext)
    {
      this.Subscribe((IObserver<T>) new AsyncSubject<T>.AwaitObserver(continuation, originalContext));
    }

    public bool IsCompleted => this._isStopped;

    public T GetResult()
    {
      if (!this._isStopped)
      {
        ManualResetEvent e = new ManualResetEvent(false);
        this.OnCompleted((Action) (() => e.Set()), false);
        e.WaitOne();
      }
      this._exception.ThrowIfNotNull();
      if (!this._hasValue)
        throw new InvalidOperationException(Strings_Linq.NO_ELEMENTS);
      return this._value;
    }

    private class Subscription : IDisposable
    {
      private readonly AsyncSubject<T> _subject;
      private IObserver<T> _observer;

      public Subscription(AsyncSubject<T> subject, IObserver<T> observer)
      {
        this._subject = subject;
        this._observer = observer;
      }

      public void Dispose()
      {
        if (this._observer == null)
          return;
        lock (this._subject._gate)
        {
          if (this._subject._isDisposed || this._observer == null)
            return;
          this._subject._observers = this._subject._observers.Remove(this._observer);
          this._observer = (IObserver<T>) null;
        }
      }
    }

    private class AwaitObserver : IObserver<T>
    {
      private readonly SynchronizationContext _context;
      private readonly Action _callback;

      public AwaitObserver(Action callback, bool originalContext)
      {
        if (originalContext)
          this._context = SynchronizationContext.Current;
        this._callback = callback;
      }

      public void OnCompleted() => this.InvokeOnOriginalContext();

      public void OnError(Exception error) => this.InvokeOnOriginalContext();

      public void OnNext(T value)
      {
      }

      private void InvokeOnOriginalContext()
      {
        if (this._context != null)
          this._context.Post((SendOrPostCallback) (c => ((Action) c)()), (object) this._callback);
        else
          this._callback();
      }
    }
  }
}
