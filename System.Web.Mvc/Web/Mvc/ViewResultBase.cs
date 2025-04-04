// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ViewResultBase
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.IO;

#nullable disable
namespace System.Web.Mvc
{
  public abstract class ViewResultBase : ActionResult
  {
    private DynamicViewDataDictionary _dynamicViewData;
    private TempDataDictionary _tempData;
    private ViewDataDictionary _viewData;
    private ViewEngineCollection _viewEngineCollection;
    private string _viewName;

    public object Model => this.ViewData.Model;

    public TempDataDictionary TempData
    {
      get
      {
        if (this._tempData == null)
          this._tempData = new TempDataDictionary();
        return this._tempData;
      }
      set => this._tempData = value;
    }

    public IView View { get; set; }

    public object ViewBag
    {
      get
      {
        if (this._dynamicViewData == null)
          this._dynamicViewData = new DynamicViewDataDictionary((Func<ViewDataDictionary>) (() => this.ViewData));
        return (object) this._dynamicViewData;
      }
    }

    public ViewDataDictionary ViewData
    {
      get
      {
        if (this._viewData == null)
          this._viewData = new ViewDataDictionary();
        return this._viewData;
      }
      set => this._viewData = value;
    }

    public ViewEngineCollection ViewEngineCollection
    {
      get => this._viewEngineCollection ?? ViewEngines.Engines;
      set => this._viewEngineCollection = value;
    }

    public string ViewName
    {
      get => this._viewName ?? string.Empty;
      set => this._viewName = value;
    }

    public override void ExecuteResult(ControllerContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      if (string.IsNullOrEmpty(this.ViewName))
        this.ViewName = context.RouteData.GetRequiredString("action");
      ViewEngineResult viewEngineResult = (ViewEngineResult) null;
      if (this.View == null)
      {
        viewEngineResult = this.FindView(context);
        this.View = viewEngineResult.View;
      }
      TextWriter output = context.HttpContext.Response.Output;
      this.View.Render(new ViewContext(context, this.View, this.ViewData, this.TempData, output), output);
      viewEngineResult?.ViewEngine.ReleaseView(context, this.View);
    }

    protected abstract ViewEngineResult FindView(ControllerContext context);
  }
}
