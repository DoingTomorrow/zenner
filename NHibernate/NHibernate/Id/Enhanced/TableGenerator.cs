// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.Enhanced.TableGenerator
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
namespace NHibernate.Id.Enhanced
{
  public class TableGenerator : 
    TransactionHelper,
    IPersistentIdentifierGenerator,
    IIdentifierGenerator,
    IConfigurable
  {
    public const string ConfigPreferSegmentPerEntity = "prefer_entity_table_as_segment_value";
    public const string TableParam = "table_name";
    public const string DefaultTable = "hibernate_sequences";
    public const string ValueColumnParam = "value_column_name";
    public const string DefaultValueColumn = "next_val";
    public const string SegmentColumnParam = "segment_column_name";
    public const string DefaultSegmentColumn = "sequence_name";
    public const string SegmentValueParam = "segment_value";
    public const string DefaultSegmentValue = "default";
    public const string SegmentLengthParam = "segment_value_length";
    public const int DefaultSegmentLength = 255;
    public const string InitialParam = "initial_value";
    public const int DefaltInitialValue = 1;
    public const string IncrementParam = "increment_size";
    public const int DefaultIncrementSize = 1;
    public const string OptimizerParam = "optimizer";
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (SequenceStyleGenerator));
    private SqlString selectQuery;
    private SqlType[] selectParameterTypes;
    private SqlString insertQuery;
    private SqlType[] insertParameterTypes;
    private SqlString updateQuery;
    private SqlType[] updateParameterTypes;

    public IType IdentifierType { get; private set; }

    public string TableName { get; private set; }

    public string SegmentColumnName { get; private set; }

    public string SegmentValue { get; private set; }

    public int SegmentValueLength { get; private set; }

    public string ValueColumnName { get; private set; }

    public int InitialValue { get; private set; }

    public int IncrementSize { get; private set; }

    public IOptimizer Optimizer { get; private set; }

    public long TableAccessCount { get; private set; }

    public virtual string GeneratorKey() => this.TableName;

    public virtual void Configure(IType type, IDictionary<string, string> parms, NHibernate.Dialect.Dialect dialect)
    {
      this.IdentifierType = type;
      this.TableName = this.DetermineGeneratorTableName(parms, dialect);
      this.SegmentColumnName = this.DetermineSegmentColumnName(parms, dialect);
      this.ValueColumnName = this.DetermineValueColumnName(parms, dialect);
      this.SegmentValue = this.DetermineSegmentValue(parms);
      this.SegmentValueLength = this.DetermineSegmentColumnSize(parms);
      this.InitialValue = this.DetermineInitialValue(parms);
      this.IncrementSize = this.DetermineIncrementSize(parms);
      this.BuildSelectQuery(dialect);
      this.BuildUpdateQuery();
      this.BuildInsertQuery();
      string str = PropertiesHelper.GetBoolean("id.optimizer.pooled.prefer_lo", parms, false) ? "pooled-lo" : "pooled";
      string defaultValue = this.IncrementSize <= 1 ? "none" : str;
      this.Optimizer = OptimizerFactory.BuildOptimizer(PropertiesHelper.GetString("optimizer", parms, defaultValue), this.IdentifierType.ReturnedClass, this.IncrementSize, (long) PropertiesHelper.GetInt32("initial_value", parms, -1));
    }

    protected string DetermineGeneratorTableName(IDictionary<string, string> parms, NHibernate.Dialect.Dialect dialect)
    {
      string table = PropertiesHelper.GetString("table_name", parms, "hibernate_sequences");
      if (table.IndexOf('.') < 0)
      {
        string schema;
        parms.TryGetValue(PersistentIdGeneratorParmsNames.Schema, out schema);
        string catalog;
        parms.TryGetValue(PersistentIdGeneratorParmsNames.Catalog, out catalog);
        table = dialect.Qualify(catalog, schema, table);
      }
      return table;
    }

    protected string DetermineSegmentColumnName(IDictionary<string, string> parms, NHibernate.Dialect.Dialect dialect)
    {
      string columnName = PropertiesHelper.GetString("segment_column_name", parms, "sequence_name");
      return dialect.QuoteForColumnName(columnName);
    }

