// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.ANSIJoinFragment
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.SqlCommand
{
  public class ANSIJoinFragment : JoinFragment
  {
    private SqlStringBuilder buffer = new SqlStringBuilder();
    private readonly SqlStringBuilder conditions = new SqlStringBuilder();

    public override void AddJoin(
      string tableName,
      string alias,
      string[] fkColumns,
      string[] pkColumns,
      JoinType joinType)
    {
      this.AddJoin(tableName, alias, fkColumns, pkColumns, joinType, (SqlString) null);
    }

    public override void AddJoin(
      string tableName,
      string alias,
      string[] fkColumns,
      string[] pkColumns,
      JoinType joinType,
      SqlString on)
    {
      string str;
      switch (joinType)
      {
        case JoinType.InnerJoin:
          str = " inner join ";
          break;
        case JoinType.LeftOuterJoin:
          str = " left outer join ";
          break;
        case JoinType.RightOuterJoin:
          str = " right outer join ";
          break;
        case JoinType.FullJoin:
          str = " full outer join ";
          break;
        default:
          throw new AssertionFailure("undefined join type");
      }
      this.buffer.Add(str + tableName + (object) ' ' + alias + " on ");
      for (int index = 0; index < fkColumns.Length; ++index)
      {
        this.buffer.Add(fkColumns[index] + "=" + alias + (object) '.' + pkColumns[index]);
        if (index < fkColumns.Length - 1)
          this.buffer.Add(" and ");
      }
      this.AddCondition(this.buffer, on);
    }

    public override SqlString ToFromFragmentString => this.buffer.ToSqlString();

    public override SqlString ToWhereFragmentString => this.conditions.ToSqlString();

    public override void AddJoins(SqlString fromFragment, SqlString whereFragment)
    {
      this.buffer.Add(fromFragment);
    }

    public JoinFragment Copy()
    {
      return (JoinFragment) new ANSIJoinFragment()
      {
        buffer = new SqlStringBuilder(this.buffer.ToSqlString())
      };
    }

    public override void AddCrossJoin(string tableName, string alias)
    {
      this.buffer.Add(", " + tableName + " " + alias);
    }

    public override bool AddCondition(string condition)
    {
      return this.AddCondition(this.conditions, condition);
    }

    public override bool AddCondition(SqlString condition)
    {
      return this.AddCondition(this.conditions, condition);
    }

    public override void AddFromFragmentString(SqlString fromFragmentString)
    {
      this.buffer.Add(fromFragmentString);
    }
  }
}
