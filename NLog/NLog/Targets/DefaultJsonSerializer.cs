// Decompiled with JetBrains decompiler
// Type: NLog.Targets.DefaultJsonSerializer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

#nullable disable
namespace NLog.Targets
{
  public class DefaultJsonSerializer : IJsonConverter, IJsonSerializer
  {
    private readonly MruCache<Type, KeyValuePair<PropertyInfo[], ReflectionHelpers.LateBoundMethod[]>> _propsCache = new MruCache<Type, KeyValuePair<PropertyInfo[], ReflectionHelpers.LateBoundMethod[]>>(10000);
    private readonly MruCache<Enum, string> _enumCache = new MruCache<Enum, string>(1500);
    private readonly JsonSerializeOptions _serializeOptions = new JsonSerializeOptions();
    private readonly JsonSerializeOptions _exceptionSerializeOptions = new JsonSerializeOptions()
    {
      SanitizeDictionaryKeys = true
    };
    private readonly IFormatProvider _defaultFormatProvider = (IFormatProvider) DefaultJsonSerializer.CreateFormatProvider();
    private const int MaxJsonLength = 524288;
    private static readonly DefaultJsonSerializer instance = new DefaultJsonSerializer();
    private const BindingFlags PublicProperties = BindingFlags.Instance | BindingFlags.Public;

    public static DefaultJsonSerializer Instance => DefaultJsonSerializer.instance;

    private DefaultJsonSerializer()
    {
    }

    public string SerializeObject(object value)
    {
      return this.SerializeObject(value, this._serializeOptions);
    }

    public string SerializeObject(object value, JsonSerializeOptions options)
    {
      if (value == null)
        return "null";
      if (value is string text)
        return DefaultJsonSerializer.QuoteValue(DefaultJsonSerializer.EscapeString(text, options.EscapeUnicode));
      TypeCode typeCode = Convert.GetTypeCode(value);
      if (typeCode != TypeCode.Object && StringHelpers.IsNullOrWhiteSpace(options.Format) && options.FormatProvider == null)
      {
        if (!options.EnumAsInteger && DefaultJsonSerializer.IsNumericTypeCode(typeCode, false) && value is Enum @enum)
          return DefaultJsonSerializer.QuoteValue(this.EnumAsString(@enum));
        string str = XmlHelper.XmlConvertToString(value, typeCode);
        return DefaultJsonSerializer.SkipQuotes(typeCode) ? str : DefaultJsonSerializer.QuoteValue(str);
      }
      StringBuilder destination = new StringBuilder();
      return !this.SerializeObject(value, destination, options) ? (string) null : destination.ToString();
    }

    public bool SerializeObject(object value, StringBuilder destination)
    {
      return this.SerializeObject(value, destination, this._serializeOptions);
    }

    public bool SerializeObject(
      object value,
      StringBuilder destination,
      JsonSerializeOptions options)
    {
      return this.SerializeObject(value, destination, options, new SingleItemOptimizedHashSet<object>(), 0);
    }

    private bool SerializeObject(
      object value,
      StringBuilder destination,
      JsonSerializeOptions options,
      SingleItemOptimizedHashSet<object> objectsInPath,
      int depth)
    {
      if (objectsInPath.Contains(value))
        return false;
      switch (value)
      {
        case null:
          destination.Append("null");
          break;
        case string text:
          DefaultJsonSerializer.QuoteValue(destination, DefaultJsonSerializer.EscapeString(text, options.EscapeUnicode));
          break;
        case IDictionary singleItem:
          using (new SingleItemOptimizedHashSet<object>.SingleItemScopedInsert((object) singleItem, ref objectsInPath, true))
          {
            this.SerializeDictionaryObject(singleItem, destination, options, objectsInPath, depth);
            break;
          }
        case IEnumerable enumerable:
          using (new SingleItemOptimizedHashSet<object>.SingleItemScopedInsert(value, ref objectsInPath, true))
          {
            this.SerializeCollectionObject(enumerable, destination, options, objectsInPath, depth);
            break;
          }
        default:
          string format = options.Format;
          bool hasFormat = !StringHelpers.IsNullOrWhiteSpace(format);
          if (options.FormatProvider != null | hasFormat && value is IFormattable formattable)
          {
            if (!this.SerializeWithFormatProvider(value, destination, options, formattable, format, hasFormat))
              return false;
            break;
          }
          if (!this.SerializeTypeCodeValue(value, destination, options, objectsInPath, depth))
            return false;
          break;
      }
      return true;
    }

