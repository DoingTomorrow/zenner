// Decompiled with JetBrains decompiler
// Type: NLog.Targets.NLogViewerParameterInfo
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Layouts;

#nullable disable
namespace NLog.Targets
{
  [NLogConfigurationItem]
  public class NLogViewerParameterInfo
  {
    [RequiredParameter]
    public string Name { get; set; }

    [RequiredParameter]
    public Layout Layout { get; set; }
  }
}
