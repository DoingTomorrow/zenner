// Decompiled with JetBrains decompiler
// Type: ExpressionSerialization.QueryCreator
// Assembly: ExpressionSerialization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D52D7A-23AF-4AE6-9DD2-C2DCB4AD474C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ExpressionSerialization.dll

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace ExpressionSerialization
{
  public class QueryCreator
  {
    private Func<Type, object> fnGetObjects;

    public QueryCreator()
      : this(new Func<Type, object>(QueryCreator.GetIEnumerableOf))
    {
    }

    public QueryCreator(Func<Type, object> fngetobjects) => this.fnGetObjects = fngetobjects;

    public object CreateQuery(Type elementType)
    {
      object obj1 = this.fnGetObjects(elementType);
      // ISSUE: reference to a compiler-generated field
      if (QueryCreator.\u003C\u003Eo__3.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        QueryCreator.\u003C\u003Eo__3.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, Type>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof (Type), typeof (QueryCreator)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Type> target1 = QueryCreator.\u003C\u003Eo__3.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Type>> p1 = QueryCreator.\u003C\u003Eo__3.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (QueryCreator.\u003C\u003Eo__3.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        QueryCreator.\u003C\u003Eo__3.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "GetType", (IEnumerable<Type>) null, typeof (QueryCreator), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = QueryCreator.\u003C\u003Eo__3.\u003C\u003Ep__0.Target((CallSite) QueryCreator.\u003C\u003Eo__3.\u003C\u003Ep__0, obj1);
      Type c = target1((CallSite) p1, obj2);
      if (!typeof (IEnumerable<>).MakeGenericType(elementType).IsAssignableFrom(c))
      {
        // ISSUE: reference to a compiler-generated field
        if (QueryCreator.\u003C\u003Eo__3.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          QueryCreator.\u003C\u003Eo__3.\u003C\u003Ep__3 = CallSite<Func<CallSite, Type, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToArray", (IEnumerable<Type>) null, typeof (QueryCreator), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target2 = QueryCreator.\u003C\u003Eo__3.\u003C\u003Ep__3.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p3 = QueryCreator.\u003C\u003Eo__3.\u003C\u003Ep__3;
        Type type = typeof (Enumerable);
        // ISSUE: reference to a compiler-generated field
        if (QueryCreator.\u003C\u003Eo__3.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          QueryCreator.\u003C\u003Eo__3.\u003C\u003Ep__2 = CallSite<Func<CallSite, Type, object, Type, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "CastToGenericEnumerable", (IEnumerable<Type>) null, typeof (QueryCreator), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = QueryCreator.\u003C\u003Eo__3.\u003C\u003Ep__2.Target((CallSite) QueryCreator.\u003C\u003Eo__3.\u003C\u003Ep__2, typeof (LinqHelper), obj1, elementType);
        obj1 = target2((CallSite) p3, type, obj3);
      }
      // ISSUE: reference to a compiler-generated field
      if (QueryCreator.\u003C\u003Eo__3.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        QueryCreator.\u003C\u003Eo__3.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, IQueryable>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof (IQueryable), typeof (QueryCreator)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, IQueryable> target3 = QueryCreator.\u003C\u003Eo__3.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, IQueryable>> p5 = QueryCreator.\u003C\u003Eo__3.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (QueryCreator.\u003C\u003Eo__3.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        QueryCreator.\u003C\u003Eo__3.\u003C\u003Ep__4 = CallSite<Func<CallSite, Type, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "AsQueryable", (IEnumerable<Type>) null, typeof (QueryCreator), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = QueryCreator.\u003C\u003Eo__3.\u003C\u003Ep__4.Target((CallSite) QueryCreator.\u003C\u003Eo__3.\u003C\u003Ep__4, typeof (Queryable), obj1);
      IQueryable queryable = target3((CallSite) p5, obj4);
      IQueryProvider provider = queryable.Provider;
      ConstructorInfo constructor = typeof (Query<>).MakeGenericType(elementType).GetConstructors()[2];
      ParameterExpression[] parameterExpressionArray = new ParameterExpression[2]
      {
        Expression.Parameter(typeof (IQueryProvider)),
        Expression.Parameter(typeof (Expression))
      };
      return Expression.Lambda((Expression) Expression.New(constructor, (Expression[]) parameterExpressionArray), parameterExpressionArray).Compile().DynamicInvoke((object) provider, (object) Expression.Constant((object) queryable));
    }

    internal static object GetIEnumerableOf(Type elementType)
    {
      object defaultInstance1 = QueryCreator.CreateDefaultInstance(typeof (List<>).MakeGenericType(elementType));
      for (int index = 0; index < 10; ++index)
      {
        object defaultInstance2 = QueryCreator.CreateDefaultInstance(elementType);
        // ISSUE: reference to a compiler-generated field
        if (QueryCreator.\u003C\u003Eo__4.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          QueryCreator.\u003C\u003Eo__4.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<Type>) null, typeof (QueryCreator), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        QueryCreator.\u003C\u003Eo__4.\u003C\u003Ep__0.Target((CallSite) QueryCreator.\u003C\u003Eo__4.\u003C\u003Ep__0, defaultInstance1, defaultInstance2);
      }
      return defaultInstance1;
    }

    internal static object CreateDefaultInstance(Type type)
    {
      ConstructorInfo constructor = ((IEnumerable<ConstructorInfo>) type.GetConstructors()).FirstOrDefault<ConstructorInfo>((Func<ConstructorInfo, bool>) (c => ((IEnumerable<ParameterInfo>) c.GetParameters()).Count<ParameterInfo>() == 0));
      return !(constructor == (ConstructorInfo) null) ? Expression.Lambda((Expression) Expression.New(constructor)).Compile().DynamicInvoke() : throw new ArgumentException(string.Format("The type {0} must have a default (parameterless) constructor!", (object) type));
    }
  }
}
