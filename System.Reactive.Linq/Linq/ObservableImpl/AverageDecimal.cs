// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.AverageDecimal
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class AverageDecimal : Producer<Decimal>
  {
    private readonly IObservable<Decimal> _source;

    public AverageDecimal(IObservable<Decimal> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<Decimal> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      AverageDecimal._ obj = new AverageDecimal._(observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<Decimal>((IObserver<Decimal>) obj);
    }

    private class _ : Sink<Decimal>, IObserver<Decimal>
    {
      private Decimal _sum;
      private long _count;

      public _(IObserver<Decimal> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._sum = 0M;
        this._count = 0L;
      }

      public void OnNext(Decimal value)
      {
        try
        {
          this._sum += value;
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
        {
          this._observer.OnNext(this._sum / (Decimal) this._count);
          this._observer.OnCompleted();
        }
        else
          this._observer.OnError((Exception) new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
        this.Dispose();
      }
    }
  }
}
