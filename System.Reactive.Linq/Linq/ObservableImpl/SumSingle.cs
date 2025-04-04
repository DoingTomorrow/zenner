// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.SumSingle
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class SumSingle : Producer<float>
  {
    private readonly IObservable<float> _source;

    public SumSingle(IObservable<float> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<float> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      SumSingle._ obj = new SumSingle._(observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<float>((IObserver<float>) obj);
    }

    private class _ : Sink<float>, IObserver<float>
    {
      private double _sum;

      public _(IObserver<float> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._sum = 0.0;
      }

      public void OnNext(float value) => this._sum += (double) value;

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnNext((float) this._sum);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
