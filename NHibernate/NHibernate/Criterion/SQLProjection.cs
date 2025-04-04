// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.SQLProjection
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
  public sealed class SQLProjection : IProjection
  {
    private readonly string sql;
    private readonly string groupBy;
    private readonly IType[] types;
    private readonly string[] aliases;
    private readonly string[] columnAliases;
    private readonly bool grouped;

    internal SQLProjection(string sql, string[] columnAliases, IType[] types)
      : this(sql, (string) null, columnAliases, types)
    {
    }

    internal SQLProjection(string sql, string groupBy, string[] columnAliases, IType[] types)
    {
      this.sql = sql;
      this.types = types;
      this.aliases = columnAliases;
      this.columnAliases = columnAliases;
      this.grouped = groupBy != null;
      this.groupBy = groupBy;
    }

    public SqlString ToSqlString(
      ICriteria criteria,
      int loc,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      return new SqlString(StringHelper.Replace(this.sql, "{alias}", criteriaQuery.GetSQLAlias(criteria)));
    }

    public SqlString ToGroupSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      return new SqlString(StringHelper.Replace(this.groupBy, "{alias}", criteriaQuery.GetSQLAlias(criteria)));
    }

    public override string ToString() => this.sql;

    public IType[] GetTypes(ICriteria crit, ICriteriaQuery criteriaQuery) => this.types;

    public string[] Aliases => this.aliases;

    public string[] GetColumnAliases(int loc) => this.columnAliases;

    public bool IsGrouped => this.grouped;

    public bool IsAggregate => false;

    public TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return new TypedValue[0];
    }

    public IType[] GetTypes(string alias, ICriteria crit, ICriteriaQuery criteriaQuery)
    {
      return (IType[]) null;
    }

    public string[] GetColumnAliases(string alias, int loc) => (string[]) null;
  }
}
