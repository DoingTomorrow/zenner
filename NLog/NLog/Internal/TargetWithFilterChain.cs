// Decompiled with JetBrains decompiler
// Type: NLog.Internal.TargetWithFilterChain
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Filters;
using NLog.Targets;
using System.Collections.Generic;

#nullable disable
namespace NLog.Internal
{
  [NLogConfigurationItem]
  internal class TargetWithFilterChain
  {
    private StackTraceUsage? _stackTraceUsage;

    public TargetWithFilterChain(Target target, IList<Filter> filterChain)
    {
      this.Target = target;
      this.FilterChain = filterChain;
    }

    public Target Target { get; private set; }

    public IList<Filter> FilterChain { get; private set; }

    public TargetWithFilterChain NextInChain { get; set; }

    public StackTraceUsage GetStackTraceUsage() => this._stackTraceUsage ?? StackTraceUsage.None;

    internal StackTraceUsage PrecalculateStackTraceUsage()
    {
      StackTraceUsage stackTraceUsage1 = StackTraceUsage.None;
      if (this.Target != null)
        stackTraceUsage1 = this.Target.StackTraceUsage;
      if (this.NextInChain != null && stackTraceUsage1 != StackTraceUsage.WithSource)
      {
        StackTraceUsage stackTraceUsage2 = this.NextInChain.PrecalculateStackTraceUsage();
        if (stackTraceUsage2 > stackTraceUsage1)
          stackTraceUsage1 = stackTraceUsage2;
      }
      this._stackTraceUsage = new StackTraceUsage?(stackTraceUsage1);
      return stackTraceUsage1;
    }
  }
}
