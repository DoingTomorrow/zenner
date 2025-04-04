// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.LikeExpression
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
  public class LikeExpression : AbstractCriterion
  {
    private readonly string value;
    private char? escapeChar;
    private readonly bool ignoreCase;
    private readonly IProjection projection;
    private readonly TypedValue typedValue;

    public LikeExpression(string propertyName, string value, char? escapeChar, bool ignoreCase)
    {
      this.projection = (IProjection) Projections.Property(propertyName);
      this.value = value;
      this.typedValue = new TypedValue((IType) NHibernateUtil.String, (object) this.value, EntityMode.Poco);
      this.escapeChar = escapeChar;
      this.ignoreCase = ignoreCase;
    }

    public LikeExpression(IProjection projection, string value, MatchMode matchMode)
    {
      this.projection = projection;
      this.value = matchMode.ToMatchString(value);
      this.typedValue = new TypedValue((IType) NHibernateUtil.String, (object) this.value, EntityMode.Poco);
    }

    public LikeExpression(string propertyName, string value)
      : this(propertyName, value, new char?(), false)
    {
    }

    public LikeExpression(string propertyName, string value, MatchMode matchMode)
      : this(propertyName, matchMode.ToMatchString(value))
    {
    }

    public LikeExpression(
      string propertyName,
      string value,
      MatchMode matchMode,
      char? escapeChar,
      bool ignoreCase)
      : this(propertyName, matchMode.ToMatchString(value), escapeChar, ignoreCase)
    {
    }

    public override SqlString ToSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      SqlString[] namesUsingProjection = CriterionUtil.GetColumnNamesUsingProjection(this.projection, criteriaQuery, criteria, enabledFilters);
      if (namesUsingProjection.Length != 1)
        throw new HibernateException("Like may only be used with single-column properties / projections.");
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder(6);
      if (this.ignoreCase)
      {
        NHibernate.Dialect.Dialect dialect = criteriaQuery.Factory.Dialect;
        sqlStringBuilder.Add(dialect.LowercaseFunction).Add("(").Add(namesUsingProjection[0]).Add(")");
      }
      else
        sqlStringBuilder.Add(namesUsingProjection[0]);
      if (this.ignoreCase)
      {
        NHibernate.Dialect.Dialect dialect = criteriaQuery.Factory.Dialect;
        sqlStringBuilder.Add(" like ").Add(dialect.LowercaseFunction).Add("(").Add(criteriaQuery.NewQueryParameter(this.typedValue).Single<Parameter>()).Add(")");
      }
      else
        sqlStringBuilder.Add(" like ").Add(criteriaQuery.NewQueryParameter(this.typedValue).Single<Parameter>());
      if (this.escapeChar.HasValue)
        sqlStringBuilder.Add(" escape '" + (object) this.escapeChar + "'");
      return sqlStringBuilder.ToSqlString();
    }

    public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return new TypedValue[1]{ this.typedValue };
    }

    public override IProjection[] GetProjections()
    {
      return new IProjection[1]{ this.projection };
    }

    public override string ToString() => this.projection.ToString() + " like " + this.value;
  }
}
