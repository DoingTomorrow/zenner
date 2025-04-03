// Decompiled with JetBrains decompiler
// Type: S3_Handler.ParameterList
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;
using System.Collections.Generic;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class ParameterList
  {
    private static S3_ParameterLoader paramDefines = new S3_ParameterLoader();
    private S3_Meter MyMeter;
    internal SortedList<string, S3_Parameter> ParameterByName;
    internal SortedList<int, S3_Parameter> ParameterByAddress;
    internal SortedList<string, int> AddressLables;
    private DynamicParameterRange dynamicRAM_ParameterRange;

    internal ParameterList(S3_Meter MyMeter)
    {
      this.MyMeter = MyMeter;
      this.ParameterByName = new SortedList<string, S3_Parameter>();
      this.ParameterByAddress = new SortedList<int, S3_Parameter>();
      this.AddressLables = new SortedList<string, int>();
    }

    internal ParameterList Clone(S3_Meter CloneMeter)
    {
      ParameterList parameterList = new ParameterList(CloneMeter);
      foreach (S3_Parameter s3Parameter1 in (IEnumerable<S3_Parameter>) this.ParameterByAddress.Values)
      {
        if (s3Parameter1.IsHardLinkedAddress)
        {
          try
          {
            S3_Parameter s3Parameter2 = s3Parameter1.Clone(CloneMeter, s3Parameter1.BlockStartAddress);
            parameterList.ParameterByName.Add(s3Parameter2.Name, s3Parameter2);
            parameterList.ParameterByAddress.Add(s3Parameter2.BlockStartAddress, s3Parameter2);
          }
          catch (Exception ex)
          {
            ZR_ClassLibMessages.AddWarning("Parameter: " + s3Parameter1.Name + " not included! Error: " + ex.Message);
          }
        }
      }
      parameterList.AddressLables = this.AddressLables;
      return parameterList;
    }

    internal void AddParameter(S3_Parameter TheParameter)
    {
      string key = TheParameter.SubDevice != 0 ? TheParameter.SubDevice.ToString() + ":" + TheParameter.Name : TheParameter.Name;
      if (this.ParameterByAddress.ContainsKey(TheParameter.BlockStartAddress) && this.ParameterByAddress[TheParameter.BlockStartAddress].Name == TheParameter.Name)
        return;
      this.ParameterByName.Add(key, TheParameter);
      this.ParameterByAddress.Add(TheParameter.BlockStartAddress, TheParameter);
    }

    internal S3_Parameter AddNewParameterFromName(string ParameterName, int AddressFromMap)
    {
      S3_Parameter s3Parameter1 = S3_ParameterLoader.GetS3Parameter(this.MyMeter, ParameterName);
      if (s3Parameter1 == null)
        return (S3_Parameter) null;
      if (s3Parameter1.ByteSize == 0)
      {
        this.AddressLables.Add(s3Parameter1.Name, AddressFromMap);
        return (S3_Parameter) null;
      }
      s3Parameter1.BlockStartAddress = AddressFromMap;
      s3Parameter1.IsHardLinkedAddress = true;
      this.ParameterByName.Add(s3Parameter1.Name, s3Parameter1);
      this.ParameterByAddress.Add(s3Parameter1.BlockStartAddress, s3Parameter1);
      if (s3Parameter1.Name == S3_ParameterNames.SerDev0_Medium_Generation.ToString())
      {
        S3_Parameter s3Parameter2 = s3Parameter1.Clone(this.MyMeter);
        s3Parameter2.Name = S3_ParameterNames.SerDev0_Medium.ToString();
        s3Parameter2.IsHardLinkedAddress = false;
        s3Parameter2.BlockStartAddress = s3Parameter1.BlockStartAddress + 1;
        s3Parameter2.IsHardLinkedAddress = true;
        s3Parameter2.IsNotLinked = true;
        s3Parameter2.ByteSize = 1;
        s3Parameter2.SegmentType = s3Parameter1.SegmentType;
        s3Parameter2.Alignment = 1;
        s3Parameter2.Statics.ParameterStorageType = typeof (byte);
        this.ParameterByName.Add(s3Parameter2.Name, s3Parameter2);
        this.ParameterByAddress.Add(s3Parameter2.BlockStartAddress, s3Parameter2);
      }
      return s3Parameter1;
    }

    internal S3_Parameter AddNewHeapParameterByName(
      string ParameterName,
      S3_MemoryBlock parantMemoryBlock)
    {
      S3_Parameter s3Parameter = S3_ParameterLoader.GetS3Parameter(this.MyMeter, ParameterName);
      if (s3Parameter == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Parameter " + ParameterName + " not found in database", S3_Meter.S3_MeterLogger);
        return (S3_Parameter) null;
      }
      s3Parameter.parentMemoryBlock = parantMemoryBlock;
      parantMemoryBlock.Add(S3_MemorySegment.RuntimeVars, (S3_MemoryBlock) s3Parameter);
      this.ParameterByName.Add(s3Parameter.Name, s3Parameter);
      if (!this.ParameterByAddress.ContainsKey(s3Parameter.BlockStartAddress))
        this.ParameterByAddress.Add(s3Parameter.BlockStartAddress, s3Parameter);
      return s3Parameter;
    }

    internal S3_Parameter AddNewHeapParameterByName(
      string ParameterName,
      S3_MemoryBlock parantMemoryBlock,
      S3_Meter sourceMeter)
    {
      S3_Parameter memoryBlockToAdd;
      if (sourceMeter.MyParameters.ParameterByName.ContainsKey(ParameterName))
      {
        S3_Parameter s3Parameter = sourceMeter.MyParameters.ParameterByName[ParameterName];
        memoryBlockToAdd = s3Parameter.Clone(this.MyMeter);
        memoryBlockToAdd.sourceMemoryBlock = (S3_MemoryBlock) s3Parameter;
      }
      else
      {
        memoryBlockToAdd = S3_ParameterLoader.GetS3Parameter(this.MyMeter, ParameterName);
        if (memoryBlockToAdd == null)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Parameter " + ParameterName + " not found in database", S3_Meter.S3_MeterLogger);
          return (S3_Parameter) null;
        }
      }
      memoryBlockToAdd.parentMemoryBlock = parantMemoryBlock;
      parantMemoryBlock.Add(S3_MemorySegment.RuntimeVars, (S3_MemoryBlock) memoryBlockToAdd);
      this.ParameterByName.Add(memoryBlockToAdd.Name, memoryBlockToAdd);
      if (!this.ParameterByAddress.ContainsKey(memoryBlockToAdd.BlockStartAddress))
        this.ParameterByAddress.Add(memoryBlockToAdd.BlockStartAddress, memoryBlockToAdd);
      return memoryBlockToAdd;
    }

    internal bool RecreateParameterByAddress()
    {
      this.ParameterByAddress.Clear();
      foreach (S3_Parameter s3Parameter in (IEnumerable<S3_Parameter>) this.ParameterByName.Values)
        this.ParameterByAddress.Add(s3Parameter.BlockStartAddress, s3Parameter);
      return true;
    }

    internal bool RemoveHeapParameterByName(string ParameterName)
    {
      int index = this.ParameterByName.IndexOfKey(ParameterName);
      if (index < 0)
        return true;
      try
      {
        S3_Parameter s3Parameter = this.ParameterByName.Values[index];
        s3Parameter.RemoveFromParentMemoryBlock();
        this.ParameterByAddress.Remove(s3Parameter.BlockStartAddress);
        this.ParameterByName.RemoveAt(index);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription("Exception: " + ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Error on delete heap parameter " + ParameterName, S3_Meter.S3_MeterLogger);
        return false;
      }
      return true;
    }

    private bool GarantDynamicParameterRangeLoaded()
    {
      if (this.dynamicRAM_ParameterRange != null)
        return true;
      this.dynamicRAM_ParameterRange = new DynamicParameterRange();
      foreach (S3_Parameter s3Parameter in (IEnumerable<S3_Parameter>) this.ParameterByName.Values)
      {
        if (s3Parameter.Statics != null && s3Parameter.Statics.IsDynamicRAM_Parameter)
        {
          int blockStartAddress = s3Parameter.BlockStartAddress;
          int num = blockStartAddress + s3Parameter.ByteSize - 1;
          if (this.dynamicRAM_ParameterRange.minAddress > blockStartAddress)
            this.dynamicRAM_ParameterRange.minAddress = blockStartAddress;
          if (this.dynamicRAM_ParameterRange.maxAddress < num)
            this.dynamicRAM_ParameterRange.maxAddress = num;
        }
      }
      return true;
    }

    internal bool GetParameterRange(string[] paramList, out DynamicParameterRange theRange)
    {
      theRange = new DynamicParameterRange();
      for (int index = 0; index < paramList.Length; ++index)
      {
        if (this.ParameterByName.ContainsKey(paramList[index]))
        {
          S3_Parameter s3Parameter = this.ParameterByName[paramList[index]];
          int blockStartAddress = s3Parameter.BlockStartAddress;
          int num = blockStartAddress + s3Parameter.ByteSize - 1;
          if (theRange.minAddress > blockStartAddress)
            theRange.minAddress = blockStartAddress;
          if (theRange.maxAddress < num)
            theRange.maxAddress = num;
        }
      }
      return true;
    }

    internal bool IsDynamicParameter(int address)
    {
      this.GarantDynamicParameterRangeLoaded();
      return address >= this.dynamicRAM_ParameterRange.minAddress && address <= this.dynamicRAM_ParameterRange.maxAddress;
    }

    internal bool GetRefreshedTemperatures(out float flowTemp, out float returnTemp)
    {
      flowTemp = 0.0f;
      returnTemp = 0.0f;
      if (!this.MyMeter.MyFunctions.MyCommands.AdcTestCycleWithSimulatedVolume(0.0f))
        return false;
      S3_Parameter s3Parameter1 = this.ParameterByName["vorlauftemperatur"];
      S3_Parameter s3Parameter2 = this.ParameterByName["ruecklauftemperatur"];
      int blockStartAddress;
      int NumberOfBytes;
      if (s3Parameter1.BlockStartAddress < s3Parameter2.BlockStartAddress)
      {
        blockStartAddress = s3Parameter1.BlockStartAddress;
        NumberOfBytes = s3Parameter2.BlockStartAddress + s3Parameter2.ByteSize - blockStartAddress;
      }
      else
      {
        blockStartAddress = s3Parameter2.BlockStartAddress;
        NumberOfBytes = s3Parameter1.BlockStartAddress + s3Parameter1.ByteSize - blockStartAddress;
      }
      if (!this.MyMeter.MyDeviceMemory.ReadDataFromConnectedDevice(blockStartAddress, NumberOfBytes))
        return false;
      flowTemp = (float) Convert.ToDouble(s3Parameter1.GetShortValue());
      flowTemp /= 100f;
      returnTemp = (float) Convert.ToDouble(s3Parameter2.GetShortValue());
      returnTemp /= 100f;
      return true;
    }

    internal bool ReadDynamicParameterFromRAM(out int StartAddress, out int ReadSize)
    {
      StartAddress = 0;
      ReadSize = 0;
      if (!this.GarantDynamicParameterRangeLoaded())
        return false;
      StartAddress = this.dynamicRAM_ParameterRange.minAddress;
      ReadSize = this.dynamicRAM_ParameterRange.maxAddress - StartAddress + 1;
      return this.MyMeter.MyDeviceMemory.ReadDataFromConnectedDevice(StartAddress, ReadSize);
    }

    internal bool ReloadDisplyValues()
    {
      this.MyMeter.MyParameters.ParameterByName["Vol_VolDisplay"].SetUintValue(Convert.ToUInt32(Math.Floor(this.MyMeter.MyParameters.ParameterByName["Bak_VolSum"].GetDoubleValue())));
      this.MyMeter.MyParameters.ParameterByName["Energy_HeatEnergyDisplay"].SetUintValue(Convert.ToUInt32(Math.Floor(this.MyMeter.MyParameters.ParameterByName["Bak_HeatEnergySum"].GetDoubleValue())));
      this.MyMeter.MyParameters.ParameterByName["Energy_ColdEnergyDisplay"].SetUintValue(Convert.ToUInt32(Math.Floor(this.MyMeter.MyParameters.ParameterByName["Bak_ColdEnergySum"].GetDoubleValue())));
      this.MyMeter.MyParameters.ParameterByName["Energy_Tariff0EnergyDisplay"].SetUintValue(Convert.ToUInt32(Math.Floor(this.MyMeter.MyParameters.ParameterByName["Bak_Tariff0EnergySum"].GetDoubleValue())));
      this.MyMeter.MyParameters.ParameterByName["Energy_Tariff1EnergyDisplay"].SetUintValue(Convert.ToUInt32(Math.Floor(this.MyMeter.MyParameters.ParameterByName["Bak_Tariff1EnergySum"].GetDoubleValue())));
      return true;
    }

    internal void InsertRelocalizableParameterData()
    {
      this.insertHeapParameterData(this.MyMeter.MyDeviceMemory.BlockBackupRuntimeVars);
      this.insertHeapParameterData(this.MyMeter.MyDeviceMemory.BlockConfiguratorHeap);
    }

    private void insertHeapParameterData(S3_MemoryBlock theBlock)
    {
      if (theBlock is S3_Parameter)
        ((S3_Parameter) theBlock).InsertData();
      if (theBlock.childMemoryBlocks == null)
        return;
      foreach (S3_MemoryBlock childMemoryBlock in theBlock.childMemoryBlocks)
        this.insertHeapParameterData(childMemoryBlock);
    }

    internal AddressRange GetParameterAddressRange(S3_ParameterNames[] volInputParams)
    {
      AddressRange parameterAddressRange = new AddressRange((uint) ushort.MaxValue, 0U);
      for (int index = 0; index < volInputParams.Length; ++index)
      {
        if (this.MyMeter.MyParameters.ParameterByName.ContainsKey(volInputParams[index].ToString()))
        {
          uint endAddress = parameterAddressRange.EndAddress;
          S3_Parameter s3Parameter = this.MyMeter.MyParameters.ParameterByName[volInputParams[index].ToString()];
          if ((long) parameterAddressRange.StartAddress > (long) s3Parameter.BlockStartAddress)
          {
            parameterAddressRange.StartAddress = (uint) s3Parameter.BlockStartAddress;
            if (parameterAddressRange.ByteSize > 0U)
              parameterAddressRange.EndAddress = endAddress;
          }
          int num = s3Parameter.BlockStartAddress + s3Parameter.ByteSize - 1;
          if ((long) parameterAddressRange.EndAddress < (long) num)
            parameterAddressRange.EndAddress = (uint) num;
        }
      }
      return parameterAddressRange;
    }
  }
}
