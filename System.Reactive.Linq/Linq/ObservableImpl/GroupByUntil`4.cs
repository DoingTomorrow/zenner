// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.GroupByUntil`4
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class GroupByUntil<TSource, TKey, TElement, TDuration> : 
    Producer<IGroupedObservable<TKey, TElement>>
  {
    private readonly IObservable<TSource> _source;
    private readonly Func<TSource, TKey> _keySelector;
    private readonly Func<TSource, TElement> _elementSelector;
    private readonly Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> _durationSelector;
    private readonly int? _capacity;
    private readonly IEqualityComparer<TKey> _comparer;
    private CompositeDisposable _groupDisposable;
    private RefCountDisposable _refCountDisposable;

    public GroupByUntil(
      IObservable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector,
      int? capacity,
      IEqualityComparer<TKey> comparer)
    {
      this._source = source;
      this._keySelector = keySelector;
      this._elementSelector = elementSelector;
      this._durationSelector = durationSelector;
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
      GroupByUntil<TSource, TKey, TElement, TDuration>._ obj = new GroupByUntil<TSource, TKey, TElement, TDuration>._(this, observer, cancel);
      setSink((IDisposable) obj);
      this._groupDisposable.Add(this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj));
      return (IDisposable) this._refCountDisposable;
    }

    private class _ : Sink<IGroupedObservable<TKey, TElement>>, IObserver<TSource>
    {
      private readonly GroupByUntil<TSource, TKey, TElement, TDuration> _parent;
      private readonly Map<TKey, ISubject<TElement>> _map;
      private ISubject<TElement> _null;
      private object _nullGate;

      public _(
        GroupByUntil<TSource, TKey, TElement, TDuration> parent,
        IObserver<IGroupedObservable<TKey, TElement>> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._map = new Map<TKey, ISubject<TElement>>(this._parent._capacity, this._parent._comparer);
        this._nullGate = new object();
      }

      private ISubject<TElement> NewSubject()
      {
        Subject<TElement> subject = new Subject<TElement>();
        return Subject.Create<TElement>((IObserver<TElement>) new AsyncLockObserver<TElement>((IObserver<TElement>) subject, new AsyncLock()), (IObservable<TElement>) subject);
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
        bool added = false;
        ISubject<TElement> subject = (ISubject<TElement>) null;
        try
        {
          if ((object) key2 == null)
          {
            lock (this._nullGate)
            {
              if (this._null == null)
              {
                this._null = this.NewSubject();
                added = true;
              }
              subject = this._null;
            }
          }
          else
            subject = this._map.GetOrAdd(key2, new Func<ISubject<TElement>>(this.NewSubject), out added);
        }
        catch (Exception ex)
        {
          this.Error(ex);
          return;
        }
        if (added)
        {
          GroupedObservable<TKey, TElement> groupedObservable1 = new GroupedObservable<TKey, TElement>(key2, subject, this._parent._refCountDisposable);
          GroupedObservable<TKey, TElement> groupedObservable2 = new GroupedObservable<TKey, TElement>(key2, subject);
          IObservable<TDuration> source;
          try
          {
            source = this._parent._durationSelector((IGroupedObservable<TKey, TElement>) groupedObservable2);
          }
          catch (Exception ex)
          {
            this.Error(ex);
            return;
          }
          lock (this._observer)
            this._observer.OnNext((IGroupedObservable<TKey, TElement>) groupedObservable1);
          SingleAssignmentDisposable self = new SingleAssignmentDisposable();
          this._parent._groupDisposable.Add((IDisposable) self);
          self.Disposable = source.SubscribeSafe<TDuration>((IObserver<TDuration>) new GroupByUntil<TSource, TKey, TElement, TDuration>._.Delta(this, key2, subject, (IDisposable) self));
        }
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
        ISubject<TElement> subject = (ISubject<TElement>) null;
        lock (this._nullGate)
          subject = this._null;
        subject?.OnCompleted();
        foreach (IObserver<TElement> observer in this._map.Values)
          observer.OnCompleted();
        lock (this._observer)
          this._observer.OnCompleted();
        this.Dispose();
      }

      private void Error(Exception exception)
      {
        ISubject<TElement> subject = (ISubject<TElement>) null;
        lock (this._nullGate)
          subject = this._null;
        subject?.OnError(exception);
        foreach (IObserver<TElement> observer in this._map.Values)
          observer.OnError(exception);
        lock (this._observer)
          this._observer.OnError(exception);
        this.Dispose();
      }

      private class Delta : IObserver<TDuration>
      {
        private readonly GroupByUntil<TSource, TKey, TElement, TDuration>._ _parent;
        private readonly TKey _key;
        private readonly ISubject<TElement> _writer;
        private readonly IDisposable _self;

        public Delta(
          GroupByUntil<TSource, TKey, TElement, TDuration>._ parent,
          TKey key,
          ISubject<TElement> writer,
          IDisposable self)
        {
          this._parent = parent;
          this._key = key;
          this._writer = writer;
          this._self = self;
        }

        public void OnNext(TDuration value) => this.OnCompleted();

        public void OnError(Exception error)
        {
          this._parent.Error(error);
          this._self.Dispose();
        }

        public void OnCompleted()
        {
          if ((object) this._key == null)
          {
            ISubject<TElement> subject = (ISubject<TElement>) null;
            lock (this._parent._nullGate)
            {
              subject = this._parent._null;
              this._parent._null = (ISubject<TElement>) null;
            }
            subject.OnCompleted();
          }
          else if (this._parent._map.Remove(this._key))
            this._writer.OnCompleted();
          this._parent._parent._groupDisposable.Remove(this._self);
        }
      }
    }
  }
}
