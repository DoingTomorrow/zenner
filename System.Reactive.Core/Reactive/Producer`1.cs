// Decompiled with JetBrains decompiler
// Type: System.Reactive.Producer`1
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive
{
  internal abstract class Producer<TSource> : IProducer<TSource>, IObservable<TSource>
  {
    public IDisposable Subscribe(IObserver<TSource> observer)
    {
      return observer != null ? this.SubscribeRaw(observer, true) : throw new ArgumentNullException(nameof (observer));
    }

    public IDisposable SubscribeRaw(IObserver<TSource> observer, bool enableSafeguard)
    {
      Producer<TSource>.State state = new Producer<TSource>.State();
      state.observer = observer;
      state.sink = new SingleAssignmentDisposable();
      state.subscription = new SingleAssignmentDisposable();
      ICancelable cancelable = StableCompositeDisposable.Create((IDisposable) state.sink, (IDisposable) state.subscription);
      if (enableSafeguard)
        state.observer = SafeObserver<TSource>.Create(state.observer, (IDisposable) cancelable);
      if (CurrentThreadScheduler.IsScheduleRequired)
        CurrentThreadScheduler.Instance.Schedule<Producer<TSource>.State>(state, new Func<IScheduler, Producer<TSource>.State, IDisposable>(this.Run));
      else
        state.subscription.Disposable = this.Run(state.observer, (IDisposable) state.subscription, new Action<IDisposable>(state.Assign));
      return (IDisposable) cancelable;
    }

    private IDisposable Run(IScheduler _, Producer<TSource>.State x)
    {
      x.subscription.Disposable = this.Run(x.observer, (IDisposable) x.subscription, new Action<IDisposable>(x.Assign));
      return Disposable.Empty;
    }

    protected abstract IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink);

    private struct State
    {
      public SingleAssignmentDisposable sink;
      public SingleAssignmentDisposable subscription;
      public IObserver<TSource> observer;

      public void Assign(IDisposable s) => this.sink.Disposable = s;
    }
  }
}
