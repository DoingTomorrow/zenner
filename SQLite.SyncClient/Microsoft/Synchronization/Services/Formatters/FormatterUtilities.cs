// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.Services.Formatters.FormatterUtilities
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;
using System.Globalization;
using System.Xml;

#nullable disable
namespace Microsoft.Synchronization.Services.Formatters
{
  internal static class FormatterUtilities
  {
    private static CultureInfo USCultureInfo = new CultureInfo("en-US");

    public static string GetEdmType(Type type)
    {
      if (type.IsGenericType())
        return FormatterUtilities.GetEdmType(type.GetGenericArguments()[0]);
      switch (type.Name)
      {
        case "Boolean":
          return "Edm.Boolean";
        case "Byte":
          return "Edm.Byte";
        case "Byte[]":
          return "Edm.Binary";
        case "Char":
        case "String":
          return "Edm.String";
        case "DBNull":
          return "null";
        case "DateTime":
          return "Edm.DateTime";
        case "DateTimeOffset":
          return "Edm.DateTimeOffset";
        case "Decimal":
          return "Edm.Decimal";
        case "Double":
          return "Edm.Double";
        case "Guid":
          return "Edm.Guid";
        case "Int16":
          return "Edm.Int16";
        case "Int32":
          return "Edm.Int32";
        case "Int64":
          return "Edm.Int64";
        case "SByte":
          return "Edm.SByte";
        case "Single":
          return "Edm.Single";
        case "TimeSpan":
          return "Edm.Time";
        default:
          throw new NotSupportedException("TypeCode " + type.Name + " is not a supported type.");
      }
    }

    public static object ConvertDateTimeForType_Json(object objValue, Type type)
    {
      if (type == FormatterConstants.DateTimeType)
        return (object) FormatterUtilities.ConvertDateTimeToJson((DateTime) objValue);
      return type == FormatterConstants.TimeSpanType ? (object) FormatterUtilities.ConvertTimeToJson((TimeSpan) objValue) : (object) FormatterUtilities.ConvertDateTimeOffsetToJson((DateTimeOffset) objValue);
    }

    public static object ConvertDateTimeForType_Atom(object objValue, Type type)
    {
      if (type == FormatterConstants.DateTimeType)
        return (object) FormatterUtilities.ConvertDateTimeToAtom((DateTime) objValue);
      return type == FormatterConstants.TimeSpanType ? (object) FormatterUtilities.ConvertTimeToAtom((TimeSpan) objValue) : (object) FormatterUtilities.ConvertDateTimeOffsetToAtom((DateTimeOffset) objValue);
    }

    public static string ConvertDateTimeToAtom(DateTime date)
    {
      return date.ToString("yyyy-MM-ddTHH:mm:ss.fffffff", (IFormatProvider) FormatterUtilities.USCultureInfo);
    }

    public static string ConvertTimeToAtom(TimeSpan t) => t.ToString();

    public static string ConvertDateTimeOffsetToAtom(DateTimeOffset dto)
    {
      return dto.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz", (IFormatProvider) FormatterUtilities.USCultureInfo);
    }

    public static string ConvertDateTimeToJson(DateTime date)
    {
      return string.Format((IFormatProvider) FormatterUtilities.USCultureInfo, "/Date({0})/", (object) ((date.Ticks - FormatterConstants.JsonDateTimeStartTime.Ticks) / FormatterConstants.JsonNanoToMilliSecondsFactor));
    }

    public static string ConvertTimeToJson(TimeSpan t)
    {
      return string.Format((IFormatProvider) FormatterUtilities.USCultureInfo, "time'{0}'", (object) t.ToString());
    }

