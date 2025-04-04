// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.CastProjection
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
  public class CastProjection : SimpleProjection
  {
    private readonly IType type;
    private readonly IProjection projection;

    public CastProjection(IType type, IProjection projection)
    {
      this.type = type;
      this.projection = projection;
    }

    public override bool IsAggregate => false;

    public override SqlString ToSqlString(
      ICriteria criteria,
      int position,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      ISessionFactoryImplementor factory = criteriaQuery.Factory;
      SqlType[] sqlTypeArray = this.type.SqlTypes((IMapping) factory);
      if (sqlTypeArray.Length != 1)
        throw new QueryException("invalid Hibernate type for CastProjection");
      string castTypeName = factory.Dialect.GetCastTypeName(sqlTypeArray[0]);
      int position1 = position * this.GetHashCode();
      SqlString sqlString = StringHelper.RemoveAsAliasesFromSql(this.projection.ToSqlString(criteria, position1, criteriaQuery, enabledFilters));
      return new SqlStringBuilder().Add("cast( ").Add(sqlString).Add(" as ").Add(castTypeName).Add(")").Add(" as ").Add(this.GetColumnAliases(position)[0]).ToSqlString();
    }

    public override IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return new IType[1]{ this.type };
    }

    public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return this.projection.GetTypedValues(criteria, criteriaQuery);
    }

    public override bool IsGrouped => this.projection.IsGrouped;

    public override SqlString ToGroupSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      return this.projection.ToGroupSqlString(criteria, criteriaQuery, enabledFilters);
    }
  }
}
