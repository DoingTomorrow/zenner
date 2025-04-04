// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Html.InputExtensions
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Data.Linq;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc.Properties;
using System.Web.Routing;

#nullable disable
namespace System.Web.Mvc.Html
{
  public static class InputExtensions
  {
    public static MvcHtmlString CheckBox(this HtmlHelper htmlHelper, string name)
    {
      return htmlHelper.CheckBox(name, (object) null);
    }

    public static MvcHtmlString CheckBox(this HtmlHelper htmlHelper, string name, bool isChecked)
    {
      return htmlHelper.CheckBox(name, isChecked, (object) null);
    }

    public static MvcHtmlString CheckBox(
      this HtmlHelper htmlHelper,
      string name,
      bool isChecked,
      object htmlAttributes)
    {
      return InputExtensions.CheckBox(htmlHelper, name, isChecked, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString CheckBox(
      this HtmlHelper htmlHelper,
      string name,
      object htmlAttributes)
    {
      return InputExtensions.CheckBox(htmlHelper, name, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString CheckBox(
      this HtmlHelper htmlHelper,
      string name,
      IDictionary<string, object> htmlAttributes)
    {
      return InputExtensions.CheckBoxHelper(htmlHelper, (ModelMetadata) null, name, new bool?(), htmlAttributes);
    }

    public static MvcHtmlString CheckBox(
      this HtmlHelper htmlHelper,
      string name,
      bool isChecked,
      IDictionary<string, object> htmlAttributes)
    {
      return InputExtensions.CheckBoxHelper(htmlHelper, (ModelMetadata) null, name, new bool?(isChecked), htmlAttributes);
    }

    public static MvcHtmlString CheckBoxFor<TModel>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, bool>> expression)
    {
      return InputExtensions.CheckBoxFor<TModel>(htmlHelper, expression, (IDictionary<string, object>) null);
    }

    public static MvcHtmlString CheckBoxFor<TModel>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, bool>> expression,
      object htmlAttributes)
    {
      return InputExtensions.CheckBoxFor<TModel>(htmlHelper, expression, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString CheckBoxFor<TModel>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, bool>> expression,
      IDictionary<string, object> htmlAttributes)
    {
      if (expression == null)
        throw new ArgumentNullException(nameof (expression));
      ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, bool>(expression, htmlHelper.ViewData);
      bool? isChecked = new bool?();
      bool result;
      if (metadata.Model != null && bool.TryParse(metadata.Model.ToString(), out result))
        isChecked = new bool?(result);
      return InputExtensions.CheckBoxHelper((HtmlHelper) htmlHelper, metadata, ExpressionHelper.GetExpressionText((LambdaExpression) expression), isChecked, htmlAttributes);
    }

    private static MvcHtmlString CheckBoxHelper(
      HtmlHelper htmlHelper,
      ModelMetadata metadata,
      string name,
      bool? isChecked,
      IDictionary<string, object> htmlAttributes)
    {
      RouteValueDictionary routeValueDictionary = InputExtensions.ToRouteValueDictionary(htmlAttributes);
      bool hasValue = isChecked.HasValue;
      if (hasValue)
        routeValueDictionary.Remove("checked");
      return InputExtensions.InputHelper(htmlHelper, InputType.CheckBox, metadata, name, (object) "true", !hasValue, ((int) isChecked ?? 0) != 0, true, false, (string) null, (IDictionary<string, object>) routeValueDictionary);
    }

    public static MvcHtmlString Hidden(this HtmlHelper htmlHelper, string name)
    {
      return InputExtensions.Hidden(htmlHelper, name, (object) null, (IDictionary<string, object>) null);
    }

    public static MvcHtmlString Hidden(this HtmlHelper htmlHelper, string name, object value)
    {
      return InputExtensions.Hidden(htmlHelper, name, value, (IDictionary<string, object>) null);
    }

    public static MvcHtmlString Hidden(
      this HtmlHelper htmlHelper,
      string name,
      object value,
      object htmlAttributes)
    {
      return InputExtensions.Hidden(htmlHelper, name, value, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString Hidden(
      this HtmlHelper htmlHelper,
      string name,
      object value,
      IDictionary<string, object> htmlAttributes)
    {
      return InputExtensions.HiddenHelper(htmlHelper, (ModelMetadata) null, value, value == null, name, htmlAttributes);
    }

    public static MvcHtmlString HiddenFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression)
    {
      return InputExtensions.HiddenFor<TModel, TProperty>(htmlHelper, expression, (IDictionary<string, object>) null);
    }

    public static MvcHtmlString HiddenFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      object htmlAttributes)
    {
      return InputExtensions.HiddenFor<TModel, TProperty>(htmlHelper, expression, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString HiddenFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      IDictionary<string, object> htmlAttributes)
    {
      ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
      return InputExtensions.HiddenHelper((HtmlHelper) htmlHelper, metadata, metadata.Model, false, ExpressionHelper.GetExpressionText((LambdaExpression) expression), htmlAttributes);
    }

    private static MvcHtmlString HiddenHelper(
      HtmlHelper htmlHelper,
      ModelMetadata metadata,
      object value,
      bool useViewData,
      string expression,
      IDictionary<string, object> htmlAttributes)
    {
      Binary binary = value as Binary;
      if (binary != (Binary) null)
        value = (object) binary.ToArray();
      if (value is byte[] inArray)
        value = (object) Convert.ToBase64String(inArray);
      return InputExtensions.InputHelper(htmlHelper, InputType.Hidden, metadata, expression, value, useViewData, false, true, true, (string) null, htmlAttributes);
    }

    public static MvcHtmlString Password(this HtmlHelper htmlHelper, string name)
    {
      return htmlHelper.Password(name, (object) null);
    }

    public static MvcHtmlString Password(this HtmlHelper htmlHelper, string name, object value)
    {
      return InputExtensions.Password(htmlHelper, name, value, (IDictionary<string, object>) null);
    }

    public static MvcHtmlString Password(
      this HtmlHelper htmlHelper,
      string name,
      object value,
      object htmlAttributes)
    {
      return InputExtensions.Password(htmlHelper, name, value, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString Password(
      this HtmlHelper htmlHelper,
      string name,
      object value,
      IDictionary<string, object> htmlAttributes)
    {
      return InputExtensions.PasswordHelper(htmlHelper, (ModelMetadata) null, name, value, htmlAttributes);
    }

    public static MvcHtmlString PasswordFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression)
    {
      return InputExtensions.PasswordFor<TModel, TProperty>(htmlHelper, expression, (IDictionary<string, object>) null);
    }

    public static MvcHtmlString PasswordFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      object htmlAttributes)
    {
      return InputExtensions.PasswordFor<TModel, TProperty>(htmlHelper, expression, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString PasswordFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      IDictionary<string, object> htmlAttributes)
    {
      if (expression == null)
        throw new ArgumentNullException(nameof (expression));
      return InputExtensions.PasswordHelper((HtmlHelper) htmlHelper, ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData), ExpressionHelper.GetExpressionText((LambdaExpression) expression), (object) null, htmlAttributes);
    }

    private static MvcHtmlString PasswordHelper(
      HtmlHelper htmlHelper,
      ModelMetadata metadata,
      string name,
      object value,
      IDictionary<string, object> htmlAttributes)
    {
      return InputExtensions.InputHelper(htmlHelper, InputType.Password, metadata, name, value, false, false, true, true, (string) null, htmlAttributes);
    }

    public static MvcHtmlString RadioButton(this HtmlHelper htmlHelper, string name, object value)
    {
      return htmlHelper.RadioButton(name, value, (object) null);
    }

    public static MvcHtmlString RadioButton(
      this HtmlHelper htmlHelper,
      string name,
      object value,
      object htmlAttributes)
    {
      return InputExtensions.RadioButton(htmlHelper, name, value, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString RadioButton(
      this HtmlHelper htmlHelper,
      string name,
      object value,
      IDictionary<string, object> htmlAttributes)
    {
      string b = Convert.ToString(value, (IFormatProvider) CultureInfo.CurrentCulture);
      bool isChecked = !string.IsNullOrEmpty(name) && string.Equals(htmlHelper.EvalString(name), b, StringComparison.OrdinalIgnoreCase);
      RouteValueDictionary routeValueDictionary = InputExtensions.ToRouteValueDictionary(htmlAttributes);
      return routeValueDictionary.ContainsKey("checked") ? InputExtensions.InputHelper(htmlHelper, InputType.Radio, (ModelMetadata) null, name, value, false, false, true, true, (string) null, (IDictionary<string, object>) routeValueDictionary) : InputExtensions.RadioButton(htmlHelper, name, value, isChecked, htmlAttributes);
    }

    public static MvcHtmlString RadioButton(
      this HtmlHelper htmlHelper,
      string name,
      object value,
      bool isChecked)
    {
      return htmlHelper.RadioButton(name, value, isChecked, (object) null);
    }

    public static MvcHtmlString RadioButton(
      this HtmlHelper htmlHelper,
      string name,
      object value,
      bool isChecked,
      object htmlAttributes)
    {
      return InputExtensions.RadioButton(htmlHelper, name, value, isChecked, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString RadioButton(
      this HtmlHelper htmlHelper,
      string name,
      object value,
      bool isChecked,
      IDictionary<string, object> htmlAttributes)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      RouteValueDictionary routeValueDictionary = InputExtensions.ToRouteValueDictionary(htmlAttributes);
      routeValueDictionary.Remove("checked");
      return InputExtensions.InputHelper(htmlHelper, InputType.Radio, (ModelMetadata) null, name, value, false, isChecked, true, true, (string) null, (IDictionary<string, object>) routeValueDictionary);
    }

    public static MvcHtmlString RadioButtonFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      object value)
    {
      return InputExtensions.RadioButtonFor<TModel, TProperty>(htmlHelper, expression, value, (IDictionary<string, object>) null);
    }

    public static MvcHtmlString RadioButtonFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      object value,
      object htmlAttributes)
    {
      return InputExtensions.RadioButtonFor<TModel, TProperty>(htmlHelper, expression, value, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString RadioButtonFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      object value,
      IDictionary<string, object> htmlAttributes)
    {
      ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
      return InputExtensions.RadioButtonHelper((HtmlHelper) htmlHelper, metadata, metadata.Model, ExpressionHelper.GetExpressionText((LambdaExpression) expression), value, new bool?(), htmlAttributes);
    }

    private static MvcHtmlString RadioButtonHelper(
      HtmlHelper htmlHelper,
      ModelMetadata metadata,
      object model,
      string name,
      object value,
      bool? isChecked,
      IDictionary<string, object> htmlAttributes)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      RouteValueDictionary routeValueDictionary = InputExtensions.ToRouteValueDictionary(htmlAttributes);
      if (isChecked.HasValue)
      {
        routeValueDictionary.Remove("checked");
      }
      else
      {
        string b = Convert.ToString(value, (IFormatProvider) CultureInfo.CurrentCulture);
        isChecked = new bool?(model != null && !string.IsNullOrEmpty(name) && string.Equals(model.ToString(), b, StringComparison.OrdinalIgnoreCase));
      }
      return InputExtensions.InputHelper(htmlHelper, InputType.Radio, metadata, name, value, false, ((int) isChecked ?? 0) != 0, true, true, (string) null, (IDictionary<string, object>) routeValueDictionary);
    }

    public static MvcHtmlString TextBox(this HtmlHelper htmlHelper, string name)
    {
      return htmlHelper.TextBox(name, (object) null);
    }

    public static MvcHtmlString TextBox(this HtmlHelper htmlHelper, string name, object value)
    {
      return InputExtensions.TextBox(htmlHelper, name, value, (string) null);
    }

    public static MvcHtmlString TextBox(
      this HtmlHelper htmlHelper,
      string name,
      object value,
      string format)
    {
      return htmlHelper.TextBox(name, value, format, (object) null);
    }

    public static MvcHtmlString TextBox(
      this HtmlHelper htmlHelper,
      string name,
      object value,
      object htmlAttributes)
    {
      return htmlHelper.TextBox(name, value, (string) null, htmlAttributes);
    }

    public static MvcHtmlString TextBox(
      this HtmlHelper htmlHelper,
      string name,
      object value,
      string format,
      object htmlAttributes)
    {
      return InputExtensions.TextBox(htmlHelper, name, value, format, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString TextBox(
      this HtmlHelper htmlHelper,
      string name,
      object value,
      IDictionary<string, object> htmlAttributes)
    {
      return InputExtensions.TextBox(htmlHelper, name, value, (string) null, htmlAttributes);
    }

    public static MvcHtmlString TextBox(
      this HtmlHelper htmlHelper,
      string name,
      object value,
      string format,
      IDictionary<string, object> htmlAttributes)
    {
      return InputExtensions.InputHelper(htmlHelper, InputType.Text, (ModelMetadata) null, name, value, value == null, false, true, true, format, htmlAttributes);
    }

    public static MvcHtmlString TextBoxFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression)
    {
      return InputExtensions.TextBoxFor<TModel, TProperty>(htmlHelper, expression, (string) null);
    }

    public static MvcHtmlString TextBoxFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      string format)
    {
      return InputExtensions.TextBoxFor<TModel, TProperty>(htmlHelper, expression, format, (IDictionary<string, object>) null);
    }

    public static MvcHtmlString TextBoxFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      object htmlAttributes)
    {
      return htmlHelper.TextBoxFor<TModel, TProperty>(expression, (string) null, htmlAttributes);
    }

    public static MvcHtmlString TextBoxFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      string format,
      object htmlAttributes)
    {
      return InputExtensions.TextBoxFor<TModel, TProperty>(htmlHelper, expression, format, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString TextBoxFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      IDictionary<string, object> htmlAttributes)
    {
      return InputExtensions.TextBoxFor<TModel, TProperty>(htmlHelper, expression, (string) null, htmlAttributes);
    }

    public static MvcHtmlString TextBoxFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      string format,
      IDictionary<string, object> htmlAttributes)
    {
      ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
      return htmlHelper.TextBoxHelper(metadata, metadata.Model, ExpressionHelper.GetExpressionText((LambdaExpression) expression), format, htmlAttributes);
    }

    private static MvcHtmlString TextBoxHelper(
      this HtmlHelper htmlHelper,
      ModelMetadata metadata,
      object model,
      string expression,
      string format,
      IDictionary<string, object> htmlAttributes)
    {
      return InputExtensions.InputHelper(htmlHelper, InputType.Text, metadata, expression, model, false, false, true, true, format, htmlAttributes);
    }

    private static MvcHtmlString InputHelper(
      HtmlHelper htmlHelper,
      InputType inputType,
      ModelMetadata metadata,
      string name,
      object value,
      bool useViewData,
      bool isChecked,
      bool setId,
      bool isExplicitValue,
      string format,
      IDictionary<string, object> htmlAttributes)
    {
      string fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
      if (string.IsNullOrEmpty(fullHtmlFieldName))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (name));
      TagBuilder tagBuilder1 = new TagBuilder("input");
      tagBuilder1.MergeAttributes<string, object>(htmlAttributes);
      tagBuilder1.MergeAttribute("type", HtmlHelper.GetInputTypeString(inputType));
      tagBuilder1.MergeAttribute(nameof (name), fullHtmlFieldName, true);
      string b = htmlHelper.FormatValue(value, format);
      bool flag = false;
      switch (inputType)
      {
        case InputType.CheckBox:
          bool? modelStateValue1 = htmlHelper.GetModelStateValue(fullHtmlFieldName, typeof (bool)) as bool?;
          if (modelStateValue1.HasValue)
          {
            isChecked = modelStateValue1.Value;
            flag = true;
            goto case InputType.Radio;
          }
          else
            goto case InputType.Radio;
        case InputType.Password:
          if (value != null)
          {
            tagBuilder1.MergeAttribute(nameof (value), b, isExplicitValue);
            break;
          }
          break;
        case InputType.Radio:
          if (!flag && htmlHelper.GetModelStateValue(fullHtmlFieldName, typeof (string)) is string modelStateValue2)
          {
            isChecked = string.Equals(modelStateValue2, b, StringComparison.Ordinal);
            flag = true;
          }
          if (!flag && useViewData)
            isChecked = htmlHelper.EvalBoolean(fullHtmlFieldName);
          if (isChecked)
            tagBuilder1.MergeAttribute("checked", "checked");
          tagBuilder1.MergeAttribute(nameof (value), b, isExplicitValue);
          break;
        default:
          string modelStateValue3 = (string) htmlHelper.GetModelStateValue(fullHtmlFieldName, typeof (string));
          tagBuilder1.MergeAttribute(nameof (value), modelStateValue3 ?? (useViewData ? htmlHelper.EvalString(fullHtmlFieldName, format) : b), isExplicitValue);
          break;
      }
      if (setId)
        tagBuilder1.GenerateId(fullHtmlFieldName);
      ModelState modelState;
      if (htmlHelper.ViewData.ModelState.TryGetValue(fullHtmlFieldName, out modelState) && modelState.Errors.Count > 0)
        tagBuilder1.AddCssClass(HtmlHelper.ValidationInputCssClassName);
      tagBuilder1.MergeAttributes<string, object>(htmlHelper.GetUnobtrusiveValidationAttributes(name, metadata));
      if (inputType != InputType.CheckBox)
        return tagBuilder1.ToMvcHtmlString(TagRenderMode.SelfClosing);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(tagBuilder1.ToString(TagRenderMode.SelfClosing));
      TagBuilder tagBuilder2 = new TagBuilder("input");
      tagBuilder2.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.Hidden));
      tagBuilder2.MergeAttribute(nameof (name), fullHtmlFieldName);
      tagBuilder2.MergeAttribute(nameof (value), "false");
      stringBuilder.Append(tagBuilder2.ToString(TagRenderMode.SelfClosing));
      return MvcHtmlString.Create(stringBuilder.ToString());
    }

    private static RouteValueDictionary ToRouteValueDictionary(
      IDictionary<string, object> dictionary)
    {
      return dictionary != null ? new RouteValueDictionary(dictionary) : new RouteValueDictionary();
    }
  }
}
