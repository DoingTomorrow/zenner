// Decompiled with JetBrains decompiler
// Type: RestSharp.Extensions.ReflectionExtensions
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

#nullable disable
namespace RestSharp.Extensions
{
  public static class ReflectionExtensions
  {
    public static T GetAttribute<T>(this MemberInfo prop) where T : Attribute
    {
      return Attribute.GetCustomAttribute(prop, typeof (T)) as T;
    }

    public static T GetAttribute<T>(this Type type) where T : Attribute
    {
      return Attribute.GetCustomAttribute((MemberInfo) type, typeof (T)) as T;
    }

    public static bool IsSubclassOfRawGeneric(this Type toCheck, Type generic)
    {
      for (; toCheck != typeof (object); toCheck = toCheck.BaseType)
      {
        Type type = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
        if (generic == type)
          return true;
      }
      return false;
    }

    public static object ChangeType(this object source, Type newType)
    {
      return Convert.ChangeType(source, newType);
    }

    public static object ChangeType(this object source, Type newType, CultureInfo culture)
    {
      return Convert.ChangeType(source, newType, (IFormatProvider) culture);
    }

    public static object FindEnumValue(this Type type, string value, CultureInfo culture)
    {
      Enum enumValue = Enum.GetValues(type).Cast<Enum>().FirstOrDefault<Enum>((Func<Enum, bool>) (v => v.ToString().GetNameVariants(culture).Contains<string>(value, (IEqualityComparer<string>) StringComparer.Create(culture, true))));
      int result;
      if (enumValue == null && int.TryParse(value, out result) && Enum.IsDefined(type, (object) result))
        enumValue = (Enum) Enum.ToObject(type, result);
      return (object) enumValue;
    }
  }
}
