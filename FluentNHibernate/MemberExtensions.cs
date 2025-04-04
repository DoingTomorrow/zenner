// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MemberExtensions
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace FluentNHibernate
{
  public static class MemberExtensions
  {
    public static Member ToMember(this PropertyInfo propertyInfo)
    {
      return propertyInfo != null ? (Member) new PropertyMember(propertyInfo) : throw new NullReferenceException("Cannot create member from null.");
    }

    public static Member ToMember(this MethodInfo methodInfo)
    {
      return methodInfo != null ? (Member) new MethodMember(methodInfo) : throw new NullReferenceException("Cannot create member from null.");
    }

    public static Member ToMember(this FieldInfo fieldInfo)
    {
      return fieldInfo != null ? (Member) new FieldMember(fieldInfo) : throw new NullReferenceException("Cannot create member from null.");
    }

    public static Member ToMember(this MemberInfo memberInfo)
    {
      switch (memberInfo)
      {
        case null:
          throw new NullReferenceException("Cannot create member from null.");
        case PropertyInfo _:
          return MemberExtensions.ToMember((PropertyInfo) memberInfo);
        case FieldInfo _:
          return MemberExtensions.ToMember((FieldInfo) memberInfo);
        case MethodInfo _:
          return MemberExtensions.ToMember((MethodInfo) memberInfo);
        default:
          throw new InvalidOperationException("Cannot convert MemberInfo '" + memberInfo.Name + "' to Member.");
      }
    }

    public static IEnumerable<Member> GetInstanceFields(this Type type)
    {
      foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        if (!field.Name.StartsWith("<"))
          yield return MemberExtensions.ToMember(field);
      }
    }

    public static IEnumerable<Member> GetInstanceMethods(this Type type)
    {
      foreach (MethodInfo method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        if (!method.Name.StartsWith("get_") && !method.Name.StartsWith("set_") && method.ReturnType != typeof (void) && method.GetParameters().Length == 0)
          yield return MemberExtensions.ToMember(method);
      }
    }

    public static IEnumerable<Member> GetInstanceProperties(this Type type)
    {
      foreach (PropertyInfo property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        yield return MemberExtensions.ToMember(property);
    }

    public static IEnumerable<Member> GetInstanceMembers(this Type type)
    {
      HashSet<Member> members = new HashSet<Member>((IEqualityComparer<Member>) new MemberEqualityComparer());
      type.GetInstanceProperties().Each<Member>((Action<Member>) (x => members.Add(x)));
      type.GetInstanceFields().Each<Member>((Action<Member>) (x => members.Add(x)));
      type.GetInstanceMethods().Each<Member>((Action<Member>) (x => members.Add(x)));
      if (type.BaseType != null && type.BaseType != typeof (object))
        type.BaseType.GetInstanceMembers().Each<Member>((Action<Member>) (x => members.Add(x)));
      return (IEnumerable<Member>) members;
    }
  }
}
