// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.RepeatingTargetWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using System.ComponentModel;

#nullable disable
namespace NLog.Targets.Wrappers
{
  [Target("RepeatingWrapper", IsWrapper = true)]
  public class RepeatingTargetWrapper : WrapperTargetBase
  {
    public RepeatingTargetWrapper()
      : this((Target) null, 3)
    {
    }

    public RepeatingTargetWrapper(string name, Target wrappedTarget, int repeatCount)
      : this(wrappedTarget, repeatCount)
    {
      this.Name = name;
    }

    public RepeatingTargetWrapper(Target wrappedTarget, int repeatCount)
    {
      this.WrappedTarget = wrappedTarget;
      this.RepeatCount = repeatCount;
      this.OptimizeBufferReuse = this.GetType() == typeof (RepeatingTargetWrapper);
    }

    [DefaultValue(3)]
    public int RepeatCount { get; set; }

    protected override void Write(AsyncLogEventInfo logEvent)
    {
      AsyncHelpers.Repeat(this.RepeatCount, logEvent.Continuation, (AsynchronousAction) (cont => this.WrappedTarget.WriteAsyncLogEvent(logEvent.LogEvent.WithContinuation(cont))));
    }
  }
}
