// Decompiled with JetBrains decompiler
// Type: GmmDbLib.BaseDbConnection
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using GmmDbLib.DataSets;
using NLog;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Data.SqlServerCe;
using System.IO;
using System.Text;
using System.Threading;

#nullable disable
namespace GmmDbLib
{
  public class BaseDbConnection
  {
    private static Logger DatabaseIdGenLogger = LogManager.GetLogger("DatabaseIdGen");
    private static Logger DatabaseAccessLogger = LogManager.GetLogger("DatabaseAccess");
    public DbConnectionInfo ConnectionInfo;
    public DbProviderFactory ProviderFactory;
    public string DatabaseLocationName;
    internal DbConnection TemplateConnecton;
    internal DbConnection lastConnection;
    private int? MeterIdRangeFirstNumber = new int?();
    private int MeterIdRangeLastNumber;

    public SortedList<string, string> DatabaseIdentification { get; private set; }

    [Obsolete]
    public BaseDbConnection(MeterDbTypes DbType, string connectionString)
    {
      this.NewFromConnectionInfo(new DbConnectionInfo()
      {
        DbType = DbType,
        ConnectionString = connectionString
      });
    }

    public BaseDbConnection(DbConnectionInfo connectionInfo)
    {
      this.NewFromConnectionInfo(connectionInfo);
    }

