﻿// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.CompoundTargetBase
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NLog.Targets.Wrappers
{
  public abstract class CompoundTargetBase : Target
  {
    protected CompoundTargetBase(params Target[] targets)
    {
      this.Targets = (IList<Target>) new List<Target>((IEnumerable<Target>) targets);
    }

    public IList<Target> Targets { get; private set; }

    public override string ToString()
    {
      string str = string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(base.ToString());
      stringBuilder.Append("(");
      foreach (Target target in (IEnumerable<Target>) this.Targets)
      {
        stringBuilder.Append(str);
        stringBuilder.Append(target.ToString());
        str = ", ";
      }
      stringBuilder.Append(")");
      return stringBuilder.ToString();
    }

    protected override void Write(LogEventInfo logEvent)
    {
      throw new NotSupportedException("This target must not be invoked in a synchronous way.");
    }

    protected override void FlushAsync(AsyncContinuation asyncContinuation)
    {
      AsyncHelpers.ForEachItemInParallel<Target>((IEnumerable<Target>) this.Targets, asyncContinuation, (AsynchronousAction<Target>) ((t, c) => t.Flush(c)));
    }
  }
}
