// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.ReWriters.MergeAggregatingResultsRewriter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.Expressions;
using NHibernate.Linq.Visitors;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;
using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.ReWriters
{
  public class MergeAggregatingResultsRewriter : QueryModelVisitorBase
  {
    private MergeAggregatingResultsRewriter()
    {
    }

    public static void ReWrite(QueryModel model)
    {
      new MergeAggregatingResultsRewriter().VisitQueryModel(model);
    }

    public override void VisitResultOperator(
      ResultOperatorBase resultOperator,
      QueryModel queryModel,
      int index)
    {
      switch (resultOperator)
      {
        case SumResultOperator _:
          queryModel.SelectClause.Selector = (Expression) new NhSumExpression(queryModel.SelectClause.Selector);
          queryModel.ResultOperators.Remove(resultOperator);
          break;
        case AverageResultOperator _:
          queryModel.SelectClause.Selector = (Expression) new NhAverageExpression(queryModel.SelectClause.Selector);
          queryModel.ResultOperators.Remove(resultOperator);
          break;
        case MinResultOperator _:
          queryModel.SelectClause.Selector = (Expression) new NhMinExpression(queryModel.SelectClause.Selector);
          queryModel.ResultOperators.Remove(resultOperator);
          break;
        case MaxResultOperator _:
          queryModel.SelectClause.Selector = (Expression) new NhMaxExpression(queryModel.SelectClause.Selector);
          queryModel.ResultOperators.Remove(resultOperator);
          break;
        case DistinctResultOperator _:
          queryModel.SelectClause.Selector = (Expression) new NhDistinctExpression(queryModel.SelectClause.Selector);
          queryModel.ResultOperators.Remove(resultOperator);
          break;
        case CountResultOperator _:
          queryModel.SelectClause.Selector = (Expression) new NhShortCountExpression(MergeAggregatingResultsRewriter.TransformCountExpression(queryModel.SelectClause.Selector));
          queryModel.ResultOperators.Remove(resultOperator);
          break;
        case LongCountResultOperator _:
          queryModel.SelectClause.Selector = (Expression) new NhLongCountExpression(MergeAggregatingResultsRewriter.TransformCountExpression(queryModel.SelectClause.Selector));
          queryModel.ResultOperators.Remove(resultOperator);
          break;
      }
      base.VisitResultOperator(resultOperator, queryModel, index);
    }

    public override void VisitSelectClause(SelectClause selectClause, QueryModel queryModel)
    {
      selectClause.TransformExpressions((Func<Expression, Expression>) (e => MergeAggregatingResultsInExpressionRewriter.Rewrite(e, new NameGenerator(queryModel))));
    }

    public override void VisitWhereClause(
      WhereClause whereClause,
      QueryModel queryModel,
      int index)
    {
      whereClause.TransformExpressions((Func<Expression, Expression>) (e => MergeAggregatingResultsInExpressionRewriter.Rewrite(e, new NameGenerator(queryModel))));
    }

    public override void VisitOrdering(
      Ordering ordering,
      QueryModel queryModel,
      OrderByClause orderByClause,
      int index)
    {
      ordering.TransformExpressions((Func<Expression, Expression>) (e => MergeAggregatingResultsInExpressionRewriter.Rewrite(e, new NameGenerator(queryModel))));
    }

    private static Expression TransformCountExpression(Expression expression)
    {
      return expression.NodeType == ExpressionType.MemberInit || expression.NodeType == ExpressionType.New || expression.NodeType == (ExpressionType) 100001 ? (Expression) new NhStarExpression(expression) : expression;
    }
  }
}
