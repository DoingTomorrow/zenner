// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.QueryJoinFragment
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.SqlCommand
{
  public class QueryJoinFragment : JoinFragment
  {
    private SqlStringBuilder afterFrom = new SqlStringBuilder();
    private SqlStringBuilder afterWhere = new SqlStringBuilder();
    private NHibernate.Dialect.Dialect dialect;
    private bool useThetaStyleInnerJoins;

    public QueryJoinFragment(NHibernate.Dialect.Dialect dialect, bool useThetaStyleInnerJoins)
    {
      this.dialect = dialect;
      this.useThetaStyleInnerJoins = useThetaStyleInnerJoins;
    }

    public override void AddJoin(
      string tableName,
      string alias,
      string[] fkColumns,
      string[] pkColumns,
      JoinType joinType)
    {
      this.AddJoin(tableName, alias, alias, fkColumns, pkColumns, joinType, (SqlString) null);
    }

    private void AddJoin(
      string tableName,
      string alias,
      string concreteAlias,
      string[] fkColumns,
      string[] pkColumns,
      JoinType joinType,
      SqlString on)
    {
      if (!this.useThetaStyleInnerJoins || joinType != JoinType.InnerJoin)
      {
        JoinFragment outerJoinFragment = this.dialect.CreateOuterJoinFragment();
        outerJoinFragment.AddJoin(tableName, alias, fkColumns, pkColumns, joinType, on);
        this.AddFragment(outerJoinFragment);
      }
      else
      {
        this.AddCrossJoin(tableName, alias);
        this.AddCondition(concreteAlias, fkColumns, pkColumns);
        this.AddCondition(on);
      }
    }

    public override void AddJoin(
      string tableName,
      string alias,
      string[] fkColumns,
      string[] pkColumns,
      JoinType joinType,
      SqlString on)
    {
      this.AddJoin(tableName, alias, alias, fkColumns, pkColumns, joinType, on);
    }

    public override SqlString ToFromFragmentString => this.afterFrom.ToSqlString();

    public override SqlString ToWhereFragmentString => this.afterWhere.ToSqlString();

    public override void AddJoins(SqlString fromFragment, SqlString whereFragment)
    {
      this.afterFrom.Add(fromFragment);
      this.afterWhere.Add(whereFragment);
    }

    public override void AddCrossJoin(string tableName, string alias)
    {
      this.afterFrom.Add(", " + tableName + (object) ' ' + alias);
    }

    private void AddCondition(string alias, string[] fkColumns, string[] pkColumns)
    {
      for (int index = 0; index < fkColumns.Length; ++index)
        this.afterWhere.Add(" and ").Add(fkColumns[index]).Add("=").Add(alias).Add(".").Add(pkColumns[index]);
    }

    public override bool AddCondition(string condition)
    {
      string str = condition.Trim();
      if (this.afterFrom.ToSqlString().ToString().IndexOf(str) >= 0 || this.afterWhere.ToSqlString().ToString().IndexOf(str) >= 0)
        return false;
      if (!condition.StartsWith(" and "))
        this.afterWhere.Add(" and ");
      this.afterWhere.Add(condition);
      return true;
    }

    public override bool AddCondition(SqlString condition)
    {
      string str = condition.Trim().ToString();
      if (this.afterFrom.ToString().IndexOf(str) >= 0 || this.afterWhere.ToString().IndexOf(str) >= 0)
        return false;
      if (!condition.StartsWithCaseInsensitive(" and "))
        this.afterWhere.Add(" and ");
      this.afterWhere.Add(condition);
      return true;
    }

    public override void AddFromFragmentString(SqlString fromFragmentString)
    {
      this.afterFrom.Add(fromFragmentString);
    }

    public void ClearWherePart() => this.afterWhere.Clear();
  }
}
