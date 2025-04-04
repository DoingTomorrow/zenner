// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Latest`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Threading;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Latest<TSource>(IObservable<TSource> source) : PushToPullAdapter<TSource, TSource>(source)
  {
    protected override PushToPullSink<TSource, TSource> Run(IDisposable subscription)
    {
      return (PushToPullSink<TSource, TSource>) new Latest<TSource>._(subscription);
    }

    private class _ : PushToPullSink<TSource, TSource>
    {
      private readonly object _gate;
      private readonly SemaphoreSlim _semaphore;
      private bool _notificationAvailable;
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
        bool flag = false;
        lock (this._gate)
        {
          flag = !this._notificationAvailable;
          this._notificationAvailable = true;
          this._kind = NotificationKind.OnNext;
          this._value = value;
        }
        if (!flag)
          return;
        this._semaphore.Release();
      }

      public override void OnError(Exception error)
      {
        this.Dispose();
        bool flag = false;
        lock (this._gate)
        {
          flag = !this._notificationAvailable;
          this._notificationAvailable = true;
          this._kind = NotificationKind.OnError;
          this._error = error;
        }
        if (!flag)
          return;
        this._semaphore.Release();
      }

      public override void OnCompleted()
      {
        this.Dispose();
        bool flag = false;
        lock (this._gate)
        {
          flag = !this._notificationAvailable;
          this._notificationAvailable = true;
          this._kind = NotificationKind.OnCompleted;
        }
        if (!flag)
          return;
        this._semaphore.Release();
      }

      public override bool TryMoveNext(out TSource current)
      {
        NotificationKind notificationKind = NotificationKind.OnNext;
        Exception exception = (Exception) null;
        this._semaphore.Wait();
        lock (this._gate)
        {
          notificationKind = this._kind;
          switch (notificationKind)
          {
            case NotificationKind.OnNext:
              TSource source = this._value;
              break;
            case NotificationKind.OnError:
              exception = this._error;
              break;
          }
          this._notificationAvailable = false;
        }
        switch (notificationKind)
        {
          case NotificationKind.OnNext:
            current = this._value;
            return true;
          case NotificationKind.OnError:
            exception.Throw();
            break;
        }
        current = default (TSource);
        return false;
      }
    }
  }
}
