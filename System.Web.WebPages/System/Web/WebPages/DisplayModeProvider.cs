// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.DisplayModeProvider
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace System.Web.WebPages
{
  public sealed class DisplayModeProvider
  {
    public static readonly string MobileDisplayModeId = "Mobile";
    public static readonly string DefaultDisplayModeId = string.Empty;
    private static readonly object _displayModeKey = new object();
    private static readonly DisplayModeProvider _instance = new DisplayModeProvider();
    private readonly List<IDisplayMode> _displayModes = new List<IDisplayMode>()
    {
      (IDisplayMode) new DefaultDisplayMode(DisplayModeProvider.MobileDisplayModeId)
      {
        ContextCondition = (Func<HttpContextBase, bool>) (context => context.GetOverriddenBrowser().IsMobileDevice)
      },
      (IDisplayMode) new DefaultDisplayMode()
    };

    internal DisplayModeProvider()
    {
    }

    public bool RequireConsistentDisplayMode { get; set; }

    public static DisplayModeProvider Instance => DisplayModeProvider._instance;

    public IList<IDisplayMode> Modes => (IList<IDisplayMode>) this._displayModes;

    public IEnumerable<IDisplayMode> GetAvailableDisplayModesForContext(
      HttpContextBase httpContext,
      IDisplayMode currentDisplayMode)
    {
      return (IEnumerable<IDisplayMode>) this.GetAvailableDisplayModesForContext(httpContext, currentDisplayMode, this.RequireConsistentDisplayMode).ToList<IDisplayMode>();
    }

    internal IEnumerable<IDisplayMode> GetAvailableDisplayModesForContext(
      HttpContextBase httpContext,
      IDisplayMode currentDisplayMode,
      bool requireConsistentDisplayMode)
    {
      return (!requireConsistentDisplayMode || currentDisplayMode == null ? (IEnumerable<IDisplayMode>) this.Modes : this.Modes.SkipWhile<IDisplayMode>((Func<IDisplayMode, bool>) (mode => mode != currentDisplayMode))).Where<IDisplayMode>((Func<IDisplayMode, bool>) (mode => mode.CanHandleContext(httpContext)));
    }

    public DisplayInfo GetDisplayInfoForVirtualPath(
      string virtualPath,
      HttpContextBase httpContext,
      Func<string, bool> virtualPathExists,
      IDisplayMode currentDisplayMode)
    {
      return this.GetDisplayInfoForVirtualPath(virtualPath, httpContext, virtualPathExists, currentDisplayMode, this.RequireConsistentDisplayMode);
    }

    internal DisplayInfo GetDisplayInfoForVirtualPath(
      string virtualPath,
      HttpContextBase httpContext,
      Func<string, bool> virtualPathExists,
      IDisplayMode currentDisplayMode,
      bool requireConsistentDisplayMode)
    {
      return this.GetAvailableDisplayModesForContext(httpContext, currentDisplayMode, requireConsistentDisplayMode).Select<IDisplayMode, DisplayInfo>((Func<IDisplayMode, DisplayInfo>) (mode => mode.GetDisplayInfo(httpContext, virtualPath, virtualPathExists))).FirstOrDefault<DisplayInfo>((Func<DisplayInfo, bool>) (info => info != null));
    }

    internal static IDisplayMode GetDisplayMode(HttpContextBase context)
    {
      return context == null ? (IDisplayMode) null : context.Items[DisplayModeProvider._displayModeKey] as IDisplayMode;
    }

    internal static void SetDisplayMode(HttpContextBase context, IDisplayMode displayMode)
    {
      if (context == null)
        return;
      context.Items[DisplayModeProvider._displayModeKey] = (object) displayMode;
    }
  }
}
