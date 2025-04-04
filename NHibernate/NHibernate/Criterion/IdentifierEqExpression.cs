// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.IdentifierEqExpression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public class IdentifierEqExpression : AbstractCriterion
  {
    private readonly object value;
    private readonly IProjection _projection;

    public IdentifierEqExpression(IProjection projection) => this._projection = projection;

    public IdentifierEqExpression(object value) => this.value = value;

    public override SqlString ToSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      string[] identifierColumns = criteriaQuery.GetIdentifierColumns(criteria);
      Parameter[] array = ((IEnumerable<TypedValue>) this.GetTypedValues(criteria, criteriaQuery)).SelectMany<TypedValue, Parameter>((Func<TypedValue, IEnumerable<Parameter>>) (t => criteriaQuery.NewQueryParameter(t))).ToArray<Parameter>();
      SqlStringBuilder result = new SqlStringBuilder(4 * identifierColumns.Length + 2);
      if (identifierColumns.Length > 1)
        result.Add("(");
      for (int paramIndex = 0; paramIndex < identifierColumns.Length; ++paramIndex)
      {
        if (paramIndex > 0)
          result.Add(" and ");
        result.Add(identifierColumns[paramIndex]).Add(" = ");
        this.AddValueOrProjection(array, paramIndex, criteria, criteriaQuery, enabledFilters, result);
      }
      if (identifierColumns.Length > 1)
        result.Add(")");
      return result.ToSqlString();
    }

    private void AddValueOrProjection(
      Parameter[] parameters,
      int paramIndex,
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters,
      SqlStringBuilder result)
    {
      if (this._projection == null)
      {
        result.Add(parameters[paramIndex]);
      }
      else
      {
        SqlString sqlString = this._projection.ToSqlString(criteria, this.GetHashCode(), criteriaQuery, enabledFilters);
        result.Add(StringHelper.RemoveAsAliasesFromSql(sqlString));
      }
    }

    public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      if (this._projection != null)
        return this._projection.GetTypedValues(criteria, criteriaQuery);
      return new TypedValue[1]
      {
        criteriaQuery.GetTypedIdentifierValue(criteria, this.value)
      };
    }

    public override IProjection[] GetProjections()
    {
      if (this._projection == null)
        return (IProjection[]) null;
      return new IProjection[1]{ this._projection };
    }

    public override string ToString()
    {
      return (this._projection != null ? (object) this._projection.ToString() : (object) "ID").ToString() + " == " + this.value;
    }
  }
}
