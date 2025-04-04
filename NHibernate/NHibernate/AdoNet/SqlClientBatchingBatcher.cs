// Decompiled with JetBrains decompiler
// Type: NHibernate.AdoNet.SqlClientBatchingBatcher
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.AdoNet.Util;
using NHibernate.Exceptions;
using NHibernate.Util;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

#nullable disable
namespace NHibernate.AdoNet
{
  public class SqlClientBatchingBatcher : AbstractBatcher
  {
    private int _batchSize;
    private int _totalExpectedRowsAffected;
    private SqlClientSqlCommandSet _currentBatch;
    private StringBuilder _currentBatchCommandsLog;
    private readonly int _defaultTimeout;

    public SqlClientBatchingBatcher(ConnectionManager connectionManager, IInterceptor interceptor)
      : base(connectionManager, interceptor)
    {
      this._batchSize = this.Factory.Settings.AdoBatchSize;
      this._defaultTimeout = PropertiesHelper.GetInt32("command_timeout", NHibernate.Cfg.Environment.Properties, -1);
      this._currentBatch = this.CreateConfiguredBatch();
      this._currentBatchCommandsLog = new StringBuilder().AppendLine("Batch commands:");
    }

    public override int BatchSize
    {
      get => this._batchSize;
      set => this._batchSize = value;
    }

    protected override int CountOfStatementsInCurrentBatch => this._currentBatch.CountOfCommands;

    public override void AddToBatch(IExpectation expectation)
    {
      this._totalExpectedRowsAffected += expectation.ExpectedRowCount;
      IDbCommand currentCommand = this.CurrentCommand;
      this.Driver.AdjustCommand(currentCommand);
      string str = (string) null;
      SqlStatementLogger sqlStatementLogger = this.Factory.Settings.SqlStatementLogger;
      if (sqlStatementLogger.IsDebugEnabled || AbstractBatcher.Log.IsDebugEnabled)
      {
        string lineWithParameters = sqlStatementLogger.GetCommandLineWithParameters(currentCommand);
        str = sqlStatementLogger.DetermineActualStyle(FormatStyle.Basic).Formatter.Format(lineWithParameters);
        this._currentBatchCommandsLog.Append("command ").Append(this._currentBatch.CountOfCommands).Append(":").AppendLine(str);
      }
      if (AbstractBatcher.Log.IsDebugEnabled)
        AbstractBatcher.Log.Debug((object) ("Adding to batch:" + str));
      this._currentBatch.Append((SqlCommand) currentCommand);
      if (this._currentBatch.CountOfCommands < this._batchSize)
        return;
      this.ExecuteBatchWithTiming(currentCommand);
    }

    protected override void DoExecuteBatch(IDbCommand ps)
    {
      AbstractBatcher.Log.DebugFormat("Executing batch");
      this.CheckReaders();
      this.Prepare((IDbCommand) this._currentBatch.BatchCommand);
      if (this.Factory.Settings.SqlStatementLogger.IsDebugEnabled)
      {
        this.Factory.Settings.SqlStatementLogger.LogBatchCommand(this._currentBatchCommandsLog.ToString());
        this._currentBatchCommandsLog = new StringBuilder().AppendLine("Batch commands:");
      }
      int rowCount;
      try
      {
        rowCount = this._currentBatch.ExecuteNonQuery();
      }
      catch (DbException ex)
      {
        throw ADOExceptionHelper.Convert(this.Factory.SQLExceptionConverter, (Exception) ex, "could not execute batch command.");
      }
      Expectations.VerifyOutcomeBatched(this._totalExpectedRowsAffected, rowCount);
      this._currentBatch.Dispose();
      this._totalExpectedRowsAffected = 0;
      this._currentBatch = this.CreateConfiguredBatch();
    }

    private SqlClientSqlCommandSet CreateConfiguredBatch()
    {
      SqlClientSqlCommandSet configuredBatch = new SqlClientSqlCommandSet();
      if (this._defaultTimeout > 0)
      {
        try
        {
          configuredBatch.CommandTimeout = this._defaultTimeout;
        }
        catch (Exception ex)
        {
          if (AbstractBatcher.Log.IsWarnEnabled)
            AbstractBatcher.Log.Warn((object) ex.ToString());
        }
      }
      return configuredBatch;
    }
  }
}
