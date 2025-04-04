// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.ToList`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class ToList<TSource> : Producer<IList<TSource>>
  {
    private readonly IObservable<TSource> _source;

    public ToList(IObservable<TSource> source) => this._source = source;

    protected override IDisposable Run(
      IObserver<IList<TSource>> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      ToList<TSource>._ obj = new ToList<TSource>._(observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
    }

    private class _ : Sink<IList<TSource>>, IObserver<TSource>
    {
      private List<TSource> _list;

      public _(IObserver<IList<TSource>> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._list = new List<TSource>();
      }

      public void OnNext(TSource value) => this._list.Add(value);

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnNext((IList<TSource>) this._list);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
