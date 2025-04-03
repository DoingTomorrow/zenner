// Decompiled with JetBrains decompiler
// Type: HandlerLib.FirmwareParameterManager
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using HandlerLib.MapManagement;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public sealed class FirmwareParameterManager
  {
    public List<FirmwareParameterInfo> ParameterInfos { get; set; }

    public Assembly HandlerAssembly { get; private set; }

    public static string FileNameWithPath { get; set; }

    public bool isDeveloperVersion { get; set; }

    public static string FileNameUserSettings { get; private set; }

    private static Dictionary<string, string> oSettingsDic { get; set; }

    public FirmwareParameterManager(Assembly handlerAssembly)
    {
      this.ParameterInfos = new List<FirmwareParameterInfo>();
      this.HandlerAssembly = handlerAssembly;
      FirmwareParameterManager.FileNameWithPath = this.revealFilename(this.HandlerAssembly);
    }

    public FirmwareParameterManager(string filenameWithPath)
    {
      this.ParameterInfos = new List<FirmwareParameterInfo>();
      this.HandlerAssembly = (Assembly) null;
      FirmwareParameterManager.FileNameWithPath = filenameWithPath;
    }

    public static List<FirmwareParameterInfo> LoadParameterInfos()
    {
      string fileNameWithPath = FirmwareParameterManager.FileNameWithPath;
      if (!File.Exists(fileNameWithPath))
        return (List<FirmwareParameterInfo>) null;
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (List<FirmwareParameterInfo>));
      TextReader textReader = (TextReader) new StreamReader(fileNameWithPath);
      List<FirmwareParameterInfo> firmwareParameterInfoList = (List<FirmwareParameterInfo>) xmlSerializer.Deserialize(textReader);
      textReader.Close();
      return firmwareParameterInfoList;
    }

    public static void SaveParameterInfos(List<FirmwareParameterInfo> theInfos)
    {
      string fileNameWithPath = FirmwareParameterManager.FileNameWithPath;
      using (TextWriter textWriter = (TextWriter) new StreamWriter(fileNameWithPath))
      {
        new XmlSerializer(typeof (List<FirmwareParameterInfo>)).Serialize(textWriter, (object) theInfos);
        textWriter.Flush();
        textWriter.Close();
      }
    }

    public string revealFilename(Assembly handlerAssembly)
    {
      this.isDeveloperVersion = false;
      string str1 = handlerAssembly.ManifestModule.Name.Replace(".dll", "");
      string str2 = str1 + "_ParameterDefinedFile.xml";
      string directoryName1 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      this.isDeveloperVersion = Debugger.IsAttached;
      string directoryName2 = Path.GetDirectoryName(directoryName1);
      Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
      string dataPath = SystemValues.DataPath;
      string path1 = Path.Combine(directoryName2, "Source", "Handlers", str1, "MapClasses", str2);
      string path2 = Path.Combine(directoryName1, str2);
      string path3 = Path.Combine(directoryName1, "MapClasses", str2);
      string path4 = Path.Combine(dataPath, "MapClasses", str2);
      if (this.isDeveloperVersion && File.Exists(path1))
        return path1;
      if (File.Exists(path3))
        return path3;
      if (File.Exists(path2))
        return path2;
      return File.Exists(path4) ? path4 : string.Empty;
    }

    private void checkPathAndCreateIfNotExists(string fileNameWithPath)
    {
      if (Directory.Exists(Path.GetDirectoryName(fileNameWithPath)))
        return;
      Directory.CreateDirectory(Path.GetDirectoryName(fileNameWithPath));
    }

    public static void GenerateParameterInfo(
      MapDefClassBase mapDef,
      ref FirmwareParameterManager fwMGR)
    {
      if (fwMGR.ParameterInfos == null)
      {
        fwMGR.ParameterInfos = new List<FirmwareParameterInfo>();
        fwMGR.ParameterInfos.Clear();
      }
      uint[] source = new uint[3]{ 1U, 2U, 4U };
      foreach (Parameter32bit parameter32bit in (IEnumerable<Parameter32bit>) mapDef.GetAllParametersList().Values)
      {
        Parameter32bit p32Bit = parameter32bit;
        if (fwMGR.ParameterInfos.SingleOrDefault<FirmwareParameterInfo>((Func<FirmwareParameterInfo, bool>) (kvp => kvp.ParameterName == p32Bit.Name)) == null)
        {
          Type type = MapReader.ConvertToRealType(p32Bit.Typ);
          if (p32Bit.Typ.Contains("UNKNOWN") && ((IEnumerable<uint>) source).Contains<uint>(p32Bit.Size))
          {
            type = MapReader.getDefaultTypeForSize(p32Bit.Size);
            Parameter32bit.SetType(type, p32Bit);
          }
          ParameterType parameterType = new ParameterType()
          {
            ParameterTypeSaved = type == (Type) null ? "UNKNOWN" : type.Name,
            ParameterTypeColPreset = 0
          };
          fwMGR.ParameterInfos.Add(new FirmwareParameterInfo()
          {
            ParameterName = p32Bit.Name,
            ParameterType = parameterType
          });
        }
      }
    }

    public static string getStringFromUserSettings(string settingName)
    {
      string fromUserSettings = string.Empty;
      string nameUserSettings = FirmwareParameterManager.FileNameUserSettings;
      if (File.Exists(nameUserSettings))
      {
        FirmwareParameterManager.loadDictionaryDataFromFile(nameUserSettings);
        Dictionary<string, string> oSettingsDic = FirmwareParameterManager.oSettingsDic;
        if (oSettingsDic.ContainsKey(settingName))
          fromUserSettings = oSettingsDic[settingName].ToLower();
      }
      return fromUserSettings;
    }

    public static void setStringToUserSettings(string settingName, string settingValue)
    {
      if (!File.Exists(FirmwareParameterManager.FileNameUserSettings))
        return;
      Dictionary<string, string> oSettingsDic = FirmwareParameterManager.oSettingsDic;
      if (oSettingsDic.ContainsKey(settingName))
        oSettingsDic[settingName] = settingValue;
      else
        oSettingsDic.Add(settingName, settingValue);
      FirmwareParameterManager.saveDictionaryDataToFile();
    }

    private static void loadDictionaryDataFromFile(string file)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      if (File.Exists(FirmwareParameterManager.FileNameUserSettings))
      {
        StreamReader streamReader = File.OpenText(FirmwareParameterManager.FileNameUserSettings);
        string[] strArray1 = streamReader.ReadToEnd().Replace("\n", "").Replace("\r", "").Split(';');
        for (int index = 0; index < strArray1.Length - 1; ++index)
        {
          string[] strArray2 = strArray1[index].Split('=');
          dictionary.Add(strArray2[0], strArray2[1]);
        }
        streamReader.Close();
      }
      FirmwareParameterManager.oSettingsDic = dictionary;
    }

    private static void saveDictionaryDataToFile()
    {
      StreamWriter text = File.CreateText(FirmwareParameterManager.FileNameUserSettings);
      Dictionary<string, string> oSettingsDic = FirmwareParameterManager.oSettingsDic;
      if (text != null && oSettingsDic != null)
      {
        foreach (string key in oSettingsDic.Keys)
        {
          string str = key + "=" + oSettingsDic[key] + ";";
          text.WriteLine(str);
        }
        text.Flush();
        text.Close();
      }
      else
      {
        if (text == null || oSettingsDic != null)
          return;
        text.Close();
      }
    }
  }
}
