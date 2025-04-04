// Decompiled with JetBrains decompiler
// Type: System.Reactive.PlatformServices.PeriodicTimerSystemClockMonitor
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.PlatformServices
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  public class PeriodicTimerSystemClockMonitor : INotifySystemClockChanged
  {
    private readonly TimeSpan _period;
    private readonly SerialDisposable _timer;
    private DateTimeOffset _lastTime;
    private EventHandler<SystemClockChangedEventArgs> _systemClockChanged;
    private const int SYNC_MAXRETRIES = 100;
    private const double SYNC_MAXDELTA = 10.0;
    private const int MAXERROR = 100;

    public PeriodicTimerSystemClockMonitor(TimeSpan period)
    {
      this._period = period;
      this._timer = new SerialDisposable();
    }

    public event EventHandler<SystemClockChangedEventArgs> SystemClockChanged
    {
      add
      {
        this.NewTimer();
        this._systemClockChanged += value;
      }
      remove
      {
        this._systemClockChanged -= value;
        this._timer.Disposable = Disposable.Empty;
      }
    }

    private void NewTimer()
    {
      this._timer.Disposable = Disposable.Empty;
      int num = 0;
      do
      {
        this._lastTime = SystemClock.UtcNow;
        this._timer.Disposable = ConcurrencyAbstractionLayer.Current.StartPeriodicTimer(new Action(this.TimeChanged), this._period);
      }
      while (Math.Abs((SystemClock.UtcNow - this._lastTime).TotalMilliseconds) > 10.0 && ++num < 100);
      if (num >= 100)
        throw new InvalidOperationException(Strings_Core.FAILED_CLOCK_MONITORING);
    }

    private void TimeChanged()
    {
      DateTimeOffset utcNow = SystemClock.UtcNow;
      if (Math.Abs((utcNow - (this._lastTime + this._period)).TotalMilliseconds) >= 100.0)
      {
        EventHandler<SystemClockChangedEventArgs> systemClockChanged = this._systemClockChanged;
        if (systemClockChanged != null)
          systemClockChanged((object) this, new SystemClockChangedEventArgs(this._lastTime + this._period, utcNow));
        this.NewTimer();
      }
      else
        this._lastTime = SystemClock.UtcNow;
    }
  }
}
