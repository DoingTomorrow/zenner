// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.GroupBy.PagingRewriter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

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
  internal static class PagingRewriter
  {
    private static readonly Type[] PagingResultOperators = new Type[2]
    {
      typeof (SkipResultOperator),
      typeof (TakeResultOperator)
    };

    public static void ReWrite(QueryModel queryModel)
    {
      if (!(queryModel.MainFromClause.FromExpression is SubQueryExpression fromExpression) || !fromExpression.QueryModel.ResultOperators.All<ResultOperatorBase>((Func<ResultOperatorBase, bool>) (x => ((IEnumerable<Type>) PagingRewriter.PagingResultOperators).Contains<Type>(x.GetType()))))
        return;
      PagingRewriter.FlattenSubQuery(fromExpression, queryModel);
    }

    private static void FlattenSubQuery(
      SubQueryExpression subQueryExpression,
      QueryModel queryModel)
    {
      QueryModel queryModel1 = subQueryExpression.QueryModel;
      MainFromClause mainFromClause = queryModel1.MainFromClause;
      if (queryModel.BodyClauses.Count == 0)
      {
        foreach (ResultOperatorBase resultOperator in (Collection<ResultOperatorBase>) queryModel1.ResultOperators)
          queryModel.ResultOperators.Add(resultOperator);
        foreach (IBodyClause bodyClause in (Collection<IBodyClause>) queryModel1.BodyClauses)
          queryModel.BodyClauses.Add(bodyClause);
        PagingRewriterSelectClauseVisitor selectClauseVisitor = new PagingRewriterSelectClauseVisitor((FromClauseBase) queryModel.MainFromClause);
        queryModel.SelectClause.TransformExpressions(new Func<Expression, Expression>(selectClauseVisitor.Swap));
      }
      else
      {
        ContainsResultOperator containsResultOperator = new ContainsResultOperator((Expression) new QuerySourceReferenceExpression((IQuerySource) mainFromClause));
        QueryModel queryModel2 = queryModel1.Clone();
        queryModel2.ResultOperators.Add((ResultOperatorBase) containsResultOperator);
        queryModel2.ResultTypeOverride = typeof (bool);
        WhereClause whereClause = new WhereClause((Expression) new SubQueryExpression(queryModel2));
        queryModel.BodyClauses.Add((IBodyClause) whereClause);
        if (!queryModel.BodyClauses.OfType<OrderByClause>().Any<OrderByClause>())
        {
          foreach (OrderByClause orderByClause in queryModel1.BodyClauses.OfType<OrderByClause>())
            queryModel.BodyClauses.Add((IBodyClause) orderByClause);
        }
      }
      SwapQuerySourceVisitor querySourceVisitor = new SwapQuerySourceVisitor((IQuerySource) queryModel.MainFromClause, (IQuerySource) mainFromClause);
      queryModel.TransformExpressions(new Func<Expression, Expression>(querySourceVisitor.Swap));
      queryModel.MainFromClause = mainFromClause;
    }
  }
}
