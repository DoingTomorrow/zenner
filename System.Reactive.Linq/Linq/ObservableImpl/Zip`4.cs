// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Zip`4
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections;
using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Zip<T1, T2, T3, TResult> : Producer<TResult>
  {
    private readonly IObservable<T1> _source1;
    private readonly IObservable<T2> _source2;
    private readonly IObservable<T3> _source3;
    private readonly Func<T1, T2, T3, TResult> _resultSelector;

    public Zip(
      IObservable<T1> source1,
      IObservable<T2> source2,
      IObservable<T3> source3,
      Func<T1, T2, T3, TResult> resultSelector)
    {
      this._source1 = source1;
      this._source2 = source2;
      this._source3 = source3;
      this._resultSelector = resultSelector;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Zip<T1, T2, T3, TResult>._ obj = new Zip<T1, T2, T3, TResult>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : ZipSink<TResult>
    {
      private readonly Zip<T1, T2, T3, TResult> _parent;
      private ZipObserver<T1> _observer1;
      private ZipObserver<T2> _observer2;
      private ZipObserver<T3> _observer3;

      public _(Zip<T1, T2, T3, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
        : base(3, observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        IDisposable[] disposableArray = new IDisposable[4];
        SingleAssignmentDisposable self1 = new SingleAssignmentDisposable();
        disposableArray[0] = (IDisposable) self1;
        this._observer1 = new ZipObserver<T1>(this._gate, (IZip) this, 0, (IDisposable) self1);
        this.Queues[0] = (ICollection) this._observer1.Values;
        SingleAssignmentDisposable self2 = new SingleAssignmentDisposable();
        disposableArray[1] = (IDisposable) self2;
        this._observer2 = new ZipObserver<T2>(this._gate, (IZip) this, 1, (IDisposable) self2);
        this.Queues[1] = (ICollection) this._observer2.Values;
        SingleAssignmentDisposable self3 = new SingleAssignmentDisposable();
        disposableArray[2] = (IDisposable) self3;
        this._observer3 = new ZipObserver<T3>(this._gate, (IZip) this, 2, (IDisposable) self3);
        this.Queues[2] = (ICollection) this._observer3.Values;
        self1.Disposable = this._parent._source1.SubscribeSafe<T1>((IObserver<T1>) this._observer1);
        self2.Disposable = this._parent._source2.SubscribeSafe<T2>((IObserver<T2>) this._observer2);
        self3.Disposable = this._parent._source3.SubscribeSafe<T3>((IObserver<T3>) this._observer3);
        disposableArray[3] = Disposable.Create((Action) (() =>
        {
          this._observer1.Values.Clear();
          this._observer2.Values.Clear();
          this._observer3.Values.Clear();
        }));
        return (IDisposable) StableCompositeDisposable.Create(disposableArray);
      }

      protected override TResult GetResult()
      {
        return this._parent._resultSelector(this._observer1.Values.Dequeue(), this._observer2.Values.Dequeue(), this._observer3.Values.Dequeue());
      }
    }
  }
}
