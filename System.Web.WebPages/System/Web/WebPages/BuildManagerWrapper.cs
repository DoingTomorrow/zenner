// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.BuildManagerWrapper
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using Microsoft.Internal.Web.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Web.Caching;
using System.Web.Compilation;
using System.Web.Hosting;
using System.Web.Util;
using System.Xml.Linq;

#nullable disable
namespace System.Web.WebPages
{
  internal sealed class BuildManagerWrapper : IVirtualPathFactory
  {
    internal static readonly Guid KeyGuid = Guid.NewGuid();
    private static readonly TimeSpan _objectFactoryCacheDuration = TimeSpan.FromMinutes(1.0);
    private readonly IVirtualPathUtility _virtualPathUtility;
    private readonly VirtualPathProvider _vpp;
    private readonly bool _isPrecompiled;
    private readonly FileExistenceCache _vppCache;
    private IEnumerable<string> _supportedExtensions;

    public BuildManagerWrapper()
      : this(HostingEnvironment.VirtualPathProvider, (IVirtualPathUtility) new VirtualPathUtilityWrapper())
    {
    }

    public BuildManagerWrapper(VirtualPathProvider vpp, IVirtualPathUtility virtualPathUtility)
    {
      this._vpp = vpp;
      this._virtualPathUtility = virtualPathUtility;
      this._isPrecompiled = this.IsNonUpdatablePrecompiledApp();
      if (this._isPrecompiled)
        return;
      this._vppCache = new FileExistenceCache(vpp);
    }

    public IEnumerable<string> SupportedExtensions
    {
      get
      {
        return this._supportedExtensions ?? (IEnumerable<string>) WebPageHttpHandler.GetRegisteredExtensions();
      }
      set => this._supportedExtensions = value;
    }

    public bool Exists(string virtualPath)
    {
      return this._isPrecompiled ? this.ExistsInPrecompiledSite(virtualPath) : this.ExistsInVpp(virtualPath);
    }

    internal bool IsNonUpdatablePrecompiledApp()
    {
      if (this._vpp == null)
        return false;
      string absolute = this._virtualPathUtility.ToAbsolute("~/PrecompiledApp.config");
      if (!this._vpp.FileExists(absolute))
        return false;
      XDocument xdocument;
      using (this._vpp.GetFile(absolute).Open())
      {
        try
        {
          xdocument = XDocument.Load(this._vpp.GetFile(absolute).Open());
        }
        catch
        {
          return false;
        }
      }
      if (xdocument.Root == null || !xdocument.Root.Name.LocalName.Equals("precompiledApp", StringComparison.OrdinalIgnoreCase))
        return false;
      XAttribute xattribute = xdocument.Root.Attribute((XName) "updatable");
      bool result;
      return xattribute != null && bool.TryParse(xattribute.Value, out result) && !result;
    }

    private bool ExistsInPrecompiledSite(string virtualPath)
    {
      string keyFromVirtualPath = BuildManagerWrapper.GetKeyFromVirtualPath(virtualPath);
      BuildManagerWrapper.BuildManagerResult buildManagerResult = (BuildManagerWrapper.BuildManagerResult) HttpRuntime.Cache.Get(keyFromVirtualPath);
      if (buildManagerResult == null)
      {
        IWebObjectFactory objectFactory = this.GetObjectFactory(virtualPath);
        buildManagerResult = new BuildManagerWrapper.BuildManagerResult()
        {
          ObjectFactory = objectFactory,
          Exists = objectFactory != null
        };
        HttpRuntime.Cache.Add(keyFromVirtualPath, (object) buildManagerResult, (CacheDependency) null, Cache.NoAbsoluteExpiration, BuildManagerWrapper._objectFactoryCacheDuration, CacheItemPriority.Low, (CacheItemRemovedCallback) null);
      }
      return buildManagerResult.Exists;
    }

    private bool ExistsInVpp(string virtualPath) => this._vppCache.FileExists(virtualPath);

    private IWebObjectFactory GetObjectFactory(string virtualPath)
    {
      if (!this.IsPathExtensionSupported(virtualPath))
        return (IWebObjectFactory) null;
      bool throwIfNotFound = false;
      return BuildManager.GetObjectFactory(virtualPath, throwIfNotFound);
    }

    public object CreateInstance(string virtualPath)
    {
      return this.CreateInstanceOfType<object>(virtualPath);
    }

    public T CreateInstanceOfType<T>(string virtualPath) where T : class
    {
      if (this._isPrecompiled)
      {
        BuildManagerWrapper.BuildManagerResult buildManagerResult = (BuildManagerWrapper.BuildManagerResult) HttpRuntime.Cache.Get(BuildManagerWrapper.GetKeyFromVirtualPath(virtualPath));
        if (buildManagerResult != null)
          return buildManagerResult.ObjectFactory.CreateInstance() as T;
      }
      return (T) BuildManager.CreateInstanceFromVirtualPath(virtualPath, typeof (T));
    }

    public bool IsPathExtensionSupported(string virtualPath)
    {
      string extension = PathUtil.GetExtension(virtualPath);
      return !string.IsNullOrEmpty(extension) && this.SupportedExtensions.Contains<string>(extension.Substring(1), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    }

    private static string GetKeyFromVirtualPath(string virtualPath)
    {
      return BuildManagerWrapper.KeyGuid.ToString() + "_" + virtualPath;
    }

    private class BuildManagerResult
    {
      public bool Exists { get; set; }

      public IWebObjectFactory ObjectFactory { get; set; }
    }
  }
}
