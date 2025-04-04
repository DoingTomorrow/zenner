// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Common.Utils.RangeValidationRuleGenerator
// Assembly: MSS.Client.UI.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15ED3F62-7ABB-4067-AE48-CE636F8F9754
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Common.dll

using MSS.Client.UI.Common.ValidationRules;
using System;
using System.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Common.Utils
{
  public class RangeValidationRuleGenerator
  {
    public ValidationRule GetRangeValidationRule(string parameterType, object min, object max)
    {
      switch (parameterType)
      {
        case "Byte":
          return (ValidationRule) this.GetRangeValidationRuleType<byte>(min, max, (byte) 0, byte.MaxValue, typeof (byte));
        case "Int32":
          return (ValidationRule) this.GetRangeValidationRuleType<int>(min, max, int.MinValue, int.MaxValue, typeof (int));
        case "UInt32":
          return (ValidationRule) this.GetRangeValidationRuleType<uint>(min, max, 0U, uint.MaxValue, typeof (uint));
        case "UInt64":
          return (ValidationRule) this.GetRangeValidationRuleType<ulong>(min, max, 0UL, ulong.MaxValue, typeof (ulong));
        case "TimeSpan":
          return (ValidationRule) this.GetRangeValidationRuleType<TimeSpan>(min, max, TimeSpan.MinValue, TimeSpan.MaxValue, typeof (TimeSpan));
        default:
          return (ValidationRule) null;
      }
    }

    private RangeValidationRule<T> GetRangeValidationRuleType<T>(
      object parameterMin,
      object parameterMax,
      T typeMin,
      T typeMax,
      Type type)
      where T : struct, IComparable<T>
    {
      T obj1;
      T obj2;
      try
      {
        T obj3 = (T) Convert.ChangeType(parameterMin, type);
        T obj4 = (T) Convert.ChangeType(parameterMax, type);
        obj1 = obj3;
        obj2 = obj4;
      }
      catch
      {
        obj1 = typeMin;
        obj2 = typeMax;
      }
      RangeValidationRule<T> validationRuleType = new RangeValidationRule<T>();
      validationRuleType.Min = obj1;
      validationRuleType.Max = obj2;
      validationRuleType.TypeOfValue = type;
      validationRuleType.ValidatesOnTargetUpdated = true;
      validationRuleType.ValidationStep = ValidationStep.ConvertedProposedValue;
      return validationRuleType;
    }
  }
}
