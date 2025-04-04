// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.MaxInt64Nullable
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class MaxInt64Nullable : Producer<long?>
  {
    private readonly IObservable<long?> _source;

    public MaxInt64Nullable(IObservable<long?> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<long?> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      MaxInt64Nullable._ obj = new MaxInt64Nullable._(observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<long?>((IObserver<long?>) obj);
    }

    private class _ : Sink<long?>, IObserver<long?>
    {
      private long? _lastValue;

      public _(IObserver<long?> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._lastValue = new long?();
      }

      public void OnNext(long? value)
      {
        if (!value.HasValue)
          return;
        if (this._lastValue.HasValue)
        {
          long? nullable = value;
          long? lastValue = this._lastValue;
          if ((nullable.GetValueOrDefault() > lastValue.GetValueOrDefault() ? (nullable.HasValue & lastValue.HasValue ? 1 : 0) : 0) == 0)
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
