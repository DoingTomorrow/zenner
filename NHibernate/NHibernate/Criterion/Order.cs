// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.Order
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public class Order
  {
    protected bool ascending;
    protected string propertyName;
    protected IProjection projection;
    private bool ignoreCase;

    public Order(IProjection projection, bool ascending)
    {
      this.projection = projection;
      this.ascending = ascending;
    }

    public Order(string propertyName, bool ascending)
    {
      this.propertyName = propertyName;
      this.ascending = ascending;
    }

    public virtual SqlString ToSqlString(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      if (this.projection != null)
        return SqlString.Empty.Append(StringHelper.RemoveAsAliasesFromSql(this.projection.ToSqlString(criteria, 0, criteriaQuery, (IDictionary<string, NHibernate.IFilter>) new Dictionary<string, NHibernate.IFilter>()))).Append(this.ascending ? " asc" : " desc");
      string[] aliasesUsingProjection = criteriaQuery.GetColumnAliasesUsingProjection(criteria, this.propertyName);
      IType typeUsingProjection = criteriaQuery.GetTypeUsingProjection(criteria, this.propertyName);
      StringBuilder stringBuilder = new StringBuilder();
      ISessionFactoryImplementor factory = criteriaQuery.Factory;
      for (int index = 0; index < aliasesUsingProjection.Length; ++index)
      {
        bool flag = this.ignoreCase && this.IsStringType(typeUsingProjection.SqlTypes((IMapping) factory)[index]);
        if (flag)
          stringBuilder.Append(factory.Dialect.LowercaseFunction).Append("(");
        stringBuilder.Append(aliasesUsingProjection[index]);
        if (flag)
          stringBuilder.Append(")");
        stringBuilder.Append(this.ascending ? " asc" : " desc");
        if (index < aliasesUsingProjection.Length - 1)
          stringBuilder.Append(", ");
      }
      return new SqlString(stringBuilder.ToString());
    }

    public override string ToString()
    {
      return (this.projection != null ? this.projection.ToString() : this.propertyName) + (this.ascending ? " asc" : " desc");
    }

    public static Order Asc(string propertyName) => new Order(propertyName, true);

    public static Order Asc(IProjection projection) => new Order(projection, true);

    public static Order Desc(IProjection projection) => new Order(projection, false);

    public static Order Desc(string propertyName) => new Order(propertyName, false);

    public TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return this.projection != null ? this.projection.GetTypedValues(criteria, criteriaQuery) : new TypedValue[0];
    }

    public Order IgnoreCase()
    {
      this.ignoreCase = true;
      return this;
    }

    private bool IsStringType(SqlType propertyType)
    {
      switch (propertyType.DbType)
      {
        case DbType.AnsiString:
          return true;
        case DbType.String:
          return true;
        case DbType.AnsiStringFixedLength:
          return true;
        case DbType.StringFixedLength:
          return true;
        default:
          return false;
      }
    }
  }
}
