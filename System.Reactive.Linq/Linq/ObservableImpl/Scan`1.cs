// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Scan`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Scan<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, TSource, TSource> _accumulator;

    public Scan(IObservable<TSource> source, Func<TSource, TSource, TSource> accumulator)
    {
      this._source = source;
      this._accumulator = accumulator;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Scan<TSource>._ obj = new Scan<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Scan<TSource> _parent;
      private TSource _accumulation;
      private bool _hasAccumulation;

      public _(Scan<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._accumulation = default (TSource);
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
            this._accumulation = value;
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
