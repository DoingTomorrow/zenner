// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Html.DefaultEditorTemplates
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Globalization;
using System.Text;
using System.Web.Mvc.Properties;
using System.Web.UI.WebControls;

#nullable disable
namespace System.Web.Mvc.Html
{
  internal static class DefaultEditorTemplates
  {
    internal static string BooleanTemplate(HtmlHelper html)
    {
      bool? nullable = new bool?();
      if (html.ViewContext.ViewData.Model != null)
        nullable = new bool?(Convert.ToBoolean(html.ViewContext.ViewData.Model, (IFormatProvider) CultureInfo.InvariantCulture));
      return !html.ViewContext.ViewData.ModelMetadata.IsNullableValueType ? DefaultEditorTemplates.BooleanTemplateCheckbox(html, ((int) nullable ?? 0) != 0) : DefaultEditorTemplates.BooleanTemplateDropDownList(html, nullable);
    }

    private static string BooleanTemplateCheckbox(HtmlHelper html, bool value)
    {
      return InputExtensions.CheckBox(html, string.Empty, value, DefaultEditorTemplates.CreateHtmlAttributes("check-box")).ToHtmlString();
    }

    private static string BooleanTemplateDropDownList(HtmlHelper html, bool? value)
    {
      return SelectExtensions.DropDownList(html, string.Empty, (IEnumerable<SelectListItem>) DefaultEditorTemplates.TriStateValues(value), DefaultEditorTemplates.CreateHtmlAttributes("list-box tri-state")).ToHtmlString();
    }

    internal static string CollectionTemplate(HtmlHelper html)
    {
      return DefaultEditorTemplates.CollectionTemplate(html, new TemplateHelpers.TemplateHelperDelegate(TemplateHelpers.TemplateHelper));
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
          string str2 = templateHelper(html, metadataForType, htmlFieldName, (string) null, DataBoundControlMode.Edit, (object) null);
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
      return DefaultEditorTemplates.StringTemplate(html);
    }

    internal static string HiddenInputTemplate(HtmlHelper html)
    {
      string str = !html.ViewContext.ViewData.ModelMetadata.HideSurroundingHtml ? DefaultDisplayTemplates.StringTemplate(html) : string.Empty;
      object obj = html.ViewContext.ViewData.Model;
      Binary binary = obj as Binary;
      if (binary != (Binary) null)
        obj = (object) Convert.ToBase64String(binary.ToArray());
      else if (obj is byte[] inArray)
        obj = (object) Convert.ToBase64String(inArray);
      return str + html.Hidden(string.Empty, obj).ToHtmlString();
    }

    internal static string MultilineTextTemplate(HtmlHelper html)
    {
      return TextAreaExtensions.TextArea(html, string.Empty, html.ViewContext.ViewData.TemplateInfo.FormattedModelValue.ToString(), 0, 0, DefaultEditorTemplates.CreateHtmlAttributes("text-box multi-line")).ToHtmlString();
    }

    private static IDictionary<string, object> CreateHtmlAttributes(
      string className,
      string inputType = null)
    {
      Dictionary<string, object> htmlAttributes = new Dictionary<string, object>()
      {
        {
          "class",
          (object) className
        }
      };
      if (inputType != null)
        htmlAttributes.Add("type", (object) inputType);
      return (IDictionary<string, object>) htmlAttributes;
    }

    internal static string ObjectTemplate(HtmlHelper html)
    {
      return DefaultEditorTemplates.ObjectTemplate(html, new TemplateHelpers.TemplateHelperDelegate(TemplateHelpers.TemplateHelper));
    }

