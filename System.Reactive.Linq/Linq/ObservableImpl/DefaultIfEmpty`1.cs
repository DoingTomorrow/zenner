// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.DefaultIfEmpty`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class DefaultIfEmpty<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly TSource _defaultValue;

    public DefaultIfEmpty(IObservable<TSource> source, TSource defaultValue)
    {
      this._source = source;
      this._defaultValue = defaultValue;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      DefaultIfEmpty<TSource>._ obj = new DefaultIfEmpty<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly DefaultIfEmpty<TSource> _parent;
      private bool _found;

      public _(DefaultIfEmpty<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._found = false;
      }

      public void OnNext(TSource value)
      {
        this._found = true;
        this._observer.OnNext(value);
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        if (!this._found)
          this._observer.OnNext(this._parent._defaultValue);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
