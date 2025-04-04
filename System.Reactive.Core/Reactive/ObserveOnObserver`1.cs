// Decompiled with JetBrains decompiler
// Type: System.Reactive.ObserveOnObserver`1
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Reactive.Concurrency;
using System.Threading;

#nullable disable
namespace System.Reactive
{
  internal class ObserveOnObserver<T> : ScheduledObserver<T>
  {
    private IDisposable _cancel;

    public ObserveOnObserver(IScheduler scheduler, IObserver<T> observer, IDisposable cancel)
      : base(scheduler, observer)
    {
      this._cancel = cancel;
    }

    protected override void OnNextCore(T value)
    {
      base.OnNextCore(value);
      this.EnsureActive();
    }

    protected override void OnErrorCore(Exception exception)
    {
      base.OnErrorCore(exception);
      this.EnsureActive();
    }

    protected override void OnCompletedCore()
    {
      base.OnCompletedCore();
      this.EnsureActive();
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (!disposing)
        return;
      Interlocked.Exchange<IDisposable>(ref this._cancel, (IDisposable) null)?.Dispose();
    }
  }
}
