// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_Meter
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using DeviceCollector;
using GmmDbLib;
using HandlerLib;
using NLog;
using StartupLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class S3_Meter
  {
    internal S3_BatteryEnergyManagement BatteryManager = (S3_BatteryEnergyManagement) null;
    internal static string[] inputMBusIdentNumber = new string[3]
    {
      "SerDev1_IdentNo",
      "SerDev2_IdentNo",
      "SerDev3_IdentNo"
    };
    internal static string[] inputMBusManufacturer = new string[3]
    {
      "SerDev1_Manufacturer",
      "SerDev2_Manufacturer",
      "SerDev3_Manufacturer"
    };
    internal static string[] inputMBusMediumGeneration = new string[3]
    {
      "SerDev1_Medium_Generation",
      "SerDev2_Medium_Generation",
      "SerDev3_Medium_Generation"
    };
    internal static string[] inputMBusSelectedListAndAddress = new string[3]
    {
      "SerDev1_SelectedList_PrimaryAddress",
      "SerDev2_SelectedList_PrimaryAddress",
      "SerDev3_SelectedList_PrimaryAddress"
    };
    internal static string[] inputFactor = new string[3]
    {
      "Cal_FaktorInput_n_0",
      "Cal_FaktorInput_n_1",
      "Cal_FaktorInput_n_2"
    };
    internal static string[] inputDevisor = new string[3]
    {
      "Cal_TeilerFaktorInput_n_0",
      "Cal_TeilerFaktorInput_n_1",
      "Cal_TeilerFaktorInput_n_2"
    };
    internal static string[] inputUnitIndex = new string[3]
    {
      "input0UnitIndex",
      "input1UnitIndex",
      "input2UnitIndex"
    };
    internal static string[] inputActualValue = new string[3]
    {
      "Cal_DisplayInput_n_0",
      "Cal_DisplayInput_n_1",
      "Cal_DisplayInput_n_2"
    };
    internal static string[] glycolSettings = new string[16]
    {
      "Ethylen_0",
      "Ethylen_20",
      "Ethylen_25",
      "Ethylen_30",
      "Ethylen_35",
      "Ethylen_40",
      "Ethylen_45",
      "Ethylen_50",
      "Propylen_20",
      "Propylen_25",
      "Propylen_30",
      "Propylen_35",
      "Propylen_40",
      "Propylen_45",
      "Propylen_50",
      "off"
    };
    private SortedList<OverrideID, ConfigurationParameter> currentParameterList;
    private StringBuilder printText;
    private int LineStartOffset;
    internal static Logger S3_MeterLogger = LogManager.GetLogger(nameof (S3_Meter));
    internal S3_HandlerFunctions MyFunctions;
    internal MeterScaling MyMeterScaling;
    internal DeviceMemory MyDeviceMemory;
    internal ParameterList MyParameters;
    internal S3_DeviceIdentification MyIdentification;
    internal TransmitParameterManager MyTransmitParameterManager;
    internal LoggerManager MyLoggerManager;
    internal FunctionManager MyFunctionManager;
    internal WriteProtTableManager MyWriteProtTableManager;
    internal BackupRuntimeVarsManager MyBackupRuntimeVarsManager;
    internal HeapManager MyHeapManager;
    internal S3_Linker MyLinker;
    internal MeterResources MyResources;
    internal CalibrationValues calibrationValues;
    internal double? TestVolumeSimulationValue;
    internal bool IsWriteProtected;
    internal DateTime MeterObjectGeneratedTimeStamp;
    internal string HardwareTypeDescription;
    internal string MeterInfoDescription;
    internal string MeterTypeDescription;
    internal string TypeOverrideString;
    internal string TypeCreationStringReplaced;
    internal string TypeCreationString;
    internal string SavedOrderNumberString;
    internal string CloneInfo;
    internal string CloneFlowName;
    internal int CloneIndex;
    internal bool resetAllValues;
    internal bool clearAllLogger;
    internal bool setMbusPrimAdrFromSerialNumber;
    internal List<KeyValuePair<string, int>> theMap;
    internal List<OverwriteHistoryItem> overwriteHistorie = new List<OverwriteHistoryItem>();
    private Random theRandom;

    internal GlobalDeviceId GetGlobalDeviceIds()
    {
      GlobalDeviceId globalDeviceIds = new GlobalDeviceId();
      globalDeviceIds.DeviceTypeName = "ZR_Serie3";
      globalDeviceIds.FirmwareVersion = this.MyIdentification.FirmwareVersionString;
      uint uintValue1 = this.MyParameters.ParameterByName["SerDev0_IdentNo"].GetUintValue();
      globalDeviceIds.Serialnumber = uintValue1.ToString("x08");
      short shortValue1 = this.MyParameters.ParameterByName["SerDev0_Manufacturer"].GetShortValue();
      globalDeviceIds.Manufacturer = MBusDevice.GetManufacturer(shortValue1);
      ushort ushortValue1 = this.MyParameters.ParameterByName["SerDev0_Medium_Generation"].GetUshortValue();
      globalDeviceIds.MeterType = (ValueIdent.ValueIdPart_MeterType) ((int) ushortValue1 >> 8);
      globalDeviceIds.Generation = ((int) ushortValue1 & (int) byte.MaxValue).ToString();
      ushort ushortValue2 = this.MyParameters.ParameterByName["SerDev0_SelectedList_PrimaryAddress"].GetUshortValue();
      globalDeviceIds.MeterNumber = ((int) ushortValue2 & (int) byte.MaxValue).ToString();
      globalDeviceIds.DeviceDetails = this.MyIdentification.GetDeviceIdInfoForConfigurator();
      if (this.MyIdentification.IsInput1Available)
      {
        GlobalDeviceId globalDeviceId = new GlobalDeviceId();
        globalDeviceIds.SubDevices.Add(globalDeviceId);
        uint uintValue2 = this.MyParameters.ParameterByName["SerDev1_IdentNo"].GetUintValue();
        globalDeviceId.Serialnumber = uintValue2.ToString("x08");
        short shortValue2 = this.MyParameters.ParameterByName["SerDev1_Manufacturer"].GetShortValue();
        globalDeviceId.Manufacturer = MBusDevice.GetManufacturer(shortValue2);
        ushort ushortValue3 = this.MyParameters.ParameterByName["SerDev1_Medium_Generation"].GetUshortValue();
        globalDeviceId.MeterType = (ValueIdent.ValueIdPart_MeterType) ((int) ushortValue3 >> 8);
        globalDeviceId.Generation = ((int) ushortValue3 & (int) byte.MaxValue).ToString();
        ushort ushortValue4 = this.MyParameters.ParameterByName["SerDev1_SelectedList_PrimaryAddress"].GetUshortValue();
        globalDeviceId.MeterNumber = ((int) ushortValue4 & (int) byte.MaxValue).ToString();
      }
      if (this.MyIdentification.IsInput2Available)
      {
        GlobalDeviceId globalDeviceId = new GlobalDeviceId();
        globalDeviceIds.SubDevices.Add(globalDeviceId);
        uint uintValue3 = this.MyParameters.ParameterByName["SerDev2_IdentNo"].GetUintValue();
        globalDeviceId.Serialnumber = uintValue3.ToString("x08");
        short shortValue3 = this.MyParameters.ParameterByName["SerDev2_Manufacturer"].GetShortValue();
        globalDeviceId.Manufacturer = MBusDevice.GetManufacturer(shortValue3);
        ushort ushortValue5 = this.MyParameters.ParameterByName["SerDev2_Medium_Generation"].GetUshortValue();
        globalDeviceId.MeterType = (ValueIdent.ValueIdPart_MeterType) ((int) ushortValue5 >> 8);
        globalDeviceId.Generation = ((int) ushortValue5 & (int) byte.MaxValue).ToString();
        ushort ushortValue6 = this.MyParameters.ParameterByName["SerDev2_SelectedList_PrimaryAddress"].GetUshortValue();
        globalDeviceId.MeterNumber = ((int) ushortValue6 & (int) byte.MaxValue).ToString();
      }
      if (this.MyIdentification.IsInput3Available)
      {
        GlobalDeviceId globalDeviceId = new GlobalDeviceId();
        globalDeviceIds.SubDevices.Add(globalDeviceId);
        uint uintValue4 = this.MyParameters.ParameterByName["SerDev3_IdentNo"].GetUintValue();
        globalDeviceId.Serialnumber = uintValue4.ToString("x08");
        short shortValue4 = this.MyParameters.ParameterByName["SerDev3_Manufacturer"].GetShortValue();
        globalDeviceId.Manufacturer = MBusDevice.GetManufacturer(shortValue4);
        ushort ushortValue7 = this.MyParameters.ParameterByName["SerDev3_Medium_Generation"].GetUshortValue();
        globalDeviceId.MeterType = (ValueIdent.ValueIdPart_MeterType) ((int) ushortValue7 >> 8);
        globalDeviceId.Generation = ((int) ushortValue7 & (int) byte.MaxValue).ToString();
        ushort ushortValue8 = this.MyParameters.ParameterByName["SerDev3_SelectedList_PrimaryAddress"].GetUshortValue();
        globalDeviceId.MeterNumber = ((int) ushortValue8 & (int) byte.MaxValue).ToString();
      }
      return globalDeviceIds;
    }

    internal SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      int SubDevice,
      SortedList<OverrideID, ConfigurationParameter> baseTypeParameterList,
      bool useRights)
    {
      if (SubDevice < 0 || SubDevice > 3)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Wrong sub device number", S3_Meter.S3_MeterLogger);
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      }
      if (SubDevice > 0)
        return this.GetConfigurationParametersInput(SubDevice - 1, baseTypeParameterList, true);
      SortedList<OverrideID, ConfigurationParameter> configurationParameters = new SortedList<OverrideID, ConfigurationParameter>();
      bool flag1 = UserManager.CheckPermission(UserManager.Role_Developer) || this.MyFunctions.IsHandlerCompleteEnabled() && this.MyFunctions.WriteEnabled;
      bool flag2 = this.MyFunctions.WriteEnabled | flag1;
      bool flag3 = (((UserManager.CheckPermission(UserRights.Rights.ProfessionalConfig) ? 1 : (UserManager.CheckPermission(UserRights.Rights.DesignerChangeMenu) ? 1 : 0)) | (flag1 ? 1 : 0)) & (flag2 ? 1 : 0)) != 0;
      bool canChanged1 = !this.IsWriteProtected & flag3;
      bool flag4 = (UserManager.CheckPermission(UserRights.Rights.DesignerChangeMenu) | flag1) & flag2;
      bool canChanged2 = !this.IsWriteProtected & flag4;
      bool flag5 = ((!UserManager.IsNewLicenseModel() ? 0 : (UserManager.CheckPermission(S3_HandlerFunctions.Right_VolumeCalibration.ToString()) ? 1 : 0)) & (flag2 ? 1 : 0)) != 0;
      bool flag6 = ((!UserManager.IsNewLicenseModel() ? 0 : (UserManager.CheckPermission(S3_HandlerFunctions.Right_TemperatureCalibration.ToString()) ? 1 : 0)) & (flag2 ? 1 : 0)) != 0;
      bool flag7 = ((!UserManager.IsNewLicenseModel() ? 0 : (UserManager.CheckPermission(S3_HandlerFunctions.Right_ChangeIdentification.ToString()) ? 1 : 0)) & (flag2 ? 1 : 0)) != 0;
      uint uintValue1 = this.MyParameters.ParameterByName["SerDev0_IdentNo"].GetUintValue();
      S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.SerialNumber)
      {
        ParameterValue = (object) uintValue1.ToString("x08"),
        HasWritePermission = flag2
      }, useRights);
      ConfigurationParameter.ChangeOverValues changeoverSetup = this.GetChangeoverSetup();
      S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.ChangeOver)
      {
        ParameterValue = (object) changeoverSetup,
        HasWritePermission = canChanged2
      }, canChanged2, useRights);
      S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.EnergyResolution)
      {
        ParameterValue = (object) this.MyMeterScaling.energyResolutionInfo.resolutionString,
        AllowedValues = this.MyMeterScaling.GetAllowedEnergyResolutions(),
        HasWritePermission = canChanged2
      }, canChanged2, useRights);
      Decimal num1 = (Decimal) this.MyParameters.ParameterByName["Bak_HeatEnergySum"].GetDoubleValue() * this.MyMeterScaling.energyLcdToBaseUnitFactor;
      S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.EnergyActualValue)
      {
        Unit = this.MyMeterScaling.energyResolutionInfo.baseUnitString,
        ParameterValue = (object) num1,
        HasWritePermission = canChanged1
      }, canChanged1, useRights);
      if (changeoverSetup == ConfigurationParameter.ChangeOverValues.Cooling || changeoverSetup == ConfigurationParameter.ChangeOverValues.ChangeOver)
      {
        Decimal num2 = (Decimal) this.MyParameters.ParameterByName["Bak_ColdEnergySum"].GetDoubleValue() * this.MyMeterScaling.energyLcdToBaseUnitFactor;
        S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.CEnergyActualValue)
        {
          Unit = this.MyMeterScaling.energyResolutionInfo.baseUnitString,
          ParameterValue = (object) num2,
          HasWritePermission = canChanged1
        }, canChanged1, useRights);
      }
      Decimal num3 = (Decimal) this.MyParameters.ParameterByName["Bak_VolSum"].GetDoubleValue() * this.MyMeterScaling.volumeLcdToQmFactor;
      S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.VolumeActualValue)
      {
        ParameterValue = (object) num3,
        HasWritePermission = canChanged1
      }, canChanged1, useRights);
      VolumeInputModes volumeInputModes = VolumeInputModes.Impulse_100Hz;
      int index1 = this.MyParameters.ParameterByName.IndexOfKey(S3_ParameterNames.VolInputSetup.ToString());
      if (index1 >= 0)
      {
        volumeInputModes = (VolumeInputModes) this.MyParameters.ParameterByName.Values[index1].GetByteValue();
        S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.InputMode)
        {
          ParameterValue = (object) volumeInputModes,
          HasWritePermission = canChanged2
        }, canChanged2, useRights);
      }
      S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.VolumeResolution)
      {
        ParameterValue = (object) this.MyMeterScaling.volumeResolutionInfo.resolutionString,
        AllowedValues = this.MyMeterScaling.GetAllowedVolumeResolutions(),
        HasWritePermission = canChanged2
      }, canChanged2, useRights);
      if (!this.MyIdentification.IsWR4 && !this.MyIdentification.IsUltrasonic || this.MyIdentification.IsWR4 && volumeInputModes != VolumeInputModes.VMCP_Interface)
        S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.VolumePulsValue)
        {
          ParameterValue = (object) (double) this.MyMeterScaling.volumePulsValueLiterPerImpuls,
          HasWritePermission = canChanged2
        }, canChanged2, useRights);
      if (baseTypeParameterList != null)
      {
        int index2 = baseTypeParameterList.IndexOfKey(OverrideID.ShowVolumeAsMass);
        if (index2 >= 0)
        {
          ConfigurationParameter ConfigParam = baseTypeParameterList.Values[index2].Clone();
          ConfigParam.ParameterValue = (object) this.MyResources.IsResourceAvailable("MassAvailable");
          ConfigParam.HasWritePermission = canChanged2;
          S3_Meter.AddToConfigParamList(configurationParameters, ConfigParam, canChanged2, useRights);
        }
      }
      else if (this.MyResources.IsResourceAvailable("MassAvailable"))
        S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.ShowVolumeAsMass)
        {
          ParameterValue = (object) true,
          HasWritePermission = canChanged2
        }, canChanged2, useRights);
      S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.SetPcTime)
      {
        ParameterValue = (object) this.MyFunctions.usePcTime,
        HasWritePermission = flag2,
        IsFunction = true
      }, useRights);
      S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.TimeZone)
      {
        ParameterValue = (object) (Decimal) this.TimeZoneHourOffset,
        HasWritePermission = flag2
      }, useRights);
      ConfigurationParameter ConfigParam1 = new ConfigurationParameter(OverrideID.DeviceClock);
      ConfigParam1.ParameterValue = (object) this.MeterTimeAsDateTime;
      ConfigParam1.HasWritePermission = !this.MyFunctions.usePcTime & flag2;
      S3_Meter.AddToConfigParamList(configurationParameters, ConfigParam1, ConfigParam1.HasWritePermission, useRights);
      ushort ushortValue1 = this.MyParameters.ParameterByName[S3_ParameterNames.Device_Setup_2.ToString()].GetUshortValue();
      if (this.MyResources.IsResourceAvailable(S3_MeterResources.SelectFlowLineAvailable.ToString()))
      {
        if (canChanged2 || ((uint) ushortValue1 & 8U) > 0U)
        {
          string str = "";
          switch ((int) ushortValue1 & 9)
          {
            case 0:
              str = S3VolMeterPositions.outlet.ToString();
              break;
            case 1:
              str = S3VolMeterPositions.inlet.ToString();
              break;
            case 9:
              str = S3VolMeterPositions.selectable.ToString();
              break;
          }
          string[] strArray1 = new string[3];
          S3VolMeterPositions volMeterPositions = S3VolMeterPositions.outlet;
          strArray1[0] = volMeterPositions.ToString();
          volMeterPositions = S3VolMeterPositions.inlet;
          strArray1[1] = volMeterPositions.ToString();
          volMeterPositions = S3VolMeterPositions.selectable;
          strArray1[2] = volMeterPositions.ToString();
          string[] strArray2 = strArray1;
          S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.VolMeterFlowPositionByUser)
          {
            AllowedValues = strArray2,
            ParameterValue = (object) str,
            HasWritePermission = canChanged2
          }, canChanged2, useRights);
          S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.OutletTempSensorInVolumeMeter)
          {
            ParameterValue = (object) (((uint) ushortValue1 & 64U) > 0U),
            HasWritePermission = canChanged2
          }, canChanged2, useRights);
        }
        else
          S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.VolMeterFlowPosition)
          {
            ParameterValue = (object) (((uint) ushortValue1 & 1U) > 0U),
            HasWritePermission = canChanged2
          }, canChanged2, useRights);
      }
      else
        S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.VolMeterFlowPosition)
        {
          ParameterValue = (object) (((uint) ushortValue1 & 1U) > 0U),
          HasWritePermission = canChanged2
        }, canChanged2, useRights);
      int index3 = this.MyParameters.ParameterByName.IndexOfKey(S3_ParameterNames.Con_Temp_Display_Range_Max.ToString());
      if (index3 >= 0)
      {
        short shortValue = this.MyParameters.ParameterByName.Values[index3].GetShortValue();
        S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.TempRangeUpperLimit)
        {
          ParameterValue = (object) ((Decimal) shortValue / 100M),
          HasWritePermission = canChanged2
        }, canChanged2, useRights);
      }
      int index4 = this.MyParameters.ParameterByName.IndexOfKey(S3_ParameterNames.Con_Temp_Display_Range_Min.ToString());
      if (index4 >= 0)
      {
        short shortValue = this.MyParameters.ParameterByName.Values[index4].GetShortValue();
        S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.TempRangeLowerLimit)
        {
          ParameterValue = (object) ((Decimal) shortValue / 100M),
          HasWritePermission = canChanged2
        }, canChanged2, useRights);
      }
      bool flag8 = canChanged1;
      int byteValue1 = (int) this.MyParameters.ParameterByName["temperaturCycleTimeCounterInit"].GetByteValue();
      int byteValue2 = (int) this.MyParameters.ParameterByName["temperaturCycleTimeSlotCounterInit"].GetByteValue();
      int num4 = 2;
      S3_ParameterNames s3ParameterNames;
      S3_MeterResources s3MeterResources;
      ulong num5;
      ulong num6;
      if (!this.MyIdentification.IsWR4)
      {
        SortedList<string, S3_Parameter> parameterByName = this.MyParameters.ParameterByName;
        s3ParameterNames = S3_ParameterNames.volumeCycleTimeCounterInit;
        string key = s3ParameterNames.ToString();
        num4 = (int) parameterByName[key].GetByteValue();
        if ((num4 & 1) != 0)
        {
          ZR_ClassLibMessages.AddWarning("Volume cycle factor is not even", S3_Meter.S3_MeterLogger);
          ++num4;
          flag8 = false;
        }
        MeterResources resources = this.MyResources;
        s3MeterResources = S3_MeterResources.IUF;
        string res = s3MeterResources.ToString();
        if (resources.IsResourceAvailable(res))
        {
          ConfigurationParameter ConfigParam2 = new ConfigurationParameter(OverrideID.CycleTimeVolume);
          ConfigParam2.ParameterValue = (object) (ulong) (num4 / 2);
          ConfigParam2.Unit = "s";
          ConfigParam2.HasWritePermission = flag1 && flag8;
          S3_Meter.AddToConfigParamList(configurationParameters, ConfigParam2, ConfigParam2.HasWritePermission, useRights);
        }
        num5 = (ulong) (num4 * byteValue1 * byteValue2 / 2);
        num6 = (ulong) (num4 * byteValue1 / 2);
      }
      else
      {
        num5 = (ulong) (byteValue1 * byteValue2 / 2);
        num6 = (ulong) (byteValue1 / 2);
      }
      string[] strArray3 = new string[30];
      for (int index5 = 0; index5 < strArray3.Length; ++index5)
        strArray3[index5] = (2 * index5 + 2).ToString();
      int length = 10;
      if (byteValue2 > length)
        length = byteValue2;
      string[] strArray4 = new string[length];
      for (int index6 = 0; index6 < strArray4.Length; ++index6)
        strArray4[index6] = ((int) num6 * (index6 + 1)).ToString();
      ConfigurationParameter ConfigParam3 = new ConfigurationParameter(OverrideID.CycleTimeStandard);
      ConfigParam3.ParameterValue = (object) num5;
      if (flag8)
        ConfigParam3.AllowedValues = strArray4;
      ConfigParam3.HasWritePermission = flag8;
      ConfigParam3.Unit = "s";
      S3_Meter.AddToConfigParamList(configurationParameters, ConfigParam3, ConfigParam3.HasWritePermission, useRights);
      ConfigurationParameter ConfigParam4 = new ConfigurationParameter(OverrideID.CycleTimeFast);
      ConfigParam4.ParameterValue = (object) num6;
      if (flag8)
        ConfigParam4.AllowedValues = strArray3;
      ConfigParam4.HasWritePermission = flag8;
      ConfigParam4.Unit = "s";
      S3_Meter.AddToConfigParamList(configurationParameters, ConfigParam4, ConfigParam4.HasWritePermission, useRights);
      if (this.MyResources.AvailableMeterResources.Keys.Contains("Radio"))
      {
        SortedList<string, S3_Parameter> parameterByName = this.MyParameters.ParameterByName;
        s3ParameterNames = S3_ParameterNames.radioCycleTimeCounterInit;
        string key = s3ParameterNames.ToString();
        int ushortValue2 = (int) parameterByName[key].GetUshortValue();
        int num7 = num4 / 2;
        int seconds1 = num7 * ushortValue2;
        List<string> stringList1 = new List<string>();
        int num8 = num7;
        int num9 = num7;
        int seconds2 = num7;
        TimeSpan timeSpan;
        while (seconds2 <= 86400)
        {
          List<string> stringList2 = stringList1;
          timeSpan = new TimeSpan(0, 0, seconds2);
          string str = timeSpan.ToString();
          stringList2.Add(str);
          if (seconds2 < 120)
          {
            seconds2 += num7;
            num8 = seconds2;
            if (num8 > 60)
              num8 = 60 / num7 * num7;
          }
          else if (seconds2 < 3600)
          {
            seconds2 += num8;
            num9 = seconds2;
          }
          else
            seconds2 += num9;
        }
        ConfigurationParameter ConfigParam5 = new ConfigurationParameter(OverrideID.CycleTimeRadio);
        ConfigurationParameter configurationParameter = ConfigParam5;
        timeSpan = new TimeSpan(0, 0, seconds1);
        string str1 = timeSpan.ToString();
        configurationParameter.ParameterValue = (object) str1;
        if (flag8)
          ConfigParam5.AllowedValues = stringList1.ToArray();
        ConfigParam5.HasWritePermission = flag8;
        ConfigParam5.Unit = "hours";
        S3_Meter.AddToConfigParamList(configurationParameters, ConfigParam5, ConfigParam5.HasWritePermission, useRights);
      }
      else if (this.MyResources.AvailableMeterResources.Keys.Contains("LoRa"))
      {
        SortedList<string, S3_Parameter> parameterByName = this.MyParameters.ParameterByName;
        s3ParameterNames = S3_ParameterNames.cfg_transmission_scenario;
        string key = s3ParameterNames.ToString();
        RadioScenario radioScenario = (RadioScenario) ((int) parameterByName[key].GetByteValue() + 200);
        List<string> stringList = new List<string>();
        stringList.Add(RadioScenario.Scenario_202_LoRaDaily.ToString());
        stringList.Add(RadioScenario.Scenario_201_LoRaMonthly.ToString());
        if (!stringList.Contains(radioScenario.ToString()))
          throw new Exception("Illegal scenario");
        ConfigurationParameter ConfigParam6 = new ConfigurationParameter(OverrideID.RadioScenario);
        ConfigParam6.ParameterValue = (object) radioScenario;
        ConfigParam6.AllowedValues = stringList.ToArray();
        ConfigParam6.HasWritePermission = flag2;
        S3_Meter.AddToConfigParamList(configurationParameters, ConfigParam6, ConfigParam6.HasWritePermission, useRights);
      }
      DateTime dateTimeValue1 = this.MyParameters.ParameterByName["EndOfBatterie"].GetDateTimeValue();
      ConfigurationParameter ConfigParam7 = new ConfigurationParameter(OverrideID.EndOfBattery);
      ConfigParam7.ParameterValue = (object) dateTimeValue1;
      ConfigParam7.HasWritePermission = flag3;
      S3_Meter.AddToConfigParamList(configurationParameters, ConfigParam7, ConfigParam7.HasWritePermission, useRights);
      ConfigurationParameter ConfigParam8 = new ConfigurationParameter(OverrideID.BatteryCapacity_mAh);
      ConfigParam8.ParameterValue = this.BatteryManager == null ? (object) double.NaN : (object) this.BatteryManager.BatteryCapacity_mAh;
      ConfigParam8.HasWritePermission = flag3;
      S3_Meter.AddToConfigParamList(configurationParameters, ConfigParam8, ConfigParam8.HasWritePermission, useRights);
      DateTime dateTimeValue2 = this.MyParameters.ParameterByName["EndOfCalibration"].GetDateTimeValue();
      ConfigurationParameter ConfigParam9 = new ConfigurationParameter(OverrideID.EndOfCalibration);
      ConfigParam9.ParameterValue = (object) dateTimeValue2.Year;
      ConfigParam9.HasWritePermission = flag3;
      S3_Meter.AddToConfigParamList(configurationParameters, ConfigParam9, ConfigParam9.HasWritePermission, useRights);
      byte byteValue3 = this.MyParameters.ParameterByName["Bak_DueDateMonth"].GetByteValue();
      ConfigurationParameter ConfigParam10;
      if (this.MyIdentification.FirmwareVersion < 117440512U)
      {
        ConfigParam10 = new ConfigurationParameter(OverrideID.DueDateMonth);
        ConfigParam10.ParameterValue = (object) byteValue3;
      }
      else
      {
        ConfigParam10 = new ConfigurationParameter(OverrideID.DueDate);
        DateTime dateTime = byteValue3 <= (byte) 12 ? new DateTime(DateTime.Now.Year, (int) byteValue3, 1) : new DateTime(DateTime.Now.Year, (int) byteValue3 & 15, 16);
        if (dateTime > DateTime.Now)
          dateTime = dateTime.AddYears(-1);
        ConfigParam10.ParameterValue = (object) dateTime;
        ConfigParam10.Format = "dd.MM";
      }
      ConfigParam10.HasWritePermission = flag2;
      S3_Meter.AddToConfigParamList(configurationParameters, ConfigParam10, useRights);
      S3_Parameter s3Parameter1 = this.MyParameters.ParameterByName["SerDev0_SelectedList_PrimaryAddress"];
      S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.MBusAddress)
      {
        ParameterValue = (object) ((int) s3Parameter1.GetUshortValue() & (int) byte.MaxValue),
        HasWritePermission = flag2
      }, useRights);
      if (this.MyIdentification.FirmwareVersion == 67309573U)
      {
        ConfigurationParameter ConfigParam11 = new ConfigurationParameter(OverrideID.MBusThirdPartySupport);
        ConfigParam11.ParameterValue = (object) this.MyTransmitParameterManager.GetMBusThirdPartySupportState();
        ConfigParam11.HasWritePermission = flag4;
        S3_Meter.AddToConfigParamList(configurationParameters, ConfigParam11, ConfigParam11.HasWritePermission, useRights);
      }
      ConfigurationParameter ConfigParam12 = new ConfigurationParameter(OverrideID.OperatingHours);
      uint uintValue2 = this.MyParameters.ParameterByName["StartOfOperation"].GetUintValue();
      ulong num10 = (ulong) ((this.MyParameters.ParameterByName["Bak_TimeBaseSecounds"].GetUintValue() - uintValue2) / 3600U);
      ConfigParam12.ParameterValue = (object) num10;
      ConfigParam12.HasWritePermission = flag3;
      S3_Meter.AddToConfigParamList(configurationParameters, ConfigParam12, useRights);
      SortedList<string, S3_Parameter> parameterByName1 = this.MyParameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.Sta_Status;
      string key1 = s3ParameterNames.ToString();
      S3_Parameter s3Parameter2 = parameterByName1[key1];
      S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.DeviceHasError)
      {
        ParameterValue = (object) (s3Parameter2.GetUshortValue() > (ushort) 0),
        HasWritePermission = false
      }, false, useRights);
      S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.Protected)
      {
        ParameterValue = (object) this.IsWriteProtected,
        HasWritePermission = false
      }, false, useRights);
      if (canChanged1 | canChanged2)
      {
        S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.SetWriteProtection)
        {
          ParameterValue = (object) this.MyFunctions.MyMeters.SetWriteProtectionOnWrite,
          HasWritePermission = flag2,
          IsFunction = true
        }, canChanged1 | canChanged2, useRights);
        S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.SetSleepMode)
        {
          ParameterValue = (object) this.MyFunctions.MyMeters.SetSleepModeOnWrite,
          HasWritePermission = flag2,
          IsFunction = true
        }, canChanged1 | canChanged2, useRights);
      }
      S3_Parameter s3Parameter3 = this.MyParameters.ParameterByName["Sta_Status"];
      S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.ErrorCode)
      {
        ParameterValue = (object) s3Parameter3.GetUshortValue(),
        Format = "X04",
        HasWritePermission = false
      }, false, useRights);
      if (flag3)
        S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.ResetAllValues)
        {
          ParameterValue = (object) false,
          HasWritePermission = flag2,
          IsFunction = true
        }, useRights);
      S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.ClearAllLoggers)
      {
        ParameterValue = (object) false,
        HasWritePermission = flag2,
        IsFunction = true
      }, useRights);
      if (flag4)
        S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.SetMbusPrimAdrFromSerialNumber)
        {
          ParameterValue = (object) this.MyTransmitParameterManager.IsMbusPrimAddressesFromSerialNumber(),
          HasWritePermission = flag2,
          IsFunction = true
        }, useRights);
      if (this.IsMBusListSwitchingPossible())
      {
        bool flag9 = !this.MyTransmitParameterManager.AreVirtualDevicesUsed();
        ConfigurationParameter ConfigParam13 = new ConfigurationParameter(OverrideID.CompactMBusList);
        ConfigParam13.ParameterValue = (object) flag9;
        MeterResources resources = this.MyResources;
        s3MeterResources = S3_MeterResources.Radio;
        string res = s3MeterResources.ToString();
        bool flag10 = resources.IsResourceAvailable(res);
        ConfigParam13.HasWritePermission = !flag10 && flag2;
        S3_Meter.AddToConfigParamList(configurationParameters, ConfigParam13, !flag10, useRights);
      }
      MeterResources resources1 = this.MyResources;
      s3MeterResources = S3_MeterResources.Radio;
      string res1 = s3MeterResources.ToString();
      if (resources1.IsResourceAvailable(res1))
      {
        ConfigurationParameter ConfigParam14 = new ConfigurationParameter(OverrideID.SelectedRadioList);
        ConfigParam14.ParameterValue = (object) this.MyTransmitParameterManager.Transmitter.Radio.Get_ActivatedListAndEncryption();
        if (ConfigParam14.ParameterValue != null)
        {
          ConfigParam14.HasWritePermission = canChanged1;
          ConfigParam14.IsFunction = false;
          ConfigParam14.AllowedValues = this.MyTransmitParameterManager.Transmitter.Radio.AvailableListAndEncryptionNames().ToArray();
          S3_Meter.AddToConfigParamList(configurationParameters, ConfigParam14, useRights);
        }
      }
      MeterResources resources2 = this.MyResources;
      s3MeterResources = S3_MeterResources.Radio;
      string res2 = s3MeterResources.ToString();
      if (resources2.IsResourceAvailable(res2) && configurationParameters.ContainsKey(OverrideID.SelectedRadioList))
      {
        RADIO_MODE? activatedRadioMode = this.MyTransmitParameterManager.Transmitter.Radio.Get_ActivatedRadioMode();
        RADIO_MODE? nullable = activatedRadioMode;
        RADIO_MODE radioMode1 = RADIO_MODE.Radio3_Sz0;
        int num11;
        if (!(nullable.GetValueOrDefault() == radioMode1 & nullable.HasValue))
        {
          nullable = activatedRadioMode;
          RADIO_MODE radioMode2 = RADIO_MODE.Radio3;
          if (!(nullable.GetValueOrDefault() == radioMode2 & nullable.HasValue))
          {
            nullable = activatedRadioMode;
            RADIO_MODE radioMode3 = RADIO_MODE.Radio3_Sz5;
            num11 = !(nullable.GetValueOrDefault() == radioMode3 & nullable.HasValue) ? 1 : 0;
            goto label_89;
          }
        }
        num11 = 0;
label_89:
        if (num11 != 0)
        {
          SortedList<string, S3_Parameter> parameterByName2 = this.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.encryptionKey;
          string key2 = s3ParameterNames.ToString();
          S3_Parameter s3Parameter4 = parameterByName2[key2];
          SortedList<string, S3_Parameter> parameterByName3 = this.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.encryptionKey1;
          string key3 = s3ParameterNames.ToString();
          S3_Parameter s3Parameter5 = parameterByName3[key3];
          SortedList<string, S3_Parameter> parameterByName4 = this.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.encryptionKey2;
          string key4 = s3ParameterNames.ToString();
          S3_Parameter s3Parameter6 = parameterByName4[key4];
          SortedList<string, S3_Parameter> parameterByName5 = this.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.encryptionKey3;
          string key5 = s3ParameterNames.ToString();
          S3_Parameter s3Parameter7 = parameterByName5[key5];
          SortedList<string, S3_Parameter> parameterByName6 = this.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.radioAndEncMode;
          string key6 = s3ParameterNames.ToString();
          bool flag11;
          if (parameterByName6.ContainsKey(key6))
          {
            SortedList<string, S3_Parameter> parameterByName7 = this.MyParameters.ParameterByName;
            s3ParameterNames = S3_ParameterNames.radioAndEncMode;
            string key7 = s3ParameterNames.ToString();
            flag11 = ((uint) parameterByName7[key7].GetByteValue() & 15U) > 0U;
          }
          else
          {
            SortedList<string, S3_Parameter> parameterByName8 = this.MyParameters.ParameterByName;
            s3ParameterNames = S3_ParameterNames.DeviceSetupNp;
            string key8 = s3ParameterNames.ToString();
            flag11 = ((int) parameterByName8[key8].GetByteValue() & 128) == 128;
          }
          ConfigurationParameter ConfigParam15 = new ConfigurationParameter(OverrideID.AESKey);
          ConfigParam15.ParameterValue = !flag11 ? (object) "OFF" : (object) AES.AesKeyToString(s3Parameter4.GetUintValue(), s3Parameter5.GetUintValue(), s3Parameter6.GetUintValue(), s3Parameter7.GetUintValue());
          ConfigParam15.HasWritePermission = flag2;
          ConfigParam15.IsFunction = false;
          ConfigParam15.IsEditable = true;
          List<string> stringList = AES.AllowedKeys();
          if (!stringList.Contains((string) ConfigParam15.ParameterValue))
            stringList.Add((string) ConfigParam15.ParameterValue);
          ConfigParam15.AllowedValues = stringList.ToArray();
          S3_Meter.AddToConfigParamList(configurationParameters, ConfigParam15, useRights);
        }
      }
      if (baseTypeParameterList != null)
      {
        if (baseTypeParameterList.IndexOfKey(OverrideID.ShowEnergyChecker) >= 0)
        {
          ConfigurationParameter ConfigParam16 = new ConfigurationParameter(OverrideID.ShowEnergyChecker);
          ConfigParam16.ParameterValue = (object) this.MyResources.IsResourceAvailable("CheckerAvailable");
          if ((bool) ConfigParam16.ParameterValue)
          {
            ConfigParam16.HasWritePermission = canChanged2;
            S3_Meter.AddToConfigParamList(configurationParameters, ConfigParam16, canChanged2, useRights);
          }
        }
      }
      else if (this.MyResources.IsResourceAvailable("CheckerAvailable"))
        S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.ShowEnergyChecker)
        {
          ParameterValue = (object) true,
          HasWritePermission = canChanged2
        }, canChanged2, useRights);
      if (baseTypeParameterList != null)
      {
        if (baseTypeParameterList.IndexOfKey(OverrideID.ShowGCAL) >= 0)
          S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.ShowGCAL)
          {
            ParameterValue = (object) this.MyResources.IsResourceAvailable("GCalAvailable"),
            HasWritePermission = canChanged2
          }, canChanged2, useRights);
      }
      else if (this.MyResources.IsResourceAvailable("GCalAvailable"))
        S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.ShowGCAL)
        {
          ParameterValue = (object) true,
          HasWritePermission = canChanged2
        }, canChanged2, useRights);
      if (baseTypeParameterList != null)
      {
        if (baseTypeParameterList.IndexOfKey(OverrideID.Glycol) >= 0)
        {
          ConfigurationParameter ConfigParam17 = new ConfigurationParameter(OverrideID.Glycol);
          if (!this.MyResources.IsResourceAvailable("GlycolAvailable"))
          {
            ConfigParam17.ParameterValue = (object) "off";
          }
          else
          {
            SortedList<string, S3_Parameter> parameterByName9 = this.MyParameters.ParameterByName;
            s3ParameterNames = S3_ParameterNames.Heap_GlycolTableIndex;
            string key9 = s3ParameterNames.ToString();
            byte index7 = parameterByName9[key9].GetByteValue();
            if ((int) index7 > S3_Meter.glycolSettings.Length)
              index7 = (byte) 0;
            ConfigParam17.ParameterValue = (object) ((IEnumerable<string>) S3_Meter.glycolSettings).ElementAt<string>((int) index7);
          }
          ConfigParam17.AllowedValues = ((IEnumerable<string>) S3_Meter.glycolSettings).ToArray<string>();
          ConfigParam17.HasWritePermission = canChanged2;
          ConfigParam17.Unit = "%";
          S3_Meter.AddToConfigParamList(configurationParameters, ConfigParam17, canChanged2, useRights);
        }
      }
      else if (this.MyResources.IsResourceAvailable("GlycolAvailable"))
      {
        ConfigurationParameter ConfigParam18 = new ConfigurationParameter(OverrideID.Glycol);
        SortedList<string, S3_Parameter> parameterByName10 = this.MyParameters.ParameterByName;
        s3ParameterNames = S3_ParameterNames.Heap_GlycolTableIndex;
        string key10 = s3ParameterNames.ToString();
        byte index8 = parameterByName10[key10].GetByteValue();
        if ((int) index8 > S3_Meter.glycolSettings.Length)
          index8 = (byte) 0;
        ConfigParam18.ParameterValue = (object) ((IEnumerable<string>) S3_Meter.glycolSettings).ElementAt<string>((int) index8);
        ConfigParam18.AllowedValues = ((IEnumerable<string>) S3_Meter.glycolSettings).ToArray<string>();
        ConfigParam18.HasWritePermission = canChanged2;
        ConfigParam18.Unit = "%";
        S3_Meter.AddToConfigParamList(configurationParameters, ConfigParam18, canChanged2, useRights);
      }
      if (!this.IsWriteProtected && (ConfigurationParameter.ActiveConfigurationLevel & ConfigurationParameter.ConfigParametersByOverrideID[OverrideID.CalVolMaxFlowLiterPerHour].DefaultConfigurationLevels) > (ConfigurationLevel) 0)
      {
        if (flag5)
        {
          if (this.calibrationValues == null)
          {
            this.GetCalibrationParameter(configurationParameters, OverrideID.CalVolMaxFlowLiterPerHour);
            this.GetCalibrationParameter(configurationParameters, OverrideID.CalVolMaxErrorPercent);
            this.GetCalibrationParameter(configurationParameters, OverrideID.CalVolNominalFlowLiterPerHour);
            this.GetCalibrationParameter(configurationParameters, OverrideID.CalVolNominalErrorPercent);
            this.GetCalibrationParameter(configurationParameters, OverrideID.CalVolMinFlowLiterPerHour);
            this.GetCalibrationParameter(configurationParameters, OverrideID.CalVolMinErrorPercent);
          }
          else
          {
            this.GetCalibrationParameter(configurationParameters, OverrideID.CalVolMaxFlowLiterPerHour, this.calibrationValues.CalVolMaxFlowLiterPerHour);
            this.GetCalibrationParameter(configurationParameters, OverrideID.CalVolMaxErrorPercent, this.calibrationValues.CalVolMaxErrorPercent);
            this.GetCalibrationParameter(configurationParameters, OverrideID.CalVolNominalFlowLiterPerHour, this.calibrationValues.CalVolNominalFlowLiterPerHour);
            this.GetCalibrationParameter(configurationParameters, OverrideID.CalVolNominalErrorPercent, this.calibrationValues.CalVolNominalErrorPercent);
            this.GetCalibrationParameter(configurationParameters, OverrideID.CalVolMinFlowLiterPerHour, this.calibrationValues.CalVolMinFlowLiterPerHour);
            this.GetCalibrationParameter(configurationParameters, OverrideID.CalVolMinErrorPercent, this.calibrationValues.CalVolMinErrorPercent);
          }
        }
        ConfigurationParameter ConfigParam19 = new ConfigurationParameter(OverrideID.TestVolumeSimulation);
        ConfigParam19.HasWritePermission = true;
        if (this.TestVolumeSimulationValue.HasValue)
          ConfigParam19.ParameterValue = (object) this.TestVolumeSimulationValue.Value;
        S3_Meter.AddToConfigParamList(configurationParameters, ConfigParam19, useRights);
      }
      if (!flag7)
        ;
      if (this.BatteryManager != null & flag3)
      {
        ConfigurationParameter configurationParameter1 = new ConfigurationParameter(OverrideID.MaxEndOfBatteryDate);
        configurationParameter1.ParameterValue = (object) this.BatteryManager.MaxEndOfBatteryDate;
        configurationParameter1.HasWritePermission = false;
        configurationParameters.Add(configurationParameter1.ParameterID, configurationParameter1);
        if (this.BatteryManager.LoRaDiagnosticCounts.HasValue)
        {
          uint num12 = this.BatteryManager.LoRaDiagnosticCounts.Value;
          ConfigurationParameter configurationParameter2 = new ConfigurationParameter(OverrideID.RemainingDiagnosticMessages);
          configurationParameter2.ParameterValue = (object) num12;
          configurationParameter2.HasWritePermission = false;
          configurationParameters.Add(configurationParameter2.ParameterID, configurationParameter2);
          double num13 = (double) this.BatteryManager.LoRaDiagnosticCounts.Value / 24.0 / 365.0;
          ConfigurationParameter configurationParameter3 = new ConfigurationParameter(OverrideID.PossibleHourDiagnosticYears);
          configurationParameter3.ParameterValue = (object) num13;
          configurationParameter3.HasWritePermission = false;
          configurationParameters.Add(configurationParameter3.ParameterID, configurationParameter3);
        }
      }
      else if (this.MyIdentification.IsLoRa)
      {
        SortedList<string, S3_Parameter> parameterByName11 = this.MyParameters.ParameterByName;
        s3ParameterNames = S3_ParameterNames.lora_diagnostic_remaining_messages;
        string key11 = s3ParameterNames.ToString();
        uint uintValue3 = parameterByName11[key11].GetUintValue();
        ConfigurationParameter configurationParameter4 = new ConfigurationParameter(OverrideID.RemainingDiagnosticMessages);
        configurationParameter4.ParameterValue = (object) uintValue3;
        configurationParameter4.HasWritePermission = false;
        configurationParameters.Add(configurationParameter4.ParameterID, configurationParameter4);
        double num14 = (double) uintValue3 / 24.0 / 365.0;
        ConfigurationParameter configurationParameter5 = new ConfigurationParameter(OverrideID.PossibleHourDiagnosticYears);
        configurationParameter5.ParameterValue = (object) num14;
        configurationParameter5.HasWritePermission = false;
        configurationParameters.Add(configurationParameter5.ParameterID, configurationParameter5);
      }
      if (this.MyIdentification.IsLoRa)
      {
        DeviceIdentification deviceIdentification = this.MyFunctions.GetDeviceIdentification();
        S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.PrintedSerialNumber)
        {
          ParameterValue = (object) deviceIdentification.PrintedSerialNumberAsString,
          HasWritePermission = false
        }, useRights);
        S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.RadioTechnology)
        {
          ParameterValue = (object) RadioTechnology.LoRa,
          HasWritePermission = false
        }, useRights);
        S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.DevEUI)
        {
          ParameterValue = (object) deviceIdentification.LoRa_DevEUI,
          HasWritePermission = flag2
        }, useRights);
        S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.JoinEUI)
        {
          ParameterValue = (object) deviceIdentification.LoRa_JoinEUI,
          HasWritePermission = flag2
        }, useRights);
        S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.AppKey)
        {
          ParameterValue = (object) deviceIdentification.LoRa_AppKey,
          HasWritePermission = flag2
        }, useRights);
        ConfigurationParameter ConfigParam20 = new ConfigurationParameter(OverrideID.Activation);
        S3_Parameter s3Parameter8 = this.MyParameters.ParameterByName["cfg_otaa_abp_mode"];
        ConfigParam20.ParameterValue = (object) (OTAA_ABP) s3Parameter8.GetByteValue();
        ConfigParam20.HasWritePermission = flag2;
        S3_Meter.AddToConfigParamList(configurationParameters, ConfigParam20, useRights);
        byte[] byteArray1 = this.MyParameters.ParameterByName["cfb_lora_nwkskey_0"].GetByteArray(8);
        byte[] byteArray2 = this.MyParameters.ParameterByName["cfb_lora_nwkskey_1"].GetByteArray(8);
        byte[] destinationArray1 = new byte[16];
        Array.Copy((Array) byteArray1, (Array) destinationArray1, 8);
        Array.Copy((Array) byteArray2, 0, (Array) destinationArray1, 8, 8);
        S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.NwkSKey)
        {
          ParameterValue = (object) destinationArray1,
          HasWritePermission = flag2
        }, useRights);
        byte[] byteArray3 = this.MyParameters.ParameterByName["cfb_lora_appskey_0"].GetByteArray(8);
        byte[] byteArray4 = this.MyParameters.ParameterByName["cfb_lora_appskey_1"].GetByteArray(8);
        byte[] destinationArray2 = new byte[16];
        Array.Copy((Array) byteArray3, (Array) destinationArray2, 8);
        Array.Copy((Array) byteArray4, 0, (Array) destinationArray2, 8, 8);
        S3_Meter.AddToConfigParamList(configurationParameters, new ConfigurationParameter(OverrideID.AppSKey)
        {
          ParameterValue = (object) destinationArray2,
          HasWritePermission = flag2
        }, useRights);
        ConfigurationParameter ConfigParam21 = new ConfigurationParameter(OverrideID.DevAddr);
        S3_Parameter s3Parameter9 = this.MyParameters.ParameterByName["cfg_lora_device_id"];
        ConfigParam21.ParameterValue = (object) s3Parameter9.GetUintValue();
        ConfigParam21.HasWritePermission = flag2;
        S3_Meter.AddToConfigParamList(configurationParameters, ConfigParam21, useRights);
        ConfigurationParameter ConfigParam22 = new ConfigurationParameter(OverrideID.NetID);
        S3_Parameter s3Parameter10 = this.MyParameters.ParameterByName["cfg_loraWan_netid"];
        ConfigParam22.ParameterValue = (object) s3Parameter10.GetUintValue();
        ConfigParam22.HasWritePermission = flag2;
        S3_Meter.AddToConfigParamList(configurationParameters, ConfigParam22, useRights);
        ConfigurationParameter ConfigParam23 = new ConfigurationParameter(OverrideID.ADR);
        S3_Parameter s3Parameter11 = this.MyParameters.ParameterByName["lora_adr"];
        ConfigParam23.ParameterValue = (object) Convert.ToBoolean(s3Parameter11.GetByteValue());
        ConfigParam23.HasWritePermission = flag2;
        S3_Meter.AddToConfigParamList(configurationParameters, ConfigParam23, useRights);
        ConfigurationParameter ConfigParam24 = new ConfigurationParameter(OverrideID.TransmissionScenario);
        S3_Parameter s3Parameter12 = this.MyParameters.ParameterByName["cfg_transmission_scenario"];
        ConfigParam24.ParameterValue = (object) s3Parameter12.GetByteValue();
        ConfigParam24.HasWritePermission = flag2;
        S3_Meter.AddToConfigParamList(configurationParameters, ConfigParam24, useRights);
      }
      return configurationParameters;
    }

    private void GetCalibrationParameter(
      SortedList<OverrideID, ConfigurationParameter> paramList,
      OverrideID overrideID,
      double value = double.NaN)
    {
      ConfigurationParameter configurationParameter = new ConfigurationParameter(overrideID);
      configurationParameter.HasWritePermission = this.calibrationValues == null;
      configurationParameter.ParameterValue = (object) value;
      if (configurationParameter.Unit == "m\u00B3/h")
        configurationParameter.Unit = "Liter/h";
      paramList.Add(overrideID, configurationParameter);
    }

    private static void AddToConfigParamList(
      SortedList<OverrideID, ConfigurationParameter> ConfigParamList,
      ConfigurationParameter ConfigParam,
      bool useRights)
    {
      S3_Meter.AddToConfigParamList(ConfigParamList, ConfigParam, true, useRights);
    }

    private static void AddToConfigParamList(
      SortedList<OverrideID, ConfigurationParameter> ConfigParamList,
      ConfigurationParameter ConfigParam,
      bool canChanged,
      bool useRights)
    {
      if (UserManager.IsNewLicenseModel() & useRights)
      {
        if (!UserManager.IsConfigParamVisible(ConfigParam.ParameterID))
          return;
        ConfigParam.HasWritePermission = canChanged && ConfigParam.HasWritePermission && UserManager.IsConfigParamEditable(ConfigParam.ParameterID);
        ConfigParamList.Add(ConfigParam.ParameterID, ConfigParam);
      }
      else
        ConfigParamList.Add(ConfigParam.ParameterID, ConfigParam);
    }

    private bool IsMBusListSwitchingPossible()
    {
      if (this.MyFunctions.MyMeters.TypeMeter != null)
      {
        string typeOverrideString = this.MyFunctions.MyMeters.TypeMeter.TypeOverrideString;
        if (typeOverrideString != null)
        {
          OverwriteRequirementsList requirementsList = new OverwriteRequirementsList(typeOverrideString);
          if (requirementsList.OverReqList.ContainsKey(OverwriteListTypes.MBusCompactType) || requirementsList.OverReqList.ContainsKey(OverwriteListTypes.MBusVirtualDevType))
            return true;
        }
        else if (!string.IsNullOrEmpty(this.TypeCreationString) && this.TypeCreationString.Contains("SP:"))
          return true;
      }
      return false;
    }

    internal ConfigurationParameter.ChangeOverValues GetChangeoverSetup()
    {
      ConfigurationParameter.ChangeOverValues changeoverSetup;
      switch ((ushort) ((uint) this.MyParameters.ParameterByName["Device_Setup"].GetUshortValue() & 768U))
      {
        case 512:
          changeoverSetup = !this.MyResources.IsResourceAvailable(ConfigurationParameter.ChangeOverValues.Cooling.ToString()) ? ConfigurationParameter.ChangeOverValues.Heating : ConfigurationParameter.ChangeOverValues.Cooling;
          break;
        case 768:
          changeoverSetup = !this.MyResources.IsResourceAvailable(ConfigurationParameter.ChangeOverValues.Cooling.ToString()) ? ConfigurationParameter.ChangeOverValues.Heating : ConfigurationParameter.ChangeOverValues.ChangeOver;
          break;
        default:
          changeoverSetup = !this.MyResources.IsResourceAvailable(ConfigurationParameter.ChangeOverValues.Heating.ToString()) ? ConfigurationParameter.ChangeOverValues.Cooling : ConfigurationParameter.ChangeOverValues.Heating;
          break;
      }
      return changeoverSetup;
    }

    internal SortedList<OverrideID, ConfigurationParameter> GetConfigurationParametersInput(
      int inputIndex,
      SortedList<OverrideID, ConfigurationParameter> baseTypeParameterList,
      bool useRights)
    {
      if (inputIndex == 2 && this.MyIdentification.IsWR4)
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      bool flag1 = UserManager.CheckPermission(UserManager.Role_Developer);
      bool flag2 = this.MyFunctions.WriteEnabled | (UserManager.CheckPermission(UserManager.Role_Developer) || this.MyFunctions.IsHandlerCompleteEnabled() && this.MyFunctions.WriteEnabled);
      InputData inputData = this.MyMeterScaling.inpData[inputIndex];
      List<string> inputOutputResources = this.MyResources.GetInputOutputResources(inputIndex);
      SortedList<OverrideID, ConfigurationParameter> ConfigParamList = new SortedList<OverrideID, ConfigurationParameter>();
      ConfigurationParameter ConfigParam1 = new ConfigurationParameter(OverrideID.InputOutputFunction);
      ConfigParam1.HasWritePermission = flag2;
      if (baseTypeParameterList != null)
      {
        int index = baseTypeParameterList.IndexOfKey(OverrideID.InputOutputFunction);
        if (index >= 0)
          ConfigParam1.AllowedValues = baseTypeParameterList.Values[index].AllowedValues;
      }
      if (ConfigParam1.AllowedValues == null)
        ConfigParam1.AllowedValues = inputOutputResources.ToArray();
      bool flag3 = this.MyResources.IsResourceAvailable(S3_MeterResources.Radio.ToString());
      if (inputData == null || inputData.impulsValueFactor == (ushort) 0)
      {
        for (int index = 0; index < inputOutputResources.Count; ++index)
        {
          if (inputOutputResources[index].StartsWith("Output"))
          {
            ConfigParam1.ParameterValue = (object) (InputOutputFunctions) Enum.Parse(typeof (InputOutputFunctions), inputOutputResources[index], true);
            break;
          }
        }
        S3_Meter.AddToConfigParamList(ConfigParamList, ConfigParam1, !flag3 | flag1, useRights);
      }
      else
      {
        ConfigParam1.ParameterValue = (object) InputOutputFunctions.Input;
        S3_Meter.AddToConfigParamList(ConfigParamList, ConfigParam1, true, useRights);
        byte byteValue = this.MyParameters.ParameterByName[S3_Meter.inputMBusSelectedListAndAddress[inputIndex]].GetByteValue();
        uint uintValue1 = this.MyParameters.ParameterByName[S3_Meter.inputMBusIdentNumber[inputIndex]].GetUintValue();
        if (this.MyTransmitParameterManager.AreVirtualDevicesUsed())
        {
          S3_Meter.AddToConfigParamList(ConfigParamList, new ConfigurationParameter(OverrideID.MBusAddress)
          {
            ParameterValue = (object) (ushort) byteValue,
            HasWritePermission = flag2
          }, useRights);
          S3_Meter.AddToConfigParamList(ConfigParamList, new ConfigurationParameter(OverrideID.SerialNumber)
          {
            ParameterValue = (object) uintValue1.ToString("x08"),
            HasWritePermission = flag2
          }, true, useRights);
        }
        byte num = (byte) ((uint) this.MyParameters.ParameterByName[S3_Meter.inputMBusMediumGeneration[inputIndex]].GetUshortValue() >> 8);
        S3_Meter.AddToConfigParamList(ConfigParamList, new ConfigurationParameter(OverrideID.Medium)
        {
          ParameterValue = (object) (MBusDeviceType) num,
          HasWritePermission = flag2
        }, true, useRights);
        ConfigurationParameter ConfigParam2 = new ConfigurationParameter(OverrideID.InputResolutionStr);
        ConfigParam2.SetValueFromStringDb(inputData.inputResolutionString);
        ConfigParam2.AllowedValues = LcdUnitsC2.AllInputUnits;
        ConfigParam2.HasWritePermission = flag2;
        S3_Meter.AddToConfigParamList(ConfigParamList, ConfigParam2, true, useRights);
        S3_Meter.AddToConfigParamList(ConfigParamList, new ConfigurationParameter(OverrideID.InputPulsValue)
        {
          ParameterValue = (object) (double) (inputData.impulsValueDecimal * inputData.inputUnitInfo.MeterPulsValue_To_DisplayPulsValue_Factor),
          HasWritePermission = flag2,
          Unit = inputData.inputUnitInfo.PulsValueUnitString
        }, true, useRights);
        Decimal uintValue2 = (Decimal) this.MyParameters.ParameterByName[S3_Meter.inputActualValue[inputIndex]].GetUintValue();
        S3_Meter.AddToConfigParamList(ConfigParamList, new ConfigurationParameter(OverrideID.InputActualValue)
        {
          ParameterValue = (object) (uintValue2 / inputData.inputUnitInfo.displayFactor),
          Unit = inputData.inputUnitInfo.displayUnitString,
          HasWritePermission = flag2
        }, true, useRights);
      }
      return ConfigParamList;
    }

    private SortedList<OverrideID, ConfigurationParameter> CurrentParameterList
    {
      get
      {
        if (this.currentParameterList == null)
        {
          this.currentParameterList = new SortedList<OverrideID, ConfigurationParameter>();
          this.GetConfigurationParameters(0, this.currentParameterList, false);
        }
        return this.currentParameterList;
      }
    }

    internal bool SetConfigurationParameterHeatMeter(
      SortedList<OverrideID, ConfigurationParameter> parameterList,
      SortedList<OverrideID, ConfigurationParameter> oldParameterList)
    {
      this.AddOverwriteHistoryItem(new OverwriteHistoryItem("HeatMeter", parameterList));
      if (this.MyResources.AvailableMeterResources.Keys.Contains("LoRa") != this.MyIdentification.IsLoRa)
        throw new Exception("Hardware info LoRa out of firmware not equal to HardwareType resource LoRa");
      this.currentParameterList = (SortedList<OverrideID, ConfigurationParameter>) null;
      this.resetAllValues = false;
      this.clearAllLogger = false;
      this.setMbusPrimAdrFromSerialNumber = false;
      int index1 = parameterList.IndexOfKey(OverrideID.SerialNumber);
      if (index1 >= 0)
      {
        ConfigurationParameter configurationParameter = parameterList.Values[index1];
        S3_Parameter s3Parameter = this.MyParameters.ParameterByName["SerDev0_IdentNo"];
        uint result;
        if (Util.TryParse(configurationParameter.GetStringValueDb(), NumberStyles.HexNumber, (IFormatProvider) null, out result))
          s3Parameter.SetUintValue(result);
      }
      int index2 = parameterList.IndexOfKey(OverrideID.VolMeterFlowPosition);
      S3_ParameterNames s3ParameterNames;
      if (index2 >= 0)
      {
        ConfigurationParameter configurationParameter = parameterList.Values[index2];
        S3_Parameter s3Parameter = this.MyParameters.ParameterByName[S3_ParameterNames.Device_Setup_2.ToString()];
        int NewValue = (int) s3Parameter.GetUshortValue() & -10;
        if ((bool) configurationParameter.ParameterValue)
          NewValue |= 1;
        s3Parameter.SetUshortValue((ushort) NewValue);
        SortedList<string, S3_Parameter> parameterByName1 = this.MyParameters.ParameterByName;
        s3ParameterNames = S3_ParameterNames.Heap_SelectTime;
        string key1 = s3ParameterNames.ToString();
        if (parameterByName1.ContainsKey(key1))
        {
          SortedList<string, S3_Parameter> parameterByName2 = this.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.Heap_SelectTime;
          string key2 = s3ParameterNames.ToString();
          parameterByName2[key2].SetUintValue(0U);
        }
      }
      int index3 = parameterList.IndexOfKey(OverrideID.VolMeterFlowPositionByUser);
      if (index3 >= 0)
      {
        SortedList<string, S3_Parameter> parameterByName3 = this.MyParameters.ParameterByName;
        s3ParameterNames = S3_ParameterNames.Device_Setup_2;
        string key3 = s3ParameterNames.ToString();
        S3_Parameter s3Parameter = parameterByName3[key3];
        int num = (int) s3Parameter.GetUshortValue() & -10;
        ConfigurationParameter configurationParameter = parameterList.Values[index3];
        int NewValue;
        if ((string) configurationParameter.ParameterValue == S3VolMeterPositions.outlet.ToString())
        {
          NewValue = num | 0;
        }
        else
        {
          string parameterValue1 = (string) configurationParameter.ParameterValue;
          S3VolMeterPositions volMeterPositions = S3VolMeterPositions.inlet;
          string str1 = volMeterPositions.ToString();
          if (parameterValue1 == str1)
          {
            NewValue = num | 1;
          }
          else
          {
            string parameterValue2 = (string) configurationParameter.ParameterValue;
            volMeterPositions = S3VolMeterPositions.selectable;
            string str2 = volMeterPositions.ToString();
            if (!(parameterValue2 == str2))
              throw new Exception("This value is not allowed for flowline: " + configurationParameter.ParameterValue?.ToString());
            NewValue = num | 9;
          }
        }
        s3Parameter.SetUshortValue((ushort) NewValue);
        SortedList<string, S3_Parameter> parameterByName4 = this.MyParameters.ParameterByName;
        s3ParameterNames = S3_ParameterNames.Heap_SelectTime;
        string key4 = s3ParameterNames.ToString();
        if (parameterByName4.ContainsKey(key4))
        {
          SortedList<string, S3_Parameter> parameterByName5 = this.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.Heap_SelectTime;
          string key5 = s3ParameterNames.ToString();
          parameterByName5[key5].SetUintValue(0U);
        }
      }
      int index4 = parameterList.IndexOfKey(OverrideID.OutletTempSensorInVolumeMeter);
      if (index4 >= 0)
      {
        ConfigurationParameter configurationParameter = parameterList.Values[index4];
        SortedList<string, S3_Parameter> parameterByName = this.MyParameters.ParameterByName;
        s3ParameterNames = S3_ParameterNames.Device_Setup_2;
        string key = s3ParameterNames.ToString();
        S3_Parameter s3Parameter = parameterByName[key];
        int ushortValue = (int) s3Parameter.GetUshortValue();
        int NewValue = (ushortValue & 8) == 0 ? ushortValue & -65 : (!(bool) configurationParameter.ParameterValue ? ushortValue & -65 : ushortValue | 64);
        s3Parameter.SetUshortValue((ushort) NewValue);
      }
      int index5 = parameterList.IndexOfKey(OverrideID.TempRangeUpperLimit);
      if (index5 >= 0)
      {
        SortedList<string, S3_Parameter> parameterByName = this.MyParameters.ParameterByName;
        s3ParameterNames = S3_ParameterNames.Con_Temp_Display_Range_Max;
        string key = s3ParameterNames.ToString();
        int index6 = parameterByName.IndexOfKey(key);
        if (index6 >= 0)
        {
          short NewValue = (short) ((Decimal) parameterList.Values[index5].ParameterValue * 100M);
          this.MyParameters.ParameterByName.Values[index6].SetShortValue(NewValue);
        }
      }
      int index7 = parameterList.IndexOfKey(OverrideID.TempRangeLowerLimit);
      if (index7 >= 0)
      {
        SortedList<string, S3_Parameter> parameterByName = this.MyParameters.ParameterByName;
        s3ParameterNames = S3_ParameterNames.Con_Temp_Display_Range_Min;
        string key = s3ParameterNames.ToString();
        int index8 = parameterByName.IndexOfKey(key);
        if (index8 >= 0)
        {
          short NewValue = (short) ((Decimal) parameterList.Values[index7].ParameterValue * 100M);
          this.MyParameters.ParameterByName.Values[index8].SetShortValue(NewValue);
        }
      }
      int index9 = parameterList.IndexOfKey(OverrideID.ChangeOver);
      ConfigurationParameter.ChangeOverValues changeOverValues1;
      S3_MeterResources s3MeterResources;
      if (index9 >= 0)
      {
        changeOverValues1 = (ConfigurationParameter.ChangeOverValues) parameterList.Values[index9].ParameterValue;
        S3_Parameter s3Parameter = this.MyParameters.ParameterByName["Device_Setup"];
        int num = (int) s3Parameter.GetUshortValue() & -769;
        MeterResources resources1 = this.MyResources;
        s3MeterResources = S3_MeterResources.Cooling_Active;
        string resourceName1 = s3MeterResources.ToString();
        resources1.DeleteResource(resourceName1, 0);
        MeterResources resources2 = this.MyResources;
        s3MeterResources = S3_MeterResources.Heating_Active;
        string resourceName2 = s3MeterResources.ToString();
        resources2.DeleteResource(resourceName2, 0);
        MeterResources resources3 = this.MyResources;
        s3MeterResources = S3_MeterResources.OnlyCooling_Active;
        string resourceName3 = s3MeterResources.ToString();
        resources3.DeleteResource(resourceName3, 0);
        int NewValue;
        switch (changeOverValues1)
        {
          case ConfigurationParameter.ChangeOverValues.ChangeOver:
            MeterResources resources4 = this.MyResources;
            ConfigurationParameter.ChangeOverValues changeOverValues2 = ConfigurationParameter.ChangeOverValues.Cooling;
            string res1 = changeOverValues2.ToString();
            if (!resources4.IsResourceAvailable(res1))
              return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.OperationCancelled, "This C5 hardware can not be used as change over meter! The cooling is not supported.");
            MeterResources resources5 = this.MyResources;
            changeOverValues2 = ConfigurationParameter.ChangeOverValues.Heating;
            string res2 = changeOverValues2.ToString();
            if (!resources5.IsResourceAvailable(res2))
              return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.OperationCancelled, "This C5 hardware can not be used as change over meter! The heating is not supported.");
            NewValue = num | 768;
            MeterResources resources6 = this.MyResources;
            s3MeterResources = S3_MeterResources.Cooling_Active;
            string resourceName4 = s3MeterResources.ToString();
            resources6.AddResource(resourceName4, 0);
            MeterResources resources7 = this.MyResources;
            s3MeterResources = S3_MeterResources.Heating_Active;
            string resourceName5 = s3MeterResources.ToString();
            resources7.AddResource(resourceName5, 0);
            break;
          case ConfigurationParameter.ChangeOverValues.Cooling:
            if (!this.MyResources.IsResourceAvailable(ConfigurationParameter.ChangeOverValues.Cooling.ToString()))
              return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.OperationCancelled, "This C5 hardware can not be used as cooling meter!");
            NewValue = num | 512;
            MeterResources resources8 = this.MyResources;
            s3MeterResources = S3_MeterResources.Cooling_Active;
            string resourceName6 = s3MeterResources.ToString();
            resources8.AddResource(resourceName6, 0);
            MeterResources resources9 = this.MyResources;
            s3MeterResources = S3_MeterResources.OnlyCooling_Active;
            string resourceName7 = s3MeterResources.ToString();
            resources9.AddResource(resourceName7, 0);
            break;
          default:
            if (!this.MyResources.IsResourceAvailable(ConfigurationParameter.ChangeOverValues.Heating.ToString()))
              return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.OperationCancelled, "This C5 hardware can not be used as heating meter!");
            NewValue = num | 256;
            MeterResources resources10 = this.MyResources;
            s3MeterResources = S3_MeterResources.Heating_Active;
            string resourceName8 = s3MeterResources.ToString();
            resources10.AddResource(resourceName8, 0);
            break;
        }
        s3Parameter.SetUshortValue((ushort) NewValue);
      }
      else
        changeOverValues1 = this.GetChangeoverSetup();
      int index10 = parameterList.IndexOfKey(OverrideID.EnergyResolution);
      if (index10 >= 0)
        this.MyMeterScaling.SetEnergyResolution(parameterList.Values[index10].GetStringValueDb());
      int index11 = parameterList.IndexOfKey(OverrideID.EnergyActualValue);
      if (index11 >= 0)
      {
        this.MyParameters.ParameterByName["Bak_HeatEnergySum"].SetDoubleValue((double) ((Decimal) parameterList.Values[index11].ParameterValue / this.MyMeterScaling.energyLcdToBaseUnitFactor));
        this.clearAllLogger = true;
      }
      if (changeOverValues1 == ConfigurationParameter.ChangeOverValues.Cooling || changeOverValues1 == ConfigurationParameter.ChangeOverValues.ChangeOver)
      {
        int index12 = parameterList.IndexOfKey(OverrideID.CEnergyActualValue);
        if (index12 >= 0)
        {
          this.MyParameters.ParameterByName["Bak_ColdEnergySum"].SetDoubleValue((double) ((Decimal) parameterList.Values[index12].ParameterValue / this.MyMeterScaling.energyLcdToBaseUnitFactor));
          this.clearAllLogger = true;
        }
      }
      int index13 = parameterList.IndexOfKey(OverrideID.VolumeResolution);
      if (index13 >= 0)
        this.MyMeterScaling.SetVolumeResolution(parameterList.Values[index13].GetStringValueDb());
      int index14 = parameterList.IndexOfKey(OverrideID.VolumePulsValue);
      if (index14 >= 0)
        this.MyMeterScaling.SetVolumePulsValue(parameterList.Values[index14].GetStringValueWin());
      int index15 = parameterList.IndexOfKey(OverrideID.VolumeActualValue);
      if (index15 >= 0)
      {
        this.MyParameters.ParameterByName["Bak_VolSum"].SetDoubleValue((double) ((Decimal) parameterList.Values[index15].ParameterValue / this.MyMeterScaling.volumeLcdToQmFactor));
        this.clearAllLogger = true;
      }
      ConfigurationParameter configurationParameter1 = (ConfigurationParameter) null;
      int index16 = parameterList.IndexOfKey(OverrideID.ShowVolumeAsMass);
      if (index16 >= 0)
      {
        configurationParameter1 = parameterList.Values[index16];
      }
      else
      {
        int index17 = oldParameterList.IndexOfKey(OverrideID.ShowVolumeAsMass);
        if (index17 >= 0)
          configurationParameter1 = oldParameterList.Values[index17];
      }
      if (configurationParameter1 != null && (bool) configurationParameter1.ParameterValue)
        this.MyResources.AddResource("MassActive");
      else
        this.MyResources.DeleteResource("MassActive");
      SortedList<string, S3_Parameter> parameterByName6 = this.MyParameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.VolInputSetup;
      string key6 = s3ParameterNames.ToString();
      int index18 = parameterByName6.IndexOfKey(key6);
      if (index18 >= 0)
      {
        int index19 = parameterList.IndexOfKey(OverrideID.InputMode);
        if (index19 >= 0)
        {
          ConfigurationParameter configurationParameter2 = parameterList.Values[index19];
          this.MyParameters.ParameterByName.Values[index18].SetByteValue((byte) configurationParameter2.ParameterValue);
        }
      }
      int index20 = parameterList.IndexOfKey(OverrideID.DeviceClock);
      if (index20 >= 0)
        this.MeterTimeAsDateTime = (DateTime) parameterList.Values[index20].ParameterValue;
      int index21 = parameterList.IndexOfKey(OverrideID.TimeZone);
      if (index21 >= 0)
        this.TimeZoneHourOffset = (double) (Decimal) parameterList.Values[index21].ParameterValue;
      int index22 = parameterList.IndexOfKey(OverrideID.SetPcTime);
      if (index22 >= 0)
        this.MyFunctions.usePcTime = (bool) parameterList.Values[index22].ParameterValue;
      if (this.MyFunctions.usePcTime)
        this.MeterTimeAsPcTime = DateTime.Now;
      int num1 = -1;
      int num2 = -1;
      ConfigurationParameter configurationParameter3 = (ConfigurationParameter) null;
      int? nullable1 = new int?();
      RadioScenario radioScenario = RadioScenario.Scenario_NotDefined;
      if (!this.MyIdentification.IsWR4)
      {
        int index23 = parameterList.IndexOfKey(OverrideID.CycleTimeVolume);
        if (index23 >= 0)
        {
          configurationParameter3 = parameterList.Values[index23];
          num1 = 2 * (int) (ulong) configurationParameter3.ParameterValue;
        }
        if (this.MyResources.AvailableMeterResources.Keys.Contains("Radio"))
        {
          int index24 = parameterList.IndexOfKey(OverrideID.CycleTimeRadio);
          if (index24 >= 0)
          {
            nullable1 = new int?((int) TimeSpan.Parse((string) parameterList.Values[index24].ParameterValue).TotalSeconds);
            num2 = 2 * nullable1.Value;
          }
        }
        else if (this.MyIdentification.IsLoRa)
        {
          int index25 = parameterList.IndexOfKey(OverrideID.RadioScenario);
          if (index25 >= 0)
          {
            radioScenario = (RadioScenario) parameterList.Values[index25].ParameterValue;
            if (radioScenario != RadioScenario.Scenario_202_LoRaDaily && radioScenario != RadioScenario.Scenario_201_LoRaMonthly)
              throw new Exception("Illegal radio scenario");
            LoRa_TransmissionScenario NewValue = (LoRa_TransmissionScenario) (radioScenario - 200);
            SortedList<string, S3_Parameter> parameterByName7 = this.MyParameters.ParameterByName;
            s3ParameterNames = S3_ParameterNames.cfg_transmission_scenario;
            string key7 = s3ParameterNames.ToString();
            parameterByName7[key7].SetByteValue((byte) NewValue);
          }
          int index26 = parameterList.IndexOfKey(OverrideID.CycleTimeRadio);
          if (index26 >= 0)
          {
            string parameterValue = (string) parameterList.Values[index26].ParameterValue;
            if ((radioScenario != RadioScenario.Scenario_202_LoRaDaily || !(parameterValue == "daily")) && (radioScenario != RadioScenario.Scenario_201_LoRaMonthly || !(parameterValue == "monthly")))
              throw new Exception("RadioScenario and CycleTimeRadio don't fit together");
          }
        }
      }
      ConfigurationParameter configurationParameter4 = (ConfigurationParameter) null;
      int index27 = parameterList.IndexOfKey(OverrideID.CycleTimeStandard);
      int num3 = -1;
      if (index27 >= 0)
      {
        configurationParameter4 = parameterList.Values[index27];
        num3 = 2 * (int) (ulong) configurationParameter4.ParameterValue;
      }
      int index28 = parameterList.IndexOfKey(OverrideID.CycleTimeFast);
      int num4 = -1;
      if (index28 >= 0)
        num4 = 2 * (int) (ulong) parameterList.Values[index28].ParameterValue;
      if (num1 > 0 || num4 > 0 || num3 >= 0 || num2 > 0)
      {
        S3_Parameter s3Parameter1 = this.MyParameters.ParameterByName["temperaturCycleTimeCounterInit"];
        int byteValue1 = (int) s3Parameter1.GetByteValue();
        S3_Parameter s3Parameter2 = this.MyParameters.ParameterByName["temperaturCycleTimeSlotCounterInit"];
        int byteValue2 = (int) s3Parameter2.GetByteValue();
        int num5 = 1;
        S3_Parameter s3Parameter3 = (S3_Parameter) null;
        S3_Parameter s3Parameter4 = (S3_Parameter) null;
        if (!this.MyIdentification.IsWR4)
        {
          SortedList<string, S3_Parameter> parameterByName8 = this.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.volumeCycleTimeCounterInit;
          string key8 = s3ParameterNames.ToString();
          s3Parameter3 = parameterByName8[key8];
          int byteValue3 = (int) s3Parameter3.GetByteValue();
          SortedList<string, S3_Parameter> parameterByName9 = this.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.radioCycleTimeCounterInit;
          string key9 = s3ParameterNames.ToString();
          if (parameterByName9.ContainsKey(key9))
          {
            SortedList<string, S3_Parameter> parameterByName10 = this.MyParameters.ParameterByName;
            s3ParameterNames = S3_ParameterNames.radioCycleTimeCounterInit;
            string key10 = s3ParameterNames.ToString();
            s3Parameter4 = parameterByName10[key10];
            num5 = (int) s3Parameter4.GetUshortValue();
          }
          if (num1 < 0)
            num1 = byteValue3;
          if (num4 < 0)
            num4 = byteValue3 * byteValue1;
          if (num3 < 0)
            num3 = byteValue3 * byteValue1 * byteValue2;
          if (num2 < 0)
            num2 = byteValue3 * num5;
          MeterResources resources = this.MyResources;
          s3MeterResources = S3_MeterResources.IUF;
          string res = s3MeterResources.ToString();
          if (!resources.IsResourceAvailable(res))
            num1 = num4;
          if (num1 < 2)
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal volume cycle time time. Min: 1 Sec.", S3_Meter.S3_MeterLogger);
          if (num4 < num1)
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal fast cycle time time. (< volume cycle time )", S3_Meter.S3_MeterLogger);
          if (num4 % num1 != 0)
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal fast cycle time. Factor from volume cycle time not integer.", S3_Meter.S3_MeterLogger);
        }
        else
        {
          if (num4 < 0)
            num4 = byteValue1;
          if (num3 < 0)
            num3 = byteValue1 * byteValue2;
        }
        if (num4 < 4)
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal fast cycle time time. Min: 2 Sec.", S3_Meter.S3_MeterLogger);
        if (num3 < num4)
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal slow cycle time time. (< fast cycle time )", S3_Meter.S3_MeterLogger);
        if (num3 % num4 != 0)
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal slow cycle time. Factor from fast cycle time not integer.", S3_Meter.S3_MeterLogger);
        int NewValue1;
        int NewValue2;
        if (!this.MyIdentification.IsWR4)
        {
          int NewValue3 = num1;
          NewValue1 = num4 / num1;
          NewValue2 = num3 / num4;
          s3Parameter3.SetByteValue((byte) NewValue3);
          if (this.MyResources.IsResourceAvailable("Radio"))
          {
            if (num1 < 4 || num1 > 20)
              return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal volume cycle time for Radio. Min: 2 Sec. - Max: 10 Sec.", S3_Meter.S3_MeterLogger);
            if (num2 % num1 != 0)
              return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal radio cycle time. Factor from volume cycle time not integer.", S3_Meter.S3_MeterLogger);
            int NewValue4 = num2 / num1;
            s3Parameter4?.SetUshortValue((ushort) NewValue4);
          }
        }
        else
        {
          NewValue1 = num4;
          NewValue2 = num3 / num4;
        }
        s3Parameter1.SetByteValue((byte) NewValue1);
        s3Parameter2.SetByteValue((byte) NewValue2);
      }
      int index29 = parameterList.IndexOfKey(OverrideID.OperatingHours);
      if (index29 >= 0)
        this.MyParameters.ParameterByName["StartOfOperation"].SetUintValue(this.MeterTimeAsSeconds1980 - (uint) (ulong) parameterList.Values[index29].ParameterValue * 3600U);
      int index30 = parameterList.IndexOfKey(OverrideID.EndOfCalibration);
      if (index30 >= 0)
        this.MyParameters.ParameterByName["EndOfCalibration"].SetDateTimeValue(new DateTime((int) parameterList.Values[index30].ParameterValue, 1, 1));
      MeterResources resources11 = this.MyResources;
      s3MeterResources = S3_MeterResources.Radio;
      string res3 = s3MeterResources.ToString();
      if (resources11.IsResourceAvailable(res3))
      {
        int index31 = parameterList.IndexOfKey(OverrideID.SelectedRadioList);
        if (index31 >= 0)
        {
          this.MyTransmitParameterManager.Transmitter.Radio.Set_ActivatedList((string) parameterList.Values[index31].ParameterValue);
        }
        else
        {
          int index32 = oldParameterList.IndexOfKey(OverrideID.SelectedRadioList);
          if (index32 >= 0)
            this.MyTransmitParameterManager.Transmitter.Radio.Set_ActivatedList((string) oldParameterList.Values[index32].ParameterValue);
        }
        int index33 = parameterList.IndexOfKey(OverrideID.AESKey);
        if (index33 >= 0)
        {
          string keyName = (string) parameterList.Values[index33].ParameterValue;
          switch (keyName)
          {
            case "default":
              keyName = "ZENNER DEFAULT KEY";
              break;
            case "off":
              keyName = "OFF";
              break;
          }
          byte[] aesKey = AES.StringToAesKey(keyName);
          SortedList<string, S3_Parameter> parameterByName11 = this.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.radioAndEncMode;
          string key11 = s3ParameterNames.ToString();
          if (parameterByName11.ContainsKey(key11))
          {
            SortedList<string, S3_Parameter> parameterByName12 = this.MyParameters.ParameterByName;
            s3ParameterNames = S3_ParameterNames.radioAndEncMode;
            string key12 = s3ParameterNames.ToString();
            byte byteValue = parameterByName12[key12].GetByteValue();
            if (aesKey == null)
            {
              byte NewValue = (byte) ((uint) byteValue & 240U);
              SortedList<string, S3_Parameter> parameterByName13 = this.MyParameters.ParameterByName;
              s3ParameterNames = S3_ParameterNames.radioAndEncMode;
              string key13 = s3ParameterNames.ToString();
              parameterByName13[key13].SetByteValue(NewValue);
              this.SetEncryptionKey(new byte[16]);
            }
            else
            {
              if (((int) byteValue & 15) == 0)
              {
                byte NewValue = (byte) ((uint) byteValue | 5U);
                SortedList<string, S3_Parameter> parameterByName14 = this.MyParameters.ParameterByName;
                s3ParameterNames = S3_ParameterNames.radioAndEncMode;
                string key14 = s3ParameterNames.ToString();
                parameterByName14[key14].SetByteValue(NewValue);
              }
              this.SetEncryptionKey(aesKey);
            }
          }
          else if (aesKey != null)
          {
            this.SetEncryptionKey(aesKey);
            SortedList<string, S3_Parameter> parameterByName15 = this.MyParameters.ParameterByName;
            s3ParameterNames = S3_ParameterNames.DeviceSetupNp;
            string key15 = s3ParameterNames.ToString();
            S3_Parameter s3Parameter = parameterByName15[key15];
            int NewValue = (int) s3Parameter.GetByteValue() | 128;
            s3Parameter.SetByteValue((byte) NewValue);
          }
          else
          {
            SortedList<string, S3_Parameter> parameterByName16 = this.MyParameters.ParameterByName;
            s3ParameterNames = S3_ParameterNames.DeviceSetupNp;
            string key16 = s3ParameterNames.ToString();
            S3_Parameter s3Parameter = parameterByName16[key16];
            byte NewValue = (byte) ((ulong) s3Parameter.GetUintValue() & 18446744073709551487UL);
            s3Parameter.SetByteValue(NewValue);
          }
        }
      }
      int index34 = parameterList.IndexOfKey(OverrideID.Activation);
      if (index34 >= 0)
        this.MyParameters.ParameterByName["cfg_otaa_abp_mode"].SetByteValue((byte) Enum.Parse(typeof (OTAA_ABP), parameterList.Values[index34].GetStringValueDb()));
      int index35 = parameterList.IndexOfKey(OverrideID.TransmissionScenario);
      if (index35 >= 0)
      {
        ConfigurationParameter configurationParameter5 = parameterList.Values[index35];
        S3_Parameter s3Parameter = this.MyParameters.ParameterByName["cfg_transmission_scenario"];
        byte NewValue = Convert.ToByte(configurationParameter5.GetStringValueDb());
        if (NewValue != (byte) 1 && NewValue != (byte) 2)
          throw new Exception("The transmission scenario " + NewValue.ToString() + " is not supported!");
        s3Parameter.SetByteValue(NewValue);
      }
      DateTime dateTime = DateTime.MinValue;
      int index36 = parameterList.IndexOfKey(OverrideID.EndOfBattery);
      if (index36 >= 0)
      {
        dateTime = (DateTime) parameterList.Values[index36].ParameterValue;
        SortedList<string, S3_Parameter> parameterByName17 = this.MyParameters.ParameterByName;
        s3ParameterNames = S3_ParameterNames.EndOfBatterie;
        string key17 = s3ParameterNames.ToString();
        parameterByName17[key17].SetDateTimeValue(dateTime);
      }
      double? nullable2 = new double?();
      if (this.BatteryManager != null)
      {
        nullable2 = new double?(this.BatteryManager.BatteryCapacity_mAh);
        this.BatteryManager = (S3_BatteryEnergyManagement) null;
      }
      int index37 = parameterList.IndexOfKey(OverrideID.BatteryCapacity_mAh);
      if (index37 >= 0)
        nullable2 = new double?((double) parameterList.Values[index37].ParameterValue);
      if (nullable2.HasValue && nullable2.Value > 0.0)
      {
        if (dateTime == DateTime.MinValue)
          dateTime = this.MyParameters.ParameterByName["EndOfBatterie"].GetDateTimeValue();
        ulong volumeCycle_s = 0;
        if (configurationParameter3 != null)
          volumeCycle_s = (ulong) configurationParameter3.ParameterValue;
        else if (oldParameterList != null && oldParameterList.ContainsKey(OverrideID.CycleTimeVolume))
          volumeCycle_s = (ulong) oldParameterList[OverrideID.CycleTimeVolume].ParameterValue;
        else if (this.CurrentParameterList.ContainsKey(OverrideID.CycleTimeVolume))
          volumeCycle_s = (ulong) oldParameterList[OverrideID.CycleTimeVolume].ParameterValue;
        ulong energyCycle_s = 0;
        if (configurationParameter4 != null)
          energyCycle_s = (ulong) configurationParameter4.ParameterValue;
        else if (oldParameterList != null && oldParameterList.ContainsKey(OverrideID.CycleTimeStandard))
          energyCycle_s = (ulong) oldParameterList[OverrideID.CycleTimeStandard].ParameterValue;
        else if (this.CurrentParameterList.ContainsKey(OverrideID.CycleTimeStandard))
          energyCycle_s = (ulong) oldParameterList[OverrideID.CycleTimeStandard].ParameterValue;
        ulong radioCycle_s = 0;
        if (radioScenario == RadioScenario.Scenario_NotDefined)
        {
          if (!nullable1.HasValue)
          {
            int index38 = oldParameterList.IndexOfKey(OverrideID.CycleTimeRadio);
            if (index38 >= 0)
              nullable1 = new int?((int) TimeSpan.Parse((string) oldParameterList.Values[index38].ParameterValue).TotalSeconds);
          }
          if (nullable1.HasValue)
            radioCycle_s = (ulong) nullable1.Value;
          else if (oldParameterList != null && oldParameterList.ContainsKey(OverrideID.RadioScenario))
            radioScenario = (RadioScenario) oldParameterList[OverrideID.RadioScenario].ParameterValue;
          else if (this.CurrentParameterList.ContainsKey(OverrideID.RadioScenario))
            radioScenario = (RadioScenario) this.CurrentParameterList[OverrideID.RadioScenario].ParameterValue;
        }
        if (radioScenario == RadioScenario.Scenario_202_LoRaDaily)
          radioCycle_s = 86400UL;
        else if (radioScenario == RadioScenario.Scenario_201_LoRaMonthly)
          radioCycle_s = 2592000UL;
        else if (radioScenario == RadioScenario.Scenario_203_LoRaHourly)
          throw new Exception("Hourly scenario not supported");
        this.BatteryManager = new S3_BatteryEnergyManagement(this.MyIdentification.HardwareMask, nullable2.Value, (double) radioCycle_s, (double) volumeCycle_s, (double) energyCycle_s, dateTime, this);
        SortedList<string, S3_Parameter> parameterByName18 = this.MyParameters.ParameterByName;
        s3ParameterNames = S3_ParameterNames.lora_diagnostic_remaining_messages;
        string key18 = s3ParameterNames.ToString();
        if (parameterByName18.ContainsKey(key18) && radioCycle_s > 0UL)
        {
          SortedList<string, S3_Parameter> parameterByName19 = this.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.lora_diagnostic_remaining_messages;
          string key19 = s3ParameterNames.ToString();
          parameterByName19[key19].SetUintValue(this.BatteryManager.LoRaDiagnosticCounts.Value);
        }
      }
      int index39 = parameterList.IndexOfKey(OverrideID.DueDateMonth);
      if (index39 >= 0)
        this.MyParameters.ParameterByName["Bak_DueDateMonth"].SetByteValue(Convert.ToByte(parameterList.Values[index39].ParameterValue));
      int index40 = parameterList.IndexOfKey(OverrideID.DueDate);
      if (index40 >= 0)
      {
        ConfigurationParameter configurationParameter6 = parameterList.Values[index40];
        S3_Parameter s3Parameter = this.MyParameters.ParameterByName["Bak_DueDateMonth"];
        DateTime parameterValue = (DateTime) configurationParameter6.ParameterValue;
        if (parameterValue.Day == 1)
        {
          s3Parameter.SetByteValue((byte) parameterValue.Month);
        }
        else
        {
          if (parameterValue.Day != 16)
            throw new Exception("Only day 1 or 16 is allowed for DueDate");
          s3Parameter.SetByteValue((byte) (parameterValue.Month + 16));
        }
      }
      int index41 = parameterList.IndexOfKey(OverrideID.MBusAddress);
      if (index41 >= 0)
      {
        object parameterValue = parameterList.Values[index41].ParameterValue;
        ushort result;
        if (parameterValue != null && Util.TryParse(parameterValue.ToString(), out result))
        {
          if (result < (ushort) 0 || result > (ushort) byte.MaxValue)
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.OperationCancelled, "Invalid M-Bus address! Please add value between 0 and 254.");
          S3_Parameter s3Parameter = this.MyParameters.ParameterByName["SerDev0_SelectedList_PrimaryAddress"];
          ushort NewValue = (ushort) ((uint) s3Parameter.GetUshortValue() & 65280U | (uint) result);
          s3Parameter.SetUshortValue(NewValue);
        }
      }
      int index42 = parameterList.IndexOfKey(OverrideID.MBusThirdPartySupport);
      if (index42 >= 0)
        this.MyTransmitParameterManager.SetMBusThirdPartySupportState(Convert.ToBoolean(parameterList.Values[index42].ParameterValue));
      else if (oldParameterList != null)
      {
        int index43 = oldParameterList.IndexOfKey(OverrideID.MBusThirdPartySupport);
        if (index43 >= 0)
          this.MyTransmitParameterManager.SetMBusThirdPartySupportState(Convert.ToBoolean(oldParameterList.Values[index43].ParameterValue));
      }
      if (!this.IsWriteProtected)
      {
        int index44 = parameterList.IndexOfKey(OverrideID.SetWriteProtection);
        if (index44 >= 0)
          this.MyFunctions.MyMeters.SetWriteProtectionOnWrite = (bool) parameterList.Values[index44].ParameterValue;
        int index45 = parameterList.IndexOfKey(OverrideID.SetSleepMode);
        if (index45 >= 0)
          this.MyFunctions.MyMeters.SetSleepModeOnWrite = (bool) parameterList.Values[index45].ParameterValue;
        int index46 = parameterList.IndexOfKey(OverrideID.OrderNumber);
        if (index46 >= 0)
        {
          ConfigurationParameter configurationParameter7 = parameterList.Values[index46];
          SortedList<string, S3_Parameter> parameterByName20 = this.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.Con_SAP_ProductionOrderNumber;
          string key20 = s3ParameterNames.ToString();
          parameterByName20[key20].SetUintValue((uint) (ulong) configurationParameter7.ParameterValue);
        }
      }
      int index47 = parameterList.IndexOfKey(OverrideID.ResetAllValues);
      if (index47 >= 0)
        this.resetAllValues = (bool) parameterList.Values[index47].ParameterValue;
      int index48 = parameterList.IndexOfKey(OverrideID.ClearAllLoggers);
      if (index48 >= 0)
        this.clearAllLogger = (bool) parameterList.Values[index48].ParameterValue;
      int index49 = parameterList.IndexOfKey(OverrideID.SetMbusPrimAdrFromSerialNumber);
      if (index49 >= 0)
        this.setMbusPrimAdrFromSerialNumber = (bool) parameterList.Values[index49].ParameterValue;
      ConfigurationParameter configurationParameter8 = (ConfigurationParameter) null;
      int index50 = parameterList.IndexOfKey(OverrideID.ShowEnergyChecker);
      if (index50 >= 0)
      {
        configurationParameter8 = parameterList.Values[index50];
      }
      else
      {
        int index51 = oldParameterList.IndexOfKey(OverrideID.ShowEnergyChecker);
        if (index51 >= 0)
          configurationParameter8 = oldParameterList.Values[index51];
      }
      if (configurationParameter8 != null && (bool) configurationParameter8.ParameterValue)
        this.MyResources.AddResource("CheckerActive");
      else
        this.MyResources.DeleteResource("CheckerActive");
      ConfigurationParameter configurationParameter9 = (ConfigurationParameter) null;
      int index52 = parameterList.IndexOfKey(OverrideID.ShowGCAL);
      if (index52 >= 0)
      {
        configurationParameter9 = parameterList.Values[index52];
      }
      else
      {
        int index53 = oldParameterList.IndexOfKey(OverrideID.ShowGCAL);
        if (index53 >= 0)
          configurationParameter9 = oldParameterList.Values[index53];
      }
      if (configurationParameter9 != null && (bool) configurationParameter9.ParameterValue)
        this.MyResources.AddResource("GCalActive");
      else
        this.MyResources.DeleteResource("GCalActive");
      SortedList<string, S3_Parameter> parameterByName21 = this.MyParameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.Heap_GlycolTableIndex;
      string key21 = s3ParameterNames.ToString();
      if (parameterByName21.ContainsKey(key21))
      {
        ConfigurationParameter configurationParameter10 = (ConfigurationParameter) null;
        int index54 = parameterList.IndexOfKey(OverrideID.Glycol);
        if (index54 >= 0)
        {
          configurationParameter10 = parameterList.Values[index54];
        }
        else
        {
          int index55 = oldParameterList.IndexOfKey(OverrideID.Glycol);
          if (index55 >= 0)
            configurationParameter10 = oldParameterList.Values[index55];
        }
        if (configurationParameter10 != null && (string) configurationParameter10.ParameterValue != "off")
        {
          byte num6 = (byte) ((IEnumerable<string>) S3_Meter.glycolSettings).ToList<string>().IndexOf((string) configurationParameter10.ParameterValue);
          SortedList<string, S3_Parameter> parameterByName22 = this.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.Heap_GlycolTableIndex;
          string key22 = s3ParameterNames.ToString();
          S3_Parameter s3Parameter = parameterByName22[key22];
          s3Parameter.PreparedParameterValue = new byte[s3Parameter.ByteSize];
          s3Parameter.PreparedParameterValue[0] = num6;
          this.MyResources.AddResource("GlycolActive");
        }
        else
          this.MyResources.DeleteResource("GlycolActive");
      }
      CalibrationValues calibrationValues = new CalibrationValues();
      int index56 = parameterList.IndexOfKey(OverrideID.CalVolMaxFlowLiterPerHour);
      if (index56 >= 0)
        calibrationValues.CalVolMaxFlowLiterPerHour = (double) parameterList.Values[index56].ParameterValue;
      int index57 = parameterList.IndexOfKey(OverrideID.CalVolMaxErrorPercent);
      if (index57 >= 0)
        calibrationValues.CalVolMaxErrorPercent = (double) parameterList.Values[index57].ParameterValue;
      int index58 = parameterList.IndexOfKey(OverrideID.CalVolNominalFlowLiterPerHour);
      if (index58 >= 0)
        calibrationValues.CalVolNominalFlowLiterPerHour = (double) parameterList.Values[index58].ParameterValue;
      int index59 = parameterList.IndexOfKey(OverrideID.CalVolNominalErrorPercent);
      if (index59 >= 0)
        calibrationValues.CalVolNominalErrorPercent = (double) parameterList.Values[index59].ParameterValue;
      int index60 = parameterList.IndexOfKey(OverrideID.CalVolMinFlowLiterPerHour);
      if (index60 >= 0)
        calibrationValues.CalVolMinFlowLiterPerHour = (double) parameterList.Values[index60].ParameterValue;
      int index61 = parameterList.IndexOfKey(OverrideID.CalVolMinErrorPercent);
      if (index61 >= 0)
        calibrationValues.CalVolMinErrorPercent = (double) parameterList.Values[index61].ParameterValue;
      int index62 = parameterList.IndexOfKey(OverrideID.CalFlowTempMinGrad);
      if (index62 >= 0)
        calibrationValues.CalFlowTempMinGrad = (double) parameterList.Values[index62].ParameterValue;
      int index63 = parameterList.IndexOfKey(OverrideID.CalFlowTempMinErrorPercent);
      if (index63 >= 0)
        calibrationValues.CalFlowTempMinErrorPercent = (double) parameterList.Values[index63].ParameterValue;
      int index64 = parameterList.IndexOfKey(OverrideID.CalFlowTempMiddleGrad);
      if (index64 >= 0)
        calibrationValues.CalFlowTempMiddleGrad = (double) parameterList.Values[index64].ParameterValue;
      int index65 = parameterList.IndexOfKey(OverrideID.CalFlowTempMiddleErrorPercent);
      if (index65 >= 0)
        calibrationValues.CalFlowTempMiddleErrorPercent = (double) parameterList.Values[index65].ParameterValue;
      int index66 = parameterList.IndexOfKey(OverrideID.CalFlowTempMaxGrad);
      if (index66 >= 0)
        calibrationValues.CalFlowTempMaxGrad = (double) parameterList.Values[index66].ParameterValue;
      int index67 = parameterList.IndexOfKey(OverrideID.CalFlowTempMaxErrorPercent);
      if (index67 >= 0)
        calibrationValues.CalFlowTempMaxErrorPercent = (double) parameterList.Values[index67].ParameterValue;
      int index68 = parameterList.IndexOfKey(OverrideID.CalReturnTempMinGrad);
      if (index68 >= 0)
        calibrationValues.CalReturnTempMinGrad = (double) parameterList.Values[index68].ParameterValue;
      int index69 = parameterList.IndexOfKey(OverrideID.CalReturnTempMinErrorPercent);
      if (index69 >= 0)
        calibrationValues.CalReturnTempMinErrorPercent = (double) parameterList.Values[index69].ParameterValue;
      int index70 = parameterList.IndexOfKey(OverrideID.CalReturnTempMiddleGrad);
      if (index70 >= 0)
        calibrationValues.CalReturnTempMiddleGrad = (double) parameterList.Values[index70].ParameterValue;
      int index71 = parameterList.IndexOfKey(OverrideID.CalReturnTempMiddleErrorPercent);
      if (index71 >= 0)
        calibrationValues.CalReturnTempMiddleErrorPercent = (double) parameterList.Values[index71].ParameterValue;
      int index72 = parameterList.IndexOfKey(OverrideID.CalReturnTempMaxGrad);
      if (index72 >= 0)
        calibrationValues.CalReturnTempMaxGrad = (double) parameterList.Values[index72].ParameterValue;
      int index73 = parameterList.IndexOfKey(OverrideID.CalReturnTempMaxErrorPercent);
      if (index73 >= 0)
        calibrationValues.CalReturnTempMaxErrorPercent = (double) parameterList.Values[index73].ParameterValue;
      int index74 = parameterList.IndexOfKey(OverrideID.TestVolumeSimulation);
      if (index74 >= 0)
      {
        this.TestVolumeSimulationValue = new double?((double) parameterList.Values[index74].ParameterValue);
        if (this.TestVolumeSimulationValue.Value == 0.0)
          this.TestVolumeSimulationValue = new double?();
      }
      if (calibrationValues.IsCalibrationOk())
        this.calibrationValues = calibrationValues;
      return true;
    }

    internal void SetEncryptionKey(byte[] KeyBytes)
    {
      if (!this.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.encryptionKey.ToString()))
        return;
      this.MyParameters.ParameterByName[S3_ParameterNames.encryptionKey.ToString()].SetUintValue(BitConverter.ToUInt32(KeyBytes, 0));
      this.MyParameters.ParameterByName[S3_ParameterNames.encryptionKey1.ToString()].SetUintValue(BitConverter.ToUInt32(KeyBytes, 4));
      this.MyParameters.ParameterByName[S3_ParameterNames.encryptionKey2.ToString()].SetUintValue(BitConverter.ToUInt32(KeyBytes, 8));
      this.MyParameters.ParameterByName[S3_ParameterNames.encryptionKey3.ToString()].SetUintValue(BitConverter.ToUInt32(KeyBytes, 12));
    }

    internal byte[] GetEncryptionKey()
    {
      if (!this.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.encryptionKey.ToString()))
        return (byte[]) null;
      byte[] encryptionKey = new byte[16];
      BitConverter.GetBytes(this.MyParameters.ParameterByName[S3_ParameterNames.encryptionKey.ToString()].GetUintValue()).CopyTo((Array) encryptionKey, 0);
      BitConverter.GetBytes(this.MyParameters.ParameterByName[S3_ParameterNames.encryptionKey1.ToString()].GetUintValue()).CopyTo((Array) encryptionKey, 4);
      BitConverter.GetBytes(this.MyParameters.ParameterByName[S3_ParameterNames.encryptionKey2.ToString()].GetUintValue()).CopyTo((Array) encryptionKey, 8);
      BitConverter.GetBytes(this.MyParameters.ParameterByName[S3_ParameterNames.encryptionKey3.ToString()].GetUintValue()).CopyTo((Array) encryptionKey, 12);
      return encryptionKey;
    }

    internal bool SetConfigurationParameterInput(
      SortedList<OverrideID, ConfigurationParameter> parameterList,
      SortedList<OverrideID, ConfigurationParameter> oldParameterList,
      int inputIndex)
    {
      this.AddOverwriteHistoryItem(new OverwriteHistoryItem("Input" + inputIndex.ToString(), parameterList));
      InputOutputFunctions inputOutputFunctions = InputOutputFunctions.BusControlled;
      int index1 = parameterList.IndexOfKey(OverrideID.InputOutputFunction);
      if (index1 >= 0)
      {
        inputOutputFunctions = (InputOutputFunctions) parameterList.Values[index1].ParameterValue;
        if (inputOutputFunctions == InputOutputFunctions.None)
          inputOutputFunctions = InputOutputFunctions.BusControlled;
      }
      else if (oldParameterList != null)
        inputOutputFunctions = (InputOutputFunctions) Enum.Parse(typeof (InputOutputFunctions), oldParameterList[OverrideID.InputOutputFunction].ParameterValue.ToString(), true);
      string absoluteResourceName = "IO_" + (inputIndex + 1).ToString() + "_" + inputOutputFunctions.ToString();
      List<string> inputOutputResources = this.MyResources.GetInputOutputResources(inputIndex);
      if (inputOutputResources != null)
      {
        foreach (string str in inputOutputResources)
          this.MyResources.DeleteResource("IO_" + (inputIndex + 1).ToString() + "_" + str);
      }
      this.MyResources.AddResource(absoluteResourceName);
      InputData inputData = this.MyMeterScaling.inpData[inputIndex];
      if (inputData != null)
      {
        inputData.inOutFunction = inputOutputFunctions;
        if (inputData.inOutFunction == InputOutputFunctions.Input)
        {
          if (inputData.impulsValueFactor == (ushort) 0)
          {
            this.MyMeterScaling.SetInputPulsValue("10", inputIndex);
            if (!this.MyIdentification.ResetInputIdentity(inputIndex))
              return false;
          }
          else
          {
            int index2 = parameterList.IndexOfKey(OverrideID.MBusAddress);
            if (index2 >= 0)
            {
              object parameterValue = parameterList.Values[index2].ParameterValue;
              ushort result;
              if (parameterValue != null && Util.TryParse(parameterValue.ToString(), out result))
              {
                if (result < (ushort) 0 || result > (ushort) byte.MaxValue)
                  return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.OperationCancelled, "Invalid M-Bus address! Please add value between 0 and 254.");
                S3_Parameter s3Parameter = this.MyParameters.ParameterByName[S3_Meter.inputMBusSelectedListAndAddress[inputIndex]];
                ushort NewValue = (ushort) ((uint) s3Parameter.GetUshortValue() & 65280U | (uint) result);
                s3Parameter.SetUshortValue(NewValue);
              }
            }
            int index3 = parameterList.IndexOfKey(OverrideID.SerialNumber);
            uint result1;
            if (index3 >= 0 && Util.TryParse(parameterList.Values[index3].GetStringValueDb(), NumberStyles.HexNumber, (IFormatProvider) null, out result1))
              this.MyParameters.ParameterByName[S3_Meter.inputMBusIdentNumber[inputIndex]].SetUintValue(result1);
            int index4 = parameterList.IndexOfKey(OverrideID.Medium);
            if (index4 >= 0)
            {
              int parameterValue = (int) (MBusDeviceType) parameterList.Values[index4].ParameterValue;
              S3_Parameter s3Parameter = this.MyParameters.ParameterByName[S3_Meter.inputMBusMediumGeneration[inputIndex]];
              ushort NewValue = (ushort) (((int) s3Parameter.GetUshortValue() & (int) byte.MaxValue) + (parameterValue << 8));
              s3Parameter.SetUshortValue(NewValue);
            }
            int index5 = parameterList.IndexOfKey(OverrideID.InputResolutionStr);
            if (index5 >= 0)
              this.MyMeterScaling.SetInputResolution(parameterList.Values[index5].GetStringValueDb(), inputIndex);
            int index6 = parameterList.IndexOfKey(OverrideID.InputPulsValue);
            if (index6 >= 0)
              this.MyMeterScaling.SetInputPulsValue(parameterList.Values[index6].GetStringValueWin(), inputIndex);
            int index7 = parameterList.IndexOfKey(OverrideID.InputActualValue);
            if (index7 >= 0)
            {
              Decimal NewValue = Decimal.Parse(parameterList.Values[index7].ParameterValue.ToString()) * inputData.inputUnitInfo.displayFactor;
              this.MyParameters.ParameterByName[S3_Meter.inputActualValue[inputIndex]].SetUintValue((uint) NewValue);
            }
          }
        }
        else
          this.MyMeterScaling.SetInputPulsValue("0", inputIndex);
      }
      return true;
    }

    internal bool IsUseCompactMBusListRequired(
      SortedList<OverrideID, ConfigurationParameter> parameterList)
    {
      int index = parameterList.IndexOfKey(OverrideID.CompactMBusList);
      if (index >= 0)
      {
        if ((bool) parameterList.Values[index].ParameterValue)
          return true;
      }
      else if (this.IsMBusListSwitchingPossible() && !this.MyTransmitParameterManager.AreVirtualDevicesUsed())
        return true;
      return false;
    }

    internal bool SetTimes(
      DateTime meterTimeFromPcTimeZone,
      double timeZoneUTC,
      int endOfBatteryYear,
      int endOfCalibrationYear)
    {
      DateTime TheTime = meterTimeFromPcTimeZone.ToUniversalTime().AddHours(timeZoneUTC);
      byte NewValue = (byte) (timeZoneUTC * 4.0);
      this.MyParameters.ParameterByName["Bak_TimeBaseSecounds"].SetUintValue(ZR_Calendar.Cal_GetMeterTime(TheTime));
      this.MyParameters.ParameterByName["Bak_TimeZoneInQuarterHours"].SetByteValue(NewValue);
      S3_Parameter s3Parameter1 = this.MyParameters.ParameterByName["EndOfBatterie"];
      if (endOfBatteryYear != 0)
        s3Parameter1.SetDateTimeValue(new DateTime(endOfBatteryYear, 1, 1));
      else
        s3Parameter1.SetDateTimeValue(new DateTime(1980, 1, 1));
      S3_Parameter s3Parameter2 = this.MyParameters.ParameterByName["EndOfCalibration"];
      if (endOfCalibrationYear != 0)
        s3Parameter2.SetDateTimeValue(new DateTime(endOfCalibrationYear, 1, 1));
      else
        s3Parameter2.SetDateTimeValue(new DateTime(1980, 1, 1));
      return true;
    }

    public void PrintConfiguration(StringBuilder printText)
    {
      this.printText = printText;
      SortedList<OverrideID, int> sortedList = new SortedList<OverrideID, int>();
      sortedList.Add(OverrideID.EnergyResolution, 0);
      sortedList.Add(OverrideID.ChangeOver, 0);
      sortedList.Add(OverrideID.VolumeResolution, 0);
      sortedList.Add(OverrideID.VolumePulsValue, 0);
      sortedList.Add(OverrideID.MBusAddress, 0);
      sortedList.Add(OverrideID.SerialNumber, 0);
      sortedList.Add(OverrideID.CycleTimeFast, 0);
      sortedList.Add(OverrideID.CycleTimeStandard, 0);
      sortedList.Add(OverrideID.EndOfBattery, 0);
      sortedList.Add(OverrideID.EndOfCalibration, 0);
      sortedList.Add(OverrideID.VolMeterFlowPosition, 0);
      sortedList.Add(OverrideID.VolMeterFlowPositionByUser, 0);
      sortedList.Add(OverrideID.DueDateMonth, 0);
      sortedList.Add(OverrideID.Medium, 0);
      sortedList.Add(OverrideID.InputPulsValue, 0);
      sortedList.Add(OverrideID.InputOutputFunction, 0);
      sortedList.Add(OverrideID.InputResolutionStr, 0);
      printText.AppendLine("--- Heatmeter ---");
      SortedList<OverrideID, ConfigurationParameter> configurationParameters1 = this.GetConfigurationParameters(0, (SortedList<OverrideID, ConfigurationParameter>) null, true);
      for (int index = 0; index < configurationParameters1.Count; ++index)
      {
        if (sortedList.ContainsKey(configurationParameters1.Keys[index]))
        {
          this.PrintLineStart();
          if (configurationParameters1.Keys[index] == OverrideID.VolMeterFlowPositionByUser)
            printText.Append(EnumTranslator.GetTranslatedEnumName((object) OverrideID.VolMeterFlowPosition));
          else
            printText.Append(EnumTranslator.GetTranslatedEnumName((object) configurationParameters1.Keys[index]));
          printText.Append(":");
          this.GarantLineLength();
          if (configurationParameters1.Keys[index] == OverrideID.EndOfCalibration && configurationParameters1.Values[index].GetStringValueWin() == "1980")
            printText.AppendLine(" ----");
          else if (configurationParameters1.Keys[index] == OverrideID.EndOfBattery && ((DateTime) configurationParameters1.Values[index].ParameterValue).Year == 1980)
            printText.AppendLine(" ----");
          else if (configurationParameters1.Keys[index] == OverrideID.VolMeterFlowPositionByUser)
          {
            string str = configurationParameters1.Values[index].GetStringValueWin();
            switch (str)
            {
              case "inlet":
                str = "true";
                break;
              case "outlet":
                str = "false";
                break;
            }
            printText.Append(str);
            printText.AppendLine();
          }
          else
          {
            printText.Append(configurationParameters1.Values[index].GetStringValueWin());
            if (configurationParameters1.Values[index].Unit != null && configurationParameters1.Values[index].Unit.Length > 0)
              printText.Append(" " + configurationParameters1.Values[index].Unit);
            printText.AppendLine();
          }
        }
      }
      Decimal num = (Decimal) this.MyParameters.ParameterByName["Bak_TimeZoneInQuarterHours"].GetByteValue() / 4M;
      this.PrintLineStart();
      printText.Append("Time zone:");
      this.GarantLineLength();
      printText.AppendLine(num.ToString());
      for (int SubDevice = 1; SubDevice < 4; ++SubDevice)
      {
        printText.AppendLine();
        printText.AppendLine("--- Input/Output " + SubDevice.ToString() + " ---");
        SortedList<OverrideID, ConfigurationParameter> configurationParameters2 = this.GetConfigurationParameters(SubDevice, (SortedList<OverrideID, ConfigurationParameter>) null, true);
        if (configurationParameters2 != null)
        {
          for (int index = 0; index < configurationParameters2.Count; ++index)
          {
            if (sortedList.ContainsKey(configurationParameters2.Keys[index]))
            {
              this.PrintLineStart();
              printText.Append(EnumTranslator.GetTranslatedEnumName((object) configurationParameters2.Keys[index]));
              printText.Append(":");
              this.GarantLineLength();
              printText.Append(configurationParameters2.Values[index].GetStringValueWin());
              if (configurationParameters2.Values[index].Unit != null && configurationParameters2.Values[index].Unit.Length > 0)
                printText.Append(" " + configurationParameters2.Values[index].Unit);
              printText.AppendLine();
            }
          }
        }
      }
    }

    private void PrintLineStart()
    {
      this.LineStartOffset = this.printText.Length;
      this.printText.Append("   ");
    }

    private void GarantLineLength()
    {
      int num = this.printText.Length - this.LineStartOffset;
      if (num < 40)
      {
        this.printText.Append(" ");
        ++num;
      }
      for (; num < 39; ++num)
        this.printText.Append(".");
      if (num >= 40)
        return;
      this.printText.Append(" ");
    }

    internal uint MeterTimeAsSeconds1980
    {
      get
      {
        S3_Parameter s3Parameter = this.MyParameters.ParameterByName["Bak_TimeBaseSecounds"];
        return !this.MyDeviceMemory.IsCacheInitialisedNoWarning(s3Parameter.BlockStartAddress, 4) ? 0U : s3Parameter.GetUintValue();
      }
      set => this.MyParameters.ParameterByName["Bak_TimeBaseSecounds"].SetUintValue(value);
    }

    internal DateTime MeterTimeAsDateTime
    {
      get => ZR_Calendar.Cal_GetDateTime(this.MeterTimeAsSeconds1980);
      set => this.MeterTimeAsSeconds1980 = ZR_Calendar.Cal_GetMeterTime(value);
    }

    internal double TimeZoneHourOffset
    {
      get
      {
        return (double) this.MyParameters.ParameterByName["Bak_TimeZoneInQuarterHours"].GetFromSignedByteValue() / 4.0;
      }
      set
      {
        this.MyParameters.ParameterByName["Bak_TimeZoneInQuarterHours"].SetByteValue((byte) (value * 4.0));
      }
    }

    internal DateTime MeterTimeAsPcTime
    {
      get
      {
        DateTime dateTime = this.MeterTimeAsDateTime;
        dateTime = dateTime.AddHours(1.0 - this.TimeZoneHourOffset);
        return dateTime.ToLocalTime();
      }
      set => this.MeterTimeAsDateTime = value.ToUniversalTime().AddHours(this.TimeZoneHourOffset);
    }

    private S3_Meter(S3_HandlerFunctions MyFunctions) => this.MyFunctions = MyFunctions;

    internal S3_Meter(S3_HandlerFunctions MyFunctions, int MemorySize)
    {
      this.MyFunctions = MyFunctions;
      this.MyDeviceMemory = new DeviceMemory(this, MemorySize);
      this.MyParameters = new ParameterList(this);
      this.MyIdentification = new S3_DeviceIdentification(this);
      this.MyLoggerManager = new LoggerManager(this.MyFunctions, this);
      this.MyTransmitParameterManager = new TransmitParameterManager(this.MyFunctions, this);
      this.MyFunctionManager = new FunctionManager(this.MyFunctions, this);
      this.MyWriteProtTableManager = new WriteProtTableManager(this.MyFunctions, this);
      this.MyBackupRuntimeVarsManager = new BackupRuntimeVarsManager(this.MyFunctions, this);
      this.MyHeapManager = new HeapManager(this.MyFunctions, this);
      this.MyLinker = new S3_Linker(this.MyFunctions, this);
    }

    internal S3_Meter(S3_HandlerFunctions MyFunctions, byte[] PackedByteList)
    {
      this.MyFunctions = MyFunctions;
      this.MyIdentification = new S3_DeviceIdentification(this);
      this.MyDeviceMemory = new DeviceMemory(this, PackedByteList);
      this.MyParameters = new ParameterList(this);
      this.MyLoggerManager = new LoggerManager(this.MyFunctions, this);
      this.MyTransmitParameterManager = new TransmitParameterManager(this.MyFunctions, this);
      this.MyFunctionManager = new FunctionManager(this.MyFunctions, this);
      this.MyWriteProtTableManager = new WriteProtTableManager(this.MyFunctions, this);
      this.MyBackupRuntimeVarsManager = new BackupRuntimeVarsManager(this.MyFunctions, this);
      this.MyHeapManager = new HeapManager(this.MyFunctions, this);
      this.MyLinker = new S3_Linker(this.MyFunctions, this);
    }

    internal S3_Meter Clone(S3_HandlerFunctions handlerFunctions)
    {
      S3_Meter.S3_MeterLogger.Info("Clone meter");
      S3_Meter s3Meter = new S3_Meter(handlerFunctions)
      {
        theMap = this.theMap,
        IsWriteProtected = this.IsWriteProtected
      };
      s3Meter.MyDeviceMemory = this.MyDeviceMemory.Clone(s3Meter);
      s3Meter.MyParameters = this.MyParameters.Clone(s3Meter);
      s3Meter.MyIdentification = this.MyIdentification.Clone(s3Meter);
      s3Meter.MyResources = this.MyResources.Clone(s3Meter);
      s3Meter.MyLoggerManager = this.MyLoggerManager.Clone(s3Meter);
      s3Meter.MyMeterScaling = this.MyMeterScaling.Clone(s3Meter);
      s3Meter.MyWriteProtTableManager = this.MyWriteProtTableManager.Clone(s3Meter);
      this.MyFunctionManager.Clone(s3Meter);
      this.MyTransmitParameterManager.Clone(s3Meter);
      this.MyLinker.Clone(s3Meter);
      s3Meter.MyDeviceMemory.AddLinkedParametersToBlocks();
      if (!s3Meter.MyIdentification.IdentificationCheckedAndOk)
        s3Meter.ClearIdentification();
      s3Meter.MeterObjectGeneratedTimeStamp = DateTime.Now;
      s3Meter.TypeOverrideString = this.TypeOverrideString;
      s3Meter.TypeCreationStringReplaced = this.TypeCreationStringReplaced;
      s3Meter.TypeCreationString = this.TypeCreationString;
      s3Meter.SavedOrderNumberString = this.SavedOrderNumberString;
      s3Meter.BatteryManager = this.BatteryManager;
      s3Meter.overwriteHistorie = this.overwriteHistorie;
      s3Meter.overwriteHistorie.Add(new OverwriteHistoryItem(nameof (Clone)));
      this.overwriteHistorie = new List<OverwriteHistoryItem>();
      return s3Meter;
    }

    internal void AddOverwriteHistoryItem(OverwriteHistoryItem theItem)
    {
      this.overwriteHistorie.Add(theItem);
    }

    internal bool Compile()
    {
      if (!this.Compile(0))
        throw new Exception("Compile error");
      return true;
    }

    internal bool Compile(int dummy)
    {
      S3_Meter.S3_MeterLogger.Info("Compile meter");
      if (!this.MyFunctions.baseTypeEditMode)
      {
        S3_Meter.S3_MeterLogger.Debug("RemoveByResources");
        if (!this.RemoveByResources())
          return false;
      }
      S3_Meter.S3_MeterLogger.Debug("ReCreateMemoryBlocks");
      if (!this.MyFunctionManager.ReCreateMemoryBlocks())
        return false;
      if (this.MyFunctions.useDevelopmentFunctions)
        this.FixBlockAddresses(this.MyDeviceMemory.meterMemory);
      this.MyTransmitParameterManager.Transmitter.AdjustUnitsDifVifandLimits();
      this.MyDeviceMemory.BlockHandlerInfo.CreateMemoryBlock();
      S3_Meter.S3_MeterLogger.Debug("Link");
      if (!this.MyLinker.Link(this.MyDeviceMemory.meterMemory) || !this.MyLinker.ResetLabels() || !this.MyParameters.RecreateParameterByAddress())
        return false;
      S3_Meter.S3_MeterLogger.Debug("Insert data");
      if (!this.MyLoggerManager.InsertData() || !this.MyFunctionManager.InsertData() || !this.MyTransmitParameterManager.Transmitter.InsertData() || !this.MyWriteProtTableManager.InsertData())
        return false;
      this.MyParameters.InsertRelocalizableParameterData();
      this.MyIdentification.UpdateRadioIDsByResources();
      this.MyDeviceMemory.BlockHandlerInfo.InsertData();
      S3_Meter.S3_MeterLogger.Debug("Link pointers");
      return this.MyLoggerManager.LinkPointer() && this.MyFunctionManager.LinkPointer() && this.MyTransmitParameterManager.LinkPointer() && this.RunConfiguratorFunctions() && this.MyMeterScaling.GarantFinalConfigurations() && this.GenerateIdentificationChecksum() && (this.calibrationValues == null || this.SetCalibration());
    }

    internal bool RunConfiguratorFunctions()
    {
      if (this.resetAllValues)
      {
        this.resetAllValues = false;
        if (!this.MyFunctions.MyMeters.OverwriteFromType.ResetAccumulatedValues())
          return false;
      }
      if (this.clearAllLogger)
      {
        this.clearAllLogger = false;
        if (!this.MyLoggerManager.ResetAndClearAllLoggers())
          return false;
      }
      if (this.setMbusPrimAdrFromSerialNumber)
      {
        this.setMbusPrimAdrFromSerialNumber = false;
        if (!this.MyTransmitParameterManager.SetMbusPrimAddressesFromSerialNumber())
          return false;
      }
      return true;
    }

    private void FixBlockAddresses(S3_MemoryBlock baseBlock)
    {
      if ((baseBlock.SegmentType == S3_MemorySegment.TransmitParameterTable || baseBlock.SegmentType == S3_MemorySegment.LoggerData) && baseBlock.sourceMemoryBlock != null)
      {
        baseBlock.BlockStartAddress = baseBlock.sourceMemoryBlock.BlockStartAddress;
        baseBlock.IsHardLinkedAddress = true;
      }
      if (baseBlock.childMemoryBlocks == null)
        return;
      foreach (S3_MemoryBlock childMemoryBlock in baseBlock.childMemoryBlocks)
        this.FixBlockAddresses(childMemoryBlock);
    }

    private bool SetCalibration()
    {
      if (this.calibrationValues.PreparedForOnePointVolumeCalibration)
      {
        VolumeGraphCalibration.GarantObjectAvailable(this.MyFunctions);
        if (!this.MyFunctions.volumeGraphCalibration.AdjustVolumeFactor((float) this.calibrationValues.CalVolMaxErrorPercent))
          return false;
      }
      else if (this.calibrationValues.PreparedForThreePointVolumeCalibration)
      {
        VolumeGraphCalibration.GarantObjectAvailable(this.MyFunctions);
        if (!this.MyFunctions.volumeGraphCalibration.AdjustVolumeCalibration((float) this.calibrationValues.CalVolMinFlowLiterPerHour, (float) this.calibrationValues.CalVolMinErrorPercent, (float) this.calibrationValues.CalVolNominalFlowLiterPerHour, (float) this.calibrationValues.CalVolNominalErrorPercent, (float) this.calibrationValues.CalVolMaxFlowLiterPerHour, (float) this.calibrationValues.CalVolMaxErrorPercent))
          return false;
      }
      if (this.calibrationValues.PreparedForTemperatureCalibration)
      {
        if (this.MyFunctions.adc_Calibration == null)
          this.MyFunctions.adc_Calibration = new ADC_Calibration(this.MyFunctions);
        this.MyFunctions.adc_Calibration.CalibrateByConfigurator(this.calibrationValues);
      }
      return true;
    }

    private bool RemoveByResources()
    {
      if (!this.MyResources.AreValid())
      {
        S3_Meter.S3_MeterLogger.Warn("The recources of WorkMeter are invalid! " + Environment.NewLine + this.MyResources?.ToString());
        this.MyResources.CleanupExclusivesResources();
        S3_Meter.S3_MeterLogger.Warn("Resouces after clean up function: " + Environment.NewLine + this.MyResources?.ToString());
      }
      this.MyResources.ResourceEventsReset();
      int removeObjectCounter;
      do
      {
        removeObjectCounter = this.MyResources.RemoveObjectCounter;
        this.MyLinker.ResetLabels();
        if (!this.MyFunctionManager.RemoveByResources() || !this.MyTransmitParameterManager.RemoveByResources() || !this.MyLoggerManager.RemoveByResources())
          return false;
      }
      while (this.MyResources.RemoveObjectCounter > removeObjectCounter);
      return true;
    }

    internal bool CreateCompleteFromMemory()
    {
      if (!this.LoadMapVars())
        return false;
      SortedList<string, S3_Parameter> parameterByName1 = this.MyParameters.ParameterByName;
      S3_ParameterNames s3ParameterNames = S3_ParameterNames.Con_HardwareTypeId;
      string key1 = s3ParameterNames.ToString();
      uint uintValue = parameterByName1[key1].GetUintValue();
      SortedList<string, S3_Parameter> parameterByName2 = this.MyParameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.Bak_HardwareAndRestrictions;
      string key2 = s3ParameterNames.ToString();
      ushort HardwareMask = (ushort) ((uint) parameterByName2[key2].GetUshortValue() & 4095U);
      if ((int) uintValue != (int) this.MyIdentification.HardwareTypeId || (int) HardwareMask != (int) (ushort) this.MyIdentification.HardwareMask)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Hardware type identification difference between data base and map data!");
        bool flag = false;
        if ((int) uintValue != (int) this.MyIdentification.HardwareTypeId)
        {
          string fromHardwareTypeId1 = this.MyFunctions.MyDatabase.GetFirmwareStringFromHardwareTypeId((int) this.MyIdentification.HardwareTypeId);
          stringBuilder.AppendLine("HardwareTypeId form data base: " + this.MyIdentification.HardwareTypeId.ToString() + "; Firmware:" + fromHardwareTypeId1);
          string fromHardwareTypeId2 = this.MyFunctions.MyDatabase.GetFirmwareStringFromHardwareTypeId((int) uintValue);
          stringBuilder.AppendLine("HardwareTypeId form map: " + uintValue.ToString() + "; Firmware:" + fromHardwareTypeId2);
          flag = true;
          if (uintValue > 0U)
            stringBuilder.AppendLine("HardwareTypeId form map is used. Please change database!");
          else
            stringBuilder.AppendLine("HardwareTypeId form data base is used. Please change map!");
        }
        if ((int) HardwareMask != (int) (ushort) this.MyIdentification.HardwareMask)
        {
          if (flag)
            stringBuilder.AppendLine();
          stringBuilder.AppendLine("Hardware mask form data base: " + this.MyIdentification.HardwareMask.ToString("x04") + "; " + ParameterService.GetHardwareString(this.MyIdentification.HardwareMask));
          stringBuilder.AppendLine("Hardware mask form map: " + HardwareMask.ToString("x04") + "; " + ParameterService.GetHardwareString((uint) HardwareMask));
        }
        ZR_ClassLibMessages.AddWarning(stringBuilder.ToString(), S3_Meter.S3_MeterLogger);
      }
      this.MyIdentification.LoadDeviceIdFromParameter();
      this.MyFunctions.MyDatabase.AddHardwareInfosFromHardwareTypeId((S3_DeviceId) this.MyIdentification);
      this.CheckIdentificationChecksum();
      if (!this.CreateToLoggerStructFromMemory())
        return false;
      if (this.MyParameters.AddressLables.ContainsKey("SERIE3_PROTECTED_CONFIG2") && !this.MyWriteProtTableManager.CreateStructureObjects())
      {
        S3_Meter.S3_MeterLogger.Error("Create write protected table error!");
        return false;
      }
      if (this.MyParameters.AddressLables.ContainsKey("SERIE3_CHANGEABLE_CONFIG"))
      {
        if (!this.MyLoggerManager.CreateStructureObjects())
        {
          S3_Meter.S3_MeterLogger.Error("MyLoggerManager.CreateStructureObjects error");
          return false;
        }
        if (!this.MyFunctionManager.CreateStructureObjects())
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Read function table error!", S3_Meter.S3_MeterLogger);
          return false;
        }
        if (!this.MyDeviceMemory.BlockHandlerInfo.CreateFromMemory())
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Read handlerInfo error!", S3_Meter.S3_MeterLogger);
          return false;
        }
        if (!this.MyTransmitParameterManager.CreateMbusTableFromMemory())
        {
          S3_Meter.S3_MeterLogger.Error("ReadAllTransmitParameterStructs error");
          return false;
        }
      }
      this.CheckWriteProtectionAndInputs();
      return true;
    }

    internal bool IsInputAvailable(int inputIndex)
    {
      switch (inputIndex)
      {
        case 0:
          return this.MyIdentification.IsInput1Available;
        case 1:
          return this.MyIdentification.IsInput2Available;
        case 2:
          return this.MyIdentification.IsInput3Available;
        default:
          throw new Exception("Illegal input index");
      }
    }

    internal bool CreateToLoggerStructFromMemory()
    {
      if (!this.MyDeviceMemory.SetBlockAddressesFromParameters())
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "SetBlockAddressesFromParameters error", S3_Meter.S3_MeterLogger);
      if (!this.MyDeviceMemory.AddLinkedParametersToBlocks())
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "AddLinkedParametersToBlocks error", S3_Meter.S3_MeterLogger);
      this.MyResources = new MeterResources(this);
      if (this.MyParameters.AddressLables.ContainsKey("SERIE3_CHANGEABLE_CONFIG") && !this.MyLoggerManager.CreateLoggersFromMemory())
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "CreateLoggersFromMemory error", S3_Meter.S3_MeterLogger);
      this.MyMeterScaling = new MeterScaling(this);
      return this.MyMeterScaling.ReadSettingsFromParameter();
    }

    internal bool CreateToLoggerTransmitParameterFromMemory()
    {
      if (!this.MyFunctionManager.CreateStructureObjects())
        return ZR_ClassLibMessages.AddErrorDescription("Create function table error!", S3_Meter.S3_MeterLogger);
      if (!this.MyTransmitParameterManager.CreateMbusTableFromMemory())
        return ZR_ClassLibMessages.AddErrorDescription("MyTransmitParameterManager.CreateStructureObjects error", S3_Meter.S3_MeterLogger);
      return this.MyWriteProtTableManager.CreateStructureObjects() || ZR_ClassLibMessages.AddErrorDescription("Create write protected table error!", S3_Meter.S3_MeterLogger);
    }

    internal void CheckWriteProtectionAndInputs()
    {
      this.IsWriteProtected = ((uint) this.MyParameters.ParameterByName[S3_ParameterNames.Bak_HardwareAndRestrictions.ToString()].GetUshortValue() & 4096U) > 0U;
    }

    internal bool LoadMapVars()
    {
      try
      {
        this.MyParameters.ParameterByName.Clear();
        this.MyParameters.ParameterByAddress.Clear();
        this.MyParameters.AddressLables.Clear();
        this.theMap = MapSelector.GetMap(this.MyIdentification.FirmwareVersion, this.MyIdentification.MapId, this.MyFunctions.MyDatabase);
        foreach (KeyValuePair<string, int> the in this.theMap)
          this.MyParameters.AddNewParameterFromName(the.Key, the.Value);
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Error on create linked map" + ZR_Constants.SystemNewLine + ex.ToString());
        return false;
      }
    }

    internal void SetPcTime()
    {
      S3_Parameter s3Parameter = this.MyParameters.ParameterByName["Bak_TimeBaseSecounds"];
      double num = (double) this.MyParameters.ParameterByName["Bak_TimeZoneInQuarterHours"].GetFromSignedByteValue() / 4.0;
      DateTime dateTime = DateTime.Now;
      dateTime = dateTime.ToUniversalTime();
      this.MeterTimeAsDateTime = dateTime.AddHours(num);
      s3Parameter.SetUintValue(this.MeterTimeAsSeconds1980);
    }

    internal void SetTypeTimesAndTypeConstants()
    {
      this.MeterTimeAsDateTime = new DateTime(2011, 1, 2, 3, 4, 5);
      this.MyParameters.ParameterByName[S3_ParameterNames.EndOfBatterie.ToString()].SetDateTimeValue(new DateTime(2012, 2, 3));
      this.MyParameters.ParameterByName[S3_ParameterNames.EndOfCalibration.ToString()].SetDateTimeValue(new DateTime(2013, 3, 4));
      this.MyParameters.ParameterByName[S3_ParameterNames.Con_MeterId.ToString()].SetUintValue(0U);
      this.MyParameters.ParameterByName[S3_ParameterNames.volumeCycleTimeCounter.ToString()].SetByteValue((byte) 0);
      this.MyParameters.ParameterByName[S3_ParameterNames.temperaturCycleTimeCounter.ToString()].SetByteValue((byte) 0);
      this.MyParameters.ParameterByName[S3_ParameterNames.Tim_BaseTimeCounter_4ms.ToString()].SetShortValue((short) 0);
      this.MyParameters.ParameterByName[S3_ParameterNames.Tim_NextEventTime_4ms.ToString()].SetShortValue((short) 0);
    }

    internal bool WriteBackupValuesToDeviceRAM()
    {
      return this.MyDeviceMemory.WriteDataToConnectedDevice(this.MyParameters.ParameterByName["Bak_TimeBaseSecounds"].BlockStartAddress, (int) this.MyParameters.ParameterByName["Con_BakupSize"].GetByteValue());
    }

    internal bool SetDeviceTime(DateTime SetTime)
    {
      ulong meterTime = (ulong) ZR_Calendar.Cal_GetMeterTime(SetTime);
      S3_Parameter s3Parameter = this.MyParameters.ParameterByName["Bak_TimeBaseSecounds"];
      s3Parameter.SetUlongValue(meterTime);
      return s3Parameter.WriteParameterToConnectedDevice();
    }

    internal bool GenerateIdentificationChecksum()
    {
      S3_Parameter s3Parameter1 = this.MyParameters.ParameterByName["Con_MeterId"];
      S3_Parameter s3Parameter2 = this.MyParameters.ParameterByName["Con_IdentificationChecksum"];
      ushort NewValue = 0;
      for (int blockStartAddress = s3Parameter1.BlockStartAddress; blockStartAddress < s3Parameter2.BlockStartAddress; blockStartAddress += 2)
        NewValue += this.MyDeviceMemory.GetUshortValue(blockStartAddress);
      return s3Parameter2.SetUshortValue(NewValue);
    }

    internal void ClearIdentification()
    {
      S3_Parameter s3Parameter1 = this.MyParameters.ParameterByName["Con_MeterId"];
      S3_Parameter s3Parameter2 = this.MyParameters.ParameterByName["Con_MeterInfoId"];
      S3_Parameter s3Parameter3 = this.MyParameters.ParameterByName["Con_Manufacturer"];
      for (int blockStartAddress = s3Parameter1.BlockStartAddress; blockStartAddress < s3Parameter3.BlockStartAddress; blockStartAddress += 2)
        this.MyDeviceMemory.SetUshortValue(blockStartAddress, (ushort) 0);
      if (!this.MyIdentification.IsWR4)
        s3Parameter2.SetUintValue(260U);
      this.GenerateIdentificationChecksum();
      this.MyIdentification.LoadDeviceIdFromParameter();
      this.CheckIdentificationChecksum();
      if (this.MyIdentification.IdentificationCheckState != S3_DeviceId.IdentificationCheckStates.ok)
        throw new Exception("IdentificationChecksum management error");
    }

    internal void CheckIdentificationChecksum()
    {
      this.MyIdentification.IdentificationCheckState = S3_DeviceId.IdentificationCheckStates.NotChecked;
      S3_Parameter s3Parameter1 = this.MyParameters.ParameterByName["Con_MeterId"];
      S3_Parameter s3Parameter2 = this.MyParameters.ParameterByName["Con_IdentificationChecksum"];
      ushort num = 0;
      for (int blockStartAddress = s3Parameter1.BlockStartAddress; blockStartAddress < s3Parameter2.BlockStartAddress; blockStartAddress += 2)
        num += this.MyDeviceMemory.GetUshortValue(blockStartAddress);
      if ((int) s3Parameter2.GetUshortValue() == (int) num)
        this.MyIdentification.IdentificationCheckState = S3_DeviceId.IdentificationCheckStates.ok;
      else
        this.MyIdentification.IdentificationCheckState = S3_DeviceId.IdentificationCheckStates.ChecksumError;
    }

    internal bool SetMeterKey(uint password, bool saveToDatabase)
    {
      ZR_ClassLibMessages.ClearErrors();
      if (!this.MyFunctions.MyCommands.DeviceProtectionSetKey(uint.MaxValue))
      {
        if (ZR_ClassLibMessages.GetLastError() != ZR_ClassLibMessages.LastErrors.NAK_Received)
          return false;
        ZR_ClassLibMessages.ClearErrors();
        ZR_ClassLibMessages.AddWarning("MeterKey already defined!");
        return true;
      }
      if (this.theRandom == null)
        this.theRandom = new Random(DateTime.Now.Millisecond);
      uint Key;
      uint num;
      do
      {
        Key = (uint) this.theRandom.Next() + (uint) this.theRandom.Next();
        num = Key ^ password;
      }
      while (num == uint.MaxValue);
      if (!this.MyFunctions.MyCommands.DeviceProtectionSetKey(num))
        return false;
      if (saveToDatabase)
      {
        if (password == 0U)
        {
          if (!this.MyFunctions.MyDatabase.SetDeviceKey(this.MyIdentification.MeterId, num, MeterDBAccess.ValueTypes.MeterKey))
            return false;
        }
        else if (!this.MyFunctions.MyDatabase.SetDeviceKey(this.MyIdentification.MeterId, Key, MeterDBAccess.ValueTypes.GovernmentRandomNr))
          return false;
      }
      return true;
    }

    internal bool SetWriteProtection()
    {
      bool flag = this.MyFunctions.MyCommands.DeviceProtectionSet();
      if (flag)
      {
        S3_Parameter s3Parameter = this.MyParameters.ParameterByName[S3_ParameterNames.Bak_HardwareAndRestrictions.ToString()];
        ushort NewValue = (ushort) ((uint) s3Parameter.GetUshortValue() | 4096U);
        s3Parameter.SetUshortValue(NewValue);
      }
      return flag;
    }

    internal bool ClearWriteProtection(uint password)
    {
      uint Key;
      MeterDBAccess.ValueTypes ValueType;
      if (!this.MyFunctions.MyDatabase.GetDeviceKeys((int) this.MyIdentification.MeterId, out Key, out ValueType))
        return false;
      if (ValueType == MeterDBAccess.ValueTypes.GovernmentRandomNr)
        Key ^= password;
      bool flag = this.MyFunctions.MyCommands.DeviceProtectionReset(Key);
      if (flag)
      {
        S3_Parameter s3Parameter = this.MyParameters.ParameterByName[S3_ParameterNames.Bak_HardwareAndRestrictions.ToString()];
        ushort NewValue = (ushort) ((uint) s3Parameter.GetUshortValue() & 4294963199U);
        s3Parameter.SetUshortValue(NewValue);
      }
      return flag;
    }

    internal void SetAllVarsToDefault()
    {
      foreach (S3_Parameter s3Parameter in (IEnumerable<S3_Parameter>) this.MyParameters.ParameterByName.Values)
        s3Parameter.SetToDefaultValue();
    }

    internal bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList,
      int SubDevice)
    {
      DateTime now = DateTime.Now;
      if (SubDevice != 0)
        return false;
      List<long> filter = (List<long>) null;
      if (ValueList == null)
        ValueList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
      else if (ValueList.Count > 0)
        filter.AddRange((IEnumerable<long>) ValueList.Keys);
      long num1 = 272769356;
      if (ValueIdent.IsExpectedValueIdent(filter, num1))
      {
        Decimal num2 = Convert.ToDecimal(this.MyParameters.ParameterByName["vorlauftemperatur"].GetShortValue());
        this.AddValue(ref ValueList, now, num1, (object) (num2 / 100M));
      }
      long num3 = 272769357;
      if (ValueIdent.IsExpectedValueIdent(filter, num3))
      {
        Decimal num4 = Convert.ToDecimal(this.MyParameters.ParameterByName["ruecklauftemperatur"].GetShortValue());
        this.AddValue(ref ValueList, now, num3, (object) (num4 / 100M));
      }
      long num5 = 272769351;
      if (ValueIdent.IsExpectedValueIdent(filter, num5))
      {
        Decimal num6 = Convert.ToDecimal(this.MyParameters.ParameterByName["delta_t"].GetShortValue());
        this.AddValue(ref ValueList, now, num5, (object) (num6 / 100M));
      }
      long num7 = 272769346;
      if (ValueIdent.IsExpectedValueIdent(filter, num7))
      {
        Decimal num8 = Convert.ToDecimal(this.MyParameters.ParameterByName["Energy_HeatEnergyDisplay"].GetUlongValue());
        this.AddValue(ref ValueList, now, num7, (object) (num8 * this.MyMeterScaling.energyLcdToMWhFactor));
      }
      long num9 = 272769355;
      if (ValueIdent.IsExpectedValueIdent(filter, num9))
      {
        Decimal num10 = Convert.ToDecimal(this.MyParameters.ParameterByName["Energy_ColdEnergyDisplay"].GetUlongValue());
        this.AddValue(ref ValueList, now, num9, (object) (num10 * this.MyMeterScaling.energyLcdToMWhFactor));
      }
      long num11 = 272769345;
      if (ValueIdent.IsExpectedValueIdent(filter, num11))
      {
        Decimal num12 = Convert.ToDecimal(this.MyParameters.ParameterByName["Vol_VolDisplay"].GetUlongValue());
        this.AddValue(ref ValueList, now, num11, (object) (num12 * this.MyMeterScaling.volumeLcdToQmFactor));
      }
      return true;
    }

    protected void AddValue(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> valueList,
      DateTime timePoint,
      long valueIdent,
      object obj)
    {
      ReadingValue readingValue = new ReadingValue();
      readingValue.value = Util.ToDouble(obj);
      readingValue.state = ReadingValueState.ok;
      if (valueList.ContainsKey(valueIdent))
      {
        if (valueList[valueIdent].ContainsKey(timePoint))
          return;
        valueList[valueIdent].Add(timePoint, readingValue);
      }
      else
        valueList.Add(valueIdent, new SortedList<DateTime, ReadingValue>()
        {
          {
            timePoint,
            readingValue
          }
        });
    }

    public DeviceState GetDeviceState()
    {
      DeviceState deviceState = (DeviceState) null;
      ByteField MemoryData;
      if (this.MyFunctions.MyCommands.ReadMemory(MemoryLocation.RAM, 512, 1, out MemoryData))
      {
        deviceState = new DeviceState();
        deviceState.unlockPressed = ((int) MemoryData.Data[0] & 4) == 0;
        deviceState.buttonPressed = ((int) MemoryData.Data[0] & 8) == 0;
      }
      return deviceState;
    }

    public MeterDisplayValues GetDisplayValues()
    {
      MeterDisplayValues displayValues = new MeterDisplayValues();
      displayValues.energyIsAvailable = false;
      displayValues.cEnergyIsAvailable = false;
      int index1 = this.MyParameters.ParameterByName.IndexOfKey(S3_ParameterNames.energyUnitIndex.ToString());
      if (index1 >= 0)
      {
        byte byteValue = this.MyParameters.ParameterByName.Values[index1].GetByteValue();
        int index2 = LcdUnitsC2.resolutionStringFromResolutionId.IndexOfKey(byteValue);
        if (index2 >= 0)
        {
          S3_Parameter s3Parameter1 = this.MyParameters.ParameterByName["Energy_HeatEnergyDisplay"];
          displayValues.energy = s3Parameter1.GetUintValue();
          LcdUnitsC2.GetUnitDataFromDisplayString(LcdUnitsC2.resolutionStringFromResolutionId.Values[index2], out displayValues.energyUnitString, out displayValues.energyAfterPointDigits, out displayValues.energyFactorFromWh);
          displayValues.energyIsAvailable = true;
          S3_Parameter s3Parameter2 = this.MyParameters.ParameterByName["Energy_ColdEnergyDisplay"];
          displayValues.cEnergy = s3Parameter2.GetUintValue();
          displayValues.cEnergyUnitString = displayValues.energyUnitString;
          displayValues.cEnergyAfterPointDigits = displayValues.energyAfterPointDigits;
          displayValues.cEnergyFactorFromWh = displayValues.energyFactorFromWh;
          displayValues.cEnergyIsAvailable = true;
        }
      }
      displayValues.volumeIsAvailable = false;
      byte byteValue1 = this.MyParameters.ParameterByName["volumeUnitIndex"].GetByteValue();
      int index3 = LcdUnitsC2.resolutionStringFromResolutionId.IndexOfKey(byteValue1);
      if (index3 >= 0)
      {
        S3_Parameter s3Parameter = this.MyParameters.ParameterByName["Vol_VolDisplay"];
        displayValues.volume = s3Parameter.GetUintValue();
        LcdUnitsC2.GetUnitDataFromDisplayString(LcdUnitsC2.resolutionStringFromResolutionId.Values[index3], out displayValues.volumeUnitString, out displayValues.volumeAfterPointDigits, out displayValues.volumeFactorFromLiter);
        displayValues.volumeIsAvailable = true;
      }
      displayValues.input0IsAvailable = false;
      int index4 = this.MyParameters.ParameterByName.IndexOfKey("input0UnitIndex");
      double factorFromBaseUnit;
      if (index4 >= 0)
      {
        byte byteValue2 = this.MyParameters.ParameterByName.Values[index4].GetByteValue();
        int index5 = LcdUnitsC2.resolutionStringFromResolutionId.IndexOfKey(byteValue2);
        if (index5 >= 0)
        {
          S3_Parameter s3Parameter = this.MyParameters.ParameterByName["Cal_DisplayInput_n_0"];
          displayValues.input0Value = s3Parameter.GetUintValue();
          LcdUnitsC2.GetUnitDataFromDisplayString(LcdUnitsC2.resolutionStringFromResolutionId.Values[index5], out displayValues.input0UnitString, out displayValues.input0AfterPointDigits, out factorFromBaseUnit);
          displayValues.input0IsAvailable = true;
        }
      }
      displayValues.input1IsAvailable = false;
      int index6 = this.MyParameters.ParameterByName.IndexOfKey("input1UnitIndex");
      if (index6 >= 0)
      {
        byte byteValue3 = this.MyParameters.ParameterByName.Values[index6].GetByteValue();
        int index7 = LcdUnitsC2.resolutionStringFromResolutionId.IndexOfKey(byteValue3);
        if (index7 >= 0)
        {
          S3_Parameter s3Parameter = this.MyParameters.ParameterByName["Cal_DisplayInput_n_1"];
          displayValues.input1Value = s3Parameter.GetUintValue();
          LcdUnitsC2.GetUnitDataFromDisplayString(LcdUnitsC2.resolutionStringFromResolutionId.Values[index7], out displayValues.input1UnitString, out displayValues.input1AfterPointDigits, out factorFromBaseUnit);
          displayValues.input1IsAvailable = true;
        }
      }
      displayValues.input2IsAvailable = false;
      int index8 = this.MyParameters.ParameterByName.IndexOfKey("input2UnitIndex");
      if (index8 >= 0)
      {
        byte byteValue4 = this.MyParameters.ParameterByName.Values[index8].GetByteValue();
        int index9 = LcdUnitsC2.resolutionStringFromResolutionId.IndexOfKey(byteValue4);
        if (index9 >= 0)
        {
          S3_Parameter s3Parameter = this.MyParameters.ParameterByName["Cal_DisplayInput_n_2"];
          displayValues.input2Value = s3Parameter.GetUintValue();
          LcdUnitsC2.GetUnitDataFromDisplayString(LcdUnitsC2.resolutionStringFromResolutionId.Values[index9], out displayValues.input2UnitString, out displayValues.input2AfterPointDigits, out factorFromBaseUnit);
          displayValues.input2IsAvailable = true;
        }
      }
      return displayValues;
    }

    internal string GetMeterValuesOverview()
    {
      StringBuilder stringBuilder = new StringBuilder();
      Decimal num1 = (Decimal) this.MyParameters.ParameterByName["Bak_HeatEnergySum"].GetDoubleValue() * this.MyMeterScaling.energyLcdToBaseUnitFactor;
      stringBuilder.AppendLine("Heating energy: " + num1.ToString() + " " + this.MyMeterScaling.energyResolutionInfo.baseUnitString);
      ConfigurationParameter.ChangeOverValues changeoverSetup = this.GetChangeoverSetup();
      Decimal num2;
      if (changeoverSetup == ConfigurationParameter.ChangeOverValues.Cooling || changeoverSetup == ConfigurationParameter.ChangeOverValues.ChangeOver)
      {
        num2 = (Decimal) this.MyParameters.ParameterByName["Bak_ColdEnergySum"].GetDoubleValue() * this.MyMeterScaling.energyLcdToBaseUnitFactor;
        stringBuilder.AppendLine("Cooling energy: " + num2.ToString() + " " + this.MyMeterScaling.energyResolutionInfo.baseUnitString);
      }
      num2 = (Decimal) this.MyParameters.ParameterByName["Bak_VolSum"].GetDoubleValue() * this.MyMeterScaling.volumeLcdToQmFactor;
      stringBuilder.AppendLine("Volume: " + num2.ToString() + " m\u00B3");
      num2 = Convert.ToDecimal(this.MyParameters.ParameterByName["vorlauftemperatur"].GetShortValue()) / 100M;
      stringBuilder.AppendLine("Inlet temperature: " + num2.ToString("0.00") + " °C");
      num2 = Convert.ToDecimal(this.MyParameters.ParameterByName["ruecklauftemperatur"].GetShortValue()) / 100M;
      stringBuilder.AppendLine("Outlet temperature: " + num2.ToString("0.00") + " °C");
      num2 = Convert.ToDecimal(this.MyParameters.ParameterByName["delta_t"].GetShortValue()) / 100M;
      stringBuilder.AppendLine("Temperature difference: " + num2.ToString("0.00") + " °C");
      return stringBuilder.ToString();
    }

    internal bool SetTypeInfos(uint SAP_MaterialNumber, uint MeterInfoId, uint MeterTypeId)
    {
      bool flag = this.MyParameters.ParameterByName[S3_ParameterNames.Con_SAP_MaterialNumber.ToString()].SetUintValue(SAP_MaterialNumber);
      S3_ParameterNames s3ParameterNames;
      if (flag)
      {
        SortedList<string, S3_Parameter> parameterByName = this.MyParameters.ParameterByName;
        s3ParameterNames = S3_ParameterNames.Con_MeterInfoId;
        string key = s3ParameterNames.ToString();
        flag = parameterByName[key].SetUintValue(MeterInfoId);
      }
      if (flag)
      {
        SortedList<string, S3_Parameter> parameterByName = this.MyParameters.ParameterByName;
        s3ParameterNames = S3_ParameterNames.Con_MeterTypeId;
        string key = s3ParameterNames.ToString();
        flag = parameterByName[key].SetUintValue(MeterTypeId);
      }
      return flag;
    }

    internal bool CalibrateClock(double error_ppm)
    {
      if (Math.Abs(error_ppm) > 240.0)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "The calibrationvalue for clockfrequency is OUT OF RANGE!!!");
        return false;
      }
      S3_Parameter s3Parameter = this.MyParameters.ParameterByName[S3_ParameterNames.Con_QuarzOffsetErr.ToString()];
      short NewValue = (short) ((int) s3Parameter.GetShortValue() - (int) (short) (error_ppm * 0.98304));
      s3Parameter.SetShortValue(NewValue);
      return true;
    }

    internal bool CalibrateRadioFrequency(double error_Hz)
    {
      S3_Parameter s3Parameter = this.MyParameters.ParameterByName[S3_ParameterNames.radioFreqOffset.ToString()];
      short shortValue = s3Parameter.GetShortValue();
      if (!this.MyFunctions.MyMeters.ConnectedMeter.MyIdentification.IsLoRa)
      {
        if (Math.Abs(error_Hz) > 1000000.0)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "The calibrationvalue for radiofrequency is OUT OF RANGE!!!");
          return false;
        }
        short NewValue = (short) ((int) shortValue - (int) (short) (error_Hz * 0.032768));
        s3Parameter.SetShortValue(NewValue);
        return true;
      }
      S3_CommandsCOMPATIBLE newCommands = this.MyFunctions.checkedNewCommands;
      int actualFrequencyIncrement = 0;
      int additionalFrequencyIncrement = 0;
      AsyncHelpers.RunSync((Func<Task>) (async () =>
      {
        await newCommands.myCommandsRadio.StopRadioTests(this.MyFunctions.progress, this.MyFunctions.cancelToken);
        actualFrequencyIncrement = await newCommands.myCommandsRadio.GetFrequencyIncrementAsync(this.MyFunctions.progress, this.MyFunctions.cancelToken);
        additionalFrequencyIncrement = actualFrequencyIncrement - (int) error_Hz;
        await newCommands.myCommandsRadio.SetFrequencyIncrementAsync(additionalFrequencyIncrement, this.MyFunctions.progress, this.MyFunctions.cancelToken);
      }));
      short NewValue1 = (short) (actualFrequencyIncrement + additionalFrequencyIncrement);
      s3Parameter.SetShortValue(NewValue1);
      return true;
    }

    internal bool SetReducedRadioPowerBy_dBm(int ppm)
    {
      if (ppm > 0 || ppm < -15)
        throw new Exception("Illegal radio power reduction value: " + ppm.ToString() + "; Allowed: 0 .. -10");
      this.MyParameters.ParameterByName[S3_ParameterNames.radioPower.ToString()].SetByteValue((byte) (18 + ppm));
      return true;
    }

    internal bool CheckCycleSettings()
    {
      if (!this.MyResources.IsResourceAvailable(S3_MeterResources.Radio.ToString()))
        return true;
      int num1 = 4;
      for (int index = 0; index < 3; ++index)
      {
        if (this.MyParameters.ParameterByName[S3_Meter.inputFactor[index]].GetUshortValue() > (ushort) 0)
          ++num1;
      }
      if ((int) this.MyParameters.ParameterByName[S3_ParameterNames.radioCycleTimeCounterInit.ToString()].GetByteValue() >= num1)
        return true;
      int num2 = (int) this.MyParameters.ParameterByName[S3_ParameterNames.volumeCycleTimeCounterInit.ToString()].GetByteValue() / 2 * num1;
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, string.Format("This radioCycleTime is not possible. In this case the minimum radioCycleFactor is {0}. This means a minimum radioCycleTime of {1} seconds", (object) num1, (object) num2));
      return false;
    }

    internal bool CheckMemoryUsing()
    {
      int index = this.theMap.FindIndex((Predicate<KeyValuePair<string, int>>) (x => x.Key == "MeterKey"));
      S3_ParameterLoader.GetS3Parameter(this, "MeterKey");
      if (index >= 0)
      {
        int num = this.theMap[index].Value - this.MyDeviceMemory.BlockLoggerData.StartAddressOfNextBlock;
        if (num < 0)
          throw new Exception("Out of flash memory. Bytes over: " + (num * -1).ToString());
      }
      return true;
    }

    internal int ReadCheckupState()
    {
      ByteField MemoryData;
      return !this.MyFunctions.MyCommands.ReadMemory(MemoryLocation.RAM, this.MyParameters.ParameterByName[S3_ParameterNames.checkupState.ToString()].BlockStartAddress, 1, out MemoryData) ? 0 : (int) MemoryData.Data[0];
    }

    internal uint? GetRadioCycleSeconds()
    {
      return this.MyResources.AvailableMeterResources.Keys.Contains("Radio") && this.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.radioCycleTimeCounterInit.ToString()) ? new uint?((uint) this.MyParameters.ParameterByName[S3_ParameterNames.volumeCycleTimeCounterInit.ToString()].GetByteValue() / 2U * (uint) this.MyParameters.ParameterByName[S3_ParameterNames.radioCycleTimeCounterInit.ToString()].GetUshortValue()) : new uint?();
    }

    internal void SetRadioCycleSeconds(uint radioCycleSeconds)
    {
      if (radioCycleSeconds == 0U || !this.MyResources.AvailableMeterResources.Keys.Contains("Radio"))
        return;
      SortedList<string, S3_Parameter> parameterByName1 = this.MyParameters.ParameterByName;
      S3_ParameterNames s3ParameterNames = S3_ParameterNames.radioCycleTimeCounterInit;
      string key1 = s3ParameterNames.ToString();
      if (parameterByName1.ContainsKey(key1))
      {
        SortedList<string, S3_Parameter> parameterByName2 = this.MyParameters.ParameterByName;
        s3ParameterNames = S3_ParameterNames.volumeCycleTimeCounterInit;
        string key2 = s3ParameterNames.ToString();
        int num = (int) parameterByName2[key2].GetByteValue() / 2;
        if ((ulong) radioCycleSeconds % (ulong) num > 0UL)
          throw new Exception("Illegal radioCycleSeconds. Factor to volumeCycleSeconds not int");
        ushort NewValue = (ushort) ((ulong) radioCycleSeconds / (ulong) num);
        SortedList<string, S3_Parameter> parameterByName3 = this.MyParameters.ParameterByName;
        s3ParameterNames = S3_ParameterNames.radioCycleTimeCounterInit;
        string key3 = s3ParameterNames.ToString();
        parameterByName3[key3].SetUshortValue(NewValue);
      }
    }

    internal string[] GetUsefulRadioCycleSeconds()
    {
      if (this.MyResources.AvailableMeterResources.Keys.Contains("Radio") && this.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.radioCycleTimeCounterInit.ToString()))
      {
        List<string> stringList = new List<string>();
        int num1 = (int) this.MyParameters.ParameterByName[S3_ParameterNames.volumeCycleTimeCounterInit.ToString()].GetByteValue() / 2;
        int num2 = 2;
        int num3 = 1;
        for (int index = num1 * num2; index <= 3600; index = num1 * num2)
        {
          stringList.Add(index.ToString());
          int num4 = num1 * (num2 + num3);
          int num5 = num4 - index;
          if (index < 60 && num4 >= 60)
          {
            if (60 % num1 == 0)
              num2 = 60 / num1;
            num3 = num2 / 6;
            if (num3 == 0)
              num3 = 1;
          }
          else if (index < 600 && num4 >= 600)
          {
            if (600 % num1 == 0)
              num2 = 600 / num1;
            num3 = num2 / 60;
            if (num3 == 0)
              num3 = 1;
          }
          else
            num2 += num3;
        }
        return stringList.ToArray();
      }
      return new string[1]{ "0" };
    }
  }
}
