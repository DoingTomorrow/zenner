// Decompiled with JetBrains decompiler
// Type: S3_Handler.RadioListHeader
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using HandlerLib;
using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal sealed class RadioListHeader : S3_MemoryBlock
  {
    private string scenarioName;

    internal string Name
    {
      get
      {
        string name = string.Empty;
        foreach (S3_MemoryBlock childMemoryBlock in this.parentMemoryBlock.childMemoryBlocks)
        {
          if (childMemoryBlock == this)
          {
            name = !this.Mode.ToString().StartsWith("Radio3_") ? this.Mode.ToString() + "_" + this.ScenarioName : this.Mode.ToString();
            break;
          }
        }
        return name;
      }
    }

    internal bool IsSelected { get; set; }

    internal bool IsSelected_Memory
    {
      get
      {
        return (int) this.MyMeter.MyParameters.ParameterByName["RadioTransmitList"].GetUshortValue() == this.BlockStartAddress + this.ScenarioNameLength + 2;
      }
      set
      {
        if (!value)
          return;
        foreach (S3_MemoryBlock childMemoryBlock in this.parentMemoryBlock.childMemoryBlocks)
        {
          if (childMemoryBlock is RadioListHeader)
            ((RadioListHeader) childMemoryBlock).IsSelected = false;
        }
        this.MyMeter.MyParameters.ParameterByName["RadioTransmitList"].SetUshortValue((ushort) (this.BlockStartAddress + this.ScenarioNameLength + 2));
        this.IsSelected = value;
      }
    }

    internal RADIO_MODE Mode { get; set; }

    internal RADIO_MODE Mode_Memory
    {
      get
      {
        return (RADIO_MODE) Enum.ToObject(typeof (RADIO_MODE), this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress + this.ScenarioNameLength));
      }
      set
      {
        this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress + this.ScenarioNameLength, (ushort) value);
        this.Mode = value;
      }
    }

    internal string ScenarioName
    {
      get => this.scenarioName;
      set
      {
        this.scenarioName = value;
        this.ScenarioNameLength = value.Length % 2 != 0 ? value.Length + 1 : value.Length + 2;
        this.firstChildMemoryBlockOffset = this.ScenarioNameLength + 2;
      }
    }

    internal string ScenarioName_Memory
    {
      get
      {
        byte[] bytes = new byte[this.ScenarioNameLength - 1];
        for (int index = 1; index < this.ScenarioNameLength; ++index)
          bytes[index - 1] = this.MyMeter.MyDeviceMemory.GetByteValue(this.BlockStartAddress + index);
        return Encoding.ASCII.GetString(bytes, 0, bytes.Length).Trim();
      }
      set
      {
        string s = value;
        if (value.Length % 2 == 0)
          s += " ";
        this.MyMeter.MyDeviceMemory.SetByteArray(this.BlockStartAddress + 1, Encoding.ASCII.GetBytes(s));
        if (this.ScenarioName != value)
          throw new Exception("Illegal ScenarioName change");
      }
    }

    internal int ScenarioNameLength { get; set; }

    internal int ScenarioNameLength_Memory
    {
      get => (int) this.MyMeter.MyDeviceMemory.GetByteValue(this.BlockStartAddress);
      set
      {
        this.MyMeter.MyDeviceMemory.SetByteValue(this.BlockStartAddress, Convert.ToByte(value));
        if (this.ScenarioNameLength != value)
          throw new Exception("Illegal ScenarioNameLength change");
      }
    }

    public RadioListHeader(S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock)
      : base(MyMeter, S3_MemorySegment.RadioListHeader, parentMemoryBlock)
    {
    }

    public RadioListHeader(
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock,
      S3_MemoryBlock sourceMemoryBlock)
      : base(MyMeter, parentMemoryBlock, sourceMemoryBlock)
    {
    }

    public RadioListHeader(S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock, int insertIndex)
      : base(MyMeter, S3_MemorySegment.RadioListHeader, parentMemoryBlock, insertIndex)
    {
    }

    internal bool CreateFromMemory(int pos)
    {
      this.ScenarioNameLength = this.ScenarioNameLength_Memory;
      this.ScenarioName = this.ScenarioName_Memory;
      this.Mode = this.Mode_Memory;
      this.IsSelected = this.IsSelected_Memory;
      this.ByteSize = this.ScenarioNameLength + 2;
      this.firstChildMemoryBlockOffset = this.ScenarioNameLength + 2;
      if (this.ScenarioName == "Sz0" && this.Mode != 0)
        throw new Exception("Illegal radio2 Sz0 combination!");
      if (this.ScenarioName == "Sz5" && this.Mode != RADIO_MODE.Radio3_Sz5)
        throw new Exception("Illegal radio3 Sz5 combination!");
      for (ushort ushortValue = this.MyMeter.MyDeviceMemory.GetUshortValue(this.StartAddressOfNextBlock); ushortValue > (ushort) 0; ushortValue = this.MyMeter.MyDeviceMemory.GetUshortValue(this.StartAddressOfNextBlock))
        new RadioListHeaderItem(this.MyMeter, (S3_MemoryBlock) this).CreateFromMemory();
      S3_DataBlock s3DataBlock = new S3_DataBlock(new byte[2], this.MyMeter, (S3_MemoryBlock) this);
      return true;
    }

    internal RadioListHeader Clone(S3_Meter theCloneMeter, S3_MemoryBlock parentMemoryBlock)
    {
      RadioListHeader cloneParentMemoryBlock = new RadioListHeader(theCloneMeter, parentMemoryBlock, (S3_MemoryBlock) this);
      cloneParentMemoryBlock.ScenarioNameLength = this.ScenarioNameLength;
      cloneParentMemoryBlock.ScenarioName = this.ScenarioName;
      cloneParentMemoryBlock.Mode = this.Mode;
      cloneParentMemoryBlock.IsSelected = this.IsSelected;
      if (this.childMemoryBlocks != null)
      {
        foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
        {
          if (childMemoryBlock is RadioListHeaderItem)
            ((RadioListHeaderItem) childMemoryBlock).Clone(theCloneMeter, (S3_MemoryBlock) cloneParentMemoryBlock);
          if (childMemoryBlock is S3_DataBlock)
            ((S3_DataBlock) childMemoryBlock).Clone(theCloneMeter, (S3_MemoryBlock) cloneParentMemoryBlock);
        }
      }
      if (this.sourceMemoryBlock != null)
        cloneParentMemoryBlock.sourceMemoryBlock = this.sourceMemoryBlock;
      return cloneParentMemoryBlock;
    }

    internal void LinkPointers()
    {
      if (this.childMemoryBlocks == null)
        return;
      foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
      {
        if (childMemoryBlock is RadioListHeaderItem)
          ((RadioListHeaderItem) childMemoryBlock).LinkPointers();
      }
    }

    internal bool InsertData(ref Dictionary<int, string> pointer)
    {
      this.ScenarioNameLength_Memory = this.ScenarioNameLength;
      this.ScenarioName_Memory = this.ScenarioName;
      this.MyMeter.MyLinker.AddLabel(this.Name, this.BlockStartAddress + this.ScenarioNameLength);
      this.Mode_Memory = this.Mode;
      this.IsSelected_Memory = this.IsSelected;
      if (this.childMemoryBlocks != null)
      {
        foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
        {
          if (childMemoryBlock is RadioListHeaderItem)
          {
            if (!(childMemoryBlock as RadioListHeaderItem).InsertData(ref pointer))
              return false;
          }
          else if (childMemoryBlock is S3_DataBlock && !(childMemoryBlock as S3_DataBlock).InsertData())
            return false;
        }
      }
      return true;
    }

    internal RadioListHeaderItem InsertList(int position)
    {
      RadioListHeaderItem radioListHeaderItem = new RadioListHeaderItem(this.MyMeter, (S3_MemoryBlock) this, position);
      radioListHeaderItem.AddressOfIdentNo = (ushort) this.MyMeter.MyParameters.ParameterByName["SerDev0_IdentNo"].BlockStartAddress;
      radioListHeaderItem.RadioIdOffset = (ushort) 0;
      S3_DataBlock s3DataBlock = new S3_DataBlock(new byte[2], this.MyMeter, (S3_MemoryBlock) new RadioList(this.MyMeter, this.parentMemoryBlock, this.parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) this) + 1));
      return radioListHeaderItem;
    }

    internal void PrintRadioList(StringBuilder printText, string startString)
    {
      if (this.childMemoryBlocks == null)
        return;
      string startString1 = startString + "   ";
      foreach (S3_MemoryBlock childMemoryBlock1 in this.childMemoryBlocks)
      {
        if (childMemoryBlock1 is RadioListHeaderItem)
        {
          printText.AppendLine(startString1 + ((RadioListHeaderItem) childMemoryBlock1).Description);
          int num1 = childMemoryBlock1.parentMemoryBlock.childMemoryBlocks.IndexOf(childMemoryBlock1) + 1;
          int num2 = this.parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) this);
          S3_MemoryBlock childMemoryBlock2 = childMemoryBlock1.parentMemoryBlock.parentMemoryBlock.childMemoryBlocks[num2 + num1];
          if (childMemoryBlock2 is RadioList)
            ((RadioList) childMemoryBlock2).PrintRadioListParameter(printText, startString1);
        }
      }
    }

    public override string ToString() => this.Name;
  }
}
