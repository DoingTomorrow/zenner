// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Html.DefaultDisplayTemplates
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc.Properties;
using System.Web.UI.WebControls;

#nullable disable
namespace System.Web.Mvc.Html
{
  internal static class DefaultDisplayTemplates
  {
    internal static string BooleanTemplate(HtmlHelper html)
    {
      bool? nullable = new bool?();
      if (html.ViewContext.ViewData.Model != null)
        nullable = new bool?(Convert.ToBoolean(html.ViewContext.ViewData.Model, (IFormatProvider) CultureInfo.InvariantCulture));
      return !html.ViewContext.ViewData.ModelMetadata.IsNullableValueType ? DefaultDisplayTemplates.BooleanTemplateCheckbox(((int) nullable ?? 0) != 0) : DefaultDisplayTemplates.BooleanTemplateDropDownList(nullable);
    }

    private static string BooleanTemplateCheckbox(bool value)
    {
      TagBuilder tagBuilder = new TagBuilder("input");
      tagBuilder.AddCssClass("check-box");
      tagBuilder.Attributes["disabled"] = "disabled";
      tagBuilder.Attributes["type"] = "checkbox";
      if (value)
        tagBuilder.Attributes["checked"] = "checked";
      return tagBuilder.ToString(TagRenderMode.SelfClosing);
    }

    private static string BooleanTemplateDropDownList(bool? value)
    {
      StringBuilder stringBuilder = new StringBuilder();
      TagBuilder tagBuilder = new TagBuilder("select");
      tagBuilder.AddCssClass("list-box");
      tagBuilder.AddCssClass("tri-state");
      tagBuilder.Attributes["disabled"] = "disabled";
      stringBuilder.Append(tagBuilder.ToString(TagRenderMode.StartTag));
      foreach (SelectListItem triStateValue in DefaultEditorTemplates.TriStateValues(value))
        stringBuilder.Append(SelectExtensions.ListItemToOption(triStateValue));
      stringBuilder.Append(tagBuilder.ToString(TagRenderMode.EndTag));
      return stringBuilder.ToString();
    }

    internal static string CollectionTemplate(HtmlHelper html)
    {
      return DefaultDisplayTemplates.CollectionTemplate(html, new TemplateHelpers.TemplateHelperDelegate(TemplateHelpers.TemplateHelper));
    }

