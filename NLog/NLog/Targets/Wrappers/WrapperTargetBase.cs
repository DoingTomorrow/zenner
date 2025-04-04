// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.WrapperTargetBase
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Config;
using System;

#nullable disable
namespace NLog.Targets.Wrappers
{
  public abstract class WrapperTargetBase : Target
  {
    [RequiredParameter]
    public Target WrappedTarget { get; set; }

    public override string ToString()
    {
      return string.Format("{0}({1})", (object) base.ToString(), (object) this.WrappedTarget);
    }

    protected override void FlushAsync(AsyncContinuation asyncContinuation)
    {
      this.WrappedTarget.Flush(asyncContinuation);
    }

    protected override sealed void Write(LogEventInfo logEvent)
    {
      throw new NotSupportedException("This target must not be invoked in a synchronous way.");
    }
  }
}
