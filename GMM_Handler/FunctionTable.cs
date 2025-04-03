// Decompiled with JetBrains decompiler
// Type: GMM_Handler.FunctionTable
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System;
using System.Collections;
using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  internal class FunctionTable : LinkBlock
  {
    internal const int TableHeaderSize = 6;
    internal ushort Checksum;
    internal int Version;
    internal int MenuColumns;
    internal int TableLen;
    internal byte[] DummyBytes;
    internal SortedList OverridesList;
    internal ArrayList FunctionNumbersList;
    internal ArrayList FunctionStartIndexOfMenuColumnList;
    internal ArrayList FunctionList;
    internal SortedList FunctionListByName;
    internal SortedList FunctionListByNumber;
    internal ArrayList FirstFunctionInColumn;

    internal FunctionTable(Meter MyMeterIn)
      : base(MyMeterIn, LinkBlockTypes.FunctionTable)
    {
    }

    internal FunctionTable Clone(Meter NewMeter)
    {
      FunctionTable functionTable = new FunctionTable(NewMeter);
      functionTable.Version = this.Version;
      functionTable.MenuColumns = this.MenuColumns;
      functionTable.DummyBytes = (byte[]) this.DummyBytes.Clone();
      functionTable.OverridesList = OverrideParameter.GetOverridesListClone(this.OverridesList);
      functionTable.FunctionNumbersList = new ArrayList();
      foreach (ushort functionNumbers in this.FunctionNumbersList)
        functionTable.FunctionNumbersList.Add((object) functionNumbers);
      functionTable.FunctionStartIndexOfMenuColumnList = new ArrayList();
      foreach (short indexOfMenuColumn in this.FunctionStartIndexOfMenuColumnList)
        functionTable.FunctionStartIndexOfMenuColumnList.Add((object) indexOfMenuColumn);
      return functionTable;
    }

    internal bool ReadFunctionTableFromConnectedDevice()
    {
      this.LinkObjList.Add((object) new CodeBlock(CodeBlock.CodeSequenceTypes.LinkerGeneratedCodeBlock, FrameTypes.None, -1));
      int valueEprom = (int) ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_FunctionTableAdr"]).ValueEprom;
      if (!this.LoadTableHeaderFromArray(this.MyMeter.Eprom, valueEprom))
        return false;
      int StartAddress = valueEprom + 6;
      int num = this.TableLen - 6;
      ByteField MemoryData = new ByteField(num);
      if (!this.MyMeter.MyCommunication.MyBus.ReadMemory(MemoryLocation.EEPROM, StartAddress, num, out MemoryData))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Read function table error");
        return false;
      }
      for (int index = 0; index < num; ++index)
        this.MyMeter.Eprom[StartAddress + index] = MemoryData.Data[index];
      if ((int) this.MyMeter.MyEpromHeader.GenerateChecksum(this.MyMeter.Eprom, valueEprom + 2, this.TableLen - 2, (ushort) 0) != (int) this.MyMeter.Eprom[valueEprom] + ((int) this.MyMeter.Eprom[valueEprom + 1] << 8))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Function table checksum error");
        return false;
      }
      if (!this.LoadFromByteArray(this.MyMeter.Eprom, valueEprom))
        return false;
      this.MyMeter.MyCompiler.GenerateCodeFromCodeBlockList(this.LinkObjList);
      return true;
    }

    internal bool ReadFunctionTableFromEprom()
    {
      this.LinkObjList.Add((object) new CodeBlock(CodeBlock.CodeSequenceTypes.LinkerGeneratedCodeBlock, FrameTypes.None, -1));
      int valueEprom = (int) ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_FunctionTableAdr"]).ValueEprom;
      if (!this.LoadTableHeaderFromArray(this.MyMeter.Eprom, valueEprom) || !this.LoadFromByteArray(this.MyMeter.Eprom, valueEprom))
        return false;
      this.MyMeter.MyCompiler.GenerateCodeFromCodeBlockList(this.LinkObjList);
      return true;
    }

    private bool LoadTableHeaderFromArray(byte[] TheArray, int Offset)
    {
      if (TheArray.Length < Offset + 6)
        return false;
      this.Checksum = ParameterService.GetFromByteArray_ushort(TheArray, ref Offset);
      this.Version = (int) TheArray[Offset++];
      this.MenuColumns = (int) TheArray[Offset++];
      this.TableLen = (int) ParameterService.GetFromByteArray_ushort(TheArray, ref Offset);
      ArrayList codeList = ((CodeBlock) this.LinkObjList[0]).CodeList;
      codeList.Add((object) CodeObject.GetCodeObject("Checksum", this.Checksum));
      codeList.Add((object) CodeObject.GetCodeObject("Version", (byte) this.Version));
      codeList.Add((object) CodeObject.GetCodeObject("Menu columns", (byte) this.MenuColumns));
      codeList.Add((object) CodeObject.GetCodeObject("Table len", (ushort) this.TableLen));
      return true;
    }

    internal bool LoadFromByteArray(byte[] TheArray, int Offset)
    {
      ArrayList codeList = ((CodeBlock) this.LinkObjList[0]).CodeList;
      int num1 = Offset + 6;
      if (TheArray.Length >= Offset + this.TableLen)
      {
        if ((int) this.MyMeter.MyEpromHeader.GenerateChecksum(TheArray, Offset + 2, this.TableLen - 2, (ushort) 0) != (int) this.Checksum)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Function table checksum error");
        }
        else
        {
          this.OverridesList = OverrideParameter.GetNewOverridesList();
          if (this.Version > 2)
          {
            int the = (int) TheArray[num1++];
            codeList.Add((object) CodeObject.GetCodeObject("Number of override parameters", (byte) the));
            for (int index = 0; index < the; ++index)
            {
              OverrideParameter TheOverrideParameter = new OverrideParameter((OverrideID) TheArray[num1++]);
              if (!this.MyMeter.MyHandler.UseOnlyDefaultValues)
              {
                if (TheOverrideParameter.LoadDataFromByteArray(TheArray, ref num1))
                  OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, TheOverrideParameter);
                else
                  goto label_45;
              }
              else
                num1 += (int) TheOverrideParameter.ByteSize;
              string str = TheOverrideParameter.ParameterID.ToString();
              codeList.Add((object) CodeObject.GetCodeObject("Override param ID: " + str, (byte) TheOverrideParameter.ParameterID));
              codeList.Add((object) CodeObject.GetCodeObject("Override param", TheOverrideParameter.Value, (int) TheOverrideParameter.ByteSize));
            }
          }
          else if (this.Version != 2)
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "No supported firmware version");
            return false;
          }
          ushort num2 = 0;
          ushort[] numArray = new ushort[this.MenuColumns];
          this.FunctionStartIndexOfMenuColumnList = new ArrayList();
          this.FunctionNumbersList = new ArrayList();
          for (short index = 0; (int) index < this.MenuColumns; ++index)
          {
            ushort TheValue;
            if (index == (short) 0)
            {
              ushort fromByteArrayUshort = ParameterService.GetFromByteArray_ushort(TheArray, ref num1);
              TheValue = ParameterService.GetFromByteArray_ushort(TheArray, ref num1);
              codeList.Add((object) CodeObject.GetCodeObject("Function start offset", fromByteArrayUshort));
              num2 = (ushort) ((uint) fromByteArrayUshort + (uint) (ushort) Offset);
              codeList.Add((object) CodeObject.GetCodeObject("Function end offset", TheValue));
            }
            else if ((int) index < this.MenuColumns - 1)
            {
              TheValue = ParameterService.GetFromByteArray_ushort(TheArray, ref num1);
              codeList.Add((object) CodeObject.GetCodeObject("Function end offset", TheValue));
            }
            else
              TheValue = (ushort) this.TableLen;
            ushort num3 = (ushort) ((uint) TheValue + (uint) (ushort) Offset);
            numArray[(int) index] = num3;
          }
          this.DummyBytes = new byte[(int) num2 - num1];
          ushort num4 = 0;
          while (num1 < (int) num2)
          {
            ushort Size = (ushort) ((uint) num2 - (uint) num1);
            if (Size > (ushort) 4)
              Size = (ushort) 4;
            for (int index = 0; index < (int) Size; ++index)
              this.DummyBytes[(int) num4++] = TheArray[num1 + index];
            ulong fromByteArrayUlong = ParameterService.GetFromByteArray_ulong(TheArray, Size, ref num1);
            codeList.Add((object) CodeObject.GetCodeObject("Additional bytes", fromByteArrayUlong, (int) Size));
          }
          short num5 = 0;
          bool flag = false;
          for (short index = 0; (int) index < this.MenuColumns; ++index)
          {
            ushort num6 = numArray[(int) index];
            if (num1 < (int) num6)
            {
              this.FunctionStartIndexOfMenuColumnList.Add((object) num5);
              if ((int) index == this.MenuColumns - 1)
                flag = true;
              do
              {
                ++num5;
                ushort TheValue = ParameterService.GetFromByteArray_ushort(TheArray, ref num1);
                if (flag)
                {
                  flag = false;
                  if ((int) TheValue != (int) this.MyMeter.MyIdent.DefaultFunctionNr)
                  {
                    if (this.MyMeter.MyHandler.UseOnlyDefaultValues)
                      TheValue = this.MyMeter.MyIdent.DefaultFunctionNr;
                    else
                      ZR_ClassLibMessages.AddWarning("Different DefaultFunction number at MeterInfo");
                  }
                }
                this.FunctionNumbersList.Add((object) TheValue);
                CodeObject codeObject = CodeObject.GetCodeObject("Column: " + index.ToString(), TheValue);
                codeList.Add((object) codeObject);
              }
              while (num1 < (int) num6);
            }
            else
              this.FunctionStartIndexOfMenuColumnList.Add((object) num5);
          }
          return true;
        }
      }