    protected string DetermineValueColumnName(IDictionary<string, string> parms, NHibernate.Dialect.Dialect dialect)
    {
      string columnName = PropertiesHelper.GetString("value_column_name", parms, "next_val");
      return dialect.QuoteForColumnName(columnName);
    }

    protected string DetermineSegmentValue(IDictionary<string, string> parms)
    {
      string defaultSegmentValue = PropertiesHelper.GetString("segment_value", parms, "");
      if (string.IsNullOrEmpty(defaultSegmentValue))
        defaultSegmentValue = this.DetermineDefaultSegmentValue(parms);
      return defaultSegmentValue;
    }

    protected string DetermineDefaultSegmentValue(IDictionary<string, string> parms)
    {
      string defaultSegmentValue = PropertiesHelper.GetBoolean("prefer_entity_table_as_segment_value", parms, false) ? parms[PersistentIdGeneratorParmsNames.Table] : "default";
      TableGenerator.log.DebugFormat("Explicit segment value for id generator [{0}.{1}] suggested; using default [{2}].", (object) this.TableName, (object) this.SegmentColumnName, (object) defaultSegmentValue);
      return defaultSegmentValue;
    }

    protected int DetermineSegmentColumnSize(IDictionary<string, string> parms)
    {
      return PropertiesHelper.GetInt32("segment_value_length", parms, (int) byte.MaxValue);
    }

    protected int DetermineInitialValue(IDictionary<string, string> parms)
    {
      return PropertiesHelper.GetInt32("initial_value", parms, 1);
    }

    protected int DetermineIncrementSize(IDictionary<string, string> parms)
    {
      return PropertiesHelper.GetInt32("increment_size", parms, 1);
    }

