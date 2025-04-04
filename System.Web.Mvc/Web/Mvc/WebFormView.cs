// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.WebFormView
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Globalization;
using System.IO;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public class WebFormView : BuildManagerCompiledView
  {
    public WebFormView(ControllerContext controllerContext, string viewPath)
      : this(controllerContext, viewPath, (string) null, (IViewPageActivator) null)
    {
    }

    public WebFormView(ControllerContext controllerContext, string viewPath, string masterPath)
      : this(controllerContext, viewPath, masterPath, (IViewPageActivator) null)
    {
    }

    public WebFormView(
      ControllerContext controllerContext,
      string viewPath,
      string masterPath,
      IViewPageActivator viewPageActivator)
      : base(controllerContext, viewPath, viewPageActivator)
    {
      this.MasterPath = masterPath ?? string.Empty;
    }

    public string MasterPath { get; private set; }

    protected override void RenderView(ViewContext viewContext, TextWriter writer, object instance)
    {
      switch (instance)
      {
        case ViewPage page:
          this.RenderViewPage(viewContext, page);
          break;
        case ViewUserControl control:
          this.RenderViewUserControl(viewContext, control);
          break;
        default:
          throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.WebFormViewEngine_WrongViewBase, new object[1]
          {
            (object) this.ViewPath
          }));
      }
    }

    private void RenderViewPage(ViewContext context, ViewPage page)
    {
      if (!string.IsNullOrEmpty(this.MasterPath))
        page.MasterLocation = this.MasterPath;
      page.ViewData = context.ViewData;
      page.RenderView(context);
    }

    private void RenderViewUserControl(ViewContext context, ViewUserControl control)
    {
      if (!string.IsNullOrEmpty(this.MasterPath))
        throw new InvalidOperationException(MvcResources.WebFormViewEngine_UserControlCannotHaveMaster);
      control.ViewData = context.ViewData;
      control.RenderView(context);
    }
  }
}
