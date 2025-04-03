// Decompiled with JetBrains decompiler
// Type: S3_Handler.MBusParameter
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
  internal sealed class MBusParameter : S3_MemoryBlock
  {
    public ControlWord0 ControlWord0 { get; set; }

    public ControlWord1 ControlWord1 { get; set; }

    public ControlWord2 ControlWord2 { get; set; }

    public List<byte> VifDif { get; set; }

    public bool IsLogger { get; set; }

    public string Name { get; set; }

    public MBusParameter Iteration { get; set; }

    public S3_MemoryBlock SourceItem { get; set; }

    public MBusParameter(S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock)
      : base(MyMeter, S3_MemorySegment.MBusParameter, parentMemoryBlock)
    {
    }

    public MBusParameter(
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock,
      S3_MemoryBlock sourceMemoryBlock)
      : base(MyMeter, parentMemoryBlock, sourceMemoryBlock)
    {
    }

    public MBusParameter(S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock, int insertIndex)
      : base(MyMeter, S3_MemorySegment.MBusParameter, parentMemoryBlock, insertIndex)
    {
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(this.Name);
      stringBuilder.Append("IsLogger: ").AppendLine(this.IsLogger.ToString());
      stringBuilder.AppendLine(this.ControlWord0.ToString());
      if (this.ControlWord1 != null)
        stringBuilder.AppendLine(this.ControlWord1.ToString());
      if (this.ControlWord2 != null)
        stringBuilder.AppendLine(this.ControlWord2.ToString());
      if (this.VifDif != null)
        stringBuilder.Append("VifDif: ").AppendLine(Util.ByteArrayToHexString(this.VifDif.ToArray()));
      if (this.Iteration != null)
      {
        stringBuilder.AppendLine("Iteration: ------------------");
        stringBuilder.AppendLine(this.Iteration.ToString());
      }
      return stringBuilder.ToString();
    }

    internal bool CreateFromMemory(bool isEnabledGroupBy)
    {
      this.ControlWord0 = new ControlWord0(this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress));
      if (this.ControlWord0.IsInvalid)
        return false;
      this.ByteSize += 2;
      if (this.ControlWord0.ItFollowsNextControlWord)
      {
        this.ControlWord1 = new ControlWord1(this.MyMeter.MyDeviceMemory.GetUshortValue(this.StartAddressOfNextBlock));
        this.ByteSize += 2;
        if (this.ControlWord1.ItFollowsNextControlWord)
        {
          this.ControlWord2 = new ControlWord2(this.MyMeter.MyDeviceMemory.GetUshortValue(this.StartAddressOfNextBlock));
          this.ByteSize += 2;
        }
      }
      this.VifDif = new List<byte>();
      for (int index = 0; index < this.ControlWord0.DifVifCount; ++index)
      {
        this.VifDif.Add(this.MyMeter.MyDeviceMemory.GetByteValue(this.StartAddressOfNextBlock));
        ++this.ByteSize;
      }
      if (this.ControlWord0.DifVifCount % 2 != 0)
        ++this.ByteSize;
      if (this.ControlWord0.ParamCode == ParamCode.LogDate || this.ControlWord0.ParamCode == ParamCode.LogDateTime || this.ControlWord0.ParamCode == ParamCode.LogValue)
      {
        this.IsLogger = true;
        if (this.ControlWord0.ControlLogger == ControlLogger.LoggerDueDateReset || this.ControlWord0.ControlLogger == ControlLogger.LoggerReset || this.ControlWord0.ControlLogger == ControlLogger.LoggerChangeChanal || this.ControlWord0.ControlLogger == ControlLogger.LoggerNextChangeChanal)
        {
          ushort ushortValue = this.MyMeter.MyDeviceMemory.GetUshortValue(this.StartAddressOfNextBlock);
          this.ByteSize += 2;
          this.Name = this.MyMeter.MyLoggerManager.GetLoggerName(ushortValue);
          this.SourceItem = (S3_MemoryBlock) this.MyMeter.MyLoggerManager.GetLoggerChanal((int) ushortValue);
          if (string.IsNullOrEmpty(this.Name))
            throw new Exception("INTERNAL ERROR: Can not find the logger at address: 0x" + ushortValue.ToString("X4") + Environment.NewLine + this?.ToString());
        }
        else
        {
          if (this.ControlWord0.ControlLogger != ControlLogger.None && this.ControlWord0.ControlLogger != ControlLogger.LoggerNext)
            throw new NotImplementedException("This case is not implemented: ControlWord0.ControlLogger=" + this.ControlWord0.ControlLogger.ToString());
          this.Name = !(this.parentMemoryBlock is MBusParameter) ? (this.parentMemoryBlock.childMemoryBlocks[this.parentMemoryBlock.childMemoryBlocks.Count - 2] as MBusParameter).Name : (this.parentMemoryBlock as MBusParameter).Name;
        }
        bool flag = this.ControlWord0.ControlLogger == ControlLogger.LoggerDueDateReset || this.ControlWord0.ControlLogger == ControlLogger.LoggerReset;
        if (isEnabledGroupBy & flag)
        {
          ControlWord0 controlWord0 = new ControlWord0(this.MyMeter.MyDeviceMemory.GetUshortValue(this.StartAddressOfNextBlock));
          if (!controlWord0.IsInvalid && controlWord0.ParamCode == this.ControlWord0.ParamCode && controlWord0.ItFollowsNextControlWord && new ControlWord1(this.MyMeter.MyDeviceMemory.GetUshortValue(this.StartAddressOfNextBlock + 2)).LoggerCycleCount > 0)
          {
            this.firstChildMemoryBlockOffset = this.ByteSize;
            this.Iteration = new MBusParameter(this.MyMeter, (S3_MemoryBlock) this);
            this.Iteration.CreateFromMemory(false);
            if (this.Iteration.Iteration != null)
              throw new Exception("INTERNAL ERROR: Logger cyclic interator can not contains another cyclic interator!");
          }
        }
      }
      else
      {
        this.IsLogger = false;
        ushort ushortValue = this.MyMeter.MyDeviceMemory.GetUshortValue(this.StartAddressOfNextBlock);
        this.ByteSize += 2;
        if (!this.MyMeter.MyParameters.ParameterByAddress.ContainsKey((int) ushortValue))
          throw new Exception("INTERNAL ERROR: Unknown pointer by M-Bus Parameter detected! Value of pointer: 0x" + ushortValue.ToString("X4"));
        this.Name = this.MyMeter.MyParameters.ParameterByAddress[(int) ushortValue].Name;
        this.SourceItem = (S3_MemoryBlock) this.MyMeter.MyParameters.ParameterByAddress[(int) ushortValue];
      }
      return true;
    }

    internal MBusParameter Clone(S3_Meter theCloneMeter, S3_MemoryBlock parentMemoryBlock)
    {
      MBusParameter parentMemoryBlock1 = new MBusParameter(theCloneMeter, parentMemoryBlock, (S3_MemoryBlock) this);
      parentMemoryBlock1.ControlWord0 = new ControlWord0(this.ControlWord0.ControlWord);
      if (this.ControlWord1 != null)
        parentMemoryBlock1.ControlWord1 = new ControlWord1(this.ControlWord1.ControlWord);
      if (this.ControlWord2 != null)
        parentMemoryBlock1.ControlWord2 = new ControlWord2(this.ControlWord2.ControlWord);
      parentMemoryBlock1.SourceItem = this.SourceItem;
      if (this.VifDif != null)
        parentMemoryBlock1.VifDif = new List<byte>((IEnumerable<byte>) this.VifDif);
      parentMemoryBlock1.IsLogger = this.IsLogger;
      parentMemoryBlock1.Name = this.Name;
      if (this.Iteration != null)
        parentMemoryBlock1.Iteration = this.Iteration.Clone(theCloneMeter, (S3_MemoryBlock) parentMemoryBlock1);
      return parentMemoryBlock1;
    }

    internal bool UpdateLogger(ParamCode paramCode, int countOfValues, ControlLogger controlLogger)
    {
      if (this.ControlWord0 == null || controlLogger == ControlLogger.LoggerNext || controlLogger == ControlLogger.None || paramCode != ParamCode.LogDate && paramCode != ParamCode.LogDateTime && paramCode != ParamCode.LogValue)
        return false;
      this.ControlWord0.ParamCode = paramCode;
      switch (paramCode)
      {
        case ParamCode.LogDate:
          this.ControlWord0.DataCount = 2;
          break;
        case ParamCode.LogDateTime:
          this.ControlWord0.DataCount = 4;
          break;
        default:
          this.ControlWord0.DataCount = this.MyMeter.MyLoggerManager.GetLoggerChanal(this.Name).chanalParameter.ByteSize;
          break;
      }
      this.ControlWord0.ControlLogger = controlLogger;
      S3_MemoryBlock parentMemoryBlock = this.parentMemoryBlock;
      while (!(parentMemoryBlock is MBusList) && !(parentMemoryBlock is RadioList))
        parentMemoryBlock = parentMemoryBlock.parentMemoryBlock;
      if (parentMemoryBlock == null)
        return false;
      byte virtualDeviceNumber = 0;
      if (parentMemoryBlock is MBusList)
        virtualDeviceNumber = ((MBusList) parentMemoryBlock).VirtualDeviceNumber;
      else if (parentMemoryBlock is RadioList)
      {
        int num1 = parentMemoryBlock.parentMemoryBlock.childMemoryBlocks.IndexOf(parentMemoryBlock) - 1;
        int num2 = 0;
        S3_MemoryBlock s3MemoryBlock = parentMemoryBlock;
        while (!(s3MemoryBlock is RadioListHeader))
          s3MemoryBlock = s3MemoryBlock.parentMemoryBlock.childMemoryBlocks[num1 - num2++];
        virtualDeviceNumber = Convert.ToByte(((RadioListHeaderItem) s3MemoryBlock.childMemoryBlocks[num2 - 1]).IndexOfVirtualDevice);
      }
      byte[] loggerDifVif1 = this.GenerateLoggerDifVif(virtualDeviceNumber);
      if (loggerDifVif1 == null)
        return false;
      this.VifDif = new List<byte>((IEnumerable<byte>) loggerDifVif1);
      this.ControlWord0.DifVifCount = loggerDifVif1.Length;
      this.ByteSize = 2;
      if (this.ControlWord0.ItFollowsNextControlWord)
        this.ByteSize += 2;
      this.ByteSize += this.VifDif.Count;
      if (this.VifDif.Count % 2 != 0)
        ++this.ByteSize;
      this.ByteSize += 2;
      if (countOfValues == 1)
      {
        if (this.Iteration != null)
        {
          this.firstChildMemoryBlockOffset = 0;
          this.Iteration.RemoveFromParentMemoryBlock();
          this.Iteration = (MBusParameter) null;
          if (this.childMemoryBlocks != null && this.childMemoryBlocks.Count == 0)
            this.childMemoryBlocks = (List<S3_MemoryBlock>) null;
        }
      }
      else
      {
        if (this.Iteration == null)
        {
          this.firstChildMemoryBlockOffset = this.ByteSize;
          if (!this.CreateIterator(virtualDeviceNumber))
            return false;
        }
        this.Iteration.ControlWord0.ParamCode = paramCode;
        this.Iteration.ControlWord0.DataCount = this.ControlWord0.DataCount;
        this.Iteration.ControlWord1.LoggerCycleCount = countOfValues - 1;
        byte[] loggerDifVif2 = this.Iteration.GenerateLoggerDifVif(virtualDeviceNumber);
        if (loggerDifVif2 == null)
          return false;
        this.Iteration.VifDif = new List<byte>((IEnumerable<byte>) loggerDifVif2);
        this.Iteration.ControlWord0.DifVifCount = loggerDifVif2.Length;
        this.Iteration.ByteSize = 2;
        if (this.Iteration.ControlWord0.ItFollowsNextControlWord)
          this.Iteration.ByteSize += 2;
        this.Iteration.ByteSize += this.Iteration.VifDif.Count;
        if (this.Iteration.VifDif.Count % 2 != 0)
          ++this.Iteration.ByteSize;
      }
      return true;
    }

    internal bool InsertData(ref Dictionary<int, string> pointer)
    {
      this.ControlWord0.DifVifCount = this.VifDif == null ? 0 : this.VifDif.Count;
      this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress, this.ControlWord0.ControlWord);
      int num = this.BlockStartAddress + 2;
      if (this.ControlWord0.ItFollowsNextControlWord)
      {
        this.MyMeter.MyDeviceMemory.SetUshortValue(num, this.ControlWord1.ControlWord);
        num += 2;
        if (this.ControlWord1.ItFollowsNextControlWord)
        {
          this.MyMeter.MyDeviceMemory.SetUshortValue(num, this.ControlWord2.ControlWord);
          num += 2;
        }
      }
      if (this.VifDif != null && this.VifDif.Count > 0)
      {
        this.MyMeter.MyDeviceMemory.SetByteArray(num, this.VifDif.ToArray());
        num += this.VifDif.Count;
        if (this.VifDif.Count % 2 != 0)
        {
          this.MyMeter.MyDeviceMemory.SetByteValue(num, (byte) 0);
          ++num;
        }
      }
      if (this.IsLogger)
      {
        if (!(this.parentMemoryBlock is MBusParameter))
        {
          if (this.ControlWord0.ControlLogger == ControlLogger.LoggerDueDateReset || this.ControlWord0.ControlLogger == ControlLogger.LoggerReset || this.ControlWord0.ControlLogger == ControlLogger.LoggerChangeChanal || this.ControlWord0.ControlLogger == ControlLogger.LoggerNextChangeChanal)
          {
            this.MyMeter.MyDeviceMemory.SetUshortValue(num, (ushort) 0);
            if (pointer.ContainsKey(num))
            {
              if (pointer[num] != this.Name)
                throw new Exception("INTERNAL ERROR");
            }
            else
              pointer.Add(num, this.Name);
          }
          if (this.Iteration != null && this.Iteration.VifDif.Count > 0)
          {
            MBusDifVif mbusDifVif = new MBusDifVif(MBusDifVif.DifVifOptions.DifSizeUnchangabel);
            mbusDifVif.LoadDifVif(this.Iteration.VifDif.ToArray());
            if (mbusDifVif.DifByteSize < 3)
              ZR_ClassLibMessages.AddWarning("Transmit list error. Wrong count of DIF's by the logger parameter! Expexted >=3 see: " + this.Name);
            this.Iteration.InsertData(ref pointer);
          }
        }
      }
      else
      {
        ushort blockStartAddress = (ushort) this.MyMeter.MyParameters.ParameterByName[this.Name].BlockStartAddress;
        this.MyMeter.MyDeviceMemory.SetUshortValue(num, blockStartAddress);
      }
      return true;
    }

    internal void AdjustDifVif()
    {
      if (this.VifDif == null || this.VifDif.Count <= 0)
        return;
      byte[] newDifVif;
      if (!this.MyMeter.MyMeterScaling.MBusDifVifAdjustUnit(this.VifDif.ToArray(), 0, this.Name, out newDifVif, out string _))
        throw new Exception("AdjustDifVif error at: " + this.Name);
      if (newDifVif != null)
      {
        if (newDifVif.Length == this.VifDif.Count)
        {
          this.VifDif = new List<byte>((IEnumerable<byte>) newDifVif);
        }
        else
        {
          int count1 = this.VifDif.Count;
          if (count1 % 2 != 0)
            ++count1;
          this.VifDif = new List<byte>((IEnumerable<byte>) newDifVif);
          this.ControlWord0.DifVifCount = this.VifDif.Count;
          int count2 = this.VifDif.Count;
          if (count2 % 2 != 0)
            ++count2;
          int num = count2 - count1;
          if (num != 0)
            this.ByteSize += num;
        }
      }
    }

    public byte[] GenerateLoggerDifVif(byte virtualDeviceNumber)
    {
      if (this.ControlWord0 == null)
        return (byte[]) null;
      bool flag1 = this.ControlWord0.ControlLogger == ControlLogger.LoggerDueDateReset || this.parentMemoryBlock is MBusParameter && ((MBusParameter) this.parentMemoryBlock).ControlWord0.ControlLogger == ControlLogger.LoggerDueDateReset;
      bool flag2 = this.ControlWord0.ControlLogger == ControlLogger.LoggerNext;
      LoggerChanal loggerChanal = this.MyMeter.MyLoggerManager.GetLoggerChanal(this.Name);
      if (loggerChanal == null || loggerChanal.chanalParameter == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "No logger found! Name: " + this.Name, S3_Meter.S3_MeterLogger);
        return (byte[]) null;
      }
      if (loggerChanal.chanalParameter.Statics.DefaultDifVif == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "No DIF VIF on parameter: " + this.Name, S3_Meter.S3_MeterLogger);
        return (byte[]) null;
      }
      MBusDifVif mbusDifVif1 = new MBusDifVif(MBusDifVif.DifVifOptions.DifSizeUnchangabel);
      if (!mbusDifVif1.LoadDifVif(loggerChanal.chanalParameter.Statics.DefaultDifVif))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal DIF VIF on parameter: " + this.Name, S3_Meter.S3_MeterLogger);
        return (byte[]) null;
      }
      if (loggerChanal.isMaxValues)
        mbusDifVif1.FunctionFiled = FunctionFiled.MaximumValue;
      bool flag3 = loggerChanal.myLoggerConfig != null && loggerChanal.myLoggerConfig.IntervalString == "day";
      MBusDifVif mbusDifVif2;
      if (this.ControlWord0.ParamCode == ParamCode.LogValue)
      {
        mbusDifVif2 = mbusDifVif1;
      }
      else
      {
        if (this.ControlWord0.ParamCode != ParamCode.LogDateTime && this.ControlWord0.ParamCode != ParamCode.LogDate)
          throw new Exception("INTERNAL ERROR: Invalid ParamCode detected! Value: " + this.ControlWord0.ParamCode.ToString());
        mbusDifVif2 = new MBusDifVif(MBusDifVif.DifVifOptions.DifSizeUnchangabel);
        if (this.ControlWord0.ParamCode == ParamCode.LogDate)
        {
          if (!mbusDifVif2.LoadDifVif(new byte[2]
          {
            (byte) 2,
            (byte) 108
          }))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal DIF VIF on parameter: " + this.Name, S3_Meter.S3_MeterLogger);
            return (byte[]) null;
          }
        }
        else if (this.ControlWord0.ParamCode == ParamCode.LogDateTime)
        {
          if (!mbusDifVif2.LoadDifVif(new byte[2]
          {
            (byte) 4,
            (byte) 109
          }))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal DIF VIF on parameter: " + this.Name, S3_Meter.S3_MeterLogger);
            return (byte[]) null;
          }
        }
        mbusDifVif2.TarifNumber = mbusDifVif1.TarifNumber;
      }
      mbusDifVif2.StorageNumber = !flag1 ? (!flag3 ? 32 : 200) : 8;
      if (flag2 && mbusDifVif2.DifByteSize < 3)
        mbusDifVif2.DifByteSize = 3;
      mbusDifVif2.Subunit = virtualDeviceNumber != (byte) 0 ? 0 : (int) loggerChanal.chanalParameter.Statics.VirtualDeviceNumber;
      return mbusDifVif2.DifVifArray;
    }

    internal string GetLoggerInfo()
    {
      int totalWidth = 15;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Name: ".PadRight(totalWidth) + this.Name);
      stringBuilder.AppendLine("Address: ".PadRight(totalWidth) + "0x" + this.BlockStartAddress.ToString("X4"));
      stringBuilder.AppendLine("ByteSize: ".PadRight(totalWidth) + this.ByteSize.ToString());
      stringBuilder.AppendLine("Dif/Vif: ".PadRight(totalWidth) + "0x" + Util.ByteArrayToHexString(this.VifDif.ToArray()));
      if (this.Iteration != null)
        stringBuilder.AppendLine("Cyclic Dif/Vif: ".PadRight(totalWidth) + "0x" + Util.ByteArrayToHexString(this.Iteration.VifDif.ToArray()));
      string zrMbusParameterId = MBusDevice.GetZR_MBusParameterID(this.VifDif.ToArray());
      if (string.IsNullOrEmpty(zrMbusParameterId))
        stringBuilder.AppendLine("ZDF: ".PadRight(totalWidth) + "!!! ERROR !!! Unknown DIF/VIF detected");
      else
        stringBuilder.AppendLine("ZDF: ".PadRight(totalWidth) + zrMbusParameterId);
      long valueIdent = TranslationRulesManager.GetValueIdent("ZRI", "HEAT", 136, zrMbusParameterId);
      if (valueIdent == -1L)
        stringBuilder.AppendLine("ValueIdent: ".PadRight(totalWidth));
      else
        stringBuilder.AppendLine("ValueIdent: ".PadRight(totalWidth) + ValueIdent.GetTranslatedValueNameForValueId(valueIdent, true));
      return stringBuilder.ToString();
    }

    internal void PrintParameter(StringBuilder printText, string startString)
    {
      if (this.IsLogger)
      {
        string str1 = this.ControlWord0.ParamCode.ToString();
        string str2 = (string) null;
        if (this.Iteration != null)
          str2 = (this.Iteration.ControlWord1.LoggerCycleCount + 1).ToString();
        LoggerChanal loggerChanal = this.MyMeter.MyLoggerManager.GetLoggerChanal(this.Name);
        if (loggerChanal.chanalParameter == null)
        {
          printText.Append(startString + this.Name);
        }
        else
        {
          string str3 = "'" + loggerChanal.chanalParameter.GetTranslatedParameterName() + "'";
          int num = 30 - str3.Length;
          while (num-- > 0)
            str3 += ".";
          printText.Append(startString + str3 + "  (" + str1 + ")");
          if (str2 != null)
            printText.Append("  (Cycles:" + str2 + ")");
        }
      }
      else
      {
        string str = "'" + S3_Parameter.GetTranslatedParameterNameByName(this.Name) + "'";
        int num = 30 - str.Length;
        while (num-- > 0)
          str += ".";
        printText.Append(startString + str);
      }
    }

    internal bool CreateIterator(byte virtualDeviceNumber)
    {
      this.Iteration = new MBusParameter(this.MyMeter, (S3_MemoryBlock) this);
      this.Iteration.ControlWord0 = new ControlWord0()
      {
        DataCount = this.ControlWord0.DataCount,
        ControlLogger = ControlLogger.LoggerNext,
        DifVifCount = 0,
        IsBCDByRadio = false,
        ItFollowsNextControlWord = true,
        ParamCode = ParamCode.LogValue
      };
      this.Iteration.ControlWord1 = new ControlWord1()
      {
        IsSwitch = false,
        LoggerCycleCount = 1,
        DecodeCommand = DecodeCommand.None,
        LoggerNextList = 0,
        ItFollowsNextControlWord = false
      };
      this.Iteration.Name = this.Name;
      this.Iteration.IsLogger = true;
      byte[] loggerDifVif = this.Iteration.GenerateLoggerDifVif(virtualDeviceNumber);
      if (loggerDifVif == null)
      {
        this.RemoveFromParentMemoryBlock();
        return false;
      }
      this.Iteration.VifDif = new List<byte>((IEnumerable<byte>) loggerDifVif);
      this.Iteration.ControlWord0.DifVifCount = loggerDifVif.Length;
      this.Iteration.ByteSize = 4 + this.Iteration.VifDif.Count;
      if (this.Iteration.ByteSize % 2 != 0)
        ++this.Iteration.ByteSize;
      return true;
    }

    internal void RecalculateByteSize()
    {
      int byteSize = this.ByteSize;
      int num = 0;
      if (this.ControlWord0 != null)
        num += 2;
      if (this.ControlWord1 != null)
        num += 2;
      if (this.ControlWord2 != null)
        num += 2;
      if (this.VifDif != null)
      {
        if (this.VifDif.Count % 2 == 0)
          num += this.VifDif.Count;
        else
          num += this.VifDif.Count + 1;
      }
      if (this.ControlWord0.ControlLogger == ControlLogger.LoggerDueDateReset || this.ControlWord0.ControlLogger == ControlLogger.LoggerReset || this.ControlWord0.ControlLogger == ControlLogger.LoggerChangeChanal || this.ControlWord0.ControlLogger == ControlLogger.LoggerNextChangeChanal)
        num += 2;
      this.ByteSize = num;
    }

    public void RemoveTarif1fromDIV(bool isLogger)
    {
      if (this.VifDif == null || this.VifDif.Count <= 0)
        return;
      MBusDifVif mbusDifVif = new MBusDifVif(MBusDifVif.DifVifOptions.DifSizeUnchangabel);
      mbusDifVif.LoadDifVif(this.VifDif.ToArray());
      if (mbusDifVif.TarifNumber == 1)
      {
        if (!isLogger && mbusDifVif.StorageNumber == 0)
        {
          mbusDifVif.DifSizeUnchangable = false;
          mbusDifVif.TarifNumber = 0;
          this.RecalculateByteSize();
        }
        else
          mbusDifVif.TarifNumber = 0;
        this.VifDif = new List<byte>((IEnumerable<byte>) mbusDifVif.DifVifArray);
      }
    }
  }
}
