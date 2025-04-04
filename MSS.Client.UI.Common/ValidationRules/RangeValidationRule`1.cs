// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Common.ValidationRules.RangeValidationRule`1
// Assembly: MSS.Client.UI.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15ED3F62-7ABB-4067-AE48-CE636F8F9754
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Common.dll

using MSS.Utils.Utils;
using System;
using System.Globalization;
using System.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Common.ValidationRules
{
  public class RangeValidationRule<T> : ValidationRule where T : struct, IComparable<T>
  {
    public T Min { get; set; }

    public T Max { get; set; }

    public Type TypeOfValue { get; set; }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
      GenericRangeValidator<T> genericRangeValidator = new GenericRangeValidator<T>(this.Min, this.Max);
      try
      {
        T obj = (T) Convert.ChangeType(value, this.TypeOfValue);
        bool flag = genericRangeValidator.Contains(obj);
        int num = flag ? 1 : 0;
        string errorContent;
        if (!flag)
          errorContent = "The value must be greater than or equal to " + (object) this.Min + " and less than or equal to " + (object) this.Max;
        else
          errorContent = string.Empty;
        return new ValidationResult(num != 0, (object) errorContent);
      }
      catch
      {
        return new ValidationResult(false, (object) "Invalid value");
      }
    }
  }
}
