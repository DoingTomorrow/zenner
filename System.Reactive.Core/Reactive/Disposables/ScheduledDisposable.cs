// Decompiled with JetBrains decompiler
// Type: System.Reactive.Disposables.ScheduledDisposable
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Reactive.Concurrency;
using System.Threading;

#nullable disable
namespace System.Reactive.Disposables
{
  public sealed class ScheduledDisposable : ICancelable, IDisposable
  {
    private readonly IScheduler _scheduler;
    private volatile IDisposable _disposable;

    public ScheduledDisposable(IScheduler scheduler, IDisposable disposable)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      if (disposable == null)
        throw new ArgumentNullException(nameof (disposable));
      this._scheduler = scheduler;
      this._disposable = disposable;
    }

    public IScheduler Scheduler => this._scheduler;

    public IDisposable Disposable
    {
      get
      {
        IDisposable disposable = this._disposable;
        return disposable == BooleanDisposable.True ? (IDisposable) DefaultDisposable.Instance : disposable;
      }
    }

    public bool IsDisposed => this._disposable == BooleanDisposable.True;

    public void Dispose() => this.Scheduler.Schedule(new Action(this.DisposeInner));

    private void DisposeInner()
    {
      IDisposable disposable = Interlocked.Exchange<IDisposable>(ref this._disposable, (IDisposable) BooleanDisposable.True);
      if (disposable == BooleanDisposable.True)
        return;
      disposable.Dispose();
    }
  }
}
