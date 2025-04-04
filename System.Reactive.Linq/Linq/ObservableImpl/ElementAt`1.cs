// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.ElementAt`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class ElementAt<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly int _index;
    private readonly bool _throwOnEmpty;

    public ElementAt(IObservable<TSource> source, int index, bool throwOnEmpty)
    {
      this._source = source;
      this._index = index;
      this._throwOnEmpty = throwOnEmpty;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      ElementAt<TSource>._ obj = new ElementAt<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly ElementAt<TSource> _parent;
      private int _i;

      public _(ElementAt<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._i = this._parent._index;
      }

      public void OnNext(TSource value)
      {
        if (this._i == 0)
        {
          this._observer.OnNext(value);
          this._observer.OnCompleted();
          this.Dispose();
        }
        --this._i;
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        if (this._parent._throwOnEmpty)
        {
          this._observer.OnError((Exception) new ArgumentOutOfRangeException("index"));
        }
        else
        {
          this._observer.OnNext(default (TSource));
          this._observer.OnCompleted();
        }
        this.Dispose();
      }
    }
  }
}
