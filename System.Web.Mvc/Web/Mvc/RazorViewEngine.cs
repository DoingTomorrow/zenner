// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.RazorViewEngine
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;

#nullable disable
namespace System.Web.Mvc
{
  public class RazorViewEngine : BuildManagerViewEngine
  {
    internal static readonly string ViewStartFileName = "_ViewStart";

    public RazorViewEngine()
      : this((IViewPageActivator) null)
    {
    }

    public RazorViewEngine(IViewPageActivator viewPageActivator)
      : base(viewPageActivator)
    {
      this.AreaViewLocationFormats = new string[4]
      {
        "~/Areas/{2}/Views/{1}/{0}.cshtml",
        "~/Areas/{2}/Views/{1}/{0}.vbhtml",
        "~/Areas/{2}/Views/Shared/{0}.cshtml",
        "~/Areas/{2}/Views/Shared/{0}.vbhtml"
      };
      this.AreaMasterLocationFormats = new string[4]
      {
        "~/Areas/{2}/Views/{1}/{0}.cshtml",
        "~/Areas/{2}/Views/{1}/{0}.vbhtml",
        "~/Areas/{2}/Views/Shared/{0}.cshtml",
        "~/Areas/{2}/Views/Shared/{0}.vbhtml"
      };
      this.AreaPartialViewLocationFormats = new string[4]
      {
        "~/Areas/{2}/Views/{1}/{0}.cshtml",
        "~/Areas/{2}/Views/{1}/{0}.vbhtml",
        "~/Areas/{2}/Views/Shared/{0}.cshtml",
        "~/Areas/{2}/Views/Shared/{0}.vbhtml"
      };
      this.ViewLocationFormats = new string[4]
      {
        "~/Views/{1}/{0}.cshtml",
        "~/Views/{1}/{0}.vbhtml",
        "~/Views/Shared/{0}.cshtml",
        "~/Views/Shared/{0}.vbhtml"
      };
      this.MasterLocationFormats = new string[4]
      {
        "~/Views/{1}/{0}.cshtml",
        "~/Views/{1}/{0}.vbhtml",
        "~/Views/Shared/{0}.cshtml",
        "~/Views/Shared/{0}.vbhtml"
      };
      this.PartialViewLocationFormats = new string[4]
      {
        "~/Views/{1}/{0}.cshtml",
        "~/Views/{1}/{0}.vbhtml",
        "~/Views/Shared/{0}.cshtml",
        "~/Views/Shared/{0}.vbhtml"
      };
      this.FileExtensions = new string[2]
      {
        "cshtml",
        "vbhtml"
      };
    }

    protected override IView CreatePartialView(
      ControllerContext controllerContext,
      string partialPath)
    {
      return (IView) new RazorView(controllerContext, partialPath, (string) null, false, (IEnumerable<string>) this.FileExtensions, this.ViewPageActivator)
      {
        DisplayModeProvider = this.DisplayModeProvider
      };
    }

    protected override IView CreateView(
      ControllerContext controllerContext,
      string viewPath,
      string masterPath)
    {
      return (IView) new RazorView(controllerContext, viewPath, masterPath, true, (IEnumerable<string>) this.FileExtensions, this.ViewPageActivator)
      {
        DisplayModeProvider = this.DisplayModeProvider
      };
    }
  }
}
