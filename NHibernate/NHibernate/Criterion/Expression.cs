// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.Expression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;

#nullable disable
namespace NHibernate.Criterion
{
  public sealed class Expression : Restrictions
  {
    private Expression()
    {
    }

    public static AbstractCriterion Sql(SqlString sql, object[] values, IType[] types)
    {
      return (AbstractCriterion) new SQLCriterion(sql, values, types);
    }

    public static AbstractCriterion Sql(SqlString sql, object value, IType type)
    {
      return Expression.Sql(sql, new object[1]{ value }, new IType[1]
      {
        type
      });
    }

    public static AbstractCriterion Sql(string sql, object value, IType type)
    {
      return Expression.Sql(sql, new object[1]{ value }, new IType[1]
      {
        type
      });
    }

    public static AbstractCriterion Sql(string sql, object[] values, IType[] types)
    {
      return (AbstractCriterion) new SQLCriterion(SqlString.Parse(sql), values, types);
    }

    public static AbstractCriterion Sql(SqlString sql)
    {
      return Expression.Sql(sql, ArrayHelper.EmptyObjectArray, ArrayHelper.EmptyTypeArray);
    }

    public static AbstractCriterion Sql(string sql)
    {
      return Expression.Sql(sql, ArrayHelper.EmptyObjectArray, ArrayHelper.EmptyTypeArray);
    }
  }
}
