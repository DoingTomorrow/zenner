// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.SchedulerOperationAwaiter
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

#nullable disable
namespace System.Reactive.Concurrency
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  public sealed class SchedulerOperationAwaiter : INotifyCompletion
  {
    private readonly Func<Action, IDisposable> _schedule;
    private readonly CancellationToken _cancellationToken;
    private readonly bool _postBackToOriginalContext;
    private readonly CancellationTokenRegistration _ctr;
    private volatile Action _continuation;
    private volatile IDisposable _work;

    internal SchedulerOperationAwaiter(
      Func<Action, IDisposable> schedule,
      CancellationToken cancellationToken,
      bool postBackToOriginalContext)
    {
      this._schedule = schedule;
      this._cancellationToken = cancellationToken;
      this._postBackToOriginalContext = postBackToOriginalContext;
      if (!cancellationToken.CanBeCanceled)
        return;
      this._ctr = this._cancellationToken.Register(new Action(this.Cancel));
    }

    public bool IsCompleted => this._cancellationToken.IsCancellationRequested;

    public void GetResult() => this._cancellationToken.ThrowIfCancellationRequested();

    public void OnCompleted(Action continuation)
    {
      if (continuation == null)
        throw new ArgumentNullException(nameof (continuation));
      if (this._continuation != null)
        throw new InvalidOperationException(Strings_Core.SCHEDULER_OPERATION_ALREADY_AWAITED);
      if (this._postBackToOriginalContext)
      {
        SynchronizationContext ctx = SynchronizationContext.Current;
        if (ctx != null)
        {
          Action original = continuation;
          continuation = (Action) (() => ctx.Post((SendOrPostCallback) (a => ((Action) a)()), (object) original));
        }
      }
      int ran = 0;
      this._continuation = (Action) (() =>
      {
        if (Interlocked.Exchange(ref ran, 1) != 0)
          return;
        this._ctr.Dispose();
        continuation();
      });
      this._work = this._schedule(this._continuation);
    }

    private void Cancel()
    {
      this._work?.Dispose();
      Action continuation = this._continuation;
      if (continuation == null)
        return;
      continuation();
    }
  }
}
