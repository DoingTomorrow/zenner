// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.WebPageRoute
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Web.WebPages.Deployment;
using System.Web.WebPages.Resources;

#nullable disable
namespace System.Web.WebPages
{
  internal sealed class WebPageRoute
  {
    private static readonly Lazy<bool> _isRootExplicitlyDisabled = new Lazy<bool>((Func<bool>) (() => WebPagesDeployment.IsExplicitlyDisabled("~/")));
    private IVirtualPathFactory _virtualPathFactory;
    private bool? _isExplicitlyDisabled;

    internal IVirtualPathFactory VirtualPathFactory
    {
      get => this._virtualPathFactory ?? (IVirtualPathFactory) VirtualPathFactoryManager.Instance;
      set => this._virtualPathFactory = value;
    }

    internal bool IsExplicitlyDisabled
    {
      get => this._isExplicitlyDisabled ?? WebPageRoute._isRootExplicitlyDisabled.Value;
      set => this._isExplicitlyDisabled = new bool?(value);
    }

    internal void DoPostResolveRequestCache(HttpContextBase context)
    {
      if (this.IsExplicitlyDisabled)
        return;
      string str1 = context.Request.AppRelativeCurrentExecutionFilePath.Substring(2) + context.Request.PathInfo;
      ReadOnlyCollection<string> registeredExtensions = WebPageHttpHandler.GetRegisteredExtensions();
      WebPageMatch webPageMatch = WebPageRoute.MatchRequest(str1, (IEnumerable<string>) registeredExtensions, this.VirtualPathFactory, context, DisplayModeProvider.Instance);
      if (webPageMatch != null)
      {
        context.Items[(object) typeof (WebPageMatch)] = (object) webPageMatch;
        string str2 = "~/" + webPageMatch.MatchedPath;
        if (WebPagesDeployment.IsExplicitlyDisabled(str2))
          return;
        IHttpHandler fromVirtualPath = WebPageHttpHandler.CreateFromVirtualPath(str2);
        if (fromVirtualPath == null)
          return;
        SessionStateUtil.SetUpSessionState(context, fromVirtualPath);
        context.RemapHandler(fromVirtualPath);
      }
      else
      {
        string extension = PathUtil.GetExtension(str1);
        foreach (string str3 in registeredExtensions)
        {
          if (string.Equals("." + str3, extension, StringComparison.OrdinalIgnoreCase))
            throw new HttpException(404, (string) null);
        }
      }
    }

    private static bool FileExists(string virtualPath, IVirtualPathFactory virtualPathFactory)
    {
      string virtualPath1 = "~/" + virtualPath;
      return virtualPathFactory.Exists(virtualPath1);
    }

    internal static WebPageMatch GetWebPageMatch(HttpContextBase context)
    {
      return (WebPageMatch) context.Items[(object) typeof (WebPageMatch)];
    }

    private static string GetRouteLevelMatch(
      string pathValue,
      IEnumerable<string> supportedExtensions,
      IVirtualPathFactory virtualPathFactory,
      HttpContextBase context,
      DisplayModeProvider displayModeProvider)
    {
      foreach (string supportedExtension in supportedExtensions)
      {
        string str = "~/" + pathValue;
        if (!str.EndsWith("." + supportedExtension, StringComparison.OrdinalIgnoreCase))
          str = str + "." + supportedExtension;
        DisplayModeProvider displayModeProvider1 = displayModeProvider;
        IDisplayMode displayMode = (IDisplayMode) null;
        string virtualPath = str;
        HttpContextBase httpContext = context;
        Func<string, bool> virtualPathExists = new Func<string, bool>(virtualPathFactory.Exists);
        IDisplayMode currentDisplayMode = displayMode;
        DisplayInfo infoForVirtualPath = displayModeProvider1.GetDisplayInfoForVirtualPath(virtualPath, httpContext, virtualPathExists, currentDisplayMode);
        if (infoForVirtualPath != null)
        {
          string routeLevelMatch = !Path.GetFileName(infoForVirtualPath.FilePath).StartsWith("_", StringComparison.OrdinalIgnoreCase) ? infoForVirtualPath.FilePath : throw new HttpException(404, WebPageResources.WebPageRoute_UnderscoreBlocked);
          if (routeLevelMatch.StartsWith("~/", StringComparison.OrdinalIgnoreCase))
            routeLevelMatch = routeLevelMatch.Remove(0, 2);
          DisplayModeProvider.SetDisplayMode(context, infoForVirtualPath.DisplayMode);
          return routeLevelMatch;
        }
      }
      return (string) null;
    }

    internal static WebPageMatch MatchRequest(
      string pathValue,
      IEnumerable<string> supportedExtensions,
      IVirtualPathFactory virtualPathFactory,
      HttpContextBase context,
      DisplayModeProvider displayModes)
    {
      string str = string.Empty;
      if (!string.IsNullOrEmpty(pathValue))
      {
        if (WebPageRoute.FileExists(pathValue, virtualPathFactory))
        {
          bool flag = false;
          foreach (string supportedExtension in supportedExtensions)
          {
            if (pathValue.EndsWith("." + supportedExtension, StringComparison.OrdinalIgnoreCase))
            {
              flag = true;
              break;
            }
          }
          if (!flag)
            return (WebPageMatch) null;
        }
        str = pathValue;
        string pathInfo = string.Empty;
        string routeLevelMatch;
        while (true)
        {
          routeLevelMatch = WebPageRoute.GetRouteLevelMatch(str, supportedExtensions, virtualPathFactory, context, displayModes);
          if (routeLevelMatch == null)
          {
            int length = str.LastIndexOf('/');
            if (length != -1)
            {
              str = str.Substring(0, length);
              pathInfo = pathValue.Substring(length + 1);
            }
            else
              goto label_17;
          }
          else
            break;
        }
        return new WebPageMatch(routeLevelMatch, pathInfo);
      }
label_17:
      return WebPageRoute.MatchDefaultFiles(pathValue, supportedExtensions, virtualPathFactory, context, displayModes, str);
    }

    private static WebPageMatch MatchDefaultFiles(
      string pathValue,
      IEnumerable<string> supportedExtensions,
      IVirtualPathFactory virtualPathFactory,
      HttpContextBase context,
      DisplayModeProvider displayModes,
      string currentLevel)
    {
      currentLevel = pathValue;
      string pathValue1;
      string pathValue2;
      if (string.IsNullOrEmpty(currentLevel))
      {
        pathValue1 = "default";
        pathValue2 = "index";
      }
      else
      {
        if (currentLevel[currentLevel.Length - 1] != '/')
          currentLevel += "/";
        pathValue1 = currentLevel + "default";
        pathValue2 = currentLevel + "index";
      }
      string routeLevelMatch1 = WebPageRoute.GetRouteLevelMatch(pathValue1, supportedExtensions, virtualPathFactory, context, displayModes);
      if (routeLevelMatch1 != null)
        return new WebPageMatch(routeLevelMatch1, string.Empty);
      string routeLevelMatch2 = WebPageRoute.GetRouteLevelMatch(pathValue2, supportedExtensions, virtualPathFactory, context, displayModes);
      return routeLevelMatch2 != null ? new WebPageMatch(routeLevelMatch2, string.Empty) : (WebPageMatch) null;
    }
  }
}
