// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Html.TextAreaExtensions
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc.Html
{
  public static class TextAreaExtensions
  {
    private const int TextAreaRows = 2;
    private const int TextAreaColumns = 20;
    private static Dictionary<string, object> implicitRowsAndColumns = new Dictionary<string, object>()
    {
      {
        "rows",
        (object) 2.ToString((IFormatProvider) CultureInfo.InvariantCulture)
      },
      {
        "cols",
        (object) 20.ToString((IFormatProvider) CultureInfo.InvariantCulture)
      }
    };

    private static Dictionary<string, object> GetRowsAndColumnsDictionary(int rows, int columns)
    {
      if (rows < 0)
        throw new ArgumentOutOfRangeException(nameof (rows), MvcResources.HtmlHelper_TextAreaParameterOutOfRange);
      if (columns < 0)
        throw new ArgumentOutOfRangeException(nameof (columns), MvcResources.HtmlHelper_TextAreaParameterOutOfRange);
      Dictionary<string, object> columnsDictionary = new Dictionary<string, object>();
      if (rows > 0)
        columnsDictionary.Add(nameof (rows), (object) rows.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      if (columns > 0)
        columnsDictionary.Add("cols", (object) columns.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      return columnsDictionary;
    }

    public static MvcHtmlString TextArea(this HtmlHelper htmlHelper, string name)
    {
      return TextAreaExtensions.TextArea(htmlHelper, name, (string) null, (IDictionary<string, object>) null);
    }

    public static MvcHtmlString TextArea(
      this HtmlHelper htmlHelper,
      string name,
      object htmlAttributes)
    {
      return TextAreaExtensions.TextArea(htmlHelper, name, (string) null, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString TextArea(
      this HtmlHelper htmlHelper,
      string name,
      IDictionary<string, object> htmlAttributes)
    {
      return TextAreaExtensions.TextArea(htmlHelper, name, (string) null, htmlAttributes);
    }

    public static MvcHtmlString TextArea(this HtmlHelper htmlHelper, string name, string value)
    {
      return TextAreaExtensions.TextArea(htmlHelper, name, value, (IDictionary<string, object>) null);
    }

    public static MvcHtmlString TextArea(
      this HtmlHelper htmlHelper,
      string name,
      string value,
      object htmlAttributes)
    {
      return TextAreaExtensions.TextArea(htmlHelper, name, value, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString TextArea(
      this HtmlHelper htmlHelper,
      string name,
      string value,
      IDictionary<string, object> htmlAttributes)
    {
      ModelMetadata modelMetadata = ModelMetadata.FromStringExpression(name, htmlHelper.ViewContext.ViewData);
      if (value != null)
        modelMetadata.Model = (object) value;
      return TextAreaExtensions.TextAreaHelper(htmlHelper, modelMetadata, name, (IDictionary<string, object>) TextAreaExtensions.implicitRowsAndColumns, htmlAttributes);
    }

    public static MvcHtmlString TextArea(
      this HtmlHelper htmlHelper,
      string name,
      string value,
      int rows,
      int columns,
      object htmlAttributes)
    {
      return TextAreaExtensions.TextArea(htmlHelper, name, value, rows, columns, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString TextArea(
      this HtmlHelper htmlHelper,
      string name,
      string value,
      int rows,
      int columns,
      IDictionary<string, object> htmlAttributes)
    {
      ModelMetadata modelMetadata = ModelMetadata.FromStringExpression(name, htmlHelper.ViewContext.ViewData);
      if (value != null)
        modelMetadata.Model = (object) value;
      return TextAreaExtensions.TextAreaHelper(htmlHelper, modelMetadata, name, (IDictionary<string, object>) TextAreaExtensions.GetRowsAndColumnsDictionary(rows, columns), htmlAttributes);
    }

    public static MvcHtmlString TextAreaFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression)
    {
      return TextAreaExtensions.TextAreaFor<TModel, TProperty>(htmlHelper, expression, (IDictionary<string, object>) null);
    }

    public static MvcHtmlString TextAreaFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      object htmlAttributes)
    {
      return TextAreaExtensions.TextAreaFor<TModel, TProperty>(htmlHelper, expression, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString TextAreaFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      IDictionary<string, object> htmlAttributes)
    {
      if (expression == null)
        throw new ArgumentNullException(nameof (expression));
      return TextAreaExtensions.TextAreaHelper((HtmlHelper) htmlHelper, ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData), ExpressionHelper.GetExpressionText((LambdaExpression) expression), (IDictionary<string, object>) TextAreaExtensions.implicitRowsAndColumns, htmlAttributes);
    }

    public static MvcHtmlString TextAreaFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      int rows,
      int columns,
      object htmlAttributes)
    {
      return TextAreaExtensions.TextAreaFor<TModel, TProperty>(htmlHelper, expression, rows, columns, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString TextAreaFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      int rows,
      int columns,
      IDictionary<string, object> htmlAttributes)
    {
      if (expression == null)
        throw new ArgumentNullException(nameof (expression));
      return TextAreaExtensions.TextAreaHelper((HtmlHelper) htmlHelper, ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData), ExpressionHelper.GetExpressionText((LambdaExpression) expression), (IDictionary<string, object>) TextAreaExtensions.GetRowsAndColumnsDictionary(rows, columns), htmlAttributes);
    }

    internal static MvcHtmlString TextAreaHelper(
      HtmlHelper htmlHelper,
      ModelMetadata modelMetadata,
      string name,
      IDictionary<string, object> rowsAndColumns,
      IDictionary<string, object> htmlAttributes,
      string innerHtmlPrefix = null)
    {
      string fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
      if (string.IsNullOrEmpty(fullHtmlFieldName))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (name));
      TagBuilder tagBuilder = new TagBuilder("textarea");
      tagBuilder.GenerateId(fullHtmlFieldName);
      tagBuilder.MergeAttributes<string, object>(htmlAttributes, true);
      tagBuilder.MergeAttributes<string, object>(rowsAndColumns, rowsAndColumns != TextAreaExtensions.implicitRowsAndColumns);
      tagBuilder.MergeAttribute(nameof (name), fullHtmlFieldName, true);
      ModelState modelState;
      if (htmlHelper.ViewData.ModelState.TryGetValue(fullHtmlFieldName, out modelState) && modelState.Errors.Count > 0)
        tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
      tagBuilder.MergeAttributes<string, object>(htmlHelper.GetUnobtrusiveValidationAttributes(name, modelMetadata));
      string s = modelState == null || modelState.Value == null ? (modelMetadata.Model == null ? string.Empty : modelMetadata.Model.ToString()) : modelState.Value.AttemptedValue;
      tagBuilder.InnerHtml = (innerHtmlPrefix ?? Environment.NewLine) + HttpUtility.HtmlEncode(s);
      return tagBuilder.ToMvcHtmlString(TagRenderMode.Normal);
    }
  }
}
