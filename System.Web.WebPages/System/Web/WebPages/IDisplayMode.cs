// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.IDisplayMode
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

#nullable disable
namespace System.Web.WebPages
{
  public interface IDisplayMode
  {
    string DisplayModeId { get; }

    bool CanHandleContext(HttpContextBase httpContext);

    DisplayInfo GetDisplayInfo(
      HttpContextBase httpContext,
      string virtualPath,
      Func<string, bool> virtualPathExists);
  }
}