label_45:
      return false;
    }

    internal bool GenerateNewList()
    {
      this.LinkObjList.Add((object) new CodeBlock(CodeBlock.CodeSequenceTypes.LinkerGeneratedCodeBlock, FrameTypes.None, -1));
      ArrayList codeList = ((CodeBlock) this.LinkObjList[0]).CodeList;
      codeList.Add((object) CodeObject.GetCodeObject("Checksum", (ushort) 0));
      codeList.Add((object) CodeObject.GetCodeObject("Version", (byte) this.Version));
      codeList.Add((object) CodeObject.GetCodeObject("Menu columns", (byte) this.MenuColumns));
      CodeObject codeObject1 = CodeObject.GetCodeObject("Table len", (ushort) this.TableLen);
      codeList.Add((object) codeObject1);
      if (!this.MyMeter.MyCompiler.GenerateCodeFromCodeBlockList(this.LinkObjList))
        return false;
      int num1 = 6;
      if (this.Version > 2)
      {
        CodeObject codeObject2 = CodeObject.GetCodeObject("Number of override parameters", (byte) 0);
        codeList.Add((object) codeObject2);
        ++num1;
        int num2 = 0;
        for (int index = 0; index < this.OverridesList.Count; ++index)
        {
          OverrideParameter byIndex = (OverrideParameter) this.OverridesList.GetByIndex(index);
          if (byIndex.AtFunctionTabel && this.MyMeter.IsMeterResourceAvailable(byIndex.NeadedRessource))
          {
            ++num2;
            CodeObject codeObject3 = CodeObject.GetCodeObject("Override param ID: " + byIndex.ParameterID.ToString(), (byte) byIndex.ParameterID);
            this.MyMeter.MyCompiler.GenerateCodeFromCodeObject(codeObject3);
            codeList.Add((object) codeObject3);
            CodeObject codeObject4 = CodeObject.GetCodeObject("Override param", byIndex.Value, (int) byIndex.ByteSize);
            this.MyMeter.MyCompiler.GenerateCodeFromCodeObject(codeObject4);
            codeList.Add((object) codeObject4);
            num1 += 1 + (int) byIndex.ByteSize;
          }
        }
        codeObject2.CodeValueCompiled = (long) num2;
      }
      CodeObject[] codeObjectArray = new CodeObject[this.MenuColumns];
      codeObjectArray[0] = CodeObject.GetCodeObject("Function start offset", (ushort) 0);
      codeList.Add((object) codeObjectArray[0]);
      int num3 = num1 + 2;
      for (short index = 1; (int) index < this.MenuColumns; ++index)
      {
        codeObjectArray[(int) index] = CodeObject.GetCodeObject("Function end offset", (ushort) 0);
        codeList.Add((object) codeObjectArray[(int) index]);
        num3 += 2;
      }
      int StartOffset = 0;
      while (StartOffset < this.DummyBytes.Length)
      {
        ushort Size = (ushort) (this.DummyBytes.Length - StartOffset);
        if (Size > (ushort) 4)
          Size = (ushort) 4;
        ulong fromByteArrayUlong = ParameterService.GetFromByteArray_ulong(this.DummyBytes, Size, ref StartOffset);
        codeList.Add((object) CodeObject.GetCodeObject("Additional bytes", fromByteArrayUlong, (int) Size));
        num3 += (int) Size;
      }
      short index1 = 0;
      for (short index2 = 0; (int) index2 < this.FunctionNumbersList.Count; ++index2)
      {
        for (; (int) index1 < this.MenuColumns && (int) (short) this.FunctionStartIndexOfMenuColumnList[(int) index1] <= (int) index2; ++index1)
          codeObjectArray[(int) index1].CodeValueCompiled = (long) num3;
        short num4 = (short) ((int) index1 - 1);
        ushort functionNumbers = (ushort) this.FunctionNumbersList[(int) index2];
        CodeObject codeObject5 = CodeObject.GetCodeObject("Column: " + num4.ToString() + " Function: " + ((Function) this.FunctionListByNumber[(object) functionNumbers]).Name, functionNumbers);
        codeList.Add((object) codeObject5);
        num3 += 2;
      }
      this.TableLen = num3;
      codeObject1.CodeValueCompiled = (long) num3;
      return this.MyMeter.MyCompiler.GenerateCodeFromCodeBlockList(this.LinkObjList);
    }

    internal void GenerateChecksum()
    {
      this.Checksum = this.MyMeter.MyEpromHeader.GenerateChecksum(this.MyMeter.Eprom, this.BlockStartAddress + 2, this.TableLen - 2, (ushort) 0);
      CodeObject code = (CodeObject) ((CodeBlock) this.LinkObjList[0]).CodeList[0];
      code.CodeValueCompiled = (long) this.Checksum;
      this.MyMeter.MyCompiler.GenerateCodeFromCodeObjectAndCopyToEprom(code);
    }

    internal void AddFunctionNames()
    {
      foreach (CodeObject code in ((CodeBlock) this.LinkObjList[0]).CodeList)
      {
        if (code.LineInfo.StartsWith("Column"))
        {
          ushort codeValueCompiled = (ushort) code.CodeValueCompiled;
          CodeObject codeObject = code;
          codeObject.LineInfo = codeObject.LineInfo + " Function: " + ((Function) this.FunctionListByNumber[(object) codeValueCompiled]).Name;
        }
      }
    }

    internal bool GetFunctionXY(int x, int y, out Function TheFunction)
    {
      TheFunction = (Function) null;
      try
      {
        int index = (int) (short) this.FunctionStartIndexOfMenuColumnList[x] + y;
        if (index >= this.FunctionList.Count || x < this.FunctionStartIndexOfMenuColumnList.Count - 1 && index >= (int) (short) this.FunctionStartIndexOfMenuColumnList[x + 1])
          return false;
        TheFunction = (Function) this.FunctionList[index];
      }
      catch
      {
        return false;
      }
      return true;
    }

    internal bool DeleteFunctionsWithMissedResources()
    {
      if (this.MyMeter.MyHandler.ExtendedTypeEditMode)
        return true;
      bool flag1;
      do
      {
        do
        {
          flag1 = false;
          ushort[] numArray = new ushort[this.FunctionNumbersList.Count];
          for (int index = 0; index < numArray.Length; ++index)
            numArray[index] = (ushort) this.FunctionNumbersList[index];
          for (int index1 = 0; index1 < numArray.Length; ++index1)
          {
            Function fullLoadedFunction = (Function) this.MyMeter.MyHandler.MyLoadedFunctions.FullLoadedFunctions[(object) numArray[index1]];
            string empty = string.Empty;
            string[] neadedResources = fullLoadedFunction.NeadedResources;
            for (int index2 = 0; index2 < neadedResources.Length; ++index2)
            {
              string[] strArray = neadedResources[index2].Split('|');
              int index3;
              for (index3 = 0; index3 < strArray.Length; ++index3)
              {
                string key = strArray[index3];
                if (key.StartsWith("e:"))
                  key = key.Substring(2);
                if (key == "Developer" || this.MyMeter.AvailableMeterResouces[(object) key] != null)
                  break;
              }
              if (index3 == strArray.Length)
              {
                if (this.MyMeter.MyHandler.showFunctionRemoveMessages)
                {
                  ZR_ClassLibMessages.AddInfo(this.MyMeter.MyHandler.MyRes.GetString("FuncDel") + ": '" + fullLoadedFunction.FullName + "' It nead: " + neadedResources[index2]);
                  goto label_23;
                }
                else
                  goto label_23;
              }
            }
            if (fullLoadedFunction.SuppliedResources.Length != 0)
            {
              string impossibleResource = IoFunctionResourceCorrelation.GetImpossibleResource(this.MyMeter.InOut1Function, this.MyMeter.InOut2Function, fullLoadedFunction.SuppliedResources);
              if (impossibleResource != null)
              {
                if (this.MyMeter.MyHandler.showFunctionRemoveMessages)
                  ZR_ClassLibMessages.AddInfo(this.MyMeter.MyHandler.MyRes.GetString("FuncDel") + ": '" + fullLoadedFunction.FullName + "' It supplies: " + impossibleResource);
              }
              else
                continue;
            }
            else
              continue;
label_23:
            this.DeleteFunction(numArray[index1]);
            flag1 = true;
          }
          if (flag1 && !this.MyMeter.CreateMeterResourceInformation())
            return false;
        }
        while (flag1);
        ushort[] numArray1 = new ushort[this.FunctionNumbersList.Count];
        for (int index = 0; index < numArray1.Length; ++index)
          numArray1[index] = (ushort) this.FunctionNumbersList[index];
        for (int index4 = 0; index4 < numArray1.Length; ++index4)
        {
          Function fullLoadedFunction = (Function) this.MyMeter.MyHandler.MyLoadedFunctions.FullLoadedFunctions[(object) numArray1[index4]];
          for (int index5 = 0; index5 < fullLoadedFunction.NotSupportedResources.Length; ++index5)
          {
            if (this.MyMeter.AvailableMeterResouces[(object) fullLoadedFunction.NotSupportedResources[index5]] != null)
            {
              if (this.MyMeter.MyHandler.showFunctionRemoveMessages)
                ZR_ClassLibMessages.AddInfo(this.MyMeter.MyHandler.MyRes.GetString("FuncDel") + ": " + fullLoadedFunction.FullName + " (" + fullLoadedFunction.NotSupportedResources[index5] + " not supported!)");
              this.DeleteFunction(numArray1[index4]);
              flag1 = true;
              break;
            }
          }
        }
        if (flag1 && !this.MyMeter.CreateMeterResourceInformation())
          return false;
      }
      while (flag1);
      bool flag2 = false;
      if (this.MyMeter.AvailableMeterResouces.IndexOfKey((object) "Inp1On") < 0 && this.MyMeter.AvailableMeterResouces.IndexOfKey((object) "Out1On") < 0)
      {
        this.MyMeter.InOut1Function = InOutFunctions.IO1_Off;
        flag2 = true;
      }
      if (this.MyMeter.AvailableMeterResouces.IndexOfKey((object) "Inp2On") < 0 && this.MyMeter.AvailableMeterResouces.IndexOfKey((object) "Out2On") < 0)
      {
        this.MyMeter.InOut2Function = InOutFunctions.IO2_Off;
        flag2 = true;
      }
      if (flag2)
      {
        OverrideParameter overrides = (OverrideParameter) this.OverridesList[(object) OverrideID.IO_Functions];
        if (overrides != null)
          overrides.Value = (ulong) this.MyMeter.InOut1Function | (ulong) this.MyMeter.InOut2Function;
      }
      string neadedResources1 = IoFunctionResourceCorrelation.GetNeadedResources(this.MyMeter.InOut1Function, this.MyMeter.InOut2Function, this.MyMeter.AvailableMeterResouces);
      if (neadedResources1.Length > 0)
      {
        ZR_ClassLibMessages.AddWarning(neadedResources1);
        ZR_ClassLibMessages.AddWarning("Missing input or output function! Neaded resources:");
      }
      return true;
    }

    internal bool DeleteFunction(int x, int y)
    {
      int index1 = (int) (short) this.FunctionStartIndexOfMenuColumnList[x] + y;
      for (int index2 = x + 1; index2 < this.FunctionStartIndexOfMenuColumnList.Count; ++index2)
        this.FunctionStartIndexOfMenuColumnList[index2] = (object) (short) ((int) (short) this.FunctionStartIndexOfMenuColumnList[index2] - 1);
      this.FunctionNumbersList.RemoveAt(index1);
      return true;
    }

    internal bool DeleteFunction(ushort FunctionNumber)
    {
      int index1 = 0;
      while (index1 < this.FunctionNumbersList.Count && (int) (ushort) this.FunctionNumbersList[index1] != (int) FunctionNumber)
        ++index1;
      if (index1 == this.FunctionNumbersList.Count)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Function not found");
        return false;
      }
      for (int index2 = 0; index2 < this.FunctionStartIndexOfMenuColumnList.Count; ++index2)
      {
        if ((int) (short) this.FunctionStartIndexOfMenuColumnList[index2] > index1)
          this.FunctionStartIndexOfMenuColumnList[index2] = (object) (short) ((int) (short) this.FunctionStartIndexOfMenuColumnList[index2] - 1);
      }
      this.FunctionNumbersList.RemoveAt(index1);
      return true;
    }

    internal bool RepareAndCompress()
    {
      for (int index = 0; index < this.FunctionNumbersList.Count; ++index)
      {
        Function loadedFunctionHeader = (Function) this.MyMeter.MyHandler.MyLoadedFunctions.LoadedFunctionHeaders[this.FunctionNumbersList[index]];
        if ((long) loadedFunctionHeader.FirmwareVersionMax < this.MyMeter.MyIdent.lFirmwareVersion || (long) loadedFunctionHeader.FirmwareVersionMin > this.MyMeter.MyIdent.lFirmwareVersion)
        {
          this.DeleteFunction(loadedFunctionHeader.Number);
          --index;
        }
      }
      return true;
    }

    internal bool AddFunction(int x, int y, int FunctionNumber)
    {
      int index1 = (int) (short) this.FunctionStartIndexOfMenuColumnList[x] + y;
      if (index1 >= this.FunctionNumbersList.Count)
      {
        this.FunctionNumbersList.Add((object) (ushort) FunctionNumber);
      }
      else
      {
        if (x < this.FunctionStartIndexOfMenuColumnList.Count - 1)
        {
          if (index1 > (int) (short) this.FunctionStartIndexOfMenuColumnList[x + 1])
            index1 = (int) (short) this.FunctionStartIndexOfMenuColumnList[x + 1];
          for (int index2 = x + 1; index2 < this.FunctionStartIndexOfMenuColumnList.Count; ++index2)
            this.FunctionStartIndexOfMenuColumnList[index2] = (object) (short) ((int) (short) this.FunctionStartIndexOfMenuColumnList[index2] + 1);
        }
        this.FunctionNumbersList.Insert(index1, (object) (ushort) FunctionNumber);
      }
      ulong NeadedIOFunction;
      ulong NeadedIOFunctionMask;
      if (MeterResource.GetNeadedIOFunction(((Function) this.MyMeter.MyHandler.MyLoadedFunctions.FullLoadedFunctions[(object) (ushort) FunctionNumber]).SuppliedResources, out NeadedIOFunction, out NeadedIOFunctionMask))
      {
        OverrideParameter overrides = (OverrideParameter) this.MyMeter.MyFunctionTable.OverridesList[(object) OverrideID.IO_Functions];
        if (overrides != null)
          overrides.Value = overrides.Value & ~NeadedIOFunctionMask | NeadedIOFunction;
      }
      return true;
    }

    internal bool AddOverridesFromParameter()
    {
      if (this.OverridesList == null)
        this.OverridesList = new SortedList();
      OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.SerialNumber, (ulong) ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_SerialNr"]).ValueEprom));
      OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.MeterID, (ulong) ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_MeterID"]).ValueEprom));
      OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.BaseTypeID, (ulong) ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_MeterInfoID"]).ValueEprom));
      OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.FactoryTypeID, (ulong) ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_MeterTypeID"]).ValueEprom));
      if (this.MyMeter.IsMeterResourceAvailable(MeterResources.CustomerId))
        OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.CustomID, (ulong) ((Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.CustomerId.ToString()]).ValueEprom));
      else
        OverrideParameter.DeleteOverrideParameter(this.OverridesList, OverrideID.CustomID);
      OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.Medium, (ulong) ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_MBusMedium"]).ValueEprom));
      object allParameter1 = this.MyMeter.AllParameters[(object) "DefaultFunction.Itr_RefreshTime"];
      if (allParameter1 != null)
        OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.CycleTimeStandard, (ulong) ((Parameter) allParameter1).ValueEprom));
      object allParameter2 = this.MyMeter.AllParameters[(object) "DefaultFunction.Itr_RefreshTimeShort"];
      if (allParameter2 != null)
        OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.CycleTimeFast, (ulong) ((Parameter) allParameter2).ValueEprom));
      object allParameter3 = this.MyMeter.AllParameters[(object) "DefaultFunction.ADC_HeatThreshold"];
      if (allParameter3 != null)
        OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.HeatThresholdTemp, (ulong) ((Parameter) allParameter3).ValueEprom));
      bool flag = true;
      object allParameter4 = this.MyMeter.AllParameters[(object) "DefaultFunction.Waerme_Grenze_DeltaT_min"];
      if (allParameter4 != null)
      {
        Parameter parameter = (Parameter) allParameter4;
        OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.MinTempDiffPlusTemp, (ulong) parameter.ValueEprom));
        if (parameter.ValueEprom != 0L)
          flag = false;
      }
      object allParameter5 = this.MyMeter.AllParameters[(object) "DefaultFunction.Kaelte_Grenze_DeltaT_min"];
      if (allParameter5 != null)
      {
        Parameter parameter = (Parameter) allParameter5;
        OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.MinTempDiffMinusTemp, (ulong) parameter.ValueEprom));
        if (parameter.ValueEprom != 0L)
          flag = false;
      }
      object allParameter6 = this.MyMeter.AllParameters[(object) "DefaultFunction.Energie_Konfiguration"];
      if (allParameter6 != null)
      {
        Parameter parameter = (Parameter) allParameter6;
        OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, ((uint) (int) parameter.ValueEprom & (uint) (int) this.MyMeter.MyCompiler.Includes[(object) "ENG_KONFIG_TEMP_VL_FEST"]) <= 0U ? (((uint) (int) parameter.ValueEprom & (uint) (int) this.MyMeter.MyCompiler.Includes[(object) "ENG_KONFIG_TEMP_RL_FEST"]) <= 0U ? new OverrideParameter(OverrideID.FixedTempSetup, FixedTempSetup.OFF.ToString(), true) : new OverrideParameter(OverrideID.FixedTempSetup, FixedTempSetup.Return.ToString(), true)) : new OverrideParameter(OverrideID.FixedTempSetup, FixedTempSetup.Flow.ToString(), true));
        OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, !flag ? (((uint) (int) parameter.ValueEprom & (uint) (int) this.MyMeter.MyCompiler.Includes[(object) "ENG_KONFIG_DELTAT_NULL_SETZEN"]) <= 0U ? new OverrideParameter(OverrideID.MimTempDiffSetup, MinimalTempDiffSetup.SetToMin.ToString(), true) : new OverrideParameter(OverrideID.MimTempDiffSetup, MinimalTempDiffSetup.SetTo0.ToString(), true)) : new OverrideParameter(OverrideID.MimTempDiffSetup, MinimalTempDiffSetup.OFF.ToString(), true));
      }
      if (this.MyMeter.MyCompiler.Includes.ContainsKey((object) "RW_KONFIG_DYN_VOL_CYCLE"))
      {
        object allParameter7 = this.MyMeter.AllParameters[(object) "DefaultFunction.RW_Typ_Konfiguration"];
        if (allParameter7 != null)
        {
          Parameter parameter = (Parameter) allParameter7;
          OverrideParameter TheOverrideParameter = new OverrideParameter(OverrideID.CycleTimeDynamic, CycleTimeChangeMethode.ExtPower.ToString(), true);
          if (((uint) (int) parameter.ValueEprom & (uint) (int) this.MyMeter.MyCompiler.Includes[(object) "RW_KONFIG_DYN_VOL_CYCLE"]) > 0U)
            TheOverrideParameter.Value |= 2UL;
          if (((uint) (int) parameter.ValueEprom & (uint) (int) this.MyMeter.MyCompiler.Includes[(object) "RW_KONFIG_DYN_TEMP_CYCLE"]) > 0U)
            TheOverrideParameter.Value |= 4UL;
          OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, TheOverrideParameter);
        }
      }
      object allParameter8 = this.MyMeter.AllParameters[(object) "DefaultFunction.Feste_Fuehlertemperatur"];
      if (allParameter8 != null)
        OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.FixedTempValue, (ulong) ((Parameter) allParameter8).ValueEprom));
      object allParameter9 = this.MyMeter.AllParameters[(object) "DefaultFunction.ADC_HeatThreshold"];
      if (allParameter9 != null)
        OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.HeatThresholdTemp, (ulong) ((Parameter) allParameter9).ValueEprom));
      object allParameter10 = this.MyMeter.AllParameters[(object) "DefaultFunction.TAR_Setup"];
      if (allParameter10 != null)
      {
        OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.TarifFunction, (ulong) ((Parameter) allParameter10).ValueEprom));
        OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.TarifRefTemp, (ulong) ((Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.TAR_RefTemp"]).ValueEprom));
      }
      OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.MBusAddress, (ulong) ((Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.MBu_Address"]).ValueEprom));
      OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.MBusIdentificationNo, (ulong) ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_MBusSerialNr"]).ValueEprom));
      OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.Baudrate, (ulong) this.MyMeter.GetBaudrate()));
      if (this.MyMeter.IsMeterResourceAvailable(MeterResources.EndOfBattery))
        OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.EndOfBattery, ZR_Calendar.Cal_GetDateTime((uint) ((Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.EndOfBattery.ToString()]).ValueEprom)));
      else
        OverrideParameter.DeleteOverrideParameter(this.OverridesList, OverrideID.EndOfBattery);
      if (this.MyMeter.IsMeterResourceAvailable(MeterResources.EndOfCalibration))
      {
        OverrideParameter TheOverrideParameter = new OverrideParameter(OverrideID.EndOfCalibration, (ulong) ZR_Calendar.Cal_GetDateTime((uint) ((Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.EndOfCalibration.ToString()]).ValueEprom).Year);
        if (!UserRights.GlobalUserRights.CheckRight(UserRights.Rights.DesignerChangeMenu))
          TheOverrideParameter.HasWritePermission = false;
        OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, TheOverrideParameter);
      }
      else
        OverrideParameter.DeleteOverrideParameter(this.OverridesList, OverrideID.EndOfCalibration);
      if (this.MyMeter.MyMath.MyBaseSettings != null)
      {
        Decimal ScaleFactor1 = (Decimal) Math.Pow(10.0, (double) MeterMath.EnergyUnits[this.MyMeter.MyMath.MyBaseSettings.EnergyUnitIndex].AfterPointDigits);
        Decimal ScaleFactor2 = (Decimal) Math.Pow(2.0, (double) this.MyMeter.MyMath.MyBaseSettings.Energy_SumExpo) * ScaleFactor1;
        Decimal ScaleFactor3 = (Decimal) Math.Pow(10.0, (double) MeterMath.VolumeUnits[this.MyMeter.MyMath.MyBaseSettings.VolumeUnitIndex].AfterPointDigits);
        Decimal ScaleFactor4 = (Decimal) Math.Pow(2.0, (double) this.MyMeter.MyMath.MyBaseSettings.Vol_SumExpo) * ScaleFactor3;
        OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.EnergyActualValue, (ulong) ((Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.Waerme_EnergSum"]).ValueEprom, ScaleFactor2));
        if (this.MyMeter.IsMeterResourceAvailable(MeterResources.CEnergy))
          OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.CEnergyActualValue, (ulong) ((Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.Kaelte_EnergSum"]).ValueEprom, ScaleFactor2));
        else
          OverrideParameter.DeleteOverrideParameter(this.OverridesList, OverrideID.CEnergyActualValue);
        OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.VolumeActualValue, (ulong) ((Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.Vol_VolSum"]).ValueEprom, ScaleFactor4));
        object allParameter11 = this.MyMeter.AllParameters[(object) "DefaultFunction.TAR_EnergySumTar0"];
        if (allParameter11 != null)
        {
          OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.TarifEnergy0, (ulong) ((Parameter) allParameter11).ValueEprom, ScaleFactor2));
          OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.TarifEnergy1, (ulong) ((Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.TAR_EnergySumTar1"]).ValueEprom, ScaleFactor2));
        }
        if (this.MyMeter.IsMeterResourceAvailable(MeterResources.DueDate))
        {
          OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.ReadingDate, (ulong) ((Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.DueDate.ToString()]).ValueEprom));
          if (this.MyMeter.IsMeterResourceAvailable(MeterResources.DueDateEnergy))
            OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.EnergyDueDateValue, (ulong) ((Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.DueDateEnergy.ToString()]).ValueEprom, ScaleFactor1));
          else
            OverrideParameter.DeleteOverrideParameter(this.OverridesList, OverrideID.EnergyDueDateValue);
          if (this.MyMeter.IsMeterResourceAvailable(MeterResources.DueDateLastEnergy))
            OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.EnergyDueDateLastValue, (ulong) ((Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.DueDateLastEnergy.ToString()]).ValueEprom, ScaleFactor1));
          else
            OverrideParameter.DeleteOverrideParameter(this.OverridesList, OverrideID.EnergyDueDateLastValue);
          if (this.MyMeter.IsMeterResourceAvailable(MeterResources.DueDateCEnergy))
            OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.CEnergyDueDateValue, (ulong) ((Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.DueDateCEnergy.ToString()]).ValueEprom, ScaleFactor1));
          else
            OverrideParameter.DeleteOverrideParameter(this.OverridesList, OverrideID.CEnergyDueDateValue);
          if (this.MyMeter.IsMeterResourceAvailable(MeterResources.DueDateLastCEnergy))
            OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.CEnergyDueDateLastValue, (ulong) ((Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.DueDateLastCEnergy.ToString()]).ValueEprom, ScaleFactor1));
          else
            OverrideParameter.DeleteOverrideParameter(this.OverridesList, OverrideID.CEnergyDueDateLastValue);
          if (this.MyMeter.IsMeterResourceAvailable(MeterResources.DueDateVolume))
            OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.VolumeDueDateValue, (ulong) ((Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.DueDateVolume.ToString()]).ValueEprom, ScaleFactor3));
          else
            OverrideParameter.DeleteOverrideParameter(this.OverridesList, OverrideID.VolumeDueDateValue);
          if (this.MyMeter.IsMeterResourceAvailable(MeterResources.DueDateLastVolume))
            OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, new OverrideParameter(OverrideID.VolumeDueDateLastValue, (ulong) ((Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.DueDateLastVolume.ToString()]).ValueEprom, ScaleFactor3));
          else
            OverrideParameter.DeleteOverrideParameter(this.OverridesList, OverrideID.VolumeDueDateLastValue);
        }
        if (this.MyMeter.IsMeterResourceAvailable(MeterResources.Inp1On))
        {
          if (this.MyMeter.IsMeterResourceAvailable(MeterResources.Inp1IdNumber))
          {
            OverrideParameter TheOverrideParameter1 = new OverrideParameter(OverrideID.Input1IdNumber, (ulong) ((Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.Inp1IdNumber.ToString()]).ValueEprom);
            TheOverrideParameter1.SubDevice = 1;
            OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, TheOverrideParameter1);
            if (this.MyMeter.IsMeterResourceAvailable(MeterResources.Inp1Type))
            {
              OverrideParameter TheOverrideParameter2 = new OverrideParameter(OverrideID.Input1Type, (ulong) ((Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.Inp1Type.ToString()]).ValueEprom);
              TheOverrideParameter2.SubDevice = 1;
              OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, TheOverrideParameter2);
            }
            else
              OverrideParameter.DeleteOverrideParameter(this.OverridesList, OverrideID.Input1Type);
          }
          else
          {
            OverrideParameter.DeleteOverrideParameter(this.OverridesList, OverrideID.Input1IdNumber);
            OverrideParameter.DeleteOverrideParameter(this.OverridesList, OverrideID.Input1Type);
          }
          Decimal ScaleFactor5 = (Decimal) Math.Pow(10.0, (double) MeterMath.InputUnits[this.MyMeter.MyMath.MyBaseSettings.Input1UnitIndex].AfterPointDigits);
          OverrideParameter TheOverrideParameter3 = new OverrideParameter(OverrideID.Input1ActualValue, (ulong) ((Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.In1Display"]).ValueEprom, ScaleFactor5);
          TheOverrideParameter3.SubDevice = 1;
          OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, TheOverrideParameter3);
          if (this.MyMeter.IsMeterResourceAvailable(MeterResources.DueDateInp1Value))
          {
            OverrideParameter TheOverrideParameter4 = new OverrideParameter(OverrideID.Input1DueDateValue, (ulong) ((Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.DueDateInp1Value.ToString()]).ValueEprom, ScaleFactor5);
            TheOverrideParameter4.SubDevice = 1;
            OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, TheOverrideParameter4);
          }
          else
            OverrideParameter.DeleteOverrideParameter(this.OverridesList, OverrideID.Input1DueDateValue);
          if (this.MyMeter.IsMeterResourceAvailable(MeterResources.DueDateInp1LastValue))
          {
            OverrideParameter TheOverrideParameter5 = new OverrideParameter(OverrideID.Input1DueDateLastValue, (ulong) ((Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.DueDateInp1LastValue.ToString()]).ValueEprom, ScaleFactor5);
            OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, TheOverrideParameter5);
            TheOverrideParameter5.SubDevice = 1;
          }
          else
            OverrideParameter.DeleteOverrideParameter(this.OverridesList, OverrideID.Input1DueDateLastValue);
        }
        if (this.MyMeter.IsMeterResourceAvailable(MeterResources.Inp2On))
        {
          if (this.MyMeter.IsMeterResourceAvailable(MeterResources.Inp2IdNumber))
          {
            OverrideParameter TheOverrideParameter6 = new OverrideParameter(OverrideID.Input2IdNumber, (ulong) ((Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.Inp2IdNumber.ToString()]).ValueEprom);
            TheOverrideParameter6.SubDevice = 2;
            OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, TheOverrideParameter6);
            if (this.MyMeter.IsMeterResourceAvailable(MeterResources.Inp2Type))
            {
              OverrideParameter TheOverrideParameter7 = new OverrideParameter(OverrideID.Input2Type, (ulong) ((Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.Inp2Type.ToString()]).ValueEprom);
              TheOverrideParameter7.SubDevice = 2;
              OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, TheOverrideParameter7);
            }
            else
              OverrideParameter.DeleteOverrideParameter(this.OverridesList, OverrideID.Input2Type);
          }
          else
          {
            OverrideParameter.DeleteOverrideParameter(this.OverridesList, OverrideID.Input2IdNumber);
            OverrideParameter.DeleteOverrideParameter(this.OverridesList, OverrideID.Input2Type);
          }
          Decimal ScaleFactor6 = (Decimal) Math.Pow(10.0, (double) MeterMath.InputUnits[this.MyMeter.MyMath.MyBaseSettings.Input2UnitIndex].AfterPointDigits);
          OverrideParameter TheOverrideParameter8 = new OverrideParameter(OverrideID.Input2ActualValue, (ulong) ((Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.In2Display"]).ValueEprom, ScaleFactor6);
          TheOverrideParameter8.SubDevice = 2;
          OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, TheOverrideParameter8);
          if (this.MyMeter.IsMeterResourceAvailable(MeterResources.DueDateInp2Value))
          {
            OverrideParameter TheOverrideParameter9 = new OverrideParameter(OverrideID.Input2DueDateValue, (ulong) ((Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.DueDateInp2Value.ToString()]).ValueEprom, ScaleFactor6);
            TheOverrideParameter9.SubDevice = 2;
            OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, TheOverrideParameter9);
          }
          else
            OverrideParameter.DeleteOverrideParameter(this.OverridesList, OverrideID.Input2DueDateValue);
          if (this.MyMeter.IsMeterResourceAvailable(MeterResources.DueDateInp2LastValue))
          {
            OverrideParameter TheOverrideParameter10 = new OverrideParameter(OverrideID.Input2DueDateLastValue, (ulong) ((Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.DueDateInp2LastValue.ToString()]).ValueEprom, ScaleFactor6);
            TheOverrideParameter10.SubDevice = 2;
            OverrideParameter.ChangeOrAddOverrideParameter(this.OverridesList, TheOverrideParameter10);
          }
          else
            OverrideParameter.DeleteOverrideParameter(this.OverridesList, OverrideID.Input2DueDateLastValue);
        }
      }
      return true;
    }

    private string GetTrueStringValue(long LongValue, Decimal TheDevisor)
    {
      try
      {
        if (TheDevisor == 1M)
          return LongValue.ToString();
        string str = (1M / TheDevisor).ToString();
        if (str.StartsWith("0" + SystemValues.ZRDezimalSeparator))
        {
          int num1 = 2;
          while (num1 < str.Length && str[num1] == '0')
            ++num1;
          if (num1 != str.Length)
          {
            Decimal num2 = Decimal.Parse(str.Substring(0, num1) + "1");
            Decimal num3 = (Decimal) LongValue / TheDevisor;
            for (int index = 0; index < 100; ++index)
            {
              string s = num3.ToString();
              int num4 = s.IndexOf(SystemValues.ZRDezimalSeparator);
              if (num4 >= 0)
              {
                int length = num4 + num1;
                if (length < s.Length)
                  s = s.Substring(0, length);
              }
              if ((long) (Decimal.Parse(s) * TheDevisor) == LongValue)
                return s;
              num3 += num2;
            }
          }
        }
      }
      catch
      {
      }
      throw new ArgumentException("True string convertion error");
    }

    internal bool CopyBaseOverridesToParameter()
    {
      if (this.OverridesList == null)
        return true;
      OverrideParameter overrides1 = (OverrideParameter) this.OverridesList[(object) OverrideID.CustomID];
      if (overrides1 != null)
      {
        Parameter parameter = (Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.CustomerId.ToString()];
        if (parameter != null)
          parameter.ValueEprom = (long) overrides1.Value;
      }
      OverrideParameter overrides2 = (OverrideParameter) this.OverridesList[(object) OverrideID.Medium];
      if (overrides2 != null)
      {
        Parameter allParameter = (Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_MBusMedium"];
        if (allParameter != null)
          allParameter.ValueEprom = (long) overrides2.Value;
      }
      OverrideParameter overrides3 = (OverrideParameter) this.OverridesList[(object) OverrideID.CycleTimeStandard];
      if (overrides3 != null)
      {
        Parameter allParameter = (Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.Itr_RefreshTime"];
        if (allParameter != null)
          allParameter.ValueEprom = (long) overrides3.Value;
      }
      OverrideParameter overrides4 = (OverrideParameter) this.OverridesList[(object) OverrideID.CycleTimeFast];
      if (overrides4 != null)
      {
        Parameter allParameter = (Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.Itr_RefreshTimeShort"];
        if (allParameter != null)
          allParameter.ValueEprom = (long) overrides4.Value;
      }
      OverrideParameter overrides5 = (OverrideParameter) this.OverridesList[(object) OverrideID.CycleTimeDynamic];
      if (overrides5 != null)
      {
        Parameter allParameter = (Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.RW_Typ_Konfiguration"];
        if (allParameter != null)
        {
          if ((overrides5.Value & 2UL) > 0UL)
            allParameter.ValueEprom |= (long) (uint) (int) this.MyMeter.MyCompiler.Includes[(object) "RW_KONFIG_DYN_VOL_CYCLE"];
          else
            allParameter.ValueEprom &= ~(long) (int) this.MyMeter.MyCompiler.Includes[(object) "RW_KONFIG_DYN_VOL_CYCLE"];
          if ((overrides5.Value & 4UL) > 0UL)
            allParameter.ValueEprom |= (long) (uint) (int) this.MyMeter.MyCompiler.Includes[(object) "RW_KONFIG_DYN_TEMP_CYCLE"];
          else
            allParameter.ValueEprom &= ~(long) (int) this.MyMeter.MyCompiler.Includes[(object) "RW_KONFIG_DYN_TEMP_CYCLE"];
        }
      }
      OverrideParameter overrides6 = (OverrideParameter) this.OverridesList[(object) OverrideID.HeatThresholdTemp];
      if (overrides6 != null)
      {
        Parameter allParameter = (Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.ADC_HeatThreshold"];
        if (allParameter != null)
          allParameter.ValueEprom = (long) overrides6.Value;
      }
      OverrideParameter overrides7 = (OverrideParameter) this.OverridesList[(object) OverrideID.MimTempDiffSetup];
      if (overrides7 != null)
      {
        Parameter allParameter1 = (Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.Energie_Konfiguration"];
        if (allParameter1 != null)
        {
          bool flag = false;
          long include = (long) (int) this.MyMeter.MyCompiler.Includes[(object) "ENG_KONFIG_DELTAT_NULL_SETZEN"];
          switch ((MinimalTempDiffSetup) overrides7.Value)
          {
            case MinimalTempDiffSetup.SetTo0:
              allParameter1.ValueEprom |= include;
              flag = true;
              break;
            case MinimalTempDiffSetup.SetToMin:
              allParameter1.ValueEprom &= ~include;
              flag = true;
              break;
            default:
              allParameter1.ValueEprom &= ~include;
              break;
          }
          if (flag)
          {
            OverrideParameter overrides8 = (OverrideParameter) this.OverridesList[(object) OverrideID.MinTempDiffPlusTemp];
            if (overrides8 != null)
            {
              Parameter allParameter2 = (Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.Waerme_Grenze_DeltaT_min"];
              if (allParameter2 != null)
                allParameter2.ValueEprom = (long) overrides8.Value;
            }
            OverrideParameter overrides9 = (OverrideParameter) this.OverridesList[(object) OverrideID.MinTempDiffMinusTemp];
            if (overrides9 != null)
            {
              Parameter allParameter3 = (Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.Kaelte_Grenze_DeltaT_min"];
              if (allParameter3 != null)
                allParameter3.ValueEprom = (long) overrides9.Value;
            }
          }
          else
          {
            Parameter allParameter4 = (Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.Waerme_Grenze_DeltaT_min"];
            if (allParameter4 != null)
              allParameter4.ValueEprom = 0L;
            Parameter allParameter5 = (Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.Kaelte_Grenze_DeltaT_min"];
            if (allParameter5 != null)
              allParameter5.ValueEprom = 0L;
          }
        }
      }
      OverrideParameter overrides10 = (OverrideParameter) this.OverridesList[(object) OverrideID.FixedTempSetup];
      if (overrides10 != null)
      {
        Parameter allParameter = (Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.Energie_Konfiguration"];
        allParameter.ValueEprom &= ~(long) (int) this.MyMeter.MyCompiler.Includes[(object) "ENG_KONFIG_TEMP_VL_FEST"];
        allParameter.ValueEprom &= ~(long) (int) this.MyMeter.MyCompiler.Includes[(object) "ENG_KONFIG_TEMP_RL_FEST"];
        if (allParameter != null)
        {
          switch ((FixedTempSetup) overrides10.Value)
          {
            case FixedTempSetup.Flow:
              allParameter.ValueEprom = (long) ((int) allParameter.ValueEprom | (int) this.MyMeter.MyCompiler.Includes[(object) "ENG_KONFIG_TEMP_VL_FEST"]);
              break;
            case FixedTempSetup.Return:
              allParameter.ValueEprom = (long) ((int) allParameter.ValueEprom | (int) this.MyMeter.MyCompiler.Includes[(object) "ENG_KONFIG_TEMP_RL_FEST"]);
              break;
          }
        }
      }
      OverrideParameter overrides11 = (OverrideParameter) this.OverridesList[(object) OverrideID.FixedTempValue];
      if (overrides11 != null)
        ((Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.Feste_Fuehlertemperatur"]).ValueEprom = (long) overrides11.Value;
      OverrideParameter overrides12 = (OverrideParameter) this.OverridesList[(object) OverrideID.TarifFunction];
      if (overrides12 != null)
      {
        ((Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.TAR_Setup"]).ValueEprom = (long) overrides12.Value;
        OverrideParameter overrides13 = (OverrideParameter) this.OverridesList[(object) OverrideID.TarifRefTemp];
        if (overrides13 != null)
          ((Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.TAR_RefTemp"]).ValueEprom = (long) overrides13.Value;
      }
      OverrideParameter overrides14 = (OverrideParameter) this.OverridesList[(object) OverrideID.MBusAddress];
      if (overrides14 != null)
        ((Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.MBu_Address"]).ValueEprom = (long) (byte) overrides14.Value;
      OverrideParameter overrides15 = (OverrideParameter) this.OverridesList[(object) OverrideID.MBusIdentificationNo];
      if (overrides15 != null)
        ((Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_MBusSerialNr"]).ValueEprom = (long) overrides15.Value;
      OverrideParameter overrides16 = (OverrideParameter) this.OverridesList[(object) OverrideID.Baudrate];
      if (overrides16 != null)
        this.MyMeter.SetBaudrate((int) overrides16.Value);
      OverrideParameter overrides17 = (OverrideParameter) this.OverridesList[(object) OverrideID.IO_Functions];
      if (overrides17 != null)
      {
        this.MyMeter.InOut1Function = (InOutFunctions) ((long) overrides17.Value & 15L);
        this.MyMeter.InOut2Function = (InOutFunctions) ((long) overrides17.Value & 240L);
      }
      OverrideParameter overrides18 = (OverrideParameter) this.OverridesList[(object) OverrideID.EndOfBattery];
      if (overrides18 != null)
      {
        object obj = this.MyMeter.AllParametersByResource[(object) MeterResources.EndOfBattery.ToString()];
        if (obj != null)
          ((Parameter) obj).ValueEprom = (long) ZR_Calendar.Cal_GetMeterTime(new DateTime(((DateTime) overrides18.ParameterValue).Year, 12, 31, 23, 59, 0));
      }
      OverrideParameter overrides19 = (OverrideParameter) this.OverridesList[(object) OverrideID.EndOfCalibration];
      if (overrides19 != null)
      {
        object obj = this.MyMeter.AllParametersByResource[(object) MeterResources.EndOfCalibration.ToString()];
        if (obj != null)
          ((Parameter) obj).ValueEprom = (long) ZR_Calendar.Cal_GetMeterTime(new DateTime((int) overrides19.Value, 12, 31, 23, 59, 0));
      }
      return true;
    }

    internal bool GarantTypeSpecOverrides()
    {
      if (this.OverridesList == null)
        return true;
      MeterResources meterResources;
      if ((OverrideParameter) this.OverridesList[(object) OverrideID.BaseConfig] == null)
      {
        OverrideParameter overrideParameter = new OverrideParameter(OverrideID.BaseConfig);
        this.OverridesList.Add((object) overrideParameter.ParameterID, (object) overrideParameter);
        SortedList availableMeterResouces = this.MyMeter.AvailableMeterResouces;
        meterResources = MeterResources.CEnergy;
        string key = meterResources.ToString();
        overrideParameter.Value = availableMeterResouces[(object) key] == null ? 1UL : 5UL;
      }
      if ((OverrideParameter) this.OverridesList[(object) OverrideID.TarifFunction] == null)
      {
        object allParameter = this.MyMeter.AllParameters[(object) "DefaultFunction.TAR_Setup"];
        if (allParameter != null)
        {
          OverrideParameter overrideParameter = new OverrideParameter(OverrideID.TarifFunction, (ulong) ((Parameter) allParameter).ValueEprom);
          this.OverridesList.Add((object) overrideParameter.ParameterID, (object) overrideParameter);
        }
      }
      if ((OverrideParameter) this.OverridesList[(object) OverrideID.ModuleType] == null)
      {
        OverrideParameter overrideParameter = new OverrideParameter(OverrideID.ModuleType);
        this.OverridesList.Add((object) overrideParameter.ParameterID, (object) overrideParameter);
        SortedList availableMeterResouces1 = this.MyMeter.AvailableMeterResouces;
        meterResources = MeterResources.Inp2On;
        string key1 = meterResources.ToString();
        int num;
        if (availableMeterResouces1[(object) key1] == null)
        {
          SortedList availableMeterResouces2 = this.MyMeter.AvailableMeterResouces;
          meterResources = MeterResources.EnToOut1;
          string key2 = meterResources.ToString();
          if (availableMeterResouces2[(object) key2] == null)
          {
            SortedList neadedMeterResources = this.MyMeter.NeadedMeterResources;
            meterResources = MeterResources.MBus;
            string key3 = meterResources.ToString();
            num = neadedMeterResources[(object) key3] != null ? 1 : 0;
            goto label_13;
          }
        }
        num = 1;
label_13:
        if (num != 0)
          overrideParameter.Value = 31UL;
      }
      if ((OverrideParameter) this.OverridesList[(object) OverrideID.IO_Functions] == null)
      {
        OverrideParameter overrideParameter = new OverrideParameter(OverrideID.IO_Functions);
        this.OverridesList.Add((object) overrideParameter.ParameterID, (object) overrideParameter);
        overrideParameter.Value = 0UL;
        SortedList neadedMeterResources1 = this.MyMeter.NeadedMeterResources;
        meterResources = MeterResources.Inp1;
        string key4 = meterResources.ToString();
        if (neadedMeterResources1[(object) key4] != null)
        {
          overrideParameter.Value += 2UL;
        }
        else
        {
          SortedList neadedMeterResources2 = this.MyMeter.NeadedMeterResources;
          meterResources = MeterResources.Out1;
          string key5 = meterResources.ToString();
          if (neadedMeterResources2[(object) key5] != null)
          {
            SortedList availableMeterResouces = this.MyMeter.AvailableMeterResouces;
            meterResources = MeterResources.EnToOut1;
            string key6 = meterResources.ToString();
            if (availableMeterResouces[(object) key6] != null)
              overrideParameter.Value += 3UL;
            else
              ++overrideParameter.Value;
          }
          else
            ++overrideParameter.Value;
        }
        SortedList neadedMeterResources3 = this.MyMeter.NeadedMeterResources;
        meterResources = MeterResources.Inp2;
        string key7 = meterResources.ToString();
        if (neadedMeterResources3[(object) key7] != null)
        {
          overrideParameter.Value += 32UL;
        }
        else
        {
          SortedList neadedMeterResources4 = this.MyMeter.NeadedMeterResources;
          meterResources = MeterResources.Out2;
          string key8 = meterResources.ToString();
          if (neadedMeterResources4[(object) key8] != null)
          {
            SortedList availableMeterResouces3 = this.MyMeter.AvailableMeterResouces;
            meterResources = MeterResources.VolToOut2;
            string key9 = meterResources.ToString();
            if (availableMeterResouces3[(object) key9] != null)
            {
              overrideParameter.Value += 64UL;
            }
            else
            {
              SortedList availableMeterResouces4 = this.MyMeter.AvailableMeterResouces;
              meterResources = MeterResources.EnToOut2;
              string key10 = meterResources.ToString();
              if (availableMeterResouces4[(object) key10] != null)
              {
                overrideParameter.Value += 80UL;
              }
              else
              {
                SortedList availableMeterResouces5 = this.MyMeter.AvailableMeterResouces;
                meterResources = MeterResources.CEnToOut2;
                string key11 = meterResources.ToString();
                if (availableMeterResouces5[(object) key11] != null)
                  overrideParameter.Value += 80UL;
                else
                  overrideParameter.Value += 16UL;
              }
            }
          }
          else
            overrideParameter.Value += 16UL;
        }
      }
      return true;
    }

    internal bool ChangeResourcesFromOverrides()
    {
      if (this.MyMeter.MyHandler.ExtendedTypeEditMode || this.OverridesList == null)
        return true;
      int num1 = 0;
      OverrideParameter overrides1 = (OverrideParameter) this.OverridesList[(object) OverrideID.BaseConfig];
      MeterResources meterResources;
      if (overrides1 != null)
      {
        ++num1;
        if (!(OverrideParameter.BaseConfigTable[(int) overrides1.Value].HeatAndCooling | OverrideParameter.BaseConfigTable[(int) overrides1.Value].Cooling))
        {
          SortedList availableMeterResouces = this.MyMeter.AvailableMeterResouces;
          meterResources = MeterResources.CEnergy;
          string key = meterResources.ToString();
          availableMeterResouces.Remove((object) key);
        }
      }
      OverrideParameter overrides2 = (OverrideParameter) this.OverridesList[(object) OverrideID.TarifFunction];
      if (overrides2 != null)
      {
        ++num1;
        if ((TarifSetup) overrides2.ParameterValue == TarifSetup.OFF)
        {
          SortedList availableMeterResouces = this.MyMeter.AvailableMeterResouces;
          meterResources = MeterResources.TAR_Energy;
          string key = meterResources.ToString();
          availableMeterResouces.Remove((object) key);
        }
      }
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      OverrideParameter overrides3 = (OverrideParameter) this.OverridesList[(object) OverrideID.ModuleType];
      if (overrides3 != null)
      {
        ++num1;
        ModuleTypeValues moduleTypeValues1 = (ModuleTypeValues) overrides3.Value;
        if (moduleTypeValues1 != 0)
        {
          this.MyMeter.AvailableMeterResouces.Remove((object) ModuleTypeValues.MBus.ToString());
          this.MyMeter.AvailableMeterResouces.Remove((object) ModuleTypeValues.ZRBus.ToString());
          this.MyMeter.AvailableMeterResouces.Remove((object) ModuleTypeValues.BusMask.ToString());
          ModuleTypeValues moduleTypeValues2 = moduleTypeValues1 & ModuleTypeValues.BusMask;
          string str1 = moduleTypeValues2.ToString();
          string str2 = str1;
          moduleTypeValues2 = ModuleTypeValues.NoValue;
          string str3 = moduleTypeValues2.ToString();
          if (str2 != str3)
          {
            MeterResource meterResource = new MeterResource(str1, (ushort) 0);
            this.MyMeter.AvailableMeterResouces.Add((object) str1, (object) meterResource);
          }
          SortedList availableMeterResouces1 = this.MyMeter.AvailableMeterResouces;
          moduleTypeValues2 = ModuleTypeValues.IO1Mask;
          string key1 = moduleTypeValues2.ToString();
          availableMeterResouces1.Remove((object) key1);
          SortedList availableMeterResouces2 = this.MyMeter.AvailableMeterResouces;
          moduleTypeValues2 = ModuleTypeValues.Inp1;
          string key2 = moduleTypeValues2.ToString();
          availableMeterResouces2.Remove((object) key2);
          SortedList availableMeterResouces3 = this.MyMeter.AvailableMeterResouces;
          moduleTypeValues2 = ModuleTypeValues.Out1;
          string key3 = moduleTypeValues2.ToString();
          availableMeterResouces3.Remove((object) key3);
          moduleTypeValues2 = moduleTypeValues1 & ModuleTypeValues.IO1Mask;
          empty2 = moduleTypeValues2.ToString();
          string str4 = empty2;
          moduleTypeValues2 = ModuleTypeValues.NoValue;
          string str5 = moduleTypeValues2.ToString();
          if (str4 != str5)
          {
            MeterResource meterResource = new MeterResource(empty2, (ushort) 0);
            this.MyMeter.AvailableMeterResouces.Add((object) empty2, (object) meterResource);
          }
          SortedList availableMeterResouces4 = this.MyMeter.AvailableMeterResouces;
          moduleTypeValues2 = ModuleTypeValues.IO2Mask;
          string key4 = moduleTypeValues2.ToString();
          availableMeterResouces4.Remove((object) key4);
          SortedList availableMeterResouces5 = this.MyMeter.AvailableMeterResouces;
          moduleTypeValues2 = ModuleTypeValues.Inp2;
          string key5 = moduleTypeValues2.ToString();
          availableMeterResouces5.Remove((object) key5);
          SortedList availableMeterResouces6 = this.MyMeter.AvailableMeterResouces;
          moduleTypeValues2 = ModuleTypeValues.Out2;
          string key6 = moduleTypeValues2.ToString();
          availableMeterResouces6.Remove((object) key6);
          moduleTypeValues2 = moduleTypeValues1 & ModuleTypeValues.IO2Mask;
          empty3 = moduleTypeValues2.ToString();
          string str6 = empty3;
          moduleTypeValues2 = ModuleTypeValues.NoValue;
          string str7 = moduleTypeValues2.ToString();
          if (str6 != str7)
          {
            MeterResource meterResource = new MeterResource(empty3, (ushort) 0);
            this.MyMeter.AvailableMeterResouces.Add((object) empty3, (object) meterResource);
          }
        }
      }
      OverrideParameter overrides4 = (OverrideParameter) this.OverridesList[(object) OverrideID.IO_Functions];
      if (overrides4 != null)
      {
        this.RestrictParameterFromModuleRessources(overrides4, empty2, empty3);
        int num2 = num1 + 1;
        this.MyMeter.InOut1Function = (InOutFunctions) ((long) overrides4.Value & 15L);
        this.MyMeter.InOut2Function = (InOutFunctions) ((long) overrides4.Value & 240L);
        if (this.MyMeter.InOut1Function != InOutFunctions.IO1_Input)
        {
          SortedList availableMeterResouces7 = this.MyMeter.AvailableMeterResouces;
          meterResources = MeterResources.Inp1On;
          string key7 = meterResources.ToString();
          availableMeterResouces7.Remove((object) key7);
          SortedList availableMeterResouces8 = this.MyMeter.AvailableMeterResouces;
          meterResources = MeterResources.DueDateInp1Value;
          string key8 = meterResources.ToString();
          availableMeterResouces8.Remove((object) key8);
          SortedList availableMeterResouces9 = this.MyMeter.AvailableMeterResouces;
          meterResources = MeterResources.DueDateInp1LastValue;
          string key9 = meterResources.ToString();
          availableMeterResouces9.Remove((object) key9);
          if (this.MyMeter.InOut1Function == InOutFunctions.IO1_Off)
          {
            SortedList availableMeterResouces10 = this.MyMeter.AvailableMeterResouces;
            meterResources = MeterResources.Out1On;
            string key10 = meterResources.ToString();
            availableMeterResouces10.Remove((object) key10);
          }
        }
        else
        {
          SortedList availableMeterResouces = this.MyMeter.AvailableMeterResouces;
          meterResources = MeterResources.Out1On;
          string key = meterResources.ToString();
          availableMeterResouces.Remove((object) key);
        }
        if (this.MyMeter.InOut2Function != InOutFunctions.IO2_Input)
        {
          SortedList availableMeterResouces11 = this.MyMeter.AvailableMeterResouces;
          meterResources = MeterResources.Inp2On;
          string key11 = meterResources.ToString();
          availableMeterResouces11.Remove((object) key11);
          SortedList availableMeterResouces12 = this.MyMeter.AvailableMeterResouces;
          meterResources = MeterResources.DueDateInp2Value;
          string key12 = meterResources.ToString();
          availableMeterResouces12.Remove((object) key12);
          SortedList availableMeterResouces13 = this.MyMeter.AvailableMeterResouces;
          meterResources = MeterResources.DueDateInp2LastValue;
          string key13 = meterResources.ToString();
          availableMeterResouces13.Remove((object) key13);
          if (this.MyMeter.InOut2Function == InOutFunctions.IO2_Off)
          {
            SortedList availableMeterResouces14 = this.MyMeter.AvailableMeterResouces;
            meterResources = MeterResources.Out2On;
            string key14 = meterResources.ToString();
            availableMeterResouces14.Remove((object) key14);
          }
        }
        else
        {
          SortedList availableMeterResouces = this.MyMeter.AvailableMeterResouces;
          meterResources = MeterResources.Out2On;
          string key = meterResources.ToString();
          availableMeterResouces.Remove((object) key);
        }
      }
      return true;
    }

    private void RestrictParameterFromModuleRessources(
      OverrideParameter TheOverrideParameter,
      string IO1RessourceString,
      string IO2RessourceString)
    {
      if (IO1RessourceString == ModuleTypeValues.NoValue.ToString())
        TheOverrideParameter.Value = TheOverrideParameter.Value & 18446744073709551600UL | 1UL;
      else if (IO1RessourceString == ModuleTypeValues.Inp1.ToString())
      {
        if (((long) TheOverrideParameter.Value & 15L) != 2L)
          TheOverrideParameter.Value = TheOverrideParameter.Value & 18446744073709551600UL | 1UL;
      }
      else if (IO1RessourceString == ModuleTypeValues.Out1.ToString() && ((long) TheOverrideParameter.Value & 15L) == 2L)
        TheOverrideParameter.Value = TheOverrideParameter.Value & 18446744073709551600UL | 1UL;
      if (IO2RessourceString == ModuleTypeValues.NoValue.ToString())
        TheOverrideParameter.Value = TheOverrideParameter.Value & 18446744073709551375UL | 16UL;
      else if (IO2RessourceString == ModuleTypeValues.Inp2.ToString())
      {
        if (((long) TheOverrideParameter.Value & 240L) == 32L)
          return;
        TheOverrideParameter.Value = TheOverrideParameter.Value & 18446744073709551375UL | 16UL;
      }
      else
      {
        if (!(IO2RessourceString == ModuleTypeValues.Out2.ToString()) || ((long) TheOverrideParameter.Value & 240L) != 32L)
          return;
        TheOverrideParameter.Value = TheOverrideParameter.Value & 18446744073709551375UL | 16UL;
      }
    }

    internal bool CopyValuesFromOverriedesToParameter()
    {
      if (this.MyMeter.MyMath.MyBaseSettings == null)
        return true;
      Decimal NewFactor1 = (Decimal) Math.Pow(10.0, (double) MeterMath.EnergyUnits[this.MyMeter.MyMath.MyBaseSettings.EnergyUnitIndex].AfterPointDigits);
      Decimal NewFactor2 = (Decimal) Math.Pow(2.0, (double) this.MyMeter.MyMath.MyBaseSettings.Energy_SumExpo) * NewFactor1;
      Decimal NewFactor3 = (Decimal) Math.Pow(10.0, (double) MeterMath.VolumeUnits[this.MyMeter.MyMath.MyBaseSettings.VolumeUnitIndex].AfterPointDigits);
      Decimal NewFactor4 = (Decimal) Math.Pow(2.0, (double) this.MyMeter.MyMath.MyBaseSettings.Vol_SumExpo) * NewFactor3;
      OverrideParameter overrides1 = (OverrideParameter) this.OverridesList[(object) OverrideID.EnergyActualValue];
      if (overrides1 != null)
      {
        Parameter allParameter = (Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.Waerme_EnergSum"];
        if (allParameter != null)
          allParameter.ValueEprom = (long) overrides1.GetParameterValue(NewFactor2);
      }
      OverrideParameter overrides2 = (OverrideParameter) this.OverridesList[(object) OverrideID.CEnergyActualValue];
      if (overrides2 != null)
      {
        Parameter allParameter = (Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.Kaelte_EnergSum"];
        if (allParameter != null)
          allParameter.ValueEprom = (long) overrides2.GetParameterValue(NewFactor2);
      }
      OverrideParameter overrides3 = (OverrideParameter) this.OverridesList[(object) OverrideID.VolumeActualValue];
      if (overrides3 != null)
      {
        Parameter allParameter = (Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.Vol_VolSum"];
        if (allParameter != null)
          allParameter.ValueEprom = (long) overrides3.GetParameterValue(NewFactor4);
      }
      OverrideParameter overrides4 = (OverrideParameter) this.OverridesList[(object) OverrideID.ReadingDate];
      if (overrides4 != null)
      {
        Parameter parameter = (Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.DueDate.ToString()];
        if (parameter != null)
          parameter.ValueEprom = (long) overrides4.Value;
      }
      OverrideParameter overrides5 = (OverrideParameter) this.OverridesList[(object) OverrideID.EnergyDueDateValue];
      if (overrides5 != null)
      {
        Parameter parameter = (Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.DueDateEnergy.ToString()];
        if (parameter != null)
          parameter.ValueEprom = (long) overrides5.GetParameterValue(NewFactor1);
      }
      OverrideParameter overrides6 = (OverrideParameter) this.OverridesList[(object) OverrideID.EnergyDueDateLastValue];
      if (overrides6 != null)
      {
        Parameter parameter = (Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.DueDateLastEnergy.ToString()];
        if (parameter != null)
          parameter.ValueEprom = (long) overrides6.GetParameterValue(NewFactor1);
      }
      OverrideParameter overrides7 = (OverrideParameter) this.OverridesList[(object) OverrideID.CEnergyDueDateValue];
      if (overrides7 != null)
      {
        Parameter parameter = (Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.DueDateCEnergy.ToString()];
        if (parameter != null)
          parameter.ValueEprom = (long) overrides7.GetParameterValue(NewFactor1);
      }
      OverrideParameter overrides8 = (OverrideParameter) this.OverridesList[(object) OverrideID.CEnergyDueDateLastValue];
      if (overrides8 != null)
      {
        Parameter parameter = (Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.DueDateLastCEnergy.ToString()];
        if (parameter != null)
          parameter.ValueEprom = (long) overrides8.GetParameterValue(NewFactor1);
      }
      OverrideParameter overrides9 = (OverrideParameter) this.OverridesList[(object) OverrideID.VolumeDueDateValue];
      if (overrides9 != null)
      {
        Parameter parameter = (Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.DueDateVolume.ToString()];
        if (parameter != null)
          parameter.ValueEprom = (long) overrides9.GetParameterValue(NewFactor3);
      }
      OverrideParameter overrides10 = (OverrideParameter) this.OverridesList[(object) OverrideID.VolumeDueDateLastValue];
      if (overrides10 != null)
      {
        Parameter parameter = (Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.DueDateLastVolume.ToString()];
        if (parameter != null)
          parameter.ValueEprom = (long) overrides10.GetParameterValue(NewFactor3);
      }
      OverrideParameter overrides11 = (OverrideParameter) this.OverridesList[(object) OverrideID.TarifEnergy0];
      if (overrides11 != null)
      {
        Parameter allParameter = (Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.TAR_EnergySumTar0"];
        if (allParameter != null)
          allParameter.ValueEprom = (long) overrides11.GetParameterValue(NewFactor2);
      }
      OverrideParameter overrides12 = (OverrideParameter) this.OverridesList[(object) OverrideID.TarifEnergy1];
      if (overrides12 != null)
      {
        Parameter allParameter = (Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.TAR_EnergySumTar1"];
        if (allParameter != null)
          allParameter.ValueEprom = (long) overrides12.GetParameterValue(NewFactor2);
      }
      Decimal NewFactor5 = (Decimal) Math.Pow(10.0, (double) MeterMath.InputUnits[this.MyMeter.MyMath.MyBaseSettings.Input1UnitIndex].AfterPointDigits);
      OverrideParameter overrides13 = (OverrideParameter) this.OverridesList[(object) OverrideID.Input1ActualValue];
      if (overrides13 != null)
      {
        Parameter allParameter = (Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.In1Display"];
        if (allParameter != null)
          allParameter.ValueEprom = (long) overrides13.GetParameterValue(NewFactor5);
      }
      OverrideParameter overrides14 = (OverrideParameter) this.OverridesList[(object) OverrideID.Input1DueDateValue];
      if (overrides14 != null)
      {
        Parameter parameter = (Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.DueDateInp1Value.ToString()];
        if (parameter != null)
          parameter.ValueEprom = (long) overrides14.GetParameterValue(NewFactor5);
      }
      OverrideParameter overrides15 = (OverrideParameter) this.OverridesList[(object) OverrideID.Input1DueDateLastValue];
      if (overrides15 != null)
      {
        Parameter parameter = (Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.DueDateInp1LastValue.ToString()];
        if (parameter != null)
          parameter.ValueEprom = (long) overrides15.GetParameterValue(NewFactor5);
      }
      OverrideParameter overrides16 = (OverrideParameter) this.OverridesList[(object) OverrideID.Input1IdNumber];
      if (overrides16 != null)
      {
        Parameter parameter = (Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.Inp1IdNumber.ToString()];
        if (parameter != null)
          parameter.ValueEprom = (long) overrides16.Value;
      }
      OverrideParameter overrides17 = (OverrideParameter) this.OverridesList[(object) OverrideID.Input1Type];
      if (overrides17 != null)
      {
        Parameter parameter = (Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.Inp1Type.ToString()];
        if (parameter != null)
          parameter.ValueEprom = (long) overrides17.Value;
      }
      Decimal NewFactor6 = (Decimal) Math.Pow(10.0, (double) MeterMath.InputUnits[this.MyMeter.MyMath.MyBaseSettings.Input2UnitIndex].AfterPointDigits);
      OverrideParameter overrides18 = (OverrideParameter) this.OverridesList[(object) OverrideID.Input2ActualValue];
      if (overrides18 != null)
      {
        Parameter allParameter = (Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.In2Display"];
        if (allParameter != null)
          allParameter.ValueEprom = (long) overrides18.GetParameterValue(NewFactor6);
      }
      OverrideParameter overrides19 = (OverrideParameter) this.OverridesList[(object) OverrideID.Input2DueDateValue];
      if (overrides19 != null)
      {
        Parameter parameter = (Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.DueDateInp2Value.ToString()];
        if (parameter != null)
          parameter.ValueEprom = (long) overrides19.GetParameterValue(NewFactor6);
      }
      OverrideParameter overrides20 = (OverrideParameter) this.OverridesList[(object) OverrideID.Input2DueDateLastValue];
      if (overrides20 != null)
      {
        Parameter parameter = (Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.DueDateInp2LastValue.ToString()];
        if (parameter != null)
          parameter.ValueEprom = (long) overrides20.GetParameterValue(NewFactor6);
      }
      OverrideParameter overrides21 = (OverrideParameter) this.OverridesList[(object) OverrideID.Input2IdNumber];
      if (overrides21 != null)
      {
        Parameter parameter = (Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.Inp2IdNumber.ToString()];
        if (parameter != null)
          parameter.ValueEprom = (long) overrides21.Value;
      }
      OverrideParameter overrides22 = (OverrideParameter) this.OverridesList[(object) OverrideID.Input2Type];
      if (overrides22 != null)
      {
        Parameter parameter = (Parameter) this.MyMeter.AllParametersByResource[(object) MeterResources.Inp2Type.ToString()];
        if (parameter != null)
          parameter.ValueEprom = (long) overrides22.Value;
      }
      return true;
    }
  }
}
