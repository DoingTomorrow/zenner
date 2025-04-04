// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Common.ValidationRules.RequiredValidationRule
// Assembly: MSS.Client.UI.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15ED3F62-7ABB-4067-AE48-CE636F8F9754
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Common.dll

using MSS.Localisation;
using System.Globalization;
using System.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Common.ValidationRules
{
  public class RequiredValidationRule : ValidationRule
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
      return string.IsNullOrWhiteSpace((string) value) ? new ValidationResult(false, (object) Resources.Validation_Required) : new ValidationResult(true, (object) null);
    }
  }
}
