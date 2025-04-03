// Decompiled with JetBrains decompiler
// Type: HandlerLib.DeviceMemory
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommonWPF;
using GmmDbLib;
using GmmDbLib.DataSets;
using HandlerLib.MapManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class DeviceMemory
  {
    public uint FirmwareVersion;
    public MapDefClassBase MapDef;
    public SortedList<string, Parameter32bit> UsedParametersByName;
    public MeterTypeNAME MeterTypeName;
    public bool CloneCreated = false;

    public SortedList<uint, DeviceMemoryStorage> MemoryBlockList { get; private set; }

    public DeviceMemory(uint FirmwareVersion)
    {
      this.FirmwareVersion = FirmwareVersion;
      this.MemoryBlockList = new SortedList<uint, DeviceMemoryStorage>();
    }

    public DeviceMemory(uint FirmwareVersion, Assembly handlerAssembly)
    {
      this.FirmwareVersion = FirmwareVersion;
      this.MemoryBlockList = new SortedList<uint, DeviceMemoryStorage>();
      this.MapDef = MapDefClassBase.GetMapObjectFromVersion(handlerAssembly, FirmwareVersion);
    }

    public DeviceMemory(byte[] compressedData)
    {
      this.MemoryBlockList = new SortedList<uint, DeviceMemoryStorage>();
      this.CreateFromCompressedData(compressedData);
    }

    public DeviceMemory(DeviceMemory sourceToClone)
    {
      this.FirmwareVersion = sourceToClone.FirmwareVersion;
      this.MemoryBlockList = new SortedList<uint, DeviceMemoryStorage>();
      foreach (DeviceMemoryStorage deviceMemoryStorage in (IEnumerable<DeviceMemoryStorage>) sourceToClone.MemoryBlockList.Values)
        this.MemoryBlockList.Add(deviceMemoryStorage.StartAddress, deviceMemoryStorage.Clone());
      this.MapDef = sourceToClone.MapDef;
      this.UsedParametersByName = sourceToClone.UsedParametersByName;
      sourceToClone.CloneCreated = true;
    }

    public void AddMemoryBlock(DeviceMemoryType deviceMemoryType, uint startAddess, uint byteSize)
    {
      DeviceMemoryStorage deviceMemoryStorage = new DeviceMemoryStorage(deviceMemoryType, startAddess, byteSize);
      this.MemoryBlockList.Add(deviceMemoryStorage.StartAddress, deviceMemoryStorage);
    }

    public void AddMemoryBlock(ReadPartsSelection readSelection, uint startAddess, uint byteSize)
    {
      DeviceMemoryType deviceMemoryType = DeviceMemoryType.FLASH;
      if ((readSelection & ReadPartsSelection.RAM_range) > ~(ReadPartsSelection.Dump | ReadPartsSelection.ProtocolOnlyMode))
        deviceMemoryType = DeviceMemoryType.DataRAM;
      DeviceMemoryStorage deviceMemoryStorage = new DeviceMemoryStorage(deviceMemoryType, startAddess, byteSize);
      deviceMemoryStorage.ReadSelection = readSelection;
      this.MemoryBlockList.Add(deviceMemoryStorage.StartAddress, deviceMemoryStorage);
    }

    public void AddMemoryBlock(
      DeviceMemoryType deviceMemoryType,
      uint startAddess,
      uint byteSize,
      uint writeSplitSize)
    {
      DeviceMemoryStorage deviceMemoryStorage = new DeviceMemoryStorage(deviceMemoryType, startAddess, byteSize, writeSplitSize);
      this.MemoryBlockList.Add(deviceMemoryStorage.StartAddress, deviceMemoryStorage);
    }

    public void AddMemoryBlock(DeviceMemoryType deviceMemoryType, AddressRange addressRangeObject)
    {
      if (addressRangeObject == null)
        return;
      this.AddMemoryBlock(deviceMemoryType, addressRangeObject.StartAddress, addressRangeObject.ByteSize);
    }

    public void AddMemoryBlock(
      DeviceMemoryType deviceMemoryType,
      AddressRange addressRangeObject,
      uint writeSplitSize)
    {
      this.AddMemoryBlock(deviceMemoryType, addressRangeObject.StartAddress, addressRangeObject.ByteSize, writeSplitSize);
    }

    public void GarantMemoryAvailable(AddressRange setRange)
    {
      if (this.AreMemoryBlocksAvailable(setRange))
        return;
      AddressRange theRange = new AddressRange(setRange.StartAddress == 0U ? 0U : setRange.StartAddress - 1U, setRange.ByteSize + 2U);
      SortedList<uint, DeviceMemoryStorage> storagesForAddressRange = this.GetDeviceMemoryStoragesForAddressRange(theRange);
      if (storagesForAddressRange.Count == 0)
      {
        DeviceMemoryStorage deviceMemoryStorage = new DeviceMemoryStorage(DeviceMemoryType.Unknown, setRange.StartAddress, setRange.ByteSize);
        this.MemoryBlockList.Add(deviceMemoryStorage.StartAddress, deviceMemoryStorage);
      }
      else
      {
        if (storagesForAddressRange.Keys[0] > setRange.StartAddress)
        {
          DeviceMemoryStorage partStorage = storagesForAddressRange.Values[0];
          DeviceMemoryStorage deviceMemoryStorage = new DeviceMemoryStorage(partStorage.MemoryType, setRange.StartAddress, (uint) ((int) partStorage.EndAddress - (int) setRange.StartAddress + 1));
          deviceMemoryStorage.IncludeDataFromPartStorage(partStorage);
          this.MemoryBlockList.Remove(partStorage.StartAddress);
          this.MemoryBlockList.Add(deviceMemoryStorage.StartAddress, deviceMemoryStorage);
          storagesForAddressRange = this.GetDeviceMemoryStoragesForAddressRange(theRange);
        }
        for (int index = 0; index < storagesForAddressRange.Count - 1; ++index)
        {
          if (storagesForAddressRange.Values[index].EndAddress < storagesForAddressRange.Values[index + 1].StartAddress)
          {
            DeviceMemoryStorage deviceMemoryStorage = new DeviceMemoryStorage(storagesForAddressRange.Values[index].MemoryType, storagesForAddressRange.Values[index].StartAddress, storagesForAddressRange.Values[index + 1].StartAddress - storagesForAddressRange.Values[index].StartAddress, storagesForAddressRange.Values[index].WriteSplitSize);
            deviceMemoryStorage.IncludeDataFromPartStorage(storagesForAddressRange.Values[index]);
            this.MemoryBlockList.Remove(storagesForAddressRange.Keys[index]);
            this.MemoryBlockList.Add(deviceMemoryStorage.StartAddress, deviceMemoryStorage);
          }
        }
        DeviceMemoryStorage partStorage1 = storagesForAddressRange.Values[storagesForAddressRange.Count - 1];
        if (partStorage1.EndAddress >= setRange.EndAddress)
          return;
        DeviceMemoryStorage deviceMemoryStorage1 = new DeviceMemoryStorage(partStorage1.MemoryType, partStorage1.StartAddress, (uint) ((int) setRange.EndAddress - (int) partStorage1.StartAddress + 1), partStorage1.WriteSplitSize);
        deviceMemoryStorage1.IncludeDataFromPartStorage(partStorage1);
        this.MemoryBlockList.Remove(partStorage1.StartAddress);
        this.MemoryBlockList.Add(deviceMemoryStorage1.StartAddress, deviceMemoryStorage1);
      }
    }

    private DeviceMemoryStorage GetDeviceMemoryStorageForAddress(uint address)
    {
      return this.MemoryBlockList.Values.FirstOrDefault<DeviceMemoryStorage>((System.Func<DeviceMemoryStorage, bool>) (x => address >= x.StartAddress && address <= x.EndAddress));
    }

    private DeviceMemoryStorage GetDeviceMemoryStorageForAddressRange(AddressRange theRange)
    {
      return this.MemoryBlockList.Values.FirstOrDefault<DeviceMemoryStorage>((System.Func<DeviceMemoryStorage, bool>) (x => theRange.StartAddress >= x.StartAddress && theRange.EndAddress <= x.EndAddress));
    }

    private DeviceMemoryStorage GetDeviceMemoryStorageForAddressRange(uint address, uint byteSize)
    {
      return this.GetDeviceMemoryStorageForAddressRange(new AddressRange(address, byteSize));
    }

    private SortedList<uint, DeviceMemoryStorage> GetDeviceMemoryStoragesForAddressRange(
      AddressRange theRange)
    {
      SortedList<uint, DeviceMemoryStorage> storagesForAddressRange = new SortedList<uint, DeviceMemoryStorage>();
      foreach (DeviceMemoryStorage deviceMemoryStorage in this.MemoryBlockList.Values.Where<DeviceMemoryStorage>((System.Func<DeviceMemoryStorage, bool>) (x =>
      {
        if (theRange.StartAddress >= x.StartAddress && theRange.StartAddress <= x.EndAddress || theRange.EndAddress >= x.StartAddress && theRange.EndAddress <= x.EndAddress)
          return true;
        return x.StartAddress >= theRange.StartAddress && x.StartAddress <= theRange.EndAddress;
      })))
        storagesForAddressRange.Add(deviceMemoryStorage.StartAddress, deviceMemoryStorage);
      return storagesForAddressRange;
    }

    public DeviceMemoryStorage GetDeviceMemoryTypeForData(
      uint address,
      uint byteSize,
      out string message)
    {
      try
      {
        DeviceMemoryStorage memoryTypeForData = (DeviceMemoryStorage) null;
        message = string.Empty;
        uint num = (uint) ((int) address + (int) byteSize - 1);
        foreach (DeviceMemoryStorage deviceMemoryStorage in (IEnumerable<DeviceMemoryStorage>) this.MemoryBlockList.Values)
        {
          if (address >= deviceMemoryStorage.StartAddress && num <= deviceMemoryStorage.EndAddress)
          {
            memoryTypeForData = deviceMemoryStorage;
            for (int index1 = 0; (long) index1 < (long) byteSize; ++index1)
            {
              int index2 = (int) ((long) (address - deviceMemoryStorage.StartAddress) + (long) index1);
              if (!deviceMemoryStorage.DataAvailable[index2])
                message = "Memory data not available... (E0)";
            }
            return memoryTypeForData;
          }
          if (address >= deviceMemoryStorage.StartAddress && address <= deviceMemoryStorage.EndAddress && num > deviceMemoryStorage.EndAddress)
          {
            memoryTypeForData = deviceMemoryStorage;
            message = "memory out of range!!! (E1)";
            return memoryTypeForData;
          }
          if (address >= deviceMemoryStorage.StartAddress && address <= deviceMemoryStorage.EndAddress || num >= deviceMemoryStorage.StartAddress && num <= deviceMemoryStorage.EndAddress)
          {
            memoryTypeForData = deviceMemoryStorage;
            message = "memory out of range!!! (E1)";
            return memoryTypeForData;
          }
          if (address <= deviceMemoryStorage.StartAddress && num >= deviceMemoryStorage.EndAddress)
          {
            memoryTypeForData = deviceMemoryStorage;
            message = "memory collision in range!!! (E1)";
            return memoryTypeForData;
          }
        }
        if (memoryTypeForData == null)
          message = "Out of defined memory blocks. (E2)";
        return memoryTypeForData;
      }
      catch (Exception ex)
      {
        message = "Out of defined memory blocks. (E2)";
        return (DeviceMemoryStorage) null;
      }
    }

    private bool AreMemoryBlocksAvailable(AddressRange testRange)
    {
      for (DeviceMemoryStorage storageForAddress = this.GetDeviceMemoryStorageForAddress(testRange.StartAddress); storageForAddress != null; storageForAddress = this.GetDeviceMemoryStorageForAddress(storageForAddress.EndAddress + 1U))
      {
        if (storageForAddress.EndAddress >= testRange.EndAddress)
          return true;
      }
      return false;
    }

    public byte? GetByte(uint address)
    {
      DeviceMemoryStorage storageForAddress = this.GetDeviceMemoryStorageForAddress(address);
      if (storageForAddress == null)
        throw new Exception("Access outside of defined memory blocks. Address: 0x" + address.ToString("x08"));
      uint index = address - storageForAddress.StartAddress;
      return !storageForAddress.DataAvailable[(int) index] ? new byte?() : new byte?(storageForAddress.Data[(int) index]);
    }

    public byte[] GetData(AddressRange addressRange, bool returnAvailableStartData = false)
    {
      return this.GetData(addressRange.StartAddress, addressRange.ByteSize, returnAvailableStartData);
    }

    public byte[] GetData(uint address, uint byteSize, bool returnAvailableStartData = false)
    {
      List<byte> byteList = new List<byte>();
      DeviceMemoryStorage storageForAddress = this.GetDeviceMemoryStorageForAddress(address);
      if (storageForAddress == null)
        return returnAvailableStartData ? byteList.ToArray() : (byte[]) null;
      uint num = address + byteSize;
      for (uint address1 = address; address1 < num; ++address1)
      {
        if (address1 > storageForAddress.EndAddress)
        {
          storageForAddress = this.GetDeviceMemoryStorageForAddress(address1);
          if (storageForAddress == null)
            return returnAvailableStartData ? byteList.ToArray() : (byte[]) null;
        }
        uint index = address1 - storageForAddress.StartAddress;
        if (!storageForAddress.DataAvailable[(int) index])
          return returnAvailableStartData ? byteList.ToArray() : (byte[]) null;
        byteList.Add(storageForAddress.Data[(int) index]);
      }
      return byteList.ToArray();
    }

    public byte[] GetAvailableData(uint address, uint maxByteSize)
    {
      List<byte> byteList = new List<byte>();
      DeviceMemoryStorage storageForAddress = this.GetDeviceMemoryStorageForAddress(address);
      if (storageForAddress != null)
      {
        while ((long) byteList.Count < (long) maxByteSize)
        {
          if (address > storageForAddress.EndAddress)
          {
            storageForAddress = this.GetDeviceMemoryStorageForAddress(address);
            if (storageForAddress == null)
              break;
          }
          uint index = address - storageForAddress.StartAddress;
          if (storageForAddress.DataAvailable[(int) index])
          {
            byteList.Add(storageForAddress.Data[(int) index]);
            ++address;
          }
          else
            break;
        }
      }
      return byteList.ToArray();
    }

    public byte[] GetAvailableData(uint address)
    {
      DeviceMemoryStorage storageForAddress = this.GetDeviceMemoryStorageForAddress(address);
      if (storageForAddress == null)
        return new byte[0];
      uint index = address - storageForAddress.StartAddress;
      uint byteSize = storageForAddress.ByteSize;
      List<byte> byteList = new List<byte>();
      while (index < byteSize && storageForAddress.DataAvailable[(int) index])
        byteList.Add(storageForAddress.Data[(int) index++]);
      return byteList.ToArray();
    }

    public void InsertAvailableData(byte[] data, uint dataStartAddress)
    {
      uint address = dataStartAddress;
      int index1 = 0;
      DeviceMemoryStorage deviceMemoryStorage = (DeviceMemoryStorage) null;
      uint index2 = 0;
      for (; index1 < data.Length; ++index1)
      {
        if (deviceMemoryStorage == null)
        {
          deviceMemoryStorage = this.GetDeviceMemoryStorageForAddress(address);
          if (deviceMemoryStorage != null)
            index2 = address - deviceMemoryStorage.StartAddress;
        }
        if (deviceMemoryStorage != null)
        {
          if (deviceMemoryStorage.DataAvailable[(int) index2])
            data[index1] = deviceMemoryStorage.Data[(int) index2];
          ++index2;
          if (index2 >= deviceMemoryStorage.ByteSize)
            deviceMemoryStorage = (DeviceMemoryStorage) null;
        }
        ++address;
      }
    }

    public SortedList<string, string> GetDataForSection(string Section)
    {
      if (string.IsNullOrEmpty(Section))
        throw new ArgumentNullException("Argument Segment is NULL or EMPTY.");
      if (!((IEnumerable<string>) this.MapDef.SectionList).Contains<string>(Section))
        throw new ArgumentNullException("Section not in SectionList.");
      SortedList<string, string> dataForSection = new SortedList<string, string>();
      foreach (Parameter32bit parameter32bit in this.MapDef.GetAllParameterForSection(Section))
      {
        if (this.IsParameterAvailable(parameter32bit.Name))
        {
          int index = this.UsedParametersByName.IndexOfKey(parameter32bit.Name);
          byte[] data = this.GetData(this.UsedParametersByName.Values[index].Address, this.UsedParametersByName.Values[index].Size);
          dataForSection.Add(parameter32bit.Name, Util.ByteArrayToString(data));
        }
      }
      return dataForSection;
    }

    public string[] GetDataAsHexStrings(uint address, uint byteSize)
    {
      string[] dataAsHexStrings = new string[(int) byteSize];
      DeviceMemoryStorage deviceMemoryStorage = (DeviceMemoryStorage) null;
      uint num = address + byteSize;
      for (uint address1 = address; address1 < num; ++address1)
      {
        if (deviceMemoryStorage == null || address1 > deviceMemoryStorage.EndAddress)
          deviceMemoryStorage = this.GetDeviceMemoryStorageForAddress(address1);
        if (deviceMemoryStorage == null)
        {
          dataAsHexStrings[(int) address1 - (int) address] = "..";
        }
        else
        {
          uint index = address1 - deviceMemoryStorage.StartAddress;
          dataAsHexStrings[(int) address1 - (int) address] = deviceMemoryStorage.DataAvailable[(int) index] ? deviceMemoryStorage.Data[(int) index].ToString("x02") : "--";
        }
      }
      return dataAsHexStrings;
    }

    public void SetData(DeviceMemory memory)
    {
      foreach (DeviceMemoryStorage deviceMemoryStorage in (IEnumerable<DeviceMemoryStorage>) memory.MemoryBlockList.Values)
        this.SetData(deviceMemoryStorage.StartAddress, deviceMemoryStorage.Data);
    }

    public void SetData(uint address, byte[] data)
    {
      this.SetData(new AddressRange(address, (uint) data.Length), data);
    }

    public void SetData(AddressRange theRange, byte[] data)
    {
      try
      {
        uint startAddress = theRange.StartAddress;
        DeviceMemoryStorage deviceMemoryStorage = this.GetDeviceMemoryStorageForAddressRange(theRange) ?? this.GetDeviceMemoryStorageForAddress(startAddress);
        if (deviceMemoryStorage == null)
          throw new Exception("DeviceMemory::SetData management (CurrentStorage) error.");
        uint num = (uint) ((ulong) startAddress + (ulong) data.Length);
        for (uint address = startAddress; address < num; ++address)
        {
          if (address > deviceMemoryStorage.EndAddress)
            deviceMemoryStorage = this.GetDeviceMemoryStorageForAddress(address);
          uint index = address - deviceMemoryStorage.StartAddress;
          deviceMemoryStorage.Data[(int) index] = data[(int) address - (int) startAddress];
          deviceMemoryStorage.DataAvailable[(int) index] = true;
        }
      }
      catch (Exception ex)
      {
        throw new Exception("SetData management error.", ex);
      }
    }

    public bool AreDataAvailable(AddressRange testRange)
    {
      return testRange != null && this.AreDataAvailable(testRange.StartAddress, testRange.ByteSize);
    }

    public bool AreDataAvailable(uint address, uint byteSize)
    {
      uint num = (uint) ((int) address + (int) byteSize - 1);
      foreach (DeviceMemoryStorage deviceMemoryStorage in this.MemoryBlockList.Values.Where<DeviceMemoryStorage>((System.Func<DeviceMemoryStorage, bool>) (x => x.ByteSize >= byteSize)).ToList<DeviceMemoryStorage>())
      {
        if (address >= deviceMemoryStorage.StartAddress && num <= deviceMemoryStorage.EndAddress)
        {
          for (int index1 = 0; (long) index1 < (long) byteSize; ++index1)
          {
            int index2 = (int) ((long) (address - deviceMemoryStorage.StartAddress) + (long) index1);
            if (!deviceMemoryStorage.DataAvailable[index2])
              return false;
          }
          return true;
        }
      }
      return false;
    }

    public List<AddressRange> GetAvailableByteRanges(uint fromAddress, uint toAddress)
    {
      List<AddressRange> availableByteRanges = new List<AddressRange>();
      uint startAddress = fromAddress;
      AddressRange addressRange = (AddressRange) null;
      foreach (DeviceMemoryStorage deviceMemoryStorage in this.MemoryBlockList.Values.Where<DeviceMemoryStorage>((System.Func<DeviceMemoryStorage, bool>) (x => x.EndAddress >= fromAddress && x.StartAddress <= toAddress)))
      {
        if (startAddress < deviceMemoryStorage.StartAddress)
        {
          startAddress = deviceMemoryStorage.StartAddress;
          if (addressRange != null)
          {
            availableByteRanges.Add(addressRange);
            addressRange = (AddressRange) null;
          }
        }
        for (; startAddress <= deviceMemoryStorage.EndAddress && startAddress <= toAddress; ++startAddress)
        {
          int index = (int) startAddress - (int) deviceMemoryStorage.StartAddress;
          if (deviceMemoryStorage.DataAvailable[index])
          {
            if (addressRange == null)
              addressRange = new AddressRange(startAddress, 1U);
            else
              ++addressRange.ByteSize;
          }
          else if (addressRange != null)
          {
            availableByteRanges.Add(addressRange);
            addressRange = (AddressRange) null;
          }
        }
      }
      if (addressRange != null)
        availableByteRanges.Add(addressRange);
      return availableByteRanges;
    }

    public AddressRange GetNextAvailableDataRange(uint address)
    {
      DeviceMemoryStorage storageForAddress = this.GetDeviceMemoryStorageForAddress(address);
      if (storageForAddress == null)
        return (AddressRange) null;
      uint index = address - storageForAddress.StartAddress;
      uint byteSize = storageForAddress.ByteSize;
      uint startAddress = address;
      while (index < byteSize && !storageForAddress.DataAvailable[(int) index])
      {
        ++index;
        ++startAddress;
      }
      uint size = 0;
      for (; index < byteSize && storageForAddress.DataAvailable[(int) index]; ++index)
        ++size;
      return size == 0U ? (AddressRange) null : new AddressRange(startAddress, size);
    }

    public T GetParameterValue<T>(string parameterName)
    {
      int index = this.UsedParametersByName != null ? this.UsedParametersByName.IndexOfKey(parameterName) : throw new HandlerMessageException("MAP not included for this device and firmware.");
      if (index < 0)
        throw new Exception("Parameter not supported:" + parameterName);
      return this.UsedParametersByName.Values[index].GetValue<T>(this);
    }

    public byte[] GetData(string parameterName)
    {
      int index = this.UsedParametersByName.IndexOfKey(parameterName);
      if (index < 0)
        throw new Exception("Parameter not supported: " + parameterName);
      Parameter32bit parameter32bit = this.UsedParametersByName.Values[index];
      return this.GetData(parameter32bit.Address, parameter32bit.Size);
    }

    public AddressRange GetAddressRange(string parameterName)
    {
      SortedList<string, Parameter32bit> mapVars = this.MapDef.MapVars;
      return !mapVars.ContainsKey(parameterName) ? (AddressRange) null : mapVars[parameterName].AddressRange;
    }

    public byte[] GetDataForParameter(string parameterName)
    {
      return this.GetData(this.GetAddressRange(parameterName));
    }

    public void SetParameterValue<T>(string parameterName, T theValue)
    {
      int index = this.UsedParametersByName.IndexOfKey(parameterName);
      if (index < 0)
        throw new Exception("Parameter not supported:" + parameterName);
      this.UsedParametersByName.Values[index].SetValue<T>(theValue, this);
    }

    public void SetData(string parameterName, byte[] buffer)
    {
      int index = this.UsedParametersByName.IndexOfKey(parameterName);
      if (index < 0)
        throw new Exception("Parameter not supported: " + parameterName);
      this.SetData(this.UsedParametersByName.Values[index].Address, buffer);
    }

    public void OverwriteUsedParameters(DeviceMemory fromMemory, string[] parameterNameList)
    {
      string key = "";
      try
      {
        for (int index = 0; index < parameterNameList.Length; ++index)
        {
          key = parameterNameList[index];
          if (fromMemory.UsedParametersByName.ContainsKey(key) && this.UsedParametersByName.ContainsKey(key))
            this.SetData(this.UsedParametersByName[key].Address, fromMemory.GetData(fromMemory.UsedParametersByName[key].AddressRange) ?? throw new Exception("Source data not found for: " + key));
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Overwrite of parameter faild: " + key, ex);
      }
    }

    public uint GetParameterAddress(string parameterName)
    {
      int index = this.UsedParametersByName.IndexOfKey(parameterName);
      if (index < 0)
        throw new Exception("Parameter not supported:" + parameterName);
      return this.UsedParametersByName.Values[index].Address;
    }

    public AddressRange GetParameterAddressRange(string parameterName)
    {
      int index = this.UsedParametersByName.IndexOfKey(parameterName);
      return index < 0 ? (AddressRange) null : this.UsedParametersByName.Values[index].AddressRange;
    }

    public T GetValue<T>(uint address) => Parameter32bit.GetValue<T>(address, this);

    public void SetValue<T>(T value, uint address)
    {
      Parameter32bit.SetValue<T>(value, address, this);
    }

    public bool IsParameterAvailable(string parameterName)
    {
      if (this.UsedParametersByName == null)
        return false;
      int index = this.UsedParametersByName.IndexOfKey(parameterName);
      return index >= 0 && this.AreDataAvailable(this.UsedParametersByName.Values[index].Address, this.UsedParametersByName.Values[index].Size);
    }

    public bool IsParameterInMap(string parameterName)
    {
      return this.UsedParametersByName != null && this.UsedParametersByName.IndexOfKey(parameterName) >= 0;
    }

    public List<AddressRange> GetAllParameterReadingRanges(
      string[] excludeParameterNames,
      uint splitSize)
    {
      List<string> stringList = new List<string>();
      foreach (Parameter32bit parameter32bit in (IEnumerable<Parameter32bit>) this.MapDef.GetAllParametersList().Values)
      {
        if (!((IEnumerable<string>) excludeParameterNames).Contains<string>(parameter32bit.Name))
          stringList.Add(parameter32bit.Name);
      }
      return this.GetAllParameterReadingRanges(stringList.ToArray(), splitSize);
    }

    public List<AddressRange> GetDefinedParameterReadingRanges(
      string[] definedParameters,
      uint splitSize)
    {
      List<Parameter32bit> list1 = this.MapDef.GetAllParametersList().Values.ToList<Parameter32bit>();
      List<AddressRange> source = new List<AddressRange>();
      foreach (Parameter32bit parameter32bit in list1)
      {
        if (((IEnumerable<string>) definedParameters).Contains<string>(parameter32bit.Name))
          source.Add(parameter32bit.AddressRange);
      }
      List<AddressRange> list2 = source.OrderBy<AddressRange, uint>((System.Func<AddressRange, uint>) (x => x.StartAddress)).ToList<AddressRange>();
      int index = 0;
      while (index < list2.Count - 1)
      {
        if (this.GetDeviceMemoryStorageForAddressRange(new AddressRange(list2[index].EndAddress, (uint) ((int) list2[index + 1].StartAddress - (int) list2[index].EndAddress + 1))) != null && list2[index + 1].StartAddress - list2[index].EndAddress <= splitSize)
        {
          list2[index].EndAddress = list2[index + 1].EndAddress;
          list2.RemoveAt(index + 1);
        }
        else
          ++index;
      }
      return list2;
    }

    public List<AddressRange> GetChangedDataRanges(DeviceMemory changedFrom)
    {
      List<AddressRange> changedDataRanges = new List<AddressRange>();
      if (this.MemoryBlockList.Count != changedFrom.MemoryBlockList.Count)
        throw new Exception("GetChangedDataRanges has different count of MemoryBlocks");
      for (int index = 0; index < this.MemoryBlockList.Count; ++index)
      {
        if ((int) this.MemoryBlockList.Values[index].ByteSize != (int) changedFrom.MemoryBlockList.Values[index].ByteSize)
          throw new Exception("GetChangedDataRanges has different MemoryBlock ByteSize on block: " + index.ToString());
      }
      if (this.MemoryBlockList.Count > 0)
      {
        foreach (DeviceMemoryStorage deviceMemoryStorage1 in (IEnumerable<DeviceMemoryStorage>) this.MemoryBlockList.Values)
        {
          uint endAddress = deviceMemoryStorage1.EndAddress;
          DeviceMemoryStorage deviceMemoryStorage2 = deviceMemoryStorage1;
          AddressRange addressRange = (AddressRange) null;
          for (uint startAddress = deviceMemoryStorage1.StartAddress; startAddress <= endAddress; ++startAddress)
          {
            byte? nullable1 = this.GetByte(startAddress);
            byte? nullable2 = changedFrom.GetByte(startAddress);
            if (!nullable1.HasValue)
            {
              if (addressRange != null)
              {
                changedDataRanges.Add(addressRange);
                addressRange = (AddressRange) null;
              }
            }
            else if (!nullable2.HasValue || (int) nullable1.Value != (int) nullable2.Value)
            {
              if (addressRange == null)
                addressRange = new AddressRange(startAddress, 1U);
              else if (startAddress > addressRange.EndAddress + 1U + deviceMemoryStorage2.WriteSplitSize)
              {
                changedDataRanges.Add(addressRange);
                addressRange = new AddressRange(startAddress, 1U);
              }
              else
                addressRange.EndAddress = startAddress;
            }
          }
          if (addressRange != null)
            changedDataRanges.Add(addressRange);
        }
      }
      return changedDataRanges;
    }

    public List<AddressRange> GetExistingDataRanges()
    {
      List<AddressRange> existingDataRanges = new List<AddressRange>();
      if (this.MemoryBlockList.Count > 0)
      {
        foreach (DeviceMemoryStorage deviceMemoryStorage1 in (IEnumerable<DeviceMemoryStorage>) this.MemoryBlockList.Values)
        {
          uint endAddress = deviceMemoryStorage1.EndAddress;
          DeviceMemoryStorage deviceMemoryStorage2 = deviceMemoryStorage1;
          AddressRange addressRange = (AddressRange) null;
          for (uint startAddress = deviceMemoryStorage1.StartAddress; startAddress <= endAddress; ++startAddress)
          {
            if (!this.GetByte(startAddress).HasValue)
            {
              if (addressRange != null)
              {
                existingDataRanges.Add(addressRange);
                addressRange = (AddressRange) null;
              }
            }
            else if (addressRange == null)
              addressRange = new AddressRange(startAddress, 1U);
            else if (startAddress > addressRange.EndAddress + 1U + deviceMemoryStorage2.WriteSplitSize)
            {
              existingDataRanges.Add(addressRange);
              addressRange = new AddressRange(startAddress, 1U);
            }
            else
              addressRange.EndAddress = startAddress;
          }
          if (addressRange != null)
            existingDataRanges.Add(addressRange);
        }
      }
      return existingDataRanges;
    }

    public List<AddressRange> GetAllAddressRanges(DeviceMemoryType? memoryType = null)
    {
      List<AddressRange> allAddressRanges = new List<AddressRange>();
      foreach (DeviceMemoryStorage deviceMemoryStorage in (IEnumerable<DeviceMemoryStorage>) this.MemoryBlockList.Values)
      {
        int num;
        if (memoryType.HasValue)
        {
          int memoryType1 = (int) deviceMemoryStorage.MemoryType;
          DeviceMemoryType? nullable = memoryType;
          int valueOrDefault = (int) nullable.GetValueOrDefault();
          num = memoryType1 == valueOrDefault & nullable.HasValue ? 1 : 0;
        }
        else
          num = 1;
        if (num != 0)
          allAddressRanges.Add(new AddressRange(deviceMemoryStorage.StartAddress, deviceMemoryStorage.ByteSize));
      }
      return allAddressRanges;
    }

    public List<AddressRangeInfo> GetAllAddressRangeInfos(DeviceMemoryType? memoryType = null)
    {
      List<AddressRangeInfo> addressRangeInfos = new List<AddressRangeInfo>();
      foreach (DeviceMemoryStorage deviceMemoryStorage in (IEnumerable<DeviceMemoryStorage>) this.MemoryBlockList.Values)
      {
        int num;
        if (memoryType.HasValue)
        {
          int memoryType1 = (int) deviceMemoryStorage.MemoryType;
          DeviceMemoryType? nullable = memoryType;
          int valueOrDefault = (int) nullable.GetValueOrDefault();
          num = memoryType1 == valueOrDefault & nullable.HasValue ? 1 : 0;
        }
        else
          num = 1;
        if (num != 0)
          addressRangeInfos.Add(new AddressRangeInfo(deviceMemoryStorage.MemoryType.ToString() + " WriteSplitSize:" + deviceMemoryStorage.WriteSplitSize.ToString(), deviceMemoryStorage.GetAddressRange()));
      }
      return addressRangeInfos;
    }

    public List<AddressRange> GetRangesForRead(List<string> ignoreParameter, int maxGapSize)
    {
      IList<Parameter32bit> values = this.MapDef.GetAllParametersList().Values;
      List<AddressRange> rangesForRead = new List<AddressRange>();
      int num1 = 0;
      uint startAddress = 0;
      uint num2 = 0;
      foreach (DeviceMemoryStorage deviceMemoryStorage in (IEnumerable<DeviceMemoryStorage>) this.MemoryBlockList.Values)
      {
        uint num3 = (uint) ((int) deviceMemoryStorage.StartAddress + (int) deviceMemoryStorage.ByteSize - 1);
        uint address = deviceMemoryStorage.StartAddress;
        while (address <= num3)
        {
          Parameter32bit parameter32bit = values.FirstOrDefault<Parameter32bit>((System.Func<Parameter32bit, bool>) (x => (int) x.Address == (int) address && !ignoreParameter.Contains(x.Name)));
          if (parameter32bit != null)
          {
            if (startAddress == 0U)
            {
              startAddress = parameter32bit.Address;
              num1 = 0;
            }
            num2 = parameter32bit.EndAddress;
            address += parameter32bit.Size;
          }
          else
          {
            ++num1;
            address++;
          }
          if (startAddress > 0U && num1 > maxGapSize)
          {
            rangesForRead.Add(new AddressRange(startAddress, (uint) ((int) num2 - (int) startAddress + 1)));
            startAddress = 0U;
            num2 = 0U;
            num1 = 0;
          }
        }
        if (startAddress > 0U)
        {
          rangesForRead.Add(new AddressRange(startAddress, (uint) ((int) num2 - (int) startAddress + 1)));
          startAddress = 0U;
          num2 = 0U;
          num1 = 0;
        }
      }
      return rangesForRead;
    }

    private void CreateFromCompressedData(byte[] compressedData)
    {
      byte[] array;
      using (MemoryStream memoryStream = new MemoryStream(compressedData))
      {
        using (MemoryStream destination = new MemoryStream())
        {
          using (GZipStream gzipStream = new GZipStream((Stream) memoryStream, CompressionMode.Decompress))
          {
            gzipStream.CopyTo((Stream) destination);
            gzipStream.Flush();
          }
          array = destination.ToArray();
        }
      }
      StringBuilder stringBuilder = new StringBuilder();
      int num1 = 0;
      byte[] numArray1 = array;
      int index1 = num1;
      int startIndex1 = index1 + 1;
      if (numArray1[index1] > (byte) 0)
        throw new Exception("Illegal compressed format");
      this.FirmwareVersion = BitConverter.ToUInt32(array, startIndex1);
      int num2 = startIndex1 + 4;
      byte[] numArray2 = array;
      int index2 = num2;
      int srcOffset = index2 + 1;
      int num3 = (int) numArray2[index2];
      List<DeviceMemoryStorage> deviceMemoryStorageList = new List<DeviceMemoryStorage>();
      for (int index3 = 0; index3 < num3; ++index3)
      {
        stringBuilder.Clear();
        byte[] numArray3 = array;
        int index4 = srcOffset;
        int startIndex2 = index4 + 1;
        int num4 = (int) numArray3[index4];
        for (int index5 = 0; index5 < num4; ++index5)
          stringBuilder.Append((char) array[startIndex2++]);
        DeviceMemoryType result;
        Enum.TryParse<DeviceMemoryType>(stringBuilder.ToString(), out result);
        uint uint32_1 = BitConverter.ToUInt32(array, startIndex2);
        int startIndex3 = startIndex2 + 4;
        uint uint32_2 = BitConverter.ToUInt32(array, startIndex3);
        srcOffset = startIndex3 + 4;
        deviceMemoryStorageList.Add(new DeviceMemoryStorage(result, uint32_1, uint32_2));
      }
      foreach (DeviceMemoryStorage deviceMemoryStorage in deviceMemoryStorageList)
      {
        Buffer.BlockCopy((Array) array, srcOffset, (Array) deviceMemoryStorage.Data, 0, (int) deviceMemoryStorage.ByteSize);
        srcOffset += (int) deviceMemoryStorage.ByteSize;
        byte num5 = 0;
        for (int index6 = 0; (long) index6 < (long) deviceMemoryStorage.ByteSize; ++index6)
        {
          if ((index6 & 7) == 0)
            num5 = array[srcOffset++];
          else
            num5 >>= 1;
          if (((int) num5 & 1) == 1)
            deviceMemoryStorage.DataAvailable[index6] = true;
        }
      }
      foreach (DeviceMemoryStorage deviceMemoryStorage in deviceMemoryStorageList)
      {
        if (!this.MemoryBlockList.ContainsKey(deviceMemoryStorage.StartAddress))
          this.MemoryBlockList.Add(deviceMemoryStorage.StartAddress, deviceMemoryStorage);
      }
    }

    public byte[] GetCompressedData()
    {
      List<byte> byteList = new List<byte>();
      int num = 0;
      byteList.Add((byte) 0);
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.FirmwareVersion));
      byteList.Add((byte) this.MemoryBlockList.Count);
      List<byte[]> numArrayList = new List<byte[]>();
      for (int index1 = 0; index1 < this.MemoryBlockList.Count; ++index1)
      {
        DeviceMemoryStorage deviceMemoryStorage = this.MemoryBlockList.Values[index1];
        byte[] numArray = DeviceMemory.PackBoolArray(deviceMemoryStorage.DataAvailable);
        numArrayList.Add(numArray);
        num = num + deviceMemoryStorage.Data.Length + numArray.Length;
        string str = deviceMemoryStorage.MemoryType.ToString();
        byte[] collection = new byte[str.Length + 1];
        collection[0] = (byte) str.Length;
        for (int index2 = 0; index2 < str.Length; ++index2)
          collection[index2 + 1] = (byte) str[index2];
        byteList.AddRange((IEnumerable<byte>) collection);
        byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(deviceMemoryStorage.StartAddress));
        byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(deviceMemoryStorage.Data.Length));
      }
      byte[] numArray1 = new byte[byteList.Count + num];
      Buffer.BlockCopy((Array) byteList.ToArray(), 0, (Array) numArray1, 0, byteList.Count);
      int dstOffset1 = byteList.Count;
      for (int index = 0; index < this.MemoryBlockList.Count; ++index)
      {
        DeviceMemoryStorage deviceMemoryStorage = this.MemoryBlockList.Values[index];
        Buffer.BlockCopy((Array) deviceMemoryStorage.Data, 0, (Array) numArray1, dstOffset1, deviceMemoryStorage.Data.Length);
        int dstOffset2 = dstOffset1 + deviceMemoryStorage.Data.Length;
        byte[] src = numArrayList[index];
        Buffer.BlockCopy((Array) src, 0, (Array) numArray1, dstOffset2, src.Length);
        dstOffset1 = dstOffset2 + src.Length;
      }
      byte[] array;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (GZipStream destination = new GZipStream((Stream) memoryStream, CompressionMode.Compress))
        {
          new MemoryStream(numArray1).CopyTo((Stream) destination);
          destination.Flush();
        }
        array = memoryStream.ToArray();
      }
      return array;
    }

    public static byte[] PackBoolArray(bool[] boolArray)
    {
      List<byte> byteList = new List<byte>();
      int num1 = 256;
      int num2 = 0;
      bool flag = false;
      for (int index = 0; index < boolArray.Length; ++index)
      {
        if (num1 >= 256)
        {
          if (flag)
            byteList.Add((byte) num2);
          num2 = 0;
          num1 = 1;
        }
        if (boolArray[index])
          num2 |= num1;
        flag = true;
        num1 <<= 1;
      }
      if (flag)
        byteList.Add((byte) num2);
      return byteList.ToArray();
    }

    public void SaveDeviceToDatabase(
      BaseDbConnection db,
      DbConnection meterUpdateCon,
      DbTransaction TheTransaction,
      DeviceIdentification DeviceId,
      byte[] CompressedMemory)
    {
      DbDataAdapter dataAdapter = db.GetDataAdapter("SELECT * FROM MeterData", meterUpdateCon, TheTransaction, out DbCommandBuilder _);
      BaseTables.MeterDataDataTable meterDataDataTable = new BaseTables.MeterDataDataTable();
      BaseTables.MeterDataRow row = meterDataDataTable.NewMeterDataRow();
      row.MeterID = (int) DeviceId.MeterID.Value;
      uint? nullable;
      int num1;
      if (DeviceId.FirmwareVersion.HasValue)
      {
        nullable = DeviceId.FirmwareVersion;
        num1 = (int) nullable.Value;
      }
      else
        num1 = -1;
      uint num2 = (uint) num1;
      nullable = DeviceId.HardwareTypeID;
      if (nullable.HasValue)
      {
        nullable = DeviceId.HardwareTypeID;
        -1 = (int) nullable.Value;
      }
      row.PValue = "0x" + num2.ToString("x8");
      DateTime now = DateTime.Now;
      row.TimePoint = now.AddMilliseconds((double) (now.Millisecond * -1));
      row.PValueID = 1;
      row.PValueBinary = CompressedMemory;
      meterDataDataTable.AddMeterDataRow(row);
      dataAdapter.Update((DataTable) meterDataDataTable);
    }

    public void ShowParameterInfo(
      string fileTitel,
      string originalTag,
      bool SuppressAddresses = false,
      bool SuppressKnownDifferences = false,
      CommonOverwriteGroups[] overwrites = null)
    {
      string str = this.WriteParameterInfoLogFile(fileTitel + "_" + originalTag, originalTag, SuppressAddresses, SuppressKnownDifferences, overwrites);
      new Process() { StartInfo = { FileName = str } }.Start();
    }

    public void CompareParameterInfo(
      string fileTitel,
      string originalTag,
      string compareTag,
      DeviceMemory compareMemory,
      bool SuppressAddresses = false,
      bool SuppressKnownDifferences = false,
      CommonOverwriteGroups[] overwrites = null)
    {
      string str1 = this.WriteParameterInfoLogFile(fileTitel + "_" + originalTag, originalTag, SuppressAddresses, SuppressKnownDifferences, overwrites);
      string str2 = compareMemory.WriteParameterInfoLogFile(fileTitel + "_" + compareTag, compareTag, SuppressAddresses, SuppressKnownDifferences, overwrites);
      new Process()
      {
        StartInfo = {
          FileName = "TortoiseMerge",
          Arguments = ("/base:\"" + str1 + "\" /theirs:\"" + str2 + "\"")
        }
      }.Start();
    }

    public void CompareSortedParameterInfo(
      string fileTitel,
      string originalTag,
      string compareTag,
      DeviceMemory compareMemory)
    {
      string str1 = this.WriteSortedParameterInfoLogFile(fileTitel + "_p_org", originalTag);
      string str2 = compareMemory.WriteSortedParameterInfoLogFile(fileTitel + "_p_cmp", compareTag);
      new Process()
      {
        StartInfo = {
          FileName = "TortoiseMerge",
          Arguments = ("/base:\"" + str1 + "\" /theirs:\"" + str2 + "\"")
        }
      }.Start();
    }

    public void ShowParameterInfo(string HeaderInfo = null)
    {
      NotepadHelper.ShowMessage(this.GetParameterInfo(HeaderInfo), "DeviceMemory info");
    }

    public string WriteParameterInfoLogFile(
      string fileTitel,
      string HeaderInfo = null,
      bool SuppressAddresses = false,
      bool SuppressKnownDifferences = false,
      CommonOverwriteGroups[] overwrites = null)
    {
      if (fileTitel == null)
        throw new HandlerMessageException("File titel for DeviceMemory.SaveToFile not defined.");
      string parameterInfo = this.GetParameterInfo(HeaderInfo, SuppressAddresses, SuppressKnownDifferences, overwrites);
      string path2 = DateTime.Now.ToString("yyyyMMdd_HHmmss_FFFF_") + fileTitel + ".txt";
      string path = Path.Combine(SystemValues.LoggDataPath, path2);
      using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.ASCII))
      {
        streamWriter.Write(parameterInfo);
        streamWriter.Flush();
        streamWriter.Close();
      }
      return path;
    }

    public string WriteSortedParameterInfoLogFile(string fileTitel, string HeaderInfo = null)
    {
      if (fileTitel == null)
        throw new HandlerMessageException("File titel for DeviceMemory.SaveToFile not defined.");
      string sortedParameterInfo = this.GetSortedParameterInfo(HeaderInfo);
      string path2 = DateTime.Now.ToString("yyyyMMdd_HHmmss_FFFF_") + fileTitel + ".txt";
      string path = Path.Combine(SystemValues.LoggDataPath, path2);
      using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.ASCII))
      {
        streamWriter.Write(sortedParameterInfo);
        streamWriter.Flush();
        streamWriter.Close();
      }
      return path;
    }

    public virtual string GetParameterInfo(
      string HeaderInfo = null,
      bool SuppressAddresses = false,
      bool SuppressKnownDifferences = false,
      CommonOverwriteGroups[] overwrites = null)
    {
      List<KeyValuePair<string, Parameter32bit>> list = ((this.MapDef != null ? this.MapDef.GetAllParametersList() : throw new HandlerMessageException("Map in DeviceMemory.SaveToFile not defined.")) ?? throw new HandlerMessageException("No parameters defined for DeviceMemory.SaveToFile")).ToList<KeyValuePair<string, Parameter32bit>>();
      list.Sort((Comparison<KeyValuePair<string, Parameter32bit>>) ((a, b) => a.Value.Address.CompareTo(b.Value.Address)));
      StringBuilder sb = new StringBuilder();
      if (HeaderInfo != null)
        sb.AppendLine(HeaderInfo);
      sb.Append(new ZENNER.CommonLibrary.FirmwareVersion(this.FirmwareVersion).ToString());
      sb.AppendLine(" (" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ")");
      sb.AppendLine();
      sb.AppendLine("ADDRESS   SIZE   SECTION                    NAME                               DATA_HEX");
      sb.AppendLine("-------------------------------------------------------------------------------------");
      list.ForEach((Action<KeyValuePair<string, Parameter32bit>>) (x => sb.AppendFormat("{0:X8}  {1,-6} {2,-26} {3,-35} {4}", (object) x.Value.Address, (object) x.Value.Size, x.Value.Section == "UNKNOWN" ? (object) "-" : (object) x.Value.Section, (object) x.Value.Name, (object) this.GetParameterData(x.Value)).AppendLine()));
      return sb.ToString();
    }

    public string GetSortedParameterInfo(string HeaderInfo = null)
    {
      List<KeyValuePair<string, Parameter32bit>> list = ((this.MapDef != null ? this.MapDef.GetAllParametersList() : throw new HandlerMessageException("Map in DeviceMemory.SaveToFile not defined.")) ?? throw new HandlerMessageException("No parameters defined for DeviceMemory.SaveToFile")).ToList<KeyValuePair<string, Parameter32bit>>();
      list.Sort((Comparison<KeyValuePair<string, Parameter32bit>>) ((a, b) => a.Value.Name.CompareTo(b.Value.Name)));
      StringBuilder sb = new StringBuilder();
      if (HeaderInfo != null)
        sb.AppendLine(HeaderInfo);
      sb.Append(new ZENNER.CommonLibrary.FirmwareVersion(this.FirmwareVersion).ToString());
      sb.AppendLine(" (" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ")");
      sb.AppendLine();
      sb.AppendLine("NAME                               DATA_HEX");
      sb.AppendLine("-----------------------------------------");
      list.ForEach((Action<KeyValuePair<string, Parameter32bit>>) (x => sb.AppendFormat("{0,-35} {1}", (object) x.Value.Name, (object) this.GetParameterData(x.Value)).AppendLine()));
      return sb.ToString();
    }

    public string GetParameterData(Parameter32bit p)
    {
      return Utility.ByteArrayToHexString(Parameter32bit.GetValueByteArray(p.Address, p.Size, this));
    }

    public void CompareMemoryInfo(
      string fileTitel,
      string originalTag,
      string compareTag,
      DeviceMemory compareMemory)
    {
      string str1 = this.WriteMemoryInfoLogFile(fileTitel + "_m_org", originalTag);
      string str2 = compareMemory.WriteMemoryInfoLogFile(fileTitel + "_m_cmp", compareTag);
      new Process()
      {
        StartInfo = {
          FileName = "TortoiseMerge",
          Arguments = ("/base:\"" + str1 + "\" /theirs:\"" + str2 + "\"")
        }
      }.Start();
    }

    public void ShowMemoryInfo(string HeaderInfo = null)
    {
      NotepadHelper.ShowMessage(this.GetMemoryInfo(HeaderInfo), "DeviceMemory info");
    }

    public string WriteMemoryInfoLogFile(string fileTitel, string HeaderInfo = null)
    {
      if (fileTitel == null)
        throw new HandlerMessageException("File titel for DeviceMemory.SaveToFile not defined.");
      string memoryInfo = this.GetMemoryInfo(HeaderInfo);
      string path2 = DateTime.Now.ToString("yyyyMMdd_HHmmss_FFFF_") + fileTitel + ".txt";
      string path = Path.Combine(SystemValues.LoggDataPath, path2);
      using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.ASCII))
      {
        streamWriter.Write(memoryInfo);
        streamWriter.Flush();
        streamWriter.Close();
      }
      return path;
    }

    public string GetMemoryInfo(string HeaderInfo = null)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (HeaderInfo != null)
        stringBuilder.AppendLine(HeaderInfo);
      stringBuilder.Append(new ZENNER.CommonLibrary.FirmwareVersion(this.FirmwareVersion).ToString());
      stringBuilder.Append(" (" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ")");
      for (int index1 = 0; index1 < this.MemoryBlockList.Count; ++index1)
      {
        DeviceMemoryStorage deviceMemoryStorage = this.MemoryBlockList.Values[index1];
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.Append("Block: 0x" + deviceMemoryStorage.StartAddress.ToString("x08") + " to 0x" + deviceMemoryStorage.EndAddress.ToString("x08") + " ; WriteSplitSize = " + deviceMemoryStorage.WriteSplitSize.ToString());
        for (uint index2 = deviceMemoryStorage.StartAddress & 4294967280U; index2 <= deviceMemoryStorage.EndAddress; ++index2)
        {
          if (((int) index2 & 15) == 0)
          {
            stringBuilder.AppendLine();
            stringBuilder.Append(index2.ToString("x08") + ": ");
          }
          else if (((int) index2 & 3) == 0)
            stringBuilder.Append(".");
          else
            stringBuilder.Append(" ");
          if (index2 < deviceMemoryStorage.StartAddress)
          {
            stringBuilder.Append("  ");
          }
          else
          {
            uint index3 = index2 - deviceMemoryStorage.StartAddress;
            if (!deviceMemoryStorage.DataAvailable[(int) index3])
              stringBuilder.Append("--");
            else
              stringBuilder.Append(deviceMemoryStorage.Data[(int) index3].ToString("x02"));
          }
        }
      }
      return stringBuilder.ToString();
    }

    public void ShowUsedParameterRanges()
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      if (this.UsedParametersByName == null || this.UsedParametersByName.Count == 0)
      {
        stringBuilder1.AppendLine("No parameters defined");
      }
      else
      {
        List<Parameter32bit> list = this.UsedParametersByName.Values.ToList<Parameter32bit>();
        list.Sort();
        foreach (Parameter32bit parameter32bit in list)
        {
          StringBuilder stringBuilder2 = stringBuilder1;
          string[] strArray = new string[8];
          strArray[0] = "0x";
          uint num = parameter32bit.Address;
          strArray[1] = num.ToString("x08");
          strArray[2] = " - 0x";
          num = parameter32bit.EndAddress;
          strArray[3] = num.ToString("x08");
          strArray[4] = "; size: 0x";
          num = parameter32bit.Size;
          strArray[5] = num.ToString();
          strArray[6] = "; ";
          strArray[7] = parameter32bit.Name;
          string str = string.Concat(strArray);
          stringBuilder2.AppendLine(str);
        }
      }
      GmmMessage.Show_Ok(stringBuilder1.ToString(), "Used handler variables and ranges");
    }
  }
}
