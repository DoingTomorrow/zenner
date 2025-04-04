// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.CatchScheduler`1
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Reactive.Disposables;
using System.Runtime.CompilerServices;

#nullable disable
namespace System.Reactive.Concurrency
{
  internal class CatchScheduler<TException> : SchedulerWrapper where TException : Exception
  {
    private readonly Func<TException, bool> _handler;

    public CatchScheduler(IScheduler scheduler, Func<TException, bool> handler)
      : base(scheduler)
    {
      this._handler = handler;
    }

    protected override Func<IScheduler, TState, IDisposable> Wrap<TState>(
      Func<IScheduler, TState, IDisposable> action)
    {
      return (Func<IScheduler, TState, IDisposable>) ((self, state) =>
      {
        try
        {
          return action(this.GetRecursiveWrapper(self), state);
        }
        catch (TException ex)
        {
          if (this._handler(ex))
            return Disposable.Empty;
          throw;
        }
      });
    }

    public CatchScheduler(
      IScheduler scheduler,
      Func<TException, bool> handler,
      ConditionalWeakTable<IScheduler, IScheduler> cache)
      : base(scheduler, cache)
    {
      this._handler = handler;
    }

    protected override SchedulerWrapper Clone(
      IScheduler scheduler,
      ConditionalWeakTable<IScheduler, IScheduler> cache)
    {
      return (SchedulerWrapper) new CatchScheduler<TException>(scheduler, this._handler, cache);
    }

    protected override bool TryGetService(
      IServiceProvider provider,
      Type serviceType,
      out object service)
    {
      service = provider.GetService(serviceType);
      if (service != null)
      {
        if (serviceType == typeof (ISchedulerLongRunning))
          service = (object) new CatchScheduler<TException>.CatchSchedulerLongRunning((ISchedulerLongRunning) service, this._handler);
        else if (serviceType == typeof (ISchedulerPeriodic))
          service = (object) new CatchScheduler<TException>.CatchSchedulerPeriodic((ISchedulerPeriodic) service, this._handler);
      }
      return true;
    }

    private class CatchSchedulerLongRunning : ISchedulerLongRunning
    {
      private readonly ISchedulerLongRunning _scheduler;
      private readonly Func<TException, bool> _handler;

      public CatchSchedulerLongRunning(
        ISchedulerLongRunning scheduler,
        Func<TException, bool> handler)
      {
        this._scheduler = scheduler;
        this._handler = handler;
      }

      public IDisposable ScheduleLongRunning<TState>(
        TState state,
        Action<TState, ICancelable> action)
      {
        return this._scheduler.ScheduleLongRunning<TState>(state, (Action<TState, ICancelable>) ((state_, cancel) =>
        {
          try
          {
            action(state_, cancel);
          }
          catch (TException ex)
          {
            if (this._handler(ex))
              return;
            throw;
          }
        }));
      }
    }

    private class CatchSchedulerPeriodic : ISchedulerPeriodic
    {
      private readonly ISchedulerPeriodic _scheduler;
      private readonly Func<TException, bool> _handler;

      public CatchSchedulerPeriodic(ISchedulerPeriodic scheduler, Func<TException, bool> handler)
      {
        this._scheduler = scheduler;
        this._handler = handler;
      }

      public IDisposable SchedulePeriodic<TState>(
        TState state,
        TimeSpan period,
        Func<TState, TState> action)
      {
        bool failed = false;
        SingleAssignmentDisposable d = new SingleAssignmentDisposable();
        d.Disposable = this._scheduler.SchedulePeriodic<TState>(state, period, (Func<TState, TState>) (state_ =>
        {
          if (failed)
            return default (TState);
          try
          {
            return action(state_);
          }
          catch (TException ex)
          {
            failed = true;
            if (!this._handler(ex))
            {
              throw;
            }
            else
            {
              d.Dispose();
              return default (TState);
            }
          }
        }));
        return (IDisposable) d;
      }
    }
  }
}
