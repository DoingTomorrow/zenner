// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.ConditionalFragment
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;

#nullable disable
namespace NHibernate.SqlCommand
{
  public class ConditionalFragment
  {
    private string tableAlias;
    private string[] lhs;
    private object[] rhs;
    private string op = "=";

    public ConditionalFragment SetOp(string op)
    {
      this.op = op;
      return this;
    }

    public ConditionalFragment SetTableAlias(string tableAlias)
    {
      this.tableAlias = tableAlias;
      return this;
    }

    public ConditionalFragment SetCondition(string[] lhs, string[] rhs)
    {
      this.lhs = lhs;
      this.rhs = (object[]) rhs;
      return this;
    }

    public ConditionalFragment SetCondition(string[] lhs, Parameter[] rhs)
    {
      this.lhs = lhs;
      this.rhs = (object[]) rhs;
      return this;
    }

    public ConditionalFragment SetCondition(string[] lhs, string rhs)
    {
      this.lhs = lhs;
      this.rhs = (object[]) ArrayHelper.FillArray(rhs, lhs.Length);
      return this;
    }

    public SqlString ToSqlStringFragment()
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder(this.lhs.Length * 4);
      string sql = this.tableAlias + (object) '.';
      for (int index = 0; index < this.lhs.Length; ++index)
      {
        sqlStringBuilder.Add(sql).Add(this.lhs[index] + this.op);
        sqlStringBuilder.AddObject(this.rhs[index]);
        if (index < this.lhs.Length - 1)
          sqlStringBuilder.Add(" and ");
      }
      return sqlStringBuilder.ToSqlString();
    }
  }
}
