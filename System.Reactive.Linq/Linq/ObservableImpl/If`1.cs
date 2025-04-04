// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.If`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class If<TResult> : Producer<TResult>, IEvaluatableObservable<TResult>
  {
    private readonly Func<bool> _condition;
    private readonly IObservable<TResult> _thenSource;
    private readonly IObservable<TResult> _elseSource;

    public If(
      Func<bool> condition,
      IObservable<TResult> thenSource,
      IObservable<TResult> elseSource)
    {
      this._condition = condition;
      this._thenSource = thenSource;
      this._elseSource = elseSource;
    }

    public IObservable<TResult> Eval() => !this._condition() ? this._elseSource : this._thenSource;

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      If<TResult>._ obj = new If<TResult>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TResult>, IObserver<TResult>
    {
      private readonly If<TResult> _parent;

      public _(If<TResult> parent, IObserver<TResult> observer, IDisposable cancel)
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
