// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.GroupJoin.GroupJoinSelectClauseRewriter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.Visitors;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.GroupJoin
{
  public class GroupJoinSelectClauseRewriter : NhExpressionTreeVisitor
  {
    private readonly IsAggregatingResults _results;

    public static Expression ReWrite(Expression expression, IsAggregatingResults results)
    {
      return new GroupJoinSelectClauseRewriter(results).VisitExpression(expression);
    }

    private GroupJoinSelectClauseRewriter(IsAggregatingResults results) => this._results = results;

    protected override Expression VisitSubQueryExpression(SubQueryExpression expression)
    {
      GroupJoinClause groupJoinClause = this.LocateGroupJoinQuerySource(expression.QueryModel);
      if (groupJoinClause != null)
      {
        Expression left = new SwapQuerySourceVisitor((IQuerySource) groupJoinClause.JoinClause, (IQuerySource) expression.QueryModel.MainFromClause).Swap(groupJoinClause.JoinClause.InnerKeySelector);
        expression.QueryModel.MainFromClause.FromExpression = groupJoinClause.JoinClause.InnerSequence;
        expression.QueryModel.BodyClauses.Add((IBodyClause) new WhereClause((Expression) Expression.Equal(left, groupJoinClause.JoinClause.OuterKeySelector)));
      }
      return (Expression) expression;
    }

    private GroupJoinClause LocateGroupJoinQuerySource(QueryModel model)
    {
      return model.BodyClauses.Count > 0 ? (GroupJoinClause) null : new NHibernate.Linq.GroupJoin.LocateGroupJoinQuerySource(this._results).Detect(model.MainFromClause.FromExpression);
    }
  }
}
