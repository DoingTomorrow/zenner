// Decompiled with JetBrains decompiler
// Type: AutoMapper.DelegateFactory
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using AutoMapper.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace AutoMapper
{
  public class DelegateFactory : IDelegateFactory
  {
    private static readonly IDictionaryFactory DictionaryFactory = PlatformAdapter.Resolve<IDictionaryFactory>();
    private static readonly AutoMapper.Internal.IDictionary<Type, LateBoundCtor> _ctorCache = DelegateFactory.DictionaryFactory.CreateDictionary<Type, LateBoundCtor>();

    public LateBoundMethod CreateGet(MethodInfo method)
    {
      ParameterExpression instanceParameter;
      ParameterExpression argumentsParameter;
      return Expression.Lambda<LateBoundMethod>((Expression) Expression.Convert(method.IsDefined(typeof (ExtensionAttribute), false) ? (Expression) Expression.Call(method, DelegateFactory.CreateParameterExpressions(method, (Expression) instanceParameter, (Expression) argumentsParameter)) : (Expression) Expression.Call((Expression) Expression.Convert((Expression) instanceParameter, method.DeclaringType), method, DelegateFactory.CreateParameterExpressions(method, (Expression) instanceParameter, (Expression) argumentsParameter)), typeof (object)), instanceParameter, argumentsParameter).Compile();
    }

    public LateBoundPropertyGet CreateGet(PropertyInfo property)
    {
      ParameterExpression parameterExpression;
      return Expression.Lambda<LateBoundPropertyGet>((Expression) Expression.Convert((Expression) Expression.Property((Expression) Expression.Convert((Expression) parameterExpression, property.DeclaringType), property), typeof (object)), parameterExpression).Compile();
    }

    public LateBoundFieldGet CreateGet(FieldInfo field)
    {
      ParameterExpression parameterExpression;
      return Expression.Lambda<LateBoundFieldGet>((Expression) Expression.Convert((Expression) Expression.Field((Expression) Expression.Convert((Expression) parameterExpression, field.DeclaringType), field), typeof (object)), parameterExpression).Compile();
    }

    public virtual LateBoundFieldSet CreateSet(FieldInfo field)
    {
      return (LateBoundFieldSet) ((target, value) => field.SetValue(target, value));
    }

    public virtual LateBoundPropertySet CreateSet(PropertyInfo property)
    {
      return (LateBoundPropertySet) ((target, value) => property.SetValue(target, value, (object[]) null));
    }

    public LateBoundCtor CreateCtor(Type type)
    {
      return DelegateFactory._ctorCache.GetOrAdd(type, (Func<Type, LateBoundCtor>) (t => ((Expression<LateBoundCtor>) (() => (object) Expression.New(type))).Compile()));
    }

    private static Expression[] CreateParameterExpressions(
      MethodInfo method,
      Expression instanceParameter,
      Expression argumentsParameter)
    {
      List<UnaryExpression> unaryExpressionList = new List<UnaryExpression>();
      ParameterInfo[] source = method.GetParameters();
      if (method.IsDefined(typeof (ExtensionAttribute), false))
      {
        Type parameterType = method.GetParameters()[0].ParameterType;
        unaryExpressionList.Add(Expression.Convert(instanceParameter, parameterType));
        source = ((IEnumerable<ParameterInfo>) source).Skip<ParameterInfo>(1).ToArray<ParameterInfo>();
      }
      unaryExpressionList.AddRange(((IEnumerable<ParameterInfo>) source).Select<ParameterInfo, UnaryExpression>((Func<ParameterInfo, int, UnaryExpression>) ((parameter, index) => Expression.Convert((Expression) Expression.ArrayIndex(argumentsParameter, (Expression) Expression.Constant((object) index)), parameter.ParameterType))));
      return (Expression[]) unaryExpressionList.ToArray();
    }

    public LateBoundParamsCtor CreateCtor(
      ConstructorInfo constructorInfo,
      IEnumerable<ConstructorParameterMap> ctorParams)
    {
      ParameterExpression paramsExpr = Expression.Parameter(typeof (object[]), "parameters");
      UnaryExpression[] array = ctorParams.Select<ConstructorParameterMap, UnaryExpression>((Func<ConstructorParameterMap, int, UnaryExpression>) ((ctorParam, i) => Expression.Convert((Expression) Expression.ArrayIndex((Expression) paramsExpr, (Expression) Expression.Constant((object) i)), ctorParam.Parameter.ParameterType))).ToArray<UnaryExpression>();
      return Expression.Lambda<LateBoundParamsCtor>((Expression) Expression.New(constructorInfo, (Expression[]) array), paramsExpr).Compile();
    }
  }
}
