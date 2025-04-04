// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.BetweenExpression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public class BetweenExpression : AbstractCriterion
  {
    private readonly object _hi;
    private readonly object _lo;
    private readonly IProjection _projection;
    private readonly string _propertyName;

    public BetweenExpression(IProjection projection, object lo, object hi)
    {
      this._projection = projection;
      this._lo = lo;
      this._hi = hi;
    }

    public BetweenExpression(string propertyName, object lo, object hi)
    {
      this._propertyName = propertyName;
      this._lo = lo;
      this._hi = hi;
    }

    public override SqlString ToSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      TypedValue[] array1 = ((IEnumerable<TypedValue>) this.GetTypedValues(criteria, criteriaQuery)).ToArray<TypedValue>();
      TypedValue parameter1 = array1[0];
      TypedValue parameter2 = array1[1];
      SqlString[] columnNames = CriterionUtil.GetColumnNames(this._propertyName, this._projection, criteriaQuery, criteria, enabledFilters);
      if (columnNames.Length == 1)
      {
        sqlStringBuilder.Add(columnNames[0]).Add(" between ").Add(criteriaQuery.NewQueryParameter(parameter1).Single<Parameter>()).Add(" and ").Add(criteriaQuery.NewQueryParameter(parameter2).Single<Parameter>());
      }
      else
      {
        bool flag = false;
        Parameter[] array2 = criteriaQuery.NewQueryParameter(parameter1).ToArray<Parameter>();
        for (int index = 0; index < columnNames.Length; ++index)
        {
          if (flag)
            sqlStringBuilder.Add(" AND ");
          flag = true;
          sqlStringBuilder.Add(columnNames[index]).Add(" >= ").Add(array2[index]);
        }
        Parameter[] array3 = criteriaQuery.NewQueryParameter(parameter2).ToArray<Parameter>();
        for (int index = 0; index < columnNames.Length; ++index)
          sqlStringBuilder.Add(" AND ").Add(columnNames[index]).Add(" <= ").Add(array3[index]);
      }
      return sqlStringBuilder.ToSqlString();
    }

    public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return CriterionUtil.GetTypedValues(criteriaQuery, criteria, this._projection, this._propertyName, this._lo, this._hi);
    }

    public override IProjection[] GetProjections()
    {
      if (this._projection == null)
        return (IProjection[]) null;
      return new IProjection[1]{ this._projection };
    }

    public override string ToString()
    {
      return this._propertyName + " between " + this._lo + " and " + this._hi;
    }
  }
}
