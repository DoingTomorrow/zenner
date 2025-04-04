// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ViewUserControl
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Web.Mvc.Properties;
using System.Web.UI;

#nullable disable
namespace System.Web.Mvc
{
  [FileLevelControlBuilder(typeof (ViewUserControlControlBuilder))]
  public class ViewUserControl : UserControl, IViewDataContainer
  {
    private AjaxHelper<object> _ajaxHelper;
    private DynamicViewDataDictionary _dynamicViewData;
    private HtmlHelper<object> _htmlHelper;
    private ViewContext _viewContext;
    private ViewDataDictionary _viewData;
    private string _viewDataKey;

    public AjaxHelper<object> Ajax
    {
      get
      {
        if (this._ajaxHelper == null)
          this._ajaxHelper = new AjaxHelper<object>(this.ViewContext, (IViewDataContainer) this);
        return this._ajaxHelper;
      }
    }

    public HtmlHelper<object> Html
    {
      get
      {
        if (this._htmlHelper == null)
          this._htmlHelper = new HtmlHelper<object>(this.ViewContext, (IViewDataContainer) this);
        return this._htmlHelper;
      }
    }

    public object Model => this.ViewData.Model;

    public TempDataDictionary TempData => this.ViewPage.TempData;

    public UrlHelper Url => this.ViewPage.Url;

    public object ViewBag
    {
      get
      {
        if (this._dynamicViewData == null)
          this._dynamicViewData = new DynamicViewDataDictionary((Func<ViewDataDictionary>) (() => this.ViewData));
        return (object) this._dynamicViewData;
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ViewContext ViewContext
    {
      get => this._viewContext ?? this.ViewPage.ViewContext;
      set => this._viewContext = value;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public ViewDataDictionary ViewData
    {
      get
      {
        this.EnsureViewData();
        return this._viewData;
      }
      set => this.SetViewData(value);
    }

    [DefaultValue("")]
    public string ViewDataKey
    {
      get => this._viewDataKey ?? string.Empty;
      set => this._viewDataKey = value;
    }

    internal ViewPage ViewPage
    {
      get
      {
        return this.Page is ViewPage page ? page : throw new InvalidOperationException(MvcResources.ViewUserControl_RequiresViewPage);
      }
    }

    public HtmlTextWriter Writer => this.ViewPage.Writer;

    protected virtual void SetViewData(ViewDataDictionary viewData) => this._viewData = viewData;

    protected void EnsureViewData()
    {
      if (this._viewData != null)
        return;
      ViewDataDictionary viewDataDictionary1 = (ViewUserControl.GetViewDataContainer((Control) this) ?? throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ViewUserControl_RequiresViewDataProvider, new object[1]
      {
        (object) this.AppRelativeVirtualPath
      }))).ViewData;
      if (!string.IsNullOrEmpty(this.ViewDataKey))
      {
        object obj = viewDataDictionary1.Eval(this.ViewDataKey);
        if (!(obj is ViewDataDictionary viewDataDictionary2))
          viewDataDictionary2 = new ViewDataDictionary(viewDataDictionary1)
          {
            Model = obj
          };
        viewDataDictionary1 = viewDataDictionary2;
      }
      this.SetViewData(viewDataDictionary1);
    }

    private static IViewDataContainer GetViewDataContainer(Control control)
    {
      while (control != null)
      {
        control = control.Parent;
        if (control is IViewDataContainer viewDataContainer)
          return viewDataContainer;
      }
      return (IViewDataContainer) null;
    }

    public virtual void RenderView(ViewContext viewContext)
    {
      using (ViewUserControl.ViewUserControlContainerPage containerPage = new ViewUserControl.ViewUserControlContainerPage(this))
        ViewUserControl.RenderViewAndRestoreContentType((ViewPage) containerPage, viewContext);
    }

    internal static void RenderViewAndRestoreContentType(
      ViewPage containerPage,
      ViewContext viewContext)
    {
      string contentType = viewContext.HttpContext.Response.ContentType;
      containerPage.RenderView(viewContext);
      viewContext.HttpContext.Response.ContentType = contentType;
    }

    [Obsolete("The TextWriter is now provided by the ViewContext object passed to the RenderView method.", true)]
    public void SetTextWriter(TextWriter textWriter)
    {
    }

    private sealed class ViewUserControlContainerPage : ViewPage
    {
      private readonly ViewUserControl _userControl;

      public ViewUserControlContainerPage(ViewUserControl userControl)
      {
        this._userControl = userControl;
      }

      public override void ProcessRequest(HttpContext context)
      {
        this._userControl.ID = ViewPage.NextId();
        this.Controls.Add((Control) this._userControl);
        base.ProcessRequest(context);
      }
    }
  }
}
