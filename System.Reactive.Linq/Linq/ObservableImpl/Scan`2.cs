// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Scan`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Scan<TSource, TAccumulate> : Producer<TAccumulate>
  {
    private readonly IObservable<TSource> _source;
    private readonly TAccumulate _seed;
    private readonly Func<TAccumulate, TSource, TAccumulate> _accumulator;

    public Scan(
      IObservable<TSource> source,
      TAccumulate seed,
      Func<TAccumulate, TSource, TAccumulate> accumulator)
    {
      this._source = source;
      this._seed = seed;
      this._accumulator = accumulator;
    }

    protected override IDisposable Run(
      IObserver<TAccumulate> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Scan<TSource, TAccumulate>._ obj = new Scan<TSource, TAccumulate>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
    }

    private class _ : Sink<TAccumulate>, IObserver<TSource>
    {
      private readonly Scan<TSource, TAccumulate> _parent;
      private TAccumulate _accumulation;
      private bool _hasAccumulation;

      public _(
        Scan<TSource, TAccumulate> parent,
        IObserver<TAccumulate> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._accumulation = default (TAccumulate);
        this._hasAccumulation = false;
      }

      public void OnNext(TSource value)
      {
        try
        {
          if (this._hasAccumulation)
          {
            this._accumulation = this._parent._accumulator(this._accumulation, value);
          }
          else
          {
            this._accumulation = this._parent._accumulator(this._parent._seed, value);
            this._hasAccumulation = true;
          }
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        this._observer.OnNext(this._accumulation);
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
