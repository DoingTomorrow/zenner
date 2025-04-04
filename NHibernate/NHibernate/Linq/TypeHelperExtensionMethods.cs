// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.TypeHelperExtensionMethods
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Linq
{
  public static class TypeHelperExtensionMethods
  {
    public static void ForEach<T>(this IEnumerable<T> query, Action<T> method)
    {
      foreach (T obj in query)
        method(obj);
    }

    public static bool IsEnumerableOfT(this Type type)
    {
      return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (IEnumerable<>);
    }

    public static bool IsNullable(this Type type)
    {
      return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>);
    }

    public static bool IsNullableOrReference(this Type type)
    {
      return !type.IsValueType || type.IsNullable();
    }

    public static Type NullableOf(this Type type) => type.GetGenericArguments()[0];

    public static bool IsPrimitive(this Type type)
    {
      return type.IsValueType || type.IsNullable() || type == typeof (string);
    }

    public static bool IsNonPrimitive(this Type type) => !type.IsPrimitive();

    public static T As<T>(this object source) => (T) source;

    internal static bool IsCollectionType(this Type type)
    {
      return typeof (IEnumerable).IsAssignableFrom(type) && type != typeof (string);
    }
  }
}
