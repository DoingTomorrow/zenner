// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.SqlBaseBuilder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;

#nullable disable
namespace NHibernate.SqlCommand
{
  public abstract class SqlBaseBuilder
  {
    private readonly NHibernate.Dialect.Dialect dialect;
    private readonly IMapping mapping;

    protected SqlBaseBuilder(NHibernate.Dialect.Dialect dialect, IMapping mapping)
    {
      this.dialect = dialect;
      this.mapping = mapping;
    }

    protected IMapping Mapping => this.mapping;

    public NHibernate.Dialect.Dialect Dialect => this.dialect;

    protected SqlString ToWhereString(string[] columnNames)
    {
      return this.ToWhereString(columnNames, " = ");
    }

    protected SqlString ToWhereString(string tableAlias, string[] columnNames)
    {
      return this.ToWhereString(tableAlias, columnNames, " = ");
    }

    protected SqlString ToWhereString(string[] columnNames, string op)
    {
      return this.ToWhereString((string) null, columnNames, op);
    }

    protected SqlString ToWhereString(string tableAlias, string[] columnNames, string op)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder(columnNames.Length * 2 + 5);
      bool flag = false;
      for (int index = 0; index < columnNames.Length; ++index)
      {
        if (!string.IsNullOrEmpty(columnNames[index]))
        {
          if (flag)
            sqlStringBuilder.Add(" AND ");
          flag = true;
          string sql = tableAlias == null || tableAlias.Length <= 0 ? columnNames[index] : tableAlias + (object) '.' + columnNames[index];
          sqlStringBuilder.Add(sql).Add(op).AddParameter();
        }
      }
      return sqlStringBuilder.ToSqlString();
    }

    protected SqlString ToWhereString(string columnName, string op)
    {
      return string.IsNullOrEmpty(columnName) ? (SqlString) null : new SqlStringBuilder(3).Add(columnName).Add(op).AddParameter().ToSqlString();
    }
  }
}
