// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.SumExpressionNode
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
  public class SumExpressionNode(
    MethodCallExpressionParseInfo parseInfo,
    LambdaExpression optionalSelector) : ResultOperatorExpressionNodeBase(parseInfo, (LambdaExpression) null, optionalSelector)
  {
    public static readonly MethodInfo[] SupportedMethods = new MethodInfo[40]
    {
      MethodCallExpressionNodeBase.GetSupportedMethod<Decimal>((Expression<Func<Decimal>>) (() => Queryable.Sum(default (IQueryable<Decimal>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<Decimal?>((Expression<Func<Decimal?>>) (() => Queryable.Sum(default (IQueryable<Decimal?>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<double>((Expression<Func<double>>) (() => Queryable.Sum(default (IQueryable<double>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<double?>((Expression<Func<double?>>) (() => Queryable.Sum(default (IQueryable<double?>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<int>((Expression<Func<int>>) (() => Queryable.Sum(default (IQueryable<int>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<int?>((Expression<Func<int?>>) (() => Queryable.Sum(default (IQueryable<int?>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<long>((Expression<Func<long>>) (() => Queryable.Sum(default (IQueryable<long>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<long?>((Expression<Func<long?>>) (() => Queryable.Sum(default (IQueryable<long?>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<float>((Expression<Func<float>>) (() => Queryable.Sum(default (IQueryable<float>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<float?>((Expression<Func<float?>>) (() => Queryable.Sum(default (IQueryable<float?>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<Decimal>((Expression<Func<Decimal>>) (() => default (IQueryable<object>).Sum<object>((Expression<Func<object, Decimal>>) (o => 0M)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<Decimal?>((Expression<Func<Decimal?>>) (() => default (IQueryable<object>).Sum<object>((Expression<Func<object, Decimal?>>) (o => (Decimal?) 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<double>((Expression<Func<double>>) (() => default (IQueryable<object>).Sum<object>((Expression<Func<object, double>>) (o => 0.0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<double?>((Expression<Func<double?>>) (() => default (IQueryable<object>).Sum<object>((Expression<Func<object, double?>>) (o => (double?) 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<int>((Expression<Func<int>>) (() => default (IQueryable<object>).Sum<object>((Expression<Func<object, int>>) (o => 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<int?>((Expression<Func<int?>>) (() => default (IQueryable<object>).Sum<object>((Expression<Func<object, int?>>) (o => (int?) 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<long>((Expression<Func<long>>) (() => default (IQueryable<object>).Sum<object>((Expression<Func<object, long>>) (o => 0L)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<long?>((Expression<Func<long?>>) (() => default (IQueryable<object>).Sum<object>((Expression<Func<object, long?>>) (o => (long?) 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<float>((Expression<Func<float>>) (() => default (IQueryable<object>).Sum<object>((Expression<Func<object, float>>) (o => 0.0f)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<float?>((Expression<Func<float?>>) (() => default (IQueryable<object>).Sum<object>((Expression<Func<object, float?>>) (o => (float?) 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<Decimal>((Expression<Func<Decimal>>) (() => default (IEnumerable<Decimal>).Sum())),
      MethodCallExpressionNodeBase.GetSupportedMethod<Decimal?>((Expression<Func<Decimal?>>) (() => default (IEnumerable<Decimal?>).Sum())),
      MethodCallExpressionNodeBase.GetSupportedMethod<double>((Expression<Func<double>>) (() => default (IEnumerable<double>).Sum())),
      MethodCallExpressionNodeBase.GetSupportedMethod<double?>((Expression<Func<double?>>) (() => default (IEnumerable<double?>).Sum())),
      MethodCallExpressionNodeBase.GetSupportedMethod<int>((Expression<Func<int>>) (() => default (IEnumerable<int>).Sum())),
      MethodCallExpressionNodeBase.GetSupportedMethod<int?>((Expression<Func<int?>>) (() => default (IEnumerable<int?>).Sum())),
      MethodCallExpressionNodeBase.GetSupportedMethod<long>((Expression<Func<long>>) (() => default (IEnumerable<long>).Sum())),
      MethodCallExpressionNodeBase.GetSupportedMethod<long?>((Expression<Func<long?>>) (() => default (IEnumerable<long?>).Sum())),
      MethodCallExpressionNodeBase.GetSupportedMethod<float>((Expression<Func<float>>) (() => default (IEnumerable<float>).Sum())),
      MethodCallExpressionNodeBase.GetSupportedMethod<float?>((Expression<Func<float?>>) (() => default (IEnumerable<float?>).Sum())),
      MethodCallExpressionNodeBase.GetSupportedMethod<Decimal>((Expression<Func<Decimal>>) (() => default (IEnumerable<object>).Sum<object>((Func<object, Decimal>) (o => 0M)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<Decimal?>((Expression<Func<Decimal?>>) (() => default (IEnumerable<object>).Sum<object>((Func<object, Decimal?>) (o => (Decimal?) 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<double>((Expression<Func<double>>) (() => default (IEnumerable<object>).Sum<object>((Func<object, double>) (o => 0.0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<double?>((Expression<Func<double?>>) (() => default (IEnumerable<object>).Sum<object>((Func<object, double?>) (o => (double?) 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<int>((Expression<Func<int>>) (() => default (IEnumerable<object>).Sum<object>((Func<object, int>) (o => 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<int?>((Expression<Func<int?>>) (() => default (IEnumerable<object>).Sum<object>((Func<object, int?>) (o => (int?) 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<long>((Expression<Func<long>>) (() => default (IEnumerable<object>).Sum<object>((Func<object, long>) (o => 0L)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<long?>((Expression<Func<long?>>) (() => default (IEnumerable<object>).Sum<object>((Func<object, long?>) (o => (long?) 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<float>((Expression<Func<float>>) (() => default (IEnumerable<object>).Sum<object>((Func<object, float>) (o => 0.0f)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<float?>((Expression<Func<float?>>) (() => default (IEnumerable<object>).Sum<object>((Func<object, float?>) (o => (float?) 0))))
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
      return (ResultOperatorBase) new SumResultOperator();
    }
  }
}
