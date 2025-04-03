// Decompiled with JetBrains decompiler
// Type: S3_Handler.RadioList
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using DeviceCollector;
using HandlerLib;
using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal sealed class RadioList : S3_MemoryBlock
  {
    internal string Name
    {
      get
      {
        return "Radio list " + this.parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) this).ToString();
      }
    }

    internal byte VirtualDeviceNumber
    {
      get => (byte) this.FindHeaderItemOfList(this).IndexOfVirtualDevice;
    }

    public RadioList(S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock)
      : base(MyMeter, S3_MemorySegment.RadioList, parentMemoryBlock)
    {
    }

    public RadioList(
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock,
      S3_MemoryBlock sourceMemoryBlock)
      : base(MyMeter, parentMemoryBlock, sourceMemoryBlock)
    {
    }

    public RadioList(S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock, int insertIndex)
      : base(MyMeter, S3_MemorySegment.RadioList, parentMemoryBlock, insertIndex)
    {
    }

    private RadioListHeaderItem FindHeaderItemOfList(RadioList list)
    {
      if (list == null)
        return (RadioListHeaderItem) null;
      for (int index1 = list.parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) list); index1 >= 0; --index1)
      {
        if (list.parentMemoryBlock.childMemoryBlocks[index1] is RadioListHeader)
        {
          RadioListHeader childMemoryBlock = list.parentMemoryBlock.childMemoryBlocks[index1] as RadioListHeader;
          int index2 = list.parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) list) - 1 - index1;
          if (childMemoryBlock.childMemoryBlocks[index2] is RadioListHeaderItem)
            return childMemoryBlock.childMemoryBlocks[index2] as RadioListHeaderItem;
        }
      }
      return (RadioListHeaderItem) null;
    }

    internal bool CreateFromMemory()
    {
      for (ushort ushortValue = this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress); ushortValue > (ushort) 0; ushortValue = this.MyMeter.MyDeviceMemory.GetUshortValue(this.StartAddressOfNextBlock))
        new MBusParameter(this.MyMeter, (S3_MemoryBlock) this).CreateFromMemory(true);
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

    internal RadioList Clone(S3_Meter theCloneMeter, S3_MemoryBlock parentMemoryBlock)
    {
      RadioList radioList = new RadioList(theCloneMeter, parentMemoryBlock, (S3_MemoryBlock) this);
      if (this.childMemoryBlocks != null)
      {
        foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
        {
          if (childMemoryBlock is MBusParameter)
            ((MBusParameter) childMemoryBlock).Clone(theCloneMeter, (S3_MemoryBlock) radioList);
          if (childMemoryBlock is S3_DataBlock)
            ((S3_DataBlock) childMemoryBlock).Clone(theCloneMeter, (S3_MemoryBlock) radioList);
          if (childMemoryBlock is MBusParameterGroup)
            ((MBusParameterGroup) childMemoryBlock).Clone(theCloneMeter, (S3_MemoryBlock) radioList);
        }
      }
      if (this.sourceMemoryBlock != null)
        radioList.sourceMemoryBlock = this.sourceMemoryBlock;
      return radioList;
    }

    internal bool InsertData(ref Dictionary<int, string> pointer)
    {
      this.MyMeter.MyLinker.AddLabel(this.Name, this.BlockStartAddress);
      if (this.childMemoryBlocks != null)
      {
        foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
        {
          switch (childMemoryBlock)
          {
            case MBusParameter _:
              MBusParameter mbusParameter = childMemoryBlock as MBusParameter;
              if (!mbusParameter.InsertData(ref pointer) || mbusParameter.Iteration != null && !mbusParameter.Iteration.InsertData(ref pointer))
                return false;
              break;
            case S3_DataBlock _:
              if (!(childMemoryBlock as S3_DataBlock).InsertData())
                return false;
              break;
            case MBusParameterGroup _:
              if (!((MBusParameterGroup) childMemoryBlock).InsertData(ref pointer))
                return false;
              break;
          }
        }
      }
      return true;
    }

    internal MBusParameter AddParameter(
      S3_Parameter parameter,
      int? pos,
      RadioListHeader radioHeader,
      int indexOfVirtualDevice)
    {
      if (parameter == null || radioHeader == null)
        return (MBusParameter) null;
      bool flag = radioHeader.Mode == RADIO_MODE.wMBusC1 || radioHeader.Mode == RADIO_MODE.wMBusC1B || radioHeader.Mode == RADIO_MODE.wMBusS1 || radioHeader.Mode == RADIO_MODE.wMBusS2 || radioHeader.Mode == RADIO_MODE.wMBusT1 || radioHeader.Mode == RADIO_MODE.wMBusT2Met || radioHeader.Mode == RADIO_MODE.wMBusT2Oth;
      if (flag && (parameter.Statics.DefaultDifVif == null || parameter.Statics.DefaultDifVif.Length == 0))
        return (MBusParameter) null;
      MBusParameter mbusParameter = !pos.HasValue ? new MBusParameter(this.MyMeter, (S3_MemoryBlock) this, this.childMemoryBlocks.Count - 1) : new MBusParameter(this.MyMeter, (S3_MemoryBlock) this, pos.Value);
      MBusDifVif mbusDifVif = (MBusDifVif) null;
      ParamCode paramCode = ParamCode.None;
      int num = parameter.ByteSize;
      if (flag)
      {
        mbusDifVif = new MBusDifVif();
        mbusDifVif.LoadDifVif(parameter.Statics.DefaultDifVif);
        mbusDifVif.Subunit = indexOfVirtualDevice != 0 ? 0 : (int) parameter.Statics.VirtualDeviceNumber;
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
      }
      mbusParameter.ControlWord0 = new ControlWord0()
      {
        DataCount = num,
        ControlLogger = ControlLogger.None,
        DifVifCount = flag ? mbusDifVif.DifVifArray.Length : 0,
        IsBCDByRadio = false,
        ItFollowsNextControlWord = false,
        ParamCode = paramCode
      };
      mbusParameter.VifDif = flag ? new List<byte>((IEnumerable<byte>) mbusDifVif.DifVifArray) : new List<byte>();
      mbusParameter.Name = parameter.Name;
      mbusParameter.IsLogger = false;
      mbusParameter.ByteSize = 2 + mbusParameter.ControlWord0.DifVifCount + 2;
      if (flag && mbusParameter.ByteSize % 2 != 0)
        ++mbusParameter.ByteSize;
      return mbusParameter;
    }

    internal MBusParameter AddLogger(
      LoggerChanal loggerChanal,
      int? pos,
      RadioListHeader radioHeader,
      int VirtualDeviceNumber)
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
      byte[] loggerDifVif = mbusParameter.GenerateLoggerDifVif(Convert.ToByte(VirtualDeviceNumber));
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
      return !mbusParameter.CreateIterator(Convert.ToByte(VirtualDeviceNumber)) ? (MBusParameter) null : mbusParameter;
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

    internal List<MBusParameter> InsertRadio3Scenario5Parameter(
      S3_Parameter parameter,
      RadioListHeader header,
      int indexOfVirtualDevice)
    {
      List<MBusParameter> mbusParameterList = new List<MBusParameter>();
      LoggerChanal loggerChanal1 = this.MyMeter.MyLoggerManager.GetLoggerChanal("LC_Year_" + parameter.Name);
      LoggerChanal loggerChanal2 = this.MyMeter.MyLoggerManager.GetLoggerChanal("LC_Month_" + parameter.Name);
      if (loggerChanal2 == null)
        throw new ArgumentException("Radio3 Scenario3 Parameter Error: Loggerchanel is not available!");
      byte num1 = 16;
      string name1 = parameter.Name;
      S3_ParameterNames s3ParameterNames = S3_ParameterNames.Energy_HeatEnergyDisplay;
      string str1 = s3ParameterNames.ToString();
      int num2;
      if (!(name1 == str1))
      {
        string name2 = parameter.Name;
        s3ParameterNames = S3_ParameterNames.Energy_ColdEnergyDisplay;
        string str2 = s3ParameterNames.ToString();
        num2 = name2 == str2 ? 1 : 0;
      }
      else
        num2 = 1;
      byte num3 = num2 == 0 ? (byte) ((uint) num1 | 1U) : (byte) ((uint) num1 | 4U);
      S3_Parameter parameter1 = this.MyMeter.MyParameters.ParameterByName["Bak_TimeBaseSecounds"];
      if (parameter1 == null)
        return (List<MBusParameter>) null;
      MBusParameter mbusParameter1 = this.AddParameter(parameter1, new int?(0), header, indexOfVirtualDevice);
      mbusParameter1.ControlWord0.DataCount = 2;
      mbusParameter1.ControlWord0.DifVifCount = 2;
      mbusParameter1.VifDif = new List<byte>()
      {
        (byte) 0,
        num3
      };
      mbusParameter1.ControlWord0.ParamCode = ParamCode.Date;
      mbusParameter1.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter1.ControlWord1 = new ControlWord1();
      mbusParameter1.ByteSize += 4;
      mbusParameter1.ControlWord1.IsSwitch = true;
      mbusParameterList.Add(mbusParameter1);
      LoggerChanal loggerChanal3;
      ControlLogger controlLogger;
      if (loggerChanal1 != null)
      {
        loggerChanal3 = loggerChanal1;
        controlLogger = ControlLogger.LoggerReset;
      }
      else
      {
        loggerChanal3 = loggerChanal2;
        controlLogger = ControlLogger.LoggerDueDateReset;
      }
      MBusParameter mbusParameter2 = this.AddLogger(loggerChanal3, new int?(1), header, indexOfVirtualDevice);
      mbusParameter2.UpdateLogger(ParamCode.LogDate, 1, controlLogger);
      mbusParameter2.VifDif = new List<byte>();
      mbusParameter2.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter2.ControlWord1 = new ControlWord1();
      mbusParameter2.ControlWord1.IsSwitch = true;
      mbusParameter2.ByteSize = 6;
      mbusParameterList.Add(mbusParameter2);
      MBusParameter mbusParameter3 = this.AddParameter(parameter, new int?(2), header, indexOfVirtualDevice);
      mbusParameter3.ControlWord0.ParamCode = ParamCode.None;
      mbusParameter3.ControlWord0.IsBCDByRadio = true;
      mbusParameter3.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter3.ControlWord1 = new ControlWord1();
      mbusParameter3.ByteSize += 2;
      mbusParameter3.ControlWord1.IsSwitch = true;
      mbusParameterList.Add(mbusParameter3);
      MBusParameter mbusParameter4 = this.AddLogger(loggerChanal2, new int?(3), header, indexOfVirtualDevice);
      mbusParameter4.firstChildMemoryBlockOffset += -mbusParameter4.VifDif.Count + 2;
      mbusParameter4.VifDif = new List<byte>();
      mbusParameter4.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter4.ControlWord0.IsBCDByRadio = true;
      mbusParameter4.ControlWord1 = new ControlWord1();
      mbusParameter4.ControlWord1.DecodeCommand = DecodeCommand.SaveValue;
      mbusParameter4.ControlWord1.IsSwitch = true;
      mbusParameter4.Iteration.VifDif = new List<byte>();
      mbusParameter4.Iteration.ControlWord0.DataCount = 3;
      mbusParameter4.Iteration.ControlWord0.IsBCDByRadio = true;
      mbusParameter4.Iteration.ControlWord1.DecodeCommand = DecodeCommand.DiffValue;
      mbusParameter4.Iteration.ControlWord1.LoggerCycleCount = 11;
      mbusParameter4.Iteration.ControlWord1.IsSwitch = true;
      mbusParameter4.Iteration.ByteSize = 4;
      mbusParameter4.ByteSize = 10;
      mbusParameterList.Add(mbusParameter4);
      return mbusParameterList;
    }

    internal List<MBusParameter> InsertRadio3Scenario3Parameter(
      S3_Parameter parameter,
      RadioListHeader header,
      int indexOfVirtualDevice)
    {
      List<MBusParameter> mbusParameterList = new List<MBusParameter>();
      LoggerChanal loggerChanal1 = this.MyMeter.MyLoggerManager.GetLoggerChanal("LC_Year_" + parameter.Name);
      LoggerChanal loggerChanal2 = this.MyMeter.MyLoggerManager.GetLoggerChanal("LC_Month_" + parameter.Name);
      if (loggerChanal2 == null)
        throw new ArgumentException("Radio3 Scenario3 Parameter Error: Loggerchanel is not available!");
      S3_Parameter parameter1 = this.MyMeter.MyParameters.ParameterByName["Bak_TimeBaseSecounds"];
      if (parameter1 == null)
        return (List<MBusParameter>) null;
      MBusParameter mbusParameter1 = this.AddParameter(parameter1, new int?(0), header, indexOfVirtualDevice);
      mbusParameter1.ControlWord0.ParamCode = ParamCode.DateTime;
      mbusParameter1.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter1.ControlWord1 = new ControlWord1();
      mbusParameter1.ByteSize += 2;
      mbusParameter1.ControlWord1.IsSwitch = true;
      mbusParameterList.Add(mbusParameter1);
      LoggerChanal loggerChanal3;
      ControlLogger controlLogger;
      if (loggerChanal1 != null)
      {
        loggerChanal3 = loggerChanal1;
        controlLogger = ControlLogger.LoggerReset;
      }
      else
      {
        loggerChanal3 = loggerChanal2;
        controlLogger = ControlLogger.LoggerDueDateReset;
      }
      MBusParameter mbusParameter2 = this.AddLogger(loggerChanal3, new int?(1), header, indexOfVirtualDevice);
      mbusParameter2.UpdateLogger(ParamCode.LogDate, 1, controlLogger);
      mbusParameter2.VifDif = new List<byte>();
      mbusParameter2.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter2.ControlWord1 = new ControlWord1();
      mbusParameter2.ControlWord1.IsSwitch = true;
      mbusParameter2.ByteSize = 6;
      mbusParameterList.Add(mbusParameter2);
      MBusParameter mbusParameter3 = this.AddLogger(loggerChanal3, new int?(2), header, indexOfVirtualDevice);
      mbusParameter3.UpdateLogger(ParamCode.LogValue, 1, controlLogger);
      mbusParameter3.VifDif = new List<byte>();
      mbusParameter3.ControlWord0.ControlLogger = ControlLogger.None;
      mbusParameter3.ControlWord0.IsBCDByRadio = true;
      mbusParameter3.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter3.ControlWord1 = new ControlWord1();
      mbusParameter3.ControlWord1.IsSwitch = true;
      mbusParameter3.ByteSize = 4;
      mbusParameterList.Add(mbusParameter3);
      MBusParameter mbusParameter4 = this.AddLogger(loggerChanal2, new int?(3), header, indexOfVirtualDevice);
      mbusParameter4.firstChildMemoryBlockOffset += -mbusParameter4.VifDif.Count + 2;
      mbusParameter4.VifDif = new List<byte>();
      mbusParameter4.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter4.ControlWord0.IsBCDByRadio = true;
      mbusParameter4.ControlWord1 = new ControlWord1();
      mbusParameter4.ControlWord1.DecodeCommand = DecodeCommand.SaveValue;
      mbusParameter4.ControlWord1.IsSwitch = true;
      mbusParameter4.Iteration.VifDif = new List<byte>();
      mbusParameter4.Iteration.ControlWord0.DataCount = 3;
      mbusParameter4.Iteration.ControlWord0.IsBCDByRadio = true;
      mbusParameter4.Iteration.ControlWord1.DecodeCommand = DecodeCommand.DiffValue;
      mbusParameter4.Iteration.ControlWord1.LoggerCycleCount = 11;
      mbusParameter4.Iteration.ControlWord1.IsSwitch = true;
      mbusParameter4.Iteration.ByteSize = 4;
      mbusParameter4.ByteSize = 10;
      mbusParameterList.Add(mbusParameter4);
      return mbusParameterList;
    }

    internal List<MBusParameter> InsertRadio3Scenario1Parameter(
      S3_Parameter parameter,
      RadioListHeader header,
      int indexOfVirtualDevice)
    {
      List<MBusParameter> mbusParameterList = new List<MBusParameter>();
      LoggerChanal loggerChanal1 = this.MyMeter.MyLoggerManager.GetLoggerChanal("LC_Year_" + parameter.Name);
      LoggerChanal loggerChanal2 = this.MyMeter.MyLoggerManager.GetLoggerChanal("LC_Month_" + parameter.Name);
      LoggerChanal loggerChanal3 = this.MyMeter.MyLoggerManager.GetLoggerChanal("LC_Day_" + parameter.Name);
      if (loggerChanal2 == null || loggerChanal3 == null)
        throw new ArgumentException("Radio3 Scenario1 Parameter Error: Loggerchanel is not available!");
      S3_Parameter parameter1 = this.MyMeter.MyParameters.ParameterByName["Bak_TimeBaseSecounds"];
      if (parameter1 == null)
        return (List<MBusParameter>) null;
      MBusParameter mbusParameter1 = this.AddParameter(parameter1, new int?(0), header, indexOfVirtualDevice);
      mbusParameter1.ControlWord0.ParamCode = ParamCode.DateTime;
      mbusParameter1.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter1.ControlWord1 = new ControlWord1();
      mbusParameter1.ByteSize += 2;
      mbusParameter1.ControlWord1.IsSwitch = true;
      mbusParameterList.Add(mbusParameter1);
      LoggerChanal loggerChanal4;
      ControlLogger controlLogger;
      if (loggerChanal1 != null)
      {
        loggerChanal4 = loggerChanal1;
        controlLogger = ControlLogger.LoggerReset;
      }
      else
      {
        loggerChanal4 = loggerChanal2;
        controlLogger = ControlLogger.LoggerDueDateReset;
      }
      MBusParameter mbusParameter2 = this.AddLogger(loggerChanal4, new int?(1), header, indexOfVirtualDevice);
      mbusParameter2.UpdateLogger(ParamCode.LogDate, 1, controlLogger);
      mbusParameter2.VifDif = new List<byte>();
      mbusParameter2.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter2.ControlWord1 = new ControlWord1();
      mbusParameter2.ControlWord1.IsSwitch = true;
      mbusParameter2.ByteSize = 6;
      mbusParameterList.Add(mbusParameter2);
      MBusParameter mbusParameter3 = this.AddLogger(loggerChanal4, new int?(2), header, indexOfVirtualDevice);
      mbusParameter3.UpdateLogger(ParamCode.LogValue, 1, controlLogger);
      mbusParameter3.VifDif = new List<byte>();
      mbusParameter3.ControlWord0.ControlLogger = ControlLogger.None;
      mbusParameter3.ControlWord0.IsBCDByRadio = true;
      mbusParameter3.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter3.ControlWord1 = new ControlWord1();
      mbusParameter3.ControlWord1.IsSwitch = true;
      mbusParameter3.ByteSize = 4;
      mbusParameterList.Add(mbusParameter3);
      MBusParameter mbusParameter4 = this.AddLogger(loggerChanal2, new int?(3), header, indexOfVirtualDevice);
      mbusParameter4.UpdateLogger(ParamCode.LogValue, 1, ControlLogger.LoggerReset);
      mbusParameter4.VifDif = new List<byte>();
      mbusParameter4.ControlWord0.IsBCDByRadio = true;
      mbusParameter4.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter4.ControlWord1 = new ControlWord1();
      mbusParameter4.ControlWord1.DecodeCommand = DecodeCommand.SaveValue;
      mbusParameter4.ControlWord1.IsSwitch = true;
      mbusParameter4.ByteSize = 6;
      mbusParameterList.Add(mbusParameter4);
      MBusParameter mbusParameter5 = this.AddLogger(loggerChanal2, new int?(4), header, indexOfVirtualDevice);
      mbusParameter5.UpdateLogger(ParamCode.LogValue, 1, ControlLogger.LoggerReset);
      mbusParameter5.ControlWord0.ControlLogger = ControlLogger.None;
      mbusParameter5.VifDif = new List<byte>();
      mbusParameter5.ControlWord0.IsBCDByRadio = true;
      mbusParameter5.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter5.ControlWord1 = new ControlWord1();
      mbusParameter5.ControlWord1.IsSwitch = true;
      mbusParameter5.ControlWord1.DecodeCommand = DecodeCommand.LoggerSelectList;
      mbusParameter5.ByteSize = 4;
      mbusParameterList.Add(mbusParameter5);
      MBusParameter mbusParameter6 = this.AddLogger(loggerChanal2, new int?(5), header, indexOfVirtualDevice);
      mbusParameter6.UpdateLogger(ParamCode.LogValue, 1, ControlLogger.LoggerReset);
      mbusParameter6.ControlWord0.ControlLogger = ControlLogger.None;
      mbusParameter6.VifDif = new List<byte>();
      mbusParameter6.ControlWord0.IsBCDByRadio = true;
      mbusParameter6.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter6.ControlWord1 = new ControlWord1();
      mbusParameter6.ControlWord1.IsSwitch = true;
      mbusParameter6.ControlWord1.DecodeCommand = DecodeCommand.LoggerSelectList;
      mbusParameter6.ControlWord1.LoggerNextList = 1;
      mbusParameter6.ByteSize = 4;
      mbusParameterList.Add(mbusParameter6);
      MBusParameter mbusParameter7 = this.AddLogger(loggerChanal3, new int?(6), header, indexOfVirtualDevice);
      mbusParameter7.UpdateLogger(ParamCode.LogValue, 1, ControlLogger.LoggerReset);
      mbusParameter7.VifDif = new List<byte>();
      mbusParameter7.ControlWord0.IsBCDByRadio = true;
      mbusParameter7.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter7.ControlWord1 = new ControlWord1();
      mbusParameter7.ControlWord1.IsSwitch = true;
      mbusParameter7.ControlWord1.DecodeCommand = DecodeCommand.SaveValue | DecodeCommand.LoggerSelectList;
      mbusParameter7.ControlWord1.LoggerNextList = 2;
      mbusParameter7.ByteSize = 6;
      mbusParameterList.Add(mbusParameter7);
      MBusParameter mbusParameter8 = this.AddLogger(loggerChanal3, new int?(7), header, indexOfVirtualDevice);
      mbusParameter8.UpdateLogger(ParamCode.LogValue, 1, ControlLogger.LoggerReset);
      mbusParameter8.ControlWord0.DataCount = 3;
      mbusParameter8.ControlWord0.ControlLogger = ControlLogger.None;
      mbusParameter8.VifDif = new List<byte>();
      mbusParameter8.ControlWord0.IsBCDByRadio = true;
      mbusParameter8.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter8.ControlWord1 = new ControlWord1();
      mbusParameter8.ControlWord1.IsSwitch = true;
      mbusParameter8.ControlWord1.DecodeCommand = DecodeCommand.DiffValue | DecodeCommand.LoggerSelectList;
      mbusParameter8.ControlWord1.LoggerNextList = 3;
      mbusParameter8.ByteSize = 4;
      mbusParameterList.Add(mbusParameter8);
      MBusParameter mbusParameter9 = this.AddLogger(loggerChanal3, new int?(8), header, indexOfVirtualDevice);
      mbusParameter9.UpdateLogger(ParamCode.LogValue, 1, ControlLogger.LoggerReset);
      mbusParameter9.ControlWord0.DataCount = 3;
      mbusParameter9.ControlWord0.ControlLogger = ControlLogger.None;
      mbusParameter9.VifDif = new List<byte>();
      mbusParameter9.ControlWord0.IsBCDByRadio = true;
      mbusParameter9.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter9.ControlWord1 = new ControlWord1();
      mbusParameter9.ControlWord1.IsSwitch = true;
      mbusParameter9.ControlWord1.DecodeCommand = DecodeCommand.DiffValue | DecodeCommand.LoggerSelectList;
      mbusParameter9.ControlWord1.LoggerNextList = 3;
      mbusParameter9.ByteSize = 4;
      mbusParameterList.Add(mbusParameter9);
      MBusParameter mbusParameter10 = this.AddLogger(loggerChanal3, new int?(9), header, indexOfVirtualDevice);
      mbusParameter10.UpdateLogger(ParamCode.LogValue, 1, ControlLogger.LoggerReset);
      mbusParameter10.ControlWord0.DataCount = 3;
      mbusParameter10.ControlWord0.ControlLogger = ControlLogger.None;
      mbusParameter10.VifDif = new List<byte>();
      mbusParameter10.ControlWord0.IsBCDByRadio = true;
      mbusParameter10.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter10.ControlWord1 = new ControlWord1();
      mbusParameter10.ControlWord1.IsSwitch = true;
      mbusParameter10.ControlWord1.DecodeCommand = DecodeCommand.DiffValue | DecodeCommand.LoggerSelectList;
      mbusParameter10.ControlWord1.LoggerNextList = 3;
      mbusParameter10.ByteSize = 4;
      mbusParameterList.Add(mbusParameter10);
      MBusParameter mbusParameter11 = this.AddLogger(loggerChanal3, new int?(10), header, indexOfVirtualDevice);
      mbusParameter11.UpdateLogger(ParamCode.LogValue, 1, ControlLogger.LoggerReset);
      mbusParameter11.ControlWord0.DataCount = 3;
      mbusParameter11.ControlWord0.ControlLogger = ControlLogger.None;
      mbusParameter11.VifDif = new List<byte>();
      mbusParameter11.ControlWord0.IsBCDByRadio = true;
      mbusParameter11.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter11.ControlWord1 = new ControlWord1();
      mbusParameter11.ControlWord1.IsSwitch = true;
      mbusParameter11.ControlWord1.DecodeCommand = DecodeCommand.DiffValue | DecodeCommand.LoggerSelectList;
      mbusParameter11.ControlWord1.LoggerNextList = 4;
      mbusParameter11.ByteSize = 4;
      mbusParameterList.Add(mbusParameter11);
      MBusParameter mbusParameter12 = this.AddLogger(loggerChanal3, new int?(11), header, indexOfVirtualDevice);
      mbusParameter12.UpdateLogger(ParamCode.LogValue, 1, ControlLogger.LoggerReset);
      mbusParameter12.ControlWord0.DataCount = 3;
      mbusParameter12.ControlWord0.ControlLogger = ControlLogger.None;
      mbusParameter12.VifDif = new List<byte>();
      mbusParameter12.ControlWord0.IsBCDByRadio = true;
      mbusParameter12.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter12.ControlWord1 = new ControlWord1();
      mbusParameter12.ControlWord1.IsSwitch = true;
      mbusParameter12.ControlWord1.DecodeCommand = DecodeCommand.DiffValue | DecodeCommand.LoggerSelectList;
      mbusParameter12.ControlWord1.LoggerNextList = 5;
      mbusParameter12.ByteSize = 4;
      mbusParameterList.Add(mbusParameter12);
      return mbusParameterList;
    }

    internal List<MBusParameter> InsertRadio2WalkbyParameter(
      S3_Parameter parameter,
      RadioListHeader header,
      int indexOfVirtualDevice)
    {
      List<MBusParameter> mbusParameterList = new List<MBusParameter>();
      if (parameter == null)
        throw new ArgumentException("Radio2 Walkby Parameter Error: Parameter is not available!");
      LoggerChanal loggerChanal1 = this.MyMeter.MyLoggerManager.GetLoggerChanal("LC_Year_" + parameter.Name);
      LoggerChanal loggerChanal2 = this.MyMeter.MyLoggerManager.GetLoggerChanal("LC_Month_" + parameter.Name);
      LoggerChanal loggerChanal3;
      ControlLogger controlLogger;
      if (loggerChanal1 != null)
      {
        loggerChanal3 = loggerChanal1;
        controlLogger = ControlLogger.LoggerReset;
      }
      else
      {
        loggerChanal3 = loggerChanal2;
        controlLogger = ControlLogger.LoggerDueDateReset;
      }
      if (loggerChanal2 == null || loggerChanal3 == null)
        throw new ArgumentException("Radio2 Walkby Parameter Error: Loggerchanel is not available!");
      S3_Parameter parameter1 = this.MyMeter.MyParameters.ParameterByName["Bak_TimeBaseSecounds"];
      if (parameter1 == null)
        return (List<MBusParameter>) null;
      MBusParameter mbusParameter1 = this.AddParameter(parameter1, new int?(0), header, indexOfVirtualDevice);
      mbusParameter1.ControlWord0.DataCount = 2;
      mbusParameter1.ControlWord0.ParamCode = ParamCode.Date;
      mbusParameter1.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter1.ControlWord1 = new ControlWord1();
      mbusParameter1.ByteSize += 2;
      mbusParameter1.ControlWord1.IsSwitch = true;
      mbusParameterList.Add(mbusParameter1);
      MBusParameter mbusParameter2 = this.AddLogger(loggerChanal3, new int?(1), header, indexOfVirtualDevice);
      mbusParameter2.UpdateLogger(ParamCode.LogDate, 1, controlLogger);
      mbusParameter2.VifDif = new List<byte>();
      mbusParameter2.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter2.ControlWord1 = new ControlWord1();
      mbusParameter2.ControlWord1.IsSwitch = true;
      mbusParameter2.ByteSize = 6;
      mbusParameterList.Add(mbusParameter2);
      MBusParameter mbusParameter3 = this.AddLogger(loggerChanal2, new int?(2), header, indexOfVirtualDevice);
      mbusParameter3.UpdateLogger(ParamCode.LogValue, 1, ControlLogger.LoggerReset);
      mbusParameter3.VifDif = new List<byte>();
      mbusParameter3.ControlWord0.IsBCDByRadio = true;
      mbusParameter3.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter3.ControlWord1 = new ControlWord1();
      mbusParameter3.ControlWord1.IsSwitch = true;
      mbusParameter3.ByteSize = 6;
      mbusParameterList.Add(mbusParameter3);
      MBusParameter mbusParameter4 = this.AddLogger(loggerChanal3, new int?(3), header, indexOfVirtualDevice);
      mbusParameter4.UpdateLogger(ParamCode.LogValue, 1, controlLogger);
      mbusParameter4.VifDif = new List<byte>();
      mbusParameter4.ControlWord0.IsBCDByRadio = true;
      mbusParameter4.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter4.ControlWord1 = new ControlWord1();
      mbusParameter4.ControlWord1.IsSwitch = true;
      mbusParameter4.ByteSize = 6;
      mbusParameterList.Add(mbusParameter4);
      MBusParameter mbusParameter5 = this.AddParameter(parameter, new int?(4), header, indexOfVirtualDevice);
      mbusParameter5.ControlWord0.ParamCode = ParamCode.None;
      mbusParameter5.ControlWord0.ItFollowsNextControlWord = true;
      mbusParameter5.ControlWord1 = new ControlWord1();
      mbusParameter5.ByteSize += 2;
      mbusParameter5.ControlWord1.IsSwitch = true;
      mbusParameterList.Add(mbusParameter5);
      return mbusParameterList;
    }

    internal void PrintRadioListParameter(StringBuilder printText, string startString)
    {
      if (this.childMemoryBlocks == null)
        return;
      string startString1 = startString + "  ";
      foreach (S3_MemoryBlock childMemoryBlock1 in this.childMemoryBlocks)
      {
        if (childMemoryBlock1 is MBusParameter)
        {
          MBusParameter mbusParameter = (MBusParameter) childMemoryBlock1;
          mbusParameter.PrintParameter(printText, startString1);
          MBusDifVif mbusDifVif = new MBusDifVif();
          mbusDifVif.LoadDifVif(mbusParameter.VifDif.ToArray());
          if (mbusDifVif.ByteSize != 0)
            printText.AppendLine("  " + mbusDifVif.ToString());
          else
            printText.AppendLine("  ");
        }
        else if (childMemoryBlock1 is MBusParameterGroup && childMemoryBlock1.childMemoryBlocks != null)
        {
          string startString2 = startString1 + "  ";
          int num1 = ((MBusParameter) childMemoryBlock1.childMemoryBlocks[childMemoryBlock1.childMemoryBlocks.Count - 1]).ControlWord1.LoggerCycleCount + 1;
          printText.AppendLine(startString1 + "Block (Cycles:" + num1.ToString() + ")");
          int num2 = childMemoryBlock1.childMemoryBlocks.Count / 2;
          for (int index = 0; index < num2; ++index)
          {
            MBusParameter childMemoryBlock2 = (MBusParameter) childMemoryBlock1.childMemoryBlocks[index];
            childMemoryBlock2.PrintParameter(printText, startString2);
            MBusDifVif mbusDifVif = new MBusDifVif();
            mbusDifVif.LoadDifVif(childMemoryBlock2.VifDif.ToArray());
            printText.AppendLine("  " + mbusDifVif.ToString());
          }
        }
      }
    }
  }
}
