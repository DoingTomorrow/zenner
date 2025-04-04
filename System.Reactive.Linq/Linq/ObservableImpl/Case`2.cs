// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Case`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Case<TValue, TResult> : Producer<TResult>, IEvaluatableObservable<TResult>
  {
    private readonly Func<TValue> _selector;
    private readonly IDictionary<TValue, IObservable<TResult>> _sources;
    private readonly IObservable<TResult> _defaultSource;

    public Case(
      Func<TValue> selector,
      IDictionary<TValue, IObservable<TResult>> sources,
      IObservable<TResult> defaultSource)
    {
      this._selector = selector;
      this._sources = sources;
      this._defaultSource = defaultSource;
    }

    public IObservable<TResult> Eval()
    {
      IObservable<TResult> observable = (IObservable<TResult>) null;
      return this._sources.TryGetValue(this._selector(), out observable) ? observable : this._defaultSource;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Case<TValue, TResult>._ obj = new Case<TValue, TResult>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TResult>, IObserver<TResult>
    {
      private readonly Case<TValue, TResult> _parent;

      public _(Case<TValue, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        IObservable<TResult> source;
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
        return source.SubscribeSafe<TResult>((IObserver<TResult>) this);
      }

      public void OnNext(TResult value) => this._observer.OnNext(value);

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
