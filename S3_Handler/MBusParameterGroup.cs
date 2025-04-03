// Decompiled with JetBrains decompiler
// Type: S3_Handler.MBusParameterGroup
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using DeviceCollector;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal sealed class MBusParameterGroup : S3_MemoryBlock
  {
    public string Name => "Parameter group";

    public int LoggerCycleCount { get; set; }

    public int ListMaxCount { get; set; }

    public bool IsDueDate { get; set; }

    public int StartStorageNumber { get; set; }

    public bool IsPrepared { get; set; }

    public int TotalCountOfValues => (this.LoggerCycleCount + 1) * this.ListMaxCount;

    public MBusParameterGroup(S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock)
      : base(MyMeter, S3_MemorySegment.MBusParameterGroup, parentMemoryBlock)
    {
      this.childMemoryBlocks = new List<S3_MemoryBlock>();
      this.IsPrepared = false;
    }

    public MBusParameterGroup(
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock,
      S3_MemoryBlock sourceMemoryBlock)
      : base(MyMeter, parentMemoryBlock, sourceMemoryBlock)
    {
      this.childMemoryBlocks = new List<S3_MemoryBlock>();
      this.IsPrepared = false;
    }

    public MBusParameterGroup(S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock, int insertIndex)
      : base(MyMeter, S3_MemorySegment.MBusParameterGroup, parentMemoryBlock, insertIndex)
    {
      this.childMemoryBlocks = new List<S3_MemoryBlock>();
      this.IsPrepared = false;
    }

    internal MBusParameterGroup Clone(S3_Meter theCloneMeter, S3_MemoryBlock parentMemoryBlock)
    {
      MBusParameterGroup parentMemoryBlock1 = new MBusParameterGroup(theCloneMeter, parentMemoryBlock, (S3_MemoryBlock) this);
      parentMemoryBlock1.IsPrepared = this.IsPrepared;
      parentMemoryBlock1.LoggerCycleCount = this.LoggerCycleCount;
      parentMemoryBlock1.ListMaxCount = this.ListMaxCount;
      parentMemoryBlock1.IsDueDate = this.IsDueDate;
      parentMemoryBlock1.StartStorageNumber = this.StartStorageNumber;
      if (this.childMemoryBlocks != null)
      {
        foreach (MBusParameter childMemoryBlock in this.childMemoryBlocks)
          childMemoryBlock.Clone(theCloneMeter, (S3_MemoryBlock) parentMemoryBlock1);
      }
      return parentMemoryBlock1;
    }

    internal bool InsertData(ref Dictionary<int, string> pointer)
    {
      if (this.childMemoryBlocks != null)
      {
        foreach (MBusParameter childMemoryBlock in this.childMemoryBlocks)
        {
          if (!childMemoryBlock.InsertData(ref pointer))
            return false;
        }
      }
      return true;
    }

    internal MBusParameter AddLogger(LoggerChanal loggerChanal, int? pos)
    {
      if (loggerChanal == null)
        throw new ArgumentNullException("The parameter 'loggerChanal' can not be null!");
      if (loggerChanal.myLoggerConfig == null)
        throw new ArgumentNullException("The parameter 'loggerChanal.myLoggerConfig' can not be null!");
      string intervalString = loggerChanal.myLoggerConfig.IntervalString;
      string loggerIntervalString = this.GetLoggerIntervalString();
      if (!string.IsNullOrEmpty(loggerIntervalString) && loggerIntervalString != intervalString)
        return (MBusParameter) null;
      S3_Parameter chanalParameter = loggerChanal.chanalParameter;
      if (chanalParameter == null || chanalParameter.Statics.DefaultDifVif == null || chanalParameter.Statics.DefaultDifVif.Length == 0)
        throw new ArgumentNullException("Invalid logger! Value: " + loggerChanal.ChanalName);
      this.Reduce();
      MBusParameter mbusParameter = !pos.HasValue ? new MBusParameter(this.MyMeter, (S3_MemoryBlock) this, 0) : new MBusParameter(this.MyMeter, (S3_MemoryBlock) this, pos.Value);
      mbusParameter.SourceItem = (S3_MemoryBlock) loggerChanal;
      mbusParameter.Name = loggerChanal.ChanalName;
      mbusParameter.IsLogger = true;
      mbusParameter.ControlWord0 = new ControlWord0()
      {
        DataCount = loggerChanal.chanalParameter.ByteSize,
        ControlLogger = ControlLogger.LoggerChangeChanal,
        DifVifCount = 0,
        IsBCDByRadio = false,
        ItFollowsNextControlWord = false,
        ParamCode = ParamCode.LogValue
      };
      byte virtualDeviceNumber = !(this.parentMemoryBlock is MBusList) ? (this.parentMemoryBlock as RadioList).VirtualDeviceNumber : (this.parentMemoryBlock as MBusList).VirtualDeviceNumber;
      byte[] loggerDifVif = mbusParameter.GenerateLoggerDifVif(virtualDeviceNumber);
      if (loggerDifVif == null)
      {
        mbusParameter.RemoveFromParentMemoryBlock();
        throw new ArgumentNullException("Invalid DIF VIF by the logger! Value: " + loggerChanal.ChanalName);
      }
      mbusParameter.ControlWord0.DifVifCount = loggerDifVif.Length;
      mbusParameter.VifDif = new List<byte>((IEnumerable<byte>) loggerDifVif);
      mbusParameter.ByteSize = 2 + mbusParameter.VifDif.Count + 2;
      if (mbusParameter.ByteSize % 2 != 0)
        ++mbusParameter.ByteSize;
      this.SetTotalCountOfValues(this.TotalCountOfValues);
      return mbusParameter;
    }

    private string GetLoggerIntervalString()
    {
      return this.childMemoryBlocks == null || this.childMemoryBlocks.Count == 0 ? string.Empty : ((this.childMemoryBlocks[0] as MBusParameter).SourceItem as LoggerChanal).myLoggerConfig.IntervalString;
    }

    internal void Reduce()
    {
      if (!this.IsPrepared)
        return;
      if (this.childMemoryBlocks == null || this.childMemoryBlocks.Count < 3)
        throw new Exception("Can not reduce parameter group! It should be at least 3 parameters.");
      for (int index = this.childMemoryBlocks.Count - 1; index >= 0; --index)
      {
        MBusParameter childMemoryBlock = this.childMemoryBlocks[index] as MBusParameter;
        if (childMemoryBlock.ControlWord0.ItFollowsNextControlWord && childMemoryBlock.ControlWord1.ItFollowsNextControlWord && childMemoryBlock.ControlWord2.IsSavePtr)
        {
          this.IsPrepared = false;
          childMemoryBlock.RemoveFromParentMemoryBlock();
          return;
        }
        childMemoryBlock.RemoveFromParentMemoryBlock();
      }
      throw new Exception("Invalid parameter group detected! Reduce failed.");
    }

    internal void Prepare()
    {
      if (this.IsPrepared)
        return;
      if (this.childMemoryBlocks == null || this.childMemoryBlocks.Count < 2)
        throw new Exception("Can not prepare parameter group! It should be at least 2 parameters.");
      if ((long) this.StartStorageNumber + (long) this.TotalCountOfValues > (long) ushort.MaxValue)
        throw new Exception("Can not prepare parameter group! Invalid start storage number.");
      int count = this.childMemoryBlocks.Count;
      MBusParameter childMemoryBlock1 = this.childMemoryBlocks[0] as MBusParameter;
      childMemoryBlock1.ControlWord0.ControlLogger = this.IsDueDate ? ControlLogger.LoggerDueDateReset : ControlLogger.LoggerReset;
      MBusDifVif mbusDifVif1 = new MBusDifVif();
      if (!mbusDifVif1.LoadDifVif(childMemoryBlock1.VifDif.ToArray()))
        return;
      mbusDifVif1.StorageNumber = this.StartStorageNumber;
      if (mbusDifVif1.DifByteSize < 3)
        mbusDifVif1.DifByteSize = 3;
      childMemoryBlock1.VifDif = new List<byte>((IEnumerable<byte>) mbusDifVif1.DifVifArray);
      childMemoryBlock1.RecalculateByteSize();
      for (int index = 1; index < count; ++index)
      {
        MBusParameter childMemoryBlock2 = this.childMemoryBlocks[index] as MBusParameter;
        childMemoryBlock2.ControlWord0.ControlLogger = ControlLogger.LoggerChangeChanal;
        MBusDifVif mbusDifVif2 = new MBusDifVif();
        if (!mbusDifVif2.LoadDifVif(childMemoryBlock2.VifDif.ToArray()))
          return;
        mbusDifVif2.StorageNumber = this.StartStorageNumber;
        if (mbusDifVif2.DifByteSize < 3)
          mbusDifVif2.DifByteSize = 3;
        childMemoryBlock2.VifDif = new List<byte>((IEnumerable<byte>) mbusDifVif2.DifVifArray);
        childMemoryBlock2.RecalculateByteSize();
      }
      MBusParameter mbusParameter1 = new MBusParameter(this.MyMeter, (S3_MemoryBlock) this)
      {
        ControlWord0 = new ControlWord0(childMemoryBlock1.ControlWord0.ControlWord)
      };
      mbusParameter1.ControlWord0.ControlLogger = ControlLogger.LoggerNextChangeChanal;
      mbusParameter1.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter1.ControlWord1 = new ControlWord1();
      mbusParameter1.ControlWord1.ItFollowsNextControlWord = true;
      mbusParameter1.ControlWord2 = new ControlWord2();
      mbusParameter1.ControlWord2.IsSavePtr = true;
      mbusParameter1.ControlWord2.IsAllowLoggerEnd = false;
      mbusParameter1.VifDif = new List<byte>((IEnumerable<byte>) childMemoryBlock1.VifDif.ToArray());
      mbusParameter1.IsLogger = childMemoryBlock1.IsLogger;
      mbusParameter1.Name = childMemoryBlock1.Name;
      mbusParameter1.RecalculateByteSize();
      for (int index = 1; index < count - 1; ++index)
      {
        MBusParameter childMemoryBlock3 = this.childMemoryBlocks[index] as MBusParameter;
        MBusParameter mbusParameter2 = new MBusParameter(this.MyMeter, (S3_MemoryBlock) this)
        {
          ControlWord0 = new ControlWord0(childMemoryBlock3.ControlWord0.ControlWord)
        };
        mbusParameter2.ControlWord0.ControlLogger = ControlLogger.LoggerChangeChanal;
        mbusParameter2.VifDif = new List<byte>((IEnumerable<byte>) childMemoryBlock3.VifDif.ToArray());
        mbusParameter2.IsLogger = childMemoryBlock3.IsLogger;
        mbusParameter2.Name = childMemoryBlock3.Name;
        mbusParameter2.RecalculateByteSize();
      }
      MBusParameter childMemoryBlock4 = this.childMemoryBlocks[count - 1] as MBusParameter;
      MBusParameter mbusParameter3 = new MBusParameter(this.MyMeter, (S3_MemoryBlock) this)
      {
        ControlWord0 = new ControlWord0(childMemoryBlock4.ControlWord0.ControlWord)
      };
      mbusParameter3.ControlWord0.ControlLogger = ControlLogger.LoggerChangeChanal;
      mbusParameter3.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter3.ControlWord1 = new ControlWord1();
      mbusParameter3.ControlWord1.LoggerCycleCount = this.LoggerCycleCount;
      mbusParameter3.ControlWord1.ItFollowsNextControlWord = true;
      mbusParameter3.ControlWord2 = new ControlWord2();
      mbusParameter3.ControlWord2.IsSavePtr = false;
      mbusParameter3.ControlWord2.IsAllowLoggerEnd = false;
      mbusParameter3.ControlWord2.ListMaxCount = this.ListMaxCount;
      mbusParameter3.VifDif = new List<byte>((IEnumerable<byte>) childMemoryBlock4.VifDif.ToArray());
      mbusParameter3.IsLogger = childMemoryBlock4.IsLogger;
      mbusParameter3.Name = childMemoryBlock4.Name;
      mbusParameter3.RecalculateByteSize();
      this.IsPrepared = true;
    }

    public int GetOutputBufferSize() => this.CalculateSize(this.LoggerCycleCount + 1);

    internal int CalculateSize(int repetitions)
    {
      if (this.childMemoryBlocks == null || this.childMemoryBlocks.Count == 0)
        return 0;
      int num = this.StartStorageNumber + this.TotalCountOfValues - 1;
      int size = 0;
      MBusDifVif mbusDifVif = new MBusDifVif();
      for (int index = 0; index < repetitions; ++index)
      {
        foreach (MBusParameter childMemoryBlock in this.childMemoryBlocks)
        {
          if (!childMemoryBlock.ControlWord0.ItFollowsNextControlWord || !childMemoryBlock.ControlWord1.ItFollowsNextControlWord || !childMemoryBlock.ControlWord2.IsSavePtr)
          {
            if (!mbusDifVif.LoadDifVif(childMemoryBlock.VifDif.ToArray()))
              return 0;
            mbusDifVif.StorageNumber = num;
            size += mbusDifVif.DifVifArray.Length + childMemoryBlock.ControlWord0.DataCount;
          }
          else
            break;
        }
      }
      return size;
    }

    internal void SetTotalCountOfValues(int totalCountOfValues)
    {
      if (totalCountOfValues < 2 || this.childMemoryBlocks == null || this.childMemoryBlocks.Count < 2)
        return;
      int valuesPerOneList = this.GetMaxLoggerValuesPerOneList();
      if (this.MyMeter.MyFunctions.baseTypeEditMode || totalCountOfValues <= valuesPerOneList)
      {
        this.LoggerCycleCount = totalCountOfValues - 1;
        this.ListMaxCount = 1;
      }
      else
      {
        this.LoggerCycleCount = valuesPerOneList - 1;
        if (totalCountOfValues % valuesPerOneList == 0)
        {
          this.ListMaxCount = totalCountOfValues / valuesPerOneList;
        }
        else
        {
          double num = (double) totalCountOfValues / (double) valuesPerOneList;
          if (this.ListMaxCount > 1 && num < (double) this.ListMaxCount)
            this.ListMaxCount -= (int) Math.Ceiling((double) this.ListMaxCount - num);
          else
            this.ListMaxCount += (int) Math.Ceiling(num - (double) this.ListMaxCount);
        }
      }
      if (this.LoggerCycleCount == 0 || this.ListMaxCount == 0)
        throw new Exception("Invalid counter calculation in MBusParameterGroup!");
    }

    internal int GetMaxLoggerValuesPerOneList()
    {
      int size = this.CalculateSize(1);
      if (size <= 0)
        return 0;
      if (this.parentMemoryBlock is MBusList)
      {
        MBusList parentMemoryBlock = this.parentMemoryBlock as MBusList;
        return (254 - size - parentMemoryBlock.GetOutputBufferSize(true)) / size + 1;
      }
      RadioList parentMemoryBlock1 = this.parentMemoryBlock as RadioList;
      return (254 - size) / size + 1;
    }
  }
}
