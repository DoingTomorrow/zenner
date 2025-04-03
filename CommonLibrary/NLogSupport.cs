// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.NLogSupport
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

#nullable disable
namespace ZENNER.CommonLibrary
{
  public static class NLogSupport
  {
    private const string ReportTokensUsed = "{ \"Report\":";
    private const string NLogStartToken = "\"NLOG\":[";
    private const string ReportEndToken = "]}";
    public static string InstallationFolder;

    public static string NLogOutputFilePath { get; private set; }

    public static string NLogConfigFilePath { get; private set; }

    public static string[] NLogSetupFilePaths { get; private set; }

    public static string[] NLogSetupFileNames { get; private set; }

    public static string ActiveConfigFileName { get; private set; }

    public static string TemporaryConfigFileName { get; private set; }

    static NLogSupport()
    {
      NLogSupport.NLogOutputFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ZENNER", "GMM", "LoggData", "NLogOutput.json");
      NLogSupport.InstallationFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      NLogSupport.NLogConfigFilePath = Path.Combine(NLogSupport.InstallationFolder, "NLog.config");
      NLogSupport.NLogSetupFilePaths = Directory.GetFiles(NLogSupport.InstallationFolder, "NLog_*.config");
      Array.Sort<string>(NLogSupport.NLogSetupFilePaths);
      NLogSupport.NLogSetupFileNames = new string[NLogSupport.NLogSetupFilePaths.Length];
      for (int index = 0; index < NLogSupport.NLogSetupFilePaths.Length; ++index)
        NLogSupport.NLogSetupFileNames[index] = Path.GetFileNameWithoutExtension(NLogSupport.NLogSetupFilePaths[index]);
      NLogSupport.GetActiveNLogConfigurationFileName();
    }

    public static string GetActiveNLogConfigurationFileName()
    {
      byte[] second = File.ReadAllBytes(NLogSupport.NLogConfigFilePath);
      for (int index = 0; index < NLogSupport.NLogSetupFilePaths.Length; ++index)
      {
        if (((IEnumerable<byte>) File.ReadAllBytes(NLogSupport.NLogSetupFilePaths[index])).SequenceEqual<byte>((IEnumerable<byte>) second))
        {
          NLogSupport.ActiveConfigFileName = NLogSupport.NLogSetupFileNames[index];
          NLogSupport.TemporaryConfigFileName = string.Empty;
          return NLogSupport.ActiveConfigFileName;
        }
      }
      return string.Empty;
    }

    public static string GetCurrentNLogSetup()
    {
      if (!string.IsNullOrEmpty(NLogSupport.TemporaryConfigFileName))
        return NLogSupport.TemporaryConfigFileName;
      return !string.IsNullOrEmpty(NLogSupport.ActiveConfigFileName) ? NLogSupport.ActiveConfigFileName : string.Empty;
    }

    public static bool ChangeDefaultToSetupFile(string fileName)
    {
      try
      {
        if (!string.IsNullOrEmpty(fileName))
        {
          if (fileName == NLogSupport.ActiveConfigFileName)
            return true;
          string str = Path.Combine(NLogSupport.InstallationFolder, fileName + ".config");
          if (File.Exists(str))
          {
            File.Copy(str, NLogSupport.NLogConfigFilePath, true);
            NLogSupport.ActiveConfigFileName = fileName;
            NLogSupport.TemporaryConfigFileName = string.Empty;
            return true;
          }
        }
      }
      catch
      {
      }
      return false;
    }

    public static bool ChangeTemporaryToSetupFile(string fileName)
    {
      try
      {
        if (!string.IsNullOrEmpty(fileName))
        {
          if (fileName == NLogSupport.ActiveConfigFileName || fileName == NLogSupport.TemporaryConfigFileName)
            return true;
          string str = Path.Combine(NLogSupport.InstallationFolder, fileName + ".config");
          if (File.Exists(str))
          {
            LogManager.LoadConfiguration(str);
            LogManager.ReconfigExistingLoggers();
            NLogSupport.ActiveConfigFileName = string.Empty;
            NLogSupport.TemporaryConfigFileName = fileName;
            return true;
          }
        }
      }
      catch
      {
      }
      return false;
    }

