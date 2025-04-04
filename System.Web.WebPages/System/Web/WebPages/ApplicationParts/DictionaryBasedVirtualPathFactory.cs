// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.ApplicationParts.DictionaryBasedVirtualPathFactory
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Generic;

#nullable disable
namespace System.Web.WebPages.ApplicationParts
{
  internal class DictionaryBasedVirtualPathFactory : IVirtualPathFactory
  {
    private Dictionary<string, Func<object>> _factories = new Dictionary<string, Func<object>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);

    internal void RegisterPath(string virtualPath, Func<object> factory)
    {
      this._factories[virtualPath] = factory;
    }

    public bool Exists(string virtualPath) => this._factories.ContainsKey(virtualPath);

    public object CreateInstance(string virtualPath) => this._factories[virtualPath]();
  }
}
