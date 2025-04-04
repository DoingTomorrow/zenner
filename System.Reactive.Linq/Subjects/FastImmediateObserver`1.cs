// Decompiled with JetBrains decompiler
// Type: System.Reactive.Subjects.FastImmediateObserver`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace System.Reactive.Subjects
{
  internal class FastImmediateObserver<T> : IScheduledObserver<T>, IObserver<T>, IDisposable
  {
    private readonly object _gate = new object();
    private volatile IObserver<T> _observer;
    private Queue<T> _queue = new Queue<T>();
    private Queue<T> _queue2;
    private Exception _error;
    private bool _done;
    private bool _busy;
    private bool _hasFaulted;

    public FastImmediateObserver(IObserver<T> observer) => this._observer = observer;

    public void Dispose() => this.Done();

    public void EnsureActive() => this.EnsureActive(1);

    public void EnsureActive(int count)
    {
      bool flag1 = false;
      lock (this._gate)
      {
        if (!this._hasFaulted)
        {
          if (!this._busy)
          {
            flag1 = true;
            this._busy = true;
          }
        }
      }
      if (!flag1)
        return;
label_8:
      Queue<T> objQueue = (Queue<T>) null;
      Exception error = (Exception) null;
      bool flag2 = false;
      lock (this._gate)
      {
        if (this._queue.Count > 0)
        {
          if (this._queue2 == null)
            this._queue2 = new Queue<T>();
          objQueue = this._queue;
          this._queue = this._queue2;
          this._queue2 = (Queue<T>) null;
        }
        if (this._error != null)
          error = this._error;
        else if (this._done)
          flag2 = true;
        else if (objQueue == null)
        {
          this._busy = false;
          return;
        }
      }
      try
      {
        if (objQueue != null)
        {
          while (objQueue.Count > 0)
            this._observer.OnNext(objQueue.Dequeue());
          lock (this._gate)
            this._queue2 = objQueue;
        }
        if (error != null)
          this.Done().OnError(error);
        else if (flag2)
          this.Done().OnCompleted();
        else
          goto label_8;
      }
      catch
      {
        lock (this._gate)
        {
          this._hasFaulted = true;
          this._queue.Clear();
        }
        throw;
      }
    }

    public void OnCompleted()
    {
      lock (this._gate)
      {
        if (this._hasFaulted)
          return;
        this._done = true;
      }
    }

    public void OnError(Exception error)
    {
      lock (this._gate)
      {
        if (this._hasFaulted)
          return;
        this._error = error;
      }
    }

    public void OnNext(T value)
    {
      lock (this._gate)
      {
        if (this._hasFaulted)
          return;
        this._queue.Enqueue(value);
      }
    }

    private IObserver<T> Done()
    {
      return Interlocked.Exchange<IObserver<T>>(ref this._observer, NopObserver<T>.Instance);
    }
  }
}
