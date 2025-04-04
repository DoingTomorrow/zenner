// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.IProjection
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Criterion
{
  public interface IProjection
  {
    SqlString ToSqlString(
      ICriteria criteria,
      int position,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters);

    SqlString ToGroupSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters);

    IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery);

    IType[] GetTypes(string alias, ICriteria criteria, ICriteriaQuery criteriaQuery);

    string[] GetColumnAliases(int loc);

    string[] GetColumnAliases(string alias, int loc);

    string[] Aliases { get; }

    bool IsGrouped { get; }

    bool IsAggregate { get; }

    TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery);
  }
}
