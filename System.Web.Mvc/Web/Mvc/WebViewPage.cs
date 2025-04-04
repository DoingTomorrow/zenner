// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.WebViewPage
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Globalization;
using System.IO;
using System.Web.Mvc.Properties;
using System.Web.WebPages;

#nullable disable
namespace System.Web.Mvc
{
  public abstract class WebViewPage : WebPageBase, IViewDataContainer, IViewStartPageChild
  {
    private ViewDataDictionary _viewData;
    private DynamicViewDataDictionary _dynamicViewData;
    private HttpContextBase _context;

    public AjaxHelper<object> Ajax { get; set; }

    public override HttpContextBase Context
    {
      get => this._context ?? this.ViewContext.HttpContext;
      set => this._context = value;
    }

    public HtmlHelper<object> Html { get; set; }

    public object Model => this.ViewData.Model;

    internal string OverridenLayoutPath { get; set; }

    public TempDataDictionary TempData => this.ViewContext.TempData;

    public UrlHelper Url { get; set; }

    public object ViewBag
    {
      get
      {
        if (this._dynamicViewData == null)
          this._dynamicViewData = new DynamicViewDataDictionary((Func<ViewDataDictionary>) (() => this.ViewData));
        return (object) this._dynamicViewData;
      }
    }

    public ViewContext ViewContext { get; set; }

    public ViewDataDictionary ViewData
    {
      get
      {
        if (this._viewData == null)
          this.SetViewData(new ViewDataDictionary());
        return this._viewData;
      }
      set => this.SetViewData(value);
    }

    protected override void ConfigurePage(WebPageBase parentPage)
    {
      this.ViewContext = parentPage is WebViewPage webViewPage ? webViewPage.ViewContext : throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.CshtmlView_WrongViewBase, new object[1]
      {
        (object) parentPage.VirtualPath
      }));
      this.ViewData = webViewPage.ViewData;
      this.InitHelpers();
    }

    public override void ExecutePageHierarchy()
    {
      TextWriter writer = this.ViewContext.Writer;
      this.ViewContext.Writer = this.Output;
      base.ExecutePageHierarchy();
      if (!string.IsNullOrEmpty(this.OverridenLayoutPath))
        this.Layout = this.OverridenLayoutPath;
      this.ViewContext.Writer = writer;
    }

    public virtual void InitHelpers()
    {
      this.Ajax = new AjaxHelper<object>(this.ViewContext, (IViewDataContainer) this);
      this.Html = new HtmlHelper<object>(this.ViewContext, (IViewDataContainer) this);
      this.Url = new UrlHelper(this.ViewContext.RequestContext);
    }

    protected virtual void SetViewData(ViewDataDictionary viewData) => this._viewData = viewData;
  }
}
