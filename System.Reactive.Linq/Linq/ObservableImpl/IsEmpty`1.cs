// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.IsEmpty`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class IsEmpty<TSource> : Producer<bool>
  {
    private readonly IObservable<TSource> _source;

    public IsEmpty(IObservable<TSource> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<bool> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      IsEmpty<TSource>._ obj = new IsEmpty<TSource>._(observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
    }

    private class _(IObserver<bool> observer, IDisposable cancel) : Sink<bool>(observer, cancel), IObserver<TSource>
    {
      public void OnNext(TSource value)
      {
        this._observer.OnNext(false);
        this._observer.OnCompleted();
        this.Dispose();
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnNext(true);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
