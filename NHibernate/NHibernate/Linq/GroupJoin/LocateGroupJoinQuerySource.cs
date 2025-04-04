// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.GroupJoin.LocateGroupJoinQuerySource
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.Visitors;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.GroupJoin
{
  public class LocateGroupJoinQuerySource : NhExpressionTreeVisitor
  {
    private readonly IsAggregatingResults _results;
    private GroupJoinClause _groupJoin;

    public LocateGroupJoinQuerySource(IsAggregatingResults results) => this._results = results;

    public GroupJoinClause Detect(Expression expression)
    {
      this.VisitExpression(expression);
      return this._groupJoin;
    }

    protected override Expression VisitQuerySourceReferenceExpression(
      QuerySourceReferenceExpression expression)
    {
      if (this._results.AggregatingClauses.Contains(expression.ReferencedQuerySource as GroupJoinClause))
        this._groupJoin = expression.ReferencedQuerySource as GroupJoinClause;
      return base.VisitQuerySourceReferenceExpression(expression);
    }
  }
}
