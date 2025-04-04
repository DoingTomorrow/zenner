// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.ReflectionUtility
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using Remotion.Linq1735139;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq
{
  public static class ReflectionUtility
  {
    public static MethodInfo GetMethod<T>(Expression<Func<T>> wrappedCall)
    {
      ArgumentUtility.CheckNotNull<Expression<Func<T>>>(nameof (wrappedCall), wrappedCall);
      switch (wrappedCall.Body.NodeType)
      {
        case ExpressionType.Call:
          return ((MethodCallExpression) wrappedCall.Body).Method;
        case ExpressionType.MemberAccess:
          MethodInfo getMethod = ((MemberExpression) wrappedCall.Body).Member is PropertyInfo member ? member.GetGetMethod() : (MethodInfo) null;
          if (getMethod != null)
            return getMethod;
          break;
      }
      throw new ArgumentException(string.Format("Cannot extract a method from the given expression '{0}'.", (object) wrappedCall.Body), nameof (wrappedCall));
    }

    public static Type GetItemTypeOfIEnumerable(Type enumerableType, string argumentName)
    {
      ArgumentUtility.CheckNotNull<Type>(nameof (enumerableType), enumerableType);
      ArgumentUtility.CheckNotNullOrEmpty(nameof (argumentName), argumentName);
      return ReflectionUtility.TryGetItemTypeOfIEnumerable(enumerableType) ?? throw new ArgumentTypeException(string.Format("Expected a type implementing IEnumerable<T>, but found '{0}'.", (object) enumerableType.FullName), argumentName, typeof (IEnumerable<>), enumerableType);
    }

    public static Type GetMemberReturnType(MemberInfo member)
    {
      ArgumentUtility.CheckNotNull<MemberInfo>(nameof (member), member);
      switch (member.MemberType)
      {
        case MemberTypes.Field:
          return ((FieldInfo) member).FieldType;
        case MemberTypes.Method:
          return ((MethodInfo) member).ReturnType;
        case MemberTypes.Property:
          return ((PropertyInfo) member).PropertyType;
        default:
          throw new ArgumentException("Argument must be FieldInfo, PropertyInfo, or MethodInfo.", nameof (member));
      }
    }

    public static Type TryGetItemTypeOfIEnumerable(Type possibleEnumerableType)
    {
      ArgumentUtility.CheckNotNull<Type>(nameof (possibleEnumerableType), possibleEnumerableType);
      if (possibleEnumerableType.IsArray)
        return possibleEnumerableType.GetElementType();
      Type implementedIenumerableType = ReflectionUtility.GetImplementedIEnumerableType(possibleEnumerableType);
      if (implementedIenumerableType == null)
        return (Type) null;
      return implementedIenumerableType.IsGenericType ? implementedIenumerableType.GetGenericArguments()[0] : typeof (object);
    }

    private static Type GetImplementedIEnumerableType(Type enumerableType)
    {
      // ISSUE: object of a compiler-generated type is created
      return ReflectionUtility.IsIEnumerable(enumerableType) ? enumerableType : ((IEnumerable<Type>) enumerableType.GetInterfaces()).Where<Type>((Func<Type, bool>) (i => ReflectionUtility.IsIEnumerable(i))).Select<Type, \u003C\u003Ef__AnonymousType4<Type, int>>((Func<Type, \u003C\u003Ef__AnonymousType4<Type, int>>) (i => new \u003C\u003Ef__AnonymousType4<Type, int>(i, i.IsGenericType ? i.GetGenericArguments().Length : 0))).OrderByDescending<\u003C\u003Ef__AnonymousType4<Type, int>, int>((Func<\u003C\u003Ef__AnonymousType4<Type, int>, int>) (_param0 => _param0.genericArgsCount)).Select<\u003C\u003Ef__AnonymousType4<Type, int>, Type>((Func<\u003C\u003Ef__AnonymousType4<Type, int>, Type>) (_param0 => _param0.i)).FirstOrDefault<Type>();
    }

    private static bool IsIEnumerable(Type enumerableType)
    {
      if (enumerableType == typeof (IEnumerable))
        return true;
      return enumerableType.IsGenericType && enumerableType.GetGenericTypeDefinition() == typeof (IEnumerable<>);
    }
  }
}
