// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.UnobtrusiveValidationAttributesGenerator
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.WebPages.Resources;

#nullable disable
namespace System.Web.Mvc
{
  public static class UnobtrusiveValidationAttributesGenerator
  {
    public static void GetValidationAttributes(
      IEnumerable<ModelClientValidationRule> clientRules,
      IDictionary<string, object> results)
    {
      if (clientRules == null)
        throw new ArgumentNullException(nameof (clientRules));
      if (results == null)
        throw new ArgumentNullException(nameof (results));
      bool flag = false;
      foreach (ModelClientValidationRule clientRule in clientRules)
      {
        flag = true;
        string str1 = "data-val-" + clientRule.ValidationType;
        UnobtrusiveValidationAttributesGenerator.ValidateUnobtrusiveValidationRule(clientRule, results, str1);
        results.Add(str1, (object) (clientRule.ErrorMessage ?? string.Empty));
        string str2 = str1 + "-";
        foreach (KeyValuePair<string, object> validationParameter in (IEnumerable<KeyValuePair<string, object>>) clientRule.ValidationParameters)
          results.Add(str2 + validationParameter.Key, validationParameter.Value ?? (object) string.Empty);
      }
      if (!flag)
        return;
      results.Add("data-val", (object) "true");
    }

    private static void ValidateUnobtrusiveValidationRule(
      ModelClientValidationRule rule,
      IDictionary<string, object> resultsDictionary,
      string dictionaryKey)
    {
      if (string.IsNullOrWhiteSpace(rule.ValidationType))
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, WebPageResources.UnobtrusiveJavascript_ValidationTypeCannotBeEmpty, new object[1]
        {
          (object) rule.GetType().FullName
        }));
      if (resultsDictionary.ContainsKey(dictionaryKey))
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, WebPageResources.UnobtrusiveJavascript_ValidationTypeMustBeUnique, new object[1]
        {
          (object) rule.ValidationType
        }));
      if (rule.ValidationType.Any<char>((Func<char, bool>) (c => !char.IsLower(c))))
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, WebPageResources.UnobtrusiveJavascript_ValidationTypeMustBeLegal, new object[2]
        {
          (object) rule.ValidationType,
          (object) rule.GetType().FullName
        }));
      foreach (string key in (IEnumerable<string>) rule.ValidationParameters.Keys)
      {
        if (string.IsNullOrWhiteSpace(key))
          throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, WebPageResources.UnobtrusiveJavascript_ValidationParameterCannotBeEmpty, new object[1]
          {
            (object) rule.GetType().FullName
          }));
        if (!char.IsLower(key.First<char>()) || key.Any<char>((Func<char, bool>) (c => !char.IsLower(c) && !char.IsDigit(c))))
          throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, WebPageResources.UnobtrusiveJavascript_ValidationParameterMustBeLegal, new object[2]
          {
            (object) key,
            (object) rule.GetType().FullName
          }));
      }
    }
  }
}
