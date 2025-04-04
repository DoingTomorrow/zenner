// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.ScheduledItem`2
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Collections.Generic;

#nullable disable
namespace System.Reactive.Concurrency
{
  public sealed class ScheduledItem<TAbsolute, TValue> : ScheduledItem<TAbsolute> where TAbsolute : IComparable<TAbsolute>
  {
    private readonly IScheduler _scheduler;
    private readonly TValue _state;
    private readonly Func<IScheduler, TValue, IDisposable> _action;

    public ScheduledItem(
      IScheduler scheduler,
      TValue state,
      Func<IScheduler, TValue, IDisposable> action,
      TAbsolute dueTime,
      IComparer<TAbsolute> comparer)
      : base(dueTime, comparer)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      this._scheduler = scheduler;
      this._state = state;
      this._action = action;
    }

    public ScheduledItem(
      IScheduler scheduler,
      TValue state,
      Func<IScheduler, TValue, IDisposable> action,
      TAbsolute dueTime)
      : this(scheduler, state, action, dueTime, (IComparer<TAbsolute>) Comparer<TAbsolute>.Default)
    {
    }

    protected override IDisposable InvokeCore() => this._action(this._scheduler, this._state);
  }
}
