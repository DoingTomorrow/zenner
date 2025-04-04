// Decompiled with JetBrains decompiler
// Type: NLog.Targets.TargetPropertyWithContext
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Layouts;

#nullable disable
namespace NLog.Targets
{
  [NLogConfigurationItem]
  [ThreadAgnostic]
  public class TargetPropertyWithContext
  {
    public TargetPropertyWithContext()
      : this((string) null, (Layout) null)
    {
    }

    public TargetPropertyWithContext(string name, Layout layout)
    {
      this.Name = name;
      this.Layout = layout;
      this.IncludeEmptyValue = true;
    }

    [RequiredParameter]
    public string Name { get; set; }

    [RequiredParameter]
    public Layout Layout { get; set; }

    public bool IncludeEmptyValue { get; set; }
  }
}
