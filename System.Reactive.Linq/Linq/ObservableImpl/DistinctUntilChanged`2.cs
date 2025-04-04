// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.DistinctUntilChanged`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class DistinctUntilChanged<TSource, TKey> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, TKey> _keySelector;
    private readonly IEqualityComparer<TKey> _comparer;

    public DistinctUntilChanged(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      this._source = source;
      this._keySelector = keySelector;
      this._comparer = comparer;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      DistinctUntilChanged<TSource, TKey>._ obj = new DistinctUntilChanged<TSource, TKey>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly DistinctUntilChanged<TSource, TKey> _parent;
      private TKey _currentKey;
      private bool _hasCurrentKey;

      public _(
        DistinctUntilChanged<TSource, TKey> parent,
        IObserver<TSource> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._currentKey = default (TKey);
        this._hasCurrentKey = false;
      }

      public void OnNext(TSource value)
      {
        TKey key = default (TKey);
        TKey y;
        try
        {
          y = this._parent._keySelector(value);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        bool flag = false;
        if (this._hasCurrentKey)
        {
          try
          {
            flag = this._parent._comparer.Equals(this._currentKey, y);
          }
          catch (Exception ex)
          {
            this._observer.OnError(ex);
            this.Dispose();
            return;
          }
        }
        if (this._hasCurrentKey && flag)
          return;
        this._hasCurrentKey = true;
        this._currentKey = y;
        this._observer.OnNext(value);
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
