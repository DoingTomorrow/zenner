// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.SplitGroupTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using System;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace NLog.Targets.Wrappers
{
  [Target("SplitGroup", IsCompound = true)]
  public class SplitGroupTarget(params Target[] targets) : CompoundTargetBase(targets)
  {
    public SplitGroupTarget()
      : this(new Target[0])
    {
    }

    public SplitGroupTarget(string name, params Target[] targets)
      : this(targets)
    {
      this.Name = name;
    }

    protected override void Write(AsyncLogEventInfo logEvent)
    {
      AsyncHelpers.ForEachItemSequentially<Target>((IEnumerable<Target>) this.Targets, logEvent.Continuation, (AsynchronousAction<Target>) ((t, cont) => t.WriteAsyncLogEvent(logEvent.LogEvent.WithContinuation(cont))));
    }

    [Obsolete("Instead override Write(IList<AsyncLogEventInfo> logEvents. Marked obsolete on NLog 4.5")]
    protected override void Write(AsyncLogEventInfo[] logEvents)
    {
      this.Write((IList<AsyncLogEventInfo>) logEvents);
    }

    protected override void Write(IList<AsyncLogEventInfo> logEvents)
    {
      InternalLogger.Trace<string, int>("SplitGroup(Name={0}): Writing {1} events", this.Name, logEvents.Count);
      for (int index = 0; index < logEvents.Count; ++index)
      {
        AsyncLogEventInfo logEvent = logEvents[index];
        logEvents[index] = new AsyncLogEventInfo(logEvent.LogEvent, SplitGroupTarget.CountedWrap(logEvent.Continuation, this.Targets.Count));
      }
      for (int index = 0; index < this.Targets.Count; ++index)
      {
        InternalLogger.Trace<string, int, Target>("SplitGroup(Name={0}): Sending {1} events to {2}", this.Name, logEvents.Count, this.Targets[index]);
        IList<AsyncLogEventInfo> logEvents1 = logEvents;
        if (index < this.Targets.Count - 1)
        {
          AsyncLogEventInfo[] array = new AsyncLogEventInfo[logEvents.Count];
          logEvents.CopyTo(array, 0);
          logEvents1 = (IList<AsyncLogEventInfo>) array;
        }
        this.Targets[index].WriteAsyncLogEvents(logEvents1);
      }
    }

    private static AsyncContinuation CountedWrap(
      AsyncContinuation originalContinuation,
      int counter)
    {
      if (counter == 1)
        return originalContinuation;
      List<Exception> exceptions = new List<Exception>();
      return (AsyncContinuation) (ex =>
      {
        if (ex != null)
        {
          lock (exceptions)
            exceptions.Add(ex);
        }
        int num = Interlocked.Decrement(ref counter);
        if (num == 0)
        {
          Exception combinedException = AsyncHelpers.GetCombinedException((IList<Exception>) exceptions);
          InternalLogger.Trace<Exception>("Combined exception: {0}", combinedException);
          originalContinuation(combinedException);
        }
        else
          InternalLogger.Trace<int>("{0} remaining.", num);
      });
    }
  }
}
