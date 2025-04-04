// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ViewMasterPage`1
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  public class ViewMasterPage<TModel> : ViewMasterPage
  {
    private AjaxHelper<TModel> _ajaxHelper;
    private HtmlHelper<TModel> _htmlHelper;
    private ViewDataDictionary<TModel> _viewData;

    public AjaxHelper<TModel> Ajax
    {
      get
      {
        if (this._ajaxHelper == null)
          this._ajaxHelper = new AjaxHelper<TModel>(this.ViewContext, (IViewDataContainer) this.ViewPage);
        return this._ajaxHelper;
      }
    }

    public HtmlHelper<TModel> Html
    {
      get
      {
        if (this._htmlHelper == null)
          this._htmlHelper = new HtmlHelper<TModel>(this.ViewContext, (IViewDataContainer) this.ViewPage);
        return this._htmlHelper;
      }
    }

    public TModel Model => this.ViewData.Model;

    public ViewDataDictionary<TModel> ViewData
    {
      get
      {
        if (this._viewData == null)
          this._viewData = new ViewDataDictionary<TModel>(this.ViewPage.ViewData);
        return this._viewData;
      }
    }
  }
}
