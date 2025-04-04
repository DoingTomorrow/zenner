// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.SumDecimal
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class SumDecimal : Producer<Decimal>
  {
    private readonly IObservable<Decimal> _source;

    public SumDecimal(IObservable<Decimal> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<Decimal> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      SumDecimal._ obj = new SumDecimal._(observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<Decimal>((IObserver<Decimal>) obj);
    }

    private class _ : Sink<Decimal>, IObserver<Decimal>
    {
      private Decimal _sum;

      public _(IObserver<Decimal> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._sum = 0M;
      }

      public void OnNext(Decimal value) => this._sum += value;

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
