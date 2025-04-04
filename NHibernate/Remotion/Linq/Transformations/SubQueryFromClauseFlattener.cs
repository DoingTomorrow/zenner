// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Transformations.SubQueryFromClauseFlattener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ExpressionTreeVisitors;
using Remotion.Linq.Collections;
using Remotion.Linq.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Transformations
{
  public class SubQueryFromClauseFlattener : QueryModelVisitorBase
  {
    public override void VisitMainFromClause(MainFromClause fromClause, QueryModel queryModel)
    {
      ArgumentUtility.CheckNotNull<MainFromClause>(nameof (fromClause), fromClause);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      if (fromClause.FromExpression is SubQueryExpression fromExpression)
        this.FlattenSubQuery(fromExpression, (FromClauseBase) fromClause, queryModel, 0);
      base.VisitMainFromClause(fromClause, queryModel);
    }

    public override void VisitAdditionalFromClause(
      AdditionalFromClause fromClause,
      QueryModel queryModel,
      int index)
    {
      ArgumentUtility.CheckNotNull<AdditionalFromClause>(nameof (fromClause), fromClause);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      if (fromClause.FromExpression is SubQueryExpression fromExpression)
        this.FlattenSubQuery(fromExpression, (FromClauseBase) fromClause, queryModel, index + 1);
      base.VisitAdditionalFromClause(fromClause, queryModel, index);
    }

    protected virtual void FlattenSubQuery(
      SubQueryExpression subQueryExpression,
      FromClauseBase fromClause,
      QueryModel queryModel,
      int destinationIndex)
    {
      ArgumentUtility.CheckNotNull<SubQueryExpression>(nameof (subQueryExpression), subQueryExpression);
      ArgumentUtility.CheckNotNull<FromClauseBase>(nameof (fromClause), fromClause);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      this.CheckFlattenable(subQueryExpression.QueryModel);
      MainFromClause mainFromClause = subQueryExpression.QueryModel.MainFromClause;
      this.CopyFromClauseData((FromClauseBase) mainFromClause, fromClause);
      QuerySourceMapping innerSelectorMapping = new QuerySourceMapping();
      innerSelectorMapping.AddMapping((IQuerySource) fromClause, subQueryExpression.QueryModel.SelectClause.Selector);
      queryModel.TransformExpressions((Func<Expression, Expression>) (ex => ReferenceReplacingExpressionTreeVisitor.ReplaceClauseReferences(ex, innerSelectorMapping, false)));
      this.InsertBodyClauses(subQueryExpression.QueryModel.BodyClauses, queryModel, destinationIndex);
      QuerySourceMapping innerBodyClauseMapping = new QuerySourceMapping();
      innerBodyClauseMapping.AddMapping((IQuerySource) mainFromClause, (Expression) new QuerySourceReferenceExpression((IQuerySource) fromClause));
      queryModel.TransformExpressions((Func<Expression, Expression>) (ex => ReferenceReplacingExpressionTreeVisitor.ReplaceClauseReferences(ex, innerBodyClauseMapping, false)));
    }

    protected virtual void CheckFlattenable(QueryModel subQueryModel)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (subQueryModel), subQueryModel);
      if (subQueryModel.ResultOperators.Count > 0)
        throw new NotSupportedException(string.Format("The subquery '{0}' cannot be flattened and pulled out of the from clause because it contains result operators.", (object) subQueryModel));
      if (subQueryModel.BodyClauses.Any<IBodyClause>((Func<IBodyClause, bool>) (bc => bc is OrderByClause)))
        throw new NotSupportedException(string.Format("The subquery '{0}' cannot be flattened and pulled out of the from clause because it contains an OrderByClause.", (object) subQueryModel));
    }

    protected void CopyFromClauseData(FromClauseBase source, FromClauseBase destination)
    {
      ArgumentUtility.CheckNotNull<FromClauseBase>(nameof (source), source);
      ArgumentUtility.CheckNotNull<FromClauseBase>(nameof (destination), destination);
      destination.FromExpression = source.FromExpression;
      destination.ItemName = source.ItemName;
      destination.ItemType = source.ItemType;
    }

    protected void InsertBodyClauses(
      ObservableCollection<IBodyClause> bodyClauses,
      QueryModel destinationQueryModel,
      int destinationIndex)
    {
      ArgumentUtility.CheckNotNull<ObservableCollection<IBodyClause>>(nameof (bodyClauses), bodyClauses);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (destinationQueryModel), destinationQueryModel);
      foreach (IBodyClause bodyClause in (Collection<IBodyClause>) bodyClauses)
      {
        destinationQueryModel.BodyClauses.Insert(destinationIndex, bodyClause);
        ++destinationIndex;
      }
    }
  }
}
