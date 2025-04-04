// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.QueryModelVisitorBase
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Collections;
using Remotion.Linq.Utilities;

#nullable disable
namespace Remotion.Linq
{
  public abstract class QueryModelVisitorBase : IQueryModelVisitor
  {
    public virtual void VisitQueryModel(QueryModel queryModel)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      queryModel.MainFromClause.Accept((IQueryModelVisitor) this, queryModel);
      this.VisitBodyClauses(queryModel.BodyClauses, queryModel);
      queryModel.SelectClause.Accept((IQueryModelVisitor) this, queryModel);
      this.VisitResultOperators(queryModel.ResultOperators, queryModel);
    }

    public virtual void VisitMainFromClause(MainFromClause fromClause, QueryModel queryModel)
    {
      ArgumentUtility.CheckNotNull<MainFromClause>(nameof (fromClause), fromClause);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
    }

    public virtual void VisitAdditionalFromClause(
      AdditionalFromClause fromClause,
      QueryModel queryModel,
      int index)
    {
      ArgumentUtility.CheckNotNull<AdditionalFromClause>(nameof (fromClause), fromClause);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
    }

    public virtual void VisitJoinClause(JoinClause joinClause, QueryModel queryModel, int index)
    {
      ArgumentUtility.CheckNotNull<JoinClause>(nameof (joinClause), joinClause);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
    }

    public virtual void VisitJoinClause(
      JoinClause joinClause,
      QueryModel queryModel,
      GroupJoinClause groupJoinClause)
    {
      ArgumentUtility.CheckNotNull<JoinClause>(nameof (joinClause), joinClause);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      ArgumentUtility.CheckNotNull<GroupJoinClause>(nameof (groupJoinClause), groupJoinClause);
    }

    public virtual void VisitGroupJoinClause(
      GroupJoinClause groupJoinClause,
      QueryModel queryModel,
      int index)
    {
      ArgumentUtility.CheckNotNull<GroupJoinClause>(nameof (groupJoinClause), groupJoinClause);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      groupJoinClause.JoinClause.Accept((IQueryModelVisitor) this, queryModel, groupJoinClause);
    }

    public virtual void VisitWhereClause(WhereClause whereClause, QueryModel queryModel, int index)
    {
      ArgumentUtility.CheckNotNull<WhereClause>(nameof (whereClause), whereClause);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
    }

    public virtual void VisitOrderByClause(
      OrderByClause orderByClause,
      QueryModel queryModel,
      int index)
    {
      ArgumentUtility.CheckNotNull<OrderByClause>(nameof (orderByClause), orderByClause);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      this.VisitOrderings(orderByClause.Orderings, queryModel, orderByClause);
    }

    public virtual void VisitOrdering(
      Ordering ordering,
      QueryModel queryModel,
      OrderByClause orderByClause,
      int index)
    {
      ArgumentUtility.CheckNotNull<Ordering>(nameof (ordering), ordering);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      ArgumentUtility.CheckNotNull<OrderByClause>(nameof (orderByClause), orderByClause);
    }

    public virtual void VisitSelectClause(SelectClause selectClause, QueryModel queryModel)
    {
      ArgumentUtility.CheckNotNull<SelectClause>(nameof (selectClause), selectClause);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
    }

    public virtual void VisitResultOperator(
      ResultOperatorBase resultOperator,
      QueryModel queryModel,
      int index)
    {
      ArgumentUtility.CheckNotNull<ResultOperatorBase>(nameof (resultOperator), resultOperator);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
    }

    protected virtual void VisitBodyClauses(
      ObservableCollection<IBodyClause> bodyClauses,
      QueryModel queryModel)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      ArgumentUtility.CheckNotNull<ObservableCollection<IBodyClause>>(nameof (bodyClauses), bodyClauses);
      foreach (ObservableCollection<IBodyClause>.IndexValuePair indexValuePair in bodyClauses.AsChangeResistantEnumerableWithIndex())
        indexValuePair.Value.Accept((IQueryModelVisitor) this, queryModel, indexValuePair.Index);
    }

    protected virtual void VisitOrderings(
      ObservableCollection<Ordering> orderings,
      QueryModel queryModel,
      OrderByClause orderByClause)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      ArgumentUtility.CheckNotNull<OrderByClause>(nameof (orderByClause), orderByClause);
      ArgumentUtility.CheckNotNull<ObservableCollection<Ordering>>(nameof (orderings), orderings);
      foreach (ObservableCollection<Ordering>.IndexValuePair indexValuePair in orderings.AsChangeResistantEnumerableWithIndex())
        indexValuePair.Value.Accept((IQueryModelVisitor) this, queryModel, orderByClause, indexValuePair.Index);
    }

    protected virtual void VisitResultOperators(
      ObservableCollection<ResultOperatorBase> resultOperators,
      QueryModel queryModel)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      ArgumentUtility.CheckNotNull<ObservableCollection<ResultOperatorBase>>(nameof (resultOperators), resultOperators);
      foreach (ObservableCollection<ResultOperatorBase>.IndexValuePair indexValuePair in resultOperators.AsChangeResistantEnumerableWithIndex())
        indexValuePair.Value.Accept((IQueryModelVisitor) this, queryModel, indexValuePair.Index);
    }
  }
}
