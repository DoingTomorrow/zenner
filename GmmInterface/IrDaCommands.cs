// Decompiled with JetBrains decompiler
// Type: ZENNER.IrDaCommands
// Assembly: GmmInterface, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 25F1E48F-52B7-4A4F-B66A-62C91CCF5C52
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmInterface.dll

using CommunicationPort.Functions;
using CommunicationPort.UserInterface;
using Devices;
using HandlerLib;
using MBusLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Entities;
using ZENNER.CommonLibrary.Exceptions;
using ZR_ClassLibrary;

#nullable disable
namespace ZENNER
{
  public class IrDaCommands : DeviceCommandsMBus, ICommand
  {
    private readonly CommunicationPortFunctions port;

    public EquipmentModel EquipmentModel { get; set; }

    public ZENNER.CommonLibrary.Entities.Meter Meter { get; set; }

    public ProfileType ProfileType { get; set; }

    public CommonLoRaCommands LoRa { get; set; }

    public CommonMBusCommands MBusCmd { get; set; }

    public CommonRadioCommands Radio { get; set; }

    public Common32BitCommands Device { get; set; }

    public SpecialCommands Special { get; set; }

    public NfcDeviceCommands NFC { get; set; }

    public IrDaCommands(CommunicationPortFunctions port)
      : base((IPort) port)
    {
      this.port = port;
      this.Device = new Common32BitCommands((DeviceCommandsMBus) this);
      this.MBusCmd = new CommonMBusCommands(this.Device);
      this.Radio = new CommonRadioCommands(this.Device);
      this.LoRa = new CommonLoRaCommands(this.Device);
      this.Special = new SpecialCommands(this.Device);
      this.NFC = new NfcDeviceCommands(port);
    }

    public void Disconnect()
    {
      if (this.port == null)
        return;
      this.port.Close();
    }

    public async Task<DateTime?> GetKeyDateAsync(ProgressHandler progress, CancellationToken token)
    {
      Common32BitCommands.KeyDate keydate = await this.Device.GetKeyDateAsync(progress, token);
      try
      {
        return keydate.FirstYear != byte.MaxValue && keydate.FirstYear != (byte) 0 ? new DateTime?(new DateTime(2000 + (int) keydate.FirstYear, (int) keydate.Month, (int) keydate.DayOfMonth)) : new DateTime?(new DateTime(2000, (int) keydate.Month, (int) keydate.DayOfMonth));
      }
      catch
      {
        return new DateTime?();
      }
    }

    public async Task SetKeyDateAsync(
      DateTime date,
      ProgressHandler progress,
      CancellationToken token)
    {
      Common32BitCommands.KeyDate kDate = new Common32BitCommands.KeyDate()
      {
        FirstYear = (byte) (date.Year - 2000),
        Month = (byte) date.Month,
        DayOfMonth = (byte) date.Day
      };
      await this.Device.SetKeyDateAsync(kDate, progress, token);
      kDate = (Common32BitCommands.KeyDate) null;
    }

    public async Task<string> GetDevEUIAsync(ProgressHandler progress, CancellationToken token)
    {
      byte[] reversed = await this.LoRa.GetDevEUIAsync(progress, token);
      if (reversed == null)
        return (string) null;
      Array.Reverse((Array) reversed);
      return Utility.ByteArrayToHexString(reversed);
    }

    public async Task<DeviceMode> GetModeAsync(ProgressHandler progress, CancellationToken token)
    {
      byte result = await this.Device.GetModeAsync(progress, token);
      return (DeviceMode) result;
    }

    public async Task SetModeAsync(
      DeviceMode mode,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.Device.SetModeAsync((byte) mode, progress, token);
    }

    public async Task<TransmissionScenario> GetTransmissionScenarioAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      byte transmissionScenarioAsync = await this.LoRa.GetTransmissionScenarioAsync(progress, token);
      return (TransmissionScenario) transmissionScenarioAsync;
    }

    public async Task SetTransmissionScenarioAsync(
      TransmissionScenario scenario,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.LoRa.SetTransmissionScenarioAsync(new byte[1]
      {
        (byte) scenario
      }, progress, token);
    }

