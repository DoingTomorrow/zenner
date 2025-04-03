// Decompiled with JetBrains decompiler
// Type: GmmDbLib.DbBasis
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

#nullable disable
namespace GmmDbLib
{
  public class DbBasis
  {
    public static DbBasis PrimaryDB;
    public static DbBasis SecondaryDB;
    public BaseDbConnection BaseDbConnection;

    public static DbBasis getDbObject(DbConnectionInfo connectionInfo)
    {
      BaseDbConnection baseDbConnection = new BaseDbConnection(connectionInfo);
      string connectionString = baseDbConnection.TemplateConnecton.ConnectionString;
      DbBasis dbObject;
      switch (connectionInfo.DbType)
      {
        case MeterDbTypes.Access:
          dbObject = (DbBasis) new AccessDB(connectionString);
          break;
        case MeterDbTypes.NPGSQL:
          dbObject = (DbBasis) new PostgreSQL(connectionString);
          break;
        case MeterDbTypes.SQLite:
          dbObject = (DbBasis) new SQLiteDB(connectionString);
          break;
        case MeterDbTypes.DBISAM:
          dbObject = (DbBasis) new DBISAM(connectionString);
          break;
        case MeterDbTypes.MSSQL:
          dbObject = (DbBasis) new MSSQLDB(connectionString);
          break;
        default:
          dbObject = (DbBasis) new MSSQLDB(connectionString);
          break;
      }
      dbObject.BaseDbConnection = baseDbConnection;
      return dbObject;
    }

    public static DbBasis getDbObject(MeterDbTypes DbType, string connectionString)
    {
      BaseDbConnection baseDbConnection = new BaseDbConnection(DbType, connectionString);
      string connectionString1 = baseDbConnection.TemplateConnecton.ConnectionString;
      DbBasis dbObject;
      switch (DbType)
      {
        case MeterDbTypes.Access:
          dbObject = (DbBasis) new AccessDB(connectionString1);
          break;
        case MeterDbTypes.NPGSQL:
          dbObject = (DbBasis) new PostgreSQL(connectionString1);
          break;
        case MeterDbTypes.SQLite:
          dbObject = (DbBasis) new SQLiteDB(connectionString1);
          break;
        case MeterDbTypes.DBISAM:
          dbObject = (DbBasis) new DBISAM(connectionString1);
          break;
        case MeterDbTypes.MSSQL:
          dbObject = (DbBasis) new MSSQLDB(connectionString1);
          break;
        default:
          throw new Exception("Database type not available");
      }
      dbObject.BaseDbConnection = baseDbConnection;
      return dbObject;
    }

    public DbBasis(string connectionString) => this.ConnectionString = connectionString;

    public DbBasis(BaseDbConnection newBaseDbConnection)
    {
      this.BaseDbConnection = newBaseDbConnection;
      this.ConnectionString = this.BaseDbConnection.TemplateConnecton.ConnectionString;
    }

    public string ConnectionString { get; private set; }

    [Obsolete]
    public IDbConnection GetDbConnection()
    {
      return (IDbConnection) this.BaseDbConnection.GetNewConnection();
    }

    [Obsolete]
    public virtual GmmDbLib.ZRDataAdapter ZRDataAdapter(string SqlCommand, IDbConnection Connection)
    {
      return new GmmDbLib.ZRDataAdapter(this.BaseDbConnection, this.BaseDbConnection.GetDataAdapter(SqlCommand, (DbConnection) Connection));
    }

    [Obsolete]
    public virtual GmmDbLib.ZRDataAdapter ZRDataAdapter(IDbCommand DbCommand)
    {
      return new GmmDbLib.ZRDataAdapter(this.BaseDbConnection, this.BaseDbConnection.GetDataAdapter((System.Data.Common.DbCommand) DbCommand));
    }

    [Obsolete]
    public virtual IDbCommand DbCommand(IDbConnection Connection) => Connection.CreateCommand();

    public virtual IDataReader DataReader() => throw new NotImplementedException();

    public virtual bool CreateTableStructure(List<string> Tables)
    {
      throw new NotImplementedException();
    }

    public virtual bool OptimizeTable(string TableName) => throw new NotImplementedException();

    public virtual long GetDatabaseSize() => throw new NotImplementedException();
  }
}
