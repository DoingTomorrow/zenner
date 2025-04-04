// Decompiled with JetBrains decompiler
// Type: RestSharp.Reflection.ReflectionUtils
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace RestSharp.Reflection
{
  internal class ReflectionUtils
  {
    public static Attribute GetAttribute(MemberInfo info, Type type)
    {
      return info == (MemberInfo) null || type == (Type) null || !Attribute.IsDefined(info, type) ? (Attribute) null : Attribute.GetCustomAttribute(info, type);
    }

    public static Attribute GetAttribute(Type objectType, Type attributeType)
    {
      return objectType == (Type) null || attributeType == (Type) null || !Attribute.IsDefined((MemberInfo) objectType, attributeType) ? (Attribute) null : Attribute.GetCustomAttribute((MemberInfo) objectType, attributeType);
    }

    public static bool IsTypeGenericeCollectionInterface(Type type)
    {
      if (!type.IsGenericType)
        return false;
      Type genericTypeDefinition = type.GetGenericTypeDefinition();
      return genericTypeDefinition == typeof (IList<>) || genericTypeDefinition == typeof (ICollection<>) || genericTypeDefinition == typeof (IEnumerable<>);
    }

    public static bool IsTypeDictionary(Type type)
    {
      if (typeof (IDictionary).IsAssignableFrom(type))
        return true;
      return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (IDictionary<,>);
    }

    public static bool IsNullableType(Type type)
    {
      return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>);
    }

    public static object ToNullableType(object obj, Type nullableType)
    {
      return obj != null ? Convert.ChangeType(obj, Nullable.GetUnderlyingType(nullableType), (IFormatProvider) CultureInfo.InvariantCulture) : (object) null;
    }
  }
}
