// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.IdentifierProjection
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public class IdentifierProjection : SimpleProjection
  {
    private bool grouped;

    protected internal IdentifierProjection(bool grouped) => this.grouped = grouped;

    protected internal IdentifierProjection()
      : this(false)
    {
    }

    public override string ToString() => "id";

    public override IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return new IType[1]
      {
        criteriaQuery.GetIdentifierType(criteria)
      };
    }

    public override SqlString ToSqlString(
      ICriteria criteria,
      int position,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      string[] identifierColumns = criteriaQuery.GetIdentifierColumns(criteria);
      for (int index = 0; index < identifierColumns.Length; ++index)
      {
        if (index > 0)
          sqlStringBuilder.Add(", ");
        sqlStringBuilder.Add(identifierColumns[index]).Add(" as y").Add((position + index).ToString()).Add("_");
      }
      return sqlStringBuilder.ToSqlString();
    }

    public override bool IsGrouped => this.grouped;

    public override bool IsAggregate => false;

    public override SqlString ToGroupSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      if (!this.grouped)
        throw new InvalidOperationException("not a grouping projection");
      return new SqlString(StringHelper.Join(",", (IEnumerable) criteriaQuery.GetIdentifierColumns(criteria)));
    }
  }
}
