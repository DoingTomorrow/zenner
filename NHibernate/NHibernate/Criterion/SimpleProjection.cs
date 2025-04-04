// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.SimpleProjection
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
  public abstract class SimpleProjection : IEnhancedProjection, IProjection
  {
    public IProjection As(string alias) => Projections.Alias((IProjection) this, alias);

    public virtual string[] GetColumnAliases(string alias, int loc) => (string[]) null;

    public virtual IType[] GetTypes(string alias, ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return (IType[]) null;
    }

    public virtual string[] GetColumnAliases(int loc)
    {
      return new string[1]{ "y" + (object) loc + "_" };
    }

    public string[] GetColumnAliases(
      string alias,
      int position,
      ICriteria criteria,
      ICriteriaQuery criteriaQuery)
    {
      return this.GetColumnAliases(alias, position);
    }

    public string[] GetColumnAliases(
      int position,
      ICriteria criteria,
      ICriteriaQuery criteriaQuery)
    {
      int columnCount = this.GetColumnCount(criteria, criteriaQuery);
      string[] columnAliases = new string[columnCount];
      for (int index = 0; index < columnCount; ++index)
      {
        columnAliases[index] = "y" + (object) position + "_";
        ++position;
      }
      return columnAliases;
    }

    public virtual string[] Aliases => new string[1];

    public abstract bool IsGrouped { get; }

    public abstract SqlString ToGroupSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters);

    public abstract bool IsAggregate { get; }

    public virtual TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return new TypedValue[0];
    }

    public abstract SqlString ToSqlString(
      ICriteria criteria,
      int position,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters);

    public abstract IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery);

    private int GetColumnCount(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      IType[] types = this.GetTypes(criteria, criteriaQuery);
      int columnCount = 0;
      for (int index = 0; index < types.Length; ++index)
        columnCount += types[index].GetColumnSpan((IMapping) criteriaQuery.Factory);
      return columnCount;
    }
  }
}
