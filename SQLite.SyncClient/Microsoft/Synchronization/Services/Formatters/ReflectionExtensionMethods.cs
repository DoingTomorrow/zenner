// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.Services.Formatters.ReflectionExtensionMethods
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace Microsoft.Synchronization.Services.Formatters
{
  public static class ReflectionExtensionMethods
  {
    public static PropertyInfo GetProperty(this Type type, string propertyName)
    {
      return type.GetTypeInfo().GetDeclaredProperty(propertyName);
    }

    public static IEnumerable<PropertyInfo> GetProperties(this Type type)
    {
      return type.GetTypeInfo().DeclaredProperties;
    }

    public static MethodInfo GetMethod(this Type type, string methodName)
    {
      return type.GetTypeInfo().GetDeclaredMethod(methodName);
    }

    public static bool IsSubclassOf(this Type type, Type parentType)
    {
      return type.GetTypeInfo().IsSubclassOf(parentType);
    }

    public static bool IsAssignableFrom(this Type type, Type parentType)
    {
      return type.GetTypeInfo().IsAssignableFrom(parentType.GetTypeInfo());
    }

    public static bool IsEnum(this Type type) => type.GetTypeInfo().IsEnum;

    public static bool IsPrimitive(this Type type) => type.GetTypeInfo().IsPrimitive;

    public static Type GetBaseType(this Type type) => type.GetTypeInfo().BaseType;

    public static bool IsGenericType(this Type type) => type.GetTypeInfo().IsGenericType;

    public static Type[] GetGenericArguments(this Type type)
    {
      return type.GetTypeInfo().GenericTypeArguments;
    }

    public static object GetPropertyValue(this object instance, string propertyValue)
    {
      return instance.GetType().GetTypeInfo().GetDeclaredProperty(propertyValue).GetValue(instance);
    }

    public static TypeInfo GetTypeInfo(this Type type) => ((IReflectableType) type).GetTypeInfo();
  }
}
