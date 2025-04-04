// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.S4_AllMeters
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using GmmDbLib;
using GmmDbLib.DataSets;
using HandlerLib;
using NLog;
using SmartFunctionCompiler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace S4_Handler.Functions
{
  internal class S4_AllMeters : ICreateMeter
  {
    internal static Logger Base_S4_AllMeters_Logger = LogManager.GetLogger(nameof (S4_AllMeters));
    internal ChannelLogger S4_AllMeters_Logger;
    private S4_HandlerFunctions myFunctions;
    internal S4_Meter ConnectedMeter;
    internal S4_Meter WorkMeter;
    internal S4_Meter TypeMeter;
    internal S4_Meter BackupMeter;
    internal S4_Meter SavedMeter;
    internal S4_Meter[] UndoMeters = new S4_Meter[3];
    private static SortedList<int, S4_AllMeters.TypeCacheData> TypeCache = new SortedList<int, S4_AllMeters.TypeCacheData>();
    internal List<AddressRangeInfo> ReadRangesList;
    internal List<AddressRangeInfo> WriteRangesList;
    private static OverrideID[] ConfigurationParameterOverwriteWhiteList = new OverrideID[11]
    {
      OverrideID.AESKey,
      OverrideID.BatteryCapacity_mAh,
      OverrideID.CommunicationScenario,
      OverrideID.DeviceUnit,
      OverrideID.DisplayMenu,
      OverrideID.DueDate,
      OverrideID.EndOfBatteryDate,
      OverrideID.JoinEUI,
      OverrideID.SmartFunctions,
      OverrideID.TimeZone,
      OverrideID.VolumeResolution
    };
    private DeviceMemory DirectWriteData;

    internal static void TypeCacheClear()
    {
      lock (S4_AllMeters.TypeCache)
        S4_AllMeters.TypeCache.Clear();
    }

    internal S4_Meter checkedConnectedMeter
    {
      get
      {
        return this.ConnectedMeter != null ? this.ConnectedMeter : throw new Exception("ConnectedMeter not defined");
      }
    }

    internal S4_Meter checkedWorkMeter
    {
      get => this.WorkMeter != null ? this.WorkMeter : throw new Exception("WorkMeter not defined");
    }

    internal S4_Meter checkedChangeMeter
    {
      get
      {
        if (this.ConnectedMeter == null)
          throw new Exception("ConnectedMeter not available");
        return this.WorkMeter != null ? this.WorkMeter : throw new Exception("WorkMeter not available");
      }
    }

    internal S4_AllMeters(S4_HandlerFunctions myFunctions)
    {
      this.myFunctions = myFunctions;
      this.S4_AllMeters_Logger = new ChannelLogger(S4_AllMeters.Base_S4_AllMeters_Logger, this.myFunctions.configList);
    }

    public IMeter CreateMeter(byte[] zippedBuffer)
    {
      return (IMeter) new S4_Meter().CreateFromData(zippedBuffer, new int?(), this.myFunctions.myHardwareTypeSupport);
    }

    public IMeter CreateMeter(byte[] zippedBuffer, int? hardwareTypeID = null)
    {
      return (IMeter) new S4_Meter().CreateFromData(zippedBuffer, hardwareTypeID, this.myFunctions.myHardwareTypeSupport);
    }

    public string SetBackupMeter(BaseTables.MeterDataRow backupRow)
    {
      string str = this.SetBackupMeter(backupRow.PValueBinary);
      this.BackupMeter.BackupTime = new DateTime?(backupRow.TimePoint);
      return str;
    }

    public string SetBackupMeter(byte[] zippedBuffer)
    {
      this.BackupMeter = this.CreateMeter(zippedBuffer) as S4_Meter;
      if (this.WorkMeter == null)
        this.WorkMeter = this.BackupMeter.Clone();
      return this.BackupMeter.GetInfo();
    }

    internal void CloneWorkMeterForUndo()
    {
      S4_AllMeters.Base_S4_AllMeters_Logger.Trace(nameof (CloneWorkMeterForUndo));
      S4_Meter s4Meter = this.WorkMeter.Clone();
      for (int index = this.UndoMeters.Length - 1; index > 1; --index)
        this.UndoMeters[index] = this.UndoMeters[index - 1];
      this.UndoMeters[0] = this.WorkMeter;
      this.WorkMeter = s4Meter;
    }

    internal void Undo()
    {
      if (this.UndoMeters[0] == null)
        throw new Exception("Undo for not saved WorkMeter");
      S4_AllMeters.Base_S4_AllMeters_Logger.Trace("Undo WorkMeter");
      this.WorkMeter = this.UndoMeters[0];
      for (int index = 0; index < this.UndoMeters.Length - 2; ++index)
        this.UndoMeters[index] = this.UndoMeters[index + 1];
      this.WorkMeter.meterMemory.CloneCreated = false;
    }

    public async Task ReadDeviceAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      ReadPartsSelection readPartsSelection)
    {
      this.ReadRangesList = new List<AddressRangeInfo>();
      Stopwatch readStopWatch = new Stopwatch();
      readStopWatch.Start();
      try
      {
        progress.LoggerMark();
        progress.BaseMessage = "Read version ";
        DeviceIdentification deviceIdentification = await this.myFunctions.ReadVersionAsync(progress, cancelToken);
        this.ConnectedMeter = new S4_Meter(deviceIdentification);
        if ((readPartsSelection & ReadPartsSelection.ProtocolOnlyMode) > ~(ReadPartsSelection.Dump | ReadPartsSelection.ProtocolOnlyMode))
        {
          readStopWatch = (Stopwatch) null;
        }
        else
        {
          progress.LoggerMark();
          if ((readPartsSelection & ReadPartsSelection.Identification) > ~(ReadPartsSelection.Dump | ReadPartsSelection.ProtocolOnlyMode))
          {
            progress.BaseMessage = "Identification ";
            if (deviceIdentification.SvnRevision.HasValue && this.ConnectedMeter.meterMemory.IsParameterInMap(S4_Params.SvnRevision_Value) && !this.myFunctions.MapCheckDisabled)
            {
              uint mapSvnRevision = this.ConnectedMeter.meterMemory.GetParameterAddress(S4_Params.SvnRevision_Value);
              uint? svnRevision = deviceIdentification.SvnRevision;
              uint num = mapSvnRevision;
              if (!((int) svnRevision.GetValueOrDefault() == (int) num & svnRevision.HasValue))
              {
                svnRevision = deviceIdentification.SvnRevision;
                throw new Exception("Firmware revision: " + svnRevision.ToString() + " not equal to map revision: " + mapSvnRevision.ToString());
              }
            }
            List<AddressRange> MemRanges = this.ConnectedMeter.meterMemory.GetDefinedParameterReadingRanges(S4_DeviceMemory.ParameterGroups[CommonOverwriteGroups.IdentData], 20U);
            this.ReadRangesList.AddRange((IEnumerable<AddressRangeInfo>) AddressRangeInfo.GetRangeInfos("Identification", MemRanges));
            progress.Split(MemRanges.Count);
            foreach (AddressRange theRange in MemRanges)
              await this.myFunctions.checkedNfcCommands.ReadMemoryAsync(theRange, (DeviceMemory) this.ConnectedMeter.meterMemory, progress, cancelToken);
            MemRanges = (List<AddressRange>) null;
          }
          progress.LoggerMark();
          if ((readPartsSelection & ReadPartsSelection.EnhancedIdentification) > ~(ReadPartsSelection.Dump | ReadPartsSelection.ProtocolOnlyMode))
          {
            progress.BaseMessage = "Enhanced identification ";
            AddressRange compilerInfo = this.ConnectedMeter.meterMemory.GetRangeOfParameters(S4_DeviceMemory.CompilerInfo);
            this.ReadRangesList.Add(new AddressRangeInfo("Compiler info", compilerInfo));
            await this.myFunctions.checkedNfcCommands.ReadMemoryAsync(compilerInfo, (DeviceMemory) this.ConnectedMeter.meterMemory, progress, cancelToken);
            this.ReadRangesList.Add(new AddressRangeInfo("Arm ID", this.ConnectedMeter.meterMemory.ArmIdRange));
            await this.myFunctions.checkedNfcCommands.ReadMemoryAsync(this.ConnectedMeter.meterMemory.ArmIdRange, (DeviceMemory) this.ConnectedMeter.meterMemory, progress, cancelToken);
            List<AddressRange> MemRanges = this.ConnectedMeter.meterMemory.GetDefinedParameterReadingRanges(S4_DeviceMemory.ParameterGroups[CommonOverwriteGroups.DeviceProtection], 20U);
            this.ReadRangesList.AddRange((IEnumerable<AddressRangeInfo>) AddressRangeInfo.GetRangeInfos("Enhanced ident", MemRanges));
            foreach (AddressRange theRange in MemRanges)
              await this.myFunctions.checkedNfcCommands.ReadMemoryAsync(theRange, (DeviceMemory) this.ConnectedMeter.meterMemory, progress, cancelToken);
            compilerInfo = (AddressRange) null;
            MemRanges = (List<AddressRange>) null;
          }
          progress.LoggerMark();
          if ((readPartsSelection & (ReadPartsSelection.LoggersMask | ReadPartsSelection.CumulatedDataMask)) > ~(ReadPartsSelection.Dump | ReadPartsSelection.ProtocolOnlyMode))
          {
            progress.BaseMessage = "RAM ";
            this.ReadRangesList.AddRange((IEnumerable<AddressRangeInfo>) AddressRangeInfo.GetRangeInfos("RAM", this.ConnectedMeter.meterMemory.UsedRamRanges));
            foreach (AddressRange theRange in this.ConnectedMeter.meterMemory.UsedRamRanges)
              await this.myFunctions.checkedNfcCommands.ReadMemoryAsync(theRange, (DeviceMemory) this.ConnectedMeter.meterMemory, progress, cancelToken);
          }
          progress.LoggerMark();
          if ((readPartsSelection & ReadPartsSelection.MonthLogger) > ~(ReadPartsSelection.Dump | ReadPartsSelection.ProtocolOnlyMode) && new FirmwareVersion(this.ConnectedMeter.deviceIdentification.FirmwareVersion.Value) >= (object) "1.4.8 IUW")
          {
            progress.BaseMessage = "Month logger ";
            await S4_LoggerManager.ReadMonthLoggerMemory(this.ConnectedMeter.meterMemory, this.myFunctions.checkedNfcCommands, progress, cancelToken);
          }
          progress.LoggerMark();
          if ((readPartsSelection & ReadPartsSelection.ConfigMask) > ~(ReadPartsSelection.Dump | ReadPartsSelection.ProtocolOnlyMode))
          {
            progress.BaseMessage = "Flash ";
            uint flashEndAddress = this.ConnectedMeter.meterMemory.GetParameterAddress(S4_Params.CRC_FW) + 10U;
            List<AddressRange> flashRanges = new List<AddressRange>();
            foreach (DeviceMemoryStorage deviceMemoryStorage in (IEnumerable<DeviceMemoryStorage>) this.ConnectedMeter.meterMemory.MemoryBlockList.Values)
            {
              DeviceMemoryStorage block = deviceMemoryStorage;
              if (block.MemoryType == DeviceMemoryType.FLASH && block.StartAddress < flashEndAddress)
              {
                AddressRange blockRange = block.GetAddressRange();
                if (blockRange.EndAddress > flashEndAddress)
                  blockRange = new AddressRange(blockRange.StartAddress, (uint) ((int) flashEndAddress - (int) blockRange.StartAddress + 1));
                if ((readPartsSelection & ReadPartsSelection.Calibration) == ~(ReadPartsSelection.Dump | ReadPartsSelection.ProtocolOnlyMode) && this.ConnectedMeter.meterMemory.CalibrationRange != null)
                {
                  List<AddressRange> ranges = AddressRange.GetRangesByExcludeRange(blockRange, this.ConnectedMeter.meterMemory.CalibrationRange);
                  flashRanges.AddRange((IEnumerable<AddressRange>) ranges);
                  ranges = (List<AddressRange>) null;
                }
                else
                  flashRanges.Add(blockRange);
                blockRange = (AddressRange) null;
                block = (DeviceMemoryStorage) null;
              }
            }
            this.ReadRangesList.AddRange((IEnumerable<AddressRangeInfo>) AddressRangeInfo.GetRangeInfos("Flash", flashRanges));
            foreach (AddressRange theRange in flashRanges)
              await this.myFunctions.checkedNfcCommands.ReadMemoryAsync(theRange, (DeviceMemory) this.ConnectedMeter.meterMemory, progress, cancelToken);
            progress.LoggerMark();
            progress.BaseMessage = "EEPROM config ";
            uint NFC_ProtocolBytes = 30;
            AddressRange CONF_PARA_MAN_Range = this.ConnectedMeter.meterMemory.MapDef.GetSectionRanges("CONF_PARA_MAN");
            AddressRange CONF_PARA_MAN_Start_Range = new AddressRange(CONF_PARA_MAN_Range.StartAddress, NFC_ProtocolBytes);
            this.ReadRangesList.Add(new AddressRangeInfo("Config param start", CONF_PARA_MAN_Start_Range));
            await this.myFunctions.checkedNfcCommands.ReadMemoryAsync(CONF_PARA_MAN_Start_Range, (DeviceMemory) this.ConnectedMeter.meterMemory, progress, cancelToken);
            uint managedParameterBytes = this.ConnectedMeter.meterMemory.GetParameterAddressRange(S4_Params.managedParam).ByteSize;
            int manParamEndCount = (int) managedParameterBytes - (int) NFC_ProtocolBytes;
            if (manParamEndCount > 0)
            {
              AddressRange manParam_End_Range = new AddressRange(CONF_PARA_MAN_Range.StartAddress + NFC_ProtocolBytes, (uint) manParamEndCount);
              this.ReadRangesList.Add(new AddressRangeInfo("Config param", manParam_End_Range));
              await this.myFunctions.checkedNfcCommands.ReadMemoryAsync(manParam_End_Range, (DeviceMemory) this.ConnectedMeter.meterMemory, progress, cancelToken);
              manParam_End_Range = (AddressRange) null;
            }
            progress.LoggerMark();
            progress.BaseMessage = "EEPROM display ";
            NFC_ProtocolBytes = 30U;
            AddressRange CONF_DISP_Range = this.ConnectedMeter.meterMemory.MapDef.GetSectionRanges("CONF_DISP");
            AddressRange CONF_DISP_Start_Range = new AddressRange(CONF_DISP_Range.StartAddress, NFC_ProtocolBytes);
            this.ReadRangesList.Add(new AddressRangeInfo("Config display start", CONF_DISP_Start_Range));
            await this.myFunctions.checkedNfcCommands.ReadMemoryAsync(CONF_DISP_Start_Range, (DeviceMemory) this.ConnectedMeter.meterMemory, progress, cancelToken);
            uint Disp_MenuSize = this.ConnectedMeter.meterMemory.GetParameterValue<uint>(S4_Params.Disp_MenuSize);
            int CONF_DISP_EndSize = (int) Disp_MenuSize * 4 + 4 - (int) NFC_ProtocolBytes;
            if (CONF_DISP_EndSize > 0)
            {
              AddressRange CONF_DISP_End_Range = new AddressRange(CONF_DISP_Range.StartAddress + NFC_ProtocolBytes, (uint) CONF_DISP_EndSize);
              this.ReadRangesList.Add(new AddressRangeInfo("Config display", CONF_DISP_End_Range));
              await this.myFunctions.checkedNfcCommands.ReadMemoryAsync(CONF_DISP_End_Range, (DeviceMemory) this.ConnectedMeter.meterMemory, progress, cancelToken);
              CONF_DISP_End_Range = (AddressRange) null;
            }
            progress.BaseMessage = "Readout group selection ";
            uint selParamCount = (uint) this.ConnectedMeter.meterMemory.GetParameterValue<ushort>(S4_Params.selParamCount);
            uint usedConfigBytes = selParamCount * 20U;
            AddressRange configAddressRange = new AddressRange(this.ConnectedMeter.meterMemory.SelectedReadoutParametersRange.StartAddress, usedConfigBytes);
            this.ReadRangesList.Add(new AddressRangeInfo("Selected groups", configAddressRange));
            await this.myFunctions.checkedNfcCommands.ReadMemoryAsync(configAddressRange, (DeviceMemory) this.ConnectedMeter.meterMemory, progress, cancelToken);
            flashRanges = (List<AddressRange>) null;
            CONF_PARA_MAN_Range = (AddressRange) null;
            CONF_PARA_MAN_Start_Range = (AddressRange) null;
            CONF_DISP_Range = (AddressRange) null;
            CONF_DISP_Start_Range = (AddressRange) null;
            configAddressRange = (AddressRange) null;
          }
          progress.LoggerMark();
          if ((readPartsSelection & ReadPartsSelection.BackupBlocks) != ~(ReadPartsSelection.Dump | ReadPartsSelection.ProtocolOnlyMode) && this.ConnectedMeter.meterMemory.FlashBackupRange != null)
          {
            progress.BaseMessage = "Flash backup ";
            this.ConnectedMeter.meterMemory.GarantMemoryAvailable(this.ConnectedMeter.meterMemory.FlashBackupRange);
            this.ReadRangesList.Add(new AddressRangeInfo("Flash backup", this.ConnectedMeter.meterMemory.FlashBackupRange));
            await this.myFunctions.checkedNfcCommands.ReadMemoryAsync(this.ConnectedMeter.meterMemory.FlashBackupRange, (DeviceMemory) this.ConnectedMeter.meterMemory, progress, cancelToken);
          }
          progress.LoggerMark();
          if ((readPartsSelection & ReadPartsSelection.SmartFunctions) > ~(ReadPartsSelection.Dump | ReadPartsSelection.ProtocolOnlyMode))
          {
            AddressRange sf_FlashRange = this.ConnectedMeter.meterMemory.MapDef.GetSectionRanges("SMART_FUNC_FL");
            AddressRange sf_RamRange = this.ConnectedMeter.meterMemory.MapDef.GetSectionRanges("SMART_FUNC_RA");
            AddressRange sf_BackupRange = this.ConnectedMeter.meterMemory.MapDef.GetSectionRanges("SMART_FUNC_BA");
            if (this.ConnectedMeter.meterMemory.IsParameterInMap(S4_Params.sfunc_offset_absolut) && this.ConnectedMeter.meterMemory.SmartFunctionFlashRange != null && sf_FlashRange != null && sf_RamRange != null && sf_BackupRange != null)
            {
              progress.BaseMessage = "Smart functions ";
              this.ConnectedMeter.MySmartFunctionManager = new S4_SmartFunctionManager(this.myFunctions.checkedNfcCommands);
              S4_SmartFunctionInfo smartFunctionInfo = await this.ConnectedMeter.MySmartFunctionManager.ReadSmartFunctionInfoAsync(progress, cancelToken);
              if (this.ConnectedMeter.MySmartFunctionManager.SmartFunctionInfo.NumberOfLoadedFunctions > (byte) 0)
              {
                AddressRange offsetDataRange = this.ConnectedMeter.meterMemory.GetParameterAddressRange(S4_Params.sfunc_offset_absolut);
                this.ReadRangesList.Add(new AddressRangeInfo("Smart functions storage offsets", offsetDataRange));
                await this.myFunctions.checkedNfcCommands.ReadMemoryAsync(offsetDataRange, (DeviceMemory) this.ConnectedMeter.meterMemory, progress, cancelToken);
                ulong offsets = this.ConnectedMeter.meterMemory.GetParameterValue<ulong>(S4_Params.sfunc_offset_absolut);
                ushort flash_func = (ushort) offsets;
                ushort flash_param = (ushort) (offsets >> 16);
                ushort ram_param = (ushort) (offsets >> 32);
                ushort backup_param = (ushort) (offsets >> 48);
                AddressRange flashFuncRange = new AddressRange(sf_FlashRange.StartAddress, (uint) flash_func + 2U);
                uint flashParamStartAdr = (uint) ((int) sf_FlashRange.EndAddress - (int) flash_param + 1);
                uint flashParamSize = (uint) ((int) sf_FlashRange.EndAddress - (int) flashParamStartAdr + 1);
                AddressRange flashParamRange = new AddressRange(flashParamStartAdr, flashParamSize);
                AddressRange ramParamRange = new AddressRange(sf_RamRange.StartAddress, (uint) ram_param);
                AddressRange backupParamRange = new AddressRange(sf_BackupRange.StartAddress, (uint) backup_param);
                if (flashFuncRange.EndAddress > sf_FlashRange.EndAddress)
                  throw new Exception("Smart function code range out of flash range");
                if (flashParamRange.StartAddress <= flashFuncRange.EndAddress)
                  throw new Exception("Smart function code and parameter ranges overlap");
                if (flashParamRange.EndAddress > sf_FlashRange.EndAddress)
                  throw new Exception("Smart function parameter ranges out of flash range");
                if (ramParamRange.EndAddress > sf_RamRange.EndAddress)
                  throw new Exception("Smart function ram ranges out of ram storage range");
                if (backupParamRange.EndAddress > sf_BackupRange.EndAddress)
                  throw new Exception("Smart function backup ranges out of backup storage range");
                progress.BaseMessage = "Smart functions code";
                this.ReadRangesList.Add(new AddressRangeInfo("Smart function code", flashFuncRange));
                await this.myFunctions.checkedNfcCommands.ReadMemoryAsync(flashFuncRange, (DeviceMemory) this.ConnectedMeter.meterMemory, progress, cancelToken);
                progress.BaseMessage = "Smart functions flash";
                if (flashParamRange.ByteSize > 0U)
                {
                  this.ReadRangesList.Add(new AddressRangeInfo("Smart function flash param", flashParamRange));
                  await this.myFunctions.checkedNfcCommands.ReadMemoryAsync(flashParamRange, (DeviceMemory) this.ConnectedMeter.meterMemory, progress, cancelToken);
                }
                progress.BaseMessage = "Smart functions RAM";
                if (ramParamRange.ByteSize > 0U)
                {
                  this.ReadRangesList.Add(new AddressRangeInfo("Smart function RAM param", ramParamRange));
                  await this.myFunctions.checkedNfcCommands.ReadMemoryAsync(ramParamRange, (DeviceMemory) this.ConnectedMeter.meterMemory, progress, cancelToken);
                }
                progress.BaseMessage = "Smart functions backup";
                if (backupParamRange.ByteSize > 0U)
                {
                  this.ReadRangesList.Add(new AddressRangeInfo("Smart function backup param", backupParamRange));
                  await this.myFunctions.checkedNfcCommands.ReadMemoryAsync(backupParamRange, (DeviceMemory) this.ConnectedMeter.meterMemory, progress, cancelToken);
                }
                this.ConnectedMeter.MySmartFunctionManager.SetFunctionIdentsFromMemory(this.ConnectedMeter.meterMemory);
                offsetDataRange = (AddressRange) null;
                flashFuncRange = (AddressRange) null;
                flashParamRange = (AddressRange) null;
                ramParamRange = (AddressRange) null;
                backupParamRange = (AddressRange) null;
              }
            }
            sf_FlashRange = (AddressRange) null;
            sf_RamRange = (AddressRange) null;
            sf_BackupRange = (AddressRange) null;
          }
          progress.LoggerMark();
          if ((readPartsSelection & ReadPartsSelection.ScenarioConfiguration) > ~(ReadPartsSelection.Dump | ReadPartsSelection.ProtocolOnlyMode) && this.ConnectedMeter.meterMemory.ScenarioConfigurationFlashRange != null)
          {
            progress.BaseMessage = "Scenario configuration ";
            this.ConnectedMeter.MyScenarioManager = new S4_ScenarioManager(this.myFunctions.myDeviceCommands, this.ConnectedMeter.meterMemory);
            ushort[] configsFromProtocol = await this.ConnectedMeter.MyScenarioManager.ReadAvailableModuleConfigurations(progress, cancelToken);
            if (configsFromProtocol.Length != 0)
            {
              await this.ConnectedMeter.MyScenarioManager.ReadConfigurationFromMap(progress, cancelToken, this.ReadRangesList);
              if (this.ConnectedMeter.MyScenarioManager.ConfigsFromMap.Count != configsFromProtocol.Length)
                throw new Exception("Number of read and map scenario configs are different!");
              for (int i = 0; i < this.ConnectedMeter.MyScenarioManager.ConfigsFromMap.Count; ++i)
              {
                if ((int) this.ConnectedMeter.MyScenarioManager.ConfigsFromMap[i].Key != (int) configsFromProtocol[i])
                  throw new Exception("Read and map scenario configs are different!");
              }
            }
            configsFromProtocol = (ushort[]) null;
          }
          progress.LoggerMark();
          progress.BaseMessage = "";
          progress.Report("Read finished. " + readStopWatch.ElapsedMilliseconds.ToString() + "ms");
          this.WorkMeter = readPartsSelection == ReadPartsSelection.FirmwareVersion ? (S4_Meter) null : this.ConnectedMeter.Clone();
          deviceIdentification = (DeviceIdentification) null;
          readStopWatch = (Stopwatch) null;
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Read device error.", ex);
      }
      finally
      {
        progress.BaseMessage = (string) null;
        readStopWatch.Stop();
      }
    }

    public async Task WriteDeviceAsync(ProgressHandler progress, CancellationToken cancelToken)
    {
      this.WriteRangesList = new List<AddressRangeInfo>();
      Stopwatch writeStopWatch = new Stopwatch();
      writeStopWatch.Start();
      try
      {
        if (this.checkedChangeMeter.meterMemory.IsParameterAvailable(S4_Params.LoRa_Minol_DeviceType))
          this.checkedChangeMeter.meterMemory.SetParameterValue<byte>(S4_Params.LoRa_Minol_DeviceType, (byte) 5);
        List<AddressRange> writeRanges = this.checkedChangeMeter.meterMemory.GetChangedDataRanges((DeviceMemory) this.ConnectedMeter.meterMemory);
        AddressRange MeterIdRange = this.checkedChangeMeter.meterMemory.GetAddressRange(S4_Params.Meter_ID.ToString());
        if (this.WorkMeter.deviceIdentification.IsProtected)
        {
          S4_ProtectionManager protectionManager = new S4_ProtectionManager(this.WorkMeter.meterMemory, this.myFunctions.checkedNfcCommands);
          protectionManager.CheckWriteEnabled(writeRanges);
          protectionManager = (S4_ProtectionManager) null;
        }
        foreach (AddressRange theRange in writeRanges)
        {
          this.WriteRangesList.Add(new AddressRangeInfo("Compiler info", theRange));
          this.WorkMeter.meterMemory.GarantMemoryAvailable(theRange);
          await this.myFunctions.checkedNfcCommands.WriteMemoryAsync(theRange, (DeviceMemory) this.WorkMeter.meterMemory, progress, cancelToken);
          if (theRange.StartAddress <= MeterIdRange.EndAddress && theRange.EndAddress >= MeterIdRange.StartAddress)
          {
            DeviceIdentification deviceIdentification = await this.myFunctions.checkedNfcCommands.ReadVersionAsync(progress, cancelToken);
          }
        }
        if (this.WorkMeter.MySmartFunctionManager != null)
        {
          S4_SmartFunctionManager SF_Manager = this.WorkMeter.MySmartFunctionManager;
          bool checkSmartFunctions = false;
          if (SF_Manager.FunctionsToDevice != null)
          {
            checkSmartFunctions = true;
            if (SF_Manager.NfcCmd == null)
              SF_Manager.NfcCmd = this.myFunctions.checkedNfcCommands;
            await SF_Manager.DeleteAllFunctionsAsync(progress, cancelToken);
            foreach (SmartFunctionIdentAndCode theFunction in SF_Manager.FunctionsToDevice)
            {
              SmartFunctionRuntimeResult functionRuntimeResult = await SF_Manager.LoadFunctionAsync(progress, cancelToken, theFunction.Code);
            }
            if (SF_Manager.ActiveFunctionsToDevice != null)
            {
              foreach (SmartFunctionIdentAndCode theFunction in SF_Manager.FunctionsToDevice)
              {
                if (!SF_Manager.ActiveFunctionsToDevice.Contains(theFunction.Name))
                {
                  int num = (int) await SF_Manager.NfcCmd.SetSmartFunctionActivationAsync(progress, cancelToken, theFunction.Name, false);
                }
              }
            }
          }
          else if (SF_Manager.ActiveFunctionsToDevice != null)
          {
            checkSmartFunctions = true;
            foreach (SmartFunctionIdentAndFlashParams theFunction in SF_Manager.FunctionsFromMemory)
            {
              if (theFunction.FunctionResult == SmartFunctionResult.NoError)
              {
                if (!SF_Manager.ActiveFunctionsToDevice.Contains(theFunction.Name))
                {
                  int num1 = (int) await SF_Manager.NfcCmd.SetSmartFunctionActivationAsync(progress, cancelToken, theFunction.Name, false);
                }
              }
              else if (theFunction.FunctionResult == SmartFunctionResult.DeactivatedByCommand && SF_Manager.ActiveFunctionsToDevice.Contains(theFunction.Name))
              {
                int num2 = (int) await SF_Manager.NfcCmd.SetSmartFunctionActivationAsync(progress, cancelToken, theFunction.Name, true);
              }
            }
          }
          if (SF_Manager.FunctionParametersForConfiguration != null)
          {
            foreach (KeyValuePair<string, List<SmartFunctionConfigParam>> keyValuePair in SF_Manager.FunctionParametersForConfiguration)
            {
              KeyValuePair<string, List<SmartFunctionConfigParam>> parameters = keyValuePair;
              string functionName = parameters.Key;
              List<SmartFunctionConfigParam> configParams = parameters.Value;
              List<SmartFunctionParameter> readedParameterList = await SF_Manager.GetFunctionParametersAsync(progress, cancelToken, functionName);
              List<SmartFunctionParameter> changtedParameterList = new List<SmartFunctionParameter>();
              foreach (SmartFunctionConfigParam functionConfigParam in configParams)
              {
                SmartFunctionConfigParam setParam = functionConfigParam;
                SmartFunctionParameter editParam = readedParameterList.Find((Predicate<SmartFunctionParameter>) (x => x.ParameterName == setParam.ParameterName));
                if (editParam.ParameterValue != setParam.ParameterValue)
                {
                  if (editParam.ParameterType == DataTypeCodes.ByteList && editParam.ParameterValue.Length != setParam.ParameterValue.Length)
                    throw new Exception("Parameter length changing not alowed.");
                  editParam.ParameterValue = editParam.ParameterValue;
                  changtedParameterList.Add(editParam);
                }
                editParam = (SmartFunctionParameter) null;
              }
              SmartFunctionRuntimeResult setResult = await SF_Manager.SetFunctionParametersAsync(progress, cancelToken, parameters.Key, changtedParameterList);
              if (!setResult.Blocked)
              {
                progress.Report("SmartFunction parameter changed for function: " + functionName);
                functionName = (string) null;
                configParams = (List<SmartFunctionConfigParam>) null;
                readedParameterList = (List<SmartFunctionParameter>) null;
                changtedParameterList = (List<SmartFunctionParameter>) null;
                setResult = (SmartFunctionRuntimeResult) null;
                parameters = new KeyValuePair<string, List<SmartFunctionConfigParam>>();
              }
              else
              {
                StringBuilder message = new StringBuilder();
                message.AppendLine("!!!! Write smart function parameter changes error !!!!");
                message.AppendLine("FunctionName: " + functionName);
                message.AppendLine("Error: " + setResult.Error);
                message.AppendLine("Error byte offset: 0x" + setResult.ErrorOffset.Value.ToString("x04"));
                throw new Exception(message.ToString());
              }
            }
          }
          if (checkSmartFunctions)
          {
            await SF_Manager.ReadLoadedFunctionsAsync(progress, cancelToken);
            if (SF_Manager.FunctionsToDevice != null)
            {
              if (SF_Manager.LoadedFunctionsInDevice.Count != SF_Manager.FunctionsToDevice.Count)
                throw new Exception("Smart functions count after load all functions not equal.");
              for (int i = 0; i < SF_Manager.FunctionsToDevice.Count; ++i)
              {
                if (SF_Manager.LoadedFunctionsInDevice[i].Name != SF_Manager.FunctionsToDevice[i].Name)
                  throw new Exception("Smart functions names out of protocol and data are not equal to load list.");
              }
            }
            foreach (SmartFunctionIdentResultAndCalls theFunction in SF_Manager.LoadedFunctionsInDevice)
            {
              SmartFunctionResult functionResult = theFunction.FunctionResult;
              switch (functionResult)
              {
                case SmartFunctionResult.NoError:
                  if (SF_Manager.ActiveFunctionsToDevice != null && !SF_Manager.ActiveFunctionsToDevice.Contains(theFunction.Name))
                    throw new Exception("Illegal activated smart function: " + theFunction.Name);
                  break;
                case SmartFunctionResult.DeactivatedByCommand:
                  if (SF_Manager.ActiveFunctionsToDevice != null && SF_Manager.ActiveFunctionsToDevice.Contains(theFunction.Name))
                    throw new Exception("Illegal deactivated smart function: " + theFunction.Name);
                  break;
                default:
                  throw new Exception("Smart functions load error. Name: " + theFunction.Name + "; FunctionResult: " + theFunction.FunctionResult.ToString() + "; Error: " + theFunction.Error);
              }
            }
          }
          SF_Manager = (S4_SmartFunctionManager) null;
        }
        uint? firmwareVersion;
        if (this.WorkMeter.ScenariosRequired)
        {
          firmwareVersion = this.WorkMeter.deviceIdentification.FirmwareVersion;
          if (new FirmwareVersion(firmwareVersion.Value) >= (object) "1.4.13 IUW")
          {
            S4_ScenarioManager scenarioManager = new S4_ScenarioManager(this.myFunctions.myDeviceCommands, this.WorkMeter.meterMemory);
            List<ushort> scenarios = scenarioManager.GetPreparedScenarios();
            byte[] configData = new byte[0];
            await scenarioManager.WriteModuleConfiguration(progress, cancelToken, ushort.MaxValue, configData);
            foreach (ushort scenario in scenarios)
            {
              byte[] scenarioConfig = scenarioManager.GetAdaptedConfiguration(scenario);
              await scenarioManager.WriteModuleConfiguration(progress, cancelToken, scenario, scenarioConfig);
              scenarioConfig = (byte[]) null;
            }
            scenarioManager = (S4_ScenarioManager) null;
            scenarios = (List<ushort>) null;
            configData = (byte[]) null;
          }
        }
        firmwareVersion = this.WorkMeter.deviceIdentification.FirmwareVersion;
        if (new FirmwareVersion(firmwareVersion.Value) >= (object) "1.4.13 IUW")
          await this.myFunctions.GenerateParameterChecksum(progress, cancelToken);
        if (this.WorkMeter.InitEventsOnWrite)
        {
          await S4_LoggerManager.AddEventAsync(DateTime.Now, LoggerEventTypes.NotDefined, (byte) 0, this.myFunctions.checkedNfcCommands, progress, cancelToken);
          this.WorkMeter.InitEventsOnWrite = false;
        }
        if (this.WorkMeter.SetTimeOnWrite || this.WorkMeter.SetPcTimeOnWrite || this.WorkMeter.SetTimeForTimeZoneFromPcTime)
        {
          Decimal timeZone = FirmwareDateTimeSupport.GetTimeZoneFromMemory((DeviceMemory) this.WorkMeter.meterMemory);
          if (this.WorkMeter.SetPcTimeOnWrite)
          {
            DateTime newDeviceTime = DateTime.Now;
            DateTimeOffset dateTimeOffset = new DateTimeOffset(newDeviceTime.Year, newDeviceTime.Month, newDeviceTime.Day, newDeviceTime.Hour, newDeviceTime.Minute, newDeviceTime.Second, new TimeSpan(0, (int) (timeZone * 60M), 0));
            DateTimeOffset dateTimeOffset1 = await this.myFunctions.checkedNfcCommands.SetSystemDateTime(dateTimeOffset, progress, cancelToken);
            dateTimeOffset = new DateTimeOffset();
          }
          else if (this.WorkMeter.SetTimeForTimeZoneFromPcTime)
          {
            DateTimeOffset dateTimeOffset2 = await this.myFunctions.SetTimeForTimeZoneFromPcTime(timeZone, progress, cancelToken);
          }
          else
          {
            DateTime newDeviceTime = FirmwareDateTimeSupport.GetDateTimeFromMemoryBCD((DeviceMemory) this.WorkMeter.meterMemory);
            DateTimeOffset dateTimeOffset = new DateTimeOffset(newDeviceTime.Year, newDeviceTime.Month, newDeviceTime.Day, newDeviceTime.Hour, newDeviceTime.Minute, newDeviceTime.Second, new TimeSpan(0, (int) (timeZone * 60M), 0));
            DateTimeOffset dateTimeOffset3 = await this.myFunctions.checkedNfcCommands.SetSystemDateTime(dateTimeOffset, progress, cancelToken);
            dateTimeOffset = new DateTimeOffset();
          }
          this.WorkMeter.SetTimeOnWrite = false;
          this.WorkMeter.SetPcTimeOnWrite = false;
          this.WorkMeter.SetTimeForTimeZoneFromPcTime = false;
        }
        if (this.WorkMeter.JoinRequestOnWrite)
        {
          await this.myFunctions.myDeviceCommands.CMDs_LoRa.SetOTAA_ABPAsync(OTAA_ABP.OTAA, progress, cancelToken);
          await this.myFunctions.myDeviceCommands.CMDs_LoRa.SendJoinRequestAsync(progress, cancelToken);
          this.WorkMeter.JoinRequestOnWrite = false;
        }
        this.ConnectedMeter = (S4_Meter) null;
        progress.Report("Write changes finished. " + writeStopWatch.ElapsedMilliseconds.ToString() + "ms");
        writeRanges = (List<AddressRange>) null;
        MeterIdRange = (AddressRange) null;
      }
      catch (Exception ex)
      {
        throw new Exception("Write device error.", ex);
      }
      finally
      {
        progress.BaseMessage = (string) null;
        writeStopWatch.Stop();
      }
      writeStopWatch = (Stopwatch) null;
    }

    internal async Task WriteClone(ProgressHandler progress, CancellationToken cancelToken)
    {
      Stopwatch writeStopWatch = new Stopwatch();
      writeStopWatch.Start();
      AddressRange activeRange = (AddressRange) null;
      try
      {
        if (this.BackupMeter == null)
          throw new Exception("BackupMeter not available");
        if (this.ConnectedMeter == null)
          throw new Exception("ConnectedMeter not available");
        uint? firmwareVersion1 = this.BackupMeter.deviceIdentification.FirmwareVersion;
        uint? firmwareVersion2 = this.ConnectedMeter.deviceIdentification.FirmwareVersion;
        if (!((int) firmwareVersion1.GetValueOrDefault() == (int) firmwareVersion2.GetValueOrDefault() & firmwareVersion1.HasValue == firmwareVersion2.HasValue))
          throw new Exception("Clone only allowed by identical firmware versions");
        if (this.ConnectedMeter.deviceIdentification.IsProtected)
          throw new Exception("The device is protected. Clone not possible.");
        DateTimeOffset mapTime = FirmwareDateTimeSupport.ToDateTimeOffsetBCD(this.myFunctions.myMeters.BackupMeter.meterMemory.GetData(S4_Params.sysDateTime));
        List<AddressRange> writeRanges = this.BackupMeter.meterMemory.GetExistingDataRanges();
        AddressRange configRamRange = this.BackupMeter.meterMemory.MapDef.GetSectionRanges("CONFIG_RAM");
        AddressRange MeterID_Range = this.BackupMeter.meterMemory.UsedParametersByName[S4_Params.Meter_ID.ToString()].AddressRange;
        await this.myFunctions.checkedNfcCommands.WriteMemoryAsync(MeterID_Range, (DeviceMemory) this.BackupMeter.meterMemory, progress, cancelToken);
        DeviceIdentification deviceIdentification = await this.myFunctions.checkedNfcCommands.ReadVersionAsync(progress, cancelToken);
        DateTimeOffset dateTimeOffset = await this.myFunctions.checkedNfcCommands.SetSystemDateTime(mapTime, progress, cancelToken);
        foreach (AddressRange addressRange in writeRanges)
        {
          AddressRange theRange = addressRange;
          activeRange = theRange;
          if ((theRange.StartAddress < this.BackupMeter.meterMemory.ArmIdRange.StartAddress || theRange.EndAddress > this.BackupMeter.meterMemory.ArmIdRange.EndAddress) && (theRange.StartAddress < this.BackupMeter.meterMemory.MeterKeyRange.StartAddress || theRange.StartAddress > this.BackupMeter.meterMemory.MeterKeyRange.EndAddress) && theRange.StartAddress <= configRamRange.EndAddress)
          {
            if (theRange.EndAddress > configRamRange.EndAddress)
              theRange.EndAddress = configRamRange.EndAddress;
            await this.myFunctions.checkedNfcCommands.WriteMemoryAsync(theRange, (DeviceMemory) this.BackupMeter.meterMemory, progress, cancelToken);
            theRange = (AddressRange) null;
          }
        }
        if (this.BackupMeter.deviceIdentification.IsProtected)
        {
          activeRange = (AddressRange) null;
          try
          {
            await this.myFunctions.ProtectionSetAgainAsync(progress, cancelToken);
          }
          catch (Exception ex)
          {
            throw new Exception("Reactivate write protection error. That needs a active MeterKey inside the device before the clone function!", ex);
          }
        }
        await this.myFunctions.checkedNfcCommands.ResetDeviceAsync(progress, cancelToken, true);
        progress.Report("Write clone finished. " + writeStopWatch.ElapsedMilliseconds.ToString() + "ms");
        mapTime = new DateTimeOffset();
        writeRanges = (List<AddressRange>) null;
        configRamRange = (AddressRange) null;
        MeterID_Range = (AddressRange) null;
      }
      catch (Exception ex)
      {
        if (activeRange != null)
          throw new Exception("Write device clone error at address range: " + activeRange.ToString(), ex);
        throw new Exception("Write device clone error.", ex);
      }
      finally
      {
        progress.BaseMessage = (string) null;
        writeStopWatch.Stop();
      }
      writeStopWatch = (Stopwatch) null;
      activeRange = (AddressRange) null;
    }

    public void OpenType(int meterInfoID)
    {
      if (this.S4_AllMeters_Logger.IsTraceEnabled)
        this.S4_AllMeters_Logger.Trace("OpenType. MeterInfoID: " + meterInfoID.ToString());
      S4_Meter s4Meter = (S4_Meter) null;
      lock (S4_AllMeters.TypeCache)
      {
        int index = S4_AllMeters.TypeCache.IndexOfKey(meterInfoID);
        if (index >= 0)
        {
          S4_AllMeters.TypeCacheData typeCacheData = S4_AllMeters.TypeCache.Values[index];
          if (typeCacheData.IsCacheTimedOver)
            S4_AllMeters.TypeCache.RemoveAt(index);
          else
            s4Meter = typeCacheData.MeterObject;
        }
      }
      if (s4Meter != null)
      {
        this.TypeMeter = s4Meter;
      }
      else
      {
        BaseType baseType = BaseType.GetBaseType(meterInfoID);
        if (baseType == null || baseType.Data == null)
          throw new Exception("Type not available! MeterInfoID: " + meterInfoID.ToString());
        if (!string.IsNullOrEmpty(baseType.Data.TypeCreationString))
        {
          if (this.S4_AllMeters_Logger.IsTraceEnabled)
            this.S4_AllMeters_Logger.Trace("OpenType. TypeCreationString = " + baseType.Data.TypeCreationString);
          this.OpenType(baseType.Data.TypeCreationString);
          this.TypeMeter.DatabaseTypeCreationString = baseType.Data.TypeCreationString;
        }
        else
        {
          this.TypeMeter = (S4_Meter) this.CreateMeter(baseType.Data.EEPdata);
          this.TypeMeter.BaseType = baseType;
        }
        this.TypeMeter.MeterInfoIdFromDb = new int?(meterInfoID);
        this.TypeMeter.meterMemory.SetParameterValue<int>(S4_Params.MeterInfo_ID, meterInfoID);
        this.TypeMeter.meterMemory.SetParameterValue<int>(S4_Params.MeterType_ID, baseType.MeterInfo.MeterTypeID);
        this.TypeMeter.meterMemory.SetParameterValue<int>(S4_Params.HardwareType_ID, baseType.MeterInfo.HardwareTypeID);
        int result;
        if (baseType.MeterInfo.PPSArtikelNr != null && int.TryParse(baseType.MeterInfo.PPSArtikelNr, out result))
          this.TypeMeter.meterMemory.SetParameterValue<int>(S4_Params.SAP_Number, result);
        this.TypeMeter.meterMemory.SetParameterValue<int>(S4_Params.BaseType_ID, this.TypeMeter.BaseType.MeterInfo.MeterInfoID);
        lock (S4_AllMeters.TypeCache)
        {
          if (S4_AllMeters.TypeCache.IndexOfKey(meterInfoID) < 0)
            S4_AllMeters.TypeCache.Add(meterInfoID, new S4_AllMeters.TypeCacheData(this.TypeMeter));
        }
      }
      if (this.WorkMeter != null)
        return;
      this.WorkMeter = this.TypeMeter.Clone();
    }

    public void OpenCompareType(int meterInfoID)
    {
      BaseType baseType = BaseType.GetBaseType(meterInfoID);
      if (baseType == null || baseType.Data == null)
        throw new Exception("Type not available! MeterInfoID: " + meterInfoID.ToString());
      this.TypeMeter = (S4_Meter) this.CreateMeter(baseType.Data.EEPdata);
      this.TypeMeter.BaseType = baseType;
      this.TypeMeter.MeterInfoIdFromDb = new int?(meterInfoID);
      S4_AllMeters.TypeCacheClear();
      if (this.WorkMeter != null)
        return;
      this.WorkMeter = this.TypeMeter.Clone();
    }

    public void OpenType(string typeCreationString)
    {
      List<TypeOverwriteData> typeOverwriteDataList = OverwriteSupport.PrepareOverwriteData(typeCreationString);
      BaseType baseType1 = typeOverwriteDataList.Count >= 1 ? BaseType.GetBaseType(typeOverwriteDataList[0].MeterInfoID) : throw new Exception("Illegal TypeCreationString ");
      if (baseType1 == null || baseType1.Data == null)
        throw new Exception("Type not available! MeterInfoID: " + typeOverwriteDataList[0].MeterInfoID.ToString());
      if (!string.IsNullOrEmpty(baseType1.Data.TypeCreationString))
        throw new Exception("Illegal TypeCreationString found! MeterInfoID: " + typeOverwriteDataList[0].MeterInfoID.ToString());
      this.TypeMeter = (S4_Meter) this.CreateMeter(baseType1.Data.EEPdata, new int?(baseType1.MeterInfo.HardwareTypeID));
      this.TypeMeter.UsedTypeCreationString = typeCreationString;
      string typeCreationString1 = S4_FirmwareVersionManager.GetRequiredTypeCreationString(this.TypeMeter.deviceIdentification.FirmwareVersion.Value, typeCreationString);
      if (typeCreationString1 != typeCreationString)
      {
        typeOverwriteDataList = OverwriteSupport.PrepareOverwriteData(typeCreationString1);
        baseType1 = typeOverwriteDataList.Count >= 1 ? BaseType.GetBaseType(typeOverwriteDataList[0].MeterInfoID) : throw new Exception("Illegal TypeCreationString ");
        if (baseType1 == null || baseType1.Data == null)
          throw new Exception("Type not available! MeterInfoID: " + typeOverwriteDataList[0].MeterInfoID.ToString());
        if (!string.IsNullOrEmpty(baseType1.Data.TypeCreationString))
          throw new Exception("Illegal TypeCreationString found! MeterInfoID: " + typeOverwriteDataList[0].MeterInfoID.ToString());
        this.TypeMeter = (S4_Meter) this.CreateMeter(baseType1.Data.EEPdata, new int?(baseType1.MeterInfo.HardwareTypeID));
        this.TypeMeter.UsedTypeCreationString = typeCreationString1;
      }
      this.TypeMeter.meterMemory.SetData(S4_Params.sysDateTime, FirmwareDateTimeSupport.ToFirmwareDateTimeBCD(DateTime.Now));
      this.TypeMeter.BaseType = baseType1;
      this.TypeMeter.MeterInfoIdFromDb = new int?(typeOverwriteDataList[0].MeterInfoID);
      if (typeOverwriteDataList.Count > 1)
      {
        S4_Meter workMeter = this.WorkMeter;
        this.WorkMeter = this.TypeMeter;
        for (int index = 1; index < typeOverwriteDataList.Count; ++index)
        {
          BaseType baseType2 = BaseType.GetBaseType(typeOverwriteDataList[index].MeterInfoID);
          if (baseType2 == null || baseType2.Data == null)
            throw new Exception("Overwrite type not available! MeterInfoID: " + typeOverwriteDataList[index].MeterInfoID.ToString());
          this.TypeMeter = (S4_Meter) this.CreateMeter(baseType2.Data.EEPdata);
          this.OverwriteSrcToDest(HandlerMeterObjects.TypeMeter, HandlerMeterObjects.WorkMeter, typeOverwriteDataList[index].OverwriteGroups, true);
        }
        this.TypeMeter = this.WorkMeter;
        this.WorkMeter = workMeter;
      }
      if (this.WorkMeter != null)
        return;
      this.WorkMeter = this.TypeMeter.Clone();
    }

    internal int SaveType(
      OpenTransaction openTransaction,
      string sapNumber,
      string description,
      int? meterhardwareID = null)
    {
      if (this.ConnectedMeter != null)
        throw new Exception("Save type after read meter not supported");
      if (this.TypeMeter == null || this.TypeMeter.BaseType == null || this.TypeMeter.BaseType.Data == null || this.TypeMeter.BaseType.MeterInfo == null || this.TypeMeter.UsedTypeCreationString == null)
        throw new Exception("Save type is only allowed after load a type by TypeCreationString");
      byte[] compressedData = this.WorkMeter.meterMemory.GetCompressedData();
      int meterHardwareID = !meterhardwareID.HasValue ? this.TypeMeter.BaseType.MeterInfo.MeterHardwareID : meterhardwareID.Value;
      return BaseType.CreateType(openTransaction, "IUW", sapNumber, this.TypeMeter.BaseType.MeterInfo.HardwareTypeID, meterHardwareID, description, compressedData, this.TypeMeter.UsedTypeCreationString, false).Value;
    }

    public void GuarnteeTypeReadyForOverwrite()
    {
      if (this.TypeMeter.DatabaseTypeCreationString == null)
        return;
      string typeCreationString = S4_FirmwareVersionManager.GetRequiredTypeCreationString(this.WorkMeter.deviceIdentification.FirmwareVersion.Value, this.TypeMeter.DatabaseTypeCreationString);
      if (!(this.TypeMeter.UsedTypeCreationString != typeCreationString))
        return;
      if (this.S4_AllMeters_Logger.IsTraceEnabled)
        this.S4_AllMeters_Logger.Trace("Change type for firmware version! TypeCreationString = " + typeCreationString);
      S4_Meter typeMeter = this.TypeMeter;
      this.OpenType(typeCreationString);
      this.TypeMeter.DatabaseTypeCreationString = typeMeter.DatabaseTypeCreationString;
      this.TypeMeter.MeterInfoIdFromDb = new int?(typeMeter.MeterInfoIdFromDb.Value);
      this.TypeMeter.meterMemory.SetParameterValue<int>(S4_Params.MeterInfo_ID, typeMeter.MeterInfoIdFromDb.Value);
      this.TypeMeter.meterMemory.SetParameterValue<int>(S4_Params.MeterType_ID, typeMeter.BaseType.MeterInfo.MeterTypeID);
      this.TypeMeter.meterMemory.SetParameterValue<int>(S4_Params.HardwareType_ID, typeMeter.BaseType.MeterInfo.HardwareTypeID);
      int result;
      if (typeMeter.BaseType.MeterInfo.PPSArtikelNr != null && int.TryParse(typeMeter.BaseType.MeterInfo.PPSArtikelNr, out result))
        this.TypeMeter.meterMemory.SetParameterValue<int>(S4_Params.SAP_Number, result);
      this.TypeMeter.meterMemory.SetParameterValue<int>(S4_Params.BaseType_ID, this.TypeMeter.BaseType.MeterInfo.MeterInfoID);
      lock (S4_AllMeters.TypeCache)
      {
        int index = S4_AllMeters.TypeCache.IndexOfKey(this.TypeMeter.MeterInfoIdFromDb.Value);
        if (index >= 0)
          S4_AllMeters.TypeCache.RemoveAt(index);
        S4_AllMeters.TypeCache.Add(this.TypeMeter.MeterInfoIdFromDb.Value, new S4_AllMeters.TypeCacheData(this.TypeMeter));
      }
    }

    internal SortedList<HandlerMeterObjects, DeviceMemory> GetAllMeterMemories()
    {
      SortedList<HandlerMeterObjects, DeviceMemory> allMeterMemories = new SortedList<HandlerMeterObjects, DeviceMemory>();
      if (this.WorkMeter != null)
        allMeterMemories.Add(HandlerMeterObjects.WorkMeter, (DeviceMemory) this.WorkMeter.meterMemory);
      if (this.ConnectedMeter != null)
        allMeterMemories.Add(HandlerMeterObjects.ConnectedMeter, (DeviceMemory) this.ConnectedMeter.meterMemory);
      if (this.TypeMeter != null)
        allMeterMemories.Add(HandlerMeterObjects.TypeMeter, (DeviceMemory) this.TypeMeter.meterMemory);
      if (this.BackupMeter != null)
        allMeterMemories.Add(HandlerMeterObjects.BackupMeter, (DeviceMemory) this.BackupMeter.meterMemory);
      if (this.SavedMeter != null)
        allMeterMemories.Add(HandlerMeterObjects.SavedMeter, (DeviceMemory) this.SavedMeter.meterMemory);
      return allMeterMemories;
    }

    internal S4_Meter GetMeterObject(HandlerMeterObjects meterObject)
    {
      S4_Meter s4Meter;
      switch (meterObject)
      {
        case HandlerMeterObjects.WorkMeter:
          s4Meter = this.WorkMeter;
          break;
        case HandlerMeterObjects.ConnectedMeter:
          s4Meter = this.ConnectedMeter;
          break;
        case HandlerMeterObjects.TypeMeter:
          s4Meter = this.TypeMeter;
          break;
        case HandlerMeterObjects.BackupMeter:
          s4Meter = this.BackupMeter;
          break;
        case HandlerMeterObjects.SavedMeter:
          s4Meter = this.SavedMeter;
          break;
        default:
          throw new Exception("Unknown meter object");
      }
      return s4Meter != null ? s4Meter : throw new Exception(meterObject.ToString() + " meter object == null");
    }

    public void OverwriteSrcToDest(
      HandlerMeterObjects sourceObject,
      HandlerMeterObjects destinationObject,
      CommonOverwriteGroups[] overwriteGroups,
      bool OpenTypeActive = false)
    {
      int index1 = Array.IndexOf<CommonOverwriteGroups>(overwriteGroups, CommonOverwriteGroups.ConfigurationParameters);
      if (index1 >= 0 && index1 != overwriteGroups.Length - 1)
      {
        overwriteGroups[index1] = overwriteGroups[overwriteGroups.Length - 1];
        overwriteGroups[overwriteGroups.Length - 1] = CommonOverwriteGroups.ConfigurationParameters;
      }
      if (((IEnumerable<CommonOverwriteGroups>) overwriteGroups).Contains<CommonOverwriteGroups>(CommonOverwriteGroups.UltrasonicHydraulicTestSetup))
      {
        if (((IEnumerable<CommonOverwriteGroups>) overwriteGroups).Contains<CommonOverwriteGroups>(CommonOverwriteGroups.UltrasonicCalibration))
          throw new ArgumentException("Overwrite group UltrasonicHydraulicTestSetup can not be combind with UltrasonicCalibration");
        if (((IEnumerable<CommonOverwriteGroups>) overwriteGroups).Contains<CommonOverwriteGroups>(CommonOverwriteGroups.UltrasonicLimits))
          throw new ArgumentException("Overwrite group UltrasonicHydraulicTestSetup can not be combind with UltrasonicLimits");
      }
      if (((IEnumerable<CommonOverwriteGroups>) overwriteGroups).Contains<CommonOverwriteGroups>(CommonOverwriteGroups.UltrasonicLimits) && ((IEnumerable<CommonOverwriteGroups>) overwriteGroups).Contains<CommonOverwriteGroups>(CommonOverwriteGroups.UltrasonicCalibration))
        throw new ArgumentException("Overwrite group UltrasonicLimits can not be combind with UltrasonicCalibration");
      string empty = string.Empty;
      SortedList<HandlerMeterObjects, DeviceMemory> allMeterMemories = this.GetAllMeterMemories();
      int index2 = allMeterMemories.IndexOfKey(sourceObject);
      if (index2 < 0)
        throw new Exception("Overwrite source object not found:" + sourceObject.ToString());
      S4_DeviceMemory s4DeviceMemory1 = (S4_DeviceMemory) allMeterMemories.Values[index2];
      int index3 = destinationObject == HandlerMeterObjects.ConnectedDevice ? allMeterMemories.IndexOfKey(HandlerMeterObjects.ConnectedMeter) : allMeterMemories.IndexOfKey(destinationObject);
      if (index3 < 0)
        throw new Exception("Overwrite destination object not found:" + destinationObject.ToString());
      S4_DeviceMemory s4DeviceMemory2 = (S4_DeviceMemory) allMeterMemories.Values[index3];
      try
      {
        if ((int) s4DeviceMemory2.FirmwareVersion != (int) s4DeviceMemory1.FirmwareVersion)
        {
          bool flag = false;
          if (s4DeviceMemory2.FirmwareVersion == 16920582U && s4DeviceMemory1.FirmwareVersion == 17059846U)
          {
            flag = true;
            foreach (CommonOverwriteGroups overwriteGroup in overwriteGroups)
            {
              int num;
              switch (overwriteGroup)
              {
                case CommonOverwriteGroups.UltrasonicCalibration:
                case CommonOverwriteGroups.UltrasonicHydraulicTestSetup:
                case CommonOverwriteGroups.UltrasonicLimits:
                case CommonOverwriteGroups.UltrasonicTempSensorCurve:
                  num = 1;
                  break;
                default:
                  num = overwriteGroup == CommonOverwriteGroups.RTC_Calibration ? 1 : 0;
                  break;
              }
              if (num == 0)
              {
                flag = false;
                break;
              }
            }
          }
          if (!flag)
          {
            CompatibilityInfo compatibilityInfos = HardwareTypeSupport.GetCompatibilityInfos(allMeterMemories.Values[index3].FirmwareVersion, allMeterMemories.Values[index2].FirmwareVersion);
            OverwriteSupport.CheckAllGroupsCompatible(overwriteGroups, compatibilityInfos);
          }
        }
        if (destinationObject == HandlerMeterObjects.ConnectedDevice)
        {
          this.OverwriteToConnectedDevice(allMeterMemories.Values[index2], allMeterMemories.Values[index3], overwriteGroups);
        }
        else
        {
          foreach (CommonOverwriteGroups overwriteGroup in overwriteGroups)
          {
            empty = overwriteGroup.ToString();
            if (!S4_DeviceMemory.ParameterGroups.ContainsKey(overwriteGroup))
              throw new Exception("OverwriteGroup not found in S4_Handler - " + empty);
            allMeterMemories.Values[index3].OverwriteUsedParameters(allMeterMemories.Values[index2], S4_DeviceMemory.ParameterGroups[overwriteGroup]);
            if (overwriteGroup == CommonOverwriteGroups.UltrasonicHydraulicTestSetup)
            {
              float theValue1 = s4DeviceMemory1.GetParameterValue<float>(S4_Params.maxPositivFlow) * 2f;
              s4DeviceMemory2.SetParameterValue<float>(S4_Params.maxPositivFlow, theValue1);
              float theValue2 = s4DeviceMemory1.GetParameterValue<float>(S4_Params.maxNegativFlow) * 2f;
              s4DeviceMemory2.SetParameterValue<float>(S4_Params.maxNegativFlow, theValue2);
              float theValue3 = s4DeviceMemory1.GetParameterValue<float>(S4_Params.minPositivFlow) / 10f;
              s4DeviceMemory2.SetParameterValue<float>(S4_Params.minPositivFlow, theValue3);
              float theValue4 = s4DeviceMemory1.GetParameterValue<float>(S4_Params.minNegativFlow) / 10f;
              s4DeviceMemory2.SetParameterValue<float>(S4_Params.minNegativFlow, theValue4);
            }
            if (overwriteGroup == CommonOverwriteGroups.UltrasonicHydraulicTestSetup || overwriteGroup == CommonOverwriteGroups.UltrasonicCalibration)
            {
              ushort num1 = 10;
              if (new FirmwareVersion(s4DeviceMemory2.FirmwareVersion) > (object) "1.7.9 IUW" | OpenTypeActive)
              {
                num1 |= (ushort) 80;
              }
              else
              {
                uint address1 = s4DeviceMemory2.GetParameterAddress(S4_Params.tdcConfigOneChannelUnit2) + 20U;
                ushort num2 = (ushort) ((uint) s4DeviceMemory2.GetValue<ushort>(address1) & 4294967167U);
                s4DeviceMemory2.SetValue<ushort>(num2, address1);
                uint address2 = s4DeviceMemory2.GetParameterAddress(S4_Params.tdcConfigTwoChannelUnit2) + 20U;
                ushort num3 = (ushort) ((uint) s4DeviceMemory2.GetValue<ushort>(address2) & 4294967167U);
                s4DeviceMemory2.SetValue<ushort>(num3, address2);
                uint address3 = s4DeviceMemory2.GetParameterAddress(S4_Params.tdcConfigTwoChannelUnit1) + 20U;
                ushort num4 = (ushort) ((uint) s4DeviceMemory2.GetValue<ushort>(address3) & 4294967167U);
                s4DeviceMemory2.SetValue<ushort>(num4, address3);
              }
              ushort parameterValue = s4DeviceMemory1.GetParameterValue<ushort>(S4_Params.ConfigFlagRegister);
              ushort theValue = (ushort) ((int) s4DeviceMemory2.GetParameterValue<ushort>(S4_Params.ConfigFlagRegister) & (int) ~num1 | (int) parameterValue & (int) num1);
              s4DeviceMemory2.SetParameterValue<ushort>(S4_Params.ConfigFlagRegister, theValue);
            }
            if (overwriteGroup == CommonOverwriteGroups.UltrasonicCalibration && s4DeviceMemory2.IsParameterAvailable(S4_Params.minimumFlowrateQ1))
            {
              float parameterValue = s4DeviceMemory2.GetParameterValue<float>(S4_Params.minPositivFlow);
              float num = (float) (((double) s4DeviceMemory2.GetParameterValue<float>(S4_Params.maxPositivFlow) - (double) parameterValue) / 100.0);
              s4DeviceMemory2.SetParameterValue<float>(S4_Params.minimumFlowrateQ1, parameterValue + num * 5f);
              s4DeviceMemory2.SetParameterValue<float>(S4_Params.transitionalFlowrateQ2, parameterValue + num * 10f);
              s4DeviceMemory2.SetParameterValue<float>(S4_Params.permanentFlowrateQ3, parameterValue + num * 70f);
              s4DeviceMemory2.SetParameterValue<float>(S4_Params.overloadFlowrateQ4, parameterValue + num * 90f);
            }
            if (overwriteGroup == CommonOverwriteGroups.ConfigurationParameters)
            {
              S4_Meter meterObject1 = this.GetMeterObject(sourceObject);
              S4_Meter meterObject2 = this.GetMeterObject(destinationObject);
              SortedList<OverrideID, ConfigurationParameter> configurationParameters = meterObject1.GetConfigurationParameters("Load source for overwrite.", true);
              SortedList<OverrideID, ConfigurationParameter> parameterList = new SortedList<OverrideID, ConfigurationParameter>();
              foreach (KeyValuePair<OverrideID, ConfigurationParameter> keyValuePair in configurationParameters)
              {
                if (keyValuePair.Key == OverrideID.DisplayMenu)
                  parameterList.Add(keyValuePair.Key, new ConfigurationParameter(OverrideID.DisplayMenu, (object) "Standard"));
                else if (((IEnumerable<OverrideID>) S4_AllMeters.ConfigurationParameterOverwriteWhiteList).Contains<OverrideID>(keyValuePair.Key))
                  parameterList.Add(keyValuePair.Key, keyValuePair.Value);
              }
              meterObject2.SetConfigurationParameters(parameterList);
            }
          }
        }
      }
      catch (Exception ex)
      {
        string str = "";
        foreach (CommonOverwriteGroups overwriteGroup in overwriteGroups)
          str = str.Length != 0 ? str + ", " + overwriteGroup.ToString() : str + overwriteGroup.ToString();
        throw new Exception("Overwrite error.\nSorce: " + new FirmwareVersion(allMeterMemories.Values[index2].FirmwareVersion).ToString() + ";\nDestination: " + new FirmwareVersion(allMeterMemories.Values[index3].FirmwareVersion).ToString() + ";\nGroups: " + str + ";\n\nError in Group: " + empty, ex);
      }
    }

    internal void OverwriteToConnectedDevice(
      DeviceMemory srcMemory,
      DeviceMemory toMemoryStructure,
      CommonOverwriteGroups[] overwriteGroups)
    {
      this.DirectWriteData = new DeviceMemory(toMemoryStructure.FirmwareVersion);
      foreach (CommonOverwriteGroups overwriteGroup in overwriteGroups)
        this.OverwriteDeviceParameters(srcMemory, toMemoryStructure, S4_DeviceMemory.ParameterGroups[overwriteGroup]);
    }

    public void OverwriteDeviceParameters(
      DeviceMemory fromMemory,
      DeviceMemory toMemoryStructure,
      string[] parameterNameList)
    {
      string key = "";
      try
      {
        for (int index = 0; index < parameterNameList.Length; ++index)
        {
          key = parameterNameList[index];
          if (fromMemory.UsedParametersByName.ContainsKey(key) && toMemoryStructure.UsedParametersByName.ContainsKey(key))
          {
            AddressRange addressRange = fromMemory.UsedParametersByName[key].AddressRange;
            byte[] data = fromMemory.GetData(addressRange);
            if (data == null)
              throw new Exception("Source data not found for: " + key);
            this.DirectWriteData.GarantMemoryAvailable(addressRange);
            this.DirectWriteData.SetData(addressRange.StartAddress, data);
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Overwrite of parameter faild: " + key, ex);
      }
    }

    internal void SaveMeterObject(HandlerMeterObjects meterObject)
    {
      switch (meterObject)
      {
        case HandlerMeterObjects.WorkMeter:
          this.SavedMeter = this.WorkMeter;
          break;
        case HandlerMeterObjects.ConnectedMeter:
          this.SavedMeter = this.ConnectedMeter;
          break;
        case HandlerMeterObjects.TypeMeter:
          this.SavedMeter = this.TypeMeter;
          break;
        case HandlerMeterObjects.BackupMeter:
          this.SavedMeter = this.BackupMeter;
          break;
        default:
          throw new Exception("Illegal meter object for SaveMeterObject");
      }
    }

    public void Dispose() => throw new NotImplementedException();

    private class TypeCacheData
    {
      private DateTime CacheTime;
      internal S4_Meter MeterObject;

      internal TypeCacheData(S4_Meter meterObject)
      {
        this.CacheTime = DateTime.Now;
        this.MeterObject = meterObject;
      }

      internal bool IsCacheTimedOver => this.CacheTime < DateTime.Now.AddHours(-2.0);
    }
  }
}
