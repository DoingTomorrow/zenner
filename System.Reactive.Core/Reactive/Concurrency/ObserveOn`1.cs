// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.ObserveOn`1
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Reactive.Disposables;
using System.Threading;

#nullable disable
namespace System.Reactive.Concurrency
{
  internal class ObserveOn<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly IScheduler _scheduler;
    private readonly SynchronizationContext _context;

    public ObserveOn(IObservable<TSource> source, IScheduler scheduler)
    {
      this._source = source;
      this._scheduler = scheduler;
    }

    public ObserveOn(IObservable<TSource> source, SynchronizationContext context)
    {
      this._source = source;
      this._context = context;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if (this._context != null)
      {
        ObserveOn<TSource>.ObserveOnSink observeOnSink = new ObserveOn<TSource>.ObserveOnSink(this, observer, cancel);
        setSink((IDisposable) observeOnSink);
        return observeOnSink.Run();
      }
      ObserveOnObserver<TSource> observeOnObserver = new ObserveOnObserver<TSource>(this._scheduler, observer, cancel);
      setSink((IDisposable) observeOnObserver);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) observeOnObserver);
    }

    private class ObserveOnSink : Sink<TSource>, IObserver<TSource>
    {
      private readonly ObserveOn<TSource> _parent;

      public ObserveOnSink(
        ObserveOn<TSource> parent,
        IObserver<TSource> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public IDisposable Run()
      {
        this._parent._context.OperationStarted();
        return (IDisposable) StableCompositeDisposable.Create(this._parent._source.SubscribeSafe<TSource>((IObserver<TSource>) this), Disposable.Create((Action) (() => this._parent._context.OperationCompleted())));
      }

      public void OnNext(TSource value)
      {
        this._parent._context.Post(new SendOrPostCallback(this.OnNextPosted), (object) value);
      }

      public void OnError(Exception error)
      {
        this._parent._context.Post(new SendOrPostCallback(this.OnErrorPosted), (object) error);
      }

      public void OnCompleted()
      {
        this._parent._context.Post(new SendOrPostCallback(this.OnCompletedPosted), (object) null);
      }

      private void OnNextPosted(object value) => this._observer.OnNext((TSource) value);

      private void OnErrorPosted(object error)
      {
        this._observer.OnError((Exception) error);
        this.Dispose();
      }

      private void OnCompletedPosted(object ignored)
      {
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}
