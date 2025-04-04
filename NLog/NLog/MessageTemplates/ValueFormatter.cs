// Decompiled with JetBrains decompiler
// Type: NLog.MessageTemplates.ValueFormatter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.Collections;
using System.Text;

#nullable disable
namespace NLog.MessageTemplates
{
  internal class ValueFormatter : IValueFormatter
  {
    private static IValueFormatter _instance;
    private const int MaxRecursionDepth = 10;
    private const int MaxValueLength = 524288;
    private const string LiteralFormatSymbol = "l";
    private readonly MruCache<Enum, string> _enumCache = new MruCache<Enum, string>(1500);

    public static IValueFormatter Instance
    {
      get
      {
        return ValueFormatter._instance ?? (ValueFormatter._instance = (IValueFormatter) new ValueFormatter());
      }
      set => ValueFormatter._instance = value ?? (IValueFormatter) new ValueFormatter();
    }

    private ValueFormatter()
    {
    }

    public bool FormatValue(
      object value,
      string format,
      CaptureType captureType,
      IFormatProvider formatProvider,
      StringBuilder builder)
    {
      if (captureType == CaptureType.Serialize)
        return ConfigurationItemFactory.Default.JsonConverter.SerializeObject(value, builder);
      if (captureType != CaptureType.Stringify)
        return this.FormatObject(value, format, formatProvider, builder);
      builder.Append('"');
      ValueFormatter.FormatToString(value, (string) null, formatProvider, builder);
      builder.Append('"');
      return true;
    }

    public bool FormatObject(
      object value,
      string format,
      IFormatProvider formatProvider,
      StringBuilder builder)
    {
      if (this.SerializeSimpleObject(value, format, formatProvider, builder))
        return true;
      if (value is IEnumerable collection)
      {
        string format1 = format;
        IFormatProvider formatProvider1 = formatProvider;
        StringBuilder builder1 = builder;
        SingleItemOptimizedHashSet<object> objectsInPath = new SingleItemOptimizedHashSet<object>();
        return this.SerializeWithoutCyclicLoop(collection, format1, formatProvider1, builder1, objectsInPath, 0);
      }
      builder.Append(Convert.ToString(value, formatProvider));
      return true;
    }

    private bool SerializeSimpleObject(
      object value,
      string format,
      IFormatProvider formatProvider,
      StringBuilder builder)
    {
      if (value is string str)
      {
        int num = format != "l" ? 1 : 0;
        if (num != 0)
          builder.Append('"');
        builder.Append(str);
        if (num != 0)
          builder.Append('"');
        return true;
      }
      if (value == null)
      {
        builder.Append("NULL");
        return true;
      }
      if (!string.IsNullOrEmpty(format) && value is IFormattable formattable)
      {
        builder.Append(formattable.ToString(format, formatProvider));
        return true;
      }
      TypeCode typeCode = Convert.GetTypeCode(value);
      switch (typeCode)
      {
        case TypeCode.Boolean:
          builder.Append((bool) value ? "true" : "false");
          return true;
        case TypeCode.Char:
          int num1 = format != "l" ? 1 : 0;
          if (num1 != 0)
            builder.Append('"');
          builder.Append((char) value);
          if (num1 != 0)
            builder.Append('"');
          return true;
        case TypeCode.SByte:
        case TypeCode.Byte:
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.Int32:
        case TypeCode.UInt32:
        case TypeCode.Int64:
        case TypeCode.UInt64:
          if (value is Enum @enum)
            this.AppendEnumAsString(builder, @enum);
          else
            ValueFormatter.AppendIntegerAsString(builder, value, typeCode);
          return true;
        default:
          return false;
      }
    }

