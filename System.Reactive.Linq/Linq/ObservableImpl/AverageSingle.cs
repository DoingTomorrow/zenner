// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.AverageSingle
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class AverageSingle : Producer<float>
  {
    private readonly IObservable<float> _source;

    public AverageSingle(IObservable<float> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<float> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      AverageSingle._ obj = new AverageSingle._(observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<float>((IObserver<float>) obj);
    }

    private class _ : Sink<float>, IObserver<float>
    {
      private double _sum;
      private long _count;

      public _(IObserver<float> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._sum = 0.0;
        this._count = 0L;
      }

      public void OnNext(float value)
      {
        try
        {
          this._sum += (double) value;
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
          this._observer.OnNext((float) this._sum / (float) this._count);
          this._observer.OnCompleted();
        }
        else
          this._observer.OnError((Exception) new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
        this.Dispose();
      }
    }
  }
}
