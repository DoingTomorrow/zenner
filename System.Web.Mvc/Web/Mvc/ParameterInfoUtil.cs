// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ParameterInfoUtil
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.ComponentModel;
using System.Reflection;

#nullable disable
namespace System.Web.Mvc
{
  internal static class ParameterInfoUtil
  {
    public static bool TryGetDefaultValue(ParameterInfo parameterInfo, out object value)
    {
      object defaultValue = parameterInfo.DefaultValue;
      if (defaultValue != DBNull.Value)
      {
        value = defaultValue;
        return true;
      }
      DefaultValueAttribute[] customAttributes = (DefaultValueAttribute[]) parameterInfo.GetCustomAttributes(typeof (DefaultValueAttribute), false);
      if (customAttributes == null || customAttributes.Length == 0)
      {
        value = (object) null;
        return false;
      }
      value = customAttributes[0].Value;
      return true;
    }
  }
}
