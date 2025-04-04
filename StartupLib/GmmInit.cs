// Decompiled with JetBrains decompiler
// Type: StartupLib.GmmInit
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using CommonWPF;
using GmmDbLib;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Markup;
using ZR_ClassLibrary;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace StartupLib
{
  public class GmmInit
  {
    private Logger gmmInitLogger = LogManager.GetLogger(nameof (GmmInit));
    private Mutex mutexGMM = new Mutex(false, "www.zenner.de GlobalMeterManager");

    public static List<CultureInfo> SupportedCulturesList { get; private set; }

    public GmmInit()
    {
      SystemValues systemValues = new SystemValues();
      PlugInLoader.GmmConfiguration = new GMMConfig();
      ZR_ClassLibMessages.RegisterThreadErrorMsgList();
    }

    public void Dispose()
    {
      this.mutexGMM.Close();
      this.mutexGMM.Dispose();
      this.mutexGMM = (Mutex) null;
    }

    public bool LoadConfiguration(string startupFile)
    {
      if (startupFile == null)
        PlugInLoader.GmmConfiguration.ReadConfigForPlugInGmm((StartupManager.StaticInstance.GetStartupInfo() ?? throw new Exception("Can not load configuration! The StartupInfo is not available.")).StartupFile);
      else
        PlugInLoader.GmmConfiguration.ReadConfigForPlugInGmm(startupFile);
      ZR_Component.CommonGmmInterface = new ZR_Component();
      ZR_Component.CommonGmmInterface.OnGarantComponentLoaded += new ZR_Component.GarantComponentLoadedFunction(PlugInLoader.GarantComponentLoaded);
      ZR_Component.CommonGmmInterface.LoadedComponentsList.Add(GMM_Components.KonfigGroup, (object) PlugInLoader.GmmConfiguration);
      this.SetGUILanguage();
      return true;
    }

    public bool InitGMM()
    {
      UserRights.GlobalUserRights = new UserRights();
      UserRights.GlobalUserRights.PluginGMMFlag = true;
      UserRights.GlobalUserRights.OnPGMMCheckPermission += new UserRights.PGMMCheckPermission(UserManager.CheckPermission);
      if (DatabaseUpgradeManager.IsNewInstalation(PlugInLoader.GmmConfiguration.GetValue("MainDataBase", "GMM_DateiName")))
        this.RenameSetupDatabaseAndUseIt();
      try
      {
        string strInhalt1 = PlugInLoader.GmmConfiguration.GetValue("MainDataBase", "GMM_DBType").ToUpper();
        string str1 = PlugInLoader.GmmConfiguration.GetValue("MainDataBase", "GMM_DataBase");
        this.gmmInitLogger.Info<string, string>("{0} {1}", strInhalt1, str1);
        if (string.IsNullOrEmpty(strInhalt1) && string.IsNullOrEmpty(str1))
        {
          string str2 = Path.Combine(SystemValues.DatabasePath, "MeterDB_New.mdb");
          string str3 = Path.Combine(SystemValues.DatabasePath, "MeterDBExternalTestbench.mdb");
          if (File.Exists(str2))
          {
            strInhalt1 = "ACCESS";
            string strInhalt2 = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Mode=ReadWrite|Share Deny None;Extended Properties=;Jet OLEDB:System database=;Jet OLEDB:Registry Path=;Jet OLEDB:Database Password=meterdbpass;Jet OLEDB:Engine Type=5;Jet OLEDB:Database Locking Mode=1;Jet OLEDB:Global Partial Bulk Ops=2;Jet OLEDB:Global Bulk Transactions=1;Jet OLEDB:New Database Password=;Jet OLEDB:Create System Database=False;Jet OLEDB:Encrypt Database=False;Jet OLEDB:Don't Copy Locale on Compact=False;Jet OLEDB:Compact Without Replica Repair=False;Jet OLEDB:SFP=False;", (object) str2);
            PlugInLoader.GmmConfiguration.SetOrUpdateValue("MainDataBase", "GMM_DBType", strInhalt1);
            PlugInLoader.GmmConfiguration.SetOrUpdateValue("MainDataBase", "GMM_DataBase", strInhalt2);
            PlugInLoader.GmmConfiguration.SetOrUpdateValue("MainDataBase", "GMM_DateiName", str2);
            PlugInLoader.GmmConfiguration.WriteConfigFile();
          }
          else if (File.Exists(str3))
          {
            strInhalt1 = "ACCESS";
            string strInhalt3 = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Mode=ReadWrite|Share Deny None;Extended Properties=;Jet OLEDB:System database=;Jet OLEDB:Registry Path=;Jet OLEDB:Database Password=meterdbpass;Jet OLEDB:Engine Type=5;Jet OLEDB:Database Locking Mode=1;Jet OLEDB:Global Partial Bulk Ops=2;Jet OLEDB:Global Bulk Transactions=1;Jet OLEDB:New Database Password=;Jet OLEDB:Create System Database=False;Jet OLEDB:Encrypt Database=False;Jet OLEDB:Don't Copy Locale on Compact=False;Jet OLEDB:Compact Without Replica Repair=False;Jet OLEDB:SFP=False;", (object) str3);
            PlugInLoader.GmmConfiguration.SetOrUpdateValue("MainDataBase", "GMM_DBType", strInhalt1);
            PlugInLoader.GmmConfiguration.SetOrUpdateValue("MainDataBase", "GMM_DataBase", strInhalt3);
            PlugInLoader.GmmConfiguration.SetOrUpdateValue("MainDataBase", "GMM_DateiName", str3);
            PlugInLoader.GmmConfiguration.WriteConfigFile();
          }
        }
        if (!this.InitDatabase())
          return false;
        if (!this.mutexGMM.WaitOne(TimeSpan.FromSeconds(1.0), false))
        {
          string MessageString = Ot.GetTranslatedLanguageText("GMM", "StartNewInstance");
          if (string.IsNullOrEmpty(MessageString))
            MessageString = "Would you start new instance of GMM?";
          if (GMM_MessageBox.ShowMessage("Global Meter Manager", MessageString, MessageBoxButtons.YesNo) != DialogResult.Yes)
          {
            this.gmmInitLogger.Info("Start canceled due to multiple instances.");
            return false;
          }
        }
        if (DbBasis.PrimaryDB != null && !string.IsNullOrEmpty(strInhalt1) && DatabaseUpgradeManager.IsUpdate())
        {
          DbBasis primaryDb = DbBasis.PrimaryDB;
          string path = Path.Combine(SystemValues.DatabasePath, "MeterDB.set");
          DbBasis dbObject = DbBasis.getDbObject(MeterDbTypes.SQLite, string.Format("Data Source={0};UTF8Encoding=True;Password=meterdbpass;journal mode=wal;synchronous=off;", (object) path));
          DatabaseUpgradeManager manager = new DatabaseUpgradeManager(primaryDb, dbObject);
          DatabaseUpgradeWindow databaseUpgradeWindow = new DatabaseUpgradeWindow((Form) null, manager);
          if (GMM_MessageBox.ShowMessage("Update MeterDB", Ot.GetTranslatedLanguageText(TranslatorKey.GMMDatabaseUpdateQuestion), MessageBoxButtons.YesNo) == DialogResult.Yes)
          {
            if (databaseUpgradeWindow.ShowDialog() != DialogResult.OK)
              return false;
            string empty = string.Empty;
            string pathToNewDatabase = !string.IsNullOrEmpty(manager.PathToOldDatabaseFile) ? Path.ChangeExtension(manager.PathToOldDatabaseFile, "db3") : Path.ChangeExtension(path, "db3");
            this.RenameSetupDatabaseAndUseIt(pathToNewDatabase);
            DbBasis.PrimaryDB = (DbBasis) null;
            DbBasis.SecondaryDB = (DbBasis) null;
            dbObject.BaseDbConnection.ConnectionInfo.UrlOrPath = pathToNewDatabase;
            string setupString = dbObject.BaseDbConnection.ConnectionInfo.SetupString;
            PlugInLoader.GmmConfiguration.SetOrUpdateValue("Startup", "DbConfigPrimary", setupString);
            PlugInLoader.GmmConfiguration.SetOrUpdateValue("Startup", "DbConfigSecundary", string.Empty);
            PlugInLoader.GmmConfiguration.WriteConfigFile();
            this.InitGMM();
          }
          else
          {
            if (!DatabaseUpgradeManager.CreateBackupOfOldDatabase(manager.PathToOldDatabaseFile))
              return false;
            string pathToNewDatabase = Path.ChangeExtension(manager.PathToOldDatabaseFile, "db3");
            this.RenameSetupDatabaseAndUseIt(pathToNewDatabase);
            DbBasis.PrimaryDB = (DbBasis) null;
            DbBasis.SecondaryDB = (DbBasis) null;
            dbObject.BaseDbConnection.ConnectionInfo.UrlOrPath = pathToNewDatabase;
            string setupString = dbObject.BaseDbConnection.ConnectionInfo.SetupString;
            PlugInLoader.GmmConfiguration.SetOrUpdateValue("Startup", "DbConfigPrimary", setupString);
            PlugInLoader.GmmConfiguration.SetOrUpdateValue("Startup", "DbConfigSecundary", string.Empty);
            PlugInLoader.GmmConfiguration.WriteConfigFile();
            this.InitGMM();
          }
        }
        TranslationRulesManager.Instance = (TranslationRulesManager) null;
        this.gmmInitLogger.Info("GMM started.");
      }
      catch (Exception ex)
      {
        int num = (int) System.Windows.Forms.MessageBox.Show(string.Format("Error message: {0} Exception is: '{1}'. Stack trace: {2}", (object) ex.Message, (object) ex.GetBaseException().GetType().ToString(), (object) ex.StackTrace), "Internal fatal error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly);
        PlugInLoader.StartRestrictedMode = true;
        return false;
      }
      try
      {
        DbBasis.PrimaryDB.ZRDataAdapter("SELECT UserId FROM SoftwareUsers WHERE 1=0", DbBasis.PrimaryDB.GetDbConnection()).Fill((DataTable) new Schema.SoftwareUsersDataTable());
      }
      catch (Exception ex)
      {
        PlugInLoader.StartRestrictedMode = true;
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Database error");
        stringBuilder.Append(ex.ToString());
        int num = (int) GMM_MessageBox.ShowMessage("Plugin GMM", stringBuilder.ToString(), true);
        return false;
      }
      PlugInLoader.UserName = PlugInLoader.GmmConfiguration.GetValue("GMM", "LastUser");
      PlugInLoader.StartComponent = PlugInLoader.GmmConfiguration.GetValue("GMM", "StartComponent");
      if (PlugInLoader.StartComponent.Equals("") || PlugInLoader.StartComponent.Equals("StartWindow"))
        PlugInLoader.StartComponent = "GMM";
      PlugInLoader.AutoLoginPrepared = UserManager.CheckAutologin(PlugInLoader.GmmConfiguration.GetValue("GMM", "Autologin"), PlugInLoader.UserName);
      PlugInLoader.AutoStartEnabled = PlugInLoader.GmmConfiguration.GetValue("GMM", "AutoStart") == true.ToString();
      PlugInLoader.QuestionBeforExit = PlugInLoader.GmmConfiguration.GetValue("GMM", "QuestionBeforExit") == true.ToString();
      return true;
    }

    public static string GenerateHardwareKey()
    {
      string str1 = UserRights.LocalInfoScrable(0);
      string str2 = UserRights.LocalInfoScrable(1);
      string str3 = UserRights.LocalInfoScrable(2);
      string InputString = "" + ParameterService.GetCharacterCode((int) UserRights.LICENSE_CODE_VERSION_PC).ToString() + str1 + str2 + str3;
      return UserRights.GetSeparatedString(InputString + UserRights.GetStringCS(InputString));
    }

    private void RenameSetupDatabaseAndUseIt()
    {
      this.RenameSetupDatabaseAndUseIt(Path.Combine(SystemValues.DatabasePath, "MeterDB.db3"));
    }

    private void RenameSetupDatabaseAndUseIt(string pathToNewDatabase)
    {
      if (string.IsNullOrEmpty(pathToNewDatabase))
        throw new ArgumentNullException("Input parameter 'pathToNewDatabase' can not be null!");
      this.gmmInitLogger.Info("Rename setup database and use it. Path to new database: " + pathToNewDatabase);
      PlugInLoader.GmmConfiguration.SetOrUpdateValue("MainDataBase", "GMM_DBType", "SQLITE");
      PlugInLoader.GmmConfiguration.SetOrUpdateValue("MainDataBase", "GMM_DateiName", pathToNewDatabase);
      PlugInLoader.GmmConfiguration.SetOrUpdateValue("MainDataBase", "GMM_Password", Datenbankverbindung.scrable("meterdbpass"));
      PlugInLoader.GmmConfiguration.SetOrUpdateValue("MainDataBase", "GMM_DataBase", string.Format("Data Source={0};UTF8Encoding=True;Password=meterdbpass;journal mode=wal;synchronous=off;", (object) pathToNewDatabase).Replace("meterdbpass", "***PASSWORD***"));
      PlugInLoader.GmmConfiguration.WriteConfigFile();
      string sourceFileName = Path.Combine(SystemValues.DatabasePath, "MeterDB.set");
      File.Copy(sourceFileName, sourceFileName + ".copy", true);
      if (File.Exists(pathToNewDatabase))
        File.Delete(pathToNewDatabase);
      File.Move(sourceFileName, pathToNewDatabase);
      DatabaseUpgradeManager.DeleteDatabaseInfoFile();
    }

    private bool InitDatabase()
    {
      Datenbankverbindung.MainDBAccess = (MeterDBAccess) null;
      Datenbankverbindung.SecDBAccess = (MeterDBAccess) null;
      DbBasis.PrimaryDB = (DbBasis) null;
      DbBasis.SecondaryDB = (DbBasis) null;
      this.gmmInitLogger.Info("Initializing primary database...");
      try
      {
        PlugInLoader.PrimaryDataBase = new Datenbankverbindung("MainDataBase", PlugInLoader.GmmConfiguration);
        Datenbankverbindung.MainDBAccess = new MeterDBAccess(PlugInLoader.PrimaryDataBase.ConnectionInfo, out DbBasis.PrimaryDB);
        Datenbankverbindung.MainDBAccess.myDB.BaseDbConnection.ConnectDatabase();
        GMM_Help.TheHelp = new GMM_Help("en");
      }
      catch (Exception ex)
      {
        PlugInLoader.StartRestrictedMode = true;
        ExceptionViewer.Show(ex, "Primary data base open error");
        return false;
      }
      return true;
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

    private void SetGUILanguage()
    {
      if (GmmInit.SupportedCulturesList == null)
      {
        CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
        GmmInit.SupportedCulturesList = new List<CultureInfo>();
        foreach (CultureInfo cultureInfo in cultures)
        {
          if (cultureInfo.IsNeutralCulture && !cultureInfo.Name.Contains("-"))
            GmmInit.SupportedCulturesList.Add(cultureInfo);
        }
        GmmInit.SupportedCulturesList.Sort((Comparison<CultureInfo>) ((s1, s2) => s1.DisplayName.CompareTo(s2.DisplayName)));
      }
      CultureInfo cultureInfo1 = (CultureInfo) null;
      string DefLanguage = PlugInLoader.GmmConfiguration.GetValue("GMM", "Language");
      if (DefLanguage.Length == 2)
        cultureInfo1 = GmmInit.SupportedCulturesList.Find((Predicate<CultureInfo>) (item => item.TwoLetterISOLanguageName == DefLanguage));
      else if (DefLanguage.Length == 3)
        cultureInfo1 = GmmInit.SupportedCulturesList.Find((Predicate<CultureInfo>) (item => item.ThreeLetterISOLanguageName == DefLanguage));
      if (cultureInfo1 == null)
      {
        string ThreadLanguage = Thread.CurrentThread.CurrentUICulture.ThreeLetterISOLanguageName;
        cultureInfo1 = GmmInit.SupportedCulturesList.Find((Predicate<CultureInfo>) (item => item.ThreeLetterISOLanguageName == ThreadLanguage)) ?? GmmInit.SupportedCulturesList.Find((Predicate<CultureInfo>) (item => item.ThreeLetterISOLanguageName == "eng"));
      }
      if (cultureInfo1 != null)
      {
        Thread.CurrentThread.CurrentUICulture = cultureInfo1;
        FrameworkElement.LanguageProperty.OverrideMetadata(typeof (FrameworkElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) XmlLanguage.GetLanguage(cultureInfo1.IetfLanguageTag)));
      }
      else
        cultureInfo1 = Thread.CurrentThread.CurrentUICulture;
      this.gmmInitLogger.Info("Using language: " + cultureInfo1.DisplayName);
    }
  }
}
