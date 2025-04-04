// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.ToLookup`3
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class ToLookup<TSource, TKey, TElement> : Producer<ILookup<TKey, TElement>>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, TKey> _keySelector;
    private readonly Func<TSource, TElement> _elementSelector;
    private readonly IEqualityComparer<TKey> _comparer;

    public ToLookup(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      IEqualityComparer<TKey> comparer)
    {
      this._source = source;
      this._keySelector = keySelector;
      this._elementSelector = elementSelector;
      this._comparer = comparer;
    }

    protected override IDisposable Run(
      IObserver<ILookup<TKey, TElement>> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      ToLookup<TSource, TKey, TElement>._ obj = new ToLookup<TSource, TKey, TElement>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
    }

    private class _ : Sink<ILookup<TKey, TElement>>, IObserver<TSource>
    {
      private readonly ToLookup<TSource, TKey, TElement> _parent;
      private System.Reactive.Lookup<TKey, TElement> _lookup;

      public _(
        ToLookup<TSource, TKey, TElement> parent,
        IObserver<ILookup<TKey, TElement>> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._lookup = new System.Reactive.Lookup<TKey, TElement>(this._parent._comparer);
      }

      public void OnNext(TSource value)
      {
        try
        {
          this._lookup.Add(this._parent._keySelector(value), this._parent._elementSelector(value));
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
        this._observer.OnNext((ILookup<TKey, TElement>) this._lookup);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
