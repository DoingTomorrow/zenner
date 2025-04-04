// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.AverageExpressionNode
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
  public class AverageExpressionNode(
    MethodCallExpressionParseInfo parseInfo,
    LambdaExpression optionalPredicate) : ResultOperatorExpressionNodeBase(parseInfo, (LambdaExpression) null, optionalPredicate)
  {
    public static readonly MethodInfo[] SupportedMethods = new MethodInfo[40]
    {
      MethodCallExpressionNodeBase.GetSupportedMethod<Decimal>((Expression<Func<Decimal>>) (() => Queryable.Average(default (IQueryable<Decimal>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<Decimal?>((Expression<Func<Decimal?>>) (() => Queryable.Average(default (IQueryable<Decimal?>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<double>((Expression<Func<double>>) (() => Queryable.Average(default (IQueryable<double>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<double?>((Expression<Func<double?>>) (() => Queryable.Average(default (IQueryable<double?>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<double>((Expression<Func<double>>) (() => Queryable.Average(default (IQueryable<int>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<double?>((Expression<Func<double?>>) (() => Queryable.Average(default (IQueryable<int?>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<double>((Expression<Func<double>>) (() => Queryable.Average(default (IQueryable<long>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<double?>((Expression<Func<double?>>) (() => Queryable.Average(default (IQueryable<long?>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<float>((Expression<Func<float>>) (() => Queryable.Average(default (IQueryable<float>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<float?>((Expression<Func<float?>>) (() => Queryable.Average(default (IQueryable<float?>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<Decimal>((Expression<Func<Decimal>>) (() => default (IQueryable<object>).Average<object>((Expression<Func<object, Decimal>>) (o => 0M)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<Decimal?>((Expression<Func<Decimal?>>) (() => default (IQueryable<object>).Average<object>((Expression<Func<object, Decimal?>>) (o => (Decimal?) 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<double>((Expression<Func<double>>) (() => default (IQueryable<object>).Average<object>((Expression<Func<object, double>>) (o => 0.0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<double?>((Expression<Func<double?>>) (() => default (IQueryable<object>).Average<object>((Expression<Func<object, double?>>) (o => (double?) 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<double>((Expression<Func<double>>) (() => default (IQueryable<object>).Average<object>((Expression<Func<object, int>>) (o => 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<double?>((Expression<Func<double?>>) (() => default (IQueryable<object>).Average<object>((Expression<Func<object, int?>>) (o => (int?) 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<double>((Expression<Func<double>>) (() => default (IQueryable<object>).Average<object>((Expression<Func<object, long>>) (o => 0L)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<double?>((Expression<Func<double?>>) (() => default (IQueryable<object>).Average<object>((Expression<Func<object, long?>>) (o => (long?) 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<float>((Expression<Func<float>>) (() => default (IQueryable<object>).Average<object>((Expression<Func<object, float>>) (o => 0.0f)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<float?>((Expression<Func<float?>>) (() => default (IQueryable<object>).Average<object>((Expression<Func<object, float?>>) (o => (float?) 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<Decimal>((Expression<Func<Decimal>>) (() => default (IEnumerable<Decimal>).Average())),
      MethodCallExpressionNodeBase.GetSupportedMethod<Decimal?>((Expression<Func<Decimal?>>) (() => default (IEnumerable<Decimal?>).Average())),
      MethodCallExpressionNodeBase.GetSupportedMethod<double>((Expression<Func<double>>) (() => default (IEnumerable<double>).Average())),
      MethodCallExpressionNodeBase.GetSupportedMethod<double?>((Expression<Func<double?>>) (() => default (IEnumerable<double?>).Average())),
      MethodCallExpressionNodeBase.GetSupportedMethod<double>((Expression<Func<double>>) (() => default (IEnumerable<int>).Average())),
      MethodCallExpressionNodeBase.GetSupportedMethod<double?>((Expression<Func<double?>>) (() => default (IEnumerable<int?>).Average())),
      MethodCallExpressionNodeBase.GetSupportedMethod<double>((Expression<Func<double>>) (() => default (IEnumerable<long>).Average())),
      MethodCallExpressionNodeBase.GetSupportedMethod<double?>((Expression<Func<double?>>) (() => default (IEnumerable<long?>).Average())),
      MethodCallExpressionNodeBase.GetSupportedMethod<float>((Expression<Func<float>>) (() => default (IEnumerable<float>).Average())),
      MethodCallExpressionNodeBase.GetSupportedMethod<float?>((Expression<Func<float?>>) (() => default (IEnumerable<float?>).Average())),
      MethodCallExpressionNodeBase.GetSupportedMethod<Decimal>((Expression<Func<Decimal>>) (() => default (IEnumerable<object>).Average<object>((Func<object, Decimal>) (o => 0M)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<Decimal?>((Expression<Func<Decimal?>>) (() => default (IEnumerable<object>).Average<object>((Func<object, Decimal?>) (o => (Decimal?) 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<double>((Expression<Func<double>>) (() => default (IEnumerable<object>).Average<object>((Func<object, double>) (o => 0.0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<double?>((Expression<Func<double?>>) (() => default (IEnumerable<object>).Average<object>((Func<object, double?>) (o => (double?) 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<double>((Expression<Func<double>>) (() => default (IEnumerable<object>).Average<object>((Func<object, int>) (o => 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<double?>((Expression<Func<double?>>) (() => default (IEnumerable<object>).Average<object>((Func<object, int?>) (o => (int?) 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<double>((Expression<Func<double>>) (() => default (IEnumerable<object>).Average<object>((Func<object, long>) (o => 0L)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<double?>((Expression<Func<double?>>) (() => default (IEnumerable<object>).Average<object>((Func<object, long?>) (o => (long?) 0)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<float>((Expression<Func<float>>) (() => default (IEnumerable<object>).Average<object>((Func<object, float>) (o => 0.0f)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<float?>((Expression<Func<float?>>) (() => default (IEnumerable<object>).Average<object>((Func<object, float?>) (o => (float?) 0))))
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
      return (ResultOperatorBase) new AverageResultOperator();
    }
  }
}
