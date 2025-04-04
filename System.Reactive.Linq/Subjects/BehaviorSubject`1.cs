// Decompiled with JetBrains decompiler
// Type: System.Reactive.Subjects.BehaviorSubject`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Subjects
{
  public sealed class BehaviorSubject<T> : SubjectBase<T>, IDisposable
  {
    private readonly object _gate = new object();
    private ImmutableList<IObserver<T>> _observers;
    private bool _isStopped;
    private T _value;
    private Exception _exception;
    private bool _isDisposed;

    public BehaviorSubject(T value)
    {
      this._value = value;
      this._observers = ImmutableList<IObserver<T>>.Empty;
    }

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

    public T Value
    {
      get
      {
        lock (this._gate)
        {
          this.CheckDisposed();
          if (this._exception != null)
            throw this._exception;
          return this._value;
        }
      }
    }

    public bool TryGetValue(out T value)
    {
      lock (this._gate)
      {
        if (this._isDisposed)
        {
          value = default (T);
          return false;
        }
        if (this._exception != null)
          throw this._exception;
        value = this._value;
        return true;
      }
    }

    public override void OnCompleted()
    {
      IObserver<T>[] observerArray = (IObserver<T>[]) null;
      lock (this._gate)
      {
        this.CheckDisposed();
        if (!this._isStopped)
        {
          observerArray = this._observers.Data;
          this._observers = ImmutableList<IObserver<T>>.Empty;
          this._isStopped = true;
        }
      }
      if (observerArray == null)
        return;
      foreach (IObserver<T> observer in observerArray)
        observer.OnCompleted();
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
      IObserver<T>[] observerArray = (IObserver<T>[]) null;
      lock (this._gate)
      {
        this.CheckDisposed();
        if (!this._isStopped)
        {
          this._value = value;
          observerArray = this._observers.Data;
        }
      }
      if (observerArray == null)
        return;
      foreach (IObserver<T> observer in observerArray)
        observer.OnNext(value);
    }

    public override IDisposable Subscribe(IObserver<T> observer)
    {
      if (observer == null)
        throw new ArgumentNullException(nameof (observer));
      Exception error = (Exception) null;
      lock (this._gate)
      {
        this.CheckDisposed();
        if (!this._isStopped)
        {
          this._observers = this._observers.Add(observer);
          observer.OnNext(this._value);
          return (IDisposable) new BehaviorSubject<T>.Subscription(this, observer);
        }
        error = this._exception;
      }
      if (error != null)
        observer.OnError(error);
      else
        observer.OnCompleted();
      return Disposable.Empty;
    }

    public override void Dispose()
    {
      lock (this._gate)
      {
        this._isDisposed = true;
        this._observers = (ImmutableList<IObserver<T>>) null;
        this._value = default (T);
        this._exception = (Exception) null;
      }
    }

    private void CheckDisposed()
    {
      if (this._isDisposed)
        throw new ObjectDisposedException(string.Empty);
    }

    private class Subscription : IDisposable
    {
      private readonly BehaviorSubject<T> _subject;
      private IObserver<T> _observer;

      public Subscription(BehaviorSubject<T> subject, IObserver<T> observer)
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
  }
}
