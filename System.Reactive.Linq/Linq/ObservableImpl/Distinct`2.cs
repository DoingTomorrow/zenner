// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Distinct`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Distinct<TSource, TKey> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, TKey> _keySelector;
    private readonly IEqualityComparer<TKey> _comparer;

    public Distinct(
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
      Distinct<TSource, TKey>._ obj = new Distinct<TSource, TKey>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Distinct<TSource, TKey> _parent;
      private HashSet<TKey> _hashSet;

      public _(Distinct<TSource, TKey> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._hashSet = new HashSet<TKey>(this._parent._comparer);
      }

      public void OnNext(TSource value)
      {
        TKey key = default (TKey);
        bool flag;
        try
        {
          flag = this._hashSet.Add(this._parent._keySelector(value));
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        if (!flag)
          return;
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
