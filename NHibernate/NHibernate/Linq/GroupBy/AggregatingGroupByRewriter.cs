// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.GroupBy.AggregatingGroupByRewriter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.Clauses;
using NHibernate.Linq.Visitors;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ResultOperators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.GroupBy
{
  public class AggregatingGroupByRewriter
  {
    private static readonly ICollection<Type> AcceptableOuterResultOperators = (ICollection<Type>) new HashSet<Type>()
    {
      typeof (SkipResultOperator),
      typeof (TakeResultOperator),
      typeof (FirstResultOperator),
      typeof (SingleResultOperator),
      typeof (AnyResultOperator),
      typeof (AllResultOperator),
      typeof (TimeoutResultOperator)
    };

    private AggregatingGroupByRewriter()
    {
    }

    public static void ReWrite(QueryModel queryModel)
    {
      if (!(queryModel.MainFromClause.FromExpression is SubQueryExpression fromExpression) || fromExpression.QueryModel.ResultOperators.Count != 1 || !(fromExpression.QueryModel.ResultOperators[0] is GroupResultOperator))
        return;
      AggregatingGroupByRewriter.FlattenSubQuery(queryModel, fromExpression.QueryModel);
    }

    private static void FlattenSubQuery(QueryModel queryModel, QueryModel subQueryModel)
    {
      using (IEnumerator<ResultOperatorBase> enumerator = queryModel.ResultOperators.Where<ResultOperatorBase>((Func<ResultOperatorBase, bool>) (resultOperator => !AggregatingGroupByRewriter.AcceptableOuterResultOperators.Contains(resultOperator.GetType()))).GetEnumerator())
      {
        if (enumerator.MoveNext())
          throw new NotImplementedException("Cannot use group by with the " + enumerator.Current.GetType().Name + " result operator.");
      }
      GroupResultOperator groupBy = (GroupResultOperator) subQueryModel.ResultOperators[0];
      queryModel.ResultOperators.Insert(0, (ResultOperatorBase) groupBy);
      for (int index = 0; index < queryModel.BodyClauses.Count; ++index)
      {
        IBodyClause bodyClause = queryModel.BodyClauses[index];
        bodyClause.TransformExpressions((Func<Expression, Expression>) (s => GroupBySelectClauseRewriter.ReWrite(s, groupBy, subQueryModel)));
        if (bodyClause is WhereClause whereClause)
        {
          queryModel.BodyClauses.RemoveAt(index);
          queryModel.BodyClauses.Insert(index, (IBodyClause) new NhHavingClause(whereClause.Predicate));
        }
      }
      foreach (IBodyClause bodyClause in (Collection<IBodyClause>) subQueryModel.BodyClauses)
        queryModel.BodyClauses.Add(bodyClause);
      queryModel.SelectClause.TransformExpressions((Func<Expression, Expression>) (s => GroupBySelectClauseRewriter.ReWrite(s, groupBy, subQueryModel)));
      SwapQuerySourceVisitor querySourceVisitor = new SwapQuerySourceVisitor((IQuerySource) queryModel.MainFromClause, (IQuerySource) subQueryModel.MainFromClause);
      queryModel.TransformExpressions(new Func<Expression, Expression>(querySourceVisitor.Swap));
      queryModel.MainFromClause = subQueryModel.MainFromClause;
    }
  }
}
