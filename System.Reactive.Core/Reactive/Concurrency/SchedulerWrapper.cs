// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.SchedulerWrapper
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Runtime.CompilerServices;

#nullable disable
namespace System.Reactive.Concurrency
{
  internal abstract class SchedulerWrapper : IScheduler, IServiceProvider
  {
    protected readonly IScheduler _scheduler;
    private readonly ConditionalWeakTable<IScheduler, IScheduler> _cache;

    public SchedulerWrapper(IScheduler scheduler)
    {
      this._scheduler = scheduler;
      this._cache = new ConditionalWeakTable<IScheduler, IScheduler>();
    }

    public DateTimeOffset Now => this._scheduler.Now;

    public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return this._scheduler.Schedule<TState>(state, this.Wrap<TState>(action));
    }

    public IDisposable Schedule<TState>(
      TState state,
      TimeSpan dueTime,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return this._scheduler.Schedule<TState>(state, dueTime, this.Wrap<TState>(action));
    }

    public IDisposable Schedule<TState>(
      TState state,
      DateTimeOffset dueTime,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return this._scheduler.Schedule<TState>(state, dueTime, this.Wrap<TState>(action));
    }

    protected virtual Func<IScheduler, TState, IDisposable> Wrap<TState>(
      Func<IScheduler, TState, IDisposable> action)
    {
      return (Func<IScheduler, TState, IDisposable>) ((self, state) => action(this.GetRecursiveWrapper(self), state));
    }

    public SchedulerWrapper(
      IScheduler scheduler,
      ConditionalWeakTable<IScheduler, IScheduler> cache)
    {
      this._scheduler = scheduler;
      this._cache = cache;
    }

    protected IScheduler GetRecursiveWrapper(IScheduler scheduler)
    {
      return this._cache.GetValue(scheduler, (ConditionalWeakTable<IScheduler, IScheduler>.CreateValueCallback) (s => (IScheduler) this.Clone(s, this._cache)));
    }

    protected abstract SchedulerWrapper Clone(
      IScheduler scheduler,
      ConditionalWeakTable<IScheduler, IScheduler> cache);

    public object GetService(Type serviceType)
    {
      if (!(this._scheduler is IServiceProvider scheduler))
        return (object) null;
      object service = (object) null;
      return this.TryGetService(scheduler, serviceType, out service) ? service : scheduler.GetService(serviceType);
    }

    protected abstract bool TryGetService(
      IServiceProvider provider,
      Type serviceType,
      out object service);
  }
}
