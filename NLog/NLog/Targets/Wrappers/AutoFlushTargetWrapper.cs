// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.AutoFlushTargetWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Conditions;
using NLog.Internal;

#nullable disable
namespace NLog.Targets.Wrappers
{
  [Target("AutoFlushWrapper", IsWrapper = true)]
  public class AutoFlushTargetWrapper : WrapperTargetBase
  {
    private bool? _asyncFlush;
    private readonly AsyncOperationCounter _pendingManualFlushList = new AsyncOperationCounter();

    public ConditionExpression Condition { get; set; }

    public bool AsyncFlush
    {
      get => this._asyncFlush ?? true;
      set => this._asyncFlush = new bool?(value);
    }

    public AutoFlushTargetWrapper()
      : this((Target) null)
    {
    }

    public AutoFlushTargetWrapper(string name, Target wrappedTarget)
      : this(wrappedTarget)
    {
      this.Name = name;
    }

    public AutoFlushTargetWrapper(Target wrappedTarget) => this.WrappedTarget = wrappedTarget;

    protected override void InitializeTarget()
    {
      base.InitializeTarget();
      if (this._asyncFlush.HasValue || !(this.WrappedTarget is BufferingTargetWrapper))
        return;
      this.AsyncFlush = false;
    }

    protected override void Write(AsyncLogEventInfo logEvent)
    {
      if (this.Condition == null || this.Condition.Evaluate(logEvent.LogEvent).Equals((object) true))
      {
        if (this.AsyncFlush)
        {
          AsyncContinuation currentContinuation = logEvent.Continuation;
          AsyncContinuation asyncContinuation = (AsyncContinuation) (ex =>
          {
            if (ex == null)
              this.WrappedTarget.Flush((AsyncContinuation) (e => { }));
            this._pendingManualFlushList.CompleteOperation(ex);
            currentContinuation(ex);
          });
          this._pendingManualFlushList.BeginOperation();
          this.WrappedTarget.WriteAsyncLogEvent(logEvent.LogEvent.WithContinuation(asyncContinuation));
        }
        else
        {
          this.WrappedTarget.WriteAsyncLogEvent(logEvent);
          this.FlushAsync((AsyncContinuation) (e => { }));
        }
      }
      else
        this.WrappedTarget.WriteAsyncLogEvent(logEvent);
    }

    protected override void FlushAsync(AsyncContinuation asyncContinuation)
    {
      this.WrappedTarget.Flush(this._pendingManualFlushList.RegisterCompletionNotification(asyncContinuation));
    }

    protected override void CloseTarget()
    {
      this._pendingManualFlushList.Clear();
      base.CloseTarget();
    }
  }
}
