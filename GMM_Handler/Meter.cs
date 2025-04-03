// Decompiled with JetBrains decompiler
// Type: GMM_Handler.Meter
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
  public class Meter
  {
    internal Meter.MeterDataStates MeterDataState = Meter.MeterDataStates.New;
    internal ZR_HandlerFunctions MyHandler;
    internal MeterCommunication MyCommunication;
    internal ZR_MeterIdent MyIdent;
    internal Linker MyLinker;
    internal Compiler MyCompiler;
    internal FunctionPalette MyPalette;
    internal ZelsiusMath MyMath = new ZelsiusMath();
    internal EpromHeader MyEpromHeader;
    internal RamParameter MyRamParameter;
    internal Backup MyBackup;
    internal FixedParameter MyFixedParameter;
    internal WritePermTable MyWritePermTable;
    internal DisplayCode MyDisplayCode;
    internal RuntimeVars MyRuntimeVars;
    internal RuntimeCode MyRuntimeCode;
    internal EpromVars MyEpromVars;
    internal EpromParameter MyEpromParameter;
    internal EpromRuntime MyEpromRuntime;
    internal MBusList MyMBusList;
    internal FunctionTable MyFunctionTable;
    internal LoggerStore MyLoggerStore;
    internal byte[] Eprom;
    internal int UsedEpromSize = 0;
    internal bool WriteEnable = false;
    internal bool WriteEnableWithOpen = false;
    internal bool KeyDataActual = false;
    internal bool DatabaseKeyIsAvailable = false;
    internal long MyDatabaseKey = 0;
    internal bool MeterKeyIsAvailable = false;
    internal long MyMeterKey = 0;
    internal bool DatabaseKeyIsSaved = false;
    internal bool MeterKeyIsSaved = false;
    internal InOutFunctions InOut1Function = InOutFunctions.IO_EnumError;
    internal InOutFunctions InOut2Function = InOutFunctions.IO_EnumError;
    internal bool[] EpromWriteEnable;
    internal bool[] RamWriteEnable;
    internal bool SecoundWriteRunning = false;
    internal SortedList<uint, Function> ConfigLoggers;
    internal ArrayList Map;
    internal ArrayList BlockLinkOrder;
    internal SortedList AllParameters;
    internal SortedList AllParametersByResource;
    internal SortedList AllEpromParametersByAddress;
    internal SortedList AllRamParametersByAddress;
    internal SortedList AliasParameters;
    internal SortedList<string, Parameter> PotentialLoggerParameters;
    internal SortedList AvailableMeterResouces;
    internal SortedList NeadedMeterResources;
    internal DateTime MeterTime;
    internal DateTime DatabaseTime = DateTime.MinValue;
    private static string[] CriticalBlockPointers = new string[7]
    {
      "EEP_Header.EEP_HEADER_RuntimeVarsAdr",
      "EEP_Header.EEP_HEADER_RuntimeCodeAdr",
      "EEP_Header.EEP_HEADER_EpromVarsAdr",
      "EEP_Header.EEP_HEADER_ParamBlockAdr",
      "EEP_Header.EEP_HEADER_EpromRuntimeAdr",
      "EEP_Header.EEP_HEADER_MBusBlockAdr",
      "EEP_Header.EEP_HEADER_FunctionTableAdr"
    };
    private static string[] ActualVariablesList = new string[14]
    {
      "DefaultFunction.Sta_Secounds",
      "Itr_NextIntervalTime",
      "DefaultFunction.Waerme_EnergSum",
      "DefaultFunction.Kaelte_EnergSum",
      "DefaultFunction.Vol_VolSum",
      "DefaultFunction.TAR_EnergySumTar0",
      "DefaultFunction.TAR_EnergySumTar1",
      "DefaultFunction.In1Display",
      "DefaultFunction.In2Display",
      "vorlauftemperatur",
      "ruecklauftemperatur",
      "delta_t",
      "Vol_Flow",
      "Leistung"
    };
    private int RamCacheAddress;
    private int RamCacheSize = 0;

    public Meter(ZR_HandlerFunctions MyHandlerIn) => this.MyHandler = MyHandlerIn;

    internal Meter BaseClone()
    {
      Meter meter = new Meter(this.MyHandler);
      meter.MyMath.MyBaseSettings = this.MyMath.MyBaseSettings.Clone();
      meter.MyIdent = this.MyIdent.Clone();
      ++meter.MyIdent.MeterClonNumber;
      meter.MyLinker = this.MyLinker.Clone(meter);
      meter.MyCompiler = this.MyCompiler.Clone(meter);
      if (this.MyPalette != null)
        meter.MyPalette = this.MyPalette.Clone(meter);
      meter.AllParameters = new SortedList();
      meter.AliasParameters = new SortedList();
      meter.MyEpromHeader = new EpromHeader(meter);
      meter.MyMBusList = this.MyMBusList.Clone(meter);
      meter.MyFunctionTable = this.MyFunctionTable.Clone(meter);
      if (this.ConfigLoggers != null)
      {
        meter.ConfigLoggers = new SortedList<uint, Function>();
        foreach (Function function in (IEnumerable<Function>) this.ConfigLoggers.Values)
          meter.ConfigLoggers.Add((uint) function.Number, function.Clone(meter));
      }
      meter.Map = this.Map;
      meter.BlockLinkOrder = this.BlockLinkOrder;
      meter.WriteEnable = this.WriteEnable;
      meter.WriteEnableWithOpen = this.WriteEnableWithOpen;
      meter.KeyDataActual = this.KeyDataActual;
      meter.DatabaseKeyIsAvailable = this.DatabaseKeyIsAvailable;
      meter.MyDatabaseKey = this.MyDatabaseKey;
      meter.MeterKeyIsAvailable = this.MeterKeyIsAvailable;
      meter.MyMeterKey = this.MyMeterKey;
      meter.DatabaseKeyIsSaved = this.DatabaseKeyIsSaved;
      meter.MeterKeyIsSaved = this.MeterKeyIsSaved;
      meter.InOut1Function = this.InOut1Function;
      meter.InOut2Function = this.InOut2Function;
      DateTime databaseTime = this.DatabaseTime;
      if (true)
        meter.DatabaseTime = this.DatabaseTime;
      return meter;
    }

    internal bool ChangeBaseCloneToCompatibeleType(ZR_MeterIdent CompatibleIdent)
    {
      if (!this.MyHandler.MyDataBaseAccess.GetHardwareTables(CompatibleIdent, out this.Map, out this.MyCompiler.Includes, out this.BlockLinkOrder))
        return false;
      this.MyIdent = CompatibleIdent;
      return true;
    }

    internal bool AddDatabaseOverridesToBaseClone()
    {
      ArrayList Overrides;
      if (!this.MyHandler.MyDataBaseAccess.GetOverrides(this.MyIdent.MeterInfoID, out Overrides))
        return false;
      foreach (OverrideParameter TheOverrideParameter in Overrides)
        OverrideParameter.ChangeOrAddOverrideParameter(this.MyFunctionTable.OverridesList, TheOverrideParameter);
      return true;
    }

    internal bool CompleteTheClone(SortedList BaseAllParameters, bool IgnoreOverrides)
    {
      return this.CompleteTheCloneToCompiledFunctions(BaseAllParameters, IgnoreOverrides) && this.MyMBusList.GenerateNewList() && this.CompleteTheCloneFromCreateFunctionTable();
    }

    internal bool CompleteTheCloneAndSetMeterKey(SortedList BaseAllParameters, uint MeterKey)
    {
      if (!this.CompleteTheCloneToCompiledFunctions(BaseAllParameters, false))
        return false;
      Parameter allParameter1 = (Parameter) this.AllParameters[(object) "EEP_Header.EEP_HEADER_MeterKey"];
      allParameter1.ValueEprom = (long) MeterKey;
      allParameter1.UpdateByteList();
      Parameter allParameter2 = (Parameter) this.AllParameters[(object) "DefaultFunction.RAM_WriteLimit"];
      allParameter2.ValueEprom = 0L;
      allParameter2.UpdateByteList();
      return this.MyMBusList.GenerateNewList() && this.CompleteTheCloneFromCreateFunctionTable();
    }

    internal bool CompleteTheCloneToCompiledFunctions(
      SortedList BaseAllParameters,
      bool IgnoreOverrides)
    {
      this.MyLinker.LinkBlockList.Add((object) this.MyEpromHeader);
      if (!this.MyLinker.GenerateBlockList() || !this.MyHandler.MyLoadedFunctions.GarantAllFunctionsLoaded(this.MyFunctionTable.FunctionNumbersList) || !this.CreateMeterResourceInformation() || !this.MyFunctionTable.DeleteFunctionsWithMissedResources() || !this.MyLinker.IncludeAllFunctions() || !this.MyCompiler.GenerateMenuObjects() || !this.MyLinker.GenerateObjectLists() || !this.MyLinker.AddMapVariables())
        return false;
      if (this.MyHandler.UseOnlyDefaultValues)
      {
        ((Parameter) this.AllParameters[(object) "EEP_Header.EEP_HEADER_MeterTypeID"]).ValueEprom = (long) this.MyIdent.MeterInfoID;
        ((Parameter) this.AllParameters[(object) "EEP_Header.EEP_HEADER_MeterInfoID"]).ValueEprom = (long) this.MyIdent.MeterInfoID;
      }
      else
        this.UpdateParametersFromExternalParameterList(BaseAllParameters, Parameter.ParameterGroups.All);
      if (!IgnoreOverrides && !this.MyFunctionTable.CopyBaseOverridesToParameter() || !this.MyLinker.UpdateAdressesAtBlockRange((LinkBlock) this.MyLinker.LinkBlockList[0], (LinkBlock) this.MyDisplayCode) || !this.CopyNeadedVars(false) || !this.MyMath.CreateBaseSettings(this.MyFunctionTable.OverridesList))
        return false;
      if (!this.MyMath.CalculateMeterSettings(this.MyIdent.lFirmwareVersion))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, this.MyMath.getLastErrorString());
        return false;
      }
      if (!this.MyDisplayCode.AdjustFunctions() || !this.ReloadNeadedVars() || !IgnoreOverrides && !this.MyFunctionTable.CopyValuesFromOverriedesToParameter() || !this.MyLinker.UpdateAdressesAtBlockRange((LinkBlock) this.MyDisplayCode, (LinkBlock) null) || !this.MyLinker.CreateParameterAddressLists())
        return false;
      this.MyEpromHeader.ClearChecksumsAndAddresses();
      return this.MyCompiler.CompileFunctions() && this.MyLinker.CompleteAllLoggerData();
    }

    internal bool CompleteTheCloneFromCreateFunctionTable()
    {
      if (!this.MyFunctionTable.GenerateNewList() || !this.MyLinker.UpdateMBusAndFunctionTableAdresses() || !this.MyLinker.SetAddressReferences() || !this.MyLinker.LinkAllPointers() || !this.MyLoggerStore.InitialiseTheLoggerAreas() || !this.MyFixedParameter.LoadEmergencyFrame() || !this.MyEpromHeader.UpdateMBusHeaderData() || !this.MyLinker.UpdateEpromParameterData() || !this.MyFunctionTable.AddOverridesFromParameter())
        return false;
      this.MeterDataState = Meter.MeterDataStates.CloneComplete;
      return true;
    }

    internal bool LoadType(int MeterInfoID) => this.LoadType(MeterInfoID, 0);

    internal bool LoadType(int MeterInfoID, int lFirmwareVersion)
    {
      try
      {
        this.MyMath = new ZelsiusMath();
        this.MyLinker = new Linker(this);
        this.MyCompiler = new Compiler(this);
        this.AllParameters = new SortedList();
        this.AliasParameters = new SortedList();
        if (!this.MyHandler.MyDataBaseAccess.GetTypeBaseData(MeterInfoID, out this.MyIdent, out this.Eprom))
          return false;
        this.MyEpromHeader = new EpromHeader(this);
        this.MyLinker.LinkBlockList.Add((object) this.MyEpromHeader);
        if (!this.MyLinker.UpdateAdresses() || !this.UpdateParameterFromEprom(true))
          return false;
        this.MyIdent.lFirmwareVersion = (long) lFirmwareVersion;
        if (!this.MyHandler.MyDataBaseAccess.GetHardwareInfo(this.MyIdent) || !this.MyHandler.MyDataBaseAccess.GetHardwareTables(this.MyIdent, out this.Map, out this.MyCompiler.Includes, out this.BlockLinkOrder))
          return false;
        this.MyFunctionTable = new FunctionTable(this);
        if (!this.MyFunctionTable.ReadFunctionTableFromEprom() || !this.CreateMeterFromFunctionTable() || !this.MyHandler.UseOnlyDefaultValues && !this.MyFunctionTable.AddOverridesFromParameter())
          return false;
        Parameter allParameter1 = (Parameter) this.AllParameters[(object) "EEP_Header.EEP_HEADER_MeterInfoID"];
        allParameter1.ValueEprom = (long) this.MyIdent.MeterInfoBaseID;
        allParameter1.UpdateByteList();
        Parameter allParameter2 = (Parameter) this.AllParameters[(object) "EEP_Header.EEP_HEADER_MeterTypeID"];
        allParameter2.ValueEprom = (long) this.MyIdent.MeterInfoID;
        allParameter2.UpdateByteList();
        this.WriteEnable = true;
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "LoadTypeException:" + ZR_Constants.SystemNewLine + ex.ToString());
        return false;
      }
    }

    internal bool LoadTypeToFunctionTable(int MeterInfoID)
    {
      this.MyMath = new ZelsiusMath();
      this.MyLinker = new Linker(this);
      this.MyCompiler = new Compiler(this);
      this.AllParameters = new SortedList();
      this.AliasParameters = new SortedList();
      if (!this.MyHandler.MyDataBaseAccess.GetTypeBaseData(MeterInfoID, out this.MyIdent, out this.Eprom))
        return false;
      this.MyEpromHeader = new EpromHeader(this);
      this.MyLinker.LinkBlockList.Add((object) this.MyEpromHeader);
      if (!this.MyLinker.UpdateAdresses() || !this.UpdateParameterFromEprom(true) || !this.MyHandler.MyDataBaseAccess.GetHardwareInfo(this.MyIdent) || !this.MyHandler.MyDataBaseAccess.GetHardwareTables(this.MyIdent, out this.Map, out this.MyCompiler.Includes, out this.BlockLinkOrder))
        return false;
      this.MyFunctionTable = new FunctionTable(this);
      return this.MyFunctionTable.ReadFunctionTableFromEprom() && this.CreateMeterFromFunctionTable();
    }

    internal bool LoadMeter(ZR_MeterIdent TheMeterIdent, DateTime StorageTime)
    {
      if (!this.MyHandler.MyDataBaseAccess.GetMeterEpromData(TheMeterIdent, ref StorageTime, out this.Eprom))
        return false;
      this.MyHandler.MyMeters.DbMeterReadEEProm = (byte[]) this.Eprom.Clone();
      this.DatabaseTime = StorageTime;
      this.MyLinker = new Linker(this);
      this.MyCompiler = new Compiler(this);
      this.AllParameters = new SortedList();
      this.AliasParameters = new SortedList();
      this.MyIdent = new ZR_MeterIdent(MeterBasis.DbMeter);
      this.MyEpromHeader = new EpromHeader(this);
      this.MyLinker.LinkBlockList.Add((object) this.MyEpromHeader);
      if (!this.MyLinker.UpdateAdresses() || !this.UpdateParameterFromEprom(true))
        return false;
      this.MyEpromHeader.UpdateIdentData();
      if (TheMeterIdent.lFirmwareVersion > 0L)
        this.MyIdent.lFirmwareVersion = TheMeterIdent.lFirmwareVersion;
      if (!this.MyHandler.MyDataBaseAccess.GetHardwareInfo(this.MyIdent))
        return false;
      if (this.Eprom.Length != this.MyIdent.extEEPSize)
      {
        byte[] numArray = new byte[this.MyIdent.extEEPSize];
        for (int index = 0; index < numArray.Length && index < this.Eprom.Length; ++index)
          numArray[index] = this.Eprom[index];
        this.Eprom = numArray;
      }
      if (!this.MyHandler.MyDataBaseAccess.GetHardwareTables(this.MyIdent, out this.Map, out this.MyCompiler.Includes, out this.BlockLinkOrder))
        return false;
      this.MyFunctionTable = new FunctionTable(this);
      return this.MyFunctionTable.ReadFunctionTableFromEprom() && this.CreateMeterFromFunctionTable();
    }

    internal bool ConnectMeter()
    {
      this.MyCommunication = new MeterCommunication(this);
      this.MyIdent = new ZR_MeterIdent(MeterBasis.Read);
      if (!this.MyCommunication.ReadVersion(this.MyIdent))
      {
        ZR_ClassLibMessages.AddErrorDescription(this.MyHandler.MyRes.GetString("ConnectMeterError"));
        return false;
      }
      this.MeterDataState = Meter.MeterDataStates.Connected;
      return true;
    }

    internal bool IdentConnectedMeter()
    {
      this.MyLinker = new Linker(this);
      this.MyCompiler = new Compiler(this);
      this.AllParameters = new SortedList();
      this.AliasParameters = new SortedList();
      this.MyEpromHeader = new EpromHeader(this);
      this.MyLinker.LinkBlockList.Add((object) this.MyEpromHeader);
      if (!this.MyLinker.UpdateAdresses() || !this.MyEpromHeader.ReadHeaderFromConnectedDevice() || !this.UpdateParameterFromEprom(true))
        return false;
      this.MyEpromHeader.UpdateIdentData();
      return true;
    }

    internal bool ReadConnectedMeter()
    {
      if (this.MeterDataState != Meter.MeterDataStates.Connected)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData);
        return false;
      }
      if (!this.MyHandler.ReadWithoutBackup && !this.MyCommunication.MyBus.RunBackup() || !this.IdentConnectedMeter() || !this.MyHandler.MyDataBaseAccess.GetHardwareInfo(this.MyIdent) || !this.MyHandler.MyDataBaseAccess.GetHardwareTables(this.MyIdent, out this.Map, out this.MyCompiler.Includes, out this.BlockLinkOrder))
        return false;
      int address = ((LinkObj) this.AllParameters[(object) "EEP_Header.EEP_HEADER_BackupChecksum"]).Address;
      int num = (int) ((Parameter) this.AllParameters[(object) "EEP_Header.EEP_HEADER_FunctionTableAdr"]).ValueEprom + 6 - address;
      ByteField MemoryData = new ByteField(num);
      if (!this.MyCommunication.MyBus.ReadMemory(MemoryLocation.EEPROM, address, num, out MemoryData))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Read memory error");
        return false;
      }
      byte[] numArray = new byte[this.MyIdent.extEEPSize];
      this.Eprom.CopyTo((Array) numArray, 0);
      this.Eprom = numArray;
      for (int index = 0; index < num; ++index)
        this.Eprom[index + address] = MemoryData.Data[index];
      this.MyFunctionTable = new FunctionTable(this);
      if (!this.MyFunctionTable.ReadFunctionTableFromConnectedDevice() || !this.MyEpromHeader.AllChecksumsOk() || !this.CreateMeterFromFunctionTable() || !this.MyMBusList.AdjustAllMBusParameterDivVifs())
        return false;
      this.WriteEnable = false;
      this.WriteEnableWithOpen = false;
      Parameter allParameter1 = (Parameter) this.AllParameters[(object) "DefaultFunction.RAM_WriteLimit"];
      if (allParameter1.ValueEprom == (long) byte.MaxValue)
      {
        this.WriteEnable = true;
      }
      else
      {
        if (!this.MyCommunication.ReadParameterValue(allParameter1, MemoryLocation.RAM))
          return false;
        if (allParameter1.ValueCPU == (long) byte.MaxValue)
          this.WriteEnable = true;
      }
      if (this.WriteEnable)
      {
        Parameter allParameter2 = (Parameter) this.AllParameters[(object) "EEP_Header.EEP_HEADER_MeterKey"];
        if (!this.MyCommunication.ReadParameterValue(allParameter2, MemoryLocation.EEPROM))
          return false;
        this.MyMeterKey = allParameter2.ValueEprom;
        this.MeterKeyIsAvailable = true;
        this.KeyDataActual = true;
      }
      else
      {
        this.GenerateWriteEnableLists(false);
        if (this.RamWriteEnable[allParameter1.AddressCPU])
          this.WriteEnableWithOpen = true;
      }
      return true;
    }

    internal bool ReadConnectedMeterLoggerData()
    {
      if (this.UsedEpromSize > 0)
        return true;
      this.UsedEpromSize = this.MyLoggerStore.BlockStartAddress;
      foreach (IntervalAndLogger allIntervallCode in this.MyLinker.AllIntervallCodes)
      {
        if ((allIntervallCode.Type != LoggerTypes.ConfigLogger || this.MyHandler.LoggerRestoreState == LoggerRestor.RestoreAll) && !allIntervallCode.ReadDataToEpromArray())
          return false;
      }
      return true;
    }

    internal List<LoggerInfo> GetLoggerInfos(out int LoggerMemorySize)
    {
      LoggerMemorySize = 0;
      List<LoggerInfo> loggerInfos = new List<LoggerInfo>();
      foreach (IntervalAndLogger allIntervallCode in this.MyLinker.AllIntervallCodes)
      {
        LoggerInfo loggerInfo = allIntervallCode.GetLoggerInfo(out LoggerMemorySize);
        if (loggerInfo != null)
          loggerInfos.Add(loggerInfo);
      }
      return loggerInfos;
    }

    internal List<LoggerInfo> ReadFixedLoggers()
    {
      int LoggerMemorySize = 0;
      List<LoggerInfo> loggerInfoList = new List<LoggerInfo>();
      foreach (IntervalAndLogger allIntervallCode in this.MyLinker.AllIntervallCodes)
      {
        if (allIntervallCode.Type == LoggerTypes.FixedLogger || allIntervallCode.Type == LoggerTypes.FixedLoggerFuture)
        {
          LoggerInfo loggerInfo = allIntervallCode.GetLoggerInfo(out LoggerMemorySize);
          if (loggerInfo != null)
          {
            if (loggerInfo.ReadLogger(DateTime.MinValue, DateTime.MaxValue))
              loggerInfoList.Add(loggerInfo);
            else
              ZR_ClassLibMessages.AddErrorDescription("Read error on logger: " + loggerInfo.LoggerName);
          }
        }
      }
      return loggerInfoList;
    }

    internal bool ReprogramLoggerData(List<LoggerInfo> OldLoggers)
    {
      foreach (LoggerInfo oldLogger in OldLoggers)
      {
        bool flag = false;
        foreach (IntervalAndLogger allIntervallCode in this.MyLinker.AllIntervallCodes)
        {
          if (allIntervallCode.FunctionNumber == oldLogger.MyLogger.FunctionNumber)
          {
            if (!allIntervallCode.ReprogramLoggerData(oldLogger))
            {
              ZR_ClassLibMessages.AddErrorDescription("Error on reload logger data: " + oldLogger.LoggerName);
              break;
            }
            flag = true;
            break;
          }
        }
        if (!flag)
          ZR_ClassLibMessages.AddErrorDescription("Logger not reloaded: " + oldLogger.LoggerName);
      }
      return true;
    }

    private bool CreateMeterFromFunctionTable()
    {
      if (!this.MyLinker.GenerateBlockList() || !this.MyHandler.MyLoadedFunctions.GarantAllFunctionsLoaded(this.MyFunctionTable.FunctionNumbersList) || !this.CreateMeterResourceInformation() || !this.MyLinker.IncludeAllFunctions())
        return false;
      this.MyFunctionTable.AddFunctionNames();
      if (!this.MyCompiler.GenerateMenuObjects() || !this.MyLinker.GenerateObjectLists() || !this.MyLinker.AdjustConfigLoggers() || !this.MyLinker.AddMapVariables() || !this.MyLinker.UpdateAdressesAtBlockRange((LinkBlock) this.MyLinker.LinkBlockList[1], (LinkBlock) this.MyDisplayCode))
        return false;
      if (this.MyHandler.UseOnlyDefaultValues)
      {
        if (!this.CopyNeadedVars(false))
          return false;
      }
      else if (!this.CopyNeadedVars(true))
        return false;
      if (!this.MyMath.GetNotAvailableOverrides(this.MyFunctionTable.OverridesList))
      {
        this.MyHandler.AddErrorPointMessage(this.MyMath.getLastErrorString());
        return false;
      }
      if (!this.MyMath.CreateBaseSettings(this.MyFunctionTable.OverridesList))
        return false;
      if (!this.MyMath.CalculateMeterSettings(this.MyIdent.lFirmwareVersion))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, this.MyMath.getLastErrorString());
        return false;
      }
      if (!this.MyDisplayCode.AdjustFunctions() || !this.MyCompiler.CompileFunctions() || !this.MyLinker.UpdateAdressesAtBlockRange((LinkBlock) this.MyDisplayCode, (LinkBlock) this.MyMBusList) || !this.UpdateParameterFromEprom(false) || !this.AdjustAdditionalImpulsValues())
        return false;
      this.MyLinker.ReloadRuntimeVarsRamAdresses();
      return this.MyLinker.CreateParameterAddressLists() && this.MyLinker.UpdateAdressesAtBlockRange((LinkBlock) this.MyMBusList, (LinkBlock) null) && this.MyLinker.CompleteAllLoggerData() && this.MyMBusList.LoadFromByteArray(this.Eprom, (int) ((Parameter) this.AllParameters[(object) "EEP_Header.EEP_HEADER_MBusBlockAdr"]).ValueEprom) && this.MyLinker.UpdateAdressesAtBlockRange((LinkBlock) this.MyMBusList, (LinkBlock) null) && this.MyLinker.LoadAllPointersFromEprom() && this.MyLinker.UpdateEpromParameterData() && this.MyCompiler.GenerateCodeFromCodeBlockList(this.MyMBusList.LinkObjList) && this.MyFunctionTable.GarantTypeSpecOverrides();
    }

    private bool AdjustAdditionalImpulsValues()
    {
      Parameter parameter1 = (Parameter) this.AllParametersByResource[(object) "Inp1Factor"];
      if (parameter1 != null)
      {
        OverrideParameter overrides = (OverrideParameter) this.MyFunctionTable.OverridesList[(object) OverrideID.Input1PulsValue];
        Decimal num = (Decimal) Math.Pow(10.0, (double) MeterMath.InputUnits[this.MyMath.MyBaseSettings.Input1UnitIndex].AfterPointDigits);
        string StringValue = ((Decimal) parameter1.ValueEprom / 64M / num).ToString();
        overrides.SetValueFromStringWin(StringValue);
      }
      Parameter parameter2 = (Parameter) this.AllParametersByResource[(object) "Inp2Factor"];
      if (parameter2 != null)
      {
        OverrideParameter overrides = (OverrideParameter) this.MyFunctionTable.OverridesList[(object) OverrideID.Input2PulsValue];
        Decimal num = (Decimal) Math.Pow(10.0, (double) MeterMath.InputUnits[this.MyMath.MyBaseSettings.Input2UnitIndex].AfterPointDigits);
        string StringValue = ((Decimal) parameter2.ValueEprom / 64M / num).ToString();
        overrides.SetValueFromStringWin(StringValue);
      }
      return true;
    }

    private bool CopyNeadedVars(bool UpdateFromEprom)
    {
      for (int index = 0; index < this.MyMath.NeadedMeterVars.Count; ++index)
      {
        string key = (string) this.MyMath.NeadedMeterVars.GetKey(index);
        if (!key.StartsWith("R:"))
        {
          Parameter allParameter = (Parameter) this.AllParameters[(object) key];
          if (allParameter == null || UpdateFromEprom && !allParameter.LoadValueFromEprom(this.Eprom))
            return false;
          this.MyMath.NeadedMeterVars.SetByIndex(index, (object) allParameter.ValueEprom);
        }
      }
      return true;
    }

    private bool ReloadNeadedVars()
    {
      for (int index = 0; index < this.MyMath.NeadedMeterVars.Count; ++index)
      {
        string key1 = (string) this.MyMath.NeadedMeterVars.GetKey(index);
        Parameter allParameter;
        if (key1.StartsWith("R:"))
        {
          string key2 = key1.Substring(2);
          if (this.AllParametersByResource != null)
          {
            allParameter = (Parameter) this.AllParametersByResource[(object) key2];
            if (allParameter == null)
              continue;
          }
          else
            continue;
        }
        else
          allParameter = (Parameter) this.AllParameters[(object) key1];
        if (allParameter == null)
          return false;
        object byIndex = this.MyMath.NeadedMeterVars.GetByIndex(index);
        if (byIndex != null)
        {
          if (allParameter.FullName == "DefaultFunction.PulsValue1")
          {
            long num = (long) byIndex;
            if (num != allParameter.ValueEprom + 1L && num != allParameter.ValueEprom - 1L)
              allParameter.ValueEprom = (long) byIndex;
          }
          else
            allParameter.ValueEprom = (long) byIndex;
        }
      }
      return true;
    }

    internal bool OverrideAllLinkerObjectsWithEpromData()
    {
      foreach (LinkBlock linkBlock in this.MyLinker.LinkBlockList)
      {
        foreach (object linkObj in linkBlock.LinkObjList)
        {
          if (linkObj is Parameter)
          {
            Parameter parameter = (Parameter) linkObj;
            if (parameter.ExistOnEprom)
              parameter.LoadValueFromEprom(this.Eprom);
          }
          else
          {
            foreach (LinkObj code in ((CodeBlock) linkObj).CodeList)
              this.OverrideLinkObjectData(code);
          }
        }
      }
      this.MeterDataState = Meter.MeterDataStates.EpromDataReloaded;
      return true;
    }

    private void OverrideLinkObjectData(LinkObj TheObject)
    {
      try
      {
        if (TheObject.LinkByteList == null)
        {
          ZR_ClassLibMessages.AddWarning("missing: LinkByteList");
        }
        else
        {
          for (int index = 0; index < TheObject.LinkByteList.Length; ++index)
            TheObject.LinkByteList[index] = this.Eprom[TheObject.Address + index];
        }
      }
      catch
      {
      }
    }

    internal bool GenerateEprom()
    {
      this.Eprom = new byte[this.MyIdent.extEEPSize];
      if (!this.MyCompiler.CopyToEprom())
        return false;
      this.MyFunctionTable.GenerateChecksum();
      this.MyEpromHeader.GenerateChecksums();
      return true;
    }

    internal bool GenerateWriteEnableLists(bool GenerateAllways)
    {
      if (!GenerateAllways && this.WriteEnable)
        return true;
      if (this.MyWritePermTable.LinkObjList.Count < 1)
        return false;
      this.EpromWriteEnable = new bool[this.MyIdent.extEEPSize];
      this.RamWriteEnable = new bool[65536];
      try
      {
        int num1 = 0;
        while (true)
        {
          ushort num2;
          ushort valueEprom;
          do
          {
            do
            {
              ArrayList linkObjList1 = this.MyWritePermTable.LinkObjList;
              int index1 = num1;
              int num3 = index1 + 1;
              num2 = (ushort) ((Parameter) linkObjList1[index1]).ValueEprom;
              if (num2 != (ushort) 0)
              {
                ArrayList linkObjList2 = this.MyWritePermTable.LinkObjList;
                int index2 = num3;
                num1 = index2 + 1;
                valueEprom = (ushort) ((Parameter) linkObjList2[index2]).ValueEprom;
                bool flag = false;
                if (((uint) num2 & 32768U) > 0U)
                {
                  num2 &= (ushort) short.MaxValue;
                  flag = true;
                }
                if (flag)
                  goto label_9;
              }
              else
                goto label_24;
            }
            while ((int) valueEprom >= this.EpromWriteEnable.Length);
            goto label_16;
label_9:;
          }
          while ((int) valueEprom >= this.RamWriteEnable.Length);
          if ((long) valueEprom + (long) num2 > (long) this.RamWriteEnable.Length)
            num2 = (ushort) ((uint) this.RamWriteEnable.Length - (uint) valueEprom);
          for (ushort index = 0; (int) index < (int) num2; ++index)
            this.RamWriteEnable[(int) valueEprom + (int) index] = true;
          goto label_21;
label_16:
          if ((long) valueEprom + (long) num2 > (long) this.EpromWriteEnable.Length)
            num2 = (ushort) ((uint) this.EpromWriteEnable.Length - (uint) valueEprom);
          for (ushort index = 0; (int) index < (int) num2; ++index)
            this.EpromWriteEnable[(int) valueEprom + (int) index] = true;
label_21:;
        }
      }
      catch
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Write permition table error");
        return false;
      }
