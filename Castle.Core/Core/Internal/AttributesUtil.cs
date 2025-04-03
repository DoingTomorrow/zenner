// Decompiled with JetBrains decompiler
// Type: Castle.Core.Internal.AttributesUtil
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Castle.Core.Internal
{
  public static class AttributesUtil
  {
    public static T GetAttribute<T>(this ICustomAttributeProvider member) where T : class
    {
      return ((IEnumerable<T>) member.GetAttributes<T>()).FirstOrDefault<T>();
    }

    public static T[] GetAttributes<T>(this ICustomAttributeProvider member) where T : class
    {
      return typeof (T) != typeof (object) ? (T[]) member.GetCustomAttributes(typeof (T), false) : (T[]) member.GetCustomAttributes(false);
    }

    public static T GetTypeAttribute<T>(this Type type) where T : class
    {
      T typeAttribute = type.GetAttribute<T>();
      if ((object) typeAttribute == null)
      {
        foreach (Type type1 in type.GetInterfaces())
        {
          typeAttribute = type1.GetTypeAttribute<T>();
          if ((object) typeAttribute != null)
            break;
        }
      }
      return typeAttribute;
    }

    public static T[] GetTypeAttributes<T>(Type type) where T : class
    {
      T[] typeAttributes = type.GetAttributes<T>();
      if (typeAttributes.Length == 0)
      {
        foreach (Type type1 in type.GetInterfaces())
        {
          typeAttributes = AttributesUtil.GetTypeAttributes<T>(type1);
          if (typeAttributes.Length > 0)
            break;
        }
      }
      return typeAttributes;
    }

    public static Type GetTypeConverter(MemberInfo member)
    {
      TypeConverterAttribute attribute = member.GetAttribute<TypeConverterAttribute>();
      return attribute != null ? Type.GetType(attribute.ConverterTypeName) : (Type) null;
    }

    public static bool HasAttribute<T>(this ICustomAttributeProvider member) where T : class
    {
      return (object) ((IEnumerable<T>) member.GetAttributes<T>()).FirstOrDefault<T>() != null;
    }
  }
}
