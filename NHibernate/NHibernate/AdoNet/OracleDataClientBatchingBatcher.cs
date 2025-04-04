// Decompiled with JetBrains decompiler
// Type: NHibernate.AdoNet.OracleDataClientBatchingBatcher
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.AdoNet.Util;
using NHibernate.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

#nullable disable
namespace NHibernate.AdoNet
{
  public class OracleDataClientBatchingBatcher : AbstractBatcher
  {
    private int _batchSize;
    private int _countOfCommands;
    private int _totalExpectedRowsAffected;
    private IDbCommand _currentBatch;
    private IDictionary<string, List<object>> _parameterValueListHashTable;
    private IDictionary<string, bool> _parameterIsAllNullsHashTable;
    private StringBuilder _currentBatchCommandsLog;

    public OracleDataClientBatchingBatcher(
      ConnectionManager connectionManager,
      IInterceptor interceptor)
      : base(connectionManager, interceptor)
    {
      this._batchSize = this.Factory.Settings.AdoBatchSize;
      this._currentBatchCommandsLog = new StringBuilder().AppendLine("Batch commands:");
    }

    public override void AddToBatch(IExpectation expectation)
    {
      bool flag = true;
      this._totalExpectedRowsAffected += expectation.ExpectedRowCount;
      string str = (string) null;
      SqlStatementLogger sqlStatementLogger = this.Factory.Settings.SqlStatementLogger;
      if (sqlStatementLogger.IsDebugEnabled || AbstractBatcher.Log.IsDebugEnabled)
      {
        string lineWithParameters = sqlStatementLogger.GetCommandLineWithParameters(this.CurrentCommand);
        str = sqlStatementLogger.DetermineActualStyle(FormatStyle.Basic).Formatter.Format(lineWithParameters);
        this._currentBatchCommandsLog.Append("command ").Append(this._countOfCommands).Append(":").AppendLine(str);
      }
      if (AbstractBatcher.Log.IsDebugEnabled)
        AbstractBatcher.Log.Debug((object) ("Adding to batch:" + str));
      if (this._currentBatch == null)
      {
        this._currentBatch = this.CurrentCommand;
        this._parameterValueListHashTable = (IDictionary<string, List<object>>) new Dictionary<string, List<object>>();
        this._parameterIsAllNullsHashTable = (IDictionary<string, bool>) new Dictionary<string, bool>();
      }
      else
        flag = false;
      foreach (IDataParameter parameter in (IEnumerable) this.CurrentCommand.Parameters)
      {
        List<object> objectList;
        if (flag)
        {
          objectList = new List<object>();
          this._parameterValueListHashTable.Add(parameter.ParameterName, objectList);
          this._parameterIsAllNullsHashTable.Add(parameter.ParameterName, true);
        }
        else
          objectList = this._parameterValueListHashTable[parameter.ParameterName];
        if (parameter.Value != DBNull.Value)
          this._parameterIsAllNullsHashTable[parameter.ParameterName] = false;
        objectList.Add(parameter.Value);
      }
      ++this._countOfCommands;
      if (this._countOfCommands < this._batchSize)
        return;
      this.ExecuteBatchWithTiming(this._currentBatch);
    }

    protected override void DoExecuteBatch(IDbCommand ps)
    {
      if (this._currentBatch == null)
        return;
      int paramValue = 0;
      this._countOfCommands = 0;
      AbstractBatcher.Log.Info((object) "Executing batch");
      this.CheckReaders();
      this.Prepare(this._currentBatch);
      if (this.Factory.Settings.SqlStatementLogger.IsDebugEnabled)
      {
        this.Factory.Settings.SqlStatementLogger.LogBatchCommand(this._currentBatchCommandsLog.ToString());
        this._currentBatchCommandsLog = new StringBuilder().AppendLine("Batch commands:");
      }
      foreach (IDataParameter parameter in (IEnumerable) this._currentBatch.Parameters)
      {
        List<object> objectList = this._parameterValueListHashTable[parameter.ParameterName];
        parameter.Value = (object) objectList.ToArray();
        paramValue = objectList.Count;
      }
      this.SetObjectParam((object) this._currentBatch, "ArrayBindCount", (object) paramValue);
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
      this._totalExpectedRowsAffected = 0;
      this._currentBatch = (IDbCommand) null;
      this._parameterValueListHashTable = (IDictionary<string, List<object>>) null;
    }

    protected override int CountOfStatementsInCurrentBatch => this._countOfCommands;

    private void SetObjectParam(object obj, string paramName, object paramValue)
    {
      obj.GetType().GetProperty(paramName).SetValue(obj, paramValue, (object[]) null);
    }

    public override int BatchSize
    {
      get => this._batchSize;
      set => this._batchSize = value;
    }
  }
}
