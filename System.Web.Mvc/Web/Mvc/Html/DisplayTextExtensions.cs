// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Html.DisplayTextExtensions
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Linq.Expressions;

#nullable disable
namespace System.Web.Mvc.Html
{
  public static class DisplayTextExtensions
  {
    public static MvcHtmlString DisplayText(this HtmlHelper html, string name)
    {
      return DisplayTextExtensions.DisplayTextHelper(html, ModelMetadata.FromStringExpression(name, html.ViewContext.ViewData));
    }

    public static MvcHtmlString DisplayTextFor<TModel, TResult>(
      this HtmlHelper<TModel> html,
      Expression<Func<TModel, TResult>> expression)
    {
      return DisplayTextExtensions.DisplayTextHelper((HtmlHelper) html, ModelMetadata.FromLambdaExpression<TModel, TResult>(expression, html.ViewData));
    }

    private static MvcHtmlString DisplayTextHelper(HtmlHelper html, ModelMetadata metadata)
    {
      string str = metadata.SimpleDisplayText;
      if (metadata.HtmlEncode)
        str = html.Encode(str);
      return MvcHtmlString.Create(str);
    }
  }
}
