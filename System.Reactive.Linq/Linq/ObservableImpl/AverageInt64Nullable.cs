// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.AverageInt64Nullable
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class AverageInt64Nullable : Producer<double?>
  {
    private readonly IObservable<long?> _source;

    public AverageInt64Nullable(IObservable<long?> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<double?> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      AverageInt64Nullable._ obj = new AverageInt64Nullable._(observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<long?>((IObserver<long?>) obj);
    }

    private class _ : Sink<double?>, IObserver<long?>
    {
      private long _sum;
      private long _count;

      public _(IObserver<double?> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._sum = 0L;
        this._count = 0L;
      }

      public void OnNext(long? value)
      {
        try
        {
          if (!value.HasValue)
            return;
          checked { this._sum += value.Value; }
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
          this._observer.OnNext(new double?((double) this._sum / (double) this._count));
        else
          this._observer.OnNext(new double?());
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
