// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.ToDictionary`3
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class ToDictionary<TSource, TKey, TElement> : Producer<IDictionary<TKey, TElement>>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, TKey> _keySelector;
    private readonly Func<TSource, TElement> _elementSelector;
    private readonly IEqualityComparer<TKey> _comparer;

    public ToDictionary(
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
      IObserver<IDictionary<TKey, TElement>> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      ToDictionary<TSource, TKey, TElement>._ obj = new ToDictionary<TSource, TKey, TElement>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
    }

    private class _ : Sink<IDictionary<TKey, TElement>>, IObserver<TSource>
    {
      private readonly ToDictionary<TSource, TKey, TElement> _parent;
      private Dictionary<TKey, TElement> _dictionary;

      public _(
        ToDictionary<TSource, TKey, TElement> parent,
        IObserver<IDictionary<TKey, TElement>> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._dictionary = new Dictionary<TKey, TElement>(this._parent._comparer);
      }

      public void OnNext(TSource value)
      {
        try
        {
          this._dictionary.Add(this._parent._keySelector(value), this._parent._elementSelector(value));
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
        this._observer.OnNext((IDictionary<TKey, TElement>) this._dictionary);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
