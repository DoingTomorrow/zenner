// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.NLogDirLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.IO;
using System.Reflection;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("nlogdir")]
  [AppDomainFixedOutput]
  [ThreadAgnostic]
  [ThreadSafe]
  public class NLogDirLayoutRenderer : LayoutRenderer
  {
    private string _nlogCombinedPath;

    static NLogDirLayoutRenderer()
    {
      Assembly assembly = typeof (LogManager).GetAssembly();
      NLogDirLayoutRenderer.NLogDir = Path.GetDirectoryName(!string.IsNullOrEmpty(assembly.Location) ? assembly.Location : new Uri(assembly.CodeBase).LocalPath);
    }

    public string File { get; set; }

    public string Dir { get; set; }

    private static string NLogDir { get; set; }

    protected override void InitializeLayoutRenderer()
    {
      this._nlogCombinedPath = (string) null;
      base.InitializeLayoutRenderer();
    }

    protected override void CloseLayoutRenderer()
    {
      this._nlogCombinedPath = (string) null;
      base.CloseLayoutRenderer();
    }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      string str = this._nlogCombinedPath ?? (this._nlogCombinedPath = PathHelpers.CombinePaths(NLogDirLayoutRenderer.NLogDir, this.Dir, this.File));
      builder.Append(str);
    }
  }
}
