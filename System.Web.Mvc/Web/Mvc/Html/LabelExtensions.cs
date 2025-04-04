// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Html.LabelExtensions
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace System.Web.Mvc.Html
{
  public static class LabelExtensions
  {
    public static MvcHtmlString Label(this HtmlHelper html, string expression)
    {
      return LabelExtensions.Label(html, expression, (string) null);
    }

    public static MvcHtmlString Label(this HtmlHelper html, string expression, string labelText)
    {
      return LabelExtensions.Label(html, expression, labelText, (IDictionary<string, object>) null, (ModelMetadataProvider) null);
    }

    public static MvcHtmlString Label(
      this HtmlHelper html,
      string expression,
      object htmlAttributes)
    {
      return html.Label(expression, (string) null, htmlAttributes, (ModelMetadataProvider) null);
    }

    public static MvcHtmlString Label(
      this HtmlHelper html,
      string expression,
      IDictionary<string, object> htmlAttributes)
    {
      return LabelExtensions.Label(html, expression, (string) null, htmlAttributes, (ModelMetadataProvider) null);
    }

    public static MvcHtmlString Label(
      this HtmlHelper html,
      string expression,
      string labelText,
      object htmlAttributes)
    {
      return html.Label(expression, labelText, htmlAttributes, (ModelMetadataProvider) null);
    }

    public static MvcHtmlString Label(
      this HtmlHelper html,
      string expression,
      string labelText,
      IDictionary<string, object> htmlAttributes)
    {
      return LabelExtensions.Label(html, expression, labelText, htmlAttributes, (ModelMetadataProvider) null);
    }

    internal static MvcHtmlString Label(
      this HtmlHelper html,
      string expression,
      string labelText,
      object htmlAttributes,
      ModelMetadataProvider metadataProvider)
    {
      return LabelExtensions.Label(html, expression, labelText, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), metadataProvider);
    }

    internal static MvcHtmlString Label(
      this HtmlHelper html,
      string expression,
      string labelText,
      IDictionary<string, object> htmlAttributes,
      ModelMetadataProvider metadataProvider)
    {
      return LabelExtensions.LabelHelper(html, ModelMetadata.FromStringExpression(expression, html.ViewData, metadataProvider), expression, labelText, htmlAttributes);
    }

    public static MvcHtmlString LabelFor<TModel, TValue>(
      this HtmlHelper<TModel> html,
      Expression<Func<TModel, TValue>> expression)
    {
      return LabelExtensions.LabelFor<TModel, TValue>(html, expression, (string) null);
    }

    public static MvcHtmlString LabelFor<TModel, TValue>(
      this HtmlHelper<TModel> html,
      Expression<Func<TModel, TValue>> expression,
      string labelText)
    {
      return LabelExtensions.LabelFor<TModel, TValue>(html, expression, labelText, (IDictionary<string, object>) null, (ModelMetadataProvider) null);
    }

    public static MvcHtmlString LabelFor<TModel, TValue>(
      this HtmlHelper<TModel> html,
      Expression<Func<TModel, TValue>> expression,
      object htmlAttributes)
    {
      return html.LabelFor<TModel, TValue>(expression, (string) null, htmlAttributes, (ModelMetadataProvider) null);
    }

    public static MvcHtmlString LabelFor<TModel, TValue>(
      this HtmlHelper<TModel> html,
      Expression<Func<TModel, TValue>> expression,
      IDictionary<string, object> htmlAttributes)
    {
      return LabelExtensions.LabelFor<TModel, TValue>(html, expression, (string) null, htmlAttributes, (ModelMetadataProvider) null);
    }

    public static MvcHtmlString LabelFor<TModel, TValue>(
      this HtmlHelper<TModel> html,
      Expression<Func<TModel, TValue>> expression,
      string labelText,
      object htmlAttributes)
    {
      return html.LabelFor<TModel, TValue>(expression, labelText, htmlAttributes, (ModelMetadataProvider) null);
    }

    public static MvcHtmlString LabelFor<TModel, TValue>(
      this HtmlHelper<TModel> html,
      Expression<Func<TModel, TValue>> expression,
      string labelText,
      IDictionary<string, object> htmlAttributes)
    {
      return LabelExtensions.LabelFor<TModel, TValue>(html, expression, labelText, htmlAttributes, (ModelMetadataProvider) null);
    }

    internal static MvcHtmlString LabelFor<TModel, TValue>(
      this HtmlHelper<TModel> html,
      Expression<Func<TModel, TValue>> expression,
      string labelText,
      object htmlAttributes,
      ModelMetadataProvider metadataProvider)
    {
      return LabelExtensions.LabelFor<TModel, TValue>(html, expression, labelText, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), metadataProvider);
    }

    internal static MvcHtmlString LabelFor<TModel, TValue>(
      this HtmlHelper<TModel> html,
      Expression<Func<TModel, TValue>> expression,
      string labelText,
      IDictionary<string, object> htmlAttributes,
      ModelMetadataProvider metadataProvider)
    {
      return LabelExtensions.LabelHelper((HtmlHelper) html, ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData, metadataProvider), ExpressionHelper.GetExpressionText((LambdaExpression) expression), labelText, htmlAttributes);
    }

    public static MvcHtmlString LabelForModel(this HtmlHelper html)
    {
      return LabelExtensions.LabelForModel(html, (string) null);
    }

    public static MvcHtmlString LabelForModel(this HtmlHelper html, string labelText)
    {
      return LabelExtensions.LabelHelper(html, html.ViewData.ModelMetadata, string.Empty, labelText);
    }

    public static MvcHtmlString LabelForModel(this HtmlHelper html, object htmlAttributes)
    {
      return LabelExtensions.LabelHelper(html, html.ViewData.ModelMetadata, string.Empty, htmlAttributes: (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString LabelForModel(
      this HtmlHelper html,
      IDictionary<string, object> htmlAttributes)
    {
      return LabelExtensions.LabelHelper(html, html.ViewData.ModelMetadata, string.Empty, htmlAttributes: htmlAttributes);
    }

    public static MvcHtmlString LabelForModel(
      this HtmlHelper html,
      string labelText,
      object htmlAttributes)
    {
      return LabelExtensions.LabelHelper(html, html.ViewData.ModelMetadata, string.Empty, labelText, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString LabelForModel(
      this HtmlHelper html,
      string labelText,
      IDictionary<string, object> htmlAttributes)
    {
      return LabelExtensions.LabelHelper(html, html.ViewData.ModelMetadata, string.Empty, labelText, htmlAttributes);
    }

    internal static MvcHtmlString LabelHelper(
      HtmlHelper html,
      ModelMetadata metadata,
      string htmlFieldName,
      string labelText = null,
      IDictionary<string, object> htmlAttributes = null)
    {
      string str = labelText;
      if (str == null)
      {
        string displayName = metadata.DisplayName;
        if (displayName == null)
        {
          string propertyName = metadata.PropertyName;
          if (propertyName == null)
            str = ((IEnumerable<string>) htmlFieldName.Split('.')).Last<string>();
          else
            str = propertyName;
        }
        else
          str = displayName;
      }
      string innerText = str;
      if (string.IsNullOrEmpty(innerText))
        return MvcHtmlString.Empty;
      TagBuilder tagBuilder = new TagBuilder("label");
      tagBuilder.Attributes.Add("for", TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));
      tagBuilder.SetInnerText(innerText);
      tagBuilder.MergeAttributes<string, object>(htmlAttributes, true);
      return tagBuilder.ToMvcHtmlString(TagRenderMode.Normal);
    }
  }
}
