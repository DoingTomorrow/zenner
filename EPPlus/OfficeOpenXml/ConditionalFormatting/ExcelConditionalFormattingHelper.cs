// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ConditionalFormatting.ExcelConditionalFormattingHelper
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Utils;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.ConditionalFormatting
{
  internal static class ExcelConditionalFormattingHelper
  {
    public static string CheckAndFixRangeAddress(string address)
    {
      address = !address.Contains<char>(',') ? address.ToUpper() : throw new FormatException("Multiple addresses may not be commaseparated, use space instead");
      if (Regex.IsMatch(address, "[A-Z]+:[A-Z]+"))
        address = AddressUtility.ParseEntireColumnSelections(address);
      return address;
    }

    public static Color ConvertFromColorCode(string colorCode)
    {
      try
      {
        return Color.FromArgb(int.Parse(colorCode.Replace("#", ""), NumberStyles.HexNumber));
      }
      catch
      {
        return Color.White;
      }
    }

    public static string GetAttributeString(XmlNode node, string attribute)
    {
      try
      {
        string str = node.Attributes[attribute].Value;
        return str == null ? string.Empty : str;
      }
      catch
      {
        return string.Empty;
      }
    }

    public static int GetAttributeInt(XmlNode node, string attribute)
    {
      try
      {
        return int.Parse(node.Attributes[attribute].Value, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture);
      }
      catch
      {
        return int.MinValue;
      }
    }

    public static int? GetAttributeIntNullable(XmlNode node, string attribute)
    {
      try
      {
        return node.Attributes[attribute] == null ? new int?() : new int?(int.Parse(node.Attributes[attribute].Value, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture));
      }
      catch
      {
        return new int?();
      }
    }

    public static bool GetAttributeBool(XmlNode node, string attribute)
    {
      try
      {
        string str = node.Attributes[attribute].Value;
        return str == "1" || str == "-1" || str.ToUpper() == "TRUE";
      }
      catch
      {
        return false;
      }
    }

    public static bool? GetAttributeBoolNullable(XmlNode node, string attribute)
    {
      try
      {
        if (node.Attributes[attribute] == null)
          return new bool?();
        string str = node.Attributes[attribute].Value;
        return new bool?(str == "1" || str == "-1" || str.ToUpper() == "TRUE");
      }
      catch
      {
        return new bool?();
      }
    }

    public static double GetAttributeDouble(XmlNode node, string attribute)
    {
      try
      {
        return double.Parse(node.Attributes[attribute].Value, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture);
      }
      catch
      {
        return double.NaN;
      }
    }

    public static Decimal GetAttributeDecimal(XmlNode node, string attribute)
    {
      try
      {
        return Decimal.Parse(node.Attributes[attribute].Value, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture);
      }
      catch
      {
        return Decimal.MinValue;
      }
    }

    public static string EncodeXML(this string s)
    {
      return s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
    }

    public static string DecodeXML(this string s)
    {
      return s.Replace("'", "&apos;").Replace("\"", "&quot;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("&", "&amp;");
    }
  }
}
