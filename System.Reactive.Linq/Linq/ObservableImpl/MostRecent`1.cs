// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.MostRecent`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class MostRecent<TSource> : PushToPullAdapter<TSource, TSource>
  {
    private readonly TSource _initialValue;

    public MostRecent(IObservable<TSource> source, TSource initialValue)
      : base(source)
    {
      this._initialValue = initialValue;
    }

    protected override PushToPullSink<TSource, TSource> Run(IDisposable subscription)
    {
      return (PushToPullSink<TSource, TSource>) new MostRecent<TSource>._(this._initialValue, subscription);
    }

    private class _ : PushToPullSink<TSource, TSource>
    {
      private volatile NotificationKind _kind;
      private TSource _value;
      private Exception _error;

      public _(TSource initialValue, IDisposable subscription)
        : base(subscription)
      {
        this._kind = NotificationKind.OnNext;
        this._value = initialValue;
      }

      public override void OnNext(TSource value)
      {
        this._value = value;
        this._kind = NotificationKind.OnNext;
      }

      public override void OnError(Exception error)
      {
        this.Dispose();
        this._error = error;
        this._kind = NotificationKind.OnError;
      }

      public override void OnCompleted()
      {
        this.Dispose();
        this._kind = NotificationKind.OnCompleted;
      }

      public override bool TryMoveNext(out TSource current)
      {
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
