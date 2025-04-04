// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.PostFilteringTargetWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Conditions;
using NLog.Config;
using System;
using System.Collections.Generic;

#nullable disable
namespace NLog.Targets.Wrappers
{
  [Target("PostFilteringWrapper", IsWrapper = true)]
  public class PostFilteringTargetWrapper : WrapperTargetBase
  {
    private static object boxedTrue = (object) true;

    public PostFilteringTargetWrapper()
      : this((Target) null)
    {
      this.Rules = (IList<FilteringRule>) new List<FilteringRule>();
    }

    public PostFilteringTargetWrapper(Target wrappedTarget)
    {
      this.Rules = (IList<FilteringRule>) new List<FilteringRule>();
      this.WrappedTarget = wrappedTarget;
    }

    public PostFilteringTargetWrapper(string name, Target wrappedTarget)
      : this(wrappedTarget)
    {
      this.Name = name;
    }

    public ConditionExpression DefaultFilter { get; set; }

    [ArrayParameter(typeof (FilteringRule), "when")]
    public IList<FilteringRule> Rules { get; private set; }

    [Obsolete("Instead override Write(IList<AsyncLogEventInfo> logEvents. Marked obsolete on NLog 4.5")]
    protected override void Write(AsyncLogEventInfo[] logEvents)
    {
      this.Write((IList<AsyncLogEventInfo>) logEvents);
    }

    protected override void Write(IList<AsyncLogEventInfo> logEvents)
    {
      ConditionExpression conditionExpression = (ConditionExpression) null;
      InternalLogger.Trace<string, int>("PostFilteringWrapper(Name={0}): Running on {1} events", this.Name, logEvents.Count);
      for (int index = 0; index < logEvents.Count; ++index)
      {
        foreach (FilteringRule rule in (IEnumerable<FilteringRule>) this.Rules)
        {
          object obj = rule.Exists.Evaluate(logEvents[index].LogEvent);
          if (PostFilteringTargetWrapper.boxedTrue.Equals(obj))
          {
            InternalLogger.Trace<string, ConditionExpression>("PostFilteringWrapper(Name={0}): Rule matched: {1}", this.Name, rule.Exists);
            conditionExpression = rule.Filter;
            break;
          }
        }
        if (conditionExpression != null)
          break;
      }
      if (conditionExpression == null)
        conditionExpression = this.DefaultFilter;
      if (conditionExpression == null)
      {
        this.WrappedTarget.WriteAsyncLogEvents(logEvents);
      }
      else
      {
        InternalLogger.Trace<string, ConditionExpression>("PostFilteringWrapper(Name={0}): Filter to apply: {1}", this.Name, conditionExpression);
        List<AsyncLogEventInfo> logEvents1 = new List<AsyncLogEventInfo>();
        for (int index = 0; index < logEvents.Count; ++index)
        {
          object obj = conditionExpression.Evaluate(logEvents[index].LogEvent);
          if (PostFilteringTargetWrapper.boxedTrue.Equals(obj))
            logEvents1.Add(logEvents[index]);
          else
            logEvents[index].Continuation((Exception) null);
        }
        InternalLogger.Trace<string, int>("PostFilteringWrapper(Name={0}): After filtering: {1} events.", this.Name, logEvents1.Count);
        if (logEvents1.Count <= 0)
          return;
        InternalLogger.Trace<string, Target>("PostFilteringWrapper(Name={0}): Sending to {1}", this.Name, this.WrappedTarget);
        this.WrappedTarget.WriteAsyncLogEvents((IList<AsyncLogEventInfo>) logEvents1);
      }
    }
  }
}