    internal static string ObjectTemplate(
      HtmlHelper html,
      TemplateHelpers.TemplateHelperDelegate templateHelper)
    {
      ViewDataDictionary viewData = html.ViewContext.ViewData;
      TemplateInfo templateInfo = viewData.TemplateInfo;
      ModelMetadata modelMetadata = viewData.ModelMetadata;
      StringBuilder stringBuilder = new StringBuilder();
      if (templateInfo.TemplateDepth > 1)
      {
        if (modelMetadata.Model == null)
          return modelMetadata.NullDisplayText;
        string str = modelMetadata.SimpleDisplayText;
        if (modelMetadata.HtmlEncode)
          str = html.Encode(str);
        return str;
      }
      foreach (ModelMetadata metadata in modelMetadata.Properties.Where<ModelMetadata>((Func<ModelMetadata, bool>) (pm => DefaultEditorTemplates.ShouldShow(pm, templateInfo))))
      {
        if (!metadata.HideSurroundingHtml)
        {
          string htmlString = LabelExtensions.LabelHelper(html, metadata, metadata.PropertyName).ToHtmlString();
          if (!string.IsNullOrEmpty(htmlString))
            stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "<div class=\"editor-label\">{0}</div>\r\n", new object[1]
            {
              (object) htmlString
            });
          stringBuilder.Append("<div class=\"editor-field\">");
        }
        stringBuilder.Append(templateHelper(html, metadata, metadata.PropertyName, (string) null, DataBoundControlMode.Edit, (object) null));
        if (!metadata.HideSurroundingHtml)
        {
          stringBuilder.Append(" ");
          stringBuilder.Append((object) html.ValidationMessage(metadata.PropertyName));
          stringBuilder.Append("</div>\r\n");
        }
      }
      return stringBuilder.ToString();
    }

    internal static string PasswordTemplate(HtmlHelper html)
    {
      return InputExtensions.Password(html, string.Empty, html.ViewContext.ViewData.TemplateInfo.FormattedModelValue, DefaultEditorTemplates.CreateHtmlAttributes("text-box single-line password")).ToHtmlString();
    }

    private static bool ShouldShow(ModelMetadata metadata, TemplateInfo templateInfo)
    {
      return metadata.ShowForEdit && metadata.ModelType != typeof (EntityState) && !metadata.IsComplexType && !templateInfo.Visited(metadata);
    }

    internal static string StringTemplate(HtmlHelper html)
    {
      return DefaultEditorTemplates.HtmlInputTemplateHelper(html);
    }

    internal static string PhoneNumberInputTemplate(HtmlHelper html)
    {
      return DefaultEditorTemplates.HtmlInputTemplateHelper(html, "tel");
    }

    internal static string UrlInputTemplate(HtmlHelper html)
    {
      return DefaultEditorTemplates.HtmlInputTemplateHelper(html, "url");
    }

    internal static string EmailAddressInputTemplate(HtmlHelper html)
    {
      return DefaultEditorTemplates.HtmlInputTemplateHelper(html, "email");
    }

    internal static string DateTimeInputTemplate(HtmlHelper html)
    {
      return DefaultEditorTemplates.HtmlInputTemplateHelper(html, "datetime");
    }

    internal static string DateInputTemplate(HtmlHelper html)
    {
      return DefaultEditorTemplates.HtmlInputTemplateHelper(html, "date");
    }

    internal static string TimeInputTemplate(HtmlHelper html)
    {
      return DefaultEditorTemplates.HtmlInputTemplateHelper(html, "time");
    }

    internal static string NumberInputTemplate(HtmlHelper html)
    {
      return DefaultEditorTemplates.HtmlInputTemplateHelper(html, "number");
    }

    private static string HtmlInputTemplateHelper(HtmlHelper html, string inputType = null)
    {
      return InputExtensions.TextBox(html, string.Empty, html.ViewContext.ViewData.TemplateInfo.FormattedModelValue, DefaultEditorTemplates.CreateHtmlAttributes("text-box single-line", inputType)).ToHtmlString();
    }

    internal static List<SelectListItem> TriStateValues(bool? value)
    {
      return new List<SelectListItem>()
      {
        new SelectListItem()
        {
          Text = MvcResources.Common_TriState_NotSet,
          Value = string.Empty,
          Selected = !value.HasValue
        },
        new SelectListItem()
        {
          Text = MvcResources.Common_TriState_True,
          Value = "true",
          Selected = value.HasValue && value.Value
        },
        new SelectListItem()
        {
          Text = MvcResources.Common_TriState_False,
          Value = "false",
          Selected = value.HasValue && !value.Value
        }
      };
    }
  }
}
