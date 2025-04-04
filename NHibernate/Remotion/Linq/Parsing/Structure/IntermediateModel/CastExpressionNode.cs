// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.CastExpressionNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Remotion.Linq.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  public class CastExpressionNode : ResultOperatorExpressionNodeBase
  {
    public static readonly MethodInfo[] SupportedMethods = new MethodInfo[2]
    {
      MethodCallExpressionNodeBase.GetSupportedMethod<IQueryable<object>>((Expression<Func<IQueryable<object>>>) (() => Queryable.Cast<object>(default (IQueryable)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<IEnumerable<object>>((Expression<Func<IEnumerable<object>>>) (() => default (IEnumerable).Cast<object>()))
    };

    public CastExpressionNode(MethodCallExpressionParseInfo parseInfo)
      : base(parseInfo, (LambdaExpression) null, (LambdaExpression) null)
    {
      if (!parseInfo.ParsedExpression.Method.IsGenericMethod || parseInfo.ParsedExpression.Method.GetGenericArguments().Length != 1)
        throw new ArgumentException("The parsed method must have exactly one generic argument.", nameof (parseInfo));
    }

    public Type CastItemType => this.ParsedExpression.Method.GetGenericArguments()[0];

    public override Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (inputParameter), inputParameter);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionToBeResolved), expressionToBeResolved);
      UnaryExpression replacementExpression = Expression.Convert((Expression) inputParameter, this.CastItemType);
      Expression expressionToBeResolved1 = ReplacingExpressionTreeVisitor.Replace((Expression) inputParameter, (Expression) replacementExpression, expressionToBeResolved);
      return this.Source.Resolve(inputParameter, expressionToBeResolved1, clauseGenerationContext);
    }

    protected override ResultOperatorBase CreateResultOperator(
      ClauseGenerationContext clauseGenerationContext)
    {
      return (ResultOperatorBase) new CastResultOperator(this.CastItemType);
    }
  }
}
