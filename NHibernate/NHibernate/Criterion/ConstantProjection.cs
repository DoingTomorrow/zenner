// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.ConstantProjection
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public class ConstantProjection : SimpleProjection
  {
    private readonly object value;
    private readonly TypedValue typedValue;

    public ConstantProjection(object value)
      : this(value, NHibernateUtil.GuessType(value.GetType()))
    {
    }

    public ConstantProjection(object value, IType type)
    {
      this.value = value;
      this.typedValue = new TypedValue(type, this.value, EntityMode.Poco);
    }

    public override bool IsAggregate => false;

    public override SqlString ToGroupSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      throw new InvalidOperationException("not a grouping projection");
    }

    public override bool IsGrouped => false;

    public override SqlString ToSqlString(
      ICriteria criteria,
      int position,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      return new SqlStringBuilder().Add(criteriaQuery.NewQueryParameter(this.typedValue).Single<Parameter>()).Add(" as ").Add(this.GetColumnAliases(position)[0]).ToSqlString();
    }

    public override IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return new IType[1]{ this.typedValue.Type };
    }

    public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return new TypedValue[1]{ this.typedValue };
    }
  }
}
