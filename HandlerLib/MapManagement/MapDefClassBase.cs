// Decompiled with JetBrains decompiler
// Type: HandlerLib.MapManagement.MapDefClassBase
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib.MapManagement
{
  public abstract class MapDefClassBase
  {
    public SortedList<string, Parameter32bit> MapVars = (SortedList<string, Parameter32bit>) null;

    public abstract byte[] FullByteList { get; }

    public abstract string[] ParameterList { get; }

    public abstract string[] TypList { get; }

    public abstract string[] SectionList { get; }

    public abstract uint[] SectionAddress { get; }

    public abstract uint[] SectionSize { get; }

    public AddressRange GetSectionRanges(string sectionName)
    {
      for (int index = 0; index < this.SectionList.Length; ++index)
      {
        if (sectionName == this.SectionList[index].Trim())
          return new AddressRange(this.SectionAddress[index], this.SectionSize[index]);
      }
      return (AddressRange) null;
    }

    public SortedList<string, AddressRange> GetAllSectionRanges()
    {
      SortedList<string, AddressRange> allSectionRanges = new SortedList<string, AddressRange>();
      for (int index = 0; index < this.SectionList.Length; ++index)
      {
        string section = this.SectionList[index];
        uint startAddress = this.SectionAddress[index];
        uint size = this.SectionSize[index];
        allSectionRanges.Add(section, new AddressRange(startAddress, size));
      }
      return allSectionRanges;
    }

    public AddressRange GetUsedAddressesForBaseAddressRange(AddressRange adrRange)
    {
      uint[] numArray = new uint[2];
      for (int index = 0; index < this.SectionList.Length; ++index)
      {
        string section = this.SectionList[index];
        uint num1 = this.SectionAddress[index];
        uint num2 = this.SectionSize[index];
        if (num1 >= adrRange.StartAddress && num1 <= adrRange.EndAddress)
        {
          numArray[0] = num1 >= numArray[0] || numArray[0] <= 0U ? (numArray[0] == 0U ? num1 : numArray[0]) : num1;
          numArray[1] += numArray[1] + num2 > adrRange.ByteSize ? adrRange.ByteSize : num2;
        }
      }
      return new AddressRange(numArray[0], numArray[1]);
    }

    public AddressRange GetAddressRangeOfParameters(SortedList<string, Parameter32bit> parameterList)
    {
      Parameter32bit parameter32bit1 = (Parameter32bit) null;
      Parameter32bit parameter32bit2 = (Parameter32bit) null;
      foreach (Parameter32bit parameter32bit3 in (IEnumerable<Parameter32bit>) parameterList.Values)
      {
        if (parameter32bit1 == null)
        {
          parameter32bit1 = parameter32bit3;
          parameter32bit2 = parameter32bit3;
        }
        else if (parameter32bit3.Address > parameter32bit2.Address)
          parameter32bit2 = parameter32bit3;
        else if (parameter32bit3.Address < parameter32bit1.Address)
          parameter32bit1 = parameter32bit3;
      }
      return new AddressRange(parameter32bit1.Address, parameter32bit2.Address - parameter32bit1.Address + parameter32bit2.Size);
    }

    public AddressRange GetAddressRangeBetweenParameters(
      string parameter1,
      string parameter2,
      SortedList<string, Parameter32bit> parameterList)
    {
      if (parameter1 == null || parameter2 == null || parameter1 == parameter2)
        throw new MapExceptionClass(MAP_EXCEPTION_HANDLE.MAP_ILLEGAL_PARAMETER, "illegal parameters");
      Parameter32bit parameter32bit1 = (Parameter32bit) null;
      Parameter32bit parameter32bit2 = (Parameter32bit) null;
      foreach (Parameter32bit parameter32bit3 in (IEnumerable<Parameter32bit>) parameterList.Values)
      {
        if (parameter32bit3.Name == parameter1)
          parameter32bit1 = parameter32bit3;
        else if (parameter32bit3.Name == parameter2)
          parameter32bit2 = parameter32bit3;
        if (parameter32bit1 != null && parameter32bit2 != null)
        {
          AddressRange betweenParameters;
          if (parameter32bit1.Address < parameter32bit2.Address)
          {
            betweenParameters = new AddressRange(parameter32bit1.Address, parameter32bit2.Address - parameter32bit1.Address + parameter32bit2.Size);
          }
          else
          {
            if (parameter32bit1.Address <= parameter32bit2.Address)
              throw new MapExceptionClass(MAP_EXCEPTION_HANDLE.MAP_PARAMETER_ADDRESS_ERROR, "Parameters have same address: " + parameter1 + " and " + parameter2);
            betweenParameters = new AddressRange(parameter32bit2.Address, parameter32bit1.Address - parameter32bit2.Address + parameter32bit1.Size);
          }
          return betweenParameters;
        }
      }
      throw new MapExceptionClass("Parameters not found: " + parameter1 + " and " + parameter2);
    }

    public AddressRange GetAddressRangeOfDefinedParameters(string[] parameterNames)
    {
      if (parameterNames == null)
        throw new Exception("illegal parameters");
      uint startAddress = uint.MaxValue;
      uint num = 0;
      SortedList<string, Parameter32bit> allParametersList = this.GetAllParametersList();
      foreach (string parameterName in parameterNames)
      {
        int index = allParametersList.IndexOfKey(parameterName);
        if (index >= 0)
        {
          Parameter32bit parameter32bit = allParametersList.Values[index];
          if (parameter32bit.Size > 0U)
          {
            if (parameter32bit.Address < startAddress)
              startAddress = parameter32bit.Address;
            if (parameter32bit.EndAddress > num)
              num = parameter32bit.EndAddress;
          }
        }
      }
      return startAddress >= num ? new AddressRange(startAddress, 0U) : new AddressRange(startAddress, (uint) ((int) num - (int) startAddress + 1));
    }

    public List<AddressRange> GetAddressRangeesOfDefinedParameters(
      string[] parameterNames,
      uint splitSize)
    {
      if (parameterNames == null)
        throw new MapExceptionClass(MAP_EXCEPTION_HANDLE.MAP_ILLEGAL_PARAMETER, "illegal parameters");
      SortedList<string, Parameter32bit> allParametersList = this.GetAllParametersList();
      SortedList<uint, Parameter32bit> sortedList = new SortedList<uint, Parameter32bit>();
      foreach (string parameterName in parameterNames)
      {
        int index = allParametersList.IndexOfKey(parameterName);
        if (index >= 0)
        {
          Parameter32bit parameter32bit = allParametersList.Values[index];
          sortedList.Add(parameter32bit.Address, parameter32bit);
        }
      }
      List<AddressRange> definedParameters = new List<AddressRange>();
      AddressRange addressRange = (AddressRange) null;
      foreach (Parameter32bit parameter32bit in (IEnumerable<Parameter32bit>) sortedList.Values)
      {
        if (addressRange == null)
          addressRange = new AddressRange(parameter32bit.Address, parameter32bit.Size);
        else if (parameter32bit.Address > addressRange.EndAddress + splitSize)
        {
          definedParameters.Add(addressRange);
          addressRange = new AddressRange(parameter32bit.Address, parameter32bit.Size);
        }
        else
          addressRange.EndAddress = parameter32bit.EndAddress;
      }
      if (addressRange != null)
        definedParameters.Add(addressRange);
      return definedParameters;
    }

    public SortedList<string, Parameter32bit> GetUsedParametersForAddressRoom(
      AddressRange adrRange,
      SortedList<string, Parameter32bit> allparams)
    {
      SortedList<string, Parameter32bit> parametersForAddressRoom = new SortedList<string, Parameter32bit>();
      foreach (Parameter32bit parameter32bit in (IEnumerable<Parameter32bit>) allparams.Values)
      {
        if (parameter32bit.Address >= adrRange.StartAddress && parameter32bit.Address <= adrRange.EndAddress)
          parametersForAddressRoom.Add(parameter32bit.Name, parameter32bit);
      }
      return parametersForAddressRoom;
    }

    public SortedList<string, Parameter32bit> GetAllParametersList()
    {
      if (this.MapVars == null)
      {
        this.MapVars = new SortedList<string, Parameter32bit>();
        int readIndex = 0;
        if (LoadDataFromMapClass.GetByteForced(this.FullByteList, ref readIndex) > (byte) 0)
          throw new MapExceptionClass(MAP_EXCEPTION_HANDLE.MAP_CLASS_ILLEGAL_FORMAT, (Exception) new FormatException("Illegal MapDef class format"));
        string str = "";
        while (readIndex < this.FullByteList.Length)
        {
          try
          {
            string stringForced = LoadDataFromMapClass.GetStringForced(this.FullByteList, ref readIndex);
            str = stringForced;
            uint uintForced1 = LoadDataFromMapClass.GetUIntForced(this.FullByteList, ref readIndex);
            uint uintForced2 = LoadDataFromMapClass.GetUIntForced(this.FullByteList, ref readIndex);
            string section = this.SectionList[(int) LoadDataFromMapClass.GetUShortForced(this.FullByteList, ref readIndex)];
            string typ = this.TypList[(int) LoadDataFromMapClass.GetUShortForced(this.FullByteList, ref readIndex)];
            Parameter32bit parameter32bit = new Parameter32bit(section, stringForced, uintForced1, uintForced2, typ);
            this.MapVars.Add(parameter32bit.Name, parameter32bit);
          }
          catch (Exception ex)
          {
            throw new MapExceptionClass(MAP_EXCEPTION_HANDLE.MAP_CLASS_ILLEGAL_FORMAT, (Exception) new FormatException("Illegal parameter config class. Error near parameter:" + str + Environment.NewLine + ex.Message));
          }
        }
      }
      return this.MapVars;
    }

    public List<AddressRange> GetUsedSubAddressRanges(
      AddressRange blockAddressRange,
      int splitSize,
      string[] notUsedParameters = null)
    {
      SortedList<string, Parameter32bit> allParametersList = this.GetAllParametersList();
      List<Parameter32bit> parameter32bitList = new List<Parameter32bit>();
      foreach (KeyValuePair<string, Parameter32bit> keyValuePair in allParametersList)
      {
        if (notUsedParameters == null || !((IEnumerable<string>) notUsedParameters).Contains<string>(keyValuePair.Key))
          parameter32bitList.Add(keyValuePair.Value);
      }
      SortedList<uint, AddressRange> sortedList = new SortedList<uint, AddressRange>();
      foreach (Parameter32bit parameter32bit in parameter32bitList)
      {
        if (parameter32bit.Address <= blockAddressRange.EndAddress && parameter32bit.Address >= blockAddressRange.StartAddress)
        {
          if (sortedList.Count == 0)
          {
            AddressRange addressRange = new AddressRange(parameter32bit.Address, parameter32bit.Size);
            sortedList.Add(addressRange.StartAddress, addressRange);
          }
          else
          {
            bool flag = false;
            for (int index = 0; index < sortedList.Count; ++index)
            {
              if ((long) sortedList.Values[index].StartAddress - (long) splitSize < (long) parameter32bit.EndAddress && (long) sortedList.Values[index].EndAddress + (long) splitSize > (long) parameter32bit.Address)
              {
                if (parameter32bit.Address < sortedList.Values[index].StartAddress)
                  sortedList.Values[index].SetStartAddressHoldEndAddress(parameter32bit.Address);
                if (parameter32bit.EndAddress > sortedList.Values[index].EndAddress)
                  sortedList.Values[index].EndAddress = parameter32bit.EndAddress;
                flag = true;
                break;
              }
            }
            if (!flag)
            {
              AddressRange addressRange = new AddressRange(parameter32bit.Address, parameter32bit.Size);
              sortedList.Add(addressRange.StartAddress, addressRange);
            }
          }
        }
      }
      int index1 = 0;
      while (index1 < sortedList.Count - 1)
      {
        if (sortedList.Values[index1 + 1].StartAddress < sortedList.Values[index1].EndAddress || (long) (sortedList.Values[index1 + 1].StartAddress - sortedList.Values[index1].EndAddress) < (long) splitSize)
        {
          if (sortedList.Values[index1 + 1].EndAddress > sortedList.Values[index1].EndAddress)
            sortedList.Values[index1].EndAddress = sortedList.Values[index1 + 1].EndAddress;
          sortedList.RemoveAt(index1 + 1);
        }
        else
          ++index1;
      }
      return sortedList.Values.ToList<AddressRange>();
    }

    public List<AddressRangeInfo> GetUsedSubRangesInfos(List<AddressRangeInfo> ranges)
    {
      SortedList<string, Parameter32bit> allParametersList = this.GetAllParametersList();
      List<AddressRangeInfo> usedSubRangesInfos = new List<AddressRangeInfo>();
      foreach (AddressRangeInfo range in ranges)
      {
        usedSubRangesInfos.Add(range);
        List<AddressRangeInfo> collection = new List<AddressRangeInfo>();
        foreach (Parameter32bit parameter32bit in (IEnumerable<Parameter32bit>) allParametersList.Values)
        {
          if (range.IsInAdressRange(parameter32bit.Address))
            collection.Add(new AddressRangeInfo("   " + parameter32bit.Name, parameter32bit.AddressRange));
        }
        collection.Sort();
        usedSubRangesInfos.AddRange((IEnumerable<AddressRangeInfo>) collection);
      }
      return usedSubRangesInfos;
    }

    public List<Parameter32bit> GetAllParameterForSection(string sectionName)
    {
      return this.GetAllParameterForSection(this.GetAllParametersList(), sectionName);
    }

    public List<Parameter32bit> GetAllParameterForSection(
      SortedList<string, Parameter32bit> allParameters,
      string sectionName)
    {
      if (string.IsNullOrEmpty(sectionName))
        throw new MapExceptionClass(MAP_EXCEPTION_HANDLE.MAP_CLASS_UNKNOWN_SECTIONNAME, (Exception) new ArgumentException("Argument for GetAllParameterForSection is NULL or EMPTY"));
      List<Parameter32bit> parameter32bitList = new List<Parameter32bit>();
      return allParameters.Values.Where<Parameter32bit>((Func<Parameter32bit, bool>) (x => x.Section.ToUpper().Equals(sectionName.ToUpper()))).ToList<Parameter32bit>();
    }

    public SortedList<string, Parameter32bit> GetUsedParametersList(string[] usedParameterNames)
    {
      return this.GetUsedParametersList(this.GetAllParametersList(), usedParameterNames);
    }

    public SortedList<string, Parameter32bit> GetUsedParametersList(
      SortedList<string, Parameter32bit> allParameters,
      string[] usedParameterNames)
    {
      SortedList<string, Parameter32bit> usedParametersList = new SortedList<string, Parameter32bit>();
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string usedParameterName in usedParameterNames)
      {
        int index = allParameters.IndexOfKey(usedParameterName);
        if (index >= 0)
          usedParametersList.Add(usedParameterName, allParameters.Values[index]);
        else
          stringBuilder.AppendLine(usedParameterName);
      }
      return usedParametersList;
    }

    public static MapDefClassBase GetMapObjectFromVersion(Assembly handlerAssembly, uint Version)
    {
      MapDefClassBase objectFromVersion = (MapDefClassBase) null;
      string str = "MapDefClass";
      if (Version > 0U)
      {
        foreach (Type type in handlerAssembly.GetTypes())
        {
          if (type.Name.StartsWith(str))
          {
            string s = type.Name.Substring(str.Length);
            if (!string.IsNullOrEmpty(s))
            {
              uint result;
              uint.TryParse(s, NumberStyles.HexNumber, (IFormatProvider) null, out result);
              if ((int) result == (int) Version)
              {
                objectFromVersion = (MapDefClassBase) Activator.CreateInstance(type);
                break;
              }
            }
          }
        }
      }
      if (objectFromVersion == null)
      {
        objectFromVersion = MapDefClassBase.GetMapObjectDynamicFromVersion(handlerAssembly, Version);
        if (objectFromVersion == null)
          throw new MapExceptionClass(MAP_EXCEPTION_HANDLE.MAPFILE_NOT_FOUND, "Can not find MapFile for " + new FirmwareVersion(Version).ToString() + " (0x" + Version.ToString("X8") + ") in the actual project!");
      }
      try
      {
        objectFromVersion.GetAllParametersList();
        FirmwareParameterManager parameterManager = new FirmwareParameterManager(handlerAssembly);
        List<FirmwareParameterInfo> firmwareParameterInfoList = FirmwareParameterManager.LoadParameterInfos();
        if (firmwareParameterInfoList == null)
          return objectFromVersion;
        foreach (FirmwareParameterInfo firmwareParameterInfo in firmwareParameterInfoList)
        {
          string parameterName = firmwareParameterInfo.ParameterName;
          string parameterTypeSaved = firmwareParameterInfo.ParameterType.ParameterTypeSaved;
          int index = objectFromVersion.MapVars.IndexOfKey(parameterName);
          if (index >= 0)
          {
            Type realType = MapReader.ConvertToRealType(parameterTypeSaved);
            if (realType != (Type) null)
              Parameter32bit.SetType(realType, objectFromVersion.MapVars.Values[index]);
          }
        }
      }
      catch
      {
      }
      return objectFromVersion;
    }

    public static MapDefClassBase GetMapObjectDynamicFromVersion(
      Assembly handlerAssembly,
      uint Version)
    {
      MapDefClassBase mapDefClassBase = (MapDefClassBase) null;
      string str = "MapDefClass";
      if (Version > 0U)
      {
        string mapName = str + Version.ToString("x8") + ".cs";
        MapDefClassDynamic mapDefClassDynamic = new MapDefClassDynamic();
        mapDefClassDynamic.readMapDefinitionFromDynamicFile(mapName, handlerAssembly.ManifestModule.Name);
        if (mapDefClassDynamic.FullByteList.Length != 0)
          mapDefClassBase = (MapDefClassBase) mapDefClassDynamic;
      }
      return mapDefClassBase != null ? mapDefClassBase : throw new MapExceptionClass(MAP_EXCEPTION_HANDLE.MAPFILE_NOT_FOUND, "Can not find Mapfile for this firmware(0x" + Version.ToString("X8") + "). (DynamicMapFile)");
    }

    public static MapDefClassBase GetCompatibleMapObjectFromVersion(
      Assembly handlerAssembly,
      uint Version)
    {
      MapDefClassBase mapDefClassBase = (MapDefClassBase) null;
      Type type1 = (Type) null;
      uint num = 0;
      string str = "MapDefClass";
      foreach (Type type2 in handlerAssembly.GetTypes())
      {
        if (type2.Name.StartsWith(str))
        {
          string s = type2.Name.Substring(str.Length);
          uint result;
          if (!string.IsNullOrEmpty(s) && uint.TryParse(s, NumberStyles.HexNumber, (IFormatProvider) null, out result))
          {
            if ((int) result == (int) Version)
            {
              mapDefClassBase = (MapDefClassBase) Activator.CreateInstance(type2);
              break;
            }
            if (((int) result & -61441) == ((int) Version & -61441) && (type1 == (Type) null || num < result))
            {
              num = result;
              type1 = type2;
            }
          }
        }
      }
      if (mapDefClassBase == null && type1 != (Type) null)
        mapDefClassBase = (MapDefClassBase) Activator.CreateInstance(type1);
      return mapDefClassBase != null ? mapDefClassBase : throw new MapExceptionClass(MAP_EXCEPTION_HANDLE.MAPFILE_FIRMWARE_NOT_SUPPORTED, "The firmware " + new FirmwareVersion(Version).ToString() + " (0x" + Version.ToString("x8") + ") is not supported! No MAP file.");
    }

    public Parameter32bit GetParameter(string parameterName)
    {
      if (string.IsNullOrEmpty(parameterName))
        throw new MapExceptionClass(MAP_EXCEPTION_HANDLE.MAP_PARAMETER_IS_NULL, "GetParameter has NULL argument.", (Exception) new ArgumentNullException(nameof (parameterName)));
      SortedList<string, Parameter32bit> allParametersList = this.GetAllParametersList();
      if (allParametersList == null)
        throw new MapExceptionClass(MAP_EXCEPTION_HANDLE.MAP_PARAMETER_LIST_IS_NULL, "The list of parameters is null!", (Exception) new ArgumentNullException(nameof (parameterName)));
      return allParametersList.ContainsKey(parameterName) ? allParametersList[parameterName] : throw new MapExceptionClass(MAP_EXCEPTION_HANDLE.MAP_PARAMETER_NOT_FOUND, "Key(" + parameterName + ") does not exists in list of parameters!", (Exception) new KeyNotFoundException("Key(" + parameterName + ") does not exists in list of parameters!"));
    }
  }
}
