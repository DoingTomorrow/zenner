// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.SubqueryProjection
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public class SubqueryProjection : SimpleProjection
  {
    private SelectSubqueryExpression _subQuery;

    protected internal SubqueryProjection(SelectSubqueryExpression subquery)
    {
      this._subQuery = subquery;
    }

    public override string ToString() => this._subQuery.ToString();

    public override bool IsGrouped => false;

    public override bool IsAggregate => false;

    public override IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      this._subQuery.InitializeInnerQueryAndParameters(criteriaQuery);
      return this._subQuery.GetTypes();
    }

    public override SqlString ToSqlString(
      ICriteria criteria,
      int loc,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      return this._subQuery.ToSqlString(criteria, criteriaQuery, enabledFilters).Append(new SqlString(new object[3]
      {
        (object) " as y",
        (object) loc.ToString(),
        (object) "_"
      }));
    }

    public override SqlString ToGroupSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      throw new InvalidOperationException("not a grouping projection");
    }

    public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return this._subQuery.GetTypedValues(criteria, criteriaQuery);
    }
  }
}
