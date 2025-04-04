// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.VirtualTimeSchedulerBase`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace System.Reactive.Concurrency
{
  public abstract class VirtualTimeSchedulerBase<TAbsolute, TRelative> : 
    IScheduler,
    IServiceProvider,
    IStopwatchProvider
    where TAbsolute : IComparable<TAbsolute>
  {
    protected VirtualTimeSchedulerBase()
      : this(default (TAbsolute), (IComparer<TAbsolute>) System.Collections.Generic.Comparer<TAbsolute>.Default)
    {
    }

    protected VirtualTimeSchedulerBase(TAbsolute initialClock, IComparer<TAbsolute> comparer)
    {
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      this.Clock = initialClock;
      this.Comparer = comparer;
    }

    protected abstract TAbsolute Add(TAbsolute absolute, TRelative relative);

    protected abstract DateTimeOffset ToDateTimeOffset(TAbsolute absolute);

    protected abstract TRelative ToRelative(TimeSpan timeSpan);

    public bool IsEnabled { get; private set; }

    protected IComparer<TAbsolute> Comparer { get; private set; }

    public abstract IDisposable ScheduleAbsolute<TState>(
      TState state,
      TAbsolute dueTime,
      Func<IScheduler, TState, IDisposable> action);

    public IDisposable ScheduleRelative<TState>(
      TState state,
      TRelative dueTime,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      TAbsolute dueTime1 = this.Add(this.Clock, dueTime);
      return this.ScheduleAbsolute<TState>(state, dueTime1, action);
    }

    public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return this.ScheduleAbsolute<TState>(state, this.Clock, action);
    }

    public IDisposable Schedule<TState>(
      TState state,
      TimeSpan dueTime,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return this.ScheduleRelative<TState>(state, this.ToRelative(dueTime), action);
    }

    public IDisposable Schedule<TState>(
      TState state,
      DateTimeOffset dueTime,
      Func<IScheduler, TState, IDisposable> action)
    {
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      return this.ScheduleRelative<TState>(state, this.ToRelative(dueTime - this.Now), action);
    }

    public void Start()
    {
      if (this.IsEnabled)
        return;
      this.IsEnabled = true;
      do
      {
        IScheduledItem<TAbsolute> next = this.GetNext();
        if (next != null)
        {
          if (this.Comparer.Compare(next.DueTime, this.Clock) > 0)
            this.Clock = next.DueTime;
          next.Invoke();
        }
        else
          this.IsEnabled = false;
      }
      while (this.IsEnabled);
    }

    public void Stop() => this.IsEnabled = false;

    public void AdvanceTo(TAbsolute time)
    {
      int num = this.Comparer.Compare(time, this.Clock);
      if (num < 0)
        throw new ArgumentOutOfRangeException(nameof (time));
      if (num == 0)
        return;
      this.IsEnabled = !this.IsEnabled ? true : throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Strings_Linq.CANT_ADVANCE_WHILE_RUNNING, new object[1]
      {
        (object) nameof (AdvanceTo)
      }));
      do
      {
        IScheduledItem<TAbsolute> next = this.GetNext();
        if (next != null && this.Comparer.Compare(next.DueTime, time) <= 0)
        {
          if (this.Comparer.Compare(next.DueTime, this.Clock) > 0)
            this.Clock = next.DueTime;
          next.Invoke();
        }
        else
          this.IsEnabled = false;
      }
      while (this.IsEnabled);
      this.Clock = time;
    }

    public void AdvanceBy(TRelative time)
    {
      TAbsolute absolute = this.Add(this.Clock, time);
      int num = this.Comparer.Compare(absolute, this.Clock);
      if (num < 0)
        throw new ArgumentOutOfRangeException(nameof (time));
      if (num == 0)
        return;
      if (!this.IsEnabled)
        this.AdvanceTo(absolute);
      else
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Strings_Linq.CANT_ADVANCE_WHILE_RUNNING, new object[1]
        {
          (object) nameof (AdvanceBy)
        }));
    }

    public void Sleep(TRelative time)
    {
      TAbsolute x = this.Add(this.Clock, time);
      if (this.Comparer.Compare(x, this.Clock) < 0)
        throw new ArgumentOutOfRangeException(nameof (time));
      this.Clock = x;
    }

    public TAbsolute Clock { get; protected set; }

    public DateTimeOffset Now => this.ToDateTimeOffset(this.Clock);

    protected abstract IScheduledItem<TAbsolute> GetNext();

    object IServiceProvider.GetService(Type serviceType) => this.GetService(serviceType);

    protected virtual object GetService(Type serviceType)
    {
      return serviceType == typeof (IStopwatchProvider) ? (object) this : (object) null;
    }

    public IStopwatch StartStopwatch()
    {
      DateTimeOffset start = this.ToDateTimeOffset(this.Clock);
      return (IStopwatch) new VirtualTimeSchedulerBase<TAbsolute, TRelative>.VirtualTimeStopwatch((Func<TimeSpan>) (() => this.ToDateTimeOffset(this.Clock) - start));
    }

    private class VirtualTimeStopwatch : IStopwatch
    {
      private readonly Func<TimeSpan> _getElapsed;

      public VirtualTimeStopwatch(Func<TimeSpan> getElapsed) => this._getElapsed = getElapsed;

      public TimeSpan Elapsed => this._getElapsed();
    }
  }
}
