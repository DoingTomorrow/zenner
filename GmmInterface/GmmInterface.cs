// Decompiled with JetBrains decompiler
// Type: ZENNER.GmmInterface
// Assembly: GmmInterface, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 25F1E48F-52B7-4A4F-B66A-62C91CCF5C52
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmInterface.dll

using GmmDbLib;
using NLog;
using PlugInLib;
using ReadoutConfiguration;
using StartupLib;
using System;
using System.Data;
using System.IO;
using ZR_ClassLibrary;

#nullable disable
namespace ZENNER
{
  public sealed class GmmInterface
  {
    private static Logger logger = LogManager.GetLogger(nameof (GmmInterface));
    private static JobManager jobManager;
    private static ScannerManager scannerManager;
    private static ConfiguratorManager configuratorManager;
    private static HandlerManager handlerManager;
    public static Devices.DeviceManager Devices = new Devices.DeviceManager();

    private GmmInterface()
    {
    }

    public static Version Version => Util.GMM_Version;

    public static string HardwareKey => HardwareKeyGenerator.GetHardwareUniqueKey();

    public static DbBasis Database => StartupManager.Database;

    public static LicenseInfo License => StartupManager.License;

    public static JobManager JobManager
    {
      get
      {
        if (GmmInterface.jobManager == null)
        {
          GmmInterface.logger.Debug("Initialize JobManager");
          GmmInterface.jobManager = new JobManager();
        }
        return GmmInterface.jobManager;
      }
    }

    public static MeterReaderManager Reader => GmmInterface.JobManager.Reader;

    public static MeterReceiverManager Receiver => GmmInterface.JobManager.Receiver;

    public static ReadoutConfiguration.DeviceManager DeviceManager
    {
      get => ReadoutConfigFunctions.Manager;
    }

    public static ScannerManager ScannerManager
    {
      get
      {
        if (GmmInterface.scannerManager == null)
        {
          GmmInterface.logger.Debug("Initialize ScannerManager");
          GmmInterface.scannerManager = new ScannerManager();
        }
        return GmmInterface.scannerManager;
      }
    }

    public static ConfiguratorManager ConfiguratorManager
    {
      get
      {
        if (GmmInterface.configuratorManager == null)
        {
          GmmInterface.logger.Debug("Initialize ConfiguratorManager");
          GmmInterface.configuratorManager = new ConfiguratorManager();
        }
        return GmmInterface.configuratorManager;
      }
    }

    public static HandlerManager HandlerManager
    {
      get
      {
        if (GmmInterface.handlerManager == null)
        {
          GmmInterface.logger.Debug("Initialize HandlerManager");
          GmmInterface.handlerManager = new HandlerManager();
        }
        return GmmInterface.handlerManager;
      }
    }

    public static void Initialize(string licenseFileName, DatabaseConnectionSettings settings)
    {
      if (string.IsNullOrEmpty(licenseFileName))
        throw new ArgumentNullException(nameof (licenseFileName), "The name of license file can not be empty!");
      if (settings == null || string.IsNullOrEmpty(settings.DataSource))
        throw new ArgumentNullException(nameof (settings), "Invalid settings for the database!");
      switch (settings.Type)
      {
        case MeterDbTypes.Access:
        case MeterDbTypes.SQLite:
          GmmInterface.Initialize(licenseFileName, settings.Type, settings.DataSource);
          break;
        case MeterDbTypes.MSSQL:
          GmmInterface.Initialize(licenseFileName, settings.Type, settings.DataSource, settings.Password, settings.User, settings.DatabaseName);
          break;
        default:
          throw new NotImplementedException(settings.Type.ToString());
      }
    }

    public static void Initialize(string licenseFileName, MeterDbTypes type, string dataSource)
    {
      if (!string.IsNullOrEmpty(licenseFileName))
        GmmInterface.logger.Debug("Initialize GMM driver. License: {0}", licenseFileName);
      else
        GmmInterface.logger.Debug("Initialize GMM driver without license.");
      if (!string.IsNullOrEmpty(dataSource))
        GmmInterface.logger.Debug<MeterDbTypes, string>("DB: {0}, Source: {1}", type, dataSource);
      else
        GmmInterface.logger.Debug<MeterDbTypes>("DB: {0}", type);
      DbBasis database = GmmInterface.GetDatabase(type, dataSource);
      StartupManager.Initialize(licenseFileName, database);
    }

    public static void Initialize(
      string licenseFileName,
      MeterDbTypes type,
      string dataSource,
      string password)
    {
      GmmInterface.logger.Debug("Initialize GMM driver");
      if (!string.IsNullOrEmpty(licenseFileName))
        GmmInterface.logger.Debug("License: {0}", licenseFileName);
      GmmInterface.logger.Debug<MeterDbTypes>("DB type: {0}", type);
      if (!string.IsNullOrEmpty(dataSource))
        GmmInterface.logger.Debug("Data source: {0}", dataSource);
      if (!string.IsNullOrEmpty(password))
        GmmInterface.logger.Debug("Password: True");
      DbBasis database = GmmInterface.GetDatabase(type, dataSource, password);
      StartupManager.Initialize(licenseFileName, database);
    }

