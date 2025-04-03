// Decompiled with JetBrains decompiler
// Type: S3_Handler.MBusTransmitter
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal sealed class MBusTransmitter : S3_MemoryBlock
  {
    public string Name { get; private set; }

    public string KeyOfParameterPrimaryAddress { get; private set; }

    public MBusTransmitter(
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock,
      string name,
      string keyOfParameterPrimaryAddress)
      : base(MyMeter, S3_MemorySegment.MBusTransmitter, parentMemoryBlock)
    {
      this.Name = name;
      this.KeyOfParameterPrimaryAddress = keyOfParameterPrimaryAddress;
    }

    public MBusTransmitter(
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock,
      S3_MemoryBlock sourceMemoryBlock,
      string name,
      string keyOfParameterPrimaryAddress)
      : base(MyMeter, parentMemoryBlock, sourceMemoryBlock)
    {
      this.Name = name;
      this.KeyOfParameterPrimaryAddress = keyOfParameterPrimaryAddress;
    }

    public MBusTransmitter(
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock,
      int insertIndex,
      string name,
      string keyOfParameterPrimaryAddress)
      : base(MyMeter, S3_MemorySegment.MBusTransmitter, parentMemoryBlock, insertIndex)
    {
      this.Name = name;
      this.KeyOfParameterPrimaryAddress = keyOfParameterPrimaryAddress;
    }

    internal bool CreateFromMemory()
    {
      ushort ushortValue = this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress);
      if (ushortValue == (ushort) 0)
      {
        this.ByteSize += 2;
        this.firstChildMemoryBlockOffset = this.ByteSize;
        return true;
      }
      this.StartAddressOfNextBlock = this.BlockStartAddress + 2 + (int) ushortValue * 2;
      this.firstChildMemoryBlockOffset = this.ByteSize;
      for (int index = 0; index < (int) ushortValue; ++index)
      {
        MBusList mbusList = new MBusList(this.MyMeter, (S3_MemoryBlock) this, this.Name + " List " + index.ToString());
        mbusList.BlockStartAddress = (int) this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress + 2 + index * 2);
        mbusList.CreateFromMemory();
      }
      return true;
    }

    internal bool InsertData(ref Dictionary<int, string> pointer)
    {
      this.MyMeter.MyLinker.AddLabel(this.Name, this.BlockStartAddress);
      this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress, this.childMemoryBlocks != null ? (ushort) this.childMemoryBlocks.Count : (ushort) 0);
      int num = this.BlockStartAddress + 2;
      if (this.childMemoryBlocks != null)
      {
        foreach (MBusList childMemoryBlock in this.childMemoryBlocks)
        {
          this.MyMeter.MyDeviceMemory.SetUshortValue(num, (ushort) 0);
          pointer.Add(num, childMemoryBlock.Name);
          num += 2;
        }
        foreach (MBusList childMemoryBlock in this.childMemoryBlocks)
        {
          if (!childMemoryBlock.InsertData(ref pointer))
            return false;
        }
      }
      return true;
    }

    internal ushort GetAddressOfSelectedList()
    {
      if (this.childMemoryBlocks == null || this.childMemoryBlocks.Count == 0)
        return 0;
      foreach (MBusList childMemoryBlock in this.childMemoryBlocks)
      {
        if (childMemoryBlock.IsSelected)
          return (ushort) childMemoryBlock.BlockStartAddress;
      }
      return (ushort) this.childMemoryBlocks[0].BlockStartAddress;
    }

    internal MBusTransmitter Clone(S3_Meter theCloneMeter, S3_MemoryBlock parentMemoryBlock)
    {
      MBusTransmitter parentMemoryBlock1 = new MBusTransmitter(theCloneMeter, parentMemoryBlock, (S3_MemoryBlock) this, this.Name, this.KeyOfParameterPrimaryAddress);
      if (this.childMemoryBlocks != null)
      {
        foreach (MBusList childMemoryBlock in this.childMemoryBlocks)
          childMemoryBlock.Clone(theCloneMeter, (S3_MemoryBlock) parentMemoryBlock1);
      }
      return parentMemoryBlock1;
    }

    internal MBusList InsertNewMBusList(int insertIndex)
    {
      string str1 = this.Name + " List ";
      int count = this.childMemoryBlocks != null ? this.childMemoryBlocks.Count : 0;
      string str2 = str1;
      int num1 = count;
      int num2 = num1 + 1;
      string str3 = num1.ToString();
      string uniqueNameOfList = str2 + str3;
      while (this.childMemoryBlocks != null && this.childMemoryBlocks.Exists((Predicate<S3_MemoryBlock>) (e => ((MBusList) e).Name == uniqueNameOfList)))
        uniqueNameOfList = this.Name + " List " + num2++.ToString();
      MBusList parentMemoryBlock = new MBusList(this.MyMeter, (S3_MemoryBlock) this, insertIndex, uniqueNameOfList);
      parentMemoryBlock.VirtualDeviceNumber = this.VirtualDeviceNumber;
      S3_DataBlock s3DataBlock = new S3_DataBlock(new byte[2], this.MyMeter, (S3_MemoryBlock) parentMemoryBlock);
      this.ByteSize += 2;
      this.firstChildMemoryBlockOffset += 2;
      return parentMemoryBlock;
    }

    public byte VirtualDeviceNumber
    {
      get
      {
        if (string.IsNullOrEmpty(this.KeyOfParameterPrimaryAddress) || this.KeyOfParameterPrimaryAddress == "SerDev0_SelectedList_PrimaryAddress")
          return 0;
        if (this.KeyOfParameterPrimaryAddress == "SerDev1_SelectedList_PrimaryAddress")
          return 1;
        if (this.KeyOfParameterPrimaryAddress == "SerDev2_SelectedList_PrimaryAddress")
          return 2;
        if (this.KeyOfParameterPrimaryAddress == "SerDev3_SelectedList_PrimaryAddress")
          return 3;
        throw new Exception("INTERNAL ERROR: Unknown KeyOfParameterPrimaryAddress detected! Value: " + this.KeyOfParameterPrimaryAddress);
      }
    }

    internal void PrintTransmitterMBusList(
      StringBuilder printText,
      string transmitterName,
      string startString)
    {
      if (this.childMemoryBlocks == null)
        return;
      string startString1 = startString + "   ";
      printText.AppendLine();
      printText.AppendLine(startString + transmitterName);
      int num = 1;
      foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
      {
        if (childMemoryBlock is MBusList)
        {
          ((MBusList) childMemoryBlock).PrintMBusList(printText, transmitterName + "_" + num.ToString(), startString1);
          ++num;
        }
      }
    }

    internal void SetAsActive(MBusList list)
    {
      if (this.childMemoryBlocks == null)
        return;
      foreach (MBusList childMemoryBlock in this.childMemoryBlocks)
        childMemoryBlock.IsSelected = false;
      list.IsSelected = true;
    }
  }
}
