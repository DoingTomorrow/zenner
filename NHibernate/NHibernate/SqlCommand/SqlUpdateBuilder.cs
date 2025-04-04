// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.SqlUpdateBuilder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.Util;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.SqlCommand
{
  public class SqlUpdateBuilder(NHibernate.Dialect.Dialect dialect, IMapping mapping) : 
    SqlBaseBuilder(dialect, mapping),
    ISqlStringBuilder
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (SqlUpdateBuilder));
    private string tableName;
    private string comment;
    private readonly LinkedHashMap<string, object> columns = new LinkedHashMap<string, object>();
    private List<SqlString> whereStrings = new List<SqlString>();
    private readonly List<SqlType> whereParameterTypes = new List<SqlType>();
    private SqlString assignments;

    public SqlUpdateBuilder SetTableName(string tableName)
    {
      this.tableName = tableName;
      return this;
    }

    public SqlUpdateBuilder SetComment(string comment)
    {
      this.comment = comment;
      return this;
    }

    public SqlUpdateBuilder AddColumn(string columnName, object val, ILiteralType literalType)
    {
      return this.AddColumn(columnName, literalType.ObjectToSQLString(val, this.Dialect));
    }

    public SqlUpdateBuilder AddColumn(string columnName, string val)
    {
      this.columns[columnName] = (object) val;
      return this;
    }

    public SqlUpdateBuilder AddColumns(string[] columnsName, string val)
    {
      foreach (string key in columnsName)
        this.columns[key] = (object) val;
      return this;
    }

    public virtual SqlUpdateBuilder AddColumn(string columnName, IType propertyType)
    {
      SqlType[] sqlTypeArray = propertyType.SqlTypes(this.Mapping);
      this.columns[columnName] = sqlTypeArray.Length <= 1 ? (object) sqlTypeArray[0] : throw new AssertionFailure("Adding one column for a composed IType.");
      return this;
    }

    public SqlUpdateBuilder AddColumns(string[] columnNames, IType propertyType)
    {
      return this.AddColumns(columnNames, (bool[]) null, propertyType);
    }

    public SqlUpdateBuilder AddColumns(string[] columnNames, bool[] updateable, IType propertyType)
    {
      SqlType[] sqlTypeArray = propertyType.SqlTypes(this.Mapping);
      for (int index = 0; index < columnNames.Length; ++index)
      {
        if (updateable == null || updateable[index])
        {
          if (index >= sqlTypeArray.Length)
            throw new AssertionFailure("Different columns and it's IType.");
          this.columns[columnNames[index]] = (object) sqlTypeArray[index];
        }
      }
      return this;
    }

    public SqlUpdateBuilder AppendAssignmentFragment(SqlString fragment)
    {
      this.assignments = this.assignments == null ? fragment : this.assignments.Append(", ").Append(fragment);
      return this;
    }

    public SqlUpdateBuilder SetWhere(string whereSql)
    {
      if (StringHelper.IsNotEmpty(whereSql))
        this.whereStrings = new List<SqlString>((IEnumerable<SqlString>) new SqlString[1]
        {
          new SqlString(whereSql)
        });
      return this;
    }

    public SqlUpdateBuilder SetIdentityColumn(string[] columnNames, IType identityType)
    {
      this.whereStrings.Add(this.ToWhereString(columnNames));
      this.whereParameterTypes.AddRange((IEnumerable<SqlType>) identityType.SqlTypes(this.Mapping));
      return this;
    }

    public SqlUpdateBuilder SetVersionColumn(string[] columnNames, IVersionType versionType)
    {
      this.whereStrings.Add(this.ToWhereString(columnNames));
      this.whereParameterTypes.AddRange((IEnumerable<SqlType>) versionType.SqlTypes(this.Mapping));
      return this;
    }

    public SqlUpdateBuilder AddWhereFragment(string[] columnNames, IType type, string op)
    {
      if (columnNames.Length > 0)
      {
        this.whereStrings.Add(this.ToWhereString(columnNames, op));
        this.whereParameterTypes.AddRange((IEnumerable<SqlType>) type.SqlTypes(this.Mapping));
      }
      return this;
    }

    public SqlUpdateBuilder AddWhereFragment(string[] columnNames, SqlType[] types, string op)
    {
      if (columnNames.Length > 0)
      {
        this.whereStrings.Add(this.ToWhereString(columnNames, op));
        this.whereParameterTypes.AddRange((IEnumerable<SqlType>) types);
      }
      return this;
    }

    public SqlUpdateBuilder AddWhereFragment(string columnName, SqlType type, string op)
    {
      if (!string.IsNullOrEmpty(columnName))
      {
        this.whereStrings.Add(this.ToWhereString(columnName, op));
        this.whereParameterTypes.Add(type);
      }
      return this;
    }

    public SqlUpdateBuilder AddWhereFragment(string whereSql)
    {
      if (!string.IsNullOrEmpty(whereSql))
        this.whereStrings.Add(new SqlString(whereSql));
      return this;
    }

    public SqlString ToSqlString()
    {
      int num1 = 3;
      if (this.columns.Count > 0)
        num1 += this.columns.Count - 1 + this.columns.Count * 3;
      int num2 = num1 + 1;
      if (this.whereStrings.Count > 0)
      {
        num2 += this.whereStrings.Count - 1;
        foreach (SqlString whereString in this.whereStrings)
          num2 += whereString.Count;
      }
      if (!string.IsNullOrEmpty(this.comment))
        ++num2;
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder(num2 + 2);
      if (!string.IsNullOrEmpty(this.comment))
        sqlStringBuilder.Add("/* " + this.comment + " */ ");
      sqlStringBuilder.Add("UPDATE ").Add(this.tableName).Add(" SET ");
      bool flag1 = false;
      bool flag2 = false;
      foreach (KeyValuePair<string, object> column in this.columns)
      {
        if (flag2)
          sqlStringBuilder.Add(", ");
        flag2 = true;
        sqlStringBuilder.Add(column.Key).Add(" = ");
        if (column.Value is SqlType)
          sqlStringBuilder.Add(Parameter.Placeholder);
        else
          sqlStringBuilder.Add((string) column.Value);
        flag1 = true;
      }
      if (this.assignments != null)
      {
        if (flag1)
          sqlStringBuilder.Add(", ");
        sqlStringBuilder.Add(this.assignments);
      }
      sqlStringBuilder.Add(" WHERE ");
      bool flag3 = false;
      foreach (SqlString whereString in this.whereStrings)
      {
        if (flag3)
          sqlStringBuilder.Add(" AND ");
        flag3 = true;
        sqlStringBuilder.Add(whereString);
      }
      if (SqlUpdateBuilder.log.IsDebugEnabled)
      {
        if (num2 < sqlStringBuilder.Count)
          SqlUpdateBuilder.log.Debug((object) ("The initial capacity was set too low at: " + (object) num2 + " for the UpdateSqlBuilder that needed a capacity of: " + (object) sqlStringBuilder.Count + " for the table " + this.tableName));
        else if (num2 > 16 && (double) num2 / (double) sqlStringBuilder.Count > 1.2)
          SqlUpdateBuilder.log.Debug((object) ("The initial capacity was set too high at: " + (object) num2 + " for the UpdateSqlBuilder that needed a capacity of: " + (object) sqlStringBuilder.Count + " for the table " + this.tableName));
      }
      return sqlStringBuilder.ToSqlString();
    }

    public SqlCommandInfo ToSqlCommandInfo()
    {
      SqlString sqlString = this.ToSqlString();
      List<SqlType> sqlTypeList = new List<SqlType>((IEnumerable<SqlType>) new SafetyEnumerable<SqlType>((IEnumerable) this.columns.Values));
      sqlTypeList.AddRange((IEnumerable<SqlType>) this.whereParameterTypes);
      return new SqlCommandInfo(sqlString, sqlTypeList.ToArray());
    }
  }
}
