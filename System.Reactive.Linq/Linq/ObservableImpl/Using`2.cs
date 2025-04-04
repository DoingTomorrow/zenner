// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Using`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Using<TSource, TResource> : Producer<TSource> where TResource : IDisposable
  {
    private readonly Func<TResource> _resourceFactory;
    private readonly Func<TResource, IObservable<TSource>> _observableFactory;

    public Using(
      Func<TResource> resourceFactory,
      Func<TResource, IObservable<TSource>> observableFactory)
    {
      this._resourceFactory = resourceFactory;
      this._observableFactory = observableFactory;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Using<TSource, TResource>._ obj = new Using<TSource, TResource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Using<TSource, TResource> _parent;

      public _(Using<TSource, TResource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        IDisposable disposable2 = Disposable.Empty;
        IObservable<TSource> source;
        try
        {
          TResource resource = this._parent._resourceFactory();
          if ((object) resource != null)
            disposable2 = (IDisposable) resource;
          source = this._parent._observableFactory(resource);
        }
        catch (Exception ex)
        {
          return (IDisposable) StableCompositeDisposable.Create(Observable.Throw<TSource>(ex).SubscribeSafe<TSource>((IObserver<TSource>) this), disposable2);
        }
        return (IDisposable) StableCompositeDisposable.Create(source.SubscribeSafe<TSource>((IObserver<TSource>) this), disposable2);
      }

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
