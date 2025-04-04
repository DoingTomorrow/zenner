// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Zip`12
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections;
using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> : Producer<TResult>
  {
    private readonly IObservable<T1> _source1;
    private readonly IObservable<T2> _source2;
    private readonly IObservable<T3> _source3;
    private readonly IObservable<T4> _source4;
    private readonly IObservable<T5> _source5;
    private readonly IObservable<T6> _source6;
    private readonly IObservable<T7> _source7;
    private readonly IObservable<T8> _source8;
    private readonly IObservable<T9> _source9;
    private readonly IObservable<T10> _source10;
    private readonly IObservable<T11> _source11;
    private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> _resultSelector;

    public Zip(
      IObservable<T1> source1,
      IObservable<T2> source2,
      IObservable<T3> source3,
      IObservable<T4> source4,
      IObservable<T5> source5,
      IObservable<T6> source6,
      IObservable<T7> source7,
      IObservable<T8> source8,
      IObservable<T9> source9,
      IObservable<T10> source10,
      IObservable<T11> source11,
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> resultSelector)
    {
      this._source1 = source1;
      this._source2 = source2;
      this._source3 = source3;
      this._source4 = source4;
      this._source5 = source5;
      this._source6 = source6;
      this._source7 = source7;
      this._source8 = source8;
      this._source9 = source9;
      this._source10 = source10;
      this._source11 = source11;
      this._resultSelector = resultSelector;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._ obj = new Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : ZipSink<TResult>
    {
      private readonly Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> _parent;
      private ZipObserver<T1> _observer1;
      private ZipObserver<T2> _observer2;
      private ZipObserver<T3> _observer3;
      private ZipObserver<T4> _observer4;
      private ZipObserver<T5> _observer5;
      private ZipObserver<T6> _observer6;
      private ZipObserver<T7> _observer7;
      private ZipObserver<T8> _observer8;
      private ZipObserver<T9> _observer9;
      private ZipObserver<T10> _observer10;
      private ZipObserver<T11> _observer11;

      public _(
        Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(11, observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        IDisposable[] disposableArray = new IDisposable[12];
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
        SingleAssignmentDisposable self4 = new SingleAssignmentDisposable();
        disposableArray[3] = (IDisposable) self4;
        this._observer4 = new ZipObserver<T4>(this._gate, (IZip) this, 3, (IDisposable) self4);
        this.Queues[3] = (ICollection) this._observer4.Values;
        SingleAssignmentDisposable self5 = new SingleAssignmentDisposable();
        disposableArray[4] = (IDisposable) self5;
        this._observer5 = new ZipObserver<T5>(this._gate, (IZip) this, 4, (IDisposable) self5);
        this.Queues[4] = (ICollection) this._observer5.Values;
        SingleAssignmentDisposable self6 = new SingleAssignmentDisposable();
        disposableArray[5] = (IDisposable) self6;
        this._observer6 = new ZipObserver<T6>(this._gate, (IZip) this, 5, (IDisposable) self6);
        this.Queues[5] = (ICollection) this._observer6.Values;
        SingleAssignmentDisposable self7 = new SingleAssignmentDisposable();
        disposableArray[6] = (IDisposable) self7;
        this._observer7 = new ZipObserver<T7>(this._gate, (IZip) this, 6, (IDisposable) self7);
        this.Queues[6] = (ICollection) this._observer7.Values;
        SingleAssignmentDisposable self8 = new SingleAssignmentDisposable();
        disposableArray[7] = (IDisposable) self8;
        this._observer8 = new ZipObserver<T8>(this._gate, (IZip) this, 7, (IDisposable) self8);
        this.Queues[7] = (ICollection) this._observer8.Values;
        SingleAssignmentDisposable self9 = new SingleAssignmentDisposable();
        disposableArray[8] = (IDisposable) self9;
        this._observer9 = new ZipObserver<T9>(this._gate, (IZip) this, 8, (IDisposable) self9);
        this.Queues[8] = (ICollection) this._observer9.Values;
        SingleAssignmentDisposable self10 = new SingleAssignmentDisposable();
        disposableArray[9] = (IDisposable) self10;
        this._observer10 = new ZipObserver<T10>(this._gate, (IZip) this, 9, (IDisposable) self10);
        this.Queues[9] = (ICollection) this._observer10.Values;
        SingleAssignmentDisposable self11 = new SingleAssignmentDisposable();
        disposableArray[10] = (IDisposable) self11;
        this._observer11 = new ZipObserver<T11>(this._gate, (IZip) this, 10, (IDisposable) self11);
        this.Queues[10] = (ICollection) this._observer11.Values;
        self1.Disposable = this._parent._source1.SubscribeSafe<T1>((IObserver<T1>) this._observer1);
        self2.Disposable = this._parent._source2.SubscribeSafe<T2>((IObserver<T2>) this._observer2);
        self3.Disposable = this._parent._source3.SubscribeSafe<T3>((IObserver<T3>) this._observer3);
        self4.Disposable = this._parent._source4.SubscribeSafe<T4>((IObserver<T4>) this._observer4);
        self5.Disposable = this._parent._source5.SubscribeSafe<T5>((IObserver<T5>) this._observer5);
        self6.Disposable = this._parent._source6.SubscribeSafe<T6>((IObserver<T6>) this._observer6);
        self7.Disposable = this._parent._source7.SubscribeSafe<T7>((IObserver<T7>) this._observer7);
        self8.Disposable = this._parent._source8.SubscribeSafe<T8>((IObserver<T8>) this._observer8);
        self9.Disposable = this._parent._source9.SubscribeSafe<T9>((IObserver<T9>) this._observer9);
        self10.Disposable = this._parent._source10.SubscribeSafe<T10>((IObserver<T10>) this._observer10);
        self11.Disposable = this._parent._source11.SubscribeSafe<T11>((IObserver<T11>) this._observer11);
        disposableArray[11] = Disposable.Create((Action) (() =>
        {
          this._observer1.Values.Clear();
          this._observer2.Values.Clear();
          this._observer3.Values.Clear();
          this._observer4.Values.Clear();
          this._observer5.Values.Clear();
          this._observer6.Values.Clear();
          this._observer7.Values.Clear();
          this._observer8.Values.Clear();
          this._observer9.Values.Clear();
          this._observer10.Values.Clear();
          this._observer11.Values.Clear();
        }));
        return (IDisposable) StableCompositeDisposable.Create(disposableArray);
      }

      protected override TResult GetResult()
      {
        return this._parent._resultSelector(this._observer1.Values.Dequeue(), this._observer2.Values.Dequeue(), this._observer3.Values.Dequeue(), this._observer4.Values.Dequeue(), this._observer5.Values.Dequeue(), this._observer6.Values.Dequeue(), this._observer7.Values.Dequeue(), this._observer8.Values.Dequeue(), this._observer9.Values.Dequeue(), this._observer10.Values.Dequeue(), this._observer11.Values.Dequeue());
      }
    }
  }
}
