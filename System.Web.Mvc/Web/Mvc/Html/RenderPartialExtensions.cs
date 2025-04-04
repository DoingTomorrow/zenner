// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Html.RenderPartialExtensions
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc.Html
{
  public static class RenderPartialExtensions
  {
    public static void RenderPartial(this HtmlHelper htmlHelper, string partialViewName)
    {
      htmlHelper.RenderPartialInternal(partialViewName, htmlHelper.ViewData, (object) null, htmlHelper.ViewContext.Writer, ViewEngines.Engines);
    }

    public static void RenderPartial(
      this HtmlHelper htmlHelper,
      string partialViewName,
      ViewDataDictionary viewData)
    {
      htmlHelper.RenderPartialInternal(partialViewName, viewData, (object) null, htmlHelper.ViewContext.Writer, ViewEngines.Engines);
    }

    public static void RenderPartial(
      this HtmlHelper htmlHelper,
      string partialViewName,
      object model)
    {
      htmlHelper.RenderPartialInternal(partialViewName, htmlHelper.ViewData, model, htmlHelper.ViewContext.Writer, ViewEngines.Engines);
    }

    public static void RenderPartial(
      this HtmlHelper htmlHelper,
      string partialViewName,
      object model,
      ViewDataDictionary viewData)
    {
      htmlHelper.RenderPartialInternal(partialViewName, viewData, model, htmlHelper.ViewContext.Writer, ViewEngines.Engines);
    }
  }
}
