// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.IQueryModelVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;

#nullable disable
namespace Remotion.Linq
{
  public interface IQueryModelVisitor
  {
    void VisitQueryModel(QueryModel queryModel);

    void VisitMainFromClause(MainFromClause fromClause, QueryModel queryModel);

    void VisitAdditionalFromClause(
      AdditionalFromClause fromClause,
      QueryModel queryModel,
      int index);

    void VisitJoinClause(JoinClause joinClause, QueryModel queryModel, int index);

    void VisitJoinClause(
      JoinClause joinClause,
      QueryModel queryModel,
      GroupJoinClause groupJoinClause);

    void VisitGroupJoinClause(GroupJoinClause joinClause, QueryModel queryModel, int index);

    void VisitWhereClause(WhereClause whereClause, QueryModel queryModel, int index);

    void VisitOrderByClause(OrderByClause orderByClause, QueryModel queryModel, int index);

    void VisitOrdering(
      Ordering ordering,
      QueryModel queryModel,
      OrderByClause orderByClause,
      int index);

    void VisitSelectClause(SelectClause selectClause, QueryModel queryModel);

    void VisitResultOperator(ResultOperatorBase resultOperator, QueryModel queryModel, int index);
  }
}
