// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.TempDirLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Internal;
using System.IO;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("tempdir")]
  [AppDomainFixedOutput]
  [ThreadAgnostic]
  [ThreadSafe]
  public class TempDirLayoutRenderer : LayoutRenderer
  {
    private static string tempDir = Path.GetTempPath();

    public string File { get; set; }

    public string Dir { get; set; }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      string str = PathHelpers.CombinePaths(TempDirLayoutRenderer.tempDir, this.Dir, this.File);
      builder.Append(str);
    }
  }
}
