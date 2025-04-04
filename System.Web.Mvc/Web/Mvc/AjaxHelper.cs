// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.AjaxHelper
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Web.Routing;

#nullable disable
namespace System.Web.Mvc
{
  public class AjaxHelper
  {
    private static string _globalizationScriptPath;
    private DynamicViewDataDictionary _dynamicViewDataDictionary;

    public AjaxHelper(ViewContext viewContext, IViewDataContainer viewDataContainer)
      : this(viewContext, viewDataContainer, RouteTable.Routes)
    {
    }

    public AjaxHelper(
      ViewContext viewContext,
      IViewDataContainer viewDataContainer,
      RouteCollection routeCollection)
    {
      if (viewContext == null)
        throw new ArgumentNullException(nameof (viewContext));
      if (viewDataContainer == null)
        throw new ArgumentNullException(nameof (viewDataContainer));
      if (routeCollection == null)
        throw new ArgumentNullException(nameof (routeCollection));
      this.ViewContext = viewContext;
      this.ViewDataContainer = viewDataContainer;
      this.RouteCollection = routeCollection;
    }

    public static string GlobalizationScriptPath
    {
      get
      {
        if (string.IsNullOrEmpty(AjaxHelper._globalizationScriptPath))
          AjaxHelper._globalizationScriptPath = "~/Scripts/Globalization";
        return AjaxHelper._globalizationScriptPath;
      }
      set => AjaxHelper._globalizationScriptPath = value;
    }

    public RouteCollection RouteCollection { get; private set; }

    public object ViewBag
    {
      get
      {
        if (this._dynamicViewDataDictionary == null)
          this._dynamicViewDataDictionary = new DynamicViewDataDictionary((Func<ViewDataDictionary>) (() => this.ViewData));
        return (object) this._dynamicViewDataDictionary;
      }
    }

    public ViewContext ViewContext { get; private set; }

    public ViewDataDictionary ViewData => this.ViewDataContainer.ViewData;

    public IViewDataContainer ViewDataContainer { get; internal set; }

    public string JavaScriptStringEncode(string message)
    {
      return string.IsNullOrEmpty(message) ? message : HttpUtility.JavaScriptStringEncode(message);
    }
  }
}
