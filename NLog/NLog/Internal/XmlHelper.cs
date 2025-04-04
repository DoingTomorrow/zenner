// Decompiled with JetBrains decompiler
// Type: NLog.Internal.XmlHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace NLog.Internal
{
  public static class XmlHelper
  {
    private static readonly Regex InvalidXmlChars = new Regex("(?<![\\uD800-\\uDBFF])[\\uDC00-\\uDFFF]|[\\uD800-\\uDBFF](?![\\uDC00-\\uDFFF])|[\\x00-\\x08\\x0B\\x0C\\x0E-\\x1F\\x7F-\\x9F\\uFEFF\\uFFFE\\uFFFF]", RegexOptions.Compiled);

    private static string RemoveInvalidXmlChars(string text)
    {
      if (string.IsNullOrEmpty(text))
        return string.Empty;
      for (int index = 0; index < text.Length; ++index)
      {
        if (!XmlConvert.IsXmlChar(text[index]))
          return XmlHelper.CreateValidXmlString(text);
      }
      return text;
    }

    private static string CreateValidXmlString(string text)
    {
      StringBuilder stringBuilder = new StringBuilder(text.Length);
      for (int index = 0; index < text.Length; ++index)
      {
        char ch = text[index];
        if (XmlConvert.IsXmlChar(ch))
          stringBuilder.Append(ch);
      }
      return stringBuilder.ToString();
    }

    internal static string XmlConvertToStringSafe(object value)
    {
      return XmlHelper.RemoveInvalidXmlChars(XmlHelper.XmlConvertToString(value));
    }

    internal static string XmlConvertToString(object value)
    {
      TypeCode typeCode = Convert.GetTypeCode(value);
      return XmlHelper.XmlConvertToString(value, typeCode);
    }

    internal static string XmlConvertToString(object value, TypeCode objTypeCode)
    {
      if (value == null)
        return "null";
      switch (objTypeCode)
      {
        case TypeCode.Boolean:
          return XmlConvert.ToString((bool) value);
        case TypeCode.Char:
          return XmlConvert.ToString((char) value);
        case TypeCode.SByte:
          return XmlConvert.ToString((sbyte) value);
        case TypeCode.Byte:
          return XmlConvert.ToString((byte) value);
        case TypeCode.Int16:
          return XmlConvert.ToString((short) value);
        case TypeCode.UInt16:
          return XmlConvert.ToString((ushort) value);
        case TypeCode.Int32:
          return XmlConvert.ToString((int) value);
        case TypeCode.UInt32:
          return XmlConvert.ToString((uint) value);
        case TypeCode.Int64:
          return XmlConvert.ToString((long) value);
        case TypeCode.UInt64:
          return XmlConvert.ToString((ulong) value);
        case TypeCode.Single:
          float f = (float) value;
          return float.IsInfinity(f) ? Convert.ToString(f, (IFormatProvider) CultureInfo.InvariantCulture) : XmlConvert.ToString(f);
        case TypeCode.Double:
          double d = (double) value;
          return double.IsInfinity(d) ? Convert.ToString(d, (IFormatProvider) CultureInfo.InvariantCulture) : XmlConvert.ToString(d);
        case TypeCode.Decimal:
          return XmlConvert.ToString((Decimal) value);
        case TypeCode.DateTime:
          return XmlConvert.ToString((DateTime) value, XmlDateTimeSerializationMode.Utc);
        case TypeCode.String:
          return (string) value;
        default:
          try
          {
            return Convert.ToString(value, (IFormatProvider) CultureInfo.InvariantCulture);
          }
          catch
          {
            return (string) null;
          }
      }
    }

    public static void WriteAttributeSafeString(
      this XmlWriter writer,
      string prefix,
      string localName,
      string ns,
      string value)
    {
      writer.WriteAttributeString(XmlHelper.RemoveInvalidXmlChars(prefix), XmlHelper.RemoveInvalidXmlChars(localName), XmlHelper.RemoveInvalidXmlChars(ns), XmlHelper.RemoveInvalidXmlChars(value));
    }

    public static void WriteAttributeSafeString(
      this XmlWriter writer,
      string thread,
      string localName)
    {
      writer.WriteAttributeString(XmlHelper.RemoveInvalidXmlChars(thread), XmlHelper.RemoveInvalidXmlChars(localName));
    }

    public static void WriteElementSafeString(
      this XmlWriter writer,
      string prefix,
      string localName,
      string ns,
      string value)
    {
      writer.WriteElementString(XmlHelper.RemoveInvalidXmlChars(prefix), XmlHelper.RemoveInvalidXmlChars(localName), XmlHelper.RemoveInvalidXmlChars(ns), XmlHelper.RemoveInvalidXmlChars(value));
    }

    public static void WriteSafeCData(this XmlWriter writer, string text)
    {
      writer.WriteCData(XmlHelper.RemoveInvalidXmlChars(text));
    }
  }
}
