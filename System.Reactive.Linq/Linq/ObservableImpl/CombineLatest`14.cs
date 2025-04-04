// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.CombineLatest`14
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> : 
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
    private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> _resultSelector;

    public CombineLatest(
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
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> resultSelector)
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
      this._resultSelector = resultSelector;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._ obj = new CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : CombineLatestSink<TResult>
    {
      private readonly CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> _parent;
      private CombineLatestObserver<T1> _observer1;
      private CombineLatestObserver<T2> _observer2;
      private CombineLatestObserver<T3> _observer3;
      private CombineLatestObserver<T4> _observer4;
      private CombineLatestObserver<T5> _observer5;
      private CombineLatestObserver<T6> _observer6;
      private CombineLatestObserver<T7> _observer7;
      private CombineLatestObserver<T8> _observer8;
      private CombineLatestObserver<T9> _observer9;
      private CombineLatestObserver<T10> _observer10;
      private CombineLatestObserver<T11> _observer11;
      private CombineLatestObserver<T12> _observer12;
      private CombineLatestObserver<T13> _observer13;

      public _(
        CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(13, observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        SingleAssignmentDisposable[] assignmentDisposableArray = new SingleAssignmentDisposable[13];
        for (int index = 0; index < 13; ++index)
          assignmentDisposableArray[index] = new SingleAssignmentDisposable();
        this._observer1 = new CombineLatestObserver<T1>(this._gate, (ICombineLatest) this, 0, (IDisposable) assignmentDisposableArray[0]);
        this._observer2 = new CombineLatestObserver<T2>(this._gate, (ICombineLatest) this, 1, (IDisposable) assignmentDisposableArray[1]);
        this._observer3 = new CombineLatestObserver<T3>(this._gate, (ICombineLatest) this, 2, (IDisposable) assignmentDisposableArray[2]);
        this._observer4 = new CombineLatestObserver<T4>(this._gate, (ICombineLatest) this, 3, (IDisposable) assignmentDisposableArray[3]);
        this._observer5 = new CombineLatestObserver<T5>(this._gate, (ICombineLatest) this, 4, (IDisposable) assignmentDisposableArray[4]);
        this._observer6 = new CombineLatestObserver<T6>(this._gate, (ICombineLatest) this, 5, (IDisposable) assignmentDisposableArray[5]);
        this._observer7 = new CombineLatestObserver<T7>(this._gate, (ICombineLatest) this, 6, (IDisposable) assignmentDisposableArray[6]);
        this._observer8 = new CombineLatestObserver<T8>(this._gate, (ICombineLatest) this, 7, (IDisposable) assignmentDisposableArray[7]);
        this._observer9 = new CombineLatestObserver<T9>(this._gate, (ICombineLatest) this, 8, (IDisposable) assignmentDisposableArray[8]);
        this._observer10 = new CombineLatestObserver<T10>(this._gate, (ICombineLatest) this, 9, (IDisposable) assignmentDisposableArray[9]);
        this._observer11 = new CombineLatestObserver<T11>(this._gate, (ICombineLatest) this, 10, (IDisposable) assignmentDisposableArray[10]);
        this._observer12 = new CombineLatestObserver<T12>(this._gate, (ICombineLatest) this, 11, (IDisposable) assignmentDisposableArray[11]);
        this._observer13 = new CombineLatestObserver<T13>(this._gate, (ICombineLatest) this, 12, (IDisposable) assignmentDisposableArray[12]);
        assignmentDisposableArray[0].Disposable = this._parent._source1.SubscribeSafe<T1>((IObserver<T1>) this._observer1);
        assignmentDisposableArray[1].Disposable = this._parent._source2.SubscribeSafe<T2>((IObserver<T2>) this._observer2);
        assignmentDisposableArray[2].Disposable = this._parent._source3.SubscribeSafe<T3>((IObserver<T3>) this._observer3);
        assignmentDisposableArray[3].Disposable = this._parent._source4.SubscribeSafe<T4>((IObserver<T4>) this._observer4);
        assignmentDisposableArray[4].Disposable = this._parent._source5.SubscribeSafe<T5>((IObserver<T5>) this._observer5);
        assignmentDisposableArray[5].Disposable = this._parent._source6.SubscribeSafe<T6>((IObserver<T6>) this._observer6);
        assignmentDisposableArray[6].Disposable = this._parent._source7.SubscribeSafe<T7>((IObserver<T7>) this._observer7);
        assignmentDisposableArray[7].Disposable = this._parent._source8.SubscribeSafe<T8>((IObserver<T8>) this._observer8);
        assignmentDisposableArray[8].Disposable = this._parent._source9.SubscribeSafe<T9>((IObserver<T9>) this._observer9);
        assignmentDisposableArray[9].Disposable = this._parent._source10.SubscribeSafe<T10>((IObserver<T10>) this._observer10);
        assignmentDisposableArray[10].Disposable = this._parent._source11.SubscribeSafe<T11>((IObserver<T11>) this._observer11);
        assignmentDisposableArray[11].Disposable = this._parent._source12.SubscribeSafe<T12>((IObserver<T12>) this._observer12);
        assignmentDisposableArray[12].Disposable = this._parent._source13.SubscribeSafe<T13>((IObserver<T13>) this._observer13);
        return (IDisposable) StableCompositeDisposable.Create((IDisposable[]) assignmentDisposableArray);
      }

      protected override TResult GetResult()
      {
        return this._parent._resultSelector(this._observer1.Value, this._observer2.Value, this._observer3.Value, this._observer4.Value, this._observer5.Value, this._observer6.Value, this._observer7.Value, this._observer8.Value, this._observer9.Value, this._observer10.Value, this._observer11.Value, this._observer12.Value, this._observer13.Value);
      }
    }
  }
}