    public async Task<double> GetProductFactorAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      byte[] value = await this.Special.GetProductFactorAsync(progress, token);
      double productFactorAsync = (double) BitConverter.ToUInt16(value, 0) / 1000.0;
      value = (byte[]) null;
      return productFactorAsync;
    }

    public async Task SetProductFactorAsync(
      double factor,
      ProgressHandler progress,
      CancellationToken token)
    {
      byte[] value = BitConverter.GetBytes(Convert.ToUInt16(factor * 1000.0));
      await this.Special.SetProductFactorAsync(value, progress, token);
      value = (byte[]) null;
    }

    public async Task SetChannelValueAsync(
      byte channel,
      uint value,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.MBusCmd.SetChannelValueAsync(channel, value, progress, token);
    }

    public async Task<uint> GetChannelValueAsync(
      byte channel,
      ProgressHandler progress,
      CancellationToken token)
    {
      FirmwareType type = (FirmwareType) this.ConnectedDeviceVersion.FirmwareVersionObj.Type;
      switch (type)
      {
        case FirmwareType.SD_LoRa:
          MBusFrame response = this.Device.DeviceCMD.MBus.Repeater.GetResultFrame(new MBusFrame(new byte[5]
          {
            (byte) 15,
            (byte) 1,
            (byte) 0,
            (byte) 29,
            (byte) 48
          }), progress, token);
          return (uint) BitConverter.ToUInt16(response.UserData, 31);
        case FirmwareType.C5_LoRa:
          byte address = 254;
          await this.MBus.ApplicationResetAsync(progress, token, address);
          await this.MBus.SND_NKE_Async(progress, token, address);
          RSP_UD reqUd2 = await this.MBus.REQ_UD2Async(progress, token, address, true, false);
          List<MBusValue> values = reqUd2.GetValues();
          object obj = values.FirstOrDefault<MBusValue>((Func<MBusValue, bool>) (x => x.StorageNumber == 0L && x.Description == "Energy")).Value;
          if (obj == null)
            throw new Exception("C5 does not have 'Energy' value!");
          uint result = 0;
          if (uint.TryParse(obj.ToString(), out result))
            return result;
          throw new Exception("C5 delivers invalid value of 'Energy'. Value: " + obj.ToString());
        default:
          uint channelValueAsync = await this.MBusCmd.GetChannelValueAsync(channel, progress, token);
          return channelValueAsync;
      }
    }

    public async Task SendJoinRequestAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.LoRa.SendJoinRequestAsync(progress, token);
    }

    public async Task<ActivationMode> GetActivationModeAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      OTAA_ABP result = await this.LoRa.GetOTAA_ABPAsync(progress, token);
      return (ActivationMode) result;
    }

    public async Task SetActivationModeOTAAAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.LoRa.SetOTAA_ABPAsync(OTAA_ABP.OTAA, progress, token);
    }

    public async Task<DateTime?> GetSystemTimeAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      Common32BitCommands.SystemTime systemTime = await this.Device.GetSystemTimeAsync(progress, token);
      try
      {
        return new DateTime?(new DateTime(2000 + (int) systemTime.Year, (int) systemTime.Month, (int) systemTime.Day, (int) systemTime.Hour, (int) systemTime.Min, (int) systemTime.Sec));
      }
      catch
      {
        return new DateTime?();
      }
    }

    public async Task<long> GetRadio3_IDAsync(ProgressHandler progress, CancellationToken token)
    {
      MBusChannelIdentification ident = await this.MBusCmd.GetChannelIdentificationAsync((byte) 85, progress, token);
      long serialNumber = ident.SerialNumber;
      ident = (MBusChannelIdentification) null;
      return serialNumber;
    }

    public async Task BackupDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.Device.BackupDeviceAsync(progress, token);
    }

    public async Task ResetDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.Device.ResetDeviceAsync(progress, token);
    }

    public async Task<IEnumerable> ReadEventsAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      if (this.ProfileType.ProfileTypeID != 73)
        return (IEnumerable) null;
      ConfigList configList = this.port.GetReadoutConfiguration();
      DeviceManager deviceManager = GmmInterface.Devices;
      deviceManager.MyCommunicationPort = new CommunicationPortWindowFunctions();
      deviceManager.MyCommunicationPort.portFunctions = this.port;
      deviceManager.PrepareCommunicationStructure(configList);
      CommonHandlerWrapper wrapper = (CommonHandlerWrapper) deviceManager.SelectedHandler;
      if (wrapper == null)
        return (IEnumerable) null;
      HandlerFunctionsForProduction iuw = wrapper.HandlerInterface;
      int channel = await iuw.ReadDeviceAsync(progress, token, ReadPartsSelection.SmartFunctionLoggers);
      return iuw.GetEvents();
    }

    public async Task<List<ZENNER.Device>> ReadValuesAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      if (progress == null)
        progress = new ProgressHandler((Action<ProgressArg>) (p => { }));
      if (this.ProfileType.ProfileTypeID == 73)
      {
        List<ZENNER.Device> deviceList = await this.ReadValuesIUWAsync(progress, token);
        return deviceList;
      }
      FirmwareType type = (FirmwareType) this.ConnectedDeviceVersion.FirmwareVersionObj.Type;
      FirmwareType firmwareType = type;
      switch (firmwareType)
      {
        case FirmwareType.EDC_LoRa:
        case FirmwareType.micro_LoRa:
        case FirmwareType.micro_wMBus:
        case FirmwareType.EDC_wMBus_ST:
        case FirmwareType.EDC_LoRa_470Mhz:
        case FirmwareType.micro_LoRa_LL:
        case FirmwareType.EDC_LoRa_915MHz:
        case FirmwareType.micro_wMBus_LL:
        case FirmwareType.micro_LL_radio3:
        case FirmwareType.EDC_LoRa_868_v3:
        case FirmwareType.EDC_LoRa_915_v2_US:
        case FirmwareType.EDC_LoRa_915_v2_BR:
          List<ZENNER.Device> deviceList1 = await this.ReadValuesEDC_LoRaAsync(progress, token);
          return deviceList1;
        case FirmwareType.PDC_LoRa:
        case FirmwareType.PDC_LoRa_915:
        case FirmwareType.UDC_LoRa_915:
        case FirmwareType.PDC_LoRa_868MHz_SD:
        case FirmwareType.PDC_GAS:
          List<ZENNER.Device> deviceList2 = await this.ReadValuesPDC_LoRaAsync(progress, token);
          return deviceList2;
        case FirmwareType.HCA_LoRa:
        case FirmwareType.M7plus:
          List<ZENNER.Device> deviceList3 = await this.ReadValuesHCA_LoRaAsync(progress, token);
          return deviceList3;
        case FirmwareType.SD_LoRa:
        case FirmwareType.C5_LoRa:
          List<ZENNER.Device> deviceList4 = await this.ReadValuesC5_LoRaAsync(progress, token);
          return deviceList4;
        case FirmwareType.TH_LoRa:
          List<ZENNER.Device> deviceList5 = await this.ReadValuesTH_LoRaAsync(progress, token);
          return deviceList5;
        default:
          throw new NotImplementedException(type.ToString());
      }
    }

    private async Task<List<ZENNER.Device>> ReadValuesIUWAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      ConfigList configList = this.port.GetReadoutConfiguration();
      DeviceManager deviceManager = GmmInterface.Devices;
      deviceManager.MyCommunicationPort = new CommunicationPortWindowFunctions();
      deviceManager.MyCommunicationPort.portFunctions = this.port;
      deviceManager.PrepareCommunicationStructure(configList);
      CommonHandlerWrapper wrapper = (CommonHandlerWrapper) deviceManager.SelectedHandler;
      if (wrapper == null)
        return (List<ZENNER.Device>) null;
      HandlerFunctionsForProduction iuw = wrapper.HandlerInterface;
      int channel = await iuw.ReadDeviceAsync(progress, token, ReadPartsSelection.MonthLogger);
      DeviceIdentification version = await this.NFC.ReadVersionAsync(progress, token);
      return new List<ZENNER.Device>()
      {
        new ZENNER.Device()
        {
          ID = version.ID_BCD_AsString,
          IdType = IdType.Serialnumber,
          Channel = channel,
          ValueSets = iuw.GetValues(channel)
        }
      };
    }

    private async Task<List<ZENNER.Device>> ReadValuesTH_LoRaAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      DateTime? nullable = await this.GetSystemTimeAsync(progress, token);
      DateTime? systemTime = nullable;
      nullable = new DateTime?();
      if (!systemTime.HasValue)
        throw new Exception("Invalid system time of device!");
      TransmissionScenario scenario = await this.GetTransmissionScenarioAsync(progress, token);
      bool isLoRa = scenario == TransmissionScenario.Scenario1_Monthly || scenario == TransmissionScenario.Scenario2_Daily || scenario == TransmissionScenario.Scenario3_Hourly;
      string ID = string.Empty;
      if (isLoRa)
        ID = await this.GetDevEUIAsync(progress, token);
      else
        ID = this.ConnectedDeviceVersion.ID_BCD_AsString;
      List<ZENNER.Device> devices = new List<ZENNER.Device>();
      ZENNER.Device device = new ZENNER.Device();
      device.ID = ID;
      device.IdType = isLoRa ? IdType.DevEUI : IdType.Serialnumber;
      device.ValueSets = new SortedList<long, SortedList<DateTime, double>>();
      uint SHT2x_T = await this.GetChannelValueAsync((byte) 1, progress, token);
      SortedList<DateTime, double> valuesT = new SortedList<DateTime, double>();
      valuesT.Add(systemTime.Value, (double) SHT2x_T / 10.0);
      device.ValueSets.Add(272770182L, valuesT);
      uint SHT2x_RH = await this.GetChannelValueAsync((byte) 2, progress, token);
      SortedList<DateTime, double> valuesRH = new SortedList<DateTime, double>();
      valuesRH.Add(systemTime.Value, (double) SHT2x_RH / 10.0);
      device.ValueSets.Add(272770260L, valuesRH);
      uint temperatureDistribution = await this.GetChannelValueAsync((byte) 3, progress, token);
      SortedList<DateTime, double> valuesTD = new SortedList<DateTime, double>();
      valuesTD.Add(systemTime.Value, (double) temperatureDistribution);
      device.ValueSets.Add(1107907718L, valuesTD);
      uint humidityDistribution = await this.GetChannelValueAsync((byte) 4, progress, token);
      SortedList<DateTime, double> valuesRHD = new SortedList<DateTime, double>();
      valuesRHD.Add(systemTime.Value, (double) temperatureDistribution);
      device.ValueSets.Add(1107907796L, valuesRHD);
      CancellationToken cancellationToken1 = await this.AddTHLoggerValues((byte) 1, progress, token, device, LogSelect.Month, ValueIdent.ValueId_Predefined.T_Month);
      CancellationToken cancellationToken2 = await this.AddTHLoggerValues((byte) 1, progress, token, device, LogSelect.HalfMonth, ValueIdent.ValueId_Predefined.T_HalfMonth);
      CancellationToken cancellationToken3 = await this.AddTHLoggerValues((byte) 1, progress, token, device, LogSelect.Daily, ValueIdent.ValueId_Predefined.T_Day);
      CancellationToken cancellationToken4 = await this.AddTHLoggerValues((byte) 2, progress, token, device, LogSelect.Month, ValueIdent.ValueId_Predefined.RH_Month);
      CancellationToken cancellationToken5 = await this.AddTHLoggerValues((byte) 2, progress, token, device, LogSelect.HalfMonth, ValueIdent.ValueId_Predefined.RH_HalfMonth);
      CancellationToken cancellationToken6 = await this.AddTHLoggerValues((byte) 2, progress, token, device, LogSelect.Daily, ValueIdent.ValueId_Predefined.RH_Day);
      devices.Add(device);
      List<ZENNER.Device> deviceList = devices;
      ID = (string) null;
      devices = (List<ZENNER.Device>) null;
      device = (ZENNER.Device) null;
      valuesT = (SortedList<DateTime, double>) null;
      valuesRH = (SortedList<DateTime, double>) null;
      valuesTD = (SortedList<DateTime, double>) null;
      valuesRHD = (SortedList<DateTime, double>) null;
      return deviceList;
    }

    private async Task<List<ZENNER.Device>> ReadValuesHCA_LoRaAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      DateTime? nullable = await this.GetSystemTimeAsync(progress, token);
      DateTime? systemTime = nullable;
      nullable = new DateTime?();
      if (!systemTime.HasValue)
        throw new Exception("Invalid system time of device!");
      TransmissionScenario scenario = await this.GetTransmissionScenarioAsync(progress, token);
      bool isLoRa = scenario == TransmissionScenario.Scenario1_Monthly || scenario == TransmissionScenario.Scenario2_Daily || scenario == TransmissionScenario.Scenario3_Hourly;
      string ID = string.Empty;
      FirmwareType type = (FirmwareType) this.ConnectedDeviceVersion.FirmwareVersionObj.Type;
      if (type == FirmwareType.M7plus)
      {
        long id = await this.GetRadio3_IDAsync(progress, token);
        ID = id.ToString();
      }
      else if (isLoRa)
        ID = await this.GetDevEUIAsync(progress, token);
      else
        ID = this.ConnectedDeviceVersion.ID_BCD_AsString;
      List<ZENNER.Device> devices = new List<ZENNER.Device>();
      ZENNER.Device device = new ZENNER.Device();
      device.ID = ID;
      device.IdType = isLoRa ? IdType.DevEUI : IdType.Serialnumber;
      device.ValueSets = new SortedList<long, SortedList<DateTime, double>>();
      uint currentValue = await this.GetChannelValueAsync((byte) 0, progress, token);
      SortedList<DateTime, double> values = new SortedList<DateTime, double>();
      values.Add(systemTime.Value, (double) currentValue);
      device.ValueSets.Add(272700040L, values);
      double K_factor = await this.GetProductFactorAsync(progress, token);
      CancellationToken cancellationToken1 = await this.AddM8LoggerValues(progress, token, device, K_factor, LogSelect.DueDate, ValueIdent.ValueId_Predefined.HCA_DueDate);
      CancellationToken cancellationToken2 = await this.AddM8LoggerValues(progress, token, device, K_factor, LogSelect.Month, ValueIdent.ValueId_Predefined.HCA_Month);
      CancellationToken cancellationToken3 = await this.AddM8LoggerValues(progress, token, device, K_factor, LogSelect.HalfMonth, ValueIdent.ValueId_Predefined.HCA_HalfMonth);
      CancellationToken cancellationToken4 = await this.AddM8LoggerValues(progress, token, device, K_factor, LogSelect.Daily, ValueIdent.ValueId_Predefined.HCA_Day);
      try
      {
        byte[] objState = await this.Special.GetFlowCheckStateAsync(progress, token);
        M8_DeviceStatus deviceStatus = (M8_DeviceStatus) BitConverter.ToUInt16(objState, 0);
        if (deviceStatus.HasFlag((Enum) M8_DeviceStatus.Hardware) || deviceStatus.HasFlag((Enum) M8_DeviceStatus.HcaError) || deviceStatus.HasFlag((Enum) M8_DeviceStatus.RadioError) || deviceStatus.HasFlag((Enum) M8_DeviceStatus.VersionLut) || deviceStatus.HasFlag((Enum) M8_DeviceStatus.Transceiver))
        {
          SortedList<DateTime, double> valuesError = new SortedList<DateTime, double>();
          valuesError.Add(systemTime.Value, 1.0);
          long valueIdent = ValueIdent.GetValueIdentOfError(ValueIdent.ValueIdPart_MeterType.HeatCostAllocator, ValueIdent.ValueIdentError.DeviceError);
          device.ValueSets.Add(valueIdent, valuesError);
          valuesError = (SortedList<DateTime, double>) null;
        }
        if (deviceStatus.HasFlag((Enum) M8_DeviceStatus.Tamper))
        {
          SortedList<DateTime, double> valuesError = new SortedList<DateTime, double>();
          valuesError.Add(systemTime.Value, 1.0);
          long valueIdent = ValueIdent.GetValueIdentOfError(ValueIdent.ValueIdPart_MeterType.HeatCostAllocator, ValueIdent.ValueIdentError.Manipulation);
          device.ValueSets.Add(valueIdent, valuesError);
          valuesError = (SortedList<DateTime, double>) null;
        }
        objState = (byte[]) null;
      }
      catch (NACK_Exception ex)
      {
      }
      devices.Add(device);
      List<ZENNER.Device> deviceList = devices;
      ID = (string) null;
      devices = (List<ZENNER.Device>) null;
      device = (ZENNER.Device) null;
      values = (SortedList<DateTime, double>) null;
      return deviceList;
    }

    private async Task<List<ZENNER.Device>> ReadValuesEDC_LoRaAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      DateTime? nullable = await this.GetSystemTimeAsync(progress, token);
      DateTime? systemTime = nullable;
      nullable = new DateTime?();
      if (!systemTime.HasValue)
        throw new Exception("Invalid system time of device!");
      TransmissionScenario scenario = await this.GetTransmissionScenarioAsync(progress, token);
      bool isLoRa = scenario == TransmissionScenario.Scenario1_Monthly || scenario == TransmissionScenario.Scenario2_Daily || scenario == TransmissionScenario.Scenario3_Hourly;
      string ID = string.Empty;
      FirmwareType type = (FirmwareType) this.ConnectedDeviceVersion.FirmwareVersionObj.Type;
      if (type == FirmwareType.micro_LL_radio3)
      {
        long id = await this.GetRadio3_IDAsync(progress, token);
        ID = id.ToString();
      }
      else if (isLoRa)
        ID = await this.GetDevEUIAsync(progress, token);
      else
        ID = this.ConnectedDeviceVersion.ID_BCD_AsString;
      List<ZENNER.Device> devices = new List<ZENNER.Device>();
      ZENNER.Device device = new ZENNER.Device();
      device.ID = ID;
      device.IdType = isLoRa ? IdType.DevEUI : IdType.Serialnumber;
      device.ValueSets = new SortedList<long, SortedList<DateTime, double>>();
      uint currentValue = await this.GetChannelValueAsync((byte) 0, progress, token);
      SortedList<DateTime, double> values = new SortedList<DateTime, double>();
      values.Add(systemTime.Value, (double) currentValue);
      device.ValueSets.Add(272699457L, values);
      CancellationToken cancellationToken1 = await this.AddLoggerValues((byte) 0, progress, token, device, LogSelect.DueDate, ValueIdent.ValueId_Predefined.Water_DueDate);
      CancellationToken cancellationToken2 = await this.AddLoggerValues((byte) 0, progress, token, device, LogSelect.Month, ValueIdent.ValueId_Predefined.Water_Month);
      CancellationToken cancellationToken3 = await this.AddLoggerValues((byte) 0, progress, token, device, LogSelect.HalfMonth, ValueIdent.ValueId_Predefined.Water_HalfMonth);
      CancellationToken cancellationToken4 = await this.AddLoggerValues((byte) 0, progress, token, device, LogSelect.Daily, ValueIdent.ValueId_Predefined.Water_Day);
      try
      {
        byte[] objState = await this.Special.GetFlowCheckStateAsync(progress, token);
        EDC2_Warning deviceStatus = (EDC2_Warning) BitConverter.ToUInt16(objState, 0);
        if (deviceStatus.HasFlag((Enum) EDC2_Warning.ABNORMAL) || deviceStatus.HasFlag((Enum) EDC2_Warning.PERMANENT_ERROR) || deviceStatus.HasFlag((Enum) EDC2_Warning.TEMPORARY_ERROR))
        {
          SortedList<DateTime, double> valuesError = new SortedList<DateTime, double>();
          valuesError.Add(systemTime.Value, 1.0);
          long valueIdent = ValueIdent.GetValueIdentOfError(ValueIdent.ValueIdPart_MeterType.Water, ValueIdent.ValueIdentError.DeviceError);
          device.ValueSets.Add(valueIdent, valuesError);
          valuesError = (SortedList<DateTime, double>) null;
        }
        if (deviceStatus.HasFlag((Enum) EDC2_Warning.TAMPER_A))
        {
          SortedList<DateTime, double> valuesError = new SortedList<DateTime, double>();
          valuesError.Add(systemTime.Value, 1.0);
          long valueIdent = ValueIdent.GetValueIdentOfError(ValueIdent.ValueIdPart_MeterType.Water, ValueIdent.ValueIdentError.Manipulation);
          device.ValueSets.Add(valueIdent, valuesError);
          valuesError = (SortedList<DateTime, double>) null;
        }
        objState = (byte[]) null;
      }
      catch (NACK_Exception ex)
      {
      }
      devices.Add(device);
      List<ZENNER.Device> deviceList = devices;
      ID = (string) null;
      devices = (List<ZENNER.Device>) null;
      device = (ZENNER.Device) null;
      values = (SortedList<DateTime, double>) null;
      return deviceList;
    }

    private async Task<List<ZENNER.Device>> ReadValuesPDC_LoRaAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      DateTime? nullable = await this.GetSystemTimeAsync(progress, token);
      DateTime? systemTime = nullable;
      nullable = new DateTime?();
      if (!systemTime.HasValue)
        throw new Exception("Invalid system time of device!");
      TransmissionScenario scenario = await this.GetTransmissionScenarioAsync(progress, token);
      bool isLoRa = scenario == TransmissionScenario.Scenario1_Monthly || scenario == TransmissionScenario.Scenario2_Daily || scenario == TransmissionScenario.Scenario3_Hourly;
      string ID = string.Empty;
      if (isLoRa)
        ID = await this.GetDevEUIAsync(progress, token);
      else
        ID = this.ConnectedDeviceVersion.ID_BCD_AsString;
      List<ZENNER.Device> devices = new List<ZENNER.Device>();
      ZENNER.Device deviceMain = new ZENNER.Device();
      deviceMain.ID = ID;
      deviceMain.IdType = isLoRa ? IdType.DevEUI : IdType.Serialnumber;
      devices.Add(deviceMain);
      ZENNER.Device device_1 = await this.ReadValuesPDC_LoRaAsync((byte) 1, systemTime.Value, progress, token);
      if (device_1 != null)
        devices.Add(device_1);
      ZENNER.Device device_2 = await this.ReadValuesPDC_LoRaAsync((byte) 2, systemTime.Value, progress, token);
      if (device_2 != null)
        devices.Add(device_2);
      List<ZENNER.Device> deviceList = devices;
      ID = (string) null;
      devices = (List<ZENNER.Device>) null;
      deviceMain = (ZENNER.Device) null;
      device_1 = (ZENNER.Device) null;
      device_2 = (ZENNER.Device) null;
      return deviceList;
    }

    private async Task<ZENNER.Device> ReadValuesPDC_LoRaAsync(
      byte channel,
      DateTime systemTime,
      ProgressHandler progress,
      CancellationToken token)
    {
      MBusChannelIdentification id = await this.MBusCmd.GetChannelIdentificationAsync(channel, progress, token);
      ZENNER.Device device = new ZENNER.Device();
      device.ID = id.SerialNumber.ToString();
      device.IdType = IdType.Serialnumber;
      device.Channel = (int) channel;
      device.ValueSets = new SortedList<long, SortedList<DateTime, double>>();
      uint currentValue = await this.GetChannelValueAsync(channel, progress, token);
      SortedList<DateTime, double> values = new SortedList<DateTime, double>();
      values.Add(systemTime, (double) currentValue);
      device.ValueSets.Add(272700106L, values);
      CancellationToken cancellationToken1 = await this.AddLoggerValues(channel, progress, token, device, LogSelect.DueDate, ValueIdent.ValueId_Predefined.Pulse_DueDate);
      CancellationToken cancellationToken2 = await this.AddLoggerValues(channel, progress, token, device, LogSelect.Month, ValueIdent.ValueId_Predefined.Pulse_Month);
      CancellationToken cancellationToken3 = await this.AddLoggerValues(channel, progress, token, device, LogSelect.HalfMonth, ValueIdent.ValueId_Predefined.Pulse_HalfMonth);
      CancellationToken cancellationToken4 = await this.AddLoggerValues(channel, progress, token, device, LogSelect.Daily, ValueIdent.ValueId_Predefined.Pulse_Day);
      ZENNER.Device device1 = device;
      id = (MBusChannelIdentification) null;
      device = (ZENNER.Device) null;
      values = (SortedList<DateTime, double>) null;
      return device1;
    }

    private async Task<CancellationToken> AddM8LoggerValues(
      ProgressHandler progress,
      CancellationToken token,
      ZENNER.Device device,
      double K_factor,
      LogSelect logSelect,
      ValueIdent.ValueId_Predefined valueIdent)
    {
      MBusChannelLog obj = await this.MBusCmd.ReadChannelLogValueAsync((byte) 0, (byte) logSelect, (byte) 0, byte.MaxValue, progress, token);
      if (obj != null)
      {
        IReadOnlyDictionary<DateTime, int> objValues = obj.GetValues();
        if (objValues != null && objValues.Count > 0)
        {
          SortedList<DateTime, double> values = new SortedList<DateTime, double>();
          foreach (KeyValuePair<DateTime, int> keyValuePair in (IEnumerable<KeyValuePair<DateTime, int>>) objValues)
          {
            KeyValuePair<DateTime, int> item = keyValuePair;
            values.Add(item.Key, (double) item.Value * K_factor / 4.0);
            item = new KeyValuePair<DateTime, int>();
          }
          device.ValueSets.Add((long) valueIdent, values);
          values = (SortedList<DateTime, double>) null;
        }
        objValues = (IReadOnlyDictionary<DateTime, int>) null;
      }
      CancellationToken cancellationToken = token;
      obj = (MBusChannelLog) null;
      return cancellationToken;
    }

    private async Task<CancellationToken> AddTHLoggerValues(
      byte channel,
      ProgressHandler progress,
      CancellationToken token,
      ZENNER.Device device,
      LogSelect logSelect,
      ValueIdent.ValueId_Predefined valueIdent)
    {
      MBusChannelLog obj = await this.MBusCmd.ReadChannelLogValueAsync(channel, (byte) logSelect, (byte) 0, byte.MaxValue, progress, token);
      if (obj != null)
      {
        IReadOnlyDictionary<DateTime, int> objValues = obj.GetValues();
        if (objValues != null && objValues.Count > 0)
        {
          SortedList<DateTime, double> values = new SortedList<DateTime, double>();
          foreach (KeyValuePair<DateTime, int> keyValuePair in (IEnumerable<KeyValuePair<DateTime, int>>) objValues)
          {
            KeyValuePair<DateTime, int> item = keyValuePair;
            values.Add(item.Key, (double) item.Value / 10.0);
            item = new KeyValuePair<DateTime, int>();
          }
          device.ValueSets.Add((long) valueIdent, values);
          values = (SortedList<DateTime, double>) null;
        }
        objValues = (IReadOnlyDictionary<DateTime, int>) null;
      }
      CancellationToken cancellationToken = token;
      obj = (MBusChannelLog) null;
      return cancellationToken;
    }

    private async Task<CancellationToken> AddLoggerValues(
      byte channel,
      ProgressHandler progress,
      CancellationToken token,
      ZENNER.Device device,
      LogSelect logSelect,
      ValueIdent.ValueId_Predefined valueIdent)
    {
      MBusChannelLog obj = await this.MBusCmd.ReadChannelLogValueAsync(channel, (byte) logSelect, (byte) 0, byte.MaxValue, progress, token);
      if (obj != null)
      {
        IReadOnlyDictionary<DateTime, int> objValues = obj.GetValues();
        if (objValues != null && objValues.Count > 0)
        {
          SortedList<DateTime, double> values = new SortedList<DateTime, double>();
          foreach (KeyValuePair<DateTime, int> keyValuePair in (IEnumerable<KeyValuePair<DateTime, int>>) objValues)
          {
            KeyValuePair<DateTime, int> item = keyValuePair;
            values.Add(item.Key, (double) item.Value);
            item = new KeyValuePair<DateTime, int>();
          }
          device.ValueSets.Add((long) valueIdent, values);
          values = (SortedList<DateTime, double>) null;
        }
        objValues = (IReadOnlyDictionary<DateTime, int>) null;
      }
      CancellationToken cancellationToken = token;
      obj = (MBusChannelLog) null;
      return cancellationToken;
    }

    private async Task<List<ZENNER.Device>> ReadValuesC5_LoRaAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      this.port.Close();
      MeterReaderManager reader = GmmInterface.Reader;
      ValueIdentSet set = (ValueIdentSet) null;
      reader.ValueIdentSetReceived += (EventHandler<ValueIdentSet>) ((sender, e) => set = e);
      MeterReaderManager meterReaderManager = reader;
      List<ZENNER.CommonLibrary.Entities.Meter> meters = new List<ZENNER.CommonLibrary.Entities.Meter>();
      meters.Add(this.Meter);
      EquipmentModel equipmentModel = this.EquipmentModel;
      ProfileType profileType = this.ProfileType;
      await meterReaderManager.ReadMeterAsync((DeviceModel) null, (List<long>) null, meters, equipmentModel, profileType);
      GmmInterface.Devices.Close();
      this.port.Open();
      TransmissionScenario scenario = await this.GetTransmissionScenarioAsync(progress, token);
      bool isLoRa = scenario == TransmissionScenario.Scenario1_Monthly || scenario == TransmissionScenario.Scenario2_Daily || scenario == TransmissionScenario.Scenario3_Hourly;
      string ID = string.Empty;
      if (isLoRa)
        ID = await this.GetDevEUIAsync(progress, token);
      else
        ID = this.ConnectedDeviceVersion.ID_BCD_AsString;
      List<ZENNER.Device> devices = new List<ZENNER.Device>();
      ZENNER.Device device = new ZENNER.Device();
      device.ID = ID;
      device.IdType = isLoRa ? IdType.DevEUI : IdType.Serialnumber;
      device.ValueSets = new SortedList<long, SortedList<DateTime, double>>();
      if (set != null && set.AvailableValues != null)
      {
        foreach (KeyValuePair<long, SortedList<DateTime, ReadingValue>> availableValue in set.AvailableValues)
        {
          KeyValuePair<long, SortedList<DateTime, ReadingValue>> item = availableValue;
          SortedList<DateTime, double> list = new SortedList<DateTime, double>();
          foreach (KeyValuePair<DateTime, ReadingValue> keyValuePair in item.Value)
          {
            KeyValuePair<DateTime, ReadingValue> val = keyValuePair;
            list.Add(val.Key, val.Value.value);
            val = new KeyValuePair<DateTime, ReadingValue>();
          }
          device.ValueSets.Add(item.Key, list);
          list = (SortedList<DateTime, double>) null;
          item = new KeyValuePair<long, SortedList<DateTime, ReadingValue>>();
        }
      }
      devices.Add(device);
      List<ZENNER.Device> deviceList = devices;
      reader = (MeterReaderManager) null;
      ID = (string) null;
      devices = (List<ZENNER.Device>) null;
      device = (ZENNER.Device) null;
      return deviceList;
    }
  }
}
