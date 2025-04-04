// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ViewStartPage
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Web.Mvc.Properties;
using System.Web.WebPages;

#nullable disable
namespace System.Web.Mvc
{
  public abstract class ViewStartPage : StartPage, IViewStartPageChild
  {
    private IViewStartPageChild _viewStartPageChild;

    public HtmlHelper<object> Html => this.ViewStartPageChild.Html;

    public UrlHelper Url => this.ViewStartPageChild.Url;

    public ViewContext ViewContext => this.ViewStartPageChild.ViewContext;

    internal IViewStartPageChild ViewStartPageChild
    {
      get
      {
        if (this._viewStartPageChild == null)
          this._viewStartPageChild = this.ChildPage is IViewStartPageChild childPage ? childPage : throw new InvalidOperationException(MvcResources.ViewStartPage_RequiresMvcRazorView);
        return this._viewStartPageChild;
      }
    }
  }
}
