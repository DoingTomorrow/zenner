// Decompiled with JetBrains decompiler
// Type: HandlerLib.MapManagement.MapReader
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib.MapManagement
{
  public class MapReader
  {
    public string ReaderName;
    public string ReaderDescription;
    public const string DefaultSectionName = "UNKNOWN";
    public const string DefaultTypName = "UNKNOWN";
    public List<AddressRange> AddressRanges;
    protected HashSet<string> ParametersToIgnore;
    public SortedList<string, MapParameterInfo> FirmwareParameterList;
    public SortedList<string, uint> FirmwareParameterListFull;
    public SortedList<uint, string> AddressList;
    public SortedList<string, MapSectionInfo> SectionList;
    public SortedList<uint, MapSectionParameters> SectionParameterList;
    public SortedList<string, ListFileParameter> ListFileParameterList;
    public List<KeyValuePair<int, string>> InfoFileLinesList;
    public string LinkerTypeAndVersion;
    public string FirmwareVersion;
    public string MapID;
    public bool ReadListFiles;
    protected int LineNumber;

    public MapReader()
    {
      this.ReaderName = nameof (MapReader);
      this.ReaderDescription = "This is the base class and should not be seen in the MapSeletor";
      this.ParametersToIgnore = new HashSet<string>();
      this.FirmwareParameterList = new SortedList<string, MapParameterInfo>();
      this.FirmwareParameterListFull = new SortedList<string, uint>();
      this.AddressList = new SortedList<uint, string>();
      this.InfoFileLinesList = new List<KeyValuePair<int, string>>();
      this.ListFileParameterList = new SortedList<string, ListFileParameter>();
      this.SectionList = new SortedList<string, MapSectionInfo>();
      this.SectionParameterList = new SortedList<uint, MapSectionParameters>();
      this.AddressRanges = new List<AddressRange>();
      this.LineNumber = 0;
      this.LinkerTypeAndVersion = string.Empty;
      this.FirmwareVersion = string.Empty;
      this.MapID = string.Empty;
      this.ReadListFiles = false;
    }

    public void Clear()
    {
      this.FirmwareParameterList.Clear();
      this.FirmwareParameterListFull.Clear();
      this.AddressList.Clear();
      this.InfoFileLinesList.Clear();
      this.ListFileParameterList.Clear();
      this.SectionList.Clear();
      this.SectionParameterList.Clear();
      this.LineNumber = 0;
      this.LinkerTypeAndVersion = string.Empty;
      this.FirmwareVersion = string.Empty;
      this.MapID = string.Empty;
      this.ReadListFiles = false;
    }

    public virtual void ReadMap(string mapPath, MapClassManager mapClassMgr = null)
    {
      throw new NotImplementedException(nameof (ReadMap));
    }

    public virtual void GenerateByteArray(string classNameSpace, string className, string mapPath)
    {
      throw new NotImplementedException("generateMapFile");
    }

    public virtual void ReadLists(string mapPath, MapClassManager mapClassMgr = null)
    {
      throw new NotImplementedException(nameof (ReadLists));
    }

    public static MapReader GetReaderForMapFile(string mapFile, out string Linker)
    {
      MapReader readerForMapFile = (MapReader) null;
      Linker = string.Empty;
      using (StreamReader streamReader = new StreamReader(mapFile))
      {
        while (true)
        {
          string str = streamReader.ReadLine();
          if (str != null)
          {
            if (str[0].Equals('#'))
            {
              if (Linker == string.Empty && str.ToUpper().IndexOf("LINKER") >= 0)
              {
                string input = str.Replace('#', ' ').Trim();
                Linker = Regex.Replace(input, "\\s+", " ");
                if (Linker.IndexOf("for ARM") > 0)
                  break;
              }
            }
            else
              goto label_11;
          }
          else
            goto label_11;
        }
        return (MapReader) new MapReaderARMv6();
      }
label_11:
      return readerForMapFile;
    }

    protected void NewSection(uint StartAddress, uint BlockSize, string SectionName)
    {
      MapSectionInfo mapSectionInfo = new MapSectionInfo();
      mapSectionInfo.SectionName = SectionName;
      mapSectionInfo.StartAddress = StartAddress;
      mapSectionInfo.BlockSize = BlockSize;
      mapSectionInfo.EndAddress = (uint) ((int) StartAddress + (int) BlockSize - 1);
      if (this.SectionList.ContainsKey(SectionName))
        return;
      this.SectionList.Add(SectionName, mapSectionInfo);
    }

    protected bool isSection(string sectionName)
    {
      foreach (string key in (IEnumerable<string>) this.SectionList.Keys)
      {
        if (key.Equals(sectionName))
          return true;
      }
      return false;
    }

    protected void NewSectionParameter(uint StartAddress, uint Size, string SectionName)
    {
      MapSectionParameters sectionParameters = new MapSectionParameters();
      sectionParameters.SectionName = SectionName;
      sectionParameters.StartAddress = StartAddress;
      sectionParameters.Size = Size;
      if (this.SectionList.ContainsKey(SectionName))
      {
        if (this.SectionParameterList.ContainsKey(StartAddress))
          return;
        this.SectionParameterList.Add(StartAddress, sectionParameters);
      }
      else
        this.AddInfo("Section is not in SectionList Name: " + SectionName);
    }

    protected string getSectionForAddress(uint startAdr)
    {
      foreach (MapSectionInfo mapSectionInfo in (IEnumerable<MapSectionInfo>) this.SectionList.Values)
      {
        if (startAdr >= mapSectionInfo.StartAddress && startAdr <= mapSectionInfo.EndAddress)
          return mapSectionInfo.SectionName;
      }
      return string.Empty;
    }

    protected void NewVariable(
      uint Address,
      string Name,
      uint BytesSize,
      string SectionName = "UNKNOWN",
      string TypName = "UNKNOWN",
      bool ShowInMAP = true)
    {
      string empty = string.Empty;
      if (!this.CheckIgnorParameter(Name))
        return;
      MapParameterInfo mapParameterInfo = new MapParameterInfo();
      mapParameterInfo.FirmwareName = Name;
      mapParameterInfo.MapAddress = Address;
      mapParameterInfo.ByteSize = BytesSize;
      mapParameterInfo.Section = SectionName;
      mapParameterInfo.Typ = TypName;
      mapParameterInfo.ShowInMAP = ShowInMAP;
      if (!this.FirmwareParameterListFull.ContainsKey(Name))
        this.FirmwareParameterListFull.Add(Name, Address);
      if (!this.AddressList.ContainsKey(Address))
        this.AddressList.Add(Address, Name);
      else
        this.AddInfo("Second use of address: 0x" + Address.ToString("x04") + " Name: " + Name);
      if (!this.FirmwareParameterList.ContainsKey(Name))
        this.FirmwareParameterList.Add(Name, mapParameterInfo.Clone());
      else
        this.AddInfo("; Second use of name: 0x" + Address.ToString("x04") + " Name: " + Name);
      if (!string.IsNullOrEmpty(SectionName.Trim()))
        return;
      this.AddInfo("SectionName of address: 0x" + Address.ToString("x04") + " Name: " + Name + " is NOT specified !!!");
    }

    public static Type getDefaultTypeForSize(uint size)
    {
      switch (size)
      {
        case 1:
          return typeof (byte);
        case 2:
          return typeof (ushort);
        case 4:
          return typeof (uint);
        case 8:
          return typeof (ulong);
        default:
          return (Type) null;
      }
    }

    public static Type ConvertToRealType(string typeName)
    {
      string str = typeName;
      Type realType;
      if (str != null)
      {
        switch (str.Length)
        {
          case 3:
            if (str == "int")
              goto label_49;
            else
              goto label_53;
          case 4:
            switch (str[0])
            {
              case 'B':
                if (str == "Byte")
                  goto label_46;
                else
                  goto label_53;
              case 'b':
                if (str == "byte")
                  goto label_46;
                else
                  goto label_53;
              case 'u':
                if (str == "uint")
                  goto label_50;
                else
                  goto label_53;
              default:
                goto label_53;
            }
          case 5:
            switch (str[0])
            {
              case 'F':
                if (str == "Float")
                  goto label_51;
                else
                  goto label_53;
              case 'I':
                switch (str)
                {
                  case "Int16":
                    goto label_47;
                  case "Int32":
                    goto label_49;
                  default:
                    goto label_53;
                }
              case 'S':
                if (str == "SByte")
                  break;
                goto label_53;
              case 'f':
                if (str == "float")
                  goto label_51;
                else
                  goto label_53;
              case 'i':
                if (str == "int_t")
                  goto label_49;
                else
                  goto label_53;
              case 's':
                switch (str)
                {
                  case "sbyte":
                    break;
                  case "short":
                    goto label_47;
                  default:
                    goto label_53;
                }
                break;
              default:
                goto label_53;
            }
            break;
          case 6:
            switch (str[0])
            {
              case 'D':
                if (str == "Double")
                  goto label_52;
                else
                  goto label_53;
              case 'S':
                if (str == "Single")
                  goto label_51;
                else
                  goto label_53;
              case 'U':
                switch (str)
                {
                  case "UInt16":
                    goto label_48;
                  case "UInt32":
                    goto label_50;
                  default:
                    goto label_53;
                }
              case 'd':
                if (str == "double")
                  goto label_52;
                else
                  goto label_53;
              case 'i':
                if (str == "int8_t")
                  break;
                goto label_53;
              case 'u':
                switch (str)
                {
                  case "ushort":
                    goto label_48;
                  case "uint_t":
                    goto label_50;
                  default:
                    goto label_53;
                }
              default:
                goto label_53;
            }
            break;
          case 7:
            switch (str[3])
            {
              case '1':
                if (str == "int16_t")
                  goto label_47;
                else
                  goto label_53;
              case '3':
                if (str == "int32_t")
                  goto label_49;
                else
                  goto label_53;
              case 'a':
                if (str == "float_t")
                  goto label_51;
                else
                  goto label_53;
              case 'r':
                if (str == "short_t")
                  goto label_47;
                else
                  goto label_53;
              case 't':
                if (str == "uint8_t")
                  goto label_46;
                else
                  goto label_53;
              default:
                goto label_53;
            }
          case 8:
            switch (str[4])
            {
              case '1':
                if (str == "uint16_t")
                  goto label_48;
                else
                  goto label_53;
              case '3':
                if (str == "uint32_t")
                  goto label_50;
                else
                  goto label_53;
              case 'l':
                if (str == "double_t")
                  goto label_52;
                else
                  goto label_53;
              case 'r':
                if (str == "ushort_t")
                  goto label_48;
                else
                  goto label_53;
              default:
                goto label_53;
            }
          case 9:
            switch (str[5])
            {
              case '3':
                if (str == "float32_t")
                  goto label_51;
                else
                  goto label_53;
              case '6':
                if (str == "float64_t")
                  goto label_52;
                else
                  goto label_53;
              default:
                goto label_53;
            }
          case 11:
            if (str == "System.Byte")
              goto label_46;
            else
              goto label_53;
          case 12:
            switch (str[10])
            {
              case '1':
                if (str == "System.Int16")
                  goto label_47;
                else
                  goto label_53;
              case '3':
                if (str == "System.Int32")
                  goto label_49;
                else
                  goto label_53;
              case 'a':
                if (str == "System.Float")
                  goto label_51;
                else
                  goto label_53;
              case 't':
                if (str == "System.SByte")
                  break;
                goto label_53;
              default:
                goto label_53;
            }
            break;
          case 13:
            switch (str[7])
            {
              case 'D':
                if (str == "System.Double")
                  goto label_52;
                else
                  goto label_53;
              case 'S':
                if (str == "System.Single")
                  goto label_51;
                else
                  goto label_53;
              case 'U':
                switch (str)
                {
                  case "System.UInt16":
                    goto label_48;
                  case "System.UInt32":
                    goto label_50;
                  default:
                    goto label_53;
                }
              default:
                goto label_53;
            }
          default:
            goto label_53;
        }
        realType = typeof (sbyte);
        goto label_54;
label_46:
        realType = typeof (byte);
        goto label_54;
label_47:
        realType = typeof (short);
        goto label_54;
label_48:
        realType = typeof (ushort);
        goto label_54;
label_49:
        realType = typeof (int);
        goto label_54;
label_50:
        realType = typeof (uint);
        goto label_54;
label_51:
        realType = typeof (float);
        goto label_54;
label_52:
        realType = typeof (double);
        goto label_54;
      }
label_53:
      realType = (Type) null;
label_54:
      return realType;
    }

    protected void NewOrUpdateFieldTypeParameter(ListFileParameter LFP)
    {
      LFP.IsOK = LFP.checkOK();
      if (this.ListFileParameterList.ContainsKey(LFP.FieldName))
      {
        ListFileParameter listFileParameter = this.ListFileParameterList[LFP.FieldName];
        if (!listFileParameter.IsOK && LFP.IsOK || listFileParameter.Type == "UNKNOWN")
          listFileParameter.UpdateParameter(LFP);
      }
      else
        this.ListFileParameterList.Add(LFP.FieldName, LFP.Clone());
      if (!this.FirmwareParameterList.ContainsKey(LFP.FieldName) || !(this.FirmwareParameterList[LFP.FieldName].Typ == "UNKNOWN") || LFP.Type == null || !(LFP.Type != "UNKNOWN"))
        return;
      this.FirmwareParameterList[LFP.FieldName].Typ = LFP.Type;
    }

    private bool CheckIgnorParameter(string paramName)
    {
      foreach (string str1 in this.ParametersToIgnore)
      {
        if (str1.EndsWith("*"))
        {
          string str2 = str1.Substring(0, str1.Length - 1);
          if (paramName.StartsWith(str2))
          {
            this.AddInfo("'" + paramName + "' ignord. It starts with: '" + str2 + "'");
            return false;
          }
        }
        else if (str1 == paramName)
        {
          this.AddInfo("'" + paramName + "' ignord. It is part of ignor lists");
          return false;
        }
      }
      return true;
    }

    protected void AddInfo(string info)
    {
      this.InfoFileLinesList.Add(new KeyValuePair<int, string>(this.LineNumber, info));
    }

    protected void ShowInfo()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (KeyValuePair<int, string> infoFileLines in this.InfoFileLinesList)
      {
        stringBuilder.Append(infoFileLines.Key.ToString("d07") + ": ");
        stringBuilder.AppendLine(infoFileLines.Value);
      }
      stringBuilder.ToString();
    }

    protected string RemoveMultipleSpaces(string ActualLine)
    {
      return Regex.Replace(ActualLine, "\\s+", " ");
    }

    protected string GetValueFromArray(
      string[] StringArray,
      string PointingField,
      short Position = 0,
      bool ExactMatch = true)
    {
      try
      {
        int num = !ExactMatch ? Array.FindIndex<string>(StringArray, (Predicate<string>) (x => x.Contains(PointingField))) : Array.FindIndex<string>(StringArray, (Predicate<string>) (x => x.Equals(PointingField)));
        return num >= 0 ? StringArray[num + (int) Position] : string.Empty;
      }
      catch (Exception ex)
      {
        return (string) null;
      }
    }
  }
}
