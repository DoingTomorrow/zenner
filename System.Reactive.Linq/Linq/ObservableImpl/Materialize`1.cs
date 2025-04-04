// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Materialize`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Materialize<TSource> : Producer<Notification<TSource>>
  {
    private readonly IObservable<TSource> _source;

    public Materialize(IObservable<TSource> source) => this._source = source;

    public IObservable<TSource> Dematerialize() => this._source.AsObservable<TSource>();

    protected override IDisposable Run(
      IObserver<Notification<TSource>> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Materialize<TSource>._ obj = new Materialize<TSource>._(observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
    }

    private class _(IObserver<Notification<TSource>> observer, IDisposable cancel) : 
      Sink<Notification<TSource>>(observer, cancel),
      IObserver<TSource>
    {
      public void OnNext(TSource value)
      {
        this._observer.OnNext(Notification.CreateOnNext<TSource>(value));
      }

      public void OnError(Exception error)
      {
        this._observer.OnNext(Notification.CreateOnError<TSource>(error));
        this._observer.OnCompleted();
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnNext(Notification.CreateOnCompleted<TSource>());
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