    public static void GarantLogFileAndLoggerRule(
      string filename,
      string logger,
      NLog.LogLevel minLevel)
    {
      string path2 = Path.Combine("ZENNER", "GMM");
      string path1 = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), path2), "LoggData");
      if (LogManager.Configuration == null)
        LogManager.Configuration = new LoggingConfiguration();
      FileTarget fileTarget = (FileTarget) null;
      foreach (Target allTarget in LogManager.Configuration.AllTargets)
      {
        if (allTarget is FileTarget)
        {
          fileTarget = (FileTarget) allTarget;
          break;
        }
      }
      if (fileTarget == null)
      {
        fileTarget = new FileTarget();
        fileTarget.FileName = (Layout) Path.Combine(path1, filename);
        fileTarget.KeepFileOpen = true;
        LogManager.Configuration.AddTarget("AutoGeneratedFile", (Target) fileTarget);
      }
      LoggingRule loggingRule1 = (LoggingRule) null;
      foreach (LoggingRule loggingRule2 in (IEnumerable<LoggingRule>) LogManager.Configuration.LoggingRules)
      {
        if (loggingRule2.NameMatches(logger) && loggingRule2.Targets.Contains((Target) fileTarget))
        {
          loggingRule1 = loggingRule2;
          break;
        }
      }
      if (loggingRule1 == null)
        LogManager.Configuration.LoggingRules.Add(new LoggingRule(logger, minLevel, (Target) fileTarget));
      LogManager.ReconfigExistingLoggers();
      LogManager.EnableLogging();
    }

    public static void TestBenchTraceActivate()
    {
      string[] strArray = new string[4]
      {
        "PlugInLoader",
        "LicenseManager",
        "Runtime",
        "OnlineTranslator"
      };
      try
      {
        if (File.Exists(NLogSupport.NLogOutputFilePath))
          File.Delete(NLogSupport.NLogOutputFilePath);
        LogManager.Configuration.LoggingRules.Clear();
        Target target1 = LogManager.Configuration.AllTargets.FirstOrDefault<Target>((Func<Target, bool>) (x => x.Name == "File"));
        Target target2 = LogManager.Configuration.AllTargets.FirstOrDefault<Target>((Func<Target, bool>) (x => x.Name == "UdpOutlet"));
        LoggingRule loggingRule1 = new LoggingRule()
        {
          LoggerNamePattern = "MiConPollingThread",
          Final = true
        };
        loggingRule1.EnableLoggingForLevel(NLog.LogLevel.Trace);
        LogManager.Configuration.LoggingRules.Add(loggingRule1);
        LoggingRule loggingRule2 = new LoggingRule()
        {
          LoggerNamePattern = "CommunicationByMinoConnect",
          Final = true
        };
        loggingRule2.EnableLoggingForLevel(NLog.LogLevel.Trace);
        LogManager.Configuration.LoggingRules.Add(loggingRule2);
        foreach (string str in strArray)
        {
          LoggingRule loggingRule3 = new LoggingRule()
          {
            LoggerNamePattern = str,
            Final = true
          };
          loggingRule3.EnableLoggingForLevels(NLog.LogLevel.Trace, NLog.LogLevel.Fatal);
          LogManager.Configuration.LoggingRules.Add(loggingRule3);
        }
        if (target1 != null)
          LogManager.Configuration.LoggingRules.Add(new LoggingRule("*", NLog.LogLevel.Trace, target1));
        if (target2 != null)
          LogManager.Configuration.LoggingRules.Add(new LoggingRule("*", NLog.LogLevel.Trace, target2));
        LogManager.Configuration.Reload();
      }
      catch
      {
      }
    }

    public static void TestBenchTraceStop(string filePath)
    {
      try
      {
        NLogSupport.SaveLogFile(filePath);
        LogManager.LoadConfiguration(NLogSupport.NLogConfigFilePath);
        LogManager.ReconfigExistingLoggers();
      }
      catch
      {
      }
    }

    public static bool IsLogFileAvailable() => File.Exists(NLogSupport.NLogOutputFilePath);

    public static void SaveLogFile(string filePath)
    {
      if (!File.Exists(NLogSupport.NLogOutputFilePath))
        return;
      File.Copy(NLogSupport.NLogOutputFilePath, filePath, true);
    }

    public static void DeleteNLogOutputFile()
    {
      if (!File.Exists(NLogSupport.NLogOutputFilePath))
        return;
      File.Delete(NLogSupport.NLogOutputFilePath);
    }

    public static void AddNlogOutputfileContent(StringBuilder fileContent)
    {
      try
      {
        fileContent.Insert(0, "{ \"Report\":" + Environment.NewLine);
        fileContent.AppendLine(",");
        fileContent.AppendLine("\"NLOG\":[");
        if (File.Exists(NLogSupport.NLogOutputFilePath))
        {
          using (StreamReader streamReader = new StreamReader(NLogSupport.NLogOutputFilePath))
          {
            int num = 0;
            string str;
            while ((str = streamReader.ReadLine()) != null)
            {
              if (streamReader.EndOfStream)
                fileContent.AppendLine(str);
              else
                fileContent.AppendLine(str + ",");
              ++num;
            }
            streamReader.Close();
          }
        }
        fileContent.AppendLine("]}");
      }
      catch
      {
      }
    }

    public static List<NLogFileRecord> GetNLogOutputfileRecords()
    {
      return !File.Exists(NLogSupport.NLogOutputFilePath) ? new List<NLogFileRecord>() : NLogSupport.GetNLogRecords(File.ReadAllText(NLogSupport.NLogOutputFilePath));
    }

    public static List<NLogFileRecord> GetNLogOutputfileRecords(string filePath)
    {
      return !File.Exists(filePath) ? new List<NLogFileRecord>() : NLogSupport.GetNLogRecords(File.ReadAllText(filePath));
    }

    public static List<NLogFileRecord> GetNLogRecords(string fileContent)
    {
      List<NLogFileRecord> nlogRecords = new List<NLogFileRecord>();
      using (StringReader stringReader = new StringReader(fileContent))
      {
        int num1 = 0;
        bool flag = true;
        while (true)
        {
          string str1;
          int num2;
          int length1;
          do
          {
            do
            {
              ++num1;
              str1 = stringReader.ReadLine();
              if (str1 != null)
              {
                if (num1 == 1 && str1.Trim() == "{ \"Report\":")
                  flag = false;
                if (flag)
                {
                  num2 = str1.IndexOf("{");
                  length1 = str1.IndexOf("}") - num2 - 1;
                  if (num2 >= 0 && length1 >= 5)
                    goto label_9;
                }
                else
                  goto label_5;
              }
              else
                break;
            }
            while (!str1.Contains("]}"));
            goto label_27;
label_5:;
          }
          while (!str1.Contains("\"NLOG\":["));
          flag = true;
          continue;
label_9:
          str1.Substring(num2 + 1, length1);
          NLogFileRecord nlogFileRecord = new NLogFileRecord();
          nlogFileRecord.Line = num1;
          int startIndex = num2 + 1;
          while (true)
          {
            int num3 = str1.IndexOf('"', startIndex);
            int num4 = str1.IndexOf('"', num3 + 1);
            int length2 = num4 - num3 - 1;
            if (length2 >= 1)
            {
              string str2 = str1.Substring(num3 + 1, length2);
              if (str1[num4 + 1] == ':')
              {
                int num5 = str1.IndexOf('"', num4 + 2);
                int num6 = str1.IndexOf('"', num5 + 1);
                int length3 = num6 - num5 - 1;
                if (length3 >= 0)
                {
                  string s = str1.Substring(num5 + 1, length3);
                  try
                  {
                    switch (str2)
                    {
                      case "time":
                        string[] strArray1 = s.Split(' ');
                        string[] strArray2 = strArray1[0].Split('-');
                        DateTime dateTime = new DateTime(int.Parse(strArray2[0]), int.Parse(strArray2[1]), int.Parse(strArray2[2]));
                        nlogFileRecord.LogTime = dateTime.Add(TimeSpan.Parse(strArray1[1]));
                        break;
                      case "logger":
                        nlogFileRecord.LoggerName = s;
                        break;
                      case "threadid":
                        nlogFileRecord.ThreadID = int.Parse(s);
                        break;
                      case "level":
                        nlogFileRecord.Level = s;
                        break;
                      case "message":
                        nlogFileRecord.Message = s;
                        break;
                    }
                  }
                  catch
                  {
                  }
                  startIndex = num6 + 1;
                }
                else
                  break;
              }
              else
                break;
            }
            else
              break;
          }
          nlogRecords.Add(nlogFileRecord);
        }
      }
label_27:
      return nlogRecords;
    }
  }
}
