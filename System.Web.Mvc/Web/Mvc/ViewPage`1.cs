// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ViewPage`1
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  public class ViewPage<TModel> : ViewPage
  {
    private ViewDataDictionary<TModel> _viewData;

    public AjaxHelper<TModel> Ajax { get; set; }

    public HtmlHelper<TModel> Html { get; set; }

    public TModel Model => this.ViewData.Model;

    public ViewDataDictionary<TModel> ViewData
    {
      get
      {
        if (this._viewData == null)
          this.SetViewData((ViewDataDictionary) new ViewDataDictionary<TModel>());
        return this._viewData;
      }
      set => this.SetViewData((ViewDataDictionary) value);
    }

    public override void InitHelpers()
    {
      base.InitHelpers();
      this.Ajax = new AjaxHelper<TModel>(this.ViewContext, (IViewDataContainer) this);
      this.Html = new HtmlHelper<TModel>(this.ViewContext, (IViewDataContainer) this);
    }

    protected override void SetViewData(ViewDataDictionary viewData)
    {
      this._viewData = new ViewDataDictionary<TModel>(viewData);
      base.SetViewData((ViewDataDictionary) this._viewData);
    }
  }
}
