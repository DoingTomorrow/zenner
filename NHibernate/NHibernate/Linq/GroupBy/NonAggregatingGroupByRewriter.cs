// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.GroupBy.NonAggregatingGroupByRewriter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.ResultOperators;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ResultOperators;
using System;
using System.Collections.ObjectModel;
using System.Linq;

#nullable disable
namespace NHibernate.Linq.GroupBy
{
  public class NonAggregatingGroupByRewriter
  {
    private NonAggregatingGroupByRewriter()
    {
    }

    public static void ReWrite(QueryModel queryModel)
    {
      if (queryModel.ResultOperators.Count == 1 && queryModel.ResultOperators[0] is GroupResultOperator && NonAggregatingGroupByRewriter.IsNonAggregatingGroupBy(queryModel))
      {
        GroupResultOperator resultOperator = (GroupResultOperator) queryModel.ResultOperators[0];
        queryModel.ResultOperators.Clear();
        queryModel.ResultOperators.Add((ResultOperatorBase) new NonAggregatingGroupBy(resultOperator));
      }
      else
      {
        if (!(queryModel.MainFromClause.FromExpression is SubQueryExpression fromExpression) || fromExpression.QueryModel.ResultOperators.Count<ResultOperatorBase>() != 1 || !(fromExpression.QueryModel.ResultOperators[0] is GroupResultOperator) || !NonAggregatingGroupByRewriter.IsNonAggregatingGroupBy(queryModel))
          return;
        new NonAggregatingGroupByRewriter().FlattenSubQuery(fromExpression, queryModel);
      }
    }

    private void FlattenSubQuery(SubQueryExpression subQueryExpression, QueryModel queryModel)
    {
      ClientSideSelect clientSideSelect = new ClientSideSelect(new NonAggregatingGroupBySelectRewriter().Visit(queryModel.SelectClause.Selector, subQueryExpression.Type.GetGenericArguments()[0], (IQuerySource) queryModel.MainFromClause));
      queryModel.SelectClause = subQueryExpression.QueryModel.SelectClause;
      queryModel.MainFromClause = subQueryExpression.QueryModel.MainFromClause;
      foreach (IBodyClause bodyClause in (Collection<IBodyClause>) subQueryExpression.QueryModel.BodyClauses)
        queryModel.BodyClauses.Add(bodyClause);
      if (queryModel.ResultOperators.Count != 0)
        throw new NotImplementedException();
      queryModel.ResultOperators.Add((ResultOperatorBase) new NonAggregatingGroupBy((GroupResultOperator) subQueryExpression.QueryModel.ResultOperators[0]));
      queryModel.ResultOperators.Add((ResultOperatorBase) clientSideSelect);
    }

    private static bool IsNonAggregatingGroupBy(QueryModel queryModel)
    {
      return new IsNonAggregatingGroupByDetectionVisitor().IsNonAggregatingGroupBy(queryModel.SelectClause.Selector);
    }
  }
}
