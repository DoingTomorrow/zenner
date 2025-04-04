// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.RowCountProjection
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlCommand;
using NHibernate.Type;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public class RowCountProjection : SimpleProjection
  {
    protected internal RowCountProjection()
    {
    }

    public override bool IsAggregate => true;

    public override IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return new IType[1]{ (IType) NHibernateUtil.Int32 };
    }

    public override SqlString ToSqlString(
      ICriteria criteria,
      int position,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      return new SqlStringBuilder().Add("count(*) as y").Add(position.ToString()).Add("_").ToSqlString();
    }

    public override string ToString() => "count(*)";

    public override bool IsGrouped => false;

    public override SqlString ToGroupSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      throw new InvalidOperationException("not a grouping projection");
    }
  }
}
