// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.MinBy`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class MinBy<TSource, TKey> : Producer<IList<TSource>>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, TKey> _keySelector;
    private readonly IComparer<TKey> _comparer;

    public MinBy(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IComparer<TKey> comparer)
    {
      this._source = source;
      this._keySelector = keySelector;
      this._comparer = comparer;
    }

    protected override IDisposable Run(
      IObserver<IList<TSource>> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      MinBy<TSource, TKey>._ obj = new MinBy<TSource, TKey>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
    }

    private class _ : Sink<IList<TSource>>, IObserver<TSource>
    {
      private readonly MinBy<TSource, TKey> _parent;
      private bool _hasValue;
      private TKey _lastKey;
      private List<TSource> _list;

      public _(MinBy<TSource, TKey> parent, IObserver<IList<TSource>> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._hasValue = false;
        this._lastKey = default (TKey);
        this._list = new List<TSource>();
      }

      public void OnNext(TSource value)
      {
        TKey key = default (TKey);
        TKey x;
        try
        {
          x = this._parent._keySelector(value);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        int num = 0;
        if (!this._hasValue)
        {
          this._hasValue = true;
          this._lastKey = x;
        }
        else
        {
          try
          {
            num = this._parent._comparer.Compare(x, this._lastKey);
          }
          catch (Exception ex)
          {
            this._observer.OnError(ex);
            this.Dispose();
            return;
          }
        }
        if (num < 0)
        {
          this._lastKey = x;
          this._list.Clear();
        }
        if (num > 0)
          return;
        this._list.Add(value);
      }

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