    protected void BuildSelectQuery(NHibernate.Dialect.Dialect dialect)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder(100);
      sqlStringBuilder.Add("select ").Add(StringHelper.Qualify("tbl", this.ValueColumnName)).Add(" from " + this.TableName + " tbl where ").Add(StringHelper.Qualify("tbl", this.SegmentColumnName) + " = ").AddParameter().Add("  ");
      this.selectQuery = dialect.ApplyLocksToSql(sqlStringBuilder.ToSqlString(), (IDictionary<string, LockMode>) new Dictionary<string, LockMode>()
      {
        ["tbl"] = LockMode.Upgrade
      }, (IDictionary<string, string[]>) new Dictionary<string, string[]>()
      {
        ["tbl"] = new string[1]{ this.ValueColumnName }
      });
      this.selectParameterTypes = (SqlType[]) new AnsiStringSqlType[1]
      {
        SqlTypeFactory.GetAnsiString(this.SegmentValueLength)
      };
    }

    protected void BuildUpdateQuery()
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder(100);
      sqlStringBuilder.Add("update " + this.TableName).Add(" set ").Add(this.ValueColumnName).Add(" = ").AddParameter().Add(" where ").Add(this.ValueColumnName).Add(" = ").AddParameter().Add(" and ").Add(this.SegmentColumnName).Add(" = ").AddParameter();
      this.updateQuery = sqlStringBuilder.ToSqlString();
      this.updateParameterTypes = new SqlType[3]
      {
        SqlTypeFactory.Int64,
        SqlTypeFactory.Int64,
        (SqlType) SqlTypeFactory.GetAnsiString(this.SegmentValueLength)
      };
    }

    protected void BuildInsertQuery()
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder(100);
      sqlStringBuilder.Add("insert into " + this.TableName).Add(" (" + this.SegmentColumnName + ", " + this.ValueColumnName + ") ").Add(" values (").AddParameter().Add(", ").AddParameter().Add(")");
      this.insertQuery = sqlStringBuilder.ToSqlString();
      this.insertParameterTypes = new SqlType[2]
      {
        (SqlType) SqlTypeFactory.GetAnsiString(this.SegmentValueLength),
        SqlTypeFactory.Int64
      };
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public virtual object Generate(ISessionImplementor session, object obj)
    {
      return this.Optimizer.Generate((IAccessCallback) new TableGenerator.TableAccessCallback(session, this));
    }

    public override object DoWorkInCurrentTransaction(
      ISessionImplementor session,
      IDbConnection conn,
      IDbTransaction transaction)
    {
      long num1;
      int num2;
      do
      {
        try
        {
          IDbCommand command1 = session.Factory.ConnectionProvider.Driver.GenerateCommand(CommandType.Text, this.selectQuery, this.selectParameterTypes);
          object obj;
          using (command1)
          {
            command1.Connection = conn;
            command1.Transaction = transaction;
            string commandText = command1.CommandText;
            ((IDataParameter) command1.Parameters[0]).Value = (object) this.SegmentValue;
            PersistentIdGeneratorParmsNames.SqlStatementLogger.LogCommand(command1, FormatStyle.Basic);
            obj = command1.ExecuteScalar();
          }
          if (obj == null)
          {
            num1 = (long) this.InitialValue;
            IDbCommand command2 = session.Factory.ConnectionProvider.Driver.GenerateCommand(CommandType.Text, this.insertQuery, this.insertParameterTypes);
            using (command2)
            {
              command2.Connection = conn;
              command2.Transaction = transaction;
              ((IDataParameter) command2.Parameters[0]).Value = (object) this.SegmentValue;
              ((IDataParameter) command2.Parameters[1]).Value = (object) num1;
              PersistentIdGeneratorParmsNames.SqlStatementLogger.LogCommand(command2, FormatStyle.Basic);
              command2.ExecuteNonQuery();
            }
          }
          else
            num1 = Convert.ToInt64(obj);
        }
        catch (Exception ex)
        {
          TableGenerator.log.Error((object) ("Unable to read or initialize hi value in " + this.TableName), ex);
          throw;
        }
        try
        {
          IDbCommand command = session.Factory.ConnectionProvider.Driver.GenerateCommand(CommandType.Text, this.updateQuery, this.updateParameterTypes);
          using (command)
          {
            command.Connection = conn;
            command.Transaction = transaction;
            int num3 = this.Optimizer.ApplyIncrementSizeToSourceValues ? this.IncrementSize : 1;
            ((IDataParameter) command.Parameters[0]).Value = (object) (num1 + (long) num3);
            ((IDataParameter) command.Parameters[1]).Value = (object) num1;
            ((IDataParameter) command.Parameters[2]).Value = (object) this.SegmentValue;
            PersistentIdGeneratorParmsNames.SqlStatementLogger.LogCommand(command, FormatStyle.Basic);
            num2 = command.ExecuteNonQuery();
          }
        }
        catch (Exception ex)
        {
          TableGenerator.log.Error((object) ("Unable to update hi value in " + this.TableName), ex);
          throw;
        }
      }
      while (num2 == 0);
      ++this.TableAccessCount;
      return (object) num1;
    }

    public virtual string[] SqlCreateStrings(NHibernate.Dialect.Dialect dialect)
    {
      return new string[1]
      {
        dialect.CreateTableString + " " + this.TableName + " ( " + this.SegmentColumnName + " " + dialect.GetTypeName((SqlType) SqlTypeFactory.GetAnsiString(this.SegmentValueLength)) + " not null, " + this.ValueColumnName + " " + dialect.GetTypeName(SqlTypeFactory.Int64) + ", " + dialect.PrimaryKeyString + " ( " + this.SegmentColumnName + ") )"
      };
    }

    public virtual string[] SqlDropString(NHibernate.Dialect.Dialect dialect)
    {
      return new string[1]
      {
        dialect.GetDropTableString(this.TableName)
      };
    }

    private class TableAccessCallback : IAccessCallback
    {
      private TableGenerator owner;
      private readonly ISessionImplementor session;

      public TableAccessCallback(ISessionImplementor session, TableGenerator owner)
      {
        this.session = session;
        this.owner = owner;
      }

      public long GetNextValue()
      {
        return Convert.ToInt64(this.owner.DoWorkInNewTransaction(this.session));
      }
    }
  }
}
