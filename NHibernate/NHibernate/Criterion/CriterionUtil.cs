// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.CriterionUtil
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Criterion
{
  public static class CriterionUtil
  {
    public static SqlString[] GetColumnNames(
      string propertyName,
      IProjection projection,
      ICriteriaQuery criteriaQuery,
      ICriteria criteria,
      IDictionary<string, IFilter> enabledFilters)
    {
      return projection == null ? CriterionUtil.GetColumnNamesUsingPropertyName(criteriaQuery, criteria, propertyName) : CriterionUtil.GetColumnNamesUsingProjection(projection, criteriaQuery, criteria, enabledFilters);
    }

    public static SqlString[] GetColumnNamesForSimpleExpression(
      string propertyName,
      IProjection projection,
      ICriteriaQuery criteriaQuery,
      ICriteria criteria,
      IDictionary<string, IFilter> enabledFilters,
      ICriterion criterion,
      object value)
    {
      return projection == null ? CriterionUtil.GetColumnNamesUsingPropertyName(criteriaQuery, criteria, propertyName, value, criterion) : CriterionUtil.GetColumnNamesUsingProjection(projection, criteriaQuery, criteria, enabledFilters);
    }

    internal static SqlString[] GetColumnNamesUsingProjection(
      IProjection projection,
      ICriteriaQuery criteriaQuery,
      ICriteria criteria,
      IDictionary<string, IFilter> enabledFilters)
    {
      return new SqlString[1]
      {
        StringHelper.RemoveAsAliasesFromSql(projection.ToSqlString(criteria, criteriaQuery.GetIndexForAlias(), criteriaQuery, enabledFilters))
      };
    }

    private static SqlString[] GetColumnNamesUsingPropertyName(
      ICriteriaQuery criteriaQuery,
      ICriteria criteria,
      string propertyName)
    {
      return Array.ConvertAll<string, SqlString>(criteriaQuery.GetColumnsUsingProjection(criteria, propertyName), (Converter<string, SqlString>) (input => new SqlString(input)));
    }

    private static SqlString[] GetColumnNamesUsingPropertyName(
      ICriteriaQuery criteriaQuery,
      ICriteria criteria,
      string propertyName,
      object value,
      ICriterion critertion)
    {
      string[] columnsUsingProjection = criteriaQuery.GetColumnsUsingProjection(criteria, propertyName);
      IType typeUsingProjection = criteriaQuery.GetTypeUsingProjection(criteria, propertyName);
      if (value != null && !(value is System.Type) && !typeUsingProjection.ReturnedClass.IsInstanceOfType(value))
        throw new QueryException(string.Format("Type mismatch in {0}: {1} expected type {2}, actual type {3}", (object) critertion.GetType(), (object) propertyName, (object) typeUsingProjection.ReturnedClass, (object) value.GetType()));
      if (typeUsingProjection.IsCollectionType)
        throw new QueryException(string.Format("cannot use collection property ({0}.{1}) directly in a criterion, use ICriteria.CreateCriteria instead", (object) criteriaQuery.GetEntityName(criteria), (object) propertyName));
      return Array.ConvertAll<string, SqlString>(columnsUsingProjection, (Converter<string, SqlString>) (col => new SqlString(col)));
    }

    public static TypedValue[] GetTypedValues(
      ICriteriaQuery criteriaQuery,
      ICriteria criteria,
      IProjection projection,
      string propertyName,
      params object[] values)
    {
      List<TypedValue> typedValueList = new List<TypedValue>();
      IPropertyProjection propertyProjection = projection as IPropertyProjection;
      if (projection == null || propertyProjection != null)
      {
        string propertyPath = propertyProjection != null ? propertyProjection.PropertyName : propertyName;
        foreach (object obj in values)
        {
          TypedValue typedValue = criteriaQuery.GetTypedValue(criteria, propertyPath, obj);
          typedValueList.Add(typedValue);
        }
      }
      else
      {
        foreach (object obj in values)
          typedValueList.Add(new TypedValue(NHibernateUtil.GuessType(obj), obj, EntityMode.Poco));
      }
      return typedValueList.ToArray();
    }
  }
}
