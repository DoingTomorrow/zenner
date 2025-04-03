// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.DatabaseUpgradeManager
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using GmmDbLib;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SQLite;
using System.IO;
using System.IO.Packaging;

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class DatabaseUpgradeManager
  {
    private static Logger logger = LogManager.GetLogger(nameof (DatabaseUpgradeManager));
    private const string NAME_OF_SETUP_DATABASE = "MeterDB.set";
    private const string NAME_OF_UPDATE_INFO_FILE = "Database.txt";
    private const string DEFAULT_DATABASE_FILE_NAME = "MeterDB.db3";
    public const string DEFAULT_GMM_DATABASE_CONNECTION_STRING = "Data Source={0};UTF8Encoding=True;Password=meterdbpass;journal mode=wal;synchronous=off;";
    public const string DEFAULT_GMM_DATABASE_CONNECTION_STRING_ACCESS = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Mode=ReadWrite|Share Deny None;Extended Properties=;Jet OLEDB:System database=;Jet OLEDB:Registry Path=;Jet OLEDB:Database Password=meterdbpass;Jet OLEDB:Engine Type=5;Jet OLEDB:Database Locking Mode=1;Jet OLEDB:Global Partial Bulk Ops=2;Jet OLEDB:Global Bulk Transactions=1;Jet OLEDB:New Database Password=;Jet OLEDB:Create System Database=False;Jet OLEDB:Encrypt Database=False;Jet OLEDB:Don't Copy Locale on Compact=False;Jet OLEDB:Compact Without Replica Repair=False;Jet OLEDB:SFP=False;";
    private DbBasis oldDatabase;
    private DbBasis newDatabase;
    private DbBasis originalDatabase;
    private MeterDatabase manager;

    public DatabaseUpgradeManager(DbBasis oldDatabase, DbBasis newDatabase)
    {
      if (oldDatabase == null)
        throw new ArgumentNullException("Construct parameter 'oldDatabase' can not be null!");
      if (newDatabase == null)
        throw new ArgumentNullException("Construct parameter 'newDatabase' can not be null!");
      this.oldDatabase = oldDatabase;
      this.newDatabase = newDatabase;
      this.IsMergeMode = false;
      this.manager = new MeterDatabase();
    }

    public int? OldDatabaseVersion => MeterDatabase.GetDatabaseVersion(this.oldDatabase);

    public string OldDatabaseVersionDate => MeterDatabase.GetDatabaseVersionDate(this.oldDatabase);

    public int? NewDatabaseVersion => MeterDatabase.GetDatabaseVersion(this.newDatabase);

    public string NewDatabaseVersionDate => MeterDatabase.GetDatabaseVersionDate(this.newDatabase);

    public string PathToCreatedDatabase { get; private set; }

    public string PathToNewDatabaseFile
    {
      get => DatabaseUpgradeManager.GetPathToDatabase(this.newDatabase);
    }

    public string PathToOldDatabaseFile
    {
      get => DatabaseUpgradeManager.GetPathToDatabase(this.oldDatabase);
    }

    public bool IsMergeMode { get; set; }

    public event EventHandler<UpgradeActionEventArgs> OnActionStateChanged;

    public static bool IsUpdate()
    {
      return DatabaseUpgradeManager.ExistPropertyWithValueByDatabaseInfoFile("UPDATE", "TRUE");
    }

    private static bool ExistPropertyWithValueByDatabaseInfoFile(
      string propertyName,
      string propertyValue)
    {
      string path = Path.Combine(SystemValues.DatabasePath, "Database.txt");
      if (!File.Exists(path))
        return false;
      string str = File.ReadAllText(path);
      if (string.IsNullOrEmpty(str))
        return false;
      int startIndex1 = str.IndexOf(propertyName, StringComparison.CurrentCultureIgnoreCase);
      if (startIndex1 < 0)
        return false;
      int startIndex2 = str.IndexOf("=", startIndex1, StringComparison.CurrentCultureIgnoreCase);
      return startIndex2 >= 0 && str.IndexOf(propertyValue, startIndex2, StringComparison.CurrentCultureIgnoreCase) >= 0;
    }

    public static bool IsNewInstalation()
    {
      return DatabaseUpgradeManager.IsNewInstalation(Path.Combine(SystemValues.DatabasePath, "MeterDB.db3"));
    }

    public static bool IsNewInstalation(string pathToDatabase)
    {
      bool flag1 = DatabaseUpgradeManager.ExistPropertyWithValueByDatabaseInfoFile("UPDATE", "TRUE");
      bool flag2 = File.Exists(Path.Combine(SystemValues.DatabasePath, "MeterDB.set"));
      bool flag3 = File.Exists(pathToDatabase);
      return flag1 & flag2 && !flag3;
    }

    public static void DeleteDatabaseInfoFile()
    {
      DatabaseUpgradeManager.logger.Info(nameof (DeleteDatabaseInfoFile));
      File.Delete(Path.Combine(SystemValues.DatabasePath, "Database.txt"));
    }

    public List<UpgradeAction> GetListOfUpgradeActions()
    {
      int? oldDatabaseVersion = this.OldDatabaseVersion;
      int? newDatabaseVersion = this.NewDatabaseVersion;
      List<UpgradeAction> ofUpgradeActions = new List<UpgradeAction>();
      if (!oldDatabaseVersion.HasValue || !newDatabaseVersion.HasValue)
        return ofUpgradeActions;
      ofUpgradeActions.Add(UpgradeAction.VerifyDatabase);
      int? nullable = oldDatabaseVersion;
      int num1 = 2;
      int num2;
      if (nullable.GetValueOrDefault() == num1 & nullable.HasValue)
      {
        nullable = newDatabaseVersion;
        int num3 = 777;
        num2 = nullable.GetValueOrDefault() == num3 & nullable.HasValue ? 1 : 0;
      }
      else
        num2 = 0;
      if (num2 != 0)
      {
        ofUpgradeActions.Add(UpgradeAction.UpgradeFilter);
        ofUpgradeActions.Add(UpgradeAction.UpgradeFilterValue);
        ofUpgradeActions.Add(UpgradeAction.UpgradeGMM_User);
        ofUpgradeActions.Add(UpgradeAction.UpgradeMeterType);
        ofUpgradeActions.Add(UpgradeAction.UpgradeMeterInfo);
        ofUpgradeActions.Add(UpgradeAction.UpgradeMTypeZelsius);
        ofUpgradeActions.Add(UpgradeAction.UpgradeMeter);
        ofUpgradeActions.Add(UpgradeAction.UpgradeNodeList);
        ofUpgradeActions.Add(UpgradeAction.UpgradeNodeLayers);
        ofUpgradeActions.Add(UpgradeAction.UpgradeNodeReferences);
        ofUpgradeActions.Add(UpgradeAction.UpgradeMeterValues);
      }
      else
      {
        nullable = oldDatabaseVersion;
        int num4 = 2;
        int num5;
        if (nullable.GetValueOrDefault() == num4 & nullable.HasValue)
        {
          nullable = newDatabaseVersion;
          int num6 = 2;
          num5 = nullable.GetValueOrDefault() == num6 & nullable.HasValue ? 1 : 0;
        }
        else
          num5 = 0;
        if (num5 != 0)
        {
          if (this.IsMergeMode)
          {
            ofUpgradeActions.Add(UpgradeAction.MergeData);
          }
          else
          {
            ofUpgradeActions.Add(UpgradeAction.CreateCopyOfSetupDB);
            ofUpgradeActions.Add(UpgradeAction.UpgradeFilter);
            ofUpgradeActions.Add(UpgradeAction.UpgradeFilterValue);
            ofUpgradeActions.Add(UpgradeAction.UpgradeGMM_User);
            ofUpgradeActions.Add(UpgradeAction.UpgradeSoftwareUsers);
            ofUpgradeActions.Add(UpgradeAction.UpgradeUserPermissions);
            ofUpgradeActions.Add(UpgradeAction.UpgradeMeterType);
            ofUpgradeActions.Add(UpgradeAction.UpgradeMeterInfo);
            ofUpgradeActions.Add(UpgradeAction.UpgradeMTypeZelsius);
            ofUpgradeActions.Add(UpgradeAction.UpgradeMeter);
            ofUpgradeActions.Add(UpgradeAction.UpgradeNodeList);
            ofUpgradeActions.Add(UpgradeAction.UpgradeNodeLayers);
            ofUpgradeActions.Add(UpgradeAction.UpgradeNodeReferences);
            ofUpgradeActions.Add(UpgradeAction.UpgradeMeterValues);
            ofUpgradeActions.Add(UpgradeAction.BackupOldDatabase);
          }
        }
        else
        {
          nullable = oldDatabaseVersion;
          int num7 = 1;
          int num8;
          if (nullable.GetValueOrDefault() == num7 & nullable.HasValue)
          {
            nullable = newDatabaseVersion;
            int num9 = 2;
            num8 = nullable.GetValueOrDefault() == num9 & nullable.HasValue ? 1 : 0;
          }
          else
            num8 = 0;
          if (num8 != 0)
          {
            if (this.IsMergeMode)
            {
              ofUpgradeActions.Add(UpgradeAction.UseSetupDatabase);
              ofUpgradeActions.Add(UpgradeAction.UpgradeGMM_User);
              ofUpgradeActions.Add(UpgradeAction.UpgradeMeterType);
              ofUpgradeActions.Add(UpgradeAction.UpgradeMeterInfo);
              ofUpgradeActions.Add(UpgradeAction.UpgradeMTypeZelsius);
              ofUpgradeActions.Add(UpgradeAction.UpgradeMeter);
              ofUpgradeActions.Add(UpgradeAction.UpgradeNodeList_1_to_2);
              ofUpgradeActions.Add(UpgradeAction.UpgradeNodeReferences_1_to_2);
              ofUpgradeActions.Add(UpgradeAction.UpgradeSubdevices_1_to_2);
              ofUpgradeActions.Add(UpgradeAction.UpgradeMeterValues_1_to_2);
              ofUpgradeActions.Add(UpgradeAction.UpgradeMeterValues);
              ofUpgradeActions.Add(UpgradeAction.UseWorkDatabase);
              ofUpgradeActions.Add(UpgradeAction.MergeData);
              ofUpgradeActions.Add(UpgradeAction.DeleteTempDatabase);
            }
            else
            {
              ofUpgradeActions.Add(UpgradeAction.CreateCopyOfSetupDB);
              ofUpgradeActions.Add(UpgradeAction.UpgradeGMM_User);
              ofUpgradeActions.Add(UpgradeAction.UpgradeMeterType);
              ofUpgradeActions.Add(UpgradeAction.UpgradeMeterInfo);
              ofUpgradeActions.Add(UpgradeAction.UpgradeMTypeZelsius);
              ofUpgradeActions.Add(UpgradeAction.UpgradeMeter);
              ofUpgradeActions.Add(UpgradeAction.UpgradeNodeList_1_to_2);
              ofUpgradeActions.Add(UpgradeAction.UpgradeNodeReferences_1_to_2);
              ofUpgradeActions.Add(UpgradeAction.UpgradeSubdevices_1_to_2);
              ofUpgradeActions.Add(UpgradeAction.UpgradeMeterValues_1_to_2);
              ofUpgradeActions.Add(UpgradeAction.UpgradeMeterValues);
              if (!(this.oldDatabase is MSSQLDB))
                ofUpgradeActions.Add(UpgradeAction.BackupOldDatabase);
            }
          }
        }
      }
      return ofUpgradeActions;
    }

    private void RaiseEventMessage(UpgradeAction act, MeterDatabase.Progress e)
    {
      this.RaiseEventMessage(act, string.Format("{0} % (success: {1}, error: {2}, count: {3})", (object) e.ProgressValue, (object) e.Successful, (object) e.Failed, (object) e.Count));
    }

    private void RaiseEventMessage(UpgradeAction act, string message)
    {
      if (this.OnActionStateChanged == null)
        return;
      this.OnActionStateChanged((object) this, new UpgradeActionEventArgs()
      {
        Action = act,
        State = message
      });
    }

    public void StartUpgrade()
    {
      List<UpgradeAction> ofUpgradeActions = this.GetListOfUpgradeActions();
      if (ofUpgradeActions == null)
        return;
      foreach (UpgradeAction upgradeAction in ofUpgradeActions)
      {
        bool flag;
        switch ((UpgradeAction) Enum.ToObject(typeof (UpgradeAction), (object) upgradeAction))
        {
          case UpgradeAction.VerifyDatabase:
            flag = this.VerifyDatabase();
            break;
          case UpgradeAction.CreateCopyOfSetupDB:
            flag = this.CreateCopyOfSetupDB();
            break;
          case UpgradeAction.UpgradeFilter:
            flag = this.UpgradeTableFilter();
            break;
          case UpgradeAction.UpgradeFilterValue:
            flag = this.UpgradeTableFilterValue();
            break;
          case UpgradeAction.UpgradeGMM_User:
            flag = this.UpgradeTableGMM_User();
            break;
          case UpgradeAction.UpgradeMeterType:
            flag = this.UpgradeTableMeterType();
            break;
          case UpgradeAction.UpgradeMeterInfo:
            flag = this.UpgradeTableMeterInfo();
            break;
          case UpgradeAction.UpgradeMTypeZelsius:
            flag = this.UpgradeTableMTypeZelsius();
            break;
          case UpgradeAction.UpgradeMeter:
            flag = this.UpgradeTableMeter();
            break;
          case UpgradeAction.UpgradeNodeList:
            flag = this.UpgradeTableNodeList();
            break;
          case UpgradeAction.UpgradeNodeList_1_to_2:
            flag = this.UpgradeTableNodeList_1_to_2();
            break;
          case UpgradeAction.UpgradeSubdevices_1_to_2:
            flag = this.UpgradeSubdevices_1_to_2();
            break;
          case UpgradeAction.UpgradeNodeLayers:
            flag = this.UpgradeTableNodeLayers();
            break;
          case UpgradeAction.UpgradeNodeReferences:
            flag = this.UpgradeTableNodeReferences();
            break;
          case UpgradeAction.UpgradeNodeReferences_1_to_2:
            flag = this.UpgradeTableNodeReferences_1_to_2();
            break;
          case UpgradeAction.UpgradeMeterValues:
            flag = this.UpgradeTableMeterValues();
            break;
          case UpgradeAction.UpgradeMeterValues_1_to_2:
            flag = this.UpgradeTableMeterValues_1_to_2();
            break;
          case UpgradeAction.BackupOldDatabase:
            flag = this.BackupOldDatabase();
            break;
          case UpgradeAction.UseSetupDatabase:
            flag = this.UseSetupDatabase();
            break;
          case UpgradeAction.UseWorkDatabase:
            flag = this.UseWorkDatabase();
            break;
          case UpgradeAction.MergeData:
            flag = this.MergeData();
            break;
          case UpgradeAction.DeleteTempDatabase:
            flag = this.DeleteTempDatabase();
            break;
          case UpgradeAction.UpgradeUserPermissions:
            flag = this.UpgradeTableUserPermissions();
            break;
          case UpgradeAction.UpgradeSoftwareUsers:
            flag = this.UpgradeTableSoftwareUsers();
            break;
          default:
            throw new ArgumentException("Action doesn't implemented! Value: " + upgradeAction.ToString());
        }
        if (flag)
          ;
      }
    }

    private bool VerifyDatabase()
    {
      this.RaiseEventMessage(UpgradeAction.VerifyDatabase, "Please wait...");
      int? oldDatabaseVersion = this.OldDatabaseVersion;
      int? newDatabaseVersion = this.NewDatabaseVersion;
      if (!oldDatabaseVersion.HasValue)
      {
        this.RaiseEventMessage(UpgradeAction.VerifyDatabase, "Old database con not be null!");
        return false;
      }
      if (!newDatabaseVersion.HasValue)
      {
        this.RaiseEventMessage(UpgradeAction.VerifyDatabase, "New database con not be null!");
        return false;
      }
      if (oldDatabaseVersion.Value > newDatabaseVersion.Value)
      {
        this.RaiseEventMessage(UpgradeAction.VerifyDatabase, string.Format("Can not upgrade database from version {0} to {1}!", (object) oldDatabaseVersion, (object) newDatabaseVersion));
        return false;
      }
      this.RaiseEventMessage(UpgradeAction.VerifyDatabase, "OK");
      return true;
    }

    private bool CreateCopyOfSetupDB()
    {
      this.RaiseEventMessage(UpgradeAction.CreateCopyOfSetupDB, "Please wait...");
      File.Copy(this.PathToNewDatabaseFile, this.PathToNewDatabaseFile + ".copy", true);
      this.RaiseEventMessage(UpgradeAction.CreateCopyOfSetupDB, "OK");
      return true;
    }

    private bool UseSetupDatabase()
    {
      this.RaiseEventMessage(UpgradeAction.UseSetupDatabase, "Please wait...");
      string str = Path.Combine(SystemValues.DatabasePath, "MeterDB.set.copy");
      if (!File.Exists(str))
        return false;
      File.Copy(str, str.Replace(".copy", ""));
      string path = Path.Combine(SystemValues.DatabasePath, "MeterDB.set");
      if (!File.Exists(path))
        return false;
      DbBasis dbObject = DbBasis.getDbObject(MeterDbTypes.SQLite, string.Format("Data Source={0};UTF8Encoding=True;Password=meterdbpass;journal mode=wal;synchronous=off;", (object) path));
      this.originalDatabase = this.newDatabase;
      this.newDatabase = dbObject;
      this.RaiseEventMessage(UpgradeAction.UseSetupDatabase, "OK");
      return true;
    }

    private bool UseWorkDatabase()
    {
      this.RaiseEventMessage(UpgradeAction.UseWorkDatabase, "Please wait...");
      this.oldDatabase = this.newDatabase;
      this.newDatabase = this.originalDatabase;
      this.RaiseEventMessage(UpgradeAction.UseWorkDatabase, "OK");
      return true;
    }

    private bool DeleteTempDatabase()
    {
      this.RaiseEventMessage(UpgradeAction.DeleteTempDatabase, "Please wait...");
      File.Delete(this.PathToOldDatabaseFile);
      this.RaiseEventMessage(UpgradeAction.DeleteTempDatabase, "OK");
      return true;
    }

    private bool BackupOldDatabase()
    {
      this.RaiseEventMessage(UpgradeAction.BackupOldDatabase, "Please wait...");
      if (this.oldDatabase is AccessDB || this.oldDatabase is SQLiteDB)
      {
        if (DatabaseUpgradeManager.CreateBackupOfOldDatabase(this.PathToOldDatabaseFile))
        {
          this.RaiseEventMessage(UpgradeAction.BackupOldDatabase, "OK");
        }
        else
        {
          this.RaiseEventMessage(UpgradeAction.BackupOldDatabase, "Failed!");
          return false;
        }
      }
      return true;
    }

    public static bool CreateBackupOfOldDatabase(string pathToOldDatabaseFile)
    {
      if (string.IsNullOrEmpty(pathToOldDatabaseFile))
        throw new ArgumentNullException("Input patrameter 'pathToOldDatabaseFile' can not be null!");
      using (Package package = Package.Open(pathToOldDatabaseFile.Replace(Path.GetExtension(pathToOldDatabaseFile), "_BACKUP_") + string.Format("{0:yyyy-MM-dd_hh-mm-ss}.zip", (object) DateTime.Now), FileMode.OpenOrCreate))
      {
        Uri partUri = PackUriHelper.CreatePartUri(new Uri(".\\" + Path.GetFileName(pathToOldDatabaseFile), UriKind.Relative));
        if (package.PartExists(partUri))
          package.DeletePart(partUri);
        PackagePart part = package.CreatePart(partUri, "", CompressionOption.Maximum);
        using (FileStream inputStream = new FileStream(pathToOldDatabaseFile, FileMode.Open, FileAccess.Read))
        {
          using (Stream stream = part.GetStream())
            DatabaseUpgradeManager.CopyStream(inputStream, stream);
        }
      }
      File.Delete(pathToOldDatabaseFile);
      return true;
    }

    private static void CopyStream(FileStream inputStream, Stream outputStream)
    {
      long length = inputStream.Length < 4096L ? inputStream.Length : 4096L;
      byte[] buffer = new byte[length];
      long num = 0;
      int count;
      while ((count = inputStream.Read(buffer, 0, buffer.Length)) != 0)
      {
        outputStream.Write(buffer, 0, count);
        num += length;
      }
    }

    private bool UpgradeTableFilter()
    {
      EventHandler<MeterDatabase.Progress> eventHandler = (EventHandler<MeterDatabase.Progress>) ((sender, e) => this.RaiseEventMessage(UpgradeAction.UpgradeFilter, e));
      this.manager.OnProgress += eventHandler;
      try
      {
        return this.manager.TransferDataToEmptyDatabase(this.oldDatabase, this.newDatabase, "Filter", string.Empty);
      }
      finally
      {
        this.manager.OnProgress -= eventHandler;
      }
    }

    private bool UpgradeTableFilterValue()
    {
      EventHandler<MeterDatabase.Progress> eventHandler = (EventHandler<MeterDatabase.Progress>) ((sender, e) => this.RaiseEventMessage(UpgradeAction.UpgradeFilterValue, e));
      this.manager.OnProgress += eventHandler;
      try
      {
        return this.manager.TransferDataToEmptyDatabase(this.oldDatabase, this.newDatabase, "FilterValue", string.Empty);
      }
      finally
      {
        this.manager.OnProgress -= eventHandler;
      }
    }

    private bool UpgradeTableGMM_User()
    {
      EventHandler<MeterDatabase.Progress> eventHandler = (EventHandler<MeterDatabase.Progress>) ((sender, e) => this.RaiseEventMessage(UpgradeAction.UpgradeGMM_User, e));
      this.manager.OnProgress += eventHandler;
      try
      {
        return this.manager.TransferDataToEmptyDatabase(this.oldDatabase, this.newDatabase, "GMM_User", "UserName <> 'Administrator'");
      }
      finally
      {
        this.manager.OnProgress -= eventHandler;
      }
    }

    private bool UpgradeTableUserPermissions()
    {
      if (!MeterDatabase.ExistTable(this.oldDatabase, "SoftwareUsers"))
        return true;
      EventHandler<MeterDatabase.Progress> eventHandler = (EventHandler<MeterDatabase.Progress>) ((sender, e) => this.RaiseEventMessage(UpgradeAction.UpgradeUserPermissions, e));
      this.manager.OnProgress += eventHandler;
      try
      {
        return this.manager.TransferDataToEmptyDatabase(this.oldDatabase, this.newDatabase, "UserPermissions", string.Empty);
      }
      finally
      {
        this.manager.OnProgress -= eventHandler;
      }
    }

    private bool UpgradeTableSoftwareUsers()
    {
      if (!MeterDatabase.ExistTable(this.oldDatabase, "SoftwareUsers"))
        return true;
      EventHandler<MeterDatabase.Progress> eventHandler = (EventHandler<MeterDatabase.Progress>) ((sender, e) => this.RaiseEventMessage(UpgradeAction.UpgradeSoftwareUsers, e));
      this.manager.OnProgress += eventHandler;
      try
      {
        return this.manager.TransferDataToEmptyDatabase(this.oldDatabase, this.newDatabase, "SoftwareUsers", string.Empty);
      }
      finally
      {
        this.manager.OnProgress -= eventHandler;
      }
    }

    private bool UpgradeTableMeterInfo()
    {
      EventHandler<MeterDatabase.Progress> eventHandler = (EventHandler<MeterDatabase.Progress>) ((sender, e) => this.RaiseEventMessage(UpgradeAction.UpgradeMeterInfo, e));
      this.manager.OnProgress += eventHandler;
      try
      {
        if (!this.manager.TransferDataToEmptyDatabase(this.oldDatabase, this.newDatabase, "MeterInfo", "MeterInfoID > 99999"))
          return false;
        int? nextUniqueId = MeterDatabase.GetNextUniqueID(this.oldDatabase, "MeterInfo", "MeterInfoID");
        if (nextUniqueId.HasValue && !MeterDatabase.UpdateNextUniqueID(this.newDatabase, "MeterInfo", "MeterInfoID", nextUniqueId.Value))
          DatabaseUpgradeManager.logger.Error<int?>("Can not update NextUniqueID! Table: MeterInfo, Field: MeterInfoID, Value: {0}", nextUniqueId);
        return true;
      }
      finally
      {
        this.manager.OnProgress -= eventHandler;
      }
    }

    private bool UpgradeTableMeterType()
    {
      EventHandler<MeterDatabase.Progress> eventHandler = (EventHandler<MeterDatabase.Progress>) ((sender, e) => this.RaiseEventMessage(UpgradeAction.UpgradeMeterType, e));
      this.manager.OnProgress += eventHandler;
      try
      {
        if (!this.manager.TransferDataToEmptyDatabase(this.oldDatabase, this.newDatabase, "MeterType", "MeterTypeID > 99999"))
          return false;
        int? nextUniqueId = MeterDatabase.GetNextUniqueID(this.oldDatabase, "MeterType", "MeterTypeID");
        if (nextUniqueId.HasValue && !MeterDatabase.UpdateNextUniqueID(this.newDatabase, "MeterType", "MeterTypeID", nextUniqueId.Value))
          DatabaseUpgradeManager.logger.Error<int?>("Can not update NextUniqueID! Table: MeterType, Field: MeterTypeID, Value: {0}", nextUniqueId);
        return true;
      }
      finally
      {
        this.manager.OnProgress -= eventHandler;
      }
    }

    private bool UpgradeTableMTypeZelsius()
    {
      EventHandler<MeterDatabase.Progress> eventHandler = (EventHandler<MeterDatabase.Progress>) ((sender, e) => this.RaiseEventMessage(UpgradeAction.UpgradeMTypeZelsius, e));
      this.manager.OnProgress += eventHandler;
      try
      {
        return this.manager.TransferDataToEmptyDatabase(this.oldDatabase, this.newDatabase, "MTypeZelsius", "MeterTypeID > 99999");
      }
      finally
      {
        this.manager.OnProgress -= eventHandler;
      }
    }

    private bool UpgradeTableMeter()
    {
      EventHandler<MeterDatabase.Progress> eventHandler = (EventHandler<MeterDatabase.Progress>) ((sender, e) => this.RaiseEventMessage(UpgradeAction.UpgradeMeter, e));
      this.manager.OnProgress += eventHandler;
      try
      {
        if (!this.manager.TransferDataToEmptyDatabase(this.oldDatabase, this.newDatabase, "Meter", string.Empty))
          return false;
        int? nextUniqueId = MeterDatabase.GetNextUniqueID(this.oldDatabase, "Meter", "MeterID");
        if (nextUniqueId.HasValue && !MeterDatabase.UpdateNextUniqueID(this.newDatabase, "Meter", "MeterID", nextUniqueId.Value))
          DatabaseUpgradeManager.logger.Error<int?>("Can not update NextUniqueID! Table: Meter, Field: MeterID, Value: {0}", nextUniqueId);
        return true;
      }
      finally
      {
        this.manager.OnProgress -= eventHandler;
      }
    }

    private bool UpgradeTableNodeList()
    {
      EventHandler<MeterDatabase.Progress> eventHandler = (EventHandler<MeterDatabase.Progress>) ((sender, e) => this.RaiseEventMessage(UpgradeAction.UpgradeNodeList, e));
      this.manager.OnProgress += eventHandler;
      try
      {
        if (!this.manager.TransferDataToEmptyDatabase(this.oldDatabase, this.newDatabase, "NodeList", string.Empty))
          return false;
        int? nextUniqueId = MeterDatabase.GetNextUniqueID(this.oldDatabase, "NodeList", "NodeID");
        if (nextUniqueId.HasValue && !MeterDatabase.UpdateNextUniqueID(this.newDatabase, "NodeList", "NodeID", nextUniqueId.Value))
          DatabaseUpgradeManager.logger.Error<int?>("Can not update NextUniqueID! Table: NodeList, Field: NodeID, Value: {0}", nextUniqueId);
        return true;
      }
      finally
      {
        this.manager.OnProgress -= eventHandler;
      }
    }

    private bool UpgradeTableNodeLayers()
    {
      EventHandler<MeterDatabase.Progress> eventHandler = (EventHandler<MeterDatabase.Progress>) ((sender, e) => this.RaiseEventMessage(UpgradeAction.UpgradeNodeLayers, e));
      this.manager.OnProgress += eventHandler;
      try
      {
        return this.manager.TransferDataToEmptyDatabase(this.oldDatabase, this.newDatabase, "NodeLayers", string.Empty);
      }
      finally
      {
        this.manager.OnProgress -= eventHandler;
      }
    }

    private bool UpgradeTableNodeReferences()
    {
      EventHandler<MeterDatabase.Progress> eventHandler = (EventHandler<MeterDatabase.Progress>) ((sender, e) => this.RaiseEventMessage(UpgradeAction.UpgradeNodeReferences, e));
      this.manager.OnProgress += eventHandler;
      try
      {
        return this.manager.TransferDataToEmptyDatabase(this.oldDatabase, this.newDatabase, "NodeReferences", string.Empty);
      }
      finally
      {
        this.manager.OnProgress -= eventHandler;
      }
    }

    private bool UpgradeTableMeterValues()
    {
      this.RaiseEventMessage(UpgradeAction.UpgradeMeterValues, "Please wait...");
      EventHandler<MeterDatabase.Progress> eventHandler = (EventHandler<MeterDatabase.Progress>) ((sender, e) => this.RaiseEventMessage(UpgradeAction.UpgradeMeterValues, e));
      this.manager.OnProgress += eventHandler;
      try
      {
        return this.manager.TransferDataToEmptyDatabase(this.oldDatabase, this.newDatabase, "MeterValues", string.Empty);
      }
      finally
      {
        this.manager.OnProgress -= eventHandler;
      }
    }

    private bool UpgradeTableNodeList_1_to_2()
    {
      EventHandler<MeterDatabase.Progress> eventHandler = (EventHandler<MeterDatabase.Progress>) ((sender, e) => this.RaiseEventMessage(UpgradeAction.UpgradeNodeList_1_to_2, e));
      this.manager.OnProgress += eventHandler;
      try
      {
        if (!this.manager.TransferTableNodeList_1_to_2(this.oldDatabase, this.newDatabase))
          return false;
        int? nextUniqueId = MeterDatabase.GetNextUniqueID(this.oldDatabase, "NodeList", "NodeID");
        if (nextUniqueId.HasValue && !MeterDatabase.UpdateNextUniqueID(this.newDatabase, "NodeList", "NodeID", nextUniqueId.Value))
          DatabaseUpgradeManager.logger.Error<int?>("Can not update NextUniqueID! Table: NodeList, Field: NodeID, Value: {0}", nextUniqueId);
        return true;
      }
      finally
      {
        this.manager.OnProgress -= eventHandler;
      }
    }

    private bool UpgradeSubdevices_1_to_2()
    {
      EventHandler<MeterDatabase.Progress> eventHandler = (EventHandler<MeterDatabase.Progress>) ((sender, e) => this.RaiseEventMessage(UpgradeAction.UpgradeSubdevices_1_to_2, e));
      this.manager.OnProgress += eventHandler;
      try
      {
        List<StructureTreeNode> structureTreeNodeList = MeterDatabase.LoadMeterInstallerTreesByLayerID(this.newDatabase, 0);
        if (structureTreeNodeList == null || structureTreeNodeList.Count == 0)
          return true;
        foreach (StructureTreeNode root in structureTreeNodeList)
        {
          foreach (StructureTreeNode tree in StructureTreeNode.ForEach(root))
          {
            bool flag = tree.NodeTyp == StructureNodeType.Meter && tree.Parent != null && tree.Parent.NodeTyp == StructureNodeType.Meter;
            string nodeSettings = tree.NodeSettings;
            ParameterService.DeleteParameter(ref nodeSettings, "ExternalIOs");
            ParameterService.DeleteParameter(ref nodeSettings, "VALUE_REQ_ID");
            tree.NodeSettings = nodeSettings;
            if (flag)
            {
              string parameter1 = ParameterService.GetParameter(tree.NodeSettings, "MED");
              string parameter2 = ParameterService.GetParameter(tree.NodeSettings, "SID");
              string parameter3 = ParameterService.GetParameter(tree.NodeSettings, "MeterTypeTranslation");
              int result = 1;
              if (string.IsNullOrEmpty(parameter3) || !int.TryParse(parameter3, out result))
              {
                string parameter4 = ParameterService.GetParameter(tree.Parent.NodeSettings, "SID[1]");
                string parameter5 = ParameterService.GetParameter(tree.Parent.NodeSettings, "SID[2]");
                if (!string.IsNullOrEmpty(parameter4) && parameter4 == parameter2)
                  result = 1;
                else if (!string.IsNullOrEmpty(parameter5) && parameter5 == parameter2)
                  result = 2;
                else
                  continue;
              }
              tree.NodeSettings = tree.Parent.NodeSettings;
              tree.SubDeviceIndex = result;
              if (!string.IsNullOrEmpty(parameter1) && Enum.IsDefined(typeof (MBusDeviceType), (object) parameter1))
                tree.Medium = (MBusDeviceType) Enum.Parse(typeof (MBusDeviceType), parameter1);
              tree.SerialNumber = parameter2;
            }
            int num1;
            if (tree.MeterID.HasValue)
            {
              int? meterId = tree.MeterID;
              int num2 = 0;
              num1 = meterId.GetValueOrDefault() == num2 & meterId.HasValue ? 1 : 0;
            }
            else
              num1 = 1;
            if (num1 != 0)
              tree.ReadEnabled = false;
            MeterDatabase.SaveTreeNode(this.newDatabase, tree);
          }
        }
        return true;
      }
      finally
      {
        this.manager.OnProgress -= eventHandler;
      }
    }

    private bool UpgradeTableNodeReferences_1_to_2()
    {
      EventHandler<MeterDatabase.Progress> eventHandler = (EventHandler<MeterDatabase.Progress>) ((sender, e) => this.RaiseEventMessage(UpgradeAction.UpgradeNodeReferences_1_to_2, e));
      this.manager.OnProgress += eventHandler;
      try
      {
        return this.manager.TransferDataToEmptyDatabase(this.oldDatabase, this.newDatabase, "NodeReferences", "LayerID > -1");
      }
      finally
      {
        this.manager.OnProgress -= eventHandler;
      }
    }

    private bool UpgradeTableMeterValues_1_to_2()
    {
      this.RaiseEventMessage(UpgradeAction.UpgradeMeterValues_1_to_2, "Please wait...");
      EventHandler<MeterDatabase.Progress> eventHandler = (EventHandler<MeterDatabase.Progress>) ((sender, e) => this.RaiseEventMessage(UpgradeAction.UpgradeMeterValues_1_to_2, e));
      this.manager.OnProgress += eventHandler;
      try
      {
        return this.manager.TransferTableMeterValues_1_to_2(this.oldDatabase, this.newDatabase);
      }
      finally
      {
        this.manager.OnProgress -= eventHandler;
      }
    }

    private static string GetPathToDatabase(DbBasis db)
    {
      if (db == null)
        return string.Empty;
      string connectionString = db.ConnectionString;
      if (string.IsNullOrEmpty(connectionString))
        return string.Empty;
      switch (db)
      {
        case AccessDB _:
          return new OleDbConnectionStringBuilder(connectionString).DataSource;
        case SQLiteDB _:
          return new SQLiteConnectionStringBuilder(connectionString).DataSource;
        default:
          return string.Empty;
      }
    }

    private bool MergeData()
    {
      EventHandler<MeterDatabase.Progress> eventHandler = (EventHandler<MeterDatabase.Progress>) ((sender, e) => this.RaiseEventMessage(UpgradeAction.MergeData, e));
      this.manager.OnProgress += eventHandler;
      try
      {
        return true;
      }
      finally
      {
        this.manager.OnProgress -= eventHandler;
      }
    }
  }
}
