// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.NotNullExpression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public class NotNullExpression : AbstractCriterion
  {
    private readonly string _propertyName;
    private IProjection _projection;
    private static readonly TypedValue[] NoValues = new TypedValue[0];

    public NotNullExpression(IProjection projection) => this._projection = projection;

    public NotNullExpression(string propertyName) => this._propertyName = propertyName;

    public override SqlString ToSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      SqlString[] columnNames = CriterionUtil.GetColumnNames(this._propertyName, this._projection, criteriaQuery, criteria, enabledFilters);
      bool flag = false;
      for (int index = 0; index < columnNames.Length; ++index)
      {
        if (flag)
          sqlStringBuilder.Add(" or ");
        flag = true;
        sqlStringBuilder.Add(columnNames[index]).Add(" is not null");
      }
      if (columnNames.Length > 1)
      {
        sqlStringBuilder.Insert(0, "(");
        sqlStringBuilder.Add(")");
      }
      return sqlStringBuilder.ToSqlString();
    }

    public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return this._projection != null ? this._projection.GetTypedValues(criteria, criteriaQuery) : NotNullExpression.NoValues;
    }

    public override IProjection[] GetProjections()
    {
      if (this._projection == null)
        return (IProjection[]) null;
      return new IProjection[1]{ this._projection };
    }

    public override string ToString()
    {
      return ((object) this._projection ?? (object) this._propertyName).ToString() + " is not null";
    }
  }
}
