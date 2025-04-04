// Decompiled with JetBrains decompiler
// Type: NLog.Targets.TraceTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System.ComponentModel;
using System.Diagnostics;

#nullable disable
namespace NLog.Targets
{
  [Target("Trace")]
  public sealed class TraceTarget : TargetWithLayout
  {
    [DefaultValue(false)]
    public bool RawWrite { get; set; }

    public TraceTarget() => this.OptimizeBufferReuse = true;

    public TraceTarget(string name)
      : this()
    {
      this.Name = name;
    }

    protected override void Write(LogEventInfo logEvent)
    {
      string message = this.RenderLogEvent(this.Layout, logEvent);
      if (this.RawWrite || logEvent.Level <= LogLevel.Debug)
        Trace.WriteLine(message);
      else if (logEvent.Level == LogLevel.Info)
        Trace.TraceInformation(message);
      else if (logEvent.Level == LogLevel.Warn)
        Trace.TraceWarning(message);
      else if (logEvent.Level == LogLevel.Error)
        Trace.TraceError(message);
      else if (logEvent.Level >= LogLevel.Fatal)
        Trace.Fail(message);
      else
        Trace.WriteLine(message);
    }
  }
}
