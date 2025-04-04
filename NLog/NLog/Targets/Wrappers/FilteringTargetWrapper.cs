// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.FilteringTargetWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Conditions;
using NLog.Config;
using System;

#nullable disable
namespace NLog.Targets.Wrappers
{
  [Target("FilteringWrapper", IsWrapper = true)]
  public class FilteringTargetWrapper : WrapperTargetBase
  {
    private static readonly object boxedBooleanTrue = (object) true;

    public FilteringTargetWrapper()
      : this((Target) null, (ConditionExpression) null)
    {
    }

    public FilteringTargetWrapper(string name, Target wrappedTarget, ConditionExpression condition)
      : this(wrappedTarget, condition)
    {
      this.Name = name;
    }

    public FilteringTargetWrapper(Target wrappedTarget, ConditionExpression condition)
    {
      this.WrappedTarget = wrappedTarget;
      this.Condition = condition;
      this.OptimizeBufferReuse = this.GetType() == typeof (FilteringTargetWrapper);
    }

    [RequiredParameter]
    public ConditionExpression Condition { get; set; }

    protected override void Write(AsyncLogEventInfo logEvent)
    {
      object obj = this.Condition.Evaluate(logEvent.LogEvent);
      if (FilteringTargetWrapper.boxedBooleanTrue.Equals(obj))
        this.WrappedTarget.WriteAsyncLogEvent(logEvent);
      else
        logEvent.Continuation((Exception) null);
    }
  }
}
