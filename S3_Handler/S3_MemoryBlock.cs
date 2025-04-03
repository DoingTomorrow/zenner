// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_MemoryBlock
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using NLog;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class S3_MemoryBlock
  {
    private static Logger S3_MemoryBlockLogger = LogManager.GetLogger(nameof (S3_MemoryBlock));
    internal static S3_MemoryBlock NoSource = new S3_MemoryBlock((S3_Meter) null, S3_MemorySegment.CompleteMeter);
    internal S3_Meter MyMeter;
    internal S3_MemorySegment SegmentType;
    internal int iBlockStartAddress = 0;
    internal int iStartAddressOfNextBlock = 0;
    internal int Alignment = 2;
    internal bool IsHardLinkedAddress = false;
    internal bool IsNotLinked = false;
    internal bool IsFixSize = false;
    internal S3_MemoryBlock parentMemoryBlock;
    internal S3_MemoryBlock sourceMemoryBlock;
    internal List<S3_MemoryBlock> childMemoryBlocks;
    internal int firstChildMemoryBlockOffset = 0;

    internal S3_MemoryBlock(S3_Meter MyMeter, S3_MemorySegment SegmentType)
    {
      this.MyMeter = MyMeter;
      this.SegmentType = SegmentType;
    }

    internal S3_MemoryBlock(S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock)
    {
      this.MyMeter = MyMeter;
      this.SegmentType = parentMemoryBlock.SegmentType;
      this.parentMemoryBlock = parentMemoryBlock;
      parentMemoryBlock.Add(this.SegmentType, this);
    }

    internal S3_MemoryBlock(
      S3_Meter MyMeter,
      S3_MemorySegment SegmentType,
      S3_MemoryBlock parentMemoryBlock)
    {
      this.MyMeter = MyMeter;
      this.SegmentType = SegmentType;
      this.ByteSize = this.ByteSize;
      this.parentMemoryBlock = parentMemoryBlock;
      parentMemoryBlock.Add(SegmentType, this);
    }

    internal S3_MemoryBlock(
      S3_Meter MyMeter,
      S3_MemorySegment SegmentType,
      S3_MemoryBlock parentMemoryBlock,
      int insertIndex)
    {
      this.MyMeter = MyMeter;
      this.SegmentType = SegmentType;
      this.ByteSize = this.ByteSize;
      this.parentMemoryBlock = parentMemoryBlock;
      parentMemoryBlock.Insert(SegmentType, this, insertIndex);
    }

    internal S3_MemoryBlock(
      S3_Meter MyMeter,
      int ByteSize,
      S3_MemorySegment SegmentType,
      S3_MemoryBlock parentMemoryBlock)
    {
      this.MyMeter = MyMeter;
      this.SegmentType = SegmentType;
      this.ByteSize = ByteSize;
      this.parentMemoryBlock = parentMemoryBlock;
      parentMemoryBlock.Add(SegmentType, this);
    }

    internal S3_MemoryBlock(
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock,
      S3_MemoryBlock sourceMemoryBlock)
    {
      this.MyMeter = MyMeter;
      this.BlockStartAddress = sourceMemoryBlock.BlockStartAddress;
      this.ByteSize = sourceMemoryBlock.ByteSize;
      this.IsHardLinkedAddress = sourceMemoryBlock.IsHardLinkedAddress;
      this.firstChildMemoryBlockOffset = sourceMemoryBlock.firstChildMemoryBlockOffset;
      this.sourceMemoryBlock = sourceMemoryBlock;
      this.SegmentType = sourceMemoryBlock.SegmentType;
      if (parentMemoryBlock == null)
        return;
      this.parentMemoryBlock = parentMemoryBlock;
      parentMemoryBlock.Add(this.SegmentType, this);
    }

    internal S3_MemoryBlock(
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock,
      S3_MemoryBlock sourceMemoryBlock,
      bool byteSizeFromSourceBlock)
    {
      this.MyMeter = MyMeter;
      this.BlockStartAddress = sourceMemoryBlock.BlockStartAddress;
      this.IsHardLinkedAddress = sourceMemoryBlock.IsHardLinkedAddress;
      this.Alignment = sourceMemoryBlock.Alignment;
      this.firstChildMemoryBlockOffset = sourceMemoryBlock.firstChildMemoryBlockOffset;
      this.sourceMemoryBlock = sourceMemoryBlock;
      this.SegmentType = sourceMemoryBlock.SegmentType;
      if (byteSizeFromSourceBlock)
        this.ByteSize = sourceMemoryBlock.ByteSize;
      if (parentMemoryBlock == null)
        return;
      this.parentMemoryBlock = parentMemoryBlock;
      parentMemoryBlock.Add(this.SegmentType, this);
    }

    internal int BlockStartAddress
    {
      set
      {
        if (this.iBlockStartAddress == value)
          return;
        if (this.IsHardLinkedAddress)
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Change of static address: 0x" + this.iBlockStartAddress.ToString("X4"), S3_MemoryBlock.S3_MemoryBlockLogger);
        int num = this.iStartAddressOfNextBlock - this.iBlockStartAddress;
        this.iBlockStartAddress = value;
        this.iStartAddressOfNextBlock = this.iBlockStartAddress + num;
        this.WortParentBlockNextBlockAddresses();
      }
      get => this.iBlockStartAddress;
    }

    internal int ByteSize
    {
      get => this.StartAddressOfNextBlock - this.iBlockStartAddress;
      set
      {
        this.iStartAddressOfNextBlock = this.iBlockStartAddress + value;
        this.WortParentBlockNextBlockAddresses();
      }
    }

    internal int StartAddressOfNextBlock
    {
      get => this.iStartAddressOfNextBlock;
      set
      {
        if (this.iStartAddressOfNextBlock == value)
          return;
        if (this.ByteSize > 0 && this.IsFixSize)
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Change of fixed block size", S3_MemoryBlock.S3_MemoryBlockLogger);
        this.iStartAddressOfNextBlock = value;
        this.WortParentBlockNextBlockAddresses();
      }
    }

    private void WortParentBlockNextBlockAddresses()
    {
      S3_MemoryBlock parentMemoryBlock = this.parentMemoryBlock;
      if (parentMemoryBlock == null || parentMemoryBlock.StartAddressOfNextBlock >= this.StartAddressOfNextBlock)
        return;
      parentMemoryBlock.StartAddressOfNextBlock = this.StartAddressOfNextBlock;
    }

    internal void Add(S3_MemorySegment theSegment, S3_MemoryBlock memoryBlockToAdd)
    {
      try
      {
        if (this.childMemoryBlocks == null)
          this.childMemoryBlocks = new List<S3_MemoryBlock>();
        int num = this.childMemoryBlocks.Count != 0 ? this.childMemoryBlocks[this.childMemoryBlocks.Count - 1].StartAddressOfNextBlock : this.BlockStartAddress + this.firstChildMemoryBlockOffset;
        memoryBlockToAdd.parentMemoryBlock = this;
        this.childMemoryBlocks.Add(memoryBlockToAdd);
        if (memoryBlockToAdd.BlockStartAddress == 0 && !memoryBlockToAdd.IsHardLinkedAddress)
          memoryBlockToAdd.BlockStartAddress = num;
        if (memoryBlockToAdd.StartAddressOfNextBlock <= this.StartAddressOfNextBlock)
          return;
        this.StartAddressOfNextBlock = memoryBlockToAdd.StartAddressOfNextBlock;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ex.ToString());
        ZR_ClassLibMessages.AddErrorDescriptionAndException(ZR_ClassLibMessages.LastErrors.IllegalData, "Error on add S3_MemorySegment", S3_MemoryBlock.S3_MemoryBlockLogger);
      }
    }

    internal void Insert(
      S3_MemorySegment theSegment,
      S3_MemoryBlock memoryBlockToAdd,
      int insertIndex)
    {
      try
      {
        if (this.childMemoryBlocks == null)
          this.childMemoryBlocks = new List<S3_MemoryBlock>();
        int num = this.childMemoryBlocks.Count != 0 ? this.childMemoryBlocks[this.childMemoryBlocks.Count - 1].StartAddressOfNextBlock : this.BlockStartAddress + this.firstChildMemoryBlockOffset;
        memoryBlockToAdd.parentMemoryBlock = this;
        this.childMemoryBlocks.Insert(insertIndex, memoryBlockToAdd);
        if (memoryBlockToAdd.BlockStartAddress != 0 || memoryBlockToAdd.IsHardLinkedAddress)
          return;
        memoryBlockToAdd.BlockStartAddress = num;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ex.ToString());
        ZR_ClassLibMessages.AddErrorDescriptionAndException(ZR_ClassLibMessages.LastErrors.IllegalData, "Error on insert S3_MemorySegment", S3_MemoryBlock.S3_MemoryBlockLogger);
      }
    }

    internal bool Fill(byte fillByte)
    {
      for (int blockStartAddress = this.BlockStartAddress; blockStartAddress < this.BlockStartAddress + this.ByteSize; ++blockStartAddress)
      {
        this.MyMeter.MyDeviceMemory.MemoryBytes[blockStartAddress] = fillByte;
        this.MyMeter.MyDeviceMemory.ByteIsDefined[blockStartAddress] = true;
      }
      return true;
    }

    internal bool RemoveFromParentMemoryBlock()
    {
      if (this.parentMemoryBlock != null)
      {
        if (!this.parentMemoryBlock.childMemoryBlocks.Remove(this))
          return false;
        this.parentMemoryBlock = (S3_MemoryBlock) null;
      }
      return true;
    }

    internal bool InsertDataFromSourceMemoryBlockIncludeChildBlocks()
    {
      foreach (S3_MemoryBlock s3MemoryBlock in S3_Linker.ForEachMemoryBlock(this))
      {
        if (!s3MemoryBlock.InsertDataFromSourceMemoryBlock())
          return false;
      }
      return true;
    }

    internal bool InsertDataFromSourceMemoryBlock()
    {
      if (this.sourceMemoryBlock == null)
      {
        for (int index = 0; index < this.ByteSize; ++index)
        {
          this.MyMeter.MyDeviceMemory.MemoryBytes[this.BlockStartAddress + index] = byte.MaxValue;
          this.MyMeter.MyDeviceMemory.ByteIsDefined[this.BlockStartAddress + index] = true;
        }
        return true;
      }
      if (this.sourceMemoryBlock.firstChildMemoryBlockOffset != this.firstChildMemoryBlockOffset)
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "InsertDataFromSourceMemoryBlock with different firstChildMemoryBlockOffset on address: 0x" + this.BlockStartAddress.ToString("x04"), S3_MemoryBlock.S3_MemoryBlockLogger);
      int num;
      if (this.childMemoryBlocks != null)
      {
        if (this.childMemoryBlocks.Count == 0)
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "InsertDataFromSourceMemoryBlock with childMemoryBlocks.Count == 0 on address: 0x" + this.BlockStartAddress.ToString("x04"), S3_MemoryBlock.S3_MemoryBlockLogger);
        if (this.firstChildMemoryBlockOffset == 0)
          return true;
        num = this.firstChildMemoryBlockOffset;
      }
      else
      {
        if (this.sourceMemoryBlock.ByteSize != this.ByteSize)
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "InsertDataFromSourceMemoryBlock with different ByteSize on address: 0x" + this.BlockStartAddress.ToString("x04"), S3_MemoryBlock.S3_MemoryBlockLogger);
        num = this.ByteSize;
      }
      for (int index = 0; index < num; ++index)
      {
        this.MyMeter.MyDeviceMemory.MemoryBytes[this.BlockStartAddress + index] = this.sourceMemoryBlock.MyMeter.MyDeviceMemory.MemoryBytes[this.sourceMemoryBlock.BlockStartAddress + index];
        this.MyMeter.MyDeviceMemory.ByteIsDefined[this.BlockStartAddress + index] = true;
      }
      return true;
    }

    internal void Clear()
    {
      if (this.childMemoryBlocks != null)
        this.childMemoryBlocks.Clear();
      this.ByteSize = 0;
      this.firstChildMemoryBlockOffset = 0;
    }

    public override string ToString()
    {
      return "0x" + this.iBlockStartAddress.ToString("X4") + "-0x" + this.StartAddressOfNextBlock.ToString("X4") + " Size: " + this.ByteSize.ToString() + " " + this.SegmentType.ToString() + " " + (this.IsHardLinkedAddress ? "Hard Linked" : "");
    }
  }
}
