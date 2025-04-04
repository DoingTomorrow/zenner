// Decompiled with JetBrains decompiler
// Type: EDC_Handler.IHandlerForEDC_Test
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using DeviceCollector;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace EDC_Handler
{
  public interface IHandlerForEDC_Test : IDisposable
  {
    bool EventLogClear();

    bool LogClearAndDisableLog();

    bool ReadDevice();

    bool WriteSystemTime(DateTime value);

    bool SaveDevice();

    bool WriteDevice();

    bool WriteMeterValue(uint value);

    uint? ReadMeterValue();

    bool SystemLogClear();

    bool StartVolumeMonitor();

    bool PulseEnable();

    bool RunRAMBackup();

    bool Overwrite(OverwritePart parts);

    bool OpenType(int meterInfoID);

    DateTime? ReadSystemTime();

    List<VolumeMonitorEventArgs> StartVolumeMonitor(int count);

    DeviceIdentification GetDeviceIdentification();

    Warning? GetWarnings();

    string GetSerialnumberFull();

    bool SetWarnings(Warning value);

    bool SetRemovalDetectionState(bool isDisabled);

    ushort? GetSensorTimeout();

    ushort? GetPulsePeriod();

    sbyte? GetCoilErrorThreshold();

    sbyte? GetCoilAmplitudeLimit();

    bool? GetFlowCheckIntervalState();

    bool? GetDataLoggingState();

    HardwareError? GetHardwareErrors();

    bool? GetRadioState();

    sbyte? GetCoilB_offset();

    DeviceVersion Version { get; set; }

    uint? GetSerialnumberPrimary();

    byte? GetCogCount();

    bool? GetRemovalDetectionState();

    bool? GetMagnetDetectionState();

    bool? GetCoilSampling();

    byte? GetPulseActivateRadio();

    ushort? GetDepassPeriod();

    bool SetFlowCheckIntervalState(bool enable);

    bool SetSensorTimeout(ushort seconds);

    bool SetRadioState(bool enable);

    bool SetDepassPeriod(ushort value);

    bool SetFrequencyOffset(short value);

    bool SetPulseActivateRadio(byte value);

    bool SetCoilAmplitudeLimit(sbyte value);

    short? GetFrequencyOffset();

    int? GetMeterValue();

    bool SetDataLoggingState(bool enable);

    bool SetCoilSampling(bool enable);

    bool SetWMBusEncryptionState(bool enable);

    bool SetBatteryEndDate(DateTime value);

    bool SetStartModule(DateTime value);

    bool SetCoilMinThreshold(sbyte value);

    bool SetCoilMaxThreshold(sbyte value);

    bool SetSerialnumberSecondary(uint value);

    bool SetObisSecondary(byte value);

    byte? GetMBusGenerationPrimary();

    bool SetMBusGenerationSecondary(byte value);

    byte? GetMBusGenerationSecondary();

    byte? GetObisPrimary();

    bool SetCoilB_offset(sbyte value);

    bool SetStartMeter(DateTime value);

    DatabaseDeviceInfo DBDeviceInfo { get; set; }

    bool SetDeviceIdentification(DeviceIdentification ident);

    bool SetAESkey(object value);

    bool SetMagnetDetectionState(bool enable);

    bool SetCogCount(byte value);

    bool? GetWMBusSynchronousTransmissioModeState();

    bool? GetWMBusInstallationPacketsState();

    ushort? GetDepassTimeout();

    MbusBaud? GetMbusBaud();

    PulseoutMode? GetPulseoutMode();

    string GetMBusListType();

    MBusDeviceType? GetMediumSecondary();

    string GetManufacturerPrimary();

    string GetManufacturerSecondary();

    bool SetObisPrimary(byte value);

    byte? GetObisSecondary();

    bool SetMBusGenerationPrimary(byte value);

    bool SetDepassTimeout(ushort value);

    bool SetRadioMode(RadioMode type);

    bool SetRadioPower(RadioPower value);

    bool SetRadioTransmitInterval(ushort interval);

    bool SetPulseMultiplier(byte value);

    RadioMode? GetRadioMode();

    RadioPower? GetRadioPower();

    bool SetPulseoutMode(PulseoutMode value);

    bool SetPulseoutWidth(ushort value);

    bool RadioReceive(
      out RadioPacket packet,
      out byte[] buffer,
      out int rssi_dBm,
      out int lqi,
      uint timeout);

    bool StartRadioReceiver();

    bool WritePulseoutQueue(short value, bool clearQueue);

    bool RadioOOK();

    bool RadioOOK(RadioMode mode, short offset, ushort timeoutInSeconds);

    bool RadioDisable();

    bool PulseDisable();

    bool StartDepassivation();

    bool SetSerialnumberPrimary(uint value);

    bool SetSerialnumberFull(string value);

    DeviceVersion ReadVersion();

    RuntimeFlags? GetRuntimeFlags();

    bool SendSND_NKE();

    string ShowHandlerWindow();

    bool SetHardwareErrors(HardwareError value);

    bool SetParameterValue<T>(string parameterName, T newValue);
  }
}
