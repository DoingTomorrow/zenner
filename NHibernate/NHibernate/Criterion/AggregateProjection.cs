// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.AggregateProjection
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public class AggregateProjection : SimpleProjection
  {
    protected readonly string aggregate;
    protected readonly IProjection projection;
    protected readonly string propertyName;

    protected internal AggregateProjection(string aggregate, string propertyName)
    {
      this.propertyName = propertyName;
      this.aggregate = aggregate;
    }

    protected internal AggregateProjection(string aggregate, IProjection projection)
    {
      this.aggregate = aggregate;
      this.projection = projection;
    }

    public override bool IsAggregate => true;

    public override string ToString()
    {
      return this.aggregate + "(" + (this.projection != null ? (object) this.projection.ToString() : (object) this.propertyName) + (object) ')';
    }

    public override IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      if (this.projection != null)
        return this.projection.GetTypes(criteria, criteriaQuery);
      return new IType[1]
      {
        criteriaQuery.GetType(criteria, this.propertyName)
      };
    }

    public override SqlString ToSqlString(
      ICriteria criteria,
      int loc,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      return this.projection != null ? new SqlString(new object[6]
      {
        (object) this.aggregate,
        (object) "(",
        (object) StringHelper.RemoveAsAliasesFromSql(this.projection.ToSqlString(criteria, loc, criteriaQuery, enabledFilters)),
        (object) ") as y",
        (object) loc.ToString(),
        (object) "_"
      }) : new SqlString(new object[6]
      {
        (object) this.aggregate,
        (object) "(",
        (object) criteriaQuery.GetColumn(criteria, this.propertyName),
        (object) ") as y",
        (object) loc.ToString(),
        (object) "_"
      });
    }

    public override bool IsGrouped => false;

    public override SqlString ToGroupSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      throw new InvalidOperationException("not a grouping projection");
    }

    public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return this.projection != null ? this.projection.GetTypedValues(criteria, criteriaQuery) : base.GetTypedValues(criteria, criteriaQuery);
    }
  }
}
