// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Sample`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Sample<TSource, TSample> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly IObservable<TSample> _sampler;

    public Sample(IObservable<TSource> source, IObservable<TSample> sampler)
    {
      this._source = source;
      this._sampler = sampler;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Sample<TSource, TSample>._ obj = new Sample<TSource, TSample>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return obj.Run();
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Sample<TSource, TSample> _parent;
      private object _gate;
      private IDisposable _sourceSubscription;
      private bool _hasValue;
      private TSource _value;
      private bool _atEnd;

      public _(Sample<TSource, TSample> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._gate = new object();
        SingleAssignmentDisposable assignmentDisposable = new SingleAssignmentDisposable();
        this._sourceSubscription = (IDisposable) assignmentDisposable;
        assignmentDisposable.Disposable = this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this);
        return (IDisposable) StableCompositeDisposable.Create(this._sourceSubscription, this._parent._sampler.SubscribeSafe<TSample>((IObserver<TSample>) new Sample<TSource, TSample>._.SampleImpl(this)));
      }

      public void OnNext(TSource value)
      {
        lock (this._gate)
        {
          this._hasValue = true;
          this._value = value;
        }
      }

      public void OnError(Exception error)
      {
        lock (this._gate)
        {
          this._observer.OnError(error);
          this.Dispose();
        }
      }

      public void OnCompleted()
      {
        lock (this._gate)
        {
          this._atEnd = true;
          this._sourceSubscription.Dispose();
        }
      }

      private class SampleImpl : IObserver<TSample>
      {
        private readonly Sample<TSource, TSample>._ _parent;

        public SampleImpl(Sample<TSource, TSample>._ parent) => this._parent = parent;

        public void OnNext(TSample value)
        {
          lock (this._parent._gate)
          {
            if (this._parent._hasValue)
            {
              this._parent._hasValue = false;
              this._parent._observer.OnNext(this._parent._value);
            }
            if (!this._parent._atEnd)
              return;
            this._parent._observer.OnCompleted();
            this._parent.Dispose();
          }
        }

        public void OnError(Exception error)
        {
          lock (this._parent._gate)
          {
            this._parent._observer.OnError(error);
            this._parent.Dispose();
          }
        }

        public void OnCompleted()
        {
          lock (this._parent._gate)
          {
            if (this._parent._hasValue)
            {
              this._parent._hasValue = false;
              this._parent._observer.OnNext(this._parent._value);
            }
            if (!this._parent._atEnd)
              return;
            this._parent._observer.OnCompleted();
            this._parent.Dispose();
          }
        }
      }
    }
  }
}
