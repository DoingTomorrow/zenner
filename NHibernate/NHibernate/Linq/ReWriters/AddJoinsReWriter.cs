// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.ReWriters.AddJoinsReWriter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.Visitors;
using NHibernate.Metadata;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using System;

#nullable disable
namespace NHibernate.Linq.ReWriters
{
  public class AddJoinsReWriter : QueryModelVisitorBase, IIsEntityDecider
  {
    private readonly ISessionFactory _sessionFactory;
    private readonly SelectJoinDetector _selectJoinDetector;
    private readonly ResultOperatorAndOrderByJoinDetector _resultOperatorAndOrderByJoinDetector;
    private readonly WhereJoinDetector _whereJoinDetector;

    private AddJoinsReWriter(ISessionFactory sessionFactory, QueryModel queryModel)
    {
      this._sessionFactory = sessionFactory;
      Joiner joiner = new Joiner(queryModel);
      this._selectJoinDetector = new SelectJoinDetector((IIsEntityDecider) this, (IJoiner) joiner);
      this._resultOperatorAndOrderByJoinDetector = new ResultOperatorAndOrderByJoinDetector((IIsEntityDecider) this, (IJoiner) joiner);
      this._whereJoinDetector = new WhereJoinDetector((IIsEntityDecider) this, (IJoiner) joiner);
    }

    public static void ReWrite(QueryModel queryModel, ISessionFactory sessionFactory)
    {
      new AddJoinsReWriter(sessionFactory, queryModel).VisitQueryModel(queryModel);
    }

    public override void VisitSelectClause(SelectClause selectClause, QueryModel queryModel)
    {
      this._selectJoinDetector.Transform(selectClause);
    }

    public override void VisitOrdering(
      Ordering ordering,
      QueryModel queryModel,
      OrderByClause orderByClause,
      int index)
    {
      this._resultOperatorAndOrderByJoinDetector.Transform(ordering);
    }

    public override void VisitResultOperator(
      ResultOperatorBase resultOperator,
      QueryModel queryModel,
      int index)
    {
      this._resultOperatorAndOrderByJoinDetector.Transform(resultOperator);
    }

    public override void VisitWhereClause(
      WhereClause whereClause,
      QueryModel queryModel,
      int index)
    {
      this._whereJoinDetector.Transform(whereClause);
    }

    public bool IsEntity(Type type) => this._sessionFactory.GetClassMetadata(type) != null;

    public bool IsIdentifier(Type type, string propertyName)
    {
      IClassMetadata classMetadata = this._sessionFactory.GetClassMetadata(type);
      return classMetadata != null && propertyName.Equals(classMetadata.IdentifierPropertyName);
    }
  }
}
