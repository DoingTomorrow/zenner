// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Next`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Threading;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Next<TSource>(IObservable<TSource> source) : PushToPullAdapter<TSource, TSource>(source)
  {
    protected override PushToPullSink<TSource, TSource> Run(IDisposable subscription)
    {
      return (PushToPullSink<TSource, TSource>) new Next<TSource>._(subscription);
    }

    private class _ : PushToPullSink<TSource, TSource>
    {
      private readonly object _gate;
      private readonly SemaphoreSlim _semaphore;
      private bool _waiting;
      private NotificationKind _kind;
      private TSource _value;
      private Exception _error;

      public _(IDisposable subscription)
        : base(subscription)
      {
        this._gate = new object();
        this._semaphore = new SemaphoreSlim(0, 1);
      }

      public override void OnNext(TSource value)
      {
        lock (this._gate)
        {
          if (this._waiting)
          {
            this._value = value;
            this._kind = NotificationKind.OnNext;
            this._semaphore.Release();
          }
          this._waiting = false;
        }
      }

      public override void OnError(Exception error)
      {
        this.Dispose();
        lock (this._gate)
        {
          this._error = error;
          this._kind = NotificationKind.OnError;
          if (this._waiting)
            this._semaphore.Release();
          this._waiting = false;
        }
      }

      public override void OnCompleted()
      {
        this.Dispose();
        lock (this._gate)
        {
          this._kind = NotificationKind.OnCompleted;
          if (this._waiting)
            this._semaphore.Release();
          this._waiting = false;
        }
      }

      public override bool TryMoveNext(out TSource current)
      {
        bool flag = false;
        lock (this._gate)
        {
          this._waiting = true;
          flag = this._kind != 0;
        }
        if (!flag)
          this._semaphore.Wait();
        switch (this._kind)
        {
          case NotificationKind.OnNext:
            current = this._value;
            return true;
          case NotificationKind.OnError:
            this._error.Throw();
            break;
        }
        current = default (TSource);
        return false;
      }
    }
  }
}
