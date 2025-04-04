// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.RazorView
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc.Properties;
using System.Web.Mvc.Razor;
using System.Web.WebPages;

#nullable disable
namespace System.Web.Mvc
{
  public class RazorView : BuildManagerCompiledView
  {
    public RazorView(
      ControllerContext controllerContext,
      string viewPath,
      string layoutPath,
      bool runViewStartPages,
      IEnumerable<string> viewStartFileExtensions)
      : this(controllerContext, viewPath, layoutPath, runViewStartPages, viewStartFileExtensions, (IViewPageActivator) null)
    {
    }

    public RazorView(
      ControllerContext controllerContext,
      string viewPath,
      string layoutPath,
      bool runViewStartPages,
      IEnumerable<string> viewStartFileExtensions,
      IViewPageActivator viewPageActivator)
      : base(controllerContext, viewPath, viewPageActivator)
    {
      this.LayoutPath = layoutPath ?? string.Empty;
      this.RunViewStartPages = runViewStartPages;
      this.StartPageLookup = new StartPageLookupDelegate(StartPage.GetStartPage);
      this.ViewStartFileExtensions = viewStartFileExtensions ?? Enumerable.Empty<string>();
    }

    public string LayoutPath { get; private set; }

    public bool RunViewStartPages { get; private set; }

    internal StartPageLookupDelegate StartPageLookup { get; set; }

    internal IVirtualPathFactory VirtualPathFactory { get; set; }

    internal DisplayModeProvider DisplayModeProvider { get; set; }

    public IEnumerable<string> ViewStartFileExtensions { get; private set; }

    protected override void RenderView(ViewContext viewContext, TextWriter writer, object instance)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      if (!(instance is WebViewPage page))
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.CshtmlView_WrongViewBase, new object[1]
        {
          (object) this.ViewPath
        }));
      page.OverridenLayoutPath = this.LayoutPath;
      page.VirtualPath = this.ViewPath;
      page.ViewContext = viewContext;
      page.ViewData = viewContext.ViewData;
      page.InitHelpers();
      if (this.VirtualPathFactory != null)
        page.VirtualPathFactory = this.VirtualPathFactory;
      if (this.DisplayModeProvider != null)
        page.DisplayModeProvider = this.DisplayModeProvider;
      WebPageRenderingBase startPage = (WebPageRenderingBase) null;
      if (this.RunViewStartPages)
        startPage = this.StartPageLookup((WebPageRenderingBase) page, RazorViewEngine.ViewStartFileName, this.ViewStartFileExtensions);
      page.ExecutePageHierarchy(new WebPageContext(viewContext.HttpContext, (WebPageRenderingBase) null, (object) null), writer, startPage);
    }
  }
}
