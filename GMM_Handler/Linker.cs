// Decompiled with JetBrains decompiler
// Type: GMM_Handler.Linker
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  internal class Linker
  {
    internal SortedList MapPointers;
    private Meter MyMeter;
    internal ArrayList LinkBlockList;
    internal ArrayList LinkPointerList;
    internal SortedList EpromCodeBlocksByName;
    internal ArrayList LinkerCodeBlockList;
    internal ArrayList EEPROM_Interval_Runtime = new ArrayList();
    internal ArrayList Event_Runtime = new ArrayList();
    internal ArrayList MBus_Runtime = new ArrayList();
    internal ArrayList Mesurement_Runtime = new ArrayList();
    internal ArrayList RAM_Runtime = new ArrayList();
    internal ArrayList Interval_Runtime = new ArrayList();
    internal ArrayList AllIntervallCodes = new ArrayList();
    internal ArrayList ReplaceParameters = new ArrayList();
    private const string JumpToEpromIntervalRuntime = "ToEprIntervallRuntime";
    private int StartOfRuntimeVars;
    private int RuntimeCodeStartAddress;
    private int FirstFreeRamAddress;

    public Linker(Meter MyMeterIn)
    {
      this.MyMeter = MyMeterIn;
      this.LinkBlockList = new ArrayList();
    }

    internal Linker Clone(Meter TheCloneMeter) => new Linker(TheCloneMeter);

    internal bool UpdateAdresses()
    {
      int blockStartAddress = this.MyMeter.MyEpromHeader.BlockStartAddress;
      foreach (LinkBlock linkBlock in this.LinkBlockList)
      {
        linkBlock.BlockStartAddress = blockStartAddress;
        if (!this.UpdateBlockAddresses(linkBlock.LinkObjList, ref blockStartAddress))
          return false;
        linkBlock.StartAddressOfNextBlock = blockStartAddress;
      }
      this.MyMeter.MyIdent.extEEPUsed = blockStartAddress;
      return true;
    }

    internal bool UpdateAdressesAtBlockRange(LinkBlock FromBlock, LinkBlock ToBlock)
    {
      int Address = FromBlock.BlockStartAddress;
      bool flag = true;
      foreach (LinkBlock linkBlock in this.LinkBlockList)
      {
        if (flag)
        {
          if (linkBlock == FromBlock)
          {
            flag = false;
          }
          else
          {
            Address = linkBlock.StartAddressOfNextBlock;
            continue;
          }
        }
        linkBlock.BlockStartAddress = Address;
        if (linkBlock == ToBlock)
          return true;
        if (!this.UpdateBlockAddresses(linkBlock.LinkObjList, ref Address))
          return false;
        linkBlock.StartAddressOfNextBlock = Address;
      }
      if (Address >= this.MyMeter.MyIdent.extEEPSize)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Out of eeprom space");
        return false;
      }
      this.MyMeter.MyIdent.extEEPUsed = Address;
      return true;
    }

    private bool UpdateBlockAddresses(ArrayList TheLinkObjList, ref int Address)
    {
      foreach (object theLinkObj in TheLinkObjList)
      {
        if (theLinkObj is Parameter)
        {
          if (!this.SetLinkAddress((LinkObj) theLinkObj, ref Address))
            return false;
        }
        else
        {
          foreach (LinkObj code in ((CodeBlock) theLinkObj).CodeList)
          {
            if (!this.SetLinkAddress(code, ref Address))
              return false;
          }
        }
      }
      this.MyMeter.MyIdent.extEEPUsed = Address;
      return true;
    }

    internal bool CreateParameterAddressLists()
    {
      this.MyMeter.AllEpromParametersByAddress = new SortedList();
      this.MyMeter.AllRamParametersByAddress = new SortedList();
      foreach (DictionaryEntry allParameter in this.MyMeter.AllParameters)
      {
        Parameter parameter = (Parameter) allParameter.Value;
        if (parameter.ExistOnEprom)
        {
          try
          {
            this.MyMeter.AllEpromParametersByAddress.Add((object) parameter.Address, (object) parameter);
          }
          catch
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal eprom parameter");
            return false;
          }
        }
        if (parameter.ExistOnCPU)
        {
          try
          {
            if (parameter.AddressCPU >= 0)
              this.MyMeter.AllRamParametersByAddress.Add((object) parameter.AddressCPU, (object) parameter);
          }
          catch
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal ram parameter");
            return false;
          }
        }
      }
      return true;
    }

    internal bool UpdateMBusAndFunctionTableAdresses()
    {
      int blockStartAddress = this.MyMeter.MyMBusList.BlockStartAddress;
      foreach (LinkObj code in ((CodeBlock) this.MyMeter.MyMBusList.LinkObjList[0]).CodeList)
      {
        if (!this.SetLinkAddress(code, ref blockStartAddress))
          return false;
      }
      this.MyMeter.MyFunctionTable.BlockStartAddress = blockStartAddress;
      foreach (LinkObj code in ((CodeBlock) this.MyMeter.MyFunctionTable.LinkObjList[0]).CodeList)
      {
        if (!this.SetLinkAddress(code, ref blockStartAddress))
          return false;
      }
      this.MyMeter.MyLoggerStore.BlockStartAddress = blockStartAddress;
      return true;
    }

    private bool SetLinkAddress(LinkObj TheLinkObj, ref int Address)
    {
      if (TheLinkObj.Size < 0)
      {
        Debug.Write("Uninitialised size");
        return false;
      }
      TheLinkObj.Address = Address;
      Address += TheLinkObj.Size;
      return true;
    }

    internal bool AreBlockAdressesUnchanged(byte[] CompareEEprom)
    {
      return this.CheckBlockAdr("EEP_Header.EEP_HEADER_RamParamBlockAdr", (long) this.MyMeter.MyRamParameter.BlockStartAddress, CompareEEprom) && this.CheckBlockAdr("EEP_Header.EEP_HEADER_BackupBlockAdr", (long) this.MyMeter.MyBackup.BlockStartAddress, CompareEEprom) && this.CheckBlockAdr("EEP_Header.EEP_HEADER_FixedParamAdr", (long) this.MyMeter.MyFixedParameter.BlockStartAddress, CompareEEprom) && this.CheckBlockAdr("EEP_Header.EEP_HEADER_WritePermTableAdr", (long) this.MyMeter.MyWritePermTable.BlockStartAddress, CompareEEprom) && this.CheckBlockAdr("EEP_Header.EEP_HEADER_DispBlockAdr", (long) this.MyMeter.MyDisplayCode.BlockStartAddress, CompareEEprom) && this.CheckBlockAdr("EEP_Header.EEP_HEADER_RuntimeVarsAdr", (long) this.MyMeter.MyRuntimeVars.BlockStartAddress, CompareEEprom) && this.CheckBlockAdr("EEP_Header.EEP_HEADER_RuntimeCodeAdr", (long) this.MyMeter.MyRuntimeCode.BlockStartAddress, CompareEEprom) && this.CheckBlockAdr("EEP_Header.EEP_HEADER_RuntimeCodeAdr", (long) this.MyMeter.MyRuntimeCode.BlockStartAddress, CompareEEprom) && this.CheckBlockAdr("EEP_Header.EEP_HEADER_RuntimeCodeAdr", (long) this.MyMeter.MyRuntimeCode.BlockStartAddress, CompareEEprom) && this.CheckBlockAdr("EEP_Header.EEP_HEADER_EpromVarsAdr", (long) this.MyMeter.MyEpromVars.BlockStartAddress, CompareEEprom) && this.CheckBlockAdr("EEP_Header.EEP_HEADER_ParamBlockAdr", (long) this.MyMeter.MyEpromParameter.BlockStartAddress, CompareEEprom) && this.CheckBlockAdr("EEP_Header.EEP_HEADER_EpromRuntimeAdr", (long) this.MyMeter.MyEpromRuntime.BlockStartAddress, CompareEEprom) && this.CheckBlockAdr("EEP_Header.EEP_HEADER_MBusBlockAdr", (long) this.MyMeter.MyMBusList.BlockStartAddress, CompareEEprom) && this.CheckBlockAdr("EEP_Header.EEP_HEADER_FunctionTableAdr", (long) this.MyMeter.MyFunctionTable.BlockStartAddress, CompareEEprom);
    }

    internal bool CheckBlockAdr(string VarName, long BlockAdr, byte[] CompareEEprom)
    {
      if (this.MyMeter.MyHandler.DisableChecks)
        return true;
      bool flag = true;
      Parameter allParameter = (Parameter) this.MyMeter.AllParameters[(object) VarName];
      if (allParameter.GetValueFromMap(CompareEEprom) != BlockAdr)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Segment size changed! Adr. name: " + allParameter.FullName);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Database error");
        flag = false;
      }
      return flag;
    }

    internal bool ReloadRuntimeVarsRamAdresses()
    {
      try
      {
        this.StartOfRuntimeVars = (int) ((Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.RuntimeVarsLocation"]).ValueEprom;
        this.RuntimeCodeStartAddress = (int) ((Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.MesurementCodeLocation"]).ValueEprom;
        if (this.RuntimeCodeStartAddress == 0)
          this.RuntimeCodeStartAddress = (int) ((Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.RuntimeCodeLocation"]).ValueEprom;
        int startOfRuntimeVars = this.StartOfRuntimeVars;
        foreach (Parameter linkObj in this.MyMeter.MyRuntimeVars.LinkObjList)
        {
          linkObj.AddressCPU = startOfRuntimeVars;
          linkObj.ExistOnCPU = true;
          startOfRuntimeVars += linkObj.Size;
        }
      }
      catch
      {
        ZR_ClassLibMessages.AddWarning("Ram address generation error");
        return false;
      }
      return true;
    }

    internal bool SetAddressReferences()
    {
      SortedList allParameters = this.MyMeter.AllParameters;
      try
      {
        ((Parameter) allParameters[(object) "EEP_Header.EEP_HEADER_RamParamBlockAdr"]).ValueEprom = (long) this.MyMeter.MyRamParameter.BlockStartAddress;
        ((Parameter) allParameters[(object) "EEP_Header.EEP_HEADER_BackupBlockAdr"]).ValueEprom = (long) this.MyMeter.MyBackup.BlockStartAddress;
        ((Parameter) allParameters[(object) "EEP_Header.EEP_HEADER_FixedParamAdr"]).ValueEprom = (long) this.MyMeter.MyFixedParameter.BlockStartAddress;
        ((Parameter) allParameters[(object) "EEP_Header.EEP_HEADER_WritePermTableAdr"]).ValueEprom = (long) this.MyMeter.MyWritePermTable.BlockStartAddress;
        ((Parameter) allParameters[(object) "EEP_Header.EEP_HEADER_DispBlockAdr"]).ValueEprom = (long) this.MyMeter.MyDisplayCode.BlockStartAddress;
        ((Parameter) allParameters[(object) "EEP_Header.EEP_HEADER_RuntimeVarsAdr"]).ValueEprom = (long) this.MyMeter.MyRuntimeVars.BlockStartAddress;
        ((Parameter) allParameters[(object) "EEP_Header.EEP_HEADER_RuntimeCodeAdr"]).ValueEprom = (long) this.MyMeter.MyRuntimeCode.BlockStartAddress;
        ((Parameter) allParameters[(object) "EEP_Header.EEP_HEADER_EpromVarsAdr"]).ValueEprom = (long) this.MyMeter.MyEpromVars.BlockStartAddress;
        ((Parameter) allParameters[(object) "EEP_Header.EEP_HEADER_ParamBlockAdr"]).ValueEprom = (long) this.MyMeter.MyEpromParameter.BlockStartAddress;
        ((Parameter) allParameters[(object) "EEP_Header.EEP_HEADER_EpromRuntimeAdr"]).ValueEprom = (long) this.MyMeter.MyEpromRuntime.BlockStartAddress;
        ((Parameter) allParameters[(object) "EEP_Header.EEP_HEADER_MBusBlockAdr"]).ValueEprom = (long) this.MyMeter.MyMBusList.BlockStartAddress;
        ((Parameter) allParameters[(object) "EEP_Header.EEP_HEADER_FunctionTableAdr"]).ValueEprom = (long) this.MyMeter.MyFunctionTable.BlockStartAddress;
        int mapPointer = (int) this.MapPointers[(object) "CSTACK"];
        int index = 0;
        while (index < this.MyMeter.AllRamParametersByAddress.Count && ((Parameter) this.MyMeter.AllRamParametersByAddress.GetByIndex(index)).AddressCPU < mapPointer)
          ++index;
        Parameter byIndex = (Parameter) this.MyMeter.AllRamParametersByAddress.GetByIndex(index - 1);
        this.StartOfRuntimeVars = byIndex.AddressCPU + byIndex.Size;
        this.FirstFreeRamAddress = this.StartOfRuntimeVars;
        ((Parameter) allParameters[(object) "DefaultFunction.RuntimeVarsLocation"]).ValueEprom = (long) this.FirstFreeRamAddress;
        foreach (Parameter linkObj in this.MyMeter.MyRuntimeVars.LinkObjList)
        {
          linkObj.AddressCPU = this.FirstFreeRamAddress;
          this.FirstFreeRamAddress += linkObj.Size;
          linkObj.ExistOnCPU = true;
          this.MyMeter.AllRamParametersByAddress.Add((object) linkObj.AddressCPU, (object) linkObj);
        }
        this.RuntimeCodeStartAddress = this.FirstFreeRamAddress;
        if (this.Mesurement_Runtime.Count > 0)
        {
          ((Parameter) allParameters[(object) "DefaultFunction.MesurementCodeLocation"]).ValueEprom = (long) this.RuntimeCodeStartAddress;
          foreach (CodeBlock codeBlock in this.Mesurement_Runtime)
          {
            foreach (LinkObj code in codeBlock.CodeList)
              this.FirstFreeRamAddress += code.Size;
          }
        }
        else
          ((Parameter) allParameters[(object) "DefaultFunction.MesurementCodeLocation"]).ValueEprom = 0L;
        ((Parameter) allParameters[(object) "DefaultFunction.RuntimeCodeLocation"]).ValueEprom = (long) this.FirstFreeRamAddress;
        this.FirstFreeRamAddress = this.RuntimeCodeStartAddress;
        if (this.RAM_Runtime.Count > 0 || this.Interval_Runtime.Count > 0 || this.EEPROM_Interval_Runtime.Count > 0)
        {
          foreach (CodeBlock linkObj in this.MyMeter.MyRuntimeCode.LinkObjList)
          {
            foreach (LinkObj code in linkObj.CodeList)
              this.FirstFreeRamAddress += code.Size;
          }
        }
        else
          ((Parameter) allParameters[(object) "DefaultFunction.RuntimeCodeLocation"]).ValueEprom = 0L;
        int addressCpu = ((Parameter) this.MyMeter.MyFixedParameter.LinkObjList[0]).AddressCPU;
        int num1 = addressCpu + this.MyMeter.MyWritePermTable.BlockStartAddress - this.MyMeter.MyFixedParameter.BlockStartAddress;
        ((Parameter) allParameters[(object) "DefaultFunction.WritePermTableLocation"]).ValueEprom = (long) num1;
        int num2 = addressCpu - ((LinkObj) this.MyMeter.MyFixedParameter.LinkObjList[0]).Address;
        foreach (Parameter linkObj in this.MyMeter.MyFixedParameter.LinkObjList)
        {
          if (linkObj.AddressCPU - linkObj.Address != num2 && linkObj.AddressCPU >= 0)
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "FixedParameter Eprom-RAM Offset Error! VarName:" + linkObj.Name);
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Linker error:");
          }
        }
        int num3 = ((Parameter) this.MyMeter.MyBackup.LinkObjList[0]).AddressCPU - ((LinkObj) this.MyMeter.MyBackup.LinkObjList[0]).Address;
        foreach (Parameter linkObj in this.MyMeter.MyBackup.LinkObjList)
        {
          if (linkObj.AddressCPU - linkObj.Address != num3)
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "BackupParameter Eprom-RAM Offset Error! VarName:" + linkObj.Name);
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Linker error:");
          }
        }
        int num4 = ((Parameter) this.MyMeter.MyRamParameter.LinkObjList[0]).AddressCPU - ((LinkObj) this.MyMeter.MyRamParameter.LinkObjList[0]).Address;
        foreach (Parameter linkObj in this.MyMeter.MyRamParameter.LinkObjList)
        {
          if (linkObj.AddressCPU - linkObj.Address != num4)
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "RamParameter Eprom-RAM Offset Error! VarName:" + linkObj.Name);
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Linker error:");
          }
        }
      }
      catch
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Linker error: Illegal address reference");
        return false;
      }
      return true;
    }

    internal bool LinkAllPointers()
    {
      foreach (LinkPointer linkPointer in this.LinkPointerList)
      {
        CodeObject pointerObject = linkPointer.PointerObject;
        if (pointerObject.CodeValue.IndexOf('.') < 0)
        {
          if (pointerObject.CodeValue == "ToEprIntervallRuntime")
          {
            CodeObject code = (CodeObject) ((CodeBlock) this.EEPROM_Interval_Runtime[0]).CodeList[0];
            if (!this.SetPointerValue(pointerObject, (object) code))
              return false;
          }
          else
          {
            Parameter TheDestinationObject1 = ((Parameter) this.MyMeter.AllParameters[(object) pointerObject.CodeValue] ?? (Parameter) this.MyMeter.AllParameters[(object) ("DefaultFunction." + pointerObject.CodeValue)] ?? (Parameter) this.MyMeter.AllParameters[(object) ("EEP_Header." + pointerObject.CodeValue)]) ?? (Parameter) this.MyMeter.AllParametersByResource[(object) pointerObject.CodeValue];
            if (TheDestinationObject1 != null)
            {
              if (!this.SetPointerValue(pointerObject, (object) TheDestinationObject1))
                return false;
            }
            else
            {
              Function function = (Function) this.MyMeter.MyFunctionTable.FunctionListByNumber[(object) (ushort) pointerObject.FunctionNumber];
              Parameter TheDestinationObject2 = (Parameter) function.ParameterListByName[(object) pointerObject.CodeValue];
              if (TheDestinationObject2 != null)
              {
                if (!this.SetPointerValue(pointerObject, (object) TheDestinationObject2))
                  return false;
              }
              else
              {
                CodeBlock codeBlock = (CodeBlock) function.EpromCodeBlocksByName[(object) pointerObject.CodeValue];
                if (codeBlock != null)
                {
                  if (codeBlock.CodeList.Count > 0)
                  {
                    CodeObject code = (CodeObject) codeBlock.CodeList[0];
                    if (!this.SetPointerValue(pointerObject, (object) code))
                      return false;
                  }
                }
                else
                {
                  MenuItem menuItem = (MenuItem) function.MenuListByName[(object) pointerObject.CodeValue];
                  if (menuItem != null && menuItem.DisplayCodeBlocks.Count > 0)
                  {
                    CodeBlock displayCodeBlock = (CodeBlock) menuItem.DisplayCodeBlocks[0];
                    if (displayCodeBlock != null && displayCodeBlock.CodeList.Count > 0)
                    {
                      CodeObject code = (CodeObject) displayCodeBlock.CodeList[0];
                      if (!this.SetPointerValue(pointerObject, (object) code))
                        return false;
                    }
                  }
                  else if (pointerObject.CodeValue.StartsWith("0x"))
                  {
                    pointerObject.CodeValueCompiled = long.Parse(pointerObject.CodeValue.Substring(2), NumberStyles.HexNumber);
                    this.MyMeter.MyCompiler.GenerateCodeFromCodeObject(pointerObject);
                  }
                  else if (pointerObject.CodeValue == "Revision4820")
                  {
                    CodeObject TheDestinationObject3 = new CodeObject(0);
                    TheDestinationObject3.Address = 100;
                    if (!this.SetPointerValue(pointerObject, (object) TheDestinationObject3))
                      return false;
                  }
                  else
                  {
                    this.MyMeter.MyHandler.AddErrorPointMessage("Pointer not found: '" + pointerObject.CodeValue + "'");
                    return false;
                  }
                }
              }
            }
          }
        }
        else
        {
          Parameter allParameter = (Parameter) this.MyMeter.AllParameters[(object) pointerObject.CodeValue];
          if (allParameter != null)
          {
            if (!this.SetPointerValue(pointerObject, (object) allParameter))
              return false;
          }
          else
          {
            Parameter aliasParameter = (Parameter) this.MyMeter.AliasParameters[(object) pointerObject.CodeValue];
            if (aliasParameter != null)
            {
              if (!this.SetPointerValue(pointerObject, (object) aliasParameter))
                return false;
            }
            else
            {
              MenuItem menuItem = (MenuItem) this.MyMeter.MyDisplayCode.AllMenusByName[(object) pointerObject.CodeValue];
              if (menuItem != null)
              {
                CodeObject code = (CodeObject) ((CodeBlock) menuItem.DisplayCodeBlocks[0]).CodeList[0];
                if (!this.SetPointerValue(pointerObject, (object) code))
                  return false;
              }
              else
              {
                CodeBlock codeBlock = (CodeBlock) this.EpromCodeBlocksByName[(object) pointerObject.CodeValue];
                if (codeBlock != null)
                {
                  if (codeBlock.CodeList.Count > 0)
                  {
                    CodeObject code = (CodeObject) codeBlock.CodeList[0];
                    if (!this.SetPointerValue(pointerObject, (object) code))
                      return false;
                  }
                  else
                  {
                    ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Linker error: Unknown destination object");
                    return false;
                  }
                }
              }
            }
          }
        }
      }
      return true;
    }

    internal bool LoadAllPointersFromEprom()
    {
      foreach (LinkPointer linkPointer in this.LinkPointerList)
      {
        CodeObject pointerObject = linkPointer.PointerObject;
        int address = pointerObject.Address;
        pointerObject.CodeValueCompiled = (long) ParameterService.GetFromByteArray_ushort(this.MyMeter.Eprom, ref address);
        this.MyMeter.MyCompiler.GenerateCodeFromCodeObject(pointerObject);
      }
      return true;
    }

    private bool SetPointerValue(CodeObject TheLinkObj, object TheDestinationObject)
    {
      int num;
      if (TheDestinationObject is Parameter)
      {
        Parameter parameter = (Parameter) TheDestinationObject;
        if (TheLinkObj.CodeType == CodeObject.CodeTypes.ePTR)
        {
          if (!parameter.ExistOnEprom)
            return this.MyMeter.MyHandler.AddErrorPointMessage("ePTR to anknown parameter: " + parameter.FullName);
          num = parameter.Address;
        }
        else
        {
          if (!parameter.ExistOnCPU)
            return this.MyMeter.MyHandler.AddErrorPointMessage("iPTR to anknown parameter: " + parameter.FullName);
          num = parameter.AddressCPU;
        }
      }
      else
        num = ((LinkObj) TheDestinationObject).Address;
      TheLinkObj.CodeValueCompiled = (long) num;
      TheLinkObj.LinkByteList = new byte[2];
      TheLinkObj.LinkByteList[0] = (byte) num;
      TheLinkObj.LinkByteList[1] = (byte) (num >> 8);
      return true;
    }

    internal bool GenerateBlockList()
    {
      for (int index = 0; index < this.MyMeter.BlockLinkOrder.Count; ++index)
      {
        switch (((BlockLinkDefines) this.MyMeter.BlockLinkOrder[index]).BlockType)
        {
          case LinkBlockTypes.EpromHeader:
            if (this.MyMeter.MyEpromHeader == null || index != 0 || this.LinkBlockList.Count != 1)
              goto default;
            else
              break;
          case LinkBlockTypes.EpromVars:
            if (this.MyMeter.MyEpromVars == null)
            {
              this.MyMeter.MyEpromVars = new EpromVars(this.MyMeter);
              this.LinkBlockList.Add((object) this.MyMeter.MyEpromVars);
              break;
            }
            goto default;
          case LinkBlockTypes.EpromParameter:
            if (this.MyMeter.MyEpromParameter == null)
            {
              this.MyMeter.MyEpromParameter = new EpromParameter(this.MyMeter);
              this.LinkBlockList.Add((object) this.MyMeter.MyEpromParameter);
              break;
            }
            goto default;
          case LinkBlockTypes.RuntimeVars:
            if (this.MyMeter.MyRuntimeVars == null)
            {
              this.MyMeter.MyRuntimeVars = new RuntimeVars(this.MyMeter);
              this.LinkBlockList.Add((object) this.MyMeter.MyRuntimeVars);
              break;
            }
            goto default;
          case LinkBlockTypes.RamParameter:
            if (this.MyMeter.MyRamParameter == null)
            {
              this.MyMeter.MyRamParameter = new RamParameter(this.MyMeter);
              this.LinkBlockList.Add((object) this.MyMeter.MyRamParameter);
              break;
            }
            goto default;
          case LinkBlockTypes.Backup:
            if (this.MyMeter.MyBackup == null)
            {
              this.MyMeter.MyBackup = new Backup(this.MyMeter);
              this.LinkBlockList.Add((object) this.MyMeter.MyBackup);
              break;
            }
            goto default;
          case LinkBlockTypes.FixedParameter:
            if (this.MyMeter.MyFixedParameter == null)
            {
              this.MyMeter.MyFixedParameter = new FixedParameter(this.MyMeter);
              this.LinkBlockList.Add((object) this.MyMeter.MyFixedParameter);
              break;
            }
            goto default;
          case LinkBlockTypes.MBusList:
            if (this.MyMeter.MyMBusList == null)
              this.MyMeter.MyMBusList = new MBusList(this.MyMeter);
            this.LinkBlockList.Add((object) this.MyMeter.MyMBusList);
            break;
          case LinkBlockTypes.LoggerStore:
            if (this.MyMeter.MyLoggerStore == null)
            {
              this.MyMeter.MyLoggerStore = new LoggerStore(this.MyMeter);
              this.LinkBlockList.Add((object) this.MyMeter.MyLoggerStore);
              break;
            }
            goto default;
          case LinkBlockTypes.WritePermTable:
            if (this.MyMeter.MyWritePermTable == null)
            {
              this.MyMeter.MyWritePermTable = new WritePermTable(this.MyMeter);
              this.LinkBlockList.Add((object) this.MyMeter.MyWritePermTable);
              break;
            }
            goto default;
          case LinkBlockTypes.DisplayCode:
            if (this.MyMeter.MyDisplayCode == null)
            {
              this.MyMeter.MyDisplayCode = new DisplayCode(this.MyMeter);
              this.LinkBlockList.Add((object) this.MyMeter.MyDisplayCode);
              break;
            }
            goto default;
          case LinkBlockTypes.RuntimeCode:
            if (this.MyMeter.MyRuntimeCode == null)
            {
              this.MyMeter.MyRuntimeCode = new RuntimeCode(this.MyMeter);
              this.LinkBlockList.Add((object) this.MyMeter.MyRuntimeCode);
              break;
            }
            goto default;
          case LinkBlockTypes.EpromRuntime:
            if (this.MyMeter.MyEpromRuntime == null)
            {
              this.MyMeter.MyEpromRuntime = new EpromRuntime(this.MyMeter);
              this.LinkBlockList.Add((object) this.MyMeter.MyEpromRuntime);
              break;
            }
            goto default;
          case LinkBlockTypes.FunctionTable:
            if (this.MyMeter.MyFunctionTable != null)
            {
              this.LinkBlockList.Add((object) this.MyMeter.MyFunctionTable);
              break;
            }
            goto default;
          default:
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Linker error: Illegal linker block btate");
            return false;
        }
      }
      return true;
    }

    internal bool IncludeAllFunctions()
    {
      SortedList fullLoadedFunctions = this.MyMeter.MyHandler.MyLoadedFunctions.FullLoadedFunctions;
      this.MyMeter.MyFunctionTable.FunctionList = new ArrayList();
      this.MyMeter.MyFunctionTable.FunctionListByName = new SortedList();
      this.MyMeter.MyFunctionTable.FunctionListByNumber = new SortedList();
      for (int index1 = 0; index1 < this.MyMeter.MyFunctionTable.FunctionNumbersList.Count; ++index1)
      {
        Function function1 = (Function) null;
        if (this.MyMeter.ConfigLoggers != null)
        {
          int index2 = this.MyMeter.ConfigLoggers.IndexOfKey((uint) (ushort) this.MyMeter.MyFunctionTable.FunctionNumbersList[index1]);
          if (index2 >= 0)
            function1 = this.MyMeter.ConfigLoggers.Values[index2];
        }
        if (function1 == null)
          function1 = (Function) fullLoadedFunctions[this.MyMeter.MyFunctionTable.FunctionNumbersList[index1]];
        Function function2 = function1.Clone(this.MyMeter);
        this.MyMeter.MyFunctionTable.FunctionList.Add((object) function2);
        this.MyMeter.MyFunctionTable.FunctionListByName.Add((object) function2.Name, (object) function2);
        this.MyMeter.MyFunctionTable.FunctionListByNumber.Add((object) function2.Number, (object) function2);
      }
      return true;
    }

    internal bool AddMapVariables()
    {
      this.MapPointers = new SortedList();
      foreach (DataBaseAccess.MapEntry mapEntry in this.MyMeter.Map)
      {
        bool flag = false;
        Parameter parameter = ((Parameter) this.MyMeter.AllParameters[(object) mapEntry.Name] ?? (Parameter) this.MyMeter.AllParameters[(object) ("DefaultFunction." + mapEntry.Name)]) ?? (Parameter) this.MyMeter.AllParameters[(object) ("EEP_Header." + mapEntry.Name)];
        if (parameter == null)
        {
          parameter = (Parameter) this.MyMeter.AllParameters[(object) ("DefaultFunction." + mapEntry.Name + "_1")];
          flag = true;
        }
        if (parameter == null)
        {
          if (mapEntry.ByteSize == (short) 0)
          {
            this.MapPointers.Add((object) mapEntry.Name, (object) mapEntry.Address);
            continue;
          }
          parameter = new Parameter(mapEntry.Name, (int) mapEntry.ByteSize, LinkBlockTypes.Static);
          parameter.FullName = mapEntry.Name;
          this.MyMeter.AllParameters.Add((object) mapEntry.Name, (object) parameter);
        }
        else if (parameter.Size != (int) mapEntry.ByteSize)
        {
          if (!flag)
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Parameter size difference! Parameter name: " + parameter.FullName);
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Linker error:");
            continue;
          }
          continue;
        }
        parameter.AddressCPU = mapEntry.Address;
        parameter.ExistOnCPU = true;
      }
      return true;
    }

    internal bool GenerateObjectLists()
    {
      this.EpromCodeBlocksByName = new SortedList();
      this.MyMeter.AllParametersByResource = new SortedList();
      this.MyMeter.MyMBusList.AllMBusParameters = new SortedList();
      for (int index = 0; index < this.MyMeter.MyFunctionTable.FunctionList.Count; ++index)
      {
        Function function = (Function) this.MyMeter.MyFunctionTable.FunctionList[index];
        function.ParameterListByName = new SortedList();
        function.EpromCodeBlocksByName = new SortedList();
        function.MenuListByName = new SortedList();
        foreach (Parameter parameter in function.ParameterList)
        {
          bool flag = true;
          switch (parameter.BlockMark)
          {
            case LinkBlockTypes.EpromHeader:
              if (flag)
              {
                string key = function.Name + "." + parameter.Name;
                parameter.FullName = key;
                this.MyMeter.AllParameters.Add((object) key, (object) parameter);
                if (function.AliasName != null)
                  this.MyMeter.AliasParameters.Add((object) (function.AliasName + "." + parameter.Name), (object) parameter);
                function.ParameterListByName.Add((object) parameter.Name, (object) parameter);
                if (parameter.MeterResource != null && parameter.MeterResource.Length > 0)
                {
                  if (this.MyMeter.AllParametersByResource.ContainsKey((object) parameter.MeterResource))
                  {
                    if (!this.MyMeter.MyHandler.BaseTypeEditMode)
                      throw new Exception("Table Datalogger wurde nicht geöffnet");
                  }
                  else
                    this.MyMeter.AllParametersByResource.Add((object) parameter.MeterResource, (object) parameter);
                }
              }
              continue;
            case LinkBlockTypes.EpromVars:
              this.MyMeter.MyEpromVars.LinkObjList.Add((object) parameter);
              goto case LinkBlockTypes.EpromHeader;
            case LinkBlockTypes.EpromParameter:
              this.MyMeter.MyEpromParameter.LinkObjList.Add((object) parameter);
              goto case LinkBlockTypes.EpromHeader;
            case LinkBlockTypes.RuntimeVars:
              this.MyMeter.MyRuntimeVars.LinkObjList.Add((object) parameter);
              goto case LinkBlockTypes.EpromHeader;
            case LinkBlockTypes.RamParameter:
              this.MyMeter.MyRamParameter.LinkObjList.Add((object) parameter);
              goto case LinkBlockTypes.EpromHeader;
            case LinkBlockTypes.Backup:
              this.MyMeter.MyBackup.LinkObjList.Add((object) parameter);
              goto case LinkBlockTypes.EpromHeader;
            case LinkBlockTypes.Static:
              flag = false;
              parameter.FullName = parameter.Name;
              this.MyMeter.AllParameters.Add((object) parameter.Name, (object) parameter);
              goto case LinkBlockTypes.EpromHeader;
            case LinkBlockTypes.FixedParameter:
              this.MyMeter.MyFixedParameter.LinkObjList.Add((object) parameter);
              goto case LinkBlockTypes.EpromHeader;
            case LinkBlockTypes.LoggerStore:
              flag = false;
              goto case LinkBlockTypes.EpromHeader;
            case LinkBlockTypes.WritePermTable:
              this.MyMeter.MyWritePermTable.LinkObjList.Add((object) parameter);
              goto case LinkBlockTypes.EpromHeader;
            case LinkBlockTypes.NotLinkedReplaceParameter:
              flag = false;
              this.ReplaceParameters.Add((object) parameter);
              goto case LinkBlockTypes.EpromHeader;
            default:
              flag = false;
              ZR_ClassLibMessages.AddWarning("Parameter at illegal block");
              goto case LinkBlockTypes.EpromHeader;
          }
        }
        foreach (CodeBlock runtimeCodeBlock in function.RuntimeCodeBlockList)
        {
          string key = function.Name + "." + runtimeCodeBlock.CodeSequenceName;
          bool flag = false;
          switch (runtimeCodeBlock.CodeSequenceType)
          {
            case CodeBlock.CodeSequenceTypes.RAM_Runtime:
              this.RAM_Runtime.Add((object) runtimeCodeBlock);
              break;
            case CodeBlock.CodeSequenceTypes.Interval_Runtime:
              this.Interval_Runtime.Add((object) runtimeCodeBlock);
              this.AllIntervallCodes.Add((object) runtimeCodeBlock);
              break;
            case CodeBlock.CodeSequenceTypes.Event_Runtime:
              this.Event_Runtime.Add((object) runtimeCodeBlock);
              flag = true;
              break;
            case CodeBlock.CodeSequenceTypes.RESET_Runtime:
              this.MyMeter.MyEpromRuntime.LinkObjList.Add((object) runtimeCodeBlock);
              flag = true;
              break;
            case CodeBlock.CodeSequenceTypes.EEPROM_Interval_Runtime:
              this.EEPROM_Interval_Runtime.Add((object) runtimeCodeBlock);
              this.AllIntervallCodes.Add((object) runtimeCodeBlock);
              flag = true;
              break;
            case CodeBlock.CodeSequenceTypes.Mesurement_Runtime:
              this.Mesurement_Runtime.Add((object) runtimeCodeBlock);
              break;
            case CodeBlock.CodeSequenceTypes.MBus_Runtime:
              this.MBus_Runtime.Add((object) runtimeCodeBlock);
              flag = true;
              break;
            default:
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Linker error: Code on illegal block");
              break;
          }
          if (flag)
          {
            function.EpromCodeBlocksByName.Add((object) runtimeCodeBlock.CodeSequenceName, (object) runtimeCodeBlock);
            this.EpromCodeBlocksByName.Add((object) key, (object) runtimeCodeBlock);
            if (function.AliasName != null && function.AliasName.Length > 0)
              this.EpromCodeBlocksByName.Add((object) (function.AliasName + "." + runtimeCodeBlock.CodeSequenceName), (object) runtimeCodeBlock);
          }
        }
        foreach (MenuItem menu in function.MenuList)
        {
          function.MenuListByName.Add((object) menu.MenuName, (object) menu);
          foreach (CodeBlock displayCodeBlock in menu.DisplayCodeBlocks)
          {
            if (displayCodeBlock.FrameType == FrameTypes.None)
            {
              switch (displayCodeBlock.CodeSequenceType)
              {
                case CodeBlock.CodeSequenceTypes.Displaycode:
                  this.MyMeter.MyDisplayCode.LinkObjList.Add((object) displayCodeBlock);
                  break;
                case CodeBlock.CodeSequenceTypes.InlineRuntimecode:
                  this.MyMeter.MyDisplayCode.LinkObjList.Add((object) displayCodeBlock);
                  this.AddRuntimeEndCode(displayCodeBlock, (int) menu.FunctionNumber);
                  break;
                case CodeBlock.CodeSequenceTypes.Brunch:
                  this.MyMeter.MyDisplayCode.LinkObjList.Add((object) displayCodeBlock);
                  break;
                default:
                  ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Linker error: Code on illegal block");
                  break;
              }
            }
            else
              this.MyMeter.MyDisplayCode.LinkObjList.Add((object) displayCodeBlock);
          }
        }
      }
      foreach (Parameter replaceParameter in this.ReplaceParameters)
      {
        Parameter allParameter = (Parameter) this.MyMeter.AllParameters[(object) ("DefaultFunction." + replaceParameter.Name)];
        allParameter.DifVifsByRes = replaceParameter.DifVifsByRes;
        allParameter.DifVifs = replaceParameter.DifVifs;
        allParameter.DifVifSize = replaceParameter.DifVifSize;
        allParameter.MBusShortOn = replaceParameter.MBusShortOn;
        allParameter.MBusOn = replaceParameter.MBusOn;
        allParameter.MBusParamConvertion = replaceParameter.MBusParamConvertion;
        allParameter.MBusParameterLength = replaceParameter.MBusParameterLength;
        allParameter.MBusParameterOverride = replaceParameter.MBusParameterOverride;
        allParameter.NameTranslated = replaceParameter.NameTranslated;
        for (int index = 0; index < replaceParameter.GroupMember.Length; ++index)
          allParameter.GroupMember[index] = replaceParameter.GroupMember[index];
      }
      for (int index = 0; index < this.MyMeter.AllParameters.Count; ++index)
      {
        Parameter byIndex = (Parameter) this.MyMeter.AllParameters.GetByIndex(index);
        if (byIndex.DifVifSize > (short) 0 && byIndex.NameTranslated != null && byIndex.NameTranslated.Length > 0 && (byIndex.MBusNeadedResources.Length <= 0 || this.MyMeter.AvailableMeterResouces[(object) byIndex.MBusNeadedResources] != null))
          this.MyMeter.MyMBusList.AllMBusParameters.Add((object) byIndex.FullName, (object) byIndex);
      }
      if (this.Mesurement_Runtime.Count > 0)
      {
        CodeBlock linkerCodeBlock = this.GenerateLinkerCodeBlock();
        this.AddRuntimeEndCode(linkerCodeBlock, -1);
        this.Mesurement_Runtime.Add((object) linkerCodeBlock);
        foreach (CodeBlock codeBlock in this.Mesurement_Runtime)
          this.MyMeter.MyRuntimeCode.LinkObjList.Add((object) codeBlock);
      }
      foreach (CodeBlock codeBlock in this.RAM_Runtime)
        this.MyMeter.MyRuntimeCode.LinkObjList.Add((object) codeBlock);
      if (this.Interval_Runtime.Count > 0 || this.EEPROM_Interval_Runtime.Count > 0)
      {
        CodeBlock linkerCodeBlock = this.GenerateLinkerCodeBlock();
        this.AddLinkerCodeObject(linkerCodeBlock, CodeObject.CodeTypes.BYTE, "RUI_CODE_IntervalTest");
        this.MyMeter.MyRuntimeCode.LinkObjList.Add((object) linkerCodeBlock);
      }
      foreach (CodeBlock codeBlock in this.Interval_Runtime)
        this.MyMeter.MyRuntimeCode.LinkObjList.Add((object) codeBlock);
      CodeBlock linkerCodeBlock1 = this.GenerateLinkerCodeBlock();
      if (this.EEPROM_Interval_Runtime.Count > 0)
      {
        this.AddLinkerCodeObject(linkerCodeBlock1, CodeObject.CodeTypes.BYTE, "RUI_CODE_EepRuntime");
        this.AddLinkerCodeObject(linkerCodeBlock1, CodeObject.CodeTypes.ePTR, "ToEprIntervallRuntime");
      }
      this.AddRuntimeEndCode(linkerCodeBlock1, -1);
      this.MyMeter.MyRuntimeCode.LinkObjList.Add((object) linkerCodeBlock1);
      CodeBlock linkerCodeBlock2 = this.GenerateLinkerCodeBlock();
      this.AddRuntimeEndCode(linkerCodeBlock2, -1);
      this.MyMeter.MyEpromRuntime.LinkObjList.Add((object) linkerCodeBlock2);
      if (this.EEPROM_Interval_Runtime.Count > 0)
      {
        foreach (CodeBlock codeBlock in this.EEPROM_Interval_Runtime)
          this.MyMeter.MyEpromRuntime.LinkObjList.Add((object) codeBlock);
        CodeBlock linkerCodeBlock3 = this.GenerateLinkerCodeBlock();
        this.AddRuntimeEndCode(linkerCodeBlock3, -1);
        this.MyMeter.MyEpromRuntime.LinkObjList.Add((object) linkerCodeBlock3);
      }
      foreach (CodeBlock TheCodeBlock in this.Event_Runtime)
      {
        this.AddRuntimeEndCode(TheCodeBlock, TheCodeBlock.FunctionNumber);
        this.MyMeter.MyEpromRuntime.LinkObjList.Add((object) TheCodeBlock);
      }
      if (this.MBus_Runtime.Count > 0)
      {
        foreach (CodeBlock codeBlock in this.MBus_Runtime)
          this.MyMeter.MyEpromRuntime.LinkObjList.Add((object) codeBlock);
        CodeBlock linkerCodeBlock4 = this.GenerateLinkerCodeBlock();
        this.AddRuntimeEndCode(linkerCodeBlock4, -1);
        this.MyMeter.MyEpromRuntime.LinkObjList.Add((object) linkerCodeBlock4);
      }
      return true;
    }

    private CodeBlock GenerateLinkerCodeBlock()
    {
      CodeBlock linkerCodeBlock = new CodeBlock(CodeBlock.CodeSequenceTypes.LinkerGeneratedCodeBlock, FrameTypes.None, -1);
      linkerCodeBlock.CodeSequenceName = "LinkerCodeBlock";
      if (this.LinkerCodeBlockList == null)
        this.LinkerCodeBlockList = new ArrayList();
      this.LinkerCodeBlockList.Add((object) linkerCodeBlock);
      return linkerCodeBlock;
    }

    private void AddLinkerCodeObject(
      CodeBlock TheCodeBlock,
      CodeObject.CodeTypes CodeType,
      string CodeValue)
    {
      CodeObject TheCodeObject = new CodeObject(-1);
      TheCodeObject.CodeType = CodeType;
      TheCodeObject.CodeValue = CodeValue;
      this.MyMeter.MyCompiler.PriCompileCodeObject(TheCodeObject);
      TheCodeBlock.CodeList.Add((object) TheCodeObject);
    }

    private void AddRuntimeEndCode(CodeBlock TheCodeBlock, int FunctionNumber)
    {
      CodeObject TheCodeObject = new CodeObject(FunctionNumber);
      TheCodeObject.CodeType = CodeObject.CodeTypes.BYTE;
      TheCodeObject.CodeValue = "RUI_CODE_End";
      this.MyMeter.MyCompiler.PriCompileCodeObject(TheCodeObject);
      TheCodeBlock.CodeList.Add((object) TheCodeObject);
    }

    internal bool UpdateEpromParameterData()
    {
      foreach (DictionaryEntry allParameter in this.MyMeter.AllParameters)
      {
        if (!((Parameter) allParameter.Value).UpdateByteList())
          return false;
      }
      return true;
    }

    internal bool AdjustConfigLoggers()
    {
      if (this.MyMeter.Eprom == null)
        return true;
      Parameter allParameter = (Parameter) this.MyMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_RuntimeCodeAdr"];
      allParameter.LoadValueFromEprom(this.MyMeter.Eprom);
      int valueEprom1 = (int) allParameter.ValueEprom;
      this.UpdateBlockAddresses(this.MyMeter.MyRuntimeCode.LinkObjList, ref valueEprom1);
      foreach (CodeBlock linkObj in this.MyMeter.MyRuntimeCode.LinkObjList)
      {
        if (linkObj is IntervalAndLogger)
        {
          IntervalAndLogger intervalAndLogger = (IntervalAndLogger) linkObj;
          if (intervalAndLogger.Type == LoggerTypes.ConfigLogger || intervalAndLogger.Type == LoggerTypes.ShortCycleLogger)
          {
            intervalAndLogger.AdjustFunction();
            int valueEprom2 = (int) allParameter.ValueEprom;
            this.UpdateBlockAddresses(this.MyMeter.MyRuntimeCode.LinkObjList, ref valueEprom2);
          }
        }
      }
      return true;
    }

    internal bool CompleteAllLoggerData()
    {
      foreach (CodeBlock linkObj in this.MyMeter.MyRuntimeCode.LinkObjList)
      {
        if (linkObj is IntervalAndLogger)
          ((IntervalAndLogger) linkObj).CompleteLoggerData();
      }
      return true;
    }

    internal void GetParameterInfo(StringBuilder TheText)
    {
      TheText.Append(ZR_Constants.SystemNewLine);
      SortedList sortedList = new SortedList();
      foreach (DictionaryEntry allParameter in this.MyMeter.AllParameters)
      {
        Parameter parameter = (Parameter) allParameter.Value;
        string[] strArray = parameter.FullName.Split('.');
        if (strArray.Length > 1)
          sortedList.Add((object) (strArray[1] + "(" + strArray[0] + ")"), (object) parameter);
        else
          sortedList.Add((object) parameter.Name, (object) parameter);
      }
      int totalWidth1 = 0;
      int totalWidth2 = 0;
      foreach (DictionaryEntry dictionaryEntry in sortedList)
      {
        Parameter parameter = (Parameter) dictionaryEntry.Value;
        if (parameter.Name.Length > totalWidth1)
          totalWidth1 = parameter.Name.Length;
        if (parameter.FunctionNumber >= 0)
        {
          int length = ((Function) this.MyMeter.MyHandler.MyLoadedFunctions.FullLoadedFunctions[(object) (ushort) parameter.FunctionNumber]).Name.Length;
          if (length > totalWidth2)
            totalWidth2 = length;
        }
      }
      foreach (DictionaryEntry dictionaryEntry in sortedList)
      {
        Parameter parameter = (Parameter) dictionaryEntry.Value;
        TheText.Append(parameter.Name.PadRight(totalWidth1, '.'));
        if (this.MyMeter.MyHandler.MyInfoFlags.ShowFunctionNumbers)
        {
          TheText.Append(" ");
          if (parameter.FunctionNumber < 0)
            TheText.Append("----");
          else
            TheText.Append(parameter.FunctionNumber.ToString("d04"));
        }
        if (this.MyMeter.MyHandler.MyInfoFlags.ShowFunctionNames)
        {
          TheText.Append(" ");
          if (parameter.FunctionNumber < 0)
            TheText.Append("----".PadRight(totalWidth2, '.'));
          else
            TheText.Append(((Function) this.MyMeter.MyHandler.MyLoadedFunctions.FullLoadedFunctions[(object) (ushort) parameter.FunctionNumber]).Name.PadRight(totalWidth2, '.'));
        }
        TheText.Append("  Adr:");
        if (parameter.Address < 0)
          TheText.Append("----");
        else
          TheText.Append(parameter.Address.ToString("x04"));
        if (parameter.ExistOnEprom)
        {
          TheText.Append(" VEpr:0x");
          TheText.Append(parameter.ValueEprom.ToString("x08"));
          TheText.Append(" = ");
          if (parameter.ParameterFormat == Parameter.BaseParameterFormat.DateTime)
          {
            DateTime dateTime = ZR_Calendar.Cal_GetDateTime((uint) parameter.ValueEprom);
            TheText.Append(" Time:'" + dateTime.ToString("dd.MM.yyyy HH:mm:ss") + "'");
          }
          else
            TheText.Append(parameter.ValueEprom.ToString("d012"));
        }
        TheText.Append(ZR_Constants.SystemNewLine);
      }
    }

    internal void GetBlockListInfo(StringBuilder TheText)
    {
      int num = -1;
      foreach (LinkBlock linkBlock in this.LinkBlockList)
      {
        TheText.Append(ZR_Constants.SystemNewLine);
        TheText.Append("*****************************************************************************" + ZR_Constants.SystemNewLine);
        TheText.Append("*********************** LinkerBlock: " + linkBlock.LinkBlockType.ToString() + ZR_Constants.SystemNewLine);
        TheText.Append(ZR_Constants.SystemNewLine);
        int RAM_Address = !(linkBlock is RuntimeCode) ? -1 : this.RuntimeCodeStartAddress;
        foreach (object linkObj in linkBlock.LinkObjList)
        {
          if (linkObj is Parameter)
          {
            ((LinkObj) linkObj).GetObjectInfo(TheText, this.MyMeter);
          }
          else
          {
            CodeBlock codeBlock = (CodeBlock) linkObj;
            if (codeBlock.FunctionNumber != num)
            {
              num = codeBlock.FunctionNumber;
              TheText.Append(ZR_Constants.SystemNewLine);
              TheText.Append("### Function: " + num.ToString() + ZR_Constants.SystemNewLine);
            }
            codeBlock.GetObjectInfo(TheText, this.MyMeter);
            foreach (LinkObj code in codeBlock.CodeList)
              code.GetObjectInfo(TheText, this.MyMeter, ref RAM_Address);
          }
        }
      }
    }

    internal void GetBlockListDiffInfo(
      StringBuilder RefText,
      StringBuilder CompareText,
      byte[] CompareProm)
    {
      string str = "";
      int num = -1;
      int TheValue = 0;
      bool flag1 = true;
      bool flag2 = false;
      bool flag3 = true;
      bool flag4 = true;
      for (int index = 0; index < RefText.Length; ++index)
      {
        char digit = RefText[index];
        if (digit == '\n' || digit == '\r')
        {
          num = -1;
          TheValue = 0;
          flag1 = true;
          flag2 = false;
          flag3 = true;
          flag4 = true;
          CompareText.Append(digit);
        }
        else
        {
          ++num;
          if (flag2)
          {
            CompareText.Append(digit);
          }
          else
          {
            if (TheValue >= CompareProm.Length)
              CompareText.Append('-');
            if (flag1)
            {
              if (!this.AddHexDigit(digit, ref TheValue, 3 - num))
                flag2 = true;
              else if (num == 3)
                flag1 = false;
              CompareText.Append(digit);
            }
            else if (flag4)
            {
              if (digit == ' ')
                flag4 = false;
              CompareText.Append(digit);
            }
            else if (flag3)
            {
              if (digit == ' ')
                CompareText.Append(digit);
              else if ((digit < '0' || digit > '9') && (digit < 'a' || digit > 'f'))
              {
                flag2 = true;
                CompareText.Append(digit);
              }
              else
              {
                str = CompareProm[TheValue++].ToString("x02");
                CompareText.Append(str[0]);
                flag3 = false;
              }
            }
            else
            {
              CompareText.Append(str[1]);
              flag3 = true;
            }
          }
        }
      }
    }

    internal bool AddHexDigit(char digit, ref int TheValue, int offset)
    {
      int num;
      switch (digit)
      {
        case '0':
          num = 0;
          break;
        case '1':
          num = 1;
          break;
        case '2':
          num = 2;
          break;
        case '3':
          num = 3;
          break;
        case '4':
          num = 4;
          break;
        case '5':
          num = 5;
          break;
        case '6':
          num = 6;
          break;
        case '7':
          num = 7;
          break;
        case '8':
          num = 8;
          break;
        case '9':
          num = 9;
          break;
        case 'a':
          num = 10;
          break;
        case 'b':
          num = 11;
          break;
        case 'c':
          num = 12;
          break;
        case 'd':
          num = 13;
          break;
        case 'e':
          num = 14;
          break;
        case 'f':
          num = 15;
          break;
        default:
          return false;
      }
      TheValue += num << 4 * offset;
      return true;
    }
  }
}
