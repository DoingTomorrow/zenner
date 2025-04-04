// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.ApplicationPart
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using Microsoft.Internal.Web.Utils;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Routing;
using System.Web.WebPages.ApplicationParts;
using System.Web.WebPages.Resources;

#nullable disable
namespace System.Web.WebPages
{
  public class ApplicationPart
  {
    private const string ModuleRootSyntax = "@/";
    private const string ResourceVirtualPathRoot = "~/r.ashx/";
    private const string ResourceRoute = "r.ashx/{module}/{*path}";
    private static readonly LazyAction _initApplicationPart = new LazyAction(new Action(ApplicationPart.InitApplicationParts));
    private static ApplicationPartRegistry _partRegistry;
    private readonly Lazy<IDictionary<string, string>> _applicationPartResources;
    private readonly Lazy<string> _applicationPartName;

    public ApplicationPart(System.Reflection.Assembly assembly, string rootVirtualPath)
      : this((IResourceAssembly) new ResourceAssembly(assembly), rootVirtualPath)
    {
    }

    internal ApplicationPart(IResourceAssembly assembly, string rootVirtualPath)
    {
      if (string.IsNullOrEmpty(rootVirtualPath))
        throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (rootVirtualPath));
      if (!rootVirtualPath.EndsWith("/", StringComparison.Ordinal))
        rootVirtualPath += "/";
      this.Assembly = assembly;
      this.RootVirtualPath = rootVirtualPath;
      this._applicationPartResources = new Lazy<IDictionary<string, string>>((Func<IDictionary<string, string>>) (() => (IDictionary<string, string>) this.Assembly.GetManifestResourceNames().ToDictionary<string, string, string>((Func<string, string>) (key => key), (Func<string, string>) (key => key), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)));
      this._applicationPartName = new Lazy<string>((Func<string>) (() => this.Assembly.Name));
    }

    internal IResourceAssembly Assembly { get; private set; }

    internal string RootVirtualPath { get; private set; }

    internal string Name => this._applicationPartName.Value;

    internal IDictionary<string, string> ApplicationPartResources
    {
      get => this._applicationPartResources.Value;
    }

    public static void Register(ApplicationPart applicationPart)
    {
      ApplicationPart._initApplicationPart.EnsurePerformed();
      ApplicationPart._partRegistry.Register(applicationPart);
    }

    public static string ProcessVirtualPath(
      System.Reflection.Assembly assembly,
      string baseVirtualPath,
      string virtualPath)
    {
      return ((ApplicationPart._partRegistry != null ? ApplicationPart._partRegistry[(IResourceAssembly) new ResourceAssembly(assembly)] : throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, WebPageResources.ApplicationPart_ModuleNotRegistered, new object[1]
      {
        (object) assembly
      }))) ?? throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, WebPageResources.ApplicationPart_ModuleNotRegistered, new object[1]
      {
        (object) assembly
      }))).ProcessVirtualPath(baseVirtualPath, virtualPath);
    }

    internal static IEnumerable<ApplicationPart> GetRegisteredParts()
    {
      ApplicationPart._initApplicationPart.EnsurePerformed();
      return ApplicationPart._partRegistry.RegisteredParts;
    }

    private string ProcessVirtualPath(string baseVirtualPath, string virtualPath)
    {
      virtualPath = ApplicationPart.ResolveVirtualPath(this.RootVirtualPath, baseVirtualPath, virtualPath);
      return !virtualPath.StartsWith(this.RootVirtualPath, StringComparison.OrdinalIgnoreCase) || !this.ApplicationPartResources.ContainsKey(this.GetResourceNameFromVirtualPath("~/" + virtualPath.Substring(this.RootVirtualPath.Length))) ? virtualPath : this.GetResourceVirtualPath(virtualPath);
    }

    internal static string ResolveVirtualPath(
      string applicationRoot,
      string baseVirtualPath,
      string virtualPath)
    {
      return virtualPath.StartsWith("@/", StringComparison.OrdinalIgnoreCase) ? applicationRoot + virtualPath.Substring("@/".Length) : VirtualPathUtility.Combine(baseVirtualPath, virtualPath);
    }

    internal Stream GetResourceStream(string virtualPath)
    {
      string name;
      return this.ApplicationPartResources.TryGetValue(this.GetResourceNameFromVirtualPath(virtualPath), out name) ? this.Assembly.GetManifestResourceStream(name) : (Stream) null;
    }

    private string GetResourceNameFromVirtualPath(string virtualPath)
    {
      return ApplicationPart.GetResourceNameFromVirtualPath(this.Name, virtualPath);
    }

    internal static string GetResourceNameFromVirtualPath(string moduleName, string virtualPath)
    {
      if (!virtualPath.StartsWith("~/", StringComparison.Ordinal))
        virtualPath = "~/" + virtualPath;
      string str1 = VirtualPathUtility.GetDirectory(virtualPath);
      if (str1.Length >= 2)
        str1 = str1.Substring(2);
      string str2 = str1.Replace('/', '.').Replace(' ', '_');
      string fileName = Path.GetFileName(virtualPath);
      return moduleName + "." + str2 + fileName;
    }

    private string GetResourceVirtualPath(string virtualPath)
    {
      return ApplicationPart.GetResourceVirtualPath(this.Name, this.RootVirtualPath, virtualPath);
    }

    internal static string GetResourceVirtualPath(
      string moduleName,
      string moduleRoot,
      string virtualPath)
    {
      virtualPath = virtualPath.Substring(moduleRoot.Length).TrimStart('/');
      return "~/r.ashx/" + HttpUtility.UrlPathEncode(moduleName) + "/" + virtualPath;
    }

    private static void InitApplicationParts()
    {
      DictionaryBasedVirtualPathFactory pathFactory = new DictionaryBasedVirtualPathFactory();
      VirtualPathFactoryManager.RegisterVirtualPathFactory((IVirtualPathFactory) pathFactory);
      ApplicationPart._partRegistry = new ApplicationPartRegistry(pathFactory);
      RouteTable.Routes.Add((RouteBase) new Route("r.ashx/{module}/{*path}", (IRouteHandler) new ResourceRouteHandler(ApplicationPart._partRegistry)));
    }
  }
}
