// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.SchedulerOperation
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Threading;

#nullable disable
namespace System.Reactive.Concurrency
{
  public sealed class SchedulerOperation
  {
    private readonly Func<Action, IDisposable> _schedule;
    private readonly CancellationToken _cancellationToken;
    private readonly bool _postBackToOriginalContext;

    internal SchedulerOperation(
      Func<Action, IDisposable> schedule,
      CancellationToken cancellationToken)
      : this(schedule, cancellationToken, false)
    {
    }

    internal SchedulerOperation(
      Func<Action, IDisposable> schedule,
      CancellationToken cancellationToken,
      bool postBackToOriginalContext)
    {
      this._schedule = schedule;
      this._cancellationToken = cancellationToken;
      this._postBackToOriginalContext = postBackToOriginalContext;
    }

    public SchedulerOperation ConfigureAwait(bool continueOnCapturedContext)
    {
      return new SchedulerOperation(this._schedule, this._cancellationToken, continueOnCapturedContext);
    }

    public SchedulerOperationAwaiter GetAwaiter()
    {
      return new SchedulerOperationAwaiter(this._schedule, this._cancellationToken, this._postBackToOriginalContext);
    }
  }
}
