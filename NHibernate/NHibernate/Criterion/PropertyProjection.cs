// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.PropertyProjection
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
  public class PropertyProjection : SimpleProjection, IPropertyProjection
  {
    private string propertyName;
    private bool grouped;

    protected internal PropertyProjection(string propertyName, bool grouped)
    {
      this.propertyName = propertyName;
      this.grouped = grouped;
    }

    protected internal PropertyProjection(string propertyName)
      : this(propertyName, false)
    {
    }

    public string PropertyName => this.propertyName;

    public override string ToString() => this.propertyName;

    public override bool IsGrouped => this.grouped;

    public override bool IsAggregate => false;

    public override IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
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
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      string[] columnsUsingProjection = criteriaQuery.GetColumnsUsingProjection(criteria, this.propertyName);
      for (int index = 0; index < columnsUsingProjection.Length; ++index)
      {
        sqlStringBuilder.Add(columnsUsingProjection[index]);
        sqlStringBuilder.Add(" as y");
        sqlStringBuilder.Add((loc + index).ToString());
        sqlStringBuilder.Add("_");
        if (index < columnsUsingProjection.Length - 1)
          sqlStringBuilder.Add(", ");
      }
      return sqlStringBuilder.ToSqlString();
    }

    public override SqlString ToGroupSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      if (!this.grouped)
        throw new InvalidOperationException("not a grouping projection");
      return new SqlString(StringHelper.Join(",", (IEnumerable) criteriaQuery.GetColumns(criteria, this.propertyName)));
    }
  }
}
