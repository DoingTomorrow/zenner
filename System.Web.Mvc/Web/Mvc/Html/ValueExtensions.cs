// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Html.ValueExtensions
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Linq.Expressions;

#nullable disable
namespace System.Web.Mvc.Html
{
  public static class ValueExtensions
  {
    public static MvcHtmlString Value(this HtmlHelper html, string name)
    {
      return html.Value(name, (string) null);
    }

    public static MvcHtmlString Value(this HtmlHelper html, string name, string format)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      return ValueExtensions.ValueForHelper(html, name, (object) null, format, true);
    }

    public static MvcHtmlString ValueFor<TModel, TProperty>(
      this HtmlHelper<TModel> html,
      Expression<Func<TModel, TProperty>> expression)
    {
      return html.ValueFor<TModel, TProperty>(expression, (string) null);
    }

    public static MvcHtmlString ValueFor<TModel, TProperty>(
      this HtmlHelper<TModel> html,
      Expression<Func<TModel, TProperty>> expression,
      string format)
    {
      ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, html.ViewData);
      return ValueExtensions.ValueForHelper((HtmlHelper) html, ExpressionHelper.GetExpressionText((LambdaExpression) expression), modelMetadata.Model, format, false);
    }

    public static MvcHtmlString ValueForModel(this HtmlHelper html)
    {
      return html.ValueForModel((string) null);
    }

    public static MvcHtmlString ValueForModel(this HtmlHelper html, string format)
    {
      return html.Value(string.Empty, format);
    }

    internal static MvcHtmlString ValueForHelper(
      HtmlHelper html,
      string name,
      object value,
      string format,
      bool useViewData)
    {
      string fullHtmlFieldName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
      string modelStateValue = (string) html.GetModelStateValue(fullHtmlFieldName, typeof (string));
      string str;
      if (modelStateValue != null)
        str = modelStateValue;
      else if (useViewData)
      {
        if (name.Length == 0)
        {
          ModelMetadata modelMetadata = ModelMetadata.FromStringExpression(string.Empty, html.ViewContext.ViewData);
          str = html.FormatValue(modelMetadata.Model, format);
        }
        else
          str = html.EvalString(name, format);
      }
      else
        str = html.FormatValue(value, format);
      return MvcHtmlString.Create(html.AttributeEncode(str));
    }
  }
}
