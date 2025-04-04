// Decompiled with JetBrains decompiler
// Type: NLog.Targets.DatabaseTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using NLog.Layouts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using System.Transactions;

#nullable disable
namespace NLog.Targets
{
  [Target("Database")]
  public class DatabaseTarget : Target, IInstallable
  {
    private static Assembly systemDataAssembly = typeof (IDbConnection).GetAssembly();
    private IDbConnection _activeConnection;
    private string _activeConnectionString;

    public DatabaseTarget()
    {
      this.Parameters = (IList<DatabaseParameterInfo>) new List<DatabaseParameterInfo>();
      this.InstallDdlCommands = (IList<DatabaseCommandInfo>) new List<DatabaseCommandInfo>();
      this.UninstallDdlCommands = (IList<DatabaseCommandInfo>) new List<DatabaseCommandInfo>();
      this.DBProvider = "sqlserver";
      this.DBHost = (Layout) ".";
      this.ConnectionStringsSettings = System.Configuration.ConfigurationManager.ConnectionStrings;
      this.CommandType = CommandType.Text;
      this.OptimizeBufferReuse = this.GetType() == typeof (DatabaseTarget);
    }

    public DatabaseTarget(string name)
      : this()
    {
      this.Name = name;
    }

    [RequiredParameter]
    [DefaultValue("sqlserver")]
    public string DBProvider { get; set; }

    public string ConnectionStringName { get; set; }

    public Layout ConnectionString { get; set; }

    public Layout InstallConnectionString { get; set; }

    [ArrayParameter(typeof (DatabaseCommandInfo), "install-command")]
    public IList<DatabaseCommandInfo> InstallDdlCommands { get; private set; }

    [ArrayParameter(typeof (DatabaseCommandInfo), "uninstall-command")]
    public IList<DatabaseCommandInfo> UninstallDdlCommands { get; private set; }

    [DefaultValue(false)]
    public bool KeepConnection { get; set; }

    [Obsolete("Value will be ignored as logging code always executes outside of a transaction. Marked obsolete on NLog 4.0 and it will be removed in NLog 6.")]
    public bool? UseTransactions { get; set; }

    public Layout DBHost { get; set; }

    public Layout DBUserName { get; set; }

    public Layout DBPassword { get; set; }

    public Layout DBDatabase { get; set; }

    [RequiredParameter]
    public Layout CommandText { get; set; }

    [DefaultValue(CommandType.Text)]
    public CommandType CommandType { get; set; }

    [ArrayParameter(typeof (DatabaseParameterInfo), "parameter")]
    public IList<DatabaseParameterInfo> Parameters { get; private set; }

    internal DbProviderFactory ProviderFactory { get; set; }

    internal ConnectionStringSettingsCollection ConnectionStringsSettings { get; set; }

    internal Type ConnectionType { get; set; }

    public void Install(InstallationContext installationContext)
    {
      this.RunInstallCommands(installationContext, (IEnumerable<DatabaseCommandInfo>) this.InstallDdlCommands);
    }

    public void Uninstall(InstallationContext installationContext)
    {
      this.RunInstallCommands(installationContext, (IEnumerable<DatabaseCommandInfo>) this.UninstallDdlCommands);
    }

    public bool? IsInstalled(InstallationContext installationContext) => new bool?();

    internal IDbConnection OpenConnection(string connectionString)
    {
      IDbConnection dbConnection = this.ProviderFactory == null ? (IDbConnection) Activator.CreateInstance(this.ConnectionType) : (IDbConnection) this.ProviderFactory.CreateConnection();
      if (dbConnection == null)
        throw new NLogRuntimeException("Creation of connection failed");
      dbConnection.ConnectionString = connectionString;
      dbConnection.Open();
      return dbConnection;
    }

    protected override void InitializeTarget()
    {
      base.InitializeTarget();
      if (this.UseTransactions.HasValue)
        InternalLogger.Warn<string>("DatabaseTarget(Name={0}): UseTransactions property is obsolete and will not be used - will be removed in NLog 6", this.Name);
      bool flag = false;
      string providerInvariantName = string.Empty;
      if (!string.IsNullOrEmpty(this.ConnectionStringName))
      {
        ConnectionStringSettings connectionStringsSetting = this.ConnectionStringsSettings[this.ConnectionStringName];
        this.ConnectionString = connectionStringsSetting != null ? (Layout) SimpleLayout.Escape(connectionStringsSetting.ConnectionString) : throw new NLogConfigurationException(string.Format("Connection string '{0}' is not declared in <connectionStrings /> section.", (object) this.ConnectionStringName));
        if (!string.IsNullOrEmpty(connectionStringsSetting.ProviderName))
          providerInvariantName = connectionStringsSetting.ProviderName;
      }
      if (this.ConnectionString != null)
      {
        try
        {
          string str = this.BuildConnectionString(LogEventInfo.CreateNullEvent());
          DbConnectionStringBuilder connectionStringBuilder = new DbConnectionStringBuilder()
          {
            ConnectionString = str
          };
          object obj1;
          if (connectionStringBuilder.TryGetValue("provider connection string", out obj1))
          {
            object obj2;
            if (connectionStringBuilder.TryGetValue("provider", out obj2))
              providerInvariantName = obj2.ToString();
            this.ConnectionString = (Layout) SimpleLayout.Escape(obj1.ToString());
          }
        }
        catch (Exception ex)
        {
          object[] objArray = new object[1]
          {
            (object) this.Name
          };
          InternalLogger.Warn(ex, "DatabaseTarget(Name={0}): DbConnectionStringBuilder failed to parse ConnectionString", objArray);
        }
      }
      if (string.IsNullOrEmpty(providerInvariantName))
      {
        foreach (DataRow row in (InternalDataCollectionBase) DbProviderFactories.GetFactoryClasses().Rows)
        {
          if ((string) row["InvariantName"] == this.DBProvider)
          {
            providerInvariantName = this.DBProvider;
            break;
          }
        }
      }
      if (!string.IsNullOrEmpty(providerInvariantName))
      {
        this.ProviderFactory = DbProviderFactories.GetFactory(providerInvariantName);
        flag = true;
      }
      if (flag)
        return;
      this.SetConnectionType();
    }