    private bool SerializeWithFormatProvider(
      object value,
      StringBuilder destination,
      JsonSerializeOptions options,
      IFormattable formattable,
      string format,
      bool hasFormat)
    {
      int length = destination.Length;
      try
      {
        bool flag = !DefaultJsonSerializer.SkipQuotes(Convert.GetTypeCode(value));
        if (flag)
          destination.Append('"');
        if (hasFormat)
        {
          IFormatProvider provider = options.FormatProvider ?? this._defaultFormatProvider;
          destination.AppendFormat(provider, "{0:" + format + "}", new object[1]
          {
            value
          });
        }
        else
          destination.Append(DefaultJsonSerializer.EscapeString(formattable.ToString("", options.FormatProvider), options.EscapeUnicode));
        if (flag)
          destination.Append('"');
        return true;
      }
      catch
      {
        destination.Length = length;
        return false;
      }
    }

    private void SerializeDictionaryObject(
      IDictionary value,
      StringBuilder destination,
      JsonSerializeOptions options,
      SingleItemOptimizedHashSet<object> objectsInPath,
      int depth)
    {
      bool flag = true;
      int depth1 = objectsInPath.Count <= 1 ? depth : depth + 1;
      if (depth1 > options.MaxRecursionLimit)
        return;
      destination.Append('{');
      foreach (DictionaryEntry dictionaryEntry in value)
      {
        int length = destination.Length;
        if (length <= 524288)
        {
          if (!flag)
            destination.Append(',');
          if (!this.SerializeObject(dictionaryEntry.Key, destination, options, objectsInPath, depth1))
          {
            destination.Length = length;
          }
          else
          {
            if (options.SanitizeDictionaryKeys)
            {
              int num1 = options.QuoteKeys ? 1 : 0;
              int num2 = destination.Length - num1;
              int keyStartIndex = length + (flag ? 0 : 1) + num1;
              if (!DefaultJsonSerializer.SanitizeDictionaryKey(destination, keyStartIndex, num2 - keyStartIndex))
              {
                destination.Length = length;
                continue;
              }
            }
            destination.Append(':');
            if (!this.SerializeObject(dictionaryEntry.Value, destination, options, objectsInPath, depth1))
              destination.Length = length;
            else
              flag = false;
          }
        }
        else
          break;
      }
      destination.Append('}');
    }

    private static bool SanitizeDictionaryKey(
      StringBuilder destination,
      int keyStartIndex,
      int keyLength)
    {
      if (keyLength == 0)
        return false;
      int num = keyStartIndex + keyLength;
      for (int index = keyStartIndex; index < num; ++index)
      {
        char c = destination[index];
        if (c != '_' && !char.IsLetterOrDigit(c))
          destination[index] = '_';
      }
      return true;
    }

    private void SerializeCollectionObject(
      IEnumerable value,
      StringBuilder destination,
      JsonSerializeOptions options,
      SingleItemOptimizedHashSet<object> objectsInPath,
      int depth)
    {
      bool flag = true;
      int depth1 = objectsInPath.Count <= 1 ? depth : depth + 1;
      if (depth1 > options.MaxRecursionLimit)
        return;
      destination.Append('[');
      foreach (object obj in value)
      {
        int length = destination.Length;
        if (length <= 524288)
        {
          if (!flag)
            destination.Append(',');
          if (!this.SerializeObject(obj, destination, options, objectsInPath, depth1))
            destination.Length = length;
          else
            flag = false;
        }
        else
          break;
      }
      destination.Append(']');
    }

