// Decompiled with JetBrains decompiler
// Type: NLog.Internal.ReflectionHelpers
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NLog.Internal
{
  internal static class ReflectionHelpers
  {
    public static Type[] SafeGetTypes(this Assembly assembly)
    {
      try
      {
        return assembly.GetTypes();
      }
      catch (ReflectionTypeLoadException ex)
      {
        foreach (Exception loaderException in ex.LoaderExceptions)
          InternalLogger.Warn(loaderException, "Type load exception.");
        List<Type> typeList = new List<Type>();
        foreach (Type type in ex.Types)
        {
          if (type != (Type) null)
            typeList.Add(type);
        }
        return typeList.ToArray();
      }
      catch (Exception ex)
      {
        InternalLogger.Warn(ex, "Type load exception.");
        return ArrayHelper.Empty<Type>();
      }
    }

    public static bool IsStaticClass(this Type type)
    {
      return type.IsClass() && type.IsAbstract() && type.IsSealed();
    }

    public static ReflectionHelpers.LateBoundMethod CreateLateBoundMethod(MethodInfo methodInfo)
    {
      ParameterExpression parameterExpression = Expression.Parameter(typeof (object), "instance");
      ParameterExpression array = Expression.Parameter(typeof (object[]), "parameters");
      List<Expression> arguments = new List<Expression>();
      ParameterInfo[] parameters1 = methodInfo.GetParameters();
      for (int index = 0; index < parameters1.Length; ++index)
      {
        BinaryExpression binaryExpression = Expression.ArrayIndex((Expression) array, (Expression) Expression.Constant((object) index));
        Type type1 = parameters1[index].ParameterType;
        if (type1.IsByRef)
          type1 = type1.GetElementType();
        Type type2 = type1;
        UnaryExpression unaryExpression = Expression.Convert((Expression) binaryExpression, type2);
        arguments.Add((Expression) unaryExpression);
      }
      MethodCallExpression body = Expression.Call(methodInfo.IsStatic ? (Expression) null : (Expression) Expression.Convert((Expression) parameterExpression, methodInfo.DeclaringType), methodInfo, (IEnumerable<Expression>) arguments);
      if (body.Type == typeof (void))
      {
        Action<object, object[]> execute = Expression.Lambda<Action<object, object[]>>((Expression) body, parameterExpression, array).Compile();
        return (ReflectionHelpers.LateBoundMethod) ((instance, parameters) =>
        {
          execute(instance, parameters);
          return (object) null;
        });
      }
      return Expression.Lambda<ReflectionHelpers.LateBoundMethod>((Expression) Expression.Convert((Expression) body, typeof (object)), parameterExpression, array).Compile();
    }

    public static bool IsEnum(this Type type) => type.IsEnum;

    public static bool IsPrimitive(this Type type) => type.IsPrimitive;

    public static bool IsSealed(this Type type) => type.IsSealed;

    public static bool IsAbstract(this Type type) => type.IsAbstract;

    public static bool IsClass(this Type type) => type.IsClass;

    public static bool IsGenericType(this Type type) => type.IsGenericType;

    public static TAttr GetCustomAttribute<TAttr>(this Type type) where TAttr : Attribute
    {
      return (TAttr) Attribute.GetCustomAttribute((MemberInfo) type, typeof (TAttr));
    }

    public static TAttr GetCustomAttribute<TAttr>(this PropertyInfo info) where TAttr : Attribute
    {
      return (TAttr) Attribute.GetCustomAttribute((MemberInfo) info, typeof (TAttr));
    }

    public static TAttr GetCustomAttribute<TAttr>(this Assembly assembly) where TAttr : Attribute
    {
      return (TAttr) Attribute.GetCustomAttribute(assembly, typeof (TAttr));
    }

    public static IEnumerable<TAttr> GetCustomAttributes<TAttr>(this Type type, bool inherit) where TAttr : Attribute
    {
      return (IEnumerable<TAttr>) type.GetCustomAttributes(typeof (TAttr), inherit);
    }

    public static Assembly GetAssembly(this Type type) => type.Assembly;

    public delegate object LateBoundMethod(object target, object[] arguments);
  }
}
