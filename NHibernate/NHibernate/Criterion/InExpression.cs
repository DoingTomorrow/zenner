// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.InExpression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public class InExpression : AbstractCriterion
  {
    private readonly IProjection _projection;
    private readonly string _propertyName;
    private object[] _values;

    public InExpression(IProjection projection, object[] values)
    {
      this._projection = projection;
      this._values = values;
    }

    public InExpression(string propertyName, object[] values)
    {
      this._propertyName = propertyName;
      this._values = values;
    }

    public override IProjection[] GetProjections()
    {
      if (this._projection == null)
        return (IProjection[]) null;
      return new IProjection[1]{ this._projection };
    }

    public override SqlString ToSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      if (this._projection == null)
        this.AssertPropertyIsNotCollection(criteriaQuery, criteria);
      if (this._values.Length == 0)
        return new SqlString("1=0");
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      SqlString[] columnNames = CriterionUtil.GetColumnNames(this._propertyName, this._projection, criteriaQuery, criteria, enabledFilters);
      Parameter[] array = this.GetParameterTypedValues(criteria, criteriaQuery).SelectMany<TypedValue, Parameter>((Func<TypedValue, IEnumerable<Parameter>>) (t => criteriaQuery.NewQueryParameter(t))).ToArray<Parameter>();
      for (int index1 = 0; index1 < columnNames.Length; ++index1)
      {
        SqlString sqlString = columnNames[index1];
        if (index1 > 0)
          sqlStringBuilder.Add(" and ");
        sqlStringBuilder.Add(sqlString).Add(" in (");
        for (int index2 = 0; index2 < this._values.Length; ++index2)
        {
          if (index2 > 0)
            sqlStringBuilder.Add(", ");
          sqlStringBuilder.Add(array[index2]);
        }
        sqlStringBuilder.Add(")");
      }
      return sqlStringBuilder.ToSqlString();
    }

    private void AssertPropertyIsNotCollection(ICriteriaQuery criteriaQuery, ICriteria criteria)
    {
      if (criteriaQuery.GetTypeUsingProjection(criteria, this._propertyName).IsCollectionType)
        throw new QueryException("Cannot use collections with InExpression");
    }

    public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      List<TypedValue> parameterTypedValues = this.GetParameterTypedValues(criteria, criteriaQuery);
      if (this._projection != null)
        parameterTypedValues.InsertRange(0, (IEnumerable<TypedValue>) this._projection.GetTypedValues(criteria, criteriaQuery));
      return parameterTypedValues.ToArray();
    }

    private List<TypedValue> GetParameterTypedValues(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery)
    {
      IType type = this.GetElementType(criteria, criteriaQuery);
      if (!type.IsComponentType)
        return ((IEnumerable<object>) this._values).Select<object, TypedValue>((Func<object, TypedValue>) (v => new TypedValue(type, v, EntityMode.Poco))).ToList<TypedValue>();
      List<TypedValue> parameterTypedValues = new List<TypedValue>();
      IAbstractComponentType abstractComponentType = (IAbstractComponentType) type;
      IType[] subtypes = abstractComponentType.Subtypes;
      for (int index1 = 0; index1 < subtypes.Length; ++index1)
      {
        for (int index2 = 0; index2 < this._values.Length; ++index2)
        {
          object propertyValue = this._values[index2] == null ? (object) null : abstractComponentType.GetPropertyValues(this._values[index2], EntityMode.Poco)[index1];
          parameterTypedValues.Add(new TypedValue(subtypes[index1], propertyValue, EntityMode.Poco));
        }
      }
      return parameterTypedValues;
    }

    private IType GetElementType(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      if (this._projection == null)
        return criteriaQuery.GetTypeUsingProjection(criteria, this._propertyName);
      IType[] types = this._projection.GetTypes(criteria, criteriaQuery);
      return types.Length == 1 ? types[0] : throw new QueryException("Cannot use projections that return more than a single column with InExpression");
    }

    public object[] Values
    {
      get => this._values;
      protected set => this._values = value;
    }

    public override string ToString()
    {
      return ((object) this._projection ?? (object) this._propertyName).ToString() + " in (" + StringHelper.ToString(this._values) + (object) ')';
    }
  }
}
