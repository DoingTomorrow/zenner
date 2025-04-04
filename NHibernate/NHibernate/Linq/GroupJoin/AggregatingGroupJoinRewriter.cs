// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.GroupJoin.AggregatingGroupJoinRewriter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq;
using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.GroupJoin
{
  public class AggregatingGroupJoinRewriter
  {
    private AggregatingGroupJoinRewriter()
    {
    }

    public static void ReWrite(QueryModel model)
    {
      IEnumerable<GroupJoinClause> groupJoinClauses = model.BodyClauses.Where<IBodyClause>((Func<IBodyClause, bool>) (bc => bc is GroupJoinClause)).Cast<GroupJoinClause>();
      if (groupJoinClauses.Count<GroupJoinClause>() == 0)
        return;
      IsAggregatingResults aggregateDetectorResults = AggregatingGroupJoinRewriter.IsAggregatingGroupJoin(model, groupJoinClauses);
      if (aggregateDetectorResults.AggregatingClauses.Count <= 0)
        return;
      model.SelectClause.TransformExpressions((Func<Expression, Expression>) (s => GroupJoinSelectClauseRewriter.ReWrite(s, aggregateDetectorResults)));
      foreach (GroupJoinClause aggregatingClause in aggregateDetectorResults.AggregatingClauses)
        model.BodyClauses.Remove((IBodyClause) aggregatingClause);
    }

    private static IsAggregatingResults IsAggregatingGroupJoin(
      QueryModel model,
      IEnumerable<GroupJoinClause> clause)
    {
      return GroupJoinAggregateDetectionVisitor.Visit(clause, model.SelectClause.Selector);
    }
  }
}
