// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Html.HtmlHelper
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using Microsoft.Internal.Web.Utils;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using System.Web.WebPages.Resources;
using System.Web.WebPages.Scope;

#nullable disable
namespace System.Web.WebPages.Html
{
  public class HtmlHelper
  {
    internal const string DefaultValidationInputErrorCssClass = "input-validation-error";
    private const string DefaultValidationInputValidCssClass = "input-validation-valid";
    private const string DefaultValidationMessageErrorCssClass = "field-validation-error";
    private const string DefaultValidationMessageValidCssClass = "field-validation-valid";
    private const string DefaultValidationSummaryErrorCssClass = "validation-summary-errors";
    private const string DefaultValidationSummaryValidCssClassName = "validation-summary-valid";
    private const int TextAreaRows = 2;
    private const int TextAreaColumns = 20;
    private static readonly object _validationMesssageErrorClassKey = new object();
    private static readonly object _validationMessageValidClassKey = new object();
    private static readonly object _validationInputErrorClassKey = new object();
    private static readonly object _validationInputValidClassKey = new object();
    private static readonly object _validationSummaryClassKey = new object();
    private static readonly object _validationSummaryValidClassKey = new object();
    private static readonly object _unobtrusiveValidationKey = new object();
    private static string _idAttributeDotReplacement;
    private readonly ValidationHelper _validationHelper;
    private static readonly IDictionary<string, object> _implicitRowsAndColumns = (IDictionary<string, object>) new Dictionary<string, object>()
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

    public IHtmlString CheckBox(string name)
    {
      IDictionary<string, object> htmlAttributes = (IDictionary<string, object>) null;
      return this.CheckBox(name, htmlAttributes);
    }

    public IHtmlString CheckBox(string name, object htmlAttributes)
    {
      return this.CheckBox(name, TypeHelper.ObjectToDictionary(htmlAttributes));
    }

    public IHtmlString CheckBox(string name, IDictionary<string, object> htmlAttributes)
    {
      return !string.IsNullOrEmpty(name) ? this.BuildCheckBox(name, new bool?(), htmlAttributes) : throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (name));
    }

    public IHtmlString CheckBox(string name, bool isChecked)
    {
      return this.CheckBox(name, isChecked, (IDictionary<string, object>) null);
    }

    public IHtmlString CheckBox(string name, bool isChecked, object htmlAttributes)
    {
      return this.CheckBox(name, isChecked, TypeHelper.ObjectToDictionary(htmlAttributes));
    }

