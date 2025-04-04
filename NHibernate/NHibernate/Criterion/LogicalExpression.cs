// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.LogicalExpression
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
  public abstract class LogicalExpression : AbstractCriterion
  {
    private ICriterion _lhs;
    private ICriterion _rhs;

    protected LogicalExpression(ICriterion lhs, ICriterion rhs)
    {
      this._lhs = lhs;
      this._rhs = rhs;
    }

    protected ICriterion LeftHandSide => this._lhs;

    protected ICriterion RightHandSide => this._rhs;

    public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      TypedValue[] typedValues1 = this._lhs.GetTypedValues(criteria, criteriaQuery);
      TypedValue[] typedValues2 = this._rhs.GetTypedValues(criteria, criteriaQuery);
      TypedValue[] destinationArray = new TypedValue[typedValues1.Length + typedValues2.Length];
      Array.Copy((Array) typedValues1, 0, (Array) destinationArray, 0, typedValues1.Length);
      Array.Copy((Array) typedValues2, 0, (Array) destinationArray, typedValues1.Length, typedValues2.Length);
      return destinationArray;
    }

    public override IProjection[] GetProjections() => (IProjection[]) null;

    public override SqlString ToSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      SqlString sqlString1 = this._lhs.ToSqlString(criteria, criteriaQuery, enabledFilters);
      SqlString sqlString2 = this._rhs.ToSqlString(criteria, criteriaQuery, enabledFilters);
      sqlStringBuilder.Add(new SqlString[2]
      {
        sqlString1,
        sqlString2
      }, "(", this.Op, ")", false);
      return sqlStringBuilder.ToSqlString();
    }

    protected abstract string Op { get; }

    public override string ToString()
    {
      return '('.ToString() + this._lhs.ToString() + (object) ' ' + this.Op + (object) ' ' + this._rhs.ToString() + (object) ')';
    }
  }
}
