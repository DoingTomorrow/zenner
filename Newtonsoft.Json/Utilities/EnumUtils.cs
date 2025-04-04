// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.EnumUtils
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

#nullable disable
namespace Newtonsoft.Json.Utilities
{
  internal static class EnumUtils
  {
    private static readonly ThreadSafeStore<Type, BidirectionalDictionary<string, string>> EnumMemberNamesPerType = new ThreadSafeStore<Type, BidirectionalDictionary<string, string>>(new Func<Type, BidirectionalDictionary<string, string>>(EnumUtils.InitializeEnumType));

    private static BidirectionalDictionary<string, string> InitializeEnumType(Type type)
    {
      BidirectionalDictionary<string, string> bidirectionalDictionary = new BidirectionalDictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      foreach (FieldInfo field in type.GetFields())
      {
        string name = field.Name;
        string second = field.GetCustomAttributes(typeof (EnumMemberAttribute), true).Cast<EnumMemberAttribute>().Select<EnumMemberAttribute, string>((Func<EnumMemberAttribute, string>) (a => a.Value)).SingleOrDefault<string>() ?? field.Name;
        if (bidirectionalDictionary.TryGetBySecond(second, out string _))
          throw new InvalidOperationException("Enum name '{0}' already exists on enum '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) second, (object) type.Name));
        bidirectionalDictionary.Set(name, second);
      }
      return bidirectionalDictionary;
    }

    public static IList<T> GetFlagsValues<T>(T value) where T : struct
    {
      Type type = typeof (T);
      if (!type.IsDefined(typeof (FlagsAttribute), false))
        throw new ArgumentException("Enum type {0} is not a set of flags.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) type));
      Type underlyingType = Enum.GetUnderlyingType(value.GetType());
      ulong uint64 = Convert.ToUInt64((object) value, (IFormatProvider) CultureInfo.InvariantCulture);
      IList<EnumValue<ulong>> namesAndValues = EnumUtils.GetNamesAndValues<T>();
      IList<T> flagsValues = (IList<T>) new List<T>();
      foreach (EnumValue<ulong> enumValue in (IEnumerable<EnumValue<ulong>>) namesAndValues)
      {
        if (((long) uint64 & (long) enumValue.Value) == (long) enumValue.Value && enumValue.Value != 0UL)
          flagsValues.Add((T) Convert.ChangeType((object) enumValue.Value, underlyingType, (IFormatProvider) CultureInfo.CurrentCulture));
      }
      if (flagsValues.Count == 0 && namesAndValues.SingleOrDefault<EnumValue<ulong>>((Func<EnumValue<ulong>, bool>) (v => v.Value == 0UL)) != null)
        flagsValues.Add(default (T));
      return flagsValues;
    }

    public static IList<EnumValue<ulong>> GetNamesAndValues<T>() where T : struct
    {
      return EnumUtils.GetNamesAndValues<ulong>(typeof (T));
    }

    public static IList<EnumValue<TUnderlyingType>> GetNamesAndValues<TUnderlyingType>(Type enumType) where TUnderlyingType : struct
    {
      if (enumType == (Type) null)
        throw new ArgumentNullException(nameof (enumType));
      IList<object> objectList = enumType.IsEnum() ? EnumUtils.GetValues(enumType) : throw new ArgumentException("Type {0} is not an Enum.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) enumType), nameof (enumType));
      IList<string> names = EnumUtils.GetNames(enumType);
      IList<EnumValue<TUnderlyingType>> namesAndValues = (IList<EnumValue<TUnderlyingType>>) new List<EnumValue<TUnderlyingType>>();
      for (int index = 0; index < objectList.Count; ++index)
      {
        try
        {
          namesAndValues.Add(new EnumValue<TUnderlyingType>(names[index], (TUnderlyingType) Convert.ChangeType(objectList[index], typeof (TUnderlyingType), (IFormatProvider) CultureInfo.CurrentCulture)));
        }
        catch (OverflowException ex)
        {
          throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Value from enum with the underlying type of {0} cannot be added to dictionary with a value type of {1}. Value was too large: {2}", new object[3]
          {
            (object) Enum.GetUnderlyingType(enumType),
            (object) typeof (TUnderlyingType),
            (object) Convert.ToUInt64(objectList[index], (IFormatProvider) CultureInfo.InvariantCulture)
          }), (Exception) ex);
        }
      }
      return namesAndValues;
    }

    public static IList<object> GetValues(Type enumType)
    {
      if (!enumType.IsEnum())
        throw new ArgumentException("Type '" + enumType.Name + "' is not an enum.");
      List<object> values = new List<object>();
      foreach (FieldInfo fieldInfo in ((IEnumerable<FieldInfo>) enumType.GetFields()).Where<FieldInfo>((Func<FieldInfo, bool>) (f => f.IsLiteral)))
      {
        object obj = fieldInfo.GetValue((object) enumType);
        values.Add(obj);
      }
      return (IList<object>) values;
    }

    public static IList<string> GetNames(Type enumType)
    {
      if (!enumType.IsEnum())
        throw new ArgumentException("Type '" + enumType.Name + "' is not an enum.");
      List<string> names = new List<string>();
      foreach (FieldInfo fieldInfo in ((IEnumerable<FieldInfo>) enumType.GetFields()).Where<FieldInfo>((Func<FieldInfo, bool>) (f => f.IsLiteral)))
        names.Add(fieldInfo.Name);
      return (IList<string>) names;
    }

    public static object ParseEnumName(string enumText, bool isNullable, Type t)
    {
      if (enumText == string.Empty & isNullable)
        return (object) null;
      BidirectionalDictionary<string, string> map = EnumUtils.EnumMemberNamesPerType.Get(t);
      string str;
      if (enumText.IndexOf(',') != -1)
      {
        string[] strArray = enumText.Split(',');
        for (int index = 0; index < strArray.Length; ++index)
        {
          string enumText1 = strArray[index].Trim();
          strArray[index] = EnumUtils.ResolvedEnumName(map, enumText1);
        }
        str = string.Join(", ", strArray);
      }
      else
        str = EnumUtils.ResolvedEnumName(map, enumText);
      return Enum.Parse(t, str, true);
    }

    public static string ToEnumName(Type enumType, string enumText, bool camelCaseText)
    {
      BidirectionalDictionary<string, string> bidirectionalDictionary = EnumUtils.EnumMemberNamesPerType.Get(enumType);
      string[] strArray = enumText.Split(',');
      for (int index = 0; index < strArray.Length; ++index)
      {
        string first = strArray[index].Trim();
        string second;
        bidirectionalDictionary.TryGetByFirst(first, out second);
        second = second ?? first;
        if (camelCaseText)
          second = StringUtils.ToCamelCase(second);
        strArray[index] = second;
      }
      return string.Join(", ", strArray);
    }

    private static string ResolvedEnumName(
      BidirectionalDictionary<string, string> map,
      string enumText)
    {
      string first;
      map.TryGetBySecond(enumText, out first);
      return first ?? enumText;
    }
  }
}
