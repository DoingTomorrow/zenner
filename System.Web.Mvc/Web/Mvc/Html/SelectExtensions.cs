// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Html.SelectExtensions
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc.Html
{
  public static class SelectExtensions
  {
    public static MvcHtmlString DropDownList(this HtmlHelper htmlHelper, string name)
    {
      return SelectExtensions.DropDownList(htmlHelper, name, (IEnumerable<SelectListItem>) null, (string) null, (IDictionary<string, object>) null);
    }

    public static MvcHtmlString DropDownList(
      this HtmlHelper htmlHelper,
      string name,
      string optionLabel)
    {
      return SelectExtensions.DropDownList(htmlHelper, name, (IEnumerable<SelectListItem>) null, optionLabel, (IDictionary<string, object>) null);
    }

    public static MvcHtmlString DropDownList(
      this HtmlHelper htmlHelper,
      string name,
      IEnumerable<SelectListItem> selectList)
    {
      return SelectExtensions.DropDownList(htmlHelper, name, selectList, (string) null, (IDictionary<string, object>) null);
    }

    public static MvcHtmlString DropDownList(
      this HtmlHelper htmlHelper,
      string name,
      IEnumerable<SelectListItem> selectList,
      object htmlAttributes)
    {
      return SelectExtensions.DropDownList(htmlHelper, name, selectList, (string) null, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString DropDownList(
      this HtmlHelper htmlHelper,
      string name,
      IEnumerable<SelectListItem> selectList,
      IDictionary<string, object> htmlAttributes)
    {
      return SelectExtensions.DropDownList(htmlHelper, name, selectList, (string) null, htmlAttributes);
    }

    public static MvcHtmlString DropDownList(
      this HtmlHelper htmlHelper,
      string name,
      IEnumerable<SelectListItem> selectList,
      string optionLabel)
    {
      return SelectExtensions.DropDownList(htmlHelper, name, selectList, optionLabel, (IDictionary<string, object>) null);
    }

    public static MvcHtmlString DropDownList(
      this HtmlHelper htmlHelper,
      string name,
      IEnumerable<SelectListItem> selectList,
      string optionLabel,
      object htmlAttributes)
    {
      return SelectExtensions.DropDownList(htmlHelper, name, selectList, optionLabel, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString DropDownList(
      this HtmlHelper htmlHelper,
      string name,
      IEnumerable<SelectListItem> selectList,
      string optionLabel,
      IDictionary<string, object> htmlAttributes)
    {
      return SelectExtensions.DropDownListHelper(htmlHelper, (ModelMetadata) null, name, selectList, optionLabel, htmlAttributes);
    }

    public static MvcHtmlString DropDownListFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      IEnumerable<SelectListItem> selectList)
    {
      return SelectExtensions.DropDownListFor<TModel, TProperty>(htmlHelper, expression, selectList, (string) null, (IDictionary<string, object>) null);
    }

    public static MvcHtmlString DropDownListFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      IEnumerable<SelectListItem> selectList,
      object htmlAttributes)
    {
      return SelectExtensions.DropDownListFor<TModel, TProperty>(htmlHelper, expression, selectList, (string) null, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString DropDownListFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      IEnumerable<SelectListItem> selectList,
      IDictionary<string, object> htmlAttributes)
    {
      return SelectExtensions.DropDownListFor<TModel, TProperty>(htmlHelper, expression, selectList, (string) null, htmlAttributes);
    }

    public static MvcHtmlString DropDownListFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      IEnumerable<SelectListItem> selectList,
      string optionLabel)
    {
      return SelectExtensions.DropDownListFor<TModel, TProperty>(htmlHelper, expression, selectList, optionLabel, (IDictionary<string, object>) null);
    }

    public static MvcHtmlString DropDownListFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      IEnumerable<SelectListItem> selectList,
      string optionLabel,
      object htmlAttributes)
    {
      return SelectExtensions.DropDownListFor<TModel, TProperty>(htmlHelper, expression, selectList, optionLabel, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString DropDownListFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      IEnumerable<SelectListItem> selectList,
      string optionLabel,
      IDictionary<string, object> htmlAttributes)
    {
      if (expression == null)
        throw new ArgumentNullException(nameof (expression));
      ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
      return SelectExtensions.DropDownListHelper((HtmlHelper) htmlHelper, metadata, ExpressionHelper.GetExpressionText((LambdaExpression) expression), selectList, optionLabel, htmlAttributes);
    }

    private static MvcHtmlString DropDownListHelper(
      HtmlHelper htmlHelper,
      ModelMetadata metadata,
      string expression,
      IEnumerable<SelectListItem> selectList,
      string optionLabel,
      IDictionary<string, object> htmlAttributes)
    {
      return htmlHelper.SelectInternal(metadata, optionLabel, expression, selectList, false, htmlAttributes);
    }

    public static MvcHtmlString ListBox(this HtmlHelper htmlHelper, string name)
    {
      return SelectExtensions.ListBox(htmlHelper, name, (IEnumerable<SelectListItem>) null, (IDictionary<string, object>) null);
    }

    public static MvcHtmlString ListBox(
      this HtmlHelper htmlHelper,
      string name,
      IEnumerable<SelectListItem> selectList)
    {
      return SelectExtensions.ListBox(htmlHelper, name, selectList, (IDictionary<string, object>) null);
    }

    public static MvcHtmlString ListBox(
      this HtmlHelper htmlHelper,
      string name,
      IEnumerable<SelectListItem> selectList,
      object htmlAttributes)
    {
      return SelectExtensions.ListBox(htmlHelper, name, selectList, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString ListBox(
      this HtmlHelper htmlHelper,
      string name,
      IEnumerable<SelectListItem> selectList,
      IDictionary<string, object> htmlAttributes)
    {
      return SelectExtensions.ListBoxHelper(htmlHelper, (ModelMetadata) null, name, selectList, htmlAttributes);
    }

    public static MvcHtmlString ListBoxFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      IEnumerable<SelectListItem> selectList)
    {
      return SelectExtensions.ListBoxFor<TModel, TProperty>(htmlHelper, expression, selectList, (IDictionary<string, object>) null);
    }

    public static MvcHtmlString ListBoxFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      IEnumerable<SelectListItem> selectList,
      object htmlAttributes)
    {
      return SelectExtensions.ListBoxFor<TModel, TProperty>(htmlHelper, expression, selectList, (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public static MvcHtmlString ListBoxFor<TModel, TProperty>(
      this HtmlHelper<TModel> htmlHelper,
      Expression<Func<TModel, TProperty>> expression,
      IEnumerable<SelectListItem> selectList,
      IDictionary<string, object> htmlAttributes)
    {
      if (expression == null)
        throw new ArgumentNullException(nameof (expression));
      ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
      return SelectExtensions.ListBoxHelper((HtmlHelper) htmlHelper, metadata, ExpressionHelper.GetExpressionText((LambdaExpression) expression), selectList, htmlAttributes);
    }

    private static MvcHtmlString ListBoxHelper(
      HtmlHelper htmlHelper,
      ModelMetadata metadata,
      string name,
      IEnumerable<SelectListItem> selectList,
      IDictionary<string, object> htmlAttributes)
    {
      return htmlHelper.SelectInternal(metadata, (string) null, name, selectList, true, htmlAttributes);
    }

    private static IEnumerable<SelectListItem> GetSelectData(
      this HtmlHelper htmlHelper,
      string name)
    {
      object obj = (object) null;
      if (htmlHelper.ViewData != null)
        obj = htmlHelper.ViewData.Eval(name);
      if (obj == null)
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.HtmlHelper_MissingSelectData, new object[2]
        {
          (object) name,
          (object) "IEnumerable<SelectListItem>"
        }));
      return obj is IEnumerable<SelectListItem> selectListItems ? selectListItems : throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.HtmlHelper_WrongSelectDataType, new object[3]
      {
        (object) name,
        (object) obj.GetType().FullName,
        (object) "IEnumerable<SelectListItem>"
      }));
    }

    internal static string ListItemToOption(SelectListItem item)
    {
      TagBuilder tagBuilder = new TagBuilder("option")
      {
        InnerHtml = HttpUtility.HtmlEncode(item.Text)
      };
      if (item.Value != null)
        tagBuilder.Attributes["value"] = item.Value;
      if (item.Selected)
        tagBuilder.Attributes["selected"] = "selected";
      return tagBuilder.ToString(TagRenderMode.Normal);
    }

    private static IEnumerable<SelectListItem> GetSelectListWithDefaultValue(
      IEnumerable<SelectListItem> selectList,
      object defaultValue,
      bool allowMultiple)
    {
      if (allowMultiple)
      {
        if (!(defaultValue is IEnumerable source) || source is string)
          throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.HtmlHelper_SelectExpressionNotEnumerable, new object[1]
          {
            (object) "expression"
          }));
      }
      else
        source = (IEnumerable) new object[1]{ defaultValue };
      HashSet<string> stringSet = new HashSet<string>(source.Cast<object>().Select<object, string>((Func<object, string>) (value => Convert.ToString(value, (IFormatProvider) CultureInfo.CurrentCulture))), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      List<SelectListItem> withDefaultValue = new List<SelectListItem>();
      foreach (SelectListItem select in selectList)
      {
        select.Selected = select.Value != null ? stringSet.Contains(select.Value) : stringSet.Contains(select.Text);
        withDefaultValue.Add(select);
      }
      return (IEnumerable<SelectListItem>) withDefaultValue;
    }

    private static MvcHtmlString SelectInternal(
      this HtmlHelper htmlHelper,
      ModelMetadata metadata,
      string optionLabel,
      string name,
      IEnumerable<SelectListItem> selectList,
      bool allowMultiple,
      IDictionary<string, object> htmlAttributes)
    {
      string fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
      if (string.IsNullOrEmpty(fullHtmlFieldName))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (name));
      bool flag = false;
      if (selectList == null)
      {
        selectList = htmlHelper.GetSelectData(name);
        flag = true;
      }
      object defaultValue = allowMultiple ? htmlHelper.GetModelStateValue(fullHtmlFieldName, typeof (string[])) : htmlHelper.GetModelStateValue(fullHtmlFieldName, typeof (string));
      if (!flag && defaultValue == null && !string.IsNullOrEmpty(name))
        defaultValue = htmlHelper.ViewData.Eval(name);
      if (defaultValue != null)
        selectList = SelectExtensions.GetSelectListWithDefaultValue(selectList, defaultValue, allowMultiple);
      StringBuilder stringBuilder = new StringBuilder();
      if (optionLabel != null)
        stringBuilder.AppendLine(SelectExtensions.ListItemToOption(new SelectListItem()
        {
          Text = optionLabel,
          Value = string.Empty,
          Selected = false
        }));
      foreach (SelectListItem select in selectList)
        stringBuilder.AppendLine(SelectExtensions.ListItemToOption(select));
      TagBuilder tagBuilder = new TagBuilder("select")
      {
        InnerHtml = stringBuilder.ToString()
      };
      tagBuilder.MergeAttributes<string, object>(htmlAttributes);
      tagBuilder.MergeAttribute(nameof (name), fullHtmlFieldName, true);
      tagBuilder.GenerateId(fullHtmlFieldName);
      if (allowMultiple)
        tagBuilder.MergeAttribute("multiple", "multiple");
      ModelState modelState;
      if (htmlHelper.ViewData.ModelState.TryGetValue(fullHtmlFieldName, out modelState) && modelState.Errors.Count > 0)
        tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
      tagBuilder.MergeAttributes<string, object>(htmlHelper.GetUnobtrusiveValidationAttributes(name, metadata));
      return tagBuilder.ToMvcHtmlString(TagRenderMode.Normal);
    }
  }
}