    private void SetConnectionType()
    {
      switch (this.DBProvider.ToUpperInvariant())
      {
        case "SQLSERVER":
        case "MSSQL":
        case "MICROSOFT":
        case "MSDE":
          this.ConnectionType = DatabaseTarget.systemDataAssembly.GetType("System.Data.SqlClient.SqlConnection", true, true);
          break;
        case "OLEDB":
          this.ConnectionType = DatabaseTarget.systemDataAssembly.GetType("System.Data.OleDb.OleDbConnection", true);
          break;
        case "ODBC":
          this.ConnectionType = DatabaseTarget.systemDataAssembly.GetType("System.Data.Odbc.OdbcConnection", true);
          break;
        default:
          this.ConnectionType = Type.GetType(this.DBProvider, true);
          break;
      }
    }

    protected override void CloseTarget()
    {
      base.CloseTarget();
      InternalLogger.Trace<string>("DatabaseTarget(Name={0}): Close connection because of CloseTarget", this.Name);
      this.CloseConnection();
    }

    protected override void Write(LogEventInfo logEvent)
    {
      try
      {
        this.WriteEventToDatabase(logEvent);
      }
      catch (Exception ex)
      {
        InternalLogger.Error(ex, "DatabaseTarget(Name={0}): Error when writing to database.", (object) this.Name);
        if (ex.MustBeRethrownImmediately())
        {
          throw;
        }
        else
        {
          InternalLogger.Trace<string>("DatabaseTarget(Name={0}): Close connection because of error", this.Name);
          this.CloseConnection();
          throw;
        }
      }
      finally
      {
        if (!this.KeepConnection)
        {
          InternalLogger.Trace<string>("DatabaseTarget(Name={0}): Close connection (KeepConnection = false).", this.Name);
          this.CloseConnection();
        }
      }
    }

    [Obsolete("Instead override Write(IList<AsyncLogEventInfo> logEvents. Marked obsolete on NLog 4.5")]
    protected override void Write(AsyncLogEventInfo[] logEvents)
    {
      this.Write((IList<AsyncLogEventInfo>) logEvents);
    }

    protected override void Write(IList<AsyncLogEventInfo> logEvents)
    {
      SortHelpers.ReadOnlySingleBucketDictionary<string, IList<AsyncLogEventInfo>> bucketDictionary = logEvents.BucketSort<AsyncLogEventInfo, string>((SortHelpers.KeySelector<AsyncLogEventInfo, string>) (c => this.BuildConnectionString(c.LogEvent)));
      try
      {
        foreach (KeyValuePair<string, IList<AsyncLogEventInfo>> keyValuePair in bucketDictionary)
        {
          for (int index = 0; index < keyValuePair.Value.Count; ++index)
          {
            AsyncLogEventInfo asyncLogEventInfo = keyValuePair.Value[index];
            try
            {
              this.WriteEventToDatabase(asyncLogEventInfo.LogEvent);
              asyncLogEventInfo.Continuation((Exception) null);
            }
            catch (Exception ex)
            {
              InternalLogger.Error(ex, "DatabaseTarget(Name={0}): Error when writing to database.", (object) this.Name);
              if (ex.MustBeRethrownImmediately())
              {
                throw;
              }
              else
              {
                InternalLogger.Trace<string>("DatabaseTarget(Name={0}): Close connection because of exception", this.Name);
                this.CloseConnection();
                asyncLogEventInfo.Continuation(ex);
                if (ex.MustBeRethrown())
                  throw;
              }
            }
          }
        }
      }
      finally
      {
        if (!this.KeepConnection)
        {
          InternalLogger.Trace<string>("DatabaseTarget(Name={0}): Close connection because of KeepConnection=false", this.Name);
          this.CloseConnection();
        }
      }
    }

