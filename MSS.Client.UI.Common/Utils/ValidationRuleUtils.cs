// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Common.Utils.ValidationRuleUtils
// Assembly: MSS.Client.UI.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15ED3F62-7ABB-4067-AE48-CE636F8F9754
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Common.dll

using MSS.Business.Modules.Configuration;
using MSS.Client.UI.Common.ValidationRules;
using MSS.Core.Model.ApplicationParamenters;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS.Client.UI.Common.Utils
{
  public class ValidationRuleUtils
  {
    public bool AreChangeableParametersValid(List<Config> changeableParameters)
    {
      if (changeableParameters != null)
      {
        RequiredValidationRule requiredValidationRule = new RequiredValidationRule();
        RangeValidationRuleGenerator validationRuleGenerator = new RangeValidationRuleGenerator();
        foreach (Config changeableParameter in changeableParameters)
        {
          if (!requiredValidationRule.Validate((object) changeableParameter.PropertyValue, CultureInfo.CurrentCulture).IsValid)
            return false;
          string type1 = changeableParameter.Type;
          ViewObjectTypeEnum viewObjectTypeEnum = ViewObjectTypeEnum.TextBox;
          string str1 = viewObjectTypeEnum.ToString();
          int num;
          if (!(type1 == str1))
          {
            string type2 = changeableParameter.Type;
            viewObjectTypeEnum = ViewObjectTypeEnum.Numeric;
            string str2 = viewObjectTypeEnum.ToString();
            num = type2 == str2 ? 1 : 0;
          }
          else
            num = 1;
          if (num != 0 && changeableParameter.Parameter is ChangeableParameter parameter)
          {
            ValidationRule rangeValidationRule = validationRuleGenerator.GetRangeValidationRule(parameter.Type.Name, parameter.ValueMin, parameter.ValueMax);
            if (rangeValidationRule != null && !rangeValidationRule.Validate((object) changeableParameter.PropertyValue, CultureInfo.CurrentCulture).IsValid)
              return false;
          }
        }
      }
      return true;
    }
  }
}
