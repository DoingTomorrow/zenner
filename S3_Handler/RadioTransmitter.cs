// Decompiled with JetBrains decompiler
// Type: S3_Handler.RadioTransmitter
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
  internal sealed class RadioTransmitter : S3_MemoryBlock
  {
    private static List<string> oldRadioListNames = new List<string>()
    {
      "Radio2_NetKeyMonth*=Sz0",
      "Radio3_NetKeyMonth*Day*=Sz1",
      "Radio3_WalkKeyMonth=Sz3",
      "wMBusT1_WalkCurKeyMonth1Diag",
      "wMBusT1_WalkCurKeyMonth1"
    };
    private static List<string> newRadioListNames = new List<string>()
    {
      "Radio3_Sz0",
      "Radio3_Sz1",
      "Radio3_Sz3",
      "wMBusT1_B",
      "wMBusT1_C"
    };

    internal string Name => "Radio";

    internal ushort Count { get; set; }

    internal ushort Count_Memory
    {
      get => this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress);
      set
      {
        this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress, value);
        this.Count = value;
      }
    }

    public RadioTransmitter(S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock)
      : base(MyMeter, S3_MemorySegment.RadioTransmitter, parentMemoryBlock)
    {
    }

    public RadioTransmitter(
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock,
      S3_MemoryBlock sourceMemoryBlock)
      : base(MyMeter, parentMemoryBlock, sourceMemoryBlock)
    {
    }

    public RadioTransmitter(S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock, int insertIndex)
      : base(MyMeter, S3_MemorySegment.RadioTransmitter, parentMemoryBlock, insertIndex)
    {
    }

    internal bool CreateFromMemory()
    {
      this.Count = this.Count_Memory;
      if (this.Count == (ushort) 0 || this.Count == ushort.MaxValue)
      {
        this.ByteSize = 2;
        this.firstChildMemoryBlockOffset = 2;
        this.Count_Memory = (ushort) 0;
        return true;
      }
      this.StartAddressOfNextBlock = this.BlockStartAddress + 2 + (int) this.Count * 2;
      this.firstChildMemoryBlockOffset = this.ByteSize;
      for (int pos = 0; pos < (int) this.Count; ++pos)
      {
        RadioListHeader radioListHeader = new RadioListHeader(this.MyMeter, (S3_MemoryBlock) this);
        radioListHeader.CreateFromMemory(pos);
        foreach (S3_MemoryBlock childMemoryBlock in radioListHeader.childMemoryBlocks)
        {
          if (!(childMemoryBlock is S3_DataBlock))
          {
            if ((int) (childMemoryBlock as RadioListHeaderItem).AddressOfList_Memory != this.StartAddressOfNextBlock)
              throw new Exception("INTERNAL ERROR: Invalid address of RadioListItem! Value: " + this.StartAddressOfNextBlock.ToString());
            new RadioList(this.MyMeter, (S3_MemoryBlock) this).CreateFromMemory();
          }
          else
            break;
        }
      }
      return true;
    }

    internal RadioTransmitter Clone(S3_Meter theCloneMeter, S3_MemoryBlock parentMemoryBlock)
    {
      RadioTransmitter radioTransmitter = new RadioTransmitter(theCloneMeter, parentMemoryBlock, (S3_MemoryBlock) this);
      radioTransmitter.Count = this.Count;
      if (this.childMemoryBlocks != null)
      {
        foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
        {
          if (childMemoryBlock is RadioListHeader)
            ((RadioListHeader) childMemoryBlock).Clone(theCloneMeter, (S3_MemoryBlock) radioTransmitter);
          if (childMemoryBlock is RadioList)
            ((RadioList) childMemoryBlock).Clone(theCloneMeter, (S3_MemoryBlock) radioTransmitter);
          if (childMemoryBlock is S3_DataBlock)
            ((S3_DataBlock) childMemoryBlock).Clone(theCloneMeter, (S3_MemoryBlock) radioTransmitter);
        }
      }
      if (this.sourceMemoryBlock != null)
        radioTransmitter.sourceMemoryBlock = this.sourceMemoryBlock;
      return radioTransmitter;
    }

    internal void LinkPointers()
    {
      if (this.childMemoryBlocks == null)
        return;
      foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
      {
        if (childMemoryBlock is RadioListHeader)
          ((RadioListHeader) childMemoryBlock).LinkPointers();
      }
    }

    internal bool InsertData(ref Dictionary<int, string> pointer)
    {
      this.MyMeter.MyLinker.AddLabel(this.Name, this.BlockStartAddress);
      this.Count_Memory = this.Count;
      if (this.childMemoryBlocks != null)
      {
        int num = this.BlockStartAddress + 2;
        foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
        {
          if (childMemoryBlock is RadioListHeader)
          {
            RadioListHeader radioListHeader = childMemoryBlock as RadioListHeader;
            this.MyMeter.MyDeviceMemory.SetUshortValue(num, (ushort) 0);
            pointer.Add(num, radioListHeader.Name);
            num += 2;
            if (!radioListHeader.InsertData(ref pointer))
              return false;
          }
          else if (childMemoryBlock is RadioList && !(childMemoryBlock as RadioList).InsertData(ref pointer))
            return false;
        }
      }
      else
        this.MyMeter.MyParameters.ParameterByName["RadioTransmitList"].SetUshortValue((ushort) this.BlockStartAddress);
      return true;
    }

    internal RadioListHeader InsertHeader(int position)
    {
      RadioListHeader parentMemoryBlock = new RadioListHeader(this.MyMeter, (S3_MemoryBlock) this, position);
      S3_DataBlock s3DataBlock = new S3_DataBlock(new byte[2], this.MyMeter, (S3_MemoryBlock) parentMemoryBlock);
      parentMemoryBlock.Mode = RADIO_MODE.wMBusT1;
      parentMemoryBlock.firstChildMemoryBlockOffset += 2;
      parentMemoryBlock.parentMemoryBlock.firstChildMemoryBlockOffset += 2;
      ++this.Count;
      return parentMemoryBlock;
    }

    internal static bool isOldListName(string Name)
    {
      return RadioTransmitter.oldRadioListNames.IndexOf(Name) != -1;
    }

    internal static string TranslateToNewListName(string listName)
    {
      int index = RadioTransmitter.oldRadioListNames.IndexOf(listName);
      return index == -1 ? listName : RadioTransmitter.newRadioListNames[index];
    }

    private string TranslateRadioName(string Name)
    {
      int index1 = RadioTransmitter.oldRadioListNames.IndexOf(Name);
      if (index1 != -1)
        return RadioTransmitter.newRadioListNames[index1];
      int index2 = RadioTransmitter.newRadioListNames.IndexOf(Name);
      return index2 == -1 ? string.Empty : RadioTransmitter.oldRadioListNames[index2];
    }

    internal List<string> AvailableListNames()
    {
      if (this == null || this.childMemoryBlocks == null || this.childMemoryBlocks.Count == 0)
        return (List<string>) null;
      List<string> stringList = new List<string>();
      foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
      {
        if (childMemoryBlock is RadioListHeader && ((RadioListHeader) childMemoryBlock).ScenarioName.IndexOf("empty") == -1)
        {
          string str = !RadioTransmitter.isOldListName(((RadioListHeader) childMemoryBlock).Name) ? ((RadioListHeader) childMemoryBlock).Name : this.TranslateRadioName(((RadioListHeader) childMemoryBlock).Name);
          stringList.Add(str);
        }
      }
      if (this.MyMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.radioAndEncMode.ToString()))
      {
        int count = stringList.Count;
        for (int index = 0; index < count; ++index)
        {
          if (stringList[index].StartsWith("wMBusT1"))
            stringList.Add(stringList[index].Replace("wMBusT1", "wMBusC1"));
        }
        stringList.Sort();
      }
      return stringList;
    }

    internal List<string> AvailableListAndEncryptionNames()
    {
      List<string> stringList1 = this.AvailableListNames();
      if (!this.MyMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.radioAndEncMode.ToString()))
        return stringList1;
      List<string> stringList2 = new List<string>();
      foreach (string str in stringList1)
      {
        if (str.Contains("wMBusT1"))
        {
          stringList2.Add(str + "_5");
          stringList2.Add(str + "_7");
        }
        else
          stringList2.Add(str);
      }
      return stringList2;
    }

    internal string Get_ActivatedList()
    {
      if (this == null || this.childMemoryBlocks == null || this.childMemoryBlocks.Count == 0)
        return (string) null;
      string activatedList = (string) null;
      foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
      {
        if (childMemoryBlock is RadioListHeader && ((RadioListHeader) childMemoryBlock).IsSelected)
        {
          if (RadioTransmitter.isOldListName(((RadioListHeader) childMemoryBlock).Name))
          {
            activatedList = this.TranslateRadioName(((RadioListHeader) childMemoryBlock).Name);
            break;
          }
          activatedList = ((RadioListHeader) childMemoryBlock).Name;
          if (this.MyMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.radioAndEncMode.ToString()) && (ushort) ((uint) this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.radioAndEncMode.ToString()].GetByteValue() >> 4) == (ushort) 8 && activatedList.StartsWith(RADIO_MODE.wMBusT1.ToString()))
            activatedList = activatedList.Replace(RADIO_MODE.wMBusT1.ToString(), RADIO_MODE.wMBusC1.ToString());
          break;
        }
      }
      return activatedList;
    }

    internal string Get_ActivatedListAndEncryption()
    {
      string activatedList = this.Get_ActivatedList();
      if (activatedList == null)
        return (string) null;
      if (this.MyMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.radioAndEncMode.ToString()) && activatedList.Contains("wMBus"))
      {
        switch ((AES_ENCRYPTION_MODE) ((int) this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.radioAndEncMode.ToString()].GetByteValue() & 15))
        {
          case AES_ENCRYPTION_MODE.MODE_5:
            activatedList += "_5";
            break;
          case AES_ENCRYPTION_MODE.MODE_7:
            activatedList += "_7";
            break;
        }
      }
      return activatedList;
    }

    internal bool Set_ActivatedList(string activeListName)
    {
      if (this == null || this.childMemoryBlocks == null || this.childMemoryBlocks.Count == 0)
        return false;
      bool flag = false;
      SortedList<string, S3_Parameter> parameterByName1 = this.MyMeter.MyParameters.ParameterByName;
      S3_ParameterNames s3ParameterNames = S3_ParameterNames.radioAndEncMode;
      string key1 = s3ParameterNames.ToString();
      if (parameterByName1.ContainsKey(key1))
      {
        SortedList<string, S3_Parameter> parameterByName2 = this.MyMeter.MyParameters.ParameterByName;
        s3ParameterNames = S3_ParameterNames.radioAndEncMode;
        string key2 = s3ParameterNames.ToString();
        S3_Parameter s3Parameter = parameterByName2[key2];
        byte byteValue = s3Parameter.GetByteValue();
        RadioSetup radioSetup = new RadioSetup(activeListName, byteValue);
        s3Parameter.SetByteValue(radioSetup.GetRadioAndEncryptionMode());
        string listName = radioSetup.GetListName();
        foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
        {
          if (childMemoryBlock is RadioListHeader)
          {
            if (((RadioListHeader) childMemoryBlock).Name == listName)
            {
              ((RadioListHeader) childMemoryBlock).IsSelected = true;
              flag = true;
            }
            else
              ((RadioListHeader) childMemoryBlock).IsSelected = false;
          }
        }
        if (flag)
          return true;
        foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
        {
          if (childMemoryBlock is RadioListHeader)
          {
            if (((RadioListHeader) childMemoryBlock).ScenarioName == radioSetup.Szenario)
            {
              ((RadioListHeader) childMemoryBlock).IsSelected = true;
              flag = true;
            }
            else
              ((RadioListHeader) childMemoryBlock).IsSelected = false;
          }
        }
        return flag;
      }
      foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
      {
        if (childMemoryBlock is RadioListHeader)
        {
          if (((RadioListHeader) childMemoryBlock).Name == activeListName)
          {
            ((RadioListHeader) childMemoryBlock).IsSelected = true;
            flag = true;
          }
          else
            ((RadioListHeader) childMemoryBlock).IsSelected = false;
        }
      }
      if (flag)
        return true;
      activeListName = this.TranslateRadioName(activeListName);
      foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
      {
        if (childMemoryBlock is RadioListHeader)
        {
          if (((RadioListHeader) childMemoryBlock).Name == activeListName)
          {
            ((RadioListHeader) childMemoryBlock).IsSelected = true;
            flag = true;
          }
          else
            ((RadioListHeader) childMemoryBlock).IsSelected = false;
        }
      }
      return flag;
    }

    internal RADIO_MODE? Get_ActivatedRadioMode()
    {
      if (this == null)
        return new RADIO_MODE?();
      if (this.childMemoryBlocks == null || this.childMemoryBlocks.Count == 0)
        return new RADIO_MODE?();
      foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
      {
        if (childMemoryBlock is RadioListHeader && ((RadioListHeader) childMemoryBlock).IsSelected)
          return new RADIO_MODE?(((RadioListHeader) childMemoryBlock).Mode);
      }
      return new RADIO_MODE?();
    }

    internal bool Set_ActivatedRadioListMode(RADIO_MODE mode)
    {
      if (this == null || this.childMemoryBlocks == null || this.childMemoryBlocks.Count == 0)
        return false;
      foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
      {
        if (childMemoryBlock is RadioListHeader && ((RadioListHeader) childMemoryBlock).IsSelected)
        {
          ((RadioListHeader) childMemoryBlock).Mode = mode;
          return true;
        }
      }
      return true;
    }

    internal void PrintTransmitterRadioListShortInfo(
      StringBuilder printText,
      string transmitterName,
      string startString)
    {
      if (this.childMemoryBlocks == null)
        return;
      printText.AppendLine();
      printText.AppendLine("--- " + transmitterName + " ---");
      foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
      {
        if (childMemoryBlock is RadioListHeader)
        {
          string name = ((RadioListHeader) childMemoryBlock).Name;
          if (((RadioListHeader) childMemoryBlock).IsSelected)
            name += " => activated";
          printText.AppendLine(startString + name);
        }
      }
    }

    internal void PrintTransmitterRadioList(
      StringBuilder printText,
      string transmitterName,
      string startString)
    {
      if (this.childMemoryBlocks == null)
        return;
      printText.AppendLine("Transmitter: " + transmitterName);
      foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
      {
        if (childMemoryBlock is RadioListHeader)
        {
          string name = ((RadioListHeader) childMemoryBlock).Name;
          if (((RadioListHeader) childMemoryBlock).IsSelected)
            name += " => activated";
          printText.AppendLine(startString + name);
          ((RadioListHeader) childMemoryBlock).PrintRadioList(printText, startString);
        }
      }
    }

    internal void GuaranteeLoggerParameterLimit()
    {
      this.GuaranteeLoggerParameterLimit((S3_MemoryBlock) this);
    }

    private void GuaranteeLoggerParameterLimit(S3_MemoryBlock block)
    {
      if (block is RadioList)
      {
        RadioTransmitter.RadioListInfo listInfo = new RadioTransmitter.RadioListInfo();
        this.GetRadioListInfo(block, listInfo);
        if (listInfo.Group == null)
          return;
        int num1 = listInfo.ParameterOutsideGroup + listInfo.ParameterInsideGroupStart + listInfo.ParameterInsideGroup * listInfo.Group.LoggerCycleCount;
        int num2 = listInfo.TransmitBytesOutsideGroup + listInfo.TransmitBytesInsideGroupStart + listInfo.TransmitBytesInsideGroup * listInfo.Group.LoggerCycleCount;
        int num3 = (206 - listInfo.TransmitBytesOutsideGroup - listInfo.TransmitBytesInsideGroupStart) / listInfo.TransmitBytesInsideGroup;
        int num4 = listInfo.TransmitBytesOutsideGroup + listInfo.TransmitBytesInsideGroupStart + listInfo.TransmitBytesInsideGroup * num3;
        if (listInfo.Group.LoggerCycleCount > num3 && listInfo.Group.childMemoryBlocks != null && listInfo.Group.childMemoryBlocks.Count > 0)
        {
          listInfo.Group.LoggerCycleCount = num3;
          ((MBusParameter) listInfo.Group.childMemoryBlocks[listInfo.Group.childMemoryBlocks.Count - 1]).ControlWord1.LoggerCycleCount = num3;
        }
      }
      else
      {
        if (block.childMemoryBlocks == null)
          return;
        foreach (S3_MemoryBlock childMemoryBlock in block.childMemoryBlocks)
          this.GuaranteeLoggerParameterLimit(childMemoryBlock);
      }
    }

    private void GetRadioListInfo(S3_MemoryBlock block, RadioTransmitter.RadioListInfo listInfo)
    {
      if (block is MBusParameter)
      {
        ++listInfo.ParameterOutsideGroup;
        MBusParameter mbusParameter = (MBusParameter) block;
        if (mbusParameter.VifDif != null)
          listInfo.TransmitBytesOutsideGroup += mbusParameter.VifDif.Count;
        listInfo.TransmitBytesOutsideGroup += mbusParameter.ControlWord0.DataCount;
      }
      else if (block is MBusParameterGroup)
      {
        listInfo.Group = listInfo.Group == null ? (MBusParameterGroup) block : throw new Exception("More than 1 group inside RadioList not supported.");
        this.GetRadioGroupInfo(block, listInfo);
      }
      else if (block.childMemoryBlocks != null)
      {
        foreach (S3_MemoryBlock childMemoryBlock in block.childMemoryBlocks)
          this.GetRadioListInfo(childMemoryBlock, listInfo);
      }
    }

    private void GetRadioGroupInfo(S3_MemoryBlock block, RadioTransmitter.RadioListInfo listInfo)
    {
      if (block is MBusParameter)
      {
        MBusParameter mbusParameter = (MBusParameter) block;
        if (mbusParameter.ControlWord1 == null && listInfo.ParameterInsideGroup == 0)
        {
          ++listInfo.ParameterInsideGroupStart;
          if (mbusParameter.VifDif != null)
            listInfo.TransmitBytesInsideGroupStart += mbusParameter.VifDif.Count;
          listInfo.TransmitBytesInsideGroupStart += mbusParameter.ControlWord0.DataCount;
        }
        else
        {
          ++listInfo.ParameterInsideGroup;
          if (mbusParameter.VifDif != null)
            listInfo.TransmitBytesInsideGroup += mbusParameter.VifDif.Count;
          listInfo.TransmitBytesInsideGroup += mbusParameter.ControlWord0.DataCount;
        }
      }
      if (block.childMemoryBlocks == null)
        return;
      foreach (S3_MemoryBlock childMemoryBlock in block.childMemoryBlocks)
        this.GetRadioGroupInfo(childMemoryBlock, listInfo);
    }

    private class RadioListInfo
    {
      internal MBusParameterGroup Group = (MBusParameterGroup) null;
      internal int ParameterInsideGroupStart = 0;
      internal int TransmitBytesInsideGroupStart = 0;
      internal int ParameterInsideGroup = 0;
      internal int TransmitBytesInsideGroup = 0;
      internal int ParameterOutsideGroup = 0;
      internal int TransmitBytesOutsideGroup = 0;
    }
  }
}
