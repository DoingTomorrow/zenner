// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.PagingRewriterSelectClauseVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Visitors
{
  internal class PagingRewriterSelectClauseVisitor : NhExpressionTreeVisitor
  {
    private readonly FromClauseBase querySource;

    public PagingRewriterSelectClauseVisitor(FromClauseBase querySource)
    {
      this.querySource = querySource;
    }

    public Expression Swap(Expression expression)
    {
      return TransparentIdentifierRemovingExpressionTreeVisitor.ReplaceTransparentIdentifiers(this.VisitExpression(expression));
    }

    protected override Expression VisitQuerySourceReferenceExpression(
      QuerySourceReferenceExpression expression)
    {
      Expression querySelectorOrNull = this.GetSubQuerySelectorOrNull(expression);
      return querySelectorOrNull != null ? this.VisitExpression(querySelectorOrNull) : base.VisitQuerySourceReferenceExpression(expression);
    }

    private Expression GetSubQuerySelectorOrNull(QuerySourceReferenceExpression expression)
    {
      if (expression.ReferencedQuerySource != this.querySource)
        return (Expression) null;
      if (!(expression.ReferencedQuerySource is FromClauseBase referencedQuerySource))
        return (Expression) null;
      return !(referencedQuerySource.FromExpression is SubQueryExpression fromExpression) ? (Expression) null : fromExpression.QueryModel.SelectClause.Selector;
    }
  }
}
