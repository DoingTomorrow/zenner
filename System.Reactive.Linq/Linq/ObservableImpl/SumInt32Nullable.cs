// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.SumInt32Nullable
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class SumInt32Nullable : Producer<int?>
  {
    private readonly IObservable<int?> _source;

    public SumInt32Nullable(IObservable<int?> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<int?> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      SumInt32Nullable._ obj = new SumInt32Nullable._(observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<int?>((IObserver<int?>) obj);
    }

    private class _ : Sink<int?>, IObserver<int?>
    {
      private int _sum;

      public _(IObserver<int?> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._sum = 0;
      }

      public void OnNext(int? value)
      {
        try
        {
          if (!value.HasValue)
            return;
          checked { this._sum += value.Value; }
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
        this._observer.OnNext(new int?(this._sum));
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
