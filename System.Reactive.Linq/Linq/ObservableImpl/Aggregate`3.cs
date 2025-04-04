// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Aggregate`3
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Aggregate<TSource, TAccumulate, TResult> : Producer<TResult>
  {
    private readonly IObservable<TSource> _source;
    private readonly TAccumulate _seed;
    private readonly Func<TAccumulate, TSource, TAccumulate> _accumulator;
    private readonly Func<TAccumulate, TResult> _resultSelector;

    public Aggregate(
      IObservable<TSource> source,
      TAccumulate seed,
      Func<TAccumulate, TSource, TAccumulate> accumulator,
      Func<TAccumulate, TResult> resultSelector)
    {
      this._source = source;
      this._seed = seed;
      this._accumulator = accumulator;
      this._resultSelector = resultSelector;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Aggregate<TSource, TAccumulate, TResult>._ obj = new Aggregate<TSource, TAccumulate, TResult>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
    }

    private class _ : Sink<TResult>, IObserver<TSource>
    {
      private readonly Aggregate<TSource, TAccumulate, TResult> _parent;
      private TAccumulate _accumulation;

      public _(
        Aggregate<TSource, TAccumulate, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._accumulation = this._parent._seed;
      }

      public void OnNext(TSource value)
      {
        try
        {
          this._accumulation = this._parent._accumulator(this._accumulation, value);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
        }
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        TResult result1 = default (TResult);
        TResult result2;
        try
        {
          result2 = this._parent._resultSelector(this._accumulation);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        this._observer.OnNext(result2);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
