// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.ReWriters.QueryReferenceExpressionFlattener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.Visitors;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ResultOperators;
using Remotion.Linq.Collections;
using Remotion.Linq.Parsing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.ReWriters
{
  public class QueryReferenceExpressionFlattener : NhExpressionTreeVisitor
  {
    private readonly QueryModel _model;
    private static readonly List<Type> FlattenableResultOperactors = new List<Type>()
    {
      typeof (CacheableResultOperator),
      typeof (TimeoutResultOperator),
      typeof (SkipResultOperator),
      typeof (TakeResultOperator)
    };

    private QueryReferenceExpressionFlattener(QueryModel model) => this._model = model;

    public static void ReWrite(QueryModel model)
    {
      QueryReferenceExpressionFlattener expressionFlattener = new QueryReferenceExpressionFlattener(model);
      model.TransformExpressions(new Func<Expression, Expression>(((ExpressionTreeVisitor) expressionFlattener).VisitExpression));
    }

    protected override Expression VisitSubQueryExpression(SubQueryExpression subQuery)
    {
      if (subQuery.QueryModel.BodyClauses.Count > 0)
        return base.VisitSubQueryExpression(subQuery);
      ObservableCollection<ResultOperatorBase> resultOperators = subQuery.QueryModel.ResultOperators;
      if (resultOperators.Count != 0 && !this.HasJustAllFlattenableOperator((IEnumerable<ResultOperatorBase>) resultOperators) || !(subQuery.QueryModel.SelectClause.Selector is QuerySourceReferenceExpression selector) || selector.ReferencedQuerySource != subQuery.QueryModel.MainFromClause)
        return base.VisitSubQueryExpression(subQuery);
      foreach (ResultOperatorBase resultOperatorBase in (Collection<ResultOperatorBase>) resultOperators)
        this._model.ResultOperators.Add(resultOperatorBase);
      return subQuery.QueryModel.MainFromClause.FromExpression;
    }

    private bool HasJustAllFlattenableOperator(IEnumerable<ResultOperatorBase> resultOperators)
    {
      return resultOperators.All<ResultOperatorBase>((Func<ResultOperatorBase, bool>) (x => QueryReferenceExpressionFlattener.FlattenableResultOperactors.Contains(x.GetType())));
    }

    protected override Expression VisitQuerySourceReferenceExpression(
      QuerySourceReferenceExpression expression)
    {
      return expression.ReferencedQuerySource is FromClauseBase referencedQuerySource && referencedQuerySource.FromExpression is QuerySourceReferenceExpression && expression.Type == referencedQuerySource.FromExpression.Type ? referencedQuerySource.FromExpression : base.VisitQuerySourceReferenceExpression(expression);
    }
  }
}