    internal void NewFromConnectionInfo(DbConnectionInfo connectionInfo)
    {
      this.ConnectionInfo = connectionInfo;
      try
      {
        switch (connectionInfo.DbType)
        {
          case MeterDbTypes.Access:
            this.CreateConnectionOleDbFactory();
            goto case MeterDbTypes.DBISAM;
          case MeterDbTypes.NPGSQL:
            this.CreateConnectionNpgsqlFactory();
            goto case MeterDbTypes.DBISAM;
          case MeterDbTypes.SQLite:
            this.CreateConnectionSQLiteFactory();
            goto case MeterDbTypes.DBISAM;
          case MeterDbTypes.DBISAM:
            if (!string.IsNullOrEmpty(this.ConnectionInfo.DatabaseName))
              break;
            try
            {
              this.ConnectionInfo.DatabaseName = Path.GetFileNameWithoutExtension(this.ConnectionInfo.UrlOrPath);
            }
            catch
            {
            }
            break;
          case MeterDbTypes.MSSQL:
            this.CreateConnectionSqlClientFactory();
            goto case MeterDbTypes.DBISAM;
          case MeterDbTypes.LocalDB:
            this.CreateConnectionSqlClientFactoryLocalDB();
            goto case MeterDbTypes.DBISAM;
          case MeterDbTypes.Microsoft_SQL_Compact:
            this.CreateConnectionSqlCeProviderFactory();
            goto case MeterDbTypes.DBISAM;
          default:
            throw new Exception("Database type not supported!");
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Create database connection error.", ex);
      }
    }

    private void CreateConnectionSQLiteFactory()
    {
      DbConnectionStringBuilder connectionStringBuilder = new DbConnectionStringBuilder();
      if (!string.IsNullOrEmpty(this.ConnectionInfo.ConnectionString))
      {
        connectionStringBuilder.ConnectionString = this.ConnectionInfo.ConnectionString;
        object obj;
        if (connectionStringBuilder.TryGetValue("Data Source", out obj))
          this.ConnectionInfo.UrlOrPath = (string) obj;
        if (connectionStringBuilder.TryGetValue("Password", out obj))
          this.ConnectionInfo.Password = (string) obj;
        connectionStringBuilder.Clear();
      }
      this.ProviderFactory = (DbProviderFactory) SQLiteFactory.Instance;
      this.TemplateConnecton = this.ProviderFactory.CreateConnection();
      connectionStringBuilder.Add("Data Source", (object) this.ConnectionInfo.UrlOrPath);
      connectionStringBuilder.Add("UTF8Encoding", (object) "True");
      connectionStringBuilder.Add("Password", (object) "meterdbpass");
      connectionStringBuilder.Add("journal mode", (object) "wal");
      connectionStringBuilder.Add("synchronous", (object) "off");
      connectionStringBuilder.Add("BinaryGUID", (object) "True");
      connectionStringBuilder.Add("FailIfMissing", (object) "True");
      this.ConnectionInfo.ConnectionString = connectionStringBuilder.ConnectionString;
      this.TemplateConnecton.ConnectionString = this.ConnectionInfo.ConnectionString;
    }

    private void CreateConnectionSqlClientFactory()
    {
      DbConnectionStringBuilder connectionStringBuilder = new DbConnectionStringBuilder();
      if (!string.IsNullOrEmpty(this.ConnectionInfo.ConnectionString))
      {
        connectionStringBuilder.ConnectionString = this.ConnectionInfo.ConnectionString;
        object obj;
        if (connectionStringBuilder.TryGetValue("Data Source", out obj))
          this.ConnectionInfo.UrlOrPath = (string) obj;
        if (connectionStringBuilder.TryGetValue("Database", out obj))
          this.ConnectionInfo.DatabaseName = (string) obj;
        if (connectionStringBuilder.TryGetValue("User Id", out obj))
          this.ConnectionInfo.UserName = (string) obj;
        if (connectionStringBuilder.TryGetValue("Password", out obj))
          this.ConnectionInfo.Password = (string) obj;
        connectionStringBuilder.Clear();
      }
      this.ProviderFactory = (DbProviderFactory) SqlClientFactory.Instance;
      this.TemplateConnecton = this.ProviderFactory.CreateConnection();
      connectionStringBuilder.Add("Data Source", (object) this.ConnectionInfo.UrlOrPath);
      connectionStringBuilder.Add("Database", (object) this.ConnectionInfo.DatabaseName);
      connectionStringBuilder.Add("User ID", (object) this.ConnectionInfo.UserName);
      connectionStringBuilder.Add("Password", (object) this.ConnectionInfo.Password);
      connectionStringBuilder.Add("Persist Security Info", (object) "True");
      this.ConnectionInfo.ConnectionString = connectionStringBuilder.ConnectionString;
      this.TemplateConnecton.ConnectionString = this.ConnectionInfo.ConnectionString;
    }

    private void CreateConnectionSqlCeProviderFactory()
    {
      this.ProviderFactory = (DbProviderFactory) SqlCeProviderFactory.Instance;
      this.TemplateConnecton = this.ProviderFactory.CreateConnection();
      this.TemplateConnecton.ConnectionString = "data source=" + this.ConnectionInfo.UrlOrPath + ";mode=Exclusive";
    }

    private void CreateConnectionSqlClientFactoryLocalDB()
    {
      DbConnectionStringBuilder connectionStringBuilder = new DbConnectionStringBuilder();
      if (!string.IsNullOrEmpty(this.ConnectionInfo.ConnectionString))
      {
        connectionStringBuilder.ConnectionString = this.ConnectionInfo.ConnectionString;
        object obj;
        if (connectionStringBuilder.TryGetValue("AttachDbFilename", out obj))
          this.ConnectionInfo.UrlOrPath = (string) obj;
        connectionStringBuilder.Clear();
      }
      this.ProviderFactory = (DbProviderFactory) SqlClientFactory.Instance;
      this.TemplateConnecton = this.ProviderFactory.CreateConnection();
      connectionStringBuilder.Add("AttachDbFilename", (object) this.ConnectionInfo.UrlOrPath);
      connectionStringBuilder.Add("Data Source", (object) "(LocalDB)\\v11.0");
      connectionStringBuilder.Add("Integrated Security", (object) "True");
      connectionStringBuilder.Add("Connect Timeout", (object) "30");
      this.TemplateConnecton.ConnectionString = connectionStringBuilder.ConnectionString;
    }

    private void CreateConnectionOleDbFactory()
    {
      DbConnectionStringBuilder connectionStringBuilder = new DbConnectionStringBuilder();
      if (!string.IsNullOrEmpty(this.ConnectionInfo.ConnectionString))
      {
        connectionStringBuilder.ConnectionString = this.ConnectionInfo.ConnectionString;
        object obj;
        if (connectionStringBuilder.TryGetValue("Data Source", out obj))
          this.ConnectionInfo.UrlOrPath = (string) obj;
        connectionStringBuilder.Clear();
      }
      this.ProviderFactory = (DbProviderFactory) OleDbFactory.Instance;
      this.TemplateConnecton = this.ProviderFactory.CreateConnection();
      connectionStringBuilder.Remove("Mode");
      connectionStringBuilder.Add("Provider", (object) "Microsoft.Jet.OLEDB.4.0");
      connectionStringBuilder.Add("User ID", (object) "");
      connectionStringBuilder.Add("Data Source", (object) this.ConnectionInfo.UrlOrPath);
      connectionStringBuilder.Add("Extended Properties", (object) "");
      connectionStringBuilder.Add("Jet OLEDB:System database", (object) "");
      connectionStringBuilder.Add("Jet OLEDB:Registry Path", (object) "");
      connectionStringBuilder.Add("Jet OLEDB:Database Password", (object) "meterdbpass");
      connectionStringBuilder.Add("Jet OLEDB:Engine Type", (object) "5");
      connectionStringBuilder.Add("Jet OLEDB:Database Locking Mode", (object) "0");
      connectionStringBuilder.Add("Jet OLEDB:Global Partial Bulk Ops", (object) "2");
      connectionStringBuilder.Add("Jet OLEDB:Global Bulk Transactions", (object) "1");
      connectionStringBuilder.Add("Jet OLEDB:New Database Password", (object) "");
      connectionStringBuilder.Add("Jet OLEDB:Create System Database", (object) "False");
      connectionStringBuilder.Add("Jet OLEDB:Encrypt Database", (object) "False");
      connectionStringBuilder.Add("Jet OLEDB:Don't Copy Locale on Compact", (object) "False");
      connectionStringBuilder.Add("Jet OLEDB:Compact Without Replica Repair", (object) "False");
      connectionStringBuilder.Add("Jet OLEDB:SFP", (object) "False");
      this.ConnectionInfo.ConnectionString = connectionStringBuilder.ConnectionString + ";Mode=ReadWrite|Share Deny None;";
      this.TemplateConnecton.ConnectionString = this.ConnectionInfo.ConnectionString;
    }

    private void CreateConnectionNpgsqlFactory()
    {
      this.ProviderFactory = (DbProviderFactory) NpgsqlFactory.Instance;
      this.TemplateConnecton = this.ProviderFactory.CreateConnection();
      DbConnectionStringBuilder connectionStringBuilder = new DbConnectionStringBuilder()
      {
        ConnectionString = this.TemplateConnecton.ConnectionString
      };
      connectionStringBuilder.Add("Server", (object) this.ConnectionInfo.UrlOrPath);
      connectionStringBuilder.Add("Database", (object) this.ConnectionInfo.DatabaseName);
      connectionStringBuilder.Add("User Id", (object) this.ConnectionInfo.UserName);
      connectionStringBuilder.Add("Password", (object) this.ConnectionInfo.Password);
      connectionStringBuilder.Add("Port", (object) "5432");
      this.ConnectionInfo.ConnectionString = connectionStringBuilder.ConnectionString;
      this.TemplateConnecton.ConnectionString = this.ConnectionInfo.ConnectionString;
    }

    public DbConnection GetNewConnection()
    {
      DbConnection connection = this.ProviderFactory.CreateConnection();
      connection.ConnectionString = this.TemplateConnecton.ConnectionString;
      if (this.ConnectionInfo.DbType == MeterDbTypes.SQLite)
        this.lastConnection = connection;
      return connection;
    }

    public string ConnectDatabase()
    {
      try
      {
        this.DatabaseLocationName = (string) null;
        StringBuilder stringBuilder = new StringBuilder();
        DbDataAdapter dataAdapter = this.GetDataAdapter("SELECT * from DatabaseIdentification", this.TemplateConnecton);
        BaseTables.DatabaseIdentificationDataTable identificationDataTable = new BaseTables.DatabaseIdentificationDataTable();
        dataAdapter.Fill((DataTable) identificationDataTable);
        if (identificationDataTable.Rows.Count <= 0)
          throw new Exception("No database identifcation.");
        this.DatabaseIdentification = new SortedList<string, string>();
        for (int index = 0; index < identificationDataTable.Rows.Count; ++index)
        {
          BaseTables.DatabaseIdentificationRow row = (BaseTables.DatabaseIdentificationRow) identificationDataTable.Rows[index];
          this.DatabaseIdentification.Add(row.InfoName, row.InfoData);
          stringBuilder.AppendLine(row.InfoName + " = " + row.InfoData);
          if (row.InfoName == "DatabaseLocationName")
            this.DatabaseLocationName = row.InfoData;
        }
        if (this.DatabaseLocationName == null)
          throw new Exception("No database location name.");
        return stringBuilder.ToString();
      }
      catch (Exception ex)
      {
        BaseDbConnection.DatabaseAccessLogger.Error("Connect database error." + Environment.NewLine + ex.Message);
        throw new Exception("Connect database error.", ex);
      }
    }

    public string GetDatabaseShortInfo()
    {
      StringBuilder stringBuilder = new StringBuilder(" on ");
      stringBuilder.Append(this.ConnectionInfo.DbType.ToString());
      if (this.ConnectionInfo.DbType == MeterDbTypes.MSSQL || this.ConnectionInfo.DbType == MeterDbTypes.NPGSQL)
      {
        if (!string.IsNullOrEmpty(this.ConnectionInfo.DatabaseName))
        {
          stringBuilder.Append("; ");
          stringBuilder.Append(this.ConnectionInfo.DatabaseName);
        }
        if (!string.IsNullOrEmpty(this.ConnectionInfo.UrlOrPath))
        {
          stringBuilder.Append("; ");
          stringBuilder.Append(this.ConnectionInfo.UrlOrPath);
        }
      }
      else if (!string.IsNullOrEmpty(this.ConnectionInfo.UrlOrPath))
      {
        stringBuilder.Append("; ");
        string str = this.ConnectionInfo.UrlOrPath;
        int num1 = 40;
        try
        {
          int num2;
          for (; str.Length > num1; str = str.Substring(num2 + 1))
          {
            num2 = str.IndexOf("\\");
            if (num2 < 0)
              break;
          }
          stringBuilder.Append(str);
        }
        catch
        {
        }
      }
      return stringBuilder.ToString();
    }

    public string GetDatabaseInfo(string LineStartText)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(LineStartText);
      stringBuilder.AppendLine("Type: ... " + this.ConnectionInfo.DbType.ToString());
      if (!string.IsNullOrEmpty(this.ConnectionInfo.DatabaseName))
      {
        stringBuilder.Append(LineStartText);
        stringBuilder.AppendLine("Name: ... " + this.ConnectionInfo.DatabaseName);
      }
      stringBuilder.Append(LineStartText);
      stringBuilder.AppendLine("Location: " + this.ConnectionInfo.UrlOrPath);
      return stringBuilder.ToString();
    }

