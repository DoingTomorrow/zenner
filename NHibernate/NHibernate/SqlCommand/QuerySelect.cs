// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.QuerySelect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using System.Collections;

#nullable disable
namespace NHibernate.SqlCommand
{
  public class QuerySelect
  {
    private readonly JoinFragment joins;
    private readonly SqlStringBuilder selectBuilder = new SqlStringBuilder();
    private readonly SqlStringBuilder whereBuilder = new SqlStringBuilder();
    private readonly SqlStringBuilder groupBy = new SqlStringBuilder();
    private readonly SqlStringBuilder orderBy = new SqlStringBuilder();
    private readonly SqlStringBuilder having = new SqlStringBuilder();
    private bool distinct;
    private static readonly ISet dontSpace = (ISet) new HashedSet();

    static QuerySelect()
    {
      QuerySelect.dontSpace.Add((object) ".");
      QuerySelect.dontSpace.Add((object) "+");
      QuerySelect.dontSpace.Add((object) "-");
      QuerySelect.dontSpace.Add((object) "/");
      QuerySelect.dontSpace.Add((object) "*");
      QuerySelect.dontSpace.Add((object) "<");
      QuerySelect.dontSpace.Add((object) ">");
      QuerySelect.dontSpace.Add((object) "=");
      QuerySelect.dontSpace.Add((object) "#");
      QuerySelect.dontSpace.Add((object) "~");
      QuerySelect.dontSpace.Add((object) "|");
      QuerySelect.dontSpace.Add((object) "&");
      QuerySelect.dontSpace.Add((object) "<=");
      QuerySelect.dontSpace.Add((object) ">=");
      QuerySelect.dontSpace.Add((object) "=>");
      QuerySelect.dontSpace.Add((object) "=<");
      QuerySelect.dontSpace.Add((object) "!=");
      QuerySelect.dontSpace.Add((object) "<>");
      QuerySelect.dontSpace.Add((object) "!#");
      QuerySelect.dontSpace.Add((object) "!~");
      QuerySelect.dontSpace.Add((object) "!<");
      QuerySelect.dontSpace.Add((object) "!>");
      QuerySelect.dontSpace.Add((object) "(");
      QuerySelect.dontSpace.Add((object) ")");
      QuerySelect.dontSpace.Add((object) new SqlString("."));
      QuerySelect.dontSpace.Add((object) new SqlString("+"));
      QuerySelect.dontSpace.Add((object) new SqlString("-"));
      QuerySelect.dontSpace.Add((object) new SqlString("/"));
      QuerySelect.dontSpace.Add((object) new SqlString("*"));
      QuerySelect.dontSpace.Add((object) new SqlString("<"));
      QuerySelect.dontSpace.Add((object) new SqlString(">"));
      QuerySelect.dontSpace.Add((object) new SqlString("="));
      QuerySelect.dontSpace.Add((object) new SqlString("#"));
      QuerySelect.dontSpace.Add((object) new SqlString("~"));
      QuerySelect.dontSpace.Add((object) new SqlString("|"));
      QuerySelect.dontSpace.Add((object) new SqlString("&"));
      QuerySelect.dontSpace.Add((object) new SqlString("<="));
      QuerySelect.dontSpace.Add((object) new SqlString(">="));
      QuerySelect.dontSpace.Add((object) new SqlString("=>"));
      QuerySelect.dontSpace.Add((object) new SqlString("=<"));
      QuerySelect.dontSpace.Add((object) new SqlString("!="));
      QuerySelect.dontSpace.Add((object) new SqlString("<>"));
      QuerySelect.dontSpace.Add((object) new SqlString("!#"));
      QuerySelect.dontSpace.Add((object) new SqlString("!~"));
      QuerySelect.dontSpace.Add((object) new SqlString("!<"));
      QuerySelect.dontSpace.Add((object) new SqlString("!>"));
      QuerySelect.dontSpace.Add((object) new SqlString("("));
      QuerySelect.dontSpace.Add((object) new SqlString(")"));
    }

