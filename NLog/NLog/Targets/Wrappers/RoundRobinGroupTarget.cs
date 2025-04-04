// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.RoundRobinGroupTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using System;
using System.Threading;

#nullable disable
namespace NLog.Targets.Wrappers
{
  [Target("RoundRobinGroup", IsCompound = true)]
  public class RoundRobinGroupTarget : CompoundTargetBase
  {
    private int _currentTarget = -1;

    public RoundRobinGroupTarget()
      : this(new Target[0])
    {
    }

    public RoundRobinGroupTarget(string name, params Target[] targets)
      : this(targets)
    {
      this.Name = name;
    }

    public RoundRobinGroupTarget(params Target[] targets)
      : base(targets)
    {
      this.OptimizeBufferReuse = this.GetType() == typeof (RoundRobinGroupTarget);
    }

    protected override void WriteAsyncThreadSafe(AsyncLogEventInfo logEvent)
    {
      this.Write(logEvent);
    }

    protected override void Write(AsyncLogEventInfo logEvent)
    {
      if (this.Targets.Count == 0)
        logEvent.Continuation((Exception) null);
      else
        this.Targets[(int) ((long) (uint) Interlocked.Increment(ref this._currentTarget) % (long) this.Targets.Count)].WriteAsyncLogEvent(logEvent);
    }
  }
}