label_24:
      return true;
    }

    internal bool UpdateParameterFromEprom(bool IncludeHeaderPointer)
    {
      foreach (DictionaryEntry allParameter in this.AllParameters)
      {
        Parameter parameter = (Parameter) allParameter.Value;
        if (IncludeHeaderPointer || !parameter.FullName.StartsWith("EEP_Header."))
          parameter.LoadValueFromEprom(this.Eprom);
      }
      return true;
    }

    internal bool AreBlocksizesUnchanged(byte[] CompareEEProm)
    {
      for (int index = 0; index < Meter.CriticalBlockPointers.Length; ++index)
      {
        Parameter allParameter = (Parameter) this.AllParameters[(object) Meter.CriticalBlockPointers[index]];
        if (allParameter.ValueEprom != allParameter.GetValueFromMap(CompareEEProm))
          ZR_ClassLibMessages.AddWarning("Pointer changed: " + allParameter.Name);
      }
      return true;
    }

    internal bool UpdateParametersFromExternalParameterList(
      SortedList ExternalParameter,
      Parameter.ParameterGroups TheOption)
    {
      foreach (DictionaryEntry allParameter in this.AllParameters)
      {
        string key = (string) allParameter.Key;
        Parameter parameter1 = (Parameter) allParameter.Value;
        Parameter parameter2 = (Parameter) ExternalParameter[(object) key];
        if (parameter2 != null && (TheOption == Parameter.ParameterGroups.All || parameter1.GroupMember[(int) TheOption]))
        {
          parameter1.CPU_ValueIsInitialised = parameter2.CPU_ValueIsInitialised;
          parameter1.ValueCPU = parameter2.ValueCPU;
          parameter1.EpromValueIsInitialised = parameter2.CPU_ValueIsInitialised;
          parameter1.ValueEprom = parameter2.ValueEprom;
          parameter1.MBusShortOn = parameter2.MBusShortOn;
          parameter1.MBusOn = parameter2.MBusOn;
        }
      }
      return true;
    }

    internal bool InitialiseAllTimes()
    {
      Parameter allParameter = (Parameter) this.AllParameters[(object) "DefaultFunction.Sta_Secounds"];
      allParameter.ValueEprom = (long) ZR_Calendar.Cal_GetMeterTime(this.MeterTime);
      if (!allParameter.UpdateByteList())
        return false;
      foreach (IntervalAndLogger allIntervallCode in this.MyLinker.AllIntervallCodes)
      {
        allIntervallCode.ClearLogger();
        allIntervallCode.InitialiseTimeVariablesFromMeterTime(this.MeterTime);
      }
      return true;
    }

    internal bool GetParameterValue(
      string Varname,
      MemoryLocation Location,
      bool DeviceRead,
      out long TheValue)
    {
      TheValue = 0L;
      Parameter allParameter = (Parameter) this.AllParameters[(object) Varname];
      if (allParameter == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Parameter not available: " + Varname);
        return false;
      }
      if (!DeviceRead && Location == MemoryLocation.RAM && !allParameter.CPU_ValueIsInitialised)
        DeviceRead = true;
      if (DeviceRead && !this.MyCommunication.ReadParameterValue(allParameter, Location))
        return false;
      TheValue = Location != MemoryLocation.RAM ? allParameter.ValueEprom : allParameter.ValueCPU;
      return true;
    }

    public bool SetParameterValue(
      string Varname,
      MemoryLocation Location,
      bool WriteImmediatly,
      long Value)
    {
      Parameter allParameter = (Parameter) this.AllParameters[(object) Varname];
      if (allParameter == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Parameter not available: " + Varname);
        return false;
      }
      if (Location == MemoryLocation.RAM)
      {
        allParameter.ValueCPU = Value;
        allParameter.CPU_ValueIsInitialised = true;
      }
      else
      {
        allParameter.ValueEprom = Value;
        allParameter.EpromValueIsInitialised = true;
        allParameter.UpdateByteList();
      }
      if (WriteImmediatly)
      {
        if (this.MyCommunication == null)
        {
          ZR_MeterIdent ident = this.MyIdent;
          if (!this.ConnectMeter())
          {
            ZR_ClassLibMessages.AddErrorDescription(this.MyHandler.MyRes.GetString("ConnectMeterError"));
            return false;
          }
          if (ident.lFirmwareVersion != this.MyIdent.lFirmwareVersion)
          {
            ZR_ClassLibMessages.AddErrorDescription("DifferentFirmware");
            return false;
          }
        }
        if (!this.MyCommunication.WriteParameterValue(allParameter, Location))
          return false;
      }
      return true;
    }

    public bool ChangeRamParameterBitsImmediately(string Varname, uint AndMask, uint OrMask)
    {
      Parameter allParameter = (Parameter) this.AllParameters[(object) Varname];
      if (allParameter == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Parameter not available: " + Varname);
        return false;
      }
      if (!allParameter.ExistOnCPU)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "No available at RAM");
        return false;
      }
      uint maxValue = (uint) byte.MaxValue;
      for (int index = 0; index < 4; ++index)
      {
        if (((int) AndMask | ~(int) maxValue) != -1)
        {
          if (((int) AndMask | (int) maxValue) != -1 || (OrMask & ~maxValue) > 0U)
            break;
        }
        else if ((OrMask & maxValue) > 0U)
        {
          if ((OrMask & ~maxValue) > 0U || ((int) AndMask | (int) maxValue) != -1)
            break;
        }
        else
        {
          maxValue <<= 8;
          continue;
        }
        byte AndMask1 = (byte) (AndMask >>= 8 * index);
        byte OrMask1 = (byte) (OrMask >>= 8 * index);
        return this.MyCommunication.MyBus.WriteBitfield(allParameter.AddressCPU + index, AndMask1, OrMask1);
      }
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal mask");
      return false;
    }

    internal bool SetMeterKey(long MeterKey, long DatabaseKey)
    {
      this.MyMeterKey = MeterKey;
      this.MeterKeyIsAvailable = true;
      this.MyDatabaseKey = DatabaseKey;
      this.DatabaseKeyIsAvailable = true;
      this.KeyDataActual = true;
      Parameter allParameter = (Parameter) this.AllParameters[(object) "EEP_Header.EEP_HEADER_MeterKey"];
      allParameter.ValueEprom = MeterKey;
      allParameter.UpdateByteList();
      return true;
    }

    internal bool DeleteMeterKey(long LockKey)
    {
      int MeterKey;
      if (this.MeterKeyIsAvailable)
        MeterKey = (int) this.MyMeterKey;
      else if (this.DatabaseKeyIsAvailable)
      {
        MeterKey = (int) ((this.MyDatabaseKey ^ LockKey) & (long) uint.MaxValue);
      }
      else
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "No key available");
        return false;
      }
      if (this.MyCommunication.MyBus.DeleteMeterKey(MeterKey))
      {
        this.WriteEnable = true;
        return true;
      }
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Communication error or wrong meter key!");
      return false;
    }

    internal bool SaveKeyToDatabase(long LockKey)
    {
      this.KeyDataActual = false;
      MeterDBAccess.ValueTypes ValueType;
      long Key;
      if (LockKey == 0L)
      {
        ValueType = MeterDBAccess.ValueTypes.MeterKey;
        Key = this.MyMeterKey;
      }
      else
      {
        ValueType = MeterDBAccess.ValueTypes.GovernmentRandomNr;
        this.MyDatabaseKey = this.MyMeterKey ^ LockKey;
        Key = this.MyDatabaseKey;
      }
      return this.MyHandler.MyDataBaseAccess.SetDeviceKey(this.MyIdent.MeterID, Key, ValueType);
    }

    public bool GetWriteState(
      out bool WriteIsEnabled,
      out bool WriteProtectionIsSet,
      out bool DatabaseKeyIsAvailable,
      out bool MeterKeyIsAvailable,
      out bool DatabaseKeyIsSaved,
      out bool MeterKeyIsSaved)
    {
      WriteIsEnabled = this.WriteEnable;
      WriteProtectionIsSet = ((Parameter) this.AllParameters[(object) "DefaultFunction.RAM_WriteLimit"]).ValueEprom != (long) byte.MaxValue;
      DatabaseKeyIsAvailable = this.DatabaseKeyIsAvailable;
      MeterKeyIsAvailable = this.MeterKeyIsAvailable;
      DatabaseKeyIsSaved = this.DatabaseKeyIsSaved;
      MeterKeyIsSaved = this.MeterKeyIsSaved;
      if (!this.KeyDataActual)
      {
        this.KeyDataActual = true;
        DatabaseKeyIsSaved = false;
        MeterKeyIsSaved = false;
        long Key;
        MeterDBAccess.ValueTypes ValueType;
        if (!this.MyHandler.MyDataBaseAccess.GetDeviceKeys(this.MyIdent.MeterID, out Key, out ValueType))
          return true;
        if (ValueType == MeterDBAccess.ValueTypes.GovernmentRandomNr)
        {
          this.DatabaseKeyIsSaved = true;
          this.MeterKeyIsSaved = false;
          if (this.DatabaseKeyIsAvailable)
          {
            if (Key != this.MyDatabaseKey)
              ZR_ClassLibMessages.AddWarning("DatabaseKey different in database and data");
          }
          else
          {
            this.DatabaseKeyIsAvailable = true;
            this.DatabaseKeyIsSaved = true;
            this.MyDatabaseKey = Key;
          }
        }
        else
        {
          this.DatabaseKeyIsSaved = false;
          this.MeterKeyIsSaved = true;
          if (this.MeterKeyIsAvailable)
          {
            if (Key != this.MyMeterKey)
              ZR_ClassLibMessages.AddWarning("MeterKey different in database and data");
          }
          else
          {
            this.MeterKeyIsAvailable = true;
            this.MeterKeyIsSaved = true;
            this.MyMeterKey = Key;
          }
        }
      }
      DatabaseKeyIsAvailable = this.DatabaseKeyIsAvailable;
      MeterKeyIsAvailable = this.MeterKeyIsAvailable;
      DatabaseKeyIsSaved = this.DatabaseKeyIsSaved;
      MeterKeyIsSaved = this.MeterKeyIsSaved;
      return true;
    }

    public bool ClearWriteProtection()
    {
      Parameter allParameter = (Parameter) this.AllParameters[(object) "DefaultFunction.RAM_WriteLimit"];
      allParameter.ValueEprom = (long) byte.MaxValue;
      allParameter.UpdateByteList();
      return true;
    }

    public bool SetWriteProtection()
    {
      Parameter allParameter = (Parameter) this.AllParameters[(object) "DefaultFunction.RAM_WriteLimit"];
      allParameter.ValueEprom = 0L;
      allParameter.UpdateByteList();
      return true;
    }

    internal bool CreateMeterResourceInformation()
    {
      SortedList fullLoadedFunctions = this.MyHandler.MyLoadedFunctions.FullLoadedFunctions;
      this.AvailableMeterResouces = new SortedList();
      this.NeadedMeterResources = new SortedList();
      MeterResource meterResource1 = new MeterResource(MeterResources.CEnergy.ToString(), (ushort) 0);
      this.AvailableMeterResouces.Add((object) meterResource1.Name, (object) meterResource1);
      string hardwareResource = this.MyIdent.HardwareResource;
      char[] chArray1 = new char[1]{ ';' };
      foreach (string str in hardwareResource.Split(chArray1))
      {
        string TheResourceName = str.Trim();
        if (TheResourceName.Length > 0)
        {
          MeterResource meterResource2 = new MeterResource(TheResourceName, (ushort) 0);
          this.AvailableMeterResouces.Add((object) meterResource2.Name, (object) meterResource2);
        }
      }
      for (ushort index1 = 0; (int) index1 < this.MyFunctionTable.FunctionNumbersList.Count; ++index1)
      {
        Function function = (Function) fullLoadedFunctions[this.MyFunctionTable.FunctionNumbersList[(int) index1]];
        for (int index2 = 0; index2 < function.SuppliedResources.Length; ++index2)
        {
          MeterResource meterResource3 = new MeterResource(function.SuppliedResources[index2], function.Number);
          if (!this.AvailableMeterResouces.ContainsKey((object) meterResource3.Name))
            this.AvailableMeterResouces.Add((object) meterResource3.Name, (object) meterResource3);
        }
        for (int index3 = 0; index3 < function.NeadedResources.Length; ++index3)
        {
          string neadedResource = function.NeadedResources[index3];
          char[] chArray2 = new char[1]{ '|' };
          foreach (string key in neadedResource.Split(chArray2))
          {
            if (key.Length > 0)
            {
              if (key.Length > 2 && key[1] == ':')
                key = key.Substring(2);
              if (this.NeadedMeterResources[(object) key] == null)
                this.NeadedMeterResources.Add((object) key, (object) 0);
            }
          }
        }
      }
      if (!this.MyFunctionTable.ChangeResourcesFromOverrides())
        return false;
      for (ushort index4 = 0; (int) index4 < this.MyFunctionTable.FunctionNumbersList.Count; ++index4)
      {
        Function function = (Function) fullLoadedFunctions[this.MyFunctionTable.FunctionNumbersList[(int) index4]];
        for (int index5 = 0; index5 < function.NeadedResources.Length; ++index5)
        {
          string neadedResource = function.NeadedResources[index5];
          char[] chArray3 = new char[1]{ '|' };
          foreach (string str in neadedResource.Split(chArray3))
          {
            string key = str.Trim();
            bool Exclusive = false;
            if (key.StartsWith("e:"))
            {
              key = key.Substring(2);
              Exclusive = true;
            }
            MeterResource availableMeterResouce = (MeterResource) this.AvailableMeterResouces[(object) key];
            if (availableMeterResouce != null)
            {
              availableMeterResouce.AddUsing(function.Number, Exclusive);
              break;
            }
          }
        }
      }
      return true;
    }

    internal bool IsMeterResourceAvailable(MeterResources TheResource)
    {
      if (TheResource == MeterResources.NoResource)
        return true;
      MeterResource availableMeterResouce = (MeterResource) this.AvailableMeterResouces[(object) TheResource.ToString()];
      return availableMeterResouce != null && !availableMeterResouce.IsBusy();
    }

    internal bool DeleteIdentData()
    {
      Parameter allParameter1 = (Parameter) this.AllParameters[(object) "EEP_Header.EEP_HEADER_MeterKey"];
      allParameter1.ValueEprom = 0L;
      allParameter1.UpdateByteList();
      Parameter allParameter2 = (Parameter) this.AllParameters[(object) "DefaultFunction.RAM_WriteLimit"];
      allParameter2.ValueEprom = (long) byte.MaxValue;
      allParameter2.UpdateByteList();
      Parameter allParameter3 = (Parameter) this.AllParameters[(object) "EEP_Header.EEP_HEADER_SerialNr"];
      allParameter3.ValueEprom = 0L;
      allParameter3.UpdateByteList();
      Parameter allParameter4 = (Parameter) this.AllParameters[(object) "EEP_Header.EEP_HEADER_MBusSerialNr"];
      allParameter4.ValueEprom = 0L;
      allParameter4.UpdateByteList();
      this.MeterTime = DateTime.Now;
      if (!this.InitialiseAllTimes())
        return false;
      this.GenerateEprom();
      return true;
    }

    internal bool GetOverrideParameterList(out SortedList TheList)
    {
      TheList = new SortedList();
      for (int index = 0; index < this.MyFunctionTable.OverridesList.Count; ++index)
      {
        OverrideParameter byIndex = (OverrideParameter) this.MyFunctionTable.OverridesList.GetByIndex(index);
        if (byIndex.NeadedRessource == MeterResources.NoResource || this.IsMeterResourceAvailable(byIndex.NeadedRessource))
        {
          OverrideParameter overrideParameter = byIndex.Clone();
          if (this.WriteEnable)
            overrideParameter.HasWritePermission = true;
          TheList.Add((object) overrideParameter.ParameterID, (object) overrideParameter);
        }
      }
      int index1 = TheList.IndexOfKey((object) OverrideID.BaseConfig);
      if (index1 >= 0 && OverrideParameter.GetBaseConfigStruct(((ConfigurationParameter) TheList.GetByIndex(index1)).GetStringValueDb()) != null)
      {
        int index2 = TheList.IndexOfKey((object) OverrideID.VolumeResolution);
        if (index2 >= 0)
          ((ConfigurationParameter) TheList.GetByIndex(index2)).AllowedValues = ZelsiusMath.GetRestrictedVolumeUnitList();
      }
      return true;
    }

    internal bool SetOverrideParameterFromList(SortedList TheParameterList)
    {
      for (int index = 0; index < TheParameterList.Count; ++index)
      {
        OverrideParameter byIndex = (OverrideParameter) TheParameterList.GetByIndex(index);
        if (this.MyFunctionTable.OverridesList[(object) byIndex.ParameterID] != null)
          this.MyFunctionTable.OverridesList[(object) byIndex.ParameterID] = (object) byIndex;
        else
          this.MyFunctionTable.OverridesList.Add((object) byIndex.ParameterID, (object) byIndex);
      }
      return true;
    }

    internal bool CompleteTheCloneWithTypeOverride(Meter MeterToOverride, Meter TypeMeter)
    {
      this.MyLinker.LinkBlockList.Add((object) this.MyEpromHeader);
      if (!this.MyLinker.GenerateBlockList() || !this.MyHandler.MyLoadedFunctions.GarantAllFunctionsLoaded(this.MyFunctionTable.FunctionNumbersList) || !this.CreateMeterResourceInformation() || !this.MyFunctionTable.DeleteFunctionsWithMissedResources() || !this.MyLinker.IncludeAllFunctions() || !this.MyCompiler.GenerateMenuObjects() || !this.MyLinker.GenerateObjectLists() || !this.MyLinker.AddMapVariables())
        return false;
      this.UpdateParametersFromExternalParameterList(MeterToOverride.AllParameters, Parameter.ParameterGroups.All);
      if (!this.MyFunctionTable.CopyBaseOverridesToParameter() || !this.MyLinker.UpdateAdressesAtBlockRange((LinkBlock) this.MyLinker.LinkBlockList[0], (LinkBlock) this.MyDisplayCode) || !this.CopyNeadedVars(false) || !this.MyMath.CreateBaseSettings(this.MyFunctionTable.OverridesList))
        return false;
      if (!this.MyMath.CalculateMeterSettings(this.MyIdent.lFirmwareVersion))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, this.MyMath.getLastErrorString());
        return false;
      }
      if (!this.MyDisplayCode.AdjustFunctions() || !this.ReloadNeadedVars() || !this.MyFunctionTable.CopyValuesFromOverriedesToParameter() || !this.MyLinker.UpdateAdressesAtBlockRange((LinkBlock) this.MyDisplayCode, (LinkBlock) null) || !this.MyLinker.CreateParameterAddressLists())
        return false;
      this.MyEpromHeader.ClearChecksumsAndAddresses();
      return this.MyCompiler.CompileFunctions();
    }

    internal int GetBaudrate()
    {
      Parameter allParameter = (Parameter) this.AllParameters[(object) "DefaultFunction.Itr_Prozessabbild"];
      try
      {
        int index1 = this.MyCompiler.Includes.IndexOfKey((object) "ITR_PAB_BAUD_MASK");
        if (index1 < 0)
        {
          int index2 = this.MyCompiler.Includes.IndexOfKey((object) "RUI_PAB_2400BAUD");
          if (index2 < 0)
            return 2400;
          int num = 1 << ((int) this.MyCompiler.Includes.GetByIndex(index2) >> 3);
          return ((int) allParameter.ValueEprom & num) == 0 ? 4800 : 2400;
        }
        int byIndex = (int) this.MyCompiler.Includes.GetByIndex(index1);
        int num1 = (int) allParameter.ValueEprom & byIndex;
        if (num1 == (int) this.MyCompiler.Includes[(object) "ITR_PAB_BAUD_2400"])
          return 2400;
        if (num1 == (int) this.MyCompiler.Includes[(object) "ITR_PAB_BAUD_9600"])
          return 9600;
        if (num1 == (int) this.MyCompiler.Includes[(object) "ITR_PAB_BAUD_38400"])
          return 38400;
        if (num1 == (int) this.MyCompiler.Includes[(object) "ITR_PAB_BAUD_300"])
          return 300;
      }
      catch
      {
      }
      return 2400;
    }

    internal void SetBaudrate(int Baudrate)
    {
      Parameter allParameter = (Parameter) this.AllParameters[(object) "DefaultFunction.Itr_Prozessabbild"];
      try
      {
        int index1 = this.MyCompiler.Includes.IndexOfKey((object) "ITR_PAB_BAUD_MASK");
        if (index1 < 0)
        {
          int index2 = this.MyCompiler.Includes.IndexOfKey((object) "RUI_PAB_2400BAUD");
          if (index2 >= 0)
          {
            long num = (long) (1 << ((int) this.MyCompiler.Includes.GetByIndex(index2) >> 3));
            if (Baudrate == 2400)
            {
              allParameter.ValueEprom |= num;
              return;
            }
            allParameter.ValueEprom &= ~num;
            return;
          }
        }
        else
        {
          long byIndex = (long) (int) this.MyCompiler.Includes.GetByIndex(index1);
          allParameter.ValueEprom &= ~byIndex;
          switch (Baudrate)
          {
            case 300:
              allParameter.ValueEprom = (long) ((int) allParameter.ValueEprom | (int) this.MyCompiler.Includes[(object) "ITR_PAB_BAUD_300"]);
              return;
            case 2400:
              allParameter.ValueEprom = (long) ((int) allParameter.ValueEprom | (int) this.MyCompiler.Includes[(object) "ITR_PAB_BAUD_2400"]);
              return;
            case 9600:
              allParameter.ValueEprom = (long) ((int) allParameter.ValueEprom | (int) this.MyCompiler.Includes[(object) "ITR_PAB_BAUD_9600"]);
              return;
            case 38400:
              allParameter.ValueEprom = (long) ((int) allParameter.ValueEprom | (int) this.MyCompiler.Includes[(object) "ITR_PAB_BAUD_38400"]);
              return;
          }
        }
      }
      catch
      {
      }
      throw new ArgumentException("Data to set baudrate not found");
    }

    internal string[] GetBaudrateList()
    {
      Parameter allParameter = (Parameter) this.AllParameters[(object) "DefaultFunction.Itr_Prozessabbild"];
      string[] baudrateList;
      if (this.MyCompiler.Includes.IndexOfKey((object) "ITR_PAB_BAUD_MASK") < 0)
        baudrateList = new string[2]{ "2400", "4800" };
      else
        baudrateList = new string[4]
        {
          "300",
          "2400",
          "9600",
          "38400"
        };
      return baudrateList;
    }

    internal char GetEEPromWriteProtectionChar(byte[] LinkByteList, int Address)
    {
      if (this.EpromWriteEnable == null && !this.GenerateWriteEnableLists(true))
        this.EpromWriteEnable = (bool[]) null;
      if (this.EpromWriteEnable == null)
        return '?';
      if (LinkByteList == null || Address < 0)
        return '*';
      for (ushort index = (ushort) Address; (int) index < Address + LinkByteList.Length; ++index)
      {
        if (!this.EpromWriteEnable[(int) index])
          return '!';
      }
      return ' ';
    }

    internal char GetRamWriteProtectionChar(int Size, int Address)
    {
      if (this.RamWriteEnable == null && !this.GenerateWriteEnableLists(true))
        this.RamWriteEnable = (bool[]) null;
      if (this.RamWriteEnable == null)
        return '?';
      if (Size == 0 || Address < 0)
        return '*';
      for (ushort index = (ushort) Address; (int) index < Address + Size; ++index)
      {
        if (!this.RamWriteEnable[(int) index])
          return '!';
      }
      return ' ';
    }

    internal SortedList<MeterDBAccess.ValueTypes, string> GetActualValues()
    {
      if (this.RamCacheSize == 0)
      {
        int num1 = 0;
        this.RamCacheAddress = 16777215;
        for (int index1 = 0; index1 < Meter.ActualVariablesList.Length; ++index1)
        {
          int index2 = this.AllParameters.IndexOfKey((object) Meter.ActualVariablesList[index1]);
          if (index2 > 0)
          {
            Parameter byIndex = (Parameter) this.AllParameters.GetByIndex(index2);
            int addressCpu = byIndex.AddressCPU;
            int num2 = addressCpu + byIndex.Size - 1;
            if (addressCpu < this.RamCacheAddress)
              this.RamCacheAddress = addressCpu;
            if (num2 > num1)
              num1 = num2;
          }
        }
        this.RamCacheSize = num1 - this.RamCacheAddress + 1;
      }
      if (this.MyCommunication.CacheParameterValues(this.RamCacheAddress, this.RamCacheSize))
      {
        SortedList<MeterDBAccess.ValueTypes, string> actualValues = new SortedList<MeterDBAccess.ValueTypes, string>();
        Decimal num3 = (Decimal) Math.Pow(10.0, (double) MeterMath.EnergyUnits[this.MyMath.MyBaseSettings.EnergyUnitIndex].AfterPointDigits);
        Decimal ScaleFactor1 = (Decimal) Math.Pow(2.0, (double) this.MyMath.MyBaseSettings.Energy_SumExpo) * num3;
        string str1 = " " + MeterMath.GetUnitText(MeterMath.EnergyUnits[this.MyMath.MyBaseSettings.EnergyUnitIndex].EnergieUnitString);
        string str2 = " " + MeterMath.GetUnitText(MeterMath.EnergyUnits[this.MyMath.MyBaseSettings.EnergyUnitIndex].PowerUnitString);
        Decimal num4 = (Decimal) Math.Pow(10.0, (double) MeterMath.VolumeUnits[this.MyMath.MyBaseSettings.VolumeUnitIndex].AfterPointDigits);
        Decimal ScaleFactor2 = (Decimal) Math.Pow(2.0, (double) this.MyMath.MyBaseSettings.Vol_SumExpo) * num4;
        string str3 = " " + MeterMath.GetUnitText(MeterMath.VolumeUnits[this.MyMath.MyBaseSettings.VolumeUnitIndex].VolumeUnitString);
        string str4 = " " + MeterMath.GetUnitText(MeterMath.VolumeUnits[this.MyMath.MyBaseSettings.VolumeUnitIndex].FlowUnitString);
        int index3 = this.AllParameters.IndexOfKey((object) "DefaultFunction.Waerme_EnergSum");
        if (index3 > 0)
        {
          Parameter byIndex = (Parameter) this.AllParameters.GetByIndex(index3);
          if (this.MyCommunication.ReadParameterValue(byIndex, MemoryLocation.RAM))
          {
            string str5 = new OverrideParameter(OverrideID.EnergyActualValue, (ulong) byIndex.ValueCPU, ScaleFactor1).GetStringValueWin() + str1;
            actualValues.Add(MeterDBAccess.ValueTypes.HeatMeterEnergy, str5);
          }
          else
            goto label_55;
        }
        int index4 = this.AllParameters.IndexOfKey((object) "DefaultFunction.Kaelte_EnergSum");
        if (index4 > 0)
        {
          Parameter byIndex = (Parameter) this.AllParameters.GetByIndex(index4);
          if (this.MyCommunication.ReadParameterValue(byIndex, MemoryLocation.RAM))
          {
            string str6 = new OverrideParameter(OverrideID.CEnergyActualValue, (ulong) byIndex.ValueCPU, ScaleFactor1).GetStringValueWin() + str1;
            actualValues.Add(MeterDBAccess.ValueTypes.HeatMeterNegativeEnergy, str6);
          }
          else
            goto label_55;
        }
        int index5 = this.AllParameters.IndexOfKey((object) "DefaultFunction.Vol_VolSum");
        if (index5 > 0)
        {
          Parameter byIndex = (Parameter) this.AllParameters.GetByIndex(index5);
          if (this.MyCommunication.ReadParameterValue(byIndex, MemoryLocation.RAM))
          {
            string str7 = new OverrideParameter(OverrideID.VolumeActualValue, (ulong) byIndex.ValueCPU, ScaleFactor2).GetStringValueWin() + str3;
            actualValues.Add(MeterDBAccess.ValueTypes.HeatMeterVolume, str7);
          }
          else
            goto label_55;
        }
        int index6 = this.AllParameters.IndexOfKey((object) "DefaultFunction.TAR_EnergySumTar0");
        if (index6 > 0)
        {
          Parameter byIndex = (Parameter) this.AllParameters.GetByIndex(index6);
          if (this.MyCommunication.ReadParameterValue(byIndex, MemoryLocation.RAM))
          {
            string str8 = new OverrideParameter(OverrideID.TarifEnergy0, (ulong) byIndex.ValueCPU, ScaleFactor1).GetStringValueWin() + str1;
            actualValues.Add(MeterDBAccess.ValueTypes.HeatMeterTarifEnergy0, str8);
          }
          else
            goto label_55;
        }
        int index7 = this.AllParameters.IndexOfKey((object) "DefaultFunction.TAR_EnergySumTar1");
        if (index7 > 0)
        {
          Parameter byIndex = (Parameter) this.AllParameters.GetByIndex(index7);
          if (this.MyCommunication.ReadParameterValue(byIndex, MemoryLocation.RAM))
          {
            string str9 = new OverrideParameter(OverrideID.TarifEnergy1, (ulong) byIndex.ValueCPU, ScaleFactor1).GetStringValueWin() + str1;
            actualValues.Add(MeterDBAccess.ValueTypes.HeatMeterTarifEnergy1, str9);
          }
          else
            goto label_55;
        }
        Decimal ScaleFactor3 = (Decimal) Math.Pow(10.0, (double) MeterMath.InputUnits[this.MyMath.MyBaseSettings.Input1UnitIndex].AfterPointDigits);
        string str10 = " " + MeterMath.GetUnitText(MeterMath.InputUnits[this.MyMath.MyBaseSettings.Input1UnitIndex].InputUnitString);
        int index8 = this.AllParameters.IndexOfKey((object) "DefaultFunction.In1Display");
        if (index8 > 0)
        {
          Parameter byIndex = (Parameter) this.AllParameters.GetByIndex(index8);
          if (this.MyCommunication.ReadParameterValue(byIndex, MemoryLocation.RAM))
          {
            string str11 = new OverrideParameter(OverrideID.Input1ActualValue, (ulong) byIndex.ValueCPU, ScaleFactor3).GetStringValueWin() + str10;
            actualValues.Add(MeterDBAccess.ValueTypes.VolumeCounter1Input, str11);
          }
          else
            goto label_55;
        }
        Decimal ScaleFactor4 = (Decimal) Math.Pow(10.0, (double) MeterMath.InputUnits[this.MyMath.MyBaseSettings.Input2UnitIndex].AfterPointDigits);
        string str12 = " " + MeterMath.GetUnitText(MeterMath.InputUnits[this.MyMath.MyBaseSettings.Input2UnitIndex].InputUnitString);
        int index9 = this.AllParameters.IndexOfKey((object) "DefaultFunction.In2Display");
        if (index9 > 0)
        {
          Parameter byIndex = (Parameter) this.AllParameters.GetByIndex(index9);
          if (this.MyCommunication.ReadParameterValue(byIndex, MemoryLocation.RAM))
          {
            string str13 = new OverrideParameter(OverrideID.Input2ActualValue, (ulong) byIndex.ValueCPU, ScaleFactor4).GetStringValueWin() + str12;
            actualValues.Add(MeterDBAccess.ValueTypes.VolumeCounter2Input, str13);
          }
          else
            goto label_55;
        }
        int index10 = this.AllParameters.IndexOfKey((object) "vorlauftemperatur");
        if (index10 > 0)
        {
          Parameter byIndex = (Parameter) this.AllParameters.GetByIndex(index10);
          if (this.MyCommunication.ReadParameterValue(byIndex, MemoryLocation.RAM))
          {
            string str14 = ((double) (short) byIndex.ValueCPU / 100.0).ToString("0.00") + " °C";
            actualValues.Add(MeterDBAccess.ValueTypes.HeatMeterUpperTemperature, str14);
          }
          else
            goto label_55;
        }
        int index11 = this.AllParameters.IndexOfKey((object) "ruecklauftemperatur");
        if (index11 > 0)
        {
          Parameter byIndex = (Parameter) this.AllParameters.GetByIndex(index11);
          if (this.MyCommunication.ReadParameterValue(byIndex, MemoryLocation.RAM))
          {
            string str15 = ((double) (short) byIndex.ValueCPU / 100.0).ToString("0.00") + " °C";
            actualValues.Add(MeterDBAccess.ValueTypes.HeatMeterLowerTemperature, str15);
          }
          else
            goto label_55;
        }
        int index12 = this.AllParameters.IndexOfKey((object) "delta_t");
        if (index12 > 0)
        {
          Parameter byIndex = (Parameter) this.AllParameters.GetByIndex(index12);
          if (this.MyCommunication.ReadParameterValue(byIndex, MemoryLocation.RAM))
          {
            string str16 = ((double) (short) byIndex.ValueCPU / 100.0).ToString("0.00") + " °C";
            actualValues.Add(MeterDBAccess.ValueTypes.HeatMeterTemperatureDifference, str16);
          }
          else
            goto label_55;
        }
        int index13 = this.AllParameters.IndexOfKey((object) "Vol_Flow");
        if (index13 > 0)
        {
          Parameter byIndex = (Parameter) this.AllParameters.GetByIndex(index13);
          if (this.MyCommunication.ReadParameterValue(byIndex, MemoryLocation.RAM))
          {
            string str17 = byIndex.ValueCPU != 0L ? ((Decimal) byIndex.ValueCPU / num4).ToString() + str4 : "0" + str4;
            actualValues.Add(MeterDBAccess.ValueTypes.HeatMeterFlow, str17);
          }
          else
            goto label_55;
        }
        int index14 = this.AllParameters.IndexOfKey((object) "Leistung");
        if (index14 > 0)
        {
          Parameter byIndex = (Parameter) this.AllParameters.GetByIndex(index14);
          if (this.MyCommunication.ReadParameterValue(byIndex, MemoryLocation.RAM))
          {
            string str18 = byIndex.ValueCPU != 0L ? ((Decimal) byIndex.ValueCPU / num3).ToString() + str2 : "0" + str4;
            actualValues.Add(MeterDBAccess.ValueTypes.HeatMeterPower, str18);
          }
          else
            goto label_55;
        }
        int index15 = this.AllParameters.IndexOfKey((object) "DefaultFunction.Sta_Secounds");
        if (index15 > 0)
        {
          Parameter byIndex = (Parameter) this.AllParameters.GetByIndex(index15);
          if (this.MyCommunication.ReadParameterValue(byIndex, MemoryLocation.RAM))
          {
            string str19 = ZR_Calendar.Cal_GetDateTime((uint) byIndex.ValueCPU).ToString("dd.MM.yyyy_HH:mm:ss");
            actualValues.Add(MeterDBAccess.ValueTypes.DeviceDateTime, str19);
          }
          else
            goto label_55;
        }
        int index16 = this.AllParameters.IndexOfKey((object) "Itr_NextIntervalTime");
        if (index16 > 0)
        {
          Parameter byIndex = (Parameter) this.AllParameters.GetByIndex(index16);
          if (this.MyCommunication.ReadParameterValue(byIndex, MemoryLocation.RAM))
          {
            string str20 = ZR_Calendar.Cal_GetDateTime((uint) byIndex.ValueCPU).ToString("dd.MM.yyyy_HH:mm:ss");
            actualValues.Add(MeterDBAccess.ValueTypes.DeviceNextEventDateTime, str20);
          }
          else
            goto label_55;
        }
        this.MyCommunication.CacheParameterValues(0, 0);
        return actualValues;
      }
label_55:
      this.MyCommunication.CacheParameterValues(0, 0);
      return (SortedList<MeterDBAccess.ValueTypes, string>) null;
    }

    internal GlobalDeviceId GetGlobalDeviceIdentification()
    {
      GlobalDeviceId deviceIdentification = new GlobalDeviceId();
      int index1 = this.MyFunctionTable.OverridesList.IndexOfKey((object) OverrideID.SerialNumber);
      if (index1 >= 0)
      {
        ConfigurationParameter byIndex = (ConfigurationParameter) this.MyFunctionTable.OverridesList.GetByIndex(index1);
        deviceIdentification.Serialnumber = byIndex.GetStringValueWin();
      }
      if (this.MyFunctionTable.OverridesList.IndexOfKey((object) OverrideID.Input1PulsValue) >= 0)
      {
        deviceIdentification.SubDevices = new List<GlobalDeviceId>();
        GlobalDeviceId globalDeviceId = new GlobalDeviceId();
        deviceIdentification.SubDevices.Add(globalDeviceId);
        int index2 = this.MyFunctionTable.OverridesList.IndexOfKey((object) OverrideID.Input1IdNumber);
        if (index2 >= 0)
        {
          ConfigurationParameter byIndex = (ConfigurationParameter) this.MyFunctionTable.OverridesList.GetByIndex(index2);
          globalDeviceId.Serialnumber = byIndex.GetStringValueWin();
        }
      }
      if (this.MyFunctionTable.OverridesList.IndexOfKey((object) OverrideID.Input2PulsValue) >= 0)
      {
        GlobalDeviceId globalDeviceId = new GlobalDeviceId();
        deviceIdentification.SubDevices.Add(globalDeviceId);
        int index3 = this.MyFunctionTable.OverridesList.IndexOfKey((object) OverrideID.Input2IdNumber);
        if (index3 >= 0)
        {
          ConfigurationParameter byIndex = (ConfigurationParameter) this.MyFunctionTable.OverridesList.GetByIndex(index3);
          globalDeviceId.Serialnumber = byIndex.GetStringValueWin();
        }
      }
      return deviceIdentification;
    }

    internal SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      ConfigurationParameter.ValueType ConfigurationType,
      int SubDevice)
    {
      SortedList<OverrideID, ConfigurationParameter> configurationParameters = new SortedList<OverrideID, ConfigurationParameter>();
      for (int index1 = 0; index1 < this.MyFunctionTable.OverridesList.Count; ++index1)
      {
        ConfigurationParameter byIndex = (ConfigurationParameter) this.MyFunctionTable.OverridesList.GetByIndex(index1);
        int index2 = ConfigurationParameter.ConfigParametersByOverrideID.IndexOfKey(byIndex.ParameterID);
        if (index2 >= 0)
        {
          ConfigurationParameter.ConPaInfo conPaInfo = ConfigurationParameter.ConfigParametersByOverrideID.Values[index2];
          if (conPaInfo.SubdeviceNumber == SubDevice)
          {
            if (conPaInfo.NewOverrideId == OverrideID.Unknown)
            {
              configurationParameters.Add(byIndex.ParameterID, byIndex);
            }
            else
            {
              ConfigurationParameter configurationParameter = byIndex.CloneWithNewId(conPaInfo.NewOverrideId);
              configurationParameters.Add(configurationParameter.ParameterID, configurationParameter);
            }
          }
        }
      }
      return configurationParameters;
    }

    internal bool SetConfigurationParameter(
      SortedList<OverrideID, ConfigurationParameter> parameterList,
      int SubDevice)
    {
      return false;
    }

    internal enum MeterDataStates
    {
      New,
      Connected,
      MapComplete,
      CloneComplete,
      EpromDataReloaded,
    }
  }
}
