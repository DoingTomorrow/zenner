// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.GMMConfig
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class GMMConfig
  {
    private static Logger logger = LogManager.GetLogger(nameof (GMMConfig));
    public const string DEFAULT_CONFIG_FILE_NAME = "Defaults.gmm";
    public const string CONFIG_STARTUP_FILE_NAME = "StartUp.gmms";
    private DataSet config;
    private readonly object syncLock = new object();
    private bool DoNotUseStartupFile = false;

    public GMMConfig()
    {
      Type type = typeof (string);
      DataTable table = new DataTable("KonfigTable");
      table.Columns.Add("GroupID", type);
      table.Columns.Add("Namespace", type);
      table.Columns.Add("Variablenname", type);
      table.Columns.Add("Variablenwert", type);
      this.config = new DataSet("KonfigПривет");
      this.config.Tables.Add(table);
      this.StartUpFileName = Path.Combine(SystemValues.DataPath, "StartUp.gmms");
      this.ConfigFileName = Path.Combine(SystemValues.DataPath, "Defaults.gmm");
    }

    public string ConfigFileName { get; set; }

    public string StartUpFileName { get; set; }

    public bool SetOrUpdateValue(string strNamespace, string strVariable, string strInhalt)
    {
      lock (this.syncLock)
      {
        DataTable table = this.config.Tables[0];
        DataRow[] dataRowArray = table.Select("GroupID = '0' and Namespace = '" + strNamespace + "' and Variablenname = '" + strVariable + "'");
        switch (dataRowArray.Length)
        {
          case 0:
            DataRow row = table.NewRow();
            row["GroupID"] = (object) 0;
            row["Namespace"] = (object) strNamespace;
            row["Variablenname"] = (object) strVariable;
            row["Variablenwert"] = (object) strInhalt;
            table.Rows.Add(row);
            table.AcceptChanges();
            return true;
          case 1:
            dataRowArray[0]["Variablenwert"] = (object) strInhalt;
            table.AcceptChanges();
            return true;
          default:
            throw new ArgumentException("Es gibt mehrere Elemente, die die Bedingungen für diese Variable erfüllen");
        }
      }
    }

    public bool RemoveValue(string strNamespace, string strVariable)
    {
      lock (this.syncLock)
      {
        DataTable table = this.config.Tables[0];
        DataRow[] dataRowArray = table.Select("GroupID = '0' and Namespace = '" + strNamespace + "' and Variablenname = '" + strVariable + "'");
        if (dataRowArray.GetLength(0) != 1)
          return false;
        dataRowArray[0].Delete();
        table.AcceptChanges();
        return true;
      }
    }

    public bool RemoveAllVariablesFromNamespace(string strNamespace)
    {
      lock (this.syncLock)
      {
        DataTable table = this.config.Tables[0];
        DataRow[] dataRowArray = table.Select("GroupID = '0' and Namespace = '" + strNamespace + "'");
        if (dataRowArray.Length == 0)
          return false;
        foreach (DataRow dataRow in dataRowArray)
          dataRow.Delete();
        table.AcceptChanges();
        return true;
      }
    }

    public string GetValue(string strNamespace, string strVariable)
    {
      lock (this.syncLock)
      {
        DataRow[] dataRowArray = this.config.Tables[0].Select("GroupID = '0' and Namespace = '" + strNamespace + "' and Variablenname = '" + strVariable + "'");
        return dataRowArray.GetLength(0) == 1 ? dataRowArray[0]["Variablenwert"].ToString() : "";
      }
    }

    public SortedList<string, string> GetValues(string strNamespace)
    {
      lock (this.syncLock)
      {
        DataRow[] dataRowArray = this.config.Tables[0].Select("GroupID = '0' and Namespace = '" + strNamespace + "'");
        if (dataRowArray == null)
          return (SortedList<string, string>) null;
        SortedList<string, string> values = new SortedList<string, string>();
        foreach (DataRow dataRow in dataRowArray)
        {
          string key = dataRow["Variablenname"].ToString();
          string str = dataRow["Variablenwert"].ToString();
          if (!values.ContainsKey(key))
            values.Add(key, str);
        }
        return values;
      }
    }

    public bool ReadConfigFile(string pathToConfigFileName)
    {
      lock (this.syncLock)
      {
        GMMConfig.logger.Info("ReadConfigFile: {0}", pathToConfigFileName);
        try
        {
          this.ConfigFileName = Path.Combine(SystemValues.DataPath, pathToConfigFileName);
          if (!File.Exists(this.ConfigFileName))
            return false;
          this.config.Clear();
          int num = (int) this.config.ReadXml(this.ConfigFileName);
          this.WriteStartFile(this.ConfigFileName);
          return true;
        }
        catch (Exception ex)
        {
          GMMConfig.logger.Error(ex.Message);
          return false;
        }
      }
    }

    public bool ReadConfigForPlugInGmm(string pathToConfigFileName)
    {
      this.DoNotUseStartupFile = true;
      lock (this.syncLock)
      {
        this.ConfigFileName = pathToConfigFileName;
        GMMConfig.logger.Info("ReadConfigFile: " + this.ConfigFileName);
        try
        {
          if (!File.Exists(this.ConfigFileName))
            return false;
          this.config.Clear();
          int num = (int) this.config.ReadXml(this.ConfigFileName);
          this.WriteStartFile(this.ConfigFileName);
          return true;
        }
        catch (Exception ex)
        {
          GMMConfig.logger.Error(ex.Message);
          return false;
        }
      }
    }

    public bool ReadConfigFile() => this.ReadConfigFile(this.ConfigFileName, this.StartUpFileName);

    public bool ReadConfigFile(string configFile, string startFile)
    {
      lock (this.syncLock)
      {
        this.ConfigFileName = configFile;
        this.StartUpFileName = startFile;
        GMMConfig.logger.Info<string, string>("Read ConfigFile: {0}, StartFile: {1}", this.ConfigFileName, this.StartUpFileName);
        try
        {
          if (File.Exists(this.StartUpFileName))
          {
            string path = string.Empty;
            using (StreamReader streamReader = new StreamReader(this.StartUpFileName))
            {
              while (true)
              {
                string str1 = streamReader.ReadLine();
                if (str1 != null)
                {
                  string str2 = str1.Trim();
                  if (str2.Length != 0)
                  {
                    if (str2.StartsWith("#"))
                    {
                      string str3 = str2.Substring(1);
                      if (SystemValues.AppPath == str3)
                        break;
                    }
                    else
                      path = str2;
                  }
                  else
                    break;
                }
                else
                  break;
              }
              streamReader.Close();
            }
            if (path.Length > 0 && File.Exists(path))
              this.ConfigFileName = path;
          }
          else
          {
            string path1 = Path.Combine(SystemValues.DataPath, "Defaults.gmms");
            if (File.Exists(path1))
            {
              string path2 = string.Empty;
              using (StreamReader streamReader = new StreamReader(path1))
              {
                while (true)
                {
                  string str4 = streamReader.ReadLine();
                  if (str4 != null)
                  {
                    string str5 = str4.Trim();
                    if (str5.Length != 0)
                    {
                      if (str5.StartsWith("#"))
                      {
                        string str6 = str5.Substring(1);
                        if (SystemValues.AppPath == str6)
                          break;
                      }
                      else
                        path2 = str5;
                    }
                    else
                      break;
                  }
                  else
                    break;
                }
                streamReader.Close();
              }
              if (path2.Length > 0 && File.Exists(path2))
                this.ConfigFileName = path2;
            }
          }
        }
        catch (Exception ex)
        {
          GMMConfig.logger.Error(ex.Message);
          return false;
        }
        return string.IsNullOrEmpty(this.ConfigFileName) ? this.ReadConfigFile(Path.Combine(SystemValues.DataPath, "Defaults.gmm")) : this.ReadConfigFile(this.ConfigFileName);
      }
    }

    public bool WriteConfigFile()
    {
      lock (this.syncLock)
      {
        try
        {
          if (string.IsNullOrEmpty(this.ConfigFileName))
          {
            this.ConfigFileName = Path.Combine(SystemValues.DataPath, "Defaults.gmm");
            this.WriteStartFile(this.ConfigFileName);
          }
          this.config.WriteXml(this.ConfigFileName);
          return true;
        }
        catch (Exception ex)
        {
          GMMConfig.logger.Error(ex.Message);
          return false;
        }
      }
    }

    public bool WriteConfigFile(string strName)
    {
      lock (this.syncLock)
      {
        try
        {
          this.WriteStartFile(strName);
          this.config.WriteXml(strName, XmlWriteMode.IgnoreSchema);
          this.ConfigFileName = strName;
          return true;
        }
        catch (Exception ex)
        {
          GMMConfig.logger.Error(ex.Message);
          return false;
        }
      }
    }

    public void WriteStartFile(string SetupFileName)
    {
      if (this.DoNotUseStartupFile)
        return;
      this.StartUpFileName = Path.Combine(SystemValues.DataPath, this.StartUpFileName);
      List<string> stringList = new List<string>();
      int num1 = -1;
      try
      {
        if (File.Exists(this.StartUpFileName))
        {
          using (StreamReader streamReader = new StreamReader(this.StartUpFileName))
          {
            int num2 = 0;
            while (true)
            {
              string str1 = streamReader.ReadLine();
              if (str1 != null)
              {
                string str2 = str1.Trim();
                if (str2.Length != 0)
                {
                  stringList.Add(str2);
                  if (str2.StartsWith("#"))
                  {
                    string str3 = str2.Substring(1);
                    if (SystemValues.AppPath == str3)
                      num1 = num2 - 1;
                  }
                  ++num2;
                }
                else
                  break;
              }
              else
                break;
            }
            streamReader.Close();
          }
        }
      }
      catch (Exception ex)
      {
        GMMConfig.logger.Error(ex.Message);
      }
      if (num1 < 0)
        num1 = stringList.Count != 1 ? stringList.Count : 0;
      using (StreamWriter streamWriter = new StreamWriter((Stream) new FileStream(this.StartUpFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite), Encoding.UTF8))
      {
        for (int index = 0; index < stringList.Count || index == num1; ++index)
        {
          if (index == num1)
          {
            streamWriter.WriteLine(SetupFileName);
            streamWriter.WriteLine("#" + SystemValues.AppPath);
            ++index;
          }
          else
            streamWriter.WriteLine(stringList[index]);
        }
        streamWriter.Close();
      }
    }
  }
}
