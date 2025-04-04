// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.QueryModelVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast;
using NHibernate.Linq.Clauses;
using NHibernate.Linq.GroupBy;
using NHibernate.Linq.GroupJoin;
using NHibernate.Linq.NestedSelects;
using NHibernate.Linq.ResultOperators;
using NHibernate.Linq.ReWriters;
using NHibernate.Linq.Visitors.ResultOperatorProcessors;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;
using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.EagerFetching;
using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Visitors
{
  public class QueryModelVisitor : QueryModelVisitorBase
  {
    private readonly IntermediateHqlTree _hqlTree;
    private static readonly ResultOperatorMap ResultOperatorMap = new ResultOperatorMap();
    private bool _serverSide = true;

    public static ExpressionToHqlTranslationResults GenerateHqlQuery(
      QueryModel queryModel,
      VisitorParameters parameters,
      bool root)
    {
      NestedSelectRewriter.ReWrite(queryModel, (ISessionFactory) parameters.SessionFactory);
      RemoveUnnecessaryBodyOperators.ReWrite(queryModel);
      MergeAggregatingResultsRewriter.ReWrite(queryModel);
      NonAggregatingGroupByRewriter.ReWrite(queryModel);
      AggregatingGroupByRewriter.ReWrite(queryModel);
      AggregatingGroupJoinRewriter.ReWrite(queryModel);
      NonAggregatingGroupJoinRewriter.ReWrite(queryModel);
      PagingRewriter.ReWrite(queryModel);
      QueryReferenceExpressionFlattener.ReWrite(queryModel);
      AddJoinsReWriter.ReWrite(queryModel, (ISessionFactory) parameters.SessionFactory);
      MoveOrderByToEndRewriter.ReWrite(queryModel);
      ResultOperatorRewriterResult operatorRewriterResult = ResultOperatorRewriter.Rewrite(queryModel);
      QuerySourceIdentifier.Visit(parameters.QuerySourceNamer, queryModel);
      QueryModelVisitor queryModelVisitor = new QueryModelVisitor(parameters, root, queryModel)
      {
        RewrittenOperatorResult = operatorRewriterResult
      };
      queryModelVisitor.Visit();
      return queryModelVisitor._hqlTree.GetTranslation();
    }

    public VisitorParameters VisitorParameters { get; private set; }

    public IStreamedDataInfo CurrentEvaluationType { get; private set; }

    public IStreamedDataInfo PreviousEvaluationType { get; private set; }

    public QueryModel Model { get; private set; }

    public ResultOperatorRewriterResult RewrittenOperatorResult { get; private set; }

    static QueryModelVisitor()
    {
      QueryModelVisitor.ResultOperatorMap.Add<AggregateResultOperator, ProcessAggregate>();
      QueryModelVisitor.ResultOperatorMap.Add<AggregateFromSeedResultOperator, ProcessAggregateFromSeed>();
      QueryModelVisitor.ResultOperatorMap.Add<FirstResultOperator, ProcessFirst>();
      QueryModelVisitor.ResultOperatorMap.Add<TakeResultOperator, ProcessTake>();
      QueryModelVisitor.ResultOperatorMap.Add<SkipResultOperator, ProcessSkip>();
      QueryModelVisitor.ResultOperatorMap.Add<GroupResultOperator, ProcessGroupBy>();
      QueryModelVisitor.ResultOperatorMap.Add<SingleResultOperator, ProcessSingle>();
      QueryModelVisitor.ResultOperatorMap.Add<ContainsResultOperator, ProcessContains>();
      QueryModelVisitor.ResultOperatorMap.Add<NonAggregatingGroupBy, ProcessNonAggregatingGroupBy>();
      QueryModelVisitor.ResultOperatorMap.Add<ClientSideSelect, ProcessClientSideSelect>();
      QueryModelVisitor.ResultOperatorMap.Add<ClientSideSelect2, ProcessClientSideSelect2>();
      QueryModelVisitor.ResultOperatorMap.Add<AnyResultOperator, ProcessAny>();
      QueryModelVisitor.ResultOperatorMap.Add<AllResultOperator, ProcessAll>();
      QueryModelVisitor.ResultOperatorMap.Add<FetchOneRequest, ProcessFetchOne>();
      QueryModelVisitor.ResultOperatorMap.Add<FetchManyRequest, ProcessFetchMany>();
      QueryModelVisitor.ResultOperatorMap.Add<CacheableResultOperator, ProcessCacheable>();
      QueryModelVisitor.ResultOperatorMap.Add<TimeoutResultOperator, ProcessTimeout>();
      QueryModelVisitor.ResultOperatorMap.Add<OfTypeResultOperator, ProcessOfType>();
      QueryModelVisitor.ResultOperatorMap.Add<CastResultOperator, ProcessCast>();
    }

    private QueryModelVisitor(
      VisitorParameters visitorParameters,
      bool root,
      QueryModel queryModel)
    {
      this.VisitorParameters = visitorParameters;
      this.Model = queryModel;
      this._hqlTree = new IntermediateHqlTree(root);
    }

    private void Visit() => this.VisitQueryModel(this.Model);

    public override void VisitMainFromClause(MainFromClause fromClause, QueryModel queryModel)
    {
      string name = this.VisitorParameters.QuerySourceNamer.GetName((IQuerySource) fromClause);
      this._hqlTree.AddFromClause((HqlTreeNode) this._hqlTree.TreeBuilder.Range(HqlGeneratorExpressionTreeVisitor.Visit(fromClause.FromExpression, this.VisitorParameters), this._hqlTree.TreeBuilder.Alias(name)));
      if (this.RewrittenOperatorResult != null)
      {
        this.CurrentEvaluationType = this.RewrittenOperatorResult.EvaluationType;
        foreach (ResultOperatorBase rewrittenOperator in this.RewrittenOperatorResult.RewrittenOperators)
          this.VisitResultOperator(rewrittenOperator, queryModel, -1);
      }
      base.VisitMainFromClause(fromClause, queryModel);
    }

    public override void VisitAdditionalFromClause(
      AdditionalFromClause fromClause,
      QueryModel queryModel,
      int index)
    {
      string name = this.VisitorParameters.QuerySourceNamer.GetName((IQuerySource) fromClause);
      if (fromClause is NhJoinClause joinClause)
        this.VisitNhJoinClause(name, joinClause);
      else if (fromClause.FromExpression is MemberExpression)
        this._hqlTree.AddFromClause((HqlTreeNode) this._hqlTree.TreeBuilder.Join(HqlGeneratorExpressionTreeVisitor.Visit(fromClause.FromExpression, this.VisitorParameters).AsExpression(), this._hqlTree.TreeBuilder.Alias(name)));
      else
        this._hqlTree.AddFromClause((HqlTreeNode) this._hqlTree.TreeBuilder.Range(HqlGeneratorExpressionTreeVisitor.Visit(fromClause.FromExpression, this.VisitorParameters), this._hqlTree.TreeBuilder.Alias(name)));
      base.VisitAdditionalFromClause(fromClause, queryModel, index);
    }

    private void VisitNhJoinClause(string querySourceName, NhJoinClause joinClause)
    {
      HqlExpression expression = HqlGeneratorExpressionTreeVisitor.Visit(joinClause.FromExpression, this.VisitorParameters).AsExpression();
      HqlAlias alias = this._hqlTree.TreeBuilder.Alias(querySourceName);
      this._hqlTree.AddFromClause(!joinClause.IsInner ? (HqlTreeNode) this._hqlTree.TreeBuilder.LeftJoin(expression, alias) : (HqlTreeNode) this._hqlTree.TreeBuilder.Join(expression, alias));
    }

    public override void VisitResultOperator(
      ResultOperatorBase resultOperator,
      QueryModel queryModel,
      int index)
    {
      this.PreviousEvaluationType = this.CurrentEvaluationType;
      this.CurrentEvaluationType = resultOperator.GetOutputDataInfo(this.PreviousEvaluationType);
      if (resultOperator is ClientSideTransformOperator)
        this._serverSide = false;
      else if (!this._serverSide)
        throw new NotSupportedException("Processing server-side result operator after doing client-side ones.  We've got the ordering wrong...");
      QueryModelVisitor.ResultOperatorMap.Process(resultOperator, this, this._hqlTree);
    }

    public override void VisitSelectClause(SelectClause selectClause, QueryModel queryModel)
    {
      this.CurrentEvaluationType = (IStreamedDataInfo) selectClause.GetOutputDataInfo();
      SelectClauseVisitor selectClauseVisitor = new SelectClauseVisitor(typeof (object[]), this.VisitorParameters);
      selectClauseVisitor.Visit(selectClause.Selector);
      if (selectClauseVisitor.ProjectionExpression != null)
        this._hqlTree.AddItemTransformer(selectClauseVisitor.ProjectionExpression);
      this._hqlTree.AddSelectClause((HqlTreeNode) this._hqlTree.TreeBuilder.Select(selectClauseVisitor.GetHqlNodes()));
      base.VisitSelectClause(selectClause, queryModel);
    }

    public override void VisitWhereClause(
      WhereClause whereClause,
      QueryModel queryModel,
      int index)
    {
      HqlBooleanExpression where = HqlGeneratorExpressionTreeVisitor.Visit(whereClause.Predicate, this.VisitorParameters).AsBooleanExpression();
      if (whereClause is NhHavingClause)
        this._hqlTree.AddHavingClause(where);
      else
        this._hqlTree.AddWhereClause(where);
    }

    public override void VisitOrderByClause(
      OrderByClause orderByClause,
      QueryModel queryModel,
      int index)
    {
      foreach (Ordering ordering in (Collection<Ordering>) orderByClause.Orderings)
        this._hqlTree.AddOrderByClause(HqlGeneratorExpressionTreeVisitor.Visit(ordering.Expression, this.VisitorParameters).AsExpression(), ordering.OrderingDirection == OrderingDirection.Asc ? (HqlDirectionStatement) this._hqlTree.TreeBuilder.Ascending() : (HqlDirectionStatement) this._hqlTree.TreeBuilder.Descending());
    }

    public override void VisitJoinClause(JoinClause joinClause, QueryModel queryModel, int index)
    {
      this._hqlTree.AddWhereClause(new EqualityHqlGenerator(this.VisitorParameters).Visit(joinClause.InnerKeySelector, joinClause.OuterKeySelector));
      this._hqlTree.AddFromClause((HqlTreeNode) this._hqlTree.TreeBuilder.Range(HqlGeneratorExpressionTreeVisitor.Visit(joinClause.InnerSequence, this.VisitorParameters), this._hqlTree.TreeBuilder.Alias(joinClause.ItemName)));
    }

    public override void VisitGroupJoinClause(
      GroupJoinClause groupJoinClause,
      QueryModel queryModel,
      int index)
    {
      throw new NotImplementedException();
    }
  }
}
