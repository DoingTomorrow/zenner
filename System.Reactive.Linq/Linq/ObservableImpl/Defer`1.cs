// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Defer`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Defer<TValue> : Producer<TValue>, IEvaluatableObservable<TValue>
  {
    private readonly Func<IObservable<TValue>> _observableFactory;

    public Defer(Func<IObservable<TValue>> observableFactory)
    {
      this._observableFactory = observableFactory;
    }

    protected override IDisposable Run(
      IObserver<TValue> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Defer<TValue>._ obj = new Defer<TValue>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    public IObservable<TValue> Eval() => this._observableFactory();

    private class _ : Sink<TValue>, IObserver<TValue>
    {
      private readonly Defer<TValue> _parent;

      public _(Defer<TValue> parent, IObserver<TValue> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        IObservable<TValue> source;
        try
        {
          source = this._parent.Eval();
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return Disposable.Empty;
        }
        return source.SubscribeSafe<TValue>((IObserver<TValue>) this);
      }

      public void OnNext(TValue value) => this._observer.OnNext(value);

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
