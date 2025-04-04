// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.ObserveOn`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Concurrency;
using System.Threading;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
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
        ObserveOn<TSource>.ObserveOnImpl observeOnImpl = new ObserveOn<TSource>.ObserveOnImpl(this, observer, cancel);
        setSink((IDisposable) observeOnImpl);
        return this._source.Subscribe((IObserver<TSource>) observeOnImpl);
      }
      ObserveOnObserver<TSource> observeOnObserver = new ObserveOnObserver<TSource>(this._scheduler, observer, cancel);
      setSink((IDisposable) observeOnObserver);
      return this._source.Subscribe((IObserver<TSource>) observeOnObserver);
    }

    private class ObserveOnImpl : Sink<TSource>, IObserver<TSource>
    {
      private readonly ObserveOn<TSource> _parent;

      public ObserveOnImpl(
        ObserveOn<TSource> parent,
        IObserver<TSource> observer,
        IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public void OnNext(TSource value)
      {
        this._parent._context.PostWithStartComplete((Action) (() => this._observer.OnNext(value)));
      }

      public void OnError(Exception error)
      {
        this._parent._context.PostWithStartComplete((Action) (() =>
        {
          this._observer.OnError(error);
          // ISSUE: reference to a compiler-generated method
          this.\u003C\u003En__0();
        }));
      }

      public void OnCompleted()
      {
        this._parent._context.PostWithStartComplete((Action) (() =>
        {
          this._observer.OnCompleted();
          this.Dispose();
        }));
      }
    }
  }
}
