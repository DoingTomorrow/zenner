// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.BaseDirLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Internal;
using NLog.Internal.Fakeables;
using System.Diagnostics;
using System.IO;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("basedir")]
  [AppDomainFixedOutput]
  [ThreadAgnostic]
  [ThreadSafe]
  public class BaseDirLayoutRenderer : LayoutRenderer
  {
    private readonly string _baseDir;
    private string _processDir;

    public bool ProcessDir { get; set; }

    public BaseDirLayoutRenderer()
      : this(LogFactory.CurrentAppDomain)
    {
    }

    public BaseDirLayoutRenderer(IAppDomain appDomain) => this._baseDir = appDomain.BaseDirectory;

    public string File { get; set; }

    public string Dir { get; set; }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      string path = this._baseDir;
      if (this.ProcessDir)
        path = this._processDir ?? (this._processDir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));
      if (path == null)
        return;
      string str = PathHelpers.CombinePaths(path, this.Dir, this.File);
      builder.Append(str);
    }
  }
}
