// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.MinInt32
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class MinInt32 : Producer<int>
  {
    private readonly IObservable<int> _source;

    public MinInt32(IObservable<int> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<int> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      MinInt32._ obj = new MinInt32._(observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<int>((IObserver<int>) obj);
    }

    private class _ : Sink<int>, IObserver<int>
    {
      private bool _hasValue;
      private int _lastValue;

      public _(IObserver<int> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._hasValue = false;
        this._lastValue = 0;
      }

      public void OnNext(int value)
      {
        if (this._hasValue)
        {
          if (value >= this._lastValue)
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
