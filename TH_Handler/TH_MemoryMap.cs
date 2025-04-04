// Decompiled with JetBrains decompiler
// Type: TH_Handler.TH_MemoryMap
// Assembly: TH_Handler, Version=1.3.4.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 02D62764-6653-46F8-9117-1BC5233AD061
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\TH_Handler.dll

using GmmDbLib;
using HandlerLib;
using NLog;
using System;
using System.Collections.Generic;
using TH_Handler.Properties;
using ZR_ClassLibrary;

#nullable disable
namespace TH_Handler
{
  public sealed class TH_MemoryMap
  {
    private static Logger logger = LogManager.GetLogger(nameof (TH_MemoryMap));
    private const ushort RAM_START = 7168;
    private const ushort RAM_END = 11263;
    private const ushort FLASH_START = 6144;
    private const ushort FLASH_END = 6655;
    private const ushort INFOA_START = 6528;
    private const ushort INFOA_END = 6655;
    internal const ushort INFOD_START = 6144;
    private byte[] memoryBytes = new byte[(int) ushort.MaxValue];
    private bool[] byteIsDefined = new bool[(int) ushort.MaxValue];
    private const ushort MAX_MEMORY_SIZE = 65535;
    private const byte MAX_BLOCK_SIZE_TO_WRITE = 64;
    private const byte MAX_BLOCK_SIZE_TO_READ = 178;
    public const int FLASH_SIZE_TO_READ = 6144;

    public TH_Version Version { get; private set; }

    public List<Parameter> Parameters { get; private set; }

    private TH_MemoryMap(TH_Version version, List<Parameter> parameters)
    {
      this.Version = version;
      this.Parameters = parameters;
      for (int index = 0; index < (int) ushort.MaxValue; ++index)
        this.memoryBytes[index] = byte.MaxValue;
    }

    public static TH_MemoryMap Create(TH_Version version)
    {
      if (version == null)
        throw new ArgumentNullException(nameof (version));
      return new TH_MemoryMap(version, MSP430.LoadParameter(Resources.ResourceManager, version.Version, new string[2]
      {
        "UPDATE_CODE",
        "DATA16_C"
      }, new string[6]
      {
        "usci",
        "bigBuffer",
        "cid",
        "data_",
        "process",
        "comm"
      }) ?? throw new Exception(Ot.Gtm(Tg.HandlerLogic, "MissingMap", "No map file available! Version:") + " " + version.VersionString));
    }

    internal TH_MemoryMap DeepCopy()
    {
      return new TH_MemoryMap(this.Version, this.Parameters)
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

    internal SortedList<ushort, byte[]> GetChangedRamBlocks(TH_Meter sourceMeter)
    {
      SortedList<ushort, byte[]> changedRamBlocks = new SortedList<ushort, byte[]>();
      List<byte> byteList = new List<byte>(64);
      int key = -1;
      if (sourceMeter == null)
      {
        foreach (Parameter parameter in this.Parameters)
        {
          if (this.byteIsDefined[(int) parameter.Address] & (parameter.Address >= (ushort) 7168 && (int) parameter.Address + parameter.Size <= 11263) & parameter.Size > 0)
          {
            byte[] dst = new byte[parameter.Size];
            Buffer.BlockCopy((Array) this.memoryBytes, (int) parameter.Address, (Array) dst, 0, parameter.Size);
            changedRamBlocks.Add(parameter.Address, dst);
          }
        }
      }
      else
      {
        for (int index1 = 7168; index1 < 11263; ++index1)
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

    public SortedList<ushort, byte[]> GetChangedFlashBlocks(
      TH_Meter sourceMeter,
      out List<ushort> segmentsToErase)
    {
      segmentsToErase = new List<ushort>();
      SortedList<ushort, byte[]> changedFlashBlocks = new SortedList<ushort, byte[]>();
      int sizeOfSegment = 128;
      for (int address = 6144; address < 6655; address += sizeOfSegment)
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

    internal byte[] Zip()
    {
      if (this.Version == null)
        return (byte[]) null;
      byte[] numArray1 = new byte[1]{ (byte) 1 };
      byte[] bytes = this.Version.GetBytes();
      byte[] numArray2 = Util.ConvertAll<bool, byte>(this.byteIsDefined, new Converter<bool, byte>(Convert.ToByte));
      byte[] memoryBytes = this.memoryBytes;
      byte[] buffer = new byte[1 + bytes.Length + 131070];
      numArray1.CopyTo((Array) buffer, 0);
      bytes.CopyTo((Array) buffer, numArray1.Length);
      numArray2.CopyTo((Array) buffer, numArray1.Length + bytes.Length);
      memoryBytes.CopyTo((Array) buffer, numArray1.Length + bytes.Length + numArray2.Length);
      return Util.Zip(buffer);
    }

    internal static TH_MemoryMap Unzip(byte[] buffer)
    {
      if (buffer == null)
        return (TH_MemoryMap) null;
      byte[] numArray1 = Util.Unzip(buffer);
      if (numArray1 == null || numArray1.Length == 0 || numArray1[0] != (byte) 1)
        return (TH_MemoryMap) null;
      int offset = 1;
      TH_Version version = TH_Version.Parse(numArray1, ref offset);
      if (version == null)
        return (TH_MemoryMap) null;
      TH_MemoryMap thMemoryMap = TH_MemoryMap.Create(version);
      byte[] numArray2 = new byte[(int) ushort.MaxValue];
      Buffer.BlockCopy((Array) numArray1, offset, (Array) numArray2, 0, numArray2.Length);
      thMemoryMap.byteIsDefined = Util.ConvertAll<byte, bool>(numArray2, new Converter<byte, bool>(Convert.ToBoolean));
      Buffer.BlockCopy((Array) numArray1, offset + (int) ushort.MaxValue, (Array) thMemoryMap.memoryBytes, 0, thMemoryMap.memoryBytes.Length);
      return thMemoryMap;
    }

    internal Parameter GetParameter(string name)
    {
      return this.Parameters.Find((Predicate<Parameter>) (e => e.Name == name)) ?? throw new Exception(Ot.Gtm(Tg.HandlerLogic, "MissingMapParameter", "Can not find the parameter in the MAP! Parameter name:") + " " + name);
    }

    internal Parameter GetParameter(ushort address)
    {
      return this.Parameters.Find((Predicate<Parameter>) (e => (int) e.Address == (int) address)) ?? throw new Exception(Ot.Gtm(Tg.HandlerLogic, "MissingMapParameter", "Can not find the parameter in the MAP! Parameter address:") + " 0x" + address.ToString("X4"));
    }

    internal RangeSet<ushort> GetMemoryBlocksToRead(bool readAllData)
    {
      bool[] points = new bool[(int) ushort.MaxValue];
      for (int index = 0; index < points.Length; ++index)
        points[index] = false;
      foreach (Parameter parameter in this.Parameters)
      {
        if (!(parameter.Segment == "SEGMENT"))
        {
          for (int address = (int) parameter.Address; address < (int) parameter.Address + parameter.Size; ++address)
            points[address] = true;
        }
      }
      for (int index = 6528; index <= 6655; ++index)
        points[index] = true;
      if (!readAllData)
        ;
      return TH_MemoryMap.ConvertBoolArrayToRangeSet(points) ?? (RangeSet<ushort>) null;
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
  }
}
