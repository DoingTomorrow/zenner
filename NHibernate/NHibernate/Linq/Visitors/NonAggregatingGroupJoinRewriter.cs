// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.NonAggregatingGroupJoinRewriter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.GroupJoin;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Visitors
{
  public class NonAggregatingGroupJoinRewriter
  {
    private readonly QueryModel _model;
    private readonly IEnumerable<GroupJoinClause> _groupJoinClauses;
    private QuerySourceUsageLocator _locator;

    private NonAggregatingGroupJoinRewriter(
      QueryModel model,
      IEnumerable<GroupJoinClause> groupJoinClauses)
    {
      this._model = model;
      this._groupJoinClauses = groupJoinClauses;
    }

    public static void ReWrite(QueryModel model)
    {
      GroupJoinClause[] array = model.BodyClauses.OfType<GroupJoinClause>().ToArray<GroupJoinClause>();
      if (!((IEnumerable<GroupJoinClause>) array).Any<GroupJoinClause>())
        return;
      new NonAggregatingGroupJoinRewriter(model, (IEnumerable<GroupJoinClause>) array).ReWrite();
    }

    private void ReWrite()
    {
      foreach (GroupJoinClause aggregatingClause in this.GetGroupJoinInformation(this._groupJoinClauses).NonAggregatingClauses)
      {
        this._locator = new QuerySourceUsageLocator((IQuerySource) aggregatingClause);
        foreach (IBodyClause bodyClause in (Collection<IBodyClause>) this._model.BodyClauses)
          this._locator.Search(bodyClause);
        if (!this.IsHierarchicalJoin(aggregatingClause))
        {
          if (this.IsFlattenedJoin(aggregatingClause))
            this.ProcessFlattenedJoin(aggregatingClause);
          else if (!this.IsOuterJoin(aggregatingClause))
            throw new NotSupportedException();
        }
      }
    }

    private void ProcessFlattenedJoin(GroupJoinClause nonAggregatingJoin)
    {
      this.SwapClause((IBodyClause) nonAggregatingJoin, (IBodyClause) nonAggregatingJoin.JoinClause);
      this._model.BodyClauses.Remove(this._locator.Clauses[0]);
      this._model.SelectClause.TransformExpressions(new Func<Expression, Expression>(new SwapQuerySourceVisitor((IQuerySource) this._locator.Clauses[0], (IQuerySource) nonAggregatingJoin.JoinClause).Swap));
    }

    private void SwapClause(IBodyClause oldClause, IBodyClause newClause)
    {
      for (int index = 0; index < this._model.BodyClauses.Count; ++index)
      {
        if (this._model.BodyClauses[index] == oldClause)
        {
          this._model.BodyClauses.RemoveAt(index);
          this._model.BodyClauses.Insert(index, newClause);
        }
      }
    }

    private bool IsOuterJoin(GroupJoinClause nonAggregatingJoin) => false;

    private bool IsFlattenedJoin(GroupJoinClause nonAggregatingJoin)
    {
      return this._locator.Clauses.Count == 1 && this._locator.Clauses[0] is AdditionalFromClause;
    }

    private bool IsHierarchicalJoin(GroupJoinClause nonAggregatingJoin)
    {
      return this._locator.Clauses.Count == 0;
    }

    private IsAggregatingResults GetGroupJoinInformation(IEnumerable<GroupJoinClause> clause)
    {
      return GroupJoinAggregateDetectionVisitor.Visit(clause, this._model.SelectClause.Selector);
    }
  }
}