    public QuerySelect(NHibernate.Dialect.Dialect dialect)
    {
      this.joins = (JoinFragment) new QueryJoinFragment(dialect, false);
    }

    public JoinFragment JoinFragment => this.joins;

    public void AddSelectFragmentString(SqlString fragment)
    {
      if (fragment.StartsWithCaseInsensitive(","))
        fragment = fragment.Substring(1);
      fragment = fragment.Trim();
      if (fragment.Length <= 0)
        return;
      if (this.selectBuilder.Count > 0)
        this.selectBuilder.Add(", ");
      this.selectBuilder.Add(fragment);
    }

    public void AddSelectColumn(string columnName, string alias)
    {
      this.AddSelectFragmentString(new SqlString(columnName + (object) ' ' + alias));
    }

    public bool Distinct
    {
      get => this.distinct;
      set => this.distinct = value;
    }

    public void SetWhereTokens(ICollection tokens) => this.AppendTokens(this.whereBuilder, tokens);

    public void SetGroupByTokens(ICollection tokens) => this.AppendTokens(this.groupBy, tokens);

    public void SetOrderByTokens(ICollection tokens) => this.AppendTokens(this.orderBy, tokens);

    public void SetHavingTokens(ICollection tokens) => this.AppendTokens(this.having, tokens);

    public void AddOrderBy(string orderBySql)
    {
      if (this.orderBy.Count > 0)
        this.orderBy.Add(", ");
      this.orderBy.Add(orderBySql);
    }

    public SqlString ToQuerySqlString()
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      sqlStringBuilder.Add("select ");
      if (this.distinct)
        sqlStringBuilder.Add("distinct ");
      SqlString sqlString1 = this.joins.ToFromFragmentString;
      if (sqlString1.StartsWithCaseInsensitive(","))
        sqlString1 = sqlString1.Substring(1);
      else if (sqlString1.StartsWithCaseInsensitive(" inner join"))
        sqlString1 = sqlString1.Substring(11);
      sqlStringBuilder.Add(this.selectBuilder.ToSqlString()).Add(" from").Add(sqlString1);
      SqlString sqlString2 = this.joins.ToWhereFragmentString.Trim();
      SqlString sqlString3 = this.whereBuilder.ToSqlString();
      bool flag1 = sqlString2.Count > 0;
      bool flag2 = sqlString3.Count > 0;
      if (flag1 || flag2)
        sqlStringBuilder.Add(" where ");
      if (flag1)
        sqlStringBuilder.Add(sqlString2.Substring(4));
      if (flag2)
      {
        if (flag1)
          sqlStringBuilder.Add(" and (");
        sqlStringBuilder.Add(sqlString3);
        if (flag1)
          sqlStringBuilder.Add(")");
      }
      if (this.groupBy.Count > 0)
        sqlStringBuilder.Add(" group by ").Add(this.groupBy.ToSqlString());
      if (this.having.Count > 0)
        sqlStringBuilder.Add(" having ").Add(this.having.ToSqlString());
      if (this.orderBy.Count > 0)
        sqlStringBuilder.Add(" order by ").Add(this.orderBy.ToSqlString());
      return sqlStringBuilder.ToSqlString();
    }

    private void AppendTokens(SqlStringBuilder builder, ICollection iter)
    {
      bool flag1 = true;
      bool flag2 = false;
      foreach (object obj in (IEnumerable) iter)
      {
        string str = obj as string;
        SqlString sqlString = obj as SqlString;
        bool flag3 = !QuerySelect.dontSpace.Contains(obj);
        bool flag4 = str == null ? sqlString.StartsWithCaseInsensitive("'") : str.StartsWith("'");
        if (flag3 && flag1 && (!flag4 || !flag2))
          builder.Add(" ");
        flag1 = flag3;
        if (obj.Equals((object) "?"))
        {
          Parameter placeholder = Parameter.Placeholder;
          builder.Add(placeholder);
        }
        else
          builder.AddObject(obj);
        flag2 = str == null ? sqlString.EndsWith("'") : str.EndsWith("'");
      }
    }
  }
}
