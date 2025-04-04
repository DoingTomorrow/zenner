// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.DriverBase
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace NHibernate.Driver
{
  public abstract class DriverBase : IDriver, ISqlParameterFormatter
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (DriverBase));
    private int commandTimeout;
    private bool prepareSql;

    public virtual void Configure(IDictionary<string, string> settings)
    {
      this.commandTimeout = PropertiesHelper.GetInt32("command_timeout", settings, -1);
      if (this.commandTimeout > -1 && DriverBase.log.IsInfoEnabled)
        DriverBase.log.Info((object) string.Format("setting ADO.NET command timeout to {0} seconds", (object) this.commandTimeout));
      this.prepareSql = PropertiesHelper.GetBoolean("prepare_sql", settings, false);
      if (!this.prepareSql || !this.SupportsPreparingCommands)
        return;
      DriverBase.log.Info((object) "preparing SQL enabled");
    }

    protected bool IsPrepareSqlEnabled => this.prepareSql;

    public abstract IDbConnection CreateConnection();

    public abstract IDbCommand CreateCommand();

    public abstract bool UseNamedPrefixInSql { get; }

    public abstract bool UseNamedPrefixInParameter { get; }

    public abstract string NamedPrefix { get; }

    public string FormatNameForSql(string parameterName)
    {
      return !this.UseNamedPrefixInSql ? "?" : this.NamedPrefix + parameterName;
    }

    public string FormatNameForParameter(string parameterName)
    {
      return !this.UseNamedPrefixInParameter ? parameterName : this.NamedPrefix + parameterName;
    }

    public virtual bool SupportsMultipleOpenReaders => true;

    protected virtual bool SupportsPreparingCommands => true;

    public virtual IDbCommand GenerateCommand(
      CommandType type,
      SqlString sqlString,
      SqlType[] parameterTypes)
    {
      IDbCommand command = this.CreateCommand();
      command.CommandType = type;
      this.SetCommandTimeout(command);
      this.SetCommandText(command, sqlString);
      this.SetCommandParameters(command, parameterTypes);
      return command;
    }

    private void SetCommandTimeout(IDbCommand cmd)
    {
      if (this.commandTimeout < 0)
        return;
      try
      {
        cmd.CommandTimeout = this.commandTimeout;
      }
      catch (Exception ex)
      {
        if (!DriverBase.log.IsWarnEnabled)
          return;
        DriverBase.log.Warn((object) ex.ToString());
      }
    }

    private static string ToParameterName(int index) => "p" + (object) index;

    string ISqlParameterFormatter.GetParameterName(int index)
    {
      return this.FormatNameForSql(DriverBase.ToParameterName(index));
    }

    private void SetCommandText(IDbCommand cmd, SqlString sqlString)
    {
      SqlStringFormatter sqlStringFormatter = this.GetSqlStringFormatter();
      sqlStringFormatter.Format(sqlString);
      cmd.CommandText = sqlStringFormatter.GetFormattedText();
    }

    protected virtual SqlStringFormatter GetSqlStringFormatter()
    {
      return new SqlStringFormatter((ISqlParameterFormatter) this, ";");
    }

    private void SetCommandParameters(IDbCommand cmd, SqlType[] sqlTypes)
    {
      for (int index = 0; index < sqlTypes.Length; ++index)
      {
        string parameterName = DriverBase.ToParameterName(index);
        IDbDataParameter parameter = this.GenerateParameter(cmd, parameterName, sqlTypes[index]);
        cmd.Parameters.Add((object) parameter);
      }
    }

    protected virtual void InitializeParameter(
      IDbDataParameter dbParam,
      string name,
      SqlType sqlType)
    {
      if (sqlType == null)
        throw new QueryException(string.Format("No type assigned to parameter '{0}'", (object) name));
      dbParam.ParameterName = this.FormatNameForParameter(name);
      dbParam.DbType = sqlType.DbType;
    }

    public IDbDataParameter GenerateParameter(IDbCommand command, string name, SqlType sqlType)
    {
      IDbDataParameter parameter = command.CreateParameter();
      this.InitializeParameter(parameter, name, sqlType);
      return parameter;
    }

    public void RemoveUnusedCommandParameters(IDbCommand cmd, SqlString sqlString)
    {
      if (!this.UseNamedPrefixInSql)
        return;
      SqlStringFormatter sqlStringFormatter = this.GetSqlStringFormatter();
      sqlStringFormatter.Format(sqlString);
      HashSet<string> assignedParameterNames = new HashSet<string>((IEnumerable<string>) sqlStringFormatter.AssignedParameterNames);
      cmd.Parameters.Cast<IDbDataParameter>().Select<IDbDataParameter, string>((System.Func<IDbDataParameter, string>) (p => p.ParameterName)).Where<string>((System.Func<string, bool>) (p => !assignedParameterNames.Contains(this.UseNamedPrefixInParameter ? p : this.FormatNameForSql(p)))).ToList<string>().ForEach((Action<string>) (unusedParameterName => cmd.Parameters.RemoveAt(unusedParameterName)));
    }

    public virtual void ExpandQueryParameters(IDbCommand cmd, SqlString sqlString)
    {
      if (this.UseNamedPrefixInSql)
        return;
      List<IDbDataParameter> dbDataParameterList = new List<IDbDataParameter>();
      foreach (object part in (IEnumerable) sqlString.Parts)
      {
        if ((object) (part as Parameter) != null)
        {
          Parameter parameter1 = (Parameter) part;
          IDbDataParameter parameter2 = (IDbDataParameter) cmd.Parameters[parameter1.ParameterPosition.Value];
          dbDataParameterList.Add(this.CloneParameter(cmd, parameter2));
        }
      }
      cmd.Parameters.Clear();
      foreach (IDbDataParameter dbDataParameter in dbDataParameterList)
        cmd.Parameters.Add((object) dbDataParameter);
    }

    public virtual IResultSetsCommand GetResultSetsCommand(ISessionImplementor session)
    {
      throw new NotSupportedException(string.Format("The driver {0} does not support multiple queries.", (object) this.GetType().FullName));
    }

    public virtual bool SupportsMultipleQueries => false;

    protected virtual IDbDataParameter CloneParameter(
      IDbCommand cmd,
      IDbDataParameter originalParameter)
    {
      IDbDataParameter parameter = cmd.CreateParameter();
      parameter.DbType = originalParameter.DbType;
      parameter.ParameterName = originalParameter.ParameterName;
      parameter.Value = originalParameter.Value;
      return parameter;
    }

    public void PrepareCommand(IDbCommand command)
    {
      this.AdjustCommand(command);
      this.OnBeforePrepare(command);
      if (!this.SupportsPreparingCommands || !this.prepareSql)
        return;
      command.Prepare();
    }

    protected virtual void OnBeforePrepare(IDbCommand command)
    {
    }

    public virtual void AdjustCommand(IDbCommand command)
    {
    }

    public IDbDataParameter GenerateOutputParameter(IDbCommand command)
    {
      IDbDataParameter parameter = this.GenerateParameter(command, "ReturnValue", SqlTypeFactory.Int32);
      parameter.Direction = ParameterDirection.Output;
      return parameter;
    }
  }
}
