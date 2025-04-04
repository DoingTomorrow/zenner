// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.AverageInt32
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class AverageInt32 : Producer<double>
  {
    private readonly IObservable<int> _source;

    public AverageInt32(IObservable<int> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<double> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      AverageInt32._ obj = new AverageInt32._(observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<int>((IObserver<int>) obj);
    }

    private class _ : Sink<double>, IObserver<int>
    {
      private long _sum;
      private long _count;

      public _(IObserver<double> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._sum = 0L;
        this._count = 0L;
      }

      public void OnNext(int value)
      {
        try
        {
          checked { this._sum += (long) value; }
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
          this._observer.OnNext((double) this._sum / (double) this._count);
          this._observer.OnCompleted();
        }
        else
          this._observer.OnError((Exception) new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
        this.Dispose();
      }
    }
  }
}
