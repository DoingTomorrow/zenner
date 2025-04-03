// Decompiled with JetBrains decompiler
// Type: AutoMapper.Internal.PrimitiveExtensions
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace AutoMapper.Internal
{
  public static class PrimitiveExtensions
  {
    public static string ToNullSafeString(this object value) => value?.ToString();

    public static bool IsNullableType(this Type type)
    {
      return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof (Nullable<>));
    }

    public static Type GetTypeOfNullable(this Type type) => type.GetGenericArguments()[0];

    public static bool IsCollectionType(this Type type)
    {
      return type.IsGenericType && (object) type.GetGenericTypeDefinition() == (object) typeof (ICollection<>) || ((IEnumerable<Type>) type.GetInterfaces()).Where<Type>((Func<Type, bool>) (t => t.IsGenericType)).Select<Type, Type>((Func<Type, Type>) (t => t.GetGenericTypeDefinition())).Any<Type>((Func<Type, bool>) (t => (object) t == (object) typeof (ICollection<>)));
    }

    public static bool IsEnumerableType(this Type type)
    {
      return ((IEnumerable<Type>) type.GetInterfaces()).Contains<Type>(typeof (IEnumerable));
    }

    public static bool IsListType(this Type type)
    {
      return ((IEnumerable<Type>) type.GetInterfaces()).Contains<Type>(typeof (IList));
    }

    public static bool IsListOrDictionaryType(this Type type)
    {
      return type.IsListType() || type.IsDictionaryType();
    }

    public static bool IsDictionaryType(this Type type)
    {
      return type.IsGenericType && (object) type.GetGenericTypeDefinition() == (object) typeof (System.Collections.Generic.IDictionary<,>) || ((IEnumerable<Type>) type.GetInterfaces()).Where<Type>((Func<Type, bool>) (t => t.IsGenericType)).Select<Type, Type>((Func<Type, Type>) (t => t.GetGenericTypeDefinition())).Any<Type>((Func<Type, bool>) (t => (object) t == (object) typeof (System.Collections.Generic.IDictionary<,>)));
    }

    public static Type GetDictionaryType(this Type type)
    {
      return type.IsGenericType && (object) type.GetGenericTypeDefinition() == (object) typeof (System.Collections.Generic.IDictionary<,>) ? type : ((IEnumerable<Type>) type.GetInterfaces()).Where<Type>((Func<Type, bool>) (t => t.IsGenericType && (object) t.GetGenericTypeDefinition() == (object) typeof (System.Collections.Generic.IDictionary<,>))).FirstOrDefault<Type>();
    }
  }
}