    public static string ConvertDateTimeOffsetToJson(DateTimeOffset dto)
    {
      return string.Format((IFormatProvider) FormatterUtilities.USCultureInfo, "datetimeoffset'{0}'", (object) dto.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz", (IFormatProvider) FormatterUtilities.USCultureInfo));
    }

    internal static object ParseDateTimeFromString(string value, Type type)
    {
      try
      {
        return value.IndexOf("date", 0, StringComparison.OrdinalIgnoreCase) >= 0 || value.Contains("time") ? FormatterUtilities.ParseJsonString(value, type) : FormatterUtilities.ParseAtomString(value, type);
      }
      catch (FormatException ex)
      {
        throw new InvalidOperationException(string.Format((IFormatProvider) FormatterUtilities.USCultureInfo, "Invalid Date/Time value received. Unable to parse value {0} to type {1}.", (object) value, (object) type.Name));
      }
    }

    private static object ParseAtomString(string value, Type type)
    {
      if (FormatterConstants.DateTimeType.IsAssignableFrom(type))
        return (object) Microsoft.Synchronization.ClientServices.XmlConverter.ToDateTime(value);
      return FormatterConstants.DateTimeOffsetType.IsAssignableFrom(type) ? (object) XmlConvert.ToDateTimeOffset(value, "yyyy-MM-ddTHH:mm:ss.fffffffzzz") : (object) TimeSpan.Parse(value);
    }

    private static object ParseJsonString(string value, Type type)
    {
      if (FormatterConstants.DateTimeType.IsAssignableFrom(type))
      {
        try
        {
          int startIndex = value.IndexOf("(", StringComparison.Ordinal) + 1;
          int num1 = value.IndexOf(")", StringComparison.Ordinal);
          long ticks1 = long.Parse(value.Substring(startIndex, num1 - startIndex), (IFormatProvider) FormatterUtilities.USCultureInfo) * FormatterConstants.JsonNanoToMilliSecondsFactor + FormatterConstants.JsonDateTimeStartTime.Ticks;
          long num2 = ticks1;
          DateTime dateTime = DateTime.MinValue;
          long ticks2 = dateTime.Ticks;
          int num3;
          if (num2 >= ticks2)
          {
            long num4 = ticks1;
            dateTime = DateTime.MaxValue;
            long ticks3 = dateTime.Ticks;
            num3 = num4 > ticks3 ? 1 : 0;
          }
          else
            num3 = 1;
          if (num3 != 0)
            throw new InvalidOperationException(string.Format("Invalid JSON DateTime value received. Value '{0}' is not a valid DateTime", (object) ticks1));
          return (object) new DateTime(ticks1);
        }
        catch (IndexOutOfRangeException ex)
        {
          throw new InvalidOperationException(string.Format((IFormatProvider) FormatterUtilities.USCultureInfo, "Invalid Json DateTime value received. Value {0} is not in format '\\/Date(ticks)\\/'.", (object) value));
        }
      }
      else if (FormatterConstants.DateTimeOffsetType.IsAssignableFrom(type))
      {
        try
        {
          int startIndex = value.IndexOf("'", StringComparison.Ordinal) + 1;
          int num = value.LastIndexOf("'", StringComparison.Ordinal);
          return (object) XmlConvert.ToDateTimeOffset(value.Substring(startIndex, num - startIndex), "yyyy-MM-ddTHH:mm:ss.fffffffzzz");
        }
        catch (IndexOutOfRangeException ex)
        {
          throw new InvalidOperationException(string.Format((IFormatProvider) FormatterUtilities.USCultureInfo, "Invalid Json DateTimeOffset value received. Value {0} is not in format 'datetimeoffset'yyyy-MM-ddTHH:mm:ss.fffffffzzz''.", (object) value));
        }
      }
      else
      {
        try
        {
          int startIndex = value.IndexOf("'", StringComparison.Ordinal) + 1;
          int num = value.LastIndexOf("'", StringComparison.Ordinal);
          return (object) TimeSpan.Parse(value.Substring(startIndex, num - startIndex));
        }
        catch (IndexOutOfRangeException ex)
        {
          throw new InvalidOperationException(string.Format((IFormatProvider) FormatterUtilities.USCultureInfo, "Invalid Json TimeSpan value received. Value {0} is not in format 'time'HH:mm:ss''.", (object) value));
        }
      }
    }
  }
}
