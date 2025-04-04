// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.MinSingleNullable
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class MinSingleNullable : Producer<float?>
  {
    private readonly IObservable<float?> _source;

    public MinSingleNullable(IObservable<float?> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<float?> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      MinSingleNullable._ obj = new MinSingleNullable._(observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<float?>((IObserver<float?>) obj);
    }

    private class _ : Sink<float?>, IObserver<float?>
    {
      private float? _lastValue;

      public _(IObserver<float?> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._lastValue = new float?();
      }

      public void OnNext(float? value)
      {
        if (!value.HasValue)
          return;
        if (this._lastValue.HasValue)
        {
          float? nullable = value;
          float? lastValue = this._lastValue;
          if (((double) nullable.GetValueOrDefault() < (double) lastValue.GetValueOrDefault() ? (nullable.HasValue & lastValue.HasValue ? 1 : 0) : 0) == 0 && !float.IsNaN(value.Value))
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
