// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Cast`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Cast<TSource, TResult> : Producer<TResult>
  {
    private readonly IObservable<TSource> _source;

    public Cast(IObservable<TSource> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Cast<TSource, TResult>._ obj = new Cast<TSource, TResult>._(observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
    }

    private class _(IObserver<TResult> observer, IDisposable cancel) : 
      Sink<TResult>(observer, cancel),
      IObserver<TSource>
    {
      public void OnNext(TSource value)
      {
        TResult result1 = default (TResult);
        TResult result2;
        try
        {
          result2 = (TResult) (object) value;
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        this._observer.OnNext(result2);
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
