// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.MaxExpressionNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;
using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  public class MaxExpressionNode(
    MethodCallExpressionParseInfo parseInfo,
    LambdaExpression optionalSelector) : ResultOperatorExpressionNodeBase(parseInfo, (LambdaExpression) null, optionalSelector)
  {
    public static readonly MethodInfo[] SupportedMethods = new MethodInfo[24]
    {
      MethodCallExpressionNodeBase.GetSupportedMethod<object>((Expression<Func<object>>) (() => default (IQueryable<object>).Max<object>())),
      MethodCallExpressionNodeBase.GetSupportedMethod<object>((Expression<Func<object>>) (() => default (IQueryable<object>).Max<object, object>(default (Expression<Func<object, object>>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<object>((Expression<Func<object>>) (() => default (IEnumerable<object>).Max<object>())),
      MethodCallExpressionNodeBase.GetSupportedMethod<object>((Expression<Func<object>>) (() => default (IEnumerable<object>).Max<object, object>(default (Func<object, object>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<Decimal>((Expression<Func<Decimal>>) (() => default (IEnumerable<Decimal>).Max())),
      MethodCallExpressionNodeBase.GetSupportedMethod<Decimal?>((Expression<Func<Decimal?>>) (() => default (IEnumerable<Decimal?>).Max())),
      MethodCallExpressionNodeBase.GetSupportedMethod<double>((Expression<Func<double>>) (() => default (IEnumerable<double>).Max())),
      MethodCallExpressionNodeBase.GetSupportedMethod<double?>((Expression<Func<double?>>) (() => default (IEnumerable<double?>).Max())),
      MethodCallExpressionNodeBase.GetSupportedMethod<int>((Expression<Func<int>>) (() => default (IEnumerable<int>).Max())),
      MethodCallExpressionNodeBase.GetSupportedMethod<int?>((Expression<Func<int?>>) (() => default (IEnumerable<int?>).Max())),
      MethodCallExpressionNodeBase.GetSupportedMethod<long>((Expression<Func<long>>) (() => default (IEnumerable<long>).Max())),
      MethodCallExpressionNodeBase.GetSupportedMethod<long?>((Expression<Func<long?>>) (() => default (IEnumerable<long?>).Max())),
      MethodCallExpressionNodeBase.GetSupportedMethod<float>((Expression<Func<float>>) (() => default (IEnumerable<float>).Max())),
      MethodCallExpressionNodeBase.GetSupportedMethod<float?>((Expression<Func<float?>>) (() => default (IEnumerable<float?>).Max())),
      MethodCallExpressionNodeBase.GetSupportedMethod<Decimal>((Expression<Func<Decimal>>) (() => default (IEnumerable<object>).Max<object>((Func<object, Decimal>) (o => 0M)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<Decimal?>((Expression<Func<Decimal?>>) (() => default (IEnumerable<object>).Max<object>((Func<object, Decimal?>) (o => (Decimal?) 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<double>((Expression<Func<double>>) (() => default (IEnumerable<object>).Max<object>((Func<object, double>) (o => 0.0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<double?>((Expression<Func<double?>>) (() => default (IEnumerable<object>).Max<object>((Func<object, double?>) (o => (double?) 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<int>((Expression<Func<int>>) (() => default (IEnumerable<object>).Max<object>((Func<object, int>) (o => 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<int?>((Expression<Func<int?>>) (() => default (IEnumerable<object>).Max<object>((Func<object, int?>) (o => (int?) 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<long>((Expression<Func<long>>) (() => default (IEnumerable<object>).Max<object>((Func<object, long>) (o => 0L)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<long?>((Expression<Func<long?>>) (() => default (IEnumerable<object>).Max<object>((Func<object, long?>) (o => (long?) 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<float>((Expression<Func<float>>) (() => default (IEnumerable<object>).Max<object>((Func<object, float>) (o => 0.0f)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<float?>((Expression<Func<float?>>) (() => default (IEnumerable<object>).Max<object>((Func<object, float?>) (o => (float?) 0))))
    };

    public override Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (inputParameter), inputParameter);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionToBeResolved), expressionToBeResolved);
      throw this.CreateResolveNotSupportedException();
    }

    protected override ResultOperatorBase CreateResultOperator(
      ClauseGenerationContext clauseGenerationContext)
    {
      return (ResultOperatorBase) new MaxResultOperator();
    }
  }
}
