// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.ReWriters.ResultOperatorRewriter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.Visitors;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ResultOperators;
using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.EagerFetching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.ReWriters
{
  public class ResultOperatorRewriter : QueryModelVisitorBase
  {
    private readonly List<ResultOperatorBase> resultOperators = new List<ResultOperatorBase>();
    private IStreamedDataInfo evaluationType;

    private ResultOperatorRewriter()
    {
    }

    public static ResultOperatorRewriterResult Rewrite(QueryModel queryModel)
    {
      ResultOperatorRewriter operatorRewriter = new ResultOperatorRewriter();
      operatorRewriter.VisitQueryModel(queryModel);
      return new ResultOperatorRewriterResult((IEnumerable<ResultOperatorBase>) operatorRewriter.resultOperators, operatorRewriter.evaluationType);
    }

    public override void VisitMainFromClause(MainFromClause fromClause, QueryModel queryModel)
    {
      base.VisitMainFromClause(fromClause, queryModel);
      ResultOperatorRewriter.ResultOperatorExpressionRewriter expressionRewriter = new ResultOperatorRewriter.ResultOperatorExpressionRewriter();
      fromClause.TransformExpressions(new Func<Expression, Expression>(expressionRewriter.Rewrite));
      if (fromClause.FromExpression.NodeType == ExpressionType.Constant)
      {
        Type type = queryModel.MainFromClause.FromExpression.Type;
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (NhQueryable<>))
          queryModel.MainFromClause.ItemType = type.GetGenericArguments()[0];
      }
      this.resultOperators.AddRange(expressionRewriter.ResultOperators);
      this.evaluationType = expressionRewriter.EvaluationType;
    }

    private class ResultOperatorExpressionRewriter : NhExpressionTreeVisitor
    {
      private static readonly Type[] rewrittenTypes = new Type[5]
      {
        typeof (FetchRequestBase),
        typeof (OfTypeResultOperator),
        typeof (CacheableResultOperator),
        typeof (TimeoutResultOperator),
        typeof (CastResultOperator)
      };
      private readonly List<ResultOperatorBase> resultOperators = new List<ResultOperatorBase>();
      private IStreamedDataInfo evaluationType;

      public IEnumerable<ResultOperatorBase> ResultOperators
      {
        get => (IEnumerable<ResultOperatorBase>) this.resultOperators;
      }

      public IStreamedDataInfo EvaluationType => this.evaluationType;

      public Expression Rewrite(Expression expression) => this.VisitExpression(expression);

      protected override Expression VisitSubQueryExpression(SubQueryExpression expression)
      {
        this.resultOperators.AddRange(expression.QueryModel.ResultOperators.Where<ResultOperatorBase>((Func<ResultOperatorBase, bool>) (r => ((IEnumerable<Type>) ResultOperatorRewriter.ResultOperatorExpressionRewriter.rewrittenTypes).Any<Type>((Func<Type, bool>) (t => t.IsAssignableFrom(r.GetType()))))));
        this.resultOperators.ForEach((Action<ResultOperatorBase>) (f => expression.QueryModel.ResultOperators.Remove(f)));
        this.evaluationType = (IStreamedDataInfo) expression.QueryModel.SelectClause.GetOutputDataInfo();
        return expression.QueryModel.ResultOperators.Count == 0 && expression.QueryModel.BodyClauses.Count == 0 ? expression.QueryModel.MainFromClause.FromExpression : base.VisitSubQueryExpression(expression);
      }
    }
  }
}
