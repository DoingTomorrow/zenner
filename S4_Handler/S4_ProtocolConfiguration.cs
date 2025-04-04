// Decompiled with JetBrains decompiler
// Type: S4_Handler.S4_ProtocolConfiguration
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using S4_Handler.Functions;
using StartupLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace S4_Handler
{
  internal class S4_ProtocolConfiguration
  {
    private S4_HandlerFunctions MyFunctions;
    private S4_DeviceCommandsNFC NFC;
    private SortedList<OverrideID, ConfigurationParameter> ReadConfigList;
    private SortedList<OverrideID, ConfigurationParameter> ChangedConfigList;
    private SortedList<OverrideID, ConfigurationParameter> WriteConfigList;
    private NfcDeviceCommands.BatteryEndDateData BatteryReadData;
    private List<SmartFunctionIdentResultAndCalls> SmartFunctionsInDevice;
    internal S4_DeviceIdentification ReadDeviceIdentification;

    internal bool IsReadDone => this.ReadConfigList != null && this.ReadConfigList.Count > 0;

    internal bool IsWritePrepared => this.WriteConfigList != null && this.WriteConfigList.Count > 0;

    internal S4_ProtocolConfiguration(S4_HandlerFunctions myFunctions)
    {
      this.MyFunctions = myFunctions;
      this.NFC = this.MyFunctions.myDeviceCommands;
      this.ReadConfigList = new SortedList<OverrideID, ConfigurationParameter>();
    }

    internal async Task ReadConfigurationParametersAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      ReadPartsSelection readPartsSelection)
    {
      this.ReadConfigList.Clear();
      this.BatteryReadData = (NfcDeviceCommands.BatteryEndDateData) null;
      this.SmartFunctionsInDevice = (List<SmartFunctionIdentResultAndCalls>) null;
      S4_AllMeters s4AllMeters = this.MyFunctions.myMeters;
      DeviceIdentification deviceIdentification = await this.MyFunctions.ReadVersionAsync(progress, cancelToken);
      s4AllMeters.ConnectedMeter = new S4_Meter((DeviceIdentification) new S4_DeviceIdentification(deviceIdentification));
      s4AllMeters = (S4_AllMeters) null;
      deviceIdentification = (DeviceIdentification) null;
      this.ReadDeviceIdentification = this.MyFunctions.myMeters.ConnectedMeter.deviceIdentification;
      uint? nullable = this.ReadDeviceIdentification.FirmwareVersion;
      bool firmwareGE_V1_7_1 = new FirmwareVersion(nullable.Value) >= (object) "1.7.1 IUW";
      this.AddConfigurationParameter(OverrideID.SerialNumberFull, (object) this.ReadDeviceIdentification.FullSerialNumber, true);
      this.AddConfigurationParameter(OverrideID.FirmwareVersion, (object) this.ReadDeviceIdentification.FirmwareVersionObj.VersionString, true);
      this.AddConfigurationParameter(OverrideID.Protected, (object) this.ReadDeviceIdentification.IsProtected, true);
      nullable = this.ReadDeviceIdentification.DeviceStatusFlags;
      this.AddConfigurationParameter(OverrideID.ErrorCode, (object) (ushort) nullable.Value, true, "x04");
      if (firmwareGE_V1_7_1)
      {
        string printedSerialNumber = await this.NFC.CMDs_Device.GetPrintedSerialNumberAsync(progress, cancelToken);
        this.AddConfigurationParameter(OverrideID.PrintedSerialNumber, (object) printedSerialNumber, true);
        printedSerialNumber = (string) null;
      }
      DateTimeOffset dateTimeOffset = await this.NFC.CommonNfcCommands.GetSystemDateTime(progress, cancelToken);
      DateTimeOffset systemTime = dateTimeOffset;
      dateTimeOffset = new DateTimeOffset();
      this.AddConfigurationParameter(OverrideID.DeviceClock, (object) systemTime.DateTime);
      int timeZone = (systemTime.DateTime - systemTime.UtcDateTime).Hours;
      this.AddConfigurationParameter(OverrideID.TimeZone, (object) (Decimal) timeZone);
      Common32BitCommands.KeyDate keyDate = await this.NFC.CMDs_Device.GetKeyDateAsync(progress, cancelToken);
      DateTime dueDate;
      try
      {
        dueDate = new DateTime(DateTime.Now.Year, (int) keyDate.Month, (int) keyDate.DayOfMonth);
      }
      catch
      {
        dueDate = new DateTime(DateTime.Now.Year, 1, 1);
      }
      if (dueDate < DateTime.Now)
        dueDate = dueDate.AddYears(1);
      ConfigurationParameter theConfigParam = this.AddConfigurationParameter(OverrideID.DueDate, (object) dueDate);
      theConfigParam.Format = "dd.MM.";
      S4_SystemState systemState = await this.NFC.CommonNfcCommands.GetDeviceStatesAsync(progress, cancelToken);
      this.AddConfigurationParameter(OverrideID.WarningInfo, (object) systemState.SysInfo, true);
      this.AddConfigurationParameter(OverrideID.DeviceMode, (object) systemState.DeviceMode.ToString(), true);
      this.AddConfigurationParameter(OverrideID.SetPcTime, (object) false);
      this.AddConfigurationParameter(OverrideID.SetTimeForTimeZoneFromPcTime, (object) false);
      Common32BitCommands.Scenarios scenario = await this.NFC.CMDs_Device.GetCommunicationScenarioAsync(progress, cancelToken);
      this.AddConfigurationParameter(OverrideID.CommunicationScenario, (object) (int) scenario.ScenarioOne.Value);
      if (firmwareGE_V1_7_1)
      {
        byte[] DevEUI_Bytes = await this.NFC.CMDs_LoRa.GetDevEUIAsync(progress, cancelToken);
        ulong DevEUI = BitConverter.ToUInt64(DevEUI_Bytes, 0);
        this.AddConfigurationParameter(OverrideID.DevEUI, (object) DevEUI);
        byte[] AppKey_Bytes = await this.NFC.CMDs_LoRa.GetAppKeyAsync(progress, cancelToken);
        this.AddConfigurationParameter(OverrideID.AppKey, (object) AppKey_Bytes);
        byte[] AppEUI_Bytes = await this.NFC.CMDs_LoRa.GetAppEUIAsync(progress, cancelToken);
        ulong AppEUI = BitConverter.ToUInt64(AppEUI_Bytes, 0);
        this.AddConfigurationParameter(OverrideID.JoinEUI, (object) AppEUI);
        byte adr = await this.NFC.CMDs_LoRa.GetAdrAsync(progress, cancelToken);
        this.AddConfigurationParameter(OverrideID.ADR, (object) (adr > (byte) 0));
        byte[] NetID_Bytes = await this.NFC.CMDs_LoRa.GetNetIDAsync(progress, cancelToken);
        uint NetID = BitConverter.ToUInt32(NetID_Bytes, 0);
        this.AddConfigurationParameter(OverrideID.NetID, (object) NetID, true);
        byte[] DevAddr_Bytes = await this.NFC.CMDs_LoRa.GetDevAddrAsync(progress, cancelToken);
        uint DevAddr = BitConverter.ToUInt32(NetID_Bytes, 0);
        this.AddConfigurationParameter(OverrideID.DevAddr, (object) DevAddr, true);
        byte[] NwkSKey_Bytes = await this.NFC.CMDs_LoRa.GetNwkSKeyAsync(progress, cancelToken);
        this.AddConfigurationParameter(OverrideID.NwkSKey, (object) NwkSKey_Bytes, true);
        byte[] AppSKey_Bytes = await this.NFC.CMDs_LoRa.GetAppSKeyAsync(progress, cancelToken);
        this.AddConfigurationParameter(OverrideID.AppSKey, (object) AppSKey_Bytes, true);
        this.AddConfigurationParameter(OverrideID.SendJoinRequest, (object) false);
        byte[] AESKey_Bytes = await this.NFC.CMDs_MBus.GetMBusKeyAsync(progress, cancelToken);
        this.AddConfigurationParameter(OverrideID.AESKey, (object) AES.AesKeyToString(AESKey_Bytes));
        NfcDeviceCommands.BatteryEndDateData batteryEndDateData = await this.NFC.CommonNfcCommands.GetBatteryEndDateAsync(progress, cancelToken);
        this.BatteryReadData = batteryEndDateData;
        batteryEndDateData = (NfcDeviceCommands.BatteryEndDateData) null;
        ConfigurationParameter batEnd = this.AddConfigurationParameter(OverrideID.EndOfBatteryDate, (object) this.BatteryReadData.EndDate);
        batEnd.Format = "d";
        if (this.BatteryReadData.BatteryPreWaringMonths.HasValue)
        {
          this.AddConfigurationParameter(OverrideID.BatteryPreWarningMonths, (object) (int) this.BatteryReadData.BatteryPreWaringMonths.Value);
          this.AddConfigurationParameter(OverrideID.BatteryDurabilityMonths, (object) (uint) this.BatteryReadData.BatteryDurabilityMonths.Value);
          if (this.BatteryReadData.BatteryCapacity_mAh.HasValue)
            this.AddConfigurationParameter(OverrideID.BatteryCapacity_mAh, (object) (double) this.BatteryReadData.BatteryCapacity_mAh.Value);
        }
        DevEUI_Bytes = (byte[]) null;
        AppKey_Bytes = (byte[]) null;
        AppEUI_Bytes = (byte[]) null;
        NetID_Bytes = (byte[]) null;
        DevAddr_Bytes = (byte[]) null;
        NwkSKey_Bytes = (byte[]) null;
        AppSKey_Bytes = (byte[]) null;
        AESKey_Bytes = (byte[]) null;
        batEnd = (ConfigurationParameter) null;
      }
      if (!firmwareGE_V1_7_1)
      {
        theConfigParam = (ConfigurationParameter) null;
        keyDate = (Common32BitCommands.KeyDate) null;
        systemState = (S4_SystemState) null;
        scenario = (Common32BitCommands.Scenarios) null;
      }
      else
      {
        List<SmartFunctionIdentResultAndCalls> identResultAndCallsList = await this.NFC.CommonNfcCommands.GetSmartFunctionsList(progress, cancelToken);
        this.SmartFunctionsInDevice = identResultAndCallsList;
        identResultAndCallsList = (List<SmartFunctionIdentResultAndCalls>) null;
        string[] strArray = S4_SmartFunctionManager.HardCodedFunctions;
        for (int index = 0; index < strArray.Length; ++index)
        {
          string hardCodedFunction = strArray[index];
          SmartFunctionIdentResultAndCalls hardCoded = this.SmartFunctionsInDevice.FirstOrDefault<SmartFunctionIdentResultAndCalls>((Func<SmartFunctionIdentResultAndCalls, bool>) (x => x.Name == hardCodedFunction));
          if (hardCoded != null)
            this.SmartFunctionsInDevice.Remove(hardCoded);
          hardCoded = (SmartFunctionIdentResultAndCalls) null;
        }
        strArray = (string[]) null;
        string[] functionList = S4_SmartFunctionManager.GetSmartFunctionNames(this.SmartFunctionsInDevice);
        string[] activeFunctionList = S4_SmartFunctionManager.GetActiveSmartFunctionNames(this.SmartFunctionsInDevice);
        if (functionList != null && functionList.Length != 0)
        {
          ConfigurationParameter asfc = this.AddConfigurationParameter(OverrideID.ActiveSmartFunctions, (object) activeFunctionList);
          if (asfc != null)
            asfc.AllowedValues = functionList;
          asfc = (ConfigurationParameter) null;
        }
        functionList = (string[]) null;
        activeFunctionList = (string[]) null;
        theConfigParam = (ConfigurationParameter) null;
        keyDate = (Common32BitCommands.KeyDate) null;
        systemState = (S4_SystemState) null;
        scenario = (Common32BitCommands.Scenarios) null;
      }
    }

    private ConfigurationParameter AddConfigurationParameter(
      OverrideID overrideID,
      object ParameterValue = null,
      bool readOnly = false,
      string format = null)
    {
      if ((ConfigurationParameter.ActiveConfigurationLevel & ConfigurationParameter.ConfigParametersByOverrideID[overrideID].DefaultConfigurationLevels) == (ConfigurationLevel) 0)
        return (ConfigurationParameter) null;
      ConfigurationParameter configurationParameter = new ConfigurationParameter(overrideID);
      if (ParameterValue != null)
        configurationParameter.ParameterValue = ParameterValue;
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
      if (readOnly)
        configurationParameter.HasWritePermission = false;
      if (format != null)
      {
        configurationParameter.Format = format;
        if (format.StartsWith("x"))
          configurationParameter.Unit = "HEX";
      }
      this.ReadConfigList.Add(overrideID, configurationParameter);
      return configurationParameter;
    }

    internal async Task WriteConfigurationParametersAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      DeviceIdentification di = await this.MyFunctions.ReadVersionAsync(progress, cancelToken);
      if (this.ReadDeviceIdentification == null)
        throw new Exception("Please read and save the identity before the device is changed.");
      if (this.ReadDeviceIdentification.FullSerialNumber != di.FullSerialNumber)
        throw new Exception("Device changed. Read before changing the configuration.");
      ConfigurationParameterListSupport confListSupport = new ConfigurationParameterListSupport(this.WriteConfigList);
      ConfigurationParameter configParam;
      if (this.BatteryReadData != null)
      {
        configParam = confListSupport.GetWorkParameterFromList(OverrideID.EndOfBatteryDate);
        ConfigurationParameter configParamDurability = confListSupport.GetWorkParameterFromList(OverrideID.BatteryDurabilityMonths);
        ConfigurationParameter configParamPreWaring = confListSupport.GetWorkParameterFromList(OverrideID.BatteryPreWarningMonths);
        ConfigurationParameter configParamBatteryCapacity = confListSupport.GetWorkParameterFromList(OverrideID.BatteryCapacity_mAh);
        if (configParam != null || configParamPreWaring != null || configParamDurability != null || configParamBatteryCapacity != null)
        {
          DateTime battEndDateTime = configParam == null ? (DateTime) this.ReadConfigList[OverrideID.EndOfBatteryDate].ParameterValue : (DateTime) configParam.ParameterValue;
          if (configParamPreWaring != null || configParamDurability != null)
          {
            byte batteryDurabilityMonth = configParamDurability == null ? this.BatteryReadData.BatteryDurabilityMonths.Value : (byte) (uint) configParamDurability.ParameterValue;
            sbyte batteryPreWaringMonth = configParamPreWaring == null ? this.BatteryReadData.BatteryPreWaringMonths.Value : (sbyte) (int) configParamPreWaring.ParameterValue;
            if (this.BatteryReadData.BatteryCapacity_mAh.HasValue)
            {
              ushort batteryCapacity = configParamPreWaring == null ? this.BatteryReadData.BatteryCapacity_mAh.Value : (ushort) (double) configParamPreWaring.ParameterValue;
            }
            await this.NFC.CommonNfcCommands.SetBatteryEndDateAsync(progress, cancelToken, battEndDateTime, new byte?(batteryDurabilityMonth), new sbyte?(batteryPreWaringMonth));
          }
          else
            await this.NFC.CommonNfcCommands.SetBatteryEndDateAsync(progress, cancelToken, battEndDateTime);
        }
        configParamDurability = (ConfigurationParameter) null;
        configParamPreWaring = (ConfigurationParameter) null;
        configParamBatteryCapacity = (ConfigurationParameter) null;
      }
      configParam = confListSupport.GetWorkParameterFromList(OverrideID.SetPcTime);
      ConfigurationParameter configParamSetTimeForTimeZoneFromPcTime = confListSupport.GetWorkParameterFromList(OverrideID.SetTimeForTimeZoneFromPcTime);
      ConfigurationParameter configParamTimeZone = confListSupport.GetWorkParameterFromList(OverrideID.TimeZone);
      ConfigurationParameter configParamDeviceClock = confListSupport.GetWorkParameterFromList(OverrideID.DeviceClock);
      bool setPcTime = configParam != null && (bool) configParam.ParameterValue;
      bool setTimeForTimeZoneFromPcTime = configParamSetTimeForTimeZoneFromPcTime != null && (bool) configParamSetTimeForTimeZoneFromPcTime.ParameterValue;
      DateTime dateTime;
      if (setPcTime | setTimeForTimeZoneFromPcTime || configParamTimeZone != null || configParamDeviceClock != null)
      {
        double timeZone = configParamTimeZone == null ? (double) (Decimal) this.ReadConfigList[OverrideID.TimeZone].ParameterValue : (double) (Decimal) configParamTimeZone.ParameterValue;
        DateTime deviceDateAndTime;
        if (setPcTime)
          deviceDateAndTime = DateTime.Now;
        else if (setTimeForTimeZoneFromPcTime)
        {
          dateTime = DateTime.Now;
          dateTime = dateTime.ToUniversalTime();
          DateTime dateTimeAtDeviceTimeZone = dateTime.AddHours(timeZone);
          deviceDateAndTime = new DateTime(dateTimeAtDeviceTimeZone.Year, dateTimeAtDeviceTimeZone.Month, dateTimeAtDeviceTimeZone.Day, dateTimeAtDeviceTimeZone.Hour, dateTimeAtDeviceTimeZone.Minute, dateTimeAtDeviceTimeZone.Second);
        }
        else if (configParamDeviceClock == null)
        {
          DateTimeOffset dateTimeOffset = await this.NFC.CommonNfcCommands.GetSystemDateTime(progress, cancelToken);
          DateTimeOffset currentDateTimeOffset = dateTimeOffset;
          dateTimeOffset = new DateTimeOffset();
          deviceDateAndTime = currentDateTimeOffset.DateTime;
          currentDateTimeOffset = new DateTimeOffset();
        }
        else
          deviceDateAndTime = (DateTime) configParamDeviceClock.ParameterValue;
        DateTimeOffset dateTimeOffset1 = new DateTimeOffset(deviceDateAndTime.Year, deviceDateAndTime.Month, deviceDateAndTime.Day, deviceDateAndTime.Hour, deviceDateAndTime.Minute, deviceDateAndTime.Second, new TimeSpan(0, (int) (timeZone * 60.0), 0));
        DateTimeOffset dateTimeOffset2 = await this.NFC.CommonNfcCommands.SetSystemDateTime(dateTimeOffset1, progress, cancelToken);
        dateTimeOffset1 = new DateTimeOffset();
      }
      configParam = confListSupport.GetWorkParameterFromList(OverrideID.DueDate);
      DateTime dueDate = (DateTime) this.ReadConfigList[OverrideID.DueDate].ParameterValue;
      if (configParam != null)
      {
        dueDate = (DateTime) configParam.ParameterValue;
        dateTime = DateTime.Now;
        dueDate = new DateTime(dateTime.Year, dueDate.Month, dueDate.Day);
        if (dueDate < DateTime.Now)
          dueDate = dueDate.AddYears(1);
        Common32BitCommands.KeyDate keyDate = new Common32BitCommands.KeyDate()
        {
          FirstYear = (byte) (dueDate.Year - 2000),
          Month = (byte) dueDate.Month,
          DayOfMonth = (byte) dueDate.Day
        };
        await this.NFC.CMDs_Device.SetKeyDateAsync(keyDate, progress, cancelToken);
        keyDate = (Common32BitCommands.KeyDate) null;
      }
      configParam = confListSupport.GetWorkParameterFromList(OverrideID.CommunicationScenario);
      ushort scenario = (ushort) (int) this.ReadConfigList[OverrideID.CommunicationScenario].ParameterValue;
      if (configParam != null)
      {
        scenario = (ushort) (int) configParam.ParameterValue;
        byte[] scenarioBytes = BitConverter.GetBytes(scenario);
        await this.NFC.CMDs_Device.SetCommunicationScenarioAsync(scenarioBytes, progress, cancelToken);
        scenarioBytes = (byte[]) null;
      }
      configParam = confListSupport.GetWorkParameterFromList(OverrideID.DevEUI);
      if (configParam != null)
      {
        byte[] DevEUI_Bytes = BitConverter.GetBytes((ulong) configParam.ParameterValue);
        await this.NFC.CMDs_LoRa.SetDevEUIAsync(DevEUI_Bytes, progress, cancelToken);
        DevEUI_Bytes = (byte[]) null;
      }
      configParam = confListSupport.GetWorkParameterFromList(OverrideID.AppKey);
      if (configParam != null)
      {
        byte[] AppKey_Bytes = (byte[]) configParam.ParameterValue;
        await this.NFC.CMDs_LoRa.SetAppKeyAsync(AppKey_Bytes, progress, cancelToken);
        AppKey_Bytes = (byte[]) null;
      }
      configParam = confListSupport.GetWorkParameterFromList(OverrideID.JoinEUI);
      if (configParam != null)
      {
        byte[] AppEUI_Bytes = BitConverter.GetBytes((ulong) configParam.ParameterValue);
        await this.NFC.CMDs_LoRa.SetAppEUIAsync(AppEUI_Bytes, progress, cancelToken);
        AppEUI_Bytes = (byte[]) null;
      }
      configParam = confListSupport.GetWorkParameterFromList(OverrideID.ADR);
      if (configParam != null)
      {
        if ((bool) configParam.ParameterValue)
          await this.NFC.CMDs_LoRa.SetAdrAsync((byte) 1, progress, cancelToken);
        else
          await this.NFC.CMDs_LoRa.SetAdrAsync((byte) 0, progress, cancelToken);
      }
      configParam = confListSupport.GetWorkParameterFromList(OverrideID.NetID);
      if (configParam != null)
      {
        byte[] NetID_Bytes = BitConverter.GetBytes((uint) configParam.ParameterValue);
        await this.NFC.CMDs_LoRa.SetNetIDAsync(NetID_Bytes, progress, cancelToken);
        NetID_Bytes = (byte[]) null;
      }
      configParam = confListSupport.GetWorkParameterFromList(OverrideID.DevAddr);
      if (configParam != null)
      {
        byte[] DevAddr_Bytes = BitConverter.GetBytes((uint) configParam.ParameterValue);
        await this.NFC.CMDs_LoRa.SetDevAddrAsync(DevAddr_Bytes, progress, cancelToken);
        DevAddr_Bytes = (byte[]) null;
      }
      configParam = confListSupport.GetWorkParameterFromList(OverrideID.NwkSKey);
      if (configParam != null)
      {
        byte[] NwkSKey_Bytes = (byte[]) configParam.ParameterValue;
        await this.NFC.CMDs_LoRa.SetNwkSKeyAsync(NwkSKey_Bytes, progress, cancelToken);
        NwkSKey_Bytes = (byte[]) null;
      }
      configParam = confListSupport.GetWorkParameterFromList(OverrideID.AppSKey);
      if (configParam != null)
      {
        byte[] AppSKey_Bytes = (byte[]) configParam.ParameterValue;
        await this.NFC.CMDs_LoRa.SetAppSKeyAsync(AppSKey_Bytes, progress, cancelToken);
        AppSKey_Bytes = (byte[]) null;
      }
      configParam = confListSupport.GetWorkParameterFromList(OverrideID.SendJoinRequest);
      if (configParam != null && (bool) configParam.ParameterValue)
      {
        await this.NFC.CMDs_LoRa.SetOTAA_ABPAsync(OTAA_ABP.OTAA, progress, cancelToken);
        await this.NFC.CMDs_LoRa.SendJoinRequestAsync(progress, cancelToken);
      }
      configParam = confListSupport.GetWorkParameterFromList(OverrideID.AESKey);
      if (configParam != null)
      {
        string AesKeyName = (string) configParam.ParameterValue;
        byte[] KeyBytes = AES.StringToAesKey(AesKeyName) ?? new byte[16];
        await this.NFC.CMDs_MBus.SetMBusKeyAsync(KeyBytes, progress, cancelToken);
        AesKeyName = (string) null;
        KeyBytes = (byte[]) null;
      }
      ConfigurationParameter configParamActiveSmartFunctions = confListSupport.GetWorkParameterFromList(OverrideID.ActiveSmartFunctions);
      if (configParamActiveSmartFunctions != null)
      {
        string[] functionList = S4_SmartFunctionManager.GetSmartFunctionNames(this.SmartFunctionsInDevice);
        string[] activeFunctionList = S4_SmartFunctionManager.GetActiveSmartFunctionNames(this.SmartFunctionsInDevice);
        string[] requiredActiveFunctions = (string[]) configParamActiveSmartFunctions.ParameterValue;
        string[] strArray = functionList;
        for (int index = 0; index < strArray.Length; ++index)
        {
          string theFunction = strArray[index];
          if (((IEnumerable<string>) activeFunctionList).Contains<string>(theFunction))
          {
            if (!((IEnumerable<string>) requiredActiveFunctions).Contains<string>(theFunction))
            {
              int num1 = (int) await this.NFC.CommonNfcCommands.SetSmartFunctionActivationAsync(progress, cancelToken, theFunction, false);
            }
          }
          else if (((IEnumerable<string>) requiredActiveFunctions).Contains<string>(theFunction))
          {
            int num2 = (int) await this.NFC.CommonNfcCommands.SetSmartFunctionActivationAsync(progress, cancelToken, theFunction, true);
          }
          theFunction = (string) null;
        }
        strArray = (string[]) null;
        functionList = (string[]) null;
        activeFunctionList = (string[]) null;
        requiredActiveFunctions = (string[]) null;
      }
      confListSupport.CheckAllParametersWorked();
      di = (DeviceIdentification) null;
      confListSupport = (ConfigurationParameterListSupport) null;
      configParam = (ConfigurationParameter) null;
      configParamSetTimeForTimeZoneFromPcTime = (ConfigurationParameter) null;
      configParamTimeZone = (ConfigurationParameter) null;
      configParamDeviceClock = (ConfigurationParameter) null;
      configParamActiveSmartFunctions = (ConfigurationParameter) null;
    }

    internal void WorkWriteParameterChanges(
      SortedList<OverrideID, ConfigurationParameter> writeList)
    {
      this.WriteConfigList = writeList;
      if (this.ChangedConfigList == null)
        this.ChangedConfigList = new SortedList<OverrideID, ConfigurationParameter>();
      for (int index = this.WriteConfigList.Count - 1; index >= 0; --index)
      {
        ConfigurationParameter configurationParameter = this.WriteConfigList.Values[index];
        this.CheckConfigurationParameter(configurationParameter);
        if (this.ChangedConfigList.ContainsKey(configurationParameter.ParameterID))
          this.ChangedConfigList.Remove(configurationParameter.ParameterID);
        if (this.ReadConfigList[configurationParameter.ParameterID].IsParameterValueEqual(configurationParameter))
          this.WriteConfigList.RemoveAt(index);
        else
          this.ChangedConfigList.Add(configurationParameter.ParameterID, configurationParameter);
      }
      if (this.ChangedConfigList.ContainsKey(OverrideID.SetTimeForTimeZoneFromPcTime) && (bool) this.ChangedConfigList[OverrideID.SetTimeForTimeZoneFromPcTime].ParameterValue)
      {
        if (this.ChangedConfigList.ContainsKey(OverrideID.SetPcTime) && (bool) this.ChangedConfigList[OverrideID.SetPcTime].ParameterValue)
          this.ChangedConfigList.Remove(OverrideID.SetPcTime);
        if (this.ChangedConfigList.ContainsKey(OverrideID.DeviceClock))
          this.ChangedConfigList.Remove(OverrideID.DeviceClock);
      }
      if (!this.ChangedConfigList.ContainsKey(OverrideID.SetPcTime) || !(bool) this.ChangedConfigList[OverrideID.SetPcTime].ParameterValue || !this.ChangedConfigList.ContainsKey(OverrideID.DeviceClock))
        return;
      this.ChangedConfigList.Remove(OverrideID.DeviceClock);
    }

    private void CheckConfigurationParameter(ConfigurationParameter changedParam)
    {
      if (changedParam.ParameterID != OverrideID.ActiveSmartFunctions)
        return;
      string[] smartFunctionNames1 = S4_SmartFunctionManager.GetSmartFunctionNames(this.SmartFunctionsInDevice);
      string[] smartFunctionNames2 = S4_SmartFunctionManager.GetActiveSmartFunctionNames(this.SmartFunctionsInDevice);
      string[] parameterValue = (string[]) changedParam.ParameterValue;
      foreach (string str in smartFunctionNames1)
      {
        string theFunction = str;
        if (((IEnumerable<string>) smartFunctionNames2).Contains<string>(theFunction))
        {
          if (!((IEnumerable<string>) parameterValue).Contains<string>(theFunction) && ((IEnumerable<string>) S4_SmartFunctionManager.HardCodedFunctions).Contains<string>(theFunction))
            throw new Exception(theFunction + " is firmware managed and cannot switch off");
        }
        else if (((IEnumerable<string>) parameterValue).Contains<string>(theFunction))
        {
          SmartFunctionResult functionResult = this.SmartFunctionsInDevice.First<SmartFunctionIdentResultAndCalls>((Func<SmartFunctionIdentResultAndCalls, bool>) (x => x.Name == theFunction)).FunctionResult;
          if (functionResult != SmartFunctionResult.DeactivatedByCommand)
            throw new Exception(functionResult.ToString() + " activation not possible. Function state: " + functionResult.ToString());
        }
      }
    }

    internal SortedList<OverrideID, ConfigurationParameter> GetConfigList()
    {
      SortedList<OverrideID, ConfigurationParameter> configList = new SortedList<OverrideID, ConfigurationParameter>();
      foreach (KeyValuePair<OverrideID, ConfigurationParameter> readConfig in this.ReadConfigList)
      {
        if (this.ChangedConfigList != null && this.ChangedConfigList.ContainsKey(readConfig.Key))
          configList.Add(readConfig.Key, this.ChangedConfigList[readConfig.Key].Clone());
        else
          configList.Add(readConfig.Key, readConfig.Value.Clone());
      }
      if (configList.ContainsKey(OverrideID.CommunicationScenario))
      {
        CommunicationScenarioRange communicationScenarioRange = ScenarioConfigurations.GetCommunicationScenarioRange((ushort) (int) configList[OverrideID.CommunicationScenario].ParameterValue);
        if (communicationScenarioRange != CommunicationScenarioRange.LoRa)
        {
          if (configList.ContainsKey(OverrideID.DevEUI))
            configList.Remove(OverrideID.DevEUI);
          if (configList.ContainsKey(OverrideID.AppKey))
            configList.Remove(OverrideID.AppKey);
          if (configList.ContainsKey(OverrideID.JoinEUI))
            configList.Remove(OverrideID.JoinEUI);
          if (configList.ContainsKey(OverrideID.NetID))
            configList.Remove(OverrideID.NetID);
          if (configList.ContainsKey(OverrideID.DevAddr))
            configList.Remove(OverrideID.DevAddr);
          if (configList.ContainsKey(OverrideID.NwkSKey))
            configList.Remove(OverrideID.NwkSKey);
          if (configList.ContainsKey(OverrideID.AppSKey))
            configList.Remove(OverrideID.AppSKey);
          if (configList.ContainsKey(OverrideID.ADR))
            configList.Remove(OverrideID.ADR);
        }
        else if (communicationScenarioRange != CommunicationScenarioRange.wMBus && configList.ContainsKey(OverrideID.AESKey))
          configList.Remove(OverrideID.AESKey);
      }
      return configList;
    }
  }
}
