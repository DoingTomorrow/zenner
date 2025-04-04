// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.GroupBy`3
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class GroupBy<TSource, TKey, TElement> : Producer<IGroupedObservable<TKey, TElement>>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, TKey> _keySelector;
    private readonly Func<TSource, TElement> _elementSelector;
    private readonly int? _capacity;
    private readonly IEqualityComparer<TKey> _comparer;
    private CompositeDisposable _groupDisposable;
    private RefCountDisposable _refCountDisposable;

    public GroupBy(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      int? capacity,
      IEqualityComparer<TKey> comparer)
    {
      this._source = source;
      this._keySelector = keySelector;
      this._elementSelector = elementSelector;
      this._capacity = capacity;
      this._comparer = comparer;
    }

    protected override IDisposable Run(
      IObserver<IGroupedObservable<TKey, TElement>> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      this._groupDisposable = new CompositeDisposable();
      this._refCountDisposable = new RefCountDisposable((IDisposable) this._groupDisposable);
      GroupBy<TSource, TKey, TElement>._ obj = new GroupBy<TSource, TKey, TElement>._(this, observer, cancel);
      setSink((IDisposable) obj);
      this._groupDisposable.Add(this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj));
      return (IDisposable) this._refCountDisposable;
    }

    private class _ : Sink<IGroupedObservable<TKey, TElement>>, IObserver<TSource>
    {
      private readonly GroupBy<TSource, TKey, TElement> _parent;
      private readonly Dictionary<TKey, ISubject<TElement>> _map;
      private ISubject<TElement> _null;

      public _(
        GroupBy<TSource, TKey, TElement> parent,
        IObserver<IGroupedObservable<TKey, TElement>> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        if (this._parent._capacity.HasValue)
          this._map = new Dictionary<TKey, ISubject<TElement>>(this._parent._capacity.Value, this._parent._comparer);
        else
          this._map = new Dictionary<TKey, ISubject<TElement>>(this._parent._comparer);
      }

      public void OnNext(TSource value)
      {
        TKey key1 = default (TKey);
        TKey key2;
        try
        {
          key2 = this._parent._keySelector(value);
        }
        catch (Exception ex)
        {
          this.Error(ex);
          return;
        }
        bool flag = false;
        ISubject<TElement> subject = (ISubject<TElement>) null;
        try
        {
          if ((object) key2 == null)
          {
            if (this._null == null)
            {
              this._null = (ISubject<TElement>) new Subject<TElement>();
              flag = true;
            }
            subject = this._null;
          }
          else if (!this._map.TryGetValue(key2, out subject))
          {
            subject = (ISubject<TElement>) new Subject<TElement>();
            this._map.Add(key2, subject);
            flag = true;
          }
        }
        catch (Exception ex)
        {
          this.Error(ex);
          return;
        }
        if (flag)
          this._observer.OnNext((IGroupedObservable<TKey, TElement>) new GroupedObservable<TKey, TElement>(key2, subject, this._parent._refCountDisposable));
        TElement element1 = default (TElement);
        TElement element2;
        try
        {
          element2 = this._parent._elementSelector(value);
        }
        catch (Exception ex)
        {
          this.Error(ex);
          return;
        }
        subject.OnNext(element2);
      }

      public void OnError(Exception error) => this.Error(error);

      public void OnCompleted()
      {
        if (this._null != null)
          this._null.OnCompleted();
        foreach (IObserver<TElement> observer in this._map.Values)
          observer.OnCompleted();
        this._observer.OnCompleted();
        this.Dispose();
      }

      private void Error(Exception exception)
      {
        if (this._null != null)
          this._null.OnError(exception);
        foreach (IObserver<TElement> observer in this._map.Values)
          observer.OnError(exception);
        this._observer.OnError(exception);
        this.Dispose();
      }
    }
  }
}
