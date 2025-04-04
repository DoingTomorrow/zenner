// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ExpressionTreeVisitors.ReferenceReplacingExpressionTreeVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Parsing;
using Remotion.Linq.Utilities;
using System;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Clauses.ExpressionTreeVisitors
{
  public class ReferenceReplacingExpressionTreeVisitor : ExpressionTreeVisitor
  {
    private readonly QuerySourceMapping _querySourceMapping;
    private readonly bool _throwOnUnmappedReferences;

    public static Expression ReplaceClauseReferences(
      Expression expression,
      QuerySourceMapping querySourceMapping,
      bool throwOnUnmappedReferences)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      ArgumentUtility.CheckNotNull<QuerySourceMapping>(nameof (querySourceMapping), querySourceMapping);
      return new ReferenceReplacingExpressionTreeVisitor(querySourceMapping, throwOnUnmappedReferences).VisitExpression(expression);
    }

    protected ReferenceReplacingExpressionTreeVisitor(
      QuerySourceMapping querySourceMapping,
      bool throwOnUnmappedReferences)
    {
      ArgumentUtility.CheckNotNull<QuerySourceMapping>(nameof (querySourceMapping), querySourceMapping);
      this._querySourceMapping = querySourceMapping;
      this._throwOnUnmappedReferences = throwOnUnmappedReferences;
    }

    protected QuerySourceMapping QuerySourceMapping => this._querySourceMapping;

    protected override Expression VisitQuerySourceReferenceExpression(
      QuerySourceReferenceExpression expression)
    {
      ArgumentUtility.CheckNotNull<QuerySourceReferenceExpression>(nameof (expression), expression);
      if (this.QuerySourceMapping.ContainsMapping(expression.ReferencedQuerySource))
        return this.QuerySourceMapping.GetExpression(expression.ReferencedQuerySource);
      if (this._throwOnUnmappedReferences)
        throw new InvalidOperationException("Cannot replace reference to clause '" + expression.ReferencedQuerySource.ItemName + "', there is no mapped expression.");
      return (Expression) expression;
    }

    protected override Expression VisitSubQueryExpression(SubQueryExpression expression)
    {
      ArgumentUtility.CheckNotNull<SubQueryExpression>(nameof (expression), expression);
      expression.QueryModel.TransformExpressions((Func<Expression, Expression>) (ex => ReferenceReplacingExpressionTreeVisitor.ReplaceClauseReferences(ex, this._querySourceMapping, this._throwOnUnmappedReferences)));
      return (Expression) expression;
    }

    protected internal override Expression VisitUnknownNonExtensionExpression(Expression expression)
    {
      return expression;
    }
  }
}
