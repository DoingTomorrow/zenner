// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.TableGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.AdoNet.Util;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;

#nullable disable
namespace NHibernate.Id
{
  public class TableGenerator : 
    TransactionHelper,
    IPersistentIdentifierGenerator,
    IIdentifierGenerator,
    IConfigurable
  {
    public const string Where = "where";
    public const string ColumnParamName = "column";
    public const string TableParamName = "table";
    public const string DefaultColumnName = "next_hi";
    public const string DefaultTableName = "hibernate_unique_key";
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (TableGenerator));
    private string tableName;
    private string columnName;
    private string whereClause;
    private string query;
    protected SqlType columnSqlType;
    protected PrimitiveType columnType;
    private SqlString updateSql;
    private SqlType[] parameterTypes;

    public virtual void Configure(IType type, IDictionary<string, string> parms, NHibernate.Dialect.Dialect dialect)
    {
      this.tableName = PropertiesHelper.GetString("table", parms, "hibernate_unique_key");
      this.columnName = PropertiesHelper.GetString("column", parms, "next_hi");
      this.whereClause = PropertiesHelper.GetString("where", parms, "");
      string schema = PropertiesHelper.GetString(PersistentIdGeneratorParmsNames.Schema, parms, (string) null);
      string catalog = PropertiesHelper.GetString(PersistentIdGeneratorParmsNames.Catalog, parms, (string) null);
      if (this.tableName.IndexOf('.') < 0)
        this.tableName = dialect.Qualify(catalog, schema, this.tableName);
      SqlStringBuilder sqlStringBuilder1 = new SqlStringBuilder(100);
      sqlStringBuilder1.Add("select " + this.columnName).Add(" from " + dialect.AppendLockHint(LockMode.Upgrade, this.tableName));
      if (!string.IsNullOrEmpty(this.whereClause))
        sqlStringBuilder1.Add(" where ").Add(this.whereClause);
      sqlStringBuilder1.Add(dialect.ForUpdateString);
      this.query = sqlStringBuilder1.ToString();
      this.columnType = type as PrimitiveType;
      if (this.columnType == null)
      {
        TableGenerator.log.Error((object) "Column type for TableGenerator is not a value type");
        throw new ArgumentException("type is not a ValueTypeType", nameof (type));
      }
      switch (type)
      {
        case Int16Type _:
          this.columnSqlType = SqlTypeFactory.Int16;
          break;
        case Int64Type _:
          this.columnSqlType = SqlTypeFactory.Int64;
          break;
        default:
          this.columnSqlType = SqlTypeFactory.Int32;
          break;
      }
      this.parameterTypes = new SqlType[2]
      {
        this.columnSqlType,
        this.columnSqlType
      };
      SqlStringBuilder sqlStringBuilder2 = new SqlStringBuilder(100);
      sqlStringBuilder2.Add("update " + this.tableName + " set ").Add(this.columnName).Add(" = ").Add(Parameter.Placeholder).Add(" where ").Add(this.columnName).Add(" = ").Add(Parameter.Placeholder);
      if (!string.IsNullOrEmpty(this.whereClause))
        sqlStringBuilder2.Add(" and ").Add(this.whereClause);
      this.updateSql = sqlStringBuilder2.ToSqlString();
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public virtual object Generate(ISessionImplementor session, object obj)
    {
      return this.DoWorkInNewTransaction(session);
    }

    public virtual string[] SqlCreateStrings(NHibernate.Dialect.Dialect dialect)
    {
      return new string[2]
      {
        "create table " + this.tableName + " ( " + this.columnName + " " + dialect.GetTypeName(this.columnSqlType) + " )",
        "insert into " + this.tableName + " values ( 1 )"
      };
    }

    public virtual string[] SqlDropString(NHibernate.Dialect.Dialect dialect)
    {
      return new string[1]
      {
        dialect.GetDropTableString(this.tableName)
      };
    }

    public string GeneratorKey() => this.tableName;

    public override object DoWorkInCurrentTransaction(
      ISessionImplementor session,
      IDbConnection conn,
      IDbTransaction transaction)
    {
      long int64;
      int num;
      do
      {
        IDbCommand command1 = conn.CreateCommand();
        IDataReader rs = (IDataReader) null;
        command1.CommandText = this.query;
        command1.CommandType = CommandType.Text;
        command1.Transaction = transaction;
        PersistentIdGeneratorParmsNames.SqlStatementLogger.LogCommand("Reading high value:", command1, FormatStyle.Basic);
        try
        {
          rs = command1.ExecuteReader();
          if (!rs.Read())
          {
            string message = !string.IsNullOrEmpty(this.whereClause) ? string.Format("could not read a hi value from table '{0}' using the where clause ({1})- you need to populate the table.", (object) this.tableName, (object) this.whereClause) : "could not read a hi value - you need to populate the table: " + this.tableName;
            TableGenerator.log.Error((object) message);
            throw new IdentifierGenerationException(message);
          }
          int64 = Convert.ToInt64(this.columnType.Get(rs, 0));
        }
        catch (Exception ex)
        {
          TableGenerator.log.Error((object) "could not read a hi value", ex);
          throw;
        }
        finally
        {
          rs?.Close();
          command1.Dispose();
        }
        IDbCommand command2 = session.Factory.ConnectionProvider.Driver.GenerateCommand(CommandType.Text, this.updateSql, this.parameterTypes);
        command2.Connection = conn;
        command2.Transaction = transaction;
        try
        {
          this.columnType.Set(command2, (object) (int64 + 1L), 0);
          this.columnType.Set(command2, (object) int64, 1);
          PersistentIdGeneratorParmsNames.SqlStatementLogger.LogCommand("Updating high value:", command2, FormatStyle.Basic);
          num = command2.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          TableGenerator.log.Error((object) ("could not update hi value in: " + this.tableName), ex);
          throw;
        }
        finally
        {
          command2.Dispose();
        }
      }
      while (num == 0);
      return (object) int64;
    }
  }
}