    public static void Initialize(
      string licenseFileName,
      MeterDbTypes type,
      string dataSource,
      string password,
      string user,
      string databaseName)
    {
      GmmInterface.logger.Debug("Initialize GMM driver");
      if (!string.IsNullOrEmpty(licenseFileName))
        GmmInterface.logger.Debug("License: {0}", licenseFileName);
      GmmInterface.logger.Debug<MeterDbTypes>("DB type: {0}", type);
      if (!string.IsNullOrEmpty(dataSource))
        GmmInterface.logger.Debug("Data source: {0}", dataSource);
      if (!string.IsNullOrEmpty(password))
        GmmInterface.logger.Debug("Password: True");
      if (!string.IsNullOrEmpty(user))
        GmmInterface.logger.Debug("User: {0}", user);
      if (!string.IsNullOrEmpty(databaseName))
        GmmInterface.logger.Debug("Database name: {0}", databaseName);
      DbBasis database = GmmInterface.GetDatabase(type, dataSource, password, user, databaseName);
      StartupManager.Initialize(licenseFileName, database);
    }

    private static DbBasis GetDatabase(MeterDbTypes type, string dataSource)
    {
      return GmmInterface.GetDatabase(type, dataSource, string.Empty, string.Empty, string.Empty);
    }

    private static DbBasis GetDatabase(MeterDbTypes type, string dataSource, string password)
    {
      return GmmInterface.GetDatabase(type, dataSource, password, string.Empty, string.Empty);
    }

    private static DbBasis GetDatabase(
      MeterDbTypes type,
      string dataSource,
      string password,
      string user,
      string databaseName)
    {
      if (string.IsNullOrEmpty(dataSource))
        throw new ArgumentNullException(nameof (dataSource), "Can not create the database object!");
      if (string.IsNullOrEmpty(password))
        password = "meterdbpass";
      DbBasis dbObject;
      switch (type)
      {
        case MeterDbTypes.Access:
          string connectionString1 = File.Exists(dataSource) ? string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Mode=ReadWrite|Share Deny None;Extended Properties=;Jet OLEDB:System database=;Jet OLEDB:Registry Path=;Jet OLEDB:Database Password={1};Jet OLEDB:Engine Type=5;Jet OLEDB:Database Locking Mode=1;Jet OLEDB:Global Partial Bulk Ops=2;Jet OLEDB:Global Bulk Transactions=1;Jet OLEDB:New Database Password=;Jet OLEDB:Create System Database=False;Jet OLEDB:Encrypt Database=False;Jet OLEDB:Don't Copy Locale on Compact=False;Jet OLEDB:Compact Without Replica Repair=False;Jet OLEDB:SFP=False;", (object) dataSource, (object) password) : throw new FileNotFoundException("Can not create the database object!", dataSource);
          dbObject = DbBasis.getDbObject(type, connectionString1);
          break;
        case MeterDbTypes.SQLite:
          string connectionString2 = File.Exists(dataSource) ? string.Format("Data Source={0};UTF8Encoding=True;Password={1};journal mode=wal;synchronous=off;", (object) dataSource, (object) password) : throw new FileNotFoundException("Can not create the database object!", dataSource);
          dbObject = DbBasis.getDbObject(type, connectionString2);
          break;
        case MeterDbTypes.MSSQL:
          if (string.IsNullOrEmpty(databaseName))
            throw new ArgumentNullException(nameof (databaseName), "Can not create the database object!");
          string connectionString3 = string.Format("Data Source={0};Database={3};Persist Security Info=True;User Id={2};Password={1};", (object) dataSource, (object) password, (object) user, (object) databaseName);
          dbObject = DbBasis.getDbObject(type, connectionString3);
          break;
        default:
          throw new NotSupportedException("This type of database is not supported! Type: " + type.ToString());
      }
      dbObject.BaseDbConnection.ConnectDatabase();
      return dbObject;
    }

    public static bool IsDatabaseValid(DatabaseConnectionSettings database)
    {
      if (database == null)
        return false;
      try
      {
        IDbConnection dbConnection = GmmInterface.GetDatabase(database.Type, database.DataSource, database.Password, database.User, database.DatabaseName).GetDbConnection();
        dbConnection.Open();
        dbConnection.Close();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static string GetMetrologicalCore() => MetrologicalCore.Get();

    public static void Dispose()
    {
      GmmInterface.logger.Debug("Dispose GMM driver");
      if (GmmInterface.jobManager != null)
      {
        GmmInterface.jobManager.Dispose();
        GmmInterface.jobManager = (JobManager) null;
      }
      if (GmmInterface.configuratorManager != null)
      {
        GmmInterface.configuratorManager.Dispose();
        GmmInterface.configuratorManager = (ConfiguratorManager) null;
      }
      if (GmmInterface.handlerManager != null)
      {
        GmmInterface.handlerManager.Dispose();
        GmmInterface.handlerManager = (HandlerManager) null;
      }
      GmmInterface.Devices.Dispose();
      StartupManager.Dispose();
    }

    public void Close()
    {
      if (GmmInterface.Devices == null)
        return;
      GmmInterface.Devices.Close();
    }
  }
}
