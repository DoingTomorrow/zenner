// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.InformixJoinFragment
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.SqlCommand
{
  public class InformixJoinFragment : JoinFragment
  {
    private readonly SqlStringBuilder afterFrom = new SqlStringBuilder();
    private readonly SqlStringBuilder afterWhere = new SqlStringBuilder();

    public override void AddJoin(
      string tableName,
      string alias,
      string[] fkColumns,
      string[] pkColumns,
      JoinType joinType)
    {
      switch (joinType)
      {
        case JoinType.InnerJoin:
          this.AddCrossJoin(tableName, alias);
          break;
        case JoinType.LeftOuterJoin:
          this.afterFrom.Add(", ").Add("outer ").Add(tableName).Add(" ").Add(alias);
          break;
        case JoinType.RightOuterJoin:
          this.afterFrom.Insert(InformixJoinFragment.GetPrevTableInsertPoint(this.afterFrom.ToSqlString()), "outer ");
          break;
        case JoinType.FullJoin:
          throw new NotSupportedException("join type not supported by Informix");
        default:
          throw new AssertionFailure("undefined join type");
      }
      for (int index = 0; index < fkColumns.Length; ++index)
      {
        this.HasThetaJoins = true;
        this.afterWhere.Add(" and " + fkColumns[index]);
        this.afterWhere.Add("=" + alias + (object) '.' + pkColumns[index]);
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
      this.AddCondition(on);
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

    private static int GetPrevTableInsertPoint(SqlString text)
    {
      int num1 = text.LastIndexOfCaseInsensitive("from");
      int num2 = text.LastIndexOfCaseInsensitive(",");
      if (num1 == -1 && num2 == -1)
        return -1;
      return num2 > num1 ? num2 + 1 : num1 + 5;
    }
  }
}
