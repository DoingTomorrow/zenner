// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.QuerySourceIdentifier
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;

#nullable disable
namespace NHibernate.Linq.Visitors
{
  public class QuerySourceIdentifier : QueryModelVisitorBase
  {
    private readonly QuerySourceNamer _namer;

    private QuerySourceIdentifier(QuerySourceNamer namer) => this._namer = namer;

    public static void Visit(QuerySourceNamer namer, QueryModel queryModel)
    {
      new QuerySourceIdentifier(namer).VisitQueryModel(queryModel);
    }

    public override void VisitMainFromClause(MainFromClause fromClause, QueryModel queryModel)
    {
      this._namer.Add((IQuerySource) fromClause);
    }

    public override void VisitAdditionalFromClause(
      AdditionalFromClause fromClause,
      QueryModel queryModel,
      int index)
    {
      this._namer.Add((IQuerySource) fromClause);
    }

    public override void VisitJoinClause(
      JoinClause joinClause,
      QueryModel queryModel,
      GroupJoinClause groupJoinClause)
    {
      this._namer.Add((IQuerySource) joinClause);
    }

    public override void VisitGroupJoinClause(
      GroupJoinClause groupJoinClause,
      QueryModel queryModel,
      int index)
    {
      this._namer.Add((IQuerySource) groupJoinClause);
    }

    public override void VisitJoinClause(JoinClause joinClause, QueryModel queryModel, int index)
    {
      this._namer.Add((IQuerySource) joinClause);
    }

    public override void VisitResultOperator(
      ResultOperatorBase resultOperator,
      QueryModel queryModel,
      int index)
    {
      if (!(resultOperator is GroupResultOperator groupResultOperator))
        return;
      this._namer.Add((IQuerySource) groupResultOperator);
    }

    public QuerySourceNamer Namer => this._namer;
  }
}
