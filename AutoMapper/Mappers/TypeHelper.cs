// Decompiled with JetBrains decompiler
// Type: AutoMapper.Mappers.TypeHelper
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using AutoMapper.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace AutoMapper.Mappers
{
  public static class TypeHelper
  {
    public static Type GetElementType(Type enumerableType)
    {
      return TypeHelper.GetElementType(enumerableType, (IEnumerable) null);
    }

    public static Type GetElementType(Type enumerableType, IEnumerable enumerable)
    {
      if (enumerableType.HasElementType)
        return enumerableType.GetElementType();
      if (enumerableType.IsGenericType && enumerableType.GetGenericTypeDefinition().Equals(typeof (IEnumerable<>)))
        return enumerableType.GetGenericArguments()[0];
      Type ienumerableType = TypeHelper.GetIEnumerableType(enumerableType);
      if ((object) ienumerableType != null)
        return ienumerableType.GetGenericArguments()[0];
      if (typeof (IEnumerable).IsAssignableFrom(enumerableType))
      {
        if (enumerable != null)
        {
          object obj = enumerable.Cast<object>().FirstOrDefault<object>();
          if (obj != null)
            return obj.GetType();
        }
        return typeof (object);
      }
      throw new ArgumentException(string.Format("Unable to find the element type for type '{0}'.", new object[1]
      {
        (object) enumerableType
      }), nameof (enumerableType));
    }

    public static Type GetEnumerationType(Type enumType)
    {
      if (enumType.IsNullableType())
        enumType = enumType.GetGenericArguments()[0];
      return !enumType.IsEnum ? (Type) null : enumType;
    }

    private static Type GetIEnumerableType(Type enumerableType)
    {
      try
      {
        return ((IEnumerable<Type>) enumerableType.GetInterfaces()).FirstOrDefault<Type>((Func<Type, bool>) (t => t.Name == "IEnumerable`1"));
      }
      catch (AmbiguousMatchException ex)
      {
        return (object) enumerableType.BaseType != (object) typeof (object) ? TypeHelper.GetIEnumerableType(enumerableType.BaseType) : (Type) null;
      }
    }
  }
}
