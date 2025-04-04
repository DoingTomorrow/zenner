// Decompiled with JetBrains decompiler
// Type: StartupLib.StartupManager
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using CommonWPF;
using GmmDbLib;
using NLog;
using PlugInLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using ZR_ClassLibrary;

#nullable disable
namespace StartupLib
{
  public class StartupManager : INotifyPropertyChanged
  {
    private static Logger logger = LogManager.GetLogger(nameof (StartupManager));
    private static StartupManager staticStartupManagerViewModel;
    public static string alternateLocation = "GMM";
    public static bool RestartGMM = false;
    public const string DefaultSetupFileName = "Defaults.gmm";
    public const string OldStartupFileName = "StartUp.gmms";
    public const string StartupFileName = "GmmStartUp.xml";
    public string GmmApplicationPath;
    public string GmmDataPath;
    public string GmmStartUpPath;
    public string StartUpFilePath;
    public string preDefinedStartupFile;
    public StartupInfoList startupInfoList;

    public static StartupManager StaticInstance
    {
      get
      {
        if (StartupManager.staticStartupManagerViewModel == null)
          StartupManager.staticStartupManagerViewModel = new StartupManager();
        return StartupManager.staticStartupManagerViewModel;
      }
    }

    public StartupManager()
    {
      this.GmmApplicationPath = Path.GetDirectoryName(Application.ExecutablePath);
      this.GmmDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Path.Combine("ZENNER", StartupManager.alternateLocation));
      this.GmmStartUpPath = Path.Combine(this.GmmDataPath, "Settings");
      this.StartUpFilePath = Path.Combine(this.GmmStartUpPath, "GmmStartUp.xml");
      Process currentProcess = Process.GetCurrentProcess();
      string processName = currentProcess.ProcessName;
      this.preDefinedStartupFile = currentProcess.StartInfo.Arguments;
      List<Process> processList = new List<Process>();
      foreach (Process process in Process.GetProcessesByName("PGMM"))
        processList.Add(process);
      foreach (Process process in Process.GetProcessesByName("PGMM.vshost"))
        processList.Add(process);
      if (processList.Count <= 1 || !(this.preDefinedStartupFile == ""))
        ;
    }

