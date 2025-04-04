// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.ReWriters.MergeAggregatingResultsInExpressionRewriter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.Expressions;
using NHibernate.Linq.Visitors;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ResultOperators;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using System;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.ReWriters
{
  internal class MergeAggregatingResultsInExpressionRewriter : NhExpressionTreeVisitor
  {
    private readonly NameGenerator _nameGenerator;

    private MergeAggregatingResultsInExpressionRewriter(NameGenerator nameGenerator)
    {
      this._nameGenerator = nameGenerator;
    }

    public static Expression Rewrite(Expression expression, NameGenerator nameGenerator)
    {
      return new MergeAggregatingResultsInExpressionRewriter(nameGenerator).VisitExpression(expression);
    }

    protected override Expression VisitSubQueryExpression(SubQueryExpression expression)
    {
      MergeAggregatingResultsRewriter.ReWrite(expression.QueryModel);
      return (Expression) expression;
    }

    protected override Expression VisitMethodCallExpression(MethodCallExpression m)
    {
      if (m.Method.DeclaringType == typeof (Queryable) || m.Method.DeclaringType == typeof (Enumerable))
      {
        switch (m.Method.Name)
        {
          case "Count":
            return this.CreateAggregate(m.Arguments[0], (LambdaExpression) m.Arguments[1], (Func<Expression, Expression>) (e => (Expression) new NhShortCountExpression(e)), (Func<ResultOperatorBase>) (() => (ResultOperatorBase) new CountResultOperator()));
          case "LongCount":
            return this.CreateAggregate(m.Arguments[0], (LambdaExpression) m.Arguments[1], (Func<Expression, Expression>) (e => (Expression) new NhLongCountExpression(e)), (Func<ResultOperatorBase>) (() => (ResultOperatorBase) new LongCountResultOperator()));
          case "Min":
            return this.CreateAggregate(m.Arguments[0], (LambdaExpression) m.Arguments[1], (Func<Expression, Expression>) (e => (Expression) new NhMinExpression(e)), (Func<ResultOperatorBase>) (() => (ResultOperatorBase) new MinResultOperator()));
          case "Max":
            return this.CreateAggregate(m.Arguments[0], (LambdaExpression) m.Arguments[1], (Func<Expression, Expression>) (e => (Expression) new NhMaxExpression(e)), (Func<ResultOperatorBase>) (() => (ResultOperatorBase) new MaxResultOperator()));
          case "Sum":
            return this.CreateAggregate(m.Arguments[0], (LambdaExpression) m.Arguments[1], (Func<Expression, Expression>) (e => (Expression) new NhSumExpression(e)), (Func<ResultOperatorBase>) (() => (ResultOperatorBase) new SumResultOperator()));
          case "Average":
            return this.CreateAggregate(m.Arguments[0], (LambdaExpression) m.Arguments[1], (Func<Expression, Expression>) (e => (Expression) new NhAverageExpression(e)), (Func<ResultOperatorBase>) (() => (ResultOperatorBase) new AverageResultOperator()));
        }
      }
      return base.VisitMethodCallExpression(m);
    }

    private Expression CreateAggregate(
      Expression fromClauseExpression,
      LambdaExpression body,
      Func<Expression, Expression> aggregateFactory,
      Func<ResultOperatorBase> resultOperatorFactory)
    {
      MainFromClause mainFromClause = new MainFromClause(this._nameGenerator.GetNewName(), body.Parameters[0].Type, fromClauseExpression);
      Expression body1 = body.Body;
      Expression expression = ReplacingExpressionTreeVisitor.Replace((Expression) body.Parameters[0], (Expression) new QuerySourceReferenceExpression((IQuerySource) mainFromClause), body1);
      QueryModel queryModel = new QueryModel(mainFromClause, new SelectClause(aggregateFactory(expression)));
      queryModel.ResultOperators.Add(resultOperatorFactory());
      SubQueryExpression aggregate = new SubQueryExpression(queryModel);
      queryModel.ResultOperators.Clear();
      return (Expression) aggregate;
    }
  }
}