    internal static string CollectionTemplate(
      HtmlHelper html,
      TemplateHelpers.TemplateHelperDelegate templateHelper)
    {
      object model = html.ViewContext.ViewData.ModelMetadata.Model;
      if (model == null)
        return string.Empty;
      if (!(model is IEnumerable enumerable))
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.Templates_TypeMustImplementIEnumerable, new object[1]
        {
          (object) model.GetType().FullName
        }));
      Type type = typeof (string);
      Type genericInterface = TypeHelpers.ExtractGenericInterface(enumerable.GetType(), typeof (IEnumerable<>));
      if (genericInterface != (Type) null)
        type = genericInterface.GetGenericArguments()[0];
      bool flag = TypeHelpers.IsNullableValueType(type);
      string htmlFieldPrefix = html.ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix;
      try
      {
        html.ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix = string.Empty;
        string str1 = htmlFieldPrefix;
        StringBuilder stringBuilder = new StringBuilder();
        int num = 0;
        foreach (object obj in enumerable)
        {
          object item = obj;
          Type modelType = type;
          if (item != null && !flag)
            modelType = item.GetType();
          ModelMetadata metadataForType = ModelMetadataProviders.Current.GetMetadataForType((Func<object>) (() => item), modelType);
          string htmlFieldName = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}[{1}]", new object[2]
          {
            (object) str1,
            (object) num++
          });
          string str2 = templateHelper(html, metadataForType, htmlFieldName, (string) null, DataBoundControlMode.ReadOnly, (object) null);
          stringBuilder.Append(str2);
        }
        return stringBuilder.ToString();
      }
      finally
      {
        html.ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix = htmlFieldPrefix;
      }
    }

    internal static string DecimalTemplate(HtmlHelper html)
    {
      if (html.ViewContext.ViewData.TemplateInfo.FormattedModelValue == html.ViewContext.ViewData.ModelMetadata.Model)
        html.ViewContext.ViewData.TemplateInfo.FormattedModelValue = (object) string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0:0.00}", new object[1]
        {
          html.ViewContext.ViewData.ModelMetadata.Model
        });
      return DefaultDisplayTemplates.StringTemplate(html);
    }

    internal static string EmailAddressTemplate(HtmlHelper html)
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "<a href=\"mailto:{0}\">{1}</a>", new object[2]
      {
        (object) html.AttributeEncode(html.ViewContext.ViewData.Model),
        (object) html.Encode(html.ViewContext.ViewData.TemplateInfo.FormattedModelValue)
      });
    }

    internal static string HiddenInputTemplate(HtmlHelper html)
    {
      return html.ViewContext.ViewData.ModelMetadata.HideSurroundingHtml ? string.Empty : DefaultDisplayTemplates.StringTemplate(html);
    }

    internal static string HtmlTemplate(HtmlHelper html)
    {
      return html.ViewContext.ViewData.TemplateInfo.FormattedModelValue.ToString();
    }

    internal static string ObjectTemplate(HtmlHelper html)
    {
      return DefaultDisplayTemplates.ObjectTemplate(html, new TemplateHelpers.TemplateHelperDelegate(TemplateHelpers.TemplateHelper));
    }

    internal static string ObjectTemplate(
      HtmlHelper html,
      TemplateHelpers.TemplateHelperDelegate templateHelper)
    {
      ViewDataDictionary viewData = html.ViewContext.ViewData;
      TemplateInfo templateInfo = viewData.TemplateInfo;
      ModelMetadata modelMetadata = viewData.ModelMetadata;
      StringBuilder stringBuilder = new StringBuilder();
      if (modelMetadata.Model == null)
        return modelMetadata.NullDisplayText;
      if (templateInfo.TemplateDepth > 1)
      {
        string str = modelMetadata.SimpleDisplayText;
        if (modelMetadata.HtmlEncode)
          str = html.Encode(str);
        return str;
      }
      foreach (ModelMetadata metadata in modelMetadata.Properties.Where<ModelMetadata>((Func<ModelMetadata, bool>) (pm => DefaultDisplayTemplates.ShouldShow(pm, templateInfo))))
      {
        if (!metadata.HideSurroundingHtml)
        {
          string displayName = metadata.GetDisplayName();
          if (!string.IsNullOrEmpty(displayName))
          {
            stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "<div class=\"display-label\">{0}</div>", new object[1]
            {
              (object) displayName
            });
            stringBuilder.AppendLine();
          }
          stringBuilder.Append("<div class=\"display-field\">");
        }
        stringBuilder.Append(templateHelper(html, metadata, metadata.PropertyName, (string) null, DataBoundControlMode.ReadOnly, (object) null));
        if (!metadata.HideSurroundingHtml)
          stringBuilder.AppendLine("</div>");
      }
      return stringBuilder.ToString();
    }

    private static bool ShouldShow(ModelMetadata metadata, TemplateInfo templateInfo)
    {
      return metadata.ShowForDisplay && metadata.ModelType != typeof (EntityState) && !metadata.IsComplexType && !templateInfo.Visited(metadata);
    }

    internal static string StringTemplate(HtmlHelper html)
    {
      return html.Encode(html.ViewContext.ViewData.TemplateInfo.FormattedModelValue);
    }

    internal static string UrlTemplate(HtmlHelper html)
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "<a href=\"{0}\">{1}</a>", new object[2]
      {
        (object) html.AttributeEncode(html.ViewContext.ViewData.Model),
        (object) html.Encode(html.ViewContext.ViewData.TemplateInfo.FormattedModelValue)
      });
    }
  }
}