    public IHtmlString CheckBox(
      string name,
      bool isChecked,
      IDictionary<string, object> htmlAttributes)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (name));
      return this.BuildCheckBox(name, new bool?(isChecked), htmlAttributes);
    }

    private IHtmlString BuildCheckBox(
      string name,
      bool? isChecked,
      IDictionary<string, object> attributes)
    {
      TagBuilder tagBuilder1 = new TagBuilder("input");
      tagBuilder1.MergeAttribute("type", "checkbox", (true ? 1 : 0) != 0);
      tagBuilder1.GenerateId(name);
      TagBuilder tagBuilder2 = tagBuilder1;
      bool flag1 = true;
      IDictionary<string, object> attributes1 = attributes;
      int num1 = flag1 ? 1 : 0;
      tagBuilder2.MergeAttributes<string, object>(attributes1, num1 != 0);
      TagBuilder tagBuilder3 = tagBuilder1;
      bool flag2 = true;
      string str = name;
      int num2 = flag2 ? 1 : 0;
      tagBuilder3.MergeAttribute(nameof (name), str, num2 != 0);
      if (HtmlHelper.UnobtrusiveJavaScriptEnabled)
      {
        IDictionary<string, object> validationAttributes = this._validationHelper.GetUnobtrusiveValidationAttributes(name);
        TagBuilder tagBuilder4 = tagBuilder1;
        bool flag3 = false;
        IDictionary<string, object> attributes2 = validationAttributes;
        int num3 = flag3 ? 1 : 0;
        tagBuilder4.MergeAttributes<string, object>(attributes2, num3 != 0);
      }
      System.Web.WebPages.Html.ModelState modelState = this.ModelState[name];
      if (modelState != null && modelState.Value != null)
      {
        bool flag4 = (bool) HtmlHelper.ConvertTo(modelState.Value, typeof (bool));
        isChecked = new bool?(((int) isChecked ?? (flag4 ? 1 : 0)) != 0);
      }
      if (isChecked.HasValue)
      {
        if (isChecked.Value)
          tagBuilder1.MergeAttribute("checked", "checked", (true ? 1 : 0) != 0);
        else
          tagBuilder1.Attributes.Remove("checked");
      }
      this.AddErrorClass(tagBuilder1, name);
      return (IHtmlString) tagBuilder1.ToHtmlString(TagRenderMode.SelfClosing);
    }

    internal HtmlHelper(ModelStateDictionary modelState, ValidationHelper validationHelper)
    {
      this.ModelState = modelState;
      this._validationHelper = validationHelper;
    }

    public static string IdAttributeDotReplacement
    {
      get
      {
        if (string.IsNullOrEmpty(HtmlHelper._idAttributeDotReplacement))
          HtmlHelper._idAttributeDotReplacement = "_";
        return HtmlHelper._idAttributeDotReplacement;
      }
      set => HtmlHelper._idAttributeDotReplacement = value;
    }

    public static string ValidationInputValidCssClassName
    {
      get
      {
        return ScopeStorage.CurrentScope[HtmlHelper._validationInputValidClassKey] is string str ? str : "input-validation-valid";
      }
      set
      {
        ScopeStorage.CurrentScope[HtmlHelper._validationInputValidClassKey] = value != null ? (object) value : throw new ArgumentNullException(nameof (value));
      }
    }

    public static string ValidationInputCssClassName
    {
      get
      {
        return ScopeStorage.CurrentScope[HtmlHelper._validationInputErrorClassKey] is string str ? str : "input-validation-error";
      }
      set
      {
        ScopeStorage.CurrentScope[HtmlHelper._validationInputErrorClassKey] = value != null ? (object) value : throw new ArgumentNullException(nameof (value));
      }
    }

    public static string ValidationMessageValidCssClassName
    {
      get
      {
        return ScopeStorage.CurrentScope[HtmlHelper._validationMessageValidClassKey] is string str ? str : "field-validation-valid";
      }
      set
      {
        ScopeStorage.CurrentScope[HtmlHelper._validationMessageValidClassKey] = value != null ? (object) value : throw new ArgumentNullException(nameof (value));
      }
    }

    public static string ValidationMessageCssClassName
    {
      get
      {
        return ScopeStorage.CurrentScope[HtmlHelper._validationMesssageErrorClassKey] is string str ? str : "field-validation-error";
      }
      set
      {
        ScopeStorage.CurrentScope[HtmlHelper._validationMesssageErrorClassKey] = value != null ? (object) value : throw new ArgumentNullException(nameof (value));
      }
    }

    public static string ValidationSummaryClass
    {
      get
      {
        return ScopeStorage.CurrentScope[HtmlHelper._validationSummaryClassKey] is string str ? str : "validation-summary-errors";
      }
      set
      {
        ScopeStorage.CurrentScope[HtmlHelper._validationSummaryClassKey] = value != null ? (object) value : throw new ArgumentNullException(nameof (value));
      }
    }

    public static string ValidationSummaryValidClass
    {
      get
      {
        return ScopeStorage.CurrentScope[HtmlHelper._validationSummaryValidClassKey] is string str ? str : "validation-summary-valid";
      }
      set
      {
        ScopeStorage.CurrentScope[HtmlHelper._validationSummaryValidClassKey] = value != null ? (object) value : throw new ArgumentNullException(nameof (value));
      }
    }

    public static bool UnobtrusiveJavaScriptEnabled
    {
      get => (bool?) ScopeStorage.CurrentScope[HtmlHelper._unobtrusiveValidationKey] ?? true;
      set => ScopeStorage.CurrentScope[HtmlHelper._unobtrusiveValidationKey] = (object) value;
    }

    private ModelStateDictionary ModelState { get; set; }

    public string AttributeEncode(object value)
    {
      return this.AttributeEncode(Convert.ToString(value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public string AttributeEncode(string value)
    {
      return string.IsNullOrEmpty(value) ? string.Empty : HttpUtility.HtmlAttributeEncode(value);
    }

    public string Encode(object value)
    {
      return this.Encode(Convert.ToString(value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public string Encode(string value)
    {
      return string.IsNullOrEmpty(value) ? string.Empty : HttpUtility.HtmlEncode(value);
    }

    public IHtmlString Raw(string value) => (IHtmlString) new HtmlString(value);

    public IHtmlString Raw(object value)
    {
      return (IHtmlString) new HtmlString(value == null ? (string) null : value.ToString());
    }

    public IHtmlString TextBox(string name)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (name));
      object obj = (object) null;
      bool isExplicitValue = false;
      IDictionary<string, object> attributes = (IDictionary<string, object>) null;
      return this.BuildInputField(name, HtmlHelper.InputType.Text, obj, isExplicitValue, attributes);
    }

    public IHtmlString TextBox(string name, object value)
    {
      IDictionary<string, object> htmlAttributes = (IDictionary<string, object>) null;
      return this.TextBox(name, value, htmlAttributes);
    }

    public IHtmlString TextBox(string name, object value, object htmlAttributes)
    {
      return this.TextBox(name, value, TypeHelper.ObjectToDictionary(htmlAttributes));
    }

    public IHtmlString TextBox(
      string name,
      object value,
      IDictionary<string, object> htmlAttributes)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (name));
      bool isExplicitValue = true;
      IDictionary<string, object> attributes = htmlAttributes;
      return this.BuildInputField(name, HtmlHelper.InputType.Text, value, isExplicitValue, attributes);
    }

    public IHtmlString Hidden(string name)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (name));
      object obj = (object) null;
      bool isExplicitValue = false;
      IDictionary<string, object> attributes = (IDictionary<string, object>) null;
      return this.BuildInputField(name, HtmlHelper.InputType.Hidden, obj, isExplicitValue, attributes);
    }

    public IHtmlString Hidden(string name, object value)
    {
      IDictionary<string, object> htmlAttributes = (IDictionary<string, object>) null;
      return this.Hidden(name, value, htmlAttributes);
    }

    public IHtmlString Hidden(string name, object value, object htmlAttributes)
    {
      return this.Hidden(name, value, TypeHelper.ObjectToDictionary(htmlAttributes));
    }

    public IHtmlString Hidden(
      string name,
      object value,
      IDictionary<string, object> htmlAttributes)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (name));
      bool isExplicitValue = true;
      IDictionary<string, object> attributes = htmlAttributes;
      return this.BuildInputField(name, HtmlHelper.InputType.Hidden, HtmlHelper.GetHiddenFieldValue(value), isExplicitValue, attributes);
    }

    private static object GetHiddenFieldValue(object value)
    {
      Binary binary = value as Binary;
      if (binary != (Binary) null)
        value = (object) binary.ToArray();
      if (value is byte[] inArray)
        value = (object) Convert.ToBase64String(inArray);
      return value;
    }

    public IHtmlString Password(string name)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (name));
      bool isExplicitValue = false;
      IDictionary<string, object> attributes = (IDictionary<string, object>) null;
      return this.BuildInputField(name, HtmlHelper.InputType.Password, (object) null, isExplicitValue, attributes);
    }

    public IHtmlString Password(string name, object value)
    {
      IDictionary<string, object> htmlAttributes = (IDictionary<string, object>) null;
      return this.Password(name, value, htmlAttributes);
    }

    public IHtmlString Password(string name, object value, object htmlAttributes)
    {
      return this.Password(name, value, TypeHelper.ObjectToDictionary(htmlAttributes));
    }

    public IHtmlString Password(
      string name,
      object value,
      IDictionary<string, object> htmlAttributes)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (name));
      bool isExplicitValue = true;
      IDictionary<string, object> attributes = htmlAttributes;
      return this.BuildInputField(name, HtmlHelper.InputType.Password, value, isExplicitValue, attributes);
    }

    private IHtmlString BuildInputField(
      string name,
      HtmlHelper.InputType type,
      object value,
      bool isExplicitValue,
      IDictionary<string, object> attributes)
    {
      TagBuilder tagBuilder1 = new TagBuilder("input");
      tagBuilder1.MergeAttribute(nameof (type), HtmlHelper.GetInputTypeString(type));
      tagBuilder1.GenerateId(name);
      TagBuilder tagBuilder2 = tagBuilder1;
      bool flag1 = true;
      IDictionary<string, object> attributes1 = attributes;
      int num1 = flag1 ? 1 : 0;
      tagBuilder2.MergeAttributes<string, object>(attributes1, num1 != 0);
      if (HtmlHelper.UnobtrusiveJavaScriptEnabled)
      {
        IDictionary<string, object> validationAttributes = this._validationHelper.GetUnobtrusiveValidationAttributes(name);
        TagBuilder tagBuilder3 = tagBuilder1;
        bool flag2 = false;
        IDictionary<string, object> attributes2 = validationAttributes;
        int num2 = flag2 ? 1 : 0;
        tagBuilder3.MergeAttributes<string, object>(attributes2, num2 != 0);
      }
      TagBuilder tagBuilder4 = tagBuilder1;
      bool flag3 = true;
      string str1 = name;
      int num3 = flag3 ? 1 : 0;
      tagBuilder4.MergeAttribute(nameof (name), str1, num3 != 0);
      System.Web.WebPages.Html.ModelState modelState = this.ModelState[name];
      if (type != HtmlHelper.InputType.Password && modelState != null)
        value = value ?? modelState.Value ?? (object) string.Empty;
      if (type != HtmlHelper.InputType.Password || type == HtmlHelper.InputType.Password && value != null)
      {
        TagBuilder tagBuilder5 = tagBuilder1;
        bool flag4 = isExplicitValue;
        string str2 = (string) HtmlHelper.ConvertTo(value, typeof (string));
        int num4 = flag4 ? 1 : 0;
        tagBuilder5.MergeAttribute(nameof (value), str2, num4 != 0);
      }
      this.AddErrorClass(tagBuilder1, name);
      return (IHtmlString) tagBuilder1.ToHtmlString(TagRenderMode.SelfClosing);
    }

    private static string GetInputTypeString(HtmlHelper.InputType inputType)
    {
      if (!Enum.IsDefined(typeof (HtmlHelper.InputType), (object) inputType))
        inputType = HtmlHelper.InputType.Text;
      return inputType.ToString().ToLowerInvariant();
    }

    private void AddErrorClass(TagBuilder tagBuilder, string name)
    {
      if (this.ModelState.IsValidField(name))
        return;
      tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
    }

    private static object ConvertTo(object value, Type type)
    {
      return HtmlHelper.UnwrapPossibleArrayType(value, type, CultureInfo.InvariantCulture);
    }

    private static object UnwrapPossibleArrayType(
      object value,
      Type destinationType,
      CultureInfo culture)
    {
      if (value == null || destinationType.IsInstanceOfType(value))
        return value;
      Array array = value as Array;
      if (destinationType.IsArray)
      {
        Type elementType = destinationType.GetElementType();
        if (array != null)
        {
          IList instance = (IList) Array.CreateInstance(elementType, array.Length);
          for (int index = 0; index < array.Length; ++index)
            instance[index] = HtmlHelper.ConvertSimpleType(array.GetValue(index), elementType, culture);
          return (object) instance;
        }
        object obj = HtmlHelper.ConvertSimpleType(value, elementType, culture);
        IList instance1 = (IList) Array.CreateInstance(elementType, 1);
        instance1[0] = obj;
        return (object) instance1;
      }
      if (array == null)
        return HtmlHelper.ConvertSimpleType(value, destinationType, culture);
      if (array.Length <= 0)
        return (object) null;
      value = array.GetValue(0);
      return HtmlHelper.ConvertSimpleType(value, destinationType, culture);
    }

    private static object ConvertSimpleType(
      object value,
      Type destinationType,
      CultureInfo culture)
    {
      if (value == null || destinationType.IsInstanceOfType(value))
        return value;
      if (value is string str && str.Trim().Length == 0)
        return (object) null;
      TypeConverter converter = TypeDescriptor.GetConverter(destinationType);
      bool flag = converter.CanConvertFrom(value.GetType());
      if (!flag)
        converter = TypeDescriptor.GetConverter(value.GetType());
      if (!flag)
      {
        if (!converter.CanConvertTo(destinationType))
          throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, WebPageResources.HtmlHelper_NoConverterExists, new object[2]
          {
            (object) value.GetType().FullName,
            (object) destinationType.FullName
          }));
      }
      try
      {
        object obj1;
        if (!flag)
        {
          TypeConverter typeConverter = converter;
          ITypeDescriptorContext descriptorContext = (ITypeDescriptorContext) null;
          CultureInfo cultureInfo = culture;
          object obj2 = value;
          Type type = destinationType;
          ITypeDescriptorContext context = descriptorContext;
          CultureInfo culture1 = cultureInfo;
          object obj3 = obj2;
          Type destinationType1 = type;
          obj1 = typeConverter.ConvertTo(context, culture1, obj3, destinationType1);
        }
        else
        {
          TypeConverter typeConverter = converter;
          ITypeDescriptorContext descriptorContext = (ITypeDescriptorContext) null;
          CultureInfo cultureInfo = culture;
          object obj4 = value;
          ITypeDescriptorContext context = descriptorContext;
          CultureInfo culture2 = cultureInfo;
          object obj5 = obj4;
          obj1 = typeConverter.ConvertFrom(context, culture2, obj5);
        }
        return obj1;
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentUICulture, WebPageResources.HtmlHelper_ConversionThrew, new object[2]
        {
          (object) value.GetType().FullName,
          (object) destinationType.FullName
        }), ex);
      }
    }

    public IHtmlString Label(string labelText)
    {
      return this.Label(labelText, (string) null, (IDictionary<string, object>) null);
    }

    public IHtmlString Label(string labelText, string labelFor)
    {
      return this.Label(labelText, labelFor, (IDictionary<string, object>) null);
    }

    public IHtmlString Label(string labelText, object attributes)
    {
      return this.Label(labelText, (string) null, TypeHelper.ObjectToDictionary(attributes));
    }

    public IHtmlString Label(string labelText, string labelFor, object attributes)
    {
      return this.Label(labelText, labelFor, TypeHelper.ObjectToDictionary(attributes));
    }

    public IHtmlString Label(
      string labelText,
      string labelFor,
      IDictionary<string, object> attributes)
    {
      if (string.IsNullOrEmpty(labelText))
        throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (labelText));
      labelFor = labelFor ?? labelText;
      TagBuilder tagBuilder = new TagBuilder("label")
      {
        InnerHtml = this.Encode(labelText)
      };
      if (!string.IsNullOrEmpty(labelFor))
        tagBuilder.MergeAttribute("for", labelFor);
      tagBuilder.MergeAttributes<string, object>(attributes, false);
      return (IHtmlString) tagBuilder.ToHtmlString(TagRenderMode.Normal);
    }

    public IHtmlString RadioButton(string name, object value)
    {
      IDictionary<string, object> htmlAttributes = (IDictionary<string, object>) null;
      return this.RadioButton(name, value, htmlAttributes);
    }

    public IHtmlString RadioButton(string name, object value, object htmlAttributes)
    {
      return this.RadioButton(name, value, TypeHelper.ObjectToDictionary(htmlAttributes));
    }

    public IHtmlString RadioButton(
      string name,
      object value,
      IDictionary<string, object> htmlAttributes)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (name));
      bool? isChecked = new bool?();
      IDictionary<string, object> attributes = htmlAttributes;
      return this.BuildRadioButton(name, value, isChecked, attributes);
    }

    public IHtmlString RadioButton(string name, object value, bool isChecked)
    {
      IDictionary<string, object> htmlAttributes = (IDictionary<string, object>) null;
      return this.RadioButton(name, value, isChecked, htmlAttributes);
    }

    public IHtmlString RadioButton(
      string name,
      object value,
      bool isChecked,
      object htmlAttributes)
    {
      return this.RadioButton(name, value, isChecked, TypeHelper.ObjectToDictionary(htmlAttributes));
    }

    public IHtmlString RadioButton(
      string name,
      object value,
      bool isChecked,
      IDictionary<string, object> htmlAttributes)
    {
      if (name == null)
        throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (name));
      return this.BuildRadioButton(name, value, new bool?(isChecked), htmlAttributes);
    }

    private IHtmlString BuildRadioButton(
      string name,
      object value,
      bool? isChecked,
      IDictionary<string, object> attributes)
    {
      string b = HtmlHelper.ConvertTo(value, typeof (string)) as string;
      TagBuilder tagBuilder1 = new TagBuilder("input");
      tagBuilder1.MergeAttribute("type", "radio", true);
      tagBuilder1.GenerateId(name);
      TagBuilder tagBuilder2 = tagBuilder1;
      bool flag1 = true;
      IDictionary<string, object> attributes1 = attributes;
      int num1 = flag1 ? 1 : 0;
      tagBuilder2.MergeAttributes<string, object>(attributes1, num1 != 0);
      TagBuilder tagBuilder3 = tagBuilder1;
      bool flag2 = true;
      string str1 = b;
      int num2 = flag2 ? 1 : 0;
      tagBuilder3.MergeAttribute(nameof (value), str1, num2 != 0);
      TagBuilder tagBuilder4 = tagBuilder1;
      bool flag3 = true;
      string str2 = name;
      int num3 = flag3 ? 1 : 0;
      tagBuilder4.MergeAttribute(nameof (name), str2, num3 != 0);
      if (HtmlHelper.UnobtrusiveJavaScriptEnabled)
      {
        IDictionary<string, object> validationAttributes = this._validationHelper.GetUnobtrusiveValidationAttributes(name);
        TagBuilder tagBuilder5 = tagBuilder1;
        bool flag4 = false;
        IDictionary<string, object> attributes2 = validationAttributes;
        int num4 = flag4 ? 1 : 0;
        tagBuilder5.MergeAttributes<string, object>(attributes2, num4 != 0);
      }
      System.Web.WebPages.Html.ModelState modelState = this.ModelState[name];
      if (modelState != null)
      {
        string a = HtmlHelper.ConvertTo(modelState.Value, typeof (string)) as string;
        isChecked = new bool?(((int) isChecked ?? (string.Equals(a, b, StringComparison.OrdinalIgnoreCase) ? 1 : 0)) != 0);
      }
      if (isChecked.HasValue)
      {
        if (isChecked.Value)
          tagBuilder1.MergeAttribute("checked", "checked", true);
        else
          tagBuilder1.Attributes.Remove("checked");
      }
      this.AddErrorClass(tagBuilder1, name);
      return (IHtmlString) tagBuilder1.ToHtmlString(TagRenderMode.SelfClosing);
    }

    public IHtmlString ListBox(string name, IEnumerable<SelectListItem> selectList)
    {
      string defaultOption = (string) null;
      IEnumerable<SelectListItem> selectList1 = selectList;
      IDictionary<string, object> htmlAttributes = (IDictionary<string, object>) null;
      return this.ListBox(name, defaultOption, selectList1, htmlAttributes);
    }

    public IHtmlString ListBox(
      string name,
      string defaultOption,
      IEnumerable<SelectListItem> selectList)
    {
      string defaultOption1 = defaultOption;
      IEnumerable<SelectListItem> selectList1 = selectList;
      object selectedValues = (object) null;
      IDictionary<string, object> htmlAttributes = (IDictionary<string, object>) null;
      return this.ListBox(name, defaultOption1, selectList1, selectedValues, htmlAttributes);
    }

    public IHtmlString ListBox(
      string name,
      IEnumerable<SelectListItem> selectList,
      object htmlAttributes)
    {
      string defaultOption = (string) null;
      IEnumerable<SelectListItem> selectList1 = selectList;
      object selectedValues = (object) null;
      object htmlAttributes1 = htmlAttributes;
      return this.ListBox(name, defaultOption, selectList1, selectedValues, htmlAttributes1);
    }

    public IHtmlString ListBox(
      string name,
      IEnumerable<SelectListItem> selectList,
      IDictionary<string, object> htmlAttributes)
    {
      string defaultOption = (string) null;
      IEnumerable<SelectListItem> selectList1 = selectList;
      object selectedValues = (object) null;
      IDictionary<string, object> htmlAttributes1 = htmlAttributes;
      return this.ListBox(name, defaultOption, selectList1, selectedValues, htmlAttributes1);
    }

    public IHtmlString ListBox(
      string name,
      string defaultOption,
      IEnumerable<SelectListItem> selectList,
      IDictionary<string, object> htmlAttributes)
    {
      object selectedValues = (object) null;
      IDictionary<string, object> htmlAttributes1 = htmlAttributes;
      return this.ListBox(name, defaultOption, selectList, selectedValues, htmlAttributes1);
    }

    public IHtmlString ListBox(
      string name,
      string defaultOption,
      IEnumerable<SelectListItem> selectList,
      object htmlAttributes)
    {
      string defaultOption1 = defaultOption;
      IEnumerable<SelectListItem> selectList1 = selectList;
      object selectedValues = (object) null;
      object htmlAttributes1 = htmlAttributes;
      return this.ListBox(name, defaultOption1, selectList1, selectedValues, htmlAttributes1);
    }

    public IHtmlString ListBox(
      string name,
      string defaultOption,
      IEnumerable<SelectListItem> selectList,
      object selectedValues,
      IDictionary<string, object> htmlAttributes)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (name));
      string defaultOption1 = defaultOption;
      IEnumerable<SelectListItem> selectList1 = selectList;
      object selectedValues1 = selectedValues;
      int? size = new int?();
      bool allowMultiple = false;
      IDictionary<string, object> htmlAttributes1 = htmlAttributes;
      return this.BuildListBox(name, defaultOption1, selectList1, selectedValues1, size, allowMultiple, htmlAttributes1);
    }

    public IHtmlString ListBox(
      string name,
      string defaultOption,
      IEnumerable<SelectListItem> selectList,
      object selectedValues,
      object htmlAttributes)
    {
      string defaultOption1 = defaultOption;
      IEnumerable<SelectListItem> selectList1 = selectList;
      object selectedValues1 = selectedValues;
      IDictionary<string, object> dictionary = TypeHelper.ObjectToDictionary(htmlAttributes);
      return this.ListBox(name, defaultOption1, selectList1, selectedValues1, dictionary);
    }

    public IHtmlString ListBox(
      string name,
      IEnumerable<SelectListItem> selectList,
      object selectedValues,
      int size,
      bool allowMultiple)
    {
      string defaultOption = (string) null;
      IEnumerable<SelectListItem> selectList1 = selectList;
      object selectedValues1 = selectedValues;
      int size1 = size;
      bool allowMultiple1 = allowMultiple;
      IDictionary<string, object> htmlAttributes = (IDictionary<string, object>) null;
      return this.ListBox(name, defaultOption, selectList1, selectedValues1, size1, allowMultiple1, htmlAttributes);
    }

    public IHtmlString ListBox(
      string name,
      string defaultOption,
      IEnumerable<SelectListItem> selectList,
      object selectedValues,
      int size,
      bool allowMultiple)
    {
      string defaultOption1 = defaultOption;
      IEnumerable<SelectListItem> selectList1 = selectList;
      object selectedValues1 = selectedValues;
      int size1 = size;
      bool allowMultiple1 = allowMultiple;
      IDictionary<string, object> htmlAttributes = (IDictionary<string, object>) null;
      return this.ListBox(name, defaultOption1, selectList1, selectedValues1, size1, allowMultiple1, htmlAttributes);
    }

    public IHtmlString ListBox(
      string name,
      string defaultOption,
      IEnumerable<SelectListItem> selectList,
      object selectedValues,
      int size,
      bool allowMultiple,
      IDictionary<string, object> htmlAttributes)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (name));
      return this.BuildListBox(name, defaultOption, selectList, selectedValues, new int?(size), allowMultiple, htmlAttributes);
    }

    public IHtmlString ListBox(
      string name,
      string defaultOption,
      IEnumerable<SelectListItem> selectList,
      object selectedValues,
      int size,
      bool allowMultiple,
      object htmlAttributes)
    {
      return this.ListBox(name, defaultOption, selectList, selectedValues, size, allowMultiple, TypeHelper.ObjectToDictionary(htmlAttributes));
    }

    private IHtmlString BuildListBox(
      string name,
      string defaultOption,
      IEnumerable<SelectListItem> selectList,
      object selectedValues,
      int? size,
      bool allowMultiple,
      IDictionary<string, object> htmlAttributes)
    {
      if (this.ModelState[name] != null)
        selectedValues = selectedValues ?? this.ModelState[name].Value;
      if (selectedValues != null)
      {
        object[] source;
        if (!allowMultiple)
          source = new object[1]
          {
            HtmlHelper.ConvertTo(selectedValues, typeof (string))
          };
        else
          source = (object[]) (HtmlHelper.ConvertTo(selectedValues, typeof (string[])) as string[]);
        HashSet<string> stringSet = new HashSet<string>(source.Cast<object>().Select<object, string>((Func<object, string>) (value => Convert.ToString(value, (IFormatProvider) CultureInfo.CurrentCulture))), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
        List<SelectListItem> selectListItemList = new List<SelectListItem>();
        bool flag1 = false;
        foreach (SelectListItem select in selectList)
        {
          bool flag2 = false;
          if (allowMultiple || !flag1)
            flag2 = select.Selected || stringSet.Contains(select.Value ?? select.Text);
          flag1 |= flag2;
          selectListItemList.Add(new SelectListItem(select)
          {
            Selected = flag2
          });
        }
        selectList = (IEnumerable<SelectListItem>) selectListItemList;
      }
      TagBuilder tagBuilder1 = new TagBuilder("select")
      {
        InnerHtml = HtmlHelper.BuildListOptions(selectList, defaultOption)
      };
      if (HtmlHelper.UnobtrusiveJavaScriptEnabled)
      {
        IDictionary<string, object> validationAttributes = this._validationHelper.GetUnobtrusiveValidationAttributes(name);
        TagBuilder tagBuilder2 = tagBuilder1;
        bool flag = false;
        IDictionary<string, object> attributes = validationAttributes;
        int num = flag ? 1 : 0;
        tagBuilder2.MergeAttributes<string, object>(attributes, num != 0);
      }
      tagBuilder1.GenerateId(name);
      tagBuilder1.MergeAttributes<string, object>(htmlAttributes);
      TagBuilder tagBuilder3 = tagBuilder1;
      bool flag3 = true;
      string str = name;
      int num1 = flag3 ? 1 : 0;
      tagBuilder3.MergeAttribute(nameof (name), str, num1 != 0);
      if (size.HasValue)
        tagBuilder1.MergeAttribute(nameof (size), size.ToString(), true);
      if (allowMultiple)
        tagBuilder1.MergeAttribute("multiple", "multiple");
      else if (tagBuilder1.Attributes.ContainsKey("multiple"))
        tagBuilder1.Attributes.Remove("multiple");
      this.AddErrorClass(tagBuilder1, name);
      return (IHtmlString) tagBuilder1.ToHtmlString(TagRenderMode.Normal);
    }

    public IHtmlString DropDownList(string name, IEnumerable<SelectListItem> selectList)
    {
      string defaultOption = (string) null;
      IEnumerable<SelectListItem> selectList1 = selectList;
      IDictionary<string, object> htmlAttributes = (IDictionary<string, object>) null;
      return this.DropDownList(name, defaultOption, selectList1, htmlAttributes);
    }

    public IHtmlString DropDownList(
      string name,
      IEnumerable<SelectListItem> selectList,
      object htmlAttributes)
    {
      string defaultOption = (string) null;
      IEnumerable<SelectListItem> selectList1 = selectList;
      object selectedValue = (object) null;
      object htmlAttributes1 = htmlAttributes;
      return this.DropDownList(name, defaultOption, selectList1, selectedValue, htmlAttributes1);
    }

    public IHtmlString DropDownList(
      string name,
      IEnumerable<SelectListItem> selectList,
      IDictionary<string, object> htmlAttributes)
    {
      string defaultOption = (string) null;
      IEnumerable<SelectListItem> selectList1 = selectList;
      object selectedValue = (object) null;
      IDictionary<string, object> htmlAttributes1 = htmlAttributes;
      return this.DropDownList(name, defaultOption, selectList1, selectedValue, htmlAttributes1);
    }

    public IHtmlString DropDownList(
      string name,
      string defaultOption,
      IEnumerable<SelectListItem> selectList)
    {
      object selectedValue = (object) null;
      IDictionary<string, object> htmlAttributes = (IDictionary<string, object>) null;
      return this.DropDownList(name, defaultOption, selectList, selectedValue, htmlAttributes);
    }

    public IHtmlString DropDownList(
      string name,
      string defaultOption,
      IEnumerable<SelectListItem> selectList,
      IDictionary<string, object> htmlAttributes)
    {
      object selectedValue = (object) null;
      IDictionary<string, object> htmlAttributes1 = htmlAttributes;
      return this.DropDownList(name, defaultOption, selectList, selectedValue, htmlAttributes1);
    }

    public IHtmlString DropDownList(
      string name,
      string defaultOption,
      IEnumerable<SelectListItem> selectList,
      object htmlAttributes)
    {
      string defaultOption1 = defaultOption;
      IEnumerable<SelectListItem> selectList1 = selectList;
      object selectedValue = (object) null;
      object htmlAttributes1 = htmlAttributes;
      return this.DropDownList(name, defaultOption1, selectList1, selectedValue, htmlAttributes1);
    }

    public IHtmlString DropDownList(
      string name,
      string defaultOption,
      IEnumerable<SelectListItem> selectList,
      object selectedValue,
      IDictionary<string, object> htmlAttributes)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (name));
      IDictionary<string, object> htmlAttributes1 = htmlAttributes;
      return this.BuildDropDownList(name, defaultOption, selectList, selectedValue, htmlAttributes1);
    }

    public IHtmlString DropDownList(
      string name,
      string defaultOption,
      IEnumerable<SelectListItem> selectList,
      object selectedValue,
      object htmlAttributes)
    {
      return this.DropDownList(name, defaultOption, selectList, selectedValue, TypeHelper.ObjectToDictionary(htmlAttributes));
    }

    private IHtmlString BuildDropDownList(
      string name,
      string defaultOption,
      IEnumerable<SelectListItem> selectList,
      object selectedValue,
      IDictionary<string, object> htmlAttributes)
    {
      if (this.ModelState[name] != null)
        selectedValue = selectedValue ?? this.ModelState[name].Value;
      selectedValue = HtmlHelper.ConvertTo(selectedValue, typeof (string));
      if (selectedValue != null)
      {
        List<SelectListItem> source = new List<SelectListItem>(selectList.Select<SelectListItem, SelectListItem>((Func<SelectListItem, SelectListItem>) (item => new SelectListItem(item))));
        StringComparer comparer = StringComparer.InvariantCultureIgnoreCase;
        SelectListItem selectListItem = source.FirstOrDefault<SelectListItem>((Func<SelectListItem, bool>) (item => item.Selected || comparer.Equals((object) (item.Value ?? item.Text), selectedValue)));
        if (selectListItem != null)
        {
          selectListItem.Selected = true;
          selectList = (IEnumerable<SelectListItem>) source;
        }
      }
      TagBuilder tagBuilder1 = new TagBuilder("select")
      {
        InnerHtml = HtmlHelper.BuildListOptions(selectList, defaultOption)
      };
      tagBuilder1.MergeAttributes<string, object>(htmlAttributes);
      TagBuilder tagBuilder2 = tagBuilder1;
      bool flag1 = true;
      string str = name;
      int num1 = flag1 ? 1 : 0;
      tagBuilder2.MergeAttribute(nameof (name), str, num1 != 0);
      tagBuilder1.GenerateId(name);
      if (HtmlHelper.UnobtrusiveJavaScriptEnabled)
      {
        IDictionary<string, object> validationAttributes = this._validationHelper.GetUnobtrusiveValidationAttributes(name);
        TagBuilder tagBuilder3 = tagBuilder1;
        bool flag2 = false;
        IDictionary<string, object> attributes = validationAttributes;
        int num2 = flag2 ? 1 : 0;
        tagBuilder3.MergeAttributes<string, object>(attributes, num2 != 0);
      }
      this.AddErrorClass(tagBuilder1, name);
      return (IHtmlString) tagBuilder1.ToHtmlString(TagRenderMode.Normal);
    }

    private static string BuildListOptions(
      IEnumerable<SelectListItem> selectList,
      string optionText)
    {
      StringBuilder stringBuilder = new StringBuilder().AppendLine();
      if (optionText != null)
        stringBuilder.AppendLine(HtmlHelper.ListItemToOption(new SelectListItem()
        {
          Text = optionText,
          Value = string.Empty
        }));
      if (selectList != null)
      {
        foreach (SelectListItem select in selectList)
          stringBuilder.AppendLine(HtmlHelper.ListItemToOption(select));
      }
      return stringBuilder.ToString();
    }

    private static string ListItemToOption(SelectListItem item)
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

    private static IDictionary<string, object> GetRowsAndColumnsDictionary(int rows, int columns)
    {
      Dictionary<string, object> columnsDictionary = new Dictionary<string, object>();
      if (rows > 0)
        columnsDictionary.Add(nameof (rows), (object) rows.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      if (columns > 0)
        columnsDictionary.Add("cols", (object) columns.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      return (IDictionary<string, object>) columnsDictionary;
    }

    public IHtmlString TextArea(string name)
    {
      string str = (string) null;
      IDictionary<string, object> htmlAttributes = (IDictionary<string, object>) null;
      return this.TextArea(name, str, htmlAttributes);
    }

    public IHtmlString TextArea(string name, object htmlAttributes)
    {
      string str = (string) null;
      IDictionary<string, object> dictionary = TypeHelper.ObjectToDictionary(htmlAttributes);
      return this.TextArea(name, str, dictionary);
    }

    public IHtmlString TextArea(string name, IDictionary<string, object> htmlAttributes)
    {
      string str = (string) null;
      IDictionary<string, object> htmlAttributes1 = htmlAttributes;
      return this.TextArea(name, str, htmlAttributes1);
    }

    public IHtmlString TextArea(string name, string value)
    {
      return this.TextArea(name, value, (IDictionary<string, object>) null);
    }

    public IHtmlString TextArea(string name, string value, object htmlAttributes)
    {
      return this.TextArea(name, value, TypeHelper.ObjectToDictionary(htmlAttributes));
    }

    public IHtmlString TextArea(
      string name,
      string value,
      IDictionary<string, object> htmlAttributes)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (name));
      return this.BuildTextArea(name, value, HtmlHelper._implicitRowsAndColumns, htmlAttributes);
    }

    public IHtmlString TextArea(
      string name,
      string value,
      int rows,
      int columns,
      object htmlAttributes)
    {
      return this.TextArea(name, value, rows, columns, TypeHelper.ObjectToDictionary(htmlAttributes));
    }

    public IHtmlString TextArea(
      string name,
      string value,
      int rows,
      int columns,
      IDictionary<string, object> htmlAttributes)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (name));
      return this.BuildTextArea(name, value, HtmlHelper.GetRowsAndColumnsDictionary(rows, columns), htmlAttributes);
    }

    private IHtmlString BuildTextArea(
      string name,
      string value,
      IDictionary<string, object> rowsAndColumnsDictionary,
      IDictionary<string, object> htmlAttributes)
    {
      TagBuilder tagBuilder1 = new TagBuilder("textarea");
      if (HtmlHelper.UnobtrusiveJavaScriptEnabled)
      {
        IDictionary<string, object> validationAttributes = this._validationHelper.GetUnobtrusiveValidationAttributes(name);
        TagBuilder tagBuilder2 = tagBuilder1;
        bool flag = false;
        IDictionary<string, object> attributes = validationAttributes;
        int num = flag ? 1 : 0;
        tagBuilder2.MergeAttributes<string, object>(attributes, num != 0);
      }
      tagBuilder1.MergeAttributes<string, object>(htmlAttributes);
      tagBuilder1.MergeAttributes<string, object>(rowsAndColumnsDictionary, rowsAndColumnsDictionary != HtmlHelper._implicitRowsAndColumns);
      if (this.ModelState[name] != null)
        value = value ?? Convert.ToString(this.ModelState[name].Value, (IFormatProvider) CultureInfo.CurrentCulture);
      tagBuilder1.InnerHtml = this.Encode(value);
      tagBuilder1.MergeAttribute(nameof (name), name);
      tagBuilder1.GenerateId(name);
      this.AddErrorClass(tagBuilder1, name);
      return (IHtmlString) tagBuilder1.ToHtmlString(TagRenderMode.Normal);
    }

    public IHtmlString ValidationMessage(string name)
    {
      return this.ValidationMessage(name, (string) null, (IDictionary<string, object>) null);
    }

    public IHtmlString ValidationMessage(string name, string message)
    {
      return this.ValidationMessage(name, message, (IDictionary<string, object>) null);
    }

    public IHtmlString ValidationMessage(string name, object htmlAttributes)
    {
      return this.ValidationMessage(name, (string) null, TypeHelper.ObjectToDictionary(htmlAttributes));
    }

    public IHtmlString ValidationMessage(string name, IDictionary<string, object> htmlAttributes)
    {
      return this.ValidationMessage(name, (string) null, htmlAttributes);
    }

    public IHtmlString ValidationMessage(string name, string message, object htmlAttributes)
    {
      return this.ValidationMessage(name, message, TypeHelper.ObjectToDictionary(htmlAttributes));
    }

    public IHtmlString ValidationMessage(
      string name,
      string message,
      IDictionary<string, object> htmlAttributes)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (name));
      return this.BuildValidationMessage(name, message, htmlAttributes);
    }

    private IHtmlString BuildValidationMessage(
      string name,
      string message,
      IDictionary<string, object> htmlAttributes)
    {
      System.Web.WebPages.Html.ModelState modelState = this.ModelState[name];
      IEnumerable<string> source = (IEnumerable<string>) null;
      if (modelState != null)
        source = (IEnumerable<string>) modelState.Errors;
      bool flag1 = source != null && source.Any<string>();
      if (!flag1 && !HtmlHelper.UnobtrusiveJavaScriptEnabled)
        return (IHtmlString) null;
      string str = (string) null;
      if (flag1)
        str = message ?? source.First<string>();
      TagBuilder tagBuilder = new TagBuilder("span")
      {
        InnerHtml = this.Encode(str)
      };
      tagBuilder.MergeAttributes<string, object>(htmlAttributes);
      if (HtmlHelper.UnobtrusiveJavaScriptEnabled)
      {
        bool flag2 = string.IsNullOrEmpty(message);
        tagBuilder.MergeAttribute("data-valmsg-for", name);
        tagBuilder.MergeAttribute("data-valmsg-replace", flag2.ToString().ToLowerInvariant());
      }
      tagBuilder.AddCssClass(flag1 ? HtmlHelper.ValidationMessageCssClassName : HtmlHelper.ValidationMessageValidCssClassName);
      return (IHtmlString) tagBuilder.ToHtmlString(TagRenderMode.Normal);
    }

    public IHtmlString ValidationSummary()
    {
      return this.BuildValidationSummary((string) null, false, (IDictionary<string, object>) null);
    }

    public IHtmlString ValidationSummary(string message)
    {
      return this.BuildValidationSummary(message, false, (IDictionary<string, object>) null);
    }

    public IHtmlString ValidationSummary(bool excludeFieldErrors)
    {
      return this.ValidationSummary((string) null, excludeFieldErrors, (IDictionary<string, object>) null);
    }

    public IHtmlString ValidationSummary(object htmlAttributes)
    {
      return this.ValidationSummary((string) null, false, htmlAttributes);
    }

    public IHtmlString ValidationSummary(IDictionary<string, object> htmlAttributes)
    {
      return this.ValidationSummary((string) null, false, htmlAttributes);
    }

    public IHtmlString ValidationSummary(string message, object htmlAttributes)
    {
      bool excludeFieldErrors = false;
      object htmlAttributes1 = htmlAttributes;
      return this.ValidationSummary(message, excludeFieldErrors, htmlAttributes1);
    }

    public IHtmlString ValidationSummary(string message, IDictionary<string, object> htmlAttributes)
    {
      bool excludeFieldErrors = false;
      IDictionary<string, object> htmlAttributes1 = htmlAttributes;
      return this.ValidationSummary(message, excludeFieldErrors, htmlAttributes1);
    }

    public IHtmlString ValidationSummary(
      string message,
      bool excludeFieldErrors,
      object htmlAttributes)
    {
      return this.ValidationSummary(message, excludeFieldErrors, TypeHelper.ObjectToDictionary(htmlAttributes));
    }

    public IHtmlString ValidationSummary(
      string message,
      bool excludeFieldErrors,
      IDictionary<string, object> htmlAttributes)
    {
      return this.BuildValidationSummary(message, excludeFieldErrors, htmlAttributes);
    }

    private IHtmlString BuildValidationSummary(
      string message,
      bool excludeFieldErrors,
      IDictionary<string, object> htmlAttributes)
    {
      IEnumerable<string> source = (IEnumerable<string>) null;
      if (excludeFieldErrors)
      {
        System.Web.WebPages.Html.ModelState modelState = this.ModelState["_FORM"];
        if (modelState != null)
          source = (IEnumerable<string>) modelState.Errors;
      }
      else
        source = this.ModelState.SelectMany<KeyValuePair<string, System.Web.WebPages.Html.ModelState>, string>((Func<KeyValuePair<string, System.Web.WebPages.Html.ModelState>, IEnumerable<string>>) (c => (IEnumerable<string>) c.Value.Errors));
      bool flag = source != null && source.Any<string>();
      if (!flag && (!HtmlHelper.UnobtrusiveJavaScriptEnabled || excludeFieldErrors))
        return (IHtmlString) null;
      TagBuilder tagBuilder = new TagBuilder("div");
      tagBuilder.MergeAttributes<string, object>(htmlAttributes);
      tagBuilder.AddCssClass(flag ? HtmlHelper.ValidationSummaryClass : HtmlHelper.ValidationSummaryValidClass);
      if (HtmlHelper.UnobtrusiveJavaScriptEnabled && !excludeFieldErrors)
        tagBuilder.MergeAttribute("data-valmsg-summary", "true");
      StringBuilder stringBuilder = new StringBuilder();
      if (message != null)
      {
        stringBuilder.Append("<span>");
        stringBuilder.Append(this.Encode(message));
        stringBuilder.AppendLine("</span>");
      }
      stringBuilder.AppendLine("<ul>");
      foreach (string str in source)
      {
        stringBuilder.Append("<li>");
        stringBuilder.Append(this.Encode(str));
        stringBuilder.AppendLine("</li>");
      }
      stringBuilder.Append("</ul>");
      tagBuilder.InnerHtml = stringBuilder.ToString();
      return (IHtmlString) tagBuilder.ToHtmlString(TagRenderMode.Normal);
    }

    private enum InputType
    {
      Text,
      Password,
      Hidden,
    }
  }
}