    public string GetDatabaseFullInfo(string LineStartText)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.GetDatabaseInfo(LineStartText));
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("*** Database settings ***");
      foreach (KeyValuePair<string, string> keyValuePair in this.DatabaseIdentification)
      {
        stringBuilder.Append(LineStartText);
        stringBuilder.Append(keyValuePair.Key);
        stringBuilder.Append("=");
        stringBuilder.AppendLine(keyValuePair.Value);
      }
      return stringBuilder.ToString();
    }

    public string[] GetAvailableLanguages()
    {
      try
      {
        using (DbConnection newConnection = this.GetNewConnection())
        {
          DbDataAdapter dataAdapter = this.GetDataAdapter("SELECT DISTINCT LanguageCode FROM OnlineTranslations", newConnection);
          DataTable dataTable = new DataTable();
          dataAdapter.Fill(dataTable);
          if (dataTable.Rows.Count > 0)
          {
            string[] availableLanguages = new string[dataTable.Rows.Count];
            for (int index = 0; index < dataTable.Rows.Count; ++index)
              availableLanguages[index] = dataTable.Rows[index][0].ToString();
            return availableLanguages;
          }
        }
      }
      catch
      {
      }
      return new string[1]{ "en" };
    }

    public bool IsProductionDatabase()
    {
      return this.DatabaseIdentification.ContainsKey(nameof (IsProductionDatabase)) && this.DatabaseIdentification[nameof (IsProductionDatabase)] == "True";
    }

    public void ExceptionIfNoSvnDatabase()
    {
      if (this.ConnectionInfo.DbType != MeterDbTypes.Access)
        throw new Exception("This function is only allowed for SVN data base.");
    }

    public DbDataAdapter GetDataAdapter(string selectSql, DbConnection connection)
    {
      DbCommand command = this.ProviderFactory.CreateCommand();
      command.CommandText = selectSql;
      command.Connection = connection;
      command.CommandTimeout = 120;
      DbDataAdapter dataAdapter = this.ProviderFactory.CreateDataAdapter();
      dataAdapter.SelectCommand = command;
      return dataAdapter;
    }

    public DbDataAdapter GetDataAdapter(
      string selectSql,
      DbConnection connection,
      out DbCommandBuilder commandBuilder)
    {
      DbCommand command = this.ProviderFactory.CreateCommand();
      command.CommandText = selectSql;
      command.Connection = connection;
      DbDataAdapter dataAdapter = this.ProviderFactory.CreateDataAdapter();
      dataAdapter.SelectCommand = command;
      commandBuilder = this.ProviderFactory.CreateCommandBuilder();
      commandBuilder.QuotePrefix = "[";
      commandBuilder.QuoteSuffix = "]";
      commandBuilder.DataAdapter = dataAdapter;
      return dataAdapter;
    }

    public DbDataAdapter GetDataAdapter(
      string selectSql,
      DbConnection connection,
      DbTransaction transaction,
      out DbCommandBuilder commandBuilder)
    {
      if (BaseDbConnection.DatabaseAccessLogger.IsTraceEnabled)
        BaseDbConnection.DatabaseAccessLogger.Trace("GetDataAdapter transaction");
      DbCommand command = this.ProviderFactory.CreateCommand();
      command.CommandText = selectSql;
      command.Connection = connection;
      command.Transaction = transaction;
      DbDataAdapter dataAdapter = this.ProviderFactory.CreateDataAdapter();
      dataAdapter.SelectCommand = command;
      commandBuilder = this.ProviderFactory.CreateCommandBuilder();
      commandBuilder.QuotePrefix = "[";
      commandBuilder.QuoteSuffix = "]";
      commandBuilder.DataAdapter = dataAdapter;
      return dataAdapter;
    }

    public DbDataAdapter GetDataAdapter(
      string selectSql,
      DbConnection connection,
      DbTransaction transaction)
    {
      return this.GetDataAdapter(selectSql, connection, transaction, out DbCommandBuilder _);
    }

    [Obsolete]
    public DbDataAdapter GetDataAdapter(DbCommand selectCommand)
    {
      DbDataAdapter dataAdapter = this.ProviderFactory.CreateDataAdapter();
      dataAdapter.SelectCommand = selectCommand;
      this.ProviderFactory.CreateCommand();
      return dataAdapter;
    }

    private DbCommand CreateCommand() => this.ProviderFactory.CreateCommand();

    public int GetNewId(string TableName) => this.GetNewIds(TableName, 1).GetNextID();

    public IdContainer GetNewIds(string TableName, int numberOfIds)
    {
      if (this.DatabaseLocationName == null)
        throw new ArgumentNullException("DatabaseLocationName");
      string selectSql = "SELECT * FROM ZRGlobalID WHERE ZRTableName = '" + TableName + "' AND DatabaseLocationName = '" + this.DatabaseLocationName + "'";
      int num1 = 0;
      IdContainer newIds;
      while (true)
      {
        try
        {
          BaseTables.ZRGlobalIDDataTable globalIdDataTable = new BaseTables.ZRGlobalIDDataTable();
          DbCommandBuilder commandBuilder;
          int zrNextNr;
          string zrFieldName;
          if (this.ConnectionInfo.DbType == MeterDbTypes.SQLite && this.lastConnection != null)
          {
            try
            {
              DbConnection lastConnection = this.lastConnection;
              DbDataAdapter dataAdapter = this.GetDataAdapter(selectSql, lastConnection, out commandBuilder);
              dataAdapter.Fill((DataTable) globalIdDataTable);
              if (globalIdDataTable.Count != 1)
              {
                num1 = 1000;
                throw new Exception("Id row not found");
              }
              zrNextNr = globalIdDataTable[0].ZRNextNr;
              zrFieldName = globalIdDataTable[0].ZRFieldName;
              int num2 = zrNextNr + numberOfIds - 1;
              if (num2 > globalIdDataTable[0].ZRLastNr)
              {
                num1 = 1000;
                throw new Exception("Out of ID range for table: " + TableName);
              }
              globalIdDataTable[0].ZRNextNr = num2 + 1;
              dataAdapter.Update((DataTable) globalIdDataTable);
            }
            catch (ObjectDisposedException ex)
            {
              num1 = 1000;
              throw new Exception("SQLite connection disposed.", (Exception) ex);
            }
          }
          else
          {
            using (DbConnection newConnection = this.GetNewConnection())
            {
              DbDataAdapter dataAdapter = this.GetDataAdapter(selectSql, newConnection, out commandBuilder);
              dataAdapter.Fill((DataTable) globalIdDataTable);
              if (globalIdDataTable.Count != 1)
              {
                num1 = 1000;
                throw new Exception("No data found! SQL: " + selectSql);
              }
              zrNextNr = globalIdDataTable[0].ZRNextNr;
              zrFieldName = globalIdDataTable[0].ZRFieldName;
              int num3 = zrNextNr + numberOfIds - 1;
              if (num3 > globalIdDataTable[0].ZRLastNr)
              {
                num1 = 1000;
                throw new Exception("Out of ID range for table: " + TableName);
              }
              globalIdDataTable[0].ZRNextNr = num3 + 1;
              dataAdapter.Update((DataTable) globalIdDataTable);
            }
          }
          newIds = new IdContainer(zrNextNr, numberOfIds, TableName, zrFieldName);
          if (BaseDbConnection.DatabaseIdGenLogger.IsTraceEnabled)
          {
            BaseDbConnection.DatabaseIdGenLogger.Trace("TableName:" + TableName + " ID:" + zrNextNr.ToString() + "+" + numberOfIds.ToString());
            break;
          }
          break;
        }
        catch (Exception ex)
        {
          if (num1 >= 10)
          {
            BaseDbConnection.DatabaseIdGenLogger.Error(nameof (TableName));
            throw new Exception("GetNewId error! " + ex.Message, ex);
          }
          Thread.Sleep(50 + new Random(DateTime.Now.Millisecond).Next(250));
          BaseDbConnection.DatabaseIdGenLogger.Warn("TableName: " + TableName + " tryCount:" + num1.ToString());
        }
        ++num1;
      }
      return newIds;
    }

    public void InsertNewID(DataRow theRow)
    {
      IdContainer newIds = this.GetNewIds(theRow.Table.TableName, 1);
      theRow[newIds.idColumnName] = (object) newIds.GetNextID();
    }

    public bool IDisInEntireRange(string TableName, int TheID)
    {
      if (this.DatabaseLocationName == null)
        throw new ArgumentNullException("DatabaseLocationName");
      BaseTables.ZRGlobalIDDataTable globalIdDataTable = new BaseTables.ZRGlobalIDDataTable();
      string selectSql = "SELECT * FROM ZRGlobalID WHERE ZRTableName = '" + TableName + "' AND DatabaseLocationName = '" + this.DatabaseLocationName + "'";
      DbCommandBuilder commandBuilder;
      if (this.ConnectionInfo.DbType == MeterDbTypes.SQLite && this.lastConnection != null)
      {
        DbConnection lastConnection = this.lastConnection;
        this.GetDataAdapter(selectSql, lastConnection, out commandBuilder).Fill((DataTable) globalIdDataTable);
      }
      else
      {
        using (DbConnection newConnection = this.GetNewConnection())
          this.GetDataAdapter(selectSql, newConnection, out commandBuilder).Fill((DataTable) globalIdDataTable);
      }
      if (globalIdDataTable.Rows.Count != 1)
        throw new Exception("Id row not found");
      return TheID >= globalIdDataTable[0].ZRFirstNr && TheID <= globalIdDataTable[0].ZRLastNr;
    }

    public bool IDisInActualRange(string TableName, int TheID)
    {
      if (this.DatabaseLocationName == null)
        throw new ArgumentNullException("DatabaseLocationName");
      BaseTables.ZRGlobalIDDataTable globalIdDataTable = new BaseTables.ZRGlobalIDDataTable();
      string selectSql = "SELECT * FROM ZRGlobalID WHERE ZRTableName = '" + TableName + "' AND DatabaseLocationName = '" + this.DatabaseLocationName + "'";
      DbCommandBuilder commandBuilder;
      if (this.ConnectionInfo.DbType == MeterDbTypes.SQLite && this.lastConnection != null)
      {
        DbConnection lastConnection = this.lastConnection;
        this.GetDataAdapter(selectSql, lastConnection, out commandBuilder).Fill((DataTable) globalIdDataTable);
      }
      else
      {
        using (DbConnection newConnection = this.GetNewConnection())
          this.GetDataAdapter(selectSql, newConnection, out commandBuilder).Fill((DataTable) globalIdDataTable);
      }
      if (globalIdDataTable.Rows.Count != 1)
        throw new Exception("Id row not found");
      return TheID >= globalIdDataTable[0].ZRFirstNr && TheID < globalIdDataTable[0].ZRNextNr;
    }

    public void CheckMeterIdNotMovedToProductionDatabase(int MeterID, DbCommand anyCommand)
    {
      try
      {
        if (this.ConnectionInfo.DbType != MeterDbTypes.MSSQL || !this.IsProductionDatabase())
          return;
        lock (this)
        {
          if (!this.MeterIdRangeFirstNumber.HasValue)
          {
            string selectSql = "SELECT * FROM ZRGlobalID WHERE ZRTableName = 'Meter' AND DatabaseLocationName = '" + this.DatabaseLocationName + "'";
            BaseTables.ZRGlobalIDDataTable globalIdDataTable = new BaseTables.ZRGlobalIDDataTable();
            this.GetDataAdapter(selectSql, anyCommand.Connection, anyCommand.Transaction).Fill((DataTable) globalIdDataTable);
            this.MeterIdRangeFirstNumber = globalIdDataTable.Count == 1 ? new int?(globalIdDataTable[0].ZRFirstNr) : throw new Exception("MeterID range not found");
            this.MeterIdRangeLastNumber = globalIdDataTable[0].ZRLastNr;
          }
          int num = MeterID;
          int? rangeFirstNumber = this.MeterIdRangeFirstNumber;
          int valueOrDefault = rangeFirstNumber.GetValueOrDefault();
          if (num < valueOrDefault & rangeFirstNumber.HasValue || MeterID > this.MeterIdRangeLastNumber)
            throw new Exception("*** This device is a developement sample! ***" + Environment.NewLine + "The device identification is not usable for production." + Environment.NewLine + "(The MeterID is not generated from production database!)");
        }
      }
      catch (Exception ex)
      {
        throw new Exception("MeterID range check error", ex);
      }
    }
  }
}
