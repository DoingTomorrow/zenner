// Decompiled with JetBrains decompiler
// Type: EDC_Handler.EDC_MemoryMap
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using EDC_Handler.Properties;
using GmmDbLib;
using HandlerLib;
using NLog;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace EDC_Handler
{
  public sealed class EDC_MemoryMap
  {
    private static Logger logger = LogManager.GetLogger("EDCMemoryMap");
    private static string databaseUsedForCache = string.Empty;
    private static Dictionary<string, List<Parameter>> cachedParameters;
    private static Dictionary<string, RangeSet<ushort>> cachedRangesToRead;
    private byte[] memoryBytes = new byte[(int) ushort.MaxValue];
    private bool[] byteIsDefined = new bool[(int) ushort.MaxValue];
    private const ushort MAX_MEMORY_SIZE = 65535;
    private const byte MAX_BLOCK_SIZE_TO_WRITE = 64;
    private const byte MAX_BLOCK_SIZE_TO_READ = 178;
    public const int FLASH_SIZE_TO_READ = 6144;

    public EDC_MemoryMap()
    {
      for (int index = 0; index < (int) ushort.MaxValue; ++index)
        this.memoryBytes[index] = byte.MaxValue;
    }

    internal EDC_MemoryMap DeepCopy()
    {
      return new EDC_MemoryMap()
      {
        byteIsDefined = this.byteIsDefined != null ? (bool[]) this.byteIsDefined.Clone() : (bool[]) null,
        memoryBytes = this.memoryBytes != null ? (byte[]) this.memoryBytes.Clone() : (byte[]) null
      };
    }

    internal void ResetMemoryMap()
    {
      for (int index = 0; index < (int) ushort.MaxValue; ++index)
      {
        if (this.byteIsDefined[index])
          this.byteIsDefined[index] = false;
        if (this.memoryBytes[index] != byte.MaxValue)
          this.memoryBytes[index] = byte.MaxValue;
      }
    }

    internal bool IsEmpty()
    {
      for (int index = 0; index < (int) ushort.MaxValue; ++index)
      {
        if (this.byteIsDefined[index])
          return false;
      }
      return true;
    }

    public bool SetMemoryBytes(ushort address, byte[] buffer)
    {
      if (buffer == null)
        throw new ArgumentException("Can not set bytes to MAP! The 'buffer' is null.");
      if ((int) address + buffer.Length > (int) ushort.MaxValue)
        throw new ArgumentOutOfRangeException("Can not set bytes at the address: 0x" + address.ToString("X4"));
      Buffer.BlockCopy((Array) buffer, 0, (Array) this.memoryBytes, (int) address, buffer.Length);
      for (int index = 0; index < buffer.Length; ++index)
        this.byteIsDefined[(int) address + index] = true;
      return true;
    }

    public byte[] GetMemoryBytes(Parameter p)
    {
      if (p == null)
        throw new ArgumentNullException("Parameter 'p' can not be null!");
      return this.GetMemoryBytes(p.Address, p.Size);
    }

    internal byte[] GetMemoryBytes(ushort address, int size)
    {
      if (size <= 0)
        throw new ArgumentException("Invalid byte size!");
      byte[] dst = this.AreBytesDefined(address, size) ? new byte[size] : throw new ArgumentException("No bytes defined at the address: 0x" + address.ToString("X4") + ", Size: " + size.ToString());
      Buffer.BlockCopy((Array) this.memoryBytes, (int) address, (Array) dst, 0, size);
      return dst;
    }

    internal bool AreBytesDefined(ushort address, int size)
    {
      for (int index = (int) address; index < (int) address + size; ++index)
      {
        if (!this.byteIsDefined[index])
          return false;
      }
      return true;
    }

    internal SortedList<ushort, byte[]> GetChangedRamBlocks(
      DeviceVersion version,
      EDC_Meter sourceMeter)
    {
      if (version == null)
        throw new ArgumentNullException(nameof (version));
      EDC_MemoryMap.CacheParameter(version);
      if (!EDC_MemoryMap.ExistParameter(version))
        return (SortedList<ushort, byte[]>) null;
      SortedList<ushort, byte[]> changedRamBlocks = new SortedList<ushort, byte[]>();
      List<byte> byteList = new List<byte>(64);
      int key = -1;
      Parameter parameter1 = EDC_MemoryMap.cachedParameters[version.VersionString].Find((Predicate<Parameter>) (e => e.Name == "DATA_CONFIG"));
      if (parameter1 == null)
        return (SortedList<ushort, byte[]>) null;
      Parameter parameter2 = EDC_MemoryMap.cachedParameters[version.VersionString].Find((Predicate<Parameter>) (e => e.Name == "CSTACK"));
      if (parameter2 == null)
        return (SortedList<ushort, byte[]>) null;
      ushort address1 = parameter1.Address;
      ushort address2 = parameter2.Address;
      if (sourceMeter == null)
      {
        foreach (Parameter parameter3 in EDC_MemoryMap.cachedParameters[version.VersionString])
        {
          if (this.byteIsDefined[(int) parameter3.Address] & ((int) parameter3.Address >= (int) address1 && (int) parameter3.Address + parameter3.Size <= (int) address2) & parameter3.Size > 0)
          {
            byte[] dst = new byte[parameter3.Size];
            Buffer.BlockCopy((Array) this.memoryBytes, (int) parameter3.Address, (Array) dst, 0, parameter3.Size);
            changedRamBlocks.Add(parameter3.Address, dst);
          }
        }
      }
      else
      {
        for (int index1 = (int) address1; index1 < (int) address2; ++index1)
        {
          bool flag1 = this.byteIsDefined[index1];
          bool flag2 = (int) this.memoryBytes[index1] != (int) sourceMeter.Map.memoryBytes[index1];
          bool flag3 = byteList.Count == 0;
          if (!(!flag2 & flag3) && !(!flag1 & flag3))
          {
            if (!flag1 && !flag3)
              throw new Exception("INTERNAL ERROR: Invalid memory map!");
            if (key == -1)
              key = index1;
            byteList.Add(this.memoryBytes[index1]);
            bool flag4 = byteList.Count == 64;
            bool flag5 = index1 + 1 == this.memoryBytes.Length;
            bool flag6 = !flag5 && !this.byteIsDefined[index1 + 1];
            if (flag4 | flag5 | flag6)
            {
              for (int index2 = byteList.Count - 1; index2 >= 0 && (int) byteList[index2] == (int) sourceMeter.Map.memoryBytes[index2 + key]; --index2)
                byteList.RemoveAt(index2);
              changedRamBlocks.Add((ushort) key, byteList.ToArray());
              byteList.Clear();
              key = -1;
            }
          }
        }
      }
      return changedRamBlocks;
    }

    internal SortedList<ushort, byte[]> GetChangedFlashBlocks(
      EDC_Meter sourceMeter,
      int segmentStartAddress,
      int segmentEndAddress,
      int sizeOfSegment,
      out List<ushort> segmentsToErase)
    {
      segmentsToErase = new List<ushort>();
      SortedList<ushort, byte[]> changedFlashBlocks = new SortedList<ushort, byte[]>();
      for (int address = segmentStartAddress; address < segmentEndAddress; address += sizeOfSegment)
      {
        if (!this.IsSegmentValid(address, sizeOfSegment))
          throw new Exception("Corrupt segment detected! Address: 0x" + address.ToString("X4"));
        if (this.IsSegmentDefined(address, sizeOfSegment) && !this.IsSegmentEmpty(address, sizeOfSegment))
        {
          for (int index = address; index < address + sizeOfSegment; ++index)
          {
            if (sourceMeter == null || (int) this.memoryBytes[index] != (int) sourceMeter.Map.memoryBytes[index])
            {
              segmentsToErase.Add((ushort) address);
              break;
            }
          }
        }
      }
      if (segmentsToErase.Count == 0)
        return (SortedList<ushort, byte[]>) null;
      List<byte> byteList = new List<byte>(64);
      int key = -1;
      for (int index = (int) segmentsToErase[0]; index < (int) segmentsToErase[segmentsToErase.Count - 1] + sizeOfSegment; ++index)
      {
        if (this.byteIsDefined[index] && !(this.memoryBytes[index] == byte.MaxValue & byteList.Count == 0))
        {
          if (key == -1)
          {
            if (index % 2 != 0)
            {
              if (!this.byteIsDefined[index])
                throw new Exception("Invalid segment detected! Address" + index.ToString());
              --index;
            }
            key = index;
          }
          byteList.Add(this.memoryBytes[index]);
          bool flag1 = byteList.Count == 64;
          bool flag2 = index + 1 == (int) segmentsToErase[segmentsToErase.Count - 1] + sizeOfSegment;
          bool flag3 = !flag2 && !this.byteIsDefined[index + 1];
          if (flag1 | flag2 | flag3)
          {
            changedFlashBlocks.Add((ushort) key, byteList.ToArray());
            byteList.Clear();
            key = -1;
          }
        }
      }
      return changedFlashBlocks;
    }

    private bool IsSegmentEmpty(int address, int sizeOfSegment)
    {
      for (int index = address; index < address + sizeOfSegment; ++index)
      {
        if (this.memoryBytes[index] != byte.MaxValue)
          return false;
      }
      return true;
    }

    private bool IsSegmentDefined(int address, int sizeOfSegment)
    {
      for (int index = address; index < address + sizeOfSegment; ++index)
      {
        if (!this.byteIsDefined[index])
          return false;
      }
      return true;
    }

    private bool IsSegmentValid(int address, int sizeOfSegment)
    {
      if (address % sizeOfSegment != 0)
        return false;
      bool flag = this.byteIsDefined[address];
      for (int index = address; index < address + sizeOfSegment; ++index)
      {
        if (this.byteIsDefined[index] != flag)
          return false;
      }
      return true;
    }

    internal byte[] Zip(DeviceVersion version)
    {
      if (version == null)
        return (byte[]) null;
      byte[] numArray1 = new byte[1]{ (byte) 1 };
      byte[] bytes = version.GetBytes();
      byte[] numArray2 = Util.ConvertAll<bool, byte>(this.byteIsDefined, new Converter<bool, byte>(Convert.ToByte));
      byte[] memoryBytes = this.memoryBytes;
      byte[] buffer = new byte[1 + bytes.Length + 131070];
      numArray1.CopyTo((Array) buffer, 0);
      bytes.CopyTo((Array) buffer, numArray1.Length);
      numArray2.CopyTo((Array) buffer, numArray1.Length + bytes.Length);
      memoryBytes.CopyTo((Array) buffer, numArray1.Length + bytes.Length + numArray2.Length);
      return Util.Zip(buffer);
    }

    internal bool Unzip(byte[] buffer, out DeviceVersion version)
    {
      version = (DeviceVersion) null;
      if (buffer == null)
        return false;
      byte[] numArray1 = Util.Unzip(buffer);
      if (numArray1 == null || numArray1.Length != 131089 || numArray1[0] != (byte) 1)
        return false;
      int offset = 1;
      version = DeviceVersion.Parse(numArray1, ref offset);
      if (version == null)
        return false;
      byte[] numArray2 = new byte[(int) ushort.MaxValue];
      Buffer.BlockCopy((Array) numArray1, offset, (Array) numArray2, 0, numArray2.Length);
      this.byteIsDefined = Util.ConvertAll<byte, bool>(numArray2, new Converter<byte, bool>(Convert.ToBoolean));
      Buffer.BlockCopy((Array) numArray1, offset + (int) ushort.MaxValue, (Array) this.memoryBytes, 0, this.memoryBytes.Length);
      return true;
    }

    internal static void CacheParameter(DeviceVersion version)
    {
      if (version == null)
        throw new ArgumentNullException(nameof (version));
      EDC_MemoryMap.CheckCache();
      if (EDC_MemoryMap.cachedParameters == null)
        EDC_MemoryMap.cachedParameters = new Dictionary<string, List<Parameter>>();
      lock (EDC_MemoryMap.cachedParameters)
      {
        if (EDC_MemoryMap.cachedParameters != null && EDC_MemoryMap.cachedParameters.ContainsKey(version.VersionString) || EDC_MemoryMap.cachedParameters.ContainsKey(version.VersionString))
          return;
        List<HandlerLib.Parameter> parameterList1 = MSP430.LoadParameter(Resources.ResourceManager, version.Version, new string[1]
        {
          "PARAM_CONST_LIST"
        }, new string[6]
        {
          "usci",
          "bigBuffer",
          "cid",
          "data_",
          "process",
          "comm"
        });
        List<Parameter> parameterList2;
        if (parameterList1 != null)
        {
          List<Parameter> edcParameter = new List<Parameter>();
          parameterList1.ForEach((Action<HandlerLib.Parameter>) (x => edcParameter.Add(new Parameter()
          {
            Address = x.Address,
            Name = x.Name,
            Size = x.Size,
            Type = x.Segment == "SEGMENT" ? S3_VariableTypes.Address : S3_VariableTypes.UINT8
          })));
          parameterList2 = edcParameter;
        }
        else
          parameterList2 = EDC_Database.LoadParameter(version);
        if (parameterList2 != null)
          EDC_MemoryMap.cachedParameters.Add(version.VersionString, parameterList2);
        EDC_MemoryMap.SaveDatabaseUsedForCache();
      }
    }

    internal static bool ExistParameter(DeviceVersion version)
    {
      if (version == null)
        throw new ArgumentNullException(nameof (version));
      return EDC_MemoryMap.cachedParameters != null && EDC_MemoryMap.cachedParameters.ContainsKey(version.VersionString);
    }

    public static List<Parameter> GetParameter(DeviceVersion version)
    {
      if (version == null)
        throw new ArgumentNullException(nameof (version));
      EDC_MemoryMap.CacheParameter(version);
      return !EDC_MemoryMap.ExistParameter(version) ? (List<Parameter>) null : EDC_MemoryMap.cachedParameters[version.VersionString];
    }

    internal static Parameter GetParameter(DeviceVersion version, string name)
    {
      return EDC_MemoryMap.GetParameter(version)?.Find((Predicate<Parameter>) (e => e.Name == name));
    }

    internal static Parameter GetParameter(DeviceVersion version, ushort address)
    {
      return EDC_MemoryMap.GetParameter(version)?.Find((Predicate<Parameter>) (e => (int) e.Address == (int) address));
    }

    internal static RangeSet<ushort> GetMemoryBlocksToRead(DeviceVersion version, bool readAllData)
    {
      if (version == null)
        throw new ArgumentNullException(nameof (version));
      EDC_MemoryMap.CacheParameter(version);
      if (!EDC_MemoryMap.ExistParameter(version))
        return (RangeSet<ushort>) null;
      string key = version.VersionString + readAllData.ToString();
      EDC_MemoryMap.CheckCache();
      Parameter parameter1 = EDC_MemoryMap.cachedParameters[version.VersionString].Find((Predicate<Parameter>) (e => e.Name == "INFOA"));
      if (parameter1 == null)
        throw new Exception("INTERNAL ERROR: Invalid MAP file! Can not find the parameter INFOA.");
      Parameter parameter2 = EDC_MemoryMap.cachedParameters[version.VersionString].Find((Predicate<Parameter>) (e => e.Name == "PARAM_CONST_LIST"));
      if (parameter2 == null)
        throw new Exception("INTERNAL ERROR: Invalid MAP file! Can not find the parameter PARAM_CONST_LIST.");
      if (EDC_MemoryMap.cachedRangesToRead != null && EDC_MemoryMap.cachedRangesToRead.ContainsKey(key))
        return EDC_MemoryMap.cachedRangesToRead[key];
      if (EDC_MemoryMap.cachedRangesToRead == null)
        EDC_MemoryMap.cachedRangesToRead = new Dictionary<string, RangeSet<ushort>>();
      bool[] points = new bool[(int) ushort.MaxValue];
      for (int index = 0; index < points.Length; ++index)
        points[index] = false;
      foreach (Parameter parameter3 in EDC_MemoryMap.cachedParameters[version.VersionString])
      {
        if (parameter3.Type != S3_VariableTypes.Address)
        {
          for (int address = (int) parameter3.Address; address < (int) parameter3.Address + parameter3.Size; ++address)
            points[address] = true;
        }
      }
      for (int address = (int) parameter1.Address; address < (int) parameter1.Address + 128; ++address)
        points[address] = true;
      if (version.Type == EDC_Hardware.EDC_Radio)
      {
        for (int address = (int) parameter2.Address; address < (int) parameter2.Address + 512; ++address)
          points[address] = true;
      }
      if (readAllData)
      {
        for (int index = 32768; index < (int) parameter2.Address; ++index)
          points[index] = true;
        ushort startRamLogger;
        ushort endRamLogger;
        Addresses.GetRamLogger(version, out startRamLogger, out endRamLogger);
        for (int index = (int) startRamLogger; index < (int) endRamLogger; ++index)
          points[index] = true;
      }
      RangeSet<ushort> rangeSet = EDC_MemoryMap.ConvertBoolArrayToRangeSet(points);
      if (rangeSet == null)
        return (RangeSet<ushort>) null;
      EDC_MemoryMap.cachedRangesToRead.Add(key, rangeSet);
      EDC_MemoryMap.SaveDatabaseUsedForCache();
      return rangeSet;
    }

    private static RangeSet<ushort> ConvertBoolArrayToRangeSet(bool[] points)
    {
      RangeSet<ushort> rangeSet = new RangeSet<ushort>();
      ushort startRange = 0;
      ushort endRange = 0;
      for (ushort index = 0; (int) index < points.Length; ++index)
      {
        if (points[(int) index])
        {
          if (startRange == (ushort) 0)
          {
            startRange = (int) index % 2 == 0 ? index : (ushort) ((int) index - 1);
            endRange = startRange;
          }
          int num = (int) index - (int) startRange;
          if (num % 2 != 0)
            ++num;
          if (num >= 178)
          {
            if ((int) endRange % 2 == 0)
              ++endRange;
            rangeSet.Add(startRange, endRange);
            startRange = (int) index % 2 == 0 ? index : (ushort) ((int) index - 1);
          }
          endRange = index;
        }
        if ((int) index + 1 == points.Length && startRange > (ushort) 0)
        {
          if ((uint) endRange % 2U > 0U)
            ++endRange;
          rangeSet.Add(startRange, endRange);
        }
      }
      return rangeSet;
    }

    internal int GetDefinedBytesCount()
    {
      int definedBytesCount = 0;
      for (int index = 0; index < this.byteIsDefined.Length; ++index)
      {
        if (this.byteIsDefined[index])
          ++definedBytesCount;
      }
      return definedBytesCount;
    }

    private static void CheckCache()
    {
      if (!string.IsNullOrEmpty(EDC_MemoryMap.databaseUsedForCache) && DbBasis.PrimaryDB != null && !(DbBasis.PrimaryDB.ConnectionString != EDC_MemoryMap.databaseUsedForCache))
        return;
      if (EDC_MemoryMap.cachedParameters != null)
        EDC_MemoryMap.cachedParameters.Clear();
      if (EDC_MemoryMap.cachedRangesToRead != null)
        EDC_MemoryMap.cachedRangesToRead.Clear();
      EDC_MemoryMap.databaseUsedForCache = string.Empty;
    }

    private static void SaveDatabaseUsedForCache()
    {
      if (string.IsNullOrEmpty(EDC_MemoryMap.databaseUsedForCache))
        EDC_MemoryMap.databaseUsedForCache = DbBasis.PrimaryDB.ConnectionString;
      else if (EDC_MemoryMap.databaseUsedForCache != DbBasis.PrimaryDB.ConnectionString)
        throw new Exception("INTERNAL ERROR: Wrong state of the handler detected! The cached objects are not of the same database source. Cached objects in handler use: " + EDC_MemoryMap.databaseUsedForCache + Environment.NewLine + " Actual database is: " + DbBasis.PrimaryDB.ConnectionString);
    }
  }
}
