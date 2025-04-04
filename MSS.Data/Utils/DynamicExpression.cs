// Decompiled with JetBrains decompiler
// Type: MSS.Data.Utils.DynamicExpression
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Utils
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
