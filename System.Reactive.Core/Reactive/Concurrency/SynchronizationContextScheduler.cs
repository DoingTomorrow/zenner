// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.SynchronizationContextScheduler
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Reactive.Disposables;
using System.Threading;

#nullable disable
namespace System.Reactive.Concurrency
{
  public class SynchronizationContextScheduler : LocalScheduler
  {
    private readonly SynchronizationContext _context;
    private readonly bool _alwaysPost;

    public SynchronizationContextScheduler(SynchronizationContext context)
    {
      this._context = context != null ? context : throw new ArgumentNullException(nameof (context));
      this._alwaysPost = true;
    }

    public SynchronizationContextScheduler(SynchronizationContext context, bool alwaysPost)
    {
      this._context = context != null ? context : throw new ArgumentNullException(nameof (context));
      this._alwaysPost = alwaysPost;
    }

    public override IDisposable Schedule<TState>(
      TState state,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      SingleAssignmentDisposable d = new SingleAssignmentDisposable();
      if (!this._alwaysPost && this._context == SynchronizationContext.Current)
        d.Disposable = action((IScheduler) this, state);
      else
        this._context.PostWithStartComplete((Action) (() =>
        {
          if (d.IsDisposed)
            return;
          d.Disposable = action((IScheduler) this, state);
        }));
      return (IDisposable) d;
    }

    public override IDisposable Schedule<TState>(
      TState state,
      TimeSpan dueTime,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      TimeSpan dueTime1 = Scheduler.Normalize(dueTime);
      return dueTime1.Ticks == 0L ? this.Schedule<TState>(state, action) : DefaultScheduler.Instance.Schedule<TState>(state, dueTime1, (Func<IScheduler, TState, IDisposable>) ((_, state1) => this.Schedule<TState>(state1, action)));
    }
  }
}
