// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.SqlSimpleSelectBuilder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Type;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.SqlCommand
{
  public class SqlSimpleSelectBuilder(NHibernate.Dialect.Dialect dialect, IMapping factory) : 
    SqlBaseBuilder(dialect, factory),
    ISqlStringBuilder
  {
    private string tableName;
    private readonly IList<string> columnNames = (IList<string>) new List<string>();
    private readonly IDictionary<string, string> aliases = (IDictionary<string, string>) new Dictionary<string, string>();
    private LockMode lockMode = LockMode.Read;
    private string comment;
    private readonly List<SqlString> whereStrings = new List<SqlString>();
    private string orderBy;

    public SqlSimpleSelectBuilder SetTableName(string tableName)
    {
      this.tableName = tableName;
      return this;
    }

    public SqlSimpleSelectBuilder AddColumn(string columnName)
    {
      this.columnNames.Add(columnName);
      return this;
    }

    public SqlSimpleSelectBuilder AddColumn(string columnName, string alias)
    {
      this.columnNames.Add(columnName);
      this.aliases[columnName] = alias;
      return this;
    }

    public SqlSimpleSelectBuilder AddColumns(string[] columnNames)
    {
      for (int index = 0; index < columnNames.Length; ++index)
      {
        if (columnNames[index] != null)
          this.AddColumn(columnNames[index]);
      }
      return this;
    }

    public SqlSimpleSelectBuilder AddColumns(string[] columnNames, string[] aliases)
    {
      for (int index = 0; index < columnNames.Length; ++index)
      {
        if (columnNames[index] != null)
          this.AddColumn(columnNames[index], aliases[index]);
      }
      return this;
    }

    public SqlSimpleSelectBuilder AddColumns(string[] columns, string[] aliases, bool[] ignore)
    {
      for (int index = 0; index < ignore.Length; ++index)
      {
        if (!ignore[index] && columns[index] != null)
          this.AddColumn(columns[index], aliases[index]);
      }
      return this;
    }

    public virtual SqlSimpleSelectBuilder SetLockMode(LockMode lockMode)
    {
      this.lockMode = lockMode;
      return this;
    }

    public string GetAlias(string columnName)
    {
      string alias;
      this.aliases.TryGetValue(columnName, out alias);
      return alias;
    }

    public SqlSimpleSelectBuilder SetIdentityColumn(string[] columnNames, IType identityType)
    {
      this.whereStrings.Add(this.ToWhereString(columnNames));
      return this;
    }

    public SqlSimpleSelectBuilder SetVersionColumn(string[] columnNames, IVersionType versionType)
    {
      this.whereStrings.Add(this.ToWhereString(columnNames));
      return this;
    }

    public SqlSimpleSelectBuilder SetOrderBy(string orderBy)
    {
      this.orderBy = orderBy;
      return this;
    }

    public SqlSimpleSelectBuilder AddWhereFragment(string[] columnNames, IType type, string op)
    {
      this.whereStrings.Add(this.ToWhereString(columnNames, op));
      return this;
    }

    public virtual SqlSimpleSelectBuilder SetComment(string comment)
    {
      this.comment = comment;
      return this;
    }

    public SqlString ToSqlString()
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      if (this.comment != null)
        sqlStringBuilder.Add("/* " + this.comment + " */ ");
      bool flag = false;
      sqlStringBuilder.Add("SELECT ");
      for (int index = 0; index < this.columnNames.Count; ++index)
      {
        string columnName = this.columnNames[index];
        string alias = this.GetAlias(columnName);
        if (flag)
          sqlStringBuilder.Add(", ");
        sqlStringBuilder.Add(columnName);
        if (alias != null && !alias.Equals(columnName))
          sqlStringBuilder.Add(" AS ").Add(alias);
        flag = true;
      }
      sqlStringBuilder.Add(" FROM ").Add(this.Dialect.AppendLockHint(this.lockMode, this.tableName));
      sqlStringBuilder.Add(" WHERE ");
      if (this.whereStrings.Count > 1)
        sqlStringBuilder.Add(this.whereStrings.ToArray(), (string) null, "AND", (string) null, false);
      else
        sqlStringBuilder.Add(this.whereStrings[0]);
      if (this.orderBy != null)
        sqlStringBuilder.Add(this.orderBy);
      if (this.lockMode != null)
        sqlStringBuilder.Add(this.Dialect.GetForUpdateString(this.lockMode));
      return sqlStringBuilder.ToSqlString();
    }
  }
}
