// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.MaxInt64
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class MaxInt64 : Producer<long>
  {
    private readonly IObservable<long> _source;

    public MaxInt64(IObservable<long> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<long> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      MaxInt64._ obj = new MaxInt64._(observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<long>((IObserver<long>) obj);
    }

    private class _ : Sink<long>, IObserver<long>
    {
      private bool _hasValue;
      private long _lastValue;

      public _(IObserver<long> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._hasValue = false;
        this._lastValue = 0L;
      }

      public void OnNext(long value)
      {
        if (this._hasValue)
        {
          if (value <= this._lastValue)
            return;
          this._lastValue = value;
        }
        else
        {
          this._lastValue = value;
          this._hasValue = true;
        }
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        if (!this._hasValue)
        {
          this._observer.OnError((Exception) new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
        }
        else
        {
          this._observer.OnNext(this._lastValue);
          this._observer.OnCompleted();
        }
        this.Dispose();
      }
    }
  }
}
