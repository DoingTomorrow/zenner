// Decompiled with JetBrains decompiler
// Type: NLog.Targets.TargetWithLayout
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Layouts;
using System.ComponentModel;

#nullable disable
namespace NLog.Targets
{
  public abstract class TargetWithLayout : Target
  {
    protected TargetWithLayout()
    {
      this.Layout = (Layout) "${longdate}|${level:uppercase=true}|${logger}|${message}";
    }

    [RequiredParameter]
    [DefaultValue("${longdate}|${level:uppercase=true}|${logger}|${message}")]
    public virtual Layout Layout { get; set; }
  }
}
