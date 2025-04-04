// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.AsObservable`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class AsObservable<TSource> : Producer<TSource>, IEvaluatableObservable<TSource>
  {
    private readonly IObservable<TSource> _source;

    public AsObservable(IObservable<TSource> source) => this._source = source;

    public IObservable<TSource> Omega() => (IObservable<TSource>) this;

    public IObservable<TSource> Eval() => this._source;

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      AsObservable<TSource>._ obj = new AsObservable<TSource>._(observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
    }

    private class _(IObserver<TSource> observer, IDisposable cancel) : 
      Sink<TSource>(observer, cancel),
      IObserver<TSource>
    {
      public void OnNext(TSource value) => this._observer.OnNext(value);

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
