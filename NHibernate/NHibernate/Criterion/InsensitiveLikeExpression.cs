// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.InsensitiveLikeExpression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public class InsensitiveLikeExpression : AbstractCriterion
  {
    private readonly string propertyName;
    private readonly object value;
    private readonly IProjection projection;

    public InsensitiveLikeExpression(IProjection projection, string value, MatchMode matchMode)
    {
      this.projection = projection;
      this.value = (object) matchMode.ToMatchString(value);
    }

    public InsensitiveLikeExpression(IProjection projection, object value)
    {
      this.projection = projection;
      this.value = value;
    }

    public InsensitiveLikeExpression(string propertyName, object value)
    {
      this.propertyName = propertyName;
      this.value = value;
    }

    public InsensitiveLikeExpression(string propertyName, string value, MatchMode matchMode)
      : this(propertyName, (object) matchMode.ToMatchString(value))
    {
    }

    public override SqlString ToSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      SqlString[] columnNames = CriterionUtil.GetColumnNames(this.propertyName, this.projection, criteriaQuery, criteria, enabledFilters);
      if (columnNames.Length != 1)
        throw new HibernateException("insensitive like may only be used with single-column properties");
      if (criteriaQuery.Factory.Dialect is PostgreSQLDialect)
      {
        sqlStringBuilder.Add(columnNames[0]);
        sqlStringBuilder.Add(" ilike ");
      }
      else
        sqlStringBuilder.Add(criteriaQuery.Factory.Dialect.LowercaseFunction).Add("(").Add(columnNames[0]).Add(")").Add(" like ");
      sqlStringBuilder.Add(criteriaQuery.NewQueryParameter(this.GetParameterTypedValue(criteria, criteriaQuery)).Single<Parameter>());
      return sqlStringBuilder.ToSqlString();
    }

    public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      List<TypedValue> typedValueList = new List<TypedValue>();
      if (this.projection != null)
        typedValueList.AddRange((IEnumerable<TypedValue>) this.projection.GetTypedValues(criteria, criteriaQuery));
      typedValueList.Add(this.GetParameterTypedValue(criteria, criteriaQuery));
      return typedValueList.ToArray();
    }

    public TypedValue GetParameterTypedValue(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      string lower = this.value.ToString().ToLower();
      if (this.projection == null)
        return criteriaQuery.GetTypedValue(criteria, this.propertyName, (object) lower);
      return ((IEnumerable<TypedValue>) CriterionUtil.GetTypedValues(criteriaQuery, criteria, this.projection, (string) null, (object) lower)).Single<TypedValue>();
    }

    public override IProjection[] GetProjections()
    {
      if (this.projection == null)
        return (IProjection[]) null;
      return new IProjection[1]{ this.projection };
    }

    public override string ToString()
    {
      return ((object) this.projection ?? (object) this.propertyName).ToString() + " ilike " + this.value;
    }
  }
}
