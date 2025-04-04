// Decompiled with JetBrains decompiler
// Type: NLog.Targets.DatabaseParameterInfo
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Layouts;
using System.ComponentModel;

#nullable disable
namespace NLog.Targets
{
  [NLogConfigurationItem]
  public class DatabaseParameterInfo
  {
    public DatabaseParameterInfo()
      : this((string) null, (Layout) null)
    {
    }

    public DatabaseParameterInfo(string parameterName, Layout parameterLayout)
    {
      this.Name = parameterName;
      this.Layout = parameterLayout;
    }

    [RequiredParameter]
    public string Name { get; set; }

    [RequiredParameter]
    public Layout Layout { get; set; }

    [DefaultValue(0)]
    public int Size { get; set; }

    [DefaultValue(0)]
    public byte Precision { get; set; }

    [DefaultValue(0)]
    public byte Scale { get; set; }
  }
}
