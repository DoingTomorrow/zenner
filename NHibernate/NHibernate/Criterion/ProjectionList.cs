// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.ProjectionList
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public class ProjectionList : IEnhancedProjection, IProjection
  {
    private IList<IProjection> elements = (IList<IProjection>) new List<IProjection>();

    protected internal ProjectionList()
    {
    }

    public ProjectionList Create() => new ProjectionList();

    public ProjectionList Add(IProjection proj)
    {
      this.elements.Add(proj);
      return this;
    }

    public ProjectionList Add(IProjection projection, string alias)
    {
      return this.Add(Projections.Alias(projection, alias));
    }

    public ProjectionList Add<T>(IProjection projection, Expression<Func<T>> alias)
    {
      return this.Add(projection, ExpressionProcessor.FindMemberExpression(alias.Body));
    }

    public IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      IList<IType> typeList = (IList<IType>) new List<IType>(this.Length);
      for (int index = 0; index < this.Length; ++index)
      {
        foreach (IType type in this[index].GetTypes(criteria, criteriaQuery))
          typeList.Add(type);
      }
      IType[] array = new IType[typeList.Count];
      typeList.CopyTo(array, 0);
      return array;
    }

    public SqlString ToSqlString(
      ICriteria criteria,
      int loc,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      for (int index = 0; index < this.Length; ++index)
      {
        IProjection projection = this[index];
        sqlStringBuilder.Add(projection.ToSqlString(criteria, loc, criteriaQuery, enabledFilters));
        loc += ProjectionList.GetColumnAliases(loc, criteria, criteriaQuery, projection).Length;
        if (index < this.elements.Count - 1)
          sqlStringBuilder.Add(", ");
      }
      return sqlStringBuilder.ToSqlString();
    }

    public SqlString ToGroupSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      for (int index = 0; index < this.Length; ++index)
      {
        IProjection projection = this[index];
        if (projection.IsGrouped)
          sqlStringBuilder.Add(projection.ToGroupSqlString(criteria, criteriaQuery, enabledFilters)).Add(", ");
      }
      if (sqlStringBuilder.Count >= 2)
        sqlStringBuilder.RemoveAt(sqlStringBuilder.Count - 1);
      return sqlStringBuilder.ToSqlString();
    }

    public string[] GetColumnAliases(int loc)
    {
      IList<string> stringList = (IList<string>) new List<string>(this.Length);
      for (int index = 0; index < this.Length; ++index)
      {
        string[] columnAliases = this[index].GetColumnAliases(loc);
        foreach (string str in columnAliases)
          stringList.Add(str);
        loc += columnAliases.Length;
      }
      string[] array = new string[stringList.Count];
      stringList.CopyTo(array, 0);
      return array;
    }

    public string[] GetColumnAliases(string alias, int loc)
    {
      for (int index = 0; index < this.Length; ++index)
      {
        string[] columnAliases = this[index].GetColumnAliases(alias, loc);
        if (columnAliases != null)
          return columnAliases;
        loc += this[index].GetColumnAliases(loc).Length;
      }
      return (string[]) null;
    }

    public string[] GetColumnAliases(
      int position,
      ICriteria criteria,
      ICriteriaQuery criteriaQuery)
    {
      IList list = (IList) new ArrayList(this.Length);
      for (int index = 0; index < this.Length; ++index)
      {
        string[] columnAliases = ProjectionList.GetColumnAliases(position, criteria, criteriaQuery, this[index]);
        ArrayHelper.AddAll(list, (IList) columnAliases);
        position += columnAliases.Length;
      }
      return ArrayHelper.ToStringArray((ICollection) list);
    }

    public string[] GetColumnAliases(
      string alias,
      int position,
      ICriteria criteria,
      ICriteriaQuery criteriaQuery)
    {
      for (int index = 0; index < this.Length; ++index)
      {
        string[] columnAliases = ProjectionList.GetColumnAliases(alias, position, criteria, criteriaQuery, this[index]);
        if (columnAliases != null)
          return columnAliases;
        position += ProjectionList.GetColumnAliases(position, criteria, criteriaQuery, this[index]).Length;
      }
      return (string[]) null;
    }

    private static string[] GetColumnAliases(
      int position,
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IProjection projection)
    {
      return !(projection is IEnhancedProjection) ? projection.GetColumnAliases(position) : ((IEnhancedProjection) projection).GetColumnAliases(position, criteria, criteriaQuery);
    }

    private static string[] GetColumnAliases(
      string alias,
      int position,
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IProjection projection)
    {
      return !(projection is IEnhancedProjection) ? projection.GetColumnAliases(alias, position) : ((IEnhancedProjection) projection).GetColumnAliases(alias, position, criteria, criteriaQuery);
    }

    public IType[] GetTypes(string alias, ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      for (int index = 0; index < this.Length; ++index)
      {
        IType[] types = this[index].GetTypes(alias, criteria, criteriaQuery);
        if (types != null)
          return types;
      }
      return (IType[]) null;
    }

    public string[] Aliases
    {
      get
      {
        IList<string> stringList = (IList<string>) new List<string>(this.Length);
        for (int index = 0; index < this.Length; ++index)
        {
          foreach (string alias in this[index].Aliases)
            stringList.Add(alias);
        }
        string[] array = new string[stringList.Count];
        stringList.CopyTo(array, 0);
        return array;
      }
    }

    public IProjection this[int index] => this.elements[index];

    public int Length => this.elements.Count;

    public override string ToString() => this.elements.ToString();

    public bool IsGrouped
    {
      get
      {
        for (int index = 0; index < this.Length; ++index)
        {
          if (this[index].IsGrouped)
            return true;
        }
        return false;
      }
    }

    public bool IsAggregate
    {
      get
      {
        for (int index = 0; index < this.Length; ++index)
        {
          if (this[index].IsAggregate)
            return true;
        }
        return false;
      }
    }

    public TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      List<TypedValue> typedValueList = new List<TypedValue>();
      foreach (IProjection element in (IEnumerable<IProjection>) this.elements)
        typedValueList.AddRange((IEnumerable<TypedValue>) element.GetTypedValues(criteria, criteriaQuery));
      return typedValueList.ToArray();
    }
  }
}
