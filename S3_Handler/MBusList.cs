// Decompiled with JetBrains decompiler
// Type: S3_Handler.MBusList
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using DeviceCollector;
using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal sealed class MBusList : S3_MemoryBlock
  {
    public ushort ControlWord { get; set; }

    public byte VirtualDeviceNumber { get; set; }

    public string Name { get; private set; }

    public bool IsSelected { get; set; }

    public MBusList(S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock, string name)
      : base(MyMeter, S3_MemorySegment.MBusList, parentMemoryBlock)
    {
      this.Name = name;
      this.firstChildMemoryBlockOffset = 2;
    }

    public MBusList(
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock,
      S3_MemoryBlock sourceMemoryBlock,
      string name)
      : base(MyMeter, parentMemoryBlock, sourceMemoryBlock)
    {
      this.Name = name;
      this.firstChildMemoryBlockOffset = 2;
    }

    public MBusList(
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock,
      int insertIndex,
      string name)
      : base(MyMeter, S3_MemorySegment.MBusList, parentMemoryBlock, insertIndex)
    {
      this.Name = name;
      this.firstChildMemoryBlockOffset = 2;
    }

    public bool IsEmpty
    {
      get
      {
        if (this.childMemoryBlocks == null || this.childMemoryBlocks.Count == 0)
          return true;
        foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
        {
          switch (childMemoryBlock)
          {
            case S3_DataBlock _:
              continue;
            case ListLink _:
              return false;
            case MBusParameterGroup _:
              if (childMemoryBlock.childMemoryBlocks != null && childMemoryBlock.childMemoryBlocks.Count > 0)
                return false;
              continue;
            default:
              MBusParameter mbusParameter = childMemoryBlock as MBusParameter;
              if (mbusParameter.IsLogger || mbusParameter.Name != "Con_SerialNumber")
                return false;
              continue;
          }
        }
        return true;
      }
    }

    public int GetOutputBufferSize(bool ignoreMBusParameterGroupSize)
    {
      int outputBufferSize1 = 21;
      if (this.childMemoryBlocks == null || this.childMemoryBlocks.Count == 0)
        return outputBufferSize1;
      int outputBufferSize2 = outputBufferSize1;
      foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
      {
        switch (childMemoryBlock)
        {
          case S3_DataBlock _:
            goto label_12;
          case ListLink _:
            continue;
          case MBusParameterGroup _:
            if (!ignoreMBusParameterGroupSize)
            {
              MBusParameterGroup mbusParameterGroup = childMemoryBlock as MBusParameterGroup;
              outputBufferSize2 += mbusParameterGroup.GetOutputBufferSize();
              break;
            }
            break;
          case MBusParameter _:
            outputBufferSize2 += MBusList.CalculateSizeOfMBusParameter(childMemoryBlock as MBusParameter);
            break;
          default:
            throw new Exception("INTERNAL ERROR: Invalid MemoryBlock detected!!!");
        }
      }
label_12:
      return outputBufferSize2;
    }

    private static int CalculateSizeOfMBusParameter(MBusParameter p)
    {
      return p.IsLogger && p.Iteration != null ? (p.Iteration.ControlWord1.LoggerCycleCount + 1) * (p.ControlWord0.DifVifCount + p.ControlWord0.DataCount) : p.ControlWord0.DifVifCount + p.ControlWord0.DataCount;
    }

    public List<string> LinkNames
    {
      get
      {
        List<string> linkNames = new List<string>();
        if (this.childMemoryBlocks == null || this.childMemoryBlocks.Count == 0)
          return linkNames;
        foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
        {
          if (childMemoryBlock is ListLink)
            linkNames.Add(((ListLink) childMemoryBlock).Name);
        }
        return linkNames;
      }
    }

    internal bool CreateFromMemory()
    {
      this.ControlWord = this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress);
      byte controlWord = (byte) this.ControlWord;
      this.VirtualDeviceNumber = (byte) ((uint) this.ControlWord >> 8);
      this.ByteSize = 2;
      string keyOfSelectedList = MBusList.GetKeyOfSelectedList(this.VirtualDeviceNumber);
      if (string.IsNullOrEmpty(keyOfSelectedList) || !this.MyMeter.MyParameters.ParameterByName.ContainsKey(keyOfSelectedList))
        return false;
      this.IsSelected = (int) this.MyMeter.MyParameters.ParameterByName[keyOfSelectedList].GetUshortValue() == this.BlockStartAddress;
      for (int index = 0; index < (int) controlWord; ++index)
      {
        ListLink listLink = new ListLink(this.MyMeter, (S3_MemoryBlock) this);
      }
      ushort ushortValue = this.MyMeter.MyDeviceMemory.GetUshortValue(this.StartAddressOfNextBlock);
      if (ushortValue > (ushort) 0)
      {
        for (; ushortValue > (ushort) 0; ushortValue = this.MyMeter.MyDeviceMemory.GetUshortValue(this.StartAddressOfNextBlock))
          new MBusParameter(this.MyMeter, (S3_MemoryBlock) this).CreateFromMemory(true);
      }
      S3_DataBlock s3DataBlock = new S3_DataBlock(new byte[2], this.MyMeter, (S3_MemoryBlock) this);
      this.FindAndCreateParameterGroups();
      return true;
    }

    private void FindAndCreateParameterGroups()
    {
      if (this.childMemoryBlocks == null || this.childMemoryBlocks.Count == 1)
        return;
      for (int index1 = 0; index1 < this.childMemoryBlocks.Count; ++index1)
      {
        int? endIndexOfGroup;
        if (this.IsGroupStart(this.childMemoryBlocks[index1], out endIndexOfGroup))
        {
          MBusParameter childMemoryBlock1 = this.childMemoryBlocks[index1] as MBusParameter;
          MBusParameter childMemoryBlock2 = this.childMemoryBlocks[endIndexOfGroup.Value] as MBusParameter;
          MBusParameterGroup mbusParameterGroup = this.AddGroup(index1);
          mbusParameterGroup.IsDueDate = childMemoryBlock1.ControlWord0.ControlLogger == ControlLogger.LoggerDueDateReset;
          MBusDifVif mbusDifVif = new MBusDifVif();
          if (!mbusDifVif.LoadDifVif(childMemoryBlock1.VifDif.ToArray()))
            throw new Exception("Invalid parameter group detected! Check transmit M-Bus list: " + this.Name);
          mbusParameterGroup.StartStorageNumber = mbusDifVif.StorageNumber;
          mbusParameterGroup.LoggerCycleCount = childMemoryBlock2.ControlWord1.LoggerCycleCount;
          mbusParameterGroup.ListMaxCount = childMemoryBlock2.ControlWord2.ListMaxCount;
          int num = endIndexOfGroup.Value - index1 + 1;
          for (int index2 = 0; index2 < num; ++index2)
          {
            S3_MemoryBlock childMemoryBlock3 = this.childMemoryBlocks[index1 + 1];
            childMemoryBlock3.RemoveFromParentMemoryBlock();
            mbusParameterGroup.Add(childMemoryBlock3.SegmentType, childMemoryBlock3);
          }
          mbusParameterGroup.BlockStartAddress = childMemoryBlock1.BlockStartAddress;
          mbusParameterGroup.StartAddressOfNextBlock = childMemoryBlock2.StartAddressOfNextBlock;
          mbusParameterGroup.IsPrepared = true;
        }
      }
    }

    private bool IsGroupStart(S3_MemoryBlock something, out int? endIndexOfGroup)
    {
      endIndexOfGroup = new int?();
      if (something is MBusParameter)
      {
        MBusParameter mbusParameter = something as MBusParameter;
        if (mbusParameter.IsLogger && (mbusParameter.ControlWord0.ControlLogger == ControlLogger.LoggerReset || mbusParameter.ControlWord0.ControlLogger == ControlLogger.LoggerDueDateReset))
        {
          int num1 = this.childMemoryBlocks.IndexOf(something) + 1;
          if (num1 > this.childMemoryBlocks.Count - 1)
            return false;
          for (int index1 = num1; index1 < this.childMemoryBlocks.Count && this.childMemoryBlocks[index1] is MBusParameter && ((MBusParameter) this.childMemoryBlocks[index1]).IsLogger; ++index1)
          {
            MBusParameter childMemoryBlock1 = this.childMemoryBlocks[index1] as MBusParameter;
            if (childMemoryBlock1.ControlWord0.ControlLogger == ControlLogger.LoggerDueDateReset || childMemoryBlock1.ControlWord0.ControlLogger == ControlLogger.LoggerReset)
              return false;
            if (childMemoryBlock1.ControlWord0.ItFollowsNextControlWord && childMemoryBlock1.ControlWord1.ItFollowsNextControlWord && childMemoryBlock1.ControlWord2.IsSavePtr)
            {
              int num2 = index1 + 1;
              if (num2 > this.childMemoryBlocks.Count - 1)
                return false;
              for (int index2 = num2; index2 < this.childMemoryBlocks.Count; ++index2)
              {
                if (!(this.childMemoryBlocks[index2] is MBusParameter) || !((MBusParameter) this.childMemoryBlocks[index2]).IsLogger)
                  return false;
                MBusParameter childMemoryBlock2 = this.childMemoryBlocks[index2] as MBusParameter;
                if (childMemoryBlock2.ControlWord0.ControlLogger == ControlLogger.LoggerDueDateReset || childMemoryBlock2.ControlWord0.ControlLogger == ControlLogger.LoggerReset || childMemoryBlock2.ControlWord0.ItFollowsNextControlWord && childMemoryBlock2.ControlWord1.ItFollowsNextControlWord && childMemoryBlock2.ControlWord2.IsSavePtr)
                  return false;
                if (childMemoryBlock2.ControlWord0.ItFollowsNextControlWord && childMemoryBlock2.ControlWord1.LoggerCycleCount > 0 && childMemoryBlock2.ControlWord1.ItFollowsNextControlWord && childMemoryBlock2.ControlWord2.ListMaxCount >= 0)
                {
                  endIndexOfGroup = new int?(index2);
                  return true;
                }
              }
            }
          }
        }
      }
      return false;
    }

    internal static string GetKeyOfSelectedList(byte virtualDeviceNumber)
    {
      switch (virtualDeviceNumber)
      {
        case 0:
          return "SerDev0_SelectedList";
        case 1:
          return "SerDev1_SelectedList";
        case 2:
          return "SerDev2_SelectedList";
        case 3:
          return "SerDev3_SelectedList";
        default:
          return (string) null;
      }
    }

    internal MBusList Clone(S3_Meter theCloneMeter, S3_MemoryBlock parentMemoryBlock)
    {
      MBusList mbusList = new MBusList(theCloneMeter, parentMemoryBlock, (S3_MemoryBlock) this, this.Name);
      mbusList.ControlWord = this.ControlWord;
      mbusList.VirtualDeviceNumber = this.VirtualDeviceNumber;
      if (this.childMemoryBlocks != null)
      {
        foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
        {
          if (childMemoryBlock is ListLink)
            ((ListLink) childMemoryBlock).Clone(theCloneMeter, (S3_MemoryBlock) mbusList);
          if (childMemoryBlock is MBusParameter)
            ((MBusParameter) childMemoryBlock).Clone(theCloneMeter, (S3_MemoryBlock) mbusList);
          if (childMemoryBlock is MBusParameterGroup)
            ((MBusParameterGroup) childMemoryBlock).Clone(theCloneMeter, (S3_MemoryBlock) mbusList);
          if (childMemoryBlock is S3_DataBlock)
            ((S3_DataBlock) childMemoryBlock).Clone(theCloneMeter, (S3_MemoryBlock) mbusList);
        }
      }
      mbusList.IsSelected = this.IsSelected;
      return mbusList;
    }

    internal bool InsertData(ref Dictionary<int, string> pointer)
    {
      this.MyMeter.MyLinker.AddLabel(this.Name, this.BlockStartAddress);
      List<S3_MemoryBlock> all = this.childMemoryBlocks.FindAll((Predicate<S3_MemoryBlock>) (e => e is ListLink));
      this.ControlWord = all != null ? (ushort) all.Count : (ushort) 0;
      this.ControlWord |= (ushort) ((uint) this.VirtualDeviceNumber << 8);
      this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress, this.ControlWord);
      if (this.childMemoryBlocks != null)
      {
        foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
        {
          switch (childMemoryBlock)
          {
            case ListLink _:
              ListLink listLink = (ListLink) childMemoryBlock;
              this.MyMeter.MyDeviceMemory.SetUshortValue(listLink.BlockStartAddress, (ushort) 0);
              pointer.Add(listLink.BlockStartAddress, listLink.Name);
              break;
            case MBusParameter _:
              if (!((MBusParameter) childMemoryBlock).InsertData(ref pointer))
                return false;
              break;
            case MBusParameterGroup _:
              if (!((MBusParameterGroup) childMemoryBlock).InsertData(ref pointer))
                return false;
              break;
            case S3_DataBlock _:
              if (!((S3_DataBlock) childMemoryBlock).InsertData())
                return false;
              break;
            default:
              throw new Exception("INTERNAL ERROR: Unknown type of S3_MemoryBlock detected!!!! " + childMemoryBlock?.ToString());
          }
        }
      }
      return true;
    }

    internal MBusParameter AddParameter(string parameterName)
    {
      return this.AddParameter(parameterName, new int?());
    }

    internal MBusParameter AddParameter(string parameterName, int? pos)
    {
      return !this.MyMeter.MyParameters.ParameterByName.ContainsKey(parameterName) ? (MBusParameter) null : this.AddParameter(this.MyMeter.MyParameters.ParameterByName[parameterName], pos);
    }

    internal MBusParameter AddParameter(S3_Parameter parameter, int? pos)
    {
      if (parameter == null || parameter.Statics.DefaultDifVif == null || parameter.Statics.DefaultDifVif.Length == 0)
        return (MBusParameter) null;
      int num = parameter.ByteSize;
      MBusParameter mbusParameter = !pos.HasValue ? new MBusParameter(this.MyMeter, (S3_MemoryBlock) this, this.childMemoryBlocks.Count - 1) : new MBusParameter(this.MyMeter, (S3_MemoryBlock) this, pos.Value);
      MBusDifVif mbusDifVif = new MBusDifVif();
      mbusDifVif.LoadDifVif(parameter.Statics.DefaultDifVif);
      mbusDifVif.Subunit = this.VirtualDeviceNumber != (byte) 0 ? 0 : (int) parameter.Statics.VirtualDeviceNumber;
      ParamCode paramCode = ParamCode.None;
      if (parameter.Statics.S3_VarType == S3_VariableTypes.MeterTime1980)
      {
        if (mbusDifVif.IsDateTime_32bit)
        {
          paramCode = ParamCode.DateTime;
          num = 4;
        }
        else
        {
          if (!mbusDifVif.IsDate_16bit)
            throw new Exception("Unknown DIF/VIF by Date detected! Value: 0x" + Util.ByteArrayToHexString(parameter.Statics.DefaultDifVif));
          paramCode = ParamCode.Date;
          num = 2;
        }
      }
      mbusParameter.ControlWord0 = new ControlWord0()
      {
        DataCount = num,
        ControlLogger = ControlLogger.None,
        DifVifCount = mbusDifVif.DifVifArray.Length,
        IsBCDByRadio = false,
        ItFollowsNextControlWord = false,
        ParamCode = paramCode
      };
      mbusParameter.VifDif = new List<byte>((IEnumerable<byte>) mbusDifVif.DifVifArray);
      mbusParameter.Name = parameter.Name;
      mbusParameter.IsLogger = false;
      mbusParameter.ByteSize = 2 + mbusDifVif.DifVifArray.Length + 2;
      if (mbusParameter.ByteSize % 2 != 0)
        ++mbusParameter.ByteSize;
      return mbusParameter;
    }

    internal ListLink AddLink(string nameOfMbusList) => this.AddLink(nameOfMbusList, new int?());

    internal ListLink AddLink(string nameOfMbusList, int? pos)
    {
      ListLink listLink;
      if (pos.HasValue)
      {
        listLink = new ListLink(this.MyMeter, (S3_MemoryBlock) this, pos.Value);
      }
      else
      {
        pos = new int?(0);
        if (this.childMemoryBlocks != null && this.childMemoryBlocks.Count > 0 && this.childMemoryBlocks[0] is ListLink)
        {
          foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
          {
            if (childMemoryBlock is ListLink)
            {
              int? nullable = pos;
              pos = nullable.HasValue ? new int?(nullable.GetValueOrDefault() + 1) : new int?();
            }
            else
              break;
          }
        }
        listLink = new ListLink(this.MyMeter, (S3_MemoryBlock) this, pos.Value);
      }
      listLink.Name = nameOfMbusList;
      listLink.ByteSize = 2;
      return listLink;
    }

    internal MBusParameter AddLogger(LoggerChanal loggerChanal, int? pos)
    {
      S3_Parameter chanalParameter = loggerChanal.chanalParameter;
      if (chanalParameter == null || chanalParameter.Statics.DefaultDifVif == null || chanalParameter.Statics.DefaultDifVif.Length == 0)
        return (MBusParameter) null;
      MBusParameter mbusParameter = !pos.HasValue ? new MBusParameter(this.MyMeter, (S3_MemoryBlock) this, this.childMemoryBlocks.Count - 1) : new MBusParameter(this.MyMeter, (S3_MemoryBlock) this, pos.Value);
      mbusParameter.ControlWord0 = new ControlWord0()
      {
        DataCount = chanalParameter.ByteSize,
        ControlLogger = ControlLogger.LoggerReset,
        DifVifCount = 0,
        IsBCDByRadio = false,
        ItFollowsNextControlWord = false,
        ParamCode = ParamCode.LogValue
      };
      mbusParameter.Name = loggerChanal.ChanalName;
      mbusParameter.IsLogger = true;
      byte[] loggerDifVif = mbusParameter.GenerateLoggerDifVif(this.VirtualDeviceNumber);
      if (loggerDifVif == null)
      {
        mbusParameter.RemoveFromParentMemoryBlock();
        return (MBusParameter) null;
      }
      mbusParameter.ControlWord0.DifVifCount = loggerDifVif.Length;
      mbusParameter.VifDif = new List<byte>((IEnumerable<byte>) loggerDifVif);
      mbusParameter.ByteSize = 2 + mbusParameter.VifDif.Count + 2;
      if (mbusParameter.ByteSize % 2 != 0)
        ++mbusParameter.ByteSize;
      mbusParameter.firstChildMemoryBlockOffset = mbusParameter.ByteSize;
      return !mbusParameter.CreateIterator(this.VirtualDeviceNumber) ? (MBusParameter) null : mbusParameter;
    }

    internal MBusParameterGroup AddGroup()
    {
      int pos = 0;
      foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
      {
        if (childMemoryBlock is ListLink)
          ++pos;
        else
          break;
      }
      return this.AddGroup(pos);
    }

    internal MBusParameterGroup AddGroup(int pos)
    {
      if (this.childMemoryBlocks != null)
      {
        foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
        {
          if (childMemoryBlock is MBusParameterGroup)
            return (MBusParameterGroup) null;
        }
      }
      return new MBusParameterGroup(this.MyMeter, (S3_MemoryBlock) this, pos)
      {
        LoggerCycleCount = 1,
        ListMaxCount = 1,
        IsDueDate = false,
        StartStorageNumber = 0
      };
    }

    internal void PrintMBusList(StringBuilder printText, string listName, string startString)
    {
      string startString1 = startString + "   ";
      printText.AppendLine(startString + this.Name);
      foreach (S3_MemoryBlock childMemoryBlock1 in this.childMemoryBlocks)
      {
        switch (childMemoryBlock1)
        {
          case ListLink _:
            printText.AppendLine(startString1 + ".. Following list: " + ((ListLink) childMemoryBlock1).Name);
            break;
          case MBusParameter _:
            MBusParameter mbusParameter = (MBusParameter) childMemoryBlock1;
            mbusParameter.PrintParameter(printText, startString1);
            MBusDifVif mbusDifVif1 = new MBusDifVif();
            mbusDifVif1.LoadDifVif(mbusParameter.VifDif.ToArray());
            printText.AppendLine("  " + mbusDifVif1.ToString());
            break;
          case MBusParameterGroup _:
            if (childMemoryBlock1.childMemoryBlocks != null)
            {
              string startString2 = startString1 + "  ";
              int num1 = ((MBusParameter) childMemoryBlock1.childMemoryBlocks[childMemoryBlock1.childMemoryBlocks.Count - 1]).ControlWord1.LoggerCycleCount + 1;
              printText.AppendLine(startString1 + "Block (Cycles:" + num1.ToString() + ")");
              int num2 = childMemoryBlock1.childMemoryBlocks.Count / 2;
              for (int index = 0; index < num2; ++index)
              {
                MBusParameter childMemoryBlock2 = (MBusParameter) childMemoryBlock1.childMemoryBlocks[index];
                childMemoryBlock2.PrintParameter(printText, startString2);
                MBusDifVif mbusDifVif2 = new MBusDifVif();
                mbusDifVif2.LoadDifVif(childMemoryBlock2.VifDif.ToArray());
                printText.AppendLine("  " + mbusDifVif2.ToString());
              }
            }
            break;
        }
      }
    }
  }
}
