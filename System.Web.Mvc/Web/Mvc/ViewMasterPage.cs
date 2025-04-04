// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ViewMasterPage
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Globalization;
using System.Web.Mvc.Properties;
using System.Web.UI;

#nullable disable
namespace System.Web.Mvc
{
  [FileLevelControlBuilder(typeof (ViewMasterPageControlBuilder))]
  public class ViewMasterPage : MasterPage
  {
    public AjaxHelper<object> Ajax => this.ViewPage.Ajax;

    public HtmlHelper<object> Html => this.ViewPage.Html;

    public object Model => this.ViewData.Model;

    public TempDataDictionary TempData => this.ViewPage.TempData;

    public UrlHelper Url => this.ViewPage.Url;

    public object ViewBag => this.ViewPage.ViewBag;

    public ViewContext ViewContext => this.ViewPage.ViewContext;

    public ViewDataDictionary ViewData => this.ViewPage.ViewData;

    internal ViewPage ViewPage
    {
      get
      {
        return this.Page is ViewPage page ? page : throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ViewMasterPage_RequiresViewPage));
      }
    }

    public HtmlTextWriter Writer => this.ViewPage.Writer;
  }
}
