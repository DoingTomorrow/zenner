// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.SumSingleNullable
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class SumSingleNullable : Producer<float?>
  {
    private readonly IObservable<float?> _source;

    public SumSingleNullable(IObservable<float?> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<float?> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      SumSingleNullable._ obj = new SumSingleNullable._(observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<float?>((IObserver<float?>) obj);
    }

    private class _ : Sink<float?>, IObserver<float?>
    {
      private double _sum;

      public _(IObserver<float?> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._sum = 0.0;
      }

      public void OnNext(float? value)
      {
        if (!value.HasValue)
          return;
        this._sum += (double) value.Value;
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnNext(new float?((float) this._sum));
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
