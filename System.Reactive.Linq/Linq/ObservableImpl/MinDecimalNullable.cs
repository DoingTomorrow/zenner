// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.MinDecimalNullable
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class MinDecimalNullable : Producer<Decimal?>
  {
    private readonly IObservable<Decimal?> _source;

    public MinDecimalNullable(IObservable<Decimal?> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<Decimal?> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      MinDecimalNullable._ obj = new MinDecimalNullable._(observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<Decimal?>((IObserver<Decimal?>) obj);
    }

    private class _ : Sink<Decimal?>, IObserver<Decimal?>
    {
      private Decimal? _lastValue;

      public _(IObserver<Decimal?> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._lastValue = new Decimal?();
      }

      public void OnNext(Decimal? value)
      {
        if (!value.HasValue)
          return;
        if (this._lastValue.HasValue)
        {
          Decimal? nullable = value;
          Decimal? lastValue = this._lastValue;
          if ((nullable.GetValueOrDefault() < lastValue.GetValueOrDefault() ? (nullable.HasValue & lastValue.HasValue ? 1 : 0) : 0) == 0)
            return;
          this._lastValue = value;
        }
        else
          this._lastValue = value;
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnNext(this._lastValue);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
