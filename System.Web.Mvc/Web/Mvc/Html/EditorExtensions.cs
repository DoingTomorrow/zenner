// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Html.EditorExtensions
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Linq.Expressions;
using System.Web.UI.WebControls;

#nullable disable
namespace System.Web.Mvc.Html
{
  public static class EditorExtensions
  {
    public static MvcHtmlString Editor(this HtmlHelper html, string expression)
    {
      return TemplateHelpers.Template(html, expression, (string) null, (string) null, DataBoundControlMode.Edit, (object) null);
    }

    public static MvcHtmlString Editor(
      this HtmlHelper html,
      string expression,
      object additionalViewData)
    {
      return TemplateHelpers.Template(html, expression, (string) null, (string) null, DataBoundControlMode.Edit, additionalViewData);
    }

    public static MvcHtmlString Editor(
      this HtmlHelper html,
      string expression,
      string templateName)
    {
      return TemplateHelpers.Template(html, expression, templateName, (string) null, DataBoundControlMode.Edit, (object) null);
    }

    public static MvcHtmlString Editor(
      this HtmlHelper html,
      string expression,
      string templateName,
      object additionalViewData)
    {
      return TemplateHelpers.Template(html, expression, templateName, (string) null, DataBoundControlMode.Edit, additionalViewData);
    }

    public static MvcHtmlString Editor(
      this HtmlHelper html,
      string expression,
      string templateName,
      string htmlFieldName)
    {
      return TemplateHelpers.Template(html, expression, templateName, htmlFieldName, DataBoundControlMode.Edit, (object) null);
    }

    public static MvcHtmlString Editor(
      this HtmlHelper html,
      string expression,
      string templateName,
      string htmlFieldName,
      object additionalViewData)
    {
      return TemplateHelpers.Template(html, expression, templateName, htmlFieldName, DataBoundControlMode.Edit, additionalViewData);
    }

    public static MvcHtmlString EditorFor<TModel, TValue>(
      this HtmlHelper<TModel> html,
      Expression<Func<TModel, TValue>> expression)
    {
      return html.TemplateFor<TModel, TValue>(expression, (string) null, (string) null, DataBoundControlMode.Edit, (object) null);
    }

    public static MvcHtmlString EditorFor<TModel, TValue>(
      this HtmlHelper<TModel> html,
      Expression<Func<TModel, TValue>> expression,
      object additionalViewData)
    {
      return html.TemplateFor<TModel, TValue>(expression, (string) null, (string) null, DataBoundControlMode.Edit, additionalViewData);
    }

    public static MvcHtmlString EditorFor<TModel, TValue>(
      this HtmlHelper<TModel> html,
      Expression<Func<TModel, TValue>> expression,
      string templateName)
    {
      return html.TemplateFor<TModel, TValue>(expression, templateName, (string) null, DataBoundControlMode.Edit, (object) null);
    }

    public static MvcHtmlString EditorFor<TModel, TValue>(
      this HtmlHelper<TModel> html,
      Expression<Func<TModel, TValue>> expression,
      string templateName,
      object additionalViewData)
    {
      return html.TemplateFor<TModel, TValue>(expression, templateName, (string) null, DataBoundControlMode.Edit, additionalViewData);
    }

    public static MvcHtmlString EditorFor<TModel, TValue>(
      this HtmlHelper<TModel> html,
      Expression<Func<TModel, TValue>> expression,
      string templateName,
      string htmlFieldName)
    {
      return html.TemplateFor<TModel, TValue>(expression, templateName, htmlFieldName, DataBoundControlMode.Edit, (object) null);
    }

    public static MvcHtmlString EditorFor<TModel, TValue>(
      this HtmlHelper<TModel> html,
      Expression<Func<TModel, TValue>> expression,
      string templateName,
      string htmlFieldName,
      object additionalViewData)
    {
      return html.TemplateFor<TModel, TValue>(expression, templateName, htmlFieldName, DataBoundControlMode.Edit, additionalViewData);
    }

    public static MvcHtmlString EditorForModel(this HtmlHelper html)
    {
      return MvcHtmlString.Create(TemplateHelpers.TemplateHelper(html, html.ViewData.ModelMetadata, string.Empty, (string) null, DataBoundControlMode.Edit, (object) null));
    }

    public static MvcHtmlString EditorForModel(this HtmlHelper html, object additionalViewData)
    {
      return MvcHtmlString.Create(TemplateHelpers.TemplateHelper(html, html.ViewData.ModelMetadata, string.Empty, (string) null, DataBoundControlMode.Edit, additionalViewData));
    }

    public static MvcHtmlString EditorForModel(this HtmlHelper html, string templateName)
    {
      return MvcHtmlString.Create(TemplateHelpers.TemplateHelper(html, html.ViewData.ModelMetadata, string.Empty, templateName, DataBoundControlMode.Edit, (object) null));
    }

    public static MvcHtmlString EditorForModel(
      this HtmlHelper html,
      string templateName,
      object additionalViewData)
    {
      return MvcHtmlString.Create(TemplateHelpers.TemplateHelper(html, html.ViewData.ModelMetadata, string.Empty, templateName, DataBoundControlMode.Edit, additionalViewData));
    }

    public static MvcHtmlString EditorForModel(
      this HtmlHelper html,
      string templateName,
      string htmlFieldName)
    {
      return MvcHtmlString.Create(TemplateHelpers.TemplateHelper(html, html.ViewData.ModelMetadata, htmlFieldName, templateName, DataBoundControlMode.Edit, (object) null));
    }

    public static MvcHtmlString EditorForModel(
      this HtmlHelper html,
      string templateName,
      string htmlFieldName,
      object additionalViewData)
    {
      return MvcHtmlString.Create(TemplateHelpers.TemplateHelper(html, html.ViewData.ModelMetadata, htmlFieldName, templateName, DataBoundControlMode.Edit, additionalViewData));
    }
  }
}
