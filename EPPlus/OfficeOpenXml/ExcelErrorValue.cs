// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelErrorValue
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml
{
  public class ExcelErrorValue
  {
    internal static ExcelErrorValue Create(eErrorType errorType) => new ExcelErrorValue(errorType);

    internal static ExcelErrorValue Parse(string val)
    {
      if (ExcelErrorValue.Values.StringIsErrorValue(val))
        return new ExcelErrorValue(ExcelErrorValue.Values.ToErrorType(val));
      if (string.IsNullOrEmpty(val))
        throw new ArgumentNullException(nameof (val));
      throw new ArgumentException("Not a valid error value: " + val);
    }

    private ExcelErrorValue(eErrorType type) => this.Type = type;

    public eErrorType Type { get; private set; }

    public override string ToString()
    {
      switch (this.Type)
      {
        case eErrorType.Div0:
          return "#DIV/0!";
        case eErrorType.NA:
          return "#N/A";
        case eErrorType.Name:
          return "#NAME?";
        case eErrorType.Null:
          return "#NULL!";
        case eErrorType.Num:
          return "#NUM!";
        case eErrorType.Ref:
          return "#REF!";
        case eErrorType.Value:
          return "#VALUE!";
        default:
          throw new ArgumentException("Invalid errortype");
      }
    }

    public static ExcelErrorValue operator +(object v1, ExcelErrorValue v2) => v2;

    public static ExcelErrorValue operator +(ExcelErrorValue v1, ExcelErrorValue v2) => v1;

    public override int GetHashCode() => base.GetHashCode();

    public override bool Equals(object obj)
    {
      return obj is ExcelErrorValue && ((ExcelErrorValue) obj).ToString() == this.ToString();
    }

    public static class Values
    {
      public const string Div0 = "#DIV/0!";
      public const string NA = "#N/A";
      public const string Name = "#NAME?";
      public const string Null = "#NULL!";
      public const string Num = "#NUM!";
      public const string Ref = "#REF!";
      public const string Value = "#VALUE!";
      private static Dictionary<string, eErrorType> _values = new Dictionary<string, eErrorType>()
      {
        {
          "#DIV/0!",
          eErrorType.Div0
        },
        {
          "#N/A",
          eErrorType.NA
        },
        {
          "#NAME?",
          eErrorType.Name
        },
        {
          "#NULL!",
          eErrorType.Null
        },
        {
          "#NUM!",
          eErrorType.Num
        },
        {
          "#REF!",
          eErrorType.Ref
        },
        {
          "#VALUE!",
          eErrorType.Value
        }
      };

      public static bool IsErrorValue(object candidate)
      {
        if (candidate == null || !(candidate is ExcelErrorValue))
          return false;
        string key = candidate.ToString();
        return !string.IsNullOrEmpty(key) && ExcelErrorValue.Values._values.ContainsKey(key);
      }

      public static bool StringIsErrorValue(string candidate)
      {
        return !string.IsNullOrEmpty(candidate) && ExcelErrorValue.Values._values.ContainsKey(candidate);
      }

      public static eErrorType ToErrorType(string val)
      {
        return !string.IsNullOrEmpty(val) && ExcelErrorValue.Values._values.ContainsKey(val) ? ExcelErrorValue.Values._values[val] : throw new ArgumentException("Invalid error code " + (val ?? "<empty>"));
      }
    }
  }
}
