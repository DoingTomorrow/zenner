// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Zip`17
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections;
using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> : 
    Producer<TResult>
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
    private readonly IObservable<T12> _source12;
    private readonly IObservable<T13> _source13;
    private readonly IObservable<T14> _source14;
    private readonly IObservable<T15> _source15;
    private readonly IObservable<T16> _source16;
    private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> _resultSelector;

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
      IObservable<T12> source12,
      IObservable<T13> source13,
      IObservable<T14> source14,
      IObservable<T15> source15,
      IObservable<T16> source16,
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> resultSelector)
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
      this._source12 = source12;
      this._source13 = source13;
      this._source14 = source14;
      this._source15 = source15;
      this._source16 = source16;
      this._resultSelector = resultSelector;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>._ obj = new Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : ZipSink<TResult>
    {
      private readonly Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> _parent;
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
      private ZipObserver<T12> _observer12;
      private ZipObserver<T13> _observer13;
      private ZipObserver<T14> _observer14;
      private ZipObserver<T15> _observer15;
      private ZipObserver<T16> _observer16;

      public _(
        Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(16, observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        IDisposable[] disposableArray = new IDisposable[17];
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
        SingleAssignmentDisposable self12 = new SingleAssignmentDisposable();
        disposableArray[11] = (IDisposable) self12;
        this._observer12 = new ZipObserver<T12>(this._gate, (IZip) this, 11, (IDisposable) self12);
        this.Queues[11] = (ICollection) this._observer12.Values;
        SingleAssignmentDisposable self13 = new SingleAssignmentDisposable();
        disposableArray[12] = (IDisposable) self13;
        this._observer13 = new ZipObserver<T13>(this._gate, (IZip) this, 12, (IDisposable) self13);
        this.Queues[12] = (ICollection) this._observer13.Values;
        SingleAssignmentDisposable self14 = new SingleAssignmentDisposable();
        disposableArray[13] = (IDisposable) self14;
        this._observer14 = new ZipObserver<T14>(this._gate, (IZip) this, 13, (IDisposable) self14);
        this.Queues[13] = (ICollection) this._observer14.Values;
        SingleAssignmentDisposable self15 = new SingleAssignmentDisposable();
        disposableArray[14] = (IDisposable) self15;
        this._observer15 = new ZipObserver<T15>(this._gate, (IZip) this, 14, (IDisposable) self15);
        this.Queues[14] = (ICollection) this._observer15.Values;
        SingleAssignmentDisposable self16 = new SingleAssignmentDisposable();
        disposableArray[15] = (IDisposable) self16;
        this._observer16 = new ZipObserver<T16>(this._gate, (IZip) this, 15, (IDisposable) self16);
        this.Queues[15] = (ICollection) this._observer16.Values;
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
        self12.Disposable = this._parent._source12.SubscribeSafe<T12>((IObserver<T12>) this._observer12);
        self13.Disposable = this._parent._source13.SubscribeSafe<T13>((IObserver<T13>) this._observer13);
        self14.Disposable = this._parent._source14.SubscribeSafe<T14>((IObserver<T14>) this._observer14);
        self15.Disposable = this._parent._source15.SubscribeSafe<T15>((IObserver<T15>) this._observer15);
        self16.Disposable = this._parent._source16.SubscribeSafe<T16>((IObserver<T16>) this._observer16);
        disposableArray[16] = Disposable.Create((Action) (() =>
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
          this._observer12.Values.Clear();
          this._observer13.Values.Clear();
          this._observer14.Values.Clear();
          this._observer15.Values.Clear();
          this._observer16.Values.Clear();
        }));
        return (IDisposable) StableCompositeDisposable.Create(disposableArray);
      }

      protected override TResult GetResult()
      {
        return this._parent._resultSelector(this._observer1.Values.Dequeue(), this._observer2.Values.Dequeue(), this._observer3.Values.Dequeue(), this._observer4.Values.Dequeue(), this._observer5.Values.Dequeue(), this._observer6.Values.Dequeue(), this._observer7.Values.Dequeue(), this._observer8.Values.Dequeue(), this._observer9.Values.Dequeue(), this._observer10.Values.Dequeue(), this._observer11.Values.Dequeue(), this._observer12.Values.Dequeue(), this._observer13.Values.Dequeue(), this._observer14.Values.Dequeue(), this._observer15.Values.Dequeue(), this._observer16.Values.Dequeue());
      }
    }
  }
}
