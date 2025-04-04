// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.AverageDecimalNullable
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class AverageDecimalNullable : Producer<Decimal?>
  {
    private readonly IObservable<Decimal?> _source;

    public AverageDecimalNullable(IObservable<Decimal?> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<Decimal?> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      AverageDecimalNullable._ obj = new AverageDecimalNullable._(observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<Decimal?>((IObserver<Decimal?>) obj);
    }

    private class _ : Sink<Decimal?>, IObserver<Decimal?>
    {
      private Decimal _sum;
      private long _count;

      public _(IObserver<Decimal?> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._sum = 0M;
        this._count = 0L;
      }

      public void OnNext(Decimal? value)
      {
        try
        {
          if (!value.HasValue)
            return;
          this._sum += value.Value;
          checked { ++this._count; }
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
        if (this._count > 0L)
          this._observer.OnNext(new Decimal?(this._sum / (Decimal) this._count));
        else
          this._observer.OnNext(new Decimal?());
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
