// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Utils.ConvertUtil
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace OfficeOpenXml.Utils
{
  internal static class ConvertUtil
  {
    internal static bool IsNumeric(object candidate)
    {
      if (candidate == null)
        return false;
      if (!candidate.GetType().IsPrimitive)
      {
        switch (candidate)
        {
          case double _:
          case Decimal _:
          case DateTime _:
          case TimeSpan _:
            break;
          default:
            return candidate is long;
        }
      }
      return true;
    }

    internal static bool IsNumericString(object candidate)
    {
      return candidate != null && Regex.IsMatch(candidate.ToString(), "^[\\d]+(\\,[\\d])?");
    }

    internal static double GetValueDouble(object v, bool ignoreBool = false)
    {
      double valueDouble;
      try
      {
        if (ignoreBool && v is bool)
          return 0.0;
        if (ConvertUtil.IsNumeric(v))
        {
          switch (v)
          {
            case DateTime dateTime:
              valueDouble = dateTime.ToOADate();
              break;
            case TimeSpan timeSpan:
              valueDouble = new DateTime(timeSpan.Ticks).ToOADate();
              break;
            default:
              valueDouble = Convert.ToDouble(v, (IFormatProvider) CultureInfo.InvariantCulture);
              break;
          }
        }
        else
          valueDouble = 0.0;
      }
      catch
      {
        valueDouble = 0.0;
      }
      return valueDouble;
    }
  }
}
