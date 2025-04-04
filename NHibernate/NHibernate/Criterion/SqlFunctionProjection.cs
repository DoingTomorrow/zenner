// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.SqlFunctionProjection
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect.Function;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public class SqlFunctionProjection : SimpleProjection
  {
    private readonly IProjection[] args;
    private readonly ISQLFunction function;
    private readonly string functionName;
    private readonly IType returnType;

    public SqlFunctionProjection(string functionName, IType returnType, params IProjection[] args)
    {
      this.functionName = functionName;
      this.returnType = returnType;
      this.args = args;
    }

    public SqlFunctionProjection(
      ISQLFunction function,
      IType returnType,
      params IProjection[] args)
    {
      this.function = function;
      this.returnType = returnType;
      this.args = args;
    }

    public override bool IsAggregate => false;

    public override bool IsGrouped
    {
      get
      {
        foreach (IProjection projection in this.args)
        {
          if (projection.IsGrouped)
            return true;
        }
        return false;
      }
    }

    public override SqlString ToGroupSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      foreach (IProjection projection in this.args)
      {
        if (projection.IsGrouped)
          sqlStringBuilder.Add(projection.ToGroupSqlString(criteria, criteriaQuery, enabledFilters)).Add(", ");
      }
      if (sqlStringBuilder.Count >= 2)
        sqlStringBuilder.RemoveAt(sqlStringBuilder.Count - 1);
      return sqlStringBuilder.ToSqlString();
    }

    public override SqlString ToSqlString(
      ICriteria criteria,
      int position,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      ISQLFunction function = this.GetFunction(criteriaQuery);
      ArrayList args = new ArrayList();
      for (int index = 0; index < this.args.Length; ++index)
      {
        SqlString projectionArgument = SqlFunctionProjection.GetProjectionArgument(criteriaQuery, criteria, this.args[index], 0, enabledFilters);
        args.Add((object) projectionArgument);
      }
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      sqlStringBuilder.Add(function.Render((IList) args, criteriaQuery.Factory));
      sqlStringBuilder.Add(" as ");
      sqlStringBuilder.Add(this.GetColumnAliases(position)[0]);
      return sqlStringBuilder.ToSqlString();
    }

    private ISQLFunction GetFunction(ICriteriaQuery criteriaQuery)
    {
      if (this.function != null)
        return this.function;
      return criteriaQuery.Factory.SQLFunctionRegistry.FindSQLFunction(this.functionName) ?? throw new HibernateException("Current dialect " + (object) criteriaQuery.Factory.Dialect + " doesn't support the function: " + this.functionName);
    }

    private static SqlString GetProjectionArgument(
      ICriteriaQuery criteriaQuery,
      ICriteria criteria,
      IProjection projection,
      int loc,
      IDictionary<string, IFilter> enabledFilters)
    {
      return StringHelper.RemoveAsAliasesFromSql(projection.ToSqlString(criteria, loc, criteriaQuery, enabledFilters));
    }

    public override IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return new IType[1]
      {
        this.GetFunction(criteriaQuery).ReturnType(this.returnType, (IMapping) criteriaQuery.Factory)
      };
    }

    public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      List<TypedValue> typedValueList = new List<TypedValue>();
      foreach (IProjection projection in this.args)
      {
        TypedValue[] typedValues = projection.GetTypedValues(criteria, criteriaQuery);
        typedValueList.AddRange((IEnumerable<TypedValue>) typedValues);
      }
      return typedValueList.ToArray();
    }
  }
}
