// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.AliasedProjection
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
  public class AliasedProjection : IEnhancedProjection, IProjection
  {
    private readonly IProjection projection;
    private readonly string alias;

    protected internal AliasedProjection(IProjection projection, string alias)
    {
      this.projection = projection;
      this.alias = alias;
    }

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
      return this.projection.ToGroupSqlString(criteria, criteriaQuery, enabledFilters);
    }

    public virtual IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return this.projection.GetTypes(criteria, criteriaQuery);
    }

    public virtual IType[] GetTypes(string alias, ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return !this.alias.Equals(alias) ? (IType[]) null : this.GetTypes(criteria, criteriaQuery);
    }

    public virtual string[] GetColumnAliases(int loc) => this.projection.GetColumnAliases(loc);

    public virtual string[] GetColumnAliases(string alias, int loc)
    {
      return !this.alias.Equals(alias) ? (string[]) null : this.GetColumnAliases(loc);
    }

    public string[] GetColumnAliases(
      int position,
      ICriteria criteria,
      ICriteriaQuery criteriaQuery)
    {
      return !(this.projection is IEnhancedProjection) ? this.GetColumnAliases(position) : ((IEnhancedProjection) this.projection).GetColumnAliases(position, criteria, criteriaQuery);
    }

    public string[] GetColumnAliases(
      string alias,
      int position,
      ICriteria criteria,
      ICriteriaQuery criteriaQuery)
    {
      return !this.alias.Equals(alias) ? (string[]) null : this.GetColumnAliases(position, criteria, criteriaQuery);
    }

    public virtual string[] Aliases
    {
      get => new string[1]{ this.alias };
    }

    public virtual bool IsGrouped => this.projection.IsGrouped;

    public bool IsAggregate => this.projection.IsAggregate;

    public TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return this.projection.GetTypedValues(criteria, criteriaQuery);
    }

    public override string ToString() => this.projection.ToString() + " as " + this.alias;
  }
}
