// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.RandomizeGroupTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using System;

#nullable disable
namespace NLog.Targets.Wrappers
{
  [Target("RandomizeGroup", IsCompound = true)]
  public class RandomizeGroupTarget : CompoundTargetBase
  {
    private readonly Random _random = new Random();

    public RandomizeGroupTarget()
      : this(new Target[0])
    {
    }

    public RandomizeGroupTarget(string name, params Target[] targets)
      : this(targets)
    {
      this.Name = name;
    }

    public RandomizeGroupTarget(params Target[] targets)
      : base(targets)
    {
      this.OptimizeBufferReuse = this.GetType() == typeof (RandomizeGroupTarget);
    }

    protected override void Write(AsyncLogEventInfo logEvent)
    {
      if (this.Targets.Count == 0)
      {
        logEvent.Continuation((Exception) null);
      }
      else
      {
        int index;
        lock (this._random)
          index = this._random.Next(this.Targets.Count);
        this.Targets[index].WriteAsyncLogEvent(logEvent);
      }
    }
  }
}