    private static void AppendIntegerAsString(StringBuilder sb, object value, TypeCode objTypeCode)
    {
      switch (objTypeCode)
      {
        case TypeCode.SByte:
          sb.AppendInvariant((int) (sbyte) value);
          break;
        case TypeCode.Byte:
          sb.AppendInvariant((int) (byte) value);
          break;
        case TypeCode.Int16:
          sb.AppendInvariant((int) (short) value);
          break;
        case TypeCode.UInt16:
          sb.AppendInvariant((int) (ushort) value);
          break;
        case TypeCode.Int32:
          sb.AppendInvariant((int) value);
          break;
        case TypeCode.UInt32:
          sb.AppendInvariant((uint) value);
          break;
        case TypeCode.Int64:
          long num1 = (long) value;
          if (num1 < (long) int.MaxValue && num1 > (long) int.MinValue)
          {
            sb.AppendInvariant((int) num1);
            break;
          }
          sb.Append(num1);
          break;
        case TypeCode.UInt64:
          ulong num2 = (ulong) value;
          if (num2 < (ulong) uint.MaxValue)
          {
            sb.AppendInvariant((uint) num2);
            break;
          }
          sb.Append(num2);
          break;
        default:
          sb.Append(XmlHelper.XmlConvertToString(value, objTypeCode));
          break;
      }
    }

    private void AppendEnumAsString(StringBuilder sb, Enum value)
    {
      string str;
      if (!this._enumCache.TryGetValue(value, out str))
      {
        str = value.ToString();
        this._enumCache.TryAddValue(value, str);
      }
      sb.Append(str);
    }

    private bool SerializeWithoutCyclicLoop(
      IEnumerable collection,
      string format,
      IFormatProvider formatProvider,
      StringBuilder builder,
      SingleItemOptimizedHashSet<object> objectsInPath,
      int depth)
    {
      if (objectsInPath.Contains((object) collection) || depth > 10)
        return false;
      if (collection is IDictionary dictionary)
      {
        using (new SingleItemOptimizedHashSet<object>.SingleItemScopedInsert((object) dictionary, ref objectsInPath, true))
          return this.SerializeDictionaryObject(dictionary, format, formatProvider, builder, objectsInPath, depth);
      }
      else
      {
        using (new SingleItemOptimizedHashSet<object>.SingleItemScopedInsert((object) collection, ref objectsInPath, true))
          return this.SerializeCollectionObject(collection, format, formatProvider, builder, objectsInPath, depth);
      }
    }

    private bool SerializeDictionaryObject(
      IDictionary dictionary,
      string format,
      IFormatProvider formatProvider,
      StringBuilder builder,
      SingleItemOptimizedHashSet<object> objectsInPath,
      int depth)
    {
      bool flag = false;
      foreach (DictionaryEntry dictionaryEntry in dictionary)
      {
        if (builder.Length > 524288)
          return false;
        if (flag)
          builder.Append(", ");
        if (dictionaryEntry.Key is string || !(dictionaryEntry.Key is IEnumerable))
          this.FormatObject(dictionaryEntry.Key, format, formatProvider, builder);
        else
          this.SerializeWithoutCyclicLoop((IEnumerable) dictionaryEntry.Key, format, formatProvider, builder, objectsInPath, depth + 1);
        builder.Append("=");
        if (dictionaryEntry.Value is string || !(dictionaryEntry.Value is IEnumerable))
          this.FormatObject(dictionaryEntry.Value, format, formatProvider, builder);
        else
          this.SerializeWithoutCyclicLoop((IEnumerable) dictionaryEntry.Value, format, formatProvider, builder, objectsInPath, depth + 1);
        flag = true;
      }
      return true;
    }

    private bool SerializeCollectionObject(
      IEnumerable collection,
      string format,
      IFormatProvider formatProvider,
      StringBuilder builder,
      SingleItemOptimizedHashSet<object> objectsInPath,
      int depth)
    {
      bool flag = false;
      foreach (object collection1 in collection)
      {
        if (builder.Length > 524288)
          return false;
        if (flag)
          builder.Append(", ");
        switch (collection1)
        {
          case IEnumerable _:
            this.SerializeWithoutCyclicLoop((IEnumerable) collection1, format, formatProvider, builder, objectsInPath, depth + 1);
            break;
          default:
            this.FormatObject(collection1, format, formatProvider, builder);
            break;
        }
        flag = true;
      }
      return true;
    }

    public static void FormatToString(
      object value,
      string format,
      IFormatProvider formatProvider,
      StringBuilder builder)
    {
      switch (value)
      {
        case string str:
          builder.Append(str);
          break;
        case IFormattable formattable:
          builder.Append(formattable.ToString(format, formatProvider));
          break;
        default:
          builder.Append(Convert.ToString(value, formatProvider));
          break;
      }
    }
  }
}
