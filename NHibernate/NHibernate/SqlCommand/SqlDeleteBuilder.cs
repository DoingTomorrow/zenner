// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.SqlDeleteBuilder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.Util;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.SqlCommand
{
  public class SqlDeleteBuilder(NHibernate.Dialect.Dialect dialect, IMapping mapping) : 
    SqlBaseBuilder(dialect, mapping),
    ISqlStringBuilder
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (SqlDeleteBuilder));
    private string tableName;
    private List<SqlString> whereStrings = new List<SqlString>();
    private readonly List<SqlType> parameterTypes = new List<SqlType>();
    private string comment;

    public SqlDeleteBuilder SetTableName(string tableName)
    {
      this.tableName = tableName;
      return this;
    }

    public SqlDeleteBuilder SetComment(string comment)
    {
      this.comment = comment;
      return this;
    }

    public SqlDeleteBuilder SetIdentityColumn(string[] columnNames, IType identityType)
    {
      this.whereStrings.Add(this.ToWhereString(columnNames));
      this.parameterTypes.AddRange((IEnumerable<SqlType>) identityType.SqlTypes(this.Mapping));
      return this;
    }

    public SqlDeleteBuilder SetVersionColumn(string[] columnNames, IVersionType versionType)
    {
      this.whereStrings.Add(this.ToWhereString(columnNames));
      this.parameterTypes.AddRange((IEnumerable<SqlType>) versionType.SqlTypes(this.Mapping));
      return this;
    }

    public SqlDeleteBuilder AddWhereFragment(string[] columnNames, IType type, string op)
    {
      this.whereStrings.Add(this.ToWhereString(columnNames, op));
      this.parameterTypes.AddRange((IEnumerable<SqlType>) type.SqlTypes(this.Mapping));
      return this;
    }

    public SqlDeleteBuilder AddWhereFragment(string[] columnNames, SqlType[] types, string op)
    {
      this.whereStrings.Add(this.ToWhereString(columnNames, op));
      this.parameterTypes.AddRange((IEnumerable<SqlType>) types);
      return this;
    }

    public SqlDeleteBuilder AddWhereFragment(string columnName, SqlType type, string op)
    {
      if (!string.IsNullOrEmpty(columnName))
      {
        this.whereStrings.Add(this.ToWhereString(columnName, op));
        this.parameterTypes.Add(type);
      }
      return this;
    }

    public SqlDeleteBuilder AddWhereFragment(string whereSql)
    {
      if (StringHelper.IsNotEmpty(whereSql))
        this.whereStrings.Add(new SqlString(whereSql));
      return this;
    }

    public virtual SqlDeleteBuilder SetWhere(string whereSql)
    {
      if (StringHelper.IsNotEmpty(whereSql))
        this.whereStrings = new List<SqlString>((IEnumerable<SqlString>) new SqlString[1]
        {
          new SqlString(whereSql)
        });
      return this;
    }

    public SqlString ToSqlString()
    {
      int num = 3 + (this.whereStrings.Count - 1);
      for (int index = 0; index < this.whereStrings.Count; ++index)
        num += this.whereStrings[index].Count;
      if (!string.IsNullOrEmpty(this.comment))
        ++num;
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder(num + 2);
      if (!string.IsNullOrEmpty(this.comment))
        sqlStringBuilder.Add("/* " + this.comment + " */ ");
      sqlStringBuilder.Add("DELETE FROM ").Add(this.tableName).Add(" WHERE ");
      if (this.whereStrings.Count > 1)
        sqlStringBuilder.Add(this.whereStrings.ToArray(), (string) null, "AND", (string) null, false);
      else
        sqlStringBuilder.Add(this.whereStrings[0]);
      if (SqlDeleteBuilder.log.IsDebugEnabled)
      {
        if (num < sqlStringBuilder.Count)
          SqlDeleteBuilder.log.Debug((object) ("The initial capacity was set too low at: " + (object) num + " for the DeleteSqlBuilder that needed a capacity of: " + (object) sqlStringBuilder.Count + " for the table " + this.tableName));
        else if (num > 16 && (double) num / (double) sqlStringBuilder.Count > 1.2)
          SqlDeleteBuilder.log.Debug((object) ("The initial capacity was set too high at: " + (object) num + " for the DeleteSqlBuilder that needed a capacity of: " + (object) sqlStringBuilder.Count + " for the table " + this.tableName));
      }
      return sqlStringBuilder.ToSqlString();
    }

    public SqlCommandInfo ToSqlCommandInfo()
    {
      return new SqlCommandInfo(this.ToSqlString(), this.parameterTypes.ToArray());
    }
  }
}
