// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionTreeVisitors.MultiReplacingExpressionTreeVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Parsing.ExpressionTreeVisitors
{
  public class MultiReplacingExpressionTreeVisitor : ExpressionTreeVisitor
  {
    private readonly IDictionary<Expression, Expression> _expressionMapping;

    public static Expression Replace(
      IDictionary<Expression, Expression> expressionMapping,
      Expression sourceTree)
    {
      ArgumentUtility.CheckNotNull<IDictionary<Expression, Expression>>(nameof (expressionMapping), expressionMapping);
      ArgumentUtility.CheckNotNull<Expression>(nameof (sourceTree), sourceTree);
      return new MultiReplacingExpressionTreeVisitor(expressionMapping).VisitExpression(sourceTree);
    }

    private MultiReplacingExpressionTreeVisitor(
      IDictionary<Expression, Expression> expressionMapping)
    {
      ArgumentUtility.CheckNotNull<IDictionary<Expression, Expression>>(nameof (expressionMapping), expressionMapping);
      this._expressionMapping = expressionMapping;
    }

    public override Expression VisitExpression(Expression expression)
    {
      Expression expression1;
      return expression != null && this._expressionMapping.TryGetValue(expression, out expression1) ? expression1 : base.VisitExpression(expression);
    }

    protected override Expression VisitSubQueryExpression(SubQueryExpression expression)
    {
      expression.QueryModel.TransformExpressions(new Func<Expression, Expression>(((ExpressionTreeVisitor) this).VisitExpression));
      return (Expression) expression;
    }

    protected internal override Expression VisitUnknownNonExtensionExpression(Expression expression)
    {
      return expression;
    }
  }
}
