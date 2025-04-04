// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.DefaultDisplayMode
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.IO;

#nullable disable
namespace System.Web.WebPages
{
  public class DefaultDisplayMode : IDisplayMode
  {
    private readonly string _suffix;

    public DefaultDisplayMode()
      : this(DisplayModeProvider.DefaultDisplayModeId)
    {
    }

    public DefaultDisplayMode(string suffix) => this._suffix = suffix ?? string.Empty;

    public Func<HttpContextBase, bool> ContextCondition { get; set; }

    public virtual string DisplayModeId => this._suffix;

    public bool CanHandleContext(HttpContextBase httpContext)
    {
      return this.ContextCondition == null || this.ContextCondition(httpContext);
    }

    public virtual DisplayInfo GetDisplayInfo(
      HttpContextBase httpContext,
      string virtualPath,
      Func<string, bool> virtualPathExists)
    {
      string filePath = this.TransformPath(virtualPath, this._suffix);
      return filePath != null && virtualPathExists(filePath) ? new DisplayInfo(filePath, (IDisplayMode) this) : (DisplayInfo) null;
    }

    protected virtual string TransformPath(string virtualPath, string suffix)
    {
      if (string.IsNullOrEmpty(suffix))
        return virtualPath;
      string extension = Path.GetExtension(virtualPath);
      return Path.ChangeExtension(virtualPath, suffix + extension);
    }
  }
}
