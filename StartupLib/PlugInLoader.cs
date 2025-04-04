// Decompiled with JetBrains decompiler
// Type: StartupLib.PlugInLoader
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using GmmDbLib;
using NLog;
using PlugInLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace StartupLib
{
  public static class PlugInLoader
  {
    private static Logger PlugInLoaderLogger = LogManager.GetLogger(nameof (PlugInLoader));
    public static AvailableLibraries availableLibraries = new AvailableLibraries();
    public static List<LoadedPlugin> loadedPlugins = new List<LoadedPlugin>();
    public static string UserName = "";
    public static string UserPassword = "";
    public static bool StartRestrictedMode;
    private static string startComponent = string.Empty;
    public static bool AutoLoginPrepared = true;
    public static bool QuestionBeforExit = false;
    public static bool AutoStartEnabled = true;
    public static Datenbankverbindung PrimaryDataBase;
    public static Datenbankverbindung SecundaryDataBase;
    public static GMMConfig GmmConfiguration;
    public static ConfigList ConfigListStatic;
    public static bool GMM_ComponentTreeAvailable = false;

    static PlugInLoader()
    {
      GMMSettings sideIrDaMinoConnect = GMMSettings.Default_MBusP2P_IrCombiHeadRoundSide_IrDa_MinoConnect;
      SortedList<string, string> settingsStringString = sideIrDaMinoConnect.AsyncComSettings_string_string;
      settingsStringString.Add("ConnectionProfileID", "17");
      foreach (KeyValuePair<DeviceCollectorSettings, object> collectorSetting in sideIrDaMinoConnect.DeviceCollectorSettings)
        settingsStringString.Add(collectorSetting.Key.ToString(), collectorSetting.Value.ToString());
      PlugInLoader.ConfigListStatic = new ConfigList(settingsStringString);
    }

    public static string StartComponent
    {
      get => PlugInLoader.startComponent;
      set
      {
        PlugInLoader.startComponent = value;
        PlugInLoader.GmmConfiguration.SetOrUpdateValue("GMM", nameof (StartComponent), PlugInLoader.startComponent);
      }
    }

    public static bool IsPluginLoaderInitialised()
    {
      return PlugInLoader.availableLibraries != null && PlugInLoader.availableLibraries.LibrariesInfo.Count > 0;
    }

    public static bool LoadLibrariesInformation(string fileName)
    {
      PlugInLoader.PlugInLoaderLogger.Trace("Load libraries information from: " + fileName);
      if (string.IsNullOrEmpty(fileName))
        return false;
      if (!File.Exists(fileName))
        return false;
      try
      {
        using (StringReader stringReader = new StringReader(File.ReadAllText(fileName)))
        {
          PlugInLoader.availableLibraries = (AvailableLibraries) new XmlSerializer(typeof (AvailableLibraries)).Deserialize((TextReader) stringReader);
          if (PlugInLoader.availableLibraries.LibraryRights == null)
            return false;
          if (PlugInLoader.PlugInLoaderLogger.IsTraceEnabled && PlugInLoader.availableLibraries.LibrariesInfo != null)
            PlugInLoader.TraceLibraryInfos();
        }
        string[] files = Directory.GetFiles(SystemValues.AppPath, "*.dll");
        if (PlugInLoader.availableLibraries.DllsInFolder != files.Length)
          return false;
      }
      catch (Exception ex)
      {
        PlugInLoader.PlugInLoaderLogger.ErrorException(ex.Message, ex);
        return false;
      }
      int count = PlugInLoader.availableLibraries.LibrariesInfo != null ? PlugInLoader.availableLibraries.LibrariesInfo.Count : 0;
      PlugInLoader.PlugInLoaderLogger.Trace("Loaded infos about " + count.ToString() + " plugin(s)");
      return true;
    }

    private static void TraceLibraryInfos()
    {
      foreach (LibraryInfo libraryInfo in PlugInLoader.availableLibraries.LibrariesInfo)
        PlugInLoader.PlugInLoaderLogger.Trace("Load plugin info: " + libraryInfo.FileName);
    }

    public static void SaveLibrariesInformation(string FileName, AvailableLibraries libraryInfo)
    {
      libraryInfo.FileCreateDate = DateTime.Now;
      using (StringWriter stringWriter = new StringWriter())
      {
        new XmlSerializer(typeof (AvailableLibraries)).Serialize((TextWriter) stringWriter, (object) libraryInfo);
        File.WriteAllText(FileName, stringWriter.ToString(), Encoding.UTF8);
      }
    }

    public static void ScanLibraries(string scanReason, IProgress<ScanInfo> progress)
    {
      PlugInLoader.availableLibraries = (AvailableLibraries) null;
      int length = Directory.GetFiles(SystemValues.AppPath, "*.dll").Length;
      PlugInLoader.PlugInLoaderLogger.Trace(nameof (ScanLibraries));
      SortedList<string, PlugInAssamblyInfo> sortedList = new SortedList<string, PlugInAssamblyInfo>();
      PlugInManager plugInManager = new PlugInManager();
      if (plugInManager != null)
        sortedList = plugInManager.GetPlugInList(Application.StartupPath);
      AvailableLibraries libraryInfo1 = new AvailableLibraries();
      libraryInfo1.FileCreationReason = scanReason;
      foreach (PlugInAssamblyInfo plugInAssamblyInfo in (IEnumerable<PlugInAssamblyInfo>) sortedList.Values)
      {
        LibraryInfo libraryInfo2 = new LibraryInfo();
        libraryInfo2.FileName = plugInAssamblyInfo.FileName;
        libraryInfo2.Name = plugInAssamblyInfo.GmmName;
        libraryInfo2.IsPlugin = plugInAssamblyInfo.type.Name == "PlugInAnchor";
        libraryInfo2.PluginPath = plugInAssamblyInfo.plugInPath;
        if (libraryInfo2.IsPlugin)
        {
          libraryInfo1.LibrariesInfo.Add(libraryInfo2);
          PlugInLoader.PlugInLoaderLogger.Trace("Found a plugin: " + libraryInfo2.FileName);
        }
      }
      try
      {
        libraryInfo1.LibraryRights = plugInManager.GetRightsList(out SortedList<string, string> _);
      }
      catch (Exception ex)
      {
        PlugInLoader.PlugInLoaderLogger.Error(ex.Message);
      }
      if (StartupManager.StaticInstance.startupInfoList != null)
      {
        StartupInfo startupInfo = StartupManager.StaticInstance.GetStartupInfo();
        string str = startupInfo.LibrariesFile;
        if (startupInfo == null || string.IsNullOrEmpty(startupInfo.LibrariesFile) || File.Exists(startupInfo.LibrariesFile))
        {
          PlugInLoader.PlugInLoaderLogger.Info("Generate new 'LibrariesFile.xml' file");
          str = StartupManager.StaticInstance.GenerateUniqueFileName("LibrariesFile", "XML");
          PlugInLoader.PlugInLoaderLogger.Info("New path is: " + str);
          StartupManager.StaticInstance.ChangeLibrariesFile(str);
        }
        libraryInfo1.DllsInFolder = length;
        PlugInLoader.SaveLibrariesInformation(str, libraryInfo1);
      }
      PlugInLoader.availableLibraries = libraryInfo1;
    }

    public static void DeleteNotUsedLibraryFiles()
    {
      try
      {
        if (StartupManager.StaticInstance.startupInfoList == null || StartupManager.StaticInstance.startupInfoList.startupInfo == null)
          return;
        SortedList<int, bool> sortedList = new SortedList<int, bool>();
        foreach (StartupInfo startupInfo in StartupManager.StaticInstance.startupInfoList.startupInfo)
        {
          int result;
          if (startupInfo.LibrariesFile != null && int.TryParse(Path.GetFileNameWithoutExtension(startupInfo.LibrariesFile).Replace("LibrariesFile", ""), out result))
            sortedList.Add(result, true);
        }
        string[] files = Directory.GetFiles(StartupManager.StaticInstance.GmmStartUpPath, "LibrariesFile*.XML");
        if (files != null)
        {
          foreach (string path in files)
          {
            int result;
            if (int.TryParse(Path.GetFileNameWithoutExtension(path).Replace("LibrariesFile", ""), out result) && !sortedList.ContainsKey(result))
              sortedList.Add(result, false);
          }
        }
        for (int index = 0; index < sortedList.Count; ++index)
        {
          if (!sortedList.Values[index])
            File.Delete(Path.Combine(StartupManager.StaticInstance.GmmStartUpPath, "LibrariesFile" + sortedList.Keys[index].ToString() + ".XML"));
        }
      }
      catch
      {
      }
    }

    public static bool IsWindowEnabled(string plugInOrSpecialWindowName)
    {
      return plugInOrSpecialWindowName == "GMM" ? PlugInLoader.GMM_ComponentTreeAvailable : PlugInLoader.availableLibraries.LibrariesInfo.Where<LibraryInfo>((Func<LibraryInfo, bool>) (x => x.Name.Equals(plugInOrSpecialWindowName))).Count<LibraryInfo>() > 0 && UserManager.CheckPermission("Plugin\\" + plugInOrSpecialWindowName);
    }

    public static bool IsPluginLoaded(string plugInName)
    {
      return PlugInLoader.loadedPlugins != null && PlugInLoader.loadedPlugins.FirstOrDefault<LoadedPlugin>((Func<LoadedPlugin, bool>) (a => a.Name == plugInName)) != null;
    }

    public static void CloseAllPluginComPorts()
    {
      try
      {
        if (PlugInLoader.IsPluginLoaded("AsyncCom"))
        {
          object obj = PlugInLoader.GetPlugIn("AsyncCom").GetPluginInfo().Interface;
          obj.GetType().GetMethod("Close").Invoke(obj, (object[]) null);
        }
        if (!PlugInLoader.IsPluginLoaded("CommunicationPort"))
          return;
        object obj1 = PlugInLoader.GetPlugIn("CommunicationPort").GetPluginInfo().Interface;
        obj1.GetType().GetMethod("Close").Invoke(obj1, (object[]) null);
      }
      catch
      {
      }
    }

    public static GmmPlugIn GetPlugIn(string plugInName)
    {
      if (PlugInLoader.loadedPlugins == null)
        return (GmmPlugIn) null;
      GmmPlugIn plugIn = (GmmPlugIn) null;
      try
      {
        IEnumerable<LoadedPlugin> source1 = PlugInLoader.loadedPlugins.Where<LoadedPlugin>((Func<LoadedPlugin, bool>) (a => a.Name.Equals(plugInName)));
        if (source1.Count<LoadedPlugin>() == 0)
        {
          PlugInLoader.PlugInLoaderLogger.Trace("GetPlugIn(not preloaded): " + plugInName);
          IEnumerable<LibraryInfo> source2 = PlugInLoader.availableLibraries.LibrariesInfo.Where<LibraryInfo>((Func<LibraryInfo, bool>) (x => x.Name.Equals(plugInName)));
          if (source2.Count<LibraryInfo>() > 0)
          {
            Assembly assembly = Assembly.LoadFrom(source2.First<LibraryInfo>().FileName);
            string typeName = ((IEnumerable<Type>) assembly.GetTypes()).Where<Type>((Func<Type, bool>) (x => x.GetInterface("IGmmPlugIn") != (Type) null)).First<Type>().ToString();
            plugIn = (GmmPlugIn) assembly.CreateInstance(typeName);
          }
          else
          {
            PlugInLoader.PlugInLoaderLogger.Error("LibrariesInfo not available: " + plugInName);
            PlugInLoader.TraceLibraryInfos();
          }
          if (plugIn != null)
          {
            PlugInLoader.PlugInLoaderLogger.Trace("Add to plugin list: " + plugInName);
            PlugInLoader.loadedPlugins.Add(new LoadedPlugin()
            {
              Name = plugInName,
              gmmPlugIn = plugIn
            });
            if (Enum.IsDefined(typeof (GMM_Components), (object) plugInName))
            {
              GMM_Components key = (GMM_Components) Enum.Parse(typeof (GMM_Components), plugInName, true);
              ZR_Component.CommonGmmInterface.LoadedComponentsList.Add(key, plugIn.GetPluginInfo().Interface);
            }
          }
        }
        else
        {
          PlugInLoader.PlugInLoaderLogger.Trace("GetPlugIn(not preloaded): " + plugInName + " Count:" + source1.Count<LoadedPlugin>().ToString());
          return source1.First<LoadedPlugin>().gmmPlugIn;
        }
      }
      catch (ReflectionTypeLoadException ex)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Exception on load plugin " + plugInName);
        PlugInLoader.PlugInLoaderLogger.Error(stringBuilder.ToString());
        PlugInLoader.PlugInLoaderLogger.Error(ex.ToString());
        if (ex.LoaderExceptions != null)
        {
          foreach (Exception loaderException in ex.LoaderExceptions)
          {
            PlugInLoader.PlugInLoaderLogger.Error(loaderException.ToString());
            stringBuilder.AppendLine(loaderException.ToString());
          }
        }
        throw new Exception(stringBuilder.ToString(), (Exception) ex);
      }
      catch (Exception ex)
      {
        string message = "Exception on load plugin " + plugInName;
        PlugInLoader.PlugInLoaderLogger.Error(message);
        PlugInLoader.PlugInLoaderLogger.Error(ex.ToString());
        throw new Exception(message, ex);
      }
      if (plugIn is IReadoutConfig)
        ((IReadoutConfig) plugIn).SetReadoutConfiguration(PlugInLoader.ConfigListStatic);
      return plugIn;
    }

    public static ConfigList LoadConfigList(string name = "")
    {
      if (name == null)
        name = string.Empty;
      string strNamespace = "ReadoutConfig_" + name;
      SortedList<string, string> values = PlugInLoader.GmmConfiguration.GetValues(strNamespace);
      return values == null || values.Count == 0 ? (ConfigList) null : new ConfigList(values);
    }

    public static void SaveStaticConfigList()
    {
      PlugInLoader.SaveConfigList(PlugInLoader.ConfigListStatic);
    }

    public static void SaveConfigList(ConfigList configList, string name = "")
    {
      if (configList == null)
        throw new ArgumentNullException(nameof (configList));
      if (name == null)
        name = string.Empty;
      string strNamespace = "ReadoutConfig_" + name;
      PlugInLoader.GmmConfiguration.RemoveAllVariablesFromNamespace(strNamespace);
      foreach (KeyValuePair<string, string> config in configList)
        PlugInLoader.GmmConfiguration.SetOrUpdateValue(strNamespace, config.Key, config.Value);
    }

    public static void GarantComponentLoaded(GMM_Components TheComponent)
    {
      string plugInName = TheComponent.ToString();
      PlugInLoader.PlugInLoaderLogger.Trace("GarantComponentLoaded: " + plugInName);
      PlugInLoader.GetPlugIn(plugInName);
    }

    public static void Dispose()
    {
      if (PlugInLoader.loadedPlugins == null)
        return;
      foreach (LoadedPlugin loadedPlugin in PlugInLoader.loadedPlugins)
        loadedPlugin.gmmPlugIn.Dispose();
      if (PlugInLoader.GmmConfiguration != null && !PlugInLoader.GmmConfiguration.WriteConfigFile())
      {
        int num = (int) GMM_MessageBox.ShowMessage("PGMM", "Can not write configuration file.", true);
      }
      PlugInLoader.availableLibraries = (AvailableLibraries) null;
      PlugInLoader.loadedPlugins = (List<LoadedPlugin>) null;
      PlugInLoader.UserName = (string) null;
      PlugInLoader.UserPassword = (string) null;
      PlugInLoader.StartRestrictedMode = false;
      PlugInLoader.startComponent = (string) null;
      PlugInLoader.AutoLoginPrepared = false;
      PlugInLoader.QuestionBeforExit = false;
      PlugInLoader.AutoStartEnabled = false;
      PlugInLoader.PrimaryDataBase = (Datenbankverbindung) null;
      PlugInLoader.SecundaryDataBase = (Datenbankverbindung) null;
      PlugInLoader.GmmConfiguration = (GMMConfig) null;
      PlugInLoader.ConfigListStatic = (ConfigList) null;
    }

    public static bool InitSecundaryDatabase()
    {
      if (Datenbankverbindung.SecDBAccess != null)
        return true;
      PlugInLoader.PlugInLoaderLogger.Info("Initializing secundary database...");
      try
      {
        PlugInLoader.SecundaryDataBase = new Datenbankverbindung("SecDataBase", PlugInLoader.GmmConfiguration);
        Datenbankverbindung.SecDBAccess = new MeterDBAccess(PlugInLoader.SecundaryDataBase.ConnectionInfo, out DbBasis.SecondaryDB);
        Datenbankverbindung.SecDBAccess.myDB.BaseDbConnection.ConnectDatabase();
      }
      catch (Exception ex)
      {
        PlugInLoader.PlugInLoaderLogger.Info("Error on database initialization" + Environment.NewLine + ex.ToString());
        PlugInLoader.SecundaryDataBase = (Datenbankverbindung) null;
        Datenbankverbindung.SecDBAccess = (MeterDBAccess) null;
        return false;
      }
      return true;
    }

    public static IWindowFunctions CreateGmmComponentAndGetIWindowFunctionsInterface(
      string pluginName)
    {
      Assembly assembly = Assembly.LoadFrom(pluginName + ".dll");
      Type type = ((IEnumerable<Type>) assembly.GetTypes()).Where<Type>((Func<Type, bool>) (x => x.GetInterface("IWindowFunctions") != (Type) null)).First<Type>((Func<Type, bool>) (x => true));
      return assembly.CreateInstance(type.ToString()) as IWindowFunctions;
    }
  }
}
