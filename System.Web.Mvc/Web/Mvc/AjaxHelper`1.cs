// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.AjaxHelper`1
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Web.Routing;

#nullable disable
namespace System.Web.Mvc
{
  public class AjaxHelper<TModel> : AjaxHelper
  {
    private DynamicViewDataDictionary _dynamicViewDataDictionary;
    private ViewDataDictionary<TModel> _viewData;

    public AjaxHelper(ViewContext viewContext, IViewDataContainer viewDataContainer)
      : this(viewContext, viewDataContainer, RouteTable.Routes)
    {
    }

    public AjaxHelper(
      ViewContext viewContext,
      IViewDataContainer viewDataContainer,
      RouteCollection routeCollection)
      : base(viewContext, viewDataContainer, routeCollection)
    {
      this._viewData = new ViewDataDictionary<TModel>(viewDataContainer.ViewData);
    }

    public new object ViewBag
    {
      get
      {
        if (this._dynamicViewDataDictionary == null)
          this._dynamicViewDataDictionary = new DynamicViewDataDictionary((Func<ViewDataDictionary>) (() => (ViewDataDictionary) this.ViewData));
        return (object) this._dynamicViewDataDictionary;
      }
    }

    public ViewDataDictionary<TModel> ViewData => this._viewData;
  }
}
