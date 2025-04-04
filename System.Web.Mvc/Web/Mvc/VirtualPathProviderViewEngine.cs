// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.VirtualPathProviderViewEngine
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc.Properties;
using System.Web.WebPages;

#nullable disable
namespace System.Web.Mvc
{
  public abstract class VirtualPathProviderViewEngine : IViewEngine
  {
    private const string CacheKeyFormat = ":ViewCacheEntry:{0}:{1}:{2}:{3}:{4}:";
    private const string CacheKeyPrefixMaster = "Master";
    private const string CacheKeyPrefixPartial = "Partial";
    private const string CacheKeyPrefixView = "View";
    private static readonly string[] _emptyLocations = new string[0];
    private DisplayModeProvider _displayModeProvider;
    private VirtualPathProvider _vpp;
    internal Func<string, string> GetExtensionThunk = new Func<string, string>(VirtualPathUtility.GetExtension);

    protected VirtualPathProviderViewEngine()
    {
      if (HttpContext.Current == null || HttpContext.Current.IsDebuggingEnabled)
        this.ViewLocationCache = DefaultViewLocationCache.Null;
      else
        this.ViewLocationCache = (IViewLocationCache) new DefaultViewLocationCache();
    }

    public string[] AreaMasterLocationFormats { get; set; }

    public string[] AreaPartialViewLocationFormats { get; set; }

    public string[] AreaViewLocationFormats { get; set; }

    public string[] FileExtensions { get; set; }

    public string[] MasterLocationFormats { get; set; }

    public string[] PartialViewLocationFormats { get; set; }

    public IViewLocationCache ViewLocationCache { get; set; }

    public string[] ViewLocationFormats { get; set; }

    protected VirtualPathProvider VirtualPathProvider
    {
      get
      {
        if (this._vpp == null)
          this._vpp = HostingEnvironment.VirtualPathProvider;
        return this._vpp;
      }
      set => this._vpp = value;
    }

    protected internal DisplayModeProvider DisplayModeProvider
    {
      get => this._displayModeProvider ?? DisplayModeProvider.Instance;
      set => this._displayModeProvider = value;
    }

