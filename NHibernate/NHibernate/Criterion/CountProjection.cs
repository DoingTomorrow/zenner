// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.CountProjection
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public class CountProjection : AggregateProjection
  {
    private bool distinct;

    protected internal CountProjection(string prop)
      : base("count", prop)
    {
    }

    protected internal CountProjection(IProjection projection)
      : base("count", projection)
    {
    }

    public override IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return new IType[1]{ (IType) NHibernateUtil.Int32 };
    }

    public override string ToString()
    {
      return !this.distinct ? base.ToString() : "distinct " + base.ToString();
    }

    public override SqlString ToSqlString(
      ICriteria criteria,
      int position,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder().Add("count(");
      if (this.distinct)
        sqlStringBuilder.Add("distinct ");
      string sql = this.projection == null ? criteriaQuery.GetColumn(criteria, this.propertyName) : StringHelper.RemoveAsAliasesFromSql(this.projection.ToSqlString(criteria, position, criteriaQuery, enabledFilters)).ToString();
      sqlStringBuilder.Add(sql).Add(") as y").Add(position.ToString()).Add("_");
      return sqlStringBuilder.ToSqlString();
    }

    public CountProjection SetDistinct()
    {
      this.distinct = true;
      return this;
    }
  }
}
