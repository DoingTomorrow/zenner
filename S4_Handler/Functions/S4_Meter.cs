// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.S4_Meter
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using GmmDbLib;
using HandlerLib;
using NLog;
using StartupLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace S4_Handler.Functions
{
  internal class S4_Meter : IMeterAndFilter, IMeter
  {
    internal S4_DeviceIdentification deviceIdentification;
    internal S4_DeviceMemory meterMemory;
    internal S4_SmartFunctionManager MySmartFunctionManager;
    internal S4_ScenarioManager MyScenarioManager;
    internal bool ScenariosRequired = false;
    internal bool UseRights = false;
    internal bool SetTimeOnWrite = false;
    internal bool SetPcTimeOnWrite = false;
    internal bool SetTimeForTimeZoneFromPcTime = false;
    internal bool JoinRequestOnWrite = false;
    internal bool InitEventsOnWrite = false;
    internal string DatabaseTypeCreationString;
    internal string UsedTypeCreationString;
    internal int? MeterInfoIdFromDb;
    internal DateTime? BackupTime;
    internal static Logger S4_ConfigurationParameterLogger = LogManager.GetLogger("S4_ConfigurationParameter");
    internal VolumeCalibrationValues calibrationValues = (VolumeCalibrationValues) null;
    internal S4_BatteryEnergyManagement BatteryManager = (S4_BatteryEnergyManagement) null;
    private static OverrideID[] MenuViewByIndex = new OverrideID[10]
    {
      OverrideID.MenuView01,
      OverrideID.MenuView02,
      OverrideID.MenuView03,
      OverrideID.MenuView04,
      OverrideID.MenuView05,
      OverrideID.MenuView06,
      OverrideID.MenuView07,
      OverrideID.MenuView08,
      OverrideID.MenuView09,
      OverrideID.MenuView10
    };
    private static OverrideID[] MenuViewByIndex_L2 = new OverrideID[10]
    {
      OverrideID.MenuView01_Sel,
      OverrideID.MenuView02_Sel,
      OverrideID.MenuView03_Sel,
      OverrideID.MenuView04_Sel,
      OverrideID.MenuView05_Sel,
      OverrideID.MenuView06_Sel,
      OverrideID.MenuView07_Sel,
      OverrideID.MenuView08_Sel,
      OverrideID.MenuView09_Sel,
      OverrideID.MenuView10_Sel
    };
    private static OverrideID[] MenuViewByIndex_Time = new OverrideID[10]
    {
      OverrideID.MenuView01_Time,
      OverrideID.MenuView02_Time,
      OverrideID.MenuView03_Time,
      OverrideID.MenuView04_Time,
      OverrideID.MenuView05_Time,
      OverrideID.MenuView06_Time,
      OverrideID.MenuView07_Time,
      OverrideID.MenuView08_Time,
      OverrideID.MenuView09_Time,
      OverrideID.MenuView10_Time
    };

    internal DeviceCharacteristics DeviceCharacteristics { get; private set; }

    internal BaseType BaseType { get; set; }

    internal bool ClockIsBinary
    {
      get
      {
        uint? firmwareVersion = this.deviceIdentification.FirmwareVersion;
        uint num = 1011006;
        return firmwareVersion.GetValueOrDefault() >= num & firmwareVersion.HasValue;
      }
    }

    public bool IsProtected => this.deviceIdentification.IsProtected;

    public S4_Meter()
    {
    }

    public S4_Meter(uint firmwareVersion)
      : this()
    {
      this.meterMemory = new S4_DeviceMemory(firmwareVersion);
    }

    public S4_Meter(DeviceIdentification deviceIdentification)
      : this()
    {
      try
      {
        this.meterMemory = new S4_DeviceMemory(deviceIdentification.FirmwareVersion.Value);
        this.deviceIdentification = new S4_DeviceIdentification(this.meterMemory, deviceIdentification);
      }
      catch
      {
        this.deviceIdentification = new S4_DeviceIdentification(deviceIdentification);
      }
    }

    public S4_Meter(
      S4_DeviceMemory meterMemory,
      DeviceIdentification deviceIdentification,
      BaseType baseType)
      : this()
    {
      if (meterMemory != null)
        this.meterMemory = meterMemory.Clone();
      this.deviceIdentification = new S4_DeviceIdentification(this.meterMemory, deviceIdentification);
      if (baseType == null)
        return;
      this.BaseType = baseType.DeepCopy();
    }

    internal S4_Meter Clone()
    {
      return new S4_Meter(this.meterMemory, (DeviceIdentification) this.deviceIdentification, this.BaseType)
      {
        SetTimeOnWrite = this.SetTimeOnWrite,
        SetPcTimeOnWrite = this.SetPcTimeOnWrite,
        SetTimeForTimeZoneFromPcTime = this.SetTimeForTimeZoneFromPcTime,
        JoinRequestOnWrite = this.JoinRequestOnWrite,
        InitEventsOnWrite = this.InitEventsOnWrite,
        DatabaseTypeCreationString = this.DatabaseTypeCreationString,
        UsedTypeCreationString = this.UsedTypeCreationString,
        MeterInfoIdFromDb = this.MeterInfoIdFromDb,
        MySmartFunctionManager = this.MySmartFunctionManager,
        MyScenarioManager = this.MyScenarioManager,
        ScenariosRequired = this.ScenariosRequired
      };
    }

    internal IMeterAndFilter CreateFromData(
      byte[] compresseddata,
      int? hardwareTypeID,
      HardwareTypeSupport hardwareTypeSupport)
    {
      this.meterMemory = new S4_DeviceMemory(compresseddata);
      this.deviceIdentification = new S4_DeviceIdentification(this.meterMemory, hardwareTypeID, hardwareTypeSupport);
      if (this.meterMemory.SmartFunctionFlashRange != null)
        this.MySmartFunctionManager = new S4_SmartFunctionManager(this.meterMemory);
      return (IMeterAndFilter) this;
    }

    internal async Task<double> ReadAndGetTemperature(
      NfcDeviceCommands nfcCom,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      uint tdc1StructAdr = this.meterMemory.GetParameterAddress(S4_Params.unit2);
      byte[] readData = await nfcCom.ReadMemoryAsync(progress, cancelToken, tdc1StructAdr + 42U, 4U);
      short temperature = BitConverter.ToInt16(readData, 0);
      ushort errorFlags = BitConverter.ToUInt16(readData, 2);
      double temperature1 = ((uint) errorFlags & 240U) <= 0U ? (double) temperature / 100.0 : double.MinValue;
      readData = (byte[]) null;
      return temperature1;
    }

    internal void InitMeterData()
    {
      FirmwareVersion firmwareVersion = new FirmwareVersion(this.deviceIdentification.FirmwareVersion.Value);
      if (firmwareVersion < (object) "1.2.3 IUW")
        return;
      S4_DiagnosticData.InitDiagnosticData(this.meterMemory, DateTime.MinValue);
      S4_DiagnosticData.InitKeyDateData(this.meterMemory, DateTime.MinValue);
      if (firmwareVersion < (object) "1.4.8 IUW")
        return;
      S4_LoggerManager.ClearMonthLoggerInMemory(this.meterMemory);
      S4_LoggerManager.ClearEventLoggerMemory(this.meterMemory);
      DeviceStateCounter.DeleteStateInMemory((DeviceMemory) this.meterMemory, this.GetStateCounterRanges());
      this.InitEventsOnWrite = true;
    }

    internal string GetBackupData()
    {
      StringBuilder stringBuilder = new StringBuilder();
      AddressRange flashBackupRange = this.meterMemory.FlashBackupRange;
      if (flashBackupRange != null && this.meterMemory.RAM_BackupRange != null)
      {
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("--- BACKUP BLOCK VALUES ---");
        stringBuilder.AppendLine();
        uint size = flashBackupRange.ByteSize / 2U;
        AddressRange addressRange1 = new AddressRange(flashBackupRange.StartAddress, size);
        AddressRange addressRange2 = new AddressRange(flashBackupRange.StartAddress + size, size);
        byte[] numArray1 = (byte[]) null;
        byte[] numArray2 = (byte[]) null;
        if (this.meterMemory.AreDataAvailable(addressRange1))
          numArray1 = this.meterMemory.GetData(addressRange1);
        if (this.meterMemory.AreDataAvailable(addressRange2))
          numArray2 = this.meterMemory.GetData(addressRange2);
        stringBuilder.AppendLine("Backup 0:");
        stringBuilder.AppendLine();
        if (numArray1 != null)
        {
          numArray1 = this.meterMemory.GetData(addressRange1);
          stringBuilder.AppendLine(this.GetBackupBlockValues(addressRange1, numArray1));
        }
        else
          stringBuilder.AppendLine("No data available");
        stringBuilder.AppendLine("Backup 1:");
        stringBuilder.AppendLine();
        if (numArray2 != null)
        {
          numArray2 = this.meterMemory.GetData(addressRange2);
          stringBuilder.AppendLine(this.GetBackupBlockValues(addressRange2, numArray2));
        }
        else
          stringBuilder.AppendLine("No data available");
        if (numArray1 != null || numArray2 != null)
        {
          stringBuilder.AppendLine();
          stringBuilder.AppendLine("--- BACKUP BLOCK DATA ---");
          stringBuilder.AppendLine();
        }
        if (numArray1 != null)
        {
          stringBuilder.AppendLine("Backup 0:");
          stringBuilder.AppendLine();
          string hexString = Util.ByteArrayToHexString(numArray1);
          int startIndex = 0;
          int num = 64;
          uint startAddress = addressRange1.StartAddress;
          do
          {
            string empty = string.Empty;
            if (startIndex < hexString.Length)
              stringBuilder.AppendLine("0x" + startAddress.ToString("x8") + ": " + hexString.Substring(startIndex, hexString.Length - startIndex < num ? hexString.Length - startIndex : num));
            startIndex += num;
            startAddress += (uint) (num / 2);
          }
          while (startIndex < hexString.Length);
        }
        if (numArray1 != null)
        {
          stringBuilder.AppendLine("Backup 1:");
          stringBuilder.AppendLine();
          string hexString = Util.ByteArrayToHexString(numArray2);
          int startIndex = 0;
          int num = 64;
          uint startAddress = addressRange2.StartAddress;
          do
          {
            string empty = string.Empty;
            if (startIndex < hexString.Length)
              stringBuilder.AppendLine("0x" + startAddress.ToString("x8") + ": " + hexString.Substring(startIndex, hexString.Length - startIndex < num ? hexString.Length - startIndex : num));
            startIndex += num;
            startAddress += (uint) (num / 2);
          }
          while (startIndex < hexString.Length);
        }
      }
      else
        stringBuilder.AppendLine("Backup ranges not defined");
      return stringBuilder.ToString();
    }

    private string GetBackupBlockValues(AddressRange backUpRange, byte[] backupData)
    {
      StringBuilder stringBuilder = new StringBuilder();
      byte[] numArray = new byte[(int) this.meterMemory.RAM_BackupRange.ByteSize];
      Buffer.BlockCopy((Array) backupData, 0, (Array) numArray, 0, numArray.Length);
      if ((int) HandlerLib.CRC.CRC_CCITT(numArray) != (int) BitConverter.ToUInt16(backupData, (int) this.meterMemory.RAM_BackupRange.ByteSize))
      {
        stringBuilder.AppendLine("CRC error");
      }
      else
      {
        stringBuilder.AppendLine("CRC ok");
        stringBuilder.AppendLine("Time stamp: " + FirmwareDateTimeSupport.ToDateTimeOffsetBCD(backupData).ToString());
        stringBuilder.AppendLine();
        uint startAddress = this.meterMemory.RAM_BackupRange.StartAddress;
        uint startIndex1 = this.meterMemory.GetParameterAddress(S4_Params.volQmSum) - startAddress;
        uint startIndex2 = this.meterMemory.GetParameterAddress(S4_Params.volQmPos) - startAddress;
        uint startIndex3 = this.meterMemory.GetParameterAddress(S4_Params.volQmNeg) - startAddress;
        stringBuilder.AppendLine("Accumulated volume ...... : " + BitConverter.ToDouble(backupData, (int) startIndex1).ToString());
        stringBuilder.AppendLine("Volume in flow direction  : " + BitConverter.ToDouble(backupData, (int) startIndex2).ToString());
        stringBuilder.AppendLine("Volume in return direction: " + BitConverter.ToDouble(backupData, (int) startIndex3).ToString());
      }
      return stringBuilder.ToString();
    }

    internal SortedList<DeviceStateCounterID, AddressRange> GetStateCounterRanges()
    {
      return new SortedList<DeviceStateCounterID, AddressRange>()
      {
        {
          DeviceStateCounterID.OperationTime,
          this.meterMemory.GetParameterAddressRange(S4_Params.time_operation)
        },
        {
          DeviceStateCounterID.ZeroFlowTime,
          this.meterMemory.GetParameterAddressRange(S4_Params.time_zeroflow)
        },
        {
          DeviceStateCounterID.FlowTime,
          this.meterMemory.GetParameterAddressRange(S4_Params.time_flow)
        },
        {
          DeviceStateCounterID.BackFlowTime,
          this.meterMemory.GetParameterAddressRange(S4_Params.time_backflow)
        },
        {
          DeviceStateCounterID.AirInTubeTime,
          this.meterMemory.GetParameterAddressRange(S4_Params.time_tubeempty)
        },
        {
          DeviceStateCounterID.OverloadTime,
          this.meterMemory.GetParameterAddressRange(S4_Params.time_flowoutofrange)
        },
        {
          DeviceStateCounterID.NFC_ActivationTime,
          this.meterMemory.GetParameterAddressRange(S4_Params.time_nfc)
        },
        {
          DeviceStateCounterID.TemperatureSensorErrorTime,
          this.meterMemory.GetParameterAddressRange(S4_Params.time_tempsensor)
        },
        {
          DeviceStateCounterID.OneSensorPairErrorTime,
          this.meterMemory.GetParameterAddressRange(S4_Params.time_transducererror)
        },
        {
          DeviceStateCounterID.SmartFunctionsCpuMsTime,
          this.meterMemory.GetParameterAddressRange(S4_Params.time_sfunc_run)
        },
        {
          DeviceStateCounterID.WatchDogCounts,
          this.meterMemory.GetParameterAddressRange(S4_Params.resetCounterIwdg)
        },
        {
          DeviceStateCounterID.ResetCounts,
          this.meterMemory.GetParameterAddressRange(S4_Params.resetCounter)
        },
        {
          DeviceStateCounterID.TDCResetCounts,
          this.meterMemory.GetParameterAddressRange(S4_Params.resetCounterTdc)
        }
      };
    }

    private void UpdateDeviceCharacteristics()
    {
      this.DeviceCharacteristics = new DeviceCharacteristics();
      this.DeviceCharacteristics.MinimumFlowrateQ1_qmPerHour = this.meterMemory.GetParameterValue<float>(S4_Params.minimumFlowrateQ1);
      this.DeviceCharacteristics.TransitionalFlowrateQ2_qmPerHour = this.meterMemory.GetParameterValue<float>(S4_Params.transitionalFlowrateQ2);
      this.DeviceCharacteristics.PermanentFlowrateQ3_qmPerHour = this.meterMemory.GetParameterValue<float>(S4_Params.permanentFlowrateQ3);
      this.DeviceCharacteristics.OverloadFlowrateQ4_qmPerHour = this.meterMemory.GetParameterValue<float>(S4_Params.overloadFlowrateQ4);
    }

    internal string GetStatusText()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.MeterInfoIdFromDb.HasValue)
        stringBuilder.Append("MeterInfoID: " + this.MeterInfoIdFromDb.ToString());
      if (this.UsedTypeCreationString != null)
        stringBuilder.Append(" -> '" + this.UsedTypeCreationString + "'");
      if (stringBuilder.Length > 0)
      {
        stringBuilder.Insert(0, "DB: *** ");
        stringBuilder.AppendLine(" ***");
      }
      if (this.BackupTime.HasValue)
        stringBuilder.Insert(0, "Backup time: " + this.BackupTime.Value.ToShortDateString() + " " + this.BackupTime.Value.ToShortTimeString() + Environment.NewLine);
      stringBuilder.Append(this.deviceIdentification.ToString());
      return stringBuilder.ToString();
    }

    public string GetInfo()
    {
      return this.meterMemory != null ? this.meterMemory.GetParameterInfo((string) null, false, false, (CommonOverwriteGroups[]) null) : string.Empty;
    }

    public bool IsParameterLikeRequired(string parameterName, string valueStartsWith = null)
    {
      SortedList<string, Parameter32bit> allParametersList = this.meterMemory.MapDef.GetAllParametersList();
      if (!allParametersList.ContainsKey(parameterName))
        return false;
      if (string.IsNullOrEmpty(valueStartsWith))
        return true;
      Parameter32bit p = allParametersList[parameterName];
      return p != null && this.meterMemory.GetParameterData(p).StartsWith(valueStartsWith);
    }

    public override string ToString()
    {
      if (this.deviceIdentification == null)
        return "Identification not available.";
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.AppendLine("IUW device summary");
      stringBuilder1.AppendLine();
      if (this.meterMemory == null)
      {
        stringBuilder1.AppendLine("Memory data not available");
      }
      else
      {
        try
        {
          if (new FirmwareVersion(this.deviceIdentification.FirmwareVersion.Value) < (object) "1.1.1 IUW")
          {
            stringBuilder1.AppendLine("Communication summary not supported for this firmware.");
          }
          else
          {
            S4_CommunicationManager communicationManager = new S4_CommunicationManager(this.meterMemory);
            S4_MenuManager s4MenuManager = new S4_MenuManager(this.meterMemory);
            stringBuilder1.AppendLine();
            stringBuilder1.AppendLine("*** Device identification ***");
            stringBuilder1.AppendLine(this.deviceIdentification.ToString());
            stringBuilder1.AppendLine();
            stringBuilder1.AppendLine("*** Date and time ***");
            if (this.meterMemory.IsParameterAvailable(S4_Params.sysDateTime))
            {
              DateTimeOffset dateTimeOffsetBcd = FirmwareDateTimeSupport.ToDateTimeOffsetBCD(this.meterMemory.GetData(S4_Params.sysDateTime));
              stringBuilder1.AppendLine("Device time: " + dateTimeOffsetBcd.ToString());
            }
            DateTime dateTime;
            if (this.meterMemory.IsParameterAvailable(S4_Params.BatterieEnd_Date))
            {
              DateTime? fromFirmwareDateBcd = FirmwareDateTimeSupport.ToDateTimeFromFirmwareDateBCD(this.meterMemory.GetParameterValue<uint>(S4_Params.BatterieEnd_Date));
              if (fromFirmwareDateBcd.HasValue)
              {
                StringBuilder stringBuilder2 = stringBuilder1;
                dateTime = fromFirmwareDateBcd.Value;
                string str = "Batterie empty date: " + dateTime.ToShortDateString();
                stringBuilder2.AppendLine(str);
              }
              else
                stringBuilder1.AppendLine("Batterie empty date not defined");
            }
            if (this.meterMemory.IsParameterAvailable(S4_Params.BatterieWarn_Date))
            {
              DateTime? fromFirmwareDateBcd = FirmwareDateTimeSupport.ToDateTimeFromFirmwareDateBCD(this.meterMemory.GetParameterValue<uint>(S4_Params.BatterieWarn_Date));
              if (fromFirmwareDateBcd.HasValue)
              {
                StringBuilder stringBuilder3 = stringBuilder1;
                dateTime = fromFirmwareDateBcd.Value;
                string str = "Batterie warning date: " + dateTime.ToShortDateString();
                stringBuilder3.AppendLine(str);
              }
              else
                stringBuilder1.AppendLine("Batterie warning date not defined");
            }
            if (this.meterMemory.IsParameterAvailable(S4_Params.BatteryDurabilityMonth))
            {
              byte parameterValue = this.meterMemory.GetParameterValue<byte>(S4_Params.BatteryDurabilityMonth);
              stringBuilder1.AppendLine("Battery durability months: " + parameterValue.ToString());
            }
            if (this.meterMemory.IsParameterAvailable(S4_Params.BatteryPreWarningMonth))
            {
              sbyte parameterValue = this.meterMemory.GetParameterValue<sbyte>(S4_Params.BatteryPreWarningMonth);
              stringBuilder1.AppendLine("Battery pre warning months: " + parameterValue.ToString());
            }
            stringBuilder1.AppendLine();
            stringBuilder1.AppendLine("*** Ultrasonic configuration ***");
            TdcChannelConfig tdcChannelConfig = new TdcChannelConfig(this.meterMemory);
            stringBuilder1.Append("Number of actice transducer channels: ");
            if (tdcChannelConfig.TwoTransducersUsed)
              stringBuilder1.AppendLine("2");
            else
              stringBuilder1.AppendLine("1");
            stringBuilder1.Append("The configured transducer distance is from ");
            stringBuilder1.Append(tdcChannelConfig.MinTransducerDistance.ToString("0.0") + "mm");
            stringBuilder1.AppendLine(" to " + tdcChannelConfig.MaxTransducerDistance.ToString("0.0") + "mm");
            stringBuilder1.AppendLine();
            if (this.meterMemory.IsParameterAvailable(S4_Params.minimumFlowrateQ1))
            {
              stringBuilder1.AppendLine();
              stringBuilder1.AppendLine("Flow limits");
              stringBuilder1.AppendLine("Qmax: " + this.meterMemory.GetParameterValue<float>(S4_Params.maxPositivFlow).ToString());
              stringBuilder1.AppendLine("Q4:   " + this.meterMemory.GetParameterValue<float>(S4_Params.overloadFlowrateQ4).ToString());
              stringBuilder1.AppendLine("Q3:   " + this.meterMemory.GetParameterValue<float>(S4_Params.permanentFlowrateQ3).ToString());
              StringBuilder stringBuilder4 = stringBuilder1;
              float parameterValue = this.meterMemory.GetParameterValue<float>(S4_Params.transitionalFlowrateQ2);
              string str1 = "Q2:   " + parameterValue.ToString();
              stringBuilder4.AppendLine(str1);
              StringBuilder stringBuilder5 = stringBuilder1;
              parameterValue = this.meterMemory.GetParameterValue<float>(S4_Params.minimumFlowrateQ1);
              string str2 = "Q1:   " + parameterValue.ToString();
              stringBuilder5.AppendLine(str2);
              StringBuilder stringBuilder6 = stringBuilder1;
              parameterValue = this.meterMemory.GetParameterValue<float>(S4_Params.minPositivFlow);
              string str3 = "Qmin: " + parameterValue.ToString();
              stringBuilder6.AppendLine(str3);
            }
            stringBuilder1.AppendLine();
            stringBuilder1.AppendLine("*** Default device units ***");
            stringBuilder1.AppendLine("   " + S4_DifVif_Parameter.BaseUnitScale[communicationManager.DefaultUnitScale].DisplayString);
            stringBuilder1.AppendLine("   " + S4_DifVif_Parameter.BaseUnitScale[S4_DifVif_Parameter.AllVolumeUnitBaseDefines[communicationManager.DefaultUnitScale].flowInfo.scaleUnit].DisplayString);
            stringBuilder1.AppendLine();
            stringBuilder1.AppendLine("*** Communication setup ***");
            stringBuilder1.AppendLine(communicationManager.ToString("   "));
          }
        }
        catch (Exception ex)
        {
          stringBuilder1.AppendLine(" !!!!!!! Communication info exception:");
          stringBuilder1.AppendLine(ex.Message);
        }
        try
        {
          if (this.meterMemory.SmartFunctionFlashRange != null)
          {
            string[] smartFunctionNames = S4_SmartFunctionManager.GetSmartFunctionNames(this.MySmartFunctionManager.FunctionsFromMemory);
            if (smartFunctionNames == null || smartFunctionNames.Length == 0)
            {
              stringBuilder1.AppendLine("No smart functions loaded");
            }
            else
            {
              stringBuilder1.AppendLine();
              stringBuilder1.AppendLine("Loaded smart functions:");
              int num = 0;
              foreach (string str in smartFunctionNames)
              {
                ++num;
                stringBuilder1.AppendLine("  " + num.ToString("d2") + ": " + str);
              }
            }
          }
        }
        catch (Exception ex)
        {
          stringBuilder1.AppendLine(" !!!!!!! SmartFunction exception:");
          stringBuilder1.AppendLine(ex.Message);
        }
        try
        {
          S4_ScenarioManager s4ScenarioManager = new S4_ScenarioManager((S4_DeviceCommandsNFC) null, this.meterMemory);
          if (!s4ScenarioManager.PrepareConfigurationFromMap())
            stringBuilder1.AppendLine("Scenarios not available inside the map");
          else
            stringBuilder1.AppendLine("   " + s4ScenarioManager.GetShortScenariosTextBlockFromMap());
        }
        catch (Exception ex)
        {
          stringBuilder1.AppendLine(" !!!!!!! Scenario exception:");
          stringBuilder1.AppendLine(ex.Message);
        }
      }
      return stringBuilder1.ToString();
    }

    internal SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      string traceInfo,
      bool allParameters = false)
    {
      S4_Meter.S4_ConfigurationParameterLogger.Trace(traceInfo);
      try
      {
        S4_MenuManager s4MenuManager = new S4_MenuManager(this.meterMemory);
        List<ViewDefinition> menuDefinition = s4MenuManager.GetMenuDefinition();
        string defaultMenuName = SupportedMenu.GetDefaultMenuName(menuDefinition);
        string resolution = s4MenuManager.GetResolution(menuDefinition);
        S4_BaseUnits baseUnit = s4MenuManager.GetBaseUnit();
        SortedList<OverrideID, ConfigurationParameter> configParamList = new SortedList<OverrideID, ConfigurationParameter>();
        uint? nullable1;
        if (this.deviceIdentification != null)
        {
          ConfigurationParameter configurationParameter1 = this.AddConfigurationParameter(configParamList, OverrideID.MeterID);
          int num1;
          if (configurationParameter1 != null)
          {
            nullable1 = this.deviceIdentification.MeterID;
            num1 = nullable1.HasValue ? 1 : 0;
          }
          else
            num1 = 0;
          if (num1 != 0)
          {
            configurationParameter1.HasWritePermission = false;
            ConfigurationParameter configurationParameter2 = configurationParameter1;
            nullable1 = this.deviceIdentification.MeterID;
            // ISSUE: variable of a boxed type
            __Boxed<uint> local = (System.ValueType) nullable1.Value;
            configurationParameter2.ParameterValue = (object) local;
          }
          ConfigurationParameter configurationParameter3 = this.AddConfigurationParameter(configParamList, OverrideID.SerialNumberFull);
          if (configurationParameter3 != null && this.deviceIdentification.FullSerialNumber != null)
          {
            configurationParameter3.HasWritePermission = false;
            configurationParameter3.ParameterValue = (object) this.deviceIdentification.FullSerialNumber;
          }
          ConfigurationParameter configurationParameter4 = this.AddConfigurationParameter(configParamList, OverrideID.PrintedSerialNumber);
          if (configurationParameter4 != null && this.deviceIdentification.PrintedSerialNumberAsString != null)
          {
            configurationParameter4.HasWritePermission = false;
            configurationParameter4.ParameterValue = (object) this.deviceIdentification.PrintedSerialNumberAsString;
          }
          ConfigurationParameter configurationParameter5 = this.AddConfigurationParameter(configParamList, OverrideID.ErrorCode);
          int num2;
          if (configurationParameter5 != null)
          {
            nullable1 = this.deviceIdentification.DeviceStatusFlags;
            num2 = nullable1.HasValue ? 1 : 0;
          }
          else
            num2 = 0;
          if (num2 != 0)
          {
            configurationParameter5.HasWritePermission = false;
            ConfigurationParameter configurationParameter6 = configurationParameter5;
            nullable1 = this.deviceIdentification.DeviceStatusFlags;
            string str = nullable1.Value.ToString("x04");
            configurationParameter6.ParameterValue = (object) str;
          }
          ConfigurationParameter configurationParameter7 = this.AddConfigurationParameter(configParamList, OverrideID.FirmwareVersion);
          FirmwareVersion firmwareVersionObj;
          int num3;
          if (configurationParameter7 != null)
          {
            firmwareVersionObj = this.deviceIdentification.FirmwareVersionObj;
            num3 = firmwareVersionObj.VersionString != null ? 1 : 0;
          }
          else
            num3 = 0;
          if (num3 != 0)
          {
            configurationParameter7.HasWritePermission = false;
            ConfigurationParameter configurationParameter8 = configurationParameter7;
            firmwareVersionObj = this.deviceIdentification.FirmwareVersionObj;
            string versionString = firmwareVersionObj.VersionString;
            configurationParameter8.ParameterValue = (object) versionString;
          }
          ConfigurationParameter configurationParameter9 = this.AddConfigurationParameter(configParamList, OverrideID.Protected);
          if (configurationParameter9 != null)
          {
            configurationParameter9.HasWritePermission = false;
            configurationParameter9.ParameterValue = (object) this.deviceIdentification.IsProtected;
          }
        }
        if (this.meterMemory.IsParameterAvailable(S4_Params.volQmSum))
        {
          ConfigurationParameter configurationParameter = this.AddConfigurationParameter(configParamList, OverrideID.VolumeActualValue);
          if (configurationParameter != null)
          {
            configurationParameter.HasWritePermission = !this.IsProtected;
            double parameterValue = this.meterMemory.GetParameterValue<double>(S4_Params.volQmSum);
            Decimal result = 0M;
            switch (baseUnit)
            {
              case S4_BaseUnits.Qm:
                if (!Decimal.TryParse(parameterValue.ToString(), out result))
                  result = 0M;
                configurationParameter.ParameterValue = (object) result;
                configurationParameter.Unit = "m\u00B3";
                break;
              case S4_BaseUnits.US_GAL:
                if (!Decimal.TryParse(S4_BaseUnitSupport.QmToUS_GAL(parameterValue).ToString(), out result))
                  result = 0M;
                configurationParameter.ParameterValue = (object) result;
                configurationParameter.Unit = "US_GAL";
                break;
              case S4_BaseUnits.Qft:
                if (!Decimal.TryParse(S4_BaseUnitSupport.QmToQf(parameterValue).ToString(), out result))
                  result = 0M;
                configurationParameter.ParameterValue = (object) result;
                configurationParameter.Unit = "ft\u00B3";
                break;
              default:
                throw new Exception("Base unit error");
            }
            configurationParameter.HasWritePermission = !this.IsProtected;
          }
        }
        if (this.meterMemory.IsParameterAvailable(S4_Params.sysDateTime))
        {
          ConfigurationParameter configurationParameter10 = this.AddConfigurationParameter(configParamList, OverrideID.DeviceClock);
          if (configurationParameter10 != null)
            configurationParameter10.ParameterValue = (object) FirmwareDateTimeSupport.GetDateTimeFromMemoryBCD((DeviceMemory) this.meterMemory);
          ConfigurationParameter configurationParameter11 = this.AddConfigurationParameter(configParamList, OverrideID.TimeZone);
          if (configurationParameter11 != null)
            configurationParameter11.ParameterValue = (object) FirmwareDateTimeSupport.GetTimeZoneFromMemory((DeviceMemory) this.meterMemory);
        }
        ConfigurationParameter configurationParameter12 = this.AddConfigurationParameter(configParamList, OverrideID.SetPcTime);
        if (configurationParameter12 != null)
          configurationParameter12.ParameterValue = (object) this.SetPcTimeOnWrite;
        ConfigurationParameter configurationParameter13 = this.AddConfigurationParameter(configParamList, OverrideID.SetTimeForTimeZoneFromPcTime);
        if (configurationParameter13 != null)
          configurationParameter13.ParameterValue = (object) this.SetTimeForTimeZoneFromPcTime;
        if (this.meterMemory.IsParameterInMap(S4_Params.Key_Date))
        {
          DateTime? nullable2 = FirmwareDateTimeSupport.ToDateTimeFromFirmwareDateBCD(this.meterMemory.GetParameterValue<uint>(S4_Params.Key_Date));
          if (!nullable2.HasValue)
          {
            nullable2 = new DateTime?(new DateTime(1, 1, 1));
          }
          else
          {
            ref DateTime? local = ref nullable2;
            DateTime dateTime1 = nullable2.Value;
            int month = dateTime1.Month;
            dateTime1 = nullable2.Value;
            int day = dateTime1.Day;
            DateTime dateTime2 = new DateTime(1, month, day);
            local = new DateTime?(dateTime2);
          }
          ConfigurationParameter configurationParameter14 = this.AddConfigurationParameter(configParamList, OverrideID.DueDate);
          if (configurationParameter14 != null)
          {
            configurationParameter14.ParameterValue = (object) nullable2;
            configurationParameter14.Format = "dd.MM.";
          }
        }
        ushort num4 = 0;
        ushort communicationScenario2 = 0;
        CommunicationScenarioRange communicationScenarioRange = CommunicationScenarioRange.Non;
        if (this.meterMemory.IsParameterAvailable(S4_Params.com_scenario_intern))
        {
          num4 = this.meterMemory.GetParameterValue<ushort>(S4_Params.com_scenario_intern);
          communicationScenarioRange = ScenarioConfigurations.GetCommunicationScenarioRange(num4);
          ConfigurationParameter configurationParameter15 = this.AddConfigurationParameter(configParamList, OverrideID.CommunicationScenario);
          if (configurationParameter15 != null)
            configurationParameter15.ParameterValue = (object) (int) num4;
        }
        else if (this.meterMemory.IsParameterAvailable(S4_Params.com_scenario_intern_lora))
        {
          communicationScenarioRange = CommunicationScenarioRange.LoRa_and_wMBus;
          num4 = this.meterMemory.GetParameterValue<ushort>(S4_Params.com_scenario_intern_lora);
          ConfigurationParameter configurationParameter16 = this.AddConfigurationParameter(configParamList, OverrideID.CommunicationScenarioLoRa);
          if (configurationParameter16 != null)
            configurationParameter16.ParameterValue = (object) (int) num4;
          communicationScenario2 = this.meterMemory.GetParameterValue<ushort>(S4_Params.com_scenario_intern_wmbus);
          ConfigurationParameter configurationParameter17 = this.AddConfigurationParameter(configParamList, OverrideID.CommunicationScenarioWmbus);
          if (configurationParameter17 != null)
            configurationParameter17.ParameterValue = (object) (int) communicationScenario2;
        }
        if (communicationScenarioRange == CommunicationScenarioRange.LoRa || communicationScenarioRange == CommunicationScenarioRange.LoRa_and_wMBus)
        {
          ConfigurationParameter configurationParameter18 = this.AddConfigurationParameter(configParamList, OverrideID.DevEUI);
          if (configurationParameter18 != null)
            configurationParameter18.ParameterValue = (object) this.meterMemory.GetParameterValue<ulong>(S4_Params.LoRa_DevEUI);
          ConfigurationParameter configurationParameter19 = this.AddConfigurationParameter(configParamList, OverrideID.AppKey);
          if (configurationParameter19 != null)
            configurationParameter19.ParameterValue = (object) this.meterMemory.GetData(S4_Params.LoRa_AppKey);
          ConfigurationParameter configurationParameter20 = this.AddConfigurationParameter(configParamList, OverrideID.JoinEUI);
          if (configurationParameter20 != null)
            configurationParameter20.ParameterValue = (object) this.meterMemory.GetParameterValue<ulong>(S4_Params.LoRa_AppEUI);
          if (this.meterMemory.IsParameterAvailable(S4_Params.cfg_lora_adr_on))
          {
            ConfigurationParameter configurationParameter21 = this.AddConfigurationParameter(configParamList, OverrideID.ADR);
            if (configurationParameter21 != null)
              configurationParameter21.ParameterValue = (object) (this.meterMemory.GetParameterValue<byte>(S4_Params.cfg_lora_adr_on) > (byte) 0);
          }
          if (this.meterMemory.IsParameterAvailable(S4_Params.bck_lora_netid))
          {
            ConfigurationParameter configurationParameter22 = this.AddConfigurationParameter(configParamList, OverrideID.NetID);
            if (configurationParameter22 != null)
            {
              configurationParameter22.ParameterValue = (object) this.meterMemory.GetParameterValue<uint>(S4_Params.bck_lora_netid);
              configurationParameter22.HasWritePermission = false;
            }
          }
          if (this.meterMemory.IsParameterAvailable(S4_Params.bck_lora_deviceid))
          {
            ConfigurationParameter configurationParameter23 = this.AddConfigurationParameter(configParamList, OverrideID.DevAddr);
            if (configurationParameter23 != null)
            {
              configurationParameter23.ParameterValue = (object) this.meterMemory.GetParameterValue<uint>(S4_Params.bck_lora_deviceid);
              configurationParameter23.HasWritePermission = false;
            }
          }
          if (this.meterMemory.IsParameterAvailable(S4_Params.bck_lora_nwkskey))
          {
            ConfigurationParameter configurationParameter24 = this.AddConfigurationParameter(configParamList, OverrideID.NwkSKey);
            if (configurationParameter24 != null)
            {
              configurationParameter24.ParameterValue = (object) this.meterMemory.GetData(S4_Params.bck_lora_nwkskey);
              configurationParameter24.HasWritePermission = false;
            }
          }
          if (this.meterMemory.IsParameterAvailable(S4_Params.bck_lora_appskey))
          {
            ConfigurationParameter configurationParameter25 = this.AddConfigurationParameter(configParamList, OverrideID.AppSKey);
            if (configurationParameter25 != null)
            {
              configurationParameter25.ParameterValue = (object) this.meterMemory.GetData(S4_Params.bck_lora_appskey);
              configurationParameter25.HasWritePermission = false;
            }
          }
          ConfigurationParameter configurationParameter26 = this.AddConfigurationParameter(configParamList, OverrideID.SendJoinRequest);
          if (configurationParameter26 != null)
            configurationParameter26.ParameterValue = (object) this.JoinRequestOnWrite;
        }
        else if ((communicationScenarioRange == CommunicationScenarioRange.wMBus || communicationScenarioRange == CommunicationScenarioRange.LoRa_and_wMBus) && this.meterMemory.IsParameterAvailable(S4_Params.WMBus_AesKey))
        {
          ConfigurationParameter configurationParameter27 = this.AddConfigurationParameter(configParamList, OverrideID.AESKey);
          if (configurationParameter27 != null)
          {
            byte[] data = this.meterMemory.GetData(S4_Params.WMBus_AesKey);
            configurationParameter27.ParameterValue = (object) AES.AesKeyToString(data);
          }
        }
        DateTime? fromFirmwareDateBcd = FirmwareDateTimeSupport.ToDateTimeFromFirmwareDateBCD(this.meterMemory.GetParameterValue<uint>(S4_Params.BatterieEnd_Date));
        if (fromFirmwareDateBcd.HasValue)
        {
          ConfigurationParameter configurationParameter28 = this.AddConfigurationParameter(configParamList, OverrideID.EndOfBatteryDate);
          if (configurationParameter28 != null)
          {
            configurationParameter28.ParameterValue = (object) fromFirmwareDateBcd;
            configurationParameter28.Format = "d";
          }
        }
        if (this.meterMemory.IsParameterAvailable(S4_Params.BatteryPreWarningMonth))
        {
          sbyte parameterValue = this.meterMemory.GetParameterValue<sbyte>(S4_Params.BatteryPreWarningMonth);
          ConfigurationParameter configurationParameter29 = this.AddConfigurationParameter(configParamList, OverrideID.BatteryPreWarningMonths);
          if (configurationParameter29 != null)
          {
            configurationParameter29.ParameterValue = (object) (int) parameterValue;
            configurationParameter29.MinParameterValue = (object) -24;
            configurationParameter29.MaxParameterValue = (object) 24;
          }
        }
        if (this.meterMemory.IsParameterAvailable(S4_Params.BatteryDurabilityMonth))
        {
          byte parameterValue = this.meterMemory.GetParameterValue<byte>(S4_Params.BatteryDurabilityMonth);
          ConfigurationParameter configurationParameter30 = this.AddConfigurationParameter(configParamList, OverrideID.BatteryDurabilityMonths);
          if (configurationParameter30 != null)
          {
            configurationParameter30.ParameterValue = (object) (uint) parameterValue;
            configurationParameter30.MinParameterValue = (object) 0U;
            configurationParameter30.MaxParameterValue = (object) (uint) byte.MaxValue;
          }
        }
        double? ParameterValue = new double?();
        if (this.meterMemory.IsParameterAvailable(S4_Params.battery_capicity))
        {
          DateTime deviceTime = !this.meterMemory.IsParameterAvailable(S4_Params.sysDateTime) ? DateTime.Now : FirmwareDateTimeSupport.ToDateTimeOffsetBCD(this.meterMemory.GetData(S4_Params.sysDateTime)).DateTime;
          ParameterValue = new double?((double) this.meterMemory.GetParameterValue<uint>(S4_Params.battery_capicity));
          this.AddConfigurationParameter(configParamList, OverrideID.BatteryCapacity_mAh, (object) ParameterValue);
          if (ParameterValue.HasValue && fromFirmwareDateBcd.HasValue && this.meterMemory.IsParameterAvailable(S4_Params.ConfigFlagRegister))
          {
            TdcChannelConfig tdcChannelConfig = new TdcChannelConfig(this.meterMemory);
            uint? hardwareId = this.deviceIdentification.HardwareID;
            nullable1 = hardwareId.HasValue ? new uint?(hardwareId.GetValueOrDefault() & 4U) : new uint?();
            uint num5 = 0;
            this.BatteryManager = (int) nullable1.GetValueOrDefault() == (int) num5 & nullable1.HasValue ? new S4_BatteryEnergyManagement(deviceTime, fromFirmwareDateBcd.Value, ParameterValue.Value, tdcChannelConfig, (ushort) 0, (ushort) 0, false) : new S4_BatteryEnergyManagement(deviceTime, fromFirmwareDateBcd.Value, ParameterValue.Value, tdcChannelConfig, num4, communicationScenario2, false);
          }
        }
        if (this.BatteryManager != null)
        {
          ConfigurationParameter configurationParameter31 = this.AddConfigurationParameter(configParamList, OverrideID.MaxEndOfBatteryDate);
          if (configurationParameter31 != null)
          {
            configurationParameter31.ParameterValue = (object) this.BatteryManager.MaxEndOfBatteryDate;
            configurationParameter31.HasWritePermission = false;
            configurationParameter31.Format = "d";
          }
        }
        bool flag1 = new FirmwareVersion(this.meterMemory.FirmwareVersion) > (object) "0.3.7 IUW";
        if (allParameters || ConfigurationParameter.ActiveConfigurationLevel == ConfigurationLevel.Standard)
        {
          ConfigurationParameter configurationParameter32 = this.AddConfigurationParameter(configParamList, OverrideID.DeviceUnit);
          if (configurationParameter32 != null)
          {
            configurationParameter32.ParameterValue = (object) s4MenuManager.GetBaseUnitString();
            configurationParameter32.AllowedValues = S4_MenuManager.AllBaseUnits;
          }
        }
        if (allParameters || ConfigurationParameter.ActiveConfigurationLevel != ConfigurationLevel.Standard)
        {
          ConfigurationParameter configurationParameter33 = this.AddConfigurationParameter(configParamList, OverrideID.VolumeResolution);
          if (configurationParameter33 != null)
          {
            configurationParameter33.ParameterValue = (object) resolution;
            configurationParameter33.AllowedValues = S4_MenuManager.EnsureValueInList(resolution, S4_MenuManager.AllMenuResolutions);
            configurationParameter33.HasWritePermission = flag1;
          }
        }
        ConfigurationParameter configurationParameter34 = this.AddConfigurationParameter(configParamList, OverrideID.DisplayMenu);
        if (configurationParameter34 != null)
        {
          configurationParameter34.ParameterValue = (object) defaultMenuName;
          configurationParameter34.AllowedValues = S4_MenuManager.EnsureValueInList(defaultMenuName, S4_MenuManager.AllMenuNames);
          configurationParameter34.HasWritePermission = flag1;
        }
        if (allParameters || ConfigurationParameter.ActiveConfigurationLevel > ConfigurationLevel.Standard)
        {
          List<ViewSetup> menuSetup = s4MenuManager.GetMenuSetup(menuDefinition);
          bool flag2 = ConfigurationParameter.ActiveConfigurationLevel == ConfigurationLevel.Huge;
          for (int index = 0; index < menuSetup.Count && index < S4_Meter.MenuViewByIndex.Length; ++index)
          {
            ConfigurationParameter configurationParameter35 = this.AddConfigurationParameter(configParamList, S4_Meter.MenuViewByIndex[index]);
            if (configurationParameter35 != null)
            {
              if (configurationParameter35.IsEditable)
                configurationParameter35.IsEditable = flag2;
              configurationParameter35.ParameterValue = (object) menuSetup[index].MainSelectionText;
              if (index == 0)
                configurationParameter35.AllowedValues = new string[1]
                {
                  menuSetup[index].MainSelectionText
                };
              else
                configurationParameter35.AllowedValues = menuSetup[index].MainSelectionsList;
              if (menuSetup[index].MainSelectionText != "Not defined")
              {
                if (menuSetup[index].SecondSelectionText != null)
                {
                  ConfigurationParameter configurationParameter36 = this.AddConfigurationParameter(configParamList, S4_Meter.MenuViewByIndex_L2[index]);
                  if (configurationParameter36 != null)
                  {
                    configurationParameter36.ParameterValue = (object) menuSetup[index].SecondSelectionText;
                    if (index == 0)
                      configurationParameter36.AllowedValues = new string[1]
                      {
                        menuSetup[index].SecondSelectionText
                      };
                    else
                      configurationParameter36.AllowedValues = menuSetup[index].SecondSelectionsList;
                  }
                }
                if (index > 0 && menuSetup[index].TimeoutSelectionText != null)
                {
                  ConfigurationParameter configurationParameter37 = this.AddConfigurationParameter(configParamList, S4_Meter.MenuViewByIndex_Time[index]);
                  if (configurationParameter37 != null)
                  {
                    configurationParameter37.ParameterValue = (object) menuSetup[index].TimeoutSelectionText;
                    configurationParameter37.AllowedValues = menuSetup[index].TimeoutSelectionsList;
                  }
                }
              }
            }
          }
        }
        this.AddVolumeCalibrationParameter(configParamList, OverrideID.CalVolMaxFlowLiterPerHour, (object) double.NaN);
        this.AddVolumeCalibrationParameter(configParamList, OverrideID.CalVolMaxErrorPercent, (object) double.NaN);
        this.AddVolumeCalibrationParameter(configParamList, OverrideID.CalVolNominalFlowLiterPerHour, (object) double.NaN);
        this.AddVolumeCalibrationParameter(configParamList, OverrideID.CalVolNominalErrorPercent, (object) double.NaN);
        this.AddVolumeCalibrationParameter(configParamList, OverrideID.CalVolMinFlowLiterPerHour, (object) double.NaN);
        this.AddVolumeCalibrationParameter(configParamList, OverrideID.CalVolMinErrorPercent, (object) double.NaN);
        if (this.meterMemory.IsParameterAvailable(S4_Params.minimumFlowrateQ1))
        {
          ConfigurationParameter configurationParameter38 = this.AddConfigurationParameter(configParamList, OverrideID.MinimumFlowQ1);
          ConfigurationParameter configurationParameter39 = this.AddConfigurationParameter(configParamList, OverrideID.PermanentFlowQ3);
          if (configurationParameter38 != null && configurationParameter39 != null)
          {
            configurationParameter38.HasWritePermission = !this.IsProtected;
            configurationParameter39.HasWritePermission = !this.IsProtected;
            configurationParameter38.ParameterValue = (object) (double) this.meterMemory.GetParameterValue<float>(S4_Params.minimumFlowrateQ1);
            configurationParameter39.ParameterValue = (object) (double) this.meterMemory.GetParameterValue<float>(S4_Params.permanentFlowrateQ3);
            configurationParameter38.Format = "0.000";
            configurationParameter39.Format = "0.000";
          }
          else if (configurationParameter38 != null || configurationParameter39 != null)
            throw new Exception("Illegal configuration for MinimumFlowQ1 and PermanentFlowQ3");
        }
        if (this.meterMemory.SmartFunctionFlashRange != null && this.MySmartFunctionManager != null)
        {
          ConfigurationParameter configurationParameter40 = this.AddConfigurationParameter(configParamList, OverrideID.SmartFunctions);
          if (configurationParameter40 != null)
          {
            configurationParameter40.ParameterValue = (object) this.MySmartFunctionManager.GetPreparedSmartFunctionNames();
            configurationParameter40.AllowedValues = this.MySmartFunctionManager.GetUsableSmartFunctionNames();
          }
          if (new FirmwareVersion(this.meterMemory.FirmwareVersion) >= (object) "1.7.1 IUW")
          {
            ConfigurationParameter configurationParameter41 = this.AddConfigurationParameter(configParamList, OverrideID.ActiveSmartFunctions);
            if (configurationParameter41 != null)
            {
              configurationParameter41.ParameterValue = (object) this.MySmartFunctionManager.GetPreparedActiveFunctionNames();
              configurationParameter41.AllowedValues = this.MySmartFunctionManager.GetPreparedSmartFunctionNames();
            }
          }
        }
        if (UserManager.CheckPermission(UserManager.Right_ReadOnly))
        {
          foreach (ConfigurationParameter configurationParameter42 in (IEnumerable<ConfigurationParameter>) configParamList.Values)
            configurationParameter42.HasWritePermission = false;
        }
        return configParamList;
      }
      catch (Exception ex)
      {
        throw new Exception(nameof (GetConfigurationParameters), ex);
      }
    }

    private ConfigurationParameter AddVolumeCalibrationParameter(
      SortedList<OverrideID, ConfigurationParameter> configParamList,
      OverrideID overrideID,
      object ParameterValue)
    {
      ConfigurationParameter configurationParameter = this.AddConfigurationParameter(configParamList, overrideID, ParameterValue);
      if (configurationParameter != null && configurationParameter.ParameterInfo.CorrespondetValueIdent == ValueIdent.ValueIdPart_PhysicalQuantity.Flow)
        configurationParameter.Unit = "Liter/h";
      return configurationParameter;
    }

    private ConfigurationParameter AddConfigurationParameter(
      SortedList<OverrideID, ConfigurationParameter> configParamList,
      OverrideID overrideID,
      object ParameterValue = null)
    {
      ConfigurationParameter configurationParameter = this.GetConfigurationParameter(overrideID, ParameterValue);
      if (configurationParameter == null)
        return (ConfigurationParameter) null;
      configParamList.Add(overrideID, configurationParameter);
      return configurationParameter;
    }

    private ConfigurationParameter GetConfigurationParameter(
      OverrideID overrideID,
      object ParameterValue = null)
    {
      if ((ConfigurationParameter.ActiveConfigurationLevel & ConfigurationParameter.ConfigParametersByOverrideID[overrideID].DefaultConfigurationLevels) == (ConfigurationLevel) 0)
        return (ConfigurationParameter) null;
      ConfigurationParameter configurationParameter = new ConfigurationParameter(overrideID);
      if (ParameterValue != null)
        configurationParameter.ParameterValue = ParameterValue;
      if (this.UseRights && UserManager.IsNewLicenseModel())
      {
        if (UserManager.IsConfigParamVisible(configurationParameter.ParameterID))
        {
          configurationParameter.IsEditable = true;
          configurationParameter.HasWritePermission = UserManager.IsConfigParamEditable(configurationParameter.ParameterID);
        }
        else
        {
          configurationParameter.IsEditable = false;
          configurationParameter.HasWritePermission = false;
        }
      }
      else
      {
        configurationParameter.IsEditable = true;
        configurationParameter.HasWritePermission = true;
      }
      return configurationParameter;
    }

    internal void SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameterList)
    {
      ConfigurationParameterListSupport confListSupport = new ConfigurationParameterListSupport(parameterList);
      SortedList<OverrideID, ConfigurationParameter> configurationParameters = this.GetConfigurationParameters("Reload for undefined set parameters", true);
      ConfigurationParameterListSupport parameterListSupport = new ConfigurationParameterListSupport(configurationParameters);
      ConfigurationParameter parameterFromList1 = confListSupport.GetWorkParameterFromList(OverrideID.SetTimeForTimeZoneFromPcTime);
      ConfigurationParameter parameterFromList2 = confListSupport.GetWorkParameterFromList(OverrideID.SetPcTime);
      ConfigurationParameter parameterFromList3 = confListSupport.GetWorkParameterFromList(OverrideID.DeviceClock);
      if (parameterFromList1 != null)
        this.SetTimeForTimeZoneFromPcTime = (bool) parameterFromList1.ParameterValue;
      else if (parameterFromList2 != null)
        this.SetPcTimeOnWrite = (bool) parameterFromList2.ParameterValue;
      else if (parameterFromList3 != null)
      {
        this.meterMemory.SetData(S4_Params.sysDateTime, FirmwareDateTimeSupport.ToFirmwareDateTimeBCD((DateTime) parameterFromList3.ParameterValue));
        this.SetTimeOnWrite = true;
      }
      if (this.SetTimeForTimeZoneFromPcTime)
        this.SetPcTimeOnWrite = false;
      ConfigurationParameter parameterFromList4 = confListSupport.GetWorkParameterFromList(OverrideID.TimeZone);
      if (parameterFromList4 != null)
        FirmwareDateTimeSupport.SetTimeZoneToMemory((Decimal) parameterFromList4.ParameterValue, (DeviceMemory) this.meterMemory);
      ConfigurationParameter parameterFromList5 = confListSupport.GetWorkParameterFromList(OverrideID.DueDate);
      DateTime now;
      if (parameterFromList5 != null && this.meterMemory.IsParameterInMap(S4_Params.Key_Date))
      {
        DateTime dateTime = (DateTime) parameterFromList5.ParameterValue;
        ref DateTime local = ref dateTime;
        now = DateTime.Now;
        int year = now.Year;
        int month = dateTime.Month;
        int day = dateTime.Day;
        local = new DateTime(year, month, day);
        if (dateTime < DateTime.Now)
          dateTime = dateTime.AddYears(1);
        this.meterMemory.SetParameterValue<uint>(S4_Params.Key_Date, FirmwareDateTimeSupport.ToFirmwareDateBCD(dateTime));
      }
      ConfigurationParameter parameterFromList6 = confListSupport.GetWorkParameterFromList(OverrideID.EndOfCalibration);
      if (parameterFromList6 != null && this.meterMemory.IsParameterAvailable(S4_Params.ApprovalEnd_Date))
        this.meterMemory.SetParameterValue<uint>(S4_Params.ApprovalEnd_Date, FirmwareDateTimeSupport.ToFirmwareDateBCD(new DateTime?((DateTime) parameterFromList6.ParameterValue).Value));
      ushort num1 = 0;
      ushort num2 = 0;
      if (this.meterMemory.IsParameterAvailable(S4_Params.com_scenario_intern))
      {
        ConfigurationParameter parameterFromList7 = confListSupport.GetWorkParameterFromList(OverrideID.CommunicationScenario);
        if (parameterFromList7 != null)
        {
          num1 = (ushort) (int) parameterFromList7.ParameterValue;
          this.meterMemory.SetParameterValue<ushort>(S4_Params.com_scenario_intern, num1);
          this.meterMemory.SetParameterValue<ushort>(S4_Params.com_scenario_extern, num1);
          this.ScenariosRequired = true;
        }
        else
          num1 = this.meterMemory.GetParameterValue<ushort>(S4_Params.com_scenario_intern);
      }
      else if (this.meterMemory.IsParameterAvailable(S4_Params.com_scenario_intern_lora))
      {
        ConfigurationParameter parameterFromList8 = confListSupport.GetWorkParameterFromList(OverrideID.CommunicationScenarioLoRa);
        if (parameterFromList8 != null)
        {
          num1 = (ushort) (int) parameterFromList8.ParameterValue;
          this.meterMemory.SetParameterValue<ushort>(S4_Params.com_scenario_intern_lora, num1);
          this.meterMemory.SetParameterValue<ushort>(S4_Params.com_scenario_extern_lora, num1);
          this.ScenariosRequired = true;
        }
        else
          num1 = this.meterMemory.GetParameterValue<ushort>(S4_Params.com_scenario_intern_lora);
        ConfigurationParameter parameterFromList9 = confListSupport.GetWorkParameterFromList(OverrideID.CommunicationScenarioWmbus);
        if (parameterFromList9 != null)
        {
          num2 = (ushort) (int) parameterFromList9.ParameterValue;
          this.meterMemory.SetParameterValue<ushort>(S4_Params.com_scenario_intern_wmbus, num2);
          this.meterMemory.SetParameterValue<ushort>(S4_Params.com_scenario_extern_wmbus, num2);
          this.ScenariosRequired = true;
        }
        else
          num2 = this.meterMemory.GetParameterValue<ushort>(S4_Params.com_scenario_intern_lora);
      }
      ConfigurationParameter parameterFromList10 = confListSupport.GetWorkParameterFromList(OverrideID.DevEUI);
      if (parameterFromList10 != null)
        this.meterMemory.SetParameterValue<ulong>(S4_Params.LoRa_DevEUI, (ulong) parameterFromList10.ParameterValue);
      ConfigurationParameter parameterFromList11 = confListSupport.GetWorkParameterFromList(OverrideID.AppKey);
      if (parameterFromList11 != null)
        this.meterMemory.SetData(S4_Params.LoRa_AppKey, (byte[]) parameterFromList11.ParameterValue);
      ConfigurationParameter parameterFromList12 = confListSupport.GetWorkParameterFromList(OverrideID.JoinEUI);
      if (parameterFromList12 != null)
        this.meterMemory.SetParameterValue<ulong>(S4_Params.LoRa_AppEUI, (ulong) parameterFromList12.ParameterValue);
      ConfigurationParameter parameterFromList13 = confListSupport.GetWorkParameterFromList(OverrideID.ADR);
      if (this.meterMemory.IsParameterAvailable(S4_Params.cfg_lora_adr_on) && parameterFromList13 != null)
      {
        if ((bool) parameterFromList13.ParameterValue)
          this.meterMemory.SetParameterValue<byte>(S4_Params.cfg_lora_adr_on, (byte) 1);
        else
          this.meterMemory.SetParameterValue<byte>(S4_Params.cfg_lora_adr_on, (byte) 0);
      }
      ConfigurationParameter parameterFromList14 = confListSupport.GetWorkParameterFromList(OverrideID.SendJoinRequest);
      if (parameterFromList14 != null)
        this.JoinRequestOnWrite = (bool) parameterFromList14.ParameterValue;
      ConfigurationParameter parameterFromList15 = confListSupport.GetWorkParameterFromList(OverrideID.AESKey);
      if (parameterFromList15 != null && this.meterMemory.IsParameterAvailable(S4_Params.WMBus_AesKey))
        this.meterMemory.SetData(S4_Params.WMBus_AesKey, AES.StringToAesKey((string) parameterFromList15.ParameterValue) ?? new byte[16]);
      ConfigurationParameter parameterFromList16 = confListSupport.GetWorkParameterFromList(OverrideID.EndOfBatteryDate);
      DateTime? nullable1;
      if (parameterFromList16 != null)
      {
        nullable1 = new DateTime?((DateTime) parameterFromList16.ParameterValue);
        this.meterMemory.SetParameterValue<uint>(S4_Params.BatterieEnd_Date, FirmwareDateTimeSupport.ToFirmwareDateBCD(nullable1.Value));
      }
      else
        nullable1 = FirmwareDateTimeSupport.ToDateTimeFromFirmwareDateBCD(this.meterMemory.GetParameterValue<uint>(S4_Params.BatterieEnd_Date));
      ConfigurationParameter parameterFromList17 = confListSupport.GetWorkParameterFromList(OverrideID.BatteryPreWarningMonths);
      if (parameterFromList17 != null && this.meterMemory.IsParameterAvailable(S4_Params.BatteryPreWarningMonth))
      {
        int parameterValue = (int) parameterFromList17.ParameterValue;
        if (parameterValue > 24 || parameterValue < -24)
          throw new Exception("BatteryPreWarningMonth range: -24 .. +24");
        this.meterMemory.SetParameterValue<byte>(S4_Params.BatteryPreWarningMonth, (byte) parameterValue);
      }
      ConfigurationParameter parameterFromList18 = confListSupport.GetWorkParameterFromList(OverrideID.BatteryDurabilityMonths);
      if (parameterFromList18 != null && this.meterMemory.IsParameterAvailable(S4_Params.BatteryDurabilityMonth))
      {
        uint parameterValue = (uint) parameterFromList18.ParameterValue;
        if (parameterValue > (uint) byte.MaxValue || parameterValue < 0U)
          throw new Exception("BatteryDurabilityMonths range: 0 .. 255");
        this.meterMemory.SetParameterValue<byte>(S4_Params.BatteryDurabilityMonth, (byte) parameterValue);
      }
      if (this.meterMemory.IsParameterAvailable(S4_Params.BatteryPreWarningMonth) && nullable1.HasValue)
      {
        int parameterValue = (int) this.meterMemory.GetParameterValue<sbyte>(S4_Params.BatteryPreWarningMonth);
        now = nullable1.Value;
        this.meterMemory.SetParameterValue<uint>(S4_Params.BatterieWarn_Date, FirmwareDateTimeSupport.ToFirmwareDateBCD(now.AddMonths(parameterValue * -1)));
      }
      ConfigurationParameter parameterFromList19 = confListSupport.GetWorkParameterFromList(OverrideID.BatteryCapacity_mAh);
      double? nullable2 = new double?();
      if (parameterFromList19 != null)
      {
        nullable2 = new double?((double) parameterFromList19.ParameterValue);
        if (this.meterMemory.IsParameterAvailable(S4_Params.battery_capicity))
          this.meterMemory.SetParameterValue<uint>(S4_Params.battery_capicity, (uint) nullable2.Value);
      }
      else if (this.meterMemory.IsParameterAvailable(S4_Params.battery_capicity))
      {
        uint parameterValue = this.meterMemory.GetParameterValue<uint>(S4_Params.battery_capicity);
        if (parameterValue > 0U)
          nullable2 = new double?((double) parameterValue);
      }
      uint? nullable3;
      if (nullable2.HasValue && nullable1.HasValue)
      {
        DateTime deviceTime;
        if (this.meterMemory.IsParameterAvailable(S4_Params.sysDateTime))
        {
          deviceTime = FirmwareDateTimeSupport.ToDateTimeOffsetBCD(this.meterMemory.GetData(S4_Params.sysDateTime)).DateTime;
          if (deviceTime < DateTime.Now)
            deviceTime = DateTime.Now;
        }
        else
          deviceTime = DateTime.Now;
        TdcChannelConfig tdcChannelConfig = new TdcChannelConfig(this.meterMemory);
        uint? hardwareId = this.deviceIdentification.HardwareID;
        nullable3 = hardwareId.HasValue ? new uint?(hardwareId.GetValueOrDefault() & 4U) : new uint?();
        uint num3 = 0;
        this.BatteryManager = (int) nullable3.GetValueOrDefault() == (int) num3 & nullable3.HasValue ? new S4_BatteryEnergyManagement(deviceTime, nullable1.Value, nullable2.Value, tdcChannelConfig, (ushort) 0, (ushort) 0, true) : new S4_BatteryEnergyManagement(deviceTime, nullable1.Value, nullable2.Value, tdcChannelConfig, num1, num2, true);
      }
      string baseUnitString = (string) null;
      ConfigurationParameter parameterFromList20 = confListSupport.GetWorkParameterFromList(OverrideID.DeviceUnit);
      if (parameterFromList20 != null)
        baseUnitString = (string) parameterFromList20.ParameterValue;
      string resolution1 = (string) null;
      ConfigurationParameter parameterFromList21 = confListSupport.GetWorkParameterFromList(OverrideID.VolumeResolution);
      if (parameterFromList21 != null)
        resolution1 = (string) parameterFromList21.ParameterValue;
      S4_MenuManager s4MenuManager = new S4_MenuManager(this.meterMemory);
      string menuName = (string) null;
      ConfigurationParameter parameterFromList22 = confListSupport.GetWorkParameterFromList(OverrideID.DisplayMenu);
      if (parameterFromList22 != null)
        menuName = (string) parameterFromList22.ParameterValue;
      if (menuName == "Standard")
      {
        if (resolution1 == null)
          throw new Exception("MenuName = 'Standard' without resolution is not possible");
        switch (S4_MenuManager.GetBaseUnit(S4_MenuManager.GetBaseUnitString(resolution1)))
        {
          case S4_BaseUnits.Qm:
            menuName = S4_SupportedMenues.Europe.ToString();
            break;
          case S4_BaseUnits.US_GAL:
          case S4_BaseUnits.Qft:
            menuName = S4_SupportedMenues.USA.ToString();
            break;
        }
      }
      string resolution2 = (string) null;
      bool flag = false;
      if (baseUnitString != null || resolution1 != null || menuName != null)
      {
        if (menuName == null)
        {
          menuName = SupportedMenu.GetDefaultMenuName(s4MenuManager.GetMenuDefinition());
          if (menuName == "Customized menu")
            menuName = (string) null;
        }
        if (menuName != null)
        {
          resolution2 = s4MenuManager.SetMenuSetup(baseUnitString, resolution1, menuName);
          flag = true;
        }
      }
      if (!flag)
      {
        List<ViewSetup> vSet = new List<ViewSetup>();
        for (int viewIndexInMenu = 0; viewIndexInMenu < S4_Meter.MenuViewByIndex.Length; ++viewIndexInMenu)
        {
          string mainSelection = (string) null;
          ConfigurationParameter parameterFromList23 = confListSupport.GetWorkParameterFromList(S4_Meter.MenuViewByIndex[viewIndexInMenu]);
          if (parameterFromList23 != null)
          {
            mainSelection = (string) parameterFromList23.ParameterValue;
          }
          else
          {
            ConfigurationParameter parameterFromList24 = parameterListSupport.GetWorkParameterFromList(S4_Meter.MenuViewByIndex[viewIndexInMenu]);
            if (parameterFromList24 != null)
              mainSelection = (string) parameterFromList24.ParameterValue;
          }
          string secondSelection = (string) null;
          ConfigurationParameter parameterFromList25 = confListSupport.GetWorkParameterFromList(S4_Meter.MenuViewByIndex_L2[viewIndexInMenu]);
          if (parameterFromList25 != null)
          {
            secondSelection = (string) parameterFromList25.ParameterValue;
          }
          else
          {
            ConfigurationParameter parameterFromList26 = parameterListSupport.GetWorkParameterFromList(S4_Meter.MenuViewByIndex_L2[viewIndexInMenu]);
            if (parameterFromList26 != null)
              secondSelection = (string) parameterFromList26.ParameterValue;
          }
          string timeoutSelection = (string) null;
          ConfigurationParameter parameterFromList27 = confListSupport.GetWorkParameterFromList(S4_Meter.MenuViewByIndex_Time[viewIndexInMenu]);
          if (parameterFromList27 != null)
          {
            timeoutSelection = (string) parameterFromList27.ParameterValue;
          }
          else
          {
            ConfigurationParameter parameterFromList28 = parameterListSupport.GetWorkParameterFromList(S4_Meter.MenuViewByIndex_Time[viewIndexInMenu]);
            if (parameterFromList28 != null)
              timeoutSelection = (string) parameterFromList28.ParameterValue;
          }
          vSet.Add(new ViewSetup(this.meterMemory.FirmwareVersion, viewIndexInMenu, mainSelection, secondSelection, timeoutSelection, s4MenuManager.DefaultUnitScale, menuName));
        }
        resolution2 = s4MenuManager.SetMenuSetup(resolution1, baseUnitString, vSet);
      }
      ConfigurationParameter parameterFromList29 = confListSupport.GetWorkParameterFromList(OverrideID.VolumeActualValue);
      if (parameterFromList29 != null)
      {
        S4_BaseUnits s4BaseUnits = resolution2 == null ? s4MenuManager.GetBaseUnit() : S4_MenuManager.GetBaseUnit(S4_MenuManager.GetBaseUnitString(resolution2));
        double num4 = (double) (Decimal) parameterFromList29.ParameterValue;
        switch (s4BaseUnits)
        {
          case S4_BaseUnits.Qm:
            this.meterMemory.SetParameterValue<double>(S4_Params.volQmSum, num4);
            if (num4 >= 0.0)
            {
              this.meterMemory.SetParameterValue<double>(S4_Params.volQmPos, num4);
              this.meterMemory.SetParameterValue<double>(S4_Params.volQmNeg, 0.0);
            }
            else
            {
              this.meterMemory.SetParameterValue<double>(S4_Params.volQmPos, 0.0);
              this.meterMemory.SetParameterValue<double>(S4_Params.volQmNeg, num4);
            }
            this.InitMeterData();
            break;
          case S4_BaseUnits.US_GAL:
            num4 = S4_BaseUnitSupport.US_GALToQm(num4);
            goto case S4_BaseUnits.Qm;
          case S4_BaseUnits.Qft:
            num4 = S4_BaseUnitSupport.QfToQm(num4);
            goto case S4_BaseUnits.Qm;
          default:
            throw new Exception("Base unit for set volume not available.");
        }
      }
      VolumeCalibrationValues calibrationValues = new VolumeCalibrationValues(confListSupport);
      if (calibrationValues.CalibrationFactors.Count > 0)
      {
        S4_TDC_Calibration s4TdcCalibration = new S4_TDC_Calibration();
        s4TdcCalibration.LoadCalibrationFromMemory(this.meterMemory);
        s4TdcCalibration.AdjustCalibration(calibrationValues.CalibrationFactors);
        s4TdcCalibration.CopyCalibrationToMemory(this.meterMemory);
      }
      ConfigurationParameter configurationParameter1 = confListSupport.GetWorkParameterFromList(OverrideID.MinimumFlowQ1);
      ConfigurationParameter configurationParameter2 = confListSupport.GetWorkParameterFromList(OverrideID.PermanentFlowQ3);
      if (this.meterMemory.IsParameterAvailable(S4_Params.minimumFlowrateQ1) && (configurationParameter1 != null || configurationParameter2 != null))
      {
        if (configurationParameter1 == null)
          configurationParameter1 = configurationParameters.ContainsKey(OverrideID.MinimumFlowQ1) ? configurationParameters[OverrideID.PermanentFlowQ3] : throw new Exception("MinimumFlowQ1 not defined");
        if (configurationParameter2 == null)
          configurationParameter2 = configurationParameters.ContainsKey(OverrideID.PermanentFlowQ3) ? configurationParameters[OverrideID.PermanentFlowQ3] : throw new Exception("PermanentFlowQ3 not defined");
        double parameterValue1 = (double) configurationParameter1.ParameterValue;
        double parameterValue2 = (double) configurationParameter2.ParameterValue;
        if (parameterValue1 >= parameterValue2)
          throw new Exception("MinimumFlowQ1 >= PermanentFlowQ3");
        float parameterValue3 = this.meterMemory.GetParameterValue<float>(S4_Params.minPositivFlow);
        float parameterValue4 = this.meterMemory.GetParameterValue<float>(S4_Params.maxPositivFlow);
        string str1 = "minPositivFlow = " + parameterValue3.ToString() + Environment.NewLine + "maxPositivFlow = " + parameterValue4.ToString() + Environment.NewLine + Environment.NewLine;
        if (parameterValue1 < (double) parameterValue3)
          throw new Exception(str1 + "MinimumFlowQ1 < minPositivFlow");
        if (parameterValue2 > (double) parameterValue4)
          throw new Exception(str1 + "PermanentFlowQ3 > maxPositivFlow");
        double theValue1 = parameterValue1 * 1.6;
        double theValue2 = parameterValue2 * 1.25;
        string str2 = str1 + "flowQ2 = " + theValue1.ToString() + Environment.NewLine + "flowQ4 = " + theValue2.ToString() + Environment.NewLine + Environment.NewLine;
        if (theValue1 >= parameterValue2)
          throw new Exception(str2 + "flowQ2 >= PermanentFlowQ3");
        if (theValue2 > (double) parameterValue4)
          throw new Exception(str2 + "flowQ4 > maxPositivFlow");
        this.meterMemory.SetParameterValue<float>(S4_Params.minimumFlowrateQ1, (float) parameterValue1);
        this.meterMemory.SetParameterValue<float>(S4_Params.transitionalFlowrateQ2, (float) theValue1);
        this.meterMemory.SetParameterValue<float>(S4_Params.permanentFlowrateQ3, (float) parameterValue2);
        this.meterMemory.SetParameterValue<float>(S4_Params.overloadFlowrateQ4, (float) theValue2);
      }
      if (this.meterMemory.SmartFunctionFlashRange != null)
      {
        ConfigurationParameter parameterFromList30 = confListSupport.GetWorkParameterFromList(OverrideID.SmartFunctionGroup);
        if (parameterFromList30 != null)
        {
          if (this.MySmartFunctionManager == null)
            this.MySmartFunctionManager = new S4_SmartFunctionManager(this.meterMemory);
          string[] strArray = S4_SmartFunctionManager.GetSmartFunctionNamesOfGroup((string) parameterFromList29.ParameterValue);
          nullable3 = this.deviceIdentification.FirmwareVersion;
          if (new FirmwareVersion(nullable3.Value) < (object) "1.7.1 IUW" && ((IEnumerable<string>) strArray).Contains<string>("HourlyVolumeLog"))
            strArray = ((IEnumerable<string>) strArray).Where<string>((Func<string, bool>) (val => val != "HourlyVolumeLog")).ToArray<string>();
          this.MySmartFunctionManager.PrepareRequiredFunctions(strArray);
        }
        ConfigurationParameter parameterFromList31 = confListSupport.GetWorkParameterFromList(OverrideID.SmartFunctions);
        if (parameterFromList31 != null)
        {
          if (parameterFromList30 != null)
            throw new Exception("If a SmartFunction group is defined, than a list of smart functions is not allowed");
          if (this.MySmartFunctionManager == null)
            this.MySmartFunctionManager = new S4_SmartFunctionManager(this.meterMemory);
          string[] strArray = (string[]) parameterFromList31.ParameterValue;
          nullable3 = this.deviceIdentification.FirmwareVersion;
          if (new FirmwareVersion(nullable3.Value) < (object) "1.7.1 IUW" && ((IEnumerable<string>) strArray).Contains<string>("HourlyVolumeLog"))
            strArray = ((IEnumerable<string>) strArray).Where<string>((Func<string, bool>) (val => val != "HourlyVolumeLog")).ToArray<string>();
          this.MySmartFunctionManager.PrepareRequiredFunctions(strArray);
        }
        ConfigurationParameter parameterFromList32 = confListSupport.GetWorkParameterFromList(OverrideID.ActiveSmartFunctions);
        if (parameterFromList32 != null && this.MySmartFunctionManager != null)
          this.MySmartFunctionManager.PrepareActiveFunctions((string[]) parameterFromList32.ParameterValue);
      }
      ConfigurationParameter parameterFromList33 = confListSupport.GetWorkParameterFromList(OverrideID.SmartFunctionConfig);
      if (parameterFromList33 != null)
      {
        if (this.MySmartFunctionManager == null)
          this.MySmartFunctionManager = new S4_SmartFunctionManager(this.meterMemory);
        string[] parameterValue = (string[]) parameterFromList33.ParameterValue;
        this.UpdateDeviceCharacteristics();
        this.MySmartFunctionManager.PrepareParameterOverwrite(this.DeviceCharacteristics, parameterValue);
      }
      confListSupport.CheckAllParametersWorked();
    }
  }
}
