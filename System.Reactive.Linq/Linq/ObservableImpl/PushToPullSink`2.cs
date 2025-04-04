// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.PushToPullSink`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal abstract class PushToPullSink<TSource, TResult> : 
    IObserver<TSource>,
    IEnumerator<TResult>,
    IDisposable,
    IEnumerator
  {
    private readonly IDisposable _subscription;
    private bool _done;

    public PushToPullSink(IDisposable subscription) => this._subscription = subscription;

    public abstract void OnNext(TSource value);

    public abstract void OnError(Exception error);

    public abstract void OnCompleted();

    public abstract bool TryMoveNext(out TResult current);

    public bool MoveNext()
    {
      if (!this._done)
      {
        TResult current = default (TResult);
        if (this.TryMoveNext(out current))
        {
          this.Current = current;
          return true;
        }
        this._done = true;
        this._subscription.Dispose();
      }
      return false;
    }

    public TResult Current { get; private set; }

    object IEnumerator.Current => (object) this.Current;

    public void Reset() => throw new NotSupportedException();

    public void Dispose() => this._subscription.Dispose();
  }
}
