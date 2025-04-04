// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.OfTypeExpressionNode
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
  public class OfTypeExpressionNode(MethodCallExpressionParseInfo parseInfo) : 
    ResultOperatorExpressionNodeBase(parseInfo, (LambdaExpression) null, (LambdaExpression) null)
  {
    public static readonly MethodInfo[] SupportedMethods = new MethodInfo[2]
    {
      MethodCallExpressionNodeBase.GetSupportedMethod<IQueryable<object>>((Expression<Func<IQueryable<object>>>) (() => Queryable.OfType<object>(default (IQueryable)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<IEnumerable<object>>((Expression<Func<IEnumerable<object>>>) (() => default (IEnumerable).OfType<object>()))
    };

    public Type SearchedItemType => this.ParsedExpression.Method.GetGenericArguments()[0];

    public override Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (inputParameter), inputParameter);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionToBeResolved), expressionToBeResolved);
      UnaryExpression replacementExpression = Expression.Convert((Expression) inputParameter, this.SearchedItemType);
      Expression expressionToBeResolved1 = ReplacingExpressionTreeVisitor.Replace((Expression) inputParameter, (Expression) replacementExpression, expressionToBeResolved);
      return this.Source.Resolve(inputParameter, expressionToBeResolved1, clauseGenerationContext);
    }

    protected override ResultOperatorBase CreateResultOperator(
      ClauseGenerationContext clauseGenerationContext)
    {
      return (ResultOperatorBase) new OfTypeResultOperator(this.SearchedItemType);
    }
  }
}