    private bool SerializeTypeCodeValue(
      object value,
      StringBuilder destination,
      JsonSerializeOptions options,
      SingleItemOptimizedHashSet<object> objectsInPath,
      int depth)
    {
      TypeCode typeCode = Convert.GetTypeCode(value);
      if (typeCode == TypeCode.Object)
      {
        if (value is Guid || value is TimeSpan || (object) (value as MemberInfo) != null || (object) (value as Assembly) != null)
          DefaultJsonSerializer.QuoteValue(destination, Convert.ToString(value, (IFormatProvider) CultureInfo.InvariantCulture));
        else if (value is DateTimeOffset)
        {
          DefaultJsonSerializer.QuoteValue(destination, string.Format("{0:yyyy-MM-dd HH:mm:ss zzz}", value));
        }
        else
        {
          int length = destination.Length;
          if (length > 524288)
            return false;
          if (depth < options.MaxRecursionLimit)
          {
            try
            {
              if (value is Exception && options == DefaultJsonSerializer.instance._serializeOptions)
                options = DefaultJsonSerializer.instance._exceptionSerializeOptions;
              using (new SingleItemOptimizedHashSet<object>.SingleItemScopedInsert(value, ref objectsInPath, false))
                return this.SerializeProperties(value, destination, options, objectsInPath, depth);
            }
            catch
            {
              destination.Length = length;
              return false;
            }
          }
          else
          {
            try
            {
              string str = DefaultJsonSerializer.EscapeString(Convert.ToString(value, (IFormatProvider) CultureInfo.InvariantCulture), options.EscapeUnicode);
              DefaultJsonSerializer.QuoteValue(destination, str);
            }
            catch
            {
              return false;
            }
          }
        }
      }
      else if (DefaultJsonSerializer.IsNumericTypeCode(typeCode, false))
      {
        this.SerializeNumber(value, destination, options, typeCode);
      }
      else
      {
        string str = XmlHelper.XmlConvertToString(value, typeCode);
        if (str == null)
          return false;
        if (DefaultJsonSerializer.SkipQuotes(typeCode))
          destination.Append(str);
        else
          DefaultJsonSerializer.QuoteValue(destination, str);
      }
      return true;
    }

    private void SerializeNumber(
      object value,
      StringBuilder destination,
      JsonSerializeOptions options,
      TypeCode objTypeCode)
    {
      if (!options.EnumAsInteger && value is Enum @enum)
        DefaultJsonSerializer.QuoteValue(destination, this.EnumAsString(@enum));
      else
        destination.AppendIntegerAsString(value, objTypeCode);
    }

    private static CultureInfo CreateFormatProvider()
    {
      CultureInfo formatProvider = new CultureInfo("en-US", false);
      NumberFormatInfo numberFormat = formatProvider.NumberFormat;
      numberFormat.NumberGroupSeparator = string.Empty;
      numberFormat.NumberDecimalSeparator = ".";
      numberFormat.NumberGroupSizes = new int[1];
      return formatProvider;
    }

    private static string QuoteValue(string value) => "\"" + value + "\"";

    private static void QuoteValue(StringBuilder destination, string value)
    {
      destination.Append('"');
      destination.Append(value);
      destination.Append('"');
    }

    private string EnumAsString(Enum value)
    {
      string str;
      if (!this._enumCache.TryGetValue(value, out str))
      {
        str = Convert.ToString((object) value, (IFormatProvider) CultureInfo.InvariantCulture);
        this._enumCache.TryAddValue(value, str);
      }
      return str;
    }

    private static bool SkipQuotes(TypeCode objTypeCode)
    {
      switch (objTypeCode)
      {
        case TypeCode.Empty:
        case TypeCode.Boolean:
          return true;
        case TypeCode.String:
          return false;
        default:
          return DefaultJsonSerializer.IsNumericTypeCode(objTypeCode, true);
      }
    }

    private static bool IsNumericTypeCode(TypeCode objTypeCode, bool includeDecimals)
    {
      switch (objTypeCode)
      {
        case TypeCode.SByte:
        case TypeCode.Byte:
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.Int32:
        case TypeCode.UInt32:
        case TypeCode.Int64:
        case TypeCode.UInt64:
          return true;
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
          return includeDecimals;
        default:
          return false;
      }
    }

    internal static string EscapeString(string text, bool escapeUnicode)
    {
      if (text == null)
        return (string) null;
      StringBuilder stringBuilder = (StringBuilder) null;
      for (int index = 0; index < text.Length; ++index)
      {
        char ch = text[index];
        if (stringBuilder == null)
        {
          if (DefaultJsonSerializer.RequiresJsonEscape(ch, escapeUnicode))
          {
            stringBuilder = new StringBuilder(text.Length + 4);
            stringBuilder.Append(text, 0, index);
          }
          else
            continue;
        }
        switch (ch)
        {
          case '\b':
            stringBuilder.Append("\\b");
            continue;
          case '\t':
            stringBuilder.Append("\\t");
            continue;
          case '\n':
            stringBuilder.Append("\\n");
            continue;
          case '\f':
            stringBuilder.Append("\\f");
            continue;
          case '\r':
            stringBuilder.Append("\\r");
            continue;
          case '"':
            stringBuilder.Append("\\\"");
            continue;
          case '/':
            stringBuilder.Append("\\/");
            continue;
          case '\\':
            stringBuilder.Append("\\\\");
            continue;
          default:
            if (DefaultJsonSerializer.EscapeChar(ch, escapeUnicode))
            {
              stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "\\u{0:x4}", new object[1]
              {
                (object) (int) ch
              });
              continue;
            }
            stringBuilder.Append(ch);
            continue;
        }
      }
      return stringBuilder != null ? stringBuilder.ToString() : text;
    }

