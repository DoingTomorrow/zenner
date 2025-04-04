// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.CombineLatest`6
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class CombineLatest<T1, T2, T3, T4, T5, TResult> : Producer<TResult>
  {
    private readonly IObservable<T1> _source1;
    private readonly IObservable<T2> _source2;
    private readonly IObservable<T3> _source3;
    private readonly IObservable<T4> _source4;
    private readonly IObservable<T5> _source5;
    private readonly Func<T1, T2, T3, T4, T5, TResult> _resultSelector;

    public CombineLatest(
      IObservable<T1> source1,
      IObservable<T2> source2,
      IObservable<T3> source3,
      IObservable<T4> source4,
      IObservable<T5> source5,
      Func<T1, T2, T3, T4, T5, TResult> resultSelector)
    {
      this._source1 = source1;
      this._source2 = source2;
      this._source3 = source3;
      this._source4 = source4;
      this._source5 = source5;
      this._resultSelector = resultSelector;
    }

    protected override IDisposable Run(
      IObserver<TResult> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      CombineLatest<T1, T2, T3, T4, T5, TResult>._ obj = new CombineLatest<T1, T2, T3, T4, T5, TResult>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : CombineLatestSink<TResult>
    {
      private readonly CombineLatest<T1, T2, T3, T4, T5, TResult> _parent;
      private CombineLatestObserver<T1> _observer1;
      private CombineLatestObserver<T2> _observer2;
      private CombineLatestObserver<T3> _observer3;
      private CombineLatestObserver<T4> _observer4;
      private CombineLatestObserver<T5> _observer5;

      public _(
        CombineLatest<T1, T2, T3, T4, T5, TResult> parent,
        IObserver<TResult> observer,
        IDisposable cancel)
        : base(5, observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        SingleAssignmentDisposable[] assignmentDisposableArray = new SingleAssignmentDisposable[5];
        for (int index = 0; index < 5; ++index)
          assignmentDisposableArray[index] = new SingleAssignmentDisposable();
        this._observer1 = new CombineLatestObserver<T1>(this._gate, (ICombineLatest) this, 0, (IDisposable) assignmentDisposableArray[0]);
        this._observer2 = new CombineLatestObserver<T2>(this._gate, (ICombineLatest) this, 1, (IDisposable) assignmentDisposableArray[1]);
        this._observer3 = new CombineLatestObserver<T3>(this._gate, (ICombineLatest) this, 2, (IDisposable) assignmentDisposableArray[2]);
        this._observer4 = new CombineLatestObserver<T4>(this._gate, (ICombineLatest) this, 3, (IDisposable) assignmentDisposableArray[3]);
        this._observer5 = new CombineLatestObserver<T5>(this._gate, (ICombineLatest) this, 4, (IDisposable) assignmentDisposableArray[4]);
        assignmentDisposableArray[0].Disposable = this._parent._source1.SubscribeSafe<T1>((IObserver<T1>) this._observer1);
        assignmentDisposableArray[1].Disposable = this._parent._source2.SubscribeSafe<T2>((IObserver<T2>) this._observer2);
        assignmentDisposableArray[2].Disposable = this._parent._source3.SubscribeSafe<T3>((IObserver<T3>) this._observer3);
        assignmentDisposableArray[3].Disposable = this._parent._source4.SubscribeSafe<T4>((IObserver<T4>) this._observer4);
        assignmentDisposableArray[4].Disposable = this._parent._source5.SubscribeSafe<T5>((IObserver<T5>) this._observer5);
        return (IDisposable) StableCompositeDisposable.Create((IDisposable[]) assignmentDisposableArray);
      }

      protected override TResult GetResult()
      {
        return this._parent._resultSelector(this._observer1.Value, this._observer2.Value, this._observer3.Value, this._observer4.Value, this._observer5.Value);
      }
    }
  }
}
