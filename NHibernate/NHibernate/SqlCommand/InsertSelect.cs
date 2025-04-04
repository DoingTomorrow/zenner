// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.InsertSelect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections.Generic;

#nullable disable
namespace NHibernate.SqlCommand
{
  public class InsertSelect : ISqlStringBuilder
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (InsertSelect));
    private string tableName;
    private string comment;
    private readonly List<string> columnNames = new List<string>();
    private SqlSelectBuilder select;

    public virtual InsertSelect SetTableName(string tableName)
    {
      this.tableName = tableName;
      return this;
    }

    public virtual InsertSelect SetComment(string comment)
    {
      this.comment = comment;
      return this;
    }

    public virtual InsertSelect AddColumn(string columnName)
    {
      this.columnNames.Add(columnName);
      return this;
    }

    public virtual InsertSelect AddColumns(string[] columnNames)
    {
      this.columnNames.AddRange((IEnumerable<string>) columnNames);
      return this;
    }

    public virtual InsertSelect SetSelect(SqlSelectBuilder select)
    {
      this.select = select;
      return this;
    }

    public SqlString ToSqlString()
    {
      if (this.tableName == null)
        throw new HibernateException("no table name defined for insert-select");
      if (this.select == null)
        throw new HibernateException("no select defined for insert-select");
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder(this.columnNames.Count + 4);
      if (this.comment != null)
        sqlStringBuilder.Add("/* " + this.comment + " */ ");
      sqlStringBuilder.Add("insert into ").Add(this.tableName);
      if (this.columnNames.Count != 0)
      {
        sqlStringBuilder.Add(" (");
        bool flag = false;
        foreach (string columnName in this.columnNames)
        {
          if (flag)
            sqlStringBuilder.Add(", ");
          sqlStringBuilder.Add(columnName);
          flag = true;
        }
        sqlStringBuilder.Add(")");
      }
      sqlStringBuilder.Add(" ").Add(this.select.ToStatementString());
      return sqlStringBuilder.ToSqlString();
    }
  }
}
