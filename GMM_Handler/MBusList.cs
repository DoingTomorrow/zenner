// Decompiled with JetBrains decompiler
// Type: GMM_Handler.MBusList
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System;
using System.Collections;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  internal class MBusList : LinkBlock
  {
    internal SortedList AllMBusParameters;
    internal int MBusRuntimePointer;
    internal byte FullParameterListDataLength;
    internal byte ShortParameterListDataLength;
    internal ArrayList ShortListParameterNames;
    internal ArrayList FullListParameterNames;
    internal SortedList<string, MBusLoggerInfo> MBusLoggerInfos;
    public short ActiveLoggerFunctionNumber = -1;
    private bool MasksOk;
    private ushort DifVifCountMask;
    private ushort LengthCountMask;
    private ushort ParamCodeMask;
    private ushort EepromMask;
    private ushort LoggerMask;
    private ushort LoggerLastMask;
    private ushort RuntimeMask;
    private ushort MinListMask;
    private ushort DateMask;
    private ushort DateTimeMask;

    internal MBusList(Meter MyMeterIn)
      : base(MyMeterIn, LinkBlockTypes.MBusList)
    {
    }

    internal MBusList Clone(Meter NewMeter)
    {
      MBusList mbusList = new MBusList(NewMeter);
      mbusList.MBusRuntimePointer = this.MBusRuntimePointer;
      mbusList.FullParameterListDataLength = this.FullParameterListDataLength;
      mbusList.ShortParameterListDataLength = this.ShortParameterListDataLength;
      mbusList.ShortListParameterNames = (ArrayList) this.ShortListParameterNames.Clone();
      mbusList.FullListParameterNames = (ArrayList) this.FullListParameterNames.Clone();
      mbusList.MBusLoggerInfos = new SortedList<string, MBusLoggerInfo>();
      for (int index = 0; index < this.MBusLoggerInfos.Count; ++index)
        mbusList.MBusLoggerInfos.Add(this.MBusLoggerInfos.Keys[index], this.MBusLoggerInfos.Values[index].Clone());
      mbusList.ActiveLoggerFunctionNumber = this.ActiveLoggerFunctionNumber;
      mbusList.MasksOk = this.MasksOk;
      mbusList.DifVifCountMask = this.DifVifCountMask;
      mbusList.LengthCountMask = this.LengthCountMask;
      mbusList.ParamCodeMask = this.ParamCodeMask;
      mbusList.EepromMask = this.EepromMask;
      mbusList.LoggerMask = this.LoggerMask;
      mbusList.LoggerLastMask = this.LoggerLastMask;
      mbusList.RuntimeMask = this.RuntimeMask;
      mbusList.MinListMask = this.MinListMask;
      mbusList.DateMask = this.DateMask;
      mbusList.DateTimeMask = this.DateTimeMask;
      return mbusList;
    }

    internal bool LoadFromByteArray(byte[] TheArray, int Offset)
    {
      if (!this.LoadAllLogger() || !this.LoadTypeMasks())
        return false;
      for (int index = 0; index < this.MyMeter.AllParameters.Count; ++index)
      {
        Parameter byIndex = (Parameter) this.MyMeter.AllParameters.GetByIndex(index);
        byIndex.MBusOn = false;
        byIndex.MBusShortOn = false;
      }
      this.ShortListParameterNames = new ArrayList();
      this.FullListParameterNames = new ArrayList();
      CodeBlock codeBlock = new CodeBlock(CodeBlock.CodeSequenceTypes.LinkerGeneratedCodeBlock, FrameTypes.None, -1);
      this.MBusRuntimePointer = (int) ParameterService.GetFromByteArray_ushort(TheArray, ref Offset);
      codeBlock.CodeList.Add((object) CodeObject.GetCodeObject("Runtime Pointer", (ushort) this.MBusRuntimePointer));
      this.FullParameterListDataLength = TheArray[Offset++];
      this.ShortParameterListDataLength = TheArray[Offset++];
      codeBlock.CodeList.Add((object) CodeObject.GetCodeObject("Full len", this.FullParameterListDataLength));
      codeBlock.CodeList.Add((object) CodeObject.GetCodeObject("Short len", this.ShortParameterListDataLength));
      bool flag = false;
      ushort fromByteArrayUshort1;
      while ((fromByteArrayUshort1 = ParameterService.GetFromByteArray_ushort(TheArray, ref Offset)) > (ushort) 0)
      {
        codeBlock.CodeList.Add((object) CodeObject.GetCodeObject("Control word", fromByteArrayUshort1));
        int valueFromMask1 = (int) this.GetValueFromMask(fromByteArrayUshort1, this.LengthCountMask);
        bool boolFromMask1 = this.GetBoolFromMask(fromByteArrayUshort1, this.EepromMask);
        bool boolFromMask2 = this.GetBoolFromMask(fromByteArrayUshort1, this.LoggerMask);
        bool boolFromMask3 = this.GetBoolFromMask(fromByteArrayUshort1, this.LoggerLastMask);
        this.GetBoolFromMask(fromByteArrayUshort1, this.RuntimeMask);
        bool boolFromMask4 = this.GetBoolFromMask(fromByteArrayUshort1, this.MinListMask);
        this.GetBoolFromMask(fromByteArrayUshort1, this.DateMask);
        this.GetBoolFromMask(fromByteArrayUshort1, this.DateTimeMask);
        if (boolFromMask2)
        {
          if (!flag)
          {
            ushort fromByteArrayUshort2 = ParameterService.GetFromByteArray_ushort(TheArray, ref Offset);
            ushort functionNumber;
            try
            {
              functionNumber = (ushort) ((Parameter) this.MyMeter.AllEpromParametersByAddress[(object) (int) fromByteArrayUshort2]).FunctionNumber;
            }
            catch
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "MBus logger function not found");
              return false;
            }
            for (ushort index = 0; (int) index < this.MBusLoggerInfos.Count; ++index)
            {
              MBusLoggerInfo mbusLoggerInfo = this.MBusLoggerInfos.Values[(int) index];
              if ((int) mbusLoggerInfo.FunctionNumber == (int) functionNumber)
              {
                if (boolFromMask4)
                  this.ShortListParameterNames.Add((object) mbusLoggerInfo.LoggerFunctionName);
                else
                  this.FullListParameterNames.Add((object) mbusLoggerInfo.LoggerFunctionName);
                mbusLoggerInfo.LoggerNumberOfEntrys = TheArray[Offset++];
                Parameter parameter = (Parameter) this.MyMeter.AllEpromParametersByAddress[(object) (int) fromByteArrayUshort2];
                CodeObject codeObject = CodeObject.GetCodeObject("Logger Pointer", fromByteArrayUshort2);
                codeObject.CodeValue = parameter.FullName;
                codeObject.CodeType = CodeObject.CodeTypes.ePTR;
                codeBlock.CodeList.Add((object) codeObject);
                codeBlock.CodeList.Add((object) CodeObject.GetCodeObject("Repeat number", mbusLoggerInfo.LoggerNumberOfEntrys));
                flag = true;
                goto label_20;
              }
            }
            throw new ArgumentOutOfRangeException("MBus-Logger-Function not found");
          }
label_20:
          if (boolFromMask3)
            flag = false;
        }
        else
        {
          int fromByteArrayUshort3 = (int) ParameterService.GetFromByteArray_ushort(TheArray, ref Offset);
          Parameter TheParameter = !boolFromMask1 ? (Parameter) this.MyMeter.AllRamParametersByAddress[(object) fromByteArrayUshort3] : (Parameter) this.MyMeter.AllEpromParametersByAddress[(object) fromByteArrayUshort3];
          if (TheParameter == null)
          {
            codeBlock.CodeList.RemoveAt(codeBlock.CodeList.Count - 1);
            break;
          }
          if (boolFromMask4)
          {
            this.ShortListParameterNames.Add((object) TheParameter.FullName);
            TheParameter.MBusShortOn = true;
            TheParameter.MBusOn = true;
          }
          else
          {
            this.FullListParameterNames.Add((object) TheParameter.FullName);
            TheParameter.MBusOn = true;
          }
          CodeObject pointerCodeObject = this.GetPointerCodeObject(TheParameter);
          if (pointerCodeObject == null)
            return false;
          pointerCodeObject.CodeValueCompiled = (long) fromByteArrayUshort3;
          this.MyMeter.MyCompiler.GenerateCodeFromCodeObject(pointerCodeObject);
          codeBlock.CodeList.Add((object) pointerCodeObject);
        }
        ushort valueFromMask2 = this.GetValueFromMask(fromByteArrayUshort1, this.DifVifCountMask);
        long fromByteArrayUlong = (long) ParameterService.GetFromByteArray_ulong(TheArray, valueFromMask2, ref Offset);
        codeBlock.CodeList.Add((object) CodeObject.GetCodeObject("Dif Vif", (ulong) fromByteArrayUlong, (int) valueFromMask2));
      }
      codeBlock.CodeList.Add((object) CodeObject.GetCodeObject("MBus end", (ushort) 0));
      this.LinkObjList.Add((object) codeBlock);
      return true;
    }

    internal bool GenerateNewList()
    {
      if (!this.LoadAllLogger() || !this.LoadTypeMasks())
        return false;
      CodeBlock TheCodeBlock = new CodeBlock(CodeBlock.CodeSequenceTypes.LinkerGeneratedCodeBlock, FrameTypes.None, -1);
      this.MBusRuntimePointer = 0;
      if (this.MyMeter.MyLinker.MBus_Runtime.Count > 0)
        this.MBusRuntimePointer = ((LinkObj) ((CodeBlock) this.MyMeter.MyLinker.MBus_Runtime[0]).CodeList[0]).Address;
      TheCodeBlock.CodeList.Add((object) CodeObject.GetCodeObject("Runtime Pointer", (ushort) this.MBusRuntimePointer));
      this.ShortParameterListDataLength = (byte) 0;
      this.FullParameterListDataLength = (byte) 0;
      TheCodeBlock.CodeList.Add((object) CodeObject.GetCodeObject("Full len", this.FullParameterListDataLength));
      TheCodeBlock.CodeList.Add((object) CodeObject.GetCodeObject("Short len", this.ShortParameterListDataLength));
      for (int index = 0; index < this.MyMeter.AllParameters.Count; ++index)
        ((Parameter) this.MyMeter.AllParameters.GetByIndex(index)).MBusOn = false;
      for (int index1 = 0; index1 < this.ShortListParameterNames.Count; ++index1)
      {
        int index2 = this.AllMBusParameters.IndexOfKey((object) (string) this.ShortListParameterNames[index1]);
        if (index2 >= 0)
        {
          Parameter byIndex = (Parameter) this.AllMBusParameters.GetByIndex(index2);
          if (byIndex == null || byIndex.DifVifSize == (short) 0)
          {
            this.ShortListParameterNames.RemoveAt(index1);
            --index1;
          }
          else
          {
            this.AdjustDifVifCode(byIndex);
            byIndex.MBusOn = true;
            byIndex.MBusShortOn = true;
            if (!this.AddParameterToMBusList(TheCodeBlock, byIndex))
              return false;
          }
        }
        else if (!this.AddLoggerToMBusList((string) this.ShortListParameterNames[index1], TheCodeBlock, true))
          return false;
      }
      this.FullParameterListDataLength = this.ShortParameterListDataLength;
      for (int index3 = 0; index3 < this.FullListParameterNames.Count; ++index3)
      {
        int index4 = this.AllMBusParameters.IndexOfKey((object) (string) this.FullListParameterNames[index3]);
        if (index4 >= 0)
        {
          Parameter byIndex = (Parameter) this.AllMBusParameters.GetByIndex(index4);
          if (byIndex == null || byIndex.DifVifSize == (short) 0)
          {
            this.ShortListParameterNames.RemoveAt(index3);
            --index3;
          }
          else
          {
            this.AdjustDifVifCode(byIndex);
            byIndex.MBusOn = true;
            byIndex.MBusShortOn = false;
            if (!this.AddParameterToMBusList(TheCodeBlock, byIndex))
              return false;
          }
        }
        else if (!this.AddLoggerToMBusList((string) this.FullListParameterNames[index3], TheCodeBlock, false))
          return false;
      }
      ((CodeObject) TheCodeBlock.CodeList[1]).CodeValueCompiled = (long) this.FullParameterListDataLength;
      ((CodeObject) TheCodeBlock.CodeList[2]).CodeValueCompiled = (long) this.ShortParameterListDataLength;
      TheCodeBlock.CodeList.Add((object) CodeObject.GetCodeObject("MBus end", (ushort) 0));
      this.LinkObjList.Add((object) TheCodeBlock);
      return this.MyMeter.MyCompiler.GenerateCodeFromCodeBlockList(this.LinkObjList);
    }

    private bool AddParameterToMBusList(CodeBlock TheCodeBlock, Parameter TheParameter)
    {
      ushort wordFromParameter = this.GetControlWordFromParameter(TheParameter);
      byte mbusParameterLength = (byte) TheParameter.MBusParameterLength;
      TheCodeBlock.CodeList.Add((object) CodeObject.GetCodeObject("Control word", wordFromParameter));
      CodeObject pointerCodeObject = this.GetPointerCodeObject(TheParameter);
      if (pointerCodeObject == null)
        return false;
      TheCodeBlock.CodeList.Add((object) pointerCodeObject);
      this.MyMeter.MyCompiler.AddPointer(pointerCodeObject);
      TheCodeBlock.CodeList.Add((object) CodeObject.GetCodeObject("Dif Vif", (ulong) TheParameter.DifVifs, (int) TheParameter.DifVifSize));
      byte num = (byte) ((uint) mbusParameterLength + (uint) (byte) TheParameter.DifVifSize);
      if (TheParameter.MBusShortOn)
        this.ShortParameterListDataLength += num;
      else
        this.FullParameterListDataLength += num;
      return true;
    }

    private bool AddLoggerToMBusList(
      string LoggerFunctionName,
      CodeBlock TheCodeBlock,
      bool ShortList)
    {
      byte num1 = 0;
      if (!this.MBusLoggerInfos.ContainsKey(LoggerFunctionName))
        return true;
      MBusLoggerInfo mbusLoggerInfo = this.MBusLoggerInfos[LoggerFunctionName];
      Function function = (Function) this.MyMeter.MyFunctionTable.FunctionListByNumber[(object) (ushort) mbusLoggerInfo.FunctionNumber];
      if (function == null)
      {
        this.MBusLoggerInfos.Remove(LoggerFunctionName);
        return true;
      }
      CodeObject codeObject = CodeObject.GetCodeObject("Logger Pointer", (ushort) 0);
      codeObject.CodeType = CodeObject.CodeTypes.ePTR;
      ArrayList arrayList = new ArrayList();
      foreach (Parameter parameter in function.ParameterList)
      {
        if (parameter.Name.EndsWith("_2S"))
          codeObject.CodeValue = parameter.FullName;
        if (parameter.BlockMark == LinkBlockTypes.LoggerStore)
          arrayList.Add((object) parameter);
      }
      if (arrayList.Count < 1 || codeObject.CodeValue.Length < 1)
        return false;
      for (int index = 0; index < arrayList.Count; ++index)
      {
        Parameter TheParameter = (Parameter) arrayList[index];
        this.AdjustDifVifCode(TheParameter);
        ushort wordFromParameter = this.GetControlWordFromParameter(TheParameter);
        this.SetBoolWithMask(ref wordFromParameter, true, this.LoggerMask);
        this.SetBoolWithMask(ref wordFromParameter, ShortList, this.MinListMask);
        if (index == arrayList.Count - 1)
          this.SetBoolWithMask(ref wordFromParameter, true, this.LoggerLastMask);
        byte num2 = (byte) ((uint) num1 + (uint) (byte) ((uint) TheParameter.MBusParameterLength * (uint) mbusLoggerInfo.LoggerNumberOfEntrys));
        TheCodeBlock.CodeList.Add((object) CodeObject.GetCodeObject("Control word", wordFromParameter));
        if (index == 0)
        {
          TheCodeBlock.CodeList.Add((object) codeObject);
          this.MyMeter.MyCompiler.AddPointer(codeObject);
          TheCodeBlock.CodeList.Add((object) CodeObject.GetCodeObject("Repeat number", mbusLoggerInfo.LoggerNumberOfEntrys));
        }
        TheCodeBlock.CodeList.Add((object) CodeObject.GetCodeObject("Dif Vif", (ulong) TheParameter.DifVifs, (int) TheParameter.DifVifSize));
        num1 = (byte) ((uint) num2 + (uint) (byte) ((uint) TheParameter.DifVifSize * (uint) mbusLoggerInfo.LoggerNumberOfEntrys));
      }
      if (ShortList)
        this.ShortParameterListDataLength += num1;
      else
        this.FullParameterListDataLength += num1;
      return true;
    }

    private CodeObject GetPointerCodeObject(Parameter TheParameter)
    {
      CodeObject pointerCodeObject;
      if (TheParameter.ExistOnCPU)
      {
        pointerCodeObject = CodeObject.GetPointerCodeObject("CPU Parameter address: " + TheParameter.FullName, TheParameter.FullName);
        pointerCodeObject.CodeType = CodeObject.CodeTypes.iPTR;
      }
      else if (TheParameter.ExistOnEprom)
      {
        pointerCodeObject = CodeObject.GetPointerCodeObject("Eprom Parameter address: " + TheParameter.FullName, TheParameter.FullName);
        pointerCodeObject.CodeType = CodeObject.CodeTypes.ePTR;
      }
      else
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "MBus parameter code object not found");
        return (CodeObject) null;
      }
      return pointerCodeObject;
    }

    private ushort GetControlWordFromParameter(Parameter TheParameter)
    {
      ushort ChangedValue = 0;
      this.SetValueWithMask(ref ChangedValue, (ushort) TheParameter.Size, this.LengthCountMask);
      this.SetValueWithMask(ref ChangedValue, (ushort) TheParameter.DifVifSize, this.DifVifCountMask);
      this.SetBoolWithMask(ref ChangedValue, !TheParameter.ExistOnCPU, this.EepromMask);
      this.SetBoolWithMask(ref ChangedValue, TheParameter.MBusShortOn, this.MinListMask);
      this.SetBoolWithMask(ref ChangedValue, TheParameter.MBusParamConvertion == Parameter.MBusParameterConversion.Date, this.DateMask);
      this.SetBoolWithMask(ref ChangedValue, TheParameter.MBusParamConvertion == Parameter.MBusParameterConversion.DateTime, this.DateTimeMask);
      return ChangedValue;
    }

    private ushort GetValueFromMask(ushort Control, ushort Mask)
    {
      if (Mask == (ushort) 0)
        return 0;
      for (; ((int) Mask & 1) == 0; Mask >>= 1)
        Control >>= 1;
      return (ushort) ((uint) Control & (uint) Mask);
    }

    private bool GetBoolFromMask(ushort Control, ushort Mask) => ((int) Control & (int) Mask) != 0;

    private void SetValueWithMask(ref ushort ChangedValue, ushort TheValue, ushort Mask)
    {
      for (ushort index = 1; ((int) Mask & (int) index) == 0; index <<= 1)
        TheValue <<= 1;
      ChangedValue += (ushort) ((uint) TheValue & (uint) Mask);
    }

    private void SetBoolWithMask(ref ushort ChangedValue, bool TheValue, ushort Mask)
    {
      if (!TheValue)
        return;
      ChangedValue |= Mask;
    }

    private bool LoadTypeMasks()
    {
      if (!this.MasksOk)
      {
        this.DifVifCountMask = (ushort) (int) this.MyMeter.MyCompiler.Includes[(object) "MBU_PARA_CONTROL_DIFVIF_COUNT"];
        this.LengthCountMask = (ushort) (int) this.MyMeter.MyCompiler.Includes[(object) "MBU_PARA_CONTROL_LENGTH_COUNT"];
        this.ParamCodeMask = (ushort) (int) this.MyMeter.MyCompiler.Includes[(object) "MBU_PARA_CONTROL_PARAM_CODE"];
        this.EepromMask = (ushort) (int) this.MyMeter.MyCompiler.Includes[(object) "MBU_PARA_CONTROL_EEPROM_PTR"];
        this.LoggerMask = (ushort) (int) this.MyMeter.MyCompiler.Includes[(object) "MBU_PARA_CONTROL_LOGGER"];
        this.LoggerLastMask = (ushort) (int) this.MyMeter.MyCompiler.Includes[(object) "MBU_PARA_CONTROL_LOGGER_LAST"];
        this.RuntimeMask = (ushort) (int) this.MyMeter.MyCompiler.Includes[(object) "MBU_PARA_CONTROL_RUNTIME"];
        this.MinListMask = (ushort) (int) this.MyMeter.MyCompiler.Includes[(object) "MBU_PARA_CONTROL_MIN_LIST"];
        this.DateMask = (ushort) (int) this.MyMeter.MyCompiler.Includes[(object) "MBU_PARA_CONTROL_DATE"];
        this.DateTimeMask = (ushort) (int) this.MyMeter.MyCompiler.Includes[(object) "MBU_PARA_CONTROL_DATE_TIME"];
        this.MasksOk = true;
      }
      return true;
    }

    private bool LoadAllLogger()
    {
      SortedList<string, MBusLoggerInfo> sortedList = (SortedList<string, MBusLoggerInfo>) null;
      if (this.MBusLoggerInfos != null)
        sortedList = this.MBusLoggerInfos;
      this.MBusLoggerInfos = new SortedList<string, MBusLoggerInfo>();
      foreach (Function function in this.MyMeter.MyFunctionTable.FunctionList)
      {
        int index = -1;
        if (sortedList != null)
          index = sortedList.IndexOfKey(function.FullName);
        if (index >= 0)
        {
          this.MBusLoggerInfos.Add(function.FullName, sortedList.Values[index]);
        }
        else
        {
          foreach (CodeBlock runtimeCodeBlock in function.RuntimeCodeBlockList)
          {
            if (runtimeCodeBlock is IntervalAndLogger)
            {
              IntervalAndLogger intervalAndLogger = runtimeCodeBlock as IntervalAndLogger;
              if (intervalAndLogger.MaxEntries > 0 && intervalAndLogger.MBusParameterLength > 0)
              {
                MBusLoggerInfo mbusLoggerInfo = new MBusLoggerInfo();
                mbusLoggerInfo.FunctionNumber = (short) function.Number;
                mbusLoggerInfo.LoggerFunctionName = function.FullName;
                mbusLoggerInfo.LoggerNumberOfEntrys = intervalAndLogger.MBusMaxEntries;
                mbusLoggerInfo.LoggerMaxNumberOfEntrys = intervalAndLogger.MaxEntries;
                mbusLoggerInfo.MBusParameterLength = intervalAndLogger.MBusParameterLength;
                mbusLoggerInfo.LoggerDifVifBytesPerEntry = intervalAndLogger.DifVifEntrySize;
                mbusLoggerInfo.LoggerBytesPerTransmit = 0;
                foreach (Parameter parameter in function.ParameterList)
                {
                  if (parameter.BlockMark == LinkBlockTypes.LoggerStore)
                    mbusLoggerInfo.LoggerBytesPerTransmit += parameter.MBusParameterLength;
                }
                this.MBusLoggerInfos.Add(function.FullName, mbusLoggerInfo);
              }
            }
          }
        }
      }
      return true;
    }

    internal bool AdjustAllMBusParameterDivVifs()
    {
      foreach (Parameter TheParameter in (IEnumerable) this.AllMBusParameters.Values)
        this.AdjustDifVifCode(TheParameter);
      return true;
    }

    internal void AdjustDifVifCode(Parameter TheParameter)
    {
      if ((TheParameter.DifVifs & 12416L) == 4224L && this.MyMeter.MyMath.MyBaseSettings.BaseConfig[0] == 'F')
        TheParameter.DifVifs &= -4097L;
      if (TheParameter.DifVifsByRes != null)
      {
        int index1;
        for (index1 = TheParameter.DifVifsByRes.Count - 1; index1 >= 0; --index1)
        {
          string key = (string) TheParameter.DifVifsByRes.GetKey(index1);
          if (!(key != string.Empty) || this.MyMeter.IsMeterResourceAvailable((MeterResources) Enum.Parse(typeof (MeterResources), key, true)))
            break;
        }
        byte[] byIndex = (byte[]) TheParameter.DifVifsByRes.GetByIndex(index1);
        TheParameter.DifVifSize = (short) byIndex.Length;
        TheParameter.DifVifs = 0L;
        for (int index2 = 0; index2 < byIndex.Length; ++index2)
          TheParameter.DifVifs += (long) byIndex[index2] << 8 * index2;
      }
      long num1;
      switch (TheParameter.MBusParameterOverride)
      {
        case Parameter.MBusParameterOverrideType.Energy:
          num1 = (long) this.MyMeter.MyMath.MyBaseSettings.MBusEnergieVIF;
          break;
        case Parameter.MBusParameterOverrideType.Volume:
          num1 = (long) this.MyMeter.MyMath.MyBaseSettings.MBusVolumeVIF;
          break;
        case Parameter.MBusParameterOverrideType.Flow:
          num1 = (long) this.MyMeter.MyMath.MyBaseSettings.MBusFlowVIF;
          break;
        case Parameter.MBusParameterOverrideType.Power:
          num1 = (long) this.MyMeter.MyMath.MyBaseSettings.MBusPowerVIF;
          break;
        case Parameter.MBusParameterOverrideType.INPUT_1:
          num1 = (long) this.MyMeter.MyMath.MyBaseSettings.MBusInput1VIF;
          break;
        case Parameter.MBusParameterOverrideType.INPUT_2:
          num1 = (long) this.MyMeter.MyMath.MyBaseSettings.MBusInput2VIF;
          break;
        default:
          return;
      }
      if (num1 < 0L)
      {
        TheParameter.MBusOn = false;
        TheParameter.MBusShortOn = false;
        ZR_ClassLibMessages.AddWarning("!! MBus VIF not available for parameter: " + TheParameter.FullName);
      }
      else
      {
        long num2 = num1 << 8;
        long num3 = 65280;
        for (long index = 128; (TheParameter.DifVifs & index) > 0L; index <<= 8)
        {
          num2 <<= 8;
          num3 <<= 8;
        }
        TheParameter.DifVifs = TheParameter.DifVifs & ~num3 | num2;
      }
    }

    public bool GetMBusVariableLists(out MBusInfo TheInfo)
    {
      TheInfo = new MBusInfo();
      TheInfo.AllParametersWithLength = new SortedList();
      TheInfo.ShortListParameterNames = new ArrayList();
      TheInfo.FullListParameterNames = new ArrayList();
      for (int index1 = 0; index1 < this.AllMBusParameters.Count; ++index1)
      {
        Parameter byIndex = (Parameter) this.AllMBusParameters.GetByIndex(index1);
        MBusParameterInfo mbusParameterInfo = new MBusParameterInfo();
        byte[] numArray = new byte[(int) byIndex.DifVifSize];
        for (int index2 = 0; index2 < (int) byIndex.DifVifSize; ++index2)
          numArray[index2] = (byte) (byIndex.DifVifs >> 8 * index2);
        mbusParameterInfo.DifVifs = numArray;
        mbusParameterInfo.ParameterInfo = byIndex.ParameterInfo;
        mbusParameterInfo.BytesPerFrame = (int) byIndex.DifVifSize + byIndex.MBusParameterLength;
        TheInfo.AllParametersWithLength.Add((object) byIndex.NameTranslated, (object) mbusParameterInfo);
      }
      for (short index3 = 0; (int) index3 < this.MBusLoggerInfos.Count; ++index3)
      {
        MBusLoggerInfo mbusLoggerInfo = this.MBusLoggerInfos.Values[(int) index3];
        MBusParameterInfo mbusParameterInfo = new MBusParameterInfo();
        mbusParameterInfo.BytesPerFrame = mbusLoggerInfo.LoggerBytesPerTransmit + mbusLoggerInfo.LoggerDifVifBytesPerEntry;
        mbusParameterInfo.ParameterInfo = mbusLoggerInfo.LoggerFunctionName;
        mbusParameterInfo.LoggerMaxEntries = mbusLoggerInfo.LoggerMaxNumberOfEntrys;
        mbusParameterInfo.LoggerEntries = (int) mbusLoggerInfo.LoggerNumberOfEntrys;
        if (mbusParameterInfo.LoggerEntries == 0)
          mbusParameterInfo.LoggerEntries = 1;
        if (this.MyMeter.MyFunctionTable.FunctionListByNumber.ContainsKey((object) (ushort) mbusLoggerInfo.FunctionNumber))
        {
          Function function = (Function) this.MyMeter.MyFunctionTable.FunctionListByNumber[(object) (ushort) mbusLoggerInfo.FunctionNumber];
          List<byte> byteList = new List<byte>(100);
          mbusParameterInfo.DifVifs = new byte[100];
          foreach (Parameter parameter in function.ParameterList)
          {
            if (parameter.BlockMark == LinkBlockTypes.LoggerStore)
            {
              long difVifs = parameter.DifVifs;
              for (int index4 = 0; index4 < (int) parameter.DifVifSize; ++index4)
              {
                byteList.Add((byte) difVifs);
                difVifs >>= 8;
              }
            }
          }
          mbusParameterInfo.DifVifs = byteList.ToArray();
          TheInfo.AllParametersWithLength.Add((object) mbusLoggerInfo.LoggerFunctionName, (object) mbusParameterInfo);
        }
      }
      foreach (string listParameterName in this.ShortListParameterNames)
      {
        Parameter allMbusParameter = (Parameter) this.AllMBusParameters[(object) listParameterName];
        string key;
        if (allMbusParameter == null)
        {
          if (this.MBusLoggerInfos.ContainsKey(listParameterName))
            key = listParameterName;
          else
            continue;
        }
        else
          key = allMbusParameter.NameTranslated;
        if (TheInfo.AllParametersWithLength.IndexOfKey((object) key) >= 0)
          TheInfo.ShortListParameterNames.Add((object) key);
      }
      foreach (string listParameterName in this.FullListParameterNames)
      {
        Parameter allMbusParameter = (Parameter) this.AllMBusParameters[(object) listParameterName];
        string key;
        if (allMbusParameter == null)
        {
          if (this.MBusLoggerInfos.ContainsKey(listParameterName))
            key = listParameterName;
          else
            continue;
        }
        else
          key = allMbusParameter.NameTranslated;
        if (TheInfo.AllParametersWithLength.IndexOfKey((object) key) >= 0)
          TheInfo.FullListParameterNames.Add((object) key);
      }
      return true;
    }

    internal bool SetMBusVariables(MBusInfo TheInfo)
    {
      if (!this.LoadAllLogger())
        return false;
      foreach (MBusParameterInfo mbusParameterInfo in (IEnumerable) TheInfo.AllParametersWithLength.Values)
      {
        if (mbusParameterInfo.LoggerMaxEntries > 0)
        {
          string parameterInfo = mbusParameterInfo.ParameterInfo;
          if (this.MBusLoggerInfos.ContainsKey(parameterInfo))
            this.MBusLoggerInfos[parameterInfo].LoggerNumberOfEntrys = (byte) mbusParameterInfo.LoggerEntries;
        }
      }
      this.ShortListParameterNames = new ArrayList();
      foreach (string listParameterName in TheInfo.ShortListParameterNames)
      {
        for (int index = 0; index < this.AllMBusParameters.Count; ++index)
        {
          Parameter byIndex = (Parameter) this.AllMBusParameters.GetByIndex(index);
          if (byIndex.NameTranslated == listParameterName)
          {
            this.ShortListParameterNames.Add((object) byIndex.FullName);
            goto label_22;
          }
        }
        int index1 = this.MBusLoggerInfos.IndexOfKey(listParameterName);
        if (index1 >= 0)
          this.ShortListParameterNames.Add((object) this.MBusLoggerInfos.Values[index1].LoggerFunctionName);
label_22:;
      }
      this.FullListParameterNames = new ArrayList();
      foreach (string listParameterName in TheInfo.FullListParameterNames)
      {
        for (int index = 0; index < this.AllMBusParameters.Count; ++index)
        {
          Parameter byIndex = (Parameter) this.AllMBusParameters.GetByIndex(index);
          if (byIndex.NameTranslated == listParameterName)
          {
            this.FullListParameterNames.Add((object) byIndex.FullName);
            goto label_36;
          }
        }
        int index2 = this.MBusLoggerInfos.IndexOfKey(listParameterName);
        if (index2 >= 0)
          this.FullListParameterNames.Add((object) this.MBusLoggerInfos.Values[index2].LoggerFunctionName);
label_36:;
      }
      return this.GenerateNewList();
    }
  }
}
