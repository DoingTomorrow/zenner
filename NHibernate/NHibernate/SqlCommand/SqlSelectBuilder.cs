// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.SqlSelectBuilder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.Util;

#nullable disable
namespace NHibernate.SqlCommand
{
  public class SqlSelectBuilder(ISessionFactoryImplementor factory) : 
    SqlBaseBuilder(factory.Dialect, (IMapping) factory),
    ISqlStringBuilder
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (SqlSelectBuilder));
    private SqlString selectClause;
    private string fromClause;
    private SqlString outerJoinsAfterFrom;
    private SqlString whereClause;
    private SqlString outerJoinsAfterWhere;
    private SqlString orderByClause;
    private string groupByClause;
    private SqlString havingClause;
    private LockMode lockMode;
    private string comment;

    public SqlSelectBuilder SetComment(string comment)
    {
      this.comment = comment;
      return this;
    }

    public SqlSelectBuilder SetFromClause(string fromClause)
    {
      this.fromClause = fromClause;
      return this;
    }

    public SqlSelectBuilder SetFromClause(string tableName, string alias)
    {
      this.fromClause = tableName + " " + alias;
      return this;
    }

    public SqlSelectBuilder SetFromClause(SqlString fromClause)
    {
      return this.SetFromClause(fromClause.ToString());
    }

    public SqlSelectBuilder SetOrderByClause(SqlString orderByClause)
    {
      this.orderByClause = orderByClause;
      return this;
    }

    public SqlSelectBuilder SetGroupByClause(string groupByClause)
    {
      this.groupByClause = groupByClause;
      return this;
    }

    public SqlSelectBuilder SetOuterJoins(
      SqlString outerJoinsAfterFrom,
      SqlString outerJoinsAfterWhere)
    {
      this.outerJoinsAfterFrom = outerJoinsAfterFrom;
      SqlString sqlString = outerJoinsAfterWhere.Trim();
      if (sqlString.StartsWithCaseInsensitive("and"))
        sqlString = sqlString.Substring(4);
      this.outerJoinsAfterWhere = sqlString;
      return this;
    }

    public SqlSelectBuilder SetSelectClause(SqlString selectClause)
    {
      this.selectClause = selectClause;
      return this;
    }

    public SqlSelectBuilder SetSelectClause(string selectClause)
    {
      this.selectClause = new SqlString(selectClause);
      return this;
    }

    public SqlSelectBuilder SetWhereClause(
      string tableAlias,
      string[] columnNames,
      IType whereType)
    {
      return this.SetWhereClause(this.ToWhereString(tableAlias, columnNames));
    }

    public SqlSelectBuilder SetWhereClause(SqlString whereSqlString)
    {
      this.whereClause = whereSqlString;
      return this;
    }

    public SqlSelectBuilder SetHavingClause(
      string tableAlias,
      string[] columnNames,
      IType whereType)
    {
      return this.SetHavingClause(this.ToWhereString(tableAlias, columnNames));
    }

    public SqlSelectBuilder SetHavingClause(SqlString havingSqlString)
    {
      this.havingClause = havingSqlString;
      return this;
    }

    public SqlSelectBuilder SetLockMode(LockMode lockMode)
    {
      this.lockMode = lockMode;
      return this;
    }

    public SqlString ToStatementString() => this.ToSqlString();

    public SqlString ToSqlString()
    {
      int num = 4 + (this.outerJoinsAfterFrom != null ? this.outerJoinsAfterFrom.Count : 0) + 1 + (this.outerJoinsAfterWhere != null ? this.outerJoinsAfterWhere.Count : 0) + 1 + 2;
      if (!string.IsNullOrEmpty(this.comment))
        ++num;
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder(num + 2);
      if (!string.IsNullOrEmpty(this.comment))
        sqlStringBuilder.Add("/* " + this.comment + " */ ");
      sqlStringBuilder.Add("SELECT ").Add(this.selectClause).Add(" FROM ").Add(this.fromClause);
      if (StringHelper.IsNotEmpty(this.outerJoinsAfterFrom))
        sqlStringBuilder.Add(this.outerJoinsAfterFrom);
      if (StringHelper.IsNotEmpty(this.whereClause) || StringHelper.IsNotEmpty(this.outerJoinsAfterWhere))
      {
        sqlStringBuilder.Add(" WHERE ");
        if (StringHelper.IsNotEmpty(this.outerJoinsAfterWhere))
        {
          sqlStringBuilder.Add(this.outerJoinsAfterWhere);
          if (StringHelper.IsNotEmpty(this.whereClause))
            sqlStringBuilder.Add(" AND ");
        }
        if (StringHelper.IsNotEmpty(this.whereClause))
          sqlStringBuilder.Add(this.whereClause);
      }
      if (StringHelper.IsNotEmpty(this.groupByClause))
        sqlStringBuilder.Add(" GROUP BY ").Add(this.groupByClause);
      if (StringHelper.IsNotEmpty(this.havingClause))
        sqlStringBuilder.Add(" HAVING ").Add(this.havingClause);
      if (StringHelper.IsNotEmpty(this.orderByClause))
        sqlStringBuilder.Add(" ORDER BY ").Add(this.orderByClause);
      if (this.lockMode != null)
        sqlStringBuilder.Add(this.Dialect.GetForUpdateString(this.lockMode));
      if (SqlSelectBuilder.log.IsDebugEnabled)
      {
        if (num < sqlStringBuilder.Count)
          SqlSelectBuilder.log.Debug((object) ("The initial capacity was set too low at: " + (object) num + " for the SelectSqlBuilder that needed a capacity of: " + (object) sqlStringBuilder.Count + " for the table " + this.fromClause));
        else if (num > 16 && (double) num / (double) sqlStringBuilder.Count > 1.2)
          SqlSelectBuilder.log.Debug((object) ("The initial capacity was set too high at: " + (object) num + " for the SelectSqlBuilder that needed a capacity of: " + (object) sqlStringBuilder.Count + " for the table " + this.fromClause));
      }
      return sqlStringBuilder.ToSqlString();
    }
  }
}
