// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Html.PartialExtensions
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Globalization;
using System.IO;

#nullable disable
namespace System.Web.Mvc.Html
{
  public static class PartialExtensions
  {
    public static MvcHtmlString Partial(this HtmlHelper htmlHelper, string partialViewName)
    {
      return htmlHelper.Partial(partialViewName, (object) null, htmlHelper.ViewData);
    }

    public static MvcHtmlString Partial(
      this HtmlHelper htmlHelper,
      string partialViewName,
      ViewDataDictionary viewData)
    {
      return htmlHelper.Partial(partialViewName, (object) null, viewData);
    }

    public static MvcHtmlString Partial(
      this HtmlHelper htmlHelper,
      string partialViewName,
      object model)
    {
      return htmlHelper.Partial(partialViewName, model, htmlHelper.ViewData);
    }

    public static MvcHtmlString Partial(
      this HtmlHelper htmlHelper,
      string partialViewName,
      object model,
      ViewDataDictionary viewData)
    {
      using (StringWriter writer = new StringWriter((IFormatProvider) CultureInfo.CurrentCulture))
      {
        htmlHelper.RenderPartialInternal(partialViewName, viewData, model, (TextWriter) writer, ViewEngines.Engines);
        return MvcHtmlString.Create(writer.ToString());
      }
    }
  }
}