    private void NotifiyPropertyChanged(string property)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(property));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public bool LoadOrCreateStartup()
    {
      bool startup = true;
      bool flag = false;
      this.startupInfoList = (StartupInfoList) null;
      if (File.Exists(this.StartUpFilePath))
        this.LoadStartupFile(this.StartUpFilePath);
      if (this.startupInfoList == null)
      {
        this.startupInfoList = new StartupInfoList();
        string str = Path.Combine(this.GmmDataPath, "StartUp.gmms");
        if (File.Exists(str))
          this.LoadOldStartupFile(str);
        flag = true;
      }
      StartupInfo startupInfo = this.GetStartupInfo();
      if (startupInfo == null)
      {
        startupInfo = new StartupInfo();
        startupInfo.InstallationFile = this.GmmApplicationPath;
        startupInfo.StartupFile = "";
        this.startupInfoList.startupInfo.Add(startupInfo);
        flag = true;
      }
      if (string.IsNullOrEmpty(startupInfo.StartupFile) || !File.Exists(startupInfo.StartupFile))
      {
        DefineNewSetupFile defineNewSetupFile = new DefineNewSetupFile(this);
        if (defineNewSetupFile.ShowWindowRequired && !defineNewSetupFile.ShowDialog().Value)
          return false;
        startupInfo.StartupFile = defineNewSetupFile.NewSetupFilePath;
        startupInfo.LicenseFile = defineNewSetupFile.NewLicenseFilePath;
        startup = true;
        flag = true;
      }
      if (flag)
        this.SaveStartupFile(this.StartUpFilePath);
      return startup;
    }

    public bool SaveConfigurationFile(string sourceFile, string destinationFile)
    {
      if (this.startupInfoList.startupInfo.Exists((Predicate<StartupInfo>) (x => x.StartupFile.Equals(destinationFile))))
        return false;
      if (!sourceFile.Equals("") && sourceFile != destinationFile)
        File.Copy(sourceFile, destinationFile, true);
      this.ChangeStartupFile(destinationFile);
      return this.SaveStartupFile(this.StartUpFilePath);
    }

    private bool LoadStartupFile(string StartupFileName)
    {
      if (string.IsNullOrEmpty(StartupFileName))
        throw new ArgumentNullException(nameof (StartupFileName), "Can not load the 'start up' file.");
      try
      {
        using (StringReader stringReader = new StringReader(File.ReadAllText(StartupFileName)))
          this.startupInfoList = (StartupInfoList) new XmlSerializer(typeof (StartupInfoList)).Deserialize((TextReader) stringReader);
        return true;
      }
      catch (Exception ex)
      {
        StartupManager.logger.ErrorException("Can not load the 'start up' file. " + ex.Message, ex);
        return false;
      }
    }

    public bool SaveStartupFile() => this.SaveStartupFile(this.StartUpFilePath);

    private bool SaveStartupFile(string StartupFileName)
    {
      if (string.IsNullOrEmpty(StartupFileName))
        throw new ArgumentNullException(nameof (StartupFileName), "Can not save the 'start up' file.");
      try
      {
        using (StringWriter stringWriter = new StringWriter())
        {
          new XmlSerializer(typeof (StartupInfoList)).Serialize((TextWriter) stringWriter, (object) this.startupInfoList);
          File.WriteAllText(StartupFileName, stringWriter.ToString(), Encoding.UTF8);
        }
        return true;
      }
      catch (Exception ex)
      {
        StartupManager.logger.ErrorException("Can not save the 'start up' file. " + ex.Message, ex);
        return false;
      }
    }

    public bool ChangeLicenseFile(string newLicenseFile)
    {
      if (string.IsNullOrEmpty(newLicenseFile))
        throw new ArgumentNullException(nameof (newLicenseFile), "Can not change the license file!");
      if (!File.Exists(newLicenseFile))
        throw new FileNotFoundException("Can not change the license file!", newLicenseFile);
      StartupInfo startupInfo = this.GetStartupInfo();
      if (startupInfo == null)
        throw new Exception("Can not change the license file! The 'startupInfo' object is not available for this installation path.");
      startupInfo.LicenseFile = newLicenseFile;
      return this.SaveStartupFile(this.StartUpFilePath);
    }

    public bool ChangeStartupFile(string newStartupFile)
    {
      if (string.IsNullOrEmpty(newStartupFile))
        throw new ArgumentNullException(nameof (newStartupFile), "Can not change the startup file!");
      StartupInfo startupInfo = this.GetStartupInfo();
      if (startupInfo == null)
        throw new Exception("Can not change the startup file! The 'startupInfo' object is not available for this installation path.");
      startupInfo.StartupFile = newStartupFile;
      return this.SaveStartupFile(this.StartUpFilePath);
    }

    public bool ChangeLibrariesFile(string newLibrariesFile)
    {
      if (string.IsNullOrEmpty(newLibrariesFile))
        throw new ArgumentNullException(nameof (newLibrariesFile), "Can not change the libraries file!");
      StartupInfo startupInfo = this.GetStartupInfo();
      if (startupInfo == null)
        throw new Exception("Can not change the libraries file! The 'startupInfo' object is not available for this installation path.");
      startupInfo.LibrariesFile = newLibrariesFile;
      return this.SaveStartupFile(this.StartUpFilePath);
    }

    public bool ChangeInstallationName(string newInstallationName)
    {
      StartupInfo startupInfo = this.GetStartupInfo();
      if (startupInfo == null)
        throw new Exception("Can not change the libraries file! The 'startupInfo' object is not available for this installation path.");
      startupInfo.InstallationName = newInstallationName;
      return this.SaveStartupFile(this.StartUpFilePath);
    }

    private void LoadOldStartupFile(string StartupFileName)
    {
      using (StreamReader streamReader = new StreamReader(StartupFileName))
      {
        string str1 = (string) null;
        string str2;
        while ((str2 = streamReader.ReadLine()) != null)
        {
          string str3 = str2.Trim();
          if (str1 == null)
          {
            str1 = str3;
          }
          else
          {
            if (str3.Length < 2 || str3[0] != '#')
              break;
            this.startupInfoList.startupInfo.Add(new StartupInfo()
            {
              InstallationFile = str3.Substring(1),
              StartupFile = str1
            });
            str1 = (string) null;
          }
        }
      }
    }

    public StartupInfo GetStartupInfo()
    {
      if (this.startupInfoList == null)
        throw new ArgumentNullException("startupInfoList", "Can not get the path to the libraries!");
      if (string.IsNullOrEmpty(this.GmmApplicationPath))
        throw new ArgumentNullException("GmmApplicationPat", "Can not get the path to the libraries!");
      IEnumerable<StartupInfo> source = this.startupInfoList.startupInfo.Where<StartupInfo>((Func<StartupInfo, bool>) (a => a.InstallationFile.ToLower() == this.GmmApplicationPath.ToLower()));
      if (source == null || source.Count<StartupInfo>() == 0)
      {
        StartupManager.logger.Warn("No StartupInfo exist for this installation path: " + this.GmmApplicationPath);
        return (StartupInfo) null;
      }
      if (source.Count<StartupInfo>() > 1)
        StartupManager.logger.Error("It exist more as one 'StartupInfo' for this installation path: " + this.GmmApplicationPath);
      return source.First<StartupInfo>();
    }

    public string GenerateUniqueFileName(string fileName, string extensionName)
    {
      string path = "";
      bool flag = false;
      int num = 0;
      while (!flag)
      {
        path = Path.Combine(this.GmmStartUpPath, fileName + num.ToString() + "." + extensionName);
        if (!File.Exists(path))
          flag = true;
        ++num;
      }
      return path;
    }

    public string CopyLicenseFile(string LicenseFileName)
    {
      string destFileName = Path.Combine(this.GmmStartUpPath, Path.GetFileName(LicenseFileName));
      if (destFileName != LicenseFileName)
        File.Copy(LicenseFileName, destFileName, true);
      return destFileName;
    }

    public static DbBasis Database => DbBasis.PrimaryDB;

    public static LicenseInfo License => PlugInLib.LicenseManager.CurrentLicense;

    public static void Initialize(string licenseFileName, DbBasis database)
    {
      if (string.IsNullOrEmpty(licenseFileName))
        throw new ArgumentNullException(nameof (licenseFileName), "Can not initialize the GMM! Missing license file.");
      if (database == null)
        throw new ArgumentNullException(nameof (database), "Can not initialize the GMM! Missing database.");
      if (!Util.IsNet45OrNewer())
        throw new NotSupportedException("GMM driver needs .NET Framework 4.5 or newer! Please install it.");
      StartupManager.Dispose();
      try
      {
        DbBasis.PrimaryDB = database;
        PlugInLib.LicenseManager.VerifyLicense(licenseFileName);
      }
      catch (Exception ex)
      {
        StartupManager.Dispose();
        throw ex;
      }
    }

    public static void Dispose()
    {
      PlugInLoader.Dispose();
      PlugInLib.LicenseManager.Dispose();
      UserManager.Dispose();
      ZR_Component.Dispose();
      SystemValues.Dispose();
      Datenbankverbindung.Dispose();
      ZR_ClassLibMessages.DeRegisterThreadErrorMsgList();
      GMM_Help.TheHelp = (GMM_Help) null;
      UserRights.GlobalUserRights = (UserRights) null;
      DbBasis.PrimaryDB = (DbBasis) null;
      DbBasis.SecondaryDB = (DbBasis) null;
      ZR_ClassLibMessages.ThreadErrorMsgLists = (SortedList<int, ZR_ClassLibMessages>) null;
      TranslationRulesManager.Instance = (TranslationRulesManager) null;
      StartupManager.staticStartupManagerViewModel = (StartupManager) null;
    }

    public static void ShowPlugin(string pluginName)
    {
      if (string.IsNullOrEmpty(pluginName))
        return;
      PlugInLoader.GetPlugIn(pluginName)?.ShowMainWindow();
    }

    public static bool CheckLicense(StartupInfo startupInfo)
    {
      bool flag = startupInfo != null && !string.IsNullOrEmpty(startupInfo.LicenseFile) && File.Exists(startupInfo.LicenseFile);
      StartupManager.logger.Trace("StartupInfo loaded");
      if (!flag)
        return StartupManager.ChangeLicenseAndReturnValid((string) null);
      try
      {
        PlugInLib.LicenseManager.VerifyLicense(startupInfo.LicenseFile);
        StartupManager.logger.Trace("LicenseFile veryfied");
        return true;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      return StartupManager.ChangeLicenseAndReturnValid(startupInfo.LicenseFile);
    }

    public static bool ChangeLicenseAndReturnValid(string licenseFile)
    {
      bool? nullable = new LicenseWindow(licenseFile).ShowDialog();
      bool flag1 = true;
      bool flag2 = nullable.GetValueOrDefault() == flag1 & nullable.HasValue;
      if (flag2)
      {
        StartupInfo startupInfo = StartupManager.StaticInstance.GetStartupInfo();
        if (startupInfo != null)
        {
          PlugInLoader.GmmConfiguration.WriteConfigFile(startupInfo.StartupFile);
          PlugInLoader.GmmConfiguration.ReadConfigForPlugInGmm(startupInfo.StartupFile);
          StartupManager.RestartGMM = true;
        }
      }
      return flag2;
    }

    private enum StartupTableColumns
    {
      Installation,
      StartupFile,
      LicenseFile,
    }
  }
}
