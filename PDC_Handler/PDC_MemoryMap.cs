// Decompiled with JetBrains decompiler
// Type: PDC_Handler.PDC_MemoryMap
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using GmmDbLib;
using NLog;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace PDC_Handler
{
  public sealed class PDC_MemoryMap
  {
    private static Logger logger = LogManager.GetLogger(nameof (PDC_MemoryMap));
    public const ushort MAX_MEMORY_SIZE = 65535;
    private const byte MAX_BLOCK_SIZE_TO_WRITE = 64;
    private const byte MAX_BLOCK_SIZE_TO_READ = 178;
    private static string databaseUsedForCache = string.Empty;
    private static Dictionary<string, List<Parameter>> cachedParameters;
    private static Dictionary<string, RangeSet<ushort>> cachedRangesToRead;
    private byte[] memoryBytes = new byte[(int) ushort.MaxValue];
    private bool[] byteIsDefined = new bool[(int) ushort.MaxValue];

    public PDC_MemoryMap()
    {
      for (int index = 0; index < (int) ushort.MaxValue; ++index)
        this.memoryBytes[index] = byte.MaxValue;
    }

    public byte[] MemoryBytes => this.memoryBytes;

    internal PDC_MemoryMap DeepCopy()
    {
      return new PDC_MemoryMap()
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

    internal bool SetMemoryBytes(ushort address, byte[] buffer)
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
      PDC_Meter sourceMeter)
    {
      if (version == null)
        throw new ArgumentNullException(nameof (version));
      PDC_MemoryMap.CacheParameter(version);
      if (!PDC_MemoryMap.ExistParameter(version))
        return (SortedList<ushort, byte[]>) null;
      SortedList<ushort, byte[]> changedRamBlocks = new SortedList<ushort, byte[]>();
      List<byte> byteList = new List<byte>(64);
      int key = -1;
      Parameter parameter1 = PDC_MemoryMap.cachedParameters[version.VersionString].Find((Predicate<Parameter>) (e => e.Name == "DATA_CONFIG"));
      if (parameter1 == null)
        return (SortedList<ushort, byte[]>) null;
      Parameter parameter2 = PDC_MemoryMap.cachedParameters[version.VersionString].Find((Predicate<Parameter>) (e => e.Name == "CSTACK"));
      if (parameter2 == null)
        return (SortedList<ushort, byte[]>) null;
      for (int address = (int) parameter1.Address; address < (int) parameter2.Address; ++address)
      {
        bool flag1 = this.byteIsDefined[address];
        bool flag2 = sourceMeter == null || (int) this.memoryBytes[address] != (int) sourceMeter.Map.memoryBytes[address];
        bool flag3 = byteList.Count == 0;
        if (!(!flag2 & flag3) && !(!flag1 & flag3))
        {
          if (!flag1 && !flag3)
            throw new Exception("INTERNAL ERROR: Invalid memory map!");
          if (key == -1)
            key = address;
          byteList.Add(this.memoryBytes[address]);
          bool flag4 = byteList.Count == 64;
          bool flag5 = address + 1 == this.memoryBytes.Length;
          bool flag6 = !flag5 && !this.byteIsDefined[address + 1];
          if (flag4 | flag5 | flag6)
          {
            for (int index = byteList.Count - 1; index >= 0 && sourceMeter != null && (int) byteList[index] == (int) sourceMeter.Map.memoryBytes[index + key]; --index)
              byteList.RemoveAt(index);
            changedRamBlocks.Add((ushort) key, byteList.ToArray());
            byteList.Clear();
            key = -1;
          }
        }
      }
      return changedRamBlocks;
    }

    internal SortedList<ushort, byte[]> GetChangedFlashBlocks(
      PDC_Meter sourceMeter,
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
      PDC_MemoryMap.CheckCache();
      if (PDC_MemoryMap.cachedParameters != null && PDC_MemoryMap.cachedParameters.ContainsKey(version.VersionString))
        return;
      if (PDC_MemoryMap.cachedParameters == null)
        PDC_MemoryMap.cachedParameters = new Dictionary<string, List<Parameter>>();
      lock (PDC_MemoryMap.cachedParameters)
      {
        if (PDC_MemoryMap.cachedParameters.ContainsKey(version.VersionString))
          return;
        List<Parameter> parameterList = PDC_Database.LoadParameter(version);
        if (parameterList != null)
          PDC_MemoryMap.cachedParameters.Add(version.VersionString, parameterList);
        PDC_MemoryMap.SaveDatabaseUsedForCache();
      }
    }

    internal static bool ExistParameter(DeviceVersion version)
    {
      if (version == null)
        throw new ArgumentNullException(nameof (version));
      return PDC_MemoryMap.cachedParameters != null && PDC_MemoryMap.cachedParameters.ContainsKey(version.VersionString);
    }

    public static List<Parameter> GetParameter(DeviceVersion version)
    {
      if (version == null)
        throw new ArgumentNullException(nameof (version));
      PDC_MemoryMap.CacheParameter(version);
      return !PDC_MemoryMap.ExistParameter(version) ? (List<Parameter>) null : PDC_MemoryMap.cachedParameters[version.VersionString];
    }

    internal static Parameter GetParameter(DeviceVersion version, string name)
    {
      return PDC_MemoryMap.GetParameter(version)?.Find((Predicate<Parameter>) (e => e.Name == name));
    }

    internal static Parameter GetParameter(DeviceVersion version, ushort address)
    {
      return PDC_MemoryMap.GetParameter(version)?.Find((Predicate<Parameter>) (e => (int) e.Address == (int) address));
    }

    internal static RangeSet<ushort> GetMemoryBlocksToRead(DeviceVersion version)
    {
      if (version == null)
        throw new ArgumentNullException(nameof (version));
      PDC_MemoryMap.CacheParameter(version);
      if (!PDC_MemoryMap.ExistParameter(version))
        return (RangeSet<ushort>) null;
      string versionString = version.VersionString;
      PDC_MemoryMap.CheckCache();
      Parameter parameter1 = PDC_MemoryMap.cachedParameters[version.VersionString].Find((Predicate<Parameter>) (e => e.Name == "INFOA"));
      if (parameter1 == null)
        throw new Exception("INTERNAL ERROR: Invalid MAP file! Can not find the parameter INFOA.");
      Parameter parameter2 = PDC_MemoryMap.cachedParameters[version.VersionString].Find((Predicate<Parameter>) (e => e.Name == "PARAM_CONST_LIST"));
      if (parameter2 == null)
        throw new Exception("INTERNAL ERROR: Invalid MAP file! Can not find the parameter PARAM_CONST_LIST.");
      if (PDC_MemoryMap.cachedRangesToRead != null && PDC_MemoryMap.cachedRangesToRead.ContainsKey(versionString))
        return PDC_MemoryMap.cachedRangesToRead[versionString];
      if (PDC_MemoryMap.cachedRangesToRead == null)
        PDC_MemoryMap.cachedRangesToRead = new Dictionary<string, RangeSet<ushort>>();
      bool[] points = new bool[(int) ushort.MaxValue];
      for (int index = 0; index < points.Length; ++index)
        points[index] = false;
      foreach (Parameter parameter3 in PDC_MemoryMap.cachedParameters[version.VersionString])
      {
        if (parameter3.Type != S3_VariableTypes.Address)
        {
          for (int address = (int) parameter3.Address; address < (int) parameter3.Address + parameter3.Size; ++address)
            points[address] = true;
        }
      }
      for (int address = (int) parameter1.Address; address < (int) parameter1.Address + 128; ++address)
        points[address] = true;
      for (int address = (int) parameter2.Address; address < (int) parameter2.Address + 1024; ++address)
        points[address] = true;
      RangeSet<ushort> rangeSet = PDC_MemoryMap.ConvertBoolArrayToRangeSet(points);
      if (rangeSet == null)
        return (RangeSet<ushort>) null;
      PDC_MemoryMap.cachedRangesToRead.Add(versionString, rangeSet);
      PDC_MemoryMap.SaveDatabaseUsedForCache();
      return rangeSet;
    }

    public static RangeSet<ushort> ConvertBoolArrayToRangeSet(bool[] points)
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
      if (!string.IsNullOrEmpty(PDC_MemoryMap.databaseUsedForCache) && DbBasis.PrimaryDB != null && !(DbBasis.PrimaryDB.ConnectionString != PDC_MemoryMap.databaseUsedForCache))
        return;
      if (PDC_MemoryMap.cachedParameters != null)
        PDC_MemoryMap.cachedParameters.Clear();
      if (PDC_MemoryMap.cachedRangesToRead != null)
        PDC_MemoryMap.cachedRangesToRead.Clear();
      PDC_MemoryMap.databaseUsedForCache = string.Empty;
    }

    private static void SaveDatabaseUsedForCache()
    {
      if (string.IsNullOrEmpty(PDC_MemoryMap.databaseUsedForCache))
        PDC_MemoryMap.databaseUsedForCache = DbBasis.PrimaryDB.ConnectionString;
      else if (PDC_MemoryMap.databaseUsedForCache != DbBasis.PrimaryDB.ConnectionString)
        throw new Exception("INTERNAL ERROR: Wrong state of the handler detected! The cached objects are not of the same database source. Cached objects in handler use: " + PDC_MemoryMap.databaseUsedForCache + Environment.NewLine + " Actual database is: " + DbBasis.PrimaryDB.ConnectionString);
    }
  }
}
