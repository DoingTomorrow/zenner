// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.QuerySourceLocator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq;
using Remotion.Linq.Clauses;
using System;

#nullable disable
namespace NHibernate.Linq.Visitors
{
  public class QuerySourceLocator : QueryModelVisitorBase
  {
    private readonly Type _type;
    private IQuerySource _querySource;

    private QuerySourceLocator(Type type) => this._type = type;

    public static IQuerySource FindQuerySource(QueryModel queryModel, Type type)
    {
      QuerySourceLocator querySourceLocator = new QuerySourceLocator(type);
      querySourceLocator.VisitQueryModel(queryModel);
      return querySourceLocator._querySource;
    }

    public override void VisitMainFromClause(MainFromClause fromClause, QueryModel queryModel)
    {
      if (this._type.IsAssignableFrom(fromClause.ItemType))
        this._querySource = (IQuerySource) fromClause;
      else
        base.VisitMainFromClause(fromClause, queryModel);
    }
  }
}
