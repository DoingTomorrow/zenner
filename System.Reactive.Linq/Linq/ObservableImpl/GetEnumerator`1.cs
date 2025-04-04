// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.GetEnumerator`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class GetEnumerator<TSource> : 
    IEnumerator<TSource>,
    IDisposable,
    IEnumerator,
    IObserver<TSource>
  {
    private readonly ConcurrentQueue<TSource> _queue;
    private TSource _current;
    private Exception _error;
    private bool _done;
    private bool _disposed;
    private readonly SemaphoreSlim _gate;
    private readonly SingleAssignmentDisposable _subscription;

    public GetEnumerator()
    {
      this._queue = new ConcurrentQueue<TSource>();
      this._gate = new SemaphoreSlim(0);
      this._subscription = new SingleAssignmentDisposable();
    }

    public IEnumerator<TSource> Run(IObservable<TSource> source)
    {
      this._subscription.Disposable = source.Subscribe((IObserver<TSource>) this);
      return (IEnumerator<TSource>) this;
    }

    public void OnNext(TSource value)
    {
      this._queue.Enqueue(value);
      this._gate.Release();
    }

    public void OnError(Exception error)
    {
      this._error = error;
      this._subscription.Dispose();
      this._gate.Release();
    }

    public void OnCompleted()
    {
      this._done = true;
      this._subscription.Dispose();
      this._gate.Release();
    }

    public bool MoveNext()
    {
      this._gate.Wait();
      if (this._disposed)
        throw new ObjectDisposedException("");
      if (this._queue.TryDequeue(out this._current))
        return true;
      this._error.ThrowIfNotNull();
      this._gate.Release();
      return false;
    }

    public TSource Current => this._current;

    object IEnumerator.Current => (object) this._current;

    public void Dispose()
    {
      this._subscription.Dispose();
      this._disposed = true;
      this._gate.Release();
    }

    public void Reset() => throw new NotSupportedException();
  }
}
