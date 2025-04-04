// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.AvgProjection
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public class AvgProjection : AggregateProjection
  {
    public AvgProjection(IProjection projection)
      : base("avg", projection)
    {
    }

    public AvgProjection(string propertyName)
      : base("avg", propertyName)
    {
    }

    public override SqlString ToSqlString(
      ICriteria criteria,
      int loc,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      ISessionFactoryImplementor factory = criteriaQuery.Factory;
      SqlType[] sqlTypeArray = NHibernateUtil.Double.SqlTypes((IMapping) factory);
      string castTypeName = factory.Dialect.GetCastTypeName(sqlTypeArray[0]);
      return new SqlString(string.Format("{0}(cast({1} as {2})) as {3}", (object) this.aggregate, (object) (this.projection == null ? criteriaQuery.GetColumn(criteria, this.propertyName) : StringHelper.RemoveAsAliasesFromSql(this.projection.ToSqlString(criteria, loc, criteriaQuery, enabledFilters)).ToString()), (object) castTypeName, (object) this.GetColumnAliases(loc)[0]));
    }

    public override IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return new IType[1]{ (IType) NHibernateUtil.Double };
    }
  }
}
