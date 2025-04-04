// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Aggregate`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Aggregate<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, TSource, TSource> _accumulator;

    public Aggregate(IObservable<TSource> source, Func<TSource, TSource, TSource> accumulator)
    {
      this._source = source;
      this._accumulator = accumulator;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Aggregate<TSource>._ obj = new Aggregate<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Aggregate<TSource> _parent;
      private TSource _accumulation;
      private bool _hasAccumulation;

      public _(Aggregate<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._accumulation = default (TSource);
        this._hasAccumulation = false;
      }

      public void OnNext(TSource value)
      {
        if (!this._hasAccumulation)
        {
          this._accumulation = value;
          this._hasAccumulation = true;
        }
        else
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
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        if (!this._hasAccumulation)
        {
          this._observer.OnError((Exception) new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
          this.Dispose();
        }
        else
        {
          this._observer.OnNext(this._accumulation);
          this._observer.OnCompleted();
          this.Dispose();
        }
      }
    }
  }
}
