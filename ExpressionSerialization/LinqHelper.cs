// Decompiled with JetBrains decompiler
// Type: ExpressionSerialization.LinqHelper
// Assembly: ExpressionSerialization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D52D7A-23AF-4AE6-9DD2-C2DCB4AD474C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ExpressionSerialization.dll

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace ExpressionSerialization
{
  public static class LinqHelper
  {
    public static IQueryable WhereCall(
      LambdaExpression wherePredicate,
      IEnumerable sourceCollection,
      Type elementType)
    {
      IQueryable queryable = LinqHelper.CastToGenericEnumerable(sourceCollection, elementType).AsQueryable();
      MethodCallExpression methodCallExpression = Expression.Call(typeof (Queryable), "Where", new Type[1]
      {
        elementType
      }, queryable.Expression, (Expression) wherePredicate);
      return queryable.Provider.CreateQuery((Expression) methodCallExpression);
    }

    public static IEnumerable CastToGenericEnumerable(IEnumerable sourceobjects, Type TSubclass)
    {
      IQueryable queryable = sourceobjects.AsQueryable();
      object obj = Expression.Lambda((Expression) Expression.Call(typeof (Queryable), "Cast", new Type[1]
      {
        TSubclass
      }, (Expression) Expression.Constant((object) queryable)), Expression.Parameter(typeof (IEnumerable))).Compile().DynamicInvoke((object) queryable);
      // ISSUE: reference to a compiler-generated field
      if (LinqHelper.\u003C\u003Eo__1.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LinqHelper.\u003C\u003Eo__1.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (LinqHelper)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return LinqHelper.\u003C\u003Eo__1.\u003C\u003Ep__0.Target((CallSite) LinqHelper.\u003C\u003Eo__1.\u003C\u003Ep__0, obj);
    }

    public static IList CastToGenericList(IEnumerable sourceobjects, Type elementType)
    {
      object instance = Activator.CreateInstance(typeof (List<>).MakeGenericType(elementType));
      object genericEnumerable = (object) LinqHelper.CastToGenericEnumerable(sourceobjects, elementType);
      // ISSUE: reference to a compiler-generated field
      if (LinqHelper.\u003C\u003Eo__2.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LinqHelper.\u003C\u003Eo__2.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (LinqHelper)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      foreach (object obj in LinqHelper.\u003C\u003Eo__2.\u003C\u003Ep__1.Target((CallSite) LinqHelper.\u003C\u003Eo__2.\u003C\u003Ep__1, genericEnumerable))
      {
        // ISSUE: reference to a compiler-generated field
        if (LinqHelper.\u003C\u003Eo__2.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          LinqHelper.\u003C\u003Eo__2.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<Type>) null, typeof (LinqHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        LinqHelper.\u003C\u003Eo__2.\u003C\u003Ep__0.Target((CallSite) LinqHelper.\u003C\u003Eo__2.\u003C\u003Ep__0, instance, obj);
      }
      // ISSUE: reference to a compiler-generated field
      if (LinqHelper.\u003C\u003Eo__2.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LinqHelper.\u003C\u003Eo__2.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, IList>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof (IList), typeof (LinqHelper)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return LinqHelper.\u003C\u003Eo__2.\u003C\u003Ep__2.Target((CallSite) LinqHelper.\u003C\u003Eo__2.\u003C\u003Ep__2, instance);
    }

    public static IEnumerable<TElement> WhereCall<TElement>(
      LambdaExpression wherePredicate,
      IEnumerable<TElement> sourceCollection = null)
    {
      IQueryable<TElement> queryable = sourceCollection.AsQueryable<TElement>();
      MethodCallExpression methodCallExpression = Expression.Call(typeof (Queryable), "Where", new Type[1]
      {
        queryable.ElementType
      }, queryable.Expression, (Expression) wherePredicate);
      return (IEnumerable<TElement>) queryable.Provider.CreateQuery<TElement>((Expression) methodCallExpression).ToArray<TElement>();
    }

    public static Expression<Func<T, TResult>> FuncToExpression<T, TResult>(
      Expression<Func<T, TResult>> func)
    {
      return func;
    }

    public static Expression<Func<TResult>> FuncToExpression<TResult>(Expression<Func<TResult>> func)
    {
      return func;
    }

    public static MemberExpression GetMemberAccess<T, TResult>(Expression<Func<T, TResult>> expr)
    {
      return (MemberExpression) expr.Body;
    }

    public static MemberExpression GetMemberAccess<T>(Expression<Func<T>> expr)
    {
      return (MemberExpression) expr.Body;
    }

    public static MethodCallExpression GetMethodCallExpression<T, TResult>(
      Expression<Func<T, TResult>> expr)
    {
      return (MethodCallExpression) expr.Body;
    }

    public static TResult Execute<TResult>(Expression expression)
    {
      return ((IEnumerable<TResult>) new TResult[0]).AsEnumerable<TResult>().AsQueryable<TResult>().Provider.Execute<TResult>(expression);
    }

    public static D RunTimeConvert<D, S>(S src, Type convertExtension) where S : new()
    {
      // ISSUE: reference to a compiler-generated field
      if (LinqHelper.\u003C\u003Eo__10<D, S>.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        LinqHelper.\u003C\u003Eo__10<D, S>.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, D>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (D), typeof (LinqHelper)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return LinqHelper.\u003C\u003Eo__10<D, S>.\u003C\u003Ep__0.Target((CallSite) LinqHelper.\u003C\u003Eo__10<D, S>.\u003C\u003Ep__0, LinqHelper.RunTimeConvert((object) src, convertExtension));
    }

    public static object RunTimeConvert(object instance, Type convertExtension)
    {
      Type srcType = instance.GetType();
      return Expression.Lambda((Expression) Expression.Call(((IEnumerable<MethodInfo>) convertExtension.GetMethods()).Select(m => new
      {
        m = m,
        parameters = m.GetParameters()
      }).Where(_param1 => _param1.m.Name == "Convert" && ((IEnumerable<ParameterInfo>) _param1.parameters).Any<ParameterInfo>((Func<ParameterInfo, bool>) (p => p.ParameterType == srcType))).Select(_param1 => _param1.m).First<MethodInfo>(), (Expression) Expression.Constant(instance)), Expression.Parameter(srcType)).Compile().DynamicInvoke(instance);
    }

    public static object CreateInstance(this Type type)
    {
      return Expression.Lambda((Expression) Expression.New(((IEnumerable<ConstructorInfo>) type.GetConstructors()).First<ConstructorInfo>((Func<ConstructorInfo, bool>) (c => ((IEnumerable<ParameterInfo>) c.GetParameters()).Count<ParameterInfo>() == 0)))).Compile().DynamicInvoke();
    }
  }
}
