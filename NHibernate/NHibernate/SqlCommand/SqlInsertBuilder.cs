// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.SqlInsertBuilder
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
  public class SqlInsertBuilder : ISqlStringBuilder
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (SqlInsertBuilder));
    private readonly ISessionFactoryImplementor factory;
    private string tableName;
    private string comment;
    private readonly LinkedHashMap<string, object> columns = new LinkedHashMap<string, object>();

    public SqlInsertBuilder(ISessionFactoryImplementor factory) => this.factory = factory;

    protected internal NHibernate.Dialect.Dialect Dialect => this.factory.Dialect;

    public virtual SqlInsertBuilder SetComment(string comment)
    {
      this.comment = comment;
      return this;
    }

    public SqlInsertBuilder SetTableName(string tableName)
    {
      this.tableName = tableName;
      return this;
    }

    public virtual SqlInsertBuilder AddColumn(string columnName, IType propertyType)
    {
      SqlType[] sqlTypeArray = propertyType.SqlTypes((IMapping) this.factory);
      this.columns[columnName] = sqlTypeArray.Length <= 1 ? (object) sqlTypeArray[0] : throw new AssertionFailure("Adding one column for a composed IType.");
      return this;
    }

    public SqlInsertBuilder AddColumn(string columnName, object val, ILiteralType literalType)
    {
      return this.AddColumn(columnName, literalType.ObjectToSQLString(val, this.Dialect));
    }

    public SqlInsertBuilder AddColumn(string columnName, string val)
    {
      this.columns[columnName] = (object) val;
      return this;
    }

    public SqlInsertBuilder AddColumns(string[] columnNames, bool[] insertable, IType propertyType)
    {
      SqlType[] sqlTypeArray = propertyType.SqlTypes((IMapping) this.factory);
      for (int index = 0; index < columnNames.Length; ++index)
      {
        if (insertable == null || insertable[index])
        {
          if (index >= sqlTypeArray.Length)
            throw new AssertionFailure("Different columns and it's IType.");
          this.columns[columnNames[index]] = (object) sqlTypeArray[index];
        }
      }
      return this;
    }

    public virtual SqlInsertBuilder AddIdentityColumn(string columnName)
    {
      string identityInsertString = this.Dialect.IdentityInsertString;
      if (identityInsertString != null)
        this.AddColumn(columnName, identityInsertString);
      return this;
    }

    public virtual SqlString ToSqlString()
    {
      int num = 5 + 2;
      if (this.columns.Count > 0)
        num += (this.columns.Count - 1) * 4;
      if (!string.IsNullOrEmpty(this.comment))
        ++num;
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder(num + 2);
      if (!string.IsNullOrEmpty(this.comment))
        sqlStringBuilder.Add("/* " + this.comment + " */ ");
      sqlStringBuilder.Add("INSERT INTO ").Add(this.tableName);
      if (this.columns.Count == 0)
      {
        sqlStringBuilder.Add(" ").Add(this.factory.Dialect.NoColumnsInsertString);
      }
      else
      {
        sqlStringBuilder.Add(" (");
        bool flag1 = false;
        foreach (string key in (IEnumerable<string>) this.columns.Keys)
        {
          if (flag1)
            sqlStringBuilder.Add(", ");
          flag1 = true;
          sqlStringBuilder.Add(key);
        }
        sqlStringBuilder.Add(") VALUES (");
        bool flag2 = false;
        foreach (object sql in (IEnumerable<object>) this.columns.Values)
        {
          if (flag2)
            sqlStringBuilder.Add(", ");
          flag2 = true;
          if (sql is SqlType)
            sqlStringBuilder.Add(Parameter.Placeholder);
          else
            sqlStringBuilder.Add((string) sql);
        }
        sqlStringBuilder.Add(")");
      }
      if (SqlInsertBuilder.log.IsDebugEnabled)
      {
        if (num < sqlStringBuilder.Count)
          SqlInsertBuilder.log.Debug((object) ("The initial capacity was set too low at: " + (object) num + " for the InsertSqlBuilder that needed a capacity of: " + (object) sqlStringBuilder.Count + " for the table " + this.tableName));
        else if (num > 16 && (double) num / (double) sqlStringBuilder.Count > 1.2)
          SqlInsertBuilder.log.Debug((object) ("The initial capacity was set too high at: " + (object) num + " for the InsertSqlBuilder that needed a capacity of: " + (object) sqlStringBuilder.Count + " for the table " + this.tableName));
      }
      return sqlStringBuilder.ToSqlString();
    }

    public SqlCommandInfo ToSqlCommandInfo()
    {
      return new SqlCommandInfo(this.ToSqlString(), this.GetParametersTypeArray());
    }

    public SqlType[] GetParametersTypeArray()
    {
      return new List<SqlType>((IEnumerable<SqlType>) new SafetyEnumerable<SqlType>((IEnumerable) this.columns.Values)).ToArray();
    }
  }
}
