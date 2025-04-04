// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.AssemblyVersionLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Internal;
using System.ComponentModel;
using System.Reflection;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("assembly-version")]
  [ThreadAgnostic]
  [ThreadSafe]
  public class AssemblyVersionLayoutRenderer : LayoutRenderer
  {
    private string _assemblyVersion;

    public AssemblyVersionLayoutRenderer() => this.Type = AssemblyVersionType.Assembly;

    [DefaultParameter]
    public string Name { get; set; }

    [DefaultValue("Assembly")]
    public AssemblyVersionType Type { get; set; }

    protected override void InitializeLayoutRenderer()
    {
      this._assemblyVersion = (string) null;
      base.InitializeLayoutRenderer();
    }

    protected override void CloseLayoutRenderer()
    {
      this._assemblyVersion = (string) null;
      base.CloseLayoutRenderer();
    }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      string str = this._assemblyVersion ?? (this._assemblyVersion = this.GetVersion());
      if (string.IsNullOrEmpty(str))
        str = string.Format("Could not find value for {0} assembly and version type {1}", string.IsNullOrEmpty(this.Name) ? (object) "entry" : (object) this.Name, (object) this.Type);
      builder.Append(str);
    }

    private string GetVersion() => this.GetVersion(this.GetAssembly());

    private Assembly GetAssembly()
    {
      return string.IsNullOrEmpty(this.Name) ? Assembly.GetEntryAssembly() : Assembly.Load(new AssemblyName(this.Name));
    }

    private string GetVersion(Assembly assembly)
    {
      switch (this.Type)
      {
        case AssemblyVersionType.File:
          if ((object) assembly == null)
            return (string) null;
          return ReflectionHelpers.GetCustomAttribute<AssemblyFileVersionAttribute>(assembly)?.Version;
        case AssemblyVersionType.Informational:
          if ((object) assembly == null)
            return (string) null;
          return ReflectionHelpers.GetCustomAttribute<AssemblyInformationalVersionAttribute>(assembly)?.InformationalVersion;
        default:
          return assembly?.GetName().Version?.ToString();
      }
    }
  }
}
