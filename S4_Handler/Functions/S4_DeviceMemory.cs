// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.S4_DeviceMemory
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using HandlerLib.MapManagement;
using SmartFunctionCompiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler.Functions
{
  internal class S4_DeviceMemory : DeviceMemory
  {
    private static string[] NotUsedMapParameters = new string[20]
    {
      "smart_func_flash",
      "ndc_config_flash",
      "selectedParam",
      "dispCtx",
      "log_nextTime",
      "log_index",
      "flash_Ctx",
      "ndc_copy_mem",
      "ndc_flashCtx",
      "value_unit",
      "nfc_Ctx",
      "sfunc",
      "sfunc_ram_hdr",
      "base_prm_hdr",
      "act_btime",
      "act_time",
      "sfunc_start_mem",
      "next_event_time",
      "psfunc_offset",
      "sfunc_flashCtx"
    };
    private SortedList<string, AddressRange> AllSections;
    internal AddressRange ArmIdRange;
    internal AddressRange RamRange;
    internal AddressRange FlashRange;
    internal AddressRange MeterKeyRange;
    internal AddressRange EEPromRange;
    internal AddressRange RAM_BackupRange;
    internal AddressRange FlashBackupRange;
    internal AddressRange CalibrationRange;
    internal AddressRange SelectedReadoutParametersRange;
    internal AddressRange SmartFunctionFlashRange;
    internal AddressRange ScenarioConfigurationFlashRange;
    internal List<AddressRange> UsedRamRanges;
    internal static S4_Params[] CompilerInfo = new S4_Params[4]
    {
      S4_Params.SvnRevision,
      S4_Params.BuildTime,
      S4_Params.IAR_CompilerVer,
      S4_Params.IAR_BuildTime
    };
    internal static S4_Params[] IdentData = new S4_Params[17]
    {
      S4_Params.SerialNumber,
      S4_Params.PrintedSerialNumber,
      S4_Params.Meter_ID,
      S4_Params.MeterInfo_ID,
      S4_Params.HardwareType_ID,
      S4_Params.BaseType_ID,
      S4_Params.MeterType_ID,
      S4_Params.SAP_Number,
      S4_Params.SAP_OrderNumber,
      S4_Params.MBus_ManuFacturer,
      S4_Params.MBus_Medium,
      S4_Params.Obis_Medium,
      S4_Params.Generation,
      S4_Params.NFC_Version,
      S4_Params.LoRa_DevEUI,
      S4_Params.LoRa_AppEUI,
      S4_Params.LoRa_AppKey
    };
    internal static S4_Params[] TypeIdentification = new S4_Params[5]
    {
      S4_Params.MeterInfo_ID,
      S4_Params.HardwareType_ID,
      S4_Params.BaseType_ID,
      S4_Params.MeterType_ID,
      S4_Params.SAP_Number
    };
    internal static S4_Params[] UltrasonicCalibration = new S4_Params[34]
    {
      S4_Params.tdcConfigOneChannelUnit2,
      S4_Params.tdcConfigTwoChannelUnit2,
      S4_Params.tdcConfigTwoChannelUnit1,
      S4_Params.nRefTempS,
      S4_Params.tdcCountsValues,
      S4_Params.tdcTempValues,
      S4_Params.tdcSonicVelValues,
      S4_Params.tdcTempErrorIndex,
      S4_Params.geometryFactor,
      S4_Params.TDC_MapTemp,
      S4_Params.TDC_MapFlow,
      S4_Params.TDC_MapCoef,
      S4_Params.maxPositivFlow,
      S4_Params.minPositivFlow,
      S4_Params.minNegativFlow,
      S4_Params.maxNegativFlow,
      S4_Params.flowRangeTabPosFlow,
      S4_Params.flowRangeTabNegFlow,
      S4_Params.pwOffsetMin,
      S4_Params.pwOffsetMax,
      S4_Params.pwLevelHigh0,
      S4_Params.pwLevelLow0,
      S4_Params.pwLevelHigh1,
      S4_Params.pwLevelLow1,
      S4_Params.pwLevelInc0,
      S4_Params.pwLevelInc1,
      S4_Params.pwLevelMin,
      S4_Params.pwLevelMax,
      S4_Params.pwOffsetAdjust,
      S4_Params.pwNumberValuesAveraging,
      S4_Params.pwConfigControl,
      S4_Params.pwDiffMax,
      S4_Params.pwLevelSwitch,
      S4_Params.maxAbsTofValue
    };
    internal static S4_Params[] UltrasonicTempSensorCurve = new S4_Params[5]
    {
      S4_Params.nRefTempS,
      S4_Params.tdcCountsValues,
      S4_Params.tdcTempValues,
      S4_Params.tdcSonicVelValues,
      S4_Params.tdcTempErrorIndex
    };
    internal static S4_Params[] UltrasonicLimits = new S4_Params[4]
    {
      S4_Params.maxPositivFlow,
      S4_Params.minPositivFlow,
      S4_Params.minNegativFlow,
      S4_Params.maxNegativFlow
    };
    internal static S4_Params[] ZeroFlowCalibration = new S4_Params[4]
    {
      S4_Params.zeroOffsetUnit1,
      S4_Params.zeroOffsetUnit2,
      S4_Params.zeroOffsetMeasUnit1,
      S4_Params.zeroOffsetMeasUnit2
    };
    internal static S4_Params[] MenuDefinition = new S4_Params[1]
    {
      S4_Params.Disp_Menu
    };
    internal static S4_Params[] DeviceProtection = new S4_Params[2]
    {
      S4_Params.Meter_IsProtected,
      S4_Params.Meter_ProtectionDate
    };
    internal static S4_Params[] RTC_Calibration = new S4_Params[1]
    {
      S4_Params.rtcCalibrationValue
    };
    internal static S4_Params[] CarrierFrequencyCalibration = new S4_Params[1]
    {
      S4_Params.cfg_radio_frequency_inc_dec
    };
    internal static S4_Params[] ConfigurationParameters = new S4_Params[16]
    {
      S4_Params.minimumFlowrateQ1,
      S4_Params.transitionalFlowrateQ2,
      S4_Params.permanentFlowrateQ3,
      S4_Params.overloadFlowrateQ4,
      S4_Params.SubPartNumber,
      S4_Params.ProductionOrderNumber,
      S4_Params.ApprovalRevision,
      S4_Params.WMBus_AesKey,
      S4_Params.FD_SerialNumber,
      S4_Params.FD_WMBus_AesKey,
      S4_Params.FD_LoRa_AppKey,
      S4_Params.FD_LoRa_DevEUI,
      S4_Params.FD_Generation,
      S4_Params.FD_MBus_ManuFacturer,
      S4_Params.FD_MBus_Medium,
      S4_Params.FD_Obis_Medium
    };
    internal static S4_Params[] EmptyParameterGroup = new S4_Params[0];
    public static SortedList<CommonOverwriteGroups, string[]> ParameterGroups;
    private static string[] KnownDifferences = new string[3]
    {
      "BuildTime",
      "sysDateTime",
      "IAR_BuildTime"
    };

    static S4_DeviceMemory()
    {
      S4_DeviceMemory.ParameterGroups = new SortedList<CommonOverwriteGroups, string[]>();
      S4_DeviceMemory.AddParameterGroupContent(CommonOverwriteGroups.IdentData, S4_DeviceMemory.IdentData);
      S4_DeviceMemory.AddParameterGroupContent(CommonOverwriteGroups.TypeIdentification, S4_DeviceMemory.TypeIdentification);
      S4_DeviceMemory.AddParameterGroupContent(CommonOverwriteGroups.UltrasonicCalibration, S4_DeviceMemory.UltrasonicCalibration);
      S4_DeviceMemory.AddParameterGroupContent(CommonOverwriteGroups.UltrasonicHydraulicTestSetup, S4_DeviceMemory.UltrasonicCalibration);
      S4_DeviceMemory.AddParameterGroupContent(CommonOverwriteGroups.UltrasonicLimits, S4_DeviceMemory.UltrasonicLimits);
      S4_DeviceMemory.AddParameterGroupContent(CommonOverwriteGroups.UltrasonicTempSensorCurve, S4_DeviceMemory.UltrasonicTempSensorCurve);
      S4_DeviceMemory.AddParameterGroupContent(CommonOverwriteGroups.MenuDefinition, S4_DeviceMemory.MenuDefinition);
      S4_DeviceMemory.AddParameterGroupContent(CommonOverwriteGroups.DeviceProtection, S4_DeviceMemory.DeviceProtection);
      S4_DeviceMemory.AddParameterGroupContent(CommonOverwriteGroups.RTC_Calibration, S4_DeviceMemory.RTC_Calibration);
      S4_DeviceMemory.AddParameterGroupContent(CommonOverwriteGroups.CarrierFrequencyCalibration, S4_DeviceMemory.CarrierFrequencyCalibration);
      S4_DeviceMemory.AddParameterGroupContent(CommonOverwriteGroups.ConfigurationParameters, S4_DeviceMemory.ConfigurationParameters);
      S4_DeviceMemory.AddParameterGroupContent(CommonOverwriteGroups.ZeroFlowCalibration, S4_DeviceMemory.ZeroFlowCalibration);
    }

    private static void AddParameterGroupContent(
      CommonOverwriteGroups theGroup,
      S4_Params[] parameterList)
    {
      string[] array = new string[parameterList.Length];
      for (int index = 0; index < parameterList.Length; ++index)
        array[index] = parameterList[index].ToString();
      Array.Sort<string>(array);
      S4_DeviceMemory.ParameterGroups.Add(theGroup, array);
    }

    internal static string GetOverwriteGroupInfo(CommonOverwriteGroups overwriteGroupe)
    {
      StringBuilder info = new StringBuilder();
      if (!S4_DeviceMemory.ParameterGroups.ContainsKey(overwriteGroupe))
        info.AppendLine("No group info defined by S4_Handler");
      else
        S4_DeviceMemory.AddGroupParameter(info, S4_DeviceMemory.ParameterGroups[overwriteGroupe]);
      return info.ToString();
    }

    private static void AddGroupParameter(StringBuilder info, string[] parameterList)
    {
      foreach (string parameter in parameterList)
        info.AppendLine(parameter);
    }

    internal S4_DeviceMemory(uint Version)
      : base(Version, Assembly.GetExecutingAssembly())
    {
      if (Version == 0U)
        return;
      if (this.MapDef == null)
        this.MapDef = MapDefClassBase.GetMapObjectDynamicFromVersion(Assembly.GetExecutingAssembly(), this.FirmwareVersion);
      this.DefineMapAndRanges();
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.ArmIdRange, 0U);
      this.AddMemoryBlock(DeviceMemoryType.DataRAM, this.RamRange, 0U);
      this.UsedRamRanges = this.MapDef.GetUsedSubAddressRanges(this.RamRange, 250);
      foreach (AddressRange usedSubAddressRange in this.MapDef.GetUsedSubAddressRanges(this.FlashRange, 250, S4_DeviceMemory.NotUsedMapParameters))
        this.AddMemoryBlock(DeviceMemoryType.FLASH, usedSubAddressRange, 250U);
      this.AddMemoryBlock(DeviceMemoryType.EEPROM, this.EEPromRange, 250U);
      if (this.SelectedReadoutParametersRange != null)
        this.AddMemoryBlock(DeviceMemoryType.FLASH, this.SelectedReadoutParametersRange, 250U);
      if (this.SmartFunctionFlashRange != null)
        this.AddMemoryBlock(DeviceMemoryType.FLASH, this.SmartFunctionFlashRange, 250U);
      if (this.ScenarioConfigurationFlashRange == null)
        return;
      this.AddMemoryBlock(DeviceMemoryType.FLASH, this.ScenarioConfigurationFlashRange, 250U);
    }

    internal S4_DeviceMemory(byte[] compressedData)
      : base(compressedData)
    {
      this.MapDef = MapDefClassBase.GetMapObjectFromVersion(Assembly.GetExecutingAssembly(), this.FirmwareVersion);
      if (this.MapDef == null)
        return;
      this.DefineMapAndRanges();
    }

    private void DefineMapAndRanges()
    {
      this.UsedParametersByName = this.MapDef.GetUsedParametersList(Enum.GetNames(typeof (S4_Params)));
      this.ArmIdRange = new AddressRange(536346704U, 24U);
      this.RamRange = new AddressRange(536870912U)
      {
        EndAddress = 536891391U
      };
      this.FlashRange = new AddressRange(134217728U)
      {
        EndAddress = 134414335U
      };
      this.MeterKeyRange = new AddressRange(134742016U)
      {
        EndAddress = 134742024U
      };
      this.EEPromRange = new AddressRange(134742016U)
      {
        EndAddress = 134748159U
      };
      this.SelectedReadoutParametersRange = this.MapDef.GetSectionRanges("CONF_PARA_SEL");
      this.SmartFunctionFlashRange = this.MapDef.GetSectionRanges("SMART_FUNC_FL");
      this.ScenarioConfigurationFlashRange = this.MapDef.GetSectionRanges("SCENARIO_CONFIG");
      this.AllSections = this.MapDef.GetAllSectionRanges();
      int index1 = this.AllSections.IndexOfKey("CONF_PARA_SEL");
      if (index1 >= 0)
      {
        uint num1 = this.AllSections.Values[index1].EndAddress + 1U;
        int index2 = this.AllSections.IndexOfKey("BACKUP");
        if (index2 >= 0)
        {
          this.RAM_BackupRange = this.AllSections.Values[index2];
          int index3 = this.AllSections.IndexOfKey("BACKUP_0");
          if (index3 >= 0)
          {
            uint startAddress1 = this.AllSections.Values[index3].StartAddress;
            int index4 = this.AllSections.IndexOfKey("BACKUP_1");
            if (index4 >= 0)
            {
              uint startAddress2 = this.AllSections.Values[index4].StartAddress;
              this.FlashBackupRange = new AddressRange(startAddress1, (uint) (((int) startAddress2 - (int) startAddress1) * 2));
            }
            else
              this.FlashBackupRange = new AddressRange(startAddress1, this.AllSections.Values[index3].ByteSize * 2U);
          }
          else
          {
            uint byteSize = this.AllSections.Values[index2].ByteSize;
            uint num2 = (uint) ((ulong) num1 & 18446744073709551488UL);
            if ((int) num2 != (int) num1)
              num2 += 128U;
            uint startAddress = num2 + byteSize;
            uint num3 = (uint) ((ulong) startAddress & 18446744073709551488UL);
            if ((int) num3 != (int) startAddress)
              startAddress = num3 + 128U;
            uint num4 = startAddress + byteSize;
            uint num5 = (uint) ((ulong) num4 & 18446744073709551488UL);
            if ((int) num5 != (int) num4)
              num4 = num5 + 128U;
            uint size = (uint) (((int) num4 - (int) startAddress) * 2);
            this.FlashBackupRange = new AddressRange(startAddress, size);
          }
        }
      }
      SortedList<string, Parameter32bit> allParametersList = this.MapDef.GetAllParametersList();
      if (!allParametersList.ContainsKey("TDCMAX_CONFIG_READ_ADR") || !allParametersList.ContainsKey("writeProtectionTable"))
        return;
      uint address1 = allParametersList["TDCMAX_CONFIG_READ_ADR"].Address;
      uint address2 = allParametersList["writeProtectionTable"].Address;
      this.CalibrationRange = new AddressRange(address1, address2 - address1);
    }

    internal AddressRange GetRangeOfParameters(S4_Params[] theParameters)
    {
      string[] parameterNames = new string[theParameters.Length];
      for (int index = 0; index < theParameters.Length; ++index)
        parameterNames[index] = theParameters[index].ToString();
      return this.MapDef.GetAddressRangeOfDefinedParameters(parameterNames);
    }

    private S4_DeviceMemory(S4_DeviceMemory S4_DeviceMemoryToClone)
      : base((DeviceMemory) S4_DeviceMemoryToClone)
    {
      this.ArmIdRange = S4_DeviceMemoryToClone.ArmIdRange.Clone();
      this.RamRange = S4_DeviceMemoryToClone.RamRange.Clone();
      this.FlashRange = S4_DeviceMemoryToClone.FlashRange.Clone();
      this.MeterKeyRange = S4_DeviceMemoryToClone.MeterKeyRange.Clone();
      this.EEPromRange = S4_DeviceMemoryToClone.EEPromRange.Clone();
      if (S4_DeviceMemoryToClone.RAM_BackupRange != null)
        this.RAM_BackupRange = S4_DeviceMemoryToClone.RAM_BackupRange.Clone();
      if (S4_DeviceMemoryToClone.FlashBackupRange != null)
        this.FlashBackupRange = S4_DeviceMemoryToClone.FlashBackupRange.Clone();
      if (S4_DeviceMemoryToClone.CalibrationRange != null)
        this.CalibrationRange = S4_DeviceMemoryToClone.CalibrationRange.Clone();
      if (S4_DeviceMemoryToClone.SelectedReadoutParametersRange != null)
        this.SelectedReadoutParametersRange = S4_DeviceMemoryToClone.SelectedReadoutParametersRange.Clone();
      if (S4_DeviceMemoryToClone.SmartFunctionFlashRange != null)
        this.SmartFunctionFlashRange = S4_DeviceMemoryToClone.SmartFunctionFlashRange.Clone();
      if (S4_DeviceMemoryToClone.ScenarioConfigurationFlashRange == null)
        return;
      this.ScenarioConfigurationFlashRange = S4_DeviceMemoryToClone.ScenarioConfigurationFlashRange.Clone();
    }

    internal S4_DeviceMemory Clone() => new S4_DeviceMemory(this);

    internal void SetParameterValue<T>(S4_Params parameterName, T theValue)
    {
      this.SetParameterValue<T>(parameterName.ToString(), theValue);
    }

    internal T GetParameterValue<T>(S4_Params parameterName)
    {
      return this.GetParameterValue<T>(parameterName.ToString());
    }

    internal uint GetParameterAddress(S4_Params parameterName)
    {
      return this.GetParameterAddress(parameterName.ToString());
    }

    internal AddressRange GetParameterAddressRange(S4_Params parameterName)
    {
      return this.GetParameterAddressRange(parameterName.ToString());
    }

    public byte[] GetData(S4_Params parameterName) => this.GetData(parameterName.ToString());

    public void SetData(S4_Params parameterName, byte[] buffer)
    {
      this.SetData(parameterName.ToString(), buffer);
    }

    internal bool IsParameterAvailable(S4_Params parameterName)
    {
      return this.IsParameterAvailable(parameterName.ToString());
    }

    public bool IsParameterInMap(S4_Params parameterName)
    {
      return this.IsParameterInMap(parameterName.ToString());
    }

    public override string GetParameterInfo(
      string HeaderInfo = null,
      bool SuppressAddresses = false,
      bool SuppressKnownDifferences = false,
      CommonOverwriteGroups[] overwrites = null)
    {
      string str1 = !SuppressAddresses ? "            " : "    ";
      SortedList<string, Parameter32bit> source = this.MapDef != null ? this.MapDef.GetAllParametersList() : throw new HandlerMessageException("Map in DeviceMemory.SaveToFile not defined.");
      if (source == null)
        throw new HandlerMessageException("No parameters defined for DeviceMemory.SaveToFile");
      List<string> stringList = (List<string>) null;
      if (overwrites != null && overwrites.Length != 0)
      {
        stringList = new List<string>();
        foreach (CommonOverwriteGroups overwrite in overwrites)
        {
          string str2 = overwrite.ToString();
          if (!S4_DeviceMemory.ParameterGroups.ContainsKey(overwrite))
            throw new Exception("OverwriteGroup not found in S4_Handler - " + str2);
          foreach (string str3 in S4_DeviceMemory.ParameterGroups[overwrite])
          {
            if (!stringList.Contains(str3))
              stringList.Add(str3);
          }
        }
      }
      List<KeyValuePair<string, Parameter32bit>> list = source.ToList<KeyValuePair<string, Parameter32bit>>();
      list.Sort((Comparison<KeyValuePair<string, Parameter32bit>>) ((a, b) => a.Value.Address.CompareTo(b.Value.Address)));
      StringBuilder result1 = new StringBuilder();
      StringBuilder result2 = new StringBuilder();
      if (HeaderInfo != null)
        result1.AppendLine(HeaderInfo);
      result1.Append(new ZENNER.CommonLibrary.FirmwareVersion(this.FirmwareVersion).ToString());
      result1.AppendLine(" (" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ")");
      result1.AppendLine();
      result1.AppendLine("-------------------------------------------------------------------------------------");
      if (overwrites != null)
      {
        result1.Append("Selected groups: ");
        for (int index = 0; index < overwrites.Length; ++index)
        {
          if (index > 0)
            result1.Append(',');
          result1.Append(overwrites[index].ToString());
        }
        result1.AppendLine();
      }
      bool flag = stringList != null && stringList.Count > 0;
      string str4 = string.Empty;
      AddressRange addressRange1 = (AddressRange) null;
      AddressRange addressRange2 = (AddressRange) null;
      foreach (KeyValuePair<string, Parameter32bit> keyValuePair in list)
      {
        string key = keyValuePair.Key;
        addressRange2 = keyValuePair.Value.AddressRange;
        if (flag)
        {
          if (!stringList.Contains(key))
            continue;
        }
        else
        {
          if (addressRange1 != null)
          {
            int num = (int) addressRange2.StartAddress - (int) addressRange1.EndAddress - 1;
            if (num < 0)
            {
              result1.AppendLine();
              result1.AppendLine("!!!!!!!!!!!!! Parameter overlap !!!!!!!!!!!!!!!!!!");
              result1.AppendLine();
            }
            else if (num > 0 && !SuppressKnownDifferences)
              this.AddGapInfo(result1, addressRange1.EndAddress + 1U, addressRange2.StartAddress - 1U, str1, SuppressAddresses);
          }
          else if (!SuppressKnownDifferences)
            this.AddGapInfo(result1, 0U, addressRange2.EndAddress - 1U, str1, SuppressAddresses);
          addressRange1 = addressRange2;
        }
        string typ = keyValuePair.Value.Typ;
        string section = keyValuePair.Value.Section;
        Parameter32bit.GetValueByteArray(keyValuePair.Value.Address, keyValuePair.Value.Size, (DeviceMemory) this);
        if (section != str4)
        {
          result1.AppendLine();
          result1.AppendLine("----- Section: " + section + " -------------------------------");
          str4 = section;
        }
        result2.Clear();
        if (!SuppressAddresses)
          result2.Append(keyValuePair.Value.Address.ToString("x08") + ": ");
        result2.Append(key);
        if (keyValuePair.Value.SystemType != (Type) null)
          result2.Append(" " + keyValuePair.Value.SystemType.ToString().Replace("System.", ""));
        if (Enum.IsDefined(typeof (S4_Params), (object) key))
          result2.Append(" HandlerManaged");
        if (!SuppressKnownDifferences || !((IEnumerable<string>) S4_DeviceMemory.KnownDifferences).Contains<string>(key))
        {
          if (key == S4_Params.event_data.ToString())
          {
            if (!this.AreDataAvailable(addressRange2))
            {
              result2.Append(" Data not availabel");
            }
            else
            {
              result2.AppendLine(" Event logger content:");
              foreach (EventLoggerData eventLoggerData in S4_LoggerManager.GetEventLoggerDataListFromMemory(this))
                result2.AppendLine(str1 + eventLoggerData.ToString());
              result2.AppendLine(str1 + "Event logger data:");
              byte[] data = this.GetData(addressRange2);
              if (!SuppressAddresses)
                result2.Append(Utility.ByteArrayToHexStringFormated(data, str1, 32, new uint?(addressRange2.StartAddress)));
              else
                result2.Append(Utility.ByteArrayToHexStringFormated(data, str1, 32));
            }
          }
          else if (key == "smart_func_flash")
          {
            if (this.SmartFunctionFlashRange != null && new ZENNER.CommonLibrary.FirmwareVersion(this.FirmwareVersion) >= (object) "1.7.1 IUW")
            {
              try
              {
                S4_SmartFunctionManager smartFunctionManager = new S4_SmartFunctionManager(this);
                smartFunctionManager.SetFunctionIdentsFromMemory(this);
                result2.AppendLine();
                result2.AppendLine(str1 + "SmartFunctions informations:");
                foreach (SmartFunctionIdentAndFlashParams identAndFlashParams in smartFunctionManager.FunctionsFromMemory)
                {
                  result2.AppendLine(str1 + "Name: " + identAndFlashParams.Name + "; Version: " + identAndFlashParams.Version.ToString());
                  if (identAndFlashParams.FlashParameters != null)
                  {
                    result2.Append(str1 + "    Parameters:");
                    foreach (KeyValuePair<string, string> flashParameter in identAndFlashParams.FlashParameters)
                      result2.Append(" " + flashParameter.Key + "=" + flashParameter.Value);
                    result2.AppendLine();
                  }
                }
                result2.Append(str1 + "SmartFunctons data:");
                result2.Append(" Reserved range:" + this.SmartFunctionFlashRange.ToString());
                addressRange2 = this.SmartFunctionFlashRange;
                addressRange1 = addressRange2;
                byte[] data = this.GetData(addressRange2, true);
                AddressRange addressRange3 = new AddressRange(addressRange2.StartAddress, (uint) data.Length);
                result2.Append(" Read data range:" + addressRange3.ToString());
                result2.AppendLine();
                result2.Append(Utility.ByteArrayToHexStringFormated(data, str1, 32, new uint?(addressRange2.StartAddress)));
              }
              catch
              {
                result2.Append(" SmartFunction exception");
              }
            }
            else
              result2.Append(" SmartFunction analysing not possible");
          }
          else
            keyValuePair.Value.AppandValueStrings((DeviceMemory) this, result2, str1);
        }
        else
          result2.Append(" KnownDiff -> Data not added");
        result1.AppendLine(result2.ToString());
      }
      if (!SuppressKnownDifferences && !flag)
        this.AddGapInfo(result1, addressRange2.EndAddress + 1U, uint.MaxValue, str1, SuppressAddresses);
      return result1.ToString();
    }

    private void AddGapInfo(
      StringBuilder result,
      uint startAddress,
      uint endAddress,
      string leftSpaceString,
      bool SuppressAddresses)
    {
      foreach (AddressRange availableByteRange in this.GetAvailableByteRanges(startAddress, endAddress))
      {
        byte[] data = this.GetData(availableByteRange);
        result.Append("gap data: ");
        if (!SuppressAddresses)
          result.Append(availableByteRange.ToString());
        else
          result.Append("size:" + availableByteRange.ByteSize.ToString());
        result.Append(" Data.Bytes:");
        if (data.Length <= 32)
        {
          result.Append(Utility.ByteArrayToHexStringFormated(data, (string) null, 32));
        }
        else
        {
          result.AppendLine();
          if (!SuppressAddresses)
            result.Append(Utility.ByteArrayToHexStringFormated(data, leftSpaceString, 32, new uint?(availableByteRange.StartAddress)));
          else
            result.Append(Utility.ByteArrayToHexStringFormated(data, leftSpaceString, 32));
        }
        result.AppendLine();
      }
    }

    internal enum ConfigFlagRegisterBits : ushort
    {
      US_ONLY_ONE_CHANAL = 1,
      LCD_REVERSE_FLOW_ARROW = 2,
      TDC_INVERT_FLOW_DIRECTIOn = 8,
      TDC_USE_PARALLEL_TOF_EVAL = 16, // 0x0010
      TDC_HIGH_SAMPLED_VALUES_LOG = 32, // 0x0020
      TDC_USE_MEDIAN_FOR_PARALLEL_TOF_EVAL = 64, // 0x0040
    }
  }
}
