// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.ApplicationParts.ResourceAssembly
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Generic;
using System.IO;
using System.Reflection;

#nullable disable
namespace System.Web.WebPages.ApplicationParts
{
  internal class ResourceAssembly : IResourceAssembly
  {
    private readonly Assembly _assembly;

    public ResourceAssembly(Assembly assembly)
    {
      this._assembly = !(assembly == (Assembly) null) ? assembly : throw new ArgumentNullException(nameof (assembly));
    }

    public string Name => new AssemblyName(this._assembly.FullName).Name;

    public Stream GetManifestResourceStream(string name)
    {
      return this._assembly.GetManifestResourceStream(name);
    }

    public IEnumerable<string> GetManifestResourceNames()
    {
      return (IEnumerable<string>) this._assembly.GetManifestResourceNames();
    }

    public IEnumerable<Type> GetTypes() => (IEnumerable<Type>) this._assembly.GetExportedTypes();

    public override bool Equals(object obj)
    {
      return obj is ResourceAssembly resourceAssembly && resourceAssembly._assembly.Equals((object) this._assembly);
    }

    public override int GetHashCode() => this._assembly.GetHashCode();

    public override string ToString() => this._assembly.ToString();
  }
}
