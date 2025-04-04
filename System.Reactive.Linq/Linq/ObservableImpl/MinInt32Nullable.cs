// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.MinInt32Nullable
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class MinInt32Nullable : Producer<int?>
  {
    private readonly IObservable<int?> _source;

    public MinInt32Nullable(IObservable<int?> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<int?> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      MinInt32Nullable._ obj = new MinInt32Nullable._(observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<int?>((IObserver<int?>) obj);
    }

    private class _ : Sink<int?>, IObserver<int?>
    {
      private int? _lastValue;

      public _(IObserver<int?> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._lastValue = new int?();
      }

      public void OnNext(int? value)
      {
        if (!value.HasValue)
          return;
        if (this._lastValue.HasValue)
        {
          int? nullable = value;
          int? lastValue = this._lastValue;
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
