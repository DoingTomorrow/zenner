// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.SumInt64
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class SumInt64 : Producer<long>
  {
    private readonly IObservable<long> _source;

    public SumInt64(IObservable<long> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<long> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      SumInt64._ obj = new SumInt64._(observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<long>((IObserver<long>) obj);
    }

    private class _ : Sink<long>, IObserver<long>
    {
      private long _sum;

      public _(IObserver<long> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._sum = 0L;
      }

      public void OnNext(long value)
      {
        try
        {
          checked { this._sum += value; }
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
        this._observer.OnNext(this._sum);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
