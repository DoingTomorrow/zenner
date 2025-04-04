// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.MaxDoubleNullable
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class MaxDoubleNullable : Producer<double?>
  {
    private readonly IObservable<double?> _source;

    public MaxDoubleNullable(IObservable<double?> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<double?> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      MaxDoubleNullable._ obj = new MaxDoubleNullable._(observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<double?>((IObserver<double?>) obj);
    }

    private class _ : Sink<double?>, IObserver<double?>
    {
      private double? _lastValue;

      public _(IObserver<double?> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._lastValue = new double?();
      }

      public void OnNext(double? value)
      {
        if (!value.HasValue)
          return;
        if (this._lastValue.HasValue)
        {
          double? nullable = value;
          double? lastValue = this._lastValue;
          if ((nullable.GetValueOrDefault() > lastValue.GetValueOrDefault() ? (nullable.HasValue & lastValue.HasValue ? 1 : 0) : 0) == 0 && !double.IsNaN(value.Value))
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
