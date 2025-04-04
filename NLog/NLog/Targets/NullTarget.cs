// Decompiled with JetBrains decompiler
// Type: NLog.Targets.NullTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System.ComponentModel;

#nullable disable
namespace NLog.Targets
{
  [Target("Null")]
  public sealed class NullTarget : TargetWithLayout
  {
    [DefaultValue(false)]
    public bool FormatMessage { get; set; }

    public NullTarget() => this.OptimizeBufferReuse = true;

    public NullTarget(string name)
      : this()
    {
      this.Name = name;
    }

    protected override void Write(LogEventInfo logEvent)
    {
      if (!this.FormatMessage)
        return;
      this.RenderLogEvent(this.Layout, logEvent);
    }
  }
}
