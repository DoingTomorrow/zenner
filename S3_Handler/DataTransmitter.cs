// Decompiled with JetBrains decompiler
// Type: S3_Handler.DataTransmitter
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using HandlerLib;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal sealed class DataTransmitter : S3_MemoryBlock
  {
    private static Logger logger = LogManager.GetLogger(nameof (DataTransmitter));
    private Dictionary<int, string> pointer;

    internal MBusList P2P
    {
      get
      {
        return this.childMemoryBlocks == null || this.childMemoryBlocks.Count <= 0 ? (MBusList) null : this.childMemoryBlocks[0] as MBusList;
      }
    }

    internal MBusTransmitter Heat
    {
      get
      {
        return this.childMemoryBlocks == null || this.childMemoryBlocks.Count <= 1 ? (MBusTransmitter) null : this.childMemoryBlocks[1] as MBusTransmitter;
      }
    }

    internal MBusTransmitter Input1
    {
      get
      {
        return this.childMemoryBlocks == null || this.childMemoryBlocks.Count <= 2 ? (MBusTransmitter) null : this.childMemoryBlocks[2] as MBusTransmitter;
      }
    }

    internal MBusTransmitter Input2
    {
      get
      {
        return this.childMemoryBlocks == null || this.childMemoryBlocks.Count <= 3 ? (MBusTransmitter) null : this.childMemoryBlocks[3] as MBusTransmitter;
      }
    }

    internal MBusTransmitter Input3
    {
      get
      {
        return this.childMemoryBlocks == null || this.childMemoryBlocks.Count <= 4 ? (MBusTransmitter) null : this.childMemoryBlocks[4] as MBusTransmitter;
      }
    }

    internal RadioTransmitter Radio
    {
      get
      {
        return this.childMemoryBlocks == null || this.childMemoryBlocks.Count <= 5 ? (RadioTransmitter) null : this.childMemoryBlocks[5] as RadioTransmitter;
      }
    }

    public DataTransmitter(S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock)
      : base(MyMeter, S3_MemorySegment.DataTransmitter, parentMemoryBlock)
    {
      this.Alignment = 1;
      this.pointer = new Dictionary<int, string>();
    }

    public DataTransmitter(
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock,
      S3_MemoryBlock sourceMemoryBlock)
      : base(MyMeter, parentMemoryBlock, sourceMemoryBlock)
    {
      this.Alignment = 1;
      this.pointer = new Dictionary<int, string>();
    }

    public DataTransmitter(S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock, int insertIndex)
      : base(MyMeter, S3_MemorySegment.DataTransmitter, parentMemoryBlock, insertIndex)
    {
      this.Alignment = 1;
      this.pointer = new Dictionary<int, string>();
    }

    internal bool CreateMbusTableFromMemory()
    {
      this.firstChildMemoryBlockOffset = 4;
      if (this.MyMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.SerDev1_IdentNo.ToString()))
        this.firstChildMemoryBlockOffset += 2;
      SortedList<string, S3_Parameter> parameterByName1 = this.MyMeter.MyParameters.ParameterByName;
      S3_ParameterNames s3ParameterNames = S3_ParameterNames.SerDev2_IdentNo;
      string key1 = s3ParameterNames.ToString();
      if (parameterByName1.ContainsKey(key1))
        this.firstChildMemoryBlockOffset += 2;
      int num;
      if (!this.MyMeter.MyIdentification.IsWR4)
      {
        SortedList<string, S3_Parameter> parameterByName2 = this.MyMeter.MyParameters.ParameterByName;
        s3ParameterNames = S3_ParameterNames.SerDev3_IdentNo;
        string key2 = s3ParameterNames.ToString();
        num = parameterByName2.ContainsKey(key2) ? 1 : 0;
      }
      else
        num = 1;
      if (num != 0)
        this.firstChildMemoryBlockOffset += 2;
      MBusList list = new MBusList(this.MyMeter, (S3_MemoryBlock) this, "P2P");
      list.CreateFromMemory();
      MBusTransmitter mbusTransmitter1 = new MBusTransmitter(this.MyMeter, (S3_MemoryBlock) this, "Heat meter", "SerDev0_SelectedList_PrimaryAddress");
      mbusTransmitter1.CreateFromMemory();
      MBusTransmitter mbusTransmitter2 = (MBusTransmitter) null;
      SortedList<string, S3_Parameter> parameterByName3 = this.MyMeter.MyParameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.SerDev1_IdentNo;
      string key3 = s3ParameterNames.ToString();
      if (parameterByName3.ContainsKey(key3) && this.MyMeter.MyIdentification.IsInput1Available)
      {
        mbusTransmitter2 = new MBusTransmitter(this.MyMeter, (S3_MemoryBlock) this, "Pulse input 1", "SerDev1_SelectedList_PrimaryAddress");
        mbusTransmitter2.CreateFromMemory();
      }
      MBusTransmitter mbusTransmitter3 = (MBusTransmitter) null;
      SortedList<string, S3_Parameter> parameterByName4 = this.MyMeter.MyParameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.SerDev2_IdentNo;
      string key4 = s3ParameterNames.ToString();
      if (parameterByName4.ContainsKey(key4) && this.MyMeter.MyIdentification.IsInput1Available)
      {
        mbusTransmitter3 = new MBusTransmitter(this.MyMeter, (S3_MemoryBlock) this, "Pulse input 2", "SerDev2_SelectedList_PrimaryAddress");
        mbusTransmitter3.CreateFromMemory();
      }
      MBusTransmitter mbusTransmitter4 = (MBusTransmitter) null;
      SortedList<string, S3_Parameter> parameterByName5 = this.MyMeter.MyParameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.SerDev3_IdentNo;
      string key5 = s3ParameterNames.ToString();
      if (parameterByName5.ContainsKey(key5) && this.MyMeter.MyIdentification.IsInput3Available)
      {
        mbusTransmitter4 = new MBusTransmitter(this.MyMeter, (S3_MemoryBlock) this, "Pulse input 3", "SerDev3_SelectedList_PrimaryAddress");
        mbusTransmitter4.CreateFromMemory();
      }
      if (this.MyMeter.MyIdentification.FirmwareVersion >= 83959813U)
      {
        ushort ushortValue = this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress);
        if (ushortValue != (ushort) 0 && ushortValue != ushort.MaxValue)
          new RadioTransmitter(this.MyMeter, (S3_MemoryBlock) this).CreateFromMemory();
      }
      if (mbusTransmitter1 != null && mbusTransmitter1.childMemoryBlocks != null)
      {
        foreach (MBusList childMemoryBlock in mbusTransmitter1.childMemoryBlocks)
          this.FixListLinkNames(childMemoryBlock);
      }
      if (mbusTransmitter2 != null && mbusTransmitter2.childMemoryBlocks != null)
      {
        foreach (MBusList childMemoryBlock in mbusTransmitter2.childMemoryBlocks)
          this.FixListLinkNames(childMemoryBlock);
      }
      if (mbusTransmitter3 != null && mbusTransmitter3.childMemoryBlocks != null)
      {
        foreach (MBusList childMemoryBlock in mbusTransmitter3.childMemoryBlocks)
          this.FixListLinkNames(childMemoryBlock);
      }
      if (mbusTransmitter4 != null && mbusTransmitter4.childMemoryBlocks != null)
      {
        foreach (MBusList childMemoryBlock in mbusTransmitter4.childMemoryBlocks)
          this.FixListLinkNames(childMemoryBlock);
      }
      if (list != null)
        this.FixListLinkNames(list);
      return true;
    }

    private void FixListLinkNames(MBusList list)
    {
      if (list.childMemoryBlocks == null)
        return;
      foreach (S3_MemoryBlock childMemoryBlock in list.childMemoryBlocks)
      {
        if (childMemoryBlock is ListLink)
        {
          MBusList listByAddresses = this.FindListByAddresses((S3_MemoryBlock) this, ((ListLink) childMemoryBlock).Address);
          if (listByAddresses != null)
            ((ListLink) childMemoryBlock).Name = listByAddresses.Name;
        }
      }
    }

    internal bool InsertData()
    {
      this.pointer.Clear();
      if (this.childMemoryBlocks == null || this.childMemoryBlocks.Count <= 0)
        return false;
      this.MyMeter.MyDeviceMemory.SetByteArray(this.BlockStartAddress, new byte[10]);
      if (!this.P2P.InsertData(ref this.pointer))
        return false;
      if (this.Radio != null)
      {
        if (!this.Radio.InsertData(ref this.pointer))
          return false;
        this.pointer.Add(this.BlockStartAddress, this.Radio.Name);
      }
      if (!this.Heat.InsertData(ref this.pointer))
        return false;
      this.pointer.Add(this.BlockStartAddress + 2, this.Heat.Name);
      if (this.Input1 != null && this.Input1.InsertData(ref this.pointer))
        this.pointer.Add(this.BlockStartAddress + 4, this.Input1.Name);
      if (this.Input2 != null && this.Input2.InsertData(ref this.pointer))
        this.pointer.Add(this.BlockStartAddress + 6, this.Input2.Name);
      if (this.Input3 != null)
      {
        if (!this.Input3.InsertData(ref this.pointer))
          return false;
        this.pointer.Add(this.BlockStartAddress + 8, this.Input3.Name);
      }
      return true;
    }

    internal void AdjustUnitsDifVifandLimits()
    {
      if (this.MyMeter.MyFunctions.baseTypeEditMode)
        return;
      this.AdjustUnitsAndDifVif((S3_MemoryBlock) this.P2P);
      this.AdjustUnitsAndDifVif((S3_MemoryBlock) this.Radio);
      this.AdjustUnitsAndDifVif((S3_MemoryBlock) this.Heat);
      this.AdjustUnitsAndDifVif((S3_MemoryBlock) this.Input1);
      this.AdjustUnitsAndDifVif((S3_MemoryBlock) this.Input2);
      this.AdjustUnitsAndDifVif((S3_MemoryBlock) this.Input3);
      if (this.Radio == null)
        return;
      this.Radio.GuaranteeLoggerParameterLimit();
    }

    internal void AdjustUnitsAndDifVif(S3_MemoryBlock s3_MemoryBlock)
    {
      if (s3_MemoryBlock == null)
        return;
      if (s3_MemoryBlock is MBusParameter)
        ((MBusParameter) s3_MemoryBlock).AdjustDifVif();
      if (s3_MemoryBlock.childMemoryBlocks == null)
        return;
      foreach (S3_MemoryBlock childMemoryBlock in s3_MemoryBlock.childMemoryBlocks)
        this.AdjustUnitsAndDifVif(childMemoryBlock);
    }

    internal bool LinkPointer()
    {
      if (this.childMemoryBlocks == null)
        return false;
      foreach (KeyValuePair<int, string> keyValuePair in this.pointer)
      {
        if (!this.MyMeter.MyLinker.MyLabelAddresses.ContainsKey(keyValuePair.Value))
          throw new Exception("INTERNAL ERROR: Unknown label detected! Name: " + keyValuePair.Value);
        if (!this.MyMeter.MyDeviceMemory.SetUshortValue(keyValuePair.Key, (ushort) this.MyMeter.MyLinker.MyLabelAddresses[keyValuePair.Value]))
          return false;
      }
      S3_Parameter s3Parameter1 = this.MyMeter.MyParameters.ParameterByName["Con_TransmitTablePtr"];
      ushort ushortValue1 = s3Parameter1.GetUshortValue();
      int NewValue1 = this.MyMeter.MyDeviceMemory.BlockMBusTable.BlockStartAddress;
      if (this.MyMeter.MyDeviceMemory.BlockMBusTable.ByteSize == 0)
        NewValue1 = 0;
      if ((int) ushortValue1 != NewValue1)
      {
        DataTransmitter.logger.Debug<string, string>("Change address of Con_TransmitTablePtr => Pointer to the transmit parameter table. New value: 0x{0}, Old value: 0x{1}", NewValue1.ToString("X4"), ushortValue1.ToString("X4"));
        if (!s3Parameter1.SetUshortValue((ushort) NewValue1))
          return false;
      }
      S3_Parameter s3Parameter2 = this.MyMeter.MyParameters.ParameterByName["SerDev0_SelectedList"];
      ushort ushortValue2 = s3Parameter2.GetUshortValue();
      int addressOfSelectedList = (int) this.Heat.GetAddressOfSelectedList();
      if ((int) ushortValue2 != addressOfSelectedList)
      {
        DataTransmitter.logger.Debug<string, string>("Change address of SerDev0_SelectedList => Pointer to selected list of heat meter. New value: 0x{0}, Old value: 0x{1}", addressOfSelectedList.ToString("X4"), ushortValue2.ToString("X4"));
        if (!s3Parameter2.SetUshortValue((ushort) addressOfSelectedList))
          return false;
      }
      if (this.MyMeter.MyTransmitParameterManager.Transmitter.Input1 != null && this.MyMeter.MyIdentification.IsInput1Available)
      {
        S3_Parameter s3Parameter3 = this.MyMeter.MyParameters.ParameterByName["SerDev1_SelectedList"];
        ushort ushortValue3 = s3Parameter3.GetUshortValue();
        int NewValue2 = 0;
        if (this.MyMeter.MyTransmitParameterManager.Transmitter.Input1 != null)
          NewValue2 = (int) this.MyMeter.MyTransmitParameterManager.Transmitter.Input1.GetAddressOfSelectedList();
        if ((int) ushortValue3 != NewValue2)
        {
          DataTransmitter.logger.Debug<string, string>("Change address of SerDev1_SelectedList => Pointer to selected list of 1 virtual meter. New value: 0x{0}, Old value: 0x{1}", NewValue2.ToString("X4"), ushortValue3.ToString("X4"));
          if (!s3Parameter3.SetUshortValue((ushort) NewValue2))
            return false;
        }
      }
      if (this.MyMeter.MyTransmitParameterManager.Transmitter.Input2 != null && this.MyMeter.MyIdentification.IsInput2Available)
      {
        S3_Parameter s3Parameter4 = this.MyMeter.MyParameters.ParameterByName["SerDev2_SelectedList"];
        ushort ushortValue4 = s3Parameter4.GetUshortValue();
        int NewValue3 = 0;
        if (this.MyMeter.MyTransmitParameterManager.Transmitter.Input2 != null)
          NewValue3 = (int) this.MyMeter.MyTransmitParameterManager.Transmitter.Input2.GetAddressOfSelectedList();
        if ((int) ushortValue4 != NewValue3)
        {
          DataTransmitter.logger.Debug<string, string>("Change address of SerDev2_SelectedList => Pointer to selected list of 2 virtual meter. New value: 0x{0}, Old value: 0x{1}", NewValue3.ToString("X4"), ushortValue4.ToString("X4"));
          if (!s3Parameter4.SetUshortValue((ushort) NewValue3))
            return false;
        }
      }
      if (this.MyMeter.MyTransmitParameterManager.Transmitter.Input3 != null && this.MyMeter.MyIdentification.IsInput3Available)
      {
        S3_Parameter s3Parameter5 = this.MyMeter.MyParameters.ParameterByName["SerDev3_SelectedList"];
        ushort ushortValue5 = s3Parameter5.GetUshortValue();
        int NewValue4 = 0;
        if (this.MyMeter.MyTransmitParameterManager.Transmitter.Input3 != null)
          NewValue4 = (int) this.MyMeter.MyTransmitParameterManager.Transmitter.Input3.GetAddressOfSelectedList();
        if ((int) ushortValue5 != NewValue4)
        {
          DataTransmitter.logger.Debug<string, string>("Change address of SerDev3_SelectedList => Pointer to selected list of 3 virtual meter. New value: 0x{0}, Old value: 0x{1}", NewValue4.ToString("X4"), ushortValue5.ToString("X4"));
          if (!s3Parameter5.SetUshortValue((ushort) NewValue4))
            return false;
        }
      }
      if (this.MyMeter.MyTransmitParameterManager.Transmitter.Radio != null)
        this.MyMeter.MyTransmitParameterManager.Transmitter.Radio.LinkPointers();
      return true;
    }

    internal DataTransmitter Clone(S3_Meter theCloneMeter, S3_MemoryBlock parentMemoryBlock)
    {
      DataTransmitter parentMemoryBlock1 = new DataTransmitter(theCloneMeter, parentMemoryBlock, (S3_MemoryBlock) this);
      if (this.childMemoryBlocks != null && this.childMemoryBlocks.Count > 0)
      {
        this.P2P.Clone(theCloneMeter, (S3_MemoryBlock) parentMemoryBlock1);
        this.Heat.Clone(theCloneMeter, (S3_MemoryBlock) parentMemoryBlock1);
        if (this.Input1 != null)
          this.Input1.Clone(theCloneMeter, (S3_MemoryBlock) parentMemoryBlock1);
        if (this.Input2 != null)
          this.Input2.Clone(theCloneMeter, (S3_MemoryBlock) parentMemoryBlock1);
        if (this.Input3 != null)
          this.Input3.Clone(theCloneMeter, (S3_MemoryBlock) parentMemoryBlock1);
        if (this.Radio != null)
          this.Radio.Clone(theCloneMeter, (S3_MemoryBlock) parentMemoryBlock1);
      }
      return parentMemoryBlock1;
    }

    private MBusList FindListByAddresses(S3_MemoryBlock root, ushort address)
    {
      if (address <= (ushort) 0 || root == null)
        return (MBusList) null;
      if (root is MBusList && root.BlockStartAddress == (int) address)
        return root as MBusList;
      if (root.childMemoryBlocks == null)
        return (MBusList) null;
      foreach (S3_MemoryBlock childMemoryBlock in root.childMemoryBlocks)
      {
        if (childMemoryBlock is MBusList && childMemoryBlock.BlockStartAddress == (int) address)
          return childMemoryBlock as MBusList;
        MBusList listByAddresses = this.FindListByAddresses(childMemoryBlock, address);
        if (listByAddresses != null)
          return listByAddresses;
      }
      return (MBusList) null;
    }

    internal MBusList FindListByName(S3_MemoryBlock root, string name)
    {
      if (string.IsNullOrEmpty(name) || root == null)
        return (MBusList) null;
      if (root is MBusList && ((MBusList) root).Name == name)
        return root as MBusList;
      if (root.childMemoryBlocks == null)
        return (MBusList) null;
      foreach (S3_MemoryBlock childMemoryBlock in root.childMemoryBlocks)
      {
        if (childMemoryBlock is MBusList && ((MBusList) childMemoryBlock).Name == name)
          return childMemoryBlock as MBusList;
        MBusList listByName = this.FindListByName(childMemoryBlock, name);
        if (listByName != null)
          return listByName;
      }
      return (MBusList) null;
    }

    internal bool Remove(S3_MemoryBlock block)
    {
      if (this.P2P == block || this.Heat == block || this.Input1 == block || this.Input2 == block || this.Input3 == block || this.Radio == block || this.P2P.childMemoryBlocks.Exists((Predicate<S3_MemoryBlock>) (e => e == block)) && this.P2P.childMemoryBlocks.Count == 2)
        return false;
      if (block is MBusList)
      {
        MBusList listOfLink = block as MBusList;
        listOfLink.parentMemoryBlock.ByteSize -= listOfLink.ByteSize;
        if (listOfLink.childMemoryBlocks != null)
        {
          for (int index = listOfLink.childMemoryBlocks.Count - 1; index >= 0; --index)
            this.Remove(listOfLink.childMemoryBlocks[index]);
        }
        listOfLink.parentMemoryBlock.ByteSize -= 2;
        listOfLink.parentMemoryBlock.firstChildMemoryBlockOffset -= 2;
        if (!listOfLink.RemoveFromParentMemoryBlock())
          return false;
        this.RemoveListLink(listOfLink);
      }
      else if (block is RadioListHeader)
      {
        RadioListHeader radioListHeader = block as RadioListHeader;
        RadioTransmitter parentMemoryBlock = radioListHeader.parentMemoryBlock as RadioTransmitter;
        int index = parentMemoryBlock.childMemoryBlocks.IndexOf(block) + 1;
        foreach (S3_MemoryBlock childMemoryBlock1 in radioListHeader.childMemoryBlocks)
        {
          if (!(childMemoryBlock1 is S3_DataBlock))
          {
            S3_MemoryBlock childMemoryBlock2 = parentMemoryBlock.childMemoryBlocks[index];
            parentMemoryBlock.ByteSize -= childMemoryBlock2.ByteSize;
            childMemoryBlock2.RemoveFromParentMemoryBlock();
          }
        }
        --parentMemoryBlock.Count;
        parentMemoryBlock.ByteSize -= 2;
        parentMemoryBlock.firstChildMemoryBlockOffset -= 2;
        parentMemoryBlock.ByteSize -= radioListHeader.ByteSize;
        radioListHeader.RemoveFromParentMemoryBlock();
        if (radioListHeader.IsSelected && parentMemoryBlock.childMemoryBlocks.Count > 0 && parentMemoryBlock.childMemoryBlocks[0] is RadioListHeader)
          ((RadioListHeader) parentMemoryBlock.childMemoryBlocks[0]).IsSelected_Memory = true;
      }
      else if (block is RadioListHeaderItem)
      {
        RadioListHeaderItem radioListHeaderItem = block as RadioListHeaderItem;
        RadioListHeader parentMemoryBlock1 = block.parentMemoryBlock as RadioListHeader;
        RadioTransmitter parentMemoryBlock2 = parentMemoryBlock1.parentMemoryBlock as RadioTransmitter;
        int num = parentMemoryBlock1.childMemoryBlocks.IndexOf(block);
        int index = parentMemoryBlock2.childMemoryBlocks.IndexOf((S3_MemoryBlock) parentMemoryBlock1) + 1 + num;
        S3_MemoryBlock childMemoryBlock = parentMemoryBlock2.childMemoryBlocks[index];
        parentMemoryBlock2.ByteSize -= childMemoryBlock.ByteSize;
        childMemoryBlock.RemoveFromParentMemoryBlock();
        radioListHeaderItem.RemoveFromParentMemoryBlock();
      }
      else
        block.RemoveFromParentMemoryBlock();
      return true;
    }

    public void RemoveEmptyList()
    {
      this.RemoveEmptyList(this.Heat);
      if (this.Input1 != null)
        this.RemoveEmptyList(this.Input1);
      if (this.Input2 != null)
        this.RemoveEmptyList(this.Input2);
      if (this.Input3 != null)
        this.RemoveEmptyList(this.Input3);
      this.RemoveEmptyList((S3_MemoryBlock) this.Radio);
    }

    private void RemoveEmptyList(MBusTransmitter t)
    {
      if (t.childMemoryBlocks == null)
        return;
      for (int index1 = t.childMemoryBlocks.Count - 1; index1 >= 0; --index1)
      {
        MBusList childMemoryBlock1 = t.childMemoryBlocks[index1] as MBusList;
        if (childMemoryBlock1.childMemoryBlocks == null || childMemoryBlock1.childMemoryBlocks.Count < 2 || childMemoryBlock1.IsEmpty)
        {
          this.Remove((S3_MemoryBlock) childMemoryBlock1);
        }
        else
        {
          for (int index2 = childMemoryBlock1.childMemoryBlocks.Count - 1; index2 >= 0; --index2)
          {
            S3_MemoryBlock childMemoryBlock2 = childMemoryBlock1.childMemoryBlocks[index2];
            if (childMemoryBlock2 is MBusParameterGroup && (childMemoryBlock2.childMemoryBlocks == null || childMemoryBlock2.childMemoryBlocks.Count < 2))
              this.Remove(childMemoryBlock2);
          }
        }
      }
    }

    private void RemoveEmptyList(S3_MemoryBlock memoryItem)
    {
      if (memoryItem == null || memoryItem.childMemoryBlocks == null)
        return;
      switch (memoryItem)
      {
        case RadioTransmitter _:
          for (int index = memoryItem.childMemoryBlocks.Count - 1; index >= 0; --index)
          {
            if (memoryItem.childMemoryBlocks[index] is RadioListHeader)
            {
              S3_MemoryBlock childMemoryBlock = memoryItem.childMemoryBlocks[index];
              this.RemoveEmptyList(childMemoryBlock);
              if (childMemoryBlock.childMemoryBlocks[0] is S3_DataBlock)
                this.Remove(childMemoryBlock);
            }
          }
          break;
        case RadioListHeader _:
          if (memoryItem == null || memoryItem.childMemoryBlocks == null)
            break;
          RadioListHeader radioListHeader = memoryItem as RadioListHeader;
          int num = radioListHeader.parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) radioListHeader);
          for (int index = radioListHeader.childMemoryBlocks.Count - 1; index >= 0; --index)
          {
            S3_MemoryBlock childMemoryBlock1 = radioListHeader.childMemoryBlocks[index];
            if (childMemoryBlock1 is RadioListHeaderItem)
            {
              S3_MemoryBlock childMemoryBlock2 = radioListHeader.parentMemoryBlock.childMemoryBlocks[num + 1 + index];
              if (radioListHeader.Mode == RADIO_MODE.Radio3_Sz0 || radioListHeader.Mode == RADIO_MODE.Radio3 || radioListHeader.Mode == RADIO_MODE.Radio3_Sz5)
              {
                if (!radioListHeader.Name.Contains("empty") && childMemoryBlock2.childMemoryBlocks.Count < 4)
                  this.Remove(childMemoryBlock1);
              }
              else if (childMemoryBlock2.childMemoryBlocks[0] is S3_DataBlock)
                this.Remove(childMemoryBlock1);
            }
          }
          break;
      }
    }

    private void RemoveListLink(MBusList listOfLink)
    {
      this.RemoveListLink(this.P2P, listOfLink);
      if (this.Heat.childMemoryBlocks != null)
      {
        foreach (MBusList childMemoryBlock in this.Heat.childMemoryBlocks)
          this.RemoveListLink(childMemoryBlock, listOfLink);
      }
      if (this.Input1.childMemoryBlocks != null)
      {
        foreach (MBusList childMemoryBlock in this.Input1.childMemoryBlocks)
          this.RemoveListLink(childMemoryBlock, listOfLink);
      }
      if (this.Input2.childMemoryBlocks != null)
      {
        foreach (MBusList childMemoryBlock in this.Input2.childMemoryBlocks)
          this.RemoveListLink(childMemoryBlock, listOfLink);
      }
      if (this.Input3 == null || this.Input3.childMemoryBlocks == null)
        return;
      foreach (MBusList childMemoryBlock in this.Input3.childMemoryBlocks)
        this.RemoveListLink(childMemoryBlock, listOfLink);
    }

    private void RemoveListLink(MBusList list, MBusList listOfLink)
    {
      if (list.childMemoryBlocks == null)
        return;
      for (int index = list.childMemoryBlocks.Count - 1; index >= 0; --index)
      {
        if (list.childMemoryBlocks[index] is ListLink && ((ListLink) list.childMemoryBlocks[index]).Name == listOfLink.Name)
          this.Remove(list.childMemoryBlocks[index]);
      }
    }

    internal void PrintMBusLists(StringBuilder printText)
    {
      string startString = "   ";
      this.P2P.PrintMBusList(printText, startString + "Point to point (Adress: 254)", startString);
      this.Heat.PrintTransmitterMBusList(printText, "Heatmeter", startString + "H:  ");
      if (this.Input1 != null)
        this.Input1.PrintTransmitterMBusList(printText, "Input1", startString + "I1: ");
      if (this.Input2 != null)
        this.Input2.PrintTransmitterMBusList(printText, "Input2", startString + "I2: ");
      if (this.Input3 == null)
        return;
      this.Input3.PrintTransmitterMBusList(printText, "Input3", startString + "I3: ");
    }

    internal void PrintRadioLists(StringBuilder printText)
    {
      if (this.Radio == null)
        return;
      string startString = "   ";
      this.Radio.PrintTransmitterRadioList(printText, "Radio", startString);
    }

    internal void PrintRadioListsShortInfo(StringBuilder printText)
    {
      if (this.Radio == null)
        return;
      string startString = "   ";
      this.Radio.PrintTransmitterRadioListShortInfo(printText, "Radio", startString);
    }

    internal void PrepareMBusParameterGroup()
    {
      this.PrepareMBusParameterGroup((S3_MemoryBlock) this.Heat);
      this.PrepareMBusParameterGroup((S3_MemoryBlock) this.Input1);
      this.PrepareMBusParameterGroup((S3_MemoryBlock) this.Input2);
      this.PrepareMBusParameterGroup((S3_MemoryBlock) this.Input3);
      this.PrepareMBusParameterGroup((S3_MemoryBlock) this.Radio);
    }

    internal void PrepareMBusParameterGroup(S3_MemoryBlock t)
    {
      if (t == null || t.childMemoryBlocks == null || t.childMemoryBlocks.Count == 0)
        return;
      foreach (S3_MemoryBlock childMemoryBlock1 in t.childMemoryBlocks)
      {
        if ((childMemoryBlock1 is MBusList || childMemoryBlock1 is RadioList) && childMemoryBlock1.childMemoryBlocks != null && childMemoryBlock1.childMemoryBlocks.Count != 0)
        {
          foreach (S3_MemoryBlock childMemoryBlock2 in childMemoryBlock1.childMemoryBlocks)
          {
            if (childMemoryBlock2 is MBusParameterGroup)
              (childMemoryBlock2 as MBusParameterGroup).Prepare();
          }
        }
      }
    }
  }
}
