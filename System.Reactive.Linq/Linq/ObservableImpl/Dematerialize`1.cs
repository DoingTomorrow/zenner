// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Dematerialize`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Dematerialize<TSource> : Producer<TSource>
  {
    private readonly IObservable<Notification<TSource>> _source;

    public Dematerialize(IObservable<Notification<TSource>> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Dematerialize<TSource>._ obj = new Dematerialize<TSource>._(observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<Notification<TSource>>((IObserver<Notification<TSource>>) obj);
    }

    private class _(IObserver<TSource> observer, IDisposable cancel) : 
      Sink<TSource>(observer, cancel),
      IObserver<Notification<TSource>>
    {
      public void OnNext(Notification<TSource> value)
      {
        switch (value.Kind)
        {
          case NotificationKind.OnNext:
            this._observer.OnNext(value.Value);
            break;
          case NotificationKind.OnError:
            this._observer.OnError(value.Exception);
            this.Dispose();
            break;
          case NotificationKind.OnCompleted:
            this._observer.OnCompleted();
            this.Dispose();
            break;
        }
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
