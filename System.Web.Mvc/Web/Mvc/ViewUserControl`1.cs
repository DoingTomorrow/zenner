// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ViewUserControl`1
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  public class ViewUserControl<TModel> : ViewUserControl
  {
    private AjaxHelper<TModel> _ajaxHelper;
    private HtmlHelper<TModel> _htmlHelper;
    private ViewDataDictionary<TModel> _viewData;

    public AjaxHelper<TModel> Ajax
    {
      get
      {
        if (this._ajaxHelper == null)
          this._ajaxHelper = new AjaxHelper<TModel>(this.ViewContext, (IViewDataContainer) this);
        return this._ajaxHelper;
      }
    }

    public HtmlHelper<TModel> Html
    {
      get
      {
        if (this._htmlHelper == null)
          this._htmlHelper = new HtmlHelper<TModel>(this.ViewContext, (IViewDataContainer) this);
        return this._htmlHelper;
      }
    }

    public TModel Model => this.ViewData.Model;

    public ViewDataDictionary<TModel> ViewData
    {
      get
      {
        this.EnsureViewData();
        return this._viewData;
      }
      set => this.SetViewData((ViewDataDictionary) value);
    }

    protected override void SetViewData(ViewDataDictionary viewData)
    {
      this._viewData = new ViewDataDictionary<TModel>(viewData);
      base.SetViewData((ViewDataDictionary) this._viewData);
    }
  }
}
