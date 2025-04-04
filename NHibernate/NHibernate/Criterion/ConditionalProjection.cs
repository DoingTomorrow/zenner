// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.ConditionalProjection
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

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
  public class ConditionalProjection : SimpleProjection
  {
    private readonly ICriterion criterion;
    private readonly IProjection whenTrue;
    private readonly IProjection whenFalse;

    public ConditionalProjection(ICriterion criterion, IProjection whenTrue, IProjection whenFalse)
    {
      this.whenTrue = whenTrue;
      this.whenFalse = whenFalse;
      this.criterion = criterion;
    }

    public override bool IsAggregate
    {
      get
      {
        IProjection[] projections = this.criterion.GetProjections();
        if (projections != null)
        {
          foreach (IProjection projection in projections)
          {
            if (projection.IsAggregate)
              return true;
          }
        }
        return this.whenFalse.IsAggregate || this.whenTrue.IsAggregate;
      }
    }

    public override SqlString ToSqlString(
      ICriteria criteria,
      int position,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      SqlString sqlString1 = this.criterion.ToSqlString(criteria, criteriaQuery, enabledFilters);
      SqlString sqlString2 = StringHelper.RemoveAsAliasesFromSql(this.whenTrue.ToSqlString(criteria, position + this.GetHashCode() + 1, criteriaQuery, enabledFilters));
      SqlString sqlString3 = StringHelper.RemoveAsAliasesFromSql(this.whenFalse.ToSqlString(criteria, position + this.GetHashCode() + 2, criteriaQuery, enabledFilters));
      return new SqlStringBuilder().Add("(").Add("case when ").Add(sqlString1).Add(" then ").Add(sqlString2).Add(" else ").Add(sqlString3).Add(" end").Add(")").Add(" as ").Add(this.GetColumnAliases(position)[0]).ToSqlString();
    }

    public override IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      IType[] types1 = this.whenTrue.GetTypes(criteria, criteriaQuery);
      IType[] types2 = this.whenFalse.GetTypes(criteria, criteriaQuery);
      bool flag = types1.Length == types2.Length;
      if (types1.Length == types2.Length)
      {
        for (int index = 0; index < types1.Length; ++index)
        {
          if (!types1[index].Equals((object) types2[index]))
          {
            flag = false;
            break;
          }
        }
      }
      if (!flag)
        throw new HibernateException("Both true and false projections must return the same types." + Environment.NewLine + "But True projection returns: [" + StringHelper.Join(", ", (IEnumerable) types1) + "] " + Environment.NewLine + "And False projection returns: [" + StringHelper.Join(", ", (IEnumerable) types2) + "]");
      return types1;
    }

    public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      List<TypedValue> typedValueList = new List<TypedValue>();
      typedValueList.AddRange((IEnumerable<TypedValue>) this.criterion.GetTypedValues(criteria, criteriaQuery));
      typedValueList.AddRange((IEnumerable<TypedValue>) this.whenTrue.GetTypedValues(criteria, criteriaQuery));
      typedValueList.AddRange((IEnumerable<TypedValue>) this.whenFalse.GetTypedValues(criteria, criteriaQuery));
      return typedValueList.ToArray();
    }

    public override bool IsGrouped
    {
      get
      {
        IProjection[] projections = this.criterion.GetProjections();
        if (projections != null)
        {
          foreach (IProjection projection in projections)
          {
            if (projection.IsGrouped)
              return true;
          }
        }
        return this.whenFalse.IsGrouped || this.whenTrue.IsGrouped;
      }
    }

    public override SqlString ToGroupSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      IProjection[] projections = this.criterion.GetProjections();
      if (projections != null)
      {
        foreach (IProjection projection in projections)
        {
          if (projection.IsGrouped)
            sqlStringBuilder.Add(projection.ToGroupSqlString(criteria, criteriaQuery, enabledFilters)).Add(", ");
        }
      }
      if (this.whenFalse.IsGrouped)
        sqlStringBuilder.Add(this.whenFalse.ToGroupSqlString(criteria, criteriaQuery, enabledFilters)).Add(", ");
      if (this.whenTrue.IsGrouped)
        sqlStringBuilder.Add(this.whenTrue.ToGroupSqlString(criteria, criteriaQuery, enabledFilters)).Add(", ");
      if (sqlStringBuilder.Count >= 2)
        sqlStringBuilder.RemoveAt(sqlStringBuilder.Count - 1);
      return sqlStringBuilder.ToSqlString();
    }
  }
}
