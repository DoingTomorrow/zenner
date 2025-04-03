// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.OldDbConfigInfo
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using GmmDbLib;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public class OldDbConfigInfo
  {
    public bool NoDataFound = false;
    public string DataBaseTypeName = "";
    public MeterDbTypes dbType;
    public string dbFileName;
    public string dbPassword;
    public string dbUser;
    public string dbDataSource;
    public string dbServer;
    public string dbPort;
    public string dbProviderString;
    private string dbNamescace;

    public OldDbConfigInfo(string DatabaseNamespaceName, GMMConfig opConfig)
    {
      this.dbNamescace = DatabaseNamespaceName;
      OldDbConfigInfo.DbConfigNames dbConfigNames;
      if (opConfig != null)
      {
        GMMConfig gmmConfig = opConfig;
        string strNamespace = DatabaseNamespaceName;
        dbConfigNames = OldDbConfigInfo.DbConfigNames.GMM_DBType;
        string strVariable = dbConfigNames.ToString();
        this.DataBaseTypeName = gmmConfig.GetValue(strNamespace, strVariable);
      }
      if (this.DataBaseTypeName == "")
      {
        this.DataBaseTypeName = "ACCESS";
        this.NoDataFound = true;
      }
      if (opConfig != null)
      {
        GMMConfig gmmConfig = opConfig;
        string strNamespace = DatabaseNamespaceName;
        dbConfigNames = OldDbConfigInfo.DbConfigNames.GMM_DateiName;
        string strVariable = dbConfigNames.ToString();
        this.dbFileName = gmmConfig.GetValue(strNamespace, strVariable);
      }
      if (this.dbFileName == "" && this.DataBaseTypeName == "ACCESS")
      {
        string path1 = Path.Combine(SystemValues.DatabasePath, "MeterDB.mdb");
        if (File.Exists(path1))
        {
          this.dbFileName = path1;
        }
        else
        {
          string path2 = Application.StartupPath + "\\..\\..\\..\\Database\\MeterDB.mdb";
          if (File.Exists(path2))
          {
            this.dbFileName = Path.GetFullPath(path2);
          }
          else
          {
            string path3 = Application.StartupPath + "\\..\\..\\..\\Database\\MeterDB_New.mdb";
            if (File.Exists(path3))
              this.dbFileName = Path.GetFullPath(path3);
          }
        }
      }
      if (opConfig != null)
      {
        GMMConfig gmmConfig1 = opConfig;
        string strNamespace1 = DatabaseNamespaceName;
        dbConfigNames = OldDbConfigInfo.DbConfigNames.GMM_Password;
        string strVariable1 = dbConfigNames.ToString();
        this.dbPassword = this.descrable(gmmConfig1.GetValue(strNamespace1, strVariable1));
        GMMConfig gmmConfig2 = opConfig;
        string strNamespace2 = DatabaseNamespaceName;
        dbConfigNames = OldDbConfigInfo.DbConfigNames.GMM_UserID;
        string strVariable2 = dbConfigNames.ToString();
        this.dbUser = gmmConfig2.GetValue(strNamespace2, strVariable2);
        GMMConfig gmmConfig3 = opConfig;
        string strNamespace3 = DatabaseNamespaceName;
        dbConfigNames = OldDbConfigInfo.DbConfigNames.GMM_DataSource;
        string strVariable3 = dbConfigNames.ToString();
        this.dbDataSource = gmmConfig3.GetValue(strNamespace3, strVariable3);
        GMMConfig gmmConfig4 = opConfig;
        string strNamespace4 = DatabaseNamespaceName;
        dbConfigNames = OldDbConfigInfo.DbConfigNames.GMM_DBServer;
        string strVariable4 = dbConfigNames.ToString();
        this.dbServer = gmmConfig4.GetValue(strNamespace4, strVariable4);
        GMMConfig gmmConfig5 = opConfig;
        string strNamespace5 = DatabaseNamespaceName;
        dbConfigNames = OldDbConfigInfo.DbConfigNames.GMM_DBPort;
        string strVariable5 = dbConfigNames.ToString();
        this.dbPort = gmmConfig5.GetValue(strNamespace5, strVariable5);
        GMMConfig gmmConfig6 = opConfig;
        string strNamespace6 = DatabaseNamespaceName;
        dbConfigNames = OldDbConfigInfo.DbConfigNames.GMM_DataBase;
        string strVariable6 = dbConfigNames.ToString();
        this.dbProviderString = gmmConfig6.GetValue(strNamespace6, strVariable6);
      }
      this.dbType = Datenbankverbindung.DatabaseTypeFromName(this.DataBaseTypeName);
    }

    public DbConnectionInfo GetNewConfigInfo()
    {
      DbConnectionInfo newConfigInfo = new DbConnectionInfo();
      newConfigInfo.DatabaseName = this.dbDataSource;
      int num = this.dbType == MeterDbTypes.Access || this.dbType == MeterDbTypes.LocalDB || this.dbType == MeterDbTypes.SQLite ? 1 : (this.dbType == MeterDbTypes.Microsoft_SQL_Compact ? 1 : 0);
      newConfigInfo.UrlOrPath = num == 0 ? this.dbServer : this.dbFileName;
      newConfigInfo.DbInstance = !(this.dbNamescace == "MainDataBase") ? DbInstances.Secundary : DbInstances.Primary;
      newConfigInfo.DbType = this.dbType;
      newConfigInfo.Password = this.dbPassword;
      newConfigInfo.UserName = this.dbUser;
      return newConfigInfo;
    }

    private string descrable(string inpassword)
    {
      string str = "";
      for (int index = 0; index < inpassword.Length; ++index)
      {
        char ch = (char) ((uint) inpassword[index] - 12U);
        str += ch.ToString();
      }
      return str;
    }

    private enum DbConfigNames
    {
      GMM_DBType,
      GMM_DateiName,
      GMM_Password,
      GMM_UserID,
      GMM_DataSource,
      GMM_DBServer,
      GMM_DBPort,
      GMM_DataBase,
    }
  }
}
