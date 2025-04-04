// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.GroupedProjection
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
  public class GroupedProjection : IProjection
  {
    private readonly IProjection projection;

    public GroupedProjection(IProjection projection) => this.projection = projection;

    public virtual SqlString ToSqlString(
      ICriteria criteria,
      int position,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      return this.projection.ToSqlString(criteria, position, criteriaQuery, enabledFilters);
    }

    public virtual SqlString ToGroupSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      return StringHelper.RemoveAsAliasesFromSql(this.projection.ToSqlString(criteria, 0, criteriaQuery, enabledFilters));
    }

    public virtual IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return this.projection.GetTypes(criteria, criteriaQuery);
    }

    public virtual IType[] GetTypes(string alias, ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return this.projection.GetTypes(alias, criteria, criteriaQuery);
    }

    public virtual string[] GetColumnAliases(int loc) => this.projection.GetColumnAliases(loc);

    public virtual string[] GetColumnAliases(string alias, int loc) => (string[]) null;

    public virtual string[] Aliases => new string[0];

    public virtual bool IsGrouped => true;

    public bool IsAggregate => this.projection.IsAggregate;

    public TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return this.projection.GetTypedValues(criteria, criteriaQuery);
    }

    public override string ToString() => this.projection.ToString();
  }
}