    private string CreateCacheKey(
      string prefix,
      string name,
      string controllerName,
      string areaName)
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, ":ViewCacheEntry:{0}:{1}:{2}:{3}:{4}:", (object) this.GetType().AssemblyQualifiedName, (object) prefix, (object) name, (object) controllerName, (object) areaName);
    }

    internal static string AppendDisplayModeToCacheKey(string cacheKey, string displayMode)
    {
      return cacheKey + displayMode + ":";
    }

    protected abstract IView CreatePartialView(
      ControllerContext controllerContext,
      string partialPath);

    protected abstract IView CreateView(
      ControllerContext controllerContext,
      string viewPath,
      string masterPath);

    protected virtual bool FileExists(ControllerContext controllerContext, string virtualPath)
    {
      return this.VirtualPathProvider.FileExists(virtualPath);
    }

    public virtual ViewEngineResult FindPartialView(
      ControllerContext controllerContext,
      string partialViewName,
      bool useCache)
    {
      if (controllerContext == null)
        throw new ArgumentNullException(nameof (controllerContext));
      if (string.IsNullOrEmpty(partialViewName))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (partialViewName));
      string requiredString = controllerContext.RouteData.GetRequiredString("controller");
      string[] searchedLocations;
      string path = this.GetPath(controllerContext, this.PartialViewLocationFormats, this.AreaPartialViewLocationFormats, "PartialViewLocationFormats", partialViewName, requiredString, "Partial", useCache, out searchedLocations);
      return string.IsNullOrEmpty(path) ? new ViewEngineResult((IEnumerable<string>) searchedLocations) : new ViewEngineResult(this.CreatePartialView(controllerContext, path), (IViewEngine) this);
    }

    public virtual ViewEngineResult FindView(
      ControllerContext controllerContext,
      string viewName,
      string masterName,
      bool useCache)
    {
      if (controllerContext == null)
        throw new ArgumentNullException(nameof (controllerContext));
      if (string.IsNullOrEmpty(viewName))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (viewName));
      string requiredString = controllerContext.RouteData.GetRequiredString("controller");
      string[] searchedLocations1;
      string path1 = this.GetPath(controllerContext, this.ViewLocationFormats, this.AreaViewLocationFormats, "ViewLocationFormats", viewName, requiredString, "View", useCache, out searchedLocations1);
      string[] searchedLocations2;
      string path2 = this.GetPath(controllerContext, this.MasterLocationFormats, this.AreaMasterLocationFormats, "MasterLocationFormats", masterName, requiredString, "Master", useCache, out searchedLocations2);
      return string.IsNullOrEmpty(path1) || string.IsNullOrEmpty(path2) && !string.IsNullOrEmpty(masterName) ? new ViewEngineResult(((IEnumerable<string>) searchedLocations1).Union<string>((IEnumerable<string>) searchedLocations2)) : new ViewEngineResult(this.CreateView(controllerContext, path1, path2), (IViewEngine) this);
    }

    private string GetPath(
      ControllerContext controllerContext,
      string[] locations,
      string[] areaLocations,
      string locationsPropertyName,
      string name,
      string controllerName,
      string cacheKeyPrefix,
      bool useCache,
      out string[] searchedLocations)
    {
      searchedLocations = VirtualPathProviderViewEngine._emptyLocations;
      if (string.IsNullOrEmpty(name))
        return string.Empty;
      string areaName = AreaHelpers.GetAreaName(controllerContext.RouteData);
      bool flag1 = !string.IsNullOrEmpty(areaName);
      List<VirtualPathProviderViewEngine.ViewLocation> viewLocations = VirtualPathProviderViewEngine.GetViewLocations(locations, flag1 ? areaLocations : (string[]) null);
      if (viewLocations.Count == 0)
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.Common_PropertyCannotBeNullOrEmpty, new object[1]
        {
          (object) locationsPropertyName
        }));
      bool flag2 = VirtualPathProviderViewEngine.IsSpecificPath(name);
      string cacheKey = this.CreateCacheKey(cacheKeyPrefix, name, flag2 ? string.Empty : controllerName, areaName);
      if (useCache)
      {
        foreach (IDisplayMode displayMode in this.DisplayModeProvider.GetAvailableDisplayModesForContext(controllerContext.HttpContext, controllerContext.DisplayMode))
        {
          string viewLocation = this.ViewLocationCache.GetViewLocation(controllerContext.HttpContext, VirtualPathProviderViewEngine.AppendDisplayModeToCacheKey(cacheKey, displayMode.DisplayModeId));
          if (viewLocation != null)
          {
            if (controllerContext.DisplayMode == null)
              controllerContext.DisplayMode = displayMode;
            return viewLocation;
          }
        }
        return (string) null;
      }
      return !flag2 ? this.GetPathFromGeneralName(controllerContext, viewLocations, name, controllerName, areaName, cacheKey, ref searchedLocations) : this.GetPathFromSpecificName(controllerContext, name, cacheKey, ref searchedLocations);
    }

    private string GetPathFromGeneralName(
      ControllerContext controllerContext,
      List<VirtualPathProviderViewEngine.ViewLocation> locations,
      string name,
      string controllerName,
      string areaName,
      string cacheKey,
      ref string[] searchedLocations)
    {
      string virtualPath1 = string.Empty;
      searchedLocations = new string[locations.Count];
      for (int index = 0; index < locations.Count; ++index)
      {
        string virtualPath2 = locations[index].Format(name, controllerName, areaName);
        DisplayInfo infoForVirtualPath = this.DisplayModeProvider.GetDisplayInfoForVirtualPath(virtualPath2, controllerContext.HttpContext, (Func<string, bool>) (path => this.FileExists(controllerContext, path)), controllerContext.DisplayMode);
        if (infoForVirtualPath != null)
        {
          string filePath = infoForVirtualPath.FilePath;
          searchedLocations = VirtualPathProviderViewEngine._emptyLocations;
          virtualPath1 = filePath;
          this.ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, VirtualPathProviderViewEngine.AppendDisplayModeToCacheKey(cacheKey, infoForVirtualPath.DisplayMode.DisplayModeId), virtualPath1);
          if (controllerContext.DisplayMode == null)
            controllerContext.DisplayMode = infoForVirtualPath.DisplayMode;
          using (IEnumerator<IDisplayMode> enumerator = this.DisplayModeProvider.Modes.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              IDisplayMode current = enumerator.Current;
              if (current.DisplayModeId != infoForVirtualPath.DisplayMode.DisplayModeId)
              {
                DisplayInfo displayInfo = current.GetDisplayInfo(controllerContext.HttpContext, virtualPath2, (Func<string, bool>) (path => this.FileExists(controllerContext, path)));
                if (displayInfo != null && displayInfo.FilePath != null)
                  this.ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, VirtualPathProviderViewEngine.AppendDisplayModeToCacheKey(cacheKey, displayInfo.DisplayMode.DisplayModeId), displayInfo.FilePath);
              }
            }
            break;
          }
        }
        else
          searchedLocations[index] = virtualPath2;
      }
      return virtualPath1;
    }

    private string GetPathFromSpecificName(
      ControllerContext controllerContext,
      string name,
      string cacheKey,
      ref string[] searchedLocations)
    {
      string virtualPath = name;
      if (!this.FilePathIsSupported(name) || !this.FileExists(controllerContext, name))
      {
        virtualPath = string.Empty;
        searchedLocations = new string[1]{ name };
      }
      this.ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, virtualPath);
      return virtualPath;
    }

    private bool FilePathIsSupported(string virtualPath)
    {
      if (this.FileExtensions == null)
        return true;
      return ((IEnumerable<string>) this.FileExtensions).Contains<string>(this.GetExtensionThunk(virtualPath).TrimStart('.'), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    }

    private static List<VirtualPathProviderViewEngine.ViewLocation> GetViewLocations(
      string[] viewLocationFormats,
      string[] areaViewLocationFormats)
    {
      List<VirtualPathProviderViewEngine.ViewLocation> viewLocations = new List<VirtualPathProviderViewEngine.ViewLocation>();
      if (areaViewLocationFormats != null)
      {
        foreach (string viewLocationFormat in areaViewLocationFormats)
          viewLocations.Add((VirtualPathProviderViewEngine.ViewLocation) new VirtualPathProviderViewEngine.AreaAwareViewLocation(viewLocationFormat));
      }
      if (viewLocationFormats != null)
      {
        foreach (string viewLocationFormat in viewLocationFormats)
          viewLocations.Add(new VirtualPathProviderViewEngine.ViewLocation(viewLocationFormat));
      }
      return viewLocations;
    }

    private static bool IsSpecificPath(string name)
    {
      char ch = name[0];
      return ch == '~' || ch == '/';
    }

    public virtual void ReleaseView(ControllerContext controllerContext, IView view)
    {
      if (!(view is IDisposable disposable))
        return;
      disposable.Dispose();
    }

    private class ViewLocation
    {
      protected string _virtualPathFormatString;

      public ViewLocation(string virtualPathFormatString)
      {
        this._virtualPathFormatString = virtualPathFormatString;
      }

      public virtual string Format(string viewName, string controllerName, string areaName)
      {
        return string.Format((IFormatProvider) CultureInfo.InvariantCulture, this._virtualPathFormatString, new object[2]
        {
          (object) viewName,
          (object) controllerName
        });
      }
    }

    private class AreaAwareViewLocation(string virtualPathFormatString) : 
      VirtualPathProviderViewEngine.ViewLocation(virtualPathFormatString)
    {
      public override string Format(string viewName, string controllerName, string areaName)
      {
        return string.Format((IFormatProvider) CultureInfo.InvariantCulture, this._virtualPathFormatString, new object[3]
        {
          (object) viewName,
          (object) controllerName,
          (object) areaName
        });
      }
    }
  }
}
