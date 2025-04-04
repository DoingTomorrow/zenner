// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Utils.DynamicExpression
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Utils
{
  public static class DynamicExpression
  {
    public static Expression Parse(Type resultType, string expression, params object[] values)
    {
      return new ExpressionParser((ParameterExpression[]) null, expression, values).Parse(resultType);
    }

    public static LambdaExpression ParseLambda(
      Type itType,
      Type resultType,
      string expression,
      params object[] values)
    {
      return DynamicExpression.ParseLambda(new ParameterExpression[1]
      {
        Expression.Parameter(itType, "")
      }, resultType, expression, values);
    }

    public static LambdaExpression ParseLambda(
      ParameterExpression[] parameters,
      Type resultType,
      string expression,
      params object[] values)
    {
      return Expression.Lambda(new ExpressionParser(parameters, expression, values).Parse(resultType), parameters);
    }

    public static Expression<Func<T, S>> ParseLambda<T, S>(
      string expression,
      params object[] values)
    {
      return (Expression<Func<T, S>>) DynamicExpression.ParseLambda(typeof (T), typeof (S), expression, values);
    }

    public static Type CreateClass(params DynamicProperty[] properties)
    {
      return ClassFactory.Instance.GetDynamicClass((IEnumerable<DynamicProperty>) properties);
    }

    public static Type CreateClass(IEnumerable<DynamicProperty> properties)
    {
      return ClassFactory.Instance.GetDynamicClass(properties);
    }
  }
}
