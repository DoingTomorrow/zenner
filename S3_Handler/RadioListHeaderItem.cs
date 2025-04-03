// Decompiled with JetBrains decompiler
// Type: S3_Handler.RadioListHeaderItem
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using HandlerLib;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal sealed class RadioListHeaderItem : S3_MemoryBlock
  {
    internal string ParamNameOfIdentNo;

    internal string Name
    {
      get => (this.parentMemoryBlock as RadioListHeader).Name + " ID: " + this.ID.ToString("X8");
    }

    internal string Description
    {
      get
      {
        string str = (this.IndexOfVirtualDevice != 0 ? "Pulse input " + this.IndexOfVirtualDevice.ToString() : "Heat meter ") + " (ID: " + this.ID.ToString("X8");
        if (((RadioListHeader) this.parentMemoryBlock).Mode == RADIO_MODE.Radio3_Sz0 || ((RadioListHeader) this.parentMemoryBlock).Mode == RADIO_MODE.Radio3 || ((RadioListHeader) this.parentMemoryBlock).Mode == RADIO_MODE.Radio3_Sz5)
          str = str + " Radio ID: " + (this.RadioID + (uint) this.RadioIdOffset).ToString("X8");
        return str + ")";
      }
    }

    public int IndexOfVirtualDevice
    {
      get
      {
        if ((int) this.AddressOfIdentNo == (int) (ushort) this.MyMeter.MyParameters.ParameterByName["SerDev0_IdentNo"].BlockStartAddress)
        {
          this.ParamNameOfIdentNo = "SerDev0_IdentNo";
          return 0;
        }
        if ((int) this.AddressOfIdentNo == (int) (ushort) this.MyMeter.MyParameters.ParameterByName["SerDev1_IdentNo"].BlockStartAddress)
        {
          this.ParamNameOfIdentNo = "SerDev1_IdentNo";
          return 1;
        }
        if ((int) this.AddressOfIdentNo == (int) (ushort) this.MyMeter.MyParameters.ParameterByName["SerDev2_IdentNo"].BlockStartAddress)
        {
          this.ParamNameOfIdentNo = "SerDev2_IdentNo";
          return 2;
        }
        if ((int) this.AddressOfIdentNo != (int) (ushort) this.MyMeter.MyParameters.ParameterByName["SerDev3_IdentNo"].BlockStartAddress)
          return -1;
        this.ParamNameOfIdentNo = "SerDev3_IdentNo";
        return 3;
      }
      set
      {
        if (value < 0 || value > 3)
          return;
        switch (value)
        {
          case 0:
            this.AddressOfIdentNo = (ushort) this.MyMeter.MyParameters.ParameterByName["SerDev0_IdentNo"].BlockStartAddress;
            this.ParamNameOfIdentNo = "SerDev0_IdentNo";
            break;
          case 1:
            this.AddressOfIdentNo = (ushort) this.MyMeter.MyParameters.ParameterByName["SerDev1_IdentNo"].BlockStartAddress;
            this.ParamNameOfIdentNo = "SerDev1_IdentNo";
            break;
          case 2:
            this.AddressOfIdentNo = (ushort) this.MyMeter.MyParameters.ParameterByName["SerDev2_IdentNo"].BlockStartAddress;
            this.ParamNameOfIdentNo = "SerDev2_IdentNo";
            break;
          case 3:
            this.AddressOfIdentNo = (ushort) this.MyMeter.MyParameters.ParameterByName["SerDev3_IdentNo"].BlockStartAddress;
            this.ParamNameOfIdentNo = "SerDev3_IdentNo";
            break;
        }
      }
    }

    internal uint ID
    {
      get
      {
        return !this.MyMeter.MyParameters.ParameterByAddress.ContainsKey((int) this.AddressOfIdentNo) ? 0U : this.MyMeter.MyParameters.ParameterByAddress[(int) this.AddressOfIdentNo].GetUintValue();
      }
    }

    internal uint RadioID
    {
      get
      {
        return !this.MyMeter.MyParameters.ParameterByAddress.ContainsKey((int) this.AddressOfIdentNo + 12) ? 0U : this.MyMeter.MyParameters.ParameterByAddress[(int) this.AddressOfIdentNo + 12].GetUintValue();
      }
    }

    internal ushort AddressOfIdentNo { get; set; }

    internal ushort AddressOfIdentNo_Memory
    {
      get => this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress);
      set
      {
        this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress, value);
        this.AddressOfIdentNo = value;
      }
    }

    internal ushort AddressOfList { get; set; }

    internal ushort AddressOfList_Memory
    {
      get => this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress + 2);
      set
      {
        this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress + 2, value);
        this.AddressOfList = value;
      }
    }

    internal ushort RadioIdOffset { get; set; }

    internal ushort RadioIdOffset_Memory
    {
      get => this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress + 4);
      set
      {
        this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress + 4, value);
        this.RadioIdOffset = value;
      }
    }

    public RadioListHeaderItem(S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock)
      : base(MyMeter, S3_MemorySegment.RadioListHeaderItem, parentMemoryBlock)
    {
      this.ByteSize = 6;
    }

    public RadioListHeaderItem(
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock,
      S3_MemoryBlock sourceMemoryBlock)
      : base(MyMeter, parentMemoryBlock, sourceMemoryBlock)
    {
      this.ByteSize = 6;
    }

    public RadioListHeaderItem(S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock, int insertIndex)
      : base(MyMeter, S3_MemorySegment.RadioListHeaderItem, parentMemoryBlock, insertIndex)
    {
      this.ByteSize = 6;
    }

    internal bool CreateFromMemory()
    {
      this.AddressOfIdentNo = this.AddressOfIdentNo_Memory;
      this.AddressOfList = this.AddressOfList_Memory;
      this.RadioIdOffset = this.RadioIdOffset_Memory;
      return true;
    }

    internal RadioListHeaderItem Clone(
      S3_Meter theCloneMeter,
      S3_MemoryBlock cloneParentMemoryBlock)
    {
      RadioListHeaderItem radioListHeaderItem = new RadioListHeaderItem(theCloneMeter, cloneParentMemoryBlock, (S3_MemoryBlock) this);
      int indexOfVirtualDevice = this.IndexOfVirtualDevice;
      radioListHeaderItem.AddressOfIdentNo = this.AddressOfIdentNo;
      radioListHeaderItem.AddressOfList = this.AddressOfList;
      radioListHeaderItem.RadioIdOffset = this.RadioIdOffset;
      radioListHeaderItem.ParamNameOfIdentNo = this.ParamNameOfIdentNo;
      if (this.sourceMemoryBlock != null)
        radioListHeaderItem.sourceMemoryBlock = this.sourceMemoryBlock;
      return radioListHeaderItem;
    }

    internal void LinkPointers()
    {
      if (this.ParamNameOfIdentNo == null)
        return;
      this.AddressOfIdentNo = (ushort) this.MyMeter.MyParameters.ParameterByName[this.ParamNameOfIdentNo].BlockStartAddress;
      this.AddressOfIdentNo_Memory = this.AddressOfIdentNo;
    }

    internal bool InsertData(ref Dictionary<int, string> pointer)
    {
      this.AddressOfIdentNo_Memory = this.AddressOfIdentNo;
      this.AddressOfList_Memory = (ushort) 0;
      int num = this.parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) this);
      if (!(this.parentMemoryBlock.parentMemoryBlock.childMemoryBlocks[this.parentMemoryBlock.parentMemoryBlock.childMemoryBlocks.IndexOf(this.parentMemoryBlock) + num + 1] is RadioList childMemoryBlock))
        throw new Exception("INTERNAL ERROR: Invalid radio structure!");
      pointer.Add(this.BlockStartAddress + 2, childMemoryBlock.Name);
      this.RadioIdOffset_Memory = this.RadioIdOffset;
      return true;
    }
  }
}
