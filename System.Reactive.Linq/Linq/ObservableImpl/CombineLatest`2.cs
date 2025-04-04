// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.CombineLatest`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class CombineLatest<TSource, TResult> : Producer<TResult>
  {
    private readonly IEnumerable<IObservable<TSource>> _sources;
    private readonly Func<IList<TSource>, TResult> _resultSelector;

    public CombineLatest(
      IEnumerable<IObservable<TSource>> sources,
      Func<IList<TSource>, TResult> resultSelector)
    {
      this._sources = sources;
      this._resultSelector = resultSelector;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      CombineLatest<TSource, TResult>._ obj = new CombineLatest<TSource, TResult>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TResult>
    {
      private readonly CombineLatest<TSource, TResult> _parent;
      private object _gate;
      private bool[] _hasValue;
      private bool _hasValueAll;
      private List<TSource> _values;
      private bool[] _isDone;
      private IDisposable[] _subscriptions;

      public _(
        CombineLatest<TSource, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        IObservable<TSource>[] array = this._parent._sources.ToArray<IObservable<TSource>>();
        int length = array.Length;
        this._hasValue = new bool[length];
        this._hasValueAll = false;
        this._values = new List<TSource>(length);
        for (int index = 0; index < length; ++index)
          this._values.Add(default (TSource));
        this._isDone = new bool[length];
        this._subscriptions = new IDisposable[length];
        this._gate = new object();
        for (int index1 = 0; index1 < length; ++index1)
        {
          int index2 = index1;
          SingleAssignmentDisposable assignmentDisposable = new SingleAssignmentDisposable();
          this._subscriptions[index2] = (IDisposable) assignmentDisposable;
          CombineLatest<TSource, TResult>._.O o = new CombineLatest<TSource, TResult>._.O(this, index2);
          assignmentDisposable.Disposable = array[index2].SubscribeSafe<TSource>((IObserver<TSource>) o);
        }
        return (IDisposable) StableCompositeDisposable.Create(this._subscriptions);
      }

      private void OnNext(int index, TSource value)
      {
        lock (this._gate)
        {
          this._values[index] = value;
          this._hasValue[index] = true;
          if (this._hasValueAll || (this._hasValueAll = ((IEnumerable<bool>) this._hasValue).All<bool>(Stubs<bool>.I)))
          {
            TResult result1 = default (TResult);
            TResult result2;
            try
            {
              result2 = this._parent._resultSelector((IList<TSource>) new ReadOnlyCollection<TSource>((IList<TSource>) this._values));
            }
            catch (Exception ex)
            {
              this._observer.OnError(ex);
              this.Dispose();
              return;
            }
            this._observer.OnNext(result2);
          }
          else
          {
            if (!((IEnumerable<bool>) this._isDone).Where<bool>((Func<bool, int, bool>) ((x, i) => i != index)).All<bool>(Stubs<bool>.I))
              return;
            this._observer.OnCompleted();
            this.Dispose();
          }
        }
      }

      private void OnError(Exception error)
      {
        lock (this._gate)
        {
          this._observer.OnError(error);
          this.Dispose();
        }
      }

      private void OnCompleted(int index)
      {
        lock (this._gate)
        {
          this._isDone[index] = true;
          if (((IEnumerable<bool>) this._isDone).All<bool>(Stubs<bool>.I))
          {
            this._observer.OnCompleted();
            this.Dispose();
          }
          else
            this._subscriptions[index].Dispose();
        }
      }

      private class O : IObserver<TSource>
      {
        private readonly CombineLatest<TSource, TResult>._ _parent;
        private readonly int _index;

        public O(CombineLatest<TSource, TResult>._ parent, int index)
        {
          this._parent = parent;
          this._index = index;
        }

        public void OnNext(TSource value) => this._parent.OnNext(this._index, value);

        public void OnError(Exception error) => this._parent.OnError(error);

        public void OnCompleted() => this._parent.OnCompleted(this._index);
      }
    }
  }
}
