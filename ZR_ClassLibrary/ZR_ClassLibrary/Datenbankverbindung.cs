// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.Datenbankverbindung
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using GmmDbLib;
using System;
using System.Data.Common;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public class Datenbankverbindung
  {
    public static MeterDBAccess MainDBAccess;
    public static MeterDBAccess SecDBAccess;
    internal const string PasswordReplace = "***PASSWORD***";
    public const string MainDatabaseNamespaceName = "MainDataBase";
    public const string SecDatabaseNamespaceName = "SecDataBase";
    private GMMConfig opConfig;
    internal MeterDbTypes DataBaseTypePrivate;
    internal string DataBaseTypeNamePrivate;
    internal bool IsDeveloper;
    internal string sDataBaseKey;
    public bool ConnectionOk = false;
    public MeterDbTypes DataBaseType;
    public string DataBaseTypeName;
    public string connectString;
    public DbConnectionInfo ConnectionInfo;
    private string ErrorMsg;
    internal string dbFileName;
    internal string dbPassword;
    internal string dbUser;
    internal string dbDataSource;
    internal string dbServer;
    internal string dbPort;
    internal string dbProviderString;

    public static void Dispose()
    {
      Datenbankverbindung.MainDBAccess = (MeterDBAccess) null;
      Datenbankverbindung.SecDBAccess = (MeterDBAccess) null;
    }

    public string SelectedConnection { get; set; }

    public Datenbankverbindung(string titel)
    {
      this.initall();
      this.setDatabaseTypeFromName();
    }

    public Datenbankverbindung(string dataBase, GMMConfig MyConfig)
    {
      this.opConfig = MyConfig;
      this.sDataBaseKey = dataBase;
      this.initall();
    }

    public void ChangeToDeveloperMode() => this.IsDeveloper = true;

    internal void initall()
    {
      try
      {
        this.ConnectionInfo = new DbConnectionInfo();
        string strInhalt;
        if (this.sDataBaseKey == "MainDataBase")
        {
          strInhalt = this.opConfig.GetValue("Startup", "DbConfigPrimary");
          if (string.IsNullOrEmpty(strInhalt))
          {
            string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Database", "MeterDB_New.mdb");
            if (File.Exists(path))
            {
              strInhalt = "DbType=Access;DbInstance=Primary;UrlOrPath=" + path + ";DatabaseName=MeterDB_New";
              this.opConfig.SetOrUpdateValue("Startup", "DbConfigPrimary", strInhalt);
            }
          }
        }
        else
        {
          strInhalt = this.opConfig.GetValue("Startup", "DbConfigSecundary");
          this.ConnectionInfo.DbInstance = DbInstances.Secundary;
        }
        bool flag = "DbType=Access;DbInstance=Primary;UrlOrPath=" == strInhalt;
        if (!string.IsNullOrEmpty(strInhalt) && !flag)
        {
          this.ConnectionInfo.SetupString = strInhalt;
        }
        else
        {
          OldDbConfigInfo oldDbConfigInfo = new OldDbConfigInfo(this.sDataBaseKey, this.opConfig);
          this.DataBaseTypeNamePrivate = oldDbConfigInfo.DataBaseTypeName;
          this.setDatabaseTypeFromName();
          this.dbFileName = oldDbConfigInfo.dbFileName;
          this.dbPassword = oldDbConfigInfo.dbPassword;
          this.dbUser = oldDbConfigInfo.dbUser;
          this.dbDataSource = oldDbConfigInfo.dbDataSource;
          this.dbServer = oldDbConfigInfo.dbServer;
          this.dbPort = oldDbConfigInfo.dbPort;
          this.dbProviderString = oldDbConfigInfo.dbProviderString;
          this.ConnectionInfo.DbType = oldDbConfigInfo.dbType;
          this.ConnectionInfo.ConnectionString = this.connectString;
          if (!string.IsNullOrEmpty(this.dbPassword))
            this.ConnectionInfo.Password = this.dbPassword;
          this.AddConnectionInfoFromConnectionString(this.ConnectionInfo, oldDbConfigInfo.dbProviderString);
          this.saveDBInfo();
        }
      }
      catch
      {
        int num = (int) MessageBox.Show("No info for the database connection found\r\nPlease go to Settings/Primary database and Settings/Secondary database!");
        return;
      }
      this.DataBaseType = this.DataBaseTypePrivate;
      this.DataBaseTypeName = this.DataBaseTypeNamePrivate;
    }

    private void AddConnectionInfoFromConnectionString(
      DbConnectionInfo connectionInfo,
      string connectionString)
    {
      if (string.IsNullOrEmpty(connectionString))
        return;
      try
      {
        DbConnectionStringBuilder connectionStringBuilder = new DbConnectionStringBuilder();
        connectionStringBuilder.ConnectionString = connectionString;
        switch (connectionInfo.DbType)
        {
          case MeterDbTypes.Access:
            object obj1;
            if (connectionStringBuilder.TryGetValue("Data Source", out obj1))
            {
              connectionInfo.UrlOrPath = (string) obj1;
              goto case MeterDbTypes.NPGSQL;
            }
            else
              goto case MeterDbTypes.NPGSQL;
          case MeterDbTypes.NPGSQL:
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
          case MeterDbTypes.SQLite:
            object obj2;
            if (connectionStringBuilder.TryGetValue("Data Source", out obj2))
            {
              connectionInfo.UrlOrPath = (string) obj2;
              goto case MeterDbTypes.NPGSQL;
            }
            else
              goto case MeterDbTypes.NPGSQL;
          case MeterDbTypes.MSSQL:
            object obj3;
            if (connectionStringBuilder.TryGetValue("Data Source", out obj3))
              connectionInfo.UrlOrPath = (string) obj3;
            if (connectionStringBuilder.TryGetValue("Database", out obj3))
              connectionInfo.DatabaseName = (string) obj3;
            if (connectionStringBuilder.TryGetValue("User Id", out obj3))
            {
              connectionInfo.UserName = (string) obj3;
              goto case MeterDbTypes.NPGSQL;
            }
            else
              goto case MeterDbTypes.NPGSQL;
          case MeterDbTypes.LocalDB:
            object obj4;
            if (connectionStringBuilder.TryGetValue("AttachDbFilename", out obj4))
            {
              connectionInfo.UrlOrPath = (string) obj4;
              goto case MeterDbTypes.NPGSQL;
            }
            else
              goto case MeterDbTypes.NPGSQL;
          case MeterDbTypes.Microsoft_SQL_Compact:
            object obj5;
            if (connectionStringBuilder.TryGetValue("Data Source", out obj5))
            {
              connectionInfo.UrlOrPath = (string) obj5;
              goto case MeterDbTypes.NPGSQL;
            }
            else
              goto case MeterDbTypes.NPGSQL;
          default:
            throw new Exception("Database type not available");
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Load database connection error.", ex);
      }
    }

    private void addErrorText(string inErrorMsg)
    {
      this.ErrorMsg = this.ErrorMsg + inErrorMsg + "\r\n";
    }

    public string getErrorText()
    {
      string errorMsg = this.ErrorMsg;
      this.ErrorMsg = "";
      return errorMsg;
    }

    public static MeterDbTypes DatabaseTypeFromName(string DataBaseTypeName)
    {
      switch (DataBaseTypeName)
      {
        case "ACCESS":
          return MeterDbTypes.Access;
        case "NPGSQL":
          return MeterDbTypes.NPGSQL;
        case "SQLITE":
          return MeterDbTypes.SQLite;
        case "DBISAM":
          return MeterDbTypes.DBISAM;
        case "MSSQL":
          return MeterDbTypes.MSSQL;
        default:
          return MeterDbTypes.Undefined;
      }
    }

    internal void setDatabaseTypeFromName()
    {
      this.DataBaseTypePrivate = Datenbankverbindung.DatabaseTypeFromName(this.DataBaseTypeNamePrivate);
    }

    public static string scrable(string inpassword)
    {
      string str = "";
      for (int index = 0; index < inpassword.Length; ++index)
      {
        char ch = (char) ((uint) inpassword[index] + 12U);
        str += ch.ToString();
      }
      return str;
    }

    internal void saveDBInfo()
    {
      if (this.ConnectionInfo.DbInstance == DbInstances.Primary)
        this.opConfig.SetOrUpdateValue("Startup", "DbConfigPrimary", this.ConnectionInfo.SetupString);
      else
        this.opConfig.SetOrUpdateValue("Startup", "DbConfigSecundary", this.ConnectionInfo.SetupString);
      this.opConfig.WriteConfigFile();
    }
  }
}
