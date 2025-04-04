// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.MsSql2005DialectQueryPager
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlCommand;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Dialect
{
  internal class MsSql2005DialectQueryPager
  {
    private readonly SqlString _sourceQuery;

    public MsSql2005DialectQueryPager(SqlString sourceQuery) => this._sourceQuery = sourceQuery;

    public SqlString PageBy(SqlString offset, SqlString limit)
    {
      return offset == null ? this.PageByLimitOnly(limit) : this.PageByLimitAndOffset(offset, limit);
    }

    private SqlString PageByLimitOnly(SqlString limit)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      int selectInsertPoint = this.GetAfterSelectInsertPoint();
      return sqlStringBuilder.Add(this._sourceQuery.Substring(0, selectInsertPoint)).Add(" TOP (").Add(limit).Add(") ").Add(this._sourceQuery.Substring(selectInsertPoint)).ToSqlString();
    }

    private SqlString PageByLimitAndOffset(SqlString offset, SqlString limit)
    {
      int fromIndex = this.GetFromIndex();
      SqlString sqlString1 = this._sourceQuery.Substring(0, fromIndex);
      List<SqlString> columnsOrAliases;
      Dictionary<SqlString, SqlString> aliasToColumn;
      Dictionary<SqlString, SqlString> columnToAlias;
      NHibernate.Dialect.Dialect.ExtractColumnOrAliasNames(sqlString1, out columnsOrAliases, out aliasToColumn, out columnToAlias);
      int startIndex = this._sourceQuery.LastIndexOfCaseInsensitive(" order by ");
      SqlString sqlString2;
      SqlString[] sortExpressions;
      if (startIndex > 0 && MsSql2005DialectQueryPager.HasMatchingParens((IEnumerable<char>) this._sourceQuery.Substring(startIndex).ToString()))
      {
        sqlString2 = this._sourceQuery.Substring(fromIndex, startIndex - fromIndex).Trim();
        sortExpressions = this._sourceQuery.Substring(startIndex).Trim().Substring(9).SplitWithRegex("(?<!\\([^\\)]*),{1}");
      }
      else
      {
        sqlString2 = this._sourceQuery.Substring(fromIndex).Trim();
        sortExpressions = new SqlString[1]
        {
          new SqlString("CURRENT_TIMESTAMP")
        };
      }
      SqlStringBuilder result = new SqlStringBuilder();
      result.Add("SELECT ");
      if (limit != null)
        result.Add("TOP (").Add(limit).Add(") ");
      else
        result.Add("TOP (" + (object) int.MaxValue + ") ");
      if (this.IsDistinct())
      {
        result.Add(StringHelper.Join(", ", (IEnumerable) columnsOrAliases)).Add(" FROM (SELECT *, ROW_NUMBER() OVER(ORDER BY ");
        MsSql2005DialectQueryPager.AppendSortExpressionsForDistinct(columnToAlias, sortExpressions, result);
        result.Add(") as __hibernate_sort_row ").Add(" FROM (").Add(sqlString1).Add(" ").Add(sqlString2).Add(") as q_) as query WHERE query.__hibernate_sort_row > ").Add(offset).Add(" ORDER BY query.__hibernate_sort_row");
      }
      else
      {
        result.Add(StringHelper.Join(", ", (IEnumerable) columnsOrAliases)).Add(" FROM (").Add(sqlString1).Add(", ROW_NUMBER() OVER(ORDER BY ");
        MsSql2005DialectQueryPager.AppendSortExpressions(aliasToColumn, sortExpressions, result);
        result.Add(") as __hibernate_sort_row ").Add(sqlString2).Add(") as query WHERE query.__hibernate_sort_row > ").Add(offset).Add(" ORDER BY query.__hibernate_sort_row");
      }
      return result.ToSqlString();
    }

    private static SqlString RemoveSortOrderDirection(SqlString sortExpression)
    {
      SqlString sqlString = sortExpression.Trim();
      if (sqlString.EndsWithCaseInsensitive("asc"))
        return sqlString.Substring(0, sqlString.Length - 3).Trim();
      return sqlString.EndsWithCaseInsensitive("desc") ? sqlString.Substring(0, sqlString.Length - 4).Trim() : sqlString.Trim();
    }

    private static void AppendSortExpressions(
      Dictionary<SqlString, SqlString> aliasToColumn,
      SqlString[] sortExpressions,
      SqlStringBuilder result)
    {
      for (int index = 0; index < sortExpressions.Length; ++index)
      {
        if (index > 0)
          result.Add(", ");
        SqlString sqlString = MsSql2005DialectQueryPager.RemoveSortOrderDirection(sortExpressions[index]);
        if (aliasToColumn.ContainsKey(sqlString))
          result.Add(aliasToColumn[sqlString]);
        else
          result.Add(sqlString);
        if (sortExpressions[index].Trim().EndsWithCaseInsensitive("desc"))
          result.Add(" DESC");
      }
    }

    private static void AppendSortExpressionsForDistinct(
      Dictionary<SqlString, SqlString> columnToAlias,
      SqlString[] sortExpressions,
      SqlStringBuilder result)
    {
      for (int index = 0; index < sortExpressions.Length; ++index)
      {
        if (index > 0)
          result.Add(", ");
        SqlString sqlString = MsSql2005DialectQueryPager.RemoveSortOrderDirection(sortExpressions[index]);
        if (sqlString.StartsWithCaseInsensitive("CURRENT_TIMESTAMP"))
        {
          result.Add(sqlString);
        }
        else
        {
          if (!columnToAlias.ContainsKey(sqlString))
            throw new HibernateException("The dialect was unable to perform paging of a statement that requires distinct results, and is ordered by a column that is not included in the result set of the query.");
          result.Add("q_.");
          result.Add(columnToAlias[sqlString]);
        }
        if (sortExpressions[index].Trim().EndsWithCaseInsensitive("desc"))
          result.Add(" DESC");
      }
    }

    private static bool HasMatchingParens(IEnumerable<char> statement)
    {
      int num = 0;
      foreach (char ch in statement)
      {
        switch (ch)
        {
          case '(':
            ++num;
            continue;
          case ')':
            --num;
            continue;
          default:
            continue;
        }
      }
      return num == 0;
    }

    private int GetFromIndex()
    {
      string text = this._sourceQuery.GetSubselectString().ToString();
      int fromIndex = this._sourceQuery.IndexOfCaseInsensitive(text);
      if (fromIndex == -1)
        fromIndex = this._sourceQuery.ToString().ToLowerInvariant().IndexOf(text.ToLowerInvariant());
      return fromIndex;
    }

    private int GetAfterSelectInsertPoint()
    {
      if (this._sourceQuery.StartsWithCaseInsensitive("select distinct"))
        return 15;
      if (this._sourceQuery.StartsWithCaseInsensitive("select"))
        return 6;
      throw new NotSupportedException("The query should start with 'SELECT' or 'SELECT DISTINCT'");
    }

    private bool IsDistinct() => this._sourceQuery.StartsWithCaseInsensitive("select distinct");
  }
}
