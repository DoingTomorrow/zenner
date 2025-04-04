// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Finally`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Finally<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly Action _finallyAction;

    public Finally(IObservable<TSource> source, Action finallyAction)
    {
      this._source = source;
      this._finallyAction = finallyAction;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Finally<TSource>._ obj = new Finally<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Finally<TSource> _parent;

      public _(Finally<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        IDisposable subscription = this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this);
        return Disposable.Create((Action) (() =>
        {
          try
          {
            subscription.Dispose();
          }
          finally
          {
            this._parent._finallyAction();
          }
        }));
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
