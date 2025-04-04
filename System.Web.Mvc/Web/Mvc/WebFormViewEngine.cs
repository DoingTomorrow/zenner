// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.WebFormViewEngine
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  public class WebFormViewEngine : BuildManagerViewEngine
  {
    public WebFormViewEngine()
      : this((IViewPageActivator) null)
    {
    }

    public WebFormViewEngine(IViewPageActivator viewPageActivator)
      : base(viewPageActivator)
    {
      this.MasterLocationFormats = new string[2]
      {
        "~/Views/{1}/{0}.master",
        "~/Views/Shared/{0}.master"
      };
      this.AreaMasterLocationFormats = new string[2]
      {
        "~/Areas/{2}/Views/{1}/{0}.master",
        "~/Areas/{2}/Views/Shared/{0}.master"
      };
      this.ViewLocationFormats = new string[4]
      {
        "~/Views/{1}/{0}.aspx",
        "~/Views/{1}/{0}.ascx",
        "~/Views/Shared/{0}.aspx",
        "~/Views/Shared/{0}.ascx"
      };
      this.AreaViewLocationFormats = new string[4]
      {
        "~/Areas/{2}/Views/{1}/{0}.aspx",
        "~/Areas/{2}/Views/{1}/{0}.ascx",
        "~/Areas/{2}/Views/Shared/{0}.aspx",
        "~/Areas/{2}/Views/Shared/{0}.ascx"
      };
      this.PartialViewLocationFormats = this.ViewLocationFormats;
      this.AreaPartialViewLocationFormats = this.AreaViewLocationFormats;
      this.FileExtensions = new string[3]
      {
        "aspx",
        "ascx",
        "master"
      };
    }

    protected override IView CreatePartialView(
      ControllerContext controllerContext,
      string partialPath)
    {
      return (IView) new WebFormView(controllerContext, partialPath, (string) null, this.ViewPageActivator);
    }

    protected override IView CreateView(
      ControllerContext controllerContext,
      string viewPath,
      string masterPath)
    {
      return (IView) new WebFormView(controllerContext, viewPath, masterPath, this.ViewPageActivator);
    }
  }
}
