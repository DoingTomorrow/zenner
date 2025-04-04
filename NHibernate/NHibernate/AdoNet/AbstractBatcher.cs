// Decompiled with JetBrains decompiler
// Type: NHibernate.AdoNet.AbstractBatcher
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.AdoNet.Util;
using NHibernate.Driver;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace NHibernate.AdoNet
{
  public abstract class AbstractBatcher : IBatcher, IDisposable
  {
    protected static readonly IInternalLogger Log = LoggerProvider.LoggerFor(typeof (AbstractBatcher));
    private static int _openCommandCount;
    private static int _openReaderCount;
    private readonly ConnectionManager _connectionManager;
    private readonly ISessionFactoryImplementor _factory;
    private readonly IInterceptor _interceptor;
    private IDbCommand _batchCommand;
    private SqlString _batchCommandSql;
    private SqlType[] _batchCommandParameterTypes;
    private readonly ISet<IDbCommand> _commandsToClose = (ISet<IDbCommand>) new HashedSet<IDbCommand>();
    private readonly ISet<IDataReader> _readersToClose = (ISet<IDataReader>) new HashedSet<IDataReader>();
    private readonly IDictionary<IDataReader, Stopwatch> _readersDuration = (IDictionary<IDataReader, Stopwatch>) new Dictionary<IDataReader, Stopwatch>();
    private IDbCommand _lastQuery;
    private bool _releasing;
    private bool _isAlreadyDisposed;

    protected AbstractBatcher(ConnectionManager connectionManager, IInterceptor interceptor)
    {
      this._connectionManager = connectionManager;
      this._interceptor = interceptor;
      this._factory = connectionManager.Factory;
    }

    protected IDriver Driver => this._factory.ConnectionProvider.Driver;

    protected IDbCommand CurrentCommand => this._batchCommand;

    public IDbCommand Generate(CommandType type, SqlString sqlString, SqlType[] parameterTypes)
    {
      SqlString sql = this.GetSQL(sqlString);
      IDbCommand command = this._factory.ConnectionProvider.Driver.GenerateCommand(type, sql, parameterTypes);
      this.LogOpenPreparedCommand();
      if (AbstractBatcher.Log.IsDebugEnabled)
        AbstractBatcher.Log.Debug((object) ("Building an IDbCommand object for the SqlString: " + (object) sql));
      this._commandsToClose.Add(command);
      return command;
    }

    protected void Prepare(IDbCommand cmd)
    {
      try
      {
        IDbConnection connection = this._connectionManager.GetConnection();
        if (cmd.Connection != null)
        {
          if (cmd.Connection != connection)
            cmd.Connection = connection;
        }
        else
          cmd.Connection = connection;
        this._connectionManager.Transaction.Enlist(cmd);
        this.Driver.PrepareCommand(cmd);
      }
      catch (InvalidOperationException ex)
      {
        throw new ADOException("While preparing " + cmd.CommandText + " an error occurred", (Exception) ex);
      }
    }

    public virtual IDbCommand PrepareBatchCommand(
      CommandType type,
      SqlString sql,
      SqlType[] parameterTypes)
    {
      if (sql.Equals((object) this._batchCommandSql) && ArrayHelper.ArrayEquals(parameterTypes, this._batchCommandParameterTypes))
      {
        if (AbstractBatcher.Log.IsDebugEnabled)
          AbstractBatcher.Log.Debug((object) ("reusing command " + this._batchCommand.CommandText));
      }
      else
      {
        this._batchCommand = this.PrepareCommand(type, sql, parameterTypes);
        this._batchCommandSql = sql;
        this._batchCommandParameterTypes = parameterTypes;
      }
      return this._batchCommand;
    }

    public IDbCommand PrepareCommand(CommandType type, SqlString sql, SqlType[] parameterTypes)
    {
      this.OnPreparedCommand();
      return this.Generate(type, sql, parameterTypes);
    }

    protected virtual void OnPreparedCommand() => this.ExecuteBatch();

    public IDbCommand PrepareQueryCommand(
      CommandType type,
      SqlString sql,
      SqlType[] parameterTypes)
    {
      IDbCommand dbCommand = this.Generate(type, sql, parameterTypes);
      this._lastQuery = dbCommand;
      return dbCommand;
    }

    public void AbortBatch(Exception e)
    {
      IDbCommand batchCommand = this._batchCommand;
      this.InvalidateBatchCommand();
      if (batchCommand == null)
        return;
      this.CloseCommand(batchCommand, (IDataReader) null);
    }

    private void InvalidateBatchCommand()
    {
      this._batchCommand = (IDbCommand) null;
      this._batchCommandSql = (SqlString) null;
      this._batchCommandParameterTypes = (SqlType[]) null;
    }

    public int ExecuteNonQuery(IDbCommand cmd)
    {
      this.CheckReaders();
      this.LogCommand(cmd);
      this.Prepare(cmd);
      Stopwatch stopwatch = (Stopwatch) null;
      if (AbstractBatcher.Log.IsDebugEnabled)
        stopwatch = Stopwatch.StartNew();
      try
      {
        return cmd.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ex.Data[(object) "actual-sql-query"] = (object) cmd.CommandText;
        AbstractBatcher.Log.Error((object) ("Could not execute command: " + cmd.CommandText), ex);
        throw;
      }
      finally
      {
        if (AbstractBatcher.Log.IsDebugEnabled && stopwatch != null)
          AbstractBatcher.Log.DebugFormat("ExecuteNonQuery took {0} ms", (object) stopwatch.ElapsedMilliseconds);
      }
    }

    public virtual IDataReader ExecuteReader(IDbCommand cmd)
    {
      this.CheckReaders();
      this.LogCommand(cmd);
      this.Prepare(cmd);
      Stopwatch stopwatch = (Stopwatch) null;
      if (AbstractBatcher.Log.IsDebugEnabled)
        stopwatch = Stopwatch.StartNew();
      IDataReader dataReader = (IDataReader) null;
      try
      {
        dataReader = cmd.ExecuteReader();
      }
      catch (Exception ex)
      {
        ex.Data[(object) "actual-sql-query"] = (object) cmd.CommandText;
        AbstractBatcher.Log.Error((object) ("Could not execute query: " + cmd.CommandText), ex);
        throw;
      }
      finally
      {
        if (AbstractBatcher.Log.IsDebugEnabled && stopwatch != null && dataReader != null)
        {
          AbstractBatcher.Log.DebugFormat("ExecuteReader took {0} ms", (object) stopwatch.ElapsedMilliseconds);
          this._readersDuration[dataReader] = stopwatch;
        }
      }
      if (!this._factory.ConnectionProvider.Driver.SupportsMultipleOpenReaders)
        dataReader = (IDataReader) new NHybridDataReader(dataReader);
      this._readersToClose.Add(dataReader);
      AbstractBatcher.LogOpenReader();
      return dataReader;
    }

    protected void CheckReaders()
    {
      if (this._factory.ConnectionProvider.Driver.SupportsMultipleOpenReaders)
        return;
      foreach (NHybridDataReader nhybridDataReader in (IEnumerable<IDataReader>) this._readersToClose)
        nhybridDataReader.ReadIntoMemory();
    }

    public void CloseCommands()
    {
      this._releasing = true;
      try
      {
        foreach (IDataReader reader in (Set<IDataReader>) new HashedSet<IDataReader>((ICollection<IDataReader>) this._readersToClose))
        {
          try
          {
            this.CloseReader(reader);
          }
          catch (Exception ex)
          {
            AbstractBatcher.Log.Warn((object) "Could not close IDataReader", ex);
          }
        }
        foreach (IDbCommand cmd in (IEnumerable<IDbCommand>) this._commandsToClose)
        {
          try
          {
            this.CloseCommand(cmd);
          }
          catch (Exception ex)
          {
            AbstractBatcher.Log.Warn((object) "Could not close ADO.NET Command", ex);
          }
        }
        this._commandsToClose.Clear();
      }
      finally
      {
        this._releasing = false;
      }
    }

    private void CloseCommand(IDbCommand cmd)
    {
      try
      {
        cmd.Dispose();
        this.LogClosePreparedCommand();
      }
      catch (Exception ex)
      {
        AbstractBatcher.Log.Warn((object) "exception clearing maxRows/queryTimeout", ex);
        return;
      }
      finally
      {
        if (!this._releasing)
          this._connectionManager.AfterStatement();
      }
      if (this._lastQuery != cmd)
        return;
      this._lastQuery = (IDbCommand) null;
    }

    public void CloseCommand(IDbCommand st, IDataReader reader)
    {
      this._commandsToClose.Remove(st);
      try
      {
        this.CloseReader(reader);
      }
      finally
      {
        this.CloseCommand(st);
      }
    }

    public void CloseReader(IDataReader reader)
    {
      if (reader == null)
        return;
      IDataReader dataReader = !(reader is ResultSetWrapper resultSetWrapper) ? reader : resultSetWrapper.Target;
      this._readersToClose.Remove(dataReader);
      try
      {
        reader.Dispose();
      }
      catch (Exception ex)
      {
        AbstractBatcher.Log.Warn((object) "exception closing reader", ex);
      }
      AbstractBatcher.LogCloseReader();
      if (!AbstractBatcher.Log.IsDebugEnabled)
        return;
      IDataReader key = !(dataReader is NHybridDataReader nhybridDataReader) ? dataReader : nhybridDataReader.Target;
      Stopwatch stopwatch;
      if (!this._readersDuration.TryGetValue(key, out stopwatch))
        return;
      this._readersDuration.Remove(key);
      AbstractBatcher.Log.DebugFormat("DataReader was closed after {0} ms", (object) stopwatch.ElapsedMilliseconds);
    }

    public void ExecuteBatch()
    {
      if (this._batchCommand == null)
        return;
      IDbCommand batchCommand = this._batchCommand;
      this.InvalidateBatchCommand();
      try
      {
        this.ExecuteBatchWithTiming(batchCommand);
      }
      finally
      {
        this.CloseCommand(batchCommand, (IDataReader) null);
      }
    }

    protected void ExecuteBatchWithTiming(IDbCommand ps)
    {
      Stopwatch stopwatch = (Stopwatch) null;
      if (AbstractBatcher.Log.IsDebugEnabled)
        stopwatch = Stopwatch.StartNew();
      int statementsInCurrentBatch = this.CountOfStatementsInCurrentBatch;
      this.DoExecuteBatch(ps);
      if (!AbstractBatcher.Log.IsDebugEnabled || stopwatch == null)
        return;
      AbstractBatcher.Log.DebugFormat("ExecuteBatch for {0} statements took {1} ms", (object) statementsInCurrentBatch, (object) stopwatch.ElapsedMilliseconds);
    }

    protected abstract void DoExecuteBatch(IDbCommand ps);

    protected abstract int CountOfStatementsInCurrentBatch { get; }

    public abstract int BatchSize { get; set; }

    public abstract void AddToBatch(IExpectation expectation);

    protected ISessionFactoryImplementor Factory => this._factory;

    protected ConnectionManager ConnectionManager => this._connectionManager;

    protected void LogCommand(IDbCommand command)
    {
      this._factory.Settings.SqlStatementLogger.LogCommand(command, FormatStyle.Basic);
    }

    private void LogOpenPreparedCommand()
    {
      if (AbstractBatcher.Log.IsDebugEnabled)
      {
        int num = Interlocked.Increment(ref AbstractBatcher._openCommandCount);
        AbstractBatcher.Log.Debug((object) ("Opened new IDbCommand, open IDbCommands: " + (object) num));
      }
      if (!this._factory.Statistics.IsStatisticsEnabled)
        return;
      this._factory.StatisticsImplementor.PrepareStatement();
    }

    private void LogClosePreparedCommand()
    {
      if (AbstractBatcher.Log.IsDebugEnabled)
      {
        int num = Interlocked.Decrement(ref AbstractBatcher._openCommandCount);
        AbstractBatcher.Log.Debug((object) ("Closed IDbCommand, open IDbCommands: " + (object) num));
      }
      if (!this._factory.Statistics.IsStatisticsEnabled)
        return;
      this._factory.StatisticsImplementor.CloseStatement();
    }

    private static void LogOpenReader()
    {
      if (!AbstractBatcher.Log.IsDebugEnabled)
        return;
      int num = Interlocked.Increment(ref AbstractBatcher._openReaderCount);
      AbstractBatcher.Log.Debug((object) ("Opened IDataReader, open IDataReaders: " + (object) num));
    }

    private static void LogCloseReader()
    {
      if (!AbstractBatcher.Log.IsDebugEnabled)
        return;
      int num = Interlocked.Decrement(ref AbstractBatcher._openReaderCount);
      AbstractBatcher.Log.Debug((object) ("Closed IDataReader, open IDataReaders :" + (object) num));
    }

    public void CancelLastQuery()
    {
      try
      {
        if (this._lastQuery == null)
          return;
        this._lastQuery.Cancel();
      }
      catch (HibernateException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw this.Convert(ex, "Could not cancel query");
      }
    }

    public bool HasOpenResources
    {
      get => this._commandsToClose.Count > 0 || this._readersToClose.Count > 0;
    }

    protected Exception Convert(Exception sqlException, string message)
    {
      return ADOExceptionHelper.Convert(this.Factory.SQLExceptionConverter, sqlException, message);
    }

    ~AbstractBatcher() => this.Dispose(false);

    public void Dispose()
    {
      AbstractBatcher.Log.Debug((object) "running BatcherImpl.Dispose(true)");
      this.Dispose(true);
    }

    protected virtual void Dispose(bool isDisposing)
    {
      if (this._isAlreadyDisposed)
        return;
      if (isDisposing)
        this.CloseCommands();
      this._isAlreadyDisposed = true;
      GC.SuppressFinalize((object) this);
    }

    protected SqlString GetSQL(SqlString sql)
    {
      sql = this._interceptor.OnPrepareStatement(sql);
      return sql != null && sql.Length != 0 ? sql : throw new AssertionFailure("Interceptor.OnPrepareStatement(SqlString) returned null or empty SqlString.");
    }
  }
}
