// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.OracleJoinFragment
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using System;
using System.Text;

#nullable disable
namespace NHibernate.SqlCommand
{
  public class OracleJoinFragment : JoinFragment
  {
    private SqlStringBuilder afterFrom = new SqlStringBuilder();
    private SqlStringBuilder afterWhere = new SqlStringBuilder();
    private static readonly ISet Operators = (ISet) new HashedSet();

    public override void AddJoin(
      string tableName,
      string alias,
      string[] fkColumns,
      string[] pkColumns,
      JoinType joinType)
    {
      this.AddCrossJoin(tableName, alias);
      for (int index = 0; index < fkColumns.Length; ++index)
      {
        this.HasThetaJoins = true;
        this.afterWhere.Add(" and " + fkColumns[index]);
        if (joinType == JoinType.RightOuterJoin || joinType == JoinType.FullJoin)
          this.afterWhere.Add("(+)");
        this.afterWhere.Add("=" + alias + (object) '.' + pkColumns[index]);
        if (joinType == JoinType.LeftOuterJoin || joinType == JoinType.FullJoin)
          this.afterWhere.Add("(+)");
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
      this.AddJoin(tableName, alias, fkColumns, pkColumns, joinType);
      if (joinType == JoinType.InnerJoin)
      {
        this.AddCondition(on);
      }
      else
      {
        if (joinType != JoinType.LeftOuterJoin)
          throw new NotSupportedException("join type not supported by OracleJoinFragment (use Oracle9Dialect)");
        this.AddLeftOuterJoinCondition(on);
      }
    }

    private void AddLeftOuterJoinCondition(SqlString on)
    {
      StringBuilder stringBuilder = new StringBuilder(on.ToString());
      for (int index = 0; index < stringBuilder.Length; ++index)
      {
        char o = stringBuilder[index];
        if (OracleJoinFragment.Operators.Contains((object) o) || o == ' ' && stringBuilder.Length > index + 3 && "is ".Equals(stringBuilder.ToString(index + 1, 3)))
        {
          stringBuilder.Insert(index, "(+)");
          index += 3;
        }
      }
      this.AddCondition(SqlString.Parse(stringBuilder.ToString()));
    }

    static OracleJoinFragment()
    {
      OracleJoinFragment.Operators.Add((object) '=');
      OracleJoinFragment.Operators.Add((object) '<');
      OracleJoinFragment.Operators.Add((object) '>');
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
      this.afterFrom.Add(", ").Add(tableName).Add(" ").Add(alias);
    }

    public override bool AddCondition(string condition)
    {
      return this.AddCondition(this.afterWhere, condition);
    }

    public override bool AddCondition(SqlString condition)
    {
      return this.AddCondition(this.afterWhere, condition);
    }

    public override void AddFromFragmentString(SqlString fromFragmentString)
    {
      this.afterFrom.Add(fromFragmentString);
    }
  }
}
