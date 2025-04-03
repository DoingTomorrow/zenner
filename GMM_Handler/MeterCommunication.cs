// Decompiled with JetBrains decompiler
// Type: GMM_Handler.MeterCommunication
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using DeviceCollector;
using System;
using System.Collections.Generic;
using System.Globalization;
using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  public class MeterCommunication
  {
    private Meter MyMeter;
    internal IDeviceCollector MyBus;
    private bool SimulatedCommunication = false;
    private SortedList<int, byte[]> CachedRamData;
    private ByteField RamParameterCacheData = (ByteField) null;
    private int RamParameterCacheAddress;

    public MeterCommunication(Meter MyMeterIn)
    {
      this.MyMeter = MyMeterIn;
      this.MyBus = this.MyMeter.MyHandler.SerBus;
      this.MyMeter.MyMath.GetCpuData += new ZelsiusMath.CpuRead(this.GetDataToInterpreter);
    }

    public MeterCommunication(Meter MyMeterIn, bool SimulatedCommunication)
    {
      this.SimulatedCommunication = SimulatedCommunication;
      this.MyMeter = MyMeterIn;
      this.MyBus = this.MyMeter.MyHandler.SerBus;
      this.MyMeter.MyMath.GetCpuData += new ZelsiusMath.CpuRead(this.GetDataToInterpreter);
    }

    private bool GetDataToInterpreter(
      string NameOrAddress,
      int NumberOfBytes,
      MemoryLocation Location,
      out byte[] Data)
    {
      Data = new byte[NumberOfBytes];
      if (!this.MyMeter.MyHandler.actualValueReadingState && !this.SimulatedCommunication)
        return true;
      try
      {
        Parameter parameter = (Parameter) null;
        int num;
        if (NameOrAddress.StartsWith("0x"))
        {
          num = int.Parse(NameOrAddress.Substring(2), NumberStyles.HexNumber);
          if (this.SimulatedCommunication)
          {
            if (Location == MemoryLocation.RAM)
            {
              int index = this.MyMeter.AllRamParametersByAddress.IndexOfKey((object) num);
              if (index >= 0)
                parameter = (Parameter) this.MyMeter.AllRamParametersByAddress.GetByIndex(index);
            }
            else
            {
              int index = this.MyMeter.AllEpromParametersByAddress.IndexOfKey((object) num);
              if (index >= 0)
                parameter = (Parameter) this.MyMeter.AllEpromParametersByAddress.GetByIndex(index);
            }
            if (parameter != null && parameter.Size != NumberOfBytes)
              parameter = (Parameter) null;
          }
        }
        else
        {
          parameter = (Parameter) this.MyMeter.AllParameters[(object) NameOrAddress] ?? (Parameter) this.MyMeter.AllParameters[(object) ("DefaultFunction." + NameOrAddress)];
          if (parameter == null)
          {
            if (this.SimulatedCommunication)
              return true;
            goto label_50;
          }
          else
            num = Location != MemoryLocation.EEPROM ? parameter.AddressCPU : parameter.Address;
        }
        if (this.SimulatedCommunication)
        {
          if (parameter == null)
            return true;
          parameter.UpdateByteList();
          if (parameter.LinkByteList == null)
            return true;
          for (int index = 0; index < NumberOfBytes; ++index)
            Data[index] = parameter.LinkByteList[index];
          return true;
        }
        if (num >= 0 && num <= (int) ushort.MaxValue)
        {
          if (Location == MemoryLocation.EEPROM)
          {
            if (num >= this.MyMeter.MyLoggerStore.BlockStartAddress)
            {
              ByteField MemoryData;
              if (this.MyBus.ReadMemory(MemoryLocation.EEPROM, num, NumberOfBytes, out MemoryData))
              {
                for (int index = 0; index < NumberOfBytes; ++index)
                  Data[index] = MemoryData.Data[index];
              }
              else
                goto label_50;
            }
            else
            {
              for (int index = 0; index < NumberOfBytes; ++index)
                Data[index] = this.MyMeter.Eprom[num + index];
            }
          }
          else
          {
            if (this.CachedRamData == null)
            {
              this.CachedRamData = new SortedList<int, byte[]>();
            }
            else
            {
              int index = this.CachedRamData.IndexOfKey(num);
              if (index >= 0)
              {
                Data = this.CachedRamData.Values[index];
                if (Data.Length == NumberOfBytes)
                  return true;
                this.CachedRamData.RemoveAt(index);
              }
            }
            ByteField MemoryData;
            if (this.MyBus.ReadMemory(MemoryLocation.RAM, num, NumberOfBytes, out MemoryData))
            {
              for (int index = 0; index < NumberOfBytes; ++index)
                Data[index] = MemoryData.Data[index];
              this.CachedRamData.Add(num, MemoryData.Data);
            }
            else
              goto label_50;
          }
        }
        else
          goto label_50;
      }
      catch
      {
        goto label_50;
      }
      return true;
label_50:
      this.MyMeter.MyHandler.actualValueReadingState = false;
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal interpreter data");
      return false;
    }

    internal bool ReadVersion(ZR_MeterIdent TheIdent)
    {
      return this.MyBus.ReadVersion(out TheIdent.MBus_Manufacturer, out TheIdent.MBus_Medium, out TheIdent.MBus_MeterType, out TheIdent.lFirmwareVersion, out TheIdent.MBus_SerialNumber);
    }

    internal bool CacheParameterValues(int StartAdress, int Size)
    {
      this.RamParameterCacheAddress = StartAdress;
      if (Size == 0)
        this.RamParameterCacheData = (ByteField) null;
      else if (!this.MyBus.ReadMemory(MemoryLocation.RAM, StartAdress, Size, out this.RamParameterCacheData))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Read parameter error");
        return false;
      }
      return true;
    }

    internal bool ReadParameterValue(Parameter TheParameter, MemoryLocation TheLocation)
    {
      ByteField MemoryData = new ByteField(TheParameter.Size);
      long num1 = 0;
      int StartAddress;
      if (TheLocation == MemoryLocation.RAM)
      {
        StartAddress = TheParameter.AddressCPU;
        if (this.RamParameterCacheData != null && StartAddress >= this.RamParameterCacheAddress && StartAddress <= this.RamParameterCacheAddress + this.RamParameterCacheData.Data.Length - TheParameter.Size)
        {
          int num2 = StartAddress - this.RamParameterCacheAddress;
          for (int index = 0; index < TheParameter.Size; ++index)
            num1 |= (long) this.RamParameterCacheData.Data[num2 + index] << index * 8;
          TheParameter.ValueCPU = num1;
          TheParameter.CPU_ValueIsInitialised = true;
          return true;
        }
      }
      else
        StartAddress = TheParameter.Address;
      if (!this.MyBus.ReadMemory(TheLocation, StartAddress, TheParameter.Size, out MemoryData))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Read parameter error");
        return false;
      }
      for (int index = 0; index < TheParameter.Size; ++index)
        num1 |= (long) MemoryData.Data[index] << index * 8;
      if (TheLocation == MemoryLocation.RAM)
      {
        TheParameter.ValueCPU = num1;
        TheParameter.CPU_ValueIsInitialised = true;
      }
      else
      {
        TheParameter.ValueEprom = num1;
        TheParameter.EpromValueIsInitialised = true;
        TheParameter.LinkByteList = MemoryData.Data;
      }
      return true;
    }

    internal bool WriteParameterValue(Parameter TheParameter, MemoryLocation TheLocation)
    {
      ByteField data = new ByteField(TheParameter.Size);
      int StartAddress;
      long num;
      if (TheLocation == MemoryLocation.RAM)
      {
        StartAddress = TheParameter.AddressCPU;
        num = TheParameter.ValueCPU;
      }
      else
      {
        StartAddress = TheParameter.Address;
        num = TheParameter.ValueEprom;
      }
      for (int index = 0; index < TheParameter.Size; ++index)
      {
        data.Add((byte) num);
        num >>= 8;
      }
      if (this.MyBus.WriteMemory(TheLocation, StartAddress, data))
        return true;
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Write parameter error");
      return false;
    }

    internal bool getDeviceTime(out DateTime ClockTime, out DateTime NextEventTime)
    {
      Parameter allParameter1 = (Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.Sta_Secounds"];
      if (this.ReadParameterValue(allParameter1, MemoryLocation.RAM))
      {
        ClockTime = ZR_Calendar.Cal_GetDateTime((uint) allParameter1.ValueCPU);
        Parameter allParameter2 = (Parameter) this.MyMeter.AllParameters[(object) "Itr_NextIntervalTime"];
        if (this.ReadParameterValue(allParameter2, MemoryLocation.RAM))
        {
          NextEventTime = ZR_Calendar.Cal_GetDateTime((uint) allParameter2.ValueCPU);
          return true;
        }
      }
      ClockTime = DateTime.MinValue;
      NextEventTime = DateTime.MinValue;
      return false;
    }

    internal bool setDeviceTime(DateTime NewTime)
    {
      Parameter allParameter = (Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.Sta_Secounds"];
      allParameter.ValueCPU = (long) ZR_Calendar.Cal_GetMeterTime(NewTime);
      allParameter.CPU_ValueIsInitialised = true;
      return this.WriteParameterValue(allParameter, MemoryLocation.RAM);
    }

    internal bool VerifyCheckSum(bool IncludeBackupChecksum)
    {
      Parameter allParameter1 = (Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_StaticChecksum"];
      Parameter allParameter2 = (Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_HeaderChecksum"];
      Parameter allParameter3 = (Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_BackupChecksum"];
      int num = !IncludeBackupChecksum ? allParameter2.Address + allParameter2.Size - allParameter1.Address : allParameter3.Address + allParameter3.Size - allParameter1.Address;
      ByteField MemoryData = new ByteField(num);
      if (!this.MyBus.ReadMemory(MemoryLocation.EEPROM, allParameter1.Address, num, out MemoryData))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Verify checksum error");
        return false;
      }
      for (int index = 0; index < num; ++index)
      {
        if ((int) MemoryData.Data[index] != (int) this.MyMeter.Eprom[index + allParameter1.Address])
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Meter replaced");
          return false;
        }
      }
      return true;
    }

    internal bool VerifyMeterID()
    {
      Parameter allParameter = (Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_MeterID"];
      int size = allParameter.Size;
      ByteField MemoryData = new ByteField(size);
      if (!this.MyBus.ReadMemory(MemoryLocation.EEPROM, allParameter.Address, allParameter.Address, out MemoryData))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Verify meter ID error");
        return false;
      }
      for (int index = 0; index < size; ++index)
      {
        if ((int) MemoryData.Data[index] != (int) this.MyMeter.Eprom[index + allParameter.Address])
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Meter replaced");
          return false;
        }
      }
      return true;
    }
  }
}
