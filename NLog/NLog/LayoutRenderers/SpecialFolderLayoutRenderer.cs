// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.SpecialFolderLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("specialfolder")]
  [AppDomainFixedOutput]
  [ThreadSafe]
  public class SpecialFolderLayoutRenderer : LayoutRenderer
  {
    [DefaultParameter]
    public Environment.SpecialFolder Folder { get; set; }

    public string File { get; set; }

    public string Dir { get; set; }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      string str = PathHelpers.CombinePaths(Environment.GetFolderPath(this.Folder), this.Dir, this.File);
      builder.Append(str);
    }
  }
}
