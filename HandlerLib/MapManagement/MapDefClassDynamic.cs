// Decompiled with JetBrains decompiler
// Type: HandlerLib.MapManagement.MapDefClassDynamic
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib.MapManagement
{
  public class MapDefClassDynamic : MapDefClassBase
  {
    public string MapName = string.Empty;
    private bool fullByteListSection = false;
    private string[] parameterList;
    private string[] typList;
    private string[] sectionList;
    private uint[] sectionAddress;
    private uint[] sectionSize;
    private byte[] fullByteList;

    public override byte[] FullByteList => this.fullByteList;

    public override string[] ParameterList => this.parameterList;

    public override string[] TypList => this.typList;

    public override string[] SectionList => this.sectionList;

    public override uint[] SectionAddress => this.sectionAddress;

    public override uint[] SectionSize => this.sectionSize;

    public void readMapDefinitionFromDynamicFile(string mapName, string callingModuleName)
    {
      this.MapName = mapName;
      this.fullByteListSection = false;
      callingModuleName = callingModuleName.Substring(0, callingModuleName.Length - 4);
      string appPath = SystemValues.AppPath;
      string path1 = Path.Combine(appPath, "MapClasses", mapName);
      string path2 = Path.Combine(appPath, "Basics", callingModuleName, "MapClasses", mapName);
      string path3 = Path.Combine(appPath, "Handlers", callingModuleName, "MapClasses", mapName);
      string str = Path.Combine(SystemValues.MappingPath, mapName);
      if (File.Exists(path3))
        this.parseMapFile(new List<string>((IEnumerable<string>) File.ReadAllLines(path3)));
      else if (File.Exists(path2))
        this.parseMapFile(new List<string>((IEnumerable<string>) File.ReadAllLines(path2)));
      else if (File.Exists(path1))
      {
        this.parseMapFile(new List<string>((IEnumerable<string>) File.ReadAllLines(path1)));
      }
      else
      {
        if (!File.Exists(str))
          throw new MapExceptionClass(MAP_EXCEPTION_HANDLE.MAPFILE_NOT_FOUND, (Exception) new FileNotFoundException("Dynamic MapFile could not be found in path!", str, (Exception) null));
        this.parseMapFile(new List<string>((IEnumerable<string>) File.ReadAllLines(str)));
      }
    }

    public void parseMapFile(List<string> mapFile)
    {
      List<byte> byteList = new List<byte>();
      foreach (string str1 in mapFile)
      {
        if (!this.fullByteListSection)
        {
          if (str1.Contains("parameterList") && !str1.Contains("public override"))
          {
            int startIndex = str1.IndexOf("{") + 1;
            int length = str1.Length - startIndex - 3;
            string str2 = str1.Substring(startIndex, length).Replace('"', ' ');
            List<string> stringList = new List<string>();
            stringList.AddRange((IEnumerable<string>) str2.Split(','));
            stringList.RemoveAll((Predicate<string>) (str => string.IsNullOrEmpty(str)));
            this.parameterList = new string[stringList.Count];
            this.parameterList = stringList.ToArray();
          }
          if (str1.Contains("typList") && !str1.Contains("public override"))
          {
            int startIndex = str1.IndexOf("{") + 1;
            int length = str1.Length - startIndex - 3;
            string str3 = str1.Substring(startIndex, length).Replace('"', ' ');
            List<string> stringList = new List<string>();
            stringList.AddRange((IEnumerable<string>) str3.Split(','));
            stringList.RemoveAll((Predicate<string>) (str => string.IsNullOrEmpty(str)));
            this.typList = new string[stringList.Count];
            this.typList = stringList.ToArray();
          }
          if (str1.Contains("sectionList") && !str1.Contains("public override"))
          {
            int startIndex = str1.IndexOf("{") + 1;
            int length = str1.Length - startIndex - 3;
            string str4 = str1.Substring(startIndex, length).Replace('"', ' ');
            List<string> stringList = new List<string>();
            stringList.AddRange((IEnumerable<string>) str4.Split(','));
            stringList.RemoveAll((Predicate<string>) (str => string.IsNullOrEmpty(str)));
            this.sectionList = new string[stringList.Count];
            this.sectionList = stringList.ToArray();
          }
          if (str1.Contains("sectionAddress") && !str1.Contains("public override"))
          {
            int startIndex = str1.IndexOf("{") + 1;
            int length = str1.Length - startIndex - 3;
            string str5 = str1.Substring(startIndex, length).Replace("\"", "");
            List<string> stringList = new List<string>();
            stringList.AddRange((IEnumerable<string>) str5.Split(','));
            stringList.RemoveAll((Predicate<string>) (str => string.IsNullOrEmpty(str)));
            uint result = 0;
            this.sectionAddress = new uint[stringList.Count];
            for (int index = 0; index < stringList.Count; ++index)
            {
              if (!uint.TryParse(stringList[index].Trim().Substring(2), NumberStyles.HexNumber, (IFormatProvider) CultureInfo.CurrentCulture, out result))
                throw new MapExceptionClass(MAP_EXCEPTION_HANDLE.MAPFILE_NOT_READABLE, "Address parsing error while MAP reading...", new Exception("Address parsing error while MAP reading..."));
              this.sectionAddress[index] = result;
            }
          }
          if (str1.Contains("sectionSize") && !str1.Contains("public override"))
          {
            int startIndex = str1.IndexOf("{") + 1;
            int length = str1.Length - startIndex - 3;
            string str6 = str1.Substring(startIndex, length).Replace("\"", "");
            List<string> stringList = new List<string>();
            stringList.AddRange((IEnumerable<string>) str6.Split(','));
            stringList.RemoveAll((Predicate<string>) (str => string.IsNullOrEmpty(str)));
            uint result = 0;
            this.sectionSize = new uint[stringList.Count];
            for (int index = 0; index < stringList.Count; ++index)
            {
              if (!uint.TryParse(stringList[index].Trim().Substring(2), NumberStyles.HexNumber, (IFormatProvider) CultureInfo.CurrentCulture, out result))
                throw new MapExceptionClass(MAP_EXCEPTION_HANDLE.MAPFILE_NOT_READABLE, new Exception("Sizes parsing error while MAP reading..."));
              this.sectionSize[index] = result;
            }
          }
        }
        if (str1.Contains("fullByteList") && !str1.Contains("public override") || this.fullByteListSection)
        {
          string str7 = new string(str1.ToCharArray());
          this.fullByteListSection = true;
          if (str1.IndexOf("};") >= 0)
          {
            this.fullByteListSection = false;
            this.fullByteList = new byte[byteList.Count];
            this.fullByteList = byteList.ToArray();
          }
          else
          {
            string str8 = str7.Replace("byte[] fullByteList = {", "").Trim();
            bool flag = false;
            while (!flag)
            {
              int startIndex = str8.IndexOf("/*");
              int count = str8.IndexOf("*/") + 2 - startIndex;
              flag = startIndex < 0;
              if (!flag)
                str8 = str8.Remove(startIndex, count);
            }
            List<string> stringList = new List<string>();
            stringList.AddRange((IEnumerable<string>) str8.Split(','));
            stringList.RemoveAll((Predicate<string>) (str => string.IsNullOrEmpty(str)));
            foreach (string str9 in stringList)
            {
              string s = str9.Replace("0x", "").Trim();
              try
              {
                byte num = byte.Parse(s, NumberStyles.HexNumber);
                byteList.Add(num);
              }
              catch (Exception ex)
              {
                throw ex;
              }
            }
          }
        }
      }
    }
  }
}
