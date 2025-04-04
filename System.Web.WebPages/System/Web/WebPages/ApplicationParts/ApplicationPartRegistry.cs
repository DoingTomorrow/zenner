// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.ApplicationParts.ApplicationPartRegistry
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web.WebPages.Resources;

#nullable disable
namespace System.Web.WebPages.ApplicationParts
{
  internal class ApplicationPartRegistry
  {
    private static readonly Type _webPageType = typeof (WebPageRenderingBase);
    private readonly DictionaryBasedVirtualPathFactory _virtualPathFactory;
    private readonly ConcurrentDictionary<string, bool> _registeredVirtualPaths;
    private readonly ConcurrentDictionary<IResourceAssembly, ApplicationPart> _applicationParts;

    public ApplicationPartRegistry(DictionaryBasedVirtualPathFactory pathFactory)
    {
      this._applicationParts = new ConcurrentDictionary<IResourceAssembly, ApplicationPart>();
      this._registeredVirtualPaths = new ConcurrentDictionary<string, bool>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      this._virtualPathFactory = pathFactory;
    }

    public IEnumerable<ApplicationPart> RegisteredParts
    {
      get => (IEnumerable<ApplicationPart>) this._applicationParts.Values;
    }

    public ApplicationPart this[string name]
    {
      get
      {
        return this._applicationParts.Values.FirstOrDefault<ApplicationPart>((Func<ApplicationPart, bool>) (appPart => appPart.Name.Equals(name, StringComparison.OrdinalIgnoreCase)));
      }
    }

    public ApplicationPart this[IResourceAssembly assembly]
    {
      get
      {
        ApplicationPart applicationPart;
        if (!this._applicationParts.TryGetValue(assembly, out applicationPart))
          applicationPart = (ApplicationPart) null;
        return applicationPart;
      }
    }

    public void Register(ApplicationPart applicationPart)
    {
      Func<object> registerPageAction = (Func<object>) null;
      this.Register(applicationPart, registerPageAction);
    }

    internal void Register(ApplicationPart applicationPart, Func<object> registerPageAction)
    {
      if (this._applicationParts.ContainsKey(applicationPart.Assembly))
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, WebPageResources.ApplicationPart_ModuleAlreadyRegistered, new object[1]
        {
          (object) applicationPart.Assembly
        }));
      if (this._registeredVirtualPaths.ContainsKey(applicationPart.RootVirtualPath))
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, WebPageResources.ApplicationPart_ModuleAlreadyRegisteredForVirtualPath, new object[1]
        {
          (object) applicationPart.RootVirtualPath
        }));
      if (!this._applicationParts.TryAdd(applicationPart.Assembly, applicationPart))
        return;
      this._registeredVirtualPaths.TryAdd(applicationPart.RootVirtualPath, true);
      foreach (Type webPageType in applicationPart.Assembly.GetTypes().Where<Type>((Func<Type, bool>) (type => type.IsSubclassOf(ApplicationPartRegistry._webPageType))))
        this.RegisterWebPage(applicationPart, webPageType, registerPageAction);
    }

    private void RegisterWebPage(
      ApplicationPart module,
      Type webPageType,
      Func<object> registerPageAction)
    {
      PageVirtualPathAttribute virtualPathAttribute = webPageType.GetCustomAttributes(typeof (PageVirtualPathAttribute), false).Cast<PageVirtualPathAttribute>().SingleOrDefault<PageVirtualPathAttribute>();
      if (virtualPathAttribute == null)
        return;
      this._virtualPathFactory.RegisterPath(ApplicationPartRegistry.GetRootRelativeVirtualPath(module.RootVirtualPath, virtualPathAttribute.VirtualPath), registerPageAction ?? ApplicationPartRegistry.NewTypeInstance(webPageType));
    }

    private static Func<object> NewTypeInstance(Type type)
    {
      return ((Expression<Func<object>>) (() => Expression.New(type))).Compile();
    }

    internal static string GetRootRelativeVirtualPath(
      string rootVirtualPath,
      string pageVirtualPath)
    {
      string relativePath = pageVirtualPath;
      if (relativePath.StartsWith("~/", StringComparison.Ordinal))
        relativePath = relativePath.Substring(2);
      if (!rootVirtualPath.EndsWith("/", StringComparison.OrdinalIgnoreCase))
        rootVirtualPath += "/";
      return VirtualPathUtility.Combine(rootVirtualPath, relativePath);
    }
  }
}
