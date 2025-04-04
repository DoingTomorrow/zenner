// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.ReflectionHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Linq
{
  public static class ReflectionHelper
  {
    public static MethodInfo GetMethodDefinition<TSource>(Expression<Action<TSource>> method)
    {
      MethodInfo method1 = ReflectionHelper.GetMethod<TSource>(method);
      return !method1.IsGenericMethod ? method1 : method1.GetGenericMethodDefinition();
    }

    public static MethodInfo GetMethod<TSource>(Expression<Action<TSource>> method)
    {
      if (method == null)
        throw new ArgumentNullException(nameof (method));
      return ((MethodCallExpression) method.Body).Method;
    }

    public static MethodInfo GetMethodDefinition(Expression<Action> method)
    {
      MethodInfo method1 = ReflectionHelper.GetMethod(method);
      return !method1.IsGenericMethod ? method1 : method1.GetGenericMethodDefinition();
    }

    public static MethodInfo GetMethod(Expression<Action> method)
    {
      if (method == null)
        throw new ArgumentNullException(nameof (method));
      return ((MethodCallExpression) method.Body).Method;
    }

    public static MemberInfo GetProperty<TSource, TResult>(
      Expression<Func<TSource, TResult>> property)
    {
      if (property == null)
        throw new ArgumentNullException(nameof (property));
      return ((MemberExpression) property.Body).Member;
    }

    internal static Type GetPropertyOrFieldType(this MemberInfo memberInfo)
    {
      switch (memberInfo)
      {
        case PropertyInfo propertyInfo:
          return propertyInfo.PropertyType;
        case FieldInfo fieldInfo:
          return fieldInfo.FieldType;
        default:
          return (Type) null;
      }
    }
  }
}