    internal static bool RequiresJsonEscape(char ch, bool escapeUnicode)
    {
      return DefaultJsonSerializer.EscapeChar(ch, escapeUnicode) || ch == '"' || ch == '/' || ch == '\\';
    }

    private static bool EscapeChar(char ch, bool escapeUnicode)
    {
      if (ch < ' ')
        return true;
      return escapeUnicode && ch > '\u007F';
    }

    private bool SerializeProperties(
      object value,
      StringBuilder destination,
      JsonSerializeOptions options,
      SingleItemOptimizedHashSet<object> objectsInPath,
      int depth)
    {
      KeyValuePair<PropertyInfo[], ReflectionHelpers.LateBoundMethod[]> props = this.GetProps(value);
      if (props.Key.Length == 0)
      {
        try
        {
          DefaultJsonSerializer.QuoteValue(destination, DefaultJsonSerializer.EscapeString(Convert.ToString(value, (IFormatProvider) CultureInfo.InvariantCulture), options.EscapeUnicode));
          return true;
        }
        catch
        {
          return false;
        }
      }
      else
      {
        destination.Append('{');
        bool flag1 = true;
        bool flag2 = props.Key.Length == props.Value.Length;
        for (int index = 0; index < props.Key.Length; ++index)
        {
          int length = destination.Length;
          try
          {
            PropertyInfo propertyInfo = props.Key[index];
            object obj = flag2 ? props.Value[index](value, (object[]) null) : propertyInfo.GetValue(value, (object[]) null);
            if (obj != null)
            {
              if (!flag1)
                destination.Append(", ");
              if (options.QuoteKeys)
                DefaultJsonSerializer.QuoteValue(destination, propertyInfo.Name);
              else
                destination.Append(propertyInfo.Name);
              destination.Append(':');
              if (!this.SerializeObject(obj, destination, options, objectsInPath, depth + 1))
                destination.Length = length;
              else
                flag1 = false;
            }
          }
          catch
          {
            destination.Length = length;
          }
        }
        destination.Append('}');
        return true;
      }
    }

    private KeyValuePair<PropertyInfo[], ReflectionHelpers.LateBoundMethod[]> GetProps(object value)
    {
      Type type = value.GetType();
      KeyValuePair<PropertyInfo[], ReflectionHelpers.LateBoundMethod[]> props;
      if (this._propsCache.TryGetValue(type, out props))
      {
        if (props.Key.Length != 0 && props.Value.Length == 0)
        {
          ReflectionHelpers.LateBoundMethod[] lateBoundMethodArray = new ReflectionHelpers.LateBoundMethod[props.Key.Length];
          for (int index = 0; index < props.Key.Length; ++index)
            lateBoundMethodArray[index] = ReflectionHelpers.CreateLateBoundMethod(props.Key[index].GetGetMethod());
          props = new KeyValuePair<PropertyInfo[], ReflectionHelpers.LateBoundMethod[]>(props.Key, lateBoundMethodArray);
          this._propsCache.TryAddValue(type, props);
        }
        return props;
      }
      PropertyInfo[] propertyInfoArray = (PropertyInfo[]) null;
      try
      {
        propertyInfoArray = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
      }
      catch (Exception ex)
      {
        object[] objArray = new object[1]{ (object) type };
        InternalLogger.Warn(ex, "Failed to get JSON properties for type: {0}", objArray);
      }
      finally
      {
        if (propertyInfoArray == null)
          propertyInfoArray = ArrayHelper.Empty<PropertyInfo>();
      }
      foreach (PropertyInfo propertyInfo in propertyInfoArray)
      {
        if (!propertyInfo.CanRead || propertyInfo.GetIndexParameters().Length != 0 || propertyInfo.GetGetMethod() == (MethodInfo) null)
        {
          propertyInfoArray = ((IEnumerable<PropertyInfo>) propertyInfoArray).Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.CanRead && p.GetIndexParameters().Length == 0 && p.GetGetMethod() != (MethodInfo) null)).ToArray<PropertyInfo>();
          break;
        }
      }
      props = new KeyValuePair<PropertyInfo[], ReflectionHelpers.LateBoundMethod[]>(propertyInfoArray, ArrayHelper.Empty<ReflectionHelpers.LateBoundMethod>());
      this._propsCache.TryAddValue(type, props);
      return props;
    }
  }
}
