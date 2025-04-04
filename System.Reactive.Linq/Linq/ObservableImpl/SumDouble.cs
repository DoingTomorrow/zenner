// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.SumDouble
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class SumDouble : Producer<double>
  {
    private readonly IObservable<double> _source;

    public SumDouble(IObservable<double> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<double> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      SumDouble._ obj = new SumDouble._(observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<double>((IObserver<double>) obj);
    }

    private class _ : Sink<double>, IObserver<double>
    {
      private double _sum;

      public _(IObserver<double> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._sum = 0.0;
      }

      public void OnNext(double value) => this._sum += value;

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnNext(this._sum);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
