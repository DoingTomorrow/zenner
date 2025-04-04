// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.AddRef`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class AddRef<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly RefCountDisposable _refCount;

    public AddRef(IObservable<TSource> source, RefCountDisposable refCount)
    {
      this._source = source;
      this._refCount = refCount;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      ICancelable cancel1 = StableCompositeDisposable.Create(this._refCount.GetDisposable(), cancel);
      AddRef<TSource>._ obj = new AddRef<TSource>._(observer, (IDisposable) cancel1);
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
