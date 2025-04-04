// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Html.NameExtensions
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Linq.Expressions;

#nullable disable
namespace System.Web.Mvc.Html
{
  public static class NameExtensions
  {
    public static MvcHtmlString Id(this HtmlHelper html, string name)
    {
      return MvcHtmlString.Create(html.AttributeEncode(html.ViewData.TemplateInfo.GetFullHtmlFieldId(name)));
    }

    public static MvcHtmlString IdFor<TModel, TProperty>(
      this HtmlHelper<TModel> html,
      Expression<Func<TModel, TProperty>> expression)
    {
      return html.Id(ExpressionHelper.GetExpressionText((LambdaExpression) expression));
    }

    public static MvcHtmlString IdForModel(this HtmlHelper html) => html.Id(string.Empty);

    public static MvcHtmlString Name(this HtmlHelper html, string name)
    {
      return MvcHtmlString.Create(html.AttributeEncode(html.ViewData.TemplateInfo.GetFullHtmlFieldName(name)));
    }

    public static MvcHtmlString NameFor<TModel, TProperty>(
      this HtmlHelper<TModel> html,
      Expression<Func<TModel, TProperty>> expression)
    {
      return html.Name(ExpressionHelper.GetExpressionText((LambdaExpression) expression));
    }

    public static MvcHtmlString NameForModel(this HtmlHelper html) => html.Name(string.Empty);
  }
}
