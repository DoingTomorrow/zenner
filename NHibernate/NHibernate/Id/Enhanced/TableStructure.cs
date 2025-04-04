// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.Enhanced.TableStructure
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.AdoNet.Util;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using System;
using System.Data;

#nullable disable
namespace NHibernate.Id.Enhanced
{
  public class TableStructure : TransactionHelper, IDatabaseStructure
  {
    private static readonly IInternalLogger Log = LoggerProvider.LoggerFor(typeof (IDatabaseStructure));
    private readonly int _incrementSize;
    private readonly int _initialValue;
    private readonly string _tableName;
    private readonly string _valueColumnName;
    private readonly SqlString _selectQuery;
    private readonly SqlString _updateQuery;
    private readonly SqlType[] _updateParameterTypes;
    private int _accessCounter;
    private bool _applyIncrementSizeToSourceValues;

    public TableStructure(
      NHibernate.Dialect.Dialect dialect,
      string tableName,
      string valueColumnName,
      int initialValue,
      int incrementSize)
    {
      this._tableName = tableName;
      this._valueColumnName = valueColumnName;
      this._initialValue = initialValue;
      this._incrementSize = incrementSize;
      SqlStringBuilder sqlStringBuilder1 = new SqlStringBuilder();
      sqlStringBuilder1.Add("select ").Add(valueColumnName).Add(" as id_val").Add(" from ").Add(dialect.AppendLockHint(LockMode.Upgrade, tableName)).Add(dialect.ForUpdateString);
      this._selectQuery = sqlStringBuilder1.ToSqlString();
      SqlStringBuilder sqlStringBuilder2 = new SqlStringBuilder();
      sqlStringBuilder2.Add("update ").Add(tableName).Add(" set ").Add(valueColumnName).Add(" = ").Add(Parameter.Placeholder).Add(" where ").Add(valueColumnName).Add(" = ").Add(Parameter.Placeholder);
      this._updateQuery = sqlStringBuilder2.ToSqlString();
      this._updateParameterTypes = new SqlType[2]
      {
        SqlTypeFactory.Int64,
        SqlTypeFactory.Int64
      };
    }

    public string Name => this._tableName;

    public int TimesAccessed => this._accessCounter;

    public int IncrementSize => this._incrementSize;

    public virtual IAccessCallback BuildCallback(ISessionImplementor session)
    {
      return (IAccessCallback) new TableStructure.TableAccessCallback(session, this);
    }

    public virtual void Prepare(IOptimizer optimizer)
    {
      this._applyIncrementSizeToSourceValues = optimizer.ApplyIncrementSizeToSourceValues;
    }

    public virtual string[] SqlCreateStrings(NHibernate.Dialect.Dialect dialect)
    {
      return new string[2]
      {
        "create table " + this._tableName + " ( " + this._valueColumnName + " " + dialect.GetTypeName(SqlTypeFactory.Int64) + " )",
        "insert into " + this._tableName + " values ( " + (object) this._initialValue + " )"
      };
    }

    public virtual string[] SqlDropStrings(NHibernate.Dialect.Dialect dialect)
    {
      return new string[1]
      {
        dialect.GetDropTableString(this._tableName)
      };
    }

    public override object DoWorkInCurrentTransaction(
      ISessionImplementor session,
      IDbConnection conn,
      IDbTransaction transaction)
    {
      long int64;
      int num1;
      do
      {
        try
        {
          IDbCommand command = session.Factory.ConnectionProvider.Driver.GenerateCommand(CommandType.Text, this._selectQuery, SqlTypeFactory.NoTypes);
          object obj;
          using (command)
          {
            command.Connection = conn;
            command.Transaction = transaction;
            PersistentIdGeneratorParmsNames.SqlStatementLogger.LogCommand(command, FormatStyle.Basic);
            obj = command.ExecuteScalar();
          }
          if (obj == null)
          {
            string message = "could not read a hi value - you need to populate the table: " + this._tableName;
            TableStructure.Log.Error((object) message);
            throw new IdentifierGenerationException(message);
          }
          int64 = Convert.ToInt64(obj);
        }
        catch (Exception ex)
        {
          TableStructure.Log.Error((object) "could not read a hi value", ex);
          throw;
        }
        try
        {
          IDbCommand command = session.Factory.ConnectionProvider.Driver.GenerateCommand(CommandType.Text, this._updateQuery, this._updateParameterTypes);
          using (command)
          {
            command.Connection = conn;
            command.Transaction = transaction;
            PersistentIdGeneratorParmsNames.SqlStatementLogger.LogCommand(command, FormatStyle.Basic);
            int num2 = this._applyIncrementSizeToSourceValues ? this._incrementSize : 1;
            ((IDataParameter) command.Parameters[0]).Value = (object) (int64 + (long) num2);
            ((IDataParameter) command.Parameters[1]).Value = (object) int64;
            num1 = command.ExecuteNonQuery();
          }
        }
        catch (Exception ex)
        {
          TableStructure.Log.Error((object) ("could not update hi value in: " + this._tableName), ex);
          throw;
        }
      }
      while (num1 == 0);
      ++this._accessCounter;
      return (object) int64;
    }

    private class TableAccessCallback : IAccessCallback
    {
      private readonly TableStructure _owner;
      private readonly ISessionImplementor _session;

      public TableAccessCallback(ISessionImplementor session, TableStructure owner)
      {
        this._session = session;
        this._owner = owner;
      }

      public virtual long GetNextValue()
      {
        return Convert.ToInt64(this._owner.DoWorkInNewTransaction(this._session));
      }
    }
  }
}
