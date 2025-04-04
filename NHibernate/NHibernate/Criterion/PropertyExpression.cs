// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.PropertyExpression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public abstract class PropertyExpression : AbstractCriterion
  {
    private static readonly TypedValue[] NoTypedValues = new TypedValue[0];
    private readonly string _lhsPropertyName;
    private readonly string _rhsPropertyName;
    private readonly IProjection _lhsProjection;
    private readonly IProjection _rhsProjection;

    protected PropertyExpression(IProjection lhsProjection, string rhsPropertyName)
    {
      this._lhsProjection = lhsProjection;
      this._rhsPropertyName = rhsPropertyName;
    }

    protected PropertyExpression(IProjection lhsProjection, IProjection rhsProjection)
    {
      this._lhsProjection = lhsProjection;
      this._rhsProjection = rhsProjection;
    }

    protected PropertyExpression(string lhsPropertyName, string rhsPropertyName)
    {
      this._lhsPropertyName = lhsPropertyName;
      this._rhsPropertyName = rhsPropertyName;
    }

    protected PropertyExpression(string lhsPropertyName, IProjection rhsProjection)
    {
      this._lhsPropertyName = lhsPropertyName;
      this._rhsProjection = rhsProjection;
    }

    protected abstract string Op { get; }

    public override SqlString ToSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      SqlString[] columnNames1 = CriterionUtil.GetColumnNames(this._lhsPropertyName, this._lhsProjection, criteriaQuery, criteria, enabledFilters);
      SqlString[] columnNames2 = CriterionUtil.GetColumnNames(this._rhsPropertyName, this._rhsProjection, criteriaQuery, criteria, enabledFilters);
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      if (columnNames1.Length > 1)
        sqlStringBuilder.Add("(");
      bool flag = true;
      foreach (SqlString sqlString in StringHelper.Add(columnNames1, this.Op, columnNames2))
      {
        if (!flag)
          sqlStringBuilder.Add(" and ");
        flag = false;
        sqlStringBuilder.Add(sqlString);
      }
      if (columnNames1.Length > 1)
        sqlStringBuilder.Add(")");
      return sqlStringBuilder.ToSqlString();
    }

    public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      if (this._lhsProjection == null && this._rhsProjection == null)
        return PropertyExpression.NoTypedValues;
      List<TypedValue> typedValueList = new List<TypedValue>();
      if (this._lhsProjection != null)
        typedValueList.AddRange((IEnumerable<TypedValue>) this._lhsProjection.GetTypedValues(criteria, criteriaQuery));
      if (this._rhsProjection != null)
        typedValueList.AddRange((IEnumerable<TypedValue>) this._rhsProjection.GetTypedValues(criteria, criteriaQuery));
      return typedValueList.ToArray();
    }

    public override string ToString()
    {
      return ((object) this._lhsProjection ?? (object) this._lhsPropertyName).ToString() + this.Op + this._rhsPropertyName;
    }

    public override IProjection[] GetProjections()
    {
      if (this._lhsProjection != null && this._rhsProjection != null)
        return new IProjection[2]
        {
          this._lhsProjection,
          this._rhsProjection
        };
      if (this._lhsProjection != null)
        return new IProjection[1]{ this._lhsProjection };
      if (this._rhsProjection == null)
        return (IProjection[]) null;
      return new IProjection[1]{ this._rhsProjection };
    }
  }
}
