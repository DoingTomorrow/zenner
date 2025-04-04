// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Do`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Do<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly Action<TSource> _onNext;
    private readonly Action<Exception> _onError;
    private readonly Action _onCompleted;

    public Do(
      IObservable<TSource> source,
      Action<TSource> onNext,
      Action<Exception> onError,
      Action onCompleted)
    {
      this._source = source;
      this._onNext = onNext;
      this._onError = onError;
      this._onCompleted = onCompleted;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Do<TSource>._ obj = new Do<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Do<TSource> _parent;

      public _(Do<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public void OnNext(TSource value)
      {
        try
        {
          this._parent._onNext(value);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        this._observer.OnNext(value);
      }

      public void OnError(Exception error)
      {
        try
        {
          this._parent._onError(error);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        try
        {
          this._parent._onCompleted();
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
