// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.SQLCriterion
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
  public class SQLCriterion : AbstractCriterion
  {
    private readonly SqlString _sql;
    private readonly TypedValue[] _typedValues;

    public SQLCriterion(SqlString sql, object[] values, IType[] types)
    {
      this._sql = sql;
      this._typedValues = new TypedValue[values.Length];
      for (int index = 0; index < this._typedValues.Length; ++index)
        this._typedValues[index] = new TypedValue(types[index], values[index], EntityMode.Poco);
    }

    public override SqlString ToSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      List<Parameter> list = this._sql.GetParameters().ToList<Parameter>();
      int num = 0;
      for (int index = 0; index < this._typedValues.Length; ++index)
      {
        foreach (Parameter parameter in criteriaQuery.NewQueryParameter(this._typedValues[index]))
          list[num++].BackTrack = parameter.BackTrack;
      }
      return this._sql.Replace("{alias}", criteriaQuery.GetSQLAlias(criteria));
    }

    public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return this._typedValues;
    }

    public override IProjection[] GetProjections() => (IProjection[]) null;

    public override string ToString() => this._sql.ToString();
  }
}