    private void WriteEventToDatabase(LogEventInfo logEvent)
    {
      using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Suppress))
      {
        this.EnsureConnectionOpen(this.BuildConnectionString(logEvent));
        using (IDbCommand command = this._activeConnection.CreateCommand())
        {
          command.CommandText = this.RenderLogEvent(this.CommandText, logEvent);
          command.CommandType = this.CommandType;
          InternalLogger.Trace<string, CommandType, string>("DatabaseTarget(Name={0}): Executing {1}: {2}", this.Name, command.CommandType, command.CommandText);
          for (int index = 0; index < this.Parameters.Count; ++index)
          {
            DatabaseParameterInfo parameter1 = this.Parameters[index];
            IDbDataParameter parameter2 = command.CreateParameter();
            parameter2.Direction = ParameterDirection.Input;
            if (parameter1.Name != null)
              parameter2.ParameterName = parameter1.Name;
            if (parameter1.Size != 0)
              parameter2.Size = parameter1.Size;
            if (parameter1.Precision != (byte) 0)
              parameter2.Precision = parameter1.Precision;
            if (parameter1.Scale != (byte) 0)
              parameter2.Scale = parameter1.Scale;
            string str = this.RenderLogEvent(parameter1.Layout, logEvent);
            parameter2.Value = (object) str;
            command.Parameters.Add((object) parameter2);
            InternalLogger.Trace<string, object, DbType>("  DatabaseTarget: Parameter: '{0}' = '{1}' ({2})", parameter2.ParameterName, parameter2.Value, parameter2.DbType);
          }
          InternalLogger.Trace<string, int>("DatabaseTarget(Name={0}): Finished execution, result = {1}", this.Name, command.ExecuteNonQuery());
        }
        transactionScope.Complete();
      }
    }

    protected string BuildConnectionString(LogEventInfo logEvent)
    {
      if (this.ConnectionString != null)
        return this.RenderLogEvent(this.ConnectionString, logEvent);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Server=");
      stringBuilder.Append(this.RenderLogEvent(this.DBHost, logEvent));
      stringBuilder.Append(";");
      if (this.DBUserName == null)
      {
        stringBuilder.Append("Trusted_Connection=SSPI;");
      }
      else
      {
        stringBuilder.Append("User id=");
        stringBuilder.Append(this.RenderLogEvent(this.DBUserName, logEvent));
        stringBuilder.Append(";Password=");
        stringBuilder.Append(this.RenderLogEvent(this.DBPassword, logEvent));
        stringBuilder.Append(";");
      }
      if (this.DBDatabase != null)
      {
        stringBuilder.Append("Database=");
        stringBuilder.Append(this.RenderLogEvent(this.DBDatabase, logEvent));
      }
      return stringBuilder.ToString();
    }

    private void EnsureConnectionOpen(string connectionString)
    {
      if (this._activeConnection != null && this._activeConnectionString != connectionString)
      {
        InternalLogger.Trace<string>("DatabaseTarget(Name={0}): Close connection because of opening new.", this.Name);
        this.CloseConnection();
      }
      if (this._activeConnection != null)
        return;
      InternalLogger.Trace<string>("DatabaseTarget(Name={0}): Open connection.", this.Name);
      this._activeConnection = this.OpenConnection(connectionString);
      this._activeConnectionString = connectionString;
    }

    private void CloseConnection()
    {
      if (this._activeConnection == null)
        return;
      this._activeConnection.Close();
      this._activeConnection.Dispose();
      this._activeConnection = (IDbConnection) null;
      this._activeConnectionString = (string) null;
    }

    private void RunInstallCommands(
      InstallationContext installationContext,
      IEnumerable<DatabaseCommandInfo> commands)
    {
      LogEventInfo logEvent = installationContext.CreateLogEvent();
      try
      {
        foreach (DatabaseCommandInfo command1 in commands)
        {
          string connectionString = command1.ConnectionString == null ? (this.InstallConnectionString == null ? this.BuildConnectionString(logEvent) : this.RenderLogEvent(this.InstallConnectionString, logEvent)) : this.RenderLogEvent(command1.ConnectionString, logEvent);
          if (this.ConnectionType == (Type) null)
            this.SetConnectionType();
          this.EnsureConnectionOpen(connectionString);
          using (IDbCommand command2 = this._activeConnection.CreateCommand())
          {
            command2.CommandType = command1.CommandType;
            command2.CommandText = this.RenderLogEvent(command1.Text, logEvent);
            try
            {
              installationContext.Trace("DatabaseTarget(Name={0}) - Executing {1} '{2}'", (object) this.Name, (object) command2.CommandType, (object) command2.CommandText);
              command2.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
              if (ex.MustBeRethrownImmediately())
                throw;
              else if (command1.IgnoreFailures || installationContext.IgnoreFailures)
              {
                installationContext.Warning(ex.Message);
              }
              else
              {
                installationContext.Error(ex.Message);
                throw;
              }
            }
          }
        }
      }
      finally
      {
        InternalLogger.Trace<string>("DatabaseTarget(Name={0}): Close connection after install.", this.Name);
        this.CloseConnection();
      }
    }
  }
}
