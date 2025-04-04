// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Html.DisplayNameExtensions
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace System.Web.Mvc.Html
{
  public static class DisplayNameExtensions
  {
    public static MvcHtmlString DisplayName(this HtmlHelper html, string expression)
    {
      return html.DisplayNameInternal(expression, (ModelMetadataProvider) null);
    }

    internal static MvcHtmlString DisplayNameInternal(
      this HtmlHelper html,
      string expression,
      ModelMetadataProvider metadataProvider)
    {
      return DisplayNameExtensions.DisplayNameHelper(ModelMetadata.FromStringExpression(expression, html.ViewData, metadataProvider), expression);
    }

    public static MvcHtmlString DisplayNameFor<TModel, TValue>(
      this HtmlHelper<IEnumerable<TModel>> html,
      Expression<Func<TModel, TValue>> expression)
    {
      return html.DisplayNameForInternal<TModel, TValue>(expression, (ModelMetadataProvider) null);
    }

    internal static MvcHtmlString DisplayNameForInternal<TModel, TValue>(
      this HtmlHelper<IEnumerable<TModel>> html,
      Expression<Func<TModel, TValue>> expression,
      ModelMetadataProvider metadataProvider)
    {
      return DisplayNameExtensions.DisplayNameHelper(ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, new ViewDataDictionary<TModel>(), metadataProvider), ExpressionHelper.GetExpressionText((LambdaExpression) expression));
    }

    public static MvcHtmlString DisplayNameFor<TModel, TValue>(
      this HtmlHelper<TModel> html,
      Expression<Func<TModel, TValue>> expression)
    {
      return html.DisplayNameForInternal<TModel, TValue>(expression, (ModelMetadataProvider) null);
    }

    internal static MvcHtmlString DisplayNameForInternal<TModel, TValue>(
      this HtmlHelper<TModel> html,
      Expression<Func<TModel, TValue>> expression,
      ModelMetadataProvider metadataProvider)
    {
      return DisplayNameExtensions.DisplayNameHelper(ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData, metadataProvider), ExpressionHelper.GetExpressionText((LambdaExpression) expression));
    }

    public static MvcHtmlString DisplayNameForModel(this HtmlHelper html)
    {
      return DisplayNameExtensions.DisplayNameHelper(html.ViewData.ModelMetadata, string.Empty);
    }

    internal static MvcHtmlString DisplayNameHelper(ModelMetadata metadata, string htmlFieldName)
    {
      string s = metadata.DisplayName;
      if (s == null)
      {
        string propertyName = metadata.PropertyName;
        if (propertyName == null)
          s = ((IEnumerable<string>) htmlFieldName.Split('.')).Last<string>();
        else
          s = propertyName;
      }
      return new MvcHtmlString(HttpUtility.HtmlEncode(s));
    }
  }
}
